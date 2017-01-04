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
    public partial class asset_switches : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected Asset oAsset;
        protected Environments oEnvironment;
        protected Classes oClass;
        protected RacksNew oRacksNew;
        protected Platforms oPlatform;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected int intProfile;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/asset/asset_switches.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oAsset = new Asset(intProfile, dsnAsset, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oRacksNew = new RacksNew(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            if (!IsPostBack)
                LoadLists();
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["parent"] == null)
                    LoadSwitches();
                else
                {
                    panAdd.Visible = true;
                    int intParent = Int32.Parse(Request.QueryString["parent"]);
                    if (!IsPostBack)
                    {
                        hdnParent.Value = intParent.ToString();
                        if (intParent > 0)
                            lblParent.Text = oRacksNew.Get(intParent, "rack");
                        else
                            lblParent.Text = "** Please Select **";
                        txtBlades.Text = "0";
                        txtPorts.Text = "0";
                    }
                }
            }
            else
            {
                panAdd.Visible = true;
                intID = Int32.Parse(Request.QueryString["id"]);
                if (!IsPostBack)
                {
                    DataSet ds = oAsset.GetSwitch(intID);
                    int intParent = Int32.Parse(ds.Tables[0].Rows[0]["rackid"].ToString());
                    hdnParent.Value = intParent.ToString();
                    txtSerial.Text = ds.Tables[0].Rows[0]["serial"].ToString();
                    txtAsset.Text = ds.Tables[0].Rows[0]["asset"].ToString();
                    int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
                    hdnModel.Value = intModel.ToString();
                    int intModelParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                    int intType = oModel.GetType(intModelParent);
                    int intPlatform = oType.GetPlatform(intType);
                    if (intPlatform > 0)
                    {
                        ddlPlatform.SelectedValue = intPlatform.ToString();
                        // Load Types
                        LoadDDL(ddlType, "name", "id", oType.Gets(intPlatform, 1), intType.ToString());
                        if (intType > 0)
                        {
                            // Load Models
                            LoadDDL(ddlModel, "name", "id", oModel.Gets(intType, 1), intModelParent.ToString());
                            if (intModelParent > 0)
                            {
                                // Load Model Properties
                                LoadDDL(ddlModelProperty, "name", "id", oModelsProperties.GetModels(1, intModelParent, 1), intModel.ToString());
                            }
                        }
                    }
                    txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    lblParent.Text = ds.Tables[0].Rows[0]["rack"].ToString();
                    txtRackPosition.Text = ds.Tables[0].Rows[0]["rackposition"].ToString();
                    txtBlades.Text = ds.Tables[0].Rows[0]["blades"].ToString();
                    txtPorts.Text = ds.Tables[0].Rows[0]["ports"].ToString();
                    chkIsIOS.Checked = (ds.Tables[0].Rows[0]["is_ios"].ToString() == "1");
                    chkIsNexus.Checked = (ds.Tables[0].Rows[0]["nexus"].ToString() == "1");
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                }
            }
            btnParent.Attributes.Add("onclick", "LoadLocationRoomRack('" + "rack" + "','" + hdnParent.ClientID + "', '', '','', '" + lblParent.ClientID + "');return false;");
            ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',1);");
            ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
            ddlPlatform.Attributes.Add("onchange", "PopulatePlatformTypes('" + ddlPlatform.ClientID + "','" + ddlType.ClientID + "','" + ddlModel.ClientID + "','" + ddlModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
            ddlType.Attributes.Add("onchange", "PopulatePlatformModels('" + ddlType.ClientID + "','" + ddlModel.ClientID + "','" + ddlModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
            ddlModel.Attributes.Add("onchange", "PopulatePlatformModelProperties('" + ddlModel.ClientID + "','" + ddlModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
            ddlModelProperty.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlModelProperty.ClientID + "','" + hdnModel.ClientID + "');");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
        }
        private void LoadLists()
        {
            LoadDDL(ddlClass, "name", "id", oClass.Gets(1), "");
            LoadDDL(ddlPlatform, "name", "platformid", oPlatform.Gets(1), "");
        }
        private void LoadDDL(DropDownList _ddl, string _text, string _value, DataSet _ds, string _id)
        {
            _ddl.Enabled = true;
            _ddl.DataTextField = _text;
            _ddl.DataValueField = _value;
            _ddl.DataSource = _ds;
            _ddl.DataBind();
            _ddl.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            if (_id != "")
                _ddl.SelectedValue = _id;
        }
        private void LoadSwitches()
        {
            panView.Visible = true;
            DataSet ds = oAsset.GetSwitchsByLocation(0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["location"].ToString();
                oNode.ToolTip = dr["location"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                LoadLocation(Int32.Parse(dr["locationid"].ToString()), oNode);
            }
            oTreeview.ExpandDepth = 1;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadLocation(int _locationid, TreeNode oParent)
        {
            DataSet ds = oAsset.GetSwitchsByLocation(_locationid);
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["rack"].ToString();
                oNode.ToolTip = dr["rack"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadRack(Int32.Parse(dr["rackid"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Switch";
                oNew.ToolTip = "Add Switch";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = Request.Path + "?parent=" + dr["rackid"].ToString();
                oNode.ChildNodes.Add(oNew);
            }
        }
        private void LoadRack(int _rackid, TreeNode oParent)
        {
            DataSet ds = oAsset.GetSwitchsByRack(_rackid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.NavigateUrl = Request.Path + "?id=" + dr["assetid"].ToString();
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intRack = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            int intModel = Int32.Parse(Request.Form[hdnModel.UniqueID]);
            int intClass = 0;
            Int32.TryParse(Request.Form[ddlClass.SelectedItem.Value], out intClass);
            int intEnv = 0;
            Int32.TryParse(Request.Form[hdnEnvironment.UniqueID], out intEnv);
            DataSet dsAsset = oAsset.Get(txtSerial.Text, intModel);
            if (dsAsset.Tables[0].Rows.Count > 0)
                intID = Int32.Parse(dsAsset.Tables[0].Rows[0]["id"].ToString());
            if (intID == 0)
            {
                intID = oAsset.Add("", intModel, txtSerial.Text, txtAsset.Text, (int)AssetStatus.InStock, intProfile, DateTime.Now, (int)AssetAttribute.Ok, 1);
                oAsset.UpdateStatus(intID, txtName.Text, (int)AssetStatus.InUse, intProfile, DateTime.Now);
                oAsset.AddSwitch(intID, intClass, intEnv, intRack, txtRackPosition.Text, Int32.Parse(txtBlades.Text), Int32.Parse(txtPorts.Text), 0, (chkIsIOS.Checked ? 1 : 0), (chkIsNexus.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            }
            else
            {
                oAsset.Update(intID, intModel, txtSerial.Text, txtAsset.Text, (int)AssetAttribute.Ok);
                oAsset.UpdateStatus(intID, txtName.Text, (int)AssetStatus.InUse, intProfile, DateTime.Now);
                oAsset.UpdateSwitch(intID, intClass, intEnv, intRack, txtRackPosition.Text, Int32.Parse(txtBlades.Text), Int32.Parse(txtPorts.Text), 0, (chkIsIOS.Checked ? 1 : 0), (chkIsNexus.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            }
            Response.Redirect(Request.Path);
        }
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?parent=0");
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oAsset.DeleteSwitch(intID);
            oAsset.Delete(intID);
            Response.Redirect(Request.Path);
        }
    }
}
