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
    public partial class config_workstation_virtual : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intOSQuestion = Int32.Parse(ConfigurationManager.AppSettings["ForecastOSQuestionID"]);
        protected int intModelVirtual = Int32.Parse(ConfigurationManager.AppSettings["VirtualWorkstationModelID"]);
        protected int intModelVMware = Int32.Parse(ConfigurationManager.AppSettings["VMwareWorkstationModelID"]);
        protected int intProfile;
        protected OnDemand oOnDemand;
        protected Forecast oForecast;
        protected Requests oRequest;
        protected Workstations oWorkstation;
        protected Storage oStorage;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected OperatingSystems oOperatingSystem;
        protected int intAnswer = 0;
        protected int intNumber = 0;
        protected int intRequest = 0;
        protected int intOS = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oOnDemand = new OnDemand(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oWorkstation = new Workstations(intProfile, dsn);
            oStorage = new Storage(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                intAnswer = Int32.Parse(Request.QueryString["aid"]);
            if (Request.QueryString["num"] != null && Request.QueryString["num"] != "")
                intNumber = Int32.Parse(Request.QueryString["num"]);
            int intWorkstation = 0;
            if (intAnswer > 0)
            {
                DataSet ds = oForecast.GetAnswer(intAnswer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int intModel = oForecast.GetModel(intAnswer);
                    intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                    int _classid = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                    int _environmentid = Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString());
                    intOS = Int32.Parse(oForecast.GetWorkstation(intAnswer, "osid"));
                    intRequest = oForecast.GetRequestID(intAnswer, true);
                    if (!IsPostBack)
                    {
                        LoadLists(_classid, _environmentid);
                        ds = oWorkstation.GetVirtual(intAnswer, intNumber);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            intWorkstation = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                            lblId.Text = intWorkstation.ToString();
                            intOS = Int32.Parse(ds.Tables[0].Rows[0]["osid"].ToString());
                        }
                        if (intOS > 0)
                        {
                            ddlOS.SelectedValue = intOS.ToString();
                            ddlServicePack.Enabled = true;
                            ddlServicePack.DataValueField = "id";
                            ddlServicePack.DataTextField = "name";
                            ddlServicePack.DataSource = oOperatingSystem.GetServicePack(intOS);
                            ddlServicePack.DataBind();
                            ddlServicePack.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                            chkComponents.DataValueField = "id";
                            chkComponents.DataTextField = "name";
                            chkComponents.DataSource = oWorkstation.GetComponentPermissionsOS(intOS);
                            chkComponents.DataBind();
                            DataSet dsComponents = oWorkstation.GetComponentsSelected(intWorkstation);
                            foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                            {
                                foreach (ListItem oItem in chkComponents.Items)
                                {
                                    if (drComponent["componentid"].ToString() == oItem.Value)
                                    {
                                        oItem.Selected = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlServicePack.SelectedValue = ds.Tables[0].Rows[0]["spid"].ToString();
                            hdnServicePack.Value = ds.Tables[0].Rows[0]["spid"].ToString();
                            ddlDomain.SelectedValue = ds.Tables[0].Rows[0]["domainid"].ToString();
                        }
                    }
                    btnClose.Attributes.Add("onclick", "return window.close();");
                    btnSaveConfig.Attributes.Add("onclick", "return ValidateDropDown('" + ddlServicePack.ClientID + "','Please select a service pack')" +
                        " && ValidateDropDown('" + ddlDomain.ClientID + "','Please select a domain')" +
                        ";");
                    ddlOS.Attributes.Add("onchange", "PopulateServicePacks('" + ddlOS.ClientID + "','" + ddlServicePack.ClientID + "');");
                    ddlServicePack.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlServicePack.ClientID + "','" + hdnServicePack.ClientID + "');");
                }
            }
        }
        private void LoadLists(int _classid, int _environmentid)
        {
            ddlOS.DataValueField = "id";
            ddlOS.DataTextField = "name";
            ddlOS.DataSource = oOperatingSystem.Gets(1, 1);
            ddlOS.DataBind();
            ddlOS.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            Domains oDomain = new Domains(intProfile, dsn);
            ddlDomain.DataValueField = "id";
            ddlDomain.DataTextField = "name";
            ddlDomain.DataSource = oDomain.GetClassEnvironment(_classid, _environmentid);
            ddlDomain.DataBind();
            ddlDomain.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void btnSaveConfig_Click(Object Sender, EventArgs e)
        {
            int intConfigured = ((ddlOS.SelectedIndex > 0 && Int32.Parse(Request.Form[hdnServicePack.UniqueID]) > 0 && ddlDomain.SelectedIndex > 0) ? 1 : 0);
            int intWorkstation = 0;
            if (lblId.Text != "")
            {
                intWorkstation = Int32.Parse(lblId.Text);
                oWorkstation.UpdateVirtual(intWorkstation, Int32.Parse(ddlOS.SelectedItem.Value), Int32.Parse(Request.Form[hdnServicePack.UniqueID]), Int32.Parse(ddlDomain.SelectedItem.Value), intConfigured);
            }
            else
                intWorkstation = oWorkstation.AddVirtual(intRequest, intAnswer, intNumber, 0, intModelVirtual, Int32.Parse(ddlOS.SelectedItem.Value), Int32.Parse(Request.Form[hdnServicePack.UniqueID]), Int32.Parse(ddlDomain.SelectedItem.Value), Int32.Parse(oForecast.GetAnswerWorkstation(intAnswer, "ramid")), 0, 0, Int32.Parse(oForecast.GetAnswerWorkstation(intAnswer, "hddid")), Int32.Parse(oForecast.GetAnswerWorkstation(intAnswer, "cpuid")), intConfigured, -1);
            oWorkstation.DeleteComponents(intWorkstation);
            foreach (ListItem oItem in chkComponents.Items)
            {
                if (oItem.Selected == true)
                    oWorkstation.AddComponents(intWorkstation, Int32.Parse(oItem.Value));
            }
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
    }
}
