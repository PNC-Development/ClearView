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
    public partial class project_request_new_dynamic_admin : System.Web.UI.UserControl, ICallbackEventHandler

    {

        private DataSet ds;
        private DataSet dsResp;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected bool boolDirector = (ConfigurationManager.AppSettings["DirectorApproval"] == "1");
        protected int intWorkflowPage = Int32.Parse(ConfigurationManager.AppSettings["WorkflowSuffix"]);
        protected int intResourceRequestPage = Int32.Parse(ConfigurationManager.AppSettings["NewResourceRequest"]);
        protected string strWorkflowBCC = ConfigurationManager.AppSettings["WorkflowBCC"];
        protected int intProfile;
        protected Platforms oPlatform;
        protected Organizations oOrganization;
        protected ProjectRequest oProjectRequest;
        protected Requests oRequest;
        protected Projects oProject;
        protected Functions oFunction;
        protected Variables oVariable;
        protected Applications oApplication;
        protected Pages oPage;
        protected AppPages oAppPage;
        protected ProjectRequest_Approval oApprove;
        protected ResourceRequest oResourceRequest;
        protected RequestItems oRequestItem;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strHTML = "";
        protected string strResult;
        protected string strPath;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            Users oUser = new Users(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oOrganization = new Organizations(intProfile, dsn);
            oProjectRequest = new ProjectRequest(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oVariable = new Variables(intEnvironment);
            oApplication = new Applications(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oAppPage = new AppPages(intProfile, dsn);
            oApprove = new ProjectRequest_Approval(intProfile, dsn, intEnvironment);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);




            ClientScriptManager cm = Page.ClientScript;
            String cbReference = cm.GetCallbackEventReference(this, "arg", "ReceiveServerData", "");
            String callbackScript = "function CallCheck(arg, context) {" + cbReference + "; }";
            cm.RegisterClientScriptBlock(this.GetType(), "CallCheck", callbackScript, true);


            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            lblTitle.Text = "Manage Prioritization";
            if (!IsPostBack)
            {
                ds = oOrganization.Gets(1);
                ddlOrganization.DataValueField = "organizationid";
                ddlOrganization.DataTextField = "name";
                ddlOrganization.DataSource = ds;
                ddlOrganization.DataBind();
                ddlOrganization.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            }


            QuestionOrder();
            ResponseOrder();

            if ((Request.QueryString["bd"] != null && Request.QueryString["bd"] != "") && (Request.QueryString["oid"] != null && Request.QueryString["oid"] != ""))
            {
                panShow.Visible = true;
                ddlBaseDisc.SelectedValue = Request.QueryString["bd"];
                ddlOrganization.SelectedValue = Request.QueryString["oid"];
                LoadQAs(ddlBaseDisc.SelectedItem.Value, Int32.Parse(ddlOrganization.SelectedValue), false);
            }

            if (Request.QueryString["coll"] != "" && Request.QueryString["coll"] != null)
            {
                chkAll.Checked = true;
                LoadQAs(ddlBaseDisc.SelectedItem.Value, Int32.Parse(ddlOrganization.SelectedValue), true);

            }

            btnView.Attributes.Add("onclick", "return ValidateDropDown('" + ddlBaseDisc.ClientID + "','Please make a selection for the project type') " +
                          " && ValidateDropDown('" + ddlOrganization.ClientID + "','Please make a selection for organization');");

            btnCopy.Attributes.Add("onclick", "return OpenWindow('PRIORITIZATION_QA','?bd=" + ddlBaseDisc.SelectedItem.Value + "&oid=" + ddlOrganization.SelectedValue + " ');");
        }
        protected void QuestionOrder()
        {
            if (Request.Form[hdnQOrder.UniqueID] != null && Request.Form[hdnQOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnQOrder.UniqueID];
                string strQid = Request.Form[hdnQuestionID.UniqueID];
                int intOrderDisplay = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("_")));
                int intOrderConfig = Int32.Parse(strOrder.Substring(strOrder.IndexOf("_") + 1));

                int intOldOrder = Int32.Parse(oProjectRequest.GetQuestion(intOrderConfig, "display"));
                if (intOldOrder < intOrderDisplay)
                    intOrderDisplay--;
                int intDisplay = 0;
                bool boolFound = false;
                ds = oProjectRequest.GetQuestions(0);
                int intCopy = 0;
                for (int ii = 1; ii <= ds.Tables[0].Rows.Count; ii++)
                {
                    int intCurrent = Int32.Parse(ds.Tables[0].Rows[ii - 1]["id"].ToString());

                    if (ii == intOrderDisplay && boolFound == false)
                    {
                        intDisplay++;
                        oProjectRequest.UpdateQuestionOrder(intOrderConfig, intDisplay);
                        intDisplay++;
                        oProjectRequest.UpdateQuestionOrder(intCurrent, intDisplay);

                    }
                    else if (intCurrent != intOrderConfig)
                    {
                        intDisplay++;
                        if (intDisplay == intOrderDisplay && boolFound == true)
                        {
                            oProjectRequest.UpdateQuestionOrder(intOrderConfig, intDisplay);
                            intDisplay++;
                        }
                        oProjectRequest.UpdateQuestionOrder(intCurrent, intDisplay);
                    }
                    else
                    {
                        boolFound = true;
                        intCopy = intCurrent;
                    }
                }
                if (boolFound && --intOrderDisplay == intDisplay)
                    oProjectRequest.UpdateResponseOrder(intCopy, ++intDisplay);

                Response.Redirect(Request.UrlReferrer.AbsoluteUri.Replace("&coll=true", ""));
            }
        }
        protected void ResponseOrder()
        {
            if (Request.Form[hdnOrder.UniqueID] != null && Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                string strQid = Request.Form[hdnQuestionID.UniqueID];
                int intOrderDisplay = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("_")));
                int intOrderConfig = Int32.Parse(strOrder.Substring(strOrder.IndexOf("_") + 1));


                int intOldOrder = Int32.Parse(oProjectRequest.GetResponse(intOrderConfig, "display"));
                if (intOldOrder < intOrderDisplay)
                    intOrderDisplay--;
                int intDisplay = 0;
                bool boolFound = false;
                dsResp = oProjectRequest.GetResponses(Int32.Parse(strQid), 0);
                int intCopy = 0;
                for (int ii = 1; ii <= dsResp.Tables[0].Rows.Count; ii++)
                {
                    int intCurrent = Int32.Parse(dsResp.Tables[0].Rows[ii - 1]["id"].ToString());

                    if (ii == intOrderDisplay && boolFound == false)
                    {
                        intDisplay++;
                        oProjectRequest.UpdateResponseOrder(intOrderConfig, intDisplay);
                        intDisplay++;
                        if (intCurrent != intOrderConfig) oProjectRequest.UpdateResponseOrder(intCurrent, intDisplay);

                    }
                    else if (intCurrent != intOrderConfig)
                    {
                        intDisplay++;
                        if (intDisplay == intOrderDisplay && boolFound == true)
                        {
                            oProjectRequest.UpdateResponseOrder(intOrderConfig, intDisplay);
                            intDisplay++;
                        }
                        oProjectRequest.UpdateResponseOrder(intCurrent, intDisplay);
                    }
                    else
                    {
                        boolFound = true;
                        intCopy = intCurrent;
                    }
                }
                if (boolFound && --intOrderDisplay == intDisplay)
                    oProjectRequest.UpdateResponseOrder(intCopy, ++intDisplay);

                Response.Redirect(Request.UrlReferrer.AbsoluteUri.Replace("&coll=true", ""));
            }
        }
        protected void LoadQAs(string bd, int oid, bool boolCollapse)
        {
            ds = oProjectRequest.GetQA(bd, oid);
            int intQuestionRow = 0;
            int intRowDivs = 0;
            int intQACount = ds.Tables[0].Rows.Count;

            StringBuilder sb = new StringBuilder();
            chkAll.Visible = intQACount > 0;
            btnCopy.Visible = intQACount > 0;

            //strHTML += "<div style=\"display:inline\">";
            sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
            sb.Append("<tr><td colspan=\"2\">");
            bool boolOther = false;
            foreach (DataRow drqa in ds.Tables[0].Rows)
            {
                boolOther = !boolOther;
                intQuestionRow++;
                intRowDivs++;

                int intQuestion = Int32.Parse(drqa["questionid"].ToString());
                int intRequired = oProjectRequest.GetQuestion(intQuestion, "required") == "" ? 0 : Int32.Parse(oProjectRequest.GetQuestion(intQuestion, "required"));

                string strDiv = "DIV_" + intRowDivs;
                string strTab = "TAB_" + intRowDivs;

                sb.Append("<div style=\"display:inline\">");
                sb.Append("<table");
                sb.Append(boolOther == true ? " bgcolor=\"#F6F6F6\"" : "");
                sb.Append(" width=\"95%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
                sb.Append("<tr><td colspan=\"2\" class=\"reddefault\" id=\"TDQ_");
                sb.Append(intQuestionRow);
                sb.Append("\" ondrop=\"onDrop2(this,'");
                sb.Append(intQuestionRow.ToString());
                sb.Append("','");
                sb.Append(hdnQOrder.ClientID);
                sb.Append("','");
                sb.Append(drqa["questionid"].ToString());
                sb.Append("','");
                sb.Append(hdnQuestionID.ClientID);
                sb.Append("');\" ondragover=\"overDrag();\" ondragenter=\"enterDrag(this);\" ondragleave=\"leaveDrag(this);\">&nbsp;</td></tr>");
                sb.Append("<tr><td colspan=\"2\"><a ondragstart=\"window.oDrag = '");
                sb.Append(drqa["questionid"]);
                sb.Append("';window.oType='Question';\" style=\"cursor:move\" title=\"Move Question\"><img src=\"/images/move.gif\" border=\"0\"/></a><a href=\"javascript:void(0);\" onclick=\"OpenWindow('PRIORITIZATION_QUESTION','?questionid=");
                sb.Append(intQuestion);
                sb.Append("&bd=");
                sb.Append(drqa["bd"].ToString());
                sb.Append("&oid=");
                sb.Append(drqa["organizationid"].ToString());
                sb.Append("&type=update');\" title=\"Edit Question\"><img src=\"/images/edit.gif\" border=\"0\"/></a> <a href=\"javascript:void(0);\" onclick=\"ShowHideDiv3('");
                sb.Append(strDiv);
                sb.Append("','imgExp");
                sb.Append(intRowDivs);
                sb.Append("');\"><img id='imgExp");
                sb.Append(intRowDivs);
                sb.Append("' src=\"");
                sb.Append(boolCollapse == true ? "/images/expand-icon.gif" : "/images/collapse-icon.gif");
                sb.Append("\" border=\"0\" title=\"");
                sb.Append(boolCollapse == true ? "Expand" : "Collapse");
                sb.Append("\"/></a> ");
                sb.Append(oProjectRequest.GetQuestion(intQuestion, "question"));
                sb.Append(intRequired == 1 ? "<span class=\"required\">&nbsp;*</span>" : "");
                sb.Append("</td></tr>");
                sb.Append("<tr><td valign=\"top\" width=\"5%\"><img src=\"/images/spacer.gif \" width=\"20\" /></td>");
                sb.Append("<td><div id=\"");
                sb.Append(strDiv);
                sb.Append("\" style=\"");
                sb.Append(boolCollapse == true ? "display:none" : "display:inline");
                sb.Append("\" width=\"100%\">");
                sb.Append("<table id=\"");
                sb.Append(strTab);
                sb.Append("\"width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");

                dsResp = oProjectRequest.GetResponses(intQuestion, 0);
                int intResponseRow = 0;
                int intRespCount = dsResp.Tables[0].Rows.Count;

                foreach (DataRow row in dsResp.Tables[0].Rows)
                {
                    intResponseRow++;
                    sb.Append("<tr><td class=\"reddefault\" id=\"TDRESP_");
                    sb.Append(intResponseRow);
                    sb.Append("\" ondrop=\"onDrop2(this,'");
                    sb.Append(intResponseRow.ToString());
                    sb.Append("','");
                    sb.Append(hdnOrder.ClientID);
                    sb.Append("','");
                    sb.Append(row["questionid"].ToString());
                    sb.Append("','");
                    sb.Append(hdnQuestionID.ClientID);
                    sb.Append("');\" ondragover=\"overDrag();\" ondragenter=\"enterDrag(this);\" ondragleave=\"leaveDrag(this);\" >&nbsp;</td></tr>");
                    sb.Append("<tr><td><a ondragstart=\"window.oDrag = '");
                    sb.Append(row["id"]);
                    sb.Append("';window.oType='Response';window.oSrc='");
                    sb.Append(row["questionid"].ToString());
                    sb.Append("';\" style=\"cursor:move\" title=\"Move Response\"><img src=\"/images/move.gif\" border=\"0\"/></a><a href=\"javascript:void(0);\" onclick=\"OpenWindow('PRIORITIZATION_RESPONSE','?questionid=");
                    sb.Append(intQuestion);
                    sb.Append("&pageid=");
                    sb.Append(intPage);
                    sb.Append("&responseid=");
                    sb.Append(row["id"]);
                    sb.Append("&bd=");
                    sb.Append(drqa["bd"]);
                    sb.Append("&oid=");
                    sb.Append(drqa["organizationid"]);
                    sb.Append("&type=update');\" title=\"Edit Response\"><img src=\"/images/edit.gif\" border=\"0\"/></a> ");
                    sb.Append(row["response"].ToString());
                    sb.Append("</td></tr>");

                    if (intResponseRow == intRespCount)
                    {
                        sb.Append("<tr><td class=\"reddefault\" id=\"TDRESP_");
                        sb.Append(++intResponseRow);
                        sb.Append("\" ondrop=\"onDrop2(this,'");
                        sb.Append(intResponseRow.ToString());
                        sb.Append("','");
                        sb.Append(hdnOrder.ClientID);
                        sb.Append("','");
                        sb.Append(row["questionid"].ToString());
                        sb.Append("','");
                        sb.Append(hdnQuestionID.ClientID);
                        sb.Append("','");
                        sb.Append(drqa["bd"]);
                        sb.Append("','");
                        sb.Append(hdnBD.ClientID);
                        sb.Append("','");
                        sb.Append(drqa["organizationid"]);
                        sb.Append("','");
                        sb.Append(hdnOID.ClientID);
                        sb.Append("');\" ondragover=\"overDrag();\" ondragenter=\"enterDrag(this);\" ondragleave=\"leaveDrag(this);\" >&nbsp;</td></tr>");
                    }
                }
                if (intQuestionRow == intQACount)
                {
                    sb.Append("<tr><td class=\"reddefault\" colspan=\"2\" id=\"TDQ_");
                    sb.Append(++intQuestionRow);
                    sb.Append("\" ondrop=\"onDrop2(this,'");
                    sb.Append(intQuestionRow.ToString());
                    sb.Append("','");
                    sb.Append(hdnQOrder.ClientID);
                    sb.Append("','");
                    sb.Append(drqa["questionid"].ToString());
                    sb.Append("','");
                    sb.Append(hdnQuestionID.ClientID);
                    sb.Append("');\" ondragover=\"overDrag();\" ondragenter=\"enterDrag(this);\" ondragleave=\"leaveDrag(this);\">&nbsp;</td></tr>");
                }

                sb.Append("<tr><td colspan=\"2\"><input type=\"button\" class=\"default\" value=\"Add Response\" width=\"100\" onclick=\"return OpenWindow('PRIORITIZATION_RESPONSE','?questionid=");
                sb.Append(drqa["questionid"]);
                sb.Append("&type=add');\" /></td></tr>");
                sb.Append("</table></div></td></tr></table></div>");

            }

            sb.Append("<br /></td></tr>");
            sb.Append("<tr><td><input type=\"button\" class=\"default\" value=\"Add Question\" width=\"100\" onclick=\"return OpenWindow('PRIORITIZATION_QUESTION','?bd=");
            sb.Append(ddlBaseDisc.SelectedItem.Value);
            sb.Append("&oid=");
            sb.Append(ddlOrganization.SelectedValue);
            sb.Append("&type=add');\" /></td></tr>");
            sb.Append("</table>");

            strHTML = sb.ToString();

        }
        protected void btnView_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?bd=" + ddlBaseDisc.SelectedItem.Value + "&oid=" + ddlOrganization.SelectedValue);
        }
        public void RaiseCallbackEvent(String eventArgument)
        {
            strPath = Request.UrlReferrer.AbsoluteUri.Replace("&coll=true", "");
            strResult = eventArgument;
            if (strResult == "true")
                strResult = Request.UrlReferrer.AbsoluteUri + "&coll=true";
            else
                strResult = strPath;
        }
        public string GetCallbackResult()
        {
            return strResult;
        } 
    }
}