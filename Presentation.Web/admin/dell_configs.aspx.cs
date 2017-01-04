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
    public partial class dell_configs : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Dells oDell;
        protected int intProfile;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/dell_configs.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oDell = new Dells(intProfile, dsn);
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["add"] == null)
                    LoopRepeater();
                else
                    panAdd.Visible = true;
            }
            else
            {
                panAdd.Visible = true;
                intID = Int32.Parse(Request.QueryString["id"]);
                if (intID > 0)
                {
                    if (!IsPostBack)
                    {
                        DataSet ds = oDell.Get(intID);
                        txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                        txtSplit.Text = ds.Tables[0].Rows[0]["xml_split"].ToString();
                        txtOperator.Text = ds.Tables[0].Rows[0]["xml_operator"].ToString();
                        txtStart.Text = ds.Tables[0].Rows[0]["xml_start"].ToString();
                        txtQueryPower.Text = ds.Tables[0].Rows[0]["query_power"].ToString();
                        txtQueryMAC1.Text = ds.Tables[0].Rows[0]["query_mac1"].ToString();
                        txtQueryMAC2.Text = ds.Tables[0].Rows[0]["query_mac2"].ToString();
                        txtPowerOn.Text = ds.Tables[0].Rows[0]["success_power_on"].ToString();
                        txtPowerOff.Text = ds.Tables[0].Rows[0]["success_power_off"].ToString();
                        txtUsername.Text = ds.Tables[0].Rows[0]["username"].ToString();
                        txtPassword.Text = ds.Tables[0].Rows[0]["password"].ToString();
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
            DataSet ds = oDell.Gets(0);
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
                oDell.Add(txtName.Text, txtSplit.Text, txtOperator.Text, txtStart.Text, txtQueryPower.Text, txtQueryMAC1.Text, txtQueryMAC2.Text, txtPowerOn.Text, txtPowerOff.Text, txtUsername.Text, txtPassword.Text, (chkEnabled.Checked ? 1 : 0));
            else
                oDell.Update(intID, txtName.Text, txtSplit.Text, txtOperator.Text, txtStart.Text, txtQueryPower.Text, txtQueryMAC1.Text, txtQueryMAC2.Text, txtPowerOn.Text, txtPowerOff.Text, txtUsername.Text, txtPassword.Text, (chkEnabled.Checked ? 1 : 0));
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oDell.Enable(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oDell.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oDell.Delete(intID);
            Response.Redirect(Request.Path);
        }
    }
}
