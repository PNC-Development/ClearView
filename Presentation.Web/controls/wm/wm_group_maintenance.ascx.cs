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
using System.Text;
namespace NCC.ClearView.Presentation.Web
{
    public partial class wm_group_maintenance : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intMyWork = Int32.Parse(ConfigurationManager.AppSettings["MyWork"]);
        protected int intProfile;
        protected Functions oFunction;
        protected Users oUser;
        protected Pages oPage;
        protected ResourceRequest oResourceRequest;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected Applications oApplication;
        protected GroupRequest oGroupRequest;
        protected Delegates oDelegate;
        protected Services oService;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected int intResourceWorkflow = 0;
        protected int intResourceParent = 0;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intService = 0;
        protected int intNumber = 0;
        protected string strDetails = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oUser = new Users(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oGroupRequest = new GroupRequest(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            string strAttributes = "";
            bool boolIncomplete = false;
            if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
            {
                intResourceWorkflow = Int32.Parse(Request.QueryString["rrid"]);
                intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                DataSet ds = oResourceRequest.Get(intResourceParent);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                    intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                    intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                    // Start Workflow Change
                    bool boolComplete = (oResourceRequest.GetWorkflow(intResourceWorkflow, "status") == "3");
                    int intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                    // End Workflow Change
                    intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                    int intApp = oRequestItem.GetItemApplication(intItem);

                    if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                    if (oService.Get(intService, "sla") != "")
                    {
                        oFunction.ConfigureToolButton(btnSLA, "/images/tool_sla");
                        int intDays = oResourceRequest.GetSLA(intResourceParent);
                        if (intDays < 1)
                            btnSLA.Style["border"] = "solid 2px #FF0000";
                        else if (intDays < 3)
                            btnSLA.Style["border"] = "solid 2px #FF9999";
                        btnSLA.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_SLA','?rrid=" + intResourceParent.ToString() + "');");
                    }
                    else
                    {
                        btnSLA.ImageUrl = "/images/tool_sla_dbl.gif";
                        btnSLA.Enabled = false;
                    }
                    if (!IsPostBack)
                    {
                        if (intResourceWorkflow > 0)
                        {
                            ds = oRequest.Get(intRequest);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                ds = oGroupRequest.GetMaintenance(intRequest, intItem, intNumber);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    if (ds.Tables[0].Rows[0]["completed"].ToString() == "")
                                        boolIncomplete = true;
                                    else
                                    {
                                        lblComplete.Text = ds.Tables[0].Rows[0]["completed"].ToString();
                                        panComplete.Visible = true;
                                    }
                                    lblRequestBy.Text = oUser.GetFullName(oRequest.GetUser(intRequest));
                                    lblRequestOn.Text = DateTime.Parse(oRequest.Get(intRequest, "created")).ToLongDateString();
                                    int intDomain = Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString());
                                    Domains oDomain = new Domains(intProfile, dsn);
                                    intDomain = Int32.Parse(oDomain.Get(intDomain, "environment"));
                                    AD oAD = new AD(intProfile, dsn, intDomain);
                                    Variables oVariable = new Variables(intDomain);
                                    DataSet dsParameters = oGroupRequest.GetMaintenanceParameters(intRequest, intItem, intNumber);
                                    StringBuilder sb = new StringBuilder(strDetails);

                                    switch (ds.Tables[0].Rows[0]["maintenance"].ToString())
                                    {
                                        case "CREATE":
                                            lblType.Text = "Group Creation";
                                            sb.Append("<tr><td><b>Group:</b></td><td>");
                                            sb.Append(ds.Tables[0].Rows[0]["name"].ToString());
                                            sb.Append("</td>");
                                            sb.Append("<tr><td><b>Domain:</b></td><td>");
                                            sb.Append(oDomain.Get(Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString()), "name"));
                                            sb.Append("</td>");
                                            panID.Visible = true;
                                            txtID.Text = ds.Tables[0].Rows[0]["name"].ToString();
                                            sb.Append("<tr><td><b>Scope:</b></td><td>");
                                            sb.Append(dsParameters.Tables[0].Rows[0]["value"].ToString());
                                            sb.Append("</td>");
                                            sb.Append("<tr><td><b>Type:</b></td><td>");
                                            sb.Append(dsParameters.Tables[0].Rows[1]["value"].ToString());
                                            sb.Append("</td>");
                                            break;
                                        case "MOVE":
                                            lblType.Text = "Group Move";
                                            sb.Append("<tr><td><b>Group:</b></td><td>");
                                            sb.Append(ds.Tables[0].Rows[0]["name"].ToString());
                                            sb.Append("</td>");
                                            sb.Append("<tr><td><b>Domain:</b></td><td>");
                                            sb.Append(oDomain.Get(Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString()), "name"));
                                            sb.Append("</td>");
                                            sb.Append("<tr><td><b>New Location:</b></td><td>");
                                            sb.Append(dsParameters.Tables[0].Rows[0]["value"].ToString());
                                            sb.Append("</td>");
                                            break;
                                        case "COPY":
                                            lblType.Text = "Group Copy";
                                            sb.Append("<tr><td><b>Group:</b></td><td>");
                                            sb.Append(ds.Tables[0].Rows[0]["name"].ToString());
                                            sb.Append("</td>");
                                            sb.Append("<tr><td><b>Domain:</b></td><td>");
                                            sb.Append(oDomain.Get(Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString()), "name"));
                                            sb.Append("</td>");
                                            panID.Visible = true;
                                            txtID.Text = dsParameters.Tables[0].Rows[0]["value"].ToString();
                                            sb.Append("<tr><td><b>Scope:</b></td><td>");
                                            sb.Append(dsParameters.Tables[0].Rows[1]["value"].ToString());
                                            sb.Append("</td>");
                                            sb.Append("<tr><td><b>Type:</b></td><td>");
                                            sb.Append(dsParameters.Tables[0].Rows[2]["value"].ToString());
                                            sb.Append("</td>");
                                            panUsers.Visible = true;
                                            string[] strUsers;
                                            char[] strSplit = { ';' };
                                            strUsers = dsParameters.Tables[0].Rows[3]["value"].ToString().Split(strSplit);
                                            for (int ii = 0; ii < strUsers.Length; ii++)
                                            {
                                                if (strUsers[ii].Trim() != "")
                                                {
                                                    DirectoryEntry oEntry2 = oAD.UserSearch(strUsers[ii].Trim());
                                                    ListItem oList = new ListItem(oEntry2.Properties["displayName"].Value.ToString() + " (" + oEntry2.Properties["name"].Value.ToString() + ")", oEntry2.Properties["name"].Value.ToString());
                                                    chkUsers.Items.Add(oList);
                                                    oList.Selected = true;
                                                }
                                            }
                                            break;
                                        case "DELETE":
                                            lblType.Text = "Group Deletion";
                                            sb.Append("<tr><td><b>Group:</b></td><td>");
                                            sb.Append(ds.Tables[0].Rows[0]["name"].ToString());
                                            sb.Append("</td>");
                                            sb.Append("<tr><td><b>Domain:</b></td><td>");
                                            sb.Append(oDomain.Get(Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString()), "name"));
                                            sb.Append("</td>");
                                            break;
                                        case "RENAME":
                                            lblType.Text = "Group Rename";
                                            sb.Append("<tr><td><b>Group:</b></td><td>");
                                            sb.Append(ds.Tables[0].Rows[0]["name"].ToString());
                                            sb.Append("</td>");
                                            sb.Append("<tr><td><b>Domain:</b></td><td>");
                                            sb.Append(oDomain.Get(Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString()), "name"));
                                            sb.Append("</td>");
                                            panID.Visible = true;
                                            txtID.Text = dsParameters.Tables[0].Rows[0]["value"].ToString();
                                            break;
                                        case "CHANGE":
                                            lblType.Text = "Group Properties Change";
                                            sb.Append("<tr><td><b>Group:</b></td><td>");
                                            sb.Append(ds.Tables[0].Rows[0]["name"].ToString());
                                            sb.Append("</td>");
                                            sb.Append("<tr><td><b>Domain:</b></td><td>");
                                            sb.Append(oDomain.Get(Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString()), "name"));
                                            sb.Append("</td>");
                                            sb.Append("<tr><td><b>New Scope:</b></td><td>");
                                            sb.Append(dsParameters.Tables[0].Rows[0]["value"].ToString());
                                            sb.Append("</td>");
                                            sb.Append("<tr><td><b>New Type:</b></td><td>");
                                            sb.Append(dsParameters.Tables[0].Rows[1]["value"].ToString());
                                            sb.Append("</td>");
                                            break;
                                        default:
                                            lblType.Text = "Unavailable";
                                            sb.Append("<tr><td colspan=\"2\"><b>Invalid Maintenance Code!</b></td>");
                                            break;
                                    }
                                    strDetails = sb.ToString();

                                    panWorkload.Visible = true;
                                }
                                else
                                    panDenied.Visible = true;
                            }
                            else
                                panDenied.Visible = true;
                        }
                        else
                            panDenied.Visible = true;
                    }
                }
                else
                    panDenied.Visible = true;

                btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                oFunction.ConfigureToolButton(btnApprove, "/images/tool_approve");
                oFunction.ConfigureToolButton(btnDeny, "/images/tool_deny");
                if (boolIncomplete == true)
                {
                    btnApprove.Attributes.Add("onclick", "return " + strAttributes + "confirm('Are you sure you want to APPROVE this request?');");
                    btnDeny.Attributes.Add("onclick", "return ValidateText('" + txtComments.ClientID + "','Please enter a reason') && confirm('Are you sure you want to DENY this request?');");
                }
                else
                {
                    btnApprove.Attributes.Add("onclick", "alert('This request has already been completed.\\n\\nIf this request continues to appear in your work queue, please contact a ClearView administrator.');return false;");
                    btnDeny.Attributes.Add("onclick", "alert('This request has already been completed.\\n\\nIf this request continues to appear in your work queue, please contact a ClearView administrator.');return false;");
                }
                oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                btnClose.Attributes.Add("onclick", "return ExitWindow();");
                // 6/1/2009 - Load ReadOnly View
                if (oResourceRequest.CanUpdate(intProfile, intResourceWorkflow, false) == false)
                {
                    oFunction.ConfigureToolButtonRO(btnApprove, btnDeny);
                    //panDenied.Visible = true;
                }
            }
            else
                panDenied.Visible = true;
        }
        protected void btnApprove_Click(Object Sender, EventArgs e)
        {
            oResourceRequest.UpdateWorkflowHours(intResourceWorkflow);
            DataSet ds = oGroupRequest.GetMaintenance(intRequest, intItem, intNumber);
            int intUser = oRequest.GetUser(intRequest);
            DataSet dsParameters = oGroupRequest.GetMaintenanceParameters(intRequest, intItem, intNumber);
            string strMaintenance = ds.Tables[0].Rows[0]["maintenance"].ToString();
            string strGroup = ds.Tables[0].Rows[0]["name"].ToString();
            int intDomain = Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString());
            Domains oDomain = new Domains(intProfile, dsn);
            intDomain = Int32.Parse(oDomain.Get(intDomain, "environment"));
            AD oAD = new AD(intProfile, dsn, intDomain);
            Variables oVariable = new Variables(intDomain);
            DirectoryEntry oEntry = oAD.GroupSearch(strGroup);
            if (oEntry != null)
            {
                switch (strMaintenance)
                {
                    case "MOVE":
                        // Group Move
                        string strPath = dsParameters.Tables[0].Rows[0]["value"].ToString();
                        oRequest.AddResult(intRequest, intItem, intNumber, "Group Move", oAD.MoveGroup(oEntry, strPath), "Group " + strGroup + " was successfully moved", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                        break;
                    case "COPY":
                        // Group Copy
                        string strID = txtID.Text;
                        string strScope2 = dsParameters.Tables[0].Rows[1]["value"].ToString();
                        if (strScope2 == "Domain Local")
                            strScope2 = "DLG";
                        if (strScope2 == "Global")
                            strScope2 = "GG";
                        if (strScope2 == "Universal")
                            strScope2 = "UG";
                        string strType2 = dsParameters.Tables[0].Rows[2]["value"].ToString();
                        if (strType2 == "Security")
                            strType2 = "S";
                        if (strType2 == "Distribution")
                            strType2 = "D";
                        oRequest.AddResult(intRequest, intItem, intNumber, "Group Copy", oAD.CreateGroup(strID, "", "Created by ClearView - " + DateTime.Now.ToShortDateString(), oVariable.GroupOU(), strScope2, strType2), "Group " + strGroup + " was successfully copied", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                        foreach (ListItem oList in chkUsers.Items)
                        {
                            if (oList.Selected == true)
                                oRequest.AddResult(intRequest, intItem, intNumber, "Group Membership", oAD.JoinGroup(oList.Value, strID, 0), "Account " + oList.Value + " was successfully added to the group " + strGroup, intEnvironment, (oService.Get(intService, "notify_client") == "1"), "");
                        }
                        break;
                    case "DELETE":
                        // Group Deletion
                        oRequest.AddResult(intRequest, intItem, intNumber, "Group Deletion", oAD.Delete(oEntry), "Group " + strGroup + " was successfully deleted", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                        break;
                    case "RENAME":
                        // Group Rename
                        string strI = txtID.Text;
                        oRequest.AddResult(intRequest, intItem, intNumber, "Group Rename", oAD.Rename(oEntry, strI, "", ""), "Group " + strGroup + " was successfully renamed", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                        break;
                    case "CHANGE":
                        // Group Change
                        string strScope3 = dsParameters.Tables[0].Rows[0]["value"].ToString();
                        if (strScope3 == "Domain Local")
                            strScope3 = "DLG";
                        if (strScope3 == "Global")
                            strScope3 = "GG";
                        if (strScope3 == "Universal")
                            strScope3 = "UG";
                        string strType3 = dsParameters.Tables[0].Rows[1]["value"].ToString();
                        if (strType3 == "Security")
                            strType3 = "S";
                        if (strType3 == "Distribution")
                            strType3 = "D";
                        oRequest.AddResult(intRequest, intItem, intNumber, "Group Change", oAD.Change(oEntry, strScope3, strType3), "Group " + strGroup + " was successfully changed", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                        break;
                }
            }
            else if (strMaintenance == "CREATE")
            {
                // Group Creation
                string strScope1 = dsParameters.Tables[0].Rows[0]["value"].ToString();
                if (strScope1 == "Domain Local")
                    strScope1 = "DLG";
                if (strScope1 == "Global")
                    strScope1 = "GG";
                if (strScope1 == "Universal")
                    strScope1 = "UG";
                string strType1 = dsParameters.Tables[0].Rows[1]["value"].ToString();
                if (strType1 == "Security")
                    strType1 = "S";
                if (strType1 == "Distribution")
                    strType1 = "D";
                oRequest.AddResult(intRequest, intItem, intNumber, "Group Creation", oAD.CreateGroup(txtID.Text, "", "Created by ClearView - " + DateTime.Now.ToShortDateString(), oVariable.GroupOU(), strScope1, strType1), "Group " + strGroup + " was successfully created", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
            }
            oGroupRequest.UpdateMaintenance(intRequest, intItem, intNumber, 1, txtComments.Text);
            Complete();
        }
        protected void btnDeny_Click(Object Sender, EventArgs e)
        {
            oGroupRequest.UpdateMaintenance(intRequest, intItem, intNumber, -1, txtComments.Text);
            Complete();
        }
        protected void Complete()
        {
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment,  0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
    }
}