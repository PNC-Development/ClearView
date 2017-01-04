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
    public partial class servername : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intPlatform = Int32.Parse(ConfigurationManager.AppSettings["ServerPlatformID"]);
        protected Pages oPage;
        protected Locations oLocation;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected Servers oServer;
        protected ServerName oServerName;
        protected OperatingSystems oOperatingSystems;
        protected ServicePacks oServicePacks;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Variables oVariable;
        protected Mnemonic oMnemonic;
        protected bool boolUsePNCNaming = (ConfigurationManager.AppSettings["USE_PNC_NAMING"] == "1");
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
            oOperatingSystems = new OperatingSystems(intProfile, dsn);
            oServicePacks = new ServicePacks(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oMnemonic = new Mnemonic(intProfile, dsn);

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            string strName = "";
            if (Request.QueryString["type"] != null && Request.QueryString["type"] != "" && Request.QueryString["save"] != null && Request.QueryString["save"] != "")
            {
                if (Request.QueryString["type"] == "NCB")
                {
                    strName = oServerName.GetName(Int32.Parse(Request.QueryString["save"]), 0);
                    if (strName != "DENIED")
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Server Name Generated Successfully!\\n\\nServer Name: " + strName + "\\n\\nThis name has been reserved to you - to release this reservation, click on My Server Names in the left menu.');window.navigate('" + oPage.GetFullLink(intPage) + "?type=" + Request.QueryString["type"] + "&dr=" + (Request.QueryString["dr"] == null ? "" : Request.QueryString["dr"]) + "&id=" + Request.QueryString["save"] + "');<" + "/" + "script>");
                }
                else if (Request.QueryString["type"] == "PNC")
                {
                    strName = oServerName.GetNameFactory(Int32.Parse(Request.QueryString["save"]), 0);
                    if (strName != "DENIED")
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Server Name Generated Successfully!\\n\\nServer Name: " + strName + "\\n\\nThis name has been reserved to you - to release this reservation, click on My Server Names in the left menu.');window.navigate('" + oPage.GetFullLink(intPage) + "?type=" + Request.QueryString["type"] + "&dr=" + (Request.QueryString["dr"] == null ? "" : Request.QueryString["dr"]) + "&id=" + Request.QueryString["save"] + "');<" + "/" + "script>");
                }
                else
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Invalid TYPE - server name generation failed!');window.navigate('" + oPage.GetFullLink(intPage) + "');<" + "/" + "script>");
            }
            if (Request.QueryString["type"] != null && Request.QueryString["type"] != "" && Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                if (Request.QueryString["type"] == "NCB")
                {
                    radType.SelectedValue = "NCB";
                    strName = oServerName.GetName(Int32.Parse(Request.QueryString["id"]), 0);
                }
                else if (Request.QueryString["type"] == "PNC")
                {
                    radType.SelectedValue = "PNC";
                    strName = oServerName.GetNameFactory(Int32.Parse(Request.QueryString["id"]), 0);
                }
                else
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Invalid TYPE - server name generation failed!');window.navigate('" + oPage.GetFullLink(intPage) + "');<" + "/" + "script>");
            }
            if (!IsPostBack)
            {
                btnSubmit.Enabled = false;
                string strAdditional = ShowForm();
                LoadLists();
                btnSubmit.Attributes.Add("onclick", "return EnsureFunction('" + ddlApplication.ClientID + "','hdnComponents')" +
                    " && ValidateDropDown('" + ddlCluster.ClientID + "','Please make a selection for the clustering options')" +
                    strAdditional +
                    " && ValidateText('" + txtName.ClientID + "','Please enter a name for this server name request')" +
                    " && ProcessButton(this)" +
                    ";");
                btnParent.Attributes.Add("onclick", "return OpenLocations('" + hdnParent.ClientID + "','" + txtParent.ClientID + "');");
                ddlLocation.Attributes.Add("onchange", "WaitDDL('" + divLocation.ClientID + "');");
                ddlOS.Attributes.Add("onchange", "WaitDDL('" + divOS.ClientID + "');");
                ddlSP.Attributes.Add("onchange", "WaitDDL('" + divSP.ClientID + "');");
                ddlClass.Attributes.Add("onchange", "WaitDDL('" + divClass.ClientID + "');");
                ddlEnvironment.Attributes.Add("onchange", "WaitDDL('" + divEnvironment.ClientID + "');");
                btnDuplicate.Attributes.Add("onclick", "return confirm('Are you sure you want to genearate another server name?\\n\\nThis new name will have the same settings as your previous request.');");
                txtMnemonic.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'500','195','" + divMnemonic.ClientID + "','" + lstMnemonic.ClientID + "','" + hdnMnemonic.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_mnemonics.aspx',2);");
                lstMnemonic.Attributes.Add("ondblclick", "AJAXClickRow();");
            }
            if (strName != "" && strName != "DENIED")
            {
                lblName.Text = strName;
                txtName.Enabled = true;
                txtName.Text = "Enter a new nickname HERE";
                txtName.Attributes.Add("onfocus", "ChangeDefaultName(this);");
                panName.Visible = true;
            }
        }
        private string ShowForm()
        {
            string strAdditional = "";
            if (Request.QueryString["type"] != null && Request.QueryString["type"] != "")
            {
                if (Request.QueryString["type"] == "NCB")
                {
                    radType.SelectedValue = "NCB";
                    divNCB.Visible = true;
                    if (Request.QueryString["dr"] != null && Request.QueryString["dr"] != "")
                    {
                        if (Request.QueryString["dr"] == "Yes")
                        {
                            radDR.SelectedValue = "Yes";
                            divNCBYes.Visible = true;
                        }
                        else
                        {
                            radDR.SelectedValue = "No";
                            divContinue.Visible = true;
                        }
                    }
                }
                else if (Request.QueryString["type"] == "PNC")
                {
                    radType.SelectedValue = "PNC";
                    divContinue.Visible = true;
                    trMnemonic.Visible = true;
                    strAdditional = " && ValidateHidden0('" + hdnMnemonic.ClientID + "','" + txtMnemonic.ClientID + "','Please enter the mnemonic of this device\\n\\n(Start typing and a list will be presented...)')";
                }
                else
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Invalid TYPE - server name generation failed!');window.navigate('" + oPage.GetFullLink(intPage) + "');<" + "/" + "script>");
            }
            return strAdditional;
        }
        private void LoadLists()
        {
            ddlLocation.Enabled = true;
            ddlLocation.DataValueField = "id";
            ddlLocation.DataTextField = "name";
            ddlLocation.DataSource = oLocation.GetAddressCommon();
            ddlLocation.DataBind();
            ddlLocation.Items.Add(new ListItem("-- NOT LISTED --", "-1"));
            ddlLocation.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            int intAddress = 0;
            if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                intAddress = Int32.Parse(Request.QueryString["aid"]);
            if (oLocation.GetAddress(intAddress).Tables[0].Rows.Count > 0)
            {
                ddlLocation.SelectedValue = intAddress.ToString();
                hdnParent.Value = intAddress.ToString();
                if (ddlLocation.SelectedItem.Value == "0")
                {
                    ddlLocation.SelectedValue = "-1";
                    panLocation.Visible = true;
                    txtParent.Text = oLocation.GetFull(intAddress);
                }
                ddlOS.Enabled = true;
                ddlOS.DataValueField = "id";
                ddlOS.DataTextField = "name";
                ddlOS.DataSource = oOperatingSystems.Gets(0, 1);
                ddlOS.DataBind();
                ddlOS.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                int intOS = 0;
                if (Request.QueryString["osid"] != null && Request.QueryString["osid"] != "")
                    intOS = Int32.Parse(Request.QueryString["osid"]);
                if (oOperatingSystems.Get(intOS).Tables[0].Rows.Count > 0)
                {
                    ddlOS.SelectedValue = intOS.ToString();
                    ddlSP.Enabled = true;
                    ddlSP.DataValueField = "id";
                    ddlSP.DataTextField = "name";
                    ddlSP.DataSource = oOperatingSystems.GetServicePack(intOS);
                    ddlSP.DataBind();
                    ddlSP.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                    int intSP = 0;
                    if (Request.QueryString["spid"] != null && Request.QueryString["spid"] != "")
                        intSP = Int32.Parse(Request.QueryString["spid"]);
                    if (oServicePacks.Get(intSP).Tables[0].Rows.Count > 0)
                    {
                        ddlSP.SelectedValue = intSP.ToString();
                        ddlClass.Enabled = true;
                        ddlClass.DataValueField = "id";
                        ddlClass.DataTextField = "name";
                        ddlClass.DataSource = oServerName.GetCodeClasses(intAddress);
                        ddlClass.DataBind();
                        lblConfig.Visible = (ddlClass.Items.Count == 0);
                        ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        int intClass = 0;
                        if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
                            intClass = Int32.Parse(Request.QueryString["cid"]);
                        if (oClass.Get(intClass).Tables[0].Rows.Count > 0)
                        {
                            ddlClass.SelectedValue = intClass.ToString();
                            ddlEnvironment.Enabled = true;
                            Environments oEnvironment = new Environments(intProfile, dsn);
                            ddlEnvironment.DataValueField = "id";
                            ddlEnvironment.DataTextField = "name";
                            ddlEnvironment.DataSource = oServerName.GetCodeEnvironments(intAddress, intClass);
                            ddlEnvironment.DataBind();
                            lblConfig.Visible = (lblConfig.Visible || ddlEnvironment.Items.Count == 0);
                            ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                            int intEnv = 0;
                            if (Request.QueryString["eid"] != null && Request.QueryString["eid"] != "")
                                intEnv = Int32.Parse(Request.QueryString["eid"]);
                            if (oEnvironment.Get(intEnv).Tables[0].Rows.Count > 0)
                            {
                                ddlEnvironment.SelectedValue = intEnv.ToString();
                                // Load Models
                                trModels.Visible = true;
                                LoadTypes(oTreeModels);
                                int intModel = 0;
                                if (Request.QueryString["mid"] != null && Request.QueryString["mid"] != "")
                                    intModel = Int32.Parse(Request.QueryString["mid"]);
                                if (oModelsProperties.Get(intModel).Tables[0].Rows.Count > 0)
                                {
                                    lblModel.Text = oModelsProperties.Get(intModel, "name");
                                    ExpandTreeview(oTreeModels, intModel.ToString(), true);
                                    // Load Applications
                                    ddlApplication.DataValueField = "id";
                                    ddlApplication.DataTextField = "name";
                                    ddlApplication.DataSource = oServerName.GetApplicationPermissionsOS(intOS);
                                    ddlApplication.DataBind();
                                    ddlApplication.Items.Insert(0, new ListItem("-- NONE --", "0"));
                                    // Load Components
                                    panComponents.Visible = true;
                                    frmComponents.Attributes.Add("src", "/frame/ondemand/config_server_components.aspx?cid=" + intClass.ToString() + "&eid=" + intEnv.ToString() + "&mid=" + intModel.ToString() + "&osid=" + intOS.ToString() + "&spid=" + intSP.ToString());
                                    // Load Clustering
                                    ddlCluster.Items.Add(new ListItem("-- SELECT --", "0"));
                                    ddlCluster.Items.Add(new ListItem("Node of a Cluster (Example: NSQ)", "1"));
                                    if (oOperatingSystems.Get(intOS, "cluster_name") == "1")
                                        ddlCluster.Items.Add(new ListItem("Cluster Name (Example: CLU)", "2"));
                                    ddlCluster.Items.Add(new ListItem("Cluster Instance Name (Example: CSQ)", "3"));
                                    ddlCluster.Items.Add(new ListItem("None of the Above", "4"));
                                    btnSubmit.Enabled = true;
                                }
                                else
                                {
                                    lblModel.Text = "<i>Please select a Model</i>";
                                    ddlApplication.Enabled = false;
                                    ddlApplication.Items.Insert(0, new ListItem("-- Please select a Model --", "0"));
                                    ddlCluster.Enabled = false;
                                    ddlCluster.Items.Insert(0, new ListItem("-- Please select a Model --", "0"));
                                }
                            }
                            else
                            {
                                lblModel.Text = "<i>Please select an Environment</i>";
                                ddlApplication.Enabled = false;
                                ddlApplication.Items.Insert(0, new ListItem("-- Please select an Environment --", "0"));
                                ddlCluster.Enabled = false;
                                ddlCluster.Items.Insert(0, new ListItem("-- Please select an Environment --", "0"));
                            }
                        }
                        else
                        {
                            ddlEnvironment.Enabled = false;
                            ddlEnvironment.Items.Insert(0, new ListItem("-- Please select a Class --", "0"));
                            lblModel.Text = "<i>Please select a Class</i>";
                            ddlApplication.Enabled = false;
                            ddlApplication.Items.Insert(0, new ListItem("-- Please select a Class --", "0"));
                            ddlCluster.Enabled = false;
                            ddlCluster.Items.Insert(0, new ListItem("-- Please select a Class --", "0"));
                            txtName.Enabled = false;
                        }
                    }
                    else
                    {
                        ddlClass.Enabled = false;
                        ddlClass.Items.Insert(0, new ListItem("-- Please select a Service Pack --", "0"));
                        ddlEnvironment.Enabled = false;
                        ddlEnvironment.Items.Insert(0, new ListItem("-- Please select a Service Pack --", "0"));
                        lblModel.Text = "<i>Please select a Service Pack</i>";
                        ddlApplication.Enabled = false;
                        ddlApplication.Items.Insert(0, new ListItem("-- Please select a Service Pack --", "0"));
                        ddlCluster.Enabled = false;
                        ddlCluster.Items.Insert(0, new ListItem("-- Please select a Service Pack --", "0"));
                        txtName.Enabled = false;
                    }
                }
                else
                {
                    ddlSP.Enabled = false;
                    ddlSP.Items.Insert(0, new ListItem("-- Please select an Operating System --", "0"));
                    ddlClass.Enabled = false;
                    ddlClass.Items.Insert(0, new ListItem("-- Please select an Operating System --", "0"));
                    ddlEnvironment.Enabled = false;
                    ddlEnvironment.Items.Insert(0, new ListItem("-- Please select an Operating System --", "0"));
                    lblModel.Text = "<i>Please select an Operating System</i>";
                    ddlApplication.Enabled = false;
                    ddlApplication.Items.Insert(0, new ListItem("-- Please select an Operating System --", "0"));
                    ddlCluster.Enabled = false;
                    ddlCluster.Items.Insert(0, new ListItem("-- Please select an Operating System --", "0"));
                    txtName.Enabled = false;
                }
            }
            else
            {
                ddlLocation.SelectedValue = intAddress.ToString();
                if (intAddress == -1)
                {
                    panLocation.Visible = true;
                    hdnParent.Value = "0";
                }
                ddlOS.Enabled = false;
                ddlOS.Items.Insert(0, new ListItem("-- Please select a Location --", "0"));
                ddlSP.Enabled = false;
                ddlSP.Items.Insert(0, new ListItem("-- Please select a Location --", "0"));
                ddlClass.Enabled = false;
                ddlClass.Items.Insert(0, new ListItem("-- Please select a Location --", "0"));
                ddlEnvironment.Enabled = false;
                ddlEnvironment.Items.Insert(0, new ListItem("-- Please select a Location --", "0"));
                lblModel.Text = "<i>Please select a Location</i>";
                ddlApplication.Enabled = false;
                ddlApplication.Items.Insert(0, new ListItem("-- Please select a Location --", "0"));
                ddlCluster.Enabled = false;
                ddlCluster.Items.Insert(0, new ListItem("-- Please select a Location --", "0"));
                txtName.Enabled = false;
            }
        }
        private void LoadTypes(TreeView oTree)
        {
            DataSet ds = oType.Gets(intPlatform, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTree.Nodes.Add(oNode);
                LoadModels(Int32.Parse(dr["id"].ToString()), oNode);
            }
            oTree.ExpandDepth = 0;
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadModels(int _typeid, TreeNode oParent)
        {
            DataSet ds = oModel.Gets(_typeid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadModelProperties(Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void LoadModelProperties(int _modelid, TreeNode oParent)
        {
            DataSet ds = oModelsProperties.GetModels(0, _modelid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                string strName = dr["name"].ToString();
                if (strName == "")
                    strName = dr["modelname"].ToString();
                oNode.Text = strName;
                oNode.ToolTip = strName;
                oNode.Value = dr["id"].ToString();
                oNode.NavigateUrl = oPage.GetFullLink(intPage) + "?type=" + Request.QueryString["type"] + "&dr=" + (Request.QueryString["dr"] == null ? "" : Request.QueryString["dr"]) + "&aid=" + Request.QueryString["aid"] + "&osid=" + Request.QueryString["osid"] + "&spid=" + Request.QueryString["spid"] + "&cid=" + Request.QueryString["cid"] + "&eid=" + Request.QueryString["eid"] + "&mid=" + dr["id"].ToString();
                //oNode.SelectAction = TreeNodeSelectAction.None;
                if (dr["id"].ToString() == Request.QueryString["mid"])
                {
                    oNode.ShowCheckBox = true;
                    oNode.Checked = true;
                    oNode.Selected = true;
                }
                oParent.ChildNodes.Add(oNode);
            }
        }
        private void ExpandTreeview(TreeView _tree, string _value, bool _check)
        {
            foreach (TreeNode _node in _tree.Nodes)
            {
                if (_node.Value == _value)
                {
                    if (_check == true)
                    {
                        _node.ShowCheckBox = true;
                        _node.Checked = true;
                    }
                    _node.Selected = true;
                }
                else if (_node.ChildNodes.Count > 0)
                {
                    bool boolExpand = ExpandNode(_node, _value, _check);
                    _node.Expanded = boolExpand;
                    if (boolExpand == true)
                        break;
                }
            }
        }
        private bool ExpandNode(TreeNode _parent, string _value, bool _check)
        {
            bool boolExpand = false;
            foreach (TreeNode _node in _parent.ChildNodes)
            {
                if (_node.Value == _value)
                {
                    if (_check == true)
                    {
                        _node.ShowCheckBox = true;
                        _node.Checked = true;
                    }
                    _node.Selected = true;
                    boolExpand = true;
                    break;
                }
                else if (_node.ChildNodes.Count > 0)
                {
                    boolExpand = ExpandNode(_node, _value, _check);
                    _node.Expanded = boolExpand;
                    if (boolExpand == true)
                        break;
                }
            }
            return boolExpand;
        }
        protected void radType_Change(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?type=" + radType.SelectedItem.Value);
        }
        protected void radDR_Change(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?type=" + Request.QueryString["type"] + "&dr=" + radDR.SelectedItem.Value);
        }
        protected void ddlLocation_Change(Object Sender, EventArgs e)
        {
            if (ddlLocation.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?type=" + Request.QueryString["type"] + "&dr=" + (Request.QueryString["dr"] == null ? "" : Request.QueryString["dr"]) + "&aid=" + ddlLocation.SelectedItem.Value);
            else if (ddlLocation.SelectedItem.Value == "-1")
                Response.Redirect(oPage.GetFullLink(intPage) + "?type=" + Request.QueryString["type"] + "&dr=" + (Request.QueryString["dr"] == null ? "" : Request.QueryString["dr"]) + "&aid=-1");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?type=" + Request.QueryString["type"] + "&dr=" + (Request.QueryString["dr"] == null ? "" : Request.QueryString["dr"]));
        }
        protected void ddlOS_Change(Object Sender, EventArgs e)
        {
            if (ddlOS.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?type=" + Request.QueryString["type"] + "&dr=" + (Request.QueryString["dr"] == null ? "" : Request.QueryString["dr"]) + "&aid=" + Request.QueryString["aid"] + "&osid=" + ddlOS.SelectedItem.Value);
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?type=" + Request.QueryString["type"] + "&dr=" + (Request.QueryString["dr"] == null ? "" : Request.QueryString["dr"]) + "&aid=" + Request.QueryString["aid"]);
        }
        protected void ddlSP_Change(Object Sender, EventArgs e)
        {
            if (ddlOS.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?type=" + Request.QueryString["type"] + "&dr=" + (Request.QueryString["dr"] == null ? "" : Request.QueryString["dr"]) + "&aid=" + Request.QueryString["aid"] + "&osid=" + Request.QueryString["osid"] + "&spid=" + ddlSP.SelectedItem.Value);
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?type=" + Request.QueryString["type"] + "&dr=" + (Request.QueryString["dr"] == null ? "" : Request.QueryString["dr"]) + "&aid=" + Request.QueryString["aid"] + "&osid=" + Request.QueryString["osid"]);
        }
        protected void ddlClass_Change(Object Sender, EventArgs e)
        {
            if (ddlClass.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?type=" + Request.QueryString["type"] + "&dr=" + (Request.QueryString["dr"] == null ? "" : Request.QueryString["dr"]) + "&aid=" + Request.QueryString["aid"] + "&osid=" + Request.QueryString["osid"] + "&spid=" + Request.QueryString["spid"] + "&cid=" + ddlClass.SelectedItem.Value);
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?type=" + Request.QueryString["type"] + "&dr=" + (Request.QueryString["dr"] == null ? "" : Request.QueryString["dr"]) + "&aid=" + Request.QueryString["aid"] + "&osid=" + Request.QueryString["osid"] + "&spid=" + Request.QueryString["spid"]);
        }
        protected void ddlEnvironment_Change(Object Sender, EventArgs e)
        {
            if (ddlClass.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?type=" + Request.QueryString["type"] + "&dr=" + (Request.QueryString["dr"] == null ? "" : Request.QueryString["dr"]) + "&aid=" + Request.QueryString["aid"] + "&osid=" + Request.QueryString["osid"] + "&spid=" + Request.QueryString["spid"] + "&cid=" + Request.QueryString["cid"] + "&eid=" + ddlEnvironment.SelectedItem.Value);
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?type=" + Request.QueryString["type"] + "&dr=" + (Request.QueryString["dr"] == null ? "" : Request.QueryString["dr"]) + "&aid=" + Request.QueryString["aid"] + "&osid=" + Request.QueryString["osid"] + "&spid=" + Request.QueryString["spid"] + "&cid=" + Request.QueryString["cid"]);
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            int intID = -100;
            string strError = "";
            string strPrefixError = "";
            bool boolNCB = (Request.QueryString["type"] == "NCB");
            bool boolPNC = (Request.QueryString["type"] == "PNC");
            string strFieldCode = "";
            string strFieldSpecific = "";
            if (boolNCB == true)
            {
                strFieldCode = "code";
            }
            else if (boolPNC == true)
            {
                strFieldCode = "factory_code";
                strFieldSpecific = "factory_code_specific";
            }

            string strComponentCode = "";
            string strComponentSpecific = "";
            if (Request.Form["hdnComponents"] != null)
            {
                string strComponents = Request.Form["hdnComponents"];
                if (strComponents != "")
                {
                    while (strComponents != "")
                    {
                        int intDetail = Int32.Parse(strComponents.Substring(0, strComponents.IndexOf("&")));
                        if (intDetail > 0)
                        {
                            int intComponent = 0;
                            if (Int32.TryParse(oServerName.GetComponentDetail(intDetail, "componentid"), out intComponent) == true)
                            {
                                string strCode = oServerName.GetComponent(intComponent, strFieldCode);
                                if (strPrefixError != "")
                                    strPrefixError += "\\n";
                                strPrefixError += " - " + oServerName.GetComponent(intComponent, "name");
                                if (strCode.Trim() != "")
                                {
                                    if (strFieldSpecific != "")
                                        strComponentSpecific = oServerName.GetComponent(intComponent, strFieldSpecific).Trim();
                                    strComponentCode = strCode.Trim();
                                    break;
                                }
                            }
                        }
                        strComponents = strComponents.Substring(strComponents.IndexOf("&") + 1);
                    }
                }
            }
            if (ddlApplication.SelectedIndex > 0)
                strPrefixError = " - " + ddlApplication.SelectedItem.Text;
            int intServerType = 0;
            if (ddlApplication.SelectedIndex > 0)
                intServerType = Int32.Parse(ddlApplication.SelectedItem.Value);
            string strPrefix = "";
            if (intServerType > 0)
            {
                if (strFieldCode != "")
                    strPrefix = oServerName.GetApplication(intServerType, strFieldCode).Trim();
                if (strFieldSpecific != "")
                    strComponentSpecific = oServerName.GetApplication(intServerType, strFieldSpecific).Trim();
            }
            else
                strPrefix = strComponentCode;
            strPrefix = strPrefix.ToUpper().Trim();
            if (strPrefix == "")
                strError = "The following component(s) or server type have not been configured for server naming...\\n\\n" + strPrefixError;
            else
            {
                int intOS = Int32.Parse(ddlOS.SelectedItem.Value);
                if (boolNCB == true)
                {
                    if (oOperatingSystems.IsMidrange(intOS) == true)
                        strPrefix = "X" + strPrefix.Substring(0, 2);
                    switch (ddlCluster.SelectedItem.Value)
                    {
                        case "1":
                            // NSQ = Node of a Cluster
                            if (strPrefix.StartsWith("X") == true)
                                strPrefix = "XN" + strPrefix[1].ToString();
                            else
                                strPrefix = "N" + strPrefix.Substring(0, 2);
                            break;
                        case "2":
                            // CLU = Cluster Name
                            strPrefix = "CLU";
                            break;
                        case "3":
                            // CSQ = Cluster Instance Name
                            if (strPrefix.StartsWith("X") == true)
                                strPrefix = "XC" + strPrefix[1].ToString();
                            else
                                strPrefix = "C" + strPrefix.Substring(0, 2);
                            break;
                    }
                    intID = oServerName.Add(Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(ddlEnvironment.SelectedItem.Value), Int32.Parse(Request.Form[hdnParent.UniqueID]), strPrefix, intProfile, txtName.Text, 1, dsnServiceEditor);
                }
                else if (boolPNC == true)
                {
                    string strOS = oOperatingSystems.Get(intOS, "factory_code");
                    if (strOS == "")
                        strError = "The selected OPERATING SYSTEM has not been configured for server naming...";
                    else
                    {
                        int intAddress = Int32.Parse(Request.QueryString["aid"]);
                        string strLocation = oLocation.GetAddress(intAddress, "factory_code");
                        if (strOS == "")
                            strError = "The selected LOCATION has not been configured for server naming...";
                        else
                        {
                            int intMnemonic = Int32.Parse(Request.Form[hdnMnemonic.UniqueID]);
                            string strMnemonic = oMnemonic.Get(intMnemonic, "factory_code");
                            if (strOS == "")
                                strError = "The selected MNEMONIC has not been configured for server naming...";
                            else
                            {
                                int intClass = Int32.Parse(Request.QueryString["cid"]);
                                int intEnv = Int32.Parse(Request.QueryString["eid"]);
                                string strEnvironment = oClass.Get(intClass, "factory_code");
                                if (strOS == "")
                                    strError = "The selected CLASS has not been configured for server naming...";
                                else
                                {
                                    // Get Server Function
                                    string strFunction = strComponentCode;
                                    if (strOS == "")
                                        strError = "The selected FUNCTION has not been configured for server naming...";
                                    else
                                    {
                                        // Get Specifics
                                        string strSpecific = strComponentSpecific;
                                        switch (ddlCluster.SelectedItem.Value)
                                        {
                                            case "1":
                                                // NSQ = Node of a Cluster
                                                strSpecific = "Z";
                                                break;
                                            case "2":
                                                // CLU = Cluster Name
                                                strSpecific = "";
                                                break;
                                            case "3":
                                                // CSQ = Cluster Instance Name
                                                strSpecific = "";
                                                break;
                                        }
                                        intID = oServerName.AddFactory(strOS, strLocation, strMnemonic, strEnvironment, intClass, intEnv, strFunction, strSpecific, intProfile, txtName.Text, dsnServiceEditor);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Redirect(intID, strError);
        }
        protected void btnDuplicate_Click(Object Sender, EventArgs e)
        {
            int intID = Int32.Parse(Request.QueryString["id"]);
            if (txtName.Text == "Enter a new nickname HERE")
                txtName.Text = "";
            if (Request.QueryString["type"] == "NCB")
                intID = oServerName.Add(intID, txtName.Text, intProfile, dsnServiceEditor);
            else if (Request.QueryString["type"] == "PNC")
                intID = oServerName.AddFactory(intID, txtName.Text, intProfile, dsnServiceEditor);
            else
                intID = -100;
            Redirect(intID, "");
        }
        private void Redirect(int intID, string strError)
        {
            if (strError == "")
            {
                if (intID == -100)
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Server Name Generated Failed!\\n\\nInvalid TYPE configuration.');<" + "/" + "script>");
                else if (intID == -1)
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Server Name Generated Failed!\\n\\nAll available server names are in use for the criteria specified - please report this problem to your ClearView administrator.');<" + "/" + "script>");
                else
                    Response.Redirect(oPage.GetFullLink(intPage) + "?type=" + Request.QueryString["type"] + "&dr=" + (Request.QueryString["dr"] == null ? "" : Request.QueryString["dr"]) + "&save=" + intID.ToString());
            }
            else
                Page.ClientScript.RegisterStartupScript(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Server Name Generated Failed!\\n\\n" + strError + "\\n\\nPlease report this problem to your ClearView administrator" + "');<" + "/" + "script>");
        }
    }
}