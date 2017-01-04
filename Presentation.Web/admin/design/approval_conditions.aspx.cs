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
using NCC.ClearView.Presentation.Web.Custom;

namespace NCC.ClearView.Presentation.Web
{
    public partial class approval_conditions : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Design oDesign;
        protected Functions oFunction;
        protected int intProfile;
        protected int intCondition = 0;
        protected int intSet = 0;
        protected string strMenuTab1 = "";
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

            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);

            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["add"] == null)
                    LoopRepeater();
                else
                {
                    if (!IsPostBack)
                        LoadLists();
                    panAdd.Visible = true;
                    btnDelete.Enabled = false;
                }
            }
            else
            {
                if (!IsPostBack)
                    LoadLists();
                panAdd.Visible = true;
                intCondition = Int32.Parse(Request.QueryString["id"]);
            }

            if (Request.QueryString["set"] != null)
            {
                intSet = Int32.Parse(Request.QueryString["set"]);
                if (intSet > 0)
                {
                    intMenuTab = 2;
                    if (!IsPostBack)
                    {
                        DataSet ds = oDesign.GetApprovalConditionalSet(intSet);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            intCondition = Int32.Parse(ds.Tables[0].Rows[0]["approvalid"].ToString());
                            ddlFieldSet.SelectedValue = ds.Tables[0].Rows[0]["field"].ToString();
                            if (ds.Tables[0].Rows[0]["is_eq"].ToString() == "1")
                                ddlOperator.SelectedValue = "eq";
                            else if (ds.Tables[0].Rows[0]["is_neq"].ToString() == "1")
                                ddlOperator.SelectedValue = "neq";
                            else if (ds.Tables[0].Rows[0]["is_lt"].ToString() == "1")
                                ddlOperator.SelectedValue = "lt";
                            else if (ds.Tables[0].Rows[0]["is_lte"].ToString() == "1")
                                ddlOperator.SelectedValue = "lte";
                            else if (ds.Tables[0].Rows[0]["is_gt"].ToString() == "1")
                                ddlOperator.SelectedValue = "gt";
                            else if (ds.Tables[0].Rows[0]["is_gte"].ToString() == "1")
                                ddlOperator.SelectedValue = "gte";
                            else if (ds.Tables[0].Rows[0]["is_in"].ToString() == "1")
                                ddlOperator.SelectedValue = "in";
                            else if (ds.Tables[0].Rows[0]["is_nin"].ToString() == "1")
                                ddlOperator.SelectedValue = "nin";
                            else if (ds.Tables[0].Rows[0]["is_ends"].ToString() == "1")
                                ddlOperator.SelectedValue = "ends";
                            else if (ds.Tables[0].Rows[0]["is_starts"].ToString() == "1")
                                ddlOperator.SelectedValue = "starts";
                            txtValue.Text = ds.Tables[0].Rows[0]["value"].ToString();
                            if (ds.Tables[0].Rows[0]["dt_int"].ToString() == "1")
                                radTypeInt.Checked = true;
                            else if (ds.Tables[0].Rows[0]["dt_date"].ToString() == "1")
                                radTypeDateTime.Checked = true;
                            else
                                radTypeString.Checked = true;
                            txtOr.Text = ds.Tables[0].Rows[0]["or_group"].ToString();
                            btnDelete.Visible = true;
                            btnCancel.Visible = true;
                            btnSetAdd.Text = "Update";
                        }
                    }
                }
            }

            Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);
            oTab.AddTab("Details", "");

            if (intCondition > 0 && !IsPostBack)
            {
                DataSet ds = oDesign.GetApprovalConditional(intCondition);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    oTab.AddTab("Condition Sets", "");
                    rptSets.DataSource = oDesign.GetApprovalConditionalSets(intCondition);
                    rptSets.DataBind();

                    hdnId.Value = intCondition.ToString();
                    txtName.Text = litName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    if (String.IsNullOrEmpty(ds.Tables[0].Rows[0]["approve_by_field"].ToString()) == false)
                    {
                        ddlField.Enabled = true;
                        ddlField.SelectedValue = ds.Tables[0].Rows[0]["approve_by_field"].ToString();
                        radApproveField.Checked = true;
                    }
                    else
                    {
                        int intGroup = 0;
                        if (Int32.TryParse(ds.Tables[0].Rows[0]["approve_by_group"].ToString(), out intGroup) && intGroup > 0)
                        {
                            ddlGroup.Enabled = true;
                            ddlGroup.SelectedValue = intGroup.ToString();
                            radApproveGroup.Checked = true;
                        }
                        else
                        {
                            radApproveRequestor.Checked = (ds.Tables[0].Rows[0]["approve_by_requestor"].ToString() == "1");
                            radApproveAppOwner.Checked = (ds.Tables[0].Rows[0]["approve_by_app_owner"].ToString() == "1");
                            radApproveATL.Checked = (ds.Tables[0].Rows[0]["approve_by_atl"].ToString() == "1");
                            radApproveASM.Checked = (ds.Tables[0].Rows[0]["approve_by_asm"].ToString() == "1");
                            radApproveSD.Checked = (ds.Tables[0].Rows[0]["approve_by_sd"].ToString() == "1");
                            radApproveDM.Checked = (ds.Tables[0].Rows[0]["approve_by_dm"].ToString() == "1");
                            radApproveCIO.Checked = (ds.Tables[0].Rows[0]["approve_by_cio"].ToString() == "1");
                        }
                    }
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                }
            }

            strMenuTab1 = oTab.GetTabs();
            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=DESIGN_APPROVE_CONDITION" + "',false,400,400);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
        }
        private void LoadLists()
        {
            ddlField.DataTextField = ddlFieldSet.DataTextField = "name";
            ddlField.DataValueField = ddlFieldSet.DataValueField = "name";
            ddlField.DataSource = ddlFieldSet.DataSource = oFunction.GetSystemTableColumns("cv_designs");
            ddlField.DataBind();
            ddlFieldSet.DataBind();
            ddlField.Items.Insert(0, new ListItem("-- SELECT --", ""));
            ddlFieldSet.Items.Insert(0, new ListItem("-- SELECT --", ""));

            ddlGroup.DataSource = oDesign.GetApprovalGroups(1);
            ddlGroup.DataTextField = "Name";
            ddlGroup.DataValueField = "id";
            ddlGroup.DataBind();
            ddlGroup.Items.Insert(0, new ListItem("-- SELECT --", ""));
        }
        private void LoopRepeater()
        {
            panView.Visible = true;
            DataSet ds = oDesign.GetApprovalConditionals(0);
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
            string strField = "";
            int intGroup = 0;
            if (radApproveField.Checked && ddlField.Enabled)
                strField = ddlField.SelectedItem.Value;
            else if (radApproveGroup.Checked && ddlGroup.Enabled)
                Int32.TryParse(ddlGroup.SelectedItem.Value, out intGroup);

            if (intCondition == 0)
            {
                intCondition = oDesign.AddApprovalConditional(txtName.Text, strField, intGroup, (radApproveRequestor.Checked ? 1 : 0), (radApproveAppOwner.Checked ? 1 : 0), (radApproveATL.Checked ? 1 : 0), (radApproveASM.Checked ? 1 : 0), (radApproveSD.Checked ? 1 : 0), (radApproveDM.Checked ? 1 : 0), (radApproveCIO.Checked ? 1 : 0), (oDesign.GetApprovalConditionals(0).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
                Response.Redirect(Request.Path + "?id=" + intCondition.ToString() + "&menu_tab=2");
            }
            else
            {
                oDesign.UpdateApprovalConditional(intCondition, txtName.Text, strField, intGroup, (radApproveRequestor.Checked ? 1 : 0), (radApproveAppOwner.Checked ? 1 : 0), (radApproveATL.Checked ? 1 : 0), (radApproveASM.Checked ? 1 : 0), (radApproveSD.Checked ? 1 : 0), (radApproveDM.Checked ? 1 : 0), (radApproveCIO.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
                if (Request.Form[hdnOrder.UniqueID] != "")
                {
                    string strOrder = Request.Form[hdnOrder.UniqueID];
                    int intCount = 0;
                    while (strOrder != "")
                    {
                        intCount++;
                        int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                        strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                        oDesign.UpdateApprovalConditionalOrder(intId, intCount);
                    }
                }
                Response.Redirect(Request.Path);
            }
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oDesign.EnableApprovalConditional(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oDesign.DeleteApprovalConditional(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oDesign.DeleteApprovalConditional(intCondition);
            Response.Redirect(Request.Path);
        }

        protected void radApproveField_CheckedChanged(object sender, EventArgs e)
        {
            ddlField.Enabled = true;
            ddlGroup.Enabled = false;
        }

        protected void radApproveGroup_CheckedChanged(object sender, EventArgs e)
        {
            ddlField.Enabled = false;
            ddlGroup.Enabled = true;
        }
        protected void radApproveBy_CheckedChanged(object sender, EventArgs e)
        {
            ddlField.Enabled = false;
            ddlGroup.Enabled = false;
        }

        protected void btnDeleteSet_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)sender;
            oDesign.DeleteApprovalConditionalSet(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&menu_tab=2");
        }

        protected void btnSetCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"]);
        }

        protected void btnSetDelete_Click(object sender, EventArgs e)
        {
            oDesign.DeleteApprovalConditionalSet(intSet);
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&menu_tab=2");
        }

        protected void btnSetAdd_Click(object sender, EventArgs e)
        {
            int is_lt = (ddlOperator.SelectedItem.Value == "lt" ? 1 : 0);
            int is_lte = (ddlOperator.SelectedItem.Value == "lte" ? 1 : 0);
            int is_gt = (ddlOperator.SelectedItem.Value == "gt" ? 1 : 0);
            int is_gte = (ddlOperator.SelectedItem.Value == "gte" ? 1 : 0);
            int is_eq = (ddlOperator.SelectedItem.Value == "eq" ? 1 : 0);
            int is_neq = (ddlOperator.SelectedItem.Value == "neq" ? 1 : 0);
            int is_in = (ddlOperator.SelectedItem.Value == "in" ? 1 : 0);
            int is_nin = (ddlOperator.SelectedItem.Value == "nin" ? 1 : 0);
            int is_ends = (ddlOperator.SelectedItem.Value == "ends" ? 1 : 0);
            int is_starts = (ddlOperator.SelectedItem.Value == "starts" ? 1 : 0);
            int dt_int = (radTypeInt.Checked ? 1 : 0);
            int dt_date = (radTypeDateTime.Checked ? 1 : 0);
            int or = 0;
            Int32.TryParse(txtOr.Text, out or);
            intCondition = Int32.Parse(Request.QueryString["id"]);
            if (intSet == 0)
                intSet = oDesign.AddApprovalConditionalSet(intCondition, ddlFieldSet.SelectedItem.Value, is_lt, is_lte, is_gt, is_gte, is_eq, is_neq, is_in, is_nin, is_ends, is_starts, dt_int, dt_date, txtValue.Text, or);
            else
                oDesign.UpdateApprovalConditionalSet(intSet, intCondition, ddlFieldSet.SelectedItem.Value, is_lt, is_lte, is_gt, is_gte, is_eq, is_neq, is_in, is_nin, is_ends, is_starts, dt_int, dt_date, txtValue.Text, or);
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&menu_tab=2");
        }
    }
}
