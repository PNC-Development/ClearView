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
    public partial class service_editor_field_mappings : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected ServiceEditor oServiceEditor;
        protected Users oUser;
        protected Services oService;
        private int intService = 0;
        private int intNextService = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oServiceEditor = new ServiceEditor(intProfile, dsnServiceEditor);
            oUser = new Users(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            Int32.TryParse(Request.QueryString["serviceid"], out intService);
            Int32.TryParse(Request.QueryString["nextservice"], out intNextService);
            if (Request.QueryString["saved"] != null)
                trSaved.Visible = true;
            if (!IsPostBack)
            {
                if (intService > 0 && intNextService > 0)
                {
                    rptView.DataSource = oServiceEditor.GetConfigsWorkflow(intService, intNextService);
                    rptView.DataBind();
                    lblNone.Visible = (rptView.Items.Count == 0);
                    foreach (RepeaterItem ri in rptView.Items)
                    {
                        Label lblService = (Label)ri.FindControl("lblService");
                        int intServiceBegin = 0;
                        if (Int32.TryParse(lblService.Text, out intServiceBegin) == true)
                        {
                            string strWorkflowTitle = oService.Get(intServiceBegin, "workflow_title");
                            if (strWorkflowTitle != "")
                                lblService.Text = strWorkflowTitle;
                            else
                                lblService.Text = oService.Get(intServiceBegin, "name");
                        }
                    }
                }
                chkInclude.Attributes.Add("onclick", "CheckListAll(this,'tblChecks');");
                chkEditable.Attributes.Add("onclick", "CheckListAll(this,'tblChecks');");
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oServiceEditor.DeleteConfigs(intNextService, 0);    // Since inheriting fields, delete all service request controls

            oServiceEditor.DeleteConfigWorkflows(intService, intNextService);
            foreach (RepeaterItem ri in rptView.Items) 
            {
                CheckBox chkInclude = (CheckBox)ri.FindControl("chkInclude");
                if (chkInclude.Checked == true)
                {
                    CheckBox chkEditable = (CheckBox)ri.FindControl("chkEditable");
                    oServiceEditor.AddConfigWorkflow(intService, intNextService, Int32.Parse(chkInclude.ToolTip), (chkEditable.Checked ? 1 : 0));
                }
            }
            oServiceEditor.AlterTable(intNextService);
            Response.Redirect(Request.Path + "?serviceid=" + intService.ToString() + "&nextservice=" + intNextService.ToString() + "&saved=true");
        }
    }
}
