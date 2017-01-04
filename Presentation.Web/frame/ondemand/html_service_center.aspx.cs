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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class html_service_center : BasePage
    {
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;

        protected string strEmail = ConfigurationManager.AppSettings["TSM_MAILBOX"];
        protected bool boolUsePNCNaming = (ConfigurationManager.AppSettings["USE_PNC_NAMING"] == "1");
        protected string strTable = "";
        private Variables oVariables;

        protected void Page_Load(object sender, EventArgs e)
        {
            Servers oServer = new Servers(0, dsn);
            VMWare oVMWare = new VMWare(0, dsn);
            Forecast oForecast = new Forecast(0, dsn);
            Environments oEnvironment = new Environments(0, dsn);
            OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);
            Classes oClass = new Classes(0, dsn);
            Requests oRequest = new Requests(0, dsn);
            Projects oProject = new Projects(0, dsn);
            ServerName oServerName = new ServerName(0, dsn);
            Locations oLocations = new Locations(0, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            ServicePacks oServicePacks = new ServicePacks(0, dsn);
            Users oUser = new Users(0, dsn);
            Organizations oOrganization = new Organizations(0, dsn);
            IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
            Domains oDomains = new Domains(0, dsn);
            oVariables = new Variables(intEnvironment);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                StringBuilder sbTable = new StringBuilder(strTable);
                int intServer = Int32.Parse(Request.QueryString["id"]);
                DataSet ds = oServer.Get(intServer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                    int intName = Int32.Parse(ds.Tables[0].Rows[0]["nameid"].ToString());
                    string strName = oServer.GetName(intServer, boolUsePNCNaming);
                    int intForecast = Int32.Parse(oForecast.GetAnswer(intAnswer, "forecastid"));
                    int intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                    int intProject = oRequest.GetProjectNumber(intRequest);
                    int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
                    int intEnv = Int32.Parse(oForecast.GetAnswer(intAnswer, "environmentid"));
                    int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());

                    string _save_location = oVariables.UploadsFolder() + "SC\\";
                    string strFileName = "SC_" + intServer.ToString() + "_" + oProject.Get(intProject, "number") + ".HTM";
                    if (Directory.Exists(_save_location) == false)
                        Directory.CreateDirectory(_save_location);
                    string strFile = _save_location + strFileName;
                    StreamWriter fp = File.CreateText(strFile);


                    string strDefault = "<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" class=\"default\">";

                    StringBuilder sbLeft1 = new StringBuilder();
                    string strLead = oProject.Get(intProject, "lead");
                    int intLead = 0;
                    if (strLead != "")
                    {
                        intLead = Int32.Parse(strLead);
                    }
                    sbLeft1.Append("<fieldset>");
                    sbLeft1.Append("<legend><b>Who is this for?</b></legend>");
                    sbLeft1.Append(strDefault);
                    sbLeft1.Append("<tr>");
                    sbLeft1.Append("<td>Client XID:</td>");
                    sbLeft1.Append("<td>");
                    sbLeft1.Append(GetBox(intLead > 0 ? oUser.GetName(intLead) : "***ERROR**", 200));
                    sbLeft1.Append("</td>");
                    sbLeft1.Append("</tr>");
                    sbLeft1.Append("<tr>");
                    sbLeft1.Append("<td>Client Name:</td>");
                    sbLeft1.Append("<td>");
                    sbLeft1.Append(GetBox(intLead > 0 ? oUser.GetFullName(intLead) : "***ERROR**", 200));
                    sbLeft1.Append("</td>");
                    sbLeft1.Append("</tr>");
                    sbLeft1.Append("<tr>");
                    sbLeft1.Append("<td>Client Phone No.:</td>");
                    sbLeft1.Append("<td>");
                    sbLeft1.Append(GetBox(intLead > 0 ? oUser.Get(intLead, "phone") : "***ERROR**", 200));
                    sbLeft1.Append("</td>");
                    sbLeft1.Append("</tr>");
                    sbLeft1.Append("<tr>");
                    sbLeft1.Append("<td>Client Department:</td>");
                    sbLeft1.Append("<td>");
                    sbLeft1.Append(GetBox("", 200));
                    sbLeft1.Append("</td>");
                    sbLeft1.Append("</tr>");
                    sbLeft1.Append("<tr>");
                    sbLeft1.Append("<td>Client Fax:</td>");
                    sbLeft1.Append("<td>");
                    sbLeft1.Append(GetBox("", 200));
                    sbLeft1.Append("</td>");
                    sbLeft1.Append("</tr>");
                    sbLeft1.Append("<tr>");
                    sbLeft1.Append("<td>Client Cost Center:</td>");
                    sbLeft1.Append("<td>");
                    sbLeft1.Append(GetBox("", 200));
                    sbLeft1.Append("</td>");
                    sbLeft1.Append("</tr>");
                    sbLeft1.Append("</table>");
                    sbLeft1.Append("</fieldset>");

                    StringBuilder sbRight1 = new StringBuilder();
                    string strIE = oProject.Get(intProject, "engineer");
                    int intIE = 0;
                    if (strIE != "")
                    {
                        intIE = Int32.Parse(strIE);
                    }
                    sbRight1.Append("<fieldset>");
                    sbRight1.Append("<legend><b>Requestor Information</b></legend>");
                    sbRight1.Append(strDefault);
                    sbRight1.Append("<tr>");
                    sbRight1.Append("<td>Requested By Name:</td>");
                    sbRight1.Append("<td>");
                    sbRight1.Append(GetBox(intIE > 0 ? oUser.GetFullName(intIE) : "***ERROR**", 200));
                    sbRight1.Append("</td>");
                    sbRight1.Append("</tr>");
                    sbRight1.Append("<tr>");
                    sbRight1.Append("<td>Requested By Phone No.:</td>");
                    sbRight1.Append("<td>");
                    sbRight1.Append(GetBox(intIE > 0 ? oUser.Get(intIE, "phone") : "***ERROR**", 200));
                    sbRight1.Append("</td>");
                    sbRight1.Append("</tr>");
                    sbRight1.Append("<tr>");
                    sbRight1.Append("<td>Requested By Email:</td>");
                    sbRight1.Append("<td>");
                    sbRight1.Append(GetBox(intIE > 0 ? oUser.GetEmail(oUser.GetName(intIE), intEnvironment) : "***ERROR**", 200));
                    sbRight1.Append("</td>");
                    sbRight1.Append("</tr>");
                    sbRight1.Append("</table>");
                    sbRight1.Append("</fieldset>");
                    sbRight1.Append("<fieldset>");
                    sbRight1.Append("<legend><b>Location Information:</b></legend>");
                    sbRight1.Append(strDefault);
                    sbRight1.Append("<tr>");
                    sbRight1.Append("<td>Location:</td>");
                    sbRight1.Append("<td>");
                    sbRight1.Append(GetBox("", 200));
                    sbRight1.Append("</td>");
                    sbRight1.Append("</tr>");
                    sbRight1.Append("<tr>");
                    sbRight1.Append("<td>Location Full Name:</td>");
                    sbRight1.Append("<td>");
                    sbRight1.Append(GetBox("", 200));
                    sbRight1.Append("</td>");
                    sbRight1.Append("</tr>");
                    sbRight1.Append("</table>");
                    sbRight1.Append("</fieldset>");

                    StringBuilder sbCenter1 = new StringBuilder();
                    sbCenter1.Append("<fieldset>");
                    sbCenter1.Append("<legend><b>Brief Desc:</b></legend>");
                    sbCenter1.Append(strDefault);
                    sbCenter1.Append("<tr>");
                    sbCenter1.Append("<td>");
                    sbCenter1.Append(GetBox(oForecast.GetAnswer(intAnswer, "name") + " (" + strName + ")", 600));
                    sbCenter1.Append("</td>");
                    sbCenter1.Append("</tr>");
                    sbCenter1.Append("</table>");
                    sbCenter1.Append("</fieldset>");

                    StringBuilder sbLeft2 = new StringBuilder();
                    sbLeft2.Append("<fieldset>");
                    sbLeft2.Append("<legend><b>Project Information:</b></legend>");
                    sbLeft2.Append(strDefault);
                    sbLeft2.Append("<tr>");
                    sbLeft2.Append("<td>Project ID:</td>");
                    sbLeft2.Append("<td>");
                    sbLeft2.Append(GetBox(oProject.Get(intProject, "number"), 200));
                    sbLeft2.Append("</td>");
                    sbLeft2.Append("<td>Project Manager XID:</td>");
                    sbLeft2.Append("<td>");
                    sbLeft2.Append(GetBox(intLead > 0 ? oUser.GetName(intLead) : "***ERROR**", 200));
                    sbLeft2.Append("</td>");
                    sbLeft2.Append("</tr>");
                    sbLeft2.Append("<tr>");
                    sbLeft2.Append("<td>Project Name:</td>");
                    sbLeft2.Append("<td>");
                    sbLeft2.Append(GetBox(oProject.Get(intProject, "name"), 200));
                    sbLeft2.Append("</td>");
                    sbLeft2.Append("<td>Project Manager Name:</td>");
                    sbLeft2.Append("<td>");
                    sbLeft2.Append(GetBox(intLead > 0 ? oUser.GetFullName(intLead) : "***ERROR**", 200));
                    sbLeft2.Append("</td>");
                    sbLeft2.Append("</tr>");
                    sbLeft2.Append("<tr>");
                    sbLeft2.Append("<td>Project Budgeted:</td>");
                    sbLeft2.Append("<td>");
                    sbLeft2.Append(GetBox(oProject.Get(intProject, "bd"), 200));
                    sbLeft2.Append("</td>");
                    sbLeft2.Append("<td>Project Manager Phone:</td>");
                    sbLeft2.Append("<td>");
                    sbLeft2.Append(GetBox(intLead > 0 ? oUser.Get(intLead, "phone") : "***ERROR**", 200));
                    sbLeft2.Append("</td>");
                    sbLeft2.Append("</tr>");
                    sbLeft2.Append("<tr>");
                    sbLeft2.Append("<td>Project Cost Ctr:</td>");
                    sbLeft2.Append("<td>");
                    sbLeft2.Append(GetBox("", 200));
                    sbLeft2.Append("</td>");
                    sbLeft2.Append("<td>Project Manager Email:</td>");
                    sbLeft2.Append("<td>");
                    sbLeft2.Append(GetBox(intLead > 0 ? oUser.GetEmail(oUser.GetName(intLead), intEnvironment) : "***ERROR**", 200));
                    sbLeft2.Append("</td>");
                    sbLeft2.Append("</tr>");
                    sbLeft2.Append("</table>");
                    sbLeft2.Append("</fieldset>");

                    OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
                    int intImplementor = 0;
                    DataSet dsTasks = oOnDemandTasks.GetPending(intAnswer);
                    if (dsTasks.Tables[0].Rows.Count > 0)
                    {
                        intImplementor = Int32.Parse(dsTasks.Tables[0].Rows[0]["resourceid"].ToString());
                        intImplementor = Int32.Parse(oResourceRequest.GetWorkflow(intImplementor, "userid"));
                    }
                    else
                        intImplementor = -999;
                    sbTable = new StringBuilder("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" class=\"default\">");
                    sbTable.Append("<tr>");
                    sbTable.Append("<td colspan=\"2\"><b>NOTE:</b> ASSIGN THIS REQUEST TO: ");
                    sbTable.Append(intImplementor > 0 || intImplementor == -999 ? oUser.GetFullName(intImplementor) + " (" + oUser.GetName(intImplementor) + ")" : "***ERROR**");
                    sbTable.Append("</td>");
                    sbTable.Append("</tr>");
                    sbTable.Append("<tr>");
                    sbTable.Append("<td>");
                    sbTable.Append(sbLeft1.ToString());
                    sbTable.Append("</td>");
                    sbTable.Append("<td>");
                    sbTable.Append(sbRight1.ToString());
                    sbTable.Append("</td>");
                    sbTable.Append("</tr>");
                    sbTable.Append("<tr>");
                    sbTable.Append("<td colspan=\"2\">");
                    sbTable.Append(sbCenter1.ToString());
                    sbTable.Append("</td>");
                    sbTable.Append("</tr>");
                    sbTable.Append("<tr>");
                    sbTable.Append("<td colspan=\"2\">");
                    sbTable.Append(sbLeft2.ToString());
                    sbTable.Append("</td>");
                    sbTable.Append("</tr>");
                    sbTable.Append("<tr>");
                    sbTable.Append("<td colspan=\"2\"><p>&nbsp;</p></td>");
                    sbTable.Append("</tr>");

                    bool boolVirtual = false;
                    if (boolVirtual == true)
                    {
                        sbTable.Append("<tr>");
                        sbTable.Append("<td colspan=\"2\" class=\"header\">REQUISITION VIRTUAL SERVER GUEST</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Requested Server Completion Date:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oForecast.GetAnswer(intAnswer, "implementation"), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Server Hardware Specifications:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("VIRTUAL SERVER GUEST VMWARE", 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Server Operating System:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oOperatingSystem.Get(Int32.Parse(ds.Tables[0].Rows[0]["osid"].ToString()), "name").ToUpper() + " (" + oServicePacks.Get(Int32.Parse(ds.Tables[0].Rows[0]["spid"].ToString()), "name").ToUpper() + ")", 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Environment:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oEnvironment.Get(Int32.Parse(oForecast.GetAnswer(intAnswer, "environmentid")), "name").ToUpper(), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Destination Class:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oClass.Get(Int32.Parse(oForecast.GetAnswer(intAnswer, "classid")), "name").ToUpper(), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr><td colspan=\"2\"><p>&nbsp;</p></td></tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Destination Domain:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oDomains.Get(Int32.Parse(ds.Tables[0].Rows[0]["domainid"].ToString()), "name").ToUpper(), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Backup Method:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("TSM", 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        string strHost = "***ERROR***";
                        DataSet dsGuest = oVMWare.GetGuest(strName);
                        if (dsGuest.Tables[0].Rows.Count > 0)
                        {
                            strHost = oVMWare.GetHost(Int32.Parse(dsGuest.Tables[0].Rows[0]["hostid"].ToString()), "name").ToUpper();
                        }
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Host Server Name:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(strHost, 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr><td colspan=\"2\"><p>&nbsp;</p></td></tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td bgcolor=\"#CCCCCC\" style=\"border:2px outset #FFFFFF\">Network Protocols</td>");
                        sbTable.Append("<td bgcolor=\"#CCCCCC\" style=\"border:2px outset #FFFFFF\">Server Software - click in box below to add</td>");
                        sbTable.Append("</tr>");
                        bool boolSQL = false;
                        bool boolIIS = false;
                        DataSet dsComp = oServerName.GetComponentDetailSelected(intServer, 1);
                        foreach (DataRow drComp in dsComp.Tables[0].Rows)
                        {
                            if (drComp["code"].ToString() == "IIS")
                            {
                                boolIIS = true;
                            }
                            if (drComp["code"].ToString() == "SQL")
                            {
                                boolSQL = true;
                            }
                        }
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("TCP/IP", 250));
                        sbTable.Append("</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox((boolSQL ? "SQL" : ""), 350));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("", 250));
                        sbTable.Append("</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox((boolIIS ? "IIS" : ""), 350));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr><td colspan=\"2\"><p>&nbsp;</p></td></tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Attached to SAN?:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("NO", 150));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Server is Clustered:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("NO", 150));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Server Load Balanced:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("NO", 150));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Maximum Allowable Downtime:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("Not Applicable", 150) + "</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td colspan=\"2\"><input type=\"checkbox\" class=\"default\"/> Hardware Refresh</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr><td colspan=\"2\"><p>&nbsp;</p></td></tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td colspan=\"2\">Intended Use Description:</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td colspan=\"2\">");
                        sbTable.Append(GetBox("", 500));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                    }
                    else
                    {
                        sbTable.Append("<tr>");
                        sbTable.Append("<td colspan=\"2\" class=\"header\">REQUISITION SERVER</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Requested Server Completion Date:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oForecast.GetAnswer(intAnswer, "implementation"), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Server Hardware Specifications:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oModelsProperties.Get(intModel, "name"), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td colspan=\"2\"><input type=\"checkbox\" class=\"default\" checked/> User re-deployable hardware if applicable</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>PO:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("", 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Server Operating System:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oOperatingSystem.Get(Int32.Parse(ds.Tables[0].Rows[0]["osid"].ToString()), "name").ToUpper() + " (" + oServicePacks.Get(Int32.Parse(ds.Tables[0].Rows[0]["spid"].ToString()), "name").ToUpper() + ")", 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Environment:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oEnvironment.Get(Int32.Parse(oForecast.GetAnswer(intAnswer, "environmentid")), "name").ToUpper(), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Destination Class:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oClass.Get(Int32.Parse(oForecast.GetAnswer(intAnswer, "classid")), "name").ToUpper(), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr><td colspan=\"2\"><p>&nbsp;</p></td></tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Destination Domain:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oDomains.Get(Int32.Parse(ds.Tables[0].Rows[0]["domainid"].ToString()), "name").ToUpper(), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Server Location:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oLocations.GetFull(Int32.Parse(oForecast.GetAnswer(intAnswer, "addressid"))).ToUpper(), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Backup Method:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("TSM", 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr><td colspan=\"2\"><p>&nbsp;</p></td></tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td bgcolor=\"#CCCCCC\" style=\"border:2px outset #FFFFFF\">Network Protocols</td>");
                        sbTable.Append("<td bgcolor=\"#CCCCCC\" style=\"border:2px outset #FFFFFF\">Server Software - click in box below to add</td>");
                        sbTable.Append("</tr>");
                        bool boolSQL = false;
                        bool boolIIS = false;
                        DataSet dsComp = oServerName.GetComponentDetailSelected(intServer, 1);
                        foreach (DataRow drComp in dsComp.Tables[0].Rows)
                        {
                            if (drComp["code"].ToString() == "IIS")
                            {
                                boolIIS = true;
                            }
                            if (drComp["code"].ToString() == "SQL")
                            {
                                boolSQL = true;
                            }
                        }
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("TCP/IP", 250));
                        sbTable.Append("</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox((boolSQL ? "SQL" : ""), 350));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("", 250));
                        sbTable.Append("</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox((boolIIS ? "IIS" : ""), 350));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr><td colspan=\"2\"><p>&nbsp;</p></td></tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Attached to SAN?:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("NO", 150));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Server is Clustered:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("NO", 150));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Server Load Balanced:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("NO", 150));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Maximum Allowable Downtime:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("Not Applicable", 150));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td colspan=\"2\"><input type=\"checkbox\" class=\"default\"/> Hardware Refresh</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr><td colspan=\"2\"><p>&nbsp;</p></td></tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td colspan=\"2\">Intended Use Description:</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td colspan=\"2\">");
                        sbTable.Append(GetBox("", 500));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                    }
                    sbTable.Append("</table>");

                    fp.WriteLine("<html>");
                    fp.WriteLine("<head>");
                    fp.WriteLine("<title>ClearView | Service Center Request Form</title>");
                    fp.WriteLine("<style type=\"text/css\">");
                    fp.WriteLine(".default {font-family: Verdana, Arial, Helvetica, sans-serif;font-size: 10px;}");
                    fp.WriteLine(".header {font-family: Verdana, Arial, Helvetica, sans-serif;font-size: 16px;font-style: italic;font-weight: bold;}");
                    fp.WriteLine("</style>");
                    fp.WriteLine("<body leftmargin=\"0\" topmargin=\"0\">");
                    fp.WriteLine(sbTable.ToString());
                    fp.WriteLine("</body>");
                    fp.WriteLine("</html>");
                    fp.Close();

                }

                strTable = sbTable.ToString();
            }
        }
        private string GetBox(string _text, int _width)
        {
            return "<input type=\"text\" class=\"default\" style=\"width:" + _width.ToString() + "px\" value=\"" + _text + "\" />";
        }
    }
}
