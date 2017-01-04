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
    public partial class whats_new : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private WhatsNew oWhatsNew;
        private int intProfile;
        private long lngID;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/whats_new.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oWhatsNew = new WhatsNew(intProfile, dsn);
            if (Request.QueryString["id"] == null)
            {
                panView.Visible = true;
                LoopRepeater();
            }
            else
            {
                panAdd.Visible = true;
                lngID = Int32.Parse(Request.QueryString["id"]);
                if (!IsPostBack)
                {
                    DataSet ds = oWhatsNew.Get(lngID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtTitle.Text = ds.Tables[0].Rows[0]["title"].ToString();
                        txtDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                        txtAttachment.Text = ds.Tables[0].Rows[0]["attachment"].ToString();
                        txtVersion.Text = ds.Tables[0].Rows[0]["Version"].ToString();
                        txtCategory.Text = ds.Tables[0].Rows[0]["Category"].ToString();
                        chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=WHATSNEW" + "',false,400,400);");
                        btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                    }
                    else
                    {
                        btnOrder.Enabled = false;
                        btnDelete.Enabled = false;
                    }
                }
            }
            btnBrowse.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtAttachment.ClientID + "','',false,400,600);");
        }

        private void LoopRepeater()
        {
            DataSet ds = oWhatsNew.Gets(0);
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
        public void OrderView(Object Sender, EventArgs e)
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
        public void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (txtCategory.Text == "")
                txtCategory.Text = "News";

            if (lngID == 0)
                oWhatsNew.Add(txtTitle.Text, txtDescription.Text, txtAttachment.Text, txtVersion.Text,txtCategory.Text, intProfile, (chkEnabled.Checked ? 1 : 0));
            else
                oWhatsNew.Update(lngID, txtTitle.Text, txtDescription.Text, txtAttachment.Text, txtVersion.Text, txtCategory.Text, intProfile, (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                long lngCount = 0;
                while (strOrder != "")
                {
                    lngCount++;
                    long lngID1 = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oWhatsNew.UpdateOrder(lngID1, lngCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        public void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oWhatsNew.Enable(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        public void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oWhatsNew.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        public void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=0");
        }
        public void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        public void btnDelete_Click(Object Sender, EventArgs e)
        {
            if (lngID != 0)
                oWhatsNew.Delete(lngID);
            Response.Redirect(Request.Path);
        }
    }
}
