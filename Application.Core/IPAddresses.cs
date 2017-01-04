using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public enum IPAddressType
    {
        ILO = 1,
        ClusterInstance = 2,
        ClusterNode = 3,
        EcomProd = 4,
        EcomService = 5,
        Primary = 6,
        Backup = 7
    }
    public class IPAddresses
	{
		private string dsn_ip = "";
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        private int intPrevious = 0;
        private string strResults = "";
        public IPAddresses(int _user, string _dsn_ip, string _dsn)
		{
			user = _user;
			dsn_ip = _dsn_ip;
			dsn = _dsn;
		}
        public DataSet GetMine(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddressesMine", arParams);
        }
        public DataSet GetMineUnused(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddressesMineUnused", arParams);
        }
        public DataSet Gets(int _networkid, int _available)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@networkid", _networkid);
            arParams[1] = new SqlParameter("@available", _available);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddresses", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddress", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public int GetNetworkVlan(int _networkid)
        {
            if (GetNetwork(_networkid, "vlanid") != "")
            {
                int intVlan = Int32.Parse(GetNetwork(_networkid, "vlanid"));
                if (GetVlan(intVlan, "vlan") != "")
                    return Int32.Parse(GetVlan(intVlan, "vlan"));
                else
                    return 0;
            }
            else
                return 0;
        }
        public int GetAddressNetwork(int _addressid)
        {
            if (Get(_addressid, "networkid") != "")
                return Int32.Parse(Get(_addressid, "networkid"));
            else
                return 0;
        }

        public DataSet Get(int _add1, int _add2, int _add3, int _add4)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@add1", _add1);
            arParams[1] = new SqlParameter("@add2", _add2);
            arParams[2] = new SqlParameter("@add3", _add3);
            arParams[3] = new SqlParameter("@add4", _add4);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddressFull", arParams);
        }
        public DataSet Get(int _add1, int _add2, int _add3, int _add4, int _available)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@add1", _add1);
            arParams[1] = new SqlParameter("@add2", _add2);
            arParams[2] = new SqlParameter("@add3", _add3);
            arParams[3] = new SqlParameter("@add4", _add4);
            arParams[4] = new SqlParameter("@available", _available);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddressFullAvailable", arParams);
        }
        public string GetName(int _id)
        {
            DataSet ds = Get(_id);
            string strReturn = "N / A";
            if (ds.Tables[0].Rows.Count > 0)
                strReturn = ds.Tables[0].Rows[0]["add1"].ToString() + "." + ds.Tables[0].Rows[0]["add2"].ToString() + "." + ds.Tables[0].Rows[0]["add3"].ToString() + "." + ds.Tables[0].Rows[0]["add4"].ToString();
            return strReturn;
        }
        public string GetName(int _id, int _userid)
        {
            DataSet ds = Get(_id);
            string strReturn = "N / A";
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["userid"].ToString() == _userid.ToString() || _userid == 0)
                    strReturn = ds.Tables[0].Rows[0]["add1"].ToString() + "." + ds.Tables[0].Rows[0]["add2"].ToString() + "." + ds.Tables[0].Rows[0]["add3"].ToString() + "." + ds.Tables[0].Rows[0]["add4"].ToString();
                else
                    strReturn = "DENIED";
            }
            return strReturn;
        }
        public int Add(int _networkid, int _add1, int _add2, int _add3, int _add4, int _userid)
		{
            int _dhcp = 0;
            DataSet ds = GetDhcps(_networkid, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["add1"].ToString()) == _add1 && Int32.Parse(dr["add2"].ToString()) == _add2 && Int32.Parse(dr["add3"].ToString()) == _add3)
                {
                    int intMin = Int32.Parse(dr["min4"].ToString());
                    int intMax = Int32.Parse(dr["max4"].ToString());
                    if (_add4 >= intMin && _add4 <= intMax)
                    {
                        _dhcp = 1;
                        break;
                    }
                }
            }
            return Add(_networkid, _add1, _add2, _add3, _add4, _dhcp, _userid);
		}
        private int Add(int _networkid, int _add1, int _add2, int _add3, int _add4, int _dhcp, int _userid)
        {
            DataSet ds = Get(_add1, _add2, _add3, _add4);
            if (ds.Tables[0].Rows.Count > 0)
            {
                // If IP address already exists, attempt to update the NETWORKID, set AVAILABLE = 0 and return ID
                DataRow dr = ds.Tables[0].Rows[0];
                int intID = Int32.Parse(dr["id"].ToString());
                int intNetwork = Int32.Parse(dr["networkid"].ToString());
                if (intNetwork != _networkid && _networkid > 0)
                    UpdateNetwork(intID, _networkid);
                UpdateAvailable(intID, 0);
                return intID;
            }
            else
            {
                // If IP address does not exist, add it and return ID
                arParams = new SqlParameter[8];
                arParams[0] = new SqlParameter("@networkid", _networkid);
                arParams[1] = new SqlParameter("@add1", _add1);
                arParams[2] = new SqlParameter("@add2", _add2);
                arParams[3] = new SqlParameter("@add3", _add3);
                arParams[4] = new SqlParameter("@add4", _add4);
                arParams[5] = new SqlParameter("@dhcp", _dhcp);
                arParams[6] = new SqlParameter("@userid", _userid);
                arParams[7] = new SqlParameter("@id", SqlDbType.Int);
                arParams[7].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_addAddress", arParams);
                return Int32.Parse(arParams[7].Value.ToString());
            }
        }
        public void UpdateDHCP(int _id, int _dhcp)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@dhcp", _dhcp);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_updateAddressDHCP", arParams);
        }
        public void UpdateAvailable(int _id, int _available)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@available", _available);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_updateAddressAvailable", arParams);
        }
        public void Update(int _id, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_updateAddress", arParams);
        }
        public void UpdateNetwork(int _id, int _networkid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@networkid", _networkid);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_updateAddressNetwork", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_deleteAddress", arParams);
        }

        public int AddDetails(string _url, int _projectid, string _instance, int _vlan, string _serial, string _server_name, int _classid, int _environmentid, int _addressid, int _modelid, string _room, string _rack, string _enclosure, string _slot, int _csm, int _type)
        {
            arParams = new SqlParameter[17];
            arParams[0] = new SqlParameter("@url", _url);
            arParams[1] = new SqlParameter("@projectid", _projectid);
            arParams[2] = new SqlParameter("@instance", _instance);
            arParams[3] = new SqlParameter("@vlan", _vlan);
            arParams[4] = new SqlParameter("@serial", _serial);
            arParams[5] = new SqlParameter("@server_name", _server_name);
            arParams[6] = new SqlParameter("@classid", _classid);
            arParams[7] = new SqlParameter("@environmentid", _environmentid);
            arParams[8] = new SqlParameter("@addressid", _addressid);
            arParams[9] = new SqlParameter("@modelid", _modelid);
            arParams[10] = new SqlParameter("@room", _room);
            arParams[11] = new SqlParameter("@rack", _rack);
            arParams[12] = new SqlParameter("@enclosure", _enclosure);
            arParams[13] = new SqlParameter("@slot", _slot);
            arParams[14] = new SqlParameter("@csm", _csm);
            arParams[15] = new SqlParameter("@type", _type);
            arParams[16] = new SqlParameter("@id", SqlDbType.Int);
            arParams[16].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_addAddressDetails", arParams);
            return Int32.Parse(arParams[16].Value.ToString());
        }
        public void AddDetail(int _ipaddressid, int _detailid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@ipaddressid", _ipaddressid);
            arParams[1] = new SqlParameter("@detailid", _detailid);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_addAddressDetail", arParams);
        }
        public DataSet GetDetail(int _ipaddressid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@ipaddressid", _ipaddressid);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddressDetail", arParams);
        }
        public string GetDetail(int _ipaddressid, string _column)
        {
            DataSet ds = GetDetail(_ipaddressid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        public DataSet GetVlans(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getVlans", arParams);
        }
        public DataSet GetVlansDell(int _classid, int _environmentid, int _addressid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getVlansDell", arParams);
        }
        public DataSet GetVlansAddress(int _classid, int _environmentid, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getVlansAddresses", arParams);
        }
        public DataSet GetVlansAddress(int _classid, int _environmentid, int _addressid, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getVlansAddress", arParams);
        }
        public DataSet GetVlansList(int _classid, int _environmentid, int _addressid, int _blades_hp, int _blades_sun, int _csm, int _ltm_web, int _ltm_app, int _ltm_middle, int _enabled)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@blades_hp", _blades_hp);
            arParams[4] = new SqlParameter("@blades_sun", _blades_sun);
            arParams[5] = new SqlParameter("@csm", _csm);
            arParams[6] = new SqlParameter("@ltm_web", _ltm_web);
            arParams[7] = new SqlParameter("@ltm_app", _ltm_app);
            arParams[8] = new SqlParameter("@ltm_middle", _ltm_middle);
            arParams[9] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getVlansAddressList", arParams);
        }
        public DataSet GetNetworksList(int _vlanid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@vlanid", _vlanid);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getNetworksAddressList", arParams);
        }
        public DataSet GetNetworksSVE(int _clusterid, int _classid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@classid", _classid);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getNetworksSVE", arParams);
        }
        public DataSet GetNetworksSVEs(int _clusterid, int _classid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@classid", _classid);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getNetworksSVEs", arParams);
        }
        public DataSet GetNetworksRange(int _add1, int _add2, int _add3, int _min4, int _max4)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@add1", _add1);
            arParams[1] = new SqlParameter("@add2", _add2);
            arParams[2] = new SqlParameter("@add3", _add3);
            arParams[3] = new SqlParameter("@min4", _min4);
            arParams[4] = new SqlParameter("@max4", _max4);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getNetworksRange", arParams);
        }
        public DataSet GetNetworksRanges(int _add1, int _add2, int _add3)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@add1", _add1);
            arParams[1] = new SqlParameter("@add2", _add2);
            arParams[2] = new SqlParameter("@add3", _add3);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getNetworksRanges", arParams);
        }
        public DataSet GetNetworksList(int _classid, int _environmentid, int _addressid, int _blades_hp, int _blades_sun, int _csm, int _ltm_web, int _ltm_app, int _ltm_middle, int _enabled)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@blades_hp", _blades_hp);
            arParams[4] = new SqlParameter("@blades_sun", _blades_sun);
            arParams[5] = new SqlParameter("@csm", _csm);
            arParams[6] = new SqlParameter("@ltm_web", _ltm_web);
            arParams[7] = new SqlParameter("@ltm_app", _ltm_app);
            arParams[8] = new SqlParameter("@ltm_middle", _ltm_middle);
            arParams[9] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getNetworksAddressListAll", arParams);
        }
        public int GetVlan(int _vlan, int _classid, int _environmentid, int _addressid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@vlan", _vlan);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@addressid", _addressid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getVlan", arParams);
            if (ds.Tables[0].Rows.Count == 0)
                return 0;
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        }
        public DataSet GetVlan(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getVlanId", arParams);
        }
        public string GetVlan(int _id, string _column)
        {
            DataSet ds = GetVlan(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddVlan(int _vlan, int _physical_windows, int _physical_unix, int _ecom_production, int _ecom_service, int _ipx, int _virtual_workstation, int _vmware_workstation_external, int _vmware_workstation_internal, int _vmware_workstation_dr, int _vmware_host, int _vmware_vmotion, int _vmware_windows, int _vmware_linux, int _blades_hp, int _blades_sun, int _apv, int _mainframe, int _csm, int _csm_soa, int _replicates, int _pxe, int _ilo, int _csm_vip, int _ltm_web, int _ltm_app, int _ltm_middle, int _ltm_vip, int _windows_cluster, int _unix_cluster, int _accenture, int _ha, int _sun_cluster, int _sun_sve, int _storage, int _dell_web, int _dell_middleware, int _dell_database, int _dell_file, int _dell_misc, int _dell_under48, int _dell_avamar, string _switchname, string _vtpdomain, int _classid, int _environmentid, int _addressid, int _resiliencyid, int _enabled)
        {
            arParams = new SqlParameter[49];
            arParams[0] = new SqlParameter("@vlan", _vlan);
            arParams[1] = new SqlParameter("@physical_windows", _physical_windows);
            arParams[2] = new SqlParameter("@physical_unix", _physical_unix);
            arParams[3] = new SqlParameter("@ecom_production", _ecom_production);
            arParams[4] = new SqlParameter("@ecom_service", _ecom_service);
            arParams[5] = new SqlParameter("@ipx", _ipx);
            arParams[6] = new SqlParameter("@virtual_workstation", _virtual_workstation);
            arParams[7] = new SqlParameter("@vmware_workstation_external", _vmware_workstation_external);
            arParams[8] = new SqlParameter("@vmware_workstation_internal", _vmware_workstation_internal);
            arParams[9] = new SqlParameter("@vmware_workstation_dr", _vmware_workstation_dr);
            arParams[10] = new SqlParameter("@vmware_host", _vmware_host);
            arParams[11] = new SqlParameter("@vmware_vmotion", _vmware_vmotion);
            arParams[12] = new SqlParameter("@vmware_windows", _vmware_windows);
            arParams[13] = new SqlParameter("@vmware_linux", _vmware_linux);
            arParams[14] = new SqlParameter("@blades_hp", _blades_hp);
            arParams[15] = new SqlParameter("@blades_sun", _blades_sun);
            arParams[16] = new SqlParameter("@apv", _apv);
            arParams[17] = new SqlParameter("@mainframe", _mainframe);
            arParams[18] = new SqlParameter("@csm", _csm);
            arParams[19] = new SqlParameter("@csm_soa", _csm_soa);
            arParams[20] = new SqlParameter("@replicates", _replicates);
            arParams[21] = new SqlParameter("@pxe", _pxe);
            arParams[22] = new SqlParameter("@ilo", _ilo);
            arParams[23] = new SqlParameter("@csm_vip", _csm_vip);
            arParams[24] = new SqlParameter("@ltm_web", _ltm_web);
            arParams[25] = new SqlParameter("@ltm_app", _ltm_app);
            arParams[26] = new SqlParameter("@ltm_middle", _ltm_middle);
            arParams[27] = new SqlParameter("@ltm_vip", _ltm_vip);
            arParams[28] = new SqlParameter("@windows_cluster", _windows_cluster);
            arParams[29] = new SqlParameter("@unix_cluster", _unix_cluster);
            arParams[30] = new SqlParameter("@accenture", _accenture);
            arParams[31] = new SqlParameter("@ha", _ha);
            arParams[32] = new SqlParameter("@sun_cluster", _sun_cluster);
            arParams[33] = new SqlParameter("@sun_sve", _sun_sve);
            arParams[34] = new SqlParameter("@storage", _storage);
            arParams[35] = new SqlParameter("@dell_web", _dell_web);
            arParams[36] = new SqlParameter("@dell_middleware", _dell_middleware);
            arParams[37] = new SqlParameter("@dell_database", _dell_database);
            arParams[38] = new SqlParameter("@dell_file", _dell_file);
            arParams[39] = new SqlParameter("@dell_misc", _dell_misc);
            arParams[40] = new SqlParameter("@dell_under48", _dell_under48);
            arParams[41] = new SqlParameter("@dell_avamar", _dell_avamar);
            arParams[42] = new SqlParameter("@switchname", _switchname);
            arParams[43] = new SqlParameter("@vtpdomain", _vtpdomain);
            arParams[44] = new SqlParameter("@classid", _classid);
            arParams[45] = new SqlParameter("@environmentid", _environmentid);
            arParams[46] = new SqlParameter("@addressid", _addressid);
            arParams[47] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[48] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_addVlan", arParams);
        }
        public void UpdateVlan(int _id, int _vlan, int _physical_windows, int _physical_unix, int _ecom_production, int _ecom_service, int _ipx, int _virtual_workstation, int _vmware_workstation_external, int _vmware_workstation_internal, int _vmware_workstation_dr, int _vmware_host, int _vmware_vmotion, int _vmware_windows, int _vmware_linux, int _blades_hp, int _blades_sun, int _apv, int _mainframe, int _csm, int _csm_soa, int _replicates, int _pxe, int _ilo, int _csm_vip, int _ltm_web, int _ltm_app, int _ltm_middle, int _ltm_vip, int _windows_cluster, int _unix_cluster, int _accenture, int _ha, int _sun_cluster, int _sun_sve, int _storage, int _dell_web, int _dell_middleware, int _dell_database, int _dell_file, int _dell_misc, int _dell_under48, int _dell_avamar, string _switchname, string _vtpdomain, int _classid, int _environmentid, int _addressid, int _resiliencyid, int _enabled)
        {
            arParams = new SqlParameter[50];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@vlan", _vlan);
            arParams[2] = new SqlParameter("@physical_windows", _physical_windows);
            arParams[3] = new SqlParameter("@physical_unix", _physical_unix);
            arParams[4] = new SqlParameter("@ecom_production", _ecom_production);
            arParams[5] = new SqlParameter("@ecom_service", _ecom_service);
            arParams[6] = new SqlParameter("@ipx", _ipx);
            arParams[7] = new SqlParameter("@virtual_workstation", _virtual_workstation);
            arParams[8] = new SqlParameter("@vmware_workstation_external", _vmware_workstation_external);
            arParams[9] = new SqlParameter("@vmware_workstation_internal", _vmware_workstation_internal);
            arParams[10] = new SqlParameter("@vmware_workstation_dr", _vmware_workstation_dr);
            arParams[11] = new SqlParameter("@vmware_host", _vmware_host);
            arParams[12] = new SqlParameter("@vmware_vmotion", _vmware_vmotion);
            arParams[13] = new SqlParameter("@vmware_windows", _vmware_windows);
            arParams[14] = new SqlParameter("@vmware_linux", _vmware_linux);
            arParams[15] = new SqlParameter("@blades_hp", _blades_hp);
            arParams[16] = new SqlParameter("@blades_sun", _blades_sun);
            arParams[17] = new SqlParameter("@apv", _apv);
            arParams[18] = new SqlParameter("@mainframe", _mainframe);
            arParams[19] = new SqlParameter("@csm", _csm);
            arParams[20] = new SqlParameter("@csm_soa", _csm_soa);
            arParams[21] = new SqlParameter("@replicates", _replicates);
            arParams[22] = new SqlParameter("@pxe", _pxe);
            arParams[23] = new SqlParameter("@ilo", _ilo);
            arParams[24] = new SqlParameter("@csm_vip", _csm_vip);
            arParams[25] = new SqlParameter("@ltm_web", _ltm_web);
            arParams[26] = new SqlParameter("@ltm_app", _ltm_app);
            arParams[27] = new SqlParameter("@ltm_middle", _ltm_middle);
            arParams[28] = new SqlParameter("@ltm_vip", _ltm_vip);
            arParams[29] = new SqlParameter("@windows_cluster", _windows_cluster);
            arParams[30] = new SqlParameter("@unix_cluster", _unix_cluster);
            arParams[31] = new SqlParameter("@accenture", _accenture);
            arParams[32] = new SqlParameter("@ha", _ha);
            arParams[33] = new SqlParameter("@sun_cluster", _sun_cluster);
            arParams[34] = new SqlParameter("@sun_sve", _sun_sve);
            arParams[35] = new SqlParameter("@storage", _storage);
            arParams[36] = new SqlParameter("@dell_web", _dell_web);
            arParams[37] = new SqlParameter("@dell_middleware", _dell_middleware);
            arParams[38] = new SqlParameter("@dell_database", _dell_database);
            arParams[39] = new SqlParameter("@dell_file", _dell_file);
            arParams[40] = new SqlParameter("@dell_misc", _dell_misc);
            arParams[41] = new SqlParameter("@dell_under48", _dell_under48);
            arParams[42] = new SqlParameter("@dell_avamar", _dell_avamar);
            arParams[43] = new SqlParameter("@switchname", _switchname);
            arParams[44] = new SqlParameter("@vtpdomain", _vtpdomain);
            arParams[45] = new SqlParameter("@classid", _classid);
            arParams[46] = new SqlParameter("@environmentid", _environmentid);
            arParams[47] = new SqlParameter("@addressid", _addressid);
            arParams[48] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[49] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_updateVlan", arParams);
        }
        public void EnableVlan(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_updateVlanEnabled", arParams);
        }
        public void DeleteVlan(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_deleteVlan", arParams);
        }

        public DataSet GetNetworks(int _vlanid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@vlanid", _vlanid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getNetworks", arParams);
        }
        public int GetNetwork(int _vlan, int _classid, int _environmentid, int _addressid, int _ip1, int _ip2, int _ip3, int _ip4)
        {
            int intNetwork = 0;
            DataSet ds = GetNetworks(GetVlan(_vlan, _classid, _environmentid, _addressid), 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["add1"].ToString()) == _ip1 && Int32.Parse(dr["add2"].ToString()) == _ip2 && Int32.Parse(dr["add3"].ToString()) == _ip3)
                {
                    int intMin = Int32.Parse(dr["min4"].ToString());
                    int intMax = Int32.Parse(dr["max4"].ToString());
                    if (_ip4 >= intMin && _ip4 <= intMax)
                    {
                        intNetwork = Int32.Parse(dr["id"].ToString());
                        break;
                    }
                }
            }
            return intNetwork;
        }
        public DataSet GetNetwork(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getNetwork", arParams);
        }
        public string GetNetworkNotifications(int _ipaddressid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@ipaddressid", _ipaddressid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getNetworkNotifications", arParams);
            string strNotify = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["notify"].ToString() != "")
                {
                    if (strNotify != "")
                        strNotify += ";";
                    strNotify += dr["notify"].ToString();
                    if (Get(_ipaddressid, "networkid") == "0")
                        UpdateNetwork(_ipaddressid, Int32.Parse(dr["id"].ToString()));
                }
            }
            return strNotify;
        }
        public string GetNetworkName(int _id)
        {
            DataSet ds = GetNetwork(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0]["add1"].ToString() + "." + ds.Tables[0].Rows[0]["add2"].ToString() + "." + ds.Tables[0].Rows[0]["add3"].ToString() + "." + ds.Tables[0].Rows[0]["min4"].ToString() + " - " + ds.Tables[0].Rows[0]["add1"].ToString() + "." + ds.Tables[0].Rows[0]["add2"].ToString() + "." + ds.Tables[0].Rows[0]["add3"].ToString() + "." + ds.Tables[0].Rows[0]["max4"].ToString();
            else
                return "";
        }
        public string GetNetwork(int _id, string _column)
        {
            DataSet ds = GetNetwork(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddNetwork(int _vlanid, int _add1, int _add2, int _add3, int _min4, int _max4, string _mask, string _gateway, int _starting, int _maximum, int _reverse, int _routable, string _notify, int _only_apps, int _only_components, string _description, int _enabled)
        {
            arParams = new SqlParameter[17];
            arParams[0] = new SqlParameter("@vlanid", _vlanid);
            arParams[1] = new SqlParameter("@add1", _add1);
            arParams[2] = new SqlParameter("@add2", _add2);
            arParams[3] = new SqlParameter("@add3", _add3);
            arParams[4] = new SqlParameter("@min4", _min4);
            arParams[5] = new SqlParameter("@max4", _max4);
            arParams[6] = new SqlParameter("@mask", _mask);
            arParams[7] = new SqlParameter("@gateway", _gateway);
            arParams[8] = new SqlParameter("@starting", _starting);
            arParams[9] = new SqlParameter("@maximum", _maximum);
            arParams[10] = new SqlParameter("@reverse", _reverse);
            arParams[11] = new SqlParameter("@routable", _routable);
            arParams[12] = new SqlParameter("@notify", _notify);
            arParams[13] = new SqlParameter("@only_apps", _only_apps);
            arParams[14] = new SqlParameter("@only_components", _only_components);
            arParams[15] = new SqlParameter("@description", _description);
            arParams[16] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_addNetwork", arParams);
        }
        public void UpdateNetwork(int _id, int _vlanid, int _add1, int _add2, int _add3, int _min4, int _max4, string _mask, string _gateway, int _starting, int _maximum, int _reverse, int _routable, string _notify, int _only_apps, int _only_components, string _description, int _enabled)
        {
            arParams = new SqlParameter[18];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@vlanid", _vlanid);
            arParams[2] = new SqlParameter("@add1", _add1);
            arParams[3] = new SqlParameter("@add2", _add2);
            arParams[4] = new SqlParameter("@add3", _add3);
            arParams[5] = new SqlParameter("@min4", _min4);
            arParams[6] = new SqlParameter("@max4", _max4);
            arParams[7] = new SqlParameter("@mask", _mask);
            arParams[8] = new SqlParameter("@gateway", _gateway);
            arParams[9] = new SqlParameter("@starting", _starting);
            arParams[10] = new SqlParameter("@maximum", _maximum);
            arParams[11] = new SqlParameter("@reverse", _reverse);
            arParams[12] = new SqlParameter("@routable", _routable);
            arParams[13] = new SqlParameter("@notify", _notify);
            arParams[14] = new SqlParameter("@only_apps", _only_apps);
            arParams[15] = new SqlParameter("@only_components", _only_components);
            arParams[16] = new SqlParameter("@description", _description);
            arParams[17] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_updateNetwork", arParams);
        }
        public void UpdateNetworkCluster(int _id, int _cluster_inuse)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@cluster_inuse", _cluster_inuse);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_updateNetworkCluster", arParams);
        }
        public void EnableNetwork(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_updateNetworkEnabled", arParams);
        }
        public void DeleteNetwork(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_deleteNetwork", arParams);
        }

        public DataSet GetDhcps(int _networkid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@networkid", _networkid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getDhcps", arParams);
        }
        public DataSet GetDhcp(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getDhcp", arParams);
        }
        public string GetDhcp(int _id, string _column)
        {
            DataSet ds = GetDhcp(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public string GetDhcpName(int _id)
        {
            DataSet ds = GetDhcp(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0]["add1"].ToString() + "." + ds.Tables[0].Rows[0]["add2"].ToString() + "." + ds.Tables[0].Rows[0]["add3"].ToString() + "." + ds.Tables[0].Rows[0]["min4"].ToString() + " - " + ds.Tables[0].Rows[0]["add1"].ToString() + "." + ds.Tables[0].Rows[0]["add2"].ToString() + "." + ds.Tables[0].Rows[0]["add3"].ToString() + "." + ds.Tables[0].Rows[0]["max4"].ToString();
            else
                return "";
        }
        public void AddDhcp(int _networkid, int _min4, int _max4, string _ips_notify, int _ips_left, int _enabled)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@networkid", _networkid);
            arParams[1] = new SqlParameter("@min4", _min4);
            arParams[2] = new SqlParameter("@max4", _max4);
            arParams[3] = new SqlParameter("@ips_notify", _ips_notify);
            arParams[4] = new SqlParameter("@ips_left", _ips_left);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_addDhcp", arParams);
        }
        public void UpdateDhcp(int _id, int _networkid, int _min4, int _max4, string _ips_notify, int _ips_left, int _enabled)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@networkid", _networkid);
            arParams[2] = new SqlParameter("@min4", _min4);
            arParams[3] = new SqlParameter("@max4", _max4);
            arParams[4] = new SqlParameter("@ips_notify", _ips_notify);
            arParams[5] = new SqlParameter("@ips_left", _ips_left);
            arParams[6] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_updateDhcp", arParams);
        }
        public void EnableDhcp(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_updateDhcpEnabled", arParams);
        }
        public void DeleteDhcp(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_deleteDhcp", arParams);
        }

        public DataSet GetVlansClasses(int _addressid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@addressid", _addressid);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getVlansClass", arParams);
        }
        public DataSet GetVlansEnvironments(int _addressid, int _classid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@addressid", _addressid);
            arParams[1] = new SqlParameter("@classid", _classid);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getVlansEnvironment", arParams);
        }

        public void AddNetworkRelation(int _networkid, int _related)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@networkid", _networkid);
            arParams[1] = new SqlParameter("@related", _related);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_addNetworkRelation", arParams);
        }
        public void DeleteNetworkRelation(int _networkid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@networkid", _networkid);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_deleteNetworkRelation", arParams);
        }
        public DataSet GetNetworkRelations(int _networkid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@networkid", _networkid);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getNetworkRelations", arParams);
        }


        public int Get_(int _classid, int _environmentid, int _addressid, int _physical_windows, int _physical_unix, int _ecom_production, int _ecom_service, int _ipx, int _virtual_workstation, int _vmware_workstation_external, int _vmware_workstation_internal, int _vmware_workstation_dr, int _vmware_host, int _vmware_vmotion, int _vmware_windows, int _vmware_linux, int _apv, int _csm, int _csm_soa, int _ilo, int _csm_vip, int _ltm_web, int _ltm_app, int _ltm_middle, int _ltm_vip, int _accenture, int _ha, int _sun_cluster, int _sun_sve, int _storage, bool _use_maximum, int _serverid, int _environment, string _dsn_service_editor)
        {
            arParams = new SqlParameter[31];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@physical_windows", _physical_windows);
            arParams[4] = new SqlParameter("@physical_unix", _physical_unix);
            arParams[5] = new SqlParameter("@ecom_production", _ecom_production);
            arParams[6] = new SqlParameter("@ecom_service", _ecom_service);
            arParams[7] = new SqlParameter("@ipx", _ipx);
            arParams[8] = new SqlParameter("@virtual_workstation", _virtual_workstation);
            arParams[9] = new SqlParameter("@vmware_workstation_external", _vmware_workstation_external);
            arParams[10] = new SqlParameter("@vmware_workstation_internal", _vmware_workstation_internal);
            arParams[11] = new SqlParameter("@vmware_workstation_dr", _vmware_workstation_dr);
            arParams[12] = new SqlParameter("@vmware_host", _vmware_host);
            arParams[13] = new SqlParameter("@vmware_vmotion", _vmware_vmotion);
            arParams[14] = new SqlParameter("@vmware_windows", _vmware_windows);
            arParams[15] = new SqlParameter("@vmware_linux", _vmware_linux);
            arParams[16] = new SqlParameter("@apv", _apv);
            arParams[17] = new SqlParameter("@csm", _csm);
            arParams[18] = new SqlParameter("@csm_soa", _csm_soa);
            arParams[19] = new SqlParameter("@ilo", _ilo);
            arParams[20] = new SqlParameter("@csm_vip", _csm_vip);
            arParams[21] = new SqlParameter("@ltm_web", _ltm_web);
            arParams[22] = new SqlParameter("@ltm_app", _ltm_app);
            arParams[23] = new SqlParameter("@ltm_middle", _ltm_middle);
            arParams[24] = new SqlParameter("@ltm_vip", _ltm_vip);
            arParams[25] = new SqlParameter("@accenture", _accenture);
            arParams[26] = new SqlParameter("@ha", _ha);
            arParams[27] = new SqlParameter("@sun_cluster", _sun_cluster);
            arParams[28] = new SqlParameter("@sun_sve", _sun_sve);
            arParams[29] = new SqlParameter("@storage", _storage);
            arParams[30] = new SqlParameter("@serverid", _serverid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddress_", arParams);
            return Get_Next(_classid, _environmentid, ds, _use_maximum, _environment, true, _dsn_service_editor);
        }
        public int Get_Dell(int _classid, int _environmentid, int _addressid, int _vmware_windows, int _vmware_linux, int _ltm_web, int _ltm_app, int _ltm_middle, int _ltm_vip, int _sun_sve, int _dell_web, int _dell_middleware, int _dell_database, int _dell_file, int _dell_misc, int _dell_under48, int _dell_avamar, bool _use_maximum, int _serverid, int _vmware_clusterid, int _resiliencyid, int _environment, string _dsn_service_editor)
        {
            arParams = new SqlParameter[20];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@vmware_windows", _vmware_windows);
            arParams[4] = new SqlParameter("@vmware_linux", _vmware_linux);
            arParams[5] = new SqlParameter("@ltm_web", _ltm_web);
            arParams[6] = new SqlParameter("@ltm_app", _ltm_app);
            arParams[7] = new SqlParameter("@ltm_middle", _ltm_middle);
            arParams[8] = new SqlParameter("@ltm_vip", _ltm_vip);
            arParams[9] = new SqlParameter("@sun_sve", _sun_sve);
            arParams[10] = new SqlParameter("@dell_web", _dell_web);
            arParams[11] = new SqlParameter("@dell_middleware", _dell_middleware);
            arParams[12] = new SqlParameter("@dell_database", _dell_database);
            arParams[13] = new SqlParameter("@dell_file", _dell_file);
            arParams[14] = new SqlParameter("@dell_misc", _dell_misc);
            arParams[15] = new SqlParameter("@dell_under48", _dell_under48);
            arParams[16] = new SqlParameter("@dell_avamar", _dell_avamar);
            arParams[17] = new SqlParameter("@serverid", _serverid);
            arParams[18] = new SqlParameter("@vmware_clusterid", _vmware_clusterid);
            arParams[19] = new SqlParameter("@resiliencyid", _resiliencyid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddress_Dell", arParams);
            return Get_Next(_classid, _environmentid, ds, _use_maximum, _environment, true, _dsn_service_editor);
        }
        public int Get_Blade_HP(int _classid, int _environmentid, int _addressid, int _ecom_production, int _ecom_service, int _vlan, int _networkid, int _ha, bool _use_maximum, int _serverid, int _resiliencyid, int _environment, string _dsn_service_editor)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@ecom_production", _ecom_production);
            arParams[4] = new SqlParameter("@ecom_service", _ecom_service);
            arParams[5] = new SqlParameter("@vlan", _vlan);
            arParams[6] = new SqlParameter("@networkid", _networkid);
            arParams[7] = new SqlParameter("@ha", _ha);
            arParams[8] = new SqlParameter("@serverid", _serverid);
            arParams[9] = new SqlParameter("@resiliencyid", _resiliencyid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddress_Blade_HP", arParams);
            return Get_Next(_classid, _environmentid, ds, _use_maximum, _environment, true, _dsn_service_editor);
        }
        public int Get_Blade_SUN(int _classid, int _environmentid, int _addressid, int _ecom_production, int _ecom_service, int _vlan, int _vlan_not, int _networkid, int _ha, bool _use_maximum, int _serverid, int _resiliencyid, int _environment, string _dsn_service_editor)
        {
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@ecom_production", _ecom_production);
            arParams[4] = new SqlParameter("@ecom_service", _ecom_service);
            arParams[5] = new SqlParameter("@vlan", _vlan);
            arParams[6] = new SqlParameter("@vlan_not", _vlan_not);
            arParams[7] = new SqlParameter("@networkid", _networkid);
            arParams[8] = new SqlParameter("@ha", _ha);
            arParams[9] = new SqlParameter("@serverid", _serverid);
            arParams[10] = new SqlParameter("@resiliencyid", _resiliencyid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddress_Blade_SUN", arParams);
            return Get_Next(_classid, _environmentid, ds, _use_maximum, _environment, true, _dsn_service_editor);
        }
        public int Get_VLAN(int _classid, int _environmentid, int _addressid, int _ecom_production, int _ecom_service, int _vlan, int _networkid, int _csm, int _csm_soa, int _ltm_web, int _ltm_app, int _ltm_middle, int _ha, bool _use_maximum, int _serverid, int _vmware_host, int _vmware_vmotion, int _resiliencyid, int _environment, string _dsn_service_editor)
        {
            arParams = new SqlParameter[17];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@ecom_production", _ecom_production);
            arParams[4] = new SqlParameter("@ecom_service", _ecom_service);
            arParams[5] = new SqlParameter("@vlan", _vlan);
            arParams[6] = new SqlParameter("@networkid", _networkid);
            arParams[7] = new SqlParameter("@csm", _csm);
            arParams[8] = new SqlParameter("@csm_soa", _csm_soa);
            arParams[9] = new SqlParameter("@ltm_web", _ltm_web);
            arParams[10] = new SqlParameter("@ltm_app", _ltm_app);
            arParams[11] = new SqlParameter("@ltm_middle", _ltm_middle);
            arParams[12] = new SqlParameter("@ha", _ha);
            arParams[13] = new SqlParameter("@serverid", _serverid);
            arParams[14] = new SqlParameter("@vmware_host", _vmware_host);
            arParams[15] = new SqlParameter("@vmware_vmotion", _vmware_vmotion);
            arParams[16] = new SqlParameter("@resiliencyid", _resiliencyid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddress_VLAN", arParams);
            return Get_Next(_classid, _environmentid, ds, _use_maximum, _environment, true, _dsn_service_editor);
        }
        public int Get_VLANExclude(int _classid, int _environmentid, int _addressid, int _ecom_production, int _ecom_service, int _vlan, int _networkid, int _csm, int _csm_soa, int _ltm_web, int _ltm_app, int _ltm_middle, bool _use_maximum, int _environment, string _dsn_service_editor)
        {
            arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@ecom_production", _ecom_production);
            arParams[4] = new SqlParameter("@ecom_service", _ecom_service);
            arParams[5] = new SqlParameter("@vlan", _vlan);
            arParams[6] = new SqlParameter("@networkid", _networkid);
            arParams[7] = new SqlParameter("@csm", _csm);
            arParams[8] = new SqlParameter("@csm_soa", _csm_soa);
            arParams[9] = new SqlParameter("@ltm_web", _ltm_web);
            arParams[10] = new SqlParameter("@ltm_app", _ltm_app);
            arParams[11] = new SqlParameter("@ltm_middle", _ltm_middle);
            DataSet ds = SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddress_VLANExclude", arParams);
            return Get_Next(_classid, _environmentid, ds, _use_maximum, _environment, true, _dsn_service_editor);
        }
        public int Get_VLANExcludeNoRoute(int _classid, int _environmentid, int _addressid, int _ecom_production, int _ecom_service, int _vlan, int _networkid, int _csm, int _csm_soa, int _ltm_web, int _ltm_app, int _ltm_middle, bool _use_maximum, int _environment, string _dsn_service_editor)
        {
            arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@ecom_production", _ecom_production);
            arParams[4] = new SqlParameter("@ecom_service", _ecom_service);
            arParams[5] = new SqlParameter("@vlan", _vlan);
            arParams[6] = new SqlParameter("@networkid", _networkid);
            arParams[7] = new SqlParameter("@csm", _csm);
            arParams[8] = new SqlParameter("@csm_soa", _csm_soa);
            arParams[9] = new SqlParameter("@ltm_web", _ltm_web);
            arParams[10] = new SqlParameter("@ltm_app", _ltm_app);
            arParams[11] = new SqlParameter("@ltm_middle", _ltm_middle);
            DataSet ds = SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddress_VLANExcludeNoRoute", arParams);
            return Get_Next(_classid, _environmentid, ds, _use_maximum, _environment, true, _dsn_service_editor);
        }
        public int Get_Network(int _classid, int _environmentid, int _addressid, int _ecom_production, int _ecom_service, int _networkid, bool _use_maximum, int _environment, string _dsn_service_editor)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@ecom_production", _ecom_production);
            arParams[4] = new SqlParameter("@ecom_service", _ecom_service);
            arParams[5] = new SqlParameter("@networkid", _networkid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddress_Network", arParams);
            return Get_Next(_classid, _environmentid, ds, _use_maximum, _environment, true, _dsn_service_editor);
        }
        public int Get_Related(int _classid, int _environmentid, int _addressid, int _related, bool _use_maximum, int _environment, string _dsn_service_editor)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@related", _related);
            DataSet ds = SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddress_Related", arParams);
            return Get_Next(_classid, _environmentid, ds, _use_maximum, _environment, true, _dsn_service_editor);
        }
        public int Get_ClusterNetwork(int _classid, int _environmentid, int _addressid, int _ecom_production, int _ecom_service, int _windows_cluster, int _unix_cluster, int _count, int _not_vlan, bool _use_maximum, string _dsn_service_editor)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@ecom_production", _ecom_production);
            arParams[4] = new SqlParameter("@ecom_service", _ecom_service);
            arParams[5] = new SqlParameter("@windows_cluster", _windows_cluster);
            arParams[6] = new SqlParameter("@unix_cluster", _unix_cluster);
            arParams[7] = new SqlParameter("@not_vlan", _not_vlan);
            DataSet ds = SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddress_ClusterNetwork", arParams);
            int intCounter = 0;
            int intNetwork = 0;
            AddResult("Based on the query, there were " + ds.Tables[0].Rows.Count.ToString() + " available private networks");
            // Check that there are enough IPs in the range...
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intPrevious = 0;
                for (intCounter = 0; intCounter < _count && intNetwork == 0; intCounter++)
                {
                    int intID = Get_Next(_classid, _environmentid, Int32.Parse(dr["id"].ToString()), Int32.Parse(dr["starting"].ToString()), Int32.Parse(dr["add1"].ToString()), Int32.Parse(dr["add2"].ToString()), Int32.Parse(dr["add3"].ToString()), Int32.Parse(dr["min4"].ToString()), Int32.Parse(dr["max4"].ToString()), Int32.Parse(dr["maximum"].ToString()), (dr["reverse"].ToString() == "1"), 0, false, _use_maximum, false, _dsn_service_editor);
                    if (intID == 0)
                        break;
                }
                if (intCounter == _count)
                {
                    // Enough IPs were found, set the network.
                    intNetwork = Int32.Parse(dr["id"].ToString());
                    break;
                }
            }
            if (intNetwork == 0)
            {
                // Not enough IPs were found, check recycled addresses
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    intPrevious = 0;
                    for (intCounter = 0; intCounter < _count && intNetwork == 0; intCounter++)
                    {
                        int intID = Get_Next(_classid, _environmentid, Int32.Parse(dr["id"].ToString()), Int32.Parse(dr["starting"].ToString()), Int32.Parse(dr["add1"].ToString()), Int32.Parse(dr["add2"].ToString()), Int32.Parse(dr["add3"].ToString()), Int32.Parse(dr["min4"].ToString()), Int32.Parse(dr["max4"].ToString()), Int32.Parse(dr["maximum"].ToString()), (dr["reverse"].ToString() == "1"), 1, false, _use_maximum, false, _dsn_service_editor);
                        if (intID == 0)
                            break;
                    }
                    if (intCounter == _count)
                    {
                        intNetwork = Int32.Parse(dr["id"].ToString());
                        break;
                    }
                }
            }
            return intNetwork;
        }
        public int Get_Cluster(int _classid, int _environmentid, int _addressid, int _ecom_production, int _ecom_service, int _windows_cluster, int _unix_cluster, int _networkid, bool _use_maximum, int _environment, string _dsn_service_editor)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@ecom_production", _ecom_production);
            arParams[4] = new SqlParameter("@ecom_service", _ecom_service);
            arParams[5] = new SqlParameter("@windows_cluster", _windows_cluster);
            arParams[6] = new SqlParameter("@unix_cluster", _unix_cluster);
            arParams[7] = new SqlParameter("@networkid", _networkid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getAddress_Cluster", arParams);
            return Get_Next(_classid, _environmentid, ds, _use_maximum, _environment, false, _dsn_service_editor);
        }
        private int Get_Next(int _classid, int _environmentid, DataSet ds, bool _use_maximum, int _environment, bool _routable_network, string _dsn_service_editor)
        {
            AddResult("Based on the query, there were " + ds.Tables[0].Rows.Count.ToString() + " available ranges");
            int intID = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intID = Get_Next(_classid, _environmentid, Int32.Parse(dr["id"].ToString()), Int32.Parse(dr["starting"].ToString()), Int32.Parse(dr["add1"].ToString()), Int32.Parse(dr["add2"].ToString()), Int32.Parse(dr["add3"].ToString()), Int32.Parse(dr["min4"].ToString()), Int32.Parse(dr["max4"].ToString()), Int32.Parse(dr["maximum"].ToString()), (dr["reverse"].ToString() == "1"), 0, true, _use_maximum, _routable_network, _dsn_service_editor);
                if (intID > 0)
                    break;
            }
            if (intID == 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    intID = Get_Next(_classid, _environmentid, Int32.Parse(dr["id"].ToString()), Int32.Parse(dr["starting"].ToString()), Int32.Parse(dr["add1"].ToString()), Int32.Parse(dr["add2"].ToString()), Int32.Parse(dr["add3"].ToString()), Int32.Parse(dr["min4"].ToString()), Int32.Parse(dr["max4"].ToString()), Int32.Parse(dr["maximum"].ToString()), (dr["reverse"].ToString() == "1"), 1, true, _use_maximum, _routable_network, _dsn_service_editor);
                    if (intID > 0)
                        break;
                }
            }

            Settings oSetting = new Settings(0, dsn);
            int intLeft = 0;
            if (Int32.TryParse(oSetting.Get("ips_left"), out intLeft) == true)
            {
                if (intLeft > 0)
                {
                    int intNetwork = 0;
                    string strRange = "";
                    string strRangeMinMax = "";
                    int intTotal = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        intNetwork = Int32.Parse(dr["id"].ToString());
                        strRange = dr["add1"].ToString() + "." + dr["add2"].ToString() + "." + dr["add3"].ToString() + ".xxx";
                        if (strRangeMinMax != "")
                            strRangeMinMax += ", ";
                        strRangeMinMax += dr["min4"].ToString() + " - " + dr["max4"].ToString();
                        intTotal += Get_Count(_classid, _environmentid, Int32.Parse(dr["id"].ToString()), Int32.Parse(dr["starting"].ToString()), Int32.Parse(dr["add1"].ToString()), Int32.Parse(dr["add2"].ToString()), Int32.Parse(dr["add3"].ToString()), Int32.Parse(dr["min4"].ToString()), Int32.Parse(dr["max4"].ToString()), Int32.Parse(dr["maximum"].ToString()), (dr["reverse"].ToString() == "1"), _use_maximum, _routable_network);
                    }

                    if (intTotal < intLeft && strRange.Trim() != "")
                    {
                        // Send notification
                        int intVLAN = 0;
                        Int32.TryParse(GetNetwork(intNetwork, "vlanid"), out intVLAN);
                        Functions oFunction = new Functions(user, dsn, _environment);
                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_IPADDRESS");
                        oFunction.SendEmail("WARNING: IP Addresses", strEMailIdsBCC, "", "", "WARNING: IP Addresses", "<p><b>This message is to inform you that you are running out of IP addresses...</b></p><p>VLAN: " + GetVlan(intVLAN, "vlan") + "<br/>Subnet: " + strRange + "<br/>Current Range(s): " + strRangeMinMax + "</p><p>There are less than " + intLeft.ToString() + " IP addresses available (" + intTotal.ToString() + " address(es) left).</p>", true, false);
                    }
                }
            }

            return intID;
        }
        private int Get_Next(int _classid, int _environmentid, int _networkid, int _starting, int _add1, int _add2, int _add3, int _min4, int _max4, int _maximum, bool _reverse, int _available, bool _add, bool _use_maximum, bool _routable_network, string _dsn_service_editor)
        {
            int intID = 0;
            int intCount = 0;
            AddResult("Checking IPs... (Class = " + _classid + ", Environment = " + _environmentid + ", NetworkID = " + _networkid.ToString() + ", Starting = " + _starting.ToString() + ", Subnet = " + _add1.ToString() + "." + _add2.ToString() + "." + _add3.ToString() + "." + _min4.ToString() + " - " + _max4.ToString() + ", Maximum = " + _maximum.ToString() + ", Reverse = " + _reverse.ToString() + ", Available = " + _available.ToString() + ", UseMax = " + _use_maximum.ToString() + ")");
            if (_reverse == false)
            {
                for (int ii = _starting; (intCount < _maximum || _use_maximum == false) && ii <= _max4 && intID == 0; ii++)
                {
                    intCount++;
                    DataSet dsIP = Get(_add1, _add2, _add3, ii, _available);
                    DataView dvIP = FilterNonRoutable(dsIP, _routable_network, _networkid, _add1, _add2, _add3, ii, _available);
                    if (_add == true)
                    {
                        if (_available == 0)
                        {
                            if (dvIP.Count == 0)
                            {
                                // Add and Attempt to PING
                                intID = Add(_networkid, _add1, _add2, _add3, ii, 0);
                                AddResult("...Checking Address " + intID.ToString() + " = " + _add1.ToString() + "." + _add2.ToString() + "." + _add3.ToString() + "." + ii.ToString() + " (" + _available.ToString() + ")");
                                Functions oFunction = new Functions(user, dsn, 0);
                                string strNewIP = GetName(intID);
                                if (Manual(strNewIP, _dsn_service_editor) == true)
                                {
                                    AddResult("......Manual Provisioning...record added, moving on...");
                                    intID = 0;
                                }
                                else if (oFunction.Ping(strNewIP, _classid, _environmentid) == true)
                                {
                                    AddResult("......Ping Successful...record added, moving on...");
                                    intID = 0;
                                }
                            }
                            else
                                AddResult("...Skipping Address : One or more records returned (" + _available.ToString() + ") = " + _add1.ToString() + "." + _add2.ToString() + "." + _add3.ToString() + "." + ii.ToString());
                        }
                        else if (_available == 1)
                        {
                            if (dvIP.Count > 0)
                            {
                                // Get and Attempt to PING
                                intID = Int32.Parse(dsIP.Tables[0].Rows[0]["id"].ToString());
                                AddResult("...Checking Address " + intID.ToString() + " = " + _add1.ToString() + "." + _add2.ToString() + "." + _add3.ToString() + "." + ii.ToString() + " (" + _available.ToString() + " = " + dsIP.Tables[0].Rows[0]["available"].ToString() + ")");
                                UpdateAvailable(intID, 0);
                                Functions oFunction = new Functions(user, dsn, 0);
                                string strNewIP = GetName(intID);
                                if (Manual(strNewIP, _dsn_service_editor) == true)
                                {
                                    AddResult("......Manual Provisioning...update available flag, moving on...");
                                    intID = 0;
                                }
                                else if (oFunction.Ping(strNewIP, _classid, _environmentid) == true)
                                {
                                    AddResult("......Ping Successful...update available flag, moving on...");
                                    intID = 0;
                                }
                                else
                                {
                                    // Update Network to NetworkID of selected environment
                                    UpdateNetwork(intID, _networkid);
                                }
                            }
                            else
                                AddResult("...Skipping Address : No records returned (" + _available.ToString() + ") = " + _add1.ToString() + "." + _add2.ToString() + "." + _add3.ToString() + "." + ii.ToString());
                        }
                        else
                            AddResult("...Skipping Address : Invalid Available (" + _available.ToString() + ") = " + _add1.ToString() + "." + _add2.ToString() + "." + _add3.ToString() + "." + ii.ToString());
                    }
                    else
                    {
                        if (ii > intPrevious && dvIP.Count == 0)
                        {
                            intPrevious = ii;
                            intID = 1;
                        }
                    }
                }
            }
            else
            {
                for (int ii = _starting; (intCount < _maximum || _use_maximum == false) && ii >= _min4 && intID == 0; ii--)
                {
                    intCount++;
                    DataSet dsIP = Get(_add1, _add2, _add3, ii, _available);
                    DataView dvIP = FilterNonRoutable(dsIP, _routable_network, _networkid, _add1, _add2, _add3, ii, _available);
                    if (_add == true)
                    {
                        if (_available == 0)
                        {
                            if (dvIP.Count == 0)
                            {
                                // Add and Attempt to PING
                                intID = Add(_networkid, _add1, _add2, _add3, ii, 0);
                                AddResult("...Checking Address " + intID.ToString() + " = " + _add1.ToString() + "." + _add2.ToString() + "." + _add3.ToString() + "." + ii.ToString() + " (" + _available.ToString() + ")");
                                Functions oFunction = new Functions(user, dsn, 0);
                                string strNewIP = GetName(intID);
                                if (Manual(strNewIP, _dsn_service_editor) == true)
                                {
                                    AddResult("......Manual Provisioning...record added, moving on...");
                                    intID = 0;
                                }
                                else if (oFunction.Ping(strNewIP, _classid, _environmentid) == true)
                                {
                                    AddResult("......Ping Successful...record added, moving on...");
                                    intID = 0;
                                }
                            }
                            else
                                AddResult("...Skipping Address : One or more records returned (" + _available.ToString() + ") = " + _add1.ToString() + "." + _add2.ToString() + "." + _add3.ToString() + "." + ii.ToString());
                        }
                        else if (_available == 1)
                        {
                            if (dvIP.Count > 0)
                            {
                                // Get and Attempt to PING
                                intID = Int32.Parse(dsIP.Tables[0].Rows[0]["id"].ToString());
                                AddResult("...Checking Address " + intID.ToString() + " = " + _add1.ToString() + "." + _add2.ToString() + "." + _add3.ToString() + "." + ii.ToString() + " (" + _available.ToString() + " = " + dsIP.Tables[0].Rows[0]["available"].ToString() + ")");
                                UpdateAvailable(intID, 0);
                                Functions oFunction = new Functions(user, dsn, 0);
                                string strNewIP = GetName(intID);
                                if (Manual(strNewIP, _dsn_service_editor) == true)
                                {
                                    AddResult("......Manual Provisioning...update available flag, moving on...");
                                    intID = 0;
                                }
                                else if (oFunction.Ping(strNewIP, _classid, _environmentid) == true)
                                {
                                    AddResult("......Ping Successful...update available flag, moving on...");
                                    intID = 0;
                                }
                                else
                                {
                                    // Update Network to NetworkID of selected environment
                                    UpdateNetwork(intID, _networkid);
                                }
                            }
                            else
                                AddResult("...Skipping Address : No records returned (" + _available.ToString() + ") = " + _add1.ToString() + "." + _add2.ToString() + "." + _add3.ToString() + "." + ii.ToString());
                        }
                        else
                            AddResult("...Skipping Address : Invalid Available (" + _available.ToString() + ") = " + _add1.ToString() + "." + _add2.ToString() + "." + _add3.ToString() + "." + ii.ToString());
                    }
                    else
                    {
                        if (ii < intPrevious && dvIP.Count == 0)
                        {
                            intPrevious = ii;
                            intID = 1;
                        }
                    }
                }
            }
            return intID;
        }
        private bool Manual(string _ip, string _dsn_service_editor)
        {
            ServiceEditor oServiceEditor = new ServiceEditor(0, _dsn_service_editor);
            DataSet dsManual = oServiceEditor.APGetIPs();
            DataView dvManual = dsManual.Tables[0].DefaultView;
            dvManual.RowFilter = "ip = '" + _ip + "'";
            return (dvManual.Count > 0);
        }
        private DataView FilterNonRoutable(DataSet _ds, bool _routable_network, int _networkid, int _add1, int _add2, int _add3, int _add4, int _available)
        {
            DataView dvIP = _ds.Tables[0].DefaultView;
            if (_routable_network == false)
            {
                AddResult("...Checking Non-routable Address = " + _add1.ToString() + "." + _add2.ToString() + "." + _add3.ToString() + "." + _add4.ToString() + " (" + _available.ToString() + ")");
                // Get rid of those addresses that are NOT on this NETWORKID (since the addresses are not routable, they are only available within the subnet / NETWORKID)
                foreach (DataRowView drIP in dvIP)
                {
                    int intTempNetworkID = 0;
                    if (Int32.TryParse(drIP["networkid"].ToString(), out intTempNetworkID) == true && intTempNetworkID > 0)
                    {
                        if (_networkid != intTempNetworkID)
                        {
                            AddResult("......deleted, network is different.");
                            drIP.Delete();
                        }
                    }
                    else
                    {
                        // Network not found...delete.
                        drIP.Delete();
                        AddResult("......deleted, network not found.");
                    }
                }
                AddResult("......address count = " + dvIP.Count.ToString());
            }
            return dvIP;
        }
        private int Get_Count(int _classid, int _environmentid, int _networkid, int _starting, int _add1, int _add2, int _add3, int _min4, int _max4, int _maximum, bool _reverse, bool _use_maximum, bool _routable_network)
        {
            int intTotal = 0;
            int intCount = 0;
            Functions oFunction = new Functions(user, dsn, 0);
            if (_reverse == false)
            {
                for (int ii = _starting; (intCount < _maximum || _use_maximum == false) && ii <= _max4; ii++)
                {
                    int _available = 0;
                    intCount++;
                    DataSet dsIP = Get(_add1, _add2, _add3, ii);
                    DataView dvIP = FilterNonRoutable(dsIP, _routable_network, _networkid, _add1, _add2, _add3, ii, 0);
                    if (dvIP.Count > 0)
                        Int32.TryParse(dvIP[0]["available"].ToString(), out _available);
                    else
                    {
                        // Record does not exist
                        _available = 0;
                    }
                    string strName = _add1.ToString() + "." + _add2.ToString() + "." + _add3.ToString() + "." + ii.ToString();
                    if (_available == 0)
                    {
                        if (dvIP.Count == 0)
                        {
                            // IP does not exist - Add and Attempt to PING
                            //if (oFunction.Ping(strName, _classid, _environmentid) == false)
                                intTotal++;
                        }
                    }
                    else if (_available == 1)
                    {
                        if (dvIP.Count > 0)
                        {
                            // IP exists but is available - get and Attempt to PING
                            //if (oFunction.Ping(strName, _classid, _environmentid) == false)
                                intTotal++;
                        }
                    }
                }
            }
            else
            {
                for (int ii = _starting; (intCount < _maximum || _use_maximum == false) && ii >= _min4; ii--)
                {
                    int _available = 0;
                    intCount++;
                    DataSet dsIP = Get(_add1, _add2, _add3, ii);
                    DataView dvIP = FilterNonRoutable(dsIP, _routable_network, _networkid, _add1, _add2, _add3, ii, 0);
                    if (dvIP.Count > 0)
                        Int32.TryParse(dvIP[0]["available"].ToString(), out _available);
                    else
                    {
                        // Record does not exist
                        _available = 0;
                    }
                    string strName = _add1.ToString() + "." + _add2.ToString() + "." + _add3.ToString() + "." + ii.ToString();
                    if (_available == 0)
                    {
                        if (dvIP.Count == 0)
                        {
                            // IP does not exist - Add and Attempt to PING
                            //if (oFunction.Ping(strName, _classid, _environmentid) == false)
                                intTotal++;
                        }
                    }
                    else if (_available == 1)
                    {
                        if (dvIP.Count > 0)
                        {
                            // IP exists but is available - get and Attempt to PING
                            //if (oFunction.Ping(strName, _classid, _environmentid) == false)
                                intTotal++;
                        }
                    }
                }
            }
            return intTotal;
        }
        public string Results()
        {
            return strResults;
        }
        private void AddResult(string _result)
        {
            if (strResults != "")
                strResults += "<br/>";
            strResults += _result;
        }
        public void ClearResults()
        {
            strResults = "";
        }


        public DataSet GetNetwork_(int _classid, int _environmentid, int _addressid, int _physical_windows, int _physical_unix, int _ecom_production, int _ecom_service, int _ipx, int _virtual_workstation, int _vmware_workstation_external, int _vmware_workstation_internal, int _vmware_workstation_dr, int _vmware_host, int _vmware_vmotion, int _vmware_windows, int _vmware_linux, int _apv, int _csm, int _csm_soa, int _ilo, int _csm_vip, int _ltm_web, int _ltm_app, int _ltm_middle, int _ltm_vip, int _accenture, int _ha, int _sun_cluster, int _storage, int _environment)
        {
            arParams = new SqlParameter[29];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@physical_windows", _physical_windows);
            arParams[4] = new SqlParameter("@physical_unix", _physical_unix);
            arParams[5] = new SqlParameter("@ecom_production", _ecom_production);
            arParams[6] = new SqlParameter("@ecom_service", _ecom_service);
            arParams[7] = new SqlParameter("@ipx", _ipx);
            arParams[8] = new SqlParameter("@virtual_workstation", _virtual_workstation);
            arParams[9] = new SqlParameter("@vmware_workstation_external", _vmware_workstation_external);
            arParams[10] = new SqlParameter("@vmware_workstation_internal", _vmware_workstation_internal);
            arParams[11] = new SqlParameter("@vmware_workstation_dr", _vmware_workstation_dr);
            arParams[12] = new SqlParameter("@vmware_host", _vmware_host);
            arParams[13] = new SqlParameter("@vmware_vmotion", _vmware_vmotion);
            arParams[14] = new SqlParameter("@vmware_windows", _vmware_windows);
            arParams[15] = new SqlParameter("@vmware_linux", _vmware_linux);
            arParams[16] = new SqlParameter("@apv", _apv);
            arParams[17] = new SqlParameter("@csm", _csm);
            arParams[18] = new SqlParameter("@csm_soa", _csm_soa);
            arParams[19] = new SqlParameter("@ilo", _ilo);
            arParams[20] = new SqlParameter("@csm_vip", _csm_vip);
            arParams[21] = new SqlParameter("@ltm_web", _ltm_web);
            arParams[22] = new SqlParameter("@ltm_app", _ltm_app);
            arParams[23] = new SqlParameter("@ltm_middle", _ltm_middle);
            arParams[24] = new SqlParameter("@ltm_vip", _ltm_vip);
            arParams[25] = new SqlParameter("@accenture", _accenture);
            arParams[26] = new SqlParameter("@ha", _ha);
            arParams[27] = new SqlParameter("@sun_cluster", _sun_cluster);
            arParams[28] = new SqlParameter("@storage", _storage);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getNetwork_", arParams);
        }

        public string GetDescription(int _ipaddressid, string _name, int _assetid, string _dsn_asset, string _prefix, int _environment)
        {
            string strReturn = "";
            if (_ipaddressid > 0)
            {
                Asset oAsset = new Asset(0, _dsn_asset);
                Classes oClass = new Classes(0, dsn);
                ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
                int intNetwork = Int32.Parse(Get(_ipaddressid, "networkid"));
                int intVLAN = 0;
                Int32.TryParse(GetNetwork(intNetwork, "vlanid"), out intVLAN);
                string strVLAN = GetVlan(intVLAN, "vlan");
                int intModel = Int32.Parse(oAsset.Get(_assetid, "modelid"));
                int intEnclosure = 0;
                if (oAsset.GetServerOrBlade(_assetid, "enclosureid") != "")
                    intEnclosure = Int32.Parse(oAsset.GetServerOrBlade(_assetid, "enclosureid"));
                string strRoom = oAsset.GetServerOrBlade(_assetid, "room");
                string strRack = oAsset.GetServerOrBlade(_assetid, "rack");
                string strEnclosure = oAsset.GetEnclosure(intEnclosure, "name");
                string strBay = oAsset.GetServerOrBlade(_assetid, "slot");
                string strRAM = oModelsProperties.Get(intModel, "ram");
                string strIP = GetName(_ipaddressid, 0);
                if (intEnclosure > 0)
                {
                    // Format: <name> - <rack> - <enclosure> - Bay <slot> - <ram> Gig <ipaddress> VL<vlan>
                    strReturn = _name.ToLower() + (_prefix == "" ? "" : "-" + _prefix) + " - " + strRack.ToUpper() + " - " + strEnclosure.ToLower() + " - Bay " + strBay + " - " + strRAM + " Gig " + strIP + " VL" + strVLAN;
                }
                else
                {
                    // Format: <name> - Room <room> - <ipaddress> - VL<vlan>
                    strReturn = _name.ToLower() + (_prefix == "" ? "" : "-" + _prefix) + (strRoom == "" ? "" : " - Room " + strRoom.ToUpper()) + " - " + strIP + " - " + "VL" + strVLAN;
                }
            }
            return strReturn;
        }

        /*
        public string SendNotificationIPClass(int _ipaddressid, int _classid, string _name, int _assetid, string _dsn_asset, string _prefix, int _environment, bool _actually_send)
        {
            string strIPEmail = "";
            if (_ipaddressid > 0)
            {
                Asset oAsset = new Asset(0, _dsn_asset);
                Classes oClass = new Classes(0, dsn);
                ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
                int intNetwork = Int32.Parse(Get(_ipaddressid, "networkid"));
                if (intNetwork == 0)
                {
                    // Run this to hopefully update the networkID if 0
                    GetNetworkNotifications(_ipaddressid);
                    intNetwork = Int32.Parse(Get(_ipaddressid, "networkid"));
                }
                int intVLAN = 0;
                Int32.TryParse(GetNetwork(intNetwork, "vlanid"), out intVLAN);
                string strVLAN = GetVlan(intVLAN, "vlan");
                if (oClass.Get(_classid, "prod") == "1" || oClass.Get(_classid, "qa") == "1")
                {
                    string strNotify = GetNetwork(intNetwork, "notify");
                    if (strNotify.Trim() != "")
                    {
                        int intModel = Int32.Parse(oAsset.Get(_assetid, "modelid"));
                        int intEnclosure = 0;
                        if (oAsset.GetServerOrBlade(_assetid, "enclosureid") != "")
                            intEnclosure = Int32.Parse(oAsset.GetServerOrBlade(_assetid, "enclosureid"));
                        if (intEnclosure > 0)
                            strIPEmail = SendNotification(strNotify, _name, true, oAsset.GetServerOrBlade(_assetid, "room"), oAsset.GetServerOrBlade(_assetid, "rack"), oAsset.GetEnclosure(intEnclosure, "name"), oAsset.GetServerOrBlade(_assetid, "slot"), oModelsProperties.Get(intModel, "ram"), GetName(_ipaddressid, 0), strVLAN, _prefix, "", _environment, _actually_send);
                        else
                            strIPEmail = SendNotification(strNotify, _name, false, oAsset.GetServerOrBlade(_assetid, "room"), oAsset.GetServerOrBlade(_assetid, "rack"), "", "", oModelsProperties.Get(intModel, "ram"), GetName(_ipaddressid, 0), strVLAN, _prefix, "", _environment, _actually_send);
                    }
                }
            }
            return strIPEmail;
        }
        public string SendNotification(string _notify, string _name, bool _blade, string _room, string _rack, string _enclosure, string _bay, string _ram, string _ip, string _vlan, string _prefix, string _suffix, int _environment, bool _actually_send)
        {
            Functions oFunction = new Functions(0, dsn, _environment);
            string strIPEmail = "";
            if (_blade == true)
            {
                // Format: <name> - <rack> - <enclosure> - Bay <slot> - <ram> Gig <ipaddress> VL<vlan>
                strIPEmail = _name.ToLower() + (_prefix == "" ? "" : "-" + _prefix) + " - " + _rack.ToUpper() + " - " + _enclosure.ToLower() + " - Bay " + _bay + " - " + _ram + " Gig " + _ip + " VL" + _vlan;
            }
            else
            {
                // Format: <name> - Room <room> - <ipaddress> - VL<vlan>
                strIPEmail = _name.ToLower() + (_prefix == "" ? "" : "-" + _prefix) + (_room == "" ? "" : " - Room " + _room.ToUpper()) + " - " + _ip + " - " + "VL" + _vlan;
            }
            if (_actually_send == true)
            {
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_IPADDRESS");
                oFunction.SendEmail("IP Assignment", _notify, "", strEMailIdsBCC, "IP Assignment" + (_suffix == "" ? "" : " [" + _suffix + "]"), strIPEmail, false, true);
            }
            return strIPEmail;
        }
        public string SendNotificationIPModel(int _ipaddressid, int _modelid, string _notify, string _name, string _room, string _rack, string _enclosure, string _slot, string _prefix, string _suffix, int _environment)
        {
            if (_ipaddressid > 0)
            {
                ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
                int intNetwork = Int32.Parse(Get(_ipaddressid, "networkid"));
                if (intNetwork == 0)
                {
                    // Run this to hopefully update the networkID if 0
                    GetNetworkNotifications(_ipaddressid);
                    intNetwork = Int32.Parse(Get(_ipaddressid, "networkid"));
                }
                if (_notify == "")
                    _notify = GetNetwork(intNetwork, "notify");
                int intVLAN = 0;
                Int32.TryParse(GetNetwork(intNetwork, "vlanid"), out intVLAN);
                string strVLAN = GetVlan(intVLAN, "vlan");
                return SendNotification(_notify, _name, (_enclosure.Trim() != ""), _room, _rack, _enclosure, _slot, oModelsProperties.Get(_modelid, "ram"), GetName(_ipaddressid, 0), strVLAN, _prefix, _suffix, _environment, true);
            }
            else
                return "";
        }
        public string SendNotificationIPRelease(int _ipaddressid, int _classid, string _name, int _environment)
        {
            string strIPEmail = "";
            if (_ipaddressid > 0)
            {
                Classes oClass = new Classes(0, dsn);
                int intNetwork = Int32.Parse(Get(_ipaddressid, "networkid"));
                if (intNetwork == 0)
                {
                    // Run this to hopefully update the networkID if 0
                    GetNetworkNotifications(_ipaddressid);
                    intNetwork = Int32.Parse(Get(_ipaddressid, "networkid"));
                }
                int intVLAN = 0;
                Int32.TryParse(GetNetwork(intNetwork, "vlanid"), out intVLAN);
                string strVLAN = GetVlan(intVLAN, "vlan");
                if (oClass.Get(_classid, "prod") == "1" || oClass.Get(_classid, "qa") == "1")
                {
                    string strNotify = GetNetwork(intNetwork, "notify");
                    if (strNotify.Trim() != "")
                    {
                        Functions oFunction = new Functions(0, dsn, _environment);
                        strIPEmail = "The IP address " + GetName(_ipaddressid, 0) + " has been released from " + _name + " and is available for use.";
                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_IPADDRESS");
                        oFunction.SendEmail("IP Release", strNotify, "", strEMailIdsBCC, "IP Release", strIPEmail, false, true);
                    }
                }
            }
            return strIPEmail;
        }
        */


        public DataSet GetVlanHA(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getVlanHA", arParams);
        }
        public string GetVlanHA(int _id, string _column)
        {
            DataSet ds = GetVlanHA(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddVlanHA(int _original_vlan, int _ha_vlan)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@original_vlan", _original_vlan);
            arParams[1] = new SqlParameter("@ha_vlan", _ha_vlan);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_addVlanHA", arParams);
        }
        public void UpdateVlanHA(int _id, int _original_vlan, int _ha_vlan)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@original_vlan", _original_vlan);
            arParams[2] = new SqlParameter("@ha_vlan", _ha_vlan);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_updateVlanHA", arParams);
        }
        public void DeleteVlanHA(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn_ip, CommandType.StoredProcedure, "pr_deleteVlanHA", arParams);
        }
        public DataSet GetVlanHAs(int _original_vlan)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@original_vlan", _original_vlan);
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getVlanHAs", arParams);
        }
        public DataSet GetVlanHAs()
        {
            return SqlHelper.ExecuteDataset(dsn_ip, CommandType.StoredProcedure, "pr_getVlanHAsAll");
        }

        #region Related
        public void AddRelated(int _answerid, int _clusterid, int _ipaddressid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            arParams[2] = new SqlParameter("@ipaddressid", _ipaddressid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addIPAddressRelated", arParams);
        }
        public DataSet GetRelated(int _answerid, int _clusterid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIPAddressRelated", arParams);
        }
        #endregion
    }
}
