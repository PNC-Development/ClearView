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
using System.Text;
using NCC.ClearView.Presentation.Web.Custom;

namespace NCC.ClearView.Presentation.Web
{
    public partial class design_questions : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Design oDesign;
        protected Functions oFunction;
        protected int intProfile;
        protected int intID = 0;
        protected string strDisabled = "";
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

            int intQuestionDisplay = 0;
            int intPhase = 0;
            int intPhaseDisplay = 0;

            if (!IsPostBack)
            {
                LoadLists();
                ddlFields.DataTextField = "name";
                ddlFields.DataValueField = "name";
                ddlFields.DataSource = oFunction.GetSystemTableColumns("cv_designs");
                ddlFields.DataBind();
                ddlFields.Items.Insert(0, new ListItem("-- SELECT --", ""));
            }
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["parent"] == null)
                    LoadTree();
                else
                {
                    panAdd.Visible = true;
                    ddlPhase.SelectedValue = Request.QueryString["parent"];
                    btnOrder.Enabled = false;
                    btnDelete.Enabled = false;
                    strDisabled = "<p><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> <b>This section is only available when updating an existing item</b></p>";
                }
            }
            else
            {
                if (Request.QueryString["save"] != null)
                    trSave.Visible = true;
                panAdd.Visible = true;
                intID = Int32.Parse(Request.QueryString["id"]);
                if (!IsPostBack)
                {
                    DataSet ds = oDesign.GetQuestion(intID);
                    Int32.TryParse(ds.Tables[0].Rows[0]["display"].ToString(), out intQuestionDisplay);
                    Int32.TryParse(ds.Tables[0].Rows[0]["phaseid"].ToString(), out intPhase);
                    Int32.TryParse(oDesign.GetPhase(intPhase, "display"), out intPhaseDisplay);
                    hdnId.Value = intID.ToString();
                    txtQuestion.Text = ds.Tables[0].Rows[0]["question"].ToString();
                    ddlPhase.SelectedValue = intPhase.ToString();
                    txtSummary.Text = ds.Tables[0].Rows[0]["summary"].ToString();
                    lblName.Text = txtSummary.Text;
                    ddlFields.SelectedValue = ds.Tables[0].Rows[0]["related_field"].ToString();
                    txtValue.Text = ds.Tables[0].Rows[0]["default_value"].ToString();
                    txtSuffix.Text = ds.Tables[0].Rows[0]["suffix"].ToString();
                    chkEmpty.Checked = (ds.Tables[0].Rows[0]["allow_empty"].ToString() == "1");
                    chkMnemonic.Checked = (ds.Tables[0].Rows[0]["is_mnemonic"].ToString() == "1");
                    chkCostCenter.Checked = (ds.Tables[0].Rows[0]["is_cost_center"].ToString() == "1");
                    chkUserSI.Checked = (ds.Tables[0].Rows[0]["is_user_si"].ToString() == "1");
                    chkUserDTG.Checked = (ds.Tables[0].Rows[0]["is_user_dtg"].ToString() == "1");
                    chkGridBackup.Checked = (ds.Tables[0].Rows[0]["is_grid_backup"].ToString() == "1");
                    chkBackupExclusions.Checked = (ds.Tables[0].Rows[0]["is_backup_exclusions"].ToString() == "1");
                    chkGridMaintenance.Checked = (ds.Tables[0].Rows[0]["is_grid_maintenance"].ToString() == "1");
                    chkStorageLuns.Checked = (ds.Tables[0].Rows[0]["is_storage_luns"].ToString() == "1");
                    chkAccounts.Checked = (ds.Tables[0].Rows[0]["is_accounts"].ToString() == "1");
                    chkDate.Checked = (ds.Tables[0].Rows[0]["is_date"].ToString() == "1");
                    chkLocation.Checked = (ds.Tables[0].Rows[0]["is_location"].ToString() == "1");
                    chkDropDown.Checked = (ds.Tables[0].Rows[0]["is_type_drop_down"].ToString() == "1");
                    chkCheckBox.Checked = (ds.Tables[0].Rows[0]["is_type_check_box"].ToString() == "1");
                    chkRadio.Checked = (ds.Tables[0].Rows[0]["is_type_radio"].ToString() == "1");
                    chkTextBox.Checked = (ds.Tables[0].Rows[0]["is_type_textbox"].ToString() == "1");
                    chkTextArea.Checked = (ds.Tables[0].Rows[0]["is_type_textarea"].ToString() == "1");
                    chkSummary.Checked = (ds.Tables[0].Rows[0]["show_summary"].ToString() == "1");
                    radShowAll.Checked = (ds.Tables[0].Rows[0]["show_all"].ToString() == "1");
                    radShowAny.Checked = (ds.Tables[0].Rows[0]["show_any"].ToString() == "1");
                    radShow.Checked = (radShowAll.Checked == false && radShowAny.Checked == false);
                    radHideAll.Checked = (ds.Tables[0].Rows[0]["hide_all"].ToString() == "1");
                    radHideAny.Checked = (ds.Tables[0].Rows[0]["hide_any"].ToString() == "1");
                    radHide.Checked = (radHideAll.Checked == false && radHideAny.Checked == false);
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                    btnAdd.Text = "Save";
                    btnAddBack.Text = "Save & Return";

                    // Load Dynamic Responses
                    LoadShowPhase(oTreeQuestionsShow, intQuestionDisplay, intPhaseDisplay, true);
                    LoadShowPhase(oTreeQuestionsHide, intQuestionDisplay, intPhaseDisplay, false);
                }
            }
            if (panAdd.Visible == true)
            {
                int intMenuTab = 0;
                if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                    intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                Tab oTab = new Tab("", intMenuTab, "divMenu1", true, false);
                oTab.AddTab("Properties", "");
                oTab.AddTab("Dynamic Responses", "");
                strMenuTab1 = oTab.GetTabs();
            }
            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + ddlPhase.ClientID + "','" + hdnOrder.ClientID + "&type=D_QUESTIONS" + "',false,400,400);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
        }
        private void LoadLists()
        {
            ddlPhase.DataTextField = "title";
            ddlPhase.DataValueField = "id";
            ddlPhase.DataSource = oDesign.GetPhases(1);
            ddlPhase.DataBind();
            ddlPhase.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void LoadTree()
        {
            panView.Visible = true;
            DataSet ds = oDesign.GetPhases(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = "&nbsp;" + dr["title"].ToString();
                oNode.ToolTip = dr["title"].ToString();
                oNode.ImageUrl = "/images/tack.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTree.Nodes.Add(oNode);
                LoadQuestion(Int32.Parse(dr["id"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Question";
                oNew.ToolTip = "Add Question";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = Request.Path + "?parent=" + dr["id"].ToString();
                oNode.ChildNodes.Add(oNew);
            }
            oTree.ExpandDepth = 1;
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadQuestion(int _parent, TreeNode oParent)
        {
            DataSet ds = oDesign.GetQuestions(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["summary"].ToString();
                oNode.ToolTip = dr["summary"].ToString();
                oNode.ImageUrl = (dr["enabled"].ToString() == "1" ? "" : "/images/cancel.gif");
                oNode.NavigateUrl = Request.Path + "?id=" + dr["id"].ToString();
                oParent.ChildNodes.Add(oNode);
            }
        }
        private void LoadShowPhase(TreeView oTree, int _display_q, int _display_p, bool _show)
        {
            DataSet ds = oDesign.GetPhases(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intDisplay = 0;
                TreeNode oNode = new TreeNode();
                oNode.Text = "&nbsp;" + dr["title"].ToString();
                oNode.ImageUrl = "/images/tack.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTree.Nodes.Add(oNode);
                if (Int32.TryParse(dr["display"].ToString(), out intDisplay) && intDisplay <= _display_p)
                    LoadShowQuestion(Int32.Parse(dr["id"].ToString()), oNode, (intDisplay == _display_p ? _display_q : 999), _show);
                else
                    oNode.ImageUrl = "/images/na.gif";
            }
            oTree.ExpandDepth = 2;
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadShowQuestion(int _parent, TreeNode oParent, int _display, bool _show)
        {
            DataSet ds = oDesign.GetQuestions(_parent, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string strSpecial = oDesign.GetQuestionSpecial(Int32.Parse(dr["id"].ToString()));
                int intDisplay = 0;
                TreeNode oNode = new TreeNode();
                oNode.Text = "&nbsp;" + dr["summary"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                if (strSpecial == "" && Int32.TryParse(dr["display"].ToString(), out intDisplay) && intDisplay < _display)
                    LoadShowResponse(Int32.Parse(dr["id"].ToString()), oNode, _show);
                else
                    oNode.ImageUrl = "/images/na.gif";
            }
        }
        private void LoadShowResponse(int _parent, TreeNode oParent, bool _show)
        {
            DataSet dsOther = oDesign.GetShows(intID, (_show ? 0 : 1));
            DataSet ds = oDesign.GetResponses(_parent, 0, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["admin"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.ShowCheckBox = true;
                oNode.Checked = false;
                oNode.SelectAction = TreeNodeSelectAction.None;
                foreach (DataRow drOther in dsOther.Tables[0].Rows)
                {
                    if (dr["id"].ToString() == drOther["responseid"].ToString())
                        oNode.Checked = true;
                }
                oParent.ChildNodes.Add(oNode);
            }
        }

        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            Save();
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }
        protected void btnAddBack_Click(Object Sender, EventArgs e)
        {
            Save();
            Response.Redirect(Request.Path);
        }
        private void Save()
        {
            int intPhase = Int32.Parse(ddlPhase.SelectedItem.Value);
            if (intID == 0)
                intID = oDesign.AddQuestion(intPhase, txtQuestion.Text, txtSummary.Text, (chkSummary.Checked ? 1 : 0), (chkMnemonic.Checked ? 1 : 0), (chkCostCenter.Checked ? 1 : 0), (chkUserSI.Checked ? 1 : 0), (chkUserDTG.Checked ? 1 : 0), (chkGridBackup.Checked ? 1 : 0), (chkBackupExclusions.Checked ? 1 : 0), (chkGridMaintenance.Checked ? 1 : 0), (chkStorageLuns.Checked ? 1 : 0), (chkAccounts.Checked ? 1 : 0), (chkDate.Checked ? 1 : 0), (chkLocation.Checked ? 1 : 0), (chkDropDown.Checked ? 1 : 0), (chkCheckBox.Checked ? 1 : 0), (chkRadio.Checked ? 1 : 0), (chkTextBox.Checked ? 1 : 0), (chkTextArea.Checked ? 1 : 0), (ddlFields.Enabled ? ddlFields.SelectedItem.Value : ""), txtValue.Text, (chkEmpty.Checked ? 1 : 0), txtSuffix.Text, (oDesign.GetQuestions(intPhase, 0).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
            else
                oDesign.UpdateQuestion(intID, intPhase, txtQuestion.Text, txtSummary.Text, (chkSummary.Checked ? 1 : 0), (chkMnemonic.Checked ? 1 : 0), (chkCostCenter.Checked ? 1 : 0), (chkUserSI.Checked ? 1 : 0), (chkUserDTG.Checked ? 1 : 0), (chkGridBackup.Checked ? 1 : 0), (chkBackupExclusions.Checked ? 1 : 0), (chkGridMaintenance.Checked ? 1 : 0), (chkStorageLuns.Checked ? 1 : 0), (chkAccounts.Checked ? 1 : 0), (chkDate.Checked ? 1 : 0), (chkLocation.Checked ? 1 : 0), (chkDropDown.Checked ? 1 : 0), (chkCheckBox.Checked ? 1 : 0), (chkRadio.Checked ? 1 : 0), (chkTextBox.Checked ? 1 : 0), (chkTextArea.Checked ? 1 : 0), (ddlFields.Enabled ? ddlFields.SelectedItem.Value : ""), txtValue.Text, (chkEmpty.Checked ? 1 : 0), txtSuffix.Text, (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oDesign.UpdateQuestionOrder(intId, intCount);
                }
            }
            // Save Dynamic Responses (SHOW)
            oDesign.DeleteShow(intID, 0);
            foreach (TreeNode oNodeP in oTreeQuestionsShow.Nodes)
                foreach (TreeNode oNodeQ in oNodeP.ChildNodes)
                    SaveShow(oNodeQ, 0);
            oDesign.UpdateQuestionShow(intID, (radShowAll.Checked ? 1 : 0), (radShowAny.Checked ? 1 : 0));
            // Save Dynamic Responses (HIDE)
            oDesign.DeleteShowResponse(intID, 1);
            foreach (TreeNode oNodeP in oTreeQuestionsHide.Nodes)
                foreach (TreeNode oNodeQ in oNodeP.ChildNodes)
                    SaveShow(oNodeQ, 1);
            oDesign.UpdateQuestionHide(intID, (radHideAll.Checked ? 1 : 0), (radHideAny.Checked ? 1 : 0));
        }
        private void SaveShow(TreeNode oParent, int _disabled)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oDesign.AddShow(Int32.Parse(oNode.Value), intID, _disabled);
            }
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oDesign.DeleteQuestion(intID);
            Response.Redirect(Request.Path);
        }
    }
}
