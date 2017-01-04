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
    public partial class inventory_demand : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intDesignBuilder = Int32.Parse(ConfigurationManager.AppSettings["ForecastEdit"]);
        protected Forecast oForecast;
        protected Functions oFunction;
        protected ModelsProperties oModelsProperties;
        protected int intProfile;
        protected int intModel = 0;
        protected string strResults = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            Pages oPage = new Pages(intProfile, dsn);
            if (Request.QueryString["done"] != null && Request.QueryString["done"] != "")
            {
                oForecast.UpdateAnswerSetComplete(Int32.Parse(Request.QueryString["done"]));
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Design Marked Completed Successfully\\n\\nPlease wait while the page reloads....');window.navigate('" + Request.Path + "?model=" + Request.QueryString["model"] + "&filters=" + Request.QueryString["filters"] + "');<" + "/" + "script>");
            }
            else if (Request.QueryString["model"] != null && Request.QueryString["model"] != "" && Request.QueryString["filters"] != null)
            {
                string strFilter = oFunction.decryptQueryString(Request.QueryString["filters"]);
                intModel = Int32.Parse(Request.QueryString["model"]);
                int intType = oModelsProperties.GetType(intModel);
                Types oType = new Types(intProfile, dsn);
                int intPlatform = oType.GetPlatform(intType);
                lblModel.Text = oModelsProperties.Get(intModel, "name");
                bool boolOther = false;
                DataSet dsDemand = oForecast.GetAnswersModel(intPlatform);
                DataTable dtDemand = dsDemand.Tables[0];
                DataRow[] drModels = dtDemand.Select("model = " + intModel.ToString() + strFilter);
                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                Users oUser = new Users(0, dsn);
                StringBuilder sb = new StringBuilder(strResults);
                OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);

                foreach (DataRow dr in drModels)
                {
                    int intProject = Int32.Parse(dr["projectid"].ToString());
                    int intAnswer = Int32.Parse(dr["id"].ToString());
                    int intImplementor = 0;
                    DataSet dsTasks = oOnDemandTasks.GetPending(intAnswer);
                    if (dsTasks.Tables[0].Rows.Count > 0)
                    {
                        intImplementor = Int32.Parse(dsTasks.Tables[0].Rows[0]["resourceid"].ToString());
                        intImplementor = Int32.Parse(oResourceRequest.GetWorkflow(intImplementor, "userid"));
                    }
                    else
                        intImplementor = -999;
                    string strXID = "";
                    if (intImplementor > 0 || intImplementor == -999)
                    strXID = oUser.GetFullName(intImplementor);
                    
                    boolOther = !boolOther;
                    sb.Append("<tr");
                    sb.Append(boolOther ? " bgcolor=\"F6F6F6\"" : "");
                    sb.Append("><td><a onclick=\"return confirm('Are you sure you want to mark this design as being completed?');\" title=\"Mark as Complete\" href=\"");
                    sb.Append(Request.Path);
                    sb.Append("?model=");
                    sb.Append(intModel.ToString());
                    sb.Append("&filters=");
                    sb.Append(Request.QueryString["filters"]);
                    sb.Append("&done=");
                    sb.Append(dr["id"].ToString());
                    sb.Append("\"><img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\"/></a></td><td>");
                    sb.Append(dr["id"].ToString());
                    sb.Append("</td><td><a href=\"");
                    sb.Append(oPage.GetFullLink(intDesignBuilder));
                    sb.Append("?id=");
                    sb.Append(dr["forecastid"].ToString());
                    sb.Append("&highlight=");
                    sb.Append(dr["id"].ToString());
                    sb.Append("\" target=\"_blank\">");
                    sb.Append(dr["project_name"].ToString());
                    sb.Append("</a></td><td>");
                    sb.Append(dr["project_number"].ToString());
                    sb.Append("</td><td>");
                    sb.Append(dr["confidence"].ToString());
                    sb.Append("</td><td>");
                    sb.Append(dr["implementation"].ToString());
                    sb.Append("</td><td>");
                    sb.Append(dr["class"].ToString());
                    sb.Append("</td><td>");
                    sb.Append(dr["environment"].ToString());
                    sb.Append("</td><td>");
                    sb.Append(dr["location"].ToString());
                    sb.Append("</td><td>");
                    sb.Append(strXID);
                    sb.Append("</td><td align=\"right\">");
                    sb.Append(dr["quantity"].ToString());
                    sb.Append("</td><td align=\"right\">");
                    sb.Append(dr["recovery_number"].ToString());
                    sb.Append("</td></tr>");
                }

                strResults = sb.ToString();
            }
        }
    }
}
