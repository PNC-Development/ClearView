using System;
using System.Collections.Generic;
using System.Text;

namespace NCC.ClearView.Data.Reporting.Refresh
{
    /// <summary>
    /// This program is responsible for refreshing the reporting databases (the ClearView 
    /// data warehouse) with data from the ClearView transactional databases.  This program 
    /// will be executed via a scheduled job on the reporting database's server.
    /// </summary>
    public class RefreshDatabase
    {
        static void Main(string[] args)
        {
            DataRefresh dataRefresh = new DataRefresh("TransactionCoreDatabase", "ReportingCoreDatabase");
            dataRefresh.Execute();

            dataRefresh = new DataRefresh("TransactionAssetDatabase", "ReportingAssetDatabase");
            dataRefresh.Execute();

            dataRefresh = new DataRefresh("TransactionIPDatabase", "ReportingIPDatabase");
            dataRefresh.Execute();
        }
    }
}
