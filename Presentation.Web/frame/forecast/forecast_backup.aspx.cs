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
    public partial class forecast_backup : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Forecast oForecast;
        protected int intID = 0;
        // All default values are assigned here
        protected double TotalFileSystemData = 0.00;

        protected double PercentageChangedDailyCFSD = 0.05;
        protected double CompressionRatioCFSD = 0.6;
        protected double CompressionRatioCDD = 0.00;  // client database data compression ration
        protected double GrowthFactor = 0.3;
        protected int TotalDbData = 0;
        protected double PercentageChangedDailyCDD = 1.00;
        protected double MaxFileSizeDiskPool = 1.5;
        // Variables for storing formula computations     
        protected double FullBackup;
        protected double IncrBU;
        protected double Dbdata;
        protected double ServerIBU;
        protected double ServerDD;
        protected void Page_Load(object sender, EventArgs e)
        {
            oForecast = new Forecast(0, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intID = Int32.Parse(Request.QueryString["id"]);
                if (oForecast.IsStorage(intID) == true)
                {
                    DataSet dsStorage = oForecast.GetStorage(intID);
                    double intHigh = double.Parse(dsStorage.Tables[0].Rows[0]["high_total"].ToString());
                    double intStandard = double.Parse(dsStorage.Tables[0].Rows[0]["standard_total"].ToString());
                    double intLow = double.Parse(dsStorage.Tables[0].Rows[0]["low_total"].ToString());
                    double intHighTest = double.Parse(dsStorage.Tables[0].Rows[0]["high_test"].ToString());
                    double intStandardTest = double.Parse(dsStorage.Tables[0].Rows[0]["standard_test"].ToString());
                    double intLowTest = double.Parse(dsStorage.Tables[0].Rows[0]["low_test"].ToString());
                    double dblTotal = intHigh + intStandard + intLow + intHighTest + intStandardTest + intLowTest;
                    TotalFileSystemData = dblTotal;
                }
                // Formula Computations
                FullBackup = Math.Round(TotalFileSystemData * (1 - CompressionRatioCFSD) * (1 + GrowthFactor), 1);
                IncrBU = Math.Round(FullBackup * PercentageChangedDailyCFSD, 1);
                Dbdata = Math.Round(TotalDbData * (1 - CompressionRatioCDD) * PercentageChangedDailyCDD * (1 + GrowthFactor), 1);
                ServerIBU = Math.Round(TotalDbData * PercentageChangedDailyCDD * (1 - CompressionRatioCDD) * (1 + GrowthFactor) < MaxFileSizeDiskPool ? TotalDbData * PercentageChangedDailyCDD * (1 - CompressionRatioCDD) * (1 + GrowthFactor) : 0, 1);
                ServerDD = Math.Round(TotalDbData * PercentageChangedDailyCDD * (1 - CompressionRatioCDD) * (1 + GrowthFactor) > MaxFileSizeDiskPool ? TotalDbData * PercentageChangedDailyCDD * (1 - CompressionRatioCDD) * (1 + GrowthFactor) : 0, 1);

                // Assign formula results to labels.
                lblFB.Text = FullBackup.ToString();
                lblIBU.Text = IncrBU.ToString();
                lbldbdata.Text = Dbdata.ToString();
                lblServerFB.Text = IncrBU.ToString();
                lblServerIBU.Text = ServerIBU.ToString();
                lblServerDD.Text = ServerDD.ToString();
                lblDiskFB.Text = Convert.ToString(IncrBU + ServerIBU);
            }
        }
    }
}
