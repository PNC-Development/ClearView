using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Net.Mail;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls;
using System.Net.NetworkInformation;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Data.SqlTypes;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Web.UI;
using System.Collections.Generic;

namespace NCC.ClearView.Application.Core
{
	public class Functions
	{
        [DllImport("netapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int NetJoinDomain(string lpServer, string lpDomain, string lpAccountOU, string lpAccount, string lpPassword, int fJoinOptions);

        private string dsn = "";
        private string strDelegates = "";
		private int user = 0;
		private int intEnvironment;
		private SqlParameter[] arParams;
        private Variables oVariable;
        private Log oLog;
        private int intDefaultSwitchTimeout = 1000;
        private int intDefaultSwitchPort = 23;
        private int intSwitchMaxLoops = 100;
        private string strSwitchBreak = "\r\n";
        private string strSwitchReturn = "<br/>";

        public Functions(int _user, string _dsn, int _environment)
		{
			user = _user;
			dsn = _dsn;
            intEnvironment = _environment;
            oVariable = new Variables(intEnvironment);
            oLog = new Log(user, dsn);
		}
        public SearchResultCollection eDirectory(string _search_by, string _search_for)
        {
            //string LDAP = "LDAP://" + oVariable.eDirectoryHost() + ":636/ou=People,o=pnc";    // switch to using non-SSL due to issues with connection
            string LDAP = "LDAP://" + oVariable.eDirectoryHost() + ":389/ou=People,o=pnc";
            bool boolLog = false;
            DataSet dsKey = GetSetupValuesByKey("LOGGING_EDIRECTORY");
            if (dsKey.Tables[0].Rows.Count > 0 && dsKey.Tables[0].Rows[0]["Value"].ToString() == "1")
                boolLog = true;
            AuthenticationTypes eDir = eDirectory(boolLog);
            DirectorySearcher oSearcher = new DirectorySearcher(new DirectoryEntry(LDAP, oVariable.eDirectoryUsername(), oVariable.eDirectoryPassword(), eDir), "(" + _search_by + "=" + _search_for + ")");
            if (boolLog)
            {
                Log oLog = new Log(0, dsn);
                oLog.AddEvent("LDAP", _search_for, LDAP + " (" + eDir.ToString() + ") using " + oVariable.eDirectoryUsername(), LoggingType.Debug);
            }
            return oSearcher.FindAll();
        }
        public AuthenticationTypes eDirectory(bool log)
        {
            AuthenticationTypes auth = AuthenticationTypes.Encryption;
            DataSet dsKey = GetSetupValuesByKey("EDIRECTORY_AUTH");
            if (dsKey.Tables[0].Rows.Count > 0)
            {
                string value = dsKey.Tables[0].Rows[0]["Value"].ToString();
                if (log)
                {
                    Log oLog = new Log(0, dsn);
                    oLog.AddEvent("LDAP", "EDIRECTORY_AUTH", value, LoggingType.Debug);
                }
                if (value.ToUpper() == "SERVERBIND")
                    auth = AuthenticationTypes.ServerBind;
                else if (value.ToUpper() == "ENCRYPTION")
                    auth = AuthenticationTypes.Encryption;
                else
                    auth = AuthenticationTypes.SecureSocketsLayer;
            }
            else
                auth = AuthenticationTypes.Encryption;
            return auth;
        }
        public SearchResultCollection eDirectory(string _lan_user_account)
        {
            return eDirectory("cn", _lan_user_account);
        }
        public string eDirectory(DirectoryEntry entry, string property)
        {
            if (entry.Properties.Contains(property))
            {
                object value = entry.Properties[property].Value;
                if (value.ToString() == "System.Byte[]")
                {
                    byte[] bytearray = (byte[])value;
                    return Encoding.Default.GetString(bytearray);
                }
                else
                    return value.ToString();
            }
            else
                return "";
        }
        public string eDirectory(SearchResult entry, string property)
        {
            if (entry.Properties.Contains(property))
            {
                object value = entry.Properties[property][0];
                if (value.ToString() == "System.Byte[]")
                {
                    byte[] bytearray = (byte[])value;
                    return Encoding.Default.GetString(bytearray);
                }
                else
                    return value.ToString();
            }
            else
                return "";
        }
        public void ExecuteNonQuery(string _commandText)
        {
            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, _commandText);
        }
        public DataSet ExecuteDataset(string _commandText)
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.Text, _commandText);
        }
        public void ExecuteSQL(string _value)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@value", _value);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "xxx_UpdateTextValue", arParams);
        }
        public void SendEmail(string _title, string _to, string _cc, string _bcc, string _subject, string _body, bool _ad, bool _text)
        {
            SendEmail(_title, _to, _cc, _bcc, _subject, _body, _ad, _text, "");
        }

        //Modified Code by Vijay
        public void SendEmail(string _title, string _to, string _cc, string _bcc, string _subject, string _body, bool _ad, bool _text, string _strpath)
        {
            bool Testing = false;
            if (intEnvironment != (int)CurrentEnvironment.CORPDMN && intEnvironment != (int)CurrentEnvironment.PNCNT_PROD) // Not Production
            {
                Testing = true;
                _subject = ">>DEV<< " + _subject;
            }

            strDelegates = "";
            Users oUser = new Users(user, dsn);
            MailMessage oMessage = new MailMessage();
            MailAddress oAddress = new MailAddress(oVariable.FromEmail());
            oMessage.From = oAddress;
            List<string> AddressedTo = new List<string>();
            List<string> Copied = new List<string>();
            List<string> Delegates = new List<string>();
            StringBuilder body = new StringBuilder();
            string[] strEmail;
            char[] strSplit = { ';' };
            strEmail = _to.Split(strSplit);
            bool boolSend = false;
            for (int ii = 0; ii < strEmail.Length; ii++)
            {
                if (strEmail[ii].Trim() != "")
                {
                    string strAddress = strEmail[ii].Replace(System.Environment.NewLine, "");
                    int intUser = oUser.GetId(strAddress);
                    if (_ad == true && strAddress.Contains("@") == false)
                    {
                        AddressedTo.Add(oUser.GetFullNameWithLanID(intUser));
                        strAddress = oUser.GetEmail(strAddress, intEnvironment);
                    }
                    else
                        AddressedTo.Add(strAddress);
                    Delegates.AddRange(AddDelegates(intUser, strSplit[0].ToString()));
                    if (strAddress != "")
                    {
                        boolSend = true;
                        if (Testing == false)
                            oAddress = new MailAddress(strAddress);
                        else
                            oAddress = new MailAddress("PT43054@PNC.COM");
                        oMessage.To.Add(oAddress);
                    }
                    else
                        oLog.Add("ERROR: Invalid TO Email Address for " + strEmail[ii]);
                }
            }
            strEmail = _cc.Split(strSplit);
            for (int ii = 0; ii < strEmail.Length; ii++)
            {
                if (strEmail[ii].Trim() != "")
                {
                    string strAddress = strEmail[ii].Replace(System.Environment.NewLine, "");
                    int intUser = oUser.GetId(strAddress);
                    if (_ad == true && strAddress.Contains("@") == false)
                    {
                        Copied.Add(oUser.GetFullNameWithLanID(intUser));
                        strAddress = oUser.GetEmail(strAddress, intEnvironment);
                    }
                    else
                        Copied.Add(strAddress);
                    Delegates.AddRange(AddDelegates(intUser, strSplit[0].ToString()));
                    if (strAddress != "")
                    {
                        if (Testing == false)
                        {
                            boolSend = true;
                            oAddress = new MailAddress(strAddress);
                            oMessage.CC.Add(oAddress);
                        }
                    }
                    else
                        oLog.Add("ERROR: Invalid CC Email Address for " + strEmail[ii]);
                }
            }
            strEmail = strDelegates.Split(strSplit);
            for (int ii = 0; ii < strEmail.Length; ii++)
            {
                if (strEmail[ii].Trim() != "")
                {
                    string strAddress = strEmail[ii].Replace(System.Environment.NewLine, "");
                    if (_ad == true && strAddress.Contains("@") == false)
                        strAddress = oUser.GetEmail(strAddress, intEnvironment);
                    if (strAddress != "")
                    {
                        if (Testing == false)
                        {
                            oAddress = new MailAddress(strAddress);
                            oMessage.To.Add(oAddress);
                        }
                    }
                    else
                        oLog.Add("ERROR: Invalid TO (Delegate) Email Address for " + strEmail[ii]);
                }
            }
            strEmail = _bcc.Split(strSplit);
            for (int ii = 0; ii < strEmail.Length; ii++)
            {
                if (strEmail[ii].Trim() != "")
                {
                    string strAddress = strEmail[ii].Replace(System.Environment.NewLine, "");
                    if (strAddress.Contains("@") == false)
                        strAddress = oUser.GetEmail(strAddress, intEnvironment);
                    if (strAddress != "")
                    {
                        if (Testing == false)
                        {
                            boolSend = true;
                            oAddress = new MailAddress(strAddress);
                            oMessage.Bcc.Add(oAddress);
                        }
                    }
                    else
                        oLog.Add("ERROR: Invalid BCC Email Address for " + strEmail[ii]);
                }
            }
            if (boolSend == true)
            {
                oMessage.Subject = _subject;
                if (_text == true)
                {
                    body.AppendLine("The ClearView Community is now available! Click the following link to join:");
                    body.AppendLine(oVariable.Community());
                    body.AppendLine("----------------------------------------------------------------------------");
                    body.AppendLine(_body);
                    oMessage.Body = body.ToString();
                }
                else
                {
                    oMessage.IsBodyHtml = true;
                    body.AppendLine("<html><body style=\"" + oVariable.DefaultFontStyle() + "\">");
                    body.AppendLine(@"<p><img src='" + oVariable.DocumentsFolder() + "logo.gif' style='border: 0px' /></p>");
                    // Banner - Start
                    //body.AppendLine("<p style=\"border:solid 2px #81a4c6; background-color: #f0f6fd; padding: 5px;\">");
                    //body.AppendLine("<b style=\"font-size:12px;\">The ClearView Community is now available! Click the following link to join:</b>");
                    //body.AppendLine("<br/><br/>");
                    //body.AppendLine("<a href=\"" + oVariable.Community() + "\">" + oVariable.Community() + "</a>");
                    //body.AppendLine("</p>");
                    // Banner - End
                    body.AppendLine("<blockquote>");
                    body.AppendLine(_body);
                    // Addresses - Start
                    body.AppendLine("<p><b>This message is addressed to the following person(s):</b><br />");
                    body.AppendLine(string.Join(";", AddressedTo.ToArray()));
                    body.AppendLine("</p>");
                    if (Delegates.Count > 0)
                    {
                        body.AppendLine("<p><b>The following person(s) are being notified as delegates:</b><br />");
                        body.AppendLine(string.Join(";", Delegates.ToArray()));
                        body.AppendLine("</p>");
                    }
                    if (Copied.Count > 0)
                    {
                        body.AppendLine("<p><b>The following person(s) have been copied for informational purposes:</b><br />");
                        body.AppendLine(string.Join(";", Copied.ToArray()));
                        body.AppendLine("</p>");
                    }
                    // Addresses - End
                    //body.AppendLine("<p><b>Subject:</b><br />");
                    //body.AppendLine(_title);
                    //body.AppendLine("</p>");
                    //body.AppendLine("<b>Message:</b><br />");
                    body.AppendLine("<p>The <a href=\"" + oVariable.Community() + "\" target=\"_blank\"/>ClearView Community</a> is now available!  Join today to learn more about the tool and get all your questions answered.</p>");
                    body.AppendLine("<p><b>NOTE:</b> This is automated email sent from the ClearView application. Please do not respond to this message.</p>");
                    body.AppendLine("</blockquote>");
                    body.AppendLine("</body></html>");
                    oMessage.Body = body.ToString();
                }
                if (_strpath != "")
                {
                    strEmail = _strpath.Split(';');
                    for (int ii = 0; ii < strEmail.Length; ii++)
                    {
                        if (strEmail[ii].Trim() != "")
                        {
                            Attachment attachement = new Attachment(strEmail[ii]);
                            oMessage.Attachments.Add(attachement);
                        }
                    }
                }
                try
                {
                    SmtpClient oClient = new SmtpClient(oVariable.SmtpServer());
                    oClient.Send(oMessage);
                }
                catch (Exception eMail)
                {
                }
            }
        }

        private List<string> AddDelegates(int _userid, string _split)
        {
            List<string> delegates = new List<string>();
            Users oUser = new Users(user, dsn);
            Delegates oDelegate = new Delegates(user, dsn);
            if (_userid > 0)
            {
                DataSet dsDelegate = oDelegate.Gets(_userid);
                foreach (DataRow drDelegate in dsDelegate.Tables[0].Rows)
                {
                    int intDelegate = Int32.Parse(drDelegate["delegate"].ToString());
                    delegates.Add(oUser.GetFullNameWithLanID(intDelegate));
                    string strDelegatePNC = oUser.Get(intDelegate, "pnc_id");
                    if (strDelegatePNC != "")
                    {
                        if (strDelegates.Contains(strDelegatePNC) == false)
                        {
                            if (strDelegates.EndsWith(_split) == false)
                                strDelegates += _split;
                            strDelegates += strDelegatePNC;
                        }
                    }
                    else
                    {
                        string strDelegateXID = oUser.Get(intDelegate, "xid");
                        if (strDelegateXID != "")
                        {
                            if (strDelegates.Contains(strDelegateXID) == false)
                            {
                                if (strDelegates.EndsWith(_split) == false)
                                    strDelegates += _split;
                                strDelegates += strDelegateXID;
                            }
                        }
                    }
                }
            }
            return delegates;
        }

        public string GetRandomString(int _length)
        {
            string strResult = System.Guid.NewGuid().ToString();
            strResult = strResult.Replace("-", string.Empty);
            if (_length <= 0 || _length > strResult.Length)
                throw new ArgumentException("Length must be between 1 and " + strResult.Length);
            return strResult.Substring(0, _length);
        }
        public bool ValidateLeadTime(int _priority, DateTime _date)
        {
            DateTime _now = DateTime.Today;
            switch (_priority)
            {
                case 1:
                    _now = _now.AddDays(14);
                    if (_now < _date)
                        return false;
                    else
                        return true;
                case 2:
                    _now = _now.AddDays(28);
                    if (_now < _date)
                        return false;
                    else
                        return true;
                case 3:
                    _now = _now.AddMonths(4);
                    if (_now < _date)
                        return false;
                    else
                        return true;
                case 4:
                    _now = _now.AddMonths(6);
                    if (_now < _date)
                        return false;
                    else
                        return true;
                case 5:
                    _now = _now.AddYears(1);
                    if (_now < _date)
                        return false;
                    else
                        return true;
                default:
                    return true;
            }
        }
        public string encryptQueryString(string strQueryString)
        {
            return (Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(strQueryString)));
        }

        public string decryptQueryString(string strQueryString)
        {
            string strReturn = "";
            try
            {
                strQueryString = strQueryString.Replace(' ', '+');
                strReturn = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(strQueryString));
            }
            catch { }
            return strReturn;
        }

        public void Alert(Page _page, string _querystring, string _message, string _title)
        {
            if (_page.Request.QueryString[_querystring] != null && _page.Request.QueryString[_querystring] != "")
                _page.ClientScript.RegisterStartupScript(_page.GetType(), _querystring + "_alert", "<script type=\"text/javascript\">AlertWindow('" + (_message == null ? "null" : encryptQueryString(_message)) + "','" + (_title == null ? "null" : encryptQueryString(_title)) + "');<" + "/" + "script>");
        }

        public int JoinDomain(string _computer, string _domain, string _username, string _password)
        {
            return NetJoinDomain(_computer, _domain, null, _username, _password, 0x00000001);
        }

        public void ConfigureToolButton(ImageButton _button, string _path)
        {
            _button.Attributes.Add("onmouseover", "SwapImageOnly(this, '" + _path + "_over.gif');");
            _button.Attributes.Add("onmouseout", "SwapImageOnly(this, '" + _path + ".gif');");
        }
        public void ConfigureToolButtonRO(ImageButton _save, ImageButton _complete)
        {
            if (_save != null)
            {
                _save.ImageUrl = "/images/tool_save_dbl.gif";
                _save.Enabled = false;
            }
            if (_complete != null)
            {
                _complete.ImageUrl = "/images/tool_complete_dbl.gif";
                _complete.Enabled = false;
            }
        }

        public string CreateBox(string _image_prefix, string _text, string _class)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
            sb.Append("<tr>");
            sb.Append("<td><img src=\"/images/");
            sb.Append(_image_prefix);
            sb.Append("top_left.gif\" border=\"0\"></td>");
            sb.Append("<td background=\"/images/");
            sb.Append(_image_prefix);
            sb.Append("top.gif\"></td>");
            sb.Append("<td><img src=\"/images/");
            sb.Append(_image_prefix);
            sb.Append("top_right.gif\" border=\"0\"></td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td background=\"/images/");
            sb.Append(_image_prefix);
            sb.Append("left.gif\"><img src=\"/images/");
            sb.Append(_image_prefix);
            sb.Append("left.gif\"></td>");
            sb.Append("<td class=\"");
            sb.Append(_class);
            sb.Append("\">");
            sb.Append(_text);
            sb.Append("</td>");
            sb.Append("<td background=\"/images/");
            sb.Append(_image_prefix);
            sb.Append("right.gif\"><img src=\"/images/");
            sb.Append(_image_prefix);
            sb.Append("right.gif\"></td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td><img src=\"/images/");
            sb.Append(_image_prefix);
            sb.Append("bottom_left.gif\" border=\"0\"></td>");
            sb.Append("<td background=\"/images/");
            sb.Append(_image_prefix);
            sb.Append("bottom.gif\"></td>");
            sb.Append("<td><img src=\"/images/");
            sb.Append(_image_prefix);
            sb.Append("bottom_right.gif\" border=\"0\"></td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            return sb.ToString();
        }

        public bool Ping(string _name, int _classid, int _environmentid)
        {
            OnDemandSending oOnDemandSending = new OnDemandSending(0, dsn);
            Settings oSetting = new Settings(user, dsn);
            int intTry = 0;
            int intMaxTries = 5;
            DataSet dsConfig = oOnDemandSending.GetConfig(_classid, _environmentid);
            if (dsConfig.Tables[0].Rows.Count == 0)
            {
                while (oSetting.Get("pinging") != "" && intTry < intMaxTries)
                {
                    System.Threading.Thread.Sleep(5000);
                    intTry++;
                }
                if (oSetting.Get("pinging") != "")
                {
                    // System still pinging...check date and time of last ping and if greater than an hour, reset it
                    DateTime datPing = DateTime.Parse(oSetting.Get("pinging"));
                    TimeSpan spnPing = DateTime.Now.Subtract(datPing);
                    if (spnPing.Hours > 1)
                    {
                        oSetting.UpdatePinging(false);
                        return Ping(_name, _classid, _environmentid);
                    }
                    else
                    {
                        // Skip the IP address for now and move on (so system does not hang)
                        string strEMailIdsTo = GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
                        if (strEMailIdsTo != "")
                            SendEmail("Pinging Warning", strEMailIdsTo, "", "", "Pinging Warning", "The PINGING field in the cv_settings table is currently in use and an IP address (" + _name + ") was skipped", false, true);
                        return true;
                    }
                }
                else
                {
                    oSetting.UpdatePinging(true);
                    Ping oPing = new Ping();
                    string strStatus = "";
                    try
                    {
                        PingReply oReply = oPing.Send(_name);
                        strStatus = oReply.Status.ToString().ToUpper();
                    }
                    catch { }
                    oSetting.UpdatePinging(false);
                    return (strStatus == "SUCCESS" ? true : false);
                }
            }
            else
            {
                string strName = dsConfig.Tables[0].Rows[0]["name"].ToString();
                string strDSN = "data source=" + strName + ";uid=cvanswer;password=v1jay;database=ClearViewAnswering";
                SqlHelper.ExecuteDataset(strDSN, CommandType.Text, "INSERT INTO cv_ondemand_answering VALUES(0, '" + _name + "', -1)");
                DataSet dsReply = SqlHelper.ExecuteDataset(strDSN, CommandType.Text, "SELECT * FROM cv_ondemand_answering WHERE name = '" + _name + "'");
                while (dsReply.Tables[0].Rows[0]["result"].ToString() == "-1" && intTry < intMaxTries)
                {
                    System.Threading.Thread.Sleep(5000);
                    intTry++;
                    dsReply = SqlHelper.ExecuteDataset(strDSN, CommandType.Text, "SELECT * FROM cv_ondemand_answering WHERE name = '" + _name + "'");
                }
                bool boolReturn = (intTry == intMaxTries || dsReply.Tables[0].Rows[0]["result"].ToString() == "1");
                SqlHelper.ExecuteNonQuery(strDSN, CommandType.Text, "DELETE FROM cv_ondemand_answering WHERE name = '" + _name + "'");
                return boolReturn;
            }
        }

        public string PingName(string _name)
        {
            string strIP = "";
            string[] oDomains = oVariable.PingDNS();
            for (int ii = 0; ii < oDomains.Length && strIP == ""; ii++)
            {
                Ping oPing = new Ping();
                PingReply oReply = oPing.Send("localhost");
                string strStatus = "";
                try
                {
                    oReply = oPing.Send(_name + oDomains[ii]);
                    strStatus = oReply.Status.ToString().ToUpper();
                }
                catch { }
                if (strStatus == "SUCCESS")
                {
                    strIP = oReply.Address.ToString();
                }
            }
            return strIP;
        }

        public string ProcessLine(string strLine, DataRow dr)
        {
            while (strLine.Contains("{") == true)
            {
                string strBefore = strLine.Substring(0, strLine.IndexOf("{"));
                string strVariable = strLine.Substring(strLine.IndexOf("{") + 1);
                string strAfter = strVariable.Substring(strVariable.IndexOf("}") + 1);
                strVariable = strVariable.Substring(0, strVariable.IndexOf("}"));
                strLine = strBefore + dr[strVariable].ToString() + strAfter;
            }
            while (strLine.Contains("@") == true)
            {
                string strBefore = strLine.Substring(0, strLine.IndexOf("@"));
                string strVariable = strLine.Substring(strLine.IndexOf("@") + 1);
                string strAfter = strVariable.Substring(strVariable.IndexOf("@") + 1);
                strVariable = strVariable.Substring(0, strVariable.IndexOf("@"));
                strLine = strBefore + ProcessVariable("@", strVariable) + strAfter;
            }
            return strLine;
        }
        private string ProcessVariable(string strType, string strVariable)
        {
            string strReturn = "UNKNOWN";
            if (strType == "@")
            {
                // Environment Variables
                if (strVariable.ToUpper() == "DOMAIN")
                    strReturn = oVariable.Domain();
                else if (strVariable.ToUpper() == "USER")
                    strReturn = oVariable.ADUser();
                else if (strVariable.ToUpper() == "PASSWORD")
                    strReturn = oVariable.ADPassword();
            }
            return strReturn;
        }
        public string BuildXmlString(string xmlRootName, string[] values)
        {
            StringBuilder xmlString = new StringBuilder();
            xmlString.AppendFormat("<{0}>", xmlRootName);
            for (int ii = 0; ii < values.Length; ii++)
                xmlString.AppendFormat("<value>{0}</value>", values[ii]);
            xmlString.AppendFormat("</{0}>", xmlRootName);
            return xmlString.ToString();
        }
        public string BuildXmlStringType(string _types)
        {
            char[] strTypeSplit = { ',' };
            string[] strServerTypes = _types.Split(strTypeSplit);
            return BuildXmlString("data", strServerTypes);
        }
        public string Replicate(string _text, int _times)
        {
            StringBuilder _return = new StringBuilder(_text.Length * _times);
            for (int ii = 0; ii < _times; ii++)
                _return.Append(_text);
            return _return.ToString();
        }
        public string Replicate(string _end_value, string _repeat_value, int _length)
        {
            int intLength = _end_value.Length;
            if (intLength < _length)
            {
                intLength = _length - intLength;
                _end_value = Replicate(_repeat_value, intLength) + _end_value;
            }
            return _end_value;
        }
        public int RandomNumber(int _min, int _max)
        {
            Random oRandom = new Random(DateTime.Now.Millisecond);
            return oRandom.Next(_min, _max);
        }
        public string FormatText(string _text)
        {
            while (_text.Contains("<br/>") == true)
                _text = _text.Replace("<br/>", Environment.NewLine);
            while (_text.Contains("<br>") == true)
                _text = _text.Replace("<br>", Environment.NewLine);
            //return "<textarea rows=\"1\" readonly=\"readonly\" style=\"" + oVariable.DefaultFontStyle() + ";border-width:0px;border-style:None;width:100%;overflow:visible\">" + _text + "</textarea>";

            while (_text.Contains(Environment.NewLine) == true)
                _text = _text.Replace(Environment.NewLine, "<br/>");
            while (_text.Contains("\r") == true)
                _text = _text.Replace("\r", "<br/>");
            while (_text.Contains("\n") == true)
                _text = _text.Replace("\n", "<br/>");
            return _text;
        }
        public string EmailComments(string _username, string _comments)
        {
            return EmailComments(_username, _comments, "", "", false);
        }
        public string EmailComments(string _username, string _comments, string _path, string _file)
        {
            return EmailComments(_username, _comments, _path, _file, false);
        }
        public string EmailComments(string _username, string _comments, string _path, string _file, bool _image)
        {
            string strBody = "";
            strBody += "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"3\" border=\"0\" align=\"center\" style=\"border:dotted 1px #779ccc;" + oVariable.DefaultFontStyle() + "\">";
            strBody += "<tr bgcolor=\"#CFE3FA\"><td colspan=\"2\"><span style=\"" + oVariable.DefaultFontStyleHeader() + "\">" + _username + "</span>&nbsp;&nbsp;added the following comments...</td></tr>";
            strBody += "<tr bgcolor=\"#FCC595\"><td height=\"1\"></td></tr>";
            if (_image)
                strBody += "<tr><td valign=\"top\"><img src=\"" + oVariable.ImageURL() + "/images/comment.gif\" align=\"absmiddle\" border=\"0\"/></td><td width=\"100%\">" + FormatText(_comments) + "</td></tr>";
            else
                strBody += "<tr><td colspan=\"2\" width=\"100%\">" + FormatText(_comments) + "</td></tr>";
            if (_path != "")
            {
                strBody += "<tr><td colspan=\"2\" style=\"border-bottom:dashed 1px #CCCCCC\">&nbsp;</td></tr>";
                if (_image)
                    strBody += "<tr><td valign=\"top\"><img src=\"" + oVariable.ImageURL() + "/images/file.gif\" align=\"absmiddle\" border=\"0\"/></td><td width=\"100%\"><a href=\"" + _path + "\" target=\"_blank\">" + _file + "</a></td></tr>";
                else
                    strBody += "<tr><td colspan=\"2\" width=\"100%\"><a href=\"" + _path + "\" target=\"_blank\">" + _file + "</a></td></tr>";
            }
            strBody += "</table>";
            return strBody;
        }
        public int CheckLogin(HttpRequest oRequest, HttpResponse oResponse, int intPage)
        {
            if (oRequest.Cookies["profileid"] != null && oRequest.Cookies["profileid"].Value != "")
                return Int32.Parse(oRequest.Cookies["profileid"].Value);
            else
            {
                Pages oPage = new Pages(0, dsn);
                string strRedirect = oPage.GetFullLink(intPage);
                oResponse.Redirect("/redirect.aspx?referrer=" + strRedirect);
                return 0;
            }
        }

        public DataSet GetSystemTableColumns(string _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@objectid", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSystemTableColumns", arParams);
        }
        public string GetRandom(int _Length)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@Length", _Length);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getRandom", arParams);
            if (o == null)
                return "";
            else
                return o.ToString();
        }
        

        public string ChangeVLAN(string _switch, string _serial, string _mac_address, string _interface, string _vlan, bool _ios, bool _ok_if_empty_mac, bool _ok_if_no_arp_lookup, bool _debug, bool _carriage_return_output)
        {
            if (_carriage_return_output == true)
                strSwitchReturn = strSwitchBreak;
            string strReturn = "";
            string strError = "";
            strReturn += "Connecting to " + _switch + strSwitchReturn;
            Ping oPing = new Ping();
            string strStatus = "";
            try
            {
                PingReply oReply = oPing.Send(_switch);
                strStatus = oReply.Status.ToString().ToUpper();
            }
            catch { }
            bool boolPinged = (strStatus == "SUCCESS");
            if (boolPinged == false)
            {
                strError = "Unable to connect to the switch..." + _switch + " (ping did not respond)";
            }
            else
            {
                //string strSerial = "USE749N7TR";
                //string strMACAddress = "00-1C-C4-EB-6C-E4";
                //string strInterface = "GI4/11";
                //string strVlan = "215";
                string strSerial = _serial;
                // Might have to get MAC Address from WebService "GetMacFromILO" function
                string strMACAddress = _mac_address;
                string strInterface = _interface;
                string strVlan = _vlan;

                if (strMACAddress != "")
                {
                    try
                    {
                        while (strMACAddress.Contains("-") == true)
                            strMACAddress = strMACAddress.Replace("-", "");
                        while (strMACAddress.Contains(":") == true)
                            strMACAddress = strMACAddress.Replace(":", "");
                        string strMAC1 = strMACAddress.Substring(0, 4);
                        string strMAC2 = strMACAddress.Substring(4, 4);
                        string strMAC3 = strMACAddress.Substring(8, 4);
                        strMACAddress = strMAC1 + "." + strMAC2 + "." + strMAC3;
                    }
                    catch
                    {
                        strError = "MAC Address is invalid (" + strMACAddress + ")";
                    }
                }

                if (strError == "")
                {
                    TelnetConnection telnet = new TelnetConnection(_switch, intDefaultSwitchPort);
                    string login = telnet.Login(oVariable.SwitchUsername(), oVariable.SwitchPassword(), intDefaultSwitchTimeout);

                    if (_debug == true)
                        strReturn += "DEBUG: " + ReadOutput(telnet, false) + strSwitchReturn;
                    telnet.WriteLine("en");
                    if (_debug == true)
                        strReturn += "DEBUG: " + ReadOutput(telnet, true) + strSwitchReturn;
                    telnet.WriteLine(oVariable.SwitchPassword());
                    if (_debug == true)
                        strReturn += "DEBUG: " + ReadOutput(telnet, true) + strSwitchReturn;

                    // Validate that the device you are about to change is actually the device
                    string strVlanCheck = "";
                    string strSerialCheck = "";
                    bool boolSerialMatch = false;
                    bool boolArpOK = false;
                    string strRunDebug = "";
                    if (_ios == true)
                    {
                        telnet.WriteLine("show run int " + strInterface);
                        string strRun = ReadOutput(telnet, true);
                        if (_debug == true)
                            strReturn += "DEBUG: " + strRun + strSwitchReturn;

                        strVlanCheck = ReadSwitchPort(strRun, "switchport access vlan");
                        strSerialCheck = ReadSwitchPort(strRun, "description");
                        strRunDebug = strRun;
                    }
                    else
                    {
                        strError = "Switchport modification only valid for IOS switches";
                    }

                    if (strError == "")
                    {
                        strReturn += "Current VLAN = " + strVlanCheck + strSwitchReturn;
                        strReturn += "Current Description (Serial) = " + strSerialCheck + strSwitchReturn;

                        bool boolOKtoChange = false;
                        if (strVlanCheck == strVlan)
                        {
                            // Already changed - nothing to do
                            strReturn += "Already changed to " + strVlanCheck + "...nothing to do" + strSwitchReturn;
                        }
                        else
                        {
                            // Validate by serial number
                            if (strSerialCheck.Trim().ToUpper() == strSerial.Trim().ToUpper())
                            {
                                // Serial Number of switchport matches the serial number of the device...OK to continue
                                boolSerialMatch = true;
                                strReturn += "Serial numbers match (" + strSerialCheck.Trim().ToUpper() + "=" + strSerial.Trim().ToUpper() + ")...ok to change..." + strSwitchReturn;
                            }

                            if (boolSerialMatch == true)
                            {
                                // Validate using ARP lookup
                                if (strMACAddress != "")
                                {
                                    if (_ios == true)
                                    {
                                        telnet.WriteLine("show mac-address-table address " + strMACAddress);
                                        string strPortCheck = GetSwitchPort(ReadOutput(telnet, true), false);

                                        if (strPortCheck.Trim().ToUpper() == strInterface.Trim().ToUpper())
                                        {
                                            // Switchport matches the switchport of the device...OK to continue
                                            boolArpOK = true;
                                            strReturn += "Switchports match (" + strPortCheck.Trim().ToUpper() + "=" + strInterface.Trim().ToUpper() + ")...ok to change..." + strSwitchReturn;
                                        }
                                        else if (strPortCheck == "" && _ok_if_no_arp_lookup == true)
                                            boolArpOK = true;
                                        else
                                            strError = "Switch Ports do NOT match (Switch=" + strPortCheck.Trim().ToUpper() + ", Server=" + strInterface.Trim().ToUpper() + ")" + strSwitchReturn;
                                    }
                                    else
                                    {
                                        strError = "Switchport modification only valid for IOS switches";
                                    }
                                }
                                else if (_ok_if_empty_mac == true)
                                    boolArpOK = true;
                                else
                                    strError = "ARP lookup required and MAC address is EMPTY...validation failed." + strSwitchReturn;

                                if (boolArpOK == true)
                                    boolOKtoChange = true;
                            }
                            else
                                strError = "Serial numbers do NOT match (Switch=" + strSerialCheck.Trim().ToUpper() + ", Server=" + strSerial.Trim().ToUpper() + ", Debug = " + strRunDebug + ")" + strSwitchReturn;
                        }

                        if (strError == "")
                        {
                            if (boolOKtoChange == true)
                            {
                                // Change the VLAN
                                if (_ios == true)
                                {
                                    telnet.WriteLine("config t");
                                    if (_debug == true)
                                        strReturn += "DEBUG: " + ReadOutput(telnet, true) + strSwitchReturn;
                                    telnet.WriteLine("int " + strInterface);
                                    if (_debug == true)
                                        strReturn += "DEBUG: " + ReadOutput(telnet, true) + strSwitchReturn;
                                    telnet.WriteLine("switchport access vlan " + strVlan);
                                    if (_debug == true)
                                        strReturn += "DEBUG: " + ReadOutput(telnet, true) + strSwitchReturn;
                                    telnet.WriteLine("no shutdown");
                                    if (_debug == true)
                                        strReturn += "DEBUG: " + ReadOutput(telnet, true) + strSwitchReturn;
                                    telnet.WriteLine("description " + strSerial.ToUpper());
                                    if (_debug == true)
                                        strReturn += "DEBUG: " + ReadOutput(telnet, true) + strSwitchReturn;
                                    telnet.WriteLine("end");
                                    if (_debug == true)
                                        strReturn += "DEBUG: " + ReadOutput(telnet, true) + strSwitchReturn;
                                    telnet.WriteLine("write");
                                    if (_debug == true)
                                        strReturn += "DEBUG: " + ReadOutput(telnet, true) + strSwitchReturn;
                                }
                                else
                                {
                                }
                                strReturn += "Update successful! (" + strVlanCheck + " to " + strVlan + ")" + strSwitchReturn;
                            }
                            else
                                strReturn += "Validation failed (???)" + strSwitchReturn;
                        }
                    }

                    // Quit
                    telnet.WriteLine("quit");
                    if (_debug == true)
                        strReturn += "DEBUG: " + ReadOutput(telnet, true) + strSwitchReturn;

                    if (telnet.IsConnected == true)
                        telnet = null;
                    strReturn += "Disconnected from " + _switch + strSwitchReturn;
                    strReturn += "Done." + strSwitchReturn;
                }
            }
            if (strError != "")
                strReturn = "ERROR: " + strError;
            return strReturn;
        }

        private string ReadOutput(TelnetConnection oTelnet, bool boolCheck)
        {
            string strReturn = "";
            string strRead = "";
            bool boolAlready = false;
            int intCount = 0;
            while (boolCheck == true && oTelnet.IsConnected == true && intCount < intSwitchMaxLoops)
            {
                intCount++;
                strRead = oTelnet.Read();
                if (strRead != "")
                    boolAlready = true;
                if (strRead == "" && boolAlready == true)
                    break;
                strReturn += strRead;
            }
            return strReturn;
        }

        private string GetSwitchPort(string strResult, bool boolStrip)
        {
            string strLast = strSwitchBreak + strSwitchBreak;
            strResult = strResult.Substring(0, strResult.LastIndexOf(strLast));
            strResult = strResult.Substring(strResult.LastIndexOf(strSwitchBreak) + strSwitchBreak.Length);
            if (strResult.StartsWith("*") == true)
            {
                strResult = strResult.Trim();
                strResult = strResult.Substring(strResult.LastIndexOf(" ") + 1);
                if (boolStrip == true)
                    strResult = strResult.Substring(2);
                return strResult;
            }
            else
                return "";
        }

        private string ReadSwitchPort(string strResult, string strValue)
        {
            if (strResult.ToUpper().Contains(strValue.ToUpper()) == true)
            {
                strResult = strResult.Substring(strResult.ToUpper().IndexOf(strValue.ToUpper()) + strValue.Length);
                if (strResult.ToUpper().Contains(strSwitchBreak.ToUpper()) == true)
                    strResult = strResult.Substring(0, strResult.IndexOf(strSwitchBreak));
                return strResult.Trim();
            }
            else
                return "";
        }

        public string OnlyLettersAndNumbersFromString(string s)
        {
            return new Regex(@"[^a-zA-Z0-9 ]").Replace(s, "");
        }

        public string ToTitleCase(string _string)
        {
            TextInfo oText = new CultureInfo("en-US", false).TextInfo;
            return oText.ToTitleCase(_string.ToLower());
        }


        public DataSet GetSetupValuesById(int _SMid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@SMId", _SMid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_GetSetupValuesById", arParams);
        }
        public string GetSetupValuesById(int _id, string _column)
        {
            DataSet ds = GetSetupValuesById(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        public DataSet GetSetupValuesByKey(string _SMKey)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@SMKey", _SMKey);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_GetSetupValuesByKey", arParams);
        }

        public string GetGetEmailAlertsEmailIds(string _emailGroups)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@SMKey", _emailGroups);
            arParams[1] = new SqlParameter("@SMEmailIds", SqlDbType.VarChar, 4000);
            //arParams[1].Size = 

            arParams[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_GetEmailAlertsEmailIds", arParams);
            return arParams[1].Value.ToString();
        }

        public string HostName(int _serverid, string _read_output_type, string _server_ipaddress, string _location_of_psexec, string _user, string _pass, int _timeout_in_minutes)
        {
            // Convert seconds to milliseconds
            _timeout_in_minutes = (_timeout_in_minutes * 60 * 1000);
            ProcessStartInfo infoAudit = new ProcessStartInfo(_location_of_psexec + "psexec");
            infoAudit.WorkingDirectory = _location_of_psexec;
            // Windows 2008: Have to remove the "-i" parameter or else the script won't run (will come up with "the following helper dll cannot be loaded...XXXX.DLL" error)
            string strAuditArguments = "\\\\" + _server_ipaddress + " -u " + _user + " -p " + _pass + " -h cmd.exe /c hostname.exe";
            infoAudit.Arguments = strAuditArguments;
            infoAudit.UseShellExecute = false;
            infoAudit.RedirectStandardOutput = true;
            infoAudit.RedirectStandardError = true;

            Process procAudit = Process.Start(infoAudit);
            procAudit.WaitForExit(_timeout_in_minutes);
            if (procAudit.HasExited == false)
                procAudit.Kill();
            string output = procAudit.StandardOutput.ReadToEnd().Replace(Environment.NewLine, "");

            Servers oServer = new Servers(user, dsn);
            oServer.AddOutput(_serverid, _read_output_type, output);

            return output;
        }

        public int ExecuteVBScript(int _serverid, bool _run_on_clearview_server, bool _got_local_file, string _read_output_type, string _server_name, string _server_serial, string _server_ipaddress, string _path_to_script, string _local_directory_with_prefix, string _additional_prefix, string _local_path_to_exe, string _local_location_on_remote_device, string _extension, string _parameters, string _location_of_psexec, string _user, string _pass, int _timeout_in_minutes, bool _interactive, bool _no_wait, int _logging, bool _delete_files)
        {
            // Convert seconds to milliseconds
            int intTimeout = (_timeout_in_minutes * 60 * 1000);
            int intTimeoutDefault = (2 * 60 * 1000);    // 2 minutes
            int intTimeoutNone = 5000;  // 5 seconds
            bool boolTimeout = false;
            bool boolNetUseError = false;

            int intReturn = (int)AuditStatus.Running;
            string strLocal = _local_directory_with_prefix.Substring(0, _local_directory_with_prefix.LastIndexOf("\\")) + _path_to_script.Substring(_path_to_script.LastIndexOf("\\"));
            //   1.) Create the BAT file which will call the VBS
            string strAuditScriptCallVBS = _local_directory_with_prefix + _additional_prefix + "1CallExe.bat";
            StreamWriter oWriterIP2 = new StreamWriter(strAuditScriptCallVBS);
            string strParameters = "";
            char[] strSplit = { ' ' };
            string[] strParametersList = _parameters.Split(strSplit);
            for (int ii = 0; ii < strParametersList.Length; ii++)
            {
                string strParameter = strParametersList[ii].Trim();
                if (strParameter != "")
                {
                    if (strParameters != "")
                        strParameters += " ";
                    strParameters += "\"" + strParameter + "\"";
                }
            }
            string strWriter2 = "";
            if (_extension.ToUpper() == "BAT" && strParameters != "")
            {
                // Add additional double-quote around executable in order to pass the parameter...
                // EXAMPLE: %WinDir%\System32\cmd.exe /c ""C:\OPTIONS\CV_AUDIT_SCRIPT_278_.BAT" "1""
                if (_run_on_clearview_server == true)
                    strWriter2 = _local_path_to_exe + " \"\"" + strLocal + "\" " + strParameters + "\"";
                else
                    strWriter2 = _local_path_to_exe + " \"\"C:\\" + _local_location_on_remote_device + "." + _extension + "\" " + strParameters + "\"";
            }
            else
            {
                if (_run_on_clearview_server == true)
                    strWriter2 = _local_path_to_exe + " \"" + strLocal + "\"" + (strParameters == "" ? "" : " " + strParameters);
                else
                    strWriter2 = _local_path_to_exe + " \"C:\\" + _local_location_on_remote_device + "." + _extension + "\"" + (strParameters == "" ? "" : " " + strParameters);
            }
            oLog.AddEvent(_server_name, _server_serial, "Execution Script: " + strWriter2, LoggingType.Debug);
            oWriterIP2.WriteLine(strWriter2);
            oWriterIP2.Flush();
            oWriterIP2.Close();


            if (_got_local_file == false)
            {
                // Need to get the file from the _path location and copy local
                //   2a.) Create the BAT file which will map the drive and copy the file to the server
                string strAuditScriptCopyFilesLocal = _local_directory_with_prefix + _additional_prefix + "2CopyFilesLocal.bat";
                StreamWriter oCopyFilesLocal = new StreamWriter(strAuditScriptCopyFilesLocal);
                // Copy the VBS which will do the actual processing
                oCopyFilesLocal.WriteLine("copy " + _path_to_script + " " + strLocal);
                oCopyFilesLocal.Flush();
                oCopyFilesLocal.Close();

                //   2b.) Execute the copy (from remote to local)
                string strAuditScriptCopyFilesLocalOut = _local_directory_with_prefix + _additional_prefix + "3ExecuteCopy.txt";
                ProcessStartInfo infoAuditCopyFilesLocal = new ProcessStartInfo(_location_of_psexec + "psexec");
                infoAuditCopyFilesLocal.WorkingDirectory = _location_of_psexec;
                string strAuditCopyFilesArguments = "-h cmd.exe /c " + strAuditScriptCopyFilesLocal + " > " + strAuditScriptCopyFilesLocalOut;
                infoAuditCopyFilesLocal.Arguments = strAuditCopyFilesArguments;
                oLog.AddEvent(_server_name, _server_serial, "Get Remote File: " + _location_of_psexec + "psexec " + strAuditCopyFilesArguments, LoggingType.Debug);
                Process procAuditCopyFilesLocal = Process.Start(infoAuditCopyFilesLocal);
                procAuditCopyFilesLocal.WaitForExit(intTimeoutDefault);
                if (procAuditCopyFilesLocal.HasExited == false)
                {
                    oLog.AddEvent(_server_name, _server_serial, "Get Remote File Timed Out", LoggingType.Debug);
                    procAuditCopyFilesLocal.Kill();
                    boolTimeout = true;
                }
                else
                    oLog.AddEvent(_server_name, _server_serial, "Get Remote File Exited (" + procAuditCopyFilesLocal.ExitCode.ToString() + ")", LoggingType.Debug);
                procAuditCopyFilesLocal.Close();
                if (boolTimeout == false)
                {
                    boolNetUseError = ReadOutput(_serverid, _read_output_type + "[1]", strAuditScriptCopyFilesLocalOut, _server_name, _server_serial, _logging, _delete_files);
                    _path_to_script = strLocal;
                }
            }

            if (boolTimeout == false && boolNetUseError == false)
            {
                if (_run_on_clearview_server == true)
                {
                    //   3.) Execute the Script using PSEXEC
                    string strAuditScriptExecuteRemote = _local_directory_with_prefix + _additional_prefix + "4ExecuteRemote.txt";
                    ProcessStartInfo infoAudit = new ProcessStartInfo(_location_of_psexec + "psexec");
                    infoAudit.WorkingDirectory = _location_of_psexec;
                    string strAuditArguments = "-h cmd.exe /c " + strAuditScriptCallVBS + " > " + strAuditScriptExecuteRemote;
                    infoAudit.Arguments = strAuditArguments;
                    oLog.AddEvent(_server_name, _server_serial, "Local Execution: " + _location_of_psexec + "psexec " + strAuditArguments, LoggingType.Debug);
                    Process procAudit = Process.Start(infoAudit);
                    if (_no_wait == true)
                    {
                        oLog.AddEvent(_server_name, _server_serial, "Local Execution of script " + _read_output_type + " running...no timeout", LoggingType.Debug);
                        procAudit.WaitForExit(intTimeoutNone);
                    }
                    else if (intTimeout == 0)
                    {
                        oLog.AddEvent(_server_name, _server_serial, "Local Execution of script " + _read_output_type + " running...default timeout = " + intTimeoutDefault.ToString(), LoggingType.Debug);
                        procAudit.WaitForExit(intTimeoutDefault);
                    }
                    else
                    {
                        oLog.AddEvent(_server_name, _server_serial, "Local Execution of script " + _read_output_type + " running...timeout = " + intTimeout.ToString(), LoggingType.Debug);
                        procAudit.WaitForExit(intTimeout);
                    }
                    if (procAudit.HasExited == false)
                    {
                        oLog.AddEvent(_server_name, _server_serial, "Local Execution Timed Out", LoggingType.Debug);
                        procAudit.Kill();
                        boolTimeout = true;
                    }
                    else
                        oLog.AddEvent(_server_name, _server_serial, "Local Execution Exited (" + procAudit.ExitCode.ToString() + ")", LoggingType.Debug);
                    if (boolTimeout == false)
                        intReturn = procAudit.ExitCode;
                    procAudit.Close();
                    if (boolTimeout == false)
                    {
                        oLog.AddEvent(_server_name, _server_serial, "Local Execution of script " + _read_output_type + " finished (" + intReturn.ToString() + ")...reading output", LoggingType.Debug);
                        ReadOutput(_serverid, _read_output_type + "[4]", strAuditScriptExecuteRemote, _server_name, _server_serial, _logging, _delete_files);
                        oLog.AddEvent(_server_name, _server_serial, "Output read for script " + _read_output_type, LoggingType.Debug);
                    }
                    else
                        oLog.AddEvent(_server_name, _server_serial, "Local Execution of script " + _read_output_type + " timed out", LoggingType.Debug);
                }
                else
                {
                    //   3.) Create the BAT file which will map the drive and copy the file to the server
                    string strAuditScriptCopyFiles = _local_directory_with_prefix + _additional_prefix + "2CopyFiles.bat";
                    StreamWriter oCopyFiles = new StreamWriter(strAuditScriptCopyFiles);
                    oCopyFiles.WriteLine("F:");
                    oCopyFiles.WriteLine("net use \\\\" + _server_ipaddress + "\\C$ /user:" + _user + " " + _pass + "");
                    string strMkDir = "";
                    if (_local_location_on_remote_device.Contains("\\") == true)
                        strMkDir = _local_location_on_remote_device.Substring(0, _local_location_on_remote_device.LastIndexOf("\\"));
                    if (strMkDir != "")
                        oCopyFiles.WriteLine("mkdir \\\\" + _server_ipaddress + "\\C$\\" + strMkDir);
                    // Copy the BAT which will call the VBS file on the server
                    oCopyFiles.WriteLine("copy " + strAuditScriptCallVBS + " \\\\" + _server_ipaddress + "\\C$\\" + _local_location_on_remote_device + "_.BAT");
                    // Copy the VBS which will do the actual processing
                    oCopyFiles.WriteLine("copy " + _path_to_script + " \\\\" + _server_ipaddress + "\\C$\\" + _local_location_on_remote_device + "." + _extension + "");
                    oCopyFiles.Flush();
                    oCopyFiles.Close();

                    //   4.) Run BAT file to copy files
                    string strAuditScriptCopyFilesOut = _local_directory_with_prefix + _additional_prefix + "3ExecuteCopy.txt";
                    ProcessStartInfo infoAuditCopyFiles = new ProcessStartInfo(_location_of_psexec + "psexec");
                    infoAuditCopyFiles.WorkingDirectory = _location_of_psexec;
                    infoAuditCopyFiles.Arguments = "-h cmd.exe /c " + strAuditScriptCopyFiles + " > " + strAuditScriptCopyFilesOut;
                    Process procAuditCopyFiles = Process.Start(infoAuditCopyFiles);
                    procAuditCopyFiles.WaitForExit(intTimeoutDefault);
                    if (procAuditCopyFiles.HasExited == false)
                    {
                        procAuditCopyFiles.Kill();
                        boolTimeout = true;
                    }
                    procAuditCopyFiles.Close();
                    if (boolTimeout == false)
                    {
                        boolNetUseError = ReadOutput(_serverid, _read_output_type + "[2]", strAuditScriptCopyFilesOut, _server_name, _server_serial, _logging, _delete_files);

                        if (boolNetUseError == false)
                        {
                            //   5.) Execute the Script locally on the server using PSEXEC
                            ProcessStartInfo infoAudit = new ProcessStartInfo(_location_of_psexec + "psexec");
                            infoAudit.WorkingDirectory = _location_of_psexec;
                            // Windows 2008: Have to remove the "-i" parameter or else the script won't run (will come up with "the following helper dll cannot be loaded...XXXX.DLL" error)
                            string strAuditArguments = "\\\\" + _server_ipaddress + " -u " + _user + " -p " + _pass + " -h cmd.exe /c C:\\" + _local_location_on_remote_device + "_.BAT";
                            if (_interactive == true)
                                strAuditArguments = "\\\\" + _server_ipaddress + " -u " + _user + " -p " + _pass + " -h -i cmd.exe /c C:\\" + _local_location_on_remote_device + "_.BAT";
                            infoAudit.Arguments = strAuditArguments;
                            oLog.AddEvent(_server_name, _server_serial, "PSEXEC: " + _location_of_psexec + "psexec " + strAuditArguments, LoggingType.Debug);
                            Process procAudit = Process.Start(infoAudit);
                            if (_no_wait == true)
                                procAudit.WaitForExit(intTimeoutNone);
                            else if (intTimeout == 0)
                                procAudit.WaitForExit(intTimeoutDefault);
                            else
                                procAudit.WaitForExit(intTimeout);
                            if (procAudit.HasExited == false)
                            {
                                oLog.AddEvent(_server_name, _server_serial, "PSEXEC Timed Out", LoggingType.Debug);
                                procAudit.Kill();
                                boolTimeout = true;
                            }
                            else
                                oLog.AddEvent(_server_name, _server_serial, "PSEXEC Exited (" + procAudit.ExitCode.ToString() + ")", LoggingType.Debug);
                            if (boolTimeout == false)
                                intReturn = procAudit.ExitCode;
                            procAudit.Close();

                            if (boolTimeout == false)
                            {
                                if (intReturn == (int)AuditStatus.InvalidParameter)
                                {
                                    // There is an issue with the -i parameter on certain Windows 2003 boxes.  Try rerunning in silent mode.
                                    ProcessStartInfo infoAudit2 = new ProcessStartInfo(_location_of_psexec + "psexec");
                                    infoAudit2.WorkingDirectory = _location_of_psexec;
                                    string strAudit2Arguments = "\\\\" + _server_ipaddress + " -u " + _user + " -p " + _pass + " -h cmd.exe /c C:\\" + _local_location_on_remote_device + "_.BAT";
                                    infoAudit2.Arguments = strAudit2Arguments;
                                    oLog.AddEvent(_server_name, _server_serial, "PSEXEC2: " + _location_of_psexec + "psexec " + strAudit2Arguments, LoggingType.Debug);
                                    Process procAudit2 = Process.Start(infoAudit2);
                                    if (_no_wait == true)
                                        procAudit2.WaitForExit(intTimeoutNone);
                                    else if (intTimeout == 0)
                                        procAudit2.WaitForExit(intTimeoutDefault);
                                    else
                                        procAudit2.WaitForExit(intTimeout);
                                    if (procAudit2.HasExited == false)
                                    {
                                        oLog.AddEvent(_server_name, _server_serial, "PSEXEC2 Timed Out", LoggingType.Debug);
                                        procAudit2.Kill();
                                        boolTimeout = true;
                                    }
                                    else
                                        oLog.AddEvent(_server_name, _server_serial, "PSEXEC2 Exited (" + procAudit2.ExitCode.ToString() + ")", LoggingType.Debug);
                                    if (boolTimeout == false)
                                        intReturn = procAudit2.ExitCode;
                                    procAudit2.Close();
                                }

                                if (boolTimeout == false)
                                {
                                    //   6.) Create BAT file to delete the files on the server
                                    string strAuditScriptDeleteFiles = _local_directory_with_prefix + _additional_prefix + "4DeleteFiles.bat";
                                    StreamWriter oDeleteFiles = new StreamWriter(strAuditScriptDeleteFiles);
                                    if (_delete_files == true)
                                    {
                                        oDeleteFiles.WriteLine("del \\\\" + _server_ipaddress + "\\C$\\" + _local_location_on_remote_device + "_.BAT");
                                        oDeleteFiles.WriteLine("del \\\\" + _server_ipaddress + "\\C$\\" + _local_location_on_remote_device + "." + _extension);
                                    }
                                    oDeleteFiles.WriteLine("net use \\\\" + _server_ipaddress + "\\C$ /dele");
                                    oDeleteFiles.Flush();
                                    oDeleteFiles.Close();

                                    //   8.) Create the VB Script to execute the deletion
                                    string strAuditScriptDeleteFilesOut = _local_directory_with_prefix + _additional_prefix + "5ExecuteDelete.txt";
                                    ProcessStartInfo infoAuditDeleteFiles = new ProcessStartInfo(_location_of_psexec + "psexec");
                                    infoAuditDeleteFiles.WorkingDirectory = _location_of_psexec;
                                    infoAuditDeleteFiles.Arguments = "-h cmd.exe /c " + strAuditScriptDeleteFiles + " > " + strAuditScriptDeleteFilesOut;
                                    Process procAuditDeleteFiles = Process.Start(infoAuditDeleteFiles);
                                    procAuditDeleteFiles.WaitForExit(intTimeoutDefault);
                                    if (procAuditDeleteFiles.HasExited == false)
                                    {
                                        procAuditDeleteFiles.Kill();
                                        boolTimeout = true;
                                    }
                                    procAuditDeleteFiles.Close();
                                    if (boolTimeout == false)
                                        boolNetUseError = ReadOutput(_serverid, _read_output_type + "[3]", strAuditScriptDeleteFilesOut, _server_name, _server_serial, _logging, _delete_files);
                                }
                            }
                        }
                    }
                }

                if (boolTimeout == false && boolNetUseError == false)
                {
                    if (_delete_files == true)
                    {
                        //   Delete local scripts
                        string strAuditScriptDeleteFilesLocal = _location_of_psexec + "DeleteLocalFiles.bat";
                        StreamWriter oDeleteFilesLocal = new StreamWriter(strAuditScriptDeleteFilesLocal);
                        oDeleteFilesLocal.WriteLine("del " + _local_directory_with_prefix + _additional_prefix + "*.TXT");
                        oDeleteFilesLocal.WriteLine("del " + _local_directory_with_prefix + _additional_prefix + "*.VBS");
                        oDeleteFilesLocal.WriteLine("del " + _local_directory_with_prefix + _additional_prefix + "*.BAT");
                        oDeleteFilesLocal.WriteLine("del " + strLocal);
                        oDeleteFilesLocal.Flush();
                        oDeleteFilesLocal.Close();

                        //   Create the VB Script to execute the deletion
                        ProcessStartInfo infoAuditDeleteFilesLocal = new ProcessStartInfo(_location_of_psexec + "psexec");
                        infoAuditDeleteFilesLocal.WorkingDirectory = _location_of_psexec;
                        infoAuditDeleteFilesLocal.Arguments = "-i cmd.exe /c " + strAuditScriptDeleteFilesLocal;
                        Process procAuditDeleteFilesLocal = Process.Start(infoAuditDeleteFilesLocal);
                        procAuditDeleteFilesLocal.WaitForExit(intTimeoutDefault);
                        if (procAuditDeleteFilesLocal.HasExited == false)
                        {
                            procAuditDeleteFilesLocal.Kill();
                            boolTimeout = true;
                        }
                        procAuditDeleteFilesLocal.Close();
                    }
                }
            }

            if (boolTimeout == true)
                intReturn = (int)AuditStatus.TimedOut;
            else if (boolNetUseError == true)
                intReturn = (int)AuditStatus.NetUseError;
            else if (intReturn == (int)AuditStatus.Running)
                intReturn = (int)AuditStatus.Success;

            return intReturn;
        }
        public bool ExecutePower(int _serverid, int _assetid, bool _power_on, string _read_output_type, string _name, int _environment, string _exe_location, int _logging, string _dsn_asset)
        {
            int intTimeoutDefault = (5 * 60 * 1000);    // 5 minutes
            bool boolTimeout = false;
            bool boolReturn = false;
            ModelsProperties oModelsProperties = new ModelsProperties(user, dsn);
            Servers oServer = new Servers(user, dsn);
            Log oLog = new Log(user, dsn);
            Asset oAsset = new Asset(user, _dsn_asset, dsn);
            Variables oVarPower = new Variables(_environment);

            string strSerial = oAsset.Get(_assetid, "serial");
            string strILO = oAsset.GetServerOrBlade(_assetid, "ilo").ToUpper();
            int intModelProperty = 0;
            if (strILO != "" && Int32.TryParse(oAsset.Get(_assetid, "modelid"), out intModelProperty) == true)
            {

                string strResultsFile = "";
                bool boolErrorFile = false;

                // Distributed
                if (oModelsProperties.IsDell(intModelProperty) == true)
                {
                    oLog.AddEvent(_name, strSerial, "ExecutePower: asset is DELL", LoggingType.Debug);
                    // DELL
                    int intDellConfig = 0;
                    Int32.TryParse(oModelsProperties.Get(intModelProperty, "dellconfigid"), out intDellConfig);
                    Dells oDell = new Dells(user, dsn);
                    DataSet dsDell = oDell.Get(intDellConfig);
                    if (dsDell.Tables[0].Rows.Count == 1)
                    {
                        DataRow drDell = dsDell.Tables[0].Rows[0];
                        string strDellSplit = drDell["xml_split"].ToString().ToUpper();
                        string strDellOperator = drDell["xml_operator"].ToString().ToUpper();
                        string strDellStart = drDell["xml_start"].ToString().ToUpper();
                        string strDellQueryPower = drDell["query_power"].ToString().ToUpper();
                        string strDellQueryMAC1 = drDell["query_mac1"].ToString().ToUpper();
                        string strDellQueryMAC2 = drDell["query_mac2"].ToString().ToUpper();
                        string strDellPowerOn = drDell["success_power_on"].ToString().ToUpper();
                        string strDellPowerOff = drDell["success_power_off"].ToString().ToUpper();
                        string strDellUsername = drDell["username"].ToString();
                        string strDellPassword = drDell["password"].ToString();

                        // Initialize the RACADM command.
                        if (_exe_location == "" ||
                                strDellSplit == "" ||
                                strDellOperator == "" ||
                                strDellStart == "" ||
                                strDellQueryPower == "" ||
                                strDellQueryMAC1 == "" ||
                                strDellQueryMAC2 == "" ||
                                strDellPowerOn == "" ||
                                strDellPowerOff == "" ||
                                strILO == "")
                        {
                            oLog.AddEvent(_name, strSerial, "ExecutePower: missing config (" + intDellConfig.ToString() + ")", LoggingType.Debug);
                            boolReturn = false;
                        }
                        else
                        {
                            char[] chrDellSplit = { '\n' };
                            Variables oVarPNC = new Variables(999);   // PNC
                            if (strDellUsername == "" || strDellPassword == "")
                            {
                                strDellUsername = oVarPNC.ADUser() + "@" + oVarPNC.FullyQualified();
                                //strDellUsername = oVarPNC.Domain() + "\\" + oVarPNC.ADUser();
                                strDellPassword = oVarPNC.ADPassword();
                            }

                            ProcessStartInfo processStartInfo = new ProcessStartInfo(_exe_location + "racadm.exe");
                            processStartInfo.WorkingDirectory = _exe_location;
                            oLog.AddEvent(_name, strSerial, "ExecutePower: command = " + _exe_location + "racadm.exe -r " + strILO + " -u " + strDellUsername + " -p **hidden** serveraction " + (_power_on ? "powerup" : "powerdown"), LoggingType.Debug);
                            processStartInfo.Arguments = "-r " + strILO + " -u " + strDellUsername + " -p " + strDellPassword + " serveraction " + (_power_on ? "powerup" : "powerdown");
                            processStartInfo.UseShellExecute = false;
                            processStartInfo.RedirectStandardOutput = true;
                            Process proc = Process.Start(processStartInfo);
                            StreamReader outputReader = proc.StandardOutput;
                            proc.WaitForExit(intTimeoutDefault);
                            if (proc.HasExited == false)
                            {
                                proc.Kill();
                                boolTimeout = true;
                            }
                            if (boolTimeout == false)
                            {
                                strResultsFile = outputReader.ReadToEnd().ToUpper();
                                oServer.AddOutput(_serverid, _read_output_type, strResultsFile);
                                try
                                {
                                    char[] strSplit = { ';' };
                                    if (_power_on == true)
                                    {
                                        string[] strDellPowerOns = strDellPowerOn.Split(strSplit);
                                        for (int ii = 0; ii < strDellPowerOns.Length; ii++)
                                        {
                                            if (strDellPowerOns[ii].Trim() != "")
                                            {
                                                if (strResultsFile.Contains(strDellPowerOns[ii].Trim()) == true)
                                                {
                                                    boolReturn = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string[] strDellPowerOffs = strDellPowerOff.Split(strSplit);
                                        for (int ii = 0; ii < strDellPowerOffs.Length; ii++)
                                        {
                                            if (strDellPowerOffs[ii].Trim() != "")
                                            {
                                                if (strResultsFile.Contains(strDellPowerOffs[ii].Trim()) == true)
                                                {
                                                    boolReturn = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }
                            proc.Close();
                        }
                    }
                }
                else
                {
                    oLog.AddEvent(_name, strSerial, "ExecutePower: asset is not DELL...using HP commands (CPQLOCFG.EXE)", LoggingType.Debug);
                    // HP
                    string strPowerScript = _exe_location + "CPQLOCFG.EXE";
                    string strPowerLog = _exe_location + _serverid.ToString() + "_PowerOut.txt";
                    string strPowerParameters = "-s " + strILO + " -l " + strPowerLog + " -f " + _exe_location + (_power_on ? "poweron" : "poweroff2") + ".xml -v -u " + oVarPower.ILOUsername() + " -p " + oVarPower.ILOPassword();
                    oLog.AddEvent(_name, strSerial, "Power " + (_power_on ? "On" : "Off") + " Script: " + strPowerScript + " " + strPowerParameters, LoggingType.Information);
                    ProcessStartInfo infoAudit = new ProcessStartInfo(_exe_location + "CPQLOCFG.EXE");
                    infoAudit.WorkingDirectory = _exe_location;
                    infoAudit.Arguments = strPowerParameters;
                    Process procAudit = Process.Start(infoAudit);
                    procAudit.WaitForExit(intTimeoutDefault);
                    if (procAudit.HasExited == false)
                    {
                        procAudit.Kill();
                        boolTimeout = true;
                    }
                    int intReturn = -999;
                    if (boolTimeout == false)
                        intReturn = procAudit.ExitCode;
                    procAudit.Close();
                    if (boolTimeout == false)
                        ReadOutput(_serverid, _read_output_type, strPowerLog, _name, strSerial, _logging, true);
                    return (intReturn == 0);
                }
            }

            return boolReturn;
        }
        /*
        public bool ExecutePower(int _serverid, string _ilo, bool _power_on, string _read_output_type, string _name, string _serial, string _user, string _pass, string _CPQLOCFG_dir, int _logging)
        {
            int intTimeoutDefault = (2 * 60 * 1000);    // 2 minutes
            bool boolTimeout = false;

            string strPowerScript = _CPQLOCFG_dir + "CPQLOCFG.EXE";
            string strPowerLog = _CPQLOCFG_dir + _serverid.ToString() + "_PowerOut.txt";
            string strPowerParameters = "-s " + _ilo + " -l " + strPowerLog + " -f " + _CPQLOCFG_dir + (_power_on ? "poweron" : "poweroff") + ".xml -v -u " + _user + " -p " + _pass;
            oLog.AddEvent(_name, _serial, "Power On Script: " + strPowerScript + " " + strPowerParameters, LoggingType.Information);
            ProcessStartInfo infoAudit = new ProcessStartInfo(_CPQLOCFG_dir + "CPQLOCFG.EXE");
            infoAudit.WorkingDirectory = _CPQLOCFG_dir;
            infoAudit.Arguments = strPowerParameters;
            Process procAudit = Process.Start(infoAudit);
            procAudit.WaitForExit(intTimeoutDefault);
            if (procAudit.HasExited == false)
            {
                procAudit.Kill();
                boolTimeout = true;
            }
            int intReturn = -999;
            if (boolTimeout == false)
                intReturn = procAudit.ExitCode;
            procAudit.Close();
            if (boolTimeout == false)
                ReadOutput(_serverid, _read_output_type, strPowerLog, _name, _serial, _logging, true);
            return (intReturn == 0);
        }
        */
        public bool ReadOutput(int _serverid, string _type, string _file, string _name, string _serial, int _logging, bool _delete_files)
        {
            Servers oServer = new Servers(0, dsn);
            bool boolContent = false;
            bool boolError = false;
            for (int ii = 0; ii < 10 && boolContent == false; ii++)
            {
                if (File.Exists(_file) == true)
                {
                    oLog.AddEvent(_name, _serial, "Server output file " + _file + " exists...reading...", LoggingType.Information);
                    string strContent = "";
                    StreamReader oReader = new StreamReader(_file);
                    try
                    {
                        strContent = oReader.ReadToEnd();
                        if (strContent != "")
                        {
                            if (_logging > 0)
                                oLog.AddEvent(_name, _serial, "Updating Database...", LoggingType.Information);
                            boolContent = true;
                            oServer.AddOutput(_serverid, _type, strContent);
                            oReader.Close();
                            if (_delete_files == true && File.Exists(_file) == true)
                                File.Delete(_file);
                            if (_logging > 0)
                                oLog.AddEvent(_name, _serial, "Server output file " + _file + " finished updating...deleted files...", LoggingType.Information);
                            // Check for NET USE command
                            if (strContent.ToUpper().Contains("NET USE") == true && strContent.ToUpper().Contains(" /DELE") == false)
                            {
                                boolError = (strContent.ToUpper().Contains("THE COMMAND COMPLETED SUCCESSFULLY") == false);
                                if (boolError == true)
                                    oLog.AddEvent(_name, _serial, "The NET USE statement [" + strContent + "] does not contain [THE COMMAND COMPLETED SUCCESSFULLY]", LoggingType.Error);
                            }
                        }
                        else
                        {
                            if (_logging > 1)
                                oLog.AddEvent(_name, _serial, "Found server output file " + _file + "...but it is blank...waiting 5 seconds...", LoggingType.Information);
                            oReader.Close();
                            Thread.Sleep(5000);
                        }
                    }
                    catch
                    {
                        if (_logging > 1)
                            oLog.AddEvent(_name, _serial, "Cannot open server output file " + _file + "...waiting 5 seconds...", LoggingType.Information);
                        oReader.Close();
                        Thread.Sleep(5000);
                    }
                }
                else
                {
                    if (_logging > 1)
                        oLog.AddEvent(_name, _serial, "Server output file " + _file + " does not exist...waiting 5 seconds...", LoggingType.Information);
                    Thread.Sleep(5000);
                }
            }
            if (boolContent == false)
            {
                oLog.AddEvent(_name, _serial, "Could Not Find Server output file " + _file, LoggingType.Warning);
                boolError = true;
            }
            return boolError;
        }

        public void ReadOutput(int _serverid, string _type, StreamReader _reader)
        {
            Servers oServer = new Servers(0, dsn);
            string strContent = _reader.ReadToEnd();
            oServer.AddOutput(_serverid, _type, strContent);
            _reader.Close();
        }

        public string GetBox(string _rgb, int _width, int _height)
        {
            return "<div style=\"width:" + _width.ToString() + ";height:" + _height.ToString() + ";background-color:#" + _rgb + ";border:solid 1px #999999\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"" + _width.ToString() + "\" height=\"" + _height.ToString() + "\"></div>";
        }

        public void SendPNCAD(string strNameGG, int _environment, bool _html)
        {
            string strNewLine = (_html ? "<br/>" : Environment.NewLine);
            string strTab = (_html ? "&nbsp;&nbsp;&nbsp;" : "   ");
            string strAppSupport = GetPNCAD(strNameGG + "AppSupport", _environment, strTab, strNewLine);
            string strAppUsers = GetPNCAD(strNameGG + "AppUsers", _environment, strTab, strNewLine);
            string strDevelopers = GetPNCAD(strNameGG + "Developers", _environment, strTab, strNewLine);
            string strPromoters = GetPNCAD(strNameGG + "Promoters", _environment, strTab, strNewLine);
            string strAuthProbMgmt = GetPNCAD(strNameGG + "AuthProbMgmt", _environment, strTab, strNewLine);
            string strAuthPromoters = GetPNCAD(strNameGG + "AuthPromoters", _environment, strTab, strNewLine);

            // Build Email
            StringBuilder strBody = new StringBuilder();
            strBody.Append("Code Promotion" + strNewLine);
            strBody.Append(strTab + strNameGG + "AuthPromoters" + strNewLine);
            strBody.Append(strTab + strTab + "(Only has a member during PTM change. This group grants elevated privileges.)" + strNewLine);
            strBody.Append(strNewLine);
            strBody.Append(strTab + strNameGG + "Promoters" + strNewLine);
            strBody.Append(strTab + strTab + "Members:" + strNewLine);
            strBody.Append(strPromoters + strNewLine);

            strBody.Append("Problem Resolution" + strNewLine);
            strBody.Append(strTab + strNameGG + "AuthProbMgmt" + strNewLine);
            strBody.Append(strTab + strTab + "(Only has members when a problem occurs. Grants Admin Access, Sev1 Infoman Ticket needed)" + strNewLine);
            strBody.Append(strNewLine);
            strBody.Append(strTab + strNameGG + "AppSupport" + strNewLine);
            strBody.Append(strTab + strTab + "Members:" + strNewLine);
            strBody.Append(strAppSupport + strNewLine);
            strBody.Append(strTab + strNameGG + "Developers" + strNewLine);
            strBody.Append(strTab + strTab + "Members:" + strNewLine);
            strBody.Append(strDevelopers + strNewLine);

            strBody.Append("Privileged Access" + strNewLine);
            strBody.Append(strTab + strNameGG + "AuthProbMgmt" + strNewLine);
            strBody.Append(strTab + strTab + "(Only has members during PTM change. This group grants Admin Access.)" + strNewLine);
            strBody.Append(strNewLine);
            strBody.Append(strTab + strNameGG + "AppSupport" + strNewLine);
            strBody.Append(strTab + strTab + "Members:" + strNewLine);
            strBody.Append(strAppSupport + strNewLine);
            strBody.Append(strTab + strNameGG + "Developers" + strNewLine);
            strBody.Append(strTab + strTab + "Members:" + strNewLine);
            strBody.Append(strDevelopers + strNewLine);
            strBody.Append(strTab + strNameGG + "Promoters" + strNewLine);
            strBody.Append(strTab + strTab + "Members:" + strNewLine);
            strBody.Append(strPromoters + strNewLine);

            string strEMailIdsTo = GetGetEmailAlertsEmailIds("EMAILGRP_ACTIVE_DIRECTORY_PTM");
            string strEMailIdsBCC = GetGetEmailAlertsEmailIds("EMAILGRP_ACTIVE_DIRECTORY");
            SendEmail("Access Management Changes", strEMailIdsTo, "", strEMailIdsBCC, "Access Management Changes (PTM)", strBody.ToString(), true, (_html == false));
        }

        private string GetPNCAD(string strGroupName, int _environment, string _tab, string _newLine)
        {
            string strReturn = "";
            char[] strSplit = { ';' };
            AD oAD = new AD(0, dsn, _environment);
            Users oUser = new Users(0, dsn);
            DirectoryEntry oEntryGroup = oAD.GroupSearch(strGroupName);
            if (oEntryGroup != null)
            {
                string strUser = oAD.GetGroupMembers(oEntryGroup);
                string[] strUsers = strUser.Split(strSplit);
                if (strUsers.Length > 0)
                {
                    for (int ii = 0; ii < strUsers.Length; ii++)
                    {
                        if (strUsers[ii].Trim() != "")
                        {
                            string strID = strUsers[ii].Trim().ToUpper();
                            string strName = "???";
                            DirectoryEntry oEntryUser = oAD.UserSearch(strUser);
                            if (oEntryUser != null)
                                strName = oEntryUser.Properties["givenname"].Value.ToString() + " " + oEntryUser.Properties["sn"].Value.ToString();
                            else
                            {
                                int intUser = oUser.GetId(strID);
                                strName = oUser.GetFullName(intUser);
                            }
                            strReturn += _tab + _tab + strName + " [" + strID + "]" + _newLine;
                        }
                    }
                }
            }
            return strReturn;
        }

        public string FormatMAC(string _mac, string _delimiter)
        {
            string strMAC = _mac;
            while (strMAC.Contains("-") == true)
                strMAC = strMAC.Replace("-", "");
            while (strMAC.Contains(":") == true)
                strMAC = strMAC.Replace(":", "");
            while (strMAC.Contains(" ") == true)
                strMAC = strMAC.Replace(" ", "");
            if (strMAC.Length != 12)
            {
                return "";
            }
            else
            {
                string strMAC1 = strMAC.Substring(0, 2);
                string strMAC2 = strMAC.Substring(2, 2);
                string strMAC3 = strMAC.Substring(4, 2);
                string strMAC4 = strMAC.Substring(6, 2);
                string strMAC5 = strMAC.Substring(8, 2);
                string strMAC6 = strMAC.Substring(10, 2);
                return strMAC1 + _delimiter + strMAC2 + _delimiter + strMAC3 + _delimiter + strMAC4 + _delimiter + strMAC5 + _delimiter + strMAC6;
            }
        }

        private string CreateRandomPassword(int passwordLength)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] chars = new char[passwordLength];
            Random rd = new Random();
            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }
        public string AddPassword(int _length)
		{
            string strPassword = CreateRandomPassword(_length);
			arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@password", strPassword);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addPassword", arParams);
            return strPassword;
		}
        public string GetPassword(string _password)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@password", _password);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPassword", arParams);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DeletePassword(_password);
                Variables oVariable = new Variables(intEnvironment);
                return oVariable.ADPassword();
            }
            else
                return "";
        }
        public void DeletePassword(string _password)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@password", _password);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deletePassword", arParams);
        }

        public string BuildBox(string _image, string _header, string _label, string _parent_class)
        {
            StringBuilder strReturn = new StringBuilder();
            strReturn.Append("<table cellpadding=\"0\" cellspacing=\"5\" border=\"0\"><tr><td rowspan=\"2\" valign=\"top\"><img src=\"");
            strReturn.Append(_image);
            strReturn.Append("\" border=\"0\" align=\"absmiddle\" /></td><td class=\"header\" width=\"100%\" valign=\"bottom\">");
            strReturn.Append(_header);
            strReturn.Append("</td></tr><tr><td width=\"100%\" valign=\"top\">");
            strReturn.Append(_label);
            strReturn.Append("</td></tr></table>");
            if (_parent_class != null && _parent_class != "")
            {
                strReturn.Insert(0, "\">");
                strReturn.Insert(0, _parent_class);
                strReturn.Insert(0, "<table cellpadding=\"0\" cellspacing=\"5\" border=\"0\"><tr><td colspan=\"2\" align=\"center\" class=\"");
                strReturn.Append("</td></tr></table>");
            }
            return strReturn.ToString();
        }
    }
}
