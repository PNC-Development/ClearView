using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class BuildLocation
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public BuildLocation(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Gets(int _classid, int _environmentid, int _addressid, int _modelid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@modelid", _modelid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getBuildLocations", arParams);
        }
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getBuildLocationsAll", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getBuildLocation", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void Add(int _classid, int _environmentid, int _addressid, int _build_classid, int _build_environmentid, int _build_addressid, int _modelid, int _enabled)
		{
			arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@build_classid", _build_classid);
            arParams[4] = new SqlParameter("@build_environmentid", _build_environmentid);
            arParams[5] = new SqlParameter("@build_addressid", _build_addressid);
            arParams[6] = new SqlParameter("@modelid", _modelid);
            arParams[7] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addBuildLocation", arParams);
		}
        public void Update(int _id, int _classid, int _environmentid, int _addressid, int _build_classid, int _build_environmentid, int _build_addressid, int _modelid, int _enabled)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@addressid", _addressid);
            arParams[4] = new SqlParameter("@build_classid", _build_classid);
            arParams[5] = new SqlParameter("@build_environmentid", _build_environmentid);
            arParams[6] = new SqlParameter("@build_addressid", _build_addressid);
            arParams[7] = new SqlParameter("@modelid", _modelid);
            arParams[8] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateBuildLocation", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteBuildLocation", arParams);
        }


        public DataSet GetRDPs(int _classid, int _environmentid, int _addressid, int _resiliencyid, int _server_altiris, int _server_mdt, int _workstation, int _zoneid)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[4] = new SqlParameter("@server_altiris", _server_altiris);
            arParams[5] = new SqlParameter("@server_mdt", _server_mdt);
            arParams[6] = new SqlParameter("@workstation", _workstation);
            arParams[7] = new SqlParameter("@zoneid", _zoneid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getBuildLocationRDPs", arParams);
        }
        public DataSet GetRDPs(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getBuildLocationRDPsAll", arParams);
        }
        public DataSet GetRDP(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getBuildLocationRDP", arParams);
        }
        public string GetRDP(int _id, string _column)
        {
            DataSet ds = GetRDP(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddRDP(int _classid, int _environmentid, int _addressid, string rdp_schedule_ws, string rdp_computer_ws, string rdp_mdt_ws, string _jumpstart_cgi, string _jumpstart_build_type, string _blade_vlan, string _vmware_vlan, string _vsphere_vlan, string _dell_vlan, string _dell_vmware_vlan, int _resiliencyid, string _source, int _server_altiris, int _server_mdt, int _workstation, int _zones, int _enabled)
        {
            arParams = new SqlParameter[20];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@rdp_schedule_ws", rdp_schedule_ws);
            arParams[4] = new SqlParameter("@rdp_computer_ws", rdp_computer_ws);
            arParams[5] = new SqlParameter("@rdp_mdt_ws", rdp_mdt_ws);
            arParams[6] = new SqlParameter("@jumpstart_cgi", _jumpstart_cgi);
            arParams[7] = new SqlParameter("@jumpstart_build_type", _jumpstart_build_type);
            arParams[8] = new SqlParameter("@blade_vlan", _blade_vlan);
            arParams[9] = new SqlParameter("@vmware_vlan", _vmware_vlan);
            arParams[10] = new SqlParameter("@vsphere_vlan", _vsphere_vlan);
            arParams[11] = new SqlParameter("@dell_vlan", _dell_vlan);
            arParams[12] = new SqlParameter("@dell_vmware_vlan", _dell_vmware_vlan);
            arParams[13] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[14] = new SqlParameter("@source", _source);
            arParams[15] = new SqlParameter("@server_altiris", _server_altiris);
            arParams[16] = new SqlParameter("@server_mdt", _server_mdt);
            arParams[17] = new SqlParameter("@workstation", _workstation);
            arParams[18] = new SqlParameter("@zones", _zones);
            arParams[19] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addBuildLocationRDP", arParams);
        }
        public void UpdateRDP(int _id, int _classid, int _environmentid, int _addressid, string rdp_schedule_ws, string rdp_computer_ws, string rdp_mdt_ws, string _jumpstart_cgi, string _jumpstart_build_type, string _blade_vlan, string _vmware_vlan, string _vsphere_vlan, string _dell_vlan, string _dell_vmware_vlan, int _resiliencyid, string _source, int _server_altiris, int _server_mdt, int _workstation, int _zones, int _enabled)
        {
            arParams = new SqlParameter[21];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@addressid", _addressid);
            arParams[4] = new SqlParameter("@rdp_schedule_ws", rdp_schedule_ws);
            arParams[5] = new SqlParameter("@rdp_computer_ws", rdp_computer_ws);
            arParams[6] = new SqlParameter("@rdp_mdt_ws", rdp_mdt_ws);
            arParams[7] = new SqlParameter("@jumpstart_cgi", _jumpstart_cgi);
            arParams[8] = new SqlParameter("@jumpstart_build_type", _jumpstart_build_type);
            arParams[9] = new SqlParameter("@blade_vlan", _blade_vlan);
            arParams[10] = new SqlParameter("@vmware_vlan", _vmware_vlan);
            arParams[11] = new SqlParameter("@vsphere_vlan", _vsphere_vlan);
            arParams[12] = new SqlParameter("@dell_vlan", _dell_vlan);
            arParams[13] = new SqlParameter("@dell_vmware_vlan", _dell_vmware_vlan);
            arParams[14] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[15] = new SqlParameter("@source", _source);
            arParams[16] = new SqlParameter("@server_altiris", _server_altiris);
            arParams[17] = new SqlParameter("@server_mdt", _server_mdt);
            arParams[18] = new SqlParameter("@workstation", _workstation);
            arParams[19] = new SqlParameter("@zones", _zones);
            arParams[20] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateBuildLocationRDP", arParams);
        }
        public void DeleteRDP(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteBuildLocationRDP", arParams);
        }

        public void AddRDPZone(int _buildlocationrdpid, int _zoneid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@buildlocationrdpid", _buildlocationrdpid);
            arParams[1] = new SqlParameter("@zoneid", _zoneid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addBuildLocationRDPZone", arParams);
        }
        public void DeleteRDPZone(int _buildlocationrdpid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@buildlocationrdpid", _buildlocationrdpid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteBuildLocationRDPZone", arParams);
        }
        public DataSet GetRDPZone(int _buildlocationrdpid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@buildlocationrdpid", _buildlocationrdpid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getBuildLocationRDPZone", arParams);
        }
    }
}
