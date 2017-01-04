using NCC.ClearView.Application.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace Presentation.Web.Services
{
    public class QIP : BaseClass
    {
        private string strQIPdir = @"F:\qip\cli\";
        private string strQIPcreate = "qip-setobject.exe";
        private string strQIPdelete = "qip-del.exe";
        private string strQIPsearch = "qip-search.exe";
        private string strQIPmove = "qip-move.exe";
        private string strQIPget = "qip-getobjectprof.exe";

        public QIP(string strWebMethodName)
            : base(strWebMethodName)
        {
        }
        public string Create(string ObjectAddress, string ObjectName, string ObjectClass, string Aliases, string DomainName, string NameService, string DynamicDNSUpdate, int intUser, int intAnswer, bool boolDeleteFiles, bool boolUpdate)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            if (boolCanWrite)
            {
                string strSearchName = SearchQIPAddress(ObjectAddress, false, boolDeleteFiles);
                string strSearchAddress = SearchQIPName(ObjectName, false, boolDeleteFiles);
                string ObjectNameFQ = ObjectName.Trim() + "." + DomainName.Trim();
                bool boolAliasExists = AliasExists(Aliases, boolDeleteFiles);
                bool boolAliasInUseByAnotherAddress = (boolAliasExists == true && AliasInUseByAnotherAddress(Aliases, ObjectAddress, boolDeleteFiles));

                bool boolSuccess = false;
                string strReturn = "";
                if (strSearchName.Trim().ToUpper() == strERRORNotFound && strSearchAddress.Trim().ToUpper() == strERRORNotFound && boolAliasExists == false)
                {
                    // Neither the address nor the name nor the alias(es) exist...ok to create
                    strReturn = CreateOrModify(ObjectAddress, ObjectName, ObjectClass, GetAliases(Aliases, ObjectAddress, true), DomainName, NameService, DynamicDNSUpdate, boolDeleteFiles, false);
                    if (strReturn.StartsWith(strERRORPrefix) == false)
                        boolSuccess = true;
                }
                else
                {
                    // Either the address or the name already exists.
                    // If name and address of existing match the new ones, its ok...send DUPLICATE
                    if (strSearchName.Trim().ToUpper() == ObjectNameFQ.ToUpper() && strSearchAddress.Trim().ToUpper() == ObjectAddress.Trim().ToUpper() && (Aliases == "" || (boolAliasExists == true && boolAliasInUseByAnotherAddress == false)))
                    {
                        strReturn = strERRORPrefix + "DUPLICATE";
                        boolSuccess = true;
                    }
                    else
                    {
                        strReturn = strERRORPrefix + "CONFLICT : Create (" + ObjectAddress + "=" + strSearchName + "," + ObjectNameFQ + "=" + strSearchAddress + ")";
                    }
                }

                if (boolSuccess == false)
                {
                }
                return strReturn;
            }
            else
            {
                oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished...Access Denied (" + strUser + ")"), EventLogEntryType.Information);
                return strERRORPrefix + "Access Denied (" + strUser + ")";
            }
        }



        public string Update(string ObjectAddress, string ObjectName, string ObjectClass, string Aliases, string DomainName, string NameService, string DynamicDNSUpdate, int intUser, int intAnswer, bool boolDeleteFiles, bool boolUpdate)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            if (boolCanWrite)
            {
                string strSearchName = SearchQIPAddress(ObjectAddress, false, boolDeleteFiles);
                string strSearchAddress = SearchQIPName(ObjectName, false, boolDeleteFiles);
                string ObjectNameFQ = ObjectName.Trim() + "." + DomainName.Trim();
                bool boolAliasExists = AliasExists(Aliases, boolDeleteFiles);
                bool boolAliasInUseByAnotherAddress = (boolAliasExists == true && AliasInUseByAnotherAddress(Aliases, ObjectAddress, boolDeleteFiles));

                bool boolSuccess = false;
                string strReturn = "";
                if (strSearchName.Trim().ToUpper() == strERRORNotFound && strSearchAddress.Trim().ToUpper() == strERRORNotFound && boolAliasExists == false)
                {
                    // Neither the address nor the name nor the alias(es) exist...ok to create
                    strReturn = CreateOrModify(ObjectAddress, ObjectName, ObjectClass, GetAliases(Aliases, ObjectAddress, true), DomainName, NameService, DynamicDNSUpdate, boolDeleteFiles, false);
                    if (strReturn.StartsWith(strERRORPrefix) == false)
                        boolSuccess = true;
                }
                else
                {
                    if (strSearchAddress.Trim().ToUpper() == strERRORNotFound || strSearchName.Trim().ToUpper() == strERRORNotFound || boolAliasExists == false)
                    {
                        // NAME has been changed (and does not exist)
                        // Update the NAME using IP ADDRESS
                        // --- OR ---
                        // IP ADDRESS has been changed (and does not exist)
                        // Update the IP ADDRESS using NAME
                        // --- OR ---
                        // ALIAS has been changed (and does not exist)
                        // Update the ALIAS using IP ADDRESS
                        strReturn = CreateOrModify(ObjectAddress, ObjectName, ObjectClass, GetAliases(Aliases, ObjectAddress, true), DomainName, NameService, DynamicDNSUpdate, boolDeleteFiles, true);
                        if (strReturn.StartsWith(strERRORPrefix) == false)
                            boolSuccess = true;
                    }
                    else
                    {
                        // Either the address or the name already exists.
                        // If name and address of existing match the new ones, its ok...send DUPLICATE
                        if (strSearchName.Trim().ToUpper() == ObjectNameFQ.ToUpper() && strSearchAddress.Trim().ToUpper() == ObjectAddress.Trim().ToUpper() && (Aliases == "" || (boolAliasExists == true && boolAliasInUseByAnotherAddress == false)))
                        {
                            strReturn = strERRORPrefix + "DUPLICATE";
                            boolSuccess = true;
                        }
                        else
                        {
                            strReturn = strERRORPrefix + "CONFLICT : Update (" + ObjectAddress + "=" + strSearchName + "," + ObjectName + "=" + strSearchAddress + ")";
                        }
                    }
                }

                if (boolSuccess == false)
                {
                }
                return strReturn;
            }
            else
            {
                oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished...Access Denied (" + strUser + ")"), EventLogEntryType.Information);
                return strERRORPrefix + "Access Denied (" + strUser + ")";
            }
        }

        private string CreateOrModify(string ObjectAddress, string ObjectName, string ObjectClass, string Aliases, string DomainName, string NameService, string DynamicDNSUpdate, bool boolDeleteFiles, bool boolUpdate)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            // Build the script
            // Format File = STATIC
            //      Format: ObjectAddress,ObjectName,ObjectClass,Aliases,DomainName,NameService,DynamicDNSUpdate
            // Data File = DYNAMIC
            //      Example: 10.129.97.95,CAT3ADP40DB,Server,CAT3ADP30DB,pncbank.com, A PTR, A PTR CNAME MX

            string strFilename = (boolUpdate ? "m_" : "c_") + ObjectName.Trim();
            string strScriptDir = strQIPdir + "scripts";
            string strScriptDirCreate = CreateDNSFolder(strScriptDir);
            string strOutputFile = strScriptDir + "\\" + strFilename + ".out";

            StartOutput(strOutputFile);

            LogIt(oOutput, "Beginning " + (boolUpdate ? "modification" : "registration") + " of " + ObjectAddress.Trim() + " (" + ObjectName.Trim() + ") in " + DomainName);
            LogIt(oOutput, strScriptDirCreate);

            // Write the Format File
            string strFormat = strScriptDir + "\\" + strFilename + ".fmt";
            StreamWriter oFormat = new StreamWriter(strFormat);
            oFormat.WriteLine("ObjectAddress,ObjectName,ObjectClass,Aliases,DomainName,NameService,DynamicDNSUpdate");
            oFormat.Flush();
            oFormat.Close();
            LogIt(oOutput, strFormat + " format file created.");

            string strInput = strScriptDir + "\\" + strFilename + ".txt";
            StreamWriter oInput = new StreamWriter(strInput);
            //oInput.WriteLine("10.129.97.95,CAT3ADP40DB,Server,CAT3ADP30DB,pncbank.com, A PTR, A PTR CNAME MX");
            oInput.WriteLine(ObjectAddress.Trim() + "," + ObjectName.Trim() + "," + ObjectClass.Trim() + "," + Aliases.Trim() + "," + DomainName.Trim() + "," + NameService.Trim() + "," + DynamicDNSUpdate.Trim());
            oInput.Flush();
            oInput.Close();
            LogIt(oOutput, strInput + " input file created.");

            //// Write the Script
            string strReject = strScriptDir + "\\" + strFilename + ".rjt";
            string strError = strScriptDir + "\\" + strFilename + ".err";

            string strOrganization = oVariable.DNSOrganization();
            if (strOrganization != "")
                strOrganization = " -o \"" + strOrganization + "\"";
            string strReturn = "";
            if (boolUpdate == true)
            {
                // UPDATE = use either the SETOBJECT (with "-t" parameter) or the MOVE command
                string strAddressSearchResult = SearchQIPAddress(ObjectAddress, false, boolDeleteFiles);
                if (strAddressSearchResult.Trim().ToUpper() == strERRORNotFound)
                {
                    // Address = NOT Found
                    // Name = Found
                    LogIt(oOutput, "UPDATE: Name is found, so you are updating the IP address with the new IP address (use the Name, use QIP-MOVE)");
                    string strAddress = SearchQIPName(ObjectName, false, boolDeleteFiles);
                    if (strAddress.StartsWith(strERRORPrefix) == false)
                    {
                        // You must use fully-qualified name when updating by name. Search by address to get name.
                        string strName = SearchQIPAddress(strAddress, false, boolDeleteFiles);
                        if (strName.StartsWith(strERRORPrefix) == false)
                        {
                            // Example: qip-move -t object -n ummy3.cle.ncc -l object -e 10.49.255.38 -u qipman -p qipman -o "PNC Internal Network"
                            strReturn = RunPSEXEC(strScriptDir, strFilename, strQIPdir + strQIPmove + " -t object -n " + strName + " -l object -e " + ObjectAddress + " -u " + oVariable.DNSUsername() + " -p " + oVariable.DNSPassword() + strOrganization, oOutput, "", true, boolDeleteFiles, true, new string[] { "OUT" });
                        }
                        else
                            strReturn = strName;
                    }
                    else
                        strReturn = strAddress;
                }
                else
                {
                    // Address = Found
                    // Name = NOT Found
                    LogIt(oOutput, "UPDATE: Name is NOT found, so you are updating the name / alias(es) with the new name / alias(es) (use the IP Address, use QIP-SETOBJECT with '-t' parameter)");
                    // Example: qip-setobject -o organization -u userid -p password -f glen-set.fmt -d glen-set.txt -t -r glen.rjt -e glen.err 
                    strReturn = RunPSEXEC(strScriptDir, strFilename, strQIPdir + strQIPcreate + strOrganization + " -u " + oVariable.DNSUsername() + " -p " + oVariable.DNSPassword() + " -f " + strFormat + " -d " + strInput + " -t" + " -r " + strReject + " -e " + strError, oOutput, strError, true, boolDeleteFiles, true, new string[] { "OUT" });
                }
            }
            else
            {
                // Example: qip-setobject -o organization -u userid -p password -f glen-set.fmt -d glen-set.txt -t -r glen.rjt -e glen.err 
                strReturn = RunPSEXEC(strScriptDir, strFilename, strQIPdir + strQIPcreate + strOrganization + " -u " + oVariable.DNSUsername() + " -p " + oVariable.DNSPassword() + " -f " + strFormat + " -d " + strInput + " -r " + strReject + " -e " + strError, oOutput, strError, true, boolDeleteFiles, true, new string[] { "OUT" });
            }
            oOutput.Flush();
            oOutput.Close();
            if (boolUpdate == true)
                AddOutput(ObjectName, "DNS_UPDATE", strOutputFile);
            else
                AddOutput(ObjectName, "DNS_CREATE", strOutputFile);
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished..." + strReturn), EventLogEntryType.Information);
            return strReturn;
        }

        public string Delete(string ObjectAddress, string ObjectName, int intUser, bool boolDeleteFiles)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            if (boolCanWrite)
            {
                if (ObjectAddress.Trim() != "")
                    return DeleteQIPAddress(ObjectAddress.Trim(), intUser, boolDeleteFiles);
                else if (ObjectName.Trim() != "")
                    return DeleteQIPName(ObjectName.Trim(), intUser, boolDeleteFiles);
                else
                    return "Invalid Parameter Data";
            }
            else
            {
                oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished...Access Denied (" + strUser + ")"), EventLogEntryType.Information);
                return strERRORPrefix + "Access Denied (" + strUser + ")";
            }
        }

        private string DeleteQIPAddress(string ObjectAddress, int intUser, bool boolDeleteFiles)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            // Format: qip-del -o organization –u userid -p password –a 198.200.138.123 -t object
            string strName = SearchQIPAddress(ObjectAddress, false, boolDeleteFiles);
            if (strName.StartsWith(strERRORPrefix) == false)
                return Delete(ObjectAddress, "", true, "d_" + ObjectAddress.Trim(), boolDeleteFiles);
            else
                return strName;
        }

        private string DeleteQIPName(string ObjectName, int intUser, bool boolDeleteFiles)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            // Format: qip-del -o organization –u userid -p password –n test.pncbank.com -t object
            string strAddress = SearchQIPName(ObjectName, false, boolDeleteFiles);
            if (strAddress.StartsWith(strERRORPrefix) == false)
            {
                // You must use fully-qualified name when deleting by name. Search by address to get name.
                ObjectName = SearchQIPAddress(strAddress, false, boolDeleteFiles);
                return Delete("", ObjectName, false, "d_" + ObjectName.Trim(), boolDeleteFiles);
            }
            else
                return strAddress;
        }

        private string Delete(string ObjectAddress, string ObjectName, bool _address, string strFilename, bool boolDeleteFiles)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            string strScriptDir = strQIPdir + "scripts";
            string strScriptDirCreate = CreateDNSFolder(strScriptDir);
            string strOutputFile = strScriptDir + "\\" + strFilename + ".out";

            StartOutput(strOutputFile);

            if (_address == true)
                LogIt(oOutput, "Beginning deletion of " + ObjectAddress.Trim());
            else
                LogIt(oOutput, "Beginning deletion of " + ObjectName.Trim());
            LogIt(oOutput, strScriptDirCreate);

            string strOrganization = oVariable.DNSOrganization();
            if (strOrganization != "")
                strOrganization = " -o \"" + strOrganization + "\"";
            string strReturn = "";
            if (_address == true)
                strReturn = RunPSEXEC(strScriptDir, strFilename, strQIPdir + strQIPdelete + strOrganization + " -u " + oVariable.DNSUsername() + " -p " + oVariable.DNSPassword() + " -a " + ObjectAddress.Trim() + " -t object", oOutput, "", true, boolDeleteFiles, true, new string[] { "OUT" });
            else
                strReturn = RunPSEXEC(strScriptDir, strFilename, strQIPdir + strQIPdelete + strOrganization + " -u " + oVariable.DNSUsername() + " -p " + oVariable.DNSPassword() + " -n " + ObjectName.Trim() + " -t object", oOutput, "", true, boolDeleteFiles, true, new string[] { "OUT" });
            oOutput.Flush();
            oOutput.Close();
            AddOutput(ObjectName, "DNS_DELETE", strOutputFile);
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished..." + strReturn), EventLogEntryType.Information);
            return strReturn;
        }

        public string Search(string ObjectAddress, string ObjectName, bool boolIsAlias, bool boolDeleteFiles)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            if (boolCanWrite)
            {
                if (ObjectAddress.Trim() != "")
                    return SearchQIPAddress(ObjectAddress.Trim(), boolIsAlias, boolDeleteFiles);
                else if (ObjectName.Trim() != "")
                    return SearchQIPName(ObjectName.Trim(), boolIsAlias, boolDeleteFiles);
                else
                    return "Invalid Parameter Data";
            }
            else
            {
                oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished...Access Denied (" + strUser + ")"), EventLogEntryType.Information);
                return strERRORPrefix + "Access Denied (" + strUser + ")";
            }
        }

        private string SearchQIPAddress(string ObjectAddress, bool boolIsAlias, bool boolDeleteFiles)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            if (boolCanRead)
            {
                if (boolIsAlias == false)
                {
                    // Get the NAME associated with an IP ADDRESS
                    // Input:  qip-search -o organization –u userid -p password –t object -a 10.49.1.1 –f search.txt
                    // Output: 10.49.255.1|healy4pnc.cle.ncc|OBJECT
                    return Search(ObjectAddress, "", "", "s_" + ObjectAddress.Trim(), boolDeleteFiles);
                }
                else
                {
                    // Get the ALIAS associated with an IP ADDRESS
                    return SearchQIPAlias(ObjectAddress, "s_" + ObjectAddress.Trim(), boolDeleteFiles);
                }
            }
            else
            {
                oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished...Access Denied (" + strUser + ")"), EventLogEntryType.Information);
                return strERRORPrefix + "Access Denied (" + strUser + ")";
            }
        }

        private string SearchQIPName(string ObjectName, bool boolIsAlias, bool boolDeleteFiles)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            if (boolCanRead)
            {
                if (boolIsAlias == false)
                {
                    // Get the IP ADDRESS associated with a NAME
                    // Input:  qip-search -o organization –u userid -p password –t object –n ohcleiis1020 –f search.txt
                    // Output: 10.49.255.1|healy4pnc.cle.ncc|OBJECT
                    if (ObjectName.Contains(".") == true)
                        ObjectName = ObjectName.Substring(0, ObjectName.IndexOf("."));
                    return Search("", ObjectName, "", "s_" + ObjectName.Trim(), boolDeleteFiles);
                }
                else
                {
                    // Get the IP ADDRESS associated with an ALIAS
                    // Input:  qip-search.exe -o organization –u userid -p password -t alias -n alias1
                    // Output: 10.49.255.1|alias1|ALIAS
                    return Search("", "", ObjectName, "s_" + ObjectName.Trim(), boolDeleteFiles);
                }
            }
            else
            {
                oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished...Access Denied (" + strUser + ")"), EventLogEntryType.Information);
                return strERRORPrefix + "Access Denied (" + strUser + ")";
            }
        }

        private string Search(string ObjectAddress, string ObjectName, string Alias, string strFilename, bool boolDeleteFiles)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            string strScriptDir = strQIPdir + "scripts";
            string strScriptDirCreate = CreateDNSFolder(strScriptDir);

            string strRead = strScriptDir + "\\" + strFilename + ".txt";
            string strOrganization = oVariable.DNSOrganization();
            if (strOrganization != "")
                strOrganization = " -o \"" + strOrganization + "\"";
            string strReturn = "";
            if (ObjectAddress != "")
                strReturn = RunPSEXEC(strScriptDir, strFilename, strQIPdir + strQIPsearch + strOrganization + " -u " + oVariable.DNSUsername() + " -p " + oVariable.DNSPassword() + " -t object -a " + ObjectAddress.Trim() + " -f " + strRead, null, strRead, false, boolDeleteFiles, false, new string[] { "NONE" });
            else if (ObjectName != "")
                strReturn = RunPSEXEC(strScriptDir, strFilename, strQIPdir + strQIPsearch + strOrganization + " -u " + oVariable.DNSUsername() + " -p " + oVariable.DNSPassword() + " -t object -n " + ObjectName.Trim() + " -f " + strRead, null, strRead, false, boolDeleteFiles, false, new string[] { "NONE" });
            else if (Alias != "")
                strReturn = RunPSEXEC(strScriptDir, strFilename, strQIPdir + strQIPsearch + strOrganization + " -u " + oVariable.DNSUsername() + " -p " + oVariable.DNSPassword() + " -t alias -n " + Alias.Trim() + " -f " + strRead, null, strRead, false, boolDeleteFiles, false, new string[] { "NONE" });
            else
                return "ERROR: Invalid Parameter";
            if (strReturn != "")
            {
                try
                {
                    char[] strSlashSplit = { '|' };
                    string[] strSlash = strReturn.Split(strSlashSplit);
                    if (strSlash.Length == 3)
                    {
                        if (ObjectAddress != "")
                            strReturn = strSlash[1];
                        else if (ObjectName != "")
                            strReturn = strSlash[0];
                        else if (Alias != "")
                            strReturn = strSlash[0];
                    }
                    else if (strSlash.Length == 1 && strSlash[0].Contains(strERRORPrefix + "ERROR") == true)
                        strReturn = strERRORNotFound;
                    else
                        strReturn = strERRORPrefix + "Duplicate records found or invalid format (# = " + strSlash.Length.ToString() + " | Message = " + strReturn + ")";
                }
                catch (Exception exFormat)
                {
                    strReturn = "EXCEPTION: " + exFormat.Message + " (" + exFormat.Source + ") (" + exFormat.StackTrace + ")";
                }
            }
            else
                strReturn = "No records found";
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished..." + strReturn), EventLogEntryType.Information);
            return strReturn;
        }

        private string SearchQIPAlias(string ObjectAddress, string strFilename, bool boolDeleteFiles)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            // DEBUG
            //string strRead = @"C:\test.txt";
            //StreamReader oReader = new StreamReader(strRead);
            //string strReturn = oReader.ReadToEnd();
            //oReader.Close();

            string strScriptDir = strQIPdir + "scripts";
            string strScriptDirCreate = CreateDNSFolder(strScriptDir);

            string strRead = strScriptDir + "\\" + strFilename + ".txt";

            string strOrganization = oVariable.DNSOrganization();
            if (strOrganization != "")
                strOrganization = " -o \"" + strOrganization + "\"";
            string strReturn = RunPSEXEC(strScriptDir, strFilename, strQIPdir + strQIPget + strOrganization + " -u " + oVariable.DNSUsername() + " -p " + oVariable.DNSPassword() + " -a " + ObjectAddress.Trim() + " -f " + strRead, null, strRead, false, boolDeleteFiles, false, new string[] { "NONE" });
            if (strReturn != "")
            {
                try
                {
                    string strNewLineReturn = "";
                    char[] strNewLineSplit = { '\n' };
                    string strNewLineString = "Aliases=";
                    string[] strNewLine = strReturn.Split(strNewLineSplit);
                    for (int ii = 0; ii < strNewLine.Length; ii++)
                    {
                        if (strNewLine[ii].Trim().StartsWith(strNewLineString) == true)
                        {
                            strNewLineReturn = strNewLine[ii].Trim();
                            strNewLineReturn = strNewLineReturn.Substring(strNewLineString.Length);
                            // Replace last '\r' character with blank
                            while (strNewLineReturn.Contains("\r") == true)
                                strNewLineReturn = strNewLineReturn.Replace("\r", "");
                            // If multiple are returned, a '.' is at the end of each alias along with a space - replace with more friendly delimiter (';')
                            while (strNewLineReturn.Contains(". ") == true)
                                strNewLineReturn = strNewLineReturn.Replace(". ", ";");
                            // Just in case the delimiter is not a '. ' in the future, replace all spaces with the delimiter ';'
                            while (strNewLineReturn.Contains(" ") == true)
                                strNewLineReturn = strNewLineReturn.Replace(" ", ";");
                            // If there is a '.' at the end, replace it with a blank
                            if (strNewLineReturn.EndsWith(".") == true)
                                strNewLineReturn = strNewLineReturn.Substring(0, strNewLineReturn.Length - 1);
                            strReturn = strNewLineReturn;
                            break;
                        }
                    }
                    if (strNewLineReturn == "")
                        strReturn = strERRORNotFound;
                }
                catch (Exception exFormat)
                {
                    strReturn = "EXCEPTION: " + exFormat.Message + " (" + exFormat.Source + ") (" + exFormat.StackTrace + ")";
                }
            }
            else
                strReturn = "No records found";
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished..." + strReturn), EventLogEntryType.Information);
            return strReturn;
        }
        private string CreateDNSFolder(string strScriptDir)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            string strReturn = "";
            if (Directory.Exists(strScriptDir) == false)
            {
                strReturn = strScriptDir + " does not exist...creating...";
                Directory.CreateDirectory(strScriptDir);
            }
            else
                strReturn = strScriptDir + " already exists (skipping creation)...";
            return strReturn;
        }
        private void AddOutput(string _name, string _title, string _file)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            string strContent = "";
            string strNow = DateTime.Now.ToShortDateString();
            while (strNow.Contains("/") == true)
                strNow = strNow.Replace("/", "");
            try
            {
                StreamReader oReader = new StreamReader(_file);
                strContent = oReader.ReadToEnd();
                oReader.Close();
            }
            catch
            {
                strContent = "There was a problem getting the information from the output file (" + _file + ")";
            }
            // Get ServerID from Name
            Servers oServer = new Servers(0, dsn);
            DataSet dsServer = oServer.Get(_name, false);
            if (dsServer.Tables[0].Rows.Count > 0)
            {
                int intServer = 0;
                if (Int32.TryParse(dsServer.Tables[0].Rows[0]["id"].ToString(), out intServer) == true)
                {
                    oServer.AddOutput(intServer, _title + "_" + strNow, strContent);
                    for (int ii = 0; ii < 5; ii++)
                    {
                        // Delete output file
                        try
                        {
                            File.Delete(_file);
                        }
                        catch
                        {
                            Thread.Sleep(3000);
                        }
                    }
                }
            }
        }

        private string RunPSEXEC(string strScriptDir, string strFilename, string strBatCommand, StreamWriter oOutput, string strFileToRead, bool boolSuccessOnlyIfFileToReadIsBlank, bool boolDeleteFiles, bool boolDeleteFilesOnlySuccess, string[] arrDeleteExceptions)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            string strBAT = strScriptDir + "\\" + strFilename + ".bat";
            string strBATOUT = strScriptDir + "\\" + strFilename + ".res";
            LogIt(oOutput, "Starting registration script..." + strBAT);
            StreamWriter oWriter = new StreamWriter(strBAT);
            LogIt(oOutput, "  Contents of script: " + strBatCommand);
            oWriter.WriteLine(strBatCommand);
            oWriter.Flush();
            oWriter.Close();

            string strPSEXEC = strScriptDir + "\\psexec.exe";
            ProcessStartInfo _info = new ProcessStartInfo(strPSEXEC);
            _info.WorkingDirectory = strScriptDir;
            _info.Arguments = "\\\\localhost -u " + oVariable.Domain() + "\\" + oVariable.ADUser() + " -p " + oVariable.ADPassword() + " -e -i cmd.exe /c " + strBAT + " >" + strBATOUT + " 2>&1";
            LogIt(oOutput, "PSEXEC Script = " + strPSEXEC + " \\\\localhost -u " + oVariable.Domain() + "\\" + oVariable.ADUser() + " -p ******** -e -i cmd.exe /c " + strBAT + " >" + strBATOUT + " 2>&1");
            Process _proc = Process.Start(_info);
            _proc.WaitForExit();
            _proc.Close();

            LogIt(oOutput, "Completed script..." + strBAT);
            string strReturn = "";
            if (strFileToRead != "")
                strReturn = ReadOutput(strFileToRead, oOutput, true);
            bool boolSuccess = false;
            if (strReturn == "" && boolSuccessOnlyIfFileToReadIsBlank == true)
            {
                strReturn = "SUCCESS";
                LogIt(oOutput, "Result: " + strReturn);
                boolSuccess = true;
            }
            else if (strReturn == "" && boolSuccessOnlyIfFileToReadIsBlank == false)
            {
                strReturn = strERRORPrefix + "ERROR: No Additional Information";
                LogIt(oOutput, strReturn);
            }
            else if (strReturn != "" && boolSuccessOnlyIfFileToReadIsBlank == true)
            {
                strReturn = strERRORPrefix + "ERROR: " + strReturn;
                LogIt(oOutput, strReturn);
            }
            else
            {
                LogIt(oOutput, "Result: " + strReturn);
                boolSuccess = true;
            }
            if ((boolSuccess == true || boolDeleteFilesOnlySuccess == false) && boolDeleteFiles == true)
            {
                LogIt(oOutput, "Deleting files...");
                foreach (string strFile in Directory.GetFiles(strScriptDir + "\\", strFilename + "*.*"))
                {
                    bool boolException = false;
                    foreach (string strException in arrDeleteExceptions)
                    {
                        if (strFile.ToUpper().EndsWith(strException.ToUpper()) == true)
                        {
                            boolException = true;
                            break;
                        }
                    }
                    if (boolException == false)
                    {
                        File.Delete(strFile);
                        LogIt(oOutput, "Deleted file: " + strFile);
                    }
                    else
                        LogIt(oOutput, "SKIP deletion of file: " + strFile);
                }
            }
            return strReturn;
        }

        public string GetMacFromILO(string _ilo, int _environment, bool _delete_files)
        {
            string strReturn = RunILOCommand(_ilo, "Get_Host_Data", _environment, _delete_files);
            string strStartString = "<FIELD NAME=\"MAC\" VALUE=\"";
            string strMAC = strReturn.Substring(strReturn.IndexOf(strStartString) + strStartString.Length);
            strMAC = strMAC.Substring(0, strMAC.IndexOf("\""));
            return strMAC;
        }

        private string RunILOCommand(string _ilo, string _xml_script, int _environment, bool _delete_files)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            // C:\380s>CPQLOCFG.EXE -s 10.49.186.110 -l c:\380s\ilo.txt -f c:\380s\Get_Host_Data.xml -v -u iadmin -p qwertyui
            string strServer = "localhost";
            if (_environment == 1)
            {
                strAPIdir = @"C:\ClearViewAPI\";
                strServer = "DXP380MPB1";
            }
            string strILO = _ilo;
            while (strILO.Contains(".") == true)
                strILO = strILO.Replace(".", "_");
            string strOutput = strAPIdir + strILO + ".out";
            string strBatCommand = strAPIdir + "CPQLOCFG.EXE -s " + _ilo + " -l " + strOutput + " -f " + strAPIdir + _xml_script + ".xml -v -u " + oVariable.ILOUsername() + " -p " + oVariable.ILOPassword();

            string strBAT = strAPIdir + strILO + ".bat";
            string strBATOUT = strAPIdir + strILO + ".txt";
            LogIt(oOutput, "Starting registration script..." + strBAT);
            StreamWriter oWriter = new StreamWriter(strBAT);
            LogIt(oOutput, "  Contents of script: " + strBatCommand);
            oWriter.WriteLine(strBatCommand);
            oWriter.Flush();
            oWriter.Close();

            string strPSEXEC = strAPIdir + "psexec.exe";
            ProcessStartInfo _info = new ProcessStartInfo(strPSEXEC);
            _info.WorkingDirectory = strAPIdir;
            _info.Arguments = "\\\\" + strServer + " -u " + oVariable.Domain() + "\\" + oVariable.ADUser() + " -p " + oVariable.ADPassword() + " -e -i cmd.exe /c " + strBAT + " >" + strBATOUT + " 2>&1";
            LogIt(oOutput, "PSEXEC Script = " + strPSEXEC + " \\\\" + strServer + " -u " + oVariable.Domain() + "\\" + oVariable.ADUser() + " -p ******** -e -i cmd.exe /c " + strBAT + " >" + strBATOUT + " 2>&1");
            Process _proc = Process.Start(_info);
            _proc.WaitForExit();
            _proc.Close();

            LogIt(oOutput, "Completed script..." + strBAT);
            string strReturn = ReadOutput(strOutput, oOutput, true);
            LogIt(oOutput, strReturn);
            if (boolOutputSuccess == true && _delete_files == true)
            {
                LogIt(oOutput, "Deleting files...");
                foreach (string strFile in Directory.GetFiles(strAPIdir, strILO + "*.*"))
                {
                    File.Delete(strFile);
                    LogIt(oOutput, "Deleted file: " + strFile);
                }
            }
            return strReturn;
        }

        private bool AliasExists(string Aliases, bool boolDeleteFiles)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            bool boolAliasExists = true;
            if (Aliases != "")
            {
                char[] strAliasSplit = { ' ' };
                string[] strAlias = Aliases.Split(strAliasSplit);
                for (int ii = 0; ii < strAlias.Length; ii++)
                {
                    if (strAlias[ii].Trim() != "")
                    {
                        string strSearchAlias = SearchQIPName(strAlias[ii].Trim(), true, boolDeleteFiles);
                        if (strSearchAlias == strERRORNotFound)
                        {
                            boolAliasExists = false;
                            break;
                        }
                    }
                }
            }
            else
                boolAliasExists = false;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished..." + boolAliasExists.ToString()), EventLogEntryType.Information);
            return boolAliasExists;
        }
        private bool AliasInUseByAnotherAddress(string Aliases, string ObjectAddress, bool boolDeleteFiles)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            bool boolAliasInUseByAnotherAddress = false;
            if (Aliases != "")
            {
                char[] strAliasSplit = { ' ' };
                string[] strAlias = Aliases.Split(strAliasSplit);
                for (int ii = 0; ii < strAlias.Length; ii++)
                {
                    if (strAlias[ii].Trim() != "")
                    {
                        string strSearchAlias = SearchQIPName(strAlias[ii].Trim(), true, boolDeleteFiles);
                        if (strSearchAlias != strERRORNotFound && strSearchAlias != ObjectAddress)
                        {
                            boolAliasInUseByAnotherAddress = true;
                            break;
                        }
                    }
                }
            }
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished..." + boolAliasInUseByAnotherAddress.ToString()), EventLogEntryType.Information);
            return boolAliasInUseByAnotherAddress;
        }

        private string GetAliases(string Aliases, string ObjectAddress, bool boolDeleteFiles)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            string strAliases = "";
            if (Aliases != "")
            {
                char[] strAliasSplit = { ' ' };
                string[] strAlias = Aliases.Split(strAliasSplit);
                for (int ii = 0; ii < strAlias.Length; ii++)
                {
                    if (strAlias[ii].Trim() != "")
                    {
                        string strSearchAlias = SearchQIPName(strAlias[ii].Trim(), true, boolDeleteFiles);
                        if (strSearchAlias == strERRORNotFound || strSearchAlias != ObjectAddress)
                        {
                            if (strSearchAlias != ObjectAddress)
                            {
                                // Might need to delete
                            }
                            if (strAliases != "")
                                strAliases += " ";
                            strAliases += strAlias[ii].Trim();
                        }
                    }
                }
            }
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Finished..." + strAliases), EventLogEntryType.Information);
            return strAliases;
        }
    }
}