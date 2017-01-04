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
using NCC.ClearView.Presentation.Web.Custom;
using System.Text;
namespace NCC.ClearView.Presentation.Web
{
    public partial class errors_server : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Servers oServer;
        protected Forecast oForecast;
        protected Cluster oCluster;
        protected Users oUser;
        protected Zeus oZeus;
        protected Asset oAsset;
        protected OnDemand oOnDemand;
        protected Errors oError;
        protected Incident oIncident;
        protected Functions oFunction;
        protected Variables oVariable;
        protected int intProfile;
        protected int intID;
        protected string strError = "";
        protected string strMenuTab1 = "";
        protected string strMenuTab2 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/errors_server.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oServer = new Servers(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oCluster = new Cluster(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oZeus = new Zeus(intProfile, dsnZeus);
            oAsset = new Asset(0, dsnAsset);
            oOnDemand = new OnDemand(intProfile, dsn);
            oError = new Errors(intProfile, dsn);
            oIncident = new Incident(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oVariable = new Variables(intEnvironment);
            if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
            {
                intID = Int32.Parse(Request.QueryString["sid"]);
                if (!IsPostBack)
                    LoadList();
                LoadServers();
            }
            else if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
            {
                intID = Int32.Parse(Request.QueryString["aid"]);
                if (!IsPostBack)
                    LoadList();
                LoadDesigns();
            }
            else if (!IsPostBack)
                LoadAll();
        }
        private void LoadAll()
        {
            panAll.Visible = true;

            TreeNode oNodeServer = new TreeNode();
            oNodeServer.Text = "Server Errors";
            oNodeServer.ToolTip = "Server Errors";
            oNodeServer.ImageUrl = "/images/folder.gif";
            oNodeServer.SelectAction = TreeNodeSelectAction.Expand;
            oTreeview.Nodes.Add(oNodeServer);
            DataSet dsS = oServer.GetErrors();
            foreach (DataRow drS in dsS.Tables[0].Rows)
                LoadErrors(Int32.Parse(drS["id"].ToString()), drS["url"].ToString(), oNodeServer);

            oTreeview.ExpandDepth = 2;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadErrors(int _id, string _url, TreeNode oParent)
        {
            switch (_url)
            {
                case "sid":
                    DataSet dsS = oServer.GetErrors(_id);
                    foreach (DataRow drS in dsS.Tables[0].Rows)
                    {
                        if (drS["fixed"].ToString() == "")
                        {
                            TreeNode oNode = new TreeNode();
                            oNode.Text = drS["name"].ToString() + " - " + drS["created"].ToString() + " (Step " + drS["step"].ToString() + ": " + drS["title"].ToString() + ")";
                            oNode.ImageUrl = "/images/cancel.gif";
                            oNode.NavigateUrl = Request.Path + "?" + _url + "=" + _id.ToString();
                            oParent.ChildNodes.Add(oNode);
                        }
                    }
                    break;
                case "aid":
                    DataSet dsF = oForecast.GetErrors(_id);
                    foreach (DataRow drF in dsF.Tables[0].Rows)
                    {
                        if (drF["fixed"].ToString() == "")
                        {
                            TreeNode oNode = new TreeNode();
                            oNode.Text = drF["name"].ToString() + " - " + drF["created"].ToString();
                            oNode.ImageUrl = "/images/cancel.gif";
                            oNode.NavigateUrl = Request.Path + "?" + _url + "=" + _id.ToString();
                            oParent.ChildNodes.Add(oNode);
                        }
                    }
                    break;
            }
        }
        private void LoadList()
        {
            ddlType.DataTextField = "name";
            ddlType.DataValueField = "id";
            ddlType.DataSource = oError.GetTypes(1);
            ddlType.DataBind();
            ddlType.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlType.Attributes.Add("onchange", "PopulateErrorType2s('" + ddlType.ClientID + "','" + ddlType2.ClientID + "');");
            ddlType2.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlType2.ClientID + "','" + hdnType2.ClientID + "');");
        }
        private void LoadIncidents()
        {
            Tab oTabIncidents = new Tab("", 0, "divMenu2", true, false);
            rptIncidents.DataSource = oIncident.Gets(lblIncident.Text);
            rptIncidents.DataBind();
            int intTabIncident = 0;
            foreach (RepeaterItem ri in rptIncidents.Items)
            {
                intTabIncident++;
                oTabIncidents.AddTab("Incident # " + intTabIncident.ToString(), "");
                ((Button)ri.FindControl("btnIncident")).Attributes.Add("onclick", "return confirm('Are you sure you want to select this incident to route?');");
            }
            strMenuTab2 = oTabIncidents.GetTabs();
            trIncidents.Visible = (rptIncidents.Items.Count == 0);

            btnIncident.Attributes.Add("onclick", "ShowHideDiv2('" + trIncident.ClientID + "');return false;");
        }
        private void LoadSolutions()
        {
            Tab oTab = new Tab("", 0, "divMenu1", true, false);
            rptRelated.DataSource = oError.Gets(lblSolution.Text, 0);
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

            btnSave.Attributes.Add("onclick", "return ProcessButton(this);");
            btnFix.Attributes.Add("onclick", "return ValidateText('" + txtProblem.ClientID + "','Please enter the problem') && ValidateText('" + txtResolution.ClientID + "','Please enter the resolution') && ValidateDropDown('" + ddlType.ClientID + "','Please select a case code') && confirm('Are you sure you want to mark this error as fixed?') && ProcessButton(this);");
            btnSolution.Attributes.Add("onclick", "ShowHideDiv2('" + trSolution.ClientID + "');return false;");
            btnNone.Attributes.Add("onclick", "return confirm('Are you sure you do not want to associate this error with any solution?');");
        }
        private void LoadServers()
        {
            panError.Visible = true;
            StringBuilder sbHidden = new StringBuilder();
            DataSet ds = oServer.GetErrors(intID);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intAsset = 0;
                if (dr["assetid"].ToString() != "")
                    intAsset = Int32.Parse(dr["assetid"].ToString());
                bool IsFixed = false;
                if (dr["fixed"].ToString() != "")
                {
                    IsFixed = true;
                    strError += "<tr class=\"deletedRow\">";
                    strError += "<td></td>";
                }
                else
                {
                    lblAsset.Text = intAsset.ToString();
                    lblStep.Text = dr["step"].ToString();
                    lblID.Text = dr["id"].ToString();
                    lblIncident.Text = lblSolution.Text = dr["reason"].ToString();
                    strError += "<tr>";
                    strError += "<td></td>";
                }
                if (intAsset == 0)
                    strError += "<td>" + dr["name"].ToString() + "</td>";
                else
                {
                    string strILO = oAsset.GetServerOrBlade(intAsset, "ilo");
                    if (strILO != "")
                        strError += "<td><a href=\"https://" + strILO + "\" target=\"_blank\">" + dr["name"].ToString() + "</a></td>";
                    else
                        strError += "<td>" + dr["name"].ToString() + "</td>";
                }
                strError += "<td>" + dr["reason"].ToString() + "</td>";
                strError += "<td><a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/service/design.aspx?id=" + oFunction.encryptQueryString(dr["answerid"].ToString()) + "','800','600')\">" + dr["answerid"].ToString() + "</a></td>";
                if (IsFixed == false)
                {
                    strError += "<td><input type=\"text\" maxlength=\"20\" name=\"ValidTextbox\" class=\"default\" style=\"width:100px\" onblur=\"UpdateTextValue(this,'HDN_I_" + dr["id"].ToString() + "');\" value=\"" + dr["incident"].ToString() + "\"></td>";
                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_I_");
                    sbHidden.Append(dr["id"].ToString());
                    sbHidden.Append("\" id=\"HDN_I_");
                    sbHidden.Append(dr["id"].ToString());
                    sbHidden.Append("\" value=\"");
                    sbHidden.Append(dr["incident"].ToString());
                    sbHidden.Append("\" />");
                    strError += "<td>";
                    strError += "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
                    strError += "<tr>";
                    int intAssigned = 0;
                    string strAssigned = "";
                    if (Int32.TryParse(dr["assigned"].ToString(), out intAssigned))
                        strAssigned = oUser.GetFullNameWithLanID(intAssigned);
                    strError += "<td><input type=\"text\" class=\"default\" onkeyup=\"return AJAXTextBoxGet(this,'150','200','DIV" + dr["id"].ToString() + "','LST" + dr["id"].ToString() + "','HDN_A_" + dr["id"].ToString() + "','" + oVariable.URL() + "/frame/users.aspx',2);\" style=\"width:150px;\" value=\"" + strAssigned + "\" /></td>";
                    strError += "</tr>";
                    strError += "<tr>";
                    strError += "<td>";
                    strError += "<div id=\"DIV" + dr["id"].ToString() + "\" style=\"overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC\">";
                    strError += "<select size=\"200\" name=\"LST" + dr["id"].ToString() + "\" id=\"LST" + dr["id"].ToString() + "\" class=\"default\" ondblclick=\"AJAXClickRow();\"></select>";
                    strError += "</div>";
                    strError += "</td>";
                    strError += "</tr>";
                    strError += "</table>";
                    strError += "</td>";
                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_A_" + dr["id"].ToString() + "\" id=\"HDN_A_" + dr["id"].ToString() + "\" value=\"" + dr["assigned"].ToString() + "\" />");
                }
                else if (String.IsNullOrEmpty(dr["incident"].ToString()))
                {
                    strError += "<td>---</td>";
                    strError += "<td>---</td>";
                }
                else
                {
                    strError += "<td>" + dr["incident"].ToString() + "</td>";
                    int intAssigned = 0;
                    string strAssigned = "";
                    if (Int32.TryParse(dr["assigned"].ToString(), out intAssigned))
                        strAssigned = oUser.GetFullNameWithLanID(intAssigned);
                    strError += "<td>" + strAssigned + "</td>";
                }
                strError += "<td>" + DateTime.Parse(dr["created"].ToString()).ToString() + "</td>";
                strError += "</tr>";
            }
            strError = "<table cellpadding=\"3\" cellspacing=\"2\" width=\"100%\" border=\"0\" style=\"border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td><b></b></td><td><b>Server Name</b></td><td><b>Reason</b></td><td><b>DesignID</b></td><td><b>Incident#</b></td><td><b>Assigned</b></td><td><b>Created</b></td></tr>" + strError + "</table>";
            litHidden.Text = sbHidden.ToString();

            LoadIncidents();
            LoadSolutions();
        }
        private void LoadDesigns()
        {
            panError.Visible = true;
            StringBuilder sbHidden = new StringBuilder();
            DataSet ds = oForecast.GetErrors(intID);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                bool IsFixed = false;
                if (dr["fixed"].ToString() != "")
                {
                    IsFixed = true;
                    strError += "<tr class=\"deletedRow\">";
                    strError += "<td></td>";
                }
                else
                {
                    lblAsset.Text = "0";
                    lblStep.Text = dr["step"].ToString();
                    lblID.Text = dr["id"].ToString();
                    lblIncident.Text = lblSolution.Text = dr["reason"].ToString();
                    strError += "<tr>";
                    strError += "<td></td>";
                }
                strError += "<td>" + dr["name"].ToString() + "</td>";
                strError += "<td>" + dr["reason"].ToString() + "</td>";
                strError += "<td><a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/service/design.aspx?id=" + oFunction.encryptQueryString(dr["answerid"].ToString()) + "','800','600')\">" + dr["answerid"].ToString() + "</a></td>";
                if (IsFixed == false)
                {
                    strError += "<td><input type=\"text\" maxlength=\"20\" name=\"ValidTextbox\" class=\"default\" style=\"width:100px\" onblur=\"UpdateTextValue(this,'HDN_I_" + dr["id"].ToString() + "');\" value=\"" + dr["incident"].ToString() + "\"></td>";
                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_I_");
                    sbHidden.Append(dr["id"].ToString());
                    sbHidden.Append("\" id=\"HDN_I_");
                    sbHidden.Append(dr["id"].ToString());
                    sbHidden.Append("\" value=\"");
                    sbHidden.Append(dr["incident"].ToString());
                    sbHidden.Append("\" />");
                    strError += "<td>";
                    strError += "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
                    strError += "<tr>";
                    int intAssigned = 0;
                    string strAssigned = "";
                    if (Int32.TryParse(dr["assigned"].ToString(), out intAssigned))
                        strAssigned = oUser.GetFullNameWithLanID(intAssigned);
                    strError += "<td><input type=\"text\" class=\"default\" onkeyup=\"return AJAXTextBoxGet(this,'150','200','DIV" + dr["id"].ToString() + "','LST" + dr["id"].ToString() + "','HDN_A_" + dr["id"].ToString() + "','" + oVariable.URL() + "/frame/users.aspx',2);\" style=\"width:150px;\" value=\"" + strAssigned + "\" /></td>";
                    strError += "</tr>";
                    strError += "<tr>";
                    strError += "<td>";
                    strError += "<div id=\"DIV" + dr["id"].ToString() + "\" style=\"overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC\">";
                    strError += "<select size=\"200\" name=\"LST" + dr["id"].ToString() + "\" id=\"LST" + dr["id"].ToString() + "\" class=\"default\" ondblclick=\"AJAXClickRow();\"></select>";
                    strError += "</div>";
                    strError += "</td>";
                    strError += "</tr>";
                    strError += "</table>";
                    strError += "</td>";
                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_A_" + dr["id"].ToString() + "\" id=\"HDN_A_" + dr["id"].ToString() + "\" value=\"" + dr["assigned"].ToString() + "\" />");
                }
                else if (String.IsNullOrEmpty(dr["incident"].ToString()))
                {
                    strError += "<td>---</td>";
                    strError += "<td>---</td>";
                }
                else
                {
                    strError += "<td>" + dr["incident"].ToString() + "</td>";
                    int intAssigned = 0;
                    string strAssigned = "";
                    if (Int32.TryParse(dr["assigned"].ToString(), out intAssigned))
                        strAssigned = oUser.GetFullNameWithLanID(intAssigned);
                    strError += "<td>" + strAssigned + "</td>";
                }
                strError += "<td>" + DateTime.Parse(dr["created"].ToString()).ToString() + "</td>";
                strError += "</tr>";
            }
            strError = "<table cellpadding=\"3\" cellspacing=\"2\" width=\"100%\" border=\"0\" style=\"border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td><b></b></td><td><b>Server Name</b></td><td><b>Reason</b></td><td><b>DesignID</b></td><td><b>Incident#</b></td><td><b>Assigned</b></td><td><b>Created</b></td></tr>" + strError + "</table>";
            litHidden.Text = sbHidden.ToString();

            LoadIncidents();
            LoadSolutions();
        }

        protected void btnFix_Click(Object Sender, EventArgs e)
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
            int intType = 0;
            Int32.TryParse(Request.Form[hdnType2.UniqueID], out intType);
            if (intType == 0)
                intType = Int32.Parse(ddlType.SelectedItem.Value);
            int intError = oError.Add(lblSolution.Text, txtProblem.Text, txtResolution.Text, intType, strPath, -999);
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

        protected void btnNone_Click(Object Sender, EventArgs e)
        {
            int intStep = Int32.Parse(lblStep.Text);
            int intAsset = Int32.Parse(lblAsset.Text);
            Fix(intStep, intAsset, 0, -999);
        }

        private void Fix(int intStep, int intAsset, int intError, int intUser)
        {
            if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
            {
                oServer.UpdateError(intID, intStep, intError, intUser, true, dsnAsset);
                // If Avamar error, reload avamar
                oServer.UpdateAvamar(intID);    // stored procedure will automatically find the place to reset.

                if (intAsset > 0)
                {
                    string strSerial = oAsset.Get(intAsset, "serial");
                    oZeus.UpdateResults(strSerial);
                }
            }
            else if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
            {
                oForecast.UpdateError(intID, intStep, intError, intUser);
                if (intStep == (int)NCC.ClearView.Application.Core.Forecast.ForecastAnswerErrorStep.Clustering)
                    oCluster.AddClustering(intID);
            }
            Response.Redirect(Request.Path);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            foreach (string strForm in Request.Form)
            {
                if (strForm.StartsWith("HDN_I_") == true)
                {
                    int intID = Int32.Parse(strForm.Substring(6));
                    int intAssigned = 0;
                    Int32.TryParse(Request.Form["HDN_A_" + intID.ToString()], out intAssigned);
                    if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
                        oServer.UpdateError(intID, Request.Form["HDN_I_" + intID.ToString()], intAssigned);
                    else if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                        oForecast.UpdateError(intID, Request.Form["HDN_I_" + intID.ToString()], intAssigned);
                }
            }
            Response.Redirect(Request.Url.PathAndQuery);
        }

        protected void btnIncident_Click(object sender, EventArgs e)
        {
            Button oButton = (Button)sender;
            int intKB = Int32.Parse(oButton.CommandArgument);
            Incident(intKB);
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            int intKB = oIncident.Add(lblIncident.Text, txtCompare.Text, ddlRoute.SelectedItem.Value, (chkAutomatic.Checked ? 1 : 0), txtMessage.Text, Int32.Parse(ddlPriority.SelectedItem.Value), 0, 1);
            Incident(intKB);
        }

        private void Incident(int intKB)
        {
            int intID = Int32.Parse(lblID.Text);
            oIncident.Add(intID, 0, intKB);
            Response.Redirect(Request.Path);
        }
    }
}
