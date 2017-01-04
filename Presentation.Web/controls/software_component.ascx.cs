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
    public partial class software_component : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Forecast oForecast;
        protected ServerName oServerName;
        protected Servers oServer;
        protected Pages oPage;
        protected OperatingSystems oOperatingSystem;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intAnswer = 0;
        private string strEMailIdsBCC = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
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
                            int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
                            if (intModel == 0)
                                intModel = oForecast.GetModelAsset(intAnswer);
                            if (intModel == 0)
                                intModel = oForecast.GetModel(intAnswer);
                            lblModel.Text = oModelsProperties.Get(intModel, "name");
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
                            lblRequestor.Text = ds.Tables[0].Rows[0]["userid"].ToString();
                            lblDesignedOn.Text = DateTime.Parse(ds.Tables[0].Rows[0]["created"].ToString()).ToString();
                            lblUpdated.Text = DateTime.Parse(ds.Tables[0].Rows[0]["modified"].ToString()).ToString();
                            panShow.Visible = true;
                            btnSave.Enabled = true;
                            // Load Components
                            rptApproval.DataSource = oServerName.GetComponentDetailUserApprovalsByUser(intProfile, intAnswer, 0);
                            rptApproval.DataBind();
                            foreach (RepeaterItem ri in rptApproval.Items)
                            {
                                int intServer = Int32.Parse(((Label)ri.FindControl("lblServer")).Text);
                                int intDetail = Int32.Parse(((Label)ri.FindControl("lblDetail")).Text);
                                RadioButton radApprove = (RadioButton)ri.FindControl("radApprove");
                                RadioButton radDeny = (RadioButton)ri.FindControl("radDeny");
                                HtmlTableRow trLicense = (HtmlTableRow)ri.FindControl("trLicense");
                                HtmlTableRow trComments = (HtmlTableRow)ri.FindControl("trComments");
                                TextBox txtLicense = (TextBox)ri.FindControl("txtLicense");
                                TextBox txtComments = (TextBox)ri.FindControl("txtComments");
                                Label lblOS = (Label)ri.FindControl("lblOS");
                                int intOS = 0;
                                Int32.TryParse(oServer.Get(intServer, "osid"), out intOS);
                                lblOS.Text = oOperatingSystem.Get(intOS, "name");
                                radApprove.Attributes.Add("onclick", "ShowHideDiv('" + trLicense.ClientID + "','inline');ShowHideDiv('" + trComments.ClientID + "','inline');");
                                radDeny.Attributes.Add("onclick", "ShowHideDiv('" + trLicense.ClientID + "','none');ShowHideDiv('" + trComments.ClientID + "','inline');");
                                DataSet dsResult = oServerName.GetComponentDetailUserApprovals(intServer, intDetail);
                                if (dsResult.Tables[0].Rows.Count > 0)
                                {
                                    // Load previous information
                                    DataRow drResult = dsResult.Tables[0].Rows[0];
                                    if (drResult["approved"].ToString() == "1")
                                    {
                                        radApprove.Checked = true;
                                        txtLicense.Text = drResult["license"].ToString();
                                        trLicense.Style["display"] = "inline";
                                        txtComments.Text = drResult["comments"].ToString();
                                        trComments.Style["display"] = "inline";
                                    }
                                    if (drResult["approved"].ToString() == "0")
                                    {
                                        radDeny.Checked = true;
                                        txtComments.Text = drResult["comments"].ToString();
                                        trComments.Style["display"] = "inline";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            string strResult = "";
            Functions oFunction = new Functions(intProfile, dsn, intEnvironment);
            Variables oVariable = new Variables(intEnvironment);
            Users oUser = new Users(intProfile, dsn);
            bool boolDenied = false;

            foreach (RepeaterItem ri in rptApproval.Items)
            {
                int intServer = Int32.Parse(((Label)ri.FindControl("lblServer")).Text);
                int intDetail = Int32.Parse(((Label)ri.FindControl("lblDetail")).Text);
                oServerName.DeleteComponentDetailUserApproval(intServer, intDetail);

                TextBox txtLicense = (TextBox)ri.FindControl("txtLicense");
                TextBox txtComments = (TextBox)ri.FindControl("txtComments");

                strResult += "<tr>";
                strResult += "<td>" + oServerName.GetComponentDetailName(intDetail) + "</td>";
                RadioButton radApprove = (RadioButton)ri.FindControl("radApprove");
                if (radApprove.Checked == true)
                {
                    oServerName.AddComponentDetailUserApproval(intServer, intDetail, intProfile, 1, txtComments.Text, txtLicense.Text);
                    strResult += "<td>Approved by " + oUser.GetFullName(intProfile) + "</td>";
                }

                RadioButton radDeny = (RadioButton)ri.FindControl("radDeny");
                if (radDeny.Checked == true)
                {
                    boolDenied = true;
                    oServerName.AddComponentDetailUserApproval(intServer, intDetail, intProfile, 0, txtComments.Text, "");
                    strResult += "<td><b>Denied by " + oUser.GetFullName(intProfile) + "</b></td>";
                    oServer.UpdateConfigured(intServer, 0);
                }

                string strComments = "";
                if (txtComments.Text != "")
                    strComments = oFunction.FormatText(txtComments.Text);
                strResult += "<td>" + strComments + "</td>";
            }

            strResult = "<tr bgcolor=\"#EEEEEE\"><td><b>Component</b></td><td><b>Status</b></td><td><b>Comments</b></td></tr>" + strResult;
            strResult = "<table border=\"0\" cellpadding=\"5\" cellspacing=\"0\" style=\"border:solid 1px #CCCCCC\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strResult + "</table>";
            // Send Email to Requestor
            string strSubject = "Software Component(s) " + (boolDenied ? "Denied" : "Approved");
            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DESIGN_BUILDER");
            oFunction.SendEmail(strSubject, oUser.GetName(Int32.Parse(lblRequestor.Text)), "", strEMailIdsBCC, strSubject, "<p><b>This message is to inform you that a software component approver has taken action on your software component request(s) for the following design...</b></p><p>" + oForecast.GetAnswerBody(intAnswer, intEnvironment, dsnAsset, dsnIP) + "</p><p>" + strResult + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/ondemand/status.aspx?id=" + intAnswer.ToString() + "\" target=\"_blank\">Click here to execute this design.</a></p>", true, false);

            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intAnswer.ToString() + "&action=done");
        }
        protected void btnFinish_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLinkRelated(intPage));
        }
    }
}