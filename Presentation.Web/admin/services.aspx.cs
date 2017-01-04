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
    public partial class services : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Services oService;
        protected RequestItems oRequestItem;
        protected Customized oCustomized;
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
            oService = new Services(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            int intApplication = 0;
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["parent"] == null)
                    LoadServices(0);
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
                if (intID > 0 && !IsPostBack)
                {
                    LoadLists();
                    DataSet ds = oService.Get(intID);
                    hdnId.Value = intID.ToString();
                    hdnOrderId.Value = Request.QueryString["orderid"];
                    txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                    int intItem = 0;
                    if (Int32.TryParse(ds.Tables[0].Rows[0]["itemid"].ToString(), out intItem) == true) 
                    {
                        lblItem.Text = oRequestItem.GetItemName(intItem);
                        intApplication = oRequestItem.GetItemApplication(intItem);
                    }
                    if (lblItem.Text == "" || intItem == 0)
                        lblItem.Text = "No Item";
                    hdnItem.Value = intItem.ToString();
                    int intDocument = 0;
                    if (Int32.TryParse(ds.Tables[0].Rows[0]["docid"].ToString(), out intDocument) == true)
                        lblDocument.Text = oCustomized.GetDocumentRepository(intDocument, "name");
                    if (lblDocument.Text == "" || intDocument == 0)
                        lblDocument.Text = "No Document";
                    hdnDocument.Value = intDocument.ToString();
                    ddlType.SelectedValue = ds.Tables[0].Rows[0]["typeid"].ToString();
                    chkShow.Checked = (ds.Tables[0].Rows[0]["show"].ToString() == "1");
                    chkProject.Checked = (ds.Tables[0].Rows[0]["project"].ToString() == "1");
                    txtHours.Text = ds.Tables[0].Rows[0]["hours"].ToString();
                    txtSLA.Text = ds.Tables[0].Rows[0]["sla"].ToString();
                    chkCanAuto.Checked = (ds.Tables[0].Rows[0]["can_automate"].ToString() == "1");
                    chkStatement.Checked = (ds.Tables[0].Rows[0]["statement"].ToString() == "1");
                    chkUpload.Checked = (ds.Tables[0].Rows[0]["upload"].ToString() == "1");
                    chkExpedite.Checked = (ds.Tables[0].Rows[0]["expedite"].ToString() == "1");
                    txtRRPath.Text = ds.Tables[0].Rows[0]["rr_path"].ToString();
                    txtWMPath.Text = ds.Tables[0].Rows[0]["wm_path"].ToString();
                    txtCPPath.Text = ds.Tables[0].Rows[0]["cp_path"].ToString();
                    txtCAPath.Text = ds.Tables[0].Rows[0]["ca_path"].ToString();
                    chkRejection.Checked = (ds.Tables[0].Rows[0]["rejection"].ToString() == "1");
                    chkAutomate.Checked = (ds.Tables[0].Rows[0]["automate"].ToString() == "1");
                    chkHours.Checked = (ds.Tables[0].Rows[0]["disable_hours"].ToString() == "1");
                    chkQuantityDevice.Checked = (ds.Tables[0].Rows[0]["quantity_is_device"].ToString() == "1");
                    chkQuantityMultiple.Checked = (ds.Tables[0].Rows[0]["multiple_quantity"].ToString() == "1");
                    chkNotifyPC.Checked = (ds.Tables[0].Rows[0]["notify_pc"].ToString() == "1");
                    chkNotifyClient.Checked = (ds.Tables[0].Rows[0]["notify_client"].ToString() == "1");
                    chkDisable.Checked = (ds.Tables[0].Rows[0]["disable_customization"].ToString() == "1");
                    chkTask.Checked = (ds.Tables[0].Rows[0]["tasks"].ToString() == "1");
                    txtEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
                    chkSameTime.Checked = (ds.Tables[0].Rows[0]["sametime"].ToString() == "1");
                    chkNotifyGreen.Checked = (ds.Tables[0].Rows[0]["notify_green"].ToString() == "1");
                    chkNotifyYellow.Checked = (ds.Tables[0].Rows[0]["notify_yellow"].ToString() == "1");
                    chkNotifyRed.Checked = (ds.Tables[0].Rows[0]["notify_red"].ToString() == "1");
                    chkWorkflow.Checked = (ds.Tables[0].Rows[0]["workflow"].ToString() == "1");
                    txtWorkflow.Text = ds.Tables[0].Rows[0]["workflow_title"].ToString();
                    chkRestrictions.Checked = (ds.Tables[0].Rows[0]["is_restricted"].ToString() == "1");
                    chkTitleOverrride.Checked = (ds.Tables[0].Rows[0]["title_override"].ToString() == "1");
                    txtTitleName.Text = ds.Tables[0].Rows[0]["title_name"].ToString();
                    chkNoSlider.Checked = (ds.Tables[0].Rows[0]["no_slider"].ToString() == "1");
                    chkHideSLA.Checked = (ds.Tables[0].Rows[0]["hide_sla"].ToString() == "1");
                    chkApproval.Checked = (ds.Tables[0].Rows[0]["approval"].ToString() == "1");
                    chkManagerApprove.Checked = (ds.Tables[0].Rows[0]["manager_approval"].ToString() == "1");
                    ddlWorkflowAssignment.SelectedValue = ds.Tables[0].Rows[0]["workflow_userid"].ToString();
                    txtNotifyComplete.Text = ds.Tables[0].Rows[0]["notify_complete"].ToString();
                    chkWorkflowConnectable.Checked = (ds.Tables[0].Rows[0]["workflow_connect"].ToString() == "1");
                    chkWorkflowSameTechnician.Checked = (ds.Tables[0].Rows[0]["same_technician"].ToString() == "1");
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                    btnAdd.Text = "Update";
                }
            }
            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnOrderId.ClientID + "','" + hdnOrder.ClientID + "&type=SERVICES" + "',false,400,400);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service?');");
            btnItem.Attributes.Add("onclick", "return OpenWindow('ITEMBROWSER','" + hdnItem.ClientID + "','&control=" + hdnItem.ClientID + "&controltext=" + lblItem.ClientID + "',false,400,600);");
            btnDocument.Attributes.Add("onclick", "return OpenWindow('DOCUMENTBROWSER','" + hdnDocument.ClientID + "','&control=" + hdnDocument.ClientID + "&controltext=" + lblDocument.ClientID + "&applicationid=" + intApplication.ToString() + "',false,400,600);");
            btnImage.Attributes.Add("onclick", "return OpenWindow('IMAGEPATH','','" + txtImage.ClientID + "',false,500,550);");
            btnRoles.Attributes.Add("onclick", "return OpenWindow('SERVICEDETAILROLES','" + hdnId.ClientID + "','',false,'500',600);");
            btnLocation.Attributes.Add("onclick", "return OpenWindow('SERVICELOCATIONS','" + hdnId.ClientID + "','',false,'500',600);");
            btnRRBrowse.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtRRPath.ClientID + "','',false,400,600);");
            btnWMBrowse.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtWMPath.ClientID + "','',false,400,600);");
            btnCPBrowse.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtCPPath.ClientID + "','',false,400,600);");
            btnCABrowse.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtCAPath.ClientID + "','',false,400,600);");
            btnUsers.Attributes.Add("onclick", "return OpenWindow('SERVICE_USERS','" + hdnId.ClientID + "','',false,'500',500);");
            btnReport.Attributes.Add("onclick", "return OpenServiceReport('/frame/report.aspx?r=" + oFunction.encryptQueryString("419") + "');");
            if (Request.QueryString["expand"] != null)
            {
                btnExpand.Text = "Minimize All";
                oTreeview.ExpandAll();
            }
            else
            {
                btnExpand.Text = "Expand All";
                oTreeview.ExpandDepth = 1;
            }
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadLists()
        {
            DataSet ds = oService.GetTypes(1);
            ddlType.DataTextField = "name";
            ddlType.DataValueField = "typeid";
            ddlType.DataSource = ds;
            ddlType.DataBind();
            ddlType.Items.Insert(0, new ListItem("-- NONE --", "0"));

            oService.LoadWorkflowUsers(oService.GetWorkflowsReceive(intID), ref ddlWorkflowAssignment);
        }
        private void LoadServices(int _parent)
        {
            panView.Visible = true;
            TreeNode oNode = new TreeNode();
            oNode.Text = "ClearView Services";
            oNode.ToolTip = "ClearView Services";
            oNode.ImageUrl = "/images/folder.gif";
            oNode.SelectAction = TreeNodeSelectAction.Expand;
            oTreeview.Nodes.Add(oNode);
            LoadServiceFolders(0, oNode);
        }
        private void LoadServiceFolders(int _parent, TreeNode oParent)
        {
            DataSet ds = oService.GetFolders(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadServiceFolders(Int32.Parse(dr["id"].ToString()), oNode);
                LoadServices(Int32.Parse(dr["id"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Service";
                oNew.ToolTip = "Add Service";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = Request.Path + "?parent=" + dr["id"].ToString();
                oNode.ChildNodes.Add(oNew);
            }
        }
        private void LoadServices(int _parent, TreeNode oParent)
        {
            DataSet ds = oService.Gets(_parent, 0, 0, 1, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = Request.Path + "?id=" + dr["serviceid"].ToString() + "&orderid=" + dr["id"].ToString();
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intItem = Int32.Parse(Request.Form[hdnItem.UniqueID]);
            int intDocument = Int32.Parse(Request.Form[hdnDocument.UniqueID]);
            if (Request.Form[hdnId.UniqueID] == "0")
            {
                int intService = oService.Add(txtName.Text, txtDescription.Text, txtImage.Text, intItem, Int32.Parse(ddlType.SelectedItem.Value), (chkShow.Checked ? 1 : 0), (chkProject.Checked ? 1 : 0), 0, double.Parse(txtHours.Text), double.Parse(txtSLA.Text), (chkApproval.Checked ? 1 : 0), (chkCanAuto.Checked ? 1 : 0), (chkStatement.Checked ? 1 : 0), (chkUpload.Checked ? 1 : 0), (chkExpedite.Checked ? 1 : 0), txtRRPath.Text, txtWMPath.Text, txtCPPath.Text, txtCAPath.Text, (chkRejection.Checked ? 1 : 0), (chkAutomate.Checked ? 1 : 0), (chkHours.Checked ? 1 : 0), (chkQuantityDevice.Checked ? 1 : 0), (chkQuantityMultiple.Checked ? 1 : 0), (chkNotifyPC.Checked ? 1 : 0), (chkNotifyClient.Checked ? 1 : 0), (chkDisable.Checked ? 1 : 0), (chkTask.Checked ? 1 : 0), txtEmail.Text, (chkSameTime.Checked ? 1 : 0), (chkNotifyGreen.Checked ? 1 : 0), (chkNotifyYellow.Checked ? 1 : 0), (chkNotifyRed.Checked ? 1 : 0), (chkWorkflow.Checked ? 1 : 0), txtWorkflow.Text, (chkTitleOverrride.Checked ? 1 : 0), txtTitleName.Text, (chkNoSlider.Checked ? 1 : 0), (chkHideSLA.Checked ? 1 : 0), (chkRestrictions.Checked ? 1 : 0), (chkManagerApprove.Checked ? 1 : 0), txtNotifyComplete.Text, Int32.Parse(ddlWorkflowAssignment.SelectedItem.Value), intDocument, (chkWorkflowConnectable.Checked ? 1 : 0), (chkWorkflowSameTechnician.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
                int intParent = Int32.Parse(Request.Form[hdnParent.UniqueID]);
                oService.AddFolders(intService, intParent, oService.Gets(intParent, 0, 0, 1, 0).Tables[0].Rows.Count + 1);
            }
            else
            {
                int intService = Int32.Parse(Request.Form[hdnId.UniqueID]);
                oService.Update(intService, txtName.Text, txtDescription.Text, txtImage.Text, intItem, Int32.Parse(ddlType.SelectedItem.Value), (chkShow.Checked ? 1 : 0), (chkProject.Checked ? 1 : 0), 0, double.Parse(txtHours.Text), double.Parse(txtSLA.Text), (chkApproval.Checked ? 1 : 0), (chkCanAuto.Checked ? 1 : 0), (chkStatement.Checked ? 1 : 0), (chkUpload.Checked ? 1 : 0), (chkExpedite.Checked ? 1 : 0), txtRRPath.Text, txtWMPath.Text, txtCPPath.Text, txtCAPath.Text, (chkRejection.Checked ? 1 : 0), (chkAutomate.Checked ? 1 : 0), (chkHours.Checked ? 1 : 0), (chkQuantityDevice.Checked ? 1 : 0), (chkQuantityMultiple.Checked ? 1 : 0), (chkNotifyPC.Checked ? 1 : 0), (chkNotifyClient.Checked ? 1 : 0), (chkDisable.Checked ? 1 : 0), (chkTask.Checked ? 1 : 0), txtEmail.Text, (chkSameTime.Checked ? 1 : 0), (chkNotifyGreen.Checked ? 1 : 0), (chkNotifyYellow.Checked ? 1 : 0), (chkNotifyRed.Checked ? 1 : 0), (chkWorkflow.Checked ? 1 : 0), txtWorkflow.Text, (chkTitleOverrride.Checked ? 1 : 0), txtTitleName.Text, (chkNoSlider.Checked ? 1 : 0), (chkHideSLA.Checked ? 1 : 0), (chkRestrictions.Checked ? 1 : 0), (chkManagerApprove.Checked ? 1 : 0), txtNotifyComplete.Text, Int32.Parse(ddlWorkflowAssignment.SelectedItem.Value), intDocument, (chkWorkflowConnectable.Checked ? 1 : 0), (chkWorkflowSameTechnician.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0), dsnAsset);
            }
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oService.UpdateFolders(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oService.Delete(intID);
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnExpand_Click(Object Sender, EventArgs e)
        {
            if (Request.QueryString["expand"] != null)
                Response.Redirect(Request.Path);
            else
                Response.Redirect(Request.Path + "?expand=true");
        }
    }
}
