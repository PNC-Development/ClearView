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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class fore_design : DesignControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Design oDesign;
        protected Pages oPage;
        protected Mnemonic oMnemonic;
        protected CostCenter oCostCenter;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected Locations oLocation;
        protected OperatingSystems oOperatingSystem;
        protected Holidays oHoliday;
        protected ModelsProperties oModelsProperties;
        protected Users oUser;
        protected Functions oFunction;
        protected Forecast oForecast;
        protected Requests oRequest;
        protected Projects oProject;
        protected int intApplication = 0;
        protected int intPage = 0;
        private double dblSLA = 10.00;
        //private string strRequiredClass = "reddefault";
        //private string strRequiredText = "** REQUIRED **";
        protected string strHighlight = "#FFEE99";
        protected bool boolDemo = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oDesign = new Design(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oMnemonic = new Mnemonic(intProfile, dsn);
            oCostCenter = new CostCenter(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            oHoliday = new Holidays(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oForecast = new Forecast(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);

            //if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                //intID = Int32.Parse(Request.QueryString["id"]);
            if (!IsPostBack)
            {
                DataSet dsSummary = oDesign.Get(this.DesignId);
                if (dsSummary.Tables[0].Rows.Count > 0)
                {
                    int intModel = oDesign.GetModelProperty(this.DesignId);
                    lblSummarySolution.Text = oModelsProperties.Get(intModel, "name");

                    DataRow drSummary = dsSummary.Tables[0].Rows[0];
                    bool boolWeb = (drSummary["web"].ToString() == "1");
                    bool boolSQL = oDesign.IsSQL(this.DesignId);
                    bool boolOracle = oDesign.IsOracle(this.DesignId);
                    bool boolOtherDB = (drSummary["other_db"].ToString() == "1");

                    DataSet dsSubmitted = oDesign.GetSubmitted(this.DesignId);
                    if (dsSubmitted.Tables[0].Rows.Count > 0)
                    {
                        trSubmitted.Visible = true;
                        int intUser = Int32.Parse(dsSubmitted.Tables[0].Rows[0]["userid"].ToString());
                        lblSummarySubmittedBy.Text = oUser.GetFullName(intUser) + " (" + oUser.GetName(intUser) + ")";
                        lblSummarySubmittedOn.Text = dsSubmitted.Tables[0].Rows[0]["created"].ToString();
                        if (dsSubmitted.Tables[0].Rows[0]["comments"].ToString() != "") 
                        {
                            trException1.Visible = true;
                            trException2.Visible = true;
                            trException3.Visible = true;
                            lblException.Text = oFunction.FormatText(dsSubmitted.Tables[0].Rows[0]["comments"].ToString());
                            lblExceptionID.Text = dsSubmitted.Tables[0].Rows[0]["exceptionID"].ToString();
                        }
                    }

                    // Project
                    int intForecast = 0;
                    int intRequest = 0;
                    int intProject = 0;
                    Int32.TryParse(drSummary["forecastid"].ToString(), out intForecast);
                    if (intForecast > 0)
                    {
                        Int32.TryParse(oForecast.Get(intForecast, "requestid"), out intRequest);
                        if (intRequest > 0)
                        {
                            intProject = oRequest.GetProjectNumber(intRequest);
                            if (intProject > 0)
                            {
                                lblSummaryProjectName.Text = oProject.Get(intProject, "name");
                                string strNumber = oProject.Get(intProject, "number");
                                lblSummaryProjectNumber.Text = strNumber;
                                // Check to see if Demo
                                DataSet dsDemo = oFunction.GetSetupValuesByKey("DEMO_PROJECT");
                                foreach (DataRow drDemo in dsDemo.Tables[0].Rows)
                                {
                                    if (strNumber == drDemo["Value"].ToString())
                                    {
                                        boolDemo = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // Mnemonic
                    int intMnemonic = 0;
                    Int32.TryParse(drSummary["mnemonicid"].ToString(), out intMnemonic);
                    if (intMnemonic > 0)
                        lblSummaryMnemonic.Text = oMnemonic.Get(intMnemonic, "factory_code") + " - " + oMnemonic.Get(intMnemonic, "name");
                    // Class + Environment + Location
                    int intClass = 0;
                    int intEnv = 0;
                    int intAddress = 0;
                    Int32.TryParse(drSummary["classid"].ToString(), out intClass);
                    if (intClass > 0)
                    {
                        lblSummaryLocation.Text = oClass.Get(intClass, "name") + ", ";
                        Int32.TryParse(drSummary["environmentid"].ToString(), out intEnv);
                        if (intEnv > 0)
                        {
                            lblSummaryLocation.Text += oEnvironment.Get(intEnv, "name");
                            Int32.TryParse(drSummary["addressid"].ToString(), out intAddress);
                            if (intAddress > 0)
                                lblSummaryLocation.Text += " at <a href=\"javascript:void(0);\" onclick=\"ShowHideDiv2('divLocation');\">" + oLocation.GetFull(intAddress) + " <img src=\"/images/help.gif\" border=\"0\" align=\"absmiddle\"/></a>";
                            else
                            {
                                bool boolProd = oClass.IsProd(intClass);
                                bool boolQA = oClass.IsQA(intClass);
                                bool boolTest = oClass.IsTestDev(intClass);
                                bool boolDR = oClass.IsDR(intClass);
                                DataSet dsLocations = oLocation.GetAddressClass((boolDR ? 1 : 0), (boolProd ? 1 : 0), (boolQA ? 1 : 0), (boolTest ? 1 : 0));
                                if (dsLocations.Tables[0].Rows.Count > 0)
                                {
                                    StringBuilder strLocation = new StringBuilder();
                                    int intLocationCount = 0;
                                    foreach (DataRow drLocation in dsLocations.Tables[0].Rows)
                                    {
                                        intLocationCount++;
                                        if (intLocationCount > 1)
                                        {
                                            if (intLocationCount > 2)
                                                strLocation.Insert(0, ", ");
                                            else
                                                strLocation.Insert(0, " or ");
                                        }
                                        if (drLocation["commonname"].ToString() != "")
                                            strLocation.Insert(0, drLocation["commonname"].ToString());
                                        else
                                            strLocation.Insert(0, oLocation.GetFull(Int32.Parse(drLocation["id"].ToString())));
                                    }
                                    lblSummaryLocation.Text += " at <a href=\"javascript:void(0);\" onclick=\"ShowHideDiv2('divLocation');\">" + strLocation.ToString() + " <img src=\"/images/help.gif\" border=\"0\" align=\"absmiddle\"/></a>";
                                }
                            }
                        }
                    }
                    // Server Type
                    if (oDesign.IsDatabase(this.DesignId) == false)
                    {
                        if (boolWeb)
                            lblSummaryServerType.Text = "Web";
                        else
                            lblSummaryServerType.Text = "Application";
                    }
                    else
                    {
                        lblSummaryServerType.Text = "Database";
                        if (boolSQL || boolOracle || boolOtherDB)
                        {
                            string strDatabase = "";
                            if (boolSQL == true)
                                strDatabase = "SQL";
                            if (boolOracle == true)
                                strDatabase = "Oracle";
                            if (boolOtherDB == true)
                                strDatabase = "Other";
                            lblSummaryServerType.Text += " (" + strDatabase + ")";
                        }
                        if (boolWeb)
                            lblSummaryServerType.Text += " + Web";
                    }
                    // Quantity
                    int intQuantity = 0;
                    Int32.TryParse(drSummary["quantity"].ToString(), out intQuantity);
                    if (intQuantity > 0)
                        lblSummaryQuantity.Text = intQuantity.ToString();
                    // OS
                    int intOS = 0;
                    Int32.TryParse(drSummary["osid"].ToString(), out intOS);
                    if (intOS > 0)
                        lblSummaryOS.Text = oOperatingSystem.Get(intOS, "name");
                    // SIZE
                    string strSize = drSummary["cores"].ToString() + " CPU(s), " + drSummary["ram"].ToString() + " GB(s) RAM";
                    lblSummarySize.Text = strSize;
                    // STORAGE
                    if (drSummary["storage"].ToString() == "1")
                    {
                        if (drSummary["persistent"].ToString() == "1")
                        {
                            int intPersistent = oDesign.GetStorageTotal(this.DesignId);
                            if (intPersistent > 0)
                                lblSummaryStorage.Text = "Persistent, " + intPersistent.ToString() + " GB(s)";
                        }
                        else if (drSummary["persistent"].ToString() == "0")
                        {
                            int intNonPersistent = 0;
                            Int32.TryParse(drSummary["non_persistent"].ToString(), out intNonPersistent);
                            if (intNonPersistent > 0)
                            {
                                lblSummaryStorage.Text = "Non-Persistent, " + intNonPersistent.ToString() + " GB(s)";
                            }
                        }
                        else
                        {
                            int intStorage = oDesign.GetStorageTotal(this.DesignId);
                            if (intStorage > 0)
                                lblSummaryStorage.Text = intStorage.ToString() + " GB(s)";
                        }
                    }
                    else if (drSummary["storage"].ToString() == "0")
                    {
                        lblSummaryStorage.Text = "No";
                    }
                    // HA
                    if (drSummary["ha"].ToString() == "1")
                    {
                        if (oModelsProperties.IsSUNVirtual(intModel) == true)
                            lblSummaryHA.Text = "Sun Virtual Environment (SVE)";
                        else if (drSummary["ha_clustering"].ToString() == "1")
                        {
                            lblSummaryHA.Text = "Clustered";
                            if (drSummary["active_passive"].ToString() == "1")
                            {
                                lblSummaryHA.Text += " (Active / Passive)";
                            }
                            else if (drSummary["active_passive"].ToString() == "2")
                            {
                                lblSummaryHA.Text += " (Active / Active)";
                            }
                        }
                        else if (drSummary["ha_load_balancing"].ToString() == "1")
                        {
                            lblSummaryHA.Text = "Load Balancing";
                        }
                    }
                    else if (drSummary["ha"].ToString() == "0")
                    {
                        lblSummaryHA.Text = "No";
                    }
                    else
                    {
                        lblSummaryHA.Text = "<i>N / A</i>";
                    }
                    // SPECIAL
                    if (oModelsProperties.IsVMwareVirtual(intModel))
                        lblSummaryBootType.Text = "Virtual Hard Disk (VHD)";
                    else if (oModelsProperties.IsStorageDB_BootLocal(intModel))
                        lblSummaryBootType.Text = "Local Disk";
                    else
                        lblSummaryBootType.Text = "SAN Disk";
                    // DATE
                    DateTime datDate = DateTime.Now;
                    if (DateTime.TryParse(drSummary["commitment"].ToString(), out datDate) == true)
                    {
                        lblSummaryDate.Text = datDate.ToShortDateString();
                        // Target completion date = 2 weeks later than commitment
                        DateTime datTarget = oHoliday.GetDays(dblSLA, datDate);
                        string strTarget = datTarget.ToShortDateString();
                        if (strTarget != "")
                            lblSummaryTarget.Text = strTarget + "&nbsp;&nbsp;&nbsp;&nbsp;(<a href=\"javascript:void(0);\" onclick=\"alert('" + strTarget + " is " + dblSLA.ToString() + " business days from your build date (" + lblSummaryDate.Text + ").\\n\\nBusiness days exclude weekends and holidays.');\">How is this calculated?</a>)";
                    }
                    // CONFIDENCE
                    lblSummaryConfidence.Text = drSummary["confidence"].ToString();
                    //if (oDesign.IsOther(this.DesignId, 1, 0, 0) == true)
                    //    btnUnlock.Visible = true;
                    btnUnlock.Attributes.Add("onclick", "return OpenWindow('DESIGN_UNLOCK','" + this.DesignId.ToString() + "')");

                    /*
                    if (drSummary["answerid"].ToString() == "")
                        lblSummaryAnswerID.Text = "N / A";
                    else
                        lblSummaryAnswerID.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/datapoint/service/design.aspx?t=design&q=" + oFunction.encryptQueryString(drSummary["answerid"].ToString()) + "',800,600);\">" + drSummary["answerid"].ToString() + "</a>";
                    */

                    if (this.ExceptionServiceFolder > 0)
                        this.ExceptionRadio.Visible = false;

                    if (this.ExceptionServiceFolder > 0 && oDesign.IsOther(this.DesignId, 0, 0, 1))
                    {
                        this.ExceptionServiceFolderPanel.Visible = true;
                        this.CompleteRadio.Enabled = false;
                    }
                    else
                    {
                        string strValidation = oDesign.GetValid(this.DesignId);
                        if (strValidation != "")
                        {
                            // Invalid Design
                            if (this.InvalidPanel != null)
                                this.InvalidPanel.Style["display"] = "inline";
                            // Hide the Rejection screen (if shown)
                            if (this.RejectPanel != null)
                                this.RejectPanel.Style["display"] = "none";

                            if (this.Demo == false && this.CompleteRadio != null)
                            {
                                this.CompleteRadio.Text += " <i>(Incomplete)</i>";
                                this.CompleteRadio.Enabled = false;
                            }
                            if (this.ValidationLabel != null)
                            {
                                string[] strValidations = strValidation.Split(new char[] { ',' });
                                foreach (string strError in strValidations)
                                {
                                    if (strError.Trim() != "")
                                    {
                                        if (this.ValidationLabel.Text != "")
                                            this.ValidationLabel.Text += "<br/>";
                                        this.ValidationLabel.Text += " - " + strError.Trim();
                                    }
                                }
                            }
                        }
                        else
                        {
                            btnUnlock.Visible = false;

                            // Valid Design
                            if (this.RejectPanel == null || this.RejectPanel.Style["display"] != "inline")
                            {
                                if (oDesign.IsOther(this.DesignId, 0, 0, 1) == true)
                                {
                                    if (this.ExceptionPanel != null)
                                        this.ExceptionPanel.Style["display"] = "inline";
                                }
                                else if (this.ValidPanel != null)
                                {
                                    this.ValidPanel.Style["display"] = "inline";
                                }
                            }
                            // Check confidence and Date
                            string strCanExecute = oDesign.CanExecute(this.DesignId);
                            if (this.Demo == true)
                            {
                                if (strCanExecute != "")
                                    this.InvalidPanel.Style["display"] = "inline";
                                if (this.ValidationLabel != null)
                                    this.ValidationLabel.Text = " - " + strCanExecute;
                            }
                            else if (this.CompleteRadio != null)
                            {
                                if (strCanExecute != "")
                                    this.CompleteRadio.Text += " <i>(" + strCanExecute + ")</i>";

                                if (this.CompleteRadio.Text.Contains("<i>"))
                                {
                                    this.CompleteRadio.Enabled = false;
                                    this.CompleteRadio.Checked = false;
                                }
                            }
                        }

                        if (this.Demo == true && InvalidPanel.Style["display"] == "inline")
                        {
                            ValidPanel.Style["display"] = "none";
                            CompleteRadio.Enabled = false;
                            ScheduleRadio.Enabled = false;
                        }
                    }

                }
            }
        }
    }
}