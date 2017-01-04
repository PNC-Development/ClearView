using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;
using NCC.ClearView.Application.Core.ClearViewWS;
using System.Xml;
using System.Collections.Generic;

namespace NCC.ClearView.Application.Core
{
	public class AvamarBackup
	{
		private string dsn = "";
		private int user = 0;
        private string strResults { get; set; }
        public AvamarBackup(int _user, string _dsn)
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

        public void Backups(int EnvironmentID)
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

            DataSet dsBackups = oServer.GetAvamarBackups();
            if (dsBackups.Tables[0].Rows.Count > 0)
            {
                oLog.AddEvent("", "", "Get avamar backups (" + dsBackups.Tables[0].Rows.Count.ToString() + ")", LoggingType.Debug);
                foreach (DataRow drBackup in dsBackups.Tables[0].Rows)
                {
                    ClearResults();

                    int intServer = Int32.Parse(drBackup["id"].ToString());
                    DateTime datCreated = DateTime.Parse(drBackup["created"].ToString());
                    int intAnswer = Int32.Parse(drBackup["answerid"].ToString());
                    string strName = drBackup["servername"].ToString();
                    string strGrid = drBackup["grid"].ToString();
                    string strDomain = drBackup["domain"].ToString();
                    string strGroup = drBackup["group"].ToString();

                    oLog.AddEvent(intAnswer, strName, "", "Starting automated Avamar backup validation", LoggingType.Debug);
                    string strError = "";

                    // Initiate backup for client
                    AvamarReturnType backup = oAvamarRegistration.API(oWebService.GetAvamarBackup(strGrid, strDomain, strName));
                    if (backup.Error == false)
                    {
                        oLog.AddEvent(intAnswer, strName, "", "There are " + backup.Nodes.Count.ToString() + " backup(s).", LoggingType.Information);
                        if (backup.Nodes.Count > 0)
                        {
                            oLog.AddEvent(intAnswer, strName, "", "Backup has been validated", LoggingType.Information);
                            oServer.UpdateAvamarBackupCompleted(intServer, backup.Nodes[0].InnerXml, DateTime.Now.ToString(), 0);
                        }
                        else
                        {
                            // Check to see if a certain amount of time has passed and if so, throw error.
                            TimeSpan span = DateTime.Now.Subtract(datCreated);
                            if (span.TotalMinutes > 90)
                            {
                                // It's been an hour and a half.  Throw error.
                                strError = "The backup has still not completed after 90 minutes on grid " + strGrid;
                            }
                        }
                    }
                    else
                        strError = backup.Message + " (" + backup.Code + ")";

                    if (strError != "")
                    {
                        oLog.AddEvent(intAnswer, strName, "", strError, LoggingType.Error);
                        oServer.UpdateAvamarBackupCompleted(intServer, strError, DateTime.Now.ToString(), 1);
                        oServer.AddError(0, 0, 0, intServer, 906, strError);
                    }
                }
            }
        }
    }
}
