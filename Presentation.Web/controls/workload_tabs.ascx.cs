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
using NCC.ClearView.Presentation.Web.Custom;
namespace NCC.ClearView.Presentation.Web
{
    public partial class workload_tabs : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProfile;
        protected Tabs oTabs;
        protected ResourceRequest oResourceRequest;
        protected RequestItems oRequestItem;
        protected Services oService;
        protected string strTabs = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oTabs = new Tabs(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
            Tab oTab = new Tab("", intMenuTab, "divMenu1", true, false);

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != "")
            {
                int intProject = Int32.Parse(Request.QueryString["pid"]);
                DataSet ds = oResourceRequest.GetProjectUser(intProject, intProfile);
                int intItem = -1;
                bool boolTasks = false;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (intItem != Int32.Parse(dr["itemid"].ToString()))
                    {
                        if (intItem > -1)
                            boolTasks = true;
                        intItem = Int32.Parse(dr["itemid"].ToString());
                        int intService = Int32.Parse(dr["serviceid"].ToString());
                        if (boolTasks == false)
                            boolTasks = (oService.Get(intService, "tasks") == "1");
                        if (boolTasks == true)
                            break;
                    }
                }
                ds = oTabs.GetRequestItemsTabs(intItem);
                int intCount = 0;
               
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    intCount++;
                    HtmlGenericControl oDiv = new HtmlGenericControl("DIV");
                    if (intCount == 1)
                    {
                        oDiv.Style["display"] = "inline";
                        //oDiv.ID = "divTab" + intCount.ToString();
                        oDiv.Controls.Add((Control)LoadControl(dr["path"].ToString()));
                        PHDiv.Controls.Add(oDiv);
                        oTab.AddTab(dr["tabname"].ToString(), "");
                        //strTab += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'" + oDiv.ClientID + "',null,null,false);\" class=\"tabheader\">" + dr["tabname"].ToString() + "</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
                    }
                    else
                    {
                        oDiv.Style["display"] = "none";
                        //oDiv.ID = "divTab" + intCount.ToString();
                        oDiv.Controls.Add((Control)LoadControl(dr["path"].ToString()));
                        PHDiv.Controls.Add(oDiv);
                        oTab.AddTab(dr["tabname"].ToString(), "");
                        //strTab += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'" + oDiv.ClientID + "',null,null,false);\" class=\"tabheader\">" + dr["tabname"].ToString() + "</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
                    }
                }
                strTabs = oTab.GetTabs();
                //if (strTab != "")
                //    strTabs += "<tr>" + strTab + "<td width=\"100%\" background=\"/images/TabEmptyBackground.gif\">&nbsp;</td></tr>";
                //if (strTabs == "")
                //    strTabs += "<tr><td class=\"header\"><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> The workload manager tabs have not been configured for this department. Please contact your ClearView administrator for additional information.</td></tr>";
                //strTabs = "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">" + strTabs + "</table>";
                if (boolTasks == true)
                {
                    // Add the tasks tab if necessary
                }
            }
        }
    }
}