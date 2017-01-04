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
using Microsoft.ApplicationBlocks.Data;
namespace NCC.ClearView.Presentation.Web
{
    public partial class workstation_virtual_check : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnRemote = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["RemoteDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        protected int intModelVirtual = Int32.Parse(ConfigurationManager.AppSettings["VirtualWorkstationModelID"]);
        protected int intModelVMware = Int32.Parse(ConfigurationManager.AppSettings["VirtualWorkstationModelID"]);
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            Workstations oWorkstation = new Workstations(0, dsn);
            OnDemand oOnDemand = new OnDemand(0, dsn);
            Forecast oForecast = new Forecast(0, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                Functions oFunction = new Functions(0, dsn, intEnvironment);
                int intWorkstation = Int32.Parse(oFunction.decryptQueryString(Request.QueryString["id"]));
                int intType = oModelsProperties.GetType(intModelVirtual);
                DataSet dsSteps = oOnDemand.GetSteps(intType, 1);
                DataSet ds = oWorkstation.GetVirtual(intWorkstation);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                    int intRemote = Int32.Parse(ds.Tables[0].Rows[0]["remoteid"].ToString());
                    int intCurrent = Int32.Parse(ds.Tables[0].Rows[0]["step"].ToString());
                    Workstations workstation = new Workstations(0, dsnRemote);
                    DataSet dsResult = workstation.GetWorkstationVirtualRemoteStatus(intRemote); 
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        int intStep = Int32.Parse(dsResult.Tables[0].Rows[0]["step"].ToString());
                        int intID = Int32.Parse(dsResult.Tables[0].Rows[0]["id"].ToString());
                        if (intStep == 1)
                        {
                            oWorkstation.AssignHost(intWorkstation, dsnRemote, dsnAsset, intEnvironment, dsnZeus);
                            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "completed", "<script type=\"text/javascript\">window.onload = new Function(\"redirectWait();\");<" + "/" + "script>");
                        }
                        else if (intCurrent < intStep)
                        {
                            oOnDemand.UpdateStepDoneWorkstation(intWorkstation, intCurrent, oOnDemand.GetStep(intStep, "done"), 0, false, false);
                            oWorkstation.NextVirtualStep(intWorkstation);
                            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "completed", "<script type=\"text/javascript\">window.onload = new Function(\"redirect();\");<" + "/" + "script>");
                        }
                        else if (intCurrent == dsSteps.Tables[0].Rows.Count)
                        {
                            oWorkstation.NextVirtualStep(intWorkstation);
                            //SqlHelper.ExecuteNonQuery(dsnRemote, CommandType.Text, "UPDATE cv_virtual_workstations SET deleted = 1 WHERE id = " + intID.ToString() + " AND deleted = 0");
                            oForecast.UpdateAnswerCompleted(intAnswer);
                            oWorkstation.UpdateVirtualCompleted(intWorkstation);
                            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "completed", "<script type=\"text/javascript\">window.onload = new Function(\"redirect();\");<" + "/" + "script>");
                        }
                    }
                }
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "completed", "<script type=\"text/javascript\">window.onload = new Function(\"redirectWait();\");<" + "/" + "script>");
            }
        }
    }
}