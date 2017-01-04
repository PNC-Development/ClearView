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
namespace NCC.ClearView.Presentation.Web
{
    public partial class wm_account_maintenance : System.Web.UI.UserControl
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
        protected AccountRequest oAccountRequest;
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
            oAccountRequest = new AccountRequest(intProfile, dsn);
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
                int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                DataSet ds = oResourceRequest.Get(intResourceParent);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                    intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                    intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                    // Workflow start
                    bool boolComplete = (oResourceRequest.GetWorkflow(intResourceWorkflow, "status") == "3");
                    int intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                    double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                    // Workflow end
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
                                ds = oAccountRequest.GetMaintenance(intRequest, intItem, intNumber);
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
                                    DataSet dsParameters = oAccountRequest.GetMaintenanceParameters(intRequest, intItem, intNumber);
                                    switch (ds.Tables[0].Rows[0]["maintenance"].ToString())
                                    {
                                        case "DISABLE":
                                            lblType.Text = "Account Disable";
                                            strDetails += "<tr><td><b>Account:</b></td><td>" + ds.Tables[0].Rows[0]["username"].ToString() + " (" + oAD.GetUserFullName(ds.Tables[0].Rows[0]["username"].ToString()) + ")</td>";
                                            strDetails += "<tr><td><b>Domain:</b></td><td>" + oDomain.Get(Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString()), "name") + "</td>";
                                            break;
                                        case "UNLOCK":
                                            lblType.Text = "Account Unlock";
                                            strDetails += "<tr><td><b>Account:</b></td><td>" + ds.Tables[0].Rows[0]["username"].ToString() + " (" + oAD.GetUserFullName(ds.Tables[0].Rows[0]["username"].ToString()) + ")</td>";
                                            strDetails += "<tr><td><b>Domain:</b></td><td>" + oDomain.Get(Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString()), "name") + "</td>";
                                            break;
                                        case "ENABLE":
                                            lblType.Text = "Account Enable";
                                            strDetails += "<tr><td><b>Account:</b></td><td>" + ds.Tables[0].Rows[0]["username"].ToString() + " (" + oAD.GetUserFullName(ds.Tables[0].Rows[0]["username"].ToString()) + ")</td>";
                                            strDetails += "<tr><td><b>Domain:</b></td><td>" + oDomain.Get(Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString()), "name") + "</td>";
                                            break;
                                        case "EMAIL":
                                            lblType.Text = "Email Enable an Account";
                                            strDetails += "<tr><td><b>Account:</b></td><td>" + ds.Tables[0].Rows[0]["username"].ToString() + " (" + oAD.GetUserFullName(ds.Tables[0].Rows[0]["username"].ToString()) + ")</td>";
                                            strDetails += "<tr><td><b>Domain:</b></td><td>" + oDomain.Get(Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString()), "name") + "</td>";
                                            break;
                                        case "MOVE":
                                            lblType.Text = "Account Move";
                                            strDetails += "<tr><td><b>Account:</b></td><td>" + ds.Tables[0].Rows[0]["username"].ToString() + " (" + oAD.GetUserFullName(ds.Tables[0].Rows[0]["username"].ToString()) + ")</td>";
                                            strDetails += "<tr><td><b>Domain:</b></td><td>" + oDomain.Get(Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString()), "name") + "</td>";
                                            strDetails += "<tr><td><b>New Location:</b></td><td>" + dsParameters.Tables[0].Rows[0]["value"].ToString() + "</td>";
                                            break;
                                        case "PASSWORD":
                                            lblType.Text = "Account Password Change";
                                            strDetails += "<tr><td><b>Account:</b></td><td>" + ds.Tables[0].Rows[0]["username"].ToString() + " (" + oAD.GetUserFullName(ds.Tables[0].Rows[0]["username"].ToString()) + ")</td>";
                                            strDetails += "<tr><td><b>Domain:</b></td><td>" + oDomain.Get(Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString()), "name") + "</td>";
                                            break;
                                        case "COPY":
                                            lblType.Text = "Account Copy";
                                            panPassword.Visible = true;
                                            strAttributes += "ValidateText('" + txtPassword.ClientID + "','Please enter a password') && ";
                                            strDetails += "<tr><td><b>Account:</b></td><td>" + ds.Tables[0].Rows[0]["username"].ToString() + " (" + oAD.GetUserFullName(ds.Tables[0].Rows[0]["username"].ToString()) + ")</td>";
                                            strDetails += "<tr><td><b>Domain:</b></td><td>" + oDomain.Get(Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString()), "name") + "</td>";
                                            panID.Visible = true;
                                            txtID.Text = dsParameters.Tables[0].Rows[0]["value"].ToString();
                                            panFirst.Visible = true;
                                            txtFirst.Text = dsParameters.Tables[0].Rows[1]["value"].ToString();
                                            panLast.Visible = true;
                                            txtLast.Text = dsParameters.Tables[0].Rows[2]["value"].ToString();
                                            panGroups.Visible = true;
                                            string[] strGroups;
                                            char[] strSplit = { ';' };
                                            strGroups = dsParameters.Tables[0].Rows[3]["value"].ToString().Split(strSplit);
                                            for (int ii = 0; ii < strGroups.Length; ii++)
                                            {
                                                if (strGroups[ii].Trim() != "")
                                                {
                                                    ListItem oList = new ListItem(strGroups[ii].Trim());
                                                    chkGroups.Items.Add(oList);
                                                    oList.Selected = true;
                                                }
                                            }
                                            break;
                                        case "DELETE":
                                            lblType.Text = "Account Deletion";
                                            strDetails += "<tr><td><b>Account:</b></td><td>" + ds.Tables[0].Rows[0]["username"].ToString() + " (" + oAD.GetUserFullName(ds.Tables[0].Rows[0]["username"].ToString()) + ")</td>";
                                            strDetails += "<tr><td><b>Domain:</b></td><td>" + oDomain.Get(Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString()), "name") + "</td>";
                                            break;
                                        case "RENAME":
                                            lblType.Text = "Account Rename";
                                            strDetails += "<tr><td><b>Account:</b></td><td>" + ds.Tables[0].Rows[0]["username"].ToString() + " (" + oAD.GetUserFullName(ds.Tables[0].Rows[0]["username"].ToString()) + ")</td>";
                                            strDetails += "<tr><td><b>Domain:</b></td><td>" + oDomain.Get(Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString()), "name") + "</td>";
                                            panID.Visible = true;
                                            txtID.Text = dsParameters.Tables[0].Rows[0]["value"].ToString();
                                            panFirst.Visible = true;
                                            txtFirst.Text = dsParameters.Tables[0].Rows[1]["value"].ToString();
                                            panLast.Visible = true;
                                            txtLast.Text = dsParameters.Tables[0].Rows[2]["value"].ToString();
                                            break;
                                        default:
                                            lblType.Text = "Unavailable";
                                            strDetails += "<tr><td colspan=\"2\"><b>Invalid Maintenance Code!</b></td>";
                                            break;
                                    }
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
                }
                else
                    panDenied.Visible = true;
            }
            else
                panDenied.Visible = true;
        }
        protected void btnApprove_Click(Object Sender, EventArgs e)
        {
            oResourceRequest.UpdateWorkflowHours(intResourceWorkflow);
            DataSet ds = oAccountRequest.GetMaintenance(intRequest, intItem, intNumber);
            int intUser = oRequest.GetUser(intRequest);
            DataSet dsParameters = oAccountRequest.GetMaintenanceParameters(intRequest, intItem, intNumber);
            string strMaintenance = ds.Tables[0].Rows[0]["maintenance"].ToString();
            string strUser = ds.Tables[0].Rows[0]["username"].ToString();
            int intDomain = Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString());
            Domains oDomain = new Domains(intProfile, dsn);
            intDomain = Int32.Parse(oDomain.Get(intDomain, "environment"));
            AD oAD = new AD(intProfile, dsn, intDomain);
            Variables oVariable = new Variables(intDomain);
            DirectoryEntry oEntry = oAD.UserSearch(strUser);
            if (oEntry != null)
            {
                switch (strMaintenance)
                {
                    case "DISABLE":
                        // Account Disable
                        oRequest.AddResult(intRequest, intItem, intNumber, "Account Disable", oAD.Enable(oEntry, false), "Account " + strUser + " was successfully disabled", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                        break;
                    case "UNLOCK":
                        // Account Unlock
                        oRequest.AddResult(intRequest, intItem, intNumber, "Account Unlock", oAD.Unlock(oEntry), "Account " + strUser + " was successfully unlocked", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                        break;
                    case "ENABLE":
                        // Account Enable
                        oRequest.AddResult(intRequest, intItem, intNumber, "Account Enable", oAD.Enable(oEntry, true), "Account " + strUser + " was successfully enabled", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                        break;
                    case "EMAIL":
                        // Account Email
                        oRequest.AddResult(intRequest, intItem, intNumber, "Account Email", oAD.CreateMailbox(oEntry), "Account " + strUser + " was successfully created a mailbox", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                        break;
                    case "MOVE":
                        // Account Move
                        string strPath = dsParameters.Tables[0].Rows[0]["value"].ToString();
                        oRequest.AddResult(intRequest, intItem, intNumber, "Account Move", oAD.MoveAccount(oEntry, strPath), "Account " + strUser + " was successfully moved", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                        break;
                    case "PASSWORD":
                        // Account Password Change
                        Encryption oEncrypt = new Encryption();
                        string strPassword = dsParameters.Tables[0].Rows[0]["value"].ToString();
                        oRequest.AddResult(intRequest, intItem, intNumber, "Account Password Change", oAD.SetPassword(oEntry, oEncrypt.Decrypt(strPassword, "adpass")), "The password for account " + strUser + " was successfully changed", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                        break;
                    case "COPY":
                        // Account Copy
                        string strID = txtID.Text;
                        string strFirst = txtFirst.Text;
                        string strLast = txtLast.Text;
                        oRequest.AddResult(intRequest, intItem, intNumber, "Account Copy", oAD.CreateUser(strID, strFirst, strLast, txtPassword.Text, "", "Created by ClearView - " + DateTime.Now.ToShortDateString(), oVariable.UserOU()), "Account " + strUser + " was successfully copied", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                        foreach (ListItem oList in chkGroups.Items)
                        {
                            if (oList.Selected == true)
                                oRequest.AddResult(intRequest, intItem, intNumber, "Group Membership", oAD.JoinGroup(strID, oList.Value, 0), "Account " + strUser + " was successfully added to the group " + oList.Value, intEnvironment, (oService.Get(intService, "notify_client") == "1"), "");
                        }
                        break;
                    case "DELETE":
                        // Account Deletion
                        oRequest.AddResult(intRequest, intItem, intNumber, "Account Deletion", oAD.Delete(oEntry), "Account " + strUser + " was successfully deleted", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                        break;
                    case "RENAME":
                        // Account Rename
                        string strI = txtID.Text;
                        string strF = txtFirst.Text;
                        string strL = txtLast.Text;
                        oRequest.AddResult(intRequest, intItem, intNumber, "Account Rename", oAD.Rename(oEntry, strI, strF, strL), "Account " + strUser + " was successfully renamed", intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intUser));
                        break;
                }
            }
            oAccountRequest.UpdateMaintenance(intRequest, intItem, intNumber, 1, txtComments.Text);
            Complete();
        }
        protected void btnDeny_Click(Object Sender, EventArgs e)
        {
            oAccountRequest.UpdateMaintenance(intRequest, intItem, intNumber, -1, txtComments.Text);
            Complete();
        }
        protected void Complete()
        {
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
    }
}