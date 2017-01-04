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

namespace NCC.ClearView.Presentation.Web
{
    public partial class costavoidance_new : System.Web.UI.UserControl
    {

        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;

        protected Customized oCustomized;
        protected Applications oApplication;
        protected Pages oPage;
        private Variables oVariables;

        protected int intApplication;
        protected int intPage;
        protected int intProfile;
        protected int intId;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oCustomized = new Customized(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intId = Int32.Parse(Request.QueryString["id"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            if (intId > 0)
            {
                DataSet ds = oCustomized.GetCostAvoidanceById(intId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    btnUpdate.Visible = true;
                    lblTitle.Text = "Edit Cost Avoidance";
                    txtCAO.Text = ds.Tables[0].Rows[0]["opportunity"].ToString();
                    txtDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                    if (ds.Tables[0].Rows[0]["path"].ToString() != "")
                    {
                        panUploaded.Visible = true;
                        hypUpload.NavigateUrl = ds.Tables[0].Rows[0]["path"].ToString();
                    }
                    else
                    {
                        panUpload.Visible = true;
                    }
                    panNew.Visible = false;
                    panView.Visible = true;
                    string strVal = ds.Tables[0].Rows[0]["addtlcostavoidance"].ToString();
                    strVal = strVal == "" ? "0" : strVal;
                    txtAddtlCA.Text = Double.Parse(strVal).ToString("F");
                    txtDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["date"].ToString()).ToShortDateString();

                }

            }
            else
            {
                panNew.Visible = true;
                panView.Visible = false;
                panUpload.Visible = true;
                btnSave.Visible = true;
                lblTitle.Text = oPage.Get(intPage, "title");
            }
            imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
            btnSave.Attributes.Add("onclick", "return ValidateText('" + txtCAO.ClientID + "','Please enter a text for Cost Avoidance Opportunity') " +
                    " && EnsureNumber('" + txtAddtlCA.ClientID + "','Please enter a valid cost') " +
                     " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid date');");

            btnUpdate.Attributes.Add("onclick", "return ValidateText('" + txtCAO.ClientID + "','Please enter a text for Cost Avoidance Opportunity') " +
                    " && EnsureNumber('" + txtAddtlCA.ClientID + "','Please enter a valid cost') " +
                     " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid date');");
            btnView.Attributes.Add("onclick", "return OpenWindow('COST_AVOIDANCE','?id=" + intId + "');");
        }

        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oVariables = new Variables(intEnvironment);
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
            int intId = oCustomized.AddCostAvoidance(txtCAO.Text, txtDescription.Text, strVirtualPath, (txtAddtlCA.Text == "" ? 0.00 : double.Parse(txtAddtlCA.Text)), DateTime.Parse(txtDate.Text), intApplication, intProfile);
            oCustomized.UpdateCategoryList(intProfile, intId);
            Response.Redirect(oPage.GetFullLink(intPage));

        }

        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            oVariables = new Variables(intEnvironment);
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
            oCustomized.UpdateCostAvoidance(intId, txtCAO.Text, txtDescription.Text, strVirtualPath, (txtAddtlCA.Text == "" ? 0.00 : double.Parse(txtAddtlCA.Text.Replace("$", ""))), DateTime.Parse(txtDate.Text));
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"]);
        }

        protected void btnDeleteAttachment_Click(Object Sender, EventArgs e)
        {
            oVariables = new Variables(intEnvironment);
            string strFile = hypUpload.NavigateUrl;
            strFile = strFile.Substring(strFile.LastIndexOf("/") + 1);
            if (File.Exists(oVariables.UploadsFolder() + strFile) == true)
                File.Delete(oVariables.UploadsFolder() + strFile);
            oCustomized.UpdateCostAvoidance(intId, "");
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"]);
        }
  
    }
}