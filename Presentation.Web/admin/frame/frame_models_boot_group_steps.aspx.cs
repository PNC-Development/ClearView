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
    public partial class frame_models_boot_group_steps : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Models oModel;

        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oModel = new Models(intProfile, dsn);
            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">alert('Step Added Successfully');<" + "/" + "script>");
            if (Request.QueryString["delete"] != null && Request.QueryString["delete"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "delete", "<script type=\"text/javascript\">alert('Step Deleted Successfully');<" + "/" + "script>");
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intID = Int32.Parse(Request.QueryString["id"]);
                if (!IsPostBack)
                {
                    lblName.Text = oModel.GetBootGroup(intID, "name");
                    rptItems.DataSource = oModel.GetBootGroupSteps(intID, 0);
                    rptItems.DataBind();
                    lblNone.Visible = (rptItems.Items.Count == 0);
                    foreach (RepeaterItem ri in rptItems.Items)
                        ((ImageButton)ri.FindControl("btnDelete")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                }
                btnClose.Attributes.Add("onclick", "return HidePanel();");
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            oModel.AddBootGroupStep(intID, txtWaitFor.Text, txtThenWrite.Text, -1, Int32.Parse(txtTimeout.Text), (chkPower.Checked ? 1 : 0), (rptItems.Items.Count + 1), 1);
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }
        protected void btnUp_Click(Object Sender, EventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            int intThis = Int32.Parse(oButton.CommandArgument);
            int intOther = 0;
            DataSet ds = oModel.GetBootGroupSteps(intID, 0);
            int intStep = 0;
            for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
            {
                intStep++;
                if (Int32.Parse(ds.Tables[0].Rows[ii]["id"].ToString()) == intThis)
                {
                    try
                    {
                        intOther = Int32.Parse(ds.Tables[0].Rows[ii - 1]["id"].ToString());
                        break;
                    }
                    catch { }
                }
            }
            if (intOther > 0)
            {
                oModel.UpdateBootGroupStep(intThis, intStep - 1);
                oModel.UpdateBootGroupStep(intOther, intStep);
            }
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
        protected void btnDown_Click(Object Sender, EventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            int intThis = Int32.Parse(oButton.CommandArgument);
            int intOther = 0;
            DataSet ds = oModel.GetBootGroupSteps(intID, 0);
            int intStep = 0;
            for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
            {
                intStep++;
                if (Int32.Parse(ds.Tables[0].Rows[ii]["id"].ToString()) == intThis)
                {
                    try
                    {
                        intOther = Int32.Parse(ds.Tables[0].Rows[ii + 1]["id"].ToString());
                        break;
                    }
                    catch { }
                }
            }
            if (intOther > 0)
            {
                oModel.UpdateBootGroupStep(intThis, intStep + 1);
                oModel.UpdateBootGroupStep(intOther, intStep);
            }
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oModel.DeleteBootGroupStep(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&delete=true");
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
