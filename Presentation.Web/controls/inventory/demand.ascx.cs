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
    public partial class demand : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Platforms oPlatform;
        protected Types oType;
        protected ModelsProperties oModelsProperties;
        protected ServiceRequests oServiceRequest;
        protected Forecast oForecast;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strDemand = "";
        protected int intMax = 50;
        protected string strFilter = "";
        protected int intGroup = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            int intPlatform = 0;
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intPlatform = Int32.Parse(Request.QueryString["id"]);
            if (intPlatform > 0)
            {
                intMax = Int32.Parse(oPlatform.Get(intPlatform, "max_inventory1"));
                if (!IsPostBack)
                {
                    LoadFilters();
                    LoadGroups(intPlatform);
                }
            }
        }
        private void LoadFilters()
        {
            strFilter += LoadFilter("pid", "projectid");
            strFilter += LoadFilter("lid", "addressid");
            strFilter += LoadFilter("xid", "confidenceid");
            strFilter += LoadFilter("cid", "classid");
            strFilter += LoadFilter("eid", "environmentid");
            if (Request.QueryString["start"] != null && Request.QueryString["start"] != "")
                strFilter += " AND (implementation >= '" + Request.QueryString["start"] + "')";
            if (Request.QueryString["end"] != null && Request.QueryString["end"] != "")
                strFilter += " AND (implementation <= '" + Request.QueryString["end"] + "')";
        }
        private string LoadFilter(string _query, string _name)
        {
            char[] strSplit = { ',' };
            string strQuery = Request.QueryString[_query];
            string strMiniFilter = "";
            string strReturn = "";
            if (strQuery != null && strQuery != "0")
            {
                string[] strQuerys;
                strQuerys = strQuery.Split(strSplit);
                for (int ii = 0; ii < strQuerys.Length; ii++)
                {
                    if (strQuerys[ii].Trim() != "")
                    {
                        if (strMiniFilter != "")
                            strMiniFilter += " OR ";
                        strMiniFilter += _name + " = " + strQuerys[ii].Trim();
                    }
                }
                strReturn += " AND (" + strMiniFilter + ")";
            }
            return strReturn;
        }
        private void LoadGroups(int _platformid)
        {
            DataSet dsDemand = oForecast.GetAnswersModel(_platformid);
            DataTable dtDemand = dsDemand.Tables[0];
            if (Request.QueryString["g1"] != null && Request.QueryString["g1"] != "" && Request.QueryString["g1"] != "0")
                strDemand += ShowGroup(_platformid, dtDemand, 1, "");
            else
                strDemand += LoadGroup(_platformid, dtDemand, "");
        }
        private string ShowGroup(int _platformid, DataTable _demand, int _group_num, string _existing_filters)
        {
            string strGroup = "";
            string strQuery = Request.QueryString["g" + _group_num.ToString()];
            _group_num = _group_num + 1;
            object oNext = Request.QueryString["g" + _group_num.ToString()];
            switch (strQuery)
            {
                case "pro":
                    string strProject = LoadFilter("pid", "projectid");
                    if (strProject.StartsWith(" AND ") == true)
                        strProject = strProject.Substring(5);
                    Projects oProject = new Projects(intProfile, dsn);
                    DataSet dsProjects = oProject.GetActive();
                    DataRow[] drProjects = dsProjects.Tables[0].Select(strProject);
                    foreach (DataRow drProject in drProjects)
                    {
                        int intID = Int32.Parse(drProject["projectid"].ToString());
                        intGroup++;
                        strGroup += "<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_" + intGroup.ToString() + "','divGroup_" + intGroup.ToString() + "');\"><img id=\"imgGroup_" + intGroup.ToString() + "\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">" + oProject.Get(intID, "name") + "</td></tr>";
                        if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                            strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + LoadGroup(_platformid, _demand, _existing_filters + " AND projectid = " + drProject["projectid"].ToString()) + "</td></tr>";
                        else
                            strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + ShowGroup(_platformid, _demand, _group_num, _existing_filters + " AND projectid = " + drProject["projectid"].ToString()) + "</td></tr>";
                    }
                    break;
                case "con":
                    string strConfidence = LoadFilter("xid", "id");
                    if (strConfidence.StartsWith(" AND ") == true)
                        strConfidence = strConfidence.Substring(5);
                    Confidence oConfidence = new Confidence(intProfile, dsn);
                    DataSet dsConfidences = oConfidence.Gets(1);
                    DataRow[] drConfidences = dsConfidences.Tables[0].Select(strConfidence);
                    foreach (DataRow drConfidence in drConfidences)
                    {
                        intGroup++;
                        strGroup += "<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_" + intGroup.ToString() + "','divGroup_" + intGroup.ToString() + "');\"><img id=\"imgGroup_" + intGroup.ToString() + "\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">" + drConfidence["name"].ToString() + "</td></tr>";
                        if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                            strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + LoadGroup(_platformid, _demand, _existing_filters + " AND confidenceid = " + drConfidence["id"].ToString()) + "</td></tr>";
                        else
                            strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + ShowGroup(_platformid, _demand, _group_num, _existing_filters + " AND confidenceid = " + drConfidence["id"].ToString()) + "</td></tr>";
                    }
                    break;
                case "loc":
                    string strLocation = LoadFilter("lid", "id");
                    if (strLocation.StartsWith(" AND ") == true)
                        strLocation = strLocation.Substring(5);
                    Locations oLocation = new Locations(intProfile, dsn);
                    DataSet dsLocations = oLocation.GetAddresss(1);
                    DataRow[] drLocations = dsLocations.Tables[0].Select(strLocation);
                    foreach (DataRow drLocation in drLocations)
                    {
                        intGroup++;
                        strGroup += "<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_" + intGroup.ToString() + "','divGroup_" + intGroup.ToString() + "');\"><img id=\"imgGroup_" + intGroup.ToString() + "\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">" + drLocation["fullname"].ToString() + "</td></tr>";
                        if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                            strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + LoadGroup(_platformid, _demand, _existing_filters + " AND addressid = " + drLocation["id"].ToString()) + "</td></tr>";
                        else
                            strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + ShowGroup(_platformid, _demand, _group_num, _existing_filters + " AND addressid = " + drLocation["id"].ToString()) + "</td></tr>";
                    }
                    break;
                case "cla":
                    string strClass = LoadFilter("cid", "id");
                    if (strClass.StartsWith(" AND ") == true)
                        strClass = strClass.Substring(5);
                    Classes oClass = new Classes(intProfile, dsn);
                    DataSet dsClasses = oClass.Gets(1);
                    DataRow[] drClasses = dsClasses.Tables[0].Select(strClass);
                    foreach (DataRow drClass in drClasses)
                    {
                        intGroup++;
                        strGroup += "<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_" + intGroup.ToString() + "','divGroup_" + intGroup.ToString() + "');\"><img id=\"imgGroup_" + intGroup.ToString() + "\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">" + drClass["name"].ToString() + "</td></tr>";
                        if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                            strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + LoadGroup(_platformid, _demand, _existing_filters + " AND classid = " + drClass["id"].ToString()) + "</td></tr>";
                        else
                            strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + ShowGroup(_platformid, _demand, _group_num, _existing_filters + " AND classid = " + drClass["id"].ToString()) + "</td></tr>";
                    }
                    break;
                case "env":
                    string strEnvironment = LoadFilter("eid", "id");
                    if (strEnvironment.StartsWith(" AND ") == true)
                        strEnvironment = strEnvironment.Substring(5);
                    Environments oEnvironment = new Environments(intProfile, dsn);
                    DataSet dsEnvironments = oEnvironment.Gets(1);
                    DataRow[] drEnvironments = dsEnvironments.Tables[0].Select(strEnvironment);
                    foreach (DataRow drEnvironment in drEnvironments)
                    {
                        intGroup++;
                        strGroup += "<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_" + intGroup.ToString() + "','divGroup_" + intGroup.ToString() + "');\"><img id=\"imgGroup_" + intGroup.ToString() + "\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">" + drEnvironment["name"].ToString() + "</td></tr>";
                        if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                            strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + LoadGroup(_platformid, _demand, _existing_filters + " AND environmentid = " + drEnvironment["id"].ToString()) + "</td></tr>";
                        else
                            strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + ShowGroup(_platformid, _demand, _group_num, _existing_filters + " AND environmentid = " + drEnvironment["id"].ToString()) + "</td></tr>";
                    }
                    break;
            }
            if (strGroup != "")
                strGroup = "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\">" + strGroup + "</table>";
            return strGroup;
        }
        private string LoadGroup(int _platformid, DataTable _demand, string _additional_filter)
        {
            string strGroup = "";
            DataSet dsTypes = oType.Gets(_platformid, 1);
            foreach (DataRow drType in dsTypes.Tables[0].Rows)
            {
                bool boolPhysical = false;
                bool boolBlade = false;
                bool boolOther = false;
                int intOldHost = -1;
                DataSet dsModels = oModelsProperties.GetTypes(1, Int32.Parse(drType["id"].ToString()), 1);
                foreach (DataRow drModel in dsModels.Tables[0].Rows)
                {
                    boolOther = !boolOther;
                    int intBlade = Int32.Parse(drModel["blade"].ToString());
                    int intHost = -1;
                    if (drModel["hostid"].ToString() != "")
                        intHost = Int32.Parse(drModel["hostid"].ToString());
                    string strType = "Unknown";
                    if (intBlade == 1)
                    {
                        strType = "Blade";
                        if (boolBlade == false)
                        {
                            if (strGroup != "")
                                strGroup += "<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>";
                            strGroup += "<tr><td colspan=\"4\" class=\"bold\">" + drType["name"].ToString() + " | " + strType + "</td></tr>";
                            boolBlade = true;
                        }
                    }
                    else if (intHost == 0)
                    {
                        strType = "Physical";
                        if (boolPhysical == false)
                        {
                            if (strGroup != "")
                                strGroup += "<tr><td colspan=\"4\"><p>&nbsp;</p></td></tr>";
                            strGroup += "<tr><td colspan=\"4\" class=\"bold\">" + drType["name"].ToString() + " | " + strType + "</td></tr>";
                            boolPhysical = true;
                        }
                    }
                    else if (intOldHost != intHost)
                    {
                        strType = drModel["host"].ToString();
                        if (intHost > 0)
                        {
                            if (strGroup != "")
                                strGroup += "<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>";
                            strGroup += "<tr><td colspan=\"4\" class=\"bold\">" + drType["name"].ToString() + " | " + strType + "</td></tr>";
                            intOldHost = intHost;
                        }
                    }
                    int intModel = Int32.Parse(drModel["id"].ToString());
                    DataRow[] drModels = _demand.Select("model = " + intModel.ToString() + strFilter + _additional_filter);
                    int intDemand = 0;
                    foreach (DataRow dr in drModels)
                    {
                        intDemand += Int32.Parse(dr["quantity"].ToString());
                        intDemand += Int32.Parse(dr["recovery_number"].ToString());
                    }
                    strGroup += "<tr" + (boolOther ? " bgcolor=\"F6F6F6\"" : "") + " class=\"" + (intDemand > 0 ? "greendefault" : (intDemand < 0 ? "reddefault" : "default")) + "\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>" + drModel["name"].ToString() + ":</td><td width=\"100%\">" + oServiceRequest.GetStatusBarFill(((double.Parse(intDemand.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300") + "</td><td nowrap>" + intDemand.ToString() + "</td></tr>";
                }
            }
            if (strGroup != "")
                strGroup = "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\">" + strGroup + "</table>";
            return strGroup;
        }
        protected DataTable SelectDistinct(string TableName, DataTable SourceTable, string FieldName)
        {
            DataTable dt = new DataTable(TableName);
            dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);

            object LastValue = null;
            foreach (DataRow dr in SourceTable.Select("", FieldName))
            {
                if (LastValue == null || !(ColumnEqual(LastValue, dr[FieldName])))
                {
                    LastValue = dr[FieldName];
                    dt.Rows.Add(new object[] { LastValue });
                }
            }
            return dt;
        }
        private bool ColumnEqual(object A, object B)
        {
            if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
                return true;
            if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
                return false;
            return (A.Equals(B));  // value type standard comparison
        }
    }
}