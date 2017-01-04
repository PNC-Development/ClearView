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
using NCC.ClearView.Presentation.Web.Custom;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Presentation.Web
{
    public partial class datapoint_cluster : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intDataPointAvailableAsset = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_ASSET"]);
        protected DataPoint oDataPoint;
        protected Cluster oCluster;
        protected Users oUser;
        protected Servers oServer;
        protected Asset oAsset;
        protected Forecast oForecast;
        protected Functions oFunction;
        protected OperatingSystems oOperatingSystem;
        protected Domains oDomain;
        protected ServerName oServerName;
        protected Log oLog;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected string strMenuTab1 = "";
        protected string strAdministration = "";
        private int intID = 0;
        private int intSaveMenuTab = 0;
        protected string strComponents = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            oDataPoint = new DataPoint(intProfile, dsn);
            oCluster = new Cluster(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            oDomain = new Domains(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
            oLog = new Log(intProfile, dsn);
            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "ASSET") == true || intDataPointAvailableAsset == 1))
            {
                panAllow.Visible = true;
                Int32.TryParse(oFunction.decryptQueryString(Request.QueryString["id"]), out intID);
                if (Request.QueryString["close"] != null)
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">window.close();<" + "/" + "script>");
                else if (intID > 0)
                {
                    DataSet ds = oCluster.Get(intID);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        // Load General Information
                        lblClusterID.Text = "#" + intID.ToString();
                        string strName = ds.Tables[0].Rows[0]["name"].ToString();
                        string strNickName = ds.Tables[0].Rows[0]["nickname"].ToString();

                        string strHeader = (strName != "" ? strName : strNickName);
                        lblHeader.Text = "&quot;" + strHeader.ToUpper() + "&quot;";
                        Master.Page.Title = "DataPoint | Cluster (" + strHeader + ")";
                        lblHeaderSub.Text = "Provides all the information about a cluster...";
                        int intMenuTab = 0;
                        if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                            intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                        Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);
                        oTab.AddTab("Cluster Information", "");
                        oTab.AddTab("Software Components", "");
                        oTab.AddTab("Instances", "");
                        oTab.AddTab("Nodes", "");
                        if (oUser.IsAdmin(intProfile) == true || oDataPoint.GetFieldPermission(intProfile, "SERVER_ADMIN") == true)
                        {
                            oTab.AddTab("Administration", "");
                            panAdministration.Visible = true;
                        }
                        strMenuTab1 = oTab.GetTabs();

                        if (!IsPostBack)
                        {
                            //LoadList();

                            // Cluster Information
                            oDataPoint.LoadTextBox(txtName, intProfile, null, "", lblName, fldName, "CLUSTER_NAME", strName, "", false, true);
                            oDataPoint.LoadTextBox(txtNickName, intProfile, null, "", lblNickName, fldNickName, "CLUSTER_NICKNAME", strNickName, "", false, true);
                            oDataPoint.LoadTextBox(txtCount, intProfile, null, "", lblCount, fldCount, "CLUSTER_NODES", ds.Tables[0].Rows[0]["nodes"].ToString(), "", false, true);
                            oDataPoint.LoadTextBox(txtDR, intProfile, null, "", lblDR, fldDR, "CLUSTER_NODES_DR", ds.Tables[0].Rows[0]["dr"].ToString(), "", false, true);
                            oDataPoint.LoadTextBox(txtHA, intProfile, null, "", lblHA, fldHA, "CLUSTER_NODES_HA", ds.Tables[0].Rows[0]["ha"].ToString(), "", false, true);
                            oDataPoint.LoadTextBox(txtServiceAccount, intProfile, null, "", lblServiceAccount, fldServiceAccount, "CLUSTER_SERVICE_ACCOUNT", ds.Tables[0].Rows[0]["service_account"].ToString(), "", false, false);
                            oDataPoint.LoadTextBox(txtVirtualName, intProfile, null, "", lblVirtualName, fldVirtualName, "CLUSTER_VIRTUAL_NAME", ds.Tables[0].Rows[0]["virtual_name"].ToString(), "", false, false);
                            oDataPoint.LoadTextBox(txtPlatformIP, intProfile, null, "", lblPlatformIP, fldPlatformIP, "CLUSTER_IP", ds.Tables[0].Rows[0]["ip"].ToString(), "", false, false);

                            DataSet dsServer = oServer.GetClusters(intID);
                            if (dsServer.Tables[0].Rows.Count > 0)
                            {
                                int intServer = Int32.Parse(dsServer.Tables[0].Rows[0]["id"].ToString());
                                int intAnswer = Int32.Parse(dsServer.Tables[0].Rows[0]["answerid"].ToString());
                                int intClass = 0;
                                int intEnv = 0;
                                DataSet dsAnswer = oForecast.GetAnswer(intAnswer);
                                if (dsAnswer.Tables[0].Rows.Count > 0)
                                {
                                    intClass = Int32.Parse(dsAnswer.Tables[0].Rows[0]["classid"].ToString());
                                    intEnv = Int32.Parse(dsAnswer.Tables[0].Rows[0]["environmentid"].ToString());
                                }
                                int intOS = Int32.Parse(dsServer.Tables[0].Rows[0]["osid"].ToString());
                                int intSP = Int32.Parse(dsServer.Tables[0].Rows[0]["spid"].ToString());
                                int intDomain = Int32.Parse(dsServer.Tables[0].Rows[0]["domainid"].ToString());
                                oDataPoint.LoadDropDown(ddlPlatformOS, intProfile, null, "", lblPlatformOS, fldPlatformOS, "SERVER_OS", "name", "id", oOperatingSystem.Gets(0, 1), intOS, false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformServicePack, intProfile, null, "", lblPlatformServicePack, fldPlatformServicePack, "SERVER_SP", "name", "id", oOperatingSystem.GetServicePack(intOS), intSP, false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformDomain, intProfile, null, "", lblPlatformDomain, fldPlatformDomain, "SERVER_DOMAIN", "name", "id", oDomain.GetClassEnvironment(intClass, intEnv), intDomain, false, false, true);

                                // Components
                                oDataPoint.LoadPanel(panComponents, intProfile, fldComponents, "SERVER_COMPONENTS");
                                if (panComponents.Visible == true)
                                    frmComponents.Attributes.Add("src", "/frame/ondemand/config_server_components.aspx?aid=" + intAnswer.ToString() + "&clusterid=" + intID.ToString());
                                else
                                {
                                    DataSet dsSelected = oServerName.GetComponentDetailSelected(intServer, 1);
                                    foreach (DataRow drSelected in dsSelected.Tables[0].Rows)
                                    {
                                        int intDetail = Int32.Parse(drSelected["detailid"].ToString());
                                        strComponents += oServerName.GetComponentDetailName(intDetail) + "<br/>";
                                    }
                                }
                            }

                            // Instances
                            rptInstances.DataSource = oCluster.GetInstances(intID);
                            rptInstances.DataBind();
                            lblInstances.Visible = (rptInstances.Items.Count == 0);

                            // Nodes
                            rptNodes.DataSource = dsServer;
                            rptNodes.DataBind();
                            lblNodes.Visible = (rptNodes.Items.Count == 0);

                            // Administrative Functions
                            if (Request.QueryString["admin"] != null)
                            {
                                if (Request.QueryString["output"] != null)
                                    strAdministration = "<tr><td>" + oLog.GetEvents(oLog.GetEventsByName(strName, (chkDebug.Checked ? (int)LoggingType.Debug : (int)LoggingType.Error)), intEnvironment) + "</td></tr>";
                            }
                        }
                    }
                    else
                    {
                        panDenied.Visible = true;
                    }
                }
                else
                    panDenied.Visible = true;
                btnClose.Attributes.Add("onclick", "window.close();return false;");
                btnPrint.Attributes.Add("onclick", "window.print();return false;");
                btnOutput.Attributes.Add("onclick", "return ProcessButton(this) && ProcessControlButton();");
            }
            else
                panDenied.Visible = true;
        }
        protected void btnOutput_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + oFunction.encryptQueryString(intID.ToString()) + "&admin=true&menu_tab=5&output=true");
        }
    }
}
