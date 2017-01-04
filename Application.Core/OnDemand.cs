using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;
using System.Text;

namespace NCC.ClearView.Application.Core
{
	public class OnDemand
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public OnDemand(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public void AddWizardStep(int _typeid, string _name, string _subtitle, string _path, int _show_cluster, int _show_csm, int _skip_cluster, int _skip_csm, int _step, int _enabled)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@typeid", _typeid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@subtitle", _subtitle);
            arParams[3] = new SqlParameter("@path", _path);
            arParams[4] = new SqlParameter("@show_cluster", _show_cluster);
            arParams[5] = new SqlParameter("@show_csm", _show_csm);
            arParams[6] = new SqlParameter("@skip_cluster", _skip_cluster);
            arParams[7] = new SqlParameter("@skip_csm", _skip_csm);
            arParams[8] = new SqlParameter("@step", _step);
            arParams[9] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOnDemandWizardStep", arParams);
        }
        public void UpdateWizardStep(int _id, string _name, string _subtitle, string _path, int _show_cluster, int _show_csm, int _skip_cluster, int _skip_csm, int _enabled)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@subtitle", _subtitle);
            arParams[3] = new SqlParameter("@path", _path);
            arParams[4] = new SqlParameter("@show_cluster", _show_cluster);
            arParams[5] = new SqlParameter("@show_csm", _show_csm);
            arParams[6] = new SqlParameter("@skip_cluster", _skip_cluster);
            arParams[7] = new SqlParameter("@skip_csm", _skip_csm);
            arParams[8] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandWizardStep", arParams);
        }
        public void UpdateWizardStepOrder(int _id, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandWizardStepOrder", arParams);
        }
        public void EnableWizardStep(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandWizardStepEnabled", arParams);
        }
        public void DeleteWizardStep(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOnDemandWizardStep", arParams);
        }
        public DataSet GetWizardStep(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandWizardStep", arParams);
        }
        public DataSet GetWizardSteps(int _typeid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@typeid", _typeid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandWizardSteps", arParams);
        }
        public string GetWizardStep(int _id, string _column)
        {
            DataSet ds = GetWizardStep(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }


        public void AddWizardStepDone(int _answerid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOnDemandWizardStepDone", arParams);
        }
        public void DeleteWizardStepDone(int _answerid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOnDemandWizardStepDone", arParams);
        }
        public DataSet GetWizardStepsDone(int _answerid, int _typeid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@typeid", _typeid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandWizardStepsDone", arParams);
        }
        public DataSet GetWizardStepsDoneBack(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandWizardStepsDoneBack", arParams);
        }
        public void Next(int _answerid, int _step)
        {
            DeleteWizardStepDone(_answerid, _step);
            AddWizardStepDone(_answerid, _step);
        }
        public void Back(int _answerid)
        {
            DataSet ds = GetWizardStepsDoneBack(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int _step = Int32.Parse(ds.Tables[0].Rows[0]["step"].ToString());
                DeleteWizardStepDone(_answerid, _step);
            }
        }


        public void AddStep(int _typeid, string _name, string _title, string _path, string _script, string _done, string _interact_path, int _zeus, int _power, int _accounts, int _installs, int _groups, int _type, int _resume_error, int _show_build, int _step, int _enabled)
        {
            arParams = new SqlParameter[17];
            arParams[0] = new SqlParameter("@typeid", _typeid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@title", _title);
            arParams[3] = new SqlParameter("@path", _path);
            arParams[4] = new SqlParameter("@script", _script);
            arParams[5] = new SqlParameter("@done", _done);
            arParams[6] = new SqlParameter("@interact_path", _interact_path);
            arParams[7] = new SqlParameter("@zeus", _zeus);
            arParams[8] = new SqlParameter("@power", _power);
            arParams[9] = new SqlParameter("@accounts", _accounts);
            arParams[10] = new SqlParameter("@installs", _installs);
            arParams[11] = new SqlParameter("@groups", _groups);
            arParams[12] = new SqlParameter("@type", _type);
            arParams[13] = new SqlParameter("@resume_error", _resume_error);
            arParams[14] = new SqlParameter("@show_build", _show_build);
            arParams[15] = new SqlParameter("@step", _step);
            arParams[16] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOnDemandStep", arParams);
        }
        public void UpdateStep(int _id, string _name, string _title, string _path, string _script, string _done, string _interact_path, int _zeus, int _power, int _accounts, int _installs, int _groups, int _type, int _resume_error, int _show_build, int _enabled)
        {
            arParams = new SqlParameter[16];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@title", _title);
            arParams[3] = new SqlParameter("@path", _path);
            arParams[4] = new SqlParameter("@script", _script);
            arParams[5] = new SqlParameter("@done", _done);
            arParams[6] = new SqlParameter("@interact_path", _interact_path);
            arParams[7] = new SqlParameter("@zeus", _zeus);
            arParams[8] = new SqlParameter("@power", _power);
            arParams[9] = new SqlParameter("@accounts", _accounts);
            arParams[10] = new SqlParameter("@installs", _installs);
            arParams[11] = new SqlParameter("@groups", _groups);
            arParams[12] = new SqlParameter("@type", _type);
            arParams[13] = new SqlParameter("@resume_error", _resume_error);
            arParams[14] = new SqlParameter("@show_build", _show_build);
            arParams[15] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandStep", arParams);
        }
        public void UpdateStepOrder(int _id, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandStepOrder", arParams);
        }
        public void EnableStep(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandStepEnabled", arParams);
        }
        public void DeleteStep(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOnDemandStep", arParams);
        }
        public string GetStep(int _typeid, int _step, string _column)
        {
            string strReturn = "";
            DataSet ds = GetSteps(_typeid, 1);
            int intStep = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intStep++;
                if (_step == intStep)
                {
                    strReturn = dr[_column].ToString();
                    break;
                }
            }
            return strReturn;
        }
        public DataSet GetStep(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandStep", arParams);
        }
        public DataSet GetSteps(int _typeid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@typeid", _typeid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandSteps", arParams);
        }
        public string GetStep(int _id, string _column)
        {
            DataSet ds = GetStep(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        public void AddStepDoneServer(int _serverid, int _step, bool _delete)
        {
            if (_delete == true)
                DeleteStepDoneServer(_serverid, _step);
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOnDemandStepDoneServer", arParams);
        }
        public void UpdateStepDoneServer(int _serverid, int _step, DateTime _datetime)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@datetime", _datetime);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandStepDoneServerTime", arParams);
        }
        public void UpdateStepDoneServerRedo(int _serverid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandStepDoneServerRedo", arParams);
        }
        public void UpdateStepDoneServer(int _serverid, int _step, string _result, int _error, bool _append, bool _delete)
        {
            StringBuilder strAppend = new StringBuilder();
            if (_append == true)
            {
                DataSet dsAppend = GetStepDoneServer(_serverid, _step);
                foreach (DataRow drAppend in dsAppend.Tables[0].Rows)
                {
                    if (drAppend["error"].ToString() != "1")
                        strAppend.Append(drAppend["result"].ToString());
                }
                AddStepDoneServer(_serverid, _step, true);
            }
            else if (_delete == true)
                    AddStepDoneServer(_serverid, _step, true);
            strAppend.Append(_result);
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@result", strAppend.ToString());
            arParams[3] = new SqlParameter("@error", _error);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandStepDoneServer", arParams);
        }
        public void UpdateStepDoneServerResult(int _serverid, int _step, string _result, bool _append)
        {
            StringBuilder strAppend = new StringBuilder();
            if (_append == true)
            {
                DataSet dsAppend = GetStepDoneServer(_serverid, _step);
                foreach (DataRow drAppend in dsAppend.Tables[0].Rows)
                {
                    if (drAppend["error"].ToString() != "1")
                        strAppend.Append(drAppend["result"].ToString());
                }
            }
            strAppend.Append(_result);
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@result", strAppend.ToString());
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandStepDoneServerResult", arParams);
        }
        public void DeleteStepDoneServer(int _serverid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOnDemandStepDoneServer", arParams);
        }
        public void DeleteStepDoneServers(int _serverid, int _delete_all_after_and_including_this_step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@step", _delete_all_after_and_including_this_step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOnDemandStepDoneServers", arParams);
        }
        public DataSet GetStepDoneServer(int _serverid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandStepDoneServer", arParams);
        }
        public void AddStepDoneWorkstation(int _workstationid, int _step, bool _delete)
        {
            if (_delete == true)
                DeleteStepDoneWorkstation(_workstationid, _step);
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOnDemandStepDoneWorkstation", arParams);
        }
        public void UpdateStepDoneWorkstation(int _workstationid, int _step, string _result, int _error, bool _append, bool _delete)
        {
            StringBuilder strAppend = new StringBuilder();
            if (_append == true)
            {
                DataSet dsAppend = GetStepDoneWorkstation(_workstationid, _step);
                foreach (DataRow drAppend in dsAppend.Tables[0].Rows)
                {
                    if (drAppend["error"].ToString() != "1")
                        strAppend.Append(drAppend["result"].ToString());
                }
                AddStepDoneWorkstation(_workstationid, _step, true);
            }
            else if (_delete == true)
                AddStepDoneWorkstation(_workstationid, _step, true);
            strAppend.Append(_result);
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@result", strAppend.ToString());
            arParams[3] = new SqlParameter("@error", _error);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandStepDoneWorkstation", arParams);
        }
        public void UpdateStepDoneWorkstationResult(int _workstationid, int _step, string _result, bool _append)
        {
            StringBuilder strAppend = new StringBuilder();
            if (_append == true)
            {
                DataSet dsAppend = GetStepDoneWorkstation(_workstationid, _step);
                foreach (DataRow drAppend in dsAppend.Tables[0].Rows)
                {
                    if (drAppend["error"].ToString() != "1")
                        strAppend.Append(drAppend["result"].ToString());
                }
            }
            strAppend.Append(_result);
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@result", strAppend.ToString());
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandStepDoneWorkstationResult", arParams);
        }
        public void UpdateStepDoneWorkstationRedo(int _workstationid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandStepDoneWorkstationRedo", arParams);
        }
        public void DeleteStepDoneWorkstation(int _workstationid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOnDemandStepDoneWorkstation", arParams);
        }
        public DataSet GetStepDoneWorkstation(int _workstationid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandStepDoneWorkstation", arParams);
        }
    }
}
