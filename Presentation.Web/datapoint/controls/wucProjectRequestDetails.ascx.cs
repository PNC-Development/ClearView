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
    public partial class wucProjectRequestDetails : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile = 0;
        protected int intApplication = 0;
        private Users oUser;
        private Services oService;
        private int? intProjectId;
        public int? ProjectId
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

                oUser = new Users(intProfile, dsn);
                oService = new Services(intProfile, dsn);

                LoadProjectRequest();
            }
        }

        private void LoadProjectRequest()
        {   
            ProjectRequest oProjectRequest =new ProjectRequest(intProfile,dsn);
            DataSet ds = oProjectRequest.GetProjectRequestDetails(intProjectId);
            if (ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
            {
                pnlProjectRequest.Visible = true;
                DataRow dr = ds.Tables[0].Rows[0];
                lblRequestedBy.Text =dr["ReqUserName"].ToString() +
                                    (dr["ReqUserXID"].ToString() != "" ? "(" + dr["ReqUserXID"].ToString() + ")" : "");
                lblRequestedOnValue.Text = dr["ReqModifiedDate"].ToString();

                lblC1.Text=(dr["ProjectReqC1"].ToString() == "1" ? "Yes" : "No");
                lblEndLife.Text=(dr["ProjectReqEndLife"].ToString() == "1" ? "Yes" : "No");
                if (dr["ProjectReqEndLife"].ToString() == "1")
                {
                    lblEndLifeDateValue.Text=(dr["ProjectReqEndlifeDate"].ToString()!=""? DateTime.Parse(dr["ProjectReqEndlifeDate"].ToString()).ToShortDateString():"");
                    lblEndLifeDate.Visible=true;
                    lblEndLifeDateValue.Visible=true;
                }
                
                

                lblInitiativeOpportunity.Text=dr["ReqDescription"].ToString();
                //Get Platform
                DataSet dsPlatform = oProjectRequest.GetPlatforms(Int32.Parse(dr["RequestID"].ToString()));
                foreach (DataRow drPlatform in dsPlatform.Tables[0].Rows)
                            lblPlatforms.Text += drPlatform["name"].ToString() + "<br/>";

               
                lblAuditReq.Text=(dr["ProjectReqType"].ToString() == "1" ? "Yes" : "No");
                if (dr["ProjectReqType"].ToString() == "1")
                {
                    lblAuditReqDateValue.Text = (dr["ProjectReqDate"].ToString() != "" ? DateTime.Parse(dr["ProjectReqDate"].ToString()).ToShortDateString() : "");
                    lblAuditReqDate.Visible = true;
                    lblAuditReqDateValue.Visible = true;
                }
                            
                lblInterdependency.Text=dr["ProjectReqInterdependency"].ToString();
                if (dr["ProjectReqProjects"].ToString()!="")
                {
                    lblProjectValues.Text=dr["ProjectReqProjects"].ToString();
                    lblProject.Visible=true;
                    lblProjectValues.Visible=true;
                }

                lblTechnolgyCapability.Text=dr["ProjectReqTechCapabilities"].ToString();
                lblEstimatedManHours.Text=dr["ProjectReqManHours"].ToString();
                
                lblDiscoveryStartDate.Text=(dr["ReqStartDate"].ToString()!=""? DateTime.Parse(dr["ReqStartDate"].ToString()).ToShortDateString():"");
                lblExpectedCompletionDate.Text=(dr["ReqEndDate"].ToString()!=""? DateTime.Parse(dr["ReqEndDate"].ToString()).ToShortDateString():"");

                lblExpectedCapitalCost.Text=dr["ProjectReqExpectedCapital"].ToString();
                lblInternalLabor.Text=dr["ProjectReqInternalLabor"].ToString();
                lblExternalLabor.Text=dr["ProjectReqExternalLabor"].ToString();

                lblMaintenanceCostIncrease.Text=dr["ProjectReqMaintenanceIncrease"].ToString();
                lblProjectExpenses.Text=dr["ProjectReqProjectExpenses"].ToString();

                lblEstimatedCostAvoidance.Text=dr["ProjectReqEstimatedAvoidance"].ToString();
                lblEstimatedCostSavings.Text=dr["ProjectReqEstimatedSavings"].ToString();

                lblRealizedCostSavings.Text=dr["ProjectReqRealizedSavings"].ToString();

                lblBusinessImpactAvoidance.Text=dr["ProjectReqBusinessAvoidance"].ToString();

                lblMaintenanceCostAvoidance.Text=dr["ProjectReqMaintenanceAvoidance"].ToString();

                lblAssetReusability.Text=dr["ProjectReqAssetReusability"].ToString();
                
                lblInternalCustomerImpact.Text=dr["ProjectReqInternalImpact"].ToString();
                lblExternalCustomerImpact.Text=dr["ProjectReqExternalImpact"].ToString();

                lblBusinessImpact.Text=dr["ProjectReqBusinessImpact"].ToString();
                lblStrategicOpportunity.Text=dr["ProjectReqStrategicOpportunity"].ToString();


                lblAcquisitionBIC.Text=dr["ProjectReqAcquisition"].ToString();
                
                lblCapabilities.Text=dr["ProjectReqCapability"].ToString();

                lblTPMRequested.Text = (dr["ProjectReqTPM"].ToString() == "1" ? "Yes" : "No");
                if (dr["ProjectReqTPM"].ToString() == "1")
                {
                    lblServiceType.Visible = true;
                    lblServiceTypeValue.Visible = true;
                    lblProjectLead.Visible = true;
                    lblProjectLeadValue.Visible = true;

                    DataSet dsProjectReq = oProjectRequest.GetResources(Int32.Parse(dr["RequestID"].ToString()));
                    if (dsProjectReq.Tables[0].Rows.Count > 0)
                    {
                        int intItem = Int32.Parse(dsProjectReq.Tables[0].Rows[0]["itemid"].ToString());
                        int intService = Int32.Parse(dsProjectReq.Tables[0].Rows[0]["serviceid"].ToString());
                        int intAccepted = Int32.Parse(dsProjectReq.Tables[0].Rows[0]["accepted"].ToString());
                        int intManager = Int32.Parse(dsProjectReq.Tables[0].Rows[0]["userid"].ToString());
                        if (intItem > 0)
                        {
                            lblServiceTypeValue.Text = oService.GetName(intService);
                            lblServiceType.Visible = true;
                            lblServiceTypeValue.Visible = true;
                        }
                        if (intAccepted == -1)
                            lblProjectLeadValue.Text = "Pending Assignment (" + dr["ReqUserName"].ToString() + ")";
                        else if (intManager > 0)
                            lblProjectLeadValue.Text = oUser.GetFullName(intManager);
                        else if (intItem > 0)
                            lblProjectLeadValue.Text = "Pending Assignment";
                    }
                }
            }
            else
            {
                pnlProjectRequest.Visible = false;
                lblNoProjectReqInfo.Visible = true;
            }
        }
    }
}