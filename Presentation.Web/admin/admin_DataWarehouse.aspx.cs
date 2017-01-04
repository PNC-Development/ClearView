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
    public partial class admin_DataWarehouse : BasePage
    {
        protected int intProfile;
        protected string dsnDW = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ClearViewDWDSN"]].ConnectionString;
        protected string strSQLJobName = ConfigurationManager.AppSettings["CLEARVIEWDW_SQL_JOB_NAME"];
        protected ManageSQLJobs oManageSQLJobs;


        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/admin_ui_controls.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");

            oManageSQLJobs = new ManageSQLJobs(intProfile, dsnDW);
            if (!IsPostBack)
            {

            }
            GetJobDetails();
        }

        protected void GetJobDetails()
        {
            DataSet dsSQLJob = oManageSQLJobs.GetJobDetails(strSQLJobName);

            DataRow drJobStatus = dsSQLJob.Tables[0].Rows[0];

            lblLastRunDate.Text = drJobStatus["last_run_date"].ToString();
            lblLastRunTime.Text = drJobStatus["last_run_time"].ToString();
            lblLastRunStatus.Text = (drJobStatus["last_run_outcome"].ToString() == "1" ? "Successful" : "Failed");
            if (drJobStatus["last_run_outcome"].ToString() == "0")
                lblLastRunStatus.CssClass = "note";
            else
                lblLastRunStatus.CssClass = "default";

            if (drJobStatus["current_execution_status"].ToString() == "1") //Currently running
            {
                lblCurrentJobStatus.CssClass = "note";
                lblCurrentJobStatus.Text = "Running";
                btnRunJob.Enabled = false;
            }
            else
            {
                lblCurrentJobStatus.CssClass = "default";
                lblCurrentJobStatus.Text = "Not Running";
                btnRunJob.Enabled = true;
                btnRunJob.Attributes.Clear();
                btnRunJob.Attributes.Add("onclick", "return confirm('Are you sure you want to run the Data Warehouse job?');");

            }

            lblNextRunDate.Text = (drJobStatus["next_run_date"].ToString() == "0" ? "--" : drJobStatus["next_run_date"].ToString());
            lblNextRunTime.Text = (drJobStatus["next_run_time"].ToString() == "0" ? "--" : drJobStatus["next_run_time"].ToString());

            string strCurrentExeStep=drJobStatus["current_execution_step"].ToString() ;
            strCurrentExeStep = strCurrentExeStep.Remove(strCurrentExeStep.IndexOf(" "));
            hdnCurrentExecutionStepId.Value=strCurrentExeStep;


            DataRow drLastJobOutCome = dsSQLJob.Tables[3].Rows[0];
            lblRunOutComeMsg.Text = drLastJobOutCome["last_outcome_message"].ToString();

            dlJobSteps.DataSource = dsSQLJob.Tables[1];
            dlJobSteps.DataBind();
        }

        protected void RunJob()
        {
            lblCurrentJobStatus.CssClass = "note";
            lblCurrentJobStatus.Text = "Started...";
            btnRunJob.Enabled = false;
            oManageSQLJobs.RunJob(strSQLJobName);
            GetJobDetails();
        }

        protected void btnRunJob_Click(object sender, EventArgs e)
        {
            RunJob();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            GetJobDetails();
        }

        protected void dlJobSteps_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
            {
                Label lblDLStepStatus = (Label)e.Item.FindControl("lblDLStepStatus");
                Label lblDLStepId = (Label)e.Item.FindControl("lblDLStepId");
                Label lblDLStepName = (Label)e.Item.FindControl("lblDLStepName");
                Label lblDLStepLastRunDate = (Label)e.Item.FindControl("lblDLStepLastRunDate");
                Label lblDLStepLastRunTime = (Label)e.Item.FindControl("lblDLStepLastRunTime");
                Label lblDLStepLastRunOutCome = (Label)e.Item.FindControl("lblDLStepLastRunOutCome");

                DataRowView drv = (DataRowView)e.Item.DataItem;
                lblDLStepId.Text = drv["step_id"].ToString();
                
                if (hdnCurrentExecutionStepId.Value == lblDLStepId.Text)
                    lblDLStepStatus.Text = "<img src=\"/admin/images/please_wait.gif\" border=\"0\"  height=\"15\"  width=\"15\"  align=\"absmiddle\"/>";
                else
                    lblDLStepStatus.Text = " ";

                lblDLStepName.Text = drv["step_name"].ToString();
                lblDLStepLastRunDate.Text = drv["last_run_date"].ToString();
                lblDLStepLastRunTime.Text = drv["last_run_time"].ToString();
                if (drv["last_run_outcome"].ToString() == "0")
                {
                    lblDLStepLastRunOutCome.Text = "Failed";
                    lblDLStepId.CssClass = "note";
                    lblDLStepName.CssClass = "note";
                    lblDLStepLastRunDate.CssClass = "note";
                    lblDLStepLastRunTime.CssClass = "note";
                    lblDLStepLastRunOutCome.CssClass = "note";
                }

                else
                {
                    lblDLStepLastRunOutCome.Text = "Successful";
                    lblDLStepId.CssClass = "default";
                    lblDLStepName.CssClass = "default";
                    lblDLStepLastRunDate.CssClass = "default";
                    lblDLStepLastRunTime.CssClass = "default";
                    lblDLStepLastRunOutCome.CssClass = "default";
                }

                
            }

        }
    }
}
