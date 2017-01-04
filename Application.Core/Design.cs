
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
        private bool IsSetup;
        public bool Setup
        {
            get { return IsSetup; }
            set { IsSetup = value; }
        }

        public List<int> arrHiddenQuestions;


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
        public int AddQuestion(int _phaseid, string _question, string _summary, int _show_summary, int _is_mnemonic, int _is_cost_center, int _is_user_si, int _is_user_dtg, int _is_grid_backup, int _is_backup_exclusions, int _is_grid_maintenance, int _is_storage_luns, int _is_accounts, int _is_date, int _is_location, int _is_type_drop_down, int _is_type_check_box, int _is_type_radio, int _is_type_textbox, int _is_type_textarea, string _related_field, string _default_value, int _allow_empty, string _suffix, int _display, int _enabled)
        {
            arParams = new SqlParameter[27];
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
            arParams[14] = new SqlParameter("@is_location", _is_location);
            arParams[15] = new SqlParameter("@is_type_drop_down", _is_type_drop_down);
            arParams[16] = new SqlParameter("@is_type_check_box", _is_type_check_box);
            arParams[17] = new SqlParameter("@is_type_radio", _is_type_radio);
            arParams[18] = new SqlParameter("@is_type_textbox", _is_type_textbox);
            arParams[19] = new SqlParameter("@is_type_textarea", _is_type_textarea);
            arParams[20] = new SqlParameter("@related_field", _related_field);
            arParams[21] = new SqlParameter("@default_value", _default_value);
            arParams[22] = new SqlParameter("@allow_empty", _allow_empty);
            arParams[23] = new SqlParameter("@suffix", _suffix);
            arParams[24] = new SqlParameter("@display", _display);
            arParams[25] = new SqlParameter("@enabled", _enabled);
            arParams[26] = new SqlParameter("@id", SqlDbType.Int);
            arParams[26].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignQuestion", arParams);
            return Int32.Parse(arParams[26].Value.ToString());
        }
        public void UpdateQuestion(int _id, int _phaseid, string _question, string _summary, int _show_summary, int _is_mnemonic, int _is_cost_center, int _is_user_si, int _is_user_dtg, int _is_grid_backup, int _is_backup_exclusions, int _is_grid_maintenance, int _is_storage_luns, int _is_accounts, int _is_date, int _is_location, int _is_type_drop_down, int _is_type_check_box, int _is_type_radio, int _is_type_textbox, int _is_type_textarea, string _related_field, string _default_value, int _allow_empty, string _suffix, int _enabled)
        {
            arParams = new SqlParameter[26];
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
            arParams[15] = new SqlParameter("@is_location", _is_location);
            arParams[16] = new SqlParameter("@is_type_drop_down", _is_type_drop_down);
            arParams[17] = new SqlParameter("@is_type_check_box", _is_type_check_box);
            arParams[18] = new SqlParameter("@is_type_radio", _is_type_radio);
            arParams[19] = new SqlParameter("@is_type_textbox", _is_type_textbox);
            arParams[20] = new SqlParameter("@is_type_textarea", _is_type_textarea);
            arParams[21] = new SqlParameter("@related_field", _related_field);
            arParams[22] = new SqlParameter("@default_value", _default_value);
            arParams[23] = new SqlParameter("@allow_empty", _allow_empty);
            arParams[24] = new SqlParameter("@suffix", _suffix);
            arParams[25] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignQuestion", arParams);
        }
        public void UpdateQuestionShow(int _id, int _show_all, int _show_any)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@show_all", _show_all);
            arParams[2] = new SqlParameter("@show_any", _show_any);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignQuestionShow", arParams);
        }
        public void UpdateQuestionHide(int _id, int _hide_all, int _hide_any)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@hide_all", _hide_all);
            arParams[2] = new SqlParameter("@hide_any", _hide_any);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignQuestionHide", arParams);
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
                else if (dr["is_location"].ToString() == "1")
                    strSpecial = "LOCATION";
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
        public int AddResponse(int _questionid, string _response, string _summary, string _admin, int _set_classid, int _set_osid, int _set_environmentclassid, int _set_environmentid, int _set_addressid, int _set_modelid, int _set_componentid, int _set_applicationid, int _set_subapplicationid, int _is_under48, int _is_over48, int _is_confidence_lock, int _is_confidence_unlock, int _is_exception, string _related_field, string _related_value, int _quantity_min, int _quantity_max, int _visible, int _select_if_one, int _display, int _enabled)
        {
            arParams = new SqlParameter[27];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@response", _response);
            arParams[2] = new SqlParameter("@summary", _summary);
            arParams[3] = new SqlParameter("@admin", _admin);
            arParams[4] = new SqlParameter("@set_classid", _set_classid);
            arParams[5] = new SqlParameter("@set_osid", _set_osid);
            arParams[6] = new SqlParameter("@set_environmentclassid", _set_environmentclassid);
            arParams[7] = new SqlParameter("@set_environmentid", _set_environmentid);
            arParams[8] = new SqlParameter("@set_addressid", _set_addressid);
            arParams[9] = new SqlParameter("@set_modelid", _set_modelid);
            arParams[10] = new SqlParameter("@set_componentid", _set_componentid);
            arParams[11] = new SqlParameter("@set_applicationid", _set_applicationid);
            arParams[12] = new SqlParameter("@set_subapplicationid", _set_subapplicationid);
            arParams[13] = new SqlParameter("@is_under48", _is_under48);
            arParams[14] = new SqlParameter("@is_over48", _is_over48);
            arParams[15] = new SqlParameter("@is_confidence_lock", _is_confidence_lock);
            arParams[16] = new SqlParameter("@is_confidence_unlock", _is_confidence_unlock);
            arParams[17] = new SqlParameter("@is_exception", _is_exception);
            arParams[18] = new SqlParameter("@related_field", _related_field);
            arParams[19] = new SqlParameter("@related_value", _related_value);
            arParams[20] = new SqlParameter("@quantity_min", _quantity_min);
            arParams[21] = new SqlParameter("@quantity_max", _quantity_max);
            arParams[22] = new SqlParameter("@visible", _visible);
            arParams[23] = new SqlParameter("@select_if_one", _select_if_one);
            arParams[24] = new SqlParameter("@display", _display);
            arParams[25] = new SqlParameter("@enabled", _enabled);
            arParams[26] = new SqlParameter("@id", SqlDbType.Int);
            arParams[26].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignResponse", arParams);
            return Int32.Parse(arParams[26].Value.ToString());
        }
        public void UpdateResponse(int _id, int _questionid, string _response, string _summary, string _admin, int _set_classid, int _set_osid, int _set_environmentclassid, int _set_environmentid, int _set_addressid, int _set_modelid, int _set_componentid, int _set_applicationid, int _set_subapplicationid, int _is_under48, int _is_over48, int _is_confidence_lock, int _is_confidence_unlock, int _is_exception, string _related_field, string _related_value, int _quantity_min, int _quantity_max, int _visible, int _select_if_one, int _enabled)
        {
            arParams = new SqlParameter[26];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@questionid", _questionid);
            arParams[2] = new SqlParameter("@response", _response);
            arParams[3] = new SqlParameter("@summary", _summary);
            arParams[4] = new SqlParameter("@admin", _admin);
            arParams[5] = new SqlParameter("@set_classid", _set_classid);
            arParams[6] = new SqlParameter("@set_osid", _set_osid);
            arParams[7] = new SqlParameter("@set_environmentclassid", _set_environmentclassid);
            arParams[8] = new SqlParameter("@set_environmentid", _set_environmentid);
            arParams[9] = new SqlParameter("@set_addressid", _set_addressid);
            arParams[10] = new SqlParameter("@set_modelid", _set_modelid);
            arParams[11] = new SqlParameter("@set_componentid", _set_componentid);
            arParams[12] = new SqlParameter("@set_applicationid", _set_applicationid);
            arParams[13] = new SqlParameter("@set_subapplicationid", _set_subapplicationid);
            arParams[14] = new SqlParameter("@is_under48", _is_under48);
            arParams[15] = new SqlParameter("@is_over48", _is_over48);
            arParams[16] = new SqlParameter("@is_confidence_lock", _is_confidence_lock);
            arParams[17] = new SqlParameter("@is_confidence_unlock", _is_confidence_unlock);
            arParams[18] = new SqlParameter("@is_exception", _is_exception);
            arParams[19] = new SqlParameter("@related_field", _related_field);
            arParams[20] = new SqlParameter("@related_value", _related_value);
            arParams[21] = new SqlParameter("@quantity_min", _quantity_min);
            arParams[22] = new SqlParameter("@quantity_max", _quantity_max);
            arParams[23] = new SqlParameter("@visible", _visible);
            arParams[24] = new SqlParameter("@select_if_one", _select_if_one);
            arParams[25] = new SqlParameter("@enabled", _enabled);
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
        public void AddShow(int _responseid, int _questionid, int _disabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            arParams[1] = new SqlParameter("@questionid", _questionid);
            arParams[2] = new SqlParameter("@disabled", _disabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignShow", arParams);
        }
        public void DeleteShow(int _questionid, int _disabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@disabled", _disabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignShow", arParams);
        }
        public DataSet GetShows(int _questionid, int _disabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@disabled", _disabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignShows", arParams);
        }
        public DataSet GetShow(int _questionid, int _disabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@disabled", _disabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignShow", arParams);
        }
        public DataSet GetShowsRelated(int _responseid, int _disabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            arParams[1] = new SqlParameter("@disabled", _disabled);
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
        public DataSet GetApprovalsGroup(int _groupid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@groupid", _groupid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApprovalsGroup", arParams);
        }
        public DataSet GetApprovals()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApprovalsAll");
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


        public void AddSubmitted(int _designid, int _userid, string _comments, string _exceptionID)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@comments", _comments);
            arParams[3] = new SqlParameter("@exceptionID", _exceptionID);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignSubmitted", arParams);
        }
        public void DeleteSubmitted(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignSubmitted", arParams);
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
        public void DeleteApproverFieldWorkflow(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignApproverFieldWorkflow", arParams);
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
        public void DeleteApproverGroupWorkflow(int _designid, int _only_exceptions)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@only_exceptions", _only_exceptions);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignApproverGroupWorkflow", arParams);
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


        public int Approve(int _designid, int _userid_cc, bool _exception, int _environment, int intImplementorDistributedService, int intWorkstationPlatform, int intImplementorMidrangeService, string _dsn_asset, string _dsn_ip, string _dsn_service_editor, int _assign_pageid, int _approve_pageid, int _view_pageid)
        {
            return Approve(_designid, _userid_cc, _exception, _environment, intImplementorDistributedService, intWorkstationPlatform, intImplementorMidrangeService, _dsn_asset, _dsn_ip, _dsn_service_editor, _assign_pageid, _approve_pageid, _view_pageid, false);
        }
        public int Approve(int _designid, int _userid_cc, bool _exception, int _environment, int intImplementorDistributedService, int intWorkstationPlatform, int intImplementorMidrangeService, string _dsn_asset, string _dsn_ip, string _dsn_service_editor, int _assign_pageid, int _approve_pageid, int _view_pageid, bool _skip_everything_and_start)
        {
            Settings oSetting = new Settings(user, dsn);
            Functions oFunction = new Functions(user, dsn, _environment);
            Variables oVariable = new Variables(_environment);
            Pages oPage = new Pages(user, dsn);
            Users oUser = new Users(user, dsn);
            Log oLog = new Log(user, dsn);
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

                if (_skip_everything_and_start)
                    oLog.AddEvent(_designid, "", "", "DesignID " + _designid.ToString() + " skip everything and start", LoggingType.Debug);
                else
                {
                    if (_exception)
                    {
                        // Send to board for approval
                        boolSent = NotifyGroup(_designid, _userid_cc, true, _environment);
                        if (boolSent)
                            oLog.AddEvent(_designid, "", "", "DesignID " + _designid.ToString() + " notifications sent to approval board", LoggingType.Debug);
                    }
                    if (boolSent == false)
                    {
                        // No Exception...first, send to individual owners (SI, DTG, etc...)
                        DataSet dsField = GetApproverFields(1);
                        SetupDesign(_designid);
                        foreach (DataRow drField in dsField.Tables[0].Rows)
                        {
                            string strField = drField["related_field"].ToString();
                            string strOwner = drField["title"].ToString();
                            int intApproverFieldID = Int32.Parse(drField["id"].ToString());
                            int intResponse = 0;
                            Int32.TryParse(GetApproverField(intApproverFieldID, "responseid"), out intResponse);
                            if (intResponse == 0 || IsResponseSelected(_designid, intResponse))
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
                                        Notify(_designid, intUser, _environment, "");
                                        // Notify client
                                        Notify(_designid, _userid_cc, _environment, "the " + strOwner);
                                    }
                                }
                            }
                        }
                        if (boolSent)
                            oLog.AddEvent(_designid, "", "", "DesignID " + _designid.ToString() + " notifications sent for field approval", LoggingType.Debug);
                    }
                    if (boolSent == false)
                    {
                        // SI completed...send to individual response owners
                        DataSet dsApproval = GetApprovals();
                        SetupDesign(_designid);
                        foreach (DataRow drApproval in dsApproval.Tables[0].Rows)
                        {
                            int intResponse = 0;
                            Int32.TryParse(drApproval["responseid"].ToString(), out intResponse);
                            if (intResponse > 0 && IsResponseSelected(_designid, intResponse))
                            {
                                int intGroup = 0;
                                if (Int32.TryParse(drApproval["groupid"].ToString(), out intGroup) == true)
                                {
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
                                            Notify(_designid, Int32.Parse(drUser["userid"].ToString()), _environment, "");
                                        // Notify client
                                        Notify(_designid, _userid_cc, _environment, "the " + GetApprovalGroup(intGroup, "name"));
                                    }
                                }
                            }
                        }
                        if (boolSent)
                            oLog.AddEvent(_designid, "", "", "DesignID " + _designid.ToString() + " notifications sent for group approval", LoggingType.Debug);
                    }
                    if (boolSent == false)
                    {
                        // Software Components
                        DataSet dsSoftware = GetSoftwareComponents(_designid);
                        if (dsSoftware.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow drSoftware in dsSoftware.Tables[0].Rows)
                            {
                                //if (drSoftware["initiated"].ToString() == "" || drSoftware["rejected"].ToString() == "-1")
                                if (drSoftware["initiated"].ToString() == "" || drSoftware["rejected"].ToString() == "-1")
                                {
                                    int intComponent = Int32.Parse(drSoftware["componentid"].ToString());
                                    int intResponse = Int32.Parse(drSoftware["responseid"].ToString());
                                    if (drSoftware["rejected"].ToString() == "-1")
                                    {
                                        // Previos request was rejected.  Delete and re-create new record for approval.
                                        DeleteSoftwareComponent(_designid, intComponent);
                                        AddSoftwareComponent(_designid, intComponent, intResponse);
                                    }
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
                                        UpdateSoftwareComponent(Int32.Parse(drSoftware["id"].ToString()), -999, 0, "Automatically Approved", 0);
                                    }
                                }
                            }
                            if (boolSent)
                                oLog.AddEvent(_designid, "", "", "DesignID " + _designid.ToString() + " notifications sent for software component approval", LoggingType.Debug);
                            else
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
                                if (boolSent)
                                    oLog.AddEvent(_designid, "", "", "DesignID " + _designid.ToString() + " software component approvals still pending", LoggingType.Debug);
                            }
                        }
                    }
                    if (boolSent == false)
                    {
                        // Now send to any additional groups for approval
                        boolSent = NotifyGroup(_designid, _userid_cc, false, _environment);
                        if (boolSent)
                            oLog.AddEvent(_designid, "", "", "DesignID " + _designid.ToString() + " notifications sent for additional group approval", LoggingType.Debug);
                    }
                }

                if (boolSent == false)
                {
                    oLog.AddEvent(_designid, "", "", "DesignID " + _designid.ToString() + " - no new notifications sent - checking freeze dates", LoggingType.Debug);
                    // Check Freeze
                    bool boolFreeze = false;
                    string strFreezeStart = oSetting.Get("freeze_start");
                    string strFreezeEnd = oSetting.Get("freeze_end");
                    if (strFreezeStart != "" && strFreezeEnd != "" && DateTime.Parse(strFreezeStart) <= DateTime.Now && DateTime.Parse(strFreezeEnd) > DateTime.Now)
                    {
                        int intFreeze = 0;
                        if (Int32.TryParse(oSetting.Get("freeze_skip_requestid"), out intFreeze) == false || intFreeze != _designid)
                            boolFreeze = true;
                    }

                    if (boolFreeze == false)
                    {
                        oLog.AddEvent(_designid, "", "", "DesignID " + _designid.ToString() + " - not in freeze", LoggingType.Debug);
                        string strCan = CanExecute(_designid);
                        if (strCan == "" || strCan.ToUpper().Contains("MNEMONIC") == false)
                        {
                            oLog.AddEvent(_designid, "", "", "DesignID " + _designid.ToString() + " - build initiated", LoggingType.Debug);
                            // INITIATE BUILD
                            Forecast oForecast = new Forecast(user, dsn);
                            Mnemonic oMnemonic = new Mnemonic(user, dsn);
                            Classes oClass = new Classes(user, dsn);
                            Domains oDomain = new Domains(user, dsn);
                            Requests oRequest = new Requests(user, dsn);
                            Projects oProject = new Projects(user, dsn);
                            ServiceRequests oServiceRequest = new ServiceRequests(user, dsn);
                            Services oService = new Services(user, dsn);
                            Servers oServer = new Servers(user, dsn);
                            Cluster oCluster = new Cluster(user, dsn);
                            Storage oStorage = new Storage(user, dsn);
                            OperatingSystems oOperatingSystem = new OperatingSystems(user, dsn);
                            ServerName oServerName = new ServerName(user, dsn);
                            ModelsProperties oModelsProperties = new ModelsProperties(user, dsn);
                            Resiliency oResiliency = new Resiliency(user, dsn);
                            Asset oAsset = new Asset(user, _dsn_asset, dsn);

                            DataSet dsSummary = Get(_designid);
                            if (dsSummary.Tables[0].Rows.Count > 0)
                            {
                                DataRow drSummary = dsSummary.Tables[0].Rows[0];
                                // Forecast
                                int intForecast = 0;
                                Int32.TryParse(drSummary["forecastid"].ToString(), out intForecast);
                                int intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                                int intProject = oRequest.GetProjectNumber(intRequest);
                                // OS
                                int intOS = 0;
                                Int32.TryParse(drSummary["osid"].ToString(), out intOS);
                                // Storage
                                int intStorage = 0;
                                Int32.TryParse(drSummary["storage"].ToString(), out intStorage);
                                // Implementor
                                int intImplementorDistributed = oService.GetItemId(intImplementorDistributedService);
                                int intImplementorMidrange = oService.GetItemId(intImplementorMidrangeService);
                                bool boolDistributed = oOperatingSystem.IsDistributed(intOS);
                                int intImplementor = GetImplementorWorkflowUser(intProject, boolDistributed, intImplementorDistributed, intImplementorMidrange);

                                if (CanAutoProvision(_designid) == true || intImplementor > 0)
                                {
                                    // Create the CV_FORECAST_ANSWERS table
                                    int intAnswer = oForecast.AddAnswer(intForecast, 1, 0, intRequestor);
                                    UpdateAnswerId(_designid, intAnswer);
                                    oLog.AddEvent(_designid, "", "", "AnswerID " + intAnswer.ToString() + " created from Design " + _designid.ToString(), LoggingType.Debug);
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
                                    // Mnemonic
                                    int intMnemonic = 0;
                                    Int32.TryParse(drSummary["mnemonicid"].ToString(), out intMnemonic);
                                    // Resiliency
                                    bool boolBIR = (drSummary["resiliency"].ToString() == "1");
                                    int intResiliency = oResiliency.GetIDFromMnemonic(intMnemonic, boolBIR, intAnswer);
                                    // Location
                                    int intAddress = 0;
                                    Int32.TryParse(drSummary["addressid"].ToString(), out intAddress);
                                    if (intAddress == 0)
                                    {
                                        oLog.AddEvent(intAnswer, "", "", "Locatrion Not Specified", LoggingType.Debug);
                                        // Will need to get the location based on capacity
                                        if (oModelsProperties.IsTypeVMware(intModel) == false && oModelsProperties.IsSUNVirtual(intModel) == false && oModelsProperties.IsIBMVirtual(intModel) == false)
                                        {
                                            if (boolTest || boolDev)
                                            {
                                                oLog.AddEvent(intAnswer, "", "", "Physical server in TEST?? --> Set to Dalton", LoggingType.Debug);
                                                // Test or Dev servers with an ADDRESSID = 0 should be set to Dalton
                                                intAddress = 696;
                                            }
                                            else
                                            {
                                                int intProjectAddress = 0;
                                                if (Int32.TryParse(oProject.Get(intProject, "addressid"), out intProjectAddress) == true && intProjectAddress > 0)
                                                {
                                                    intAddress = intProjectAddress;
                                                    oLog.AddEvent(intAnswer, "", "", "Physical server, Project Address = " + intProjectAddress.ToString(), LoggingType.Debug);
                                                }
                                                else if (boolProd || boolBIR)
                                                {
                                                    // Physical server - let location be decided at run time
                                                    oLog.AddEvent(intAnswer, "", "", "Physical server - location will be determined at run-time", LoggingType.Debug);
                                                    // OLD => Get based on capacity
                                                    // OLD => intAddress = UpdateDesignLocation(_designid, _dsn_asset);
                                                }
                                                else
                                                {
                                                    // QA + non-BRII => Get based on capacity
                                                    oLog.AddEvent(intAnswer, "", "", "QA, non-BRII server - location is being searched based on inventory...", LoggingType.Debug);
                                                    intAddress = UpdateDesignLocation(_designid, _dsn_asset);
                                                    oLog.AddEvent(intAnswer, "", "", "QA, non-BRII server - location = " + intAddress.ToString(), LoggingType.Debug);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // VMware, Sun Virtual or IBM Virtual (without address set)
                                            int intProjectAddress = 0;
                                            if (Int32.TryParse(oProject.Get(intProject, "addressid"), out intProjectAddress) == true && intProjectAddress > 0)
                                            {
                                                intAddress = intProjectAddress;
                                                oLog.AddEvent(intAnswer, "", "", "Non-Physical server, Project Address = " + intProjectAddress.ToString(), LoggingType.Debug);
                                            }
                                            else
                                            {
                                                // Check Resiliency Location
                                                DataSet dsLocations = oResiliency.GetLocations(intResiliency);
                                                if (dsLocations.Tables[0].Rows.Count == 1)
                                                {
                                                    Int32.TryParse(dsLocations.Tables[0].Rows[0]["prodID"].ToString(), out intAddress);
                                                    oLog.AddEvent(intAnswer, "", "", "Non-Physical server, Resiliency Address = " + intAddress.ToString(), LoggingType.Debug);
                                                }

                                                if (intAddress == 0)
                                                {
                                                    // Get based on capacity, but no capacity for virtual...so set to cleveland
                                                    intAddress = 715;   // Cleveland
                                                    oLog.AddEvent(intAnswer, "", "", "Non-Physical server, Address = " + intAddress.ToString(), LoggingType.Debug);
                                                }
                                            }

                                            if (intAddress > 0)
                                            {
                                                oProject.UpdateLocation(intProject, intAddress, (boolBIR ? 1 : 0));
                                                oLog.AddEvent(intAnswer, "", "", "Non-Physical server, Project Address Updated to be " + intAddress.ToString(), LoggingType.Debug);
                                            }
                                        }
                                    }
                                    else
                                        oLog.AddEvent(intAnswer, "", "", "Locatrion = " + intAddress.ToString(), LoggingType.Debug);
                                    // Quantity
                                    int intQuantity = 0;
                                    Int32.TryParse(drSummary["quantity"].ToString(), out intQuantity);
                                    // Date
                                    DateTime datCommitment = DateTime.Now;
                                    DateTime.TryParse(drSummary["commitment"].ToString(), out datCommitment);
                                    // Cost
                                    int intCost = 0;
                                    Int32.TryParse(drSummary["costid"].ToString(), out intCost);
                                    bool boolWeb = (drSummary["web"].ToString() == "1");
                                    bool boolSQL = IsSQL(_designid);
                                    int intSP = 0;
                                    if (intOS > 0)
                                        Int32.TryParse(oOperatingSystem.Get(intOS, "default_sp"), out intSP);
                                    // ApplicationID and SubApplicationID
                                    int intApp = 0;
                                    Int32.TryParse(drSummary["applicationid"].ToString(), out intApp);
                                    int intSubApp = 0;
                                    Int32.TryParse(drSummary["subapplicationid"].ToString(), out intSubApp);
                                    // Save the Domain and Service Pack
                                    Update(_designid, null, null, intSP, intDomain, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

                                    // Forecast - 1st page
                                    oForecast.UpdateAnswer(intAnswer, 0, 0, "", 0, "", "", _designid.ToString(), intAddress, intClass, 0, intEnv, 1, intApp, intSubApp, intQuantity, (boolBIR ? 1 : 0));
                                    DataSet dsBackup = GetBackup(_designid);
                                    oForecast.UpdateBackup(intAnswer, (dsBackup.Tables[0].Rows.Count > 0 ? 1 : 0));     // At least one backup window defined
                                    DataSet dsStorage = GetStorageAll(_designid);
                                    oForecast.UpdateStorage(intAnswer, (intStorage == 1 && dsStorage.Tables[0].Rows.Count > 0 ? 1 : 0));
                                    // Forecast - last page
                                    oForecast.UpdateAnswer(intAnswer, datCommitment, 5, intRequestor);
                                    oForecast.UpdateAnswerRecovery(intAnswer, (boolProd ? intQuantity : 0));
                                    oForecast.UpdateAnswerHA(intAnswer, 0);
                                    string strMnemonic = oMnemonic.Get(intMnemonic, "factory_code");
                                    List<string> strMnemonicFeed = oMnemonic.GetFeed(strMnemonic);
                                    int intDeptManager = oMnemonic.GetFeedUser(strMnemonicFeed, MnemonicFeed.DM, GetUser(oMnemonic.Get(intMnemonic, "DMName")));
                                    int intAppTechLead = oMnemonic.GetFeedUser(strMnemonicFeed, MnemonicFeed.ATL, GetUser(oMnemonic.Get(intMnemonic, "ATLName")));
                                    int intAppOwner = oMnemonic.GetFeedUser(strMnemonicFeed, MnemonicFeed.AppOwner, GetUser(oMnemonic.Get(intMnemonic, "AppOwner")));
                                    // OnDemand Forecast - app page
                                    oForecast.UpdateAnswer(intAnswer, oMnemonic.Get(intMnemonic, "name"), "", intMnemonic, intCost, 2, intDeptManager, intAppTechLead, 0, intAppOwner, 0, 0, 0);
                                    intRequest = oForecast.GetRequestID(intAnswer, true);
                                    if (intRequest == 0)
                                    {
                                        intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                                        intProject = oRequest.GetProjectNumber(intRequest);
                                        intRequest = oRequest.Add(intProject, intRequestor);
                                    }
                                    oForecast.UpdateAnswer(intAnswer, intRequest);
                                    // Cluster
                                    int intInstances = -1;
                                    double dblQuorum = 0.00;
                                    // HA
                                    if (drSummary["ha"].ToString() == "1")
                                    {
                                        oLog.AddEvent(intAnswer, "", "", "HA", LoggingType.Debug);
                                        if (drSummary["ha_clustering"].ToString() == "1")
                                        {
                                            oLog.AddEvent(intAnswer, "", "", "HA - Clustering", LoggingType.Debug);
                                            Int32.TryParse(drSummary["instances"].ToString(), out intInstances);
                                            double.TryParse(drSummary["quorum"].ToString(), out dblQuorum);
                                        }
                                        else if (drSummary["ha_sve"].ToString() == "1")
                                        {
                                            oLog.AddEvent(intAnswer, "", "", "HA - SVE", LoggingType.Debug);
                                            intInstances = 1;
                                        }
                                    }
                                    int intCluster = 0;
                                    if (intInstances > 0)
                                    {
                                        oLog.AddEvent(intAnswer, "", "", "Configuring the instance(s)", LoggingType.Debug);
                                        // Configure the cluster
                                        intCluster = oCluster.Add(intRequest, "", intQuantity, (boolProd ? intQuantity : 0), 0);
                                        oCluster.UpdateLocalNodes(intCluster, 1);
                                        oCluster.UpdateNonShared(intCluster, 1);
                                        oCluster.UpdateAddInstance(intCluster, 1);
                                        DataSet dsShared = GetStorageShared(_designid, true);
                                        // Configure storage drives (windows only)
                                        int intP = 0;
                                        if (IsWindows(_designid))
                                        {
                                            if (boolSQL)
                                            {
                                                // P DRIVE
                                                oLog.AddEvent(intAnswer, "", "", "Adding P Drive", LoggingType.Debug);
                                                intP = oStorage.AddLun(intAnswer, 0, intCluster, 0, 0, -10, (boolProd ? 1.00 : 0.00), (boolQA ? 1.00 : 0.00), (boolTest || boolDev ? 1.00 : 0.00));
                                            }
                                            if (dblQuorum > 0.00)
                                            {
                                                // Q DRIVE
                                                oLog.AddEvent(intAnswer, "", "", "Adding Q Drive", LoggingType.Debug);
                                                oStorage.AddLun(intAnswer, 0, intCluster, 0, 0, -1, (boolProd ? dblQuorum : 0.00), (boolQA ? dblQuorum : 0.00), (boolTest || boolDev ? dblQuorum : 0.00));
                                            }
                                        }
                                        //bool boolOneInstanceMultipleDrives = (intInstances == 1 && dsShared.Tables[0].Rows.Count > 1);
                                        bool boolOneInstanceMultipleDrives = (intInstances == 1);
                                        for (int ii = 1; ii <= intInstances; ii++)
                                        {
                                            int intInstance = oCluster.AddInstance(intCluster, "", (boolSQL ? 1 : 0));
                                            if (intP > 0)
                                            {
                                                oStorage.UpdateLun(intP, intInstance);
                                                intP = 0;   // so it only happens once
                                            }
                                            // Add shared storage for each instance
                                            AddStorage(dsShared, intAnswer, intCluster, intInstance, 0, (boolOneInstanceMultipleDrives ? 0 : ii), boolProd, boolQA, boolTest, boolDev, boolDR, boolProd);
                                        }
                                    }
                                    for (int ii = 1; ii <= intQuantity; ii++)
                                    {
                                        // Add server record
                                        int intServer = oServer.Add(intRequest, intAnswer, intModel, 0, intCluster, ii, intOS, intSP, 0, intDomain, 0, 0, 0, (boolProd || boolDR ? 1 : 0), 0, "", 0, 0, 1, 1, 1, 1, 0, 1, 0, 0);
                                        // Add software components
                                        DataSet dsSoftware = GetSoftwareComponents(_designid);
                                        int intMHS = 0; // MHS selected in any of the software components
                                        foreach (DataRow drSoftware in dsSoftware.Tables[0].Rows)
                                        {
                                            if (drSoftware["mhs"].ToString() == "1")
                                                intMHS = 1;
                                            int intComponent = Int32.Parse(drSoftware["componentid"].ToString());
                                            if (intComponent > 0)
                                            {
                                                oServerName.AddComponentDetailSelected(intServer, intComponent, 0, false);
                                                oServerName.AddComponentDetailPrerequisites(intServer, intComponent, false);
                                            }
                                        }
                                        oServer.UpdateMHS(intServer, intMHS);
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
                                        AddStorage(GetStorageShared(_designid, false), intAnswer, intCluster, 0, ii, 0, boolProd, boolQA, boolTest, boolDev, boolDR, false);
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
                                    // Generate request for Implementor
                                    if (oServiceRequest.Get(intRequest, "requestid") == "")
                                        oServiceRequest.Add(intRequest, 1, 1);
                                    int intResource = oServiceRequest.AddRequest(intRequest, (boolDistributed ? intImplementorDistributed : intImplementorMidrange), (boolDistributed ? intImplementorDistributedService : intImplementorMidrangeService), 0, 0.00, 2, 1, _dsn_service_editor);
                                    if (oServiceRequest.NotifyApproval(intResource, _approve_pageid, _environment, "", _dsn_service_editor) == false)
                                        oServiceRequest.NotifyTeamLead((boolDistributed ? intImplementorDistributed : intImplementorMidrange), intResource, _assign_pageid, _view_pageid, _environment, "", _dsn_service_editor, _dsn_asset, _dsn_ip, 0);
                                    intReturnAnswerID = 0;
                                }
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
                    else
                    {
                        // There was a problem executing the build...email the requestor.
                        oFunction.SendEmail("Design Approved - Freeze", oUser.GetName(intRequestor), "", strEMailIdsBCC, "Design Approved - Freeze", "<p><b>One of your designs has been completely approved, but cannot be executed until after the freeze.</b></p><p><b>NOTE:</b> Once the freeze has ended on <b>" + strFreezeEnd + "</b>, it is your responsibility to open the design and click the EXECUTE button to start the provisioning process.</p><p>" + GetSummary(_designid, _environment) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/design.aspx?id=" + _designid.ToString() + "\" target=\"_blank\">Click here to review this design.</a></p>", true, false);
                    }
                }
            }
            return intReturnAnswerID;
        }
        public int GetImplementorWorkflow(int _designid, int intImplementorDistributedService, int intImplementorMidrangeService, bool _userid)
        {
            int intForecast = 0;
            int intOS = 0;
            if (Int32.TryParse(Get(_designid, "forecastid"), out intForecast) == true && Int32.TryParse(Get(_designid, "osid"), out intOS) == true)
            {
                Forecast oForecast = new Forecast(0, dsn);
                Requests oRequest = new Requests(0, dsn);
                OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);
                Services oService = new Services(0, dsn);
                int intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                int intProject = oRequest.GetProjectNumber(intRequest);
                if (_userid == true)
                    return GetImplementorWorkflowUser(intProject, oOperatingSystem.IsDistributed(intOS), oService.GetItemId(intImplementorDistributedService), oService.GetItemId(intImplementorMidrangeService));
                else
                    return GetImplementorWorkflow(intProject, oOperatingSystem.IsDistributed(intOS), oService.GetItemId(intImplementorDistributedService), oService.GetItemId(intImplementorMidrangeService));
            }
            else
                return 0;
        }
        public int GetImplementorWorkflowUser(int _projectid, bool _distributed, int intImplementorDistributed, int intImplementorMidrange)
        {
            ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
            int intImplementor = 0;
            int intWorkflow = GetImplementorWorkflow(_projectid, _distributed, intImplementorDistributed, intImplementorMidrange);
            if (intWorkflow > 0)
                Int32.TryParse(oResourceRequest.GetWorkflow(intWorkflow, "userid"), out intImplementor);
            return intImplementor;
        }
        public int GetImplementorWorkflow(int _projectid, bool _distributed, int intImplementorDistributed, int intImplementorMidrange)
        {
            ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
            int intImplementor = 0;
            DataSet dsResource = oResourceRequest.GetProjectItem(_projectid, intImplementorDistributed);
            if (_distributed == false)
                dsResource = oResourceRequest.GetProjectItem(_projectid, intImplementorMidrange);
            if (dsResource.Tables[0].Rows.Count > 0)
                intImplementor = (dsResource.Tables[0].Rows[0]["id"].ToString() == "" ? (dsResource.Tables[0].Rows[0]["assigned"].ToString() == "" ? -1 : 0) : Int32.Parse(dsResource.Tables[0].Rows[0]["id"].ToString()));
            return intImplementor;
        }
        public string CanAutoProvisionReason(int _designid)
        {
            // Only three situations can occur....
            //      1) OS = AIX (check "manual_build" flag on OS)
            //      2) Location is not OPS Center / Dalton (check "manual_build" flag on location)
            //      3) "manual_build" flag is checked in ModelsProperty

            string strManual = "";
            // Location from Design
            int intAddress = 0;
            if (Int32.TryParse(Get(_designid, "addressid"), out intAddress))
            {
                Locations oLocation = new Locations(user, dsn);
                if (oLocation.IsManual(intAddress) == true)
                    strManual = "Location &quot;" + oLocation.GetFull(intAddress) + "&quot; is configured for manual build";
                else
                {
                    // OS from Server Records
                    OperatingSystems oOperatingSystem = new OperatingSystems(user, dsn);
                    ModelsProperties oModelsProperties = new ModelsProperties(user, dsn);
                    int intModel = GetModelProperty(_designid);
                    if (intModel > 0)
                    {
                        if (oModelsProperties.Get(intModel, "manual_build") == "1")
                            strManual = "Model &quot;" + oModelsProperties.Get(intModel, "name") + "&quot; is configured for manual build";
                        else
                        {
                            int intOS = 0;
                            if (Int32.TryParse(Get(_designid, "osid"), out intOS) == true)
                            {
                                if (oOperatingSystem.Get(intOS, "manual_build") == "1")
                                    strManual = "Operating System &quot;" + oOperatingSystem.Get(intOS, "name") + "&quot; is configured for manual build";
                            }
                            else
                                strManual = "Operating System ID &quot;" + Get(_designid, "osid") + "&quot; is not found";
                        }
                    }
                    else
                        strManual = "Model ID &quot;" + intModel.ToString() + "&quot; is not found";
                }
            }
            else
                strManual = "Location ID &quot;" + Get(_designid, "addressid") + "&quot; is not found";
            return strManual;
        }
        public bool CanAutoProvision(int _designid)
        {
            return (CanAutoProvisionReason(_designid) == "");
        }
        public int UpdateDesignLocation(int _designid, string _dsn_asset)
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

        public int GetUser(string _user)
        {
            int intUser = 0;
            Users oUser = new Users(user, dsn);
            DataSet dsUser = oUser.Gets(_user);
            foreach (DataRow drUser in dsUser.Tables[0].Rows)
            {
                // Find first record where both the XID and PNC ID are not blank and don't equal each other
                if (drUser["pnc_id"].ToString().ToUpper() != "" && drUser["xid"].ToString().ToUpper() != ""
                    && drUser["pnc_id"].ToString().ToUpper() != drUser["xid"].ToString().ToUpper())
                {
                    intUser = Int32.Parse(drUser["userid"].ToString());
                    break;
                }
            }
            if (intUser == 0 && dsUser.Tables[0].Rows.Count > 0)
                // If best account not found, but an account exists, just settle and use that one
                intUser = Int32.Parse(dsUser.Tables[0].Rows[0]["userid"].ToString());
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
                // Removed the allocation of shared luns on 1/23/13
                if (intDrive == _driveid || _driveid == 0)
                    oStorage.AddLun(_answerid, _instanceid, _clusterid, 0, _number, intDrive, "Standard", drStorage["path"].ToString(), (_prod ? dblSize : 0.00), (_qa ? dblSize : 0.00), (_test || _dev ? dblSize : 0.00), (_replicate ? 1 : 0), 0);
            }
        }
        private bool NotifyGroup(int _designid, int _userid_cc, bool _only_exceptions, int _environment)
        {
            bool boolSent = false;
            DataSet dsGroup = GetApproverGroups((_only_exceptions ? 1 : 0), 1);
            foreach (DataRow drGroup in dsGroup.Tables[0].Rows)
            {
                int intGroup = Int32.Parse(drGroup["groupid"].ToString());
                bool going = false;
                // Check to see if it's related to a response (if so, skip since it would have kicked off)
                if (_only_exceptions)
                    going = true;
                else
                {
                    DataSet dsResponses = GetApprovalsGroup(intGroup);
                    if (dsResponses.Tables[0].Rows.Count == 0)
                        going = true;
                }
                if (going)
                {
                    string strGroup = drGroup["group"].ToString();
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
                            Notify(_designid, Int32.Parse(drUser["userid"].ToString()), _environment, "");
                        // Notify client
                        Notify(_designid, _userid_cc, _environment, "the " + strGroup);
                    }
                }
            }
            return boolSent;
        }
        private void Notify(int _designid, int _userid, int _environment, string _client_who_is_assigned)
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
            string strBody = "A design has been submitted that requires your approval; you are required to approve or deny this request.";
            if (_client_who_is_assigned != "")
                strBody = "The following design has been sent to " + _client_who_is_assigned + " for approval.";
            if (strURL == "")
                oFunction.SendEmail("Design Approval (" + _designid.ToString() + ")", oUser.GetName(_userid), "", strEMailIdsBCC, "Design Approval (#" + _designid.ToString() + ")", "<p>" + oUser.GetFullName(_userid) + ",</p><p><b>" + strBody + "</b></p><p>" + GetSummary(_designid, _environment) + "</p>", true, false);
            else
                oFunction.SendEmail("Design Approval (" + _designid.ToString() + ")", oUser.GetName(_userid), "", strEMailIdsBCC, "Design Approval (#" + _designid.ToString() + ")", "<p>" + oUser.GetFullName(_userid) + ",</p><p><b>" + strBody + "</b></p><p>" + GetSummary(_designid, _environment) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strURL + oPage.GetFullLink(intPage) + "?id=" + _designid.ToString() + "\" target=\"_blank\">Click here to review this request.</a></p>", true, false);
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
        public DataSet GetStorageDrives(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignStorages", arParams);
        }
        public DataSet GetStorageAll(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignStorageAll", arParams);
        }
        public DataSet GetStorageDrive(int _designid, int _driveid)
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
        public void AddLunSQLPNC(int _designid, double _size, double _non, double _percent, double _tempDB, double _compressionPercentage, double _tempDBOverhead, bool _2008, bool _shared)
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
            AddLun(_designid, intDrive, "", dblLUN.ToString(), _shared);


            // R:\Production
            dblLUN = 10.00;
            AddLun(_designid, intDrive, "\\Production", dblLUN.ToString(), _shared);


            if (IsCluster(_designid))
            {
                // R:\Production\OEM
                dblLUN = 10.00;
                AddLun(_designid, intDrive, "\\Production\\oracle_oem", dblLUN.ToString(), _shared);
            }


            // R:\Production\Database\SQL01 ***
            dblLUN = dblOverallSize;
            double dblLUN_Database = Minimum(dblLUN);
            for (double dblStart = 1.00; dblStart <= dblDividend; dblStart += 1.00)
            {
                string strBefore = "";
                if (dblStart.ToString().Length == 1)
                    strBefore = "0";
                AddLun(_designid, intDrive, "\\Production\\Database\\SQL" + strBefore + dblStart.ToString(), Minimum(dblEachLUN).ToString(), _shared);
            }


            // R:\Production\Logs
            dblLUN = dblOverallSize;
            if (dblLUN > 2000.00)
                dblLUN = 2000.00;   // 2 TB is maximum for LOGS
            double dblLUN_Logs = Minimum(dblLUN);
            AddLun(_designid, intDrive, "\\Production\\Logs", Minimum(dblLUN).ToString(), _shared);


            // R:\Production\TempDB
            //dblLUN = (_tempDB * _tempDBOverhead);
            if (_tempDB == 0.00)
            {
                // Calcualate tempDB dynamically
                _tempDB = (_size * .02);                                // 2% of database size
                _tempDB += (_size * _percent);                          // Plus x% of largest table
            }
            dblLUN = RoundUp(_tempDB + (_tempDB * _tempDBOverhead));    // Plus OverHead value
            AddLun(_designid, intDrive, "\\Production\\TempDB", Minimum(dblLUN).ToString(), _shared);


            // R:\Production\Backups\SQL01*
            double dblBackups = dblOverallSize;
            if (_2008 == true)
                dblBackups = ((dblLUN_Logs + dblLUN_Database) * _compressionPercentage);
            else
                dblBackups = (dblLUN_Logs + dblLUN_Database);
            if (dblBackups < dblLunMax)
            {
                // Save backups to one single LUN
                AddLun(_designid, intDrive, "\\Production\\Backups\\SQL01", Minimum(dblBackups).ToString(), _shared);
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
                        AddLun(_designid, intDrive, "\\Production\\Backups\\SQL" + strBefore + dblStart.ToString(), Minimum(dblEachLUN).ToString(), _shared);
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
                        AddLun(_designid, intDrive, "\\Production\\Backups\\SQL" + strBefore + dblStart.ToString(), Minimum(dblEachLUN).ToString(), _shared);
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
                        AddLun(_designid, intDrive, "\\Production\\Backups\\SQL" + strBefore + dblStart.ToString(), Minimum(dblEachLUNBackup).ToString(), _shared);
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
            DataSet dsStorage = GetStorageDrives(_designid);
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
            DataSet dsStorage = GetStorageDrives(_designid);
            foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
            {
                if (Int32.TryParse(drStorage["size"].ToString(), out intTemp) == true)
                    intTotal += intTemp;
            }
            DataSet dsApp = GetStorageDrive(_designid, -1000);
            if (dsApp.Tables[0].Rows.Count > 0 && IsWindows(_designid))
            {
                if (Int32.TryParse(dsApp.Tables[0].Rows[0]["size"].ToString(), out intTemp) == true)
                    intTotal += intTemp;
            }
            return intTotal;
        }
        public bool IsStorageShared(int _designid)
        {
            bool boolShared = false;
            DataSet dsStorage = GetStorageAll(_designid);
            foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
            {
                if (drStorage["shared"].ToString() == "1")
                {
                    boolShared = true;
                    break;
                }
            }
            return boolShared;
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
        public void DeleteSoftwareComponents(int _designid, bool _recreate)
        {
            if (_recreate == true)
            {
                arParams = new SqlParameter[1];
                arParams[0] = new SqlParameter("@designid", _designid);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignSoftwareComponents", arParams);
            }
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@userid", (_recreate ? 0 : -1));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignSoftwareComponents", arParams);
        }
        public void UpdateSoftwareComponents(int _designid, int _componentid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@componentid", _componentid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignSoftwareComponents", arParams);
        }
        public void UpdateSoftwareComponent(int _id, int _userid, int _rejected, string _reason, int _mhs)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@rejected", _rejected);
            arParams[3] = new SqlParameter("@reason", _reason);
            arParams[4] = new SqlParameter("@mhs", _mhs);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignSoftwareComponent", arParams);
        }
        public void UpdateSoftwareComponentWorkflows(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignSoftwareComponentWorkflows", arParams);
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
            //DataSet dsQuestion = GetQuestions(_phaseid, 1);
            //foreach (DataRow drQuestion in dsQuestion.Tables[0].Rows)
            DataRow[] drQuestions = LoadQuestions(_id, _phaseid);
            foreach (DataRow drQuestion in drQuestions)
            {
                int intQuestion = Int32.Parse(drQuestion["id"].ToString());

                if (drQuestion["available"].ToString() == "1" && drQuestion["show_summary"].ToString() == "1")
                {
                    bool boolDropDown = (drQuestion["is_type_drop_down"].ToString() == "1");
                    bool boolCheckBox = (drQuestion["is_type_check_box"].ToString() == "1");
                    bool boolRadio = (drQuestion["is_type_radio"].ToString() == "1");
                    bool boolTextBox = (drQuestion["is_type_textbox"].ToString() == "1");
                    bool boolTextArea = (drQuestion["is_type_textarea"].ToString() == "1");
                    string strFieldQuestion = drQuestion["related_field"].ToString();
                    string strValueQuestion = drQuestion["default_value"].ToString();
                    bool FoundValueQuestion = false;
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
                        case "LOCATION":
                            Locations oLocation = new Locations(user, dsn);
                            int intLocation = 0;
                            if (Int32.TryParse(Get(_id, strFieldQuestion), out intLocation) == true && intLocation > 0)
                            {
                                string strCommon = oLocation.GetAddress(intLocation, "commonname");
                                if (strCommon == "")
                                    strReturn = AddReturn(strReturn, oLocation.GetFull(intLocation));
                                else
                                    strReturn = strCommon;
                            }
                            break;
                        default:
                            DataRow[] drResponses = LoadResponses(_id, intQuestion);
                            if (boolTextBox || boolTextArea)
                            {
                                strReturn = AddReturn(strReturn, Get(_id, strFieldQuestion));
                            }
                            else
                            {
                                //DataSet dsResponse = GetResponses(intQuestion, 0, 1);
                                //foreach (DataRow drResponse in dsResponse.Tables[0].Rows)
                                foreach (DataRow drResponse in drResponses)
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
                                                if (drResponse["available"].ToString() == "1")
                                                {
                                                    // This option was selected....show response.
                                                    strReturn = AddReturn(strReturn, drResponse["summary"].ToString());
                                                    ResponseID = intResponseTemp;
                                                    //break;
                                                }
                                            }
                                            else if (String.IsNullOrEmpty(strValue) == false && strSet == strValueQuestion)
                                                FoundValueQuestion = true;
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
                            if (strReturn == "" && strValueQuestion != "" && FoundValueQuestion == false)
                            {
                                strReturn = strValueQuestion;
                                // Try to resolve the provided response with the actual (verbiage) response
                                //foreach (DataRow drResponse in dsResponse.Tables[0].Rows)
                                foreach (DataRow drResponse in drResponses)
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
            //DataSet dsQuestion = GetQuestions(_phaseid, 1);
            //foreach (DataRow drQuestion in dsQuestion.Tables[0].Rows)
            DataRow[] drQuestions = LoadQuestions(_id, _phaseid);
            foreach (DataRow drQuestion in drQuestions)
            {
                int intQuestion = Int32.Parse(drQuestion["id"].ToString());
                if (drQuestion["available"].ToString() == "1")
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
            if (this.Setup == false)
                SetupDesign(_id);
            bool boolUnder48 = IsUnder48(_id, true);
            // The value should be like this...."7_Dev" where "7" is the responseid and "Dev" is the actual value.  Split them....
            int intResponse = 0;
            if (_value.Contains("_") == true)
            {
                intResponse = Int32.Parse(_value.Substring(0, _value.IndexOf("_")));
                _value = _value.Substring(_value.IndexOf("_") + 1);
            }
            else
            {
                // Try to get responseID by the field
                DataRow[] drFields = dtResponses.Select("ResponseField = '" + _field + "' AND QuestionID = " + _questionid + " AND available = 1");
                if (drFields.Length == 1)
                    intResponse = Int32.Parse(drFields[0]["ResponseID"].ToString());
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
                string strType = GetDataType(_field);
                if (strType != "")
                {
                    _id = UpdateTable(_id, _forecastid, _phaseid, _field, _value, intResponse);
                    if (_value == "")
                    {
                        // Value was un-selected.  Clear Responses for all questions related to this response.
                        if (intResponse > 0)
                        {
                            UpdateResponseSelected(_id, intResponse, false);
                            DataSet dsShows = GetShowsRelated(intResponse, 0);
                            foreach (DataRow drShow in dsShows.Tables[0].Rows)
                            {
                                int intQuestionClear = Int32.Parse(drShow["questionid"].ToString());
                                ClearResponses(_id, intQuestionClear);
                            }
                            // Find and clear any auto-selections (RESET WINDOWS CLUSTERING, ENTERPRISE -> STANDARD)
                            DataSet dsSelections = GetSelections(intResponse);
                            foreach (DataRow drSelection in dsSelections.Tables[0].Rows)
                            {
                                int intSelectedResponse = Int32.Parse(drSelection["setid"].ToString());
                                bool boolSelected = false;
                                // Check to make sure all other responses are not selected
                                DataSet dsSet = GetSelectionSet(intSelectedResponse);
                                foreach (DataRow drSet in dsSet.Tables[0].Rows)
                                {
                                    int intTemp = Int32.Parse(drSet["responseid"].ToString());
                                    if (IsResponseVisible(_id, intTemp, true) == true && IsResponseSelected(_id, intTemp) == true)
                                        boolSelected = true;
                                }
                                if (boolSelected == false)
                                    UpdateResponseSelected(_id, intSelectedResponse, false);
                                DataSet dsSelect = GetResponse(intSelectedResponse);
                                string strSelectedField = "";
                                string strSelectedValue = "";
                                foreach (DataRow drSelect in dsSelect.Tables[0].Rows)
                                {
                                    int intSelectedQuestion = Int32.Parse(drSelect["questionid"].ToString());
                                    strSelectedField = GetQuestion(intSelectedQuestion, "related_field");
                                    if (strSelectedField == "")
                                        strSelectedField = drSelect["related_field"].ToString();
                                    strSelectedValue = drSelect["related_value"].ToString();
                                    int intTemp = 0;
                                    if (Int32.TryParse(drSelect["set_classid"].ToString(), out intTemp) && intTemp > 0)
                                        strSelectedValue = intTemp.ToString();
                                    else if (Int32.TryParse(drSelect["set_osid"].ToString(), out intTemp) && intTemp > 0)
                                        strSelectedValue = intTemp.ToString();
                                    else if (Int32.TryParse(drSelect["set_environmentid"].ToString(), out intTemp) && intTemp > 0)
                                        strSelectedValue = intTemp.ToString();

                                    if (strSelectedField != "")
                                    {
                                        // Got the field...now find what it should be changed to.
                                        if (Get(_id, strSelectedField) == strSelectedValue)
                                        {
                                            ResetField(_id, strSelectedField);
                                            // Will need to clear this...since it no longer applies
                                            LoadDesign(_id);
                                            //DataSet dsRelated = GetRelatedFields(strSelectedField);
                                            //int intResetQuestion = 0;
                                            //foreach (DataRow drRelated in dsRelated.Tables[0].Rows)
                                            //{
                                            //    // Get auto-selections on field
                                            //    intResetQuestion = Int32.Parse(drRelated["id"].ToString());
                                            //    if (drRelated["response"].ToString() == "1")
                                            //        intResetQuestion = Int32.Parse(GetResponse(intResetQuestion, "questionid"));
                                            //}
                                            //if (intResetQuestion > 0)
                                            //{

                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (intComponentID == 0)
                        Int32.TryParse(_field, out intComponentID);

                    if (intComponentID > 0)
                    {
                        string strSQL = GetSQL(_field, _value);
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

                // Add auto-response value(s)
                if (_value != "")
                {
                    List<string> strFieldsCheck = new List<string>();
                    DataSet dsSelection = GetSelections(intResponse);
                    foreach (DataRow drSelection in dsSelection.Tables[0].Rows)
                    {
                        int intSet = Int32.Parse(drSelection["setid"].ToString());
                        DataSet dsResponse = GetResponse(intSet);
                        // Check to make sure the value is not already set (if multiple selections are causing multiple changes)
                        bool boolAlreadySet = false;
                        bool boolChange = false;
                        string strField = "";
                        string strValue = "";
                        foreach (DataRow drResponse in dsResponse.Tables[0].Rows)
                        {
                            if (GetResponse(intResponse, "visible") == "1" || IsResponseVisible(_id, intSet, true) == false)
                            {
                                intQuestion = Int32.Parse(drResponse["questionid"].ToString());
                                strField = GetQuestion(intQuestion, "related_field");
                                if (strField == "")
                                    strField = drResponse["related_field"].ToString();
                                strValue = drResponse["related_value"].ToString();
                                bool boolAlreadyChangedField = false;
                                foreach (string strFieldCheck in strFieldsCheck)
                                {
                                    if (strFieldCheck == strField)
                                    {
                                        boolAlreadyChangedField = true;
                                        break;
                                    }
                                }
                                if (boolAlreadyChangedField == false)
                                {
                                    int intTemp = 0;
                                    if (Int32.TryParse(drResponse["set_classid"].ToString(), out intTemp) && intTemp > 0)
                                        strValue = intTemp.ToString();
                                    else if (Int32.TryParse(drResponse["set_osid"].ToString(), out intTemp) && intTemp > 0)
                                        strValue = intTemp.ToString();
                                    else if (Int32.TryParse(drResponse["set_environmentid"].ToString(), out intTemp) && intTemp > 0)
                                        strValue = intTemp.ToString();

                                    if (strField != "")
                                    {
                                        if (Get(_id, strField) == strValue)
                                        {
                                            boolAlreadySet = true;
                                            break;
                                        }
                                        else
                                        {
                                            boolChange = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (boolChange == true && boolAlreadySet == false)
                        {
                            int intCurrent = 0;
                            // Get the currently selected Response
                            DataRow[] dsCurrent = LoadResponses(_id, intQuestion);
                            foreach (DataRow drCurrent in dsCurrent)
                            {
                                if (drCurrent["selected"].ToString() == "1")
                                {
                                    Int32.TryParse(drCurrent["ResponseID"].ToString(), out intCurrent);
                                    break;
                                }
                            }
                            // Not set, so change it.
                            //if (intCurrent == 0 || (intCurrent > 0 && GetResponse(intCurrent, "visible") == "1"))
                            if (intCurrent == 0 || (intCurrent > 0 && IsResponseVisible(_id, intCurrent, true)))
                            {
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
                                                strFieldsCheck.Add(drQuestion["related_field"].ToString());
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
        private string GetSQL(string _field, string _value)
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
            return strSQL;
        }
        private int UpdateTable(int _id, int _forecastid, int _phaseid, string _field, string _value, int _responseid_optional) 
        {
            string strSQL = GetSQL(_field, _value);
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
                // Update the Setup Table
                if (_responseid_optional > 0 && this.Setup == true)
                    UpdateResponseSelected(_id, _responseid_optional, true);
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
        public void Update(int _id, int? _classid, int? _osid, int? _spid, int? _domainid, string _backup_frequency, int? _dr, int? _environmentid, int? _ha, int? _ha_clustering, int? _ha_load_balancing, int? _active_passive, int? _instances, int? _quorum, int? _middleware, int? _application, int? _quantity, int? _addressid, int? _modelid, int? _applicationid, int? _subapplicationid, int? _cores, int? _ram)
        {
            // First, check to see if the application / subapplication are set to a model.
            if (_applicationid == 35)
            {
                // if SVE
                if (_modelid != 696 && _modelid != 697)
                {
                    // AND selected model is changed from SVE, then clear the SVE flags
                    _applicationid = 0;
                    _subapplicationid = 0;
                }
            }
            arParams = new SqlParameter[23];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@osid", _osid);
            arParams[3] = new SqlParameter("@spid", _spid);
            arParams[4] = new SqlParameter("@domainid", _domainid);
            arParams[5] = new SqlParameter("@backup_frequency", _backup_frequency);
            arParams[6] = new SqlParameter("@dr", _dr);
            arParams[7] = new SqlParameter("@environmentid", _environmentid);
            arParams[8] = new SqlParameter("@ha", _ha);
            arParams[9] = new SqlParameter("@ha_clustering", _ha_clustering);
            arParams[10] = new SqlParameter("@ha_load_balancing", _ha_load_balancing);
            arParams[11] = new SqlParameter("@active_passive", _active_passive);
            arParams[12] = new SqlParameter("@instances", _instances);
            arParams[13] = new SqlParameter("@quorum", _quorum);
            arParams[14] = new SqlParameter("@middleware", _middleware);
            arParams[15] = new SqlParameter("@application", _application);
            arParams[16] = new SqlParameter("@quantity", _quantity);
            arParams[17] = new SqlParameter("@addressid", _addressid);
            arParams[18] = new SqlParameter("@modelid", _modelid);
            arParams[19] = new SqlParameter("@applicationid", _applicationid);
            arParams[20] = new SqlParameter("@subapplicationid", _subapplicationid);
            arParams[21] = new SqlParameter("@cores", _cores);
            arParams[22] = new SqlParameter("@ram", _ram);
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
        public void UpdateExceptionDone(int _id, bool _is_exception_done)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@is_exception_done", (_is_exception_done ? 1 : 0));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignExceptionDone", arParams);
        }
        public bool IsExceptionDone(int _designid)
        {
            return (Get(_designid, "is_exception_done") == "1");
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
                    //DataSet dsResponse = GetResponses(_questionid, 0, 1);
                    //foreach (DataRow drResponse in dsResponse.Tables[0].Rows)
                    DataRow[] drResponses = LoadResponses(_id, _questionid);
                    foreach (DataRow drResponse in drResponses)
                    {
                        // Go through all responses
                        int intResponse = Int32.Parse(drResponse["id"].ToString());
                        if (drResponse["available"].ToString() == "1")
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
        public void UpdateResponseSelected(int _id, int _responseid, bool _selected)
        {
            if (this.Setup == false)
                SetupDesign(_id);
            DataRow[] drSelected = LoadResponse(_id, _responseid);
            if (drSelected.Length > 0)
                drSelected[0]["selected"] = (_selected ? "1" : "0");
        }
        public bool IsResponseSelected(int _id, int _responseid)
        {
            if (this.Setup == false)
                SetupDesign(_id);
            DataRow[] drSelected = LoadResponse(_id, _responseid);
            if (drSelected.Length > 0)
                return (drSelected[0]["selected"].ToString() == "1");
            else
                return false;
        }
        public bool IsResponseVisible(int _id, int _responseid, bool _check_question)
        {
            if (this.Setup == false)
                SetupDesign(_id);
            DataRow[] drSelected = LoadResponse(_id, _responseid);
            if (drSelected.Length > 0)
                return ((_check_question == false || IsQuestionVisible(_id, Int32.Parse(drSelected[0]["QuestionID"].ToString()))) && drSelected[0]["available"].ToString() == "1");
            else
                return false;
        }
        public bool IsQuestionVisible(int _id, int _questionid)
        {
            if (this.Setup == false)
                SetupDesign(_id);
            DataRow[] drSelected = LoadQuestion(_id, _questionid);
            if (drSelected.Length > 0)
                return (drSelected[0]["available"].ToString() == "1");
            else
                return false;
        }
        public bool IsQuestionSelected(int _id, int _questionid)
        {
            if (this.Setup == false)
                SetupDesign(_id);
            bool boolSelected = false;
            DataSet dsSelected = GetSelection(_questionid);
            if (dsSelected.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drSelected in dsSelected.Tables[0].Rows)
                {
                    int intResponse = Int32.Parse(drSelected["responseid"].ToString());
                    if (IsResponseVisible(_id, intResponse, false) == true && IsResponseSelected(_id, intResponse) == true)
                    {
                        // The selected response matches the one causing this response to be auto-selected.
                        boolSelected = true;
                        break;
                    }
                }
            }
            return boolSelected;
        }
        //public bool IsSelected(int _id, int _questionid, int _responseid)
        //{
        //    bool boolSelected = false;
        //    bool boolHidden = false;
        //    //for (int ii = 0; ii < arrHiddenQuestions.Count; ii++)
        //    //{
        //    //    if (arrHiddenQuestions[ii] == _questionid)
        //    //    {
        //    //        boolHidden = true;
        //    //        break;
        //    //    }
        //    //}
        //    if (boolHidden == false)    // If the question is hidden, the response cannot be selected.
        //    {
        //        //int[] intSelected = GetResponseSelected(_id, _questionid);
        //        List<int> arrSelected = GetResponseSelected(_id, _questionid);
        //        for (int ii = 0; ii < arrSelected.Count; ii++)
        //        {
        //            if (arrSelected[ii] == _responseid)
        //            {
        //                boolSelected = true;
        //                break;
        //            }
        //        }
        //        if (boolSelected == false)
        //        {
        //            bool boolUnder48 = IsUnder48(_id);
        //            // Check if the response is the only one and SELECT IF ONE is enabled.
        //            DataSet dsResponse = GetResponses(_questionid, (boolUnder48 ? 1 : 0), (boolUnder48 ? 0 : 1), 1, 1);
        //            int intResponseOne = 0;
        //            int intResponseCount = dsResponse.Tables[0].Rows.Count;
        //            foreach (DataRow drResponse in dsResponse.Tables[0].Rows)
        //            {
        //                int intResponseTemp = Int32.Parse(drResponse["id"].ToString());
        //                if (IsShowResponse(intResponseTemp, _id) == false)
        //                    intResponseCount--;
        //                else
        //                {
        //                    if (intResponseOne != 0)    // No longer the default, so must be more than one valid response.
        //                        break;
        //                    intResponseOne = intResponseTemp;
        //                }
        //            }
        //            if (intResponseCount == 1 && _responseid == intResponseOne && GetResponse(intResponseOne, "select_if_one") == "1")
        //                boolSelected = true;
        //        }
        //    }
        //    return boolSelected;
        //}

        public bool IsLocked(int _id, int _phaseid)
        {
            if (this.Setup == false)
                SetupDesign(_id);
            DataRow[] drPhase = dtPhases.Select("PhaseID = " + _phaseid.ToString());
            return (drPhase[0]["locked"].ToString() == "1");
            //bool boolLocked = false;
            //DataSet dsRestrictions = GetRestriction(_phaseid, 1);
            //if (dsRestrictions.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow drRestriction in dsRestrictions.Tables[0].Rows)
            //    {
            //        int intResponse = Int32.Parse(drRestriction["responseid"].ToString());
            //        int intQuestion = 0;
            //        if (Int32.TryParse(GetResponse(intResponse, "questionid"), out intQuestion) == true)
            //        {
            //            if (IsSelected(_id, intQuestion, intResponse) == true)
            //            {
            //                // The selected response matches the one causing this response to be auto-selected.
            //                boolLocked = true;
            //                break;
            //            }
            //        }
            //    }
            //}
            //if (boolLocked == false)
            //{
            //    // Check to see if one or more of the selected responses are set to ENABLE
            //    dsRestrictions = GetRestriction(_phaseid, 0);
            //    if (dsRestrictions.Tables[0].Rows.Count > 0)
            //    {
            //        foreach (DataRow drRestriction in dsRestrictions.Tables[0].Rows)
            //        {
            //            int intResponse = Int32.Parse(drRestriction["responseid"].ToString());
            //            int intQuestion = 0;
            //            if (Int32.TryParse(GetResponse(intResponse, "questionid"), out intQuestion) == true)
            //            {
            //                if (IsSelected(_id, intQuestion, intResponse) == false)
            //                {
            //                    // The response needed for this phase to be enabled is NOT selected...so make it locked.
            //                    boolLocked = true;
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}
            //return boolLocked;
        }

        //public bool IsSelected(int _id, int _questionid)
        //{
        //    bool boolSelected = false;
        //    DataSet dsSelected = GetSelection(_questionid);
        //    if (dsSelected.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow drSelected in dsSelected.Tables[0].Rows)
        //        {
        //            int intResponse = Int32.Parse(drSelected["responseid"].ToString());
        //            int intQuestion = 0;
        //            if (Int32.TryParse(GetResponse(intResponse, "questionid"), out intQuestion) == true)
        //            {
        //                if (IsSelected(_id, intQuestion, intResponse) == true)
        //                {
        //                    // The selected response matches the one causing this response to be auto-selected.
        //                    boolSelected = true;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    return boolSelected;
        //}
        //public bool IsHidden(int _id, int _questionid, HttpRequest _request)
        //{
        //    bool boolHidden = false;
        //    DataSet dsShow = GetShow(_questionid);
        //    if (dsShow.Tables[0].Rows.Count > 0)
        //    {
        //        //for (int ii = 0; ii < arrHiddenQuestions.Count; ii++)
        //        //{
        //        //    if (arrHiddenQuestions[ii] == _questionid)
        //        //    {
        //        //        boolHidden = true;
        //        //        break;
        //        //    }
        //        //}
        //        if (boolHidden == false)
        //        {
        //            foreach (DataRow drShow in dsShow.Tables[0].Rows)
        //            {
        //                // ALL responses must be selected for it to be shown.
        //                int intResponse = Int32.Parse(drShow["responseid"].ToString());
        //                int intQuestion = 0;
        //                if (Int32.TryParse(GetResponse(intResponse, "questionid"), out intQuestion) == true)
        //                {
        //                    if (IsSelected(_id, intQuestion, intResponse) == false)
        //                    {
        //                        // The selected response does not match.  Check Postback...
        //                        bool boolPostback = false;
        //                        if (_request != null)
        //                        {
        //                            string strSet = GetResponse(intResponse, "related_value");
        //                            foreach (string strForm in _request.Form)
        //                            {
        //                                if (strForm.StartsWith("HDN_") == true)
        //                                {
        //                                    string strPostbackField = strForm.Substring(4);
        //                                    // strPostbackField : "persistent"
        //                                    string strPostbackValue = _request.Form[strForm];
        //                                    // strPostbackValue : "29_1"
        //                                    // Get ResponseID
        //                                    if (strPostbackValue.Contains("_"))
        //                                    {
        //                                        string strReponseID = strPostbackValue.Substring(0, strPostbackValue.IndexOf("_"));
        //                                        string strSelected = strPostbackValue.Substring(strPostbackValue.IndexOf("_") + 1);
        //                                        if (strReponseID == intResponse.ToString() && strSelected == strSet)
        //                                        {
        //                                            boolPostback = true;
        //                                            break;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        if (boolPostback == false)
        //                        {
        //                            //causing this response to be hidden.
        //                            boolHidden = true;
        //                            break;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        // Its been selected, but is it hidden?
        //                        if (IsHidden(_id, intQuestion, _request) == true)
        //                            boolHidden = true;
        //                        break;
        //                    }
        //                }
        //            }
        //            if (boolHidden == true)
        //                arrHiddenQuestions.Add(_questionid);
        //        }
        //    }
        //    return boolHidden;
        //}

        public void ClearResponses(int _id, int _questionid)
        {
            // Clear other responses
            DataSet dsResponses = GetResponses(_questionid, 0, 1);
            foreach (DataRow drResponses in dsResponses.Tables[0].Rows)
            {
                string strTempField = GetQuestion(_questionid, "related_field");
                int intComponentID = 0;
                Int32.TryParse(drResponses["set_componentid"].ToString(), out intComponentID);
                bool boolFieldQuestion = true;
                if (strTempField == "")
                {
                    strTempField = drResponses["related_field"].ToString();
                    boolFieldQuestion = false;
                }
                if (strTempField != "")
                {
                    // Make sure that this question's field is only updated by this question
                    // (otherwise, we could be overwriting a non-hidden value)
                    DataSet dsClear = GetRelatedFields(strTempField);
                    if (dsClear.Tables[0].Rows.Count == 1)
                    {
                        ResetField(_id, strTempField);
                        if (boolFieldQuestion == true)
                            break;
                    }
                }
                else if (intComponentID > 0)
                {
                    // Delete the component selection
                    DeleteSoftwareComponent(_id, intComponentID);
                }
            }
        }

        public void ResetField(int _id, string _field)
        {
            string strTempType = GetDataType(_field);
            string strTempSQL = "";
            if (strTempType == "DATETIME")
                strTempSQL = "null";
            else if (strTempType == "INT")
                strTempSQL = "0";
            else if (strTempType == "VARCHAR")
                strTempSQL = "''";
            strTempSQL = "UPDATE " + strTable + " SET [" + _field + "] = " + strTempSQL + " WHERE id = " + _id.ToString();
            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, strTempSQL);
        }

        public bool IsUnder48(int _designid, bool _check_client_selection)
        {
            bool boolUnder48 = false;
            if (_check_client_selection == true)
                boolUnder48 = (Get(_designid, "dr") == "1");
            else
            {
                int intResRating = GetResRating(_designid);
                boolUnder48 = (intResRating < 48);
            }
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
            return (Get(_designid, "ha_clustering") == "1" || Get(_designid, "ha_sve") == "1");
        }
        public bool IsStorage(int _designid)
        {
            return (Get(_designid, "storage") == "1");
        }
        public bool IsStoragePersistent(int _designid)
        {
            return (IsStorage(_designid) && (IsRelatedFields("persistent") == false || Get(_designid, "persistent") == "1"));
        }
        public bool IsLocked(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getDesignConfidenceLock", arParams);
            return (o != null);
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
                    if (IsResponseSelected(_id, intResponse) == true)
                    {
                        // The selected response matches the one causing this response to be auto-selected.
                        boolOther = true;
                        break;
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
                if (this.Setup == false)
                    SetupDesign(_designid);

                strValid = "";
                DataRow drSummary = dsSummary.Tables[0].Rows[0];
                bool boolWeb = (drSummary["web"].ToString() == "1");
                bool boolSQL = IsSQL(_designid);
                bool boolOracle = IsOracle(_designid);
                bool boolOtherDB = (drSummary["other_db"].ToString() == "1");

                // Check to see if Demo
                bool boolDemo = false;
                int intForecast = 0;
                int intRequest = 0;
                int intProject = 0;
                Int32.TryParse(drSummary["forecastid"].ToString(), out intForecast);
                if (intForecast > 0)
                {
                    Forecast oForecast = new Forecast(user, dsn);
                    Requests oRequest = new Requests(user, dsn);
                    Projects oProject = new Projects(user, dsn);
                    Functions oFunction = new Functions(user, dsn, 0);
                    Int32.TryParse(oForecast.Get(intForecast, "requestid"), out intRequest);
                    if (intRequest > 0)
                    {
                        intProject = oRequest.GetProjectNumber(intRequest);
                        if (intProject > 0)
                        {
                            string strNumber = oProject.Get(intProject, "number");
                            DataSet dsDemo = oFunction.GetSetupValuesByKey("DEMO_PROJECT");
                            foreach (DataRow drDemo in dsDemo.Tables[0].Rows)
                            {
                                if (strNumber == drDemo["Value"].ToString())
                                {
                                    boolDemo = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                // Mnemonic
                int intMnemonic = 0;
                Int32.TryParse(drSummary["mnemonicid"].ToString(), out intMnemonic);
                if (IsRelatedFields("mnemonicid") && intMnemonic == 0)
                    strValid += "Mnemonic Required,";

                // SI
                if (boolDemo == false)
                {
                    int intSI = 0;
                    Int32.TryParse(drSummary["si"].ToString(), out intSI);
                    if (IsRelatedFields("si") && intSI == 0)
                        strValid += "Service Integrator Required,";
                }

                // Cost
                int intCost = 0;
                Int32.TryParse(drSummary["costid"].ToString(), out intCost);
                if (IsRelatedFields("costid") && intCost == 0)
                    strValid += "Cost Center Required,";

                // Replatforming
                if (IsRelatedFields("replatforming") && drSummary["replatforming"].ToString() != "1" && drSummary["replatforming"].ToString() != "0")
                    strValid += "Business Infrastructure Recovery Required,";

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
                        strValid += "Network Segment Required,";

                    if (boolProd)
                    {
                        if (IsRelatedFields("dr") && drSummary["dr"].ToString() == "")
                            strValid += "DR Requirements Required,";
                    }

                    // Platform Type
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
                    int intQuantityMin = 0;
                    int intQuantityMax = 0;
                    DataRow[] drResponses = dtResponses.Select("selected = 1 AND available = 1 AND (quantity_min > 0 OR quantity_max > 0)", "PhaseDisplay, QuestionDisplay, ResponseDisplay");
                    foreach (DataRow drResponse in drResponses)
                    {
                        int intQuantityMinTemp = Int32.Parse(drResponse["quantity_min"].ToString());
                        if (intQuantityMin == 0 || (intQuantityMinTemp > 0 && intQuantityMinTemp < intQuantityMin))
                            intQuantityMin = intQuantityMinTemp;
                        int intQuantityMaxTemp = Int32.Parse(drResponse["quantity_max"].ToString());
                        if (intQuantityMax == 0 || (intQuantityMaxTemp > 0 && intQuantityMaxTemp > intQuantityMax))
                            intQuantityMax = intQuantityMaxTemp;
                    }
                    if (intQuantityMin > 0 && intQuantity < intQuantityMin)
                        strValid += "Quantity Restriction (Must be at least " + intQuantityMin.ToString() + "),";
                    if (intQuantityMax > 0 && intQuantity > intQuantityMax)
                        strValid += "Quantity Restriction (Must be no more than " + intQuantityMax.ToString() + "),";

                    // OS
                    int intOS = 0;
                    Int32.TryParse(drSummary["osid"].ToString(), out intOS);
                    if (IsRelatedFields("osid") && intOS == 0)
                        strValid += "Operating System Required,";

                    // CORES
                    int intCores = 0;
                    Int32.TryParse(drSummary["cores"].ToString(), out intCores);
                    if (IsRelatedFields("cores") && intCores == 0)
                        strValid += "Number of CPUs Required,";

                    // RAM
                    int intRam = 0;
                    Int32.TryParse(drSummary["ram"].ToString(), out intRam);
                    if (IsRelatedFields("ram") && intRam == 0)
                        strValid += "Amount of RAM Required,";

                    // BACKUP
                    if (IsRelatedFields("backup_frequency"))
                    {
                        string strFrequency = drSummary["backup_frequency"].ToString();
                        if (strFrequency == "")
                            strValid += "Backup Frequency Required,";
                        else if (intMnemonic > 0)
                        {
                            // Check that the backup is correct.
                            bool boolIsUnder48 = IsUnder48(_designid, true);
                            if (boolIsUnder48 && (strFrequency == "W" || strFrequency == "M"))
                                strValid += "Invalid Backup Frequency Specified,";
                        }
                    }
                    if (GetBackup(_designid).Tables[0].Rows.Count == 0)
                        strValid += "Backup Window Required,";

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
                        DataSet dsApp = GetStorageDrive(_designid, -1000);
                        if (dsApp.Tables[0].Rows.Count > 0)
                            double.TryParse(dsApp.Tables[0].Rows[0]["size"].ToString(), out dblAppDrive);
                        if (dblAppDrive == 0.00)
                            strValid += "Application Drive Storage Amount Required,";
                    }

                    int intInstances = -1;
                    if (boolTest || boolQA || boolProd)
                    {
                        // HA
                        if (drSummary["ha"].ToString() == "1")
                        {
                            if (drSummary["ha_sve"].ToString() == "1")
                            {
                                if (IsStorageShared(_designid) == false)
                                    strValid += "SVE Requires Shared Storage,";
                            }
                            else if (drSummary["ha_clustering"].ToString() == "1")
                            {
                                if (drSummary["active_passive"].ToString() != "1" && drSummary["active_passive"].ToString() != "2")
                                    strValid += "Cluster Type Required,";
                                Int32.TryParse(drSummary["instances"].ToString(), out intInstances);
                                if (intInstances == 0)
                                    strValid += "Number of Cluster Instances Required,";
                                int intQuorum = 0;
                                Int32.TryParse(drSummary["quorum"].ToString(), out intQuorum);
                                if (intQuorum == 0)
                                    strValid += "Quorum Drive Size Required,";

                                if (IsStorageShared(_designid) == false)
                                    strValid += "Clustering Requires Shared Storage,";
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
                    }

                    if (boolQA || boolProd)
                    {
                        // MAINTENANCE
                        if (GetMaintenance(_designid).Tables[0].Rows.Count == 0)
                            strValid += "Maintenance Window Required,";
                        // MAINFRAME
                        if (IsRelatedFields("mainframe") && drSummary["mainframe"].ToString() != "1" && drSummary["mainframe"].ToString() != "0")
                            strValid += "Mainframe Selection Required,";
                    }

                    // DATE
                    DateTime datDate = DateTime.Now;
                    if (IsRelatedFields("commitment") && DateTime.TryParse(drSummary["commitment"].ToString(), out datDate) == false)
                        strValid += "Commitment Date Required,";

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
                    if (IsLocked(_designid) == true)
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
                            if (Int32.TryParse(Get(_designid, "mnemonicid"), out intMnemonic) == true)
                            {
                                string strMnemonic = oMnemonic.Get(intMnemonic, "factory_code");
                                string strStatus = oMnemonic.GetFeed(strMnemonic, MnemonicFeed.Status);
                                if (strStatus.ToUpper() == "APPROVED" || oMnemonic.Get(intMnemonic, "status").Trim().ToUpper() == "APPROVED")
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
                                strReturn = "Invalid Mnemonic";
                        }
                        else
                            //strReturn = "Waiting for Commitment Date";
                            strReturn = "Commitment Date Expired";
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
                Forecast oForecast = new Forecast(user, dsn);
                Requests oRequest = new Requests(user, dsn);
                Projects oProject = new Projects(user, dsn);
                Variables oVariable = new Variables(_environment);

                DataRow drSummary = dsSummary.Tables[0].Rows[0];
                bool boolWeb = (drSummary["web"].ToString() == "1");
                bool boolSQL = IsSQL(_designid);
                bool boolOracle = IsOracle(_designid);
                bool boolOtherDB = (drSummary["other_db"].ToString() == "1");

                int intModel = GetModelProperty(_designid);

                strSummary.Append("<table width=\"100%\" cellpadding=\"5\" cellspacing=\"0\" border=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                // Project
                int intForecast = 0;
                int intRequest = 0;
                int intProject = 0;
                Int32.TryParse(drSummary["forecastid"].ToString(), out intForecast);
                if (intForecast > 0)
                {
                    Int32.TryParse(oForecast.Get(intForecast, "requestid"), out intRequest);
                    if (intRequest > 0)
                    {
                        intProject = oRequest.GetProjectNumber(intRequest);
                        if (intProject > 0)
                        {
                            strSummary.Append("<tr>");
                            strSummary.Append("<td nowrap class=\"bold\">Project Name:</td>");
                            strSummary.Append("<td width=\"50%\">");
                            strSummary.Append(oProject.Get(intProject, "name"));
                            strSummary.Append("</td>");
                            strSummary.Append("<td nowrap class=\"bold\">Project Number:</td>");
                            strSummary.Append("<td width=\"50%\">");
                            strSummary.Append(oProject.Get(intProject, "number"));
                            strSummary.Append("</td>");
                            strSummary.Append("</tr>");
                        }
                    }
                }

                
                strSummary.Append("<tr>");
                // Mnemonic
                strSummary.Append("<td nowrap class=\"bold\">Mnemonic:</td>");
                strSummary.Append("<td width=\"50%\">");
                int intMnemonic = 0;
                Int32.TryParse(drSummary["mnemonicid"].ToString(), out intMnemonic);
                if (intMnemonic > 0)
                    strSummary.Append(oMnemonic.Get(intMnemonic, "factory_code") + " - " + oMnemonic.Get(intMnemonic, "name"));
                strSummary.Append("</td>");
                // Quantity
                strSummary.Append("<td nowrap class=\"bold\">Quantity:</td>");
                strSummary.Append("<td width=\"50%\">");
                int intQuantity = 0;
                Int32.TryParse(drSummary["quantity"].ToString(), out intQuantity);
                if (intQuantity > 0)
                {
                    strSummary.Append(intQuantity.ToString());
                    if (IsProd(_designid) == true && IsUnder48(_designid, true) == true)
                        strSummary.Append(" ( + " + intQuantity.ToString() + " for DR)");
                }
                strSummary.Append("</td>");
                strSummary.Append("</tr>");


                strSummary.Append("<tr>");
                // Platform Type
                strSummary.Append("<td nowrap class=\"bold\">Platform Type:</td>");
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
                    else
                    {
                        int intStorage = GetStorageTotal(_designid);
                        if (intStorage > 0)
                            strSummary.Append(intStorage.ToString() + " GB(s)");
                    }
                }
                else if (drSummary["storage"].ToString() == "0")
                    strSummary.Append("No");
                strSummary.Append("</td>");
                strSummary.Append("</tr>");


                strSummary.Append("<tr>");
                // SIZE
                strSummary.Append("<td nowrap class=\"bold\">Platform Size:</td>");
                strSummary.Append("<td width=\"50%\">");
                string strSize = drSummary["cores"].ToString() + " CPU(s), " + drSummary["ram"].ToString() + " GB(s) RAM";
                strSummary.Append(strSize);
                strSummary.Append("</td>");
                // HA
                strSummary.Append("<td nowrap class=\"bold\">High Availability:</td>");
                strSummary.Append("<td width=\"50%\">");
                if (drSummary["ha"].ToString() == "1")
                {
                    if (oModelsProperties.IsSUNVirtual(intModel) == true)
                        strSummary.Append("Sun Virtual Environment (SVE)");
                    else if (drSummary["ha_clustering"].ToString() == "1")
                    {
                        strSummary.Append("Clustered");
                        if (drSummary["active_passive"].ToString() == "1")
                            strSummary.Append(" (Active / Passive)");
                        else if (drSummary["active_passive"].ToString() == "2")
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
                // OS
                strSummary.Append("<td nowrap class=\"bold\">Operating System:</td>");
                strSummary.Append("<td width=\"50%\">");
                int intOS = 0;
                Int32.TryParse(drSummary["osid"].ToString(), out intOS);
                if (intOS > 0)
                    strSummary.Append(oOperatingSystem.Get(intOS, "name"));
                strSummary.Append("</td>");
                // SPECIAL
                strSummary.Append("<td nowrap class=\"bold\">Platform Boot Type:</td>");
                strSummary.Append("<td width=\"50%\">");
                if (oModelsProperties.IsVMwareVirtual(intModel))
                    strSummary.Append("Virtual Hard Disk (VHD)");
                else if (oModelsProperties.IsStorageDB_BootLocal(intModel))
                    strSummary.Append("Local Disk");
                else
                    strSummary.Append("SAN Disk");
                strSummary.Append("</td>");
                strSummary.Append("</tr>");


                strSummary.Append("<tr>");
                // DATE
                strSummary.Append("<td nowrap class=\"bold\">Commitment Date:</td>");
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
                        else
                        {
                            bool boolProd = oClass.IsProd(intClass);
                            bool boolQA = oClass.IsQA(intClass);
                            bool boolTest = oClass.IsTestDev(intClass);
                            bool boolDR = oClass.IsDR(intClass);
                            DataSet dsLocations = oLocation.GetAddressClass((boolDR ? 1 : 0), (boolProd ? 1 : 0), (boolQA ? 1 : 0), (boolTest ? 1 : 0));
                            if (dsLocations.Tables[0].Rows.Count > 0)
                            {
                                StringBuilder strLocation = new StringBuilder();
                                int intLocationCount = 0;
                                foreach (DataRow drLocation in dsLocations.Tables[0].Rows)
                                {
                                    intLocationCount++;
                                    if (intLocationCount > 1)
                                    {
                                        if (intLocationCount > 2)
                                            strLocation.Insert(0, ", ");
                                        else
                                            strLocation.Insert(0, " or ");
                                    }
                                    if (drLocation["commonname"].ToString() != "")
                                        strLocation.Insert(0, drLocation["commonname"].ToString());
                                    else
                                        strLocation.Insert(0, oLocation.GetFull(Int32.Parse(drLocation["id"].ToString())));
                                }
                                strSummary.Append(" at " + strLocation.ToString());
                            }
                        }
                    }
                }
                strSummary.Append("</td>");
                strSummary.Append("</tr>");

                strSummary.Append("<tr><td colspan=\"4\" width=\"100%\">&nbsp;</td></tr>");
                strSummary.Append("<tr><td colspan=\"4\"><b>The following solution has been selected:</b></td></tr>");


                strSummary.Append("<tr bgcolor='#FFEE99'>");
                // Solution / Model
                strSummary.Append("<td nowrap><b>Solution Determined:</b></td>");
                strSummary.Append("<td colspan=\"3\" width=\"100%\">");
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

        public void SetupDesign(int _designid)
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
            DataRow[] drResponses = dtResponses.Select("ResponseShowsDisabled IS NOT NULL OR ResponseShowsEnabled IS NOT NULL", "PhaseDisplay, QuestionDisplay, ResponseDisplay");
            for (int ii = 0; ii < drResponses.Length; ii++)
            {
                int intResponse = Int32.Parse(drResponses[ii]["ResponseID"].ToString());
                DataSet dsShowResponseShow = GetShowResponses(intResponse, 0);
                DataSet dsShowResponseHide = GetShowResponses(intResponse, 1);
                if (dsShowResponseShow.Tables[0].Rows.Count > 0 || dsShowResponseHide.Tables[0].Rows.Count > 0)
                {
                    bool boolShowAll = (drResponses[ii]["ResponseShowAll"].ToString() == "1");
                    bool boolShowAny = (drResponses[ii]["ResponseShowAny"].ToString() == "1");
                    bool boolHideAll = (drResponses[ii]["ResponseHideAll"].ToString() == "1");
                    bool boolHideAny = (drResponses[ii]["ResponseHideAny"].ToString() == "1");
                    bool boolSelectedOne = false;
                    bool boolSelectedAll = true;   // Will set to FALSE if one of them is not selected.

                    if ((boolShowAll || boolShowAny) && dsShowResponseShow.Tables[0].Rows.Count > 0)
                    {
                        // One or more responses are required to be selected for this response to be shown.
                        foreach (DataRow drShowResponse in dsShowResponseShow.Tables[0].Rows)
                        {
                            int intRequired = Int32.Parse(drShowResponse["requiredid"].ToString());
                            DataRow[] drPhases = dtResponses.Select("ResponseID = " + intRequired.ToString());
                            if (drPhases[0]["available"].ToString() == "1" && drPhases[0]["selected"].ToString() == "1")
                            {
                                boolSelectedOne = true;
                                // This response is selected - if ANY of these can be selected for it to be shown, break and continue.
                                if (boolShowAny == true)
                                    break;
                            }
                            else if (boolShowAll == true)
                            {
                                // This response is NOT selected - if ALL of these must be selected for it to be shown, EXIT.
                                drResponses[ii]["available"] = "0";
                                break;
                            }
                        }
                    }

                    if (drResponses[ii]["available"].ToString() == "1")
                    {
                        if ((boolHideAll || boolHideAny) && dsShowResponseHide.Tables[0].Rows.Count > 0)
                        {
                            // This option will be shown unless one or more responses are selected.
                            foreach (DataRow drShowResponse in dsShowResponseHide.Tables[0].Rows)
                            {
                                int intRequired = Int32.Parse(drShowResponse["requiredid"].ToString());
                                DataRow[] drPhases = dtResponses.Select("ResponseID = " + intRequired.ToString());
                                if (drPhases[0]["available"].ToString() == "1" && drPhases[0]["selected"].ToString() == "1")
                                {
                                    // This response is selected - if ANY of these can be selected for it to be hidden, EXIT.
                                    if (boolHideAny == true)
                                    {
                                        drResponses[ii]["available"] = "0";
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
                                drResponses[ii]["available"] = "0";
                        }
                        else if (boolShowAny == true && dsShowResponseShow.Tables[0].Rows.Count > 0)
                            drResponses[ii]["available"] = (boolSelectedOne ? "1" : "0");
                    }
                }
            }


            // ********************************************************************************
            // Show / Hide Questions 
            // ********************************************************************************
            DataRow[] drQuestions = dtQuestions.Select("QuestionShowsDisabled IS NOT NULL OR QuestionShowsEnabled IS NOT NULL", "PhaseDisplay, QuestionDisplay");
            for (int ii = 0; ii < drQuestions.Length; ii++)
            {
                int intQuestion = Int32.Parse(drQuestions[ii]["QuestionID"].ToString());
                int intPhase = Int32.Parse(drQuestions[ii]["PhaseID"].ToString());
                DataSet dsShowQuestionShow = GetShows(intQuestion, 0);
                DataSet dsShowQuestionHide = GetShows(intQuestion, 1);
                if (dsShowQuestionShow.Tables[0].Rows.Count > 0 || dsShowQuestionHide.Tables[0].Rows.Count > 0)
                {
                    bool boolShowAll = (drQuestions[ii]["QuestionShowAll"].ToString() == "1");
                    bool boolShowAny = (drQuestions[ii]["QuestionShowAny"].ToString() == "1");
                    bool boolHideAll = (drQuestions[ii]["QuestionHideAll"].ToString() == "1");
                    bool boolHideAny = (drQuestions[ii]["QuestionHideAny"].ToString() == "1");
                    bool boolSelectedOne = false;
                    bool boolSelectedAll = true;   // Will set to FALSE if one of them is not selected.

                    if ((boolShowAll || boolShowAny) && dsShowQuestionShow.Tables[0].Rows.Count > 0)
                    {
                        // One or more responses are required to be selected for this response to be shown.
                        foreach (DataRow drShowQuestion in dsShowQuestionShow.Tables[0].Rows)
                        {
                            int intResponse = Int32.Parse(drShowQuestion["responseid"].ToString());
                            DataRow[] drPhases = dtResponses.Select("ResponseID = " + intResponse.ToString());
                            if (drPhases[0]["available"].ToString() == "1" && drPhases[0]["selected"].ToString() == "1")
                            {
                                boolSelectedOne = true;
                                // This response is selected - if ANY of these can be selected for it to be shown, break and continue.
                                if (boolShowAny == true)
                                    break;
                            }
                            else if (boolShowAll == true)
                            {
                                // This response is NOT selected - if ALL of these must be selected for it to be shown, EXIT.
                                drQuestions[ii]["available"] = "0";
                                break;
                            }
                        }
                    }

                    if (drQuestions[ii]["available"].ToString() == "1")
                    {
                        if ((boolHideAll || boolHideAny) && dsShowQuestionHide.Tables[0].Rows.Count > 0)
                        {
                            // This option will be shown unless one or more responses are selected.
                            foreach (DataRow drShowQuestion in dsShowQuestionHide.Tables[0].Rows)
                            {
                                int intResponse = Int32.Parse(drShowQuestion["responseid"].ToString());
                                DataRow[] drPhases = dtResponses.Select("ResponseID = " + intResponse.ToString());
                                if (drPhases[0]["available"].ToString() == "1" && drPhases[0]["selected"].ToString() == "1")
                                {
                                    // This response is selected - if ANY of these can be selected for it to be hidden, EXIT.
                                    if (boolHideAny == true)
                                    {
                                        drQuestions[ii]["available"] = "0";
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
                                drQuestions[ii]["available"] = "0";
                        }
                        else if (boolShowAny == true && dsShowQuestionShow.Tables[0].Rows.Count > 0)
                            drQuestions[ii]["available"] = (boolSelectedOne ? "1" : "0");
                    }
                }
            }
                
            //    DataSet dsShow = GetShow(intQuestion, 0);
            //    bool boolSamePhase = false;
            //    if (dsShow.Tables[0].Rows.Count > 1)
            //    {
            //        int intTemp = 0;
            //    }
            //    bool boolFound = false;
            //    foreach (DataRow drShow in dsShow.Tables[0].Rows)
            //    {
            //        int intResponse = Int32.Parse(drShow["responseid"].ToString());
            //        DataRow[] drResponse = dtResponses.Select("ResponseID = " + intResponse.ToString());
            //        int intResponseQuestion = Int32.Parse(drResponse[0]["QuestionID"].ToString());
            //        DataRow[] drQuestion = dtQuestions.Select("QuestionID = " + intResponseQuestion.ToString());
            //        int intQuestionPhase = Int32.Parse(drQuestion[0]["PhaseID"].ToString());
            //        if (intPhase == intQuestionPhase)
            //            boolSamePhase = true;   // Set boolSamePhase to true so that if a response from another phase comes in, it won't overwrite this one (same phase takes precedence)
            //        if (boolFound == false && (intPhase == intQuestionPhase || boolSamePhase == false))
            //            drQuestions[ii]["visible"] = (drQuestion[0]["visible"].ToString() == "1" && drResponse[0]["selected"].ToString() == "1" && drResponse[0]["visible"].ToString() == "1" ? "1" : "0");
            //        else if (drResponse[0]["selected"].ToString() == "0" && drResponse[0]["visible"].ToString() == "1")
            //            drQuestions[ii]["visible"] = "0";

            //        if (drQuestions[ii]["visible"].ToString() == "0")
            //        {
            //            DataRow[] drShowResponses = dtResponses.Select("QuestionID = " + drQuestions[ii]["QuestionID"].ToString() + " AND QuestionVisible = 1", "PhaseDisplay, QuestionDisplay, ResponseDisplay");
            //            foreach (DataRow drShowResponse in drShowResponses)
            //                drShowResponse["QuestionVisible"] = "0";
            //        }
            //        else
            //            boolFound = true;
            //    }
            //}


            // ********************************************************************************
            // Lock / Unlock Phases
            // ********************************************************************************
            //drResponses = dtResponses.Select("selected = 1 AND visible = 1");
            drResponses = dtResponses.Select("selected = 1 AND available = 1 AND (ResponseRestrictionDisabled IS NOT NULL OR ResponseRestrictionEnabled IS NOT NULL)", "PhaseDisplay, QuestionDisplay, ResponseDisplay");
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


            this.Setup = true;
        }

        public void LoadDesign(int _designid)
        {
            // ********************************************************************************
            // Configure LOCATION and MODEL
            // ********************************************************************************
            int intForecast = 0;
            Int32.TryParse(Get(_designid, "forecastid"), out intForecast);

            // OS
            int intOSNew = 0;
            int intOS = 0;
            Int32.TryParse(Get(_designid, "osid"), out intOS);
            // First, try to find all responses that are not OS responses, but change the OS
            DataRow[] drResponses = dtResponses.Select("selected = 1 AND QuestionField <> 'osid' AND ResponseField <> 'osid' AND set_osid > 0", "PhaseDisplay, QuestionDisplay, ResponseDisplay");
            foreach (DataRow drResponse in drResponses)
            {
                intOSNew = Int32.Parse(drResponse["set_osid"].ToString());
                if (drResponse["available"].ToString() == "0")    // Operating System priority is based on visibility.  If not visible, takes precedence.
                    break;
            }
            if (intOSNew == 0)
            {
                // Next, try any that auto-select an OS response
                drResponses = dtResponses.Select("QuestionField = 'osid' OR ResponseField = 'osid'", "PhaseDisplay, QuestionDisplay, ResponseDisplay");
                foreach (DataRow drResponse in drResponses)
                {
                    int intResponseID = Int32.Parse(drResponse["ResponseID"].ToString());   // "Windows Enterprise"
                    int intOSTemp = Int32.Parse(drResponse["set_osid"].ToString());         // Store the selected OS ID
                    DataSet dsAutoSelect = GetSelectionSet(intResponseID);                  // Get all responses that auto-select "Windows Enterprise"
                    foreach (DataRow drAutoSelect in dsAutoSelect.Tables[0].Rows)
                    {
                        intResponseID = Int32.Parse(drAutoSelect["responseid"].ToString());     // "Windows Clustering" auto-selects "Windows Enterprise"
                        if (IsResponseVisible(_designid, intResponseID, true) && IsResponseSelected(_designid, intResponseID))
                        {
                            // "Windows Clustering" has been selected
                            intOSNew = intOSTemp;
                            if (drResponse["available"].ToString() == "0")    // Operating System priority is based on visibility.  If not visible, takes precedence.
                                break;
                        }
                    }
                }

            }
            if (intOSNew == 0)
            {
                // No responses that change the OS...go ahead and include OS responses.
                drResponses = dtResponses.Select("selected = 1 AND available = 1 AND set_osid > 0", "PhaseDisplay, QuestionDisplay, ResponseDisplay");
                //drResponses = dtResponses.Select("selected = 1 AND set_osid > 0", "PhaseDisplay, QuestionDisplay, ResponseDisplay");
                foreach (DataRow drResponse in drResponses)
                    intOSNew = Int32.Parse(drResponse["set_osid"].ToString());
            }
            if (intOSNew != intOS)
                UpdateTable(_designid, intForecast, 0, "osid", intOSNew.ToString(), 0);
            if (intOSNew == 0)    // Clear the responses
            {
                DataSet dsRelated = GetRelatedFields("osid");
                foreach (DataRow drRelated in dsRelated.Tables[0].Rows)
                {
                    int intQuestionCheck = Int32.Parse(drRelated["id"].ToString());
                    DataRow[] drResponeClears = dtResponses.Select("ResponseID = " + intQuestionCheck.ToString(), "PhaseDisplay, QuestionDisplay, ResponseDisplay");
                    if (drRelated["response"].ToString() == "0")
                        drResponeClears = dtResponses.Select("QuestionID = " + intQuestionCheck.ToString(), "PhaseDisplay, QuestionDisplay, ResponseDisplay");
                    foreach (DataRow drResponeClear in drResponeClears)
                        drResponeClear["selected"] = "0";
                }

            }
            // LOCATION
            int intLocationNew = 0;
            int intLocation = 0;
            Int32.TryParse(Get(_designid, "addressid"), out intLocation);
            drResponses = dtResponses.Select("selected = 1 AND available = 1 AND set_addressid > 0", "PhaseDisplay, QuestionDisplay, ResponseDisplay");
            foreach (DataRow drResponse in drResponses)
            {
                intLocationNew = Int32.Parse(drResponse["set_addressid"].ToString());
                break;
            }
            if (intLocationNew != intLocation)
                UpdateTable(_designid, intForecast, 0, "addressid", intLocationNew.ToString(), 0);
            // MODEL
            int intModelNew = 0;
            int intDesignModelNew = 0;
            int intDesignModel = 0;
            Int32.TryParse(Get(_designid, "design_modelid"), out intDesignModel);
            drResponses = dtResponses.Select("selected = 1 AND available = 1 AND set_modelid > 0", "PhaseDisplay, QuestionDisplay, ResponseDisplay");
            foreach (DataRow drResponse in drResponses)
            {
                intDesignModelNew = GetModelPriority(intDesignModelNew, Int32.Parse(drResponse["set_modelid"].ToString()));
                if (intDesignModelNew > 0)
                    Int32.TryParse(GetModel(intDesignModelNew, "modelid"), out intModelNew);
                //break;
            }
            if (intDesignModelNew != intDesignModel)
            {
                UpdateTable(_designid, intForecast, 0, "design_modelid", intDesignModelNew.ToString(), 0);
                UpdateTable(_designid, intForecast, 0, "modelid", intModelNew.ToString(), 0);
            }
            // APP / SUBAPP
            int intApplicationNew = 0;
            int intApplication = 0;
            Int32.TryParse(Get(_designid, "applicationid"), out intApplication);
            drResponses = dtResponses.Select("selected = 1 AND available = 1 AND set_applicationid > 0", "PhaseDisplay, QuestionDisplay, ResponseDisplay");
            foreach (DataRow drResponse in drResponses)
            {
                intApplicationNew = Int32.Parse(drResponse["set_applicationid"].ToString());
                break;
            }
            if (intApplicationNew != intApplication)
                UpdateTable(_designid, intForecast, 0, "applicationid", intApplicationNew.ToString(), 0);
            // SUBAPP
            int intSubApplicationNew = 0;
            int intSubApplication = 0;
            Int32.TryParse(Get(_designid, "subapplicationid"), out intSubApplication);
            drResponses = dtResponses.Select("selected = 1 AND available = 1 AND set_subapplicationid > 0", "PhaseDisplay, QuestionDisplay, ResponseDisplay");
            foreach (DataRow drResponse in drResponses)
            {
                intSubApplicationNew = Int32.Parse(drResponse["set_subapplicationid"].ToString());
                break;
            }
            if (intSubApplicationNew != intSubApplication)
                UpdateTable(_designid, intForecast, 0, "subapplicationid", intSubApplicationNew.ToString(), 0);
        }
        public DataRow[] LoadQuestions(int _designid, int _phaseid)
        {
            if (_designid > 0 && this.Setup == false)
                SetupDesign(_designid);
            return (dtQuestions.Select("PhaseID = " + _phaseid.ToString(), "PhaseDisplay, QuestionDisplay"));
        }
        public DataRow[] LoadQuestion(int _designid, int _questionid)
        {
            if (_designid > 0 && this.Setup == false)
                SetupDesign(_designid);
            return (dtQuestions.Select("QuestionID = " + _questionid.ToString()));
        }
        public DataRow[] LoadResponses(int _designid, int _questionid)
        {
            if (_designid > 0 && this.Setup == false)
                SetupDesign(_designid);
            return (dtResponses.Select("QuestionID = " + _questionid.ToString(), "PhaseDisplay, QuestionDisplay, ResponseDisplay"));
        }
        public DataRow[] LoadResponses(int _designid, int _questionid, bool _under48)
        {
            if (_designid > 0 && this.Setup == false)
                SetupDesign(_designid);
            return (dtResponses.Select("QuestionID = " + _questionid.ToString() + " AND is_under48 >= " + (_under48 ? "1" : "0") + " AND is_over48 >= " + (_under48 ? "0" : "1") + " AND available = 1", "PhaseDisplay, QuestionDisplay, ResponseDisplay"));
        }
        public DataRow[] LoadResponse(int _designid, int _responseid)
        {
            if (_designid > 0 && this.Setup == false)
                SetupDesign(_designid);
            return (dtResponses.Select("ResponseID = " + _responseid.ToString()));
        }

        public DataSet GetForecast()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignsForecast");
        }






        public int AddApprovalConditional(string _name, string _approve_by_field, int _approve_by_group, int _approve_by_requestor, int _approve_by_app_owner, int _approve_by_atl, int _approve_by_asm, int _approve_by_sd, int _approve_by_dm, int _approve_by_cio, int _display, int _enabled)
        {
            arParams = new SqlParameter[13];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@approve_by_field", _approve_by_field);
            arParams[2] = new SqlParameter("@approve_by_group", _approve_by_group);
            arParams[3] = new SqlParameter("@approve_by_requestor", _approve_by_requestor);
            arParams[4] = new SqlParameter("@approve_by_app_owner", _approve_by_app_owner);
            arParams[5] = new SqlParameter("@approve_by_atl", _approve_by_atl);
            arParams[6] = new SqlParameter("@approve_by_asm", _approve_by_asm);
            arParams[7] = new SqlParameter("@approve_by_sd", _approve_by_sd);
            arParams[8] = new SqlParameter("@approve_by_dm", _approve_by_dm);
            arParams[9] = new SqlParameter("@approve_by_cio", _approve_by_cio);
            arParams[10] = new SqlParameter("@display", _display);
            arParams[11] = new SqlParameter("@enabled", _enabled);
            arParams[12] = new SqlParameter("@id", SqlDbType.Int);
            arParams[12].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignApprovalConditional", arParams);
            return Int32.Parse(arParams[12].Value.ToString());
        }
        public void UpdateApprovalConditional(int _id, string _name, string _approve_by_field, int _approve_by_group, int _approve_by_requestor, int _approve_by_app_owner, int _approve_by_atl, int _approve_by_asm, int _approve_by_sd, int _approve_by_dm, int _approve_by_cio, int _enabled)
        {
            arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@approve_by_field", _approve_by_field);
            arParams[3] = new SqlParameter("@approve_by_group", _approve_by_group);
            arParams[4] = new SqlParameter("@approve_by_requestor", _approve_by_requestor);
            arParams[5] = new SqlParameter("@approve_by_app_owner", _approve_by_app_owner);
            arParams[6] = new SqlParameter("@approve_by_atl", _approve_by_atl);
            arParams[7] = new SqlParameter("@approve_by_asm", _approve_by_asm);
            arParams[8] = new SqlParameter("@approve_by_sd", _approve_by_sd);
            arParams[9] = new SqlParameter("@approve_by_dm", _approve_by_dm);
            arParams[10] = new SqlParameter("@approve_by_cio", _approve_by_cio);
            arParams[11] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignApprovalConditional", arParams);
        }
        public void UpdateApprovalConditionalOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignApprovalConditionalOrder", arParams);
        }
        public void EnableApprovalConditional(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignApprovalConditionalEnabled", arParams);
        }
        public void DeleteApprovalConditional(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignApprovalConditional", arParams);
        }
        public DataSet GetApprovalConditional(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApprovalConditional", arParams);
        }
        public string GetApprovalConditional(int _id, string _column)
        {
            DataSet ds = GetApprovalConditional(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetApprovalConditionals(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApprovalConditionals", arParams);
        }

        
        public int AddApprovalConditionalSet(int _approvalid, string _field, int _is_lt, int _is_lte, int _is_gt, int _is_gte, int _is_eq, int _is_neq, int _is_in, int _is_nin, int _is_ends, int _is_starts, int _dt_int, int _dt_date, string _value, int _or_group)
        {
            arParams = new SqlParameter[17];
            arParams[0] = new SqlParameter("@approvalid", _approvalid);
            arParams[1] = new SqlParameter("@field", _field);
            arParams[2] = new SqlParameter("@is_lt", _is_lt);
            arParams[3] = new SqlParameter("@is_lte", _is_lte);
            arParams[4] = new SqlParameter("@is_gt", _is_gt);
            arParams[5] = new SqlParameter("@is_gte", _is_gte);
            arParams[6] = new SqlParameter("@is_eq", _is_eq);
            arParams[7] = new SqlParameter("@is_neq", _is_neq);
            arParams[8] = new SqlParameter("@is_in", _is_in);
            arParams[9] = new SqlParameter("@is_nin", _is_nin);
            arParams[10] = new SqlParameter("@is_ends", _is_ends);
            arParams[11] = new SqlParameter("@is_starts", _is_starts);
            arParams[12] = new SqlParameter("@dt_int", _dt_int);
            arParams[13] = new SqlParameter("@dt_date", _dt_date);
            arParams[14] = new SqlParameter("@value", _value);
            arParams[15] = new SqlParameter("@or_group", _or_group);
            arParams[16] = new SqlParameter("@id", SqlDbType.Int);
            arParams[16].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignApprovalConditionalSet", arParams);
            return Int32.Parse(arParams[16].Value.ToString());
        }
        public void UpdateApprovalConditionalSet(int _id, int _approvalid, string _field, int _is_lt, int _is_lte, int _is_gt, int _is_gte, int _is_eq, int _is_neq, int _is_in, int _is_nin, int _is_ends, int _is_starts, int _dt_int, int _dt_date, string _value, int _or_group)
        {
            arParams = new SqlParameter[17];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@approvalid", _approvalid);
            arParams[2] = new SqlParameter("@field", _field);
            arParams[3] = new SqlParameter("@is_lt", _is_lt);
            arParams[4] = new SqlParameter("@is_lte", _is_lte);
            arParams[5] = new SqlParameter("@is_gt", _is_gt);
            arParams[6] = new SqlParameter("@is_gte", _is_gte);
            arParams[7] = new SqlParameter("@is_eq", _is_eq);
            arParams[8] = new SqlParameter("@is_neq", _is_neq);
            arParams[9] = new SqlParameter("@is_in", _is_in);
            arParams[10] = new SqlParameter("@is_nin", _is_nin);
            arParams[11] = new SqlParameter("@is_ends", _is_ends);
            arParams[12] = new SqlParameter("@is_starts", _is_starts);
            arParams[13] = new SqlParameter("@dt_int", _dt_int);
            arParams[14] = new SqlParameter("@dt_date", _dt_date);
            arParams[15] = new SqlParameter("@value", _value);
            arParams[16] = new SqlParameter("@or_group", _or_group);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignApprovalConditionalSet", arParams);
        }
        public void DeleteApprovalConditionalSet(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignApprovalConditionalSet", arParams);
        }
        public DataSet GetApprovalConditionalSet(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApprovalConditionalSet", arParams);
        }
        public string GetApprovalConditionalSet(int _id, string _column)
        {
            DataSet ds = GetApprovalConditionalSet(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetApprovalConditionalSets(int _approvalid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@approvalid", _approvalid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDesignApprovalConditionalSets", arParams);
        }

        public void AddApprovalConditionalWorkflow(int _designid, int _approvalid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@designid", _designid);
            arParams[1] = new SqlParameter("@approvalid", _approvalid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDesignApprovalConditionalWorkflow", arParams);
        }

        public void GetApprovalConditionalWorkflow(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_getDesignApprovalConditionalWorkflow", arParams);
        }

        public void DeleteApprovalConditionalWorkflow(int _designid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@designid", _designid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDesignApprovalConditionalWorkflow", arParams);
        }

        public void UpdateApprovalConditionalWorkflow(int _id, int _userid, int _rejected, string _reason)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@rejected", _rejected);
            arParams[3] = new SqlParameter("@reason", _reason);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDesignApprovalConditionalWorkflow", arParams);
        }

    }
}
