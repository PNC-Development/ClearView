using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;
using NCC.ClearView.Application.Core.ClearViewWS;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace NCC.ClearView.Application.Core
{
    public class AvamarReturnType
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }
        public XmlNodeList Nodes { get; set; }
    }
    public class AvamarRegistration
	{
		private string dsn = "";
		private int user = 0;
        private string strResults { get; set; }
        public AvamarRegistration(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}

        private void AddResult(int intAnswer, string strName, Log oLog, string _result)
        {
            oLog.AddEvent(intAnswer, strName, "", _result, LoggingType.Debug);
            if (strResults != "")
                strResults += "<br/>";
            strResults += _result;
        }
        public void ClearResults()
        {
            strResults = "";
        }
        public const int AvamarAlreadyRegistered = 22238;   // Event Code from Avamar API for "A client with this name already exists and duplicate names are prohibited."
        public const int AvamarAlreadyRetired = 22236;      // Event Code from Avamar API for "Client does not exist."
        public const int AvamarAlreadyMember = 22242;      // Event Code from Avamar API for "Client is already a member of group."
        public class AvamarGroupReturnType
        {
            public int GroupID { get; set; }
            public string GroupName { get; set; }
            public string GroupNameFQ { get; set; }
            public int Registered { get; set; }
        }
        public class AvamarGroupDayofWeek
        {
            public System.DayOfWeek Day { get; set; }
            public Dictionary<string, int> Hours = new Dictionary<string, int>();
            public int Total { get; set; }
            public void LoadHours(string hours)
            {
                Hours.Add("12AM", Int32.Parse(hours[0].ToString()));
                Hours.Add("1AM", Int32.Parse(hours[1].ToString()));
                Hours.Add("2AM", Int32.Parse(hours[2].ToString()));
                Hours.Add("3AM", Int32.Parse(hours[3].ToString()));
                Hours.Add("4AM", Int32.Parse(hours[4].ToString()));
                Hours.Add("5AM", Int32.Parse(hours[5].ToString()));
                Hours.Add("6AM", Int32.Parse(hours[6].ToString()));
                Hours.Add("7AM", Int32.Parse(hours[7].ToString()));
                Hours.Add("8AM", Int32.Parse(hours[8].ToString()));
                Hours.Add("9AM", Int32.Parse(hours[9].ToString()));
                Hours.Add("10AM", Int32.Parse(hours[10].ToString()));
                Hours.Add("11AM", Int32.Parse(hours[11].ToString()));
                Hours.Add("12PM", Int32.Parse(hours[12].ToString()));
                Hours.Add("1PM", Int32.Parse(hours[13].ToString()));
                Hours.Add("2PM", Int32.Parse(hours[14].ToString()));
                Hours.Add("3PM", Int32.Parse(hours[15].ToString()));
                Hours.Add("4PM", Int32.Parse(hours[16].ToString()));
                Hours.Add("5PM", Int32.Parse(hours[17].ToString()));
                Hours.Add("6PM", Int32.Parse(hours[18].ToString()));
                Hours.Add("7PM", Int32.Parse(hours[19].ToString()));
                Hours.Add("8PM", Int32.Parse(hours[20].ToString()));
                Hours.Add("9PM", Int32.Parse(hours[21].ToString()));
                Hours.Add("10PM", Int32.Parse(hours[22].ToString()));
                Hours.Add("11PM", Int32.Parse(hours[23].ToString()));
                foreach (KeyValuePair<string, int> hour in Hours)
                {
                    if (hour.Value == 1)
                        Total++;
                }

            }
        }
        public AvamarReturnType API(string xml)
        {
            AvamarReturnType api = new AvamarReturnType();
            if (!string.IsNullOrEmpty(xml) && xml.TrimStart().StartsWith("<"))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNodeList nodes = doc.SelectNodes("/CLIOutput/Results");
                if (nodes.Count > 0)
                {
                    api.Error = (nodes[0]["ReturnCode"].InnerText == "1");
                    api.Message = nodes[0]["EventSummary"].InnerText;
                    int code = 0;
                    Int32.TryParse(nodes[0]["EventCode"].InnerText, out code);
                    api.Code = code;
                    api.Nodes = doc.SelectNodes("/CLIOutput/Data/Row");
                }
                else
                {
                    api.Error = true;
                    api.Message = "There are no nodes in the return XML document";
                    api.Code = 0;
                }
            }
            else
            {
                api.Error = true;
                api.Message = xml;
            }
            return api;
        }
        private bool Exists(XmlNodeList xnList, string[] attributes, string value)
        {
            bool exists = false;
            foreach (XmlNode xn in xnList)
            {
                string value1 = "";
                foreach (string attribute in attributes)
                    value1 += xn[attribute].InnerText.Trim().ToUpper();
                string value2 = value.Trim().ToUpper();
                //if (value1.StartsWith("/") == false && value2.StartsWith("/") == true && value2.Length > 1)
                //    value2 = value2.Substring(1);
                if (value1 == value2)
                {
                    exists = true;
                    break;
                }
            }
            return exists;
        }

        private int GetOffset(int offset, DataRow drGroup)
        {
            bool bool1200AM = (drGroup["AM1200"].ToString() == "1");
            bool bool100AM = (drGroup["AM100"].ToString() == "1");
            bool bool200AM = (drGroup["AM200"].ToString() == "1");
            bool bool300AM = (drGroup["AM300"].ToString() == "1");
            bool bool400AM = (drGroup["AM400"].ToString() == "1");
            bool bool500AM = (drGroup["AM500"].ToString() == "1");
            bool bool600AM = (drGroup["AM600"].ToString() == "1");
            bool bool700AM = (drGroup["AM700"].ToString() == "1");
            bool bool800AM = (drGroup["AM800"].ToString() == "1");
            bool bool900AM = (drGroup["AM900"].ToString() == "1");
            bool bool1000AM = (drGroup["AM1000"].ToString() == "1");
            bool bool1100AM = (drGroup["AM1100"].ToString() == "1");
            bool bool1200PM = (drGroup["PM1200"].ToString() == "1");
            bool bool100PM = (drGroup["PM100"].ToString() == "1");
            bool bool200PM = (drGroup["PM200"].ToString() == "1");
            bool bool300PM = (drGroup["PM300"].ToString() == "1");
            bool bool400PM = (drGroup["PM400"].ToString() == "1");
            bool bool500PM = (drGroup["PM500"].ToString() == "1");
            bool bool600PM = (drGroup["PM600"].ToString() == "1");
            bool bool700PM = (drGroup["PM700"].ToString() == "1");
            bool bool800PM = (drGroup["PM800"].ToString() == "1");
            bool bool900PM = (drGroup["PM900"].ToString() == "1");
            bool bool1000PM = (drGroup["PM1000"].ToString() == "1");
            bool bool1100PM = (drGroup["PM1100"].ToString() == "1");

            int difference = 999;
            if (bool1200AM) difference = GetDiff(-12, offset, difference);
            if (bool100AM) difference = GetDiff(-11, offset, difference);
            if (bool200AM) difference = GetDiff(-10, offset, difference);
            if (bool300AM) difference = GetDiff(-9, offset, difference);
            if (bool400AM) difference = GetDiff(-8, offset, difference);
            if (bool500AM) difference = GetDiff(-7, offset, difference);
            if (bool600AM) difference = GetDiff(-6, offset, difference);
            if (bool700AM) difference = GetDiff(-5, offset, difference);
            if (bool800AM) difference = GetDiff(-4, offset, difference);
            if (bool900AM) difference = GetDiff(-3, offset, difference);
            if (bool1000AM) difference = GetDiff(-2, offset, difference);
            if (bool1100AM) difference = GetDiff(-1, offset, difference);
            if (bool1200PM) difference = GetDiff(0, offset, difference);
            if (bool100PM) difference = GetDiff(1, offset, difference);
            if (bool200PM) difference = GetDiff(2, offset, difference);
            if (bool300PM) difference = GetDiff(3, offset, difference);
            if (bool400PM) difference = GetDiff(4, offset, difference);
            if (bool500PM) difference = GetDiff(5, offset, difference);
            if (bool600PM) difference = GetDiff(6, offset, difference);
            if (bool700PM) difference = GetDiff(7, offset, difference);
            if (bool800PM) difference = GetDiff(8, offset, difference);
            if (bool900PM) difference = GetDiff(9, offset, difference);
            if (bool1000PM) difference = GetDiff(10, offset, difference);
            if (bool1100PM) difference = GetDiff(11, offset, difference);
            return difference;
        }
        private int GetDiff(int multiplier, int offset, int difference)
        {
            int temp = multiplier - offset;
            temp = Math.Abs(temp);
            if (temp < difference)
                difference = temp;
            return difference;
        }

        private AvamarGroupReturnType GetGroup(DataSet dsGroups, XmlNodeList groups, string strFrequency, List<AvamarGroupDayofWeek> SelectedDays, int intAnswer, string strName, Log oLog, Avamar oAvamar, int EnvironmentID, int intSelectedGrid, string strSelectedGrid, ClearViewWebServices oWebService)
        {
            Users oUser = new Users(user, dsn);
            Platforms oPlatform = new Platforms(user, dsn);
            Functions oFunction = new Functions(user, dsn, EnvironmentID);
            AvamarGroupReturnType ReturnType = new AvamarGroupReturnType();
            ReturnType.GroupID = 0;

            int GroupOffset = 999;
            foreach (DataRow drGroup in dsGroups.Tables[0].Rows)
            {
                int intGroup = Int32.Parse(drGroup["id"].ToString());
                string strDomain = drGroup["FQDN"].ToString();
                string strGroup = drGroup["name"].ToString();
                if (Exists(groups, new string[] { "Group" }, strGroup))
                {
                    int Daily = Int32.Parse(drGroup["daily"].ToString());
                    int Weekly = Int32.Parse(drGroup["weekly"].ToString());
                    StringBuilder WeeklyDays = new StringBuilder();
                    int WeeklySunday = Int32.Parse(drGroup["sunday"].ToString());
                    if (WeeklySunday == 1)
                        WeeklyDays.Append("Su");
                    int WeeklyMonday = Int32.Parse(drGroup["monday"].ToString());
                    if (WeeklyMonday == 1)
                        WeeklyDays.Append("Mo");
                    int WeeklyTuesday = Int32.Parse(drGroup["tuesday"].ToString());
                    if (WeeklyTuesday == 1)
                        WeeklyDays.Append("Tu");
                    int WeeklyWednesday = Int32.Parse(drGroup["wednesday"].ToString());
                    if (WeeklyWednesday == 1)
                        WeeklyDays.Append("We");
                    int WeeklyThursday = Int32.Parse(drGroup["thursday"].ToString());
                    if (WeeklyThursday == 1)
                        WeeklyDays.Append("Th");
                    int WeeklyFriday = Int32.Parse(drGroup["friday"].ToString());
                    if (WeeklyFriday == 1)
                        WeeklyDays.Append("Fr");
                    int WeeklySaturday = Int32.Parse(drGroup["saturday"].ToString());
                    if (WeeklySaturday == 1)
                        WeeklyDays.Append("Sa");
                    int Monthly = Int32.Parse(drGroup["monthly"].ToString());
                    int DayOfMonth = Int32.Parse(drGroup["day"].ToString());

                    int intThreshold = Int32.Parse(drGroup["threshold"].ToString());
                    int intMaximum = Int32.Parse(drGroup["maximum"].ToString());
                    int intRegistered = Int32.Parse(drGroup["registered"].ToString());

                    if ((strFrequency == "D" && Daily == 1)
                        || (strFrequency == "W" && Weekly == 1)
                        || (strFrequency == "M" && Monthly == 1))
                    {
                        bool weeklyOK = false;
                        if (Weekly == 1)
                        {
                            // Additional validation on day of week.
                            if (WeeklySunday == 1 && SelectedDays[0].Total > 0)
                                weeklyOK = true;
                            else if (WeeklyMonday == 1 && SelectedDays[1].Total > 0)
                                weeklyOK = true;
                            else if (WeeklyTuesday == 1 && SelectedDays[2].Total > 0)
                                weeklyOK = true;
                            else if (WeeklyWednesday == 1 && SelectedDays[3].Total > 0)
                                weeklyOK = true;
                            else if (WeeklyThursday == 1 && SelectedDays[4].Total > 0)
                                weeklyOK = true;
                            else if (WeeklyFriday == 1 && SelectedDays[5].Total > 0)
                                weeklyOK = true;
                            else if (WeeklySaturday == 1 && SelectedDays[6].Total > 0)
                                weeklyOK = true;
                        }
                        else
                            weeklyOK = true;

                        if (weeklyOK)
                        {
                            AvamarReturnType registered = API(oWebService.GetAvamarGroupClients(strSelectedGrid, strDomain + strGroup));
                            if (registered.Error == false)
                            {
                                int intAlready = registered.Nodes.Count;
                                int intDifference = (intMaximum - intAlready);
                                AddResult(intAnswer, strName, oLog, "...." + strGroup + " = " + intDifference.ToString() + " available (" + intAlready.ToString() + " registered)");
                                // 1) Check that there is capacity
                                if (intDifference > 0)
                                {
                                    AddResult(intAnswer, strName, oLog, "...." + strGroup + " = there is capacity...");
                                    // Check threshold
                                    if (intThreshold > 0 && intAlready > intThreshold)
                                    {
                                        DataSet dsOwners = oPlatform.GetUsers(17);
                                        foreach (DataRow drOwner in dsOwners.Tables[0].Rows)
                                        {
                                            // Email owners
                                            oFunction.SendEmail("Threshold: " + strGroup, oUser.GetName(Int32.Parse(drOwner["userid"].ToString())), "", "", "Threshold: " + strGroup, "<p><b>This message is to inform you that a threshold has been breached on the following group:</b></p><p>Group: " + strGroup + "</p><p>Threshold: " + intThreshold.ToString() + "</p><p>Registered: " + intAlready.ToString() + "</p><p>Login to ClearView and click on Inventory Manager to change thresholds or increase capacity.</p>", true, false);
                                        }
                                        oAvamar.AddLog(intSelectedGrid, intGroup, 0, "Has breached the threshold value of " + intThreshold.ToString() + "(value = " + intAlready.ToString() + ")");
                                    }
                                    else
                                        AddResult(intAnswer, strName, oLog, "...." + strGroup + " = not at threshold...");

                                    int MinimumOffset = 999;
                                    // Loop through each selected day.
                                    for (int ii = 0; ii < 7; ii++)
                                    {
                                        // Check to make sure it's either Daily, Monthly or the selected day.
                                        if (Weekly == 0
                                            || (WeeklySunday == 1 && ii == 0)
                                            || (WeeklyMonday == 1 && ii == 1)
                                            || (WeeklyTuesday == 1 && ii == 2)
                                            || (WeeklyWednesday == 1 && ii == 3)
                                            || (WeeklyThursday == 1 && ii == 4)
                                            || (WeeklyFriday == 1 && ii == 5)
                                            || (WeeklySaturday == 1 && ii == 6)
                                        )
                                        {
                                            // Loop through all selections for the current day.
                                            foreach (KeyValuePair<string, int> hour in SelectedDays[ii].Hours)
                                            {
                                                if (hour.Value == 1)
                                                {
                                                    int Offset = 0;
                                                    if (hour.Key == "12AM") Offset = -12;
                                                    else if (hour.Key == "1AM") Offset = -11;
                                                    else if (hour.Key == "2AM") Offset = -10;
                                                    else if (hour.Key == "3AM") Offset = -9;
                                                    else if (hour.Key == "4AM") Offset = -8;
                                                    else if (hour.Key == "5AM") Offset = -7;
                                                    else if (hour.Key == "6AM") Offset = -6;
                                                    else if (hour.Key == "7AM") Offset = -5;
                                                    else if (hour.Key == "8AM") Offset = -4;
                                                    else if (hour.Key == "9AM") Offset = -3;
                                                    else if (hour.Key == "10AM") Offset = -2;
                                                    else if (hour.Key == "11AM") Offset = -1;
                                                    else if (hour.Key == "12PM") Offset = 0;
                                                    else if (hour.Key == "1PM") Offset = 1;
                                                    else if (hour.Key == "2PM") Offset = 2;
                                                    else if (hour.Key == "3PM") Offset = 3;
                                                    else if (hour.Key == "4PM") Offset = 4;
                                                    else if (hour.Key == "5PM") Offset = 5;
                                                    else if (hour.Key == "6PM") Offset = 6;
                                                    else if (hour.Key == "7PM") Offset = 7;
                                                    else if (hour.Key == "8PM") Offset = 8;
                                                    else if (hour.Key == "9PM") Offset = 9;
                                                    else if (hour.Key == "10PM") Offset = 10;
                                                    else if (hour.Key == "11PM") Offset = 11;
                                                    int DiffOffSet = GetOffset(Offset, drGroup);
                                                    // If selected, add offset.
                                                    if (DiffOffSet < MinimumOffset)
                                                    {
                                                        MinimumOffset = DiffOffSet;
                                                        if (MinimumOffset == 0) // Not going to find any better than the exact start time.
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        if (MinimumOffset == 0) // Not going to find any better than the exact start time.
                                            break;
                                    }

                                    AddResult(intAnswer, strName, oLog, "...." + strGroup + " = Offset (" + MinimumOffset.ToString() + " < " + GroupOffset.ToString() + "...");
                                    if (MinimumOffset < GroupOffset)
                                    {
                                        GroupOffset = MinimumOffset;
                                        ReturnType.GroupID = intGroup;
                                        ReturnType.GroupName = strGroup;
                                        ReturnType.Registered = intRegistered;
                                        if (strDomain == "/")
                                            ReturnType.GroupNameFQ = strDomain + strGroup;
                                        else
                                            ReturnType.GroupNameFQ = strDomain + "/" + strGroup;
                                        if (GroupOffset == 0) // Not going to find any better than the exact start time on a selected day.
                                        {
                                            AddResult(intAnswer, strName, oLog, "...." + strGroup + " is the selected cluster group (exact match)");
                                            break;
                                        }
                                        else
                                            AddResult(intAnswer, strName, oLog, "...." + strGroup + " is now the selected cluster group (" + MinimumOffset.ToString() + " HR offset)");
                                    }
                                }
                                else
                                {
                                    string message = "At maximum (" + intMaximum.ToString() + ")";
                                    oAvamar.AddLog(intSelectedGrid, intGroup, 0, message);
                                    AddResult(intAnswer, strName, oLog, "...." + strGroup + " = " + message);
                                }
                            }
                            else
                            {
                                string message = "ERROR: " + registered.Message + " (" + registered.Code + ")";
                                oAvamar.AddLog(intSelectedGrid, intGroup, 0, message);
                                AddResult(intAnswer, strName, oLog, "...." + strGroup + " = " + message);
                            }
                        }
                        else
                        {
                            string message = "Not configured for the day(s) requested (" + WeeklyDays.ToString() + ")";
                            oAvamar.AddLog(intSelectedGrid, intGroup, 0, message);
                            AddResult(intAnswer, strName, oLog, "...." + strGroup + " = " + message);
                        }
                    }
                    else
                    {
                        string message = "Frequency does not match (" + strFrequency + ")";
                        //oAvamar.AddLog(intSelectedGrid, intGroup, 0, message);
                        AddResult(intAnswer, strName, oLog, "...." + strGroup + " = " + message);
                    }
                }
                else
                {
                    string message = "Not found on the grid (" + strSelectedGrid + ")";
                    oAvamar.AddLog(intSelectedGrid, intGroup, 0, message);
                    AddResult(intAnswer, strName, oLog, "...." + strGroup + " = " + message);
                }
            }

            return ReturnType;
        }

        public void Registrations(int EnvironmentID, string strScripts, string dsnAsset, string dsnServiceEditor, string dsnIP, int ViewPage, int AssignPage)
        {
            // Setup Classes
            Servers oServer = new Servers(0, dsn);
            Mnemonic oMnemonic = new Mnemonic(0, dsn);
            Design oDesign = new Design(0, dsn);
            Log oLog = new Log(0, dsn);

            DataSet dsRegistrations = oServer.GetAvamarRegistrations();
            if (dsRegistrations.Tables[0].Rows.Count > 0)
            {
                oLog.AddEvent("", "", "Get avamar registrations (" + dsRegistrations.Tables[0].Rows.Count.ToString() + ")", LoggingType.Debug);
                foreach (DataRow drRegistration in dsRegistrations.Tables[0].Rows)
                {
                    ClearResults();

                    int intServer = Int32.Parse(drRegistration["id"].ToString());
                    bool boolCluster = (drRegistration["clusterid"].ToString() != "0");
                    int intDomainEnvironment = Int32.Parse(drRegistration["environment"].ToString());
                    oServer.UpdateAvamarRegistrationStarted(intServer, DateTime.Now.ToString());
                    int intAnswer = Int32.Parse(drRegistration["answerid"].ToString());
                    int intMnemonicID = Int32.Parse(drRegistration["mnemonicid"].ToString());
                    int intDesign = Int32.Parse(drRegistration["designid"].ToString());
                    int intDR = Int32.Parse(drRegistration["dr"].ToString());
                    bool boolOnsite = (drRegistration["onsite"].ToString() == "1");
                    string strFrequency = drRegistration["backup_frequency"].ToString();
                    string strName = drRegistration["servername"].ToString();
                    string strIP = drRegistration["ipaddress"].ToString();
                    int intAddress = Int32.Parse(drRegistration["addressid"].ToString());
                    int intClass = Int32.Parse(drRegistration["classid"].ToString());
                    int intEnvironment = Int32.Parse(drRegistration["environmentid"].ToString());
                    int intHRs = oMnemonic.GetResRatingHRs(intMnemonicID);
                    if (intDR == 1) // use the flag from design builder to dictate recovery
                        intHRs = 6;
                    string strError = "";

                    // Load backups for design
                    List<AvamarGroupDayofWeek> SelectedDays = new List<AvamarGroupDayofWeek>();
                    DataSet dsBackup = oDesign.GetBackup(intDesign);
                    if (dsBackup.Tables[0].Rows.Count > 0)
                    {
                        DataRow drBackup = dsBackup.Tables[0].Rows[0];
                        // Sunday
                        AvamarGroupDayofWeek Sunday = new AvamarGroupDayofWeek();
                        Sunday.Day = System.DayOfWeek.Sunday;
                        Sunday.LoadHours(drBackup["sun"].ToString());
                        SelectedDays.Add(Sunday);
                        // Monday
                        AvamarGroupDayofWeek Monday = new AvamarGroupDayofWeek();
                        Monday.Day = System.DayOfWeek.Monday;
                        Monday.LoadHours(drBackup["mon"].ToString());
                        SelectedDays.Add(Monday);
                        // Tuesday
                        AvamarGroupDayofWeek Tuesday = new AvamarGroupDayofWeek();
                        Tuesday.Day = System.DayOfWeek.Tuesday;
                        Tuesday.LoadHours(drBackup["tue"].ToString());
                        SelectedDays.Add(Tuesday);
                        // Wednesday
                        AvamarGroupDayofWeek Wednesday = new AvamarGroupDayofWeek();
                        Wednesday.Day = System.DayOfWeek.Wednesday;
                        Wednesday.LoadHours(drBackup["wed"].ToString());
                        SelectedDays.Add(Wednesday);
                        // Thursday
                        AvamarGroupDayofWeek Thursday = new AvamarGroupDayofWeek();
                        Thursday.Day = System.DayOfWeek.Thursday;
                        Thursday.LoadHours(drBackup["thu"].ToString());
                        SelectedDays.Add(Thursday);
                        // Friday
                        AvamarGroupDayofWeek Friday = new AvamarGroupDayofWeek();
                        Friday.Day = System.DayOfWeek.Friday;
                        Friday.LoadHours(drBackup["fri"].ToString());
                        SelectedDays.Add(Friday);
                        // Saturday
                        AvamarGroupDayofWeek Saturday = new AvamarGroupDayofWeek();
                        Saturday.Day = System.DayOfWeek.Saturday;
                        Saturday.LoadHours(drBackup["sat"].ToString());
                        SelectedDays.Add(Saturday);

                        strError = Register(EnvironmentID, intServer, boolCluster, intDomainEnvironment, intAnswer, intMnemonicID, SelectedDays, intHRs, strFrequency, strName, strIP, intAddress, intClass, intEnvironment, boolOnsite, strScripts, dsnAsset, dsnServiceEditor, dsnIP, ViewPage, AssignPage);
                    }
                    else
                        strError = "Missing design backup information";

                    if (strError != "")
                    {
                        oLog.AddEvent(intAnswer, strName, "", strError, LoggingType.Error);
                        oServer.UpdateAvamarRegistrationCompleted(intServer, 0, 0, "", strError, DateTime.Now.ToString(), 1);
                        oServer.AddError(0, 0, 0, intServer, 906, strError);
                    }
                }
            }
        }

        public string Register(int EnvironmentID, int intServer, bool boolCluster, int intDomainEnvironment, int intAnswer, int intMnemonicID, List<AvamarGroupDayofWeek> SelectedDays, int intHRs, string strFrequency, string strName, string strIP, int intAddress, int intClass, int intEnvironment, bool boolOnsite, string strScripts, string dsnAsset, string dsnServiceEditor, string dsnIP, int ViewPage, int AssignPage)
        {
            // Setup Classes
            Users oUser = new Users(user, dsn);
            Platforms oPlatform = new Platforms(user, dsn);
            Functions oFunction = new Functions(user, dsn, EnvironmentID);
            Servers oServer = new Servers(0, dsn);
            NCC.ClearView.Application.Core.Avamar oAvamar = new NCC.ClearView.Application.Core.Avamar(0, dsn);
            Log oLog = new Log(0, dsn);
            Variables oVariable = new Variables(EnvironmentID);
            Variables oDomainVariable = new Variables(intDomainEnvironment);

            string strError = "";
            try
            {
                // Setup Webservice for querying via SSH
                System.Net.NetworkCredential oCredentialsDNS = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                ClearViewWebServices oWebService = new ClearViewWebServices();
                oWebService.Url = oVariable.WebServiceURL();

                oLog.AddEvent(intAnswer, strName, "", "Starting automated Avamar registration", LoggingType.Information);
                string strSelectedGrid = "";
                int intSelectedGrid = 0;
                int intSelectedGridRegistered = 0;
                double dblGridUtilization = 100.00;
                int intGridMnemonics = 0;
                // First, query grids based on location, class and environment
                DataSet dsGrids = oAvamar.GetGridQuery(intAddress, intClass, intEnvironment, intMnemonicID, intServer);
                AddResult(intAnswer, strName, oLog, "There are " + dsGrids.Tables[0].Rows.Count.ToString() + " grids for CLASSID = " + intClass.ToString() + ", ENVIRONMENTID = " + intEnvironment.ToString() + ", ADDRESSID = " + intAddress.ToString() + ", MNEMONIC = " + intMnemonicID.ToString());
                foreach (DataRow drGrid in dsGrids.Tables[0].Rows)
                {
                    int intGrid = Int32.Parse(drGrid["id"].ToString());
                    string strGrid = drGrid["name"].ToString();
                    double dblThreshold = double.Parse(drGrid["threshold"].ToString());
                    double dblMaximum = double.Parse(drGrid["maximum"].ToString());
                    int intRegistered = Int32.Parse(drGrid["registered"].ToString());
                    int intMnemonics = Int32.Parse(drGrid["mnemonic"].ToString());

                    oLog.AddEvent(intAnswer, strName, "", "Checking grid " + strGrid, LoggingType.Debug);

                    // 1) Check that there is capacity
                    AvamarReturnType grid = API(oWebService.GetAvamarGrid(strGrid));
                    if (grid.Error == false)
                    {
                        string utilization = "";
                        foreach (XmlNode xn in grid.Nodes)
                        {
                            if (xn["Attribute"].InnerText == "Server utilization")
                            {
                                utilization = xn["Value"].InnerText;
                                break;
                            }
                        }
                        if (string.IsNullOrEmpty(utilization) == false)
                        {
                            AddResult(intAnswer, strName, oLog, ".." + strGrid + " = " + utilization + " utilization");
                            double dblUtilization = double.Parse(utilization.Replace("%", ""));
                            if (dblUtilization < dblMaximum)
                            {
                                // Check threshold
                                if (dblThreshold > 0.00 && dblUtilization >= dblThreshold)
                                {
                                    // Email owners
                                    DataSet dsOwners = oPlatform.GetUsers(17);
                                    foreach (DataRow drOwner in dsOwners.Tables[0].Rows)
                                    {
                                        // Email owners
                                        oFunction.SendEmail("Threshold: " + strGrid, oUser.GetName(Int32.Parse(drOwner["userid"].ToString())), "", "", "Threshold: " + strGrid, "<p><b>This message is to inform you that a threshold has been breached on the following grid:</b></p><p>Grid: " + strGrid + "</p><p>Threshold: " + dblThreshold.ToString("0") + "%</p><p>Utilization: " + dblUtilization.ToString("0") + "%</p><p>Login to ClearView and click on Inventory Manager to change thresholds or increase capacity.</p>", true, false);
                                    }
                                    oAvamar.AddLog(intGrid, 0, 0, "Has breached the threshold value of " + dblThreshold.ToString("0") + "(value = " + dblUtilization.ToString("0") + ")");
                                }

                                // 2) Check for maximum utilization
                                if (dblUtilization < dblGridUtilization)
                                {
                                    dblGridUtilization = dblUtilization;
                                    intSelectedGrid = intGrid;
                                    strSelectedGrid = strGrid;
                                    AddResult(intAnswer, strName, oLog, ".." + strGrid + " is now the selected grid (utilization)");
                                }
                                else if (dblUtilization == dblGridUtilization)
                                {
                                    // 3) Check for maximum mnemonics
                                    if (intMnemonics > intGridMnemonics)
                                    {
                                        intGridMnemonics = intMnemonics;
                                        intSelectedGrid = intGrid;
                                        strSelectedGrid = strGrid;
                                        intSelectedGridRegistered = intRegistered;
                                        AddResult(intAnswer, strName, oLog, ".." + strGrid + " is now the selected grid (mnemonics)");
                                    }
                                    else
                                        AddResult(intAnswer, strName, oLog, ".." + strGrid + " has less than other mnemonics (" + intMnemonics.ToString() + ")...skipping...");
                                }
                                else
                                    AddResult(intAnswer, strName, oLog, ".." + strGrid + " utilization is less than other grids...skipping...");
                            }
                            else
                            {
                                string message = "Has reached maximum capacity (" + dblMaximum.ToString("0") + "%)";
                                oAvamar.AddLog(intGrid, 0, 0, message);
                                AddResult(intAnswer, strName, oLog, ".." + strGrid + " = " + message);
                            }
                        }
                        else
                        {
                            string message = "Utilization could not be queried";
                            oAvamar.AddLog(intGrid, 0, 0, message);
                            AddResult(intAnswer, strName, oLog, ".." + strGrid + " = " + message);
                        }
                    }
                    else
                    {
                        strError = grid.Message + " (" + grid.Code + ")";
                        oAvamar.AddLog(intGrid, 0, 0, strError);
                        AddResult(intAnswer, strName, oLog, ".." + strGrid + " = " + strError);
                        break;
                    }
                }

                if (strError == "")
                {
                    if (intSelectedGrid > 0)
                    {
                        // Second, select the domains.
                        string strSelectedDomain = "";
                        string strSelectedDomainFQ = "";
                        int intSelectedDomain = 0;
                        int intSelectedDomainRegistered = 0;
                        // Load domains from grid
                        AvamarReturnType domains = API(oWebService.GetAvamarDomains(strSelectedGrid));
                        if (domains.Error == false)
                        {
                            // Load domains from internal inventory
                            DataSet dsDomains = oAvamar.GetDomainQuery(intSelectedGrid, intServer, intHRs);
                            AddResult(intAnswer, strName, oLog, "..There are " + dsDomains.Tables[0].Rows.Count.ToString() + " domains for GRIDID = " + intSelectedGrid.ToString() + " (" + strSelectedGrid + "), SERVERID = " + intServer.ToString() + ", HOURS = " + intHRs.ToString());
                            foreach (DataRow drDomain in dsDomains.Tables[0].Rows)
                            {
                                int intDomain = Int32.Parse(drDomain["id"].ToString());
                                string strDomain = drDomain["name"].ToString();
                                string strFQDN = drDomain["FQDN"].ToString();
                                if (String.IsNullOrEmpty(strDomain) || Exists(domains.Nodes, new string[] { "Domain", "Name" }, strFQDN))
                                {
                                    bool boolCatchAll = (drDomain["catchall"].ToString() == "1");
                                    int intRegistered = Int32.Parse(drDomain["registered"].ToString());

                                    if (boolCatchAll == false)
                                    {
                                        intSelectedDomain = intDomain;
                                        intSelectedDomainRegistered = intRegistered;
                                        strSelectedDomain = strDomain;
                                        strSelectedDomainFQ = strFQDN;
                                        AddResult(intAnswer, strName, oLog, "...." + strFQDN + " is now the selected domain (application)");
                                        // Since application takes precendence over catch-all, if catch-all is false, then it's because the application matched so end the loop immediately
                                        break;
                                    }
                                    else
                                    {
                                        intSelectedDomain = intDomain;
                                        intSelectedDomainRegistered = intRegistered;
                                        strSelectedDomain = strDomain;
                                        strSelectedDomainFQ = strFQDN;
                                        AddResult(intAnswer, strName, oLog, "...." + strFQDN + " is now the selected domain (catch-all)");
                                    }
                                }
                                else
                                {
                                    string message = "Not found on the grid (" + strSelectedGrid + ")";
                                    oAvamar.AddLog(intSelectedGrid, 0, intDomain, message);
                                    AddResult(intAnswer, strName, oLog, "...." + strFQDN + " = " + message);
                                }
                            }

                            if (intSelectedDomain > 0)
                            {
                                // Third, select the group.
                                string strSelectedGroup = "";
                                string strSelectedGroupFQ = "";
                                int intSelectedGroup = 0;
                                int intSelectedGroupRegistered = 0;
                                // Load groups from grid
                                AvamarReturnType groups = API(oWebService.GetAvamarGroups(strSelectedGrid));
                                if (groups.Error == false)
                                {
                                    // Load groups from internal inventory
                                    DataSet dsGroups = oAvamar.GetGroupQuery(intSelectedGrid, intServer, 0);
                                    AddResult(intAnswer, strName, oLog, "..There are " + dsGroups.Tables[0].Rows.Count.ToString() + " groups for GRIDID = " + intSelectedGrid.ToString() + " (" + strSelectedGrid + "), SERVERID = " + intServer.ToString());
                                    AvamarGroupReturnType SelectedGroup = GetGroup(dsGroups, groups.Nodes, strFrequency, SelectedDays, intAnswer, strName, oLog, oAvamar, EnvironmentID, intSelectedGrid, strSelectedGrid, oWebService);
                                    intSelectedGroup = SelectedGroup.GroupID;
                                    intSelectedGroupRegistered = SelectedGroup.Registered;
                                    strSelectedGroup = SelectedGroup.GroupName;
                                    strSelectedGroupFQ = SelectedGroup.GroupNameFQ;

                                    if (intSelectedGroup > 0)
                                    {
                                        // Fourth, select the cluster group.
                                        string strSelectedClusterGroup = "";
                                        string strSelectedClusterGroupFQ = "";
                                        int intSelectedClusterGroup = 0;
                                        int intSelectedClusterGroupRegistered = 0;
                                        if (boolCluster)
                                        {
                                            DataSet dsCluster = oAvamar.GetGroupQuery(intSelectedGrid, intServer, 1);
                                            AddResult(intAnswer, strName, oLog, "..There are " + dsCluster.Tables[0].Rows.Count.ToString() + " groups for GRIDID = " + intSelectedGrid.ToString() + ", SERVERID = " + intServer.ToString() + ", CLUSTERING = 1");
                                            AvamarGroupReturnType SelectedClusterGroup = GetGroup(dsGroups, groups.Nodes, strFrequency, SelectedDays, intAnswer, strName, oLog, oAvamar, EnvironmentID, intSelectedGrid, strSelectedGrid, oWebService);
                                            intSelectedClusterGroup = SelectedClusterGroup.GroupID;
                                            intSelectedClusterGroupRegistered = SelectedClusterGroup.Registered;
                                            strSelectedClusterGroup = SelectedClusterGroup.GroupName;
                                            strSelectedClusterGroupFQ = SelectedClusterGroup.GroupNameFQ;
                                        }
                                        else
                                            AddResult(intAnswer, strName, oLog, "....clustering group is not required.");

                                        if (intSelectedClusterGroup > 0 || boolCluster == false)
                                        {
                                            if (boolOnsite == false && strName.StartsWith("W") == false)
                                            {
                                                boolOnsite = true;  // if not windows, cannot run script so act like it is onsite.
                                                oLog.AddEvent(intAnswer, strName, "", "Even though this is an offsite server, it's not Windows, so we have to act like it's onsite", LoggingType.Information);
                                            }
                                            if (boolOnsite == false)
                                            {
                                                oLog.AddEvent(intAnswer, strName, "", "Since this is an offsite server, we need to schedule a task for activation.", LoggingType.Information);

                                                string strAdminUser = oDomainVariable.Domain() + "\\" + oDomainVariable.ADUser();
                                                string strAdminPass = oDomainVariable.ADPassword();

                                                oLog.AddEvent(intAnswer, strName, "", "Creating files in " + strScripts, LoggingType.Debug);

                                                // Create batch file on remote server (will call VBS)
                                                string strBatchRemote = strScripts + strName + ".bat";
                                                StreamWriter oWriterRemote = new StreamWriter(strBatchRemote);
                                                oWriterRemote.WriteLine("@ECHO OFF");
                                                oWriterRemote.WriteLine("wscript.exe \"C:\\OPTIONS\\AvamarOffsite.vbs\" \"" + strSelectedGrid + "\"");
                                                oWriterRemote.WriteLine("echo Return code = %ErrorLevel%");
                                                oWriterRemote.WriteLine("IF %ERRORLEVEL% EQU 0 (");
                                                oWriterRemote.WriteLine("    REM **No error found**");
                                                oWriterRemote.WriteLine("    DEL \"C:\\OPTIONS\\AvamarOffsite.*\"");
                                                oWriterRemote.WriteLine("    SCHTASKS /Delete /TN ActivateAvamar /F");
                                                oWriterRemote.WriteLine(") ELSE (");
                                                oWriterRemote.WriteLine("    REM **An error was found**");
                                                oWriterRemote.WriteLine(")");
                                                oWriterRemote.Flush();
                                                oWriterRemote.Close();
                                                oLog.AddEvent(intAnswer, strName, "", "Created VBS to activate to " + strSelectedGrid, LoggingType.Debug);

                                                // Create batch file to copy VBS to the remote server.
                                                string strBatchLocal = strScripts + strName + "_copy.bat";
                                                StreamWriter oWriterLocal = new StreamWriter(strBatchLocal);
                                                oWriterLocal.WriteLine("net use \\\\" + strIP + "\\C$ /user:" + strAdminUser + " " + strAdminPass + "");
                                                oWriterLocal.WriteLine("mkdir \\\\" + strIP + "\\C$\\OPTIONS");
                                                oWriterLocal.WriteLine("copy " + strBatchRemote + " \\\\" + strIP + "\\C$\\OPTIONS\\AvamarOffsite.bat");
                                                oWriterLocal.WriteLine("copy " + oVariable.DocumentsFolder() + "scripts\\AvamarOffsite.vbs \\\\" + strIP + "\\C$\\OPTIONS\\AvamarOffsite.vbs");
                                                oWriterLocal.Flush();
                                                oWriterLocal.Close();
                                                oLog.AddEvent(intAnswer, strName, "", "Created BAT file to copy VBS to " + strIP, LoggingType.Debug);

                                                // Copy VBS to remote server.
                                                string strFileOut = strScripts + strName + ".txt";
                                                ProcessStartInfo infoCopy = new ProcessStartInfo(strScripts + "psexec");
                                                infoCopy.WorkingDirectory = strScripts;
                                                infoCopy.Arguments = "-h cmd.exe /c " + strBatchLocal + " > " + strFileOut;
                                                bool boolTimeout = false;
                                                Process procCopy = Process.Start(infoCopy);
                                                int intTimeout = (1 * 60 * 1000);    // 1 minute
                                                procCopy.WaitForExit(intTimeout);
                                                if (procCopy.HasExited == false)
                                                {
                                                    procCopy.Kill();
                                                    boolTimeout = true;
                                                }
                                                procCopy.Close();
                                                if (boolTimeout == false)
                                                {
                                                    oLog.AddEvent(intAnswer, strName, "", "Files have been copied to " + strIP + " - create scheduled task.", LoggingType.Debug);

                                                    // Create a scheduled task to run automatically on the server.  Do this first since you can always overwrite files.
                                                    string strBatchTask = strScripts + strName + "_task.bat";
                                                    StreamWriter oWriterTask = new StreamWriter(strBatchTask);
                                                    oWriterTask.WriteLine("schtasks /CREATE /TN ActivateAvamar /RU \"SYSTEM\" /RL HIGHEST /TR \"C:\\OPTIONS\\AvamarOffsite.bat\" /ST 06:30 /SC hourly /MO 1 /NP /F");
                                                    oWriterTask.Flush();
                                                    oWriterTask.Close();

                                                    // Execute / create scheduled task
                                                    Functions oFunctionDomain = new Functions(0, dsn, intDomainEnvironment);
                                                    int intTaskReturn = oFunctionDomain.ExecuteVBScript(intServer, false, true, "Avamar Task", strName, "", strIP, strBatchTask, strScripts + strName, "AVAMAR", "%windir%\\system32\\cmd.exe /c", "OPTIONS\\CV_AVAMAR_TASK", "BAT", "", strScripts, strAdminUser, strAdminPass, 1, false, false, 0, false);
                                                    oLog.AddEvent(intAnswer, strName, "", "Scheduled task has been created (" + intTaskReturn.ToString() + ")", LoggingType.Information);

                                                }
                                                else
                                                    strError = "A timeout occurred while trying to copy the avamar activation script to the server";
                                            }

                                            if (strError == "")
                                            {
                                                oLog.AddEvent(intAnswer, strName, "", "Executing registration scripts...please wait...", LoggingType.Information);
                                                // Found all, now register.
                                                string strClient = strName + "." + oDomainVariable.FullyQualified();

                                                try
                                                {
                                                    // Add client to domain
                                                    AvamarReturnType client = API(oWebService.AddAvamarClient(strSelectedGrid, strSelectedDomainFQ, strClient, strIP));
                                                    if (client.Error == false || client.Code == AvamarAlreadyRegistered)
                                                    {
                                                        if (client.Code == AvamarAlreadyRegistered)
                                                            oLog.AddEvent(intAnswer, strName, "", "Client was already registered.", LoggingType.Information);
                                                        // Add client to group(s)
                                                        AvamarReturnType group = API(oWebService.AddAvamarGroup(strSelectedGrid, strSelectedDomainFQ, strClient, strSelectedGroupFQ));
                                                        if (group.Error == false || group.Code == AvamarAlreadyMember)
                                                        {
                                                            if (group.Code == AvamarAlreadyMember)
                                                                oLog.AddEvent(intAnswer, strName, "", "Group # 1 was already added.", LoggingType.Information);
                                                            else
                                                                oLog.AddEvent(intAnswer, strName, "", "Group # 1 has been added.", LoggingType.Information);
                                                            bool ok = false;
                                                            string strClusterGroupMessage = "";
                                                            if (intSelectedClusterGroup > 0)
                                                            {
                                                                AvamarReturnType cluster = API(oWebService.AddAvamarGroup(strSelectedGrid, strSelectedDomainFQ, strClient, strSelectedClusterGroupFQ));
                                                                if (cluster.Error == false || cluster.Code == AvamarAlreadyMember)
                                                                {
                                                                    if (cluster.Code == AvamarAlreadyMember)
                                                                        oLog.AddEvent(intAnswer, strName, "", "Group # 2 was already added.", LoggingType.Information);
                                                                    else
                                                                        oLog.AddEvent(intAnswer, strName, "", "Group # 2 has been added.", LoggingType.Information);
                                                                    ok = true;
                                                                    strClusterGroupMessage = cluster.Message;
                                                                }
                                                                else
                                                                {
                                                                    strError = cluster.Message + " (" + cluster.Code + ")";
                                                                    oAvamar.AddLog(intSelectedGrid, intSelectedGroup, 0, strError);
                                                                    AddResult(intAnswer, strName, oLog, "...." + strSelectedGroupFQ + " = " + strError);
                                                                }
                                                            }
                                                            else
                                                                ok = true;
                                                            if (ok)
                                                            {
                                                                // Enable client
                                                                AvamarReturnType enabled = API(oWebService.UpdateAvamarClient(strSelectedGrid, strSelectedDomainFQ, strClient, true));
                                                                if (enabled.Error == false)
                                                                {
                                                                    // Done!
                                                                    oLog.AddEvent(intAnswer, strName, "", "Registration process complete. Waiting 60 seconds...", LoggingType.Information);
                                                                    oAvamar.AddLog(intSelectedGrid, intSelectedGroup, intSelectedDomain, strName + " has been registered");

                                                                    oServer.UpdateAvamarRegistrationCompleted(intServer, intSelectedGrid, intSelectedDomain, "", client.Message, DateTime.Now.ToString(), 0);
                                                                    // Increase Grid Registration
                                                                    intSelectedGridRegistered++;
                                                                    oAvamar.UpdateGrid(intSelectedGrid, intSelectedGridRegistered);
                                                                    // Increase Domain Registration
                                                                    intSelectedDomainRegistered++;
                                                                    oAvamar.UpdateDomain(intSelectedDomain, intSelectedDomainRegistered);

                                                                    oServer.AddAvamarGroup(intServer, intSelectedGroup, "", group.Message);
                                                                    // Increase Group (1) Registration
                                                                    intSelectedGroupRegistered++;
                                                                    oAvamar.UpdateGroup(intSelectedGroup, intSelectedGroupRegistered);
                                                                    if (intSelectedClusterGroup > 0)
                                                                    {
                                                                        oServer.AddAvamarGroup(intServer, intSelectedClusterGroup, "", strClusterGroupMessage);
                                                                        // Increase Group (2) Registration
                                                                        intSelectedClusterGroupRegistered++;
                                                                        oAvamar.UpdateGroup(intSelectedClusterGroup, intSelectedClusterGroupRegistered);
                                                                    }

                                                                    // Wait 1 minute for synchronization
                                                                    Thread.Sleep(60000);

                                                                    // Process next step in provisioning (to kick out B.C. / transparent tasks)
                                                                    oServer.GetExecution(intServer, EnvironmentID, dsnAsset, dsnIP, dsnServiceEditor, AssignPage, ViewPage);

                                                                    // Add to activation queue.
                                                                    oServer.AddAvamarActivation(intServer, strClient, strSelectedGrid, strSelectedDomainFQ, strSelectedGroupFQ, strSelectedClusterGroupFQ, "", (boolOnsite ? DateTime.Now.ToString() : ""));
                                                                    oLog.AddEvent(intAnswer, strName, "", "Server has been added to activation queue.", LoggingType.Information);
                                                                }
                                                                else
                                                                {
                                                                    strError = enabled.Message + " (" + enabled.Code + ")";
                                                                    oAvamar.AddLog(intSelectedGrid, 0, intSelectedDomain, strError);
                                                                    AddResult(intAnswer, strName, oLog, "...." + strSelectedDomainFQ + " = " + strError);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            strError = group.Message + " (" + group.Code + ")";
                                                            oAvamar.AddLog(intSelectedGrid, intSelectedGroup, 0, strError);
                                                            AddResult(intAnswer, strName, oLog, "...." + strSelectedGroupFQ + " = " + strError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        strError = client.Message + " (" + client.Code + ")";
                                                        oAvamar.AddLog(intSelectedGrid, 0, 0, strError);
                                                        AddResult(intAnswer, strName, oLog, "...." + strClient + " = " + strError);
                                                    }
                                                }
                                                catch (Exception exError)
                                                {
                                                    strError = exError.Message;
                                                    if (exError.InnerException != null)
                                                        strError += " ~ " + exError.InnerException.Message;
                                                }
                                            }
                                        }
                                        else if (boolCluster)
                                            strError = "Could not locate a cluster group";
                                    }
                                    else
                                        strError = "Could not locate a group";
                                }
                                else
                                {
                                    strError = groups.Message + " (" + groups.Code + ")";
                                    oAvamar.AddLog(intSelectedGrid, 0, 0, strError);
                                    AddResult(intAnswer, strName, oLog, "...." + strSelectedGrid + " = " + strError);
                                }
                            }
                            else
                                strError = "Could not locate a domain";
                        }
                        else
                        {
                            strError = domains.Message + " (" + domains.Code + ")";
                            oAvamar.AddLog(intSelectedGrid, 0, 0, strError);
                            AddResult(intAnswer, strName, oLog, "...." + strSelectedGrid + " = " + strError);
                        }
                    }
                    else
                        strError = "Could not locate a grid";
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                strError = ex.Message;
                AddResult(intAnswer, strName, oLog, strError);
            }

            // Save log
            oServer.AddOutput(intServer, "AVAMAR_REGISTER", strResults);

            return strError;
        }
    }
}
