using System;
using System.Management;
using System.Collections.Generic;
using System.Text;

namespace NCC.ClearView.Application.Core
{
    public class WMIConnection
    {
        ManagementScope connectionScope;
        ConnectionOptions options;

        public ManagementScope GetConnectionScope
        {
            get { return connectionScope; }
        }
        public ConnectionOptions GetOptions
        {
            get { return options; }
        }
        public static ConnectionOptions SetConnectionOptions()
        {
            ConnectionOptions options = new ConnectionOptions();
            options.Impersonation = ImpersonationLevel.Impersonate;
            //options.Authentication = AuthenticationLevel.Default;
            options.EnablePrivileges = true;
            return options;
        }

        public static ManagementScope SetConnectionScope(string machineName, ConnectionOptions options, string location)
        {
            ManagementScope connectScope = new ManagementScope();
            connectScope.Path = new ManagementPath(@"\\" + machineName + @"\root\" + location);
            connectScope.Options = options;
            connectScope.Connect();
            return connectScope;
        }
        public WMIConnection()
        {
            EstablishConnection(null, null, null, System.Environment.MachineName, "CIMV2");
        }

        public WMIConnection(string userName, string password, string domain, string machineName, string location)
        {
            if (location == "")
                EstablishConnection(userName, password, domain, machineName, "CIMV2");
            else
                EstablishConnection(userName, password, domain, machineName, location);
        }
        private void EstablishConnection(string userName, string password, string domain, string machineName, string location)
        {
            options = SetConnectionOptions();
            if (domain != null || userName != null)
            {
                options.Username = domain + "\\" + userName;
                options.Password = password;
            }
            connectionScope = SetConnectionScope(machineName, options, location);
        }
    }
}
