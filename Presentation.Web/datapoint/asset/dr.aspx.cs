using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NCC.ClearView.Application.Core;
using NCC.ClearView.Presentation.Web.Custom;
using System.Diagnostics;
using System.DirectoryServices;
using Microsoft.ApplicationBlocks.Data;
using System.Management;

namespace NCC.ClearView.Presentation.Web
{
    public partial class dr : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intDataPointAvailableAsset = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_ASSET"]);
        protected DataPoint oDataPoint;
        protected Users oUser;
        protected Servers oServer;
        protected Asset oAsset;
        protected Functions oFunction;
        protected Domains oDomain;
        protected Classes oClass;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected string strMenuTab1 = "";
        protected int intServer = 0;
        protected int intAsset = 0;
        private int intID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            oDataPoint = new DataPoint(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oDomain = new Domains(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "ASSET") == true || intDataPointAvailableAsset == 1))
            {
                panAllow.Visible = true;
                if (Request.QueryString["save"] != null)
                    panSave.Visible = true;
                Int32.TryParse(oFunction.decryptQueryString(Request.QueryString["id"]), out intID);
                if (Request.QueryString["close"] != null)
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">window.close();<" + "/" + "script>");
                else if (Request.QueryString["q"] != null && Request.QueryString["q"] != "")
                {
                    string strQuery = oFunction.decryptQueryString(Request.QueryString["q"]);
                    DataSet ds = oDataPoint.GetAssetName(strQuery, intID, 0, "", "", 0);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        string strHeader = (strQuery.Length > 15 ? strQuery.Substring(0, 15) + "..." : strQuery);
                        lblHeader.Text = "&quot;" + strHeader.ToUpper() + "&quot;";
                        Master.Page.Title = "DataPoint | Server (DR) (" + strHeader + ")";
                        lblHeaderSub.Text = "Provides all the information about a DR server...";
                        int intMenuTab = 0;
                        if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                            intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                        Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);
                        oTab.AddTab("Platform Information", "");
                        strMenuTab1 = oTab.GetTabs();

                        if (ds.Tables[0].Rows[0]["id"].ToString() != "")
                        {
                            intAsset = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                            lblServerID.Text = intAsset.ToString();
                            if (ds.Tables[0].Rows[0]["serverid"].ToString() != "")
                            {
                                intServer = Int32.Parse(ds.Tables[0].Rows[0]["serverid"].ToString());
                                lblServerID.Text += " (" + intServer.ToString() + ")";
                            }
                        }

                        if (!IsPostBack)
                        {
                            if (intServer > 0)
                            {
                                string strName = strQuery;
                                // Load General Information
                                int intAssetClass = 0;
                                int intAssetEnv = 0;
                                DataSet dsAssets = oServer.GetAssets(intServer);
                                foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                                {
                                    if (Int32.Parse(drAsset["assetid"].ToString()) == intAsset)
                                    {
                                        intAssetClass = Int32.Parse(drAsset["classid"].ToString());
                                        intAssetEnv = Int32.Parse(drAsset["environmentid"].ToString());
                                        break;
                                    }
                                }
                                oDataPoint.LoadTextBox(txtName, intProfile, null, "", lblName, fldName, "SERVER_NAME", strName, "", true, false);
                                if (intAsset > 0)
                                {
                                    // Asset Information
                                    string strSerial = oAsset.Get(intAsset, "serial");
                                    string strURL = oDataPoint.GetAssetSerialOrTag(strSerial, "", "url");
                                    oDataPoint.LoadTextBox(txtPlatformSerial, intProfile, btnPlatformSerial, "/datapoint/asset/" + strURL + ".aspx?t=serial&q=" + oFunction.encryptQueryString(strSerial) + "&id=" + oFunction.encryptQueryString(intAsset.ToString()), lblPlatformSerial, fldPlatformSerial, "SERVER_SERIAL", strSerial, "", true, false);
                                    if (strURL == "")
                                    {
                                        // Vmware DR
                                        lblPlatformSerial.Text = txtPlatformSerial.Text;
                                    }
                                    string strAsset = oAsset.Get(intAsset, "asset");
                                    oDataPoint.LoadTextBox(txtPlatformAsset, intProfile, null, "", lblPlatformAsset, fldPlatformAsset, "SERVER_ASSET", strAsset, "", true, false);
                                }
                                int intDomain = 0;
                                int intDomainEnvironment = 0;
                                DataSet dsServer = oServer.Get(intServer);
                                if (dsServer.Tables[0].Rows.Count > 0)
                                {
                                    // DR Counterpart
                                    string strDR = oServer.GetName(intServer, true);
                                    oDataPoint.LoadTextBox(txtPlatformDRCounterPart, intProfile, btnPlatformDRCounterPart, "/datapoint/asset/datapoint_asset_search.aspx?t=name&q=" + oFunction.encryptQueryString(strDR), lblPlatformDRCounterPart, fldPlatformDRCounterPart, "SERVER_DR", strDR, "", true, false);
                                }
                                if (intDomain > 0)
                                    intDomainEnvironment = Int32.Parse(oDomain.Get(intDomain, "environment"));
                                hdnEnvironment.Value = intAssetEnv.ToString();
                                oDataPoint.LoadDropDown(ddlPlatformClass, intProfile, null, "", lblPlatformClass, fldPlatformClass, "SERVER_CLASS", "name", "id", oClass.Gets(1), intAssetClass, false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformEnvironment, intProfile, null, "", lblPlatformEnvironment, fldPlatformEnvironment, "SERVER_ENVIRONMENT", "name", "id", oClass.GetEnvironment(intAssetClass, 0), intAssetEnv, false, false, true);
                            }
                        }

                    }
                    else
                    {
                        Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx?t=name&q=" + oFunction.encryptQueryString(strQuery) + "&r=0");
                    }
                }
                btnClose.Attributes.Add("onclick", "window.close();return false;");
                btnPrint.Attributes.Add("onclick", "window.print();return false;");
                btnSave.Attributes.Add("onclick", oDataPoint.LoadValidation());
                btnSaveClose.Attributes.Add("onclick", oDataPoint.LoadValidation());
                ddlPlatformClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlPlatformClass.ClientID + "','" + ddlPlatformEnvironment.ClientID + "',0);");
                ddlPlatformEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlPlatformEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
            }
            else
                panDenied.Visible = true;
        }
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intSave = Save();
            Response.Redirect(Request.Path + "?q=" + Request.QueryString["q"] + "&" + (intSave > 0 ? "save" : "error") + "=" + intSave.ToString());
        }
        protected void btnSaveClose_Click(Object Sender, EventArgs e)
        {
            int intSave = Save();
            Response.Redirect(Request.Path + "?q=" + Request.QueryString["q"] + "&" + (intSave > 0 ? "close" : "error") + "=" + intSave.ToString());
        }
        protected int Save()
        {
            // Update Asset
            oServer.UpdateAsset(intAsset, Int32.Parse(ddlPlatformClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]));
            return 10;
        }

    }
}
