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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class project_info : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected int intProfile;
        protected string strBody = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            Projects oProject = new Projects(intProfile, dsn);
            Organizations oOrganization = new Organizations(intProfile, dsn);
            Segment oSegment = new Segment(intProfile, dsn);
            Users oUser = new Users(intProfile, dsn);
            StatusLevels oStatusLevel = new StatusLevels();
            StringBuilder sb = new StringBuilder(strBody);

            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                int intProject = Int32.Parse(Request.QueryString["id"]);
                DataSet ds = oProject.Get(intProject);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    sb.Append("<tr><td nowrap><b>Project Name:</b></td><td>");
                    sb.Append(ds.Tables[0].Rows[0]["name"].ToString());
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap><b>Project Number:</b></td><td>");
                    sb.Append(ds.Tables[0].Rows[0]["number"].ToString());
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap><b>Initiative Type:</b></td><td>");
                    sb.Append(ds.Tables[0].Rows[0]["bd"].ToString());
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap><b>Organization:</b></td><td>");
                    sb.Append(oOrganization.GetName(Int32.Parse(ds.Tables[0].Rows[0]["organization"].ToString())));
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap><b>Segment:</b></td><td>");
                    sb.Append(oSegment.GetName(Int32.Parse(ds.Tables[0].Rows[0]["segmentid"].ToString())));
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap><b>Project Lead:</b></td><td>");
                    sb.Append(oUser.GetFullName(Int32.Parse(ds.Tables[0].Rows[0]["lead"].ToString())));
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap><b>Executive Sponsor:</b></td><td>");
                    sb.Append(oUser.GetFullName(Int32.Parse(ds.Tables[0].Rows[0]["executive"].ToString())));
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap><b>Working Sponsor:</b></td><td>");
                    sb.Append(oUser.GetFullName(Int32.Parse(ds.Tables[0].Rows[0]["working"].ToString())));
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap><b>Integration Engineer:</b></td><td>");
                    sb.Append(oUser.GetFullName(Int32.Parse(ds.Tables[0].Rows[0]["engineer"].ToString())));
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap><b>Technical Lead:</b></td><td>");
                    sb.Append(oUser.GetFullName(Int32.Parse(ds.Tables[0].Rows[0]["technical"].ToString())));
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap><b>Status:</b></td><td>");
                    sb.Append(oStatusLevel.HTML(Int32.Parse(ds.Tables[0].Rows[0]["status"].ToString())));
                    sb.Append("</td></tr>");
                }
            }

            if (sb.ToString() != "")
            {
                sb.Insert(0, "<table border=\"0\" cellpadding=\"4\" cellspacing=\"3\">");
                sb.Append("</table>");
            }

            strBody = sb.ToString();
        }
    }
}
