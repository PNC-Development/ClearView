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
using NCC.ClearView.Presentation.Web.Custom;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Presentation.Web
{
    public partial class form : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intDataPointAvailableService = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_SERVICE"]);
        protected DataPoint oDataPoint;
        protected Users oUser;
        protected ResourceRequest oResourceRequest;
        protected Requests oRequest;
        protected Functions oFunction;
        protected RequestFields oRequestField;
        protected Pages oPage;
        protected Log oLog;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected int intForm = 0;
        protected string strOriginal = "";
        protected string strRequestor = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            oDataPoint = new DataPoint(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oRequestField = new RequestFields(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oLog = new Log(intProfile, dsn);
            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "SERVICE") == true || intDataPointAvailableService == 1))
            {
                panAllow.Visible = true;
                if (Request.QueryString["close"] != null)
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">window.close();<" + "/" + "script>");
                else if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    string strID = oFunction.decryptQueryString(Request.QueryString["id"]);
                    intForm = Int32.Parse(strID);
                    DataSet ds = oDataPoint.GetServiceRequestForm(intForm);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        // Load General Information
                       
                        //string strHeader = intResource.ToString();
                        string strHeader = ds.Tables[0].Rows[0]["requestid"].ToString() 
                                +"-" +(ds.Tables[0].Rows[0]["ServiceId"]!=DBNull.Value?ds.Tables[0].Rows[0]["ServiceId"].ToString():"0") 
                                +"-"+(ds.Tables[0].Rows[0]["number"] != DBNull.Value ? ds.Tables[0].Rows[0]["number"].ToString() : "0");

                        lblHeader.Text = "&quot;" + strHeader + "&quot;";
                        Master.Page.Title = "DataPoint | Request (" + strHeader + ")";
                        lblHeaderSub.Text = "Provides all the information about a resource request...";
                        
                        if (!IsPostBack)
                        {
                            int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                            int intRequestor = oRequest.GetUser(intRequest);
                            strRequestor = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenWindow('PROFILE','?userid=" + intRequestor.ToString() + "');\">" + oUser.GetFullName(intRequestor) + " [" + oUser.GetName(intRequestor) + "]</a>";
                            int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                            int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                            int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                            strOriginal = oRequestField.GetBody(intRequest, intItem, intNumber, intService, 0, 0, dsnServiceEditor, intEnvironment, dsnAsset, dsnIP);
                        }
                    }
                    else
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">window.close();<" + "/" + "script>");
                }
                else
                    Response.Redirect("/datapoint/service/datapoint_service_search.aspx");
                btnClose.Attributes.Add("onclick", "window.close();return false;");
                btnPrint.Attributes.Add("onclick", "window.print();return false;");
            }
            else
                panDenied.Visible = true;
        }
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect("/datapoint/service/datapoint_service_search.aspx");
        }
    }
}
