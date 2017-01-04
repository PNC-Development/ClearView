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
using System.IO;
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class enhancement_new : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intService = Int32.Parse(ConfigurationManager.AppSettings["HELP_SERVICEID"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
       
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);

        protected Requests oRequest;
        protected Services oService;
        protected ServiceRequests oServiceRequest;
        protected Customized oCustomized;
        protected Pages oPage;
        protected ResourceRequest oResourceRequest;
        protected StatusLevels oStatusLevel;
        protected Users oUser;
        protected Applications oApplication;
        protected Enhancements oEnhancement;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProfile;
        protected int intRequest;
        protected int intItemID;
        protected int intId;
        protected string strMessages = "";
        private Variables oVariables;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oRequest = new Requests(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oStatusLevel = new StatusLevels();
            oUser = new Users(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oEnhancement = new Enhancements(intProfile, dsn);
            oVariables = new Variables(intEnvironment);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intId = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["old"] != null && Request.QueryString["old"] != "")
                intId = Int32.Parse(Request.QueryString["old"]);

            if (!IsPostBack)
            {
                drpModules.DataValueField = "pageid";
                drpModules.DataTextField = "menutitle";
                drpModules.DataSource = oPage.Gets(intApplication, intProfile, 0, 1, 1);
                drpModules.DataBind();
                drpModules.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            }

            if (intId > 0)
            {
                panEnhancement.Visible = true;
                DataSet ds = oCustomized.GetEnhancementByID(intId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    panStatus.Visible = true;
                    lblTitle.Text = "Edit Enhancement";
                    txtTitle.Text = ds.Tables[0].Rows[0]["title"].ToString();
                    txtDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                    drpModules.SelectedValue = ds.Tables[0].Rows[0]["pageid"].ToString();
                    txtNumUsers.Text = ds.Tables[0].Rows[0]["num_users"].ToString();
                    txtURL.Text = ds.Tables[0].Rows[0]["url"].ToString();
                    if (ds.Tables[0].Rows[0]["path"].ToString() != "")
                    {
                        panUploaded.Visible = true;
                        hypUpload.NavigateUrl = ds.Tables[0].Rows[0]["path"].ToString();
                    }
                    else
                    {
                        panUpload.Visible = true;
                    }
                    txtStartDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["startdate"].ToString()).ToShortDateString();
                    txtEndDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["enddate"].ToString()).ToShortDateString();
                    lblStatus.Text = oStatusLevel.HTMLSupport(Int32.Parse(ds.Tables[0].Rows[0]["status"].ToString()));
                    lblStatus.Attributes.Add("oncontextmenu", "alert('CVT" + intRequest.ToString() + "');");
                    if (ds.Tables[0].Rows[0]["status"].ToString() == "0")
                    {
                        btnUpdate.Visible = true;
                        panActive.Visible = true;
                    }
                    int intRelease = 0;
                    if (Int32.TryParse(ds.Tables[0].Rows[0]["release"].ToString(), out intRelease) == true && intRelease > 0)
                        lblRelease.Text = DateTime.Parse(oEnhancement.GetVersion(intRelease, "release")).ToLongDateString();
                    else
                        lblRelease.Text = "To Be Determined";
                    lblRequestBy.Text = oUser.GetFullName(intProfile);
                    lblRequestOn.Text = DateTime.Parse(ds.Tables[0].Rows[0]["created"].ToString()).ToString();

                    intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                    oCustomized.UpdateEnhancementNew(intRequest, 0);
                    panMessage.Visible = (oCustomized.GetMessages(intRequest, 0).Tables[0].Rows.Count > 0);
                    strMessages = oCustomized.GetMessages(intRequest, false, "#E1FFE1");
                    if (ds.Tables[0].Rows[0]["status"].ToString() == "2" || ds.Tables[0].Rows[0]["status"].ToString() == "7")
                        btnMessage.Attributes.Add("onclick", "ShowHideDiv2('" + divMessage.ClientID + "');return false;");
                    else
                        btnMessage.Attributes.Add("onclick", "alert('You can only post a message to an ACTIVE thread.\\n\\nSince this thread is no longer active, additional posts have been disabled');return false;");
                }
            }
            else
            {
                panNoEnhancement.Visible = true;
                //panEnhancement.Visible = true;
                //hypCommunity.NavigateUrl = oVariables.Community();
                panUpload.Visible = true;
                lblRequestBy.Text = oUser.GetFullName(intProfile);
                lblRequestOn.Text = DateTime.Now.ToString();
                lblTitle.Text = oPage.Get(intPage, "title");
                txtNumUsers.Text = "0";
                btnSave.Visible = true;
                panActive.Visible = true;
            }

            btnSave.Attributes.Add("onclick", "return ValidateText('" + txtTitle.ClientID + "','Please enter a title')" +
                               " && ValidateText('" + txtDescription.ClientID + "','Please enter a description')" +
                               " && ValidateNumber('" + txtNumUsers.ClientID + "','Please enter a valid number for the number of users')" +
                               ";");
            btnUpdate.Attributes.Add("onclick", "return ValidateText('" + txtTitle.ClientID + "','Please enter a title')" +
                               " && ValidateText('" + txtDescription.ClientID + "','Please enter a description')" +
                               " && ValidateNumber('" + txtNumUsers.ClientID + "','Please enter a valid number for the number of users')" +
                               ";");
            btnResponse.Attributes.Add("onclick", "return ValidateText('" + txtResponse.ClientID + "','Please enter a response');");
            btnDeleteAttachment.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this file?');");
            imgStartDate.Attributes.Add("onclick", "return ShowCalendar('" + txtStartDate.ClientID + "');");
            imgEndDate.Attributes.Add("onclick", "return ShowCalendar('" + txtEndDate.ClientID + "');");
        }

        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            string strVirtualPath = "";
            if (fileUpload.FileName != "" && fileUpload.PostedFile != null)
            {
                string strExtension = fileUpload.FileName;
                string strType = strExtension.Substring(0, 3);
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                string strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + intProfile.ToString() + strExtension;
                strVirtualPath = oVariables.UploadsFolder() + strFile;
                string strPath = oVariables.UploadsFolder() + strFile;
                fileUpload.PostedFile.SaveAs(strPath);
            }
            DateTime datStart = DateTime.Now;
            if (txtStartDate.Text != "")
                datStart = DateTime.Parse(txtStartDate.Text);
            DateTime datEnd = DateTime.Now;
            if (txtEndDate.Text != "")
                datEnd = DateTime.Parse(txtEndDate.Text);
            intRequest = oRequest.AddTask(0, intProfile, txtDescription.Text, datStart, datEnd);
            intItemID = oService.GetItemId(intService);
            int intResource = oServiceRequest.AddRequest(intRequest, intItemID, intService, 0, 0.00, 2, 1, dsnServiceEditor);
            oServiceRequest.Update(intRequest, txtTitle.Text);
            oResourceRequest.UpdateName(intResource, txtTitle.Text);
            oServiceRequest.Add(intRequest, 1, 1);
            oCustomized.AddEnhancement(intRequest, txtTitle.Text, txtDescription.Text, Int32.Parse(drpModules.SelectedValue), Int32.Parse(txtNumUsers.Text == "" ? "0" : txtNumUsers.Text), txtURL.Text, strVirtualPath, datStart, datEnd, intProfile, 0);
            if (oServiceRequest.NotifyApproval(intResource, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                oServiceRequest.NotifyTeamLead(intItemID, intResource, intAssignPage, intViewPage, intEnvironment,  "", dsnServiceEditor, dsnAsset, dsnIP, 0);
            Response.Redirect(oPage.GetFullLinkRelated(intPage));
        }

        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            string strVirtualPath = "";
            if (fileUpload.FileName != "" && fileUpload.PostedFile != null)
            {
                string strExtension = fileUpload.FileName;
                string strType = strExtension.Substring(0, 3);
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                string strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + intProfile.ToString() + strExtension;
                strVirtualPath = oVariables.UploadsFolder() + strFile;
                string strPath = oVariables.UploadsFolder() + strFile;
                fileUpload.PostedFile.SaveAs(strPath);
            }
            else
                strVirtualPath = hypUpload.NavigateUrl;
            oCustomized.UpdateEnhancement(intId, txtTitle.Text, txtDescription.Text, Int32.Parse(drpModules.SelectedValue), Int32.Parse(txtNumUsers.Text), txtURL.Text, strVirtualPath, DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text));
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"]);
        }

        protected void btnDeleteAttachment_Click(Object Sender, EventArgs e)
        {
            string strFile = hypUpload.NavigateUrl;
            strFile = strFile.Substring(strFile.LastIndexOf("/") + 1);
            if (File.Exists(oVariables.UploadsFolder() + strFile) == true)
                File.Delete(oVariables.UploadsFolder() + strFile);
            oCustomized.UpdateEnhancement(intId, "");
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"]);
        }

        protected void btnResponse_Click(Object Sender, EventArgs e)
        {
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            Users oUser = new Users(0, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
            int intNumber = 0;
            int intUser = 0;
            int intResource = 0;
            DataSet dsRequest = oResourceRequest.GetWorkflowRequestAll(intRequest);
            if (dsRequest.Tables[0].Rows.Count > 0)
            {
                intNumber = Int32.Parse(dsRequest.Tables[0].Rows[0]["number"].ToString());
                intUser = Int32.Parse(dsRequest.Tables[0].Rows[0]["userid"].ToString());
                intResource = Int32.Parse(dsRequest.Tables[0].Rows[0]["id"].ToString());
            }
            char chType = (intNumber == 1 ? 'E' : 'S');
            string strVirtualPath = "";
            string strFile = "";
            if (oFile.FileName != "" && oFile.PostedFile != null)
            {
                string strExtension = oFile.FileName;
                string strType = strExtension.Substring(0, 3);
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + intProfile.ToString() + strExtension;
                strVirtualPath = oVariables.UploadsFolder() + strFile;
                string strPath = oVariables.UploadsFolder() + strFile;
                oFile.PostedFile.SaveAs(strPath);
            }
            oCustomized.AddMessage(intRequest, chType, txtResponse.Text, strVirtualPath, intApplication, intProfile, 0, 0);

            StringBuilder sb = new StringBuilder();
            sb.Append("<p><a href=\"");
            sb.Append(oVariables.URL());
            sb.Append("/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=");
            sb.Append(intResource);
            sb.Append("\" target=\"_blank\">Click here to view this ticket or submit a response</a></p>");
            sb.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"3\" border=\"0\" align=\"center\" style=\"border:solid 1px #CCCCCC;");
            sb.Append(oVariables.DefaultFontStyle());
            sb.Append("\">");
            sb.Append("<tr bgcolor=\"#EEEEEE\"><td><span style=\"");
            sb.Append(oVariables.DefaultFontStyleHeader());
            sb.Append("\">");
            sb.Append(oUser.GetFullName(intProfile));
            sb.Append("</span>&nbsp;&nbsp;[");
            sb.Append(DateTime.Now.ToString());
            sb.Append("]:</td></tr>");
            sb.Append("<tr><td>");
            sb.Append(oFunction.FormatText(txtResponse.Text));
            sb.Append("</td></tr>");

            if (strVirtualPath != "")
            {
                sb.Append("<tr><td style=\"border-bottom:dashed 1px #CCCCCC\">&nbsp;</td></tr>");
                sb.Append("<tr><td><img src=\"");
                sb.Append(oVariables.ImageURL());
                sb.Append("/images/file.gif\" align=\"absmiddle\" border=\"0\"/> <a href=\"");
                sb.Append(oVariables.URL());
                sb.Append(strVirtualPath);
                sb.Append("\" target=\"_blank\">");
                sb.Append(strFile);
                sb.Append("</a></td></tr>");
            }

            sb.Append("</table>");
            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
            oFunction.SendEmail("Enhancement Response [CVT" + intRequest.ToString() + "]", oUser.GetName(intUser), "", strEMailIdsBCC, "Enhancement  Response [#CVT" + intRequest.ToString() + "]", "<p>" + oCustomized.GetEnhancementBody(intId, intEnvironment, false) + "</p><p>" + sb.ToString() + "</p>", true, false);
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"]);
        }
    }
}