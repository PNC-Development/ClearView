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
    public partial class enhancement_approvals_groups : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Enhancements oEnhancement;
        protected int intEnhancementID = 0;
        protected int intStep = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            oEnhancement = new Enhancements(0, dsn);
            if (Request.QueryString["enhancementid"] != null && Request.QueryString["enhancementid"] != "")
                Int32.TryParse(Request.QueryString["enhancementid"], out intEnhancementID);
            if (Request.QueryString["step"] != null && Request.QueryString["step"] != "")
                Int32.TryParse(Request.QueryString["step"], out intStep);

            if (intEnhancementID > 0 && intStep > 0)
            {
                if (!IsPostBack)
                {
                    ddlGroup.DataTextField = "name";
                    ddlGroup.DataValueField = "id";
                    ddlGroup.DataSource = oEnhancement.GetApprovalGroups(1);
                    ddlGroup.DataBind();
                    ddlGroup.Items.Insert(0, new ListItem("-- SELECT --", "0"));

                    rptItems.DataSource = oEnhancement.GetApprovals(intEnhancementID, intStep);
                    rptItems.DataBind();
                    lblNone.Visible = (rptItems.Items.Count == 0);
                    foreach (RepeaterItem ri in rptItems.Items)
                        ((LinkButton)ri.FindControl("btnDelete")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                }
                btnClose.Attributes.Add("onclick", "window.parent.navigate(window.parent.location);return false;");
                btnAdd.Attributes.Add("onclick", "return ValidateDropDown('" + ddlGroup + "','Please select a group')" +
                    ";");
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            oEnhancement.AddApproval(intEnhancementID, intStep, Int32.Parse(ddlGroup.SelectedItem.Value));
            Response.Redirect(Request.Url.PathAndQuery);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oEnhancement.DeleteApproval(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Url.PathAndQuery);
        }
    }
}
