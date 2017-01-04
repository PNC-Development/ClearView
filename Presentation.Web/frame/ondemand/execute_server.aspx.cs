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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class execute_server : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected bool boolUseCSM = (ConfigurationManager.AppSettings["USE_CSM_EXECUTION"] == "1");
        protected int intForecastPage = Int32.Parse(ConfigurationManager.AppSettings["ForecastEdit"]);
        protected int intConfidence100 = Int32.Parse(ConfigurationManager.AppSettings["CONFIDENCE_100"]);
        protected int intProfile;
        protected Forecast oForecast;
        protected OnDemand oOnDemand;
        protected Pages oPage;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected ServiceRequests oServiceRequest;
        protected Storage oStorage;
        protected Users oUser;
        protected Functions oFunction;

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
            oUser = new Users(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);

            if (Request.QueryString["parent"] != null && Request.QueryString["parent"] != "")
                intForecast = Int32.Parse(Request.QueryString["parent"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);

            if (oUser.GetApplicationUrl(intProfile, intForecastPage) == "")
            {
                panDenied.Visible = true;
                hypDesign.NavigateUrl = "/datapoint/service/design.aspx?id=" + oFunction.encryptQueryString(intID.ToString());
            }
            else
            {
                panAllow.Visible = true;
                if (Request.QueryString["saved"] != null)
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Forecast Equipment Saved');if(window.opener!=null)window.opener.navigate('" + oPage.GetFullLink(intForecastPage) + "?id=" + intForecast.ToString() + "');window.close();<" + "/" + "script>");
                int intType = 0;
                int intStep = 0;
                int intModel = 0;
                bool boolOverrideReject = false;
                bool boolOverridePending = false;
                bool boolConfidence = false;
                if (intID > 0)
                {
                    int _step = 0;
                    DataSet ds = oForecast.GetAnswer(intID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int intQuantity = Int32.Parse(ds.Tables[0].Rows[0]["quantity"].ToString());
                        if (intConfidence100 == Int32.Parse(ds.Tables[0].Rows[0]["confidenceid"].ToString()))
                            boolConfidence = true;
                        if (Int32.Parse(ds.Tables[0].Rows[0]["override"].ToString()) == -10)
                            boolOverrideReject = true;
                        if (Int32.Parse(ds.Tables[0].Rows[0]["override"].ToString()) == -1)
                            boolOverridePending = true;
                        lblQuantity.Text = intQuantity.ToString();
                        intQuantity = intQuantity * 25;
                        lblMinimum.Text = intQuantity.ToString();
                        lblForecastCount.Text = ds.Tables[0].Rows[0]["quantity"].ToString();
                        lblCurrentCount.Text = oForecast.TotalServerCount(intID, boolUseCSM).ToString();
                        if (lblCurrentCount.Text != lblForecastCount.Text)
                            lblCurrentCount.CssClass = "biggerreddefault";
                        lblCurrentDR.Text = oForecast.TotalDRCount(intID, boolUseCSM).ToString();
                        lblForecastDR.Text = ds.Tables[0].Rows[0]["recovery_number"].ToString();
                        if (lblCurrentDR.Text != lblForecastDR.Text)
                            lblCurrentDR.CssClass = "biggerreddefault";
                        lblCurrentHA.Text = oForecast.TotalHACount(intID, boolUseCSM).ToString();
                        lblForecastHA.Text = ds.Tables[0].Rows[0]["ha"].ToString();
                        if (lblCurrentHA.Text != lblForecastHA.Text)
                            lblCurrentHA.CssClass = "biggerreddefault";

                        if (oForecast.IsStorage(intID) == true)
                        {
                            DataSet dsStorage = oForecast.GetStorage(intID);
                            double dblHigh = double.Parse(dsStorage.Tables[0].Rows[0]["high_total"].ToString());
                            double dblStandard = double.Parse(dsStorage.Tables[0].Rows[0]["standard_total"].ToString());
                            double dblLow = double.Parse(dsStorage.Tables[0].Rows[0]["low_total"].ToString());
                            double dblHighQA = double.Parse(dsStorage.Tables[0].Rows[0]["high_qa"].ToString());
                            double dblStandardQA = double.Parse(dsStorage.Tables[0].Rows[0]["standard_qa"].ToString());
                            double dblLowQA = double.Parse(dsStorage.Tables[0].Rows[0]["low_qa"].ToString());
                            double dblHighTest = double.Parse(dsStorage.Tables[0].Rows[0]["high_test"].ToString());
                            double dblStandardTest = double.Parse(dsStorage.Tables[0].Rows[0]["standard_test"].ToString());
                            double dblLowTest = double.Parse(dsStorage.Tables[0].Rows[0]["low_test"].ToString());
                            double dblHighHA = double.Parse(dsStorage.Tables[0].Rows[0]["high_ha"].ToString());
                            double dblStandardHA = double.Parse(dsStorage.Tables[0].Rows[0]["standard_ha"].ToString());
                            double dblLowHA = double.Parse(dsStorage.Tables[0].Rows[0]["low_ha"].ToString());
                            //double dblHighRep = double.Parse(dsStorage.Tables[0].Rows[0]["high_replicated"].ToString());
                            //double dblStandardRep = double.Parse(dsStorage.Tables[0].Rows[0]["standard_replicated"].ToString());
                            //double dblLowRep = double.Parse(dsStorage.Tables[0].Rows[0]["low_replicated"].ToString());
                            //double dblTotal = dblHigh + dblStandard + dblLow + dblHighQA + dblStandardQA + dblLowQA + dblHighTest + dblStandardTest + dblLowTest + dblHighRep + dblStandardRep + dblLowRep + dblHighHA + dblStandardHA + dblLowHA;
                            double dblTotal = dblHigh + dblStandard + dblLow + dblHighQA + dblStandardQA + dblLowQA + dblHighTest + dblStandardTest + dblLowTest + dblHighHA + dblStandardHA + dblLowHA;
                            lblForecastStorage.Text = dblTotal.ToString() + " GB";
                            lblBladeStorage.Text = dblTotal.ToString();
                            lblForecastStorageHighP.Text = dblHigh.ToString() + " GB";
                            lblForecastStorageHighQ.Text = dblHighQA.ToString() + " GB";
                            lblForecastStorageHighT.Text = dblHighTest.ToString() + " GB";
                            lblForecastStorageHighH.Text = dblHighHA.ToString() + " GB";
                            lblForecastStorageStandardP.Text = dblStandard.ToString() + " GB";
                            lblForecastStorageStandardQ.Text = dblStandardQA.ToString() + " GB";
                            lblForecastStorageStandardT.Text = dblStandardTest.ToString() + " GB";
                            lblForecastStorageStandardH.Text = dblStandardHA.ToString() + " GB";
                            lblForecastStorageLowP.Text = dblLow.ToString() + " GB";
                            lblForecastStorageLowQ.Text = dblLowQA.ToString() + " GB";
                            lblForecastStorageLowT.Text = dblLowTest.ToString() + " GB";
                            lblForecastStorageLowH.Text = dblLowHA.ToString() + " GB";
                            dsStorage = oStorage.GetLuns(intID);
                            dblHigh = 0.00;
                            dblStandard = 0.00;
                            dblLow = 0.00;
                            dblHighQA = 0.00;
                            dblStandardQA = 0.00;
                            dblLowQA = 0.00;
                            dblHighTest = 0.00;
                            dblStandardTest = 0.00;
                            dblLowTest = 0.00;
                            dblTotal = 0.00;
                            dblHighHA = 0.00;
                            dblStandardHA = 0.00;
                            dblLowHA = 0.00;
                            double dblTotalQA = 0.00;
                            double dblTotalTest = 0.00;

                            foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
                            {
                                double dblQuantity = 1.00;
                                if (Int32.Parse(drStorage["clusterid"].ToString()) > 0 && Int32.Parse(drStorage["instanceid"].ToString()) == 0)
                                {
                                    int intCluster = Int32.Parse(drStorage["clusterid"].ToString());
                                    Cluster oCluster = new Cluster(intProfile, dsn);
                                    dblQuantity = double.Parse(oCluster.Get(intCluster, "nodes"));
                                }
                                if (drStorage["size"].ToString() != "")
                                {
                                    if (drStorage["performance"].ToString() == "High")
                                    {
                                        dblHigh += (double.Parse(drStorage["size"].ToString()) * dblQuantity);
                                        if (drStorage["high_availability"].ToString() == "1")
                                            dblHighHA += (double.Parse(drStorage["size"].ToString()) * dblQuantity);
                                    }
                                    if (drStorage["performance"].ToString() == "Standard")
                                    {
                                        dblStandard += (double.Parse(drStorage["size"].ToString()) * dblQuantity);
                                        if (drStorage["high_availability"].ToString() == "1")
                                            dblStandardHA += (double.Parse(drStorage["size"].ToString()) * dblQuantity);
                                    }
                                    if (drStorage["performance"].ToString() == "Low")
                                    {
                                        dblLow += (double.Parse(drStorage["size"].ToString()) * dblQuantity);
                                        if (drStorage["high_availability"].ToString() == "1")
                                            dblLowHA += (double.Parse(drStorage["size"].ToString()) * dblQuantity);
                                    }
                                    dblTotal += (double.Parse(drStorage["size"].ToString()) * dblQuantity);
                                    if (drStorage["high_availability"].ToString() == "1")
                                        dblTotal += (double.Parse(drStorage["size"].ToString()) * dblQuantity);
                                }
                                if (drStorage["size_qa"].ToString() != "")
                                {
                                    if (drStorage["performance"].ToString() == "High")
                                        dblHighQA += (double.Parse(drStorage["size_qa"].ToString()) * dblQuantity);
                                    if (drStorage["performance"].ToString() == "Standard")
                                        dblStandardQA += (double.Parse(drStorage["size_qa"].ToString()) * dblQuantity);
                                    if (drStorage["performance"].ToString() == "Low")
                                        dblLowQA += (double.Parse(drStorage["size_qa"].ToString()) * dblQuantity);
                                    dblTotalQA += (double.Parse(drStorage["size_qa"].ToString()) * dblQuantity);
                                }
                                if (drStorage["size_test"].ToString() != "")
                                {
                                    if (drStorage["performance"].ToString() == "High")
                                        dblHighTest += (double.Parse(drStorage["size_test"].ToString()) * dblQuantity);
                                    if (drStorage["performance"].ToString() == "Standard")
                                        dblStandardTest += (double.Parse(drStorage["size_test"].ToString()) * dblQuantity);
                                    if (drStorage["performance"].ToString() == "Low")
                                        dblLowTest += (double.Parse(drStorage["size_test"].ToString()) * dblQuantity);
                                    dblTotalTest += (double.Parse(drStorage["size_test"].ToString()) * dblQuantity);
                                }
                                DataSet dsMount = oStorage.GetMountPoints(Int32.Parse(drStorage["id"].ToString()));
                                foreach (DataRow drMount in dsMount.Tables[0].Rows)
                                {
                                    if (drMount["size"].ToString() != "")
                                    {
                                        if (drMount["performance"].ToString() == "High")
                                        {
                                            dblHigh += (double.Parse(drMount["size"].ToString()) * dblQuantity);
                                            if (drMount["high_availability"].ToString() == "1")
                                                dblHighHA += (double.Parse(drStorage["size"].ToString()) * dblQuantity);
                                        }
                                        if (drMount["performance"].ToString() == "Standard")
                                        {
                                            dblStandard += (double.Parse(drMount["size"].ToString()) * dblQuantity);
                                            if (drMount["high_availability"].ToString() == "1")
                                                dblStandardHA += (double.Parse(drStorage["size"].ToString()) * dblQuantity);
                                        }
                                        if (drMount["performance"].ToString() == "Low")
                                        {
                                            dblLow += (double.Parse(drMount["size"].ToString()) * dblQuantity);
                                            if (drMount["high_availability"].ToString() == "1")
                                                dblLowHA += (double.Parse(drStorage["size"].ToString()) * dblQuantity);
                                        }
                                        dblTotal += (double.Parse(drMount["size"].ToString()) * dblQuantity);
                                        if (drMount["high_availability"].ToString() == "1")
                                            dblTotal += (double.Parse(drStorage["size"].ToString()) * dblQuantity);
                                    }
                                    if (drStorage["size_qa"].ToString() != "")
                                    {
                                        if (drMount["performance"].ToString() == "High")
                                            dblHighQA += (double.Parse(drMount["size_qa"].ToString()) * dblQuantity);
                                        if (drMount["performance"].ToString() == "Standard")
                                            dblStandardQA += (double.Parse(drMount["size_qa"].ToString()) * dblQuantity);
                                        if (drMount["performance"].ToString() == "Low")
                                            dblLowQA += (double.Parse(drMount["size_qa"].ToString()) * dblQuantity);
                                        dblTotalQA += (double.Parse(drMount["size_qa"].ToString()) * dblQuantity);
                                    }
                                    if (drMount["size_test"].ToString() != "")
                                    {
                                        if (drMount["performance"].ToString() == "High")
                                            dblHighTest += (double.Parse(drMount["size_test"].ToString()) * dblQuantity);
                                        if (drMount["performance"].ToString() == "Standard")
                                            dblStandardTest += (double.Parse(drMount["size_test"].ToString()) * dblQuantity);
                                        if (drMount["performance"].ToString() == "Low")
                                            dblLowTest += (double.Parse(drMount["size_test"].ToString()) * dblQuantity);
                                        dblTotalTest += (double.Parse(drMount["size_test"].ToString()) * dblQuantity);
                                    }
                                }
                            }
                            double dblOverall = dblTotal + dblTotalQA + dblTotalTest;
                            lblCurrentStorage.Text = dblOverall.ToString() + " GB";
                            lblCurrentStorageHighP.Text = dblHigh.ToString() + " GB";
                            lblCurrentStorageHighQ.Text = dblHighQA.ToString() + " GB";
                            lblCurrentStorageHighT.Text = dblHighTest.ToString() + " GB";
                            lblCurrentStorageHighH.Text = dblHighHA.ToString() + " GB";
                            lblCurrentStorageStandardP.Text = dblStandard.ToString() + " GB";
                            lblCurrentStorageStandardQ.Text = dblStandardQA.ToString() + " GB";
                            lblCurrentStorageStandardT.Text = dblStandardTest.ToString() + " GB";
                            lblCurrentStorageStandardH.Text = dblStandardHA.ToString() + " GB";
                            lblCurrentStorageLowP.Text = dblLow.ToString() + " GB";
                            lblCurrentStorageLowQ.Text = dblLowQA.ToString() + " GB";
                            lblCurrentStorageLowT.Text = dblLowTest.ToString() + " GB";
                            lblCurrentStorageLowH.Text = dblLowHA.ToString() + " GB";
                            if (lblCurrentStorage.Text != lblForecastStorage.Text)
                                lblCurrentStorage.CssClass = "biggerreddefault";
                        }
                        else
                        {
                            lblCurrentStorage.Text = "0 GB";
                            lblForecastStorage.Text = "0 GB";
                        }
                        intModel = oForecast.GetModelAsset(intID);
                        if (intModel == 0)
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
                            _step = Int32.Parse(Request.QueryString["step"]);
                            if (_step <= dsSteps.Tables[0].Rows.Count)
                                intStep = _step;
                        }
                        else if (Request.QueryString["view"] != null && Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
                        {
                            _step = Int32.Parse(Request.QueryString["sid"]);
                            if (_step <= dsSteps.Tables[0].Rows.Count)
                                intStep = Int32.Parse(dsSteps.Tables[0].Rows[_step - 1]["id"].ToString());
                        }
                        else
                        {
                            if (dsStepsDone.Tables[0].Rows.Count > 0)
                                intStep = Int32.Parse(dsStepsDone.Tables[0].Rows[0]["id"].ToString());
                        }
                        if (oForecast.IsStorage(intID) == true)
                            btnDetails.Attributes.Add("onclick", "ShowHideDiv2('" + divDetails.ClientID + "');return false;");
                        else
                            btnDetails.Enabled = false;
                    }
                    if (Request.QueryString["save"] != null)
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">if(window.opener!=null)window.opener.navigate('" + oPage.GetFullLink(intForecastPage) + "?id=" + intForecast.ToString() + "');<" + "/" + "script>");
                    DataSet dsStep = oOnDemand.GetWizardStep(intStep);
                    if (dsStep.Tables[0].Rows.Count > 0)
                    {
                        imgStep.ImageUrl = "/images/wizard.gif";
                        lblTitle.Text = dsStep.Tables[0].Rows[0]["name"].ToString();
                        lblSubTitle.Text = dsStep.Tables[0].Rows[0]["subtitle"].ToString();
                        if (boolConfidence == false)
                            panConfidence.Visible = true;
                        else if (boolOverridePending == true)
                            panOverridePending.Visible = true;
                        else if (boolOverrideReject == true)
                            panOverrideReject.Visible = true;
                        else
                        {
                            string strPath = dsStep.Tables[0].Rows[0]["path"].ToString();
                            bool boolSkip = false;
                            bool boolShowCluster = (dsStep.Tables[0].Rows[0]["show_cluster"].ToString() == "1");
                            bool boolShowCSM = (dsStep.Tables[0].Rows[0]["show_csm"].ToString() == "1");
                            bool boolSkipCluster = (dsStep.Tables[0].Rows[0]["skip_cluster"].ToString() == "1");
                            bool boolSkipCSM = (dsStep.Tables[0].Rows[0]["skip_csm"].ToString() == "1");
                            if (oForecast.IsHACluster(intID) == true)
                            {
                                if (boolShowCluster == false)
                                    boolSkip = true;
                            }
                            else if (boolSkipCluster == true)
                                boolSkip = true;
                            if (oForecast.IsHACSM(intID) == true)
                            {
                                if (boolShowCSM == false)
                                    boolSkip = true;
                            }
                            else if (boolSkipCSM == true)
                                boolSkip = true;
                            if (boolSkip == true)
                            {
                                if (Request.QueryString["backward"] != null)
                                {
                                    if (Request.QueryString["view"] != null)
                                    {
                                        _step--;
                                        Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&sid=" + _step.ToString() + "&view=true");
                                    }
                                    else
                                    {
                                        oOnDemand.Back(intID);
                                        Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&backward=true");
                                    }
                                }
                                else
                                {
                                    if (Request.QueryString["view"] != null)
                                    {
                                        _step++;
                                        Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&sid=" + _step.ToString() + "&view=true");
                                    }
                                    else
                                    {
                                        oOnDemand.Next(intID, intStep);
                                        Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&forward=true");
                                    }
                                }
                            }
                            if (strPath != "")
                            {
                                panStep.Visible = true;
                                if (Request.QueryString["sid"] == null || Request.QueryString["sid"] == "")
                                    Response.Redirect(Request.Path + "?id=" + intID + "&sid=" + intStep);
                                PHStep.Controls.Add((Control)LoadControl(strPath));
                            }
                        }
                    }
                    else
                    {
                        // START ON DEMAND
                        Response.Redirect("/frame/ondemand/status.aspx?id=" + Request.QueryString["id"]);
                    }
                }
            }
            btnClose.Attributes.Add("onclick", "return window.close();");
        }
    }
}
