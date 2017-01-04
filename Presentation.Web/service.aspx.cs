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
    public partial class service : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intResourceRequest = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequest"]);
        protected string strTitle = ConfigurationManager.AppSettings["appTitle"];
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Services oService = new Services(intProfile, dsn);
            Requests oRequest = new Requests(intProfile, dsn);

            if (Int32.TryParse(Request.Cookies["profileid"].Value, out intProfile) == true)
            {
                int intService = 0;
                if (Int32.TryParse(Request.Cookies["sid"].Value, out intService) == true)
                {
                    int intProject = 0;
                    Int32.TryParse(Request.Cookies["pid"].Value, out intProject);

                    int intRequest = 0;
                    Int32.TryParse(Request.Cookies["rid"].Value, out intRequest);

                    int intNewRequest = 0;
                    if (intRequest == 0)
                    {
                        if (oService.Get(intService, "project") == "1" && intProject == 0)
                            Response.Write("Service " + oService.GetName(intService).ToUpper() + " requires a project. Please specify a projectid (pid=) or a requestid (rid=)");
                        else
                            intNewRequest = oRequest.Add((intProject == 0 ? -1 : intProject), intProfile);
                    }
                    else
                    {
                        DataSet dsR = oRequest.Get(intRequest);
                        if (dsR.Tables[0].Rows.Count > 0)
                        {
                            intProject = Int32.Parse(dsR.Tables[0].Rows[0]["projectid"].ToString());
                            intNewRequest = oRequest.Add(intProject, intProfile);
                        }
                    }

                    if (intNewRequest > 0)
                    {
                        string strQ = Request.QueryString["q"];
                        ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
                        RequestItems oRequestItem = new RequestItems(intProfile, dsn);
                        Pages oPage = new Pages(intProfile, dsn);
                        oServiceRequest.Add(intNewRequest, 1, 0);
                        oService.AddSelected(intNewRequest, intService, 1);
                        int intItem = oService.GetItemId(intService);
                        oRequestItem.AddForm(intNewRequest, intItem, intService, 1);
                        Response.Redirect(oPage.GetFullLink(intResourceRequest) + "?rid=" + intNewRequest.ToString() + "&q=" + strQ);
                    }
                }
            }
            else
                Response.Redirect("/redirect.aspx?referrer=" + Request.Url.PathAndQuery);
        }
    }
}
