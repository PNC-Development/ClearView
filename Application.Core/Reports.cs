using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.IO;

namespace NCC.ClearView.Application.Core
{
	public class Reports
	{
		private string dsn = "";
		private int user = 0;
        private bool logging = false;
		private SqlParameter[] arParams;
		private DataSet ds;    
        private Log oLog;
        public Reports(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
            oLog = new Log(user, dsn);
		}
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getReport", arParams);
		}
        public string Get(int _id, string _column)
        {
            ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public string GetName(int _id)
		{
			string strName = "Unavailable";
            try { strName = Get(_id).Tables[0].Rows[0]["title"].ToString(); }
			catch {}
			return strName;
		}
        public DataSet Gets(int _parent, int _enabled)
		{
			arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getReports", arParams);
		}
        public DataSet Gets(int _userid, string _search)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@search", _search);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getReportSearch", arParams);
        }
        public void Add(string _title, int _old, string _path, string _physical, string _description, string _about, string _image, int _parent, int _percentage, int _toggle, int _application, int _display, int _enabled)
        {
			arParams = new SqlParameter[13];
            arParams[0] = new SqlParameter("@title", _title);
            arParams[1] = new SqlParameter("@old", _old);
            arParams[2] = new SqlParameter("@path", _path);
            arParams[3] = new SqlParameter("@physical", _physical);
            arParams[4] = new SqlParameter("@description", _description);
            arParams[5] = new SqlParameter("@about", _about);
            arParams[6] = new SqlParameter("@image", _image);
            arParams[7] = new SqlParameter("@parent", _parent);
            arParams[8] = new SqlParameter("@percentage", _percentage);
            arParams[9] = new SqlParameter("@toggle", _toggle);
            arParams[10] = new SqlParameter("@application", _application);
            arParams[11] = new SqlParameter("@display", _display);
            arParams[12] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addReport", arParams);
		}
        public void Update(int _id, string _title, int _old, string _path, string _physical, string _description, string _about, string _image, int _parent, int _percentage, int _toggle, int _application, int _enabled)
		{
			arParams = new SqlParameter[13];
			arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@title", _title);
            arParams[2] = new SqlParameter("@old", _old);
            arParams[3] = new SqlParameter("@path", _path);
            arParams[4] = new SqlParameter("@physical", _physical);
            arParams[5] = new SqlParameter("@description", _description);
            arParams[6] = new SqlParameter("@about", _about);
            arParams[7] = new SqlParameter("@image", _image);
            arParams[8] = new SqlParameter("@parent", _parent);
            arParams[9] = new SqlParameter("@percentage", _percentage);
            arParams[10] = new SqlParameter("@toggle", _toggle);
            arParams[11] = new SqlParameter("@application", _application);
            arParams[12] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateReport", arParams);
		}
        public void Update(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateReportOrder", arParams);
        }
        public void Enable(int _id, int _enabled) 
		{
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateReportEnabled", arParams);
		}
        public void Delete(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteReport", arParams);
		}
        public int GetParent(int _id)
        {
            int intParent = 0;
            try { intParent = Int32.Parse(Get(_id).Tables[0].Rows[0]["parent"].ToString()); }
            catch { }
            return intParent;
        }

        public DataSet GetGroup(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getReportGroup", arParams);
        }
        public string GetGroupName(int _id)
        {
            string strName = "Unavailable";
            try { strName = GetGroup(_id).Tables[0].Rows[0]["name"].ToString(); }
            catch { }
            return strName;
        }
        public DataSet GetGroups(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getReportGroups", arParams);
        }
        public void AddGroup(string _name, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addReportGroup", arParams);
        }
        public void UpdateGroup(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateReportGroup", arParams);
        }
        public void EnableGroup(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateReportGroupEnabled", arParams);
        }
        public void DeleteGroup(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteReportGroup", arParams);
        }

        public DataSet GetRoles(int _applicationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getReportRoles", arParams);
        }
        public DataSet GetRole(int _userid, int _reportid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@reportid", _reportid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getReportRole", arParams);
        }
        public void AddRole(int _applicationid, int _groupid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@groupid", _groupid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addReportRole", arParams);
        }
        public void DeleteRole(int _applicationid, int _groupid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@groupid", _groupid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteReportRole", arParams);
        }

        public DataSet GetPermissions(int _reportid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@reportid", _reportid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getReportPermissions", arParams);
        }
        public void AddPermission(int _reportid, int _groupid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@reportid", _reportid);
            arParams[1] = new SqlParameter("@groupid", _groupid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addReportPermission", arParams);
        }
        public void DeletePermission(int _reportid, int _groupid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@reportid", _reportid);
            arParams[1] = new SqlParameter("@groupid", _groupid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteReportPermission", arParams);
        }

        public DataSet GetApplications(int _reportid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@reportid", _reportid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getReportApplications", arParams);
        }
        public void AddApplication(int _reportid, int _applicationid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@reportid", _reportid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addReportApplication", arParams);
        }
        public void DeleteApplication(int _reportid, int _applicationid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@reportid", _reportid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteReportApplication", arParams);
        }

        public DataSet GetUsers(int _reportid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@reportid", _reportid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getReportUsers", arParams);
        }
        public void AddUser(int _reportid, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@reportid", _reportid);
            arParams[1] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addReportUser", arParams);
        }
        public void DeleteUser(int _reportid, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@reportid", _reportid);
            arParams[1] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteReportUser", arParams);
        }

        public void AddFavorite(int _userid, int _reportid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@reportid", _reportid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addReportFavorite", arParams);
        }
        public DataSet GetFavorites(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getReportFavorites", arParams);
        }
        public void DeleteFavorite(int _userid, int _reportid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@reportid", _reportid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteReportFavorite", arParams);
        }
        #region OrderReport     
       
        public int AddOrderReport(string _title, string _data_source, string _chart_type, string _report_upload, string _instructions, string _data_exclusion)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@title", _title);
            arParams[1] = new SqlParameter("@data_source", _data_source);
            arParams[2] = new SqlParameter("@chart_type", _chart_type);
            arParams[3] = new SqlParameter("@report_upload", _report_upload);
            arParams[4] = new SqlParameter("@instructions", _instructions);             
            arParams[5] = new SqlParameter("@data_exclusion", _data_exclusion);
            arParams[6] = new SqlParameter("@id", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOrderReport", arParams);
            return Int32.Parse(arParams[6].Value.ToString());
        }

        public void AddOrderReportApplications(int _report_id, string _appname)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@reportid", _report_id);
            arParams[1] = new SqlParameter("@appname", _appname);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOrderReportApplications", arParams);
        }


        public void AddOrderReportDataFields(int _report_id, string _name, string _type)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@reportid", _report_id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@type", _type);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOrderReportDataFields", arParams);
        }

        public void AddOrderReportCalculation(int _report_id, int _fieldid, string _formula)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@reportid", _report_id);
            arParams[1] = new SqlParameter("@fieldid", _fieldid);
            arParams[2] = new SqlParameter("@formula", _formula);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOrderReportCalculation", arParams);
        }

        public void UpdateOrderReportDataField(int _id, int _report_id, string _name, string _type)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@reportid", _report_id);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@type", _type);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOrderReportDataFields", arParams);
        }

        public void UpdateOrderReportCalculation(int _id, int _report_id, int _field_id, string _formula)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@reportid", _report_id);
            arParams[2] = new SqlParameter("@fieldid", _field_id);
            arParams[3] = new SqlParameter("@formula", _formula);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOrderReportCalculation", arParams);
        }        

        public void UpdateOrderReport(int _id, int _request_id, int _item_id, int _number)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@requestid", _request_id);
            arParams[2] = new SqlParameter("@itemid", _item_id);
            arParams[3] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOrderReport", arParams);
        }
        public DataSet GetOrderReport(int _request_id, int _item_id, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _request_id);
            arParams[1] = new SqlParameter("@itemid", _item_id);
            arParams[2] = new SqlParameter("@number", _number);
           return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOrderReport", arParams);
        }

        public string GetOrderReport(int _request_id, int _item_id, int _number,string _column)
        {
            DataSet ds = GetOrderReport(_request_id, _item_id, _number);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }


        public DataSet GetOrderReportCalculations(int _report_id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@reportid", _report_id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOrderReportCalculations", arParams);
        }
      
        public DataSet GetOrderReportDataFields(int _report_id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@reportid", _report_id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOrderReportDataFields", arParams);
        }

        public DataSet GetOrderReportApplications(int _report_id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@reportid", _report_id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOrderReportApplications", arParams);
        }

        public void DeleteOrderReportCalculation(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOrderReportCalculation", arParams);
        }
        public void DeleteOrderReportDataField(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOrderReportDataFields", arParams);
        }

        #endregion
        #region OrderReport(Data Source)
        public void AddDataSource(string _name, int _display, int _enabled)
        {
            if (logging == true)
                oLog.Add("Add Data Source(Order Report) " + _name);
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@display", _display);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOrderReportDataSource", arParams);
        }
        public void UpdateDataSource(int _id, string _name, int _enabled)
        {
            if (logging == true)
                oLog.Add("Update Data Source(Order Report)" + GetDataSourceName(_id));
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOrderReportDataSource", arParams);
        }
        public void UpdateDataSourceOrder(int _id, int _display)
        {
            if (logging == true)
                oLog.Add("Update Data Source(Order Report) order " + GetDataSourceName(_id));
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOrderReportDataSourceOrder", arParams);
        }
        public void EnableDataSource(int _id, int _enabled)
        {
            if (logging == true)
            {
                if (_enabled == 1)
                    oLog.Add("Enable Data Source(Order Report) " + GetDataSourceName(_id));
                else
                    oLog.Add("Disable Data Source(Order Report) " + GetChartName(_id));
            }
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOrderReportDataSourceEnabled", arParams);
        }
        public void DeleteDataSource(int _id)
        {
            if (logging == true)
                oLog.Add("Delete  Data Source(Order Report) " + GetDataSourceName(_id));
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOrderReportDataSource", arParams);
        }

        public string GetDataSourceName(int _id)
        {
            string strName = "Unavailable";
            try { strName = Get(_id).Tables[0].Rows[0]["name"].ToString(); }
            catch { }
            return strName;
        }
        public DataSet GetDataSources(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOrderReportDataSources", arParams);
        }
        public DataSet GetDataSource(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOrderReportDataSource", arParams);
        }
        #endregion
        #region OrderReport(Charts)

        public void AddChart(string _name, string _url, int _display, int _enabled)
        {
            if (logging == true)
                oLog.Add("Add Data Source(Order Report) " + _name);
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@url", _url);
            arParams[2] = new SqlParameter("@display", _display);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOrderReportChart", arParams);
        }
        public void UpdateChart(int _id, string _name, string _url, int _enabled)
        {
            if (logging == true)
                oLog.Add("Update Data Source(Order Report)" + GetChartName(_id));
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@url", _url);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOrderReportChart", arParams);
        }
        public void UpdateChartOrder(int _id, int _display)
        {
            if (logging == true)
                oLog.Add("Update Data Source(Order Report) order " + GetChartName(_id));
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOrderReportChartOrder", arParams);
        }
        public void EnableChart(int _id, int _enabled)
        {
            if (logging == true)
            {
                if (_enabled == 1)
                    oLog.Add("Enable Data Source(Order Report) " + GetChartName(_id));
                else
                    oLog.Add("Disable Data Source(Order Report) " + GetChartName(_id));
            }
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOrderReportChartEnabled", arParams);
        }
        public void DeleteChart(int _id)
        {
            if (logging == true)
                oLog.Add("Delete  Data Source(Order Report) " + GetChartName(_id));
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOrderReportChart", arParams);
        }
        public string GetChartName(int _id)
        {
            string strName = "Unavailable";
            try { strName = Get(_id).Tables[0].Rows[0]["name"].ToString(); }
            catch { }
            return strName;
        }

        public DataSet GetCharts(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOrderReportCharts", arParams);
        }
        public DataSet GetChart(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOrderReportChart", arParams);
        }
        #endregion 
    }
}
