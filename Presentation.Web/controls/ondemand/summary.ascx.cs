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
    public partial class summary : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected OnDemand oOnDemand;
        protected Forecast oForecast;
        protected Classes oClass;
        protected int intID = 0;
        protected int intStep = 0;
        protected int intType = 0;
        protected int intRequest = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oOnDemand = new OnDemand(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
                intStep = Int32.Parse(Request.QueryString["sid"]);
            if (oForecast.GetAnswer(intID, "completed") == "" && Request.QueryString["view"] == null)
            {
                if (Request.QueryString["step"] != null && Request.QueryString["step"] != "")
                    panUpdate.Visible = true;
                else
                    panNavigation.Visible = true;
            }
            else
                btnClose.Text = "Close";
            if (intID > 0)
            {
                DataSet ds = oForecast.GetAnswer(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                    if (oClass.IsProd(intClass) && oClass.Get(intClass, "pnc") == "0")
                    {
                        if (ds.Tables[0].Rows[0]["production"].ToString() != "")
                            txtDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["production"].ToString()).ToShortDateString();
                    }
                    else if (Request.QueryString["view"] == null)
                    {
                        oOnDemand.Next(intID, Int32.Parse(Request.QueryString["sid"]));
                        Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
                    }
                    else
                        panTest.Visible = true;
                }
            }
            imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
            btnNext.Attributes.Add("onclick", "return ValidateDate('" + txtDate.ClientID + "','Please enter a valid date')" +
                ";");
            btnClose.Attributes.Add("onclick", "return window.close();");
        }
        private void Save()
        {
            if (txtDate.Text != "")
                oForecast.UpdateAnswerProduction(intID, DateTime.Parse(txtDate.Text));
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            oOnDemand.Back(intID);
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&backward=true");
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            Save();
            oOnDemand.Next(intID, Int32.Parse(Request.QueryString["sid"]));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&forward=true");
        }
        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            Save();
            oOnDemand.Next(intID, Int32.Parse(Request.QueryString["sid"]));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&forward=true");
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
    }
}