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
    public partial class sys_new : System.Web.UI.UserControl
    {

        protected DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected New oNew;
        protected Functions oFunction;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oNew = new New(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            if (Request.Path == "/index.aspx")
                Response.Redirect("/interior.aspx");
            rptNew.DataSource = oNew.Gets(1);
            rptNew.DataBind();
            foreach (RepeaterItem ri in rptNew.Items)
            {
                Label lblAttachment = (Label)ri.FindControl("lblAttachment");
                if (lblAttachment.Text != "")
                {
                    Panel panAttachment = (Panel)ri.FindControl("panAttachment");
                    panAttachment.Visible = true;
                    string strFile = lblAttachment.Text;
                    strFile = strFile.Substring(strFile.LastIndexOf("/") + 1);
                    lblAttachment.Text = "<img src=\"/images/file.gif\" align=\"absmiddle\" border=\"0\"/> <a href=\"" + lblAttachment.Text + "\" target=\"_blank\">" + strFile + "</a>";
                }
            }
            lblNew.Visible = (rptNew.Items.Count == 0);
        }
    }
}