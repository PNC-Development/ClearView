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
    public partial class report_scorecard : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            Reports oReport = new Reports(0, dsn);
            if (!IsPostBack)
            {
                if (Request.QueryString["n"] != null && Request.QueryString["n"] != "")
                {
                    try
                    {
                        Variables oVariable = new Variables(intEnvironment);
                        Functions oFunction = new Functions(0, dsn, intEnvironment);
                        string strURL = "";
                        DataSet dsKey = oFunction.GetSetupValuesByKey(Environment.MachineName + "_REPORTING");
                        if (dsKey.Tables[0].Rows.Count > 0)
                            strURL = dsKey.Tables[0].Rows[0]["Value"].ToString();
                        if (strURL != "")
                        {
                            Uri u = new Uri(strURL);
                            ReportViewer1.ServerReport.ReportServerUrl = u;
                            ReportViewer1.ServerReport.ReportPath = "/Project Reports/Infrastructure Project Status Scorecard";
                            ReportViewer1.ShowParameterPrompts = false;
                            ReportCredentials oReportCredential = new ReportCredentials(0, dsn, intEnvironment);
                            ReportViewer1.ServerReport.ReportServerCredentials = oReportCredential;
                            ReportParameter[] parameters = new ReportParameter[1];
                            parameters[0] = new ReportParameter("Project", oFunction.decryptQueryString(Request.QueryString["n"]));
                            ReportViewer1.ServerReport.SetParameters(parameters);
                            ReportViewer1.ZoomPercent = 100;
                            ReportViewer1.ServerReport.Refresh();
                            panShow.Visible = true;
                        }
                        else
                            Response.Write("Invalid Reporting URL ~ " + Environment.MachineName + "_REPORTING");
                    }
                    catch (Exception ex)
                    {
                        panError.Visible = true;
                        Response.Write(ex.Message);
                    }
                }
            }
        }
    }
}
