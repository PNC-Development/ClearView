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
    public partial class scheduling_controls : System.Web.UI.UserControl
    {

        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intVacationPage = Int32.Parse(ConfigurationManager.AppSettings["VacationRequest"]);
        protected Pages oPage;
        protected Scheduling oScheduling;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        private StringBuilder sb = new StringBuilder();
        private DateTime schd_date;
        private DateTime current_date;
        protected int date_compare;
        private string from, to;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oScheduling = new Scheduling(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["all"] != null)
            {
                calThis.DayStyle.Width = Unit.Percentage(15.00);
                calThis.DayStyle.Height = Unit.Percentage(100.00);
                calThis.Width = Unit.Percentage(100.00);
                calThis.Height = Unit.Percentage(100.00);
            }
            else
            {
                calThis.DayStyle.Width = Unit.Percentage(15.00);
                calThis.DayStyle.Height = Unit.Pixel(80);
                calThis.Width = Unit.Percentage(100.00);
                calThis.Height = Unit.Pixel(500);
            }
            lblTitle.Text = oPage.Get(intPage, "title");
            DateTime _date = DateTime.Today;
            if (Request.QueryString["d"] != null)
            {
                DateTime.TryParse(Request.QueryString["d"], out _date);
            }
            calThis.VisibleDate = _date;
            calThis.SelectedDate = _date;
            ddlYear.SelectedValue = _date.Year.ToString();
            ddlMonth.SelectedValue = _date.Month.ToString();
            btnToday.ToolTip = DateTime.Today.ToLongDateString();


            // DateTime fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime fromDate, toDate;
            if (_date != DateTime.Today)
                fromDate = _date;
            else
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            from = fromDate.ToString("M/d/yyyy");

            toDate = new DateTime(fromDate.Year, fromDate.AddMonths(1).Month, 1);
            toDate = toDate.AddDays(-1);

            to = toDate.ToString("M/d/yyyy");

            ds = oScheduling.GetSch(DateTime.Parse(from), DateTime.Parse(to), -1);
        }
        protected void DayRender(Object Sender, DayRenderEventArgs e)
        {
            e.Day.IsSelectable = false;
            e.Cell.ToolTip = e.Day.Date.ToLongDateString();
            StringBuilder sb = new StringBuilder();
            sb.Append("<div style=\"width:100%;height:100%;overflow:visible\">");
            sb.Append("<table width=\"100%\" cellpadding=\"1\" cellspacing=\"0\" border=\"0\">");

            if (e.Day.IsOtherMonth == false && e.Day.IsWeekend == false)
            {
                sb.Append("<tr><td class=\"calendarhead\">");
                sb.Append(e.Day.DayNumberText);
                sb.Append("</td></tr>");
            }
            else
            {
                sb.Append("<tr><td class=\"calendarotherhead\">");
                sb.Append(e.Day.DayNumberText);
                sb.Append("</td></tr>");
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    schd_date = DateTime.Parse(dr["date_sch"].ToString());
                    current_date = e.Day.Date;
                    date_compare = DateTime.Compare(current_date, schd_date);

                    if (date_compare == 0 || date_compare > 0 && schd_date == current_date)
                    {
                        if (DateTime.Compare(schd_date, DateTime.Now.Date) < 0)
                        {
                            sb.Append("<tr><td class=\"calendar\">");
                            sb.Append(dr["event"].ToString().Trim());
                            sb.Append("-");
                            sb.Append(dr["start_time"]);
                            sb.Append("</a></span></td></td></td></tr>");
                        }
                        else
                        {
                            if (oScheduling.VerifyUser(intProfile, Int32.Parse(dr["schd_id"].ToString())) == 1)
                            {
                                sb.Append("<tr><td class=\"calendar\"><img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\"/>&nbsp;<a href=\"javascript:void(0);\" onclick=\"DateSelect('?schd_id=");
                                sb.Append(dr["schd_id"]);
                                sb.Append("&date=");
                                sb.Append(e.Day.Date.ToShortDateString());
                                sb.Append("');\">");
                                sb.Append(dr["event"].ToString().Trim());
                                sb.Append(" (");
                                sb.Append(dr["start_time"]);
                                sb.Append(")</a></span></td></td></td></tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td class=\"calendar\"><a href=\"javascript:void(0);\" onclick=\"DateSelect('?schd_id=");
                                sb.Append(dr["schd_id"]);
                                sb.Append("&date=");
                                sb.Append(e.Day.Date.ToShortDateString());
                                sb.Append("');\">");
                                sb.Append(dr["event"].ToString().Trim());
                                sb.Append(" (");
                                sb.Append(dr["start_time"]);
                                sb.Append(")</a></span></td></td></td></tr>");
                            }
                        }
                    }
                }
            }

            sb.Append("</table>");
            sb.Append("</div>");

            e.Cell.Text = sb.ToString();
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