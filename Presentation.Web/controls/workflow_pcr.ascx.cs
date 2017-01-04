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
    public partial class workflow_pcr : System.Web.UI.UserControl
    {
        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intViewRequest = Int32.Parse(ConfigurationManager.AppSettings["ViewRequest"]);
        protected int intWorkloadManager = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intPCRPage = Int32.Parse(ConfigurationManager.AppSettings["PCR_WORKFLOW"]);
        protected int intProfile;
        protected TPM oTPM;
        protected Projects oProject;
        protected Users oUser;
        protected Pages oPage;
        protected ResourceRequest oResourceRequest;
        protected Variables oVariable;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intId = 0;
        protected string strDetails = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder(strDetails);
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oTPM = new TPM(intProfile, dsn, intEnvironment);
            oProject = new Projects(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intId = Int32.Parse(Request.QueryString["id"]);
                lblId.Text = Request.QueryString["id"];
            }
            if (Request.QueryString["action"] != null && Request.QueryString["action"] != "")
                panFinish.Visible = true;
            else
            {
                if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                    intApplication = Int32.Parse(Request.QueryString["applicationid"]);
                if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                    intPage = Int32.Parse(Request.QueryString["pageid"]);
                if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                    intApplication = Int32.Parse(Request.Cookies["application"].Value);
                if (!IsPostBack)
                {
                    bool boolDeny = true;
                    if (intId > 0)
                    {
                        ds = oTPM.GetPCR(intId, intProfile);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            boolDeny = false;
                            bool boolButtons = false;
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                if (dr["status"].ToString() == "0")
                                {
                                    boolButtons = true;
                                    lblStep.Text = dr["step"].ToString();
                                }
                            }
                            btnApprove.Enabled = boolButtons;
                            btnDeny.Enabled = boolButtons;
                        }
                    }
                    if (boolDeny == false)
                    {
                        panWorkflow.Visible = true;
                        int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                        int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                        int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                        DataSet dsResource = oResourceRequest.Get(intRequest, intItem, intNumber);
                        string strUsers = "";
                        foreach (DataRow drResource in dsResource.Tables[0].Rows)
                        {
                            int intUser = Int32.Parse(drResource["userid"].ToString());
                            if (strUsers != "")
                                strUsers += ", ";
                            strUsers += oUser.GetFullName(intUser) + " (" + oUser.GetName(intUser) + ")";
                        }
                        sb.Append("<tr><td nowrap><b>Submitter:</b></td><td width=\"100%\">");
                        sb.Append(strUsers);
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td nowrap><b>Submitted On:</b></td><td width=\"100%\">");
                        sb.Append(ds.Tables[0].Rows[0]["modified"].ToString());
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td nowrap><b>Project Information:</b></td><td width=\"100%\"><a href=\"");
                        sb.Append(oPage.GetFullLink(intViewRequest));
                        sb.Append("?rid=");
                        sb.Append(intRequest.ToString());
                        sb.Append("\" target=\"_blank\">Click Here to View</a></td></tr>");
                        string strReason = "None";
                        string[] strReasons;
                        char[] strSplit = { ';' };
                        strReasons = ds.Tables[0].Rows[0]["reasons"].ToString().Split(strSplit);
                        for (int ii = 0; ii < strReasons.Length; ii++)
                        {
                            if (strReasons[ii].Trim() != "")
                            {
                                if (strReason != "")
                                    strReason += strReasons[ii];
                                else
                                    strReason += ", " + strReasons[ii];
                            }
                        }
                        sb.Append("<tr><td nowrap><b>Reason(s):</b></td><td width=\"100%\">");
                        sb.Append(strReason);
                        sb.Append("</td></tr>");
                        lblDetailId.Text = ds.Tables[0].Rows[0]["detailid"].ToString();
                        if (ds.Tables[0].Rows[0]["scope"].ToString() != "0")
                        {
                            sb.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade/></td></tr>");
                            sb.Append("<tr><td colspan=\"2\" class=\"header\"><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"> Scope Change</td></tr>");
                        }
                        if (ds.Tables[0].Rows[0]["s"].ToString() != "0")
                        {
                            sb.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade/></td></tr>");
                            sb.Append("<tr><td colspan=\"2\" class=\"header\"><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"> Schedule Change</td></tr>");
                            if (ds.Tables[0].Rows[0]["sds"].ToString() != "")
                            {
                                sb.Append("<tr><td nowrap><b>Discovery Start Date:</b></td><td width=\"100%\">");
                                sb.Append(DateTime.Parse(ds.Tables[0].Rows[0]["sds"].ToString()).ToLongDateString());
                                sb.Append("</td></tr>");
                                sb.Append("<tr><td nowrap><b>Discovery End Date:</b></td><td width=\"100%\">");
                                sb.Append(DateTime.Parse(ds.Tables[0].Rows[0]["sde"].ToString()).ToLongDateString());
                                sb.Append("</td></tr>");
                            }
                            if (ds.Tables[0].Rows[0]["sps"].ToString() != "")
                            {
                                sb.Append("<tr><td nowrap><b>Planning Start Date:</b></td><td width=\"100%\">");
                                sb.Append(DateTime.Parse(ds.Tables[0].Rows[0]["sps"].ToString()).ToLongDateString());
                                sb.Append("</td></tr>");
                                sb.Append("<tr><td nowrap><b>Planning End Date:</b></td><td width=\"100%\">");
                                sb.Append(DateTime.Parse(ds.Tables[0].Rows[0]["spe"].ToString()).ToLongDateString());
                                sb.Append("</td></tr>");
                            }
                            if (ds.Tables[0].Rows[0]["ses"].ToString() != "")
                            {
                                sb.Append("<tr><td nowrap><b>Execution Start Date:</b></td><td width=\"100%\">");
                                sb.Append(DateTime.Parse(ds.Tables[0].Rows[0]["ses"].ToString()).ToLongDateString());
                                sb.Append("</td></tr>");
                                sb.Append("<tr><td nowrap><b>Execution End Date:</b></td><td width=\"100%\">");
                                sb.Append(DateTime.Parse(ds.Tables[0].Rows[0]["see"].ToString()).ToLongDateString());
                                sb.Append("</td></tr>");
                            }
                            if (ds.Tables[0].Rows[0]["scs"].ToString() != "")
                            {
                                sb.Append("<tr><td nowrap><b>Closing Start Date:</b></td><td width=\"100%\">");
                                sb.Append(DateTime.Parse(ds.Tables[0].Rows[0]["scs"].ToString()).ToLongDateString());
                                sb.Append("</td></tr>");
                                sb.Append("<tr><td nowrap><b>Closing End Date:</b></td><td width=\"100%\">");
                                sb.Append(DateTime.Parse(ds.Tables[0].Rows[0]["sce"].ToString()).ToLongDateString());
                                sb.Append("</td></tr>");
                            }
                        }
                        if (ds.Tables[0].Rows[0]["f"].ToString() != "0")
                        {
                            sb.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade/></td></tr>");
                            sb.Append("<tr><td colspan=\"2\" class=\"header\"><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"> Financial Change</td></tr>");

                            if (ds.Tables[0].Rows[0]["fd"].ToString() != "0")
                            {
                                sb.Append("<tr><td nowrap><b>Discovery:</b></td><td width=\"100%\">$");
                                sb.Append(double.Parse(ds.Tables[0].Rows[0]["fd"].ToString()).ToString("F"));
                                sb.Append("</td></tr>");
                            }

                            if (ds.Tables[0].Rows[0]["fp"].ToString() != "0")
                            {
                                sb.Append("<tr><td nowrap><b>Planning:</b></td><td width=\"100%\">$");
                                sb.Append(double.Parse(ds.Tables[0].Rows[0]["fp"].ToString()).ToString("F"));
                                sb.Append("</td></tr>");
                            }

                            if (ds.Tables[0].Rows[0]["fe"].ToString() != "0")
                            {
                                sb.Append("<tr><td nowrap><b>Execution:</b></td><td width=\"100%\">$");
                                sb.Append(double.Parse(ds.Tables[0].Rows[0]["fe"].ToString()).ToString("F"));
                                sb.Append("</td></tr>");
                            }

                            if (ds.Tables[0].Rows[0]["fc"].ToString() != "0")
                            {
                                sb.Append("<tr><td nowrap><b>Closing:</b></td><td width=\"100%\">$");
                                sb.Append(double.Parse(ds.Tables[0].Rows[0]["fc"].ToString()).ToString("F"));
                                sb.Append("</td></tr>");
                            }
                        }
                        sb.Insert(0, "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"3\" border=\"0\">");
                        sb.Append("</table>");
                    }
                    else
                    {
                        panDenied.Visible = true;
                    }
                }
            }
            strDetails = sb.ToString();
            btnClose.Attributes.Add("onclick", "return CloseWindow();");
            btnFinish.Attributes.Add("onclick", "return CloseWindow();");
            btnApprove.Attributes.Add("onclick", "return confirm('Are you sure you want to APPROVE this request?');");
            btnDeny.Attributes.Add("onclick", "return ValidateText('" + txtComments.ClientID + "','Please enter some comments') && confirm('Are you sure you want to DENY this request?');");
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            Button oButton = (Button)Sender;
            int intStep = Int32.Parse(lblStep.Text);
            int intDetailId = Int32.Parse(lblDetailId.Text);
            oTPM.UpdatePCRDetail(intDetailId, Int32.Parse(oButton.CommandArgument), txtComments.Text);
            //oTPM.ApprovePCR(intId, intStep + 1, Int32.Parse(oButton.CommandArgument), intPCRPage, intWorkloadManager, strBCC);
            oTPM.ApprovePCR(intId, intStep + 1, Int32.Parse(oButton.CommandArgument), intPCRPage, intWorkloadManager,  Request.PhysicalApplicationPath + oTPM.GetPCRPath(intId));
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + lblId.Text + "&action=done");
        }
    }
}