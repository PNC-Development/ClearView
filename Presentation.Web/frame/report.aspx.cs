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
using Microsoft.Reporting.WebForms;

namespace NCC.ClearView.Presentation.Web
{
    public partial class report : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            Reports oReport = new Reports(0, dsn);
            if (!IsPostBack)
            {
                int intApplication = -1;
                if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                    intApplication = Int32.Parse(Request.QueryString["applicationid"]);
                if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                    intApplication = Int32.Parse(Request.Cookies["application"].Value);
                if (intApplication > -1 && Request.QueryString["r"] != null && Request.QueryString["r"] != "")
                {
                    try
                    {
                        Variables oVariable = new Variables(intEnvironment);
                        Functions oFunction = new Functions(0, dsn, intEnvironment);
                        int intReport = Int32.Parse(oFunction.decryptQueryString(Request.QueryString["r"]));
                        string strReport = oReport.Get(intReport, "path");
                        if (strReport != "")
                        {
                            bool boolOLD = (oReport.Get(intReport, "old") == "1");
                            string strURL = "";
                            if (boolOLD == false)
                            {
                                DataSet dsKey = oFunction.GetSetupValuesByKey(Environment.MachineName + "_REPORTING");
                                if (dsKey.Tables[0].Rows.Count > 0)
                                    strURL = dsKey.Tables[0].Rows[0]["Value"].ToString();
                            }
                            //else
                            //    strURL = oVariable.DefaultReportURL_OLD();
                            if (strURL != "")
                            {
                                Uri u = new Uri(strURL);
                                ReportViewer1.ServerReport.ReportServerUrl = u;
                                ReportViewer1.ServerReport.ReportPath = strReport;
                                ReportCredentials oReportCredential = new ReportCredentials(0, dsn, intEnvironment);
                                ReportViewer1.ServerReport.ReportServerCredentials = oReportCredential;
                                ReportViewer1.ZoomPercent = Int32.Parse(oReport.Get(intReport, "percentage"));
                                if (oReport.Get(intReport, "application") == "1")
                                {
                                    ReportParameter[] parameters = new ReportParameter[1];
                                    parameters[0] = new ReportParameter("applicationid", intApplication.ToString());
                                    ReportViewer1.ServerReport.SetParameters(parameters);
                                    // ???
                                    ReportViewer1.ServerReport.Refresh();
                                }
                            }
                            else
                                Response.Write("Invalid Reporting URL ~ " + Environment.MachineName + "_REPORTING");
                        }
                        else
                            Response.Write("Invalid Report Path");
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                    }
                }
            }
        }
    }
}
