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
    public partial class workflow : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Services oService;
        protected int intService = 0;
        protected int intParent = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            oService = new Services(0, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intService = Int32.Parse(Request.QueryString["id"]);
                //if (Request.QueryString["sametime"] != null && Request.QueryString["sametime"] != "")
                //{
                //    int intSameTime = Int32.Parse(Request.QueryString["sametime"]);
                //    oService.UpdateEditorWorkflow(intService, intSameTime, 1);
                //}
            }
            if (Request.QueryString["parent"] != null && Request.QueryString["parent"] != "")
                intParent = Int32.Parse(Request.QueryString["parent"]);
            if (!IsPostBack)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = "ClearView Services";
                oNode.ToolTip = "ClearView Services";
                oNode.SelectAction = TreeNodeSelectAction.None;
                treLocation.Nodes.Add(oNode);
                LoadLocations(0, oNode);
                treLocation.CollapseAll();
                oNode.Expand();
                oNode.ShowCheckBox = false;
                btnSave.Attributes.Add("onclick", "return ValidateHidden('" + hdnService.ClientID + "','" + treLocation.ClientID + "','Please select a service')" +
                    ";");
            }
        }
        private void LoadLocations(int _parent, TreeNode oParent)
        {
            DataSet ds = oService.GetFolders(_parent, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.Text = "&nbsp;" + dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oParent.ChildNodes.Add(oNode);
                LoadLocations(Int32.Parse(dr["id"].ToString()), oNode);
                LoadServices(Int32.Parse(dr["id"].ToString()), oNode);
                //if (oNode.ChildNodes.Count == 0)
                //{
                //    TreeNode oNone = new TreeNode();
                //    oNone.Text = "No services with workflow enabled";
                //    oNone.SelectAction = TreeNodeSelectAction.None;
                //    oNone.ImageUrl = "/images/close-icon.gif";
                //    oParent.ChildNodes.Add(oNone);
                //}
            }
        }
        private void LoadServices(int _parent, TreeNode oParent)
        {
            DataSet ds = oService.Gets(_parent, 1, 0, 1, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                if (Int32.Parse(dr["serviceid"].ToString()) == intService)
                {
                    oNode.Text = "&nbsp;" + "<span class=\"help\">" + dr["name"].ToString() + "</span>";
                    oNode.ImageUrl = "/images/cancel.gif";
                    oNode.ShowCheckBox = false;
                }
                else
                    oNode.Text = "<span class=\"default\"><input id=\"rad" + dr["serviceid"].ToString() + "\" type=\"radio\" onclick=\"UpdateWorkflow(this,'" + hdnService.ClientID + "','" + dr["serviceid"].ToString() + "');\" /><label for=\"rad" + dr["serviceid"].ToString() + "\">" + dr["name"].ToString() + "</label></span>";
                oNode.Value = dr["serviceid"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intPreService = Int32.Parse(Request.Form[hdnService.UniqueID]);
            //if (oService.GetWorkflowsReceive(intNextService).Tables[0].Rows.Count > 0)
            //    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "already", "<script type=\"text/javascript\">alert('The service you selected is already a member of another workflow.\\n\\nA service can only be configured as part of one workflow. Select another service or contact the service owner for more information');<" + "/" + "script>");
            //else
            //{
                DataSet dsWorkflows = oService.GetWorkflows(intPreService);
                int intCount = dsWorkflows.Tables[0].Rows.Count + 1;
                oService.AddWorkflow(intPreService, intService, intCount);
                if (intCount > 1)
                    oService.UpdateEditorWorkflow(intService, 1, 1, 1);
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">window.parent.navigate(window.parent.location);<" + "/" + "script>");
            //}
        }
    }
}
