using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;
using System.Text;

namespace NCC.ClearView.Application.Core
{
	public class Customized
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Customized(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public void AddVirtualWorkstationAccount(int _requestid, int _itemid, int _number, int _workstationid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@workstationid", _workstationid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addRR_virtual_workstation_account", arParams);
        }
        public DataSet GetVirtualWorkstationAccount(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRR_virtual_workstation_account", arParams);
        }
        public DataSet GetTPMServiceRequestsApplication(int _projectid, int _applicationid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_tpm_ServiceRequestsApplication", arParams);
        }
        public DataSet GetTPMServiceRequests(int _projectid, int _itemid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_tpm_ServiceRequests", arParams);
        }
        public DataSet GetTPM(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_tpm", arParams);
        }
        public void AddTPM(int _requestid, int _itemid, int _number, int _priority, string _statement, DateTime _start_date, DateTime _end_date)
		{
			arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@priority", _priority);
            arParams[4] = new SqlParameter("@statement", _statement);
            arParams[5] = new SqlParameter("@start_date", _start_date);
            arParams[6] = new SqlParameter("@end_date", _end_date);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWM_tpm", arParams);
		}
        public void UpdateTPM(int _requestid, int _itemid, int _number, int _financials_exclude, string _start_d, string _end_d, string _start_p, string _end_p, string _start_e, string _end_e, string _start_c, string _end_c, string _costs, string _ppm, string _appsd, string _apped, string _appsp, string _appep, string _appse, string _appee, string _appsc, string _appec, string _appid, string _appexd, string _apphd, string _actid, string _acted, string _acthd, string _estid, string _ested, string _esthd, string _appip, string _appexp, string _apphp, string _actip, string _actep, string _acthp, string _estip, string _estep, string _esthp, string _appie, string _appexe, string _apphe, string _actie, string _actee, string _acthe, string _estie, string _estee, string _esthe, string _appic, string _appexc, string _apphc, string _actic, string _actec, string _acthc, string _estic, string _estec, string _esthc, string _sharepoint, string _better, string _worse, string _lessons)
        {
            arParams = new SqlParameter[62];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@financials_exclude", _financials_exclude);
            arParams[4] = new SqlParameter("@start_d", (_start_d == "" ? SqlDateTime.Null : DateTime.Parse(_start_d)));
            arParams[5] = new SqlParameter("@end_d", (_end_d == "" ? SqlDateTime.Null : DateTime.Parse(_end_d)));
            arParams[6] = new SqlParameter("@start_p", (_start_p == "" ? SqlDateTime.Null : DateTime.Parse(_start_p)));
            arParams[7] = new SqlParameter("@end_p", (_end_p == "" ? SqlDateTime.Null : DateTime.Parse(_end_p)));
            arParams[8] = new SqlParameter("@start_e", (_start_e == "" ? SqlDateTime.Null : DateTime.Parse(_start_e)));
            arParams[9] = new SqlParameter("@end_e",(_end_e == "" ? SqlDateTime.Null : DateTime.Parse(_end_e))); 
            arParams[10] = new SqlParameter("@start_c", (_start_c == "" ? SqlDateTime.Null : DateTime.Parse(_start_c)));
            arParams[11] = new SqlParameter("@end_c", (_end_c == "" ? SqlDateTime.Null : DateTime.Parse(_end_c)));
            arParams[12] = new SqlParameter("@costs", _costs);
            arParams[13] = new SqlParameter("@ppm", _ppm);
            arParams[14] = new SqlParameter("@appsd", (_appsd == "" ? SqlDateTime.Null : DateTime.Parse(_appsd)));
            arParams[15] = new SqlParameter("@apped", (_apped == "" ? SqlDateTime.Null : DateTime.Parse(_apped)));
            arParams[16] = new SqlParameter("@appsp", (_appsp == "" ? SqlDateTime.Null : DateTime.Parse(_appsp)));
            arParams[17] = new SqlParameter("@appep", (_appep == "" ? SqlDateTime.Null : DateTime.Parse(_appep)));
            arParams[18] = new SqlParameter("@appse", (_appse == "" ? SqlDateTime.Null : DateTime.Parse(_appse)));
            arParams[19] = new SqlParameter("@appee", (_appee == "" ? SqlDateTime.Null : DateTime.Parse(_appee)));
            arParams[20] = new SqlParameter("@appsc", (_appsc == "" ? SqlDateTime.Null : DateTime.Parse(_appsc)));
            arParams[21] = new SqlParameter("@appec", (_appec == "" ? SqlDateTime.Null : DateTime.Parse(_appec)));
            arParams[22] = new SqlParameter("@appid", (_appid == "" ? 0.00 : double.Parse(_appid)));
            arParams[23] = new SqlParameter("@appexd", (_appexd == "" ? 0.00 : double.Parse(_appexd)));
            arParams[24] = new SqlParameter("@apphd", (_apphd == "" ? 0.00 : double.Parse(_apphd)));
            arParams[25] = new SqlParameter("@actid", (_actid == "" ? 0.00 : double.Parse(_actid)));
            arParams[26] = new SqlParameter("@acted", (_acted == "" ? 0.00 : double.Parse(_acted)));
            arParams[27] = new SqlParameter("@acthd", (_acthd == "" ? 0.00 : double.Parse(_acthd)));
            arParams[28] = new SqlParameter("@estid", (_estid == "" ? 0.00 : double.Parse(_estid)));
            arParams[29] = new SqlParameter("@ested", (_ested == "" ? 0.00 : double.Parse(_ested)));
            arParams[30] = new SqlParameter("@esthd", (_esthd == "" ? 0.00 : double.Parse(_esthd)));
            arParams[31] = new SqlParameter("@appip", (_appip == "" ? 0.00 : double.Parse(_appip)));
            arParams[32] = new SqlParameter("@appexp", (_appexp == "" ? 0.00 : double.Parse(_appexp)));
            arParams[33] = new SqlParameter("@apphp", (_apphp == "" ? 0.00 : double.Parse(_apphp)));
            arParams[34] = new SqlParameter("@actip", (_actip == "" ? 0.00 : double.Parse(_actip)));
            arParams[35] = new SqlParameter("@actep", (_actep == "" ? 0.00 : double.Parse(_actep)));
            arParams[36] = new SqlParameter("@acthp", (_acthp == "" ? 0.00 : double.Parse(_acthp)));
            arParams[37] = new SqlParameter("@estip", (_estip == "" ? 0.00 : double.Parse(_estip)));
            arParams[38] = new SqlParameter("@estep", (_estep == "" ? 0.00 : double.Parse(_estep)));
            arParams[39] = new SqlParameter("@esthp", (_esthp == "" ? 0.00 : double.Parse(_esthp)));
            arParams[40] = new SqlParameter("@appie", (_appie == "" ? 0.00 : double.Parse(_appie)));
            arParams[41] = new SqlParameter("@appexe", (_appexe == "" ? 0.00 : double.Parse(_appexe)));
            arParams[42] = new SqlParameter("@apphe", (_apphe == "" ? 0.00 : double.Parse(_apphe)));
            arParams[43] = new SqlParameter("@actie", (_actie == "" ? 0.00 : double.Parse(_actie)));
            arParams[44] = new SqlParameter("@actee", (_actee == "" ? 0.00 : double.Parse(_actee)));
            arParams[45] = new SqlParameter("@acthe", (_acthe == "" ? 0.00 : double.Parse(_acthe)));
            arParams[46] = new SqlParameter("@estie", (_estie == "" ? 0.00 : double.Parse(_estie)));
            arParams[47] = new SqlParameter("@estee", (_estee == "" ? 0.00 : double.Parse(_estee)));
            arParams[48] = new SqlParameter("@esthe", (_esthe == "" ? 0.00 : double.Parse(_esthe)));
            arParams[49] = new SqlParameter("@appic", (_appic == "" ? 0.00 : double.Parse(_appic)));
            arParams[50] = new SqlParameter("@appexc", (_appexc == "" ? 0.00 : double.Parse(_appexc)));
            arParams[51] = new SqlParameter("@apphc", (_apphc == "" ? 0.00 : double.Parse(_apphc)));
            arParams[52] = new SqlParameter("@actic", (_actic == "" ? 0.00 : double.Parse(_actic)));
            arParams[53] = new SqlParameter("@actec", (_actec == "" ? 0.00 : double.Parse(_actec)));
            arParams[54] = new SqlParameter("@acthc", (_acthc == "" ? 0.00 : double.Parse(_acthc)));
            arParams[55] = new SqlParameter("@estic", (_estic == "" ? 0.00 : double.Parse(_estic)));
            arParams[56] = new SqlParameter("@estec", (_estec == "" ? 0.00 : double.Parse(_estec)));
            arParams[57] = new SqlParameter("@esthc", (_esthc == "" ? 0.00 : double.Parse(_esthc)));
            arParams[58] = new SqlParameter("@sharepoint", _sharepoint);
            arParams[59] = new SqlParameter("@better", _better);
            arParams[60] = new SqlParameter("@worse", _worse);
            arParams[61] = new SqlParameter("@lessons", _lessons);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWM_tpm", arParams);
        }

        public DataSet GetRemediationRequests(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_remediationRequests", arParams);
        }
        public DataSet GetRemediation(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_remediation", arParams);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["number"].ToString() != "")
                {
                    try
                    {
                        ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                        DataSet dsResource = oResourceRequest.Get(_requestid, _itemid, _number);
                        foreach (DataRow drResource in dsResource.Tables[0].Rows)
                        {
                            int intResourceWorkflow = Int32.Parse(drResource["id"].ToString());
                            oResourceRequest.AddChangeControl(intResourceWorkflow, ds.Tables[0].Rows[0]["cc_number"].ToString(), DateTime.Parse(ds.Tables[0].Rows[0]["cc_date"].ToString() + " " + ds.Tables[0].Rows[0]["cc_time"].ToString()), ds.Tables[0].Rows[0]["cc_comments"].ToString());
                        }
                    }
                    catch {}
                    SqlHelper.ExecuteDataset(dsn, CommandType.Text, "UPDATE cv_WM_remediation SET number = '' WHERE requestid = " + _requestid + " AND itemid = " + _itemid + " AND number = " + _number);
                }
            }
            return ds;
        }
        public void AddRemediation(int _requestid, int _itemid, int _number, string _reason, string _component, string _funding, int _priority, int _tpm, string _statement, int _devices, double _hours, DateTime _start_date, DateTime _end_date, string _cc_number, string _cc_date, string _cc_time, string _cc_comments)
        {
            arParams = new SqlParameter[17];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@reason", _reason);
            arParams[4] = new SqlParameter("@component", _component);
            arParams[5] = new SqlParameter("@funding", _funding);
            arParams[6] = new SqlParameter("@priority", _priority);
            arParams[7] = new SqlParameter("@tpm", _tpm);
            arParams[8] = new SqlParameter("@statement", _statement);
            arParams[9] = new SqlParameter("@devices", _devices);
            arParams[10] = new SqlParameter("@hours", _hours);
            arParams[11] = new SqlParameter("@start_date", _start_date);
            arParams[12] = new SqlParameter("@end_date", _end_date);
            arParams[13] = new SqlParameter("@cc_number", _cc_number);
            arParams[14] = new SqlParameter("@cc_date", _cc_date);
            arParams[15] = new SqlParameter("@cc_time", _cc_time);
            arParams[16] = new SqlParameter("@cc_comments", _cc_comments);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWM_remediation", arParams);
        }
        public void AddServerArchive(int _requestid, int _itemid, int _number, string _servername, int _modelid, string _appcode, int _classid, DateTime _end_date, string _statement)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@servername", _servername);
            arParams[4] = new SqlParameter("@modelid", _modelid);
            arParams[5] = new SqlParameter("@appcode", _appcode);
            arParams[6] = new SqlParameter("@classid", _classid);
            arParams[7] = new SqlParameter("@end_date", _end_date);
            arParams[8] = new SqlParameter("@statement", _statement);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWM_ServerArchive", arParams);
        }
        public void UpdateServerArchiveT(int _requestid, int _itemid, int _number, int _T1, int _T2, int _T3)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@T1", _T1);
            arParams[4] = new SqlParameter("@T2", _T2);
            arParams[5] = new SqlParameter("@T3", _T3);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWM_ServerArchiveT", arParams);
        }
        public void UpdateServerArchiveG(int _requestid, int _itemid, int _number, int _G1, int _G2, int _G3, int _G4, int _G5, int _G6)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@G1", _G1);
            arParams[4] = new SqlParameter("@G2", _G2);
            arParams[5] = new SqlParameter("@G3", _G3);
            arParams[6] = new SqlParameter("@G4", _G4);
            arParams[7] = new SqlParameter("@G5", _G5);
            arParams[8] = new SqlParameter("@G6", _G6);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWM_ServerArchiveG", arParams);
        }
        public DataSet GetServerArchive(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_ServerArchive", arParams);
        }
        public void AddServerRetrieve(int _requestid, int _itemid, int _number, string _servername, string _backto, int _modelid, string _appcode, int _classid, DateTime _end_date, string _statement)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@servername", _servername);
            arParams[4] = new SqlParameter("@backto", _backto);
            arParams[5] = new SqlParameter("@modelid", _modelid);
            arParams[6] = new SqlParameter("@appcode", _appcode);
            arParams[7] = new SqlParameter("@classid", _classid);
            arParams[8] = new SqlParameter("@end_date", _end_date);
            arParams[9] = new SqlParameter("@statement", _statement);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWM_ServerRetrieve", arParams);
        }
        public void UpdateServerRetrieveG(int _requestid, int _itemid, int _number, int _G1, int _G2, int _G3, int _G4, int _G5, string _hostname)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@G1", _G1);
            arParams[4] = new SqlParameter("@G2", _G2);
            arParams[5] = new SqlParameter("@G3", _G3);
            arParams[6] = new SqlParameter("@G4", _G4);
            arParams[7] = new SqlParameter("@G5", _G5);
            arParams[8] = new SqlParameter("@hostname", _hostname);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWM_ServerRetrieveG", arParams);
        }
        public void UpdateServerRetrieveTV(int _requestid, int _itemid, int _number, int _TV1, int _TV2)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@TV1", _TV1);
            arParams[4] = new SqlParameter("@TV2", _TV2);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWM_ServerRetrieveTV", arParams);
        }
        public void UpdateServerRetrieveTP(int _requestid, int _itemid, int _number, int _TP1, int _TP2, int _TP3, int _TP4, int _TP5, int _TP6, int _TP7, int _TP8, int _TP9, int _TP10)
        {
            arParams = new SqlParameter[13];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@TP1", _TP1);
            arParams[4] = new SqlParameter("@TP2", _TP2);
            arParams[5] = new SqlParameter("@TP3", _TP3);
            arParams[6] = new SqlParameter("@TP4", _TP4);
            arParams[7] = new SqlParameter("@TP5", _TP5);
            arParams[8] = new SqlParameter("@TP6", _TP6);
            arParams[9] = new SqlParameter("@TP7", _TP7);
            arParams[10] = new SqlParameter("@TP8", _TP8);
            arParams[11] = new SqlParameter("@TP9", _TP9);
            arParams[12] = new SqlParameter("@TP10", _TP10);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWM_ServerRetrieveTP", arParams);
        }
        public DataSet GetServerRetrieve(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_ServerRetrieve", arParams);
        }
        public DataSet GetWorkstationRequests(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_workstationRequests", arParams);
        }
        public DataSet GetWorkstation(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_workstation", arParams);
        }
        public void AddWorkstation(int _requestid, int _itemid, int _number, string _reason, string _statement, int _expedite, DateTime _start_date, DateTime _end_date)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@reason", _reason);
            arParams[4] = new SqlParameter("@statement", _statement);
            arParams[5] = new SqlParameter("@expedite", _expedite);
            arParams[6] = new SqlParameter("@start_date", _start_date);
            arParams[7] = new SqlParameter("@end_date", _end_date);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWM_workstation", arParams);
        }
        public DataSet GetIISRequests(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_workstationRequests", arParams);
        }
        public DataSet GetIIS(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_iis", arParams);
        }
        public void AddIIS(int _requestid, int _itemid, int _number, string _reason, string _statement, int _expedite, DateTime _start_date, DateTime _end_date)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@reason", _reason);
            arParams[4] = new SqlParameter("@statement", _statement);
            arParams[5] = new SqlParameter("@expedite", _expedite);
            arParams[6] = new SqlParameter("@start_date", _start_date);
            arParams[7] = new SqlParameter("@end_date", _end_date);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWM_iis", arParams);
        }
        public DataSet GetThirdTierDistributedRequests(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_third_tier_distributedRequests", arParams);
        }
        public DataSet GetThirdTierDistributed(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_third_tier_distributed", arParams);
        }
        public void AddThirdTierDistributed(int _requestid, int _itemid, int _number, int _priority, string _statement, double _hours, DateTime _start_date, DateTime _end_date)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@priority", _priority);
            arParams[4] = new SqlParameter("@statement", _statement);
            arParams[5] = new SqlParameter("@hours", _hours);
            arParams[6] = new SqlParameter("@start_date", _start_date);
            arParams[7] = new SqlParameter("@end_date", _end_date);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWM_third_tier_distributed", arParams);
        }

        public DataSet GetGenericRequests(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_genericRequests", arParams);
        }
        public DataSet GetGeneric(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_generic", arParams);
        }
        public void AddGeneric(int _requestid, int _itemid, int _number, int _priority, string _statement, DateTime _start_date, DateTime _end_date, int _expedite)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@priority", _priority);
            arParams[4] = new SqlParameter("@statement", _statement);
            arParams[5] = new SqlParameter("@start_date", _start_date);
            arParams[6] = new SqlParameter("@end_date", _end_date);
            arParams[7] = new SqlParameter("@expedite", _expedite);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWM_generic", arParams);
        }

        public DataSet GetStorage(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_storage", arParams);
        }
        public void AddStorage(int _requestid, int _itemid, int _number, int _assetid, int _lunid, int _mountid, double _amount, int _high, double _high_total, double _high_qa, double _high_test, double _high_replicated, string _high_level, int _standard, double _standard_total, double _standard_qa, double _standard_test, double _standard_replicated, string _standard_level, int _low, double _low_total, double _low_qa, double _low_test, double _low_replicated, string _low_level, DateTime _start_date, DateTime _end_date)
        {
            arParams = new SqlParameter[27];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@assetid", _assetid);
            arParams[4] = new SqlParameter("@lunid", _lunid);
            arParams[5] = new SqlParameter("@mountid", _mountid);
            arParams[6] = new SqlParameter("@amount", _amount);
            arParams[7] = new SqlParameter("@high", _high);
            arParams[8] = new SqlParameter("@high_total", _high_total);
            arParams[9] = new SqlParameter("@high_qa", _high_qa);
            arParams[10] = new SqlParameter("@high_test", _high_test);
            arParams[11] = new SqlParameter("@high_replicated", _high_replicated);
            arParams[12] = new SqlParameter("@high_level", _high_level);
            arParams[13] = new SqlParameter("@standard", _standard);
            arParams[14] = new SqlParameter("@standard_total", _standard_total);
            arParams[15] = new SqlParameter("@standard_qa", _standard_qa);
            arParams[16] = new SqlParameter("@standard_test", _standard_test);
            arParams[17] = new SqlParameter("@standard_replicated", _standard_replicated);
            arParams[18] = new SqlParameter("@standard_level", _standard_level);
            arParams[19] = new SqlParameter("@low", _low);
            arParams[20] = new SqlParameter("@low_total", _low_total);
            arParams[21] = new SqlParameter("@low_qa", _low_qa);
            arParams[22] = new SqlParameter("@low_test", _low_test);
            arParams[23] = new SqlParameter("@low_replicated", _low_replicated);
            arParams[24] = new SqlParameter("@low_level", _low_level);
            arParams[25] = new SqlParameter("@start_date", _start_date);
            arParams[26] = new SqlParameter("@end_date", _end_date);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWM_storage", arParams);
        }

        #region IDC
        #region IDC Details
        public int AddIDCDetails(int _requestid, int _itemid, int _number, string _investigated, int _investigated_by, string _followup_date, string _date_engaged, string _phase_engaged, string _effort_size, string _involvement, string _eit_testing, string _project_class, string _enterprise_release, string _no_involve, int _idc_spoc, string _comments)
        {
            arParams = new SqlParameter[17];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@investigated", _investigated);
            arParams[4] = new SqlParameter("@investigated_by", _investigated_by);
            arParams[5] = new SqlParameter("@followup_date", (_followup_date == "" ? SqlDateTime.Null : DateTime.Parse(_followup_date)));
            arParams[6] = new SqlParameter("@date_engaged", (_date_engaged == "" ? SqlDateTime.Null : DateTime.Parse(_date_engaged)));
            arParams[7] = new SqlParameter("@phase_engaged", _phase_engaged);
            arParams[8] = new SqlParameter("@effort_size", _effort_size);
            arParams[9] = new SqlParameter("@involvement", _involvement);
            arParams[10] = new SqlParameter("@eit_testing", _eit_testing);
            arParams[11] = new SqlParameter("@project_class", _project_class);
            arParams[12] = new SqlParameter("@enterprise_release", _enterprise_release);
            arParams[13] = new SqlParameter("@no_involve", _no_involve);
            arParams[14] = new SqlParameter("@idc_spoc", _idc_spoc);
            arParams[15] = new SqlParameter("@comments", _comments);
            arParams[16] = new SqlParameter("@id", SqlDbType.Int);
            arParams[16].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWM_IDC", arParams);
            return Int32.Parse(arParams[16].Value.ToString());
        }
        public void UpdateIDCDetails(int _requestid, int _itemid, int _number, string _investigated, int _investigated_by, string _followup_date, string _date_engaged, string _phase_engaged, string _effort_size, string _involvement, string _eit_testing, string _project_class, string _enterprise_release, string _no_involve, int _idc_spoc, string _comments)
        {
            arParams = new SqlParameter[16];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@investigated", _investigated);
            arParams[4] = new SqlParameter("@investigated_by", _investigated_by);
            arParams[5] = new SqlParameter("@followup_date", (_followup_date == "" ? SqlDateTime.Null : DateTime.Parse(_followup_date)));
            arParams[6] = new SqlParameter("@date_engaged", (_date_engaged == "" ? SqlDateTime.Null : DateTime.Parse(_date_engaged)));
            arParams[7] = new SqlParameter("@phase_engaged", _phase_engaged);
            arParams[8] = new SqlParameter("@effort_size", _effort_size);
            arParams[9] = new SqlParameter("@involvement", _involvement);
            arParams[10] = new SqlParameter("@eit_testing", _eit_testing);
            arParams[11] = new SqlParameter("@project_class", _project_class);
            arParams[12] = new SqlParameter("@enterprise_release", _enterprise_release);
            arParams[13] = new SqlParameter("@no_involve", _no_involve);
            arParams[14] = new SqlParameter("@idc_spoc", _idc_spoc);
            arParams[15] = new SqlParameter("@comments", _comments);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWM_IDC", arParams);
        }
        public void UpdateIDCDetails(int _requestid, int _itemid, int _number, double _slide_statement, double _slide_alternatives, double _slide_recommendations, double _slide_high_level, double _slide_detailed)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@slide_statement", _slide_statement);
            arParams[4] = new SqlParameter("@slide_alternatives", _slide_alternatives);
            arParams[5] = new SqlParameter("@slide_recommendations", _slide_recommendations);
            arParams[6] = new SqlParameter("@slide_high_level", _slide_high_level);
            arParams[7] = new SqlParameter("@slide_detailed", _slide_detailed);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWM_IDC_slider", arParams);
        }
        public DataSet GetIDCDetails(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_IDC", arParams);
        }
        #endregion
        #region IDCAssetTypes (Admin side)
        public DataSet GetIDCAssetTypes(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIDCAssetTypes", arParams);
        }

        public DataSet GetIDCAssetType(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIDCAssetType", arParams);
        }

        public void AddIDCAssetType(string _name, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addIDCAssetType", arParams);
        }

        public void UpdateIDCAssetType(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateIDCAssetType", arParams);
        }
        public void EnableIDCAssetType(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateIDCAssetTypeEnabled", arParams);
        }
        public void DeleteIDCAssetType(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteIDCAssetType", arParams);
        }

        #endregion
        #region Resource Types (Admin side)
        public void AddResourceType(string _resource_type, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@resource_type", _resource_type);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResourceType", arParams);
        }

        public void UpdateResourceType(int _id, string _resource_type, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@resource_type", _resource_type);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceType", arParams);
        }

        public void EnableResourceType(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceTypeEnabled", arParams);
        }
        public void DeleteResourceType(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteResourceType", arParams);
        }

        public DataSet GetResourceTypes(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceTypes", arParams);
        }

        public DataSet GetResourceType(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceType", arParams);
        }

        public string GetResourceTypeName(int _id, string _column)
        {
            DataSet ds = GetResourceType(_id);

            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        #endregion
        #region TechAssets
        public int AddTechAsset(int _requestid, int _itemid, int _number, int _asset_typeid, string _name, string _sale_status, string _last_modified, DateTime _last_updated)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@asset_typeid", _asset_typeid);
            arParams[4] = new SqlParameter("@name", _name);
            arParams[5] = new SqlParameter("@salestatus", _sale_status);
            arParams[6] = new SqlParameter("@lastmodified", _last_modified);
            arParams[7] = new SqlParameter("@lastupdated", _last_updated);
            arParams[8] = new SqlParameter("@id", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addTechAsset", arParams);
            return Int32.Parse(arParams[8].Value.ToString());
        }

        public void UpdateTechAsset(int _id, int _asset_typeid, string _name, string _sale_status, string _last_modified, DateTime _last_updated)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@asset_typeid", _asset_typeid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@salestatus", _sale_status);
            arParams[4] = new SqlParameter("@lastmodified", _last_modified);
            arParams[5] = new SqlParameter("@lastupdated", _last_updated);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTechAsset", arParams);
        }

        //public void UpdateTechAsset_WMIDC(int _rrid, int _id, int _wm_idc_id)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@rrid", _rrid);
        //    arParams[1] = new SqlParameter("@id", _id);
        //    arParams[2] = new SqlParameter("@wm_idc_id", _wm_idc_id);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTechAsset_WMIDC", arParams);
        //}


        public void DeleteTechAsset(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteTechAsset", arParams);
        }

        public DataSet GetTechAssets()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTechAssetsAll");
        }

        public DataSet GetTechAssets(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTechAssets", arParams);
        }
        public DataSet GetTechAsset(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTechAsset", arParams);
        }

        public string GetTechAssetTypeName(int _id, string _column)
        {
            DataSet ds = GetResourceType(_id);

            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        #endregion
        #region Resource Assignments      
        public int AddResourceAssignment(int _requestid, int _itemid, int _number, int _resource_typeid, string _requested_by, DateTime _requested_date, DateTime _fulfill_date, string _resource_assigned, string _status)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@resource_typeid", _resource_typeid);
            arParams[4] = new SqlParameter("@requested_by", _requested_by);
            arParams[5] = new SqlParameter("@requested_date", _requested_date);
            arParams[6] = new SqlParameter("@fulfill_date", _fulfill_date);
            arParams[7] = new SqlParameter("@resource_assigned", _resource_assigned);
            arParams[8] = new SqlParameter("@status", _status);
            arParams[9] = new SqlParameter("@id", SqlDbType.Int);
            arParams[9].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResourceAssignment", arParams);
            return Int32.Parse(arParams[9].Value.ToString());
        }

        public void UpdateResourceAssignement(int _id, int _resource_typeid, string _requested_by, DateTime _requested_date, DateTime _fulfill_date, string _resource_assigned, string _status)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@resource_typeid", _resource_typeid);
            arParams[2] = new SqlParameter("@requested_by", _requested_by);
            arParams[3] = new SqlParameter("@requested_date", _requested_date);
            arParams[4] = new SqlParameter("@fulfill_date", _fulfill_date);
            arParams[5] = new SqlParameter("@resource_assigned", _resource_assigned);
            arParams[6] = new SqlParameter("@status", _status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceAssignment", arParams);
        }

        //public void UpdateResourceAssignment_WMIDC(int _rrid, int _id, int _wm_idc_id)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@rrid", _rrid);
        //    arParams[1] = new SqlParameter("@id", _id);
        //    arParams[2] = new SqlParameter("@wm_idc_id", _wm_idc_id);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResourceAssignment_WMIDC", arParams);
        //}

        public void DeleteResourceAssignment(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteResourceAssignment", arParams);
        }

        public DataSet GetResourceAssignments()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceAssignmentsAll");
        }

        public DataSet GetResourceAssignments(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceAssignments", arParams);
        }

        public DataSet GetResourceAssignment(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResourceAssignment", arParams);
        }
        #endregion     
      
        #endregion

        #region ISSUES/ENHANCEMENTS BETA
        public void AddEnhancement(int _requestid, string _title, string _description,int _pageid, int _num_users, string _url, string _path, DateTime _start_date, DateTime _end_date, int _user_id,int _status)
        {
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@requestid", _requestid);                          
            arParams[1] = new SqlParameter("@title", _title);
            arParams[2] = new SqlParameter("@description", _description);
            arParams[3] = new SqlParameter("@pageid", _pageid);
            arParams[4] = new SqlParameter("@numusers ", _num_users);
            arParams[5] = new SqlParameter("@url", _url);
            arParams[6] = new SqlParameter("@path", _path);
            arParams[7] = new SqlParameter("@startdate", _start_date);
            arParams[8] = new SqlParameter("@enddate", _end_date);
            arParams[9] = new SqlParameter("@userid", _user_id);
            arParams[10] = new SqlParameter("@status", _status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addEnhancementh", arParams);
        }
        public DataSet GetEnhancementUser(int _user_id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _user_id);          
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementUser", arParams);             
        }

        public DataSet GetEnhancementUser(int _user_id, string _statusIds, DateTime _submittedDateFrom, DateTime _submittedDateTo)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@userid", _user_id);
            arParams[1] = new SqlParameter("@StatusIds", _statusIds);
            arParams[2] = new SqlParameter("@SubmittedDateFrom", _submittedDateFrom);
            arParams[3] = new SqlParameter("@SubmittedDateTo", _submittedDateTo);

            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementUser", arParams);
        }
        public DataSet GetEnhancementAssigned(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementAssigned", arParams);
        }
        public DataSet GetEnhancement(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);            
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementh", arParams);             
        }

        public string GetEnhancementBody(int _id, int _environment, bool _all) 
        {
            Pages oPage = new Pages(0, dsn);
            Users oUser = new Users(0, dsn);
            Variables oVariable = new Variables(_environment);
            Functions oFunction = new Functions(0, dsn, _environment);
            StatusLevels oStatusLevel = new StatusLevels();
            StringBuilder sbBody = new StringBuilder();
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
            if (_id > 0)
            {
                DataSet ds = GetEnhancementByID(_id);
                sbBody.Append("<tr><td nowrap><b>ID:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>CVT");
                sbBody.Append(ds.Tables[0].Rows[0]["requestid"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Title:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["title"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Description:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(oFunction.FormatText(ds.Tables[0].Rows[0]["description"].ToString()));
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                if (_all == true)
                {
                    sbBody.Append("<tr><td nowrap><b>Module:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(oPage.GetName(Int32.Parse(ds.Tables[0].Rows[0]["pageid"].ToString())));
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Number of Users:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(ds.Tables[0].Rows[0]["num_users"].ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    string strURL = ds.Tables[0].Rows[0]["url"].ToString();
                    if (strURL != "")
                    {
                        strURL = "<a href=\"" + strURL + "\" target=\"_blank\">" + strURL + "</a>";
                    }
                    else
                    {
                        strURL = "N / A";
                    }
                    sbBody.Append("<tr><td nowrap><b>URL:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(strURL);
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    string strPath = ds.Tables[0].Rows[0]["path"].ToString();
                    if (strPath != "")
                    {
                        //strPath = "<a href=\"" + oVariable.URL() + strPath + "\" target=\"_blank\">Click Here to View</a>";
                        strPath = "<a href=\"" + strPath + "\" target=\"_blank\">Click Here to View</a>";
                    }
                    else
                    {
                        strPath = "N / A";
                    }
                    sbBody.Append("<tr><td nowrap><b>Screenshot:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(strPath);
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Start Date:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(DateTime.Parse(ds.Tables[0].Rows[0]["startdate"].ToString()).ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>End Date:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(DateTime.Parse(ds.Tables[0].Rows[0]["enddate"].ToString()).ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Release Date:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    Enhancements oEnhancement = new Enhancements(user, dsn);
                    int intRelease = 0;
                    if (Int32.TryParse(ds.Tables[0].Rows[0]["release"].ToString(), out intRelease) == true && intRelease > 0)
                        sbBody.Append(DateTime.Parse(oEnhancement.GetVersion(intRelease, "release")).ToLongDateString());
                    else
                        sbBody.Append("To Be Determined");
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Priority:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(ds.Tables[0].Rows[0]["priority"].ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                }
                sbBody.Append("<tr><td nowrap><b>Support Type:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>Enhancement</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Status:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(oStatusLevel.HTMLSupport(Int32.Parse(ds.Tables[0].Rows[0]["status"].ToString())));
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Created  By:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(oUser.GetFullName(Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString())));
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Created On:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(DateTime.Parse(ds.Tables[0].Rows[0]["created"].ToString()).ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Modified On:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(DateTime.Parse(ds.Tables[0].Rows[0]["modified"].ToString()).ToString());
                sbBody.Append("</td></tr>");
            }
            if (sbBody.ToString() != "")
            {
                sbBody.Insert(0, "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                sbBody.Append("</table>");
            }
            return sbBody.ToString();
        }

        public int GetEnhancementID(int _requestid)
        {
            if (GetEnhancement(_requestid, "id") != "")
                return Int32.Parse(GetEnhancement(_requestid, "id"));
            else
                return 0;
        }

        public DataSet GetEnhancementByID(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getEnhancementhById", arParams);
        }       

        public string GetEnhancement(int _requestid,string _column)
        {
            DataSet ds = GetEnhancement(_requestid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        
        public void UpdateEnhancement(int _id,  string _title, string _description, int _pageid, int _num_users, string _url, string _path, DateTime _start_date, DateTime _end_date)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@id", _id);             
            arParams[1] = new SqlParameter("@title", _title);
            arParams[2] = new SqlParameter("@description", _description);
            arParams[3] = new SqlParameter("@pageid", _pageid);
            arParams[4] = new SqlParameter("@numusers ", _num_users);
            arParams[5] = new SqlParameter("@url", _url);
            arParams[6] = new SqlParameter("@path", _path);
            arParams[7] = new SqlParameter("@startdate", _start_date);
            arParams[8] = new SqlParameter("@enddate", _end_date);             
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnhancementh", arParams);
        }

        public void UpdateEnhancement(int _id, string _path)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@path", _path);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateEnhancementhPath", arParams);
        }

        public void UpdateEnhancementStatus(int _requestid, int _status, int _release, string _priority)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@status", _status);
            arParams[2] = new SqlParameter("@release", _release);
            arParams[3] = new SqlParameter("@priority", _priority);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_UpdateEnhancementStatus", arParams);
        }

        public void UpdateEnhancementNew(int _requestid, int _new)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@new", _new);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_UpdateEnhancementNew", arParams);
        }

        public void AddIssue(int _requestid, string _title, string _description, int _pageid, int _num_users, string _url, string _path, int _user_id, int _status)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@requestid", _requestid);                          
            arParams[1] = new SqlParameter("@title", _title);
            arParams[2] = new SqlParameter("@description", _description);
            arParams[3] = new SqlParameter("@pageid", _pageid);
            arParams[4] = new SqlParameter("@numusers", _num_users);
            arParams[5] = new SqlParameter("@url", _url);
            arParams[6] = new SqlParameter("@path", _path);
            arParams[7] = new SqlParameter("@userid", _user_id);
            arParams[8] = new SqlParameter("@status", _status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addIssue", arParams);
        }
        public DataSet GetIssueAssigned(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIssuesAssigned", arParams);
        }
        public DataSet GetIssueUser(int _user_id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _user_id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIssueUser", arParams);
        }

        public DataSet GetIssueUser(int _user_id,string _statusIds, DateTime _submittedDateFrom, DateTime _submittedDateTo)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@userid", _user_id);
            arParams[1] = new SqlParameter("@StatusIds", _statusIds);
            arParams[2] = new SqlParameter("@SubmittedDateFrom", _submittedDateFrom);
            arParams[3] = new SqlParameter("@SubmittedDateTo", _submittedDateTo);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIssueUser", arParams);
        }

        public DataSet GetIssue(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIssue", arParams);
        }

        public string GetIssueBody(int _id, int _environment, bool _all)
        {
            Pages oPage = new Pages(0, dsn);
            Users oUser = new Users(0, dsn);
            Variables oVariable = new Variables(_environment);
            Functions oFunction = new Functions(0, dsn, _environment);
            StatusLevels oStatusLevel = new StatusLevels();
            StringBuilder sbBody = new StringBuilder();
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
            if (_id > 0)
            {
                DataSet ds = GetIssueByID(_id);
                sbBody.Append("<tr><td nowrap><b>ID:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>CVT");
                sbBody.Append(ds.Tables[0].Rows[0]["requestid"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Title:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["title"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Description:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(oFunction.FormatText(ds.Tables[0].Rows[0]["description"].ToString()));
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                if (_all == true)
                {
                    sbBody.Append("<tr><td nowrap><b>Module:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(oPage.GetName(Int32.Parse(ds.Tables[0].Rows[0]["pageid"].ToString())));
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Number of Users:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(ds.Tables[0].Rows[0]["num_users"].ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    string strURL = ds.Tables[0].Rows[0]["url"].ToString();
                    if (strURL != "")
                    {
                        strURL = "<a href=\"" + strURL + "\" target=\"_blank\">" + strURL + "</a>";
                    }
                    else
                    {
                        strURL = "N / A";
                    }
                    sbBody.Append("<tr><td nowrap><b>URL:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(strURL);
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    string strPath = ds.Tables[0].Rows[0]["path"].ToString();
                    if (strPath != "")
                    {
                        //strPath = "<a href=\"" + oVariable.URL() + strPath + "\" target=\"_blank\">Click Here to View</a>";
                        strPath = "<a href=\"" + strPath + "\" target=\"_blank\">Click Here to View</a>";
                    }
                    else
                    {
                        strPath = "N / A";
                    }
                    sbBody.Append("<tr><td nowrap><b>Screenshot:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(strPath);
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                }
                sbBody.Append("<tr><td nowrap><b>Support Type:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>Issue</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Status:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(oStatusLevel.HTMLSupport(Int32.Parse(ds.Tables[0].Rows[0]["status"].ToString())));
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Created  By:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(oUser.GetFullName(Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString())));
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Created On:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(DateTime.Parse(ds.Tables[0].Rows[0]["created"].ToString()).ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Modified On:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(DateTime.Parse(ds.Tables[0].Rows[0]["modified"].ToString()).ToString());
                sbBody.Append("</td></tr>");
            }
            if (sbBody.ToString() != "")
            {
                sbBody.Insert(0, "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                sbBody.Append("</table>");
            }

            return sbBody.ToString();
        }

        public int GetIssueID(int _requestid)
        {
            if (GetIssue(_requestid, "id") != "")
                return Int32.Parse(GetIssue(_requestid, "id"));
            else
                return 0;
        }

        public DataSet GetIssueByID(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getIssueById", arParams);
        }

        public string GetIssue(int _requestid, string _column)
        {
            DataSet ds = GetIssue(_requestid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        public void UpdateIssue(int _id, string _title, string _description, int _pageid, int _num_users, string _url, string _path)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@title", _title);
            arParams[2] = new SqlParameter("@description", _description);
            arParams[3] = new SqlParameter("@pageid", _pageid);
            arParams[4] = new SqlParameter("@numusers ", _num_users);
            arParams[5] = new SqlParameter("@url", _url);
            arParams[6] = new SqlParameter("@path", _path);            
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateIssue", arParams);
        }

        public void UpdateIssue(int _id, string _path)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@path", _path);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateIssuePath", arParams);
        }

        public void UpdateIssueStatus(int _requestid, int _status)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);             
            arParams[1] = new SqlParameter("@status", _status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_UpdateIssueStatus", arParams);
        }

        public void UpdateIssueNew(int _requestid, int _new)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@new", _new);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_UpdateIssueNew", arParams);
        }

        public void AddMessage(int _requestid, char _type, string _message, string _path, int _applicationid, int _userid, int _admin, int _new)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@type", _type);
            arParams[2] = new SqlParameter("@message", _message);
            arParams[3] = new SqlParameter("@path", _path);
            arParams[4] = new SqlParameter("@applicationid", _applicationid);
            arParams[5] = new SqlParameter("@userid", _userid);
            arParams[6] = new SqlParameter("@admin", _admin);
            arParams[7] = new SqlParameter("@new", _new);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addMessage", arParams);
        }

        public void UpdateMessage(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateMessage", arParams);
        }

        public DataSet GetMessages(int _requestid, int _new)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@new", _new);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getMessages", arParams);
        }

        public string GetMessages(int _requestid, bool _use_other, string _admin_color)
        {
            Users oUser = new Users(0, dsn);
            Functions oFunction = new Functions(0, dsn, 0);
            Applications oApplication = new Applications(0, dsn);
            StringBuilder sbMessages = new StringBuilder();
            bool boolOther = false;
            DataSet ds = GetMessages(_requestid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (sbMessages.ToString() != "")
                {
                    sbMessages.Append("<tr><td colspan=\"2\">&nbsp;</td></tr>");
                }
                boolOther = !boolOther;
                string strTR = "";
                if (dr["admin"].ToString() == "1")
                {
                    if (_use_other == true)
                        strTR = "<tr" + (boolOther ? " bgcolor=\"#F6F6F6\"" : "") + ">";
                    else
                        strTR = "<tr bgcolor=\"" + _admin_color + "\">";
                    sbMessages.Append(strTR);
                    sbMessages.Append("<td valign=\"top\" rowspan=\"2\"><img src=\"/images/administrator.gif\" align=\"left\" vspace=\"3\" border=\"0\" width=\"90\" height=\"90\" style=\"border:solid 1px #CCCCCC\"/></td>");
                    sbMessages.Append("<td valign=\"top\" class=\"default\" width=\"100%\" height=\"1\"><b>ClearView Administrator</b> replied on ");
                    sbMessages.Append(dr["created"].ToString());
                    sbMessages.Append(":<br/><br/>");
                    sbMessages.Append(oFunction.FormatText(dr["message"].ToString()));
                    sbMessages.Append("</td>");
                    sbMessages.Append("</tr>");
                }
                else
                {
                    if (_use_other == true)
                        strTR = "<tr" + (boolOther ? " bgcolor=\"#F6F6F6\"" : "") + ">";
                    else
                        strTR = "<tr>";
                    sbMessages.Append(strTR);
                    sbMessages.Append("<td valign=\"top\" rowspan=\"2\"><img src=\"/frame/picture.aspx?xid=");
                    sbMessages.Append(oUser.GetName(Int32.Parse(dr["userid"].ToString())));
                    sbMessages.Append("\" align=\"left\" vspace=\"3\" border=\"0\" width=\"90\" height=\"90\" style=\"border:solid 1px #CCCCCC\"/></td>");
                    sbMessages.Append("<td valign=\"top\" class=\"default\" width=\"100%\" height=\"1\"><span class=\"bold\">");
                    sbMessages.Append(oUser.GetFullName(Int32.Parse(dr["userid"].ToString())));
                    sbMessages.Append("</span>, <span class=\"greenlink\">");
                    sbMessages.Append(oApplication.GetName(Int32.Parse(dr["applicationid"].ToString())));
                    sbMessages.Append("</span> replied on ");
                    sbMessages.Append(dr["created"].ToString());
                    sbMessages.Append(":<br/><br/>");
                    sbMessages.Append(oFunction.FormatText(dr["message"].ToString()));
                    sbMessages.Append("</td>");
                    sbMessages.Append("</tr>");
                }
                if (dr["path"].ToString() != "")
                {
                    string strFile = dr["path"].ToString();
                    //strFile = strFile.Substring(strFile.LastIndexOf("/") + 1);
                    strFile = strFile.Substring(strFile.LastIndexOf("\\") + 1);
                    sbMessages.Append(strTR);
                    sbMessages.Append("<td valign=\"bottom\" style=\"border-top:dashed 1px #CCCCCC\"><img src=\"/images/file.gif\" align=\"absmiddle\" border=\"0\"/> <a href=\"");
                    sbMessages.Append(dr["path"].ToString());
                    sbMessages.Append("\" target=\"_blank\">");
                    sbMessages.Append(strFile);
                    sbMessages.Append("</a></td></tr>");
                }
                else
                {
                    sbMessages.Append(strTR);
                    sbMessages.Append("<td></td></tr>");
                }
                sbMessages.Append("<tr><td>&nbsp;</td></tr>");
            }

            sbMessages.Insert(0, "<table width=\"100%\" cellpadding=\"5\" cellspacing=\"0\" border=\"0\" class=\"default\">");
            sbMessages.Append("</table>");

            return sbMessages.ToString();
        }
        #endregion
        #region UserGuide (Admin)
        public void AddUserGuide(int _pageid, string _path, int _display, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@pageid", _pageid);
            arParams[1] = new SqlParameter("@path", _path);
            arParams[2] = new SqlParameter("@display", _display);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addUserGuide", arParams);
        }
        public void UpdateUserGuide(int _id,int _pageid, string _path, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@pageid", _pageid);
            arParams[2] = new SqlParameter("@path", _path);             
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateUserGuide", arParams);
        }
        public void EnableUserGuide(int _id,int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateUserGuideEnabled", arParams);
        }
        public void UpdateUserGuideOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);              
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateUserGuideOrder", arParams);
        }
        public DataSet GetUserGuide(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);             
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getUserGuide", arParams);
        }

        public string GetUserGuide(int _id, string _column)
        {
            DataSet ds = GetUserGuide(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
   
        public DataSet GetUserGuides(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);             
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getUserGuides", arParams);
        }

        public void DeleteUserGuide(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteUserGuide", arParams);
        }

       
        #endregion
        #region UserGuide
        public DataSet GetUserGuideByPage(int _pageid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@pageid", _pageid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getUserGuideByPage", arParams);
        }

        public DataSet GetModules(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModules", arParams);
        }     
         
        #endregion
        #region Category (Admin)
        public DataSet GetCategories(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCategories", arParams);
        }
        public DataSet GetCategory(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCategory", arParams);
        }
        public string GetCategory(int _id, string _column)
        {
            DataSet ds = GetCategory(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public int AddCategory(string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            arParams[2] = new SqlParameter("@id", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addCategory", arParams);
            return Int32.Parse(arParams[2].Value.ToString());
        }
        public void UpdateCategory(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCategory", arParams);
        }
        public void UpdateCategoryOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCategoryOrder", arParams);
        }
        public void EnableCategory(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCategoryEnabled", arParams);
        }
        public void DeleteCategory(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteCategory", arParams);
        }

        public void AddTypeApplication(int _typeid, char _type, int _applicationid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@typeid", _typeid);
            arParams[1] = new SqlParameter("@type", _type);
            arParams[2] = new SqlParameter("@applicationid", _applicationid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addTypeApplication", arParams);
        }

        #endregion
        #region Items (Admin)
        public DataSet GetItems(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getItems", arParams);
        }
        public DataSet GetItem(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getItem", arParams);
        }
        public string GetItem(int _id, string _column)
        {
            DataSet ds = GetItem(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddItem(string _name, string _amount, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@amount", _amount);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addItem", arParams);
        }
        public void UpdateItem(int _id, string _name, string _amount, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@amount", _amount);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateItem", arParams);
        }
        public void UpdateItemOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateItemOrder", arParams);
        }
        public void EnableItem(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateItemEnabled", arParams);
        }
        public void DeleteItem(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteItem", arParams);
        }

        #endregion
        #region Category_List

        public void AddCategoryList(int _parent, int _categoryid, int _itemid, int _userid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@categoryid", _categoryid);
            arParams[2] = new SqlParameter("@itemid", _itemid);
            arParams[3] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addCategoryList", arParams);
        }

        public void UpdateCategoryList(int _userid, int _parent)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@parent", _parent);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCategoryListParent", arParams);
        }

        public void UpdateCategoryList(int _id, int _categoryid, int _itemid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@categoryid", _categoryid);
            arParams[2] = new SqlParameter("@itemid", _itemid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCategoryList", arParams);

        }

        public DataSet GetCategoryList(int _parent)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@parent", _parent);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCategoryList", arParams);

        }

        public void DeleteCategoryList(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteCategoryList", arParams);
        }
        #endregion
        #region Cost Avoidance
        public int AddCostAvoidance(string _opportunity, string _description, string _path, double _addtl_costavoidance, DateTime _date, int _applicationid, int _userid)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@opportunity", _opportunity);
            arParams[1] = new SqlParameter("@description", _description);
            arParams[2] = new SqlParameter("@path", _path);
            arParams[3] = new SqlParameter("@addtl_costavoidance", _addtl_costavoidance);
            arParams[4] = new SqlParameter("@date", _date);
            arParams[5] = new SqlParameter("@applicationid", _applicationid);
            arParams[6] = new SqlParameter("@userid", _userid);
            arParams[7] = new SqlParameter("@id", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addCostAvoidance", arParams);
            return Int32.Parse(arParams[7].Value.ToString());
        }

        public DataSet GetCostAvoidanceAll(int _applicationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCostAvoidanceAll", arParams);
        }

        public DataSet GetCostAvoidanceByUser(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCostAvoidanceByUser", arParams);
        }

        public DataSet GetCostAvoidanceById(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCostAvoidanceById", arParams);
        }

        public void UpdateCostAvoidance(int _id, string _path)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@path", _path);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCostAvoidancePath", arParams);
        }

        public void UpdateCostAvoidance(int _id, string _opportunity, string _description, string _path, double _addtl_costavoidance, DateTime _date)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@opportunity", _opportunity);
            arParams[2] = new SqlParameter("@description", _description);
            arParams[3] = new SqlParameter("@path", _path);
            arParams[4] = new SqlParameter("@addtl_costavoidance", _addtl_costavoidance);
            arParams[5] = new SqlParameter("@date", _date);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCostAvoidance", arParams);
        }



        #endregion

        #region DocumentRepository Repository
        public int AddDocumentRepository(int _applicationid, int _profileid, string _name, string _type, int _department, string _path, string _parent, int _size)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@profileid", _profileid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@type", _type);
            arParams[4] = new SqlParameter("@department", _department);
            arParams[5] = new SqlParameter("@path", _path);
            arParams[6] = new SqlParameter("@parent", _parent);
            arParams[7] = new SqlParameter("@size", _size);
            arParams[8] = new SqlParameter("@id", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDocumentRepository", arParams);
            return Int32.Parse(arParams[8].Value.ToString());
        }

        public void UpdateDocumentRepository(int _id, string _name, string _type, string _path)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@path", _path);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDocumentRepository", arParams);
        }

        public void UpdateDocumentRepositoryParent(int _id, string _parent, string _path)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@parent", _parent);
            arParams[2] = new SqlParameter("@path", _path);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDocumentRepositoryParent", arParams);
        }

        public void UpdateDocumentRepositorySecurity(int _id, int _security)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@security", _security);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDocumentRepositorySecurity", arParams);
        }
        public void DeleteDocumentRepository(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDocumentRepository", arParams);
        }

        public DataSet GetDocumentRepositoryUser(int _profileid, int _applicationid, string _parent)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@profileid", _profileid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            arParams[2] = new SqlParameter("@parent", _parent);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDocumentRepositoryByUser", arParams);
        }

        public DataSet GetDocumentRepositoryApplication(int _applicationid, string _parent)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@parent", _parent);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDocumentRepositoryApplication", arParams);
        }

        public DataSet GetDocumentRepositoryId(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDocumentRepositoryById", arParams);
        }

        public string GetDocumentRepository(int _id, string _column)
        {
            DataSet ds = GetDocumentRepositoryId(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0][_column].ToString();
            }
            else
                return "";
        }

        public DataSet GetDocumentRepositoryByParent(string _parent)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@parent", _parent);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDocumentRepositoryByParent", arParams);
        }

        #endregion
        #region DocumentRepository Repository Shares
        public void AddDocumentRepositoryShare(int _docid, int _security, string _sharetype, int _applicationid, int _profileid, int _ownerid)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@docid", _docid);
            arParams[1] = new SqlParameter("@security", _security);
            arParams[2] = new SqlParameter("@sharetype", _sharetype);
            arParams[3] = new SqlParameter("@applicationid", _applicationid);
            arParams[4] = new SqlParameter("@profileid", _profileid);
            arParams[5] = new SqlParameter("@ownerid", _ownerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDocumentRepositoryShare", arParams);
        }

        public void UpdateDocumentRepositoryShare(int _id, int _docid, int _security, string _sharetype, int _applicationid, int _profileid, int _ownerid)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@docid", _docid);
            arParams[2] = new SqlParameter("@security", _security);
            arParams[3] = new SqlParameter("@sharetype", _sharetype);
            arParams[4] = new SqlParameter("@applicationid", _applicationid);
            arParams[5] = new SqlParameter("@profileid", _profileid);
            arParams[6] = new SqlParameter("@ownerid", _ownerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDocumentRepositoryShare", arParams);
        }

        public void DeleteDocumentRepositoryShare(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDocumentRepositoryShare", arParams);
        }

        public DataSet GetDocumentRepositorySharesByIds(int _docid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@docid", _docid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDocumentRepositorySharesByIds", arParams);
        }

        public string GetDocumentRepositorySharesByIds(int _docid, string _column)
        {
            DataSet ds = GetDocumentRepositorySharesByIds(_docid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        public DataSet GetDocumentRepositorySharesById(int _docid, string _sharetype)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@docid", _docid);
            arParams[1] = new SqlParameter("@sharetype", _sharetype);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDocumentRepositorySharesById", arParams);
        }

        public DataSet GetDocumentRepositorySharesByApplication(int _applicationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDocumentRepositorySharesByApplication", arParams);
        }

        public DataSet GetDocumentRepositorySharesByUser(int _profileid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@profileid", _profileid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDocumentRepositorySharesByUser", arParams);
        }

        #endregion


        public string GetNext(string _name)
        {
            if (_name == "A") return "B";
            else if (_name == "B") return "C";
            else if (_name == "C") return "D";
            else if (_name == "D") return "E";
            else if (_name == "E") return "F";
            else if (_name == "F") return "G";
            else if (_name == "G") return "H";
            else if (_name == "H") return "I";
            else if (_name == "I") return "J";
            else if (_name == "J") return "K";
            else if (_name == "K") return "L";
            else if (_name == "L") return "M";
            else if (_name == "M") return "N";
            else if (_name == "N") return "O";
            else if (_name == "O") return "P";
            else if (_name == "P") return "Q";
            else if (_name == "Q") return "R";
            else if (_name == "R") return "S";
            else if (_name == "S") return "T";
            else if (_name == "T") return "U";
            else if (_name == "U") return "V";
            else if (_name == "V") return "W";
            else if (_name == "W") return "X";
            else if (_name == "X") return "Y";
            else if (_name == "Y") return "Z";
            else if (_name == "Z") return "#";
            else return "!";
        }

        public void AddStorage3rd(int _requestid, int _itemid, int _number, string _servername, string _os, string _maintenance, string _currently, string _type, string _dr, string _performance, string _change, string _cluster, string _sql, string _version, int _dba, string _cluster_group_new, int _tsm, string _networkname, string _ipaddress, string _cluster_group_existing, int _databasesql0x, int _backupsql0x, string _newdriveletter, string _newmountpoint, string _increase, string _description, int _classid, int _environmentid, int _addressid, string _fabric, string _replicated, int _ha, string _shared, string _expand, double _amount, string _luns, string _www, string _uid, string _node, string _encname, string _encslot, string _repservername, string _repwww, string _repencname, string _repencslot, double _allocated, int _midrange, int _userid, DateTime _end_date, string _filesystem, string _client_amount)
        {
            arParams = new SqlParameter[51];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@servername", _servername);
            arParams[4] = new SqlParameter("@os", _os);
            arParams[5] = new SqlParameter("@maintenance", _maintenance);
            arParams[6] = new SqlParameter("@currently", _currently);
            arParams[7] = new SqlParameter("@type", _type);
            arParams[8] = new SqlParameter("@dr", _dr);
            arParams[9] = new SqlParameter("@performance", _performance);
            arParams[10] = new SqlParameter("@change", _change);
            arParams[11] = new SqlParameter("@cluster", _cluster);
            arParams[12] = new SqlParameter("@sql", _sql);
            arParams[13] = new SqlParameter("@version", _version);
            arParams[14] = new SqlParameter("@dba", _dba);
            arParams[15] = new SqlParameter("@cluster_group_new", _cluster_group_new);
            arParams[16] = new SqlParameter("@tsm", _tsm);
            arParams[17] = new SqlParameter("@networkname", _networkname);
            arParams[18] = new SqlParameter("@ipaddress", _ipaddress);
            arParams[19] = new SqlParameter("@cluster_group_existing", _cluster_group_existing);
            arParams[20] = new SqlParameter("@databasesql0x", _databasesql0x);
            arParams[21] = new SqlParameter("@backupsql0x", _backupsql0x);
            arParams[22] = new SqlParameter("@newdriveletter", _newdriveletter);
            arParams[23] = new SqlParameter("@newmountpoint", _newmountpoint);
            arParams[24] = new SqlParameter("@increase", _increase);
            arParams[25] = new SqlParameter("@description", _description);
            arParams[26] = new SqlParameter("@classid", _classid);
            arParams[27] = new SqlParameter("@environmentid", _environmentid);
            arParams[28] = new SqlParameter("@addressid", _addressid);
            arParams[29] = new SqlParameter("@fabric", _fabric);
            arParams[30] = new SqlParameter("@replicated", _replicated);
            arParams[31] = new SqlParameter("@ha", _ha);
            arParams[32] = new SqlParameter("@shared", _shared);
            arParams[33] = new SqlParameter("@expand", _expand);
            arParams[34] = new SqlParameter("@amount", _amount);
            arParams[35] = new SqlParameter("@luns", _luns);
            arParams[36] = new SqlParameter("@www", _www);
            arParams[37] = new SqlParameter("@uid", _uid);
            arParams[38] = new SqlParameter("@node", _node);
            arParams[39] = new SqlParameter("@encname", _encname);
            arParams[40] = new SqlParameter("@encslot", _encslot);
            arParams[41] = new SqlParameter("@repservername", _repservername);
            arParams[42] = new SqlParameter("@repwww", _repwww);
            arParams[43] = new SqlParameter("@repencname", _repencname);
            arParams[44] = new SqlParameter("@repencslot", _repencslot);
            arParams[45] = new SqlParameter("@allocated", _allocated);
            arParams[46] = new SqlParameter("@midrange", _midrange);
            arParams[47] = new SqlParameter("@userid", _userid);
            arParams[48] = new SqlParameter("@end_date", _end_date);
            arParams[49] = new SqlParameter("@filesystem", _filesystem);
            arParams[50] = new SqlParameter("@client_amount", _client_amount);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addStorage3rd", arParams);
        }
        public void UpdateStorage3rdFlow1(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateStorage3rdFlow1", arParams);
        }
        public void UpdateStorage3rdFlow2(int _requestid, int _itemid, int _number, int _itemid2, int _number2)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@itemid2", _itemid2);
            arParams[4] = new SqlParameter("@number2", _number2);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateStorage3rdFlow2", arParams);
        }
        public void UpdateStorage3rdFlow3(int _requestid, int _itemid2, int _number2, int _itemid3, int _number3)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid2", _itemid2);
            arParams[2] = new SqlParameter("@number2", _number2);
            arParams[3] = new SqlParameter("@itemid3", _itemid3);
            arParams[4] = new SqlParameter("@number3", _number3);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateStorage3rdFlow3", arParams);
        }
        public string GetStorage3rdBody(int _requestid, int _itemid, int _number, int _environment)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);

            Requests oRequest = new Requests(0, dsn);
            Users oUser = new Users(0, dsn);
            Classes oClass = new Classes(0, dsn);
            Environments oEnvironment = new Environments(0, dsn);
            Locations oLocation = new Locations(0, dsn);
            Variables oVariable = new Variables(_environment);
            StringBuilder sbDetails = new StringBuilder();
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorage3rdBody", arParams);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intMidrange = Int32.Parse(ds.Tables[0].Rows[0]["midrange"].ToString());
                int intUser = oRequest.GetUser(_requestid);
                int intDBA = 0;
                if (ds.Tables[0].Rows[0]["dba"].ToString() != "")
                    intDBA = Int32.Parse(ds.Tables[0].Rows[0]["dba"].ToString());
                sbDetails.Append("<tr><td colspan=\"2\">Amount Allocated By Storage Team:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["allocated"].ToString() != "" ? double.Parse(ds.Tables[0].Rows[0]["allocated"].ToString()).ToString("F") : "---");
                sbDetails.Append(" GB</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Operating System:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["os"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Maintenance Window:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["maintenance"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Where is your DR site:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["dr"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Have you scheduled the change control:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["change"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Is this server a cluster:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["cluster"].ToString());
                sbDetails.Append("</td></tr>");
                if (intMidrange == 1)
                {
                    if (ds.Tables[0].Rows[0]["cluster"].ToString().ToUpper().Contains("YES") == true)
                    {
                        // CLUSTER = YES
                        sbDetails.Append("<tr><td colspan=\"2\">Is the new SAN going into a new cluster group or an existing one:</td></tr>");
                        sbDetails.Append("<tr><td><img src=\"");
                        sbDetails.Append(oVariable.ImageURL());
                        sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                        sbDetails.Append(ds.Tables[0].Rows[0]["cluster_group_new"].ToString());
                        sbDetails.Append("</td></tr>");
                        if (ds.Tables[0].Rows[0]["cluster_group_new"].ToString().ToUpper().Contains("NEW") == true)
                        {
                            sbDetails.Append("<tr><td colspan=\"2\">Have you requested the following (if new cluster group):</td></tr>");
                            sbDetails.Append("<tr><td><img src=\"");
                            sbDetails.Append(oVariable.ImageURL());
                            sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">" + "TSM Backup : ");
                            sbDetails.Append(ds.Tables[0].Rows[0]["tsm"].ToString() == "1" ? "Yes" : "No");
                            sbDetails.Append("<br/>Network Name : ");
                            sbDetails.Append(ds.Tables[0].Rows[0]["networkname"].ToString() == "1" ? "Yes (" + ds.Tables[0].Rows[0]["networkname"].ToString() + ")" : "No");
                            sbDetails.Append("<br/>IP Address : ");
                            sbDetails.Append(ds.Tables[0].Rows[0]["ipaddress"].ToString() == "1" ? "Yes (" + ds.Tables[0].Rows[0]["ipaddress"].ToString() + ")" : "No");
                            sbDetails.Append("</td></tr>");
                        }
                        else
                        {
                            sbDetails.Append("<tr><td colspan=\"2\">What cluster group should the drive be added to (if existing one):</td></tr>");
                            sbDetails.Append("<tr><td><img src=\"");
                            sbDetails.Append(oVariable.ImageURL());
                            sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                            sbDetails.Append(ds.Tables[0].Rows[0]["cluster_group_existing"].ToString());
                            sbDetails.Append("</td></tr>");
                            sbDetails.Append("<tr><td colspan=\"2\">Are you requesting the increase in size of an existing filesystem or a brand new filesystem?</td></tr>");
                            sbDetails.Append("<tr><td><img src=\"");
                            sbDetails.Append(oVariable.ImageURL());
                            sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                            sbDetails.Append(ds.Tables[0].Rows[0]["increase"].ToString());
                            sbDetails.Append("</td></tr>");
                            sbDetails.Append("<tr><td colspan=\"2\">Please enter the name of the filesystem:</td></tr>");
                            sbDetails.Append("<tr><td><img src=\"");
                            sbDetails.Append(oVariable.ImageURL());
                            sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                            sbDetails.Append(ds.Tables[0].Rows[0]["filesystem"].ToString());
                            sbDetails.Append("</td></tr>");
                        }
                    }
                    else
                    {
                        // CLUSTER = NO
                        sbDetails.Append("<tr><td colspan=\"2\">Are you requesting the increase in size of an existing filesystem or a brand new filesystem?</td></tr>");
                        sbDetails.Append("<tr><td><img src=\"");
                        sbDetails.Append(oVariable.ImageURL());
                        sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                        sbDetails.Append(ds.Tables[0].Rows[0]["increase"].ToString());
                        sbDetails.Append("</td></tr>");
                        sbDetails.Append("<tr><td colspan=\"2\">Please enter the name of the filesystem:</td></tr>");
                        sbDetails.Append("<tr><td><img src=\"");
                        sbDetails.Append(oVariable.ImageURL());
                        sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                        sbDetails.Append(ds.Tables[0].Rows[0]["filesystem"].ToString());
                        sbDetails.Append("</td></tr>");
                    }
                    sbDetails.Append("<tr><td colspan=\"2\">How much ADDITIONAL storage do you require?</td></tr>");
                    sbDetails.Append("<tr><td><img src=\"");
                    sbDetails.Append(oVariable.ImageURL());
                    sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                    sbDetails.Append(ds.Tables[0].Rows[0]["client_amount"].ToString());
                    sbDetails.Append("</td></tr>");
                }
                else
                {
                    if (ds.Tables[0].Rows[0]["cluster"].ToString().ToUpper().Contains("YES") == true)
                    {
                        // CLUSTER = YES
                        sbDetails.Append("<tr><td colspan=\"2\">Is this a SQL server:</td></tr>");
                        sbDetails.Append("<tr><td><img src=\"");
                        sbDetails.Append(oVariable.ImageURL());
                        sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                        sbDetails.Append(ds.Tables[0].Rows[0]["sql"].ToString());
                        sbDetails.Append("</td></tr>");
                        if (ds.Tables[0].Rows[0]["sql"].ToString().ToUpper().Contains("YES") == true)
                        {
                            // SQL = YES
                            sbDetails.Append("<tr><td colspan=\"2\">What version of SQL (if SQL):</td></tr>");
                            sbDetails.Append("<tr><td><img src=\"");
                            sbDetails.Append(oVariable.ImageURL());
                            sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                            sbDetails.Append(ds.Tables[0].Rows[0]["version"].ToString());
                            sbDetails.Append("</td></tr>");
                            if (ds.Tables[0].Rows[0]["version"].ToString().Contains("2005") == true)
                            {
                                sbDetails.Append("<tr><td colspan=\"2\">Are you requesting an additional database/sql0x or backup/sql0x folder?</td></tr>");
                                sbDetails.Append("<tr><td><img src=\"");
                                sbDetails.Append(oVariable.ImageURL());
                                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">" + "database/sql0x : ");
                                sbDetails.Append(ds.Tables[0].Rows[0]["databasesql0x"].ToString() == "1" ? "Yes" : "No");
                                sbDetails.Append("<br/>backup/sql0x : ");
                                sbDetails.Append(ds.Tables[0].Rows[0]["backupsql0x"].ToString() == "1" ? "Yes" : "No");
                                sbDetails.Append("</td></tr>");
                            }
                            sbDetails.Append("<tr><td colspan=\"2\">Who is your DBA (if SQL):</td></tr>");
                            sbDetails.Append("<tr><td><img src=\"");
                            sbDetails.Append(oVariable.ImageURL());
                            sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                            sbDetails.Append(oUser.GetFullName(intDBA));
                            sbDetails.Append(" (");
                            sbDetails.Append(oUser.GetName(intDBA));
                            sbDetails.Append(")");
                            sbDetails.Append("</td></tr>");
                            sbDetails.Append("<tr><td colspan=\"2\">Is the new SAN going into a new cluster group or an existing one:</td></tr>");
                            sbDetails.Append("<tr><td><img src=\"");
                            sbDetails.Append(oVariable.ImageURL());
                            sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                            sbDetails.Append(ds.Tables[0].Rows[0]["cluster_group_new"].ToString());
                            sbDetails.Append("</td></tr>");
                            if (ds.Tables[0].Rows[0]["cluster_group_new"].ToString().ToUpper().Contains("NEW") == true)
                            {
                                sbDetails.Append("<tr><td colspan=\"2\">Have you requested the following (if new cluster group):</td></tr>");
                                sbDetails.Append("<tr><td><img src=\"");
                                sbDetails.Append(oVariable.ImageURL());
                                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                                sbDetails.Append("TSM Backup : ");
                                sbDetails.Append(ds.Tables[0].Rows[0]["tsm"].ToString() == "1" ? "Yes" : "No");
                                sbDetails.Append("<br/>Network Name : ");
                                sbDetails.Append(ds.Tables[0].Rows[0]["networkname"].ToString() == "1" ? "Yes (" + ds.Tables[0].Rows[0]["networkname"].ToString() + ")" : "No");
                                sbDetails.Append("<br/>IP Address : ");
                                sbDetails.Append(ds.Tables[0].Rows[0]["ipaddress"].ToString() == "1" ? "Yes (" + ds.Tables[0].Rows[0]["ipaddress"].ToString() + ")" : "No");
                                sbDetails.Append("</td></tr>");
                            }
                            else
                            {
                                sbDetails.Append("<tr><td colspan=\"2\">What cluster group should the drive be added to (if existing one):</td></tr>");
                                sbDetails.Append("<tr><td><img src=\"");
                                sbDetails.Append(oVariable.ImageURL());
                                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                                sbDetails.Append(ds.Tables[0].Rows[0]["cluster_group_existing"].ToString());
                                sbDetails.Append("</td></tr>");
                            }
                        }
                        else
                        {
                            // SQL = NO
                            sbDetails.Append("<tr><td colspan=\"2\">Are you requesting a new drive letter or a mount point:</td></tr>");
                            sbDetails.Append("<tr><td><img src=\"");
                            sbDetails.Append(oVariable.ImageURL());
                            sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                            sbDetails.Append(ds.Tables[0].Rows[0]["newdriveletter"].ToString());
                            sbDetails.Append("</td></tr>");
                            if (ds.Tables[0].Rows[0]["newdriveletter"].ToString().ToUpper().Contains("MOUNT POINT") == true)
                            {
                                sbDetails.Append("<tr><td colspan=\"2\">Where should the mount point be attached (if new mount point):</td></tr>");
                                sbDetails.Append("<tr><td><img src=\"");
                                sbDetails.Append(oVariable.ImageURL());
                                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                                sbDetails.Append(ds.Tables[0].Rows[0]["newmountpoint"].ToString());
                                sbDetails.Append("</td></tr>");
                            }
                            sbDetails.Append("<tr><td colspan=\"2\">Is the new SAN going into a new cluster group or an existing one:</td></tr>");
                            sbDetails.Append("<tr><td><img src=\"");
                            sbDetails.Append(oVariable.ImageURL());
                            sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                            sbDetails.Append(ds.Tables[0].Rows[0]["cluster_group_new"].ToString());
                            sbDetails.Append("</td></tr>");
                            if (ds.Tables[0].Rows[0]["cluster_group_new"].ToString().ToUpper().Contains("NEW") == true)
                            {
                                sbDetails.Append("<tr><td colspan=\"2\">Have you requested the following (if new cluster group):</td></tr>");
                                sbDetails.Append("<tr><td><img src=\"");
                                sbDetails.Append(oVariable.ImageURL());
                                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                                sbDetails.Append("TSM Backup : ");
                                sbDetails.Append(ds.Tables[0].Rows[0]["tsm"].ToString() == "1" ? "Yes" : "No");
                                sbDetails.Append("<br/>Network Name : ");
                                sbDetails.Append(ds.Tables[0].Rows[0]["networkname"].ToString() == "1" ? "Yes (" + ds.Tables[0].Rows[0]["networkname"].ToString() + ")" : "No");
                                sbDetails.Append("<br/>IP Address : ");
                                sbDetails.Append(ds.Tables[0].Rows[0]["ipaddress"].ToString() == "1" ? "Yes (" + ds.Tables[0].Rows[0]["ipaddress"].ToString() + ")" : "No");
                                sbDetails.Append("</td></tr>");
                            }
                            else
                            {
                                sbDetails.Append("<tr><td colspan=\"2\">What cluster group should the drive be added to (if existing one):</td></tr>");
                                sbDetails.Append("<tr><td><img src=\"");
                                sbDetails.Append(oVariable.ImageURL());
                                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                                sbDetails.Append(ds.Tables[0].Rows[0]["cluster_group_existing"].ToString());
                                sbDetails.Append("</td></tr>");
                            }
                        }
                    }
                    else
                    {
                        // CLUSTER = NO
                        sbDetails.Append("<tr><td colspan=\"2\">Is this a SQL server:</td></tr>");
                        sbDetails.Append("<tr><td><img src=\"");
                        sbDetails.Append(oVariable.ImageURL());
                        sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                        sbDetails.Append(ds.Tables[0].Rows[0]["sql"].ToString());
                        sbDetails.Append("</td></tr>");
                        if (ds.Tables[0].Rows[0]["sql"].ToString().ToUpper().Contains("YES") == true)
                        {
                            // SQL = YES
                            sbDetails.Append("<tr><td colspan=\"2\">What version of SQL (if SQL):</td></tr>");
                            sbDetails.Append("<tr><td><img src=\"");
                            sbDetails.Append(oVariable.ImageURL());
                            sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                            sbDetails.Append(ds.Tables[0].Rows[0]["version"].ToString());
                            sbDetails.Append("</td></tr>");
                            if (ds.Tables[0].Rows[0]["version"].ToString().Contains("2005") == true)
                            {
                                sbDetails.Append("<tr><td colspan=\"2\">Are you requesting an additional database/sql0x or backup/sql0x folder?</td></tr>");
                                sbDetails.Append("<tr><td><img src=\"");
                                sbDetails.Append(oVariable.ImageURL());
                                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                                sbDetails.Append("database/sql0x : ");
                                sbDetails.Append(ds.Tables[0].Rows[0]["databasesql0x"].ToString() == "1" ? "Yes" : "No");
                                sbDetails.Append("<br/>backup/sql0x : ");
                                sbDetails.Append(ds.Tables[0].Rows[0]["backupsql0x"].ToString() == "1" ? "Yes" : "No");
                                sbDetails.Append("</td></tr>");
                            }
                            sbDetails.Append("<tr><td colspan=\"2\">Who is your DBA (if SQL):</td></tr>");
                            sbDetails.Append("<tr><td><img src=\"");
                            sbDetails.Append(oVariable.ImageURL());
                            sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                            sbDetails.Append(oUser.GetFullName(intDBA));
                            sbDetails.Append(" (");
                            sbDetails.Append(oUser.GetName(intDBA));
                            sbDetails.Append(")");
                            sbDetails.Append("</td></tr>");
                        }
                        else
                        {
                            // SQL = NO
                            sbDetails.Append("<tr><td colspan=\"2\">Are you requesting the increase in size of an existing drive letter or a brand new drive letter (max size 750 GB):</td></tr>");
                            sbDetails.Append("<tr><td><img src=\"");
                            sbDetails.Append(oVariable.ImageURL());
                            sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                            sbDetails.Append(ds.Tables[0].Rows[0]["increase"].ToString());
                            sbDetails.Append("</td></tr>");
                        }
                    }
                }

                sbDetails.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade/></td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Name of Requestor:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(oUser.GetFullName(intUser));
                sbDetails.Append(" (");
                sbDetails.Append(oUser.GetName(intUser));
                sbDetails.Append(")");
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Statement of Work:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["description"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Server Name:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["servername"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Estimated End Date:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(DateTime.Parse(ds.Tables[0].Rows[0]["end_date"].ToString()).ToShortDateString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Is this device already SAN Connected:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["currently"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">What type of device is this:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["type"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">What class is this device in:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(oClass.Get(Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString())));
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">What environment is this device in:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(oEnvironment.Get(Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString())));
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">What location is this device in:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(oLocation.GetFull(Int32.Parse(ds.Tables[0].Rows[0]["addressid"].ToString())));
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">What fabric is the device on:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["fabric"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Is this device being replicated:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["replicated"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Please select a type of storage:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["shared"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Do you want to expand a LUN or add an additional LUN:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["expand"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Additional Total Capacity Needed (in GB):</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["amount"].ToString() != "" ? double.Parse(ds.Tables[0].Rows[0]["amount"].ToString()).ToString("F") : "---");
                sbDetails.Append("</td></tr>");
                char[] strSplit = { ',' };
                string[] strLUNs = ds.Tables[0].Rows[0]["luns"].ToString().Split(strSplit);
                string strLUN = "";
                for (int ii = 0; ii < strLUNs.Length; ii++)
                {
                    if (strLUNs[ii].Trim() != "")
                        strLUN += strLUNs[ii].Trim() + "<br/>";
                }
                sbDetails.Append("<tr><td colspan=\"2\">Please enter the LUN drive and UID, followed by the amount of storage you want to have added to that LUN (additional capacity only) (in GB):</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(strLUN);
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Enter the World Wide Port names:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["www"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">UID:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["uid"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Select the Storage Performance Tier:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["performance"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Clustered NODE Server Names:</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["node"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Enclosure Name (if blade):</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["encname"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Enclosure Slot (if blade):</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["encslot"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Server name of DR device (if replicated):</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["repservername"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">World Wide Port names of the DR device (if replicated):</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["repwww"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Enclosure Name (if blade and replicated):</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["repencname"].ToString());
                sbDetails.Append("</td></tr>");
                sbDetails.Append("<tr><td colspan=\"2\">Enclosure Slot (if blade and replicated):</td></tr>");
                sbDetails.Append("<tr><td><img src=\"");
                sbDetails.Append(oVariable.ImageURL());
                sbDetails.Append("/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sbDetails.Append(ds.Tables[0].Rows[0]["repencslot"].ToString());
                sbDetails.Append("</td></tr>");
            }
            return sbDetails.ToString();
        }
        public DataSet GetStorage3rd(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorage3rd", arParams);
        }
        public DataSet GetStorage3rdFlow1(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorage3rdFlow1", arParams);
        }
        public DataSet GetStorage3rdFlow2(int _requestid, int _itemid2, int _number2)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid2", _itemid2);
            arParams[2] = new SqlParameter("@number2", _number2);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorage3rdFlow2", arParams);
        }
        public DataSet GetStorage3rdFlow3(int _requestid, int _itemid3, int _number3)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid3", _itemid3);
            arParams[2] = new SqlParameter("@number3", _number3);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorage3rdFlow3", arParams);
        }
        public void UpdateStorage3rd(int _requestid, int _itemid, int _number, int _newitemid, int _newnumber)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@newitemid", _newitemid);
            arParams[4] = new SqlParameter("@newnumber", _newnumber);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateStorage3rd", arParams);
        }
        public void UpdateStorage3rd(int _requestid, int _itemid, int _number, double _allocated)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@allocated", _allocated);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateStorage3rdAllocated", arParams);
        }
        public DataSet GetStorage3rdForecast()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorage3rdForecast");
        }
        public DataSet GetStorage3rdForecast(int _prod, int _qa, int _test, int _addressid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@prod", _prod);
            arParams[1] = new SqlParameter("@qa", _qa);
            arParams[2] = new SqlParameter("@test", _test);
            arParams[3] = new SqlParameter("@addressid", _addressid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorage3rdForecastDetails", arParams);
        }
        public DataSet GetDecommissionServer(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_DecommissionServer", arParams);
        }
        public DataSet GetDecommissionServerDeleted(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_DecommissionServerDeleted", arParams);
        }

        public DataSet GetDecommissionServerIM(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWM_DecommissionServer_IM", arParams);
        }

        public void AddDecommissionServer(int _requestid, int _itemid, int _number, string _servername, int _serverid, DateTime _poweroff, string _change, string _reason, int _classid, int _environmentid, int _addressid, int _modelid, string _serial, string _serial_dr, int _retrieve, string _retrieve_description, string _retrieve_address, string _retrieve_locator)
        {
            arParams = new SqlParameter[18];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@servername", _servername);
            arParams[4] = new SqlParameter("@serverid", _serverid);
            arParams[5] = new SqlParameter("@poweroff", _poweroff);
            arParams[6] = new SqlParameter("@change", _change);
            arParams[7] = new SqlParameter("@reason", _reason);
            arParams[8] = new SqlParameter("@classid", _classid);
            arParams[9] = new SqlParameter("@environmentid", _environmentid);
            arParams[10] = new SqlParameter("@addressid", _addressid);
            arParams[11] = new SqlParameter("@modelid", _modelid);
            arParams[12] = new SqlParameter("@serial", _serial);
            arParams[13] = new SqlParameter("@serial_dr", _serial_dr);
            arParams[14] = new SqlParameter("@retrieve", _retrieve);
            arParams[15] = new SqlParameter("@retrieve_description", _retrieve_description);
            arParams[16] = new SqlParameter("@retrieve_address", _retrieve_address);
            arParams[17] = new SqlParameter("@retrieve_locator", _retrieve_locator);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWM_DecommissionServer", arParams);
        }
        public void DeleteDecommissionServer(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteWM_DecommissionServer", arParams);
        }
        public void UpdateDecommissionServer(int _requestid, int _itemid, int _number, string _poweredoff, string _blackedout, string _renamed, string _destroy, int _decommed, int _disposed, string _destroyed, int? _SANflag, int? _CSMflag, int? _ip_build, int? _ip_final)
        {
            arParams = new SqlParameter[14];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@poweredoff", (_poweredoff == "" ? SqlDateTime.Null : DateTime.Parse(_poweredoff)));
            arParams[4] = new SqlParameter("@blackedout", (_blackedout == "" ? SqlDateTime.Null : DateTime.Parse(_blackedout)));
            arParams[5] = new SqlParameter("@renamed", (_renamed == "" ? SqlDateTime.Null : DateTime.Parse(_renamed)));
            arParams[6] = new SqlParameter("@destroy", (_destroy == "" ? SqlDateTime.Null : DateTime.Parse(_destroy)));
            arParams[7] = new SqlParameter("@destroyed", (_destroyed == "" ? SqlDateTime.Null : DateTime.Parse(_destroyed)));
            arParams[8] = new SqlParameter("@decommed", _decommed);
            arParams[9] = new SqlParameter("@disposed", _disposed);
            if (_SANflag != null)
                arParams[10] = new SqlParameter("@SAN",  _SANflag);
            if (_CSMflag != null)
                arParams[11] = new SqlParameter("@CSM", _CSMflag);
            if (_ip_build != null)
                arParams[12] = new SqlParameter("@ip_build", _ip_build);
            if (_ip_final != null)
                arParams[13] = new SqlParameter("@ip_final", _ip_final);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWM_DecommissionServer", arParams);
        }
        public void UpdateDecommissionServer(int _requestid, int _itemid, int _number, string _poweroff_new)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@poweroff_new", (_poweroff_new == "" ? SqlDateTime.Null : DateTime.Parse(_poweroff_new)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWM_DecommissionServerEdit", arParams);
        }
        public void UpdateDecommissionServerReclaimStorage(int _requestid, int _itemid, int _number, double _reclaimed_storage, double _reclaimed_amt, int _reclaimed_tier, string _reclaimed_environment, string _reclaimed_storage_precooldown, string _reclaimed_storage_cooldown,
            string _reclaimed_storage_cr2, string _reclaimed_storage_classification, string _reclaimed_storage_vendor,
            int _reclaimed_storage_location, string _reclaimed_storage_array, string _reclaimed_storage_notes)
        {
            arParams = new SqlParameter[15];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@reclaimed_storage", _reclaimed_storage);
            arParams[4] = new SqlParameter("@reclaimed_amt", _reclaimed_amt);
            arParams[5] = new SqlParameter("@reclaimed_tier", _reclaimed_tier);
            arParams[6] = new SqlParameter("@reclaimed_environment", _reclaimed_environment);
            arParams[7] = new SqlParameter("@reclaimed_storage_precooldown", (_reclaimed_storage_precooldown == "" ? SqlDateTime.Null : DateTime.Parse(_reclaimed_storage_precooldown)));
            arParams[8] = new SqlParameter("@reclaimed_storage_cooldown", (_reclaimed_storage_cooldown == "" ? SqlDateTime.Null : DateTime.Parse(_reclaimed_storage_cooldown)));
            arParams[9] = new SqlParameter("@reclaimed_storage_cr2", _reclaimed_storage_cr2);
            arParams[10] = new SqlParameter("@reclaimed_storage_classification", _reclaimed_storage_classification);
            arParams[11] = new SqlParameter("@reclaimed_storage_vendor", _reclaimed_storage_vendor);
            arParams[12] = new SqlParameter("@reclaimed_storage_location", _reclaimed_storage_location);
            arParams[13] = new SqlParameter("@reclaimed_storage_array", _reclaimed_storage_array);
            arParams[14] = new SqlParameter("@reclaimed_storage_notes", _reclaimed_storage_notes);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWM_DecommissionServerReclaimStorage", arParams);
        }
        public void UpdateDecommissionServerReclaimBackup(int _requestid, int _itemid, int _number, double _reclaimed_backup)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@reclaimed_backup", _reclaimed_backup);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWM_DecommissionServerReclaimBackup", arParams);
        }

        public void AddDecommissionServerIM(int _requestid, int _itemid, int _number, int _serverid,
                    int? _ServerDestroyed, int? _DestroyUnRack, int? _DestroyWipeDrives, int? _DestroyDispose,
                    int? _ServerRedeployed, int? _RedeployVerifyServerModel, int? _RedeployMoveServerToDeploy)
        {
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@serverid", _serverid);
            if (_ServerDestroyed != null)
            {
                arParams[4] = new SqlParameter("@ServerDestroyed", _ServerDestroyed);
                if (_DestroyUnRack != null)
                    arParams[5] = new SqlParameter("@DestroyUnRack", _DestroyUnRack);
                if (_DestroyWipeDrives != null)
                    arParams[6] = new SqlParameter("@DestroyWipeDrives", _DestroyWipeDrives);
                if (_DestroyDispose != null)
                    arParams[7] = new SqlParameter("@DestroyDispose", _DestroyDispose);
            }
            if (_ServerRedeployed != null)
            {
                arParams[8] = new SqlParameter("@ServerRedeployed", _ServerRedeployed);
                if (_RedeployVerifyServerModel != null)
                    arParams[9] = new SqlParameter("@RedeployVerifyServerModel", _RedeployVerifyServerModel);
                if (_RedeployMoveServerToDeploy != null)
                    arParams[10] = new SqlParameter("@RedeployMoveServerToDeploy", _RedeployMoveServerToDeploy);
            }
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWM_DecommissionServer_IM", arParams);
        }

        public void UpdateDecommissionServerIM(int _requestid, int _itemid, int _number,
                    int? _ServerDestroyed, int? _DestroyUnRack, int? _DestroyWipeDrives, int? _DestroyDispose,
                    int? _ServerRedeployed, int? _RedeployVerifyServerModel, int? _RedeployMoveServerToDeploy)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            if (_ServerDestroyed != null)
            {
                arParams[3] = new SqlParameter("@ServerDestroyed", _ServerDestroyed);
                if (_DestroyUnRack != null)
                    arParams[4] = new SqlParameter("@DestroyUnRack", _DestroyUnRack);
                if (_DestroyWipeDrives != null)
                    arParams[5] = new SqlParameter("@DestroyWipeDrives", _DestroyWipeDrives);
                if (_DestroyDispose != null)
                    arParams[6] = new SqlParameter("@DestroyDispose", _DestroyDispose);
            }
            if (_ServerRedeployed != null)
            {
                arParams[7] = new SqlParameter("@ServerRedeployed", _ServerRedeployed);
                if (_RedeployVerifyServerModel != null)
                    arParams[8] = new SqlParameter("@RedeployVerifyServerModel", _RedeployVerifyServerModel);
                if (_RedeployMoveServerToDeploy != null)
                    arParams[9] = new SqlParameter("@RedeployMoveServerToDeploy", _RedeployMoveServerToDeploy);
            }
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWM_DecommissionServer_IM", arParams);
        }

        public string GetDecommissionServerBody(int _requestid, int _itemid, int _number, string _dsn_asset)
        {
            Variables oVariable = new Variables(0);
            Servers oServer = new Servers(0, dsn);
            Asset oAsset = new Asset(0, _dsn_asset);
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            Classes oClass = new Classes(0, dsn);
            Environments oEnvironment = new Environments(0, dsn);
            Locations oLocation = new Locations(0, dsn);
            StringBuilder sbBody = new StringBuilder();
            DataSet ds = GetDecommissionServer(_requestid, _itemid, _number);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intServer = Int32.Parse(ds.Tables[0].Rows[0]["serverid"].ToString());
                int intAsset = 0;
                int intAssetDR = 0;
                if (intServer > 0)
                {
                    DataSet dsAsset = oServer.GetAssets(intServer);
                    foreach (DataRow drAsset in dsAsset.Tables[0].Rows)
                    {
                        if (drAsset["latest"].ToString() == "1")
                            intAsset = Int32.Parse(drAsset["assetid"].ToString());
                        if (drAsset["dr"].ToString() == "1")
                            intAssetDR = Int32.Parse(drAsset["assetid"].ToString());
                    }
                }
                sbBody.Append("<tr><td nowrap><b>Server Name:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                string strName = ds.Tables[0].Rows[0]["servername"].ToString();
                sbBody.Append(strName);
                sbBody.Append("</td></tr>");
                sbBody.Append("<tr><td nowrap><b>Change Control #:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["change"].ToString());
                sbBody.Append("</td></tr>");
                DataSet dsDecom = oAsset.GetDecommission(intAsset, strName, false);
                if ((dsDecom.Tables[0].Rows.Count == 0) || (dsDecom.Tables[0].Rows.Count > 0 && dsDecom.Tables[0].Rows[0]["running"].ToString() != "2"))
                {
                    sbBody.Append("<tr><td nowrap><b>Power Off Date:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    if (ds.Tables[0].Rows[0]["poweroff"].ToString() != "")
                        sbBody.Append(DateTime.Parse(ds.Tables[0].Rows[0]["poweroff"].ToString()).ToLongDateString());
                    else if (intAsset > 0)
                    {
                        if (dsDecom.Tables[0].Rows.Count > 0 && dsDecom.Tables[0].Rows[0]["turnedoff"].ToString() != "")
                            sbBody.Append(DateTime.Parse(dsDecom.Tables[0].Rows[0]["turnedoff"].ToString()).ToLongDateString());
                        else
                            sbBody.Append(" N / A");
                    }
                    else
                        sbBody.Append(" N / A");
                    sbBody.Append("</td></tr>");
                }
                if (ds.Tables[0].Rows[0]["poweroff_new"].ToString() != "")
                {
                    sbBody.Append("<tr class=\"highlight\"><td nowrap><b>*** NEW Power Off Date ***:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(DateTime.Parse(ds.Tables[0].Rows[0]["poweroff_new"].ToString()).ToLongDateString());
                    sbBody.Append("</td></tr>");
                }
                sbBody.Append("<tr><td nowrap><b>Reason:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["reason"].ToString());
                sbBody.Append("</td></tr>");

                if (intServer > 0)
                {
                    if (intAsset > 0)
                    {
                        int intClass = 0;
                        int intEnv = 0;
                        int intAddress = 0;
                        try
                        {
                            intClass = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "classid"));
                            intEnv = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "environmentid"));
                            intAddress = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "addressid"));
                        }
                        catch
                        {
                            DataSet dsCatch = oServer.GetAsset(intAsset);
                            if (dsCatch.Tables[0].Rows.Count > 0)
                            {
                                intClass = Int32.Parse(dsCatch.Tables[0].Rows[0]["classid"].ToString());
                                intEnv = Int32.Parse(dsCatch.Tables[0].Rows[0]["environmentid"].ToString());
                                intAddress = Int32.Parse(dsCatch.Tables[0].Rows[0]["addressid"].ToString());
                            }
                        }
                        int intModel = 0;
                        if (oAsset.Get(intAsset, "modelid") != "")
                            intModel = Int32.Parse(oAsset.Get(intAsset, "modelid"));
                        sbBody.Append("<tr><td nowrap><b>Model:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(oModelsProperties.Get(intModel, "name"));
                        sbBody.Append("</td></tr>");
                        sbBody.Append("<tr><td nowrap><b>Class:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(oClass.Get(intClass, "name"));
                        sbBody.Append("</td></tr>");
                        sbBody.Append("<tr><td nowrap><b>Environment:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(oEnvironment.Get(intEnv, "name"));
                        sbBody.Append("</td></tr>");
                        sbBody.Append("<tr><td nowrap><b>Location:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(oLocation.GetFull(intAddress));
                        sbBody.Append("</td></tr>");
                        sbBody.Append(GetDecommissionServerBody(intAsset, intModel, _dsn_asset, ""));
                        if (intAssetDR > 0)
                        {
                            sbBody.Append(GetDecommissionServerBody(intAssetDR, intModel, _dsn_asset, " (DR)"));
                        }
                    }
                }
                else
                {
                    int intModel = 0;
                    Int32.TryParse(ds.Tables[0].Rows[0]["modelid"].ToString(), out intModel);
                    if (intModel > 0)
                    {
                        sbBody.Append("<tr><td nowrap><b>Model:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(oModelsProperties.Get(intModel, "name"));
                        sbBody.Append("</td></tr>");
                    }
                    int intClass = 0;
                    Int32.TryParse(ds.Tables[0].Rows[0]["classid"].ToString(), out intClass);
                    if (intClass > 0)
                    {
                        sbBody.Append("<tr><td nowrap><b>Class:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(oClass.Get(intClass, "name"));
                        sbBody.Append("</td></tr>");
                    }
                    int intEnv = 0;
                    Int32.TryParse(ds.Tables[0].Rows[0]["environmentid"].ToString(), out intEnv);
                    if (intEnv > 0)
                    {
                        sbBody.Append("<tr><td nowrap><b>Environment:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(oEnvironment.Get(intEnv, "name"));
                        sbBody.Append("</td></tr>");
                    }
                    int intAddress = 0;
                    Int32.TryParse(ds.Tables[0].Rows[0]["addressid"].ToString(), out intAddress);
                    if (intAddress > 0)
                    {
                        sbBody.Append("<tr><td nowrap><b>Location:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(oLocation.GetFull(intAddress));
                        sbBody.Append("</td></tr>");
                    }
                    sbBody.Append("<tr><td nowrap><b>Serial Number:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(ds.Tables[0].Rows[0]["serial"].ToString());
                    sbBody.Append("</td></tr>");
                    if (ds.Tables[0].Rows[0]["serial_dr"].ToString() != "")
                    {
                        sbBody.Append("<tr><td nowrap><b>Serial Number (DR):</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(ds.Tables[0].Rows[0]["serial_dr"].ToString());
                        sbBody.Append("</td></tr>");
                    }
                }
                int intRetrieve = 0;
                Int32.TryParse(ds.Tables[0].Rows[0]["retrieve"].ToString(), out intRetrieve);
                sbBody.Append("<tr><td nowrap><b>Retrieve Special Hardware:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(intRetrieve == 1 ? "Yes" : "No");
                sbBody.Append("</td></tr>");
                if (intRetrieve == 1)
                {
                    sbBody.Append("<tr><td nowrap><b>Provide Description:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(ds.Tables[0].Rows[0]["retrieve_description"].ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append("<tr><td nowrap><b>Mail To (Address):</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(ds.Tables[0].Rows[0]["retrieve_address"].ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append("<tr><td nowrap><b>Mail To (Locator):</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(ds.Tables[0].Rows[0]["retrieve_locator"].ToString());
                    sbBody.Append("</td></tr>");
                }
            }
            return sbBody.ToString();
        }
        public string GetDecommissionServerBody(int _assetid, int _modelid, string _dsn_asset, string _suffix)
        {
            Asset oAsset = new Asset(0, _dsn_asset);
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            StringBuilder sbReturn = new StringBuilder();
            sbReturn.Append("<tr><td nowrap><b>Serial Number");
            sbReturn.Append(_suffix);
            sbReturn.Append(":</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
            sbReturn.Append(oAsset.Get(_assetid, "serial"));
            sbReturn.Append("</td></tr>");
            sbReturn.Append("<tr><td nowrap><b>Asset Tag");
            sbReturn.Append(_suffix);
            sbReturn.Append(":</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
            sbReturn.Append(oAsset.Get(_assetid, "asset"));
            sbReturn.Append("</td></tr>");
            if (oModelsProperties.IsTypeVMware(_modelid) == true)
            {
            }
            else
            {
                DataSet dsHBA = oAsset.GetHBA(_assetid);
                string strHBA = "";
                foreach (DataRow drHBA in dsHBA.Tables[0].Rows)
                {
                    if (strHBA != "")
                        strHBA += ", ";
                    strHBA += drHBA["name"].ToString();
                }
                sbReturn.Append("<tr><td nowrap><b>World Wide Port Names");
                sbReturn.Append(_suffix);
                sbReturn.Append(":</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbReturn.Append(strHBA);
                sbReturn.Append("</td></tr>");
                sbReturn.Append("<tr><td nowrap><b>Room");
                sbReturn.Append(_suffix);
                sbReturn.Append(":</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbReturn.Append(oAsset.GetServerOrBlade(_assetid, "room"));
                sbReturn.Append("</td></tr>");
                sbReturn.Append("<tr><td nowrap><b>Rack");
                sbReturn.Append(_suffix);
                sbReturn.Append(":</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbReturn.Append(oAsset.GetServerOrBlade(_assetid, "rack"));
                sbReturn.Append("</td></tr>");
                sbReturn.Append("<tr><td nowrap><b>Rack Position");
                sbReturn.Append(_suffix);
                sbReturn.Append(":</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbReturn.Append(oAsset.GetServerOrBlade(_assetid, "rackposition"));
                sbReturn.Append("</td></tr>");
                sbReturn.Append("<tr><td nowrap><b>Fabric");
                sbReturn.Append(_suffix);
                sbReturn.Append(":</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbReturn.Append(oModelsProperties.Get(_modelid, "fabric") == "0" ? "Cisco" : "Brocade");
                sbReturn.Append("</td></tr>");
                if (oAsset.GetServerOrBlade(_assetid, "enclosureid") != "")
                {
                    int intEnclosure = Int32.Parse(oAsset.GetServerOrBlade(_assetid, "enclosureid"));
                    if (intEnclosure > 0)
                    {
                        sbReturn.Append("<tr><td nowrap><b>Enclosure");
                        sbReturn.Append(_suffix);
                        sbReturn.Append(":</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbReturn.Append(oAsset.Get(intEnclosure, "name"));
                        sbReturn.Append("</td></tr>");
                        sbReturn.Append("<tr><td nowrap><b>Slot");
                        sbReturn.Append(_suffix);
                        sbReturn.Append(":</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbReturn.Append(oAsset.GetServerOrBlade(_assetid, "slot"));
                        sbReturn.Append("</td></tr>");
                    }
                }
            }
            return sbReturn.ToString();
        }

        public DataSet GetWMServerArchiveServerNames()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT DISTINCT servername FROM cv_WM_server_archive WHERE deleted = 0");
        }


        public void AddPNCDNSConflict(int _requestid, int _itemid, int _number, int _answerid, string ObjectAddress, string ObjectName, string ObjectClass, string Aliases, string DomainName, string NameService, string DynamicDNSUpdate)
        {
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@answerid", _answerid);
            arParams[4] = new SqlParameter("@ObjectAddress", ObjectAddress);
            arParams[5] = new SqlParameter("@ObjectName", ObjectName);
            arParams[6] = new SqlParameter("@ObjectClass", ObjectClass);
            arParams[7] = new SqlParameter("@Aliases", Aliases);
            arParams[8] = new SqlParameter("@DomainName", DomainName);
            arParams[9] = new SqlParameter("@NameService", NameService);
            arParams[10] = new SqlParameter("@DynamicDNSUpdate", DynamicDNSUpdate);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addPNCDNSConflict", arParams);
        }
        public DataSet GetPNCDNSConflict(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPNCDNSConflict", arParams);
        }
        public DataSet GetPNCDNSConflict(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPNCDNSConflictAnswer", arParams);
        }
        public DataSet UpdatePNCDNSConflict(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_updatePNCDNSConflict", arParams);
        }
        public string GetPNCDNSConflictBody(int _requestid, int _itemid, int _number)
        {
            DataSet ds = GetPNCDNSConflict(_requestid, _itemid, _number);
            Users oUser = new Users(0, dsn);
            Requests oRequest = new Requests(0, dsn);
            StringBuilder sbDetails = new StringBuilder();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                int intUser = oRequest.GetUser(_requestid);
                sbDetails.Append("<tr><td>IP Address:</td><td>" + dr["ObjectAddress"].ToString() + "</td></tr>");
                sbDetails.Append("<tr><td>Device Name:</td><td>" + dr["ObjectName"].ToString() + "</td></tr>");
                sbDetails.Append("<tr><td>Class:</td><td>" + dr["ObjectClass"].ToString() + "</td></tr>");
                sbDetails.Append("<tr><td>Aliases:</td><td>" + dr["Aliases"].ToString() + "</td></tr>");
                sbDetails.Append("<tr><td>Domain:</td><td>" + dr["DomainName"].ToString() + "</td></tr>");
                sbDetails.Append("<tr><td>Name Service:</td><td>" + dr["NameService"].ToString() + "</td></tr>");
                sbDetails.Append("<tr><td>Dynamic DNS Update:</td><td>" + dr["DynamicDNSUpdate"].ToString() + "</td></tr>");
                sbDetails.Append("<tr><td>Requestor:</td><td>" + oUser.GetFullName(intUser) + " (" + oUser.GetName(intUser) + ")" + "</td></tr>");
            }
            return sbDetails.ToString();
        }

    }
}
