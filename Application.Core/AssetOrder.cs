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
    public enum AssestOrderReqStatus
    {
        Pending = 0,
        Active = 1,
        Rejected = 2,
        Hold = 3,
        Completed = 4,
        UpdateOnly = 999
    }

    public enum AssetOrderType
    {
        Procure = 1,
        ReDeploy = 2,
        Movement = 3,
        Dispose= 4
    }

    public class AssetOrder
    {
        private string dsnCV = "";
        private string dsnCVAsset = "";
        private int intUser = 0;
        private int intEnvironment = 0;
       
        private Models oModel;
        private ModelsProperties oModelProperty;
        private Asset oAsset;
        private Variables oVariable;
        private Functions oFunction;

        private SqlParameter[] arParams; 

        private string strIPAddressPattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";

        private string strMACAddressPattern = @"^([0-9a-fA-F][0-9a-fA-F]:){5}([0-9a-fA-F][0-9a-fA-F])$";
      

        public AssetOrder(int _user, string _dsnCV, string _dsnCVAsset, int _environment)
		{
			dsnCV = _dsnCV;
            dsnCVAsset = _dsnCVAsset;
            intUser = _user;
            intEnvironment = _environment;
            
            oModel = new Models(intUser, dsnCV);
            oModelProperty = new ModelsProperties(intUser, dsnCV);
            oAsset = new Asset(intUser, dsnCVAsset,dsnCV);
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(intUser, dsnCV, intEnvironment);
        }


        public int AddOrderId(int _user)
        {
            arParams = new SqlParameter[25];
            arParams[0] = new SqlParameter("@OrderId", 0);
            arParams[1] = new SqlParameter("@CreatedBy", _user);
            arParams[0].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsnCV, CommandType.StoredProcedure, "pr_addAssetOrderIds", arParams);
            return Int32.Parse(arParams[0].Value.ToString());

        }

        public void AddOrder(int _OrderId,int _RequestId,int _ItemId,int _Number,int _OrderType,
                            string _NickName,int _ModelId,
                            int _LocationId, int _RoomId, int _ZoneId, int _RackId,
                            string _RackPos, int _ResiliencyId, int _OperatingSystemGroupId,
                            int _ClassId,   int _EnvironmentId,
                            int _EnclosureId,int _EnclosureSlot,
                            int _RequestedQuantity,int _ProcureQuantity,int _ReDeployQuantity,int _ReturnedQuantity,
                            DateTime _RequestedByDate, int _IsClustered, int _SanAttached, int _IsBootLun,
                            int _Switch1, string _Port1, int _Switch2, string _Port2, int _StatusId, int _CreatedBy)
{
         arParams = new SqlParameter[32];
         arParams[0] = new SqlParameter("@OrderId", _OrderId);
         arParams[1] = new SqlParameter("@RequestId", _RequestId);
         arParams[2] = new SqlParameter("@ItemId", _ItemId);
         arParams[3] = new SqlParameter("@Number", _Number);
         arParams[4] = new SqlParameter("@OrderType", _OrderType);
         arParams[5] = new SqlParameter("@NickName", _NickName);
         arParams[6] = new SqlParameter("@ModelId", _ModelId);
         arParams[7] = new SqlParameter("@LocationId", _LocationId);
         arParams[8] = new SqlParameter("@RoomId", _RoomId);
         arParams[9] = new SqlParameter("@ZoneId", _ZoneId);
         arParams[10] = new SqlParameter("@RackId", _RackId);
         arParams[11] = new SqlParameter("@RackPos", _RackPos);
         arParams[12] = new SqlParameter("@ResiliencyId", _ResiliencyId);
         arParams[13] = new SqlParameter("@OperatingSystemGroupId", _OperatingSystemGroupId);
         arParams[14] = new SqlParameter("@ClassId", _ClassId);
         arParams[15] = new SqlParameter("@EnvironmentId", _EnvironmentId);
         arParams[16] = new SqlParameter("@EnclosureId", _EnclosureId);
         arParams[17] = new SqlParameter("@EnclosureSlot", _EnclosureSlot);
         arParams[18] = new SqlParameter("@RequestedQuantity", _RequestedQuantity);
         arParams[19] = new SqlParameter("@ProcureQuantity", _ProcureQuantity);
         arParams[20] = new SqlParameter("@ReDeployQuantity", _ReDeployQuantity);
         arParams[21] = new SqlParameter("@ReturnedQuantity", _ReturnedQuantity);
         arParams[22] = new SqlParameter("@RequestedByDate", _RequestedByDate);
         arParams[23] = new SqlParameter("@IsClustered", _IsClustered);
         arParams[24] = new SqlParameter("@SanAttached", _SanAttached);
         arParams[25] = new SqlParameter("@IsBootLun", _IsBootLun);
         arParams[26] = new SqlParameter("@Switch1", _Switch1);
         arParams[27] = new SqlParameter("@Port1", _Port1);
         arParams[28] = new SqlParameter("@Switch2", _Switch2);
         arParams[29] = new SqlParameter("@Port2", _Port2);
         arParams[30] = new SqlParameter("@StatusId", _StatusId);
         arParams[31] = new SqlParameter("@CreatedBy", _CreatedBy);
         SqlHelper.ExecuteNonQuery(dsnCV, CommandType.StoredProcedure, "pr_addAssetOrder", arParams);

        }

        public void UpdateOrder(int _OrderId, int _RequestId, int _ItemId, int _Number, int _OrderType,
                               string _NickName, int _ModelId,
                               int _LocationId, int _RoomId, int _ZoneId, int _RackId,
                                string _RackPos, int _ResiliencyId, int _OperatingSystemGroupId,
                               int _ClassId, int _EnvironmentId,
                               int _EnclosureId, int _EnclosureSlot,
                               int _RequestedQuantity, int _ProcureQuantity, int _ReDeployQuantity, int _ReturnedQuantity,
                               DateTime _RequestedByDate, int _IsClustered, int _SanAttached, int _IsBootLun,
                                int _Switch1, string _Port1, int _Switch2, string _Port2, int _StatusId, int _modifiedBy)
        {
            arParams = new SqlParameter[32];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            arParams[1] = new SqlParameter("@RequestId", _RequestId);
            arParams[2] = new SqlParameter("@ItemId", _ItemId);
            arParams[3] = new SqlParameter("@Number", _Number);
            arParams[4] = new SqlParameter("@OrderType", _OrderType);
            arParams[5] = new SqlParameter("@NickName", _NickName);
            arParams[6] = new SqlParameter("@ModelId", _ModelId);
            arParams[7] = new SqlParameter("@LocationId", _LocationId);
            arParams[8] = new SqlParameter("@RoomId", _RoomId);
            arParams[9] = new SqlParameter("@ZoneId", _ZoneId);
            arParams[10] = new SqlParameter("@RackId", _RackId);
            arParams[11] = new SqlParameter("@RackPos", _RackPos);
            arParams[12] = new SqlParameter("@ResiliencyId", _ResiliencyId);
            arParams[13] = new SqlParameter("@OperatingSystemGroupId", _OperatingSystemGroupId);
            arParams[14] = new SqlParameter("@ClassId", _ClassId);
            arParams[15] = new SqlParameter("@EnvironmentId", _EnvironmentId);
            arParams[16] = new SqlParameter("@EnclosureId", _EnclosureId);
            arParams[17] = new SqlParameter("@EnclosureSlot", _EnclosureSlot);
            arParams[18] = new SqlParameter("@RequestedQuantity", _RequestedQuantity);
            arParams[19] = new SqlParameter("@ProcureQuantity", _ProcureQuantity);
            arParams[20] = new SqlParameter("@ReDeployQuantity", _ReDeployQuantity);
            arParams[21] = new SqlParameter("@ReturnedQuantity", _ReturnedQuantity);
            arParams[22] = new SqlParameter("@RequestedByDate", _RequestedByDate);
            arParams[23] = new SqlParameter("@IsClustered", _IsClustered);
            arParams[24] = new SqlParameter("@SanAttached", _SanAttached);
            arParams[25] = new SqlParameter("@IsBootLun", _IsBootLun);
            arParams[26] = new SqlParameter("@Switch1", _Switch1);
            arParams[27] = new SqlParameter("@Port1", _Port1);
            arParams[28] = new SqlParameter("@Switch2", _Switch2);
            arParams[29] = new SqlParameter("@Port2", _Port2);
            arParams[30] = new SqlParameter("@StatusId", _StatusId);
            arParams[31] = new SqlParameter("@ModifiedBy", _modifiedBy);
            SqlHelper.ExecuteNonQuery(dsnCV, CommandType.StoredProcedure, "pr_updateAssetOrder", arParams);
        }

        public void DeleteOrder(int _OrderId)
        {
            arParams = new SqlParameter[25];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            SqlHelper.ExecuteNonQuery(dsnCV, CommandType.StoredProcedure, "pr_deleteAssetOrder", arParams);

        }

        public void UpdateProcurementDetails(int _OrderId,
                                            string _NickName,
                                            int _ProcureQuantity,int _ReDeployQuantity,int _ReturnedQuantity,
                                            int _PurchaseOrderStatusId,string _PurchaseOrderComments,DateTime? _PurchaseOrderDate,float _PurchaseOrderPrice,
                                            int _VendorOrderStatusId,DateTime? _VendorOrderDate,string _VendorTrackingNumber,
                                            int _ProjectId, string _PurchaseOrderNumber, int _ApprovedQuantity, DateTime? _ApprovedOn,
                                            string _PurchaseOrderUpload, string _QuoteNumber, DateTime? _QuoteDate, DateTime? _WarrantyDate, float _SystemPrice, 
                                            float _SalesTax, string _ManufacturerQuoteUpload, int _AttentionTo, int _modifiedBy)
        {
            arParams = new SqlParameter[25];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            arParams[1] = new SqlParameter("@NickName", _NickName);
            arParams[2] = new SqlParameter("@ProcureQuantity", _ProcureQuantity);
            arParams[3] = new SqlParameter("@ReDeployQuantity", _ReDeployQuantity);
            arParams[4] = new SqlParameter("@ReturnedQuantity", _ReturnedQuantity);
            arParams[5] = new SqlParameter("@PurchaseOrderStatusId", _PurchaseOrderStatusId);
            arParams[6] = new SqlParameter("@PurchaseOrderComments", _PurchaseOrderComments);
            if (_PurchaseOrderDate !=null)
                arParams[7] = new SqlParameter("@PurchaseOrderDate", _PurchaseOrderDate);

            arParams[8] = new SqlParameter("@PurchaseOrderPrice", _PurchaseOrderPrice);

            arParams[9] = new SqlParameter("@VendorOrderStatusId", _VendorOrderStatusId);
            
            if (_VendorOrderDate!=null)
                arParams[10] = new SqlParameter("@VendorOrderDate", _VendorOrderDate);
            
            arParams[11] = new SqlParameter("@VendorTrackingNumber", _VendorTrackingNumber);
            arParams[12] = new SqlParameter("@ProjectId", _ProjectId);
            arParams[13] = new SqlParameter("@PurchaseOrderNumber", _PurchaseOrderNumber);
            arParams[14] = new SqlParameter("@ApprovedQuantity", _ApprovedQuantity);
            arParams[15] = new SqlParameter("@ApprovedOn", _ApprovedOn);
            arParams[16] = new SqlParameter("@PurchaseOrderUpload", _PurchaseOrderUpload);
            arParams[17] = new SqlParameter("@QuoteNumber", _QuoteNumber);
            arParams[18] = new SqlParameter("@QuoteDate", _QuoteDate);
            arParams[19] = new SqlParameter("@WarrantyDate", _WarrantyDate);
            arParams[20] = new SqlParameter("@SystemPrice", _SystemPrice);
            arParams[21] = new SqlParameter("@SalesTax", _SalesTax);
            arParams[22] = new SqlParameter("@ManufacturerQuoteUpload", _ManufacturerQuoteUpload);
            arParams[23] = new SqlParameter("@AttentionTo", _AttentionTo);
            arParams[24] = new SqlParameter("@ModifiedBy", _modifiedBy);

            SqlHelper.ExecuteNonQuery(dsnCV, CommandType.StoredProcedure, "pr_updateAssetOrderProcurementDetails", arParams);
         
        }


        public void UpdateAssetOrderDepotDateReceived(int _OrderId,int _ReceivedDepotId,DateTime? _ReceivedDate,int _modifiedBy)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            arParams[1] = new SqlParameter("@ReceivedDepotId", _ReceivedDepotId);
            arParams[2] = new SqlParameter("@ReceivedDate", _ReceivedDate);
            arParams[3] = new SqlParameter("@ModifiedBy", _modifiedBy);

            SqlHelper.ExecuteNonQuery(dsnCV, CommandType.StoredProcedure, "pr_updateAssetOrderDepotDateReceived", arParams);

        }

        public void UpdateOrderStatus(int _OrderId, int _StatusId, int _modifiedBy)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            arParams[1] = new SqlParameter("@StatusId", _StatusId);
            arParams[2] = new SqlParameter("@ModifiedBy", _modifiedBy);

            SqlHelper.ExecuteNonQuery(dsnCV, CommandType.StoredProcedure, "pr_updateAssetOrderStatus", arParams);
        }

        public DataSet Get(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@RequestId", _requestid);
            //arParams[1] = new SqlParameter("@ItemId", _itemid);
            arParams[2] = new SqlParameter("@Number", _number);
            return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetOrders", arParams);
        }

        public DataSet GetByAsset(int _assetid, bool _include_cancelled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@cancelled", (_include_cancelled ? 1 : 0));
            return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetOrdersByAsset", arParams);
        }
        public DataSet GetByOrderID(int _orderid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@orderid", _orderid);
            return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetOrdersByOrderID", arParams);
        }
        

        public DataSet Get(Int64 _OrderId)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetOrders", arParams);
        }

        public DataSet Get(int? _platformId, int? _modelId, string _statusIds,
                           DateTime? dtSubmittedFrom, DateTime? _dtSubmittedTo,
                           string _orderBy, int? _order, int? _page, int? _recordsPerPage)
        {

            arParams = new SqlParameter[10];
            if (_platformId != null)
                arParams[0] = new SqlParameter("@PlatformId", _platformId);
            if (_modelId!=null)
                arParams[1] = new SqlParameter("@ModelId", _modelId);
            if (_statusIds != "")
                arParams[2] = new SqlParameter("@StatusIds", _statusIds);
            if (dtSubmittedFrom != null)
                arParams[3] = new SqlParameter("@SubmittedDateFrom", dtSubmittedFrom);
            if (_dtSubmittedTo != null)
                arParams[4] = new SqlParameter("@SubmittedDateTo", _dtSubmittedTo);
            if (_orderBy != null)
                arParams[5] = new SqlParameter("@OrderBy", _orderBy);
            if (_order != null)
                arParams[6] = new SqlParameter("@Order", _order);
            if (_page != null)
                arParams[7] = new SqlParameter("@Page", _page);
            if (_recordsPerPage != null)
                arParams[8] = new SqlParameter("@RecsPerPage", _recordsPerPage);

            return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetOrders", arParams);

        }


        public string GetOrderBody(int _requestid, int _itemid, int _number)
        {
            StringBuilder sbBodyModelThresholdAttributes = new StringBuilder();
            StringBuilder sbBody = new StringBuilder();
            //string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
            string strSpacerRow = "<tr><td colspan=\"3\">&nbsp;</td></tr>";

            string strSpacerTD = "<td style=\"width:10;text-align:left\"></td>";
            string strTRstart = "<tr>";
            string strTRend = "</tr>";
            string strTDstart = "<td>";
            string strTDend = "</td>";
            int intOrderType = 0;
            int intOrderId=0;
            string strAssetCategoryId = "";
            DataSet ds = Get(_requestid, _itemid, _number);
            if (ds.Tables[0].Rows.Count > 0)
            {
             
                DataRow dr = ds.Tables[0].Rows[0];
                intOrderType = Int32.Parse(dr["OrderType"].ToString());
                intOrderId= Int32.Parse(dr["OrderId"].ToString());

                sbBody.Append(strTRstart + strTDstart + "Order Type:" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart +"<b>" + dr["OrderTypeName"].ToString()+"</b>" + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "Description:" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + dr["NickName"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "Model:" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + "<b>" + dr["ModelName"].ToString() + "</b>" + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "Barcode:" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + "<iframe src=\"" + oVariable.URL() + "/admin/asset/asset_barcodes_view.aspx?modelid=" + dr["ModelId"].ToString() + "\" frameborder=\"0\" scrolling=\"no\" width=\"200\" height=\"120\"></iframe>" + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                strAssetCategoryId = oModelProperty.Get(Int32.Parse(dr["ModelId"].ToString()), "asset_category");
                

               // sbBodyModelThresholdAttributes.Append(GetModelThresholdAttributes(Int32.Parse(dr["ModelId"].ToString())));

                if (intOrderType != (int)AssetOrderType.Dispose)
                {
                    sbBody.Append(strTRstart + strTDstart + "Intended Location:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + dr["Location"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);
                    if (strAssetCategoryId == "1" || strAssetCategoryId == "3" || strAssetCategoryId == "4") // Server, Enclosure
                    {

                        sbBody.Append(strTRstart + strTDstart + "Intended Room:" + strTDend + strSpacerTD);
                        sbBody.Append(strTDstart + dr["Room"].ToString() + strTDend + strTRend);
                        sbBody.Append(strSpacerRow);
                    }
                    if (strAssetCategoryId == "1" || strAssetCategoryId == "3" || strAssetCategoryId == "4") // Server, Enclosure
                    {

                        sbBody.Append(strTRstart + strTDstart + "Intended Zone:" + strTDend + strSpacerTD);
                        sbBody.Append(strTDstart + dr["Zone"].ToString() + strTDend + strTRend);
                        sbBody.Append(strSpacerRow);
                    }
                    if (strAssetCategoryId == "1" || strAssetCategoryId == "3") // Server, Enclosure, 
                    {
                        sbBody.Append(strTRstart + strTDstart + "Intended Rack:" + strTDend + strSpacerTD);
                        sbBody.Append(strTDstart + dr["Rack"].ToString() + strTDend + strTRend);
                        sbBody.Append(strSpacerRow);
                        sbBody.Append(strTRstart + strTDstart + "Intended Rack Position:" + strTDend + strSpacerTD);
                        sbBody.Append(strTDstart + (dr["RackPos"].ToString() == "" ? "Unknown" : dr["RackPos"].ToString()) + strTDend + strTRend);
                        sbBody.Append(strSpacerRow);
                    }

                    if (strAssetCategoryId == "1" || strAssetCategoryId == "2" || strAssetCategoryId == "3")
                    {
                        sbBody.Append(strTRstart + strTDstart + "Intended Resiliency:" + strTDend + strSpacerTD);
                        sbBody.Append(strTDstart + (dr["Resiliency"].ToString() == "" ? "Unknown" : dr["Resiliency"].ToString()) + strTDend + strTRend);
                        sbBody.Append(strSpacerRow);

                        sbBody.Append(strTRstart + strTDstart + "Intended Operating System Group:" + strTDend + strSpacerTD);
                        sbBody.Append(strTDstart + (dr["OperatingSystemGroup"].ToString() == "" ? "Unknown" : dr["OperatingSystemGroup"].ToString()) + strTDend + strTRend);
                        sbBody.Append(strSpacerRow);

                        sbBody.Append(strTRstart + strTDstart + "Intended Class:" + strTDend + strSpacerTD);
                        sbBody.Append(strTDstart + dr["Class"].ToString() + strTDend + strTRend);
                        sbBody.Append(strSpacerRow);

                        sbBody.Append(strTRstart + strTDstart + "Intended Environment:" + strTDend + strSpacerTD);
                        sbBody.Append(strTDstart + dr["Environment"].ToString() + strTDend + strTRend);
                        sbBody.Append(strSpacerRow);
                    }

                    if (strAssetCategoryId == "2")
                    {
                        sbBody.Append(strTRstart + strTDstart + "Intended Enclosure:" + strTDend + strSpacerTD);
                        sbBody.Append(strTDstart + (dr["Enclosure"].ToString() == "" ? "Unknown" : dr["Enclosure"].ToString()) + strTDend + strTRend);
                        sbBody.Append(strSpacerRow);

                        sbBody.Append(strTRstart + strTDstart + "Intended Enclosure Slot:" + strTDend + strSpacerTD);
                        sbBody.Append(strTDstart + (dr["Enclosure"].ToString() == "" ? "Unknown" : dr["EnclosureSlot"].ToString()) + strTDend + strTRend);
                        sbBody.Append(strSpacerRow);
                    }
                }

                sbBody.Append(strTRstart + strTDstart + "Requested Quantity:" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + dr["RequestedQuantity"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "Requested By Date:" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + (dr["RequestedByDate"].ToString() == "" ? "--" : DateTime.Parse(dr["RequestedByDate"].ToString()).ToShortDateString()) + strTDend + strTRend);

                sbBody.Append(strSpacerRow);
                

                if (intOrderType == (int)AssetOrderType.Procure)
                {
                    sbBody.Append(strTRstart + strTDstart + "Procure Quantity:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + dr["ProcureQuantity"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Re-Deploy Quantity:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + dr["ReDeployQuantity"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Returned Quantity:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + dr["ReturnedQuantity"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Requested By Date:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart +(dr["RequestedByDate"].ToString()==""?"--": DateTime.Parse( dr["RequestedByDate"].ToString()).ToShortDateString()) + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Tracking Number:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + dr["VendorTrackingNumber"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Purchase Order Number:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + dr["PurchaseOrderNumber"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Purchase Order Status:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + (dr["PurchaseOrderStatus"].ToString() == "" ? "--" : dr["PurchaseOrderStatus"].ToString()) + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Purchase Order Date:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + (dr["PurchaseOrderDate"].ToString() ==""?"--":DateTime.Parse(dr["PurchaseOrderDate"].ToString()).ToShortDateString()) + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Purchase Order Price(Total Amount):" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + dr["PurchaseOrderPrice"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Vendor Order Status:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + (dr["VendorOrderStatus"].ToString() == "" ? "--" : dr["VendorOrderStatus"].ToString()) + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Vendor Order Date:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + (dr["VendorOrderDate"].ToString()==""?"--":DateTime.Parse(dr["VendorOrderDate"].ToString()).ToShortDateString()) + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Vendor Tracking Number:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + (dr["VendorTrackingNumber"].ToString() == "" ? "--" : dr["VendorTrackingNumber"].ToString()) + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Manufacture Order Number:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + (dr["QuoteNumber"].ToString() == "" ? "--" : dr["QuoteNumber"].ToString()) + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);
                    

                    Users oUser = new Users(intUser, dsnCV);
                    int intAttentionTo = 0;
                    Int32.TryParse(dr["AttentionTo"].ToString(), out intAttentionTo);
                    sbBody.Append(strTRstart + strTDstart + "Attention To:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + (intAttentionTo == 0 ? "--" : oUser.GetFullNameWithLanID(intAttentionTo)) + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Is Clustered:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + (dr["IsClustered"].ToString() == "1" ? "Yes" : (dr["IsClustered"].ToString() == "0" ? "No" : "N / A")) + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "SAN attached:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + (dr["SanAttached"].ToString() == "1" ? "Yes" : (dr["SanAttached"].ToString() == "0" ? "No" : "N / A")) + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Has Boot LUN:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + (dr["IsBootLun"].ToString() == "1" ? "Yes" : (dr["IsBootLun"].ToString() == "0" ? "No" : "N / A")) + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    int Switch1 = 0;
                    Int32.TryParse(dr["Switch1"].ToString(), out Switch1);
                    sbBody.Append(strTRstart + strTDstart + "Switch 1:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + (Switch1 == 0 ? "--" : oAsset.GetSwitch(Switch1, "name")) + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Port 1:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + dr["Port1"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    int Switch2 = 0;
                    Int32.TryParse(dr["Switch2"].ToString(), out Switch2);
                    sbBody.Append(strTRstart + strTDstart + "Switch 2:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + (Switch2 == 0 ? "--" : oAsset.GetSwitch(Switch2, "name")) + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                    sbBody.Append(strTRstart + strTDstart + "Port 2:" + strTDend + strSpacerTD);
                    sbBody.Append(strTDstart + dr["Port2"].ToString() + strTDend + strTRend);
                    sbBody.Append(strSpacerRow);

                }
            }

            //For Decomm -Dispose get the Retrieve special hardware results 
            if (intOrderType == (int)AssetOrderType.Dispose || intOrderType == (int)AssetOrderType.ReDeploy)
            {
                DataSet dsDecomm = oAsset.GetDecommission(_requestid, _number, 2);
                if (dsDecomm.Tables[0].Rows.Count==1)
                {
                    DataRow drDecomm = dsDecomm.Tables[0].Rows[0];
                    if (drDecomm["retrieve"].ToString() == "1")
                    {
                        string strDecommResults = "";
                        strDecommResults = "<b>Description :</b> " + drDecomm["retrieve_description"].ToString() + "<br/>";
                        strDecommResults = "<b>Mailed to Address :</b> " + drDecomm["retrieve_address"].ToString() + "<br/>";
                        strDecommResults = "<b>Mailed to Locator :</b> " + drDecomm["retrieve_locator"].ToString() + "<br/>";

                        sbBody.Append(strTRstart + strTDstart + "Retrieve  Special Hardware:" + strTDend + strSpacerTD);
                        sbBody.Append(strTDstart + strDecommResults + strTDend + strTRend);
                        sbBody.Append(strSpacerRow);
                    }
                }
            }


            if (sbBody.ToString() != "")
            {
                sbBody.Insert(0, "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                sbBody.Append("</table>");

                sbBody.Append(sbBodyModelThresholdAttributes);

                sbBody.Append(GetSelectedAssetsBody(intOrderId));
            }
            return sbBody.ToString();
        }


        public string GetSelectedAssetsBody(int _orderId)
        {
            StringBuilder sbBody = new StringBuilder();
            DataTable dtSelected = null;
            Locations oLocation = new Locations(intUser, dsnCV);
            DataSet dsAssetSelection = GetAssetOrderAssetSelection(_orderId);
            foreach (DataRow drSelected in dsAssetSelection.Tables[0].Rows)
            {
                DataSet dsAsset = oAsset.GetAssetsAll(Int32.Parse(drSelected["assetid"].ToString()));
                if (dtSelected != null)
                {
                    foreach (DataRow drtemp in dsAsset.Tables[0].Rows)
                    {
                        dtSelected.ImportRow(drtemp);
                    }
                }
                else
                    dtSelected = dsAsset.Tables[0];
            }
            string strSort = "AssetSerial";

            DataRow[] drSelect = null;
            if (dtSelected!=null)
            {
                dtSelected.DefaultView.Sort = strSort;
                drSelect = dtSelected.Select();
            }

            if (drSelect != null)
            {
                if (drSelect.Length > 0)
                {
                    string strTRstart = "<tr>";
                    string strTRend = "</tr>";
                    string strTDstart = "<td>";
                    string strTDend = "</td>";

                    sbBody.Append("<tr bgcolor=\"#EEEEEE\" valign=\"top\">");
                    sbBody.Append("<td width=\"10%\">" + "<b>" + "Asset Serial#" + "</b>" + strTDend);
                    sbBody.Append("<td width=\"10%\">" + "<b>" + "AssetTag" + "</b>" + strTDend);
                    sbBody.Append("<td width=\"10%\">" + "<b>" + "Asset Status" + "</b>" + strTDend);
                    sbBody.Append("<td width=\"30%\">" + "<b>" + "Current Location" + "</b>" + strTDend);
                    sbBody.Append("<td width=\"10%\">" + "<b>" + "Current Class" + "</b>" + strTDend);
                    sbBody.Append("<td width=\"10%\">" + "<b>" + "Current Environment" + "</b>" + strTDend);
                    sbBody.Append("<td width=\"10%\">" + "<b>" + "Current Enclosure" + "</b>" + strTDend);
                    sbBody.Append("<td width=\"10%\">" + "<b>" + "Current Enclosure Slot" + "</b>" + strTDend);
                    sbBody.Append("<td width=\"10%\">" + "<b>" + "Current Room" + "</b>" + strTDend);
                    sbBody.Append("<td width=\"10%\">" + "<b>" + "Current Rack" + "</b>" + strTDend);


                    foreach (DataRow dr in drSelect)
                    {
                        sbBody.Append(strTRstart);
                        sbBody.Append(strTDstart + dr["AssetSerial"].ToString() + strTDend);
                        sbBody.Append(strTDstart + dr["AssetTag"].ToString() + strTDend);
                        sbBody.Append(strTDstart + dr["AssetStatus"].ToString() + strTDend);
                        string strLocation = "";
                        if (dr["LocationId"].ToString() != "")
                        {
                            strLocation = oLocation.GetAddress(Int32.Parse(dr["LocationId"].ToString()), "commonname");
                            if (strLocation == "")
                                strLocation = dr["Location"].ToString();
                        }

                        sbBody.Append(strTDstart + strLocation + strTDend);
                        sbBody.Append(strTDstart + dr["Class"].ToString() + strTDend);
                        sbBody.Append(strTDstart + dr["Environment"].ToString() + strTDend);
                        sbBody.Append(strTDstart + dr["Enclosure"].ToString() + strTDend);
                        sbBody.Append(strTDstart + dr["Slot"].ToString() + strTDend);
                        sbBody.Append(strTDstart + dr["Room"].ToString() + strTDend);
                        sbBody.Append(strTDstart + dr["Rack"].ToString() + strTDend);
                        sbBody.Append(strTRend);
                    }

                    sbBody.Append(strTRend);
                    if (sbBody.ToString() != "")
                    {
                        sbBody.Insert(0, "</br></br><b>Selected Assets </b></br></br>");


                        sbBody.Insert(0, "<table border=\"1\" width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" class=\"default\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                        sbBody.Append("</table>");


                    }
                }
            }
            return sbBody.ToString();
        }
        public string GetModelThresholdAttributes(int _modelId)
        {
            StringBuilder sbBody = new StringBuilder();
            string strSpacerRow = "<tr><td colspan=\"2\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";

            string strSpacerTD = "<td style=\"width:10;text-align:left\"></td>";
            string strTRstart = "<tr>";
            string strTRend = "</tr>";
            string strTDstart = "<td>";
            string strTDend = "</td>";

            DataSet ds = oModelProperty.Get(_modelId);
              if (ds.Tables[0].Rows.Count > 0)
              {
                  DataRow dr = ds.Tables[0].Rows[0];

                  sbBody.Append(strTRstart + strTDstart + "Warning:" + strTDend + strSpacerTD);
                  sbBody.Append(strTDstart + dr["StorageThresholdMin"].ToString() + strTDend + strTRend);
                  sbBody.Append(strSpacerRow);

                  sbBody.Append(strTRstart + strTDstart + "Critical:" + strTDend + strSpacerTD);
                  sbBody.Append(strTDstart + dr["StorageThresholdMax"].ToString() + strTDend + strTRend);
                  sbBody.Append(strSpacerRow);

                  sbBody.Append(strTRstart + strTDstart + "Target Impact Date:" + strTDend + strSpacerTD);
                  sbBody.Append(strTDstart + "Fix" + strTDend + strTRend);
                  sbBody.Append(strSpacerRow);

                  sbBody.Append(strTRstart + strTDstart + "Inhouse Quantity" + strTDend + strSpacerTD);
                  sbBody.Append(strTDstart + "Fix" + strTDend + strTRend);
                  sbBody.Append(strSpacerRow);

              }
            if (sbBody.ToString() != "")
            {
                sbBody.Insert(0, "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                sbBody.Append("</table>");
            }

            if (sbBody.ToString() != "")
            {
                
                sbBody.Insert(0, "<fieldset><legend class=\"tableheader\"><b>Model Threshold Attributes</b></legend>");
                sbBody.Append("</fieldset>");
            }

            return sbBody.ToString();


        }


        public DataSet GetAssetOrderResourceRequests(int _OrderId, int _Number, int _ServiceId)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            arParams[1] = new SqlParameter("@Number", _Number);
            arParams[2] = new SqlParameter("@ServiceId", _ServiceId);
            return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetResourceRequests", arParams);
        }

        public DataSet GetAssetOrderResourceRequestsByRequest(int _Request, int _Number)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@RequestId", _Request);
            arParams[1] = new SqlParameter("@Number", _Number);
            return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetResourceRequests", arParams);
        }
        public DataSet GetAssetOrderResourceRequests(int _OrderId, int _Number)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            arParams[1] = new SqlParameter("@Number", _Number);
            return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetResourceRequests", arParams);
        }
        public void InitiateNextServiceRequestOrCompleteRequest(int _OrderId,int _Number, int _CompletedServiceId, bool boolUpdateAssetWithOrder, string dsnServiceEditor, int intAssignPage, int intViewPage, string dsnAsset, string dsnIP)
        {
            Requests oRequest = new Requests(intUser, dsnCV);
            ResourceRequest oResourceRequest = new ResourceRequest(intUser, dsnCV);
            Services oService = new Services(intUser, dsnCV);
            ServiceRequests oServiceRequest = new ServiceRequests(intUser, dsnCV);
            ServiceDetails oServiceDetail = new ServiceDetails(intUser, dsnCV);
            AssetCategoryDeploymentConfig oAssetCategoryDeploymentConfig = new AssetCategoryDeploymentConfig(intUser, dsnCV);

            DataRow drOrder=Get(_OrderId).Tables[0].Rows[0];
            DataSet dsAssetCategoryDeployConfig = oAssetCategoryDeploymentConfig.getsByAssetCategoryAndOrderType(
                                                                Int32.Parse(drOrder["AssetCategoryId"].ToString()),
                                                                Int32.Parse(drOrder["OrderType"].ToString()),
                                                                1);
     
            int intRequestId = Int32.Parse(drOrder["RequestId"].ToString());
            int intServiceId=0;
            int intItemId=0;
            string strCustomName="";
            bool boolAllStepCompleted = true;
            if (boolUpdateAssetWithOrder==true)
            {
               if (Int32.Parse(drOrder["OrderType"].ToString())==(int)AssetOrderType.ReDeploy ||
                         Int32.Parse(drOrder["OrderType"].ToString())==(int)AssetOrderType.Movement ||
                         Int32.Parse(drOrder["OrderType"].ToString())==(int)AssetOrderType.Dispose)
               {
                    //1. For Selected Assets => Update New OrderId
                    DataSet dsAssetSelected = GetAssetOrderAssetSelection(_OrderId);
                    foreach (DataRow dr in dsAssetSelected.Tables[0].Rows)
                    {
                        oAsset.updateNewOrderId(_OrderId, Int32.Parse(dr["AssetId"].ToString()));
                    }

                    //2. If it's Re-Deployment clear the asset configuration
                    if (Int32.Parse(drOrder["OrderType"].ToString()) == (int)AssetOrderType.ReDeploy)
                        ClearAssetConfiguration(_OrderId, intUser);

                    //3. If it's Asset Movement set Asset Attribute =Moving
                    if (Int32.Parse(drOrder["OrderType"].ToString()) == (int)AssetOrderType.Movement)
                        oAsset.UpdateAllAssetAttributeForOrder(_OrderId, (int)AssetAttribute.Moving, intUser);

               }
            }


             foreach (DataRow drConfig in dsAssetCategoryDeployConfig.Tables[0].Rows)
             {
                    DataSet dsRR = GetAssetOrderResourceRequests(_OrderId,_Number, Int32.Parse(drConfig["ServiceId"].ToString()));

                     if (dsRR.Tables[0].Rows.Count>0)  //Check if Service Initiated
                     {   DataRow drRR = dsRR.Tables[0].Rows[0];
                         if (drRR["RRStatus"].ToString() != "3")
                         {
                             boolAllStepCompleted = false;
                             break;
                         }
                         if (_CompletedServiceId == Int32.Parse(drConfig["ServiceId"].ToString()))
                         {
                             if (drConfig["IsAssetStatusChangeApplicable"].ToString() == "1" && Int32.Parse(drConfig["AssetStatusOut"].ToString()) != -999)
                                 oAsset.UpdateAllAssetStatusForOrder(_OrderId, "", Int32.Parse(drConfig["AssetStatusOut"].ToString()), intUser);
                         } 
                     }
                     else    //Initiate the Service
                     {
                         intServiceId = Int32.Parse(drConfig["ServiceId"].ToString());
                         if (oResourceRequest.GetAllService(intRequestId, intServiceId, _Number).Tables[0].Rows.Count == 0)
                         {
                             strCustomName = drConfig["CustomTaskName"].ToString();
                             intItemId = oService.GetItemId(intServiceId);

                             //For Before Starting Service => Update Assets Status Based on configuration
                             if (drConfig["IsAssetStatusChangeApplicable"].ToString() == "1" && Int32.Parse(drConfig["AssetStatusIn"].ToString()) != -999)
                                 oAsset.UpdateAllAssetStatusForOrder(_OrderId, "", Int32.Parse(drConfig["AssetStatusIn"].ToString()), intUser);

                             double dblServiceHours = oServiceDetail.GetHours(intServiceId, 1);
                             int intResource = oServiceRequest.AddRequest(intRequestId, intItemId, intServiceId, 1, dblServiceHours, 2, _Number, dsnServiceEditor);
                             oServiceRequest.Update(intRequestId, strCustomName);
                             oResourceRequest.UpdateName(intResource, strCustomName);
                             oServiceRequest.Add(intRequestId, 1, 1);
                             oServiceRequest.NotifyTeamLead(intItemId, intResource, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                         }
                         boolAllStepCompleted = false;
                         break;
                     
                     }
             }

             if (boolAllStepCompleted == true)
             {
                 if (Int32.Parse(drOrder["OrderType"].ToString()) == (int)AssetOrderType.Movement)
                     oAsset.UpdateAllAssetAttributeForOrder(_OrderId, (int)AssetAttribute.Ok, intUser);

                 UpdateOrderStatus(_OrderId, (int)AssestOrderReqStatus.Completed, intUser);
             }


        }

        public DataSet GetActiveAssetOrderAssets(int _ModelId)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@ModelId", _ModelId);
            return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getActiveAssetOrderAssets", arParams);
        }

        #region Clear Existing Asset Configuration
        
        public void ClearAssetConfiguration(int _OrderId, int _userId)
            {

                DataSet ds = oAsset.GetAssetsByOrder(_OrderId);
                foreach (DataRow drAsset in ds.Tables[0].Rows)
                {
                    string strAssetCategory = "";
                    strAssetCategory = oModelProperty.Get(Int32.Parse(drAsset["modelid"].ToString()), "asset_category");
                    DataSet dsAssetCurrent = oAsset.GetAssetsAll(Int32.Parse(drAsset["id"].ToString()));
                    foreach (DataRow drAssetCurrent in dsAssetCurrent.Tables[0].Rows)
                    {
                        switch (strAssetCategory)
                        {
                            case "1":  //Physical Server   
                                //Delete current record
                                oAsset.DeleteServer(Int32.Parse(drAsset["id"].ToString()), false, _userId);
                                //Add new record >>Clear VLAN ,Build Network
                                oAsset.AddServer(Int32.Parse(drAsset["id"].ToString()),
                                                (int)AssetStatus.InStock, _userId, DateTime.Now,
                                                Int32.Parse(drAssetCurrent["ClassId"].ToString()),
                                                Int32.Parse(drAssetCurrent["EnvironmentId"].ToString()),
                                                //Int32.Parse(drAssetCurrent["LocationId"].ToString()),
                                                //Int32.Parse(drAssetCurrent["RoomId"].ToString()),
                                                Int32.Parse(drAssetCurrent["RackId"].ToString()),
                                                drAssetCurrent["RackPosition"].ToString(), drAssetCurrent["ILO"].ToString(),
                                                drAssetCurrent["DummyName"].ToString(),
                                                drAssetCurrent["MacAddress"].ToString(),
                                                0, 0,
                                                Int32.Parse(drAssetCurrent["resiliencyid"].ToString()),
                                                Int32.Parse(drAssetCurrent["operatingsystemgroupid"].ToString()));

                                break;
                            case "2": //Blade 
                                //Delete current record
                                oAsset.DeleteBlade(Int32.Parse(drAsset["id"].ToString()), false, _userId);
                                //Add new record >> Clear WWP names, VLAN, MAC, Build Network, Build Network Alias
                                oAsset.AddBlade(Int32.Parse(drAsset["id"].ToString()),
                                                (int)AssetStatus.InStock, _userId, DateTime.Now,
                                                Int32.Parse((drAssetCurrent["EnclosureId"].ToString() == "" ? "0" : drAssetCurrent["EnclosureId"].ToString())),
                                                Int32.Parse(drAssetCurrent["ClassId"].ToString()),
                                                Int32.Parse(drAssetCurrent["EnvironmentId"].ToString()),
                                                drAssetCurrent["ILO"].ToString(),
                                                drAssetCurrent["DummyName"].ToString(),
                                                "", 0, 0,
                                                Int32.Parse((drAssetCurrent["Slot"].ToString() == "" ? "0" : drAssetCurrent["Slot"].ToString())),
                                                Int32.Parse((drAssetCurrent["Spare"].ToString() == "" ? "0" : drAssetCurrent["Spare"].ToString())),
                                                Int32.Parse(drAssetCurrent["resiliencyid"].ToString()),
                                                Int32.Parse(drAssetCurrent["operatingsystemgroupid"].ToString()));

                                //Delete the HBA's
                                DataSet dsHBA = oAsset.GetHBA(Int32.Parse(drAsset["id"].ToString()));
                                foreach (DataRow drHBA in dsHBA.Tables[0].Rows)
                                {
                                    oAsset.DeleteHBA(Int32.Parse(drHBA["id"].ToString()));
                                }


                                break;
                            default:

                                oAsset.UpdateStatus(Int32.Parse(drAsset["id"].ToString()), "", (int)AssetStatus.InStock, _userId, DateTime.Now);

                                break;
                        }
                    }
                }
                
            }

        #endregion

        #region Asset Staging And Configuration
            public bool IsAssetStagedAndConfigured(int _OrderId, int _assetId, ref string _comments)
            {
                //This function checks the staging and configuration for Server, blade, Enclosure and Racks only.

                string strAssetCategoryId = "";
                int intModelPropertyModelId = 0;
                int intModelTypeId = 0;
                int intRequiredHBACount = 1;
                DataSet dsOrder = Get(_OrderId);
                bool boolValidateSlot = false;
                if (dsOrder.Tables[0].Rows.Count == 1)
                {
                    DataRow drOrder = dsOrder.Tables[0].Rows[0];

                    DataSet dsAsset = oAsset.GetAssetsAll(_assetId);
                    if (dsAsset.Tables[0].Rows.Count == 1)
                    {
                        DataRow drAsset = dsAsset.Tables[0].Rows[0];
                        int intModelId = Int32.Parse(drOrder["ModelId"].ToString());
                        strAssetCategoryId = oModelProperty.Get(intModelId, "asset_category");
                        bool IsDell = oModelProperty.IsDell(intModelId);

                        if (drAsset["AssetModelId"].ToString() == "") _comments += "Model, " + "\n";
                        if (drAsset["AssetModelId"].ToString() != drOrder["ModelId"].ToString()) _comments += "Model is not as per the request, " + "\n";
                        if (drAsset["AssetSerial"].ToString() == "") _comments += "Serial# not found, " + "\n";
                        if (drAsset["AssetTag"].ToString() == "") _comments += "Asset Tag not found, " + "\n";
                        if (drAsset["AssetModelId"].ToString() != "")
                        {
                            intModelPropertyModelId = Int32.Parse(oModelProperty.Get(Int32.Parse(drAsset["AssetModelId"].ToString()), "modelid"));
                            intModelTypeId = Int32.Parse(oModel.Get(intModelPropertyModelId, "TypeId"));
                        }
                        int intLocationId = 0;
                        Int32.TryParse(drOrder["LocationId"].ToString(), out intLocationId);
                        if (intLocationId == 0)
                            Int32.TryParse(drAsset["LocationId"].ToString(), out intLocationId);
                        if (strAssetCategoryId == "1")//Physical Server
                        {
                            if (drAsset["LocationId"].ToString() == "" || drAsset["LocationId"].ToString() != drOrder["LocationId"].ToString())
                                _comments += "Location, " + "\n";


                            if (drAsset["RoomId"].ToString() == "" || drAsset["RoomId"].ToString() == "0")
                                _comments += "Room, " + "\n";
                            else if (drOrder["RoomId"].ToString() != "0" && drOrder["Room"].ToString().ToUpper().Contains("UNKNOWN") == false && drOrder["RoomId"].ToString() != drAsset["RoomId"].ToString())
                                _comments += "Room, " + "\n";

                            if (drAsset["RackId"].ToString() == "" || drAsset["RackId"].ToString() == "0")
                                _comments += "Rack, " + "\n";
                            else if (drOrder["RackId"].ToString() != "0" && drOrder["Rack"].ToString().ToUpper().Contains("UNKNOWN") == false && drOrder["RackId"].ToString() != drAsset["RackId"].ToString())
                                _comments += "Rack, " + "\n";

                            if (drAsset["RackPosition"].ToString() == "")
                                _comments += "Rack Position, " + "\n";
                            else if (drOrder["RackPos"].ToString() != "" && drOrder["RackPos"].ToString() != drAsset["RackPosition"].ToString())
                                _comments += "Rack Position, " + "\n";

                            if (drAsset["ClassId"].ToString() == "" || drAsset["ClassId"].ToString() != drOrder["ClassId"].ToString())
                                _comments += "Class, " + "\n";

                            if (drAsset["EnvironmentId"].ToString() == "" || drAsset["EnvironmentId"].ToString() != drOrder["EnvironmentId"].ToString())
                                _comments += "Environment, " + "\n";


                            if (drAsset["ILO"].ToString() == "")
                                _comments += "ILO, " + "\n";
                            else
                            {
                                Regex regExpIpAddress = new Regex(strIPAddressPattern);
                                if (regExpIpAddress.IsMatch(drAsset["ilo"].ToString(), 0) == false)
                                    _comments += "Valid ILO, " + "\n";
                            }

                            if (drAsset["DummyName"].ToString() == "")
                                _comments += "Dummy Name, " + "\n";

                            //if (drAsset["MacAddress"].ToString() == "")
                            //    _comments += "MAC Address, " + "\n";
                            //else
                            //{
                            //    Regex regExpMacAddress = new Regex(strMACAddressPattern);
                            //    if (regExpMacAddress.IsMatch(drAsset["MacAddress"].ToString(), 0) == false)
                            //        _comments += "Valid MAC Address, " + "\n";
                            //}


                            if (drAsset["vLAN"].ToString() == "" || drAsset["vLAN"].ToString() == "0")
                                _comments += "VLAN, " + "\n";

                            if (intModelTypeId == 15)
                            {
                                if (drAsset["BuildNetworkId"].ToString() == "" || drAsset["BuildNetworkId"].ToString() == "0")
                                    _comments += "Build Network, " + "\n";
                            }

                            if (drAsset["ResiliencyId"].ToString() == "" || drAsset["ResiliencyId"].ToString() == "0")
                                _comments += "Resiliency, " + "\n";
                            else if (drOrder["ResiliencyId"].ToString() != "-1" && drOrder["ResiliencyId"].ToString() != drAsset["ResiliencyId"].ToString())
                                _comments += "Resiliency, " + "\n";

                            if (drAsset["OperatingSystemGroupId"].ToString() == "")
                                _comments += "Operating System Group, " + "\n";
                            else if (drOrder["OperatingSystemGroupId"].ToString() != "-1" && drOrder["OperatingSystemGroupId"].ToString() != "0" && drOrder["OperatingSystemGroupId"].ToString() != drAsset["OperatingSystemGroupId"].ToString())
                                _comments += "Operating System Group, " + "\n";

                            if (IsDell == false && oAsset.GetHBA(_assetId).Tables[0].Rows.Count < intRequiredHBACount)
                                _comments += "World Wide Port Names, " + "\n";

                        }


                        else if (strAssetCategoryId == "2") //Blade
                        {
                            if (drOrder["Enclosure"].ToString() != "" && (drAsset["LocationId"].ToString() == "" || drAsset["LocationId"].ToString() != drOrder["LocationId"].ToString()))
                                _comments += "Location, " + "\n";

                            if (drAsset["ClassId"].ToString() == "" || drAsset["ClassId"].ToString() != drOrder["ClassId"].ToString())
                                _comments += "Class, " + "\n";

                            if (drAsset["EnvironmentId"].ToString() == "" || drAsset["EnvironmentId"].ToString() != drOrder["EnvironmentId"].ToString())
                                _comments += "Environment, " + "\n";

                            if (drOrder["Enclosure"].ToString() != "" && (drAsset["EnclosureId"].ToString() == "" || drAsset["EnclosureId"].ToString() != drOrder["EnclosureId"].ToString()))
                                _comments += "Enclosure, " + "\n";

                            if (boolValidateSlot == true)
                            {
                                if (drOrder["Enclosure"].ToString() != "" && (drAsset["Slot"].ToString() == "" || drAsset["Slot"].ToString() != drOrder["EnclosureSlot"].ToString()))
                                    _comments += "Slot, " + "\n";
                            }
                            else
                            {
                                if (drAsset["Slot"].ToString() == "")
                                    _comments += "Slot, " + "\n";
                            }

                            if (drAsset["ILO"].ToString() == "")
                                _comments += "ILO, " + "\n";
                            else
                            {
                                Regex regExpIpAddress = new Regex(strIPAddressPattern);
                                if (regExpIpAddress.IsMatch(drAsset["ilo"].ToString(), 0) == false)
                                    _comments += "Valid ILO, " + "\n";
                            }

                            if (drAsset["DummyName"].ToString() == "")
                                _comments += "Dummy Name, " + "\n";

                            //if (drAsset["MacAddress"].ToString() == "")
                            //    _comments += "MAC Address, " + "\n";
                            //else
                            //{
                            //    Regex regExpMacAddress = new Regex(strMACAddressPattern);
                            //    if (regExpMacAddress.IsMatch(drAsset["macaddress"].ToString(), 0) == false)
                            //        _comments += "Valid MAC Address, " + "\n";
                            //}

                            if (drAsset["vLAN"].ToString() == "" || drAsset["vLAN"].ToString() == "0")
                                _comments += "VLAN, " + "\n";

                            if (intModelTypeId == 15)
                            {
                                if (drAsset["BuildNetworkId"].ToString() == "" || drAsset["BuildNetworkId"].ToString() == "0")
                                    _comments += "Build Network, " + "\n";
                            }

                            if (drAsset["ResiliencyId"].ToString() == "" || drAsset["ResiliencyId"].ToString() == "0")
                                _comments += "Resiliency, " + "\n";
                            else if (drOrder["ResiliencyId"].ToString() != "-1" && drOrder["ResiliencyId"].ToString() != drAsset["ResiliencyId"].ToString())
                                _comments += "Resiliency, " + "\n";

                            if (drAsset["OperatingSystemGroupId"].ToString() == "")
                                _comments += "Operating System Group, " + "\n";
                            else if (drOrder["OperatingSystemGroupId"].ToString() != "-1" && drOrder["OperatingSystemGroupId"].ToString() != "0" && drOrder["OperatingSystemGroupId"].ToString() != drAsset["OperatingSystemGroupId"].ToString())
                                _comments += "Operating System Group, " + "\n";

                            if (IsDell == false && oAsset.GetHBA(_assetId).Tables[0].Rows.Count < intRequiredHBACount)
                                _comments += "World Wide Port Names, " + "\n";


                        }
                        else if (strAssetCategoryId == "3") //Enclosure
                        {
                            if (drAsset["AssetName"].ToString() == "" )
                                _comments += "Device Name, " + "\n";

                            if (drAsset["LocationId"].ToString() == "" || drAsset["LocationId"].ToString() != drOrder["LocationId"].ToString())
                                _comments += "Location, " + "\n";

                            if (drAsset["RoomId"].ToString() == "" || drAsset["RoomId"].ToString() == "0")
                                _comments += "Room, " + "\n";
                            else if (drOrder["RoomId"].ToString() != "0" && drOrder["Room"].ToString().ToUpper().Contains("UNKNOWN") == false && drOrder["RoomId"].ToString() != drAsset["RoomId"].ToString())
                                _comments += "Room, " + "\n";

                            if (drAsset["RackId"].ToString() == "" || drAsset["RackId"].ToString() == "0")
                                _comments += "Rack, " + "\n";
                            else if (drOrder["RackId"].ToString() != "0" && drOrder["Rack"].ToString().ToUpper().Contains("UNKNOWN") == false && drOrder["RackId"].ToString() != drAsset["RackId"].ToString())
                                _comments += "Rack, " + "\n";

                            if (drAsset["RackPosition"].ToString() == "")
                                _comments += "Rack position, " + "\n";

                            if (drAsset["vLAN"].ToString() == "" || drAsset["vlan"].ToString() == "0")
                                _comments += "VLAN, " + "\n";

                            if (drAsset["OAIP"].ToString() == "")
                                _comments += "On Board Administration IP Address, " + "\n";
                            else
                            {
                                Regex regExpIpAddress = new Regex(strIPAddressPattern);
                                if (regExpIpAddress.IsMatch(drAsset["OAIP"].ToString(), 0) == false)
                                    _comments += "Valid On Board Administration IP Address, " + "\n";
                            }

                        }
                        else if (strAssetCategoryId == "4") //Rack
                        {
                            if (drAsset["Rack"].ToString() == "")
                                _comments += "Rack Name, " + "\n";

                            if (drAsset["RoomId"].ToString() == "" || drAsset["RoomId"].ToString() == "0")
                                _comments += "Room," + "\n";
                            else if (drOrder["RoomId"].ToString() != "0" && drOrder["Room"].ToString().ToUpper().Contains("UNKNOWN") == false && drOrder["RoomId"].ToString() != drAsset["RoomId"].ToString())
                                _comments += "Room, " + "\n";

                        }

                        DataSet dsSwitchesNetwork = oAsset.GetSwitchports(_assetId, SwitchPortType.Network);
                        Locations oLocation = new Locations(0, dsnCV);
                        if (oModelProperty.IsConfigureSwitches(intModelId) && dsSwitchesNetwork.Tables[0].Rows.Count == 0 && oLocation.IsOffsite(intLocationId) == false)
                            _comments += "Switchport Configuration, " + "\n";

                        ////
                        if (_comments != "")
                        {
                            string[] strsplit ={ "," };
                            _comments = _comments.Trim().Substring(0, _comments.Trim().Length - 1);
                            _comments = "Please update the following information..." + "\n" + _comments;
                            return false;
                        }
                        else return true;

                    }
                    else if (dsAsset.Tables[0].Rows.Count == 0)
                    {
                        _comments = "Asset not found !";
                        return false;
                    }

                }

                return false;


            }
        #endregion

        #region Asset Order comments
            public int AddUpdateAssetOrderComment(int _Id,int _OrderId,string _comments,int _userId,int _deleted)
            {
                 arParams = new SqlParameter[6];
                arParams[0] = new SqlParameter("@Id", _Id);
                arParams[1] = new SqlParameter("@OrderId", _OrderId);
                arParams[2] = new SqlParameter("@Comments", _comments);
                arParams[3] = new SqlParameter("@CreatedBy", _userId);
                arParams[4] = new SqlParameter("@ModifiedBy", _userId);
                arParams[5] = new SqlParameter("@Deleted", _deleted);
                SqlHelper.ExecuteNonQuery(dsnCV, CommandType.StoredProcedure, "pr_addupdateAssetOrderComment", arParams);
                arParams[0].Direction = ParameterDirection.Output;
                return Int32.Parse(arParams[0].Value.ToString());

            }
            public int DeleteAssetOrderComment(int _Id, int _userId)
            {

                arParams = new SqlParameter[6];
                arParams[0] = new SqlParameter("@Id", _Id);
                arParams[1] = new SqlParameter("@ModifiedBy", _userId);
                SqlHelper.ExecuteNonQuery(dsnCV, CommandType.StoredProcedure, "pr_deleteAssetOrderComment", arParams);
                arParams[0].Direction = ParameterDirection.Output;
                return Int32.Parse(arParams[0].Value.ToString());

            }
            public DataSet GetAssetOrderComments(int _Id, int _OrderId)
            {
                arParams = new SqlParameter[3];
                arParams[0] = new SqlParameter("@Id", _Id);
                arParams[1] = new SqlParameter("@OrderId", _OrderId);
                return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetOrderComments", arParams);
            }
            public DataSet GetAssetOrderComments(int _OrderId)
            {
                arParams = new SqlParameter[3];
                arParams[0] = new SqlParameter("@OrderId", _OrderId);
                return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetOrderComments", arParams);
            }
        #endregion

        #region Asset Order - Asset Selection
        public void AddRemoveAssetOrderAssetSelection(int _OrderId,int _AssetId,int _userId,int _AddRemove)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            arParams[1] = new SqlParameter("@AssetId", _AssetId);
            arParams[2] = new SqlParameter("@CreatedBy", _userId);
            arParams[3] = new SqlParameter("@ModifiedBy", _userId);
            arParams[4] = new SqlParameter("@AddRemove", _AddRemove);
            SqlHelper.ExecuteNonQuery(dsnCV, CommandType.StoredProcedure, "pr_addremoveAssetOrderAssetSelection", arParams);
        }

        public void DeleteAssetOrderAssetSelection(int _OrderId)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            SqlHelper.ExecuteNonQuery(dsnCV, CommandType.StoredProcedure, "pr_deleteAssetOrderAssetSelection", arParams);
        }
        public void DeleteAssetOrderAssetSelection(int _OrderId, int _AssetId)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            arParams[1] = new SqlParameter("@AssetId", _AssetId);
            SqlHelper.ExecuteNonQuery(dsnCV, CommandType.StoredProcedure, "pr_deleteAssetOrderAssetSelections", arParams);
        }

        public DataSet GetAssetOrderAssetSelection(int _OrderId)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@OrderId", _OrderId);
            return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetOrderAssetSelection", arParams);
        }

        public DataSet GetAssetOrderAssetSelection(int _OrderId, int _AssetId)
        {
                arParams = new SqlParameter[10];
                arParams[0] = new SqlParameter("@OrderId", _OrderId);
                arParams[1] = new SqlParameter("@AssetId", _AssetId);
                return SqlHelper.ExecuteDataset(dsnCV, CommandType.StoredProcedure, "pr_getAssetOrderAssetSelection", arParams);
            }
        #endregion
        }
        
}
