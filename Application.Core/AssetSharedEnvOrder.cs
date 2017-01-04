using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;
using Tamir.SharpSsh;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


namespace NCC.ClearView.Application.Core
{
    
    public enum AssetSharedEnvOrderType
    {
        AddCluster = 1,
        AddHost = 2,
        AddStorage = 3,
    }

    public class AssetSharedEnvOrder
    {
        private string dsnCV = "";
        private string dsnCVAsset = "";
        private SqlParameter[] arParams;
        private int intUser = 0;
        private int intEnvironment = 0;
        private Variables oVariable;
        private Functions oFunction;
        private ModelsProperties oModelProperty;
        private Models oModel;
        private Asset oAsset;



        public AssetSharedEnvOrder(int _user, string _dsnCV, string _dsnCVAsset, int _environment)
		{
			intUser = _user;
            dsnCV = _dsnCV;
            dsnCVAsset = _dsnCVAsset;
            intEnvironment = _environment;
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(0, dsnCV, intEnvironment);
            oModelProperty = new ModelsProperties(intUser, dsnCV);
            oModel = new Models(intUser, dsnCV);
            oAsset = new Asset(intUser, dsnCVAsset);
        }


        # region Asset Shared Env. Order - Add, Update, Get

        public void AddOrder(int _OrderId, int _RequestId, int _ItemId, int _Number, int _OrderType,
                            string _NickName,int _ModelId,
                            int _LocationId,int _ClassId,int _EnvironmentId,int _ParentId,
                            DateTime _RequestedByDate,int _ClusterType,
                            int _Function_VMWARE_Workstation,int _Function_VMWARE_Server,int _Function_VMWARE_Windows,int _Function_VMWARE_Linux,
                            int _Function_SUN_Container, int _Function_SUN_LDOM, 
                            float _StorageAmt,int _StatusId,int _CreatedBy)
        {
         arParams = new SqlParameter[25];
         arParams[0] = new SqlParameter("@OrderId", _OrderId);
         arParams[1] = new SqlParameter("@RequestId", _RequestId);
         arParams[2] = new SqlParameter("@ItemId", _ItemId);
         arParams[3] = new SqlParameter("@Number", _Number);
         arParams[4] = new SqlParameter("@OrderType", _OrderType);
         arParams[5] = new SqlParameter("@NickName", _NickName);
         arParams[6] = new SqlParameter("@ModelId", _ModelId);
         arParams[7] = new SqlParameter("@LocationId", _LocationId);
         arParams[8] = new SqlParameter("@ClassId", _ClassId);
         arParams[9] = new SqlParameter("@EnvironmentId", _EnvironmentId);
         arParams[10] = new SqlParameter("@ParentId", _ParentId);
         arParams[11] = new SqlParameter("@RequestedByDate", _RequestedByDate);
         arParams[12] = new SqlParameter("@ClusterType", _ClusterType);
         arParams[13] = new SqlParameter("@Function_VMWARE_Workstation", _Function_VMWARE_Workstation);
         arParams[14] = new SqlParameter("@Function_VMWARE_Server", _Function_VMWARE_Server);
         arParams[15] = new SqlParameter("@Function_VMWARE_Windows", _Function_VMWARE_Windows);
         arParams[16] = new SqlParameter("@Function_VMWARE_Linux", _Function_VMWARE_Linux);
         arParams[17] = new SqlParameter("@Function_SUN_Container", _Function_SUN_Container);
         arParams[18] = new SqlParameter("@Function_SUN_LDOM", _Function_SUN_LDOM);
         arParams[19] = new SqlParameter("@StorageAmt", _StorageAmt);
         arParams[20] = new SqlParameter("@StatusId", _StatusId);
         arParams[21] = new SqlParameter("@CreatedBy", _CreatedBy);
         SqlHelper.ExecuteNonQuery(dsnCV, CommandType.StoredProcedure, "pr_addAssetSharedEnvOrder", arParams);

        }

        public void UpdateOrder(int _OrderId, int _RequestId, int _ItemId, int _Number, int _OrderType,
                                        string _NickName,int _ModelId,
                                        int _LocationId,int _ClassId,int _EnvironmentId,int _ParentId,
                                        DateTime _RequestedByDate,int _ClusterType,
                                        int _Function_VMWARE_Workstation,int _Function_VMWARE_Server,int _Function_VMWARE_Windows,int _Function_VMWARE_Linux,
                                        int _Function_SUN_Container,int _Function_SUN_LDOM,
                                        float _StorageAmt, int _StatusId, int _ModifiedBy)
        {
            arParams = new SqlParameter[25];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            arParams[1] = new SqlParameter("@RequestId", _RequestId);
            arParams[2] = new SqlParameter("@ItemId", _ItemId);
            arParams[3] = new SqlParameter("@Number", _Number);
            arParams[4] = new SqlParameter("@OrderType", _OrderType);
            arParams[5] = new SqlParameter("@NickName", _NickName);
            arParams[6] = new SqlParameter("@ModelId", _ModelId);
            arParams[7] = new SqlParameter("@LocationId", _LocationId);
            arParams[8] = new SqlParameter("@ClassId", _ClassId);
            arParams[9] = new SqlParameter("@EnvironmentId", _EnvironmentId);
            arParams[10] = new SqlParameter("@ParentId", _ParentId);
            arParams[11] = new SqlParameter("@RequestedByDate", _RequestedByDate);
            arParams[12] = new SqlParameter("@ClusterType", _ClusterType);
            arParams[13] = new SqlParameter("@Function_VMWARE_Workstation", _Function_VMWARE_Workstation);
            arParams[14] = new SqlParameter("@Function_VMWARE_Server", _Function_VMWARE_Server);
            arParams[15] = new SqlParameter("@Function_VMWARE_Windows", _Function_VMWARE_Windows);
            arParams[16] = new SqlParameter("@Function_VMWARE_Linux", _Function_VMWARE_Linux);
            arParams[17] = new SqlParameter("@Function_SUN_Container", _Function_SUN_Container);
            arParams[18] = new SqlParameter("@Function_SUN_LDOM", _Function_SUN_LDOM);
            arParams[19] = new SqlParameter("@StorageAmt", _StorageAmt);
            arParams[20] = new SqlParameter("@StatusId", _StatusId);
            arParams[21] = new SqlParameter("@ModifiedBy", _ModifiedBy);

            SqlHelper.ExecuteNonQuery(dsnCV, CommandType.StoredProcedure, "pr_updateAssetSharedEnvOrder", arParams);

        }

      

      
        public void UpdateOrderStatus(int _OrderId,int _StatusId,int _ModifiedBy)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            arParams[1] = new SqlParameter("@StatusId", _StatusId);
            arParams[2] = new SqlParameter("@ModifiedBy", _ModifiedBy);

            SqlHelper.ExecuteNonQuery(dsnCV, CommandType.StoredProcedure, "pr_updateAssetSharedEnvOrderStatus", arParams);


        }
        
        public DataSet Get(int _RequestId, int _ItemId, int _Number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@RequestId", _RequestId);
            arParams[1] = new SqlParameter("@ItemId", _ItemId);
            arParams[2] = new SqlParameter("@Number", _Number);
            return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetSharedEnvOrders", arParams);
        }

        public DataSet Get(int _RequestId, int _Number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@RequestId", _RequestId);
            arParams[2] = new SqlParameter("@Number", _Number);
            return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetSharedEnvOrders", arParams);
        }

        public DataSet Get(int _OrderId)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetSharedEnvOrders", arParams);
        }

        public DataSet Get(int? _modelId, string _statusIds,
                           DateTime? dtSubmittedFrom, DateTime? _dtSubmittedTo,
                           string _orderBy, int? _order, int? _page, int? _recordsPerPage)
        {

            arParams = new SqlParameter[10];
            if (_modelId!=null)
                arParams[0] = new SqlParameter("@ModelId", _modelId);
            if (_statusIds != "")
                arParams[1] = new SqlParameter("@StatusIds", _statusIds);
            if (dtSubmittedFrom != null)
                arParams[2] = new SqlParameter("@SubmittedDateFrom", dtSubmittedFrom);
            if (_dtSubmittedTo != null)
                arParams[3] = new SqlParameter("@SubmittedDateTo", _dtSubmittedTo);
            if (_orderBy != null)
                arParams[4] = new SqlParameter("@OrderBy", _orderBy);
            if (_order != null)
                arParams[5] = new SqlParameter("@Order", _order);
            if (_page != null)
                arParams[6] = new SqlParameter("@Page", _page);
            if (_recordsPerPage != null)
                arParams[7] = new SqlParameter("@RecsPerPage", _recordsPerPage);

            return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetSharedEnvOrders", arParams);

        }


        public string GetOrderBody(int _RequestId, int _ItemId, int _Number)
        {
            
            StringBuilder sbBody = new StringBuilder();
            //string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
            string strSpacerRow = "<tr><td colspan=\"3\">&nbsp;</td></tr>";

            string strSpacerTD = "<td style=\"width:10;text-align:left\"></td>";
            string strTRstart = "<tr>";
            string strTRend = "</tr>";
            string strTDstart = "<td>";
            string strTDend = "</td>";
            DataSet ds = Get(_RequestId, _ItemId, _Number);
            if (ds.Tables[0].Rows.Count > 0)
            {

                DataRow dr = ds.Tables[0].Rows[0];

                string strOrderType = "";
                int intOrderType = 0;
                intOrderType = Int32.Parse(dr["OrderType"].ToString());
                if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster )
                        strOrderType="Add Cluster";
                    else if (intOrderType == (int)AssetSharedEnvOrderType.AddHost)
                        strOrderType = "Add Host";
                    else if (intOrderType == (int)AssetSharedEnvOrderType.AddStorage)
                        strOrderType = "Add Storage";

                sbBody.Append(strTRstart + strTDstart + "Order Type:" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + "<b>" + strOrderType + "</b>" + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "Nick Name:" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + dr["NickName"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);
                if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster || intOrderType == (int)AssetSharedEnvOrderType.AddHost)
                {
                    sbBody.Append(strTRstart + strTDstart + "Model:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + dr["ModelName"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Location:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + dr["Location"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Class:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + dr["Class"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Environment:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + dr["Environment"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);
                }
                if (intOrderType == (int)AssetSharedEnvOrderType.AddStorage)
                {
                    sbBody.Append(strTRstart + strTDstart + "Storage Amount:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + dr["StorageAmt"].ToString() +" GB"+ strTDend + strTRend);
                    sbBody.Append(strSpacerRow);
                }
               sbBody.Append(strTRstart + strTDstart + "Requested By Date:" + strTDend + strSpacerTD);
               sbBody.Append(strTDstart + (dr["RequestedByDate"].ToString() == "" ? "" : DateTime.Parse(dr["RequestedByDate"].ToString()).ToShortDateString()) + strTDend + strTRend);
               sbBody.Append(strSpacerRow);

                if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster) //Display Cluster Function  And Type
                {
                    string strClusterType="";
                    if (dr["ClusterType"].ToString()=="1")
                        strClusterType="VMWARE";
                    else if (dr["ClusterType"].ToString()=="2")
                        strClusterType="SUN";
                    else if (dr["ClusterType"].ToString()=="3")
                        strClusterType="ORACLE";
                    else if (dr["ClusterType"].ToString()=="4")
                        strClusterType="SQL";

                    sbBody.Append(strTRstart + strTDstart + "Type:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + strClusterType + strTRend);
                    sbBody.Append(strSpacerRow);

                    string strClusterFunctions="";

                    if (dr["ClusterType"].ToString() == "1")
                    {
                        if (dr["Function_VMWARE_Workstation"].ToString() == "1")
                            strClusterFunctions = (strClusterFunctions == "" ? strClusterFunctions : strClusterFunctions + ", ") + "Windows";

                        if (dr["Function_VMWARE_Server"].ToString() == "1")
                            strClusterFunctions = (strClusterFunctions == "" ? strClusterFunctions : strClusterFunctions + ", ") + "Server";

                        if (dr["Function_VMWARE_Windows"].ToString() == "1")
                            strClusterFunctions = (strClusterFunctions == "" ? strClusterFunctions : strClusterFunctions + ", ") + "Workstation";

                        if (dr["Function_VMWARE_Linux"].ToString() == "1")
                            strClusterFunctions = (strClusterFunctions == "" ? strClusterFunctions : strClusterFunctions + ", ") + "Linux";
                       
                    }
                    if (dr["ClusterType"].ToString() == "2")
                    {
                        if (dr["Function_SUN_Container"].ToString() == "1")
                            strClusterFunctions = (strClusterFunctions == "" ? strClusterFunctions : strClusterFunctions + ", ") + "Container";

                        if (dr["Function_SUN_LDOM"].ToString() == "1")
                            strClusterFunctions = (strClusterFunctions == "" ? strClusterFunctions : strClusterFunctions + ", ") + "LDOM";

                    }
                    
                    strClusterFunctions =(strClusterFunctions!=""?strClusterFunctions:strClusterFunctions+" -- " );

                    
                    sbBody.Append(strTRstart + strTDstart + "Functions:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + strClusterFunctions + strTRend);
                    sbBody.Append(strSpacerRow);



                }

                //Display Parent

                string strParent = "";

                if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster)
                    strParent = "Folder:";
                else if (intOrderType == (int)AssetSharedEnvOrderType.AddHost)
                    strParent = "Cluster:";
                else if (intOrderType == (int)AssetSharedEnvOrderType.AddStorage)
                    strParent = "Cluster:";
                sbBody.Append(strTRstart + strTDstart + strParent + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + dr["ParentName"].ToString() + strTRend);
                sbBody.Append(strSpacerRow);

            }

            if (sbBody.ToString() != "")
            {
                sbBody.Insert(0, "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                sbBody.Append("</table>");

               
            }
            return sbBody.ToString();
        }

        #endregion

        #region Add, Deleted, Get Datastore Selection
        
        public void AddDataStoreSelection(int _OrderId,int _ClusterId, string _Name, int _StorageType, int _Replicated, int _CreatedBy)
        {
        
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            arParams[1] = new SqlParameter("@ClusterId", _ClusterId);
            arParams[2] = new SqlParameter("@Name", _Name);
            arParams[3] = new SqlParameter("@Storage_Type", _StorageType);
            arParams[4] = new SqlParameter("@Replicated", _Replicated);
            arParams[5] = new SqlParameter("@CreatedBy", _CreatedBy);
            SqlHelper.ExecuteNonQuery(dsnCV, CommandType.StoredProcedure, "pr_addAssetSharedEnvOrderDataStoreSelection", arParams);
        }

        public void DeleteDataStoreSelection(int _Id, int _ModifiedBy)
        {
 
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@Id", _Id);
            arParams[1] = new SqlParameter("@ModifiedBy", _ModifiedBy);
            SqlHelper.ExecuteNonQuery(dsnCV, CommandType.StoredProcedure, "pr_deleteAssetSharedEnvOrderDataStoreSelection", arParams);
        }

        public DataSet GetDataStoreSelection(int _OrderId)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetSharedEnvOrderDataStoreSelection", arParams);
        }

        public DataSet GetHostSelection(int _OrderId)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetSharedEnvOrderHostSelection", arParams);
        }

        #endregion
    }


}
