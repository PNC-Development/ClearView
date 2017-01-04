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
    public partial class servers : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected OnDemand oOnDemand;
        protected Forecast oForecast;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Cluster oCluster;
        protected CSMConfig oCSMConfig;
        protected Servers oServer;
        protected Storage oStorage;
        protected int intID = 0;
        protected int intStep = 0;
        protected int intType = 0;
        protected int intRequest = 0;
        protected string strDevices = "";
        protected int intTotalCount = 0;
        protected int intTotalDR = 0;
        protected bool boolOther = false;
        protected bool boolConfigured = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "ClearView Device Configuration";
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oOnDemand = new OnDemand(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oCluster = new Cluster(intProfile, dsn);
            oCSMConfig = new CSMConfig(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oStorage = new Storage(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
                intStep = Int32.Parse(Request.QueryString["sid"]);
            if (oForecast.GetAnswer(intID, "completed") == "" && Request.QueryString["view"] == null)
            {
                if (Request.QueryString["step"] != null && Request.QueryString["step"] != "")
                    panUpdate.Visible = true;
                else
                    panNavigation.Visible = true;
            }
            else
                btnClose.Text = "Close";
            bool boolStorage = true;
            if (intID > 0)
            {
                Page.Title = "ClearView Device Configuration | Design # " + intID.ToString();
                oServer.UpdateModels(intID);
                DataSet ds = oForecast.GetAnswer(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intRequest = oForecast.GetRequestID(intID, true);
                    intTotalCount = Int32.Parse(ds.Tables[0].Rows[0]["quantity"].ToString());
                    intTotalDR = Int32.Parse(ds.Tables[0].Rows[0]["recovery_number"].ToString());
                    int intModel = oForecast.GetModel(intID);
                    intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                    intType = oModel.GetType(intModel);
                    DataSet dsSteps = oOnDemand.GetWizardSteps(intType, 1);
                    int intCount = Int32.Parse(oOnDemand.GetWizardStep(intStep, "step"));
                    if (dsSteps.Tables[0].Rows.Count == intCount)
                        btnNext.Text = "Finish";
                    if (intCount == 0 || intCount == 1)
                        btnBack.Enabled = false;
                    if (oForecast.IsStorage(intID) == true)
                    {
                        DataSet dsStorage = oForecast.GetStorage(intID);
                        double dblHighA = double.Parse(dsStorage.Tables[0].Rows[0]["high_total"].ToString());
                        double dblStandardA = double.Parse(dsStorage.Tables[0].Rows[0]["standard_total"].ToString());
                        double dblLowA = double.Parse(dsStorage.Tables[0].Rows[0]["low_total"].ToString());
                        double dblHighQAA = double.Parse(dsStorage.Tables[0].Rows[0]["high_qa"].ToString());
                        double dblStandardQAA = double.Parse(dsStorage.Tables[0].Rows[0]["standard_qa"].ToString());
                        double dblLowQAA = double.Parse(dsStorage.Tables[0].Rows[0]["low_qa"].ToString());
                        double dblHighTestA = double.Parse(dsStorage.Tables[0].Rows[0]["high_test"].ToString());
                        double dblStandardTestA = double.Parse(dsStorage.Tables[0].Rows[0]["standard_test"].ToString());
                        double dblLowTestA = double.Parse(dsStorage.Tables[0].Rows[0]["low_test"].ToString());
                        dsStorage = oStorage.GetLuns(intID);
                        double dblHighU = 0.00;
                        double dblStandardU = 0.00;
                        double dblLowU = 0.00;
                        double dblHighQAU = 0.00;
                        double dblStandardQAU = 0.00;
                        double dblLowQAU = 0.00;
                        double dblHighTestU = 0.00;
                        double dblStandardTestU = 0.00;
                        double dblLowTestU = 0.00;
                        foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
                        {
                            if (drStorage["size"].ToString() != "")
                            {
                                if (drStorage["performance"].ToString() == "High")
                                    dblHighU += double.Parse(drStorage["size"].ToString());
                                if (drStorage["performance"].ToString() == "Standard")
                                    dblStandardU += double.Parse(drStorage["size"].ToString());
                                if (drStorage["performance"].ToString() == "Low")
                                    dblLowU += double.Parse(drStorage["size"].ToString());
                            }
                            if (drStorage["size_qa"].ToString() != "")
                            {
                                if (drStorage["performance"].ToString() == "High")
                                    dblHighQAU += double.Parse(drStorage["size_qa"].ToString());
                                if (drStorage["performance"].ToString() == "Standard")
                                    dblStandardQAU += double.Parse(drStorage["size_qa"].ToString());
                                if (drStorage["performance"].ToString() == "Low")
                                    dblLowQAU += double.Parse(drStorage["size_qa"].ToString());
                            }
                            if (drStorage["size_test"].ToString() != "")
                            {
                                if (drStorage["performance"].ToString() == "High")
                                    dblHighTestU += double.Parse(drStorage["size_test"].ToString());
                                if (drStorage["performance"].ToString() == "Standard")
                                    dblStandardTestU += double.Parse(drStorage["size_test"].ToString());
                                if (drStorage["performance"].ToString() == "Low")
                                    dblLowTestU += double.Parse(drStorage["size_test"].ToString());
                            }
                            DataSet dsMount = oStorage.GetMountPoints(Int32.Parse(drStorage["id"].ToString()));
                            foreach (DataRow drMount in dsMount.Tables[0].Rows)
                            {
                                if (drMount["size"].ToString() != "")
                                {
                                    if (drMount["performance"].ToString() == "High")
                                        dblHighU += double.Parse(drMount["size"].ToString());
                                    if (drMount["performance"].ToString() == "Standard")
                                        dblStandardU += double.Parse(drMount["size"].ToString());
                                    if (drMount["performance"].ToString() == "Low")
                                        dblLowU += double.Parse(drMount["size"].ToString());
                                }
                                if (drMount["size_qa"].ToString() != "")
                                {
                                    if (drMount["performance"].ToString() == "High")
                                        dblHighQAU += double.Parse(drMount["size_qa"].ToString());
                                    if (drMount["performance"].ToString() == "Standard")
                                        dblStandardQAU += double.Parse(drMount["size_qa"].ToString());
                                    if (drMount["performance"].ToString() == "Low")
                                        dblLowQAU += double.Parse(drMount["size_qa"].ToString());
                                }
                                if (drMount["size_test"].ToString() != "")
                                {
                                    if (drMount["performance"].ToString() == "High")
                                        dblHighTestU += double.Parse(drMount["size_test"].ToString());
                                    if (drMount["performance"].ToString() == "Standard")
                                        dblStandardTestU += double.Parse(drMount["size_test"].ToString());
                                    if (drMount["performance"].ToString() == "Low")
                                        dblLowTestU += double.Parse(drMount["size_test"].ToString());
                                }
                            }
                        }
                        if (dblHighA < dblHighU || dblStandardA < dblStandardU || dblLowA < dblLowU || dblHighQAA < dblHighQAU || dblStandardQAA < dblStandardQAU || dblLowQAA < dblLowQAU || dblHighTestA < dblHighTestU || dblStandardTestA < dblStandardTestU || dblLowTestA < dblLowTestU)
                            boolStorage = false;
                    }
                    if (!IsPostBack)
                        LoadDevices();
                    if (boolConfigured == true && boolStorage == true)
                        panValid.Visible = true;
                }
            }
            btnClose.Attributes.Add("onclick", "return window.close();");
            if (boolConfigured == false)
            {
                btnNext.Attributes.Add("onclick", "alert('You cannot continue until you have configured all devices.\\n\\nClick OK to close this window. Then click [Edit] to configure each device.');return false;");
                btnUpdate.Attributes.Add("onclick", "alert('You cannot continue until you have configured all devices.\\n\\nClick OK to close this window. Then click [Edit] to configure each device.');return false;");
            }
            //if (boolStorage == false)
            //{
            //    btnNext.Attributes.Add("onclick", "alert('You have allocated more storage than you requested. \\n\\nClick OK to close this window. Then click [Edit] to modify the storage allocation.\\n\\nClick \"Storage Details\" for additional information.');return false;");
            //    btnUpdate.Attributes.Add("onclick", "alert('You have allocated more storage than you requested. \\n\\nClick OK to close this window. Then click [Edit] to modify the storage allocation.\\n\\nClick \"Storage Details\" for additional information.');return false;");
            //}
        }
        private void LoadDevices()
        {
            DataSet dsClusters = oCluster.Gets(intRequest);
            foreach (DataRow drCluster in dsClusters.Tables[0].Rows)
            {
                AddDevice("Cluster Node", drCluster["name"].ToString(), drCluster["nodes"].ToString(), Int32.Parse(drCluster["id"].ToString()), 0, 0);
                try
                {
                    int intCount = Int32.Parse(drCluster["nodes"].ToString());
                    if (intCount > 0)
                        AddDevice("Cluster Node", drCluster["name"].ToString(), drCluster["nodes"].ToString(), Int32.Parse(drCluster["id"].ToString()), 0, 0);
                }
                catch { }
            }
            DataSet dsConfigs = oCSMConfig.Gets(intRequest);
            foreach (DataRow drConfig in dsConfigs.Tables[0].Rows)
            {
                AddDevice("CSM Config", drConfig["name"].ToString(), drConfig["servers"].ToString(), 0, Int32.Parse(drConfig["id"].ToString()), 0);
                try
                {
                    int intCount = Int32.Parse(drConfig["servers"].ToString());
                    if (intCount > 0)
                        AddDevice("CSM Config", drConfig["name"].ToString(), drConfig["servers"].ToString(), 0, Int32.Parse(drConfig["id"].ToString()), 0);
                }
                catch { }
            }
            if (strDevices == "")
            {
                for (int ii = 1; ii <= intTotalCount; ii++)
                {
                    AddDevice("Server", "---", ii.ToString(), 0, 0, ii);
                }
            }
        }
        private void AddDevice(string _type, string _name, string _count, int _clusterid, int _csmconfigid, int _number)
        {
            boolOther = !boolOther;
            strDevices += "<tr" + (boolOther ? " bgcolor=\"F6F6F6\"" : "") + ">";
            strDevices += "<td>" + _type + "</td>";
            strDevices += "<td>" + _name + "</td>";
            strDevices += "<td>" + _count + "</td>";
            DataSet ds = oServer.Get(intID, _csmconfigid, _clusterid, _number);
            bool boolDeviceConfigured = true;
            if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0]["configured"].ToString() != "1")
                boolDeviceConfigured = false;
            if (boolDeviceConfigured == true)
                strDevices += "<td nowrap align=\"center\"><img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\"/> <input type=\"button\" class=\"default\" value=\"Edit\" style=\"width:50px\" onclick=\"OpenOnDemandDevice(" + intID + "," + _clusterid.ToString() + "," + _csmconfigid.ToString() + "," + _number.ToString() + ");\"/></td>";
            else
            {
                strDevices += "<td nowrap align=\"center\"><img src=\"/images/required.gif\" border=\"0\" align=\"absmiddle\"/> <input type=\"button\" class=\"default\" value=\"Edit\" style=\"width:50px\" onclick=\"OpenOnDemandDevice(" + intID + "," + _clusterid.ToString() + "," + _csmconfigid.ToString() + "," + _number.ToString() + ");\"/></td>";
                boolConfigured = false;
            }
            strDevices += "<td>&nbsp;</td>";
            if (ds.Tables[0].Rows.Count == 0)
            {
                strDevices += "<td nowrap align=\"center\"><input type=\"button\" class=\"default\" value=\"Edit\" style=\"width:50px\" disabled /></td>";
                boolConfigured = false;
            }
            else
            {
                Domains oDomain = new Domains(0, dsn);
                int intDomain = Int32.Parse(ds.Tables[0].Rows[0]["test_domainid"].ToString());
                if (intDomain == 0)
                    intDomain = Int32.Parse(ds.Tables[0].Rows[0]["domainid"].ToString());
                if (oDomain.Get(intDomain, "account_setup") == "1" && oForecast.IsOSDistributed(intID) == true)
                {
                    if (ds.Tables[0].Rows[0]["accounts"].ToString() == "0")
                    {
                        strDevices += "<td nowrap align=\"center\"><img src=\"/images/required.gif\" border=\"0\" align=\"absmiddle\"/> <input type=\"button\" class=\"default\" value=\"Edit\" style=\"width:50px\" onclick=\"OpenWindow('NEW_WINDOW','/frame/ondemand/accounts_new.aspx?id=" + ds.Tables[0].Rows[0]["id"].ToString() + "');\"/></td>";
                        boolConfigured = false;
                    }
                    else
                        strDevices += "<td nowrap align=\"center\"><img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\"/> <input type=\"button\" class=\"default\" value=\"Edit\" style=\"width:50px\" onclick=\"OpenWindow('NEW_WINDOW','/frame/ondemand/accounts_new.aspx?id=" + ds.Tables[0].Rows[0]["id"].ToString() + "');\"/></td>";
                }
                else
                {
                    if (ds.Tables[0].Rows[0]["accounts"].ToString() == "0")
                        oServer.UpdateAccounts(Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString()), 1);
                    strDevices += "<td nowrap align=\"center\"><input type=\"button\" class=\"default\" value=\"N / A\" style=\"width:50px\" disabled /></td>";
                }
            }
            strDevices += "<td>&nbsp;</td>";
            if (oForecast.IsStorage(intID) == true)
            {
                bool boolDeviceStorage = true;
                if (ds.Tables[0].Rows.Count == 0 || Int32.Parse(ds.Tables[0].Rows[0]["local_storage"].ToString()) == 0 || (Int32.Parse(ds.Tables[0].Rows[0]["local_storage"].ToString()) == 1 && Int32.Parse(ds.Tables[0].Rows[0]["fdrive"].ToString()) == 0))
                    boolDeviceStorage = false;
                if (boolDeviceStorage == true)
                    strDevices += "<td nowrap align=\"center\"><img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\"/> <input type=\"button\" class=\"default\" value=\"Edit\" style=\"width:50px\" " + (boolDeviceConfigured == false ? "disabled " : "") + "onclick=\"OpenOnDemandStorage(" + intID + "," + _clusterid.ToString() + "," + _csmconfigid.ToString() + "," + _number.ToString() + ");\"/></td>";
                else
                {
                    strDevices += "<td nowrap align=\"center\"><img src=\"/images/required.gif\" border=\"0\" align=\"absmiddle\"/> <input type=\"button\" class=\"default\" value=\"Edit\" style=\"width:50px\" " + (boolDeviceConfigured == false ? "disabled " : "") + "onclick=\"OpenOnDemandStorage(" + intID + "," + _clusterid.ToString() + "," + _csmconfigid.ToString() + "," + _number.ToString() + ");\"/></td>";
                    boolConfigured = false;
                }
            }
            else
                strDevices += "<td nowrap align=\"center\"><input type=\"button\" class=\"default\" value=\"N / A\" style=\"width:50px\" disabled /></td>";
            strDevices += "</tr>";
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            oOnDemand.Back(intID);
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&backward=true");
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            oOnDemand.Next(intID, Int32.Parse(Request.QueryString["sid"]));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&forward=true");
        }
        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            oOnDemand.Next(intID, Int32.Parse(Request.QueryString["sid"]));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&forward=true");
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
    }
}