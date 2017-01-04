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
    public partial class frame_question_platforms : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected Forecast oForecast;
        protected Platforms oPlatform;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected int intQuestion;
        protected int intPlatform = 0;
        protected int intClass = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oForecast = new Forecast(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            btnClose.Attributes.Add("onclick", "return HidePanel();");
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intQuestion = Int32.Parse(Request.QueryString["id"]);
            if (!IsPostBack)
            {
                if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != "")
                    intPlatform = Int32.Parse(Request.QueryString["pid"]);
                if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
                    intClass = Int32.Parse(Request.QueryString["cid"]);
                LoadPlatforms();
                if (intQuestion > 0)
                    lblName.Text = oForecast.GetQuestion(intQuestion, "name");
                // Expand Nodes
                foreach (TreeNode oP in oTree.Nodes)
                {
                    int _platformid = Int32.Parse(oP.Value);
                    foreach (TreeNode oC in oP.ChildNodes)
                    {
                        int _classid = Int32.Parse(oC.Value);
                        bool boolExpand = false;
                        foreach (TreeNode oE in oC.ChildNodes)
                        {
                            if (_platformid == intPlatform && (intClass == _classid || intClass == 0))
                            {
                                oE.Checked = true;
                                oE.ImageUrl = "/images/check.gif";
                            }
                            if (oE.Checked == true)
                            {
                                boolExpand = true;
                                oE.Expand();
                                //                            break;
                            }
                        }
                        if (boolExpand == true)
                        {
                            oC.Expand();
                            oP.Expand();
                        }
                    }
                }
            }
        }
        private void LoadPlatforms()
        {
            DataSet ds = oPlatform.GetForecasts(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString() + " [<a href=\"" + Request.Path + "?id=" + intQuestion.ToString() + "&pid=" + dr["platformid"].ToString() + "\">Check All</a>]";
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["platformid"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTree.Nodes.Add(oNode);
                LoadClasses(oNode, Int32.Parse(dr["platformid"].ToString()));
                if (Int32.Parse(dr["platformid"].ToString()) != intPlatform)
                    oNode.Collapse();
            }
        }
        private void LoadClasses(TreeNode oParent, int _platformid)
        {
            DataSet ds = oClass.Gets(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString() + " [<a href=\"" + Request.Path + "?id=" + intQuestion.ToString() + "&pid=" + _platformid.ToString() + "&cid=" + dr["id"].ToString() + "\">Check All</a>]";
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadEnvironments(oNode, _platformid, Int32.Parse(dr["id"].ToString()));
                if ((_platformid != intPlatform) || (intClass > 0 && Int32.Parse(dr["id"].ToString()) != intClass))
                    oNode.Collapse();
            }
        }
        private void LoadEnvironments(TreeNode oParent, int _platformid, int _classid)
        {
            DataSet dsOther = oForecast.GetQuestionPlatformByQuestion(intQuestion);
            DataSet ds = oClass.GetEnvironment(_classid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oNode.Checked = false;
                foreach (DataRow drOther in dsOther.Tables[0].Rows)
                {
                    if (_platformid.ToString() == drOther["platformid"].ToString() && _classid.ToString() == drOther["classid"].ToString() && dr["id"].ToString() == drOther["environmentid"].ToString())
                        oNode.Checked = true;
                }
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oForecast.DeleteQuestionPlatform(intQuestion);
            foreach (TreeNode oNode in oTree.Nodes)
                SaveClass(oNode, Int32.Parse(oNode.Value));
            Reload();
        }
        private void SaveClass(TreeNode oParent, int _platformid)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
                SaveEnvironment(oNode, _platformid, Int32.Parse(oNode.Value));
        }
        private void SaveEnvironment(TreeNode oParent, int _platformid, int _classid)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oForecast.AddQuestionPlatform(intQuestion, _platformid, _classid, Int32.Parse(oNode.Value));
            }
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
