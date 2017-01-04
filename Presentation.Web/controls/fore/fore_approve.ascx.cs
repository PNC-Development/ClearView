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
    public partial class fore_approve : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strWorkflowBCC = ConfigurationManager.AppSettings["WorkflowBCC"];

        protected int intConfidenceUnlock = Int32.Parse(ConfigurationManager.AppSettings["CONFIDENCE_UNLOCK"]);
        protected int intProfile;
        protected Forecast oForecast;
        protected Pages oPage;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intAnswer = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.QueryString["action"] != null && Request.QueryString["action"] != "")
                panFinish.Visible = true;
            else
            {
                if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                    intApplication = Int32.Parse(Request.QueryString["applicationid"]);
                if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                    intApplication = Int32.Parse(Request.Cookies["application"].Value);
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                    intAnswer = Int32.Parse(Request.QueryString["id"]);
                if (!IsPostBack)
                {
                    if (intAnswer > 0)
                    {
                        DataSet ds = oForecast.GetAnswer(intAnswer);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Projects oProject = new Projects(intProfile, dsn);
                            Requests oRequest = new Requests(intProfile, dsn);
                            Organizations oOrganization = new Organizations(intProfile, dsn);
                            Segment oSegment = new Segment(intProfile, dsn);
                            Platforms oPlatform = new Platforms(intProfile, dsn);
                            ModelsProperties oModelsProperties = new ModelsProperties(intProfile, dsn);
                            Confidence oConfidence = new Confidence(intProfile, dsn);
                            Classes oClass = new Classes(intProfile, dsn);
                            Environments oEnvironment = new Environments(intProfile, dsn);
                            Locations oLocation = new Locations(intProfile, dsn);
                            Users oUser = new Users(intProfile, dsn);
                            lblID.Text = intAnswer.ToString();
                            int intForecast = Int32.Parse(ds.Tables[0].Rows[0]["forecastid"].ToString());
                            int intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                            int intProject = oRequest.GetProjectNumber(intRequest);
                            lblName.Text = oProject.Get(intProject, "name");
                            lblNumber.Text = oProject.Get(intProject, "number");
                            lblPortfolio.Text = oOrganization.GetName(Int32.Parse(oProject.Get(intProject, "organization")));
                            lblSegment.Text = oSegment.GetName(Int32.Parse(oProject.Get(intProject, "segmentid")));
                            lblPlatform.Text = oPlatform.GetName(Int32.Parse(ds.Tables[0].Rows[0]["platformid"].ToString()));
                            lblNickname.Text = ds.Tables[0].Rows[0]["name"].ToString();
                            lblModel.Text = oModelsProperties.Get(Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString()), "name");
                            lblCommitment.Text = DateTime.Parse(ds.Tables[0].Rows[0]["implementation"].ToString()).ToLongDateString();
                            lblQuantity.Text = ds.Tables[0].Rows[0]["quantity"].ToString();
                            lblConfidence.Text = oConfidence.Get(Int32.Parse(ds.Tables[0].Rows[0]["confidenceid"].ToString()), "name");
                            int intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                            lblClass.Text = oClass.Get(intClass, "name");
                            if (oClass.IsProd(intClass))
                            {
                                panTest.Visible = true;
                                lblTest.Text = (ds.Tables[0].Rows[0]["test"].ToString() == "1" ? "Yes" : "No");
                            }
                            lblEnvironment.Text = oEnvironment.Get(Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString()), "name");
                            lblLocation.Text = oLocation.GetFull(Int32.Parse(ds.Tables[0].Rows[0]["addressid"].ToString()));
                            lblIP.Text = ds.Tables[0].Rows[0]["workstation"].ToString();
                            lblDesignedBy.Text = oUser.GetFullName(Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString()));
                            lblDesignedOn.Text = DateTime.Parse(ds.Tables[0].Rows[0]["created"].ToString()).ToString();
                            lblUpdated.Text = DateTime.Parse(ds.Tables[0].Rows[0]["modified"].ToString()).ToString();
                            panShow.Visible = true;
                            btnApprove.Enabled = true;
                            btnDeny.Enabled = true;
                        }
                    }
                }
            }
            btnApprove.Attributes.Add("onclick", "return confirm('Are you sure you want to APPROVE this request?');");
            btnDeny.Attributes.Add("onclick", "return ValidateText('" + txtComments.ClientID + "','Please enter a reason for the rejection of this request') && confirm('Are you sure you want to DENY this request?');");
        }
        protected void btnApprove_Click(Object Sender, EventArgs e)
        {
            oForecast.UpdateAnswerApproval(intAnswer, 1, intProfile, txtComments.Text);
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intAnswer.ToString() + "&action=done");
        }
        protected void btnDeny_Click(Object Sender, EventArgs e)
        {
            oForecast.UpdateAnswerApproval(intAnswer, -10, intProfile, txtComments.Text);
            oForecast.UpdateAnswerConfidence(intAnswer, intConfidenceUnlock);
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intAnswer.ToString() + "&action=done");
        }
        protected void btnFinish_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLinkRelated(intPage));
        }
    }
}