using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.DirectoryServices;
using System.Text;

namespace NCC.ClearView.Application.Core
{
	public class AccountRequest
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public AccountRequest(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet GetApprovalUser(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAccountRequestsApproval", arParams);
        }
        public DataSet GetApproval(int _domainid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAccountRequestApproval", arParams);
        }
        public bool GetApproval(int _domainid, int _userid)
        {
            DataSet ds = GetApproval(_domainid);
            bool boolFound = false;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["userid"].ToString()) == _userid)
                {
                    boolFound = true;
                    break;
                }
            }
            return boolFound;
        }
        public DataSet GetExceptions(int _domainid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAccountRequestExceptions", arParams);
        }
        public bool GetExceptions(int _domainid, string _name)
        {
            DataSet ds = GetExceptions(_domainid);
            bool boolFound = false;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["exact"].ToString() == "1")
                {
                    if (_name.Trim().ToUpper() == dr["name"].ToString().Trim().ToUpper())
                    {
                        boolFound = true;
                        break;
                    }
                }
                else if (dr["starts"].ToString() == "1")
                {
                    if (_name.Trim().ToUpper().StartsWith(dr["name"].ToString().Trim().ToUpper()) == true)
                    {
                        boolFound = true;
                        break;
                    }
                }
                else if (dr["ends"].ToString() == "1")
                {
                    if (_name.Trim().ToUpper().EndsWith(dr["name"].ToString().Trim().ToUpper()) == true)
                    {
                        boolFound = true;
                        break;
                    }
                }
                else
                {
                    if (_name.Trim().ToUpper().Contains(dr["name"].ToString().Trim().ToUpper()) == true)
                    {
                        boolFound = true;
                        break;
                    }
                }
            }
            return boolFound;
        }
        public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
			return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAccountRequest", arParams);
		}
        public DataSet Gets(int _requestid, int _itemid, int _number, int _approval)
		{
			arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@approval", _approval);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAccountRequests", arParams);
		}
        public void Add(int _requestid, int _itemid, int _number, string _xid, int _domain, string _adgroups, string _localgroups, int _email, int _approval)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@xid", _xid);
            arParams[4] = new SqlParameter("@domain", _domain);
            arParams[5] = new SqlParameter("@adgroups", _adgroups);
            arParams[6] = new SqlParameter("@localgroups", _localgroups);
            arParams[7] = new SqlParameter("@email", _email);
            arParams[8] = new SqlParameter("@approval", _approval);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAccountRequest", arParams);
        }
        public void Complete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccountRequestCompleted", arParams);
        }
        public void Delete(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAccountRequest", arParams);
		}

        public void Process(int _requestid, int _itemid, int _number, string _server, string _password, int _environment, int _approval)
        {
            DataSet ds = Gets(_requestid, _itemid, _number, _approval);
            foreach (DataRow dr in ds.Tables[0].Rows)
                Process(_requestid, _itemid, _number, Int32.Parse(dr["id"].ToString()), _server, _password, _environment);
        }
        public void Deny(int _requestid, int _itemid, int _number, int _environment)
        {
            Requests oRequest = new Requests(user, dsn);
            Variables oVariable = new Variables(_environment);
            Users oUser = new Users(user, dsn);
            DataSet ds = Gets(_requestid, _itemid, _number, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int _id = Int32.Parse(dr["id"].ToString());
                DateTime _now = DateTime.Now;
                string strUnique = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                string strXID = ds.Tables[0].Rows[0]["xid"].ToString().ToUpper();
                int intUser = oUser.GetId(strXID);
                int intDomain = Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString());
                Variables oVar = new Variables(intDomain);
                string strFName = oUser.Get(intUser, "fname");
                string strLName = oUser.Get(intUser, "lname");
                //string strNotify = "<p><a href=\"javascript:void(0);\" class=\"bold\" onclick=\"ShowAccountDetail('divAccount_" + strUnique + "');\">Account " + strXID.ToUpper() + " in " + oVar.Name() + "</a><br/>";
                //strNotify += "<div id=\"divAccount_" + strUnique + "\" style=\"display:none\"><table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">";
                //strNotify += "<tr><td>The account " + strXID + " was DENIED for " + strFName + " " + strLName + " in " + oVar.Name() + "</td></tr>";
                //strNotify += "</table></div></p>";
                StringBuilder sbNotify = new StringBuilder();
                sbNotify.Append("<p><table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" style=\"");
                sbNotify.Append(oVariable.DefaultFontStyle());
                sbNotify.Append("\">");
                sbNotify.Append("<tr><td><b><u>Account &quot;");
                sbNotify.Append(strXID.ToUpper());
                sbNotify.Append("&quot; in ");
                sbNotify.Append(oVar.Name());
                sbNotify.Append("</u></b></td></tr>");
                sbNotify.Append("<tr><td>The account ");
                sbNotify.Append(strXID);
                sbNotify.Append(" was DENIED for ");
                sbNotify.Append(strFName);
                sbNotify.Append(" ");
                sbNotify.Append(strLName);
                sbNotify.Append(" in ");
                sbNotify.Append(oVar.Name());
                sbNotify.Append("</td></tr>");
                sbNotify.Append("</table></p>");
                Complete(_id);
                oRequest.AddResult(_requestid, _itemid, _number, sbNotify.ToString());
            }
        }
        public void Process(int _requestid, int _itemid, int _number, int _id, string _server, string _password, int _environment)
        {
            Requests oRequest = new Requests(user, dsn);
            Variables oVariable = new Variables(_environment);
            Users oUser = new Users(user, dsn);
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DateTime _now = DateTime.Now;
                string strUnique = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                string strXID = ds.Tables[0].Rows[0]["xid"].ToString().ToUpper();
                int intUser = oUser.GetId(strXID);
                int intDomain = Int32.Parse(ds.Tables[0].Rows[0]["domain"].ToString());
                Domains oDomain = new Domains(user, dsn);
                if (oDomain.Get(intDomain, "account_setup") == "1" || oDomain.Get(intDomain, "account_maintenance") == "1")
                {
                    intDomain = Int32.Parse(oDomain.Get(intDomain, "environment"));
                    Variables oVar = new Variables(intDomain);
                    AD oAD = new AD(user, dsn, intDomain);
                    string strFName = oUser.Get(intUser, "fname");
                    string strLName = oUser.Get(intUser, "lname");
                    string strResult = "";
                    string strID = strXID;
                    string strAction = "";
                    //string strNotify = "<p><a href=\"javascript:void(0);\" class=\"bold\" onclick=\"ShowAccountDetail('divAccount_" + strUnique + "');\">Account " + strXID.ToUpper() + " in " + oVar.Name() + "</a><br/>";
                    //strNotify += "<div id=\"divAccount_" + strUnique + "\" style=\"display:none\"><table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">";
                    string strNotify = "<p><table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">";
                    strNotify += "<tr><td><b><u>Account &quot;" + strXID.ToUpper() + "&quot; in " + oVar.Name() + "</u></b></td></tr>";
                    // Add User
                    if (intDomain != (int)CurrentEnvironment.CORPDMN && intDomain != (int)CurrentEnvironment.PNCNT_PROD)
                    {
                        strID = "E" + strID.Substring(1);
                        if (oAD.Search(strID, false) != null)
                            strAction = " already existed";
                        else
                        {
                            strID = "T" + strID.Substring(1);
                            if (oAD.Search(strID, false) != null)
                                strAction = " already existed";
                            else if (oDomain.Get(intDomain, "account_maintenance") == "1")
                            {
                                strResult = oAD.CreateUser(strID, strFName, strLName, _password, "", "Created by ClearView - " + DateTime.Now.ToShortDateString(), "");
                                strAction = " was created";
                            }
                            else
                                strResult = "Cannot create accounts in this domain";
                        }
                    }
                    else
                    {
                        if (oAD.Search(strID, false) != null)
                            strAction = " already existed";
                        else if (oDomain.Get(intDomain, "account_maintenance") == "1")
                        {
                            strResult = oAD.CreateUser(strID, strFName, strLName, _password, "", "Created by ClearView - " + DateTime.Now.ToShortDateString(), "");
                            strAction = " was created";
                        }
                        else
                            strResult = "Cannot create accounts in this domain";
                    }
                    if (strResult == "")
                    {
                        strNotify += "<tr><td>The account " + strID + strAction + " for " + strFName + " " + strLName + " in " + oVar.Name() + "</td></tr>";
                        // Local Groups
                        string strLocal = ds.Tables[0].Rows[0]["localgroups"].ToString();
                        if (strLocal != "")
                        {
                            string[] strGroups;
                            char[] strSplit = { ';' };
                            strGroups = strLocal.Split(strSplit);
                            DirectoryEntry oAccount = new DirectoryEntry("WinNT://" + oVar.Domain() + "/" + strID + ",user", oVar.Domain() + "\\" + oVar.ADUser(), oVar.ADPassword());
                            DirectoryEntry oServer = new DirectoryEntry("WinNT://" + oVar.Domain() + "/" + _server + ",computer", oVar.Domain() + "\\" + oVar.ADUser(), oVar.ADPassword());
                            for (int ii = 0; ii < strGroups.Length; ii++)
                            {
                                if (strGroups[ii].Trim() != "")
                                {
                                    try
                                    {
                                        DirectoryEntry oGroup = oServer.Children.Find(strGroups[ii]);
                                        oGroup.Invoke("Add", new object[] { oAccount.Path });
                                        strNotify += "<tr><td>The account " + strID + " was successfully added to the local group " + strGroups[ii] + "</td></tr>";
                                    }
                                    catch
                                    {
                                        strNotify += "<tr><td>There was a problem adding the account " + strID + " to the local group " + strGroups[ii] + "</td></tr>";
                                    }
                                }
                            }
                        }
                        // Global Groups
                        string strGlobal = ds.Tables[0].Rows[0]["adgroups"].ToString();
                        if (strGlobal != "")
                        {
                            string[] strGroups;
                            char[] strSplit = { ';' };
                            strGroups = strGlobal.Split(strSplit);
                            for (int ii = 0; ii < strGroups.Length; ii++)
                            {
                                if (strGroups[ii].Trim() != "")
                                {
                                    if (oAD.Search(strGroups[ii], false) == null)
                                    {
                                        strResult = oAD.CreateGroup(strGroups[ii], "", "Created by ClearView - " + DateTime.Now.ToShortDateString(), "", "GG", "S");
                                        if (strResult == "")
                                            strNotify += "<tr><td>The group " + strGroups[ii] + " was successfully created in " + oVar.Name() + "</td></tr>";
                                        else
                                            strNotify += "<tr><td>There was a problem creating the group " + strGroups[ii] + " in " + oVar.Name() + "</td></tr>";
                                    }
                                    else
                                    {
                                        strResult = "";
                                        strNotify += "<tr><td>The group " + strGroups[ii] + " already exists in " + oVar.Name() + "</td></tr>";
                                    }
                                    if (strResult == "")
                                    {
                                        strResult = oAD.JoinGroup(strID, strGroups[ii], 0);
                                        if (strResult == "")
                                            strNotify += "<tr><td>The account " + strID + " was successfully added to the domain group " + strGroups[ii] + "</td></tr>";
                                        else
                                            strNotify += "<tr><td>There was a problem adding the account " + strID + " to the domain group " + strGroups[ii] + "</td></tr>";
                                    }
                                }
                            }
                        }
                    }
                    else
                        strNotify += "<tr><td>There was a problem creating an account for " + strFName + " " + strLName + " in " + oVar.Name() + " - ERROR: " + strResult + "</td></tr>";
                    strNotify += "</table></p>";
                    //strNotify += "</table></div></p>";
                    Complete(_id);
                    oRequest.AddResult(_requestid, _itemid, _number, strNotify);
                }
            }
        }


        public DataSet GetMaintenance(int _requestid, string _maintenance)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@maintenance", _maintenance);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAccountMaintenances", arParams);
        }
        public DataSet GetMaintenance(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAccountMaintenance", arParams);
        }
        public void AddMaintenance(int _requestid, int _itemid, int _number, string _maintenance, string _username, int _domain)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@maintenance", _maintenance);
            arParams[4] = new SqlParameter("@username", _username);
            arParams[5] = new SqlParameter("@domain", _domain);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAccountMaintenance", arParams);
        }
        public void UpdateMaintenance(int _requestid, int _itemid, int _number, int _approval, string _reason)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@approval", _approval);
            arParams[4] = new SqlParameter("@reason", _reason);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccountMaintenance", arParams);
        }
        public DataSet GetMaintenanceParameters(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAccountMaintenanceParameters", arParams);
        }
        public void AddMaintenanceParameter(int _requestid, int _itemid, int _number, string _value)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@value", _value);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAccountMaintenanceParameter", arParams);
        }
        public void DeleteMaintenanceParameters(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAccountMaintenanceParameters", arParams);
        }
    }
}
