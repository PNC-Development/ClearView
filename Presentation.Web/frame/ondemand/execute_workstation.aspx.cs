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
    public partial class execute_workstation : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected int intForecastPage = Int32.Parse(ConfigurationManager.AppSettings["ForecastEdit"]);
        protected int intProfile;
        protected Forecast oForecast;
        protected OnDemand oOnDemand;
        protected Pages oPage;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected ServiceRequests oServiceRequest;
        protected Storage oStorage;
        protected Services oService;
        protected int intForecast;
        protected int intID = 0;
        protected string strSteps = "";
        protected string strProgress = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oOnDemand = new OnDemand(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oStorage = new Storage(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            if (Request.QueryString["parent"] != null && Request.QueryString["parent"] != "")
                intForecast = Int32.Parse(Request.QueryString["parent"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["saved"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Forecast Equipment Saved');if(window.opener!=null)window.opener.navigate('" + oPage.GetFullLink(intForecastPage) + "?id=" + intForecast.ToString() + "');window.close();<" + "/" + "script>");
            int intType = 0;
            int intStep = 0;
            int intModel = 0;
            bool boolApproved = true;
            imgStep.ImageUrl = "/images/wizard.gif";
            if (intID > 0)
            {
                DataSet ds = oForecast.GetAnswer(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                    DataSet dsApproval = oService.GetSelected(intRequest);
                    foreach (DataRow drApproval in dsApproval.Tables[0].Rows)
                    {
                        if (drApproval["approved"].ToString() == "0")
                        {
                            boolApproved = false;
                            break;
                        }
                    }
                    int intQuantity = Int32.Parse(ds.Tables[0].Rows[0]["quantity"].ToString());
                    lblForecastCount.Text = ds.Tables[0].Rows[0]["quantity"].ToString();
                    lblCurrentCount.Text = oForecast.TotalWorkstationCount(intID).ToString();
                    if (lblCurrentCount.Text != lblForecastCount.Text)
                        lblCurrentCount.CssClass = "biggerreddefault";
                    intModel = oForecast.GetModel(intID);
                    intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                    intType = oModel.GetType(intModel);
                    DataSet dsSteps = oOnDemand.GetWizardSteps(intType, 1);
                    DataSet dsStepsDone = oOnDemand.GetWizardStepsDone(intID, intType);
                    int intStepsTotal = dsSteps.Tables[0].Rows.Count;
                    int intStepsNotDone = (intStepsTotal - dsStepsDone.Tables[0].Rows.Count);
                    double dblProgress = double.Parse(intStepsNotDone.ToString()) / double.Parse(intStepsTotal.ToString());
                    dblProgress = dblProgress * 100.00;
                    strProgress = oServiceRequest.GetStatusBarBlue(dblProgress, "90", true);
                    if (Request.QueryString["step"] != null && Request.QueryString["step"] != "")
                    {
                        int _step = Int32.Parse(Request.QueryString["step"]);
                        if (_step <= dsSteps.Tables[0].Rows.Count)
                            intStep = _step;
                    }
                    else
                    {
                        if (dsStepsDone.Tables[0].Rows.Count > 0)
                            intStep = Int32.Parse(dsStepsDone.Tables[0].Rows[0]["id"].ToString());
                    }
                }
                if (Request.QueryString["save"] != null)
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">if(window.opener!=null)window.opener.navigate('" + oPage.GetFullLink(intForecastPage) + "?id=" + intForecast.ToString() + "');<" + "/" + "script>");
                if (boolApproved == false)
                    panPending.Visible = true;
                else
                {
                    DataSet dsStep = oOnDemand.GetWizardStep(intStep);
                    if (dsStep.Tables[0].Rows.Count > 0)
                    {
                        lblTitle.Text = dsStep.Tables[0].Rows[0]["name"].ToString();
                        lblSubTitle.Text = dsStep.Tables[0].Rows[0]["subtitle"].ToString();
                        string strPath = dsStep.Tables[0].Rows[0]["path"].ToString();
                        if (strPath != "")
                        {
                            panStep.Visible = true;
                            if (Request.QueryString["sid"] == null || Request.QueryString["sid"] == "")
                                Response.Redirect(Request.Path + "?id=" + intID + "&sid=" + intStep);
                            PHStep.Controls.Add((Control)LoadControl(strPath));
                        }
                    }
                    else
                    {
                        // START ON DEMAND
                        Response.Redirect("/frame/ondemand/status.aspx?id=" + Request.QueryString["id"]);
                    }
                }
            }
        }
    }
}
