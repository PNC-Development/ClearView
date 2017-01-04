using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class WindowsService
	{
		private string dsn = "";
		private int user = 0;
        public WindowsService(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
	}
}
