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
using NCC.ClearView.Application.Core.ClearViewWS;
namespace NCC.ClearView.Presentation.Web
{
    public partial class rr_dns_delete : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected Servers oServer;
        protected Classes oClass;
        protected Applications oApplication;
        protected Forecast oForecast;
        protected Functions oFunction;
        protected Users oUser;
        protected DNS oDNS;
        protected Variables oVariable;
        protected Settings oSetting;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected string strContacts = "";
        protected string strConfirm = "";
        protected string strDomain = "N/A";
        protected bool boolDNS_QIP = false;
        protected bool boolDNS_Bluecat = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oUser = new Users(intProfile, dsn);
            oDNS = new DNS(intProfile, dsn);
            Variables oVariableUser = new Variables(intEnvironment);
            if (intEnvironment < 3)
                intEnvironment = 3;
            oVariable = new Variables(intEnvironment);
            oSetting = new Settings(0, dsn);
            boolDNS_QIP = oSetting.IsDNS_QIP();
            boolDNS_Bluecat = oSetting.IsDNS_Bluecat();
            trQIP.Visible = boolDNS_QIP;

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rid"] != "" && Request.QueryString["rid"] != null)
            {
                int intRequest = Int32.Parse(Request.QueryString["rid"]);
                LoadValues();
                int intItem = Int32.Parse(lblItem.Text);
                int intNumber = Int32.Parse(lblNumber.Text);
                int intApp = oRequestItem.GetItemApplication(intItem);
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable != "")
                {
                    panDeliverable.Visible = true;
                    btnDeliverable.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','" + strDeliverable + "');");
                }
                DataSet ds = oDNS.GetDNS(intRequest, intItem, intNumber);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (!IsPostBack)
                        LoadServer(ds.Tables[0].Rows[0]);
                }
                else
                {
                    panSearch.Visible = true;
                    btnReset.Enabled = false;
                    btnConfirm.Visible = false;
                    btnNext.Visible = false;
                }
                btnContinue.Attributes.Add("onclick", "return EnsureDNS('" + txtSearchName.ClientID + "','" + txtSearchIP.ClientID + "')" +
                    " && ProcessButton(this)" +
                    ";");
                btnReset.Attributes.Add("onclick", "return ProcessButton(this)" +
                    ";");
            }
            txtSearchName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnContinue.ClientID + "').click();return false;}} else {return true}; ");
            txtSearchIP.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnContinue.ClientID + "').click();return false;}} else {return true}; ");
            btnCancel1.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service request?');");
        }
        private void LoadValues()
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            DataSet dsItems = oRequestItem.GetForms(intRequest);
            if (dsItems.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drItem in dsItems.Tables[0].Rows)
                {
                    if (drItem["done"].ToString() == "-1")
                    {
                        lblItem.Text = drItem["itemid"].ToString();
                        lblNumber.Text = drItem["number"].ToString();
                        break;
                    }
                }
            }
        }
        protected void btnContinue_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            int intItem = Int32.Parse(lblItem.Text);
            int intNumber = Int32.Parse(lblNumber.Text);
            oDNS.DeleteDNS(intRequest, intItem, intNumber);
            if (txtSearchName.Text != "")
                oDNS.AddDNS(intRequest, intItem, intNumber, "DELETE", "NAME", "", "", txtSearchName.Text, "", "", "", 0);
            if (txtSearchIP.Text != "")
                oDNS.AddDNS(intRequest, intItem, intNumber, "DELETE", "IP", txtSearchIP.Text, "", "", "", "", "", 0);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"]);
        }
        protected void btnReset_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            int intItem = Int32.Parse(lblItem.Text);
            int intNumber = Int32.Parse(lblNumber.Text);
            oDNS.DeleteDNS(intRequest, intItem, intNumber);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"]);
        }
        private void LoadServer(DataRow dr)
        {
            int intRequest = Int32.Parse(dr["requestid"].ToString());
            int intItem = Int32.Parse(dr["itemid"].ToString());
            int intNumber = Int32.Parse(dr["number"].ToString());

            string strNameCurrent = dr["name_current"].ToString();
            string strIPCurrent = dr["ip_current"].ToString();
            string strAliasCurrent = dr["alias_current"].ToString();
            string strNameNew = dr["name_new"].ToString();
            string strIPNew = dr["ip_new"].ToString();
            string strAliasNew = dr["alias_new"].ToString();

            txtSearchName.Text = strNameCurrent;
            txtSearchIP.Text = strIPCurrent;

            if (strNameCurrent != "" && strIPCurrent != "")
            {
                // Show Confirmation Page
                lblName.Text = strNameCurrent;
                lblIP.Text = strIPCurrent;
                lblAlias.Text = strAliasCurrent;
                panConfirm.Visible = true;
                strConfirm = oDNS.GetDNSBody(intRequest, intItem, intNumber, true, intEnvironment);
                btnNext.Attributes.Add("onclick", "return ValidateCheck('" + chkAgree.ClientID + "','Please check the box stating that you agree to the disclaimer notice')" +
                    " && ProcessButton(this)" +
                    ";");
                btnDiscard.Attributes.Add("onclick", "return confirm('WARNING: Starting over will discard all the changes you have made.\\n\\nAre you sure you want to continue?') && ProcessButton(this);");
                btnContinue.Visible = false;
                btnConfirm.Visible = false;
            }
            else
            {
                panSearch.Visible = true;
                btnNext.Visible = false;

                // Get Record from DNS
                System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                ClearViewWebServices oWebService = new ClearViewWebServices();
                oWebService.Timeout = Int32.Parse(ConfigurationManager.AppSettings["WS_TIMEOUT"]);
                oWebService.Credentials = oCredentials;
                oWebService.Url = oVariable.WebServiceURL();

                // Get Values
                if (strNameCurrent != "")
                {
                    if (boolDNS_QIP == true)
                    {
                        strIPCurrent = oWebService.SearchDNSforPNC("", strNameCurrent, false, true);
                        if (strIPCurrent.StartsWith("***") == false)
                        {
                            strNameCurrent = oWebService.SearchDNSforPNC(strIPCurrent, "", false, true);
                            strAliasCurrent = oWebService.SearchDNSforPNC(strIPCurrent, "", true, true);
                        }
                        else
                        {
                            strNameCurrent = "";
                            strIPCurrent = "";
                        }
                        txtSearchIP.Text = strIPCurrent;
                    }
                    if (boolDNS_Bluecat == true)
                    {
                        strIPCurrent = oWebService.SearchBluecatDNS("", strNameCurrent);
                        if (strIPCurrent.StartsWith("***") == false)
                        {
                            strNameCurrent = oWebService.SearchBluecatDNS(strIPCurrent, "");
                            strAliasCurrent = "";
                        }
                        else
                        {
                            strNameCurrent = "";
                            strIPCurrent = "";
                        }
                        txtSearchIP.Text = strIPCurrent;
                    }
                }
                else if (strIPCurrent != "")
                {
                    if (boolDNS_QIP == true)
                    {
                        strNameCurrent = oWebService.SearchDNSforPNC(strIPCurrent, "", false, true);
                        if (strNameCurrent.StartsWith("***") == false)
                            strAliasCurrent = oWebService.SearchDNSforPNC(strIPCurrent, "", true, true);
                        else
                        {
                            strNameCurrent = "";
                            strIPCurrent = "";
                        }
                        txtSearchName.Text = strNameCurrent;
                    }
                    if (boolDNS_Bluecat == true)
                    {
                        strNameCurrent = oWebService.SearchBluecatDNS(strIPCurrent, "");
                        if (strNameCurrent.StartsWith("***") == false)
                            strAliasCurrent = "";
                        else
                        {
                            strNameCurrent = "";
                            strIPCurrent = "";
                        }
                        txtSearchName.Text = strNameCurrent;
                    }
                }

                if (strNameCurrent.Contains(".") == true)
                {
                    strDomain = strNameCurrent.Substring(strNameCurrent.IndexOf("."));
                    strNameCurrent = strNameCurrent.Substring(0, strNameCurrent.IndexOf("."));
                }

                if (strNameCurrent != "" && strIPCurrent != "")
                {
                    DataSet ds = oServer.GetDNS(strNameCurrent);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        bool boolPermit = false;
                        int intServer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                        int intUser = (ds.Tables[0].Rows[0]["userid"].ToString() == "" ? 0 : Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString()));
                        int intOwner = (ds.Tables[0].Rows[0]["appcontact"].ToString() == "" ? 0 : Int32.Parse(ds.Tables[0].Rows[0]["appcontact"].ToString()));
                        int intPrimary = (ds.Tables[0].Rows[0]["admin1"].ToString() == "" ? 0 : Int32.Parse(ds.Tables[0].Rows[0]["admin1"].ToString()));
                        int intSecondary = (ds.Tables[0].Rows[0]["admin2"].ToString() == "" ? 0 : Int32.Parse(ds.Tables[0].Rows[0]["admin2"].ToString()));
                        int intRequestor = (ds.Tables[0].Rows[0]["requestor"].ToString() == "" ? 0 : Int32.Parse(ds.Tables[0].Rows[0]["requestor"].ToString()));
                        int intClass = (ds.Tables[0].Rows[0]["classid"].ToString() == "" ? 0 : Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString()));
                        int intProd = (intClass > 0 ? (oClass.IsProd(intClass) ? 1 : 0) : -1);

                        lblName.Text = strNameCurrent;
                        lblIP.Text = strIPCurrent;
                        lblAlias.Text = strAliasCurrent;
                        lblDomain.Text = strDomain;

                        // Check Permission and either show read only, or permit edit
                        if (intProfile == intOwner || intProfile == intPrimary || intProfile == intSecondary || intProfile == intRequestor)
                            boolPermit = true;
                        if (oApplication.Get(intApplication, "dns") == "1" || oUser.IsAdmin(intProfile) || intProfile == intUser)
                            boolPermit = true;

                        if (boolPermit == true)
                        {
                            panShow.Visible = true;
                            btnContinue.Visible = false;
                            txtSearchName.Enabled = false;
                            txtSearchIP.Enabled = false;
                            string strChange = "";
                            if (intProd != 0 || oServer.Get(intServer, "infrastructure") == "1")
                            {
                                panChange.Visible = true;
                                strChange = " && ValidateTextLength('" + txtChange.ClientID + "', 'Please enter a valid change control number\\n\\n - Must start with either \"CHG\" or \"PTM\"\\n - Must be exactly 10 characters in length', 10, ['CHG','PTM'], ['CHG0000000','PTM0000000','CHG1111111','PTM1111111','CHG9999999','PTM9999999','CHGXXXXXXX','PTMXXXXXXX'])";
                            }
                            btnConfirm.Attributes.Add("onclick", "return ValidateText('" + txtReason.ClientID + "','Please enter a reason')" +
                                strChange +
                                " && ProcessButton(this)" +
                                ";");
                        }
                        else
                        {
                            btnConfirm.Visible = false;
                            panAccess.Visible = true;
                            if (intUser > 0)
                                strContacts += "<tr><td>Server Owner:</td><td>" + oUser.GetFullName(intUser) + " (" + oUser.GetName(intUser) + ")" + "</td></tr>";
                            if (intOwner > 0)
                                strContacts += "<tr><td>Departmental Manager:</td><td>" + oUser.GetFullName(intOwner) + " (" + oUser.GetName(intOwner) + ")" + "</td></tr>";
                            if (intPrimary > 0)
                                strContacts += "<tr><td>Application Technical Lead:</td><td>" + oUser.GetFullName(intPrimary) + " (" + oUser.GetName(intPrimary) + ")" + "</td></tr>";
                            if (intSecondary > 0)
                                strContacts += "<tr><td>Administrative Contact:</td><td>" + oUser.GetFullName(intSecondary) + " (" + oUser.GetName(intSecondary) + ")" + "</td></tr>";
                            if (intRequestor > 0)
                                strContacts += "<tr><td>Design Initiated By:</td><td>" + oUser.GetFullName(intRequestor) + " (" + oUser.GetName(intRequestor) + ")" + "</td></tr>";
                            DataSet dsDecoms = oApplication.GetDecoms();
                            if (dsDecoms.Tables[0].Rows.Count > 0)
                            {
                                strContacts += "<tr><td colspan=\"2\">Alternatively, you can contact a resource from one of the following departments:</td></tr>";
                                foreach (DataRow drDecom in dsDecoms.Tables[0].Rows)
                                    strContacts += "<tr><td></td><td>" + drDecom["name"].ToString() + "</td></tr>";
                            }
                        }
                    }
                    else
                    {
                        btnReset.Enabled = false;
                        btnConfirm.Visible = false;
                        panExist.Visible = true;
                        lblExist.Text = "This device &quot;" + strNameCurrent + "&nbsp; does not exist in the database. Please try again...";
                    }
                }
                else
                {
                    btnReset.Enabled = false;
                    btnConfirm.Visible = false;
                    panExist.Visible = true;
                    lblExist.Text = "This device does not exist in DNS. Please try again...";
                }
            }
        }
        protected void btnDiscard_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            int intItem = Int32.Parse(lblItem.Text);
            int intNumber = Int32.Parse(lblNumber.Text);
            oDNS.DeleteDNS(intRequest, intItem, intNumber);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnConfirm_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            int intItem = Int32.Parse(lblItem.Text);
            int intNumber = Int32.Parse(lblNumber.Text);
            string strNameCurrent = lblName.Text;
            string strIPCurrent = lblIP.Text;
            string strAliasCurrent = lblAlias.Text;
            string strDomain = lblDomain.Text;
            oDNS.UpdateDNS(intRequest, intItem, intNumber, strIPCurrent, "", strNameCurrent, "", strAliasCurrent, "", strDomain, 0, 0, txtChange.Text, txtReason.Text);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            oRequestItem.UpdateForm(intRequest, false);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            int intItem = Int32.Parse(lblItem.Text);
            int intNumber = Int32.Parse(lblNumber.Text);
            oRequestItem.UpdateForm(intRequest, true);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            oRequest.Cancel(intRequest);
            Response.Redirect(oPage.GetFullLink(intPage));
        }
    }
}