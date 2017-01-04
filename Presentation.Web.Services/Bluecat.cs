using NCC.ClearView.Application.Core;
using NCC.ClearView.Application.Core.Proteus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;

namespace Presentation.Web.Services
{
    public class Bluecat : BaseClass
    {
        public class BlueCatEntity
        {
            public int Count { get; set; }
            public APIEntity Entity { get; set; }
            public string Name { get; set; }
            public string Results { get; set; }
        }
        private string strStaging = "IP Address held for server staging via Ciora, Healy, Whelan";
        private string strBluecatLogin = "";
        private string strBluecatSuffix = ".pncbank.com";
        private bool boolLike = false;

        public Bluecat(string strWebMethodName)
            : base(strWebMethodName)
        {
        }
        public string Create(string ObjectAddress, string ObjectName, string Description, string MacAddress)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);

            if (boolCanWrite)
            {
                return CreateOrUpdateBluecat(ObjectAddress, ObjectName, Description, MacAddress, false, false);
            }
            else
            {
                oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished...Access Denied (" + strUser + ")"), EventLogEntryType.Information);
                return strERRORPrefix + "Access Denied (" + strUser + ")";
            }
        }
        public string Update(string ObjectAddress, string ObjectName, string Description, string MacAddress)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);

            if (boolCanWrite)
            {
                return CreateOrUpdateBluecat(ObjectAddress, ObjectName, Description, MacAddress, true, false);
            }
            else
            {
                oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished...Access Denied (" + strUser + ")"), EventLogEntryType.Information);
                return strERRORPrefix + "Access Denied (" + strUser + ")";
            }
        }
        public string UpdateDescription(string ObjectAddress, string Description)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            string strReturn = "";

            if (boolCanWrite)
            {
                ProteusAPI oProteusAPI = new ProteusAPI();
                oProteusAPI.CookieContainer = new CookieContainer();
                //oProteusAPI.Proxy = oVariable.GetProxy(dsn);
                oProteusAPI.Url = oVariable.BlueCatWebService(dsn);

                // Login
                oProteusAPI = LoginBluecat(oProteusAPI, oVariable);

                if (oProteusAPI == null)
                    return strERRORPrefix + "ERROR: " + "Unable to login (" + strBluecatLogin + ")";
                else
                {
                    BlueCatEntity oSearch = SearchBluecat(oProteusAPI, ObjectAddress, "", true);
                    if (oSearch.Count == 0)
                    {
                        strReturn = strERRORPrefix + "ERROR: " + "There were no records found (" + oSearch.Results + ")";
                        LogIt(ObjectAddress, strReturn, LoggingType.Error);
                    }
                    else if (oSearch.Count > 1)
                    {
                        strReturn = strERRORPrefix + "ERROR: " + "There were multiple records found (" + oSearch.Results + ")";
                        LogIt(ObjectAddress, strReturn, LoggingType.Error);
                    }
                    else
                    {
                        try
                        {
                            LogIt(ObjectAddress, "Attempting to update the BlueCat description = " + Description, LoggingType.Information);
                            oSearch.Entity.properties = "NWS=clearview|requestor=clearview|modified-by=xacview|modified=" + DateTime.Now.ToString() + "|name=" + Description + "|";
                            oProteusAPI.update(oSearch.Entity);
                            strReturn = "SUCCESS";
                            LogIt(ObjectAddress, "Description updated SUCCESSFULLY!!", LoggingType.Information);
                        }
                        catch (Exception exBluecat)
                        {
                            strReturn = strERRORPrefix + "ERROR: (Bluecat # 3) " + exBluecat.Message;
                            LogIt(ObjectAddress, strReturn, LoggingType.Error);
                        }
                    }

                    // Log out
                    oProteusAPI.logout();
                }
            }
            else
            {
                oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished...Access Denied (" + strUser + ")"), EventLogEntryType.Information);
                return strERRORPrefix + "Access Denied (" + strUser + ")";
            }
            return strReturn;
        }

        private string CreateOrUpdateBluecat(string ObjectAddress, string ObjectName, string Description, string MacAddress, bool boolUpdate, bool boolFromUpdate)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            bool boolSuccess = false;
            string strReturn = "";

            // Setup ObjectName and Description
            ObjectName = ObjectName.Trim();
            Description = Description.Trim();

            // Remove DNS suffix from ObjectName
            ObjectName = ObjectName.ToLower();
            if (ObjectName.Contains(strBluecatSuffix) == true)
                ObjectName = ObjectName.Substring(0, ObjectName.IndexOf(strBluecatSuffix));
            ObjectName = ObjectName.ToUpper();

            // Remove ObjectName from Description
            if (Description.Trim() == "")
                Description = ObjectName;
            else if (Description.ToUpper().Contains(ObjectName) == false)
                Description = ObjectName + " - " + Description;

            Description = Description.ToLower();

            // Log
            LogIt(ObjectName, "Beginning " + (boolUpdate ? "UPDATE" : "CREATE") + " for " + ObjectAddress + " (" + Description + ")", LoggingType.Information);

            Variables oVariable = new Variables(intEnvironment);
            ProteusAPI oProteusAPI = new ProteusAPI();
            oProteusAPI.CookieContainer = new CookieContainer();
            //oProteusAPI.Proxy = oVariable.GetProxy(dsn);
            oProteusAPI.Url = oVariable.BlueCatWebService(dsn);

            // Login
            oProteusAPI = LoginBluecat(oProteusAPI, oVariable);

            if (oProteusAPI == null)
                return strERRORPrefix + "ERROR: " + "Unable to login (" + strBluecatLogin + ")";
            else
            {
                BlueCatEntity oSearchIP = SearchBluecat(oProteusAPI, ObjectAddress, "", false);
                BlueCatEntity oSearchIPStaging = SearchBluecat(oProteusAPI, ObjectAddress, "", true);
                BlueCatEntity oSearchName = SearchBluecat(oProteusAPI, "", ObjectName, false);
                if (oSearchIP.Count > 1)
                {
                    LogIt(ObjectName, "There were multiple records found (IP Search = " + oSearchIP.Count.ToString() + " ~ " + oSearchIP.Results + ")", LoggingType.Error);
                    return strERRORPrefix + "ERROR: " + "There were multiple records found (IP Search = " + oSearchIP.Count.ToString() + " ~ " + oSearchIP.Results + ")";
                }
                else if (oSearchName.Count > 1)
                {
                    LogIt(ObjectName, "There were multiple records found (Name Search = " + oSearchName.Count.ToString() + " ~ " + oSearchName.Results + ")", LoggingType.Error);
                    return strERRORPrefix + "ERROR: " + "There were multiple records found (Name Search = " + oSearchName.Count.ToString() + " ~ " + oSearchName.Results + ")";
                }
                else if (oSearchIP.Count == 0 && oSearchName.Count == 0)
                {
                    // Neither the address nor the name nor the alias(es) exist...ok to create
                    LogIt(ObjectName, "Neither the address nor the name nor the alias(es) exist...ok to create", LoggingType.Information);

                    // Get Required Parent Objects
                    APIEntity oConfiguration = oProteusAPI.getEntityByName(0, oVariable.BlueCatConfiguration(), "Configuration");
                    APIEntity oView = oProteusAPI.getEntityByName(oConfiguration.id, oVariable.BlueCatView(), "View");
                    Functions oFunction = new Functions(0, dsn, intEnvironment);
                    string strEMailIdsTO = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_BLUECAT_DNS_TO");
                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_BLUECAT_DNS_BCC");

                    if (oSearchIPStaging.Count == 1)
                    {
                        bool boolStageUpdated = false;
                        for (int ii = 0; ii < 5 && boolSuccess == false; ii++)
                        {
                            if (strReturn != "")
                            {
                                if (strReturn.ToUpper().Contains("TRY AGAIN") == true)
                                {
                                    LogIt(ObjectName, "Waiting 3 seconds to try again...", LoggingType.Debug);
                                    Thread.Sleep(3000); // Wait 3 seconds
                                    strReturn = "";
                                }
                                else
                                {
                                    // A different error has happened.  Exit the loop (since it's more severe)
                                    break;
                                }
                            }
                            try
                            {
                                if (boolStageUpdated == false)
                                {
                                    LogIt(ObjectName, "Updating staged record...", LoggingType.Debug);
                                    string properties = "NWS=clearview|requestor=clearview|modified-by=xacview|modified=" + DateTime.Now.ToString() + "|name=" + Description + "|";
                                    oSearchIPStaging.Entity.name = Description;
                                    oSearchIPStaging.Entity.properties = properties;
                                    oProteusAPI.update(oSearchIPStaging.Entity);
                                    LogIt(ObjectName, "Staged record updated.", LoggingType.Information);
                                    boolStageUpdated = true;
                                    LogIt(ObjectName, "Waiting 2 seconds to add the Host Record...", LoggingType.Debug);
                                    Thread.Sleep(2000); // Wait 2 seconds
                                }
                                LogIt(ObjectName, "Adding Host Record...", LoggingType.Debug);
                                string strLower = ObjectName + strBluecatSuffix;
                                oProteusAPI.addHostRecord(oView.id, strLower.ToLower(), ObjectAddress, -1, ""); // no properties for HostRecord (only IP Record)
                                LogIt(ObjectName, "Host record added.", LoggingType.Information);
                                // Email IP Address Mailbox
                                oFunction.SendEmail("BlueCat IP Notification", strEMailIdsTO, "", strEMailIdsBCC, "BlueCat IP Notification", "The following IP Address was " + (boolFromUpdate ? "UPDATED" : "CREATED") + " in BlueCat..." + Environment.NewLine + Environment.NewLine + ObjectName + strBluecatSuffix + " = " + ObjectAddress + " (" + Description + ")", false, true);
                                LogIt(ObjectName, "BlueCat IP Notification Sent", LoggingType.Information);
                                LogIt(ObjectName, "Record created SUCCESSFULLY!!", LoggingType.Information);
                                strReturn = "SUCCESS";
                                boolSuccess = true;
                                /*
                                // Delete the IP address (to get rid of the "IP Address held for server staging via Ciora, Healy, Whelan..." mesage)
                                LogIt(ObjectName, "Delete the IP address to get rid of STAGING placeholder", LoggingType.Information);
                                LogIt(ObjectName, DeleteBluecat(ObjectAddress, "", true, true, false), LoggingType.Debug);
                                */
                            }
                            catch (Exception exBluecat)
                            {
                                strReturn = strERRORPrefix + "ERROR: (Bluecat # 2) " + exBluecat.Message;
                                LogIt(ObjectName, strReturn, LoggingType.Error);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            LogIt(ObjectName, "Creating record...", LoggingType.Information);
                            // hostname,viewId,reverseFlag(true|false),sameAsZoneFlag(true|false)
                            string strLower = ObjectName + strBluecatSuffix;
                            string hostInfo = strLower.ToLower() + "," + oView.id + ",true,false";
                            string properties = "NWS=clearview|requestor=clearview|modified-by=xacview|modified=" + DateTime.Now.ToString() + "|name=" + Description + "|";
                            oProteusAPI.assignIP4Address(oConfiguration.id, ObjectAddress, MacAddress, hostInfo, "MAKE_STATIC", properties);
                            // Email IP Address Mailbox
                            oFunction.SendEmail("BlueCat IP Notification", strEMailIdsTO, "", strEMailIdsBCC, "BlueCat IP Notification", "The following IP Address was " + (boolFromUpdate ? "UPDATED" : "CREATED") + " in BlueCat..." + Environment.NewLine + Environment.NewLine + ObjectName + strBluecatSuffix + " = " + ObjectAddress + " (" + Description + ")", false, true);
                            LogIt(ObjectName, "BlueCat IP Notification Sent", LoggingType.Information);
                            LogIt(ObjectName, "Record created SUCCESSFULLY!!", LoggingType.Information);
                            strReturn = "SUCCESS";
                            boolSuccess = true;
                        }
                        catch (Exception exBluecat)
                        {
                            strReturn = strERRORPrefix + "ERROR: (Bluecat # 1) " + exBluecat.Message;
                            LogIt(ObjectName, strReturn, LoggingType.Error);
                        }
                    }
                }
                else
                {
                    if (boolUpdate == true)
                    {
                        if (oSearchName.Count == 0 || oSearchIP.Count == 0)
                        {
                            // NAME has been changed (and does not exist)
                            // Update the NAME using IP ADDRESS
                            // --- OR ---
                            // IP ADDRESS has been changed (and does not exist)
                            // Update the IP ADDRESS using NAME

                            if (oSearchName.Count == 1 && oSearchIP.Count == 0)
                            {
                                //strReturn = "BLAH1";
                                LogIt(ObjectName, "The NAME exists and IP address does not, so delete the NAME record", LoggingType.Information);
                                // Delete Name Record
                                LogIt(ObjectName, Delete("", ObjectName, false, true, false), LoggingType.Debug);
                                LogIt(ObjectName, "NAME record deleted, now create the record", LoggingType.Information);
                                return CreateOrUpdateBluecat(ObjectAddress, ObjectName, Description, MacAddress, false, true);
                            }
                            else if (oSearchName.Count == 0 && oSearchIP.Count == 1)
                            {
                                //strReturn = "BLAH2";
                                LogIt(ObjectName, "The IP address exists and NAME does not, so delete the IP address record", LoggingType.Information);
                                // Delete Address Record
                                LogIt(ObjectName, Delete(ObjectAddress, "", false, true, false), LoggingType.Debug);
                                LogIt(ObjectName, "IP address record deleted, now create the record", LoggingType.Information);
                                return CreateOrUpdateBluecat(ObjectAddress, ObjectName, Description, MacAddress, false, true);
                            }
                        }
                        // If it gets to this point, then it must not have returned a CreateOrUpdateBluecat result.
                        strReturn = strERRORPrefix + "ERROR: " + "Cannot update when there are multiple records found for both the IP and Name";
                        LogIt(ObjectName, strReturn, LoggingType.Error);
                    }
                    else
                    {
                        LogIt(ObjectName, "Either the name or IP Address exist (or both)...checking for CONFLICT / DUPLICATE", LoggingType.Information);
                        string strName = "";
                        if (oSearchIP.Count > 0 && oSearchIP.Name != "")
                            strName = oSearchIP.Name.Trim();
                        string strAddress = "";
                        if (oSearchName.Count > 0 && oSearchName.Entity != null)
                            strAddress = GetBluecatProperty(oSearchName.Entity, "address").Trim();

                        // There is an unknown delay in getting the Address...attempting to fix...
                        if (strName.ToUpper() != ObjectName.Trim().ToUpper() && strAddress.ToUpper() == ObjectAddress.Trim().ToUpper())
                        {
                            LogIt(ObjectName, "Timing issue...trying to get the latest IP Address information again...", LoggingType.Information);
                            // Check one more time for IP Address...
                            oSearchIP = SearchBluecat(oProteusAPI, ObjectAddress, "", false);
                            if (oSearchIP.Count == 1 && oSearchIP.Name != "")
                                strName = oSearchIP.Name.Trim();
                        }

                        if (strName.ToUpper() == ObjectName.Trim().ToUpper() && strAddress.ToUpper() == ObjectAddress.Trim().ToUpper())
                        {
                            LogIt(ObjectName, "Duplicate information found...check for proper HostRecord", LoggingType.Information);
                            APIEntity[] oHostRecords = oProteusAPI.getLinkedEntities(oSearchIP.Entity.id, "HostRecord", 0, 100);
                            if (oHostRecords == null)
                                oHostRecords = oProteusAPI.getLinkedEntities(oSearchName.Entity.id, "HostRecord", 0, 100);

                            if (oHostRecords == null || oHostRecords.Length == 0)
                            {
                                LogIt(ObjectName, "HostRecord not found, create it", LoggingType.Information);
                                // Create the HostRecord
                                APIEntity oConfiguration2 = oProteusAPI.getEntityByName(0, oVariable.BlueCatConfiguration(), "Configuration");
                                APIEntity oView2 = oProteusAPI.getEntityByName(oConfiguration2.id, oVariable.BlueCatView(), "View");
                                string strLower = ObjectName + strBluecatSuffix;
                                oProteusAPI.addHostRecord(oView2.id, strLower.ToLower(), strAddress, -1, "");
                                strReturn = "SUCCESS";
                                boolSuccess = true;
                                LogIt(ObjectName, "HostRecord created SUCCESSFULLY!!", LoggingType.Information);
                            }
                            else if (oHostRecords.Length == 1)
                            {
                                strReturn = strERRORPrefix + "DUPLICATE";
                                boolSuccess = true;
                                LogIt(ObjectName, "Duplicate Record.", LoggingType.Information);
                            }
                            else
                            {
                                strReturn = strERRORPrefix + "HostRecords (" + oHostRecords.Length.ToString() + "): Create (" + ObjectAddress + "=" + strName + "," + ObjectName + "=" + strAddress + ")";
                                LogIt(ObjectName, strReturn, LoggingType.Error);
                            }
                        }
                        else
                        {
                            strReturn = strERRORPrefix + "CONFLICT: Create (" + ObjectAddress + "=" + strName + "," + ObjectName + "=" + strAddress + ")";
                            LogIt(ObjectName, strReturn, LoggingType.Error);
                        }
                    }
                }

                // Log out
                oProteusAPI.logout();
            }

            if (boolSuccess == false)
            {
            }
            return strReturn;
        }

        public string Delete(string ObjectAddress, string ObjectName, bool IncludeStaging, bool HideNotification, bool Decommission)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            string strReturn = "";

            if (boolCanWrite)
            {
                Variables oVariable = new Variables(intEnvironment);
                ProteusAPI oProteusAPI = new ProteusAPI();
                oProteusAPI.CookieContainer = new CookieContainer();
                //oProteusAPI.Proxy = oVariable.GetProxy(dsn);
                oProteusAPI.Url = oVariable.BlueCatWebService(dsn);

                // Login
                oProteusAPI = LoginBluecat(oProteusAPI, oVariable);

                if (oProteusAPI == null)
                    return strERRORPrefix + "ERROR: " + "Unable to login (" + strBluecatLogin + ")";
                else
                {
                    BlueCatEntity oSearchIP = SearchBluecat(oProteusAPI, ObjectAddress, "", IncludeStaging);
                    BlueCatEntity oSearchName = SearchBluecat(oProteusAPI, "", ObjectName, false);
                    if (oSearchIP.Count == 0 && oSearchName.Count == 0)
                    {
                        strReturn = "SUCCESS";
                        LogIt(ObjectName, "Neither the IP Address or NAME exist...return SUCCESS", LoggingType.Information);
                    }
                    else if (oSearchIP.Count > 1 || oSearchName.Count > 1)
                    {
                        strReturn = strERRORPrefix + "ERROR: " + "There were multiple records found (" + (oSearchIP.Count > 1 ? oSearchIP.Results : oSearchName.Results) + ")";
                        LogIt(ObjectName, strReturn, LoggingType.Error);
                    }
                    else
                    {
                        try
                        {
                            BlueCatEntity oDelete = oSearchIP;
                            if (oSearchIP.Count == 0)
                                oDelete = oSearchName;
                            LogIt(ObjectName, "Deleting Record....", LoggingType.Information);
                            //LogIt(ObjectName, "oDelete = " + oDelete.ToString(), LoggingType.Debug);
                            //LogIt(ObjectName, "oDelete.Count = " + oDelete.Count.ToString(), LoggingType.Debug);
                            //LogIt(ObjectName, "oDelete.Entity = " + oDelete.Entity.ToString(), LoggingType.Debug);
                            //LogIt(ObjectName, "oDelete.Entity.id = " + oDelete.Entity.id.ToString(), LoggingType.Debug);
                            oProteusAPI.delete(oDelete.Entity.id);
                            if (HideNotification == false)
                            {
                                // Email IP Address Mailbox
                                Functions oFunction = new Functions(0, dsn, intEnvironment);
                                string strSubject = (Decommission ? "IP Decommission - BlueCat" : "BlueCat IP Notification");
                                string strEMailIdsTO = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_BLUECAT_DNS_TO");
                                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_BLUECAT_DNS_BCC");
                                oFunction.SendEmail(strSubject, strEMailIdsTO, "", strEMailIdsBCC, strSubject, "The following IP Address was DELETED from BlueCat..." + Environment.NewLine + Environment.NewLine + oDelete.Name + " = " + GetBluecatProperty(oDelete.Entity, "address"), false, true);
                                LogIt(ObjectName, "BlueCat IP Notification Sent", LoggingType.Information);
                            }
                            if (IncludeStaging == false)
                            {
                                LogIt(ObjectName, "Creating staging record...", LoggingType.Debug);
                                // We did not delete a staging record...so set the record we just deleted back to a staging record.
                                APIEntity oConfiguration = oProteusAPI.getEntityByName(0, oVariable.BlueCatConfiguration(), "Configuration");
                                APIEntity oView = oProteusAPI.getEntityByName(oConfiguration.id, oVariable.BlueCatView(), "View");

                                string properties = "NWS=clearview|requestor=clearview|modified-by=xacview|modified=" + DateTime.Now.ToString() + "|name=" + strStaging + "|";
                                oProteusAPI.assignIP4Address(oConfiguration.id, GetBluecatProperty(oDelete.Entity, "address"), "", "", "MAKE_STATIC", properties);
                                LogIt(ObjectName, "Staging record created", LoggingType.Information);
                            }
                            strReturn = "SUCCESS";
                            LogIt(ObjectName, "Record deleted SUCCESSFULLY!!", LoggingType.Information);
                        }
                        catch (Exception exBluecat)
                        {
                            strReturn = strERRORPrefix + "ERROR: (Bluecat # 2) " + exBluecat.Message;
                            LogIt(ObjectName, strReturn, LoggingType.Error);
                        }
                    }

                    // Log out
                    oProteusAPI.logout();
                }
            }
            else
            {
                oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished...Access Denied (" + strUser + ")"), EventLogEntryType.Information);
                return strERRORPrefix + "Access Denied (" + strUser + ")";
            }
            return strReturn;
        }
        public string Search(string ObjectAddress, string ObjectName)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            string strReturn = "";

            if (boolCanWrite)
            {
                Variables oVariable = new Variables(intEnvironment);
                ProteusAPI oProteusAPI = new ProteusAPI();
                oProteusAPI.CookieContainer = new CookieContainer();
                //oProteusAPI.Proxy = oVariable.GetProxy(dsn);
                oProteusAPI.Url = oVariable.BlueCatWebService(dsn);

                // Login
                oProteusAPI = LoginBluecat(oProteusAPI, oVariable);

                if (oProteusAPI == null)
                    return strERRORPrefix + "ERROR: " + "Unable to login (" + strBluecatLogin + ")";
                else
                {
                    BlueCatEntity oEntity = SearchBluecat(oProteusAPI, ObjectAddress, ObjectName, false);
                    if (oEntity.Count == 0)
                        strReturn = strERRORNotFound;
                    else if (oEntity.Count > 1)
                        strReturn = strERRORPrefix + "ERROR: " + "There were multiple records found (" + oEntity.Results + ")";
                    else
                    {
                        if (ObjectAddress.Trim() != "")
                            strReturn = oEntity.Name;
                        else if (ObjectName.Trim() != "")
                            strReturn = GetBluecatProperty(oEntity.Entity, "address");
                        else
                            strReturn = strERRORPrefix + "ERROR: " + "Invalid Search";
                    }

                    // Log out
                    oProteusAPI.logout();
                }
            }
            else
            {
                oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished...Access Denied (" + strUser + ")"), EventLogEntryType.Information);
                return strERRORPrefix + "Access Denied (" + strUser + ")";
            }
            return strReturn;
        }
        private BlueCatEntity SearchBluecat(ProteusAPI oProteusAPI, string ObjectAddress, string ObjectName, bool IncludeStaging)
        {
            BlueCatEntity oReturn = new BlueCatEntity();
            if (ObjectAddress.Trim() != "")
            {
                string strSearch = ObjectAddress.Trim();
                APIEntity[] oSearch = oProteusAPI.searchByCategory(strSearch, "IP4_OBJECTS", 0, 1000);
                if (oSearch.Length == 1)
                {
                    if (oSearch[0] != null && (IncludeStaging == true || (oSearch[0].name != null && oSearch[0].name.ToUpper().Contains("STAGING") == false)))
                    {
                        oReturn.Entity = oSearch[0];
                        oReturn.Count = 1;
                    }
                    //else if (oSearch[0] != null && IncludeStaging == false && oSearch[0].name == null)
                    //{
                    //    // The IP address does not have a NAME.  Check to make sure it doesn't have a HostRecord.
                    //    APIEntity[] oEntities = oProteusAPI.getLinkedEntities(oSearch[0].id, "HostRecord", 0, 100);
                    //    if (oEntities.Length > 0)
                    //    {
                    //        // Found HostRecord for IP address, with no name.
                    //        oReturn.Entity = oEntities[0];
                    //        oReturn.Count = 1;
                    //    }
                    //}
                }
                else
                {
                    foreach (APIEntity oSearching in oSearch)
                    {
                        if (oSearching != null && (IncludeStaging == true || (oSearching.name != null && oSearching.name.ToUpper().Contains("STAGING") == false)))
                        {
                            string strIP = GetBluecatProperty(oSearching, "address");
                            if (oReturn.Results != "")
                                oReturn.Results += ",";
                            oReturn.Results += strIP;
                            if (strIP == strSearch)
                            {
                                oReturn.Count = 1;
                                oReturn.Entity = oSearching;
                                break;
                            }
                        }
                    }
                    // LIKE
                    if (boolLike == false && oReturn.Entity == null)
                        oReturn.Count = 0;
                }
            }
            else if (ObjectName.Trim() != "")
            {
                string strSearch = ObjectName.Trim();
                int intFound = 0;
                APIEntity[] oSearch = oProteusAPI.searchByCategory(strSearch, "IP4_OBJECTS", 0, 1000);
                oReturn.Count = oSearch.Length;
                if (oReturn.Count == 0)
                {
                    // Try to search for HostRecords.
                    oSearch = oProteusAPI.searchByObjectTypes(strSearch, "HostRecord", 0, 1000);
                    if (oSearch.Length > 0)
                    {
                        oReturn.Count = oSearch.Length;
                        foreach (APIEntity oSearching in oSearch)
                        {
                            // Found a DNS Host Record (that was not labeled properly)
                            string strName = "N/A";
                            if (oSearching.name != null)
                                strName = oSearching.name;
                            if (strName.Contains(" -") == true)
                            {
                                // The IPAM Name might be "WSERVER-backup - Room A - etc..." so get it to "WSERVER-backup"
                                strName = strName.Substring(0, strName.IndexOf(" -"));
                            }
                            if (oReturn.Results != "")
                                oReturn.Results += ",";
                            oReturn.Results += strName;
                            if (strName.ToUpper() == strSearch.ToUpper())
                            {
                                APIEntity[] oEntities = oProteusAPI.getLinkedEntities(oSearching.id, "IP4Address", 0, 100);
                                if (oEntities.Length == 1)
                                {
                                    // Got the IP Address Record
                                    oReturn.Entity = oEntities[0];
                                    oReturn.Count = 1;
                                    // Update Host Record with Name
                                    oReturn.Entity.name = strSearch.ToUpper();
                                    oProteusAPI.update(oReturn.Entity);
                                }
                            }
                        }
                        // LIKE
                        if (boolLike == false && oReturn.Entity == null)
                            oReturn.Count = 0;
                    }
                }
                else
                {
                    if (oReturn.Count == 1)
                        oReturn.Count = 0;
                    foreach (APIEntity oSearching in oSearch)
                    {
                        string strName = "N/A";
                        if (oSearching.name != null)
                            strName = oSearching.name;
                        APIEntity[] oEntities = oProteusAPI.getLinkedEntities(oSearching.id, "HostRecord", 0, 100);
                        if (oEntities != null && oEntities.Length == 1 && oEntities[0].name != null)
                            strName = oEntities[0].name;
                        if (strName.Contains(" -") == true)
                        {
                            // The IPAM Name might be "WSERVER-backup - Room A - etc..." so get it to "WSERVER-backup"
                            strName = strName.Substring(0, strName.IndexOf(" -"));
                        }
                        if (oReturn.Results != "")
                            oReturn.Results += ",";
                        oReturn.Results += strName;
                        if (strName.ToUpper() == strSearch.ToUpper())
                        {
                            intFound++;
                            oReturn.Count = intFound;
                            //oReturn.Count = 1;
                            oReturn.Entity = oSearching;
                            //break;
                        }
                    }
                    // LIKE
                    if (boolLike == false && oReturn.Entity == null)
                        oReturn.Count = 0;
                }
            }
            if (oReturn != null && oReturn.Entity != null)
            {
                if (oReturn.Entity.name != null)
                {
                    string strFQDN = oReturn.Entity.name;
                    if (strFQDN.Contains(" -") == true)
                    {
                        // The IPAM Name might be "WSERVER-backup - Room A - etc..." so get it to "WSERVER-backup"
                        oReturn.Name = strFQDN.Substring(0, strFQDN.IndexOf(" -"));
                    }
                    else
                        oReturn.Name = strFQDN;
                }
                APIEntity[] oFQDNs = oProteusAPI.getLinkedEntities(oReturn.Entity.id, "HostRecord", 0, 100);
                if (oFQDNs != null && oFQDNs.Length == 1)
                    oReturn.Name = oFQDNs[0].name;
            }
            return oReturn;
        }
        private string GetBluecatProperty(APIEntity _entity, string _property)
        {
            string strReturn = "";
            string[] _properties = _entity.properties.Split('|');
            foreach (string strProperty in _properties)
            {
                if (strProperty.ToUpper().StartsWith(_property.ToUpper()) == true)
                {
                    if (strProperty.Contains("=") == true)
                    {
                        strReturn = strProperty.Substring(strProperty.IndexOf("=") + 1);
                        break;
                    }
                }
            }
            return strReturn;
        }
        private ProteusAPI LoginBluecat(ProteusAPI oProteusAPI, Variables oVariable)
        {
            strBluecatLogin = "";
            try
            {
                oProteusAPI.login(oVariable.BlueCatUsername(), oVariable.BlueCatPassword());
                APIEntity oTest = oProteusAPI.getEntityByName(0, oVariable.BlueCatConfiguration(), "Configuration");
            }
            catch (Exception exLogin)
            {
                //if (exLogin.Message.ToUpper().Contains("HTTP STATUS 503: SERVICE UNAVAILABLE") == true)
                //{
                //    oProteusAPI.Url = oVariable.BlueCatWebService2();
                //    try
                //    {
                //        oProteusAPI.login(oVariable.BlueCatUsername(), oVariable.BlueCatPassword());
                //        APIEntity oTest2 = oProteusAPI.getEntityByName(0, oVariable.BlueCatConfiguration(), "Configuration");
                //    }
                //    catch (Exception exLogin2)
                //    {
                //        strBluecatLogin = exLogin2.Message;
                //        return null;
                //    }
                //}
                //else
                //{
                strBluecatLogin = exLogin.Message;
                return null;
                //}
            }
            return oProteusAPI;
        }
    }
}