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
    public partial class rr_storage_3rd_vmware : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected int intProjectRequestPage = Int32.Parse(ConfigurationManager.AppSettings["ProjectRequest"]);
        protected Pages oPage;
        protected RequestItems oRequestItem;
        protected Applications oApplication;
        protected Variables oVariable;
        protected RequestFields oRequestField;
        protected Services oService;
        protected Customized oCustomized;
        protected Requests oRequest;
        protected Projects oProject;
        protected ProjectsPending oProjectsPending;
        protected ServiceRequests oServiceRequest;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected string strTable = "";
        protected string strScript = "";
        protected string strLocation = "";
        protected Locations oLocation;
        protected Classes oClass;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oRequestField = new RequestFields(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            oProjectsPending = new ProjectsPending(intProfile, dsn, intEnvironment);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rid"] != "" && Request.QueryString["rid"] != null)
            {
                LoadValues();
                if (!IsPostBack)
                    LoadLists();
                // Custom Loads
                int intItem = Int32.Parse(lblItem.Text);
                int intApp = oRequestItem.GetItemApplication(intItem);
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable != "")
                {
                    panDeliverable.Visible = true;
                    btnDeliverable.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','" + strDeliverable + "');");
                }
            }
            strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, intLocation, true, "ddlCommon");
            ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',0);");
            ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
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
            btnCancel1.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service request?');");
            btnNext.Attributes.Add("onclick", "return ValidateText('" + txtServerName.ClientID + "','Please enter the server name')" +
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
                " && ValidateDropDown('" + ddlHA.ClientID + "','Please select if this device requires high availability storage')" +
                " && ValidateDropDown('" + ddlType2.ClientID + "','Please select a type of storage')" +
                " && ValidateDropDown('" + ddlExpand.ClientID + "','Please select if you want to expand a LUN or add an additional LUN')" +
                " && ValidateNumber0('" + txtAdditional.ClientID + "','Please enter the total amount of storage')" +
                ";");
            imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
            btnLunAdd.Attributes.Add("onclick", "return ValidateText('" + txtLUNs.ClientID + "','Please enter some text') && ValidateNoComma('" + txtLUNs.ClientID + "','The text cannot contain a comma (,)\\n\\nPlease click OK and remove all commas') && ListControlIn('" + lstLUNs.ClientID + "','" + hdnLUNs.ClientID + "','" + txtLUNs.ClientID + "');");
            btnLunDelete.Attributes.Add("onclick", "return ListControlOut('" + lstLUNs.ClientID + "','" + hdnLUNs.ClientID + "');");
            txtLUNs.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnLunAdd.ClientID + "').click();return false;}} else {return true}; ");
        }
        private void LoadLists()
        {
            ddlClass.DataTextField = "name";
            ddlClass.DataValueField = "id";
            ddlClass.DataSource = oClass.GetForecasts(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void LoadValues()
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            DataSet dsItems = oRequestItem.GetForms(intRequest);
            if (dsItems.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drItem in dsItems.Tables[0].Rows)
                {
                    if (drItem["done"].ToString() == "-1")
                    {
                        lblItem.Text = drItem["itemid"].ToString();
                        lblNumber.Text = drItem["number"].ToString();
                        lblService.Text = drItem["serviceid"].ToString();
                        break;
                    }
                }
            }
        }
        protected void lnkRequest_Click(Object Sender, EventArgs e)
        {
            Users oUser = new Users(intProfile, dsn);
            string strDefault = oUser.GetApplicationUrl(intProfile, intProjectRequestPage);
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            Requests oRequest = new Requests(intProfile, dsn);
            Response.Redirect(strDefault + oPage.GetFullLink(intProjectRequestPage) + "?pid=" + oRequest.GetProjectNumber(intRequest).ToString());
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            oRequestItem.UpdateForm(intRequest, false);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            int intItem = Int32.Parse(lblItem.Text);
            int intNumber = Int32.Parse(lblNumber.Text);
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
            oServiceRequest.Update(intRequest, "Server Growth Request (" + txtServerName.Text + ")");
            oCustomized.AddStorage3rd(intRequest, intItem, intNumber, txtServerName.Text, ddlOS.SelectedItem.Value, ddlMaintenance.SelectedItem.Value, ddlCurrent.SelectedItem.Value, ddlType.SelectedItem.Value, ddlDR.SelectedItem.Value, ddlPerformance.SelectedItem.Value, ddlChange.SelectedItem.Value, ddlCluster.SelectedItem.Value, _sql, _version, _dba, _cluster_group_new, _tsm, _networkname, _ipaddress, _cluster_group_existing, chkSQLYes2005.Items[0].Selected ? 1 : 0, chkSQLYes2005.Items[1].Selected ? 1 : 0, _newdriveletter, _newmountpoint, _increase, txtDescription.Text, Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), Int32.Parse(Request.Form[hdnLocation.UniqueID]), ddlFabric.SelectedItem.Value, ddlReplicated.SelectedItem.Value, (ddlHA.SelectedItem.Value == "Yes" ? 1 : 0), ddlType2.SelectedItem.Value, ddlExpand.SelectedItem.Value, double.Parse(txtAdditional.Text), Request.Form[hdnLUNs.UniqueID], txtWWW.Text, txtUID.Text, txtNode.Text, txtEnclosureName.Text, txtEnclosureSlot.Text, txtReplicatedServerName.Text, txtReplicatedWWW.Text, txtReplicatedEnclosureName.Text, txtReplicatedEnclosureSlot.Text, 0.00, 0, intProfile, DateTime.Parse(txtDate.Text), "", "");
            oCustomized.UpdateStorage3rdFlow1(intRequest, intItem, intNumber);
            oCustomized.UpdateStorage3rdFlow2(intRequest, intItem, intNumber, intItem, intNumber);
            oRequestItem.UpdateForm(intRequest, true);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            oRequest.Cancel(intRequest);
            Response.Redirect(oPage.GetFullLink(intPage));
        }
    }
}