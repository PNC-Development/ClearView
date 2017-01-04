using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;
using System.Text;

namespace NCC.ClearView.Application.Core
{
	public class Forecast
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        private Design oDesign;
        public Forecast(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
            oDesign = new Design(user, dsn);
		}
        public DataSet GetsSearch(string _nickname, int _designid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@nickname", _nickname);
            arParams[1] = new SqlParameter("@designid", _designid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastsSearch", arParams);
        }
        public DataSet Gets()
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", user);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecasts", arParams);
        }
        public DataSet GetsInactive()
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", user);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastsInactive", arParams);
        }
        public DataSet Gets(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastsUser", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecast", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetProject(int _projectid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastProject", arParams);
        }
        public DataSet GetRequest(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastRequest", arParams);
        }
        public int GetPlatformCount(int _platformid, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            arParams[1] = new SqlParameter("@userid", _userid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getForecastsPlatformUser", arParams);
            if (o == null || o.ToString() == "")
                return 0;
            else
                return Int32.Parse(o.ToString());
        }
        public int GetPlatformCount(int _platformid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getForecastsPlatform", arParams);
            if (o == null || o.ToString() == "")
                return 0;
            else
                return Int32.Parse(o.ToString());
        }
        public int Add(int _requestid, string _pnc_project, int _userid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@pnc_project", _pnc_project);
            arParams[2] = new SqlParameter("@userid", _userid);
            arParams[3] = new SqlParameter("@id", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecast", arParams);
            return Int32.Parse(arParams[3].Value.ToString());
        }
        public void Update(int _id, string _pnc_project)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@pnc_project", _pnc_project);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastPNC", arParams);
        }
        public void Update(int _id, int _active)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@active", _active);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecast", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecast", arParams);
        }

        public void AddQuestion(string _name, string _question, int _type, int _hide_override, int _required, int _display, int _enabled)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@question", _question);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@hide_override", _hide_override);
            arParams[4] = new SqlParameter("@required", _required);
            arParams[5] = new SqlParameter("@display", _display);
            arParams[6] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastQuestion", arParams);
        }
        public void UpdateQuestion(int _id, string _name, string _question, int _type, int _hide_override, int _required, int _enabled)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@question", _question);
            arParams[3] = new SqlParameter("@type", _type);
            arParams[4] = new SqlParameter("@hide_override", _hide_override);
            arParams[5] = new SqlParameter("@required", _required);
            arParams[6] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastQuestion", arParams);
        }
        public void UpdateQuestionOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastQuestionOrder", arParams);
        }
        public void EnableQuestion(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastQuestionEnabled", arParams);
        }
        public void DeleteQuestion(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastQuestion", arParams);
        }
        public string GetQuestion(int _id, string _column)
        {
            DataSet ds = GetQuestion(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetQuestion(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastQuestion", arParams);
        }
        public DataSet GetQuestions(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastQuestions", arParams);
        }

        public void AddResponse(int _questionid, string _name, string _response, int _variance, int _custom, int _os_distributed, int _os_midrange, int _cores, int _ram, int _web, int _dbase, int _ha_none, int _ha_cluster, int _ha_csm, int _ha_csm_middleware, int _ha_csm_app, int _ha_room, int _dr_under, int _dr_over, int _one_one, int _many_one, string _components, int _forecastresponsecategory, int _display, int _enabled)
        {
            arParams = new SqlParameter[25];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@response", _response);
            arParams[3] = new SqlParameter("@variance", _variance);
            arParams[4] = new SqlParameter("@custom", _custom);
            arParams[5] = new SqlParameter("@os_distributed", _os_distributed);
            arParams[6] = new SqlParameter("@os_midrange", _os_midrange);
            arParams[7] = new SqlParameter("@cores", _cores);
            arParams[8] = new SqlParameter("@ram", _ram);
            arParams[9] = new SqlParameter("@web", _web);
            arParams[10] = new SqlParameter("@dbase", _dbase);
            arParams[11] = new SqlParameter("@ha_none", _ha_none);
            arParams[12] = new SqlParameter("@ha_cluster", _ha_cluster);
            arParams[13] = new SqlParameter("@ha_csm", _ha_csm);
            arParams[14] = new SqlParameter("@ha_csm_middleware", _ha_csm_middleware);
            arParams[15] = new SqlParameter("@ha_csm_app", _ha_csm_app);
            arParams[16] = new SqlParameter("@ha_room", _ha_room);
            arParams[17] = new SqlParameter("@dr_under", _dr_under);
            arParams[18] = new SqlParameter("@dr_over", _dr_over);
            arParams[19] = new SqlParameter("@one_one", _one_one);
            arParams[20] = new SqlParameter("@many_one", _many_one);
            arParams[21] = new SqlParameter("@components", _components);
            arParams[22] = new SqlParameter("@forecast_response_category_id", _forecastresponsecategory);
            arParams[23] = new SqlParameter("@display", _display);
            arParams[24] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastResponse", arParams);
        }
        public void UpdateResponse(int _id, int _questionid, string _name, string _response, int _variance, int _custom, int _os_distributed, int _os_midrange, int _cores, int _ram, int _web, int _dbase, int _ha_none, int _ha_cluster, int _ha_csm, int _ha_csm_middleware, int _ha_csm_app, int _ha_room, int _dr_under, int _dr_over, int _one_one, int _many_one, string _components, int _forecastresponsecategory, int _enabled)
        {
            arParams = new SqlParameter[25];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@questionid", _questionid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@response", _response);
            arParams[4] = new SqlParameter("@variance", _variance);
            arParams[5] = new SqlParameter("@custom", _custom);
            arParams[6] = new SqlParameter("@os_distributed", _os_distributed);
            arParams[7] = new SqlParameter("@os_midrange", _os_midrange);
            arParams[8] = new SqlParameter("@cores", _cores);
            arParams[9] = new SqlParameter("@ram", _ram);
            arParams[10] = new SqlParameter("@web", _web);
            arParams[11] = new SqlParameter("@dbase", _dbase);
            arParams[12] = new SqlParameter("@ha_none", _ha_none);
            arParams[13] = new SqlParameter("@ha_cluster", _ha_cluster);
            arParams[14] = new SqlParameter("@ha_csm", _ha_csm);
            arParams[15] = new SqlParameter("@ha_csm_middleware", _ha_csm_middleware);
            arParams[16] = new SqlParameter("@ha_csm_app", _ha_csm_app);
            arParams[17] = new SqlParameter("@ha_room", _ha_room);
            arParams[18] = new SqlParameter("@dr_under", _dr_under);
            arParams[19] = new SqlParameter("@dr_over", _dr_over);
            arParams[20] = new SqlParameter("@one_one", _one_one);
            arParams[21] = new SqlParameter("@many_one", _many_one);
            arParams[22] = new SqlParameter("@components", _components);
            arParams[23] = new SqlParameter("@forecast_response_category_id", _forecastresponsecategory);
            arParams[24] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastResponse", arParams);
        }
        public void UpdateResponseOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastResponseOrder", arParams);
        }
        public void EnableResponse(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastResponseEnabled", arParams);
        }
        public void DeleteResponse(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastResponse", arParams);
        }
        public string GetResponse(int _id, string _column)
        {
            DataSet ds = GetResponse(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetResponse(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastResponse", arParams);
        }
        public DataSet GetResponses(int _questionid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastResponses", arParams);
        }
        public DataSet GetResponsesNoCustom(int _questionid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastResponsesNoCustom", arParams);
        }
        public DataSet GetResponses(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastResponsesAnswer", arParams);
        }
        //public bool GetResponses(int _answerid, string _value)
        //{
        //    DataSet ds = GetResponses(_answerid);
        //    bool boolFound = false;
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        if (dr["response"].ToString().ToUpper() == _value.ToUpper())
        //        {
        //            boolFound = true;
        //            break;
        //        }
        //    }
        //    return boolFound;
        //}
        public bool IsOSDistributed(int _answerid)
        {
            DataSet ds = GetResponses(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
                return Is(ds, "os_distributed");
            else
            {
                ds = oDesign.GetAnswer(_answerid);
                if (ds.Tables[0].Rows.Count == 0)
                    return false;
                else
                {
                    int intOS = 0;
                    OperatingSystems oOperatingSystem = new OperatingSystems(user, dsn);
                    if (Int32.TryParse(ds.Tables[0].Rows[0]["osid"].ToString(), out intOS) == true)
                        return oOperatingSystem.IsDistributed(intOS);
                    else
                        return false;
                }
            }
        }
        public bool IsOSMidrange(int _answerid)
        {
            DataSet ds = GetResponses(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
                return Is(ds, "os_midrange");
            else
            {
                ds = oDesign.GetAnswer(_answerid);
                if (ds.Tables[0].Rows.Count == 0)
                    return false;
                else
                {
                    int intOS = 0;
                    OperatingSystems oOperatingSystem = new OperatingSystems(user, dsn);
                    if (Int32.TryParse(ds.Tables[0].Rows[0]["osid"].ToString(), out intOS) == true)
                        return oOperatingSystem.IsMidrange(intOS);
                    else
                        return false;
                }
            }
        }
        //public bool IsCores(int _answerid)
        //{
        //    DataSet ds = GetResponses(_answerid);
        //    if (ds.Tables[0].Rows.Count > 0)
        //        return Is(ds, "cores");
        //    else
        //    {
        //        ds = oDesign.GetAnswer(_answerid);
        //        if (ds.Tables[0].Rows.Count == 0)
        //            return false;
        //        else
        //            return false;
        //    }
        //}
        //public bool IsRAM(int _answerid)
        //{
        //    DataSet ds = GetResponses(_answerid);
        //    if (ds.Tables[0].Rows.Count > 0)
        //        return Is(ds, "ram");
        //    else
        //    {
        //        ds = oDesign.GetAnswer(_answerid);
        //        if (ds.Tables[0].Rows.Count == 0)
        //            return false;
        //        else
        //            return false;
        //    }
        //}
        public bool IsWeb(int _answerid)
        {
            DataSet ds = GetResponses(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
                return Is(ds, "web");
            else
            {
                ds = oDesign.GetAnswer(_answerid);
                if (ds.Tables[0].Rows.Count == 0)
                    return false;
                else
                    return (ds.Tables[0].Rows[0]["web"].ToString() == "1");
            }
        }
        public bool IsDatabase(int _answerid)
        {
            DataSet ds = GetResponses(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
                return Is(ds, "dbase");
            else
            {
                ds = oDesign.GetAnswer(_answerid);
                if (ds.Tables[0].Rows.Count == 0)
                    return false;
                else
                    return (ds.Tables[0].Rows[0]["database"].ToString() == "1");
            }
        }
        public bool IsHANone(int _answerid)
        {
            DataSet ds = GetResponses(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
                return Is(ds, "ha_none");
            else
            {
                ds = oDesign.GetAnswer(_answerid);
                if (ds.Tables[0].Rows.Count == 0)
                    return false;
                else
                    return (ds.Tables[0].Rows[0]["ha_clustering"].ToString() != "1" && ds.Tables[0].Rows[0]["ha_load_balancing"].ToString() != "1");
            }
        }
        public bool IsHACluster(int _answerid)
        {
            DataSet ds = GetResponses(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
                return Is(ds, "ha_cluster");
            else
            {
                ds = oDesign.GetAnswer(_answerid);
                if (ds.Tables[0].Rows.Count == 0)
                    return false;
                else
                {
                    ds = oDesign.GetAnswer(_answerid);
                    if (ds.Tables[0].Rows.Count == 0)
                        return false;
                    else
                        return (ds.Tables[0].Rows[0]["ha_clustering"].ToString() == "1");
                }
            }
        }
        public bool IsHACSM(int _answerid)
        {
            DataSet ds = GetResponses(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
                return Is(ds, "ha_csm");
            else
            {
                ds = oDesign.GetAnswer(_answerid);
                if (ds.Tables[0].Rows.Count == 0)
                    return false;
                else
                    return (ds.Tables[0].Rows[0]["ha_load_balancing"].ToString() == "1");
            }
        }
        public bool IsHACSMMiddleware(int _answerid)
        {
            DataSet ds = GetResponses(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
                return Is(ds, "ha_csm_middleware");
            else
            {
                ds = oDesign.GetAnswer(_answerid);
                if (ds.Tables[0].Rows.Count == 0)
                    return false;
                else
                    return (ds.Tables[0].Rows[0]["middleware"].ToString() == "1");
            }
        }
        public bool IsHACSMApp(int _answerid)
        {
            DataSet ds = GetResponses(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
                return Is(ds, "ha_csm_app");
            else
            {
                ds = oDesign.GetAnswer(_answerid);
                if (ds.Tables[0].Rows.Count == 0)
                    return false;
                else
                {
                    if (ds.Tables[0].Rows[0]["web"].ToString() == "1")
                        return (ds.Tables[0].Rows[0]["application"].ToString() == "1");
                    else
                        return true;
                }
            }
        }
        public bool IsHACSMWeb(int _answerid)
        {
            DataSet ds = oDesign.GetAnswer(_answerid);
            if (ds.Tables[0].Rows.Count == 0)
                return false;
            else
            {
                if (ds.Tables[0].Rows[0]["web"].ToString() == "1")
                    return (ds.Tables[0].Rows[0]["application"].ToString() != "1");
                else
                    return false;
            }
        }
        public bool IsHARoom(int _answerid)
        {
            DataSet ds = GetResponses(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
                return Is(ds, "ha_room");
            else
                return false;
        }
        public bool IsDRUnder48(int _answerid, bool _check_client_selection)
        {
            DataSet ds = GetResponses(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
                return Is(ds, "dr_under");
            else
            {
                ds = oDesign.GetAnswer(_answerid);
                if (ds.Tables[0].Rows.Count == 0)
                    return false;
                else
                {
                    int intDesign = 0;
                    if (Int32.TryParse(ds.Tables[0].Rows[0]["id"].ToString(), out intDesign) == true)
                        return oDesign.IsUnder48(intDesign, true);
                    else
                        return false;
                }
            }
        }
        public bool IsDROver48(int _answerid, bool _check_client_selection)
        {
            DataSet ds = GetResponses(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
                return Is(ds, "dr_over");
            else
            {
                ds = oDesign.GetAnswer(_answerid);
                if (ds.Tables[0].Rows.Count == 0)
                    return false;
                else
                {
                    int intDesign = 0;
                    if (Int32.TryParse(ds.Tables[0].Rows[0]["id"].ToString(), out intDesign) == true)
                        return (oDesign.IsUnder48(intDesign, true) == false);
                    else
                        return false;
                }
            }
        }
        public bool IsDROneToOne(int _answerid)
        {
            DataSet ds = GetResponses(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
                return Is(ds, "one_one");
            else
            {
                ds = oDesign.GetAnswer(_answerid);
                if (ds.Tables[0].Rows.Count == 0)
                    return false;
                else
                    return true;
            }
        }
        public bool IsDRManyToOne(int _answerid)
        {
            DataSet ds = GetResponses(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
                return Is(ds, "many_one");
            else
            {
                ds = oDesign.GetAnswer(_answerid);
                if (ds.Tables[0].Rows.Count == 0)
                    return false;
                else
                    return false;
            }
        }
        private bool Is(DataSet ds, string _column)
        {
            bool boolFound = false;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr[_column].ToString().ToUpper() == "1")
                {
                    boolFound = true;
                    break;
                }
            }
            return boolFound;
        }
        public int GetRAM(int _answerid)
        {
            int intReturn = 0;
            Int32.TryParse(GetValue(_answerid, "ram"), out intReturn);
            return intReturn;
        }
        public int GetCPU(int _answerid)
        {
            int intReturn = 0;
            Int32.TryParse(GetValue(_answerid, "cores"), out intReturn);
            return intReturn;
        }
        private string GetValue(int _answerid, string _column)
        {
            DataSet ds = GetResponses(_answerid);
            string strResponse = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr[_column].ToString().ToUpper() == "1")
                {
                    strResponse = dr["response"].ToString();
                    break;
                }
            }
            return strResponse;
        }
        public string GetComponents(int _answerid)
        {
            DataSet ds = GetResponses(_answerid);
            string strComponents = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["components"].ToString().Trim() != "")
                {
                    if (strComponents.EndsWith(",") == false)
                        strComponents += ",";
                    strComponents += dr["components"].ToString().Trim();
                }
            }
            return strComponents;
        }
        public void AddResponseAdditional(int _responseid, int _additionalid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            arParams[1] = new SqlParameter("@additionalid", _additionalid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastResponseAdditional", arParams);
        }
        public void DeleteResponseAdditional(int _responseid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastResponseAdditional", arParams);
        }
        public DataSet GetResponseAdditional(int _responseid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastResponseAdditional", arParams);
        }

        public int AddAnswer(int _forecastid, int _platformid, int _hostid, int _userid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@forecastid", _forecastid);
            arParams[1] = new SqlParameter("@platformid", _platformid);
            arParams[2] = new SqlParameter("@hostid", _hostid);
            arParams[3] = new SqlParameter("@userid", _userid);
            arParams[4] = new SqlParameter("@id", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastAnswer", arParams);
            return Int32.Parse(arParams[4].Value.ToString());
        }
        public void AddAnswerUnlock(int _answerid, int _userid, string _reason)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@reason", _reason);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastAnswerUnlock", arParams);
        }
        public void UpdateAnswer(int _id, int _override, int _breakfix, string _change, int _nameid, string _workstation, string _code, string _name, int _addressid, int _classid, int _test, int _environmentid, int _maintenanceid, int _applicationid, int _subapplicationid, int _quantity, int _resiliency)
        {
            arParams = new SqlParameter[17];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@override", _override);
            arParams[2] = new SqlParameter("@breakfix", _breakfix);
            arParams[3] = new SqlParameter("@change", _change);
            arParams[4] = new SqlParameter("@nameid", _nameid);
            arParams[5] = new SqlParameter("@workstation", _workstation);
            arParams[6] = new SqlParameter("@code", _code);
            arParams[7] = new SqlParameter("@name", _name);
            arParams[8] = new SqlParameter("@addressid", _addressid);
            arParams[9] = new SqlParameter("@classid", _classid);
            arParams[10] = new SqlParameter("@test", _test);
            arParams[11] = new SqlParameter("@environmentid", _environmentid);
            arParams[12] = new SqlParameter("@maintenanceid", _maintenanceid);
            arParams[13] = new SqlParameter("@applicationid", _applicationid);
            arParams[14] = new SqlParameter("@subapplicationid", _subapplicationid);
            arParams[15] = new SqlParameter("@quantity", _quantity);
            arParams[16] = new SqlParameter("@resiliency", _resiliency);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerFirst", arParams);
        }
        public void UpdateAnswer(int _id, DateTime _implementation, int _confidenceid, int _userid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@implementation", _implementation);
            arParams[2] = new SqlParameter("@confidenceid", _confidenceid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerLast", arParams);
            AddAnswerConfidence(_id, _implementation, _confidenceid, _userid);
        }
        public void AddAnswerConfidence(int _answerid, DateTime _implementation, int _confidenceid, int _userid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@implementation", _implementation);
            arParams[2] = new SqlParameter("@confidenceid", _confidenceid);
            arParams[3] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastAnswerConfidence", arParams);
        }
        public void UpdateAnswerExecution(int _id, string _execution)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@execution", (_execution == "" ? SqlDateTime.Null : DateTime.Parse(_execution)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerExecution", arParams);
        }
        public void UpdateAnswerChange(int _id, string _change)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@change", _change);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerChange", arParams);
        }
        public void UpdateAnswerStorageOverride(int _id, int _storage_override, int _storage_overrideby)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@storage_override", _storage_override);
            arParams[2] = new SqlParameter("@storage_overrideby", _storage_overrideby);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerStorageOverride", arParams);
        }
        public string GetResiliencyAlert(int _answerid)
        {
            string strReturn = "";
            if (GetAnswer(_answerid, "resiliency") == "1")
                strReturn = "<div style=\"border:solid 2px #990000; padding:5px; width:425px\"><img src=\"/images/siren.gif\" align=\"absmiddle\" border=\"0\"/>&nbsp;This design is part of the <b>Business Infrastructure Recovery</b> Effort</div>";
            return strReturn;
        }

        public void DeleteStorageForOverride(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteStorageForOverride", arParams);
        }
        public void UpdateAnswerOverride(int _id, int _override, int _overrideby)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@override", _override);
            arParams[2] = new SqlParameter("@overrideby", _overrideby);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerOverride", arParams);
        }
        public void UpdateAnswerProduction(int _id, DateTime _production)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@production", _production);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerProduction", arParams);
        }
        public void UpdateAnswerImplementation(int _id, DateTime _implementation)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@implementation", _implementation);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerImplementation", arParams);
        }
        public void UpdateAnswerApproval(int _id, int _override, int _overrideby, string _comments)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@override", _override);
            arParams[2] = new SqlParameter("@overrideby", _overrideby);
            arParams[3] = new SqlParameter("@comments", _comments);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerApproval", arParams);
        }
        public void UpdateAnswerConfidence(int _id, int _confidenceid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@confidenceid", _confidenceid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerConfidence", arParams);
        }
        public void UpdateAnswerAvamar(int _id, int _avamar)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@avamar", _avamar);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerAvamar", arParams);
        }
        public void UpdateAnswerLocation(int _id, int _addressid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@addressid", _addressid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerLocation", arParams);
        }
        public void UpdateAnswer(int _id, string _appname, string _appcode, int _mnemonicid, int _costcenterid, int _dr_criticality, int _appcontact, int _admin1, int _admin2, int _appowner, int _networkengineer, int _costid, int _poolid)
        {
            arParams = new SqlParameter[13];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@appname", _appname);
            arParams[2] = new SqlParameter("@appcode", _appcode);
            arParams[3] = new SqlParameter("@mnemonicid", _mnemonicid);
            arParams[4] = new SqlParameter("@costcenterid", _costcenterid);
            arParams[5] = new SqlParameter("@dr_criticality", _dr_criticality);
            arParams[6] = new SqlParameter("@appcontact", _appcontact);
            arParams[7] = new SqlParameter("@admin1", _admin1);
            arParams[8] = new SqlParameter("@admin2", _admin2);
            arParams[9] = new SqlParameter("@appowner", _appowner);
            arParams[10] = new SqlParameter("@networkengineer", _networkengineer);
            arParams[11] = new SqlParameter("@costid", _costid);
            arParams[12] = new SqlParameter("@poolid", _poolid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerApp", arParams);
        }
        public void UpdateAnswer(int _id, int _requestid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@requestid", _requestid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerRequest", arParams);
        }
        public void UpdateAnswerService(int _id, int _serviceid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerService", arParams);
        }
        public void UpdateAnswerRecovery(int _id, int _recovery_number)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@recovery_number", _recovery_number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerRecovery", arParams);
        }
        public void UpdateAnswerHA(int _id, int _ha)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@ha", _ha);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerHA", arParams);
        }
        
        public void UpdateAnswerModel(int _id, int _modelid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@modelid", _modelid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerModel", arParams);
        }
        public void UpdateAnswerVendor(int _id, int _vendorid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@vendorid", _vendorid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerVendor", arParams);
        }
        public void UpdateAnswerStep(int _id, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerStep", arParams);
        }
        public void UpdateAnswerStep(int _id, int _step, int _done)
        {
            DataSet ds = GetAnswer(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intStep = Int32.Parse(ds.Tables[0].Rows[0]["step"].ToString());
                if (_done > -1)
                    AddStepDone(_id, intStep, _done);
                intStep = intStep + _step;
                UpdateAnswerStep(_id, intStep);
            }
        }
        public void UpdateAnswerCompleted(int _id)
        {
            bool boolComplete = true;
            Servers oServer = new Servers(0, dsn);
            DataSet dsServers = oServer.GetAnswer(_id);
            foreach (DataRow drServer in dsServers.Tables[0].Rows)
            {
                if (drServer["step"].ToString() != "999")
                {
                    boolComplete = false;
                    break;
                }
            }
            Workstations oWorkstation = new Workstations(0, dsn);
            DataSet dsWorkstations = oWorkstation.GetAnswer(_id);
            foreach (DataRow drWorkstation in dsWorkstations.Tables[0].Rows)
            {
                if (drWorkstation["step"].ToString() != "999")
                {
                    boolComplete = false;
                    break;
                }
            }
            DataSet dsWorkstationVirtuals = oWorkstation.GetVirtualAnswer(_id);
            foreach (DataRow drWorkstationVirtual in dsWorkstationVirtuals.Tables[0].Rows)
            {
                if (drWorkstationVirtual["step"].ToString() != "999")
                {
                    boolComplete = false;
                    break;
                }
            }
            if (boolComplete == true)
            {
                if (GetAnswer(_id, "completed") == "")
                    UpdateAnswerCompleted(_id, DateTime.Now.ToString());
                int intForecast = Int32.Parse(GetAnswer(_id, "forecastid"));
                // Check to see if all designs are completed, and if so, change active flag to 0 to remove from listing
                bool boolActive = false;
                DataSet dsAnswers = GetAnswers(intForecast);
                foreach (DataRow drAnswer in dsAnswers.Tables[0].Rows)
                {
                    if (drAnswer["completed"].ToString() == "")
                    {
                        boolActive = true;
                        break;
                    }
                }
                if (boolActive == false)
                    Update(intForecast, 0);
            }
        }
        public void UpdateAnswerSetComplete(int _id)
        {
            Servers oServer = new Servers(0, dsn);
            DataSet dsServers = oServer.GetAnswer(_id);
            foreach (DataRow drServer in dsServers.Tables[0].Rows)
                oServer.UpdateStep(Int32.Parse(drServer["id"].ToString()), 999);
            Workstations oWorkstation = new Workstations(0, dsn);
            DataSet dsWorkstations = oWorkstation.GetAnswer(_id);
            foreach (DataRow drWorkstation in dsWorkstations.Tables[0].Rows)
                oWorkstation.UpdateStep(Int32.Parse(drWorkstation["id"].ToString()), 999);
            DataSet dsWorkstationVirtuals = oWorkstation.GetVirtualAnswer(_id);
            foreach (DataRow drWorkstationVirtual in dsWorkstationVirtuals.Tables[0].Rows)
                oWorkstation.UpdateStep(Int32.Parse(drWorkstationVirtual["id"].ToString()), 999);
            UpdateAnswerCompleted(_id, DateTime.Now.ToString());
        }
        public void UpdateAnswerCompleted(int _id, string _completed)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@completed", (_completed == "" ? SqlDateTime.Null : DateTime.Parse(_completed)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerCompleted", arParams);
        }
        public void UpdateAnswerValidated(int _id, string _validated)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@validated", (_validated == "" ? SqlDateTime.Null : DateTime.Parse(_validated)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerValidated", arParams);
        }
        public void UpdateAnswerExecuted(int _id, string _executed, int _executed_by)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@executed", (_executed == "" ? SqlDateTime.Null : DateTime.Parse(_executed)));
            arParams[2] = new SqlParameter("@executed_by", _executed_by);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerExecuted", arParams);
        }
        public void UpdateAnswerFinished(int _id, string _finished)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@finished", (_finished == "" ? SqlDateTime.Null : DateTime.Parse(_finished)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerFinished", arParams);
        }
        public int AddAnswerVendor(int _platformid, int _typeid, string _make, string _modelname, string _size_w, string _size_h, string _amp, string _description)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            arParams[1] = new SqlParameter("@typeid", _typeid);
            arParams[2] = new SqlParameter("@make", _make);
            arParams[3] = new SqlParameter("@modelname", _modelname);
            arParams[4] = new SqlParameter("@size_w", _size_w);
            arParams[5] = new SqlParameter("@size_h", _size_h);
            arParams[6] = new SqlParameter("@amp", _amp);
            arParams[7] = new SqlParameter("@description", _description);
            arParams[8] = new SqlParameter("@id", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastAnswerVendor", arParams);
            return Int32.Parse(arParams[8].Value.ToString());
        }
        public void UpdateAnswerVendor(int _id, int _platformid, int _typeid, string _make, string _modelname, string _size_w, string _size_h, string _amp, string _description)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@platformid", _platformid);
            arParams[2] = new SqlParameter("@typeid", _typeid);
            arParams[3] = new SqlParameter("@make", _make);
            arParams[4] = new SqlParameter("@modelname", _modelname);
            arParams[5] = new SqlParameter("@size_w", _size_w);
            arParams[6] = new SqlParameter("@size_h", _size_h);
            arParams[7] = new SqlParameter("@amp", _amp);
            arParams[8] = new SqlParameter("@description", _description);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerVendorDetail", arParams);
        }
        public void DeleteAnswer(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastAnswer", arParams);
        }
        public DataSet GetAnswer(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswer", arParams);
        }
        public DataSet GetAnswerWorkstation(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswerWorkstation", arParams);
        }
        public string GetAnswerWorkstation(int _id, string _column)
        {
            DataSet ds = GetAnswerWorkstation(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetAnswerRequest(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswerRequest", arParams);
        }
        public DataSet GetAnswerService(int _serviceid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswerService", arParams);
        }
        public string GetAnswer(int _id, string _column)
        {
            DataSet ds = GetAnswer(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public int GetModel(int _answerid)
        {
            int intModel = 0;
            int intHost = 0;
            int intVendor = 0;
            Int32.TryParse(GetAnswer(_answerid, "vendorid"), out intVendor);
            if (intVendor > 0)
            {
                Settings oSetting = new Settings(0, dsn);
                return Int32.Parse(oSetting.Get("vendor_model_id"));
            }
            else
            {
                if (GetAnswer(_answerid, "hostid") != "")
                    intHost = Int32.Parse(GetAnswer(_answerid, "hostid"));
                if (intHost == 0)
                {
                    if (GetAnswer(_answerid, "modelid") != "")
                        intModel = Int32.Parse(GetAnswer(_answerid, "modelid"));
                    if (intModel == 0)
                    {
                        arParams = new SqlParameter[1];
                        arParams[0] = new SqlParameter("@answerid", _answerid);
                        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastModel", arParams);
                        if (ds.Tables[0].Rows.Count > 0)
                            return Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
                        else
                            return GetModelAsset(_answerid);
                    }
                    else
                        return intModel;
                }
                else
                {
                    Host oHost = new Host(user, dsn);
                    if (oHost.Get(intHost, "modelid") != "")
                        return Int32.Parse(oHost.Get(intHost, "modelid"));
                    else
                        return 0;
                }
            }
        }
        public int GetModelAsset(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastModelAsset", arParams);
            int intModel = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                Int32.TryParse(ds.Tables[0].Rows[0]["modelid"].ToString(), out intModel);
                return intModel;
            }
            else
                return 0;
        }
        
        public DataSet GetAnswers(int _forecastid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@forecastid", _forecastid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswers", arParams);
        }
        /*
        public DataSet GetAnswersAccenture(int _forecastid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@forecastid", _forecastid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswersAccenture", arParams);
        }
        */
        public DataSet GetAnswersOverride()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswersOverride");
        }
        public DataSet GetAnswersPlatform(int _platformid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswersPlatform", arParams);
        }
        public DataSet GetAnswersDay(int _platformid, int _userid, int _days)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@days", _days);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswersDay", arParams);
        }
        public DataSet GetAnswersModel(int _platformid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswersModel", arParams);
        }
        public void AddAnswerPlatform(int _answerid, int _questionid, int _responseid, string _custom)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@questionid", _questionid);
            arParams[2] = new SqlParameter("@responseid", _responseid);
            arParams[3] = new SqlParameter("@custom", _custom);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastAnswerPlatform", arParams);
        }
        public void DeleteAnswerPlatform(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastAnswerPlatforms", arParams);
        }
        public void DeleteAnswerPlatform(int _answerid, int _questionid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@questionid", _questionid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastAnswerPlatform", arParams);
        }
        public DataSet GetAnswerPlatform(int _answerid, int _questionid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@questionid", _questionid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswerPlatform", arParams);
        }
        public string GetAnswerPlatformResponse(int _answerid, int _questionid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@questionid", _questionid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswerPlatformResponse", arParams);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0]["response"].ToString();
            else
                return "";
        }
        public bool GetAnswerPlatform(int _answerid, int _questionid, int _responseid)
        {
            DataSet ds = GetAnswerPlatform(_answerid, _questionid);
            bool boolSelected = false;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["responseid"].ToString() == _responseid.ToString())
                {
                    boolSelected = true;
                    break;
                }
            }
            return boolSelected;
        }
        public string GetAnswerPlatformCustom(int _answerid, int _questionid, int _responseid)
        {
            DataSet ds = GetAnswerPlatform(_answerid, _questionid);
            string strAnswer = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["responseid"].ToString() == _responseid.ToString())
                {
                    strAnswer = dr["custom"].ToString();
                    break;
                }
            }
            return strAnswer;
        }

        public DataSet GetCustom(int _responseid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@responseid", _responseid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastCustom", arParams);
        }
        public DataSet GetCustoms(int _responseid)
        {
            int _questionid = Int32.Parse(GetResponse(_responseid, "questionid"));
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastCustoms", arParams);
        }

        public void AddQuestionPlatform(int _questionid, int _platformid, int _classid, int _environmentid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@platformid", _platformid);
            arParams[2] = new SqlParameter("@classid", _classid);
            arParams[3] = new SqlParameter("@environmentid", _environmentid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastQuestionPlatform", arParams);
        }
        public void DeleteQuestionPlatform(int _questionid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastQuestionPlatform", arParams);
        }
        public DataSet GetQuestionPlatformByQuestion(int _questionid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastQuestionPlatformByQuestion", arParams);
        }
        public DataSet GetQuestionPlatform(int _platformid, int _classid, int _environmentid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastQuestionPlatform", arParams);
        }

        public void AddAffects(int _questionid, int _affectedid, int _state)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@affectedid", _affectedid);
            arParams[2] = new SqlParameter("@state", _state);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastAffects", arParams);
        }
        public void UpdateAffects(int _id, int _state)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@state", _state);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAffects", arParams);
        }
        public void DeleteAffects(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastAffects", arParams);
        }
        public DataSet GetAffectsByQuestion(int _questionid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAffectByQuestion", arParams);
        }
        public DataSet GetAffectsByQuestionAll(int _questionid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAffectByQuestionAll", arParams);
        }
        public DataSet GetAffectsByAffected(int _affectedid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@affectedid", _affectedid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAffectByAffected", arParams);
        }
        public DataSet GetAffects(int _questionid, int _affectedid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@affectedid", _affectedid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAffects", arParams);
        }
        public string GetAffects(int _questionid, int _affectedid, int _responseid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@affectedid", _affectedid);
            arParams[2] = new SqlParameter("@responseid", _responseid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getForecastAffectsResponse", arParams);
            if (o == null)
                return "";
            else
                return o.ToString();
        }

        public void AddAffected(int _questionid, int _affectedid, int _responseid, int _state)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@affectedid", _affectedid);
            arParams[2] = new SqlParameter("@responseid", _responseid);
            arParams[3] = new SqlParameter("@state", _state);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastAffected", arParams);
        }
        public void UpdateAffected(int _id, int _state)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@state", _state);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAffected", arParams);
        }
        public void DeleteAffected(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastAffected", arParams);
        }
        public DataSet GetAffected(int _questionid, int _affectedid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@affectedid", _affectedid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAffecteds", arParams);
        }
        public DataSet GetAffected(int _questionid, int _affectedid, int _responseid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@affectedid", _affectedid);
            arParams[2] = new SqlParameter("@responseid", _responseid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAffected", arParams);
        }

        public void AddLineItem(string _name, int _display, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@display", _display);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastLineItem", arParams);
        }
        public void UpdateLineItem(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastLineItem", arParams);
        }
        public void UpdateLineItemOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastLineItemOrder", arParams);
        }
        public void EnableLineItem(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastLineItemEnabled", arParams);
        }
        public void DeleteLineItem(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastLineItem", arParams);
        }
        public string GetLineItem(int _id, string _column)
        {
            DataSet ds = GetQuestion(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetLineItem(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastLineItem", arParams);
        }
        public DataSet GetLineItems(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastLineItems", arParams);
        }

        public void AddAcquisition(int _modelid, int _lineitemid, double _cost, int _prod, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            arParams[1] = new SqlParameter("@lineitemid", _lineitemid);
            arParams[2] = new SqlParameter("@cost", _cost);
            arParams[3] = new SqlParameter("@prod", _prod);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastAcquisition", arParams);
        }
        public void UpdateAcquisition(int _id, int _lineitemid, double _cost, int _prod, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@lineitemid", _lineitemid);
            arParams[2] = new SqlParameter("@cost", _cost);
            arParams[3] = new SqlParameter("@prod", _prod);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAcquisition", arParams);
        }
        public void DeleteAcquisition(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastAcquisition", arParams);
        }
        public string GetAcquisition(int _id, string _column)
        {
            DataSet ds = GetQuestion(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetAcquisition(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAcquisition", arParams);
        }
        public DataSet GetAcquisitions(int _modelid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAcquisitions", arParams);
        }

        public void AddOperation(int _modelid, int _lineitemid, double _cost, int _prod, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            arParams[1] = new SqlParameter("@lineitemid", _lineitemid);
            arParams[2] = new SqlParameter("@cost", _cost);
            arParams[3] = new SqlParameter("@prod", _prod);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastOperation", arParams);
        }
        public void UpdateOperation(int _id, int _lineitemid, double _cost, int _prod, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@lineitemid", _lineitemid);
            arParams[2] = new SqlParameter("@cost", _cost);
            arParams[3] = new SqlParameter("@prod", _prod);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastOperation", arParams);
        }
        public void DeleteOperation(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastOperation", arParams);
        }
        public string GetOperation(int _id, string _column)
        {
            DataSet ds = GetQuestion(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetOperation(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastOperation", arParams);
        }
        public DataSet GetOperations(int _modelid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastOperations", arParams);
        }

        public void AddStreetValue(int _modelid, int _lineitemid, double _cost, int _prod, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            arParams[1] = new SqlParameter("@lineitemid", _lineitemid);
            arParams[2] = new SqlParameter("@cost", _cost);
            arParams[3] = new SqlParameter("@prod", _prod);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastStreetValue", arParams);
        }
        public void UpdateStreetValue(int _id, int _lineitemid, double _cost, int _prod, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@lineitemid", _lineitemid);
            arParams[2] = new SqlParameter("@cost", _cost);
            arParams[3] = new SqlParameter("@prod", _prod);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastStreetValue", arParams);
        }
        public void DeleteStreetValue(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastStreetValue", arParams);
        }
        public string GetStreetValue(int _id, string _column)
        {
            DataSet ds = GetQuestion(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetStreetValue(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastStreetValue", arParams);
        }
        public DataSet GetStreetValues(int _modelid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastStreetValues", arParams);
        }

        public void AddStorage(int _answerid, int _high, double _high_total, double _high_qa, double _high_test, double _high_replicated, string _high_level, double _high_ha, int _standard, double _standard_total, double _standard_qa, double _standard_test, double _standard_replicated, string _standard_level, double _standard_ha, int _low, double _low_total, double _low_qa, double _low_test, double _low_replicated, string _low_level, double _low_ha)
        {
            arParams = new SqlParameter[22];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@high", _high);
            arParams[2] = new SqlParameter("@high_total", _high_total);
            arParams[3] = new SqlParameter("@high_qa", _high_qa);
            arParams[4] = new SqlParameter("@high_test", _high_test);
            arParams[5] = new SqlParameter("@high_replicated", _high_replicated);
            arParams[6] = new SqlParameter("@high_level", _high_level);
            arParams[7] = new SqlParameter("@high_ha", _high_ha);
            arParams[8] = new SqlParameter("@standard", _standard);
            arParams[9] = new SqlParameter("@standard_total", _standard_total);
            arParams[10] = new SqlParameter("@standard_qa", _standard_qa);
            arParams[11] = new SqlParameter("@standard_test", _standard_test);
            arParams[12] = new SqlParameter("@standard_replicated", _standard_replicated);
            arParams[13] = new SqlParameter("@standard_level", _standard_level);
            arParams[14] = new SqlParameter("@standard_ha", _standard_ha);
            arParams[15] = new SqlParameter("@low", _low);
            arParams[16] = new SqlParameter("@low_total", _low_total);
            arParams[17] = new SqlParameter("@low_qa", _low_qa);
            arParams[18] = new SqlParameter("@low_test", _low_test);
            arParams[19] = new SqlParameter("@low_replicated", _low_replicated);
            arParams[20] = new SqlParameter("@low_level", _low_level);
            arParams[21] = new SqlParameter("@low_ha", _low_ha);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastStorage", arParams);
        }
        public void UpdateStorage(int _answerid, int _storage)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@storage", _storage);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastStorage", arParams);
        }
        public void DeleteStorage(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastStorage", arParams);
        }
        public DataSet GetStorage(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastStorage", arParams);
        }

        public void AddStorageOS(int _answerid, int _high, double _high_total, double _high_qa, double _high_test, double _high_replicated, string _high_level, double _high_ha, int _standard, double _standard_total, double _standard_qa, double _standard_test, double _standard_replicated, string _standard_level, double _standard_ha, int _low, double _low_total, double _low_qa, double _low_test, double _low_replicated, string _low_level, double _low_ha)
        {
            arParams = new SqlParameter[22];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@high", _high);
            arParams[2] = new SqlParameter("@high_total", _high_total);
            arParams[3] = new SqlParameter("@high_qa", _high_qa);
            arParams[4] = new SqlParameter("@high_test", _high_test);
            arParams[5] = new SqlParameter("@high_replicated", _high_replicated);
            arParams[6] = new SqlParameter("@high_level", _high_level);
            arParams[7] = new SqlParameter("@high_ha", _high_ha);
            arParams[8] = new SqlParameter("@standard", _standard);
            arParams[9] = new SqlParameter("@standard_total", _standard_total);
            arParams[10] = new SqlParameter("@standard_qa", _standard_qa);
            arParams[11] = new SqlParameter("@standard_test", _standard_test);
            arParams[12] = new SqlParameter("@standard_replicated", _standard_replicated);
            arParams[13] = new SqlParameter("@standard_level", _standard_level);
            arParams[14] = new SqlParameter("@standard_ha", _standard_ha);
            arParams[15] = new SqlParameter("@low", _low);
            arParams[16] = new SqlParameter("@low_total", _low_total);
            arParams[17] = new SqlParameter("@low_qa", _low_qa);
            arParams[18] = new SqlParameter("@low_test", _low_test);
            arParams[19] = new SqlParameter("@low_replicated", _low_replicated);
            arParams[20] = new SqlParameter("@low_level", _low_level);
            arParams[21] = new SqlParameter("@low_ha", _low_ha);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastStorageOS", arParams);
        }
        public void UpdateStorageOS(int _answerid, int _storage)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@storage", _storage);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastStorageOS", arParams);
        }
        public void DeleteStorageOS(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastStorageOS", arParams);
        }
        public DataSet GetStorageOS(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastStorageOS", arParams);
        }

        public void AddBackup(int _answerid, int _daily, int _weekly, string _weekly_day, int _monthly, string _monthly_day, string _monthly_days, int _time, string _time_hour, string _time_switch, string _start_date, int _recoveryid, string _cf_percent, string _cf_compression, string _cf_average, string _cf_backup, string _cf_archive, string _cf_window, string _cf_sets, string _cd_type, string _cd_percent, string _cd_compression, string _cd_versions, string _cd_window, string _cd_growth, double _average_one, string _documentation)
        {
            arParams = new SqlParameter[27];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@daily", _daily);
            arParams[2] = new SqlParameter("@weekly", _weekly);
            arParams[3] = new SqlParameter("@weekly_day", _weekly_day);
            arParams[4] = new SqlParameter("@monthly", _monthly);
            arParams[5] = new SqlParameter("@monthly_day", _monthly_day);
            arParams[6] = new SqlParameter("@monthly_days", _monthly_days);
            arParams[7] = new SqlParameter("@time", _time);
            arParams[8] = new SqlParameter("@time_hour", _time_hour);
            arParams[9] = new SqlParameter("@time_switch", _time_switch);
            arParams[10] = new SqlParameter("@start_date", _start_date);
            arParams[11] = new SqlParameter("@recoveryid", _recoveryid);
            arParams[12] = new SqlParameter("@cf_percent", _cf_percent);
            arParams[13] = new SqlParameter("@cf_compression", _cf_compression);
            arParams[14] = new SqlParameter("@cf_average", _cf_average);
            arParams[15] = new SqlParameter("@cf_backup", _cf_backup);
            arParams[16] = new SqlParameter("@cf_archive", _cf_archive);
            arParams[17] = new SqlParameter("@cf_window", _cf_window);
            arParams[18] = new SqlParameter("@cf_sets", _cf_sets);
            arParams[19] = new SqlParameter("@cd_type", _cd_type);
            arParams[20] = new SqlParameter("@cd_percent", _cd_percent);
            arParams[21] = new SqlParameter("@cd_compression", _cd_compression);
            arParams[22] = new SqlParameter("@cd_versions", _cd_versions);
            arParams[23] = new SqlParameter("@cd_window", _cd_window);
            arParams[24] = new SqlParameter("@cd_growth", _cd_growth);
            arParams[25] = new SqlParameter("@average_one", _average_one);
            arParams[26] = new SqlParameter("@documentation", _documentation);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastBackup", arParams);
        }
        public void UpdateBackup(int _answerid, int _backup)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@backup", _backup);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastBackup", arParams);
        }
        public void DeleteBackup(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastBackup", arParams);
        }
        public DataSet GetBackup(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastBackup", arParams);
        }
        public DataSet GetBackup(DateTime _day)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@day", _day);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastBackupScheduleDate", arParams);
        }
        public void AddBackupExclusion(int _answerid, string _path)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@path", _path);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastBackupExclusion", arParams);
        }
        public void DeleteBackupExclusion(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastBackupExclusion", arParams);
        }
        public DataSet GetBackupExclusions(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastBackupExclusions", arParams);
        }
        public void AddBackupInclusion(int _answerid, string _path)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@path", _path);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastBackupInclusion", arParams);
        }
        public void DeleteBackupInclusion(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastBackupInclusion", arParams);
        }
        public DataSet GetBackupInclusions(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastBackupInclusions", arParams);
        }
        public void AddBackupRetention(int _answerid, string _path, DateTime _first, int _number, string _type, string _hour, string _switch, string _occurence, string _occurs)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@path", _path);
            arParams[2] = new SqlParameter("@first", _first);
            arParams[3] = new SqlParameter("@number", _number);
            arParams[4] = new SqlParameter("@type", _type);
            arParams[5] = new SqlParameter("@hour", _hour);
            arParams[6] = new SqlParameter("@switch", _switch);
            arParams[7] = new SqlParameter("@occurence", _occurence);
            arParams[8] = new SqlParameter("@occurs", _occurs);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastBackupRetention", arParams);
        }
        public void DeleteBackupRetention(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastBackupRetention", arParams);
        }
        public DataSet GetBackupRetentions(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastBackupRetentions", arParams);
        }
        

        public void AddStep(int _platformid, string _name, string _subtitle, string _path, string _override_path, string _image_path, int _additional, int _step, int _enabled)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@subtitle", _subtitle);
            arParams[3] = new SqlParameter("@path", _path);
            arParams[4] = new SqlParameter("@override_path", _override_path);
            arParams[5] = new SqlParameter("@image_path", _image_path);
            arParams[6] = new SqlParameter("@additional", _additional);
            arParams[7] = new SqlParameter("@step", _step);
            arParams[8] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastStep", arParams);
        }
        public void UpdateStep(int _id, string _name, string _subtitle, string _path, string _override_path, string _image_path, int _additional, int _enabled)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@subtitle", _subtitle);
            arParams[3] = new SqlParameter("@path", _path);
            arParams[4] = new SqlParameter("@override_path", _override_path);
            arParams[5] = new SqlParameter("@image_path", _image_path);
            arParams[6] = new SqlParameter("@additional", _additional);
            arParams[7] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastStep", arParams);
        }
        public void UpdateStepOrder(int _id, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastStepOrder", arParams);
        }
        public void EnableStep(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastStepEnabled", arParams);
        }
        public void DeleteStep(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastStep", arParams);
        }
        public DataSet GetStep(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastStep", arParams);
        }
        public DataSet GetSteps(int _platformid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastSteps", arParams);
        }
        public string GetStep(int _id, string _column)
        {
            DataSet ds = GetStep(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        public void AddStepAdditional(string _name, string _path, int _display, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@path", _path);
            arParams[2] = new SqlParameter("@display", _display);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastStepAdditional", arParams);
        }
        public void UpdateStepAdditional(int _id, string _name, string _path, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@path", _path);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastStepAdditional", arParams);
        }
        public void UpdateStepAdditionalOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastStepAdditionalOrder", arParams);
        }
        public void EnableStepAdditional(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastStepAdditionalEnabled", arParams);
        }
        public void DeleteStepAdditional(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastStepAdditional", arParams);
        }
        public DataSet GetStepAdditional(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastStepAdditional", arParams);
        }
        public DataSet GetStepAdditionals(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastStepAdditionals", arParams);
        }
        public string GetStepAdditional(int _id, string _column)
        {
            DataSet ds = GetStepAdditional(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        public void AddStepAdditionalDone(int _answerid, int _additionalid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@additionalid", _additionalid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastStepAdditionalDone", arParams);
        }
        public DataSet GetStepAdditionalsDone(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastStepAdditionalsDone", arParams);
        }
        public bool GetStepAdditionalsDone(int _answerid, int _additionalid)
        {
            DataSet ds = GetStepAdditionalsDone(_answerid);
            bool boolFound = false;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["additionalid"].ToString()) == _additionalid)
                {
                    boolFound = true;
                    break;
                }
            }
            return boolFound;
        }
        public void DeleteStepAdditionalDone(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastStepAdditionalDone", arParams);
        }



        public string AddStepDone(int _answerid, int _step, int _done)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@done", _done);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastStepDone", arParams);
            string strAlert = "";
            if (_done == 1)
            {
                // return a string stating the pages that have been reset.
                int intPlatform = Int32.Parse(GetAnswer(_answerid, "platformid"));
                DataSet dsReset = GetStepReset(intPlatform, _step);
                foreach (DataRow dr in dsReset.Tables[0].Rows)
                {
                    AddStepDone(_answerid, Int32.Parse(dr["reset"].ToString()), 0);
                    strAlert += "\\n - Step " + dr["reset"].ToString() + ": " + dr["name"].ToString();
                }
            }
            if (strAlert != "")
                strAlert = "alert('The following steps were reset since they are dependant on answers you may have just changed.\\nPlease validate the following steps...\\n" + strAlert + "\\n\\nTo validate, click on the step and click \"Update\" to approve your information');";
            return strAlert;
        }
        public DataSet GetStepsDone(int _answerid, int _done)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@done", _done);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastStepsDone", arParams);
        }


        public int GetRequestID(int _answerid, bool _search_reset)
        {
            int intReturn = 0;
            string strRequest = GetAnswer(_answerid, "requestid");
            if (strRequest == "")
                intReturn = 0;
            else
                intReturn = Int32.Parse(strRequest);
            if (intReturn == 0 && _search_reset == true)
                intReturn = GetReset(_answerid);
            return intReturn;
        }
        public int TotalServerCount(int _answerid, bool _use_csm)
        {
            int intCount = 0;
            int intRequest = GetRequestID(_answerid, true);
            if (IsHACluster(_answerid) == true)
            {
                Cluster oCluster = new Cluster(user, dsn);
                DataSet ds = oCluster.Gets(intRequest);
                foreach (DataRow dr in ds.Tables[0].Rows)
                    intCount += Int32.Parse(dr["nodes"].ToString());
            }
            else if (IsHACSM(_answerid) == true && _use_csm == true)
            {
                CSMConfig oCSMConfig = new CSMConfig(user, dsn);
                DataSet ds = oCSMConfig.Gets(intRequest);
                foreach (DataRow dr in ds.Tables[0].Rows)
                    intCount += Int32.Parse(dr["servers"].ToString());
            }
            else
            {
                Servers oServer = new Servers(0, dsn);
                DataSet ds = oServer.GetAnswer(_answerid);
                intCount = ds.Tables[0].Rows.Count;
            }
            return intCount;
        }
        public int TotalDRCount(int _answerid, bool _use_csm)
        {
            int intCount = 0;
            int intRequest = GetRequestID(_answerid, true);
            if (IsHACluster(_answerid) == true)
            {
                Cluster oCluster = new Cluster(user, dsn);
                DataSet ds = oCluster.Gets(intRequest);
                foreach (DataRow dr in ds.Tables[0].Rows)
                    intCount += Int32.Parse(dr["dr"].ToString());
            }
            else if (IsHACSM(_answerid) == true && _use_csm == true)
            {
                CSMConfig oCSMConfig = new CSMConfig(user, dsn);
                DataSet ds = oCSMConfig.Gets(intRequest);
                foreach (DataRow dr in ds.Tables[0].Rows)
                    intCount += Int32.Parse(dr["dr"].ToString());
            }
            else
            {
                Servers oServer = new Servers(0, dsn);
                DataSet ds = oServer.GetAnswer(_answerid);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["dr"].ToString() == "1")
                        intCount++;
                }
            }
            return intCount;
        }
        public int TotalHACount(int _answerid, bool _use_csm)
        {
            int intCount = 0;
            int intRequest = GetRequestID(_answerid, true);
            if (IsHACluster(_answerid) == true)
            {
                Cluster oCluster = new Cluster(user, dsn);
                DataSet ds = oCluster.Gets(intRequest);
                foreach (DataRow dr in ds.Tables[0].Rows)
                    intCount += Int32.Parse(dr["ha"].ToString());
            }
            else if (IsHACSM(_answerid) == true && _use_csm == true)
            {
                CSMConfig oCSMConfig = new CSMConfig(user, dsn);
                DataSet ds = oCSMConfig.Gets(intRequest);
                foreach (DataRow dr in ds.Tables[0].Rows)
                    intCount += Int32.Parse(dr["ha"].ToString());
            }
            else
            {
                Servers oServer = new Servers(0, dsn);
                DataSet ds = oServer.GetAnswer(_answerid);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["ha"].ToString() == "1")
                        intCount++;
                }
            }
            return intCount;
        }
        public int TotalWorkstationCount(int _answerid)
        {
            int intCount = 0;
            Workstations oWorkstation = new Workstations(0, dsn);
            DataSet ds = oWorkstation.GetAnswer(_answerid);
            intCount = ds.Tables[0].Rows.Count;
            return intCount;
        }
        public bool IsStorage(int _answerid)
        {
            bool boolStorage = false;
            Design oDesign = new Design(user, dsn);
            DataSet dsDesign = oDesign.GetAnswer(_answerid);
            if (dsDesign.Tables[0].Rows.Count > 0)
                boolStorage = (dsDesign.Tables[0].Rows[0]["storage"].ToString() == "1");
            else
            {
                DataSet dsStorage = GetStorage(_answerid);
                if (dsStorage.Tables[0].Rows.Count > 0 && (dsStorage.Tables[0].Rows[0]["high"].ToString() == "1" || dsStorage.Tables[0].Rows[0]["standard"].ToString() == "1" || dsStorage.Tables[0].Rows[0]["low"].ToString() == "1"))
                    boolStorage = true;
            }
            return boolStorage;
        }
        public DataSet GetStorageSummary(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswerStorage", arParams);
        }
        public double GetStorageForecastedTotal(int _answerid, int intStoragePerBladeApp, string strFilter, string _additional_filter, bool _prod, bool _qa, bool _test, bool _replicated, bool _ha)
        {
            double dblTotal = 0.00;
            DataTable _table = GetStorageForecasted(_answerid, intStoragePerBladeApp, strFilter, _additional_filter);
            if (_prod == true)
            {
                dblTotal += double.Parse(_table.Rows[0]["high_total"].ToString());
                dblTotal += double.Parse(_table.Rows[0]["standard_total"].ToString());
                dblTotal += double.Parse(_table.Rows[0]["low_total"].ToString());
            }
            if (_qa == true)
            {
                dblTotal += double.Parse(_table.Rows[0]["high_qa"].ToString());
                dblTotal += double.Parse(_table.Rows[0]["standard_qa"].ToString());
                dblTotal += double.Parse(_table.Rows[0]["low_qa"].ToString());
            }
            if (_test == true)
            {
                dblTotal += double.Parse(_table.Rows[0]["high_test"].ToString());
                dblTotal += double.Parse(_table.Rows[0]["standard_test"].ToString());
                dblTotal += double.Parse(_table.Rows[0]["low_test"].ToString());
            }
            if (_replicated == true)
            {
                dblTotal += double.Parse(_table.Rows[0]["high_replicated"].ToString());
                dblTotal += double.Parse(_table.Rows[0]["standard_replicated"].ToString());
                dblTotal += double.Parse(_table.Rows[0]["low_replicated"].ToString());
            }
            if (_ha == true)
            {
                dblTotal += double.Parse(_table.Rows[0]["high_ha"].ToString());
                dblTotal += double.Parse(_table.Rows[0]["standard_ha"].ToString());
                dblTotal += double.Parse(_table.Rows[0]["low_ha"].ToString());
            }
            return dblTotal;
        }
        public DataTable GetStorageForecasted(int _answerid, int intStoragePerBladeApp, string strFilter, string _additional_filter)
        {
            DataTable myDataTable = new DataTable();
            AddColumn("high_total", "System.Int32", myDataTable);
            AddColumn("standard_total", "System.Int32", myDataTable);
            AddColumn("low_total", "System.Int32", myDataTable);
            AddColumn("ports_total", "System.Int32", myDataTable);
            AddColumn("high_qa", "System.Int32", myDataTable);
            AddColumn("standard_qa", "System.Int32", myDataTable);
            AddColumn("low_qa", "System.Int32", myDataTable);
            AddColumn("ports_qa", "System.Int32", myDataTable);
            AddColumn("high_test", "System.Int32", myDataTable);
            AddColumn("standard_test", "System.Int32", myDataTable);
            AddColumn("low_test", "System.Int32", myDataTable);
            AddColumn("ports_test", "System.Int32", myDataTable);
            AddColumn("high_replicated", "System.Int32", myDataTable);
            AddColumn("standard_replicated", "System.Int32", myDataTable);
            AddColumn("low_replicated", "System.Int32", myDataTable);
            AddColumn("high_ha", "System.Int32", myDataTable);
            AddColumn("standard_ha", "System.Int32", myDataTable);
            AddColumn("low_ha", "System.Int32", myDataTable);
            DataRow myDataRow;
            int intHighProd = 0;
            int intStandardProd = 0;
            int intLowProd = 0;
            int intPortsProd = 0;
            int intHighQA = 0;
            int intStandardQA = 0;
            int intLowQA = 0;
            int intPortsQA = 0;
            int intHighTest = 0;
            int intStandardTest = 0;
            int intLowTest = 0;
            int intPortsTest = 0;
            int intHighRep = 0;
            int intStandardRep = 0;
            int intLowRep = 0;
            int intHighHA = 0;
            int intStandardHA = 0;
            int intLowHA = 0;
            DataSet ds = GetStorageSummary(_answerid);
            if (strFilter.StartsWith(" AND ") == true)
                strFilter = strFilter.Substring(5);
            if (strFilter == "" && _additional_filter.StartsWith(" AND ") == true)
                _additional_filter = _additional_filter.Substring(5);
            DataRow[] drReturn = ds.Tables[0].Select(strFilter + _additional_filter);
            foreach (DataRow dr in drReturn)
            {
                int intHP = 0;
                int intSP = 0;
                int intLP = 0;
                int intHQ = 0;
                int intSQ = 0;
                int intLQ = 0;
                int intHT = 0;
                int intST = 0;
                int intLT = 0;
                int intAllocated = (intStoragePerBladeApp * Int32.Parse(dr["quantity"].ToString()));
                // Production
                if (dr["high_total"].ToString() != "")
                    intHP += Int32.Parse(dr["high_total"].ToString());
                if (dr["high_total_os"].ToString() != "")
                    intHP += Int32.Parse(dr["high_total_os"].ToString());
                if (dr["blade"].ToString() == "1" && dr["standard_total"].ToString() != "")
                {
                    int intStandardProdBlade = Int32.Parse(dr["standard_total"].ToString());
                    if (intStandardProdBlade > intAllocated)
                        intSP += (intStandardProdBlade - intAllocated);
                }
                else if (dr["standard_total"].ToString() != "")
                    intSP += Int32.Parse(dr["standard_total"].ToString());
                if (dr["blade"].ToString() != "1")
                {
                    if (dr["standard_total_os"].ToString() != "")
                        intSP += Int32.Parse(dr["standard_total_os"].ToString());
                }
                if (dr["low_total"].ToString() != "")
                    intLP += Int32.Parse(dr["low_total"].ToString());
                if (dr["low_total_os"].ToString() != "")
                    intLP += Int32.Parse(dr["low_total_os"].ToString());
                // QA
                if (dr["high_qa"].ToString() != "")
                    intHQ += Int32.Parse(dr["high_qa"].ToString());
                if (dr["high_total_os"].ToString() != "")
                    intHQ += Int32.Parse(dr["high_qa_os"].ToString());
                if (dr["blade"].ToString() == "1" && dr["standard_qa"].ToString() != "")
                {
                    int intStandardQABlade = Int32.Parse(dr["standard_qa"].ToString());
                    if (intStandardQABlade > intAllocated)
                        intSQ += (intStandardQABlade - intAllocated);
                }
                else if (dr["standard_qa"].ToString() != "")
                    intSQ += Int32.Parse(dr["standard_qa"].ToString());
                if (dr["blade"].ToString() != "1")
                {
                    if (dr["standard_qa_os"].ToString() != "")
                        intSQ += Int32.Parse(dr["standard_qa_os"].ToString());
                }
                if (dr["low_qa"].ToString() != "")
                    intLQ += Int32.Parse(dr["low_qa"].ToString());
                if (dr["low_qa_os"].ToString() != "")
                    intLQ += Int32.Parse(dr["low_qa_os"].ToString());
                // Test
                if (dr["high_test"].ToString() != "")
                    intHT += Int32.Parse(dr["high_test"].ToString());
                if (dr["high_test_os"].ToString() != "")
                    intHT += Int32.Parse(dr["high_test_os"].ToString());
                if (dr["blade"].ToString() == "1" && dr["standard_test"].ToString() != "")
                {
                    int intStandardTestBlade = Int32.Parse(dr["standard_test"].ToString());
                    if (intStandardTestBlade > intAllocated)
                        intST += (intStandardTestBlade - intAllocated);
                }
                else if (dr["standard_test"].ToString() != "")
                        intST += Int32.Parse(dr["standard_test"].ToString());
                if (dr["blade"].ToString() != "1")
                {
                    if (dr["standard_test_os"].ToString() != "")
                        intST += Int32.Parse(dr["standard_test_os"].ToString());
                }
                if (dr["low_test"].ToString() != "")
                    intLT += Int32.Parse(dr["low_test"].ToString());
                if (dr["low_test_os"].ToString() != "")
                    intLT += Int32.Parse(dr["low_test_os"].ToString());
                // Replication
                if (dr["high_replicated"].ToString() != "")
                    intHighRep += Int32.Parse(dr["high_replicated"].ToString());
                if (dr["high_replicated_os"].ToString() != "")
                    intHighRep += Int32.Parse(dr["high_replicated_os"].ToString());
                if (dr["standard_replicated"].ToString() != "")
                    intStandardRep += Int32.Parse(dr["standard_replicated"].ToString());
                if (dr["standard_replicated_os"].ToString() != "")
                    intStandardRep += Int32.Parse(dr["standard_replicated_os"].ToString());
                if (dr["low_replicated"].ToString() != "")
                    intLowRep += Int32.Parse(dr["low_replicated"].ToString());
                if (dr["low_replicated_os"].ToString() != "")
                    intLowRep += Int32.Parse(dr["low_replicated_os"].ToString());
                // High Availability
                if (dr["high_ha"].ToString() != "")
                    intHighHA += Int32.Parse(dr["high_ha"].ToString());
                if (dr["high_ha_os"].ToString() != "")
                    intHighHA += Int32.Parse(dr["high_ha_os"].ToString());
                if (dr["standard_ha"].ToString() != "")
                    intStandardHA += Int32.Parse(dr["standard_ha"].ToString());
                if (dr["standard_ha_os"].ToString() != "")
                    intStandardHA += Int32.Parse(dr["standard_ha_os"].ToString());
                if (dr["low_ha"].ToString() != "")
                    intLowHA += Int32.Parse(dr["low_ha"].ToString());
                if (dr["low_ha_os"].ToString() != "")
                    intLowHA += Int32.Parse(dr["low_ha_os"].ToString());
                // Match up
                intHighProd += intHP;
                intStandardProd += intSP;
                intLowProd += intLP;
                if ((intHP + intSP + intLP) > 0)
                    intPortsProd += Int32.Parse(dr["ports"].ToString());
                intHighQA += intHQ;
                intStandardQA += intSQ;
                intLowQA += intLQ;
                if ((intHQ + intSQ + intLQ) > 0)
                    intPortsQA += Int32.Parse(dr["ports"].ToString());
                intHighTest += intHT;
                intStandardTest += intST;
                intLowTest += intLT;
                if ((intHT + intST + intLT) > 0)
                    intPortsTest += Int32.Parse(dr["ports"].ToString());
            }
            myDataRow = myDataTable.NewRow();
            myDataRow["high_total"] = intHighProd;
            myDataRow["standard_total"] = intStandardProd;
            myDataRow["low_total"] = intLowProd;
            myDataRow["ports_total"] = intPortsProd;
            myDataRow["high_qa"] = intHighQA;
            myDataRow["standard_qa"] = intStandardQA;
            myDataRow["low_qa"] = intLowQA;
            myDataRow["ports_qa"] = intPortsQA;
            myDataRow["high_test"] = intHighTest;
            myDataRow["standard_test"] = intStandardTest;
            myDataRow["low_test"] = intLowTest;
            myDataRow["ports_test"] = intPortsTest;
            myDataRow["high_replicated"] = intHighRep;
            myDataRow["standard_replicated"] = intStandardRep;
            myDataRow["low_replicated"] = intLowRep;
            myDataRow["high_ha"] = intHighRep;
            myDataRow["standard_ha"] = intStandardRep;
            myDataRow["low_ha"] = intLowRep;
            myDataTable.Rows.Add(myDataRow);
            return myDataTable;
        }
        private void AddColumn(string _name, string _type, DataTable _table)
        {
            DataColumn myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType(_type);
            myDataColumn.ColumnName = _name;
            _table.Columns.Add(myDataColumn);
        }
        public void Unlock(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerUnlock", arParams);
        }

        public void AddStepReset(int _platformid, int _step, int _reset)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@reset", _reset);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastStepReset", arParams);
        }
        public void DeleteStepReset(int _platformid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastStepReset", arParams);
        }
        public DataSet GetStepReset(int _platformid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            arParams[1] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastStepReset", arParams);
        }
        public DataSet GetAnswersUpcomingStorage()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswersUpcomingStorage");
        }

        public void AddReset(int _answerid, int _requestid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@requestid", _requestid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastAnswerReset", arParams);
        }
        public void DeleteReset(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastAnswerReset", arParams);
        }
        public int GetReset(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getForecastAnswerReset", arParams);
            if (o == null || o.ToString() == "")
                return 0;
            else
                return Int32.Parse(o.ToString());
        }

        public void AddWorkstation(int _answerid, int _ram, int _os, int _recovery, int _internal, int _hdd, int _cpuid)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@ram", _ram);
            arParams[2] = new SqlParameter("@os", _os);
            arParams[3] = new SqlParameter("@recovery", _recovery);
            arParams[4] = new SqlParameter("@internal", _internal);
            arParams[5] = new SqlParameter("@hdd", _hdd);
            arParams[6] = new SqlParameter("@cpuid", _cpuid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastWorkstation", arParams);
        }
        public void DeleteWorkstation(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteForecastWorkstation", arParams);
        }
        public DataSet GetWorkstation(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastWorkstation", arParams);
        }
        public string GetWorkstation(int _answerid, string _column)
        {
            DataSet ds = GetWorkstation(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }


        public bool GetPlatformMidrangeForecast(int _forecastid)
        {
            DataSet dsAnswers = GetAnswers(_forecastid);
            bool boolMidrange = false;
            foreach (DataRow drAnswer in dsAnswers.Tables[0].Rows)
            {
                boolMidrange = GetPlatformMidrange(Int32.Parse(drAnswer["id"].ToString()));
                if (boolMidrange == true)
                    break;
            }
            return boolMidrange;
        }
        public bool GetPlatformMidrange(int _answerid)
        {
            if (IsOSMidrange(_answerid) == true)
                return true;
            else
            {
                bool boolMidrange = false;
                int intType = 0;
                Int32.TryParse(GetAnswer(_answerid, "typeid"), out intType);
                if (intType > 0)
                {
                    Types oType = new Types(0, dsn);
                    if (oType.Get(intType, "name").ToUpper() == "MIDRANGE")
                        boolMidrange = true;
                }
                else
                {
                    DataSet dsAnswers = GetResponses(_answerid);
                    foreach (DataRow dr in dsAnswers.Tables[0].Rows)
                    {
                        string strResponse = dr["response"].ToString().ToUpper();
                        if (strResponse.Contains("AIX") || strResponse.Contains("SOLARIS") || strResponse.Contains("RHEL") || strResponse.Contains("NOVELL"))
                        {
                            boolMidrange = true;
                            break;
                        }
                    }
                }
                return boolMidrange;
            }
        }
        public bool GetPlatformDistributedForecast(int _forecastid, int _workstation_platform)
        {
            DataSet dsAnswers = GetAnswers(_forecastid);
            bool boolDistributed = false;
            foreach (DataRow drAnswer in dsAnswers.Tables[0].Rows)
            {
                boolDistributed = GetPlatformDistributed(Int32.Parse(drAnswer["id"].ToString()), _workstation_platform);
                if (boolDistributed == true)
                    break;
            }
            return boolDistributed;
        }
        public bool GetPlatformDistributed(int _answerid, int _workstation_platform)
        {
            if (Int32.Parse(GetAnswer(_answerid, "platformid")) == _workstation_platform)
                return true;
            else if (IsOSDistributed(_answerid) == true)
                return true;
            else
            {
                bool boolDistributed = false;
                int intType = 0;
                Int32.TryParse(GetAnswer(_answerid, "typeid"), out intType);
                if (intType > 0)
                {
                    Types oType = new Types(0, dsn);
                    if (oType.Get(intType, "name").ToUpper() == "DISTRIBUTED")
                        boolDistributed = true;
                }
                else
                {
                    DataSet dsAnswers = GetResponses(_answerid);
                    foreach (DataRow dr in dsAnswers.Tables[0].Rows)
                    {
                        string strResponse = dr["response"].ToString().ToUpper();
                        if (strResponse.Contains("WINDOWS"))
                        {
                            boolDistributed = true;
                            break;
                        }
                    }
                }
                return boolDistributed;
            }
        }
        public string GetAnswerBody(int _id, int _environment, string _dsn_asset, string _dsn_ip)
        {
            Requests oRequest = new Requests(user, dsn);
            Projects oProject = new Projects(user, dsn);
            Users oUser = new Users(user, dsn);
            Organizations oOrganization = new Organizations(user, dsn);
            Segment oSegment = new Segment(user, dsn);
            Variables oVariable = new Variables(_environment);
            StatusLevels oStatusLevel = new StatusLevels();
            Platforms oPlatform = new Platforms(0, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            Confidence oConfidence = new Confidence(0, dsn);
            Classes oClass = new Classes(0, dsn);
            Environments oEnvironment = new Environments(0, dsn);
            Locations oLocation = new Locations(0, dsn);
            Servers oServer = new Servers(0, dsn);
            Asset oAsset = new Asset(0, _dsn_asset);
            IPAddresses oIPAddress = new IPAddresses(0, _dsn_ip, dsn);

            StringBuilder sbBody = new StringBuilder();
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
            
            int intForecast = 0;
            Int32.TryParse(GetAnswer(_id, "forecastid"), out intForecast);
            if (intForecast > 0)
            {
                int intRequest = 0;
                Int32.TryParse(Get(intForecast, "requestid"), out intRequest);
                int intProject = oRequest.GetProjectNumber(intRequest);
                if (intProject > 0)
                {
                    DataSet dsProject = oProject.Get(intProject);
                    if (dsProject.Tables[0].Rows.Count > 0)
                    {
                        sbBody.Append("<tr><td nowrap><b>Design ID:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(_id.ToString());
                        sbBody.Append("</td></tr>");
                        sbBody.Append(strSpacerRow);
                        sbBody.Append("<tr><td nowrap><b>Project Name:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(dsProject.Tables[0].Rows[0]["name"].ToString());
                        sbBody.Append("</td></tr>");
                        sbBody.Append(strSpacerRow);
                        if (dsProject.Tables[0].Rows[0]["number"].ToString() != "")
                        {
                            sbBody.Append("<tr><td nowrap><b>Project Number:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                            sbBody.Append(dsProject.Tables[0].Rows[0]["number"].ToString());
                            sbBody.Append("</td></tr>");
                            sbBody.Append(strSpacerRow);
                        }
                        sbBody.Append("<tr><td nowrap><b>Initiative Type:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(dsProject.Tables[0].Rows[0]["bd"].ToString());
                        sbBody.Append("</td></tr>");
                        sbBody.Append(strSpacerRow);
                        sbBody.Append("<tr><td nowrap><b>Organization:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(oOrganization.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["organization"].ToString())));
                        sbBody.Append("</td></tr>");
                        sbBody.Append(strSpacerRow);
                        string strSegment = oSegment.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["segmentid"].ToString()));
                        if (strSegment != "")
                        {
                            sbBody.Append("<tr><td nowrap><b>Segment:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                            sbBody.Append(oSegment.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["segmentid"].ToString())));
                            sbBody.Append("</td></tr>");
                            sbBody.Append(strSpacerRow);
                        }
                        sbBody.Append("<tr><td nowrap><b>Status:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(oStatusLevel.HTML(Int32.Parse(dsProject.Tables[0].Rows[0]["status"].ToString())));
                        sbBody.Append("</td></tr>");
                        sbBody.Append(strSpacerRow);
                    }
                }
            }
            if (_id > 0)
            {
                DataSet dsAnswer = GetAnswer(_id);
                if (dsAnswer.Tables[0].Rows.Count > 0)
                {
                    StringBuilder sbServers = new StringBuilder();
                    DataSet dsServers = oServer.GetAnswer(_id);
                    foreach (DataRow drServer in dsServers.Tables[0].Rows)
                    {
                        int intServer = Int32.Parse(drServer["id"].ToString());
                        if (drServer["servername"].ToString() != "")
                        {
                            sbServers.Append(drServer["servername"].ToString());
                            sbServers.Append(" (");
                            sbServers.Append(oServer.GetIPs(intServer, 0, 1, 0, 0, _dsn_ip, "", ""));
                            sbServers.Append(")");
                            sbServers.Append("<br/>");
                        }
                    }
                    if (sbServers.ToString() != "")
                    {
                        sbBody.Append("<tr><td nowrap><b>Servers:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(sbServers.ToString());
                        sbBody.Append("</td></tr>");
                        sbBody.Append(strSpacerRow);
                    }
                    sbBody.Append("<tr><td nowrap><b>Platform:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(oPlatform.GetName(Int32.Parse(dsAnswer.Tables[0].Rows[0]["platformid"].ToString())));
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Nickname:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(dsAnswer.Tables[0].Rows[0]["name"].ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    int intModel = GetModelAsset(_id);
                    if (intModel == 0)
                        intModel = GetModel(_id);
                    sbBody.Append("<tr><td nowrap><b>Model:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(oModelsProperties.Get(intModel, "name"));
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Commitment Date:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(DateTime.Parse(dsAnswer.Tables[0].Rows[0]["implementation"].ToString()).ToLongDateString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Quantity:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(dsAnswer.Tables[0].Rows[0]["quantity"].ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Confidence:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(oConfidence.Get(Int32.Parse(dsAnswer.Tables[0].Rows[0]["confidenceid"].ToString()), "name"));
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    int intClass = Int32.Parse(dsAnswer.Tables[0].Rows[0]["classid"].ToString());
                    sbBody.Append("<tr><td nowrap><b>Class:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(oClass.Get(intClass, "name"));
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    if (oClass.IsProd(intClass))
                    {
                        sbBody.Append("<tr><td nowrap><b>Test First?:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(dsAnswer.Tables[0].Rows[0]["test"].ToString() == "1" ? "Yes" : "No");
                        sbBody.Append("</td></tr>");
                        sbBody.Append(strSpacerRow);
                    }
                    sbBody.Append("<tr><td nowrap><b>Environment:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(oEnvironment.Get(Int32.Parse(dsAnswer.Tables[0].Rows[0]["environmentid"].ToString()), "name"));
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Location:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(oLocation.GetFull(Int32.Parse(dsAnswer.Tables[0].Rows[0]["addressid"].ToString())));
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Created By:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(oUser.GetFullName(Int32.Parse(dsAnswer.Tables[0].Rows[0]["userid"].ToString())));
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Created On:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(DateTime.Parse(dsAnswer.Tables[0].Rows[0]["created"].ToString()).ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Updated On:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(DateTime.Parse(dsAnswer.Tables[0].Rows[0]["modified"].ToString()).ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    if (dsAnswer.Tables[0].Rows[0]["executed"].ToString() != "")
                    {
                        sbBody.Append("<tr><td nowrap><b>Executed On:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(DateTime.Parse(dsAnswer.Tables[0].Rows[0]["executed"].ToString()).ToString());
                        sbBody.Append("</td></tr>");
                        sbBody.Append(strSpacerRow);
                    }
                    int intExecutedBy = (dsAnswer.Tables[0].Rows[0]["executed_by"].ToString() == "" ? 0 : Int32.Parse(dsAnswer.Tables[0].Rows[0]["executed_by"].ToString()));
                    if (intExecutedBy > 0)
                    {
                        sbBody.Append("<tr><td nowrap><b>Executed By:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(oUser.GetFullName(intExecutedBy) + " (" + oUser.GetName(intExecutedBy) + ")");
                        sbBody.Append("</td></tr>");
                        sbBody.Append(strSpacerRow);
                    }
                    if (dsAnswer.Tables[0].Rows[0]["completed"].ToString() != "")
                    {
                        sbBody.Append("<tr><td nowrap><b>Built On:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(DateTime.Parse(dsAnswer.Tables[0].Rows[0]["completed"].ToString()).ToString());
                        sbBody.Append("</td></tr>");
                        sbBody.Append(strSpacerRow);
                    }
                    if (dsAnswer.Tables[0].Rows[0]["finished"].ToString() != "")
                    {
                        sbBody.Append("<tr><td nowrap><b>Completed On:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(DateTime.Parse(dsAnswer.Tables[0].Rows[0]["finished"].ToString()).ToString());
                        sbBody.Append("</td></tr>");
                        sbBody.Append(strSpacerRow);
                    }
                }
            }

            if (sbBody.ToString() != "")
            {
                sbBody.Insert(0, "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                sbBody.Append("</table>");
            }
            
            return sbBody.ToString();
        }
        public DataSet GetAnswerIncomplete()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswerIncomplete");
        }
        public DataSet GetAnswerComplete()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswerComplete");
        }
        public void AddAnswerUpdate(int _answerid, int _completed, int _valid, string _comments, int _userid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@completed", _completed);
            arParams[2] = new SqlParameter("@valid", _valid);
            arParams[3] = new SqlParameter("@comments", _comments);
            arParams[4] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastAnswerUpdate", arParams);
        }
        public DataSet GetAnswerUpdate(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswerUpdate", arParams);
        }
        public void AddAnswerUpdateSent(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastAnswerUpdateSent", arParams);
        }
        public DataSet GetAnswerUpdateSent(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswerUpdateSent", arParams);
        }

        public bool NotifyImplementor(int intAnswer, int intModel, int intImplementorDistributed, int intWorkstationPlatform, int intImplementorMidrange, int intEnvironment, int intProfile, string dsnAsset, string dsnIP)
        {
            Users oUser = new Users(0, dsn);
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            Variables oVariable = new Variables(intEnvironment);
            bool boolAssigned = false;
            if (intAnswer > 0 && intModel > 0)
            {
                int intForecast = Int32.Parse(GetAnswer(intAnswer, "forecastid"));
                int intRequest = Int32.Parse(GetAnswer(intAnswer, "requestid"));
                if (intForecast > 0)
                    intRequest = Int32.Parse(Get(intForecast, "requestid"));
                //int intRequest = GetRequestID(intAnswer, true);
                ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                if (oServiceRequest.Get(intRequest).Tables[0].Rows.Count == 0)
                    oServiceRequest.Add(intRequest, 1, 1);
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
                if (CanAutoProvision(intAnswer) == false)
                {
                    if (intForecast > 0)
                    {
                        int intImplementor = 0;
                        Requests oRequest = new Requests(0, dsn);
                        int intProject = oRequest.GetProjectNumber(intRequest);
                        ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                        string strType = "Distributed";
                        DataSet dsResource = oResourceRequest.GetProjectItem(intProject, intImplementorDistributed);
                        if (GetPlatformDistributed(intAnswer, intWorkstationPlatform) == false)
                        {
                            dsResource = oResourceRequest.GetProjectItem(intProject, intImplementorMidrange);
                            strType = "Midrange";
                        }
                        if (dsResource.Tables[0].Rows.Count > 0)
                            intImplementor = (dsResource.Tables[0].Rows[0]["userid"].ToString() == "" ? 0 : Int32.Parse(dsResource.Tables[0].Rows[0]["userid"].ToString()));
                        int intDesign = 0;
                        DataSet dsDesign = oDesign.GetAnswer(intAnswer);
                        if (dsDesign.Tables[0].Rows.Count > 0)
                            Int32.TryParse(dsDesign.Tables[0].Rows[0]["id"].ToString(), out intDesign);
                        int intResourceWorkflow = 0;
                        int intResourceParent = 0;
                        int intUser = 0;
                        if (intImplementor > 0)
                        {
                            bool boolAssign = true;
                            OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
                            if (oOnDemandTasks.GetModel(intAnswer).Tables[0].Rows.Count > 0)
                                boolAssign = false;
                            // Get Task from Pending
                            DataSet dsPending = oOnDemandTasks.GetPending(intAnswer);
                            if (dsPending.Tables[0].Rows.Count > 0)
                            {
                                intResourceWorkflow = Int32.Parse(dsPending.Tables[0].Rows[0]["resourceid"].ToString());
                                intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                                int intNumberExecute = Int32.Parse(oResourceRequest.Get(intResourceParent, "number"));
                                Solution oSolution = new Solution(0, dsn);
                                DataSet dsSolution = oSolution.GetCodeModel(intModel);
                                int intServiceExecute = Int32.Parse(dsSolution.Tables[0].Rows[0]["serviceid"].ToString());
                                Services oService = new Services(0, dsn);
                                int intItemExecute = oService.GetItemId(intServiceExecute);
                                int intQuantityExecute = Int32.Parse(GetAnswer(intAnswer, "quantity"));
                                ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
                                int intType = oModelsProperties.GetType(intModel);
                                intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                                int intServiceExecuteOLD = Int32.Parse(oResourceRequest.Get(intResourceParent, "serviceid"));
                                int intItemExecuteOLD = Int32.Parse(oResourceRequest.Get(intResourceParent, "itemid"));
                                if (boolAssign == false && (intServiceExecuteOLD != intServiceExecute || intItemExecuteOLD != intItemExecute))
                                    boolAssign = true;
                                if (boolAssign == true)
                                {
                                    boolAssigned = true;
                                    oOnDemandTasks.DeleteAll(intAnswer);
                                    oOnDemandTasks.AddGenericII(intRequest, intItemExecute, intNumberExecute, intAnswer, intModel);
                                    oResourceRequest.UpdateAccepted(intResourceParent, 1);
                                    oResourceRequest.UpdateWorkflowAssigned(intResourceWorkflow, intUser);
                                    oResourceRequest.UpdateItemAndService(intResourceParent, intItemExecute, intServiceExecute);
                                    oResourceRequest.UpdateName(intResourceParent, "#" + intAnswer.ToString() + " : " + GetAnswer(intAnswer, "name") + " [" + DateTime.Parse(GetAnswer(intAnswer, "implementation")).ToShortDateString() + "]");
                                    // Add email to assignee
                                    oFunction.SendEmail("Manual Provisioning Task (" + (intDesign > 0 ? intDesign.ToString() : intAnswer.ToString()) + ")", oUser.GetName(intUser), "", strEMailIdsBCC, "Manual Provisioning Task (#" + (intDesign > 0 ? intDesign.ToString() : intAnswer.ToString()) + ")", "<p><b>The following design has been executed. You are required to build this server manually.</b></p><p>" + (intDesign > 0 ? oDesign.GetSummary(intDesign, intEnvironment) : GetAnswerBody(intAnswer, intEnvironment, dsnAsset, dsnIP)) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=" + intResourceWorkflow.ToString() + "\" target=\"_blank\">Click here to view this request.</a></p>", true, false);
                                }
                                else
                                    boolAssigned = false;
                            }
                            else
                            {
                                // Resource was not assigned prior to execution. Create the assignment and refresh page.
                                int intNextNumber = oResourceRequest.GetNumber(intRequest);
                                intResourceParent = oResourceRequest.Add(intRequest, -1, -1, intNextNumber, "Provisioning Task (" + strType + ")", 0, 6.00, 2, 1, 1, 1);
                                intResourceWorkflow = oResourceRequest.AddWorkflow(intResourceParent, 0, "Provisioning Task (" + strType + ")", intImplementor, 0, 6.00, 2, 0);
                                oOnDemandTasks.AddPending(intAnswer, intResourceWorkflow);
                                oResourceRequest.UpdateAssignedBy(intResourceParent, -999);
                                return NotifyImplementor(intAnswer, intModel, intImplementorDistributed, intWorkstationPlatform, intImplementorMidrange, intEnvironment, intProfile, dsnAsset, dsnIP);
                            }
                        }
                        else
                            boolAssigned = false;
                        if (intResourceWorkflow > 0 && boolAssigned == true)
                        {
                            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
                            Projects oProject = new Projects(0, dsn);
                            Pages oPage = new Pages(0, dsn);
                            string strCC = "";
                            Delegates oDelegate = new Delegates(0, dsn);
                            DataSet dsDelegates = oDelegate.Gets(intUser);
                            foreach (DataRow drDelegate in dsDelegates.Tables[0].Rows)
                                strCC += oUser.GetName(Int32.Parse(drDelegate["delegate"].ToString())) + ";";
                            oFunction.SendEmail("Auto-Provisioning Receipt", oUser.GetName(intProfile), "", strEMailIdsBCC, "Auto-Provisioning Receipt", "<p><b>Your implementor (" + oUser.GetFullName(intUser) + ") has been successfully notified of your auto-provisioning request.</b></p><p>" + GetAnswerBody(intAnswer, intEnvironment, dsnAsset, dsnIP) + "</p>", true, false);
                        }
                    }
                    else
                        boolAssigned = true;
                }
                else
                {
                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DESIGN_BUILDER");
                    oFunction.SendEmail("Auto-Provisioning Receipt", oUser.GetName(intProfile), "", strEMailIdsBCC, "Auto-Provisioning Receipt", "<p><b>Your build has been successfully initiated.</b></p><p>" + GetAnswerBody(intAnswer, intEnvironment, dsnAsset, dsnIP) + "</p>", true, false);
                    boolAssigned = true;
                }
            }
            return boolAssigned;

        }
        //public void AddInventoryView(int _platformid, int _userid, string _view)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@platformid", _platformid);
        //    arParams[1] = new SqlParameter("@userid", _userid);
        //    arParams[2] = new SqlParameter("@view", _view);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addInventoryView", arParams);
        //}
        //public void DeleteInventoryView(int _id, int _userid)
        //{
        //    arParams = new SqlParameter[2];
        //    arParams[0] = new SqlParameter("@id", _id);
        //    arParams[1] = new SqlParameter("@userid", _userid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteInventoryView", arParams);
        //}
        //public DataSet GetInventoryView(int _platformid, int _userid)
        //{
        //    arParams = new SqlParameter[2];
        //    arParams[0] = new SqlParameter("@platformid", _platformid);
        //    arParams[1] = new SqlParameter("@userid", _userid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_getInventoryView", arParams);
        //}

        public void EnforceRecovery(int intAnswer, int intDRHourQuestion, int intDRHourResponse, int intDRRecoveryQuestion, int intDRRecoveryResponse, int intQuantity)
        {
            bool boolPermitNoReplication = false;
            ServerName oServerName = new ServerName(0, dsn);

            int intSubApplication = 0;
            if (GetAnswer(intAnswer, "subapplicationid") != "")
                intSubApplication = Int32.Parse(GetAnswer(intAnswer, "subapplicationid"));
            if (intSubApplication > 0)
                boolPermitNoReplication = (oServerName.GetSubApplication(intSubApplication, "permit_no_replication") == "1");

            if (boolPermitNoReplication == false)
            {
                int intApplication = 0;
                if (GetAnswer(intAnswer, "applicationid") != "")
                    intApplication = Int32.Parse(GetAnswer(intAnswer, "applicationid"));
                if (intApplication > 0)
                    boolPermitNoReplication = (oServerName.GetApplication(intApplication, "permit_no_replication") == "1");
            }
            int intModel = GetModel(intAnswer);
            if (intModel > 0)
            {
                ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
                if (boolPermitNoReplication == false && oModelsProperties.IsEnforce1to1Recovery(intModel) == true)
                {
                    // Set Under 48 Hours
                    DeleteAnswerPlatform(intAnswer, intDRHourQuestion);
                    AddAnswerPlatform(intAnswer, intDRHourQuestion, intDRHourResponse, "");
                    // Set One-to-One Recovery
                    DeleteAnswerPlatform(intAnswer, intDRRecoveryQuestion);
                    AddAnswerPlatform(intAnswer, intDRRecoveryQuestion, intDRRecoveryResponse, "");
                    // Add quantity
                    UpdateAnswerRecovery(intAnswer, intQuantity);
                }
                else if (IsDRUnder48(intAnswer, false) == true && oModelsProperties.IsNoManyto1Recovery(intModel) == true && IsDRManyToOne(intAnswer))
                {
                    // Set One-to-One Recovery
                    DeleteAnswerPlatform(intAnswer, intDRRecoveryQuestion);
                    AddAnswerPlatform(intAnswer, intDRRecoveryQuestion, intDRRecoveryResponse, "");
                    // Add quantity
                    UpdateAnswerRecovery(intAnswer, intQuantity);
                }
            }

        }

        public DataSet GetForecast(int id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "TEMP_Forecast", arParams);
        }

        public string CanAutoProvisionReason(int _id)
        {
            // Only three situations can occur....
            //      1) OS = AIX (check "manual_build" flag on OS)
            //      2) Location is not OPS Center / Dalton (check "manual_build" flag on location)
            //      3) "manual_build" flag is checked in ModelsProperty

            string strManual = "";
            // Location from Design
            int intAddress = 0;
            if (Int32.TryParse(GetAnswer(_id, "addressid"), out intAddress))
            {
                Locations oLocation = new Locations(user, dsn);
                if (oLocation.IsManual(intAddress) == true)
                    strManual = "Location &quot;" + oLocation.GetFull(intAddress) + "&quot; is configured for manual build";
                else
                {
                    // OS from Server Records
                    OperatingSystems oOperatingSystem = new OperatingSystems(user, dsn);
                    ModelsProperties oModelsProperties = new ModelsProperties(user, dsn);
                    Servers oServer = new Servers(user, dsn);
                    DataSet dsServers = oServer.GetAnswer(_id);
                    if (dsServers.Tables[0].Rows.Count == 0)
                    {
                        // Probably hasn't executed...get the model from the selection matrix
                        int intModel = GetModelAsset(_id);
                        if (intModel == 0)
                            intModel = GetModel(_id);
                        if (oModelsProperties.Get(intModel, "manual_build") == "1")
                            strManual = "Model &quot;" + oModelsProperties.Get(intModel, "name") + "&quot; is configured for manual build";
                    }
                    else
                    {
                        foreach (DataRow drServer in dsServers.Tables[0].Rows)
                        {
                            int intOS = 0;
                            if (Int32.TryParse(drServer["osid"].ToString(), out intOS))
                            {
                                if (oOperatingSystem.Get(intOS, "manual_build") == "1")
                                {
                                    strManual = "Operating System &quot;" + oOperatingSystem.Get(intOS, "name") + "&quot; is configured for manual build";
                                    break;
                                }
                                else
                                {
                                    // Check ModelsProperty from Server Records
                                    int intModel = 0;
                                    if (Int32.TryParse(drServer["modelid"].ToString(), out intModel))
                                    {
                                        if (oModelsProperties.Get(intModel, "manual_build") == "1")
                                        {
                                            strManual = "Model &quot;" + oModelsProperties.Get(intModel, "name") + "&quot; is configured for manual build";
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        strManual = "Model ID &quot;" + drServer["modelid"].ToString() + "&quot; is not found";
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                strManual = "Operating System ID &quot;" + drServer["osid"].ToString() + "&quot; is not found";
                                break;
                            }
                        }
                    }
                }
            }
            else
                strManual = "Location ID &quot;" + GetAnswer(_id, "addressid") + "&quot; is not found";
            return strManual;
        }
        public bool CanAutoProvision(int _id)
        {
            return (CanAutoProvisionReason(_id) == "");
        }

        #region forecast response category

        public DataSet GetForecastResposeCategory(int? _id, string _name, int? _enabled)
        {
            arParams = new SqlParameter[5];
            if(_id!=null)
                arParams[0] = new SqlParameter("@Id", _id);
            if (_name != "")
                arParams[1] = new SqlParameter("@Name", _name);
            if (_id != null)
                arParams[2] = new SqlParameter("@Enabled", _enabled);

            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastResponseCategory", arParams);
        }

        public void AddForecastResposeCategory(string _Name, int _enabled, int _userid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@Name", _Name);
            arParams[1] = new SqlParameter("@Enabled", _enabled);
            arParams[2] = new SqlParameter("@CreatedBy", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastResposeCategory", arParams);
        }

        public void UpdateForecastResposeCategory(int _id, string _Name, int _enabled, int _userid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@Id", _id);
            arParams[1] = new SqlParameter("@Name", _Name);
            arParams[2] = new SqlParameter("@Enabled", _enabled);
            arParams[3] = new SqlParameter("@ModifiedBy", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastResposeCategory", arParams);
        }

        public void DeleteForecastResposeCategory(int _id,int _userid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@Id", _id);
            arParams[1] = new SqlParameter("@ModifiedBy", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastResposeCategoryDeleted", arParams);
        }

        public void EnableForecastResposeCategory(int _id, int _enabled, int _userid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@Id", _id);
            arParams[1] = new SqlParameter("@Enabled", _enabled);
            arParams[2] = new SqlParameter("@ModifiedBy", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastResposeCategoryEnabled", arParams);
        }
        #endregion



        #region Errors
        public enum ForecastAnswerErrorStep : int
        {
            Clustering = 99991,
        }
        public int AddError(int _requestid, int _itemid, int _number, int _answerid, int _step, string _reason)
        {
            Errors oError = new Errors(user, dsn);
            oError.CheckError(GetErrorLatest(_answerid, _step));

            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@answerid", _answerid);
            arParams[4] = new SqlParameter("@step", _step);
            arParams[5] = new SqlParameter("@reason", _reason);
            arParams[6] = new SqlParameter("@id", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addForecastAnswerError", arParams);
            return Int32.Parse(arParams[6].Value.ToString());
        }

        public DataSet GetErrorLatest(int _answerid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswerErrorLatest", arParams);
        }
        public DataSet GetError(int _answerid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswerError", arParams);
        }
        public DataSet GetErrors(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswerErrors", arParams);
        }

        public DataSet GetErrorByRequest(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@RequestId", _requestid);
            arParams[1] = new SqlParameter("@ItemId", _itemid);
            arParams[2] = new SqlParameter("@Number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getForecastAnswerErrorsByRequest", arParams);
        }

        public string GetErrorDetailsBody(int _requestid, int _itemid, int _number, int _environment)
        {
            Variables oVariable = new Variables(_environment);
            Functions oFunction = new Functions(0, dsn, _environment);
            StringBuilder sbBody = new StringBuilder();
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";

            string strSpacerTD = "<td style=\"width:10;text-align:left\"></td>";
            string strTRstart = "<tr>";
            string strTRend = "</tr>";
            string strTDstart = "<td>";
            string strTDend = "</td>";


            DataSet dsError = GetErrorByRequest(_requestid, _itemid, _number);
            if (dsError.Tables[0].Rows.Count > 0)
            {
                DataRow drError = dsError.Tables[0].Rows[0];
                int intAnswer = Int32.Parse(drError["answerid"].ToString());

                sbBody.Append(strTRstart + strTDstart + "<b>Error Message:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["reason"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Design ID:</b>" + strTDend + strSpacerTD);
                DataSet dsDesign = oDesign.GetAnswer(intAnswer);
                if (dsDesign.Tables[0].Rows.Count > 0)
                    sbBody.Append(strTDstart + "<a href=\"/datapoint/service/design.aspx?id=" + oFunction.encryptQueryString(intAnswer.ToString()) + "\" target=\"_blank\">" + dsDesign.Tables[0].Rows[0]["id"].ToString() + "</a>" + strTDend + strTRend);
                else
                    sbBody.Append(strTDstart + "<a href=\"/datapoint/service/design.aspx?id=" + oFunction.encryptQueryString(intAnswer.ToString()) + "\" target=\"_blank\">" + intAnswer.ToString() + "</a>" + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Model:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["model"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Class:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["class"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Environment:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["environment"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Executed:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["executed"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Executed By:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["ExecutedByName"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strSpacerRow);
            }

            if (sbBody.ToString() != "")
            {
                sbBody.Insert(0, "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                sbBody.Append("</table>");
            }
            return sbBody.ToString();


        }

        public void UpdateError(int _answerid, int _step, int _errorid, int _userid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@errorid", _errorid);
            arParams[3] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerError", arParams);
        }

        public void UpdateError(int _id, string _incident, int _assigned)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@incident", _incident);
            arParams[2] = new SqlParameter("@assigned", _assigned);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateForecastAnswerErrorIncident", arParams);
        }

        #endregion
    }
}
