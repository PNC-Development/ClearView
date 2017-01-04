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
    public partial class supply_component : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Platforms oPlatform;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Forecast oForecast;
        protected Pages oPage;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intPlatform = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intPlatform = Int32.Parse(Request.QueryString["id"]);
            if (intPlatform > 0)
            {
                LoadLists();
            }
            ddlTypes.Attributes.Add("onchange", "WaitDDL('" + divTypes.ClientID + "');");
        }
        public void LoadLists()
        {
            ddlTypes.DataValueField = "id";
            ddlTypes.DataTextField = "name";
            ddlTypes.DataSource = oType.Gets(intPlatform, 1);
            ddlTypes.DataBind();
            ddlTypes.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            int intType = 0;
            if (Request.QueryString["tid"] != null)
            {
                Int32.TryParse(Request.QueryString["tid"], out intType);
                ddlTypes.SelectedValue = intType.ToString();
                string strPath = oType.Get(intType, "configuration_path");
                phComponent.Controls.Add((UserControl)LoadControl(strPath));

            }
        }
        protected void ddlTypes_Change(Object Sender, EventArgs e)
        {
            if (ddlTypes.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&tid=" + ddlTypes.SelectedItem.Value + "&div=S");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"]);
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"]);
        }
    }
}