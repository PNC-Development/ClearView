using System;
using System.IO;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.ApplicationBlocks.Data;
using NCC.ClearView.Application.Core;
using Tamir.SharpSsh;

namespace ClearViewCaptureRealTimeInfo
{
    public class EnclosureAndBlades
    {

        protected SqlParameter[] arParams;
        protected string strSupportTeamEMailIds = "";
        protected string strSSHCommand = "";
        protected DataTable dtEncInfoAttributes = new DataTable();
        protected DataTable dtBladeInfoAttributes = new DataTable();
        protected DataTable dtBladeStatusAttributes = new DataTable();
        protected Functions oFunction;
        protected Asset oAsset;
        protected Variables oVariable;
        private string strErrorMsg = "";
        private bool boolErrorOccured=false;

        public EnclosureAndBlades()
        {
               oFunction = new Functions(0,ServiceCaptureRealTimeInfo.dsn, ServiceCaptureRealTimeInfo.intEnvironment);
               oAsset = new Asset(0, ServiceCaptureRealTimeInfo.dsnAsset);
               oVariable = new Variables(ServiceCaptureRealTimeInfo.intEnvironment);
               strSupportTeamEMailIds = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");

               defineOutputAttributes();
        }

        protected void defineOutputAttributes()
        {
            DataColumn colEncInfo1 = new DataColumn("Id", typeof(int));
            DataColumn colEncInfo2 = new DataColumn("Attribute", typeof(string));
            DataColumn colEncInfo3 = new DataColumn("ContentSearchStartSection", typeof(string));
            DataColumn colEncInfo4 = new DataColumn("ContentSearchEndSection", typeof(string));

            //Enclosure Info
            dtEncInfoAttributes.Columns.Add(colEncInfo1); dtEncInfoAttributes.Columns.Add(colEncInfo2);
            dtEncInfoAttributes.Columns.Add(colEncInfo3); dtEncInfoAttributes.Columns.Add(colEncInfo4);

            dtEncInfoAttributes.Rows.Add(1, "Enclosure Name:", "Enclosure Information:", "Power Distribution Unit:");
            dtEncInfoAttributes.Rows.Add(2, "Enclosure Type:", "Enclosure Information:", "Power Distribution Unit:");
            dtEncInfoAttributes.Rows.Add(3, "Serial Number:", "Enclosure Information:", "Power Distribution Unit:");
            dtEncInfoAttributes.Rows.Add(4, "UUID:", "Enclosure Information:", "Power Distribution Unit:");
            dtEncInfoAttributes.Rows.Add(5, "Asset Tag:", "Enclosure Information:", "Power Distribution Unit:");
            dtEncInfoAttributes.Rows.Add(6, "Midplane Spare Part Number:", "Enclosure Information:", "Power Distribution Unit:");
            dtEncInfoAttributes.Rows.Add(7, "PDU Type:", "Power Distribution Unit:", "Onboard Administrator Tray Information:");
            dtEncInfoAttributes.Rows.Add(8, "PDU Spare Part Number:", "Power Distribution Unit:", "Onboard Administrator Tray Information:");
            dtEncInfoAttributes.Rows.Add(9, "Type:", "Onboard Administrator Tray Information:", "");
            dtEncInfoAttributes.Rows.Add(10, "Spare Part Number:", "Onboard Administrator Tray Information::", "");
            dtEncInfoAttributes.Rows.Add(11, "Serial Number:", "Onboard Administrator Tray Information::", "");

            //Blade Info
            DataColumn colServerInfo1 = new DataColumn("Id", typeof(int));
            DataColumn colServerInfo2 = new DataColumn("Attribute", typeof(string));
            DataColumn colServerInfo3 = new DataColumn("ContentSearchStartSection", typeof(string));
            DataColumn colServerInfo4 = new DataColumn("ContentSearchEndSection", typeof(string));

            dtBladeInfoAttributes.Columns.Add(colServerInfo1); dtBladeInfoAttributes.Columns.Add(colServerInfo2);
            dtBladeInfoAttributes.Columns.Add(colServerInfo3); dtBladeInfoAttributes.Columns.Add(colServerInfo4);
            dtBladeInfoAttributes.Rows.Add(1, "Type:", "Server Blade #SLOT# Information:", "Management Processor Information:");
            dtBladeInfoAttributes.Rows.Add(2, "Manufacturer:", "Server Blade #SLOT# Information:", "Management Processor Information:");
            dtBladeInfoAttributes.Rows.Add(3, "Product Name:", "Server Blade #SLOT# Information:", "Management Processor Information:");
            dtBladeInfoAttributes.Rows.Add(4, "Part Number:", "Server Blade #SLOT# Information:", "Management Processor Information:");
            dtBladeInfoAttributes.Rows.Add(5, "System Board Spare Part Number:", "Server Blade #SLOT# Information:", "Management Processor Information:");
            dtBladeInfoAttributes.Rows.Add(6, "Serial Number:", "Server Blade #SLOT# Information:", "Management Processor Information:");
            dtBladeInfoAttributes.Rows.Add(7, "UUID:", "Server Blade #SLOT# Information:", "Management Processor Information:");
            dtBladeInfoAttributes.Rows.Add(8, "Server Name:", "Server Blade #SLOT# Information:", "Management Processor Information:");
            dtBladeInfoAttributes.Rows.Add(9, "Asset Tag:", "Server Blade #SLOT# Information:", "Management Processor Information:");
            dtBladeInfoAttributes.Rows.Add(10, "ROM Version:", "Server Blade #SLOT# Information:", "Management Processor Information:");
            dtBladeInfoAttributes.Rows.Add(11, "Memory:", "Server Blade #SLOT# Information:", "Management Processor Information:");
            dtBladeInfoAttributes.Rows.Add(12, "NIC 1 MAC Address:", "Server Blade #SLOT# Information:", "Management Processor Information:");
            dtBladeInfoAttributes.Rows.Add(13, "IP Address:", "Management Processor Information:", "");

            //Blade Status
            DataColumn colServerStatus1 = new DataColumn("Id", typeof(int));
            DataColumn colServerStatus2 = new DataColumn("Attribute", typeof(string));
            DataColumn colServerStatus3 = new DataColumn("ContentSearchStartSection", typeof(string));
            DataColumn colServerStatus4 = new DataColumn("ContentSearchEndSection", typeof(string));
            dtBladeStatusAttributes.Columns.Add(colServerStatus1); dtBladeStatusAttributes.Columns.Add(colServerStatus2);
            dtBladeStatusAttributes.Columns.Add(colServerStatus3); dtBladeStatusAttributes.Columns.Add(colServerStatus4);
            dtBladeStatusAttributes.Rows.Add(1, "Power:", "Blade #1 Status:", "Diagnostic Status:");
            dtBladeStatusAttributes.Rows.Add(2, "Current Wattage used:", "Blade #1 Status:", "Diagnostic Status:");
            dtBladeStatusAttributes.Rows.Add(3, "Health:", "Blade #1 Status:", "Diagnostic Status:");
            dtBladeStatusAttributes.Rows.Add(4, "Unit Identification LED:", "Blade #1 Status:", "Diagnostic Status:");
            dtBladeStatusAttributes.Rows.Add(5, "Internal Data", "Diagnostic Status:", "");
            dtBladeStatusAttributes.Rows.Add(6, "Management Processor", "Diagnostic Status:", "");
            dtBladeStatusAttributes.Rows.Add(7, "I/O Configuration", "Diagnostic Status:", "");
            dtBladeStatusAttributes.Rows.Add(8, "Power", "Diagnostic Status:", "");
            dtBladeStatusAttributes.Rows.Add(9, "Cooling", "Diagnostic Status:", "");
            dtBladeStatusAttributes.Rows.Add(10, "Location", "Diagnostic Status:", "");
            dtBladeStatusAttributes.Rows.Add(11, "Device Failure", "Diagnostic Status:", "");
            dtBladeStatusAttributes.Rows.Add(12, "Device Degraded", "Diagnostic Status:", "");
            dtBladeStatusAttributes.Rows.Add(13, "VCM Configured", "Diagnostic Status:", "");
            dtBladeStatusAttributes.Rows.Add(14, "iLO Network", "Diagnostic Status:", "");





        }

        public void CaptureEnclosureAndBladeInfo()
        {

            try
            {
                //Delete the previous log file
                DeleteLog();
                //Clean the records from database
                ClearEnclosureAndBladeRecords();

                //Get the Enclosure and loop through each enclosure
                DataSet dsEnc = oAsset.GetEnclosures(10);

                foreach (DataRow dr in dsEnc.Tables[0].Rows)
                {
                    string host = "";
                    try
                    {
                        if (dr["make"].ToString().ToUpper() != "SUN" )
                        {
                            string[] strIP = dr["oa_ip"].ToString().Split('.');

                            if (dr["oa_ip"].ToString() == "" || strIP.Length < 4)
                            {
                                strErrorMsg = "Enclosure Name :" + dr["Name"].ToString() + "\r\n";
                                strErrorMsg = strErrorMsg + "Serial #:" + dr["Serial"].ToString() + "\r\n";
                                strErrorMsg = strErrorMsg + "Invalid OA IP Address : " + dr["oa_ip"].ToString() + "\r\n";
                                throw new Exception(strErrorMsg);

                            }

                            host = dr["oa_ip"].ToString();
                            SshExec oSSH = new SshExec(host, oVariable.ILOUsername(), oVariable.ILOPassword());
                            oSSH.Connect();

                                long lngEncId = CaptureEnclosureInfo(host, oSSH);
                                CaptureBladeInfo(host,oSSH, lngEncId);

                            oSSH.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        strErrorMsg = "Host Name :" + host + "\r\n";
                        boolErrorOccured = true;
                        CreateLog(strErrorMsg+ ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                 //bool rethrow = ExceptionPolicy.HandleException(ex, "Log Only Policy");
                //if (rethrow){  throw;}   

                CreateLog(ex.Message);
            }
            finally
            { 
                if (boolErrorOccured==true)
                    oFunction.SendEmail("ERROR: Capturing Real Time Information for Enclosures and Blades", strSupportTeamEMailIds, "", "", "ERROR: Capturing Real Time Information for Enclosures and Blades", "<p><b>An error occurred while capturing Real Time Information for Enclosures and Blades...</b></p><p>Please find attached file for more details.</p>",true, false, ServiceCaptureRealTimeInfo.LogFilePath);
                else
                    oFunction.SendEmail("Capturing Real Time Information for Enclosures and Blades successfully completed", strSupportTeamEMailIds, "", "", "Capturing Real Time Information for Enclosures and Blades successfully completed", "<p><b>Capturing Real Time Information for Enclosures and Blades successfully completed</b></p>", true, false);
            }
        }


        protected long CaptureEnclosureInfo(string _host, SshExec oSSH)
        {

            try
            {
                string strOutput = "";
                string strEnclosureName = "";
                string strEnclosureType = "";
                string strSerialNumber = "";
                string strUUID = "";
                string strAssetTag = "";

                strSSHCommand = "show enclosure info";
                strOutput = runSSHCommand(_host,oSSH, strSSHCommand);
                foreach (DataRow dr in dtEncInfoAttributes.Rows)
                {
                    switch (Int32.Parse(dr["id"].ToString()))
                    {
                        case 1:
                            strEnclosureName = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                            dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                            break;
                        case 2:
                            strEnclosureType = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                            dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                            break;
                        case 3:
                            strSerialNumber = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                            dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                            break;
                        case 4:
                            strUUID = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                            dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                            break;
                        case 5:
                            strAssetTag = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                            dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                            break;
                    }
                }

                return AddEnclosureInfo(strEnclosureName, strEnclosureType, strSerialNumber,strAssetTag,_host, strUUID );

            }
            catch (Exception ex)
            {
                
                strErrorMsg = "Host Name :" + _host + "\r\n";
                strErrorMsg = strErrorMsg + strSSHCommand + "\r\n";

                boolErrorOccured = true;
                throw new Exception(strErrorMsg + ex.Message);
            }

            }

        protected void CaptureBladeInfo(string _host,  SshExec oSSH,long _lngEncId)
        {
            string strOutput = "";

            long lngEncId = 0;
            int intSlot = 0;
            string strType = "";
            string strManufacturer = "";
            string strProductName = "";
            string strSerialNumber = "";
            string strUUID = "";
            string strServerName = "";
            string strAssetTag = "";
            string strMemory = "";
            string strNIC1_MACAddress = "";
            string strILO_IPAddress = "";
            string strStatus_Power = "";
            string strStatus_CurrentWattageUsed = "";
            string strStatus_Health = "";
            string strStatus_UnitIdentificationLED = "";
            string strDiaStatus_InternalData = "";
            string strDiaStatus_ManagementProcessor = "";
            string strDiaStatus_IOConfig = "";
            string strDiaStatus_Power = "";
            string strDiaStatus_Cooling = "";
            string strDiaStatus_Location = "";
            string strDiaStatus_DeviceFailure = "";
            string strDiaStatus_DeviceDegraded = "";
            string strDiaStatus_VCMConfigured = "";
            string strDiaStatus_ILONetwork = "";
            int i = 1; ;
           
                lngEncId = _lngEncId;
                for (i = 1; i <= 16; i++)
                {
                    try
                    {
                        strOutput = "";
                        strSSHCommand = "show server info " + i.ToString();
                        strOutput = runSSHCommand(_host,oSSH, strSSHCommand);

                        foreach (DataRow dr in dtBladeInfoAttributes.Rows)
                        {
                            if (dr["ContentSearchStartSection"].ToString().Contains("SLOT#"))
                                dr["ContentSearchStartSection"] = dr["ContentSearchStartSection"].ToString().Replace("SLOT#", i.ToString());

                            if (dr["ContentSearchEndSection"].ToString().Contains("SLOT#"))
                                dr["ContentSearchEndSection"] = dr["ContentSearchEndSection"].ToString().Replace("SLOT#", i.ToString());

                            switch (Int32.Parse(dr["id"].ToString()))
                            {
                                case 1:
                                    strType = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 2:
                                    strManufacturer = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 3:
                                    strProductName = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 6:
                                    strSerialNumber = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 7:
                                    strUUID = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 8:
                                    strServerName = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 9:
                                    strAssetTag = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 11:
                                    strMemory = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 12:
                                    strNIC1_MACAddress = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 13:
                                    strILO_IPAddress = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                            }

                        }

                        strOutput = "";
                        strSSHCommand = "show server status " + i.ToString();
                        strOutput = runSSHCommand(_host,oSSH, strSSHCommand);

                        foreach (DataRow dr in dtBladeStatusAttributes.Rows)
                        {
                            if (dr["ContentSearchStartSection"].ToString().Contains("SLOT#"))
                                dr["ContentSearchStartSection"] = dr["ContentSearchStartSection"].ToString().Replace("SLOT#", i.ToString());

                            if (dr["ContentSearchEndSection"].ToString().Contains("SLOT#"))
                                dr["ContentSearchEndSection"] = dr["ContentSearchEndSection"].ToString().Replace("SLOT#", i.ToString());

                            switch (Int32.Parse(dr["id"].ToString()))
                            {
                                case 1:
                                    strStatus_Power = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 2:
                                    strStatus_CurrentWattageUsed = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 3:
                                    strStatus_Health = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 4:
                                    strStatus_UnitIdentificationLED = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 5:
                                    strDiaStatus_InternalData = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 6:
                                    strDiaStatus_ManagementProcessor = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 7:
                                    strDiaStatus_IOConfig = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 8:
                                    strDiaStatus_Power = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 9:
                                    strDiaStatus_Cooling = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 10:
                                    strDiaStatus_Location = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 11:
                                    strDiaStatus_DeviceFailure = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 12:
                                    strDiaStatus_DeviceDegraded = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 13:
                                    strDiaStatus_VCMConfigured = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                                case 14:
                                    strDiaStatus_ILONetwork = getSSHCommandAttributeValue(strOutput, dr["Attribute"].ToString(),
                                    dr["ContentSearchStartSection"].ToString(), dr["ContentSearchEndSection"].ToString());
                                    break;
                            }

                        }
                        AddBladeInfo(lngEncId,
                                           i,
                                           strType,
                                           strManufacturer,
                                           strProductName,
                                           strSerialNumber,
                                           strUUID,
                                           strServerName,
                                           strAssetTag,
                                           strMemory,
                                           strNIC1_MACAddress,
                                           strILO_IPAddress,
                                           strStatus_Power,
                                           strStatus_CurrentWattageUsed,
                                           strStatus_Health,
                                           strStatus_UnitIdentificationLED,
                                           strDiaStatus_InternalData,
                                           strDiaStatus_ManagementProcessor,
                                           strDiaStatus_IOConfig,
                                           strDiaStatus_Power,
                                           strDiaStatus_Cooling,
                                           strDiaStatus_Location,
                                           strDiaStatus_DeviceFailure,
                                           strDiaStatus_DeviceDegraded,
                                           strDiaStatus_VCMConfigured,
                                           strDiaStatus_ILONetwork);

                    }

                    catch (Exception ex)
                    {
                        strErrorMsg = "Host Name :" + _host + "\r\n";
                        strErrorMsg = strErrorMsg + strSSHCommand + "\r\n";

                        boolErrorOccured = true;
                        CreateLog(strErrorMsg + ex.Message);
                    }

                } 
           
        }

        # region Database - Operatiions
            protected void ClearEnclosureAndBladeRecords()
            {

                SqlHelper.ExecuteNonQuery(ServiceCaptureRealTimeInfo.dsn, CommandType.StoredProcedure, "pr_DeleteRealTimeEnclosureAndBladeInfo", arParams);
            }
            protected long AddEnclosureInfo(string _strEncName, string _strEncType, string _strSerialNo,
                                            string _strAssetTag, string _strOAIP, string _strUUID)
            {
                arParams = new SqlParameter[8];
                arParams[0] = new SqlParameter("@EncId", SqlDbType.BigInt);
                arParams[0].Direction = ParameterDirection.Output;
                arParams[1] = new SqlParameter("@EncName", _strEncName);
                arParams[2] = new SqlParameter("@EncType", _strEncType);
                arParams[3] = new SqlParameter("@SerialNumber", _strSerialNo);
                arParams[4] = new SqlParameter("@AssetTag", _strAssetTag);
                arParams[5] = new SqlParameter("@OA_IP", _strOAIP);
                arParams[6] = new SqlParameter("@UUID", _strUUID);
                

                SqlHelper.ExecuteNonQuery(ServiceCaptureRealTimeInfo.dsn, CommandType.StoredProcedure, "pr_AddRealTimeEnclosureInfo", arParams);
                return Int64.Parse(arParams[0].Value.ToString());

            }

            protected long AddBladeInfo(long _lngEncId, int _intSlot, string _strType
                                           , string _strManufacturer
                                           , string _strProductName
                                           , string _strSerialNumber
                                           , string _strUUID
                                           , string _strServerName
                                           , string _strAssetTag
                                           , string _strMemory
                                           , string _strNIC1_MACAddress
                                           , string _strILO_IPAddress
                                           , string _strStatus_Power
                                           , string _strStatus_CurrentWattageUsed
                                           , string _strStatus_Health
                                           , string _strStatus_UnitIdentificationLED
                                           , string _strDiaStatus_InternalData
                                           , string _strDiaStatus_ManagementProcessor
                                           , string _strDiaStatus_IOConfig
                                           , string _strDiaStatus_Power
                                           , string _strDiaStatus_Cooling
                                           , string _strDiaStatus_Location
                                           , string _strDiaStatus_DeviceFailure
                                           , string _strDiaStatus_DeviceDegraded
                                           , string _strDiaStatus_VCMConfigured
                                           , string _strDiaStatus_ILONetwork)
            {
                arParams = new SqlParameter[30];
                arParams[0] = new SqlParameter("@BladeId", SqlDbType.BigInt);
                arParams[0].Direction = ParameterDirection.Output;
                arParams[1] = new SqlParameter("@EncId", _lngEncId);
                arParams[2] = new SqlParameter("@Slot", _intSlot);
                arParams[3] = new SqlParameter("@Type", _strType);
                arParams[4] = new SqlParameter("@Manufacturer", _strManufacturer);
                arParams[5] = new SqlParameter("@ProductName", _strProductName);
                arParams[6] = new SqlParameter("@SerialNumber", _strSerialNumber);
                arParams[7] = new SqlParameter("@UUID", _strUUID);
                arParams[8] = new SqlParameter("@ServerName", _strServerName);
                arParams[9] = new SqlParameter("@AssetTag", _strAssetTag);
                arParams[10] = new SqlParameter("@Memory", _strMemory);
                arParams[11] = new SqlParameter("@NIC1_MACAddress", _strNIC1_MACAddress);
                arParams[12] = new SqlParameter("@ILO_IPAddress", _strILO_IPAddress);
                arParams[13] = new SqlParameter("@Status_Power", _strStatus_Power);
                arParams[14] = new SqlParameter("@Status_CurrentWattageUsed", _strStatus_CurrentWattageUsed);
                arParams[15] = new SqlParameter("@Status_Health", _strStatus_Health);
                arParams[16] = new SqlParameter("@Status_UnitIdentificationLED", _strStatus_UnitIdentificationLED);
                arParams[17] = new SqlParameter("@DiaStatus_InternalData", _strDiaStatus_InternalData);
                arParams[18] = new SqlParameter("@DiaStatus_ManagementProcessor", _strDiaStatus_ManagementProcessor);
                arParams[19] = new SqlParameter("@DiaStatus_IOConfig", _strDiaStatus_IOConfig);
                arParams[20] = new SqlParameter("@DiaStatus_Power", _strDiaStatus_Power);
                arParams[21] = new SqlParameter("@DiaStatus_Cooling", _strDiaStatus_Cooling);
                arParams[22] = new SqlParameter("@DiaStatus_Location", _strDiaStatus_Location);
                arParams[23] = new SqlParameter("@DiaStatus_DeviceFailure", _strDiaStatus_DeviceFailure);
                arParams[24] = new SqlParameter("@DiaStatus_DeviceDegraded", _strDiaStatus_DeviceDegraded);
                arParams[25] = new SqlParameter("@DiaStatus_VCMConfigured", _strDiaStatus_VCMConfigured);
                arParams[26] = new SqlParameter("@DiaStatus_ILONetwork", _strDiaStatus_ILONetwork);




                SqlHelper.ExecuteNonQuery(ServiceCaptureRealTimeInfo.dsn, CommandType.StoredProcedure, "pr_AddRealTimeBladeInfo", arParams);
                return Int64.Parse(arParams[0].Value.ToString());

        }

        #endregion

        # region SSH Commands

            protected string runSSHCommand(string _host,SshExec oSSH, string _commands)
            {
                string strResult = "";
                string strSSHConnect = "";
               

                char[] strSplit = { ';' };
                string[] strCommand = _commands.Split(strSplit);
                for (int ii = 0; ii < strCommand.Length; ii++)
                {
                    if (strCommand[ii].Trim() != "")
                    {
                        strSSHConnect = "Host = " + _host + ", Command = " + strCommand[ii].Trim();
                        string strResultTemp = oSSH.RunCommand(strCommand[ii].Trim());
                        if (strResultTemp.Contains("Invalid Arguments") == true)
                        {
                            strResult = "";
                            throw new Exception(strResultTemp + "\r\n");
                            break;
                        }
                        else
                            strResult += strResultTemp + ";";
                    }
                }
               
                return strResult;
            }


            public string getSSHCommandAttributeValue(string _output, string _attribute, string _contentSearchStartSection, string _contentSearchEndSection)
            {
                string strReturn = "";
                string strContentSearchSection ="";
                while (_output.Contains(": ") == true)
                    _output = _output.Replace(": ", ":");
                while (_output.Contains(" :") == true)
                    _output = _output.Replace(" :", ":");

                if (_contentSearchEndSection != "")
                {
                    int intStart = _output.IndexOf(_contentSearchStartSection);
                    int intStop = _output.IndexOf(_contentSearchEndSection);
                    if (intStop == -1 || intStop == -1)
                        return strReturn;
                    strContentSearchSection = _output.Substring(intStart + 1, intStop - intStart - 1);
                }
                else
                {
                    if (_output.IndexOf(_contentSearchStartSection) == -1)
                        return strReturn;
                    strContentSearchSection = _output.Substring(_output.IndexOf(_contentSearchStartSection));
                }

                if (strContentSearchSection.IndexOf(_attribute)==-1)
                    return strReturn;
                
                strReturn = strContentSearchSection.Substring(strContentSearchSection.IndexOf(_attribute) + _attribute.Length );
                strReturn = strReturn.Substring(0, strReturn.IndexOf("\n"));
                return strReturn.Trim();
            }

        #endregion


        #region Log Error
            
            private void CreateLog(string strlogMessage)
            {
                String strLogfilePath = ServiceCaptureRealTimeInfo.LogFilePath; ;

                StringBuilder sb = new StringBuilder();
                if(File.Exists(strLogfilePath) ==false)
                {
                     using (StreamWriter sw = new StreamWriter(strLogfilePath))
                        {
                            sw.Write(sb.ToString());
                            sw.Close();
                        }

                }
                
                using (StreamWriter w = File.AppendText(strLogfilePath))
                {   
                    // Update the underlying file.
                    w.WriteLine ("------------------------------------------------------------------------");
                    w.WriteLine("{0} {1}", DateTime.Now.ToLongDateString(),DateTime.Now.ToLongTimeString());
                    w.WriteLine("  :");
                    w.WriteLine("  :{0}", strlogMessage);
                    w.WriteLine ("------------------------------------------------------------------------");
                    // Close the writer and underlying file.
                    w.Flush();
                    w.Close();
                }

            }

            private void DeleteLog()
            { 
                String strLogfilePath = ServiceCaptureRealTimeInfo.LogFilePath; ;
                if (File.Exists(strLogfilePath) == true)
                    File.Delete(strLogfilePath);
               
            }
            
        #endregion
    }
}
