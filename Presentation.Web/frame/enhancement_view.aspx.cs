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
    public partial class enhancement_view : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intService = Int32.Parse(ConfigurationManager.AppSettings["HELP_SERVICEID"]);
        protected Customized oCustomized;
        protected ServiceRequests oServiceRequest;
        protected Requests oRequest;
        protected Pages oPage;
        protected int intProfile;
        protected DataSet ds;
        private Variables oVariables;

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oCustomized = new Customized(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
                intRequest = Int32.Parse(Request.QueryString["rid"]);

            ds = oCustomized.GetEnhancement(intRequest);

            if (!IsPostBack)
            {
                drpModules.DataSource = oPage.Gets(0, 1);
                drpModules.DataTextField = "title";
                drpModules.DataValueField = "pageid";
                drpModules.DataBind();
                drpModules.SelectedValue = ds.Tables[0].Rows[0]["pageid"].ToString();

                txtTitle.Text = ds.Tables[0].Rows[0]["title"].ToString();
                txtDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                txtNumUsers.Text = ds.Tables[0].Rows[0]["num_users"].ToString();
                txtURL.Text = ds.Tables[0].Rows[0]["url"].ToString();
                lblPath.Text = ds.Tables[0].Rows[0]["path"].ToString();
                txtStartDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["startdate"].ToString()).ToShortDateString();
                txtEndDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["enddate"].ToString()).ToShortDateString();
            }

            imgStartDate.Attributes.Add("onclick", "return OpenCalendar('" + txtStartDate.ClientID + "');");
            imgEndDate.Attributes.Add("onclick", "return OpenCalendar('" + txtEndDate.ClientID + "');");

        }
        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            oVariables = new Variables(intEnvironment);
            if (fileUpload.FileName != "" && fileUpload.PostedFile != null)
            {
                string strExtension = fileUpload.FileName;
                string strType = strExtension.Substring(0, 3);
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                string strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + intProfile.ToString() + strExtension;
                string strVirtualPath = oVariables.UploadsFolder() + strFile;
                string strPath = oVariables.UploadsFolder() + strFile;
                fileUpload.PostedFile.SaveAs(strPath);
                lblPath.Text = strVirtualPath;
            }

            oCustomized.UpdateEnhancement(intRequest, txtTitle.Text, txtDescription.Text, Int32.Parse(drpModules.SelectedValue), Int32.Parse(txtNumUsers.Text), txtURL.Text, lblPath.Text, DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text));
            oServiceRequest.Update(intRequest, txtTitle.Text);
            oRequest.UpdateDescription(intRequest, txtTitle.Text);
            ClientScript.RegisterClientScriptBlock(typeof(Page), "redirect", "window.top.navigate(window.top.location);", true);

        }
    }
}
