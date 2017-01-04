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
    public partial class prioritization_qa : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;

        protected DataSet ds;
        protected DataSet dsQA;
        protected int intProfile;
        protected int intQuestion;
        protected int intOrganizationID;
        protected string strBase;
        protected Organizations oOrganization;

        // Vijay Code - Start
        protected ProjectRequest oProjectRequest;
        // Vijay Code - End
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oOrganization = new Organizations(intProfile, dsn);
            oProjectRequest = new ProjectRequest(intProfile, dsn);

            if (Request.QueryString["oid"] != null && Request.QueryString["oid"] != "")
                intOrganizationID = Int32.Parse(Request.QueryString["oid"]);

            if (Request.QueryString["bd"] != null && Request.QueryString["bd"] != "")
                strBase = Request.QueryString["bd"];

            btnSave.Attributes.Add("onclick", "return confirm('WARNING: This will overwrite the project prioritization questions and answers for the checked portfolios.\\nThis action cannot be undone!\\n\\nAre you sure you want to continue?');");


            if (!IsPostBack)
            {
                ds = oOrganization.Gets(1);
                Load();
            }

        }

        private void Load()
        {
            TreeNode oNode;
            oNode = new TreeNode();
            oNode.Text = "Discretionary";
            oNode.ToolTip = "Discretionary";
            oNode.SelectAction = TreeNodeSelectAction.Expand;
            oTreeview.Nodes.Add(oNode);
            Load(oNode);
            oNode.Expand();

            oNode = new TreeNode();
            oNode.Text = "Base";
            oNode.ToolTip = "Base";
            oNode.SelectAction = TreeNodeSelectAction.Expand;
            oTreeview.Nodes.Add(oNode);
            Load(oNode);
            oNode.Expand();

            //oNode = new TreeNode();
            //oNode.Text = "Infrastructure";
            //oNode.ToolTip = "Infrastructure";
            //oNode.SelectAction = TreeNodeSelectAction.Expand;
            //oTreeview.Nodes.Add(oNode);
            //Load(oNode);
            //oNode.Expand();
        }
        private void Load(TreeNode oParent)
        {
            dsQA = oProjectRequest.GetQA(strBase, intOrganizationID);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["organizationid"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oNode.Checked = false;
                foreach (DataRow drQA in dsQA.Tables[0].Rows)
                {
                    if (drQA["organizationid"].ToString() == oNode.Value.ToString() && drQA["bd"].ToString() == oParent.Text)
                    {
                        oNode.Checked = true;
                        oNode.ShowCheckBox = false;
                        oNode.ImageUrl = "/images/check.gif";
                    }

                }
                oParent.ChildNodes.Add(oNode);

            }
            oTreeview.ExpandDepth = 1;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            foreach (TreeNode oNode in oTreeview.Nodes)
            {
                SaveQA(oNode);
            }
            ClientScript.RegisterClientScriptBlock(typeof(Page), "redirect", "window.top.navigate(window.top.location);", true);
        }

        private void SaveQA(TreeNode oParent)
        {

            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true && !(oParent.Text.ToString() == strBase && oNode.Value.ToString() == intOrganizationID.ToString()))
                {
                    int _organization_id = Int32.Parse(oNode.Value);
                    oProjectRequest.DeleteQA(oParent.Text, _organization_id);
                    dsQA = oProjectRequest.GetQA(strBase, intOrganizationID);
                    foreach (DataRow drQA in dsQA.Tables[0].Rows)
                    {
                        intQuestion = Int32.Parse(drQA["questionid"].ToString());
                        oProjectRequest.AddQA(oParent.Text, _organization_id, intQuestion);
                    }
                }
            }

        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
