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
    public partial class resource_request_email : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected ResourceRequest oResourceRequest;
        protected Users oUser;
        protected Variables oVariable;
        protected Functions oFunction;
        private int intResource = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oResourceRequest = new ResourceRequest(0, dsn);
            oUser = new Users(0, dsn);
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(0, dsn, intEnvironment);

            if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
            {
                intResource = Int32.Parse(Request.QueryString["rrid"]);
                txtEmployee.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divEmployee.ClientID + "','" + lstEmployee.ClientID + "','" + hdnEmployee.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstEmployee.Attributes.Add("ondblclick", "AJAXClickRow();");
            }
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            bool boolSent = false;
            DataSet dsWF = oResourceRequest.GetWorkflow(intResource);
            int intTo = 0;

            if (dsWF.Tables[0].Rows.Count > 0 && Int32.TryParse(Request.Form[hdnEmployee.UniqueID], out intTo) == true)
            {
                int intParent = Int32.Parse(dsWF.Tables[0].Rows[0]["parent"].ToString());
                DataSet ds = oResourceRequest.Get(intParent);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                    int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                    int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                    oFunction.SendEmail("Service Request [CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString() + "]", oUser.GetName(intTo), "", oUser.GetName(intProfile), "Service Request [CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString() + "]", "<p>The following service request information has been sent to you by <b>" + oUser.GetFullName(intProfile) + "</b>.</p>" + (txtComments.Text == "" ? "" : "<p>" + txtComments.Text + "</p>") + "<p>" + oResourceRequest.GetSummary(intParent, intResource, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                    boolSent = true;
                }
            }

            if (boolSent)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">alert('An email was successfully sent.');window.parent.location.reload();window.close();<" + "/" + "script>");
            else
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">alert('There was a problem sending the email.');window.parent.location.reload();window.close();<" + "/" + "script>");
        }
    }
}
