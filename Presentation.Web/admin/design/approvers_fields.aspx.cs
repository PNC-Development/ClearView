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
    public partial class design_approvers_fields : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Design oDesign;
        protected Functions oFunction;
        protected int intProfile;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = Request.Path;
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oDesign = new Design(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            if (!IsPostBack)
            {
                ddlFields.DataTextField = "name";
                ddlFields.DataValueField = "name";
                ddlFields.DataSource = oFunction.GetSystemTableColumns("cv_designs");
                ddlFields.DataBind();
                ddlFields.Items.Insert(0, new ListItem("-- SELECT --", ""));
            }
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["add"] == null)
                    LoopRepeater();
                else
                {
                    panAdd.Visible = true;
                    btnDelete.Enabled = false;
                }
            }
            else
            {
                panAdd.Visible = true;
                intID = Int32.Parse(Request.QueryString["id"]);
                if (intID > 0)
                {
                    if (!IsPostBack)
                    {
                        DataSet ds = oDesign.GetApproverField(intID);
                        hdnId.Value = intID.ToString();
                        ddlFields.SelectedValue = ds.Tables[0].Rows[0]["related_field"].ToString();
                        txtTitle.Text = ds.Tables[0].Rows[0]["title"].ToString();
                        chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnAdd.Text = "Update";
                    }
                }
                else
                {
                    btnDelete.Enabled = false;
                }
            }
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
        }
        private void LoopRepeater()
        {
            panView.Visible = true;
            DataSet ds = oDesign.GetApproverFields(0);
            DataView dv = ds.Tables[0].DefaultView;
            if (Request.QueryString["sort"] != null)
                dv.Sort = Request.QueryString["sort"].ToString();
            rptView.DataSource = dv;
            rptView.DataBind();
            foreach (RepeaterItem ri in rptView.Items)
            {
                ImageButton oDelete = (ImageButton)ri.FindControl("btnDelete");
                oDelete.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this item?');");
                ImageButton oEnable = (ImageButton)ri.FindControl("btnEnable");
                if (oEnable.ImageUrl == "/admin/images/enabled.gif")
                {
                    oEnable.ToolTip = "Click to disable";
                    oEnable.Attributes.Add("onClick", "return confirm('Are you sure you want to disable this item?');");
                }
                else
                    oEnable.ToolTip = "Click to enable";
            }
        }
        protected void OrderView(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            string strSort;
            if (Request.QueryString["sort"] == null)
                strSort = oButton.CommandArgument + " ASC";
            else
                if (Request.QueryString["sort"].ToString() == (oButton.CommandArgument + " ASC"))
                    strSort = oButton.CommandArgument + " DESC";
                else
                    strSort = oButton.CommandArgument + " ASC";
            Response.Redirect(Request.Path + "?sort=" + strSort);
        }
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=0");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (intID == 0)
                oDesign.AddApproverField(ddlFields.SelectedItem.Value, txtTitle.Text, 0, (chkEnabled.Checked ? 1 : 0));
            else
                oDesign.UpdateApproverField(intID, ddlFields.SelectedItem.Value, txtTitle.Text, 0, (chkEnabled.Checked ? 1 : 0));
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oDesign.EnableApproverField(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oDesign.DeleteApproverField(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oDesign.DeleteApproverField(intID);
            Response.Redirect(Request.Path);
        }
    }
}
