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
using System.Diagnostics;
using System.Text;
namespace NCC.ClearView.Presentation.Web
{
    public partial class server_output : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intUnder48A = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_RESPONSE"]);
        protected int intUnder48Q = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_QUESTION"]);
        protected int intDesignBuilder = Int32.Parse(ConfigurationManager.AppSettings["ForecastEdit"]);
        protected Pages oPage;
        protected Users oUser;
        protected Servers oServer;
        protected ResourceRequest oResourceRequest;
        protected Forecast oForecast;
        protected Functions oFunctions;
        protected OnDemandTasks oOnDemandTasks;
        protected Log oLog;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strMultiple = "";
        protected string strResults = "";
        protected string strLog = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oFunctions = new Functions(intProfile, dsn, intEnvironment);
            oOnDemandTasks = new OnDemandTasks(intProfile, dsn);
            oLog = new Log(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            if (!IsPostBack)
            {
                if (Request.QueryString["name"] != null)
                {
                    string strQuery = oFunctions.decryptQueryString(Request.QueryString["name"]);
                    txtName.Text = strQuery;
                    CheckResults(strQuery);
                }
                txtName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");
                btnSearch.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter the server name');");
            }
        }
        private void CheckResults(string strQuery)
        {
            panSearch.Visible = true;
            DataSet ds = oServer.Get(strQuery, false);
            if (ds.Tables[0].Rows.Count == 1)
            {
                lblServer.Text = strQuery.ToUpper();
                int intServer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                DataSet dsOutput = oServer.GetOutput(intServer);
                StringBuilder sb = new StringBuilder(strResults);

                foreach (DataRow drOutput in dsOutput.Tables[0].Rows)
                {
                    sb.Append("<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowHideDiv2('div");
                    sb.Append(drOutput["id"].ToString());
                    sb.Append("');\">");
                    sb.Append(drOutput["type"].ToString());
                    sb.Append("</a></td></tr>");
                    sb.Append("<tr id=\"div");
                    sb.Append(drOutput["id"].ToString());
                    sb.Append("\" style=\"display:none\"><td>");
                    sb.Append(oFunctions.FormatText(drOutput["output"].ToString()));
                    sb.Append("</td></tr>");
                }

                strResults = sb.ToString();
            }
            else
            {
                //Workstations oWorkstation = new Workstations(0, dsn);
                //oWorkstation.GetName(strQuery);
                //ds = oW
                //DataSet dsOutput = oWorkstation.GetVirtualOutput(intWorkstation);
                //foreach (DataRow drOutput in dsOutput.Tables[0].Rows)
                //{
                //    strAdministration += "<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowHideDiv2('div" + drOutput["id"].ToString() + "');\">" + drOutput["type"].ToString() + "</a></td></tr>";
                //    strAdministration += "<tr id=\"div" + drOutput["id"].ToString() + "\" style=\"display:none\"><td>" + oFunction.FormatText(drOutput["output"].ToString()) + "</td></tr>";
                //}
            }
            // Get Event Log Entries
            if (Request.QueryString["log"] != null)
            {
                chkLog.Checked = true;
                strLog += "<tr><td>" + oLog.GetEvents(oLog.GetEventsByName(strQuery.ToUpper(), (int)LoggingType.Debug), intEnvironment) + "</td></tr>";
            }
        }
        protected void btnSearch_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?name=" + oFunctions.encryptQueryString(txtName.Text) + (chkLog.Checked ? "&log=true" : ""));
        }
    }
}
