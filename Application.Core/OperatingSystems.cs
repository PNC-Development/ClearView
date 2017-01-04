using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class OperatingSystems
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public OperatingSystems(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOperatingSystemsAll", arParams);
        }
        public DataSet Gets(int _workstation, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@workstation", _workstation);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOperatingSystems", arParams);
        }
        public DataSet GetStandard(int _workstation)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@workstation", _workstation);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOperatingSystemsStandard", arParams);
        }
        
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOperatingSystem", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public int Add(string _name, int _cluster_name, int _workstation, string _code, string _factory_code, string _citrix_config, string _zeus_os, string _zeus_os_version, string _zeus_build_type, string _zeus_build_type_pnc, string _vmware_os, string _boot_environment, string _task_sequence, string _altiris, int _linux, int _aix, int _solaris, int _windows, int _windows2008, int _rdp_altiris, int _rdp_mdt, int _e1000, int _manual_build, int _default_sp, int _standard, int _display, int _enabled)
        {
            arParams = new SqlParameter[28];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@cluster_name", _cluster_name);
            arParams[2] = new SqlParameter("@workstation", _workstation);
            arParams[3] = new SqlParameter("@code", _code);
            arParams[4] = new SqlParameter("@factory_code", _factory_code);
            arParams[5] = new SqlParameter("@citrix_config", _citrix_config);
            arParams[6] = new SqlParameter("@zeus_os", _zeus_os);
            arParams[7] = new SqlParameter("@zeus_os_version", _zeus_os_version);
            arParams[8] = new SqlParameter("@zeus_build_type", _zeus_build_type);
            arParams[9] = new SqlParameter("@zeus_build_type_pnc", _zeus_build_type_pnc);
            arParams[10] = new SqlParameter("@vmware_os", _vmware_os);
            arParams[11] = new SqlParameter("@boot_environment", _boot_environment);
            arParams[12] = new SqlParameter("@task_sequence", _task_sequence);
            arParams[13] = new SqlParameter("@altiris", _altiris);
            arParams[14] = new SqlParameter("@linux", _linux);
            arParams[15] = new SqlParameter("@aix", _aix);
            arParams[16] = new SqlParameter("@solaris", _solaris);
            arParams[17] = new SqlParameter("@windows", _windows);
            arParams[18] = new SqlParameter("@windows2008", _windows2008);
            arParams[19] = new SqlParameter("@rdp_altiris", _rdp_altiris);
            arParams[20] = new SqlParameter("@rdp_mdt", _rdp_mdt);
            arParams[21] = new SqlParameter("@e1000", _e1000);
            arParams[22] = new SqlParameter("@manual_build", _manual_build);
            arParams[23] = new SqlParameter("@default_sp", _default_sp);
            arParams[24] = new SqlParameter("@standard", _standard);
            arParams[25] = new SqlParameter("@display", _display);
            arParams[26] = new SqlParameter("@enabled", _enabled);
            arParams[27] = new SqlParameter("@id", SqlDbType.Int);
            arParams[27].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOperatingSystem", arParams);
            return Int32.Parse(arParams[27].Value.ToString());
        }
        public void Update(int _id, string _name, int _cluster_name, int _workstation, string _code, string _factory_code, string _citrix_config, string _zeus_os, string _zeus_os_version, string _zeus_build_type, string _zeus_build_type_pnc, string _vmware_os, string _boot_environment, string _task_sequence, string _altiris, int _linux, int _aix, int _solaris, int _windows, int _windows2008, int _rdp_altiris, int _rdp_mdt, int _e1000, int _manual_build, int _default_sp, int _standard, int _enabled)
        {
            arParams = new SqlParameter[27];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@cluster_name", _cluster_name);
            arParams[3] = new SqlParameter("@workstation", _workstation);
            arParams[4] = new SqlParameter("@code", _code);
            arParams[5] = new SqlParameter("@factory_code", _factory_code);
            arParams[6] = new SqlParameter("@citrix_config", _citrix_config);
            arParams[7] = new SqlParameter("@zeus_os", _zeus_os);
            arParams[8] = new SqlParameter("@zeus_os_version", _zeus_os_version);
            arParams[9] = new SqlParameter("@zeus_build_type", _zeus_build_type);
            arParams[10] = new SqlParameter("@zeus_build_type_pnc", _zeus_build_type_pnc);
            arParams[11] = new SqlParameter("@vmware_os", _vmware_os);
            arParams[12] = new SqlParameter("@boot_environment", _boot_environment);
            arParams[13] = new SqlParameter("@task_sequence", _task_sequence);
            arParams[14] = new SqlParameter("@altiris", _altiris);
            arParams[15] = new SqlParameter("@linux", _linux);
            arParams[16] = new SqlParameter("@aix", _aix);
            arParams[17] = new SqlParameter("@solaris", _solaris);
            arParams[18] = new SqlParameter("@windows", _windows);
            arParams[19] = new SqlParameter("@windows2008", _windows2008);
            arParams[20] = new SqlParameter("@rdp_altiris", _rdp_altiris);
            arParams[21] = new SqlParameter("@rdp_mdt", _rdp_mdt);
            arParams[22] = new SqlParameter("@e1000", _e1000);
            arParams[23] = new SqlParameter("@manual_build", _manual_build);
            arParams[24] = new SqlParameter("@default_sp", _default_sp);
            arParams[25] = new SqlParameter("@standard", _standard);
            arParams[26] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOperatingSystem", arParams);
        }
        public void UpdateOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOperatingSystemOrder", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOperatingSystemEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOperatingSystem", arParams);
        }

        public bool IsMidrange(int _id)
        {
            return (IsLinux(_id) == true || IsAix(_id) == true || IsSolaris(_id) == true);
        }
        public bool IsLinux(int _id)
        {
            return (Get(_id, "linux") == "1");
        }
        public bool IsAix(int _id)
        {
            return (Get(_id, "aix") == "1");
        }
        public bool IsSolaris(int _id)
        {
            return (Get(_id, "solaris") == "1");
        }
        public bool IsWindows(int _id)
        {
            return (Get(_id, "windows") == "1");
        }
        public bool IsWindows2008(int _id)
        {
            return (Get(_id, "windows2008") == "1");
        }
        public bool IsDistributed(int _id)
        {
            return (IsWindows(_id) || IsWindows2008(_id));
        }


        public void AddServicePack(int _osid, int _spid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@osid", _osid);
            arParams[1] = new SqlParameter("@spid", _spid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOperatingSystemServicePack", arParams);
        }
        public void DeleteServicePack(int _osid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@osid", _osid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOperatingSystemServicePack", arParams);
        }
        public DataSet GetServicePack(int _osid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@osid", _osid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOperatingSystemServicePack", arParams);
        }


        public void AddGroup(string _name, int _display, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@display", _display);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOperatingSystemGroup", arParams);
        }
        public void UpdateGroup(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOperatingSystemGroup", arParams);
        }
        public void UpdateGroupOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOperatingSystemGroupOrder", arParams);
        }
        public void EnableGroup(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOperatingSystemGroupEnabled", arParams);
        }
        public void DeleteGroup(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOperatingSystemGroup", arParams);
        }
        public DataSet GetGroup(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOperatingSystemGroup", arParams);
        }
        public string GetGroup(int _id, string _column)
        {
            DataSet ds = GetGroup(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetGroups(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOperatingSystemGroups", arParams);
        }

        public void AddsGroup(int _osid, int _groupid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@osid", _osid);
            arParams[1] = new SqlParameter("@groupid", _groupid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOperatingSystemsGroup", arParams);
        }
        public void DeletesGroup(int _groupid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@groupid", _groupid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOperatingSystemsGroup", arParams);
        }
        public DataSet GetsGroup(int _groupid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@groupid", _groupid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOperatingSystemsGroup", arParams);
        }

    }
}
