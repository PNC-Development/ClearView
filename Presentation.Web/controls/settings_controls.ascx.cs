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
using System.DirectoryServices;
namespace NCC.ClearView.Presentation.Web
{
    public partial class settings_controls : System.Web.UI.UserControl
    {

        private DataSet ds;
        protected Pages oPage;
        protected Users oUser;
        protected Variables oVariable;
        private NCC.ClearView.Application.Core.Roles oRole;
        protected Functions oFunction;
        protected Users_At oUserAt;
        protected Vacation oVacation;
        protected Settings oSetting;
        protected Delegates oDelegate;
        protected Applications oApplication;
        protected AD oAD;

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected string strMenu;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected int intCount = 1;
        protected int intModify = 0;
        protected bool boolShowImage = false;
        protected string strYes = "<img src=\"/images/check_small.gif\" border=\"0\" align=\"absmiddle\">";
        protected string strNo = "<img src=\"/images/cancel.gif\" border=\"0\" align=\"absmiddle\">";

        protected string strHTML = "";
        protected int intOwner = 0;
        protected int intAppOwnerId = 0;
        protected Tab oTab = null;
        protected string strMenuTab1 = "";
        private string strEMailIdsBCC = "";
        private int intMenuResourceReporting = 5;
        private int intMenuApplicationManagement = 7;


      

        protected void Page_Load(object sender, EventArgs e)
        {

            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oRole = new NCC.ClearView.Application.Core.Roles(intProfile, dsn);
            oUserAt = new Users_At(intProfile, dsn);
            oVacation = new Vacation(intProfile, dsn);
            oSetting = new Settings(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oAD = new AD(intProfile, dsn, intEnvironment);


            //Menus
            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
            
            oTab = new Tab(hdnType.ClientID, intMenuTab, "divMenu1", true, false);

            oTab.AddTab("My Information", "");
            oTab.AddTab("My Contact Information", "");
            oTab.AddTab("My Scheduled Time Off", "");
            oTab.AddTab("Out of Office Buddies", "");
            oTab.AddTab("My Direct Reports", "");
            oTab.AddTab("My Direct Reports Scheduled Time Off", "");
            oTab.AddTab("Application Management", "");
            
            strMenuTab1 = oTab.GetTabs();

            //End Menus

            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);

           
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            string strTitle = oPage.Get(intPage, "title");

            lblTitle.Text = strTitle;
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intModify = Int32.Parse(Request.QueryString["id"]);

            if (!IsPostBack)
            {
                LoadResourceReporting();

                LoadList();
               
                if (Request.QueryString["action"] != null && Request.QueryString["action"] != "")
                {
                    trSaved.Visible = true;
                    switch (Request.QueryString["action"])
                    {
                        case "profile":
                            lblSaved.Text = "<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Profile Information Saved";
                            break;
                        case "report":
                            lblSaved.Text = "<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Direct Report Added";
                            break;
                        case "delete":
                            lblSaved.Text = "<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Direct Report Removed";
                            break;
                        case "error":
                            lblSaved.Text = "<img src='/images/bigX.gif' border='0' align='absmiddle' /> Account Already Exists";
                            break;
                        case "same":
                            lblSaved.Text = "<img src='/images/bigX.gif' border='0' align='absmiddle' /> You Cannot Modify the Properties of Your Account";
                            break;
                        case "vacation":
                            lblSaved.Text = "<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Event Deleted";
                            break;
                        case "addbuddy":
                            lblSaved.Text = "<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Out of Office Buddy Added";
                            break;
                        case "buddy":
                            lblSaved.Text = "<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Out of Office Buddy Removed";
                            break;
                        case "approve":
                            lblSaved.Text = "<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Vacation Request Approved";
                            break;
                        case "deny":
                            lblSaved.Text = "<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Vacation Request Denied";
                            break;
                       case "contactinfo":
                             lblSaved.Text = "<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Contact Information Saved";
                            break;
                    }
                }
                ds = oUser.Get(intProfile);
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["enabled"].ToString() == "1")
                {
                    
                    bool boolManager = (ds.Tables[0].Rows[0]["ismanager"].ToString() == "1");
                    bool boolLoadProfile = true;
                    if (intModify > 0)
                    {
                        ds = oUser.Get(intModify);
                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["enabled"].ToString() == "1")
                        {
                            txtManagerUser.Text = ds.Tables[0].Rows[0]["xid"].ToString();
                            if (boolShowImage == true)
                            {
                                imgManagerHeader.Width = Unit.Pixel(90);
                                imgManagerHeader.Height = Unit.Pixel(90);
                                imgManagerHeader.ImageUrl = "/frame/picture.aspx?xid=" + ds.Tables[0].Rows[0]["xid"].ToString();
                            }
                            else
                                imgManagerHeader.ImageUrl = "/images/profile.gif";
                            txtManagerPNC.Text = ds.Tables[0].Rows[0]["pnc_id"].ToString();
                            if (txtManagerPNC.Text == txtManagerUser.Text)
                            {
                                // User is a PNC employee, hide the PNC ID
                                rowManagerPNC.Visible = false;
                            }
                            else if (txtManagerPNC.Text == "")
                                txtManagerPNC.Enabled = true;
                            txtManagerFirst.Text = ds.Tables[0].Rows[0]["fname"].ToString();
                            txtManagerLast.Text = ds.Tables[0].Rows[0]["lname"].ToString();
                            lblHeader.Text = "(" + ds.Tables[0].Rows[0]["fname"].ToString() + " " + ds.Tables[0].Rows[0]["lname"].ToString() + ")";
                            lblVacationName.Text = "(" + ds.Tables[0].Rows[0]["fname"].ToString() + " " + ds.Tables[0].Rows[0]["lname"].ToString() + ")";
                            int intManager = Int32.Parse(ds.Tables[0].Rows[0]["manager"].ToString());
                            if (intManager > 0)
                                lblManagerReports.Text = oUser.GetFullName(intManager) + " (" + oUser.GetName(intManager) + ")";
                            else
                                lblManagerReports.Text = "None";
                            txtManagerVacation.Text = ds.Tables[0].Rows[0]["vacation"].ToString();
                            chkManagerIsManager.Checked = (ds.Tables[0].Rows[0]["ismanager"].ToString() == "1");
                            lblManagerBoard.Text = (ds.Tables[0].Rows[0]["board"].ToString() == "1" ? strYes + "&nbsp;You are a member of the board" : strNo + "&nbsp;You are <i>not</i> a member of the board");
                            lblManagerDirector.Text = (ds.Tables[0].Rows[0]["director"].ToString() == "1" ? strYes + "&nbsp;You are a director" : strNo + "&nbsp;You are <i>not</i> a director");
                            txtManagerPager.Text = ds.Tables[0].Rows[0]["pager"].ToString();
                            ddlManagerUserAt.SelectedValue = ds.Tables[0].Rows[0]["atid"].ToString();
                            txtManagerPhone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                            chkManagerShowReturns.Checked = (ds.Tables[0].Rows[0]["show_returns"].ToString() == "1");
                            chkManagerUngroupProjects.Checked = (ds.Tables[0].Rows[0]["ungroup_projects"].ToString() == "1");
                            txtManagerSkills.Text = ds.Tables[0].Rows[0]["other"].ToString();
                            imgManagerPicture.ImageUrl = "/frame/picture.aspx?xid=" + ds.Tables[0].Rows[0]["xid"].ToString();
                            imgManagerPicture.Style["border"] = "solid 1px #999999";
                            panManagerProfile.Visible = true;
                            boolLoadProfile = false;
                            ds = oVacation.Gets(intModify, DateTime.Today.Year);
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                if (dr["vacation"].ToString() == "1")
                                    dr["reason"] = "Vacation";
                                else if (dr["holiday"].ToString() == "1")
                                    dr["reason"] = "Floating Holiday";
                                else if (dr["personal"].ToString() == "1")
                                    dr["reason"] = "Personal / Sick Day";
                                if (dr["morning"].ToString() == "1")
                                    dr["duration"] = "Morning";
                                else if (dr["afternoon"].ToString() == "1")
                                    dr["duration"] = "Afternoon";
                                else
                                    dr["duration"] = "Full Day";
                            }
                            lblNone.Visible = (ds.Tables[0].Rows.Count == 0);
                            rptVacation.DataSource = ds;
                            rptVacation.DataBind();
                            foreach (RepeaterItem ri in rptVacation.Items)
                                ((LinkButton)ri.FindControl("btnDeleteVacation")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this event?');");
                            // panVacation.Visible = true;
                        }
                    }
                    if (boolLoadProfile == true)
                    {
                        ds = oUser.Get(intProfile);
                        txtUser.Text = ds.Tables[0].Rows[0]["xid"].ToString();
                        if (boolShowImage == true)
                        {
                            imgHeader.Width = Unit.Pixel(90);
                            imgHeader.Height = Unit.Pixel(90);
                            imgHeader.ImageUrl = "/frame/picture.aspx?xid" + ds.Tables[0].Rows[0]["xid"].ToString();
                        }
                        else
                            imgHeader.ImageUrl = "/images/profile.gif";
                        txtPNC.Text = ds.Tables[0].Rows[0]["pnc_id"].ToString();
                        if (txtPNC.Text == txtUser.Text)
                        {
                            // User is a PNC employee, hide the PNC ID
                            rowPNC.Visible = false;
                        }
                        else if (txtPNC.Text == "")
                            txtPNC.Enabled = true;
                        txtFirst.Text = ds.Tables[0].Rows[0]["fname"].ToString();
                        txtLast.Text = ds.Tables[0].Rows[0]["lname"].ToString();
                        int intManager = Int32.Parse(ds.Tables[0].Rows[0]["manager"].ToString());
                        if (intManager > 0)
                        {
                            hdnManager.Value = intManager.ToString();
                            pnlManager.Visible = false;
                            lblManager.Visible = true;
                            lblManager.Text = oUser.GetFullName(intManager) + " (" + oUser.GetName(intManager) + ")";
                        }
                        else
                        {
                            hdnManager.Value = intManager.ToString();
                            pnlManager.Visible = true;
                            lblManager.Visible = false;
                            lblManager.Text = "None";
                        }
                        lblVacation.Text = ds.Tables[0].Rows[0]["vacation"].ToString();
                        if (lblVacation.Text == "")
                            lblVacation.Text = "Not Configured";
                        lblIsManager.Text = (ds.Tables[0].Rows[0]["ismanager"].ToString() == "1" ? strYes + "&nbsp;You are a manager" : strNo + "&nbsp;You are <i>not</i> a manager");
                        hdnIsManager.Value = ds.Tables[0].Rows[0]["ismanager"].ToString();
                        if (ds.Tables[0].Rows[0]["ismanager"].ToString() == "1")
                            btnIsManager.Visible = false;
                        lblBoard.Text = (ds.Tables[0].Rows[0]["board"].ToString() == "1" ? strYes + "&nbsp;You are a member of the board" : strNo + "&nbsp;You are <i>not</i> a member of the board");
                        lblDirector.Text = (ds.Tables[0].Rows[0]["director"].ToString() == "1" ? strYes + "&nbsp;You are a director" : strNo + "&nbsp;You are <i>not</i> a director");
                        txtPager.Text = ds.Tables[0].Rows[0]["pager"].ToString();
                        ddlUserAt.SelectedValue = ds.Tables[0].Rows[0]["atid"].ToString();
                        txtPhone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                        chkShowReturns.Checked = (ds.Tables[0].Rows[0]["show_returns"].ToString() == "1");
                        chkUngroupProjects.Checked = (ds.Tables[0].Rows[0]["ungroup_projects"].ToString() == "1");
                        txtSkills.Text = ds.Tables[0].Rows[0]["other"].ToString();
                        imgPicture.ImageUrl = "/frame/picture.aspx?xid=" + ds.Tables[0].Rows[0]["xid"].ToString();
                        imgPicture.Style["border"] = "solid 1px #999999";
                        btnPicture.Attributes.Add("onclick", "return OpenWindow('IMAGE','');");
                        panProfile.Visible = true;

                        DateTime dtLastModified = DateTime.Parse(ds.Tables[0].Rows[0]["modified"].ToString());
                        DateTime dtNextUpdates = dtLastModified.AddMonths(3);
                        if (dtNextUpdates <= DateTime.Now)  //Check for 3 Months
                            pnlInfo.Visible = true;

                        ucUserContactInfo.UserId = intProfile;
                        //LoadUserContactDetails(intProfile);
                       
                        ds = oVacation.Gets(intProfile, DateTime.Today.Year);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr["vacation"].ToString() == "1")
                                dr["reason"] = "Vacation";
                            else if (dr["holiday"].ToString() == "1")
                                dr["reason"] = "Floating Holiday";
                            else if (dr["personal"].ToString() == "1")
                                dr["reason"] = "Personal / Sick Day";
                            if (dr["morning"].ToString() == "1")
                                dr["duration"] = "Morning";
                            else if (dr["afternoon"].ToString() == "1")
                                dr["duration"] = "Afternoon";
                            else
                                dr["duration"] = "Full Day";
                        }
                        lblNone.Visible = (ds.Tables[0].Rows.Count == 0);
                        rptVacation.DataSource = ds;
                        rptVacation.DataBind();
                        foreach (RepeaterItem ri in rptVacation.Items)
                            ((LinkButton)ri.FindControl("btnDeleteVacation")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this event?');");
                        //panVacation.Visible = true;

                        // Load Buddies
                        rptBuddies.DataSource = oDelegate.Gets(intProfile);
                        rptBuddies.DataBind();
                        lblNoBuddies.Visible = (rptBuddies.Items.Count == 0);
                        foreach (RepeaterItem ri in rptBuddies.Items)
                            ((LinkButton)ri.FindControl("btnDeleteBuddy")).Attributes.Add("onclick", "return confirm('Are you sure you want to remove yourself as an out of office buddy for this person?');");
                        btnAddBuddy.Attributes.Add("onclick", "return OpenWindow('DELEGATE','?id=" + intProfile.ToString() + "&parent=" + intPage.ToString() + "');");
                        // Load Covering
                        rptCovering.DataSource = oDelegate.GetDelegations(intProfile);
                        rptCovering.DataBind();
                        lblCovering.Visible = (rptCovering.Items.Count == 0);
                        foreach (RepeaterItem ri in rptCovering.Items)
                            ((LinkButton)ri.FindControl("btnDeleteBuddy")).Attributes.Add("onclick", "return confirm('Are you sure you want to remove this buddy?');");

                        // panDeligates.Visible = true;
                    }
                    if (boolManager == true)
                    {
                        DataColumn oColumn;

                        ds = oUser.GetManagerReports(intProfile, 0, 0, 0);
                        DataTable oReports = new DataTable();
                        //                    DataColumn oColumn;
                        oColumn = new DataColumn();
                        oColumn.DataType = System.Type.GetType("System.Int32");
                        oColumn.ColumnName = "vacationid";
                        oReports.Columns.Add(oColumn);
                        oColumn = new DataColumn();
                        oColumn.DataType = System.Type.GetType("System.Int32");
                        oColumn.ColumnName = "userid";
                        oReports.Columns.Add(oColumn);
                        oColumn = new DataColumn();
                        oColumn.DataType = System.Type.GetType("System.String");
                        oColumn.ColumnName = "start_date";
                        oReports.Columns.Add(oColumn);
                        oColumn = new DataColumn();
                        oColumn.DataType = System.Type.GetType("System.String");
                        oColumn.ColumnName = "duration";
                        oReports.Columns.Add(oColumn);
                        oColumn = new DataColumn();
                        oColumn.DataType = System.Type.GetType("System.String");
                        oColumn.ColumnName = "reason";
                        oReports.Columns.Add(oColumn);
                        foreach (DataRow drVacation in ds.Tables[0].Rows)
                        {
                            int intTemp = Int32.Parse(drVacation["userid"].ToString());
                            DataSet dsReportsVacation = oVacation.Gets(intTemp, DateTime.Today.Year);
                            foreach (DataRow dr in dsReportsVacation.Tables[0].Rows)
                            {
                                if (dr["approved"].ToString() == "0")
                                {
                                    DataRow oRow = oReports.NewRow();
                                    oRow["vacationid"] = dr["vacationid"].ToString();
                                    oRow["userid"] = dr["userid"].ToString();
                                    oRow["start_date"] = dr["start_date"].ToString();
                                    oRow["duration"] = dr["duration"].ToString();
                                    if (dr["vacation"].ToString() == "1")
                                        oRow["reason"] = "Vacation";
                                    else if (dr["holiday"].ToString() == "1")
                                        oRow["reason"] = "Floating Holiday";
                                    else if (dr["personal"].ToString() == "1")
                                        oRow["reason"] = "Personal / Sick Day";
                                    if (dr["morning"].ToString() == "1")
                                        oRow["duration"] = "Morning";
                                    else if (dr["afternoon"].ToString() == "1")
                                        oRow["duration"] = "Afternoon";
                                    else
                                        oRow["duration"] = "Full Day";
                                    oReports.Rows.Add(oRow);
                                }
                            }
                        }
                        rptReportsVacation.DataSource = oReports;
                        rptReportsVacation.DataBind();
                        foreach (RepeaterItem ri in rptReportsVacation.Items)
                        {
                            LinkButton oApprove = (LinkButton)ri.FindControl("btnApprove");
                            oApprove.Attributes.Add("onclick", "return confirm('Are you sure you want to APPROVE this request?');");
                            LinkButton oDeny = (LinkButton)ri.FindControl("btnDeny");
                            oDeny.Attributes.Add("onclick", "return confirm('Are you sure you want to DENY this request?');");
                        }
                        lblReports.Visible = (rptReportsVacation.Items.Count == 0);
                    }
                }
                else
                    panError.Visible = true;
            }
            txtSkills.Attributes.Add("onkeypress", "return CancelEnter();");
          
            btnProfile.Attributes.Add("onclick", "return ValidateText('" + txtUser.ClientID + "','Please enter a user ID')" +
                " && ValidateText('" + txtFirst.ClientID + "','Please enter a first name')" +
                " && ValidateText('" + txtLast.ClientID + "','Please enter a last name')" +
                //" && ValidateText('" + txtPager.ClientID + "','Please enter a pager number')" +
                //" && ValidateDropDown('" + ddlUserAt.ClientID + "','Please select a type of pager')" +
                " && ProcessButton(this)" +
                ";");
            btnManager.Attributes.Add("onclick", "return ValidateText('" + txtManagerUser.ClientID + "','Please enter a user ID')" +
                " && ValidateText('" + txtManagerFirst.ClientID + "','Please enter a first name')" +
                " && ValidateText('" + txtManagerLast.ClientID + "','Please enter a last name')" +
                //" && ValidateText('" + txtManagerPager.ClientID + "','Please enter a pager number')" +
                //" && ValidateDropDown('" + ddlManagerUserAt.ClientID + "','Please select a type of pager')" +
                " && ValidateNumber('" + txtManagerVacation.ClientID + "','Please enter a valid number of vacation days')" +
                " && ProcessButton(this)" +
                ";");
            btnManagerUpdate.Attributes.Add("onclick", "return ValidateText('" + txtPNC.ClientID + "','Please enter your PNC ID') && confirm('NOTE: ClearView will now attempt to retrieve your manager from eDirectory.\\n\\nIf successful and correct, you will need to click the \"Save Profile\" button to update your manager.') && ProcessButton(this);");
            btnIsManager.Attributes.Add("onclick", "return confirm('NOTE: ClearView will now attempt to connect to eDirectory to verify you have direct reports.\\n\\nIf you do, you will need to click the \"Save Profile\" button to update this change.') && ProcessButton(this);");
           
            btnClose.Attributes.Add("onclick", "return CloseWindow();");

            txtManager.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divManager.ClientID + "','" + lstManager.ClientID + "','" + hdnManager.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstManager.Attributes.Add("ondblclick", "AJAXClickRow();");
           
            txtAppResource.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAppResource.ClientID + "','" + lstAppResource.ClientID + "','" + hdnAppResource.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstAppResource.Attributes.Add("ondblclick", "AJAXClickRow();");
            btnAddAppResource.Attributes.Add("onclick", "return ValidateHidden('" + hdnAppResource.ClientID + "','" + txtAppResource.ClientID + "','Please select the user');");


            txtReportingUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divReportingUser.ClientID + "','" + lstReportingUser.ClientID + "','" + hdnReportingUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstReportingUser.Attributes.Add("ondblclick", "AJAXClickRow();");
            btnAddReportingUser.Attributes.Add("onclick", "return ValidateHidden('" + hdnReportingUser.ClientID + "','" + txtReportingUser.ClientID + "','Please select the user');");

        }

        
        private void LoadList()
        {

            ddlApplication.DataSource = oRole.Gets(intProfile);
            ddlApplication.DataTextField = "name";
            ddlApplication.DataValueField = "applicationid";
            ddlApplication.DataBind();
            ddlApplication.Items.Insert(0, new ListItem("-- SELECT --", "-1"));

            DataSet ds = oUserAt.Gets(1);
            ddlUserAt.DataTextField = "name";
            ddlUserAt.DataValueField = "atid";
            ddlUserAt.DataSource = ds;
            ddlUserAt.DataBind();
            ddlUserAt.Items.Insert(0, new ListItem("-- SELECT --", "0"));

            ddlManagerUserAt.DataTextField = "name";
            ddlManagerUserAt.DataValueField = "atid";
            ddlManagerUserAt.DataSource = ds;
            ddlManagerUserAt.DataBind();
            ddlManagerUserAt.Items.Insert(0, new ListItem("-- SELECT --", "0"));

        }

        protected void btnProfile_Click(Object Sender, EventArgs e)
        {
            ds = oUser.Get(intProfile);
            //int intManger = Int32.Parse(ds.Tables[0].Rows[0]["manager"].ToString());
            string strManager = Request.Form[hdnManager.UniqueID];
            int intManager = 0;
            // Possible that Request.Form[hdnManager.UniqueID] gave us a null value.  Try getting it another way
            if (strManager == null)
                strManager = hdnManager.Value;

            // Two options...either it has the UserID of the manager, or the P-ID of the manager
            // First, see if it's the UserID
            if (Int32.TryParse(strManager, out intManager) == false)
            {
                // We know it's NOT the UserID - see if it's the P-ID
                if (strManager != null && strManager != "")
                {
                    // Manager does not exist in ClearView, but does in eDirectory.
                    int intGroup = Int32.Parse(ConfigurationManager.AppSettings["CLEARVIEW_USER_GROUPID"]);
                    intManager = oUser.Register(strManager, intGroup, intEnvironment);
                }
            }
            if (intManager == 0)
                Int32.TryParse(ds.Tables[0].Rows[0]["manager"].ToString(), out intManager);
            int intIsManager = (ds.Tables[0].Rows[0]["ismanager"].ToString() == "1" ? 1 : 0);
            if (intIsManager == 0)
                Int32.TryParse(Request.Form[hdnIsManager.UniqueID], out intIsManager);
            int intBoard = (ds.Tables[0].Rows[0]["board"].ToString() == "1" ? 1 : 0);
            int intDirector = (ds.Tables[0].Rows[0]["director"].ToString() == "1" ? 1 : 0);
            int intMultiple = (ds.Tables[0].Rows[0]["multiple_apps"].ToString() == "1" ? 1 : 0);
            int intLocation = (ds.Tables[0].Rows[0]["add_location"].ToString() == "1" ? 1 : 0);
            int intAdmin = (ds.Tables[0].Rows[0]["admin"].ToString() == "1" ? 1 : 0);
            oUser.Update(intProfile, txtUser.Text, txtPNC.Text, txtFirst.Text, txtLast.Text, intManager, intIsManager, intBoard, intDirector, txtPager.Text, Int32.Parse(ddlUserAt.SelectedItem.Value), txtPhone.Text, txtSkills.Text, Int32.Parse(lblVacation.Text), intMultiple, (chkUngroupProjects.Checked ? 1 : 0), (chkShowReturns.Checked ? 1 : 0), intLocation, intAdmin, 1);
            Response.Redirect(oPage.GetFullLink(intPage) + "?action=profile");
        }

        protected void btnManager_Click(Object Sender, EventArgs e)
        {
            ds = oUser.Get(intModify);
            int intManger = Int32.Parse(ds.Tables[0].Rows[0]["manager"].ToString());
            int intIsManager = (ds.Tables[0].Rows[0]["ismanager"].ToString() == "1" ? 1 : 0);
            int intBoard = (ds.Tables[0].Rows[0]["board"].ToString() == "1" ? 1 : 0);
            int intDirector = (ds.Tables[0].Rows[0]["director"].ToString() == "1" ? 1 : 0);
            int intMultiple = (ds.Tables[0].Rows[0]["multiple_apps"].ToString() == "1" ? 1 : 0);
            int intLocation = (ds.Tables[0].Rows[0]["add_location"].ToString() == "1" ? 1 : 0);
            int intAdmin = (ds.Tables[0].Rows[0]["admin"].ToString() == "1" ? 1 : 0);
            oUser.Update(intModify, txtManagerUser.Text, txtManagerPNC.Text, txtManagerFirst.Text, txtManagerLast.Text, intManger, intIsManager, intBoard, intDirector, txtManagerPager.Text, Int32.Parse(ddlManagerUserAt.SelectedItem.Value), txtManagerPhone.Text, txtManagerSkills.Text, Int32.Parse(txtManagerVacation.Text), intMultiple, (chkManagerUngroupProjects.Checked ? 1 : 0), (chkManagerShowReturns.Checked ? 1 : 0), intLocation, intAdmin, 1);
            Response.Redirect(oPage.GetFullLink(intPage) + "?action=profile");
        }

        protected void btnManagerUpdate_Click(Object Sender, EventArgs e)
        {
            int intManager = 0;
            SearchResultCollection oCollection = oFunction.eDirectory(txtPNC.Text);
            if (oCollection.Count == 1 && oCollection[0].Properties.Contains("pncmanagerid") == true)
            {
                string strManager = oFunction.eDirectory(oCollection[0], "pncmanagerid");
                DataSet dsManager = oUser.Gets(strManager);
                if (dsManager.Tables[0].Rows.Count == 1)
                {
                    intManager = Int32.Parse(dsManager.Tables[0].Rows[0]["userid"].ToString());
                    hdnManager.Value = intManager.ToString();
                    lblManager.Text = oUser.GetFullName(intManager) + " (" + oUser.GetName(intManager) + ")";
                }
                else
                {
                    hdnManager.Value = oFunction.eDirectory(oCollection[0], "pncmanagerid");
                    lblManager.Text = oFunction.eDirectory(oCollection[0], "pncmanagername") + " (" + oFunction.eDirectory(oCollection[0], "pncmanagerid") + ")";
                }
            }
            if (intManager == 0)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('There was a problem retrieving your manager from eDirectory.\\nPlease submit an issue request (located under the support module).');<" + "/" + "script>");
            else
            {
                string strManager = lblManager.Text;
                while (strManager.Contains("'"))
                    strManager = strManager.Replace("'", "");
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('ClearView has been informed that your manager is " + strManager + ".\\n\\nIf correct, you need to click \"Save Profile\" to update ClearView.');<" + "/" + "script>");
            }
        }

        protected void btnIsManager_Click(Object Sender, EventArgs e)
        {
            SearchResultCollection oCollection = oFunction.eDirectory("pncmanagerid", txtPNC.Text);
            if (oCollection.Count > 0)
            {
                hdnIsManager.Value = "1";
                lblIsManager.Text = strYes + "&nbsp;You are a manager";
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('ClearView has been informed that you have " + oCollection.Count + " direct reports.\\n\\nOnce you click \"Save Profile\", you will be configured as a manager.');<" + "/" + "script>");
            }
            else
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('There was a problem retrieving your information from eDirectory.\\nPlease submit an issue request (located under the support module).');<" + "/" + "script>");
        }

        protected void btnApprove_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            int intVacation = Int32.Parse(oButton.CommandArgument);
            oVacation.Update(intVacation, 1);
            int intUser = Int32.Parse(oVacation.Get(intVacation, "userid"));
            strEMailIdsBCC = "";

            oFunction.SendEmail("ClearView Out of Office Request", oUser.GetName(intUser), "", strEMailIdsBCC, "ClearView Out of Office Request", "<p><b>The following out of office request has been APPROVED by your MANAGER (" + oUser.GetFullName(intProfile) + ")</b><p><p>" + oVacation.GetBody(intVacation, intEnvironment) + "</p>", true, false);
            Response.Redirect(oPage.GetFullLink(intPage) + "?action=approve&menu_tab=5");
        }
        protected void btnDeny_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            int intVacation = Int32.Parse(oButton.CommandArgument);
            oVacation.Update(intVacation, -1);
            int intUser = Int32.Parse(oVacation.Get(intVacation, "userid"));
            strEMailIdsBCC = "";
            oFunction.SendEmail("ClearView Out of Office Request", oUser.GetName(intUser), "", strEMailIdsBCC, "ClearView Out of Office Request", "<p><b>The following out of office request has been DENIED by your MANAGER (" + oUser.GetFullName(intProfile) + ")</b><p><p>" + oVacation.GetBody(intVacation, intEnvironment) + "</p>", true, false);
            Response.Redirect(oPage.GetFullLink(intPage) + "?action=deny&menu_tab=5");
        }
        protected void btnDeleteVacation_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oVacation.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(oPage.GetFullLink(intPage) + "?action=vacation&menu_tab=2");
        }
        protected void btnDeleteBuddy_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oDelegate.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(oPage.GetFullLink(intPage) + "?action=buddy&menu_tab=3");
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage));
        }

        #region Resource Reporting Structure

        private void LoadResourceReporting()
            {
                bool boolManager=false;
                tvResourceReporting.Nodes.Clear();

                DataSet dsResourceReportingHierarchy = oUser.GetUserReportingHierarchy(intProfile,0);

               ds = oUser.Get(intProfile);
               if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["enabled"].ToString() == "1")
                    boolManager = (ds.Tables[0].Rows[0]["ismanager"].ToString() == "1");

                pnlAddReportingUser.Visible = boolManager;

                //Get the Current Profile User
                DataRow[] drSelect = null;
                drSelect = dsResourceReportingHierarchy.Tables[0].Select("userid = " + intProfile);
                if (drSelect.Length > 0)
                {
                    DataRow dr;
                    dr = drSelect[0];
                    TreeNode oActingNode = new TreeNode();
                    oActingNode.Text =  dr["UserName"].ToString();
                    oActingNode.ToolTip = "  Manager : "+  (dr["manager"].ToString()=="0"?"" :dr["ManagerName"].ToString());
                    oActingNode.Value = dr["UserId"].ToString();
                    oActingNode.SelectAction = TreeNodeSelectAction.Select;
                    oActingNode.Expand();
                    //oActingNode.ImageUrl = "/images/folder.gif";
                    //oActingNode.NavigateUrl = oPage.GetFullLink(intPage) + "?menu_tab=" +intMenuTabOrgChart.ToString() +"&userid=" + dr["UserId"].ToString();
                    
                    //Check for current selected user
                    if (txtReportingSelectedUserValue.Text != "")
                    {
                        if (txtReportingSelectedUserValue.Text == oActingNode.Value)
                        {
                            oActingNode.Selected = true;
                            LoadResourceReportingList(Int32.Parse(oActingNode.Value));
                        }
                    }
                    else
                    {
                        oActingNode.Selected = true;
                        lblReportingSelectedUser.Text = oActingNode.Text;
                        txtReportingSelectedUserValue.Text = oActingNode.Value;
                        LoadResourceReportingList(Int32.Parse(dr["UserId"].ToString()));
                    }

                    LoadResourceReportingHierarchy(dsResourceReportingHierarchy.Tables[0], oActingNode);
                    tvResourceReporting.Nodes.Add(oActingNode);
                    
                }

               

            }

            private void LoadResourceReportingHierarchy(DataTable dt, TreeNode parent)
            {

                DataRow[] drSelect = null;
                drSelect = dt.Select("Manager = " + Int32.Parse(parent.Value));

                foreach (DataRow dr in drSelect)
                {
                    TreeNode oActingNode = new TreeNode();
                    oActingNode.Text = dr["UserName"].ToString();
                    oActingNode.ToolTip = "  Manager : " + dr["ManagerName"].ToString();
                    oActingNode.Value = dr["UserId"].ToString();
                    oActingNode.SelectAction = TreeNodeSelectAction.Select;
                    oActingNode.Expand();

                    //oActingNode.ImageUrl = "/images/folder.gif";
                    //oActingNode.NavigateUrl = oPage.GetFullLink(intPage) + "?menu_tab=" + intMenuTabOrgChart.ToString() +"&userid=" + dr["UserId"].ToString();

                    //Check for current selected user
                    if (txtReportingSelectedUserValue.Text != "")
                    {
                        if (txtReportingSelectedUserValue.Text == oActingNode.Value)
                        {
                            oActingNode.Selected = true;
                            LoadResourceReportingList(Int32.Parse(oActingNode.Value));
                        }
                    }
                   

                    parent.ChildNodes.Add(oActingNode);
                    LoadResourceReportingHierarchy(dt, oActingNode);
                }


            }

            private void LoadResourceReportingList(int intUserId)
            {
                //Get all the resources reporting to resource
                DataSet dsResourceReportingHierarchy = oUser.GetUserReportingHierarchy(intUserId,1);
                DataTable dtResources = dsResourceReportingHierarchy.Tables[0];
                DataRow drCurrentUser=null;
                //Add additional columns for showing Leave Balance
                DataColumn oColumn;
                oColumn = new DataColumn();
                oColumn.DataType = System.Type.GetType("System.String");
                oColumn.ColumnName = "Floating";
                dtResources.Columns.Add(oColumn);
                oColumn = new DataColumn();
                oColumn.DataType = System.Type.GetType("System.String");
                oColumn.ColumnName = "Personal";
                dtResources.Columns.Add(oColumn);
                oColumn = new DataColumn();
                oColumn.DataType = System.Type.GetType("System.String");
                oColumn.ColumnName = "Vacation";
                dtResources.Columns.Add(oColumn);

                //Get the vacation/leave balance for each resource
                foreach (DataRow drVacation in dtResources.Rows)
                {   
                    if (Int32.Parse( drVacation["userid"].ToString()) ==intProfile)
                        drCurrentUser=drVacation;

                    int intTmpUser = Int32.Parse(drVacation["userid"].ToString());
                    DataSet dsVacation = oVacation.Gets(intTmpUser, DateTime.Today.Year);
                    double dblFloating = double.Parse(oSetting.Get("floating"));
                    double dblPersonal = double.Parse(oSetting.Get("personal"));
                    string strVacation = oUser.Get(intTmpUser, "vacation");
                    double dblVacation = 0.00;


                    if (strVacation != "")
                    {
                        dblVacation = double.Parse(strVacation);
                        foreach (DataRow dr in dsVacation.Tables[0].Rows)
                        {
                            if (dr["vacation"].ToString() == "1")
                            {
                                if (dr["morning"].ToString() == "1")
                                    dblVacation = dblVacation - .5;
                                else if (dr["afternoon"].ToString() == "1")
                                    dblVacation = dblVacation - .5;
                                else
                                    dblVacation = dblVacation - 1;
                            }
                            else if (dr["holiday"].ToString() == "1")
                            {
                                if (dr["morning"].ToString() == "1")
                                    dblFloating = dblFloating - .5;
                                else if (dr["afternoon"].ToString() == "1")
                                    dblFloating = dblFloating - .5;
                                else
                                    dblFloating = dblFloating - 1;
                            }
                            else if (dr["personal"].ToString() == "1")
                            {
                                if (dr["morning"].ToString() == "1")
                                    dblPersonal = dblPersonal - .5;
                                else if (dr["afternoon"].ToString() == "1")
                                    dblPersonal = dblPersonal - .5;
                                else
                                    dblPersonal = dblPersonal - 1;
                            }
                        }
                    }
                    drVacation["Floating"] = dblFloating.ToString();
                    drVacation["Personal"] = dblPersonal.ToString();
                    drVacation["Vacation"] = dblVacation.ToString();
                 
                }

                
                //if (drCurrentUser!=null)
                //    dtResources.Rows.Remove(drCurrentUser);
                dlResourceList.DataSource = dtResources;
                dlResourceList.DataBind();


            }

            protected void tvResourceReporting_TreeNodePopulate(object sender, TreeNodeEventArgs e)
            {

                //if (e.Node.Value != "")
                //{
                //    LoadResourceReportingList(Int32.Parse(e.Node.Value));

                //}
            }

            protected void tvResourceReporting_SelectedNodeChanged(object sender, EventArgs e)
            {
                if (tvResourceReporting.SelectedNode != null)
                {
                    lblReportingSelectedUser.Text = tvResourceReporting.SelectedNode.Text;
                    txtReportingSelectedUserValue.Text = tvResourceReporting.SelectedNode.Value;

                    LoadResourceReportingList(Int32.Parse(tvResourceReporting.SelectedNode.Value));
      
                    oTab.SelectedTab = intMenuResourceReporting;
                    strMenuTab1 = oTab.GetTabs();
                }
            }

            protected void dlResourceList_ItemDataBound(object sender, DataListItemEventArgs e)
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView drv = (DataRowView)e.Item.DataItem;

                    Label lblRLUserName = (Label)e.Item.FindControl("lblRLUserName");
                    lblRLUserName.Text = drv["UserName"].ToString();

                    Label lblRLVacation = (Label)e.Item.FindControl("lblRLVacation");
                    lblRLVacation.Text = drv["Vacation"].ToString();

                    Label lblRLPersonal = (Label)e.Item.FindControl("lblRLPersonal");
                    lblRLPersonal.Text = drv["Personal"].ToString();

                    Label lblRLFloating = (Label)e.Item.FindControl("lblRLFloating");
                    lblRLFloating.Text = drv["Floating"].ToString();

                    LinkButton lnkbtnRLEdit = (LinkButton)e.Item.FindControl("lnkbtnRLEdit");
                    lnkbtnRLEdit.CommandArgument = drv["UserID"].ToString();
                    if (drv["UserID"].ToString() == intProfile.ToString())
                        lnkbtnRLEdit.Enabled = false;
                    
                    LinkButton lnkbtnRLRemove = (LinkButton)e.Item.FindControl("lnkbtnRLRemove");
                    lnkbtnRLRemove.CommandArgument = drv["UserID"].ToString();

                    if (drv["UserID"].ToString() == intProfile.ToString())
                        lnkbtnRLRemove.Enabled = false;
                    else
                    {
                        lnkbtnRLRemove.Enabled = true;
                        lnkbtnRLRemove.Attributes.Add("onclick", "return confirm('Are you sure you want to remove this employee?');");
                    }

                }
            }

            protected void btnAddReportingUser_Click(Object Sender, EventArgs e)
            {
                if (tvResourceReporting.SelectedNode != null)
                {
                    int intSelectedUser = Int32.Parse(txtReportingSelectedUserValue.Text);
                    int intReportingUser = Int32.Parse(hdnReportingUser.Value);
                    if (intReportingUser != Int32.Parse(txtReportingSelectedUserValue.Text))
                    {
                        oUser.Update(intReportingUser, Int32.Parse(txtReportingSelectedUserValue.Text));
                       
                        txtReportingUser.Text = "";
                        hdnReportingUser.Value = "";
                        oFunction.SendEmail("ClearView : Added to reporting list of " + oUser.GetFullName(intSelectedUser), oUser.GetName(intReportingUser), "", strEMailIdsBCC, "ClearView : Added to reporting list of " + oUser.GetFullName(intSelectedUser), "<p><b>" + oUser.GetFullName(intReportingUser) + " (USER ID: " + intReportingUser.ToString() + ") has been added to the reporting list of " + oUser.GetFullName(intSelectedUser) + ".</b>" +
                                  "<p></p><p><a href=\"" + oVariable.URL() + oPage.GetFullLink(intPage) + "\"" + ">Click here to view your settings.</a></p>", true, false);

                        LoadResourceReporting();
                        oTab.SelectedTab = intMenuResourceReporting;
                        strMenuTab1 = oTab.GetTabs();

                    }
                }
              
            }

            protected void lnkbtnRLEdit_Click(Object Sender, EventArgs e)
            {
                LinkButton oButton = (LinkButton)Sender;
                if (Int32.Parse(oButton.CommandArgument) == intProfile)
                    Response.Redirect(oPage.GetFullLink(intPage) + "?action=same");
                else
                    Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + oButton.CommandArgument + "&menu_tab=1");
            }

            protected void lnkbtnRLRemove_Click(Object Sender, EventArgs e)
            {
                int intSelectedUser = Int32.Parse(txtReportingSelectedUserValue.Text);
                LinkButton oButton = (LinkButton)Sender;
                int intUser = Int32.Parse(oButton.CommandArgument);
                int intManager =Int32.Parse(oUser.Get(intUser, "manager"));

                oUser.Update(intUser, 0);
                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");


                string strDefault = oUser.GetApplicationUrl(intUser, intPage);
                string strRedirect = "/" + strDefault + oPage.GetFullLink(intPage);
                oFunction.SendEmail("ClearView : Removal from reporting list of " + oUser.GetFullName(intManager), oUser.GetName(intUser), "", strEMailIdsBCC, "ClearView : Removal from reporting list of " + oUser.GetFullName(intManager), "<p><b>" + oUser.GetFullName(intUser) + " (USER ID: " + intUser.ToString() + ") has been removed from the reporting list of " + oUser.GetFullName(intManager) + ".</b>" +
                                    "<p></p><p><a href=\"" + oVariable.URL() + strRedirect + "\"" + ">Click here to view your settings.</a></p>", true, false);

                if (txtReportingSelectedUserValue.Text == oButton.CommandArgument)
                {
                    txtReportingSelectedUserValue.Text = "";
                    lblReportingSelectedUser.Text = "";
                }
                LoadResourceReporting();
                oTab.SelectedTab = intMenuResourceReporting;
                strMenuTab1 = oTab.GetTabs();
            }
        #endregion

        #region Application Resource Management

            protected void lnkbtnAppRLRemove_Click(Object Sender, EventArgs e)
            {
                LinkButton oButton = (LinkButton)Sender;
                int intAppUser = Int32.Parse(oButton.CommandArgument);
                int intApplication = Int32.Parse(ddlApplication.SelectedValue);

                int intMultiple_Apps = Int32.Parse(oUser.Get(intAppUser, "multiple_apps"));

                if (intMultiple_Apps != 1)
                {
                    oRole.DeleteUser(intAppUser);
                    int intGroup = Int32.Parse(ConfigurationManager.AppSettings["CLEARVIEW_USER_GROUPID"]);
                    oRole.Add(intAppUser, intGroup);

                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");

                    string strDefault = oUser.GetApplicationUrl(intAppUser, intPage);
                    string strRedirect = "/" + strDefault + oPage.GetFullLink(intPage);
                    oFunction.SendEmail("ClearView : Removal from application " + ddlApplication.SelectedItem.Text, oUser.GetName(intAppUser), "", strEMailIdsBCC, "ClearView : Removal from application ", "<p><b>" + oUser.GetFullName(intAppUser) + " (USER ID: " + intAppUser.ToString() + ") has been removed from the application " + ddlApplication.SelectedItem.Text + ".</b>" +
                                        "<p></p><p><a href=\"" + oVariable.URL() + strRedirect + "\"" + ">Click here to view your settings.</a></p>", true, false);
                }
                LoadApplicationResources();
                oTab.SelectedTab = intMenuApplicationManagement;
                strMenuTab1 = oTab.GetTabs();
               
            }

            protected void btnAddAppResource_Click(Object Sender, EventArgs e)
            {
                int intAppId = Int32.Parse(ddlApplication.SelectedValue);
                intAppOwnerId = Int32.Parse(oApplication.Get(intAppId, "userid"));

                int intAppUser = Int32.Parse(hdnAppResource.Value);
                int intMultiple_Apps = Int32.Parse(oUser.Get(intAppUser, "multiple_apps"));
                

                DataSet ds = oUser.GetApplicationMgmt(intAppId);
                

                DataSet dsGroup = oRole.GetUser(intAppOwnerId);
                if (intMultiple_Apps != 1) oRole.DeleteUser(intAppUser);

                foreach (DataRow drGroup in dsGroup.Tables[0].Rows)
                {
                    if (drGroup["applicationname"].ToString() == oApplication.GetName(intAppId))
                    {
                        int intGroup = Int32.Parse(drGroup["groupid"].ToString());
                        oRole.Add(intAppUser, intGroup);

                        string strDefault = oUser.GetApplicationUrl(intAppUser, intPage);
                        string strRedirect = "/" + strDefault + oPage.GetFullLink(intPage);
                        oFunction.SendEmail("ClearView : Added to application " + ddlApplication.SelectedItem.Text, oUser.GetName(intAppUser), "", strEMailIdsBCC, "ClearView : Added to application  " + ddlApplication.SelectedItem.Text, "<p><b>" + oUser.GetFullName(intAppUser) + " (USER ID: " + intAppUser.ToString() + ") has been added to application " + drGroup["applicationname"].ToString() + ".</b>" +
                                        "<p></p><p><a href=\"" + oVariable.URL() + strRedirect + "\"" + ">Click here to view your settings.</a></p>", true, false);

                        break;
                    }
                }

                txtAppResource.Text = "";
                hdnAppResource.Value = "";
                LoadApplicationResources();

                oTab.SelectedTab = intMenuApplicationManagement;
                strMenuTab1 = oTab.GetTabs();


            }

            protected void ddlApplication_OnSelectedIndexChanged(Object Sender, EventArgs e)
            {
                //Load Applications Resources
                if (ddlApplication.SelectedValue != "-1")
                {
                    int intAppId = Int32.Parse(ddlApplication.SelectedValue);
                    intAppOwnerId = Int32.Parse(oApplication.Get(intAppId, "userid"));
                    LoadApplicationResources();

                    oTab.SelectedTab = intMenuApplicationManagement;
                    strMenuTab1 = oTab.GetTabs();

                }

            }

            protected void dlApplicationResources_ItemDataBound(object sender, DataListItemEventArgs e)
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView drv = (DataRowView)e.Item.DataItem;

                    Label lblAppRLUserName = (Label)e.Item.FindControl("lblAppRLUserName");
                    lblAppRLUserName.Text = drv["name"].ToString();

                    LinkButton lnkbtnAppRLRemove = (LinkButton)e.Item.FindControl("lnkbtnAppRLRemove");
                    lnkbtnAppRLRemove.CommandArgument = drv["userid"].ToString();
                    if (intProfile != intAppOwnerId)
                        lnkbtnAppRLRemove.Enabled = false;
                    else
                    {
                        lnkbtnAppRLRemove.Enabled = true;
                        lnkbtnAppRLRemove.Attributes.Add("onclick", "return confirm('Are you sure you want to remove this employee from application " + ddlApplication.SelectedItem.Text + " ?');");
                    }
                }
            }

            private void LoadApplicationResources()
            {
                int intAppId = Int32.Parse(ddlApplication.SelectedValue);
                intAppOwnerId = Int32.Parse(oApplication.Get(intAppId, "userid"));

                DataSet dsAppResources = oUser.GetApplicationMgmt(intAppId);
               

                if (intProfile != intAppOwnerId)
                    pnlAddAppResource.Visible = false;
                else
                    pnlAddAppResource.Visible = true;

                dlApplicationResources.DataSource = dsAppResources.Tables[0];
                dlApplicationResources.DataBind();

                pnlAppResourceList.Visible = (dsAppResources.Tables[0].Rows.Count != 0);

                
            }

        #endregion

    }
}