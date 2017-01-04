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
    public partial class workstations_vmware : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected OnDemand oOnDemand;
        protected Forecast oForecast;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Workstations oWorkstation;
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
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oOnDemand = new OnDemand(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oWorkstation = new Workstations(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
                intStep = Int32.Parse(Request.QueryString["sid"]);
            if (Request.QueryString["step"] != null && Request.QueryString["step"] != "")
                panUpdate.Visible = true;
            else
                panNavigation.Visible = true;
            if (intID > 0)
            {
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
                    if (!IsPostBack)
                        LoadDevices(intModel);
                    if (boolConfigured == true)
                        panValid.Visible = true;
                    //                else
                    //                    panInvalid.Visible = true;
                }
            }
            btnClose.Attributes.Add("onclick", "return window.close();");
            btnNext.Attributes.Add("onclick", "return ValidateBoolean(" + (boolConfigured ? "true" : "false") + ",'You cannot continue until you have configured all devices.\\n\\nClick OK to close this window. Then click [Edit] to configure each device.');");
            btnUpdate.Attributes.Add("onclick", "return ValidateBoolean(" + (boolConfigured ? "true" : "false") + ",'You cannot continue until you have configured all devices.\\n\\nClick OK to close this window. Then click [Edit] to configure each device.');");
        }
        private void LoadDevices(int intModel)
        {
            for (int ii = 1; ii <= intTotalCount; ii++)
                AddDevice(oModel.Get(intModel, "name"), "---", ii.ToString(), ii);
        }
        private void AddDevice(string _type, string _name, string _count, int _number)
        {
            boolOther = !boolOther;
            strDevices += "<tr" + (boolOther ? " bgcolor=\"F6F6F6\"" : "") + ">";
            strDevices += "<td>" + _type + "</td>";
            strDevices += "<td>" + _name + "</td>";
            strDevices += "<td>" + _count + "</td>";
            DataSet ds = oWorkstation.GetVirtual(intID, _number);
            bool boolDeviceConfigured = true;
            if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0]["configured"].ToString() != "1")
                boolDeviceConfigured = false;
            if (boolDeviceConfigured == true)
                strDevices += "<td nowrap align=\"center\"><img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\"/> <input type=\"button\" class=\"default\" value=\"Edit\" style=\"width:50px\" onclick=\"OpenOnDemandDevice(" + intID + "," + _number.ToString() + ");\"/></td>";
            else
            {
                strDevices += "<td nowrap align=\"center\"><img src=\"/images/required.gif\" border=\"0\" align=\"absmiddle\"/> <input type=\"button\" class=\"default\" value=\"Edit\" style=\"width:50px\" onclick=\"OpenOnDemandDevice(" + intID + "," + _number.ToString() + ");\"/></td>";
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
                if (ds.Tables[0].Rows[0]["accounts"].ToString() == "0")
                {
                    strDevices += "<td nowrap align=\"center\"><img src=\"/images/required.gif\" border=\"0\" align=\"absmiddle\"/> <input type=\"button\" class=\"default\" value=\"Edit\" style=\"width:50px\" onclick=\"OpenWindow('NEW_WINDOW','/frame/ondemand/accounts_vmware.aspx?id=" + ds.Tables[0].Rows[0]["id"].ToString() + "');\"/></td>";
                    boolConfigured = false;
                }
                else
                    strDevices += "<td nowrap align=\"center\"><img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\"/> <input type=\"button\" class=\"default\" value=\"Edit\" style=\"width:50px\" onclick=\"OpenWindow('NEW_WINDOW','/frame/ondemand/accounts_vmware.aspx?id=" + ds.Tables[0].Rows[0]["id"].ToString() + "');\"/></td>";
            }
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