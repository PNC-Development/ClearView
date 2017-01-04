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
    public partial class implementation_calendar : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intVacationPage = Int32.Parse(ConfigurationManager.AppSettings["VacationRequest"]);
        protected Pages oPage;
        protected ResourceRequest oResourceRequest;
        protected Users oUser;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected int intRows = 5;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            DateTime _date = DateTime.Today;
            if (Request.QueryString["d"] != null)
            {
                DateTime.TryParse(Request.QueryString["d"], out _date);
            }
            DataSet ds = oResourceRequest.GetChangeControlsUser(intProfile);
            rptView.DataSource = ds;
            rptView.DataBind();
            lblNone.Visible = (rptView.Items.Count == 0);
            calThis.VisibleDate = _date;
            calThis.SelectedDate = _date;
            ddlYear.SelectedValue = _date.Year.ToString();
            ddlMonth.SelectedValue = _date.Month.ToString();
            btnToday.ToolTip = DateTime.Today.ToLongDateString();
        }
        protected void DayRender(Object Sender, DayRenderEventArgs e)
        {
            e.Day.IsSelectable = false;
            if (e.Day.Date != calThis.SelectedDate)
            {
                e.Cell.Attributes.Add("onmouseover", "CalendarOver(this);");
                e.Cell.Attributes.Add("onmouseout", "CalendarOut(this);");
            }
            e.Cell.Attributes.Add("onclick", "ShowChanges('" + e.Day.Date.ToShortDateString() + "');");
            e.Cell.ToolTip = e.Day.Date.ToLongDateString();
            e.Cell.Text = "<table cellpadding=\"1\" cellspacing=\"0\" border=\"0\">";
            if (e.Day.IsOtherMonth == false && e.Day.IsWeekend == false)
                e.Cell.Text += "<tr><td class=\"calendarhead\">" + e.Day.DayNumberText + "</td></tr>";
            else
                e.Cell.Text += "<tr><td class=\"calendarotherhead\">" + e.Day.DayNumberText + "</td></tr>";
            DataSet ds = oResourceRequest.GetChangeControlsDate(e.Day.Date);
            int intCount = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (intCount > intRows)
                    break;
                string strName = dr["projectname"].ToString();
                string strNumber = dr["projectnumber"].ToString();
                string strChange = dr["number"].ToString();
                int intUser = Int32.Parse(dr["userid"].ToString());
                string strTime = DateTime.Parse(dr["implementation"].ToString()).ToShortTimeString();
                e.Cell.Text += "<tr><td class=\"smallcalendar\"><a href=\"javascript:void(0);\" class=\"smallcalendar\" onclick=\"ShowChange('" + dr["changeid"].ToString() + "');\" title=\"Change Control: " + strChange + "&#13;Project Name:" + strName + "&#13;Project Number:" + strNumber + "&#13;Technician: " + oUser.GetFullName(intUser) + "&#13;Start Time: " + strTime + "\">" + strChange + "&nbsp;(" + oUser.GetName(intUser) + ")</a></td></tr>";
                intCount++;
            }
            e.Cell.Text += "<tr><td class=\"smallcalendar\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"150\" height=\"1\"/></td></tr>";
            e.Cell.Text += "</table>";
        }
        protected void ChangeDate(Object Sender, EventArgs e)
        {
            System.Web.UI.WebControls.Calendar _calendar = (System.Web.UI.WebControls.Calendar)Sender;
            Response.Redirect(oPage.GetFullLink(intPage) + "?d=" + Server.UrlPathEncode(_calendar.SelectedDate.ToShortDateString()));
        }
        protected void ChangeMonth(Object sender, MonthChangedEventArgs e)
        {
            DateTime _date = calThis.SelectedDate;
            if (e.NewDate < e.PreviousDate)
                _date = _date.AddMonths(-1);
            else
                _date = _date.AddMonths(1);
            while (_date.Day != 1)
                _date = _date.AddDays(-1);
            Response.Redirect(oPage.GetFullLink(intPage) + "?d=" + Server.UrlPathEncode(_date.ToShortDateString()));
        }
        protected void btnToday_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?d=" + Server.UrlPathEncode(DateTime.Today.ToShortDateString()));
        }
        protected void btnGo_Click(Object Sender, ImageClickEventArgs e)
        {
            DateTime _date = DateTime.Today;
            try { _date = DateTime.Parse(ddlMonth.SelectedItem.Value + "/1/" + ddlYear.SelectedItem.Value); }
            catch { }
            Response.Redirect(oPage.GetFullLink(intPage) + "?d=" + Server.UrlPathEncode(_date.ToShortDateString()));
        }
    }
}