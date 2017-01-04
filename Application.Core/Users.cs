using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.DirectoryServices;
using System.Configuration;
using System.Net;
using System.Text;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace NCC.ClearView.Application.Core
{
	public class Users
	{
		private string dsn = "";
		private int user = 0;
		private bool logging = false;
		private SqlParameter[] arParams;
		private DataSet ds;
		private Encryption oEncrypt;
		private Log oLog;
		public Users(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
			oLog = new Log(user, dsn);
			oEncrypt = new Encryption();
		}
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
			return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getUser", arParams);
		}
        public DataSet Gets(string _xid, int _show_xid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@xid", _xid);
            arParams[1] = new SqlParameter("@show_xid", _show_xid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getUserXIDs", arParams);
        }
        public DataSet Gets(string _pnc_id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@pnc_id", _pnc_id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getUserPNCIDs", arParams);
        }
        public DataSet Gets(int _enabled)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@enabled", _enabled);
			return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getUsers", arParams);
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
            DataSet dsGet = Get(_id);
            try 
            {
                strName = dsGet.Tables[0].Rows[0]["pnc_id"].ToString().ToUpper();
                // 7/28/10 - CODE PUSH FIX
                if (strName == "")
                {
                //    // try to get email address of XID, then get PID
                //    string strEmail = GetEmail(dsGet.Tables[0].Rows[0]["xid"].ToString().ToUpper());
                //    if (strEmail != "") 
                //    {
                //        // Replace NCB with PNC
                //        strEmail = strEmail.Replace("nationalcity.com", "pnc.com");

                //        oAD.Search(strEmail, "mail")
                //    }

                    strName = dsGet.Tables[0].Rows[0]["xid"].ToString().ToUpper();
                }
            }
			catch {}
			return strName;
		}
        public string GetName(int _id, bool _xid)
        {
            string strName = "Unavailable";
            DataSet dsGet = Get(_id);
            try
            {
                strName = dsGet.Tables[0].Rows[0]["pnc_id"].ToString().ToUpper();
                if (strName == "" || _xid == true)
                    strName = dsGet.Tables[0].Rows[0]["xid"].ToString().ToUpper();
            }
            catch { }
            return strName;
        }
        public string GetEmail(string _xid, int _environment)
        {
            string strEmail = "";
            int intUser = GetId(_xid);
            string strXID = Get(intUser, "xid");
            string strPNC = Get(intUser, "pnc_id");
            bool boolPNC = false;
            if (strPNC != "")
            {
                //if (HttpContext.Current != null && HttpContext.Current.Request != null)
                //{
                    // Assume we are coming from the website (which can access the LDAP call directly)
                    Functions oFunction = new Functions(user, dsn, _environment);
                    SearchResultCollection oCollection = oFunction.eDirectory(strPNC);
                    if (oCollection.Count == 1 && oCollection[0].GetDirectoryEntry().Properties.Contains("mail") == true)
                        strEmail = oCollection[0].GetDirectoryEntry().Properties["mail"].Value.ToString();
                //}
                //else
                //{
                //    // Assume we are coming from the windows service (which cannot access the LDAP call directly)
                //    Variables oVariable = new Variables(_environment);
                //    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateFbCertificate);
                //    string url = oVariable.URL() + "/activedirectory.aspx?email=" + strPNC;
                //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //    NetworkCredential netCredential = new NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                //    request.Credentials = netCredential;
                //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //    Encoding encoding = ASCIIEncoding.ASCII;
                //    using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                //        strEmail = reader.ReadToEnd().Trim();

                //    if (strEmail == "")
                //        strEmail = strPNC + "@pnc.com";
                //}
            }

            //if ((boolPNC == false || strEmail == "") && strXID != "")
            //{
            //    AD oAD = new AD(user, dsn, _environment);
            //    SearchResultCollection oResult = oAD.Search(strXID, "cn");
            //    if (oResult.Count == 1 && oResult[0].GetDirectoryEntry().Properties.Contains("mail") == true)
            //        strEmail = oResult[0].GetDirectoryEntry().Properties["mail"].Value.ToString();
            //}
            return strEmail;
        }
        private static bool ValidateFbCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
        public string GetFullNameAD(int _id, int _environment)
        {
            string strFName = "";
            string strLName = "";
            bool boolPNC = false;
            if (_environment == (int)CurrentEnvironment.CORPDMN || _environment == (int)CurrentEnvironment.PNCNT_PROD)
            {
                AD oPNC = new AD(user, dsn, (int)CurrentEnvironment.PNCNT_PROD);
                string _pnc = Get(_id, "pnc_id");
                if (_pnc != "")
                {
                    boolPNC = true;
                    SearchResultCollection oResult = oPNC.Search(_pnc, "cn");
                    if (oResult[0].Properties.Contains("givenname") == true)
                        strFName = oResult[0].GetDirectoryEntry().Properties["givenname"].Value.ToString();
                    if (oResult[0].Properties.Contains("sn") == true)
                        strLName = oResult[0].GetDirectoryEntry().Properties["sn"].Value.ToString();
                }
            }
            if (boolPNC == false)
            {
                AD oAD = new AD(user, dsn, (int)CurrentEnvironment.CORPDMN);
                string _xid = Get(_id, "xid");
                if (_xid != "")
                {
                    SearchResultCollection oResult = oAD.Search(_xid, "cn");
                    if (oResult[0].Properties.Contains("givenname") == true)
                        strFName = oResult[0].GetDirectoryEntry().Properties["givenname"].Value.ToString();
                    if (oResult[0].Properties.Contains("sn") == true)
                        strLName = oResult[0].GetDirectoryEntry().Properties["sn"].Value.ToString();
                }
            }
            return strFName + " " + strLName;
        }
        public string GetFullNameAD(string _xid, int _environment)
        {
            AD oAD = new AD(user, dsn, _environment);
            string strFName = "";
            string strLName = "";
            SearchResultCollection oResult = oAD.Search(_xid, "cn");
            if (oResult[0].Properties.Contains("givenname") == true)
                strFName = oResult[0].GetDirectoryEntry().Properties["givenname"].Value.ToString();
            if (oResult[0].Properties.Contains("sn") == true)
                strLName = oResult[0].GetDirectoryEntry().Properties["sn"].Value.ToString();
            return strFName + " " + strLName;
        }
        public string GetFullName(int _id)
        {
            ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0]["fname"].ToString() + " " + ds.Tables[0].Rows[0]["lname"].ToString();
            else
                return "";
        }
        public string GetFullNameWithLanID(int _id)
        {
            ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0]["fname"].ToString() + " " + ds.Tables[0].Rows[0]["lname"].ToString() + " (" + GetName(_id) + ")";
            else
                return "";
        }
        public string GetFullName(string _xid)
        {
            return GetFullName(GetId(_xid));
        }
        public int GetId(string _username) 
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@username", _username);
			object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getUserId", arParams);
			if (o == null)
				return 0;
			else
				return Int32.Parse(o.ToString());
		}
        public int GetIdName(string _name) 
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@name", _name);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getUserIdName", arParams);
			if (o == null)
				return 0;
			else
				return Int32.Parse(o.ToString());
		}

        public DataSet GetUserContactInfo(int _intUserId)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@UserId", _intUserId);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getUserContactInfo", arParams);
        }

        public int Add(string _xid, string _pnc_id, string _fname, string _lname, int _manager, int _ismanager, int _board, int _director, string _pager, int _atid, string _phone, string _other, int _vacation, int _multiple_apps, int _add_location, int _admin, int _enabled)
		{
            while (_xid.Contains("-PNC") == true)
                _xid = _xid.Replace("-PNC", "");
            while (_pnc_id.Contains("-PNC") == true)
                _pnc_id = _pnc_id.Replace("-PNC", "");
            if (logging == true) 
				oLog.Add("Add user account " + _xid);
			arParams = new SqlParameter[18];
            arParams[0] = new SqlParameter("@xid", _xid);
            arParams[1] = new SqlParameter("@pnc_id", _pnc_id);
            arParams[2] = new SqlParameter("@fname", _fname);
            arParams[3] = new SqlParameter("@lname", _lname);
            arParams[4] = new SqlParameter("@manager", _manager);
            arParams[5] = new SqlParameter("@ismanager", _ismanager);
            arParams[6] = new SqlParameter("@board", _board);
            arParams[7] = new SqlParameter("@director", _director);
            arParams[8] = new SqlParameter("@pager", _pager);
            arParams[9] = new SqlParameter("@atid", _atid);
            arParams[10] = new SqlParameter("@phone", _phone);
            arParams[11] = new SqlParameter("@other", _other);
            arParams[12] = new SqlParameter("@vacation", _vacation);
            arParams[13] = new SqlParameter("@multiple_apps", _multiple_apps);
            arParams[14] = new SqlParameter("@add_location", _add_location);
            arParams[15] = new SqlParameter("@admin", _admin);
            arParams[16] = new SqlParameter("@enabled", _enabled);
            arParams[17] = new SqlParameter("@userid", SqlDbType.Int);
            arParams[17].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addUser", arParams);
            return Int32.Parse(arParams[17].Value.ToString());
		}
        public void Update(int _id, string _xid, string _pnc_id, string _fname, string _lname, int _manager, int _ismanager, int _board, int _director, string _pager, int _atid, string _phone, string _other, int _vacation, int _multiple_apps, int _ungroup_projects, int _show_returns, int _add_location, int _admin, int _enabled)
		{
			if (logging == true) 
				oLog.Add("Update user account " + GetName(_id));
			arParams = new SqlParameter[20];
			arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@xid", _xid);
            arParams[2] = new SqlParameter("@pnc_id", _pnc_id);
            arParams[3] = new SqlParameter("@fname", _fname);
            arParams[4] = new SqlParameter("@lname", _lname);
            arParams[5] = new SqlParameter("@manager", _manager);
            arParams[6] = new SqlParameter("@ismanager", _ismanager);
            arParams[7] = new SqlParameter("@board", _board);
            arParams[8] = new SqlParameter("@director", _director);
            arParams[9] = new SqlParameter("@pager", _pager);
            arParams[10] = new SqlParameter("@atid", _atid);
            arParams[11] = new SqlParameter("@phone", _phone);
            arParams[12] = new SqlParameter("@other", _other);
            arParams[13] = new SqlParameter("@vacation", _vacation);
            arParams[14] = new SqlParameter("@multiple_apps", _multiple_apps);
            arParams[15] = new SqlParameter("@ungroup_projects", _ungroup_projects);
            arParams[16] = new SqlParameter("@show_returns", _show_returns);
            arParams[17] = new SqlParameter("@add_location", _add_location);
            arParams[18] = new SqlParameter("@admin", _admin);
            arParams[19] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateUser", arParams);
		}

        public void AddUpdateUserContactInfo(int _intUserId,
                                             string _strWork_Title,
                                             string _strWork_Company,
                                             string _strWork_Department,
                                             string _strWork_Phone,
                                             string _strWork_Fax,
                                             string _strWork_CellPhone,
                                             string _strWork_Pager,
                                             int _intWork_PagerATId,
                                             string _strWork_Email,
                                             string _strWork_MailLocator,
                                             string _strWork_OfficeNo,
                                             string _strWork_AddressLine1,
                                             string _strWork_AddressLine2,
                                             string _strWork_City,
                                             string _strWork_State,
                                             string _strWork_ZIP,
                                             string _strWork_Country,
                                             string _strHome_Phone,
                                             string _strHome_Fax,
                                             string _strHome_CellPhone,
                                             string _strHome_Pager,
                                             int _intHome_PagerATId,
                                             string _strHome_Email,
                                             string _strHome_AddressLine1,
                                             string _strHome_AddressLine2,
                                             string _strHome_City,
                                             string _strHome_State,
                                             string _strHome_ZIP,
                                             string _strHome_Country,
                                             int _intCreatedby,
                                             int _intModifiedBy,
                                             int _intEnabled,
                                             int _intDeleted)
        {

            arParams = new SqlParameter[35];
            arParams[0] = new SqlParameter("@UserId", _intUserId);
            arParams[1] = new SqlParameter("@Work_Title", _strWork_Title);
            arParams[2] = new SqlParameter("@Work_Company", _strWork_Company);
            arParams[3] = new SqlParameter("@Work_Department", _strWork_Department);
            arParams[4] = new SqlParameter("@Work_Phone", _strWork_Phone);
            arParams[5] = new SqlParameter("@Work_Fax", _strWork_Fax);
            arParams[6] = new SqlParameter("@Work_CellPhone", _strWork_CellPhone);
            arParams[7] = new SqlParameter("@Work_Pager", _strWork_Pager);
            arParams[8] = new SqlParameter("@Work_PagerATId", _intWork_PagerATId);
            arParams[9] = new SqlParameter("@Work_Email", _strWork_Email);
            arParams[10] = new SqlParameter("@Work_MailLocator", _strWork_MailLocator);
            arParams[11] = new SqlParameter("@Work_OfficeNo", _strWork_OfficeNo);
            arParams[12] = new SqlParameter("@Work_AddressLine1", _strWork_AddressLine1);
            arParams[13] = new SqlParameter("@Work_AddressLine2", _strWork_AddressLine2);
            arParams[14] = new SqlParameter("@Work_City", _strWork_City);
            arParams[15] = new SqlParameter("@Work_State", _strWork_State);
            arParams[16] = new SqlParameter("@Work_ZIP", _strWork_ZIP);
            arParams[17] = new SqlParameter("@Work_Country", _strWork_Country);
            arParams[18] = new SqlParameter("@Home_Phone", _strHome_Phone);
            arParams[19] = new SqlParameter("@Home_Fax", _strHome_Fax);
            arParams[20] = new SqlParameter("@Home_CellPhone", _strHome_CellPhone);
            arParams[21] = new SqlParameter("@Home_Pager", _strHome_Pager);
            arParams[22] = new SqlParameter("@Home_PagerATId", _intHome_PagerATId);
            arParams[23] = new SqlParameter("@Home_Email", _strHome_Email);
            arParams[24] = new SqlParameter("@Home_AddressLine1", _strHome_AddressLine1);
            arParams[25] = new SqlParameter("@Home_AddressLine2", _strHome_AddressLine2);
            arParams[26] = new SqlParameter("@Home_City", _strHome_City);
            arParams[27] = new SqlParameter("@Home_State", _strHome_State);
            arParams[28] = new SqlParameter("@Home_ZIP", _strHome_ZIP);
            arParams[29] = new SqlParameter("@Home_Country", _strHome_Country);
            arParams[30] = new SqlParameter("@Createdby", _intCreatedby);
            arParams[31] = new SqlParameter("@ModifiedBy", _intModifiedBy);
            arParams[32] = new SqlParameter("@Enabled", _intEnabled);
            arParams[33] = new SqlParameter("@Deleted", _intDeleted);


            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addUpdateUserContactInfo", arParams);
        
        }



        public void UpdateProfileUpdateByUser(int _id, string _pnc_id, string _fname, string _lname, int _manager, string _pager, int _atid, string _phone, string _other)
        {
            if (logging == true)
                oLog.Add("Update user account " + GetName(_id));
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@pnc_id", _pnc_id);
            arParams[2] = new SqlParameter("@fname", _fname);
            arParams[3] = new SqlParameter("@lname", _lname);
            arParams[4] = new SqlParameter("@manager", _manager);
            arParams[5] = new SqlParameter("@pager", _pager);
            arParams[6] = new SqlParameter("@atid", _atid);
            arParams[7] = new SqlParameter("@phone", _phone);
            arParams[8] = new SqlParameter("@other", _other);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateUserProfileByUser", arParams);
        }

        public void Update(int _id, int _manager)
        {
            if (logging == true)
                oLog.Add("Update user account MANAGER " + GetName(_id));
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@manager", _manager);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateUserManager", arParams);
        }
        public void Update(int _id, string _xid, string _pnc_id, string _fname, string _lname, int _manager, string _phone)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@xid", _xid);
            arParams[2] = new SqlParameter("@pnc_id", _pnc_id);
            arParams[3] = new SqlParameter("@fname", _fname);
            arParams[4] = new SqlParameter("@lname", _lname);
            arParams[5] = new SqlParameter("@manager", _manager);
            arParams[6] = new SqlParameter("@phone", _phone);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateUserAD", arParams);
        }
        public void Update(int _id, string _xid, string _pnc_id)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@xid", _xid);
            arParams[2] = new SqlParameter("@pnc_id", _pnc_id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateUserID", arParams);
        }
        public void Enable(int _id, int _enabled) 
		{
			if (logging == true) 
			{
				if (_enabled == 1)
					oLog.Add("Enable user account " + GetName(_id));
				else
					oLog.Add("Disable user account " + GetName(_id));
			}
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@enabled", _enabled);
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateUserEnabled", arParams);
		}
		public void Delete(int _id)
		{
			if (logging == true) 
				oLog.Add("Delete user account " + GetName(_id));
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteUser", arParams);
		}
        public int Login(string _username, string _password, int _environment, bool _log, bool _admin)
        {
            bool boolDebug = false;
            string strDebug = ConfigurationManager.AppSettings["LoginDebug"];
            if (strDebug != null && strDebug != "")
            {
                string[] strDebugs = strDebug.Split(new char[] { ';' });
                for (int ii = 0; ii < strDebugs.Length; ii++)
                {
                    if (strDebugs[ii].ToString().Trim() != "" && strDebugs[ii].ToString().ToUpper() == _username.ToUpper())
                    {
                        boolDebug = true;
                        break;
                    }
                }
            }
            Variables oNCB = new Variables(_environment);
            int _id = GetId(_username);
            if (boolDebug)
                oLog.AddEvent(_username, _username, "Username Returned ID = " + _id.ToString(), LoggingType.Information);
            if (_id != 0)
            {
                if (boolDebug)
                    oLog.AddEvent(_username, _username, "Checking against environment " + _environment.ToString(), LoggingType.Information);
                DirectoryEntry oEntry = new DirectoryEntry(oNCB.primaryDC(dsn), oNCB.Domain() + "\\" + _username, _password);
                DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
                oSearcher.Filter = "(objectCategory=user)";
                try
                {
                    SearchResult oResult = oSearcher.FindOne();
                    AddLogin(_username);
                    if (boolDebug)
                        oLog.AddEvent(_username, _username, "Valid Login # 1!", LoggingType.Information);
                    return _id;
                }
                catch (Exception exEnvironment)
                {
                    if (boolDebug)
                        oLog.AddEvent(_username, _username, "Environment " + _environment.ToString() + " error # 1 = " + exEnvironment.Message, LoggingType.Error);
                    // ADD PNC Authentication
                    if (_environment == (int)CurrentEnvironment.CORPDMN)
                    {
                        Variables oPNC = new Variables((int)CurrentEnvironment.PNCNT_PROD);
                        DirectoryEntry oPNCEntry = new DirectoryEntry(oPNC.primaryDC(dsn), oPNC.Domain() + "\\" + _username, _password);
                        DirectorySearcher oPNCSearcher = new DirectorySearcher(oPNCEntry);
                        oSearcher.Filter = "(objectCategory=user)";
                        try
                        {
                            SearchResult oPNCResult = oPNCSearcher.FindOne();
                            AddLogin(_username);
                            if (boolDebug)
                                oLog.AddEvent(_username, _username, "Valid Login # 2!", LoggingType.Information);
                            return _id;
                        }
                        catch (Exception exPNCNT)
                        {
                            // ADD PNC Authentication
                            if (boolDebug)
                                oLog.AddEvent(_username, _username, "Environment " + ((int)CurrentEnvironment.PNCNT_PROD).ToString() + " error # 2 = " + exPNCNT.Message, LoggingType.Error);
                            return -10;
                        }
                    }
                    else if (_environment == (int)CurrentEnvironment.PNCNT_PROD || _environment == (int)CurrentEnvironment.PNCNT_LOCALHOST)
                    {
                        oNCB = new Variables((int)CurrentEnvironment.CORPDMN);
                        DirectoryEntry oNCBEntry = new DirectoryEntry(oNCB.primaryDC(dsn), oNCB.Domain() + "\\" + _username, _password);
                        DirectorySearcher oNCBSearcher = new DirectorySearcher(oNCBEntry);
                        oNCBSearcher.Filter = "(objectCategory=user)";
                        try
                        {
                            SearchResult oNCBResult = oNCBSearcher.FindOne();
                            AddLogin(_username);
                            if (boolDebug)
                                oLog.AddEvent(_username, _username, "Valid Login # 3!", LoggingType.Information);
                            return _id;
                        }
                        catch (Exception exCORPDMN)
                        {
                            // ADD PNC Authentication
                            if (boolDebug)
                                oLog.AddEvent(_username, _username, "Environment " + ((int)CurrentEnvironment.CORPDMN).ToString() + " error # 3 = " + exCORPDMN.Message, LoggingType.Error);
                            return -100;
                        }
                    }
                    else
                    {
                        if (boolDebug)
                            oLog.AddEvent(_username, _username, "Environment " + _environment.ToString() + " not equal to a known configured environment", LoggingType.Error);
                        return -1;
                    }
                }
            }
            else
            {
                if (boolDebug)
                    oLog.AddEvent(_username, _username, "Checking aggainst admin account", LoggingType.Information);
                Settings oSetting = new Settings(user, dsn);
                if (oEncrypt.Encrypt(_username, "uncview") == oSetting.Get("username") && oEncrypt.Encrypt(_password, "pwcview") == oSetting.Get("password"))
                {
                    if (boolDebug)
                        oLog.AddEvent(_username, _username, "Admin accepted", LoggingType.Information);
                    return -2;
                }
                else
                {
                    if (boolDebug)
                        oLog.AddEvent(_username, _username, "Admin invalid", LoggingType.Error);
                    if (_admin == true)
                    {
                        arParams = new SqlParameter[2];
                        arParams[0] = new SqlParameter("@username", _username);
                        arParams[1] = new SqlParameter("@password", _password);
                        SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addLoginInvalid", arParams);
                    }
                    return 0;
                }
            }
        }
        public void AddLogin(string _username)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@xid", _username);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addLogin", arParams);
        }
        public DataSet GetLogins()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLogins");
        }
        public DataSet GetBoard()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getUserBoard");
        }
        public DataSet GetDirector()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getUserDirector");
        }
        public string GetApplicationUrl(int _userid, int _pageid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@pageid", _pageid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getUserApplication", arParams);
            if (o == null)
                return "";
            else
                return o.ToString();
        }
        public DataSet GetApplication(int _applicationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getUsersApplication", arParams);
            string strUser = "0";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (strUser == dr["userid"].ToString())
                    dr.Delete();
                else
                    strUser = dr["userid"].ToString();
            }
            return ds;
        }
        public DataSet GetReports(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getUserReports", arParams);
        }
        public DataSet GetManagerReports(int _userid, int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@requestid", _requestid);
            arParams[2] = new SqlParameter("@serviceid", _serviceid);
            arParams[3] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getUserManagerReports", arParams);
        }

        public DataSet GetUserReportingHierarchy(int _userid, int _intHierarchyLevel)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@HierarchyLevel", _intHierarchyLevel);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getUserReportingHierarchy", arParams);
        }


        public int GetManager(int _userid, bool _teamleadok)
        {
            int intManager = Int32.Parse(Get(_userid, "manager"));
            if (intManager == 0)
                return 0;
            else
            {
                if (Get(intManager, "ismanager") == "1")
                    return intManager;
                else
                {
                    if (intManager == _userid || _teamleadok == true)
                        return intManager;
                    else
                        return GetManager(intManager, _teamleadok);
                }
            }
        }
        public bool IsManager(int _userid, int _managerid, bool _teamleadok)
        {
            if (_userid == 0)
                return false;
            int intManager = Int32.Parse(Get(_userid, "manager"));
            int intOldManager = 0;
            while (intManager > 0 && intManager != intOldManager)
            {
                intOldManager = intManager;
                if (intManager == _managerid)
                {
                    if (Get(intManager, "ismanager") == "1")
                        return true;
                    else
                    {
                        if (_teamleadok == true)
                            return true;
                        else
                            intManager = GetManager(intManager, _teamleadok);
                    }
                }
                else
                {
                    if (intManager == _userid)
                        return false;
                    else
                        intManager = GetManager(intManager, _teamleadok);
                }
            }
            return false;
        }
        public bool IsManager(string _xid, int _environment)
        {
            AD oAD = new AD(0, dsn, _environment);
            SearchResultCollection oResult = oAD.Search(_xid, "cn");
            bool boolReturn = false;
            if (oResult.Count > 0 && oResult[0].GetDirectoryEntry().Properties.Contains("directreports") == true)
                boolReturn = true;
            return boolReturn;
        }

        public void AddPage(int _pageid, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@pageid", _pageid);
            arParams[1] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addUserPage", arParams);
        }
        public void GetPages(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_getUserPages", arParams);
        }
        public DataSet GetPages(int _userid, int _parent, int _link, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@parent", _parent);
            arParams[2] = new SqlParameter("@link", _link);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getUserPagesTree", arParams);
        }
        public void DeletePage(int _parent, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteUserPage", arParams);
        }
        public void DeletePage(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteUserPages", arParams);
        }
        public DataSet GetApplicationMgmt(int _applicationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getUsersApplicationMgmt", arParams);
        }
        public bool IsAdmin(int _userid)
        {
            return (Get(_userid, "admin") == "1");
        }
        public int Register(string _pnc_id, int _default_group_id, int _environment)
        {
            int intUserID = 0;
            Functions oFunction = new Functions(user, dsn, _environment);
            // First locate the object
            SearchResultCollection oCollection = oFunction.eDirectory(_pnc_id);
            if (oCollection.Count == 1) 
            {
                string strXID = _pnc_id;
                if (oCollection[0].GetDirectoryEntry().Properties.Contains("businesscategory"))
                    strXID = oCollection[0].GetDirectoryEntry().Properties["businesscategory"].Value.ToString();
                string strFirst = "";
                if (oCollection[0].GetDirectoryEntry().Properties.Contains("givenname"))
                    strFirst = oCollection[0].GetDirectoryEntry().Properties["givenname"].Value.ToString();
                string strLast = "";
                if (oCollection[0].GetDirectoryEntry().Properties.Contains("sn"))
                    strLast = oCollection[0].GetDirectoryEntry().Properties["sn"].Value.ToString();
                string strPhone = "";
                if (oCollection[0].GetDirectoryEntry().Properties.Contains("telephonenumber"))
                    strPhone = oCollection[0].GetDirectoryEntry().Properties["telephonenumber"].Value.ToString();

                if (oCollection[0].Properties.Contains("pncmanagerid") == true)
                {
                    // Get manager
                    string strManager = oFunction.eDirectory(oCollection[0], "pncmanagerid");
                    intUserID = Register(_pnc_id, strXID, strFirst, strLast, strPhone, strManager, _default_group_id, _environment);
                }
            }
            return intUserID;
        }
        public int Register(string _pnc_id, string _xid, string _first, string _last, string _phone, string _manager, int _default_group_id, int _environment)
        {
            int intUserID = 0;
            Roles oRole = new Roles(user, dsn);

            if (_manager == "")
            {
                // We've reached the top level.  
                // Add account
                intUserID = Add(_xid, _pnc_id, _first, _last, 0, 0, 0, 0, "", 0, _phone, "", 0, 0, 0, 0, 1);
                // Configure the default clearview application
                oRole.Add(intUserID, _default_group_id);
            }
            else
            {
                // This person has a manager.  Locate the UserID  
                int intManager = GetId(_manager);
                if (intManager == 0)
                {
                    // Manager does not exist, so add him/her
                    intManager = Register(_manager, _default_group_id, _environment);
                }
                // Add account
                intUserID = Register(_pnc_id, _xid, _first, _last, _phone, intManager, _default_group_id, _environment);
            }
            return intUserID;
        }
        public int Register(string _pnc_id, string _xid, string _first, string _last, string _phone, int _manager, int _default_group_id, int _environment)
        {
            int intUserID = 0;
            Roles oRole = new Roles(user, dsn);
            // Add account
            intUserID = Add(_xid, _pnc_id, _first, _last, _manager, 0, 0, 0, "", 0, _phone, "", 0, 0, 0, 0, 1);
            // Add Manager's Role(s)
            DataSet dsRoles = oRole.Gets(_manager);
            foreach (DataRow drRole in dsRoles.Tables[0].Rows)
            {
                int intApp = Int32.Parse(drRole["applicationid"].ToString());
                oRole.Add(intUserID, oRole.Get(_manager, intApp));
            }
            return intUserID;
        }
    }
}
