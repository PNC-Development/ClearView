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
    public partial class frame_project_request_question_class : BasePage
    {
        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected int intQuestion;

        // Vijay Code - Start
        protected ProjectRequest oProjectRequest;
        // Vijay Code - End
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();

            // Vijay Code - Start
            oProjectRequest = new ProjectRequest(intProfile, dsn);

            // Vijay Code - End
            if (Request.QueryString["questionid"] != null && Request.QueryString["questionid"] != "")
                intQuestion = Int32.Parse(Request.QueryString["questionid"]);

            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                ClientScript.RegisterStartupScript(this.GetType(), "test", "alert('Record saved');", true);

            btnClose.Attributes.Add("onclick", "return HidePanel();");
            //  btnAdd.Attributes.Add("onclick", "return Add("+drpClass.ClientID+","+drpWeight.ClientID+");");
            if (hdnTest.Value != "" && hdnTest.Value != null)
            {
                string[] strSplit = hdnTest.Value.Split('&');
                // oProjectRequest.AddResponseClass(Int32.Parse(lblId.Text), Int32.Parse(strSplit[0]), Int32.Parse(strSplit[1]));            

                Response.Redirect(Request.UrlReferrer.AbsoluteUri + "&save=true");
            }
            if (!IsPostBack)
            {
                ds = oProjectRequest.GetClasses(1);
                DataSet dsQ = oProjectRequest.GetQuestionsClass(intQuestion);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    TreeNode oNode = new TreeNode();
                    oNode.Text = dr["name"].ToString();
                    oNode.ToolTip = dr["name"].ToString();
                    oNode.Value = dr["id"].ToString();
                    oNode.SelectAction = TreeNodeSelectAction.Select;
                    oNode.Checked = false;
                    foreach (DataRow dr2 in dsQ.Tables[0].Rows)
                    {
                        if (dr2["classid"].ToString() == oNode.Value.ToString())
                            oNode.Checked = true;
                    }
                    oTreeview.Nodes.Add(oNode);
                }
            }

        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oProjectRequest.DeleteQuestionsClass(intQuestion);
            foreach (TreeNode oNode in oTreeview.Nodes)
            {
                if (oNode.Checked == true)
                    oProjectRequest.AddQuestionsClass(intQuestion, Int32.Parse(oNode.Value));
            }
            Reload();
        }

        protected  void btnDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            int intid = Int32.Parse(oButton.CommandArgument);
            //oProjectRequest.DeleteResponseClass(intid);
            Response.Redirect(Request.UrlReferrer.AbsoluteUri);
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
