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
    public partial class resource_request_sla : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;

        protected ResourceRequest oResourceRequest;
        protected Services oService;
        protected Holidays oHoliday;
        protected Users oUser;
        protected string strView = "SLA Information Unavailable";
        protected void Page_Load(object sender, EventArgs e)
        {
            oResourceRequest = new ResourceRequest(0, dsn);
            oService = new Services(0, dsn);
            oHoliday = new Holidays(0, dsn);
            oUser = new Users(0, dsn);
            if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
            {
                int intService = Int32.Parse(oResourceRequest.Get(Int32.Parse(Request.QueryString["rrid"]), "serviceid"));
                DateTime datTime = DateTime.Now;
                string strAssigned = oResourceRequest.Get(Int32.Parse(Request.QueryString["rrid"]), "assigned");
                if (strAssigned != "")
                    datTime = DateTime.Parse(strAssigned);
                double dblSLA = oService.GetSLA(intService);
                if (dblSLA > 0.00)
                {
                    panShow.Visible = true;
                    lblAssigned.Text = datTime.ToLongDateString();
                    dblSLA = dblSLA / 8.00;
                    datTime = oHoliday.GetDays(dblSLA, datTime);
                    lblDays.Text = dblSLA.ToString();
                    lblDeadline.Text = datTime.ToLongDateString();
                }
                else
                {
                    panHide.Visible = true;
                    string strManager = "";
                    DataSet dsManagers = oService.GetUser(intService, -1);
                    foreach (DataRow drManager in dsManagers.Tables[0].Rows)
                        strManager += "<img src=\"/images/user.gif\" border=\"0\" align=\"absmiddle\"/> " + oUser.GetFullName(Int32.Parse(drManager["userid"].ToString())) + "<br/>";
                    lblManager.Text = strManager;
                }
            }
            btnClose.Attributes.Add("onclick", "parent.HidePanel();");
        }
    }
}
