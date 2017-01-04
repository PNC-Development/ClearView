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
    public partial class rr_dns_create : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected bool boolChange = (ConfigurationManager.AppSettings["REQUIRE_CC_DNS_CREATE"] == "1");
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
            strDomain = "." + oVariable.DNS_Domain();
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
                if (!IsPostBack)
                {
                    ddlType.DataValueField = "id";
                    ddlType.DataTextField = "name";
                    ddlType.DataSource = oDNS.Gets(1);
                    ddlType.DataBind();
                    ddlType.Items.Insert(0, new ListItem("-- SELECT --", "0"));
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
                string strQIP = "";
                if (trQIP.Visible == true)
                    strQIP = "&& ValidateDropDown('" + ddlType.ClientID + "','Please select an object type')";

                btnContinue.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a device name')" + 
                    strQIP +
                    " && ValidateNumberBetween('" + txtIP1.ClientID + "',1,255,'Please enter a valid IP Address')" +
                    " && ValidateNumberBetween('" + txtIP2.ClientID + "',1,255,'Please enter a valid IP Address')" +
                    " && ValidateNumberBetween('" + txtIP3.ClientID + "',1,255,'Please enter a valid IP Address')" +
                    " && ValidateNumberBetween('" + txtIP4.ClientID + "',1,255,'Please enter a valid IP Address')" +
                    " && ProcessButton(this)" +
                    ";");
                btnReset.Attributes.Add("onclick", "return ProcessButton(this)" +
                    ";");
            }
            txtName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnContinue.ClientID + "').click();return false;}} else {return true}; ");
            txtIP1.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnContinue.ClientID + "').click();return false;}} else {return true}; ");
            txtIP2.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnContinue.ClientID + "').click();return false;}} else {return true}; ");
            txtIP3.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnContinue.ClientID + "').click();return false;}} else {return true}; ");
            txtIP4.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnContinue.ClientID + "').click();return false;}} else {return true}; ");
            txtAlias.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnAdd.ClientID + "').click();return false;}} else {return true}; ");
            btnCancel1.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service request?');");
            txtContact1.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divContact1.ClientID + "','" + lstContact1.ClientID + "','" + hdnContact1.ClientID + "','" + oVariableUser.URL() + "/frame/users.aspx',2);");
            lstContact1.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtContact2.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divContact2.ClientID + "','" + lstContact2.ClientID + "','" + hdnContact2.ClientID + "','" + oVariableUser.URL() + "/frame/users.aspx',2);");
            lstContact2.Attributes.Add("ondblclick", "AJAXClickRow();");
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
            string strIP = txtIP1.Text + "." + txtIP2.Text + "." + txtIP3.Text + "." + txtIP4.Text;
            oDNS.AddDNS(intRequest, intItem, intNumber, "CREATE", "", "", strIP, "", txtName.Text, "", "NONE", Int32.Parse(ddlType.SelectedItem.Value));
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

            string strNameNew = dr["name_new"].ToString();
            string strIPNew = dr["ip_new"].ToString();
            string strAliasNew = dr["alias_new"].ToString();

            ddlType.SelectedValue = dr["typeid"].ToString();
            txtName.Text = strNameNew;
            char[] strIPSplit = { '.' };
            string[] strIP = strIPNew.Split(strIPSplit);
            txtIP1.Text = strIP[0];
            txtIP2.Text = strIP[1];
            txtIP3.Text = strIP[2];
            txtIP4.Text = strIP[3];

            if (strAliasNew != "NONE")
            {
                // Show Confirmation Page
                lblName.Text = strNameNew;
                lblIP.Text = strIPNew;
                lblAlias.Text = strAliasNew;
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
                bool boolFoundName = false;
                bool boolFoundIP = false;
                // Get Values
                if (strNameNew != "")
                {
                    string strTemp = "";
                    if (boolDNS_QIP == true)
                    {
                        strTemp = oWebService.SearchDNSforPNC("", strNameNew, false, true);
                        if (strTemp.StartsWith("***") == false)
                            boolFoundName = true;
                    }
                    if (boolDNS_Bluecat == true)
                    {
                        strTemp = oWebService.SearchBluecatDNS("", strNameNew);
                        if (strTemp.StartsWith("***") == false)
                            boolFoundName = true;
                    }
                }
                if (strIPNew != "")
                {
                    string strTemp = "";
                    if (boolDNS_QIP == true)
                    {
                        strTemp = oWebService.SearchDNSforPNC(strIPNew, "", false, true);
                        if (strTemp.StartsWith("***") == false)
                            boolFoundIP = true;
                    }
                    if (boolDNS_Bluecat == true)
                    {
                        strTemp = oWebService.SearchBluecatDNS(strIPNew, "");
                        if (strTemp.StartsWith("***") == false)
                            boolFoundIP = true;
                    }
                }

                //DataSet ds = oServer.GetDNS(strNameNew);
                //if (ds.Tables[0].Rows.Count == 0)
                if (boolFoundName == false && boolFoundIP == false)
                {
                    //if (boolFound == true)
                    //{
                    //    btnReset.Enabled = false;
                    //    btnConfirm.Visible = false;
                    //    panExist.Visible = true;
                    //    lblExist.Text = "This device already exists in DNS. Please try again...";
                    //}
                    //else
                    //{
                        bool boolPermit = true;
                        panAlias.Visible = boolDNS_QIP;
                        panShow.Visible = true;
                        lblName.Text = strNameNew;
                        lblIP.Text = strIPNew;
                        lblAlias.Text = strAliasNew;

                        // Check Permission and either show read only, or permit edit
                        if (oApplication.Get(intApplication, "dns") == "1" || oUser.IsAdmin(intProfile))
                            boolPermit = true;

                        if (boolPermit == true)
                        {
                            btnContinue.Visible = false;
                            ddlType.Enabled = false;
                            txtName.Enabled = false;
                            txtIP1.Enabled = false;
                            txtIP2.Enabled = false;
                            txtIP3.Enabled = false;
                            txtIP4.Enabled = false;
                            txtAlias.Enabled = true;
                            lstAlias.Enabled = true;
                            txtContact1.Enabled = true;
                            txtContact2.Enabled = true;
                            btnAdd.Attributes.Add("onclick", "return AddDNS('" + lstAlias.ClientID + "','" + txtAlias.ClientID + "','" + hdnAlias.ClientID + "');");
                            btnEdit.Attributes.Add("onclick", "return EditDNS('" + lstAlias.ClientID + "','" + txtAlias.ClientID + "','" + hdnAlias.ClientID + "');");
                            btnRemove.Attributes.Add("onclick", "return RemoveDNS('" + lstAlias.ClientID + "','" + hdnAlias.ClientID + "');");
                            string strChange = "";
                            if (boolChange == true)
                            {
                                panChange.Visible = true;
                                strChange = " && ValidateTextLength('" + txtChange.ClientID + "', 'Please enter a valid change control number\\n\\n - Must start with either \"CHG\" or \"PTM\"\\n - Must be exactly 10 characters in length', 10, ['CHG','PTM'], ['CHG0000000','PTM0000000','CHG1111111','PTM1111111','CHG9999999','PTM9999999','CHGXXXXXXX','PTMXXXXXXX'])";
                            }
                            btnConfirm.Attributes.Add("onclick", "return ValidateHidden0('" + hdnContact1.ClientID + "','" + txtContact1.ClientID + "','Please enter the LAN ID of your contact # 1')" +
                                " && ValidateHidden0('" + hdnContact2.ClientID + "','" + txtContact2.ClientID + "','Please enter the LAN ID of your contact # 2')" +
                                strChange +
                                " && ValidateText('" + txtReason.ClientID + "','Please enter a reason')" +
                                " && ProcessButton(this)" +
                                ";");
                        }
                        else
                        {
                            btnConfirm.Visible = false;
                            panAccess.Visible = true;
                            btnAdd.Enabled = false;
                            btnEdit.Enabled = false;
                            btnRemove.Enabled = false;
                            DataSet dsDecoms = oApplication.GetDecoms();
                            if (dsDecoms.Tables[0].Rows.Count > 0)
                            {
                                strContacts += "<tr><td colspan=\"2\">Alternatively, you can contact a resource from one of the following departments:</td></tr>";
                                foreach (DataRow drDecom in dsDecoms.Tables[0].Rows)
                                    strContacts += "<tr><td></td><td>" + drDecom["name"].ToString() + "</td></tr>";
                            }
                        }
                    //}
                }
                else
                {
                    btnReset.Enabled = false;
                    btnConfirm.Visible = false;
                    panExist.Visible = true;
                    lblExist.Text = "The " + (boolFoundName ? "device name" : "IP address") + " is already registered in DNS. Please try again...";
                    //lblExist.Text = "This device already exists in the database. Please try again...";
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
            string strNameNew = lblName.Text;
            string strIPNew = lblIP.Text;
            string strAlias = Request.Form[hdnAlias.UniqueID];
            while (strAlias.Contains(";") == true)
                strAlias = strAlias.Replace(";", " ");
            string strAliasNew = strAlias;
            oDNS.UpdateDNS(intRequest, intItem, intNumber, "", strIPNew, "", strNameNew, "", strAliasNew, "", Int32.Parse(Request.Form[hdnContact1.UniqueID]), Int32.Parse(Request.Form[hdnContact2.UniqueID]), txtChange.Text, txtReason.Text);
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