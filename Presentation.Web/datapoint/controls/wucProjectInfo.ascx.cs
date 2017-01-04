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
    public partial class wucProjectInfo : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile = 0;
        protected int intApplication = 0;
        private Users oUser;
        protected DataPoint oDataPoint;
        
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
                oDataPoint = new DataPoint(intProfile, dsn);
                LoadProjectInfo();
            }
        }

        private void LoadProjectInfo()
        { 
             DataSet dsProject = oDataPoint.GetProjectSearchResults(
                                intProjectId,
                                null,
                                null,
                                null,
                                null, null, null, null, null, null, null, null,
                                null, 0, 1, 0);

             if (dsProject.Tables.Count != 0 && dsProject.Tables[0].Rows.Count > 0)
             {
                 pnlProjectInfo.Visible = true;
                 DataRow dr = dsProject.Tables[0].Rows[0];

                 lblProjectNumber.Text = (dr["ProjectNumber"] != DBNull.Value ? dr["ProjectNumber"].ToString() : "");


                 lblProjectName.Text = dr["ProjectName"].ToString();
                 lblProjectName.ToolTip = dr["ProjectId"].ToString();

                 string strProjectManager = "";
                 strProjectManager = dr["ProjectLeadName"].ToString();
                 strProjectManager += (dr["ProjectLeadXID"].ToString() != "" ? "(" + dr["ProjectLeadXID"].ToString() + ")" : "");
                 lblProjectManager.Text = strProjectManager;
                 lblProjectManager.Attributes.Add("onclick", "return OpenWindow('PROFILE','?userid=" + dr["ProjectLead"].ToString() + "');");


                 lblProjectType.Text = dr["ProjectBaseDiscretion"].ToString();
                 lblProjectStatus.Text = dr["ProjectStatus"].ToString();

                 string strExecutiveSponsor = "";
                 strExecutiveSponsor = dr["ProjectExecutiveSponsorName"].ToString();
                 strExecutiveSponsor += (dr["ProjectExecutiveSponsorXID"].ToString() != "" ? "(" + dr["ProjectExecutiveSponsorXID"].ToString() + ")" : "");
                 lblExecutiveSponsor.Text = strExecutiveSponsor;
                 lblExecutiveSponsor.Attributes.Add("onclick", "return OpenWindow('PROFILE','?userid=" + dr["ProjectExecutiveSponsorID"].ToString() + "');");

                 string strWorkingSponsor = "";
                 strWorkingSponsor = dr["ProjectWorkingSponsorName"].ToString();
                 strWorkingSponsor += (dr["ProjectWorkingSponsorXID"].ToString() != "" ? "(" + dr["ProjectWorkingSponsorXID"].ToString() + ")" : "");
                 lblWorkingSponsor.Text = strWorkingSponsor;
                 lblWorkingSponsor.Attributes.Add("onclick", "return OpenWindow('PROFILE','?userid=" + dr["ProjectWorkingSponsorID"].ToString() + "');");

                 lblOrganization.Text = dr["ProjectOrgName"].ToString();
                 lblSegment.Text = dr["ProjectSegmentName"].ToString();
                 lblProjectInitiatedOn.Text = (dr["ProjectModified"].ToString() != "" ? DateTime.Parse(dr["ProjectModified"].ToString()).ToShortDateString() : "");

             }
             else
             {
                 lblNoProjectInfo.Visible = true;
             }
        }
    }
}