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
using System.DirectoryServices;
using NCC.ClearView.Presentation.Web.Custom;
using System.IO;

namespace NCC.ClearView.Presentation.Web
{
    public partial class build_fixed : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected int intID;
        protected int intUser;
        protected Workstations oWorkstation;
        protected Servers oServer;
        protected Functions oFunction;
        protected Users oUser;
        protected VMWare oVMWare;
        protected Asset oAsset;
        protected Errors oError;
        protected Zeus oZeus;
        protected Variables oVariable;
        protected OperatingSystems oOperatingSystem;
        protected string strMenuTab1 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            oWorkstation = new Workstations(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oUser = new Users(intProfile, dsn);
            oVMWare = new VMWare(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset, dsn);
            oError = new Errors(intProfile, dsn);
            oZeus = new Zeus(intProfile, dsnZeus);
            oVariable = new Variables(intEnvironment);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);

            lblTitle.Text = "Provisioning Issue";
            string strUser = Request.ServerVariables["logon_user"];
            strUser = strUser.Substring(strUser.LastIndexOf("\\") + 1);
            intUser = oUser.GetId(strUser);

            if (intUser > 0)
            {
                if (Request.QueryString["fixed"] != null && Request.QueryString["fixed"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "fixed", "<script type=\"text/javascript\">alert('The issue has been saved and the device is queued to continue.\\n\\nThis window will now be closed.');window.close();<" + "/" + "script>");
                if (Request.QueryString["type"] != null && Request.QueryString["id"] != null)
                {
                    try
                    {
                        intID = Int32.Parse(oFunction.decryptQueryString(Request.QueryString["id"]));
                    }
                    catch
                    {
                        intID = 0;
                    }
                    if (intID > 0)
                    {
                        if (!IsPostBack)
                        {
                            bool boolVMware = false;
                            if (Request.QueryString["type"] == "s")
                            {
                                lblType.Text = "Server";
                                lblLabel.Text = "Design ID:";
                                DataSet ds = oServer.GetErrors(intID);
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    if (dr["fixed"].ToString() == "")
                                    {
                                        lblName.Text = dr["servername"].ToString();
                                        lblDate.Text = dr["created"].ToString();
                                        lblIssue.Text = dr["reason"].ToString();
                                        
                                        int intServer = 0;
                                        if (dr["serverid"].ToString() != "")
                                            intServer = Int32.Parse(dr["serverid"].ToString());
                                        if (intServer > 0)
                                        {
                                            int intOS = 0;
                                            Int32.TryParse(oServer.Get(intServer, "osid"), out intOS);
                                            lblOS.Text = oOperatingSystem.Get(intOS, "name");
                                            lblValue.Text = oServer.Get(intServer, "answerid");
                                        }

                                        int intAsset = 0;
                                        if (dr["assetid"].ToString() != "")
                                            intAsset = Int32.Parse(dr["assetid"].ToString());
                                        lblAsset.Text = intAsset.ToString();
                                        lblStep.Text = dr["step"].ToString();

                                        if (intAsset != 0)
                                        {
                                            string strILO = oAsset.GetServerOrBlade(intAsset, "ilo");
                                            if (strILO != "")
                                            {
                                                panVMwareNo.Visible = true;
                                                lblConsole.Text = "<a href=\"https://" + strILO + "\" target=\"_blank\">" + strILO + "</a>";
                                            }
                                            else
                                            {
                                                boolVMware = true;
                                            }
                                        }
                                    }
                                }

                                // Load Existing Errors
                                Tab oTab = new Tab("", 0, "divMenu1", true, false);
                                rptRelated.DataSource = oError.Gets(lblIssue.Text, intUser);
                                rptRelated.DataBind();
                                int intTab = 0;
                                foreach (RepeaterItem ri in rptRelated.Items)
                                {
                                    intTab++;
                                    oTab.AddTab("Solution # " + intTab.ToString(), "");
                                    ((Button)ri.FindControl("btnSelect")).Attributes.Add("onclick", "return confirm('Are you sure you want to select this solution as the fix?');");
                                    Label lblAttach = (Label)ri.FindControl("lblAttach");
                                    Panel panAttach = (Panel)ri.FindControl("panAttach");
                                    if (lblAttach.Text != "")
                                    {
                                        panAttach.Visible = true;
                                        string strAttach = lblAttach.Text;
                                        if (strAttach.Contains("\\") == true)
                                            strAttach = strAttach.Substring(strAttach.LastIndexOf("\\") + 1);
                                        lblAttach.Text = "<a href=\"" + lblAttach.Text + "\" target=\"_blank\">" + strAttach + "</a>";
                                    }
                                }
                                strMenuTab1 = oTab.GetTabs();
                                trNone.Visible = (rptRelated.Items.Count == 0);

                            }
                            else if (Request.QueryString["type"] == "w")
                            {
                                if (!IsPostBack)
                                {
                                    lblType.Text = "Workstation";
                                    lblLabel.Text = "Request ID:";
                                    DataSet ds = oWorkstation.GetVirtualErrors(intID);
                                    foreach (DataRow dr in ds.Tables[0].Rows)
                                    {
                                        if (dr["fixed"].ToString() == "")
                                        {
                                            lblName.Text = dr["workstationname"].ToString();
                                            lblDate.Text = dr["created"].ToString();
                                            lblIssue.Text = dr["reason"].ToString();

                                            int intWorkstation = 0;
                                            if (dr["workstationid"].ToString() != "")
                                                intWorkstation = Int32.Parse(dr["workstationid"].ToString());
                                            if (intWorkstation > 0)
                                            {
                                                int intOS = 0;
                                                Int32.TryParse(oWorkstation.GetVirtual(intWorkstation, "osid"), out intOS);
                                                lblOS.Text = oOperatingSystem.Get(intOS, "name");
                                                lblValue.Text = oWorkstation.GetVirtual(intWorkstation, "requestid");
                                            }

                                            int intAsset = 0;
                                            if (dr["assetid"].ToString() != "")
                                                intAsset = Int32.Parse(dr["assetid"].ToString());
                                            lblAsset.Text = intAsset.ToString();
                                            lblStep.Text = dr["step"].ToString();

                                            boolVMware = true;
                                        }
                                    }

                                    // Load Existing Errors
                                    Tab oTab = new Tab("", 0, "divMenu1", true, false);
                                    rptRelated.DataSource = oError.Gets(lblError.Text, 0);
                                    rptRelated.DataBind();
                                    int intTab = 0;
                                    foreach (RepeaterItem ri in rptRelated.Items)
                                    {
                                        intTab++;
                                        oTab.AddTab("Solution # " + intTab.ToString(), "");
                                        ((Button)ri.FindControl("btnSelect")).Attributes.Add("onclick", "return confirm('Are you sure you want to select this solution as the fix?');");
                                        Label lblAttach = (Label)ri.FindControl("lblAttach");
                                        Panel panAttach = (Panel)ri.FindControl("panAttach");
                                        if (lblAttach.Text != "")
                                        {
                                            panAttach.Visible = true;
                                            string strAttach = lblAttach.Text;
                                            if (strAttach.Contains("\\") == true)
                                                strAttach = strAttach.Substring(strAttach.LastIndexOf("\\") + 1);
                                            lblAttach.Text = "<a href=\"" + lblAttach.Text + "\" target=\"_blank\">" + strAttach + "</a>";
                                        }
                                    }
                                    strMenuTab1 = oTab.GetTabs();
                                    trNone.Visible = (rptRelated.Items.Count == 0);
                                }
                            }

                            if (boolVMware == true)
                            {
                                panVMware.Visible = true;
                                DataSet dsGuest = oVMWare.GetGuest(lblName.Text);
                                if (dsGuest.Tables[0].Rows.Count > 0)
                                {
                                    DataRow drGuest = dsGuest.Tables[0].Rows[0];
                                    int intDatastore = Int32.Parse(drGuest["datastoreid"].ToString());
                                    lblDataStore.Text = oVMWare.GetDatastore(intDatastore, "name");
                                    int intHost = Int32.Parse(drGuest["hostid"].ToString());
                                    int intCluster = Int32.Parse(oVMWare.GetHost(intHost, "clusterid"));
                                    lblCluster.Text = oVMWare.GetCluster(intCluster, "name");
                                    int intFolder = Int32.Parse(oVMWare.GetCluster(intCluster, "folderid"));
                                    lblFolder.Text = oVMWare.GetFolder(intFolder, "name");
                                    int intDataCenter = Int32.Parse(oVMWare.GetFolder(intFolder, "datacenterid"));
                                    lblDataCenter.Text = oVMWare.GetDatacenter(intDataCenter, "name");
                                    int intVirtualCenter = Int32.Parse(oVMWare.GetDatacenter(intDataCenter, "virtualcenterid"));
                                    lblVirtualCenter.Text = oVMWare.GetVirtualCenter(intVirtualCenter, "name");
                                }
                            }

                            if (lblName.Text == "")
                                lblError.Text = "This device is not experiencing any provisioning issues at the moment (" + intID.ToString() + ")";

                            // Load the case codes
                            ddlCode.DataTextField = "name";
                            ddlCode.DataValueField = "id";
                            ddlCode.DataSource = oError.GetTypeTypes(2, 1);
                            ddlCode.DataBind();
                            ddlCode.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        }
                    }
                    else
                        lblError.Text = "Could not find record";
                }
                else
                    lblError.Text = "Invalid Parameter(s)";
            }
            else
                lblError.Text = "Could not find a user account for userID = " + strUser;

            if (lblError.Text == "")
            {
                panIssue.Visible = true;
                btnFixed.Attributes.Add("onclick", "return ValidateText('" + txtIssue.ClientID + "','Please enter the issue') && ValidateText('" + txtResolution.ClientID + "','Please enter the resolution') && ValidateDropDown('" + ddlCode.ClientID + "','Please select a case code') && confirm('Are you sure you want to mark this error as fixed?') && ProcessButton(this);");
                radNew.Attributes.Add("onclick", "ShowHideDiv('" + divNew.ClientID + "','inline');ShowHideDiv('" + divExisting.ClientID + "','none');");
                radExisting.Attributes.Add("onclick", "ShowHideDiv('" + divNew.ClientID + "','none');ShowHideDiv('" + divExisting.ClientID + "','inline');");
            }
            else
            {
                panDenied.Visible = true;
                btnClose.Attributes.Add("onclick", "window.close();");
            }
        }
        protected void btnFixed_Click(Object Sender, EventArgs e)
        {
            int intStep = Int32.Parse(lblStep.Text);
            int intAsset = Int32.Parse(lblAsset.Text);
            string strPath = "";
            if (txtFile.FileName != "" && txtFile.PostedFile != null)
            {
                string strDirectory = oVariable.DocumentsFolder() + "errors";
                if (Directory.Exists(strDirectory) == false)
                    Directory.CreateDirectory(strDirectory);
                string strFile = txtFile.PostedFile.FileName.Trim();
                string strFileName = strFile.Substring(strFile.LastIndexOf("\\") + 1);
                string strExtension = txtFile.FileName;
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                strPath = strDirectory + "\\" + strFileName;
                txtFile.PostedFile.SaveAs(strPath);
            }
            int intError = oError.Add(lblError.Text, txtIssue.Text, txtResolution.Text, 0, strPath, intUser);
            Fix(intStep, intAsset, intError, -999);
        }
        protected void btnSelect_Click(Object Sender, EventArgs e)
        {
            int intStep = Int32.Parse(lblStep.Text);
            int intAsset = Int32.Parse(lblAsset.Text);
            Button oButton = (Button)Sender;
            int intError = Int32.Parse(oButton.CommandArgument);
            Fix(intStep, intAsset, intError, -999);
        }
        private void Fix(int intStep, int intAsset, int intError, int intUser)
        {
            if (lblType.Text == "Server")
                oServer.UpdateError(intID, intStep, intError, intUser, true, dsnAsset);
            else if (lblType.Text == "Workstation")
                oWorkstation.UpdateVirtualError(intID, intStep, intError, intUser);
            else
                Response.Redirect(Request.Url.PathAndQuery + "&error=true");

            if (intAsset > 0)
            {
                string strSerial = oAsset.Get(intAsset, "serial");
                oZeus.UpdateResults(strSerial);
            }
            Response.Redirect(Request.Url.PathAndQuery + "&fixed=true");
        }
    }
}
