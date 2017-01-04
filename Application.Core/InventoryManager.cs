using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class InventoryManager
	{
		private string dsn = "";
		private int user = 0;
		
		private SqlParameter[] arParams;
		
        public InventoryManager(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
			
		}
	

        public DataSet GetIMServerDemand(
                                        string LocationIds ,
                                        string ProjectIds ,
                                        string ModelIds,
                                        string ClassIds ,
                                        string EnvironmentIds ,
                                        string ConfidenceIds ,
                                        string ModelPlatformIds,
                                        DateTime? DateFrom, 
                                        DateTime? DateTo,
                                        int GrpLocation,
                                        int GrpProject,
                                        int GrpClass,
                                        int GrpEnv,
                                        int GrpConfidence,
                                        int GrpModelType)
        {
            arParams = new SqlParameter[25];
            if(LocationIds!="")
                arParams[1] = new SqlParameter("@LocationIds", LocationIds);
            if (ProjectIds != "")
                arParams[2] = new SqlParameter("@ProjectIds", ProjectIds);
            if (ModelIds != "")
                arParams[3] = new SqlParameter("@ModelIds", ModelIds);
            if (ClassIds != "")
                arParams[4] = new SqlParameter("@ClassIds", ClassIds);
            if (EnvironmentIds != "")
                arParams[5] = new SqlParameter("@EnvironmentIds", EnvironmentIds);
            if (ConfidenceIds != "")
                arParams[6] = new SqlParameter("@ConfidenceIds", ConfidenceIds);
            if (ModelPlatformIds != "")
                arParams[7] = new SqlParameter("@ModelPlatformIds", ModelPlatformIds);
            if (DateFrom != null)
                arParams[8] = new SqlParameter("@DateFrom", DateFrom);
            if (DateTo != null)
                arParams[9] = new SqlParameter("@DateTo", DateTo);

            arParams[10] = new SqlParameter("@GrpLocation", GrpLocation);
            arParams[11] = new SqlParameter("@GrpProject", GrpProject);
            arParams[12] = new SqlParameter("@GrpClass", GrpClass);
            arParams[13] = new SqlParameter("@GrpEnv", GrpEnv);
            arParams[14] = new SqlParameter("@GrpConfidence", GrpConfidence);
            arParams[15] = new SqlParameter("@GrpModelType", GrpModelType);
            
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIMServerDemand", arParams);

        }

        public DataSet GetIMServerSupplyAndDemand(
                                                string LocationIds,
                                                string ModelIds,
                                                string ClassIds,
                                                string EnvironmentIds,
                                                string ModelTypes,
                                                string ModelPlatformIds,
                                                int GrpLocation,
                                                int GrpClass,
                                                int GrpEnv,
                                                int GrpModelType,
                                                int WarningAndCritical)
        {

            arParams = new SqlParameter[25];
            if (LocationIds != "")
                arParams[0] = new SqlParameter("@LocationIds", LocationIds);
            if (ModelIds != "")
                arParams[1] = new SqlParameter("@ModelIds", ModelIds);
            if (ClassIds != "")
                arParams[2] = new SqlParameter("@ClassIds", ClassIds);
            if (EnvironmentIds != "")
                arParams[3] = new SqlParameter("@EnvironmentIds", EnvironmentIds);
            if (ModelTypes != "")
                arParams[4] = new SqlParameter("@ModelTypes", ModelTypes);
            if (ModelPlatformIds != "")
                arParams[5] = new SqlParameter("@ModelPlatformIds", ModelPlatformIds);
            
            arParams[6] = new SqlParameter("@GrpLocation", GrpLocation);
            arParams[7] = new SqlParameter("@GrpClass", GrpClass);
            arParams[8] = new SqlParameter("@GrpEnv", GrpEnv);
            arParams[9] = new SqlParameter("@GrpModelType", GrpModelType);
            arParams[10] = new SqlParameter("@WarningAndCritical", WarningAndCritical);

            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIMServerSupplyAndDemand", arParams);
        }


        public DataSet GetIMStorageDemand(
                                string LocationIds,
                                string ProjectIds,
                                string ModelIds,
                                string ClassIds,
                                string EnvironmentIds,
                                string ConfidenceIds,
                                DateTime? DateFrom,
                                DateTime? DateTo,
                                int GrpLocation,
                                int GrpProject,
                                int GrpClass,
                                int GrpEnv,
                                int GrpConfidence,
                                int IncludeServerGrowthForms)
        {
            arParams = new SqlParameter[25];
            if (LocationIds != "")
                arParams[1] = new SqlParameter("@LocationIds", LocationIds);
            if (ProjectIds != "")
                arParams[2] = new SqlParameter("@ProjectIds", ProjectIds);
            if (ModelIds != "")
                arParams[3] = new SqlParameter("@ModelIds", ModelIds);
            if (ClassIds != "")
                arParams[4] = new SqlParameter("@ClassIds", ClassIds);
            if (EnvironmentIds != "")
                arParams[5] = new SqlParameter("@EnvironmentIds", EnvironmentIds);
            if (ConfidenceIds != "")
                arParams[6] = new SqlParameter("@ConfidenceIds", ConfidenceIds);
            if (DateFrom != null)
                arParams[7] = new SqlParameter("@DateFrom", DateFrom);
            if (DateTo != null)
                arParams[8] = new SqlParameter("@DateTo", DateTo);

            arParams[9] = new SqlParameter("@GrpLocation", GrpLocation);
            arParams[10] = new SqlParameter("@GrpProject", GrpProject);
            arParams[11] = new SqlParameter("@GrpClass", GrpClass);
            arParams[12] = new SqlParameter("@GrpEnv", GrpEnv);
            arParams[13] = new SqlParameter("@GrpConfidence", GrpConfidence);
            arParams[14] = new SqlParameter("@IncludeServerGrowthForms", IncludeServerGrowthForms);

            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIMStorageDemand", arParams);

        }


    }
}
