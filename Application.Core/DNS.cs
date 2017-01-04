using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;

namespace NCC.ClearView.Application.Core
{
    public class DNS
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public DNS(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDNSTypes", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDNSType", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void Add(string _name, string _value, int _display, int _enabled)
		{
			arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@value", _value);
            arParams[2] = new SqlParameter("@display", _display);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDNSType", arParams);
		}
        public void Update(int _id, string _name, string _value, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@value", _value);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDNSType", arParams);
        }
        public void UpdateOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDNSTypeOrder", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDNSTypeEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDNSType", arParams);
        }




        public DataSet GetDNS(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDNS", arParams);
        }
        public void AddDNS(int _requestid, int _itemid, int _number, string _action, string _type, string _ip_current, string _ip_new, string _name_current, string _name_new, string _alias_current, string _alias_new, int _typeid)
        {
            arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@action", _action);
            arParams[4] = new SqlParameter("@type", _type);
            arParams[5] = new SqlParameter("@ip_current", _ip_current);
            arParams[6] = new SqlParameter("@ip_new", _ip_new);
            arParams[7] = new SqlParameter("@name_current", _name_current);
            arParams[8] = new SqlParameter("@name_new", _name_new);
            arParams[9] = new SqlParameter("@alias_current", _alias_current);
            arParams[10] = new SqlParameter("@alias_new", _alias_new);
            arParams[11] = new SqlParameter("@typeid", _typeid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDNS", arParams);
        }
        public void UpdateDNS(int _requestid, int _itemid, int _number, string _ip_current, string _ip_new, string _name_current, string _name_new, string _alias_current, string _alias_new, string _domain, int _admin1, int _admin2, string _change, string _reason)
        {
            arParams = new SqlParameter[14];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@ip_current", _ip_current);
            arParams[4] = new SqlParameter("@ip_new", _ip_new);
            arParams[5] = new SqlParameter("@name_current", _name_current);
            arParams[6] = new SqlParameter("@name_new", _name_new);
            arParams[7] = new SqlParameter("@alias_current", _alias_current);
            arParams[8] = new SqlParameter("@alias_new", _alias_new);
            arParams[9] = new SqlParameter("@domain", _domain);
            arParams[10] = new SqlParameter("@admin1", _admin1);
            arParams[11] = new SqlParameter("@admin2", _admin2);
            arParams[12] = new SqlParameter("@change", _change);
            arParams[13] = new SqlParameter("@reason", _reason);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDNS", arParams);
        }
        public void UpdateDNSCompleted(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDNSCompleted", arParams);
        }
        public void DeleteDNS(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDNS", arParams);
        }
        public string GetDNSBody(int _requestid, int _itemid, int _number, bool _table, int _environment)
        {
            DataSet ds = GetDNS(_requestid, _itemid, _number);
            Users oUser = new Users(0, dsn);
            Requests oRequest = new Requests(0, dsn);
            Settings oSetting = new Settings(0, dsn);
            bool boolDNS_QIP = oSetting.IsDNS_QIP();
            bool boolDNS_Bluecat = oSetting.IsDNS_Bluecat();
            StringBuilder sbDetails = new StringBuilder();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                int intUser = oRequest.GetUser(_requestid);
                switch (dr["action"].ToString())
                {
                    case "CREATE":
                        sbDetails.Append("<tr><td>Action:</td><td>" + "Create a DNS Record" + "</td></tr>");
                        sbDetails.Append("<tr><td><b>IP Address:</b></td><td><b>" + dr["ip_new"].ToString() + "</b></td></tr>");
                        sbDetails.Append("<tr><td><b>Device Name:</b></td><td><b>" + dr["name_new"].ToString() + "</b></td></tr>");
                        if (boolDNS_QIP == true)
                        {
                            string strAliasNew = dr["alias_new"].ToString();
                            while (strAliasNew.Contains(" ") == true)
                                strAliasNew = strAliasNew.Replace(" ", "<br/>");
                            sbDetails.Append("<tr><td valign=\"top\">Alias(es):</td><td valign=\"top\">" + (strAliasNew == "" ? "<i>None</i>" : strAliasNew) + "</td></tr>");
                        }
                        break;
                    case "UPDATE":
                        sbDetails.Append("<tr><td>Action:</td><td>" + "Update a DNS Record" + "</td></tr>");
                        if (dr["name_current"].ToString() != "")
                            sbDetails.Append("<tr><td>Current Device Name:</td><td>" + dr["name_current"].ToString() + "</td></tr>");
                        if (dr["name_new"].ToString() != "")
                            sbDetails.Append("<tr><td><b>New Device Name:</b></td><td><b>" + dr["name_new"].ToString() + "</b></td></tr>");
                        if (dr["ip_current"].ToString() != "")
                            sbDetails.Append("<tr><td>Current IP Address:</td><td>" + dr["ip_current"].ToString() + "</td></tr>");
                        if (dr["ip_new"].ToString() != "")
                            sbDetails.Append("<tr><td><b>New IP Address:</b></td><td><b>" + dr["ip_new"].ToString() + "</b></td></tr>");
                        if (boolDNS_QIP == true)
                        {
                            if (dr["alias_current"].ToString() != "")
                            {
                                string strAlias = dr["alias_current"].ToString();
                                while (strAlias.Contains(";") == true)
                                    strAlias = strAlias.Replace(";", "<br/>");
                                sbDetails.Append("<tr><td valign=\"top\">Current Alias(es):</td><td valign=\"top\">" + (strAlias == "" ? "<i>None</i>" : strAlias) + "</td></tr>");
                            }
                            if (dr["alias_new"].ToString() != "" || (dr["name_new"].ToString() == "" && dr["ip_new"].ToString() == ""))
                            {
                                string strAlias = dr["alias_new"].ToString();
                                while (strAlias.Contains(" ") == true)
                                    strAlias = strAlias.Replace(" ", "<br/>");
                                sbDetails.Append("<tr><td valign=\"top\"><b>New Alias(es):</b></td><td valign=\"top\"><b>" + (strAlias == "" ? "<i>None</i>" : strAlias) + "</b></td></tr>");
                            }
                        }
                        break;
                    case "DELETE":
                        sbDetails.Append("<tr><td>Action:</td><td>" + "Delete a DNS Record" + "</td></tr>");
                        if (dr["name_current"].ToString() != "")
                            sbDetails.Append("<tr><td><b>Device Name:</b></td><td><b>" + dr["name_current"].ToString() + "</b></td></tr>");
                        if (dr["ip_current"].ToString() != "")
                            sbDetails.Append("<tr><td><b>IP Address:</b></td><td><b>" + dr["ip_current"].ToString() + "</b></td></tr>");
                        break;
                }
                if (dr["change"].ToString() != "")
                    sbDetails.Append("<tr><td>Change Control #:</td><td>" + dr["change"].ToString() + "</td></tr>");
                if (dr["reason"].ToString() != "")
                    sbDetails.Append("<tr><td>Reason:</td><td>" + dr["reason"].ToString() + "</td></tr>");
                sbDetails.Append("<tr><td>Requestor:</td><td>" + oUser.GetFullName(intUser) + " (" + oUser.GetName(intUser) + ")" + "</td></tr>");
                if (_table == true)
                {
                    Variables oVariable = new Variables(_environment);
                    sbDetails.Insert(0, "<table cellpadding=\"3\" cellspacing=\"2\" border=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                    sbDetails.Append("</table>");
                }
            }
            return sbDetails.ToString();
        }
    }
}
