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
    public partial class rr_server_retrieve : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intPlatform = Int32.Parse(ConfigurationManager.AppSettings["ServerPlatformID"]);
        protected Pages oPage;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected Applications oApplication;
        protected Variables oVariable;
        protected RequestFields oRequestField;
        protected Services oService;
        protected Customized oCustomized;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oRequestField = new RequestFields(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
            {
                if (!IsPostBack)
                    LoadLists();
                if (Request.QueryString["q"] != null && Request.QueryString["q"] != "")
                    txtServer.Text = Request.QueryString["q"];
                LoadValues();
                btnNext.Attributes.Add("onclick", "return ValidateText('" + txtServer.ClientID + "','Please enter a server name')" +
                    " && ValidateRadioButtons('" + radPhysical.ClientID + "','" + radVirtual.ClientID + "','Please select the destination server type for this retrieval')" +
                    " && (document.getElementById('" + radPhysical.ClientID + "').checked == false || (document.getElementById('" + radPhysical.ClientID + "').checked == true && ValidateDropDown('" + ddlModel.ClientID + "','Please select the preferred server model')))" +
                    " && ValidateText('" + txtCode.ClientID + "','Please enter an application code')" +
                    " && ValidateDropDown('" + ddlClass.ClientID + "','Please select a class')" +
                    " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid date of completion')" +
                    " && ValidateCheck('" + chkAgreement.ClientID + "','Please check that you have read and agreed to the disclaimer')" +
                    ";");
                radPhysical.Attributes.Add("onclick", "ShowHideDiv('" + divPhysical.ClientID + "','inline');");
                radVirtual.Attributes.Add("onclick", "ShowHideDiv('" + divPhysical.ClientID + "','none');");
                imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
                btnSearch.Attributes.Add("onclick", "return ShowTextInfo('" + txtServer.ClientID + "','ARCHIVED_SEARCH');");
                txtServer.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");
                int intItem = Int32.Parse(lblItem.Text);
                int intApp = oRequestItem.GetItemApplication(intItem);
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable != "")
                {
                    panDeliverable.Visible = true;
                    btnDeliverable.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','" + strDeliverable + "');");
                }
            }
            btnCancel1.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service request?');");
        }
        private void LoadLists()
        {
            Classes oClasses = new Classes(intProfile, dsn);
            ddlClass.DataValueField = "id";
            ddlClass.DataTextField = "name";
            ddlClass.DataSource = oClasses.Gets(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            Types oType = new Types(intProfile, dsn);
            Models oModel = new Models(intProfile, dsn);
            DataSet ds = oType.Gets(intPlatform, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DataSet ds2 = oModel.Gets(Int32.Parse(dr["id"].ToString()), 0);
                foreach (DataRow dr2 in ds2.Tables[0].Rows)
                    ddlModel.Items.Insert(0, new ListItem(dr["name"].ToString() + " | " + dr2["name"].ToString(), dr2["id"].ToString()));
            }
            ddlModel.Items.Insert(0, new ListItem("-- SELECT --", "0"));
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
            string strBack = "Physical";
            if (radVirtual.Checked == true)
                strBack = "Virtual";
            oCustomized.AddServerRetrieve(intRequest, intItem, intNumber, txtServer.Text, strBack, Int32.Parse(ddlModel.SelectedItem.Value), txtCode.Text, Int32.Parse(ddlClass.SelectedItem.Value), DateTime.Parse(txtDate.Text), txtStatement.Text);
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