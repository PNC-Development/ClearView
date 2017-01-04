using System;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using Microsoft.ApplicationBlocks.Data;
using System.Reflection;
using ActiveDs;
using CDOEXM;
using System.Text;
using System.Security.Principal;

namespace NCC.ClearView.Application.Core
{
	public class AD
	{
        private int intEnvironment;
        private const int ADS_UF_ACCOUNTDISABLE = 0x00000002;
        private const int ADS_UF_LOCKOUT = 0x00000010;
        private const int ADS_UF_PASSWD_NOTREQD = 0x00000020;
        private const int ADS_UF_PASSWD_CANT_CHANGE = 0x000000040;
        private const int ADS_UF_WORKSTATION_TRUST_ACCOUNT = 0x00001000;
        private const int ADS_UF_DONT_EXPIRE_PASSWD = 0x00010000;
        private const int ADS_UF_PASSWORD_EXPIRED = 0x00800000;
        private const string PASSWORD_GUID = "{ab721a53-1e2f-11d0-9819-00aa0040529b}";
        private string dsn = "";
        private int user = 0;
        private SqlParameter[] arParams;

        public AD(int _user, string _dsn, int _environment)
		{
            user = _user;
            dsn = _dsn;
            Settings oSetting = new Settings(_user, _dsn);
            intEnvironment = _environment;
		}
		public SearchResultCollection Search(string _user, string _filters) 
		{
			Variables oVariable = new Variables(intEnvironment);
            DirectoryEntry oEntry = new DirectoryEntry(oVariable.primaryDC(dsn), oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
			DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
			string strFilter = "";
			string[] strType;
			char[] strSplit  = {';'};
			strType = _filters.Split(strSplit);
			for(int ii = 0; ii < strType.Length; ii++)
			{
				if (strFilter == "")
					strFilter = "(" + strType[ii] + "=" +_user + "*)";
				else
					strFilter = "(|(" + strType[ii] + "=" +_user + "*)" + strFilter + ")";
			}
			oSearcher.Filter = strFilter;
			return oSearcher.FindAll();
		}
        public SearchResultCollection ComputerSearch(string _cn)
        {
            Variables oVariable = new Variables(intEnvironment);
            DirectoryEntry oEntry = new DirectoryEntry(oVariable.primaryDC(dsn), oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
            DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
            oSearcher.Filter = "(&(objectCategory=computer)(cn=" + _cn + "*))";
            return oSearcher.FindAll();
        }
        public DirectoryEntry UserSearch(string _samaccountname)
        {
            Variables oVariable = new Variables(intEnvironment);
            DirectoryEntry oEntry = new DirectoryEntry(oVariable.primaryDC(dsn), oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
            DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
            oSearcher.Filter = "(&(objectCategory=Person)(samaccountname=" + _samaccountname + "*))";
            SearchResult oResult = oSearcher.FindOne();
            if (oResult == null)
                return null;
            else
                return oResult.GetDirectoryEntry();
        }
        public SearchResultCollection UserSearch(string _user, string _filters)
        {
            Variables oVariable = new Variables(intEnvironment);
            DirectoryEntry oEntry = new DirectoryEntry(oVariable.primaryDC(dsn), oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
            DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
            string strFilter = "";
            string[] strType;
            char[] strSplit = { ';' };
            strType = _filters.Split(strSplit);
            for (int ii = 0; ii < strType.Length; ii++)
            {
                if (strFilter == "")
                    strFilter = "(" + strType[ii] + "=" + _user + "*)";
                else
                    strFilter = "(|(" + strType[ii] + "=" + _user + "*)" + strFilter + ")";
            }
            oSearcher.Filter = "(&(objectClass=organizationalPerson)" + strFilter + ")";
            return oSearcher.FindAll();
        }
        public string GetUserFullName(string _samaccountname)
        {
            Variables oVariable = new Variables(intEnvironment);
            DirectoryEntry oEntry = new DirectoryEntry(oVariable.primaryDC(dsn), oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
            DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
            oSearcher.Filter = "(&(objectCategory=Person)(samaccountname=" + _samaccountname + "*))";
            SearchResult oResult = oSearcher.FindOne();
            if (oResult == null)
                return "Not Found";
            else
                return oResult.GetDirectoryEntry().Properties["displayName"].Value.ToString();
        }
        public DirectoryEntry GroupSearch(string _samaccountname)
        {
            Variables oVariable = new Variables(intEnvironment);
            DirectoryEntry oEntry = new DirectoryEntry(oVariable.primaryDC(dsn), oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
            DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
            oSearcher.Filter = "(&(objectCategory=group)(samaccountname=" + _samaccountname + "*))";
            SearchResult oResult = oSearcher.FindOne();
            if (oResult == null)
                return null;
            else
                return oResult.GetDirectoryEntry();
        }
        public string GetGroupName(string _samaccountname)
        {
            Variables oVariable = new Variables(intEnvironment);
            DirectoryEntry oEntry = new DirectoryEntry(oVariable.primaryDC(dsn), oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
            DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
            oSearcher.Filter = "(&(objectCategory=group)(samaccountname=" + _samaccountname + "*))";
            SearchResult oResult = oSearcher.FindOne();
            if (oResult == null)
                return "Not Found";
            else
                return oResult.GetDirectoryEntry().Properties["name"].Value.ToString();
        }
        public DirectoryEntry Search(string _samaccountname, bool _computer) 
        {
            Variables oVariable = new Variables(intEnvironment);
            DirectoryEntry oEntry = new DirectoryEntry(oVariable.primaryDC(dsn), oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
            DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
            oSearcher.Filter = "(samaccountname=" + _samaccountname + (_computer ? "$" : "") + ")";
            SearchResult oResult = oSearcher.FindOne();
            if (oResult == null)
                return null;
            else
                return oResult.GetDirectoryEntry();
        }
        public DirectoryEntry SearchOU(string _name)
        {
            Variables oVariable = new Variables(intEnvironment);
            DirectoryEntry oEntry = new DirectoryEntry(oVariable.primaryDC(dsn), oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
            DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
            oSearcher.Filter = "(&(objectCategory=organizationalUnit)(name=" + _name + "*))";
            SearchResult oResult = oSearcher.FindOne();
            if (oResult == null)
                return null;
            else
                return oResult.GetDirectoryEntry();
            
        }
		public string Unlock(DirectoryEntry oEntry) 
		{
            string strReturn = "";
            try
            {
			    ActiveDs.LargeIntegerClass oLarge = new ActiveDs.LargeIntegerClass();
			    oLarge.HighPart = 0;
			    oLarge.LowPart = 0;
			    oEntry.Properties["lockouttime"].Value = oLarge;
			    oEntry.CommitChanges();
            }
            catch (Exception er)
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public string GetSID(DirectoryEntry oEntry)
        {
            string strReturn = "";
            try
            {
                SecurityIdentifier sid = new SecurityIdentifier((byte[])oEntry.Properties["objectSid"].Value, 0);
                strReturn = sid.ToString();
            }
            catch (Exception er)
            {
                //strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public bool IsLocked(DirectoryEntry oEntry) 
		{
			ActiveDs.LargeInteger oLarge;
			if (oEntry.Properties.Contains("lockouttime") == true) 
			{
				oLarge = (ActiveDs.LargeInteger)oEntry.Properties["lockouttime"].Value;
				return (oLarge.HighPart != 0 || oLarge.LowPart != 0);
			}
			else
				return false;
		}
        public string GetLockoutTime(DirectoryEntry oEntry)
        {
            string strDate = "";
            ActiveDs.LargeInteger oLarge;
            if (oEntry.Properties.Contains("lockouttime") == true)
            {
                oLarge = (ActiveDs.LargeInteger)oEntry.Properties["lockouttime"].Value;
                long _date = (long)oLarge.HighPart << 32 | (uint)oLarge.LowPart;
                strDate = DateTime.FromFileTime(_date).ToString();
            }
            return strDate;
        }
        public string Delete(DirectoryEntry oEntry)
        {
            string strReturn = "";
            try
            {
                string strPath = oEntry.Path;
                Variables oVariable = new Variables(intEnvironment);
                DirectoryEntry oDelete = new DirectoryEntry(strPath, oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
                oDelete.DeleteTree();
                oDelete.CommitChanges();
            }
            catch (Exception er)
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public string GetGroups(DirectoryEntry oEntry)
        {
            // returns as a ; delimited string
            string strGroups = "";
            int intLetterCount = 0;
            if (oEntry.Properties.Contains("memberof") == true)
            {
                foreach (string strGroup in oEntry.Properties["memberof"])
                {
                    string strTemp = strGroup;
                    if (strTemp.Length == 1)
                    {
                        intLetterCount++;
                        if (intLetterCount > 3)
                        {
                            if (strTemp[1] == ',')
                                break;
                            else
                                strGroups += strTemp;
                        }
                    }
                    else
                    {
                        if (strTemp.Length > 3)
                            strTemp = strTemp.Substring(3);
                        if (strTemp.IndexOf(",") > 0)
                            strTemp = strTemp.Substring(0, strTemp.IndexOf(","));
                        strGroups += strTemp + ";";
                    }
                }
            }
            return strGroups;
        }
        public string GetGroupMembers(DirectoryEntry oEntry)
        {
            // returns as a ; delimited string
            string strGroups = "";
            int intLetterCount = 0;
            if (oEntry.Properties.Contains("member") == true)
            {
                foreach (string strGroup in oEntry.Properties["member"])
                {
                    string strTemp = strGroup;
                    if (strTemp.Length == 1)
                    {
                        intLetterCount++;
                        if (intLetterCount > 3)
                        {
                            if (strTemp[1] == ',')
                                break;
                            else
                                strGroups += strTemp;
                        }
                    }
                    else
                    {
                        if (strTemp.Length > 3)
                            strTemp = strTemp.Substring(3);
                        if (strTemp.IndexOf(",") > 0)
                            strTemp = strTemp.Substring(0, strTemp.IndexOf(","));
                        strGroups += strTemp + ";";
                    }
                }
            }
            return strGroups;
        }
        public string Enable(DirectoryEntry oEntry, bool _enable)
        {
            string strReturn = "";
            try
            {
                int val = (int)oEntry.Properties["userAccountControl"].Value;
                if (_enable == true)
                    oEntry.Properties["userAccountControl"].Value = val & ~ADS_UF_ACCOUNTDISABLE;
                else
                    oEntry.Properties["userAccountControl"].Value = val | ADS_UF_ACCOUNTDISABLE;
                oEntry.CommitChanges(); 
            }
            catch (Exception er)
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public bool Exists(DirectoryEntry oEntry)
        {
            return DirectoryEntry.Exists(oEntry.Path);
        }
        public bool IsDisabled(DirectoryEntry oEntry)
        {
            int val = (int)oEntry.Properties["userAccountControl"].Value;
            return ((val & ADS_UF_ACCOUNTDISABLE) == ADS_UF_ACCOUNTDISABLE);
        }
        public string SetExpiration(DirectoryEntry oEntry, DateTime _expires)
        {
            string strReturn = "";
            try
            {
                Type type = oEntry.NativeObject.GetType();
                Object adsNative = oEntry.NativeObject;
                type.InvokeMember("AccountExpirationDate", BindingFlags.SetProperty, null, adsNative, new object[] { _expires.ToShortDateString() });
                oEntry.CommitChanges();
            }
            catch (Exception er)
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public string SetDoNotExpire(DirectoryEntry oEntry, bool _do_not_expire)
        {
            string strReturn = "";
            try
            {
                int val = (int)oEntry.Properties["userAccountControl"].Value;
                if (_do_not_expire == true)
                    oEntry.Properties["userAccountControl"].Value = val | ADS_UF_DONT_EXPIRE_PASSWD;
                else
                    oEntry.Properties["userAccountControl"].Value = val & ~ADS_UF_DONT_EXPIRE_PASSWD;
                oEntry.CommitChanges();
            }
            catch (Exception er)
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public bool IsDoNotExpire(DirectoryEntry oEntry)
        {
            int val = (int)oEntry.Properties["userAccountControl"].Value;
            return ((val & ADS_UF_DONT_EXPIRE_PASSWD) == ADS_UF_DONT_EXPIRE_PASSWD);
        }
        public string SetCannotChangePassword(DirectoryEntry oEntry, bool _cannot_change_password)
        {
            string strReturn = "";
            try
            {
                string[] trustees = new string[]{@"NT AUTHORITY\SELF","EVERYONE"};

                IADsSecurityDescriptor sd = (ActiveDs.IADsSecurityDescriptor)oEntry.Properties["ntSecurityDescriptor"].Value;
                IADsAccessControlList acl = (IADsAccessControlList)sd.DiscretionaryAcl;
                IADsAccessControlEntry ace = new AccessControlEntry();
                foreach (string trustee in trustees)
                {
                    ace.Trustee = trustee;
                    ace.AceFlags = 0;
                    if (_cannot_change_password == true)
                        ace.AceType = (int)ADS_ACETYPE_ENUM.ADS_ACETYPE_ACCESS_DENIED_OBJECT;
                    else
                        ace.AceType = (int)ADS_ACETYPE_ENUM.ADS_ACETYPE_ACCESS_ALLOWED_OBJECT;
                    ace.Flags = (int)ADS_FLAGTYPE_ENUM.ADS_FLAG_OBJECT_TYPE_PRESENT;
                    ace.ObjectType = PASSWORD_GUID;
                    ace.AccessMask = (int)ADS_RIGHTS_ENUM.ADS_RIGHT_DS_CONTROL_ACCESS;
                    acl.AddAce(ace);
                }
                sd.DiscretionaryAcl = acl;
                oEntry.Properties["ntSecurityDescriptor"].Value = sd;
                oEntry.CommitChanges();
            }
            catch (Exception er)
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public bool IsCannotChangePassword(DirectoryEntry oEntry)
        {
            int val = (int)oEntry.Properties["userAccountControl"].Value;
            return ((val & ADS_UF_PASSWD_CANT_CHANGE) == ADS_UF_PASSWD_CANT_CHANGE);
        }
        public string SetPassword(DirectoryEntry oEntry, string _password)
        {
            string strReturn = "";
            try
            {
                oEntry.Invoke("SetPassword", new object[] { _password });
                oEntry.CommitChanges();
            }
            catch (Exception er)
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public string CreateOU(string _name, string _description, string _parent_path)
        {
            string strReturn = "";
            try
            {
                if (SearchOU(_name) == null)
                {
                    Variables oVariable = new Variables(intEnvironment);
                    string strPath = oVariable.OU();
                    if (_parent_path != "")
                        strPath = _parent_path;
                    strPath = "LDAP://" + oVariable.primaryDCName(dsn) + "/" + strPath + oVariable.LDAP();
                    DirectoryEntry oParent = new DirectoryEntry(strPath, oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
                    DirectoryEntry oGroup = oParent.Children.Add("OU=" + _name, "OrganizationalUnit");
                    //oGroup.Properties["samAccountName"].Value = _name;
                    if (_description != "")
                        oGroup.Properties["description"].Value = _description;
                    oGroup.CommitChanges();
                }
            }
            catch (Exception er)
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public string CreateGroup(string _name, string _description, string _notes, string _parent_path, string _scope, string _type)
        {
            string strReturn = "";
            try
            {
                if (Search(_name, false) == null)
                {
                    Variables oVariable = new Variables(intEnvironment);
                    string strPath = oVariable.GroupOU();
                    if (_parent_path != "")
                        strPath = _parent_path;
                    strPath = "LDAP://" + oVariable.primaryDCName(dsn) + "/" + strPath + oVariable.LDAP();
                    DirectoryEntry oParent = new DirectoryEntry(strPath, oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
                    DirectoryEntry oGroup = oParent.Children.Add("CN=" + _name, "group");
                    if (_type == "S")
                    {
                        if (_scope == "DLG")
                            oGroup.Properties["groupType"].Value = ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_DOMAIN_LOCAL_GROUP | ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_SECURITY_ENABLED;
                        if (_scope == "GG")
                            oGroup.Properties["groupType"].Value = ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_GLOBAL_GROUP | ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_SECURITY_ENABLED;
                        if (_scope == "UG")
                            oGroup.Properties["groupType"].Value = ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_UNIVERSAL_GROUP | ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_SECURITY_ENABLED;
                    }
                    if (_type == "D")
                    {
                        if (_scope == "DLG")
                            oGroup.Properties["groupType"].Value = ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_DOMAIN_LOCAL_GROUP;
                        if (_scope == "GG")
                            oGroup.Properties["groupType"].Value = ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_GLOBAL_GROUP;
                        if (_scope == "UG")
                            oGroup.Properties["groupType"].Value = ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_UNIVERSAL_GROUP;
                    }
                    oGroup.Properties["samAccountName"].Value = _name;
                    if (_description != "")
                        oGroup.Properties["description"].Value = _description;
                    if (_notes != "")
                        oGroup.Properties["info"].Value = _notes;
                    oGroup.CommitChanges();
                }
            }
            catch (Exception er) 
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public string CreateUser(string _name, string _fname, string _lname, string _password, string _description, string _notes, string _parent_path)
        {
            return CreateUser(_name, _fname, _lname, _password, _lname + ", " + _fname, _description, _notes, _parent_path, false, false);
        }
        public string CreateUser(string _name, string _fname, string _lname, string _password, string _displayName, string _description, string _notes, string _parent_path, bool _do_not_expire, bool _cannot_change_password)
        {
            string strReturn = "";
            try
            {
                if (Search(_name, false) == null)
                {
                    Variables oVariable = new Variables(intEnvironment);
                    string strPath = oVariable.UserOU();
                    if (_parent_path != "")
                        strPath = _parent_path;
                    strPath = "LDAP://" + oVariable.primaryDCName(dsn) + "/" + strPath + oVariable.LDAP();
                    DirectoryEntry oParent = new DirectoryEntry(strPath, oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
                    DirectoryEntry oUser = oParent.Children.Add("CN=" + _name.ToUpper(), "user");
                    oUser.Properties["samAccountName"].Value = _name.ToUpper();
                    oUser.Properties["name"].Value = _name.ToUpper();
                    oUser.Properties["givenname"].Value = _fname;
                    oUser.Properties["sn"].Value = _lname;
                    oUser.Properties["displayname"].Value = _displayName;
                    if (_description != "")
                        oUser.Properties["description"].Value = _description;
                    if (_notes != "")
                        oUser.Properties["info"].Value = _notes;
                    oUser.CommitChanges();
	                oUser.Invoke("setPassword", _password);
                	oUser.Properties["userAccountControl"].Value = 512;   // enable account - by default they are disabled
                    oUser.CommitChanges();
                    if (_cannot_change_password == true)
                        SetCannotChangePassword(oUser, true);
                    if (_do_not_expire == true)
                        SetDoNotExpire(oUser, true);
                }
            }
            catch (Exception er)
            {
                string message = er.Message;
                while (er.InnerException != null)
                {
                    er = er.InnerException;
                    message += " | " + er.Message;
                }
                strReturn = message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public string MoveAccount(DirectoryEntry oEntry, string _parent_path)
        {
            string strReturn = "";
            try
            {
                Variables oVariable = new Variables(intEnvironment);
                string strPath = oVariable.UserOU();
                if (_parent_path != "")
                {
                    if (_parent_path.IndexOf("DC=") > -1)
                        strPath = _parent_path.Substring(0, _parent_path.IndexOf("DC="));
                    else
                        strPath = _parent_path;
                }
                strPath = "LDAP://" + oVariable.primaryDCName(dsn) + "/" + strPath + oVariable.LDAP();
                DirectoryEntry oParent = new DirectoryEntry(strPath, oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
                oEntry.MoveTo(oParent);
            }
            catch (Exception er)
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public string Rename(DirectoryEntry oEntry, string _id, string _first, string _last)
        {
            string strReturn = "";
            try
            {
                if (_first != "" && _last != "")
                {
                    oEntry.Properties["givenname"].Value = _first;
                    oEntry.Properties["sn"].Value = _last;
                    oEntry.Properties["displayname"].Value = _last + ", " + _first;
                }
                oEntry.Properties["samAccountName"].Value = _id.ToUpper();
                oEntry.CommitChanges();
                // Rename CN
                oEntry.Rename("CN=" + _id.ToUpper());
                oEntry.CommitChanges();
            }
            catch (Exception er)
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public string Change(DirectoryEntry oEntry, string _scope, string _type)
        {
            string strReturn = "";
            try
            {
                if (_type == "S")
                {
                    // Change to Universal to allow change to all others
                    oEntry.Properties["groupType"].Value = ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_UNIVERSAL_GROUP | ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_SECURITY_ENABLED;
                    oEntry.CommitChanges();
                    if (_scope == "DLG")
                        oEntry.Properties["groupType"].Value = ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_DOMAIN_LOCAL_GROUP | ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_SECURITY_ENABLED;
                    else if (_scope == "GG")
                        oEntry.Properties["groupType"].Value = ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_GLOBAL_GROUP | ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_SECURITY_ENABLED;
                }
                else
                {
                    // Change to Universal to allow change to all others
                    oEntry.Properties["groupType"].Value = ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_UNIVERSAL_GROUP;
                    oEntry.CommitChanges();
                    if (_scope == "DLG")
                        oEntry.Properties["groupType"].Value = ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_DOMAIN_LOCAL_GROUP;
                    else if (_scope == "GG")
                        oEntry.Properties["groupType"].Value = ActiveDs.ADS_GROUP_TYPE_ENUM.ADS_GROUP_TYPE_GLOBAL_GROUP;
                }
                oEntry.CommitChanges();
            }
            catch (Exception er)
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public string MoveGroup(DirectoryEntry oEntry, string _parent_path)
        {
            string strReturn = "";
            try
            {
                Variables oVariable = new Variables(intEnvironment);
                string strPath = oVariable.GroupOU();
                if (_parent_path != "")
                {
                    if (_parent_path.IndexOf("DC=") > -1)
                        strPath = _parent_path.Substring(0, _parent_path.IndexOf("DC="));
                    else
                        strPath = _parent_path;
                }
                strPath = "LDAP://" + oVariable.primaryDCName(dsn) + "/" + strPath + oVariable.LDAP();
                DirectoryEntry oParent = new DirectoryEntry(strPath, oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
                oEntry.MoveTo(oParent);
            }
            catch (Exception er)
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public string JoinGroup(string strGroupToBeJoined, string strGroupJoinTo, int intOtherEnvironment)
        {
            string strReturn = "";
            try
            {
                DirectoryEntry oUser = Search(strGroupToBeJoined, false);
                string strSID = "";
                if (oUser == null && intOtherEnvironment > 0)
                {
                    // Search other domain
                    AD oAD999 = new AD(0, dsn, intOtherEnvironment);
                    Variables oVariable999 = new Variables(intOtherEnvironment);
                    DirectoryEntry oEntry999 = new DirectoryEntry("LDAP://" + oVariable999.primaryDCName(dsn) + "/" + oVariable999.LDAP(), oVariable999.Domain() + "\\" + oVariable999.ADUser(), oVariable999.ADPassword());
                    DirectorySearcher oSearcher999 = new DirectorySearcher(oEntry999);
                    oSearcher999.Filter = "(samaccountname=" + strGroupToBeJoined + ")";
                    SearchResult oResult999 = oSearcher999.FindOne();
                    strSID = oAD999.GetSID(oResult999.GetDirectoryEntry());
                }
                else if (oUser != null)
                    strSID = GetSID(oUser);

                DirectoryEntry oGroup = Search(strGroupJoinTo, false);
                if (strSID != "" && oGroup != null)
                    strReturn = JoinGroup(strSID, oGroup);
            }
            catch (Exception er)
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public string JoinGroup(string _sid, DirectoryEntry oGroup)
        {
            string strReturn = "";
            try
            {
                oGroup.Properties["member"].Add("<SID=" + _sid + ">");
                oGroup.CommitChanges();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException E)
            {
                string strTest = E.ExtendedErrorMessage;
                strReturn = E.Message;
                if (strReturn.Contains("The object already exists") == true)
                    strReturn = "";
            }
            return strReturn;
        }
        public string RemoveGroup(string strUser, string strGroup)
        {
            string strReturn = "";
            try
            {
                DirectoryEntry oUser = Search(strUser, false);
                DirectoryEntry oGroup = Search(strGroup, false);
                if (oUser != null && oGroup != null)
                    strReturn = RemoveGroup(oUser, oGroup);
            }
            catch (Exception er)
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public string RemoveGroup(DirectoryEntry oUser, DirectoryEntry oGroup)
        {
            string strReturn = "";
            try
            {
                oGroup.Properties["member"].Remove(oUser.Properties["distinguishedName"].Value);
                oGroup.CommitChanges();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException E)
            {
                string strTest = E.ExtendedErrorMessage;
                strReturn = E.Message;
            }
            return strReturn;
        }
        public string GetComputerOU(string _name)
        {
            Variables oVariable = new Variables(intEnvironment);
            if (_name.StartsWith("TXP") || _name.StartsWith("WXP"))
                return oVariable.WorkstationOU();
            else
            {
                string strOU = "";
                if (_name.Contains("APP"))
                    strOU = "OU=OUs_APP,";
                else if (_name.Contains("BTM"))
                    strOU = "OU=OUs_BTM,";
                else if (_name.Contains("CTX"))
                    strOU = "OU=OUs_CTX,";
                else if (_name.Contains("FIL"))
                    strOU = "OU=OUs_FIL,";
                else if (_name.Contains("IIS"))
                    strOU = "OU=OUs_IIS,";
                else if (_name.Contains("NAV"))
                    strOU = "OU=OUs_NAV,";
                else if (_name.Contains("PRT"))
                    strOU = "OU=OUs_PrtSrvs,";
                else if (_name.Contains("SMI"))
                    strOU = "OU=OUs_SMI,";
                else if (_name.Contains("SQL"))
                    strOU = "OU=OUs_SQL,";
                else if (_name.Contains("UTL"))
                    strOU = "OU=OUs_UTL,";
                else if (_name.Contains("VIC"))
                    strOU = "OU=OUs_VIC,";
                return strOU + oVariable.ServerOU();
            }
        }
        public string CreateWorkstation(string _name, string _description, string _notes, string _parent_path)
        {
            string strReturn = "";
            try
            {
                if (Search(_name, true) == null)
                {
                    Variables oVariable = new Variables(intEnvironment);
                    string strPath = oVariable.WorkstationOU();
                    if (_parent_path != "")
                        strPath = _parent_path;
                    strPath = "LDAP://" + oVariable.primaryDCName(dsn) + "/" + strPath + oVariable.LDAP();
                    DirectoryEntry oParent = new DirectoryEntry(strPath, oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
                    DirectoryEntry oObject = oParent.Children.Add("CN=" + _name.ToUpper(), "computer");
                    oObject.Properties["samAccountName"].Value = _name.ToUpper() + "$";
                    oObject.Properties["dnshostname"].Value = _name.ToUpper() + "." + oVariable.FullyQualified();
                    // The correct userAccountControl value for a computer is 4096.  (If this were a user account then the value would be 512 or 514.)
                    oObject.Properties["userAccountControl"].Value = ADS_UF_PASSWD_NOTREQD | ADS_UF_WORKSTATION_TRUST_ACCOUNT;
                    if (_description != "")
                        oObject.Properties["description"].Value = _description;
                    if (_notes != "")
                        oObject.Properties["info"].Value = _notes;
                    oObject.CommitChanges();
//                    oObject.Properties["userAccountControl"].Value = 512;   // enable workstation - by default they are disabled
//                    oObject.CommitChanges();
                }
            }
            catch (Exception er)
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public string CreateServer(string _name, string _description, string _notes, string _parent_path)
        {
            string strReturn = "";
            try
            {
                if (Search(_name, true) == null)
                {
                    Variables oVariable = new Variables(intEnvironment);
                    string strPath = _parent_path;
                    strPath = "LDAP://" + oVariable.primaryDCName(dsn) + "/" + strPath + oVariable.LDAP();
                    DirectoryEntry oParent = new DirectoryEntry(strPath, oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
                    DirectoryEntry oObject = oParent.Children.Add("CN=" + _name.ToUpper(), "computer");
                    oObject.Properties["samAccountName"].Value = _name.ToUpper() + "$";
                    oObject.Properties["dnshostname"].Value = _name.ToUpper() + "." + oVariable.FullyQualified();
                    // The correct userAccountControl value for a computer is 4096.  (If this were a user account then the value would be 512 or 514.)
                    oObject.Properties["userAccountControl"].Value = ADS_UF_PASSWD_NOTREQD | ADS_UF_WORKSTATION_TRUST_ACCOUNT;
                    if (_description != "")
                        oObject.Properties["description"].Value = _description;
                    if (_notes != "")
                        oObject.Properties["info"].Value = _notes;
                    oObject.CommitChanges();
                }
            }
            catch (Exception er)
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public string CreateMailbox(DirectoryEntry oEntry)
        {
            string strReturn = "";
            try
            {
                CDOEXM.IMailboxStore oMailbox = (IMailboxStore)oEntry.NativeObject;
                if (intEnvironment == 1 || intEnvironment == 2)
                {
                    if (oMailbox.HomeMDB == null)
                    {
                        oEntry.Properties["homeMDB"].Value = "CN=Mailbox Store 1 (OHCLEMSX4201),CN=First Storage Group,CN=InformationStore,CN=OHCLEMSX4201,CN=Servers,CN=NCB Administrative Group,CN=Administrative Groups,CN=NCC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=ntl-city,DC=dev";
                        oEntry.Properties["homeMTA"].Value = "CN=Microsoft MTA,CN=OHCLEMSX4201,CN=Servers,CN=NCB Administrative Group,CN=Administrative Groups,CN=NCC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=ntl-city,DC=dev";
                        oEntry.Properties["legacyExchangeDN"].Value = "/o=NCC/ou=First Administrative Group/cn=Recipients/cn=" + oEntry.Properties["name"].Value.ToString();
                        oEntry.Properties["mailNickname"].Value = oEntry.Properties["name"].Value.ToString();
                        oEntry.Properties["msExchHomeServerName"].Value = "/o=NCC/ou=First Administrative Group/cn=Configuration/cn=Servers/cn=OHCLEMSX4201";
                        oEntry.Properties["msExchUserAccountControl"].Value = 0;
                        oEntry.CommitChanges();
                    }
                    oMailbox.CreateMailbox("LDAP://Mailbox Store 1 (OHCLEMSX4201),CN=First Storage Group,CN=InformationStore,CN=OHCLEMSX4201,CN=Servers,CN=NCB Administrative Group,CN=Administrative Groups,CN=NCC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=corpdev,DC=ntl-city,DC=net");
                    oEntry.CommitChanges();
                }
                else if (intEnvironment == 3)
                {
                }
                else if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                {
                }
            }
            catch (Exception er)
            {
                if (er.Message != "Value does not fall within the expected range.")
                    strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public void AddGroup(string _name, int _domain)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@domain", _domain);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addActiveDirectoryGroup", arParams);
        }
        public void DeleteGroups(int _domain)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@domain", _domain);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteActiveDirectoryGroups", arParams);
        }
        public DataSet GetGroups(int _domain)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@domain", _domain);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getActiveDirectoryGroups", arParams);
        }
        public DataSet GetGroups(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getActiveDirectoryGroup", arParams);
        }
        public string MoveServer(DirectoryEntry oEntry, string _parent_path)
        {
            string strReturn = "";
            try
            {
                Variables oVariable = new Variables(intEnvironment);
                string strPath = oVariable.ServerOU();
                if (_parent_path != "")
                {
                    if (_parent_path.IndexOf("DC=") > -1)
                        strPath = _parent_path.Substring(0, _parent_path.IndexOf("DC="));
                    else
                        strPath = _parent_path;
                }
                strPath = "LDAP://" + oVariable.primaryDCName(dsn) + "/" + strPath + oVariable.LDAP();
                DirectoryEntry oParent = new DirectoryEntry(strPath, oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
                oEntry.MoveTo(oParent);
            }
            catch (Exception er)
            {
                strReturn = er.Message + " ~ (Source: " + er.Source + ") (Stack Trace: " + er.StackTrace + ")";
            }
            return strReturn;
        }
        public int Sync(double dblSleep, bool _update)
        {
            Settings oSetting = new Settings(0, dsn);
            Users oUser = new Users(0, dsn);
            Roles oRole = new Roles(0, dsn);
            Variables oVariable = new Variables(intEnvironment);

            string strADSync = oSetting.Get("ad_sync").ToUpper();
            StringBuilder strResults = new StringBuilder();
            int intCreates = 0;
            int intUpdates = 0;
            int intCount = 0;
            int intAttempt = 0;
            int intAttempts = 100;

            // Process for strADSync (Example = XAB where X = Letter1, A = Letter2, B = Letter3)
            while (intCount == 0 && intAttempt < intAttempts)
            {
                intAttempt++;
                SearchResultCollection oResults = Search(strADSync, "sAMAccountName");
                strResults.Append(intAttempt.ToString() + ".) Searching " + strADSync + "% = ");
                if (oResults.Count == 0)
                {
                    strResults.Append("no results...increasing letter..." + Environment.NewLine);
                    strADSync = NextLetters(strADSync);
                }
                else
                {
                    strResults.Append("FOUND!!! (Count = " + oResults.Count.ToString() + ").  Begin processing..." + Environment.NewLine);
                    //strResults.Append(Environment.NewLine);

                    foreach (SearchResult oResult in oResults)
                    {
                        string strXID = "";
                        if (oResult.Properties.Contains("sAMAccountName") == true)
                            strXID = oResult.GetDirectoryEntry().Properties["sAMAccountName"].Value.ToString();
                        else if (oResult.Properties.Contains("mailnickname") == true)
                            strXID = oResult.GetDirectoryEntry().Properties["mailnickname"].Value.ToString();
                        string strFirst = "";
                        if (oResult.Properties.Contains("givenname") == true)
                            strFirst = oResult.GetDirectoryEntry().Properties["givenname"].Value.ToString();
                        string strLast = "";
                        if (oResult.Properties.Contains("sn") == true)
                            strLast = oResult.GetDirectoryEntry().Properties["sn"].Value.ToString();
                        int intManager = 0;
                        if (oResult.Properties.Contains("manager") == true)
                        {
                            string strManager = oResult.GetDirectoryEntry().Properties["manager"].Value.ToString();
                            DirectoryEntry oManager = new DirectoryEntry("LDAP://" + oVariable.primaryDCName(dsn) + "/" + strManager, oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
                            strManager = "";
                            if (oManager.Properties.Contains("sAMAccountName") == true)
                                strManager = oManager.Properties["sAMAccountName"].Value.ToString();
                            else if (oManager.Properties.Contains("mailnickname") == true)
                                strManager = oManager.Properties["mailnickname"].Value.ToString();
                            if (strManager != "")
                                intManager = oUser.GetId(strManager);
                        }
                        string strPhone = "";
                        if (oResult.Properties.Contains("telephonenumber") == true)
                            strPhone = oResult.GetDirectoryEntry().Properties["telephonenumber"].Value.ToString();

                        if (strXID != "")
                        {
                            strResults.Append(strXID + " (" + strFirst + " " + strLast + ")...");
                            /*
                            if (intEnvironment == 1 && strXID.ToUpper() == "ESXH33T")
                            {
                                DirectoryEntry oEntry = oResult.GetDirectoryEntry();
                                oEntry.Properties["extensionattribute10"].Value = "PT43054";
                                oEntry.CommitChanges();
                            }
                            */
                            if (oResult.Properties.Contains("extensionattribute10") == true)
                            {
                                string strID = oResult.GetDirectoryEntry().Properties["extensionattribute10"].Value.ToString();
                                int intUser = oUser.GetId(strID);

                                if (intUser == 0)
                                {
                                    // User does not exist - check NCB ID
                                    strResults.Append("no...trying XID...");
                                    intUser = oUser.GetId(strXID);
                                    if (intUser == 0)
                                    {
                                        strResults.Append("no...adding (" + strID + ")...");
                                        // Add the user
                                        int intUserNew = oUser.Add(strXID, strID, strFirst, strLast, intManager, 0, 0, 0, "", 0, strPhone, "", 0, 0, 0, 0, 1);
                                        intCount++;
                                        intCreates++;
                                        // Load Manager's Role(s)
                                        DataSet dsRoles = oRole.Gets(intManager);
                                        foreach (DataRow drRole in dsRoles.Tables[0].Rows)
                                        {
                                            int intApp = Int32.Parse(drRole["applicationid"].ToString());
                                            oRole.Add(intUserNew, oRole.Get(intManager, intApp));
                                        }
                                        if (intManager > 0)
                                            strResults.Append("manager = " + intManager.ToString() + ", userid = " + intUserNew.ToString() + "... ***ADDED***");
                                        else
                                            strResults.Append("userid = " + intUserNew.ToString() + "... ***ADDED***");
                                    }
                                }

                                if (intUser > 0)
                                {
                                    // User exists with NCB credentials - update to PNC
                                    DataSet dsUser = oUser.Get(intUser);
                                    if (dsUser.Tables[0].Rows.Count == 1)
                                    {
                                        string strUpdate = "";
                                        bool boolUpdate = false;
                                        DataRow drUser = dsUser.Tables[0].Rows[0];
                                        if (drUser["xid"].ToString().ToUpper() != strXID.ToUpper())
                                        {
                                            if (strUpdate != "")
                                                strUpdate += ", ";
                                            strUpdate += "X = " + strXID;
                                            boolUpdate = true;
                                        }
                                        if (drUser["pnc_id"].ToString().ToUpper() != strID.ToUpper())
                                        {
                                            if (strUpdate != "")
                                                strUpdate += ", ";
                                            strUpdate += "P = " + strID;
                                            boolUpdate = true;
                                        }
                                        if (drUser["fname"].ToString().ToUpper() != strFirst.ToUpper())
                                            strFirst = drUser["fname"].ToString();
                                        if (drUser["lname"].ToString().ToUpper() != strLast.ToUpper())
                                            strLast = drUser["lname"].ToString();
                                        int intManagerOld = 0;
                                        if (Int32.TryParse(drUser["manager"].ToString(), out intManagerOld) == true)
                                        {
                                            if (intManager == 0 && intManagerOld > 0)
                                                intManager = intManagerOld;
                                            if ((intManagerOld == 0 && intManager > 0) || (intManagerOld != intManager))
                                            {
                                                if (strUpdate != "")
                                                    strUpdate += ", ";
                                                strUpdate += "M = " + oUser.GetName(intManager);
                                                boolUpdate = true;
                                            }
                                        }
                                        if (drUser["phone"].ToString().Trim() != "" && drUser["phone"].ToString().ToUpper() != strPhone.ToUpper())
                                            strPhone = drUser["phone"].ToString();

                                        if (boolUpdate == true)
                                        {
                                            if (_update == true)
                                            {
                                                // Update User Profile from AD
                                                oUser.Update(intUser, strXID, strID, strFirst, strLast, intManager, strPhone);
                                                intCount++;
                                                intUpdates++;
                                                strResults.Append("found..." + strUpdate + "... ***UPDATED***");
                                            }
                                            else
                                            {
                                                // Skip for now
                                                strResults.Append("found...skipping update...");
                                            }
                                        }
                                        else
                                        {
                                            // no updates needed
                                            strResults.Append("found...nothing to update...");
                                        }
                                    }
                                    else
                                        strResults.Append("found...invalid user records (" + dsUser.Tables[0].Rows.Count.ToString() + ")...");
                                }
                            }
                            else
                                strResults.Append("extensionattribute10 does not exist");

                            strResults.Append(Environment.NewLine);
                        }
                    }

                    if (intCount == 0)
                    {
                        // No records were processed, increase letter...
                        strADSync = NextLetters(strADSync);
                    }
                }
            }
            strResults.Append("****************************************" + Environment.NewLine);
            strResults.Append("TOTAL ADDED = " + intCount.ToString() + Environment.NewLine);
            strResults.Append("****************************************" + Environment.NewLine);
            strResults.Append(Environment.NewLine);

            string strNewLetter = NextLetters(strADSync);
            oSetting.UpdateADSych(strNewLetter);
            strResults.Append("Increased AD SYNC LETTER to " + strNewLetter + Environment.NewLine);
            strResults.Append("Waiting " + dblSleep.ToString("0") + " minutes..." + Environment.NewLine);

            // Add to logging
            if (intCount == 0)
                return AddSync("---", strResults.ToString(), intCreates, intUpdates);
            else
                return AddSync(strADSync, strResults.ToString(), intCreates, intUpdates);
        }
        private string NextLetters(string _ad_sync)
        {
            string strLetter1 = _ad_sync[0].ToString();
            string strLetter2 = _ad_sync[1].ToString();
            string strLetter3 = _ad_sync[2].ToString();
            // Increase the letter array
            string strNewLetter1 = strLetter1;
            string strNewLetter2 = strLetter2;
            string strNewLetter3 = NextLetter(strLetter3);
            if (strNewLetter3 == "A")   // Reached the end and starting over, increase next letter
            {
                strNewLetter2 = NextLetter(strLetter2);
                if (strNewLetter2 == "A")   // Reached the end and starting over, increase next letter
                {
                    // Keep first letter = "X", comment line to prevent increase
                    //strNewLetter1 = NextLetter(strLetter1);
                    strNewLetter1 = "X";
                }
            }
            return strNewLetter1 + strNewLetter2 + strNewLetter3;
        }
        private string NextLetter(string _letter)
        {
            string strReturn = "";
            string[] letterArray = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            for (int ii = 0; ii < letterArray.Length; ii++)
            {
                if (letterArray[ii].ToUpper() == _letter.ToUpper())
                {
                    if (ii == (letterArray.Length - 1))
                        strReturn = "A";
                    else
                        strReturn = letterArray[ii + 1].ToUpper();
                    break;
                }
            }
            return strReturn;
        }
        public DataSet GetSync(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getADSync", arParams);
        }
        public string GetSync(int _id, string _column)
        {
            DataSet ds = GetSync(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetSync()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getADSyncs");
        }
        public int AddSync(string _ad_sync, string _results, int _creates, int _updates)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@ad_sync", _ad_sync);
            arParams[1] = new SqlParameter("@results", _results);
            arParams[2] = new SqlParameter("@creates", _creates);
            arParams[3] = new SqlParameter("@updates", _updates);
            arParams[4] = new SqlParameter("@id", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addADSync", arParams);
            return Int32.Parse(arParams[4].Value.ToString());
        }
    }
}
