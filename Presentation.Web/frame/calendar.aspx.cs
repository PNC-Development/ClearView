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
    public partial class calendar : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strMinimum;
        protected Holidays oHoliday;
        protected void Page_Load(object sender, EventArgs e)
        {
            oHoliday = new Holidays(0, dsn);
            if (!IsPostBack)
            {
                double dblMinimum = 0.00;
                if (Request.QueryString["min"] != null)
                    double.TryParse(Request.QueryString["min"], out dblMinimum);
                if (dblMinimum > 0.00)
                    strMinimum = oHoliday.GetDays(dblMinimum, DateTime.Today).ToShortDateString();
                if (Request.QueryString["date"] != null)
                {
                    try
                    {
                        UpdateCalendars(DateTime.Parse(Request.QueryString["date"]));
                    }
                    catch
                    {
                        UpdateCalendars(DateTime.Parse(DateTime.Today.ToShortDateString()));
                    }
                }
                else
                    UpdateCalendars(DateTime.Parse(DateTime.Today.ToShortDateString()));
                btnToday.Attributes.Add("onclick", "DateSelect('" + DateTime.Today.ToShortDateString() + "');");
            }
        }
        protected void DayRender(Object Sender, DayRenderEventArgs e)
        {
            e.Day.IsSelectable = false;
            DateTime datTemp = DateTime.Today;
            if (strMinimum == "" || DateTime.TryParse(strMinimum, out datTemp) == false || e.Day.Date >= DateTime.Parse(strMinimum))
            {
                if (e.Day.Date != calThis.SelectedDate)
                {
                    e.Cell.Attributes.Add("onmouseover", "CalendarOver(this);");
                    e.Cell.Attributes.Add("onmouseout", "CalendarOut(this);");
                }
                e.Cell.Attributes.Add("onclick", "DateSelect('" + e.Day.Date.ToShortDateString() + "');");
            }
            else
            {
                e.Cell.CssClass = "component_unavailable";
            }
            e.Cell.ToolTip = e.Day.Date.ToLongDateString();
        }
        protected void ChangeDate(Object Sender, EventArgs e)
        {
            System.Web.UI.WebControls.Calendar _calendar = (System.Web.UI.WebControls.Calendar)Sender;
            DateTime _date = _calendar.SelectedDate;
            UpdateCalendars(_date);
        }
        protected void ChangeMonth(Object sender, MonthChangedEventArgs e)
        {
            DateTime _date = DateTime.Parse(Request.Cookies["calendardate"].Value);
            if (e.NewDate < e.PreviousDate)
                _date = _date.AddMonths(-1);
            else
                _date = _date.AddMonths(1);
            while (_date.Day != 1)
                _date = _date.AddDays(-1);
            while (_date.DayOfWeek == System.DayOfWeek.Saturday || _date.DayOfWeek == System.DayOfWeek.Sunday)
                _date = _date.AddDays(1);
            UpdateCalendars(_date);
        }
        protected void btnToday_Click(Object Sender, EventArgs e)
        {
            UpdateCalendars(DateTime.Today);
        }
        public void UpdateCalendars(DateTime _date)
        {
            Response.Cookies["calendardate"].Value = _date.ToString();
            calThis.VisibleDate = _date;
            calThis.SelectedDate = _date;
        }
    }
}
