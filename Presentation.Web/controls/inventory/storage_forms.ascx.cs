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
    public partial class storage_forms : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Projects oProject;
        protected Pages oPage;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strForms = "";
        protected string strFilter = "";
        protected string strParameters = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oProject = new Projects(intProfile, dsn);
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
                if (!IsPostBack)
                {
                    LoadLists();
                    LoadFilters();
                    LoadProjects();
                }
                btnProjects.Attributes.Add("onclick", "return MakeWider(this, '" + lstProjects.ClientID + "');");
                btnProjectsClear.Attributes.Add("onclick", "return ClearList('" + lstProjects.ClientID + "');");
            }
        }
        private void LoadProjects()
        {
            Forecast oForecast = new Forecast(intProfile, dsn);
            DataSet dsForm = oForecast.Gets();
            if (strFilter.StartsWith(" AND ") == true)
                strFilter = strFilter.Substring(5);
            bool boolOther = false;
            DataRow[] drReturn = dsForm.Tables[0].Select(strFilter);
            Users oUser = new Users(intProfile, dsn);
            int intCount = 0;
            foreach (DataRow dr in drReturn)
            {
                intCount++;
                string strNumber = (dr["number"].ToString() == "" ? "-----" : dr["number"].ToString());
                //            if (strForms != "")
                //                strForms += "<tr><td colspan=\"6\">&nbsp;</td></tr>";
                strForms += "<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_" + intCount.ToString() + "','divGroup_" + intCount.ToString() + "');\"><img id=\"imgGroup_" + intCount.ToString() + "\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td nowrap>" + strNumber + "</td><td nowrap>" + dr["name"].ToString() + "</td><td nowrap>" + dr["bd"].ToString() + "</td><td nowrap>" + dr["portfolio"].ToString() + "</td><td width=\"100%\">&nbsp;</td></tr>";
                string strForm = "";
                DataSet dsAnswers = oForecast.GetAnswers(Int32.Parse(dr["id"].ToString()));
                foreach (DataRow drAnswer in dsAnswers.Tables[0].Rows)
                {
                    if (oForecast.IsStorage(Int32.Parse(drAnswer["id"].ToString())) == true)
                    {
                        boolOther = !boolOther;
                        strForm += "<tr" + (boolOther ? " bgcolor=\"F6F6F6\"" : "") + "><td nowrap><img src=\"/images/down_right.gif\" border=\"0\" align=\"absmiddle\"/></td><td nowrap>" + drAnswer["name"].ToString() + "</td><td nowrap>" + drAnswer["confidence"].ToString() + "</td><td nowrap>" + drAnswer["modified"].ToString() + "</td><td width=\"100%\" align=\"right\">[<a href=\"javascript:void(0);\" onclick=\"OpenWindow('INVENTORY_BACKUP','" + drAnswer["id"].ToString() + "');\">View TSM Sizer</a>]&nbsp;&nbsp;&nbsp;[<a href=\"javascript:void(0);\" onclick=\"OpenWindow('INVENTORY_BACKUP_REGISTRATION','" + drAnswer["id"].ToString() + "');\">View TSM Registration</a>]</td></tr>";
                    }
                }
                if (strForm != "")
                    strForms += "<tr id=\"divGroup_" + intCount.ToString() + "\" style=\"display:none\"><td></td><td colspan=\"5\"><table width=\"100%\" cellpadding=\"5\" cellspacing=\"0\" border=\"0\"><tr class=\"bold\"><td></td><td>Nickname</td><td>Confidence</td><td>Last Modified</td><td></td></tr>" + strForm + "</table></td></tr>";
            }
            if (strForms != "")
                strForms = "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"2\" border=\"0\"><tr class=\"bold\"><td></td><td nowrap>Project Number</td><td nowrap>Project Name</td><td nowrap>Project Type</td><td nowrap>Portfolio</td></tr>" + strForms + "</table>";
        }
        private void LoadLists()
        {
            Projects oProject = new Projects(intProfile, dsn);
            lstProjects.DataValueField = "projectid";
            lstProjects.DataTextField = "name";
            lstProjects.DataSource = oProject.GetActive();
            lstProjects.DataBind();
            lstProjects.Items.Insert(0, new ListItem("-- ALL --", "0"));
            LoadList("pid", "Project(s)", lstProjects, null);
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
            foreach (ListItem oList in lstProjects.Items)
            {
                if (oList.Selected == true)
                {
                    sb.Append("&pid=");
                    sb.Append(oList.Value);
                }
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=F" + sb.ToString());
        }
        private void LoadFilters()
        {
            strFilter += LoadFilter("pid", "projectid");
            hdnFilter.Value = strFilter;
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
        protected bool ColumnEqual(object A, object B)
        {
            if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
                return true;
            if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
                return false;
            return (A.Equals(B));  // value type standard comparison
        }
    }
}