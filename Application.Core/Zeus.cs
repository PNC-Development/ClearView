using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class Zeus
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Zeus(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public string AddBuild(int _serverid, int _workstationid, int _vmware_workstationid, string _serial, string _asset, string _name, string _array_config, string _os, string _os_version, int _sp, string _build_type, string _domain, int _environment, string _source, int _disable_hba, string _macaddress, string _macaddress2, string _ipaddress, string _ipaddress_vlan, string _ipaddress_subnet, string _ipaddress_gateway, string _ipbackup, string _ipbackup_vlan, string _ipbackup_subnet, string _ipbackup_gateway, string _interface1, string _mpipaddress1, string _interface2, string _mpipaddress2, string _build_flags, string _flags, int _wipedata)
        {
            DataSet dsMAC = GetBuildMAC(_macaddress, 1);
            if (dsMAC.Tables[0].Rows.Count > 0)
                return "Duplicate MAC address found in the imaging configuration database ~ (1) " + _macaddress;
            else
            {
                dsMAC = GetBuildMAC(_macaddress2, 2);
                if (_macaddress2.Trim() != "" && dsMAC.Tables[0].Rows.Count > 0)
                    return "Duplicate MAC address found in the imaging configuration database ~ (2) " + _macaddress2;
                else
                {
                    arParams = new SqlParameter[32];
                    arParams[0] = new SqlParameter("@serverid", _serverid);
                    arParams[1] = new SqlParameter("@workstationid", _workstationid);
                    arParams[2] = new SqlParameter("@vmware_workstationid", _vmware_workstationid);
                    arParams[3] = new SqlParameter("@serial", _serial);
                    arParams[4] = new SqlParameter("@asset", _asset);
                    arParams[5] = new SqlParameter("@name", _name);
                    arParams[6] = new SqlParameter("@array_config", _array_config);
                    arParams[7] = new SqlParameter("@os", _os);
                    arParams[8] = new SqlParameter("@os_version", _os_version);
                    arParams[9] = new SqlParameter("@sp", _sp);
                    arParams[10] = new SqlParameter("@build_type", _build_type);
                    arParams[11] = new SqlParameter("@domain", _domain);
                    arParams[12] = new SqlParameter("@environment", _environment);
                    arParams[13] = new SqlParameter("@source", _source);
                    arParams[14] = new SqlParameter("@disable_hba", _disable_hba);
                    arParams[15] = new SqlParameter("@macaddress", _macaddress);
                    arParams[16] = new SqlParameter("@macaddress2", _macaddress2);
                    arParams[17] = new SqlParameter("@ipaddress", _ipaddress);
                    arParams[18] = new SqlParameter("@ipaddress_vlan", _ipaddress_vlan);
                    arParams[19] = new SqlParameter("@ipaddress_subnet", _ipaddress_subnet);
                    arParams[20] = new SqlParameter("@ipaddress_gateway", _ipaddress_gateway);
                    arParams[21] = new SqlParameter("@ipbackup", _ipbackup);
                    arParams[22] = new SqlParameter("@ipbackup_vlan", _ipbackup_vlan);
                    arParams[23] = new SqlParameter("@ipbackup_subnet", _ipbackup_subnet);
                    arParams[24] = new SqlParameter("@ipbackup_gateway", _ipbackup_gateway);
                    arParams[25] = new SqlParameter("@interface1", _interface1);
                    arParams[26] = new SqlParameter("@mpipaddress1", _mpipaddress1);
                    arParams[27] = new SqlParameter("@interface2", _interface2);
                    arParams[28] = new SqlParameter("@mpipaddress2", _mpipaddress2);
                    arParams[29] = new SqlParameter("@build_flags", _build_flags);
                    arParams[30] = new SqlParameter("@flags", _flags);
                    arParams[31] = new SqlParameter("@wipedata", _wipedata);
                    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addZeusBuild", arParams);
                    return "";
                }
            }
        }
        public void UpdateBuildRebuild(string _name, string _serial, int _wipedata)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@serial", _serial);
            arParams[2] = new SqlParameter("@wipedata", _wipedata);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateZeusBuildRebuild", arParams);
        }
        public void UpdateBuildDHCP(string _name, string _serial, string _dhcp)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@serial", _serial);
            arParams[2] = new SqlParameter("@dhcp", _dhcp);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateZeusBuildDHCP", arParams);
        }
        public void DeleteBuild(string _serial)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serial", _serial);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteZeusBuild", arParams);
        }
        public void DeleteBuildName(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteZeusBuildName", arParams);
        }
        //public void DeleteBuildMAC(string _macaddress)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@macaddress", _macaddress);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteZeusBuildMAC", arParams);
        //}
        public DataSet GetBuildServer(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZeusBuildServer", arParams);
        }
        public DataSet GetBuildServer(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZeusBuildName", arParams);
        }
        public DataSet GetBuildMAC(string _macaddress, int _number)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@macaddress", _macaddress);
            arParams[1] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZeusBuildMAC", arParams);
        }
        public DataSet GetResults(string _serial)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serial", _serial);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZeusResults", arParams);
        }
        public DataSet GetResult(string _serial, int _error)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serial", _serial);
            arParams[1] = new SqlParameter("@error", _error);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZeusResult", arParams);
        }
        public void UpdateResults(string _serial)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serial", _serial);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateZeusResults", arParams);
        }
        public void DeleteResults(string _serial)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serial", _serial);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteZeusResult", arParams);
        }
        public void DeleteResults()
        {
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteZeusResults");
        }
        public DataSet GetBuildWorkstation(int _workstationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZeusBuildWorkstation", arParams);
        }
        public DataSet GetBuildWorkstationVMware(int _vmware_workstationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@vmware_workstationid", _vmware_workstationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZeusBuildWorkstationVMware", arParams);
        }


        public void AddApp(string _serial, string _name, string _location)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serial", _serial);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@location", _location);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addZeusApp", arParams);
        }
        public void DeleteApps(string _serial)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serial", _serial);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteZeusApps", arParams);
        }
        public DataSet GetApps(string _serial)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serial", _serial);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZeusApps", arParams);
        }


        public void AddPNCGroup(int _serverid, string _name, string _ipaddress)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@ipaddress", _ipaddress);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addZeusPNCGroup", arParams);
        }
        public void UpdatePNCGroup(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateZeusPNCGroup", arParams);
        }
        public void UpdatePNCGroup(int _serverid, string _result)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@result", _result);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateZeusPNCGroupResult", arParams);
        }
        public DataSet GetPNCGroups()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZeusPNCGroups");
        }
        public DataSet GetPNCGroup(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZeusPNCGroup", arParams);
        }

        public void AddLun(string _serial, string _path, int _size)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serial", _serial);
            arParams[1] = new SqlParameter("@path", _path);
            arParams[2] = new SqlParameter("@size", _size);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addZeusLun", arParams);
        }
        public void DeleteLuns(string _serial)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serial", _serial);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteZeusLuns", arParams);
        }
        public DataSet GetLuns(string _serial)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serial", _serial);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZeusLuns", arParams);
        }
        public void AddLuns(int intAnswer, string _dsn, string _dsn_asset)
        {
            Servers oServer = new Servers(0, _dsn);
            OperatingSystems oOperatingSystem = new OperatingSystems(0, _dsn);
            Asset oAsset = new Asset(0, _dsn_asset);
            DataSet dsServer = oServer.GetAnswer(intAnswer);
            foreach (DataRow drServer in dsServer.Tables[0].Rows)
            {
                int intServer = Int32.Parse(drServer["id"].ToString());
                int intOS = Int32.Parse(drServer["osid"].ToString());
                bool boolFileSystem = oOperatingSystem.IsMidrange(intOS);
                string strName = oServer.GetName(intServer, true);
                int intAsset = 0;
                DataSet dsAsset = oServer.GetAssets(intServer);
                foreach (DataRow drAsset in dsAsset.Tables[0].Rows)
                {
                    if (drAsset["latest"].ToString() == "1")
                        intAsset = Int32.Parse(drAsset["assetid"].ToString());
                }
                if (intAsset > 0)
                {
                    string strSerial = oAsset.Get(intAsset, "serial");
                    int intModel = Int32.Parse(oAsset.Get(intAsset, "modelid"));
                    GetLuns(strSerial, strName, intAnswer, boolFileSystem, Int32.Parse(drServer["clusterid"].ToString()), Int32.Parse(drServer["csmconfigid"].ToString()), Int32.Parse(drServer["number"].ToString()), _dsn);
                    GetLunsShared(strSerial, strName, intAnswer, boolFileSystem, _dsn);
                }
            }
        }
        private void GetLuns(string strSerial, string strName, int intAnswer, bool boolFileSystem, int intCluster, int intCSMConfig, int intNumber, string _dsn)
        {
            Storage oStorage = new Storage(0, _dsn);
            Forecast oForecast = new Forecast(0, _dsn);
            DataSet dsLuns = new DataSet();
            if (intCluster == 0)
                dsLuns = oStorage.GetLuns(intAnswer, 0, intCluster, intCSMConfig, intNumber);
            else
                dsLuns = oStorage.GetLunsClusterNonShared(intAnswer, intCluster, intNumber);
            bool boolOverride = (oForecast.GetAnswer(intAnswer, "storage_override") == "1");
            AddLun(strSerial, strName, dsLuns, boolFileSystem, boolOverride, _dsn);
        }
        private void GetLunsShared(string strSerial, string strName, int intAnswer, bool boolFileSystem, string _dsn)
        {
            Servers oServer = new Servers(0, _dsn);
            Storage oStorage = new Storage(0, _dsn);
            Forecast oForecast = new Forecast(0, _dsn);
            bool boolOverride = (oForecast.GetAnswer(intAnswer, "storage_override") == "1");
            int intClusterOLD = 0;
            DataSet dsCluster = oServer.GetAnswerClusters(intAnswer);
            foreach (DataRow drCluster in dsCluster.Tables[0].Rows)
            {
                int intClusterID = Int32.Parse(drCluster["clusterid"].ToString());
                if (intClusterOLD != intClusterID)
                {
                    if (intClusterID > 0)
                    {
                        DataSet dsLuns = oStorage.GetLunsClusterShared(intAnswer, intClusterID);
                        AddLun(strSerial, strName, dsLuns, boolFileSystem, boolOverride, _dsn);
                    }
                }
                intClusterOLD = intClusterID;
            }
        }
        private void AddLun(string strSerial, string strName, DataSet dsLuns, bool boolFileSystem, bool boolOverride, string _dsn)
        {
            Storage oStorage = new Storage(0, _dsn);
            foreach (DataRow drLun in dsLuns.Tables[0].Rows)
            {
                string strLetter = drLun["letter"].ToString();
                if (strLetter == "")
                {
                    if (drLun["driveid"].ToString() == "-1000")
                        strLetter = "E";
                    else if (drLun["driveid"].ToString() == "-100")
                        strLetter = "F";
                    else if (drLun["driveid"].ToString() == "-10")
                        strLetter = "P";
                    else if (drLun["driveid"].ToString() == "-1")
                        strLetter = "Q";
                }
                string strPath = strLetter + ":" + drLun["path"].ToString();
                if ((boolOverride == true && drLun["driveid"].ToString() == "0") || boolFileSystem == true)
                    strPath = drLun["path"].ToString();
                int intSize = Int32.Parse(drLun["size"].ToString());
                if (intSize == 0)
                    intSize = Int32.Parse(drLun["size_qa"].ToString());
                if (intSize == 0)
                    intSize = Int32.Parse(drLun["size_test"].ToString());
                AddLun(strSerial, strPath, intSize);
                DataSet dsPoints = oStorage.GetMountPoints(Int32.Parse(drLun["id"].ToString()));
                int intPoint = 0;
                foreach (DataRow drPoint in dsPoints.Tables[0].Rows)
                {
                    intPoint++;
                    strPath = strLetter + ":\\SH" + drLun["driveid"].ToString() + "VOL" + (intPoint < 10 ? "0" : "") + intPoint.ToString();
                    if (boolFileSystem == true)
                        strPath = drPoint["path"].ToString();
                    intSize = Int32.Parse(drPoint["size"].ToString());
                    if (intSize == 0)
                        intSize = Int32.Parse(drPoint["size_qa"].ToString());
                    if (intSize == 0)
                        intSize = Int32.Parse(drPoint["size_test"].ToString());
                    AddLun(strSerial, strPath, intSize);
                }
            }
        }

        #region ArrayConfig
        public int AddArrayConfig(string _name, string _array_config, string _array_config_san, string _array_config_cluster, int _display, int _enabled)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@array_config", _array_config);
            arParams[2] = new SqlParameter("@array_config_san", _array_config_san);
            arParams[3] = new SqlParameter("@array_config_cluster", _array_config_cluster);
            arParams[4] = new SqlParameter("@display", _display);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            arParams[6] = new SqlParameter("@id", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addZeusArrayConfig", arParams);
            return Int32.Parse(arParams[6].Value.ToString());
        }
        public void UpdateArrayConfig(int _id, string _name, string _array_config, string _array_config_san, string _array_config_cluster, int _enabled)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@array_config", _array_config);
            arParams[3] = new SqlParameter("@array_config_san", _array_config_san);
            arParams[4] = new SqlParameter("@array_config_cluster", _array_config_cluster);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateZeusArrayConfig", arParams);
        }
        public void UpdateArrayConfigOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateZeusArrayConfigOrder", arParams);
        }
        public void EnableArrayConfig(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateZeusArrayConfigEnabled", arParams);
        }
        public void DeleteArrayConfig(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteZeusArrayConfig", arParams);
        }
        public DataSet GetArrayConfig(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZeusArrayConfig", arParams);
        }
        public string GetArrayConfig(int _id, string _column)
        {
            DataSet ds = GetArrayConfig(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetArrayConfigs(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZeusArrayConfigs", arParams);
        }
        #endregion

        #region BuildType
        public int AddBuildType(string _name, string _build_type, int _display, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@build_type", _build_type);
            arParams[2] = new SqlParameter("@display", _display);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            arParams[4] = new SqlParameter("@id", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addZeusBuildType", arParams);
            return Int32.Parse(arParams[4].Value.ToString());
        }
        public void UpdateBuildType(int _id, string _name, string _build_type, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@build_type", _build_type);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateZeusBuildType", arParams);
        }
        public void UpdateBuildTypeOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateZeusBuildTypeOrder", arParams);
        }
        public void EnableBuildType(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateZeusBuildTypeEnabled", arParams);
        }
        public void DeleteBuildType(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteZeusBuildType", arParams);
        }
        public DataSet GetBuildType(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZeusBuildType", arParams);
        }
        public string GetBuildType(int _id, string _column)
        {
            DataSet ds = GetBuildType(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetBuildTypes(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getZeusBuildTypes", arParams);
        }
        #endregion

        #region TSM
        public void AddTSM(string _serial, string _name, string _tsm_server, string _tsm_port, string _tsm_domain, string _tsm_schedule, string _tsm_cloptset)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@serial", _serial);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@tsm_server", _tsm_server);
            arParams[3] = new SqlParameter("@tsm_port", _tsm_port);
            arParams[4] = new SqlParameter("@tsm_domain", _tsm_domain);
            arParams[5] = new SqlParameter("@tsm_schedule", _tsm_schedule);
            arParams[6] = new SqlParameter("@tsm_cloptset", _tsm_cloptset);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addZeusTSM", arParams);
        }
        public void DeleteTSM(string _serial, string _name)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serial", _serial);
            arParams[1] = new SqlParameter("@name", _name);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteZeusTSM", arParams);
        }
        public void UpdateTSM(string _serial, string _name)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serial", _serial);
            arParams[1] = new SqlParameter("@name", _name);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateZeusTSM", arParams);
        }
        #endregion
    }
}
