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
    public partial class settings : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Platforms oPlatform;
        protected Types oType;
        protected ModelsProperties oModelsProperties;
        protected Asset oAsset;
        protected ServiceRequests oServiceRequest;
        protected Forecast oForecast;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strThreshold = "";
        protected string strModelThreshold = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            int intPlatform = 0;
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intPlatform = Int32.Parse(Request.QueryString["id"]);
            if (intPlatform > 0)
            {
                if (!IsPostBack)
                {
                    txtMax1.Text = oPlatform.Get(intPlatform, "max_inventory1");
                    bool boolOther2 = false;
                    DataSet dsTypes = oType.Gets(intPlatform, 1);
                    foreach (DataRow drType in dsTypes.Tables[0].Rows)
                    {
                        boolOther2 = !boolOther2;
                        strThreshold += "<tr" + (boolOther2 ? " bgcolor=\"F6F6F6\"" : "") + " class=\"default\"><td nowrap class=\"bold\">" + drType["name"].ToString() + ":</td><td nowrap><input id=\"" + drType["id"].ToString() + "_warning\" name=\"" + drType["id"].ToString() + "_warning\" type=\"text\" class=\"default\" maxlength=\"5\" value=\"" + drType["inventory_warning"].ToString() + "\" style=\"width:50px\"/></td><td nowrap><input id=\"" + drType["id"].ToString() + "_critical\" name=\"" + drType["id"].ToString() + "_critical\" type=\"text\" class=\"default\" maxlength=\"5\" value=\"" + drType["inventory_critical"].ToString() + "\" style=\"width:50px\"/></td></tr>";

                        bool boolPhysical = false;
                        bool boolBlade = false;
                        bool boolOther = false;
                        int intOldHost = -1;
                        DataSet dsModels = oModelsProperties.GetTypes(1, Int32.Parse(drType["id"].ToString()), 1);
                        foreach (DataRow drModel in dsModels.Tables[0].Rows)
                        {
                            boolOther = !boolOther;
                            int intBlade = Int32.Parse(drModel["blade"].ToString());
                            int intHost = -1;
                            if (drModel["hostid"].ToString() != "")
                                intHost = Int32.Parse(drModel["hostid"].ToString());
                            string strType = "Unknown";
                            int intModel = Int32.Parse(drModel["id"].ToString());
                            if (intBlade == 1)
                            {
                                strType = "Blade";
                                if (boolBlade == false)
                                {
                                    if (strModelThreshold != "")
                                        strModelThreshold += "<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>";
                                    strModelThreshold += "<tr><td nowrap colspan=\"3\" class=\"bold\">" + strType + "</td><td nowrap align=\"center\" class=\"bold\">Thresholds</td></tr>";
                                    boolBlade = true;
                                }
                            }
                            else if (intHost == 0)
                            {
                                strType = "Physical";
                                if (boolPhysical == false)
                                {
                                    if (strModelThreshold != "")
                                        strModelThreshold += "<tr><td colspan=\"4\"><p>&nbsp;</p><p>&nbsp;</p></td></tr>";
                                    strModelThreshold += "<tr><td colspan=\"4\" class=\"header\">" + drType["name"].ToString() + "</td></tr>";
                                    strModelThreshold += "<tr><td nowrap colspan=\"3\" class=\"bold\">" + strType + "</td><td nowrap align=\"center\" class=\"bold\">Thresholds</td></tr>";
                                    boolPhysical = true;
                                }
                            }
                            else if (intOldHost != intHost)
                            {
                                strType = drModel["host"].ToString();
                                if (intHost > 0)
                                {
                                    if (strModelThreshold != "")
                                        strModelThreshold += "<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>";
                                    strModelThreshold += "<tr><td nowrap colspan=\"3\" class=\"bold\">" + strType + "</td><td nowrap align=\"center\" class=\"bold\">Thresholds</td></tr>";
                                    intOldHost = intHost;
                                }
                            }
                            strModelThreshold += "<tr" + (boolOther ? " bgcolor=\"F6F6F6\"" : "") + " class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap><input type=\"button\" class=\"default\" value=\"Edit\" onclick=\"return OpenWindow('MODEL_THRESHOLDS','?mid=" + drModel["id"].ToString() + "');\" style=\"width:75px\"/></td><td nowrap width=\"100%\">" + drModel["name"].ToString() + ":</td><td nowrap align=\"center\">" + oModelsProperties.GetThresholds(intModel, 1).Tables[0].Rows.Count.ToString() + "</td></tr>";
                        }

                    }
                }
            }
            btnMaximum.Attributes.Add("onclick", "return ValidateNumber0('" + txtMax1.ClientID + "','Please enter a valid number');");
        }
        protected void btnMaximum_Click(Object Sender, EventArgs e)
        {
            oPlatform.Update(Int32.Parse(Request.QueryString["id"]), Int32.Parse(txtMax1.Text), 0, 0);
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=t");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            foreach (string strName in Request.Form)
            {
                string strForm = strName.ToUpper();
                if (strForm.EndsWith("_WARNING") == true)
                {
                    int intType = Int32.Parse(strForm.Substring(0, strForm.IndexOf("_")));
                    int intWarning = Int32.Parse(Request.Form[intType.ToString() + "_WARNING"]);
                    int intCritical = Int32.Parse(Request.Form[intType.ToString() + "_CRITICAL"]);
                    oType.Update(intType, intWarning, intCritical);
                }
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=t");
        }
    }
}