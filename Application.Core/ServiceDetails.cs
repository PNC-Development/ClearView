using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
using System.Web;
using System.Text;

namespace NCC.ClearView.Application.Core
{
	public class ServiceDetails
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public ServiceDetails(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceDetail", arParams);
		}
        public int Get(string _name, int _parent)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@parent", _parent);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceDetailName", arParams);
            if (ds.Tables[0].Rows.Count > 0)
                return Int32.Parse(ds.Tables[0].Rows[0]["detailid"].ToString());
            else
                return 0;
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
        public DataSet Gets(int _serviceid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceDetails", arParams);
        }
        public DataSet Gets(int _serviceid, int _parent, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@parent", _parent);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServiceDetailsParent", arParams);
        }
        public int Add(int _serviceid, string _name, int _parent, double _hours, double _additional, int _checkbox, int _display, int _enabled)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@parent", _parent);
            arParams[3] = new SqlParameter("@hours", _hours);
            arParams[4] = new SqlParameter("@additional", _additional);
            arParams[5] = new SqlParameter("@checkbox", _checkbox);
            arParams[6] = new SqlParameter("@display", _display);
            arParams[7] = new SqlParameter("@enabled", _enabled);
            arParams[8] = new SqlParameter("@id", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServiceDetail", arParams);
            return Int32.Parse(arParams[8].Value.ToString());
        }
        public void Update(int _id, string _name, int _parent, double _hours, double _additional, int _checkbox, int _enabled)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@parent", _parent);
            arParams[3] = new SqlParameter("@hours", _hours);
            arParams[4] = new SqlParameter("@additional", _additional);
            arParams[5] = new SqlParameter("@checkbox", _checkbox);
            arParams[6] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceDetail", arParams);
        }
        public void Update(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceDetailOrder", arParams);
        }
        public void Enable(int _id, int _enabled) 
		{
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServiceDetailEnabled", arParams);
		}
        public void Delete(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServiceDetail", arParams);
		}
        public int GetParent(int _id)
        {
            int intParent = 0;
            try { Int32.TryParse(Get(_id).Tables[0].Rows[0]["parent"].ToString(), out intParent); }
            catch { }
            return intParent;
        }

        public double GetHours(int _serviceid, double _quantity)
        {
            double dblHours = 0.00;
            DataSet ds = Gets(_serviceid, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                double _hours = double.Parse(dr["hours"].ToString());
                double _additional = double.Parse(dr["additional"].ToString());
                if (_additional > -1)
                {
                    dblHours += _hours;
                    double dblQuantity = _quantity - 1;
                    dblHours += _additional * dblQuantity;
                }
                else
                    dblHours += _hours * _quantity;
            }
            if (dblHours == 0.00)
            {
                Services oService = new Services(user, dsn);
                dblHours = double.Parse(oService.Get(_serviceid, "hours"));
                dblHours = dblHours * _quantity;
            }
            return dblHours;
        }
        public double GetDetailHours(int _serviceid, int _id, bool _additional)
        {
            double dblHours = 0.00;
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dblHours = double.Parse(ds.Tables[0].Rows[0]["hours"].ToString());
                double dblAdditional = double.Parse(ds.Tables[0].Rows[0]["additional"].ToString());
                if (dblAdditional > -1  && _additional == true)
                    dblHours = dblAdditional;
                ds = ds = Gets(_serviceid, _id);
                foreach (DataRow dr in ds.Tables[0].Rows)
                    dblHours += GetDetailHours(_serviceid, Int32.Parse(dr["id"].ToString()), _additional);
            }
            return dblHours;
        }

        public string LoadCheckboxes(int _requestid, int _itemid, int _number, int _resourceid, int _serviceid)
        {
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            DataSet dsWF = oResourceRequest.GetRequestService(_requestid, _serviceid, _number);
            bool boolDone = false;
            if (dsWF.Tables[0].Rows.Count > 0)
            {
                int intStatus = 0;
                Int32.TryParse(dsWF.Tables[0].Rows[0]["status"].ToString(), out intStatus);
                boolDone = (intStatus == 3 || dsWF.Tables[0].Rows[0]["completed"].ToString() != "");
            }
            StringBuilder sbReturn = new StringBuilder();
            DataSet ds = Gets(_serviceid, 0, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intID = Int32.Parse(dr["id"].ToString());
                if (dr["checkbox"].ToString() == "1")
                {
                    int intValue = oResourceRequest.GetDetailValue(_requestid, _itemid, _number, (oResourceRequest.GetWorkflow(_resourceid, "joined") == "1" ? 0 : _resourceid), intID);
                    if (intValue == -1)
                    {
                        intValue = 0;
                        oResourceRequest.AddDetails(_requestid, _itemid, _number, (oResourceRequest.GetWorkflow(_resourceid, "joined") == "1" ? 0 : _resourceid), intID, intValue);
                    }
                    sbReturn.Append("<tr><td><table id=\"chkTaskList" + _resourceid.ToString() + "\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\"><tr><td valign=\"top\"><input id=\"chkDetail_");
                    sbReturn.Append(intID.ToString());
                    sbReturn.Append("\" type=\"checkbox\" name=\"chkDetail_");
                    sbReturn.Append(intID.ToString());
                    sbReturn.Append("\" onclick=\"UpdateRRCheckDetail('hdnDetail_");
                    sbReturn.Append(intID.ToString());
                    sbReturn.Append("', this);\"");
                    sbReturn.Append(intValue == 1 || boolDone ? " checked" : "");
                    sbReturn.Append(" /></td><td><label for=\"chkDetail_");
                    sbReturn.Append(intID.ToString());
                    sbReturn.Append("\">");
                    sbReturn.Append(dr["name"].ToString());
                    sbReturn.Append("</label><input type=\"hidden\" name=\"hdnDetail_");
                    sbReturn.Append(intID.ToString());
                    sbReturn.Append("\" id=\"hdnDetail_");
                    sbReturn.Append(intID.ToString());
                    sbReturn.Append("\" value=\"");
                    sbReturn.Append(intValue.ToString());
                    sbReturn.Append("\" /></td></tr></table></td></tr>");
                }
            }
            if (sbReturn.ToString() != "")
            {
                sbReturn.Insert(0, "<table cellpadding=\"2\" cellspacing=\"1\" border=\"0\">");
                sbReturn.Append("</table>");
            }
            return sbReturn.ToString();
        }
        public void UpdateCheckboxes(HttpRequest oForm, int _resourceid, int _requestid, int _itemid, int _number)
        {
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            bool boolJoined = (oResourceRequest.GetWorkflow(_resourceid, "joined") == "1");
            // First, uncheck the checkboxes.
            if (boolJoined)
                oResourceRequest.DeleteDetails(_requestid, _itemid, _number);
            else
                oResourceRequest.DeleteDetails(_requestid, _itemid, _number, _resourceid);


            int intParent = oResourceRequest.GetWorkflowParent(_resourceid);
            DataSet dsWF = oResourceRequest.GetWorkflowsParent(intParent);
            if (boolJoined)
            {
                foreach (DataRow drWF in dsWF.Tables[0].Rows)
                {
                    oResourceRequest.DeleteWorkflowHours(Int32.Parse(drWF["id"].ToString()));
                    oResourceRequest.UpdateWorkflowUsed(Int32.Parse(drWF["id"].ToString()), 0.00);
                }
            }
            else
            {
                oResourceRequest.DeleteWorkflowHours(_resourceid);
                oResourceRequest.UpdateWorkflowUsed(_resourceid, 0.00);
            }
            double dblQuantity = double.Parse(oResourceRequest.GetWorkflow(_resourceid, "devices"));
            foreach (string strForm in oForm.Form)
            {
                string strNameID = strForm;
                strNameID = strNameID.ToUpper();
                if (strForm.StartsWith("hdnDetail_") == true)
                {
                    string strValue = oForm.Form[strForm];
                    int intDetail = Int32.Parse(strForm.Substring(strForm.IndexOf("hdnDetail_") + 10));
                    oResourceRequest.AddDetails(_requestid, _itemid, _number, (boolJoined ? 0 : _resourceid), intDetail, Int32.Parse(strValue));
                    if (Int32.Parse(strValue) == 1)
                    {
                        // selected - the following function will update all JOINED values
                        oResourceRequest.UpdateWorkflowHours(_resourceid, (double.Parse(Get(intDetail, "hours")) * dblQuantity));
                    }
                }
            }
        }
    }
}
