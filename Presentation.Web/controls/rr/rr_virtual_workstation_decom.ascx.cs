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
    public partial class rr_virtual_workstation_decom : System.Web.UI.UserControl
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
                else
                    btnNext.Enabled = false;
                btnContinue.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a workstation name') && ProcessButton(this) && LoadWait();");
            }
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
                    if (dsWorkstation.Tables[0].Rows[0]["deleted"].ToString() != "0")
                    {
                        panDeleted.Visible = true;
                        btnNext.Enabled = false;
                    }
                    else
                    {
                        bool boolAlready = false;
                        int intAsset = Int32.Parse(dsWorkstation.Tables[0].Rows[0]["assetid"].ToString());
                        Asset oAsset = new Asset(intProfile, dsnAsset);
                        DataSet dsDecom = oAsset.GetDecommission(intAsset, strName, true);
                        if (dsDecom.Tables[0].Rows.Count > 0)
                        {
                            DataRow drDecom = dsDecom.Tables[0].Rows[0];
                            boolAlready = true;
                            lblAlreadySerial.Text = oAsset.Get(intAsset, "serial");
                            lblAlreadyBy.Text = oUser.GetFullName(Int32.Parse(drDecom["userid"].ToString()));
                            lblAlreadyOn.Text = drDecom["created"].ToString();
                            lblAlreadyPower.Text = DateTime.Parse(drDecom["decom"].ToString()).ToLongDateString();
                            if (drDecom["running"].ToString() == "-1")
                                lblAlreadyStatus.Text = "Manual Intervention Required";
                            else if (drDecom["running"].ToString() == "-2")
                            {
                                boolAlready = false;
                                lblAlreadyStatus.Text = "Cancelled";
                            }
                            else if (drDecom["running"].ToString() == "2")
                                lblAlreadyStatus.Text = "In Progress";
                            else if (drDecom["running"].ToString() == "3")
                                lblAlreadyStatus.Text = "Completed";
                            else if (drDecom["running"].ToString() == "1")
                                lblAlreadyStatus.Text = "Running...";
                            else if (drDecom["running"].ToString() == "0")
                            {
                                if (drDecom["recommissioned"].ToString() != "")
                                {
                                    boolAlready = false;
                                    lblAlreadyStatus.Text = "Recommissioned on " + DateTime.Parse(drDecom["recommissioned"].ToString()).ToLongDateString();
                                }
                                else if (drDecom["destroyed"].ToString() != "")
                                {
                                    lblAlreadyStatus.Text = "Powered off on " + DateTime.Parse(drDecom["turnedoff"].ToString()).ToLongDateString();
                                    lblAlreadyStatus.Text += "<br/>Finished on " + DateTime.Parse(drDecom["destroyed"].ToString()).ToLongDateString();
                                }
                                else if (drDecom["destroy"].ToString() != "")
                                {
                                    lblAlreadyStatus.Text = "Powered off on " + DateTime.Parse(drDecom["turnedoff"].ToString()).ToLongDateString();
                                    lblAlreadyStatus.Text += "<br/>Will be finished on " + DateTime.Parse(drDecom["destroy"].ToString()).ToLongDateString();
                                }
                                else if (drDecom["turnedoff"].ToString() != "")
                                    lblAlreadyStatus.Text = "Powered off on " + DateTime.Parse(drDecom["turnedoff"].ToString()).ToLongDateString();
                                else
                                    lblAlreadyStatus.Text = "Will be powered off on " + DateTime.Parse(drDecom["decom"].ToString()).ToLongDateString();
                            }
                            else
                                lblAlreadyStatus.Text = "Status Unavailable";
                        }

                        if (boolAlready == false)
                        {
                            //int intAnswer = Int32.Parse(dsWorkstation.Tables[0].Rows[0]["answerid"].ToString());
                            //int intOwner = Int32.Parse(oForecast.GetAnswer(intAnswer, "appcontact"));
                            //if (intProfile == intOwner || oApplication.Get(intApplication, "decom") == "1" || oUser.IsAdmin(intProfile))
                            //{
                            panValid.Visible = true;
                            lblId.Text = intName.ToString();
                            //}
                            //else
                            //{
                            //    panInvalid.Visible = true;
                            //    lblOwner.Text = oUser.GetFullName(intOwner);
                            //    btnNext.Enabled = false;
                            //}
                        }
                        else
                        {
                            panAlready.Visible = true;
                            btnNext.Enabled = false;
                        }
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
            int intItem = Int32.Parse(lblItem.Text);
            int intNumber = Int32.Parse(lblNumber.Text);
            oWorkstation.AddVirtualDecommission(intRequest, intItem, intNumber, Int32.Parse(lblId.Text));
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