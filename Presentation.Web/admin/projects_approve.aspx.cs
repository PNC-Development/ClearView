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
namespace NCC.ClearView.Presentation.Web
{
    public partial class projects_approve : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected bool boolDirector = (ConfigurationManager.AppSettings["DirectorApproval"] == "1");
        protected int intResourceRequestPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intWorkflowPage = Int32.Parse(ConfigurationManager.AppSettings["WorkflowSuffix"]);
        protected int intWorkloadPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected Projects oProject;
        protected Requests oRequest;
        protected ProjectRequest oProjectRequest;
        protected ProjectRequest_Approval oApprove;
        protected Users oUser;
        protected Organizations oOrganization;
        protected int intProfile;
        protected int intRequest = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/projects_approve.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oProject = new Projects(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oProjectRequest = new ProjectRequest(intProfile, dsn);
            oApprove = new ProjectRequest_Approval(intProfile, dsn, intEnvironment);
            oUser = new Users(intProfile, dsn);
            oOrganization = new Organizations(intProfile, dsn);
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
            {
                panView.Visible = true;
                intRequest = Int32.Parse(Request.QueryString["rid"]);
                if (!IsPostBack)
                {
                    for (int ii = 1; ii <= 4; ii++)
                    {
                        string strName = "Manager";
                        if (ii == 2)
                            strName = "Platform";
                        else if (ii == 3)
                            strName = "Board";
                        else if (ii == 4)
                            strName = "Director";
                        TreeNode oParent = new TreeNode();
                        oParent.Text = strName;
                        oParent.Value = ii.ToString();
                        oParent.ToolTip = strName;
                        oParent.ImageUrl = "/images/folder.gif";
                        oParent.SelectAction = TreeNodeSelectAction.Expand;
                        oTreeview.Nodes.Add(oParent);
                        DataSet ds = oApprove.GetAwaitingRequest(intRequest, ii);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            TreeNode oNode = new TreeNode();
                            oNode.Text = dr["username"].ToString();
                            oNode.Value = dr["userid"].ToString();
                            oNode.ToolTip = dr["username"].ToString();
                            oNode.ShowCheckBox = true;
                            oNode.SelectAction = TreeNodeSelectAction.None;
                            oParent.ChildNodes.Add(oNode);
                        }
                    }
                }
            }
            else
            {
                panAll.Visible = true;
                rptView.DataSource = oApprove.GetAwaiting();
                rptView.DataBind();
            }
            btnApprove.Attributes.Add("onclick", "return confirm('Are you sure you want to continue?');");
            btnReject.Attributes.Add("onclick", "return confirm('Are you sure you want to continue?');");
            btnFuture.Attributes.Add("onclick", "return confirm('Are you sure you want to continue?');");
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            Button oButton = (Button)Sender;
            foreach (TreeNode oNode in oTreeview.Nodes)
                Approve(oNode, Int32.Parse(oNode.Value), Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        private void Approve(TreeNode oParent, int _step, int _approval)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                {
                    int intUser = Int32.Parse(oNode.Value);
                    switch (_step)
                    {
                        case 1:
                            oApprove.ManagerApproval(intRequest, intUser, _approval, intWorkflowPage, "", boolDirector);
                            break;
                        case 2:
                            oApprove.PlatformApproval(intRequest, intUser, _approval, intWorkflowPage, "", boolDirector);
                            break;
                        case 3:
                            oApprove.BoardApproval(intRequest, intUser, _approval, intWorkflowPage, intWorkloadPage, "", intResourceRequestPage, boolDirector);
                            break;
                        case 4:
                            oApprove.DirectorApproval(intRequest, intUser, _approval, intWorkflowPage, intWorkloadPage, "", intResourceRequestPage);
                            break;
                    }
                }
            }
        }
        protected  void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
    }
}
