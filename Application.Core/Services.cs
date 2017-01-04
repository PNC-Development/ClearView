using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;

namespace NCC.ClearView.Application.Core
{
    public struct WorkflowServices
    {
        public int ServiceID;
        public int Level;
        public bool SameTime;
        public int Children;
        public string Created;
        public string Completed;
    }
    public class Services
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        private StringBuilder strServiceItem;
        private StringBuilder strServiceItemEnd;
        public Services(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getService", arParams);
		}
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public string GetName(int _id)
		{
			string strName = "Unavailable";
            try { strName = Get(_id).Tables[0].Rows[0]["name"].ToString(); }
			catch {}
			return strName;
		}
        public DataSet GetTPM()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServicesTPM");
        }
        public DataSet GetPending()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServicesPending");
        }
        public DataSet Gets(int _parent, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServicesUser", arParams);
        }

        public int Add(string _name, string _description, string _image, int _itemid, int _typeid, int _show, int _project, int _step, double _hours, double _sla, int _approval, int _can_automate, int _statement, int _upload, int _expedite, string _rr_path, string _wm_path, string _cp_path, string _ca_path, int _rejection, int _automate, int _disable_hours, int _quantity_is_device, int _multiple_quantity, int _notify_pc, int _notify_client, int _disable_customization, int _tasks, string _email, int _sametime, int _notify_green, int _notify_yellow, int _notify_red, int _workflow, string _workflow_title, int _title_override, string _title_name, int _no_slider, int _hide_sla, int _is_restricted, int _manager_approval, string _notify_complete, int _workflow_userid, int _docid, int _workflow_connect, int _same_technician, int _enabled)
        {
            arParams = new SqlParameter[48];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@description", _description);
            arParams[2] = new SqlParameter("@image", _image);
            arParams[3] = new SqlParameter("@itemid", _itemid);
            arParams[4] = new SqlParameter("@typeid", _typeid);
            arParams[5] = new SqlParameter("@show", _show);
            arParams[6] = new SqlParameter("@project", _project);
            arParams[7] = new SqlParameter("@step", _step);
            arParams[8] = new SqlParameter("@hours", _hours);
            arParams[9] = new SqlParameter("@sla", _sla);
            arParams[10] = new SqlParameter("@approval", _approval);
            arParams[11] = new SqlParameter("@can_automate", _can_automate);
            arParams[12] = new SqlParameter("@statement", _statement);
            arParams[13] = new SqlParameter("@upload", _upload);
            arParams[14] = new SqlParameter("@expedite", _expedite);
            arParams[15] = new SqlParameter("@rr_path", _rr_path);
            arParams[16] = new SqlParameter("@wm_path", _wm_path);
            arParams[17] = new SqlParameter("@cp_path", _cp_path);
            arParams[18] = new SqlParameter("@ca_path", _ca_path);
            arParams[19] = new SqlParameter("@rejection", _rejection);
            arParams[20] = new SqlParameter("@automate", _automate);
            arParams[21] = new SqlParameter("@disable_hours", _disable_hours);
            arParams[22] = new SqlParameter("@quantity_is_device", _quantity_is_device);
            arParams[23] = new SqlParameter("@multiple_quantity", _multiple_quantity);
            arParams[24] = new SqlParameter("@notify_pc", _notify_pc);
            arParams[25] = new SqlParameter("@notify_client", _notify_client);
            arParams[26] = new SqlParameter("@disable_customization", _disable_customization);
            arParams[27] = new SqlParameter("@tasks", _tasks);
            arParams[28] = new SqlParameter("@email", _email);
            arParams[29] = new SqlParameter("@sametime", _sametime);
            arParams[30] = new SqlParameter("@notify_green", _notify_green);
            arParams[31] = new SqlParameter("@notify_yellow", _notify_yellow);
            arParams[32] = new SqlParameter("@notify_red", _notify_red);
            arParams[33] = new SqlParameter("@workflow", _workflow);
            arParams[34] = new SqlParameter("@workflow_title", _workflow_title);
            arParams[35] = new SqlParameter("@title_override", _title_override);
            arParams[36] = new SqlParameter("@title_name", _title_name);
            arParams[37] = new SqlParameter("@no_slider", _no_slider);
            arParams[38] = new SqlParameter("@hide_sla", _hide_sla);
            arParams[39] = new SqlParameter("@is_restricted", _is_restricted);
            arParams[40] = new SqlParameter("@manager_approval", _manager_approval);
            arParams[41] = new SqlParameter("@notify_complete", _notify_complete);
            arParams[42] = new SqlParameter("@workflow_userid", _workflow_userid);
            arParams[43] = new SqlParameter("@docid", _docid);
            arParams[44] = new SqlParameter("@workflow_connect", _workflow_connect);
            arParams[45] = new SqlParameter("@same_technician", _same_technician);
            arParams[46] = new SqlParameter("@enabled", _enabled);
            arParams[47] = new SqlParameter("@serviceid", SqlDbType.Int);
            arParams[47].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addService", arParams);
            return Int32.Parse(arParams[47].Value.ToString());
        }
        public void Update(int _serviceid, string _name, string _description, string _image, int _itemid, int _typeid, int _show, int _project, int _step, double _hours, double _sla, int _approval, int _can_automate, int _statement, int _upload, int _expedite, string _rr_path, string _wm_path, string _cp_path, string _ca_path, int _rejection, int _automate, int _disable_hours, int _quantity_is_device, int _multiple_quantity, int _notify_pc, int _notify_client, int _disable_customization, int _tasks, string _email, int _sametime, int _notify_green, int _notify_yellow, int _notify_red, int _workflow, string _workflow_title, int _title_override, string _title_name, int _no_slider, int _hide_sla, int _is_restricted, int _manager_approval, string _notify_complete, int _workflow_userid, int _docid, int _workflow_connect, int _same_technician, int _enabled, string _dsn_asset)
        {
            UpdateServiceItem(_serviceid, GetItemId(_serviceid), _itemid, _dsn_asset);     // Update the new RequestItem for the ServiceID in all other tables.
            arParams = new SqlParameter[48];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@description", _description);
            arParams[3] = new SqlParameter("@image", _image);
            arParams[4] = new SqlParameter("@itemid", _itemid);
            arParams[5] = new SqlParameter("@typeid", _typeid);
            arParams[6] = new SqlParameter("@show", _show);
            arParams[7] = new SqlParameter("@project", _project);
            arParams[8] = new SqlParameter("@step", _step);
            arParams[9] = new SqlParameter("@hours", _hours);
            arParams[10] = new SqlParameter("@sla", _sla);
            arParams[11] = new SqlParameter("@approval", _approval);
            arParams[12] = new SqlParameter("@can_automate", _can_automate);
            arParams[13] = new SqlParameter("@statement", _statement);
            arParams[14] = new SqlParameter("@upload", _upload);
            arParams[15] = new SqlParameter("@expedite", _expedite);
            arParams[16] = new SqlParameter("@rr_path", _rr_path);
            arParams[17] = new SqlParameter("@wm_path", _wm_path);
            arParams[18] = new SqlParameter("@cp_path", _cp_path);
            arParams[19] = new SqlParameter("@ca_path", _ca_path);
            arParams[20] = new SqlParameter("@rejection", _rejection);
            arParams[21] = new SqlParameter("@automate", _automate);
            arParams[22] = new SqlParameter("@disable_hours", _disable_hours);
            arParams[23] = new SqlParameter("@quantity_is_device", _quantity_is_device);
            arParams[24] = new SqlParameter("@multiple_quantity", _multiple_quantity);
            arParams[25] = new SqlParameter("@notify_pc", _notify_pc);
            arParams[26] = new SqlParameter("@notify_client", _notify_client);
            arParams[27] = new SqlParameter("@disable_customization", _disable_customization);
            arParams[28] = new SqlParameter("@tasks", _tasks);
            arParams[29] = new SqlParameter("@email", _email);
            arParams[30] = new SqlParameter("@sametime", _sametime);
            arParams[31] = new SqlParameter("@notify_green", _notify_green);
            arParams[32] = new SqlParameter("@notify_yellow", _notify_yellow);
            arParams[33] = new SqlParameter("@notify_red", _notify_red);
            arParams[34] = new SqlParameter("@workflow", _workflow);
            arParams[35] = new SqlParameter("@workflow_title", _workflow_title);
            arParams[36] = new SqlParameter("@title_override", _title_override);
            arParams[37] = new SqlParameter("@title_name", _title_name);
            arParams[38] = new SqlParameter("@no_slider", _no_slider);
            arParams[39] = new SqlParameter("@hide_sla", _hide_sla);
            arParams[40] = new SqlParameter("@is_restricted", _is_restricted);
            arParams[41] = new SqlParameter("@manager_approval", _manager_approval);
            arParams[42] = new SqlParameter("@notify_complete", _notify_complete);
            arParams[43] = new SqlParameter("@workflow_userid", _workflow_userid);
            arParams[44] = new SqlParameter("@docid", _docid);
            arParams[45] = new SqlParameter("@workflow_connect", _workflow_connect);
            arParams[46] = new SqlParameter("@same_technician", _same_technician);
            arParams[47] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateService", arParams);
        }
        public void UpdateEditor(int _id, string _email, int _workflow_userid, int _approval)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@email", _email);
            arParams[2] = new SqlParameter("@workflow_userid", _workflow_userid);
            arParams[3] = new SqlParameter("@approval", _approval);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceEditorUsers", arParams);
        }
        public void UpdateEditor(int _id, int _can_automate, int _statement, int _upload, int _expedite, int _rejection, int _multiple_quantity, int _tasks, int _notify_green, int _notify_yellow, int _notify_red, int _manager_approval, string _notify_complete)
        {
            arParams = new SqlParameter[13];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@can_automate", _can_automate);
            arParams[2] = new SqlParameter("@statement", _statement);
            arParams[3] = new SqlParameter("@upload", _upload);
            arParams[4] = new SqlParameter("@expedite", _expedite);
            arParams[5] = new SqlParameter("@rejection", _rejection);
            arParams[6] = new SqlParameter("@multiple_quantity", _multiple_quantity);
            arParams[7] = new SqlParameter("@tasks", _tasks);
            arParams[8] = new SqlParameter("@notify_green", _notify_green);
            arParams[9] = new SqlParameter("@notify_yellow", _notify_yellow);
            arParams[10] = new SqlParameter("@notify_red", _notify_red);
            arParams[11] = new SqlParameter("@manager_approval", _manager_approval);
            arParams[12] = new SqlParameter("@notify_complete", _notify_complete);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceEditor2", arParams);
        }
        public void UpdateEditorWorkflow(int _id, int _sametime, int _workflow_connect, int _same_technician)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@sametime", _sametime);
            arParams[2] = new SqlParameter("@workflow_connect", _workflow_connect);
            arParams[3] = new SqlParameter("@same_technician", _same_technician);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceEditorWorkflow", arParams);
        }
        public void UpdateEditorWorkflow(int _id, int _workflow, string _workflow_title)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@workflow", _workflow);
            arParams[2] = new SqlParameter("@workflow_title", _workflow_title);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceEditorWorkflows", arParams);
        }
        public void UpdateEditorRestriction(int _id, int _is_restricted)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@is_restricted", _is_restricted);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceEditorRestriction", arParams);
        }
        public void Update(int _id, int _itemid, string _dsn_asset)
        {
            UpdateServiceItem(_id, GetItemId(_id), _itemid, _dsn_asset);
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceItem", arParams);
        }
        public void UpdateEditor(int _id, double _hours, int _no_slider, double _sla, int _hide_sla)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@hours", _hours);
            arParams[2] = new SqlParameter("@no_slider", _no_slider);
            arParams[3] = new SqlParameter("@sla", _sla);
            arParams[4] = new SqlParameter("@hide_sla", _hide_sla);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceEditor3", arParams);
        }
        public void UpdateEditor(int _id, string _name, string _description, int _show, int _project, int _title_override, string _title_name)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@description", _description);
            arParams[3] = new SqlParameter("@show", _show);
            arParams[4] = new SqlParameter("@project", _project);
            arParams[5] = new SqlParameter("@title_override", _title_override);
            arParams[6] = new SqlParameter("@title_name", _title_name);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceEditor1", arParams);
        }
        public void UpdateStep(int _id, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceStep", arParams);
        }
        public void Enable(int _id, int _enabled) 
		{
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceEnabled", arParams);
		}
        public void Delete(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteService", arParams);
		}
        public int GetItemId(int _id)
        {
            int intItem = 0;
            try { Int32.TryParse(Get(_id).Tables[0].Rows[0]["itemid"].ToString(), out intItem); }
            catch { }
            return intItem;
        }


        public void AddSelected(int _requestid, int _serviceid, int _quantity)
        {
            DataSet dsApprovers = GetUser(_serviceid, -10);
            int intApproval = ((Get(_serviceid, "approval") == "1" && dsApprovers.Tables[0].Rows.Count > 0) ? 0 : 1);

            DataSet ds = GetSelected(_requestid, _serviceid);
            int intNumber = (ds.Tables[0].Rows.Count + 1);
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", intNumber);
            arParams[3] = new SqlParameter("@quantity", _quantity);
            arParams[4] = new SqlParameter("@approved", intApproval);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServiceSelected", arParams);
        }
        public void UpdateSelected(int _requestid, int _serviceid, int _number, int _quantity)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@quantity", _quantity);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceSelected", arParams);
        }
        public void UpdateSelectedApprove(int _requestid, int _serviceid, int _number, int _approved, int _approvedby, DateTime _approvedon, string _reason)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@approved", _approved);
            arParams[4] = new SqlParameter("@approvedby", _approvedby);
            arParams[5] = new SqlParameter("@approvedon", _approvedon);
            arParams[6] = new SqlParameter("@reason", _reason);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceSelectedApprove", arParams);
        }
        public void CancelSelected(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceSelectedCancelled", arParams);
        }
        //public DataSet GetSelectedCart(int _requestid)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServicesSelectedCart", arParams);
        //}
        public DataSet GetSelectedById(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServicesSelectedById", arParams);
        }
        public DataSet GetSelected(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServicesSelected", arParams);
        }
        public DataSet GetSelected(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServicesSelectedService", arParams);
        }
        public string GetSelected(int _requestid, int _serviceid, int _number, string _column)
        {
            DataSet ds = GetSelected(_requestid, _serviceid, _number);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetSelected(int _requestid, int _serviceid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServicesSelectedServiceAll", arParams);
        }
        public void DeleteSelected(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServicesSelected", arParams);
        }
        public void DeleteSelected(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServiceSelected", arParams);
        }

        public DataSet GetType(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceType", arParams);
        }
        public string GetType(int _id, string _column)
        {
            DataSet ds = GetType(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetTypes(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceTypes", arParams);
        }
        public void AddType(string _name, string _path, int _ondemand, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@path", _path);
            arParams[2] = new SqlParameter("@ondemand", _ondemand);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServiceType", arParams);
        }
        public void UpdateType(int _id, string _name, string _path, int _ondemand, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@path", _path);
            arParams[3] = new SqlParameter("@ondemand", _ondemand);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceType", arParams);
        }
        public void EnableType(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceTypeEnabled", arParams);
        }
        public void DeleteType(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServiceType", arParams);
        }

        public string GetFolderLocation(int _folderid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@folderid", _folderid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getServiceFolderLocation", arParams);
            if (o == null)
                return "";
            else
                return o.ToString();
        }
        public DataSet GetFolders(int _parent, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceFolders", arParams);
        }
        public int GetFolderParent(int _id)
        {
            int intParent = 0;
            try { Int32.TryParse(GetFolder(_id, "parent"), out intParent); }
            catch { }
            return intParent;
        }
        public DataSet GetFolder(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceFolder", arParams);
        }
        public string GetFolder(int _id, string _column)
        {
            DataSet ds = GetFolder(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddFolder(string _name, string _description, string _image, int _parent, int _userid, int _display, int _enabled)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@description", _description);
            arParams[2] = new SqlParameter("@image", _image);
            arParams[3] = new SqlParameter("@parent", _parent);
            arParams[4] = new SqlParameter("@userid", _userid);
            arParams[5] = new SqlParameter("@display", _display);
            arParams[6] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServiceFolder", arParams);
        }
        public void UpdateFolder(int _id, string _name, string _description, string _image, int _parent, int _userid, int _enabled)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@description", _description);
            arParams[3] = new SqlParameter("@image", _image);
            arParams[4] = new SqlParameter("@parent", _parent);
            arParams[5] = new SqlParameter("@userid", _userid);
            arParams[6] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceFolder", arParams);
        }
        public void UpdateFolderOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceFolderOrder", arParams);
        }
        public void EnableFolder(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceFolderEnabled", arParams);
        }
        public void DeleteFolder(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServiceFolder", arParams);
        }
        public string BreadCrumb(int _id, string _url, string _divider)
        {
            string strCrumbs = "";
            while (_id > 0)
            {
                strCrumbs = _divider + "<a href=\"" + _url + "&sid=" + _id.ToString() + "\">" + GetFolder(_id, "name") + "</a>" + strCrumbs;
                _id = GetFolderParent(_id);
            }
            strCrumbs = "<a href=\"" + _url + "\">All Services</a>" + strCrumbs;
            return strCrumbs;
        }


        
        public void AddFolders(int _serviceid, int _folderid, int _display)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@folderid", _folderid);
            arParams[2] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServicesFolders", arParams);
        }
        public void UpdateFolders(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServicesFolders", arParams);
        }
        public void DeleteFolders(int _serviceid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServicesFolders", arParams);
        }
        /// <summary>
        /// Select all services
        /// </summary>
        /// <param name="_folderid">the folder</param>
        /// <param name="_enabled">1 = only enabled, 0 = all</param>
        /// <param name="_show">1 = only show, 0 = all</param>
        /// <param name="_workflow">1 = all services, 0 = only workflow disabled</param>
        /// <param name="_workflow_connect">1 = only workflow connectable, 0 = all services</param>
        /// <returns></returns>
        public DataSet Gets(int _folderid, int _enabled, int _show, int _workflow, int _workflow_connect)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@folderid", _folderid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            arParams[2] = new SqlParameter("@show", _show);
            arParams[3] = new SqlParameter("@workflow", _workflow);
            arParams[4] = new SqlParameter("@workflow_connect", _workflow_connect);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServices", arParams);
        }
        public DataSet GetsSearch(string _name, int _enabled, int _show)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            arParams[2] = new SqlParameter("@show", _show);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServicesSearch", arParams);
        }
        private bool IsFolder(int _folderid, int _serviceid, int _enabled)
        {
            bool boolFound = false;
            DataSet ds = Gets(_folderid, _enabled, 0, 1, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["serviceid"].ToString()) == _serviceid)
                {
                    boolFound = true;
                    break;
                }
            }
            return boolFound;
        }
        public bool ExpandLocations(TreeNode oParent, int _serviceid, int _enabled)
        {
            if (IsFolder(Int32.Parse(oParent.Value), _serviceid, _enabled) == true)
            {
                oParent.Checked = true;
                oParent.Expand();
                return true;
            }
            else if (oParent.ChildNodes.Count > 0)
            {
                bool boolReturn = false;
                foreach (TreeNode oNode in oParent.ChildNodes)
                {
                    bool boolChild = ExpandLocations(oNode, _serviceid, _enabled);
                    if (boolReturn == false)
                        boolReturn = boolChild;
                    // Sending a break at this point would prevent any subsequent folder(s) in the tree to be checked if selected
                    //if (boolReturn == true)
                    //    break;
                }
                if (boolReturn == true)
                    oParent.Expand();
                return boolReturn;
            }
            else
                return false;
        }
        public DataSet GetFolders(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServicesFolders", arParams);
        }
        public string GetFolders(int _id, string _column)
        {
            DataSet ds = GetFolders(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetFoldersService(int _serviceid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServicesFoldersService", arParams);
        }
        public int GetFolderCount(int _id)
        {
            int intCount = 0;
            DataSet dsFolders = GetFolders(_id, 1);
            foreach (DataRow drFolder in dsFolders.Tables[0].Rows)
                intCount += GetFolderCount(Int32.Parse(drFolder["id"].ToString()));
            DataSet dsServices = Gets(_id, 1, 1, 0, 0);
            return (dsServices.Tables[0].Rows.Count + intCount);
        }

        public void AddUser(int _serviceid, int _userid, int _assign)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@assign", _assign);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServiceUser", arParams);
        }
        public void DeleteUser(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServiceUser", arParams);
        }
        public DataSet GetUsers(int _serviceid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceUsers", arParams);
        }
        public DataSet GetUser(int _serviceid, int _assign)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@assign", _assign);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceUser", arParams);
        }
        public bool IsManager(int _id, int _userid)
        {
            bool boolReturn = false;
            Delegates oDelegate = new Delegates(user, dsn);
            DataSet ds = GetUser(_id, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["userid"].ToString()) == _userid || oDelegate.Get(Int32.Parse(dr["userid"].ToString()), _userid) > 0)
                {
                    boolReturn = true;
                    break;
                }
            }
            if (boolReturn == false)
            {
                ds = GetUser(_id, -1);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Int32.Parse(dr["userid"].ToString()) == _userid || oDelegate.Get(Int32.Parse(dr["userid"].ToString()), _userid) > 0)
                    {
                        boolReturn = true;
                        break;
                    }
                }
            }
            return boolReturn;
        }

        public double GetSLA(int _id)
        {
            double dblSLA = 0.00;
            string strSLA = Get(_id, "sla");
            if (strSLA != "")
                dblSLA = double.Parse(strSLA);
            return dblSLA;
        }

        public DataSet GetWorkflows(int _serviceid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceWorkflows", arParams);
        }
        public DataSet GetWorkflowsReceive(int _nextservice)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@nextservice", _nextservice);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceWorkflowsReceive", arParams);
        }
        public DataSet GetWorkflowsReceive(int _nextservice, int _requestid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@nextservice", _nextservice);
            arParams[1] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceWorkflowsReceiveRequest", arParams);
        }
        public DataSet GetWorkflow(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceWorkflowId", arParams);
        }
        public DataSet GetWorkflow(int _serviceid, int _nextservice)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@nextservice", _nextservice);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceWorkflow", arParams);
        }
        public string GetWorkflow(int _id, string _column)
        {
            DataSet ds = GetWorkflow(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public string GetWorkflow(int _serviceid, int _nextservice, string _column)
        {
            DataSet ds = GetWorkflow(_serviceid, _nextservice);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddWorkflow(int _serviceid, int _nextservice, int _display)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@nextservice", _nextservice);
            arParams[2] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServiceWorkflow", arParams);
        }
        public void UpdateWorkflow(int _serviceid, int _nextservice, int _only, int _continue)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@nextservice", _nextservice);
            arParams[2] = new SqlParameter("@only", _only);
            arParams[3] = new SqlParameter("@continue", _continue);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceWorkflow", arParams);
        }
        public void UpdateWorkflowOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceWorkflowOrder", arParams);
        }
        public void DeleteWorkflow(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServiceWorkflow", arParams);
        }

        //public void UpdateServiceItem(int _id, int _itemid_OLD, int _itemid_NEW, string _dsn_asset)
        //{
        //    int intItem = GetItemId(_id);
        //    if (intItem != _itemid_NEW)
        //    {
        //        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_resource_requests WHERE serviceid = " + _id.ToString());
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            int intItemCheck = Int32.Parse(dr["itemid"].ToString());
        //            if (intItemCheck == intItem)
        //            {
        //                int intRequest = Int32.Parse(dr["requestid"].ToString());
        //                int intNumber = Int32.Parse(dr["number"].ToString());
        //                // SPECIAL
        //                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_request_forms SET itemid = " + _itemid_NEW.ToString() + " WHERE serviceid = " + _id.ToString() + " AND itemid = " + intItem.ToString());
        //                // GENERIC
        //                UpdateServiceItem(dsn, "cv_request_forms", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_WM_storage", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_ondemand_tasks_ii", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_account_requests", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_project_closure_PDF", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_request_results", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_WM_project_coordinator_hours", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_account_maintenance", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_group_maintenance", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_resource_requests_details", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_ondemand_tasks_generic_ii", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_WM_remediation", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_group_maintenance_parameters", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_account_maintenance_parameters", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_ondemand_tasks_vmware_ii", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_ondemand_tasks_physical_ii", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_ondemand_tasks_blade_ii", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_RR_virtual_workstations_accounts", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_WM_third_tier_distributed", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_order_report", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_WM_server_retrieve", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_WM_server_archive", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_WM_workstation", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_resource_assignment", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_TechAssets", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_ondemand_tasks_server_pnc_dns", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_ondemand_tasks_server_pnc_security", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_WM_IDC", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_WM_iis", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_WM_project_coordinator", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_csrc", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_WM_tpm", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_ondemand_tasks_server_storage", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_WM_decommission_server", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_pcr", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_WM_decommission_server_IM", intRequest, intItem, _itemid_NEW, intNumber);
        //                UpdateServiceItem(dsn, "cv_ondemand_tasks_server_backup", intRequest, intItem, _itemid_NEW, intNumber);
        //                // ASSET
        //                UpdateServiceItem(_dsn_asset, "cva_decommissions", intRequest, intItem, _itemid_NEW, intNumber);
        //            }
        //        }
        //        // RESOURCE REQUEST
        //        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests SET itemid = " + _itemid_NEW.ToString() + " WHERE serviceid = " + _id.ToString() + " AND itemid = " + intItem.ToString());
        //    }
        //}
        //private void UpdateServiceItem(string _dsn, string _table, int _requestid, int _itemid_OLD, int _itemid_NEW, int _number)
        //{
        //    DataSet dsTables = SqlHelper.ExecuteDataset(_dsn, CommandType.Text, "select name from sys.tables where name = '" + _table + "'");
        //    if (dsTables.Tables[0].Rows.Count > 0)
        //        SqlHelper.ExecuteNonQuery(_dsn, CommandType.Text, "UPDATE " + _table + " SET itemid = " + _itemid_NEW.ToString() + " WHERE requestid = " + _requestid.ToString() + " AND itemid = " + _itemid_OLD.ToString() + " AND number = " + _number.ToString());
        //}
        /// <summary>
        /// Updates the ITEMID of a recently changed service (or a full list of services based on ITEMID)
        /// </summary>
        /// <param name="_serviceid">The serviceID of the service you wish to change. Set to 0 if you are changing an entire ITEMID.</param>
        /// <param name="_itemid_OLD">The itemID of all the services you wish to change. Set to 0 if you are changing a single SERVICEID.</param>
        /// <param name="_itemid_NEW">The new ITEMID of the service(s).</param>
        /// <param name="_dsn_asset">The dsn of the ClearViewAsset database.</param>
        public void UpdateServiceItem(int _serviceid, int _itemid_OLD, int _itemid_NEW, string _dsn_asset)
        {
            if (_itemid_OLD != _itemid_NEW)
            {
                strServiceItem = new StringBuilder();
                strServiceItemEnd = new StringBuilder();
                strServiceItem.AppendLine("USE ClearView");
                Database(dsn);
                strServiceItem.AppendLine("USE ClearViewAsset");
                Database(_dsn_asset);
                strServiceItem.AppendLine("USE ClearView");
                strServiceItem.AppendLine(strServiceItemEnd.ToString());
                Log oLog = new Log(0, dsn);
                oLog.AddEvent("ITEMID", "ITEMID", "Changing ITEMID (" + _serviceid.ToString() + "," + _itemid_OLD.ToString() + "," + _itemid_NEW.ToString() + " ~ " + strServiceItem.ToString(), LoggingType.Debug);
                // Execute Script
                arParams = new SqlParameter[3];
                arParams[0] = new SqlParameter("@serviceid", _serviceid);
                arParams[1] = new SqlParameter("@itemid_OLD", _itemid_OLD);
                arParams[2] = new SqlParameter("@itemid_NEW", _itemid_NEW);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, strServiceItem.ToString(), arParams);
            }
        }
        private void Database(string _dsn)
        {
            DataSet dsTD = SqlHelper.ExecuteDataset(_dsn, CommandType.Text, "select name from sys.tables order by name");
            foreach (DataRow drTD in dsTD.Tables[0].Rows)
            {
                bool boolRequest = false;
                bool boolService = false;
                bool boolItem = false;
                bool boolNumber = false;
                DataSet dsD = SqlHelper.ExecuteDataset(_dsn, CommandType.Text, "select c.name, c.max_length AS length, t.name AS type from sys.columns c LEFT OUTER JOIN sys.types t ON t.system_type_id = c.system_type_id AND t.user_type_id <= 255 where c.object_id =object_id('" + drTD["name"].ToString() + "')");
                foreach (DataRow drD in dsD.Tables[0].Rows)
                {
                    if (drD["name"].ToString().ToUpper() == "REQUESTID")
                        boolRequest = true;
                    else if (drD["name"].ToString().ToUpper() == "ITEMID")
                        boolItem = true;
                    else if (drD["name"].ToString().ToUpper() == "NUMBER")
                        boolNumber = true;
                    else if (drD["name"].ToString().ToUpper() == "SERVICEID")
                        boolService = true;

                    if (boolRequest && boolItem && boolNumber && boolService)
                        break;
                }
                if (boolRequest && boolItem && boolNumber && boolService == false)
                {
                    strServiceItem.AppendLine("UPDATE " + drTD["name"].ToString() + " SET itemid = @itemid_NEW FROM " + drTD["name"].ToString() + " cvR INNER JOIN ClearView.dbo.cv_resource_requests cvRR ON cvR.requestid = cvRR.requestid AND (@serviceid = 0 OR cvRR.serviceid = @serviceid) AND cvRR.itemid = @itemid_OLD AND cvR.number = cvRR.number WHERE cvR.itemid = @itemid_OLD");
                    strServiceItem.AppendLine("UPDATE " + drTD["name"].ToString() + " SET itemid = @itemid_NEW FROM " + drTD["name"].ToString() + " cvR INNER JOIN ClearView.dbo.cv_request_forms cvRR ON cvR.requestid = cvRR.requestid AND (@serviceid = 0 OR cvRR.serviceid = @serviceid) AND cvRR.itemid = @itemid_OLD AND cvR.number = cvRR.number WHERE cvR.itemid = @itemid_OLD");
                }
                else if (boolRequest && boolItem && boolNumber && boolService)
                    strServiceItemEnd.AppendLine("UPDATE " + drTD["name"].ToString() + " SET itemid = @itemid_NEW WHERE (@serviceid = 0 OR serviceid = @serviceid) AND (@itemid_OLD = 0 OR itemid = @itemid_OLD)");
            }
        }

        public void AddFavorite(int _userid, int _serviceid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServiceFavorite", arParams);
        }
        public void DeleteFavorite(int _userid, int _serviceid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServiceFavorite", arParams);
        }
        public DataSet GetFavorite(int _userid, int _serviceid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceFavorite", arParams);
        }
        public DataSet GetFavorites(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceFavorites", arParams);
        }

        public void AddRestriction(int _serviceid, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServiceRestriction", arParams);
        }
        public void DeleteRestriction(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServiceRestriction", arParams);
        }
        public bool IsRestricted(int _serviceid, int _userid)
        {
            Users oUser = new Users(user, dsn);
            return (Get(_serviceid, "is_restricted") == "1" && GetRestrictions(_serviceid, _userid).Tables[0].Rows.Count == 0 && oUser.IsAdmin(_userid) == false);
        }
        public DataSet GetRestrictions(int _serviceid)
        {
            return GetRestrictions(_serviceid, 0);
        }
        private DataSet GetRestrictions(int _serviceid, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceRestrictions", arParams);
        }

        public void LoadWorkflowUsers(DataSet ds, ref DropDownList ddl)
        {
            LoadWorkflowUsers2(ds, ref ddl);
            ddl.Items.Insert(0, new ListItem("Original Requestor", "-1"));
            ddl.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void LoadWorkflowUsers2(DataSet ds, ref DropDownList ddl)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ddl.Items.Add(new ListItem(dr["service"].ToString(), dr["serviceid"].ToString()));
                LoadWorkflowUsers2(GetWorkflowsReceive(Int32.Parse(dr["serviceid"].ToString())), ref ddl);
            }
        }
        public List<WorkflowServices> PreviousService(int _requestid, int _serviceid, int _number)
        {
            List<WorkflowServices> lstServices = new List<WorkflowServices>();
            PreviousService(_requestid, _serviceid, _number, 1, lstServices);
            return lstServices;
        }
        private void PreviousService(int _requestid, int _serviceid, int _number, int _level, List<WorkflowServices> lstServices)
        {
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            DataSet dsBefore = GetWorkflowsReceive(_serviceid);
            foreach (DataRow drBefore in dsBefore.Tables[0].Rows)
            {
                int intBefore = Int32.Parse(drBefore["serviceid"].ToString());
                WorkflowServices _wf = new WorkflowServices();
                _wf.ServiceID = intBefore;
                _wf.Level = _level;
                _wf.SameTime = (Get(intBefore, "sametime") == "1");
                _wf.Children = GetWorkflows(intBefore).Tables[0].Rows.Count;
                DataSet dsR = oResourceRequest.GetRequestService(_requestid, intBefore, _number);
                if (dsR.Tables[0].Rows.Count > 0)
                {
                    _wf.Created = dsR.Tables[0].Rows[0]["created"].ToString();
                    _wf.Completed = dsR.Tables[0].Rows[0]["completed"].ToString();
                }
                lstServices.Add(_wf);
                PreviousService(_requestid, intBefore, _number, _level + 1, lstServices);
            }
        }
    }
}
