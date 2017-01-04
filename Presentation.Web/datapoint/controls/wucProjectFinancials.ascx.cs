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
    public partial class wucProjectFinancials : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile = 0;
        protected int intApplication = 0;
      
        private DataPoint oDataPoint;
        
        private int intProjectId;
        public int ProjectId
        {
            get { return intProjectId; }
            set { intProjectId = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                    intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
                if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                    intApplication = Int32.Parse(Request.QueryString["applicationid"]);
                if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                    intApplication = Int32.Parse(Request.Cookies["application"].Value);

                oDataPoint = new DataPoint(intProfile, dsn);
                GetProjectFinancials();
            }
        }

        private void GetProjectFinancials()
        {
          
            DataSet ds = oDataPoint.GetProjectFinancials(intProjectId);
            if (ds.Tables.Count != 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    pnlProjectFinancials.Visible = true;
                    //Get the details
                    #region Details Totals
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["Phase"].ToString() == "1")//Discovery
                        {

                            if (dr["Source"].ToString() == "1") //Internal Labor
                            {
                                lblDisILApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblDisILActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblDisILEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblDisILCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblDisILVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblDisILVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                            else if (dr["Source"].ToString() == "2") //External Labor
                            {
                                lblDisELApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblDisELActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblDisELEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblDisELCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblDisELVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblDisELVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                            else if (dr["Source"].ToString() == "3") //HW/SW/One Time Cost
                            {
                                lblDisHSApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblDisHSActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblDisHSEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblDisHSCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblDisHSVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblDisHSVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                        }
                        else if (dr["Phase"].ToString() == "2")//Planning
                        {

                            if (dr["Source"].ToString() == "1") //Internal Labor
                            {
                                lblPlanILApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblPlanILActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblPlanILEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblPlanILCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblPlanILVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblPlanILVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                            else if (dr["Source"].ToString() == "2") //External Labor
                            {
                                lblPlanELApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblPlanELActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblPlanELEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblPlanELCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblPlanELVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblPlanELVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                            else if (dr["Source"].ToString() == "3") //HW/SW/One Time Cost
                            {
                                lblPlanHSApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblPlanHSActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblPlanHSEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblPlanHSCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblPlanHSVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblPlanHSVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                        }
                        else if (dr["Phase"].ToString() == "3")//Execution
                        {

                            if (dr["Source"].ToString() == "1") //Internal Labor
                            {
                                lblExecILApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblExecILActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblExecILEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblExecILCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblExecILVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblExecILVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                            else if (dr["Source"].ToString() == "2") //External Labor
                            {
                                lblExecELApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblExecELActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblExecELEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblExecELCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblExecELVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblExecELVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                            else if (dr["Source"].ToString() == "3") //HW/SW/One Time Cost
                            {
                                lblExecHSApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblExecHSActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblExecHSEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblExecHSCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblExecHSVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblExecHSVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                        }

                        else if (dr["Phase"].ToString() == "4")//Closing
                        {

                            if (dr["Source"].ToString() == "1") //Internal Labor
                            {
                                lblCloseILApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblCloseILActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblCloseILEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblCloseILCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblCloseILVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblCloseILVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                            else if (dr["Source"].ToString() == "2") //External Labor
                            {
                                lblCloseELApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblCloseELActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblCloseELEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblCloseELCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblCloseELVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblCloseELVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                            else if (dr["Source"].ToString() == "3") //HW/SW/One Time Cost
                            {
                                lblCloseHSApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblCloseHSActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblCloseHSEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblCloseHSCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblCloseHSVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblCloseHSVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                        }
                    }
                    #endregion
                    //Get the Phase total
                    #region Phase Totals
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            if (dr["Phase"].ToString() == "1")//Discovery
                            {
                                lblDisApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblDisActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblDisEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblDisCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblDisVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblDisVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                            else if (dr["Phase"].ToString() == "2")//Planning
                            {
                                lblPlanApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblPlanActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblPlanEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblPlanCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblPlanVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblPlanVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                            else if (dr["Phase"].ToString() == "3")//Execution
                            {
                                lblExecApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblExecActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblExecEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblExecCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblExecVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblExecVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                            else if (dr["Phase"].ToString() == "4")//Closing
                            {
                                lblCloseApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblCloseActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblCloseEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblCloseCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblCloseVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblCloseVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                        }
                    }
                    #endregion

                    //Get the Sourcewise total
                    #region Phase Totals
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            if (dr["Source"].ToString() == "1")//Internal Labor
                            {
                                lblILApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblILActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblILEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblILCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblILVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblILVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                            else if (dr["Source"].ToString() == "2") //External Labor
                            {
                                lblELApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblELActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblELEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblELCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblELVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblELVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }
                            else if (dr["Source"].ToString() == "3") //HW/SW/One Time Cost
                            {
                                lblHSApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                                lblHSActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                                lblHSEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                                lblHSCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                                lblHSVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                                lblHSVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                            }

                        }
                    }
                    #endregion

                    //Get the Project total
                    #region Phase Totals
                    if (ds.Tables[3].Rows.Count == 1)
                    {
                        DataRow dr = ds.Tables[3].Rows[0];
                        lblApprovedC1PCRs.Text = double.Parse(dr["ApprovedC1PCRs"].ToString()).ToString("F");
                        lblActualsToDate.Text = double.Parse(dr["ActualsToDate"].ToString()).ToString("F");
                        lblEstimateToComplete.Text = double.Parse(dr["EstimateToComplete"].ToString()).ToString("F");
                        lblCurrentForecast.Text = double.Parse(dr["CurrentForecast"].ToString()).ToString("N");
                        lblVariance.Text = double.Parse(dr["Variance"].ToString()).ToString("N");
                        lblVariancePercent.Text = double.Parse(dr["VariancePercent"].ToString()).ToString("N");
                    }
                    #endregion
                }
                else
                    lblNoProjectFinancials.Visible = true;
            }
        }


    }
}
   