using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class AssetCategory
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public AssetCategory(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
    
        public void Add(string _AssetCategoryName, int _enabled, int _userid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@AssetCategoryName", _AssetCategoryName);
            arParams[1] = new SqlParameter("@Enabled", _enabled);
            arParams[2] = new SqlParameter("@CreatedBy", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAssetCategory", arParams);
        }

        public void Update(int _Id, string _AssetCategoryName, int _enabled, int _userid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@Id", _Id);
            arParams[1] = new SqlParameter("@AssetCategoryName", _AssetCategoryName);
            arParams[2] = new SqlParameter("@Enabled", _enabled);
            arParams[3] = new SqlParameter("@ModifiedBy", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAssetCategory", arParams);
        }

        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@Enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAssetCategory", arParams);
        }

        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@Id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAssetCategory", arParams);
        }

        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAssetCategory", arParams);
        }

        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@Id", _id);
            arParams[1] = new SqlParameter("@Enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAssetCategoryEnabled", arParams);
        }

        

        
    }


    public class AssetCategoryDeploymentConfig
    { 
    
        private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public AssetCategoryDeploymentConfig(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}

        public void add(int _AssetCategoryId ,
                        int _ServiceId ,
                        int _IsRequiredForProcurement ,
                        int _IsRequiredForReDeployment ,
                        int _IsRequiredForMovement ,
                        int _IsRequiredForDispose ,
                        int _IsAssetStatusChangeApplicable ,
                        int _AssetStatusIn ,
                        int _AssetStatusOut,
                        string _CustomTaskName,
                        int _DisplayOrder ,
                        int _Enabled ,
                        int _CreatedBy )
        {
            arParams = new SqlParameter[15];
            arParams[0] = new SqlParameter("@AssetCategoryId", _AssetCategoryId);
            arParams[1] = new SqlParameter("@ServiceId", _ServiceId);
            arParams[2] = new SqlParameter("@IsRequiredForProcurement", _IsRequiredForProcurement);
            arParams[3] = new SqlParameter("@IsRequiredForReDeployment", _IsRequiredForReDeployment);
            arParams[4] = new SqlParameter("@IsRequiredForMovement", _IsRequiredForMovement);
            arParams[5] = new SqlParameter("@IsRequiredForDispose", _IsRequiredForDispose);
            arParams[6] = new SqlParameter("@IsAssetStatusChangeApplicable", _IsAssetStatusChangeApplicable);
            arParams[7] = new SqlParameter("@AssetStatusIn", _AssetStatusIn);
            arParams[8] = new SqlParameter("@AssetStatusOut", _AssetStatusOut);
            arParams[9] = new SqlParameter("@CustomTaskName", _CustomTaskName);
            arParams[10] = new SqlParameter("@Enabled", _Enabled);
            arParams[11] = new SqlParameter("@CreatedBy", _CreatedBy);

            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAssetCategoryDeploymentConfig", arParams);
        }

        public void update(
                        int _Id ,
                        int _AssetCategoryId ,
                        int _ServiceId ,
                        int _IsRequiredForProcurement ,
                        int _IsRequiredForReDeployment ,
                        int _IsRequiredForMovement ,
                        int _IsRequiredForDispose ,
                        int _IsAssetStatusChangeApplicable ,
                        int _AssetStatusIn ,
                        int _AssetStatusOut,
                        string _CustomTaskName ,
                        int _DisplayOrder ,
                        int _Enabled ,
                        int _ModifiedBy)
        {
            arParams = new SqlParameter[15];
            arParams[0] = new SqlParameter("@Id", _Id);
            arParams[1] = new SqlParameter("@AssetCategoryId", _AssetCategoryId);
            arParams[2] = new SqlParameter("@ServiceId", _ServiceId);
            arParams[3] = new SqlParameter("@IsRequiredForProcurement", _IsRequiredForProcurement);
            arParams[4] = new SqlParameter("@IsRequiredForReDeployment", _IsRequiredForReDeployment);
            arParams[5] = new SqlParameter("@IsRequiredForMovement", _IsRequiredForMovement);
            arParams[6] = new SqlParameter("@IsRequiredForDispose", _IsRequiredForDispose);
            arParams[7] = new SqlParameter("@IsAssetStatusChangeApplicable", _IsAssetStatusChangeApplicable);
            arParams[8] = new SqlParameter("@AssetStatusIn", _AssetStatusIn);
            arParams[9] = new SqlParameter("@AssetStatusOut", _AssetStatusOut);
            arParams[10] = new SqlParameter("@CustomTaskName", _CustomTaskName);
            arParams[11] = new SqlParameter("@DisplayOrder", _DisplayOrder);
            arParams[12] = new SqlParameter("@Enabled", _Enabled);
            arParams[13] = new SqlParameter("@ModifiedBy", _ModifiedBy);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAssetCategoryDeploymentConfig", arParams);
        }

        public DataSet gets(int _Id)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@Id", _Id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAssetCategoryDeploymentConfig", arParams);
        }

        public DataSet gets(int _AssetCategoryId, int? _Enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@AssetCategoryId", _AssetCategoryId);
            if (_Enabled!=null)
                arParams[1] = new SqlParameter("@Enabled", _Enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAssetCategoryDeploymentConfig", arParams);
        }


        public DataSet getsByAssetCategoryAndOrderType(int _AssetCategoryId, int _OrderType, int? _Enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@AssetCategoryId", _AssetCategoryId);
            arParams[1] = new SqlParameter("@OrderType", _OrderType);
            if (_Enabled != null)
                arParams[2] = new SqlParameter("@Enabled", _Enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAssetCategoryDeploymentConfig", arParams);
        }
        public void delete(int _Id, int _ModifiedBy)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@Id", _Id);
            arParams[1] = new SqlParameter("@ModifiedBy", _ModifiedBy);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAssetCategoryDeploymentConfig", arParams);
        }

    }


}
