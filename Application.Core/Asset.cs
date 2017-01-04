using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;
using Tamir.SharpSsh;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace NCC.ClearView.Application.Core
{
    public enum AssetStatus
    {
        Returned = -100,
        Disposed = -10,
        Decommissioned = -1,
        Arrived = 0,
        InStock = 1,
        Available = 2,
        InUse = 10,
        Reserved = 100
    }

    public enum AssetAttribute
    {
        Ok = 0,
        Bad = 1,
        Moving = 2,
        Reserve = 3
    }

    public enum AssetPowerStatus
    {
        On = 1,
        Off = 2,
        Error = 3
    }

    public enum DellQueryType
    {
        Power = 1,
        MacAddress = 2,
        WWPN = 3
    }

    public enum SwitchPortType
    {
        ALL = 0,
        Network = 1,
        Storage = 2,
        Backup = 3,
        Cluster = 4,
        Remote = 5
    }

    public enum DellBladeSwitchportType
    {
        VLAN = 1,
        Mode = 2,
        Status = 3,
        VLANs = 4,
        Description = 5,
        Config = 6
    }

    public enum DellBladeSwitchportMode
    {
        Access = 1,
        Trunk = 2,
        Shutdown = 3
    }

    public class Asset
	{
		private string dsn = "";
        private string dsnCV = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public string strVirtualConnect = "";
        private string strResultsFile = "";
        private int intDellSwitchMaxLoops = 100;
        private int intDefaultSwitchPort = 23;
        private int intDefaultSwitchTimeout = 1000;

        public Asset(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}

        public Asset(int _user, string _dsn, string _dsnCV)
        {
            user = _user;
            dsn = _dsn;
            dsnCV = _dsnCV;
        }

        public string VirtualConnect()
        {
            return strVirtualConnect;
        }
        public string ResultsFile()
        {
            return strResultsFile;
        }
        public void AddStatus(int _assetid, string _name, int _status, int _userid, DateTime _datestamp)
        {
           if (_name == "")
           {
                
                // If name is blank and status is the same as before, make sure to copy the name to the new status or device name will be lost.
                DataSet dsStatus = GetStatus(_assetid);
                if (dsStatus.Tables[0].Rows.Count > 0)
                {
                    if (Int32.Parse(dsStatus.Tables[0].Rows[0]["status"].ToString()) == _status)
                        _name = dsStatus.Tables[0].Rows[0]["name"].ToString();
                }
            }
            int intAttribute = 0;
            Int32.TryParse(Get(_assetid, "asset_attribute"), out intAttribute);
            if (intAttribute == (int)AssetAttribute.Ok)
            {
                arParams = new SqlParameter[5];
                arParams[0] = new SqlParameter("@assetid", _assetid);
                arParams[1] = new SqlParameter("@name", _name);
                arParams[2] = new SqlParameter("@status", _status);
                arParams[3] = new SqlParameter("@userid", _userid);
                arParams[4] = new SqlParameter("@datestamp", _datestamp);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addStatus", arParams);
            }
        }
        public void UpdateStatus(int _assetid, string _name, int _status, int _userid, DateTime _datestamp)
        {
            int intAttribute = 0;
            Int32.TryParse(Get(_assetid, "asset_attribute"), out intAttribute);
            if (intAttribute == (int)AssetAttribute.Ok)
            {
                arParams = new SqlParameter[5];
                arParams[0] = new SqlParameter("@assetid", _assetid);
                arParams[1] = new SqlParameter("@name", _name);
                arParams[2] = new SqlParameter("@status", _status);
                arParams[3] = new SqlParameter("@userid", _userid);
                arParams[4] = new SqlParameter("@datestamp", _datestamp);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateStatus", arParams);
            }
        }
        public void UpdateStatus(int _assetid, string _name, int _userid, DateTime _datestamp)
        {
            int intAttribute = 0;
            Int32.TryParse(Get(_assetid, "asset_attribute"), out intAttribute);
            if (intAttribute == (int)AssetAttribute.Ok)
            {
                arParams = new SqlParameter[4];
                arParams[0] = new SqlParameter("@assetid", _assetid);
                arParams[1] = new SqlParameter("@name", _name);
                arParams[2] = new SqlParameter("@userid", _userid);
                arParams[3] = new SqlParameter("@datestamp", _datestamp);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateStatusName", arParams);
            }
        }
        public DataSet GetStatus(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStatus", arParams);
        }
        public string GetStatus(int _assetid, string _column)
        {
            DataSet ds = GetStatus(_assetid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public int Add(string _tracking, int _modelid, string _serial, string _asset, int _status, int _userid, DateTime _datestamp, int _assetAttribute, int _validated)
        {
            DataSet ds = Get(_serial, _modelid);
            if (ds.Tables[0].Rows.Count == 0)
            {
                Orders oOrder = new Orders(user, dsn);
                int intOrder = oOrder.Get(_tracking);
                arParams = new SqlParameter[7];
                arParams[0] = new SqlParameter("@orderid", intOrder);
                arParams[1] = new SqlParameter("@modelid", _modelid);
                arParams[2] = new SqlParameter("@serial", _serial);
                arParams[3] = new SqlParameter("@asset", _asset);
                arParams[4] = new SqlParameter("@AssetAttribute", _assetAttribute);
                arParams[5] = new SqlParameter("@validated", _validated);
                arParams[6] = new SqlParameter("@id", SqlDbType.Int);
                arParams[6].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAsset", arParams);
                int intAsset = Int32.Parse(arParams[6].Value.ToString());
                AddStatus(intAsset, "", _status, _userid, _datestamp);
                return intAsset;
            }
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString()); ;
        }
        //Asset Add - Overwrite with depot(Received Location)
        public int Add(int _orderid, int _depotid, int _modelid, string _serial, string _asset, int _status, int _userid, DateTime _datestamp, int _assetAttribute, int _validated, int _returned, string _comments)
        {
            DataSet ds = Get(_serial, _modelid);
            if (ds.Tables[0].Rows.Count == 0)
            {   arParams = new SqlParameter[10];
                arParams[0] = new SqlParameter("@orderid", _orderid);
                arParams[1] = new SqlParameter("@depotid", _depotid);
                arParams[2] = new SqlParameter("@modelid", _modelid);
                arParams[3] = new SqlParameter("@serial", _serial);
                arParams[4] = new SqlParameter("@asset", _asset);
                arParams[5] = new SqlParameter("@AssetAttribute", _assetAttribute);
                arParams[6] = new SqlParameter("@validated", _validated);
                arParams[7] = new SqlParameter("@returned", _returned);
                arParams[8] = new SqlParameter("@comments", _comments);
                arParams[9] = new SqlParameter("@id", SqlDbType.Int);
                arParams[9].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAsset", arParams);
                int intAsset = Int32.Parse(arParams[9].Value.ToString());
                AddStatus(intAsset, "", _status, _userid, _datestamp);
                return intAsset;
            }
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString()); ;
        }
        public void Update(int _assetid, string _asset)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@asset", _asset);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAssetAsset", arParams);
        }
        public void Update(int _assetid, string _serial, string _asset)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@serial", _serial);
            arParams[2] = new SqlParameter("@asset", _asset);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAssetSerial", arParams);
        }
        public void UpdateMac(int _assetid, string _macaddress)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@macaddress", _macaddress);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAssetMac", arParams);
        }
        public void Update(int _assetid, int _assetAttribute)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@AssetAttribute", _assetAttribute);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAssetAttribute", arParams);
        }
        public void Update(int _assetid, int _modelid, string _serial, string _asset, int _assetAttribute)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@modelid", _modelid);
            arParams[2] = new SqlParameter("@serial", _serial);
            arParams[3] = new SqlParameter("@asset", _asset);
            arParams[4] = new SqlParameter("@AssetAttribute", _assetAttribute);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAsset", arParams);
        }
        //Update Asset with new order id
        public void updateNewOrderId(int _newOrderId,int _assetId)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@assetid", _assetId);
            arParams[1] = new SqlParameter("@neworderid", _newOrderId);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAssetNewOrderId", arParams);
        }

        //Update Asset Status Based on Request
        public void UpdateAllAssetStatusForOrder(int _orderId, string _name, int _status, int _userId)
        {
            DataSet ds = GetAssetsByOrder(_orderId);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {   //1.Update Asset status : only if asset attribute is ok And
                //  There is change in status or Status Name
                string strName = _name;
                //2. For Enclosure or Rack => if the name is blank use the previos name
                ModelsProperties oModelsProperties = new ModelsProperties(_userId,dsnCV);

                int intAssetCategory = Int32.Parse(oModelsProperties.Get(Int32.Parse(dr["modelid"].ToString()), "asset_category"));
                if (intAssetCategory == 3 || intAssetCategory == 4)
                    if (strName == "")
                        strName = dr["name"].ToString();

                int intAttribute = 0;
                Int32.TryParse(dr["asset_attribute"].ToString(), out intAttribute);
                if (intAttribute == (int)AssetAttribute.Ok &&
                   (Int32.Parse(dr["status"].ToString()) != _status || dr["name"].ToString() != strName))
                    UpdateStatus(Int32.Parse(dr["id"].ToString()), strName, _status, _userId, DateTime.Now);
            }
        }

        //Update Asset Attribute Based on Request
        public void UpdateAllAssetAttributeForOrder(int _orderId, int _assetAttribute, int _userId)
        {
            DataSet ds = GetAssetsByOrder(_orderId);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Update(Int32.Parse(dr["id"].ToString()), _assetAttribute);
            }
        }

        
        //Update Asset Returned & Comments
        public void UpdateReturned(int _assetid, int _returned, string _comments)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@returned", _returned);
            arParams[2] = new SqlParameter("@comments", _comments);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAssetReturned", arParams);
        }

        public void UpdateModel(int _assetid, int _modelid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@modelid", _modelid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAssetModel", arParams);
        }
        public void UpdateValidated(int _assetid, int _validated)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@validated", _validated);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAssetValidated", arParams);
        }
        public int GetType(int _assetid, string _dsn)
        {
            int intType = 0;
            string strModel = Get(_assetid, "modelid");
            if (strModel != "")
            {
                ModelsProperties oModelsProperties = new ModelsProperties(0, _dsn);
                intType = oModelsProperties.GetType(Int32.Parse(strModel));
            }
            return intType;
        }
        public DataSet Get(string _serial, int _modelid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serial", _serial.Trim().ToUpper());
            arParams[1] = new SqlParameter("@modelid", _modelid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAssetDuplicate", arParams);
        }
        public DataSet Get(string _serial)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serial", _serial.Trim().ToUpper());
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAssetSerial", arParams);
        }
        public DataSet Gets(int _platformid, int _status)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            arParams[1] = new SqlParameter("@status", _status);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAssets", arParams);
        }
        //Get assets based on Order Id
        public DataSet GetAssetsByOrder(int _orderId)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@OrderId", _orderId);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAsset", arParams);
        }

        public DataSet Get(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAsset", arParams);
        }

        public string Get(int _assetid, string _column)
        {
            DataSet ds = Get(_assetid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetCount(int _modelid, int _status)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            arParams[1] = new SqlParameter("@status", _status);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAssetCount", arParams);
        }
        public void Delete(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAsset", arParams);
        }
        public DataSet GetAll(int _typeid, int _status)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@typeid", _typeid);
            arParams[1] = new SqlParameter("@status", _status);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAll", arParams);
        }

        // FACILITIES
        #region Facilities
        public DataSet GetDemandFacilities()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDemandFacilities");
        }
        #endregion

        // SEARCH
        #region Search
        public DataSet GetSearch(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSearch", arParams);
        }
        public int AddSearchName(int _userid, string _type, string _name, string _serial, string _asset)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@type", _type);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@serial", _serial);
            arParams[4] = new SqlParameter("@asset", _asset);
            arParams[5] = new SqlParameter("@id", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSearchName", arParams);
            return Int32.Parse(arParams[5].Value.ToString());
        }
        public int AddSearchClass(int _userid, string _type, int _platformid, int _classid, int _environmentid)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@type", _type);
            arParams[2] = new SqlParameter("@platformid", _platformid);
            arParams[3] = new SqlParameter("@classid", _classid);
            arParams[4] = new SqlParameter("@environmentid", _environmentid);
            arParams[5] = new SqlParameter("@id", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSearchClass", arParams);
            return Int32.Parse(arParams[5].Value.ToString());
        }
        public int AddSearchPlatform(int _userid, string _type, int _platformid, int _typeid, int _modelid)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@type", _type);
            arParams[2] = new SqlParameter("@platformid", _platformid);
            arParams[3] = new SqlParameter("@typeid", _typeid);
            arParams[4] = new SqlParameter("@modelid", _modelid);
            arParams[5] = new SqlParameter("@id", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSearchPlatform", arParams);
            return Int32.Parse(arParams[5].Value.ToString());
        }
        public int AddSearchDepot(int _userid, string _type, int _platformid, int _depotid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@type", _type);
            arParams[2] = new SqlParameter("@platformid", _platformid);
            arParams[3] = new SqlParameter("@depotid", _depotid);
            arParams[4] = new SqlParameter("@id", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSearchDepot", arParams);
            return Int32.Parse(arParams[4].Value.ToString());
        }
        public DataSet GetSearchResults(int _id)
        {
            DataSet ds = GetSearch(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataSet dsReturn = new DataSet();
                switch (ds.Tables[0].Rows[0]["type"].ToString())
                {
                    case "N":
                        arParams = new SqlParameter[3];
                        arParams[0] = new SqlParameter("@name", ds.Tables[0].Rows[0]["name"].ToString());
                        arParams[1] = new SqlParameter("@serial", ds.Tables[0].Rows[0]["serial"].ToString());
                        arParams[2] = new SqlParameter("@asset", ds.Tables[0].Rows[0]["asset"].ToString());
                        dsReturn = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSearchName", arParams);
                        break;
                    case "C":
                        arParams = new SqlParameter[3];
                        arParams[0] = new SqlParameter("@platformid", Int32.Parse(ds.Tables[0].Rows[0]["platformid"].ToString()));
                        arParams[1] = new SqlParameter("@classid", Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString()));
                        arParams[2] = new SqlParameter("@environmentid", Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString()));
                        dsReturn = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSearchClass", arParams);
                        break;
                    case "T":
                        arParams = new SqlParameter[3];
                        arParams[0] = new SqlParameter("@platformid", Int32.Parse(ds.Tables[0].Rows[0]["platformid"].ToString()));
                        arParams[1] = new SqlParameter("@typeid", Int32.Parse(ds.Tables[0].Rows[0]["typeid"].ToString()));
                        arParams[2] = new SqlParameter("@modelid", Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString()));
                        dsReturn = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSearchPlatform", arParams);
                        break;
                    case "D":
                        arParams = new SqlParameter[2];
                        arParams[0] = new SqlParameter("@platformid", Int32.Parse(ds.Tables[0].Rows[0]["platformid"].ToString()));
                        arParams[1] = new SqlParameter("@depotid", Int32.Parse(ds.Tables[0].Rows[0]["depotid"].ToString()));
                        dsReturn = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSearchDepot", arParams);
                        break;
                }
                return dsReturn;
            }
            else
                return new DataSet();
        }
        #endregion

        // SERVER
        #region Server

        public void AddServer(int _assetid, int _status, int _userid, DateTime _datestamp, int _classid, int _environmentid, int _rackid, string _rackposition, string _ilo, string _dummy_name, string _macaddress, int _vlan, int _build_network_id, int _resiliencyid, int _operatingsystemgroupid)
        {
            AddStatus(_assetid, "", _status, _userid, _datestamp);
            arParams = new SqlParameter[13];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@rackid", _rackid);
            arParams[4] = new SqlParameter("@rackposition", _rackposition);
            arParams[5] = new SqlParameter("@ilo", _ilo);
            arParams[6] = new SqlParameter("@dummy_name", _dummy_name);
            arParams[7] = new SqlParameter("@macaddress", _macaddress);
            arParams[8] = new SqlParameter("@vlan", _vlan);
            arParams[9] = new SqlParameter("@build_network_id", _build_network_id);
            arParams[10] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[11] = new SqlParameter("@operatingsystemgroupid", _operatingsystemgroupid);
            arParams[12] = new SqlParameter("@createdby", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServer", arParams);
        }


        //public void UpdateServer(int _assetid, string _name, int _status, int _userid, DateTime _datestamp, int _classid, int _environmentid, int _rackid, string _rackposition, string _ilo, string _dummy_name, string _macaddress, int _vlan, int _build_network_id, int _resiliencyid, int _operatingsystemgroupid)
        //{
        //    AddStatus(_assetid, _name, _status, _userid, _datestamp);
        //    arParams = new SqlParameter[13];
        //    arParams[0] = new SqlParameter("@assetid", _assetid);
        //    arParams[1] = new SqlParameter("@classid", _classid);
        //    arParams[2] = new SqlParameter("@environmentid", _environmentid);
        //    arParams[3] = new SqlParameter("@rackid", _rackid);
        //    arParams[4] = new SqlParameter("@rackposition", _rackposition);
        //    arParams[5] = new SqlParameter("@ilo", _ilo);
        //    arParams[6] = new SqlParameter("@dummy_name", _dummy_name);
        //    arParams[7] = new SqlParameter("@macaddress", _macaddress);
        //    arParams[8] = new SqlParameter("@vlan", _vlan);
        //    arParams[9] = new SqlParameter("@build_network_id", _build_network_id);
        //    arParams[10] = new SqlParameter("@resiliencyid", _resiliencyid);
        //    arParams[11] = new SqlParameter("@operatingsystemgroupid", _operatingsystemgroupid);
        //    arParams[12] = new SqlParameter("@modifiedby", _userid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServer", arParams);
        //}
        ////Update server info and status

        //public void UpdateServerInfoOnly(int _assetid, string _name, int _status, int _userid, DateTime _datestamp, int _classid, int _environmentid, int _rackid, string _rackposition, string _ilo, string _dummy_name, string _macaddress, int _vlan, int _build_network_id, int _resiliencyid, int _operatingsystemgroupid)
        //{
        //    //UpdateStatus(_assetid, _name, _status, _userid, _datestamp);
        //    arParams = new SqlParameter[13];
        //    arParams[0] = new SqlParameter("@assetid", _assetid);
        //    arParams[1] = new SqlParameter("@classid", _classid);
        //    arParams[2] = new SqlParameter("@environmentid", _environmentid);
        //    arParams[3] = new SqlParameter("@rackid", _rackid);
        //    arParams[4] = new SqlParameter("@rackposition", _rackposition);
        //    arParams[5] = new SqlParameter("@ilo", _ilo);
        //    arParams[6] = new SqlParameter("@dummy_name", _dummy_name);
        //    arParams[7] = new SqlParameter("@macaddress", _macaddress);
        //    arParams[8] = new SqlParameter("@vlan", _vlan);
        //    arParams[9] = new SqlParameter("@build_network_id", _build_network_id);
        //    arParams[10] = new SqlParameter("@resiliencyid", _resiliencyid);
        //    arParams[11] = new SqlParameter("@operatingsystemgroupid", _operatingsystemgroupid);
        //    arParams[12] = new SqlParameter("@modifiedby", _userid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServer", arParams);
        //}

        public void UpdateServer(int _assetid, string _name, int _status, int _userid, DateTime _datestamp, int _classid, int _environmentid, int _rackid, string _rackposition, string _ilo, string _dummy_name, string _macaddress, int _vlan, int _build_network_id, int _resiliencyid, int _operatingsystemgroupid)
        {
            AddStatus(_assetid, _name, _status, _userid, _datestamp);
            arParams = new SqlParameter[13];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@rackid", _rackid);
            arParams[4] = new SqlParameter("@rackposition", _rackposition);
            arParams[5] = new SqlParameter("@ilo", _ilo);
            arParams[6] = new SqlParameter("@dummy_name", _dummy_name);
            arParams[7] = new SqlParameter("@macaddress", _macaddress);
            arParams[8] = new SqlParameter("@vlan", _vlan);
            arParams[9] = new SqlParameter("@build_network_id", _build_network_id);
            arParams[10] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[11] = new SqlParameter("@operatingsystemgroupid", _operatingsystemgroupid);
            arParams[12] = new SqlParameter("@modifiedby", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServer", arParams);
        }
        //Update server info and status

        public void UpdateServerInfoOnly(int _assetid, string _name, int _status, int _userid, DateTime _datestamp, int _classid, int _environmentid, int _rackid, string _rackposition, string _ilo, string _dummy_name, string _macaddress, int _vlan, int _build_network_id, int _resiliencyid, int _operatingsystemgroupid)
        {
            //UpdateStatus(_assetid, _name, _status, _userid, _datestamp);
            arParams = new SqlParameter[13];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@rackid", _rackid);
            arParams[4] = new SqlParameter("@rackposition", _rackposition);
            arParams[5] = new SqlParameter("@ilo", _ilo);
            arParams[6] = new SqlParameter("@dummy_name", _dummy_name);
            arParams[7] = new SqlParameter("@macaddress", _macaddress);
            arParams[8] = new SqlParameter("@vlan", _vlan);
            arParams[9] = new SqlParameter("@build_network_id", _build_network_id);
            arParams[10] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[11] = new SqlParameter("@operatingsystemgroupid", _operatingsystemgroupid);
            arParams[12] = new SqlParameter("@modifiedby", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServer", arParams);
        }


        public void DeleteServer(int _assetid, bool _redeploy, int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServer", arParams);
            if (_redeploy == true)
                AddStatus(_assetid, GetStatus(_assetid, "name"), (int)AssetStatus.Arrived, _userid, DateTime.Now);
        }
        public DataSet GetServerOrBlades(int _modelid, int _status)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            arParams[1] = new SqlParameter("@status", _status);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerOrBlades", arParams);
        }
        public DataSet GetServerOrBlade(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerOrBlade", arParams);
        }
        public string GetServerOrBlade(int _assetid, string _column)
        {
            DataSet ds = GetServerOrBlade(_assetid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public int GetServerOrBladeAvailableLocation(int _classid, int _environmentid, int _modelid, int _prod, int _qa, int _test_dev)
        {
            int intLocation = 0;
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@modelid", _modelid);
            arParams[3] = new SqlParameter("@prod", _prod);
            arParams[4] = new SqlParameter("@qa", _qa);
            arParams[5] = new SqlParameter("@test", _test_dev);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerOrBladeAvailableLocation", arParams);
            if (ds.Tables[0].Rows.Count > 0)
                Int32.TryParse(ds.Tables[0].Rows[0]["addressid"].ToString(), out intLocation);
            return intLocation;
        }
        public DataSet GetServerOrBladeAvailable(int _classid, int _environmentid, int _addressid, int _modelid, int _projectid, int _resiliencyid, int _osid)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@modelid", _modelid);
            arParams[4] = new SqlParameter("@projectid", _projectid);
            arParams[5] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[6] = new SqlParameter("@osid", _osid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerOrBladeAvailable", arParams);
        }
        public List<int> GetServerOrBladeAvailable(int _classid, int _environmentid, int _addressid, int _modelid, int _answerid, string _dsn, string _ha, bool _build, int _projectid, int _resiliencyid, int _osid, string _name, bool _getDR, bool _dell, string _dsn_service_editor)
        {
            Forecast oForecast = new Forecast(user, dsnCV);
            Projects oProject = new Projects(user, dsnCV);
            Locations oLocation = new Locations(user, dsnCV);
            Resiliency oResiliency = new Resiliency(user, dsnCV);
            Classes oClass = new Classes(user, dsnCV);
            Log oLog = new Log(user, dsnCV);
            bool boolHA = true;
            List<int> lstAsset = new List<int>();
            int intAsset = 0;
            int intAssetHA = 0;
            int intAssetDR = 0;
            string _ha_text = _ha;
            if (oForecast.IsHANone(_answerid) == true)
                boolHA = false;
            else
            {
                while (_ha_text != "")
                {
                    int intQuestion = Int32.Parse(_ha_text.Substring(0, _ha_text.IndexOf("_")));
                    _ha_text = _ha_text.Substring(_ha_text.IndexOf("_") + 1);
                    int intResponse = Int32.Parse(_ha_text.Substring(0, _ha_text.IndexOf(";")));
                    _ha_text = _ha_text.Substring(_ha_text.IndexOf(";") + 1);
                    if (oForecast.GetAnswerPlatform(_answerid, intQuestion, intResponse) == true)
                    {
                        boolHA = false;
                        break;
                    }
                }
            }
            
            // Select the asset.
            // If there are multiple locations configured for the Resiliency, check the ADDRESSID of the project, then loop through the locations.
            // Else, use the location passed into this function.
            // Production locations come from the Resiliency Locations.
            // QA, Test and DEV come from the Location flags.
            List<List<int>> lstLocations = new List<List<int>>();
            DataSet dsLocations = oResiliency.GetLocations(_resiliencyid);
            oLog.AddEvent(_answerid, _name, "", "ResiliencyID = " + _resiliencyid.ToString(), LoggingType.Debug);
            int intProjectAddress = -1;
            int intDesign = 0;
            int intDesignAddress = 0;
            Design oDesign = new Design(0, dsnCV);
            DataSet dsDesign = oDesign.GetAnswer(_answerid);
            if (dsDesign.Tables[0].Rows.Count > 0)
            {
                Int32.TryParse(dsDesign.Tables[0].Rows[0]["id"].ToString(), out intDesign);
                Int32.TryParse(dsDesign.Tables[0].Rows[0]["addressid"].ToString(), out intDesignAddress);
            }
            int intAddressCounter = 0;
            //int intBIR = 0;
            bool boolProd = oClass.IsProd(_classid);
            bool boolQA = oClass.IsQA(_classid);
            if (dsLocations.Tables[0].Rows.Count == 0)
            {
                oLog.AddEvent(_answerid, _name, "", "No Resiliency Locations => Old Data Center Strategy (based on addressid = " + _addressid.ToString() + ")", LoggingType.Debug);
                if (_addressid == 0)
                {
                    oLog.AddEvent(_answerid, _name, "", "Need to determine location based on requirements", LoggingType.Debug);
                }
                // There are no locations, which means it is the OLD data center strategy (under 48 hours and no BIR).
                lstLocations.Add(new List<int>());
                lstLocations[0].Add(_addressid);   // Use the location passed into the function.
                if (boolProd && _addressid == 715)
                    lstLocations[0].Add(696);   // Dalton for Cleveland
                else
                    lstLocations[0].Add(0);     // No DR for anywhere other than Cleveland
                intAddressCounter++;
            }
            else 
            {
                // New Data Center Stragey
                oLog.AddEvent(_answerid, _name, "", dsLocations.Tables[0].Rows.Count.ToString() + " Resiliency Locations => New Data Center Strategy", LoggingType.Debug);
                //intBIR = 1;
                if (boolProd == true || boolQA == true)
                {
                    // Check Project for Location
                    if ((Int32.TryParse(oProject.Get(_projectid, "addressid"), out intProjectAddress) == true && intProjectAddress > 0) ||
                        intDesignAddress > 0)
                    {
                        if (intDesignAddress > 0)
                            oLog.AddEvent(_answerid, _name, "", "*** OVERRIDE ADDRESS FOUND *** = " + intDesignAddress.ToString(), LoggingType.Debug); 
                        else if (intProjectAddress > 0)
                        {
                            //Int32.TryParse(oProject.Get(_projectid, "bir"), out intBIR);
                            //oLog.AddEvent(_answerid, _name, "", "*** PROJECT LOCATION FOUND *** = " + intProjectAddress.ToString() + ", BIR = " + intBIR.ToString(), LoggingType.Debug);
                            oLog.AddEvent(_answerid, _name, "", "*** PROJECT LOCATION FOUND *** = " + intProjectAddress.ToString(), LoggingType.Debug);
                            intDesignAddress = intProjectAddress;
                        }
                        lstLocations.Add(new List<int>());
                        lstLocations[intAddressCounter].Add(intDesignAddress);     // Use the location in which the project has already been built
                        int intDesignAddressDR = 0;
                        //intDesignAddressDR = oResiliency.GetLocationDR(intDesignAddress, intBIR);
                        // Try to match from existing Locations
                        foreach (DataRow drLocation in dsLocations.Tables[0].Rows)
                        {
                            if (Int32.Parse(drLocation["prodID"].ToString()) == intDesignAddress) 
                            {
                                intDesignAddressDR = Int32.Parse(drLocation["drID"].ToString());
                                break;
                            }
                        }
                        oLog.AddEvent(_answerid, _name, "", "DR ADDRESS = " + intDesignAddressDR.ToString(), LoggingType.Debug);
                        lstLocations[intAddressCounter].Add(intDesignAddressDR);   // Hardcoded DR counterpart
                        intAddressCounter++;
                    }
                    else
                    {
                        oLog.AddEvent(_answerid, _name, "", "Project Address Not Found...", LoggingType.Debug);
                        // Project has not been designated.
                        if (boolProd == true)    // Production
                        {
                            // From resiliency locations table
                            foreach (DataRow drLocation in dsLocations.Tables[0].Rows)
                            {
                                lstLocations.Add(new List<int>());
                                lstLocations[intAddressCounter].Add(Int32.Parse(drLocation["prodID"].ToString()));
                                lstLocations[intAddressCounter].Add(Int32.Parse(drLocation["drID"].ToString()));
                                intAddressCounter++;
                            }
                        }
                        else    // QA
                        {
                            // From locations table (for QA)
                            dsLocations = oLocation.GetAddressClass(0, 0, 1, 0);
                            foreach (DataRow drLocation in dsLocations.Tables[0].Rows)
                            {
                                lstLocations.Add(new List<int>());
                                lstLocations[intAddressCounter].Add(Int32.Parse(drLocation["id"].ToString()));
                                lstLocations[intAddressCounter].Add(0);   // No DR for QA
                                intAddressCounter++;
                            }
                        }
                    }
                }
                else  // Must be Test or Dev 
                {
                    if (intDesignAddress > 0)
                    {
                        oLog.AddEvent(_answerid, _name, "", "*** OVERRIDE ADDRESS FOUND *** = " + intDesignAddress.ToString(), LoggingType.Debug);
                        lstLocations.Add(new List<int>());
                        lstLocations[intAddressCounter].Add(intDesignAddress);
                        lstLocations[intAddressCounter].Add(0);   // No DR for Test/Dev
                        intAddressCounter++;
                    }
                    else
                    {
                        // From locations table (for Test)
                        dsLocations = oLocation.GetAddressClass(0, 0, 0, 1);
                        foreach (DataRow drLocation in dsLocations.Tables[0].Rows)
                        {
                            lstLocations.Add(new List<int>());
                            lstLocations[intAddressCounter].Add(Int32.Parse(drLocation["id"].ToString()));
                            lstLocations[intAddressCounter].Add(0);   // No DR for Test/Dev
                            intAddressCounter++;
                        }
                    }
                }
            }


            int intSelectedAsset = 0;
            int intSelectedAssetDR = 0;
            for (int ii = 0; ii < intAddressCounter; ii++)
            {
                if (intSelectedAsset > 0)
                    break;
                oLog.AddEvent(_answerid, _name, "", "Trying location = " + lstLocations[ii][0].ToString() + " (" + oLocation.GetFull(lstLocations[ii][0]) + "), HA = " + (boolHA ? "Yes" : "No"), LoggingType.Debug);
                DataSet dsAssets = GetServerOrBladeAvailable(_classid, _environmentid, lstLocations[ii][0], _modelid, _projectid, _resiliencyid, _osid);
                // Remove Manual Assets
                ServiceEditor oServiceEditor = new ServiceEditor(0, _dsn_service_editor);
                DataSet dsAssetsManual = oServiceEditor.APGetSerials();
                for (int aa = 0; aa < dsAssets.Tables[0].Rows.Count; aa++)
                {
                    DataRow drAsset = dsAssets.Tables[0].Rows[aa];
                    string strSerial = drAsset["serial"].ToString();
                    DataView dvAssetsManual = dsAssetsManual.Tables[0].DefaultView;
                    dvAssetsManual.RowFilter = "serial = '" + strSerial + "'";
                    if (dvAssetsManual.Count > 0)
                    {
                        oLog.AddEvent(_answerid, _name, strSerial, "Excluding asset since it was used during manual provisioning", LoggingType.Information);
                        dsAssets.Tables[0].Rows.Remove(drAsset);
                        aa--;
                    }
                }
                oLog.AddEvent(_answerid, _name, "", "GetServerOrBladeAvailable(" + _classid.ToString() + "," + _environmentid.ToString() + "," + lstLocations[ii][0].ToString() + "," + _modelid.ToString() + "," + _projectid.ToString() + "," + _resiliencyid.ToString() + "," + _osid.ToString() + ") = " + dsAssets.Tables[0].Rows.Count.ToString() + " Assets", LoggingType.Debug);
                if (boolHA == true)
                {
                    foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                    {
                        if (CheckServerOrBladeAvailable(Int32.Parse(drAsset["id"].ToString()), _answerid, true, _dsn) == false)
                        {
                            if (_getDR && boolProd && lstLocations[ii][1] > 0)
                            {
                                int intTemp = Int32.Parse(drAsset["id"].ToString());
                                oLog.AddEvent(_answerid, _name, "", "Asset Found (" + intTemp.ToString() + ")...looking at DR location (" + oLocation.GetFull(lstLocations[ii][1]) + ")", LoggingType.Debug);
                                DataSet dsDR = GetServerOrBladeAvailableDR(_environmentid, _modelid, intTemp, _dell, lstLocations[ii][1]);
                                if (dsDR.Tables[0].Rows.Count > 0)
                                {
                                    intAssetDR = Int32.Parse(dsDR.Tables[0].Rows[0]["id"].ToString());
                                    oLog.AddEvent(_answerid, _name, "", "DR Asset Found (" + intAssetDR.ToString() + ")", LoggingType.Debug);
                                    intAsset = intTemp;
                                    break;
                                }
                                else
                                    oLog.AddEvent(_answerid, _name, "", "No Assets found at DR location ~ GetServerOrBladeAvailableDR(" + _environmentid.ToString() + "," + _modelid.ToString() + "," + intTemp.ToString() + "," + _dell.ToString() + "," + lstLocations[ii][1].ToString() + ")", LoggingType.Debug);
                            }
                            else
                            {
                                intAssetDR = -1;
                                intAsset = Int32.Parse(drAsset["id"].ToString());
                                oLog.AddEvent(_answerid, _name, "", "Asset Found (" + intAsset.ToString() + ") #1...DR not required (" + lstLocations[ii][1].ToString() + ")", LoggingType.Debug);
                                break;
                            }
                        }
                        else if (intAssetHA == 0 && CheckServerOrBladeDifferent(_answerid, false, _dsn) == true)
                        {
                            if (_getDR && boolProd && lstLocations[ii][1] > 0)
                            {
                                int intTemp = Int32.Parse(drAsset["id"].ToString());
                                oLog.AddEvent(_answerid, _name, "", "Asset Found (" + intTemp.ToString() + ")...looking at DR location (" + oLocation.GetFull(lstLocations[ii][1]) + ")", LoggingType.Debug);
                                DataSet dsDR = GetServerOrBladeAvailableDR(_environmentid, _modelid, intTemp, _dell, lstLocations[ii][1]);
                                if (dsDR.Tables[0].Rows.Count > 0)
                                {
                                    intAssetDR = Int32.Parse(dsDR.Tables[0].Rows[0]["id"].ToString());
                                    oLog.AddEvent(_answerid, _name, "", "DR Asset Found (" + intAssetDR.ToString() + ")", LoggingType.Debug);
                                    intAssetHA = intTemp;
                                }
                                else
                                    oLog.AddEvent(_answerid, _name, "", "No Assets found at DR location ~ GetServerOrBladeAvailableDR(" + _environmentid.ToString() + "," + _modelid.ToString() + "," + intTemp.ToString() + "," + _dell.ToString() + "," + lstLocations[ii][1].ToString() + ")", LoggingType.Debug);
                            }
                            else
                            {
                                intAssetDR = -1;
                                intAssetHA = Int32.Parse(drAsset["id"].ToString());
                                oLog.AddEvent(_answerid, _name, "", "Asset Found (" + intAsset.ToString() + ") #2...DR not required (" + lstLocations[ii][1].ToString() + ")", LoggingType.Debug);
                            }
                        }
                    }
                    if (intAsset == 0)
                    {
                        foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                        {
                            if (CheckServerOrBladeAvailable(Int32.Parse(drAsset["id"].ToString()), _answerid, false, _dsn) == false)
                            {
                                if (_getDR && boolProd && lstLocations[ii][1] > 0)
                                {
                                    int intTemp = Int32.Parse(drAsset["id"].ToString());
                                    oLog.AddEvent(_answerid, _name, "", "Asset Found (" + intTemp.ToString() + ")...looking at DR location (" + oLocation.GetFull(lstLocations[ii][1]) + ")", LoggingType.Debug);
                                    DataSet dsDR = GetServerOrBladeAvailableDR(_environmentid, _modelid, intTemp, _dell, lstLocations[ii][1]);
                                    if (dsDR.Tables[0].Rows.Count > 0)
                                    {
                                        intAssetDR = Int32.Parse(dsDR.Tables[0].Rows[0]["id"].ToString());
                                        oLog.AddEvent(_answerid, _name, "", "DR Asset Found (" + intAssetDR.ToString() + ")", LoggingType.Debug);
                                        intAsset = intTemp;
                                        break;
                                    }
                                    else
                                        oLog.AddEvent(_answerid, _name, "", "No Assets found at DR location ~ GetServerOrBladeAvailableDR(" + _environmentid.ToString() + "," + _modelid.ToString() + "," + intTemp.ToString() + "," + _dell.ToString() + "," + lstLocations[ii][1].ToString() + ")", LoggingType.Debug);
                                }
                                else
                                {
                                    intAssetDR = -1;
                                    intAsset = Int32.Parse(drAsset["id"].ToString());
                                    oLog.AddEvent(_answerid, _name, "", "Asset Found (" + intAsset.ToString() + ") #3...DR not required (" + lstLocations[ii][1].ToString() + ")", LoggingType.Debug);
                                    break;
                                }
                            }
                            else if (intAssetHA == 0 && CheckServerOrBladeDifferent(_answerid, false, _dsn) == true)
                            {
                                if (_getDR && boolProd && lstLocations[ii][1] > 0)
                                {
                                    int intTemp = Int32.Parse(drAsset["id"].ToString());
                                    oLog.AddEvent(_answerid, _name, "", "Asset Found (" + intTemp.ToString() + ")...looking at DR location (" + oLocation.GetFull(lstLocations[ii][1]) + ")", LoggingType.Debug);
                                    DataSet dsDR = GetServerOrBladeAvailableDR(_environmentid, _modelid, intTemp, _dell, lstLocations[ii][1]);
                                    if (dsDR.Tables[0].Rows.Count > 0)
                                    {
                                        intAssetDR = Int32.Parse(dsDR.Tables[0].Rows[0]["id"].ToString());
                                        oLog.AddEvent(_answerid, _name, "", "DR Asset Found (" + intAssetDR.ToString() + ")", LoggingType.Debug);
                                        intAssetHA = intTemp;
                                    }
                                    else
                                        oLog.AddEvent(_answerid, _name, "", "No Assets found at DR location ~ GetServerOrBladeAvailableDR(" + _environmentid.ToString() + "," + _modelid.ToString() + "," + intTemp.ToString() + "," + _dell.ToString() + "," + lstLocations[ii][1].ToString() + ")", LoggingType.Debug);
                                }
                                else
                                {
                                    intAssetDR = -1;
                                    intAssetHA = Int32.Parse(drAsset["id"].ToString());
                                    oLog.AddEvent(_answerid, _name, "", "Asset Found (" + intAsset.ToString() + ") #4...DR not required (" + lstLocations[ii][1].ToString() + ")", LoggingType.Debug);
                                }
                            }
                        }
                    }
                }
                // The following line would permit HA Designs (cluster / csm / F5 / etc...) to have more than one blade assigned in the same enclosure.
                //if (boolHA == false || (_build == true && intAsset == 0))
                // The following line does NOT permit HA Designs (cluster / csm / F5 / etc...) to have more than one blade assigned in the same enclosure.
                if (boolHA == false)
                {
                    if (_getDR && boolProd && lstLocations[ii][1] > 0)
                    {
                        foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                        {
                            int intTemp = Int32.Parse(drAsset["id"].ToString());
                            oLog.AddEvent(_answerid, _name, "", "Asset Found (" + intTemp.ToString() + ")...looking at DR location (" + oLocation.GetFull(lstLocations[ii][1]) + ")", LoggingType.Debug);
                            DataSet dsDR = GetServerOrBladeAvailableDR(_environmentid, _modelid, intTemp, _dell, lstLocations[ii][1]);
                            if (dsDR.Tables[0].Rows.Count > 0)
                            {
                                intAssetDR = Int32.Parse(dsDR.Tables[0].Rows[0]["id"].ToString());
                                oLog.AddEvent(_answerid, _name, "", "DR Asset Found (" + intAssetDR.ToString() + ")", LoggingType.Debug);
                                intAsset = intTemp;
                                break;
                            }
                            else
                                oLog.AddEvent(_answerid, _name, "", "No Assets found at DR location ~ GetServerOrBladeAvailableDR(" + _environmentid.ToString() + "," + _modelid.ToString() + "," + intTemp.ToString() + "," + _dell.ToString() + "," + lstLocations[ii][1].ToString() + ")", LoggingType.Debug);
                        }
                    }
                    else if (dsAssets.Tables[0].Rows.Count > 0)
                    {
                        intAssetDR = -1;
                        intAsset = Int32.Parse(dsAssets.Tables[0].Rows[0]["id"].ToString());
                        oLog.AddEvent(_answerid, _name, "", "Asset Found (" + intAsset.ToString() + ") #5...DR not required (" + lstLocations[ii][1].ToString() + ")", LoggingType.Debug);
                    }
                }
                if (intAsset == 0 && boolHA == true && intAssetHA > 0)
                    intSelectedAsset = intAssetHA;
                else
                    intSelectedAsset = intAsset;
                // Update DR asset
                intSelectedAssetDR = intAssetDR;
            }
            if (intSelectedAsset > 0 && intProjectAddress == 0 && (boolProd == true || boolQA == true))
            {
                // intProjectAddress will only be 0 if it's QA or TEST and the projectID is not set (otherwise it would be the projectID or -1)
                Int32.TryParse(GetServerOrBlade(intSelectedAsset, "LocationId"), out intProjectAddress);
                //oProject.UpdateLocation(_projectid, intProjectAddress, intBIR);
                oProject.UpdateLocation(_projectid, intProjectAddress, 1);
                oLog.AddEvent(_answerid, _name, "", "Project (" + _projectid.ToString() + ") updated with asset location " + intProjectAddress.ToString() + " (" + oLocation.GetFull(intProjectAddress) + ")", LoggingType.Debug);
            }
            if (intSelectedAsset > 0 && _addressid == 0)
            {
                Int32.TryParse(GetServerOrBlade(intSelectedAsset, "LocationId"), out intProjectAddress);
                oForecast.UpdateAnswerLocation(_answerid, intProjectAddress);
                oLog.AddEvent(_answerid, _name, "", "Design (" + _answerid.ToString() + ") updated with asset location " + intProjectAddress.ToString() + " (" + oLocation.GetFull(intProjectAddress) + ")", LoggingType.Debug);
            }
            lstAsset.Add(intSelectedAsset);
            lstAsset.Add(intSelectedAssetDR);
            return lstAsset;
        }
        public bool CheckServerOrBladeAvailable(int _new, int _answerid, bool _rack, string _dsn)
        {
            Servers oServer = new Servers(0, _dsn);
            DataSet dsServer = oServer.GetAnswer(_answerid);
            bool boolSame = false;
            foreach (DataRow drServer in dsServer.Tables[0].Rows)
            {
                if (boolSame == true)
                    break;
                int intServer = Int32.Parse(drServer["id"].ToString());
                DataSet dsAsset = oServer.GetAssets(intServer);
                foreach (DataRow drAsset in dsAsset.Tables[0].Rows)
                {
                    if (drAsset["latest"].ToString() == "1")
                    {
                        int intAsset = Int32.Parse(drAsset["assetid"].ToString());
                        if (_rack == true)
                        {
                            if (GetServerOrBlade(_new, "rack") == GetServerOrBlade(intAsset, "rack"))
                            {
                                boolSame = true;
                                break;
                            }
                        }
                        else
                        {
                            if (GetServerOrBlade(_new, "enclosureid") != "0" && (GetServerOrBlade(_new, "enclosureid") == GetServerOrBlade(intAsset, "enclosureid")))
                            {
                                boolSame = true;
                                break;
                            }
                        }
                    }

                }
            }
            return boolSame;
        }
        public bool CheckServerOrBladeDifferent(int _answerid, bool _rack, string _dsn)
        {
            Servers oServer = new Servers(0, _dsn);
            DataSet dsServer = oServer.GetAnswer(_answerid);
            bool boolDifferent = false;
            int intAssetOLD = 0;
            foreach (DataRow drServer in dsServer.Tables[0].Rows)
            {
                if (boolDifferent == true)
                    break;
                int intServer = Int32.Parse(drServer["id"].ToString());
                DataSet dsAsset = oServer.GetAssets(intServer);
                foreach (DataRow drAsset in dsAsset.Tables[0].Rows)
                {
                    if (drAsset["latest"].ToString() == "1")
                    {
                        int intAsset = Int32.Parse(drAsset["assetid"].ToString());
                        if (intAssetOLD > 0)
                        {
                            if (_rack == true)
                            {
                                if (GetServerOrBlade(intAssetOLD, "rack") != GetServerOrBlade(intAsset, "rack"))
                                {
                                    boolDifferent = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (GetServerOrBlade(intAssetOLD, "enclosureid") == "0" || (GetServerOrBlade(intAssetOLD, "enclosureid") != GetServerOrBlade(intAsset, "enclosureid")))
                                {
                                    boolDifferent = true;
                                    break;
                                }
                            }
                        }
                        intAssetOLD = intAsset;
                    }
                }
            }
            return boolDifferent;
        }
        public int GetDR(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getDR", arParams);
            if (o == null)
                return 0;
            else 
                return Int32.Parse(o.ToString());
        }
        public DataSet GetServerOrBladeAvailableDR(int _environmentid, int _modelid, int _blade_assetid, bool _dell, int? _addressid)
        {
            int intDR = 0;
            int intBay = 0;
            if (_blade_assetid > 0 && _dell == false)   // Match slot for slot DR.  Only for HPs (not DELLs)
            {
                int intEnclosure = Int32.Parse(GetServerOrBlade(_blade_assetid, "enclosureid"));
                intDR = GetEnclosureDR(intEnclosure);
                intBay = Int32.Parse(GetServerOrBlade(_blade_assetid, "slot"));
            }
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@environmentid", _environmentid);
            arParams[1] = new SqlParameter("@modelid", _modelid);
            arParams[2] = new SqlParameter("@enclosureid", intDR);
            arParams[3] = new SqlParameter("@slot", intBay);
            if (_addressid != null)
                arParams[4] = new SqlParameter("@addressid", _addressid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerOrBladeAvailableDR", arParams);
        }
        #endregion

        // NETWORK
        #region NETWORK
        public void AddNetwork(int _assetid, string _name, int _status, int _userid, DateTime _datestamp, int _depotid, int _depotroomid, int _shelfid, int _available_ports, int _classid, int _environmentid, int _addressid, int _roomid, int _rackid, string _rackposition)
        {
            AddStatus(_assetid, _name, _status, _userid, _datestamp);
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@depotid", _depotid);
            arParams[2] = new SqlParameter("@depotroomid", _depotroomid);
            arParams[3] = new SqlParameter("@shelfid", _shelfid);
            arParams[4] = new SqlParameter("@available_ports", _available_ports);
            arParams[5] = new SqlParameter("@classid", _classid);
            arParams[6] = new SqlParameter("@environmentid", _environmentid);
            arParams[7] = new SqlParameter("@addressid", _addressid);
            arParams[8] = new SqlParameter("@roomid", _roomid);
            arParams[9] = new SqlParameter("@rackid", _rackid);
            arParams[10] = new SqlParameter("@rackposition", _rackposition);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addNetwork", arParams);
        }
        public void UpdateNetwork(int _assetid, string _name, int _status, int _userid, DateTime _datestamp, int _depotid, int _depotroomid, int _shelfid, int _available_ports, int _classid, int _environmentid, int _addressid, int _roomid, int _rackid, string _rackposition)
        {
            AddStatus(_assetid, _name, _status, _userid, _datestamp);
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@depotid", _depotid);
            arParams[2] = new SqlParameter("@depotroomid", _depotroomid);
            arParams[3] = new SqlParameter("@shelfid", _shelfid);
            arParams[4] = new SqlParameter("@available_ports", _available_ports);
            arParams[5] = new SqlParameter("@classid", _classid);
            arParams[6] = new SqlParameter("@environmentid", _environmentid);
            arParams[7] = new SqlParameter("@addressid", _addressid);
            arParams[8] = new SqlParameter("@roomid", _roomid);
            arParams[9] = new SqlParameter("@rackid", _rackid);
            arParams[10] = new SqlParameter("@rackposition", _rackposition);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateNetwork", arParams);
        }
        public DataSet GetNetwork(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getNetwork", arParams);
        }
        public string GetNetwork(int _assetid, string _column)
        {
            DataSet ds = GetNetwork(_assetid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetSupplyNetwork()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSupplyNetwork");
        }
        #endregion

        // VIRTUAL / VMWARE SHARED
        #region VIRTUAL / VMWARE SHARED
        public int AddGuest(string _name, int _modelid, string _serial, string _asset, int _status, int _userid, DateTime _datestamp, int _hostid, double _ram, double _processors, double _storage, int _classid, int _environmentid, int _addressid, int _classid_move, int _environmentid_move)
        {
            int intAsset = Add(_name, _modelid, _serial, _asset, _status, _userid, _datestamp, 0, 1);
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@assetid", intAsset);
            arParams[1] = new SqlParameter("@hostid", _hostid);
            arParams[2] = new SqlParameter("@ram", _ram);
            arParams[3] = new SqlParameter("@processors", _processors);
            arParams[4] = new SqlParameter("@storage", _storage);
            arParams[5] = new SqlParameter("@classid", _classid);
            arParams[6] = new SqlParameter("@environmentid", _environmentid);
            arParams[7] = new SqlParameter("@addressid", _addressid);
            arParams[8] = new SqlParameter("@classid_move", _classid_move);
            arParams[9] = new SqlParameter("@environmentid_move", _environmentid_move);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addGuest", arParams);
            return intAsset;
        }
        public void UpdateGuest(int _assetid, string _name, int _status, int _userid, DateTime _datestamp, int _classid, int _environmentid)
        {
            AddStatus(_assetid, _name, _status, _userid, _datestamp);
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateGuest", arParams);
        }
        public DataSet GetGuest(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getGuest", arParams);
        }
        public string GetGuest(int _assetid, string _column)
        {
            DataSet ds = GetGuest(_assetid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public string GetGuestSerial(int _modelid)
        {
            // Format: 9340-9625-6908-1956-1571-9480-64
            Functions oFunction = new Functions(0, dsn, 0);
            string strSerial = "";
            DataSet ds = Get(strSerial, _modelid);
            while (strSerial == "" || ds.Tables[0].Rows.Count > 0)
            {
                int int1 = oFunction.RandomNumber(1000, 9999);
                int int2 = oFunction.RandomNumber(1000, 9999);
                while (int1 == int2)
                    int2 = oFunction.RandomNumber(1000, 9999);
                int int3 = oFunction.RandomNumber(1000, 9999);
                while (int2 == int3)
                    int3 = oFunction.RandomNumber(1000, 9999);
                int int4 = oFunction.RandomNumber(1000, 9999);
                while (int3 == int4)
                    int4 = oFunction.RandomNumber(1000, 9999);
                int int5 = oFunction.RandomNumber(1000, 9999);
                while (int4 == int5)
                    int5 = oFunction.RandomNumber(1000, 9999);
                int int6 = oFunction.RandomNumber(1000, 9999);
                while (int5 == int6)
                    int6 = oFunction.RandomNumber(1000, 9999);
                int int7 = oFunction.RandomNumber(10, 99);
                strSerial = int1.ToString() + "-" + int2.ToString() + "-" + int3.ToString() + "-" + int4.ToString() + "-" + int5.ToString() + "-" + int6.ToString() + "-" + int7.ToString();
            }
            return strSerial;
        }
        public DataSet DeleteGuest(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_deleteGuest", arParams);
        }

        public DataSet GetHostLocations(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getHostLocations", arParams);
        }
        public void DeleteHostLocations(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteHostLocations", arParams);
        }
        public void AddHostLocation(int _assetid, int _classid, int _environmentid, int _addressid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@addressid", _addressid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addHostLocation", arParams);
        }
        #endregion

        // VIRTUAL
        #region VIRTUAL
        public void AddVirtualHost(int _assetid, int _hostid, int _guests, double _processors)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@hostid", _hostid);
            arParams[2] = new SqlParameter("@guests", _guests);
            arParams[3] = new SqlParameter("@processors", _processors);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addHostVirtual", arParams);
        }
        public DataSet GetVirtualHosts(int _environment, int _osid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@environment", _environment);
            arParams[1] = new SqlParameter("@osid", _osid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getHostVirtuals", arParams);
        }
        public DataSet GetVirtualHost(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getHostVirtual", arParams);
        }
        public string GetVirtualHost(int _assetid, string _column)
        {
            DataSet ds = GetVirtualHost(_assetid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        public void AddVirtualHostOs(int _assetid, int _osid, string _virtualdir, string _gzippath, string _image)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@osid", _osid);
            arParams[2] = new SqlParameter("@virtualdir", _virtualdir);
            arParams[3] = new SqlParameter("@gzippath", _gzippath);
            arParams[4] = new SqlParameter("@image", _image);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addHostVirtualOs", arParams);
        }
        public DataSet GetVirtualHostOs(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getHostVirtualOs", arParams);
        }
        public void DeleteVirtualHostOs(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteHostVirtualOs", arParams);
        }

        public void AddVirtualHostEnvironment(int _assetid, int _environment)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@environment", _environment);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addHostVirtualEnvironment", arParams);
        }
        public DataSet GetVirtualHostEnvironment(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getHostVirtualEnvironment", arParams);
        }
        public void DeleteVirtualHostEnvironment(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteHostVirtualEnvironment", arParams);
        }
        #endregion

        // VMWARE
        #region VMWARE
        public void AddVMWareHost(int _assetid, int _hostid, int _guests, double _processors)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@hostid", _hostid);
            arParams[2] = new SqlParameter("@guests", _guests);
            arParams[3] = new SqlParameter("@processors", _processors);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addHostVMWare", arParams);
        }
        public DataSet GetVMWareHost(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getHostVMWare", arParams);
        }
        public string GetVMWareHost(int _assetid, string _column)
        {
            DataSet ds = GetVMWareHost(_assetid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetVMWareHosts(int _hostid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@hostid", _hostid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getHostsVMWare", arParams);
        }
        public DataSet GetVMWareHostsStorage(int _hostid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@hostid", _hostid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getHostsStorageVMWare", arParams);
        }
        #endregion

        // IPs
        #region IP ADDRESSES
        public void AddIP(int _assetid, int _ipaddressid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@ipaddressid", _ipaddressid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addIP", arParams);
        }
        public DataSet GetIP(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIPs", arParams);
        }
        public void DeleteIP(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteIPs", arParams);
        }
        #endregion
        
        // BLADES
        #region Blades
        public void AddEnclosure(int _enclosureid, int _drid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@enclosureid", _enclosureid);
            arParams[1] = new SqlParameter("@drid", _drid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addEnclosureDR", arParams);
        }
        public int GetEnclosureDR(int _enclosureid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enclosureid", _enclosureid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getEnclosureDR", arParams);
            if (o == null || o.ToString() == "")
                return 0;
            else
                return Int32.Parse(o.ToString());
        }

        public void AddEnclosure(int _assetid, string _name, int _status, int _userid, DateTime _datestamp, int _classid, int _environmentid, int _rackid, string _rackposition, int _vlan, string _oa_ip, int _resiliencyid)
        {
            AddStatus(_assetid, _name, _status, _userid, _datestamp);
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@rackid", _rackid);
            arParams[4] = new SqlParameter("@rackposition", _rackposition);
            arParams[5] = new SqlParameter("@vlan", _vlan);
            arParams[6] = new SqlParameter("@oa_ip", _oa_ip);
            arParams[7] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[8] = new SqlParameter("@createdby", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addEnclosure", arParams);
        }


        //public void AddEnclosure(int _assetid, string _name, int _status, int _userid, DateTime _datestamp, int _classid, int _environmentid, int _addressid, int _roomid, int _rackid, string _rackposition, int _vlan, string _oa_ip)
        //{
        //    AddStatus(_assetid, _name, _status, _userid, _datestamp);
        //    arParams = new SqlParameter[10];
        //    arParams[0] = new SqlParameter("@assetid", _assetid);
        //    arParams[1] = new SqlParameter("@classid", _classid);
        //    arParams[2] = new SqlParameter("@environmentid", _environmentid);
        //    arParams[3] = new SqlParameter("@addressid", _addressid);
        //    arParams[4] = new SqlParameter("@roomid", _roomid);
        //    arParams[5] = new SqlParameter("@rackid", _rackid);
        //    arParams[6] = new SqlParameter("@rackposition", _rackposition);
        //    arParams[7] = new SqlParameter("@vlan", _vlan);
        //    arParams[8] = new SqlParameter("@oa_ip", _oa_ip);
        //    arParams[9] = new SqlParameter("@createdby", _userid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addEnclosure", arParams);
        //}

        public void UpdateEnclosure(int _assetid, string _name, int _status, int _userid, DateTime _datestamp, int _classid, int _environmentid, int _rackid, string _rackposition, int _vlan, string _oa_ip, int _resiliencyid)
        {
            AddStatus(_assetid, _name, _status, _userid, _datestamp);
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@rackid", _rackid);
            arParams[4] = new SqlParameter("@rackposition", _rackposition);
            arParams[5] = new SqlParameter("@vlan", _vlan);
            arParams[6] = new SqlParameter("@oa_ip", _oa_ip);
            arParams[7] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[8] = new SqlParameter("@modifiedby", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnclosure", arParams);
        }

        //public void UpdateEnclosure(int _assetid, string _name, int _status, int _userid, DateTime _datestamp, int _classid, int _environmentid, int _addressid, int _roomid, int _rackid, string _rackposition, int _vlan, string _oa_ip)
        //{
        //    AddStatus(_assetid, _name, _status, _userid, _datestamp);
        //    arParams = new SqlParameter[10];
        //    arParams[0] = new SqlParameter("@assetid", _assetid);
        //    arParams[1] = new SqlParameter("@classid", _classid);
        //    arParams[2] = new SqlParameter("@environmentid", _environmentid);
        //    arParams[3] = new SqlParameter("@addressid", _addressid);
        //    arParams[4] = new SqlParameter("@roomid", _roomid);
        //    arParams[5] = new SqlParameter("@rackid", _rackid);
        //    arParams[6] = new SqlParameter("@rackposition", _rackposition);
        //    arParams[7] = new SqlParameter("@vlan", _vlan);
        //    arParams[8] = new SqlParameter("@oa_ip", _oa_ip);
        //    arParams[9] = new SqlParameter("@modifiedby", _userid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnclosure", arParams);
        //}
        //Update Enclsoure Info and Status
        //public void UpdateEnclosureInfoOnly(int _assetid, string _name, int _status, int _userid, DateTime _datestamp, int _classid, int _environmentid, int _addressid, int _roomid, int _rackid, string _rackposition, int _vlan, string _oa_ip)
        //{
        //    //UpdateStatus(_assetid, _name, _status, _userid, _datestamp);
        //    arParams = new SqlParameter[10];
        //    arParams[0] = new SqlParameter("@assetid", _assetid);
        //    arParams[1] = new SqlParameter("@classid", _classid);
        //    arParams[2] = new SqlParameter("@environmentid", _environmentid);
        //    arParams[3] = new SqlParameter("@addressid", _addressid);
        //    arParams[4] = new SqlParameter("@roomid", _roomid);
        //    arParams[5] = new SqlParameter("@rackid", _rackid);
        //    arParams[6] = new SqlParameter("@rackposition", _rackposition);
        //    arParams[7] = new SqlParameter("@vlan", _vlan);
        //    arParams[8] = new SqlParameter("@oa_ip", _oa_ip);
        //    arParams[9] = new SqlParameter("@modifiedby", _userid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnclosure", arParams);
        //}

        public void UpdateEnclosureInfoOnly(int _assetid, string _name, int _status, int _userid, DateTime _datestamp, int _classid, int _environmentid, int _rackid, string _rackposition, int _vlan, string _oa_ip, int _resiliencyid)
        {
            //UpdateStatus(_assetid, _name, _status, _userid, _datestamp);
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@rackid", _rackid);
            arParams[4] = new SqlParameter("@rackposition", _rackposition);
            arParams[5] = new SqlParameter("@vlan", _vlan);
            arParams[6] = new SqlParameter("@oa_ip", _oa_ip);
            arParams[7] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[8] = new SqlParameter("@modifiedby", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnclosure", arParams);
        }

        public void DeleteEnclosure(int _assetid, bool _redeploy, int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteEnclosure", arParams);
            if (_redeploy == true)
                AddStatus(_assetid, GetStatus(_assetid, "name"), (int)AssetStatus.Arrived, _userid, DateTime.Now);
        }
        public void DeleteEnclosureBlades(int _assetid, bool _redeploy, int _userid)
        {
            DataSet ds = GetEnclosureBlades(_assetid);
            foreach (DataRow dr in ds.Tables[0].Rows)
                DeleteBlade(Int32.Parse(dr["assetid"].ToString()), _redeploy, _userid);
        }
        public DataSet GetEnclosures(int _status)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@status", _status);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnclosures", arParams);
        }
        //Get Enclosures based on Model Id and Status
        public DataSet GetEnclosures(int _modelid,int _status)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@modelId", _modelid);
            arParams[1] = new SqlParameter("@status", _status);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnclosures", arParams);
        }
        //public DataSet GetEnclosuresClass(int _classid)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@classid", _classid);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnclosuresClass", arParams);
        //}
        public DataSet GetEnclosuresClass(string _classes)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@classes", _classes);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnclosuresClasses", arParams);
        }
        public DataSet GetEnclosuresDR()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnclosuresDR");
        }
        public DataSet GetEnclosureBlades(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnclosureBlades", arParams);
        }
        public DataSet GetEnclosure(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnclosure", arParams);
        }
        public string GetEnclosure(int _assetid, string _column)
        {
            DataSet ds = GetEnclosure(_assetid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public string ExecuteVirtualConnectHelper(int _assetid, string _command, int _environment)
        {
            int intEnclosure = Int32.Parse(GetServerOrBlade(_assetid, "enclosureid"));
            return ExecuteVirtualConnectHelperEnclosure(intEnclosure, _command, _environment);
        }
        public string ExecuteVirtualConnectHelperEnclosure(int _enclosureid, string _command, int _environment)
        {
            Variables oVariable = new Variables(_environment);
            string strHost = "";
            string strReturn = "";
            DataSet dsVirtualConnect = GetEnclosureVCs(_enclosureid, 1);
            foreach (DataRow drVirtualConnect in dsVirtualConnect.Tables[0].Rows)
            {
                strHost = drVirtualConnect["virtual_connect"].ToString();
                try
                {
                    strReturn = ExecuteVirtualConnect(strHost, oVariable.ILOUsername(), oVariable.ILOPassword(), _command);
                }
                catch { }
                if (strReturn != "")
                    break;
            }
            if (strReturn != "")
                strReturn += " (" + strHost + ")";
            return strReturn;
        }
        public string ExecuteVirtualConnect(string _host, string _username, string _password, string _commands)
        {
            SshExec oSSH = new SshExec(_host, _username, _password);
            oSSH.Connect();
            string strReturn = "";
            char[] strSplit = { ';' };
            string[] strCommand = _commands.Split(strSplit);
            for (int ii = 0; ii < strCommand.Length; ii++)
            {
                if (strCommand[ii].Trim() != "")
                {
                    strVirtualConnect = "Host = " + _host + ", Command = " + strCommand[ii].Trim();
                    string strReturnTemp = oSSH.RunCommand(strCommand[ii].Trim());
                    // Virtual Connect Manager not found at this IP address. Please use IP Address: 10.249.236.122 ; 
                    if (strReturnTemp.ToUpper().Contains("VIRTUAL CONNECT MANAGER NOT FOUND AT THIS IP ADDRESS") == true) 
                    {
                        strReturn = "";
                        break;
                    }
                    else
                        strReturn += strReturnTemp + ";";
                }
            }
            oSSH.Close();
            return strReturn;
        }
        public string ExecuteVirtualConnectIP(int _assetid, int _answerid, int _environment, string _vlan, int _nic, bool _PXE_enabled, bool _PXE_disabled, bool _PXE_UseBios)
        {
            return ExecuteVirtualConnectIP(_assetid, _answerid, _environment, _vlan, _nic, _PXE_enabled, _PXE_disabled, _PXE_UseBios, false);
        }
        public string ExecuteVirtualConnectIP(int _assetid, int _answerid, int _environment, string _vlan, int _nic, bool _PXE_enabled, bool _PXE_disabled, bool _PXE_UseBios, bool _overwrite_power)
        {
            // Since currently, only virtual connect IP is available for HP (not DELL), don't worry about the _EXE_LOCATION parameter
            AssetPowerStatus powStatus = PowerStatus(_assetid, _answerid, _environment, "");
            if (powStatus == AssetPowerStatus.Error)
                return "ERROR: There was a problem connecting to virtual connect";
            else if (powStatus != AssetPowerStatus.Off && _overwrite_power == false)
                return "ERROR: The device is not powered off (" + powStatus.ToString() + ")";
            else
            {
                string strProfile = GetVirtualConnectSetting(_assetid, "Server Profile", _environment);
                if (strProfile == "" || strProfile == "<Unassigned>")
                    return "ERROR: The profile is invalid (" + strProfile + ")";
                else
                    return ExecuteVirtualConnectHelper(_assetid, "set enet-connection " + strProfile + " " + _nic.ToString() + " Network=" + _vlan + (_PXE_enabled == true || _PXE_disabled == true || _PXE_UseBios == true ? ";set enet-connection " + strProfile + " " + _nic.ToString() + " PXE=" + (_PXE_enabled ? "enabled" : (_PXE_disabled ? "disabled" : "UseBios")) : ""), _environment);
            }
        }
        public string GetVirtualConnectMACAddress(int _assetid, int _answerid, int _environment, int _nic, string _file_location, string _dsn, string _exe_location, Log oLog, string _name)
        {
            string strSerial = Get(_assetid, "serial");
            string strReturn = "";
            string strErrorFile = "";
            int intModelProperty = 0;
            if (Int32.TryParse(Get(_assetid, "modelid"), out intModelProperty) == true)
            {
                Models oModel = new Models(0, _dsn);
                ModelsProperties oModelsProperties = new ModelsProperties(0, _dsn);
                int intModel = 0;
                if (Int32.TryParse(oModelsProperties.Get(intModelProperty, "modelid"), out intModel) == true)
                {
                    if (oModel.Get(intModel, "grouping") == "2")
                    {
                        oLog.AddEvent(_answerid, _name, strSerial, "Model is Midrange", LoggingType.Debug);
                        // Midrange
                        int intModelBootGroup = 0;
                        Int32.TryParse(oModel.Get(intModel, "boot_groupid"), out intModelBootGroup);
                        string strHost = GetServerOrBlade(_assetid, "ilo");
                        Solaris oSolaris = new Solaris(0, _dsn);
                        strReturn = oSolaris.GetMacAddress(strHost, intModelBootGroup, _environment, oLog, _name, strSerial);
                        oLog.AddEvent(_answerid, _name, strSerial, "Return value = " + strReturn, LoggingType.Debug);
                        if (strReturn == "")
                        {
                            strReturn = "**ERROR**";
                            strErrorFile = "Problem getting from Solaris";
                        }
                    }
                    else
                    {
                        oLog.AddEvent(_answerid, _name, strSerial, "Model is Not Midrange", LoggingType.Debug);
                        strResultsFile = "";
                        bool boolErrorFile = false;
                        // Distributed
                        if (oModelsProperties.IsDell(intModelProperty) == true)
                        {
                            oLog.AddEvent(_answerid, _name, strSerial, "Model is Dell", LoggingType.Debug);
                            // DELL
                            string strValue = GetDellSysInfo(_assetid, _answerid, intModelProperty, _exe_location, DellQueryType.MacAddress, _nic);
                            oLog.AddEvent(_answerid, _name, strSerial, "Return value = " + strValue, LoggingType.Debug);
                            if (strValue == "")
                            {
                                strReturn = "**ERROR**";
                                strErrorFile = "Problem getting from Dell";
                            }
                            else
                                strReturn = strValue;
                        }
                        else
                        {
                            oLog.AddEvent(_answerid, _name, strSerial, "Model is HP", LoggingType.Debug);
                            // HP
                            string strProfile = GetVirtualConnectSetting(_assetid, "Server Profile", _environment);
                            if (strProfile != "" && strProfile != "<Unassigned>")
                            {
                                strResultsFile = ExecuteVirtualConnectHelper(_assetid, "show profile " + strProfile, _environment);
                                try
                                {
                                    string strResult = strResultsFile.Substring(strResultsFile.IndexOf("Ethernet Network Connections"));
                                    strResult = strResult.Substring(strResult.IndexOf("==\n") + 3);
                                    strResult = strResult.Substring(strResult.IndexOf("==\n") + 3);
                                    strResult = strResult.Substring(0, strResult.IndexOf("\nFC SAN Connections"));
                                    char[] strSplit = { '\n' };
                                    string[] strMACs = strResult.Split(strSplit);
                                    for (int ii = 0; ii < strMACs.Length; ii++)
                                    {
                                        if (strMACs[ii].Trim() != "" && _nic == (ii + 1))
                                        {
                                            string strMAC = strMACs[ii].Trim();
                                            while (strMAC.Contains("  ") == true)
                                                strMAC = strMAC.Replace("  ", " ");
                                            string strResultValue = strMAC;
                                            string strNumber = strResultValue.Substring(0, strResultValue.IndexOf(" "));
                                            strResultValue = strResultValue.Substring(strResultValue.IndexOf(" ") + 1);
                                            string strVLAN = strResultValue.Substring(0, strResultValue.IndexOf(" "));
                                            strResultValue = strResultValue.Substring(strResultValue.IndexOf(" ") + 1);
                                            string strStatus = strResultValue.Substring(0, strResultValue.IndexOf(" "));
                                            strResultValue = strResultValue.Substring(strResultValue.IndexOf(" ") + 1);
                                            string strPXE = strResultValue.Substring(0, strResultValue.IndexOf(" "));
                                            strResultValue = strResultValue.Substring(strResultValue.IndexOf(" ") + 1);
                                            // At this point, either the MAC address alone or the MAC address with information at the end exists.
                                            if (strResultValue.IndexOf(" ") > -1)
                                            {
                                                // Get rid of the information at the end
                                                strResultValue = strResultValue.Substring(0, strResultValue.IndexOf(" "));
                                            }
                                            else
                                            {
                                                // Nothing to do since only the MAC address exists
                                            }
                                            strReturn = strResultValue;
                                            oLog.AddEvent(_answerid, _name, strSerial, "Return value = " + strReturn, LoggingType.Debug);
                                            break;
                                        }
                                    }
                                }
                                catch
                                {
                                    boolErrorFile = true;
                                    strErrorFile = strResultsFile;
                                    oLog.AddEvent(_answerid, _name, strSerial, strErrorFile, LoggingType.Error);
                                }
                            }
                            else
                            {
                                strReturn = "**ERROR**";
                                strErrorFile = "Unassigned Profile (" + strProfile + ")";
                                oLog.AddEvent(_answerid, _name, strSerial, strErrorFile, LoggingType.Error);
                            }
                        }

                        if (boolErrorFile == true)
                        {
                            StreamWriter filMAC = File.CreateText(_file_location + _assetid.ToString() + ".txt");
                            filMAC.Write(strErrorFile); // Write the actual error
                            filMAC.Flush();
                            filMAC.Close();
                            strReturn = "**ERROR**";
                        }
                    }
                }
                else
                {
                    // 1 of 2: The only way there wouldn't be a results file (ASSET#.TXT) is if the code reached here
                    strReturn = "**ERROR**";
                    oLog.AddEvent(_answerid, _name, strSerial, "ERROR # 1", LoggingType.Error);
                }
            }
            else
            {
                // 2 of 2: The only way there wouldn't be a results file (ASSET#.TXT) is if the code reached here
                strReturn = "**ERROR**";
                oLog.AddEvent(_answerid, _name, strSerial, "ERROR # 2", LoggingType.Error);
            }
            return strReturn;
        }
        public string GetWWPNs(int _assetid, int _answerid, int _environment, string _file_location, string _dsn, string _exe_location, Log oLog, string _name)
        {
            string strError = "";
            string strSerial = Get(_assetid, "serial");
            int intModelProperty = 0;
            if (Int32.TryParse(Get(_assetid, "modelid"), out intModelProperty) == true)
            {
                Models oModel = new Models(0, _dsn);
                ModelsProperties oModelsProperties = new ModelsProperties(0, _dsn);
                int intModel = 0;
                if (Int32.TryParse(oModelsProperties.Get(intModelProperty, "modelid"), out intModel) == true)
                {
                    if (oModel.Get(intModel, "grouping") == "2")
                    {
                        oLog.AddEvent(_answerid, _name, strSerial, "Model is Midrange - cannot query WWPNs", LoggingType.Debug);
                    }
                    else
                    {
                        oLog.AddEvent(_answerid, _name, strSerial, "Model is Not Midrange", LoggingType.Debug);
                        strResultsFile = "";
                        // Distributed
                        if (oModelsProperties.IsDell(intModelProperty) == true)
                        {
                            oLog.AddEvent(_answerid, _name, strSerial, "Model is Dell", LoggingType.Debug);
                            // DELL
                            for (int ii = 1; ii <= 2; ii++)
                            {
                                string strValue = GetDellSysInfo(_assetid, _answerid, intModelProperty, _exe_location, DellQueryType.WWPN, ii);
                                oLog.AddEvent(_answerid, _name, strSerial, "Return value = " + strValue, LoggingType.Debug);
                                if (strValue == "")
                                {
                                    strError = "There was a problem getting the WWPNs ~ nothing returned";
                                    break;
                                }
                                else
                                {
                                    // strValue has the WWPN
                                    AddHBA(_assetid, strValue);
                                    oLog.AddEvent(_answerid, _name, strSerial, "WWPN " + strValue + " added to " + strSerial, LoggingType.Debug);
                                }
                            }
                        }
                        else
                        {
                            oLog.AddEvent(_answerid, _name, strSerial, "Model is HP - cannot query WWPNs", LoggingType.Debug);
                        }
                    }
                }
                else
                {
                    // 1 of 2: The only way there wouldn't be a results file (ASSET#.TXT) is if the code reached here
                    oLog.AddEvent(_answerid, _name, strSerial, "Unable to get model ID for model property ID = " + intModelProperty.ToString(), LoggingType.Error);
                    strError = "There was a problem getting the WWPNs ~ could not get model ID";
                }
            }
            else
            {
                // 2 of 2: The only way there wouldn't be a results file (ASSET#.TXT) is if the code reached here
                oLog.AddEvent(_answerid, _name, strSerial, "Unable to get model property ID for asset ID = " + _assetid.ToString(), LoggingType.Error);
                strError = "There was a problem getting the WWPNs ~ could not get model property ID";
            }
            return strError;
        }
        public AssetPowerStatus PowerStatus(int _assetid, int _answerid, int _environment, string _exe_location)
        {
            int intTimeoutDefault = (5 * 60 * 1000);    // 5 minutes
            bool boolTimeout = false;
            AssetPowerStatus oReturn = AssetPowerStatus.Error;

            Log oLog = new Log(user, dsnCV);
            ModelsProperties oModelsProperties = new ModelsProperties(user, dsnCV);
            Variables oVarPower = new Variables(_environment);

            //string strSerial = Get(_assetid, "serial");
            string strILO = GetServerOrBlade(_assetid, "ilo").ToUpper();
            string _name = GetStatus(_assetid, "name");
            string strSerial = Get(_assetid, "serial");

            int intModelProperty = 0;
            if (strILO != "" && Int32.TryParse(Get(_assetid, "modelid"), out intModelProperty) == true)
            {

                // Distributed
                if (oModelsProperties.IsDell(intModelProperty) == true)
                {
                    oLog.AddEvent(_answerid, _name, strSerial, "Model is DELL...checking using DRAC " + strILO, LoggingType.Information);
                    // DELL
                    string strValue = GetDellSysInfo(_assetid, _answerid, intModelProperty, _exe_location, DellQueryType.Power, 0);
                    if (strValue == "")
                    {
                        oLog.AddEvent(_answerid, _name, strSerial, "** No return value from API - this could mean a problem logging into the DRAC", LoggingType.Information);
                        oReturn = AssetPowerStatus.Error;
                    }
                    else if (strValue.Trim().ToUpper() == "OFF")
                    {
                        oLog.AddEvent(_answerid, _name, strSerial, "Power = OFF", LoggingType.Information);
                        oReturn = AssetPowerStatus.Off;
                    }
                    else
                    {
                        oLog.AddEvent(_answerid, _name, strSerial, "Power = ON", LoggingType.Information);
                        oReturn = AssetPowerStatus.On;
                    }
                }
                else
                {
                    // HP
                    string strPower = "";
                    try
                    {
                        strPower = GetVirtualConnectSetting(_assetid, "Power", _environment);
                    }
                    catch
                    {
                        strPower = "ERROR";
                    }
                    if (strPower == "ERROR")
                        oReturn = AssetPowerStatus.Error;
                    else if (strPower.Trim().ToUpper() == "OFF")
                        oReturn = AssetPowerStatus.Off;
                    else
                        oReturn = AssetPowerStatus.On;
                }
            }

            return oReturn;
        }
        private string GetDellSysInfo(int _assetid, int _answerid, int _modelid, string _exe_location, DellQueryType _type, int _nic)
        {
            string strReturn = "";
            Log oLog = new Log(0, dsnCV);
            string _name = GetStatus(_assetid, "name");
            string strSerial = Get(_assetid, "serial");

            int intDellConfig = 0;
            ModelsProperties oModelsProperties = new ModelsProperties(user, dsnCV);
            bool boolBlade = oModelsProperties.IsTypeBlade(_modelid);
            Int32.TryParse(oModelsProperties.Get(_modelid, "dellconfigid"), out intDellConfig);
            Dells oDell = new Dells(user, dsnCV);
            DataSet dsDell = oDell.Get(intDellConfig);
            if (dsDell.Tables[0].Rows.Count == 1)
            {
                DataRow drDell = dsDell.Tables[0].Rows[0];
                string strDellSplit = drDell["xml_split"].ToString().ToUpper();
                string strDellOperator = drDell["xml_operator"].ToString().ToUpper();
                string strDellStart = drDell["xml_start"].ToString().ToUpper();
                string strDellQueryPower = drDell["query_power"].ToString().ToUpper();
                string strDellQueryMAC1 = drDell["query_mac1"].ToString().ToUpper();
                string strDellQueryMAC2 = drDell["query_mac2"].ToString().ToUpper();
                string strDellPowerOn = drDell["success_power_on"].ToString().ToUpper();
                string strDellPowerOff = drDell["success_power_off"].ToString().ToUpper();
                string strDellUsername = drDell["username"].ToString();
                string strDellPassword = drDell["password"].ToString();
                string strILO = GetServerOrBlade(_assetid, "ilo").ToUpper();
                oLog.AddEvent(_answerid, _name, strSerial, "Found DELL Config for ILO " + strILO, LoggingType.Debug);

                // Initialize the RACADM command.
                if (_exe_location == "" ||
                        strDellSplit == "" ||
                        strDellOperator == "" ||
                        strDellStart == "" ||
                        strDellQueryPower == "" ||
                        strDellQueryMAC1 == "" ||
                        strDellQueryMAC2 == "" ||
                        strDellPowerOn == "" ||
                        strDellPowerOff == "" ||
                        strILO == "")
                    strReturn = "";
                else
                {
                    char[] chrDellSplit = { '\n' };
                    int intTimeoutDefault = (5 * 60 * 1000);    // 5 minutes
                    bool boolTimeout = false;
                    Variables oVariable = new Variables(999);   // PNC
                    if (strDellUsername == "" || strDellPassword == "")
                    {
                        strDellUsername = oVariable.ADUser() + "@" + oVariable.FullyQualified();
                        strDellPassword = oVariable.ADPassword();
                    }
                    if (_type == DellQueryType.WWPN)
                    {
                        oLog.AddEvent(_answerid, _name, strSerial, "Getting WWPN...", LoggingType.Debug);
                        ProcessStartInfo processStartInfo = new ProcessStartInfo(_exe_location + "racadm.exe");
                        processStartInfo.WorkingDirectory = _exe_location;
                        processStartInfo.Arguments = "-r " + strILO + " -u " + strDellUsername + " -p " + strDellPassword + " hwinventory FC";
                        oLog.AddEvent(_answerid, _name, strSerial, "Executing command " + _exe_location + "racadm.exe -r " + strILO + " -u " + strDellUsername + " -p *** hwinventory FC", LoggingType.Debug);
                        processStartInfo.UseShellExecute = false;
                        processStartInfo.RedirectStandardOutput = true;
                        Process proc = Process.Start(processStartInfo);
                        StreamReader outputReader = proc.StandardOutput;
                        proc.WaitForExit(intTimeoutDefault);
                        if (proc.HasExited == false)
                        {
                            proc.Kill();
                            boolTimeout = true;
                        }
                        if (boolTimeout == false)
                        {
                            strResultsFile = outputReader.ReadToEnd().ToUpper();
                            oLog.AddEvent(_answerid, _name, strSerial, "Return value = " + strResultsFile, LoggingType.Debug);
                            try
                            {
                                string[] lines = strResultsFile.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                                int counter = 0;
                                foreach (string line in lines)
                                {
                                    string replaced = line.Replace("\r", "").Trim();
                                    if (replaced.ToUpper().StartsWith("FC.MEZZANINE") || replaced.ToUpper().StartsWith("FC.SLOT"))
                                    {
                                        counter++;
                                        if (counter == _nic)
                                        {
                                            strReturn = replaced.Substring(replaced.LastIndexOf("-") + 1);
                                            strReturn = strReturn.Trim();
                                            break;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                oLog.AddEvent(_answerid, _name, strSerial, ex.Message, LoggingType.Error);
                            }
                        }
                        proc.Close();
                    }
                    if (_type == DellQueryType.Power)
                    {
                        oLog.AddEvent(_answerid, _name, strSerial, "Checking power status...", LoggingType.Debug);
                        ProcessStartInfo processStartInfo = new ProcessStartInfo(_exe_location + "racadm.exe");
                        processStartInfo.WorkingDirectory = _exe_location;
                        processStartInfo.Arguments = "-r " + strILO + " -u " + strDellUsername + " -p " + strDellPassword + " getsysinfo -s";
                        oLog.AddEvent(_answerid, _name, strSerial, "Executing command " + _exe_location + "racadm.exe -r " + strILO + " -u " + strDellUsername + " -p *** getsysinfo -s", LoggingType.Debug);
                        processStartInfo.UseShellExecute = false;
                        processStartInfo.RedirectStandardOutput = true;
                        Process proc = Process.Start(processStartInfo);
                        StreamReader outputReader = proc.StandardOutput;
                        proc.WaitForExit(intTimeoutDefault);
                        if (proc.HasExited == false)
                        {
                            proc.Kill();
                            boolTimeout = true;
                        }
                        if (boolTimeout == false)
                        {
                            strResultsFile = outputReader.ReadToEnd().ToUpper();
                            oLog.AddEvent(_answerid, _name, strSerial, "Return value = " + strResultsFile, LoggingType.Debug);
                            try
                            {
                                strReturn = ParseDellOutput(strResultsFile, strDellStart, strDellQueryPower, strDellOperator, chrDellSplit);
                            }
                            catch (Exception ex)
                            {
                                oLog.AddEvent(_answerid, _name, strSerial, ex.Message, LoggingType.Error);
                            }
                        }
                        proc.Close();
                    }
                    if (_type == DellQueryType.MacAddress)
                    {
                        oLog.AddEvent(_answerid, _name, strSerial, "Checking MAC Address...", LoggingType.Debug);
                        if (boolBlade == true)
                        {
                            oLog.AddEvent(_answerid, _name, strSerial, "This is a blade", LoggingType.Debug);
                            int intEnclosure = Int32.Parse(GetServerOrBlade(_assetid, "enclosureid"));
                            int intEnclosureModel = Int32.Parse(Get(intEnclosure, "modelid"));
                            int intEnclosureDellConfig = 0;
                            Int32.TryParse(oModelsProperties.Get(intEnclosureModel, "dellconfigid"), out intEnclosureDellConfig);
                            DataSet dsEnclosureDell = oDell.Get(intEnclosureDellConfig);
                            if (dsEnclosureDell.Tables[0].Rows.Count == 1)
                            {
                                DataRow drEnclosureDell = dsEnclosureDell.Tables[0].Rows[0];
                                strDellUsername = drEnclosureDell["username"].ToString();
                                strDellPassword = drEnclosureDell["password"].ToString();
                                if (strDellUsername == "" || strDellPassword == "")
                                {
                                    strDellUsername = oVariable.ADUser() + "@" + oVariable.FullyQualified();
                                    strDellPassword = oVariable.ADPassword();
                                }
                            }
                            string strSlot = GetServerOrBlade(_assetid, "slot");
                            string strOA = GetEnclosure(intEnclosure, "oa_ip");
                            oLog.AddEvent(_answerid, _name, strSerial, "Connecting to Management Console " + strOA, LoggingType.Debug);

                            SshShell oSSHshell = new SshShell(strOA, strDellUsername, strDellPassword);
                            oSSHshell.RemoveTerminalEmulationCharacters = true;
                            oSSHshell.Connect();
                            if (oSSHshell.Connected == true && oSSHshell.ShellOpened == true)
                            {
                                oLog.AddEvent(_answerid, _name, strSerial, "Logged into enclosure (" + strOA + ")", LoggingType.Debug);
                                while (strResultsFile.Contains("$") == false)
                                {
                                    strResultsFile += oSSHshell.Expect("$").ToUpper();
                                    oLog.AddEvent(_answerid, _name, strSerial, "Found (banner) $ : " + strResultsFile, LoggingType.Debug);
                                }
                                strResultsFile = "";
                                oLog.AddEvent(_answerid, _name, strSerial, "Writing : " + "getflexaddr -i " + strSlot, LoggingType.Debug);
                                oSSHshell.WriteLine("getflexaddr -i " + strSlot);
                                while (strResultsFile.Contains("$") == false)
                                {
                                    strResultsFile += oSSHshell.Expect("$").ToUpper();
                                    oLog.AddEvent(_answerid, _name, strSerial, "Found $ : " + strResultsFile, LoggingType.Debug);
                                }
                            }
                            else
                                oLog.AddEvent(_answerid, _name, strSerial, "Disconnected from the OA", LoggingType.Error);
                            try
                            {
                                if (strResultsFile.Contains(strDellQueryMAC1) == true)
                                {
                                    strResultsFile = strResultsFile.Substring(strResultsFile.IndexOf(strDellQueryMAC1));
                                    oLog.AddEvent(_answerid, _name, strSerial, strResultsFile, LoggingType.Debug);
                                    if (_nic > 1)
                                    {
                                        if (strResultsFile.Contains(strDellQueryMAC1) == true)
                                        {
                                            strResultsFile = strResultsFile.Substring(strDellQueryMAC1.Length);
                                            oLog.AddEvent(_answerid, _name, strSerial, strResultsFile, LoggingType.Debug);
                                            if (strResultsFile.Contains(strDellQueryMAC2) == true)
                                            {
                                                strResultsFile = strResultsFile.Substring(strResultsFile.IndexOf(strDellQueryMAC2));
                                                oLog.AddEvent(_answerid, _name, strSerial, strResultsFile, LoggingType.Debug);
                                            }
                                            else
                                                oLog.AddEvent(_answerid, _name, strSerial, strDellQueryMAC2 + " was not found in " + strResultsFile, LoggingType.Error);
                                        }
                                        else
                                            oLog.AddEvent(_answerid, _name, strSerial, strDellQueryMAC1 + " was not found in " + strResultsFile, LoggingType.Error);
                                    }

                                    char[] strSplit = { '\n' };
                                    string strLine = "";
                                    string[] strResults = strResultsFile.Split(strSplit);
                                    if (strResults.Length > 0)
                                        strLine = strResults[0].Trim().ToUpper();
                                    while (strLine.Contains("  ") == true)
                                        strLine = strLine.Replace("  ", " ");
                                    if (strLine.Contains(" ") == true)
                                        strReturn = strLine.Substring(strLine.LastIndexOf(" ") + 1);
                                    if (strReturn.Contains("(") == true)
                                        strReturn = strReturn.Substring(0, strReturn.IndexOf("("));
                                }
                                else
                                    oLog.AddEvent(_answerid, _name, strSerial, strDellQueryMAC1 + " was not found in " + strResultsFile, LoggingType.Error);
                            }
                            catch (Exception ex)
                            {
                                oLog.AddEvent(_answerid, _name, strSerial, ex.Message, LoggingType.Error);
                            }
                            if (oSSHshell.Connected == true && oSSHshell.ShellOpened == true)
                            {
                                oSSHshell.WriteLine("exit");
                                oLog.AddEvent(_answerid, _name, strSerial, "Logged out of enclosure", LoggingType.Debug);
                            }
                        }
                        else
                        {
                            oLog.AddEvent(_answerid, _name, strSerial, "This is a NOT blade", LoggingType.Debug);
                            ProcessStartInfo processStartInfo = new ProcessStartInfo(_exe_location + "racadm.exe");
                            oLog.AddEvent(_answerid, _name, strSerial, "Executing command " + _exe_location + "racadm.exe -r " + strILO + " -u " + strDellUsername + " -p *** getsysinfo -s", LoggingType.Debug);
                            processStartInfo.WorkingDirectory = _exe_location;
                            //processStartInfo.Arguments = "-r " + strOA + " -u " + strDellUsername + " -p " + strDellPassword + " getflexaddr -i " + strSlot;
                            processStartInfo.Arguments = "-r " + strILO + " -u " + strDellUsername + " -p " + strDellPassword + " getsysinfo -s";
                            processStartInfo.UseShellExecute = false;
                            processStartInfo.RedirectStandardOutput = true;
                            Process proc = Process.Start(processStartInfo);
                            StreamReader outputReader = proc.StandardOutput;
                            proc.WaitForExit(intTimeoutDefault);
                            if (proc.HasExited == false)
                            {
                                proc.Kill();
                                boolTimeout = true;
                            }
                            if (boolTimeout == false)
                            {
                                strResultsFile = outputReader.ReadToEnd().ToUpper();
                                oLog.AddEvent(_answerid, _name, strSerial, "Return value = " + strResultsFile, LoggingType.Debug);
                                try
                                {
                                    if (_nic == 1)
                                        strReturn = ParseDellOutput(strResultsFile, strDellStart, strDellQueryMAC1, strDellOperator, chrDellSplit);
                                    else
                                        strReturn = ParseDellOutput(strResultsFile, strDellStart, strDellQueryMAC2, strDellOperator, chrDellSplit);
                                }
                                catch (Exception ex)
                                {
                                    oLog.AddEvent(_answerid, _name, strSerial, ex.Message, LoggingType.Error);
                                }
                            }
                            proc.Close();
                        }
                    }
                }
            }
            return strReturn;
        }
        private string ParseDellOutput(string strContents, string strStart, string strFind, string strOperator, char[] chrDellSplit)
        {
            string strReturn = "";
            char[] strSplit = { ';' };
            string[] strStarts = strStart.Split(strSplit);
            for (int ii = 0; ii < strStarts.Length; ii++)
            {
                if (strStarts[ii].Trim() != "")
                {
                    if (strContents.Contains(strStarts[ii].Trim()) == true)
                    {
                        // Get to the contents
                        string strResult = strContents.Substring(strContents.IndexOf(strStarts[ii].Trim()));
                        string[] strResults = strResult.Split(chrDellSplit);
                        string[] strFinds = strFind.Split(strSplit);
                        // Loop through each value set
                        for (int jj = 0; jj < strResults.Length; jj++)
                        {
                            if (strResults[jj].Trim() != "")
                            {
                                // Loop through all labels
                                for (int kk = 0; kk < strFinds.Length; kk++)
                                {
                                    if (strFinds[kk].Trim() != "" && strResults[jj].StartsWith(strFinds[kk].Trim()) == true)
                                    {
                                        // Found label!
                                        string strLine = strResults[jj].Trim().ToUpper();
                                        if (strLine.Contains(strOperator) == true)
                                        {
                                            string strValue = strLine.Substring(strLine.IndexOf(strOperator) + 1);
                                            strReturn = strValue.Trim();
                                        }
                                        break;
                                    }
                                }
                            }
                            if (strReturn != "")
                                break;
                        }
                    }
                }
                if (strReturn != "")
                    break;
            }
            return strReturn;
        }
        public string GetVirtualConnectSetting(int _assetid, string _setting, int _environment)
        {
            int intSlot = Int32.Parse(GetServerOrBlade(_assetid, "slot"));
            string strResults = ExecuteVirtualConnectHelper(_assetid, "show server enc0:" + intSlot.ToString(), _environment);
            return GetVirtualConnectSetting(strResults, _setting);
        }
        public string GetVirtualConnectSetting(string _results, string _setting)
        {
            string strReturn = "";
            if (_results != "")
            {
                while (_results.Contains(": ") == true)
                    _results = _results.Replace(": ", ":");
                while (_results.Contains(" :") == true)
                    _results = _results.Replace(" :", ":");
                if (_results.Contains(_setting + ":") == true)
                    strReturn = _results.Substring(_results.IndexOf(_setting + ":") + _setting.Length + 1);
                if (strReturn.Contains("\n") == true)
                    strReturn = strReturn.Substring(0, strReturn.IndexOf("\n"));
            }
            return strReturn;
        }
        public DataTable GetVirtualConnectVLANs(int _enclosureid, int _environment, string _file_location, string _dsn)
        {
            DataTable tabNetworks = new DataTable();
            AddColumn("name", "System.String", tabNetworks);
            AddColumn("vlan", "System.Int32", tabNetworks);
            DataRow oDataRow;
            char[] strSplit = { '\n' };
            string strNetworks = ExecuteVirtualConnectHelperEnclosure(_enclosureid, "show network", _environment);
            try
            {
                string[] strNetwork = strNetworks.Split(strSplit);
                for (int ii = 0; ii < strNetwork.Length; ii++)
                {
                    if (strNetwork[ii].Trim() != "" && strNetwork[ii].Trim().Contains("Disabled") == true)
                    {
                        string strLine = strNetwork[ii].Trim();
                        // This line should have a VLAN on it.
                        while (strLine.Contains("  ") == true)
                            strLine = strLine.Replace("  ", " ");

                        string strVLAN = strLine.Substring(0, strLine.IndexOf(" "));
                        strLine = strLine.Substring(strLine.IndexOf(" ") + 1);
                        string strStatus = strLine.Substring(0, strLine.IndexOf(" "));
                        strLine = strLine.Substring(strLine.IndexOf(" ") + 1);
                        string strSharedUplink = strLine.Substring(0, strLine.IndexOf(" "));
                        strLine = strLine.Substring(strLine.IndexOf(" ") + 1);
                        string strVLANID = strLine.Substring(0, strLine.IndexOf(" "));

                        oDataRow = tabNetworks.NewRow();
                        int intVLAN = 0;
                        Int32.TryParse(strVLANID, out intVLAN);
                        oDataRow["name"] = strVLAN;
                        oDataRow["vlan"] = intVLAN;
                        tabNetworks.Rows.Add(oDataRow);
                    }
                }
            }
            catch
            {
                StreamWriter filMAC = File.CreateText(_file_location + _enclosureid.ToString() + ".txt");
                filMAC.Write(strNetworks);
                filMAC.Flush();
                filMAC.Close();
            }
            return tabNetworks;
        }
        public string GetVirtualConnectVLAN(int _assetid, int _environment, char[] _split, string _file_location, string _dsn)
        {
            string strReturn = "";
            int intModelProperty = 0;
            if (Int32.TryParse(Get(_assetid, "modelid"), out intModelProperty) == true)
            {
                Models oModel = new Models(0, _dsn);
                ModelsProperties oModelsProperties = new ModelsProperties(0, _dsn);
                int intModel = 0;
                if (Int32.TryParse(oModelsProperties.Get(intModelProperty, "modelid"), out intModel) == true)
                {
                    if (oModel.Get(intModel, "grouping") == "2")
                    {
                        // Midrange
                        return "**ERROR**";
                    }
                    else
                    {
                        // Distributed
                        string strProfile = GetVirtualConnectSetting(_assetid, "Server Profile", _environment);
                        if (strProfile != "" && strProfile != "<Unassigned>")
                        {
                            string strResults = ExecuteVirtualConnectHelper(_assetid, "show profile " + strProfile, _environment);
                            try
                            {
                                string strResult = strResults.Substring(strResults.IndexOf("Ethernet Network Connections"));
                                strResult = strResult.Substring(strResult.IndexOf("==\n") + 3);
                                strResult = strResult.Substring(strResult.IndexOf("==\n") + 3);
                                strResult = strResult.Substring(0, strResult.IndexOf("\nFC SAN Connections"));
                                char[] strSplit = { '\n' };
                                string[] strMACs = strResult.Split(strSplit);
                                for (int ii = 0; ii < strMACs.Length; ii++)
                                {
                                    if (strMACs[ii].Trim() != "")
                                    {
                                        string strMAC = strMACs[ii].Trim();
                                        while (strMAC.Contains("  ") == true)
                                            strMAC = strMAC.Replace("  ", " ");
                                        string strResultValue = strMAC;
                                        string strNumber = strResultValue.Substring(0, strResultValue.IndexOf(" "));
                                        strResultValue = strResultValue.Substring(strResultValue.IndexOf(" ") + 1);
                                        string strVLAN = strResultValue.Substring(0, strResultValue.IndexOf(" "));
                                        /*
                                        strResultValue = strResultValue.Substring(strResultValue.IndexOf(" ") + 1);
                                        string strStatus = strResultValue.Substring(0, strResultValue.IndexOf(" "));
                                        strResultValue = strResultValue.Substring(strResultValue.IndexOf(" ") + 1);
                                        string strPXE = strResultValue.Substring(0, strResultValue.IndexOf(" "));
                                        strResultValue = strResultValue.Substring(strResultValue.IndexOf(" ") + 1);
                                        // At this point, either the MAC address alone or the MAC address with information at the end exists.
                                        if (strResultValue.IndexOf(" ") > -1)
                                        {
                                            // Get rid of the information at the end
                                            strResultValue = strResultValue.Substring(0, strResultValue.IndexOf(" "));
                                        }
                                        else
                                        {
                                            // Nothing to do since only the MAC address exists
                                        }
                                        strReturn = strResultValue;
                                        */
                                        if (strReturn != "")
                                            strReturn += _split[0].ToString();
                                        strReturn += strVLAN;
                                        //break;
                                    }
                                }
                                return strReturn;
                            }
                            catch
                            {
                                StreamWriter filMAC = System.IO.File.CreateText(_file_location + _assetid.ToString() + ".txt");
                                filMAC.Write(strResults);
                                filMAC.Flush();
                                filMAC.Close();
                                return "**ERROR**";
                            }
                        }
                        else
                            return "**ERROR**";
                    }
                }
                else
                    return "**ERROR**";
            }
            else
                return "**ERROR**";
        }
        private void AddColumn(string _name, string _type, DataTable _table)
        {
            DataColumn myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType(_type);
            myDataColumn.ColumnName = _name;
            _table.Columns.Add(myDataColumn);
        }

        public DataSet GetEnclosureVCs(int _enclosureid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@enclosureid", _enclosureid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnclosureVCs", arParams);
        }
        public DataSet GetEnclosureVC(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnclosureVC", arParams);
        }
        public string GetEnclosureVC(int _id, string _column)
        {
            DataSet ds = GetEnclosureVC(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddEnclosureVC(int _enclosureid, string _virtual_connect, int _display, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@enclosureid", _enclosureid);
            arParams[1] = new SqlParameter("@virtual_connect", _virtual_connect);
            arParams[2] = new SqlParameter("@display", _display);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addEnclosureVC", arParams);
        }
        public void UpdateEnclosureVC(int _id, string _virtual_connect, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@virtual_connect", _virtual_connect);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnclosureVC", arParams);
        }
        public void UpdateEnclosureVCOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnclosureVCOrder", arParams);
        }
        public void EnableEnclosureVC(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnclosureVCEnabled", arParams);
        }
        public void DeleteEnclosureVC(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteEnclosureVC", arParams);
        }



        public void AddBlade(int _assetid, int _status, int _userid, DateTime _datestamp, int _enclosureid, string _ilo, string _dummy_name, string _macaddress, int _vlan, int _build_network_id, int _slot, int _spare, int _resiliencyid, int _operatingsystemgroupid)
        {
            AddStatus(_assetid, "", _status, _userid, _datestamp);
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@enclosureid", _enclosureid);
            arParams[2] = new SqlParameter("@ilo", _ilo);
            arParams[3] = new SqlParameter("@dummy_name", _dummy_name);
            arParams[4] = new SqlParameter("@macaddress", _macaddress);
            arParams[5] = new SqlParameter("@vlan", _vlan);
            arParams[6] = new SqlParameter("@build_network_id", _build_network_id);
            arParams[7] = new SqlParameter("@slot", _slot);
            arParams[8] = new SqlParameter("@spare", _spare);
            arParams[9] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[10] = new SqlParameter("@operatingsystemgroupid", _operatingsystemgroupid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addBlade", arParams);
        }

        public void AddBlade(int _assetid, int _status, int _userid, DateTime _datestamp, int _enclosureid, int _classid, int _environmentid, string _ilo, string _dummy_name, string _macaddress, int _vlan, int _build_network_id, int _slot, int _spare, int _resiliencyid, int _operatingsystemgroupid)
        {
            AddStatus(_assetid, "", _status, _userid, _datestamp);
            arParams = new SqlParameter[14];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@enclosureid", _enclosureid);
            arParams[2] = new SqlParameter("@classid", _classid);
            arParams[3] = new SqlParameter("@environmentid", _environmentid);
            arParams[4] = new SqlParameter("@ilo", _ilo);
            arParams[5] = new SqlParameter("@dummy_name", _dummy_name);
            arParams[6] = new SqlParameter("@macaddress", _macaddress);
            arParams[7] = new SqlParameter("@vlan", _vlan);
            arParams[8] = new SqlParameter("@build_network_id", _build_network_id);
            arParams[9] = new SqlParameter("@slot", _slot);
            arParams[10] = new SqlParameter("@spare", _spare);
            arParams[11] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[12] = new SqlParameter("@operatingsystemgroupid", _operatingsystemgroupid);
            arParams[13] = new SqlParameter("@createdby", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addBlade", arParams);
        }

        //public void UpdateBlade(int _assetid, string _name, int _status, int _userid, DateTime _datestamp, int _enclosureid, int _classid, int _environmentid, string _ilo, string _dummy_name, string _macaddress, int _vlan, int _build_network_id, int _slot, int _spare, int _resiliencyid, int _operatingsystemgroupid)
        //{
        //    AddStatus(_assetid, _name, _status, _userid, _datestamp);
        //    arParams = new SqlParameter[14];
        //    arParams[0] = new SqlParameter("@assetid", _assetid);
        //    arParams[1] = new SqlParameter("@enclosureid", _enclosureid);
        //    arParams[2] = new SqlParameter("@classid", _classid);
        //    arParams[3] = new SqlParameter("@environmentid", _environmentid);
        //    arParams[4] = new SqlParameter("@ilo", _ilo);
        //    arParams[5] = new SqlParameter("@dummy_name", _dummy_name);
        //    arParams[6] = new SqlParameter("@macaddress", _macaddress);
        //    arParams[7] = new SqlParameter("@vlan", _vlan);
        //    arParams[8] = new SqlParameter("@build_network_id", _build_network_id);
        //    arParams[9] = new SqlParameter("@slot", _slot);
        //    arParams[10] = new SqlParameter("@spare", _spare);
        //    arParams[11] = new SqlParameter("@resiliencyid", _resiliencyid);
        //    arParams[12] = new SqlParameter("@operatingsystemgroupid", _operatingsystemgroupid);
        //    arParams[13] = new SqlParameter("@modifiedby", _userid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateBlade", arParams);
        //}
        ////Update Blade Info and Staus
        //public void UpdateBladeInfoOnly(int _assetid, string _name, int _status, int _userid, DateTime _datestamp, int _enclosureid, int _classid, int _environmentid, string _ilo, string _dummy_name, string _macaddress, int _vlan, int _build_network_id, int _slot, int _spare, int _resiliencyid, int _operatingsystemgroupid)
        //{
        //    //UpdateStatus(_assetid, _name, _status, _userid, _datestamp);
        //    arParams = new SqlParameter[14];
        //    arParams[0] = new SqlParameter("@assetid", _assetid);
        //    arParams[1] = new SqlParameter("@enclosureid", _enclosureid);
        //    arParams[2] = new SqlParameter("@classid", _classid);
        //    arParams[3] = new SqlParameter("@environmentid", _environmentid);
        //    arParams[4] = new SqlParameter("@ilo", _ilo);
        //    arParams[5] = new SqlParameter("@dummy_name", _dummy_name);
        //    arParams[6] = new SqlParameter("@macaddress", _macaddress);
        //    arParams[7] = new SqlParameter("@vlan", _vlan);
        //    arParams[8] = new SqlParameter("@build_network_id", _build_network_id);
        //    arParams[9] = new SqlParameter("@slot", _slot);
        //    arParams[10] = new SqlParameter("@spare", _spare);
        //    arParams[11] = new SqlParameter("@resiliencyid", _resiliencyid);
        //    arParams[12] = new SqlParameter("@operatingsystemgroupid", _operatingsystemgroupid);
        //    arParams[13] = new SqlParameter("@modifiedby", _userid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateBlade", arParams);
        //}
        public void UpdateBlade(int _assetid, string _name, int _status, int _userid, DateTime _datestamp, int _enclosureid, int _classid, int _environmentid, string _ilo, string _dummy_name, string _macaddress, int _vlan, int _build_network_id, int _slot, int _spare, int _resiliencyid, int _operatingsystemgroupid)
        {
            AddStatus(_assetid, _name, _status, _userid, _datestamp);
            arParams = new SqlParameter[14];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@enclosureid", _enclosureid);
            arParams[2] = new SqlParameter("@classid", _classid);
            arParams[3] = new SqlParameter("@environmentid", _environmentid);
            arParams[4] = new SqlParameter("@ilo", _ilo);
            arParams[5] = new SqlParameter("@dummy_name", _dummy_name);
            arParams[6] = new SqlParameter("@macaddress", _macaddress);
            arParams[7] = new SqlParameter("@vlan", _vlan);
            arParams[8] = new SqlParameter("@build_network_id", _build_network_id);
            arParams[9] = new SqlParameter("@slot", _slot);
            arParams[10] = new SqlParameter("@spare", _spare);
            arParams[11] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[12] = new SqlParameter("@operatingsystemgroupid", _operatingsystemgroupid);
            arParams[13] = new SqlParameter("@modifiedby", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateBlade", arParams);
        }
        //Update Blade Info and Staus
        public void UpdateBladeInfoOnly(int _assetid, string _name, int _status, int _userid, DateTime _datestamp, int _enclosureid, int _classid, int _environmentid, string _ilo, string _dummy_name, string _macaddress, int _vlan, int _build_network_id, int _slot, int _spare, int _resiliencyid, int _operatingsystemgroupid)
        {
            //UpdateStatus(_assetid, _name, _status, _userid, _datestamp);
            arParams = new SqlParameter[14];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@enclosureid", _enclosureid);
            arParams[2] = new SqlParameter("@classid", _classid);
            arParams[3] = new SqlParameter("@environmentid", _environmentid);
            arParams[4] = new SqlParameter("@ilo", _ilo);
            arParams[5] = new SqlParameter("@dummy_name", _dummy_name);
            arParams[6] = new SqlParameter("@macaddress", _macaddress);
            arParams[7] = new SqlParameter("@vlan", _vlan);
            arParams[8] = new SqlParameter("@build_network_id", _build_network_id);
            arParams[9] = new SqlParameter("@slot", _slot);
            arParams[10] = new SqlParameter("@spare", _spare);
            arParams[11] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[12] = new SqlParameter("@operatingsystemgroupid", _operatingsystemgroupid);
            arParams[13] = new SqlParameter("@modifiedby", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateBlade", arParams);
        }

        public void DeleteBlade(int _assetid, bool _redeploy, int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteBlade", arParams);
            if (_redeploy == true)
                AddStatus(_assetid, GetStatus(_assetid, "name"), (int)AssetStatus.Arrived, _userid, DateTime.Now);
        }
        public int GetBladeFullHeight(int _assetid)
        {
            int intSlot = Int32.Parse(GetServerOrBlade(_assetid, "slot"));
            if (intSlot < 9)
                return intSlot + 8;
            else
                return intSlot - 8;
        }
        public string GetBladeBody(int _assetid, int _environment)
        {
            Variables oVariable = new Variables(_environment);
            string strBody = "";
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
            //if (_assetid > 0)
            //{
            //    DataSet ds = Get(_id);
            //    strBody += "<tr><td nowrap><b>Project Name:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + dsProject.Tables[0].Rows[0]["name"].ToString() + "</td></tr>";
            //    strBody += strSpacerRow;
            //    if (dsProject.Tables[0].Rows[0]["number"].ToString() != "")
            //    {
            //        strBody += "<tr><td nowrap><b>Project Number:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + dsProject.Tables[0].Rows[0]["number"].ToString();
            //        if (_highlight == true)
            //            strBody += "&nbsp;&nbsp;&nbsp;<span style=\"color:#990000; padding:3px; background-color:#FFFFCC\"><b>Note:</b> Please retain this project number for future reference.</span>";
            //        strBody += "</td></tr>";
            //        strBody += strSpacerRow;
            //    }
            //    strBody += "<tr><td nowrap><b>Initiative Type:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + dsProject.Tables[0].Rows[0]["bd"].ToString() + "</td></tr>";
            //    strBody += strSpacerRow;
            //    strBody += "<tr><td nowrap><b>Organization:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oOrganization.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["organization"].ToString())) + "</td></tr>";
            //    strBody += strSpacerRow;
            //    string strSegment = oSegment.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["segmentid"].ToString()));
            //    if (strSegment != "")
            //    {
            //        strBody += "<tr><td nowrap><b>Segment:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oSegment.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["segmentid"].ToString())) + "</td></tr>";
            //        strBody += strSpacerRow;
            //    }
            //    strBody += "<tr><td nowrap><b>Created  By:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oUser.GetFullName(Int32.Parse(dsProject.Tables[0].Rows[0]["userid"].ToString())) + "</td></tr>";
            //    strBody += strSpacerRow;
            //    strBody += "<tr><td nowrap><b>Created On:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + DateTime.Parse(dsProject.Tables[0].Rows[0]["modified"].ToString()).ToLongDateString() + "</td></tr>";
            //    strBody += strSpacerRow;
            //    strBody += "<tr><td nowrap><b>Status:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oStatusLevel.HTML(Int32.Parse(dsProject.Tables[0].Rows[0]["status"].ToString())) + "</td></tr>";
            //}
            if (strBody != "")
                strBody = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strBody + "</table>";
            return strBody;
        }
        #endregion

        // HBAs
        #region HBA
        public void AddHBA(int _assetid, string _name)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@name", _name);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addHBA", arParams);
        }
        public DataSet GetHBA(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getHBAs", arParams);
        }
        public void DeleteHBA(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteHBA", arParams);
        }
        #endregion

        // RESERVATIONS
        #region RESERVATIONS
        public void AddReservation(int _buildid, int _reserveid, int _removable)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@buildid", _buildid);
            arParams[1] = new SqlParameter("@reserveid", _reserveid);
            arParams[2] = new SqlParameter("@removable", _removable);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addReservation", arParams);
        }
        public void DeleteReservation(int _buildid, int _reserveid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@buildid", _buildid);
            arParams[1] = new SqlParameter("@reserveid", _reserveid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteReservation", arParams);
        }
        public DataSet GetReservations(int _buildid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@buildid", _buildid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getReservations", arParams);
        }
        #endregion

        // VSG Numbers
        #region VSG
        public void AddVSG(string _name, string _type)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@type", _type);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVSG", arParams);
        }
        public string GetVSG(string _type)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@type", _type);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getVSG", arParams);
            if (o == null)
                return "";
            else
                return o.ToString();
        }
        public string GetVSG(string _name, string _type)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@type", _type);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVSGName", arParams);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0]["name"].ToString();
            else
                return "";
        }
        public DataSet UpdateVSG(string _name, string _assignedto, string _type)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@assignedto", _assignedto);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVSG", arParams);

            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@type", _type);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVSGs", arParams);
        }
        #endregion

        // DECOMMISSIONS
        #region DECOMMISSIONS
        public bool AddDecommission(int _requestid, int _itemid, int _number, int _assetid, int _userid, string _reason, DateTime _decom, string _name, int _dr, string _poweroff_new)
        {
            DataSet dsDecom = GetDecommission(_assetid, _name, true);
            if (dsDecom.Tables[0].Rows.Count > 0)
            {
                arParams = new SqlParameter[5];
                arParams[0] = new SqlParameter("@requestid", _requestid);
                arParams[1] = new SqlParameter("@itemid", _itemid);
                arParams[2] = new SqlParameter("@number", _number);
                arParams[3] = new SqlParameter("@assetid", _assetid);
                arParams[4] = new SqlParameter("@decom", (_poweroff_new == "" ? _decom : DateTime.Parse(_poweroff_new)));
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDecommissionEdit", arParams);
                return false;
            }
            else
            {
                // Delete all previous decommission requests (that might have been recommissioned)
                DeleteDecommission(_assetid);
                // Add new decommission request
                arParams = new SqlParameter[9];
                arParams[0] = new SqlParameter("@requestid", _requestid);
                arParams[1] = new SqlParameter("@itemid", _itemid);
                arParams[2] = new SqlParameter("@number", _number);
                arParams[3] = new SqlParameter("@assetid", _assetid);
                arParams[4] = new SqlParameter("@userid", _userid);
                arParams[5] = new SqlParameter("@reason", _reason);
                arParams[6] = new SqlParameter("@decom", _decom);
                arParams[7] = new SqlParameter("@name", _name);
                arParams[8] = new SqlParameter("@dr", _dr);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDecommission", arParams);
                return true;
            }
        }
        public void UpdateDecommission(int _requestid, int _itemid, int _number, int _active)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@active", _active);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDecommissionActive", arParams);
        }
        public void UpdateDecommission(int _requestid, int _itemid, int _number, string _decom)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@decom", (_decom == "" ? SqlDateTime.Null : DateTime.Parse(_decom)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDecommissionDate", arParams);
        }
        public void UpdateDecommissionRunning(int _assetid, int _running)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@running", _running);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDecommissionRunning", arParams);
        }
        public void UpdateDecommissionServiceNowOperationalStatus(string _name, int _service_now_operational_status)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@service_now_operational_status", _service_now_operational_status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDecommissionServiceNowOperationalStatus", arParams);
        }
        public void UpdateDecommissionConfirmed(string _name, int _confirmed_by)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@confirmed_by", _confirmed_by);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDecommissionConfirmed", arParams);
        }
        public DataSet GetDecommissionRecommission(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDecommissionRecommission", arParams);
        }
        public DataSet GetDecommissionBypass(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDecommissionBypass", arParams);
        }
        public void UpdateDecommissionRecommission(int _assetid, int _recommissioned_by, string _recommissioned_reason)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@recommissioned_by", _recommissioned_by);
            arParams[2] = new SqlParameter("@recommissioned_reason", _recommissioned_reason);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDecommissionRecommission", arParams);
        }
        public void UpdateDecommissionBypass(int _assetid, string _destroy, int _bypassed_by, string _bypassed_reason, string _bypassed_ptm)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@destroy", (_destroy == "" ? SqlDateTime.Null : DateTime.Parse(_destroy)));
            arParams[2] = new SqlParameter("@bypassed_by", _bypassed_by);
            arParams[3] = new SqlParameter("@bypassed_reason", _bypassed_reason);
            arParams[4] = new SqlParameter("@bypassed_ptm", _bypassed_ptm);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDecommissionBypass", arParams);
        }
        public void UpdateDecommissionFixed(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDecommissionFixed", arParams);
        }
        public void UpdateDecommissionFixed(int _requestid, int _number)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDecommissionFixedRR", arParams);
        }
        public DataSet GetDecommissions(string _types, DateTime _decom)
        {
            Functions oFunction = new Functions(0, dsn, 0);
            string strServerTypes = oFunction.BuildXmlStringType(_types);
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@types", strServerTypes);
            arParams[1] = new SqlParameter("@decom", _decom);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDecommissions", arParams);
        }
        public DataSet GetDecommission(int _requestid, int _number, int _running)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@number", _number);
            arParams[2] = new SqlParameter("@running", _running);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDecommissionRR", arParams);
        }
        public DataSet GetDecommission(int _assetid, string _name, bool _decommed_or_future)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@decommed_or_future", (_decommed_or_future ? 1 : 0));
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDecommission", arParams);
        }
        public DataSet GetDecommission(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDecommissionName", arParams);
        }
        public void UpdateDecommission(int _assetid, DateTime _destroy, int _vmware, string _name)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@destroy", _destroy);
            arParams[2] = new SqlParameter("@vmware", _vmware);
            arParams[3] = new SqlParameter("@name", _name);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDecommission", arParams);
        }
        public DataSet GetDecommissionDestroys(string _types, DateTime _destroy)
        {
            Functions oFunction = new Functions(0, dsn, 0);
            string strServerTypes = oFunction.BuildXmlStringType(_types);
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@types", strServerTypes);
            arParams[1] = new SqlParameter("@destroy", _destroy);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDecommissionDestroys", arParams);
        }
        public void UpdateDecommissionDestroy(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDecommissionDestroy", arParams);
        }
        public void DeleteDecommission(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDecommission", arParams);
        }
        public DataSet GetDecommissionErrors(int _serviceid_decom, int _serviceid_destroy)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serviceid_decom", _serviceid_decom);
            arParams[1] = new SqlParameter("@serviceid_destroy", _serviceid_destroy);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDecommissionErrors", arParams);
        }


        public void AddDecommissionWarning(string _emails, int _days, string _servername, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@emails", _emails);
            arParams[1] = new SqlParameter("@days", _days);
            arParams[2] = new SqlParameter("@servername", _servername);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDecommissionWarning", arParams);
        }
        public void UpdateDecommissionWarning(int _id, string _emails, int _days, string _servername, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@emails", _emails);
            arParams[2] = new SqlParameter("@days", _days);
            arParams[3] = new SqlParameter("@servername", _servername);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDecommissionWarning", arParams);
        }
        public void EnableDecommissionWarning(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDecommissionWarningEnabled", arParams);
        }
        public void DeleteDecommissionWarning(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDecommissionWarning", arParams);
        }
        public DataSet GetDecommissionWarning(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDecommissionWarning", arParams);
        }
        public string GetDecommissionWarning(int _id, string _column)
        {
            DataSet ds = GetDecommissionWarning(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetDecommissionWarnings(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDecommissionWarnings", arParams);
        }


        public void AddDecommissionWarningSent(int _decommissionid, int _warningid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@decommissionid", _decommissionid);
            arParams[1] = new SqlParameter("@warningid", _warningid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDecommissionWarningSent", arParams);
        }
        public DataSet GetDecommissionWarnings()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDecommissionsWarnings");
        }
        #endregion

        public string GetDRBody(int intAsset, int intAssetDR, string strHeader, string strHeaderDR, int intEnvironment)
        {
            Variables oVariable = new Variables(intEnvironment);
            string strAsset = GetDRBody(intAsset, strHeader, intEnvironment);
            string strAssetDR = GetDRBody(intAssetDR, strHeaderDR, intEnvironment);
            string strBody = "";
            if (strAsset != "")
                strAsset = "<table border=\"0\" cellpadding=\"3\" cellspacing=\"2\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strAsset + "</table>";
            if (strAssetDR != "")
                strAssetDR = "<table border=\"0\" cellpadding=\"3\" cellspacing=\"2\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strAssetDR + "</table>";
            if (strAsset != "" && strAssetDR != "")
                strBody = "<table border=\"0\" cellpadding=\"2\" cellspacing=\"1\" style=\"" + oVariable.DefaultFontStyle() + "\"><tr><td>" + strAsset + "</td><td>" + strAssetDR + "</tr></table>";
            return strBody;
        }
        private string GetDRBody(int intAsset, string strHeader, int intEnvironment)
        {
            Variables oVariable = new Variables(intEnvironment);
            StringBuilder sbBody = new StringBuilder();
            DataSet dsAsset = GetServerOrBlade(intAsset);
            if (dsAsset.Tables[0].Rows.Count > 0)
            {
                if (strHeader != "")
                {
                    sbBody.Append("<tr><td colspan=\"2\"><span style=\"");
                    sbBody.Append(oVariable.DefaultFontStyleHeader());
                    sbBody.Append("\">");
                    sbBody.Append(strHeader);
                    sbBody.Append("</span></td></tr>");
                }
                sbBody.Append("<tr><td nowrap><b>Make:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(dsAsset.Tables[0].Rows[0]["make"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append("<tr><td nowrap><b>Model:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(dsAsset.Tables[0].Rows[0]["model"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append("<tr><td nowrap><b>Serial Number:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(dsAsset.Tables[0].Rows[0]["serial"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append("<tr><td nowrap><b>Asset Tag:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(dsAsset.Tables[0].Rows[0]["serial"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append("<tr><td nowrap><b>Device Name:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(dsAsset.Tables[0].Rows[0]["name"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append("<tr><td nowrap><b>Remote Management:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(dsAsset.Tables[0].Rows[0]["ilo"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append("<tr><td nowrap><b>Class:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(dsAsset.Tables[0].Rows[0]["class"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append("<tr><td nowrap><b>Environment:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(dsAsset.Tables[0].Rows[0]["environment"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append("<tr><td nowrap><b>Location:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(dsAsset.Tables[0].Rows[0]["location"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append("<tr><td nowrap><b>Room:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(dsAsset.Tables[0].Rows[0]["room"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append("<tr><td nowrap><b>Rack:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(dsAsset.Tables[0].Rows[0]["rack"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append("<tr><td nowrap><b>Rack Position:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(dsAsset.Tables[0].Rows[0]["rackposition"].ToString());
                sbBody.Append("</td></tr>");
                int intEnclosure = Int32.Parse(dsAsset.Tables[0].Rows[0]["enclosureid"].ToString());
                if (intEnclosure > 0)
                {
                    sbBody.Append("<tr><td nowrap><b>Enclosure:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(GetEnclosure(intEnclosure, "name"));
                    sbBody.Append("</td></tr>");
                    sbBody.Append("<tr><td nowrap><b>Bay #:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(dsAsset.Tables[0].Rows[0]["slot"].ToString() + "</td></tr>");
                }
            }
            return sbBody.ToString();
        }

        public DataSet GetAssetsSerial(string strSerial)
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cva_assets WHERE serial LIKE '" + strSerial + "%' AND deleted = 0");
        }

        public DataSet GetCVAStatus(int intAsset)
        {
            string strSQL = "";
            strSQL = strSQL + "SELECT cva_status.*, cvStatusList.StatusDescription AS statusname FROM cva_status ";
            strSQL=strSQL+ " INNER JOIN ClearView.dbo.cv_StatusList cvStatusList ";
            strSQL=strSQL+ " 	ON  cva_status.status = cvStatusList.StatusValue ";
            strSQL=strSQL+ " AND cvStatusList.StatusKey='ASSETSTATUS' AND cvStatusList.deleted = 0 ";
            strSQL=strSQL+ " WHERE assetid = " + intAsset.ToString() + " ORDER BY cva_status.datestamp";

            return SqlHelper.ExecuteDataset(dsn, CommandType.Text, strSQL);
        }

        //public DataSet GetAssetServers(string _name)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@name", _name);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAssetServers", arParams);
        //}

        public DataSet GetProvisioningHistory(string assetHistorySelect)
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.Text, assetHistorySelect);
        }

        public DataSet GetProvisioningHistory(int intAsset)
        {
            string strSQL="";
            strSQL=strSQL+" SELECT cva_status.*,  cvStatusList.StatusDescription AS statusname FROM cva_status ";
            strSQL=strSQL + " INNER JOIN ClearView.dbo.cv_StatusList cvStatusList ";
            strSQL=strSQL+" ON  cva_status.status = cvStatusList.StatusValue ";
            strSQL=strSQL+" AND cvStatusList.StatusKey='ASSETSTATUS' AND cvStatusList.deleted = 0 ";
            strSQL=strSQL+" WHERE assetid = " + intAsset.ToString() ;


            return SqlHelper.ExecuteDataset(dsn, CommandType.Text, strSQL);
        }

        #region Switches
        public void AddSwitch(int _assetid, int _classid, int _environmentid, int _rackid, string _rackposition, int _blades, int _ports, int _ipaddressid, int _is_ios, int _nexus, int _enabled)
        {
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@rackid", _rackid);
            arParams[4] = new SqlParameter("@rackposition", _rackposition);
            arParams[5] = new SqlParameter("@blades", _blades);
            arParams[6] = new SqlParameter("@ports", _ports);
            arParams[7] = new SqlParameter("@ipaddressid", _ipaddressid);
            arParams[8] = new SqlParameter("@is_ios", _is_ios);
            arParams[9] = new SqlParameter("@nexus", _nexus);
            arParams[10] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSwitch", arParams);
        }
        public void UpdateSwitch(int _assetid, int _classid, int _environmentid, int _rackid, string _rackposition, int _blades, int _ports, int _ipaddressid, int _is_ios, int _nexus, int _enabled)
        {
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@rackid", _rackid);
            arParams[4] = new SqlParameter("@rackposition", _rackposition);
            arParams[5] = new SqlParameter("@blades", _blades);
            arParams[6] = new SqlParameter("@ports", _ports);
            arParams[7] = new SqlParameter("@ipaddressid", _ipaddressid);
            arParams[8] = new SqlParameter("@is_ios", _is_ios);
            arParams[9] = new SqlParameter("@nexus", _nexus);
            arParams[10] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSwitch", arParams);
        }
        public void DeleteSwitch(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteSwitch", arParams);
        }
        public DataSet GetSwitch(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSwitch", arParams);
        }

        public string GetSwitch(int _assetid, string _column)
        {
            DataSet ds = GetSwitch(_assetid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetSwitchsByRack(int _rackid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@rackid", _rackid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSwitchsByRack", arParams);
        }
        public DataSet GetSwitchsByLocation(int _locationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@locationid", _locationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSwitchsByLocation", arParams);
        }
        /*
        public DataSet GetSwitchs(int _classid, int _environmentid, int _addressid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSwitchs", arParams);
        }
        */
        public DataSet GetSwitchports(int _assetid, SwitchPortType _type)
        {
            return GetSwitchports(_assetid, _type, 0);
        }
        public DataSet GetSwitchports(int _assetid, SwitchPortType _type, int _nic)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@type", _type);
            arParams[2] = new SqlParameter("@nic", _nic);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSwitchports", arParams);
        }
        public void AddSwitchport(int _switchid, int _assetid, SwitchPortType _type, int _nic, string _blade, int _port, string _interface)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@switchid", _switchid);
            arParams[1] = new SqlParameter("@assetid", _assetid);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@nic", _nic);
            arParams[4] = new SqlParameter("@blade", _blade);
            arParams[5] = new SqlParameter("@port", _port);
            arParams[6] = new SqlParameter("@interface", _interface);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSwitchport", arParams);
        }
        public void UpdateSwitchport(int _switchid, int _assetid, SwitchPortType _type, int _nic, int _vlan)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@switchid", _switchid);
            arParams[1] = new SqlParameter("@assetid", _assetid);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@nic", _nic);
            arParams[4] = new SqlParameter("@vlan", _vlan);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSwitchport", arParams);
        }
        public void DeleteSwitchport(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteSwitchport", arParams);
        }
        #endregion

        #region Racks
        public DataSet GetRacks(int _modelid, int _status)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@ModelId", _modelid);
            arParams[1] = new SqlParameter("@Status", _status);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRackDetails", arParams);
        }

        public DataSet GetRackByAsset(int _AssetId)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@AssetId", _AssetId);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRackDetails", arParams);
        }

        #endregion

        public DataSet GetAssetsAll(int intModelId, int intStatus)
        {

            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@ModelId", intModelId);
            arParams[1] = new SqlParameter("@AssetStatusId", intStatus);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAssetAll", arParams);
        }

        public DataSet GetAssetsAll(int intAssetId)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@AssetId", intAssetId);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAssetAll", arParams);
        }



        #region Asset Attributes


        public void addAssetAttributes(string _name, int _userId, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@Name", _name);
            arParams[1] = new SqlParameter("@CreatedBy", _userId);
            arParams[2] = new SqlParameter("@Enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAssetAttributes", arParams);
        }

        public void updateAssetAttributes(int _attributeId, int _name, int _userId, int _enabled, int _deleted)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@AttributeId", _attributeId);
            arParams[1] = new SqlParameter("@Name", _name);
            arParams[2] = new SqlParameter("@ModifiedBy", _userId);
            arParams[3] = new SqlParameter("@Enabled", _enabled);
            arParams[4] = new SqlParameter("@Deleted", _deleted);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAssetAttributes", arParams);
        }

        public DataSet getAssetAttributes(int? _attributeId, string _name, int? _enabled)
        {
            arParams = new SqlParameter[5];
            if (_attributeId!=null)
                arParams[0] = new SqlParameter("@AttributeId", _attributeId);
            if (_name != "")
                arParams[1] = new SqlParameter("@Name", _name);
            if (_enabled != null)
                arParams[2] = new SqlParameter("@Enabled", _enabled);
            
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAssetAttributes", arParams);
        }

        public void addAssetAttributesForAsset(int _assetId, int _assetAttributeId, string _comments, int _userId)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@AssetId", _assetId);
            arParams[1] = new SqlParameter("@AssetAttributeId", _assetAttributeId);
            arParams[3] = new SqlParameter("@Comments", _comments);
            arParams[4] = new SqlParameter("@CreatedBy", _userId);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAssetAttributesForAsset", arParams);
        }

        public string getAssetAttributesComments(int _assetId)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@AssetId", _assetId);

            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAssetAttributesForAsset", arParams);

            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0]["Comments"].ToString();
            else
                return "";

        }

        #endregion


        public DataSet GetDellBladeSwitchports(string _enclosure, int _slot)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@enclosure", _enclosure);
            arParams[1] = new SqlParameter("@slot", _slot);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDellBladeSwitchport", arParams);
        }
        public void AddDellBladeSwitchport(string _enclosure, int _slot, string _switchA, string _interfaceA, string _switchB, string _interfaceB)
        {
            // Delete Old
            DeleteDellBladeSwitchports(_enclosure, _slot);
            // Add New
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@enclosure", _enclosure);
            arParams[1] = new SqlParameter("@slot", _slot);
            arParams[2] = new SqlParameter("@switchA", _switchA);
            arParams[3] = new SqlParameter("@interfaceA", _interfaceA);
            arParams[4] = new SqlParameter("@switchB", _switchB);
            arParams[5] = new SqlParameter("@interfaceB", _interfaceB);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDellBladeSwitchport", arParams);
        }
        public void DeleteDellBladeSwitchports(string _enclosure, int _slot)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@enclosure", _enclosure);
            arParams[1] = new SqlParameter("@slot", _slot);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDellBladeSwitchport", arParams);
        }

        public SshShell LoginDellSwitchport(string strHost, string strUser, string strPass)
        {
            SshShell oSSHshell = new SshShell(strHost, strUser, strPass);
            oSSHshell.RemoveTerminalEmulationCharacters = true;
            oSSHshell.Connect();
            string strLogin = GetDellSwitchportOutput(oSSHshell);
            if (strLogin == "**INVALID**")
            {
                // Invalid Username / Password
                return null;
            }
            else
                return oSSHshell;
        }

        public string GetDellSwitchportOutput(SshShell oSSHshell)
        {
            string strRead = oSSHshell.Expect("#");
            if (strRead != "")
            {
                if (strRead.ToUpper().Contains("LOGIN INCORRECT") || strRead.ToUpper().Contains("ACCESS DENIED"))
                {
                    // Set return value to "**INVALID**" and return
                    strRead = "**INVALID**";
                }
            }
            return strRead;
        }
        public string GetDellSwitchportOutputWait(SshShell oSSHshell)
        {
            string strRead = oSSHshell.Expect("#] 100%");   // Finished writing
            strRead += oSSHshell.Expect("#");               // Back to normal
            return strRead;
        }
        public string WriteDellSwitchport(SshShell oSSHshell, string _command, int _pounds)
        {
            Log oLog = new Log(0, dsnCV);
            oSSHshell.WriteLine(_command);
            string strReturn = GetDellSwitchportOutput(oSSHshell);
            oLog.AddEvent("", "", "Command " + _command + " returned " + strReturn, LoggingType.Debug);
            for (int ii = 1; ii < _pounds; ii++)
            {
                strReturn += GetDellSwitchportOutput(oSSHshell);
                oLog.AddEvent("", "", "Command " + _command + " returned " + strReturn, LoggingType.Debug);
            }
            return strReturn;
        }
        public string GetDellSwitchportOutput(SshShell oSSHshell, string _interface, DellBladeSwitchportType _type, int _assetid)
        {
            Log oLog = new Log(0, dsnCV);
            string _name = GetStatus(_assetid, "name");
            string _serial = Get(_assetid, "serial");
            string strReturn = "";

            if (_type == DellBladeSwitchportType.VLANs || _type == DellBladeSwitchportType.Description || _type == DellBladeSwitchportType.Config)
            {
                // show run int eth 101/1/32
                System.Threading.Thread.Sleep(3000);    // Wait 3 seconds
                oLog.AddEvent(_name, _serial, "Running command..." + "show run int eth " + _interface, LoggingType.Debug);
                string strOutput = WriteDellSwitchport(oSSHshell, "show run int eth " + _interface, 1);
                oLog.AddEvent(_name, _serial, "Output = " + strOutput, LoggingType.Debug);
                if (strOutput.ToUpper().Contains("ETHERNET" + _interface) == true)
                {
                    // Get from start of output
                    strOutput = strOutput.Substring(strOutput.ToUpper().LastIndexOf("ETHERNET" + _interface));
                    // Only get first line of output
                    oLog.AddEvent(_name, _serial, "Output = " + strOutput, LoggingType.Debug);
                    if (strOutput.Contains(Environment.NewLine + Environment.NewLine) == true)
                        strOutput = strOutput.Substring(0, strOutput.IndexOf(Environment.NewLine + Environment.NewLine));
                    oLog.AddEvent(_name, _serial, "Output = " + strOutput, LoggingType.Debug);
                    // Split into array (by the single spaces)
                    string[] strOutputs = Regex.Split(strOutput, Environment.NewLine);
                    // Replace all extra spaces
                    while (strOutput.Contains("  ") == true)
                        strOutput = strOutput.Replace("  ", " ");
                    // Get the value
                    switch (_type)
                    {
                        case DellBladeSwitchportType.Config:
                            strReturn = strOutput;
                            break;
                        case DellBladeSwitchportType.VLANs:
                            for (int ii = 0; ii < strOutputs.Length; ii++)
                            {
                                string strLine = strOutputs[ii].Trim();
                                string strTrunk = "SWITCHPORT TRUNK ALLOWED VLAN";
                                string strAccess = "SWITCHPORT ACCESS VLAN";
                                if (strLine.StartsWith(strTrunk))
                                {
                                    strReturn = strLine.Substring(strTrunk.Length + 1).Trim();
                                    break;
                                }
                                else if (strLine.StartsWith(strAccess))
                                {
                                    strReturn = strLine.Substring(strAccess.Length + 1).Trim();
                                    break;
                                }
                            }
                            break;
                        case DellBladeSwitchportType.Description:
                            for (int ii = 0; ii < strOutputs.Length; ii++)
                            {
                                string strLine = strOutputs[ii].Trim();
                                string strDescription = "DESCRIPTION";
                                if (strLine.StartsWith(strDescription))
                                {
                                    strReturn = strLine.Substring(strDescription.Length + 1).Trim();
                                    break;
                                }
                            }
                            break;
                    }
                }
            }
            else
            {
                // show int eth 101/1/32 br
                System.Threading.Thread.Sleep(3000);    // Wait 3 seconds
                oLog.AddEvent(_name, _serial, "Running command..." + "show int eth " + _interface + " br", LoggingType.Debug);
                string strOutput = WriteDellSwitchport(oSSHshell, "show int eth " + _interface + " br", 2);
                oLog.AddEvent(_name, _serial, "Output = " + strOutput, LoggingType.Debug);
                if (strOutput.ToUpper().Contains("ETH" + _interface) == true)
                {
                    // Get from start of output
                    strOutput = strOutput.Substring(strOutput.ToUpper().IndexOf("ETH" + _interface));
                    oLog.AddEvent(_name, _serial, "Output = " + strOutput, LoggingType.Debug);
                    // Only get first line of output
                    if (strOutput.Contains(Environment.NewLine) == true)
                        strOutput = strOutput.Substring(0, strOutput.IndexOf(Environment.NewLine));
                    oLog.AddEvent(_name, _serial, "Output = " + strOutput, LoggingType.Debug);
                    // Replace all extra spaces
                    while (strOutput.Contains("  ") == true)
                        strOutput = strOutput.Replace("  ", " ");
                    // Split into array (by the single spaces)
                    string[] strOutputs = strOutput.Split(new char[] { ' ' });
                    // Get the value
                    switch (_type)
                    {
                        case DellBladeSwitchportType.VLAN:
                            strReturn = strOutputs[1];
                            break;
                        case DellBladeSwitchportType.Mode:
                            strReturn = strOutputs[3];
                            break;
                        case DellBladeSwitchportType.Status:
                            strReturn = strOutputs[4];
                            break;
                    }
                }
            }
            return strReturn;
        }
        public string ChangeDellSwitchport(SshShell oSSHshell, string _interface, DellBladeSwitchportMode _mode, string[] _vlans, string _native_vlan, string _description, bool _override_connected, int _assetid)
        {
            string strTrunkVLANs = "";
            for (int ii = 0; ii < _vlans.Length; ii++)
            {
                if (strTrunkVLANs != "")
                    strTrunkVLANs += ",";
                strTrunkVLANs += _vlans[ii];
            }
            return ChangeDellSwitchport(oSSHshell, _interface, _mode, strTrunkVLANs, _native_vlan, _description, _override_connected, _assetid);
        }
        public string ChangeDellSwitchport(SshShell oSSHshell, string _interface, DellBladeSwitchportMode _mode, string _vlans, string _native_vlan, string _description, bool _override_connected, int _assetid)
        {
            string strReturn = "";
            //if (_description == "")
            //    _description = "ClearView - " + DateTime.Today.ToShortDateString();
            //else
            //    _description = _description + " - " + DateTime.Today.ToShortDateString();

            string strMode = GetDellSwitchportOutput(oSSHshell, _interface, DellBladeSwitchportType.Mode, _assetid).Trim().ToUpper();
            System.Threading.Thread.Sleep(3000);    // Wait 3 seconds
            if (strMode == "")
                strReturn = "There was an unknown error accessing the switch";
            else if (strMode == "ACCESS" || strMode == "TRUNK")
            {
                string strConnected = GetDellSwitchportOutput(oSSHshell, _interface, DellBladeSwitchportType.Status, _assetid).Trim().ToUpper();
                System.Threading.Thread.Sleep(3000);    // Wait 3 seconds
                string strVLANs = GetDellSwitchportOutput(oSSHshell, _interface, DellBladeSwitchportType.VLANs, _assetid);
                System.Threading.Thread.Sleep(3000);    // Wait 3 seconds
                string[] oVlans = _vlans.Split(new char[] { ',' });
                switch (_mode)
                {
                    case DellBladeSwitchportMode.Access:
                        if (oVlans.Length == 1)
                        {
                            // Access mode is enabled when server is going to be provisioned.  It should be DOWN.
                            if (strConnected == "DOWN" || _override_connected == true)
                            {
                                WriteDellSwitchport(oSSHshell, "config t", 1);
                                WriteDellSwitchport(oSSHshell, "interface ethernet " + _interface, 1);
                                WriteDellSwitchport(oSSHshell, "switchport", 1);
                                WriteDellSwitchport(oSSHshell, "switchport mode access", 1);
                                WriteDellSwitchport(oSSHshell, "switchport access vlan " + oVlans[0], 1);
                                if (strMode == "TRUNK")
                                {
                                    WriteDellSwitchport(oSSHshell, "no switchport mode trunk", 1);
                                    WriteDellSwitchport(oSSHshell, "no switchport trunk allowed vlan " + strVLANs, 1);
                                }
                                WriteDellSwitchport(oSSHshell, "description " + _description, 1);
                                WriteDellSwitchport(oSSHshell, "no shutdown", 1);
                                WriteDellSwitchport(oSSHshell, "wr", 1);
                                string strWait = GetDellSwitchportOutputWait(oSSHshell);
                                WriteDellSwitchport(oSSHshell, "exit", 1);
                                WriteDellSwitchport(oSSHshell, "exit", 1);
                            }
                            else
                            {
                                strReturn = "The switchport is currently connected (" + strConnected + ")";
                            }
                        }
                        else
                        {
                            strReturn = "When configuring ACCESS mode, only one VLAN can be specified";
                        }
                        break;
                    case DellBladeSwitchportMode.Trunk:
                        // Trunk mode is enabled when server has been provisioned, and the static address is going to be applied.  It should be DOWN.
                        if (strConnected == "DOWN" || strMode == "ACCESS" || _override_connected == true)
                        {
                            int intOldVLAN = 0;
                            if (strVLANs == "" || Int32.TryParse(strVLANs, out intOldVLAN) == true)
                            {
                                string strTrunkVLANs = "";
                                for (int ii = 0; ii < oVlans.Length; ii++)
                                {
                                    if (strTrunkVLANs != "")
                                        strTrunkVLANs += ",";
                                    strTrunkVLANs += oVlans[ii];
                                }
                                WriteDellSwitchport(oSSHshell, "config t", 1);
                                WriteDellSwitchport(oSSHshell, "interface ethernet " + _interface, 1);
                                WriteDellSwitchport(oSSHshell, "shutdown", 1);
                                if (strVLANs != "")
                                    WriteDellSwitchport(oSSHshell, "no switchport access vlan " + intOldVLAN.ToString(), 1);
                                WriteDellSwitchport(oSSHshell, "switchport mode trunk", 1);
                                WriteDellSwitchport(oSSHshell, "switchport trunk allowed vlan " + strTrunkVLANs, 1);
                                if (_native_vlan == null)
                                    WriteDellSwitchport(oSSHshell, "no switchport trunk native vlan", 1);
                                else if (_native_vlan != "")
                                    WriteDellSwitchport(oSSHshell, "switchport trunk native vlan " + _native_vlan, 1);
                                WriteDellSwitchport(oSSHshell, "description " + _description, 1);
                                WriteDellSwitchport(oSSHshell, "no shutdown", 1);
                                WriteDellSwitchport(oSSHshell, "wr", 1);
                                string strWait = GetDellSwitchportOutputWait(oSSHshell);
                                WriteDellSwitchport(oSSHshell, "exit", 1);
                                WriteDellSwitchport(oSSHshell, "exit", 1);
                            }
                            else
                            {
                                strReturn = "The existing VLAN has more than one value in ACCESS Mode (" + strVLANs + ")";
                            }
                        }
                        else
                        {
                            strReturn = "The switchport is currently connected (" + strConnected + ") or it is not in ACCESS Mode (" + strMode + ")";
                        }
                        break;
                    case DellBladeSwitchportMode.Shutdown:
                        // Shutdown mode is enabled when server has been decommissioned.  It should be DOWN.
                        if (strConnected == "DOWN" || _override_connected == true)
                        {
                            WriteDellSwitchport(oSSHshell, "config t", 1);
                            WriteDellSwitchport(oSSHshell, "interface ethernet " + _interface, 1);
                            if (strMode == "TRUNK")
                            {
                                WriteDellSwitchport(oSSHshell, "no switchport mode trunk", 1);
                                WriteDellSwitchport(oSSHshell, "no switchport trunk allowed vlan " + strVLANs, 1);
                                WriteDellSwitchport(oSSHshell, "no switchport trunk native vlan", 1);
                            }
                            if (strMode == "ACCESS")
                            {
                                WriteDellSwitchport(oSSHshell, "no switchport mode access", 1);
                                WriteDellSwitchport(oSSHshell, "no switchport access vlan " + strVLANs, 1);
                            }
                            WriteDellSwitchport(oSSHshell, "description " + _description, 1);
                            WriteDellSwitchport(oSSHshell, "shutdown", 1);
                            WriteDellSwitchport(oSSHshell, "wr", 1);
                            string strWait = GetDellSwitchportOutputWait(oSSHshell);
                            WriteDellSwitchport(oSSHshell, "exit", 1);
                            WriteDellSwitchport(oSSHshell, "exit", 1);
                        }
                        else
                        {
                            strReturn = "The switchport is currently connected (" + strConnected + ")";
                        }
                        break;
                }
            }
            else
                strReturn = "The mode of the switchport specified is invalid (" + strMode + ")";
            return strReturn;
        }

        //public TelnetConnection LoginDellSwitchport(string strHost, string strUser, string strPass)
        //{
        //    TelnetConnection oTelnet = new TelnetConnection(strHost, intDefaultSwitchPort);
        //    string strLogin = oTelnet.Login(strUser, strPass, intDefaultSwitchTimeout);
        //    strLogin = GetDellSwitchportOutput(oTelnet);
        //    if (strLogin == "**INVALID**")
        //    {
        //        // Invalid Username / Password
        //        return null;
        //    }
        //    else
        //        return oTelnet;
        //}

        //public string GetDellSwitchportOutput(TelnetConnection oTelnet)
        //{
        //    string strReturn = "";
        //    string strRead = "";
        //    bool boolAlready = false;
        //    int intCount = 0;
        //    int intAlready = 0;
        //    while (oTelnet.IsConnected == true && intCount < intDellSwitchMaxLoops)
        //    {
        //        intCount++;
        //        strRead = oTelnet.Read();
        //        if (strRead != "")
        //        {
        //            if (strRead.ToUpper().Contains("LOGIN INCORRECT") || strRead.ToUpper().Contains("ACCESS DENIED"))
        //            {
        //                // Set return value to "**INVALID**" and return
        //                strReturn = "**INVALID**";
        //                break;
        //            }
        //            boolAlready = true;
        //            intAlready = 0;
        //        }
        //        if (strRead == "")
        //        {
        //            intAlready++;
        //            if (boolAlready == true)
        //            {
        //                if (intAlready > 5)
        //                    break;
        //            }
        //        }
        //        strReturn += strRead;
        //    }
        //    return strReturn;
        //}
        //public string GetDellSwitchportOutput(TelnetConnection oTelnet, string _interface, DellBladeSwitchportType _type, int _assetid)
        //{
        //    Log oLog = new Log(0, dsnCV);
        //    string _name = GetStatus(_assetid, "name");
        //    string _serial = Get(_assetid, "serial");
        //    string strReturn = "";

        //    if (_type == DellBladeSwitchportType.VLANs || _type == DellBladeSwitchportType.Description || _type == DellBladeSwitchportType.Config)
        //    {
        //        // show run int eth 101/1/32
        //        oTelnet.WriteLine("show run int eth " + _interface);
        //        oLog.AddEvent(_name, _serial, "Running command..." + "show run int eth " + _interface, LoggingType.Debug);
        //        string strOutput = GetDellSwitchportOutput(oTelnet).ToUpper();
        //        oLog.AddEvent(_name, _serial, "Output = " + strOutput, LoggingType.Debug);
        //        if (strOutput.Contains("ETHERNET" + _interface) == true)
        //        {
        //            // Get from start of output
        //            strOutput = strOutput.Substring(strOutput.LastIndexOf("ETHERNET" + _interface));
        //            // Only get first line of output
        //            oLog.AddEvent(_name, _serial, "Output = " + strOutput, LoggingType.Debug);
        //            if (strOutput.Contains(Environment.NewLine + Environment.NewLine) == true)
        //                strOutput = strOutput.Substring(0, strOutput.IndexOf(Environment.NewLine + Environment.NewLine));
        //            oLog.AddEvent(_name, _serial, "Output = " + strOutput, LoggingType.Debug);
        //            // Split into array (by the single spaces)
        //            string[] strOutputs = Regex.Split(strOutput, Environment.NewLine);
        //            // Replace all extra spaces
        //            while (strOutput.Contains("  ") == true)
        //                strOutput = strOutput.Replace("  ", " ");
        //            // Get the value
        //            switch (_type)
        //            {
        //                case DellBladeSwitchportType.Config:
        //                    strReturn = strOutput;
        //                    break;
        //                case DellBladeSwitchportType.VLANs:
        //                    for (int ii = 0; ii < strOutputs.Length; ii++)
        //                    {
        //                        string strLine = strOutputs[ii].Trim();
        //                        string strTrunk = "SWITCHPORT TRUNK ALLOWED VLAN";
        //                        string strAccess = "SWITCHPORT ACCESS VLAN";
        //                        if (strLine.StartsWith(strTrunk))
        //                        {
        //                            strReturn = strLine.Substring(strTrunk.Length + 1).Trim();
        //                            break;
        //                        }
        //                        else if (strLine.StartsWith(strAccess))
        //                        {
        //                            strReturn = strLine.Substring(strAccess.Length + 1).Trim();
        //                            break;
        //                        }
        //                    }
        //                    break;
        //                case DellBladeSwitchportType.Description:
        //                    for (int ii = 0; ii < strOutputs.Length; ii++)
        //                    {
        //                        string strLine = strOutputs[ii].Trim();
        //                        string strDescription = "DESCRIPTION";
        //                        if (strLine.StartsWith(strDescription))
        //                        {
        //                            strReturn = strLine.Substring(strDescription.Length + 1).Trim();
        //                            break;
        //                        }
        //                    }
        //                    break;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // show int eth 101/1/32 br
        //        oTelnet.WriteLine("show int eth " + _interface + " br");
        //        oLog.AddEvent(_name, _serial, "Running command..." + "show int eth " + _interface + " br", LoggingType.Debug);
        //        string strOutput = GetDellSwitchportOutput(oTelnet).ToUpper();
        //        oLog.AddEvent(_name, _serial, "Output = " + strOutput, LoggingType.Debug);
        //        if (strOutput.Contains("ETH" + _interface) == true)
        //        {
        //            // Get from start of output
        //            strOutput = strOutput.Substring(strOutput.IndexOf("ETH" + _interface));
        //            oLog.AddEvent(_name, _serial, "Output = " + strOutput, LoggingType.Debug);
        //            // Only get first line of output
        //            if (strOutput.Contains(Environment.NewLine) == true)
        //                strOutput = strOutput.Substring(0, strOutput.IndexOf(Environment.NewLine));
        //            oLog.AddEvent(_name, _serial, "Output = " + strOutput, LoggingType.Debug);
        //            // Replace all extra spaces
        //            while (strOutput.Contains("  ") == true)
        //                strOutput = strOutput.Replace("  ", " ");
        //            // Split into array (by the single spaces)
        //            string[] strOutputs = strOutput.Split(new char[] { ' ' });
        //            // Get the value
        //            switch (_type)
        //            {
        //                case DellBladeSwitchportType.VLAN:
        //                    strReturn = strOutputs[1];
        //                    break;
        //                case DellBladeSwitchportType.Mode:
        //                    strReturn = strOutputs[3];
        //                    break;
        //                case DellBladeSwitchportType.Status:
        //                    strReturn = strOutputs[4];
        //                    break;
        //            }
        //        }
        //    }
        //    return strReturn;
        //}
        //public bool GetDellSwitchportConnected(TelnetConnection oTelnet, string _interface, int _assetid)
        //{
        //    return (GetDellSwitchportOutput(oTelnet, _interface, DellBladeSwitchportType.Status, _assetid) == "UP");
        //}
        //public string ChangeDellSwitchport(TelnetConnection oTelnet, string _interface, DellBladeSwitchportMode _mode, string[] _vlans, string _description, bool _override_connected, int _assetid)
        //{
        //    string strTrunkVLANs = "";
        //    for (int ii = 0; ii < _vlans.Length; ii++)
        //    {
        //        if (strTrunkVLANs != "")
        //            strTrunkVLANs += ",";
        //        strTrunkVLANs += _vlans[ii];
        //    }
        //    return ChangeDellSwitchport(oTelnet, _interface, _mode, strTrunkVLANs, _description, _override_connected, _assetid);
        //}
        //public string ChangeDellSwitchport(TelnetConnection oTelnet, string _interface, DellBladeSwitchportMode _mode, string _vlans, string _description, bool _override_connected, int _assetid)
        //{
        //    string strReturn = "";
        //    //if (_description == "")
        //    //    _description = "ClearView - " + DateTime.Today.ToShortDateString();
        //    //else
        //    //    _description = _description + " - " + DateTime.Today.ToShortDateString();

        //    bool boolConnected = GetDellSwitchportConnected(oTelnet, _interface, _assetid);
        //    string strMode = GetDellSwitchportOutput(oTelnet, _interface, DellBladeSwitchportType.Mode, _assetid);
        //    if (strMode == "")
        //        strReturn = "There was an unknown error accessing the switch";
        //    else
        //    {
        //        string strVLANs = GetDellSwitchportOutput(oTelnet, _interface, DellBladeSwitchportType.VLANs, _assetid);
        //        string[] oVlans = _vlans.Split(new char[] { ',' });
        //        switch (_mode)
        //        {
        //            case DellBladeSwitchportMode.Access:
        //                if (oVlans.Length == 1)
        //                {
        //                    // Access mode is enabled when server is going to be provisioned.  It should be DOWN.
        //                    if (boolConnected == false || _override_connected == true)
        //                    {
        //                        oTelnet.WriteLine("config t");
        //                        oTelnet.WriteLine("interface ethernet " + _interface);
        //                        oTelnet.WriteLine("switchport");
        //                        oTelnet.WriteLine("switchport mode access");
        //                        oTelnet.WriteLine("switchport access vlan " + oVlans[0]);
        //                        if (strMode == "TRUNK")
        //                        {
        //                            oTelnet.WriteLine("no switchport mode trunk");
        //                            oTelnet.WriteLine("no switchport trunk allowed vlan " + strVLANs);
        //                        }
        //                        oTelnet.WriteLine("description " + _description);
        //                        oTelnet.WriteLine("no shutdown");
        //                        oTelnet.WriteLine("wr");
        //                        string strWait = GetDellSwitchportOutput(oTelnet);
        //                        oTelnet.WriteLine("exit");
        //                        oTelnet.WriteLine("exit");
        //                    }
        //                    else
        //                    {
        //                        strReturn = "The switchport is currently connected";
        //                    }
        //                }
        //                else
        //                {
        //                    strReturn = "When configuring ACCESS mode, only one VLAN can be specified";
        //                }
        //                break;
        //            case DellBladeSwitchportMode.Trunk:
        //                // Trunk mode is enabled when server has been provisioned, and the static address is going to be applied.  It should be DOWN.
        //                if (boolConnected == false || strMode == "ACCESS" || _override_connected == true)
        //                {
        //                    int intOldVLAN = 0;
        //                    if (strVLANs == "" || Int32.TryParse(strVLANs, out intOldVLAN) == true)
        //                    {
        //                        string strTrunkVLANs = "";
        //                        for (int ii = 0; ii < oVlans.Length; ii++)
        //                        {
        //                            if (strTrunkVLANs != "")
        //                                strTrunkVLANs += ",";
        //                            strTrunkVLANs += oVlans[ii];
        //                        }
        //                        oTelnet.WriteLine("config t");
        //                        oTelnet.WriteLine("interface ethernet " + _interface);
        //                        oTelnet.WriteLine("shutdown");
        //                        if (strVLANs != "")
        //                            oTelnet.WriteLine("no switchport access vlan " + intOldVLAN.ToString());
        //                        oTelnet.WriteLine("switchport mode trunk");
        //                        oTelnet.WriteLine("switchport trunk allowed vlan " + strTrunkVLANs);
        //                        oTelnet.WriteLine("description " + _description);
        //                        oTelnet.WriteLine("no shutdown");
        //                        oTelnet.WriteLine("wr");
        //                        string strWait = GetDellSwitchportOutput(oTelnet);
        //                        oTelnet.WriteLine("exit");
        //                        oTelnet.WriteLine("exit");
        //                    }
        //                    else
        //                    {
        //                        strReturn = "The existing VLAN has more than one value in ACCESS Mode (" + strVLANs + ")";
        //                    }
        //                }
        //                else
        //                {
        //                    strReturn = "The switchport is currently connected or it is not in ACCESS Mode (" + strMode + ")";
        //                }
        //                break;
        //            case DellBladeSwitchportMode.Shutdown:
        //                // Shutdown mode is enabled when server has been decommissioned.  It should be DOWN.
        //                if (boolConnected == false || _override_connected == true)
        //                {
        //                    oTelnet.WriteLine("config t");
        //                    oTelnet.WriteLine("interface ethernet " + _interface);
        //                    if (strMode == "TRUNK")
        //                    {
        //                        oTelnet.WriteLine("no switchport mode trunk");
        //                        oTelnet.WriteLine("no switchport trunk allowed vlan " + strVLANs);
        //                    }
        //                    if (strMode == "ACCESS")
        //                    {
        //                        oTelnet.WriteLine("no switchport mode access");
        //                        oTelnet.WriteLine("no switchport access vlan " + strVLANs);
        //                    }
        //                    oTelnet.WriteLine("description " + _description);
        //                    oTelnet.WriteLine("shutdown");
        //                    oTelnet.WriteLine("wr");
        //                    string strWait = GetDellSwitchportOutput(oTelnet);
        //                    oTelnet.WriteLine("exit");
        //                    oTelnet.WriteLine("exit");
        //                }
        //                else
        //                {
        //                    strReturn = "The switchport is currently connected";
        //                }
        //                break;
        //        }
        //    }
        //    return strReturn;
        //}
    }
}
