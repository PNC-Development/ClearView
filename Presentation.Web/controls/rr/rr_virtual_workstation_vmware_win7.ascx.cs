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
using System.DirectoryServices;
namespace NCC.ClearView.Presentation.Web
{
    public partial class rr_virtual_workstation_vmware_win7 : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intConfidence = Int32.Parse(ConfigurationManager.AppSettings["CONFIDENCE_100"]);
        protected int intModelVirtual = Int32.Parse(ConfigurationManager.AppSettings["VirtualWorkstationModelID"]);
        protected int intModelVMware = Int32.Parse(ConfigurationManager.AppSettings["VMwareWorkstationModelID"]);
        protected int intCore = Int32.Parse(ConfigurationManager.AppSettings["CoreEnvironmentID"]);
        //protected int intXP = Int32.Parse(ConfigurationManager.AppSettings["OS_XP"]);
        protected Pages oPage;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected Applications oApplication;
        protected Locations oLocation;
        protected Classes oClass;
        protected Forecast oForecast;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected OperatingSystems oOperatingSystems;
        protected VirtualHDD oVirtualHDD;
        protected VirtualRam oVirtualRam;
        protected VirtualCPU oVirtualCPU;
        protected Variables oVariable;
        protected Functions oFunction;
        protected Users oUser;
        protected Domains oDomain;
        protected Workstations oWorkstation;
        protected CostCenter oCostCenter;
        protected int intAnswer = 0;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected int intMaxWorkstationsPerDay = Int32.Parse(ConfigurationManager.AppSettings["VMwareWorkstationsMax"]);
        protected string strMenuTab1 = "";
        protected int intWorkstation = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oOperatingSystems = new OperatingSystems(intProfile, dsn);
            oVirtualHDD = new VirtualHDD(intProfile, dsn);
            oVirtualRam = new VirtualRam(intProfile, dsn);
            oVirtualCPU = new VirtualCPU(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(0, dsn, intEnvironment);
            oUser = new Users(intProfile, dsn);
            oDomain = new Domains(intProfile, dsn);
            oWorkstation = new Workstations(intProfile, dsn);
            oCostCenter = new CostCenter(intProfile, dsn);

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rid"] != "" && Request.QueryString["rid"] != null)
            {
                if (!IsPostBack)
                    LoadLists();
                LoadValues();
                int intItem = Int32.Parse(lblItem.Text);
                int intApp = oRequestItem.GetItemApplication(intItem);
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable != "")
                {
                    panDeliverable.Visible = true;
                    btnDeliverable.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','" + strDeliverable + "');");
                }
                if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                    Int32.TryParse(Request.QueryString["id"], out intWorkstation);
                if (intWorkstation > 0)
                    panAccounts.Visible = true;
                else
                    panAccountsNo.Visible = true;
                if (Request.QueryString["userid"] != null && Request.QueryString["userid"] != "")
                {
                    int intUser = Int32.Parse(Request.QueryString["userid"]);
                    trAccountUpdate.Visible = true;
                    lblXID.Text = oUser.GetFullName(intUser) + " (" + oUser.GetName(intUser) + ")";
                    btnAddAccount.Text = "Update";
                    btnCancelAccount.Visible = true;
                }
                else
                    trNew.Visible = true;

                if (Request.QueryString["accts"] != null && Request.QueryString["accts"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "accts", "<script type=\"text/javascript\">alert('Enter at least one account for this workstation');<" + "/" + "script>");
                if (Request.QueryString["qty"] != null && Request.QueryString["qty"] != "")
                {
                    if (Request.QueryString["qty"] == intMaxWorkstationsPerDay.ToString())
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "qty", "<script type=\"text/javascript\">alert('NOTE: You can request up to " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day.\\n\\nCurrently, you have requested " + Request.QueryString["qty"] + " virtual workstations and cannot be allocated additional hardware until tomorrow.\\n\\nIf your initiative requires more than " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day, you must use design builder.\\nPlease contact your technical lead or ClearView administrator for additional information.');<" + "/" + "script>");
                    else if (Request.QueryString["qty"] == "0")
                    {
                        int intDiff = intMaxWorkstationsPerDay - Int32.Parse(Request.QueryString["qty"]);
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "qty", "<script type=\"text/javascript\">alert('NOTE: You can request up to " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day.\\n\\nCurrently, you have requested " + Request.QueryString["qty"] + " virtual workstations. Please enter a quantity of " + intDiff.ToString() + " or less to continue.\\n\\nIf your initiative requires more than " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day, you must use design builder.\\nPlease contact your technical lead or ClearView administrator for additional information.');<" + "/" + "script>");
                    }
                    else
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "qty", "<script type=\"text/javascript\">alert('NOTE: You can request up to " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day.\\n\\nPlease enter a quantity of " + intMaxWorkstationsPerDay.ToString() + " or less to continue.\\n\\nIf your initiative requires more than " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day, you must use design builder.\\nPlease contact your technical lead or ClearView administrator for additional information.');<" + "/" + "script>");
                }
                else if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                {
                    intAnswer = Int32.Parse(Request.QueryString["aid"]);
                    if (!IsPostBack)
                    {
                        DataSet dsAnswer = oForecast.GetAnswer(intAnswer);
                        if (dsAnswer.Tables[0].Rows.Count > 0)
                        {
                            txtName.Text = dsAnswer.Tables[0].Rows[0]["name"].ToString();
                            ddlLocation.SelectedValue = dsAnswer.Tables[0].Rows[0]["addressid"].ToString();
                            ddlClass.SelectedValue = dsAnswer.Tables[0].Rows[0]["classid"].ToString();
                            txtQuantity.Text = dsAnswer.Tables[0].Rows[0]["quantity"].ToString();
                            dsAnswer = oForecast.GetWorkstation(intAnswer);
                            if (dsAnswer.Tables[0].Rows.Count > 0)
                            {
                                ddlRam.SelectedValue = dsAnswer.Tables[0].Rows[0]["ramid"].ToString();
                                ddlOS.SelectedValue = dsAnswer.Tables[0].Rows[0]["osid"].ToString();
                                chkDR.Checked = (dsAnswer.Tables[0].Rows[0]["recovery"].ToString() == "1");
                                radEmployee.SelectedValue = dsAnswer.Tables[0].Rows[0]["internal"].ToString();
                                ddlHardDrive.SelectedValue = dsAnswer.Tables[0].Rows[0]["hddid"].ToString();
                                ddlCPU.SelectedValue = dsAnswer.Tables[0].Rows[0]["cpuid"].ToString();
                            }
                        }
                        else
                            intAnswer = 0;
                    }
                }
                
                if (intAnswer == 0)
                {
                    DataSet dsService = oForecast.GetAnswerService(Int32.Parse(Request.QueryString["rid"]));
                    if (dsService.Tables[0].Rows.Count > 0)
                        Redirect("&aid=" + dsService.Tables[0].Rows[0]["id"].ToString() + "&menu_tab=2");
                }
                if (intWorkstation > 0)
                {
                    DataSet dsWorkstation = oWorkstation.GetVirtual(intWorkstation);
                    if (dsWorkstation.Tables[0].Rows.Count > 0)
                    {
                        // Load Workstation
                        txtName.Text = dsWorkstation.Tables[0].Rows[0]["nickname"].ToString();
                        ddlLocation.SelectedValue = dsWorkstation.Tables[0].Rows[0]["addressid"].ToString();
                        int intClass = Int32.Parse(dsWorkstation.Tables[0].Rows[0]["classid"].ToString());
                        ddlClass.SelectedValue = intClass.ToString();
                        txtQuantity.Text = dsWorkstation.Tables[0].Rows[0]["quantity"].ToString();
                        int intOS = Int32.Parse(dsWorkstation.Tables[0].Rows[0]["osid"].ToString());
                        LoadOS(intOS);
                        ddlOS.SelectedValue = intOS.ToString();
                        int intDomain = Int32.Parse(dsWorkstation.Tables[0].Rows[0]["domainid"].ToString());
                        lblDomain.Text = oDomain.Get(intDomain, "name");
                        radEmployee.SelectedValue = dsWorkstation.Tables[0].Rows[0]["internal"].ToString();
                        chkDR.Checked = (dsWorkstation.Tables[0].Rows[0]["recovery"].ToString() == "1");
                        ddlRam.SelectedValue = dsWorkstation.Tables[0].Rows[0]["ramid"].ToString();
                        ddlHardDrive.SelectedValue = dsWorkstation.Tables[0].Rows[0]["hddid"].ToString();
                        ddlCPU.SelectedValue = dsWorkstation.Tables[0].Rows[0]["cpuid"].ToString();
                        int intManager = Int32.Parse(dsWorkstation.Tables[0].Rows[0]["appcontact"].ToString());
                        txtManager.Text = oUser.GetFullName(intManager);
                        hdnManager.Value = intManager.ToString();
                        int intCost = Int32.Parse(dsWorkstation.Tables[0].Rows[0]["costcenterid"].ToString());
                        txtCostCenter.Text = oCostCenter.GetName(intCost);
                        hdnCostCenter.Value = intCost.ToString();

                        // Load Accounts
                        txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                        lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
                        chkAdmin.Attributes.Add("onclick", "CheckAdmin(this);");
                        rptAccounts.DataSource = oWorkstation.GetAccountsVMware(intWorkstation);
                        rptAccounts.DataBind();
                        foreach (RepeaterItem ri in rptAccounts.Items)
                        {
                            LinkButton _delete = (LinkButton)ri.FindControl("btnDeleteAccount");
                            _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this account?') && LoadWait();");
                        }
                        if (rptAccounts.Items.Count == 0)
                        {
                            lblNone.Visible = true;
                            btnNext.Attributes.Add("onclick", "alert('Enter at least one account for this workstation');return false;");
                        }
                        else
                            btnNext.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                        if (oClass.IsProd(intClass))
                            panProduction.Visible = true;
                        else
                            panAdmin.Visible = true;
                        btnAddAccount.Attributes.Add("onclick", "return ValidateHidden('" + hdnUser.ClientID + "','" + txtUser.ClientID + "','Please enter a username, first name or last name') && ProcessButton(this) && LoadWait();");
                        
                        trUpdate.Visible = true;
                        btnUpdate.Attributes.Add("onclick", "return ValidateDropDown('" + ddlClass.ClientID + "','Please select a class')" +
                            " && ValidateNumber0('" + txtQuantity.ClientID + "','Please enter a valid quantity')" +
                            " && ValidateDropDown('" + ddlOS.ClientID + "','Please select an Operating System')" +
                            " && ValidateDropDown('" + ddlRam.ClientID + "','Please select a RAM')" +
                            " && ValidateDropDown('" + ddlCPU.ClientID + "','Please select a CPU')" +
                            " && ValidateDropDown('" + ddlHardDrive.ClientID + "','Please select a hard drive')" +
                            " && ValidateHidden0('" + hdnManager.ClientID + "','" + txtManager.ClientID + "','Please enter the LAN ID of your user access administrator')" +
                            " && ValidateHidden0('" + hdnCostCenter.ClientID + "','" + txtCostCenter.ClientID + "','Please enter the cost center to be billed for this workstation')" +
                            " && ProcessButton(this) && LoadWait()" +
                            ";");
                    }
                    else
                        intWorkstation = 0;
                }

                if (intWorkstation == 0)
                {
                    btnNext.Attributes.Add("onclick", "return ValidateDropDown('" + ddlClass.ClientID + "','Please select a class')" +
                        " && ValidateNumber0('" + txtQuantity.ClientID + "','Please enter a valid quantity')" +
                        " && ValidateDropDown('" + ddlOS.ClientID + "','Please select an Operating System')" +
                        " && ValidateDropDown('" + ddlRam.ClientID + "','Please select a RAM')" +
                        " && ValidateDropDown('" + ddlCPU.ClientID + "','Please select a CPU')" +
                        " && ValidateDropDown('" + ddlHardDrive.ClientID + "','Please select a hard drive')" +
                        " && ValidateHidden0('" + hdnManager.ClientID + "','" + txtManager.ClientID + "','Please enter the LAN ID of your user access administrator')" +
                        " && ValidateHidden0('" + hdnCostCenter.ClientID + "','" + txtCostCenter.ClientID + "','Please enter the cost center to be billed for this workstation')" +
                        " && ProcessButton(this) && LoadWait()" +
                        ";");
                }
            }
            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
            Tab oTab = new Tab("", intMenuTab, "divMenu1", true, false);
            oTab.AddTab("Workstation Details", "");
            oTab.AddTab("Account Configuration", "");
            strMenuTab1 = oTab.GetTabs();

            btnCancel1.Attributes.Add("onclick", "return confirm('Are you sure you want to cancel this service request?');");
            ddlOS.Attributes.Add("onchange", "LoadWait();");
            ddlRam.Attributes.Add("onchange", "LoadWait();");
            ddlCPU.Attributes.Add("onchange", "LoadWait();");
            ddlHardDrive.Attributes.Add("onchange", "LoadWait();");
            txtManager.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divManager.ClientID + "','" + lstManager.ClientID + "','" + hdnManager.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstManager.Attributes.Add("ondblclick", "AJAXClickRow();");
            btnManager.Attributes.Add("onclick", "return OpenWindow('NEW_USER','');");
            btnManager2.Attributes.Add("onclick", "return OpenWindow('NEW_USER','');");
            txtCostCenter.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'200','195','" + divCostCenter.ClientID + "','" + lstCostCenter.ClientID + "','" + hdnCostCenter.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_cost_centers.aspx',5);");
            lstCostCenter.Attributes.Add("ondblclick", "AJAXClickRow();");
        }
        private void LoadLists()
        {
            ddlClass.DataTextField = "name";
            ddlClass.DataValueField = "id";
            ddlClass.DataSource = oClass.GetWorkstationVMwares(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            //ddlOS.DataTextField = "name";
            //ddlOS.DataValueField = "id";
            //ddlOS.DataSource = oOperatingSystems.Gets(1, 1);
            //ddlOS.DataBind();
            ddlOS.Items.Insert(0, new ListItem("Windows 7", "44"));
            ddlOS.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            radEmployee.SelectedValue = "0";
        }
        protected void ddlOS_Change(Object Sender, EventArgs e)
        {
            LoadOS(Int32.Parse(ddlOS.SelectedItem.Value));
        }
        private void LoadOS(int _osid)
        {
            ddlRam.Items.Clear();
            ddlCPU.Items.Clear();
            ddlHardDrive.Items.Clear();

            if (_osid > 0)
            {
                bool boolXP = (oOperatingSystems.Get(_osid, "code") == "XP");
                bool boolWin7 = (oOperatingSystems.Get(_osid, "code") == "7");

                DataSet dsRam = oVirtualRam.GetVMwares((boolXP ? 1 : 0), (boolWin7 ? 1 : 0), 1);
                ddlRam.Enabled = true;
                ddlRam.DataTextField = "name";
                ddlRam.DataValueField = "id";
                ddlRam.DataSource = dsRam;
                ddlRam.DataBind();
                ddlRam.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                foreach (DataRow drRam in dsRam.Tables[0].Rows)
                {
                    if (drRam["default"].ToString() == "1")
                    {
                        foreach (ListItem li in ddlRam.Items)
                        {
                            if (li.Value == drRam["id"].ToString())
                            {
                                li.Selected = true;
                                break;
                            }
                        }
                        break;
                    }
                }
                DataSet dsCPU = oVirtualCPU.GetVMwares((boolXP ? 1 : 0), (boolWin7 ? 1 : 0), 1);
                ddlCPU.Enabled = true;
                ddlCPU.DataTextField = "name";
                ddlCPU.DataValueField = "id";
                ddlCPU.DataSource = dsCPU;
                ddlCPU.DataBind();
                ddlCPU.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                foreach (DataRow drCPU in dsCPU.Tables[0].Rows)
                {
                    if (drCPU["default"].ToString() == "1")
                    {
                        foreach (ListItem li in ddlCPU.Items)
                        {
                            if (li.Value == drCPU["id"].ToString())
                            {
                                li.Selected = true;
                                break;
                            }
                        }
                        break;
                    }
                }
                DataSet dsHardDrive = oVirtualHDD.GetVMwares((boolXP ? 1 : 0), (boolWin7 ? 1 : 0), 1);
                ddlHardDrive.Enabled = true;
                ddlHardDrive.DataTextField = "name";
                ddlHardDrive.DataValueField = "id";
                ddlHardDrive.DataSource = dsHardDrive;
                ddlHardDrive.DataBind();
                ddlHardDrive.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                foreach (DataRow drHardDrive in dsHardDrive.Tables[0].Rows)
                {
                    if (drHardDrive["default"].ToString() == "1")
                    {
                        foreach (ListItem li in ddlHardDrive.Items)
                        {
                            if (li.Value == drHardDrive["id"].ToString())
                            {
                                li.Selected = true;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            else
            {
                ddlRam.Enabled = false;
                ddlRam.Items.Insert(0, new ListItem("-- Select an Operating System --", "0"));
                ddlCPU.Enabled = false;
                ddlCPU.Items.Insert(0, new ListItem("-- Select an Operating System --", "0"));
                ddlHardDrive.Enabled = false;
                ddlHardDrive.Items.Insert(0, new ListItem("-- Select an Operating System --", "0"));
            }
        }
        protected void ddlRam_Change(Object Sender, EventArgs e)
        {
            int intID = Int32.Parse(ddlRam.SelectedItem.Value);
            if (oVirtualRam.Get(intID, "prompt") != "")
            {
                trRam.Visible = true;
                lblRam.Text = oVirtualRam.Get(intID, "prompt");
            }
            else
                trRam.Visible = false;
        }
        protected void ddlCPU_Change(Object Sender, EventArgs e)
        {
            int intID = Int32.Parse(ddlCPU.SelectedItem.Value);
            if (oVirtualCPU.Get(intID, "prompt") != "")
            {
                trCPU.Visible = true;
                lblCPU.Text = oVirtualCPU.Get(intID, "prompt");
            }
            else
                trCPU.Visible = false;
        }
        protected void ddlHardDrive_Change(Object Sender, EventArgs e)
        {
            int intID = Int32.Parse(ddlHardDrive.SelectedItem.Value);
            if (oVirtualHDD.Get(intID, "prompt") != "")
            {
                trHardDrive.Visible = true;
                lblHardDrive.Text = oVirtualHDD.Get(intID, "prompt");
            }
            else
                trHardDrive.Visible = false;
        }
        private void LoadValues()
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            DataSet dsItems;
            int intForm = 0;
            if (Request.QueryString["formid"] != null && Request.QueryString["formid"] != "")
            {
                intForm = Int32.Parse(Request.QueryString["formid"]);
                dsItems = oRequestItem.GetForm(intRequest, intForm);
            }
            else
                dsItems = oRequestItem.GetForms(intRequest);
            if (dsItems.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drItem in dsItems.Tables[0].Rows)
                {
                    if (drItem["done"].ToString() == "-1" || intForm > 0)
                    {
                        lblItem.Text = drItem["itemid"].ToString();
                        if (intForm > 0 && Request.QueryString["num"] != null && Request.QueryString["num"] != "")
                            lblNumber.Text = Request.QueryString["num"];
                        else
                            lblNumber.Text = drItem["number"].ToString();
                        lblService.Text = drItem["serviceid"].ToString();
                        break;
                    }
                }
            }
        }
        protected void btnAddAccount_Click(Object Sender, EventArgs e)
        {
            if (Request.QueryString["userid"] != null && Request.QueryString["userid"] != "")
            {
                lblError.Text = "";
                int intUser = Int32.Parse(Request.QueryString["userid"]);
                AD oAD = new AD(0, dsn, (int)CurrentEnvironment.CORPDMN);
                string strXID = txtXID.Text.Trim().ToUpper();
                SearchResultCollection oResults = oAD.Search(strXID, "sAMAccountName");
                if (oResults.Count == 1)
                {
                    SearchResult oResult = oResults[0];
                    if (oResult.Properties.Contains("sAMAccountName") == true)
                        strXID = oResult.GetDirectoryEntry().Properties["sAMAccountName"].Value.ToString();
                    if (oResult.Properties.Contains("extensionattribute10") == true)
                    {
                        string strID = oUser.GetName(intUser, false);
                        string strPNCID = oResult.GetDirectoryEntry().Properties["extensionattribute10"].Value.ToString();
                        if (strID.ToUpper().Trim() == strPNCID.Trim().ToUpper())
                        {
                            oUser.Update(intUser, strXID, strID.ToUpper());
                            int intAdmin = Int32.Parse(Request.QueryString["admin"]);
                            if (panAdmin.Visible == false)
                                intAdmin = 0;
                            oWorkstation.AddAccountFix(0, intWorkstation, intUser, intAdmin, Int32.Parse(Request.QueryString["remote"]));
                            Redirect("&menu_tab=2&add=true");
                        }
                        else
                            lblError.Text = "<p>The X-ID you entered (" + strXID + ") has a different PNC ID configured (" + strPNCID + ").</p><p>Please try again or contact your clearview administrator.</p>";
                    }
                    else
                        lblError.Text = "<p>The X-ID you entered (" + strXID + ") does not have a PNC ID attribute configured.</p><p>Please try again or contact your clearview administrator.</p>";
                }
                else if (oResults.Count > 1)
                {
                    lblError.Text = "There were " + oResults.Count.ToString() + " accounts found in CORPDMN for the account " + txtXID.Text + ". Please try again.";
                }
                else
                {
                    lblError.Text = "Could not find that X-ID in CORPDMN. Please try again.";
                }
            }
            else
            {
                int intUser = Int32.Parse(Request.Form[hdnUser.UniqueID]);
                bool boolOffshore = false;
                string strID = oUser.GetName(intUser, false);
                string strXID = oUser.GetName(intUser, true);
                if (Int32.Parse(radEmployee.SelectedItem.Value) == 1 || strID.ToUpper().Trim().StartsWith("XX") == true)
                    boolOffshore = true;
                else if (strXID.ToUpper().Trim().StartsWith("XX") == true)
                    boolOffshore = true;
                if (boolOffshore == true)
                {
                    if (strXID == "" || strXID == strID)
                    {
                        boolOffshore = false;
                        // Get X-ID (since it is needed for the INI file and AD setup)
                        SearchResultCollection oCollection = oFunction.eDirectory(strXID);
                        if (oCollection.Count == 1 && oCollection[0].GetDirectoryEntry().Properties.Contains("businesscategory") == true)
                        {
                            boolOffshore = true;
                            strXID = oCollection[0].GetDirectoryEntry().Properties["businesscategory"].Value.ToString();
                            oUser.Update(intUser, strXID, strID);
                        }
                        else
                        {
                            oCollection = oFunction.eDirectory("businesscategory", strXID);
                            if (oCollection.Count == 1)
                            {
                                boolOffshore = true;
                                strID = oCollection[0].GetDirectoryEntry().Properties["cn"].Value.ToString();
                                oUser.Update(intUser, strXID, strID);
                            }
                            else
                                Redirect("&aid=" + intAnswer.ToString() + "&menu_tab=2&userid=" + intUser.ToString() + "&admin=" + (chkAdmin.Checked ? "1" : "0") + "&remote=" + (chkRemote.Checked ? "1" : "0"));
                        }
                    }
                    if (boolOffshore == true)
                    {
                        oWorkstation.AddAccountFix(0, intWorkstation, intUser, (chkAdmin.Checked ? 1 : 0), (chkRemote.Checked ? 1 : 0));
                        Redirect("&aid=" + intAnswer.ToString() + "&menu_tab=2&add=true");
                    }
                }
                else
                {
                    // Try to automatically get XX-ID from X-ID
                    bool boolFound = false;
                    AD oAD = new AD(0, dsn, (int)CurrentEnvironment.CORPDMN);
                    SearchResultCollection oResults = oAD.Search(strXID, "sAMAccountName");
                    if (oResults.Count == 1)
                    {
                        SearchResult oResult = oResults[0];
                        if (oResult.Properties.Contains("extensionattribute10") == true)
                        {
                            string strPNCID = oResult.GetDirectoryEntry().Properties["extensionattribute10"].Value.ToString();
                            if (strPNCID.ToUpper().Trim().StartsWith("XX") == true)
                            {
                                boolFound = true;
                                oUser.Update(intUser, strXID, strPNCID.ToUpper());
                                oWorkstation.AddAccountFix(0, intWorkstation, intUser, (chkAdmin.Checked ? 1 : 0), (chkRemote.Checked ? 1 : 0));
                                Redirect("&aid=" + intAnswer.ToString() + "&menu_tab=2&add=true");
                            }
                        }
                    }
                    if (boolFound == false)
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "offshore", "<script type=\"text/javascript\">alert('This service is only available for OFFSHORE users.\\n\\nThe account you entered is not an offshore user (does not start with XX).\\n\\nPlease enter a valid offshore user account.');<" + "/" + "script>");
                }
            }
        }

        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            Update();
            Redirect("&menu_tab=2");
        }

        protected void btnCancelAccount_Click(Object Sender, EventArgs e)
        {
            Redirect("&menu_tab=2");
        }

        protected void btnDeleteAccount_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oWorkstation.DeleteAccount(Int32.Parse(oDelete.CommandArgument));
            Redirect("&menu_tab=2&delete=true");
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            oRequestItem.UpdateForm(intRequest, false);
            Redirect("");
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            int intItem = Int32.Parse(lblItem.Text);
            int intNumber = Int32.Parse(lblNumber.Text);
            DataSet dsToday = oForecast.GetAnswersDay(intWorkstationPlatform, intProfile, 1);
            int intQuantity = 0;
            foreach (DataRow drToday in dsToday.Tables[0].Rows)
                intQuantity += Int32.Parse(drToday["quantity"].ToString());
            int intTotal = Int32.Parse(txtQuantity.Text) + intQuantity;
            if (intTotal <= intMaxWorkstationsPerDay)
            {
                if (intWorkstation == 0)
                {
                    // Add Answer
                    if (intAnswer == 0)
                    {
                        intAnswer = oForecast.AddAnswer(0, intWorkstationPlatform, 0, intProfile);
                        oForecast.UpdateAnswerStep(intAnswer, 1, 1);
                    }
                    oForecast.UpdateAnswerService(intAnswer, intRequest);
                    int intClass = Int32.Parse(ddlClass.SelectedItem.Value);
                    oForecast.UpdateAnswer(intAnswer, 0, 0, "", 0, Request.ServerVariables["REMOTE_HOST"], "", txtName.Text, Int32.Parse(ddlLocation.SelectedItem.Value), intClass, 0, intCore, 0, 0, 0, Int32.Parse(txtQuantity.Text), 0);
                    // Add Model
                    oForecast.UpdateAnswerModel(intAnswer, intModelVMware);
                    oForecast.DeleteWorkstation(intAnswer);
                    oForecast.AddWorkstation(intAnswer, Int32.Parse(ddlRam.SelectedItem.Value), Int32.Parse(ddlOS.SelectedItem.Value), (chkDR.Checked ? 1 : 0), Int32.Parse(radEmployee.SelectedItem.Value), Int32.Parse(ddlHardDrive.SelectedItem.Value), Int32.Parse(ddlCPU.SelectedItem.Value));
                    oForecast.UpdateAnswerStep(intAnswer, 1, 1);
                    // Add Commitment Date
                    oForecast.UpdateAnswer(intAnswer, DateTime.Today, intConfidence, intProfile);
                    oForecast.UpdateAnswerStep(intAnswer, 1, 1);
                    // Manager and Cost Center
                    int intCostCenter = 0;
                    Int32.TryParse(Request.Form[hdnCostCenter.UniqueID], out intCostCenter);
                    oForecast.UpdateAnswer(intAnswer, "", "", 0, intCostCenter, 0, Int32.Parse(Request.Form[hdnManager.UniqueID]), 0, 0, 0, 0, 0, 0);
                    // Create Workstation Record
                    int intOS = Int32.Parse(ddlOS.SelectedItem.Value);
                    int intSP = 0;
                    Int32.TryParse(oOperatingSystems.Get(intOS, "default_sp"), out intSP);
                    DataSet dsDomains = oDomain.GetClassEnvironment(intClass, intCore);
                    int intDomain = 0;
                    if (dsDomains.Tables[0].Rows.Count > 0)
                        intDomain = Int32.Parse(dsDomains.Tables[0].Rows[0]["id"].ToString());
                    intWorkstation = oWorkstation.AddVirtual(intRequest, intAnswer, intNumber, 1, intModelVMware, intOS, intSP, intDomain, Int32.Parse(ddlRam.SelectedItem.Value), (chkDR.Checked ? 1 : 0), Int32.Parse(radEmployee.SelectedItem.Value), Int32.Parse(ddlHardDrive.SelectedItem.Value), Int32.Parse(ddlCPU.SelectedItem.Value), 1, 1);
                    Redirect("&id=" + intWorkstation.ToString() + "&menu_tab=2");
                }
                else
                {
                    Update();
                    if (rptAccounts.Items.Count > 0)
                    {
                        oRequestItem.UpdateForm(intRequest, true);
                        Redirect("");
                    }
                    else
                        Redirect("&menu_tab=2&accts=0");
                }
            }
            else
                Redirect("&qty=" + intQuantity.ToString());
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            oRequest.Cancel(intRequest);
            Response.Redirect(oPage.GetFullLink(intPage));
        }
        protected void Redirect(string _additional)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + (string.IsNullOrEmpty(Request.QueryString["id"]) || _additional.Contains("&id=") ? "" : "&id=" + Request.QueryString["id"]) + _additional);
        }
        protected void Update()
        {
            int intClass = Int32.Parse(ddlClass.SelectedItem.Value);
            // Manager and Cost Center
            int intCostCenter = 0;
            Int32.TryParse(Request.Form[hdnCostCenter.UniqueID], out intCostCenter);
            oForecast.UpdateAnswer(intAnswer, "", "", 0, intCostCenter, 0, Int32.Parse(Request.Form[hdnManager.UniqueID]), 0, 0, 0, 0, 0, 0);
            int intOS = Int32.Parse(ddlOS.SelectedItem.Value);
            int intSP = 0;
            Int32.TryParse(oOperatingSystems.Get(intOS, "default_sp"), out intSP);
            DataSet dsDomains = oDomain.GetClassEnvironment(intClass, intCore);
            int intDomain = 0;
            if (dsDomains.Tables[0].Rows.Count > 0)
                intDomain = Int32.Parse(dsDomains.Tables[0].Rows[0]["id"].ToString());
            oWorkstation.UpdateVirtual(intWorkstation, intOS, intSP, intDomain, Int32.Parse(ddlRam.SelectedItem.Value), (chkDR.Checked ? 1 : 0), Int32.Parse(radEmployee.SelectedItem.Value), Int32.Parse(ddlHardDrive.SelectedItem.Value), Int32.Parse(ddlCPU.SelectedItem.Value), 1);
        }
    }
}