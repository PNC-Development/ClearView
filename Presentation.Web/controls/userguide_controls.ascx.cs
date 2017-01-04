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
    public partial class userguide_controls : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intService = Int32.Parse(ConfigurationManager.AppSettings["HELP_SERVICEID"]);

        protected Requests oRequest;
        protected Services oService;
        protected ServiceRequests oServiceRequest;
        protected Customized oCustomized;
        protected Variables oVariable;

        protected int intProfile;
        private DataSet ds;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oRequest = new Requests(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            btnSend.Attributes.Add("onclick", "return ValidateTreeNodeSelection()");
            if (!IsPostBack)
            {
                LoadTreeView();
            }
        }

        private void LoadTreeView()
        {
            ds = oCustomized.GetModules(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["pageid"].ToString();
                oNode.Value = dr["pageid"].ToString();
                oNode.NavigateUrl = "javascript:void(0);";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                LoadChildNode(oNode);
                oNode.Expand();
            }
            oTreeview.ExpandDepth = 1;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
            oTreeview.Attributes.Add("onclick", "OnTreeClick(event);");

        }

        private void LoadChildNode(TreeNode oParent)
        {
            DataSet ds2 = oCustomized.GetUserGuideByPage(Int32.Parse(oParent.Value));
            foreach (DataRow dr in ds2.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = "&nbsp;" + dr["path"].ToString().Replace("/uploads/", "");
                oNode.Value = dr["id"].ToString();
                oNode.ToolTip = dr["id"].ToString();
                // oNode.ImageUrl = "/images/icons/pdf.gif";                
                oNode.SelectAction = TreeNodeSelectAction.Select;
                oNode.NavigateUrl = dr["path"].ToString();
                oNode.Target = "_blank";

                oParent.ChildNodes.Add(oNode);
            }

        }
    }
}