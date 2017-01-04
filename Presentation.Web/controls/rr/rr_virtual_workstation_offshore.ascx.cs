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
    public partial class rr_virtual_workstation_offshore : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected int intConfidence = Int32.Parse(ConfigurationManager.AppSettings["CONFIDENCE_100"]);
        protected int intModelVirtual = Int32.Parse(ConfigurationManager.AppSettings["VirtualWorkstationModelID"]);
        protected int intModelVMware = Int32.Parse(ConfigurationManager.AppSettings["VMwareWorkstationModelID"]);
        protected int intCore = Int32.Parse(ConfigurationManager.AppSettings["CoreEnvironmentID"]);
        protected Pages oPage;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected Applications oApplication;
        protected Locations oLocation;
        protected Forecast oForecast;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected OperatingSystems oOperatingSystems;
        protected VirtualHDD oVirtualHDD;
        protected VirtualRam oVirtualRam;
        protected VirtualCPU oVirtualCPU;
        protected Classes oClass;
        protected int intAnswer = 0;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected int intMaxWorkstationsPerDay = Int32.Parse(ConfigurationManager.AppSettings["VirtualWorkstationsMax"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oOperatingSystems = new OperatingSystems(intProfile, dsn);
            oVirtualHDD = new VirtualHDD(intProfile, dsn);
            oVirtualRam = new VirtualRam(intProfile, dsn);
            oVirtualCPU = new VirtualCPU(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
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
                int intAddress = 0;
                if (Request.QueryString["qty"] != null && Request.QueryString["qty"] != "")
                {
                    if (Request.QueryString["qty"] == intMaxWorkstationsPerDay.ToString())
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "qty", "<script type=\"text/javascript\">alert('NOTE: You can request up to " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day.\\n\\nCurrently, you have requested " + Request.QueryString["qty"] + " virtual workstations and cannot be allocated additional hardware until tomorrow.\\n\\nIf your initiative requires more than " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day, you must use design builder.\\nPlease contact your technical lead or ClearView administrator for additional information.');<" + "/" + "script>");
                    else if (Request.QueryString["qty"] == "0")
                    {
                        int intDiff = intMaxWorkstationsPerDay - Int32.Parse(Request.QueryString["qty"]);
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "qty", "<script type=\"text/javascript\">alert('NOTE: You can request up to " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day.\\n\\nCurrently, you have requested " + Request.QueryString["qty"] + " virtual workstations. Please enter a quantity of " + intDiff.ToString() + " or less to continue.\\n\\nIf your initiative requires more than " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day, you must use design builder.\\nPlease contact your technical lead or ClearView administrator for additional information.');<" + "/" + "script>");
                    }
                    else
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "qty", "<script type=\"text/javascript\">alert('NOTE: You can request up to " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day.\\n\\nPlease enter a quantity of " + intMaxWorkstationsPerDay.ToString() + " or less to continue.\\n\\nIf your initiative requires more than " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day, you must use design builder.\\nPlease contact your technical lead or ClearView administrator for additional information.');<" + "/" + "script>");
                    intAddress = intLocation;
                }
                else if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                {
                    intAnswer = Int32.Parse(Request.QueryString["aid"]);
                    if (!IsPostBack)
                    {
                        DataSet dsAnswer = oForecast.GetAnswer(intAnswer);
                        if (dsAnswer.Tables[0].Rows.Count > 0)
                        {
                            txtName.Text = dsAnswer.Tables[0].Rows[0]["name"].ToString();
                            intAddress = Int32.Parse(dsAnswer.Tables[0].Rows[0]["addressid"].ToString());
                            txtQuantity.Text = dsAnswer.Tables[0].Rows[0]["quantity"].ToString();
                        }
                    }
                }
                else
                {
                    DataSet dsService = oForecast.GetAnswerService(Int32.Parse(Request.QueryString["rid"]));
                    if (dsService.Tables[0].Rows.Count > 0)
                        Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + "&aid=" + dsService.Tables[0].Rows[0]["id"].ToString());
                    intAddress = intLocation;
                }
                if (intAddress > 0)
                    txtParent.Text = oLocation.GetFull(intAddress);
                hdnParent.Value = intAddress.ToString();
                btnNext.Attributes.Add("onclick", "return ValidateNumber0('" + txtQuantity.ClientID + "','Please enter a valid quantity')" +
                    " && ValidateDropDown('" + ddlRam.ClientID + "','Please select a RAM')" +
                    " && ValidateDropDown('" + ddlOS.ClientID + "','Please select an operating system')" +
                    " && ValidateDropDown('" + ddlCPU.ClientID + "','Please select a CPU')" +
                    " && ValidateDropDown('" + ddlHardDrive.ClientID + "','Please select a hard drive')" +
                    ";");
            }
            btnCancel1.Attributes.Add("onclick", "return confirm('Are you sure you want to cancel this service request?');");
        }
        private void LoadLists()
        {
            ddlRam.DataTextField = "name";
            ddlRam.DataValueField = "id";
            ddlRam.DataSource = oVirtualRam.GetVirtuals(1);
            ddlRam.DataBind();
            ddlRam.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlOS.DataTextField = "name";
            ddlOS.DataValueField = "id";
            ddlOS.DataSource = oOperatingSystems.Gets(1, 1);
            ddlOS.DataBind();
            ddlOS.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlCPU.DataTextField = "name";
            ddlCPU.DataValueField = "id";
            ddlCPU.DataSource = oVirtualCPU.GetVirtuals(1);
            ddlCPU.DataBind();
            ddlCPU.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlHardDrive.DataTextField = "name";
            ddlHardDrive.DataValueField = "id";
            ddlHardDrive.DataSource = oVirtualHDD.GetVirtuals(1);
            ddlHardDrive.DataBind();
            ddlHardDrive.Items.Insert(0, new ListItem("-- SELECT --", "0"));
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
            DataSet dsToday = oForecast.GetAnswersDay(intWorkstationPlatform, intProfile, 1);
            int intQuantity = 0;
            foreach (DataRow drToday in dsToday.Tables[0].Rows)
                intQuantity += Int32.Parse(drToday["quantity"].ToString());
            int intTotal = Int32.Parse(txtQuantity.Text) + intQuantity;
            if (intTotal <= intMaxWorkstationsPerDay)
            {
                // Add Answer
                if (intAnswer == 0)
                {
                    intAnswer = oForecast.AddAnswer(0, intWorkstationPlatform, 0, intProfile);
                    oForecast.UpdateAnswerStep(intAnswer, 1, 1);
                }
                oForecast.UpdateAnswerService(intAnswer, intRequest);
                int intClass = 0;
                DataSet dsClasses = oClass.Gets(1);
                foreach (DataRow drClass in dsClasses.Tables[0].Rows)
                {
                    if (drClass["prod"].ToString() == "1" && drClass["pnc"].ToString() != "1")
                    {
                        intClass = Int32.Parse(drClass["id"].ToString());
                        break;
                    }
                }
                oForecast.UpdateAnswer(intAnswer, 0, 0, "", 0, Request.ServerVariables["REMOTE_HOST"], "", txtName.Text, Int32.Parse(Request.Form[hdnParent.UniqueID]), intClass, 0, intCore, 0, 0, 0, Int32.Parse(txtQuantity.Text), 0);
                // Add Model
                oForecast.UpdateAnswerModel(intAnswer, intModelVirtual);
                oForecast.DeleteWorkstation(intAnswer);
                oForecast.AddWorkstation(intAnswer, Int32.Parse(ddlRam.SelectedItem.Value), Int32.Parse(ddlOS.SelectedItem.Value), 0, 0, Int32.Parse(ddlHardDrive.SelectedItem.Value), Int32.Parse(ddlCPU.SelectedItem.Value));
                oForecast.UpdateAnswerStep(intAnswer, 1, 1);
                // Add Commitment Date
                oForecast.UpdateAnswer(intAnswer, DateTime.Today, intConfidence, intProfile);
                oForecast.UpdateAnswerStep(intAnswer, 1, 1);
                oRequestItem.UpdateForm(intRequest, true);
                Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
            }
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString() + "&qty=" + intQuantity.ToString());
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            oRequest.Cancel(intRequest);
            Response.Redirect(oPage.GetFullLink(intPage));
        }
    }
}