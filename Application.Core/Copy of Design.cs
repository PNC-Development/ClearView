using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;
using System.Text;
using System.Web;
using System.Collections.Generic;

namespace NCC.ClearView.Application.Core
{
    public class Design
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        private string strTable = "cv_designs";
        protected string strAppend = "";
        private int intResponseSelected = 0;
        public int ResponseID
        {
            get { return intResponseSelected; }
            set { intResponseSelected = value; }
        }
        private DataTable dtPhases;
        public DataTable Phases
        {
            get { return dtPhases; }
            set { dtPhases = value; }
        }
        private DataTable dtQuestions;
        public DataTable Questions
        {
            get { return dtQuestions; }
            set { dtQuestions = value; }
        }
        private DataTable dtResponses;
        public DataTable Responses
        {
            get { return dtResponses; }
            set { dtResponses = value; }
        }

        public List<int> arrHiddenQuestions;
        public bool boolTooLong = true;


        public Design(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
            arrHiddenQuestions = new List<int>();
		}


        // *****************************************************************************************
        // *****************************************************************************************
        // **   PHASE CONFIGURATION
        // *****************************************************************************************
        // *****************************************************************************************
        public int AddPhase(string _title, string _description, string _help, int _display, int _enabled)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@title", _title);
            arParams[1] = new SqlParameter("@description", _description);
            arParams[2] = new SqlParameter("@help", _help);
            arParams[3] = new SqlParameter("@display", _display);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            arParams[5] = new SqlParameter("@id", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignPhase", arParams);
            return Int32.Parse(arParams[5].Value.ToString());
        }
        public void UpdatePhase(int _id, string _title, string _description, string _help, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@title", _title);
            arParams[2] = new SqlParameter("@description", _description);
            arParams[3] = new SqlParameter("@help", _help);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignPhase", arParams);
        }
        public void UpdatePhaseOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignPhaseOrder", arParams);
        }
        public void EnablePhase(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignPhaseEnabled", arParams);
        }
        public void DeletePhase(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignPhase", arParams);
        }
        public DataSet GetPhase(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignPhase", arParams);
        }
        public string GetPhase(int _id, string _column)
        {
            DataSet ds = GetPhase(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetPhases(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignPhases", arParams);
        }
        public DataSet GetPhaseOrder(int _display)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@display", _display);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignPhaseDisplay", arParams);
        }
        public int GetPhaseMax()
        {
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getDesignPhaseMax");
            if (o != null)
                return Int32.Parse(o.ToString());
            else
                return 0;
        }



        // *****************************************************************************************
        // *****************************************************************************************
        // **   QUESTION CONFIGURATION
        // *****************************************************************************************
        // *****************************************************************************************
        public int AddQuestion(int _phaseid, string _question, string _summary, int _show_summary, int _is_mnemonic, int _is_cost_center, int _is_user_si, int _is_user_dtg, int _is_grid_backup, int _is_backup_exclusions, int _is_grid_maintenance, int _is_storage_luns, int _is_accounts, int _is_date, int _is_type_drop_down, int _is_type_check_box, int _is_type_radio, int _is_type_textbox, int _is_type_textarea, string _related_field, string _default_value, int _allow_empty, string _suffix, int _display, int _enabled)
        {
            arParams = new SqlParameter[26];
            arParams[0] = new SqlParameter("@phaseid", _phaseid);
            arParams[1] = new SqlParameter("@question", _question);
            arParams[2] = new SqlParameter("@summary", _summary);
            arParams[3] = new SqlParameter("@show_summary", _show_summary);
            arParams[4] = new SqlParameter("@is_mnemonic", _is_mnemonic);
            arParams[5] = new SqlParameter("@is_cost_center", _is_cost_center);
            arParams[6] = new SqlParameter("@is_user_si", _is_user_si);
            arParams[7] = new SqlParameter("@is_user_dtg", _is_user_dtg);
            arParams[8] = new SqlParameter("@is_grid_backup", _is_grid_backup);
            arParams[9] = new SqlParameter("@is_backup_exclusions", _is_backup_exclusions);
            arParams[10] = new SqlParameter("@is_grid_maintenance", _is_grid_maintenance);
            arParams[11] = new SqlParameter("@is_storage_luns", _is_storage_luns);
            arParams[12] = new SqlParameter("@is_accounts", _is_accounts);
            arParams[13] = new SqlParameter("@is_date", _is_date);
            arParams[14] = new SqlParameter("@is_type_drop_down", _is_type_drop_down);
            arParams[15] = new SqlParameter("@is_type_check_box", _is_type_check_box);
            arParams[16] = new SqlParameter("@is_type_radio", _is_type_radio);
            arParams[17] = new SqlParameter("@is_type_textbox", _is_type_textbox);
            arParams[18] = new SqlParameter("@is_type_textarea", _is_type_textarea);
            arParams[19] = new SqlParameter("@related_field", _related_field);
            arParams[20] = new SqlParameter("@default_value", _default_value);
            arParams[21] = new SqlParameter("@allow_empty", _allow_empty);
            arParams[22] = new SqlParameter("@suffix", _suffix);
            arParams[23] = new SqlParameter("@display", _display);
            arParams[24] = new SqlParameter("@enabled", _enabled);
            arParams[25] = new SqlParameter("@id", SqlDbType.Int);
            arParams[25].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignQuestion", arParams);
            return Int32.Parse(arParams[25].Value.ToString());
        }
        public void UpdateQuestion(int _id, int _phaseid, string _question, string _summary, int _show_summary, int _is_mnemonic, int _is_cost_center, int _is_user_si, int _is_user_dtg, int _is_grid_backup, int _is_backup_exclusions, int _is_grid_maintenance, int _is_storage_luns, int _is_accounts, int _is_date, int _is_type_drop_down, int _is_type_check_box, int _is_type_radio, int _is_type_textbox, int _is_type_textarea, string _related_field, string _default_value, int _allow_empty, string _suffix, int _enabled)
        {
            arParams = new SqlParameter[25];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@phaseid", _phaseid);
            arParams[2] = new SqlParameter("@question", _question);
            arParams[3] = new SqlParameter("@summary", _summary);
            arParams[4] = new SqlParameter("@show_summary", _show_summary);
            arParams[5] = new SqlParameter("@is_mnemonic", _is_mnemonic);
            arParams[6] = new SqlParameter("@is_cost_center", _is_cost_center);
            arParams[7] = new SqlParameter("@is_user_si", _is_user_si);
            arParams[8] = new SqlParameter("@is_user_dtg", _is_user_dtg);
            arParams[9] = new SqlParameter("@is_grid_backup", _is_grid_backup);
            arParams[10] = new SqlParameter("@is_backup_exclusions", _is_backup_exclusions);
            arParams[11] = new SqlParameter("@is_grid_maintenance", _is_grid_maintenance);
            arParams[12] = new SqlParameter("@is_storage_luns", _is_storage_luns);
            arParams[13] = new SqlParameter("@is_accounts", _is_accounts);
            arParams[14] = new SqlParameter("@is_date", _is_date);
            arParams[15] = new SqlParameter("@is_type_drop_down", _is_type_drop_down);
            arParams[16] = new SqlParameter("@is_type_check_box", _is_type_check_box);
            arParams[17] = new SqlParameter("@is_type_radio", _is_type_radio);
            arParams[18] = new SqlParameter("@is_type_textbox", _is_type_textbox);
            arParams[19] = new SqlParameter("@is_type_textarea", _is_type_textarea);
            arParams[20] = new SqlParameter("@related_field", _related_field);
            arParams[21] = new SqlParameter("@default_value", _default_value);
            arParams[22] = new SqlParameter("@allow_empty", _allow_empty);
            arParams[23] = new SqlParameter("@suffix", _suffix);
            arParams[24] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignQuestion", arParams);
        }
        public void UpdateQuestionOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignQuestionOrder", arParams);
        }
        public void EnableQuestion(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignQuestionEnabled", arParams);
        }
        public void DeleteQuestion(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignQuestion", arParams);
        }
        public DataSet GetQuestion(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignQuestion", arParams);
        }
        public string GetQuestion(int _id, string _column)
        {
            DataSet ds = GetQuestion(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetQuestions(int _phaseid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@phaseid", _phaseid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignQuestions", arParams);
        }

        public string GetQuestionSpecial(int _id)
        {
            string strSpecial = "";
            DataSet ds = GetQuestion(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["is_mnemonic"].ToString() == "1")
                    strSpecial = "MNEMONIC";
                else if (dr["is_cost_center"].ToString() == "1")
                    strSpecial = "COST_CENTER";
                else if (dr["is_user_si"].ToString() == "1")
                    strSpecial = "USER_SI";
                else if (dr["is_user_dtg"].ToString() == "1")
                    strSpecial = "USER_DTG";
                else if (dr["is_grid_backup"].ToString() == "1")
                    strSpecial = "GRID_BACKUP";
                else if (dr["is_backup_exclusions"].ToString() == "1")
                    strSpecial = "BACKUP_EXCLUSION";
                else if (dr["is_grid_maintenance"].ToString() == "1")
                    strSpecial = "GRID_MAINTENANCE";
                else if (dr["is_storage_luns"].ToString() == "1")
                    strSpecial = "STORAGE_LUNS";
                else if (dr["is_accounts"].ToString() == "1")
                    strSpecial = "ACCOUNTS";
                else if (dr["is_date"].ToString() == "1")
                    strSpecial = "DATE";
            }
            return strSpecial;
        }
        public bool IsQuestionCorrelated(int _id)
        {
            return (GetQuestion(_id, "relate_responses") == "1");
        }



        // *****************************************************************************************
        // *****************************************************************************************
        // **   RESPONSE CONFIGURATION
        // *****************************************************************************************
        // *****************************************************************************************
        public int AddResponse(int _questionid, string _response, string _summary, int _set_classid, int _set_osid, int _set_environmentclassid, int _set_environmentid, int _set_addressid, int _set_modelid, int _set_componentid, int _is_under48, int _is_over48, int _is_confidence_lock, int _is_confidence_unlock, int _is_exception, string _related_field, string _related_value, int _visible, int _select_if_one, int _display, int _enabled)
        {
            arParams = new SqlParameter[22];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@response", _response);
            arParams[2] = new SqlParameter("@summary", _summary);
            arParams[3] = new SqlParameter("@set_classid", _set_classid);
            arParams[4] = new SqlParameter("@set_osid", _set_osid);
            arParams[5] = new SqlParameter("@set_environmentclassid", _set_environmentclassid);
            arParams[6] = new SqlParameter("@set_environmentid", _set_environmentid);
            arParams[7] = new SqlParameter("@set_addressid", _set_addressid);
            arParams[8] = new SqlParameter("@set_modelid", _set_modelid);
            arParams[9] = new SqlParameter("@set_componentid", _set_componentid);
            arParams[10] = new SqlParameter("@is_under48", _is_under48);
            arParams[11] = new SqlParameter("@is_over48", _is_over48);
            arParams[12] = new SqlParameter("@is_confidence_lock", _is_confidence_lock);
            arParams[13] = new SqlParameter("@is_confidence_unlock", _is_confidence_unlock);
            arParams[14] = new SqlParameter("@is_exception", _is_exception);
            arParams[15] = new SqlParameter("@related_field", _related_field);
            arParams[16] = new SqlParameter("@related_value", _related_value);
            arParams[17] = new SqlParameter("@visible", _visible);
            arParams[18] = new SqlParameter("@select_if_one", _select_if_one);
            arParams[19] = new SqlParameter("@display", _display);
            arParams[20] = new SqlParameter("@enabled", _enabled);
            arParams[21] = new SqlParameter("@id", SqlDbType.Int);
            arParams[21].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignResponse", arParams);
            return Int32.Parse(arParams[21].Value.ToString());
        }
        public void UpdateResponse(int _id, int _questionid, string _response, string _summary, int _set_classid, int _set_osid, int _set_environmentclassid, int _set_environmentid, int _set_addressid, int _set_modelid, int _set_componentid, int _is_under48, int _is_over48, int _is_confidence_lock, int _is_confidence_unlock, int _is_exception, string _related_field, string _related_value, int _visible, int _select_if_one, int _enabled)
        {
            arParams = new SqlParameter[21];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@questionid", _questionid);
            arParams[2] = new SqlParameter("@response", _response);
            arParams[3] = new SqlParameter("@summary", _summary);
            arParams[4] = new SqlParameter("@set_classid", _set_classid);
            arParams[5] = new SqlParameter("@set_osid", _set_osid);
            arParams[6] = new SqlParameter("@set_environmentclassid", _set_environmentclassid);
            arParams[7] = new SqlParameter("@set_environmentid", _set_environmentid);
            arParams[8] = new SqlParameter("@set_addressid", _set_addressid);
            arParams[9] = new SqlParameter("@set_modelid", _set_modelid);
            arParams[10] = new SqlParameter("@set_componentid", _set_componentid);
            arParams[11] = new SqlParameter("@is_under48", _is_under48);
            arParams[12] = new SqlParameter("@is_over48", _is_over48);
            arParams[13] = new SqlParameter("@is_confidence_lock", _is_confidence_lock);
            arParams[14] = new SqlParameter("@is_confidence_unlock", _is_confidence_unlock);
            arParams[15] = new SqlParameter("@is_exception", _is_exception);
            arParams[16] = new SqlParameter("@related_field", _related_field);
            arParams[17] = new SqlParameter("@related_value", _related_value);
            arParams[18] = new SqlParameter("@visible", _visible);
            arParams[19] = new SqlParameter("@select_if_one", _select_if_one);
            arParams[20] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignResponse", arParams);
        }
        public void UpdateResponseShow(int _id, int _show_all, int _show_any)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@show_all", _show_all);
            arParams[2] = new SqlParameter("@show_any", _show_any);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignResponseShow", arParams);
        }
        public void UpdateResponseHide(int _id, int _hide_all, int _hide_any)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@hide_all", _hide_all);
            arParams[2] = new SqlParameter("@hide_any", _hide_any);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignResponseHide", arParams);
        }
        public void UpdateResponseOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignResponseOrder", arParams);
        }
        public void EnableResponse(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignResponseEnabled", arParams);
        }
        public void DeleteResponse(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignResponse", arParams);
        }
        public DataSet GetResponse(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignResponse", arParams);
        }
        public string GetResponse(int _id, string _column)
        {
            DataSet ds = GetResponse(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetResponses(int _questionid, int _visible, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@visible", _visible);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignResponses", arParams);
        }
        public DataSet GetResponses(int _questionid, int _is_under48, int _is_over48, int _visible, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@is_under48", _is_under48);
            arParams[2] = new SqlParameter("@is_over48", _is_over48);
            arParams[3] = new SqlParameter("@visible", _visible);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignResponsesDR", arParams);
        }
        public DataSet GetResponsesOther(int _is_confidence_lock, int _is_confidence_unlock, int _is_exception, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@is_confidence_lock", _is_confidence_lock);
            arParams[1] = new SqlParameter("@is_confidence_unlock", _is_confidence_unlock);
            arParams[2] = new SqlParameter("@is_exception", _is_exception);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignResponsesOther", arParams);
        }
        public DataSet GetResponsesComponent(int _iis, int _web, int _sql, int _dbase)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@iis", _iis);
            arParams[1] = new SqlParameter("@web", _web);
            arParams[2] = new SqlParameter("@sql", _sql);
            arParams[3] = new SqlParameter("@dbase", _dbase);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignResponsesComponent", arParams);
        }

        public DataSet GetResponsePhases(int _phaseid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@phaseid", _phaseid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignResponsePhases", arParams);
        }
        public DataSet GetResponseLocations()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignResponseLocations");
        }
        public DataSet GetResponseModels()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignResponseModels");
        }
        public DataSet GetResponseComponents(int _componentid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@componentid", _componentid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignResponseComponents", arParams);
        }



        // *****************************************************************************************
        // *****************************************************************************************
        // **   APPROVAL GROUP CONFIGURATION
        // *****************************************************************************************
        // *****************************************************************************************
        public int AddApprovalGroup(string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            arParams[2] = new SqlParameter("@id", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignApprovalGroup", arParams);
            return Int32.Parse(arParams[2].Value.ToString());
        }
        public void UpdateApprovalGroup(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignApprovalGroup", arParams);
        }
        public void EnableApprovalGroup(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignApprovalGroupEnabled", arParams);
        }
        public void DeleteApprovalGroup(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignApprovalGroup", arParams);
        }
        public DataSet GetApprovalGroup(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApprovalGroup", arParams);
        }
        public string GetApprovalGroup(int _id, string _column)
        {
            DataSet ds = GetApprovalGroup(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetApprovalGroups(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApprovalGroups", arParams);
        }



        // *****************************************************************************************
        // *****************************************************************************************
        // **   PHASE RESTRICTION CONFIGURATION
        // *****************************************************************************************
        // *****************************************************************************************
        public void AddRestriction(int _responseid, int _phaseid, int _disabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            arParams[1] = new SqlParameter("@phaseid", _phaseid);
            arParams[2] = new SqlParameter("@disabled", _disabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignRestriction", arParams);
        }
        public void DeleteRestriction(int _responseid, int _disabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            arParams[1] = new SqlParameter("@disabled", _disabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignRestriction", arParams);
        }
        public DataSet GetRestrictions(int _responseid, int _disabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            arParams[1] = new SqlParameter("@disabled", _disabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignRestrictions", arParams);
        }
        public DataSet GetRestriction(int _phaseid, int _disabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@phaseid", _phaseid);
            arParams[1] = new SqlParameter("@disabled", _disabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignRestriction", arParams);
        }



        // *****************************************************************************************
        // *****************************************************************************************
        // **   DYNAMIC QUESTION CONFIGURATION
        // *****************************************************************************************
        // *****************************************************************************************
        public void AddShow(int _responseid, int _questionid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            arParams[1] = new SqlParameter("@questionid", _questionid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignShow", arParams);
        }
        public void DeleteShow(int _responseid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignShow", arParams);
        }
        public DataSet GetShows(int _responseid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignShows", arParams);
        }
        public DataSet GetShow(int _questionid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignShow", arParams);
        }
        public DataSet GetShowsRelated(int _questionid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignShowsRelated", arParams);
        }




        // *****************************************************************************************
        // *****************************************************************************************
        // **   AUTO-RESPONSE CONFIGURATION
        // *****************************************************************************************
        // *****************************************************************************************
        public void AddSelection(int _responseid, int _setid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            arParams[1] = new SqlParameter("@setid", _setid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignSelection", arParams);
        }
        public void DeleteSelection(int _responseid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignSelection", arParams);
        }
        public DataSet GetSelections(int _responseid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignSelections", arParams);
        }
        public DataSet GetSelection(int _questionid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignSelection", arParams);
        }
        public DataSet GetSelectionSet(int _setid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@setid", _setid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignSelectionSet", arParams);
        }




        // *****************************************************************************************
        // *****************************************************************************************
        // **   DYNAMIC RESPONSE CONFIGURATION
        // *****************************************************************************************
        // *****************************************************************************************
        public void AddShowResponse(int _responseid, int _requiredid, int _disabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            arParams[1] = new SqlParameter("@requiredid", _requiredid);
            arParams[2] = new SqlParameter("@disabled", _disabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignShowResponse", arParams);
        }
        public void DeleteShowResponse(int _responseid, int _disabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            arParams[1] = new SqlParameter("@disabled", _disabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignShowResponse", arParams);
        }
        public DataSet GetShowResponses(int _responseid, int _disabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            arParams[1] = new SqlParameter("@disabled", _disabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignShowResponses", arParams);
        }
        public DataSet GetShowResponse(int _requiredid, int _disabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requiredid", _requiredid);
            arParams[1] = new SqlParameter("@disabled", _disabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignShowResponse", arParams);
        }
        //public DataSet GetShowResponsesRelated(int _questionid)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@requiredid", _requiredid);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignShowResponsesRelated", arParams);
        //}
        public bool IsShowResponse(int _responseid, int _designid)
        {
            if (boolTooLong == true)
                return true;
            bool boolShown = true;
            DataSet dsResponse = GetResponse(_responseid);
            if (dsResponse.Tables[0].Rows.Count > 0)
            {
                bool boolShowAll = (dsResponse.Tables[0].Rows[0]["show_all"].ToString() == "1");
                bool boolShowAny = (dsResponse.Tables[0].Rows[0]["show_any"].ToString() == "1");
                bool boolHideAll = (dsResponse.Tables[0].Rows[0]["hide_all"].ToString() == "1");
                bool boolHideAny = (dsResponse.Tables[0].Rows[0]["hide_any"].ToString() == "1");
                int intQuestion = Int32.Parse(dsResponse.Tables[0].Rows[0]["questionid"].ToString());

                bool boolHidden = false;
                for (int ii = 0; ii < arrHiddenQuestions.Count; ii++)
                {
                    if (arrHiddenQuestions[ii] == intQuestion)
                    {
                        boolHidden = true;
                        break;
                    }
                }
                if (boolHidden == false)    // If the question is hidden, the response cannot be selected.
                {
                    if (boolShowAll || boolShowAny || boolHideAll || boolHideAny)
                    {
                        DataSet dsResponseShow = GetShowResponses(_responseid, 0);
                        DataSet dsResponseHide = GetShowResponses(_responseid, 1);
                        if (dsResponseShow.Tables[0].Rows.Count > 0 || dsResponseHide.Tables[0].Rows.Count > 0)
                        {
                            bool boolSelectedOne = false;
                            bool boolSelectedAll = true;   // Will set to FALSE if one of them is not selected.

                            if ((boolShowAll || boolShowAny) && dsResponseShow.Tables[0].Rows.Count > 0)
                            {
                                // One or more responses are required to be selected for this response to be shown.
                                foreach (DataRow drResponse in dsResponseShow.Tables[0].Rows)
                                {
                                    int intRequired = Int32.Parse(drResponse["requiredid"].ToString());
                                    int intRequiredQuestion = 0;
                                    if (Int32.TryParse(GetResponse(intRequired, "questionid"), out intRequiredQuestion) == true)
                                    {
                                        if (IsSelected(_designid, intRequiredQuestion, intRequired) == true)
                                        {
                                            boolSelectedOne = true;
                                            // This response is selected - if ANY of these can be selected for it to be shown, break and continue.
                                            if (boolShowAny == true)
                                                break;
                                        }
                                        else if (boolShowAll == true)
                                        {
                                            // This response is NOT selected - if ALL of these must be selected for it to be shown, EXIT.
                                            boolShown = false;
                                            break;
                                        }
                                    }
                                }
                            }


                            if (boolShown == true)
                            {
                                if ((boolHideAll || boolHideAny) && dsResponseHide.Tables[0].Rows.Count > 0)
                                {
                                    // This option will be shown unless one or more responses are selected.
                                    foreach (DataRow drResponse in dsResponseHide.Tables[0].Rows)
                                    {
                                        int intRequired = Int32.Parse(drResponse["requiredid"].ToString());
                                        int intRequiredQuestion = 0;
                                        if (Int32.TryParse(GetResponse(intRequired, "questionid"), out intRequiredQuestion) == true)
                                        {
                                            if (IsSelected(_designid, intRequiredQuestion, intRequired) == true)
                                            {
                                                // This response is selected - if ANY of these can be selected for it to be hidden, EXIT.
                                                if (boolHideAny == true)
                                                {
                                                    boolShown = false;
                                                    break;
                                                }
                                            }
                                            else if (boolHideAll == true)
                                            {
                                                // This response is NOT selected - if ALL of these must be selected for it to be hidden, break and continue.
                                                boolSelectedAll = false;
                                                break;
                                            }
                                        }
                                    }
                                    if (boolHideAll == true && boolSelectedAll == true)
                                        boolShown = false;
                                }
                                else if (boolShowAny == true && dsResponseShow.Tables[0].Rows.Count > 0)
                                    boolShown = boolSelectedOne;
                            }
                        }
                    }
                }
            }
            return boolShown;
        }




        // *****************************************************************************************
        // *****************************************************************************************
        // **   MODEL CONFIGURATION
        // *****************************************************************************************
        // *****************************************************************************************
        public int AddModel(string _name, int _modelid, int _cores, int _ram, int _display, int _enabled)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@modelid", _modelid);
            arParams[2] = new SqlParameter("@cores", _cores);
            arParams[3] = new SqlParameter("@ram", _ram);
            arParams[4] = new SqlParameter("@display", _display);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            arParams[6] = new SqlParameter("@id", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignModel", arParams);
            return Int32.Parse(arParams[6].Value.ToString());
        }
        public void UpdateModel(int _id, string _name, int _modelid, int _cores, int _ram, int _enabled)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@modelid", _modelid);
            arParams[3] = new SqlParameter("@cores", _cores);
            arParams[4] = new SqlParameter("@ram", _ram);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignModel", arParams);
        }
        public void UpdateModelOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignModelOrder", arParams);
        }
        public void EnableModel(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignModelEnabled", arParams);
        }
        public void DeleteModel(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignModel", arParams);
        }
        public DataSet GetModel(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignModel", arParams);
        }
        public string GetModel(int _id, string _column)
        {
            DataSet ds = GetModel(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetModels(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignModels", arParams);
        }

        public void AddModelInventory(int _modelid, int _addressid, int _classid, int _environmentid, int _inventory)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            arParams[1] = new SqlParameter("@addressid", _addressid);
            arParams[2] = new SqlParameter("@classid", _classid);
            arParams[3] = new SqlParameter("@environmentid", _environmentid);
            arParams[4] = new SqlParameter("@inventory", _inventory);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignModelInventory", arParams);
        }
        public void DeleteModelInventory(int _modelid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignModelInventory", arParams);
        }
        public DataSet GetModelInventorys(int _modelid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignModelInventorys", arParams);
        }
        public DataSet GetModelInventory(int _modelid, int _addressid, int _classid, int _environmentid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            arParams[1] = new SqlParameter("@addressid", _addressid);
            arParams[2] = new SqlParameter("@classid", _classid);
            arParams[3] = new SqlParameter("@environmentid", _environmentid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignModelInventory", arParams);
        }
        public bool IsModelInventory(int _modelid, int _addressid, int _classid, int _environmentid)
        {
            DataSet ds = GetModelInventory(_modelid, _addressid, _classid, _environmentid);
            return (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0]["inventory"].ToString() == "0");
        }




        // *****************************************************************************************
        // *****************************************************************************************
        // **   APPROVAL CONFIGURATION
        // *****************************************************************************************
        // *****************************************************************************************
        public void AddApproval(int _responseid, int _groupid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            arParams[1] = new SqlParameter("@groupid", _groupid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignApproval", arParams);
        }
        public void DeleteApproval(int _responseid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignApproval", arParams);
        }
        public DataSet GetApprovals(int _responseid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApprovals", arParams);
        }

        public void AddApprovalUser(int _groupid, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@groupid", _groupid);
            arParams[1] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignApprovalUser", arParams);
        }
        public void DeleteApprovalUser(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignApprovalUser", arParams);
        }
        public DataSet GetApprovalUsers(int _groupid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@groupid", _groupid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApprovalUsers", arParams);
        }


        public int AddApproverGroup(int _groupid, int _only_exceptions, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@groupid", _groupid);
            arParams[1] = new SqlParameter("@only_exceptions", _only_exceptions);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            arParams[3] = new SqlParameter("@id", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignApproverGroup", arParams);
            return Int32.Parse(arParams[3].Value.ToString());
        }
        public void UpdateApproverGroup(int _id, int _groupid, int _only_exceptions, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@groupid", _groupid);
            arParams[2] = new SqlParameter("@only_exceptions", _only_exceptions);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignApproverGroup", arParams);
        }
        public void EnableApproverGroup(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignApproverGroupEnabled", arParams);
        }
        public void DeleteApproverGroup(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignApproverGroup", arParams);
        }
        public DataSet GetApproverGroup(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApproverGroup", arParams);
        }
        public string GetApproverGroup(int _id, string _column)
        {
            DataSet ds = GetApproverGroup(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetApproverGroups(int _only_exceptions, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@only_exceptions", _only_exceptions);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApproverGroups", arParams);
        }
        public DataSet GetApproverGroups(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApproverGroupsAll", arParams);
        }

        public int AddApproverField(string _related_field, string _title, int _responseid, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@related_field", _related_field);
            arParams[1] = new SqlParameter("@title", _title);
            arParams[2] = new SqlParameter("@responseid", _responseid);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            arParams[4] = new SqlParameter("@id", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignApproverField", arParams);
            return Int32.Parse(arParams[4].Value.ToString());
        }
        public void UpdateApproverField(int _id, string _related_field, string _title, int _responseid, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@related_field", _related_field);
            arParams[2] = new SqlParameter("@title", _title);
            arParams[3] = new SqlParameter("@responseid", _responseid);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignApproverField", arParams);
        }
        public void EnableApproverField(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignApproverFieldEnabled", arParams);
        }
        public void DeleteApproverField(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignApproverField", arParams);
        }
        public DataSet GetApproverField(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApproverField", arParams);
        }
        public string GetApproverField(int _id, string _column)
        {
            DataSet ds = GetApproverField(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetApproverFields(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApproverFields", arParams);
        }


        public void AddSubmitted(int _designid, int _userid, string _comments)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@comments", _comments);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignSubmitted", arParams);
        }
        public DataSet GetSubmitted(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignSubmitted", arParams);
        }
        public void AddApproverFieldWorkflow(int _designid, int _approver_fieldid, int _assignedid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@approver_fieldid", _approver_fieldid);
            arParams[2] = new SqlParameter("@assignedid", _assignedid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignApproverFieldWorkflow", arParams);
        }
        public DataSet GetApproverFieldWorkflow(int _designid, int _approver_fieldid, int _assignedid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@approver_fieldid", _approver_fieldid);
            arParams[2] = new SqlParameter("@assignedid", _assignedid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApproverFieldWorkflow", arParams);
        }
        public void UpdateApproverFieldWorkflow(int _id, int _userid, int _rejected, string _reason)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@rejected", _rejected);
            arParams[3] = new SqlParameter("@reason", _reason);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignApproverFieldWorkflow", arParams);
        }
        public void UpdateApproverFieldWorkflows(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignApproverFieldWorkflows", arParams);
        }
        public void AddApproverGroupWorkflow(int _designid, int _groupid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@groupid", _groupid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignApproverGroupWorkflow", arParams);
        }
        public DataSet GetApproverGroupWorkflow(int _designid, int _groupid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@groupid", _groupid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApproverGroupWorkflow", arParams);
        }
        public void UpdateApproverGroupWorkflow(int _id, int _userid, int _rejected, string _reason)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@rejected", _rejected);
            arParams[3] = new SqlParameter("@reason", _reason);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignApproverGroupWorkflow", arParams);
        }
        public void UpdateApproverGroupWorkflows(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignApproverGroupWorkflows", arParams);
        }
        
        public DataSet GetApprovalsUser(int _userid, int _designid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@designid", _designid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApprovalsUser", arParams);
        }

        
        public int Approve(int _designid, int _userid_cc, bool _exception, int _environment, int intImplementorDistributed, int intWorkstationPlatform, int intImplementorMidrange, string _dsn_asset, string _dsn_ip)
        {
            Settings oSetting = new Settings(user, dsn);
            Functions oFunction = new Functions(user, dsn, _environment);
            Variables oVariable = new Variables(_environment);
            Pages oPage = new Pages(user, dsn);
            Users oUser = new Users(user, dsn);
            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DESIGN_BUILDER");

            int intReturnAnswerID = 0;
            int intRequestor = 0;
            DataSet dsSubmitted = GetSubmitted(_designid);
            if (dsSubmitted.Tables[0].Rows.Count > 0)
                Int32.TryParse(dsSubmitted.Tables[0].Rows[0]["userid"].ToString(), out intRequestor);
            // Gets called when the design is submiited, or when an approver APPROVES a request (DENIES do not get here).
            DataSet dsDesign = Get(_designid);
            if (dsDesign.Tables[0].Rows.Count > 0)
            {
                DataRow drDesign = dsDesign.Tables[0].Rows[0];
                bool boolSent = false;

                if (_exception)
                {
                    // Send to board for approval
                    boolSent = NotifyGroup(_designid, _userid_cc, true, _environment);
                }
                if (boolSent == false)
                {
                    // No Exception...first, send to individual owners (SI, DTG, etc...)
                    DataSet dsField = GetApproverFields(1);
                    foreach (DataRow drField in dsField.Tables[0].Rows) 
                    {
                        string strField = drField["related_field"].ToString();
                        int intApproverFieldID = Int32.Parse(drField["id"].ToString());
                        int intResponse = 0;
                        Int32.TryParse(GetApproverField(intApproverFieldID, "responseid"), out intResponse);
                        int intQuestion = 0;
                        if (intResponse == 0 || (Int32.TryParse(GetResponse(intResponse, "questionid"), out intQuestion) && IsSelected(_designid, intQuestion, intResponse) == true)) 
                        {
                            // Either the required response was selected or there was no response (ResponseID = 0) so assume it is a "global" approval
                            int intUser = 0;
                            if (Int32.TryParse(Get(_designid, strField), out intUser) == true)
                            {
                                // First, check to make sure we haven't already submitted it.
                                bool boolAlready = false;
                                DataSet dsAlready = GetApproverFieldWorkflow(_designid, intApproverFieldID, intUser);
                                foreach (DataRow drAlready in dsAlready.Tables[0].Rows)
                                {
                                    if (drAlready["rejected"].ToString() != "-1")
                                    {
                                        boolAlready = true;
                                        break;
                                    }
                                }
                                if (boolAlready == false)
                                {
                                    // Submit it
                                    boolSent = true;
                                    AddApproverFieldWorkflow(_designid, intApproverFieldID, intUser);
                                    Notify(_designid, intUser, _userid_cc, _environment);
                                }
                            }
                        }
                    }
                }
                if (boolSent == false)
                {
                    // Software Components
                    DataSet dsSoftware = GetSoftwareComponents(_designid);
                    if (dsSoftware.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drSoftware in dsSoftware.Tables[0].Rows)
                        {
                            if (drSoftware["initiated"].ToString() == "")
                            {
                                int intComponent = Int32.Parse(drSoftware["componentid"].ToString());
                                // Update the initiated field
                                UpdateSoftwareComponents(_designid, intComponent);
                                ServerName oServerName = new ServerName(user, dsn);
                                if (oServerName.GetComponentDetail(intComponent, "approval") == "1")
                                {
                                    // Requires approval - send to owners
                                    string strNotify = "";
                                    string[] strNotifys;
                                    char[] strSplit = { ';' };

                                    DataSet dsNotify = oServerName.GetComponentDetailUsers(intComponent, 1);
                                    foreach (DataRow drNotify in dsNotify.Tables[0].Rows)
                                    {
                                        int intUser = Int32.Parse(drNotify["userid"].ToString());
                                        bool boolFound = false;
                                        strNotifys = strNotify.Split(strSplit);
                                        for (int ii = 0; ii < strNotifys.Length; ii++)
                                        {
                                            if (strNotifys[ii].Trim() != "")
                                            {
                                                if (strNotifys[ii].Trim().ToUpper() == intUser.ToString().Trim().ToUpper())
                                                {
                                                    boolFound = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (boolFound == false)
                                        {
                                            if (strNotify != "")
                                                strNotify += ";";
                                            strNotify += intUser.ToString();
                                        }
                                    }
                                    strNotifys = strNotify.Split(strSplit);
                                    for (int ii = 0; ii < strNotifys.Length; ii++)
                                    {
                                        if (strNotifys[ii].Trim() != "")
                                        {
                                            int intUser = 0;
                                            if (Int32.TryParse(strNotifys[ii].Trim(), out intUser) == true)
                                            {
                                                boolSent = true;
                                                string strURL = "";
                                                int intPage = 0;
                                                if (Int32.TryParse(oSetting.Get("design_approval_page"), out intPage) == true)
                                                    strURL = oUser.GetApplicationUrl(intUser, intPage);
                                                if (strURL == "")
                                                    oFunction.SendEmail("Software Component Approval", oUser.GetName(intUser), "", strEMailIdsBCC, "Software Component Approval", "<p><b>A design has been configured with software components that require your approval.</b><p><p>" + GetSummary(_designid, _environment) + "</p>", true, false);
                                                else
                                                    oFunction.SendEmail("Software Component Approval", oUser.GetName(intUser), "", strEMailIdsBCC, "Software Component Approval", "<p><b>A design has been configured with software components that require your approval.</b><p><p>" + GetSummary(_designid, _environment) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strURL + oPage.GetFullLink(intPage) + "?id=" + _designid.ToString() + "\" target=\"_blank\">Click here to review this request.</a></p>", true, false);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    // Automatically approve as clearview
                                    UpdateSoftwareComponent(Int32.Parse(drSoftware["id"].ToString()), -999, 0, "Automatically Approved");
                                }
                            }
                        }
                        if (boolSent == false)
                        {
                            // No new components have been sent.  This might have come from an approved component, and there
                            // might be one or more components still active.  Check to make sure all are complete.
                            int intActive = 0;     // Allow only 1 to be active (this one).
                            DataSet dsActive = LoadWorkflow(_designid);
                            foreach (DataRow drActive in dsActive.Tables[0].Rows)
                            {
                                if (drActive["completed"].ToString() == "")
                                {
                                    intActive++;
                                    if (intActive > 1)
                                    {
                                        boolSent = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (boolSent == false)
                {
                    // Now send to any additional groups for approval
                    boolSent = NotifyGroup(_designid, _userid_cc, false, _environment);
                }

                if (boolSent == false)
                {
                    string strCan = CanExecute(_designid);
                    if (strCan == "" || strCan.ToUpper().Contains("MNEMONIC") == false)
                    {
                        // INITIATE BUILD
                        Forecast oForecast = new Forecast(user, dsn);
                        Mnemonic oMnemonic = new Mnemonic(user, dsn);
                        Classes oClass = new Classes(user, dsn);
                        Domains oDomain = new Domains(user, dsn);
                        Requests oRequest = new Requests(user, dsn);
                        ServiceRequests oServiceRequest = new ServiceRequests(user, dsn);
                        Servers oServer = new Servers(user, dsn);
                        Cluster oCluster = new Cluster(user, dsn);
                        Storage oStorage = new Storage(user, dsn);
                        OperatingSystems oOperatingSystem = new OperatingSystems(user, dsn);
                        ServerName oServerName = new ServerName(user, dsn);
                        ModelsProperties oModelsProperties = new ModelsProperties(user, dsn);
                        Asset oAsset = new Asset(user, _dsn_asset, dsn);

                        DataSet dsSummary = Get(_designid);
                        if (dsSummary.Tables[0].Rows.Count > 0)
                        {
                            DataRow drSummary = dsSummary.Tables[0].Rows[0];
                            // Forecast
                            int intForecast = 0;
                            Int32.TryParse(drSummary["forecastid"].ToString(), out intForecast);
                            // Create the CV_FORECAST_ANSWERS table
                            int intAnswer = oForecast.AddAnswer(intForecast, 1, 0, intRequestor);
                            UpdateAnswerId(_designid, intAnswer);
                            // Class + Environment + Location
                            bool boolDev = false;
                            bool boolTest = false;
                            bool boolQA = false;
                            bool boolProd = false;
                            bool boolDR = false;
                            // Class
                            int intClass = 0;
                            Int32.TryParse(drSummary["classid"].ToString(), out intClass);
                            boolTest = (oClass.IsTest(intClass));
                            boolQA = (oClass.IsQA(intClass));
                            boolProd = (oClass.IsProd(intClass));
                            boolDR = (oClass.IsDR(intClass));
                            boolDev = (boolTest == false && boolQA == false && boolProd == false && boolDR == false);
                            // Environment
                            int intEnv = 0;
                            Int32.TryParse(drSummary["environmentid"].ToString(), out intEnv);
                            // Domain
                            int intDomain = 0;
                            DataSet dsDomain = oDomain.GetClassEnvironment(intClass, intEnv);
                            if (dsDomain.Tables[0].Rows.Count > 0)
                                Int32.TryParse(dsDomain.Tables[0].Rows[0]["id"].ToString(), out intDomain);
                            // Model
                            int intModel = GetModelProperty(_designid);
                            // Location
                            int intAddress = 0;
                            Int32.TryParse(drSummary["addressid"].ToString(), out intAddress);
                            if (intAddress == 0)
                            {
                                // Will need to get the location based on capacity
                                if (oModelsProperties.IsTypeVMware(intModel) == false && oModelsProperties.IsSUNVirtual(intModel) == false && oModelsProperties.IsIBMVirtual(intModel) == false)
                                {
                                    // Get based on capacity
                                    intAddress = UpdateLocation(_designid, _dsn_asset);
                                }
                                else
                                {
                                    // No capacity for Virtual
                                    intAddress = 715;   // Cleveland
                                }
                            }
                            // Quantity
                            int intQuantity = 0;
                            Int32.TryParse(drSummary["quantity"].ToString(), out intQuantity);
                            // Resiliency
                            int intResiliency = 0;
                            Int32.TryParse(drSummary["resiliency"].ToString(), out intResiliency);
                            // Date
                            DateTime datCommitment = DateTime.Now;
                            DateTime.TryParse(drSummary["commitment"].ToString(), out datCommitment);
                            // Mnemonic
                            int intMnemonic = 0;
                            Int32.TryParse(drSummary["mnemonicid"].ToString(), out intMnemonic);
                            // Cost
                            int intCost = 0;
                            Int32.TryParse(drSummary["costid"].ToString(), out intCost);
                            bool boolWeb = (drSummary["web"].ToString() == "1");
                            bool boolSQL = IsSQL(_designid);
                            // OS
                            int intOS = 0;
                            Int32.TryParse(drSummary["osid"].ToString(), out intOS);
                            int intSP = 0;
                            if (intOS > 0)
                                Int32.TryParse(oOperatingSystem.Get(intOS, "default_sp"), out intSP);
                            // Save the Domain and Service Pack
                            Update(_designid, null, null, intSP, intDomain, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);  

                            // Forecast - 1st page
                            oForecast.UpdateAnswer(intAnswer, 0, 0, "", 0, "", "", _designid.ToString(), intAddress, intClass, 0, intEnv, 1, 0, 0, intQuantity, intResiliency);
                            DataSet dsBackup = GetBackup(_designid);
                            oForecast.UpdateBackup(intAnswer, (dsBackup.Tables[0].Rows.Count > 0 ? 1 : 0));
                            DataSet dsStorage = GetStorage(_designid);
                            oForecast.UpdateStorage(intAnswer, (dsStorage.Tables[0].Rows.Count > 0 ? 1 : 0));
                            // Forecast - last page
                            oForecast.UpdateAnswer(intAnswer, datCommitment, 5, intRequestor);
                            oForecast.UpdateAnswerRecovery(intAnswer, (boolProd ? intQuantity : 0));
                            oForecast.UpdateAnswerHA(intAnswer, 0);
                            int intDeptManager = GetUser(oMnemonic.Get(intMnemonic, "DMName"));
                            int intAppTechLead = GetUser(oMnemonic.Get(intMnemonic, "ATLName"));
                            int intAppOwner = GetUser(oMnemonic.Get(intMnemonic, "AppOwner"));
                            // OnDemand Forecast - app page
                            oForecast.UpdateAnswer(intAnswer, oMnemonic.Get(intMnemonic, "name"), "", intMnemonic, intCost, 2, intDeptManager, intAppTechLead, 0, intAppOwner, 0, 0, 0);
                            int intRequest = oForecast.GetRequestID(intAnswer, true);
                            if (intRequest == 0)
                            {
                                intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                                int intProject = oRequest.GetProjectNumber(intRequest);
                                intRequest = oRequest.Add(intProject, intRequestor);
                            }
                            oForecast.UpdateAnswer(intAnswer, intRequest);
                            // Cluster
                            int intInstances = -1;
                            double dblQuorum = 0.00;
                            if (boolQA || boolProd)
                            {
                                // HA
                                if (drSummary["ha"].ToString() == "1")
                                {
                                    if (drSummary["ha_clustering"].ToString() == "1")
                                    {
                                        Int32.TryParse(drSummary["instances"].ToString(), out intInstances);
                                        double.TryParse(drSummary["quorum"].ToString(), out dblQuorum);
                                    }
                                }
                            }
                            int intCluster = 0;
                            if (intInstances > 0)
                            {
                                // Configure the cluster
                                intCluster = oCluster.Add(intRequest, "", intQuantity, (boolProd ? intQuantity : 0), 0);
                                oCluster.UpdateLocalNodes(intCluster, 1);
                                oCluster.UpdateNonShared(intCluster, 1);
                                oCluster.UpdateAddInstance(intCluster, 1);
                                // Configure storage drives (windows only)
                                if (IsWindows(_designid))
                                {
                                    if (boolSQL)
                                    {
                                        // P DRIVE
                                        oStorage.AddLun(intAnswer, 0, 0, 0, 0, -10, (boolProd ? 1.00 : 0.00), (boolQA ? 1.00 : 0.00), (boolTest || boolDev ? 1.00 : 0.00));
                                    }
                                    // Q DRIVE
                                    oStorage.AddLun(intAnswer, 0, 0, 0, 0, -1, (boolProd ? dblQuorum : 0.00), (boolQA ? dblQuorum : 0.00), (boolTest || boolDev ? dblQuorum : 0.00));
                                }
                                for (int ii = 1; ii <= intInstances; ii++)
                                {
                                    int intInstance = oCluster.AddInstance(intCluster, "", (boolSQL ? 1 : 0));
                                    // Add shared storage for each instance
                                    AddStorage(GetStorageShared(_designid, true), intAnswer, intCluster, intInstance, 0, ii, boolProd, boolQA, boolTest, boolDev, boolDR, boolProd);
                                }
                            }
                            for (int ii = 1; ii <= intQuantity; ii++)
                            {
                                // Add server record
                                int intServer = oServer.Add(intRequest, intAnswer, intModel, 0, intCluster, ii, intOS, intSP, 0, intDomain, 0, 0, 0, (boolProd || boolDR ? 1 : 0), 0, "", 0, 0, 1, 1, 1, 1, 0, 1, 0, 0);
                                // Add software components
                                DataSet dsSoftware = GetSoftwareComponents(_designid);
                                foreach (DataRow drSoftware in dsSoftware.Tables[0].Rows)
                                {
                                    int intComponent = Int32.Parse(drSoftware["componentid"].ToString());
                                    if (intComponent > 0)
                                    {
                                        oServerName.AddComponentDetailSelected(intServer, intComponent, 0, false);
                                        oServerName.AddComponentDetailPrerequisites(intServer, intComponent, false);
                                    }
                                }
                                // Add accounts
                                DataSet dsAccounts = GetAccounts(_designid);
                                foreach (DataRow drAccount in dsAccounts.Tables[0].Rows)
                                {
                                    int intAccount = Int32.Parse(drAccount["userid"].ToString());
                                    string strAccess = "";
                                    switch (drAccount["access"].ToString())
                                    {
                                        case "D":
                                            strAccess = "Developers_" + drAccount["remote"].ToString() + ";";
                                            break;
                                        case "P":
                                            strAccess = "Promoters_" + drAccount["remote"].ToString() + ";";
                                            break;
                                        case "S":
                                            strAccess = "AppSupport_" + drAccount["remote"].ToString() + ";";
                                            break;
                                        case "U":
                                            strAccess = "AppUsers_" + drAccount["remote"].ToString() + ";";
                                            break;
                                        default:
                                            strAccess = "";
                                            break;
                                    }
                                    oServer.AddAccount(intServer, oUser.GetName(intAccount), intDomain, 0, "", strAccess, 0);
                                }
                                // Add non-shared storage (if not a cluster)
                                AddStorage(GetStorageShared(_designid, false), intAnswer, 0, 0, ii, 1, boolProd, boolQA, boolTest, boolDev, boolDR, false);
                            }

                            // Configuration is done...now time to kick off the build.
                            oForecast.DeleteReset(intAnswer);
                            oServiceRequest.Add(intRequest, 1, 1);
                            oForecast.UpdateAnswerExecuted(intAnswer, DateTime.Now.ToString(), intRequestor);
                            if (oForecast.CanAutoProvision(intAnswer) == true)
                            {
                                // Only start the build for auto-provision enabled servers
                                oServer.Start(intRequest);
                            }
                            else
                            {
                                // Else set the step = 999
                                DataSet dsServers = oServer.GetAnswer(intAnswer);
                                foreach (DataRow drServer in dsServers.Tables[0].Rows)
                                    oServer.UpdateStep(Int32.Parse(drServer["id"].ToString()), 999);
                            }
                            bool boolNotify = oForecast.NotifyImplementor(intAnswer, intModel, intImplementorDistributed, intWorkstationPlatform, intImplementorMidrange, _environment, intRequestor, _dsn_asset, _dsn_ip);
                            intReturnAnswerID = intAnswer;
                        }
                        else
                        {
                            // Summary does not exist
                        }
                    }
                    else
                    {
                        // There was a problem executing the build...email the requestor.
                        oFunction.SendEmail("Design Approved - Mnemonic Pending", oUser.GetName(intRequestor), "", strEMailIdsBCC, "Design Approved - Mnemonic Pending", "<p><b>One of your designs has been completely approved, but is associated to a mnemonic that has yet to be approved.</b></p><p><b>NOTE:</b> Once the mnemonic has been approved, it is your responsibility to open the design and click the EXECUTE button to start the provisioning process.</p><p>" + GetSummary(_designid, _environment) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/design.aspx?id=" + _designid.ToString() + "\" target=\"_blank\">Click here to review this design.</a></p>", true, false);
                    }
                }
            }
            return intReturnAnswerID;
        }
        public int UpdateLocation(int _designid, string _dsn_asset)
        {
            Classes oClass = new Classes(0, dsn);
            Forecast oForecast = new Forecast(0, dsn);
            Asset oAsset = new Asset(0, _dsn_asset, dsn);
            int intClass = 0;
            int intEnv = 0;
            int intModel = 0;
            int intAnswer = 0;
            int intAddress = 0;
            int intProd = 0;
            int intQA = 0;
            int intTestDev = 0;

            Int32.TryParse(Get(_designid, "addressid"), out intAddress);
            if (intAddress == 0 && _dsn_asset != "")
            {
                if (Int32.TryParse(Get(_designid, "classid"), out intClass) == true)
                {
                    if (oClass.IsProd(intClass))
                        intProd = 1;
                    else if (oClass.IsQA(intClass))
                        intQA = 1;
                    else if (oClass.IsTestDev(intClass))
                        intTestDev = 1;
                    if (Int32.TryParse(Get(_designid, "environmentid"), out intEnv) == true)
                    {
                        if (Int32.TryParse(Get(_designid, "modelid"), out intModel) == true)
                        {
                            if (Int32.TryParse(Get(_designid, "answerid"), out intAnswer) == true)
                            {
                                intAddress = oAsset.GetServerOrBladeAvailableLocation(intClass, intEnv, intModel, intProd, intQA, intTestDev);
                                oForecast.UpdateAnswerLocation(intAnswer, intAddress);
                            }
                        }
                    }
                }
            }
            return intAddress;
        }

        private int GetUser(string _user)
        {
            int intUser = 0;
            Users oUser = new Users(user, dsn);
            while (_user.Contains("  ") == true)
                _user = _user.Replace("  ", " ");
            _user = _user.Trim();
            DataSet dsUser = oUser.Gets(_user);
            string strFirst = "";
            string strMiddle = "";
            string strLast = "";
            // Get First Name (before first " ")
            if (_user.Contains(" ") == true)
                strFirst = _user.Substring(0, _user.IndexOf(" "));
            _user = _user.Substring(_user.IndexOf(" ") + 1);
            if (_user.Contains(" ") == true)
            {
                // Get Middle Name (before second " ")
                strMiddle = _user.Substring(0, _user.IndexOf(" "));
                _user = _user.Substring(_user.IndexOf(" ") + 1);
            }
            // Get Last Name (before last " ")
            if (_user.Contains(" ") == true)
                strLast = _user.Substring(_user.LastIndexOf(" ") + 1);
            else
                strLast = _user;

            DataSet dsTemp = dsUser;
            string strUser = "";
            for (int ii = 0; ii < 7 && dsUser.Tables[0].Rows.Count != 1; ii++)
            {
                // Search for the user record using a variety of naming queries
                if (ii == 0)
                    strUser = strLast;
                else if (ii == 1)
                    strUser = strFirst;
                else if (ii == 2)
                    strUser = strFirst + " " + strLast;
                else if (ii == 3)
                    strUser = strLast + " " + strFirst;
                else if (ii == 4)
                    strUser = strFirst + " " + strMiddle + " " + strLast;
                else if (ii == 5)
                    strUser = strMiddle + " " + strLast;
                else if (ii == 6)
                    strUser = strFirst + " " + strMiddle;

                dsUser = oUser.Gets(strUser);
                if (dsUser.Tables[0].Rows.Count > 0)
                    dsTemp = dsUser;
            }

            // Return the user record (or the closest record)
            if (dsUser.Tables[0].Rows.Count == 1)
                Int32.TryParse(dsUser.Tables[0].Rows[0]["userid"].ToString(), out intUser);
            else if (dsTemp.Tables[0].Rows.Count > 0)
                Int32.TryParse(dsTemp.Tables[0].Rows[0]["userid"].ToString(), out intUser);

            return intUser;
        }
        private void AddStorage(DataSet dsStorage, int _answerid, int _clusterid, int _instanceid, int _number, int _driveid, bool _prod, bool _qa, bool _test, bool _dev, bool _dr, bool _replicate)
        {
            Storage oStorage = new Storage(user, dsn);
            foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
            {
                int intDrive = 0;
                Int32.TryParse(drStorage["driveid"].ToString(), out intDrive);
                double dblSize = double.Parse(drStorage["size"].ToString());
                oStorage.AddLun(_answerid, _instanceid, _clusterid, 0, _number, (intDrive < 0 ? intDrive : _driveid), "Standard", drStorage["path"].ToString(), (_prod ? dblSize : 0.00), (_qa ? dblSize : 0.00), (_test || _dev ? dblSize : 0.00), (_replicate ? 1 : 0), 0);
            }
        }
        private bool NotifyGroup(int _designid, int _userid_cc, bool _only_exceptions, int _environment)
        {
            bool boolSent = false;
            DataSet dsGroup = GetApproverGroups((_only_exceptions ? 1 : 0), 1);
            foreach (DataRow drGroup in dsGroup.Tables[0].Rows)
            {
                int intGroup = Int32.Parse(drGroup["groupid"].ToString());
                // First, check to make sure we haven't already submitted it.
                bool boolAlready = false;
                DataSet dsAlready = GetApproverGroupWorkflow(_designid, intGroup);
                foreach (DataRow drAlready in dsAlready.Tables[0].Rows)
                {
                    if (drAlready["rejected"].ToString() != "-1")
                    {
                        boolAlready = true;
                        break;
                    }
                }
                if (boolAlready == false)
                {
                    // Submit it
                    boolSent = true;
                    AddApproverGroupWorkflow(_designid, intGroup);
                    // Get Users
                    DataSet dsUser = GetApprovalUsers(intGroup);
                    foreach (DataRow drUser in dsUser.Tables[0].Rows)
                        Notify(_designid, Int32.Parse(drUser["userid"].ToString()), _userid_cc, _environment);
                }
            }
            return boolSent;
        }
        private void Notify(int _designid, int _userid, int _userid_cc, int _environment)
        {
            string strURL = "";
            int intPage = 0;
            Settings oSetting = new Settings(user, dsn);
            Users oUser = new Users(user, dsn);
            Pages oPage = new Pages(user, dsn);
            Functions oFunction = new Functions(user, dsn, _environment);
            Variables oVariable = new Variables(_environment);
            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DESIGN_WORKFLOW");

            if (Int32.TryParse(oSetting.Get("design_approval_page"), out intPage) == true)
                strURL = oUser.GetApplicationUrl(_userid, intPage);
            if (strURL == "")
                oFunction.SendEmail("Design Approval (#" + _designid.ToString() + ")", oUser.GetName(_userid), oUser.GetName(_userid_cc), strEMailIdsBCC, "Design Approval (#" + _designid.ToString() + ")", "<p>" + oUser.GetFullName(_userid) + ",</p><p><b>A design has been submitted that requires your approval; you are required to approve or deny this request.</b></p><p>" + GetSummary(_designid, _environment) + "</p>", true, false);
            else
                oFunction.SendEmail("Design Approval (#" + _designid.ToString() + ")", oUser.GetName(_userid), oUser.GetName(_userid_cc), strEMailIdsBCC, "Design Approval (#" + _designid.ToString() + ")", "<p>" + oUser.GetFullName(_userid) + ",</p><p><b>A design has been submitted that requires your approval; you are required to approve or deny this request.</b></p><p>" + GetSummary(_designid, _environment) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strURL + oPage.GetFullLink(intPage) + "?id=" + _designid.ToString() + "\" target=\"_blank\">Click here to review this request.</a></p>" + oVariable.EmailFooter(), true, false);
        }
        public DataSet LoadWorkflow(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApproverWorkflows", arParams);

        }




        // *****************************************************************************************
        // *****************************************************************************************
        // **   EXCLUSION CONFIGURATION
        // *****************************************************************************************
        // *****************************************************************************************
        public DataSet GetExclusions(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignExclusions", arParams);
        }
        public void AddExclusion(int _designid, string _path)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@path", _path);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignExclusion", arParams);
        }
        public void DeleteExclusions(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignExclusion", arParams);
        }



        // *****************************************************************************************
        // *****************************************************************************************
        // **   GRID CONFIGURATION
        // *****************************************************************************************
        // *****************************************************************************************
        public DataSet GetBackup(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignBackup", arParams);
        }
        public int GetBackupCount(int _designid)
        {
            return GetGridCount(GetBackup(_designid));
        }
        public void AddBackup(int _designid, HttpRequest _request)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@sun", GetGrid(_request.Form["hdnBackup"], 1));
            arParams[2] = new SqlParameter("@mon", GetGrid(_request.Form["hdnBackup"], 2));
            arParams[3] = new SqlParameter("@tue", GetGrid(_request.Form["hdnBackup"], 3));
            arParams[4] = new SqlParameter("@wed", GetGrid(_request.Form["hdnBackup"], 4));
            arParams[5] = new SqlParameter("@thu", GetGrid(_request.Form["hdnBackup"], 5));
            arParams[6] = new SqlParameter("@fri", GetGrid(_request.Form["hdnBackup"], 6));
            arParams[7] = new SqlParameter("@sat", GetGrid(_request.Form["hdnBackup"], 7));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignBackup", arParams);
        }
        public DataSet GetMaintenance(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignMaintenance", arParams);
        }
        public int GetMaintenanceCount(int _designid)
        {
            return GetGridCount(GetMaintenance(_designid));
        }
        public void AddMaintenance(int _designid, HttpRequest _request)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@sun", GetGrid(_request.Form["hdnMaintenance"], 1));
            arParams[2] = new SqlParameter("@mon", GetGrid(_request.Form["hdnMaintenance"], 2));
            arParams[3] = new SqlParameter("@tue", GetGrid(_request.Form["hdnMaintenance"], 3));
            arParams[4] = new SqlParameter("@wed", GetGrid(_request.Form["hdnMaintenance"], 4));
            arParams[5] = new SqlParameter("@thu", GetGrid(_request.Form["hdnMaintenance"], 5));
            arParams[6] = new SqlParameter("@fri", GetGrid(_request.Form["hdnMaintenance"], 6));
            arParams[7] = new SqlParameter("@sat", GetGrid(_request.Form["hdnMaintenance"], 7));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignMaintenance", arParams);
        }

        public string GetGrid(string _hidden, int _day)
        {
            StringBuilder strReturn = new StringBuilder();
            strReturn.Append(GetGrid(_hidden, _day, "12:00 AM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "1:00 AM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "2:00 AM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "3:00 AM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "4:00 AM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "5:00 AM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "6:00 AM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "7:00 AM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "8:00 AM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "9:00 AM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "10:00 AM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "11:00 AM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "12:00 PM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "1:00 PM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "2:00 PM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "3:00 PM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "4:00 PM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "5:00 PM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "6:00 PM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "7:00 PM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "8:00 PM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "9:00 PM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "10:00 PM", strAppend));
            strReturn.Append(GetGrid(_hidden, _day, "11:00 PM", strAppend));
            return strReturn.ToString();
        }
        private string GetGrid(string _hidden, int _day, string _time, string _append)
        {
            string strReturn = "1"; // unchecked
            string[] strSelections = _hidden.Split(new char[] { '&' });
            foreach (string strSelection in strSelections)
            {
                string[] strValues = strSelection.Split(new char[] { '_' });
                int intDay = 0;
                if (Int32.TryParse(strValues[0], out intDay) && intDay == _day)
                {
                    // Days match...check time
                    if (_time == strValues[1])
                    {
                        // Times match...return selection
                        strReturn = strValues[2];
                        break;
                    }
                }
            }
            return strReturn + _append;
        }
        private int GetGridCount(DataSet _ds)
        {
            int intCount = 0;
            foreach (DataRow dr in _ds.Tables[0].Rows)
            {
                intCount += (dr["sun"].ToString().Split('0').Length - 1);
                intCount += (dr["mon"].ToString().Split('0').Length - 1);
                intCount += (dr["tue"].ToString().Split('0').Length - 1);
                intCount += (dr["wed"].ToString().Split('0').Length - 1);
                intCount += (dr["thu"].ToString().Split('0').Length - 1);
                intCount += (dr["fri"].ToString().Split('0').Length - 1);
                intCount += (dr["sat"].ToString().Split('0').Length - 1);
            }
            return intCount;
        }
        public string LoadGrid(string _value, int _day, ref int _counter)
        {
            StringBuilder strReturn = new StringBuilder();
            strReturn.Append(LoadGrid(_value, _day, 0, "12:00 AM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 1, "1:00 AM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 2, "2:00 AM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 3, "3:00 AM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 4, "4:00 AM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 5, "5:00 AM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 6, "6:00 AM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 7, "7:00 AM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 8, "8:00 AM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 9, "9:00 AM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 10, "10:00 AM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 11, "11:00 AM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 12, "12:00 PM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 13, "1:00 PM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 14, "2:00 PM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 15, "3:00 PM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 16, "4:00 PM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 17, "5:00 PM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 18, "6:00 PM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 19, "7:00 PM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 20, "8:00 PM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 21, "9:00 PM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 22, "10:00 PM", ref _counter));
            strReturn.Append(LoadGrid(_value, _day, 23, "11:00 PM", ref _counter));
            return strReturn.ToString();
        }
        private string LoadGrid(string _value, int _day, int _index, string _time, ref int _counter)
        {
            if (_value[_index] == '0')
            {
                _counter++;
                return _day.ToString() + "_" + _time + "_0&";
            }
            else
                return "";
        }



        // *****************************************************************************************
        // *****************************************************************************************
        // **   STORAGE CONFIGURATION
        // *****************************************************************************************
        // *****************************************************************************************
        public DataSet GetStorage(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignStorages", arParams);
        }
        public DataSet GetStorage(int _designid, int _driveid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@driveid", _driveid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignStorage", arParams);
        }
        public DataSet GetStorageShared(int _designid, bool _shared)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@shared", (_shared ? 1 : 0));
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignStorageShared", arParams);
        }
        public DataSet GetStorageID(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignStorageID", arParams);
        }
        public string GetStorageID(int _id, string _column)
        {
            DataSet ds = GetStorageID(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddLun(int _designid, int _driveid, string _path, string _size, bool _shared)
        {
            int intSize = 0;
            Int32.TryParse(_size, out intSize);
            bool boolIsSQL = IsSQL(_designid);
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@driveid", _driveid);
            arParams[2] = new SqlParameter("@path", _path);
            arParams[3] = new SqlParameter("@size", intSize);
            arParams[4] = new SqlParameter("@shared", (_shared ? 1 : 0));
            arParams[5] = new SqlParameter("@db", (boolIsSQL ? 1 : 0));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignStorage", arParams);
        }
        public void UpdateLun(int _id, int _driveid, string _path, string _size, bool _shared)
        {
            int intSize = 0;
            Int32.TryParse(_size, out intSize);
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@driveid", _driveid);
            arParams[2] = new SqlParameter("@path", _path);
            arParams[3] = new SqlParameter("@size", intSize);
            arParams[4] = new SqlParameter("@shared", (_shared ? 1 : 0));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignStorage", arParams);
        }
        public void DeleteLun(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignStorage", arParams);
        }
        public void DeleteLuns(int _designid, int _db)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@db", _db);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignStorages", arParams);
        }
        public DataSet GetDrives()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorageDriveLetters");
        }
        public void AddLunSQLPNC(int _designid, double _size, double _non, double _percent, double _tempDB, double _compressionPercentage, double _tempDBOverhead, bool _2008)
        {
            double dblLunMin = 500.00;
            double dblLunMax = 750.00;

            Forecast oForecast = new Forecast(0, dsn);
            Classes oClass = new Classes(0, dsn);
            _percent = (_percent / 100.00);
            double dblLUN = 0.00;

            double dblOverallSize = (_size * (1.00 + _percent + 0.05));
            double dblDividend = dblOverallSize / dblLunMax;
            dblDividend = Math.Ceiling(dblDividend);
            //Response.Write(dblDividend.ToString("0") + "<br/>");
            double dblEachLUN = (dblOverallSize / dblDividend);
            //Response.Write(dblEachLUN.ToString() + "<br/>");
            dblEachLUN = RoundUp(dblEachLUN);
            dblOverallSize = (dblEachLUN * dblDividend);
            //Response.Write(dblEachLUN.ToString() + " x " + dblDividend.ToString() + " = " + dblOverallSize.ToString() + "<br/>");


            int intDrive = GetNextDrive(_designid);
            // R:
            dblLUN = 1.00;
            AddLun(_designid, intDrive, "", dblLUN.ToString(), true);


            // R:\Production
            dblLUN = 10.00;
            AddLun(_designid, intDrive, "\\Production", dblLUN.ToString(), true);


            if (IsCluster(_designid))
            {
                // R:\Production\OEM
                dblLUN = 10.00;
                AddLun(_designid, intDrive, "\\Production\\oracle_oem", dblLUN.ToString(), true);
            }


            // R:\Production\Database\SQL01 ***
            dblLUN = dblOverallSize;
            double dblLUN_Database = Minimum(dblLUN);
            for (double dblStart = 1.00; dblStart <= dblDividend; dblStart += 1.00)
            {
                string strBefore = "";
                if (dblStart.ToString().Length == 1)
                    strBefore = "0";
                AddLun(_designid, intDrive, "\\Production\\Database\\SQL" + strBefore + dblStart.ToString(), Minimum(dblEachLUN).ToString(), true);
            }


            // R:\Production\Logs
            dblLUN = dblOverallSize;
            if (dblLUN > 2000.00)
                dblLUN = 2000.00;   // 2 TB is maximum for LOGS
            double dblLUN_Logs = Minimum(dblLUN);
            AddLun(_designid, intDrive, "\\Production\\Logs", Minimum(dblLUN).ToString(), true);


            // R:\Production\TempDB
            //dblLUN = (_tempDB * _tempDBOverhead);
            if (_tempDB == 0.00)
            {
                // Calcualate tempDB dynamically
                _tempDB = (_size * .02);                                // 2% of database size
                _tempDB += (_size * _percent);                          // Plus x% of largest table
            }
            dblLUN = RoundUp(_tempDB + (_tempDB * _tempDBOverhead));    // Plus OverHead value
            AddLun(_designid, intDrive, "\\Production\\TempDB", Minimum(dblLUN).ToString(), true);


            // R:\Production\Backups\SQL01*
            double dblBackups = dblOverallSize;
            if (_2008 == true)
                dblBackups = ((dblLUN_Logs + dblLUN_Database) * _compressionPercentage);
            else
                dblBackups = (dblLUN_Logs + dblLUN_Database);
            if (dblBackups < dblLunMax)
            {
                // Save backups to one single LUN
                AddLun(_designid, intDrive, "\\Production\\Backups\\SQL01", Minimum(dblBackups).ToString(), true);
            }
            else
            {
                if (dblBackups == dblLUN_Database)
                {
                    // Same configuration as Database LUNs
                    for (double dblStart = 1.00; dblStart <= (dblDividend * 1.00); dblStart += 1.00)
                    {
                        string strBefore = "";
                        if (dblStart.ToString().Length == 1)
                            strBefore = "0";
                        AddLun(_designid, intDrive, "\\Production\\Backups\\SQL" + strBefore + dblStart.ToString(), Minimum(dblEachLUN).ToString(), true);
                    }
                }
                else if (dblBackups == (dblLUN_Database * 2.00))
                {
                    // Double the configuration of Database LUNs
                    for (double dblStart = 1.00; dblStart <= (dblDividend * 2.00); dblStart += 1.00)
                    {
                        string strBefore = "";
                        if (dblStart.ToString().Length == 1)
                            strBefore = "0";
                        AddLun(_designid, intDrive, "\\Production\\Backups\\SQL" + strBefore + dblStart.ToString(), Minimum(dblEachLUN).ToString(), true);
                    }
                }
                else
                {
                    // Neither of the above, split them out
                    double dblDividendB = dblBackups / dblLunMax;
                    dblDividendB = Math.Ceiling(dblDividendB);
                    double dblEachLUNBackup = (dblBackups / dblDividendB);
                    dblEachLUNBackup = RoundUp(dblEachLUNBackup);
                    dblBackups = (dblEachLUNBackup * dblDividendB);

                    for (double dblStart = 1.00; dblStart <= dblDividendB; dblStart += 1.00)
                    {
                        string strBefore = "";
                        if (dblStart.ToString().Length == 1)
                            strBefore = "0";
                        AddLun(_designid, intDrive, "\\Production\\Backups\\SQL" + strBefore + dblStart.ToString(), Minimum(dblEachLUNBackup).ToString(), true);
                    }
                }
            }


            // R:\AppData
            if (_non > 0.00)
                AddLun(_designid, intDrive, "\\AppData", Minimum(_non).ToString(), false);
        }
        private double RoundUp(double _size)
        {
            _size = Math.Ceiling(_size);
            while (_size % 5.00 != 0.00)
                _size += 1.00;
            return _size;
        }
        private double Minimum(double _size)
        {
            if (_size > 0.00 && _size < 10.00)
                _size = 10.00;
            return Math.Ceiling(_size);
        }
        private int GetNextDrive(int _designid)
        {
            int intReturn = 0;
            DataSet dsStorage = GetStorage(_designid);
            // First Check to be sure database is correct
            foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
            {
                int intDrive = 0;
                if (Int32.TryParse(drStorage["driveid"].ToString(), out intDrive) == true)
                {
                    if (intDrive > intReturn)
                        intReturn = intDrive;
                }
            }
            return intReturn + 1;
        }
        public int GetStorageTotal(int _designid)
        {
            int intTotal = 0;
            int intTemp = 0;
            DataSet dsStorage = GetStorage(_designid);
            foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
            {
                if (Int32.TryParse(drStorage["size"].ToString(), out intTemp) == true)
                    intTotal += intTemp;
            }
            DataSet dsApp = GetStorage(_designid, -1000);
            if (dsApp.Tables[0].Rows.Count > 0 && IsWindows(_designid))
            {
                if (Int32.TryParse(dsApp.Tables[0].Rows[0]["size"].ToString(), out intTemp) == true)
                    intTotal += intTemp;
            }
            return intTotal;
        }



        // *****************************************************************************************
        // *****************************************************************************************
        // **   ACCOUNT CONFIGURATION
        // *****************************************************************************************
        // *****************************************************************************************
        public DataSet GetAccounts(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignAccounts", arParams);
        }
        public bool AddAccount(int _designid, int _userid, string _access, int _remote)
        {
            bool boolDuplicate = false;
            DataSet dsAccount = GetAccounts(_designid);
            foreach (DataRow drAccount in dsAccount.Tables[0].Rows)
            {
                if (drAccount["userid"].ToString() == _userid.ToString())
                {
                    boolDuplicate = true;
                    break;
                }
            }
            if (boolDuplicate == false)
            {
                arParams = new SqlParameter[4];
                arParams[0] = new SqlParameter("@designid", _designid);
                arParams[1] = new SqlParameter("@userid", _userid);
                arParams[2] = new SqlParameter("@access", _access);
                arParams[3] = new SqlParameter("@remote", _remote);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignAccount", arParams);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void DeleteAccount(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignAccount", arParams);
        }



        // *****************************************************************************************
        // *****************************************************************************************
        // **   SOFTWARE COMPONENT CONFIGURATION
        // *****************************************************************************************
        // *****************************************************************************************
        public bool IsSoftwareComponent(int _designid, int _componentid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@componentid", _componentid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignSoftwareComponent", arParams);
            return (ds.Tables[0].Rows.Count > 0);
        }
        public DataSet GetSoftwareComponents(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignSoftwareComponents", arParams);
        }
        public void AddSoftwareComponent(int _designid, int _componentid, int _responseid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@componentid", _componentid);
            arParams[2] = new SqlParameter("@responseid", _responseid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignSoftwareComponent", arParams);
        }
        public void DeleteSoftwareComponent(int _designid, int _componentid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@componentid", _componentid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignSoftwareComponent", arParams);
        }
        public void DeleteSoftwareComponents(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignSoftwareComponents", arParams);
        }
        public void UpdateSoftwareComponents(int _designid, int _componentid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@componentid", _componentid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignSoftwareComponents", arParams);
        }
        public void UpdateSoftwareComponent(int _id, int _userid, int _rejected, string _reason)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@rejected", _rejected);
            arParams[3] = new SqlParameter("@reason", _reason);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignSoftwareComponent", arParams);
        }






        // *****************************************************************************************
        // *****************************************************************************************
        // **   DESIGN CONFIGURATION
        // *****************************************************************************************
        // *****************************************************************************************
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesign", arParams);
        }
        public void AddPhaseCompletion(int _designid, int _phaseid, int _userid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@phaseid", _phaseid);
            arParams[2] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignPhaseCompletion", arParams);
        }
        public DataSet GetPhaseCompletion(int _designid, int _phaseid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@phaseid", _phaseid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignPhaseCompletion", arParams);
        }
        public string GetValue(int _id, int _phaseid)
        {
            string strReturn = "";
            Users oUser = new Users(0, dsn);
            DataSet dsQuestion = GetQuestions(_phaseid, 1);
            foreach (DataRow drQuestion in dsQuestion.Tables[0].Rows)
            {
                int intQuestion = Int32.Parse(drQuestion["id"].ToString());

                if (IsHidden(_id, intQuestion, null) == false && drQuestion["show_summary"].ToString() == "1")
                {
                    bool boolDropDown = (drQuestion["is_type_drop_down"].ToString() == "1");
                    bool boolCheckBox = (drQuestion["is_type_check_box"].ToString() == "1");
                    bool boolRadio = (drQuestion["is_type_radio"].ToString() == "1");
                    bool boolTextBox = (drQuestion["is_type_textbox"].ToString() == "1");
                    bool boolTextArea = (drQuestion["is_type_textarea"].ToString() == "1");
                    string strFieldQuestion = drQuestion["related_field"].ToString();
                    string strValueQuestion = drQuestion["default_value"].ToString();
                    string strSuffix = drQuestion["suffix"].ToString();

                    string strSpecial = GetQuestionSpecial(intQuestion);
                    switch (strSpecial)
                    {
                        case "MNEMONIC":
                            Mnemonic oMnemonic = new Mnemonic(user, dsn);
                            int intMnemonic = 0;
                            if (Int32.TryParse(Get(_id, strFieldQuestion), out intMnemonic) == true && intMnemonic > 0)
                                strReturn = AddReturn(strReturn, oMnemonic.Get(intMnemonic, "factory_code"));
                            break;
                        case "COST_CENTER":
                            CostCenter oCostCenter = new CostCenter(user, dsn);
                            int intCost = 0;
                            if (Int32.TryParse(Get(_id, strFieldQuestion), out intCost) == true && intCost > 0)
                                strReturn = AddReturn(strReturn, oCostCenter.GetName(intCost));
                            break;
                        case "USER_SI":
                            int intSI = 0;
                            if (Int32.TryParse(Get(_id, strFieldQuestion), out intSI) == true && intSI > 0)
                                strReturn = AddReturn(strReturn, oUser.GetFullName(intSI) + " (" + oUser.GetName(intSI) + ")");
                            break;
                        case "USER_DTG":
                            int intDTG = 0;
                            if (Int32.TryParse(Get(_id, strFieldQuestion), out intDTG) == true && intDTG > 0)
                                strReturn = AddReturn(strReturn, oUser.GetFullName(intDTG) + " (" + oUser.GetName(intDTG) + ")");
                            break;
                        case "GRID_BACKUP":
                            strReturn = GetBackupCount(_id).ToString() + " Conflict(s)";
                            break;
                        case "BACKUP_EXCLUSION":
                            strReturn = GetExclusions(_id).Tables[0].Rows.Count.ToString() + " Exclusion(s)";
                            break;
                        case "GRID_MAINTENANCE":
                            strReturn = GetMaintenanceCount(_id).ToString() + " Conflict(s)";
                            break;
                        case "STORAGE_LUNS":
                            strReturn = GetStorageTotal(_id).ToString("N0") + " GB(s)";
                            break;
                        case "ACCOUNTS":
                            strReturn = GetAccounts(_id).Tables[0].Rows.Count.ToString() + " Account(s)";
                            break;
                        case "DATE":
                            string strDate = Get(_id, "commitment");
                            if (strDate != "")
                                strReturn = AddReturn(strReturn, DateTime.Parse(strDate).ToShortDateString());
                            break;
                        default:
                            DataSet dsResponse = GetResponses(intQuestion, 0, 1);
                            if (boolTextBox || boolTextArea)
                            {
                                strReturn = AddReturn(strReturn, Get(_id, strFieldQuestion));
                            }
                            else
                            {
                                foreach (DataRow drResponse in dsResponse.Tables[0].Rows)
                                {
                                    int intClass = 0;
                                    int intOS = 0;
                                    int intEnv = 0;
                                    int intComponentID = 0;
                                    // Get related field
                                    string strField = (strFieldQuestion == "" ? drResponse["related_field"].ToString() : drQuestion["related_field"].ToString());
                                    // Get configured value
                                    string strSet = drResponse["related_value"].ToString();
                                    if (strField != "")
                                    {
                                        // Get value
                                        string strValue = Get(_id, strField);

                                        // If either the class, os or environment are selected...no need to go through the individual results to find 
                                        // the selected item.  Just get it straight from the column (strField).
                                        if (Int32.TryParse(drResponse["set_classid"].ToString(), out intClass) && intClass > 0)
                                        {
                                            // Must be Class Question....return selected class
                                            Classes oClass = new Classes(user, dsn);
                                            if (strField == "")
                                                strReturn = AddReturn(strReturn, "Invalid ClsssID Configuration");
                                            else
                                            {
                                                if (Int32.TryParse(strValue, out intClass) == true)
                                                {
                                                    strReturn = AddReturn(strReturn, oClass.Get(intClass, "name"));
                                                    ResponseID = Int32.Parse(drResponse["id"].ToString());
                                                }
                                            }
                                            break;
                                        }
                                        else if (Int32.TryParse(drResponse["set_osid"].ToString(), out intOS) && intOS > 0)
                                        {
                                            OperatingSystems oOperatingSystem = new OperatingSystems(user, dsn);
                                            if (strField == "")
                                                strReturn = AddReturn(strReturn, "Invalid OsID Configuration");
                                            else
                                            {
                                                if (Int32.TryParse(strValue, out intOS) == true)
                                                {
                                                    strReturn = AddReturn(strReturn, oOperatingSystem.Get(intOS, "name"));
                                                    ResponseID = Int32.Parse(drResponse["id"].ToString());
                                                }
                                            }
                                            break;
                                        }
                                        else if (Int32.TryParse(drResponse["set_environmentid"].ToString(), out intEnv) && intEnv > 0)
                                        {
                                            Environments oEnvironment = new Environments(user, dsn);
                                            if (strField == "")
                                                strReturn = AddReturn(strReturn, "Invalid EnvironmentID Configuration");
                                            else
                                            {
                                                if (Int32.TryParse(strValue, out intEnv) == true)
                                                {
                                                    strReturn = AddReturn(strReturn, oEnvironment.Get(intEnv, "name"));
                                                    ResponseID = Int32.Parse(drResponse["id"].ToString());
                                                }
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            if (strValue == strSet)
                                            {
                                                // Check to make sure the selections were selected.
                                                int intResponseTemp = Int32.Parse(drResponse["id"].ToString());
                                                if (IsShowResponse(intResponseTemp, _id) == true)
                                                {
                                                    // This option was selected....show response.
                                                    strReturn = AddReturn(strReturn, drResponse["summary"].ToString());
                                                    ResponseID = intResponseTemp;
                                                    //break;
                                                }
                                            }
                                        }
                                    }
                                    else if (Int32.TryParse(drResponse["set_componentid"].ToString(), out intComponentID) && intComponentID > 0)
                                    {
                                        if (IsSoftwareComponent(_id, intComponentID) == true)
                                        {
                                            strReturn = AddReturn(strReturn, drResponse["summary"].ToString());
                                            ResponseID = Int32.Parse(drResponse["id"].ToString());
                                        }
                                        //break;
                                    }
                                    else
                                        strReturn = AddReturn(strReturn, "*** Missing FIELD value for Response = " + drResponse["response"].ToString() + " ***");
                                }
                            }
                            if (strReturn == "" && strValueQuestion != "")
                            {
                                strReturn = strValueQuestion;
                                // Try to resolve the provided response with the actual (verbiage) response
                                foreach (DataRow drResponse in dsResponse.Tables[0].Rows)
                                {
                                    if (drResponse["related_value"].ToString() == strValueQuestion)
                                    {
                                        strReturn = drResponse["summary"].ToString();
                                        break;
                                    }
                                }
                            }
                            break;
                    }
                    if (strReturn != "" && strSuffix != "")
                        strReturn += "&nbsp;" + strSuffix;
                }
            }
            return strReturn;
        }
        public string GetException(int _id, int _phaseid)
        {
            string strReturn = "";
            DataSet dsQuestion = GetQuestions(_phaseid, 1);
            foreach (DataRow drQuestion in dsQuestion.Tables[0].Rows)
            {
                int intQuestion = Int32.Parse(drQuestion["id"].ToString());
                if (IsHidden(_id, intQuestion, null) == false)
                {
                    bool boolTextBox = (drQuestion["is_type_textbox"].ToString() == "1");
                    bool boolTextArea = (drQuestion["is_type_textarea"].ToString() == "1");

                    DataSet dsResponse = GetResponses(intQuestion, 0, 1);
                    foreach (DataRow drResponse in dsResponse.Tables[0].Rows)
                    {
                        // Get related field
                        string strField = (drQuestion["related_field"].ToString() == "" ? drResponse["related_field"].ToString() : drQuestion["related_field"].ToString());
                        // Get configured value
                        string strSet = drResponse["related_value"].ToString();
                        if (strField != "")
                        {
                            // Get value
                            string strValue = Get(_id, strField);

                            int intTemp = 0;
                            if (Int32.TryParse(drResponse["set_classid"].ToString(), out intTemp) && intTemp > 0)
                            {
                                // Response is set to a ClassID
                                if (strValue == intTemp.ToString())
                                {
                                    // This ClassID was selected...check exception flag for response
                                    if (drResponse["is_exception"].ToString() == "1")
                                        strReturn = drResponse["response"].ToString();
                                }
                            }
                            else if (Int32.TryParse(drResponse["set_osid"].ToString(), out intTemp) && intTemp > 0)
                            {
                                // Response is set to a OsID
                                if (strValue == intTemp.ToString())
                                {
                                    // This OsID was selected...check exception flag for response
                                    if (drResponse["is_exception"].ToString() == "1")
                                        strReturn = drResponse["response"].ToString();
                                }
                            }
                            else if (Int32.TryParse(drResponse["set_environmentid"].ToString(), out intTemp) && intTemp > 0)
                            {
                                // Response is set to a EnvironmentID
                                if (strValue == intTemp.ToString())
                                {
                                    // This EnvironmentID was selected...check exception flag for response
                                    if (drResponse["is_exception"].ToString() == "1")
                                        strReturn = drResponse["response"].ToString();
                                }
                            }
                            else
                            {
                                if (boolTextBox || boolTextArea)
                                {
                                    // No text can be an exception
                                }
                                else
                                {
                                    if (strValue == strSet && drResponse["is_exception"].ToString() == "1")
                                        strReturn = drResponse["response"].ToString();
                                }
                            }
                            if (strReturn != "")
                            {
                                // Found at least one exception...no need to continue
                                break;
                            }
                        }
                    }
                }
            }
            return strReturn;
        }
        private string AddReturn(string _return, string _value)
        {
            string strReturn = _return;
            if (strReturn != _value)
            {
                if (strReturn != "")
                {
                    //_return += "<br/>";
                    if (strReturn.ToUpper() == "YES")
                        strReturn = "";
                    else
                        strReturn += ", ";
                }
                strReturn += _value;
            }
            if (strReturn.ToUpper() != "NONE" && strReturn.ToUpper().Contains("NONE") == true)
            {
                // Remove NONE
                if (strReturn.ToUpper().Contains("NONE,") == true)
                {
                    string strBefore = strReturn.Substring(0, strReturn.ToUpper().IndexOf("NONE,")).Trim();
                    string strAfter = strReturn.Substring(strReturn.ToUpper().IndexOf("NONE,") + 5).Trim();
                    strReturn = strBefore + strAfter;
                }
                else
                {
                    string strBefore = strReturn.Substring(0, strReturn.ToUpper().IndexOf("NONE")).Trim();
                    string strAfter = strReturn.Substring(strReturn.ToUpper().IndexOf("NONE") + 4).Trim();
                    if (strBefore[strBefore.Length - 1] == ',')
                        strBefore = strBefore.Substring(0, strBefore.Length - 1);
                    strReturn = strBefore + strAfter;
                }
            }
            return strReturn;
        }
        public int AddValue(int _id, int _forecastid, int _phaseid, int _questionid, string _field, string _value) 
        {
            // The value should be like this...."7_Dev" where "7" is the responseid and "Dev" is the actual value.  Split them....
            int intResponse = 0;
            if (_value.Contains("_") == true) 
            {
                intResponse = Int32.Parse(_value.Substring(0, _value.IndexOf("_")));
                _value = _value.Substring(_value.IndexOf("_") + 1);
            }
            
            bool boolWrongQuestion = false;
            int intQuestion = 0;
            if (intResponse > 0 && Int32.TryParse(GetResponse(intResponse, "questionid"), out intQuestion) == true)
            {
                if (intQuestion != _questionid)
                    boolWrongQuestion = true;
            }

            int intComponentID = 0;
            if (intResponse > 0)
                Int32.TryParse(GetResponse(intResponse, "set_componentid"), out intComponentID);

            if (boolWrongQuestion == false)
            {
                // Get type (so we know how to create the UPDATE statement)
                string strType = GetDataType(_field);

                string strSQL = _value;
                // Get rid of trailing ','...(exists if checkbox)
                if (strSQL.Length > 0 && strSQL[strSQL.Length - 1] == ',')
                    strSQL = strSQL.Substring(0, strSQL.Length - 1);
                if (strType == "DATETIME")
                {
                    if (strSQL != "")
                        strSQL = "'" + DateTime.Parse(strSQL).ToShortDateString() + "'";
                    else
                        strSQL = "null";
                }
                else if (strType == "INT")
                {
                    if (strSQL == "")
                        strSQL = "0";
                }
                else if (strType == "VARCHAR")
                {
                    if (strSQL.Contains("'") == true)
                        strSQL = "'" + strSQL.Replace("'", "''") + "'";
                    else
                        strSQL = "'" + strSQL + "'";
                }

                if (strType != "")
                {
                    if (_id == 0)
                    {
                        arParams = new SqlParameter[1];
                        arParams[0] = new SqlParameter("@id", SqlDbType.Int);
                        arParams[0].Direction = ParameterDirection.Output;
                        strSQL = "INSERT INTO " + strTable + " ([forecastid], [" + _field + "], [phaseid], [created], [modified], [deleted]) VALUES (" + _forecastid.ToString() + ", " + strSQL + ", " + _phaseid.ToString() + ", getdate(), getdate(), 0) SET @id = SCOPE_IDENTITY()";
                        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, strSQL, arParams);
                        _id = Int32.Parse(arParams[0].Value.ToString());
                    }
                    else
                    {
                        strSQL = "UPDATE " + strTable + " SET [" + _field + "] = " + strSQL + ", modified = getdate() WHERE id = " + _id.ToString();
                        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, strSQL);
                    }
                }
                else
                {
                    if (intComponentID == 0)
                        Int32.TryParse(_field, out intComponentID);

                    if (intComponentID > 0)
                    {
                        if (strSQL == "")
                            strSQL = "0";
                        if (strSQL == "0")
                        {
                            // Remove it
                            DeleteSoftwareComponent(_id, intComponentID);
                        }
                        else
                        {
                            // Add it if it doesn't exist.
                            if (IsSoftwareComponent(_id, intComponentID) == false)
                                AddSoftwareComponent(_id, intComponentID, intResponse);
                        }
                    }
                }

                // Add corresponding value(s)
                bool boolVisibleSet = false;
                DataSet dsSelection = GetSelections(intResponse);
                foreach (DataRow drSelection in dsSelection.Tables[0].Rows)
                {
                    int intSet = Int32.Parse(drSelection["setid"].ToString());
                    DataSet dsResponse = GetResponse(intSet);
                    foreach (DataRow drResponse in dsResponse.Tables[0].Rows)
                    {
                        if (drResponse["visible"].ToString() == "1")
                        {
                            boolVisibleSet = true;
                            break;
                        }
                    }
                    if (boolVisibleSet == true)
                        break;
                }
                foreach (DataRow drSelection in dsSelection.Tables[0].Rows)
                {
                    int intSet = Int32.Parse(drSelection["setid"].ToString());
                    DataSet dsResponse = GetResponse(intSet);
                    // Check to make sure the value is not already set (if multiple selections are causing multiple changes)
                    bool boolAlreadySet = false;
                    string strField = "";
                    string strValue = "";
                    foreach (DataRow drResponse in dsResponse.Tables[0].Rows)
                    {
                        if (boolVisibleSet == false || drResponse["visible"].ToString() == "1")
                        {
                            intQuestion = Int32.Parse(drResponse["questionid"].ToString());
                            strField = drResponse["related_field"].ToString();
                            strValue = drResponse["related_value"].ToString();
                            int intTemp = 0;
                            if (Int32.TryParse(drResponse["set_classid"].ToString(), out intTemp) && intTemp > 0)
                                strValue = intTemp.ToString();
                            else if (Int32.TryParse(drResponse["set_osid"].ToString(), out intTemp) && intTemp > 0)
                                strValue = intTemp.ToString();
                            else if (Int32.TryParse(drResponse["set_environmentid"].ToString(), out intTemp) && intTemp > 0)
                                strValue = intTemp.ToString();

                            if (strField != "" && Get(_id, strField) == strValue)
                            {
                                boolAlreadySet = true;
                                break;
                            }
                        }
                    }
                    if (boolAlreadySet == false)
                    {
                        // Not set, so change it.
                        ClearResponses(_id, intQuestion);
                        DataSet dsQuestion = GetQuestion(intQuestion);
                        if (dsQuestion.Tables[0].Rows.Count > 0)
                        {
                            DataRow drQuestion = dsQuestion.Tables[0].Rows[0];
                            int intPhase = Int32.Parse(drQuestion["phaseid"].ToString());
                            strField = (drQuestion["related_field"].ToString() == "" ? strField : drQuestion["related_field"].ToString());
                            if (strField != "" && Get(_id, strField) != strValue)
                            {
                                if (strValue != "")
                                {
                                    AddValue(_id, _forecastid, intPhase, intQuestion, strField, intSet.ToString() + "_" + strValue);
                                    if (drQuestion["related_field"].ToString() != "")
                                        break;
                                }
                            }
                        }
                    }
                }
                if (boolTooLong == false)
                {
                    // If a response of this question is not selected and has an auto-selection, un-select it and find new field. (ex: cluster -> enterprise)
                    if (GetQuestion(_questionid, "is_type_drop_down") == "1" || GetQuestion(_questionid, "is_type_radio") == "1")   // (ex: cluster is radio)
                    {
                        // Get each response under the question
                        bool boolUnder48 = IsUnder48(_id);
                        DataSet dsResponse = GetResponses(_questionid, (boolUnder48 ? 1 : 0), (boolUnder48 ? 0 : 1), 1, 1);
                        foreach (DataRow drResponse in dsResponse.Tables[0].Rows)
                        {
                            int intResponseTemp = Int32.Parse(drResponse["id"].ToString());
                            if (intResponse > 0 && intResponseTemp != intResponse && IsShowResponse(intResponseTemp, _id) == true)  // Yes does not equal No (selected response)
                            {
                                // Get auto-selections
                                DataSet dsSelectionClear = GetSelections(intResponseTemp);
                                foreach (DataRow drSelectionClear in dsSelectionClear.Tables[0].Rows)
                                {
                                    int intClear = Int32.Parse(drSelectionClear["setid"].ToString());  // Get enterprise
                                    int intClearQuestion = 0;
                                    if (Int32.TryParse(GetResponse(intClear, "questionid"), out intClearQuestion) == true)  // Get OS
                                    {
                                        DataSet dsClear = GetResponse(intClear);
                                        foreach (DataRow drClear in dsClear.Tables[0].Rows)
                                        {
                                            // See if invisible (meaning it is an odd selection)
                                            if (drClear["visible"].ToString() == "0")    // Enterprise is hidden
                                            {
                                                string strField = GetQuestion(intClearQuestion, "related_field");
                                                if (strField == "")
                                                    strField = drClear["related_field"].ToString();  // OSID
                                                bool boolFieldUpdatedWithVisible = false;

                                                // Get all responses for the field.
                                                DataSet dsRelated = GetRelatedFields(strField);
                                                foreach (DataRow drRelated in dsRelated.Tables[0].Rows)
                                                {
                                                    int intQuestionCheck = Int32.Parse(drRelated["id"].ToString());
                                                    if (drRelated["response"].ToString() == "1")
                                                        Int32.TryParse(GetResponse(intQuestionCheck, "questionid"), out intQuestionCheck);

                                                    // See if any other responses are related to this question
                                                    DataSet dsOtherSelection = GetSelection(intQuestionCheck);
                                                    foreach (DataRow drOtherSelection in dsOtherSelection.Tables[0].Rows)
                                                    {
                                                        int intOtherResponse = Int32.Parse(drOtherSelection["responseid"].ToString());
                                                        int intOtherQuestion = 0;
                                                        if (Int32.TryParse(GetResponse(intOtherResponse, "questionid"), out intOtherQuestion) == true)
                                                        {
                                                            if (IsSelected(_id, intOtherQuestion, intOtherResponse) == true)
                                                            {
                                                                int intOtherSet = Int32.Parse(drOtherSelection["setid"].ToString());
                                                                DataSet dsOtherSet = GetResponse(intOtherSet);
                                                                if (dsOtherSet.Tables[0].Rows.Count == 1)
                                                                {
                                                                    DataRow drOtherSet = dsOtherSet.Tables[0].Rows[0];
                                                                    if (Int32.TryParse(drOtherSet["questionid"].ToString(), out intOtherQuestion) == true)
                                                                    {
                                                                        string strSet = drOtherSet["related_value"].ToString();
                                                                        int intTemp = 0;
                                                                        if (Int32.TryParse(drOtherSet["set_classid"].ToString(), out intTemp) && intTemp > 0)
                                                                            strSet = intTemp.ToString();
                                                                        else if (Int32.TryParse(drOtherSet["set_osid"].ToString(), out intTemp) && intTemp > 0)
                                                                            strSet = intTemp.ToString();
                                                                        else if (Int32.TryParse(drOtherSet["set_environmentid"].ToString(), out intTemp) && intTemp > 0)
                                                                            strSet = intTemp.ToString();

                                                                        if (drOtherSet["visible"].ToString() == "1")
                                                                        {
                                                                            boolFieldUpdatedWithVisible = true;
                                                                            // Set the value
                                                                            AddValue(_id, _forecastid, _phaseid, intOtherQuestion, strField, intOtherSet.ToString() + "_" + strSet);
                                                                        }
                                                                        else
                                                                        {
                                                                            string strValue = Get(_id, strField);
                                                                            if (strSet != strValue && boolFieldUpdatedWithVisible == false)
                                                                                AddValue(_id, _forecastid, _phaseid, intOtherQuestion, strField, intOtherSet.ToString() + "_" + strSet);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return _id;
        }
        public string GetDataType(string _field)
        {
            string strType = "";
            Functions oFunction = new Functions(0, dsn, 0);
            DataSet dsFields = oFunction.GetSystemTableColumns(strTable);
            foreach (DataRow drField in dsFields.Tables[0].Rows)
            {
                if (drField["name"].ToString().ToUpper() == _field.ToUpper())
                {
                    strType = drField["type"].ToString().ToUpper();
                    break;
                }
            }
            return strType;
        }
        public DataSet GetsSearch(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignsSearch", arParams);
        }
        public DataSet Gets()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignsAll");
        }
        public DataSet Gets(int _forecastid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@forecastid", _forecastid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesigns", arParams);
        }
        public DataSet GetsUser(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignsUser", arParams);
        }
        public DataSet GetRelatedFields(string _related_field)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@related_field", _related_field);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignRelatedFields", arParams);
        }
        public bool IsRelatedFields(string _related_field)
        {
            return (GetRelatedFields(_related_field).Tables[0].Rows.Count > 0);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesign", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetAnswer(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignAnswer", arParams);
        }
        public string GetAnswer(int _answerid, string _column)
        {
            DataSet ds = GetAnswer(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void Update(int _id, int? _classid, int? _osid, int? _spid, int? _domainid, string _backup_frequency, int? _environmentid, int? _ha, int? _ha_clustering, int? _ha_load_balancing, int? _active_passive, int? _instances, int? _quorum, int? _middleware, int? _application, int? _quantity, int? _addressid, int? _modelid, int? _applicationid, int? _subapplicationid)
        {
            arParams = new SqlParameter[20];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@osid", _osid);
            arParams[3] = new SqlParameter("@spid", _spid);
            arParams[4] = new SqlParameter("@domainid", _domainid);
            arParams[5] = new SqlParameter("@backup_frequency", _backup_frequency);
            arParams[6] = new SqlParameter("@environmentid", _environmentid);
            arParams[7] = new SqlParameter("@ha", _ha);
            arParams[8] = new SqlParameter("@ha_clustering", _ha_clustering);
            arParams[9] = new SqlParameter("@ha_load_balancing", _ha_load_balancing);
            arParams[10] = new SqlParameter("@active_passive", _active_passive);
            arParams[11] = new SqlParameter("@instances", _instances);
            arParams[12] = new SqlParameter("@quorum", _quorum);
            arParams[13] = new SqlParameter("@middleware", _middleware);
            arParams[14] = new SqlParameter("@application", _application);
            arParams[15] = new SqlParameter("@quantity", _quantity);
            arParams[16] = new SqlParameter("@addressid", _addressid);
            arParams[17] = new SqlParameter("@modelid", _modelid);
            arParams[18] = new SqlParameter("@applicationid", _applicationid);
            arParams[19] = new SqlParameter("@subapplicationid", _subapplicationid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesign", arParams);
        }
        public int Next(int _id, bool _forwards)
        {
            int intReturn = 0;
            int intPhaseID = 0;
            if (Int32.TryParse(Get(_id, "phaseid"), out intPhaseID) == true)
            {
                // Now, get the DISPLAY of the phase
                int intDisplay = 0;
                if (Int32.TryParse(GetPhase(intPhaseID, "display"), out intDisplay) == true)
                {
                    DataSet dsPhases = GetPhases(1);
                    if (_forwards == true)
                    {
                        intDisplay++;
                        int intMax = GetPhaseMax();    // Maximum DISPLAY field
                        if (intDisplay > intMax)
                        {
                            // Move to summary screen
                            intReturn = 999;
                        }
                        else
                        {
                            while (intDisplay <= intMax)
                            {
                                DataSet dsOrder = GetPhaseOrder(intDisplay);
                                if (dsOrder.Tables[0].Rows.Count > 0)
                                {
                                    // Get the PhaseID of the next record (based on DISPLAYID)
                                    int intPhase = Int32.Parse(dsOrder.Tables[0].Rows[0]["id"].ToString());
                                    if (IsLocked(_id, intPhase) == false)
                                    {
                                        intReturn = intPhase;
                                        break;
                                    }
                                }
                                // This incremental phase is locked or does not exist.  Move to the next one.
                                intDisplay++;
                            }
                        }
                    }
                    else
                    {
                        intDisplay--;
                        while (intDisplay >= 1)
                        {
                            DataSet dsOrder = GetPhaseOrder(intDisplay);
                            if (dsOrder.Tables[0].Rows.Count > 0)
                            {
                                // Get the PhaseID of the previous record (based on DISPLAYID)
                                int intPhase = Int32.Parse(dsOrder.Tables[0].Rows[0]["id"].ToString());
                                if (IsLocked(_id, intPhase) == false)
                                {
                                    intReturn = intPhase;
                                    break;
                                }
                            }
                            // This phase is locked or does not exist.  Move to the previous one.
                            intDisplay--;
                        }
                    }
                }
            }
            if (intReturn > 0)
                UpdatePhaseId(_id, intReturn);
            return intReturn;
        }
        public int GetPhaseID(int _id)
        {
            if (_id == 0)
                return 1;
            else
            {
                int intPhaseID = 0;
                if (Int32.TryParse(Get(_id, "phaseid"), out intPhaseID) == true)
                    return intPhaseID;
                else
                    return 0;
            }
        }
        public void UpdatePhaseId(int _id, int _phaseid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@phaseid", _phaseid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignPhaseid", arParams);
        }
        public void UpdateMISNotified(int _id, string _mis_notified)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@mis_notified", (_mis_notified == "" ? SqlDateTime.Null : DateTime.Parse(_mis_notified)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignMISNotified", arParams);
        }
        public void UpdateMISApproved(int _id, string _mis_approved)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@mis_approved", (_mis_approved == "" ? SqlDateTime.Null : DateTime.Parse(_mis_approved)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignMISApproved", arParams);
        }
        public void UpdateMISRejected(int _id, string _mis_rejected)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@mis_rejected", (_mis_rejected == "" ? SqlDateTime.Null : DateTime.Parse(_mis_rejected)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignMISRejected", arParams);
        }
        public void UpdateAnswerId(int _id, int _answerid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@answerid", _answerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignAnswerid", arParams);
        }
        public void UpdateException(int _id, bool _is_exception)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@is_exception", (_is_exception ? 1 : 0));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignException", arParams);
        }
        public void UpdateAccountEmail(int _id, int _accounts_email)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@accounts_email", _accounts_email);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignAccountEmail", arParams);
        }

        public List<int> GetResponseSelected(int _id, int _questionid)
        {
            //int intCounter = 0;
            List<int> arrSelected = new List<int>();
            //arrSelected.Add(32);
            //int[] intSelected = new int[10];
            DataSet dsQuestion = GetQuestion(_questionid);
            if (dsQuestion.Tables[0].Rows.Count > 0)
            {
                DataRow drQuestion = dsQuestion.Tables[0].Rows[0];
                bool boolDropDown = (drQuestion["is_type_drop_down"].ToString() == "1");
                bool boolCheckBox = (drQuestion["is_type_check_box"].ToString() == "1");
                bool boolRadio = (drQuestion["is_type_radio"].ToString() == "1");
                bool boolTextBox = (drQuestion["is_type_textbox"].ToString() == "1");
                bool boolTextArea = (drQuestion["is_type_textarea"].ToString() == "1");

                string strSpecial = GetQuestionSpecial(_questionid);
                if (strSpecial == "")
                {
                    DataSet dsResponse = GetResponses(_questionid, 0, 1);
                    foreach (DataRow drResponse in dsResponse.Tables[0].Rows)
                    {
                        // Go through all responses
                        int intResponse = Int32.Parse(drResponse["id"].ToString());
                        if (IsShowResponse(intResponse, _id) == true)
                        {
                            int intClass = 0;
                            int intOS = 0;
                            int intEnv = 0;
                            int intComponentID = 0;
                            // Get related field
                            string strField = (drQuestion["related_field"].ToString() == "" ? drResponse["related_field"].ToString() : drQuestion["related_field"].ToString());
                            // Get configured value
                            string strSet = drResponse["related_value"].ToString();
                            if (strField != "")
                            {
                                // Get value
                                string strValue = Get(_id, strField);

                                if (Int32.TryParse(drResponse["set_classid"].ToString(), out intClass) && intClass > 0)
                                {
                                    if (strField != "")
                                    {
                                        int intValue = 0;
                                        if (Int32.TryParse(strValue, out intValue) == true)
                                        {
                                            // Compare the selected value (intValue) with this value
                                            if (intValue == intClass)
                                            {
                                                // This response has been selected...
                                                arrSelected.Add(intResponse);
                                                //intSelected[intCounter] = intResponse;
                                                //intCounter++;
                                                //break;
                                            }
                                        }
                                    }
                                }
                                else if (Int32.TryParse(drResponse["set_osid"].ToString(), out intOS) && intOS > 0)
                                {
                                    if (strField != "")
                                    {
                                        int intValue = 0;
                                        if (Int32.TryParse(strValue, out intValue) == true)
                                        {
                                            // Compare the selected value (intValue) with this value
                                            if (intValue == intOS)
                                            {
                                                // This response has been selected...
                                                arrSelected.Add(intResponse);
                                                //intSelected[intCounter] = intResponse;
                                                //intCounter++;
                                                //break;
                                            }
                                        }
                                    }
                                }
                                else if (Int32.TryParse(drResponse["set_environmentid"].ToString(), out intEnv) && intEnv > 0)
                                {
                                    if (strField != "")
                                    {
                                        int intValue = 0;
                                        if (Int32.TryParse(strValue, out intValue) == true)
                                        {
                                            // Compare the selected value (intValue) with this value
                                            if (intValue == intEnv)
                                            {
                                                // This response has been selected...
                                                arrSelected.Add(intResponse);
                                                //intSelected[intCounter] = intResponse;
                                                //intCounter++;
                                                //break;
                                            }
                                        }
                                    }
                                }
                                else if (Int32.TryParse(drResponse["set_componentid"].ToString(), out intEnv) && intEnv > 0)
                                {
                                    if (strField != "")
                                    {
                                        int intValue = 0;
                                        if (Int32.TryParse(strValue, out intValue) == true)
                                        {
                                            // Compare the selected value (intValue) with this value
                                            if (intValue == intEnv)
                                            {
                                                // This response has been selected...
                                                arrSelected.Add(intResponse);
                                                //intSelected[intCounter] = intResponse;
                                                //intCounter++;
                                                //break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (boolTextBox == false && boolTextArea == false && strValue == strSet)
                                    {
                                        // This response has been selected...
                                        arrSelected.Add(intResponse);
                                        //intSelected[intCounter] = intResponse;
                                        //intCounter++;
                                        //break;
                                    }
                                }
                            }
                            else if (Int32.TryParse(drResponse["set_componentid"].ToString(), out intComponentID) && intComponentID > 0)
                            {
                                if (IsSoftwareComponent(_id, intComponentID) == true)
                                {
                                    // This response has been selected...
                                    arrSelected.Add(intResponse);
                                    //intSelected[intCounter] = intResponse;
                                    //intCounter++;
                                    //break;
                                }
                            }
                        }
                    }
                }
            }
            return arrSelected;
        }
        public bool IsSelected(int _id, int _questionid, int _responseid)
        {
            bool boolSelected = false;
            bool boolHidden = false;
            //for (int ii = 0; ii < arrHiddenQuestions.Count; ii++)
            //{
            //    if (arrHiddenQuestions[ii] == _questionid)
            //    {
            //        boolHidden = true;
            //        break;
            //    }
            //}
            if (boolHidden == false)    // If the question is hidden, the response cannot be selected.
            {
                //int[] intSelected = GetResponseSelected(_id, _questionid);
                List<int> arrSelected = GetResponseSelected(_id, _questionid);
                for (int ii = 0; ii < arrSelected.Count; ii++)
                {
                    if (arrSelected[ii] == _responseid)
                    {
                        boolSelected = true;
                        break;
                    }
                }
                if (boolSelected == false)
                {
                    bool boolUnder48 = IsUnder48(_id);
                    // Check if the response is the only one and SELECT IF ONE is enabled.
                    DataSet dsResponse = GetResponses(_questionid, (boolUnder48 ? 1 : 0), (boolUnder48 ? 0 : 1), 1, 1);
                    int intResponseOne = 0;
                    int intResponseCount = dsResponse.Tables[0].Rows.Count;
                    foreach (DataRow drResponse in dsResponse.Tables[0].Rows)
                    {
                        int intResponseTemp = Int32.Parse(drResponse["id"].ToString());
                        if (IsShowResponse(intResponseTemp, _id) == false)
                            intResponseCount--;
                        else
                        {
                            if (intResponseOne != 0)    // No longer the default, so must be more than one valid response.
                                break;
                            intResponseOne = intResponseTemp;
                        }
                    }
                    if (intResponseCount == 1 && _responseid == intResponseOne && GetResponse(intResponseOne, "select_if_one") == "1")
                        boolSelected = true;
                }
            }
            return boolSelected;
        }

        public bool IsLocked(int _id, int _phaseid)
        {
            bool boolLocked = false;
            DataSet dsRestrictions = GetRestriction(_phaseid, 1);
            if (dsRestrictions.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drRestriction in dsRestrictions.Tables[0].Rows)
                {
                    int intResponse = Int32.Parse(drRestriction["responseid"].ToString());
                    int intQuestion = 0;
                    if (Int32.TryParse(GetResponse(intResponse, "questionid"), out intQuestion) == true)
                    {
                        if (IsSelected(_id, intQuestion, intResponse) == true)
                        {
                            // The selected response matches the one causing this response to be auto-selected.
                            boolLocked = true;
                            break;
                        }
                    }
                }
            }
            if (boolLocked == false)
            {
                // Check to see if one or more of the selected responses are set to ENABLE
                dsRestrictions = GetRestriction(_phaseid, 0);
                if (dsRestrictions.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drRestriction in dsRestrictions.Tables[0].Rows)
                    {
                        int intResponse = Int32.Parse(drRestriction["responseid"].ToString());
                        int intQuestion = 0;
                        if (Int32.TryParse(GetResponse(intResponse, "questionid"), out intQuestion) == true)
                        {
                            if (IsSelected(_id, intQuestion, intResponse) == false)
                            {
                                // The response needed for this phase to be enabled is NOT selected...so make it locked.
                                boolLocked = true;
                                break;
                            }
                        }
                    }
                }
            }
            return boolLocked;
        }

        public bool IsSelected(int _id, int _questionid)
        {
            bool boolSelected = false;
            DataSet dsSelected = GetSelection(_questionid);
            if (dsSelected.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drSelected in dsSelected.Tables[0].Rows)
                {
                    int intResponse = Int32.Parse(drSelected["responseid"].ToString());
                    int intQuestion = 0;
                    if (Int32.TryParse(GetResponse(intResponse, "questionid"), out intQuestion) == true)
                    {
                        if (IsSelected(_id, intQuestion, intResponse) == true)
                        {
                            // The selected response matches the one causing this response to be auto-selected.
                            boolSelected = true;
                            break;
                        }
                    }
                }
            }
            return boolSelected;
        }
        public bool IsHidden(int _id, int _questionid, HttpRequest _request)
        {
            bool boolHidden = false;
            DataSet dsShow = GetShow(_questionid);
            if (dsShow.Tables[0].Rows.Count > 0)
            {
                //for (int ii = 0; ii < arrHiddenQuestions.Count; ii++)
                //{
                //    if (arrHiddenQuestions[ii] == _questionid)
                //    {
                //        boolHidden = true;
                //        break;
                //    }
                //}
                if (boolHidden == false)
                {
                    foreach (DataRow drShow in dsShow.Tables[0].Rows)
                    {
                        // ALL responses must be selected for it to be shown.
                        int intResponse = Int32.Parse(drShow["responseid"].ToString());
                        int intQuestion = 0;
                        if (Int32.TryParse(GetResponse(intResponse, "questionid"), out intQuestion) == true)
                        {
                            if (IsSelected(_id, intQuestion, intResponse) == false)
                            {
                                // The selected response does not match.  Check Postback...
                                bool boolPostback = false;
                                if (_request != null)
                                {
                                    string strSet = GetResponse(intResponse, "related_value");
                                    foreach (string strForm in _request.Form)
                                    {
                                        if (strForm.StartsWith("HDN_") == true)
                                        {
                                            string strPostbackField = strForm.Substring(4);
                                            // strPostbackField : "persistent"
                                            string strPostbackValue = _request.Form[strForm];
                                            // strPostbackValue : "29_1"
                                            // Get ResponseID
                                            if (strPostbackValue.Contains("_"))
                                            {
                                                string strReponseID = strPostbackValue.Substring(0, strPostbackValue.IndexOf("_"));
                                                string strSelected = strPostbackValue.Substring(strPostbackValue.IndexOf("_") + 1);
                                                if (strReponseID == intResponse.ToString() && strSelected == strSet)
                                                {
                                                    boolPostback = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (boolPostback == false)
                                {
                                    //causing this response to be hidden.
                                    boolHidden = true;
                                    break;
                                }
                            }
                            else
                            {
                                // Its been selected, but is it hidden?
                                if (IsHidden(_id, intQuestion, _request) == true)
                                    boolHidden = true;
                                break;
                            }
                        }
                    }
                    if (boolHidden == true)
                        arrHiddenQuestions.Add(_questionid);
                }
            }
            return boolHidden;
        }

        public void ClearResponses(int _id, int _questionid)
        {
            // Clear other responses
            DataSet dsResponses = GetResponses(_questionid, 0, 1);
            foreach (DataRow drResponses in dsResponses.Tables[0].Rows)
            {
                string strTempField = GetQuestion(_questionid, "related_field");
                bool boolFieldQuestion = true;
                if (strTempField == "")
                {
                    strTempField = drResponses["related_field"].ToString();
                    boolFieldQuestion = false;
                }
                // Make sure that this question's field is only updated by this question
                // (otherwise, we could be overwriting a non-hidden value)
                DataSet dsClear = GetRelatedFields(strTempField);
                if (dsClear.Tables[0].Rows.Count == 1)
                {
                    string strTempType = GetDataType(strTempField);
                    string strTempSQL = "";
                    if (strTempType == "DATETIME")
                        strTempSQL = "null";
                    else if (strTempType == "INT")
                        strTempSQL = "0";
                    else if (strTempType == "VARCHAR")
                        strTempSQL = "''";
                    strTempSQL = "UPDATE " + strTable + " SET [" + strTempField + "] = " + strTempSQL + " WHERE id = " + _id.ToString();
                    SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, strTempSQL);
                    if (boolFieldQuestion == true)
                        break;
                }
            }
        }

        public bool IsUnder48(int _designid)
        {
            bool boolUnder48 = false;
            int intResRating = GetResRating(_designid);
            boolUnder48 = (intResRating < 48);
            return boolUnder48;
        }
        public bool IsDatabase(int _designid)
        {
            return (Get(_designid, "database") == "1");
        }
        public bool IsSQL(int _designid)
        {
            return (IsComponent(_designid, GetResponsesComponent(0, 0, 1, 0)));
        }
        public bool IsOracle(int _designid)
        {
            return (IsComponent(_designid, GetResponsesComponent(0, 0, 0, 1)));
        }
        private bool IsComponent(int _designid, DataSet dsComponent)
        {
            bool boolSelected = false;
            foreach (DataRow drComponent in dsComponent.Tables[0].Rows)
            {
                if (IsSoftwareComponent(_designid, Int32.Parse(drComponent["componentid"].ToString())) == true)
                {
                    boolSelected = true;
                    break;
                }
            }
            return boolSelected;
        }
        public bool IsCluster(int _designid)
        {
            return (Get(_designid, "ha_clustering") == "1");
        }
        public bool IsStorage(int _designid)
        {
            return (Get(_designid, "storage") == "1");
        }
        public bool IsStoragePersistent(int _designid)
        {
            return (IsStorage(_designid) && (IsRelatedFields("persistent") == false || Get(_designid, "persistent") == "1"));
        }
        public bool IsOther(int _id, int _is_confidence_lock, int _is_confidence_unlock, int _is_exception)
        {
            bool boolOther = false;
            DataSet dsOther = GetResponsesOther(_is_confidence_lock, _is_confidence_unlock, _is_exception, 1);
            if (dsOther.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drOther in dsOther.Tables[0].Rows)
                {
                    int intResponse = Int32.Parse(drOther["id"].ToString());
                    int intQuestion = 0;
                    if (Int32.TryParse(GetResponse(intResponse, "questionid"), out intQuestion) == true)
                    {
                        if (IsSelected(_id, intQuestion, intResponse) == true)
                        {
                            // The selected response matches the one causing this response to be auto-selected.
                            boolOther = true;
                            break;
                        }
                    }
                }
            }
            return boolOther;
        }
        public bool IsWindows(int _designid)
        {
            OperatingSystems oOperatingSystem = new OperatingSystems(user, dsn);
            int intTemp = 0;
            if (Int32.TryParse(Get(_designid, "osid"), out intTemp) == true)
                return oOperatingSystem.IsWindows(intTemp) || oOperatingSystem.IsWindows2008(intTemp);
            else
                return false;
        }
        public int GetResRating(int _id)
        {
            int intHours = 0;
            int intMnemonic = 0;
            Int32.TryParse(Get(_id, "mnemonicid"), out intMnemonic);
            if (intMnemonic > 0)
            {
                Mnemonic oMnemonic = new Mnemonic(user, dsn);
                intHours = oMnemonic.GetResRatingHRs(intMnemonic);
            }
            return intHours;
        }

        public void Unlock(int _designid)
        {
            // Set confidence back
            DataSet dsOther = GetResponsesOther(0, 1, 0, 1);
            foreach (DataRow drOther in dsOther.Tables[0].Rows)
            {
                int intResponse = Int32.Parse(drOther["id"].ToString());
                int intQuestion = 0;
                if (Int32.TryParse(GetResponse(intResponse, "questionid"), out intQuestion) == true)
                {
                    string strValue = drOther["related_value"].ToString();
                    string strField = drOther["related_field"].ToString();
                    string strFieldQuestion = GetQuestion(intQuestion, "related_field");
                    if (strField != "")
                    {
                        if (Get(_designid, strField) != strValue)
                            AddValue(_designid, 0, 0, intQuestion, strField, strValue);
                    }
                    else if (strFieldQuestion != "")
                    {
                        if (Get(_designid, strFieldQuestion) != strValue)
                            AddValue(_designid, 0, 0, intQuestion, strFieldQuestion, strValue);
                    }
                }
            }
            // Notify outstanding approvals, and remove them
        }

        public bool IsValid(int _designid)
        {
            return (GetValid(_designid) == "");
        }
        public string GetValid(int _designid)
        {
            Classes oClass = new Classes(user, dsn);
            string strValid = "Invalid Design (" + _designid.ToString() + ")";
            DataSet dsSummary = Get(_designid);
            if (dsSummary.Tables[0].Rows.Count > 0)
            {
                strValid = "";
                DataRow drSummary = dsSummary.Tables[0].Rows[0];
                bool boolWeb = (drSummary["web"].ToString() == "1");
                bool boolSQL = IsSQL(_designid);
                bool boolOracle = IsOracle(_designid);
                bool boolOtherDB = (drSummary["other_db"].ToString() == "1");

                // Mnemonic
                int intMnemonic = 0;
                Int32.TryParse(drSummary["mnemonicid"].ToString(), out intMnemonic);
                if (IsRelatedFields("mnemonicid") && intMnemonic == 0)
                    strValid += "Mnemonic Required,";

                // Cost
                int intCost = 0;
                Int32.TryParse(drSummary["costid"].ToString(), out intCost);
                if (IsRelatedFields("costid") && intCost == 0)
                    strValid += "Cost Center Required,";

                // Class + Environment + Location
                bool boolDev = false;
                bool boolTest = false;
                bool boolQA = false;
                bool boolProd = false;
                bool boolDR = false;
                int intClass = 0;
                int intEnv = 0;
                Int32.TryParse(drSummary["classid"].ToString(), out intClass);
                if (IsRelatedFields("classid") && intClass == 0)
                    strValid += "Environment Required,";
                else
                {
                    boolTest = (oClass.IsTest(intClass));
                    boolQA = (oClass.IsQA(intClass));
                    boolProd = (oClass.IsProd(intClass));
                    boolDR = (oClass.IsDR(intClass));
                    boolDev = (boolTest == false && boolQA == false && boolProd == false && boolDR == false);
                    Int32.TryParse(drSummary["environmentid"].ToString(), out intEnv);
                    if (IsRelatedFields("environmentid") && intEnv == 0)
                        strValid += "Server Location Required,";

                    // Server Type
                    if (IsDatabase(_designid) == true)
                    {
                        if (boolSQL == false && boolOracle == false && boolOtherDB == false)
                            strValid += "Database Option Required,";
                    }

                    // Quantity
                    int intQuantity = 0;
                    Int32.TryParse(drSummary["quantity"].ToString(), out intQuantity);
                    if (IsRelatedFields("quantity") && intQuantity == 0)
                        strValid += "Quantity Required,";

                    // OS
                    int intOS = 0;
                    Int32.TryParse(drSummary["osid"].ToString(), out intOS);
                    if (IsRelatedFields("osid") && intOS == 0)
                        strValid += "Operating System Required,";

                    // SIZE
                    if (IsRelatedFields("size") && drSummary["size"].ToString() == "")
                        strValid += "Server Size Required,";

                    // STORAGE
                    bool boolAppDrive = false;
                    if (IsRelatedFields("storage"))
                    {
                        if (drSummary["storage"].ToString() == "1")
                        {
                            if (IsRelatedFields("persistent"))
                            {
                                if (drSummary["persistent"].ToString() == "1")
                                {
                                    int intPersistent = GetStorageTotal(_designid);
                                    if (intPersistent == 0)
                                        strValid += "Storage Allocation Required,";
                                    else
                                        boolAppDrive = IsWindows(_designid);
                                }
                                else if (drSummary["persistent"].ToString() == "0")
                                {
                                    int intNonPersistent = 0;
                                    Int32.TryParse(drSummary["non_persistent"].ToString(), out intNonPersistent);
                                    if (IsRelatedFields("non_persistent") && intNonPersistent == 0)
                                        strValid += "Storage Amount Required,";
                                }
                                else
                                    strValid += "Storage Type Required,";
                            }
                            else
                            {
                                int intPersistent = GetStorageTotal(_designid);
                                if (intPersistent == 0)
                                    strValid += "Storage Allocation Required,";
                                else
                                    boolAppDrive = IsWindows(_designid);
                            }
                        }
                        else if (drSummary["storage"].ToString() != "0")
                            strValid += "Storage Selection Required,";
                    }
                    if (boolAppDrive)
                    {
                        double dblAppDrive = 0.00;
                        DataSet dsApp = GetStorage(_designid, -1000);
                        if (dsApp.Tables[0].Rows.Count > 0)
                            double.TryParse(dsApp.Tables[0].Rows[0]["size"].ToString(), out dblAppDrive);
                        if (dblAppDrive == 0.00)
                            strValid += "Application Drive Storage Amount Required,";
                    }

                    int intInstances = -1;
                    if (boolQA || boolProd)
                    {
                        // HA
                        if (drSummary["ha"].ToString() == "1")
                        {
                            if (drSummary["ha_clustering"].ToString() == "1")
                            {
                                if (drSummary["active_passive"].ToString() == "")
                                    strValid += "Cluster Type Required,";
                                Int32.TryParse(drSummary["instances"].ToString(), out intInstances);
                                if (intInstances == 0)
                                    strValid += "Number of Cluster Instances Required,";
                                int intQuorum = 0;
                                Int32.TryParse(drSummary["quorum"].ToString(), out intQuorum);
                                if (intQuorum == 0)
                                    strValid += "Quorum Drive Size Required,";
                            }
                            else if (drSummary["ha_load_balancing"].ToString() == "1")
                            {
                                if (drSummary["middleware"].ToString() == "" || (boolWeb == true && drSummary["application"].ToString() == ""))
                                    strValid += "Load Balancing Information Required,";
                            }
                            else
                                strValid += "High Availability Type Required,";
                        }
                        else if (IsRelatedFields("ha") && drSummary["ha"].ToString() != "0")
                            strValid += "High Availability Selection Required,";
                        // BACKUP
                        if (IsRelatedFields("backup_frequency"))
                        {
                            string strFrequency = drSummary["backup_frequency"].ToString();
                            if (strFrequency == "")
                                strValid += "Backup Frequency Required,";
                            else if (intMnemonic > 0)
                            {
                                // Check that the backup is correct.
                                bool boolIsUnder48 = IsUnder48(_designid);
                                if (boolIsUnder48 && (strFrequency == "W" || strFrequency == "M"))
                                    strValid += "Invalid Backup Frequency Specified,";
                            }
                        }
                        if (GetBackup(_designid).Tables[0].Rows.Count == 0)
                            strValid += "Backup Window Required,";
                        // MAINTENANCE
                        if (GetMaintenance(_designid).Tables[0].Rows.Count == 0)
                            strValid += "Maintenance Window Required,";
                    }

                    if (boolQA || boolProd)
                    {
                        // MAINFRAME
                        if (IsRelatedFields("mainframe") && drSummary["mainframe"].ToString() != "1" && drSummary["mainframe"].ToString() != "0")
                            strValid += "Mainframe Selection Required,";
                    }

                    // DATE
                    DateTime datDate = DateTime.Now;
                    if (IsRelatedFields("commitment") && DateTime.TryParse(drSummary["commitment"].ToString(), out datDate) == false)
                        strValid += "Build Date Required,";

                    // CONFIDENCE
                    if (IsRelatedFields("confidence") && drSummary["confidence"].ToString() == "")
                        strValid += "Confidence Level Required,";
                }
            }
            return strValid;
        }

        public string CanExecute(int _designid)
        {
            string strReturn = GetValid(_designid);
            if (strReturn == "")
            {
                // Design is valid (all required questions have been answered)
                if (Get(_designid, "phaseid") == "999")
                {
                    // All phases have been completed
                    if (IsOther(_designid, 1, 0, 0) == true)
                    {
                        // Confident
                        Holidays oHoliday = new Holidays(user, dsn);
                        DateTime datDate = DateTime.Now;
                        //if (DateTime.TryParse(Get(_designid, "commitment"), out datDate) == true && datDate <= DateTime.Today)
                        if (DateTime.TryParse(Get(_designid, "commitment"), out datDate) == true && datDate >= oHoliday.GetDays(10.00, DateTime.Today))
                        {
                            // Commitment Date has passed
                            Mnemonic oMnemonic = new Mnemonic(user, dsn);
                            int intMnemonic = 0;
                            if (Int32.TryParse(Get(_designid, "mnemonicid"), out intMnemonic) == true && oMnemonic.Get(intMnemonic, "status").Trim().ToUpper() == "APPROVED")
                            {
                                // Mnemonic is approved
                                int intModel = GetModelProperty(_designid);
                                if (intModel > 0)
                                {
                                }
                                else
                                    strReturn = "Solution Unavailable";
                            }
                            else
                                strReturn = "Mnemonic Pending Approval";
                        }
                        else
                            //strReturn = "Waiting for Build Date";
                            strReturn = "Build Date Expired";
                    }
                    else
                        strReturn = "Not Confident Enough";
                }
                else
                    strReturn = "Incomplete";
            }
            return strReturn;
        }

        public int GetModelProperty(int _designid)
        {
            int intModel = 0;
            if (Int32.TryParse(Get(_designid, "modelid"), out intModel) == true)
            {
                // Got the modelid from designs table
                //Int32.TryParse(GetModel(intModel, "modelid"), out intModel);
            }
            return intModel;
        }
        public int GetModelPriority(int _modelid1, int _modelid2)
        {
            int intReturn = _modelid1;
            if (_modelid1 == 0)
                intReturn = _modelid2;
            else if (_modelid2 == 0)
                intReturn = _modelid1;
            else
            {
                int intModel1 = 0;
                int intModel2 = 0;
                if (Int32.TryParse(GetModel(_modelid1, "display"), out intModel1) == true)
                {
                    if (Int32.TryParse(GetModel(_modelid2, "display"), out intModel2) == true)
                    {
                        if (intModel1 > intModel2)  // 1 = 9, 2 = 7...2 is less than 7...7 is lower on list and acceptable.
                            intReturn = _modelid2;
                    }
                }
            }
            return intReturn;
        }
        public bool IsProd(int _designid)
        {
            Classes oClass = new Classes(user, dsn);
            int intClass = 0;
            return (Int32.TryParse(Get(_designid, "classid"), out intClass) == true && oClass.IsProd(intClass));
        }
        public bool IsQA(int _designid)
        {
            Classes oClass = new Classes(user, dsn);
            int intClass = 0;
            return (Int32.TryParse(Get(_designid, "classid"), out intClass) == true && oClass.IsQA(intClass));
        }

        public string GetSummary(int _designid, int _environment)
        {
            StringBuilder strSummary = new StringBuilder();
            DataSet dsSummary = Get(_designid);
            if (dsSummary.Tables[0].Rows.Count > 0)
            {
                Mnemonic oMnemonic = new Mnemonic(user, dsn);
                CostCenter oCostCenter = new CostCenter(user, dsn);
                OperatingSystems oOperatingSystem = new OperatingSystems(user, dsn);
                Classes oClass = new Classes(user, dsn);
                Environments oEnvironment = new Environments(user, dsn);
                Locations oLocation = new Locations(user, dsn);
                ModelsProperties oModelsProperties = new ModelsProperties(user, dsn);
                Variables oVariable = new Variables(_environment);

                DataRow drSummary = dsSummary.Tables[0].Rows[0];
                bool boolWeb = (drSummary["web"].ToString() == "1");
                bool boolSQL = IsSQL(_designid);
                bool boolOracle = IsOracle(_designid);
                bool boolOtherDB = (drSummary["other_db"].ToString() == "1");

                strSummary.Append("<table width=\"100%\" cellpadding=\"5\" cellspacing=\"0\" border=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                strSummary.Append("<tr>");
                // Mnemonic
                strSummary.Append("<td nowrap class=\"bold\">Mnemonic:</td>");
                strSummary.Append("<td width=\"50%\">");
                int intMnemonic = 0;
                Int32.TryParse(drSummary["mnemonicid"].ToString(), out intMnemonic);
                if (intMnemonic > 0)
                    strSummary.Append(oMnemonic.Get(intMnemonic, "factory_code") + " - " + oMnemonic.Get(intMnemonic, "name"));
                strSummary.Append("</td>");
                // Cost
                strSummary.Append("<td nowrap class=\"bold\">Cost Center:</td>");
                strSummary.Append("<td width=\"50%\">");
                int intCost = 0;
                Int32.TryParse(drSummary["costid"].ToString(), out intCost);
                if (intCost > 0)
                    strSummary.Append(oCostCenter.GetName(intCost));
                strSummary.Append("</td>");
                strSummary.Append("</tr>");


                strSummary.Append("<tr>");
                // Server Type
                strSummary.Append("<td nowrap class=\"bold\">Server Type:</td>");
                strSummary.Append("<td width=\"50%\">");
                if (IsDatabase(_designid) == false)
                {
                    if (boolWeb)
                        strSummary.Append("Web");
                    else
                        strSummary.Append("Application");
                }
                else
                {
                    strSummary.Append("Database");
                    if (boolSQL || boolOracle || boolOtherDB)
                    {
                        string strDatabase = "";
                        if (boolSQL == true)
                            strDatabase = "SQL";
                        if (boolOracle == true)
                            strDatabase = "Oracle";
                        if (boolOtherDB == true)
                            strDatabase = "Other";
                        strSummary.Append(" (" + strDatabase + ")");
                    }
                    if (boolWeb)
                        strSummary.Append(" + Web");
                }
                strSummary.Append("</td>");
                // Quantity
                strSummary.Append("<td nowrap class=\"bold\">Quantity:</td>");
                strSummary.Append("<td width=\"50%\">");
                int intQuantity = 0;
                Int32.TryParse(drSummary["quantity"].ToString(), out intQuantity);
                if (intQuantity > 0)
                    strSummary.Append(intQuantity.ToString());
                strSummary.Append("</td>");
                strSummary.Append("</tr>");


                strSummary.Append("<tr>");
                // SIZE
                strSummary.Append("<td nowrap class=\"bold\">Server Size:</td>");
                strSummary.Append("<td width=\"50%\">");
                string strSize = (drSummary["size"].ToString() == "L" ? "Large" : (drSummary["size"].ToString() == "M" ? "Medium" : (drSummary["size"].ToString() == "S" ? "Small" : "")));
                strSummary.Append(strSize);
                if (strSize != "")
                    strSummary.Append(" <i>(see reference architecture)</i>");
                strSummary.Append("</td>");
                // STORAGE
                strSummary.Append("<td nowrap class=\"bold\">Storage:</td>");
                strSummary.Append("<td width=\"50%\">");
                if (drSummary["storage"].ToString() == "1")
                {
                    if (drSummary["persistent"].ToString() == "1")
                    {
                        int intPersistent = GetStorageTotal(_designid);
                        if (intPersistent > 0)
                            strSummary.Append("Persistent, " + intPersistent.ToString() + " GB(s)");
                    }
                    else if (drSummary["persistent"].ToString() == "0")
                    {
                        int intNonPersistent = 0;
                        Int32.TryParse(drSummary["non_persistent"].ToString(), out intNonPersistent);
                        if (intNonPersistent > 0)
                            strSummary.Append("Non-Persistent, " + intNonPersistent.ToString() + " GB(s)");
                    }
                }
                else if (drSummary["storage"].ToString() == "0")
                    strSummary.Append("No");
                strSummary.Append("</td>");
                strSummary.Append("</tr>");


                strSummary.Append("<tr>");
                // OS
                strSummary.Append("<td nowrap class=\"bold\">Operating System:</td>");
                strSummary.Append("<td width=\"50%\">");
                int intOS = 0;
                Int32.TryParse(drSummary["osid"].ToString(), out intOS);
                if (intOS > 0)
                    strSummary.Append(oOperatingSystem.Get(intOS, "name"));
                strSummary.Append("</td>");
                // HA
                strSummary.Append("<td nowrap class=\"bold\">High Availability:</td>");
                strSummary.Append("<td width=\"50%\">");
                if (drSummary["ha"].ToString() == "1")
                {
                    if (drSummary["ha_clustering"].ToString() == "1")
                    {
                        strSummary.Append("Clustered");
                        if (drSummary["active_passive"].ToString() == "1")
                            strSummary.Append(" (Active / Passive)");
                        else if (drSummary["active_passive"].ToString() == "0")
                            strSummary.Append(" (Active / Active)");
                    }
                    else if (drSummary["ha_load_balancing"].ToString() == "1")
                        strSummary.Append("Load Balancing");
                }
                else if (drSummary["ha"].ToString() == "0")
                    strSummary.Append("No");
                strSummary.Append("</td>");
                strSummary.Append("</tr>");


                strSummary.Append("<tr>");
                // SPECIAL
                strSummary.Append("<td nowrap class=\"bold\">Special Hardware:</td>");
                strSummary.Append("<td width=\"50%\">");
                if (drSummary["special"].ToString() == "")
                    strSummary.Append("None");
                else
                    strSummary.Append(drSummary["special"].ToString());
                strSummary.Append("</td>");
                // MAINFRAME
                strSummary.Append("<td nowrap class=\"bold\">Mainframe:</td>");
                strSummary.Append("<td width=\"50%\">");
                if (drSummary["mainframe"].ToString() == "1")
                    strSummary.Append("Yes");
                else if (drSummary["mainframe"].ToString() == "0")
                    strSummary.Append("No");
                strSummary.Append("</td>");
                strSummary.Append("</tr>");


                strSummary.Append("<tr>");
                // DATE
                strSummary.Append("<td nowrap class=\"bold\">Build Date:</td>");
                strSummary.Append("<td width=\"50%\">");
                DateTime datDate = DateTime.Now;
                if (DateTime.TryParse(drSummary["commitment"].ToString(), out datDate) == true)
                    strSummary.Append(datDate.ToShortDateString());
                strSummary.Append("</td>");
                // CONFIDENCE
                strSummary.Append("<td nowrap class=\"bold\">Confidence Level:</td>");
                strSummary.Append("<td width=\"50%\">");
                strSummary.Append(drSummary["confidence"].ToString());
                strSummary.Append("</td>");
                strSummary.Append("</tr>");


                strSummary.Append("<tr>");
                // Class + Environment + Location
                strSummary.Append("<td nowrap class=\"bold\">Location:</td>");
                strSummary.Append("<td colspan=\"3\" width=\"100%\">");
                int intClass = 0;
                int intEnv = 0;
                int intAddress = 0;
                Int32.TryParse(drSummary["classid"].ToString(), out intClass);
                if (intClass > 0)
                {
                    strSummary.Append(oClass.Get(intClass, "name") + ", ");
                    Int32.TryParse(drSummary["environmentid"].ToString(), out intEnv);
                    if (intEnv > 0)
                    {
                        strSummary.Append(oEnvironment.Get(intEnv, "name"));
                        Int32.TryParse(drSummary["addressid"].ToString(), out intAddress);
                        if (intAddress > 0)
                            strSummary.Append(" at " + oLocation.GetFull(intAddress));
                    }
                }
                strSummary.Append("</td>");
                strSummary.Append("</tr>");

                strSummary.Append("<tr><td colspan=\"4\" width=\"100%\">&nbsp;</td></tr>");
                strSummary.Append("<tr><td colspan=\"4\" class=\"biggerbold\">Per the <a href=\"http://eawiki/ArchitectureDocs/CtaWikiPortal\"><img src=\"" + oVariable.ImageURL() + "/images/file.gif\" border=\"0\" align=\"absmiddle\" /> PNC reference architecture</a>, the following solution has been selected:</td></tr>");


                strSummary.Append("<tr bgcolor='<%=strHighlight %>'>");
                // Solution / Model
                strSummary.Append("<td nowrap class=\"bold\">Solution:</td>");
                strSummary.Append("<td colspan=\"3\" width=\"100%\">");
                int intModel = GetModelProperty(_designid);
                strSummary.Append(oModelsProperties.Get(intModel, "name"));
                strSummary.Append("</td>");
                strSummary.Append("</tr>");

                strSummary.Append("</table>");
            }

            return strSummary.ToString();
        }

        public string LoadMatrix(string _type, string _letter, string _sun, string _mon, string _tue, string _wed, string _thu, string _fri, string _sat)
        {
            StringBuilder strMatrix = new StringBuilder();
            strMatrix.Append("<table id=\"tbl" + _type + "\" cellpadding=\"6\" cellspacing=\"0\" border=\"0\" style=\"border:solid 1px #CCCCCC\">");
            strMatrix.Append("<tr bgcolor=\"#EEEEEE\">");
            strMatrix.Append("<td>Time:</td>");
            strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictsDay(this,'hdn" + _type + "',1);\">Sun</a></td>");
            strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictsDay(this,'hdn" + _type + "',2);\">Mon</a></td>");
            strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictsDay(this,'hdn" + _type + "',3);\">Tue</a></td>");
            strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictsDay(this,'hdn" + _type + "',4);\">Wed</a></td>");
            strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictsDay(this,'hdn" + _type + "',5);\">Thu</a></td>");
            strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictsDay(this,'hdn" + _type + "',6);\">Fri</a></td>");
            strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictsDay(this,'hdn" + _type + "',7);\">Sat</a></td>");
            strMatrix.Append("</tr>");

            DateTime _date = DateTime.Today;
            for (int ii = 0; ii < 24; ii++)
            {
                string strTime = _date.ToShortTimeString();
                strMatrix.Append("<tr>");
                strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictsTime(this,'hdn" + _type + "','" + strTime + "');\">" + strTime + "</a></td>");
                /*
                strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictCheck(this,'hdn" + _type + "',1,'" + strTime + "');\" class=\"" + (_sun[ii] == '1' ? "selectGridCheck" : "selectGridCancel") + "\"></a></td>");
                strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictCheck(this,'hdn" + _type + "',2,'" + strTime + "');\" class=\"" + (_mon[ii] == '1' ? "selectGridCheck" : "selectGridCancel") + "\"></a></td>");
                strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictCheck(this,'hdn" + _type + "',3,'" + strTime + "');\" class=\"" + (_tue[ii] == '1' ? "selectGridCheck" : "selectGridCancel") + "\"></a></td>");
                strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictCheck(this,'hdn" + _type + "',4,'" + strTime + "');\" class=\"" + (_wed[ii] == '1' ? "selectGridCheck" : "selectGridCancel") + "\"></a></td>");
                strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictCheck(this,'hdn" + _type + "',5,'" + strTime + "');\" class=\"" + (_thu[ii] == '1' ? "selectGridCheck" : "selectGridCancel") + "\"></a></td>");
                strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictCheck(this,'hdn" + _type + "',6,'" + strTime + "');\" class=\"" + (_fri[ii] == '1' ? "selectGridCheck" : "selectGridCancel") + "\"></a></td>");
                strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictCheck(this,'hdn" + _type + "',7,'" + strTime + "');\" class=\"" + (_sat[ii] == '1' ? "selectGridCheck" : "selectGridCancel") + "\"></a></td>");
                */
                strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictLetter(this,'hdn" + _type + "',1,'" + strTime + "','" + _letter + "');\">" + (_sun[ii] == '1' ? "<b>" + _letter + "</b>" : "-") + "</a></td>");
                strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictLetter(this,'hdn" + _type + "',2,'" + strTime + "','" + _letter + "');\">" + (_mon[ii] == '1' ? "<b>" + _letter + "</b>" : "-") + "</a></td>");
                strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictLetter(this,'hdn" + _type + "',3,'" + strTime + "','" + _letter + "');\">" + (_tue[ii] == '1' ? "<b>" + _letter + "</b>" : "-") + "</a></td>");
                strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictLetter(this,'hdn" + _type + "',4,'" + strTime + "','" + _letter + "');\">" + (_wed[ii] == '1' ? "<b>" + _letter + "</b>" : "-") + "</a></td>");
                strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictLetter(this,'hdn" + _type + "',5,'" + strTime + "','" + _letter + "');\">" + (_thu[ii] == '1' ? "<b>" + _letter + "</b>" : "-") + "</a></td>");
                strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictLetter(this,'hdn" + _type + "',6,'" + strTime + "','" + _letter + "');\">" + (_fri[ii] == '1' ? "<b>" + _letter + "</b>" : "-") + "</a></td>");
                strMatrix.Append("<td><a href=\"javascript:void(0);\" onclick=\"SetConflictLetter(this,'hdn" + _type + "',7,'" + strTime + "','" + _letter + "');\">" + (_sat[ii] == '1' ? "<b>" + _letter + "</b>" : "-") + "</a></td>");

                strMatrix.Append("</tr>");
                _date = _date.AddHours(1.00);
            }
            strMatrix.Append("</table>");
            return strMatrix.ToString();
        }

        public void SetupDesign(int _designid, HttpResponse _response)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignSetup", arParams);

            dtPhases = ds.Tables[0];
            dtQuestions = ds.Tables[1];
            dtResponses = ds.Tables[2];


            // ********************************************************************************
            // Show / Hide Responses 
            // ********************************************************************************
            for (int ii = 0; ii < dtResponses.Rows.Count; ii++)
            {
                int intResponse = Int32.Parse(dtResponses.Rows[ii]["ResponseID"].ToString());
                bool boolShowAll = (dtResponses.Rows[ii]["ResponseShowAll"].ToString() == "1");
                bool boolShowAny = (dtResponses.Rows[ii]["ResponseShowAny"].ToString() == "1");
                bool boolHideAll = (dtResponses.Rows[ii]["ResponseHideAll"].ToString() == "1");
                bool boolHideAny = (dtResponses.Rows[ii]["ResponseHideAny"].ToString() == "1");
                DataSet dsShowResponseShow = GetShowResponses(intResponse, 0);
                DataSet dsShowResponseHide = GetShowResponses(intResponse, 1);
                if (dsShowResponseShow.Tables[0].Rows.Count > 0 || dsShowResponseHide.Tables[0].Rows.Count > 0)
                {
                    bool boolSelectedOne = false;
                    bool boolSelectedAll = true;   // Will set to FALSE if one of them is not selected.

                    if ((boolShowAll || boolShowAny) && dsShowResponseShow.Tables[0].Rows.Count > 0)
                    {
                        // One or more responses are required to be selected for this response to be shown.
                        foreach (DataRow drShowResponse in dsShowResponseShow.Tables[0].Rows)
                        {
                            int intRequired = Int32.Parse(drShowResponse["requiredid"].ToString());
                            DataRow[] drPhases = dtResponses.Select("ResponseID = " + intRequired.ToString());
                            if (drPhases[0]["selected"].ToString() == "1")
                            {
                                boolSelectedOne = true;
                                // This response is selected - if ANY of these can be selected for it to be shown, break and continue.
                                if (boolShowAny == true)
                                    break;
                            }
                            else if (boolShowAll == true)
                            {
                                // This response is NOT selected - if ALL of these must be selected for it to be shown, EXIT.
                                dtResponses.Rows[ii]["visible"] = "0";
                                break;
                            }
                        }

                        if (dtResponses.Rows[ii]["visible"].ToString() == "1")
                        {
                            if ((boolHideAll || boolHideAny) && dsShowResponseHide.Tables[0].Rows.Count > 0)
                            {
                                // This option will be shown unless one or more responses are selected.
                                foreach (DataRow drShowResponse in dsShowResponseHide.Tables[0].Rows)
                                {
                                    int intRequired = Int32.Parse(drShowResponse["requiredid"].ToString());
                                    DataRow[] drPhases = dtResponses.Select("ResponseID = " + intRequired.ToString());
                                    if (drPhases[0]["selected"].ToString() == "1")
                                    {
                                        // This response is selected - if ANY of these can be selected for it to be hidden, EXIT.
                                        if (boolHideAny == true)
                                        {
                                            dtResponses.Rows[ii]["visible"] = "0";
                                            break;
                                        }
                                    }
                                    else if (boolHideAll == true)
                                    {
                                        // This response is NOT selected - if ALL of these must be selected for it to be hidden, break and continue.
                                        boolSelectedAll = false;
                                        break;
                                    }
                                }
                                if (boolHideAll == true && boolSelectedAll == true)
                                    dtResponses.Rows[ii]["visible"] = "0";
                            }
                            else if (boolShowAny == true && dsShowResponseShow.Tables[0].Rows.Count > 0)
                                dtResponses.Rows[ii]["visible"] = (boolSelectedOne ? "1" : "0");
                        }
                    }
                }
            }


            // ********************************************************************************
            // Show / Hide Questions 
            // ********************************************************************************
            for (int ii = 0; ii < dtQuestions.Rows.Count; ii++)
            {
                int intQuestion = Int32.Parse(dtQuestions.Rows[ii]["QuestionID"].ToString());
                DataSet dsShow = GetShow(intQuestion);
                foreach (DataRow drShow in dsShow.Tables[0].Rows)
                {
                    int intResponse = Int32.Parse(drShow["responseid"].ToString());
                    DataRow[] drPhases = dtResponses.Select("ResponseID = " + intResponse.ToString());
                    dtQuestions.Rows[ii]["visible"] = (drPhases[0]["selected"].ToString() == "1" && drPhases[0]["visible"].ToString() == "1" ? "1" : "0");
                }
            }


            // ********************************************************************************
            // Lock / Unlock Phases
            // ********************************************************************************
            DataRow[] drResponses = dtResponses.Select("selected = 1 AND visible = 1");
            foreach (DataRow drResponse in drResponses)
            {
                int intResponse = Int32.Parse(drResponse["ResponseID"].ToString());
                DataSet dsPhaseEnable = GetRestrictions(intResponse, 0);
                foreach (DataRow drPhaseEnable in dsPhaseEnable.Tables[0].Rows)
                {
                    int intPhase = Int32.Parse(drPhaseEnable["phaseid"].ToString());
                    DataRow[] drPhases = dtPhases.Select("PhaseID = " + intPhase.ToString());
                    foreach (DataRow drPhase in drPhases)
                        drPhase["locked"] = "0";
                }
                DataSet dsPhaseDisable = GetRestrictions(intResponse, 1);
                foreach (DataRow drPhaseDisable in dsPhaseDisable.Tables[0].Rows)
                {
                    int intPhase = Int32.Parse(drPhaseDisable["phaseid"].ToString());
                    DataRow[] drPhases = dtPhases.Select("PhaseID = " + intPhase.ToString());
                    foreach (DataRow drPhase in drPhases)
                        drPhase["locked"] = "1";
                }
            }



            _response.Write("<table>");
            _response.Write("<tr>");
            foreach (DataColumn dc in dtPhases.Columns)
                _response.Write("<td>" + dc.ColumnName + "</td>");
            _response.Write("</tr>");
            for (int ii = 0; ii < dtPhases.Rows.Count; ii++)
            {
                _response.Write("<tr>");
                foreach (DataColumn dc in dtPhases.Columns)
                    _response.Write("<td>" + dtPhases.Rows[ii][dc.ColumnName].ToString() + "</td>");
                _response.Write("</tr>");
            }
            _response.Write("</table>");
            _response.Write("<p>&nbsp;</p>");


            _response.Write("<table>");
            _response.Write("<tr>");
            foreach (DataColumn dc in dtQuestions.Columns)
                _response.Write("<td>" + dc.ColumnName + "</td>");
            _response.Write("</tr>");
            for (int ii = 0; ii < dtQuestions.Rows.Count; ii++)
            {
                _response.Write("<tr>");
                foreach (DataColumn dc in dtQuestions.Columns)
                    _response.Write("<td>" + dtQuestions.Rows[ii][dc.ColumnName].ToString() + "</td>");
                _response.Write("</tr>");
            }
            _response.Write("</table>");
            _response.Write("<p>&nbsp;</p>");


            _response.Write("<table>");
            _response.Write("<tr>");
            foreach (DataColumn dc in dtResponses.Columns)
                _response.Write("<td>" + dc.ColumnName + "</td>");
            _response.Write("</tr>");
            for (int ii = 0; ii < dtResponses.Rows.Count; ii++)
            {
                _response.Write("<tr>");
                foreach (DataColumn dc in dtResponses.Columns)
                    _response.Write("<td>" + dtResponses.Rows[ii][dc.ColumnName].ToString() + "</td>");
                _response.Write("</tr>");
            }
            _response.Write("</table>");
            _response.Write("<p>&nbsp;</p>");
        }
    }
}
