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
    public partial class wm_storage_3rd : System.Web.UI.UserControl
    {

        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intMyWork = Int32.Parse(ConfigurationManager.AppSettings["MyWork"]);
        protected int intServiceSAN = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_SERVER_GROWTH_SAN"]);
        protected string strRemediationTeam = ConfigurationManager.AppSettings["REMEDIATION_TEAM"];
        protected int intProfile;
        protected Projects oProject;
        protected Functions oFunction;
        protected Users oUser;
        protected Pages oPage;
        protected ResourceRequest oResourceRequest;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected Services oService;
        protected ServiceRequests oServiceRequest;
        protected RequestFields oRequestField;
        protected Applications oApplication;
        protected ServiceDetails oServiceDetail;
        protected Delegates oDelegate;
        protected Customized oCustomized;
        protected Variables oVariable;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolDetails = false;
        protected bool boolExecution = false;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intNumber = 0;
        protected int intService = 0;
        protected string strLocation = "";
        protected Locations oLocation;
        protected Classes oClass;
        private string strEMailIdsBCC = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oProject = new Projects(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oUser = new Users(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oRequestField = new RequestFields(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oServiceDetail = new ServiceDetails(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oLocation = new Locations(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
            {
                // Start Workflow Change
                lblResourceWorkflow.Text = Request.QueryString["rrid"];
                int intResourceWorkflow = Int32.Parse(Request.QueryString["rrid"]);
                int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                ds = oResourceRequest.Get(intResourceParent);
                // End Workflow Change
                intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                lblRequestedBy.Text = oUser.GetFullName(oRequest.GetUser(intRequest));
                lblRequestedOn.Text = DateTime.Parse(oRequest.Get(intRequest, "created")).ToLongDateString();
                intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                // Start Workflow Change
                bool boolComplete = (oResourceRequest.GetWorkflow(intResourceWorkflow, "status") == "3");
                int intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                txtCustom.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "name");
                double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                // End Workflow Change
                intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                lblService.Text = oService.Get(intService, "name");
                int intApp = oRequestItem.GetItemApplication(intItem);

                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Status Updates has been Added');<" + "/" + "script>");
                if (!IsPostBack)
                {
                    double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                    lblUpdated.Text = oResourceRequest.GetUpdated(intResourceWorkflow);
                    if (dblAllocated == dblUsed)
                    {
                        if (boolComplete == false)
                        {
                            oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                            btnComplete.Attributes.Add("onclick", "return confirm('Are you sure you want to mark this as completed and remove it from your workload?');");
                        }
                        else
                        {
                            oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                            btnComplete.Attributes.Add("onclick", "alert('This task has already been marked COMPLETE. You can close this window.');return false;");
                        }
                        panComplete.Visible = true;
                        panReject.Visible = false;
                    }
                    else
                    {
                        btnComplete.ImageUrl = "/images/tool_complete_dbl.gif";
                        btnComplete.Enabled = false;
                        btnReject.Attributes.Add("onclick", "return ValidateText('" + txtReject.ClientID + "','Please enter a reason for the rejection\\n\\nNOTE: This message will be displayed to the requestor') && confirm('Are you sure you want to reject this request?');");
                    }
                    dblUsed = (dblUsed / dblAllocated) * 100;
                    intProject = oRequest.GetProjectNumber(intRequest);
                    hdnTab.Value = "D";
                    panWorkload.Visible = true;
                    LoadLists();
                    LoadStatus(intResourceWorkflow);
                    LoadInformation(intResourceWorkflow);
                    LoadDetails();

                    strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, intLocation, true, "ddlCommon");
                    ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',0);");
                    ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
                    btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                    oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                    oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                    btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                    oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                    btnClose.Attributes.Add("onclick", "return ExitWindow();");
                    ddlCluster.Attributes.Add("onchange", "ResetDiv(null);SwapDivDDL(this,'" + divClusterYes.ClientID + "','" + divClusterYesGroup.ClientID + "','" + divClusterNo.ClientID + "',null);");
                    ddlClusterYesSQL.Attributes.Add("onchange", "ResetDiv('" + divClusterYesSQLNoMount.ClientID + "');SwapDivDDL(this,'" + divClusterYesSQLYes.ClientID + "',null,'" + divClusterYesSQLNo.ClientID + "',null);");
                    ddlClusterYesSQLYesVersion.Attributes.Add("onchange", "ShowDivDDL(this,'" + divSQLYes2005.ClientID + "',1);");
                    ddlClusterYesSQLGroup.Attributes.Add("onchange", "SwapDivDDL(this,'" + divClusterYesGroupNew.ClientID + "',null,'" + divClusterYesGroupExisting.ClientID + "',null);");
                    ddlClusterYesSQLNoType.Attributes.Add("onchange", "ShowDivDDL(this,'" + divClusterYesSQLNoMount.ClientID + "',2);");
                    ddlClusterNoSQL.Attributes.Add("onchange", "SwapDivDDL(this,'" + divClusterNoSQLYes.ClientID + "',null,'" + divClusterNoSQLNo.ClientID + "',null);");
                    ddlClusterNoSQLYesVersion.Attributes.Add("onchange", "ShowDivDDL(this,'" + divSQLYes2005.ClientID + "',1);");
                    txtClusterNoSQLDBA.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divClusterNoSQLDBA.ClientID + "','" + lstClusterNoSQLDBA.ClientID + "','" + hdnDBA.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                    lstClusterNoSQLDBA.Attributes.Add("ondblclick", "AJAXClickRow();");
                    txtClusterYesSQLDBA.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divClusterYesSQLDBA.ClientID + "','" + lstClusterYesSQLDBA.ClientID + "','" + hdnDBA.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                    lstClusterYesSQLDBA.Attributes.Add("ondblclick", "AJAXClickRow();");
                    btnSave.Attributes.Add("onclick", "return ValidateText('" + txtServerName.ClientID + "','Please enter the server name')" +
                        " && ValidateDropDown('" + ddlOS.ClientID + "','Please select the operating system')" +
                        " && ValidateDropDown('" + ddlMaintenance.ClientID + "','Please select the maintenance window')" +
                        " && ValidateDropDown('" + ddlCurrent.ClientID + "','Please select if the server currently has SAN')" +
                        " && ValidateDropDown('" + ddlType.ClientID + "','Please select the server type')" +
                        " && ValidateDropDown('" + ddlDR.ClientID + "','Please select the DR options')" +
                        " && ValidateDropDown('" + ddlPerformance.ClientID + "','Please select the performance type')" +
                        " && ValidateDropDown('" + ddlChange.ClientID + "','Please select if you have scheduled a change')" +
                        " && ValidateDropDown('" + ddlCluster.ClientID + "','Please select if the server is part of a cluster')" +
                        " && EnsureStorage3rd('" + ddlCluster.ClientID + "','" + ddlClusterNoSQL.ClientID + "','" + ddlClusterNoSQLNo.ClientID + "','" + ddlClusterNoSQLYesVersion.ClientID + "','" + hdnDBA.ClientID + "','" + txtClusterNoSQLDBA.ClientID + "','" + ddlClusterYesSQL.ClientID + "','" + ddlClusterYesSQLYesVersion.ClientID + "','" + ddlClusterYesSQLGroup.ClientID + "','" + txtClusterYesGroupExisting.ClientID + "','" + chkClusterYesGroupNewNetwork.ClientID + "','" + txtClusterYesGroupNewNetwork.ClientID + "','" + chkClusterYesGroupNewIP.ClientID + "','" + txtClusterYesGroupNewIP.ClientID + "','" + txtClusterYesSQLDBA.ClientID + "','" + ddlClusterYesSQLNoType.ClientID + "','" + txtClusterYesSQLNoMount.ClientID + "')" +
                        " && ValidateText('" + txtDescription.ClientID + "','Please enter a description of the work to be performed')" +
                        " && ValidateDropDown('" + ddlClass.ClientID + "','Please select the class')" +
                        " && ValidateDropDown('" + ddlEnvironment.ClientID + "','Please select the environment')" +
                        " && ValidateDropDown('" + ddlFabric.ClientID + "','Please select the fabric')" +
                        " && ValidateDropDown('" + ddlReplicated.ClientID + "','Please select if this device is being replicated')" +
                        " && ValidateDropDown('" + ddlType2.ClientID + "','Please select a type of storage')" +
                        " && ValidateDropDown('" + ddlExpand.ClientID + "','Please select if you want to expand a LUN or add an additional LUN')" +
                        " && ValidateNumber0('" + txtAdditional.ClientID + "','Please enter the total amount of storage')" +
                        " && ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "')" +
                        ";");

                    btnLunAdd.Attributes.Add("onclick", "return ValidateText('" + txtLUNs.ClientID + "','Please enter some text') && ValidateNoComma('" + txtLUNs.ClientID + "','The text cannot contain a comma (,)\\n\\nPlease click OK and remove all commas') && ListControlIn('" + lstLUNs.ClientID + "','" + hdnLUNs.ClientID + "','" + txtLUNs.ClientID + "');");
                    btnLunDelete.Attributes.Add("onclick", "return ListControlOut('" + lstLUNs.ClientID + "','" + hdnLUNs.ClientID + "');");
                    txtLUNs.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnLunAdd.ClientID + "').click();return false;}} else {return true}; ");
                    // 6/1/2009 - Load ReadOnly View
                    if (oResourceRequest.CanUpdate(intProfile, intResourceWorkflow, false) == false)
                    {
                        oFunction.ConfigureToolButtonRO(btnSave, btnComplete);
                        //panDenied.Visible = true;
                    }
                }
            }
            else
                panDenied.Visible = true;

        }
        private void LoadLists()
        {
            ddlClass.DataTextField = "name";
            ddlClass.DataValueField = "id";
            ddlClass.DataSource = oClass.GetForecasts(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void LoadInformation(int _request)
        {
            if (intProject > 0)
            {
                lblName.Text = oProject.Get(intProject, "name");
                lblNumber.Text = oProject.Get(intProject, "number");
                lblType.Text = "Project";
            }
            else
            {
                lblName.Text = oResourceRequest.GetWorkflow(_request, "name");
                lblNumber.Text = "CVT" + intRequest.ToString();
                lblType.Text = "Task";
            }
            if (Request.QueryString["div"] != null)
            {
                switch (Request.QueryString["div"])
                {
                    case "E":
                        boolExecution = true;
                        break;
                }
            }
            if (boolDetails == false && boolExecution == false)
                boolDetails = true;
        }

        private void LoadStatus(int _resourceid)
        {
            DataSet dsStatus = oResourceRequest.GetStatuss(_resourceid);
            rptStatus.DataSource = dsStatus;
            rptStatus.DataBind();
            lblNoStatus.Visible = (rptStatus.Items.Count == 0);
            double dblTotalStatus = 0.00;
            foreach (RepeaterItem ri in rptStatus.Items)
            {
                Label _status = (Label)ri.FindControl("lblStatus");
                double dblStatus = double.Parse(_status.Text);
                if (dblTotalStatus == 0.00)
                    dblTotalStatus = dblStatus;
                _status.Text = oResourceRequest.GetStatus(dblStatus, 50, 15);
            }
            lblStatus.Text = oResourceRequest.GetStatus(dblTotalStatus, 50, 15);
        }

        private void LoadDetails()
        {
            DataSet ds = oCustomized.GetStorage3rd(intRequest, intItem, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtServerName.Text = ds.Tables[0].Rows[0]["servername"].ToString();
                lblDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["end_date"].ToString()).ToShortDateString();
                ddlOS.SelectedValue = ds.Tables[0].Rows[0]["os"].ToString();
                ddlMaintenance.SelectedValue = ds.Tables[0].Rows[0]["maintenance"].ToString();
                ddlCurrent.SelectedValue = ds.Tables[0].Rows[0]["currently"].ToString();
                ddlType.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                ddlDR.SelectedValue = ds.Tables[0].Rows[0]["dr"].ToString();
                ddlPerformance.SelectedValue = ds.Tables[0].Rows[0]["performance"].ToString();
                ddlChange.SelectedValue = ds.Tables[0].Rows[0]["change"].ToString();
                ddlCluster.SelectedValue = ds.Tables[0].Rows[0]["cluster"].ToString();
                ddlClusterYesSQL.SelectedValue = ds.Tables[0].Rows[0]["sql"].ToString();
                ddlClusterNoSQL.SelectedValue = ds.Tables[0].Rows[0]["sql"].ToString();
                ddlClusterYesSQLYesVersion.SelectedValue = ds.Tables[0].Rows[0]["version"].ToString();
                ddlClusterNoSQLYesVersion.SelectedValue = ds.Tables[0].Rows[0]["version"].ToString();
                int intDBA = Int32.Parse(ds.Tables[0].Rows[0]["dba"].ToString());
                if (intDBA > 0)
                {
                    txtClusterNoSQLDBA.Text = oUser.GetFullName(intDBA) + " (" + oUser.GetName(intDBA) + ")";
                    txtClusterYesSQLDBA.Text = oUser.GetFullName(intDBA) + " (" + oUser.GetName(intDBA) + ")";
                    hdnDBA.Value = intDBA.ToString();
                }
                ddlClusterYesSQLGroup.SelectedValue = ds.Tables[0].Rows[0]["cluster_group_new"].ToString();
                chkClusterYesGroupNewTSM.Checked = (ds.Tables[0].Rows[0]["tsm"].ToString() == "1");
                if (ds.Tables[0].Rows[0]["networkname"].ToString() != "")
                {
                    chkClusterYesGroupNewNetwork.Checked = true;
                    txtClusterYesGroupNewNetwork.Text = ds.Tables[0].Rows[0]["networkname"].ToString();
                }
                if (ds.Tables[0].Rows[0]["ipaddress"].ToString() != "")
                {
                    chkClusterYesGroupNewIP.Checked = true;
                    txtClusterYesGroupNewIP.Text = ds.Tables[0].Rows[0]["ipaddress"].ToString();
                }
                txtClusterYesGroupExisting.Text = ds.Tables[0].Rows[0]["cluster_group_existing"].ToString();
                chkSQLYes2005.Items[0].Selected = (ds.Tables[0].Rows[0]["databasesql0x"].ToString() == "1");
                chkSQLYes2005.Items[1].Selected = (ds.Tables[0].Rows[0]["backupsql0x"].ToString() == "1");
                ddlClusterYesSQLNoType.SelectedValue = ds.Tables[0].Rows[0]["newdriveletter"].ToString();
                txtClusterYesSQLNoMount.Text = ds.Tables[0].Rows[0]["newmountpoint"].ToString();
                ddlClusterNoSQLNo.SelectedValue = ds.Tables[0].Rows[0]["increase"].ToString();
                txtDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();

                int intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                ddlClass.SelectedValue = intClass.ToString();
                int intEnv = Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString());
                hdnEnvironment.Value = intEnv.ToString();
                ddlEnvironment.Enabled = true;
                ddlEnvironment.DataTextField = "name";
                ddlEnvironment.DataValueField = "id";
                ddlEnvironment.DataSource = oClass.GetEnvironment(intClass, 0);
                ddlEnvironment.DataBind();
                ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlEnvironment.SelectedValue = intEnv.ToString();
                intLocation = Int32.Parse(ds.Tables[0].Rows[0]["addressid"].ToString());
                strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, intLocation, true, "ddlCommon");
                hdnLocation.Value = intLocation.ToString();

                ddlFabric.SelectedValue = ds.Tables[0].Rows[0]["fabric"].ToString();
                ddlReplicated.SelectedValue = ds.Tables[0].Rows[0]["replicated"].ToString();
                ddlType2.SelectedValue = ds.Tables[0].Rows[0]["shared"].ToString();
                ddlExpand.SelectedValue = ds.Tables[0].Rows[0]["expand"].ToString();
                txtAdditional.Text = double.Parse(ds.Tables[0].Rows[0]["amount"].ToString()).ToString("F");
                hdnLUNs.Value = ds.Tables[0].Rows[0]["luns"].ToString();
                char[] strSplit = { ',' };
                string[] strValues = ds.Tables[0].Rows[0]["luns"].ToString().Split(strSplit);
                for (int ii = 0; ii < strValues.Length; ii++)
                {
                    if (strValues[ii].Trim() != "")
                        lstLUNs.Items.Add(strValues[ii].Trim());
                }
                txtWWW.Text = ds.Tables[0].Rows[0]["www"].ToString();
                txtUID.Text = ds.Tables[0].Rows[0]["luns"].ToString();
                txtNode.Text = ds.Tables[0].Rows[0]["node"].ToString();
                txtEnclosureName.Text = ds.Tables[0].Rows[0]["encname"].ToString();
                txtEnclosureSlot.Text = ds.Tables[0].Rows[0]["encslot"].ToString();
                txtReplicatedServerName.Text = ds.Tables[0].Rows[0]["repservername"].ToString();
                txtReplicatedWWW.Text = ds.Tables[0].Rows[0]["repwww"].ToString();
                txtReplicatedEnclosureName.Text = ds.Tables[0].Rows[0]["repencname"].ToString();
                txtReplicatedEnclosureSlot.Text = ds.Tables[0].Rows[0]["repencslot"].ToString();
                lblMidrange.Text = ds.Tables[0].Rows[0]["midrange"].ToString();
                if (ddlCluster.SelectedIndex == 1)
                {
                    // CLUSTER = YES
                    divClusterYes.Style["display"] = "inline";
                    divClusterYesGroup.Style["display"] = "inline";
                    if (ddlClusterYesSQL.SelectedIndex == 1)
                    {
                        // SQL = YES
                        divClusterYesSQLYes.Style["display"] = "inline";
                        if (ddlClusterYesSQLYesVersion.SelectedIndex == 1)
                            divSQLYes2005.Style["display"] = "inline";
                        if (ddlClusterYesSQLGroup.SelectedIndex == 1)
                            divClusterYesGroupNew.Style["display"] = "inline";
                        else
                            divClusterYesGroupExisting.Style["display"] = "inline";
                    }
                    else
                    {
                        // SQL = NO
                        divClusterYesSQLNo.Style["display"] = "inline";
                        if (ddlClusterYesSQLNoType.SelectedIndex == 2)
                            divClusterYesSQLNoMount.Style["display"] = "inline";
                        if (ddlClusterYesSQLGroup.SelectedIndex == 1)
                            divClusterYesGroupNew.Style["display"] = "inline";
                        else
                            divClusterYesGroupExisting.Style["display"] = "inline";
                    }
                }
                else
                {
                    // CLUSTER = NO
                    divClusterNo.Style["display"] = "inline";
                    if (ddlClusterNoSQL.SelectedIndex == 1)
                    {
                        // SQL = YES
                        divClusterNoSQLYes.Style["display"] = "inline";
                        if (ddlClusterYesSQLYesVersion.SelectedIndex == 1)
                            divSQLYes2005.Style["display"] = "inline";
                    }
                    else
                    {
                        // SQL = NO
                        divClusterNoSQLNo.Style["display"] = "inline";
                    }
                }
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);

            if (ddlStatus.SelectedIndex > -1 && txtComments.Text.Trim() != "")
            {
                oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text, intProfile);
                //CVT62149 Workload Manager Red Light Status =Hold
                if (ddlStatus.SelectedValue == "1") //Red
                    oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 5, true);
                else
                    oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 2, true);
            }

            double dblHours = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
            double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);

            dblHours = (dblHours - dblUsed);
            if (dblHours > 0.00)
                oResourceRequest.UpdateWorkflowHours(intResourceWorkflow, dblHours);
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            string _sql = "";
            string _version = "";
            int _dba = 0;
            string _cluster_group_new = "";
            string _cluster_group_existing = "";
            int _tsm = 0;
            string _networkname = "";
            string _ipaddress = "";
            string _newdriveletter = "";
            string _newmountpoint = "";
            string _increase = "";
            if (ddlCluster.SelectedIndex == 1)
            {
                // CLUSTER = YES
                _sql = ddlClusterYesSQL.SelectedItem.Value;
                if (ddlClusterYesSQL.SelectedIndex == 1)
                {
                    // SQL = YES
                    _version = ddlClusterYesSQLYesVersion.SelectedItem.Value;
                    _dba = Int32.Parse(Request.Form[hdnDBA.UniqueID]);
                    _cluster_group_new = ddlClusterYesSQLGroup.SelectedItem.Value;
                    _cluster_group_existing = txtClusterYesGroupExisting.Text;
                    _tsm = (chkClusterYesGroupNewTSM.Checked ? 1 : 0);
                    _networkname = (chkClusterYesGroupNewNetwork.Checked ? txtClusterYesGroupNewNetwork.Text : "");
                    _ipaddress = (chkClusterYesGroupNewIP.Checked ? txtClusterYesGroupNewIP.Text : "");
                }
                else
                {
                    // SQL = NO
                    _newdriveletter = ddlClusterYesSQLNoType.SelectedItem.Value;
                    _newmountpoint = txtClusterYesSQLNoMount.Text;
                    _cluster_group_new = ddlClusterYesSQLGroup.SelectedItem.Value;
                    _cluster_group_existing = txtClusterYesGroupExisting.Text;
                    _tsm = (chkClusterYesGroupNewTSM.Checked ? 1 : 0);
                    _networkname = (chkClusterYesGroupNewNetwork.Checked ? txtClusterYesGroupNewNetwork.Text : "");
                    _ipaddress = (chkClusterYesGroupNewIP.Checked ? txtClusterYesGroupNewIP.Text : "");
                }
            }
            else
            {
                // CLUSTER = NO
                _sql = ddlClusterNoSQL.SelectedItem.Value;
                if (ddlClusterNoSQL.SelectedIndex == 1)
                {
                    // SQL = YES
                    _version = ddlClusterNoSQLYesVersion.SelectedItem.Value;
                    _dba = Int32.Parse(Request.Form[hdnDBA.UniqueID]);
                }
                else
                {
                    // SQL = NO
                    _increase = ddlClusterNoSQLNo.SelectedItem.Value;
                }
            }
            oCustomized.AddStorage3rd(intRequest, intItem, intNumber, txtServerName.Text, ddlOS.SelectedItem.Value, ddlMaintenance.SelectedItem.Value, ddlCurrent.SelectedItem.Value, ddlType.SelectedItem.Value, ddlDR.SelectedItem.Value, ddlPerformance.SelectedItem.Value, ddlChange.SelectedItem.Value, ddlCluster.SelectedItem.Value, _sql, _version, _dba, _cluster_group_new, _tsm, _networkname, _ipaddress, _cluster_group_existing, chkSQLYes2005.Items[0].Selected ? 1 : 0, chkSQLYes2005.Items[1].Selected ? 1 : 0, _newdriveletter, _newmountpoint, _increase, txtDescription.Text, Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), Int32.Parse(Request.Form[hdnLocation.UniqueID]), ddlFabric.SelectedItem.Value, ddlReplicated.SelectedItem.Value, 0, ddlType2.SelectedItem.Value, ddlExpand.SelectedItem.Value, double.Parse(txtAdditional.Text), Request.Form[hdnLUNs.UniqueID], txtWWW.Text, txtUID.Text, txtNode.Text, txtEnclosureName.Text, txtEnclosureSlot.Text, txtReplicatedServerName.Text, txtReplicatedWWW.Text, txtReplicatedEnclosureName.Text, txtReplicatedEnclosureSlot.Text, 0.00, Int32.Parse(lblMidrange.Text), intProfile, DateTime.Parse(lblDate.Text), "", "");
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            //// Send to SAN Team
            int intNewService = intServiceSAN;
            int intNewItem = oService.GetItemId(intNewService);
            int intNewNumber = oResourceRequest.GetNumber(intRequest, intNewItem);
            oCustomized.UpdateStorage3rd(intRequest, intItem, intNumber, intNewItem, intNewNumber);
            oCustomized.UpdateStorage3rdFlow2(intRequest, intItem, intNumber, intNewItem, intNewNumber);
            int intResource = oServiceRequest.AddRequest(intRequest, intNewItem, intNewService, 0, 0.00, 2, intNewNumber, dsnServiceEditor);
            oServiceRequest.NotifyTeamLead(intNewItem, intResource, intAssignPage, intViewPage, intEnvironment,  "", dsnServiceEditor, dsnAsset, dsnIP, 0);
            // Notify Remediation Team (but do not create request)
            string strDetails = "";
            DataSet ds = oCustomized.GetStorage3rd(intRequest, intNewItem, intNewNumber);
            if (ds.Tables[0].Rows.Count > 0)
                strDetails = oCustomized.GetStorage3rdBody(intRequest, intItem, intNumber, intEnvironment);
            if (strDetails != "")
                strDetails = "<table width=\"100%\" border=\"0\" cellSpacing=\"2\" cellPadding=\"4\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strDetails + "</table>";
            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
            oFunction.SendEmail("Incoming Storage Request [CVT" + intRequest.ToString() + "]", strRemediationTeam, "", strEMailIdsBCC, "Incoming Storage Request [#CVT" + intRequest.ToString() + "]", "<p><b>The following server growth request has been sent to the SAN department...this message is to inform you that it will arrive soon.</b></p><p>When the SAN team has finished configuring this request, another notification will be sent to you to assign a resource.</p><p>" + strDetails + "</p>", true, false);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
        protected void btnReject_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            int intUser = oRequest.GetUser(intRequest);
            string strComments = "";
            if (txtReject.Text.Trim() != "")
                strComments = "<p>The following comments were added:<br/>" + txtReject.Text + "</p>";
            string strService = oService.Get(intService, "name");
            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT,EMAILGRP_REQUEST_STATUS");
            oFunction.SendEmail("Request REJECTED: " + strService, oUser.GetName(intUser), "", strEMailIdsBCC, "Request REJECTED: " + strService, "<p><b>The following request has been rejected by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>" + strComments, true, false);
            oResourceRequest.UpdateAccepted(intResourceParent, -1);
            oResourceRequest.UpdateReason(intResourceParent, txtReject.Text);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
    }
}