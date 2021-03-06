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
using System.DirectoryServices;
namespace NCC.ClearView.Presentation.Web
{
    public partial class rr_group_rename : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected RequestItems oRequestItem;
        protected Applications oApplication;
        protected GroupRequest oGroupRequest;
        protected Requests oRequest;
        protected Domains oDomain;
        protected Functions oFunction;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oGroupRequest = new GroupRequest(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oDomain = new Domains(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rid"] != "" && Request.QueryString["rid"] != null)
            {
                if (!IsPostBack)
                    LoadLists();
                LoadValues();
                int intItem = Int32.Parse(lblItem.Text);
                int intApp = oRequestItem.GetItemApplication(intItem);
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable != "")
                {
                    panDeliverable.Visible = true;
                    btnDeliverable.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','" + strDeliverable + "');");
                }
                if (Request.QueryString["g"] != null && Request.QueryString["g"] != "" && Request.QueryString["d"] != null && Request.QueryString["d"] != "")
                {
                    if (!IsPostBack)
                        LoadObjects();
                    btnNext.Attributes.Add("onclick", "return ValidateText('" + txtTo.ClientID + "','Please enter a group name')" +
                        ";");
                }
                else
                {
                    btnNext.Enabled = false;
                }
                btnContinue.Attributes.Add("onclick", "return ValidateText('" + txtFrom.ClientID + "','Please enter a group name')" +
                    " && ValidateDropDown('" + ddlDomain.ClientID + "','Please select a domain')" +
                    ";");
            }
            btnCancel1.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service request?');");
        }
        private void LoadLists()
        {
            ddlDomain.DataValueField = "id";
            ddlDomain.DataTextField = "name";
            ddlDomain.DataSource = oDomain.GetsGroupMaintenance(0, 1);
            ddlDomain.DataBind();
            ddlDomain.Items.Insert(0, new ListItem("-- SELECT --", "0"));
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
                        break;
                    }
                }
            }
        }
        protected void btnContinue_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + "&g=" + oFunction.encryptQueryString(txtFrom.Text) + "&d=" + oFunction.encryptQueryString(ddlDomain.SelectedItem.Value));
        }
        private void LoadObjects()
        {
            string strGroup = oFunction.decryptQueryString(Request.QueryString["g"]);
            txtFrom.Text = strGroup;
            int intDomain = Int32.Parse(oFunction.decryptQueryString(Request.QueryString["d"]));
            ddlDomain.SelectedValue = intDomain.ToString();
            intDomain = Int32.Parse(oDomain.Get(intDomain, "environment"));
            AD oAD = new AD(intProfile, dsn, intDomain);
            lblGroup.Text = oAD.GetUserFullName(strGroup);
            DirectoryEntry oEntry = oAD.UserSearch(strGroup);
            Variables oVariable = new Variables(intDomain);
            if (oEntry != null)
            {
                txtTo.Text = oEntry.Properties["name"].Value.ToString();
                panContinue.Visible = true;
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('The group could not be found.\\n\\nPlease enter a valid group name to continue.');<" + "/" + "script>");
                btnNext.Enabled = false;
            }
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
            oGroupRequest.AddMaintenance(intRequest, intItem, intNumber, "RENAME", txtFrom.Text, Int32.Parse(ddlDomain.SelectedItem.Value));
            oGroupRequest.DeleteMaintenanceParameters(intRequest, intItem, intNumber);
            oGroupRequest.AddMaintenanceParameter(intRequest, intItem, intNumber, txtTo.Text);
            oRequest.UpdateUnNamedRequest(intRequest, "Group Maintenance");
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