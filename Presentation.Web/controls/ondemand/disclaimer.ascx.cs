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
    public partial class disclaimer : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intPlatformServer = Int32.Parse(ConfigurationManager.AppSettings["ServerPlatformID"]);
        protected int intPlatformWorkstation = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intProfile;
        protected OnDemand oOnDemand;
        protected Forecast oForecast;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oOnDemand = new OnDemand(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            Page.Title = "ClearView Provisioning Disclaimer | Design # " + intID.ToString();
            if (intID > 0)
            {
                int intPlatform = Int32.Parse(oForecast.GetAnswer(intID, "platformid"));
                if (intPlatform == intPlatformWorkstation)
                    panWorkstation.Visible = true;
                else
                    panServer.Visible = true;
            }
            btnClose.Attributes.Add("onclick", "return window.close();");
        }
        protected void btnBegin_Click(Object Sender, EventArgs e)
        {
            int intRequest = oForecast.GetRequestID(intID, true);
            if (intRequest == 0)
            {
                Requests oRequest = new Requests(intProfile, dsn);
                ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
                int intForecast = Int32.Parse(oForecast.GetAnswer(intID, "forecastid"));
                if (intForecast > 0)
                {
                    intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                    int intProject = oRequest.GetProjectNumber(intRequest);
                    intRequest = oRequest.Add(intProject, intProfile);
                }
                else
                    intRequest = oRequest.Add(0, intProfile);
            }
            oForecast.UpdateAnswer(intID, intRequest);
            oOnDemand.Next(intID, Int32.Parse(Request.QueryString["sid"]));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&forward=true");
        }
    }
}