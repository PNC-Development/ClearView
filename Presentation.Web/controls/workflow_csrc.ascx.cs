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
    public partial class workflow_csrc : System.Web.UI.UserControl
    {
        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intViewRequest = Int32.Parse(ConfigurationManager.AppSettings["ViewRequest"]);
        protected int intWorkloadManager = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intCSRCPage = Int32.Parse(ConfigurationManager.AppSettings["CSRC_WORKFLOW"]);
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
                        ds = oTPM.GetCSRC(intId, intProfile);
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
                        sb.Append("<tr><td nowrap><b>CSRC Document:</b></td><td width=\"100%\"><a href=\"");
                        sb.Append(oVariable.URL());
                        sb.Append("/");
                        sb.Append(ds.Tables[0].Rows[0]["path"].ToString().Replace("\\", "/"));
                        sb.Append("\" target=\"_blank\">Click Here to View</a></td></tr>");
                        sb.Append("<tr><td nowrap><b>Project Information:</b></td><td width=\"100%\"><a href=\"");
                        sb.Append(oPage.GetFullLink(intViewRequest));
                        sb.Append("?rid=");
                        sb.Append(intRequest.ToString());
                        sb.Append("\" target=\"_blank\">Click Here to View</a></td></tr>");

                        lblDetailId.Text = ds.Tables[0].Rows[0]["detailid"].ToString();

                        if (ds.Tables[0].Rows[0]["ds"].ToString() != "")
                        {
                            sb.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade/></td></tr>");
                            sb.Append("<tr><td nowrap><b>Discovery Phase Start Date:</b></td><td width=\"100%\">");
                            sb.Append(DateTime.Parse(ds.Tables[0].Rows[0]["ds"].ToString()).ToLongDateString());
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap><b>Discovery Phase End Date:</b></td><td width=\"100%\">");
                            sb.Append(DateTime.Parse(ds.Tables[0].Rows[0]["de"].ToString()).ToLongDateString());
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap><b>Discovery Internal Labor:</b></td><td width=\"100%\">$");
                            sb.Append(double.Parse(ds.Tables[0].Rows[0]["di"].ToString()).ToString("F"));
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap><b>Discovery External Labor:</b></td><td width=\"100%\">$");
                            sb.Append(double.Parse(ds.Tables[0].Rows[0]["dex"].ToString()).ToString("F"));
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap><b>Discovery HW/SW/One Time Cost:</b></td><td width=\"100%\">$");
                            sb.Append(double.Parse(ds.Tables[0].Rows[0]["dh"].ToString()).ToString("F"));
                            sb.Append("</td></tr>");
                        }
                        if (ds.Tables[0].Rows[0]["ps"].ToString() != "")
                        {
                            sb.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade/></td></tr>");
                            sb.Append("<tr><td nowrap><b>Planning Phase Start Date:</b></td><td width=\"100%\">");
                            sb.Append(DateTime.Parse(ds.Tables[0].Rows[0]["ps"].ToString()).ToLongDateString());
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap><b>Planning Phase End Date:</b></td><td width=\"100%\">");
                            sb.Append(DateTime.Parse(ds.Tables[0].Rows[0]["pe"].ToString()).ToLongDateString());
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap><b>Planning Internal Labor:</b></td><td width=\"100%\">$");
                            sb.Append(double.Parse(ds.Tables[0].Rows[0]["pi"].ToString()).ToString("F"));
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap><b>Planning External Labor:</b></td><td width=\"100%\">$");
                            sb.Append(double.Parse(ds.Tables[0].Rows[0]["pex"].ToString()).ToString("F"));
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap><b>Planning HW/SW/One Time Cost:</b></td><td width=\"100%\">$");
                            sb.Append(double.Parse(ds.Tables[0].Rows[0]["ph"].ToString()).ToString("F"));
                            sb.Append("</td></tr>");
                        }
                        if (ds.Tables[0].Rows[0]["es"].ToString() != "")
                        {
                            sb.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade/></td></tr>");
                            sb.Append("<tr><td nowrap><b>Execution Phase Start Date:</b></td><td width=\"100%\">");
                            sb.Append(DateTime.Parse(ds.Tables[0].Rows[0]["es"].ToString()).ToLongDateString());
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap><b>Execution Phase End Date:</b></td><td width=\"100%\">");
                            sb.Append(DateTime.Parse(ds.Tables[0].Rows[0]["ee"].ToString()).ToLongDateString());
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap><b>Execution Internal Labor:</b></td><td width=\"100%\">$");
                            sb.Append(double.Parse(ds.Tables[0].Rows[0]["ei"].ToString()).ToString("F"));
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap><b>Execution External Labor:</b></td><td width=\"100%\">$");
                            sb.Append(double.Parse(ds.Tables[0].Rows[0]["eex"].ToString()).ToString("F"));
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap><b>Execution HW/SW/One Time Cost:</b></td><td width=\"100%\">$");
                            sb.Append(double.Parse(ds.Tables[0].Rows[0]["eh"].ToString()).ToString("F"));
                            sb.Append("</td></tr>");
                        }
                        if (ds.Tables[0].Rows[0]["cs"].ToString() != "")
                        {
                            sb.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade/></td></tr>");
                            sb.Append("<tr><td nowrap><b>Closing Phase Start Date:</b></td><td width=\"100%\">");
                            sb.Append(DateTime.Parse(ds.Tables[0].Rows[0]["cs"].ToString()).ToLongDateString());
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap><b>Closing Phase End Date:</b></td><td width=\"100%\">");
                            sb.Append(DateTime.Parse(ds.Tables[0].Rows[0]["ce"].ToString()).ToLongDateString());
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap><b>Closing Internal Labor:</b></td><td width=\"100%\">$");
                            sb.Append(double.Parse(ds.Tables[0].Rows[0]["ci"].ToString()).ToString("F"));
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap><b>Closing External Labor:</b></td><td width=\"100%\">$");
                            sb.Append(double.Parse(ds.Tables[0].Rows[0]["cex"].ToString()).ToString("F"));
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap><b>Closing HW/SW/One Time Cost:</b></td><td width=\"100%\">$");
                            sb.Append(double.Parse(ds.Tables[0].Rows[0]["ch"].ToString()).ToString("F"));
                            sb.Append("</td></tr>");
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
            oTPM.UpdateCSRCDetail(intDetailId, Int32.Parse(oButton.CommandArgument), txtComments.Text);
            //oTPM.ApproveCSRC(intId, intStep + 1, Int32.Parse(oButton.CommandArgument), intCSRCPage, intWorkloadManager, strBCC);
            oTPM.ApproveCSRC(intId, intStep + 1, Int32.Parse(oButton.CommandArgument), intCSRCPage, intWorkloadManager, Request.PhysicalApplicationPath + oTPM.GetCSRCPath(intId));
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + lblId.Text + "&action=done");
        }
    }
}