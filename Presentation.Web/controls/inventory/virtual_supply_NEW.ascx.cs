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
    public partial class virtual_supply_NEW : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Platforms oPlatform;
        protected Types oType;
        protected ModelsProperties oModelsProperties;
        protected Asset oAsset;
        protected ServiceRequests oServiceRequest;
        protected Pages oPage;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strSupply = "";
        protected int intMax = 50;
        protected string strFilter = "";
        protected string strParameters = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
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
                    LoadLists();
                    LoadFilters();
                    LoadGroups(intPlatform);
                }
                btnClasses.Attributes.Add("onclick", "return MakeWider(this, '" + lstClasses.ClientID + "');");
                btnClassesClear.Attributes.Add("onclick", "return ClearList('" + lstClasses.ClientID + "');");
                btnEnvironments.Attributes.Add("onclick", "return MakeWider(this, '" + lstEnvironments.ClientID + "');");
                btnEnvironmentsClear.Attributes.Add("onclick", "return ClearList('" + lstEnvironments.ClientID + "');");
                btnLocations.Attributes.Add("onclick", "return MakeWider(this, '" + lstLocations.ClientID + "');");
                btnLocationsClear.Attributes.Add("onclick", "return ClearList('" + lstLocations.ClientID + "');");
                lstClasses.Attributes.Add("onchange", "PopulateEnvironmentsList('" + lstClasses.ClientID + "','" + lstEnvironments.ClientID + "',0);");
                lstEnvironments.Attributes.Add("onchange", "UpdateListHidden('" + lstEnvironments.ClientID + "','" + hdnEnvironment.ClientID + "');");
            }
        }
        public void LoadLists()
        {
            Locations oLocation = new Locations(intProfile, dsn);
            lstLocations.DataValueField = "id";
            lstLocations.DataTextField = "fullname";
            lstLocations.DataSource = oLocation.GetAddresssOrdered(1);
            lstLocations.DataBind();
            lstLocations.Items.Insert(0, new ListItem("-- ALL --", "0"));
            LoadList("lid", "Location(s)", lstLocations, null);
            Classes oClass = new Classes(intProfile, dsn);
            DataSet dsClasses = oClass.Gets(1);
            lstClasses.DataValueField = "id";
            lstClasses.DataTextField = "name";
            lstClasses.DataSource = dsClasses;
            lstClasses.DataBind();
            lstClasses.Items.Insert(0, new ListItem("-- ALL --", "0"));
            LoadList("cid", "Class(es)", lstClasses, null);
            if (Request.QueryString["cid"] != null)
            {
                string strValue = Request.QueryString["cid"];
                if (strValue == "0")
                {
                    foreach (DataRow drClass in dsClasses.Tables[0].Rows)
                        strValue += drClass["id"].ToString() + ",";
                }
                string[] strValues;
                char[] strSplit = { ',' };
                strValues = strValue.Split(strSplit);
                Functions oFunction = new Functions(intProfile, dsn, intEnvironment);
                strValue = oFunction.BuildXmlString("data", strValues);
                lstEnvironments.DataValueField = "id";
                lstEnvironments.DataTextField = "name";
                lstEnvironments.DataSource = oClass.GetEnvironments(strValue, 0);
                lstEnvironments.DataBind();
                lstEnvironments.Items.Insert(0, new ListItem("-- ALL --", "0"));
                LoadList("eid", "Environment(s)", lstEnvironments, hdnEnvironment);
            }
            else
            {
                lstEnvironments.Items.Insert(0, new ListItem("-- Please select a Class --", "0"));
                lstEnvironments.Enabled = false;
            }
            if (strParameters != "")
            {
                strParameters = "<table cellpadding=\"3\" cellspacing=\"1\" border=\"0\">" + strParameters + "</table>";
                panParameters.Visible = true;
            }
        }
        private void LoadList(string _query, string _name, ListBox _box, HiddenField _hidden)
        {
            char[] strSplit = { ',' };
            string strQuery = Request.QueryString[_query];
            if (strQuery != null)
            {
                StringBuilder sb = new StringBuilder(strParameters);
                bool boolAll = (strQuery == "0");
                if (boolAll == false)
                {
                    sb.Append("<tr><td><b>");
                    sb.Append(_name);
                    sb.Append(":</b></td></tr>");
                }
                string[] strQuerys;
                strQuerys = strQuery.Split(strSplit);
                for (int ii = 0; ii < strQuerys.Length; ii++)
                {
                    if (strQuerys[ii].Trim() != "")
                    {
                        foreach (ListItem oList in _box.Items)
                        {
                            if (oList.Value == strQuerys[ii].Trim())
                            {
                                if (boolAll == false)
                                {
                                    sb.Append("<tr><td>&nbsp;&nbsp;&nbsp;&quot;");
                                    sb.Append(oList.Text);
                                    sb.Append("&quot;</td></tr>");
                                }

                                oList.Selected = true;

                                if (_hidden != null)
                                    _hidden.Value += oList.Value + ";";
                                break;
                            }
                        }
                    }
                }

                strParameters = sb.ToString();
            }
        }
        protected void btnGo_Click(Object Sender, EventArgs e)
        {
            Filter();
        }
        private void Filter()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ListItem oList in lstLocations.Items)
            {
                if (oList.Selected == true)
                {
                    sb.Append("&lid=");
                    sb.Append(oList.Value);
                }
            }
            foreach (ListItem oList in lstClasses.Items)
            {
                if (oList.Selected == true)
                {
                    sb.Append("&cid=");
                    sb.Append(oList.Value);
                }
            }
            string strEnvironment = Request.Form[hdnEnvironment.UniqueID];
            string[] strEnvironments;
            char[] strSplit = { ';' };
            strEnvironments = strEnvironment.Split(strSplit);
            for (int ii = 0; ii < strEnvironments.Length; ii++)
            {
                if (strEnvironments[ii].Trim() != "")
                {
                    sb.Append("&eid=");
                    sb.Append(strEnvironments[ii].Trim());
                }
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=S" + sb.ToString());
        }
        private void LoadFilters()
        {
            strFilter += LoadFilter("lid", "addressid");
            strFilter += LoadFilter("cid", "classid");
            strFilter += LoadFilter("eid", "environmentid");
        }
        private string LoadFilter(string _query, string _name)
        {
            char[] strSplit = { ',' };
            string strQuery = Request.QueryString[_query];
            StringBuilder sb = new StringBuilder();
            string strReturn = "";
            if (strQuery != null && strQuery != "0")
            {
                string[] strQuerys;
                strQuerys = strQuery.Split(strSplit);
                for (int ii = 0; ii < strQuerys.Length; ii++)
                {
                    if (strQuerys[ii].Trim() != "")
                    {
                        if (sb.ToString() != "")
                        {
                            sb.Append(" OR ");
                        }
                        sb.Append(_name);
                        sb.Append(" = ");
                        sb.Append(strQuerys[ii].Trim());
                    }
                }
                strReturn += " AND (" + sb.ToString() + ")";
            }
            return strReturn;
        }
        private void LoadGroups(int _platformid)
        {
            DataSet dsSupply = oType.Gets(_platformid, 1);
            DataTable dtSupply = dsSupply.Tables[0];
            LoadGroup(_platformid, dtSupply, "");
        }
        private void LoadGroup(int _platformid, DataTable _demand, string _additional_filter)
        {
            foreach (DataRow drType in _demand.Rows)
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
                            if (strSupply != "")
                                strSupply += "<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>";
                            strSupply += "<tr><td colspan=\"4\" class=\"bold\">" + strType + "</td></tr>";
                            boolBlade = true;
                        }
                    }
                    else if (intHost == 0)
                    {
                        strType = "Physical";
                        if (boolPhysical == false)
                        {
                            if (strSupply != "")
                                strSupply += "<tr><td colspan=\"4\"><p>&nbsp;</p><p>&nbsp;</p></td></tr>";
                            strSupply += "<tr><td colspan=\"4\" class=\"header\">" + drType["name"].ToString() + "</td></tr>";
                            strSupply += "<tr><td colspan=\"4\" class=\"bold\">" + strType + "</td></tr>";
                            boolPhysical = true;
                        }
                    }
                    else if (intOldHost != intHost)
                    {
                        strType = drModel["host"].ToString();
                        if (intHost > 0)
                        {
                            if (strSupply != "")
                                strSupply += "<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>";
                            strSupply += "<tr><td colspan=\"4\" class=\"bold\">" + strType + "</td></tr>";
                            intOldHost = intHost;
                        }
                    }
                    int intModel = Int32.Parse(drModel["id"].ToString());
                    // Supply
                    DataSet dsModel = oAsset.GetCount(intModel, (int)AssetStatus.Available);
                    DataRow[] drModels = dsModel.Tables[0].Select("true " + strFilter + _additional_filter);
                    if (intHost == 0)
                    {
                        int intSupply = drModels.Length;
                        strSupply += "<tr" + (boolOther ? " bgcolor=\"F6F6F6\"" : "") + "><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>" + (intSupply == 0 ? drModel["name"].ToString() : "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('INVENTORY_SUPPLY','?model=" + drModel["id"].ToString() + "&status=1');\">" + drModel["name"].ToString() + "</a>") + ":</td><td width=\"100%\">" + oServiceRequest.GetStatusBarFill(((double.Parse(intSupply.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#6699CC") + "</td><td nowrap>" + intSupply.ToString() + "</td></tr>";
                    }
                    else
                    {
                        DataSet dsHosts = oAsset.GetVMWareHosts(intHost);
                        double dblRAM = 0.0;
                        double dblProcessor = 0.0;
                        double dblDiskSpace = 0.0;
                        foreach (DataRow drHost in dsHosts.Tables[0].Rows)
                        {
                            dblRAM += double.Parse(drHost["ram"].ToString());
                            dblProcessor += double.Parse(drHost["cpu_count"].ToString());
                        }
                        DataSet dsHostStorage = oAsset.GetVMWareHostsStorage(intHost);
                        foreach (DataRow drHostStorage in dsHostStorage.Tables[0].Rows)
                        {
                            dblDiskSpace += double.Parse(drHostStorage["l_actual_size"].ToString());
                            dblDiskSpace += double.Parse(drHostStorage["l_actual_size_qa"].ToString());
                            dblDiskSpace += double.Parse(drHostStorage["l_actual_size_test"].ToString());
                            dblDiskSpace += double.Parse(drHostStorage["m_actual_size"].ToString());
                            dblDiskSpace += double.Parse(drHostStorage["m_actual_size_qa"].ToString());
                            dblDiskSpace += double.Parse(drHostStorage["m_actual_size_test"].ToString());
                        }
                        strSupply += "<tr" + (boolOther ? " bgcolor=\"F6F6F6\"" : "") + " class=\"" + (dblRAM > 0.00 ? "greendefault" : (dblRAM < 0.00 ? "reddefault" : "default")) + "\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>RAM:</td><td width=\"100%\">" + oServiceRequest.GetStatusBarFill(((dblRAM / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300") + "</td><td nowrap>" + dblRAM.ToString() + " GB</td></tr>";
                        strSupply += "<tr" + (boolOther ? " bgcolor=\"F6F6F6\"" : "") + " class=\"" + (dblProcessor > 0.00 ? "greendefault" : (dblProcessor < 0.00 ? "reddefault" : "default")) + "\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>Processors:</td><td width=\"100%\">" + oServiceRequest.GetStatusBarFill(((dblProcessor / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300") + "</td><td nowrap>" + dblProcessor.ToString() + "</td></tr>";
                        strSupply += "<tr" + (boolOther ? " bgcolor=\"F6F6F6\"" : "") + " class=\"" + (dblDiskSpace > 0.00 ? "greendefault" : (dblDiskSpace < 0.00 ? "reddefault" : "default")) + "\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>Disk Space:</td><td width=\"100%\">" + oServiceRequest.GetStatusBarFill(((dblDiskSpace / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300") + "</td><td nowrap>" + dblDiskSpace.ToString() + " GB</td></tr>";
                    }
                }
            }
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