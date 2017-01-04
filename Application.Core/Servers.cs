using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;
using System.Data.SqlTypes;
using NCC.ClearView.Application.Core.w08r2;
using System.IO;

namespace NCC.ClearView.Application.Core
{
    public class Servers
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        private string strPreviousStep = "";
        private bool boolTaskRunning = false;

        public Servers(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public int Add(int _requestid, int _answerid, int _modelid, int _csmconfigid, int _clusterid, int _number, int _osid, int _spid, int _templateid, int _domainid, int _test_domainid, int _infrastructure, int _ha, int _dr, int _dr_exist, string _dr_name, int _dr_consistency, int _dr_consistencyid, int _configured, int _local_storage, int _accounts, int _fdrive, int _dba, int _pnc, int _vmware_clusterid, int _dns_auto)
        {
            Forecast oForecast = new Forecast(user, dsn);
            int intModel = _modelid;
            if (_answerid > 0 && _modelid == 0)
                intModel = oForecast.GetModel(_answerid);
            arParams = new SqlParameter[27];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@answerid", _answerid);
            arParams[2] = new SqlParameter("@modelid", intModel);
            arParams[3] = new SqlParameter("@csmconfigid", _csmconfigid);
            arParams[4] = new SqlParameter("@clusterid", _clusterid);
            arParams[5] = new SqlParameter("@number", _number);
            arParams[6] = new SqlParameter("@osid", _osid);
            arParams[7] = new SqlParameter("@spid", _spid);
            arParams[8] = new SqlParameter("@templateid", _templateid);
            arParams[9] = new SqlParameter("@domainid", _domainid);
            arParams[10] = new SqlParameter("@test_domainid", _test_domainid);
            arParams[11] = new SqlParameter("@infrastructure", _infrastructure);
            arParams[12] = new SqlParameter("@ha", _ha);
            arParams[13] = new SqlParameter("@dr", _dr);
            arParams[14] = new SqlParameter("@dr_exist", _dr_exist);
            arParams[15] = new SqlParameter("@dr_name", _dr_name);
            arParams[16] = new SqlParameter("@dr_consistency", _dr_consistency);
            arParams[17] = new SqlParameter("@dr_consistencyid", _dr_consistencyid);
            arParams[18] = new SqlParameter("@configured", _configured);
            arParams[19] = new SqlParameter("@local_storage", _local_storage);
            arParams[20] = new SqlParameter("@accounts", _accounts);
            arParams[21] = new SqlParameter("@fdrive", _fdrive);
            arParams[22] = new SqlParameter("@dba", _dba);
            arParams[23] = new SqlParameter("@pnc", _pnc);
            arParams[24] = new SqlParameter("@vmware_clusterid", _vmware_clusterid);
            arParams[25] = new SqlParameter("@dns_auto", _dns_auto);
            arParams[26] = new SqlParameter("@id", SqlDbType.Int);
            arParams[26].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServer", arParams);
            return Int32.Parse(arParams[26].Value.ToString());
        }
        public void Update(int _id, int _osid, int _spid, int _templateid, int _domainid, int _test_domainid, int _infrastructure, int _ha, int _dr, int _dr_exist, string _dr_name, int _dr_consistency, int _dr_consistencyid, int _configured, int _dba, int _pnc)
        {
            arParams = new SqlParameter[16];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@osid", _osid);
            arParams[2] = new SqlParameter("@spid", _spid);
            arParams[3] = new SqlParameter("@templateid", _templateid);
            arParams[4] = new SqlParameter("@domainid", _domainid);
            arParams[5] = new SqlParameter("@test_domainid", _test_domainid);
            arParams[6] = new SqlParameter("@infrastructure", _infrastructure);
            arParams[7] = new SqlParameter("@ha", _ha);
            arParams[8] = new SqlParameter("@dr", _dr);
            arParams[9] = new SqlParameter("@dr_exist", _dr_exist);
            arParams[10] = new SqlParameter("@dr_name", _dr_name);
            arParams[11] = new SqlParameter("@dr_consistency", _dr_consistency);
            arParams[12] = new SqlParameter("@dr_consistencyid", _dr_consistencyid);
            arParams[13] = new SqlParameter("@configured", _configured);
            arParams[14] = new SqlParameter("@dba", _dba);
            arParams[15] = new SqlParameter("@pnc", _pnc);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServer", arParams);
        }
        public void Update(int _id, int _ha, int _dr)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@ha", _ha);
            arParams[2] = new SqlParameter("@dr", _dr);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerHA", arParams);
        }
        public void UpdateMHS(int _id, int _mhs)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@mhs", _mhs);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerMHS", arParams);
        }
        public void UpdateDHCP(int _id, string _dhcp)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@dhcp", _dhcp);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerDHCP", arParams);
        }
        public void UpdateReclaimStorage(int _id, double _reclaimed_storage, double _reclaimed_amt, int _reclaimed_tier,
            string _reclaimed_environment, string _reclaimed_storage_precooldown, string _reclaimed_storage_cooldown,
            string _reclaimed_storage_cr2, string _reclaimed_storage_classification, string _reclaimed_storage_vendor,
            int _reclaimed_storage_location, string _reclaimed_storage_array, string _reclaimed_storage_notes)
        {
            arParams = new SqlParameter[13];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@reclaimed_storage", _reclaimed_storage);
            arParams[2] = new SqlParameter("@reclaimed_amt", _reclaimed_amt);
            arParams[3] = new SqlParameter("@reclaimed_tier", _reclaimed_tier);
            arParams[4] = new SqlParameter("@reclaimed_environment", _reclaimed_environment);
            arParams[5] = new SqlParameter("@reclaimed_storage_precooldown", (_reclaimed_storage_precooldown == "" ? SqlDateTime.Null : DateTime.Parse(_reclaimed_storage_precooldown)));
            arParams[6] = new SqlParameter("@reclaimed_storage_cooldown", (_reclaimed_storage_cooldown == "" ? SqlDateTime.Null : DateTime.Parse(_reclaimed_storage_cooldown)));
            arParams[7] = new SqlParameter("@reclaimed_storage_cr2", _reclaimed_storage_cr2);
            arParams[8] = new SqlParameter("@reclaimed_storage_classification", _reclaimed_storage_classification);
            arParams[9] = new SqlParameter("@reclaimed_storage_vendor", _reclaimed_storage_vendor);
            arParams[10] = new SqlParameter("@reclaimed_storage_location", _reclaimed_storage_location);
            arParams[11] = new SqlParameter("@reclaimed_storage_array", _reclaimed_storage_array);
            arParams[12] = new SqlParameter("@reclaimed_storage_notes", _reclaimed_storage_notes);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerReclaimStorage", arParams);
        }
        public void UpdateReclaimBackup(int _id, double _reclaimed_backup)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@reclaimed_backup", _reclaimed_backup);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerReclaimBackup", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServer", arParams);
        }
        public void UpdateModels(int _answerid)
        {
            Forecast oForecast = new Forecast(user, dsn);
            int intModel = oForecast.GetModel(_answerid);
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@modelid", intModel);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerModels", arParams);
        }
        public void UpdateZeus(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerZeus", arParams);
        }
        public void UpdateZeusError(int _id, int _zeus_error)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@zeus_error", _zeus_error);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerZeusError", arParams);
        }
        public void UpdateTSM(int _id, int _tsm_schedule, int _tsm_cloptset, string _tsm_register, string _tsm_define, int _tsm_bypass)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@tsm_schedule", _tsm_schedule);
            arParams[2] = new SqlParameter("@tsm_cloptset", _tsm_cloptset);
            arParams[3] = new SqlParameter("@tsm_register", _tsm_register);
            arParams[4] = new SqlParameter("@tsm_define", _tsm_define);
            arParams[5] = new SqlParameter("@tsm_bypass", _tsm_bypass);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerTSM", arParams);
        }
        public void UpdateTSMSchedule(int _id, int _tsm_schedule)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@tsm_schedule", _tsm_schedule);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerTSMSchedule", arParams);
        }
        public void UpdateTSM(int _id, string _tsm_output)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@tsm_output", _tsm_output);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerTSMOutput", arParams);
        }
        public void UpdateTSMRegistered(int _id, string _tsm_registered)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@tsm_registered", (_tsm_registered == "" ? SqlDateTime.Null : DateTime.Parse(_tsm_registered)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerTSMRegistered", arParams);
        }
        public void UpdateStorageConfigured(int _id, string _storage_configured)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@storage_configured", (_storage_configured == "" ? SqlDateTime.Null : DateTime.Parse(_storage_configured)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerStorageConfigured", arParams);
        }
        public DataSet GetStorageConfigured()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerStorageConfigured");
        }
        public void UpdateDNS(int _id, int _dns_auto, string _dns_output)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@dns_auto", _dns_auto);
            arParams[2] = new SqlParameter("@dns_output", _dns_output);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerDNS", arParams);
        }
        public void UpdateLocalStorage(int _id, int _local_storage)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@local_storage", _local_storage);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerLocalStorage", arParams);
        }
        public void UpdateAccounts(int _id, int _accounts)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@accounts", _accounts);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerAccounts", arParams);
        }
        public void UpdateConfigured(int _id, int _configured)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@configured", _configured);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerConfigured", arParams);
        }
        public void UpdateFDrive(int _id, int _fdrive)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@fdrive", _fdrive);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerFDrive", arParams);
        }
        public void UpdateSVECluster(int _serverid, int _clusterid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerSVECluster", arParams);
        }
        public void DeleteSVE(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerSVE", arParams);
        }
        public void UpdateServerNamed(int _id, int _nameid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@nameid", _nameid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNamed", arParams);
        }
        public void UpdateBuildStarted(int _id, string _build_started)
        {
            UpdateBuildStarted(_id, _build_started, false);
        }
        public void UpdateBuildStarted(int _id, string _build_started, bool _override_date)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@build_started", (_build_started == "" ? SqlDateTime.Null : DateTime.Parse(_build_started)));
            arParams[2] = new SqlParameter("@override_date", (_override_date ? 1 : 0));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerBuildStarted", arParams);
        }
        public void UpdateBuildCompleted(int _id, string _build_completed)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@build_completed", (_build_completed == "" ? SqlDateTime.Null : DateTime.Parse(_build_completed)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerBuildCompleted", arParams);
        }
        public void UpdateBuildReady(int _id, string _build_ready, bool _override_date)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@build_ready", (_build_ready == "" ? SqlDateTime.Null : DateTime.Parse(_build_ready)));
            arParams[2] = new SqlParameter("@override_date", (_override_date ? 1 : 0));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerBuildReady", arParams);
        }
        public void UpdateMISAudits(int _id, string _mis_audits)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@mis_audits", (_mis_audits == "" ? SqlDateTime.Null : DateTime.Parse(_mis_audits)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerMISAudit", arParams);
        }
        public void UpdateRebuild(int _id, string _rebuild)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@rebuild", (_rebuild == "" ? SqlDateTime.Null : DateTime.Parse(_rebuild)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerRebuild", arParams);
            UpdateRebuildCompleted(_id, _rebuild);
        }
        public void UpdateRebuilding(int _id, int _rebuilding)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@rebuilding", _rebuilding);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerRebuilding", arParams);
        }
        public void UpdateDecommissioned(int _id, string _decom)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@decom", (_decom == "" ? SqlDateTime.Null : DateTime.Parse(_decom)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerDecommissioned", arParams);
        }
        public void AddIP(int _serverid, int _ipaddressid, int _auto_assign, int _final, int _vmotion, int _avamar)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@ipaddressid", _ipaddressid);
            arParams[2] = new SqlParameter("@auto_assign", _auto_assign);
            arParams[3] = new SqlParameter("@final", _final);
            arParams[4] = new SqlParameter("@vmotion", _vmotion);
            arParams[5] = new SqlParameter("@avamar", _avamar);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerIP", arParams);
        }
        public DataSet GetIP(int _serverid, int _auto_assign, int _final, int _vmotion, int _avamar)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@auto_assign", _auto_assign);
            arParams[2] = new SqlParameter("@final", _final);
            arParams[3] = new SqlParameter("@vmotion", _vmotion);
            arParams[4] = new SqlParameter("@avamar", _avamar);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerIP", arParams);
        }
        public DataSet GetIP(int _serverid, int _ipaddressid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@ipaddressid", _ipaddressid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerIPs", arParams);
        }
        public string GetIPs(int _serverid, int _auto_assign, int _final, int _vmotion, int _avamar, string _dsn_ip, string _separate, string _none)
        {
            return GetIPs(_serverid, 0, _auto_assign, _final, _vmotion, _avamar, _dsn_ip, _separate, _none, false);
        }
        public string GetIPs(int _serverid, int _auto_assign, int _final, int _vmotion, int _avamar, string _dsn_ip, string _separate, string _none, bool _add_vlan)
        {
            return GetIPs(_serverid, 0, _auto_assign, _final, _vmotion, _avamar, _dsn_ip, _separate, _none, _add_vlan);
        }
        public string GetIPs(int _serverid, int _clusterid, int _auto_assign, int _final, int _vmotion, int _avamar, string _dsn_ip, string _separate, string _none, bool _add_vlan)
        {
            if (_separate == "")
                _separate = ", ";
            if (_none == "")
                _none = "N / A";

            IPAddresses oIPAddress = new IPAddresses(user, _dsn_ip, dsn);
            string strIPs = "";
            if (_clusterid > 0)
            {
                DataSet dsServer = GetClusters(_clusterid);
                if (dsServer.Tables[0].Rows.Count > 1)
                {
                    // This is a cluster
                    foreach (DataRow drServer in dsServer.Tables[0].Rows)
                    {
                        DataSet dsIP = GetIP(Int32.Parse(drServer["id"].ToString()), _auto_assign, _final, _vmotion, _avamar);
                        if (dsIP.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow drIP in dsIP.Tables[0].Rows)
                            {
                                if (drIP["ipaddressid"].ToString() != "")
                                {
                                    int intIPAddress = Int32.Parse(drIP["ipaddressid"].ToString());
                                    if (strIPs != "")
                                        strIPs += _separate;
                                    strIPs += oIPAddress.GetName(intIPAddress, 0);
                                    if (_add_vlan == true)
                                    {
                                        int intNetwork = 0;
                                        if (Int32.TryParse(oIPAddress.Get(intIPAddress, "networkid"), out intNetwork) && intNetwork > 0)
                                            strIPs += " (VLAN: " + oIPAddress.GetNetworkVlan(intNetwork).ToString() + ")";
                                    }
                                }
                            }
                        }
                    }
                    // Now add any additional IPs
                    int intAnswer = 0;
                    Int32.TryParse(Get(_serverid, "answerid"), out intAnswer);
                    DataSet dsAdditional = oIPAddress.GetRelated(intAnswer, _clusterid);
                    if (dsAdditional.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drAdditional in dsAdditional.Tables[0].Rows)
                        {
                            if (drAdditional["ipaddressid"].ToString() != "")
                            {
                                int intIPAddress = Int32.Parse(drAdditional["ipaddressid"].ToString());
                                if (strIPs != "")
                                    strIPs += _separate;
                                strIPs += oIPAddress.GetName(intIPAddress, 0);
                                if (_add_vlan == true)
                                {
                                    int intNetwork = 0;
                                    if (Int32.TryParse(oIPAddress.Get(intIPAddress, "networkid"), out intNetwork) && intNetwork > 0)
                                        strIPs += " (VLAN: " + oIPAddress.GetNetworkVlan(intNetwork).ToString() + ")";
                                }
                            }
                        }
                    }
                }
                else
                    _clusterid = 0;
            }
            
            if (_clusterid == 0)
            {
                DataSet dsIP = GetIP(_serverid, _auto_assign, _final, _vmotion, _avamar);
                if (dsIP.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drIP in dsIP.Tables[0].Rows)
                    {
                        if (drIP["ipaddressid"].ToString() != "")
                        {
                            int intIPAddress = Int32.Parse(drIP["ipaddressid"].ToString());
                            if (strIPs != "")
                                strIPs += _separate;
                            strIPs += oIPAddress.GetName(intIPAddress, 0);
                            if (_add_vlan == true)
                            {
                                int intNetwork = 0;
                                if (Int32.TryParse(oIPAddress.Get(intIPAddress, "networkid"), out intNetwork) && intNetwork > 0)
                                    strIPs += " (VLAN: " + oIPAddress.GetNetworkVlan(intNetwork).ToString() + ")";
                            }
                        }
                    }
                }
            }
            if (strIPs == "")
                strIPs = _none;
            return strIPs;
        }
        public void DeleteIP(int _serverid, int _auto_assign, int _final, int _vmotion, int _avamar, string _dsn_ip)
        {
            DataSet dsIP = GetIP(_serverid, _auto_assign, _final, _vmotion, _avamar);
            foreach (DataRow drIP in dsIP.Tables[0].Rows)
            {
                int intIP = Int32.Parse(drIP["ipaddressid"].ToString());
                DeleteIP(_serverid, intIP, _dsn_ip);
            }
        }
        public void UpdateIP(int _serverid, int _ipaddressid, int _auto_assign, int _final, int _vmotion, int _avamar, string _dsn_ip)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@ipaddressid", _ipaddressid);
            arParams[2] = new SqlParameter("@auto_assign", _auto_assign);
            arParams[3] = new SqlParameter("@final", _final);
            arParams[4] = new SqlParameter("@vmotion", _vmotion);
            arParams[5] = new SqlParameter("@avamar", _avamar);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerIP", arParams);

            bool boolDeleteIP = true;
            DataSet dsIP = GetIP(_serverid, _ipaddressid);
            foreach (DataRow drIP in dsIP.Tables[0].Rows)
            {
                if (drIP["vmotion"].ToString() == "1" || drIP["final"].ToString() == "1" || drIP["auto_assign"].ToString() == "1")
                {
                    boolDeleteIP = false;
                    break;
                }
            }
            if (boolDeleteIP == true)
                DeleteIP(_serverid, _ipaddressid, _dsn_ip);
        }
        public void DeleteServerIP(int _serverid, int _ipaddressid, string _dsn_ip)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@ipaddressid", _ipaddressid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerIP", arParams);
        }
        public void DeleteIP(int _serverid, int _ipaddressid, string _dsn_ip)
        {
            DeleteServerIP(_serverid, _ipaddressid, _dsn_ip);
            IPAddresses oIPAddresses = new IPAddresses(0, _dsn_ip, dsn);
            oIPAddresses.UpdateAvailable(_ipaddressid, 1);
        }

        public void AddIPNicInterface(int _serveripid, string _macaddress, string _network_interface_name, int _cluster_interface, int _public_interface)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@serveripid", _serveripid);
            arParams[1] = new SqlParameter("@macaddress", _macaddress);
            arParams[2] = new SqlParameter("@network_interface_name", _network_interface_name);
            arParams[3] = new SqlParameter("@cluster_interface", _cluster_interface);
            arParams[4] = new SqlParameter("@public_interface", _public_interface);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerIPNicInterface", arParams);
        }
        public void UpdateIPNicInterface(int _id, int _serveripid, string _macaddress, string _network_interface_name, int _cluster_interface, int _public_interface)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@serveripid", _serveripid);
            arParams[2] = new SqlParameter("@macaddress", _macaddress);
            arParams[3] = new SqlParameter("@network_interface_name", _network_interface_name);
            arParams[4] = new SqlParameter("@cluster_interface", _cluster_interface);
            arParams[5] = new SqlParameter("@public_interface", _public_interface);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerIPNicInterface", arParams);
        }
        public void DeleteIPNicInterface(int _serveripid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serveripid", _serveripid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerIPNicInterface", arParams);
        }
        public DataSet GetIPNicInterface(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerIPNicInterface", arParams);
        }
        public string GetIPNicInterface(int _id, string _column)
        {
            DataSet ds = GetIPNicInterface(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetIPNicInterfaces(int _serveripid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serveripid", _serveripid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerIPNicInterfaces", arParams);
        }

        //public void AddAsset(int _serverid, int _assetid, int _build, int _test, int _ecom, int _prod, int _dr, int _latest)
        //{
        //    if (_dr == 0)
        //        UpdateAsset(_serverid, _assetid);
        //    arParams = new SqlParameter[8];
        //    arParams[0] = new SqlParameter("@serverid", _serverid);
        //    arParams[1] = new SqlParameter("@assetid", _assetid);
        //    arParams[2] = new SqlParameter("@build", _build);
        //    arParams[3] = new SqlParameter("@test", _test);
        //    arParams[4] = new SqlParameter("@ecom", _ecom);
        //    arParams[5] = new SqlParameter("@prod", _prod);
        //    arParams[6] = new SqlParameter("@dr", _dr);
        //    arParams[7] = new SqlParameter("@latest", _latest);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerAsset", arParams);
        //}
        //public void UpdateAsset(int _serverid, int _assetid)
        //{
        //    arParams = new SqlParameter[2];
        //    arParams[0] = new SqlParameter("@serverid", _serverid);
        //    arParams[1] = new SqlParameter("@assetid", _assetid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerAssetLatest", arParams);
        //}
        public void AddHA(int _serverid, int _serverid_ha)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@serverid_ha", _serverid_ha);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerHA", arParams);
        }
        public DataSet GetHA(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerHA", arParams);
        }
        public DataSet GetHAs(int _serverid_ha)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid_ha", _serverid_ha);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerHAs", arParams);
        }
        public DataSet DeleteHA(int _serverid)
        {
            // Delete servers
            DataSet ds = GetHA(_serverid);
            foreach (DataRow dr in ds.Tables[0].Rows)
                Delete(Int32.Parse(dr["serverid_ha"].ToString()));
            // Delete associations
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_deleteServerHA", arParams);
        }
        public void AddAsset(int _serverid, int _assetid, int _classid, int _environmentid, int _removable, int _dr)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@assetid", _assetid);
            arParams[2] = new SqlParameter("@classid", _classid);
            arParams[3] = new SqlParameter("@environmentid", _environmentid);
            arParams[4] = new SqlParameter("@removable", _removable);
            arParams[5] = new SqlParameter("@dr", _dr);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerAsset", arParams);
        }
        public void UpdateAsset(int _id, int _classid, int _environmentid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerAsset", arParams);
        }
        public void UpdateAssetDecom(int _serverid, int _assetid, string _decommed)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@assetid", _assetid);
            arParams[2] = new SqlParameter("@decommed", (_decommed == "" ? SqlDateTime.Null : DateTime.Parse(_decommed)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerAssetDecom", arParams);
        }
        public void DeleteAsset(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerAssets", arParams);
        }
        public void DeleteAsset(int _serverid, int _assetid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@assetid", _assetid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerAsset", arParams);
        }
        public DataSet GetAssetsAsset(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerAssetsAsset", arParams);
        }
        public DataSet GetAssetsServer(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerAssetsServer", arParams);
        }
        public DataSet GetAsset(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServersAsset", arParams);
        }
        //public DataSet GetAssets(int _answerid, int _classid, int _environmentid)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@answerid", _answerid);
        //    arParams[1] = new SqlParameter("@classid", _classid);
        //    arParams[2] = new SqlParameter("@environmentid", _environmentid);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServersAssets", arParams);
        //}
        public DataSet GetAssets(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServersAssetsAll", arParams);
        }
        public DataSet GetAssetsNot(int _answerid, int _classid, int _environmentid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServersAssetsNot", arParams);
        }
        public void UpdateAnswer(int _answerid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerAnswerStep", arParams);
        }
        public void UpdateStep(int _id, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerStep", arParams);
        }

        public void UpdatePause(int _id, int _pause)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@paused", _pause);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerPause", arParams);
        }

        public void UpdateStepSkip(int _id, int _step_skip_start, int _step_skip_goto)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@step_skip_start", _step_skip_start);
            arParams[2] = new SqlParameter("@step_skip_goto", _step_skip_goto);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerStepSkip", arParams);
        }
        public DataSet Get(int _answerid, int _csmconfigid, int _clusterid, int _number)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@csmconfigid", _csmconfigid);
            arParams[2] = new SqlParameter("@clusterid", _clusterid);
            arParams[3] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServer", arParams);
        }

        public DataSet GetVMwareCluster(int _clusterid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServersVMwareCluster", arParams);
        }
        public DataSet GetSVE(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServersSVE", arParams);
        }
        public DataSet GetSVEClusters(int _clusterid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServersSVECluster", arParams);
        }
        public DataSet GetVMwareClusters()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServersVMwareClusters");
        }
        public DataSet GetVMwareClustersStatus()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServersVMwareClustersStatus");
        }

        public DataSet GetManual(int _answerid, bool _prod)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@prod", (_prod ? 1 : 0));
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerManual", arParams);
        }
        public DataSet GetAnswer(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerAnswer", arParams);
        }
        public DataSet GetAnswerClusters(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerAnswerClusters", arParams);
        }
        public string GetName(int _id, bool _use_pnc)
        {
            ServerName oServerName = new ServerName(user, dsn);
            string strName = Get(_id, "nameid");
            if (strName == "" || strName == "0")
                return "";
            else
            {
                if (Get(_id, "pnc") == "1" && _use_pnc == true)
                    return oServerName.GetNameFactory(Int32.Parse(strName), 0);
                else
                    return oServerName.GetName(Int32.Parse(strName), 0);
            }
        }
        public string GetNameDR(int _id, bool _use_pnc)
        {
            ServerName oServerName = new ServerName(user, dsn);
            string strName = "";
            if (Get(_id, "dr") == "1")
            {
                if (Get(_id, "dr_name") != "")
                    strName = Get(_id, "dr_name");
                else
                {
                    strName = Get(_id, "nameid");
                    if (strName != "" && strName != "0")
                    {
                        if (Get(_id, "pnc") == "1" && _use_pnc == true)
                            strName = oServerName.GetNameFactory(Int32.Parse(strName), 0);
                        else
                            strName = oServerName.GetName(Int32.Parse(strName), 0);
                    }
                    strName = strName + "-DR";
                }
            }
            return strName;
        }
        public DataSet GetRequests(int _requestid, int _latest)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@latest", _latest);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServersRequest", arParams);
        }
        public DataSet GetClusters(int _clusterid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServersCluster", arParams);
        }
        public void DeleteClusters(int _clusterid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServersCluster", arParams);
        }
        public DataSet GetSteps(int id, int _answerid, int _step)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", id);
            arParams[1] = new SqlParameter("@answerid", _answerid);
            arParams[2] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServersStep", arParams);
        }
        public DataSet GetTypes(string _types)
        {
            Functions oFunction = new Functions(0, dsn, 0);
            string strServerTypes = oFunction.BuildXmlStringType(_types);
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@types", strServerTypes);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerTypes", arParams);
        }
        public DataSet GetScheduler(string _types)
        {
            Functions oFunction = new Functions(0, dsn, 0);
            string strServerTypes = oFunction.BuildXmlStringType(_types);
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@types", strServerTypes);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSchedulerRestartServer", arParams);
        }
        public DataSet Gets()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServers");
        }
        public DataSet GetCompletes()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServersCompleted");
        }
        public DataSet GetTSMs()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServersTSM");
        }
        public DataSet Get(string _name, bool _can_be_null)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@can_be_null", (_can_be_null ? 1 : 0));
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerByName", arParams);
        }
        public DataSet GetDecommission(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerDecommission", arParams);
        }
        public DataSet GetDNS(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerDNS", arParams);
        }
        public int GetStep(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            DataSet dsStep = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerStep", arParams);
            int intStep = 0;
            if (dsStep.Tables[0].Rows.Count > 0)
                Int32.TryParse(dsStep.Tables[0].Rows[0]["step"].ToString(), out intStep);
            return intStep;
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerId", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public string GetIPBuild(int _id)
        {
            string strIP = "";
            DataSet ds = GetIP(_id, 1, 0, 0, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                strIP = dr["ipaddressid"].ToString();
                break;
            }
            return strIP;
        }
        public void Start(int _requestid)
        {
            // Fix for duplicate servers (for deleted clusters)
            DeleteCluster();
            // Add HA Servers
            int intNumber = 0;
            DataSet ds = GetRequests(_requestid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["number"].ToString()) > intNumber && (dr["ha"].ToString() == "0" || dr["ha"].ToString() == "1"))
                    intNumber = Int32.Parse(dr["number"].ToString());
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["ha"].ToString() == "1")
                {
                    int intID = Int32.Parse(dr["id"].ToString());
                    DeleteHA(intID);
                    int intAnswer = Int32.Parse(dr["answerid"].ToString());
                    int intCSM = Int32.Parse(dr["csmconfigid"].ToString());
                    int intCluster = Int32.Parse(dr["clusterid"].ToString());
                    intNumber++;
                    int intOS = Int32.Parse(dr["osid"].ToString());
                    int intSP = Int32.Parse(dr["spid"].ToString());
                    int intTemplate = Int32.Parse(dr["templateid"].ToString());
                    int intDomain = Int32.Parse(dr["domainid"].ToString());
                    int intDomainTest = Int32.Parse(dr["test_domainid"].ToString());
                    int intINF = Int32.Parse(dr["infrastructure"].ToString());
                    int intDRExist = Int32.Parse(dr["dr_exist"].ToString());
                    string strDRName = dr["dr_name"].ToString();
                    int intDRCons = Int32.Parse(dr["dr_consistency"].ToString());
                    int intDRConsID = Int32.Parse(dr["dr_consistencyid"].ToString());
                    int intLocal = Int32.Parse(dr["local_storage"].ToString());
                    int intAccounts = Int32.Parse(dr["accounts"].ToString());
                    int intF = Int32.Parse(dr["fdrive"].ToString());
                    int intDBA = Int32.Parse(dr["dba"].ToString());
                    int intPNC = Int32.Parse(dr["pnc"].ToString());
                    int intDNSAuto = Int32.Parse(dr["dns_auto"].ToString());
                    int intServer = Add(_requestid, intAnswer, 0, intCSM, intCluster, intNumber, intOS, intSP, intTemplate, intDomain, intDomainTest, intINF, 10, 0, intDRExist, strDRName, intDRCons, intDRConsID, 1, intLocal, intAccounts, intF, intDBA, intPNC, 0, intDNSAuto);
                    ServerName oServerName = new ServerName(0, dsn);
                    DataSet dsComponents = oServerName.GetComponentDetailSelected(intID, 1);
                    foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                        oServerName.AddComponentDetailSelected(intServer, Int32.Parse(drComponent["detailid"].ToString()), Int32.Parse(drComponent["prerequisiteid"].ToString()), false);
                    AddHA(intID, intServer);
                }
            }
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerStart", arParams);
        }
        public void DeleteCluster()
        {
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerCluster");
        }
        public void NextStep(int _id) 
        {
            int intStep = Int32.Parse(Get(_id, "step"));
            bool boolRebuild = (Get(_id, "rebuilding") == "1");
            OnDemand oOnDemand = new OnDemand(0, dsn);
            int intRedo = (intStep * -1);
            if (oOnDemand.GetStepDoneServer(_id, intRedo).Tables[0].Rows.Count > 0)
            {
                // Found a (-)STEP...so this was a redo step.
                DataSet dsRedo = oOnDemand.GetStepDoneServer(_id, intStep);
                if (dsRedo.Tables[0].Rows.Count > 0)
                {
                    // Get new result
                    string strNewResult = dsRedo.Tables[0].Rows[0]["result"].ToString();
                    // Delete new record
                    oOnDemand.DeleteStepDoneServer(_id, intStep);
                    // Update the redone step to positive step (to replace the one just deleted)
                    oOnDemand.UpdateStepDoneServerRedo(_id, intRedo);
                    // Update the redone step with the new result
                    oOnDemand.UpdateStepDoneServerResult(_id, intStep, strNewResult, false);
                }
                if (boolRebuild == true)
                {
                    // Set the next step to redo...and so on...
                    oOnDemand.UpdateStepDoneServerRedo(_id, intStep + 1);
                }
            }
            UpdateStep(_id, intStep + 1);
        }

        //public void AddComponent(int _serverid, int _componentid)
        //{
        //    arParams = new SqlParameter[2];
        //    arParams[0] = new SqlParameter("@serverid", _serverid);
        //    arParams[1] = new SqlParameter("@componentid", _componentid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerComponent", arParams);
        //}
        //public void UpdateComponent(int _serverid, int _componentid, int _done)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@serverid", _serverid);
        //    arParams[1] = new SqlParameter("@componentid", _componentid);
        //    arParams[2] = new SqlParameter("@done", _done);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerComponent", arParams);
        //}
        //public void DeleteComponent(int _serverid)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@serverid", _serverid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerComponents", arParams);
        //}
        //public DataSet GetComponents(int _serverid)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@serverid", _serverid);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerComponents", arParams);
        //}
        //public DataSet GetComponents()
        //{
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerComponentsAll");
        //}
        //public DataSet GetComponentsActive(int _serverid)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@serverid", _serverid);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerComponentsActive", arParams);
        //}

        //public DataSet GetComponentScripts(int _componentid, int _enabled)
        //{
        //    arParams = new SqlParameter[2];
        //    arParams[0] = new SqlParameter("@componentid", _componentid);
        //    arParams[1] = new SqlParameter("@enabled", _enabled);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerComponentScripts", arParams);
        //}
        //public DataSet GetComponentScript(int _id)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@id", _id);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerComponentScript", arParams);
        //}
        //public string GetComponentScript(int _id, string _column)
        //{
        //    DataSet ds = GetComponentScript(_id);
        //    if (ds.Tables[0].Rows.Count > 0)
        //        return ds.Tables[0].Rows[0][_column].ToString();
        //    else
        //        return "";
        //}
        //public void AddComponentScript(int _componentid, string _name, string _script, int _display, int _enabled)
        //{
        //    arParams = new SqlParameter[5];
        //    arParams[0] = new SqlParameter("@componentid", _componentid);
        //    arParams[1] = new SqlParameter("@name", _name);
        //    arParams[2] = new SqlParameter("@script", _script);
        //    arParams[3] = new SqlParameter("@display", _display);
        //    arParams[4] = new SqlParameter("@enabled", _enabled);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerComponentScript", arParams);
        //}
        //public void UpdateComponentScript(int _id, int _componentid, string _name, string _script, int _enabled)
        //{
        //    arParams = new SqlParameter[5];
        //    arParams[0] = new SqlParameter("@id", _id);
        //    arParams[1] = new SqlParameter("@componentid", _componentid);
        //    arParams[2] = new SqlParameter("@name", _name);
        //    arParams[3] = new SqlParameter("@script", _script);
        //    arParams[4] = new SqlParameter("@enabled", _enabled);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerComponentScript", arParams);
        //}
        //public void UpdateComponentScriptOrder(int _id, int _display)
        //{
        //    arParams = new SqlParameter[2];
        //    arParams[0] = new SqlParameter("@id", _id);
        //    arParams[1] = new SqlParameter("@display", _display);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerComponentScriptOrder", arParams);
        //}
        //public void EnableComponentScript(int _id, int _enabled)
        //{
        //    arParams = new SqlParameter[2];
        //    arParams[0] = new SqlParameter("@id", _id);
        //    arParams[1] = new SqlParameter("@enabled", _enabled);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerComponentScriptEnabled", arParams);
        //}
        //public void DeleteComponentScript(int _id)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@id", _id);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerComponentScript", arParams);
        //}

        public int AddError(int _requestid, int _itemid, int _number, int _serverid, int _step, string _reason)
        {
            Errors oError = new Errors(user, dsn);
            oError.CheckError(GetErrorLatest(_serverid, _step));

            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@serverid", _serverid);
            arParams[4] = new SqlParameter("@step", _step);
            arParams[5] = new SqlParameter("@reason", _reason);
            arParams[6] = new SqlParameter("@id", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerError", arParams);
            return Int32.Parse(arParams[6].Value.ToString());
        }

        public DataSet GetErrorLatest(int _serverid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerErrorLatest", arParams);
        }
        public DataSet GetError(int _serverid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerError", arParams);
        }
        public DataSet GetErrors(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerErrors", arParams);
        }

        public DataSet GetErrorByRequest(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@RequestId", _requestid);
            arParams[1] = new SqlParameter("@ItemId", _itemid);
            arParams[2] = new SqlParameter("@Number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerErrorsByRequest", arParams);
        }

        public string GetErrorDetailsBody(int _requestid, int _itemid, int _number, int _environment)
        {
            Variables oVariable = new Variables(_environment);
            Functions oFunction = new Functions(0, dsn, _environment);
            StringBuilder sbBody = new StringBuilder();
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";

            string strSpacerTD = "<td style=\"width:10;text-align:left\"></td>";
            string strTRstart = "<tr>";
            string strTRend = "</tr>";
            string strTDstart = "<td>";
            string strTDend = "</td>";


            DataSet dsError = GetErrorByRequest(_requestid, _itemid, _number);
                if (dsError.Tables[0].Rows.Count > 0)
                {   DataRow drError= dsError.Tables[0].Rows[0];
                    sbBody.Append(strTRstart + strTDstart + "<b>Error Message:</b>" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + drError["reason"].ToString() + strTDend + strTRend );
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "<b>Device Name:</b>" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + "<a href=\"/datapoint/asset/server.aspx?t=name&q=" + oFunction.encryptQueryString(drError["servername"].ToString()) + "\" target=\"_blank\">" + drError["servername"].ToString() + "</a>" + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "<b>Model:</b>" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + drError["ModelName"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "<b>Serial Number:</b>" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + drError["serial"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "<b>Asset Number:</b>" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + drError["assettag"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    if (drError["ModelTypeCategory"].ToString() == "Blade")
                    {
                        sbBody.Append(strTRstart + strTDstart + "<b>Enclosure:</b>" + strTDend + strSpacerTD);
                        sbBody.Append(strTDstart + drError["Enclosure"].ToString() + strTDend + strTRend);
                        sbBody.Append(strSpacerRow);
                    }

                    if (drError["ModelTypeCategory"].ToString() == "Virtual")
                    {
                        VMWare oVMWare = new VMWare(0, dsn);
                        DataSet dsVirtual = oVMWare.GetGuest(drError["servername"].ToString());
                        if (dsVirtual.Tables[0].Rows.Count == 1)
                        {
                            DataRow drVirtual = dsVirtual.Tables[0].Rows[0];
                            int intHost = Int32.Parse(drVirtual["hostid"].ToString());
                            int intDataStore = Int32.Parse(drVirtual["datastoreid"].ToString());
                            int intCluster = Int32.Parse(oVMWare.GetHost(intHost, "clusterid"));
                            int intFolder = Int32.Parse(oVMWare.GetCluster(intCluster, "folderid"));
                            int intDataCenter = Int32.Parse(oVMWare.GetFolder(intFolder, "datacenterid"));
                            int intVirtualCenter = Int32.Parse(oVMWare.GetDatacenter(intDataCenter, "virtualcenterid"));

                            sbBody.Append(strTRstart + strTDstart + "<b>Virtual Center Server:</b>" + strTDend + strSpacerTD);
                            sbBody.Append(strTDstart + oVMWare.GetVirtualCenter(intVirtualCenter, "name") + strTDend + strTRend);
                            sbBody.Append(strSpacerRow);

                            sbBody.Append(strTRstart + strTDstart + "<b>Data Center:</b>" + strTDend + strSpacerTD);
                            sbBody.Append(strTDstart + oVMWare.GetDatacenter(intDataCenter, "name") + strTDend + strTRend);
                            sbBody.Append(strSpacerRow);

                            sbBody.Append(strTRstart + strTDstart + "<b>Folder:</b>" + strTDend + strSpacerTD);
                            sbBody.Append(strTDstart + oVMWare.GetFolder(intFolder, "name") + strTDend + strTRend);
                            sbBody.Append(strSpacerRow);

                            sbBody.Append(strTRstart + strTDstart + "<b>Cluster:</b>" + strTDend + strSpacerTD);
                            sbBody.Append(strTDstart + oVMWare.GetCluster(intCluster, "name") + strTDend + strTRend);
                            sbBody.Append(strSpacerRow);

                            sbBody.Append(strTRstart + strTDstart + "<b>Host Name:</b>" + strTDend + strSpacerTD);
                            sbBody.Append(strTDstart + oVMWare.GetHost(intHost, "name") + strTDend + strTRend);
                            sbBody.Append(strSpacerRow);

                            sbBody.Append(strTRstart + strTDstart + "<b>Data Store:</b>" + strTDend + strSpacerTD);
                            sbBody.Append(strTDstart + oVMWare.GetDatastore(intDataStore, "name") + strTDend + strTRend);
                            sbBody.Append(strSpacerRow);

                        }
                    }
                    else
                    {
                        sbBody.Append(strTRstart + strTDstart + "<b>ILO:</b>" + strTDend + strSpacerTD);
                        sbBody.Append(strTDstart + "<a href=\"https://" + drError["ilo"].ToString() + "\" target=\"_blank\">" + drError["ilo"].ToString() + "</a>" + strTDend + strTRend);
                        sbBody.Append(strSpacerRow);
                    }

                    sbBody.Append(strTRstart + strTDstart + "<b>Class:</b>" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + drError["class"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "<b>Environment:</b>" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + drError["Environment"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "<b>Domain:</b>" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + drError["domain"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "<b>Executed:</b>" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + drError["executed"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "<b>Executed By:</b>" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + drError["ExecutedByName"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "<b>Elapsed Time:</b>" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + drError["ElapsedTime"].ToString() +" Days"+ strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strSpacerRow);
                }
               
            if (sbBody.ToString() != "")
            {
                sbBody.Insert(0, "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                sbBody.Append("</table>");
            }
            return sbBody.ToString();
        
        
        }

        public DataSet GetErrorsUser(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerErrorsUser", arParams);
        }
        public DataSet GetErrors()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerErrorsAll");
        }
        public void UpdateError(int _serverid, int _step, int _errorid, int _userid, bool _delete_audit, string _dsn_asset)
        {
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            Forecast oForecast = new Forecast(user, dsn);
            // Update Asset Location (if applicable) for new Design Builder
            //int intAnswer = 0;
            //if (Int32.TryParse(Get(_serverid, "answerid"), out intAnswer) == true)
            //{
            //    int intAddress = 0;
            //    Int32.TryParse(oForecast.GetAnswer(intAnswer, "addressid"), out intAddress);
            //    if (intAddress == 0)
            //    {
            //        Design oDesign = new Design(user, dsn);
            //        int intDesign = 0;
            //        if (Int32.TryParse(oDesign.GetAnswer(intAnswer, "id"), out intDesign) == true)
            //            oDesign.UpdateLocation(intDesign, _dsn_asset);
            //    }
            //}

            // Clear All Audit Errors
            Audit oAudit = new Audit(user, dsn);
            DataSet dsAudit = oAudit.GetErrors(_serverid);
            foreach (DataRow drAudit in dsAudit.Tables[0].Rows)
            {
                int intRequest = Int32.Parse(drAudit["requestid"].ToString());
                int intService = Int32.Parse(drAudit["serviceid"].ToString());
                int intNumber = Int32.Parse(drAudit["number"].ToString());
                int intAudit = Int32.Parse(drAudit["auditid"].ToString());
                int intMIS = 0;
                Int32.TryParse(drAudit["mis"].ToString(), out intMIS);

                DataSet dsRR = oResourceRequest.GetAllService(intRequest, intService, intNumber);
                foreach (DataRow drRR in dsRR.Tables[0].Rows)
                {
                    int intRRW = Int32.Parse(drRR["RRWID"].ToString());
                    oResourceRequest.UpdateWorkflowStatus(intRRW, (int)ResourceRequestStatus.Closed, true);
                    int intRR = Int32.Parse(drRR["RRID"].ToString());
                    oResourceRequest.UpdateStatusOverall(intRR, (int)ResourceRequestStatus.Closed);
                }

                oAudit.UpdateErrorCompleted(intRequest, intService, intNumber);
                if (_delete_audit == true)
                {
                    oAudit.UpdateError(intRequest, intService, intNumber, 1, "");
                    // Delete all audit scripts and set step to start again
                    oAudit.DeleteServer(_serverid, intMIS);
                    oAudit.DeleteServerDetailRemote(intAudit);
                }
            }

            // Clear All Provisioning Errors
            DataSet dsErrors = GetErrors(_serverid);
            foreach (DataRow drError in dsErrors.Tables[0].Rows)
            {
                if (drError["fixed"].ToString() == "")
                {
                    int intRequest = Int32.Parse(drError["requestid"].ToString());
                    int intItem = Int32.Parse(drError["itemid"].ToString());
                    int intNumber = Int32.Parse(drError["number"].ToString());

                    DataSet dsRR = oResourceRequest.GetAllItem(intRequest, intItem, intNumber);
                    foreach (DataRow drRR in dsRR.Tables[0].Rows)
                    {
                        int intRRW = Int32.Parse(drRR["RRWID"].ToString());
                        oResourceRequest.UpdateWorkflowStatus(intRRW, (int)ResourceRequestStatus.Closed, true);
                        int intRR = Int32.Parse(drRR["RRID"].ToString());
                        oResourceRequest.UpdateStatusOverall(intRR, (int)ResourceRequestStatus.Closed);
                    }
                }
            }

            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@errorid", _errorid);
            arParams[3] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerError", arParams);

            OnDemand oOnDemand = new OnDemand(user, dsn);
            oOnDemand.DeleteStepDoneServer(_serverid, _step);
            UpdateZeusError(_serverid, 0);
            
            // Clear all installation errors
            ServerName oServerName = new ServerName(0, dsn);
            oServerName.UpdateComponentDetailSelecteds(_serverid, -10, -1);

            // Reset Pause Flag to 0 (since it will not be fixed while in paused state...while RR is being worked)
            UpdatePause(_serverid, 0);
        }

        public void UpdateError(int _id, string _incident, int _assigned)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@incident", _incident);
            arParams[2] = new SqlParameter("@assigned", _assigned);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerErrorIncident", arParams);
        }
        

        public void AddAccount(int _serverid, string _xid, int _domain, int _admin, string _localgroups, string _domaingroups, int _email)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@xid", _xid);
            arParams[2] = new SqlParameter("@domain", _domain);
            arParams[3] = new SqlParameter("@admin", _admin);
            arParams[4] = new SqlParameter("@localgroups", _localgroups);
            arParams[5] = new SqlParameter("@domaingroups", _domaingroups);
            arParams[6] = new SqlParameter("@email", _email);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerAccount", arParams);
        }
        public DataSet GetAccounts(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerAccounts", arParams);
        }
        public void DeleteAccount(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerAccount", arParams);
        }

        public void AddGeneric(int _serverid, string _vio1, string _vio2, string _vio1_dr, string _vio2_dr, string _vio1_prod, string _vio2_prod, string _ww1, string _ww2, string _ww1_dr, string _ww2_dr, string _ww1_prod, string _ww2_prod, string _dummy_name, string _dummy_name_dr, string _dummy_name_prod)
        {
            arParams = new SqlParameter[16];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@vio1", _vio1);
            arParams[2] = new SqlParameter("@vio2", _vio2);
            arParams[3] = new SqlParameter("@vio1_dr", _vio1_dr);
            arParams[4] = new SqlParameter("@vio2_dr", _vio2_dr);
            arParams[5] = new SqlParameter("@vio1_prod", _vio1_prod);
            arParams[6] = new SqlParameter("@vio2_prod", _vio2_prod);
            arParams[7] = new SqlParameter("@ww1", _ww1);
            arParams[8] = new SqlParameter("@ww2", _ww2);
            arParams[9] = new SqlParameter("@ww1_dr", _ww1_dr);
            arParams[10] = new SqlParameter("@ww2_dr", _ww2_dr);
            arParams[11] = new SqlParameter("@ww1_prod", _ww1_prod);
            arParams[12] = new SqlParameter("@ww2_prod", _ww2_prod);
            arParams[13] = new SqlParameter("@dummy_name", _dummy_name);
            arParams[14] = new SqlParameter("@dummy_name_dr", _dummy_name_dr);
            arParams[15] = new SqlParameter("@dummy_name_prod", _dummy_name_prod);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerGeneric", arParams);
        }
        public DataSet GetGeneric(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerGeneric", arParams);
        }
        public string GetGeneric(int _serverid, string _column)
        {
            DataSet ds = GetGeneric(_serverid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddOutput(int _serverid, string _type, string _output)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@type", _type);
            arParams[2] = new SqlParameter("@output", _output);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerOutput", arParams);
        }
        public DataSet GetOutput(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerOutputs", arParams);
        }
        public DataSet GetOutput(int _serverid, string _type)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@type", _type);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerOutput", arParams);
        }

        public string GetExecution(int intServer, int _environment, string _dsn_asset, string _dsn_ip, string _dsn_service_editor, int _assign_pageid, int _view_pageid)
        {
            string strExecutionServer = "";
            DataSet dsServer = Get(intServer);
            if (dsServer.Tables[0].Rows.Count > 0)
            {
                Log oLog = new Log(0, dsn);
                Forecast oForecast = new Forecast(0, dsn);
                ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
                VMWare oVMWare = new VMWare(0, dsn);
                int intVMwareClusterID = 0;
                Int32.TryParse(dsServer.Tables[0].Rows[0]["vmware_clusterid"].ToString(), out intVMwareClusterID);
                DataSet dsVMwareHost = oVMWare.GetHostNewResults(intServer);
                if (intVMwareClusterID > 0 && dsVMwareHost.Tables[0].Rows.Count > 0)
                {
                    DateTime datStart = DateTime.Parse(dsServer.Tables[0].Rows[0]["created"].ToString());
                    int intVMwareHostStep = 0;
                    foreach (DataRow drVMwareHost in dsVMwareHost.Tables[0].Rows)
                    {
                        intVMwareHostStep++;
                        DateTime datEnd = DateTime.Parse(drVMwareHost["modified"].ToString());
                        strExecutionServer += AddExecution(-999, 0, "Host Provisioning Message", drVMwareHost["results"].ToString(), datStart.ToString(), "---", 0.00, (drVMwareHost["error"].ToString() == "0" ? datEnd.ToString() : ""), intVMwareHostStep, (drVMwareHost["error"].ToString() == "1"), 0, false, _environment, "", 0);
                        datStart = datEnd;
                    }
                    strExecutionServer = GetExecutionTable(strExecutionServer, false, 0);
                }
                else
                {
                    int intAnswer = Int32.Parse(dsServer.Tables[0].Rows[0]["answerid"].ToString());
                    //if (oForecast.CanAutoProvision(intAnswer) == false)
                        //strExecutionServer = "<img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\" /> The model of this asset is not auto-provisioning enabled";
                    //else
                    //{
                        int intModel = Int32.Parse(dsServer.Tables[0].Rows[0]["modelid"].ToString());
                        int intName = Int32.Parse(dsServer.Tables[0].Rows[0]["nameid"].ToString());
                        int intCurrent = Int32.Parse(dsServer.Tables[0].Rows[0]["step"].ToString());
                        string strRebuild = dsServer.Tables[0].Rows[0]["rebuild"].ToString();
                        bool boolPNC = (dsServer.Tables[0].Rows[0]["pnc"].ToString() == "1");
                        DataSet ds = oForecast.GetAnswer(intAnswer);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int intMnemonic = 0;
                            Int32.TryParse(ds.Tables[0].Rows[0]["mnemonicid"].ToString(), out intMnemonic);
                            int intExecutedBy = 0;
                            Int32.TryParse(ds.Tables[0].Rows[0]["executed_by"].ToString(), out intExecutedBy);
                            if (intExecutedBy < 0)
                                strExecutionServer = "<img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\" /> This asset was imported prior to auto-provisioning";
                            else if (intExecutedBy == 0)
                                strExecutionServer = "<img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\" /> The design associated to this asset has not been executed";
                            else
                            {
                                ServerName oServerName = new ServerName(0, dsn);
                                Mnemonic oMnemonic = new Mnemonic(0, dsn);
                                Design oDesign = new Design(0, dsn);
                                OnDemand oOnDemand = new OnDemand(0, dsn);
                                OnDemandTasks oOnDemandTask = new OnDemandTasks(0, dsn);
                                Asset oAsset = new Asset(0, _dsn_asset);
                                Services oService = new Services(0, dsn);
                                Holidays oHoliday = new Holidays(0, dsn);
                                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                                Requests oRequest = new Requests(0, dsn);
                                Projects oProject = new Projects(0, dsn);
                                Users oUser = new Users(0, dsn);

                                if (oForecast.GetAnswer(intAnswer, "executed") != "")
                                {
                                    string strName = "Device";
                                    if (intName > 0)
                                    {
                                        if (boolPNC == true)
                                            strName = oServerName.GetNameFactory(intName, 0);
                                        else
                                            strName = oServerName.GetName(intName, 0);
                                    }

                                    int intStep = 0;
                                    int intType = oModelsProperties.GetType(intModel);
                                    DataSet dsSteps = oOnDemand.GetSteps(intType, 1);
                                    DataSet dsServers = GetAnswer(intAnswer);
                                    bool boolNotDone = false;

                                    if (intCurrent < 999)
                                    {
                                        foreach (DataRow drServers in dsServers.Tables[0].Rows)
                                        {
                                            int intDoneServer = Int32.Parse(drServers["id"].ToString());
                                            intStep = 0;
                                            foreach (DataRow drStep in dsSteps.Tables[0].Rows)
                                            {
                                                intStep++;
                                                DataSet dsResult = oOnDemand.GetStepDoneServer(intDoneServer, intStep);
                                                if (dsResult.Tables[0].Rows.Count > 0)
                                                {
                                                    if (dsResult.Tables[0].Rows[0]["error"].ToString() == "1")
                                                        boolNotDone = true;
                                                    if (dsResult.Tables[0].Rows[0]["finished"].ToString() == "")
                                                        boolNotDone = true;
                                                }
                                                else
                                                    boolNotDone = true;
                                            }
                                        }
                                    }

                                    PNCTasks oPNCTask = new PNCTasks(user, dsn);
                                    DataSet dsPNC = oPNCTask.GetStepsDesign(intAnswer, 0, intServer);
                                    DateTime datExecuted = DateTime.Parse(ds.Tables[0].Rows[0]["executed"].ToString());
                                    if (oForecast.CanAutoProvision(intAnswer) == true)
                                    {
                                        // Loop through the OnDemand Steps
                                        intStep = 0;
                                        DateTime datCompleted = datExecuted;
                                        DateTime datScheduled = DateTime.Now;
                                        if (DateTime.TryParse(ds.Tables[0].Rows[0]["execution"].ToString(), out datScheduled) == true)
                                            datCompleted = datScheduled;
                                        DateTime datStart = datCompleted;
                                        foreach (DataRow drStep in dsSteps.Tables[0].Rows)
                                        {
                                            intStep++;
                                            DataSet dsResult = oOnDemand.GetStepDoneServer(intServer, intStep);
                                            //if (intStep == 1 && strRebuild != "")
                                            //    datStart = DateTime.Parse(strRebuild);
                                            //else
                                                datStart = datCompleted;
                                            bool boolStepError = false;
                                            string incident = "";
                                            int assigned = 0;
                                            string strMessage = "";
                                            if (dsResult.Tables[0].Rows.Count > 0)
                                            {
                                                foreach (DataRow drResult in dsResult.Tables[0].Rows)
                                                    strMessage += drResult["result"].ToString();
                                                if (dsResult.Tables[0].Rows[0]["error"].ToString() == "1")
                                                {
                                                    boolStepError = true;
                                                    strMessage = dsResult.Tables[0].Rows[0]["result"].ToString();
                                                    DataSet dsError = GetError(intServer, intStep);
                                                    if (dsError.Tables[0].Rows.Count > 0)
                                                    {
                                                        incident = dsError.Tables[0].Rows[0]["incident"].ToString();
                                                        Int32.TryParse(dsError.Tables[0].Rows[0]["assigned"].ToString(), out assigned);
                                                    }
                                                }
                                                if (dsResult.Tables[0].Rows[0]["finished"].ToString() != "")
                                                    datCompleted = DateTime.Parse(dsResult.Tables[0].Rows[0]["finished"].ToString());
                                            }
                                            if (drStep["zeus"].ToString() == "1")
                                            {
                                                int intAsset = 0;
                                                DataSet dsAssets = GetAssets(intServer);
                                                foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                                                {
                                                    if (drAsset["latest"].ToString() == "0" && drAsset["dr"].ToString() == "0")
                                                    {
                                                        intAsset = Int32.Parse(drAsset["assetid"].ToString());
                                                        break;
                                                    }
                                                    else if (drAsset["latest"].ToString() == "1" && drAsset["dr"].ToString() == "0")
                                                    {
                                                        intAsset = Int32.Parse(drAsset["assetid"].ToString());
                                                        break;
                                                    }
                                                }
                                                if (intAsset > 0)
                                                    strMessage = "<p>[<a href=\"javascript:void(0);\" onclick=\"OpenWindow('ZEUS_STATUS','" + oAsset.Get(intAsset, "serial") + "');\">View Build Status</a>]</p>" + strMessage;
                                            }
                                            strExecutionServer += AddExecution(-999, 0, drStep["title"].ToString(), strMessage, datStart.ToString(), "---", 0.00, (intStep < intCurrent && boolStepError == false ? datCompleted.ToString() : ""), intStep, boolStepError, 0, (intStep > intCurrent), _environment, incident, assigned);
                                        }
                                    }
                                    else
                                    {
                                        string strManualCompleted = "";
                                        int intImplementorUser = 0;
                                        int intImplementorWorkflow = 0;
                                        int intImplementorParent = 0;
                                        DataSet dsImplementor = oOnDemandTask.GetPending(intAnswer);
                                        if (dsImplementor.Tables[0].Rows.Count > 0)
                                        {
                                            intImplementorWorkflow = Int32.Parse(dsImplementor.Tables[0].Rows[0]["resourceid"].ToString());
                                            intImplementorParent = Int32.Parse(oResourceRequest.GetWorkflow(intImplementorWorkflow, "parent"));
                                            intImplementorUser = Int32.Parse(oResourceRequest.GetWorkflow(intImplementorWorkflow, "userid"));
                                            DataSet dsManual = oOnDemandTask.GetGenericII(intAnswer);
                                            if (dsManual.Tables[0].Rows.Count > 0)
                                                strManualCompleted = dsManual.Tables[0].Rows[0]["chk5"].ToString();
                                            foreach (DataRow drPNC in dsPNC.Tables[0].Rows)
                                            {
                                                string strDateCreated = drPNC["created"].ToString();
                                                string strDateCompleted = drPNC["completed"].ToString();
                                                if (strDateCreated != "" && strDateCompleted == "")
                                                {
                                                    // There is another task open that the manual build is waiting on.
                                                    // Set the completed date to that created date
                                                    strManualCompleted = strDateCreated;
                                                }
                                            }
                                        }
                                        strExecutionServer += AddExecution(intImplementorUser, 0, "Manual Provisioning", (strManualCompleted == "" ? "" : "The server was provisioned successfully"), datExecuted.ToString(), "---", 0.00, strManualCompleted, intStep, false, intImplementorParent, false, _environment, "", 0);
                                    }

                                    // Add Post Imaging services (if applicable)
                                    string strCompleted = oForecast.GetAnswer(intAnswer, "completed");
                                    string strFinished = oForecast.GetAnswer(intAnswer, "finished");
                                    Customized oCustomized = new Customized(user, dsn);
                                    if (strFinished == "")
                                    {
                                        // Check and Set Finished (if all transparent tasks are done and non-transparent are open)
                                        bool boolFinishTransparentClosed = false;
                                        bool boolFinishTransparentOpen = false;
                                        bool boolFinishNonTransparentOpen = false;
                                        int intTransparent = 0;
                                        int intNonTransparent = 0;
                                        DateTime datLastOpen = DateTime.MinValue;
                                        foreach (DataRow drPNC in dsPNC.Tables[0].Rows)
                                        {
                                            bool boolNonTransparent = (drPNC["non_transparent"].ToString() == "1");
                                            string strDateCreated = drPNC["created"].ToString();
                                            string strDateCompleted = drPNC["completed"].ToString();
                                            if (boolNonTransparent == false)
                                            {
                                                intTransparent++;
                                                // Transparent
                                                if (strDateCreated == "" || strDateCompleted == "") // Hasn't been created, or is still open
                                                    boolFinishTransparentOpen = true;
                                                else
                                                {
                                                    if (DateTime.Parse(strDateCompleted) > datLastOpen)
                                                        datLastOpen = DateTime.Parse(strDateCompleted);
                                                    boolFinishTransparentClosed = true;
                                                }
                                            }
                                            else
                                            {
                                                intNonTransparent++;
                                                // Non-transparent
                                                if (strDateCreated != "" && strDateCompleted == "")
                                                    boolFinishNonTransparentOpen = true;
                                            }
                                        }
                                        if ((intTransparent == 0 || (boolFinishTransparentClosed && boolFinishTransparentOpen == false)) && (intNonTransparent == 0 || boolFinishNonTransparentOpen))
                                        {
                                            // All transparent tasks have been closed and at least one non-transparent task is open.
                                            // Finish the design (since it might take a while for the non-transparent ones to close). 
                                            // We do not want them lingering out there, because any new services could be initiated months later.
                                            strFinished = datLastOpen.ToString();
                                            oForecast.UpdateAnswerFinished(intAnswer, strFinished);
                                            // Now, re-initialize the design (send birth cert if necessary)
                                            int intRequest = oForecast.GetRequestID(intAnswer, true);
                                            oPNCTask.InitiateNextStep(intRequest, intAnswer, intModel, _environment, _assign_pageid, _view_pageid, _dsn_asset, _dsn_ip, _dsn_service_editor, false);
                                        }
                                    }

                                    int intStepIncomplete = 0;
                                    int intStepLastComplete = 0;
                                    bool boolIncomplete = false;
                                    foreach (DataRow drPNC in dsPNC.Tables[0].Rows)
                                    {
                                        int intStepPNC = Int32.Parse(drPNC["step"].ToString());
                                        int intServicePNC = Int32.Parse(drPNC["serviceid"].ToString());
                                        string strServicePNC = oService.GetName(intServicePNC);
                                        bool boolAssignImplementor = (drPNC["implementor"].ToString() == "1");
                                        bool boolAssignNetworkEngineer = (drPNC["network_engineer"].ToString() == "1");
                                        bool boolAssignDBA = (drPNC["dba"].ToString() == "1");
                                        bool boolAssignProjectManager = (drPNC["project_manager"].ToString() == "1");
                                        bool boolAssignDepartmentalManager = (drPNC["departmental_manager"].ToString() == "1");
                                        bool boolAssignApplicationLead = (drPNC["application_lead"].ToString() == "1");
                                        bool boolAssignAdministrativeContact = (drPNC["administrative_contact"].ToString() == "1");
                                        bool boolAssignApplicationOwner = (drPNC["application_owner"].ToString() == "1");
                                        bool boolAssignRequestor = (drPNC["requestor"].ToString() == "1");
                                        bool boolNonTransparent = (drPNC["non_transparent"].ToString() == "1");
                                        bool boolClient = (drPNC["client"].ToString() == "1");
                                        string strDateSLA = "";
                                        string strDateCreated = drPNC["created"].ToString();
                                        string strDateCompleted = drPNC["completed"].ToString();
                                        double dblTaskSLA = oService.GetSLA(intServicePNC);
                                        bool boolCreated = false;
                                        int intServiceUser = 0;
                                        int intWorkflowParent = Int32.Parse(drPNC["id"].ToString());
                                        int intRequest = oForecast.GetRequestID(intAnswer, true);

                                        if (strCompleted != "" && strFinished == "")
                                            oLog.AddEvent(intAnswer, strName, intRequest.ToString(), "* Checking Service " + strServicePNC + "...", LoggingType.Debug);
                                        if (strDateCreated != "")
                                        {
                                            // Task has been created
                                            boolCreated = true;
                                            intServiceUser = Int32.Parse(drPNC["userid"].ToString());
                                            if (strCompleted != "" && strFinished == "")
                                                oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => Task is already created", LoggingType.Debug);
                                        }
                                        else
                                        {
                                            if (strCompleted != "" && strFinished == "")
                                                oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => Task has NOT been created", LoggingType.Debug);
                                            // Task has not been created
                                            if ((boolIncomplete == false && intStepLastComplete < intStepPNC) || intStepLastComplete == intStepPNC || intStepIncomplete == intStepPNC)
                                            {
                                                if (strCompleted != "" && strFinished == "")
                                                    oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => Create Task", LoggingType.Debug);
                                                // Create task
                                                if (strCompleted != "" && boolNotDone == false && (strFinished == "" || (boolNonTransparent == true && boolIncomplete == true)))
                                                    // Not completed.  Done w/ Provisioning.  Not Finished or Transparent Finished and at least one task open, and this is a non-transparent.
                                                {
                                                    oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => Not completed.  Done w/ Provisioning.  Not Finished or Transparent Finished and at least one task open, and this is a non-transparent.", LoggingType.Debug);
                                                    boolCreated = true;
                                                    strDateCreated = DateTime.Now.ToString();

                                                    bool boolStorage = (drPNC["storage"].ToString() == "1");
                                                    bool boolTSM = (drPNC["tsm"].ToString() == "1");
                                                    bool boolLegato = (drPNC["legato"].ToString() == "1");
                                                    bool boolDNS = (drPNC["dns"].ToString() == "1");
                                                    int intProject = oRequest.GetProjectNumber(intRequest);

                                                    // Get Assigned User
                                                    int intUserAssigned = 0;
                                                    if (boolAssignImplementor == true) 
                                                    {
                                                        DataSet dsImplementor = oOnDemandTask.GetPending(intAnswer);
                                                        if (dsImplementor.Tables[0].Rows.Count > 0)
                                                        {
                                                            int intImplementorWorkflow = Int32.Parse(dsImplementor.Tables[0].Rows[0]["resourceid"].ToString());
                                                            intUserAssigned = Int32.Parse(oResourceRequest.GetWorkflow(intImplementorWorkflow, "userid"));
                                                            oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => Assigned User = " + intUserAssigned.ToString(), LoggingType.Debug);
                                                        }
                                                    }
                                                    else if (boolAssignNetworkEngineer == true)
                                                    {
                                                        Int32.TryParse(oForecast.GetAnswer(intAnswer, "networkengineer"), out intUserAssigned);
                                                        oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => Network Engineer = " + intUserAssigned.ToString(), LoggingType.Debug);
                                                    }
                                                    else if (boolAssignDBA == true)
                                                    {
                                                        Int32.TryParse(dsServer.Tables[0].Rows[0]["dba"].ToString(), out intUserAssigned);
                                                        oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => DBA = " + intUserAssigned.ToString(), LoggingType.Debug);
                                                    }
                                                    else if (boolAssignProjectManager == true)
                                                    {
                                                        string strMnemonic = oMnemonic.Get(intMnemonic, "factory_code");
                                                        string strPM = oMnemonic.GetFeed(strMnemonic, MnemonicFeed.PM);
                                                        if (strPM != "")
                                                            intUserAssigned = oUser.GetId(strPM);
                                                        else
                                                            intUserAssigned = oDesign.GetUser(oMnemonic.Get(intMnemonic, "PMName"));
                                                        //intUserAssigned = oDesign.GetUser(oMnemonic.Get(intMnemonic, "PMName"));
                                                        //Int32.TryParse(oProject.Get(intProject, "lead"), out intUserAssigned);
                                                        if (intUserAssigned == 0)
                                                            intUserAssigned = 14781;
                                                        oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => PMName = " + intUserAssigned.ToString(), LoggingType.Debug);
                                                    }
                                                    else if (boolAssignDepartmentalManager == true)
                                                    {
                                                        Int32.TryParse(oForecast.GetAnswer(intAnswer, "appcontact"), out intUserAssigned);
                                                        oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => appcontact = " + intUserAssigned.ToString(), LoggingType.Debug);
                                                    }
                                                    else if (boolAssignApplicationLead == true)
                                                    {
                                                        Int32.TryParse(oForecast.GetAnswer(intAnswer, "admin1"), out intUserAssigned);
                                                        oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => admin1 = " + intUserAssigned.ToString(), LoggingType.Debug);
                                                    }
                                                    else if (boolAssignAdministrativeContact == true)
                                                    {
                                                        Int32.TryParse(oForecast.GetAnswer(intAnswer, "admin2"), out intUserAssigned);
                                                        oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => admin2 = " + intUserAssigned.ToString(), LoggingType.Debug);
                                                    }
                                                    else if (boolAssignApplicationOwner == true)
                                                    {
                                                        Int32.TryParse(oForecast.GetAnswer(intAnswer, "appowner"), out intUserAssigned);
                                                        oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => appowner = " + intUserAssigned.ToString(), LoggingType.Debug);
                                                    }
                                                    else if (boolAssignRequestor == true)
                                                    {
                                                        Int32.TryParse(oForecast.GetAnswer(intAnswer, "executed_by"), out intUserAssigned);
                                                        oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => executed_by = " + intUserAssigned.ToString(), LoggingType.Debug);
                                                    }

                                                    if (boolClient == true)
                                                    {
                                                        // Mark that MIS has been notified
                                                        DataSet dsDesign = oDesign.GetAnswer(intAnswer);
                                                        if (dsDesign.Tables[0].Rows.Count > 0)
                                                        {
                                                            int intDesign = Int32.Parse(dsDesign.Tables[0].Rows[0]["id"].ToString());
                                                            oDesign.UpdateMISNotified(intDesign, DateTime.Now.ToString());
                                                            oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => MIS Notified = " + DateTime.Now.ToString(), LoggingType.Debug);
                                                        }
                                                    }

                                                    // Validate once more that the task has not already been created.
                                                    int intServiceItemId = oService.GetItemId(intServicePNC);
                                                    Classes oClass = new Classes(0, dsn);
                                                    int intClass = 0;
                                                    Int32.TryParse(oForecast.GetAnswer(intAnswer, "classid"), out intClass);
                                                    DataSet dsAlready = oOnDemandTask.GetServerOther(intServicePNC, intAnswer);
                                                    if (boolStorage)
                                                        dsAlready = oOnDemandTask.GetServerStorage(intAnswer, (oClass.IsProd(intClass) ? 1 : 0));
                                                    else if (boolTSM || boolLegato)
                                                        dsAlready = oOnDemandTask.GetServerBackup(intAnswer, intServiceItemId);

                                                    if (dsAlready.Tables[0].Rows.Count == 0)
                                                    {
                                                        oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => Generating request", LoggingType.Debug);
                                                        ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                                                        ServiceDetails oServiceDetail = new ServiceDetails(0, dsn);
                                                        int _number = oResourceRequest.GetNumber(intRequest, intServiceItemId);
                                                        if (boolStorage)
                                                        {
                                                            if (oClass.IsProd(intClass))
                                                                oOnDemandTask.AddServerStorage(intRequest, intServiceItemId, _number, intAnswer, 1, intModel);
                                                            else
                                                                oOnDemandTask.AddServerStorage(intRequest, intServiceItemId, _number, intAnswer, 0, intModel);
                                                            oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => Added to Storage (" + (oClass.IsProd(intClass) ? "Prod" : "Non-Prod") + ")", LoggingType.Debug);
                                                        }
                                                        else if (boolTSM || boolLegato)
                                                        {
                                                            oOnDemandTask.AddServerBackup(intRequest, intServiceItemId, _number, intAnswer, intModel);
                                                            oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => Added to Backup", LoggingType.Debug);
                                                        }
                                                        else
                                                        {
                                                            oOnDemandTask.AddServerOther(intRequest, intServicePNC, _number, intAnswer, intModel);
                                                            oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => Added to Server Other", LoggingType.Debug);
                                                        }
                                                        double dblServiceHours = oServiceDetail.GetHours(intServicePNC, 1);
                                                        intWorkflowParent = oServiceRequest.AddRequest(intRequest, intServiceItemId, intServicePNC, 1, dblServiceHours, 2, _number, _dsn_service_editor);
                                                        oServiceRequest.NotifyTeamLead(intServiceItemId, intWorkflowParent, _assign_pageid, _view_pageid, _environment, "", _dsn_service_editor, _dsn_asset, _dsn_ip, intUserAssigned);
                                                        oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => Team Lead Notified", LoggingType.Debug);
                                                    }
                                                    else
                                                        oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => Request has already been generated", LoggingType.Debug);
                                                }
                                            }
                                        }
                                        if (boolCreated)
                                        {
                                            if (strDateCompleted == "")
                                            {
                                                // Task is still open
                                                boolIncomplete = true;
                                                if (intStepIncomplete == 0)
                                                    intStepIncomplete = intStepPNC;
                                                if (strCompleted != "" && strFinished == "")
                                                    oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => boolIncomplete = true", LoggingType.Debug);
                                            }
                                            if (boolIncomplete == false)
                                                intStepLastComplete = intStepPNC;
                                            DateTime datTaskSLA = oHoliday.GetHours(dblTaskSLA, DateTime.Parse(strDateCreated));
                                            strDateSLA = datTaskSLA.ToString();
                                        }
                                        if (boolIncomplete == true && boolCreated && strDateCompleted == "" && intStepPNC > intStepLastComplete && intStepPNC > intStepIncomplete)
                                        {
                                            oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => The step was created out of order and is not done, so delete it", LoggingType.Debug);
                                            // The step was created out of order and is not done, so delete it
                                            oResourceRequest.Delete(intWorkflowParent);
                                            DataSet dsDelete = oResourceRequest.GetWorkflowsParent(intWorkflowParent);
                                            foreach (DataRow drDelete in dsDelete.Tables[0].Rows)
                                                oResourceRequest.DeleteWorkflow(Int32.Parse(drDelete["id"].ToString()));
                                            oOnDemandTask.DeleteServerOther(intServicePNC, intAnswer);
                                            boolCreated = false;
                                            //intWorkflow = 0;
                                            intWorkflowParent = 0;
                                            intServiceUser = 0;
                                        }
                                        if (intServiceUser == 0)
                                        {
                                            // Get User
                                            DataSet dsManager = oService.GetUser(intServicePNC, 0);  // Technicians
                                            foreach (DataRow drManager in dsManager.Tables[0].Rows)
                                            {
                                                intServiceUser = Int32.Parse(drManager["userid"].ToString());
                                                break;
                                            }
                                            if (intServiceUser == 0)
                                            {
                                                dsManager = oService.GetUser(intServicePNC, 1);  // Managers
                                                foreach (DataRow drManager in dsManager.Tables[0].Rows)
                                                {
                                                    intServiceUser = Int32.Parse(drManager["userid"].ToString());
                                                    break;
                                                }
                                            }
                                        }
                                        if (boolNonTransparent == false && (boolCreated || strFinished == ""))
                                        {
                                            if (strCompleted != "" && strFinished == "")
                                                oLog.AddEvent(intAnswer, strName, intRequest.ToString(), strServicePNC + " => AddExecution()", LoggingType.Debug);
                                            // Create the task
                                            intStep++;
                                            // Look up incident for manual task (that has been converted to automation)
                                            DataSet dsResult = oOnDemand.GetStepDoneServer(intServer, intServicePNC);
                                            string incident = "";
                                            int assigned = 0;
                                            DataSet dsError = GetError(intServer, intServicePNC);
                                            if (dsError.Tables[0].Rows.Count > 0)
                                            {
                                                incident = dsError.Tables[0].Rows[0]["incident"].ToString();
                                                Int32.TryParse(dsError.Tables[0].Rows[0]["assigned"].ToString(), out assigned);
                                                strExecutionServer += AddExecution(intServiceUser, intServicePNC, oService.GetName(intServicePNC), dsError.Tables[0].Rows[0]["reason"].ToString(), strDateCreated, strDateSLA, dblTaskSLA, strDateCompleted, intStep, true, intWorkflowParent, (boolCreated == false), _environment, incident, assigned);
                                            }
                                            else
                                                strExecutionServer += AddExecution(intServiceUser, intServicePNC, oService.GetName(intServicePNC), drPNC["comments"].ToString(), strDateCreated, strDateSLA, dblTaskSLA, strDateCompleted, intStep, (drPNC["status"].ToString() == "1"), intWorkflowParent, (boolCreated == false), _environment, "", 0);
                                        }
                                    }

                                    strExecutionServer = GetExecutionTable(strExecutionServer, false, 0);
                                    //strExecutionServer = "<div style=\"display:none\">" + strExecutionServer + "<br/></div>";
                                }
                            }
                        }
                    //}
                }
            }
            return strExecutionServer;
        }
        public string GetExecutionTable(string _table, bool _border, int _width)
        {
            return "<table" + (_width > 0 ? " width=\"" + _width.ToString() + "px\"" : "") + " cellpadding=\"7\" cellspacing=\"0\" border=\"0\"" + (_border ? " style=\"border:solid 1px #CCCCCC\"" : "") + ">" + _table + "</table>";
        }
        public string GetExecutionUser(int _userid)
        {
            Users oUser = new Users(0, dsn);
            if (_userid == -999)
                return "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"alert('ClearView is a system account which is used for automated tasks.');\">ClearView</a>";
            else if (_userid == 0)
                return "---";
            else
                return "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenWindow('PROFILE','?userid=" + _userid.ToString() + "');\">" + oUser.GetFullName(_userid) + " [" + oUser.GetName(_userid) + "]</a>";
        }
        private string AddExecution(int _userid, int _serviceid, string _title, string _result, string _start, string _sla, double _sla_hours, string _end, int _step, bool _error, int _resource, bool _waiting, int _environment, string _incident, int _assigned)
        {
            Services oService = new Services(0, dsn);
            Users oUser = new Users(0, dsn);
            Functions oFunction = new Functions(0, dsn, _environment);
            StringBuilder sbReturn = new StringBuilder();
            string strImage = "/images/bigArrowRight.gif";
            string strError = "";
            bool boolActive = true;
            if (_waiting == true)
            {
                strImage = "/images/ico_hourglass.gif";
                boolActive = false;
            }
            else if (_error == true)
            {
                strImage = "/images/ico_error.gif";
                boolActive = false;
                if (_result.Contains("~") == true)
                    strError = _result.Substring(0, _result.IndexOf("~"));
                else
                    strError = _result;
            }
            else if (_end != "")
            {
                strImage = "/images/ico_check.gif";
                boolActive = false;
            }
            if (string.IsNullOrEmpty(_incident) == false)
                _userid = _assigned;
            sbReturn.Append("<tr>");
            sbReturn.Append("<td valign=\"top\"></td>");
            sbReturn.Append("<td>");
            sbReturn.Append("<table width=\"700\" cellpadding=\"0\" cellspacing=\"5\" border=\"0\"");
            sbReturn.Append(boolActive ? " style=\"background-color:#FFFF99\"" : (_step % 2 == 0 ? "" : " style=\"background-color:#E6E6E6\""));
            sbReturn.Append(">");
            sbReturn.Append("<tr>");
            sbReturn.Append("<td valign=\"top\" align=\"center\">");
            if (_userid > 0)
            {
                sbReturn.Append("<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenWindow('PROFILE','?userid=");
                sbReturn.Append(_userid.ToString());
                sbReturn.Append("');\" title=\"");
                sbReturn.Append(oUser.GetFullName(_userid));
                sbReturn.Append("\">");
            }
            sbReturn.Append("<img src='");
            sbReturn.Append(_userid > 0 ? "/frame/picture.aspx?xid=" + oUser.GetName(_userid) : (_userid == -999 ? "/images/clearview.gif" : (_waiting == true ? "/images/nobody.gif" : "/images/nophoto.gif")));
            sbReturn.Append("' align='absmiddle' border='0' style='height:90px;width:90px;border-width:0px;border:solid 1px #666666;' />");
            if (_userid > 0)
                sbReturn.Append("</a>");
            sbReturn.Append("</td>");
            sbReturn.Append("<td width=\"100%\" valign=\"top\"><span class=\"" + (_waiting == true ? "headergray" : "header") + "\">");
            sbReturn.Append(_title);
            sbReturn.Append("</span><br/><br/>");
            string person = "";
            if (_error == true)
            {
                sbReturn.Append(_resource > 0 ? strError + "<p><a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(_resource.ToString()) + "', '800', '600');\">View Request Status</a></p>" : (_waiting == false ? (strError == "" ? "No Information" : strError) : "<i>Queued (will be initiated after the previous steps are completed)</i>"));
                // try to find incident
                if (string.IsNullOrEmpty(_incident) == false)
                {
                    sbReturn.Append("<br/><br/>Tracking # " + _incident);
                    DataSet dsKey = oFunction.GetSetupValuesByKey("INCIDENTS");
                    if (dsKey.Tables[0].Rows.Count > 0)
                    {
                        string incidents = dsKey.Tables[0].Rows[0]["Value"].ToString();
                        StreamReader theReader = new StreamReader(incidents);
                        string theContents = theReader.ReadToEnd();
                        string[] theLines = theContents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string theLine in theLines)
                        {
                            if (theLine.Contains(_incident))
                            {
                                string[] theFields = theLine.Split(new char[] { ',' }, StringSplitOptions.None);
                                person = theFields[5].Replace("\"", "");
                                if (String.IsNullOrEmpty(person) == false)
                                    sbReturn.Append("<br/>Assigned To: " + person);
                                string group = theFields[4].Replace("\"", "");
                                if (String.IsNullOrEmpty(group) == false)
                                    sbReturn.Append("<br/>Group: " + group);
                                string prior = theFields[3].Replace("\"", "");
                                if (String.IsNullOrEmpty(prior) == false)
                                {
                                    int priority = 0;
                                    DateTime datStarted = DateTime.MinValue;
                                    if (Int32.TryParse(prior.Substring(0, 1), out priority) == true
                                        && DateTime.TryParse(_start, out datStarted) == true)
                                    {
                                        if (priority == 1)
                                            _sla_hours = 2.00;
                                        else if (priority == 2)
                                            _sla_hours = 4.00;
                                        else if (priority == 3)
                                            _sla_hours = (24.00 * 2.00);
                                        else
                                            _sla_hours = (24.00 * 10.00);
                                        _sla = datStarted.AddHours(_sla_hours).ToString();
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                sbReturn.Append("<br/><br/><a class='build_error' href=\"javascript:void(0);\" onclick=\"OpenWindow('PROVISIONING_ERROR', '");
                sbReturn.Append(oFunction.encryptQueryString(_result) + "&incident=" + _incident);
                sbReturn.Append("');\"><img src='/images/plus.gif' border='0' align='absmiddle'/>&nbsp;For more information about this error, click here</a>");
            }
            else
                sbReturn.Append(_resource > 0 ? _result + "<p><a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(_resource.ToString()) + "', '800', '600');\">View Request Status</a></p>" : (_waiting == false ? (_result == "" ? "No Information" : _result) : "<i>Queued (will be initiated after the previous steps are completed)</i>"));
            sbReturn.Append("</td>");
            sbReturn.Append("</tr>");
            sbReturn.Append("<tr>");
            sbReturn.Append("<td nowrap align=\"center\">");
            sbReturn.Append(strImage == "" ? "" : "<img src=\"" + strImage + "\" align=\"absmiddle\" border=\"0\"/>");
            sbReturn.Append("</td>");
            sbReturn.Append("<td nowrap valign=\"top\">");
            sbReturn.Append("<table cellpadding=\"0\" cellspacing=\"5\" border=\"0\">");
            sbReturn.Append("<tr><td nowrap><b>Resource:</td><td width=\"100%\">");
            if (String.IsNullOrEmpty(person))
                sbReturn.Append(GetExecutionUser(_userid));
            else
                sbReturn.Append(person);
            sbReturn.Append("</td></tr>");
            bool boolSLABreach = false;
            try
            {
                DateTime datSLA = DateTime.Parse(_sla);
                if (DateTime.Now > datSLA)
                    boolSLABreach = true;
            }
            catch { }
            sbReturn.Append("<tr><td nowrap><b>Started:</td><td width=\"100%\">");
            sbReturn.Append(_waiting == false ? _start : "---");
            sbReturn.Append("</td></tr>");
            if (_end == "" && boolSLABreach == true)
            {
                sbReturn.Append("<tr><td nowrap><b>Target (SLA):</td><td width=\"100%\" class=\"note\">");
                sbReturn.Append(_waiting == false ? _sla : "---");
                sbReturn.Append(" (");
                sbReturn.Append(_sla_hours.ToString("F"));
                sbReturn.Append(" HRs)</td></tr>");
            }
            else
            {
                sbReturn.Append("<tr><td nowrap><b>Target (SLA):</td><td width=\"100%\">");
                sbReturn.Append(_waiting == false ? _sla : "---");
                sbReturn.Append(" (");
                sbReturn.Append(_sla_hours.ToString("F"));
                sbReturn.Append(" HRs)</td></tr>");
            }
            sbReturn.Append("<tr><td nowrap><b>Completed:</td><td width=\"100%\">");
            sbReturn.Append(_waiting == false && _error == false && _end != "" ? _end : "---");
            sbReturn.Append("</td></tr>");
            sbReturn.Append("</table>");
            sbReturn.Append("</tr>");
            sbReturn.Append("</table>");
            sbReturn.Append("</td>");
            sbReturn.Append("</tr>");
            return sbReturn.ToString();
        }
        public DataSet GetTSM(string _servername)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@servername", _servername);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerTSM", arParams);
        }
        public DataSet GetDecommissionAll(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDecommission", arParams);
        }

        public void AddSwitchport(int _serverid, string _switch, string _interface)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@switch", _switch);
            arParams[2] = new SqlParameter("@interface", _interface);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerSwitchport", arParams);
        }
        public void DeleteSwitchports(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerSwitchports", arParams);
        }
        public DataSet GetSwitchport(int _serverid, string _switch, string _interface)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@switch", _switch);
            arParams[2] = new SqlParameter("@interface", _interface);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerSwitchport", arParams);
        }

        public void ClearMDT(int _serverid, string _dsn_asset, string _dsn_zeus, int _environment)
        {
            // Clear MDT Records
            DataSet dsServer = Get(_serverid);
            if (dsServer.Tables[0].Rows.Count == 1)
            {
                OperatingSystems oOperatingSystem = new OperatingSystems(user, dsn);
                ModelsProperties oModelsProperties = new ModelsProperties(user, dsn);
                BuildLocation oBuildLocation = new BuildLocation(user, dsn);
                Forecast oForecast = new Forecast(user, dsn);
                Asset oAsset = new Asset(user, _dsn_asset, dsn);
                Variables oVariable = new Variables(_environment);
                Functions oFunction = new Functions(user, dsn, _environment);
                Log oLog = new Log(user, dsn);
                Zeus oZeus = new Zeus(user, _dsn_zeus);
                Resiliency oResiliency = new Resiliency(user, dsn);

                DataRow drServer = dsServer.Tables[0].Rows[0];
                int intAnswer = Int32.Parse(drServer["answerid"].ToString());
                DataSet dsAnswer = oForecast.GetAnswer(intAnswer);
                if (dsAnswer.Tables[0].Rows.Count == 1)
                {
                    DataRow drAnswer = dsAnswer.Tables[0].Rows[0];
                    try
                    {
                        DataSet dsZeus = oZeus.GetBuildServer(_serverid);
                        if (dsZeus.Tables[0].Rows.Count == 1)
                        {
                            DataRow drZeus = dsZeus.Tables[0].Rows[0];
                            string strMAC1 = drZeus["macaddress"].ToString();
                            string strZeusMAC1 = oFunction.FormatMAC(strMAC1, ":");
                            string strMAC2 = drZeus["macaddress2"].ToString();
                            string strZeusMAC2 = "";
                            if (strMAC2 != "")
                                strZeusMAC2 = oFunction.FormatMAC(strMAC2, ":");

                            int intModel = Int32.Parse(drServer["modelid"].ToString());
                            int intOS = Int32.Parse(drServer["osid"].ToString());
                            bool boolRDPAltiris = (oOperatingSystem.Get(intOS, "rdp_altiris") == "1");
                            bool boolRDPMDT = (oOperatingSystem.Get(intOS, "rdp_mdt") == "1");
                            if (oModelsProperties.IsDell(intModel) == true && oOperatingSystem.IsSolaris(intOS) == false)
                            {
                                // Windows 2008, Windows 2003 and RHEL on DELLs have to go through the same build VLAN (920).
                                // So, they will have to be kicked off by Windows Deployment Toolkit.
                                boolRDPMDT = true;
                                boolRDPAltiris = false;
                            }
                            int intClass = Int32.Parse(drAnswer["classid"].ToString());
                            int intEnv = Int32.Parse(drAnswer["environmentid"].ToString());
                            int intAddress = Int32.Parse(drAnswer["addressid"].ToString());
                            int intMnemonic = 0;
                            if (drAnswer["mnemonicid"].ToString() != "")
                                intMnemonic = Int32.Parse(drAnswer["mnemonicid"].ToString());
                            bool boolBIR = (drAnswer["resiliency"].ToString() == "1");    // Related to the Business Resiliency Effort (use old data center strategy)
                            int intResiliency = oResiliency.GetIDFromMnemonic(intMnemonic, boolBIR, intAnswer);

                            int intAsset = 0;
                            DataSet dsAssets = GetAssetsServer(_serverid);
                            foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                            {
                                if (drAsset["latest"].ToString() == "1" && Int32.TryParse(drAsset["id"].ToString(), out intAsset) == true)
                                    break;
                            }
                            int intZone = 0;
                            if (intAsset > 0 && oAsset.GetServerOrBlade(intAsset, "zoneid") != "")
                                Int32.TryParse(oAsset.GetServerOrBlade(intAsset, "zoneid"), out intZone);
                            string strSerial = oAsset.Get(intAsset, "serial");
                            string strName = GetName(_serverid, true);
                            DataSet dsBuildRDP = oBuildLocation.GetRDPs(intClass, intEnv, intAddress, intResiliency, (boolRDPAltiris ? 1 : 0), (boolRDPMDT ? 1 : 0), 0, intZone);
                            if (dsBuildRDP.Tables[0].Rows.Count > 0)
                            {
                                string strRDPMDTWebService = dsBuildRDP.Tables[0].Rows[0]["rdp_mdt_ws"].ToString();
                                string strRDPComputerWebService = dsBuildRDP.Tables[0].Rows[0]["rdp_computer_ws"].ToString();

                                if (boolRDPMDT)
                                {
                                    System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                                    BuildSubmit oMDT = new BuildSubmit();
                                    oMDT.Proxy = oVariable.GetProxy(dsn);
                                    oMDT.Credentials = oCredentials;
                                    oMDT.Url = strRDPMDTWebService;
                                    try
                                    {
                                        oLog.AddEvent(strName, strSerial, "Running ForceCleanup MDT on " + strRDPMDTWebService + " (" + strName + ", " + strZeusMAC1 + ", " + "ServerShare" + ")", LoggingType.Information);
                                        oMDT.ForceCleanup(strName, strZeusMAC1, "ServerShare");
                                        oLog.AddEvent(strName, strSerial, "Waiting 10 seconds...", LoggingType.Debug);
                                        System.Threading.Thread.Sleep(10000);
                                    }
                                    catch { }
                                    if (strZeusMAC2 != "")
                                    {
                                        try
                                        {
                                            oLog.AddEvent(strName, strSerial, "Running ForceCleanup MDT on " + strRDPMDTWebService + " (" + strName + ", " + strZeusMAC2 + ", " + "ServerShare" + ")", LoggingType.Information);
                                            oMDT.ForceCleanup(strName, strZeusMAC2, "ServerShare");
                                            oLog.AddEvent(strName, strSerial, "Waiting 5 seconds...", LoggingType.Debug);
                                            System.Threading.Thread.Sleep(5000);
                                        }
                                        catch { }
                                    }
                                }
                                if (boolRDPAltiris)
                                {
                                    System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.AltirisUsername(), oVariable.AltirisPassword(), oVariable.AltirisDomain());
                                    AltirisWS_Computer.ComputerManagementService oComputer = new AltirisWS_Computer.ComputerManagementService();
                                    oComputer.Credentials = oCredentials;
                                    oComputer.Url = strRDPComputerWebService;
                                    int intDeleteComputer = oComputer.GetComputerID(strName, 1);
                                    oLog.AddEvent(strName, strSerial, "Deleting Altiris ComputerID " + intDeleteComputer.ToString() + " on " + strRDPComputerWebService, LoggingType.Information);
                                    if (intDeleteComputer > 0)
                                    {
                                        bool boolDelete = oComputer.DeleteComputer(intDeleteComputer);
                                        oLog.AddEvent(strName, strSerial, "Finished Deleting Altiris", LoggingType.Information);
                                    }
                                    else
                                        oLog.AddEvent(strName, strSerial, "Finished Deleting Altiris (Not Found)", LoggingType.Information);
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
        }


        #region Avamar

        public void UpdateAvamar(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerAvamar", arParams);
        }
        public DataSet GetAvamar(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerAvamar", arParams);
        }

        // REGISTRATIONS
        public DataSet GetAvamarRegistrations()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServersAvamarRegistrations");
        }
        public int AddAvamar(int _serverid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@id", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerAvamar", arParams);
            return Int32.Parse(arParams[1].Value.ToString());
        }
        public void UpdateAvamarRegistrationStarted(int _serverid, string _started)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@started", (_started == "" ? SqlDateTime.Null : DateTime.Parse(_started)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerAvamarRegistrationStarted", arParams);
        }
        public void UpdateAvamarRegistrationCompleted(int _serverid, int _gridid, int _domainid, string _command, string _output, string _completed, int _error)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@gridid", _gridid);
            arParams[2] = new SqlParameter("@domainid", _domainid);
            arParams[3] = new SqlParameter("@command", _command);
            arParams[4] = new SqlParameter("@output", _output);
            arParams[5] = new SqlParameter("@completed", (_completed == "" ? SqlDateTime.Null : DateTime.Parse(_completed)));
            arParams[6] = new SqlParameter("@error", _error);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerAvamarRegistrationCompleted", arParams);
        }
        public void AddAvamarGroup(int _serverid, int _groupid, string _command, string _output)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@groupid", _groupid);
            arParams[2] = new SqlParameter("@command", _command);
            arParams[3] = new SqlParameter("@output", _output);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerAvamarGroup", arParams);
        }
        

        // ACTIVATIONS
        public DataSet GetAvamarActivations()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServersAvamarActivations");
        }
        public int AddAvamarActivation(int _serverid, string _servername, string _grid, string _domain, string _group1, string _group2, string _group3, string _online)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@servername", _servername);
            arParams[2] = new SqlParameter("@grid", _grid);
            arParams[3] = new SqlParameter("@domain", _domain);
            arParams[4] = new SqlParameter("@group1", _group1);
            arParams[5] = new SqlParameter("@group2", _group2);
            arParams[6] = new SqlParameter("@group3", _group3);
            arParams[7] = new SqlParameter("@online", (_online == "" ? SqlDateTime.Null : DateTime.Parse(_online)));
            arParams[8] = new SqlParameter("@id", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerAvamarActivation", arParams);
            return Int32.Parse(arParams[8].Value.ToString());
        }
        public void UpdateAvamarActivationStarted(int _serverid, string _started)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@started", (_started == "" ? SqlDateTime.Null : DateTime.Parse(_started)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerAvamarActivationStarted", arParams);
        }
        public void UpdateAvamarActivationCompleted(int _serverid, string _command, string _output, string _completed, int _error)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@command", _command);
            arParams[2] = new SqlParameter("@output", _output);
            arParams[3] = new SqlParameter("@completed", (_completed == "" ? SqlDateTime.Null : DateTime.Parse(_completed)));
            arParams[4] = new SqlParameter("@error", _error);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerAvamarActivationCompleted", arParams);
        }


        // BACKUPS
        public DataSet GetAvamarBackups()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServersAvamarBackups");
        }
        public int AddAvamarBackup(int _serverid, string _servername, string _grid, string _domain, string _group)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@servername", _servername);
            arParams[2] = new SqlParameter("@grid", _grid);
            arParams[3] = new SqlParameter("@domain", _domain);
            arParams[4] = new SqlParameter("@group", _group);
            arParams[5] = new SqlParameter("@id", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerAvamarBackup", arParams);
            return Int32.Parse(arParams[5].Value.ToString());
        }
        public void UpdateAvamarBackupCompleted(int _serverid, string _output, string _completed, int _error)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@output", _output);
            arParams[2] = new SqlParameter("@completed", (_completed == "" ? SqlDateTime.Null : DateTime.Parse(_completed)));
            arParams[3] = new SqlParameter("@error", _error);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerAvamarBackupCompleted", arParams);
        }

        #endregion


        public void AddRebuild(int _requestid, int _serviceid, int _number, int _serverid, string _change)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@serverid", _serverid);
            arParams[4] = new SqlParameter("@change", _change);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerRebuild", arParams);
        }
        public DataSet GetRebuild(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerRebuild", arParams);
        }
        public DataSet GetRebuild(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerRebuildID", arParams);
        }
        public void UpdateRebuild(int _requestid, int _serviceid, int _number, int _serverid, string _change)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@serverid", _serverid);
            arParams[4] = new SqlParameter("@change", _change);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerRebuilds", arParams);
        }
        public void UpdateRebuildCompleted(int _serverid, string _completed)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@completed", (_completed == "" ? SqlDateTime.Null : DateTime.Parse(_completed)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerRebuildCompleted", arParams);
        }
        
    }
}
