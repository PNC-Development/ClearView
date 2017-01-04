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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class design_responses : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected Design oDesign;
        protected OperatingSystems oOperatingSystem;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected Functions oFunction;
        protected Locations oLocation;
        protected ServerName oServerName;
        protected int intProfile;
        protected int intID = 0;
        protected string strDisabled = "";
        protected string strMenuTab1 = "";
        protected string strLocation = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = Request.Path;
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oDesign = new Design(intProfile, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oLocation = new Locations(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);

            int intQuestion = 0;
            int intQuestionDisplay = 0;
            int intPhase = 0;
            int intPhaseDisplay = 0;
            int intClassID = 0;
            int intOSID = 0;
            int intEnvironmentClassID = 0;
            int intEnvironmentID = 0;
            int intLocationID = 0;
            int intModelID = 0;
            int intComponentID = 0;
            int intApplicationID = 0;
            int intSubApplicationID = 0;
            if (!IsPostBack)
            {
                ddlFields.DataTextField = "name";
                ddlFields.DataValueField = "name";
                ddlFields.DataSource = oFunction.GetSystemTableColumns("cv_designs");
                ddlFields.DataBind();
                ddlFields.Items.Insert(0, new ListItem("-- SELECT --", ""));
            }
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["parent"] == null)
                    LoadTree(oTreeResponses);
                else
                {
                    panAdd.Visible = true;
                    intQuestion = Int32.Parse(Request.QueryString["parent"]);
                    string strField = oDesign.GetQuestion(intQuestion, "related_field");
                    if (strField != "")
                    {
                        ddlFields.SelectedValue = strField;
                        ddlFields.Enabled = false;
                    }
                    strDisabled = "<p><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> <b>This section is only available when updating an existing item</b></p>";
                    txtQuantityMin.Text = "0";
                    txtQuantityMax.Text = "0";
                    btnOrder.Enabled = false;
                    btnDelete.Enabled = false;
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
                    DataSet ds = oDesign.GetResponse(intID);
                    hdnId.Value = intID.ToString();
                    txtAdmin.Text = ds.Tables[0].Rows[0]["admin"].ToString();
                    txtResponse.Text = ds.Tables[0].Rows[0]["response"].ToString();
                    txtSummary.Text = ds.Tables[0].Rows[0]["summary"].ToString();
                    lblName.Text = txtAdmin.Text;
                    Int32.TryParse(ds.Tables[0].Rows[0]["questionid"].ToString(), out intQuestion);
                    Int32.TryParse(oDesign.GetQuestion(intQuestion, "display"), out intQuestionDisplay);
                    Int32.TryParse(oDesign.GetQuestion(intQuestion, "phaseid"), out intPhase);
                    Int32.TryParse(oDesign.GetPhase(intPhase, "display"), out intPhaseDisplay);
                    string strField = oDesign.GetQuestion(intQuestion, "related_field");
                    if (strField != "")
                    {
                        ddlFields.SelectedValue = strField;
                        ddlFields.Enabled = false;
                    }
                    txtValue.Enabled = false;
                    txtValue.Text = "Not Available";
                    if (Int32.TryParse(ds.Tables[0].Rows[0]["set_classid"].ToString(), out intClassID) == true && intClassID > 0)
                    {
                        chkClass.Checked = true;
                        trClass.Visible = true;
                    }
                    else if (Int32.TryParse(ds.Tables[0].Rows[0]["set_osid"].ToString(), out intOSID) == true && intOSID > 0)
                    {
                        chkOS.Checked = true;
                        trOS.Visible = true;
                    }
                    else if (Int32.TryParse(ds.Tables[0].Rows[0]["set_componentid"].ToString(), out intComponentID) == true && intComponentID > 0)
                    {
                        chkComponent.Checked = true;
                        trComponent1.Visible = true;
                        trComponent2.Visible = true;
                        ddlFields.Enabled = false;
                    }
                    else if (Int32.TryParse(ds.Tables[0].Rows[0]["set_environmentclassid"].ToString(), out intEnvironmentClassID) == true && intEnvironmentClassID > 0 && Int32.TryParse(ds.Tables[0].Rows[0]["set_environmentid"].ToString(), out intEnvironmentID) == true && intEnvironmentID > 0)
                    {
                        chkEnvironment.Checked = true;
                        trEnvironment1.Visible = true;
                        trEnvironment2.Visible = true;
                    }
                    else
                    {
                        txtValue.Enabled = true;
                        if (ddlFields.Enabled)
                            ddlFields.SelectedValue = ds.Tables[0].Rows[0]["related_field"].ToString();
                    }

                    if (Int32.TryParse(ds.Tables[0].Rows[0]["set_addressid"].ToString(), out intLocationID) == true && intLocationID > 0)
                    {
                        chkLocation.Checked = true;
                        trLocation.Visible = true;
                    }
                    if (Int32.TryParse(ds.Tables[0].Rows[0]["set_modelid"].ToString(), out intModelID) == true && intModelID > 0)
                    {
                        chkModel.Checked = true;
                        trModel.Visible = true;
                    }
                    if (Int32.TryParse(ds.Tables[0].Rows[0]["set_applicationid"].ToString(), out intApplicationID) == true && intApplicationID > 0)
                    {
                        chkInfrastructure.Checked = true;
                        trInfrastructure.Visible = true;
                        if (Int32.TryParse(ds.Tables[0].Rows[0]["set_subapplicationid"].ToString(), out intSubApplicationID) == true && intSubApplicationID > 0)
                            trInfrastructure2.Style["display"] = "inline";
                    }

                    chkUnder48.Checked = (ds.Tables[0].Rows[0]["is_under48"].ToString() == "1");
                    chkOver48.Checked = (ds.Tables[0].Rows[0]["is_over48"].ToString() == "1");
                    chkConfidenceLock.Checked = (ds.Tables[0].Rows[0]["is_confidence_lock"].ToString() == "1");
                    chkConfidenceUnlock.Checked = (ds.Tables[0].Rows[0]["is_confidence_unlock"].ToString() == "1");
                    chkException.Checked = (ds.Tables[0].Rows[0]["is_exception"].ToString() == "1");
                    if (txtValue.Enabled)
                        txtValue.Text = ds.Tables[0].Rows[0]["related_value"].ToString();
                    radShowAll.Checked = (ds.Tables[0].Rows[0]["show_all"].ToString() == "1");
                    radShowAny.Checked = (ds.Tables[0].Rows[0]["show_any"].ToString() == "1");
                    radShow.Checked = (radShowAll.Checked == false && radShowAny.Checked == false);
                    radHideAll.Checked = (ds.Tables[0].Rows[0]["hide_all"].ToString() == "1");
                    radHideAny.Checked = (ds.Tables[0].Rows[0]["hide_any"].ToString() == "1");
                    radHide.Checked = (radHideAll.Checked == false && radHideAny.Checked == false);
                    chkVisible.Checked = (ds.Tables[0].Rows[0]["visible"].ToString() == "1");
                    chkSelectIfOne.Checked = (ds.Tables[0].Rows[0]["select_if_one"].ToString() == "1");
                    txtQuantityMin.Text = ds.Tables[0].Rows[0]["quantity_min"].ToString();
                    txtQuantityMax.Text = ds.Tables[0].Rows[0]["quantity_max"].ToString();
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                    btnAdd.Text = "Save";
                    btnAddBack.Text = "Save & Return";

                    // Load Phase Restrictions
                    LoadRestrictions(oTreeRestrictionsDisabled, intPhaseDisplay, 1);

                    // Load Phase Restrictions
                    LoadRestrictions(oTreeRestrictionsEnabled, intPhaseDisplay, 0);

                    // Load Auto-Selections
                    LoadSelectionPhase(oTreeSelections);

                    //// Load Dynamic Questions
                    //LoadShowPhase(oTreeQuestions, intQuestionDisplay, intPhaseDisplay);

                    // Load Dynamic Responses
                    LoadShowResponses(oTreeResponsesShow, intQuestionDisplay, intPhaseDisplay, true);
                    LoadShowResponses(oTreeResponsesHide, intQuestionDisplay, intPhaseDisplay, false);

                    // Load Approvals
                    LoadApprovals(oTreeApprovals);

                    // Load Configuration
                    StringBuilder strConfiguration = new StringBuilder();
                    strConfiguration.Append("<p><b>Responses causing this option to be auto-selected:</b><br/>");
                    DataSet dsSelections = oDesign.GetSelectionSet(intID);
                    foreach (DataRow drSelection in dsSelections.Tables[0].Rows)
                    {
                        int intResponse = Int32.Parse(drSelection["responseid"].ToString());
                        strConfiguration.Append("&nbsp;&nbsp;-&nbsp;<a href=\"responses.aspx?id=" + intResponse.ToString() + "&menu_tab=3\">");
                        strConfiguration.Append(oDesign.GetResponse(intResponse, "response"));
                        strConfiguration.Append("</a><br/>");
                    }
                    strConfiguration.Append("</p>");
                    strConfiguration.Append("<p><b>Selecting this response causes the following questions to be SHOWN:</b><br/>");
                    dsSelections = oDesign.GetShowsRelated(intID, 0);
                    foreach (DataRow drSelection in dsSelections.Tables[0].Rows)
                    {
                        int intConfigQuestion = Int32.Parse(drSelection["questionid"].ToString());
                        strConfiguration.Append("&nbsp;&nbsp;-&nbsp;<a href=\"questions.aspx?id=" + intConfigQuestion.ToString() + "&menu_tab=2\">");
                        strConfiguration.Append(oDesign.GetQuestion(intConfigQuestion, "question"));
                        strConfiguration.Append("</a><br/>");
                    }
                    strConfiguration.Append("</p>");
                    strConfiguration.Append("<p><b>Selecting this response causes the following questions to be HIDDEN:</b><br/>");
                    dsSelections = oDesign.GetShowsRelated(intID, 1);
                    foreach (DataRow drSelection in dsSelections.Tables[0].Rows)
                    {
                        int intConfigQuestion = Int32.Parse(drSelection["questionid"].ToString());
                        strConfiguration.Append("&nbsp;&nbsp;-&nbsp;<a href=\"questions.aspx?id=" + intConfigQuestion.ToString() + "&menu_tab=2\">");
                        strConfiguration.Append(oDesign.GetQuestion(intConfigQuestion, "question"));
                        strConfiguration.Append("</a><br/>");
                    }
                    strConfiguration.Append("</p>");
                    strConfiguration.Append("<p><b>Selecting this response causes the following responses to be SHOWN:</b><br/>");
                    dsSelections = oDesign.GetShowResponse(intID, 0);
                    foreach (DataRow drSelection in dsSelections.Tables[0].Rows)
                    {
                        int intResponse = Int32.Parse(drSelection["responseid"].ToString());
                        strConfiguration.Append("&nbsp;&nbsp;-&nbsp;<a href=\"responses.aspx?id=" + intResponse.ToString() + "&menu_tab=5\">");
                        strConfiguration.Append(oDesign.GetResponse(intResponse, "response"));
                        strConfiguration.Append("</a><br/>");
                    }
                    strConfiguration.Append("</p>");
                    strConfiguration.Append("<p><b>Selecting this response causes the following responses to be HIDDEN:</b><br/>");
                    dsSelections = oDesign.GetShowResponse(intID, 1);
                    foreach (DataRow drSelection in dsSelections.Tables[0].Rows)
                    {
                        int intResponse = Int32.Parse(drSelection["responseid"].ToString());
                        strConfiguration.Append("&nbsp;&nbsp;-&nbsp;<a href=\"responses.aspx?id=" + intResponse.ToString() + "&menu_tab=5\">");
                        strConfiguration.Append(oDesign.GetResponse(intResponse, "response"));
                        strConfiguration.Append("</a><br/>");
                    }
                    strConfiguration.Append("</p>");
                    litConfiguration.Text = strConfiguration.ToString();
                }
            }
            if (!IsPostBack)
            {
                if (intQuestion > 0)
                    LoadQuestion(intQuestion, intPhase);
                LoadClass(intClassID, intEnvironmentClassID, intEnvironmentID);
                LoadOS(intOSID);
                LoadModel(intModelID);
                LoadComponents(intComponentID);
                LoadInfrastructure(intApplicationID, intSubApplicationID);
            }
            if (panAdd.Visible == true)
            {
                int intMenuTab = 0;
                if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                    intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                Tab oTab = new Tab("", intMenuTab, "divMenu1", true, false);
                oTab.AddTab("Properties", "");
                oTab.AddTab("Phase Restrictions", "");
                oTab.AddTab("Auto-Response Selections", "");
                oTab.AddTab("Dynamic Questions", "");
                oTab.AddTab("Dynamic Responses", "");
                oTab.AddTab("Approvals", "");
                oTab.AddTab("Configuration", "");
                strMenuTab1 = oTab.GetTabs();
            }
            if (intLocationID > 0)
                intLocation = intLocationID;
            ddlEnvironmentClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlEnvironmentClass.ClientID + "','" + ddlEnvironment.ClientID + "',1);");
            ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
            ddlProduct.Attributes.Add("onchange", "PopulateSoftwareComponents('" + ddlProduct.ClientID + "','" + ddlComponent.ClientID + "');");
            ddlComponent.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlComponent.ClientID + "','" + hdnComponent.ClientID + "');");
            strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, intLocation, true, "ddlCommon");
            hdnLocation.Value = intLocation.ToString();
            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + ddlQuestion.ClientID + "','" + hdnOrder.ClientID + "&type=D_RESPONSES" + "',false,400,400);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
        }
        private void LoadQuestion(int _questionid, int _phaseid)
        {
            if (_phaseid == 0)
                Int32.TryParse(oDesign.GetQuestion(_questionid, "phaseid"), out _phaseid);
            ddlQuestion.DataTextField = "summary";
            ddlQuestion.DataValueField = "id";
            ddlQuestion.DataSource = oDesign.GetQuestions(_phaseid, 1);
            ddlQuestion.DataBind();
            ddlQuestion.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlQuestion.SelectedValue = _questionid.ToString();
        }
        private void LoadClass(int _classid, int _environmentclassid, int _environmentid)
        {
            ddlClass.DataTextField = "name";
            ddlClass.DataValueField = "id";
            ddlClass.DataSource = oClass.GetForecasts(1); ;
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlClass.SelectedValue = _classid.ToString();


            ddlEnvironmentClass.DataTextField = "name";
            ddlEnvironmentClass.DataValueField = "id";
            ddlEnvironmentClass.DataSource = oClass.GetForecasts(1);
            ddlEnvironmentClass.DataBind();
            ddlEnvironmentClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            if (_environmentclassid > 0)
            {
                ddlEnvironmentClass.SelectedValue = _environmentclassid.ToString();

                ddlEnvironment.DataTextField = "name";
                ddlEnvironment.DataValueField = "id";
                ddlEnvironment.DataSource = oClass.GetEnvironment(_environmentclassid, 1);
                ddlEnvironment.DataBind();
                ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                if (_environmentid > 0)
                {
                    ddlEnvironment.Enabled = true;
                    ddlEnvironment.SelectedValue = _environmentid.ToString();
                }
            }
            hdnEnvironment.Value = _environmentid.ToString();
        }
        private void LoadOS(int _osid)
        {
            ddlOS.DataTextField = "name";
            ddlOS.DataValueField = "id";
            ddlOS.DataSource = oOperatingSystem.Gets(0, 1); ;
            ddlOS.DataBind();
            ddlOS.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlOS.SelectedValue = _osid.ToString();
        }
        private void LoadModel(int _modelid)
        {
            ddlModel.DataTextField = "name";
            ddlModel.DataValueField = "id";
            ddlModel.DataSource = oDesign.GetModels(1);
            ddlModel.DataBind();
            ddlModel.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlModel.SelectedValue = _modelid.ToString();
        }
        private void LoadComponents(int _componentid)
        {
            ddlProduct.DataTextField = "name";
            ddlProduct.DataValueField = "id";
            ddlProduct.DataSource = oServerName.GetComponents(1);
            ddlProduct.DataBind();
            ddlProduct.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            if (_componentid > 0)
            {
                int intProduct = 0;
                if (Int32.TryParse(oServerName.GetComponentDetail(_componentid, "componentid"), out intProduct) == true)
                {
                    ddlProduct.SelectedValue = intProduct.ToString();

                    ddlComponent.DataTextField = "name";
                    ddlComponent.DataValueField = "id";
                    ddlComponent.DataSource = oServerName.GetComponentDetails(intProduct, 1);
                    ddlComponent.DataBind();
                    ddlComponent.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                    ddlComponent.Enabled = true;
                    ddlComponent.SelectedValue = _componentid.ToString();
                }
            }
            hdnComponent.Value = _componentid.ToString();
        }
        private void LoadInfrastructure(int _applicationid, int _subapplicationid)
        {
            ddlApplications.DataTextField = "name";
            ddlApplications.DataValueField = "id";
            ddlApplications.DataSource = oServerName.GetApplicationsForecast(1);
            ddlApplications.DataBind();
            ddlApplications.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            if (_applicationid > 0)
            {
                ddlApplications.SelectedValue = _applicationid.ToString();
                DataSet dsSubApplications = oServerName.GetSubApplications(_applicationid, 1);
                if (dsSubApplications.Tables[0].Rows.Count > 0)
                {
                    ddlSubApplications.DataTextField = "name";
                    ddlSubApplications.DataValueField = "id";
                    ddlSubApplications.DataSource = dsSubApplications;
                    ddlSubApplications.DataBind();
                    ddlSubApplications.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                    trInfrastructure2.Style["display"] = "inline";
                    ddlSubApplications.SelectedValue = _subapplicationid.ToString();
                    hdnSubApplication.Value = _subapplicationid.ToString();
                }
            }
            ddlApplications.Attributes.Add("onchange", "PopulateSubApplications('" + ddlApplications.ClientID + "','" + ddlSubApplications.ClientID + "','" + trInfrastructure2.ClientID + "');");
            ddlSubApplications.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlSubApplications.ClientID + "','" + hdnSubApplication.ClientID + "');");
        }
        private void LoadTree(TreeView oTree)
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
            }
            oTree.ExpandDepth = 1;
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadQuestion(int _parent, TreeNode oParent)
        {
            DataSet ds = oDesign.GetQuestions(_parent, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                // Figure out if it is "special"
                string strSpecial = oDesign.GetQuestionSpecial(Int32.Parse(dr["id"].ToString()));
                TreeNode oNode = new TreeNode();
                oNode.Text = "&nbsp;" + dr["summary"].ToString();
                oNode.ToolTip = dr["summary"].ToString();
                oNode.ImageUrl = (strSpecial == "" ? "/images/folder.gif" : "/images/na.gif");
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                TreeNode oNew = new TreeNode();
                if (strSpecial == "" && dr["is_type_textbox"].ToString() != "1" && dr["is_type_textarea"].ToString() != "1")
                {
                    LoadResponse(Int32.Parse(dr["id"].ToString()), oNode);
                    oNew.Text = "&nbsp;Add Response";
                    oNew.ToolTip = "Add Response";
                    oNew.ImageUrl = "/images/green_right.gif";
                    oNew.NavigateUrl = Request.Path + "?parent=" + dr["id"].ToString();
                }
                else
                {
                    oNew.Text = "Question: &quot;" + strSpecial + "&quot;";
                    oNew.SelectAction = TreeNodeSelectAction.None;
                }
                oNode.ChildNodes.Add(oNew);
            }
        }
        private void LoadResponse(int _parent, TreeNode oParent)
        {
            DataSet ds = oDesign.GetResponses(_parent, 0, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["admin"].ToString();
                oNode.ToolTip = dr["admin"].ToString();
                if (dr["enabled"].ToString() != "1")
                    oNode.ImageUrl = "/images/cancel.gif";
                else if (dr["is_exception"].ToString() == "1")
                    oNode.ImageUrl = "/images/docshare.gif";
                oNode.NavigateUrl = Request.Path + "?id=" + dr["id"].ToString();
                oParent.ChildNodes.Add(oNode);
            }
        }
        private void LoadRestrictions(TreeView oTree, int _display, int _disabled)
        {
            DataSet dsOther = oDesign.GetRestrictions(intID, _disabled);
            DataSet ds = oDesign.GetPhases(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intDisplay = 0;
                TreeNode oNode = new TreeNode();
                oNode.Text = "&nbsp;" + dr["title"].ToString();
                oNode.ToolTip = dr["title"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oNode.ShowCheckBox = (Int32.TryParse(dr["display"].ToString(), out intDisplay) && intDisplay > _display);
                if (oNode.ShowCheckBox == false)
                    oNode.ImageUrl = "/images/na.gif";
                oNode.Checked = false;
                foreach (DataRow drOther in dsOther.Tables[0].Rows)
                {
                    if (dr["id"].ToString() == drOther["phaseid"].ToString())
                        oNode.Checked = true;
                }
                oTree.Nodes.Add(oNode);
            }
            oTree.ExpandDepth = 0;
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }


        private void LoadSelectionPhase(TreeView oTree)
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
                LoadSelectionQuestion(Int32.Parse(dr["id"].ToString()), oNode);
            }
            oTree.ExpandDepth = 2;
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadSelectionQuestion(int _parent, TreeNode oParent)
        {
            DataSet ds = oDesign.GetQuestions(_parent, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string strSpecial = oDesign.GetQuestionSpecial(Int32.Parse(dr["id"].ToString()));
                TreeNode oNode = new TreeNode();
                oNode.Text = "&nbsp;" + dr["summary"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                if (strSpecial == "")
                    LoadSelections(Int32.Parse(dr["id"].ToString()), oNode);
                else
                    oNode.ImageUrl = "/images/na.gif";
            }
        }
        private void LoadSelections(int _parent, TreeNode oParent)
        {
            DataSet dsOther = oDesign.GetSelections(intID);
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
                    if (dr["id"].ToString() == drOther["setid"].ToString())
                        oNode.Checked = true;
                }
                oParent.ChildNodes.Add(oNode);
            }
        }


        //private void LoadShowPhase(TreeView oTree, int _display_q, int _display_p)
        //{
        //    DataSet ds = oDesign.GetPhases(1);
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        int intDisplay = 0;
        //        TreeNode oNode = new TreeNode();
        //        oNode.Text = "&nbsp;" + dr["title"].ToString();
        //        oNode.ImageUrl = "/images/tack.gif";
        //        oNode.SelectAction = TreeNodeSelectAction.Expand;
        //        oTree.Nodes.Add(oNode);
        //        if (Int32.TryParse(dr["display"].ToString(), out intDisplay) && intDisplay >= _display_p)
        //            LoadShowQuestion(Int32.Parse(dr["id"].ToString()), oNode, (intDisplay == _display_p ? _display_q : 0));
        //        else
        //            oNode.ImageUrl = "/images/na.gif";
        //    }
        //    oTree.ExpandDepth = 2;
        //    oTree.Attributes.Add("oncontextmenu", "return false;");
        //}
        //private void LoadShowQuestion(int _parent, TreeNode oParent, int _display)
        //{
        //    DataSet dsOther = oDesign.GetShows(intID);
        //    DataSet ds = oDesign.GetQuestions(_parent, 0);
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        int intDisplay = 0;
        //        TreeNode oNode = new TreeNode();
        //        oNode.Text = dr["summary"].ToString();
        //        oNode.Value = dr["id"].ToString();
        //        oNode.SelectAction = TreeNodeSelectAction.None;
        //        if (Int32.TryParse(dr["display"].ToString(), out intDisplay) && intDisplay > _display)
        //        {
        //            oNode.ShowCheckBox = true;
        //            oNode.Checked = false;
        //            foreach (DataRow drOther in dsOther.Tables[0].Rows)
        //            {
        //                if (dr["id"].ToString() == drOther["questionid"].ToString())
        //                    oNode.Checked = true;
        //            }
        //        }
        //        else
        //            oNode.ImageUrl = "/images/na.gif";
        //        oParent.ChildNodes.Add(oNode);
        //    }
        //}


        private void LoadShowResponses(TreeView oTree, int _display_q, int _display_p, bool _show)
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
                    LoadShowResponseQuestion(Int32.Parse(dr["id"].ToString()), oNode, (intDisplay == _display_p ? _display_q : 999), _show);
                else
                    oNode.ImageUrl = "/images/na.gif";
            }
            oTree.ExpandDepth = 3;
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadShowResponseQuestion(int _parent, TreeNode oParent, int _display, bool _show)
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
            DataSet dsOther = oDesign.GetShowResponses(intID, (_show ? 0 : 1));
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
                    if (dr["id"].ToString() == drOther["requiredid"].ToString())
                        oNode.Checked = true;
                }
                oParent.ChildNodes.Add(oNode);
            }
        }


        private void LoadApprovals(TreeView oTree)
        {
            DataSet dsOther = oDesign.GetApprovals(intID);
            DataSet ds = oDesign.GetApprovalGroups(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.ShowCheckBox = true;
                oNode.Checked = false;
                oNode.SelectAction = TreeNodeSelectAction.None;
                foreach (DataRow drOther in dsOther.Tables[0].Rows)
                {
                    if (dr["id"].ToString() == drOther["groupid"].ToString())
                        oNode.Checked = true;
                }
                oTree.Nodes.Add(oNode);
            }
            oTree.ExpandDepth = 2;
            oTree.Attributes.Add("oncontextmenu", "return false;");
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
            int intQuestion = Int32.Parse(ddlQuestion.SelectedItem.Value);
            int intClassID = 0;
            if (chkClass.Checked)
                Int32.TryParse(ddlClass.SelectedItem.Value, out intClassID);
            int intComponentID = 0;
            if (chkComponent.Checked)
                Int32.TryParse(Request.Form[hdnComponent.UniqueID], out intComponentID);
            int intOSID = 0;
            if (chkOS.Checked)
                Int32.TryParse(ddlOS.SelectedItem.Value, out intOSID);
            int intEnvironmentClassID = 0;
            if (chkEnvironment.Checked)
                Int32.TryParse(ddlEnvironmentClass.SelectedItem.Value, out intEnvironmentClassID);
            int intEnvironmentID = 0;
            if (chkEnvironment.Checked)
                Int32.TryParse(Request.Form[hdnEnvironment.UniqueID], out intEnvironmentID);
            int intLocationID = 0;
            if (chkLocation.Checked)
                Int32.TryParse(Request.Form[hdnLocation.UniqueID], out intLocationID);
            int intModelID = 0;
            if (chkModel.Checked)
                Int32.TryParse(ddlModel.SelectedItem.Value, out intModelID);
            int intApp = 0;
            int intSubApp = 0;
            if (chkInfrastructure.Checked && ddlApplications.SelectedIndex > -1)
            {
                Int32.TryParse(ddlApplications.SelectedItem.Value, out intApp);
                Int32.TryParse(Request.Form[hdnSubApplication.UniqueID], out intSubApp);
            }
            if (intID == 0)
                intID = oDesign.AddResponse(intQuestion, txtResponse.Text, txtSummary.Text, txtAdmin.Text, intClassID, intOSID, intEnvironmentClassID, intEnvironmentID, intLocationID, intModelID, intComponentID, intApp, intSubApp, (chkUnder48.Checked ? 1 : 0), (chkOver48.Checked ? 1 : 0), (chkConfidenceLock.Checked ? 1 : 0), (chkConfidenceUnlock.Checked ? 1 : 0), (chkException.Checked ? 1 : 0), (ddlFields.Enabled ? ddlFields.SelectedItem.Value : ""), txtValue.Text, Int32.Parse(txtQuantityMin.Text), Int32.Parse(txtQuantityMax.Text), (chkVisible.Checked ? 1 : 0), (chkSelectIfOne.Checked ? 1 : 0), (oDesign.GetResponses(intQuestion, 0, 0).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
            else
                oDesign.UpdateResponse(intID, intQuestion, txtResponse.Text, txtSummary.Text, txtAdmin.Text, intClassID, intOSID, intEnvironmentClassID, intEnvironmentID, intLocationID, intModelID, intComponentID, intApp, intSubApp, (chkUnder48.Checked ? 1 : 0), (chkOver48.Checked ? 1 : 0), (chkConfidenceLock.Checked ? 1 : 0), (chkConfidenceUnlock.Checked ? 1 : 0), (chkException.Checked ? 1 : 0), (ddlFields.Enabled ? ddlFields.SelectedItem.Value : ""), txtValue.Text, Int32.Parse(txtQuantityMin.Text), Int32.Parse(txtQuantityMax.Text), (chkVisible.Checked ? 1 : 0), (chkSelectIfOne.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oDesign.UpdateResponseOrder(intId, intCount);
                }
            }
            // Save Phase Restrictions (DISABLED)
            oDesign.DeleteRestriction(intID, 1);
            foreach (TreeNode oNode in oTreeRestrictionsDisabled.Nodes)
            {
                if (oNode.Checked == true)
                    oDesign.AddRestriction(intID, Int32.Parse(oNode.Value), 1);
            }
            // Save Phase Restrictions (ENABLED)
            oDesign.DeleteRestriction(intID, 0);
            foreach (TreeNode oNode in oTreeRestrictionsEnabled.Nodes)
            {
                if (oNode.Checked == true)
                    oDesign.AddRestriction(intID, Int32.Parse(oNode.Value), 0);
            }
            // Save Auto-Selections
            oDesign.DeleteSelection(intID);
            foreach (TreeNode oNodeP in oTreeSelections.Nodes)
                foreach (TreeNode oNodeQ in oNodeP.ChildNodes)
                    SaveSelection(oNodeQ);
            // Save Dynamic Questions
            //oDesign.DeleteShow(intID);
            //foreach (TreeNode oNodeP in oTreeQuestions.Nodes)
            //    SaveShow(oNodeP);
            // Save Dynamic Responses (SHOW)
            oDesign.DeleteShowResponse(intID, 0);
            foreach (TreeNode oNodeP in oTreeResponsesShow.Nodes)
                foreach (TreeNode oNodeQ in oNodeP.ChildNodes)
                    SaveShowResponse(oNodeQ, 0);
            oDesign.UpdateResponseShow(intID, (radShowAll.Checked ? 1 : 0), (radShowAny.Checked ? 1 : 0));
            // Save Dynamic Responses (HIDE)
            oDesign.DeleteShowResponse(intID, 1);
            foreach (TreeNode oNodeP in oTreeResponsesHide.Nodes)
                foreach (TreeNode oNodeQ in oNodeP.ChildNodes)
                    SaveShowResponse(oNodeQ, 1);
            oDesign.UpdateResponseHide(intID, (radHideAll.Checked ? 1 : 0), (radHideAny.Checked ? 1 : 0));
            // Save Approvals
            oDesign.DeleteApproval(intID);
            foreach (TreeNode oNode in oTreeApprovals.Nodes)
            {
                if (oNode.Checked == true)
                    oDesign.AddApproval(intID, Int32.Parse(oNode.Value));
            }

        }
        private void SaveSelection(TreeNode oParent)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oDesign.AddSelection(intID, Int32.Parse(oNode.Value));
            }
        }
        //private void SaveShow(TreeNode oParent)
        //{
        //    foreach (TreeNode oNode in oParent.ChildNodes)
        //    {
        //        if (oNode.Checked == true)
        //            oDesign.AddShow(intID, Int32.Parse(oNode.Value));
        //    }
        //}
        private void SaveShowResponse(TreeNode oParent, int _disabled)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oDesign.AddShowResponse(intID, Int32.Parse(oNode.Value), _disabled);
            }
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oDesign.DeleteResponse(intID);
            Response.Redirect(Request.Path);
        }

        protected void chkClass_Change(Object Sender, EventArgs e)
        {
            trClass.Visible = chkClass.Checked;
            panAdd.Visible = true;
        }
        protected void chkOS_Change(Object Sender, EventArgs e)
        {
            trOS.Visible = chkOS.Checked;
            panAdd.Visible = true;
        }
        protected void chkComponent_Change(Object Sender, EventArgs e)
        {
            trComponent1.Visible = chkComponent.Checked;
            trComponent2.Visible = chkComponent.Checked;
            panAdd.Visible = true;
        }
        protected void chkEnvironment_Change(Object Sender, EventArgs e)
        {
            trEnvironment1.Visible = chkEnvironment.Checked;
            trEnvironment2.Visible = chkEnvironment.Checked;
            panAdd.Visible = true;
        }
        protected void chkLocation_Change(Object Sender, EventArgs e)
        {
            trLocation.Visible = chkLocation.Checked;
            panAdd.Visible = true;
        }
        protected void chkModel_Change(Object Sender, EventArgs e)
        {
            trModel.Visible = chkModel.Checked;
            panAdd.Visible = true;
        }
        protected void chkInfrastructure_Change(Object Sender, EventArgs e)
        {
            trInfrastructure.Visible = chkInfrastructure.Checked;
            if (chkInfrastructure.Checked == false)
                trInfrastructure2.Style["display"] = "none";
            panAdd.Visible = true;
        }
    }
}
