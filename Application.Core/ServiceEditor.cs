using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;
using System.Text;
using System.Web;
using System.IO;

namespace NCC.ClearView.Application.Core
{
	public class ServiceEditor
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        private string strRequired = "";
        public ServiceEditor(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}

        public DataSet APGetNames()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_AP_getNames");
        }
        public DataSet APGetIPs()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_AP_getIPs");
        }
        public DataSet APGetSerials()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_AP_getSerials");
        }


        public DataSet GetFields(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getFields", arParams);
        }
        public DataSet GetField(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getField", arParams);
        }
        public string GetField(int _id, string _column)
        {
            DataSet ds = GetField(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddField(string _name, string _type, string _code, string _description, string _image, int _length, int _width, int _display, int _enabled)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@type", _type);
            arParams[2] = new SqlParameter("@code", _code);
            arParams[3] = new SqlParameter("@description", _description);
            arParams[4] = new SqlParameter("@image", _image);
            arParams[5] = new SqlParameter("@length", _length);
            arParams[6] = new SqlParameter("@width", _width);
            arParams[7] = new SqlParameter("@display", _display);
            arParams[8] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_addField", arParams);
        }
        public void UpdateField(int _id, string _name, string _type, string _code, string _description, string _image, int _length, int _width, int _enabled)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@code", _code);
            arParams[4] = new SqlParameter("@description", _description);
            arParams[5] = new SqlParameter("@image", _image);
            arParams[6] = new SqlParameter("@length", _length);
            arParams[7] = new SqlParameter("@width", _width);
            arParams[8] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_updateField", arParams);
        }
        public void UpdateFieldOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_updateFieldOrder", arParams);
        }
        public void EnableField(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_updateFieldEnabled", arParams);
        }
        public void DeleteField(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_deleteField", arParams);
        }

        public int AddConfig(int _serviceid, int _fieldid, int _wm, string _question, int _length, int _width, int _height, int _checked, int _direction, int _multiple, string _tip, int _required, string _required_text, string _url, string _other_text, int _display, int _enabled)
        {
            int intDB = AddFieldDB();
            arParams = new SqlParameter[19];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@fieldid", _fieldid);
            arParams[2] = new SqlParameter("@wm", _wm);
            arParams[3] = new SqlParameter("@question", _question);
            arParams[4] = new SqlParameter("@dbfield", intDB);
            arParams[5] = new SqlParameter("@length", _length);
            arParams[6] = new SqlParameter("@width", _width);
            arParams[7] = new SqlParameter("@height", _height);
            arParams[8] = new SqlParameter("@checked", _checked);
            arParams[9] = new SqlParameter("@direction", _direction);
            arParams[10] = new SqlParameter("@multiple", _multiple);
            arParams[11] = new SqlParameter("@tip", _tip);
            arParams[12] = new SqlParameter("@required", _required);
            arParams[13] = new SqlParameter("@required_text", _required_text);
            arParams[14] = new SqlParameter("@url", _url);
            arParams[15] = new SqlParameter("@other_text", _other_text);
            arParams[16] = new SqlParameter("@display", _display);
            arParams[17] = new SqlParameter("@enabled", _enabled);
            arParams[18] = new SqlParameter("@id", SqlDbType.Int);
            arParams[18].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_addConfig", arParams);
            return Int32.Parse(arParams[18].Value.ToString());
        }
        public void UpdateConfig(int _id, int _wm, string _question, int _length, int _width, int _height, int _checked, int _direction, int _multiple, string _tip, int _required, string _required_text, string _url, string _other_text, int _enabled)
        {
            arParams = new SqlParameter[15];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@wm", _wm);
            arParams[2] = new SqlParameter("@question", _question);
            arParams[3] = new SqlParameter("@length", _length);
            arParams[4] = new SqlParameter("@width", _width);
            arParams[5] = new SqlParameter("@height", _height);
            arParams[6] = new SqlParameter("@checked", _checked);
            arParams[7] = new SqlParameter("@direction", _direction);
            arParams[8] = new SqlParameter("@multiple", _multiple);
            arParams[9] = new SqlParameter("@tip", _tip);
            arParams[10] = new SqlParameter("@required", _required);
            arParams[11] = new SqlParameter("@required_text", _required_text);
            arParams[12] = new SqlParameter("@url", _url);
            arParams[13] = new SqlParameter("@other_text", _other_text);
            arParams[14] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_updateConfig", arParams);
        }
        public void UpdateConfigOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_updateConfigOrder", arParams);
        }
        public void DeleteConfig(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_deleteConfig", arParams);
        }
        public void DeleteConfigs(int _serviceid, int _wm)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@wm", _wm);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_deleteConfigs", arParams);
        }
        //public DataSet GetConfigs(int _serviceid, int _enabled)
        //{
        //    arParams = new SqlParameter[2];
        //    arParams[0] = new SqlParameter("@serviceid", _serviceid);
        //    arParams[1] = new SqlParameter("@enabled", _enabled);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getConfigs", arParams);
        //}
        public DataSet GetConfigs(int _serviceid, int _wm, int _enabled)
        {
            return GetConfigs(0, _serviceid, 0, _wm, _enabled);
        }
        public DataSet GetConfigs(int _requestid, int _serviceid, int _number, int _wm, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@wm", _wm);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getConfigsWM", arParams);
        }
        public DataSet GetConfigs(int _serviceid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getConfigs", arParams);
        }
        public DataSet GetConfig(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getConfig", arParams);
        }
        public string GetConfig(int _id, string _column)
        {
            DataSet ds = GetConfig(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddConfigValue(int _configid, string _value, int _display)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@configid", _configid);
            arParams[1] = new SqlParameter("@value", _value);
            arParams[2] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_addConfigValue", arParams);
        }
        public void UpdateConfigValue(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_updateConfigValueOrder", arParams);
        }
        public DataSet GetConfigValues(int _configid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@configid", _configid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getConfigValues", arParams);
        }
        public DataSet GetConfigValue(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getConfigValue", arParams);
        }
        public string GetConfigValue(int _id, string _column)
        {
            DataSet ds = GetConfigValue(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void DeleteConfigValues(int _configid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@configid", _configid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_deleteConfigValues", arParams);
        }
        public void DeleteConfigValue(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_deleteConfigValue", arParams);
        }
        public int AddFieldDB()
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_addFieldDB", arParams);
            return Int32.Parse(arParams[0].Value.ToString());
        }

        public string LoadForm(int _serviceid, bool _wm, bool _edit, string _hidden, int _environment, DataSet ds, string _dsn)
        {
            return LoadForm(0, _serviceid, 0, _wm, _edit, _hidden, _environment, ds, _dsn);
        }
        public string LoadForm(int _requestid, int _serviceid, int _number, bool _wm, bool _edit, string _hidden, int _environment, DataSet ds, string _dsn)
        {
            string strForm = "";
            int intRow = 0;
            Variables oVariable = new Variables(_environment);
            Functions oFunction = new Functions(user, _dsn, _environment);
            Services oService = new Services(user, _dsn);
            Servers oServer = new Servers(user, _dsn);
            Users oUser = new Users(user, _dsn);
            Locations oLocation = new Locations(user, _dsn);
            Mnemonic oMnemonic = new Mnemonic(user, _dsn);

            DataSet dsForm = GetConfigs(_requestid, _serviceid, _number, (_wm == true ? 1 : 0), (_edit == true ? 0 : 1));
            foreach (DataRow drForm in dsForm.Tables[0].Rows)
            {
                intRow++;
                string strRow = "";
                int intConfig = Int32.Parse(drForm["id"].ToString());
                DataSet dsValues = GetConfigValues(intConfig);
                int intCount = 0;
                string strContents = "";
                int intValue = 0;
                string strValue = "";
                string[] strValues;
                bool boolEditable = (drForm["editable"].ToString() == "1");
                string strDefaultText = "";
                DataSet dsWorkflow = oService.GetWorkflowsReceive(_serviceid);
                DataSet dsWorkflowSent = oService.GetWorkflows(_serviceid);
                int intServicePrevious = 0;
                bool boolInherited = (drForm["inherited"].ToString() == "1");
                if (boolInherited)
                {
                    strDefaultText = "** Inherited from the previous workflow **";
                    Int32.TryParse(dsWorkflow.Tables[0].Rows[0]["serviceid"].ToString(), out intServicePrevious);
                }
                string strReturnValue = "";
                char[] strSplit = { ',' };
                string strCode = drForm["code"].ToString().ToUpper();
                string strField = drForm["dbfield"].ToString() + "_" + (_wm ? "1" : "0") + "_" + (_edit ? "1" : "0");
                bool boolSpan = false;

                switch (strCode)
                {
                    case "DATE":
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            strValue = ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString();
                            if (_wm == true && strValue == "")
                            {
                                if (intServicePrevious > 0)
                                    strValue = LoadPreviousData(ds, intServicePrevious, drForm["dbfield"].ToString());
                                else if (dsWorkflowSent.Tables[0].Rows.Count > 0)
                                    strValue = ds.Tables[0].Rows[0][drForm["dbfield_shared"].ToString()].ToString();
                            }
                            if (strValue != "")
                                strValue = DateTime.Parse(strValue).ToShortDateString();
                            else
                                strValue = "";
                        }
                        else if (_wm == false && _edit == true && boolInherited == true)
                            strValue = strDefaultText;
                        if (boolEditable == true && (boolInherited == false || _wm == true))
                        {
                            int intMinimum = Int32.Parse(drForm["width"].ToString());
                            int intMaximum = Int32.Parse(drForm["height"].ToString());
                            strContents += "<input name=\"" + strField + "\" type=\"text\" maxlength=\"10\" id=\"" + strField + "\" class=\"default\" style=\"width:100px;\" value=\"" + strValue + "\" /> <input type=\"image\" name=\"IMG" + strField + "\" id=\"IMG" + strField + "\" src=\"/images/calendar.gif\" align=\"absmiddle\" onclick=\"return ShowCalendar('" + strField + "');\" style=\"border-width:0px;\" />";
                            if (drForm["required"].ToString() == "1")
                                strRequired += " && ValidateDate('" + strField + "','" + drForm["required_text"].ToString().Replace("'", "\\'") + "')";
                            if (intMinimum > 0)
                            {
                                DateTime datMinimum = DateTime.Now.AddDays(intMinimum);
                                strContents += "<input type=\"hidden\" id=\"hdnMin" + strField + "\" name=\"hdnMin" + strField + "\" value=\"" + datMinimum.ToShortDateString() + "\" />";
                                strRequired += " && ValidateDateHidden('" + strField + "','hdnMin" + strField + "','NOTE: The service owner has configured a date restriction for this service.\\n\\n')";
                            }
                            if (intMaximum > 0)
                            {
                                DateTime datMaximum = DateTime.Now.AddDays(intMaximum);
                                strContents += "<input type=\"hidden\" id=\"hdnMax" + strField + "\" name=\"hdnMax" + strField + "\" value=\"" + datMaximum.ToShortDateString() + "\" />";
                                strRequired += " && ValidateDateHiddenBefore('" + strField + "','hdnMax" + strField + "','NOTE: The service owner has configured a date restriction for this service.\\n\\n')";
                            }
                        }
                        else
                            strContents += strValue;
                        break;
                    case "TEXTBOX":
                        if (ds != null && ds.Tables[0].Rows.Count > 0) 
                        {
                            strValue = ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString();
                            if (_wm == true && strValue == "")
                            {
                                if (intServicePrevious > 0)
                                    strValue = LoadPreviousData(ds, intServicePrevious, drForm["dbfield"].ToString());
                                else if (dsWorkflowSent.Tables[0].Rows.Count > 0)
                                    strValue = ds.Tables[0].Rows[0][drForm["dbfield_shared"].ToString()].ToString();
                            }
                        }
                        else if (_wm == false && _edit == true && boolInherited == true)
                            strValue = strDefaultText;
                        if (boolEditable == true && (boolInherited == false || _wm == true))
                        {
                            strContents += "<input name=\"" + strField + "\" type=\"text\" maxlength=\"" + drForm["length"].ToString() + "\" id=\"" + strField + "\" class=\"default\" style=\"width:" + drForm["width"].ToString() + "px;\" value=\"" + strValue + "\" />";
                            if (drForm["required"].ToString() == "1")
                                strRequired += " && ValidateText('" + strField + "','" + drForm["required_text"].ToString().Replace("'", "\\'") + "')";
                        }
                        else
                            strContents += strValue;
                        break;
                    case "TEXTAREA":
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            strValue = ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString();
                            if (_wm == true && strValue == "")
                            {
                                if (intServicePrevious > 0)
                                    strValue = LoadPreviousData(ds, intServicePrevious, drForm["dbfield"].ToString());
                                else if (dsWorkflowSent.Tables[0].Rows.Count > 0)
                                    strValue = ds.Tables[0].Rows[0][drForm["dbfield_shared"].ToString()].ToString();
                            }
                        }
                        else if (_wm == false && _edit == true && boolInherited == true)
                            strValue = strDefaultText;
                        if (boolEditable == true && (boolInherited == false || _wm == true))
                        {
                            strContents += "<textarea name=\"" + strField + "\" rows=\"" + drForm["height"].ToString() + "\" id=\"" + strField + "\" class=\"default\" style=\"width:" + drForm["width"].ToString() + "px;\">" + strValue + "</textarea>";
                            if (drForm["required"].ToString() == "1")
                                strRequired += " && ValidateText('" + strField + "','" + drForm["required_text"].ToString().Replace("'", "\\'") + "')";
                        }
                        else
                            strContents += oFunction.FormatText(strValue);
                        break;
                    case "USERS":
                    case "SERVERS":
                    case "MNEMONICS":
                    case "APPROVER":
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            Int32.TryParse(ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString(), out intValue);
                            if (_wm == true && intValue == 0)
                            {
                                if (intServicePrevious > 0)
                                    Int32.TryParse(LoadPreviousData(ds, intServicePrevious, drForm["dbfield"].ToString()), out intValue);
                                else if (dsWorkflowSent.Tables[0].Rows.Count > 0)
                                    Int32.TryParse(ds.Tables[0].Rows[0][drForm["dbfield_shared"].ToString()].ToString(), out intValue);
                            }
                        }
                        else if (_wm == false && _edit == true && boolInherited == true)
                            strValue = strDefaultText;
                        if (intValue > 0)
                        {
                            if (strCode == "USERS" || strCode == "APPROVER")
                                strValue = oUser.GetFullName(intValue) + " (" + oUser.GetName(intValue) + ")";
                            else if (strCode == "SERVERS")
                                strValue = oServer.Get(intValue, "servername");
                            else if (strCode == "MNEMONICS")
                                strValue = oMnemonic.Get(intValue, "factory_code") + " - " + oMnemonic.Get(intValue, "name");
                        }
                        if (boolEditable == true && (boolInherited == false || _wm == true))
                        {
                            strContents += "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
                            double dblHeight = (11.8 * double.Parse(drForm["height"].ToString()));
                            if (strCode == "USERS" || strCode == "APPROVER")
                                strContents += "<tr><td><input name=\"TXT" + strField + "\" type=\"text\" id=\"TXT" + strField + "\" class=\"default\" onkeyup=\"return AJAXTextBoxGet(this,'" + drForm["width"].ToString() + "','" + dblHeight.ToString("0") + "','DIV" + strField + "','LST" + strField + "','" + strField + "','" + oVariable.URL() + "/frame/users.aspx',2);\" style=\"width:" + drForm["width"].ToString() + "px;\" value=\"" + strValue + "\" /></td></tr>";
                            else if (strCode == "SERVERS")
                                strContents += "<tr><td><input name=\"TXT" + strField + "\" type=\"text\" id=\"TXT" + strField + "\" class=\"default\" onkeyup=\"return AJAXTextBoxGet(this,'" + drForm["width"].ToString() + "','" + dblHeight.ToString("0") + "','DIV" + strField + "','LST" + strField + "','" + strField + "','" + oVariable.URL() + "/frame/ajax/ajax_servernames.aspx',3);\" style=\"width:" + drForm["width"].ToString() + "px;\" value=\"" + strValue + "\" /></td></tr>";
                            else if (strCode == "MNEMONICS")
                                strContents += "<tr><td><input name=\"TXT" + strField + "\" type=\"text\" id=\"TXT" + strField + "\" class=\"default\" onkeyup=\"return AJAXTextBoxGet(this,'" + drForm["width"].ToString() + "','" + dblHeight.ToString("0") + "','DIV" + strField + "','LST" + strField + "','" + strField + "','" + oVariable.URL() + "/frame/ajax/ajax_mnemonics.aspx',2);\" style=\"width:" + drForm["width"].ToString() + "px;\" value=\"" + strValue + "\" /></td></tr>";
                            strContents += "<tr><td>";
                            strContents += "<div id=\"DIV" + strField + "\" style=\"overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC\">";
                            strContents += "<select size=\"" + drForm["height"].ToString() + "\" name=\"LST" + strField + "\" id=\"LST" + strField + "\" class=\"default\" ondblclick=\"AJAXClickRow();\"></select>";
                            strContents += "</div>";
                            strContents += "<input type=\"hidden\" name=\"" + strField + "\" id=\"" + strField + "\" value=\"" + intValue.ToString() + "\" />";
                            strContents += "</td></tr></table>";
                            if (drForm["required"].ToString() == "1")
                                strRequired += " && ValidateHidden0('" + strField + "','TXT" + strField + "','" + drForm["required_text"].ToString().Replace("'", "\\'") + "')";
                        }
                        else
                            strContents += strValue;
                        break;
                    case "CHECKBOX":
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            Int32.TryParse(ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString(), out intValue);
                            if (_wm == true && intValue == 0)
                            {
                                if (intServicePrevious > 0)
                                    Int32.TryParse(LoadPreviousData(ds, intServicePrevious, drForm["dbfield"].ToString()), out intValue);
                                else if (dsWorkflowSent.Tables[0].Rows.Count > 0)
                                    Int32.TryParse(ds.Tables[0].Rows[0][drForm["dbfield_shared"].ToString()].ToString(), out intValue);
                            }
                        }
                        else if (_wm == false && _edit == true && boolInherited == true)
                            strValue = strDefaultText;
                        if (boolEditable == true && (boolInherited == false || _wm == true))
                            strContents += "<span class=\"default\"><input id=\"" + strField + "\" type=\"checkbox\" name=\"" + strField + "\"" + (intValue == 1 || drForm["checked"].ToString() == "1" ? " checked=\"checked\"" : "") + "/></span>";
                        else
                            strContents += (strValue != "" ? strValue : (intValue == 1 ? "Yes" : "No"));
                        break;
                    case "DROPDOWN":
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            strValue = ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString();
                            if (_wm == true && (strValue == "" || strValue == "0"))
                            {
                                if (intServicePrevious > 0)
                                    strValue = LoadPreviousData(ds, intServicePrevious, drForm["dbfield"].ToString());
                                else if (dsWorkflowSent.Tables[0].Rows.Count > 0)
                                    strValue = ds.Tables[0].Rows[0][drForm["dbfield_shared"].ToString()].ToString();
                            }
                        }
                        else if (_wm == false && _edit == true && boolInherited == true)
                            strValue = strDefaultText;
                        if (boolEditable == true && (boolInherited == false || _wm == true))
                        {
                            strContents += "<select onchange=\"LoadEditorAffectsDropDown(this);\" name=\"" + strField + "\" id=\"" + strField + "\" class=\"default\" style=\"width:" + drForm["width"].ToString() + "px;\">";
                            strContents += "<option title=\"0_" + intConfig.ToString() + "\" value=\"0\">-- SELECT --</option>";
                            foreach (DataRow drValue in dsValues.Tables[0].Rows)
                                strContents += "<option title=\"" + drValue["id"].ToString() + "_" + intConfig.ToString() + "\" " + (strValue == drValue["value"].ToString() ? "selected=\"selected\"" : "") + " value=\"" + drValue["value"].ToString() + "\">" + drValue["value"].ToString() + "</option>";
                            strContents += "</select>";
                            if (drForm["required"].ToString() == "1")
                                strRequired += " && ValidateDropDown('" + strField + "','" + drForm["required_text"].ToString().Replace("'", "\\'") + "')";
                        }
                        else
                            strContents += strValue;
                        break;
                    case "CHECKLIST":
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            strValue = ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString();
                            if (_wm == true && strValue == "")
                            {
                                if (intServicePrevious > 0)
                                    strValue = LoadPreviousData(ds, intServicePrevious, drForm["dbfield"].ToString());
                                else if (dsWorkflowSent.Tables[0].Rows.Count > 0)
                                    strValue = ds.Tables[0].Rows[0][drForm["dbfield_shared"].ToString()].ToString();
                            }
                        }
                        else if (_wm == false && _edit == true && boolInherited == true)
                            strValue = strDefaultText;
                        strValues = strValue.Split(strSplit);
                        if (boolEditable == true && (boolInherited == false || _wm == true))
                        {
                            strContents += "<table id=\"TBL" + strField + "\" class=\"default\" border=\"0\">";
                            if (drForm["direction"].ToString() == "1")
                                strContents += "<tr>";
                            string strChecks = "";
                            foreach (DataRow drValue in dsValues.Tables[0].Rows)
                            {
                                string strID = strField + "_" + intCount.ToString();
                                string strItem = drValue["value"].ToString();
                                bool boolValue = false;
                                for (int ii = 0; ii < strValues.Length; ii++)
                                {
                                    if (strValues[ii].Trim() != "" && strValues[ii].Trim() == drValue["value"].ToString())
                                    {
                                        boolValue = true;
                                        break;
                                    }
                                }
                                if (boolValue == true)
                                    strChecks += drValue["value"].ToString() + ",";
                                if (drForm["direction"].ToString() == "0")
                                    strContents += "<tr>";
                                strContents += "<td><input id=\"" + strID + "\" type=\"checkbox\" name=\"CHK" + strField + "\"" + (boolValue == true ? " checked=\"checked\"" : "") + " onclick=\"ChangeCheckItemsComma('" + strField + "','" + drValue["value"].ToString() + "',this);\" /><label for=\"" + strID + "\">" + strItem + "</label></td>";
                                if (drForm["direction"].ToString() == "0")
                                    strContents += "</tr>";
                                intCount++;
                            }
                            if (drForm["direction"].ToString() == "1")
                                strContents += "</tr>";
                            strContents += "</table>";
                            strContents += "<input type=\"hidden\" name=\"" + strField + "\" id=\"" + strField + "\" value=\"" + strChecks + "\" />";
                            if (drForm["required"].ToString() == "1")
                                strRequired += " && ValidateHidden('" + strField + "','TBL" + strField + "','" + drForm["required_text"].ToString().Replace("'", "\\'") + "')";
                        }
                        else
                        {
                            for (int ii = 0; ii < strValues.Length; ii++)
                            {
                                if (strValues[ii].Trim() != "")
                                    strReturnValue += strValues[ii].Trim() + "~";
                            }
                        }
                        break;
                    case "RADIOLIST":
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            strValue = ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString();
                            if (_wm == true && strValue == "")
                            {
                                if (intServicePrevious > 0)
                                    strValue = LoadPreviousData(ds, intServicePrevious, drForm["dbfield"].ToString());
                                else if (dsWorkflowSent.Tables[0].Rows.Count > 0)
                                    strValue = ds.Tables[0].Rows[0][drForm["dbfield_shared"].ToString()].ToString();
                            }
                        }
                        else if (_wm == false && _edit == true && boolInherited == true)
                            strValue = strDefaultText;
                        if (boolEditable == true && (boolInherited == false || _wm == true))
                        {
                            strContents += "<table id=\"TBL" + strField + "\" class=\"default\" border=\"0\">";
                            if (drForm["direction"].ToString() == "1")
                                strContents += "<tr>";
                            foreach (DataRow drValue in dsValues.Tables[0].Rows)
                            {
                                string strID = strField + "_" + intCount.ToString();
                                string strItem = drValue["value"].ToString();
                                if (drForm["direction"].ToString() == "0")
                                    strContents += "<tr>";
                                strContents += "<td><input id=\"" + strID + "\" type=\"radio\" name=\"" + strField + "\" value=\"" + strItem + "\" " + (strValue == strItem ? " checked" : "") + " onclick=\"LoadEditorAffectsRadio(this);\" title=\"" + drValue["id"].ToString() + "_" + intConfig.ToString() + "\" /><label for=\"" + strID + "\">" + strItem + "</label></td>";
                                if (drForm["direction"].ToString() == "0")
                                    strContents += "</tr>";
                                intCount++;
                            }
                            if (drForm["direction"].ToString() == "1")
                                strContents += "</tr>";
                            strContents += "</table>";
                            if (drForm["required"].ToString() == "1")
                                strRequired += " && ValidateRadioList('TBL" + strField + "','" + drForm["required_text"].ToString().Replace("'", "\\'") + "')";
                        }
                        else
                            strContents += strValue;
                        break;
                    case "LISTBOX":
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            strValue = ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString();
                            if (_wm == true && strValue == "")
                            {
                                if (intServicePrevious > 0)
                                    strValue = LoadPreviousData(ds, intServicePrevious, drForm["dbfield"].ToString());
                                else if (dsWorkflowSent.Tables[0].Rows.Count > 0)
                                    strValue = ds.Tables[0].Rows[0][drForm["dbfield_shared"].ToString()].ToString();
                            }
                        }
                        else if (_wm == false && _edit == true && boolInherited == true)
                            strValue = strDefaultText;
                        strValues = strValue.Split(strSplit);
                        if (boolEditable == true && (boolInherited == false || _wm == true))
                        {
                            strContents += "<select size=\"" + drForm["height"].ToString() + "\" name=\"" + strField + "\"" + (drForm["multiple"].ToString() == "1" ? " multiple=\"multiple\"" : "") + " id=\"" + strField + "\" class=\"default\" style=\"width:" + drForm["width"].ToString() + "px;\">";
                            foreach (DataRow drValue in dsValues.Tables[0].Rows)
                            {
                                bool boolValue = false;
                                for (int ii = 0; ii < strValues.Length; ii++)
                                {
                                    if (strValues[ii].Trim() != "" && strValues[ii].Trim() == drValue["value"].ToString())
                                    {
                                        boolValue = true;
                                        break;
                                    }
                                }
                                strContents += "<option" + (boolValue == true ? " selected=\"selected\"" : "") + " value=\"" + drValue["value"].ToString() + "\">" + drValue["value"].ToString() + "</option>";
                            }
                            strContents += "</select>";
                            if (drForm["required"].ToString() == "1")
                                strRequired += " && ValidateListBox('" + strField + "','" + drForm["required_text"].ToString().Replace("'", "\\'") + "')";
                        }
                        else
                        {
                            for (int ii = 0; ii < strValues.Length; ii++)
                            {
                                if (strValues[ii].Trim() != "")
                                    strReturnValue += strValues[ii].Trim() + "~";
                            }
                        }
                        break;
                    case "LIST":
                    case "SERVERLIST":
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            strValue = ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString();
                            if (_wm == true && strValue == "")
                            {
                                if (intServicePrevious > 0)
                                    strValue = LoadPreviousData(ds, intServicePrevious, drForm["dbfield"].ToString());
                                else if (dsWorkflowSent.Tables[0].Rows.Count > 0)
                                    strValue = ds.Tables[0].Rows[0][drForm["dbfield_shared"].ToString()].ToString();
                            }
                        }
                        else if (_wm == false && _edit == true && boolInherited == true)
                            strValue = strDefaultText;
                        strValues = strValue.Split(strSplit);
                        if (boolEditable == true && (boolInherited == false || _wm == true))
                        {
                            strContents += "<table cellpadding=\"2\" cellspacing=\"0\" border=\"0\"><tr><td colspan=\"2\">";
                            if (strCode == "LIST")
                                strContents += "<input name=\"TXT" + strField + "\" type=\"text\" maxlength=\"" + drForm["length"].ToString() + "\" id=\"TXT" + strField + "\" class=\"default\" style=\"width:" + drForm["width"].ToString() + "px;\" onkeydown=\"if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('btnAdd" + strField + "').click();return false;}} else {return true}; \" />";
                            else if (strCode == "SERVERLIST")
                            {
                                strContents += "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
                                double dblHeight = (11.8 * double.Parse(drForm["height"].ToString()));
                                strContents += "<tr><td><input name=\"TXT" + strField + "\" type=\"text\" id=\"TXT" + strField + "\" class=\"default\" onkeyup=\"return AJAXTextBoxGet(this,'" + drForm["width"].ToString() + "','" + dblHeight.ToString("0") + "','DIV" + strField + "','LST" + strField + "','HDN" + strField + "','" + oVariable.URL() + "/frame/ajax/ajax_servernames.aspx',3);\" style=\"width:" + drForm["width"].ToString() + "px;\" /></td></tr>";
                                strContents += "<tr><td>";
                                strContents += "<div id=\"DIV" + strField + "\" style=\"overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC\">";
                                strContents += "<select size=\"" + drForm["height"].ToString() + "\" name=\"LST" + strField + "\" id=\"LST" + strField + "\" class=\"default\" ondblclick=\"AJAXClickRow();\"></select>";
                                strContents += "</div>";
                                strContents += "<input type=\"hidden\" name=\"HDN" + strField + "\" id=\"HDN" + strField + "\" value=\"\" />";
                                strContents += "</td></tr></table>";
                            }
                            strContents += "</td></tr><tr>";
                            if (strCode == "LIST")
                                strContents += "<td><input type=\"submit\" name=\"btnAdd" + strField + "\" value=\"Add to List\" onclick=\"return ValidateText('TXT" + strField + "','Please enter some text') &amp;&amp; ValidateNoComma('TXT" + strField + "','The text cannot contain a comma (,)\\n\\nPlease click OK and remove all commas') &amp;&amp; ListControlIn('SEL" + strField + "','" + strField + "','TXT" + strField + "');\" id=\"btnAdd" + strField + "\" class=\"default\" style=\"width:75px;\" /></td>";
                            else if (strCode == "SERVERLIST")
                                strContents += "<td><input type=\"submit\" name=\"btnAdd" + strField + "\" value=\"Add to List\" onclick=\"return ValidateText('TXT" + strField + "','Please enter some text') &amp;&amp; ValidateNoComma('TXT" + strField + "','The text cannot contain a comma (,)\\n\\nPlease click OK and remove all commas') &amp;&amp; ListControlInH('SEL" + strField + "','" + strField + "','TXT" + strField + "','HDN" + strField + "');\" id=\"btnAdd" + strField + "\" class=\"default\" style=\"width:75px;\" /></td>";
                            strContents += "<td align=\"right\"><input type=\"submit\" name=\"btnRemove" + strField + "\" value=\"Remove from List\" onclick=\"return ListControlOut('SEL" + strField + "','" + strField + "');\" id=\"btnRemove" + strField + "\" class=\"default\" style=\"width:115px;\" /></td>";
                            strContents += "</tr><tr>";
                            strContents += "<td colspan=\"2\"><select size=\"" + drForm["height"].ToString() + "\" name=\"SEL" + strField + "\" id=\"SEL" + strField + "\" class=\"default\" style=\"width:" + drForm["width"].ToString() + "px;\">";
                            string strLists = "";
                            for (int ii = 0; ii < strValues.Length; ii++)
                            {
                                if (strValues[ii].Trim() != "")
                                {
                                    if (strCode == "LIST")
                                        strContents += "<option value=\"" + strValues[ii].Trim() + "\">" + strValues[ii].Trim() + "</option>";
                                    else if (strCode == "SERVERLIST")
                                        strContents += "<option value=\"" + strValues[ii].Trim() + "\">" + oServer.Get(Int32.Parse(strValues[ii].Trim()), "servername") + "</option>";
                                    strLists += strValues[ii].Trim() + ",";
                                }
                            }
                            strContents += "</select>";
                            strContents += "</td>";
                            strContents += "</tr></table>";
                            strContents += "<input type=\"hidden\" name=\"" + strField + "\" id=\"" + strField + "\" value=\"" + strLists + "\" />";
                            if (drForm["required"].ToString() == "1")
                                strRequired += " && ValidateHidden('" + strField + "','TXT" + strField + "','" + drForm["required_text"].ToString().Replace("'", "\\'") + "')";
                        }
                        else
                        {
                            for (int ii = 0; ii < strValues.Length; ii++)
                            {
                                if (strValues[ii].Trim() != "")
                                    strReturnValue += strValues[ii].Trim() + "~";
                            }
                        }
                        break;
                    case "HYPERLINK":
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            strValue = ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString();
                            if (_wm == true && strValue == "")
                            {
                                if (intServicePrevious > 0)
                                    strValue = LoadPreviousData(ds, intServicePrevious, drForm["dbfield"].ToString());
                                else if (dsWorkflowSent.Tables[0].Rows.Count > 0)
                                    strValue = ds.Tables[0].Rows[0][drForm["dbfield_shared"].ToString()].ToString();
                            }
                        }
                        else if (_wm == false && _edit == true && boolInherited == true)
                            strValue = strDefaultText;
                        if (boolEditable == true && (boolInherited == false || _wm == true))
                        {
                            strContents += "<input name=\"" + strField + "\" type=\"text\" id=\"" + strField + "\" class=\"default\" style=\"width:" + drForm["width"].ToString() + "px;\" value=\"" + strValue + "\" />&nbsp;<a href=\"javascript:void(0);\" title=\"Click here to preview the link\" onclick=\"OpenTextBoxHyperlink('" + strField + "');\"><img src=\"/images/search.gif\" border=\"0\" align=\"absmiddle\"/>";
                            if (drForm["required"].ToString() == "1")
                                strRequired += " && ValidateText('" + strField + "','" + drForm["required_text"].ToString().Replace("'", "\\'") + "') && ValidateHyperlink('" + strField + "','" + drForm["required_text"].ToString().Replace("'", "\\'") + "')";
                        }
                        else
                            strContents += (strValue == strDefaultText ? strValue : "<a href=\"" + strValue + "\" target=\"_blank\" title=\"Click here to open this link\">" + strValue + "</a>");
                        break;
                    case "FILEUPLOAD":
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            strValue = ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString();
                            if (_wm == true && strValue == "")
                            {
                                if (intServicePrevious > 0)
                                    strValue = LoadPreviousData(ds, intServicePrevious, drForm["dbfield"].ToString());
                                else if (dsWorkflowSent.Tables[0].Rows.Count > 0)
                                    strValue = ds.Tables[0].Rows[0][drForm["dbfield_shared"].ToString()].ToString();
                            }
                        }
                        else if (_wm == false && _edit == true && boolInherited == true)
                            strValue = strDefaultText;
                        if (boolEditable == true && (boolInherited == false || _wm == true))
                        {
                            strContents += "<input name=\"" + strField + "\" type=\"file\" runat=\"server\" id=\"" + strField + "\" class=\"file\" style=\"width:" + drForm["width"].ToString() + "px;\" value=\"" + strValue + "\" />";
                            if (strValue != "")
                            {
                                strContents += "<br/><br/>";
                                strContents += "<table cellpadding=\"0\" cellspacing=\"5\" border=\"0\" style=\"border:solid 1px #EE2C2C; background-color:#F7EFEE\">";
                                strContents += "<tr><td><b>NOTE:</b> A file has already been uploaded. If you wish to overwrite the existing file with a new file, please click the &quot;Browse...&quot; button.</td></tr>";
                                strContents += "<tr><td>To view the uploaded file, click on the following button...</td></tr>";
                                strContents += "<tr><td><input type=\"button\" title=\"Click here to view the file\" onclick=\"OpenFileUpload('" + strValue.Replace("\\", "\\\\") + "');\" value=\"View File\"></td></tr>";
                                strContents += "<tr><td>If you do not want to upload a new file, and you want to delete the file you have already uploaded, please check the following checkbox...</td></tr>";
                                //strContents += "<tr><td><input id=\"" + strField + "_CLEAR\" runat=\"server\"" + (drForm["required"].ToString() == "1" ? "disabled " : "") + " type=\"checkbox\" name=\"" + strField + "_CLEAR\"/><label for=\"" + strField + "_CLEAR\">Remove File" + (drForm["required"].ToString() == "1" ? " (Since this is a required field, you cannot remove the file)" : "") + "</label></td></tr>";
                                strContents += "<tr><td><input id=\"" + strField + "_CLEAR\" runat=\"server\" type=\"checkbox\" name=\"" + strField + "_CLEAR\"/><label for=\"" + strField + "_CLEAR\">Remove File</label></td></tr>";
                                strContents += "</table>";
                            }
                            //strContents += "<input type=\"hidden\" name=\"" + strField + "_CLEAR\" id=\"" + strField + "_CLEAR\" value=\"0\" />";
                            if (drForm["required"].ToString() == "1" && strValue == "")
                                strRequired += " && ValidateText('" + strField + "','" + drForm["required_text"].ToString().Replace("'", "\\'") + "')";
                        }
                        else
                            strContents += (strValue == strDefaultText ? strValue : "<a href=\"" + strValue + "\" target=\"_blank\" title=\"Click here to open this file\">" + strValue + "</a>");
                        break;
                    //case "SERVERS":
                    //    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    //    {
                    //        Int32.TryParse(ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString(), out intValue);
                    //        if (_wm == true && intValue == 0)
                    //        {
                    //            if (intServicePrevious > 0)
                    //                Int32.TryParse(LoadPreviousData(ds, intServicePrevious, drForm["dbfield"].ToString()), out intValue);
                    //            else if (dsWorkflowSent.Tables[0].Rows.Count > 0)
                    //                Int32.TryParse(ds.Tables[0].Rows[0][drForm["dbfield_shared"].ToString()].ToString(), out intValue);
                    //        }
                    //    }
                    //    else if (_wm == false && _edit == true && boolInherited == true)
                    //        strValue = strDefaultText;
                    //    if (intValue > 0)
                    //    {
                    //        Servers oServer = new Servers(0, dsn);
                    //        strValue = oServer.Get(intValue, "servername");
                    //    }
                    //    if (boolEditable == true && (boolInherited == false || _wm == true))
                    //    {
                    //        strContents += "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
                    //        strContents += "<tr><td><input name=\"TXT" + strField + "\" type=\"text\" id=\"TXT" + strField + "\" class=\"default\" onkeyup=\"return AJAXTextBoxGet(this,'" + drForm["width"].ToString() + "','195','DIV" + strField + "','LST" + strField + "','" + strField + "','" + oVariable.URL() + "/frame/ajax/ajax_servernames.aspx',3);\" style=\"width:" + drForm["width"].ToString() + "px;\" value=\"" + strValue + "\" /></td></tr>";
                    //        strContents += "<tr><td>";
                    //        strContents += "<div id=\"DIV" + strField + "\" style=\"overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC\">";
                    //        strContents += "<select size=\"" + drForm["height"].ToString() + "\" name=\"LST" + strField + "\" id=\"LST" + strField + "\" class=\"default\" ondblclick=\"AJAXClickRow();\"></select>";
                    //        strContents += "</div>";
                    //        strContents += "<input type=\"hidden\" name=\"" + strField + "\" id=\"" + strField + "\" value=\"" + intValue.ToString() + "\" />";
                    //        strContents += "</td></tr></table>";
                    //        if (drForm["required"].ToString() == "1")
                    //            strRequired += " && ValidateHidden0('" + strField + "','TXT" + strField + "','" + drForm["required_text"].ToString().Replace("'", "\\'") + "')";
                    //    }
                    //    else
                    //        strContents += strValue;
                    //    break;
                    case "DISCLAIMER":
                        boolSpan = true;
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            Int32.TryParse(ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString(), out intValue);
                            if (_wm == true && intValue == 0)
                            {
                                if (intServicePrevious > 0)
                                    Int32.TryParse(LoadPreviousData(ds, intServicePrevious, drForm["dbfield"].ToString()), out intValue);
                                else if (dsWorkflowSent.Tables[0].Rows.Count > 0)
                                    Int32.TryParse(ds.Tables[0].Rows[0][drForm["dbfield_shared"].ToString()].ToString(), out intValue);
                            }
                        }
                        else if (_wm == false && _edit == true && boolInherited == true)
                            strValue = strDefaultText;

                        StringBuilder strQuestionHighlighted = new StringBuilder();
                        string strQuestion = drForm["question"].ToString();
                        string strHighlight = drForm["other_text"].ToString();
                        if (strHighlight != "" && strQuestion.Contains(strHighlight))
                        {
                            strQuestionHighlighted.Append(strQuestion.Substring(0, strQuestion.IndexOf(strHighlight)));
                            strQuestionHighlighted.Append("<a href='" + drForm["url"].ToString() + "' target='_blank'>" + strHighlight + "</a>");
                            strQuestionHighlighted.Append(strQuestion.Substring(strQuestion.IndexOf(strHighlight) + strHighlight.Length));
                        }
                        else
                            strQuestionHighlighted.Append("<a href='" + drForm["url"].ToString() + "' target='_blank'>" + strQuestion + "</a>");

                        if (boolEditable == true && (boolInherited == false || _wm == true))
                        {
                            strContents += "<input name=\"" + strField + "\" type=\"checkbox\" id=\"" + strField + "\"" + (intValue == 1 || drForm["checked"].ToString() == "1" ? " checked=\"checked\"" : "") + "/>&nbsp;<label for=\"" + strField + "\">" + strQuestionHighlighted.ToString() + "</label>";
                            if (drForm["required"].ToString() == "1")
                                strRequired += " && ValidateCheck('" + strField + "','" + drForm["required_text"].ToString().Replace("'", "\\'") + "')";
                        }
                        else
                            strContents += (strValue != "" ? strValue : (intValue == 1 ? "Yes" : "No"));
                        break;
                    case "LOCATION":
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            Int32.TryParse(ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString(), out intValue);
                            if (_wm == true && intValue == 0)
                            {
                                if (intServicePrevious > 0)
                                    Int32.TryParse(LoadPreviousData(ds, intServicePrevious, drForm["dbfield"].ToString()), out intValue);
                                else if (dsWorkflowSent.Tables[0].Rows.Count > 0)
                                    Int32.TryParse(ds.Tables[0].Rows[0][drForm["dbfield_shared"].ToString()].ToString(), out intValue);
                            }
                        }
                        else if (_wm == false && _edit == true && boolInherited == true)
                            strValue = strDefaultText;
                        if (intValue > 0)
                            strValue = oLocation.GetFull(intValue);
                        if (boolEditable == true && (boolInherited == false || _wm == true))
                        {
                            int intLocationDefault = 0;
                            Int32.TryParse(drForm["width"].ToString(), out intLocationDefault);
                            strContents += oLocation.LoadDDL("ddlState" + strField, "ddlCity" + strField, "ddlAddress" + strField, strField, intLocationDefault, true, "ddlCommon" + strField);
                            strContents += "<input type=\"hidden\" name=\"" + strField + "\" id=\"" + strField + "\" value=\"" + intValue.ToString() + "\" />";
                            if (drForm["required"].ToString() == "1")
                                strRequired += " && ValidateHidden0('" + strField + "','ddlCommon" + strField + "','" + drForm["required_text"].ToString().Replace("'", "\\'") + "')";
                        }
                        else
                            strContents += strValue;
                        break;
                }
                if (strContents != "")
                {
                    strRow += "<table title=\"" + drForm["tip"].ToString() + "\" id=\"TBL_" + intConfig.ToString() + "\" cellspacing=\"0\" cellpadding=\"2\" border=\"0\">";
                    if (_edit == true)
                        strRow += "<tr><td colspan=\"2\" class=\"reddefault\" ondrop=\"onDrop(this,'" + intRow.ToString() + "','" + _hidden + "');\" ondragover=\"overDrag();\" ondragenter=\"enterDrag(this);\" ondragleave=\"leaveDrag(this);\">&nbsp;</td></tr>";
                    else
                        strRow += "<tr><td colspan=\"2\">&nbsp;</td></tr>";
                    if (boolSpan == false)
                        strRow += "<tr><td colspan=\"2\">" + drForm["question"].ToString() + (drForm["required"].ToString() == "1" ? "<span class=\"required\">&nbsp;*" : "") + "</td></tr>";
                    strRow += "<tr>";
                    if (_edit == true)
                        strRow += "<td valign=\"top\"><table cellpadding=\"2\" cellspacing=\"2\" border=\"0\"><tr><td><a ondragstart=\"window.oDrag = '" + intConfig.ToString() + "';\" style=\"cursor:move\" title=\"Move Control\"><img src=\"/images/move.gif\" border=\"0\"/></a></td><td><a href=\"javascript:void(0);\" onclick=\"OpenWindow('NEW_CONTROL','?wm=" + (_wm == true ? "1" : "0") + "&serviceid=" + _serviceid.ToString() + "&configid=" + drForm["id"].ToString() + "');\" title=\"Edit Control\"><img src=\"/images/edit.gif\" border=\"0\"/></a></td><td><a href=\"javascript:void(0);\" onclick=\"OpenWindow('SERVICE_EDITOR_DISPLAY','?wm=" + (_wm == true ? "1" : "0") + "&serviceid=" + _serviceid.ToString() + "&configid=" + drForm["id"].ToString() + "');\" title=\"Show / Hide Options\"><img src=\"/images/eye.gif\" border=\"0\"/></a></td></tr></table></td>";
                    else
                        strRow += "<td><img src=\"/images/spacer.gif\" border=\"0\" width=\"50\" height=\"1\"/></td>";
                    strRow += "<td width=\"100%\">";
                    strRow += strContents;
                    if (strReturnValue != "")
                    {
                        char[] strReturnSplit = { '~' };
                        string[] strReturnValues = strReturnValue.Split(strReturnSplit);
                        for (int ii = 0; ii < strReturnValues.Length; ii++)
                        {
                            if (strReturnValues[ii].Trim() != "")
                                strRow += strReturnValues[ii].Trim() + "<br/>";
                        }
                    }
                    strRow += "</td>";
                    strRow += "</tr>";
                    strRow += "</table>";
                }
                if (_edit == true)
                    strForm += "<tr><td>" + strRow + "</td></tr>";
                else
                {
                    string strDisplay = "inline";
                    DataSet dsAff = GetConfigAffectsConfig(intConfig);
                    if (drForm["hidden"].ToString() == "1" && dsAff.Tables[0].Rows.Count > 0)
                    {
                        if (_wm)
                        {
                            bool found = false;
                            // make sure the affected value is visible in the workload manager (might only have been applicable in the service request)
                            foreach (DataRow drAff in dsAff.Tables[0].Rows)
                            {
                                try
                                {
                                    int intAffValue = Int32.Parse(drAff["valueid"].ToString());
                                    DataSet dsAffValue = GetConfigValue(intAffValue);
                                    int intAffConfig = Int32.Parse(dsAffValue.Tables[0].Rows[0]["configid"].ToString());
                                    foreach (DataRow drFormAff in dsForm.Tables[0].Rows)
                                    {
                                        if (intAffConfig == Int32.Parse(drFormAff["id"].ToString()))
                                        {
                                            found = true;   // config applies to this question
                                            break;
                                        }
                                    }
                                }
                                catch { }
                                if (found)
                                    break;
                            }
                            if (found)
                                strDisplay = "none";    // since a config that's affecting visibility is found in the WM configs, hide the question
                        }
                        else
                            strDisplay = "none";    // since this is on the service request, it's obvious that it's hidden
                        foreach (DataRow drAff in dsAff.Tables[0].Rows)
                        {
                            try
                            {
                                int intAffValue = Int32.Parse(drAff["valueid"].ToString());
                                DataSet dsAffValue = GetConfigValue(intAffValue);
                                string strAffValue = dsAffValue.Tables[0].Rows[0]["value"].ToString();
                                int intAffConfig = Int32.Parse(dsAffValue.Tables[0].Rows[0]["configid"].ToString());
                                string strAffField = GetConfig(intAffConfig, "dbfield");
                                if (ds.Tables[0].Rows[0][strAffField].ToString() == strAffValue)
                                {
                                    strDisplay = "inline";
                                    break;
                                }
                            }
                            catch { }
                        }
                    }
                    strForm += "<tr id=\"div" + intConfig.ToString() + "\" style=\"display:" + strDisplay + "\"><td>" + strRow + "</td></tr>";
                }
            }
            intRow++;
            if (_edit == true)
                strForm += "<tr><td class=\"reddefault\" ondrop=\"onDrop(this,'" + intRow.ToString() + "','" + _hidden + "');\" ondragover=\"overDrag();\" ondragenter=\"enterDrag(this);\" ondragleave=\"leaveDrag(this);\">&nbsp;</td></tr>";
            if (strForm != "")
                strForm = "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">" + strForm + "</table>";
            return strForm;
        }
        private string LoadPreviousData(DataSet ds, int intServicePrevious, string _dbfield)
        {
            string strReturn = "";
            try
            {
                int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                DataSet dsPrevious = GetRequestData(intRequest, intServicePrevious, intNumber, 1, "");
                if (dsPrevious.Tables[0].Rows.Count > 0)
                    strReturn = dsPrevious.Tables[0].Rows[0][_dbfield].ToString();
            }
            catch { }
            return strReturn;
        }
        public string GetRequired()
        {
            return strRequired.Replace(Environment.NewLine, "\\n");
        }

        public void AddRequest(int _requestid, int _serviceid, int _number, string _title, int _priority, string _statement, DateTime _start_date, DateTime _end_date, int _expedite)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@title", _title);
            arParams[4] = new SqlParameter("@priority", _priority);
            arParams[5] = new SqlParameter("@statement", _statement);
            arParams[6] = new SqlParameter("@start_date", _start_date);
            arParams[7] = new SqlParameter("@end_date", _end_date);
            arParams[8] = new SqlParameter("@expedite", _expedite);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_addRequest", arParams);
        }
        public DataSet GetRequestData(int _requestid, int _serviceid, int _number, int _wm, string _dsn)
        {
            bool boolTable = false;
            string strTable = "set_GEN_" + _serviceid.ToString();
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "select name from sys.tables where name = '" + strTable + "'");
            boolTable = (o != null);
            string strSQL = "";
            string strJoin = "";
            string strJoinOther = "";
            strSQL = "SELECT set_requests.*";
            if (boolTable == true)
            {
                DataSet ds = GetConfigs(_requestid, _serviceid, _number, _wm, 1);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Int32.Parse(dr["serviceid"].ToString()) == _serviceid)
                    {
                        strSQL += ", " + strTable + ".[" + dr["dbfield"].ToString() + "]";
                        if (_wm == 1 && dr["dbfield"].ToString() != dr["dbfield_shared"].ToString())
                        {
                            string strSQLShared = ", " + strTable + ".[" + dr["dbfield_shared"].ToString() + "]";
                            if (strSQL.Contains(strSQLShared) == false)
                                strSQL += strSQLShared;
                        }
                    }
                    else
                    {
                        string strServiceTable = "set_GEN_" + dr["serviceid"].ToString();
                        strSQL += ", " + strServiceTable + ".[" + dr["dbfield"].ToString() + "]";
                        if (_wm == 1 && dr["dbfield"].ToString() != dr["dbfield_shared"].ToString())
                        {
                            string strSQLShared = ", " + strServiceTable + ".[" + dr["dbfield_shared"].ToString() + "]";
                            if (strSQL.Contains(strSQLShared) == false)
                                strSQL += strSQLShared;
                        }
                        string strNewJoin = " LEFT OUTER JOIN " + strServiceTable + " ON " + strServiceTable + ".requestid = " + strTable + ".requestid AND " + strServiceTable + ".number = " + strTable + ".number";
                        if (strJoinOther.Contains(strNewJoin) == false)
                            strJoinOther += strNewJoin;
                    }
                }
                strJoin += " INNER JOIN " + strTable + strJoinOther + " ON " + strTable + ".requestid = set_requests.requestid AND " + strTable + ".serviceid = set_requests.serviceid AND " + strTable + ".number = set_requests.number";
            }
            strSQL += " FROM set_requests" + strJoin + " WHERE set_requests.requestid = @requestid AND set_requests.serviceid = @serviceid AND set_requests.number = @number AND set_requests.deleted = 0";

            if (_dsn != "")
            {
                Functions oFunction = new Functions(user, _dsn, 0);
                DataSet dsKey = oFunction.GetSetupValuesByKey("LOGGING_EDIRECTORY");
                if (dsKey.Tables[0].Rows.Count > 0 && dsKey.Tables[0].Rows[0]["Value"].ToString() == "1")
                {
                    Log oLog = new Log(user, _dsn);
                    string strCVT = "CVT" + _requestid.ToString() + "-" + _serviceid.ToString() + "-" + _number.ToString();
                    oLog.AddEvent(_requestid.ToString(), strCVT, strSQL.Replace("@requestid", _requestid.ToString()).Replace("@serviceid", _serviceid.ToString()).Replace("@number", _number.ToString()), LoggingType.Debug);
                }
            }

            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.Text, strSQL, arParams);
        }
        //public int GetRequestFirst(int _requestid, int _serviceid, int _number, int _wm, string _dsn)
        //{
        //    int intFirstService = _serviceid;
        //    Services oService = new Services(0, _dsn);
        //    DataSet ds = GetRequestData(_requestid, _serviceid, _number, _wm);
        //    if (ds.Tables[0].Rows.Count == 0)
        //    {
        //        DataSet dsReceive = oService.GetWorkflowsReceive(intFirstService);
        //        while (dsReceive.Tables[0].Rows.Count > 0)
        //        {
        //            intFirstService = Int32.Parse(dsReceive.Tables[0].Rows[0]["serviceid"].ToString());
        //            dsReceive = oService.GetWorkflowsReceive(intFirstService);
        //        }
        //    }
        //    return intFirstService;
        //}
        //public DataSet GetRequestFirstData(int _requestid, int _serviceid, int _number, int _wm, string _dsn)
        //{
        //    _serviceid = GetRequestFirst(_requestid, _serviceid, _number, _wm, _dsn);
        //    return GetRequestData(_requestid, _serviceid, _number, _wm);
        //}
        public DataSet GetRequestFirstData2(int _requestid, int _serviceid, int _number, int _wm, string _dsn)
        {
            return GetRequestData(_requestid, _serviceid, _number, _wm, _dsn);
        }
        public string GetRequestBody(int _requestid, int _serviceid, int _number, string _dsn)
        {
            StringBuilder sbReturn = new StringBuilder();
            Users oUser = new Users(0, _dsn);
            Locations oLocation = new Locations(0, _dsn);
            Servers oServer = new Servers(0, _dsn);
            Mnemonic oMnemonic = new Mnemonic(0, _dsn);
            Services oService = new Services(0, _dsn);
            Functions oFunction = new Functions(0, _dsn, 0);
            //_serviceid = GetRequestFirst(_requestid, _serviceid, _number, 0, _dsn);
            DataSet ds = GetRequestData(_requestid, _serviceid, _number, 0, _dsn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                sbReturn.Append("<tr><td colspan=\"2\"><span style=\"width:100%;border-bottom:1 dotted #CCCCCC;\"/></td></tr>");
                sbReturn.Append("<tr><td colspan=\"2\"><b>Service Request Details</b></td></tr>");
                if (oService.Get(_serviceid, "project") != "1")
                {
                    if (oService.Get(_serviceid, "title_override") == "1")
                    {
                        sbReturn.Append("<tr><td colspan=\"2\">");
                        sbReturn.Append(oService.Get(_serviceid, "title_name"));
                        sbReturn.Append(":</td></tr>");
                    }
                    else
                    {
                        sbReturn.Append("<tr><td colspan=\"2\">Please enter a title for this request:</td></tr>");
                    }
                    sbReturn.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"50\" height=\"1\"/></td><td>");
                    sbReturn.Append(ds.Tables[0].Rows[0]["title"].ToString());
                    sbReturn.Append("</td></tr>");
                }
                if (oService.Get(_serviceid, "statement") == "1")
                {
                    sbReturn.Append("<tr><td colspan=\"2\">Please enter your statement of work:</td></tr>");
                    sbReturn.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"50\" height=\"1\"/></td><td>");
                    sbReturn.Append(oFunction.FormatText(ds.Tables[0].Rows[0]["statement"].ToString()));
                    sbReturn.Append("</td></tr>");
                }
                //sbReturn.Append("<tr><td colspan=\"2\">Please select your priority:</td></tr>");
                //sbReturn.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"50\" height=\"1\"/></td><td>");
                //sbReturn.Append(ds.Tables[0].Rows[0]["priority"].ToString());
                //sbReturn.Append("</td></tr>");
                //if (oService.Get(_serviceid, "expedite") == "1")
                //{
                //    sbReturn.Append("<tr><td colspan=\"2\">Please select if you would like to expedite this request:</td></tr>");
                //    sbReturn.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"50\" height=\"1\"/></td><td>");
                //    sbReturn.Append(ds.Tables[0].Rows[0]["expedite"].ToString() == "1" ? "Yes" : "No");
                //    sbReturn.Append("</td></tr>");
                //}
                string[] strValues;
                char[] strSplit = { ',' };
                DataSet dsForm = GetConfigs(_requestid, _serviceid, _number, 0, 1);
                foreach (DataRow drForm in dsForm.Tables[0].Rows)
                {
                    if (drForm["details"].ToString() == "1")
                    {
                        int intServiceCompare = 0;
                        Int32.TryParse(drForm["serviceid"].ToString(), out intServiceCompare);
                        string strReturnValue = "";
                        switch (drForm["code"].ToString().ToUpper())
                        {
                            case "DATE":
                                if (ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString() != "")
                                    strReturnValue = DateTime.Parse(ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString()).ToShortDateString();
                                break;
                            case "TEXTBOX":
                                strReturnValue = ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString();
                                break;
                            case "TEXTAREA":
                                strReturnValue = oFunction.FormatText(ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString());
                                break;
                            case "USERS":
                            case "APPROVER":
                                int intUser = 0;
                                if (ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString() != "")
                                {
                                    Int32.TryParse(ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString(), out intUser);
                                    if (intUser != 0)
                                        strReturnValue = oUser.GetFullName(intUser) + " (" + oUser.GetName(intUser) + ")";
                                }
                                else
                                    strReturnValue = "";
                                break;
                            case "SERVERS":
                                int intServer = 0;
                                if (ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString() != "")
                                {
                                    Int32.TryParse(ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString(), out intServer);
                                    if (intServer != 0)
                                        strReturnValue = oServer.Get(intServer, "servername");
                                }
                                else
                                    strReturnValue = "";
                                break;
                            case "MNEMONICS":
                                int intMnemonic = 0;
                                if (ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString() != "")
                                {
                                    Int32.TryParse(ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString(), out intMnemonic);
                                    if (intMnemonic != 0)
                                        strReturnValue = oMnemonic.Get(intMnemonic, "factory_code") + " - " + oMnemonic.Get(intMnemonic, "name");
                                }
                                else
                                    strReturnValue = "";
                                break;
                            case "CHECKBOX":
                                strReturnValue = (ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString() == "1" ? "Yes" : "No");
                                break;
                            case "DROPDOWN":
                                strReturnValue = ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString();
                                break;
                            case "CHECKLIST":
                                strValues = ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString().Split(strSplit);
                                for (int ii = 0; ii < strValues.Length; ii++)
                                {
                                    if (strValues[ii].Trim() != "")
                                        strReturnValue += strValues[ii].Trim() + "~";
                                }
                                break;
                            case "RADIOLIST":
                                strReturnValue = ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString();
                                break;
                            case "LISTBOX":
                                strValues = ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString().Split(strSplit);
                                for (int ii = 0; ii < strValues.Length; ii++)
                                {
                                    if (strValues[ii].Trim() != "")
                                        strReturnValue += strValues[ii].Trim() + "~";
                                }
                                break;
                            case "LIST":
                                strValues = ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString().Split(strSplit);
                                for (int ii = 0; ii < strValues.Length; ii++)
                                {
                                    if (strValues[ii].Trim() != "")
                                        strReturnValue += strValues[ii].Trim() + "~";
                                }
                                break;
                            case "SERVERLIST":
                                strValues = ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString().Split(strSplit);
                                for (int ii = 0; ii < strValues.Length; ii++)
                                {
                                    int intServerList = 0;
                                    if (strValues[ii].Trim() != "" && Int32.TryParse(strValues[ii].Trim(), out intServerList))
                                    {
                                        if (intServerList != 0)
                                            strReturnValue += oServer.Get(intServerList, "servername") + "~";
                                    }
                                }
                                break;
                            case "HYPERLINK":
                                strReturnValue = "<a href=\"" + ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString() + "\" target=\"_blank\" title=\"Click here to open this link\">" + ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString() + "</a>";
                                break;
                            case "FILEUPLOAD":
                                if (ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString() != "")
                                    strReturnValue = "<input type=\"button\" title=\"Click here to view the file\" onclick=\"OpenFileUpload('" + ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString().Replace("\\", "\\\\") + "');\" value=\"View File\">";
                                else
                                    strReturnValue = "A file was not uploaded";
                                break;
                            case "DISCLAIMER":
                                strReturnValue = (ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString() == "1" ? "Yes" : "No");
                                break;
                            case "LOCATION":
                                int intLocation = 0;
                                if (ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString() != "")
                                {
                                    Int32.TryParse(ds.Tables[0].Rows[0][drForm["dbfield"].ToString()].ToString(), out intLocation);
                                    if (intLocation != 0)
                                        strReturnValue = oLocation.GetFull(intLocation);
                                }
                                else
                                    strReturnValue = "";
                                break;
                        }
                        //if (strReturnValue == "" && intServiceCompare == _serviceid)
                        if (strReturnValue == "")
                            strReturnValue = "<a href=\"javascript:alert('There is no response for one of the following reasons\\n\\n - This field is not required\\n - This field was added after this request was submitted\\n - The format (field type) of this field has changed\\n\\nIf you feel this message is incorrect, please contact your ClearView administrator.');\">No Response Provided</a>";
                        if (strReturnValue != "")
                        {
                            sbReturn.Append("<tr><td colspan=\"2\">");
                            sbReturn.Append(drForm["question"].ToString());
                            sbReturn.Append("</td></tr>");
                        }
                        char[] strReturnSplit = { '~' };
                        string[] strReturnValues = strReturnValue.Split(strReturnSplit);
                        for (int ii = 0; ii < strReturnValues.Length; ii++)
                        {
                            if (strReturnValues[ii].Trim() != "")
                            {
                                sbReturn.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"50\" height=\"1\"/></td><td>");
                                sbReturn.Append(strReturnValues[ii].Trim());
                                sbReturn.Append("</td></tr>");
                            }
                        }
                    }
                }
            }
            return sbReturn.ToString();
        }
        public void AlterTable(int _serviceid)
        {
            bool boolExists = false;
            string strTable = "set_GEN_" + _serviceid.ToString();
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select name from sys.tables order by name");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (strTable == dr["name"].ToString())
                {
                    boolExists = true;
                    break;
                }
            }
            string strSQL = "";
            ds = GetConfigs(_serviceid, -1, 1);
            if (boolExists == false)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["editable"].ToString() == "1")   // Only change fields that are stored (because they are editable)
                        strSQL += ",[" + dr["dbfield"].ToString() + "] [" + dr["type"].ToString() + "] NULL";
                }
                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "CREATE TABLE [" + strTable + "]([requestid] [int] NULL,[serviceid] [int] NULL,[number] [int] NULL,[modified] [datetime] NULL" + strSQL + ") ON [PRIMARY]");
            }
            else
            {
                DataSet dsFields = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select c.name, c.max_length AS length, t.name AS type from sys.columns c LEFT OUTER JOIN sys.types t ON t.system_type_id = c.system_type_id AND t.user_type_id <= 255 where c.object_id =object_id('" + strTable + "')");
                //// Check for ones to be deleted
                //foreach (DataRow drField in dsFields.Tables[0].Rows)
                //{
                //    if (drField["name"].ToString() != "requestid" && drField["name"].ToString() != "serviceid" && drField["name"].ToString() != "number" && drField["name"].ToString() != "modified")
                //    {
                //        bool boolDelete = true;
                //        foreach (DataRow dr in ds.Tables[0].Rows)
                //        {
                //            if (dr["editable"].ToString() == "1" && drField["name"].ToString() == dr["dbfield"].ToString())
                //            {
                //                boolDelete = false;
                //                break;
                //            }
                //        }
                //        //if (boolDelete == true)
                //        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "ALTER TABLE [" + strTable + "] DROP COLUMN [" + drField["name"].ToString() + "]");
                //    }
                //}
                // Check for ones to be added
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["editable"].ToString() == "1")   // Only add the ones that need to be saved (because they are editable)
                    {
                        bool boolFound = false;
                        foreach (DataRow drField in dsFields.Tables[0].Rows)
                        {
                            if (drField["name"].ToString() == dr["dbfield"].ToString())
                            {
                                boolFound = true;
                                break;
                            }
                        }
                        if (boolFound == false)
                        {
                            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "ALTER TABLE [" + strTable + "] ADD [" + dr["dbfield"].ToString() + "] " + dr["type"].ToString());
                            // Because of alter, get the latest fields (so we don't try to add the same one twice)
                            dsFields = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select c.name, c.max_length AS length, t.name AS type from sys.columns c LEFT OUTER JOIN sys.types t ON t.system_type_id = c.system_type_id AND t.user_type_id <= 255 where c.object_id =object_id('" + strTable + "')");
                        }
                    }
                }
            }
        }

        public void DeleteRequestData(string strTable, int intRequest, int intService, int intNumber)
        {
            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "DELETE FROM " + strTable + " WHERE requestid = " + intRequest.ToString() + " AND serviceid = " + intService.ToString() + " AND number = " + intNumber.ToString());
        }

        public void AddRequestData(string strSQL, int intRequest, int intService, int intNumber, SqlParameter[] arParamsSQL, DateTime modified)
        {
            arParamsSQL[0] = new SqlParameter("@requestid", intRequest);
            arParamsSQL[1] = new SqlParameter("@serviceid", intService);
            arParamsSQL[2] = new SqlParameter("@number", intNumber);
            arParamsSQL[3] = new SqlParameter("@modified", modified);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, strSQL, arParamsSQL);
        }

        public void AddConfigWorkflow(int _serviceid, int _nextservice, int _configid, int _editable)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@nextservice", _nextservice);
            arParams[2] = new SqlParameter("@configid", _configid);
            arParams[3] = new SqlParameter("@editable", _editable);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_addConfigWorkflow", arParams);
        }
        public void DeleteConfigWorkflows(int _serviceid, int _nextservice)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@nextservice", _nextservice);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_deleteConfigsWorkflow", arParams);
        }
        public DataSet GetConfigsWorkflow(int _serviceid, int _nextservice)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@nextservice", _nextservice);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getConfigsWorkflow", arParams);
        }
        public DataSet GetConfigWorkflows(int _configid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@configid", _configid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getConfigsWorkflows", arParams);
        }
        public DataSet GetConfigWorkflowsNext(int _nextservice)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@nextservice", _nextservice);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getConfigsWorkflowsNext", arParams);
        }

        public void AddConfigWorkflowShared(int _dbfield_read)
        {
            int intDB = AddFieldDB();
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@dbfield_read", _dbfield_read);
            arParams[1] = new SqlParameter("@dbfield_write", intDB);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_addConfigWorkflowShared", arParams);
        }
        public void DeleteConfigWorkflowShared(int _dbfield_read)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@dbfield_read", _dbfield_read);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_deleteConfigWorkflowShared", arParams);
        }
        public DataSet GetConfigsWorkflowShared(int _dbfield)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@dbfield", _dbfield);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getConfigsWorkflowShared", arParams);
        }


        public string SaveForm(HttpRequest _request, int intRequest, int intService, int intNumber, bool _wm, int _environment, string _dsn)
        {
            string strRequired = "";
            DataSet dsData = GetRequestData(intRequest, intService, intNumber, (_wm ? 1 : 0), _dsn);
            Variables oVariable = new Variables(_environment);
            string strUpload = oVariable.UploadsFolder() + "REQUESTS\\";
            string strTable = "set_GEN_" + intService.ToString();
            string strSQL1 = "";
            string strSQL2 = "";
            string strSQL3 = "";
            DataSet ds = GetConfigs(intService, (_wm == true ? 1 : 0), 1);
            int intParameters = 4;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["editable"].ToString() == "1")
                    intParameters++;
            }
            SqlParameter[] arParams = new SqlParameter[intParameters];
            intParameters = 4;
            int intFileCount = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                bool boolRequired = (dr["required"].ToString() == "1");
                if (dr["editable"].ToString() == "1")
                {
                    string strField = dr["dbfield"].ToString();
                    string strRequestField = strField + "_" + (_wm ? "1" : "0") + "_0";
                    string strCode = dr["code"].ToString();
                    string strValue = _request.Form[strRequestField];
                    bool boolNULL = false;
                    bool boolSkip = false;
                    switch (strCode.ToUpper())
                    {
                        case "USERS":
                        case "SERVERS":
                        case "MNEMONICS":
                        case "APPROVER":
                            if (strValue == null)
                                strValue = "0";
                            if (strRequired == "" && boolRequired && strValue == "0")
                                strRequired = IsIncomplete(Int32.Parse(dr["id"].ToString()), dsData);
                            break;
                        case "CHECKBOX":
                            if (strValue == null)
                                strValue = "0";
                            else
                                strValue = "1";
                            break;
                        case "DATE":
                            if (strValue == null || strValue == "")
                                boolNULL = true;
                            if (strRequired == "" && boolRequired && (strValue == null || strValue == ""))
                                strRequired = IsIncomplete(Int32.Parse(dr["id"].ToString()), dsData);
                            break;
                        case "DROPDOWN":
                            if (strRequired == "" && boolRequired && (strValue == null || strValue == "0"))
                                strRequired = IsIncomplete(Int32.Parse(dr["id"].ToString()), dsData);
                            break;
                        case "FILEUPLOAD":
                            HttpPostedFile oFile = (HttpPostedFile)_request.Files[intFileCount];
                            if (oFile != null && oFile.FileName != "")
                            {
                                if (Directory.Exists(strUpload) == false)
                                    Directory.CreateDirectory(strUpload);

                                string strExtension = Path.GetExtension(oFile.FileName);
                                // Get Extension
                                //string strExtension = oFile.FileName.Substring(oFile.FileName.LastIndexOf("."));
                                // Get Filenaming
                                string strFilename = strUpload + intRequest.ToString() + "_" + intService.ToString() + "_" + intNumber.ToString() + "_" + strField;
                                // Delete Previous File(s)
                                //string[] strFilesToDelete = Directory.GetFiles(strFilename + "*.*");
                                //foreach (string strFileToDelete in strFilesToDelete)
                                //    File.Delete(strFileToDelete);
                                // Add Extension
                                strFilename += strExtension;
                                // Upload and Save File
                                oFile.SaveAs(strFilename);
                                strValue = strFilename;
                            }
                            else
                            {
                                // Clear the previously uploaded file
                                string strClear = _request.Form[strRequestField + "_CLEAR"];
                                if (strClear == "on")
                                    strValue = "";
                                else
                                {
                                    boolSkip = true;
                                    if (dsData.Tables[0].Rows.Count > 0 && dsData.Tables[0].Rows[0][strField].ToString() != "")
                                    {
                                        // File previously uploaded
                                        strValue = dsData.Tables[0].Rows[0][strField].ToString();
                                    }
                                }
                            }
                            intFileCount++;
                            if (strRequired == "" && boolRequired && (strValue == null || strValue == "") && (oFile == null || oFile.FileName == ""))
                                strRequired = IsIncomplete(Int32.Parse(dr["id"].ToString()), dsData);
                            break;
                        case "DISCLAIMER":
                            if (strValue == "on")
                                strValue = "1";
                            else
                                strValue = "0";
                            break;
                        case "LOCATION":
                            if (strValue == null)
                                strValue = "0";
                            if (strRequired == "" && boolRequired && strValue == "0")
                                strRequired = IsIncomplete(Int32.Parse(dr["id"].ToString()), dsData);
                            break;
                        default:
                            if (strValue == null)
                                strValue = "";
                            if (strRequired == "" && boolRequired && strValue == "")
                                strRequired = IsIncomplete(Int32.Parse(dr["id"].ToString()), dsData);
                            break;
                    }
                    if (boolSkip == false)
                    {
                        strSQL1 += " ,[" + strField + "]";
                        strSQL2 += " ,@" + strField;
                        strSQL3 += "[" + strField + "] = @" + strField + ", ";
                        if (boolNULL == true)
                            arParams[intParameters] = new SqlParameter("@" + strField, SqlDateTime.Null);
                        else
                            arParams[intParameters] = new SqlParameter("@" + strField, strValue);
                        intParameters++;
                    }
                }
            }
            string strSQL = "";
            DataSet dsWM = GetRequestData(intRequest, intService, intNumber, (_wm ? 1 : 0), _dsn);
            if (dsWM.Tables[0].Rows.Count == 0)
            {
                // Came from Service Request, so delete all prior data
                strSQL = "INSERT INTO " + strTable + "([requestid], [serviceid], [number], [modified]" + strSQL1 + ") VALUES (@requestid, @serviceid, @number, @modified" + strSQL2 + ")";
            }
            else
            {
                // Came from workload manager, so keep data and perform update
                strSQL = "UPDATE " + strTable + " SET " + strSQL3 + "[modified] = getdate() WHERE [requestid] = @requestid AND [serviceid] = @serviceid AND [number] = @number";
            }
            AddRequestData(strSQL, intRequest, intService, intNumber, arParams, DateTime.Now);
            return strRequired;
        }
        public string IsIncomplete(int _configid, DataSet _data)
        {
            string strInvalid = GetConfig(_configid, "question");  // By default, it is already invalid.  Only way to make it valid would be to find this is a hidden field.
            // Check Affects, and make sure affected response(s) are selected.
            DataSet dsAffects = GetConfigAffectsConfig(_configid);
            foreach (DataRow drAffects in dsAffects.Tables[0].Rows)
            {
                int intValue = Int32.Parse(drAffects["valueid"].ToString());
                string strValue = GetConfigValue(intValue, "value");    // "Yes"
                int intParent = Int32.Parse(GetConfigValue(intValue, "configid"));
                string strField = GetConfig(intParent, "dbfield");
                // Check the value of the field
                if (_data.Tables[0].Columns.Contains(strField) == false)
                {
                    // The column doesn't exist...nothing to compare.
                    strInvalid = "";
                }
                else if (_data.Tables[0].Rows.Count > 0 && _data.Tables[0].Rows[0][strField].ToString() != strValue)
                {
                    // The response that causes this selection to be shown was not selected.  Validation can be skipped.
                    strInvalid = "";
                }
            }
            return strInvalid;
        }
        public void AddConfigAffect(int _configid, int _valueid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@configid", _configid);
            arParams[1] = new SqlParameter("@valueid", _valueid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_addConfigAffect", arParams);
        }
        public void DeleteConfigAffects(int _configid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@configid", _configid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_deleteConfigAffects", arParams);
        }
        public DataSet GetConfigAffects(int _configid, int _valueid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@configid", _configid);
            arParams[1] = new SqlParameter("@valueid", _valueid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getConfigAffects", arParams);
        }
        public DataSet GetConfigAffectsConfig(int _configid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@configid", _configid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getConfigAffectsConfig", arParams);
        }
        public DataSet GetConfigAffectsValue(int _valueid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@valueid", _valueid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getConfigAffectsValue", arParams);
        }

        public DataSet GetApprovals(int _serviceid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getApprovals", arParams);
        }



        public int AddWorkflowCondition(string _name, int _serviceid, int _nextservice, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@nextservice", _nextservice);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            arParams[4] = new SqlParameter("@id", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_addWorkflowCondition", arParams);
            return Int32.Parse(arParams[4].Value.ToString());
        }
        public void UpdateWorkflowCondition(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_updateWorkflowCondition", arParams);
        }
        public void EnableWorkflowCondition(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_updateWorkflowConditionEnabled", arParams);
        }
        public void DeleteWorkflowCondition(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_deleteWorkflowCondition", arParams);
        }
        public DataSet GetWorkflowCondition(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getWorkflowCondition", arParams);
        }
        public string GetWorkflowCondition(int _id, string _column)
        {
            DataSet ds = GetWorkflowCondition(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetWorkflowConditions(int _serviceid, int _nextservice, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@nextservice", _nextservice);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getWorkflowConditions", arParams);
        }

        public void AddWorkflowConditionValue(int _conditionid, int _valueid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@conditionid", _conditionid);
            arParams[1] = new SqlParameter("@valueid", _valueid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_addWorkflowConditionValue", arParams);
        }
        public void DeleteWorkflowConditionValue(int _conditionid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@conditionid", _conditionid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "sep_deleteWorkflowConditionValue", arParams);
        }
        public DataSet GetWorkflowConditionValue(int _conditionid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@conditionid", _conditionid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getWorkflowConditionValue", arParams);
        }
        public DataSet GetWorkflowConditionValues(int _conditionid, int _requestid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@conditionid", _conditionid);
            arParams[1] = new SqlParameter("@requestid", _requestid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getWorkflowConditionValues", arParams);
        }
        public bool IsWorkflowConditionValue(int _conditionid, int _valueid)
        {
            bool boolReturn = false;
            DataSet ds = GetWorkflowConditionValue(_conditionid);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["valueid"].ToString()) == _valueid)
                {
                    boolReturn = true;
                    break;
                }
            }
            return boolReturn;
        }
        public DataSet GetWorkflow(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getWorkflowRequest", arParams);
        }
        public DataSet GetWorkflow(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sep_getWorkflowsRequest", arParams);
        }

    }
}
