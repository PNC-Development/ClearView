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
    public partial class rr_virtual_workstation_vmware : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intConfidence = Int32.Parse(ConfigurationManager.AppSettings["CONFIDENCE_100"]);
        protected int intModelVirtual = Int32.Parse(ConfigurationManager.AppSettings["VirtualWorkstationModelID"]);
        protected int intModelVMware = Int32.Parse(ConfigurationManager.AppSettings["VMwareWorkstationModelID"]);
        protected int intCore = Int32.Parse(ConfigurationManager.AppSettings["CoreEnvironmentID"]);
        //protected int intXP = Int32.Parse(ConfigurationManager.AppSettings["OS_XP"]);
        protected Pages oPage;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected Applications oApplication;
        protected Locations oLocation;
        protected Classes oClass;
        protected Forecast oForecast;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected OperatingSystems oOperatingSystems;
        protected VirtualHDD oVirtualHDD;
        protected VirtualRam oVirtualRam;
        protected VirtualCPU oVirtualCPU;
        protected Services oService;
        protected Functions oFunction;
        protected int intAnswer = 0;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected int intMaxWorkstationsPerDay = Int32.Parse(ConfigurationManager.AppSettings["VMwareWorkstationsMax"]);
        private bool boolReqDenied = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oOperatingSystems = new OperatingSystems(intProfile, dsn);
            oVirtualHDD = new VirtualHDD(intProfile, dsn);
            oVirtualRam = new VirtualRam(intProfile, dsn);
            oVirtualCPU = new VirtualCPU(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            if (Request.QueryString["denied"] != null)
                boolReqDenied = true;

            if (Request.QueryString["rid"] != "" && Request.QueryString["rid"] != null)
            {
                if (!IsPostBack)
                    LoadLists();
                panNavigation.Visible = true;
                LoadValues();
                int intItem = Int32.Parse(lblItem.Text);
                int intService = Int32.Parse(lblService.Text);
                int intApp = oRequestItem.GetItemApplication(intItem);
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable != "")
                {
                    btnDeliverable.Visible = true;
                    btnDeliverable.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','" + strDeliverable + "');");
                }
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
                            ddlLocation.SelectedValue = dsAnswer.Tables[0].Rows[0]["addressid"].ToString();
                            ddlClass.SelectedValue = dsAnswer.Tables[0].Rows[0]["classid"].ToString();
                            txtQuantity.Text = dsAnswer.Tables[0].Rows[0]["quantity"].ToString();
                            dsAnswer = oForecast.GetWorkstation(intAnswer);
                            if (dsAnswer.Tables[0].Rows.Count > 0)
                            {
                                ddlOS.SelectedValue = dsAnswer.Tables[0].Rows[0]["osid"].ToString();
                                ChangeOS();
                                chkDR.Checked = (dsAnswer.Tables[0].Rows[0]["recovery"].ToString() == "1");
                                radEmployee.SelectedValue = dsAnswer.Tables[0].Rows[0]["internal"].ToString();
                                ddlRam.SelectedValue = dsAnswer.Tables[0].Rows[0]["ramid"].ToString();
                                ddlHardDrive.SelectedValue = dsAnswer.Tables[0].Rows[0]["hddid"].ToString();
                                ddlCPU.SelectedValue = dsAnswer.Tables[0].Rows[0]["cpuid"].ToString();
                            }
                        }
                    }
                    if (Request.QueryString["formid"] != null && Request.QueryString["formid"] != "")
                    {
                        panUpdate.Visible = true;
                        panNavigation.Visible = false;
                        if (boolReqDenied)
                        {
                            int intRequest = Int32.Parse(Request.QueryString["rid"]);
                            int intNumber = Int32.Parse(lblNumber.Text);
                            DataSet dsSelected2 = oService.GetSelected(intRequest, intService, intNumber);
                            for (int ii = intNumber; ii > 0 && dsSelected2.Tables[0].Rows.Count == 0; ii--)
                                dsSelected2 = oService.GetSelected(intRequest, intService, ii - 1);
                            if (dsSelected2.Tables[0].Rows.Count > 0)
                            {
                                if (Int32.Parse(dsSelected2.Tables[0].Rows[0]["approved"].ToString()) < 0)
                                {
                                    // Rejected
                                    lblReqDenyCommentValue.Text = oFunction.FormatText(dsSelected2.Tables[0].Rows[0]["reason"].ToString());
                                    pnlReqDenied.Visible = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    string strFormID = "";
                    if (Request.QueryString["formid"] != null && Request.QueryString["formid"] != "")
                        strFormID = "&formid=" + Request.QueryString["formid"];
                    string strNumID = "";
                    if (Request.QueryString["num"] != null && Request.QueryString["num"] != "")
                        strNumID = "&num=" + Request.QueryString["num"];
                    string strDenied = "";
                    if (Request.QueryString["denied"] != null && Request.QueryString["denied"] != "")
                        strDenied = "&denied=" + Request.QueryString["denied"];
                    DataSet dsService = oForecast.GetAnswerService(Int32.Parse(Request.QueryString["rid"]));
                    if (dsService.Tables[0].Rows.Count > 0)
                        Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + strFormID + strNumID + strDenied + "&aid=" + dsService.Tables[0].Rows[0]["id"].ToString());
                }
                btnNext.Attributes.Add("onclick", "return ValidateDropDown('" + ddlClass.ClientID + "','Please select a class')" +
                    " && ValidateNumber0('" + txtQuantity.ClientID + "','Please enter a valid quantity')" +
                    " && ValidateDropDown('" + ddlOS.ClientID + "','Please select an Operating System')" +
                    " && ValidateDropDown('" + ddlRam.ClientID + "','Please select a RAM')" +
                    " && ValidateDropDown('" + ddlCPU.ClientID + "','Please select a CPU')" +
                    " && ValidateDropDown('" + ddlHardDrive.ClientID + "','Please select a hard drive')" +
                    ";");
            }
            btnCancelR.Attributes.Add("onclick", "return confirm('Are you sure you want to cancel this service request?');");
            ddlOS.Attributes.Add("onchange", "LoadWait();");
        }
        private void LoadLists()
        {
            ddlClass.DataTextField = "name";
            ddlClass.DataValueField = "id";
            ddlClass.DataSource = oClass.GetWorkstationVMwares(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlOS.DataTextField = "name";
            ddlOS.DataValueField = "id";
            ddlOS.DataSource = oOperatingSystems.Gets(1, 1);
            ddlOS.DataBind();
            ddlOS.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            radEmployee.SelectedValue = "0";
        }
        protected void ddlOS_Change(Object Sender, EventArgs e)
        {
            ChangeOS();
        }
        protected void ChangeOS()
        {
            int intOS = Int32.Parse(ddlOS.SelectedItem.Value);
            ddlRam.Items.Clear();
            ddlCPU.Items.Clear();
            ddlHardDrive.Items.Clear();

            if (intOS > 0)
            {
                bool boolXP = (oOperatingSystems.Get(intOS, "code") == "XP");
                bool boolWin7 = (oOperatingSystems.Get(intOS, "code") == "7");

                ddlRam.Enabled = true;
                ddlRam.DataTextField = "name";
                ddlRam.DataValueField = "id";
                ddlRam.DataSource = oVirtualRam.GetVMwares((boolXP ? 1 : 0), (boolWin7 ? 1 : 0), 1);
                ddlRam.DataBind();
                ddlRam.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlCPU.Enabled = true;
                ddlCPU.DataTextField = "name";
                ddlCPU.DataValueField = "id";
                ddlCPU.DataSource = oVirtualCPU.GetVMwares((boolXP ? 1 : 0), (boolWin7 ? 1 : 0), 1);
                ddlCPU.DataBind();
                ddlCPU.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlHardDrive.Enabled = true;
                ddlHardDrive.DataTextField = "name";
                ddlHardDrive.DataValueField = "id";
                ddlHardDrive.DataSource = oVirtualHDD.GetVMwares((boolXP ? 1 : 0), (boolWin7 ? 1 : 0), 1);
                ddlHardDrive.DataBind();
                ddlHardDrive.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            }
            else
            {
                ddlRam.Enabled = false;
                ddlRam.Items.Insert(0, new ListItem("-- Select an Operating System --", "0"));
                ddlCPU.Enabled = false;
                ddlCPU.Items.Insert(0, new ListItem("-- Select an Operating System --", "0"));
                ddlHardDrive.Enabled = false;
                ddlHardDrive.Items.Insert(0, new ListItem("-- Select an Operating System --", "0"));
            }
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
                        if (intForm > 0 && Request.QueryString["num"] != null && Request.QueryString["num"] != "")
                            lblNumber.Text = Request.QueryString["num"];
                        else
                            lblNumber.Text = drItem["number"].ToString();
                        lblService.Text = drItem["serviceid"].ToString();
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
            Save(intRequest);
            oRequestItem.UpdateForm(intRequest, true);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            Save(intRequest);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        private void Save(int intRequest)
        {
            int intItem = Int32.Parse(lblItem.Text);
            int intService = Int32.Parse(lblService.Text);
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
                oForecast.UpdateAnswer(intAnswer, 0, 0, "", 0, Request.ServerVariables["REMOTE_HOST"], "", txtName.Text, Int32.Parse(ddlLocation.SelectedItem.Value), Int32.Parse(ddlClass.SelectedItem.Value), 0, intCore, 0, 0, 0, Int32.Parse(txtQuantity.Text), 0);
                // Add Model
                oForecast.UpdateAnswerModel(intAnswer, intModelVMware);
                oForecast.DeleteWorkstation(intAnswer);
                oForecast.AddWorkstation(intAnswer, Int32.Parse(ddlRam.SelectedItem.Value), Int32.Parse(ddlOS.SelectedItem.Value), (chkDR.Checked ? 1 : 0), Int32.Parse(radEmployee.SelectedItem.Value), Int32.Parse(ddlHardDrive.SelectedItem.Value), Int32.Parse(ddlCPU.SelectedItem.Value));
                oForecast.UpdateAnswerStep(intAnswer, 1, 1);
                // Add Commitment Date
                oForecast.UpdateAnswer(intAnswer, DateTime.Today, intConfidence, intProfile);
                oForecast.UpdateAnswerStep(intAnswer, 1, 1);

                if (boolReqDenied)
                    oService.UpdateSelectedApprove(intRequest, intService, Int32.Parse(lblNumber.Text), 0, 0, DateTime.Now, "");
            }
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString() + "&qty=" + intQuantity.ToString());
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnCancelR_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            oRequest.Cancel(intRequest);
            Response.Redirect(oPage.GetFullLink(intPage));
        }
    }
}