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
    public partial class clustering : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected bool boolUseCSM = (ConfigurationManager.AppSettings["USE_CSM_EXECUTION"] == "1");
        protected int intProfile;
        protected OnDemand oOnDemand;
        protected Forecast oForecast;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Cluster oCluster;
        protected int intID = 0;
        protected int intStep = 0;
        protected int intType = 0;
        protected int intRequest = 0;
        protected bool boolMidrange = false;
        protected string strAttributes = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "ClearView Clustering Information";
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oOnDemand = new OnDemand(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oCluster = new Cluster(intProfile, dsn);
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
            int intForecastCount = 0;
            int intForecastDRCount = 0;
            int intCurrentCount = 0;
            int intCurrentDRCount = 0;
            if (intID > 0)
            {
                Page.Title = "ClearView Clustering Information | Design # " + intID.ToString();
                DataSet ds = oForecast.GetAnswer(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intRequest = oForecast.GetRequestID(intID, true);
                    intForecastCount = Int32.Parse(ds.Tables[0].Rows[0]["quantity"].ToString());
                    intForecastDRCount = Int32.Parse(ds.Tables[0].Rows[0]["recovery_number"].ToString());
                    intCurrentCount = oForecast.TotalServerCount(intID, boolUseCSM);
                    intCurrentDRCount = oForecast.TotalDRCount(intID, boolUseCSM);
                    int intModel = oForecast.GetModel(intID);
                    intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                    intType = oModel.GetType(intModel);
                    if (oForecast.IsOSMidrange(intID) == true)
                        boolMidrange = true;
                    DataSet dsSteps = oOnDemand.GetWizardSteps(intType, 1);
                    int intCount = Int32.Parse(oOnDemand.GetWizardStep(intStep, "step"));
                    if (dsSteps.Tables[0].Rows.Count == intCount)
                        btnNext.Text = "Finish";
                    if (intCount == 0 || intCount == 1)
                        btnBack.Enabled = false;
                    if (!IsPostBack)
                        LoadClusters();
                }
            }
            btnClose.Attributes.Add("onclick", "return window.close();");
            btnAdd.Attributes.Add("onclick", "return OpenCluster('" + intID.ToString() + "','');");
            if (strAttributes == "")
            {
                if (intCurrentCount > 0)
                    panValid.Visible = true;
                btnNext.Attributes.Add("onclick", "return ValidateEqual('" + intForecastCount + "','" + intCurrentCount + "','WARNING: Your Current Server Count does not equal your Forecasted Server Count.\\n\\nForecasted Server Count = " + intForecastCount + "\\nCurrent Server Count = " + intCurrentCount + "\\n\\nIf you choose to proceed, you will lose the remaining devices.\\n\\nAre you sure you want to continue?')" +
                    ";");
                btnUpdate.Attributes.Add("onclick", "return ValidateEqual('" + intForecastDRCount + "','" + intCurrentDRCount + "','WARNING: Your Current DR Count does not equal your Forecasted DR Count.\\n\\nForecasted DR Count = " + intForecastCount + "\\nCurrent DR Count = " + intCurrentCount + "\\n\\nIf you choose to proceed, you will lose the remaining devices.\\n\\nAre you sure you want to continue?')" +
                    ";");
            }
            else
            {
                btnNext.Attributes.Add("onclick", strAttributes);
                btnUpdate.Attributes.Add("onclick", strAttributes);
            }
        }
        private void LoadClusters()
        {
            DataSet ds = oCluster.Gets(intRequest);
            StringBuilder sb = new StringBuilder(strAttributes);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intCluster = Int32.Parse(dr["id"].ToString());
                DataSet dsCluster = oCluster.Get(intCluster);
                if (dsCluster.Tables[0].Rows.Count > 0)
                {
                    if (dsCluster.Tables[0].Rows[0]["local_nodes"].ToString() != "1")
                    {
                        sb.Append("alert('You have not configured the LOCAL NODES for cluster \"");
                        sb.Append(dsCluster.Tables[0].Rows[0]["nickname"].ToString());
                        sb.Append("\"\\n\\nPlease click \"Edit\" from the cluster list to modify this cluster');return false;");
                    }
                    if (dsCluster.Tables[0].Rows[0]["non_shared"].ToString() != "1")
                    {
                        sb.Append("alert('You have not configured the NON-SHARED STORAGE for cluster \"");
                        sb.Append(dsCluster.Tables[0].Rows[0]["nickname"].ToString());
                        sb.Append("\"\\n\\nPlease click \"Edit\" from the cluster list to modify this cluster');return false;");
                    }
                    if (dsCluster.Tables[0].Rows[0]["add_instance"].ToString() != "1")
                    {
                        sb.Append("alert('You have not ADDED AN INSTANCE for cluster \"");
                        sb.Append(dsCluster.Tables[0].Rows[0]["nickname"].ToString());
                        sb.Append("\"\\n\\nPlease click \"Edit\" from the cluster list to modify this cluster');return false;");
                    }
                    else
                    {
                        DataSet dsInstances = oCluster.GetInstances(intCluster);
                        if (dsInstances.Tables[0].Rows.Count == 0)
                        {
                            sb.Append("alert('You have not ADDED AN INSTANCE for cluster \"");
                            sb.Append(dsCluster.Tables[0].Rows[0]["nickname"].ToString());
                            sb.Append("\"\\n\\nPlease click \"Edit\" from the cluster list to modify this cluster');return false;");
                        }
                    }
                    if (boolMidrange == false && dsCluster.Tables[0].Rows[0]["quorum"].ToString() != "1")
                    {
                        sb.Append("alert('You have not configured the QUORUM for cluster \"");
                        sb.Append(dsCluster.Tables[0].Rows[0]["nickname"].ToString());
                        sb.Append("\"\\n\\nPlease click \"Edit\" from the cluster list to modify this cluster');return false;");
                    }
                }
            }

            strAttributes = sb.ToString();

            rptClusters.DataSource = oCluster.Gets(intRequest);
            rptClusters.DataBind();
            foreach (RepeaterItem ri in rptClusters.Items)
            {
                ((LinkButton)ri.FindControl("btnDelete")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            }
            lblNone.Visible = (rptClusters.Items.Count == 0);
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            oOnDemand.Back(intID);
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&backward=true");
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            oOnDemand.Next(intID, Int32.Parse(Request.QueryString["sid"]));
            Servers oServer = new Servers(0, dsn);
            DataSet dsClusters = oCluster.Gets(intRequest);
            foreach (DataRow drCluster in dsClusters.Tables[0].Rows)
            {
                int intNodes = Int32.Parse(drCluster["nodes"].ToString());
                int intDRLeft = Int32.Parse(drCluster["dr"].ToString());
                int intHALeft = Int32.Parse(drCluster["ha"].ToString());
                int intCluster = Int32.Parse(drCluster["id"].ToString());
                oServer.DeleteClusters(intCluster);
                DataSet dsServers = oServer.GetClusters(intCluster);
                if (dsServers.Tables[0].Rows.Count > 0)
                {
                    int intServerCopy = Int32.Parse(dsServers.Tables[0].Rows[0]["id"].ToString());
                    int intHA = 0;
                    if (intHALeft > 0)
                    {
                        intHALeft--;
                        intHA = 1;
                    }
                    int intDR = 0;
                    if (intDRLeft > 0)
                    {
                        intDRLeft--;
                        intDR = 1;
                    }
                    oServer.Update(intServerCopy, intHA, intDR);
                    for (int ii = 1; ii < intNodes; ii++)
                    {
                        intHA = 0;
                        if (intHALeft > 0)
                        {
                            intHALeft--;
                            intHA = 1;
                        }
                        intDR = 0;
                        if (intDRLeft > 0)
                        {
                            intDRLeft--;
                            intDR = 1;
                        }
                        int intOS = Int32.Parse(dsServers.Tables[0].Rows[0]["osid"].ToString());
                        int intSP = Int32.Parse(dsServers.Tables[0].Rows[0]["spid"].ToString());
                        int intTemplate = Int32.Parse(dsServers.Tables[0].Rows[0]["templateid"].ToString());
                        int intDomain = Int32.Parse(dsServers.Tables[0].Rows[0]["domainid"].ToString());
                        int intDomainTest = Int32.Parse(dsServers.Tables[0].Rows[0]["test_domainid"].ToString());
                        int intINF = Int32.Parse(dsServers.Tables[0].Rows[0]["infrastructure"].ToString());
                        int intDRExist = Int32.Parse(dsServers.Tables[0].Rows[0]["dr_exist"].ToString());
                        string strDRName = dsServers.Tables[0].Rows[0]["dr_name"].ToString();
                        int intDRCons = Int32.Parse(dsServers.Tables[0].Rows[0]["dr_consistency"].ToString());
                        int intDRConsID = Int32.Parse(dsServers.Tables[0].Rows[0]["dr_consistencyid"].ToString());
                        int intLocal = Int32.Parse(dsServers.Tables[0].Rows[0]["local_storage"].ToString());
                        int intAccounts = Int32.Parse(dsServers.Tables[0].Rows[0]["accounts"].ToString());
                        int intF = Int32.Parse(dsServers.Tables[0].Rows[0]["fdrive"].ToString());
                        int intDBA = Int32.Parse(dsServers.Tables[0].Rows[0]["dba"].ToString());
                        int intPNC = Int32.Parse(dsServers.Tables[0].Rows[0]["pnc"].ToString());
                        int intDNSAuto = Int32.Parse(dsServers.Tables[0].Rows[0]["dns_auto"].ToString());
                        int intServer = oServer.Add(intRequest, intID, 0, 0, intCluster, ii, intOS, intSP, intTemplate, intDomain, intDomainTest, intINF, intHA, intDR, intDRExist, strDRName, intDRCons, intDRConsID, 1, intLocal, intAccounts, intF, intDBA, intPNC, 0, intDNSAuto);
                        ServerName oServerName = new ServerName(0, dsn);
                        DataSet dsComponents = oServerName.GetComponentDetailSelected(intServerCopy, 1);
                        foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                            oServerName.AddComponentDetailSelected(intServer, Int32.Parse(drComponent["detailid"].ToString()), Int32.Parse(drComponent["prerequisiteid"].ToString()), false);

                    }
                }
            }
            // Add the server entries for each node.
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
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oCluster.Delete(Int32.Parse(oButton.CommandArgument), intID);
            Response.Redirect(Request.Path + "?id=" + intID + "&delete=true");
        }
    }
}