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
    public partial class vacation_control : System.Web.UI.UserControl
    {

        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intVacationPage = Int32.Parse(ConfigurationManager.AppSettings["VacationRequest"]);
        protected Pages oPage;
        protected AppPages oAppPage;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected Vacation oVacation;
        protected Holidays oHoliday;
        protected Settings oSetting;
        protected Users oUser;
        protected Applications oApplication;
        protected Variables oVariable;
        protected Functions oFunction;
        protected int intRows = 5;
        protected int intLeadDays = 14;
        protected bool boolAll = false;
        private string strEMailIdsBCC = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oAppPage = new AppPages(intProfile, dsn);
            oVacation = new Vacation(intProfile, dsn);
            oHoliday = new Holidays(intProfile, dsn);
            oSetting = new Settings(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oApplication = new Applications(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["all"] != null)
            {
                btnShowAll.Text = "Show Preview of Resources";
                boolAll = true;
                intRows = 100;
                calThis.DayStyle.Width = Unit.Percentage(15.00);
                calThis.DayStyle.Height = Unit.Percentage(100.00);
                calThis.Width = Unit.Percentage(100.00);
                calThis.Height = Unit.Percentage(100.00);
            }
            else
            {
                btnShowAll.Text = "Show Full Listing of Resources";
                calThis.DayStyle.Width = Unit.Percentage(15.00);
                calThis.DayStyle.Height = Unit.Pixel(80);
                calThis.Width = Unit.Percentage(100.00);
                calThis.Height = Unit.Pixel(500);
            }
            lblTitle.Text = oPage.Get(intPage, "title");
            DateTime _date = DateTime.Today;
            double dblFloating = double.Parse(oSetting.Get("floating"));
            double dblPersonal = double.Parse(oSetting.Get("personal"));
            string strVacation = oUser.Get(intProfile, "vacation");
            bool boolDisable = true;
            if (strVacation != "")
            {
                double dblVacation = double.Parse(strVacation);
                if (dblVacation > 0)
                {
                    ds = oVacation.Gets(intProfile, DateTime.Today.Year);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (DateTime.Parse(dr["start_date"].ToString()).Year == DateTime.Today.Year)
                        {
                            if (dr["vacation"].ToString() == "1")
                            {
                                dr["reason"] = "Vacation";
                                if (dr["morning"].ToString() == "1")
                                {
                                    dr["duration"] = "Morning";
                                    dblVacation = dblVacation - .5;
                                }
                                else if (dr["afternoon"].ToString() == "1")
                                {
                                    dr["duration"] = "Afternoon";
                                    dblVacation = dblVacation - .5;
                                }
                                else
                                {
                                    dr["duration"] = "Full Day";
                                    dblVacation = dblVacation - 1;
                                }
                            }
                            else if (dr["holiday"].ToString() == "1")
                            {
                                dr["reason"] = "Floating Holiday";
                                if (dr["morning"].ToString() == "1")
                                {
                                    dr["duration"] = "Morning";
                                    dblFloating = dblFloating - .5;
                                }
                                else if (dr["afternoon"].ToString() == "1")
                                {
                                    dr["duration"] = "Afternoon";
                                    dblFloating = dblFloating - .5;
                                }
                                else
                                {
                                    dr["duration"] = "Full Day";
                                    dblFloating = dblFloating - 1;
                                }
                            }
                            else if (dr["personal"].ToString() == "1")
                            {
                                dr["reason"] = "Personal / Sick Day";
                                if (dr["morning"].ToString() == "1")
                                {
                                    dr["duration"] = "Morning";
                                    dblPersonal = dblPersonal - .5;
                                }
                                else if (dr["afternoon"].ToString() == "1")
                                {
                                    dr["duration"] = "Afternoon";
                                    dblPersonal = dblPersonal - .5;
                                }
                                else
                                {
                                    dr["duration"] = "Full Day";
                                    dblPersonal = dblPersonal - 1;
                                }
                            }
                        }
                    }
                    lblVacation.Text = dblVacation.ToString();
                    lblFloating.Text = dblFloating.ToString();
                    lblPersonal.Text = dblPersonal.ToString();
                    bool boolVacation = true;
                    bool boolFloating = true;
                    bool boolPersonal = true;
                    if (dblVacation < 1)
                    {
                        radVacation.Enabled = false;
                        boolVacation = false;
                    }
                    if (dblFloating < 1)
                    {
                        radHoliday.Enabled = false;
                        boolFloating = false;
                    }
                    if (dblPersonal < 1)
                    {
                        radPersonal.Enabled = false;
                        boolPersonal = false;
                    }
                    if (boolVacation == false && boolFloating == false && boolPersonal == false)
                    {
                        boolDisable = false;
                        radReason.Checked = true;
                        divReason.Style["display"] = "inline";
                    }
                    else
                        boolDisable = false;
                    lblNone.Visible = (ds.Tables[0].Rows.Count == 0);
                    rptView.DataSource = ds;
                    rptView.DataBind();
                    foreach (RepeaterItem ri in rptView.Items)
                    {
                        LinkButton _btnDelete = (LinkButton)ri.FindControl("btnDelete");
                        _btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this event?');");
                        Label _lblDate = (Label)ri.FindControl("lblDate");
                        if (DateTime.Parse(_lblDate.Text) < DateTime.Today)
                            _btnDelete.Visible = false;
                    }
                }
            }
            if (boolDisable == true)
            {
                txtStart.Enabled = false;
                txtEnd.Enabled = false;
                imgStart.Enabled = false;
                imgEnd.Enabled = false;
                radMorning.Enabled = false;
                radAfternoon.Enabled = false;
                radDay.Enabled = false;
                radDays.Enabled = false;
                radVacation.Enabled = false;
                radPersonal.Enabled = false;
                radHoliday.Enabled = false;
                radReason.Enabled = false;
                ddlReason.Enabled = false;
                btnSubmit.Enabled = false;
                lblConfigure.Visible = true;
            }
            if (Request.QueryString["d"] != null)
            {
                DateTime.TryParse(Request.QueryString["d"], out _date);
            }
            calThis.VisibleDate = _date;
            calThis.SelectedDate = _date;
            ddlYear.SelectedValue = _date.Year.ToString();
            ddlMonth.SelectedValue = _date.Month.ToString();
            btnToday.ToolTip = DateTime.Today.ToLongDateString();
            imgStart.Attributes.Add("onclick", "return ShowCalendar('" + txtStart.ClientID + "');");
            imgEnd.Attributes.Add("onclick", "return ShowCalendar('" + txtEnd.ClientID + "');");
            btnSubmit.Attributes.Add("onclick", "return ValidateVacation('" + radMorning.ClientID + "','" + radAfternoon.ClientID + "','" + radDay.ClientID + "','" + radDays.ClientID + "','" + txtStart.ClientID + "','" + txtEnd.ClientID + "','" + radVacation.ClientID + "','" + radHoliday.ClientID + "','" + radPersonal.ClientID + "','" + radReason.ClientID + "','" + ddlReason.ClientID + "');");
            radMorning.Attributes.Add("onclick", "Single('" + divStartDate.ClientID + "','" + divEndDate.ClientID + "','" + divType.ClientID + "','" + divReason.ClientID + "','" + divSubmit.ClientID + "');");
            radAfternoon.Attributes.Add("onclick", "Single('" + divStartDate.ClientID + "','" + divEndDate.ClientID + "','" + divType.ClientID + "','" + divReason.ClientID + "','" + divSubmit.ClientID + "');");
            radDay.Attributes.Add("onclick", "Single('" + divStartDate.ClientID + "','" + divEndDate.ClientID + "','" + divType.ClientID + "','" + divReason.ClientID + "','" + divSubmit.ClientID + "');");
            radDays.Attributes.Add("onclick", "Multiple('" + divStartDate.ClientID + "','" + divEndDate.ClientID + "','" + divType.ClientID + "','" + divReason.ClientID + "','" + divSubmit.ClientID + "');");
            radVacation.Attributes.Add("onclick", "ShowHideDiv('" + divReason.ClientID + "','none');");
            radPersonal.Attributes.Add("onclick", "ShowHideDiv('" + divReason.ClientID + "','none');");
            radHoliday.Attributes.Add("onclick", "ShowHideDiv('" + divReason.ClientID + "','none');");
            radReason.Attributes.Add("onclick", "ShowHideDiv('" + divReason.ClientID + "','inline');");
        }
        protected void DayRender(Object Sender, DayRenderEventArgs e)
        {
            e.Day.IsSelectable = false;
            if (e.Day.Date != calThis.SelectedDate)
            {
                e.Cell.Attributes.Add("onmouseover", "CalendarOver(this);");
                e.Cell.Attributes.Add("onmouseout", "CalendarOut(this);");
            }
            e.Cell.Attributes.Add("onclick", "DateSelect('" + e.Day.Date.ToShortDateString() + "');");
            e.Cell.ToolTip = e.Day.Date.ToLongDateString();
            if (boolAll == false)
                e.Cell.Text = "<div style=\"width:100%;height:80;overflow:hidden\">";
            else
                e.Cell.Text = "<div style=\"width:100%;height:100%;overflow:visible\">";
            e.Cell.Text += "<table width=\"100%\" cellpadding=\"1\" cellspacing=\"0\" border=\"0\">";
            if (e.Day.IsOtherMonth == false && e.Day.IsWeekend == false)
                e.Cell.Text += "<tr><td class=\"calendarhead\">" + e.Day.DayNumberText + "</td></tr>";
            else
                e.Cell.Text += "<tr><td class=\"calendarotherhead\">" + e.Day.DayNumberText + "</td></tr>";
            ds = oHoliday.Get(e.Day.Date);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                    e.Cell.Text += "<tr><td class=\"calendarred\">" + dr["name"].ToString() + "</span></td></tr>";
            }
            else
            {
                ds = oVacation.Get(e.Day.Date, intApplication);
                int intCount = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    while (intCount < ds.Tables[0].Rows.Count && intCount < intRows)
                    {
                        string strType = "";
                        string strReason = ds.Tables[0].Rows[intCount]["reason"].ToString();
                        if (ds.Tables[0].Rows[intCount]["vacation"].ToString() == "1")
                        {
                            strType = "V:";
                            strReason = "Vacation";
                        }
                        else if (ds.Tables[0].Rows[intCount]["holiday"].ToString() == "1")
                        {
                            strType = "F:";
                            strReason = "Floating Holiday";
                        }
                        else if (ds.Tables[0].Rows[intCount]["personal"].ToString() == "1")
                        {
                            strType = "P:";
                            strReason = "Personal / Sick day";
                        }
                        else if (strReason == "Highland Hills")
                            strType = "HH:";
                        if (e.Day.IsOtherMonth == false && e.Day.IsWeekend == false)
                            e.Cell.Text += "<tr><td class=\"calendar\"><span title=\"" + ds.Tables[0].Rows[0]["username"].ToString() + "&#13;" + strReason + "&#13;" + e.Day.Date.ToString("MMMM dd, yyyy") + "\">" + strType + ds.Tables[0].Rows[intCount]["username"].ToString() + "</span></td></tr>";
                        else
                            e.Cell.Text += "<tr><td class=\"calendarother\"><span title=\"" + ds.Tables[0].Rows[0]["username"].ToString() + "&#13;" + strReason + "&#13;" + e.Day.Date.ToString("MMMM dd, yyyy") + "\">" + strType + ds.Tables[0].Rows[intCount]["username"].ToString() + "</span></td></tr>";
                        intCount++;
                    }
                }
            }
            e.Cell.Text += "</table>";
            //if (boolAll == false)
            e.Cell.Text += "</div>";
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
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            bool boolApprove = (oApplication.Get(intApplication, "approve_vacation") == "1");
            string strEmployees = oApplication.Get(intApplication, "employees_needed");
            int intEmployees = 0;
            if (strEmployees != "")
                intEmployees = Int32.Parse(strEmployees);
            DataSet dsEmployees = oUser.GetApplication(intApplication);
            DateTime _date = DateTime.Today;
            string strError = "";
            DateTime _start = DateTime.Parse(txtStart.Text);
            TimeSpan oSpan = new TimeSpan();
            oSpan = _start.Subtract(_date);
            int intManager = oUser.GetManager(intProfile, true);
            if (oSpan.Days < intLeadDays)
                boolApprove = true;
            if (radDays.Checked == true)
            {
                DateTime _end = DateTime.Parse(txtEnd.Text);
                while (_start <= _end)
                {
                    DataSet dsOff = oVacation.Get(_start, intApplication);
                    int intDiff = dsEmployees.Tables[0].Rows.Count - dsOff.Tables[0].Rows.Count;
                    if (intDiff > intEmployees)
                    {
                        if (_start.DayOfWeek != System.DayOfWeek.Saturday && _start.DayOfWeek != System.DayOfWeek.Sunday)
                        {
                            int intVacation = oVacation.Add(intProfile, intApplication, _start, (radMorning.Checked ? 1 : 0), (radAfternoon.Checked ? 1 : 0), (radVacation.Checked ? 1 : 0), (radHoliday.Checked ? 1 : 0), (radPersonal.Checked ? 1 : 0), ddlReason.SelectedItem.Value, (boolApprove ? 0 : 1));
                            if (boolApprove == true)
                            {
                                string strDefault = oUser.GetApplicationUrl(intManager, intVacationPage);
                                if (strDefault == "")
                                    oFunction.SendEmail("ClearView Out of Office Request", oUser.GetName(intManager), "", strEMailIdsBCC, "ClearView Out of Office Request", "<p><b>The following out of office request requires your approval...</b><p><p>" + oVacation.GetBody(intVacation, intEnvironment) + "</p>", true, false);
                                else
                                    oFunction.SendEmail("ClearView Out of Office Request", oUser.GetName(intManager), "", strEMailIdsBCC, "ClearView Out of Office Request", "<p><b>The following out of office request requires your approval...</b><p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intVacationPage) + "?id=" + intVacation.ToString() + "\" target=\"_blank\">Click here to view this out of office request.</a></p><p>" + oVacation.GetBody(intVacation, intEnvironment) + "</p>", true, false);
                            }
                        }
                    }
                    else
                        strError += "<tr><td>" + _start.ToShortDateString() + "</td></tr>";
                    _start = _start.AddDays(1);
                }
            }
            else
            {
                DataSet dsOff = oVacation.Get(_start, intApplication);
                int intDiff = dsEmployees.Tables[0].Rows.Count - dsOff.Tables[0].Rows.Count;
                if (intDiff > intEmployees)
                {
                    if (_start.DayOfWeek != System.DayOfWeek.Saturday && _start.DayOfWeek != System.DayOfWeek.Sunday)
                    {
                        int intVacation = oVacation.Add(intProfile, intApplication, _start, (radMorning.Checked ? 1 : 0), (radAfternoon.Checked ? 1 : 0), (radVacation.Checked ? 1 : 0), (radHoliday.Checked ? 1 : 0), (radPersonal.Checked ? 1 : 0), ddlReason.SelectedItem.Value, (boolApprove ? 0 : 1));
                        if (boolApprove == true)
                        {
                            string strDefault = oUser.GetApplicationUrl(intManager, intVacationPage);
                            if (strDefault == "")
                                oFunction.SendEmail("ClearView Out of Office Request", oUser.GetName(intManager), "", strEMailIdsBCC, "ClearView Out of Office Request", "<p><b>The following out of office request requires your approval...</b><p><p>" + oVacation.GetBody(intVacation, intEnvironment) + "</p>", true, false);
                            else
                                oFunction.SendEmail("ClearView Out of Office Request", oUser.GetName(intManager), "", strEMailIdsBCC, "ClearView Out of Office Request", "<p><b>The following out of office request requires your approval...</b><p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intVacationPage) + "?id=" + intVacation.ToString() + "\" target=\"_blank\">Click here to view this out of office request.</a></p><p>" + oVacation.GetBody(intVacation, intEnvironment) + "</p>", true, false);
                        }
                    }
                }
                else
                    strError += "<tr><td>" + _start.ToShortDateString() + "</td></tr>";
            }
            try { _date = DateTime.Parse(ddlMonth.SelectedItem.Value + "/1/" + ddlYear.SelectedItem.Value); }
            catch { }
            if (strError == "")
                Response.Redirect(oPage.GetFullLink(intPage) + "?d=" + Server.UrlPathEncode(_date.ToShortDateString()));
            else
                lblError.Text += "<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\">" + strError + "</table>";
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            DateTime _date = DateTime.Today;
            try { _date = DateTime.Parse(ddlMonth.SelectedItem.Value + "/1/" + ddlYear.SelectedItem.Value); }
            catch { }
            LinkButton oButton = (LinkButton)Sender;
            oVacation.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(oPage.GetFullLink(intPage) + "?d=" + Server.UrlPathEncode(_date.ToShortDateString()));
        }
        protected void btnShowAll_Click(Object Sender, EventArgs e)
        {
            if (Request.QueryString["all"] != null)
                Response.Redirect(oPage.GetFullLink(intPage) + "?d=" + Request.QueryString["d"]);
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?d=" + Request.QueryString["d"] + "&all=true");
        }
    }
}