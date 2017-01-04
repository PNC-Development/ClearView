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
    public partial class did_you_know : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intProfile;
        protected DidYouKnow oDidYouKnow;
        protected string strKnow = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oDidYouKnow = new DidYouKnow(intProfile, dsn);
            DataSet ds = oDidYouKnow.Gets();
            if (ds.Tables[0].Rows.Count == 0)
            {
                strKnow = "<p><div align=\"center\"><br/><img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\"/> &quot;Did You Know&quot; has not been configured.</div></p>";
                lblTip.Text = "Tip # 0 / 0";
            }
            else
            {
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    int intId = Int32.Parse(Request.QueryString["id"]);
                    lblTip.Text = "Tip # " + intId.ToString() + " / " + ds.Tables[0].Rows.Count;
                    intId = intId - 1;
                    strKnow = ds.Tables[0].Rows[intId]["description"].ToString();
                }
                else
                {
                    Random oRandom = new Random(DateTime.Now.Millisecond);
                    int intId = oRandom.Next(1, ds.Tables[0].Rows.Count);
                    Response.Redirect(Request.Path + "?id=" + intId.ToString());
                }
            }
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            DataSet ds = oDidYouKnow.Gets();
            int intId = Int32.Parse(Request.QueryString["id"]);
            if (intId == ds.Tables[0].Rows.Count)
                intId = 1;
            else
                intId = intId + 1;
            Response.Redirect(Request.Path + "?id=" + intId.ToString());
        }
    }
}
