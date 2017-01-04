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
    public partial class scheduling : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected DataSet ds;
        protected int intProfile;
        protected Users oUser;
        protected Scheduling oScheduling;
        protected static string lname;
        protected static string fname;
        protected static string phone;
        protected int schd_id;
        protected int is_regd;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            int intApplication = 290;
            oUser = new Users(intProfile, dsn);
            oScheduling = new Scheduling(intProfile, dsn);
            if (Request.QueryString["schd_id"] != null && Request.QueryString["schd_id"] != "")
            {
                schd_id = Int32.Parse(Request.QueryString["schd_id"]);
                ds = oScheduling.GetSch(DateTime.Parse(Request.QueryString["date"]), DateTime.MinValue, schd_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblFacilitator.Text = ds.Tables[0].Rows[0]["facilitator"].ToString();
                    lblEventName.Text = ds.Tables[0].Rows[0]["event"].ToString();
                    lblLocation.Text = ds.Tables[0].Rows[0]["location"].ToString();
                    lblNetMeeting.Text = ds.Tables[0].Rows[0]["netmeeting"].ToString() == string.Empty ? "Not Available" : ds.Tables[0].Rows[0]["netmeeting"].ToString();
                    lblConfLine.Text = ds.Tables[0].Rows[0]["confline"].ToString() == string.Empty ? "Not Available" : ds.Tables[0].Rows[0]["confline"].ToString();
                    lblPassCode.Text = ds.Tables[0].Rows[0]["passcode"].ToString() == string.Empty ? "Not Available" : ds.Tables[0].Rows[0]["passcode"].ToString();
                    lblStartTime.Text = ds.Tables[0].Rows[0]["start_time"].ToString();
                    lblEndTime.Text = ds.Tables[0].Rows[0]["end_time"].ToString();
                    if (Request.QueryString["view"] != null && Request.QueryString["view"] != "")
                    {
                        panView.Visible = true;
                        rptUsers.DataSource = oScheduling.GetSchUsers(schd_id);
                        rptUsers.DataBind();
                        lblUsersNone.Visible = (rptUsers.Items.Count == 0);
                    }
                    else
                    {
                        if (Request.QueryString["max"] != null)
                            lblResult.Text = "<img src='/images/bigError.gif' border='0' align='absmiddle' /> Exceeded the maximum number of people";
                        if (Request.QueryString["register"] != null)
                            lblResult.Text = "<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Registration Successful";
                        if (Request.QueryString["unregister"] != null)
                            lblResult.Text = "<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Unregistration Successful";
                        panEdit.Visible = true;
                        is_regd = oScheduling.VerifyUser(intProfile, schd_id);
                        if (is_regd == 1)
                            panUnregister.Visible = true;
                        else
                            panRegister.Visible = true;
                        if (!IsPostBack)
                        {
                            ds = oUser.Get(intProfile);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                lblName.Text = ds.Tables[0].Rows[0]["lname"].ToString() + ", " + ds.Tables[0].Rows[0]["fname"].ToString();
                                if (ds.Tables[0].Rows[0]["phone"].ToString() != String.Empty)
                                    txtPhone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                            }
                            drpDept.DataSource = oScheduling.GetDeptName(intProfile);
                            drpDept.DataTextField = "name";
                            drpDept.DataValueField = "name";
                            drpDept.DataBind();
                        }
                    }
                }
                if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                    intApplication = Int32.Parse(Request.Cookies["application"].Value);
                lblDate.Text = DateTime.Parse(Request.QueryString["date"]).ToLongDateString();
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            ds = oUser.Get(intProfile);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lname = ds.Tables[0].Rows[0]["lname"].ToString();
                fname = ds.Tables[0].Rows[0]["fname"].ToString();
                phone = ds.Tables[0].Rows[0]["phone"].ToString();
            }
            int result = oScheduling.RegisterUser(intProfile, ds.Tables[0].Rows[0]["lname"].ToString(), ds.Tables[0].Rows[0]["fname"].ToString(), txtPhone.Text, drpDept.SelectedItem.Text, schd_id);
            if (result == 0)
                Response.Redirect(Request.Path + "?schd_id=" + Request.QueryString["schd_id"] + "&date=" + Request.QueryString["date"] + "&register=true");
            else
                Response.Redirect(Request.Path + "?schd_id=" + Request.QueryString["schd_id"] + "&date=" + Request.QueryString["date"] + "&max=true");
        }
        protected void btnUnRegister_Click(object sender, EventArgs e)
        {
            int result = oScheduling.UnregisterUser(intProfile, schd_id);
            Response.Redirect(Request.Path + "?schd_id=" + Request.QueryString["schd_id"] + "&date=" + Request.QueryString["date"] + "&unregister=true");
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?schd_id=" + Request.QueryString["schd_id"] + "&date=" + Request.QueryString["date"] + "&view=true");
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?schd_id=" + Request.QueryString["schd_id"] + "&date=" + Request.QueryString["date"]);
        }
    }
}
