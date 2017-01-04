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
using NCC.ClearView.Presentation.Web.Custom;

namespace NCC.ClearView.Presentation.Web
{
    public partial class enhancement : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intService = Int32.Parse(ConfigurationManager.AppSettings["HELP_ENHANCEMENT_SERVICEID"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
       
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);

        protected Requests oRequest;
        protected Services oService;
        protected ServiceRequests oServiceRequest;
        protected Enhancements oEnhancement;
        protected Pages oPage;
        protected ResourceRequest oResourceRequest;
        protected StatusLevels oStatusLevel;
        protected Users oUser;
        protected Customized oCustomized;
        protected Functions oFunction;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProfile;
        protected int intRequest = 0;
        protected int intID;
        protected int intIDold;
        protected string strMessages = "";
        private Variables oVariable;
        protected string strMenuTab1 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oRequest = new Requests(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oEnhancement = new Enhancements(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oStatusLevel = new StatusLevels();
            oUser = new Users(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oVariable = new Variables(intEnvironment);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["old"] != null && Request.QueryString["old"] != "")
                intIDold = Int32.Parse(Request.QueryString["old"]);

            if (!IsPostBack)
                LoadLists();

            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
            Tab oTab = new Tab("", intMenuTab, "divMenu1", true, false);
            oTab.AddTab("Enhancement Details", "");
            if (intID > 0)
            {
                panEnhancement.Visible = true;
                DataSet ds = oEnhancement.Get(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    panNew.Visible = true;
                    trStatus.Visible = true;
                    lblTitle.Text = "Edit Enhancement";
                    DataSet dsMessages = oEnhancement.GetMessages(intID);
                    if (!IsPostBack)
                    {
                        txtTitle.Text = ds.Tables[0].Rows[0]["title"].ToString();
                        txtDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                        ddlUsers.SelectedValue = ds.Tables[0].Rows[0]["users"].ToString();
                        txtURL.Text = ds.Tables[0].Rows[0]["url"].ToString();
                        if (ds.Tables[0].Rows[0]["url"].ToString() != "")
                        {
                            hypURL.Visible = true;
                            hypURL.NavigateUrl = ds.Tables[0].Rows[0]["url"].ToString();
                        }
                        if (ds.Tables[0].Rows[0]["screenshot"].ToString() != "")
                        {
                            panUploaded.Visible = true;
                            hypUpload.NavigateUrl = ds.Tables[0].Rows[0]["screenshot"].ToString();
                        }
                        else
                        {
                            panUpload.Visible = true;
                            hypUpload.Visible = false;
                        }
                        radRelease1.SelectedValue = ds.Tables[0].Rows[0]["release1"].ToString();
                        radRelease2.SelectedValue = ds.Tables[0].Rows[0]["release2"].ToString();
                        int intStatus = Int32.Parse(ds.Tables[0].Rows[0]["status"].ToString());
                        lblStatus.Text = oEnhancement.Status(intID);
                        int intRR = Int32.Parse(ds.Tables[0].Rows[0]["rrid"].ToString());
                        //foreach (DataRow drStep in dsSteps.Tables[0].Rows)
                        //{
                        //    if (drStep["requires_approval"].ToString() == "1" && drStep["approved"].ToString() == "")
                        //        boolApproved = false;
                        //    if (drStep["step"].ToString() == "1")
                        //        boolFunctionalReq = true;
                        //}

                        if (intStatus != (int)EnhancementStatus.Cancelled && intStatus != (int)EnhancementStatus.Completed && intStatus != (int)EnhancementStatus.Denied && intStatus != (int)EnhancementStatus.Duplicate)
                        {
                            trSave1.Visible = true;
                            trSave2.Visible = true;
                            btnCancel.Visible = true;
                            btnUpdate.Visible = true;
                            btnCancel.CommandArgument = intRR.ToString();
                            if (intStatus == (int)EnhancementStatus.UnderReview)
                            {
                                // Request is active but not yet assigned, so nobody is working on it yet.
                            }
                            else
                            {
                                btnUpdate.Enabled = false;
                                trLocked.Visible = true;
                                btnDeleteAttachment.Enabled = false;
                                oTab.AddTab("Functional Requirements", "");
                                DataSet dsSteps = oEnhancement.GetSteps(intID, 1, 0);
                                if (dsSteps.Tables[0].Rows.Count == 1 && dsSteps.Tables[0].Rows[0]["completed"].ToString() != "")
                                {
                                    DataSet dsDocuments = oEnhancement.GetDocuments(intID);
                                    hypFunctionalRequirements.NavigateUrl = dsDocuments.Tables[0].Rows[0]["path"].ToString();
                                    lblDays.Text = dsDocuments.Tables[0].Rows[0]["days"].ToString();
                                    lblRelease.Text = DateTime.Parse(dsDocuments.Tables[0].Rows[0]["release"].ToString()).ToShortDateString();

                                    if (dsSteps.Tables[0].Rows[0]["approved"].ToString() == "" && dsSteps.Tables[0].Rows[0]["rejected"].ToString() == "")
                                    {
                                        trFunctionalRequirementsDocument.Visible = true;
                                        btnFunctionalApproving.Enabled = true;
                                        btnFunctionalRejecting.Enabled = true;
                                        btnFunctionalRejecting.Attributes.Add("onclick", "ShowHideDiv('" + trFunctionalReject.ClientID + "','inline');ShowHideDiv('" + trFunctionalButtons.ClientID + "','none');return false;");
                                        btnFunctionalCancelReject.Attributes.Add("onclick", "ShowHideDiv('" + trFunctionalReject.ClientID + "','none');ShowHideDiv('" + trFunctionalButtons.ClientID + "','inline');return false;");
                                        btnFunctionalApproving.Attributes.Add("onclick", "ShowHideDiv('" + trFunctionalApprove.ClientID + "','inline');ShowHideDiv('" + trFunctionalButtons.ClientID + "','none');return false;");
                                        btnFunctionalCancelApprove.Attributes.Add("onclick", "ShowHideDiv('" + trFunctionalApprove.ClientID + "','none');ShowHideDiv('" + trFunctionalButtons.ClientID + "','inline');return false;");
                                        btnFunctionalReject.Attributes.Add("onclick", "return ValidateText('" + txtFunctionalReject.ClientID + "','Specify why you are rejecting the functional requirements') && confirm('Rejecting the functional requirements document will cause delays in this enhancement being released.\\n\\nAre you sure you want to reject the functional requirements document?') && ProcessButton(this) && LoadWait();");
                                        btnFunctionalApprove.Attributes.Add("onclick", "return ValidateCheck('" + chkApprove.ClientID + "','You must accept the acknowledgement to approve the functional requirements') && ProcessButton(this) && LoadWait();");
                                    }
                                    else
                                    {
                                        trFunctionalButtons.Visible = false;
                                        trFunctionalStatus.Visible = true;
                                        if (dsSteps.Tables[0].Rows[0]["approved"].ToString() != "")
                                        {
                                            imgFunctionalStatus.ImageUrl = "/images/bigCheckBox.gif";
                                            lblFunctionalStatus.Text = "Approved on " + dsSteps.Tables[0].Rows[0]["approved"].ToString();
                                        }
                                        if (dsSteps.Tables[0].Rows[0]["rejected"].ToString() != "")
                                        {
                                            imgFunctionalStatus.ImageUrl = "/images/bigError2.gif";
                                            lblFunctionalStatus.Text = "Rejected on " + dsSteps.Tables[0].Rows[0]["rejected"].ToString();
                                        }
                                    }
                                }
                                oTab.AddTab("Message Thread (" + dsMessages.Tables[0].Rows.Count.ToString() + ")", "");
                                oTab.AddTab("Log (" + oEnhancement.LoadLog(intID, rptLog, lblLog) + ")", "");
                            }
                            btnMessageReplyImage.Attributes.Add("onclick", "ShowHideDiv2('" + divMessageReply.ClientID + "');return false;");
                        }
                        else
                        {
                            btnDeleteAttachment.Visible = false;
                            btnMessageReplyImage.Attributes.Add("onclick", "alert('You can only post a message to an ACTIVE thread.\\n\\nSince this thread is no longer active, additional posts have been disabled');return false;");
                            btnMessageReply.Enabled = false;
                            if (ds.Tables[0].Rows[0]["reason"].ToString() != "")
                            {
                                trComments.Visible = true;
                                lblComments.Text = ds.Tables[0].Rows[0]["reason"].ToString();
                            }
                        }
                        lblRequestBy.Text = oUser.GetFullName(intProfile);
                        lblRequestOn.Text = DateTime.Parse(ds.Tables[0].Rows[0]["created"].ToString()).ToString();
                    }
                    strMessages = oEnhancement.GetMessages(intID, false, "#E1FFE1");
                }
            }
            else if (intIDold > 0)
            {
                // OLD ENHANCEMENT (Read Only)
                panEnhancement.Visible = true;
                DataSet ds = oCustomized.GetEnhancementByID(intIDold);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    panOld.Visible = true;
                    lblTitle.Text = "Edit Enhancement";
                    lblOldTitle.Text = ds.Tables[0].Rows[0]["title"].ToString();
                    lblOldDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                    lblOldModule.Text = oPage.Get(Int32.Parse(ds.Tables[0].Rows[0]["pageid"].ToString()), "title");
                    lblOldNumUsers.Text = ds.Tables[0].Rows[0]["num_users"].ToString();
                    if (ds.Tables[0].Rows[0]["url"].ToString() != "")
                        hypOldURL.NavigateUrl = ds.Tables[0].Rows[0]["url"].ToString();
                    else
                        hypOldURL.Visible = false;
                    if (ds.Tables[0].Rows[0]["path"].ToString() != "")
                        hypOldAttach.NavigateUrl = ds.Tables[0].Rows[0]["path"].ToString();
                    else
                        hypOldAttach.Visible = false;
                    lblOldStart.Text = DateTime.Parse(ds.Tables[0].Rows[0]["startdate"].ToString()).ToShortDateString();
                    lblOldEnd.Text = DateTime.Parse(ds.Tables[0].Rows[0]["enddate"].ToString()).ToShortDateString();
                    lblOldStatus.Text = oStatusLevel.HTMLSupport(Int32.Parse(ds.Tables[0].Rows[0]["status"].ToString()));
                    int intRelease = 0;
                    if (Int32.TryParse(ds.Tables[0].Rows[0]["release"].ToString(), out intRelease) == true && intRelease > 0)
                        lblOldRelease.Text = DateTime.Parse(oEnhancement.GetVersion(intRelease, "release")).ToLongDateString();
                    else
                        lblOldRelease.Text = "To Be Determined";
                    lblOldRequestedBy.Text = oUser.GetFullName(intProfile);
                    lblOldRequestedOn.Text = DateTime.Parse(ds.Tables[0].Rows[0]["created"].ToString()).ToString();

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
                //panNoEnhancement.Visible = true;
                panEnhancement.Visible = true;
                hypCommunity.NavigateUrl = oVariable.Community();
                panUpload.Visible = true;
                lblRequestBy.Text = oUser.GetFullName(intProfile);
                lblRequestOn.Text = DateTime.Now.ToString();
                lblTitle.Text = oPage.Get(intPage, "title");
                btnSave.Visible = true;
                trSave1.Visible = true;
                trSave2.Visible = true;
                panNew.Visible = true;
            }
            strMenuTab1 = oTab.GetTabs();

            btnSave.Attributes.Add("onclick", "return ValidateText('" + txtTitle.ClientID + "','Please enter a title')" +
                               " && ValidateText('" + txtDescription.ClientID + "','Please enter a description')" +
                               " && ValidateDropDown('" + ddlUsers.ClientID + "','Please make a selection for the number of users benefited')" +
                               " && ValidateRadioList('" + radRelease1.ClientID + "','Please make a selection for your preferred release date')" +
                               " && ValidateRadioList('" + radRelease2.ClientID + "','Please make a selection for your alternative release date')" +
                               " && ProcessButton(this) && LoadWait()" +
                               ";");
            btnUpdate.Attributes.Add("onclick", "return ValidateText('" + txtTitle.ClientID + "','Please enter a title')" +
                               " && ValidateText('" + txtDescription.ClientID + "','Please enter a description')" +
                               " && ValidateDropDown('" + ddlUsers.ClientID + "','Please make a selection for the number of users benefited')" +
                               " && ValidateRadioList('" + radRelease1.ClientID + "','Please make a selection for your preferred release date')" +
                               " && ValidateRadioList('" + radRelease2.ClientID + "','Please make a selection for your alternative release date')" +
                               " && ProcessButton(this) && LoadWait()" +
                               ";");
            btnCancel.Attributes.Add("onclick", "return confirm('WARNING: Any changes related to this enhancement and the ClearView system will be removed.\\n\\nAre you sure you want to cancel this enhancement?') && ProcessButton(this) && LoadWait();");
            btnResponse.Attributes.Add("onclick", "return ValidateText('" + txtResponse.ClientID + "','Please enter a response') && ProcessButton(this) && LoadWait();;");
            btnMessageReply.Attributes.Add("onclick", "return ValidateText('" + txtMessageReply.ClientID + "','Please enter a response') && ProcessButton(this) && LoadWait();;");
            btnDeleteAttachment.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this file?') && ProcessButton(this) && LoadWait();;");
        }

        protected void LoadLists()
        {
            oEnhancement.AddVersions(radRelease1);
            foreach (ListItem radRelease in radRelease1.Items)
                radRelease.Attributes.Add("onclick", "DisableOtherRadioList(this,'" + radRelease2.ClientID + "',true);");
            oEnhancement.AddVersions(radRelease2);
            foreach (ListItem radRelease in radRelease2.Items)
                radRelease.Attributes.Add("onclick", "DisableOtherRadioList(this,'" + radRelease1.ClientID + "',false);");
        }

        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            intRequest = oRequest.Add(0, intProfile);
            int intItemID = oService.GetItemId(intService);
            int intResource = oServiceRequest.AddRequest(intRequest, intItemID, intService, 0, 0.00, (int)EnhancementStatus.UnderReview, 1, dsnServiceEditor);
            oServiceRequest.Update(intRequest, txtTitle.Text);
            oResourceRequest.UpdateName(intResource, txtTitle.Text);
            oServiceRequest.Add(intRequest, 1, 1);
            intID = oEnhancement.Add(txtTitle.Text, txtDescription.Text, Int32.Parse(ddlUsers.SelectedItem.Value), txtURL.Text, fileUpload, intEnvironment, Int32.Parse(radRelease1.SelectedItem.Value), Int32.Parse(radRelease2.SelectedItem.Value), intProfile, intResource);
            if (oServiceRequest.NotifyApproval(intResource, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                oServiceRequest.NotifyTeamLead(intItemID, intResource, intAssignPage, intViewPage, intEnvironment,  "", dsnServiceEditor, dsnAsset, dsnIP, 0);
            oEnhancement.AddLog(intID, 0, "Submitted Original Enhancement", intProfile, "");
            Response.Redirect(oPage.GetFullLinkRelated(intPage));
        }

        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            oEnhancement.Update(intID, txtTitle.Text, txtDescription.Text, Int32.Parse(ddlUsers.SelectedItem.Value), txtURL.Text, hypUpload.NavigateUrl, fileUpload, intEnvironment, Int32.Parse(radRelease1.SelectedItem.Value), Int32.Parse(radRelease2.SelectedItem.Value), intProfile);
            oEnhancement.AddLog(intID, 0, "Modified Enhancement", intProfile, "");
            Redirect("");
        }

        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            oResourceRequest.UpdateStatusOverall(Int32.Parse(btnCancel.CommandArgument), (int)ResourceRequestStatus.Cancelled);
            oEnhancement.AddLog(intID, 0, "Cancelled Enhancement", intProfile, "");
            Redirect("");
        }

        protected void btnDeleteAttachment_Click(Object Sender, EventArgs e)
        {
            string strFile = hypUpload.NavigateUrl;
            strFile = strFile.Substring(strFile.LastIndexOf("/") + 1);
            if (File.Exists(strFile) == true)
                File.Delete(strFile);
            oEnhancement.DeleteScreenshot(intID);
            Redirect("");
        }

        protected void btnMessageReply_Click(Object Sender, EventArgs e)
        {
            Reply(filMessageReply, txtMessageReply);
        }
        protected void btnResponse_Click(Object Sender, EventArgs e)
        {
            Reply(oFile, txtResponse);
        }
        private void Reply(FileUpload _file, TextBox _text) 
        {
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
            if (_file.FileName != "" && _file.PostedFile != null)
            {
                string strExtension = _file.FileName;
                string strType = strExtension.Substring(0, 3);
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + intProfile.ToString() + strExtension;
                strVirtualPath = oVariable.UploadsFolder() + strFile;
                string strPath = oVariable.UploadsFolder() + strFile;
                _file.PostedFile.SaveAs(strPath);
            }
            if (intID > 0)
                oEnhancement.AddMessage(intID, _text.Text, strVirtualPath, intProfile, 0);
            else
                oCustomized.AddMessage(intRequest, chType, _text.Text, strVirtualPath, intApplication, intProfile, 0, 0);

            StringBuilder sb = new StringBuilder();
            sb.Append("<p><a href=\"");
            sb.Append(oVariable.URL());
            sb.Append("/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=");
            sb.Append(intResource);
            sb.Append("\" target=\"_blank\">Click here to view this ticket or submit a response</a></p>");
            sb.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"3\" border=\"0\" align=\"center\" style=\"border:solid 1px #CCCCCC;");
            sb.Append(oVariable.DefaultFontStyle());
            sb.Append("\">");
            sb.Append("<tr bgcolor=\"#EEEEEE\"><td><span style=\"");
            sb.Append(oVariable.DefaultFontStyleHeader());
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
                sb.Append(oVariable.ImageURL());
                sb.Append("/images/file.gif\" align=\"absmiddle\" border=\"0\"/> <a href=\"");
                sb.Append(oVariable.URL());
                sb.Append(strVirtualPath);
                sb.Append("\" target=\"_blank\">");
                sb.Append(strFile);
                sb.Append("</a></td></tr>");
            }

            sb.Append("</table>");
            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
            oFunction.SendEmail("Enhancement Response [CVT" + intRequest.ToString() + "]", oUser.GetName(intUser), "", strEMailIdsBCC, "Enhancement  Response [#CVT" + intRequest.ToString() + "]", "<p>" + oCustomized.GetEnhancementBody(intIDold, intEnvironment, false) + "</p><p>" + sb.ToString() + "</p>", true, false);
            Redirect("");
        }
        protected void btnFunctionalApprove_Click(Object Sender, EventArgs e)
        {
            oEnhancement.UpdateLog(intID, 1, DateTime.Now.ToString(), "", "");
            Redirect("&menu_tab=2");
        }
        protected void btnFunctionalReject_Click(Object Sender, EventArgs e)
        {
            oEnhancement.UpdateLog(intID, 1, "", DateTime.Now.ToString(), txtFunctionalReject.Text);
            Redirect("&menu_tab=2");
        }
        private void Redirect(string _additional)
        {
            if (Request.QueryString["id"] != null)
                Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + _additional);
            if (Request.QueryString["old"] != null)
                Response.Redirect(oPage.GetFullLink(intPage) + "?old=" + Request.QueryString["old"] + _additional);
        }
    }
}