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
    public partial class forecast_backup_registration : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected Forecast oForecast;
        protected Servers oServer;
        protected string strHosts = "";
        protected string strRequirements = "";
        protected string strExclusions = "";
        protected bool boolUsePNCNaming = (ConfigurationManager.AppSettings["USE_PNC_NAMING"] == "1");
        protected void Page_Load(object sender, EventArgs e)
        {
            oForecast = new Forecast(0, dsn);
            oServer = new Servers(0, dsn);
            if (Request.QueryString["id"] != null)
            {
                int intID = Int32.Parse(Request.QueryString["id"]);
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
                    int TotalFileSystemData = Int32.Parse(dblTotal.ToString());
                    lblTCDC.Text = TotalFileSystemData.ToString() + " GB";
                }
                int intForecast = Int32.Parse(oForecast.GetAnswer(intID, "forecastid"));
                int intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                Requests oRequest = new Requests(0, dsn);
                int intProject = oRequest.GetProjectNumber(intRequest);
                Projects oProject = new Projects(0, dsn);
                //rptApp.DataSource = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select top 1 * from cv_ondemand_answering");
                //rptApp.DataBind();
                lblAppNone.Visible = (rptApp.Items.Count == 0);
                lblPrjName.Text = oProject.Get(intProject, "name");
                Users oUser = new Users(0, dsn);
                string strLead = oProject.Get(intProject, "lead");
                if (strLead != "")
                {
                    lblPrjMgr.Text = oUser.GetFullName(Int32.Parse(strLead));
                    lblPhoneNum1.Text = oUser.Get(Int32.Parse(strLead), "phone");
                }
                string strRequester = oForecast.Get(intForecast, "userid");
                if (strRequester != "")
                {
                    lblRequester.Text = oUser.GetFullName(Int32.Parse(strRequester));
                    lblPhoneNum2.Text = oUser.Get(Int32.Parse(strRequester), "phone");
                }
                lblDate.Text = DateTime.Parse(oForecast.GetAnswer(intID, "implementation")).ToLongDateString();
                // Get data from servers table
                ServerName oServerName = new ServerName(0, dsn);
                IPAddresses oIPAddress = new IPAddresses(0, dsnIP, dsn);
                OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);
                ServicePacks oServicePack = new ServicePacks(0, dsn);
                DataSet dsServer = oServer.GetAnswer(intID);
                StringBuilder sbHosts = new StringBuilder(strHosts);
                foreach (DataRow drServer in dsServer.Tables[0].Rows)
                {
                    int intServer = Int32.Parse(dsServer.Tables[0].Rows[0]["id"].ToString());
                    sbHosts.Append("<tr>");
                    sbHosts.Append("<td>");
                    sbHosts.Append(oServer.GetName(intServer, boolUsePNCNaming));
                    sbHosts.Append("</td>");
                    sbHosts.Append("<td>");
                    sbHosts.Append(oServer.GetIPs(intServer, 0, 1, 0, 0, dsnIP, "", ""));
                    sbHosts.Append("</td>");
                    sbHosts.Append("<td>");
                    sbHosts.Append(oOperatingSystem.Get(Int32.Parse(dsServer.Tables[0].Rows[0]["osid"].ToString()), "name"));
                    sbHosts.Append("</td>");
                    sbHosts.Append("<td>");
                    sbHosts.Append(oServicePack.Get(Int32.Parse(dsServer.Tables[0].Rows[0]["spid"].ToString()), "name"));
                    sbHosts.Append("</td>");
                    sbHosts.Append("</tr>");
                }
                strHosts = sbHosts.ToString();
                lblHostNone.Visible = (strHosts == "");
                lblRecoveryLocation.Text = "Ops Center";
                // Get data from backup table
                DataSet dsBackup = oForecast.GetBackup(intID);
                if (dsBackup.Tables[0].Rows.Count > 0)
                {
                    if (dsBackup.Tables[0].Rows[0]["daily"].ToString() == "1")
                        lblBackupFreq.Text = "Daily";
                    else if (dsBackup.Tables[0].Rows[0]["weekly"].ToString() == "1")
                        lblBackupFreq.Text = "Weekly";
                    else if (dsBackup.Tables[0].Rows[0]["monthly"].ToString() == "1")
                        lblBackupFreq.Text = "Monthly";
                    if (dsBackup.Tables[0].Rows[0]["time"].ToString() == "1")
                        lblCSB.Text = dsBackup.Tables[0].Rows[0]["time_hour"].ToString() + " " + dsBackup.Tables[0].Rows[0]["time_switch"].ToString();
                    else
                        lblCSB.Text = "Don't Care";
                    lblAvgSize.Text = dsBackup.Tables[0].Rows[0]["average_one"].ToString() + " GB";
                    lblPTD.Text = dsBackup.Tables[0].Rows[0]["documentation"].ToString();
                    if (lblPTD.Text == "")
                    {
                        lblPTD.Text = "Not Specified";
                        lblPTD.CssClass = "redbold";
                    }
                    if (dsBackup.Tables[0].Rows[0]["start_date"].ToString() != "")
                        lblFirstBackupDate.Text = DateTime.Parse(dsBackup.Tables[0].Rows[0]["start_date"].ToString()).ToShortDateString();
                }
                // Load Exclusions
                DataSet dsExclusions = oForecast.GetBackupExclusions(intID);
                foreach (DataRow drExclusion in dsExclusions.Tables[0].Rows)
                    strExclusions += "<tr><td>" + drExclusion["path"].ToString() + "</td></tr>";
                if (strExclusions == "")
                    strExclusions = "<tr><td class=\"redbold\">No Exclusions</td></tr>";
                // Load Requirements
                DataSet dsRequirements = oForecast.GetBackupRetentions(intID);
                StringBuilder sbRequirements = new StringBuilder(strRequirements);
                foreach (DataRow drRequirement in dsRequirements.Tables[0].Rows)
                {
                    sbRequirements.Append("<tr><td nowrap>Path:</td><td width=\"100%\">");
                    sbRequirements.Append(drRequirement["path"].ToString());
                    sbRequirements.Append("</td></tr>");
                    sbRequirements.Append("<tr><td nowrap>First Archival:</td><td width=\"100%\">");
                    sbRequirements.Append(DateTime.Parse(drRequirement["first"].ToString()).ToShortDateString());
                    sbRequirements.Append("</td></tr>");
                    sbRequirements.Append("<tr><td nowrap>Retention Period:</td><td width=\"100%\">");
                    sbRequirements.Append(drRequirement["number"].ToString() == "0" ? "" : drRequirement["number"].ToString() + " ");
                    sbRequirements.Append(drRequirement["type"].ToString());
                    sbRequirements.Append("</td></tr>");
                    sbRequirements.Append("<tr><td nowrap>Start Time:</td><td width=\"100%\">");
                    sbRequirements.Append(drRequirement["hour"].ToString());
                    sbRequirements.Append(" ");
                    sbRequirements.Append(drRequirement["switch"].ToString());
                    sbRequirements.Append("</td></tr>");
                    sbRequirements.Append("<tr><td nowrap>Frequency:</td><td width=\"100%\">");
                    sbRequirements.Append(drRequirement["occurence"].ToString());
                    sbRequirements.Append(drRequirement["occurs"].ToString() == "" ? "" : " occurring on " + drRequirement["occurs"].ToString());
                    sbRequirements.Append("</td></tr>");
                    sbRequirements.Append("<tr><td colspan=\"2\">&nbsp;</td></tr>");
                }
                strRequirements = sbRequirements.ToString();
                lblCCDU.Text = "5 GB";
                lblPolicyExcl.Text = "No Exceptions";
            }
        }
    }
}
