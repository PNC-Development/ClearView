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
    public partial class frame_service_details : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected Services oService;
        protected ServiceDetails oServiceDetail;
        protected int intService;
        protected int intDetail = -1;
        protected int intParent = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            {
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
                oService = new Services(intProfile, dsn);
                oServiceDetail = new ServiceDetails(intProfile, dsn);
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                    intService = Int32.Parse(Request.QueryString["id"]);
                if (Request.QueryString["did"] != null)
                {
                    panEdit.Visible = true;
                    intDetail = Int32.Parse(Request.QueryString["did"]);
                    if (!IsPostBack)
                    {
                        DataSet ds = oServiceDetail.Get(intDetail);
                        txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                        txtHours.Text = ds.Tables[0].Rows[0]["hours"].ToString();
                        txtAdditional.Text = ds.Tables[0].Rows[0]["additional"].ToString();
                        chkCheck.Checked = (ds.Tables[0].Rows[0]["checkbox"].ToString() == "1");
                        chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                    }
                }
                else if (Request.QueryString["parent"] != null)
                {
                    panEdit.Visible = true;
                    intParent = Int32.Parse(Request.QueryString["parent"]);
                    chkEnabled.Checked = true;
                }
                else
                {
                    panAll.Visible = true;
                    if (!IsPostBack)
                        LoadDetails(0, null);
                }
                if (intService > 0)
                    lblName.Text = oService.GetName(intService);
            }
        }
        private void LoadDetails(int _parent, TreeNode oParent)
        {
            DataSet ds = oServiceDetail.Gets(intService, _parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = (dr["enabled"].ToString() == "1" ? "/images/go.gif" : "/images/cancel.gif");
                oNode.NavigateUrl = Request.Path + "?id=" + intService.ToString() + "&did=" + dr["id"].ToString();
                if (oParent != null)
                    oParent.ChildNodes.Add(oNode);
                else
                    oTree.Nodes.Add(oNode);
                LoadDetails(Int32.Parse(dr["id"].ToString()), oNode);
            }
            TreeNode oNew = new TreeNode();
            oNew.Text = "&nbsp;Add a New Task";
            oNew.ToolTip = "Add a New Task";
            oNew.ImageUrl = "/images/postit.gif";
            oNew.NavigateUrl = Request.Path + "?id=" + intService.ToString() + "&parent=" + _parent.ToString();
            if (oParent != null)
                oParent.ChildNodes.Add(oNew);
            else
                oTree.Nodes.Add(oNew);
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            if (intParent == -1)
                oServiceDetail.Update(intDetail, txtName.Text, Int32.Parse(oServiceDetail.Get(intDetail, "parent")), double.Parse(txtHours.Text), double.Parse(txtAdditional.Text), (chkCheck.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            else
                oServiceDetail.Add(intService, txtName.Text, intParent, double.Parse(txtHours.Text), double.Parse(txtAdditional.Text), (chkCheck.Checked ? 1 : 0), (oServiceDetail.Gets(intService, intParent, 0).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
            Response.Redirect(Request.Path + "?id=" + intService.ToString());
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + intService.ToString());
        }
    }
}
