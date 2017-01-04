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
    public partial class rr_server_rebuild : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected Servers oServer;
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
                int intService = Int32.Parse(lblService.Text);
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
                else if (Request.QueryString["formid"] == null || Request.QueryString["formid"] == "")
                    btnNext.Enabled = false;
                btnContinue.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a server name') && ProcessButton(this) && LoadWait();");
            }
            btnCancel1.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service request?');");
        }
        private void LoadValues()
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            DataSet dsItems;
            int intForm = 0;
            if (Request.QueryString["formid"] != null && Request.QueryString["formid"] != "")
            {
                intForm = Int32.Parse(Request.QueryString["formid"]);
                dsItems = oRequestItem.GetForm(intRequest, intForm);
            }
            else
                dsItems = oRequestItem.GetForms(intRequest);
            if (dsItems.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drItem in dsItems.Tables[0].Rows)
                {
                    if (drItem["done"].ToString() == "-1" || intForm > 0)
                    {
                        lblItem.Text = drItem["itemid"].ToString();
                        lblService.Text = drItem["serviceid"].ToString();
                        if (intForm > 0 && Request.QueryString["num"] != null && Request.QueryString["num"] != "")
                            lblNumber.Text = Request.QueryString["num"];
                        else
                            lblNumber.Text = drItem["number"].ToString();
                        break;
                    }
                }
            }
            else
                lblService.Text = intForm.ToString();
            // Load Data
            if (Request.QueryString["formid"] != null && Request.QueryString["formid"] != "")
            {
                int intItem = Int32.Parse(lblItem.Text);
                int intNumber = Int32.Parse(lblNumber.Text);
                DataSet dsEdit = oServer.GetRebuild(intRequest, intForm, intNumber);
                foreach (DataRow drEdit in dsEdit.Tables[0].Rows)
                {
                    int intID = 0;
                    if (Int32.TryParse(drEdit["serverid"].ToString(), out intID) == true)
                    {
                        lblId.Text = intID.ToString();
                        //txtChange.Text = drEdit["change"].ToString();
                        txtName.Text = lblName.Text = oServer.GetName(intID, true);
                        btnContinue.Enabled = false;
                        panFound.Visible = true;
                        btnNext.Text = "Update";
                        btnBack.Text = "Cancel";
                        //btnNext.Attributes.Add("onclick", "return ValidateTextLength('" + txtChange.ClientID + "', 'Please enter a valid change control number\\n\\n - Must start with either \"CHG\" or \"PTM\"\\n - Must be exactly 10 characters in length', 10, ['CHG','PTM'], ['CHG0000000','PTM0000000','CHG1111111','PTM1111111','CHG9999999','PTM9999999','CHGXXXXXXX','PTMXXXXXXX'])" +
                        //    " && ProcessButton(this) && LoadWait()" +
                        //    ";");
                        btnNext.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait()" +
                            ";");
                        btnCancel1.Visible = false;
                    }
                    break;
                }
            }
        }
        protected void btnContinue_Click(Object Sender, EventArgs e)
        {
            string formid = "";
            if (Request.QueryString["formid"] != null && Request.QueryString["formid"] != "")
                formid = "&formid=" + Request.QueryString["formid"];
            string num = "";
            if (Request.QueryString["num"] != null && Request.QueryString["num"] != "")
                num = "&num=" + Request.QueryString["num"];
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + formid + num + "&n=" + oFunction.encryptQueryString(txtName.Text));
        }
        private void LoadServer()
        {
            string strName = oFunction.decryptQueryString(Request.QueryString["n"]);
            txtName.Text = strName;
            DataSet ds = oServer.Get(strName, false);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intServer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                lblName.Text = oServer.GetName(intServer, true);
                if (ds.Tables[0].Rows[0]["rebuilding"].ToString() == "1")
                {
                    panAlready.Visible = true;
                    btnNext.Enabled = false;
                }
                else
                {
                    panFound.Visible = true;
                    lblId.Text = intServer.ToString();
                    //btnNext.Attributes.Add("onclick", "return ValidateTextLength('" + txtChange.ClientID + "', 'Please enter a valid change control number\\n\\n - Must start with either \"CHG\" or \"PTM\"\\n - Must be exactly 10 characters in length', 10, ['CHG','PTM'], ['CHG0000000','PTM0000000','CHG1111111','PTM1111111','CHG9999999','PTM9999999','CHGXXXXXXX','PTMXXXXXXX'])" +
                    //" && ProcessButton(this) && LoadWait()" +
                    //    ";");
                    btnNext.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait()" +
                        ";");
                }
            }
            else
            {
                panExists.Visible = true;
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
            int intService = Int32.Parse(lblService.Text);
            int intNumber = Int32.Parse(lblNumber.Text);
            DataSet dsRebuild = oServer.GetRebuild(intRequest, intService, intNumber);
            if (dsRebuild.Tables[0].Rows.Count > 0)
                oServer.UpdateRebuild(intRequest, intService, intNumber, Int32.Parse(lblId.Text), "");
            else
                oServer.AddRebuild(intRequest, intService, intNumber, Int32.Parse(lblId.Text), "");
            if (btnNext.Text != "Update")
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