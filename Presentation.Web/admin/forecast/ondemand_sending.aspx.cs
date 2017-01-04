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
    public partial class ondemand_sending : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected OnDemandSending oOnDemandSending;
        protected Environments oEnvironment;
        protected Classes oClass;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/forecast/ondemand_sending.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oOnDemandSending = new OnDemandSending(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            if (!IsPostBack)
            {
                LoadClasses();
                LoadLists();
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',0);");
                ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
            }
        }
        private void LoadLists()
        {
            ddlClass.DataTextField = "name";
            ddlClass.DataValueField = "id";
            ddlClass.DataSource = oClass.Gets(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void LoadClasses()
        {
            DataSet ds = oClass.Gets(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                LoadEnvironments(Int32.Parse(dr["id"].ToString()), oNode);
            }
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadEnvironments(int _parent, TreeNode oParent)
        {
            DataSet ds = oClass.GetEnvironment(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                int intCount = LoadAnswer(_parent, Int32.Parse(dr["id"].ToString()), oNode);
                if (intCount == 0)
                {
                    TreeNode oNew = new TreeNode();
                    oNew.Text = "&nbsp;Add Answering Service";
                    oNew.ToolTip = "Add Answering Service";
                    oNew.ImageUrl = "/images/green_right.gif";
                    oNew.NavigateUrl = "javascript:Add('" + _parent.ToString() + "','" + dr["id"].ToString() + "');";
                    oNode.ChildNodes.Add(oNew);
                }
            }
        }
        protected int LoadAnswer(int _classid, int _environmentid, TreeNode oParent)
        {
            DataSet ds = oOnDemandSending.GetConfig(_classid, _environmentid);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = "javascript:Edit('" + dr["id"].ToString() + "','" + _classid.ToString() + "','" + _environmentid.ToString() + "','" + dr["name"].ToString() + "','" + dr["enabled"].ToString() + "');";
                oParent.ChildNodes.Add(oNode);
            }
            return ds.Tables[0].Rows.Count;
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intClass = Int32.Parse(ddlClass.SelectedItem.Value);
            int intEnv = Int32.Parse(Request.Form[hdnEnvironment.UniqueID]);
            if (Request.Form[hdnId.UniqueID] == "0")
                oOnDemandSending.AddConfig(intClass, intEnv, txtName.Text, (chkEnabled.Checked ? 1 : 0));
            else
                oOnDemandSending.UpdateConfig(Int32.Parse(Request.Form[hdnId.UniqueID]), intClass, intEnv, txtName.Text, (chkEnabled.Checked ? 1 : 0));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oOnDemandSending.DeleteConfig(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
