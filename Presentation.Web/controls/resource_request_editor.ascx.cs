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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class resource_request_editor : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strWM = ConfigurationManager.AppSettings["GENERIC_WM"];
        protected string strCP = ConfigurationManager.AppSettings["GENERIC_CP"];
        protected string strRR = ConfigurationManager.AppSettings["GENERIC_RR"];
        protected string strCA = ConfigurationManager.AppSettings["GENERIC_CA"];
        protected string strAdmins = ConfigurationManager.AppSettings["Administrators"];
        protected int intServiceType = Int32.Parse(ConfigurationManager.AppSettings["DefaultServiceType"]);
        protected int intProfile;
        protected Applications oApplication;
        protected Pages oPage;
        protected Users oUser;
        protected ServiceEditor oServiceEditor;
        protected RequestItems oRequestItem;
        protected Services oService;
        protected ServiceDetails oServiceDetail;
        protected Variables oVariable;
        protected Functions oFunction;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intService = 0;
        protected int intItem = 0;
        protected int intTaskCount = 0;
        protected string strEdit = "";
        protected string strServicesC = "";
        protected string strServicesI = "";
        protected string strForm = "";
        protected string strFormPreview = "";
        protected string strFormWM = "";
        //protected string strParent = "";
        protected string strCheckboxes = "";
        protected string strMenuTab1 = "";
        protected string strMenuTab2 = "";
        protected string strMenuTab3 = "";
        private string strEMailIdsBCC = "";
        protected string strIndent = "15";

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder(strEdit);
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oApplication = new Applications(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oServiceEditor = new ServiceEditor(intProfile, dsnServiceEditor);
            oRequestItem = new RequestItems(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceDetail = new ServiceDetails(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            oFunction.Alert(Page, "update", "Information Saved Successfully", "Update Complete");

            if (String.IsNullOrEmpty(Request.QueryString["new"]) == false)
            {
                string strNew = oFunction.decryptQueryString(Request.QueryString["new"]);
                if (String.IsNullOrEmpty(Request.QueryString["publish"]) == true)
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "publishing", "<script type=\"text/javascript\">Publish('" + oPage.GetFullLink(intPage) + "'," + strNew + ");<" + "/" + "script>");
                else
                {
                    // Confirmed to publish
                    int intNew = 0;
                    try
                    {
                        intNew = Int32.Parse(strNew);
                        WhatsNew oWhatsNew = new WhatsNew(intProfile, dsn);
                        oWhatsNew.Add("New ClearView Service Available: " + oService.GetName(intNew), oUser.GetFullName(intProfile) + " [" + oUser.GetName(intProfile) + "]" + " created the service <b>" + oService.GetName(intNew) + "</b>. This service is used for the following types of requests: " + oService.Get(intNew, "description"), "", "", "Service Editor", intProfile, 1);
                        Response.Redirect(oPage.GetFullLink(intPage));
                    }
                    catch
                    {
                        Response.Redirect(oPage.GetFullLink(intPage));
                    }
                }
            }

            lblTitle.Text = oPage.Get(intPage, "title");

            if (oUser.IsManager(oUser.GetName(intProfile), intEnvironment) == true || true)
            {
                panPermit.Visible = true;
                LoadGroups();
                LoadServices();
                if (strServicesC == "")
                    strServicesC = "<tr><td colspan=\"10\"><img src='/images/alert.gif' border='0' align='absmiddle'> You have not added any services</td></tr>";
                sb.Append("<table width=\"100%\" border=\"0\" cellpadding=\"4\" cellspacing=\"3\">");
                if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
                {
                    intService = Int32.Parse(Request.QueryString["sid"]);
                    if (intService == 0)
                    {
                        // New Service
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "creating", "<script type=\"text/javascript\">NewTab('divMenu1','" + hdnType.ClientID + "',this,3);<" + "/" + "script>");
                    }
                    else
                    {
                        intItem = oService.GetItemId(intService);
                        int intApp = oRequestItem.GetItemApplication(intItem);
                        bool boolPermit = false;
                        DataSet dsUsers = oService.GetUser(intService, -1);
                        foreach (DataRow drUser in dsUsers.Tables[0].Rows)
                        {
                            if (Int32.Parse(drUser["userid"].ToString()) == intProfile)
                            {
                                boolPermit = true;
                                break;
                            }
                        }
                        if (boolPermit == true)
                        {
                            if (!IsPostBack)
                            {
                                DataSet ds = oService.Get(intService);
                                DataSet dsItem = oRequestItem.GetItem(intItem);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    lblName2.Text = ds.Tables[0].Rows[0]["name"].ToString();
                                    lblName31.Text = lblName2.Text;
                                    lblName32.Text = lblName2.Text;
                                    lblName4.Text = lblName2.Text;
                                    lblName5.Text = lblName2.Text;
                                    lblName6.Text = lblName2.Text;
                                    lblName7.Text = lblName2.Text;
                                    lblName8.Text = lblName2.Text;
                                    int intStep = Int32.Parse(ds.Tables[0].Rows[0]["step"].ToString());
                                    int intEdit = 0;
                                    if (Request.QueryString["edit"] != null && Request.QueryString["edit"] != "")
                                        intEdit = Int32.Parse(Request.QueryString["edit"]);

                                    DataSet dsWorkflowReceive = oService.GetWorkflowsReceive(intService);

                                    // Step 1
                                    sb.Append("<tr>");
                                    if ((intEdit > 1 || (intStep > 1 && intEdit == 0)) && intEdit != 1 && (intStep != -1 || intEdit != 0))
                                    {
                                        sb.Append("<td nowrap><img src=\"/images/1on.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"greenlink\"><a class=\"greenlink\" href=\"");
                                        sb.Append(oPage.GetFullLink(intPage));
                                        sb.Append("?sid=");
                                        sb.Append(intService.ToString());
                                        sb.Append("&menu_tab=3&edit=1\">Create the Service</a></td>");
                                    }
                                    else if (intStep == 0 || (intStep == 1 && intEdit == 0) || intEdit == 1 || (intStep == -1 && intEdit == 0))
                                    {
                                        panStep1.Visible = true;
                                        panStep1Update.Visible = true;
                                        txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                                        txtDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                                        if (oApplication.Get(intApp, "request_items") == "1")
                                        {
                                            bool boolGroup = false;
                                            foreach (ListItem oList in lstGroup.Items)
                                            {
                                                if (oList.Value == intItem.ToString())
                                                {
                                                    boolGroup = true;
                                                    break;
                                                }
                                            }
                                            if (boolGroup == true)
                                            {
                                                panGroup.Visible = true;
                                                lstGroup.SelectedValue = intItem.ToString();
                                            }
                                        }
                                        if (ds.Tables[0].Rows[0]["show"].ToString() == "1")
                                            radEnabledYes.Checked = true;
                                        else
                                            radEnabledNo.Checked = true;
                                        if (ds.Tables[0].Rows[0]["project"].ToString() == "1")
                                            radProjectYes.Checked = true;
                                        else
                                        {
                                            radProjectNo.Checked = true;
                                            divProjectNo.Style["display"] = "inline";
                                            if (ds.Tables[0].Rows[0]["title_override"].ToString() == "1")
                                            {
                                                radTitleOverrideYes.Checked = true;
                                                divTitleOverrideYes.Style["display"] = "inline";
                                                txtTitleName.Text = ds.Tables[0].Rows[0]["title_name"].ToString();
                                            }
                                            else
                                                radTitleOverrideNo.Checked = true;
                                        }
                                        sb.Append("<td nowrap><img src=\"/images/1on.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"greenlink\"><b>Create the Service</b></td>");
                                    }
                                    sb.Append("</tr><tr>");
                                    // Step 2
                                    if ((intStep == 0 || intStep == -1 || intEdit > 2 || (intStep > 2 && intEdit == 0) || (intEdit < 2 && intEdit > 0 && (intStep == 0 || intStep >= 2))) && intEdit != 2)
                                    {
                                        sb.Append("<td nowrap><img src=\"/images/2on.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"greenlink\"><a class=\"greenlink\" href=\"");
                                        sb.Append(oPage.GetFullLink(intPage));
                                        sb.Append("?sid=");
                                        sb.Append(intService.ToString());
                                        sb.Append("&menu_tab=3&edit=2\">Configure the Service</a></td>");
                                    }
                                    else if ((intStep == 2 && intEdit == 0) || intEdit == 2)
                                    {
                                        panStep2.Visible = true;
                                        if (intStep == 2)
                                            panStep2Next.Visible = true;
                                        else
                                            panStep2Update.Visible = true;

                                        radRejectYes.Checked = (ds.Tables[0].Rows[0]["rejection"].ToString() == "1");
                                        radRejectNo.Checked = (ds.Tables[0].Rows[0]["rejection"].ToString() == "0");
                                        if (radRejectYes.Checked == false && radRejectNo.Checked == false)
                                            radRejectYes.Checked = true;
                                        radQuantityYes.Checked = (ds.Tables[0].Rows[0]["multiple_quantity"].ToString() == "1");
                                        radQuantityNo.Checked = (ds.Tables[0].Rows[0]["multiple_quantity"].ToString() == "0");
                                        if (radQuantityYes.Checked == false && radQuantityNo.Checked == false)
                                            radQuantityNo.Checked = true;
                                        radTasksYes.Checked = (ds.Tables[0].Rows[0]["tasks"].ToString() == "1");
                                        radTasksNo.Checked = (ds.Tables[0].Rows[0]["tasks"].ToString() == "0");
                                        if (radTasksYes.Checked == false && radTasksNo.Checked == false)
                                            radTasksYes.Checked = true;
                                        radAutomateYes.Checked = (ds.Tables[0].Rows[0]["can_automate"].ToString() == "1");
                                        radAutomateNo.Checked = (ds.Tables[0].Rows[0]["can_automate"].ToString() == "0");
                                        if (radAutomateYes.Checked == false && radAutomateNo.Checked == false)
                                            radAutomateNo.Checked = true;
                                        radStatementYes.Checked = (ds.Tables[0].Rows[0]["statement"].ToString() == "1");
                                        radStatementNo.Checked = (ds.Tables[0].Rows[0]["statement"].ToString() == "0");
                                        if (radStatementYes.Checked == false && radStatementNo.Checked == false)
                                            radStatementNo.Checked = true;
                                        radUploadYes.Checked = (ds.Tables[0].Rows[0]["upload"].ToString() == "1");
                                        radUploadNo.Checked = (ds.Tables[0].Rows[0]["upload"].ToString() == "0");
                                        if (radUploadYes.Checked == false && radUploadNo.Checked == false)
                                            radUploadNo.Checked = true;
                                        radExpediteYes.Checked = (ds.Tables[0].Rows[0]["expedite"].ToString() == "1");
                                        radExpediteNo.Checked = (ds.Tables[0].Rows[0]["expedite"].ToString() == "0");
                                        if (radExpediteYes.Checked == false && radExpediteNo.Checked == false)
                                            radExpediteYes.Checked = true;
                                        radNotifyRedYes.Checked = (ds.Tables[0].Rows[0]["notify_red"].ToString() == "1");
                                        radNotifyRedNo.Checked = (ds.Tables[0].Rows[0]["notify_red"].ToString() == "0");
                                        if (radNotifyRedYes.Checked == false && radNotifyRedNo.Checked == false)
                                            radNotifyRedYes.Checked = true;
                                        radNotifyYellowYes.Checked = (ds.Tables[0].Rows[0]["notify_yellow"].ToString() == "1");
                                        radNotifyYellowNo.Checked = (ds.Tables[0].Rows[0]["notify_yellow"].ToString() == "0");
                                        if (radNotifyYellowYes.Checked == false && radNotifyYellowNo.Checked == false)
                                            radNotifyYellowYes.Checked = true;
                                        radNotifyGreenYes.Checked = (ds.Tables[0].Rows[0]["notify_green"].ToString() == "1");
                                        radNotifyGreenNo.Checked = (ds.Tables[0].Rows[0]["notify_green"].ToString() == "0");
                                        if (radNotifyGreenYes.Checked == false && radNotifyGreenNo.Checked == false)
                                            radNotifyGreenYes.Checked = true;
                                        radManagerApproveYes.Checked = (ds.Tables[0].Rows[0]["manager_approval"].ToString() == "1");
                                        radManagerApproveNo.Checked = (ds.Tables[0].Rows[0]["manager_approval"].ToString() == "0");
                                        txtNotifyComplete.Text = ds.Tables[0].Rows[0]["notify_complete"].ToString();
                                        if (radManagerApproveYes.Checked == false && radManagerApproveNo.Checked == false)
                                            radManagerApproveNo.Checked = true;

                                        sb.Append("<td nowrap><img src=\"/images/2on.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"greenlink\"><b>Configure the Service</b></td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td nowrap><img src=\"/images/2off.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"calendarother\">Configure the Service</td>");
                                    }
                                    sb.Append("</tr><tr>");
                                    // Step 3
                                    if ((intStep == 0 || intStep == -1 || intEdit > 3 || (intStep > 3 && intEdit == 0) || (intEdit < 3 && intEdit > 0 && (intStep == 0 || intStep >= 3))) && intEdit != 3)
                                    {
                                        sb.Append("<td nowrap><img src=\"/images/3on.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"greenlink\"><a class=\"greenlink\" href=\"");
                                        sb.Append(oPage.GetFullLink(intPage));
                                        sb.Append("?sid=");
                                        sb.Append(intService.ToString());
                                        sb.Append("&menu_tab=3&edit=3\">Configure the Hours</a></td>");
                                    }
                                    else if ((intStep == 3 && intEdit == 0) || intEdit == 3)
                                    {
                                        panStep3.Visible = true;
                                        if (intStep == 3)
                                            panStep3Next.Visible = true;
                                        else
                                            panStep3Update.Visible = true;

                                        if (ds.Tables[0].Rows[0]["tasks"].ToString() == "1")
                                        {
                                            panTasksOn.Visible = true;
                                            LoadDetails(intService);
                                            if (intTaskCount == 0)
                                            {
                                                btnNext3.Attributes.Add("onclick", "alert('Please add at least one (1) task for this service');return false;");
                                                btnUpdate3.Attributes.Add("onclick", "alert('Please add at least one (1) task for this service');return false;");
                                            }
                                            else
                                            {
                                                btnNext3.Attributes.Add("onclick", "return ValidateNumber0Warning('" + txtSLA.ClientID + "','Please enter a valid number of hours','WARNING: You are attempting to save a service with an SLA of zero (0.00) hours.\\n\\nIt is highly recommended that you enter a valid number of hours for the SLA of this service.\\n\\nAre you sure you want to continue?')" +
                                                    " && ValidateRadioButtons('" + radSLAHideYes.ClientID + "','" + radSLAHideNo.ClientID + "','Please select if you want to hide the SLA from the service request summary')" +
                                                    " && ProcessButtons(this,'" + btnBack3.ClientID + "') && LoadWait()" +
                                                    ";");
                                                btnUpdate3.Attributes.Add("onclick", "return ValidateNumber0Warning('" + txtSLA.ClientID + "','Please enter a valid number of hours','WARNING: You are attempting to save a service with an SLA of zero (0.00) hours.\\n\\nIt is highly recommended that you enter a valid number of hours for the SLA of this service.\\n\\nAre you sure you want to continue?')" +
                                                    " && ValidateRadioButtons('" + radSLAHideYes.ClientID + "','" + radSLAHideNo.ClientID + "','Please select if you want to hide the SLA from the service request summary')" +
                                                    " && ProcessButtons(this,'" + btnCancel3.ClientID + "') && LoadWait()" +
                                                    ";");
                                            }
                                        }
                                        else
                                        {
                                            panTasksOff.Visible = true;
                                            txtHours.Text = double.Parse(ds.Tables[0].Rows[0]["hours"].ToString()).ToString("F");
                                            btnNext3.Attributes.Add("onclick", "return ValidateNumber0('" + txtHours.ClientID + "','Please enter a valid number of hours')" +
                                                " && ValidateRadioButtons('" + radNoSliderYes.ClientID + "','" + radNoSliderNo.ClientID + "','Please select if you want to enable fast-completion')" +
                                                " && ValidateNumber0Warning('" + txtSLA.ClientID + "','Please enter a valid number of hours','WARNING: You are attempting to save a service with an SLA of zero (0.00) hours.\\n\\nIt is highly recommended that you enter a valid number of hours for the SLA of this service.\\n\\nAre you sure you want to continue?')" +
                                                " && ValidateRadioButtons('" + radSLAHideYes.ClientID + "','" + radSLAHideNo.ClientID + "','Please select if you want to hide the SLA from the service request summary')" +
                                                " && ProcessButtons(this,'" + btnBack3.ClientID + "') && LoadWait()" +
                                                ";");
                                            btnUpdate3.Attributes.Add("onclick", "return ValidateNumber0('" + txtHours.ClientID + "','Please enter a valid number of hours')" +
                                                " && ValidateRadioButtons('" + radNoSliderYes.ClientID + "','" + radNoSliderNo.ClientID + "','Please select if you want to enable fast-completion')" +
                                                " && ValidateNumber0Warning('" + txtSLA.ClientID + "','Please enter a valid number of hours','WARNING: You are attempting to save a service with an SLA of zero (0.00) hours.\\n\\nIt is highly recommended that you enter a valid number of hours for the SLA of this service.\\n\\nAre you sure you want to continue?')" +
                                                " && ValidateRadioButtons('" + radSLAHideYes.ClientID + "','" + radSLAHideNo.ClientID + "','Please select if you want to hide the SLA from the service request summary')" +
                                                " && ProcessButtons(this,'" + btnCancel3.ClientID + "') && LoadWait()" +
                                                ";");
                                        }
                                        txtSLA.Text = double.Parse(ds.Tables[0].Rows[0]["sla"].ToString()).ToString("F");
                                        radRejectYes.Checked = (ds.Tables[0].Rows[0]["rejection"].ToString() == "1");
                                        radNoSliderYes.Checked = (ds.Tables[0].Rows[0]["no_slider"].ToString() == "1");
                                        radNoSliderNo.Checked = (ds.Tables[0].Rows[0]["no_slider"].ToString() == "0");
                                        if (radNoSliderYes.Checked == false && radNoSliderNo.Checked == false)
                                            radNoSliderNo.Checked = true;
                                        radSLAHideYes.Checked = (ds.Tables[0].Rows[0]["hide_sla"].ToString() == "1");
                                        radSLAHideNo.Checked = (ds.Tables[0].Rows[0]["hide_sla"].ToString() == "0");
                                        if (radSLAHideYes.Checked == false && radSLAHideNo.Checked == false)
                                            radSLAHideNo.Checked = true;

                                        sb.Append("<td nowrap><img src=\"/images/3on.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"greenlink\"><b>Configure the Hours</b></td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td nowrap><img src=\"/images/3off.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"calendarother\">Configure the Hours</td>");
                                    }
                                    sb.Append("</tr><tr>");
                                    // Step 4
                                    if ((intStep == 0 || intStep == -1 || intEdit > 4 || (intStep > 4 && intEdit == 0) || (intEdit < 4 && intEdit > 0 && (intStep == 0 || intStep >= 4))) && intEdit != 4)
                                    {
                                        sb.Append("<td nowrap><img src=\"/images/4on.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"greenlink\"><a class=\"greenlink\" href=\"");
                                        sb.Append(oPage.GetFullLink(intPage));
                                        sb.Append("?sid=");
                                        sb.Append(intService.ToString());
                                        sb.Append("&menu_tab=3&edit=4\">Configure the Location</a></td>");
                                    }
                                    else if ((intStep == 4 && intEdit == 0) || intEdit == 4)
                                    {
                                        panStep4.Visible = true;
                                        if (intStep == 4)
                                            panStep4Next.Visible = true;
                                        else
                                            panStep4Update.Visible = true;

                                        LoadLocations();

                                        sb.Append("<td nowrap><img src=\"/images/4on.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"greenlink\"><b>Configure the Location</b></td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td nowrap><img src=\"/images/4off.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"calendarother\">Configure the Location</td>");
                                    }
                                    sb.Append("</tr><tr>");
                                    // Step 5
                                    if ((intStep == 0 || intStep == -1 || intEdit > 5 || (intStep > 5 && intEdit == 0) || (intEdit < 5 && intEdit > 0 && (intStep == 0 || intStep >= 5))) && intEdit != 5)
                                    {
                                        sb.Append("<td nowrap><img src=\"/images/5on.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"greenlink\"><a class=\"greenlink\" href=\"");
                                        sb.Append(oPage.GetFullLink(intPage));
                                        sb.Append("?sid=");
                                        sb.Append(intService.ToString());
                                        sb.Append("&menu_tab=3&edit=5\">Configure Restrictions</a></td>");
                                    }
                                    else if ((intStep == 5 && intEdit == 0) || intEdit == 5)
                                    {
                                        panStep5.Visible = true;
                                        if (intStep == 5)
                                            panStep5Next.Visible = true;
                                        else
                                            panStep5Update.Visible = true;

                                        rptAccess.DataSource = oService.GetRestrictions(intService);
                                        rptAccess.DataBind();
                                        foreach (RepeaterItem ri in rptAccess.Items)
                                            ((LinkButton)ri.FindControl("btnDeleteR")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this requestor?') && LoadWait();");
                                        lblAccess.Visible = (rptAccess.Items.Count == 0);
                                        bool boolR = false;
                                        if (Request.QueryString["addr"] != null || Request.QueryString["delr"] != null)
                                            boolR = true;
                                        else if (ds.Tables[0].Rows[0]["is_restricted"].ToString() == "1")
                                            boolR = true;
                                        if (boolR == true)
                                        {
                                            radAccessRestricted.Checked = true;
                                            divAccessRestricted.Style["display"] = "inline";
                                        }
                                        else
                                            radAccessEveryone.Checked = true;

                                        sb.Append("<td nowrap><img src=\"/images/5on.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"greenlink\"><b>Configure Restrictions</b></td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td nowrap><img src=\"/images/5off.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"calendarother\">Configure Restrictions</td>");
                                    }
                                    sb.Append("</tr><tr>");
                                    // Step 6
                                    if ((intStep == 0 || intStep == -1 || intEdit > 6 || (intStep > 6 && intEdit == 0) || (intEdit < 6 && intEdit > 0 && (intStep == 0 || intStep >= 6))) && intEdit != 6)
                                    {
                                        sb.Append("<td nowrap><img src=\"/images/6on.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"greenlink\"><a class=\"greenlink\" href=\"");
                                        sb.Append(oPage.GetFullLink(intPage));
                                        sb.Append("?sid=");
                                        sb.Append(intService.ToString());
                                        sb.Append("&menu_tab=3&edit=6\">Create the Form</a></td>");
                                    }
                                    else if ((intStep == 6 && intEdit == 0) || intEdit == 6)
                                    {
                                        panStep6.Visible = true;
                                        if (intStep == 6)
                                            panStep6Next.Visible = true;
                                        else
                                            panStep6Update.Visible = true;

                                        lblHeader.Text = oService.GetName(intService);
                                        lblHeaderSub.Text = oService.Get(intService, "description");
                                        radWorkflowYes.Checked = (ds.Tables[0].Rows[0]["workflow"].ToString() == "1");
                                        radWorkflowNo.Checked = (ds.Tables[0].Rows[0]["workflow"].ToString() == "0");
                                        txtWorkflowTitle.Text = ds.Tables[0].Rows[0]["workflow_title"].ToString();
                                        //if (txtWorkflowTitle.Text == "")
                                        //    txtWorkflowTitle.Text = lblHeader.Text;

                                        if (radWorkflowYes.Checked)
                                            divWorkflowYes.Style["display"] = "inline";
                                        if (radWorkflowNo.Checked)
                                            divWorkflowNo.Style["display"] = "inline";
                                        // LOAD MENU
                                        if (ds.Tables[0].Rows[0]["disable_customization"].ToString() == "1")
                                            panFormNo.Visible = true;
                                        else
                                        {
                                            panFormYes.Visible = true;
                                            Tab oTab2 = new Tab("", 0, "divMenu2", true, false);
                                            oTab2.AddTab("Edit Service Request Form", "");
                                            oTab2.AddTab("Preview Service Request Form", "");
                                            oTab2.AddTab("Edit Workload Manager", "");
                                            strMenuTab2 = oTab2.GetTabs();
                                            Tab oTab3 = new Tab("", 0, "divMenu3", true, false);
                                            oTab3.AddTab("Edit Service Request Form", "");
                                            oTab3.AddTab("Inherited Request Information", "");
                                            oTab3.AddTab("Edit Workload Manager", "");
                                            strMenuTab3 = oTab3.GetTabs();
                                            // LOAD FORM
                                            strForm = oServiceEditor.LoadForm(intService, false, true, hdnOrderSR.ClientID, intEnvironment, null, dsn);
                                            if (oServiceEditor.GetConfigs(intService, 0, 1).Tables[0].Rows.Count == 0)
                                                strForm = "<img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> <b>NOTE:</b> You have not added any controls. To add a control, click the &quot;Add Control&quot; button.";
                                            // PREVIEW FORM
                                            strFormPreview = oServiceEditor.LoadForm(intService, false, false, hdnOrderSR.ClientID, intEnvironment, null, dsn);
                                            if (oService.Get(intService, "statement") == "1")
                                                panStatement.Visible = true;
                                            else
                                                panStatement.Visible = false;
                                            panUpload.Visible = (oService.Get(intService, "upload") == "1");
                                            //if (oService.Get(intService, "expedite") == "1")
                                            //    panExpedite.Visible = true;
                                            //else
                                            panExpedite.Visible = false;
                                            if (oService.Get(intService, "project") == "1")
                                                panTitle.Visible = false;
                                            else
                                            {
                                                panTitle.Visible = true;
                                                if (oService.Get(intService, "title_override") == "1")
                                                    lblTitleName.Text = oService.Get(intService, "title_name");
                                            }
                                            int intPriority = 3;
                                            int intWorkingDays = oApplication.GetLead(intApp, intPriority);
                                            if (intWorkingDays > 0)
                                            {
                                                panDeliverable.Visible = true;
                                                //oApplication.AssignPriority(intApp, radPriority, lblDeliverable.ClientID, txtEnd.ClientID, hdnEnd.ClientID);
                                                lblDeliverable.Text = intWorkingDays.ToString();
                                                //txtEnd.Text = DateTime.Today.AddDays(intWorkingDays).ToShortDateString();
                                                hdnEnd.Value = DateTime.Today.AddDays(intWorkingDays).ToShortDateString();
                                            }
                                            radPriority.SelectedValue = intPriority.ToString();
                                            // LOAD Workload Manager
                                            strCheckboxes = oServiceDetail.LoadCheckboxes(0, 0, 0, 0, intService);
                                            // LOAD Workload Manager Form
                                            strFormWM = oServiceEditor.LoadForm(intService, true, true, hdnOrderWM.ClientID, intEnvironment, null, dsn);
                                            if (oServiceEditor.GetConfigs(intService, 1, 1).Tables[0].Rows.Count == 0)
                                                strFormWM = "<img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> <b>NOTE:</b> You have not added any controls. To add a control, click the &quot;Add Control&quot; button.";
                                        }

                                        sb.Append("<td nowrap><img src=\"/images/6on.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"greenlink\"><b>Create the Form</b></td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td nowrap><img src=\"/images/6off.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"calendarother\">Create the Form</td>");
                                    }
                                    sb.Append("</tr><tr>");
                                    // Step 7
                                    if ((intStep == 0 || intStep == -1 || intEdit > 7 || (intStep > 7 && intEdit == 0) || (intEdit < 7 && intEdit > 0 && (intStep == 0 || intStep >= 7))) && intEdit != 7)
                                    {
                                        sb.Append("<td nowrap><img src=\"/images/7on.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"greenlink\"><a class=\"greenlink\" href=\"");
                                        sb.Append(oPage.GetFullLink(intPage));
                                        sb.Append("?sid=");
                                        sb.Append(intService.ToString());
                                        sb.Append("&menu_tab=3&edit=7\">Configure Workflow</a></td>");
                                    }
                                    else if ((intStep == 7 && intEdit == 0) || intEdit == 7)
                                    {
                                        panStep7.Visible = true;
                                        if (intStep == 7)
                                            panStep7Next.Visible = true;
                                        else
                                            panStep7Update.Visible = true;

                                        if (ds.Tables[0].Rows[0]["disable_customization"].ToString() == "1" || ds.Tables[0].Rows[0]["workflow"].ToString() == "0")
                                            btnWorkflow.Enabled = false;
                                        if (ds.Tables[0].Rows[0]["workflow"].ToString() == "1")
                                        {
                                            panWorkflow.Visible = true;
                                            btnWorkflow.Attributes.Add("onclick", "return AddWorkflow('" + intService.ToString() + "','" + radSameTimeYes.ClientID + "')" +
                                                " && ProcessButtons(this) && LoadWait()" +
                                                ";");
                                        }

                                        radWorkflowConnectYes.Checked = (ds.Tables[0].Rows[0]["workflow_connect"].ToString() == "1");
                                        radWorkflowConnectNo.Checked = (ds.Tables[0].Rows[0]["workflow_connect"].ToString() == "0");
                                        radWorkflowSameYes.Checked = (ds.Tables[0].Rows[0]["same_technician"].ToString() == "1");
                                        radWorkflowSameNo.Checked = (ds.Tables[0].Rows[0]["same_technician"].ToString() == "0");
                                        radSameTimeYes.Checked = (ds.Tables[0].Rows[0]["sametime"].ToString() == "1");
                                        radSameTimeNo.Checked = (ds.Tables[0].Rows[0]["sametime"].ToString() == "0");
                                        rptWorkflowsReceive.DataSource = oService.GetWorkflowsReceive(intService);
                                        rptWorkflowsReceive.DataBind();
                                        lblWorkflowsReceive.Visible = (rptWorkflowsReceive.Items.Count == 0);
                                        foreach (RepeaterItem ri in rptWorkflowsReceive.Items)
                                        {
                                            Label lblAssignment = (Label)ri.FindControl("lblAssignment");
                                            int intNextService = Int32.Parse(lblAssignment.Text);
                                            lblAssignment.Text = GetAssignment(intNextService);
                                            if (ds.Tables[0].Rows[0]["disable_customization"].ToString() == "1")
                                            {
                                                ((LinkButton)ri.FindControl("btnWorkflowDelete")).Enabled = false;
                                                ((LinkButton)ri.FindControl("btnWorkflowFields")).Enabled = false;
                                                ((LinkButton)ri.FindControl("btnWorkflowConditions")).Enabled = false;
                                            }
                                            else
                                            {
                                                ((LinkButton)ri.FindControl("btnWorkflowDelete")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this workflow?') && LoadWait();");
                                                ((LinkButton)ri.FindControl("btnWorkflowFields")).Attributes.Add("onclick", "return OpenWindow('SERVICE_EDITOR_FIELD_MAPPINGS','?serviceid=" + intNextService.ToString() + "&nextservice=" + intService.ToString() + "');");
                                                ((LinkButton)ri.FindControl("btnWorkflowConditions")).Attributes.Add("onclick", "return OpenWindow('SERVICE_EDITOR_CONDITIONS','?serviceid=" + intNextService.ToString() + "&nextservice=" + intService.ToString() + "');");
                                            }
                                        }

                                        rptWorkflow.DataSource = oService.GetWorkflows(intService);
                                        rptWorkflow.DataBind();
                                        panWorkflowMultiple.Visible = (rptWorkflow.Items.Count > 1);
                                        if (rptWorkflow.Items.Count == 0)
                                        {
                                            lblWorkflow.Visible = true;
                                            if (radWorkflowConnectYes.Checked)
                                                lblWorkflow.Text = "<img src='/images/alert.gif' border='0' align='absmiddle'> This service has no outgoing workflows...";
                                            else
                                                lblWorkflow.Text = "<img src='/images/cancel.gif' border='0' align='absmiddle'> Currently, this service is configured to prevent outgoing workflows...";
                                        }
                                        foreach (RepeaterItem ri in rptWorkflow.Items)
                                        {
                                            Label lblAssignment = (Label)ri.FindControl("lblAssignment");
                                            int intNextService = Int32.Parse(lblAssignment.Text);
                                            lblAssignment.Text = GetAssignment(intNextService);
                                        }
                                        btnWorkflowPrint.Attributes.Add("onclick", "return OpenWindow('SERVICE_EDITOR_WORKFLOW_PRINT','?serviceid=" + intService.ToString() + "');");

                                        sb.Append("<td nowrap><img src=\"/images/7on.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"greenlink\"><b>Configure Workflow</b></td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td nowrap><img src=\"/images/7off.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"calendarother\">Configure Workflow</td>");
                                    }
                                    sb.Append("</tr><tr>");
                                    // Step 8
                                    if ((intStep == 0 || intStep == -1 || intEdit > 8 || (intStep > 8 && intEdit == 0) || (intEdit < 8 && intEdit > 0 && (intStep == 0 || intStep >= 8))) && intEdit != 8)
                                    {
                                        sb.Append("<td nowrap><img src=\"/images/8on.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"greenlink\"><a class=\"greenlink\" href=\"");
                                        sb.Append(oPage.GetFullLink(intPage));
                                        sb.Append("?sid=");
                                        sb.Append(intService.ToString());
                                        sb.Append("&menu_tab=3&edit=8\">Configure the Users</a></td>");
                                    }
                                    else if ((intStep == 8 && intEdit == 0) || intEdit == 8)
                                    {
                                        panStep8.Visible = true;
                                        if (intStep == 8)
                                            panStep8Next.Visible = true;
                                        else
                                            panStep8Update.Visible = true;

                                        string strEmail = ds.Tables[0].Rows[0]["email"].ToString();
                                        if (strEmail != "0")
                                        {
                                            if (strEmail == "")
                                                radNotifyMT.Checked = true;
                                            else
                                            {
                                                radNotifyMBX.Checked = true;
                                                divNotifyMBX.Style["display"] = "inline";
                                                txtNotifyMBX.Text = strEmail;
                                            }
                                        }
                                        if (radNotifyMT.Checked == false && radNotifyMBX.Checked == false)
                                            radNotifyMT.Checked = true;
                                        rptAssignM.DataSource = oService.GetUser(intService, 1);
                                        rptAssignM.DataBind();
                                        foreach (RepeaterItem ri in rptAssignM.Items)
                                            ((LinkButton)ri.FindControl("btnDeleteM")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this supervisor?') && LoadWait();");
                                        lblAssignM.Visible = (rptAssignM.Items.Count == 0);
                                        rptAssignT.DataSource = oService.GetUser(intService, 0);
                                        rptAssignT.DataBind();
                                        foreach (RepeaterItem ri in rptAssignT.Items)
                                            ((LinkButton)ri.FindControl("btnDeleteT")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this resource?') && LoadWait();");
                                        lblAssignT.Visible = (rptAssignT.Items.Count == 0);
                                        radAssignW.Attributes.Add("onclick", "ShowHideDiv('" + divAssignW.ClientID + "','inline');ShowHideDiv('" + divAssignM.ClientID + "','none');ShowHideDiv('" + divAssignT.ClientID + "','none');ShowHideDiv('" + divNotify.ClientID + "','none');");
                                        if (ds.Tables[0].Rows[0]["workflow"].ToString() != "1")
                                            radAssignW.Text += " (<i>Workflow must be enabled</i>)";
                                        else if (dsWorkflowReceive.Tables[0].Rows.Count == 0)
                                            radAssignW.Text += " (<i>No previous workflows</i>)";
                                        else
                                            radAssignW.Enabled = true;
                                        oService.LoadWorkflowUsers(dsWorkflowReceive, ref ddlWorkflowUser);
                                        rptOwners.DataSource = oService.GetUser(intService, -1);
                                        rptOwners.DataBind();
                                        foreach (RepeaterItem ri in rptOwners.Items)
                                        {
                                            LinkButton btnDeleteO = (LinkButton)ri.FindControl("btnDeleteO");
                                            if (btnDeleteO.CommandName != intProfile.ToString())
                                                btnDeleteO.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service owner?') && LoadWait();");
                                            else
                                                btnDeleteO.Enabled = false;
                                        }
                                        lblOwner.Visible = (rptOwners.Items.Count == 0);
                                        rptApprove.DataSource = oService.GetUser(intService, -10);
                                        rptApprove.DataBind();
                                        foreach (RepeaterItem ri in rptApprove.Items)
                                            ((LinkButton)ri.FindControl("btnDeleteA")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this approver?') && LoadWait();");
                                        lblApprove.Visible = (rptApprove.Items.Count == 0);
                                        bool boolA = false;
                                        if (Request.QueryString["adda"] != null || Request.QueryString["dela"] != null)
                                            boolA = true;
                                        else if (ds.Tables[0].Rows[0]["approval"].ToString() == "1")
                                            boolA = true;
                                        bool boolM = false;
                                        bool boolT = false;
                                        bool boolW = false;
                                        int intWorkflowUserID = 0;
                                        if (Int32.TryParse(ds.Tables[0].Rows[0]["workflow_userid"].ToString(), out intWorkflowUserID) == true && intWorkflowUserID > 0)
                                        {
                                            boolW = true;
                                            ddlWorkflowUser.SelectedValue = intWorkflowUserID.ToString();
                                        }
                                        if (Request.QueryString["addm"] != null || Request.QueryString["delm"] != null)
                                            boolM = true;
                                        else if (Request.QueryString["addt"] != null || Request.QueryString["delt"] != null)
                                            boolT = true;
                                        else if (boolW == false)
                                        {
                                            if (rptAssignM.Items.Count > 0)
                                                boolM = true;
                                            else if (rptAssignT.Items.Count > 0)
                                                boolT = true;
                                        }
                                        if (boolA == true)
                                        {
                                            radApprovalYes.Checked = true;
                                            divApprovalYes.Style["display"] = "inline";
                                        }
                                        else
                                            radApprovalNo.Checked = true;
                                        if (boolM == true)
                                        {
                                            radAssignM.Checked = true;
                                            divAssignM.Style["display"] = "inline";
                                            divNotify.Style["display"] = "inline";
                                        }
                                        if (boolT == true)
                                        {
                                            radAssignT.Checked = true;
                                            divAssignT.Style["display"] = "inline";
                                            divNotify.Style["display"] = "inline";
                                        }
                                        if (boolW == true)
                                        {
                                            radAssignW.Checked = true;
                                            divAssignW.Style["display"] = "inline";
                                        }
                                        sb.Append("<td nowrap><img src=\"/images/8on.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"greenlink\"><b>Configure the Users</b></td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td nowrap><img src=\"/images/8off.png\" border=\"0\" /></td>");
                                        sb.Append("<td nowrap class=\"calendarother\">Configure the Users</td>");
                                    }
                                    sb.Append("</tr>");
                                }
                                else
                                    Response.Redirect(oPage.GetFullLink(intPage));
                            }
                            else
                            {
                                DataSet dsOrder = oServiceEditor.GetConfigs(intService, 0, 0);
                                // Check to see if there was a re-order
                                bool boolReOrderSR = ReOrder(oServiceEditor.GetConfigs(intService, 0, 0), hdnOrderSR);
                                bool boolReOrderWM = ReOrder(oServiceEditor.GetConfigs(intService, 1, 0), hdnOrderWM);
                                if (boolReOrderSR || boolReOrderWM)
                                    EditRedirect("&order=true", false);
                            }
                        }
                        else
                            Response.Redirect(oPage.GetFullLink(intPage));
                    }
                }
                if (intService == 0)
                {
                    btnNew.Attributes.Add("onclick", "NewTab('divMenu1','" + hdnType.ClientID + "',this,3);return false;");
                    panStep1.Visible = true;
                    panStep1Next.Visible = true;
                    if (oApplication.Get(intApplication, "request_items") == "1")
                        panGroup.Visible = true;
                    sb.Append("<tr><td nowrap><img src=\"/images/1on.png\" border=\"0\" /></td>");
                    sb.Append("<td nowrap class=\"greenlink\"><b>Create the Service</b></td></tr>");
                    sb.Append("<tr><td nowrap><img src=\"/images/2off.png\" border=\"0\" /></td>");
                    sb.Append("<td nowrap class=\"calendarother\">Configure the Service</td></tr>");
                    sb.Append("<tr><td nowrap><img src=\"/images/3off.png\" border=\"0\" /></td>");
                    sb.Append("<td nowrap class=\"calendarother\">Configure the Hours</td></tr>");
                    sb.Append("<tr><td nowrap><img src=\"/images/4off.png\" border=\"0\" /></td>");
                    sb.Append("<td nowrap class=\"calendarother\">Configure the Location</td></tr>");
                    sb.Append("<tr><td nowrap><img src=\"/images/5off.png\" border=\"0\" /></td>");
                    sb.Append("<td nowrap class=\"calendarother\">Configure Restrictions</td></tr>");
                    sb.Append("<tr><td nowrap><img src=\"/images/6off.png\" border=\"0\" /></td>");
                    sb.Append("<td nowrap class=\"calendarother\">Create the Form</td></tr>");
                    sb.Append("<tr><td nowrap><img src=\"/images/7off.png\" border=\"0\" /></td>");
                    sb.Append("<td nowrap class=\"calendarother\">Configure Workflow</td></tr>");
                    sb.Append("<tr><td nowrap><img src=\"/images/8off.png\" border=\"0\" /></td>");
                    sb.Append("<td nowrap class=\"calendarother\">Configure the Users</td></tr>");
                }
                else
                    btnNew.Attributes.Add("onclick", "return ProcessButtons(this,'" + btnNew.ClientID + "') && LoadWait();");
                sb.Append("</table>");
                int intMenuTab = 0;
                if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                    intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                Tab oTab = new Tab(hdnType.ClientID, intMenuTab, "divMenu1", true, false);
                oTab.AddTab("Current Service(s)", "");
                oTab.AddTab("Incomplete / Pending Service(s)", "");
                if (intMenuTab == 3)
                    oTab.AddTab("Edit a Service", "");
                //else
                //    oTab.AddTab("Create a Service", "");
                //oTab.AddTab("Help Documentation", "");
                strMenuTab1 = oTab.GetTabs();
                btnNext1.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a service name')" +
                    " && ValidateText('" + txtDescription.ClientID + "','Please enter a description')" +
                    " && CheckGroups('" + lstGroup.ClientID + "')" +
                    " && ValidateRadioButtons('" + radProjectYes.ClientID + "','" + radProjectNo.ClientID + "','Please select if you want this service to be associated with a project')" +
                    " && (document.getElementById('" + radProjectNo.ClientID + "').checked == false || (document.getElementById('" + radProjectNo.ClientID + "').checked == true && ValidateRadioButtons('" + radTitleOverrideYes.ClientID + "','" + radTitleOverrideNo.ClientID + "','Please select whether or not you want to the override the title')))" +
                    " && (document.getElementById('" + radTitleOverrideYes.ClientID + "').checked == false || (document.getElementById('" + radTitleOverrideYes.ClientID + "').checked == true && ValidateText('" + txtTitleName.ClientID + "','Please enter a name for the custom title')))" +
                    " && ValidateRadioButtons('" + radEnabledYes.ClientID + "','" + radEnabledNo.ClientID + "','Please select if you want this service to be enabled')" +
                    " && ProcessButtons(this,'" + btnBack1.ClientID + "') && LoadWait()" +
                    ";");
                btnUpdate1.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a service name')" +
                    " && ValidateText('" + txtDescription.ClientID + "','Please enter a description')" +
                    " && CheckGroups('" + lstGroup.ClientID + "')" +
                    " && ValidateRadioButtons('" + radProjectYes.ClientID + "','" + radProjectNo.ClientID + "','Please select if you want this service to be associated with a project')" +
                    " && (document.getElementById('" + radProjectNo.ClientID + "').checked == false || (document.getElementById('" + radProjectNo.ClientID + "').checked == true && ValidateRadioButtons('" + radTitleOverrideYes.ClientID + "','" + radTitleOverrideNo.ClientID + "','Please select whether or not you want to the override the title')))" +
                    " && (document.getElementById('" + radTitleOverrideYes.ClientID + "').checked == false || (document.getElementById('" + radTitleOverrideYes.ClientID + "').checked == true && ValidateText('" + txtTitleName.ClientID + "','Please enter a name for the custom title')))" +
                    " && ValidateRadioButtons('" + radEnabledYes.ClientID + "','" + radEnabledNo.ClientID + "','Please select if you want this service to be enabled')" +
                    " && ProcessButtons(this,'" + btnCancel1.ClientID + "') && LoadWait()" +
                    ";");
                btnNext2.Attributes.Add("onclick", "return ValidateRadioButtons('" + radRejectYes.ClientID + "','" + radRejectNo.ClientID + "','Please select whether or not you want to allow your team to reject requests')" +
                    " && ValidateRadioButtons('" + radManagerApproveYes.ClientID + "','" + radManagerApproveNo.ClientID + "','Please select if this service requires the requestor\\'s manager to approve to submit')" +
                    " && ValidateRadioButtons('" + radQuantityYes.ClientID + "','" + radQuantityNo.ClientID + "','Please select if you would like the user to be able to change the quantity of the service')" +
                    " && ValidateRadioButtons('" + radTasksYes.ClientID + "','" + radTasksNo.ClientID + "','Please select if this service has multiple tasks or actions that must be finished to complete this task')" +
                    " && ValidateRadioButtons('" + radAutomateYes.ClientID + "','" + radAutomateNo.ClientID + "','Please select if you think this service can be automated')" +
                    " && ValidateRadioButtons('" + radStatementYes.ClientID + "','" + radStatementNo.ClientID + "','Please select if you want the statement of work field to be included')" +
                    " && ValidateRadioButtons('" + radUploadYes.ClientID + "','" + radUploadNo.ClientID + "','Please select if you would like a file upload feature to be included')" +
                    " && ValidateRadioButtons('" + radExpediteYes.ClientID + "','" + radExpediteNo.ClientID + "','Please select if this service can be expedited')" +
                    " && ValidateRadioButtons('" + radNotifyRedYes.ClientID + "','" + radNotifyRedNo.ClientID + "','Please select if you want the requestor to be notified when a status has been changed to RED (On Hold)')" +
                    " && ValidateRadioButtons('" + radNotifyYellowYes.ClientID + "','" + radNotifyYellowNo.ClientID + "','Please select if you want the requestor to be notified when a status has been changed to YELLOW (Issue Encountered)')" +
                    " && ValidateRadioButtons('" + radNotifyGreenYes.ClientID + "','" + radNotifyGreenNo.ClientID + "','Please select if you want the requestor to be notified when a status has been changed back to GREEN (Issue Resoloved / Taken Off Hold)')" +
                    " && ProcessButtons(this,'" + btnBack2.ClientID + "') && LoadWait()" +
                    ";");
                btnUpdate2.Attributes.Add("onclick", "return EnsureMailbox('" + radNotifyMT.ClientID + "','" + radNotifyMBX.ClientID + "','" + txtNotifyMBX.ClientID + "')" +
                    " && ValidateRadioButtons('" + radRejectYes.ClientID + "','" + radRejectNo.ClientID + "','Please select whether or not you want to allow your team to reject requests')" +
                    " && ValidateRadioButtons('" + radManagerApproveYes.ClientID + "','" + radManagerApproveNo.ClientID + "','Please select if this service requires the requestor\\'s manager to approve to submit')" +
                    " && ValidateRadioButtons('" + radQuantityYes.ClientID + "','" + radQuantityNo.ClientID + "','Please select if you would like the user to be able to change the quantity of the service')" +
                    " && ValidateRadioButtons('" + radTasksYes.ClientID + "','" + radTasksNo.ClientID + "','Please select if this service has multiple tasks or actions that must be finished to complete this task')" +
                    " && ValidateRadioButtons('" + radAutomateYes.ClientID + "','" + radAutomateNo.ClientID + "','Please select if you think this service can be automated')" +
                    " && ValidateRadioButtons('" + radStatementYes.ClientID + "','" + radStatementNo.ClientID + "','Please select if you want the statement of work field to be included')" +
                    " && ValidateRadioButtons('" + radUploadYes.ClientID + "','" + radUploadNo.ClientID + "','Please select if you would like a file upload feature to be included')" +
                    " && ValidateRadioButtons('" + radExpediteYes.ClientID + "','" + radExpediteNo.ClientID + "','Please select if this service can be expedited')" +
                    " && ValidateRadioButtons('" + radNotifyRedYes.ClientID + "','" + radNotifyRedNo.ClientID + "','Please select if you want the requestor to be notified when a status has been changed to RED (On Hold)')" +
                    " && ValidateRadioButtons('" + radNotifyYellowYes.ClientID + "','" + radNotifyYellowNo.ClientID + "','Please select if you want the requestor to be notified when a status has been changed to YELLOW (Issue Encountered)')" +
                    " && ValidateRadioButtons('" + radNotifyGreenYes.ClientID + "','" + radNotifyGreenNo.ClientID + "','Please select if you want the requestor to be notified when a status has been changed back to GREEN (Issue Resoloved / Taken Off Hold)')" +
                    " && ProcessButtons(this,'" + btnCancel2.ClientID + "') && LoadWait()" +
                    ";");
                btnNext4.Attributes.Add("onclick", "return ProcessButtons(this,'" + btnBack4.ClientID + "') && LoadWait();");
                btnUpdate4.Attributes.Add("onclick", "return ProcessButtons(this,'" + btnCancel4.ClientID + "') && LoadWait();");
                btnNext5.Attributes.Add("onclick", "return ProcessButtons(this,'" + btnBack5.ClientID + "') && LoadWait();");
                btnUpdate5.Attributes.Add("onclick", "return ProcessButtons(this,'" + btnCancel5.ClientID + "') && LoadWait();");
                txtAccess.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAccess.ClientID + "','" + lstAccess.ClientID + "','" + hdnAccess.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstAccess.Attributes.Add("ondblclick", "AJAXClickRow();");
                btnAccess.Attributes.Add("onclick", "return ValidateHidden0('" + hdnAccess.ClientID + "','" + txtAccess.ClientID + "','Please enter the LAN ID of the requestor') && ProcessButton(this) && LoadWait();");
                int intWorkflowsReceive = oService.GetWorkflowsReceive(intService).Tables[0].Rows.Count;
                int intWorkflowsSend = oService.GetWorkflows(intService).Tables[0].Rows.Count;
                btnNext6.Attributes.Add("onclick", "return ValidateRadioButtons('" + radWorkflowYes.ClientID + "','" + radWorkflowNo.ClientID + "','Please select whether or not you want workflow to be enabled')" +
                    " && EnsureWorkflowGone('" + radWorkflowNo.ClientID + "','" + radWorkflowYes.ClientID + "'," + intWorkflowsReceive + ")" +
                    //" && ValidateText('" + txtWorkflowTitle.ClientID + "','Enter a name for the workflow')" +
                    " && ProcessButtons(this,'" + btnBack6.ClientID + "') && LoadWait()" +
                    ";");
                btnUpdate6.Attributes.Add("onclick", "return ValidateRadioButtons('" + radWorkflowYes.ClientID + "','" + radWorkflowNo.ClientID + "','Please select whether or not you want workflow to be enabled')" +
                    " && EnsureWorkflowGone('" + radWorkflowNo.ClientID + "','" + radWorkflowYes.ClientID + "'," + intWorkflowsReceive + ")" +
                    //" && ValidateText('" + txtWorkflowTitle.ClientID + "','Enter a name for the workflow')" +
                    " && ProcessButtons(this,'" + btnCancel6.ClientID + "') && LoadWait()" +
                    ";");
                btnNext7.Attributes.Add("onclick", "return ValidateRadioButtons('" + radWorkflowConnectYes.ClientID + "','" + radWorkflowConnectNo.ClientID + "','Please select whether or not you want others to be able to connect a workflow to this service')" +
                    " && ValidateRadioButtons('" + radWorkflowSameYes.ClientID + "','" + radWorkflowSameNo.ClientID + "','Please select whether or not you want to allow the same person to be assigned to multiple tasks in the workflow')" +
                    " && (" + intWorkflowsSend + " < 2 || (" + intWorkflowsSend + " >= 2 && ValidateRadioButtons('" + radSameTimeYes.ClientID + "','" + radSameTimeNo.ClientID + "','Please select how you want to handle the completion of the multiple workflows')))" +
                    " && ProcessButtons(this,'" + btnBack7.ClientID + "') && LoadWait()" +
                    ";");
                btnUpdate7.Attributes.Add("onclick", "return ValidateRadioButtons('" + radWorkflowConnectYes.ClientID + "','" + radWorkflowConnectNo.ClientID + "','Please select whether or not you want others to be able to connect a workflow to this service')" +
                    " && ValidateRadioButtons('" + radWorkflowSameYes.ClientID + "','" + radWorkflowSameNo.ClientID + "','Please select whether or not you want to allow the same person to be assigned to multiple tasks in the workflow')" +
                    " && (" + intWorkflowsSend + " < 2 || (" + intWorkflowsSend + " >= 2 && ValidateRadioButtons('" + radSameTimeYes.ClientID + "','" + radSameTimeNo.ClientID + "','Please select how you want to handle the completion of the multiple workflows')))" +
                    " && ProcessButtons(this,'" + btnCancel7.ClientID + "') && LoadWait()" +
                    ";");
                radWorkflowYes.Attributes.Add("onclick", "ShowHideDiv('" + divWorkflowYes.ClientID + "','inline');ShowHideDiv('" + divWorkflowNo.ClientID + "','none');");
                radWorkflowNo.Attributes.Add("onclick", "ShowHideDiv('" + divWorkflowNo.ClientID + "','inline');ShowHideDiv('" + divWorkflowYes.ClientID + "','none');");
                radNotifyMBX.Attributes.Add("onclick", "ShowHideDiv('" + divNotifyMBX.ClientID + "','inline');");
                radNotifyMT.Attributes.Add("onclick", "ShowHideDiv('" + divNotifyMBX.ClientID + "','none');");
                radAccessEveryone.Attributes.Add("onclick", "ShowHideDiv('" + divAccessRestricted.ClientID + "','none');");
                radAccessRestricted.Attributes.Add("onclick", "ShowHideDiv('" + divAccessRestricted.ClientID + "','inline');");
                btnControl.Attributes.Add("onclick", "return OpenWindow('NEW_CONTROL','?wm=0&serviceid=" + intService.ToString() + "');");
                btnControlWM.Attributes.Add("onclick", "return OpenWindow('NEW_CONTROL','?wm=1&serviceid=" + intService.ToString() + "');");
                btnControlWM2.Attributes.Add("onclick", "return OpenWindow('NEW_CONTROL','?wm=1&serviceid=" + intService.ToString() + "');");
                btnOrder.Attributes.Add("onclick", "return OpenWindow('TASKS_ORDER','?id=" + intService.ToString() + "');");
                radProjectYes.Attributes.Add("onclick", "document.getElementById('" + radTitleOverrideNo.ClientID + "').click();ShowHideDiv('" + divProjectNo.ClientID + "','none');ShowHideDiv('" + divTitleOverrideYes.ClientID + "','none');");
                radProjectNo.Attributes.Add("onclick", "ShowHideDiv('" + divProjectNo.ClientID + "','inline');ShowHideDivCheck('" + divTitleOverrideYes.ClientID + "',document.getElementById('" + radTitleOverrideYes.ClientID + "'));");
                radTitleOverrideYes.Attributes.Add("onclick", "ShowHideDiv('" + divTitleOverrideYes.ClientID + "','inline');");
                radTitleOverrideNo.Attributes.Add("onclick", "ShowHideDiv('" + divTitleOverrideYes.ClientID + "','none');");

                radAssignM.Attributes.Add("onclick", "ShowHideDiv('" + divNotify.ClientID + "','inline');ShowHideDiv('" + divAssignM.ClientID + "','inline');ShowHideDiv('" + divAssignT.ClientID + "','none');ShowHideDiv('" + divAssignW.ClientID + "','none');");
                radAssignT.Attributes.Add("onclick", "ShowHideDiv('" + divNotify.ClientID + "','inline');ShowHideDiv('" + divAssignT.ClientID + "','inline');ShowHideDiv('" + divAssignM.ClientID + "','none');ShowHideDiv('" + divAssignW.ClientID + "','none');");
                txtAssignM.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAssignM2.ClientID + "','" + lstAssignM.ClientID + "','" + hdnAssignM.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstAssignM.Attributes.Add("ondblclick", "AJAXClickRow();");
                btnAssignM.Attributes.Add("onclick", "return ValidateHidden0('" + hdnAssignM.ClientID + "','" + txtAssignM.ClientID + "','Please enter the LAN ID of the supervisor') && ProcessButton(this) && LoadWait();");
                txtAssignT.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAssignT2.ClientID + "','" + lstAssignT.ClientID + "','" + hdnAssignT.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstAssignT.Attributes.Add("ondblclick", "AJAXClickRow();");
                btnAssignT.Attributes.Add("onclick", "return ValidateHidden0('" + hdnAssignT.ClientID + "','" + txtAssignT.ClientID + "','Please enter the LAN ID of the resource') && ProcessButton(this) && LoadWait();");
                txtOwner.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divOwner2.ClientID + "','" + lstOwner.ClientID + "','" + hdnOwner.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstOwner.Attributes.Add("ondblclick", "AJAXClickRow();");
                btnOwner.Attributes.Add("onclick", "return ValidateHidden0('" + hdnOwner.ClientID + "','" + txtOwner.ClientID + "','Please enter the LAN ID of the resource') && ProcessButton(this) && LoadWait();");
                radApprovalYes.Attributes.Add("onclick", "ShowHideDiv('" + divApprovalYes.ClientID + "','inline');");
                radApprovalNo.Attributes.Add("onclick", "ShowHideDiv('" + divApprovalYes.ClientID + "','none');");
                txtApprove.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divApprove.ClientID + "','" + lstApprove.ClientID + "','" + hdnApprove.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstApprove.Attributes.Add("ondblclick", "AJAXClickRow();");
                btnApprove.Attributes.Add("onclick", "return ValidateHidden0('" + hdnApprove.ClientID + "','" + txtApprove.ClientID + "','Please enter the LAN ID of the approver') && ProcessButton(this) && LoadWait();");
                btnNext8.Attributes.Add("onclick", "return EnsureMailbox('" + radNotifyMT.ClientID + "','" + radNotifyMBX.ClientID + "','" + txtNotifyMBX.ClientID + "')" +
                    " && EnsureAssignment('" + radAssignM.ClientID + "'," + rptAssignM.Items.Count + ",'" + radAssignT.ClientID + "'," + rptAssignT.Items.Count + ",'" + radAssignW.ClientID + "','" + ddlWorkflowUser.ClientID + "')" +
                    " && ProcessButtons(this,'" + btnBack2.ClientID + "') && LoadWait()" +
                    ";");
                btnUpdate8.Attributes.Add("onclick", "return EnsureMailbox('" + radNotifyMT.ClientID + "','" + radNotifyMBX.ClientID + "','" + txtNotifyMBX.ClientID + "')" +
                    " && EnsureAssignment('" + radAssignM.ClientID + "'," + rptAssignM.Items.Count + ",'" + radAssignT.ClientID + "'," + rptAssignT.Items.Count + ",'" + radAssignW.ClientID + "','" + ddlWorkflowUser.ClientID + "')" +
                    " && ProcessButtons(this,'" + btnCancel2.ClientID + "') && LoadWait()" +
                    ";");
            }

            strEdit = sb.ToString();
        }
        private string GetAssignment(int _serviceid)
        {
            StringBuilder strAssignment = new StringBuilder();
            DataSet dsAssignmentT = oService.GetUser(_serviceid, 0);
            if (dsAssignmentT.Tables[0].Rows.Count > 0)
            {
                strAssignment.Append("<u>Assigned To:</u>");
                foreach (DataRow drAssignment in dsAssignmentT.Tables[0].Rows)
                {
                    strAssignment.Append("<br/>");
                    int intAssignment = Int32.Parse(drAssignment["userid"].ToString());
                    strAssignment.Append(oUser.GetFullName(intAssignment) + " (" + oUser.GetName(intAssignment) + ")");
                }
            }
            DataSet dsAssignmentM = oService.GetUser(_serviceid, 1);
            if (dsAssignmentM.Tables[0].Rows.Count > 0)
            {
                strAssignment.Append("<u>Assigned By:</u>");
                foreach (DataRow drAssignment in dsAssignmentM.Tables[0].Rows)
                {
                    strAssignment.Append("<br/>");
                    int intAssignment = Int32.Parse(drAssignment["userid"].ToString());
                    strAssignment.Append(oUser.GetFullName(intAssignment) + " (" + oUser.GetName(intAssignment) + ")");
                }
            }
            return strAssignment.ToString();
        }
        private bool ReOrder(DataSet _ds, HiddenField _hidden)
        {
            bool boolDone = false;
            if (Request.Form[_hidden.UniqueID] != null && Request.Form[_hidden.UniqueID] != "")
            {
                boolDone = true;
                string strOrder = Request.Form[_hidden.UniqueID];
                int intOrderDisplay = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("_")));
                int intOrderConfig = Int32.Parse(strOrder.Substring(strOrder.IndexOf("_") + 1));
                int intOldOrder = Int32.Parse(oServiceEditor.GetConfig(intOrderConfig, "display"));
                if (intOldOrder < intOrderDisplay)
                    intOrderDisplay--;
                int intDisplay = 0;
                bool boolFound = false;
                for (int ii = 1; ii <= _ds.Tables[0].Rows.Count; ii++)
                {
                    int intCurrent = Int32.Parse(_ds.Tables[0].Rows[ii - 1]["id"].ToString());
                    if (ii == intOrderDisplay && boolFound == false)
                    {
                        intDisplay++;
                        oServiceEditor.UpdateConfigOrder(intOrderConfig, intDisplay);
                        intDisplay++;
                        oServiceEditor.UpdateConfigOrder(intCurrent, intDisplay);
                    }
                    else if (intCurrent != intOrderConfig)
                    {
                        intDisplay++;
                        if (intDisplay == intOrderDisplay && boolFound == true)
                        {
                            oServiceEditor.UpdateConfigOrder(intOrderConfig, intDisplay);
                            intDisplay++;
                        }
                        oServiceEditor.UpdateConfigOrder(intCurrent, intDisplay);
                    }
                    else
                        boolFound = true;
                }
            }
            return boolDone;
        }
        private void LoadGroups()
        {
            DataSet dsGroups = oRequestItem.GetItems(intApplication, 1, 1);
            lstGroup.DataValueField = "itemid";
            lstGroup.DataTextField = "service_title";
            lstGroup.DataSource = dsGroups;
            lstGroup.DataBind();
            if (dsGroups.Tables[0].Rows.Count > 0)
                lblGroup.Text = dsGroups.Tables[0].Rows[0]["itemid"].ToString();
        }
        private void LoadLocations()
        {
            TreeNode oNode = new TreeNode();
            oNode.Text = "ClearView Services";
            oNode.Value = "0";
            oNode.ToolTip = "ClearView Services";
            oNode.SelectAction = TreeNodeSelectAction.None;
            treLocation.Nodes.Add(oNode);
            LoadLocations(0, oNode);
            treLocation.CollapseAll();
            oNode.Expand();
            oService.ExpandLocations(oNode, intService, 1);
            oNode.ShowCheckBox = false;
        }
        private void LoadLocations(int _parentid, TreeNode oParent)
        {
            DataSet ds = oService.GetFolders(_parentid, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oParent.ChildNodes.Add(oNode);
                LoadLocations(Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void LoadServices()
        {
            LoadServicesApproved(0, "", "");
        }
        private void LoadServicesApproved(int _parentid, string _spacer, string _parent)
        {
            int intSpacer = 0;
            if (_spacer != "")
            {
                intSpacer = Int32.Parse(_spacer) + 25;
                _spacer = "<img src=\"/images/spacer.gif\" border=\"0\" width=\"" + intSpacer.ToString() + "px\" height=\"1px\"/>";
            }
            // Load Parent Services
            bool boolChildren = false;
            DataSet ds = oService.GetFolders(_parentid, 1);
            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                boolChildren = true;
                int intFolder = Int32.Parse(dr["id"].ToString());
                //sb = new StringBuilder(strParent);
                sb = new StringBuilder(_parent);
                sb.Append("<tr>");
                sb.Append("<td><table cellpadding=\"2\" cellspacing=\"0\" border=\"0\"><tr><td>");
                sb.Append(_spacer);
                sb.Append("</td><td><img src=\"/images/folder.gif\" border=\"0\" /></td><td>");
                sb.Append(dr["name"].ToString());
                sb.Append("</td></tr></table></td>");
                sb.Append("<td>");
                sb.Append(dr["created"].ToString());
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append(dr["modified"].ToString());
                sb.Append("</td>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                //strParent = sb.ToString();
                LoadServicesApproved(intFolder, intSpacer.ToString(), sb.ToString());
            }

            // Load Child Services
            ds = oService.Gets(_parentid, intProfile);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //strServicesC += strParent;
                //strParent = "";
                string strServiceTemp = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intService = Int32.Parse(dr["serviceid"].ToString());
                    string strService = "";
                    bool boolComplete = false;
                    string strShow = "Incomplete";
                    string strShowCSS = "bluenote";
                    int intStep = Int32.Parse(dr["step"].ToString());
                    int intWorkflow = 0;
                    Int32.TryParse(dr["workflow"].ToString(), out intWorkflow);
                    if (intStep != 0)
                    {
                        if (intStep == -1)
                        {
                            strShow = "Pending";
                            strShowCSS = "pending";
                        }
                        else
                        {
                            strShow = "Incomplete";
                            strShowCSS = "bluenote";
                        }
                    }
                    else if (dr["show"].ToString() == "0")
                    {
                        strShow = "Hidden";
                        strShowCSS = "reddefault";
                        boolComplete = true;
                    }
                    else if (intWorkflow != 0)
                    {
                        strShow = "Workflow";
                        strShowCSS = "waiting";
                        boolComplete = true;
                    }
                    else if (dr["show"].ToString() == "1")
                    {
                        strShow = "Available";
                        strShowCSS = "approved";
                        boolComplete = true;
                    }
                    strService += "<tr onmouseover=\"CellRowOver(this);\" onmouseout=\"CellRowOut(this);\" onclick=\"ShowService('" + oPage.GetFullLink(intPage) + "?sid=" + intService.ToString() + "');\">";
                    if (boolComplete == true)
                        strService += "<td><table cellpadding=\"2\" cellspacing=\"0\" border=\"0\"><tr><td>" + _spacer + "</td><td><img src=\"/images/down_right.gif\" border=\"0\" /></td><td>" + dr["name"].ToString() + "</td></tr></table></td>";
                    else
                        strService += "<td>" + dr["name"].ToString() + "</td>";
                    strService += "<td>" + dr["created"].ToString() + "</td>";
                    strService += "<td>" + dr["modified"].ToString() + "</td>";
                    strService += "<td class=\"" + strShowCSS + "\">" + strShow + "</td>";
                    strService += "</tr>";
                    if (boolComplete == true)
                        strServiceTemp += strService;
                    //strServicesC += strService;
                    else
                        strServicesI += strService;
                }
                if (strServiceTemp != "")
                {
                    strServicesC += _parent;
                    strServicesC += strServiceTemp;
                }
            }
            //else if (boolChildren == false)
            //strParent = "";
        }
        private void LoadDetails(int _serviceid)
        {
            TreeNode oNode = new TreeNode();
            oNode.Text = "&nbsp;\"" + oService.GetName(intService) + "\" Tasks";
            oNode.ToolTip = oNode.Text;
            oNode.ImageUrl = "/images/folder.gif";
            oNode.SelectAction = TreeNodeSelectAction.None;
            oTree.Nodes.Add(oNode);
            LoadDetails(_serviceid, 0, oNode);
            TreeNode oNew = new TreeNode();
            oNew.Text = "&nbsp;Add a New Task";
            oNew.ToolTip = "Add a New Task";
            oNew.ImageUrl = "/images/postit.gif";
            oNew.NavigateUrl = "javascript:AddTask('" + _serviceid + "','0');";
            oNode.ChildNodes.Add(oNew);
            oTree.ExpandDepth = 1;
            oTree.Attributes.Add("oncontextmenu", "return false;");
            lblTotal.Text = oServiceDetail.GetHours(intService, 1).ToString("F");
        }
        private void LoadDetails(int _serviceid, int _parentid, TreeNode oParent)
        {
            DataSet ds = oServiceDetail.Gets(_serviceid, _parentid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intTaskCount++;
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString() + " (" + double.Parse(dr["hours"].ToString()).ToString("N") + " HRs)";
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = (dr["enabled"].ToString() == "1" ? "/images/check.gif" : "/images/cancel.gif");
                oNode.NavigateUrl = "javascript:EditTask('" + dr["id"].ToString() + "','" + _serviceid.ToString() + "','" + _parentid.ToString() + "');";
                oParent.ChildNodes.Add(oNode);
                LoadDetails(_serviceid, Int32.Parse(dr["id"].ToString()), oNode);
                //TreeNode oNew = new TreeNode();
                //oNew.Text = "&nbsp;Add a New Task";
                //oNew.ToolTip = "Add a New Task";
                //oNew.ImageUrl = "/images/postit.gif";
                //oNew.NavigateUrl = "javascript:AddTask('" + _serviceid + "','" + dr["id"].ToString() + "');";
                //oNode.ChildNodes.Add(oNew);
            }
        }
        protected void btnNext1_Click(Object Sender, EventArgs e)
        {
            int intItem = 0;
            if (panGroup.Visible == true)
                intItem = Int32.Parse(lstGroup.SelectedItem.Value);
            else if (lblGroup.Text != "")
                intItem = Int32.Parse(lblGroup.Text);
            else
                intItem = oRequestItem.AddItem(intApplication, oApplication.GetName(intApplication), oApplication.GetName(intApplication), "", 0, 1, 1, 1, 1);
            intService = oService.Add(txtName.Text, txtDescription.Text, "", intItem, intServiceType, (radEnabledYes.Checked ? 1 : 0), (radProjectYes.Checked ? 1 : 0), 2, 0.00, 0.00, -1, -1, -1, -1, -1, strRR, strWM, strCP, strCA, -1, 0, 0, 0, -1, 0, 1, 0, -1, "0", 1, -1, -1, -1, 0, "", -1, "", -1, -1, -1, -1, "", 0, 0, 0, 1, 1);
            oService.AddUser(intService, intProfile, -1);
            oService.AddFolders(intService, 0, 0);
            EditRedirect("", true);
        }
        protected void btnUpdate1_Click(Object Sender, EventArgs e)
        {
            if (panGroup.Visible == true)
                oService.Update(intService, Int32.Parse(lstGroup.SelectedItem.Value), dsnAsset);
            int intStep = Int32.Parse(oService.Get(intService, "step"));
            oService.UpdateEditor(intService, txtName.Text, txtDescription.Text, (radEnabledYes.Checked ? 1 : 0), (radProjectYes.Checked ? 1 : 0), (radTitleOverrideYes.Checked ? 1 : 0), (radTitleOverrideYes.Checked ? txtTitleName.Text : ""));
            EditRedirect("&update=true", false);
        }
        protected void btnBack2_Click(Object Sender, EventArgs e)
        {
            oService.UpdateStep(intService, 1);
            EditRedirect("", true);
        }
        protected void btnNext2_Click(Object Sender, EventArgs e)
        {
            oService.UpdateEditor(intService, (radAutomateYes.Checked ? 1 : 0), (radStatementYes.Checked ? 1 : 0), (radUploadYes.Checked ? 1 : 0), (radExpediteYes.Checked ? 1 : 0), (radRejectYes.Checked ? 1 : 0), (radQuantityYes.Checked ? 1 : 0), (radTasksYes.Checked ? 1 : 0), (radNotifyGreenYes.Checked ? 1 : 0), (radNotifyYellowYes.Checked ? 1 : 0), (radNotifyRedYes.Checked ? 1 : 0), (radManagerApproveYes.Checked ? 1 : 0), txtNotifyComplete.Text);
            oService.UpdateStep(intService, 3);
            EditRedirect("", true);
        }
        protected void btnUpdate2_Click(Object Sender, EventArgs e)
        {
            if (oService.Get(intService, "tasks") == "1" && radTasksNo.Checked == true)
            {
                // Update the hours with the total of the tasks
                int intNoSlider = Int32.Parse(oService.Get(intService, "no_slider"));
                double dblSLA = double.Parse(oService.Get(intService, "sla"));
                int intHideSLA = Int32.Parse(oService.Get(intService, "hide_sla"));
                oService.UpdateEditor(intService, oServiceDetail.GetHours(intService, 1), intNoSlider, dblSLA, intHideSLA);
            }
            oService.UpdateEditor(intService, (radAutomateYes.Checked ? 1 : 0), (radStatementYes.Checked ? 1 : 0), (radUploadYes.Checked ? 1 : 0), (radExpediteYes.Checked ? 1 : 0), (radRejectYes.Checked ? 1 : 0), (radQuantityYes.Checked ? 1 : 0), (radTasksYes.Checked ? 1 : 0), (radNotifyGreenYes.Checked ? 1 : 0), (radNotifyYellowYes.Checked ? 1 : 0), (radNotifyRedYes.Checked ? 1 : 0), (radManagerApproveYes.Checked ? 1 : 0), txtNotifyComplete.Text);
            EditRedirect("&update=true", false);
        }
        protected void btnBack3_Click(Object Sender, EventArgs e)
        {
            oService.UpdateStep(intService, 2);
            EditRedirect("", true);
        }
        protected void btnNext3_Click(Object Sender, EventArgs e)
        {
            double dblHours = 0.00;
            if (txtHours.Text != "")
                dblHours = double.Parse(txtHours.Text);
            oService.UpdateEditor(intService, dblHours, (radNoSliderYes.Checked ? 1 : 0), double.Parse(txtSLA.Text), (radSLAHideYes.Checked ? 1 : 0));
            oService.UpdateStep(intService, 4);
            EditRedirect("", true);
        }
        protected void btnUpdate3_Click(Object Sender, EventArgs e)
        {
            double dblHours = 0.00;
            if (txtHours.Text != "")
                dblHours = double.Parse(txtHours.Text);
            oService.UpdateEditor(intService, dblHours, (radNoSliderYes.Checked ? 1 : 0), double.Parse(txtSLA.Text), (radSLAHideYes.Checked ? 1 : 0));
            ResourceRequest oResourceRequest = new ResourceRequest(intProfile, dsn);
            oResourceRequest.UpdateWorkflowAllocated(intService);
            EditRedirect("&update=true", false);
        }
        protected void btnBack4_Click(Object Sender, EventArgs e)
        {
            oService.UpdateStep(intService, 3);
            EditRedirect("", true);
        }
        protected void btnNext4_Click(Object Sender, EventArgs e)
        {
            oService.DeleteFolders(intService);
            foreach (TreeNode oNode in treLocation.Nodes)
                SaveLocation(oNode);
            oService.UpdateStep(intService, 5);
            EditRedirect("", true);
        }
        protected void btnUpdate4_Click(Object Sender, EventArgs e)
        {
            oService.DeleteFolders(intService);
            foreach (TreeNode oNode in treLocation.Nodes)
                SaveLocation(oNode);
            EditRedirect("&update=true", false);
        }
        protected void btnBack5_Click(Object Sender, EventArgs e)
        {
            oService.UpdateStep(intService, 4);
            EditRedirect("", true);
        }
        protected void btnNext5_Click(Object Sender, EventArgs e)
        {
            oService.UpdateEditorRestriction(intService, (radAccessRestricted.Checked ? 1 : 0));
            oService.UpdateStep(intService, 6);
            EditRedirect("", true);
        }
        protected void btnUpdate5_Click(Object Sender, EventArgs e)
        {
            oService.UpdateEditorRestriction(intService, (radAccessRestricted.Checked ? 1 : 0));
            EditRedirect("&update=true", false);
        }
        protected void SaveLocation(TreeNode oParent)
        {
            if (oParent.Checked == true)
                oService.AddFolders(intService, Int32.Parse(oParent.Value), 0);
            foreach (TreeNode oNode in oParent.ChildNodes)
                SaveLocation(oNode);
        }
        protected void btnBack6_Click(Object Sender, EventArgs e)
        {
            oService.UpdateStep(intService, 5);
            EditRedirect("", true);
        }
        protected void btnNext6_Click(Object Sender, EventArgs e)
        {
            oService.UpdateEditorWorkflow(intService, (radWorkflowYes.Checked ? 1 : 0), txtWorkflowTitle.Text);
            if (panFormYes.Visible == true)
                oServiceEditor.AlterTable(intService);
            if (radWorkflowNo.Checked == true)
                DeleteWorkflows();
            oService.UpdateStep(intService, 7);
            EditRedirect("", true);
        }
        protected void btnUpdate6_Click(Object Sender, EventArgs e)
        {
            oService.UpdateEditorWorkflow(intService, (radWorkflowYes.Checked ? 1 : 0), txtWorkflowTitle.Text);
            if (panFormYes.Visible == true)
                oServiceEditor.AlterTable(intService);
            if (radWorkflowNo.Checked == true)
                DeleteWorkflows();
            EditRedirect("&update=true", false);
        }
        protected void btnBack7_Click(Object Sender, EventArgs e)
        {
            oService.UpdateStep(intService, 6);
            EditRedirect("", true);
        }
        protected void btnNext7_Click(Object Sender, EventArgs e)
        {
            oService.UpdateEditorWorkflow(intService, (panWorkflow.Visible ? (radSameTimeYes.Checked ? 1 : 0) : -1), (radWorkflowConnectYes.Checked ? 1 : 0), (radWorkflowSameYes.Checked ? 1 : 0));
            oService.UpdateStep(intService, 8);
            EditRedirect("", true);
        }
        protected void btnUpdate7_Click(Object Sender, EventArgs e)
        {
            oService.UpdateEditorWorkflow(intService, (panWorkflow.Visible ? (radSameTimeYes.Checked ? 1 : 0) : -1), (radWorkflowConnectYes.Checked ? 1 : 0), (radWorkflowSameYes.Checked ? 1 : 0));
            EditRedirect("&update=true", false);
        }
        protected void btnBack8_Click(Object Sender, EventArgs e)
        {
            oService.UpdateStep(intService, 7);
            EditRedirect("", true);
        }
        protected void btnNext8_Click(Object Sender, EventArgs e)
        {
            oService.UpdateEditor(intService, (radNotifyMT.Checked ? "" : txtNotifyMBX.Text), (radAssignW.Checked ? Int32.Parse(ddlWorkflowUser.SelectedItem.Value) : 0), (radApprovalYes.Checked ? 1 : 0));

            // Finish up the service
            string strEmail = "";
            string[] strAdmin;
            char[] strSplit = { ';' };
            strAdmin = strAdmins.Split(strSplit);
            for (int ii = 0; ii < strAdmin.Length; ii++)
            {
                if (strAdmin[ii].Trim() != "")
                    strEmail += oUser.GetName(Int32.Parse(strAdmin[ii].Trim())) + ";";
            }
            DataSet ds = oService.GetFoldersService(intService);
            if (ds.Tables[0].Rows.Count > 0)
            {
                // Service was placed - do not route
                oService.UpdateStep(intService, 0);
                StringBuilder sb = new StringBuilder();
                sb.Append("<table cellpadding=\"4\" cellspacing=\"3\" border=\"0\" style=\"");
                sb.Append(oVariable.DefaultFontStyle());
                sb.Append("\">");
                sb.Append("<tr><td><b>Service Name:</b></td><td>");
                sb.Append(oService.Get(intService, "name"));
                sb.Append("</td></tr>");
                sb.Append("<tr><td><b>Application:</b></td><td>");
                sb.Append(oApplication.GetName(intApplication));
                sb.Append("</td></tr>");
                sb.Append("<tr><td><b>Group:</b></td><td>");
                sb.Append(oRequestItem.GetItemName(oService.GetItemId(intService)));
                sb.Append("</td></tr>");
                sb.Append("<tr><td><b>Created By:</b></td><td>");
                sb.Append(oUser.GetFullName(intProfile));
                sb.Append("</td></tr>");
                sb.Append("<tr><td><b>Created On:</b></td><td>");
                sb.Append(DateTime.Now.ToLongDateString());
                sb.Append(" at ");
                sb.Append(DateTime.Now.ToShortTimeString());
                sb.Append("</td></tr>");
                string strLocations = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                    strLocations += oService.GetFolder(Int32.Parse(dr["folderid"].ToString()), "name") + "<br/>";
                sb.Append("<tr><td><b>Location(s):</b></td><td>");
                sb.Append(strLocations);
                sb.Append("</td></tr>");
                sb.Append("</table>");
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
                oFunction.SendEmail("ClearView Service Editor Notification", strEmail, "", strEMailIdsBCC, "ClearView Service Editor Notification", "<p><b>A new service editor has been submitted. Here is the summary of this service...</b></p><p>" + sb.ToString() + "</p><p><b>NOTE:</b> You do not need to take action on this service. This message is simply to notify you of the newly created service.</p>", true, false);
            }
            else
            {
                oService.UpdateStep(intService, -1);
                // Send for approval
                string strURL = oVariable.URL() + "/admin/services_pending.aspx?id=" + intService.ToString();
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                oFunction.SendEmail("ClearView Service Editor Approval", strEmail, "", strEMailIdsBCC, "ClearView Service Editor Approval", "<p><b>A new service editor has been submitted and requires you to configure its location.</b></p><p><a href=\"" + strURL + "\" target=\"_blank\">Click here to view this service.</a></p>", true, false);
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?new=" + oFunction.encryptQueryString(intService.ToString()));
        }
        protected void btnUpdate8_Click(Object Sender, EventArgs e)
        {
            oService.UpdateEditor(intService, (radNotifyMT.Checked ? "" : txtNotifyMBX.Text), (radAssignW.Checked ? Int32.Parse(ddlWorkflowUser.SelectedItem.Value) : 0), (radApprovalYes.Checked ? 1 : 0));
            EditRedirect("&update=true", false);
        }
        protected void DeleteWorkflows()
        {
            DataSet dsWorkflows = oService.GetWorkflowsReceive(intService);
            foreach (DataRow drWorkflow in dsWorkflows.Tables[0].Rows)
                oService.DeleteWorkflow(Int32.Parse(drWorkflow["id"].ToString()));
            // Delete from Service Editor
            oServiceEditor.DeleteConfigWorkflows(0, intService);
        }
        protected void btnWorkflowDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oService.DeleteWorkflow(Int32.Parse(oButton.CommandArgument));
            EditRedirect("&delete=true", false);
        }
        protected void btnAssignM_Click(Object Sender, EventArgs e)
        {
            DataSet ds = oService.GetUser(intService, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
                oService.DeleteUser(Int32.Parse(dr["id"].ToString()));
            bool boolDuplicate = false;
            int intUser = Int32.Parse(Request.Form[hdnAssignM.UniqueID]);
            ds = oService.GetUser(intService, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["userid"].ToString()) == intUser)
                {
                    boolDuplicate = true;
                    break;
                }
            }
            if (boolDuplicate == false)
                oService.AddUser(intService, intUser, 1);
            EditRedirect("&addm=true", false);
        }
        protected void btnAssignT_Click(Object Sender, EventArgs e)
        {
            DataSet ds = oService.GetUser(intService, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
                oService.DeleteUser(Int32.Parse(dr["id"].ToString()));
            bool boolDuplicate = false;
            int intUser = Int32.Parse(Request.Form[hdnAssignT.UniqueID]);
            ds = oService.GetUser(intService, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["userid"].ToString()) == intUser)
                {
                    boolDuplicate = true;
                    break;
                }
            }
            if (boolDuplicate == false)
                oService.AddUser(intService, intUser, 0);
            EditRedirect("&addt=true", false);
        }
        protected void btnOwner_Click(Object Sender, EventArgs e)
        {
            bool boolDuplicate = false;
            int intUser = Int32.Parse(Request.Form[hdnOwner.UniqueID]);
            DataSet ds = oService.GetUser(intService, -1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["userid"].ToString()) == intUser)
                {
                    boolDuplicate = true;
                    break;
                }
            }
            if (boolDuplicate == false)
                oService.AddUser(intService, intUser, -1);
            EditRedirect("&addo=true", false);
        }
        protected void btnApprove_Click(Object Sender, EventArgs e)
        {
            bool boolDuplicate = false;
            int intUser = Int32.Parse(Request.Form[hdnApprove.UniqueID]);
            DataSet ds = oService.GetUser(intService, -10);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["userid"].ToString()) == intUser)
                {
                    boolDuplicate = true;
                    break;
                }
            }
            if (boolDuplicate == false)
                oService.AddUser(intService, intUser, -10);
            EditRedirect("&adda=true", false);
        }
        protected void btnDeleteM_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oService.DeleteUser(Int32.Parse(oDelete.CommandArgument));
            EditRedirect("&delm=true", false);
        }
        protected void btnDeleteT_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oService.DeleteUser(Int32.Parse(oDelete.CommandArgument));
            EditRedirect("&delt=true", false);
        }
        protected void btnDeleteO_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oService.DeleteUser(Int32.Parse(oDelete.CommandArgument));
            EditRedirect("&delo=true", false);
        }
        protected void btnDeleteA_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oService.DeleteUser(Int32.Parse(oDelete.CommandArgument));
            EditRedirect("&dela=true", false);
        }
        protected void btnAccess_Click(Object Sender, EventArgs e)
        {
            bool boolDuplicate = false;
            int intUser = Int32.Parse(Request.Form[hdnAccess.UniqueID]);
            DataSet ds = oService.GetRestrictions(intService);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["userid"].ToString()) == intUser)
                {
                    boolDuplicate = true;
                    break;
                }
            }
            if (boolDuplicate == false)
                oService.AddRestriction(intService, intUser);
            EditRedirect("&addr=true", false);
        }
        protected void btnDeleteR_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oService.DeleteRestriction(Int32.Parse(oDelete.CommandArgument));
            EditRedirect("&delr=true", false);
        }
        protected void EditRedirect(string _end, bool _next)
        {
            string strEdit = "";
            if (Request.QueryString["edit"] != null && _next == false)
                strEdit = "&edit=" + Request.QueryString["edit"];
            Response.Redirect(oPage.GetFullLink(intPage) + "?sid=" + intService.ToString() + "&menu_tab=3" + strEdit + _end);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage));
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?sid=0&menu_tab=3");
        }
    }
}