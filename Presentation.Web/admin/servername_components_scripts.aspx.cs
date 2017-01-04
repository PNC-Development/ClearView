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
    public partial class servername_components_scripts : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected ServerName oServerName;
        protected int intProfile;
        protected int intDetail = 0;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oServerName = new ServerName(intProfile, dsn);

            if (Request.QueryString["detailid"] != null)
                intDetail = Int32.Parse(Request.QueryString["detailid"]);

            if (intDetail > 0)
            {
                hdnParent.Value = intDetail.ToString();
                lblParent.Text = oServerName.GetComponentDetail(intDetail, "name");
                if (Request.QueryString["id"] == null)
                    LoopRepeater();
                else
                {
                    panAdd.Visible = true;
                    intID = Int32.Parse(Request.QueryString["id"]);
                    if (!IsPostBack)
                    {
                        if (intID > 0)
                        {
                            DataSet ds = oServerName.GetComponentDetailScript(intID);
                            txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                            txtScript.Text = ds.Tables[0].Rows[0]["script"].ToString();
                            chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                            btnAdd.Text = "Update";
                        }
                        else
                        {
                            btnOrder.Enabled = false;
                            btnDelete.Enabled = false;
                        }
                    }
                }
            }
            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnParent.ClientID + "','" + hdnOrder.ClientID + "&type=COMPONENT_SCRIPTS" + "',false,400,400);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
        }
        private void LoopRepeater()
        {
            panView.Visible = true;
            DataSet ds = oServerName.GetComponentDetailScripts(intDetail, 0);
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
            Response.Redirect(Request.Path + "?detailid=" + intDetail.ToString() + "&sort=" + strSort);
        }
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?detailid=" + intDetail.ToString() + "&id=0");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (intID == 0)
                oServerName.AddComponentDetailScript(intDetail, txtName.Text, txtScript.Text, (oServerName.GetComponentDetailScripts(intDetail, 0).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
            else
                oServerName.UpdateComponentDetailScript(intID, intDetail, txtName.Text, txtScript.Text, (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oServerName.UpdateComponentDetailScriptOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path + "?detailid=" + intDetail.ToString());
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oServerName.EnableComponentDetailScript(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path + "?detailid=" + intDetail.ToString());
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oServerName.DeleteComponentDetailScript(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?detailid=" + intDetail.ToString());
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?detailid=" + intDetail.ToString());
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oServerName.DeleteComponentDetailScript(intID);
            Response.Redirect(Request.Path + "?detailid=" + intDetail.ToString());
        }
    }
}
