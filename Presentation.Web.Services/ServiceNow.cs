using NCC.ClearView.Application.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Presentation.Web.Services
{
    public class ServiceNow
    {
        protected string dsn { get; set; }
        protected string dsnAsset { get; set; }

        public ServiceNow()
        {
            dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
            dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        }
        public string GetServer(string URL, string Username, string Password, string Name)
        {
            string error = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL);
                    var byteArray = Encoding.ASCII.GetBytes(Username + ":" + Password);    // _clearview_user:_clearview_user
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    var message = new HttpRequestMessage
                    {
                        //Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };
                    message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage post = client.GetAsync("/api/now/v1/table/cmdb_ci_server?sysparm_query=name=" + Name).Result;
                    var content = post.Content.ReadAsStringAsync();
                    if (post.IsSuccessStatusCode)
                    {
                        byte[] bytes = Encoding.ASCII.GetBytes(content.Result);
                        using (var stream = new MemoryStream(bytes))
                        {
                            var quotas = new XmlDictionaryReaderQuotas();
                            var jsonReader = JsonReaderWriterFactory.CreateJsonReader(stream, quotas);
                            var xml = XDocument.Load(jsonReader);
                            error = xml.ToString();
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
        public string DecomServer(string URL, string Username, string Password, string Name, ServiceNowDecom Decom)
        {
            XmlDocument doc = new XmlDocument();
            string error = GetServer(URL, Username, Password, Name);
            try
            {
                doc.LoadXml(error);

                error = "";
                try
                {
                    XmlNodeList nodes = doc.SelectNodes("/root/result/item");
                    if (nodes.Count > 0)
                    {
                        Log oLog = new Log(0, dsn);
                        Asset oAsset = new Asset(0, dsnAsset);

                        ServiceNowUpdate update = new ServiceNowUpdate();
                        update.u_action = "update";
                        update.u_name = nodes[0]["name"].InnerText;
                        oLog.AddEvent(update.u_name, "ServiceNow", "Updating Record...", LoggingType.Debug);
                        if (Decom == ServiceNowDecom.Decommission)
                        {
                            oLog.AddEvent(update.u_name, "ServiceNow", "Decommission", LoggingType.Debug);
                            int OperationalState = 0;
                            if (Int32.TryParse(nodes[0]["u_desired_operational_state"].InnerText, out OperationalState) == true)
                            {
                                oLog.AddEvent(update.u_name, "ServiceNow", "Desired Operational State (from Service Now) = " + OperationalState.ToString(), LoggingType.Debug);
                                oAsset.UpdateDecommissionServiceNowOperationalStatus(update.u_name, OperationalState);
                            }
                            update.u_desired_operational_state = ServiceNowDesiredOperationalStatus.PendingDecommission;
                        }
                        else if (Decom == ServiceNowDecom.Destroy)
                        {
                            oLog.AddEvent(update.u_name, "ServiceNow", "Destroy", LoggingType.Debug);
                            update.u_install_status = ServiceNowInstallStatus.Retired;
                            update.u_desired_operational_state = ServiceNowDesiredOperationalStatus.Decommissioned;
                        }
                        else if (Decom == ServiceNowDecom.Recommission)
                        {
                            oLog.AddEvent(update.u_name, "ServiceNow", "Recommission", LoggingType.Debug);
                            DataSet dsDecom = oAsset.GetDecommission(update.u_name);
                            if (dsDecom.Tables[0].Rows.Count > 0)
                            {
                                int OperationalState = 0;
                                Int32.TryParse(dsDecom.Tables[0].Rows[0]["service_now_operational_status"].ToString(), out OperationalState);
                                oLog.AddEvent(update.u_name, "ServiceNow", "Desired Operational State (from ClearView DB) = " + OperationalState.ToString(), LoggingType.Debug);
                                if (OperationalState > 0)
                                    update.u_desired_operational_state = OperationalState.ToString();
                            }
                        }

                        string json = JSON.SerializeJSON<ServiceNowUpdate>(update);
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
                            oLog.AddEvent(update.u_name, "ServiceNow", content.Result, LoggingType.Debug);
                            if (post.IsSuccessStatusCode)
                            {
                                ServiceNowResponse response = JSON.DeserializeJSON<ServiceNowResponse>(content.Result);
                                foreach (ServiceNowResponseResult result in response.result)
                                {
                                    string status = result.status.Trim().ToLower();
                                    if (status != "updated" && status != "ignored")
                                    {
                                        error = "There was a problem updating the record in Service Now - status = " + status;
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
                    else if (nodes.Count == 0)
                    {
                        // No error message since it doesn't exist.
                    }
                    else
                    {
                        error = "More than one record was returned (" + nodes.Count.ToString() + ")";
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
            catch
            {
                return error;
            }
        }



        public string GetIncident(string URL, string Username, string Password, int ErrorID, string What)
        {
            return GetIncidentQuery(URL, Username, Password, String.Format("/api/now/table/incident?sysparm_query=u_event_parameter={0}^u_event_what={1}&sysparm_limit={2}", ErrorID, What, 2));
        }
        public string GetIncident(string URL, string Username, string Password, string Number)
        {
            return GetIncidentQuery(URL, Username, Password, String.Format("/api/now/table/incident?sysparm_query=number={0}&sysparm_limit={1}", Number, 2));
        }
        private string GetIncidentQuery(string URL, string Username, string Password, string Query)
        {
            string error = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL);
                    var byteArray = Encoding.ASCII.GetBytes(Username + ":" + Password);    // _clearview_user:_clearview_user
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    var message = new HttpRequestMessage
                    {
                        //Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };
                    message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage post = client.GetAsync(Query).Result;
                    var content = post.Content.ReadAsStringAsync();
                    if (post.IsSuccessStatusCode)
                    {
                        ServiceNowIncidents response = JSON.DeserializeJSON<ServiceNowIncidents>(content.Result);
                        if (response.result.Count != 1)
                            error = "!" + "A unique record could not be found (" + response.result.Count.ToString() + ")";
                        else
                        {
                            foreach (ServiceNowIncident result in response.result)
                            {
                                string incident = result.number;
                                string state = result.state.Trim().ToLower();
                                string assigned = GetReferrer(URL, Username, Password, result.assigned_to.link, "user_name");
                                if (result.assigned_to.link == null)
                                {
                                    // Nobody assigned - get owner of group.
                                    string u_manager_1 = GetReferrer(URL, Username, Password, result.assignment_group.link, "u_manager_1");
                                    assigned = GetReferrer(URL, Username, Password, u_manager_1, "user_name");
                                }
                                string group = GetReferrer(URL, Username, Password, result.assignment_group.link);
                                string resolved = GetReferrer(URL, Username, Password, result.resolved_by.link, "user_name");
                                error = String.Format("{0}|{1}|{2}|{3}|{4}", incident, state, assigned, group, resolved);
                            }
                        }
                    }
                    else if (String.IsNullOrWhiteSpace(content.Result) == false)
                        error = "!" + content.Result;
                    else
                        error = "!" + post.ReasonPhrase + " (" + (int)post.StatusCode + ")";
                }
            }
            catch (Exception ex)
            {
                if (error == "")
                    error = "!";
                error += ex.Message;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    error += " ~ " + ex.Message;
                }
            }
            return error;
        }
        private string GetReferrer(string URL, string Username, string Password, string Link)
        {
            return GetReferrer(URL, Username, Password, Link, "name");
        }
        private string GetReferrer(string URL, string Username, string Password, string Link, string Field)
        {
            string error = "";
            if (Link == null)
                return "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL);
                    var byteArray = Encoding.ASCII.GetBytes(Username + ":" + Password);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    var message = new HttpRequestMessage
                    {
                        //Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };
                    message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage post = client.GetAsync(Link.Substring(Link.IndexOf("/api/now"))).Result;
                    var content = post.Content.ReadAsStringAsync();
                    if (post.IsSuccessStatusCode)
                    {
                        XDocument xml = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(Encoding.ASCII.GetBytes(content.Result), new XmlDictionaryReaderQuotas()));
                        foreach (XElement xe in xml.Descendants("result"))
                        {
                            if (xe.Element(Field) != null)
                            {
                                if (xe.Element(Field).HasElements)
                                    error = xe.Element(Field).Element("link").Value;
                                else
                                    error = xe.Element(Field).Value;
                            }
                        }
                    }
                    else if (String.IsNullOrWhiteSpace(content.Result) == false)
                        error = "!" + content.Result;
                    else
                        error = "!" + post.ReasonPhrase + " (" + (int)post.StatusCode + ")";
                }
            }
            catch (Exception ex)
            {
                if (error == "")
                    error = "!";
                error += ex.Message;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    error += " ~ " + ex.Message;
                }
            }
            return error;
        }

    }
}