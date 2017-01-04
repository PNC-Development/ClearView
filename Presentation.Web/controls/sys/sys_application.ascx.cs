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
    public partial class sys_application : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected int intClearViewUsersGroupID = Int32.Parse(ConfigurationManager.AppSettings["ClearViewUsersGroupID"]);
        private NCC.ClearView.Application.Core.Roles oRole;
        protected Permissions oPermission;
        protected ColorLevels oColor;
        protected string strMenu;
        protected bool boolRedirect = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPermission = new Permissions(intProfile, dsn);
            oRole = new NCC.ClearView.Application.Core.Roles(intProfile, dsn);
            oColor = new ColorLevels();
            DataSet ds = oRole.Gets(intProfile);
            int intApplications = 0;
            string strDefaultApp = "";
            StringBuilder sb = new StringBuilder(strMenu);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intApplications++;
                strDefaultApp = "/" + dr["url"].ToString() + "/default.aspx";
                string strColor = oColor.Name(Int32.Parse(dr["permission"].ToString()));

                sb.Append("<tr><td>&nbsp;</td><td><img src=\"/images/");
                sb.Append(strColor);
                sb.Append("level.gif\" title=\"");
                sb.Append(strColor);
                sb.Append("\" border=\"0\" align=\"absmiddle\">&nbsp;<a href=\"/");
                sb.Append(dr["url"].ToString());
                sb.Append("/default.aspx\">");
                sb.Append(dr["name"].ToString());
                sb.Append("</a></td></tr>");
            }

            if (sb.ToString() != "")
            {
                if (intApplications == 1 && boolRedirect == true && strDefaultApp != "")
                {
                    Response.Redirect(strDefaultApp);
                }
                else
                {
                    sb.Insert(0, "<tr height=\"5\"><td colspan=\"2\"><img src=\"/images/spacer.gif\" border=\"0\" height=\"5\" width=\"1\" /></td></tr>");
                }
            }
            else
            {
                //sb.Append("<p><font class=\"error\">There are no applications...</font><p/>");
                // Add the default application
                oRole.Add(intProfile, intClearViewUsersGroupID);
                Response.Redirect(Request.Url.PathAndQuery);
            }

            strMenu = sb.ToString();
        }
        protected void btnLogout_Click(Object Sender, EventArgs e)
        {
            Response.Cookies["profileid"].Value = "";
            Response.Redirect("/index.aspx?logout=true");
        }
    }
}