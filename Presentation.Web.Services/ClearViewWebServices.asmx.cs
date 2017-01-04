using NCC.ClearView.Application.Core;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

namespace Presentation.Web.Services
{
    /// <summary>
    /// Summary description for ClearViewWebServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ClearViewWebServices : System.Web.Services.WebService
    {

        // ************************************************************************************************************************
        // ************************************************************************************************************************
        //          QIP
        // ************************************************************************************************************************
        // ************************************************************************************************************************
        #region QIP
        [WebMethod(Description = "Create a DNS entry in QIP")]
        public string CreateDNSforPNC(string ObjectAddress, string ObjectName, string ObjectClass, string Aliases, string DomainName, string NameService, string DynamicDNSUpdate, int intUser, int intAnswer, bool boolDeleteFiles)
        {
            try
            {
                QIP qip = new QIP("CreateDNSforPNC");
                return qip.Create(ObjectAddress, ObjectName, ObjectClass, Aliases, DomainName, NameService, DynamicDNSUpdate, intUser, intAnswer, boolDeleteFiles, false);
            }
            catch (SoapException ex)
            {
                throw ex;
            }
        }
        [WebMethod(Description = "Update a DNS entry in QIP")]
        public string UpdateDNSforPNC(string ObjectAddress, string ObjectName, string ObjectClass, string Aliases, string DomainName, string NameService, string DynamicDNSUpdate, int intUser, int intAnswer, bool boolDeleteFiles)
        {
            try
            {
                QIP qip = new QIP("UpdateDNSforPNC");
                return qip.Update(ObjectAddress, ObjectName, ObjectClass, Aliases, DomainName, NameService, DynamicDNSUpdate, intUser, intAnswer, boolDeleteFiles, true);
            }
            catch (SoapException ex)
            {
                throw ex;
            }
        }
        [WebMethod(Description = "Delete a DNS entry in QIP")]
        public string DeleteDNSforPNC(string ObjectAddress, string ObjectName, int intUser, bool boolDeleteFiles)
        {
            try
            {
                QIP qip = new QIP("DeleteDNSforPNC");
                return qip.Delete(ObjectAddress, ObjectName, intUser, boolDeleteFiles);
            }
            catch (SoapException ex)
            {
                throw ex;
            }
        }
        [WebMethod(Description = "Search a DNS entry in QIP")]
        public string SearchDNSforPNC(string ObjectAddress, string ObjectName, bool boolIsAlias, bool boolDeleteFiles)
        {
            try
            {
                QIP qip = new QIP("SearchDNSforPNC");
                return qip.Search(ObjectAddress, ObjectName, boolIsAlias, boolDeleteFiles);
            }
            catch (SoapException ex)
            {
                throw ex;
            }
        }
        #endregion




        // ************************************************************************************************************************
        // ************************************************************************************************************************
        //          OTHER
        // ************************************************************************************************************************
        // ************************************************************************************************************************
        #region Other
        //[WebMethod(Description = "Loads project information into ClearView")]
        //public bool InsertProject(string _ProjectName, string _ProjectNumber, string _ProjectManager, string _ProjectDesc, int _CostCenter)
        //{
        //    try
        //    {
        //        Other other = new Other("InsertProject");
        //        return other.InsertProject(_ProjectName, _ProjectNumber, _ProjectManager, _ProjectDesc, _CostCenter);
        //    }
        //    catch (SoapException ex)
        //    {
        //        throw ex;
        //    }
        //}
        [WebMethod(Description = "Get the MAC Address of a Physical Server using the ILO")]
        public string GetMacFromILO(string _ilo, int _environment, bool _delete_files)
        {
            try
            {
                Other other = new Other("GetMacFromILO");
                return other.GetMacFromILO(_ilo, _environment, _delete_files);
            }
            catch (SoapException ex)
            {
                throw ex;
            }
        }
        #endregion




        // ************************************************************************************************************************
        // ************************************************************************************************************************
        //          BLUECAT
        // ************************************************************************************************************************
        // ************************************************************************************************************************
        #region BlueCat
        [WebMethod(Description = "Create a DNS entry in Bluecat")]
        public string CreateBluecatDNS(string ObjectAddress, string ObjectName, string Description, string MacAddress)
        {
            try
            {
                Bluecat bluecat = new Bluecat("CreateDNSforPNC");
                return bluecat.Create(ObjectAddress, ObjectName, Description, MacAddress);
            }
            catch (SoapException ex)
            {
                throw ex;
            }
        }
        [WebMethod(Description = "Update a DNS entry in Bluecat")]
        public string UpdateBluecatDNS(string ObjectAddress, string ObjectName, string Description, string MacAddress)
        {
            try
            {
                Bluecat bluecat = new Bluecat("UpdateDNSforPNC");
                return bluecat.Update(ObjectAddress, ObjectName, Description, MacAddress);
            }
            catch (SoapException ex)
            {
                throw ex;
            }
        }
        [WebMethod(Description = "Delete a DNS entry in Bluecat")]
        public string DeleteBluecatDNS(string ObjectAddress, string ObjectName, bool IncludeStaging, bool Decommission)
        {
            try
            {
                Bluecat bluecat = new Bluecat("DeleteDNSforPNC");
                return bluecat.Delete(ObjectAddress, ObjectName, IncludeStaging, false, Decommission);
            }
            catch (SoapException ex)
            {
                throw ex;
            }
        }
        [WebMethod(Description = "Search a DNS entry in Bluecat")]
        public string SearchBluecatDNS(string ObjectAddress, string ObjectName)
        {
            try
            {
                Bluecat bluecat = new Bluecat("SearchDNSforPNC");
                return bluecat.Search(ObjectAddress, ObjectName);
            }
            catch (SoapException ex)
            {
                throw ex;
            }
        }
        [WebMethod(Description = "Update a DNS entry description in Bluecat")]
        public string UpdateBluecatDescriptionDNS(string ObjectAddress, string Description)
        {
            try
            {
                Bluecat bluecat = new Bluecat("UpdateDNSforPNC");
                return bluecat.UpdateDescription(ObjectAddress, Description);
            }
            catch (SoapException ex)
            {
                throw ex;
            }
        }
        #endregion




        // ************************************************************************************************************************
        // ************************************************************************************************************************
        //          AVAMAR
        // ************************************************************************************************************************
        // ************************************************************************************************************************
        #region Avamar
        [WebMethod(Description = "Get the grid properties (server show-prop)")]
        public string GetAvamarGrid(string grid)
        {
            return ExecuteAvamar("GetAvamar", grid, "sudo /usr/local/avamar/bin/mccli server show-prop --verbose=true");
        }

        // CLIENT
        [WebMethod(Description = "Registers a client to the grid and adds it to the default group")]
        public string AddAvamarClient(string grid, string fully_qualified_domain_name, string fully_qualified_client_name, string ipaddress)
        {
            return ExecuteAvamar("AddAvamar", grid, "sudo /usr/local/avamar/bin/mccli client add --domain=" + fully_qualified_domain_name + " --name=" + fully_qualified_client_name.ToLower() + " --pageport=28002 --pageaddr=" + ipaddress);
        }
        [WebMethod(Description = "Gets the information related to a client")]
        public string GetAvamarClient(string grid, string fully_qualified_domain_name, string fully_qualified_client_name)
        {
            return ExecuteAvamar("GetAvamar", grid, "sudo /usr/local/avamar/bin/mccli client show --verbose=true --domain=" + fully_qualified_domain_name + " --name=" + fully_qualified_client_name);
        }
        [WebMethod(Description = "Enable or Disable a client")]
        public string UpdateAvamarClient(string grid, string fully_qualified_domain_name, string fully_qualified_client_name, bool enabled)
        {
            // to be done fourth.
            return ExecuteAvamar("EnableAvamar", grid, "sudo /usr/local/avamar/bin/mccli client edit --enabled=" + (enabled ? "true" : "false") + " --domain=" + fully_qualified_domain_name + " --name=" + fully_qualified_client_name);
        }
        [WebMethod(Description = "Activate a client")]
        public string ActivateAvamarClient(string grid, string fully_qualified_domain_name, string fully_qualified_client_name)
        {
            // to be done second.
            return ExecuteAvamar("EnableAvamar", grid, "sudo /usr/local/avamar/bin/mccli client invite --domain=" + fully_qualified_domain_name + " --name=" + fully_qualified_client_name);
        }
        [WebMethod(Description = "Retire a client")]
        public string DeleteAvamarClient(string grid, string fully_qualified_domain_name, string fully_qualified_client_name)
        {
            return ExecuteAvamar("RemoveAvamar", grid, "sudo /usr/local/avamar/bin/mccli client retire --domain=" + fully_qualified_domain_name + " --name=" + fully_qualified_client_name);
        }

        // DOMAIN
        [WebMethod(Description = "Get the domains for the grid (domain show)")]
        public string GetAvamarDomains(string grid)
        {
            return ExecuteAvamar("GetAvamar", grid, "sudo /usr/local/avamar/bin/mccli domain show --recursive=true");
        }
        [WebMethod(Description = "Get the clients of a domain")]
        public string GetAvamarDomainClients(string grid, string domain)
        {
            return ExecuteAvamar("GetAvamar", grid, "sudo /usr/local/avamar/bin/mccli client show --domain=" + domain);
        }

        // GROUP
        [WebMethod(Description = "Get the groups for the grid (group show)")]
        public string GetAvamarGroups(string grid)
        {
            return ExecuteAvamar("GetAvamar", grid, "sudo /usr/local/avamar/bin/mccli group show --verbose=true --recursive=true");
        }
        [WebMethod(Description = "Registers a client to the grid and adds it to the default group")]
        public string AddAvamarGroup(string grid, string fully_qualified_domain_name, string fully_qualified_client_name, string fully_qualified_group_name)
        {
            // to be done third.
            return ExecuteAvamar("AddAvamar", grid, "sudo /usr/local/avamar/bin/mccli group add-client --client-domain=" + fully_qualified_domain_name + " --client-name=" + fully_qualified_client_name + " --name=" + fully_qualified_group_name);
        }
        [WebMethod(Description = "Get the clients of a group")]
        public string GetAvamarGroupClients(string grid, string fully_qualified_group_name)
        {
            return ExecuteAvamar("GetAvamar", grid, "sudo /usr/local/avamar/bin/mccli group show --clients=true --recursive=true --verbose=true --name=" + fully_qualified_group_name);
        }
        [WebMethod(Description = "Removes the group from a client")]
        public string DeleteAvamarGroup(string grid, string fully_qualified_domain_name, string fully_qualified_client_name, string fully_qualified_group_name)
        {
            return ExecuteAvamar("RemoveAvamar", grid, "sudo /usr/local/avamar/bin/mccli group remove-client --client-domain=" + fully_qualified_domain_name + " --client-name=" + fully_qualified_client_name + " --name=" + fully_qualified_group_name);
        }

        // BACKUP
        [WebMethod(Description = "Get the output of a backup that was started")]
        public string GetAvamarBackup(string grid, string fully_qualified_domain_name, string fully_qualified_client_name)
        {
            return ExecuteAvamar("GetAvamar", grid, "sudo /usr/local/avamar/bin/mccli backup show --domain=" + fully_qualified_domain_name + " --name=" + fully_qualified_client_name);
        }
        [WebMethod(Description = "Starts an on-demand backup")]
        public string StartAvamarBackup(string grid, string fully_qualified_domain_name, string fully_qualified_group_name, string fully_qualified_client_name)
        {
            return ExecuteAvamar("AddAvamar", grid, "sudo /usr/local/avamar/bin/mccli client backup-group-dataset --domain=" + fully_qualified_domain_name + " --group-name=" + fully_qualified_group_name + " --name=" + fully_qualified_client_name);
        }

        private string ExecuteAvamar(string strWebMethodName, string grid, string command)
        {
            try
            {
                Avamar avamar = new Avamar(strWebMethodName);
                return avamar.RunCommand(grid, command);
            }
            catch (SoapException ex)
            {
                throw ex;
            }
        }
        #endregion



        // ************************************************************************************************************************
        // ************************************************************************************************************************
        //          POWERSHELL
        // ************************************************************************************************************************
        // ************************************************************************************************************************
        #region POWERSHELL
        [WebMethod(Description = "Configure a server's IP Address")]
        public string ConfigureWindowsIPAddress(int intAnswer, int intNumber, string ScriptEnvironment, string strIP, string[] strServersDNS, string strMACAddress, string strName, string strSerial)
        {
            try
            {
                Powershell powershell = new Powershell("ServerProcessing");
                return powershell.ServerProcessing(intAnswer, intNumber, ScriptEnvironment, strIP, strServersDNS, strMACAddress, strName, strSerial);
            }
            catch (SoapException ex)
            {
                throw ex;
            }
        }
        #endregion



        // ************************************************************************************************************************
        // ************************************************************************************************************************
        //          SERVICENOW
        // ************************************************************************************************************************
        // ************************************************************************************************************************
        #region SERVICENOW
        [WebMethod(Description = "Create a server entry in ServiceNow")]
        public string CreateServiceNowServer(string URL, string Domain, string IP, string Manufacturer, string Mnemonic, string Model, string Name, string ZeusBuildTypeForOperatingSystem, string Serial, string ServiceNowClass, string ServiceNowEnvironment, bool IsVirtual, DateTime Installed, string ServiceNowLocation, string BuildingCodeForDataCenter, string Username, string Password)
        {
            string error = "";
            try
            {
                string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
                Log oLog = new Log(0, dsn);
                oLog.AddEvent(Name, "ServiceNow", "Adding Record...", LoggingType.Debug);

                ServiceNowInsert insert = new ServiceNowInsert();
                insert.u_discovery_source = "ClearView";
                insert.u_fqdn = Name + "." + Domain;
                insert.u_install_status = ((ServiceNowEnvironment == ServiceNowEnvironments.Production || ServiceNowEnvironment == ServiceNowEnvironments.DR) ? ServiceNowInstallStatus.PendingInstall : ServiceNowInstallStatus.Installed);
                insert.u_ip_address = IP;
                insert.u_manufacturer = Manufacturer;
                insert.u_associated_mnemonic = Mnemonic;
                insert.u_model_id = Model;
                insert.u_name = Name;
                insert.u_os = ZeusBuildTypeForOperatingSystem;
                insert.u_serial_number = Serial;
                insert.u_sys_class_name = ServiceNowClass;
                insert.u_environment = ServiceNowEnvironment;
                insert.u_virtual = (IsVirtual ? "true" : "false");
                insert.u_dns_domain = Domain;
                insert.u_install_date = Installed.ToString("yyyy-MM-dd hh:mm:ss");
                insert.u_company = "PNC";
                insert.u_location = ServiceNowLocation;
                insert.u_data_center = BuildingCodeForDataCenter;
                insert.u_rack = "Virtual";

                string json = JSON.SerializeJSON<ServiceNowInsert>(insert);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL);
                    var byteArray = Encoding.ASCII.GetBytes(Username + ":" + Password);    // _clearview_user:_clearview_user
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    var message = new HttpRequestMessage
                    {
                        Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };
                    message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage post = client.PostAsync("/api/now/v1/import/imp_server", message.Content).Result;
                    var content = post.Content.ReadAsStringAsync();
                    oLog.AddEvent(Name, "ServiceNow", content.Result, LoggingType.Debug);
                    if (post.IsSuccessStatusCode)
                    {
                        ServiceNowResponse response = JSON.DeserializeJSON<ServiceNowResponse>(content.Result);
                        foreach (ServiceNowResponseResult result in response.result)
                        {
                            string status = result.status.Trim().ToLower();
                            if (status != "updated" && status != "inserted" && status != "ignored")
                            {
                                error = "There was a problem inserting the record into Service Now - status = " + status;
                                break;
                            }
                        }
                    }
                    else if (String.IsNullOrWhiteSpace(content.Result) == false)
                        error = content.Result;
                    else
                        error = post.ReasonPhrase + " (" + (int)post.StatusCode + ")";
                }
            }
            catch (Exception ex)
            {
                error += ex.Message;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    error += " ~ " + ex.Message;
                }
            }
            return error;
        }

        [WebMethod(Description = "Get a server entry in ServiceNow")]
        public string GetServiceNowServer(string URL, string Username, string Password, string Name)
        {
            ServiceNow oServiceNow = new ServiceNow();
            return oServiceNow.GetServer(URL, Username, Password, Name);
        }
        [WebMethod(Description = "Decom a server entry in ServiceNow")]
        public string DecomServiceNowServer(string URL, string Username, string Password, string Name)
        {
            ServiceNow oServiceNow = new ServiceNow();
            return oServiceNow.DecomServer(URL, Username, Password, Name, ServiceNowDecom.Decommission);
        }
        [WebMethod(Description = "Destroy a server entry in ServiceNow")]
        public string DestroyServiceNowServer(string URL, string Username, string Password, string Name)
        {
            ServiceNow oServiceNow = new ServiceNow();
            return oServiceNow.DecomServer(URL, Username, Password, Name, ServiceNowDecom.Destroy);
        }
        [WebMethod(Description = "Recommission a server entry in ServiceNow")]
        public string RecomServiceNowServer(string URL, string Username, string Password, string Name)
        {
            ServiceNow oServiceNow = new ServiceNow();
            return oServiceNow.DecomServer(URL, Username, Password, Name, ServiceNowDecom.Recommission);
        }


        [WebMethod(Description = "Get an incident entry in ServiceNow")]
        public string GetServiceNowIncidentNumber(string URL, string Username, string Password, string Number)
        {
            ServiceNow oServiceNow = new ServiceNow();
            return oServiceNow.GetIncident(URL, Username, Password, Number);
        }
        [WebMethod(Description = "Get an incident entry in ServiceNow")]
        public string GetServiceNowIncident(string URL, string Username, string Password, int ErrorID, string What)
        {
            ServiceNow oServiceNow = new ServiceNow();
            return oServiceNow.GetIncident(URL, Username, Password, ErrorID, What);
        }
        #endregion



        // ************************************************************************************************************************
        // ************************************************************************************************************************
        //          COLLABORATION
        // ************************************************************************************************************************
        // ************************************************************************************************************************
        #region COLLABORATION
        [WebMethod(Description = "Get the path to a vmware machine")]
        public string GetVMwarePath(string Name, string URL, string Token)
        {
            string path = "";
            try
            {
                string json = "{\"token\":\"" + Token + "\", \"Name\":\"" + Name + "\"}";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL);
                    var inputMessage = new HttpRequestMessage
                    {
                        Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };
                    inputMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage post = client.PostAsync("VirtualCollector", inputMessage.Content).Result;
                    if (post.IsSuccessStatusCode)
                    {
                        var content = post.Content.ReadAsStringAsync();
                        path = content.Result;
                    }
                    else
                        path = "!" + post.ReasonPhrase + " (" + (int)post.StatusCode + ")";
                }
            }
            catch (Exception ex)
            {
                path = "!" + ex.Message;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    path += " ~ " + ex.Message;
                }
            }
            return path;
        }
        #endregion
    }
}
