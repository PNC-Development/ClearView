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
    public partial class forecast_operational_costs : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Models oModel;
        protected Types oType;
        protected Platforms oPlatform;
        protected Forecast oForecast;
        protected int intProfile;
        protected int intModel = 0;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/forecast/forecast_operational_costs.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oModel = new Models(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            if (Request.QueryString["mid"] != null && Request.QueryString["mid"] != "")
                intModel = Int32.Parse(Request.QueryString["mid"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (!IsPostBack)
            {
                if (intModel == 0)
                {
                    LoadAll();
                    panAll.Visible = true;
                }
                else
                {
                    LoadList();
                    LoadModel();
                    panView.Visible = true;
                }
            }
        }
        protected void LoadAll()
        {
            DataSet ds = oPlatform.Gets(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                LoadTypes(Int32.Parse(dr["platformid"].ToString()), oNode);
            }
        }
        protected void LoadTypes(int _platformid, TreeNode oParent)
        {
            DataSet ds = oType.Gets(_platformid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadModels(Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        protected void LoadModels(int _typeid, TreeNode oParent)
        {
            DataSet ds = oModel.Gets(_typeid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = Request.Path + "?mid=" + dr["id"].ToString();
                oParent.ChildNodes.Add(oNode);
            }
            oTreeview.ExpandDepth = 2;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        protected void LoadList()
        {
            DataSet ds = oForecast.GetLineItems(1);
            ddlLineItem.DataTextField = "name";
            ddlLineItem.DataValueField = "id";
            ddlLineItem.DataSource = ds;
            ddlLineItem.DataBind();
            ddlLineItem.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void LoadModel()
        {
            lblName.Text = oModel.Get(intModel, "name");
            rptAll.DataSource = oForecast.GetOperations(intModel, 0);
            rptAll.DataBind();
            foreach (RepeaterItem ri in rptAll.Items)
                ((LinkButton)ri.FindControl("btnDelete")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            lblNone.Visible = (rptAll.Items.Count == 0);
            if (intID > 0)
            {
                DataSet ds = oForecast.GetOperation(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlLineItem.SelectedValue = ds.Tables[0].Rows[0]["lineitemid"].ToString();
                    txtCost.Text = double.Parse(ds.Tables[0].Rows[0]["cost"].ToString()).ToString("F");
                    chkProduction.Checked = (ds.Tables[0].Rows[0]["prod"].ToString() == "1");
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                    btnAdd.Text = "Update";
                }
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (intID == 0)
                oForecast.AddOperation(intModel, Int32.Parse(ddlLineItem.SelectedItem.Value), double.Parse(txtCost.Text), (chkProduction.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            else
                oForecast.UpdateOperation(intID, Int32.Parse(ddlLineItem.SelectedItem.Value), double.Parse(txtCost.Text), (chkProduction.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            Response.Redirect(Request.Path + "?mid=" + intModel.ToString());
        }
        protected void btnEdit_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            Response.Redirect(Request.Path + "?mid=" + intModel.ToString() + "&id=" + oButton.CommandArgument);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oForecast.DeleteOperation(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?mid=" + intModel.ToString());
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
    }
}
