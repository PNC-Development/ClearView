using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Web;
using Microsoft.Reporting.WebForms;
using System.Net;
using System.Security.Principal;

namespace NCC.ClearView.Application.Core
{
    public class ReportCredentials : IReportServerCredentials
	{
		private string dsn = "";
		private int user = 0;
        private int intEnvironment;
        public ReportCredentials(int _user, string _dsn, int _environment)
		{
			user = _user;
			dsn = _dsn;
            intEnvironment = _environment;
        }
        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;
            }
        }
        public ICredentials NetworkCredentials
        {
            get
            {
                Variables oVariable = new Variables(intEnvironment);
                return new NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
            }
        }
        public bool GetFormsCredentials(out Cookie authCookie, out string userName, out string password, out string authority)
        {
            authCookie = null;
            userName = null;
            password = null;
            authority = null;

            // Not using form credentials
            return false;
        }
    }
}
