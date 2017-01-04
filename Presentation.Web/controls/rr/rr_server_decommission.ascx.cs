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
    public partial class rr_server_decommission : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected Servers oServer;
        protected Asset oAsset;
        protected Applications oApplication;
        protected Forecast oForecast;
        protected Functions oFunction;
        protected Users oUser;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oApplication = new Applications(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oUser = new Users(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rid"] != "" && Request.QueryString["rid"] != null)
            {
                LoadValues();
                int intItem = Int32.Parse(lblItem.Text);
                int intApp = oRequestItem.GetItemApplication(intItem);
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable != "")
                {
                    panDeliverable.Visible = true;
                    btnDeliverable.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','" + strDeliverable + "');");
                }
                if (Request.QueryString["n"] != null && Request.QueryString["n"] != "")
                {
                    if (!IsPostBack)
                        LoadServer();
                }
                else
                    btnNext.Enabled = false;
                if (panChange.Visible == true)
                {
                    btnContinue.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a server name')" +
                        " && ValidateTextLength('" + txtChange.ClientID + "','Please enter a valid change control number\\n\\n - Must start with \"CHG\"\\n - Must be exactly 10 characters in length', 10, 'CHG')" +
                        ";");
                }
                else
                {
                    btnContinue.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a server name')" +
                        ";");
                }
            }
            txtName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnContinue.ClientID + "').click();return false;}} else {return true}; ");
            imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
            btnCancel1.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service request?');");
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
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + "&n=" + oFunction.encryptQueryString(txtName.Text.Trim()));
        }
        private void LoadServer()
        {
            string strName = oFunction.decryptQueryString(Request.QueryString["n"]);
            txtName.Text = strName;
            DataSet ds = oServer.Get(strName, true);
            if (ds.Tables[0].Rows.Count == 1)
            {
                int intAnswer = 0;
                int intProject = 0;
                if (ds.Tables[0].Rows[0]["answerid"].ToString() != "")
                    intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                if (ds.Tables[0].Rows[0]["projectid"].ToString() != "")
                    intProject = Int32.Parse(ds.Tables[0].Rows[0]["projectid"].ToString());
                if (intProject > 0 && intAnswer > 0)
                {
                    int intOwner = Int32.Parse(oForecast.GetAnswer(intAnswer, "appcontact"));
                    int intPrimary = Int32.Parse(oForecast.GetAnswer(intAnswer, "admin1"));
                    int intSecondary = Int32.Parse(oForecast.GetAnswer(intAnswer, "admin2"));
                    int intRequestor = Int32.Parse(oForecast.GetAnswer(intAnswer, "userid"));
                    if (intProfile == intOwner || intProfile == intPrimary || intProfile == intSecondary || intProfile == intRequestor)
                    {
                        panShow.Visible = true;
                        panValid.Visible = true;
                        lblId.Text = ds.Tables[0].Rows[0]["id"].ToString();
                        btnNext.Attributes.Add("onclick", "return ValidateDate('" + txtDate.ClientID + "','Please enter a valid date') && ValidateText('" + txtReason.ClientID + "','Please enter a reason');");
                    }
                    else
                    {
                        panInvalid.Visible = true;
                        lblOwner.Text = oUser.GetFullName(intOwner) + " (" + oUser.GetName(intOwner) + ")";
                        lblPrimary.Text = oUser.GetFullName(intPrimary) + " (" + oUser.GetName(intPrimary) + ")";
                        lblSecondary.Text = oUser.GetFullName(intSecondary) + " (" + oUser.GetName(intSecondary) + ")";
                        lblRequestor.Text = oUser.GetFullName(intRequestor) + " (" + oUser.GetName(intRequestor) + ")";
                        btnNext.Enabled = false;
                    }
                }
                else
                {
                    panStatus.Visible = true;
                    btnNext.Enabled = false;
                }
            }
            else
            {
                panExist.Visible = true;
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
            string strName = oFunction.decryptQueryString(Request.QueryString["n"]);
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            int intItem = Int32.Parse(lblItem.Text);
            int intNumber = Int32.Parse(lblNumber.Text);
            int intServer = Int32.Parse(lblId.Text);
            DataSet dsAssets = oServer.GetAssets(intServer);
            foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
            {
                if (drAsset["latest"].ToString() == "1" || drAsset["dr"].ToString() == "1")
                {
                    bool boolUnique = oAsset.AddDecommission(intRequest, intItem, intNumber, Int32.Parse(drAsset["assetid"].ToString()), intProfile, txtReason.Text, DateTime.Parse(txtDate.Text), strName + (drAsset["dr"].ToString() == "1" ? "-DR" : ""), (drAsset["dr"].ToString() == "1" ? 1 : 0), "");
                }
            }
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