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
    public partial class vacation_list : System.Web.UI.UserControl
    {

        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Pages oPage;
        protected AppPages oAppPage;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected Vacation oVacation;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oAppPage = new AppPages(intProfile, dsn);
            oVacation = new Vacation(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            DateTime _date = DateTime.Today;
            lblTitle.Text = "Out of Office Calendar for " + _date.ToLongDateString();
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
            lblNone.Visible = (ds.Tables[0].Rows.Count == 0);
        }
    }
}