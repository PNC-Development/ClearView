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
using System.IO;
using NCC.ClearView.Presentation.Web.Custom;
namespace NCC.ClearView.Presentation.Web
{
    public partial class errors_decommission : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intServiceDecom = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_DECOMMISSION_SUPPORT"]);
        protected int intServiceDestroy = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_DESTROY_SUPPORT"]);
        protected int intServiceDecomW = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_DECOMMISSION_SUPPORT_WORKSTATION"]);
        protected int intServiceDestroyW = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_DESTROY_SUPPORT_WORKSTATION"]);
        protected Asset oAsset;
        protected Requests oRequest;
        protected Log oLog;
        protected int intProfile;
        protected string strErrors = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/errors_decommission.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oAsset = new Asset(0, dsnAsset);
            oRequest = new Requests(intProfile, dsn);
            oLog = new Log(intProfile, dsn);
            if (!IsPostBack)
            {
                if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "" && Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
                {
                    int intAsset = Int32.Parse(Request.QueryString["aid"]);
                    int intRequest = Int32.Parse(Request.QueryString["rid"]);
                    oAsset.UpdateDecommissionFixed(intAsset);
                    oRequest.DeleteResults(intRequest);
                    Response.Redirect(Request.Path);
                }
                else
                    LoadErrors();
            }
        }
        private void LoadErrors()
        {
            LoadErrors(oAsset.GetDecommissionErrors(intServiceDecom, intServiceDestroy));
            LoadErrors(oAsset.GetDecommissionErrors(intServiceDecomW, intServiceDestroyW));
            if (strErrors == "")
                strErrors = "<tr><td>There is no information...</td></tr>";
            strErrors = "<table width=\"100%\" cellpadding=\"3\" cellspacing=\"2\" border=\"0\">" + strErrors;
            strErrors += "</table>";
        }
        private void LoadErrors(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    strErrors += "<tr>";
                    int intAsset = Int32.Parse(dr["assetid"].ToString());
                    int intRequest = Int32.Parse(dr["requestid"].ToString());
                    int intItem = Int32.Parse(dr["itemid"].ToString());
                    int intNumber = Int32.Parse(dr["number"].ToString());
                    string strName = dr["name"].ToString();
                    strErrors += "<td nowrap><a href=\"javascript:void(0);\" onclick=\"ShowHideDiv2('divError_" + dr["id"].ToString() + "');\">" + dr["name"].ToString() + "</a></td>";
                    string strResult = "No information...";
                    DataSet dsLog = oLog.GetEventsByName(strName, (int)LoggingType.Error);
                    if (dsLog.Tables[0].Rows.Count > 0)
                        strResult = dsLog.Tables[0].Rows[0]["message"].ToString();
                    strErrors += "<td width=\"100%\">" + strResult + "</td>";
                    strErrors += "<td nowrap>" + dr["modified"].ToString() + "</td>";
                    strErrors += "<td nowrap><a onclick=\"return confirm('Are you sure you want to mark this as fixed?');\" href=\"" + Request.Path + "?aid=" + intAsset.ToString() + "&rid=" + intRequest.ToString() + "\">Fixed</a></td>";
                    strErrors += "</tr>";
                    strErrors += "<tr id=\"divError_" + dr["id"].ToString() + "\" style=\"display:none\">";
                    strErrors += "<td></td>";
                    strErrors += "<td colspan=\"3\">";
                    strErrors += "<table cellpadding=\"3\" cellspacing=\"2\" border=\"0\">";
                    foreach (DataRow drLog in dsLog.Tables[0].Rows)
                        strErrors += "<tr><td valign=\"top\" nowrap><b>" + drLog["created"].ToString() + "&nbsp;:</b></td><td valign=\"top\">" + drLog["message"].ToString() + "</td></tr>";
                    strErrors += "</table>";
                    strErrors += "</td>";
                    strErrors += "</tr>";
                }
            }
        }

    }
}
