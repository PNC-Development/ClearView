using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;
using NCC.ClearView.Application.Core.ClearViewWS;
using System.Xml;
using System.Collections.Generic;
using System.Threading;

namespace NCC.ClearView.Application.Core
{
	public class AvamarActivation
	{
		private string dsn = "";
		private int user = 0;
        private string strResults { get; set; }
        public AvamarActivation(int _user, string _dsn)
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

        public void Activations(int EnvironmentID)
        {
            // Setup Classes
            Servers oServer = new Servers(0, dsn);
            AvamarRegistration oAvamarRegistration = new AvamarRegistration(0, dsn);
            Log oLog = new Log(0, dsn);
            Variables oVariable = new Variables(EnvironmentID);

            // Setup Webservice for querying via SSH
            System.Net.NetworkCredential oCredentialsDNS = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
            ClearViewWebServices oWebService = new ClearViewWebServices();
            oWebService.Url = oVariable.WebServiceURL();

            DataSet dsActivations = oServer.GetAvamarActivations();
            if (dsActivations.Tables[0].Rows.Count > 0)
            {
                oLog.AddEvent("", "", "Get avamar activations (" + dsActivations.Tables[0].Rows.Count.ToString() + ")", LoggingType.Debug);
                foreach (DataRow drActivation in dsActivations.Tables[0].Rows)
                {
                    ClearResults();

                    int intServer = Int32.Parse(drActivation["id"].ToString());
                    oServer.UpdateAvamarActivationStarted(intServer, DateTime.Now.ToString());
                    int intAnswer = Int32.Parse(drActivation["answerid"].ToString());
                    string strName = drActivation["servername"].ToString();
                    string strGrid = drActivation["grid"].ToString();
                    string strDomain = drActivation["domain"].ToString();
                    string strGroup1 = drActivation["group1"].ToString();
                    string strGroup2 = drActivation["group2"].ToString();
                    string strGroup3 = drActivation["group3"].ToString();

                    oLog.AddEvent(intAnswer, strName, "", "Starting automated Avamar activation", LoggingType.Information);
                    string strError = "";
                    string strSuccess = "";

                    try
                    {
                        // Activate client
                        AvamarReturnType activate = oAvamarRegistration.API(oWebService.ActivateAvamarClient(strGrid, strDomain, strName));
                        if (activate.Error == false)
                        {
                            strSuccess = activate.Message;
                            // Wait 1/2 minute for synchronization
                            oLog.AddEvent(intAnswer, strName, "", "Activation script complete = " + activate.Message + ".", LoggingType.Information);
                            bool HasActivated = false;
                            for (int ii = 0; ii < 10 && HasActivated == false; ii++)
                            {
                                int activateCount = ii + 1;
                                oLog.AddEvent(intAnswer, strName, "", "Checking activation status (" + activateCount.ToString() + " of 10)...", LoggingType.Debug);
                                AvamarReturnType activated = oAvamarRegistration.API(oWebService.GetAvamarClient(strGrid, strDomain, strName));
                                if (activated.Error == false)
                                {
                                    foreach (XmlNode node in activated.Nodes)
                                    {
                                        if (node["Attribute"].InnerText == "Activated")
                                        {
                                            if (node["Value"].InnerText.Trim().ToUpper() == "YES")
                                            {
                                                HasActivated = true;
                                                oLog.AddEvent(intAnswer, strName, "", "Client has activated!", LoggingType.Information);
                                            }
                                            else
                                            {
                                                oLog.AddEvent(intAnswer, strName, "", "Client is not active. Waiting 30 seconds before retrying...", LoggingType.Debug);
                                                Thread.Sleep(30000);
                                            }
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    strError = "Error while activating - " + activated.Message;
                                    break;
                                }
                            }

                            if (String.IsNullOrEmpty(strError))
                            {
                                if (HasActivated)
                                {
                                    // Initiate backup
                                    oLog.AddEvent(intAnswer, strName, "", "Initiating backup...", LoggingType.Information);
                                    AvamarReturnType start = oAvamarRegistration.API(oWebService.StartAvamarBackup(strGrid, strDomain, strGroup1, strName));
                                    if (start.Error)
                                        strError = start.Message + " (" + start.Code + ")";
                                    else if (String.IsNullOrEmpty(strGroup2) == false)
                                    {
                                        oLog.AddEvent(intAnswer, strName, "", "Backup for " + strGroup1 + " started = " + start.Message, LoggingType.Information);
                                        AvamarReturnType start2 = oAvamarRegistration.API(oWebService.StartAvamarBackup(strGrid, strDomain, strGroup2, strName));
                                        if (start2.Error)
                                            strError = start2.Message + " (" + start2.Code + ")";
                                        else if (String.IsNullOrEmpty(strGroup3) == false)
                                        {
                                            oLog.AddEvent(intAnswer, strName, "", "Backup for " + strGroup2 + " started = " + start2.Message, LoggingType.Information);
                                            AvamarReturnType start3 = oAvamarRegistration.API(oWebService.StartAvamarBackup(strGrid, strDomain, strGroup3, strName));
                                            if (start3.Error)
                                                strError = start3.Message + " (" + start3.Code + ")";
                                            else
                                                oLog.AddEvent(intAnswer, strName, "", "Backup for " + strGroup3 + " started = " + start3.Message, LoggingType.Information);
                                        }
                                    }
                                }
                                else
                                    strError = "Client has not activated after 5 minutes";
                            }
                        }
                        else
                            strError = activate.Message + " (" + activate.Code + ")";
                    }
                    catch (Exception exError)
                    {
                        strError = exError.Message;
                        if (exError.InnerException != null)
                            strError += " ~ " + exError.InnerException.Message;
                    }

                    if (strError != "")
                    {
                        oLog.AddEvent(intAnswer, strName, "", strError, LoggingType.Error);
                        oServer.UpdateAvamarActivationCompleted(intServer, "", strError, DateTime.Now.ToString(), 1);
                        oServer.AddError(0, 0, 0, intServer, 906, strError);
                    }
                    else
                    {
                        oLog.AddEvent(intAnswer, strName, "", "Activation completed and backup initiated", LoggingType.Information);
                        oServer.UpdateAvamarActivationCompleted(intServer, "", strSuccess, DateTime.Now.ToString(), 0);
                        oServer.AddAvamarBackup(intServer, strName, strGrid, strDomain, strGroup1);
                    }
                }
            }
        }
    }
}
