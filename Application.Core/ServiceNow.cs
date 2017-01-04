using System;
using System.Collections.Generic;
using System.Text;

namespace NCC.ClearView.Application.Core
{
    public static class ServiceNowClasses
    {
        public static string AIX = "cmdb_ci_aix_server";
        public static string Linux = "cmdb_ci_linux_server";
        public static string Solaris = "cmdb_ci_solaris_server";
        public static string Windows = "cmdb_ci_win_server";
    }
    public static class ServiceNowEnvironments
    {
        public static string Development = "Development";
        public static string Test = "Test";
        public static string QA = "QA";
        public static string Production = "Production";
        public static string DR = "Disaster Recovery";
    }
    public static class ServiceNowDesiredOperationalStatus
    {
        public static string Active = "1000";
        public static string PreProduction = "500";
        public static string PendingDecommission = "-1";
        public static string Decommissioned = "-2";
    }
    public static class ServiceNowInstallStatus
    {
        public static string Installed = "1";
        public static string PendingInstall = "4";
        public static string Retired = "7";
    }
    public class ServiceNowData
    {
        public string u_discovery_source { get; set; }
        public string u_fqdn { get; set; }
        public string u_install_status { get; set; }
        public string u_ip_address { get; set; }
        public string u_manufacturer { get; set; }
        public string u_associated_mnemonic { get; set; }
        public string u_model_id { get; set; }
        public string u_name { get; set; }
        public string u_os { get; set; }
        public string u_serial_number { get; set; }
        public string u_sys_class_name { get; set; }
        public string u_environment { get; set; }
        public string u_virtual { get; set; }
    }
    public class ServiceNowInsert : ServiceNowData
    {
        // optional
        public string u_dns_domain { get; set; }
        public string u_install_date { get; set; }
        public string u_company { get; set; }
        public string u_location { get; set; }
        public string u_data_center { get; set; }
        public string u_rack { get; set; }
    }
    public class ServiceNowUpdate : ServiceNowData
    {
        public string u_action { get; set; }
        // optional
        public string u_desired_operational_state { get; set; }
    }
    public class ServiceNowResponse
    {
        /*
        {
           "import_set": "ISET0063987",
           "staging_table": "imp_server",
           "result": [   {
              "transform_map": "Server API Transform",
              "table": "cmdb_ci_server",
              "display_name": "name",
              "display_value": "HEALY0002",
              "record_link": "https://webtest-itsm.pncbank.com/api/now/table/cmdb_ci_server/cca3739e7dee1a4089c718469f4dce54",
              "status": "inserted",
              "sys_id": "cca3739e7dee1a4089c718469f4dce54"
           }]
        }
        */
        public string import_set { get; set; }
        public string staging_table { get; set; }
        public List<ServiceNowResponseResult> result { get; set; }
    }
    public class ServiceNowResponseResult
    {
        public string transform_map { get; set; }
        public string table { get; set; }
        public string display_name { get; set; }
        public string display_value { get; set; }
        public string record_link { get; set; }
        public string status { get; set; }
        public string sys_id { get; set; }
    }
    public enum ServiceNowDecom
    {
        Decommission,
        Destroy,
        Recommission
    }
    public class ServiceNowIncidents
    {
        public List<ServiceNowIncident> result { get; set; }
    }
    public class ServiceNowIncident
    {
        public string number { get; set; }
        public string state { get; set; }
        public string priority { get; set; }
        public ServiceNowReferrer assignment_group { get; set; }
        public ServiceNowReferrer caller_id { get; set; }
        public ServiceNowReferrer assigned_to { get; set; }
        public ServiceNowReferrer resolved_by { get; set; }
    }
    public class ServiceNowReferrer
    {
        public string link { get; set; }
        public string value { get; set; }
    }
}
