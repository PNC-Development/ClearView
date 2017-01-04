using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using Tamir.SharpSsh;

namespace NCC.ClearView.Application.Core
{
    public class Solaris
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;

        public Solaris(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet GetBuildNetworks(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSolarisBuildNetworks", arParams);
        }
        public DataSet GetBuildNetwork(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSolarisBuildNetwork", arParams);
        }
        public string GetBuildNetwork(int _id, string _column)
        {
            DataSet ds = GetBuildNetwork(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddBuildNetwork(string _name, int _display, int _enabled)
		{
			arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@display", _display);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSolarisBuildNetwork", arParams);
		}
        public void UpdateBuildNetwork(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSolarisBuildNetwork", arParams);
        }
        public void UpdateBuildNetworkOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSolarisBuildNetworkOrder", arParams);
        }
        public void EnableBuildNetwork(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSolarisBuildNetworkEnabled", arParams);
        }
        public void DeleteBuildNetwork(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteSolarisBuildNetwork", arParams);
        }


        public DataSet GetInterfaces(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSolarisInterfaces", arParams);
        }
        public DataSet GetInterface(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSolarisInterface", arParams);
        }
        public string GetInterface(int _id, string _column)
        {
            DataSet ds = GetInterface(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddInterface(string _name, int _display, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@display", _display);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSolarisInterface", arParams);
        }
        public void UpdateInterface(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSolarisInterface", arParams);
        }
        public void UpdateInterfaceOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSolarisInterfaceOrder", arParams);
        }
        public void EnableInterface(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSolarisInterfaceEnabled", arParams);
        }
        public void DeleteInterface(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteSolarisInterface", arParams);
        }


        public DataSet GetBuildTypes(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSolarisBuildTypes", arParams);
        }
        public DataSet GetBuildType(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSolarisBuildType", arParams);
        }
        public string GetBuildType(int _id, string _column)
        {
            DataSet ds = GetBuildType(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddBuildType(string _name, int _display, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@display", _display);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSolarisBuildType", arParams);
        }
        public void UpdateBuildType(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSolarisBuildType", arParams);
        }
        public void UpdateBuildTypeOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSolarisBuildTypeOrder", arParams);
        }
        public void EnableBuildType(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSolarisBuildTypeEnabled", arParams);
        }
        public void DeleteBuildType(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteSolarisBuildType", arParams);
        }

        public string GetMacAddress(string _host, int _boot_groupid, int _environment, Log oLog, string _name, string _serial)
        {
            oLog.AddEvent(_name, _serial, "Starting MAC Address lookup...", LoggingType.Debug);
            string strMAC = "";
            if (_boot_groupid > 0)
            {
                Models oModel = new Models(0, dsn);
                string strUsername = oModel.GetBootGroup(_boot_groupid, "username");
                string strPassword = oModel.GetBootGroup(_boot_groupid, "password");
                string strExpects = oModel.GetBootGroup(_boot_groupid, "regular");
                string strCommand = oModel.GetBootGroup(_boot_groupid, "mac_query_command");
                string strStart = oModel.GetBootGroup(_boot_groupid, "mac_query_substring_start");
                oLog.AddEvent(_name, _serial, "Connecting...", LoggingType.Debug);
                SshShell oSSHshell = new SshShell(_host, strUsername, strPassword);
                oSSHshell.RemoveTerminalEmulationCharacters = true;
                oSSHshell.Connect();
                string[] strExpect = strExpects.Split(new char[] { '|' });
                if (oSSHshell.Connected == true && oSSHshell.ShellOpened == true)
                {
                    // Wait for prompt
                    string strBanner = oSSHshell.Expect(strExpects);
                    oLog.AddEvent(_name, _serial, "Received banner prompt = " + strBanner, LoggingType.Debug);
                    if (IsInOutput(strBanner, strExpect) == false)
                    {
                        oLog.AddEvent(_name, _serial, "Did not recieve all the output...trying again # 1...", LoggingType.Debug);
                        strBanner = oSSHshell.Expect(strExpects);
                        oLog.AddEvent(_name, _serial, "Received banner prompt # 1 = " + strBanner, LoggingType.Debug);
                    }
                    if (IsInOutput(strBanner, strExpect) == false)
                    {
                        oLog.AddEvent(_name, _serial, "Did not recieve all the output...trying again # 2...", LoggingType.Debug);
                        strBanner = oSSHshell.Expect(strExpects);
                        oLog.AddEvent(_name, _serial, "Received banner prompt # 2 = " + strBanner, LoggingType.Debug);
                    }
                    if (IsInOutput(strBanner, strExpect) == false)
                    {
                        oLog.AddEvent(_name, _serial, "Did not recieve all the output...trying again # 3...", LoggingType.Debug);
                        strBanner = oSSHshell.Expect(strExpects);
                        oLog.AddEvent(_name, _serial, "Received banner prompt # 3 = " + strBanner, LoggingType.Debug);
                    }
                    if (IsInOutput(strBanner, strExpect) == true)
                    {
                        oLog.AddEvent(_name, _serial, "Writing command = " + strCommand, LoggingType.Debug);
                        // Send Command to get MAC Address
                        oSSHshell.WriteLine(strCommand);
                        // Wait for prompt
                        strMAC = oSSHshell.Expect(strExpects);
                        oLog.AddEvent(_name, _serial, "Received Response = " + strMAC, LoggingType.Debug);
                        oLog.AddEvent(_name, _serial, "Parsing using " + strStart, LoggingType.Debug);
                        strMAC = ParseOutput(strMAC, strStart, Environment.NewLine);
                    }
                }
                oSSHshell.Close();
            }
            return strMAC;
        }
        public bool IsInOutput(string _output, string[] _expects)
        {
            bool boolReturn = false;
            foreach (string _expect in _expects)
            {
                if (_output.ToUpper().Contains(_expect.ToUpper()) == true)
                {
                    boolReturn = true;
                    break;
                }
            }
            return boolReturn;
        }
        public string ParseOutput(string _output, string _start, string _end)
        {
            if (_output.Contains(_start) == true)
            {
                string strBeginning = _output.Substring(_output.IndexOf(_start) + _start.Length).Trim();
                if (strBeginning.Contains(_end) == true)
                {
                    strBeginning = strBeginning.Substring(0, strBeginning.IndexOf(_end)).Trim();
                    return strBeginning.Trim();
                }
                else
                    return "";
            }
            else
                return "";
        }


        public DataSet GetSVEClusters(int _available, int _mnemonics, int _trunking, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@available", _available);
            arParams[1] = new SqlParameter("@mnemonics", _mnemonics);
            arParams[2] = new SqlParameter("@trunking", _trunking);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSVEClusters", arParams);
        }
        public DataSet GetSVEClustersAssign(int _db, int _classid, int _addressid, int _resiliencyid, int _serverid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@db", _db);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[4] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSVEClustersAssign", arParams);
        }
        public DataSet GetSVECluster(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSVECluster", arParams);
        }
        public string GetSVECluster(int _id, string _column)
        {
            DataSet ds = GetSVECluster(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public int AddSVECluster(string _name, int _db, int _classid, int _resiliencyid, int _networks, int _available, string _comments, int _storage_allocated, int _trunking, int _enabled)
        {
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@db", _db);
            arParams[2] = new SqlParameter("@classid", _classid);
            arParams[3] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[4] = new SqlParameter("@networks", _networks);
            arParams[5] = new SqlParameter("@available", _available);
            arParams[6] = new SqlParameter("@comments", _comments);
            arParams[7] = new SqlParameter("@storage_allocated", _storage_allocated);
            arParams[8] = new SqlParameter("@trunking", _trunking);
            arParams[9] = new SqlParameter("@enabled", _enabled);
            arParams[10] = new SqlParameter("@id", SqlDbType.Int);
            arParams[10].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSVECluster", arParams);
            return Int32.Parse(arParams[10].Value.ToString());
        }
        public void UpdateSVECluster(int _id, string _name, int _db, int _classid, int _resiliencyid, int _networks, int _available, string _comments, int _storage_allocated, int _trunking, int _enabled)
        {
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@db", _db);
            arParams[3] = new SqlParameter("@classid", _classid);
            arParams[4] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[5] = new SqlParameter("@networks", _networks);
            arParams[6] = new SqlParameter("@available", _available);
            arParams[7] = new SqlParameter("@comments", _comments);
            arParams[8] = new SqlParameter("@storage_allocated", _storage_allocated);
            arParams[9] = new SqlParameter("@trunking", _trunking);
            arParams[10] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSVECluster", arParams);
        }
        public void EnableSVECluster(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSVEClusterEnabled", arParams);
        }
        public void DeleteSVECluster(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteSVECluster", arParams);
        }


        // NETWORKS
        public void AddSVENetwork(int _clusterid, int _networkid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@networkid", _networkid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSVENetwork", arParams);
        }
        public void DeleteSVENetwork(int _clusterid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteSVENetwork", arParams);
        }
        public DataSet GetSVENetworks(int _clusterid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSVENetworks", arParams);
        }


        // LOCATIONS
        public int AddSVELocation(int _clusterid, int _addressid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@addressid", _addressid);
            arParams[2] = new SqlParameter("@id", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSVELocation", arParams);
            return Int32.Parse(arParams[2].Value.ToString());
        }
        public void DeleteSVELocation(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteSVELocation", arParams);
        }
        public DataSet GetSVELocations(int _clusterid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSVELocations", arParams);
        }


        public void AddSVEGuest(int _clusterid, int _serverid, int _poolid, double _allocated, int _cpu, int _ram)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@serverid", _serverid);
            arParams[2] = new SqlParameter("@poolid", _poolid);
            arParams[3] = new SqlParameter("@allocated", _allocated);
            arParams[4] = new SqlParameter("@cpu", _cpu);
            arParams[5] = new SqlParameter("@ram", _ram);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSVEGuest", arParams);
        }
        public void UpdateSVEGuestCluster(int _serverid, int _clusterid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSVEGuestCluster", arParams);
        }
        public void UpdateSVEGuest(int _serverid, int _poolid, double _allocated, int _cpu, int _ram)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@poolid", _poolid);
            arParams[2] = new SqlParameter("@allocated", _allocated);
            arParams[3] = new SqlParameter("@cpu", _cpu);
            arParams[4] = new SqlParameter("@ram", _ram);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSVEGuest", arParams);
        }
        public DataSet GetSVEGuest(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSVEGuest", arParams);
        }
        public string GetSVEGuest(int _serverid, string _column)
        {
            DataSet ds = GetSVEGuest(_serverid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetSVEGuests(int _clusterid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSVEGuests", arParams);
        }
        public DataSet GetSVEGuestsByPool(int _poolid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@poolid", _poolid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSVEGuestsByPool", arParams);
        }

        public DataSet GetProcessorPools(int _clusterid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProcessorPools", arParams);
        }
        public DataSet GetProcessorPool(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProcessorPool", arParams);
        }
        public string GetProcessorPool(int _id, string _column)
        {
            DataSet ds = GetProcessorPool(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddProcessorPool(int _clusterid, string _name, string _description, int _low, int _high, int _warning, int _critical, int _error, int _enabled)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@description", _description);
            arParams[3] = new SqlParameter("@low", _low);
            arParams[4] = new SqlParameter("@high", _high);
            arParams[5] = new SqlParameter("@warning", _warning);
            arParams[6] = new SqlParameter("@critical", _critical);
            arParams[7] = new SqlParameter("@error", _error);
            arParams[8] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProcessorPool", arParams);
        }
        public void UpdateProcessorPool(int _id, int _clusterid, string _name, string _description, int _low, int _high, int _warning, int _critical, int _error, int _enabled)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@description", _description);
            arParams[4] = new SqlParameter("@low", _low);
            arParams[5] = new SqlParameter("@high", _high);
            arParams[6] = new SqlParameter("@warning", _warning);
            arParams[7] = new SqlParameter("@critical", _critical);
            arParams[8] = new SqlParameter("@error", _error);
            arParams[9] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProcessorPool", arParams);
        }
        public void DeleteProcessorPool(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteProcessorPool", arParams);
        }

        public DataSet GetSVEMnemonics(int _clusterid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSVEMnemonics", arParams);
        }
        public DataSet GetSVEMnemonic(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSVEMnemonic", arParams);
        }
        public string GetSVEMnemonic(int _id, string _column)
        {
            DataSet ds = GetSVEMnemonic(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddSVEMnemonic(int _clusterid, int _mnemonicid, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@mnemonicid", _mnemonicid);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSVEMnemonic", arParams);
        }
        public void UpdateSVEMnemonic(int _id, int _clusterid, int _mnemonicid, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            arParams[2] = new SqlParameter("@mnemonicid", _mnemonicid);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSVEMnemonic", arParams);
        }
        public void DeleteSVEMnemonic(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteSVEMnemonic", arParams);
        }
    }
}
