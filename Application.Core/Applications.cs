using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Web.UI.WebControls;
using System.IO;

namespace NCC.ClearView.Application.Core
{
	public class Applications
	{
		private string dsn = "";
		private int user = 0;
		private bool logging = false;
		private SqlParameter[] arParams;
		private Log oLog;
        public Applications(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
			oLog = new Log(user, dsn);
		}
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
			return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getApplication", arParams);
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
        public bool IsManager(int _id, int _userid)
        {
            return (GetManager(_id) == _userid);
        }
        public int GetManager(int _id)
        {
            string strManager = Get(_id, "userid");
            if (strManager != "")
                return Int32.Parse(strManager);
            else
                return 0;
        }
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getApplications", arParams);
        }
        public DataSet GetDecoms()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getApplicationDecoms");
        }
        public DataSet Gets(int _parent, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getApplicationsTree", arParams);
        }
        public int GetParent(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getApplicationParent", arParams);
            if (o == null)
                return 0;
            else
                return Int32.Parse(o.ToString());
        }
        public int Add(string _name, string _url, string _service_title, string _orgchart, string _description, string _image, int _userid, int _parent, int _priority1, int _priority2, int _tpm, int _disable_manager, int _manager_approve, int _platform_approve, string _deliverables_doc, int _lead1, int _lead2, int _lead3, int _lead4, int _lead5, int _approve_vacation, int _employees_needed, int _service_search_items, int _send_reminders, int _request_items, int _decom, int _admin, int _dns, int _enabled)
        {
            if (logging == true)
                oLog.Add("Add application " + _name);
            arParams = new SqlParameter[30];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@url", _url);
            arParams[2] = new SqlParameter("@service_title", _service_title);
            arParams[3] = new SqlParameter("@orgchart", _orgchart);
            arParams[4] = new SqlParameter("@description", _description);
            arParams[5] = new SqlParameter("@image", _image);
            arParams[6] = new SqlParameter("@userid", _userid);
            arParams[7] = new SqlParameter("@parent", _parent);
            arParams[8] = new SqlParameter("@priority1", _priority1);
            arParams[9] = new SqlParameter("@priority2", _priority2);
            arParams[10] = new SqlParameter("@tpm", _tpm);
            arParams[11] = new SqlParameter("@disable_manager", _disable_manager);
            arParams[12] = new SqlParameter("@manager_approve", _manager_approve);
            arParams[13] = new SqlParameter("@platform_approve", _platform_approve);
            arParams[14] = new SqlParameter("@deliverables_doc", _deliverables_doc);
            arParams[15] = new SqlParameter("@lead1", _lead1);
            arParams[16] = new SqlParameter("@lead2", _lead2);
            arParams[17] = new SqlParameter("@lead3", _lead3);
            arParams[18] = new SqlParameter("@lead4", _lead4);
            arParams[19] = new SqlParameter("@lead5", _lead5);
            arParams[20] = new SqlParameter("@approve_vacation", _approve_vacation);
            arParams[21] = new SqlParameter("@employees_needed", _employees_needed);
            arParams[22] = new SqlParameter("@service_search_items", _service_search_items);
            arParams[23] = new SqlParameter("@send_reminders", _send_reminders);
            arParams[24] = new SqlParameter("@request_items", _request_items);
            arParams[25] = new SqlParameter("@decom", _decom);
            arParams[26] = new SqlParameter("@admin", _admin);
            arParams[27] = new SqlParameter("@dns", _dns);
            arParams[28] = new SqlParameter("@enabled", _enabled);
            arParams[29] = new SqlParameter("@applicationid", SqlDbType.Int);
            arParams[29].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addApplication", arParams);
            return Int32.Parse(arParams[29].Value.ToString());
        }
        public void Update(int _applicationid, string _name, string _url, string _service_title, string _orgchart, string _description, string _image, int _userid, int _parent, int _priority1, int _priority2, int _tpm, int _disable_manager, int _manager_approve, int _platform_approve, string _deliverables_doc, int _lead1, int _lead2, int _lead3, int _lead4, int _lead5, int _approve_vacation, int _employees_needed, int _service_search_items, int _send_reminders, int _request_items, int _decom, int _admin, int _dns, int _enabled)
        {
            if (logging == true)
                oLog.Add("Update application " + GetName(_applicationid));
            arParams = new SqlParameter[30];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@url", _url);
            arParams[3] = new SqlParameter("@service_title", _service_title);
            arParams[4] = new SqlParameter("@orgchart", _orgchart);
            arParams[5] = new SqlParameter("@description", _description);
            arParams[6] = new SqlParameter("@image", _image);
            arParams[7] = new SqlParameter("@userid", _userid);
            arParams[8] = new SqlParameter("@parent", _parent);
            arParams[9] = new SqlParameter("@priority1", _priority1);
            arParams[10] = new SqlParameter("@priority2", _priority2);
            arParams[11] = new SqlParameter("@tpm", _tpm);
            arParams[12] = new SqlParameter("@disable_manager", _disable_manager);
            arParams[13] = new SqlParameter("@manager_approve", _manager_approve);
            arParams[14] = new SqlParameter("@platform_approve", _platform_approve);
            arParams[15] = new SqlParameter("@deliverables_doc", _deliverables_doc);
            arParams[16] = new SqlParameter("@lead1", _lead1);
            arParams[17] = new SqlParameter("@lead2", _lead2);
            arParams[18] = new SqlParameter("@lead3", _lead3);
            arParams[19] = new SqlParameter("@lead4", _lead4);
            arParams[20] = new SqlParameter("@lead5", _lead5);
            arParams[21] = new SqlParameter("@approve_vacation", _approve_vacation);
            arParams[22] = new SqlParameter("@employees_needed", _employees_needed);
            arParams[23] = new SqlParameter("@service_search_items", _service_search_items);
            arParams[24] = new SqlParameter("@send_reminders", _send_reminders);
            arParams[25] = new SqlParameter("@request_items", _request_items);
            arParams[26] = new SqlParameter("@decom", _decom);
            arParams[27] = new SqlParameter("@admin", _admin);
            arParams[28] = new SqlParameter("@dns", _dns);
            arParams[29] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateApplication", arParams);
        }
        public void Enable(int _id, int _enabled) 
		{
			if (logging == true) 
			{
				if (_enabled == 1)
                    oLog.Add("Enable application " + GetName(_id));
				else
                    oLog.Add("Disable application " + GetName(_id));
			}
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateApplicationEnabled", arParams);
		}
		public void Delete(int _id)
		{
			if (logging == true)
                oLog.Add("Delete application " + GetName(_id));
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteApplication", arParams);
		}
        public string GetFullLink(int _id)
        {
            string strLink = "";
            while (_id != 0)
            {
                strLink = "/" + Get(_id, "url") + strLink;
                _id = GetParent(_id);
            }
            return strLink;
        }
        public void AssignPriority(int _id, RadioButtonList _radio, string _labelid, string _textid, string _hiddenid)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                try
                {
                    int intLead = Int32.Parse(ds.Tables[0].Rows[0]["lead1"].ToString());
                    _radio.Items[0].Attributes.Add("onclick", "AssignPriority('" + _labelid + "','" + intLead.ToString() + "');AssignPriority('" + _textid + "','" + DateTime.Today.AddDays(intLead).ToShortDateString() + "');AssignPriority('" + _hiddenid + "','" + DateTime.Today.AddDays(intLead).ToShortDateString() + "');");
                    intLead = Int32.Parse(ds.Tables[0].Rows[0]["lead2"].ToString());
                    _radio.Items[1].Attributes.Add("onclick", "AssignPriority('" + _labelid + "','" + intLead.ToString() + "');AssignPriority('" + _textid + "','" + DateTime.Today.AddDays(intLead).ToShortDateString() + "');AssignPriority('" + _hiddenid + "','" + DateTime.Today.AddDays(intLead).ToShortDateString() + "');");
                    intLead = Int32.Parse(ds.Tables[0].Rows[0]["lead3"].ToString());
                    _radio.Items[2].Attributes.Add("onclick", "AssignPriority('" + _labelid + "','" + intLead.ToString() + "');AssignPriority('" + _textid + "','" + DateTime.Today.AddDays(intLead).ToShortDateString() + "');AssignPriority('" + _hiddenid + "','" + DateTime.Today.AddDays(intLead).ToShortDateString() + "');");
                    intLead = Int32.Parse(ds.Tables[0].Rows[0]["lead4"].ToString());
                    _radio.Items[3].Attributes.Add("onclick", "AssignPriority('" + _labelid + "','" + intLead.ToString() + "');AssignPriority('" + _textid + "','" + DateTime.Today.AddDays(intLead).ToShortDateString() + "');AssignPriority('" + _hiddenid + "','" + DateTime.Today.AddDays(intLead).ToShortDateString() + "');");
                    intLead = Int32.Parse(ds.Tables[0].Rows[0]["lead5"].ToString());
                    _radio.Items[4].Attributes.Add("onclick", "AssignPriority('" + _labelid + "','" + intLead.ToString() + "');AssignPriority('" + _textid + "','" + DateTime.Today.AddDays(intLead).ToShortDateString() + "');AssignPriority('" + _hiddenid + "','" + DateTime.Today.AddDays(intLead).ToShortDateString() + "');");
                }
                catch { }
            }
        }
        public int GetLead(int _id, int _priority)
        {
            int intLead = 0;
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                try
                {

                    Int32.TryParse(ds.Tables[0].Rows[0]["lead" + _priority.ToString()].ToString(), out intLead);
                }
                catch { }
            }
            return intLead;
        }
        public string GetUrl(int _applicationid, int _pageid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@pageid", _pageid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getApplicationUrl", arParams);
            if (o == null)
                return "";
            else
                return o.ToString();
        }

        public DataSet GetApplicationsServices(int _applicationid, int _itemid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getApplicationsServices", arParams);
        }
        public bool IsAdmin(int _applicationid)
        {
            return (Get(_applicationid, "admin") == "1");
        }
    }
}
