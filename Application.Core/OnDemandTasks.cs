using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;
using System.Text;

namespace NCC.ClearView.Application.Core
{
	public class OnDemandTasks
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public OnDemandTasks(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public void AddPending(int _answerid, int _resourceid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@resourceid", _resourceid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOnDemandPending", arParams);
        }
        public DataSet GetPending(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskPending", arParams);
        }
        public void DeletePending(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOnDemandPending", arParams);
        }

        #region NEW II
        //public void AddII(int _requestid, int _itemid, int _number, int _answerid, int _modelid)
        //{
        //    arParams = new SqlParameter[5];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    arParams[3] = new SqlParameter("@answerid", _answerid);
        //    arParams[4] = new SqlParameter("@modelid", _modelid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOnDemandTaskII", arParams);
        //}
        //public void DeleteII(int _answerid)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@answerid", _answerid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOnDemandTaskII", arParams);
        //}
        //public void UpdateIIStep(int _requestid, int _itemid, int _number, int _step)
        //{
        //    arParams = new SqlParameter[4];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    arParams[3] = new SqlParameter("@step", _step);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskII", arParams);
        //}
        //public void UpdateIIComplete(int _requestid, int _itemid, int _number)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskIIComplete", arParams);
        //}
        //public void UpdateIINotifications(int _requestid, int _itemid, int _number)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskIINotifications", arParams);
        //}
        //public DataSet GetII(int _requestid, int _itemid, int _number)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskII", arParams);
        //}
        #endregion

        #region Physical
        //public void AddPhysicalII(int _requestid, int _itemid, int _number, int _answerid, int _modelid)
        //{
        //    arParams = new SqlParameter[5];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    arParams[3] = new SqlParameter("@answerid", _answerid);
        //    arParams[4] = new SqlParameter("@modelid", _modelid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOnDemandTaskPhysicalII", arParams);
        //}
        //public void DeletePhysicalII(int _answerid)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@answerid", _answerid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOnDemandTaskPhysicalII", arParams);
        //}
        //public void UpdatePhysicalII(int _requestid, int _itemid, int _number, int _chk1, int _chk3, int _chk4, int _chk5, int _chk6, int _chk7, int _chk8, int _chk9, int _chk10, int _chk11, int _chk12, int _chk13, int _chk14, int _chk15)
        //{
        //    arParams = new SqlParameter[17];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    arParams[3] = new SqlParameter("@chk1", _chk1);
        //    arParams[4] = new SqlParameter("@chk3", _chk3);
        //    arParams[5] = new SqlParameter("@chk4", _chk4);
        //    arParams[6] = new SqlParameter("@chk5", _chk5);
        //    arParams[7] = new SqlParameter("@chk6", _chk6);
        //    arParams[8] = new SqlParameter("@chk7", _chk7);
        //    arParams[9] = new SqlParameter("@chk8", _chk8);
        //    arParams[10] = new SqlParameter("@chk9", _chk9);
        //    arParams[11] = new SqlParameter("@chk10", _chk10);
        //    arParams[12] = new SqlParameter("@chk11", _chk11);
        //    arParams[13] = new SqlParameter("@chk12", _chk12);
        //    arParams[14] = new SqlParameter("@chk13", _chk13);
        //    arParams[15] = new SqlParameter("@chk14", _chk14);
        //    arParams[16] = new SqlParameter("@chk15", _chk15);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskPhysicalII", arParams);
        //}
        //public void UpdatePhysicalIIStep(int _requestid, int _itemid, int _number, int _step)
        //{
        //    arParams = new SqlParameter[4];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    arParams[3] = new SqlParameter("@step", _step);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskPhysicalIIStep", arParams);
        //}
        //public void UpdatePhysicalIIProd(int _requestid, int _itemid, int _number)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskPhysicalIIProd", arParams);
        //}
        //public void UpdatePhysicalIIComplete(int _requestid, int _itemid, int _number)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskPhysicalIIComplete", arParams);
        //}
        //public void UpdatePhysicalIINotifications(int _requestid, int _itemid, int _number)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskPhysicalIINotifications", arParams);
        //}
        //public DataSet GetPhysicalII(int _requestid, int _itemid, int _number)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskPhysicalII", arParams);
        //}
        #endregion

        #region Blade
        //public void AddBladeII(int _requestid, int _itemid, int _number, int _answerid, int _modelid)
        //{
        //    arParams = new SqlParameter[5];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    arParams[3] = new SqlParameter("@answerid", _answerid);
        //    arParams[4] = new SqlParameter("@modelid", _modelid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOnDemandTaskBladeII", arParams);
        //}
        //public void DeleteBladeII(int _answerid)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@answerid", _answerid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOnDemandTaskBladeII", arParams);
        //}
        //public void UpdateBladeII(int _requestid, int _itemid, int _number, int _chk1, int _chk3, int _chk4, int _chk5, int _chk6, int _chk7, int _chk8, int _chk9, int _chk10, int _chk11, int _chk12)
        //{
        //    arParams = new SqlParameter[14];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    arParams[3] = new SqlParameter("@chk1", _chk1);
        //    arParams[4] = new SqlParameter("@chk3", _chk3);
        //    arParams[5] = new SqlParameter("@chk4", _chk4);
        //    arParams[6] = new SqlParameter("@chk5", _chk5);
        //    arParams[7] = new SqlParameter("@chk6", _chk6);
        //    arParams[8] = new SqlParameter("@chk7", _chk7);
        //    arParams[9] = new SqlParameter("@chk8", _chk8);
        //    arParams[10] = new SqlParameter("@chk9", _chk9);
        //    arParams[11] = new SqlParameter("@chk10", _chk10);
        //    arParams[12] = new SqlParameter("@chk11", _chk11);
        //    arParams[13] = new SqlParameter("@chk12", _chk12);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskBladeII", arParams);
        //}
        //public void UpdateBladeIIProd(int _requestid, int _itemid, int _number)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskBladeIIProd", arParams);
        //}
        //public void UpdateBladeIIMedia(int _requestid, int _itemid, int _number, int _TSM, int _SAN)
        //{
        //    arParams = new SqlParameter[5];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    arParams[3] = new SqlParameter("@TSM", _TSM);
        //    arParams[4] = new SqlParameter("@SAN", _SAN);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskBladeIIMedia", arParams);
        //}
        //public void UpdateBladeIIComplete(int _requestid, int _itemid, int _number)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskBladeIIComplete", arParams);
        //}
        //public void UpdateBladeIINotifications(int _requestid, int _itemid, int _number)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskBladeIINotifications", arParams);
        //}
        //public DataSet GetBladeII(int _requestid, int _itemid, int _number)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskBladeII", arParams);
        //}
        #endregion

        #region VMware
        //public void AddVMWareII(int _requestid, int _itemid, int _number, int _answerid, int _modelid)
        //{
        //    arParams = new SqlParameter[5];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    arParams[3] = new SqlParameter("@answerid", _answerid);
        //    arParams[4] = new SqlParameter("@modelid", _modelid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOnDemandTaskVMWareII", arParams);
        //}
        //public void DeleteVMWareII(int _answerid)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@answerid", _answerid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOnDemandTaskVMWareII", arParams);
        //}
        //public void UpdateVMWareII(int _requestid, int _itemid, int _number, int _chk1, int _chk3, int _chk4, int _chk5, int _chk6, int _chk7, int _chk8, int _chk9, int _chk10, int _chk11)
        //{
        //    arParams = new SqlParameter[13];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    arParams[3] = new SqlParameter("@chk1", _chk1);
        //    arParams[4] = new SqlParameter("@chk3", _chk3);
        //    arParams[5] = new SqlParameter("@chk4", _chk4);
        //    arParams[6] = new SqlParameter("@chk5", _chk5);
        //    arParams[7] = new SqlParameter("@chk6", _chk6);
        //    arParams[8] = new SqlParameter("@chk7", _chk7);
        //    arParams[9] = new SqlParameter("@chk8", _chk8);
        //    arParams[10] = new SqlParameter("@chk9", _chk9);
        //    arParams[11] = new SqlParameter("@chk10", _chk10);
        //    arParams[12] = new SqlParameter("@chk11", _chk11);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskVMWareII", arParams);
        //}
        //public void UpdateVMWareIIProd(int _requestid, int _itemid, int _number)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskVMWareIIProd", arParams);
        //}
        //public void UpdateVMWareIIComplete(int _requestid, int _itemid, int _number)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskVMWareIIComplete", arParams);
        //}
        //public void UpdateVMWareIINotifications(int _requestid, int _itemid, int _number)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskVMWareIINotifications", arParams);
        //}
        //public DataSet GetVMWareII(int _requestid, int _itemid, int _number)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@itemid", _itemid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskVMWareII", arParams);
        //}
        #endregion

        #region Generic
        public void AddGenericII(int _requestid, int _itemid, int _number, int _answerid, int _modelid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@answerid", _answerid);
            arParams[4] = new SqlParameter("@modelid", _modelid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOnDemandTaskGenericII", arParams);
        }
        public void DeleteGenericII(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOnDemandTaskGenericII", arParams);
        }
        public void UpdateGenericII(int _requestid, int _itemid, int _number, string _chk1, string _chk3, string _chk4, string _chk5, string _chk6, string _chk7, string _chk8, string _chk9, string _chk10, string _chk11, string _chk12, string _chk13, string _chk14, string _chk15)
        {
            arParams = new SqlParameter[17];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@chk1", (_chk1 == "" ? SqlDateTime.Null : DateTime.Parse(_chk1)));
            arParams[4] = new SqlParameter("@chk3", (_chk3 == "" ? SqlDateTime.Null : DateTime.Parse(_chk3)));
            arParams[5] = new SqlParameter("@chk4", (_chk4 == "" ? SqlDateTime.Null : DateTime.Parse(_chk4)));
            arParams[6] = new SqlParameter("@chk5", (_chk5 == "" ? SqlDateTime.Null : DateTime.Parse(_chk5)));
            arParams[7] = new SqlParameter("@chk6", (_chk6 == "" ? SqlDateTime.Null : DateTime.Parse(_chk6)));
            arParams[8] = new SqlParameter("@chk7", (_chk7 == "" ? SqlDateTime.Null : DateTime.Parse(_chk7)));
            arParams[9] = new SqlParameter("@chk8", (_chk8 == "" ? SqlDateTime.Null : DateTime.Parse(_chk8)));
            arParams[10] = new SqlParameter("@chk9", (_chk9 == "" ? SqlDateTime.Null : DateTime.Parse(_chk9)));
            arParams[11] = new SqlParameter("@chk10", (_chk10 == "" ? SqlDateTime.Null : DateTime.Parse(_chk10)));
            arParams[12] = new SqlParameter("@chk11", (_chk11 == "" ? SqlDateTime.Null : DateTime.Parse(_chk11)));
            arParams[13] = new SqlParameter("@chk12", (_chk12 == "" ? SqlDateTime.Null : DateTime.Parse(_chk12)));
            arParams[14] = new SqlParameter("@chk13", (_chk13 == "" ? SqlDateTime.Null : DateTime.Parse(_chk13)));
            arParams[15] = new SqlParameter("@chk14", (_chk14 == "" ? SqlDateTime.Null : DateTime.Parse(_chk14)));
            arParams[16] = new SqlParameter("@chk15", (_chk15 == "" ? SqlDateTime.Null : DateTime.Parse(_chk15)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskGenericII", arParams);
        }
        public void UpdateGenericIIProd(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskGenericIIProd", arParams);
        }
        public void UpdateGenericIIComplete(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskGenericIIComplete", arParams);
        }
        public void UpdateGenericIINotificationsTest(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskGenericIINotificationsTest", arParams);
        }
        public void UpdateGenericIINotificationsProd(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskGenericIINotificationsProd", arParams);
        }
        public DataSet GetGenericII(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskGenericII", arParams);
        }
        public DataSet GetGenericII(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskGenericIIAnswer", arParams);
        }
        #endregion

        public void AddServerStorage(int _requestid, int _itemid, int _number, int _answerid, int _prod, int _modelid)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@answerid", _answerid);
            arParams[4] = new SqlParameter("@prod", _prod);
            arParams[5] = new SqlParameter("@modelid", _modelid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOnDemandTaskServerStorage", arParams);
        }
        public void UpdateServerStorage(int _requestid, int _itemid, int _number, int _chk1)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@chk1", _chk1);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskServerStorage", arParams);
        }
        public void UpdateServerStorageComplete(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskServerStorageComplete", arParams);
        }
        public DataSet GetServerStorage(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskServerStorage", arParams);
        }
        public DataSet GetServerStorageProd(int _requestid, int _itemid, int _answerid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskServerStorageProd", arParams);
        }
        public DataSet GetServerStorage(int _answerid, int _prod)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@prod", _prod);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskServerStorageAnswer", arParams);
        }

        public void AddServerBackup(int _requestid, int _itemid, int _number, int _answerid, int _modelid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@answerid", _answerid);
            arParams[4] = new SqlParameter("@modelid", _modelid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOnDemandTaskServerBackup", arParams);
        }
        public void UpdateServerBackup(int _requestid, int _itemid, int _number, int _chk1)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@chk1", _chk1);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskServerBackup", arParams);
        }
        public void UpdateServerBackupComplete(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskServerBackupComplete", arParams);
        }
        public DataSet GetServerBackup(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskServerBackup", arParams);
        }
        public DataSet GetServerBackup(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskServerBackupAnswer", arParams);
        }
        public DataSet GetServerBackup(int _answerid, int _itemid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskServerBackupAnswerItem", arParams);
        }

        public string GetBody(int _answerid, int _implementor_distributed, int _implementor_midrange)
        {
            StringBuilder sbBody = new StringBuilder();
            Forecast oForecast = new Forecast(user, dsn);
            Users oUser = new Users(0, dsn);
             int intForecast = 0;
            if (oForecast.GetAnswer(_answerid, "forecastid") != "")
                intForecast = Int32.Parse(oForecast.GetAnswer(_answerid, "forecastid"));
            if (intForecast > 0)
            {

                int intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                Requests oRequest = new Requests(0, dsn);
                int intProject = oRequest.GetProjectNumber(intRequest);
                Projects oProject = new Projects(0, dsn);
                Design oDesign = new Design(0, dsn);
                DataSet dsDesign = oDesign.GetAnswer(_answerid);
                sbBody.Append("<tr><td colspan=\"2\">");
                sbBody.Append(oForecast.GetResiliencyAlert(_answerid));
                sbBody.Append("</td></tr>");
                if (dsDesign.Tables[0].Rows.Count > 0)
                {
                    sbBody.Append("<tr><td nowrap><b>Design ID:</b></td><td width=\"100%\">");
                    sbBody.Append(dsDesign.Tables[0].Rows[0]["id"].ToString());
                    //sbBody.Append("</td></tr>");
                    //sbBody.Append("<tr><td nowrap><b>Legacy Design ID:</b></td><td width=\"100%\">");
                    //sbBody.Append(_answerid.ToString());
                }
                else
                {
                    sbBody.Append("<tr><td nowrap><b>Design ID:</b></td><td width=\"100%\">");
                    sbBody.Append(_answerid.ToString());
                }
                sbBody.Append("</td></tr>");
                sbBody.Append("<tr><td nowrap><b>Project Name:</b></td><td width=\"100%\">");
                sbBody.Append(oProject.Get(intProject, "name"));
                sbBody.Append("</td></tr>");
                sbBody.Append("<tr><td nowrap><b>Project Number:</b></td><td width=\"100%\">");
                sbBody.Append(oProject.Get(intProject, "number"));
                sbBody.Append("</td></tr>");
                int intEngineer = Int32.Parse(oProject.Get(intProject, "engineer"));
                sbBody.Append("<tr><td nowrap><b>Integration Engineer:</b></td><td width=\"100%\">");
                sbBody.Append(oUser.GetFullName(intEngineer));
                sbBody.Append(" (");
                sbBody.Append(oUser.GetName(intEngineer));
                sbBody.Append(")</td></tr>");
                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                int intImplementor = -999;
                sbBody.Append("<tr><td nowrap><b>Design Implementor:</b></td><td width=\"100%\">");
                sbBody.Append(oUser.GetFullName(intImplementor));
                sbBody.Append("</td></tr>");
                DataSet dsForecast = oForecast.GetAnswer(_answerid);
                if (dsForecast.Tables[0].Rows.Count > 0)
                {
                    int intPlatform = Int32.Parse(dsForecast.Tables[0].Rows[0]["platformid"].ToString());
                    int intClass = Int32.Parse(dsForecast.Tables[0].Rows[0]["classid"].ToString());
                    int intEnvir = Int32.Parse(dsForecast.Tables[0].Rows[0]["environmentid"].ToString());
                    int intAddress = Int32.Parse(dsForecast.Tables[0].Rows[0]["addressid"].ToString());
                    sbBody.Append("<tr><td nowrap><b>Application Name:</b></td><td width=\"100%\">");
                    sbBody.Append(dsForecast.Tables[0].Rows[0]["appname"].ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append("<tr><td nowrap><b>Application Code:</b></td><td width=\"100%\">");
                    sbBody.Append(dsForecast.Tables[0].Rows[0]["appcode"].ToString());
                    sbBody.Append("</td></tr>");
                    int intContact = Int32.Parse(dsForecast.Tables[0].Rows[0]["appcontact"].ToString());
                    string strContact = "PENDING";
                    if (intContact > 0)
                    {
                        strContact = oUser.GetFullName(intContact) + " (" + oUser.GetName(intContact) + ")";
                    }
                    sbBody.Append("<tr><td nowrap><b>Departmental Manager:</b></td><td width=\"100%\">");
                    sbBody.Append(strContact);
                    sbBody.Append("</td></tr>");
                    int intFirst = Int32.Parse(dsForecast.Tables[0].Rows[0]["admin1"].ToString());
                    string strFirst = "PENDING";
                    if (intFirst > 0)
                        strFirst = oUser.GetFullName(intFirst) + " (" + oUser.GetName(intFirst) + ")";
                    sbBody.Append("<tr><td nowrap><b>Application Technical Lead:</b></td><td width=\"100%\">");
                    sbBody.Append(strFirst);
                    sbBody.Append("</td></tr>");
                    int intSecond = Int32.Parse(dsForecast.Tables[0].Rows[0]["admin2"].ToString());
                    string strSecond = "PENDING";
                    if (intSecond > 0)
                        strSecond = oUser.GetFullName(intSecond) + " (" + oUser.GetName(intSecond) + ")";
                    sbBody.Append("<tr><td nowrap><b>Administrative Contact:</b></td><td width=\"100%\">");
                    sbBody.Append(strSecond);
                    sbBody.Append("</td></tr>");
                    int intAppOwner = Int32.Parse(dsForecast.Tables[0].Rows[0]["appowner"].ToString());
                    string strAppOwner = "PENDING";
                    if (intAppOwner > 0)
                        strAppOwner = oUser.GetFullName(intAppOwner) + " (" + oUser.GetName(intAppOwner) + ")";
                    sbBody.Append("<tr><td nowrap><b>Application Owner:</b></td><td width=\"100%\">");
                    sbBody.Append(strAppOwner);
                    sbBody.Append("</td></tr>");
                    int intNetworkEngineer = Int32.Parse(dsForecast.Tables[0].Rows[0]["networkengineer"].ToString());
                    string strNetworkEngineer = "N / A";
                    if (intNetworkEngineer > 0)
                        strNetworkEngineer = oUser.GetFullName(intNetworkEngineer) + " (" + oUser.GetName(intNetworkEngineer) + ")";
                    sbBody.Append("<tr><td nowrap><b>Network Engineer:</b></td><td width=\"100%\">");
                    sbBody.Append(strNetworkEngineer);
                    sbBody.Append("</td></tr>");
                    Locations oLocation = new Locations(0, dsn);
                    sbBody.Append("<tr><td nowrap><b>Location:</b></td><td width=\"100%\">");
                    sbBody.Append(oLocation.GetFull(intAddress));
                    sbBody.Append("</td></tr>");
                    Classes oClass = new Classes(0, dsn);
                    sbBody.Append("<tr><td nowrap><b>Class:</b></td><td width=\"100%\">");
                    sbBody.Append(oClass.Get(intClass, "name"));
                    sbBody.Append("</td></tr>");
                    Environments oEnvironment = new Environments(0, dsn);
                    sbBody.Append("<tr><td nowrap><b>Environment:</b></td><td width=\"100%\">");
                    sbBody.Append(oEnvironment.Get(intEnvir, "name"));
                    sbBody.Append("</td></tr>");
                    Servers oServer = new Servers(0, dsn);
                    DataSet dsServer = oServer.GetAnswer(_answerid);
                    if (dsServer.Tables[0].Rows.Count > 0)
                    {
                        OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);
                        sbBody.Append("<tr><td nowrap><b>Operating System:</b></td><td width=\"100%\">");
                        sbBody.Append(oOperatingSystem.Get(Int32.Parse(dsServer.Tables[0].Rows[0]["osid"].ToString()), "name"));
                        sbBody.Append("</td></tr>");
                    }
                    int intModel = oForecast.GetModelAsset(_answerid);
                    if (intModel == 0)
                        intModel = oForecast.GetModel(_answerid);
                    //int intModel = 0;
                    //DataSet dsModel = GetModel(_answerid);
                    //if (dsModel.Tables[0].Rows.Count > 0)
                    //    intModel = Int32.Parse(dsModel.Tables[0].Rows[0]["modelid"].ToString());
                    if (intModel > 0)
                    {
                        ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
                        sbBody.Append("<tr><td nowrap><b>Model:</b></td><td width=\"100%\">");
                        sbBody.Append(oModelsProperties.Get(intModel, "name"));
                        sbBody.Append("</td></tr>");
                    }
                    sbBody.Append("<tr><td nowrap><b>Quantity:</b></td><td width=\"100%\">");
                    sbBody.Append(dsForecast.Tables[0].Rows[0]["quantity"].ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append("<tr><td nowrap><b>Recovery Count:</b></td><td width=\"100%\">");
                    sbBody.Append(dsForecast.Tables[0].Rows[0]["recovery_number"].ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append("<tr><td nowrap><b>Commitment Date:</b></td><td width=\"100%\">");
                    sbBody.Append(DateTime.Parse(dsForecast.Tables[0].Rows[0]["implementation"].ToString()).ToShortDateString());
                    sbBody.Append("</td></tr>");
                    DataSet dsQuestions = oForecast.GetQuestionPlatform(intPlatform, intClass, intEnvir);
                    foreach (DataRow drQuestion in dsQuestions.Tables[0].Rows)
                    {
                        string strResponse = "";
                        int intQuestion = Int32.Parse(drQuestion["id"].ToString());
                        DataSet dsAnswers = oForecast.GetAnswerPlatform(_answerid, intQuestion);
                        foreach (DataRow drAnswer in dsAnswers.Tables[0].Rows)
                            strResponse += "<tr><td valign=\"top\"></td><td> " + oForecast.GetResponse(Int32.Parse(drAnswer["responseid"].ToString()), "response") + "</td></tr>";
                        if (strResponse != "")
                        {
                            sbBody.Append("<tr><td valign=\"top\" colspan=\"2\"><table cellpadding=\"1\" cellspacing=\"1\" border=\"0\">");
                            sbBody.Append("<tr><td valign=\"top\"><img src=\"/images/help.gif\" align=\"absmiddle\" border=\"0\"/></td><td>");
                            sbBody.Append(drQuestion["question"].ToString());
                            sbBody.Append("</td></tr>");
                            sbBody.Append(strResponse);
                            sbBody.Append("</table></td></tr>");
                        }
                    }
                }
                if (sbBody.ToString() == "")
                {
                    sbBody.Append("Information Unavailable");
                }
                else
                {
                    sbBody.Insert(0, "<table cellpadding=\"3\" cellspacing=\"2\" border=\"0\">");
                    sbBody.Append("</table>");
                }
            }
            return sbBody.ToString();
        }
        public DataSet GetModel(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskModel", arParams);
        }
        public DataSet GetProd(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskProd", arParams);
        }
        public void DeleteAll(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOnDemandTaskModel", arParams);
        }

        public void AddSuccess(int _answerid, string _type, int _success, string _comments)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@type", _type);
            arParams[2] = new SqlParameter("@success", _success);
            arParams[3] = new SqlParameter("@comments", _comments);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOnDemandTaskSuccess", arParams);
        }
        public DataSet GetSuccess(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskSuccess", arParams);
        }
        public void UpdateStorage(int _answerid, int _modelid)
        {
            Forecast oForecast = new Forecast(user, dsn);
            //ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            int intStep = 0;
            string strTable = "";
            if (oForecast.CanAutoProvision(_answerid) == false)
            {
                intStep = 7;
                strTable = "cv_ondemand_tasks_generic_ii";
            }
            if (strTable != "")
            {
                int intResource = 0;
                DataSet dsTasks = GetPending(_answerid);
                if (dsTasks.Tables[0].Rows.Count > 0)
                    intResource = Int32.Parse(dsTasks.Tables[0].Rows[0]["resourceid"].ToString());
                if (intResource > 0)
                    UpdateTask(intResource, intStep, strTable, 1);
            }
        }
        private void UpdateTask(int _resourcerequestid, int _check_number, string _tablename, int _value)
        {
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            _resourcerequestid = oResourceRequest.GetWorkflowParent(_resourcerequestid);
            DataSet dsResource = oResourceRequest.Get(_resourcerequestid);
            UpdateTask(Int32.Parse(dsResource.Tables[0].Rows[0]["requestid"].ToString()), Int32.Parse(dsResource.Tables[0].Rows[0]["itemid"].ToString()), Int32.Parse(dsResource.Tables[0].Rows[0]["number"].ToString()), _tablename, _check_number, _value);
        }
        private void UpdateTask(int _requestid, int _itemid, int _number, string _table, int _check_number, int _value)
        {
            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE " + _table + " SET chk" + _check_number.ToString() + " = " + _value.ToString() + ", modified = getdate() WHERE requestid = " + _requestid.ToString() + " and itemid = " + _itemid.ToString() + " and number = " + _number.ToString());
        }


        public void AddServerOther(int _requestid, int _serviceid, int _number, int _answerid, int _modelid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@answerid", _answerid);
            arParams[4] = new SqlParameter("@modelid", _modelid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOnDemandTaskServerOther", arParams);
        }
        public void UpdateServerOther(int _requestid, int _serviceid, int _number, int _chk1)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@chk1", _chk1);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskServerOther", arParams);
        }
        public void UpdateServerOtherComplete(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskServerOtherComplete", arParams);
        }
        public DataSet GetServerOther(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskServerOther", arParams);
        }
        public DataSet GetServerOther(int _serviceid, int _answerid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskServerOtherAnswer", arParams);
        }
        public void DeleteServerOther(int _serviceid, int _answerid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@answerid", _answerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOnDemandTaskServerOther", arParams);
        }

        public string GetServerOther(int _requestid, int _serviceid, int _number, int _environment, string _dsn_asset, string _dsn_ip)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            DataSet dsAnswer = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandTaskServerOthers", arParams);

            Servers oServer = new Servers(0, dsn);
            Design oDesign = new Design(0, dsn);
            Forecast oForecast = new Forecast(0, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
            Requests oRequest = new Requests(0, dsn);
            Projects oProject = new Projects(0, dsn);
            Mnemonic oMnemonic = new Mnemonic(0, dsn);
            Resiliency oResiliency = new Resiliency(0, dsn);
            Users oUser = new Users(0, dsn);
            IPAddresses oIPAddresses = new IPAddresses(0, _dsn_ip, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            Variables oVariable = new Variables(_environment);
            Functions oFunction = new Functions(0, dsn, _environment);
            StringBuilder sbReturn = new StringBuilder();
            int intAnswer = 0;
            if (dsAnswer.Tables[0].Rows.Count > 0)
                intAnswer = Int32.Parse(dsAnswer.Tables[0].Rows[0]["answerid"].ToString());
            if (intAnswer > 0)
            {
                DataSet ds = oServer.GetAnswer(intAnswer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int intForecast = 0;
                    int intRequest = 0;
                    int intProject = 0;
                    if (oForecast.GetAnswer(intAnswer, "forecastid") != "" && oForecast.GetAnswer(intAnswer, "forecastid") != "0")
                    {
                        intForecast = Int32.Parse(oForecast.GetAnswer(intAnswer, "forecastid"));
                        intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                        intProject = oRequest.GetProjectNumber(intRequest);
                    }
                    int intUser = Int32.Parse(oForecast.GetAnswer(intAnswer, "userid"));
                    bool boolPNC = (ds.Tables[0].Rows[0]["pnc"].ToString() == "1");
                    int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
                    DateTime datCreated = DateTime.Now;
                    if (oForecast.GetAnswer(intAnswer, "completed") != "")
                        datCreated = DateTime.Parse(oForecast.GetAnswer(intAnswer, "completed"));

                    sbReturn.Append("<tr><td colspan=\"2\">");
                    sbReturn.Append(oForecast.GetResiliencyAlert(intAnswer));
                    sbReturn.Append("</td></tr>");

                    if (intForecast > 0 && intProject > 0)
                    {
                        sbReturn.Append("<tr><td valign=\"top\">Project Name:</td><td valign=\"top\">");
                        sbReturn.Append(oProject.Get(intProject, "name"));
                        sbReturn.Append("</td></tr>");
                        sbReturn.Append("<tr><td valign=\"top\">Project Number:</td><td valign=\"top\">");
                        sbReturn.Append(oProject.Get(intProject, "number"));
                        sbReturn.Append("</td></tr>");
                        sbReturn.Append("<tr><td valign=\"top\">Application Name:</td><td valign=\"top\">");
                        sbReturn.Append(oForecast.GetAnswer(intAnswer, "appname"));
                        sbReturn.Append("</td></tr>");
                    }

                    DataSet dsDesign = oDesign.GetAnswer(intAnswer);
                    int intMnemonic = 0;
                    if (oForecast.GetAnswer(intAnswer, "mnemonicid") != "")
                        intMnemonic = Int32.Parse(oForecast.GetAnswer(intAnswer, "mnemonicid"));
                    sbReturn.Append("<tr><td valign=\"top\">Mnemonic:</td><td valign=\"top\">");
                    sbReturn.Append(oMnemonic.Get(intMnemonic, "factory_code"));
                    sbReturn.Append("</td></tr>");
                    if (intMnemonic > 0)
                    {
                        int intResiliency = oResiliency.GetIDFromMnemonic(intMnemonic, false, intAnswer);
                        sbReturn.Append("<tr><td valign=\"top\">Resiliency:</td><td valign=\"top\">");
                        sbReturn.Append(oResiliency.Get(intResiliency, "name"));
                        sbReturn.Append("</td></tr>");
                    }
                    sbReturn.Append("<tr><td valign=\"top\">Requested By:</td><td valign=\"top\">");
                    sbReturn.Append(oUser.GetFullName(intUser));
                    sbReturn.Append(" (");
                    sbReturn.Append(oUser.GetName(intUser));
                    sbReturn.Append(")");
                    sbReturn.Append("</td></tr>");
                    sbReturn.Append("<tr><td valign=\"top\">Created On:</td><td valign=\"top\">");
                    sbReturn.Append(datCreated.ToLongDateString());
                    sbReturn.Append("</td></tr>");
                    string strNames = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int intServer = Int32.Parse(dr["id"].ToString());
                        int intName = Int32.Parse(dr["nameid"].ToString());
                        string strName = oServer.GetName(intServer, true);
                        strName = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/asset/datapoint_asset_search.aspx?t=name&q=" + oFunction.encryptQueryString(strName) + "','800','600')\">" + strName + "</a>";
                        if (strNames != "")
                            strNames += "<br/>";

                        string strIPs = oServer.GetIPs(intServer, 0, 1, 0, 0, _dsn_ip, "", "");
                        if (strIPs != "")
                            strName += " = " + strIPs;

                        strNames += strName;
                    }
                    sbReturn.Append("<tr><td valign=\"top\">Server Names / IPs:</td><td valign=\"top\">");
                    sbReturn.Append(strNames);
                    sbReturn.Append("</td></tr>");
                    sbReturn.Append("<tr><td valign=\"top\">Design:</td><td valign=\"top\">");
                    if (dsDesign.Tables[0].Rows.Count > 0)
                        sbReturn.Append("<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/service/design.aspx?id=" + oFunction.encryptQueryString(intAnswer.ToString()) + "','800','600')\">" + (dsDesign.Tables[0].Rows[0]["name"].ToString() == "" ? "No Name" : dsDesign.Tables[0].Rows[0]["name"].ToString()) + " (" + dsDesign.Tables[0].Rows[0]["id"].ToString() + ")" + "</a>");
                    else
                        sbReturn.Append("<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/service/design.aspx?id=" + oFunction.encryptQueryString(intAnswer.ToString()) + "','800','600')\">" + (oForecast.GetAnswer(intAnswer, "name") == "" ? "No Name" : oForecast.GetAnswer(intAnswer, "name")) + " (" + intAnswer.ToString() + ")" + "</a>");
                    sbReturn.Append("</td></tr>");
                    sbReturn.Append("<tr><td valign=\"top\">Model:</td><td valign=\"top\">");
                    sbReturn.Append(oModelsProperties.Get(intModel, "name"));
                    sbReturn.Append("</td></tr>");
                }
            }
            //if (strReturn != "")
            //    strReturn = "<table cellpadding=\"3\" cellspacing=\"2\" border=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strReturn + "</table>";
            return sbReturn.ToString();
        }


        public void UpdateTime(int _resourcerequestid, DateTime _datetime, bool _override_complete)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@resourcerequestid", _resourcerequestid);
            arParams[1] = new SqlParameter("@datetime", _datetime);
            arParams[2] = new SqlParameter("@override_complete", (_override_complete ? 1 : 0));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandTaskTime", arParams);
        }
    }
}
