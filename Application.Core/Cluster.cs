using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;

namespace NCC.ClearView.Application.Core
{
	public class Cluster
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Cluster(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public int Add(int _requestid, string _nickname, int _nodes, int _dr, int _ha)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@nickname", _nickname);
            arParams[2] = new SqlParameter("@nodes", _nodes);
            arParams[3] = new SqlParameter("@dr", _dr);
            arParams[4] = new SqlParameter("@ha", _ha);
            arParams[5] = new SqlParameter("@id", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addCluster", arParams);
            return Int32.Parse(arParams[5].Value.ToString());
        }
        public void Update(int _id, string _nickname, int _nodes, int _dr, int _ha)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@nickname", _nickname);
            arParams[2] = new SqlParameter("@nodes", _nodes);
            arParams[3] = new SqlParameter("@dr", _dr);
            arParams[4] = new SqlParameter("@ha", _ha);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCluster", arParams);
        }
        public void UpdateName(int _id, string _name)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClusterName", arParams);
        }
        public void UpdateIP(int _id, int _ipaddressid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@ipaddressid", _ipaddressid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClusterIP", arParams);
        }
        public void UpdateNetwork(int _id, int _networkid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@networkid", _networkid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClusterNetwork", arParams);
        }
        public void UpdateServiceAccount(int _id, string _service_account)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@service_account", _service_account);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClusterServiceAccount", arParams);
        }
        public void UpdateVirtualName(int _id, string _virtual_name)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@virtual_name", _virtual_name);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClusterVirtualName", arParams);
        }
        public void UpdateQuorum(int _id, int _quorum)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@quorum", _quorum);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClusterQuorum", arParams);
        }
        public void UpdateLocalNodes(int _id, int _local_nodes)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@local_nodes", _local_nodes);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClusterLocalNodes", arParams);
        }
        public void UpdateNonShared(int _id, int _non_shared)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@non_shared", _non_shared);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClusterNonShared", arParams);
        }
        public void UpdateAddInstance(int _id, int _add_instance)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@add_instance", _add_instance);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClusterAddInstance", arParams);
        }
        public void UpdateSQL(int _id, int _sql)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@sql", _sql);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClusterSQL", arParams);
        }
        public void Delete(int _id, int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteCluster", arParams);
            Storage oStorage = new Storage(user, dsn);
            oStorage.DeleteLuns(_answerid, 0, _id, 0, 0);
        }
        public DataSet Gets(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getClusters", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCluster", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        public int AddInstance(int _clusterid, string _nickname, int _sql)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@nickname", _nickname);
            arParams[2] = new SqlParameter("@sql", _sql);
            arParams[3] = new SqlParameter("@id", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addClusterInstance", arParams);
            return Int32.Parse(arParams[3].Value.ToString());
        }
        public void UpdateInstance(int _id, string _nickname)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@nickname", _nickname);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClusterInstance", arParams);
        }
        public void UpdateInstanceName(int _id, string _name)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClusterInstanceName", arParams);
        }
        public void UpdateInstanceIP(int _id, int _ipaddressid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@ipaddressid", _ipaddressid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClusterInstanceIP", arParams);
        }
        public void DeleteInstance(int _id, int _clusterid, int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteClusterInstance", arParams);
            Storage oStorage = new Storage(user, dsn);
            oStorage.DeleteLuns(_answerid, _id, _clusterid, 0, 0);
        }
        public DataSet GetInstances(int _clusterid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getClusterInstances", arParams);
        }
        public DataSet GetInstance(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getClusterInstance", arParams);
        }
        public string GetInstance(int _id, string _column)
        {
            DataSet ds = GetInstance(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }



        // CLUSTERING
        public DataSet GetClustering()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getClustering");
        }
        public int AddClustering(int _answerid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@id", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addClustering", arParams);
            return Int32.Parse(arParams[1].Value.ToString());
        }
        public void UpdateClusteringStarted(int _answerid, string _started)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@started", (_started == "" ? SqlDateTime.Null : DateTime.Parse(_started)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClusteringStarted", arParams);
        }
        public void UpdateClusteringCompleted(int _answerid, string _output, string _completed, int _error)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@output", _output);
            arParams[2] = new SqlParameter("@completed", (_completed == "" ? SqlDateTime.Null : DateTime.Parse(_completed)));
            arParams[3] = new SqlParameter("@error", _error);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateClusteringCompleted", arParams);
        }

    }
}
