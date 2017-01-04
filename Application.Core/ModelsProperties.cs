using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class ModelsProperties
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public ModelsProperties(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Gets(int _available, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@available", _available);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelProperties", arParams);
        }
        public DataSet GetTypes(int _available, int _typeid, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@available", _available);
            arParams[1] = new SqlParameter("@typeid", _typeid);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelPropertiesType", arParams);
        }
        public DataSet GetPlatforms(int _available, int _platformid, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@available", _available);
            arParams[1] = new SqlParameter("@platformid", _platformid);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelPropertiesPlatform", arParams);
        }
        public DataSet GetModels(int _available, int _modelid, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@available", _available);
            arParams[1] = new SqlParameter("@modelid", _modelid);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelPropertiesModel", arParams);
        }
        public int Get(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelPropertyName", arParams);
            if (ds.Tables[0].Rows.Count > 0)
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
            else
                return 0;
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelProperty", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public int GetType(int _id)
        {
            int intModel = 0;
            int intType = 0;
            Int32.TryParse(Get(_id, "modelid"), out intModel);
            Models oModel = new Models(user, dsn);
            Int32.TryParse(oModel.Get(intModel, "typeid"), out intType);
            return intType;
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteModelProperty", arParams);
        }
        public void AddNetwork(int _modelid, int _available, int _replicate_times, double _amp, int _network_ports, int _enabled)
		{
			arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            arParams[1] = new SqlParameter("@available", _available);
            arParams[2] = new SqlParameter("@replicate_times", _replicate_times);
            arParams[3] = new SqlParameter("@amp", _amp);
            arParams[4] = new SqlParameter("@network_ports", _network_ports);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addModelPropertyNetwork", arParams);
		}
        public void UpdateNetwork(int _id, int _modelid, int _available, int _replicate_times, double _amp, int _network_ports, int _enabled)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@modelid", _modelid);
            arParams[2] = new SqlParameter("@available", _available);
            arParams[3] = new SqlParameter("@replicate_times", _replicate_times);
            arParams[4] = new SqlParameter("@amp", _amp);
            arParams[5] = new SqlParameter("@network_ports", _network_ports);
            arParams[6] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateModelPropertyNetwork", arParams);
        }
        public void AddOther(int _modelid, int _available, int _replicate_times, double _amp, int _network_ports, int _storage_ports, string _name, int _ram, int _cpu_count, double _cpu_speed, int _chipset, double _storageThresholdMin, double _storageThresholdMax, int _asset_category, int _high_availability, int _high_performance, int _low_performance, int _enforce_1_1_recovery, int _no_many_1_recovery, int _vmware_virtual, int _ibm_virtual, int _sun_virtual, int _storage_db_boot_local, int _storage_db_boot_san_windows, int _storage_db_boot_san_unix, int _storage_de_fdrive_can_local, int _storage_de_fdrive_must_san, int _storage_de_fdrive_only, int _manual_build, int _type_blade, int _type_physical, int _type_vmware, int _type_enclosure, int _config_service_pack, int _config_vmware_template, int _config_maintenance_level, int _vio, int _fabric, int _storage_type, int _inventory, int _dell, int _dellconfigid, int _configure_switches, int _enter_name, int _enter_ip, int _associate_project, int _full_height, int _enabled)
        {
            arParams = new SqlParameter[48];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            arParams[1] = new SqlParameter("@available", _available);
            arParams[2] = new SqlParameter("@replicate_times", _replicate_times);
            arParams[3] = new SqlParameter("@amp", _amp);
            arParams[4] = new SqlParameter("@network_ports", _network_ports);
            arParams[5] = new SqlParameter("@storage_ports", _storage_ports);
            arParams[6] = new SqlParameter("@name", _name);
            arParams[7] = new SqlParameter("@ram", _ram);
            arParams[8] = new SqlParameter("@cpu_count", _cpu_count);
            arParams[9] = new SqlParameter("@cpu_speed", _cpu_speed);
            arParams[10] = new SqlParameter("@chipset", _chipset);
            arParams[11] = new SqlParameter("@high_availability", _high_availability);
            arParams[12] = new SqlParameter("@high_performance", _high_performance);
            arParams[13] = new SqlParameter("@low_performance", _low_performance);
            arParams[14] = new SqlParameter("@enforce_1_1_recovery", _enforce_1_1_recovery);
            arParams[15] = new SqlParameter("@no_many_1_recovery", _no_many_1_recovery);
            arParams[16] = new SqlParameter("@vmware_virtual", _vmware_virtual);
            arParams[17] = new SqlParameter("@ibm_virtual", _ibm_virtual);
            arParams[18] = new SqlParameter("@sun_virtual", _sun_virtual);
            arParams[19] = new SqlParameter("@storage_db_boot_local", _storage_db_boot_local);
            arParams[20] = new SqlParameter("@storage_db_boot_san_windows", _storage_db_boot_san_windows);
            arParams[21] = new SqlParameter("@storage_db_boot_san_unix", _storage_db_boot_san_unix);
            arParams[22] = new SqlParameter("@storage_de_fdrive_can_local", _storage_de_fdrive_can_local);
            arParams[23] = new SqlParameter("@storage_de_fdrive_must_san", _storage_de_fdrive_must_san);
            arParams[24] = new SqlParameter("@storage_de_fdrive_only", _storage_de_fdrive_only);
            arParams[25] = new SqlParameter("@manual_build", _manual_build);
            arParams[26] = new SqlParameter("@type_blade", _type_blade);
            arParams[27] = new SqlParameter("@type_physical", _type_physical);
            arParams[28] = new SqlParameter("@type_vmware", _type_vmware);
            arParams[29] = new SqlParameter("@type_enclosure", _type_enclosure);
            arParams[30] = new SqlParameter("@config_service_pack", _config_service_pack);
            arParams[31] = new SqlParameter("@config_vmware_template", _config_vmware_template);
            arParams[32] = new SqlParameter("@config_maintenance_level", _config_maintenance_level);
            arParams[33] = new SqlParameter("@vio", _vio);
            arParams[34] = new SqlParameter("@fabric", _fabric);
            arParams[35] = new SqlParameter("@storage_type", _storage_type);
            arParams[36] = new SqlParameter("@inventory", _inventory);
            arParams[37] = new SqlParameter("@dell", _dell);
            arParams[38] = new SqlParameter("@dellconfigid", _dellconfigid);
            arParams[39] = new SqlParameter("@configure_switches", _configure_switches);
            arParams[40] = new SqlParameter("@enter_name", _enter_name);
            arParams[41] = new SqlParameter("@enter_ip", _enter_ip);
            arParams[42] = new SqlParameter("@associate_project", _associate_project);
            arParams[43] = new SqlParameter("@enabled", _enabled);
            arParams[44] = new SqlParameter("@StorageThresholdMin", _storageThresholdMin);
            arParams[45] = new SqlParameter("@StorageThresholdMax", _storageThresholdMax);
            arParams[46] = new SqlParameter("@asset_category", _asset_category);
            arParams[47] = new SqlParameter("@full_height", _full_height);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addModelPropertyOther", arParams);
        }
        public void UpdateOther(int _id, int _modelid, int _available, int _replicate_times, double _amp, int _network_ports, int _storage_ports, string _name, int _ram, int _cpu_count, double _cpu_speed, int _chipset, double _storageThresholdMin, double _storageThresholdMax, int _asset_category, int _high_availability, int _high_performance, int _low_performance, int _enforce_1_1_recovery, int _no_many_1_recovery, int _vmware_virtual, int _ibm_virtual, int _sun_virtual, int _storage_db_boot_local, int _storage_db_boot_san_windows, int _storage_db_boot_san_unix, int _storage_de_fdrive_can_local, int _storage_de_fdrive_must_san, int _storage_de_fdrive_only, int _manual_build, int _type_blade, int _type_physical, int _type_vmware, int _type_enclosure, int _config_service_pack, int _config_vmware_template, int _config_maintenance_level, int _vio, int _fabric, int _storage_type, int _inventory, int _dell, int _dellconfigid, int _configure_switches, int _enter_name, int _enter_ip, int _associate_project, int _full_height, int _enabled)
        {
            arParams = new SqlParameter[49];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@modelid", _modelid);
            arParams[2] = new SqlParameter("@available", _available);
            arParams[3] = new SqlParameter("@replicate_times", _replicate_times);
            arParams[4] = new SqlParameter("@amp", _amp);
            arParams[5] = new SqlParameter("@network_ports", _network_ports);
            arParams[6] = new SqlParameter("@storage_ports", _storage_ports);
            arParams[7] = new SqlParameter("@name", _name);
            arParams[8] = new SqlParameter("@ram", _ram);
            arParams[9] = new SqlParameter("@cpu_count", _cpu_count);
            arParams[10] = new SqlParameter("@cpu_speed", _cpu_speed);
            arParams[11] = new SqlParameter("@chipset", _chipset);
            arParams[12] = new SqlParameter("@high_availability", _high_availability);
            arParams[13] = new SqlParameter("@high_performance", _high_performance);
            arParams[14] = new SqlParameter("@low_performance", _low_performance);
            arParams[15] = new SqlParameter("@enforce_1_1_recovery", _enforce_1_1_recovery);
            arParams[16] = new SqlParameter("@no_many_1_recovery", _no_many_1_recovery);
            arParams[17] = new SqlParameter("@vmware_virtual", _vmware_virtual);
            arParams[18] = new SqlParameter("@ibm_virtual", _ibm_virtual);
            arParams[19] = new SqlParameter("@sun_virtual", _sun_virtual);
            arParams[20] = new SqlParameter("@storage_db_boot_local", _storage_db_boot_local);
            arParams[21] = new SqlParameter("@storage_db_boot_san_windows", _storage_db_boot_san_windows);
            arParams[22] = new SqlParameter("@storage_db_boot_san_unix", _storage_db_boot_san_unix);
            arParams[23] = new SqlParameter("@storage_de_fdrive_can_local", _storage_de_fdrive_can_local);
            arParams[24] = new SqlParameter("@storage_de_fdrive_must_san", _storage_de_fdrive_must_san);
            arParams[25] = new SqlParameter("@storage_de_fdrive_only", _storage_de_fdrive_only);
            arParams[26] = new SqlParameter("@manual_build", _manual_build);
            arParams[27] = new SqlParameter("@type_blade", _type_blade);
            arParams[28] = new SqlParameter("@type_physical", _type_physical);
            arParams[29] = new SqlParameter("@type_vmware", _type_vmware);
            arParams[30] = new SqlParameter("@type_enclosure", _type_enclosure);
            arParams[31] = new SqlParameter("@config_service_pack", _config_service_pack);
            arParams[32] = new SqlParameter("@config_vmware_template", _config_vmware_template);
            arParams[33] = new SqlParameter("@config_maintenance_level", _config_maintenance_level);
            arParams[34] = new SqlParameter("@vio", _vio);
            arParams[35] = new SqlParameter("@fabric", _fabric);
            arParams[36] = new SqlParameter("@storage_type", _storage_type);
            arParams[37] = new SqlParameter("@inventory", _inventory);
            arParams[38] = new SqlParameter("@dell", _dell);
            arParams[39] = new SqlParameter("@dellconfigid", _dellconfigid);
            arParams[40] = new SqlParameter("@configure_switches", _configure_switches);
            arParams[41] = new SqlParameter("@enter_name", _enter_name);
            arParams[42] = new SqlParameter("@enter_ip", _enter_ip);
            arParams[43] = new SqlParameter("@associate_project", _associate_project);
            arParams[44] = new SqlParameter("@enabled", _enabled);
            arParams[45] = new SqlParameter("@StorageThresholdMin", _storageThresholdMin);
            arParams[46] = new SqlParameter("@StorageThresholdMax", _storageThresholdMax);
            arParams[47] = new SqlParameter("@asset_category", _asset_category);
            arParams[48] = new SqlParameter("@full_height", _full_height);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateModelPropertyOther", arParams);
        }


        public DataSet GetThresholds(int _propertyid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@propertyid", _propertyid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelPropertyThresholds", arParams);
        }
        public int GetThresholdQuantity(int _propertyid, int _quantity, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@propertyid", _propertyid);
            arParams[1] = new SqlParameter("@quantity", _quantity);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelPropertyThresholdQuantity", arParams);
            if (ds.Tables[0].Rows.Count > 0)
                return Int32.Parse(ds.Tables[0].Rows[0]["number_days"].ToString());
            else
                return 0;
        }
        public DataSet GetThreshold(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelPropertyThreshold", arParams);
        }
        public string GetThreshold(int _id, string _column)
        {
            DataSet ds = GetThreshold(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddThreshold(int _propertyid, int _qty_from, int _qty_to, int _number_days, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@propertyid", _propertyid);
            arParams[1] = new SqlParameter("@qty_from", _qty_from);
            arParams[2] = new SqlParameter("@qty_to", _qty_to);
            arParams[3] = new SqlParameter("@number_days", _number_days);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addModelPropertyThreshold", arParams);
        }
        public void UpdateThreshold(int _id, int _qty_from, int _qty_to, int _number_days, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@qty_from", _qty_from);
            arParams[2] = new SqlParameter("@qty_to", _qty_to);
            arParams[3] = new SqlParameter("@number_days", _number_days);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateModelPropertyThreshold", arParams);
        }
        public void EnableThreshold(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateModelPropertyThresholdEnabled", arParams);
        }
        public void DeleteThreshold(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteModelPropertyThreshold", arParams);
        }

        // Booleans based on specific model properties
        public bool IsStorageDB_BootLocal(int _id)
        {
            return (Get(_id, "storage_db_boot_local") == "1");
        }
        public bool IsStorageDB_BootSANWindows(int _id)
        {
            return (Get(_id, "storage_db_boot_san_windows") == "1");
        }
        public bool IsStorageDB_BootSANUnix(int _id)
        {
            return (Get(_id, "storage_db_boot_san_unix") == "1");
        }
        public bool IsHighAvailability(int _id)
        {
            return (Get(_id, "high_availability") == "1");
        }
        public bool IsEnforce1to1Recovery(int _id)
        {
            return (Get(_id, "enforce_1_1_recovery") == "1");
        }
        public bool IsNoManyto1Recovery(int _id)
        {
            return (Get(_id, "no_many_1_recovery") == "1");
        }
        public bool IsVMwareVirtual(int _id)
        {
            return (Get(_id, "vmware_virtual") == "1");
        }
        public bool IsIBMVirtual(int _id)
        {
            return (Get(_id, "ibm_virtual") == "1");
        }
        public bool IsSUNVirtual(int _id)
        {
            return (Get(_id, "sun_virtual") == "1");
        }
        public bool IsTypeVMware(int _id)
        {
            return (Get(_id, "type_vmware") == "1");
        }
        public bool IsTypeBlade(int _id)
        {
            return (Get(_id, "type_blade") == "1");
        }
        public bool IsTypePhysical(int _id)
        {
            return (Get(_id, "type_physical") == "1");
        }
        public bool IsTypeEnclosure(int _id)
        {
            return (Get(_id, "type_enclosure") == "1");
        }
        public bool IsStorageDE_FDriveCanBeLocal(int _id)
        {
            return (Get(_id, "storage_de_fdrive_can_local") == "1");
        }
        public bool IsStorageDE_FDriveMustBeOnSAN(int _id)
        {
            return (Get(_id, "storage_de_fdrive_must_san") == "1");
        }
        public bool IsStorageDE_FDriveOnly(int _id)
        {
            return (Get(_id, "storage_de_fdrive_only") == "1");
        }
        //public bool IsNotExecutable(int _id)
        //{
        //    return (Get(_id, "not_executable") == "1");
        //}
        //public bool IsClientCanExecute(int _id)
        //{
        //    return (Get(_id, "client_can_execute") == "1");
        //}
        public bool IsManualBuild(int _id)
        {
            return (Get(_id, "manual_build") == "1");
        }
        public bool IsInventory(int _id)
        {
            return (Get(_id, "inventory") == "1");
        }
        public bool IsDell(int _id)
        {
            return (Get(_id, "dell") == "1");
        }
        public bool IsConfigureSwitches(int _id)
        {
            return (Get(_id, "configure_switches") == "1");
        }
        public bool IsEnterName(int _id)
        {
            return (Get(_id, "enter_name") == "1");
        }
        public bool IsEnterIP(int _id)
        {
            return (Get(_id, "enter_ip") == "1");
        }
        public bool IsAssociateProject(int _id)
        {
            return (Get(_id, "associate_project") == "1");
        }
        public bool IsFullHeight(int _id)
        {
            return (Get(_id, "full_height") == "1");
        }
        public bool IsConfigServicePack(int _id)
        {
            return (Get(_id, "config_service_pack") == "1");
        }
        public bool IsConfigVMWareTemplate(int _id)
        {
            return (Get(_id, "config_vmware_template") == "1");
        }
        public bool IsConfigMaintenanceLevel(int _id)
        {
            return (Get(_id, "config_maintenance_level") == "1");
        }
        public bool IsVIO(int _id)
        {
            return (Get(_id, "vio") == "1");
        }
        public string GetFabric(int _id)
        {
            if (IsFabricCisco(_id))
                return "Cisco";
            else if (IsFabricBrocade(_id))
                return "Brocade";
            else if (IsFabricVMAX(_id))
                return "VMAX";
            else 
                return "???";
        }
        private bool IsFabricCisco(int _id)
        {
            return (Get(_id, "fabric") == "0");
        }
        private bool IsFabricBrocade(int _id)
        {
            return (Get(_id, "fabric") == "1");
        }
        private bool IsFabricVMAX(int _id)
        {
            return (Get(_id, "fabric") == "2");
        }
    }
}
