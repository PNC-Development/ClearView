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
    public partial class forecast_update : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Pages oPage;
        protected Forecast oForecast;
        protected int intProfile;
        protected int intId;
        protected int intCount;
        protected string strHTML;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);

            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intId = Int32.Parse(Request.QueryString["id"]);

            intCount = oForecast.GetAnswerUpdate(intId).Tables[0].Rows.Count;

            if ((Request.QueryString["update"] != null && Request.QueryString["update"] != "") || intCount > 0)
                panUpdate.Visible = false;


            lblSaved.Text = "<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Design Status was updated successfully";

            lblSaved.Visible = panUpdate.Visible == false;
            strHTML = oForecast.GetAnswerBody(intId, intEnvironment, dsnAsset, dsnIP);
            imgCommitmentDate.Attributes.Add("onclick", "return ShowCalendar('" + txtCommitmentDate.ClientID + "');");
            rblComplete.Attributes.Add("onclick", "RadioButtonListClick('" + rblComplete.ClientID + "')");
            rblValid.Attributes.Add("onclick", "RadioButtonListClick('" + rblValid.ClientID + "')");
            btnSave.Attributes.Add("onclick", "return ValidateStatusUpdate('" + rblComplete.ClientID + "','" + rblValid.ClientID + "') ");
        }

        protected void btnSave_Click(Object sender, EventArgs e)
        {
            int intValid = -1;
            int intComplete = Int32.Parse(rblComplete.SelectedValue);
            if (intComplete == 1)
                oForecast.UpdateAnswerSetComplete(intId);
            else
            {
                intValid = Int32.Parse(rblValid.SelectedValue);
                if (intValid == 1)
                {
                    if (txtCommitmentDate.Text != "")
                        oForecast.UpdateAnswerImplementation(intId, DateTime.Parse(txtCommitmentDate.Text));
                }
                else
                    oForecast.DeleteAnswer(intId);
            }
            oForecast.AddAnswerUpdate(intId, intComplete, intValid, txtComments.Text, intProfile);
            Response.Redirect(Request.Path + "?id=" + intId + "&update=success");
        }
    }
}
