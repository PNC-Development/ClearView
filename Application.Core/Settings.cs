using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Net.Mail;
using System.Data.SqlTypes;

namespace NCC.ClearView.Application.Core
{
    public enum DNS_Status
    {
        QIP_Only = -10,
        QIP_BluecatDEV = -1,
        QIP_Bluecat = 0,
        Bluecat_Only = 1
    }
    public class Settings
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        private DNS_Status dnsStatus = DNS_Status.QIP_Only;
        public Settings(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
		public void Update(string _username, string _password, int _environment) 
		{
			Encryption oEncrypt = new Encryption();
            Variables oVariable = new Variables(_environment);
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@username", oEncrypt.Encrypt(_username, "uncview"));
			arParams[1] = new SqlParameter("@password", oEncrypt.Encrypt(_password, "pwcview"));
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSetting", arParams);

            Functions oFunction = new Functions(0, dsn, _environment);
            string strEMailIdsTO = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
            oFunction.SendEmail("ClearView Admin Account for " + DateTime.Today.ToShortDateString(), strEMailIdsTO, "", "", "ClearView Admin Account for " + DateTime.Today.ToShortDateString(), "U: " + _username + "<br/>P: " + _password, false, false);
		}
		public string Get(string _setting)
		{
			DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSetting");
			return ds.Tables[0].Rows[0][_setting].ToString();
		}
        public void UpdatePinging(bool _pinging)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@pinging", (_pinging == true ? DateTime.Now : SqlDateTime.Null));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSettingPing", arParams);
        }
        public void UpdateError(string _error)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@error", _error);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSettingError", arParams);
        }
        public void UpdateADSych(string _ad_sync)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@ad_sync", _ad_sync);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSettingADSych", arParams);
        }
        public void SystemError(int _serverid, int _workstationid, int _stepid, string _error, int _assetid, int _modelid, bool _is_vmware, VMWare _vmware, int _environment, string _dsn_asset)
        {
            int intError = 0;
            if (_stepid > 0)
            {
                string strName = "";
                string strType = "";
                int intID = 0;
                OnDemand oOnDemand = new OnDemand(0, dsn);
                if (_serverid > 0)
                {
                    oOnDemand.UpdateStepDoneServer(_serverid, _stepid, _error, 1, false, false);
                    Servers oServer = new Servers(0, dsn);
                    intError = oServer.AddError(0, 0, 0, _serverid, _stepid, _error);
                    strName = oServer.GetName(_serverid, true);
                    strType = "server";
                    intID = _serverid;
                }
                if (_workstationid > 0)
                {
                    oOnDemand.UpdateStepDoneWorkstation(_workstationid, _stepid, _error, 1, false, false);
                    Workstations oWorkstation = new Workstations(0, dsn);
                    intError = oWorkstation.AddVirtualError(0, 0, 0, _workstationid, _stepid, _error);
                    int intName = 0;
                    if (Int32.TryParse(oWorkstation.GetVirtual(_workstationid, "nameid"), out intName) == true)
                        strName = oWorkstation.GetName(intName);
                    strType = "workstation";
                    intID = _workstationid;
                }
                if (_serverid > 0 || _workstationid > 0)
                {
                    ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
                    Functions oFunction = new Functions(0, dsn, _environment);
                    Variables oVariable = new Variables(_environment);
                    Asset oAsset = new Asset(0, _dsn_asset);

                    int intType = oModelsProperties.GetType(_modelid);
                    DataSet dsSteps = oOnDemand.GetSteps(intType, 1);
                    string strStep = "N / A";
                    if (dsSteps.Tables[0].Rows.Count > 0)
                        strStep = dsSteps.Tables[0].Rows[_stepid - 1]["name"].ToString();
                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_SUPPORT");
                    try
                    {
                        if (_is_vmware == true)
                            oFunction.SendEmail("Auto-Provisioning FATAL ERROR: " + strName, strEMailIdsBCC, "", "", "Auto-Provisioning FATAL ERROR: " + strName, "<p><b>This message is to inform you that the " + strType + " " + strName + " has encountered a FATAL error and has been stopped!</b><p><p>Serial Number: " + oAsset.Get(_assetid, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(_assetid, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(_modelid, "name").ToUpper() + "<br/>Step #: " + _stepid.ToString() + "<br/>Step: " + strStep + "<br/>Error: " + _error + "<br/>DataCenter: " + _vmware.DataCenter() + "<br/>Virtual Center: " + _vmware.VirtualCenter() + "</p><p>When this issue has been resolved, <a href=\"" + oVariable.URL() + "/admin/errors_" + strType + ".aspx?id=" + intID.ToString() + "\" target=\"_blank\">click here</a> to clear this error and continue with the build.</p>", true, false);
                        else
                            oFunction.SendEmail("Auto-Provisioning FATAL ERROR: " + strName, strEMailIdsBCC, "", "", "Auto-Provisioning FATAL ERROR: " + strName, "<p><b>This message is to inform you that the " + strType + " " + strName + " has encountered a FATAL error and has been stopped!</b><p><p>Serial Number: " + oAsset.Get(_assetid, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(_assetid, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(_modelid, "name").ToUpper() + "<br/>Step #: " + _stepid.ToString() + "<br/>Step: " + strStep + "<br/>Error: " + _error + "<br/>ILO: <a href=\"https://" + oAsset.GetServerOrBlade(_assetid, "ilo") + "\" target=\"_blank\">" + oAsset.GetServerOrBlade(_assetid, "ilo") + "</a></p><p>When this issue has been resolved, <a href=\"" + oVariable.URL() + "/admin/errors_" + strType + ".aspx?id=" + intID.ToString() + "\" target=\"_blank\">click here</a> to clear this error and continue with the build.</p>", true, false);
                    }
                    catch (Exception ex)
                    {
                        oFunction.SendEmail("Auto-Provisioning FATAL ERROR: " + strName, strEMailIdsBCC, "", "", "Auto-Provisioning FATAL ERROR: " + strName, "<p><b>This message is to inform you that the " + strType + " " + strName + " has encountered a FATAL error and has been stopped!</b><p><p>Serial Number: " + oAsset.Get(_assetid, "serial").ToUpper() + "<br/>Asset Tag: " + oAsset.Get(_assetid, "asset").ToUpper() + "<br/>Model: " + oModelsProperties.Get(_modelid, "name").ToUpper() + "<br/>Step #: " + _stepid.ToString() + "<br/>Step: " + strStep + "<br/>Error: " + _error + "<br/>Notification Error: (Error Message: " + ex.Message + ") (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ")</p><p>When this issue has been resolved, <a href=\"" + oVariable.URL() + "/admin/errors_" + strType + ".aspx?id=" + intID.ToString() + "\" target=\"_blank\">click here</a> to clear this error and continue with the build.</p>", true, false);
                    }
                }
            }
            UpdateError(_error);
        }
        public object GetSystemDown()
        {
            arParams = new SqlParameter[1];
            return SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getSystemDown", arParams);
        }

        public bool IsDNS_QIP()
        {
            int intStatusDNS = 100;
            if (Int32.TryParse(Get("dns_status"), out intStatusDNS) == true)
                dnsStatus = (DNS_Status)intStatusDNS;
            if (dnsStatus == DNS_Status.QIP_Only || dnsStatus == DNS_Status.QIP_BluecatDEV || dnsStatus == DNS_Status.QIP_Bluecat)
                return true;
            else
                return false;
        }
        public bool IsDNS_Bluecat()
        {
            int intStatusDNS = 100;
            if (Int32.TryParse(Get("dns_status"), out intStatusDNS) == true)
                dnsStatus = (DNS_Status)intStatusDNS;
            if (dnsStatus == DNS_Status.Bluecat_Only || dnsStatus == DNS_Status.QIP_BluecatDEV || dnsStatus == DNS_Status.QIP_Bluecat)
                return true;
            else
                return false;
        }
    }
}
