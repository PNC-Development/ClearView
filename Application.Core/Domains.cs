using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class Domains
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Domains(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomains", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomain", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetsTest(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomainsTest", arParams);
        }
        public DataSet GetsGroupMaintenance(int _test_domain, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@test_domain", _test_domain);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomainsGroupMaintenance", arParams);
        }
        public DataSet GetsAccountMaintenance(int _test_domain, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@test_domain", _test_domain);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomainsAccountMaintenance", arParams);
        }
        public void Add(string _name, string _zeus, int _environment, int _account_setup, int _account_maintenance, int _group_maintenance, int _test_domain, int _move, int _display, int _enabled)
		{
			arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@zeus", _zeus);
            arParams[2] = new SqlParameter("@environment", _environment);
            arParams[3] = new SqlParameter("@account_setup", _account_setup);
            arParams[4] = new SqlParameter("@account_maintenance", _account_maintenance);
            arParams[5] = new SqlParameter("@group_maintenance", _group_maintenance);
            arParams[6] = new SqlParameter("@test_domain", _test_domain);
            arParams[7] = new SqlParameter("@move", _move);
            arParams[8] = new SqlParameter("@display", _display);
            arParams[9] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDomain", arParams);
		}
        public void Update(int _id, string _name, string _zeus, int _environment, int _account_setup, int _account_maintenance, int _group_maintenance, int _test_domain, int _move, int _enabled)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@zeus", _zeus);
            arParams[3] = new SqlParameter("@environment", _environment);
            arParams[4] = new SqlParameter("@account_setup", _account_setup);
            arParams[5] = new SqlParameter("@account_maintenance", _account_maintenance);
            arParams[6] = new SqlParameter("@group_maintenance", _group_maintenance);
            arParams[7] = new SqlParameter("@test_domain", _test_domain);
            arParams[8] = new SqlParameter("@move", _move);
            arParams[9] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDomain", arParams);
        }
        public void UpdateOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDomainOrder", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDomainEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDomain", arParams);
        }


        public void AddClassEnvironment(int _domainid, int _classid, int _environmentid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDomainClassEnvironment", arParams);
        }
        public void DeleteClassEnvironment(int _domainid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDomainClassEnvironment", arParams);
        }
        public DataSet GetClassEnvironment(int _domainid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomainClassEnvironmentByDomain", arParams);
        }
        public DataSet GetClassEnvironment(int _classid, int _environmentid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomainClassEnvironment", arParams);
        }
        public DataSet GetClassEnvironments()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomainClassEnvironments");
        }
        public DataSet GetClassEnvironments(int _domainid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomainClassEnvironmentsClass", arParams);
        }
        

        public DataSet GetAdminGroups(int _domainid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomainAdminGroups", arParams);
        }
        public DataSet GetAdminGroups(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomainAdminGroupsAll", arParams);
        }
        public DataSet GetAdminGroup(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomainAdminGroup", arParams);
        }
        public string GetAdminGroup(int _id, string _column)
        {
            DataSet ds = GetAdminGroup(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddAdminGroup(int _domainid, string _name, int _display, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@display", _display);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDomainAdminGroup", arParams);
        }
        public void UpdateAdminGroup(int _id, int _domainid, string _name, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@domainid", _domainid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDomainAdminGroup", arParams);
        }
        public void UpdateAdminGroupOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDomainAdminGroupOrder", arParams);
        }
        public void EnableAdminGroup(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDomainAdminGroupEnabled", arParams);
        }
        public void DeleteAdminGroup(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDomainAdminGroup", arParams);
        }

        public DataSet GetSuffixs(int _domainid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomainSuffixs", arParams);
        }
        public DataSet GetSuffixs(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomainSuffixsAll", arParams);
        }
        public DataSet GetSuffix(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomainSuffix", arParams);
        }
        public string GetSuffix(int _id, string _column)
        {
            DataSet ds = GetSuffix(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddSuffix(int _domainid, string _name, int _display, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@display", _display);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDomainSuffix", arParams);
        }
        public void UpdateSuffix(int _id, int _domainid, string _name, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@domainid", _domainid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDomainSuffix", arParams);
        }
        public void UpdateSuffixOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDomainSuffixOrder", arParams);
        }
        public void EnableSuffix(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDomainSuffixEnabled", arParams);
        }
        public void DeleteSuffix(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDomainSuffix", arParams);
        }




        public void AddClassDNS(int _domainid, int _classid, int _addressid, string _dns_ip1, string _dns_ip2, string _dns_ip3, string _dns_ip4, string _wins_ip1, string _wins_ip2, string _wins_ip3, string _wins_ip4)
        {
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@dns_ip1", _dns_ip1);
            arParams[4] = new SqlParameter("@dns_ip2", _dns_ip2);
            arParams[5] = new SqlParameter("@dns_ip3", _dns_ip3);
            arParams[6] = new SqlParameter("@dns_ip4", _dns_ip4);
            arParams[7] = new SqlParameter("@wins_ip1", _wins_ip1);
            arParams[8] = new SqlParameter("@wins_ip2", _wins_ip2);
            arParams[9] = new SqlParameter("@wins_ip3", _wins_ip3);
            arParams[10] = new SqlParameter("@wins_ip4", _wins_ip4);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDomainClassDNS", arParams);
        }
        public void UpdateClassDNS(int _id, string _dns_ip1, string _dns_ip2, string _dns_ip3, string _dns_ip4, string _wins_ip1, string _wins_ip2, string _wins_ip3, string _wins_ip4)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@dns_ip1", _dns_ip1);
            arParams[2] = new SqlParameter("@dns_ip2", _dns_ip2);
            arParams[3] = new SqlParameter("@dns_ip3", _dns_ip3);
            arParams[4] = new SqlParameter("@dns_ip4", _dns_ip4);
            arParams[5] = new SqlParameter("@wins_ip1", _wins_ip1);
            arParams[6] = new SqlParameter("@wins_ip2", _wins_ip2);
            arParams[7] = new SqlParameter("@wins_ip3", _wins_ip3);
            arParams[8] = new SqlParameter("@wins_ip4", _wins_ip4);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDomainClassDNS", arParams);
        }
        public void DeleteClassDNS(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDomainClassDNS", arParams);
        }
        public DataSet GetClassDNS(int _domainid, int _classid, int _addressid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomainClassDNS", arParams);
        }
        public DataSet GetClassDNS(int _domainid, int _classid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            arParams[1] = new SqlParameter("@classid", _classid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomainClassDNSs", arParams);
        }
        public DataSet GetClassDNS(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDomainClassDNSID", arParams);
        }
    }
}
