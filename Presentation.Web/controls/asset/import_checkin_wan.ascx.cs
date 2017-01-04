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
using System.IO;
using Microsoft.ApplicationBlocks.Data;
using System.Data.OleDb;
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class import_checkin_wan : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
      
        protected Pages oPage;
        protected Platforms oPlatform;
        protected Asset oAsset;
        protected Variables oVariable;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strResults = "";
        private StringBuilder sb = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oVariable = new Variables(intEnvironment);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["import"] != null && Request.QueryString["import"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "imported", "<script type=\"text/javascript\">alert('Asset import has finished!\\n\\nAn email has been sent to you with the results of this import.');window.navigate('" + oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "');<" + "/" + "script>");
            if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != "")
                btnTemplate.Attributes.Add("onclick", "return OpenWindow('ASSETIMPORT', '" + oPlatform.Get(Int32.Parse(Request.QueryString["pid"]), "asset_checkin_path_excel") + "');");
            btnImport.Attributes.Add("onclick", "return ValidateText('" + oFile.ClientID + "','Please select a file to import');");
        }

        protected void btnImport_Click(Object Sender, EventArgs e)
        {
            int intSuccess = 0;
            int intInvalid = 0;
            int intOverwrite = 0;

            if (oFile.PostedFile != null && oFile.FileName != "")
            {
                oVariable = new Variables(intEnvironment);
                string strPhysical = oVariable.DocumentsFolder() + "\\imports\\";
                if (Directory.Exists(strPhysical) == false)
                    Directory.CreateDirectory(strPhysical);
                DateTime _now = DateTime.Now;
                string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                string strFile = strPhysical + strNow + ".xls";
                oFile.PostedFile.SaveAs(strFile);
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFile + ";Extended Properties=Excel 8.0;";
                OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [DoNotRename$]", strConn);
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "ExcelInfo");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[0].ToString().Trim() == "")
                        break;
                    else
                    {
                        
                        bool boolInvalid = false;
                        string strModel = dr[0].ToString().Trim();
                        string strPlatform = strModel.Substring(0, strModel.IndexOf("-")).Trim();
                        int intPlatform = GetPlatform(strPlatform);
                        string strSerial = dr[1].ToString().Trim();
                        if (strModel.IndexOf("-") > -1)
                        {
                            strModel = strModel.Substring(strModel.IndexOf("-") + 1).Trim();
                            if (strModel.IndexOf("-") > -1)
                            {
                                string strType = strModel.Substring(0, strModel.IndexOf("-")).Trim();
                                int intType = GetType(strType, intPlatform);
                                int intModel = 0;
                                if (strModel.IndexOf("-") > -1)
                                {
                                    strModel = strModel.Substring(strModel.IndexOf("-") + 1).Trim();
                                    intModel = GetModel(strModel, intType);
                                    string strAsset = dr[2].ToString().Trim();
                                    int intDepot = GetDepot(dr[3].ToString().Trim());
                                    int intDepotRoom = GetDepotRoom(dr[4].ToString().Trim());
                                    int intShelfNumber = GetShelf(dr[5].ToString().Trim());
                                    string strProjectNumber = dr[6].ToString().Trim();
                                    string strPONumber = dr[7].ToString().Trim();
                                    string strReceivedOn = dr[8].ToString().Trim();
                                    DateTime datReceived = DateTime.Now;
                                    if (strReceivedOn != "")
                                        datReceived = DateTime.Parse(strReceivedOn);
                                    DataSet dsExists = oAsset.Get(strSerial, intModel);
                                    int intAsset = 0;
                                    if (dsExists.Tables[0].Rows.Count > 0)
                                    {
                                        if (chkOverwrite.Checked == true)
                                        {
                                            intOverwrite++;
                                            intAsset = Int32.Parse(dsExists.Tables[0].Rows[0]["id"].ToString());
                                            intSuccess++;
                                            oAsset.UpdateNetwork(intAsset, "", (int)AssetStatus.Available, intProfile, datReceived, intDepot, intDepotRoom, intShelfNumber, 0, 0, 0, 0, 0, 0, "");
                                            sb.Append("<tr><td><img src=\"");
                                            sb.Append(oVariable.ImageURL());
                                            sb.Append("/images/check.gif\" border=\"0\" align=\"absmiddle\"/></td><td>Asset ");
                                            sb.Append(strSerial);
                                            sb.Append(" (");
                                            sb.Append(dr[0].ToString().Trim());
                                            sb.Append(") already exists and was successfully overwritten</td></tr>");

                                        }
                                        else
                                        {
                                            sb.Append("<tr><td><img src=\"");
                                            sb.Append(oVariable.ImageURL());
                                            sb.Append("/images/alert.gif\" border=\"0\" align=\"absmiddle\"/></td><td>Asset ");
                                            sb.Append(strSerial);
                                            sb.Append(" (");
                                            sb.Append(dr[0].ToString().Trim());
                                            sb.Append(") already exists and was NOT overwritten</td></tr>");
                                        }
                                    }
                                    else
                                    {
                                        intSuccess++;
                                        intAsset = oAsset.Add("", intModel, strSerial, strAsset, (int)AssetStatus.Arrived, intProfile, datReceived, 0, 1);
                                        oAsset.AddNetwork(intAsset, "", (int)AssetStatus.Available, intProfile, datReceived, intDepot, intDepotRoom, intShelfNumber, 0, 0, 0, 0, 0, 0, "");
                                        sb.Append("<tr><td><img src=\"");
                                        sb.Append(oVariable.ImageURL());
                                        sb.Append("/images/check.gif\" border=\"0\" align=\"absmiddle\"/></td><td>Asset ");
                                        sb.Append(strSerial);
                                        sb.Append(" (");
                                        sb.Append(dr[0].ToString().Trim());
                                        sb.Append(") was imported successfully</td></tr>");
                                    }
                                }
                                else
                                    boolInvalid = true;
                            }
                            else
                                boolInvalid = true;
                        }
                        else
                            boolInvalid = true;

                        if (boolInvalid == true)
                        {
                            intInvalid++;
                            sb.Append("<tr><td><img src=\"");
                            sb.Append(oVariable.ImageURL());
                            sb.Append("/images/error.gif\" border=\"0\" align=\"absmiddle\"/></td><td>There was a problem with the information provided for asset serial# ");
                            sb.Append(strSerial);
                            sb.Append(" (");
                            sb.Append(dr[0].ToString().Trim());
                            sb.Append(")</td></tr>");
                        }
                    }
                }
                sb.Append("<tr><td colspan=\"2\">&nbsp;</td></tr><tr><td colspan=\"2\"><b><img src=\"");
                sb.Append(oVariable.ImageURL());
                sb.Append("/images/bigCheck.gif\" border=\"0\" align=\"absmiddle\"/> Successfully Imported ");
                sb.Append(intSuccess);
                sb.Append(" Assets, Error Importing ");
                sb.Append(intInvalid);
                sb.Append(" Assets, Overwrote ");
                sb.Append(intOverwrite);
                sb.Append(" Assets</b></td></tr>");

                sb.Insert(0, "<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                sb.Append("</table>");
                Functions oFunction = new Functions(intProfile, dsn, intEnvironment);
                Users oUser = new Users(intProfile, dsn);

                strResults = sb.ToString();
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_INVENTORY_MANAGER");
                oFunction.SendEmail("ClearView Asset Import", oUser.GetName(intProfile), "", strEMailIdsBCC, "ClearView Asset Import", "<p>" + strResults + "</p>", true, false);
            }

            Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&import=true");
        }
        protected int GetPlatform(string _name)
        {
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_platforms WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
            if (ds.Tables[0].Rows.Count == 0)
            {
                oPlatform.Add(_name, intProfile, 0, "", "", 0, 0, 0, 0, "", "", "", "", "", "", "", "", "", "", "", 100, 0, 0, 1);
                sb.Append("<tr><td>-</td><td>Add Platform ");
                sb.Append(_name);
                sb.Append("</td></tr>");
                return GetPlatform(_name);
            }
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["platformid"].ToString());
        }
        protected int GetType(string _name, int _platformid)
        {
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_types WHERE name = '" + _name + "' AND platformid = " + _platformid + " AND enabled = 1 AND deleted = 0");
            if (ds.Tables[0].Rows.Count == 0)
            {
                Types oType = new Types(intProfile, dsn);
                oType.Add(_platformid, _name, "", "", "", "", "", "", "", "", 10, 5, 0, 1);
                sb.Append("<tr><td>-</td><td>Add Asset Type ");
                sb.Append(_name);
                sb.Append("</td></tr>");
                return GetType(_name, _platformid);
            }
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        }
        protected int GetModel(string _name, int _typeid)
        {
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_models WHERE name = '" + _name + "' AND typeid = " + _typeid + " AND enabled = 1 AND deleted = 0");
            if (ds.Tables[0].Rows.Count == 0)
            {
                Models oModel = new Models(intProfile, dsn);
                oModel.Add(_typeid, _name, "", "", 0, 0, 0,0,0,0, 0, 0, 0, 0, 0, 0, 0, 1);
                sb.Append("<tr><td>-</td><td>Add Model ");
                sb.Append(_name);
                sb.Append("</td></tr>");
                return GetModel(_name, _typeid);
            }
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        }
        protected int GetDepot(string _name)
        {
            if (_name.Trim() == "")
                return 0;
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_depot WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
            if (ds.Tables[0].Rows.Count == 0)
            {
                Depot oDepot = new Depot(intProfile, dsn);
                oDepot.Add(_name, 1);
                sb.Append("<tr><td>-</td><td>Add Depot ");
                sb.Append(_name);
                sb.Append("</td></tr>");
                return GetDepot(_name);
            }
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        }
        protected int GetDepotRoom(string _name)
        {
            if (_name.Trim() == "")
                return 0;
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_depot_rooms WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
            if (ds.Tables[0].Rows.Count == 0)
            {
                DepotRoom oDepotRoom = new DepotRoom(intProfile, dsn);
                oDepotRoom.Add(_name, 1);
                sb.Append("<tr><td>-</td><td>Add Depot Room ");
                sb.Append(_name);
                sb.Append("</td></tr>");
                return GetDepotRoom(_name);
            }
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        }
        protected int GetShelf(string _name)
        {
            if (_name.Trim() == "")
                return 0;
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_shelfs WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
            if (ds.Tables[0].Rows.Count == 0)
            {
                Shelf oShelf = new Shelf(intProfile, dsn);
                oShelf.Add(_name, 1);
                sb.Append("<tr><td>-</td><td>Add Shelf ");
                sb.Append(_name);
                sb.Append("</td></tr>");
                return GetShelf(_name);
            }
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        }
    }
}