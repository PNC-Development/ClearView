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
    public partial class datapoint_controls : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intDataPointAvailableService = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_SERVICE"]);
        protected int intDataPointAvailableAsset = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_ASSET"]);
        protected int intDataPointAvailableProject = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_PROJECT"]);
        protected int intDataPointAvailablePeople = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_PEOPLE"]);

        protected Pages oPage;
        protected DataPoint oDataPoint;
        protected Users oUser;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strDataPoint = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oDataPoint = new DataPoint(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (intPage == 0)
                lblTitle.Text = "Please select from one of the following...";
            else
                lblTitle.Text = oPage.Get(intPage, "title");

            // SERVICE
            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "SERVICE") == true || intDataPointAvailableService == 1))
                LoadModule("/datapoint/service/datapoint_service_search.aspx", "/images/workload48.gif", "Service Search", "Search on services using a ClearView request name, request #, project name or project #.", "SERVICE");
            else
                LoadModule("", "/images/workload48.gif", "Service Search", "Search on services using a ClearView request name, request #, project name or project #.", "SERVICE");

            // ASSET
            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "ASSET") == true || intDataPointAvailableAsset == 1))
                LoadModule("/datapoint/asset/datapoint_asset_search.aspx", "/images/assets.gif", "Asset Search", "Search on asset information related to servers and workstations.", "ASSET");
            else
                LoadModule("", "/images/assets.gif", "Asset Search", "Search on asset information related to servers and workstations.", "ASSET");

            // PROJECT
            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "PROJECT") == true || intDataPointAvailableProject == 1))
                LoadModule("/datapoint/projects/datapoint_project_search.aspx", "/images/project.gif", "Project Search", "Search on projects.", "PROJECT");
            else
                LoadModule("", "/images/project.gif", "Project Search", "Search on a project to view financials, resource involvement, documents, etc...", "PROJECT");

            // PEOPLE
            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "PEOPLE") == true || intDataPointAvailablePeople == 1))
                LoadModule("/datapoint/people/datapoint_people_search.aspx", "/images/users40.gif", "People Search", "Search on resource.", "PEOPLE");
            else
                LoadModule("", "/images/users40.gif", "People Search", "Search on a resource for request information, management structure, documents, etc...", "PEOPLE");

            // DOCUMENTS
            LoadModule("", "/images/documents_mine.gif", "Documents Search", "Search the ClearView document repository for a file", "DOCUMENTS");
        }

        protected void LoadModule(string _path, string _image, string _title, string _description, string _key)
        {
            StringBuilder sb = new StringBuilder(strDataPoint);

            if (_path != "")
            {
                sb.Append("<tr onmouseover=\"ImageCellRowOver(this);\" onmouseout=\"ImageCellRowOut(this);\" onclick=\"ImageCellRowClick('");
                sb.Append(_path);
                sb.Append("');\">");
            }
            else
            {
                sb.Append("<tr>");
            }

            sb.Append("<td colspan=\"2\">");
            sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"5\" border=\"0\">");
            sb.Append("<tr>");
            sb.Append("<td rowspan=\"2\"><img src=\"");
            sb.Append(_image);
            sb.Append("\" border=\"0\" align=\"absmiddle\" /></td>");
            sb.Append("<td class=\"");
            sb.Append(_path != "" ? "header" : "headergray");
            sb.Append("\" width=\"100%\" valign=\"bottom\">");
            sb.Append(_title);
            sb.Append("</td>");
            sb.Append("</tr><tr>");
            sb.Append("<td width=\"100%\" valign=\"top\">");
            sb.Append(_description);
            sb.Append("</td>");
            sb.Append("</tr></table></td></tr>");

            if (oUser.IsAdmin(intProfile) == true)
            {
                sb.Append("<tr>");
                sb.Append("<td colspan=\"2\"><input type=\"submit\" value=\"Assign Permissions\" onclick=\"return OpenWindow('DATAPOINT_APPLICATIONS','?key=");
                sb.Append(_key);
                sb.Append("');\" class=\"default\" style=\"width:175px;\" /></td>");
                sb.Append("</tr>");
            }

            sb.Append("<tr>");
            sb.Append("<td colspan=\"2\">&nbsp;</td>");
            sb.Append("</tr>");

            strDataPoint = sb.ToString();
        }
    }
}