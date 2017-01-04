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
    public partial class rr_virtual_workstation_rebuild : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected Workstations oWorkstation;
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
            oWorkstation = new Workstations(intProfile, dsn);
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
                        LoadWorkstation();
                }
                else if (Request.QueryString["formid"] == null || Request.QueryString["formid"] == "")
                    btnNext.Enabled = false;
                btnContinue.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a workstation name') && ProcessButton(this) && LoadWait();");
            }
            btnCancel1.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service request?');");
            imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
        }
        private void LoadValues()
        {
            //int intRequest = Int32.Parse(Request.QueryString["rid"]);
            //DataSet dsItems = oRequestItem.GetForms(intRequest);
            //if (dsItems.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow drItem in dsItems.Tables[0].Rows)
            //    {
            //        if (drItem["done"].ToString() == "-1")
            //        {
            //            lblItem.Text = drItem["itemid"].ToString();
            //            lblService.Text = drItem["serviceid"].ToString();
            //            lblNumber.Text = drItem["number"].ToString();
            //            break;
            //        }
            //    }
            //}
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
                DataSet dsEdit = oWorkstation.GetVirtualRebuild(intRequest, intForm, intNumber);
                foreach (DataRow drEdit in dsEdit.Tables[0].Rows)
                {
                    if (drEdit["cancelled"].ToString() == "")
                    {
                        int intName = 0;
                        if (Int32.TryParse(drEdit["nameid"].ToString(), out intName) == true)
                        {
                            lblId.Text = drEdit["workstationid"].ToString();
                            txtDate.Text = DateTime.Parse(drEdit["scheduled"].ToString()).ToShortDateString();
                            txtName.Text = lblName.Text = oWorkstation.GetName(intName);
                            btnContinue.Enabled = false;
                            panFound.Visible = true;
                            btnNext.Text = "Update";
                            btnBack.Text = "Cancel";
                            btnNext.Attributes.Add("onclick", "return ValidateDate('" + txtDate.ClientID + "','Please enter a valid date')" +
                                " && ValidateDateToday('" + txtDate.ClientID + "','The date must occur after today')" +
                                " && ProcessButton(this) && LoadWait()" +
                                ";");
                            btnCancel1.Visible = false;
                        }
                        break;
                    }
                }
            }
        }
        protected void btnContinue_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + "&n=" + oFunction.encryptQueryString(txtName.Text));
        }
        private void LoadWorkstation()
        {
            string strName = oFunction.decryptQueryString(Request.QueryString["n"]);
            txtName.Text = strName;
            DataSet ds = oWorkstation.GetName(strName);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intName = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                DataSet dsWorkstation = oWorkstation.GetVirtualName(intName);
                if (dsWorkstation.Tables[0].Rows.Count > 0)
                {
                    bool boolAlready = false;
                    int intID = Int32.Parse(dsWorkstation.Tables[0].Rows[0]["id"].ToString());
                    lblName.Text = oWorkstation.GetName(Int32.Parse(dsWorkstation.Tables[0].Rows[0]["nameid"].ToString()));
                    DataSet dsRebuild = oWorkstation.GetVirtualRebuild(intID);
                    foreach (DataRow drRebuild in dsRebuild.Tables[0].Rows)
                    {
                        if (drRebuild["submitted"].ToString() != "" && drRebuild["scheduled"].ToString() != "" && drRebuild["completed"].ToString() == "" && drRebuild["cancelled"].ToString() == "")
                        {
                            // Submitted, has not completed, is scheduled and is not cancelled
                            DateTime datPower = DateTime.Parse(drRebuild["scheduled"].ToString());
                            lblAlreadyOn.Text = drRebuild["submitted"].ToString();
                            lblAlreadyBy.Text = oUser.GetFullName(oRequest.GetUser(Int32.Parse(drRebuild["requestid"].ToString())));
                            lblAlreadyPower.Text = txtDate.Text = datPower.ToShortDateString();
                            // status - to do
                            if (drRebuild["started"].ToString() != "")
                            {
                                lblAlreadyStatus.Text = "Powered off on " + DateTime.Parse(drRebuild["turnedoff"].ToString()).ToLongDateString();
                                lblAlreadyStatus.Text += "<br/>Rebuild started on " + DateTime.Parse(drRebuild["started"].ToString()).ToLongDateString();
                            }
                            else if (drRebuild["rebuild"].ToString() != "")
                            {
                                lblAlreadyStatus.Text = "Powered off on " + DateTime.Parse(drRebuild["turnedoff"].ToString()).ToLongDateString();
                                lblAlreadyStatus.Text += "<br/>Will be rebuilt on " + DateTime.Parse(drRebuild["rebuild"].ToString()).ToLongDateString();
                            }
                            else if (drRebuild["turnedoff"].ToString() != "")
                                lblAlreadyStatus.Text = "Powered off on " + DateTime.Parse(drRebuild["turnedoff"].ToString()).ToLongDateString();
                            else
                                lblAlreadyStatus.Text = "Will be powered off on " + DateTime.Parse(drRebuild["scheduled"].ToString()).ToLongDateString();

                            boolAlready = true;
                            break;
                        }
                    }
                    
                    if (boolAlready == false)
                    {
                        panFound.Visible = true;
                        lblId.Text = intID.ToString();
                        btnNext.Attributes.Add("onclick", "return ValidateDate('" + txtDate.ClientID + "','Please enter a valid date')" +
                            " && ValidateDateToday('" + txtDate.ClientID + "','The date must occur after today')" +
                            " && ProcessButton(this) && LoadWait()" +
                            ";");
                    }
                    else
                    {
                        panAlready.Visible = true;
                        btnNext.Enabled = false;
                    }
                }
            }
            else
            {
                panWorkstation.Visible = true;
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
            bool boolSubmitted = false; 
            DataSet dsRebuild = oWorkstation.GetVirtualRebuild(intRequest, intService, intNumber);
            foreach (DataRow drRebuild in dsRebuild.Tables[0].Rows)
            {
                if (drRebuild["cancelled"].ToString() == "")
                {
                    oWorkstation.UpdateVirtualRebuildCancel(intRequest, intService, intNumber);
                    boolSubmitted = (drRebuild["submitted"].ToString() != "");
                    break;
                }
            }
            oWorkstation.AddVirtualRebuild(Int32.Parse(lblId.Text), intRequest, intService, intNumber, txtDate.Text);
            if (boolSubmitted)
                oWorkstation.UpdateVirtualRebuild(intRequest, intService, intNumber);
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