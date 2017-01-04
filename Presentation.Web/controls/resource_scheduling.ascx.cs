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
using System.Text.RegularExpressions;
using System.Drawing;
namespace NCC.ClearView.Presentation.Web
{
    public partial class resource_scheduling : System.Web.UI.UserControl
    {

        private DataSet ds;
        private DataSet ds2;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Pages oPage;
        private NCC.ClearView.Application.Core.ResourceScheduling oResourceSch;

        protected bool boolMonthly = false;
        protected bool boolWeekly = false;
        protected bool boolDaily = false;

        private DateTime current_date;
        private DateTime start_date;
        private DateTime end_date;
        private DateTime start_time;
        private DateTime end_time;
        private DateTime dt;

        protected string strHTML = "";
        protected string strWeekView = "";

        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProfile;

        private Table table;
        private TableCell td;
        private TableRow tr;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oResourceSch = new NCC.ClearView.Application.Core.ResourceScheduling(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");

            if (Request.QueryString["dt"] != null)
                dt = DateTime.Parse(Request.QueryString["dt"]);
            else
                dt = DateTime.Now;

            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "success", "alert('Resource added successfully!');", true);
            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "success", "alert('Resource was not added successfully!\\n\\nThere was a conflict in this resources schedule. Please select a new time.);", true);

            if (Request.QueryString["d"] != null)
                dt = DateTime.Parse(Request.QueryString["d"]);
            else
                dt = DateTime.Now;

            if (!IsPostBack)
                LoadLists();
            CalendarMonth.VisibleDate = dt;
            CalendarMonth.SelectedDate = DateTime.Now;
            imgStartDate.Attributes.Add("onclick", "return ShowCalendar('" + txtStartDate.ClientID + "');");
            imgEndDate.Attributes.Add("onclick", "return ShowCalendar('" + txtEndDate.ClientID + "');");
            btnAdd.Attributes.Add("onclick", "return ValidateDropDown('" + ddlUser.ClientID + "','Please make a selection for the resource')" +
                  "&& ValidateText('" + txtTitle.ClientID + "','Please enter a title')" +
                  "&& ValidateDate('" + txtStartDate.ClientID + "','Please enter a valid start date')" +
                  "&& ValidateDate('" + txtEndDate.ClientID + "','Please enter a valid end date')" +
                  "&& ValidateDropDown('" + ddlStart.ClientID + "','Please make a selection for the start time')" +
                  "&& ValidateDropDown('" + ddlEnd.ClientID + "','Please make a selection for the end time')" +
                  "&& ValidateDates('" + txtStartDate.ClientID + "','" + txtEndDate.ClientID + "','End Date cannot be less than start date')" +
                 ";");

            if (boolMonthly == false && boolDaily == false && boolWeekly == false)
                boolMonthly = true;

            GenerateMonthlyView();
            GenerateWeeklyView();
            GenerateDailyView();
        }
        private void LoadLists()
        {
            Users oUser = new Users(intProfile, dsn);
            ddlUser.DataValueField = "userid";
            ddlUser.DataTextField = "username";
            ddlUser.DataSource = oUser.Gets(1);
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void GenerateMonthlyView()
        {
            DateTime fromdt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime todt = new DateTime(fromdt.Year, fromdt.AddMonths(1).Month, 1);
            todt = todt.AddDays(-1);
            ds = oResourceSch.GetResourceSchedulingNormalView(DateTime.Parse(fromdt.ToString("M/d/yyyy")), DateTime.Parse(todt.ToString("M/d/yyyy")));
        }
        protected void GenerateDailyView()
        {
            ds2 = oResourceSch.GetResourceSchedulingTodayView(DateTime.Parse(DateTime.Now.ToString("M/d/yyyy")));
            strHTML = "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"default\"> ";
            strHTML += "<tr><td colspan=\"2\" align=\"center\" class=\"greentableheader\">" + DateTime.Now.ToString("dddd, MMMM dd yyyy") + "</td></tr>";

            table = new Table();
            table.Width = Unit.Percentage(100);
            table.CellPadding = 2;
            table.CellSpacing = 0;

            tr = new TableRow();
            td = new TableCell();
            td.Attributes["align"] = "left";
            td.ColumnSpan = 2;
            td.CssClass = "hugeheader";
            td.Text = DateTime.Now.ToString("dddd, MMMM dd yyyy");
            tr.Cells.Add(td);
            table.Rows.Add(tr);

            tr = new TableRow();
            td = new TableCell();
            td.Attributes["align"] = "left";
            td.ColumnSpan = 2;
            td.CssClass = "default";
            td.Text = "&nbsp;";
            tr.Cells.Add(td);
            table.Rows.Add(tr);

            foreach (ListItem item in ddlStart.Items)
            {
                if (item.Text.Trim() != "-- SELECT --")
                {
                    DateTime item_time = DateTime.Parse(item.Text);
                    string[] strArr = item.Text.Replace("AM", "").Replace("PM", "").Split(':');

                    string pattern = @"\w{2}$";

                    string xx = Regex.Match(item.Text, pattern).Groups[0].ToString();


                    tr = new TableRow();
                    TableCell td1 = new TableCell();
                    TableCell td2 = new TableCell();
                    td1.VerticalAlign = VerticalAlign.Top;
                    td1.Text = "<font size=\"4\"> " + strArr[0] + "</font><span style=\"vertical-align: super;\">" + strArr[1] + "</span>" + "<font size=\"1.5\">" + xx + "</font>&nbsp;&nbsp;&nbsp;";
                    string strBackColor = "#FFFFFF";
                    td1.BackColor = ColorTranslator.FromHtml(strBackColor);
                    td1.Wrap = false;
                    td2.VerticalAlign = VerticalAlign.Top;
                    td2.BackColor = ColorTranslator.FromHtml(strBackColor);
                    td2.Width = Unit.Percentage(100.00);

                    start_time = DateTime.Parse(ds2.Tables[0].Rows[0]["starttime"].ToString());
                    end_time = DateTime.Parse(ds2.Tables[0].Rows[0]["endtime"].ToString());

                    if (item_time >= start_time && item_time <= end_time)
                    {
                        strBackColor = "#F6F6F6";
                        td1.BackColor = ColorTranslator.FromHtml(strBackColor);
                        td2.BackColor = ColorTranslator.FromHtml(strBackColor);
                        td2.Text += ds2.Tables[0].Rows[0]["username"].ToString().Trim() + "<br/>" + ds2.Tables[0].Rows[0]["title"].ToString().Trim();
                    }
                    tr.Cells.Add(td1);
                    tr.Cells.Add(td2);
                    table.Rows.Add(tr);
                    tr = new TableRow();
                    td = new TableCell();
                    td.ColumnSpan = 2;
                    td.CssClass = "default";
                    td.BackColor = ColorTranslator.FromHtml(strBackColor);
                    td.Text = "<span style=\"width:100%;border-bottom:1 dotted #CCCCCC;\"/>";
                    tr.Cells.Add(td);
                    table.Rows.Add(tr);
                }
            }
            strHTML += "</table>";
            phDiv.Controls.Add(table);
        }
        protected void GenerateWeeklyView()
        {
            DateTime _today = DateTime.Now;
            switch (_today.DayOfWeek)
            {
                case System.DayOfWeek.Monday: break;
                case System.DayOfWeek.Tuesday: _today = _today.AddDays(-1); break;
                case System.DayOfWeek.Wednesday: _today = _today.AddDays(-2); break;
                case System.DayOfWeek.Thursday: _today = _today.AddDays(-3); break;
                case System.DayOfWeek.Friday: _today = _today.AddDays(-4); break;
                case System.DayOfWeek.Saturday: _today = _today.AddDays(-5); break;
                case System.DayOfWeek.Sunday: _today = _today.AddDays(-6); break;
            }
            string strRow1 = "";
            string strRow2 = "";
            string strRow3 = "";
            string strRow4 = "";

            for (int intCount = 0; intCount < 7; intCount++)
            {
                string strWeek = "";
                strWeek += "<td class=\"default\"" + (System.DayOfWeek.Wednesday == _today.DayOfWeek ? " rowspan=\"2\"" : "") + ">";
                string strHeight = (_today.DayOfWeek == System.DayOfWeek.Saturday || _today.DayOfWeek == System.DayOfWeek.Sunday ? "61" : "150");
                strWeek += "<table height=\"" + strHeight + "\" width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"border:solid 1px #CCCCCC\">";
                strWeek += "<tr height=\"1\" bgcolor=\"#EEEEEE\"><td align=\"center\" class=\"bold\">" + _today.ToString("dddd, MMMM dd") + "</td></tr>";

                strWeek += "<tr><td valign=\"top\"><div style=\"width:100%;height:" + strHeight + "px;overflow:auto\">";
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    start_date = DateTime.Parse(row["startdate"].ToString());
                    end_date = DateTime.Parse(row["enddate"].ToString());

                    if ((start_date.Date <= _today.Date && end_date.Date >= _today.Date) || (start_date.Date >= _today.Date && end_date.Date <= _today.Date))
                    {
                        strWeek += row["username"].ToString().Trim() + "<br>" + row["title"].ToString().Trim() + "<br>" + row["starttime"].ToString().Trim() + "-" + row["endtime"].ToString().Trim() + "<br>[<a href=\"javascript:void()\" onclick=\"test();\" class=\"default\">Delete</a>]";
                        strWeek += "<span style=\"width:100%;border-bottom:1 dotted #CCCCCC;\"/></span>";
                    }

                }
                if (strWeek == "")
                    strWeek += "&nbsp;";

                strWeek += "</div></td></tr>";
                strWeek += "</table></td>";

                switch (_today.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        strRow1 += strWeek;
                        break;
                    case System.DayOfWeek.Tuesday:
                        strRow2 += strWeek;
                        break;
                    case System.DayOfWeek.Wednesday:
                        strRow3 += strWeek;
                        break;
                    case System.DayOfWeek.Thursday:
                        strRow1 += strWeek;
                        break;
                    case System.DayOfWeek.Friday:
                        strRow2 += strWeek;
                        break;
                    case System.DayOfWeek.Saturday:
                        strRow3 += strWeek;
                        break;
                    case System.DayOfWeek.Sunday:
                        strRow4 += strWeek;
                        break;
                }

                _today = _today.AddDays(1);
            }

            strWeekView = "<table width=\"100%\" cellpadding=\"2\" cellspacing=\"2\" border=\"0\" class=\"default\"> ";
            strWeekView += "<tr>" + strRow1 + "</tr>";
            strWeekView += "<tr>" + strRow2 + "</tr>";
            strWeekView += "<tr>" + strRow3 + "</tr>";
            strWeekView += "<tr>" + strRow4 + "</tr>";
            strWeekView += "</table>";

        }
        protected void DayRender(object sender, System.Web.UI.WebControls.DayRenderEventArgs e)
        {
            e.Cell.ToolTip = e.Day.Date.ToLongDateString();
            int intCount = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                e.Cell.Text = "<div style=\"width:100%;height:100%;overflow:visible\">";
                e.Cell.Text += "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
                if (e.Day.IsOtherMonth == false && e.Day.IsWeekend == false)
                    e.Cell.Text += "<tr><td class=\"calendarhead\">" + e.Day.DayNumberText + "</td></tr>";
                else
                    e.Cell.Text += "<tr><td class=\"calendarotherhead\">" + e.Day.DayNumberText + "</td></tr>";

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    start_date = DateTime.Parse(dr["startdate"].ToString());
                    end_date = DateTime.Parse(dr["enddate"].ToString());
                    current_date = e.Day.Date;

                    if (current_date >= start_date && current_date <= end_date)
                    {
                        e.Cell.Text += "<tr><td class=\"default\">" + dr["username"].ToString().Trim() + "<br>" + dr["title"].ToString().Trim() + "<br>" + dr["starttime"].ToString().Trim() + "-" + dr["endtime"].ToString().Trim() + "<br>[<a href=\"javascript:void()\" onclick=\"test();\" class=\"default\">Delete</a>]" + "<span style=\"width:100%;border-bottom:1 dotted #CCCCCC;\"/></span></td></tr>";
                        e.Cell.Text += "";
                    }
                    intCount++;
                }

                e.Cell.Text += "</table>";
                e.Cell.Text += "</div>";
            }
        }
        protected void ChangeMonth(Object sender, MonthChangedEventArgs e)
        {
            DateTime _date = CalendarMonth.SelectedDate;
            if (e.NewDate < e.PreviousDate)
                _date = _date.AddMonths(-1);
            else
                _date = _date.AddMonths(1);
            while (_date.Day != 1)
                _date = _date.AddDays(-1);

            Response.Redirect(oPage.GetFullLink(intPage) + "?d=" + Server.UrlPathEncode(_date.ToShortDateString()));
        }
        protected void btnAdd_Click(object Sender, EventArgs e)
        {
            int intError = oResourceSch.Add(Int32.Parse(ddlStart.SelectedItem.Value), txtTitle.Text.Trim(), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text), ddlStart.SelectedItem.Value, ddlEnd.SelectedItem.Value);
            if (intError == -1)
                Response.Redirect(oPage.GetFullLink(intPage) + "?error=true");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?save=true");
        }    
    }
}