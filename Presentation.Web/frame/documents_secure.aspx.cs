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
    public partial class documents_secure : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intProfile;
        protected int intProject = 0;
        protected int intRequest = 0;
        protected Documents oDocument;
        protected Requests oRequest;
        private Variables oVariables;

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            oVariables = new Variables(intEnvironment);
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oDocument = new Documents(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            if (Request.QueryString["error"] != null && Request.QueryString["error"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "error", "<script type=\"text/javascript\">alert('Please select a file to upload');<" + "/" + "script>");
            if (Request.QueryString["success"] != null && Request.QueryString["success"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "success", "<script type=\"text/javascript\">alert('File uploaded successfully');<" + "/" + "script>");
            if (Request.QueryString["delete"] != null && Request.QueryString["delete"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "delete", "<script type=\"text/javascript\">alert('File deleted successfully');<" + "/" + "script>");
            if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != "")
                intProject = Int32.Parse(Request.QueryString["pid"]);
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
            {
                intRequest = Int32.Parse(Request.QueryString["rid"]);
                intProject = oRequest.GetProjectNumber(intRequest);
            }
            if (Request.QueryString["PR"] != null)
            {
                radSecurity.SelectedValue = "1";
                radSecurity.Enabled = false;
            }
            if (!IsPostBack)
            {
                if (intProject > 0)
                    rptDocuments.DataSource = oDocument.GetsMine(intProject, intProfile);
                else
                    rptDocuments.DataSource = oDocument.GetsRequest(intRequest, intProfile);
                rptDocuments.DataBind();
                lblNone.Visible = (rptDocuments.Items.Count == 0);
                ParseAttachments(oVariables.UploadsFolder());
            }
        }
        private void ParseAttachments(string _path)
        {
            foreach (RepeaterItem ri in rptDocuments.Items)
            {
                System.Web.UI.WebControls.Image imgAttachment_ = (System.Web.UI.WebControls.Image)ri.FindControl("imgAttachment");
                HyperLink lblName_ = (HyperLink)ri.FindControl("lblName");
                lblName_.NavigateUrl = oVariables.UploadsFolder() + lblName_.NavigateUrl;
                LinkButton btnDelete_ = (LinkButton)ri.FindControl("btnDelete");
                btnDelete_.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this document?');");
                //btnDelete_.CommandArgument = _path + lblName_.Text;
            }
        }
        protected void btnUpload_Click(Object Sender, EventArgs e)
        {
            if (oFile.PostedFile != null && oFile.FileName != "")
            {
                string strExtension = oFile.FileName;
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                string strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + intProfile.ToString() + strExtension;
                oFile.PostedFile.SaveAs(oVariables.UploadsFolder() + strFile);
                oDocument.Add(intProject, intRequest, txtName.Text, strFile, txtDescription.Text, Int32.Parse(radSecurity.SelectedItem.Value), intProfile);
                Response.Redirect(Request.Path + URL() + "&success=true");
            }
            else
                Response.Redirect(Request.Path + URL() + "&error=true");
        }
        protected void btnDeleteLink_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            int intDocument = Int32.Parse(oDelete.CommandArgument);
            oDocument.Delete(intDocument, true, oVariables.DocumentsFolder());
            Response.Redirect(Request.Path + URL() + "&delete=true");
        }
        public string URL()
        {
            string strPR = "";
            if (Request.QueryString["PR"] != null)
                strPR = "&PR=true";
            if (intProject > 0)
                return "?pid=" + intProject.ToString() + strPR;
            else
                return "?rid=" + intRequest.ToString() + strPR;
        }
    }
}
