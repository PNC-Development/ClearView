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
    public partial class config_csm : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected bool boolUseCSM = (ConfigurationManager.AppSettings["USE_CSM_EXECUTION"] == "1");
        protected int intProfile;
        protected OnDemand oOnDemand;
        protected Forecast oForecast;
        protected CSMConfig oCSMConfig;
        protected Requests oRequest;
        protected int intAnswer = 0;
        protected int intConfig = 0;
        protected int intRequest = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oOnDemand = new OnDemand(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oCSMConfig = new CSMConfig(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                intAnswer = Int32.Parse(Request.QueryString["aid"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intConfig = Int32.Parse(Request.QueryString["id"]);
            int intServer = 0;
            int intDR = 0;
            if (intAnswer > 0)
            {
                DataSet ds = oForecast.GetAnswer(intAnswer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intServer = Int32.Parse(ds.Tables[0].Rows[0]["quantity"].ToString()) - oForecast.TotalServerCount(intAnswer, boolUseCSM);
                    intDR = Int32.Parse(ds.Tables[0].Rows[0]["recovery_number"].ToString()) - oForecast.TotalDRCount(intAnswer, boolUseCSM);
                    intRequest = oForecast.GetRequestID(intAnswer, true);
                    if (!IsPostBack)
                    {
                        panView.Visible = true;
                        ds = oCSMConfig.Get(intConfig);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                            txtServers.Text = ds.Tables[0].Rows[0]["servers"].ToString();
                            txtDR.Text = ds.Tables[0].Rows[0]["dr"].ToString();
                        }
                    }
                }
            }
            btnClose.Attributes.Add("onclick", "return window.close();");
            btnDenied.Attributes.Add("onclick", "return window.close();");
            btnSave.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a custom name for this cluster')" +
                " && ValidateNumber0('" + txtServers.ClientID + "','Please enter a valid number for the number of servers')" +
                " && ValidateNumberLess('" + txtServers.ClientID + "'," + intServer + ",'You cannot add any more than " + intServer + " servers')" +
                " && ValidateNumber('" + txtDR.ClientID + "','Please enter a valid number for the number of DR servers')" +
                " && ValidateNumberLess('" + txtDR.ClientID + "'," + intDR + ",'You cannot add any more than " + intDR + " DR servers')" +
                ";");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            if (intConfig == 0)
                intConfig = oCSMConfig.Add(intRequest, txtName.Text, Int32.Parse(txtServers.Text), Int32.Parse(txtDR.Text));
            else
                oCSMConfig.Update(intConfig, txtName.Text, Int32.Parse(txtServers.Text), Int32.Parse(txtDR.Text));
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
    }
}
