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
    public partial class server_info : System.Web.UI.UserControl
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
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strMultiple = "";
        protected string strResults = "";
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
            DataSet ds = oServer.Get(strQuery, false);
            if (ds.Tables[0].Rows.Count == 1)
            {
                panSearch.Visible = true;
                lblServer.Text = strQuery.ToUpper();
                int intAnswer = 0;
                int intProject = 0;
                if (ds.Tables[0].Rows[0]["answerid"].ToString() != "")
                    intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                if (ds.Tables[0].Rows[0]["projectid"].ToString() != "")
                    intProject = Int32.Parse(ds.Tables[0].Rows[0]["projectid"].ToString());
                if (intProject > 0 && intAnswer > 0)
                {
                    ModelsProperties oModelsProperties = new ModelsProperties(intProfile, dsn);
                    Types oType = new Types(intProfile, dsn);
                    int intServer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                    int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
                    strResults += "<tr><td nowrap>Server Name Generation:</td><td width=\"100%\">Generated via On-Demand Server Auto-Provisioning</td></tr>";
                    strResults += "<tr><td nowrap>Project ID:</td><td width=\"100%\">" + intProject.ToString() + "</td></tr>";
                    strResults += "<tr><td nowrap>Project Name:</td><td width=\"100%\">" + ds.Tables[0].Rows[0]["name"].ToString() + "</td></tr>";
                    strResults += "<tr><td nowrap>Project Number:</td><td width=\"100%\">" + ds.Tables[0].Rows[0]["number"].ToString() + "</td></tr>";
                    int intLead = 0;
                    if (ds.Tables[0].Rows[0]["lead"].ToString() != "")
                        intLead = Int32.Parse(ds.Tables[0].Rows[0]["lead"].ToString());
                    strResults += "<tr><td nowrap>Project Lead:</td><td width=\"100%\">" + oUser.GetFullName(intLead) + " (" + oUser.GetName(intLead) + ")</td></tr>";
                    int intEngineer = 0;
                    if (ds.Tables[0].Rows[0]["engineer"].ToString() != "")
                        intEngineer = Int32.Parse(ds.Tables[0].Rows[0]["engineer"].ToString());
                    strResults += "<tr><td nowrap>Integration Engineer:</td><td width=\"100%\">" + oUser.GetFullName(intEngineer) + " (" + oUser.GetName(intEngineer) + ")</td></tr>";
                    strResults += "<tr><td nowrap>Implementor:</td><td width=\"100%\">" + GetImplementor(intProject, intAnswer) + "</td></tr>";
                    int intBackup = Int32.Parse(oForecast.GetAnswer(intAnswer, "backup"));
                    DataSet dsBackup = oOnDemandTasks.GetServerBackup(intAnswer);
                    if (dsBackup.Tables[0].Rows.Count > 0)
                    {
                        int intRequestB = Int32.Parse(dsBackup.Tables[0].Rows[0]["requestid"].ToString());
                        int intItemB = Int32.Parse(dsBackup.Tables[0].Rows[0]["itemid"].ToString());
                        int intNumberB = Int32.Parse(dsBackup.Tables[0].Rows[0]["number"].ToString());
                        DataSet dsResourceB = oResourceRequest.Get(intRequestB, intItemB, intNumberB);
                        if (dsResourceB.Tables[0].Rows.Count > 0)
                        {
                            strResults += "<tr><td nowrap>Backup:</td>";
                            string strUsersB = "";
                            foreach (DataRow drResourceB in dsResourceB.Tables[0].Rows)
                            {
                                int intUserB = Int32.Parse(drResourceB["userid"].ToString());
                                if (strUsersB != "")
                                    strUsersB += ", ";
                                strUsersB += oUser.GetFullName(intUserB) + "&nbsp;&nbsp;&nbsp;(" + (dsBackup.Tables[0].Rows[0]["chk1"].ToString() == "1" ? "Complete" : "Incomplete") + ")";
                            }
                            strResults += "<td width=\"100%\">" + strUsersB + "</td></tr>";
                        }
                        else
                            strResults += "<tr><td nowrap>Backup:</td><td width=\"100%\">Pending Assignment</td></tr>";
                    }
                    else
                        strResults += "<tr><td nowrap>Backup:</td><td width=\"100%\">Not Submitted (" + (intBackup == 1 ? "Client <b>DID</b> Request a Backup" : "Client <b>DID NOT</b> Request a Backup") + ")</td></tr>";
                    if (oModelsProperties.IsTypeVMware(intModel) == false)
                    {
                        int intStorage = Int32.Parse(oForecast.GetAnswer(intAnswer, "storage"));
                        DataSet dsStorageT = oOnDemandTasks.GetServerStorage(intAnswer, 0);
                        if (dsStorageT.Tables[0].Rows.Count > 0)
                        {
                            int intRequestST = Int32.Parse(dsStorageT.Tables[0].Rows[0]["requestid"].ToString());
                            int intItemST = Int32.Parse(dsStorageT.Tables[0].Rows[0]["itemid"].ToString());
                            int intNumberST = Int32.Parse(dsStorageT.Tables[0].Rows[0]["number"].ToString());
                            DataSet dsResourceST = oResourceRequest.Get(intRequestST, intItemST, intNumberST);
                            if (dsResourceST.Tables[0].Rows.Count > 0)
                            {
                                strResults += "<tr><td nowrap>Storage (TEST):</td>";
                                string strUsersST = "";
                                foreach (DataRow drResourceST in dsResourceST.Tables[0].Rows)
                                {
                                    int intUserST = Int32.Parse(drResourceST["userid"].ToString());
                                    if (strUsersST != "")
                                        strUsersST += ", ";
                                    strUsersST += oUser.GetFullName(intUserST) + "&nbsp;&nbsp;&nbsp;(" + (dsStorageT.Tables[0].Rows[0]["chk1"].ToString() == "1" ? "Complete" : "Incomplete") + ")";
                                }
                                strResults += "<td width=\"100%\">" + strUsersST + "</td></tr>";
                            }
                            else
                                strResults += "<tr><td nowrap>Storage (TEST):</td><td width=\"100%\">Pending Assignment</td></tr>";
                        }
                        else
                            strResults += "<tr><td nowrap>Storage (TEST):</td><td width=\"100%\">Not Submitted (" + (intStorage == 1 ? "Client <b>DID</b> Request TEST Storage" : "Client <b>DID NOT</b> Request TEST Storage") + ")</td></tr>";
                        DataSet dsStorageP = oOnDemandTasks.GetServerStorage(intAnswer, 1);
                        if (dsStorageP.Tables[0].Rows.Count > 0)
                        {
                            int intRequestSP = Int32.Parse(dsStorageP.Tables[0].Rows[0]["requestid"].ToString());
                            int intItemSP = Int32.Parse(dsStorageP.Tables[0].Rows[0]["itemid"].ToString());
                            int intNumberSP = Int32.Parse(dsStorageP.Tables[0].Rows[0]["number"].ToString());
                            DataSet dsResourceSP = oResourceRequest.Get(intRequestSP, intItemSP, intNumberSP);
                            if (dsResourceSP.Tables[0].Rows.Count > 0)
                            {
                                strResults += "<tr><td nowrap>Storage (PROD):</td>";
                                string strUsersSP = "";
                                foreach (DataRow drResourceSP in dsResourceSP.Tables[0].Rows)
                                {
                                    int intUserSP = Int32.Parse(drResourceSP["userid"].ToString());
                                    if (strUsersSP != "")
                                        strUsersSP += ", ";
                                    strUsersSP += oUser.GetFullName(intUserSP) + "&nbsp;&nbsp;&nbsp;(" + (dsStorageP.Tables[0].Rows[0]["chk1"].ToString() == "1" ? "Complete" : "Incomplete") + ")";
                                }
                                strResults += "<td width=\"100%\">" + strUsersSP + "</td></tr>";
                            }
                            else
                                strResults += "<tr><td nowrap>Storage (PROD):</td><td width=\"100%\">Pending Assignment</td></tr>";
                        }
                        else
                            strResults += "<tr><td nowrap>Storage (PROD):</td><td width=\"100%\">Not Submitted (" + (intStorage == 1 ? "Client <b>DID</b> Request PROD Storage" : "Client <b>DID NOT</b> Request PROD Storage") + ")</td></tr>";
                    }
                    else
                    {
                        strResults += "<tr><td nowrap>Storage (TEST):</td><td width=\"100%\">VMware Does Not Require Storage Configuration</td></tr>";
                        strResults += "<tr><td nowrap>Storage (PROD):</td><td width=\"100%\">VMware Does Not Require Storage Configuration</td></tr>";
                    }
                    strResults += "<tr><td nowrap>Model:</td><td width=\"100%\">" + oModelsProperties.Get(intModel, "name") + "</td></tr>";
                    strResults += "<tr><td nowrap>NickName:</td><td width=\"100%\">" + oForecast.GetAnswer(intAnswer, "name") + "</td></tr>";
                    strResults += "<tr><td nowrap>Current State:</td><td width=\"100%\">" + (ds.Tables[0].Rows[0]["step"].ToString() == "0" ? "Awaiting Execution" : (ds.Tables[0].Rows[0]["step"].ToString() == "999" ? "Completed Build" : "Building...")) + "</td></tr>";
                    strResults += "<tr><td nowrap colspan=\"2\"><img src=\"/images/icons/pdf.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"/frame/ondemand/pdf_tsm.aspx?id=" + intAnswer.ToString() + "\">Click here to view the TSM Registration Form</a></td></tr>";
                    strResults += "<tr><td nowrap colspan=\"2\"><img src=\"/images/icons/pdf.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"/frame/ondemand/pdf_san.aspx?id=" + intAnswer.ToString() + "\">Click here to view the SAN Registration Form (TEST)</a></td></tr>";
                    strResults += "<tr><td nowrap colspan=\"2\"><img src=\"/images/icons/pdf.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"/frame/ondemand/pdf_san.aspx?id=" + intAnswer.ToString() + "&prod=true\">Click here to view the SAN Registration Form (PROD)</a></td></tr>";
                    strResults += "<tr><td nowrap colspan=\"2\"><img src=\"/images/icons/html.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"/frame/ondemand/pdf_sc.aspx?id=" + intServer.ToString() + "\">Click here to view the Service Center Request</a></td></tr>";
                    strResults += "<tr><td nowrap colspan=\"2\"><img src=\"/images/icons/pdf.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"/frame/ondemand/pdf_birth.aspx?id=" + intAnswer.ToString() + "\">Click here to view the Birth Certificate</a></td></tr>";
                    strResults += "<tr><td nowrap colspan=\"2\"><img src=\"/images/file.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"javascript:void(0);\" onclick=\"OpenWindow('FORECAST_EQUIPMENT','?id=" + intAnswer.ToString() + "');\">Click here to view the design</a></td></tr>";
                    strResults += "<tr><td nowrap colspan=\"2\"><img src=\"/images/file.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"" + oPage.GetFullLink(intDesignBuilder) + "?id=" + oForecast.GetAnswer(intAnswer, "forecastid") + "&highlight=" + intAnswer.ToString() + "\" target=\"_blank\">Click here to view the OVERALL design</a></td></tr>";
                    int intType = oModelsProperties.GetType(intModel);
                    string strExecute = oType.Get(intType, "forecast_execution_path");
                    if (strExecute != "")
                    {
                        strResults += "<tr><td nowrap colspan=\"2\"><img src=\"/images/user.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"javascript:void(0);\" onclick=\"OpenWindow('NEW_WINDOW','" + strExecute + "?id=" + intAnswer.ToString() + "&sid=2&view=true');\">Click here to view the application configuration</a></td></tr>";
                        if (oForecast.IsOSDistributed(intAnswer) == true)
                        {
                            if (oForecast.IsHACluster(intAnswer) == true)
                                strResults += "<tr><td nowrap colspan=\"2\"><img src=\"/images/config.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"javascript:void(0);\" onclick=\"OpenWindow('NEW_WINDOW','" + strExecute + "?id=" + intAnswer.ToString() + "&sid=3&view=true');\">Click here to view the device configuration</a></td></tr>";
                            else
                                strResults += "<tr><td nowrap colspan=\"2\"><img src=\"/images/config.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"javascript:void(0);\" onclick=\"OpenWindow('NEW_WINDOW','" + strExecute + "?id=" + intAnswer.ToString() + "&sid=3&view=true');\">Click here to view the device configuration</a></td></tr>";
                            strResults += "<tr><td nowrap colspan=\"2\"><img src=\"/images/calendar.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"javascript:void(0);\" onclick=\"OpenWindow('NEW_WINDOW','" + strExecute + "?id=" + intAnswer.ToString() + "&sid=4&view=true');\">Click here to view the production go live date</a></td></tr>";
                        }
                        else
                        {
                            if (oForecast.IsHACluster(intAnswer) == true)
                                strResults += "<tr><td nowrap colspan=\"2\"><img src=\"/images/config.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"javascript:void(0);\" onclick=\"OpenWindow('NEW_WINDOW','" + strExecute + "?id=" + intAnswer.ToString() + "&sid=3&view=true');\">Click here to view the device configuration</a></td></tr>";
                            else
                                strResults += "<tr><td nowrap colspan=\"2\"><img src=\"/images/config.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"javascript:void(0);\" onclick=\"OpenWindow('NEW_WINDOW','" + strExecute + "?id=" + intAnswer.ToString() + "&sid=3&view=true');\">Click here to view the device configuration</a></td></tr>";
                            strResults += "<tr><td nowrap colspan=\"2\"><img src=\"/images/calendar.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"javascript:void(0);\" onclick=\"OpenWindow('NEW_WINDOW','" + strExecute + "?id=" + intAnswer.ToString() + "&sid=4&view=true');\">Click here to view the production go live date</a></td></tr>";
                        }
                        strResults += "<tr><td nowrap colspan=\"2\"><img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\"/> <a href=\"javascript:void(0);\" onclick=\"OpenWindow('FORECAST_EXECUTE','" + strExecute + "?id=" + intAnswer.ToString() + "');\">Click here to view the execution</a></td></tr>";
                    }
                    strResults += "<tr><td nowrap colspan=\"2\" class=\"header\"><b>Asset History:</b></td></tr>";
                    string strAssets = "";
                    DataSet dsAssets = oServer.GetAssets(intServer);
                    foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                        strAssets += "<tr><td>" + drAsset["serial"].ToString() + "</td><td>" + drAsset["asset"].ToString() + "</td><td>" + drAsset["model"].ToString() + "</td><td>" + drAsset["class"].ToString() + "</td><td>" + drAsset["environment"].ToString() + "</td><td>" + drAsset["datestamp"].ToString() + "</td></tr>";
                    if (strAssets == "")
                        strAssets += "<tr><td colspan=\"6\"><img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\"/> There are no assets</td></tr>";
                    strAssets = "<tr class=\"bold\"><td>Serial</td><td>Asset</td><td>Model</td><td>Class</td><td>Environment</td><td>DateStamp</td></tr>" + strAssets;
                    strAssets = "<table cellpadding=\"3\" cellspacing=\"2\" border=\"0\">" + strAssets + "</table>";
                    strResults += "<tr><td nowrap colspan=\"2\">" + strAssets + "</td></tr>";
                }
                else
                {
                    strResults += "<tr><td nowrap>Server Name Generation:</td><td width=\"100%\">Generated via Custom Functions, Server Names</td></tr>";
                    int intUser = Int32.Parse(ds.Tables[0].Rows[0]["servernameuserid"].ToString());
                    strResults += "<tr><td nowrap>Implementor:</td><td width=\"100%\">" + oUser.GetFullName(intUser) + " (" + oUser.GetName(intUser) + ")</td></tr>";
                    strResults += "<tr><td nowrap>Description:</td><td width=\"100%\">" + ds.Tables[0].Rows[0]["description"].ToString() + "</td></tr>";
                }
            }
            else
            {
                panMultiple.Visible = true;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    strMultiple += "<tr onmouseover=\"CellRowOver(this);\" onmouseout=\"CellRowOut(this);\" onclick=\"window.navigate('" + oPage.GetFullLink(intPage) + "?name=" + oFunctions.encryptQueryString(dr["servername"].ToString()) + "');\">";
                    int intAnswer = 0;
                    int intProject = 0;
                    if (dr["answerid"].ToString() != "")
                        intAnswer = Int32.Parse(dr["answerid"].ToString());
                    if (dr["projectid"].ToString() != "")
                        intProject = Int32.Parse(dr["projectid"].ToString());
                    strMultiple += "<td>" + dr["servername"].ToString() + "</td>";
                    if (intAnswer == 0 && intProject == 0)
                    {
                        int intImplementor = Int32.Parse(dr["servernameuserid"].ToString());
                        strMultiple += "<td>" + oUser.GetFullName(intImplementor) + " (" + oUser.GetName(intImplementor) + ")" + "</td>";
                        strMultiple += "<td>Custom Function</td>";
                    }
                    else if (intAnswer > 0 && intProject > 0)
                    {
                        strMultiple += "<td>" + GetImplementor(intProject, intAnswer) + "</td>";
                        strMultiple += "<td>Design Builder Execution</td>";
                    }
                    else
                    {
                        strMultiple += "<td>*** ERROR ***</td>";
                        strMultiple += "<td>???</td>";
                    }
                    strMultiple += "<td>" + dr["name"].ToString() + "</td>";
                    strMultiple += "<td>" + dr["number"].ToString() + "</td>";
                    strMultiple += "</tr>";
                }
            }
        }
        protected void btnSearch_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?name=" + oFunctions.encryptQueryString(txtName.Text));
        }
        private string GetImplementor(int intProject, int intAnswer)
        {
            string strImplementor = "";
            try
            {
                int intImplementor = 0;
                DataSet dsTasks = oOnDemandTasks.GetPending(intAnswer);
                if (dsTasks.Tables[0].Rows.Count > 0)
                {
                    intImplementor = Int32.Parse(dsTasks.Tables[0].Rows[0]["resourceid"].ToString());
                    intImplementor = Int32.Parse(oResourceRequest.GetWorkflow(intImplementor, "userid"));
                }
                else
                    intImplementor = -999;
                if (intImplementor > 0 || intImplementor == -999)
                    strImplementor = oUser.GetFullName(intImplementor) + " (" + oUser.GetName(intImplementor) + ")";
            }
            catch
            {
                strImplementor = "** ERROR **";
            }
            return strImplementor;
        }
    }
}