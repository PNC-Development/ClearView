using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;
using Tamir.SharpSsh;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


namespace NCC.ClearView.Application.Core
{

    public class ManageSQLJobs
    {
        private string dsnDW = "";
        private int intUser = 0;

        private SqlParameter[] arParams;



        public ManageSQLJobs(int _user, string _dsnDW)
        {
            dsnDW = _dsnDW;
            intUser = _user;
        }

        public DataSet GetJobDetails(string _jobName)
        {
            return SqlHelper.ExecuteDataset(dsnDW, CommandType.Text, "exec msdb.dbo.sp_help_job @job_name = N'" + _jobName + "'", arParams);

        }

        public DataSet RunJob(string _jobName)
        {
            return SqlHelper.ExecuteDataset(dsnDW, CommandType.Text, "EXEC msdb.dbo.sp_start_job N'" + _jobName + "'", arParams);

        }


    }
}
