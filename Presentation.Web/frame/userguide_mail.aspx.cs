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
    public partial class userguide_mail : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Functions oFunction;
        protected Variables oVariable;
        protected Customized oCustomized;
        protected Pages oPage;

        protected string strAttachement = "";
        protected string strPath = "";
        protected string strName = "";
        protected string strModule = "";
        protected string strBody = "";

        protected int intProfile;
        protected int intId;
        protected string[] strIds;
        protected string strPaths = "";
        private Variables oVariables;

        private string strEMailIdsBCC = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            oVariables = new Variables(intEnvironment);
            StringBuilder sbAttachement = new StringBuilder(strAttachement);
            StringBuilder sbBody = new StringBuilder(strBody);
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oVariable = new Variables(intEnvironment);
            oCustomized = new Customized(intProfile, dsn);

            oPage = new Pages(intProfile, dsn);
            if (Request.QueryString["ids"] != null && Request.QueryString["ids"] != "")
            {
                strIds = Request.QueryString["ids"].Split(':');
            }

            sbAttachement.Append("<tr><td class=\"greentableheader\" colspan=\"2\"><b>User Guide List:</b></td></tr>");
            sbAttachement.Append("<tr><td colspan=\"2\"><table width=\"100%\" cellpadding=\"2\" cellspacing=\"2\" border=\"0\" style=\"border: dashed 1px #CCCCCC;");
            sbAttachement.Append(oVariable.DefaultFontStyle());
            sbAttachement.Append("\">");
            sbAttachement.Append("<tr bgcolor=\"#EEEEEE\" class=\"default\"><td><b>Module</b></td><td><b>User Guide</b></td></tr>");

            sbBody.Append("<table width=\"100%\" cellpadding=\"2\" cellspacing=\"0\" border=\"0\" style=\"");
            sbBody.Append(oVariable.DefaultFontStyle());
            sbBody.Append("\">");
            sbBody.Append(sbAttachement.ToString());

            foreach (string str in strIds)
            {
                if (str != "")
                {
                    intId = Int32.Parse(str);
                    strPath = oCustomized.GetUserGuide(intId, "path");
                    strName = strPath.Replace("/uploads/", "");
                    strModule = oPage.GetName(Int32.Parse(oCustomized.GetUserGuide(intId, "pageid")));
                    sbAttachement.Append("<tr class=\"default\"><td nowrap valign=\"top\">");
                    sbAttachement.Append(strModule);
                    sbAttachement.Append("</td><td nowrap valign=\"top\"><a href=\"");
                    sbAttachement.Append(oVariable.URL());
                    sbAttachement.Append("/");
                    sbAttachement.Append(strPath);
                    sbAttachement.Append("\" target=\"_blank\"><img src=\"/images/icons/pdf.gif \" align=\"absmiddle\" border=\"0\" /> ");
                    sbAttachement.Append(strName);
                    sbAttachement.Append("</a></td></tr> ");
                    sbBody.Append("<tr class=\"default\"><td nowrap valign=\"top\">");
                    sbBody.Append(strModule);
                    sbBody.Append("</td><td nowrap valign=\"top\">");
                    sbBody.Append(strName);
                    sbBody.Append("</td></tr> ");
                    strPaths += oVariable.DocumentsFolder() + strPath + ";";
                }
            }

            sbAttachement.Append("</table>");
            sbBody.Append("<tr><td><b>Comments:</b></td></tr><tr><td colspan=\"2\"><p>");
            sbBody.Append(txtComments.Text);
            sbBody.Append("</p></td></tr></table>");

            strAttachement = sbAttachement.ToString();
            strBody = sbBody.ToString();

            txtTo.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divTo.ClientID + "','" + lstTo.ClientID + "','" + hdnTo.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstTo.Attributes.Add("ondblclick", "AJAXClickRow();");
            btnSend.Attributes.Add("onclick", "return ValidateText('" + hdnValues.ClientID + "','Please enter a recipient!');");
            btnAdd.Attributes.Add("onclick", "return addRecipient('" + txtTo.ClientID + "');");
            btnUpdate.Attributes.Add("onclick", "return updateRecipient('" + txtTo.ClientID + "','hdnPosition');");
        }

        protected void btnSend_Click(Object Sender, EventArgs e)
        {
            hdnValues.Value = hdnValues.Value.ToString().Replace(",", ";");
            strPath = strPath.Replace("/", "\\");
            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
            oFunction.SendEmail("ClearView Help", hdnValues.Value, "", strEMailIdsBCC, "User Guide(s)", strBody, true, false, strPaths);
            ClientScript.RegisterClientScriptBlock(typeof(Page), "redirect", "window.top.navigate(window.top.location);", true);
        }
    }
}
