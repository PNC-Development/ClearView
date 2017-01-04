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
    public partial class vacation : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected DataSet ds;
        protected int intProfile;
        protected Vacation oVacation;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            int intApplication = 0;
            oVacation = new Vacation(intProfile, dsn);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["d"] != null && Request.QueryString["d"] != "" && intApplication > 0)
            {
                DateTime _date = DateTime.Parse(Request.QueryString["d"]);
                ds = oVacation.Get(_date, intApplication);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["vacation"].ToString() == "1")
                        dr["reason"] = "Vacation";
                    else if (dr["holiday"].ToString() == "1")
                        dr["reason"] = "Floating Holiday";
                    else if (dr["personal"].ToString() == "1")
                        dr["reason"] = "Personal / Sick Day";
                    if (dr["morning"].ToString() == "1")
                        dr["duration"] = "Morning";
                    else if (dr["afternoon"].ToString() == "1")
                        dr["duration"] = "Afternoon";
                    else
                        dr["duration"] = "Full Day";
                }
                rptView.DataSource = ds;
                rptView.DataBind();
                lblDate.Text = _date.ToLongDateString();
            }
        }
    }
}
