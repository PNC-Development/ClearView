using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using NCC.ClearView.Application.Core;
using System.DirectoryServices;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace ClearViewService
{
    public class AutoTasks
    {
        private string dsn = "";
        private string dsnAsset = "";
        private string dsnIP = "";
        private string dsnServiceEditor = "";
        private int intAutoAccount;
        private int intOrganization;
        private int intRemediationItem;
        private int intEnvironment;
        private int intAssignPage;
        private int intViewPage;
        private int intCounter;
        private EventLog oLog;
        public AutoTasks(string _dsn, string _dsnAsset, string _dsnIP, string _dsnServiceEditor, int _auto_account, int _organization, int _remediation_itemid, int _environment, int _assign_page, int _view_page, EventLog _log)
		{
            dsn = _dsn;
            dsnAsset = _dsnAsset;
            dsnIP = _dsnIP;
            dsnServiceEditor = _dsnServiceEditor;
            intAutoAccount = _auto_account;
            intOrganization = _organization;
            intRemediationItem = _remediation_itemid;
            intEnvironment = _environment;
            //strBCC = _bcc;
            intAssignPage = _assign_page;
            intViewPage = _view_page;
            intCounter = 0;
            oLog = _log;
        }
        public void AD_Old_Accounts()
        {
            Requests oRequest = new Requests(0, dsn);
            Projects oProject = new Projects(0, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
            ADObject oADObject = new ADObject(0, dsn);
            int intRequest = 28815;
            if (oServiceRequest.Get(intRequest).Tables[0].Rows.Count == 0)
                oServiceRequest.Add(intRequest, 1, 1);
            double dblHours = 0.00;
            if (intEnvironment == 4)
            {
                dblHours += LastLogin(2, 365, 0, intRequest);
                dblHours += LastLogin(3, 365, 0, intRequest);
            }
            else
                dblHours += LastLogin(2, 365, 0, intRequest);
            if (dblHours > 0)
            {
                double dblTime = 5.00 / 60.00;
                double dblDevices = dblHours / dblTime;
                dblDevices = Math.Round(dblDevices);
                int intDevices = Int32.Parse(dblDevices.ToString());
                int intResource = oServiceRequest.AddRequest(intRequest, intRemediationItem, 0, intDevices, dblHours, 2, 1, dsnServiceEditor);
                oServiceRequest.NotifyTeamLead(intRemediationItem, intResource, intAssignPage, intViewPage, intEnvironment,  "", dsnServiceEditor, dsnAsset, dsnIP, 0);
            }
        }
        public void AD_Mismatched_Accounts()
        {
            Requests oRequest = new Requests(0, dsn);
            Projects oProject = new Projects(0, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
            int intRequest = 28815;
            if (oServiceRequest.Get(intRequest).Tables[0].Rows.Count == 0)
                oServiceRequest.Add(intRequest, 1, 1);
            double dblHours = 0.00;
            if (intEnvironment == 4)
            {
                dblHours += CompareDomainAccounts(2, 4, "X", 0, intRequest);
                dblHours += CompareDomainAccounts(3, 4, "X", 0, intRequest);
            }
            else
                dblHours += CompareDomainAccounts(2, 3, "T", 0, intRequest);
            double dblTime = 5.00 / 60.00;
            double dblDevices = dblHours / dblTime;
            dblDevices = Math.Round(dblDevices);
            int intDevices = Int32.Parse(dblDevices.ToString());
            int intResource = oServiceRequest.AddRequest(intRequest, intRemediationItem, 0, intDevices, dblHours, 2, 1, dsnServiceEditor);
            oServiceRequest.NotifyTeamLead(intRemediationItem, intResource, intAssignPage, intViewPage, intEnvironment,  "", dsnServiceEditor, dsnAsset, dsnIP, 0);
        }
        public void AD_Old_Computers()
        {
        }
        public void AD_Mismatched_Computers()
        {
            Requests oRequest = new Requests(0, dsn);
            Projects oProject = new Projects(0, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
            int intRequest = 28815;
            if (oServiceRequest.Get(intRequest).Tables[0].Rows.Count == 0)
                oServiceRequest.Add(intRequest, 1, 1);
            double dblHours = 0.00;
            if (intEnvironment == 4)
            {
                dblHours += CompareDomainComputers(2, 3, 0, intRequest);
            }
            else
                dblHours += CompareDomainComputers(2, 3, 0, intRequest);
            double dblTime = 5.00 / 60.00;
            double dblDevices = dblHours / dblTime;
            dblDevices = Math.Round(dblDevices);
            int intDevices = Int32.Parse(dblDevices.ToString());
            int intResource = oServiceRequest.AddRequest(intRequest, intRemediationItem, 0, intDevices, dblHours, 2, 1, dsnServiceEditor);
            oServiceRequest.NotifyTeamLead(intRemediationItem, intResource, intAssignPage, intViewPage, intEnvironment,  "", dsnServiceEditor, dsnAsset, dsnIP, 0);
        }


        private double CompareDomainComputers(int _search_domain, int _searched_domain, int _computers, int _requestid)
        {
            double dblHours = 0.00;
            double dblTime = 5.00 / 60.00;
            try
            {
                ADObject oADObject = new ADObject(0, dsn);
                DataPoint oDataPoint = new DataPoint(0, dsn);
                Variables oDevV = new Variables(_search_domain);
                DirectoryEntry oDevE = new DirectoryEntry(oDevV.primaryDC(dsn), oDevV.Domain() + "\\" + oDevV.ADUser(), oDevV.ADPassword());
                DirectorySearcher oDevS = new DirectorySearcher(oDevE);
                oDevS.Filter = "(objectCategory=computer)";                   
                SearchResultCollection oDevs = oDevS.FindAll();
                string[] a1 = new string[10000];
                int intCount = 0;
                foreach (SearchResult oDev in oDevs)
                {
                    if (oDev.Properties.Contains("sAMAccountName") == true)
                    {
                        a1[intCount] = oDev.Properties["sAMAccountName"][0].ToString();
                        intCount++;
                    }
                }
                Variables oTestV = new Variables(_searched_domain);
                DirectoryEntry oTestE = new DirectoryEntry(oTestV.primaryDC(dsn), oTestV.Domain() + "\\" + oTestV.ADUser(), oTestV.ADPassword());
                DirectorySearcher oTestS = new DirectorySearcher(oTestE);
                oTestS.Filter = "(objectCategory=computer)";
                SearchResultCollection oTests = oTestS.FindAll();
                string[] a2 = new string[10000];
                intCount = 0;
                foreach (SearchResult oTest in oTests)
                {
                    if (oTest.Properties.Contains("sAMAccountName") == true)
                    {
                        a2[intCount] = oTest.Properties["sAMAccountName"][0].ToString();
                        intCount++;
                    }
                }
                string[] a3 = new string[10000];
                intCount = 0;
                for (int ii = 0; ii < 10000 && a1[ii] != null; ii++)
                {
                    for (int jj = 0; jj < 10000 && a2[jj] != null; jj++)
                    {
                        if (a1[ii] == a2[jj])
                        {
                            a3[intCount] = a2[jj];
                            intCount++;
                            break;
                        }
                    }
                }
                Ping oPing = new Ping();
                for (int kk = 0; kk < 10000 && a3[kk] != null; kk++)
                {
                    if (_computers > 0 && intCounter > _computers)
                        break;
                    string strServer = a3[kk].Replace("$", "");
                    string strStatus = "";
                    PingReply oReply;
                    try
                    {
                        string strDevSuffix = oDevV.FullyQualified();
                        oReply = oPing.Send(strServer + "." + strDevSuffix);
                        strStatus = oReply.Status.ToString();
                    }
                    catch {}
                    if (strStatus == "Success")
                        AddDomainComputer(_requestid, strServer, _searched_domain);
                    else 
                    {
                        try
                        {
                            string strTestSuffix = oTestV.FullyQualified();
                            oReply = oPing.Send(strServer + "." + strTestSuffix);
                            strStatus = oReply.Status.ToString();
                        }
                        catch {}
                        if (strStatus == "Success")
                            AddDomainComputer(_requestid, strServer, _search_domain);
                        else
                        {
                            try
                            {
                                oReply = oPing.Send(strServer);
                                strStatus = oReply.Status.ToString();
                            }
                            catch { }
                            if (strStatus == "Success")
                            {
                                int intDomain = oDevV.DomainID(oDataPoint.GetDomainNameFix(strServer, intEnvironment));
                                if (intDomain > 0)
                                {
                                    if (intDomain == _search_domain)
                                        intDomain = _searched_domain;
                                    else
                                        intDomain = _search_domain;
                                    AddDomainComputer(_requestid, strServer, intDomain);
                                }
                                else
                                {
                                    AddDomainComputer(_requestid, strServer, _search_domain);
                                    AddDomainComputer(_requestid, strServer, _searched_domain);
                                    dblHours += dblTime;
                                }
                            }
                            else
                            {
                                AddDomainComputer(_requestid, strServer, _search_domain);
                                AddDomainComputer(_requestid, strServer, _searched_domain);
                                dblHours += dblTime;
                            }
                        }
                    }
                    dblHours += dblTime;
                    intCounter++;
                }
            }
            catch (Exception er2)
            {
                oLog.WriteEntry(String.Format("ERROR: " + er2.Message), EventLogEntryType.Error);
            }
            return dblHours;
        }
        private void AddDomainComputer(int _requestid, string _server, int _domain)
        {
            Variables oVariable = new Variables(_domain);
            ADObject oADObject = new ADObject(0, dsn);
            AD oAD = new AD(0, dsn, _domain);
            SearchResultCollection oResults = oAD.ComputerSearch(_server);
            foreach (SearchResult oResult in oResults)
            {
                string strPath = "";
                if (oResult.Properties.Contains("adspath") == true)
                    strPath = oResult.Properties["adspath"][0].ToString();
                string strDate = "";
                if (oResult.Properties.Contains("whencreated") == true)
                    strDate = oResult.Properties["whencreated"][0].ToString();
                oADObject.AddDomainAccount(_requestid, _domain, _server, strPath, _server + " is not a member of " + oVariable.Domain().ToUpper(), strDate);
            }
        }
        private double CompareDomainAccounts(int _search_domain, int _searched_domain, string _account_preface, int _accounts, int _requestid)
        {
            double dblHours = 0.00;
            double dblTime = 5.00 / 60.00;
            Variables vDev = new Variables(_search_domain);
            Variables vProd = new Variables(_searched_domain);
            ADObject oADObject = new ADObject(0, dsn);
            DirectoryEntry oDev = new DirectoryEntry(vDev.primaryDC(dsn), vDev.ADUser(), vDev.ADPassword());
            DirectoryEntry oProd = new DirectoryEntry(vProd.primaryDC(dsn), vProd.ADUser(), vProd.ADPassword());
            DirectorySearcher sDev = new DirectorySearcher(oDev);
            sDev.Filter = "(&(objectCategory=user)(|(sAMAccountName=t*)(|(sAMAccountName=e*)(sAMAccountName=x*))))";
            SearchResultCollection resDevs = sDev.FindAll();
            foreach (SearchResult resDev in resDevs)
            {
                if (_accounts > 0 && intCounter > _accounts)
                    break;
                if (resDev.Properties.Contains("sAMAccountName") == true)
                {
                    string strXid = resDev.Properties["sAMAccountName"][0].ToString();
                    string strNewXid = _account_preface + strXid.Substring(1);
                    DirectorySearcher sProd = new DirectorySearcher(oProd);
                    sProd.Filter = "(&(objectCategory=user)(sAMAccountName=" + strNewXid + "))";
                    SearchResult resProd = sProd.FindOne();
                    if (resProd == null)
                    {
                        string strFName = "";
                        if (resDev.Properties.Contains("givenname") == true)
                            strFName = resDev.Properties["givenname"][0].ToString();
                        string strLName = "";
                        if (resDev.Properties.Contains("sn") == true)
                            strLName = resDev.Properties["sn"][0].ToString();
                        string strName = "";
                        if (strFName != "" || strLName != "")
                            strName = "(" + strFName + " " + strLName + ")";
                        string strPath = "";
                        if (resDev.Properties.Contains("adspath") == true)
                            strPath = resDev.Properties["adspath"][0].ToString();
                        string strDate = "";
                        if (resDev.Properties.Contains("whencreated") == true)
                            strDate = resDev.Properties["whencreated"][0].ToString();
                        oADObject.AddDomainAccount(_requestid, _search_domain, strNewXid.ToUpper() + strName, strPath, vDev.Name() + " account not found in " + vProd.Name() + "(" + strNewXid + ")", strDate);
                        intCounter++;
                        dblHours += dblTime;
                    }
                }
            }
            return dblHours;
        }
        private double LastLogin(int _search_domain, int _days, int _accounts, int _requestid)
        {
            double dblHours = 0.00;
            double dblTime = 5.00 / 60.00;
            ADObject oADObject = new ADObject(0, dsn);
            Variables oVariable = new Variables(_search_domain);
            DirectoryEntry oEntry = new DirectoryEntry(oVariable.primaryDC(dsn), oVariable.ADUser(), oVariable.ADPassword());
            DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
            oSearcher.Filter = "(&(objectCategory=user)(|(sAMAccountName=t*)(|(sAMAccountName=e*)(sAMAccountName=x*))))";
            SearchResultCollection oResults = oSearcher.FindAll();
            foreach (SearchResult oResult in oResults)
            {
                if (_accounts > 0 && intCounter > _accounts)
                    break;
                if (oResult.Properties.Contains("sAMAccountName") == true)
                {
                    string strXid = oResult.Properties["sAMAccountName"][0].ToString();
                    string strFName = "";
                    if (oResult.Properties.Contains("givenname") == true)
                        strFName = oResult.Properties["givenname"][0].ToString();
                    string strLName = "";
                    if (oResult.Properties.Contains("sn") == true)
                        strLName = oResult.Properties["sn"][0].ToString();
                    string strName = "";
                    if (strFName != "" || strLName != "")
                        strName = "(" + strFName + " " + strLName + ")";
                    string strPath = "";
                    if (oResult.Properties.Contains("adspath") == true)
                        strPath = oResult.Properties["adspath"][0].ToString();
                    DateTime dc1 = CheckDomain(1, _search_domain, strXid, _days);
                    DateTime dc2 = CheckDomain(2, _search_domain, strXid, _days);
                    DateTime dcGreater = new DateTime();
                    DateTime dcLess = new DateTime();
                    if (dc1 > dc2)
                    {
                        dcGreater = dc1;
                        dcLess = dc2;
                    }
                    else
                    {
                        dcGreater = dc2;
                        dcLess = dc1;
                    }
                    TimeSpan oSpan = DateTime.Today.Subtract(dcGreater);
                    TimeSpan oSpanLess = DateTime.Today.Subtract(dcLess);
                    if (oSpan.Days > _days)
                    {
                        string strDate = dcGreater.ToString();
                        oADObject.AddDomainAccount(_requestid, _search_domain, strXid.ToUpper() + strName, strPath, "Account has not logged in for " + oSpan.Days.ToString() + " days in " + oVariable.Name() + " (" + oSpanLess.Days.ToString() + " in other DC)", strDate);
                        intCounter++;
                        dblHours += dblTime;
                    }
                }
            }
            return dblHours;
        }
        private DateTime CheckDomain(int _domain, int _search_domain, string _xid, int _days)
        {
            DateTime _return = DateTime.Today;
            Variables oVariable = new Variables(_search_domain);
            DirectoryEntry oEntry = new DirectoryEntry();
            if (_domain == 1)
                oEntry = new DirectoryEntry(oVariable.primaryDC(dsn), oVariable.ADUser(), oVariable.ADPassword());
            if (_domain == 2)
                oEntry = new DirectoryEntry(oVariable.secondaryDC(), oVariable.ADUser(), oVariable.ADPassword());
            DirectorySearcher oSearcher = new DirectorySearcher(oEntry);
            oSearcher.Filter = "(&(objectCategory=user)(sAMAccountName=" + _xid + "))";
            SearchResult oResult = oSearcher.FindOne();
            if (oResult != null)
            {
                if (oResult.Properties.Contains("sAMAccountName") == true)
                {
                    ActiveDs.LargeInteger oLarge;
                    if (oResult.GetDirectoryEntry().Properties.Contains("lastlogon") == true)
                    {
                        oLarge = (ActiveDs.LargeInteger)oResult.GetDirectoryEntry().Properties["lastlogon"].Value;
                        long _date = (long)oLarge.HighPart << 32 | (uint)oLarge.LowPart;
                        DateTime _logon = DateTime.FromFileTime(_date);
                        TimeSpan oSpan = DateTime.Today.Subtract(_logon);
                        if (_date > 0.00 && oSpan.Days > _days)
                            _return = _logon;
                    }
                }
            }
            return _return;
        }

    }
}
