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
    public partial class status : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected int intProfile;
        protected int intID = 0;
        protected int intRequest = 0;
        protected Forecast oForecast;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intID = Int32.Parse(Request.QueryString["id"]);
                intRequest = oForecast.GetRequestID(intID, true);
            }
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
                intRequest = Int32.Parse(Request.QueryString["rid"]);
            if (intRequest > 0)
            {
                DataSet ds = oForecast.GetAnswerRequest(intRequest);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                    int intModel = oForecast.GetModelAsset(intAnswer);
                    if (intModel == 0)
                        intModel = oForecast.GetModel(intAnswer);
                    intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                    oForecast.UpdateAnswer(intAnswer, intRequest);
                    oForecast.DeleteReset(intAnswer);
                    int intType = oModel.GetType(intModel);
                    string strPath = oType.Get(intType, "ondemand_execution_path");
                    if (strPath != "")
                        PHStep.Controls.Add((Control)LoadControl(strPath));
                }
            }
        }
    }
}
