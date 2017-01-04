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
    public partial class rr_storage_3rd_mid : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
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
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rid"] != "" && Request.QueryString["rid"] != null)
            {
                LoadValues();
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
            ddlCluster.Attributes.Add("onchange", "ResetDiv(null);SwapDivDDL(this,null,'" + divClusterYesGroup.ClientID + "','" + divClusterNo.ClientID + "',null);");
            ddlClusterYesSQLGroup.Attributes.Add("onchange", "SwapDivDDL(this,'" + divClusterYesGroupNew.ClientID + "',null,'" + divClusterYesGroupExisting.ClientID + "',null);");
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
                " && EnsureStorage3rd('" + ddlCluster.ClientID + "','" + ddlClusterYesSQLGroup.ClientID + "','" + txtClusterYesGroupExisting.ClientID + "','" + ddlClusterYesGroupExisting.ClientID + "','" + txtClusterYesGroupExistingFileSystem.ClientID + "','" + chkClusterYesGroupNewNetwork.ClientID + "','" + txtClusterYesGroupNewNetwork.ClientID + "','" + chkClusterYesGroupNewIP.ClientID + "','" + txtClusterYesGroupNewIP.ClientID + "','" + ddlClusterNo.ClientID + "','" + txtClusterNo.ClientID + "')" +
                " && ValidateNumber0('" + txtAdditional.ClientID + "','Please enter the amount of ADDITIONAL storage to be added')" +
                " && ValidateText('" + txtDescription.ClientID + "','Please enter a description of the work to be performed')" +
                ";");
            imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
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
            string _filesystem = "";
            string _client_amount = "";
            if (ddlCluster.SelectedIndex == 1)
            {
                // CLUSTER = YES
                if (ddlClusterYesSQLGroup.SelectedIndex == 1)
                {
                    // GROUP = NEW
                    _cluster_group_new = ddlClusterYesSQLGroup.SelectedItem.Value;
                    _tsm = (chkClusterYesGroupNewTSM.Checked ? 1 : 0);
                    _networkname = (chkClusterYesGroupNewNetwork.Checked ? txtClusterYesGroupNewNetwork.Text : "");
                    _ipaddress = (chkClusterYesGroupNewIP.Checked ? txtClusterYesGroupNewIP.Text : "");
                }
                else
                {
                    _cluster_group_existing = txtClusterYesGroupExisting.Text;
                    _increase = ddlClusterYesGroupExisting.SelectedItem.Value;
                    _filesystem = txtClusterYesGroupExistingFileSystem.Text;
                }
            }
            else
            {
                // CLUSTER = NO
                _increase = ddlClusterNo.SelectedItem.Value;
                _filesystem = txtClusterNo.Text;
            }
            _client_amount = txtAdditional.Text;
            oServiceRequest.Update(intRequest, "Server Growth Request (" + txtServerName.Text + ")");
            oCustomized.AddStorage3rd(intRequest, intItem, intNumber, txtServerName.Text, ddlOS.SelectedItem.Value, ddlMaintenance.SelectedItem.Value, ddlCurrent.SelectedItem.Value, ddlType.SelectedItem.Value, ddlDR.SelectedItem.Value, ddlPerformance.SelectedItem.Value, ddlChange.SelectedItem.Value, ddlCluster.SelectedItem.Value, _sql, _version, _dba, _cluster_group_new, _tsm, _networkname, _ipaddress, _cluster_group_existing, 0, 0, _newdriveletter, _newmountpoint, _increase, txtDescription.Text, 0, 0, 0, "", "", 0, "", "", 0.00, "", "", "", "", "", "", "", "", "", "", 0.00, 1, intProfile, DateTime.Parse(txtDate.Text), _filesystem, _client_amount);
            oCustomized.UpdateStorage3rdFlow1(intRequest, intItem, intNumber);
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