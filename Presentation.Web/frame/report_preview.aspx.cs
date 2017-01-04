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
    public partial class report_preview : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            Reports oReport = new Reports(0, dsn);
            if (Request.QueryString["r"] != null && Request.QueryString["r"] != "")
            {
                try
                {
                    Variables oVariable = new Variables(intEnvironment);
                    Functions oFunction = new Functions(0, dsn, intEnvironment);
                    int intReport = Int32.Parse(oFunction.decryptQueryString(Request.QueryString["r"]));
                    string strReport = oReport.Get(intReport, "path");
                    string strPhysical = oReport.Get(intReport, "physical");
                    if (strReport != "")
                    {
                        ReportViewer1.Visible = true;
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
                            ReportViewer1.ZoomPercent = 10;
                            ReportViewer1.ShowToolBar = false;
                            ReportViewer1.WaitMessageFont.Size = FontUnit.XXSmall;
                            ReportViewer1.SizeToReportContent = true;
                        }
                        else
                        {
                            divNA.Visible = true;
                        }
                    }
                    else
                    {
                        divNA.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }
    }
}
