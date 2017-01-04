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
    public partial class fore_storage : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intStoragePerBladeOs = Int32.Parse(ConfigurationManager.AppSettings["STORAGE_PER_BLADE_OS"]);
        protected int intStoragePerBladeApp = Int32.Parse(ConfigurationManager.AppSettings["STORAGE_PER_BLADE_APP"]);
        protected int intProfile;
        protected Forecast oForecast;
        protected Platforms oPlatform;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (intID > 0)
            {
                DataSet ds = oForecast.GetAnswer(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int intPlatform = Int32.Parse(ds.Tables[0].Rows[0]["platformid"].ToString());
                    int intQuantity = Int32.Parse(ds.Tables[0].Rows[0]["quantity"].ToString());
                    // Get Model
                    int intModel = oForecast.GetModel(intID);
                    int intParent = 0;
                    if (intModel > 0)
                    {
                        panContinue.Visible = true;
                        if (oModelsProperties.Get(intModel).Tables[0].Rows.Count > 0)
                            intParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                        if (oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                        {
                            panBlade.Visible = true;
                            lblQuantity.Text = intQuantity.ToString();
                            int intMinimumOS = intQuantity * intStoragePerBladeOs;
                            int intMinimumApp = intQuantity * intStoragePerBladeApp;
                            lblMinimumOS.Text = intMinimumOS.ToString();
                            lblMinimumApp.Text = intMinimumApp.ToString();
                        }
                        string strPath = "/controls/sys/sys_default.ascx";
                        if (oModelsProperties.IsStorageDB_BootLocal(intModel) == true)
                            strPath = "/controls/fore/fore_storage_old.ascx";
                        else if (oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                            strPath = "/controls/fore/fore_storage_new.ascx";
                        else if (oModelsProperties.IsStorageDB_BootSANUnix(intModel) == true)
                            strPath = "/controls/fore/fore_storage_new_unix.ascx";
                        else
                            Response.Write("MODEL: " + intModel.ToString());
                        PHStorage.Controls.Add((UserControl)LoadControl(strPath));
                    }
                    else
                    {
                        DataSet dsStorage = oForecast.GetStorage(intID);
                        if (dsStorage.Tables[0].Rows.Count > 0)
                        {
                            panReset.Visible = true;
                            btnReset.Attributes.Add("onclick", "return OpenWindow('RESET_STORAGE', '?id=" + intID.ToString() + "');");
                        }
                        else
                            panStop.Visible = true;
                    }
                }
            }
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            oForecast.UpdateAnswerStep(intID, -1, -1);
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
    }
}