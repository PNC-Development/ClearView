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
    public partial class design_print : System.Web.UI.Page
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Design oDesign;
        protected Users oUser;
        protected ModelsProperties oModelsProperties;
        protected Functions oFunction;
        protected int intID;
        protected string strHighlight = "#FFEE99";
        protected StringBuilder strBackup;
        protected StringBuilder strMaintenance;
        protected bool boolWindows = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oDesign = new Design(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);

            if (Request.QueryString["id"] != null)
                Int32.TryParse(Request.QueryString["id"], out intID);

            if (intID > 0)
            {
                lblID.Text = intID.ToString();
                DataSet dsSummary = oDesign.Get(intID);
                if (dsSummary.Tables[0].Rows.Count > 0)
                {
                    DataRow drSummary = dsSummary.Tables[0].Rows[0];
                    Mnemonic oMnemonic = new Mnemonic(intProfile, dsn);
                    CostCenter oCostCenter = new CostCenter(intProfile, dsn);
                    OperatingSystems oOperatingSystem = new OperatingSystems(intProfile, dsn);
                    Classes oClass = new Classes(intProfile, dsn);
                    Environments oEnvironment = new Environments(intProfile, dsn);
                    Locations oLocation = new Locations(intProfile, dsn);

                    bool boolWeb = (drSummary["web"].ToString() == "1");
                    bool boolSQL = oDesign.IsSQL(intID);
                    bool boolOracle = oDesign.IsOracle(intID);
                    bool boolOtherDB = (drSummary["other_db"].ToString() == "1");
                    boolWindows = oDesign.IsWindows(intID);

                    DataSet dsSubmitted = oDesign.GetSubmitted(intID);
                    if (dsSubmitted.Tables[0].Rows.Count > 0)
                    {
                        int intUser = Int32.Parse(dsSubmitted.Tables[0].Rows[0]["userid"].ToString());
                        lblRequestedBy.Text = oUser.GetFullName(intUser) + " (" + oUser.GetName(intUser) + ")";
                        lblRequestedOn.Text = dsSubmitted.Tables[0].Rows[0]["created"].ToString();
                        if (dsSubmitted.Tables[0].Rows[0]["comments"].ToString() != "")
                        {
                            trException1.Visible = true;
                            trException2.Visible = true;
                            lblException.Text = dsSubmitted.Tables[0].Rows[0]["comments"].ToString();
                        }
                    }
                    else
                    {
                        lblRequestedBy.Text = oUser.GetFullName(intProfile) + " (" + oUser.GetName(intProfile) + ")";
                        lblRequestedOn.Text = dsSummary.Tables[0].Rows[0]["modified"].ToString();
                    }

                    // Mnemonic
                    int intMnemonic = 0;
                    Int32.TryParse(drSummary["mnemonicid"].ToString(), out intMnemonic);
                    if (intMnemonic > 0)
                    {
                        lblMnemonic.Text = oMnemonic.Get(intMnemonic, "factory_code") + " - " + oMnemonic.Get(intMnemonic, "name");
                        string strMnemonicCode = oMnemonic.Get(intMnemonic, "factory_code");
                        string strMnemonicStatus = oMnemonic.GetFeed(strMnemonicCode, MnemonicFeed.Status);
                        if (strMnemonicStatus == "")
                            strMnemonicStatus = oMnemonic.Get(intMnemonic, "status");
                        lblMnemonicStatus.Text = strMnemonicStatus;
                        string strMnemonicRTO = oMnemonic.GetFeed(strMnemonicCode, MnemonicFeed.ResRating);
                        if (strMnemonicRTO == "")
                            strMnemonicRTO = oMnemonic.Get(intMnemonic, "ResRating");
                        lblMnemonicRTO.Text = strMnemonicRTO;
                    }
                    // Server Type
                    if (oDesign.IsDatabase(intID) == false)
                    {
                        if (boolWeb)
                            lblServerType.Text = "Web";
                        else
                            lblServerType.Text = "Application";
                    }
                    else
                    {
                        lblServerType.Text = "Database";
                        if (boolSQL || boolOracle || boolOtherDB)
                        {
                            string strDatabase = "";
                            if (boolSQL == true)
                                strDatabase = "SQL";
                            if (boolOracle == true)
                                strDatabase = "Oracle";
                            if (boolOtherDB == true)
                                strDatabase = "Other";
                            lblServerType.Text += " (" + strDatabase + ")";
                        }
                        if (boolWeb)
                            lblServerType.Text += " + Web";
                    }
                    // OS
                    int intOS = 0;
                    Int32.TryParse(drSummary["osid"].ToString(), out intOS);
                    if (intOS > 0)
                        lblOS.Text = oOperatingSystem.Get(intOS, "name");
                    // SIZE
                    string strSize = drSummary["cores"].ToString() + " CPU(s), " + drSummary["ram"].ToString() + " GB(s) RAM";
                    lblSize.Text = strSize;
                    // MAINFRAME
                    if (drSummary["mainframe"].ToString() == "1")
                        lblMainframe.Text = "Yes";
                    else if (drSummary["mainframe"].ToString() == "0")
                        lblMainframe.Text = "No";
                    // SPECIAL
                    lblSpecial.Text = drSummary["special"].ToString();
                    if (lblSpecial.Text == "")
                        lblSpecial.Text = "None";
                    // HA
                    if (drSummary["ha"].ToString() == "1")
                    {
                        if (drSummary["ha_clustering"].ToString() == "1")
                        {
                            lblHA.Text = "Clustering";
                            if (drSummary["active_passive"].ToString() == "1")
                                lblHA.Text += " (Active / Passive)";
                            else if (drSummary["active_passive"].ToString() == "2")
                                lblHA.Text += " (Active / Active)";
                            lblHA.Text += "<br/>" + drSummary["instances"].ToString() + " Instances";
                            //lblHA.Text += ", " + drSummary["nodes"].ToString() + " Nodes";
                        }
                        else if (drSummary["ha_load_balancing"].ToString() == "1")
                        {
                            lblHA.Text = "Load Balancing";
                            if (drSummary["middleware"].ToString() == "1")
                                lblHA.Text += " (Middleware)";
                            else if (boolWeb == true)
                            {
                                if (drSummary["application"].ToString() == "1")
                                    lblHA.Text += " (Web + App)";
                                else
                                    lblHA.Text += " (Web)";
                            }
                            else
                                lblHA.Text += " (App)";
                        }
                    }
                    else if (drSummary["ha"].ToString() == "0")
                        lblHA.Text = "No";
                    // SOFTWARE
                    if (drSummary["ndm"].ToString() == "1")
                        lblSoftware.Text += "ConnectDirect";
                    if (drSummary["ca7"].ToString() == "1")
                        lblSoftware.Text += (lblSoftware.Text != "" ? "<br/>" : "") + "CA7";
                    if (lblSoftware.Text == "")
                        lblSoftware.Text = "None";

                    // Middlware
                    DataSet dsSoftware = oDesign.GetSoftwareComponents(intID);
                    foreach (DataRow drSoftware in dsSoftware.Tables[0].Rows)
                    {
                        // Loop through select components
                        int intComponentID = Int32.Parse(drSoftware["componentid"].ToString());
                        int intResponse = Int32.Parse(drSoftware["responseid"].ToString());
                        lblMiddleware.Text += (lblMiddleware.Text != "" ? "<br/>" : "") + oDesign.GetResponse(intResponse, "response");
                    }
                    if (lblMiddleware.Text == "")
                        lblMiddleware.Text = "None";

                    // Legacy Design ID
                    if (drSummary["answerid"].ToString() == "")
                        lblAnswerID.Text = "N / A";
                    else
                        lblAnswerID.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/datapoint/service/design.aspx?t=design&q=" + oFunction.encryptQueryString(drSummary["answerid"].ToString()) + "',800,600);\">" + drSummary["answerid"].ToString() + "</a>";
                    

                    // Quantity
                    int intQuantity = 0;
                    Int32.TryParse(drSummary["quantity"].ToString(), out intQuantity);
                    if (intQuantity > 0)
                        lblQuantity.Text = intQuantity.ToString();
                    if (oDesign.IsProd(intID) == true && oDesign.IsUnder48(intID, true) == true)
                        lblQuantity.Text += " ( + " + intQuantity.ToString() + " for DR)";
                    // DATE
                    DateTime datDate = DateTime.Now;
                    if (DateTime.TryParse(drSummary["commitment"].ToString(), out datDate) == true)
                        lblDate.Text = datDate.ToShortDateString();
                    // CONFIDENCE
                    lblConfidence.Text = drSummary["confidence"].ToString();

                    // LOCATION
                    int intClass = 0;
                    int intEnv = 0;
                    int intAddress = 0;
                    Int32.TryParse(drSummary["classid"].ToString(), out intClass);
                    if (intClass > 0)
                    {
                        lblLocation.Text = oClass.Get(intClass, "name") + ", ";
                        Int32.TryParse(drSummary["environmentid"].ToString(), out intEnv);
                        if (intEnv > 0)
                        {
                            lblLocation.Text += oEnvironment.Get(intEnv, "name");
                            Int32.TryParse(drSummary["addressid"].ToString(), out intAddress);
                            if (intAddress > 0)
                                lblLocation.Text += " at " + oLocation.GetFull(intAddress);
                            else
                                lblLocation.Text += " at <a href=\"javascript:void(0);\" onclick=\"alert('The datacenter will be either the Cleveland Data Center or the Summit Data Center.\\n\\nThis will be decided during execution. It is based on the available inventory at that time.');\">Cleveland or Summit Data Center</a>";
                        }
                    }
                    // SOLUTION
                    int intModel = oDesign.GetModelProperty(intID);
                    lblSolution.Text = oModelsProperties.Get(intModel, "name");

                    // Server Boot Type
                    if (oModelsProperties.IsVMwareVirtual(intModel))
                        lblServerBootType.Text = "Virtual Hard Disk (VHD)";
                    else if (oModelsProperties.IsStorageDB_BootLocal(intModel))
                        lblServerBootType.Text = "Local Disk";
                    else
                        lblServerBootType.Text = "SAN Disk";

                    // ACCOUNTS
                    rptAccounts.DataSource = oDesign.GetAccounts(intID);
                    rptAccounts.DataBind();
                    foreach (RepeaterItem ri in rptAccounts.Items)
                    {
                        Label _permissions = (Label)ri.FindControl("lblPermissions");
                        switch (_permissions.Text)
                        {
                            case "0":
                                _permissions.Text = "-----";
                                break;
                            case "D":
                                _permissions.Text = "Developer";
                                break;
                            case "P":
                                _permissions.Text = "Promoter";
                                break;
                            case "S":
                                _permissions.Text = "AppSupport";
                                break;
                            case "U":
                                _permissions.Text = "AppUsers";
                                break;
                        }
                        if (_permissions.ToolTip == "1")
                            _permissions.Text += " (R/D)";
                    }
                    lblNone.Visible = (rptAccounts.Items.Count == 0);

                    // STORAGE
                    if (drSummary["storage"].ToString() == "1")
                    {
                        if (drSummary["persistent"].ToString() == "1")
                        {
                            panStorage.Visible = true;
                            int intPersistent = oDesign.GetStorageTotal(intID);
                            if (intPersistent > 0)
                                lblStorage.Text = "Persistent, " + intPersistent.ToString() + " GB(s)";
                        }
                        else if (drSummary["persistent"].ToString() == "0")
                        {
                            int intNonPersistent = 0;
                            Int32.TryParse(drSummary["non_persistent"].ToString(), out intNonPersistent);
                            if (intNonPersistent > 0)
                                lblStorage.Text = "Non-Persistent, " + intNonPersistent.ToString() + " GB(s)";
                        }
                        else
                        {
                            panStorage.Visible = true;
                            int intStorage = oDesign.GetStorageTotal(intID);
                            if (intStorage > 0)
                                lblStorage.Text = intStorage.ToString() + " GB(s)";
                        }
                    }
                    else if (drSummary["storage"].ToString() == "0")
                        lblStorage.Text = "No";
                    // STORAGE LUNs
                    rptStorage.DataSource = oDesign.GetStorageDrives(intID);
                    rptStorage.DataBind();
                    foreach (RepeaterItem ri in rptStorage.Items)
                    {
                        CheckBox _shared = (CheckBox)ri.FindControl("chkStorageSize");
                        _shared.Checked = (_shared.Text == "1");
                        _shared.Text = "";
                    }
                    if (boolWindows)
                    {
                        trStorageApp.Visible = true;
                        DataSet dsApp = oDesign.GetStorageDrive(intID, -1000);
                        if (dsApp.Tables[0].Rows.Count > 0)
                        {
                            int intTemp = 0;
                            if (Int32.TryParse(dsApp.Tables[0].Rows[0]["size"].ToString(), out intTemp) == true)
                                txtStorageSizeE.Text = intTemp.ToString();
                        }
                    }
                    if (oDesign.IsProd(intID) == true || oDesign.IsQA(intID) == true)
                    {
                        // BACKUP
                        panBackup.Visible = true;
                        lblFrequency.Text = (drSummary["backup_frequency"].ToString() == "D" ? "Daily" : (drSummary["backup_frequency"].ToString() == "W" ? "Weekly" : (drSummary["backup_frequency"].ToString() == "M" ? "Monthly" : "N / A")));
                        strBackup = new StringBuilder();
                        DataSet dsBackup = oDesign.GetBackup(intID);
                        if (dsBackup.Tables[0].Rows.Count > 0)
                        {
                            DataRow drBackup = dsBackup.Tables[0].Rows[0];
                            for (int ii = 0; ii < 7; ii++)
                            {
                                strBackup.Append("<tr>");
                                strBackup.Append("<td>");
                                string strCheck = "";
                                if (ii == 0)
                                {
                                    strBackup.Append("Sunday");
                                    strCheck = drBackup["sun"].ToString();
                                }
                                else if (ii == 1)
                                {
                                    strBackup.Append("Monday");
                                    strCheck = drBackup["mon"].ToString();
                                }
                                else if (ii == 2)
                                {
                                    strBackup.Append("Tuesday");
                                    strCheck = drBackup["tue"].ToString();
                                }
                                else if (ii == 3)
                                {
                                    strBackup.Append("Wednesday");
                                    strCheck = drBackup["wed"].ToString();
                                }
                                else if (ii == 4)
                                {
                                    strBackup.Append("Thursday");
                                    strCheck = drBackup["thu"].ToString();
                                }
                                else if (ii == 5)
                                {
                                    strBackup.Append("Friday");
                                    strCheck = drBackup["fri"].ToString();
                                }
                                else
                                {
                                    strBackup.Append("Saturday");
                                    strCheck = drBackup["sat"].ToString();
                                }
                                strBackup.Append("</td>");
                                for (int jj = 0; jj < 24; jj++)
                                {
                                    strBackup.Append("<td>");
                                    if (strCheck[jj] == '1')
                                        strBackup.Append("<b>B</b>");
                                    else
                                        strBackup.Append("-");
                                    strBackup.Append("</td>");
                                }
                                strBackup.Append("</tr>");
                            }
                        }
                        // BACKUP EXCLUSIONS
                        panExclusions.Visible = true;
                        rptExclusions.DataSource = oDesign.GetExclusions(intID);
                        rptExclusions.DataBind();
                        lblExclusion.Visible = (rptExclusions.Items.Count == 0);

                        // MAINTENANCE WINDOW
                        panMaintenance.Visible = true;
                        strMaintenance = new StringBuilder();
                        DataSet dsMaintenance = oDesign.GetMaintenance(intID);
                        if (dsMaintenance.Tables[0].Rows.Count > 0)
                        {
                            DataRow drMaintenance = dsMaintenance.Tables[0].Rows[0];
                            for (int ii = 0; ii < 7; ii++)
                            {
                                strMaintenance.Append("<tr>");
                                strMaintenance.Append("<td>");
                                string strCheck = "";
                                if (ii == 0)
                                {
                                    strMaintenance.Append("Sunday");
                                    strCheck = drMaintenance["sun"].ToString();
                                }
                                else if (ii == 1)
                                {
                                    strMaintenance.Append("Monday");
                                    strCheck = drMaintenance["mon"].ToString();
                                }
                                else if (ii == 2)
                                {
                                    strMaintenance.Append("Tuesday");
                                    strCheck = drMaintenance["tue"].ToString();
                                }
                                else if (ii == 3)
                                {
                                    strMaintenance.Append("Wednesday");
                                    strCheck = drMaintenance["wed"].ToString();
                                }
                                else if (ii == 4)
                                {
                                    strMaintenance.Append("Thursday");
                                    strCheck = drMaintenance["thu"].ToString();
                                }
                                else if (ii == 5)
                                {
                                    strMaintenance.Append("Friday");
                                    strCheck = drMaintenance["fri"].ToString();
                                }
                                else
                                {
                                    strMaintenance.Append("Saturday");
                                    strCheck = drMaintenance["sat"].ToString();
                                }
                                strMaintenance.Append("</td>");
                                for (int jj = 0; jj < 24; jj++)
                                {
                                    strMaintenance.Append("<td>");
                                    if (strCheck[jj] == '1')
                                        strMaintenance.Append("<b>M</b>");
                                    else
                                        strMaintenance.Append("-");
                                    strMaintenance.Append("</td>");
                                }
                                strMaintenance.Append("</tr>");
                            }
                        }
                    }
                    else
                    {
                        lblHA.Text = "N / A";
                        lblMainframe.Text = "N / A";
                    }
                    // APPROVALS
                    rptWorkflow.DataSource = oDesign.LoadWorkflow(intID);
                    rptWorkflow.DataBind();
                    lblWorkflow.Visible = (rptWorkflow.Items.Count == 0);
                }
            }
        }
    }
}
