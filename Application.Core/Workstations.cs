using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;
using System.Data.SqlTypes;

namespace NCC.ClearView.Application.Core
{
    public class Workstations
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Workstations(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        //public int Add(int _requestid, int _answerid, int _number, int _osid, int _spid, int _domainid, int _configured)
        //{
        //    arParams = new SqlParameter[8];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    arParams[1] = new SqlParameter("@answerid", _answerid);
        //    arParams[2] = new SqlParameter("@number", _number);
        //    arParams[3] = new SqlParameter("@osid", _osid);
        //    arParams[4] = new SqlParameter("@spid", _spid);
        //    arParams[5] = new SqlParameter("@domainid", _domainid);
        //    arParams[6] = new SqlParameter("@configured", _configured);
        //    arParams[7] = new SqlParameter("@id", SqlDbType.Int);
        //    arParams[7].Direction = ParameterDirection.Output;
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWorkstation", arParams);
        //    return Int32.Parse(arParams[7].Value.ToString());
        //}
        //public void Update(int _id, int _osid, int _spid, int _domainid, int _configured)
        //{
        //    arParams = new SqlParameter[5];
        //    arParams[0] = new SqlParameter("@id", _id);
        //    arParams[1] = new SqlParameter("@osid", _osid);
        //    arParams[2] = new SqlParameter("@spid", _spid);
        //    arParams[3] = new SqlParameter("@domainid", _domainid);
        //    arParams[4] = new SqlParameter("@configured", _configured);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstation", arParams);
        //}
        //public void UpdateAsset(int _id, int _assetid)
        //{
        //    arParams = new SqlParameter[2];
        //    arParams[0] = new SqlParameter("@id", _id);
        //    arParams[1] = new SqlParameter("@assetid", _assetid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationAsset", arParams);
        //}
        //public void UpdateWorkstationIPAddress(int _id, int _ipaddressid)
        //{
        //    arParams = new SqlParameter[2];
        //    arParams[0] = new SqlParameter("@id", _id);
        //    arParams[1] = new SqlParameter("@ipaddressid", _ipaddressid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationIPAddress", arParams);
        //}
        public void UpdateStep(int _id, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationStep", arParams);
        }
        //public DataSet Get(int _answerid, int _number)
        //{
        //    arParams = new SqlParameter[2];
        //    arParams[0] = new SqlParameter("@answerid", _answerid);
        //    arParams[1] = new SqlParameter("@number", _number);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstation", arParams);
        //}
        public DataSet GetAnswer(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationAnswer", arParams);
        }
        //public DataSet GetAsset(int _assetid)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@assetid", _assetid);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationsAsset", arParams);
        //}
        //public DataSet GetRequests(int _requestid)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationsRequest", arParams);
        //}
        //public DataSet Gets()
        //{
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstations");
        //}
        //public DataSet GetSteps(int _id, int _answerid, int _step)
        //{
        //    arParams = new SqlParameter[3];
        //    arParams[0] = new SqlParameter("@id", _id);
        //    arParams[1] = new SqlParameter("@answerid", _answerid);
        //    arParams[2] = new SqlParameter("@step", _step);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationsStep", arParams);
        //}
        //public DataSet Get(int _id)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@id", _id);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationId", arParams);
        //}
        //public string Get(int _id, string _column)
        //{
        //    DataSet ds = Get(_id);
        //    if (ds.Tables[0].Rows.Count > 0)
        //        return ds.Tables[0].Rows[0][_column].ToString();
        //    else
        //        return "";
        //}
        //public void Start(int _requestid)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@requestid", _requestid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationStart", arParams);
        //}
        //public void NextStep(int _id) 
        //{
        //    int intStep = Int32.Parse(Get(_id, "step"));
        //    UpdateStep(_id, intStep + 1);
        //}



        // Virtual
        public DataSet GetVirtualAsset(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualAsset", arParams);
        }
        public int GetVirtualStep(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            DataSet dsStep = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualStep", arParams);
            int intStep = 0;
            if (dsStep.Tables[0].Rows.Count > 0)
                Int32.TryParse(dsStep.Tables[0].Rows[0]["step"].ToString(), out intStep);
            return intStep;
        }
        public DataSet GetVirtual(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualId", arParams);
        }
        public string GetVirtual(int _id, string _column)
        {
            DataSet ds = GetVirtual(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetVirtual(int _answerid, int _number)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtual", arParams);
        }
        
        public DataSet GetVirtualNames()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualNames");
        }
        public DataSet GetVirtualName(int _nameid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@nameid", _nameid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualName", arParams);
        }
        public DataSet GetVirtualAnswer(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualAnswer", arParams);
        }
        public DataSet GetVirtualRequests(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualsRequest", arParams);
        }
        public DataSet GetVirtualSteps(int _id, int _answerid, int _step)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@answerid", _answerid);
            arParams[2] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualsStep", arParams);
        }
        public int AddVirtual(int _requestid, int _answerid, int _number, int _vmware, int _modelid, int _osid, int _spid, int _domainid, int _ramid, int _recovery, int _internal, int _hddid, int _cpuid, int _configured, int _accounts)
        {
            arParams = new SqlParameter[16];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@answerid", _answerid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@vmware", _vmware);
            arParams[4] = new SqlParameter("@modelid", _modelid);
            arParams[5] = new SqlParameter("@osid", _osid);
            arParams[6] = new SqlParameter("@spid", _spid);
            arParams[7] = new SqlParameter("@domainid", _domainid);
            arParams[8] = new SqlParameter("@ramid", _ramid);
            arParams[9] = new SqlParameter("@recovery", _recovery);
            arParams[10] = new SqlParameter("@internal", _internal);
            arParams[11] = new SqlParameter("@hddid", _hddid);
            arParams[12] = new SqlParameter("@cpuid", _cpuid);
            arParams[13] = new SqlParameter("@configured", _configured);
            arParams[14] = new SqlParameter("@accounts", _accounts);
            arParams[15] = new SqlParameter("@id", SqlDbType.Int);
            arParams[15].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWorkstationVirtual", arParams);
            return Int32.Parse(arParams[15].Value.ToString());
        }
        public int AddVirtualCopy(int _workstationid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@number", _number);
            arParams[2] = new SqlParameter("@id", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWorkstationVirtualCopy", arParams);
            return Int32.Parse(arParams[2].Value.ToString());
        }
        public void UpdateVirtual(int _id, int _virtualhostid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@virtualhostid", _virtualhostid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualHost", arParams);
        }
        public void UpdateVirtual(int _id, int _osid, int _spid, int _domainid, int _configured)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@osid", _osid);
            arParams[2] = new SqlParameter("@spid", _spid);
            arParams[3] = new SqlParameter("@domainid", _domainid);
            arParams[4] = new SqlParameter("@configured", _configured);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtual", arParams);
        }
        public void UpdateVirtual(int _id, int _osid, int _spid, int _domainid, int _ramid, int _recovery, int _internal, int _hddid, int _cpuid, int _configured)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@osid", _osid);
            arParams[2] = new SqlParameter("@spid", _spid);
            arParams[3] = new SqlParameter("@domainid", _domainid);
            arParams[4] = new SqlParameter("@ramid", _ramid);
            arParams[5] = new SqlParameter("@recovery", _recovery);
            arParams[6] = new SqlParameter("@internal", _internal);
            arParams[7] = new SqlParameter("@hddid", _hddid);
            arParams[8] = new SqlParameter("@cpuid", _cpuid);
            arParams[9] = new SqlParameter("@configured", _configured);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualRequest", arParams);
        }
        public void UpdateVirtualCompleted(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualCompleted", arParams);
        }
        public void UpdateVirtualDecommissioned(int _id, string _decommissioned)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@decommissioned", (_decommissioned == "" ? SqlDateTime.Null : DateTime.Parse(_decommissioned)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualDecommissioned", arParams);
        }
        public void UpdateVirtualRecommissioned(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualRecommissioned", arParams);
        }
        public void UpdateVirtualName(int _id, int _nameid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@nameid", _nameid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualName", arParams);
        }
        public void DeleteVirtual(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteWorkstationVirtual", arParams);
        }
        public void NextVirtualStep(int _id)
        {
            int intStep = Int32.Parse(GetVirtual(_id, "step"));
            OnDemand oOnDemand = new OnDemand(0, dsn);
            int intRedo = (intStep * -1);
            if (oOnDemand.GetStepDoneWorkstation(_id, intRedo).Tables[0].Rows.Count > 0)
            {
                // Found a (-)STEP...so this was a redo step.
                DataSet dsRedo = oOnDemand.GetStepDoneWorkstation(_id, intStep);
                if (dsRedo.Tables[0].Rows.Count > 0)
                {
                    // Get new result
                    string strNewResult = dsRedo.Tables[0].Rows[0]["result"].ToString();
                    // Delete new record
                    oOnDemand.DeleteStepDoneWorkstation(_id, intStep);
                    // Update the redone step to positive step (to replace the one just deleted)
                    oOnDemand.UpdateStepDoneWorkstationRedo(_id, intRedo);
                    // Update the redone step with the new result
                    oOnDemand.UpdateStepDoneWorkstationResult(_id, intStep, strNewResult, false);
                }
                // If rebuild, move to redo the next step
                DataSet dsRebuild = GetVirtualRebuild(_id);
                foreach (DataRow drRebuild in dsRebuild.Tables[0].Rows)
                    if (drRebuild["submitted"].ToString() != ""
                        && drRebuild["scheduled"].ToString() != ""
                        && DateTime.Parse(drRebuild["scheduled"].ToString()) < DateTime.Now
                        && drRebuild["started"].ToString() != ""
                        && drRebuild["completed"].ToString() == ""
                        && drRebuild["cancelled"].ToString() == "")
                    {
                        // Set the next step to redo...and so on...
                        oOnDemand.UpdateStepDoneWorkstationRedo(_id, intStep + 1);
                        break;
                    }
            }
            UpdateVirtualStep(_id, intStep + 1);
        }
        public void UpdateVirtualStep(int _id, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualStep", arParams);
        }

        // VMware
        #region VMware
        public void UpdateVirtualAccounts(int _id, int _accounts)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@accounts", _accounts);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualAccounts", arParams);
        }
        public DataSet GetVirtualTypes(int _typeid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@typeid", _typeid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualTypes", arParams);
        }
        public DataSet GetScheduler(int _typeid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@typeid", _typeid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSchedulerRestartWorkstation", arParams);
        }
        public void UpdateVirtualZeus(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualZeus", arParams);
        }
        public void UpdateVirtualZeusError(int _id, int _zeus_error)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@zeus_error", _zeus_error);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualZeusError", arParams);
        }

        public int AddVirtualError(int _requestid, int _itemid, int _number, int _workstationid, int _step, string _reason)
        {
            Errors oError = new Errors(user, dsn);
            oError.CheckError(GetVirtualErrorLatest(_workstationid, _step));

            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@workstationid", _workstationid);
            arParams[4] = new SqlParameter("@step", _step);
            arParams[5] = new SqlParameter("@reason", _reason);
            arParams[6] = new SqlParameter("@id", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWorkstationVirtualError", arParams);
            return Int32.Parse(arParams[6].Value.ToString());
        }

        public DataSet GetVirtualErrorLatest(int _workstationid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualErrorLatest", arParams);
        }
        public DataSet GetVirtualError(int _workstationid, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualError", arParams);
        }
        public DataSet GetVirtualErrors(int _workstationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualErrors", arParams);
        }

        public DataSet GetVirtualErrorsByRequest(int _requestid, int _number)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@RequestId", _requestid);
            arParams[1] = new SqlParameter("@Number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualErrorsByRequest", arParams);
        }
        public string GetApprovalSummary(int _requestid)
        {
            StringBuilder sbReturn = new StringBuilder();
            Forecast oForecast = new Forecast(0, dsn);
            Classes oClass = new Classes(0, dsn);
            Environments oEnvironment = new Environments(0, dsn);
            VirtualRam oVirtualRam = new VirtualRam(0, dsn);
            VirtualHDD oVirtualHDD = new VirtualHDD(0, dsn);
            VirtualCPU oVirtualCPU = new VirtualCPU(0, dsn);
            Locations oLocation = new Locations(0, dsn);
            OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);
            ServicePacks oServicePack = new ServicePacks(0, dsn);
            Users oUser = new Users(0, dsn);
            CostCenter oCostCenter = new CostCenter(0, dsn);
            DataSet ds = oForecast.GetAnswerService(_requestid);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intAddress = 0;
                Int32.TryParse(ds.Tables[0].Rows[0]["addressid"].ToString(), out intAddress);
                if (intAddress > 0)
                {
                    sbReturn.Append("<tr><td valign=\"top\">Location:</td><td valign=\"top\">");
                    sbReturn.Append(oLocation.GetFull(intAddress));
                    sbReturn.Append("</td></tr>");
                }
                int intClass = 0;
                Int32.TryParse(ds.Tables[0].Rows[0]["classid"].ToString(), out intClass);
                if (intClass > 0)
                {
                    sbReturn.Append("<tr><td valign=\"top\">Class:</td><td valign=\"top\">");
                    sbReturn.Append(oClass.Get(intClass, "name"));
                    sbReturn.Append("</td></tr>");
                }
                int intEnv = 0;
                Int32.TryParse(ds.Tables[0].Rows[0]["environmentid"].ToString(), out intEnv);
                if (intEnv > 0)
                {
                    sbReturn.Append("<tr><td valign=\"top\">Environment:</td><td valign=\"top\">");
                    sbReturn.Append(oEnvironment.Get(intEnv, "name"));
                    sbReturn.Append("</td></tr>");
                }
                int intQuantity = 0;
                Int32.TryParse(ds.Tables[0].Rows[0]["quantity"].ToString(), out intQuantity);
                int intManager = 0;
                Int32.TryParse(ds.Tables[0].Rows[0]["appcontact"].ToString(), out intManager);
                if (intManager > 0)
                {
                    sbReturn.Append("<tr><td valign=\"top\">Workstation Manager:</td><td valign=\"top\">");
                    sbReturn.Append(oUser.GetFullName(intManager));
                    sbReturn.Append("</td></tr>");
                }
                int intCostCenter = 0;
                Int32.TryParse(ds.Tables[0].Rows[0]["costcenterid"].ToString(), out intCostCenter);
                if (intCostCenter > 0)
                {
                    sbReturn.Append("<tr><td valign=\"top\">Cost Center:</td><td valign=\"top\">");
                    sbReturn.Append(oCostCenter.GetName(intCostCenter));
                    sbReturn.Append("</td></tr>");
                }
                int intAnswer = 0;
                Int32.TryParse(ds.Tables[0].Rows[0]["id"].ToString(), out intAnswer);
                if (intAnswer > 0)
                {
                    ds = GetVirtualAnswer(intAnswer);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        sbReturn.Append("<tr><td valign=\"top\">Employee Type:</td><td valign=\"top\">");
                        sbReturn.Append(ds.Tables[0].Rows[0]["internal"].ToString() == "1" ? "Internal User" : "External User");
                        sbReturn.Append("</td></tr>");
                        sbReturn.Append("<tr><td valign=\"top\">DR Testing:</td><td valign=\"top\">");
                        sbReturn.Append(ds.Tables[0].Rows[0]["recovery"].ToString() == "1" ? "Yes" : "No");
                        sbReturn.Append("</td></tr>");
                        sbReturn.Append("<tr><td valign=\"top\">Quantity:</td><td valign=\"top\">");
                        sbReturn.Append(intQuantity.ToString());
                        sbReturn.Append("</td></tr>");
                        int intOS = 0;
                        Int32.TryParse(ds.Tables[0].Rows[0]["osid"].ToString(), out intOS);
                        if (intOS > 0)
                        {
                            sbReturn.Append("<tr><td valign=\"top\">Operating System:</td><td valign=\"top\">");
                            sbReturn.Append(oOperatingSystem.Get(intOS, "name"));
                            sbReturn.Append("</td></tr>");
                        }
                        int intSP = 0;
                        Int32.TryParse(ds.Tables[0].Rows[0]["spid"].ToString(), out intSP);
                        if (intSP > 0)
                        {
                            sbReturn.Append("<tr><td valign=\"top\">Service Pack:</td><td valign=\"top\">");
                            sbReturn.Append(oServicePack.Get(intSP, "name"));
                            sbReturn.Append("</td></tr>");
                        }
                        int intRAM = 0;
                        Int32.TryParse(ds.Tables[0].Rows[0]["ramid"].ToString(), out intRAM);
                        if (intRAM > 0)
                        {
                            sbReturn.Append("<tr><td valign=\"top\">RAM:</td><td valign=\"top\">");
                            sbReturn.Append(oVirtualRam.Get(intRAM, "name"));
                            sbReturn.Append("</td></tr>");
                        }
                        int intCPU = 0;
                        Int32.TryParse(ds.Tables[0].Rows[0]["cpuid"].ToString(), out intCPU);
                        if (intCPU > 0)
                        {
                            sbReturn.Append("<tr><td valign=\"top\">CPU(s):</td><td valign=\"top\">");
                            sbReturn.Append(oVirtualCPU.Get(intCPU, "name"));
                            sbReturn.Append("</td></tr>");
                        }
                        int intHDD = 0;
                        Int32.TryParse(ds.Tables[0].Rows[0]["hddid"].ToString(), out intHDD);
                        if (intHDD > 0)
                        {
                            sbReturn.Append("<tr><td valign=\"top\">Hard Drive:</td><td valign=\"top\">");
                            sbReturn.Append(oVirtualHDD.Get(intHDD, "name"));
                            sbReturn.Append("</td></tr>");
                        }
                        int intWorkstation = 0;
                        Int32.TryParse(ds.Tables[0].Rows[0]["id"].ToString(), out intWorkstation);
                        if (intWorkstation > 0)
                        {
                            DataSet dsAccounts = GetAccountsVMware(intWorkstation);
                            StringBuilder strAccounts = new StringBuilder();
                            foreach (DataRow drAccount in dsAccounts.Tables[0].Rows)
                            {
                                int intAccount = Int32.Parse(drAccount["userid"].ToString());
                                strAccounts.Append(oUser.GetFullName(intAccount));
                                strAccounts.Append(" (");
                                strAccounts.Append(oUser.GetName(intAccount));
                                strAccounts.Append(")<br/>");
                            }
                            sbReturn.Append("<tr><td valign=\"top\">Accounts:</td><td valign=\"top\">");
                            sbReturn.Append(strAccounts.ToString());
                            sbReturn.Append("</td></tr>");
                        }
                    }
                    else
                    {
                        ds = oForecast.GetAnswerWorkstation(intAnswer);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int intRAM = 0;
                            Int32.TryParse(ds.Tables[0].Rows[0]["ramid"].ToString(), out intRAM);
                            if (intRAM > 0)
                            {
                                sbReturn.Append("<tr><td valign=\"top\">RAM:</td><td valign=\"top\">");
                                sbReturn.Append(oVirtualRam.Get(intRAM, "name"));
                                sbReturn.Append("</td></tr>");
                            }
                            int intCPU = 0;
                            Int32.TryParse(ds.Tables[0].Rows[0]["cpuid"].ToString(), out intCPU);
                            if (intCPU > 0)
                            {
                                sbReturn.Append("<tr><td valign=\"top\">CPU(s):</td><td valign=\"top\">");
                                sbReturn.Append(oVirtualCPU.Get(intCPU, "name"));
                                sbReturn.Append("</td></tr>");
                            }
                            sbReturn.Append("<tr><td valign=\"top\">Employee Type:</td><td valign=\"top\">");
                            sbReturn.Append(ds.Tables[0].Rows[0]["internal"].ToString() == "1" ? "Internal User" : "External User");
                            sbReturn.Append("</td></tr>");
                            sbReturn.Append("<tr><td valign=\"top\">DR Testing:</td><td valign=\"top\">");
                            sbReturn.Append(ds.Tables[0].Rows[0]["recovery"].ToString() == "1" ? "Yes" : "No");
                            sbReturn.Append("</td></tr>");
                            sbReturn.Append("<tr><td valign=\"top\">Quantity:</td><td valign=\"top\">");
                            sbReturn.Append(intQuantity.ToString());
                            sbReturn.Append("</td></tr>");
                            int intHDD = 0;
                            Int32.TryParse(ds.Tables[0].Rows[0]["hddid"].ToString(), out intHDD);
                            if (intHDD > 0)
                            {
                                sbReturn.Append("<tr><td valign=\"top\">Hard Drive:</td><td valign=\"top\">");
                                sbReturn.Append(oVirtualHDD.Get(intHDD, "name"));
                                sbReturn.Append("</td></tr>");
                            }
                            int intOS = 0;
                            Int32.TryParse(ds.Tables[0].Rows[0]["osid"].ToString(), out intOS);
                            if (intOS > 0)
                            {
                                sbReturn.Append("<tr><td valign=\"top\">Operating System:</td><td valign=\"top\">");
                                sbReturn.Append(oOperatingSystem.Get(intOS, "name"));
                                sbReturn.Append("</td></tr>");
                            }
                        }
                    }
                }
            }
            return sbReturn.ToString();
        }

        public string GetVirtualErrorDetailsBody(int _requestid, int _number, int _environment)
        {
            Variables oVariable = new Variables(_environment);
            Functions oFunction = new Functions(0, dsn, _environment);
            StringBuilder sbBody = new StringBuilder();
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";

            string strSpacerTD = "<td style=\"width:10;text-align:left\"></td>";
            string strTRstart = "<tr>";
            string strTRend = "</tr>";
            string strTDstart = "<td>";
            string strTDend = "</td>";


            DataSet dsError = GetVirtualErrorsByRequest(_requestid, _number);
            if (dsError.Tables[0].Rows.Count > 0)
            {
                DataRow drError = dsError.Tables[0].Rows[0];
                sbBody.Append(strTRstart + strTDstart + "<b>Error Message:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["reason"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Device Name:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["workstationname"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Model:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["ModelName"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Serial Number:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["serial"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Asset Number:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["assettag"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Class:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["class"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Environment:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["Environment"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Domain:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["domain"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                //sbBody.Append(strTRstart + strTDstart + "<b>Enclosure:</b>" + strTDend + strSpacerTD);
                //sbBody.Append(strTDstart + " NEED To Fix" + strTDend + strTRend);
                //sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Virtual Center Server:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["VirtualCenterName"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Data Center:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["DataCenterName"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Folder:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["FolderName"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Cluster:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["ClusterName"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Host Name:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["HostName"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Data Store:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["DataStoreName"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Executed Date:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["ExecutedDate"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Executed By:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["ExecutedByUserName"].ToString() + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strTRstart + strTDstart + "<b>Elapsed Time:</b>" + strTDend + strSpacerTD);
                sbBody.Append(strTDstart + drError["ElapsedTime"].ToString() +" Days" + strTDend + strTRend);
                sbBody.Append(strSpacerRow);

                sbBody.Append(strSpacerRow);
            }

            if (sbBody.ToString() != "")
            {
                sbBody.Insert(0, "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                sbBody.Append("</table>");
            }
            return sbBody.ToString();


        }

        public DataSet GetVirtualErrors()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualErrorsAll");
        }
        public void UpdateVirtualError(int _workstationid, int _step, int _errorid, int _userid)
        {
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            // Clear All Provisioning Errors
            DataSet dsErrors = GetVirtualErrors(_workstationid);
            foreach (DataRow drError in dsErrors.Tables[0].Rows)
            {
                if (drError["fixed"].ToString() == "")
                {
                    int intRequest = Int32.Parse(drError["requestid"].ToString());
                    int intItem = Int32.Parse(drError["itemid"].ToString());
                    int intNumber = Int32.Parse(drError["number"].ToString());

                    DataSet dsRR = oResourceRequest.GetAllItem(intRequest, intItem, intNumber);
                    foreach (DataRow drRR in dsRR.Tables[0].Rows)
                    {
                        int intRRW = Int32.Parse(drRR["RRWID"].ToString());
                        oResourceRequest.UpdateWorkflowStatus(intRRW, (int)ResourceRequestStatus.Closed, true);
                        int intRR = Int32.Parse(drRR["RRID"].ToString());
                        oResourceRequest.UpdateStatusOverall(intRR, (int)ResourceRequestStatus.Closed);
                    }
                }
            }

            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@errorid", _errorid);
            arParams[3] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualError", arParams);

            OnDemand oOnDemand = new OnDemand(user, dsn);
            oOnDemand.DeleteStepDoneWorkstation(_workstationid, _step);
            UpdateVirtualZeusError(_workstationid, 0);

        }
        public void UpdateVirtualError(int _id, string _incident, int _assigned)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@incident", _incident);
            arParams[2] = new SqlParameter("@assigned", _assigned);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualErrorIncident", arParams);
        }
        public void UpdateVirtualNetwork(int _id, int _networkid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@networkid", _networkid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualNetwork", arParams);
        }

        public void AddVirtualOutput(int _workstationid, string _type, string _output)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@type", _type);
            arParams[2] = new SqlParameter("@output", _output);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWorkstationVirtualOutput", arParams);
        }
        public DataSet GetVirtualOutput(int _workstationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualOutputs", arParams);
        }
        public DataSet GetVirtualOutput(int _workstationid, string _type)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@type", _type);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualOutput", arParams);
        }
        #endregion

        // Microsoft Virtual Workstation
        #region Microsoft Virtual Workstation
        public DataSet GetVirtuals(int _virtualhostid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@virtualhostid", _virtualhostid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtuals", arParams);
        }
        public void UpdateVirtualAsset(int _id, int _assetid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@assetid", _assetid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualAsset", arParams);
        }
        public void UpdateVirtualRemote(int _id, int _remoteid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@remoteid", _remoteid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualRemote", arParams);
        }
        public void StartVirtual(int _requestid, string _remote_dsn, string _asset_dsn, int _environment, string _zeus_dsn)
        {
            StartVirtual(_requestid);
            DataSet ds = GetVirtualRequests(_requestid);
            OperatingSystems oOperatingSystem = new OperatingSystems(user, dsn);
            Workstations oWorkstation = new Workstations(user, dsn);
            Workstations oRemote = new Workstations(user, _remote_dsn);
            Requests oRequest = new Requests(user, dsn);
            Users oUser = new Users(user, dsn);
            Forecast oForecast = new Forecast(user, dsn);
            Classes oClass = new Classes(user, dsn);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intID = Int32.Parse(dr["id"].ToString());
                int intOS = Int32.Parse(dr["osid"].ToString());
                // Create Name
                string strCode = oOperatingSystem.Get(intOS, "code");
                int intClass = Int32.Parse(dr["classid"].ToString());
                int intName = Int32.Parse(dr["nameid"].ToString());
                if (intName == 0)
                    intName = oWorkstation.AddName((oClass.IsProd(intClass) ? "W" : "T"), strCode, "V", intClass, Int32.Parse(dr["environmentid"].ToString()), 0, false);
                oWorkstation.UpdateVirtualName(intID, intName);
                string strName = oWorkstation.GetName(intName);
                int intAnswer = Int32.Parse(dr["answerid"].ToString());
                int intOwner = Int32.Parse(oForecast.GetAnswer(intAnswer, "appcontact"));
                oRemote.AddRemoteNotify(strName, oUser.GetName(intOwner));
                int intRequestor = oRequest.GetUser(_requestid);
                oRemote.AddRemoteNotify(strName, oUser.GetName(intRequestor));
                AssignHost(intID, _remote_dsn, _asset_dsn, _environment, _zeus_dsn);
            }
        }
        public void StartVirtual(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualStart", arParams);
        }
        public void AssignHost(int _id, string _remote_dsn, string _asset_dsn, int _environment, string _zeus_dsn)
        {
            Domains oDomain = new Domains(0, dsn);
            Asset oAsset = new Asset(0, _asset_dsn);
            OnDemand oOnDemand = new OnDemand(user, dsn);
            Workstations oRemote = new Workstations(user, _remote_dsn);
            Users oUser = new Users(user, dsn);
            Forecast oForecast = new Forecast(user, dsn);
            Zeus oZeus = new Zeus(user, _zeus_dsn);
            ServicePacks oServicePack = new ServicePacks(user, dsn);
            OperatingSystems oOperatingSystem = new OperatingSystems(user, dsn);
            Workstations oWorkstation = new Workstations(user, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(user, dsn);
            Classes oClass = new Classes(user, dsn);
            Variables oVariable = new Variables(_environment);
            DataSet ds = GetVirtual(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intOS = Int32.Parse(ds.Tables[0].Rows[0]["osid"].ToString());
                string strCode = oOperatingSystem.Get(intOS, "code");
                int intDomain = Int32.Parse(ds.Tables[0].Rows[0]["domainid"].ToString());
                int intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                int intEnvironment = Int32.Parse(oDomain.Get(intDomain, "environment"));
                int intName = Int32.Parse(ds.Tables[0].Rows[0]["nameid"].ToString());
                if (intName == 0)
                    intName = oWorkstation.AddName((oClass.IsProd(intClass) ? "W" : "T"), strCode, "V", intClass, Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString()), 0, false);
                oWorkstation.UpdateVirtualName(_id, intName);
                string strName = oWorkstation.GetName(intName);
                int intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                int intUser = Int32.Parse(oForecast.GetAnswer(intAnswer, "userid"));
                bool boolError = true;
                bool boolNotify = false;
                int intCount = 0;
                DataSet dsHosts = oAsset.GetVirtualHosts(intEnvironment, intOS);
                foreach (DataRow drHost in dsHosts.Tables[0].Rows)
                {
                    int intHostID = Int32.Parse(drHost["assetid"].ToString());
                    int intMax = Int32.Parse(drHost["guests"].ToString());
                    int intCurrent = GetVirtuals(intHostID).Tables[0].Rows.Count;
                    if (intCurrent < intMax)
                    {
                        if (intCount == 0)
                        {
                            boolError = false;
                            UpdateVirtual(_id, intHostID);
                            if (intCurrent + 1 == intMax)
                                boolNotify = true;
                        }
                        intCount++;
                    }
                }
                if (intCount < 2)
                    boolNotify = true;
                Functions oFunction = new Functions(0, dsn, _environment);
                if (boolNotify == true)
                {
                    // Send email notification to Inventory Managers to obtain a new host
                    string strBody = "";
                    string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
                    dsHosts = oAsset.GetVirtualHosts(intEnvironment, intOS);
                    foreach (DataRow drHost in dsHosts.Tables[0].Rows)
                    {
                        int intHostID = Int32.Parse(drHost["assetid"].ToString());
                        int intMax = Int32.Parse(drHost["guests"].ToString());
                        int intCurrent = GetVirtuals(intHostID).Tables[0].Rows.Count;
                        DataSet dsAsset = oAsset.GetStatus(intHostID);
                        if (dsAsset.Tables[0].Rows.Count > 0)
                        {
                            int intLeft = intMax - intCurrent;
                            strBody += "<tr><td nowrap><b>" + dsAsset.Tables[0].Rows[0]["name"].ToString() + ":</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + intLeft.ToString() + "</td></tr>";
                            strBody += strSpacerRow;
                        }
                    }
                    if (strBody != "")
                        strBody = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\"><tr><td><b>Host Name</b></td><td><b>Available Guests</b></td>" + strBody + "</table>";
                    strBody = "<p><b>A new virtual workstation host is needed. You are required to act on this request</b></p><p>Domain:" + oDomain.Get(intDomain, "name") + "<br/>Operating System:" + oOperatingSystem.Get(intOS, "name") + "</p><p>The following list represents the hosts that are still available...</p>" + strBody;
                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING,EMAILGRP_INVENTORY_MANAGER");
                    oFunction.SendEmail("Virtual Workstation Host Required", strEMailIdsBCC, "", "", "Virtual Workstation Host Required", strBody, true, false);
                }
                if (boolError == true)
                {
                    string strError = "<p><b>There was a problem assigning a workstation to a host.</b></p><p>Domain:" + oDomain.Get(intDomain, "name") + "<br/>Operating System:" + oOperatingSystem.Get(intOS, "name") + "<br/>Requested By:" + oUser.GetFullName(intUser) + "</p>";
                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                    oFunction.SendEmail("Virtual Workstation Host Assignment Error", strEMailIdsBCC, "", "", "Virtual Workstation Host Assignment Error", strError, true, false);
                }
                ds = GetVirtual(_id);
                int intSP = Int32.Parse(ds.Tables[0].Rows[0]["spid"].ToString());
                if (ds.Tables[0].Rows[0]["hostname"].ToString() != "")
                {
                    int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
                    int intType = oModelsProperties.GetType(intModel);
                    DataSet dsSteps = oOnDemand.GetSteps(intType, 1);
                    string strSerial = oAsset.GetGuestSerial(intModel);
                    int intAsset = oAsset.AddGuest(ds.Tables[0].Rows[0]["name"].ToString(), intModel, strSerial, strSerial, (int)AssetStatus.Available, user, DateTime.Now, Int32.Parse(ds.Tables[0].Rows[0]["hostid"].ToString()), double.Parse(ds.Tables[0].Rows[0]["ram"].ToString()), double.Parse("1.00"), double.Parse(ds.Tables[0].Rows[0]["hdd"].ToString()), Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString()), Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString()), Int32.Parse(ds.Tables[0].Rows[0]["addressid"].ToString()), 0, 0);
                    oAsset.AddStatus(intAsset, ds.Tables[0].Rows[0]["name"].ToString(), (int)AssetStatus.InUse, user, DateTime.Now);
                    oZeus.AddBuild(0, _id, 0, strSerial, strSerial, ds.Tables[0].Rows[0]["name"].ToString(), "BASIC", oOperatingSystem.Get(intOS, "zeus_os"), oOperatingSystem.Get(intOS, "zeus_os_version"), Int32.Parse(oServicePack.Get(intSP, "number")), oOperatingSystem.Get(intOS, "zeus_build_type"), oDomain.Get(intDomain, "name"), intEnvironment, "SERVER", 0, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0);
                    int intRemote = oRemote.AddRemoteVirtual(intEnvironment, intAnswer, ds.Tables[0].Rows[0]["hostname"].ToString(), ds.Tables[0].Rows[0]["virtualdir"].ToString(), ds.Tables[0].Rows[0]["gzippath"].ToString(), ds.Tables[0].Rows[0]["image"].ToString(), ds.Tables[0].Rows[0]["name"].ToString(), strSerial, dsSteps.Tables[0].Rows.Count, Int32.Parse(ds.Tables[0].Rows[0]["ram"].ToString()), oUser.GetName(Int32.Parse(oForecast.GetAnswer(intAnswer, "appcontact"))) + " - " + oUser.GetFullName(Int32.Parse(oForecast.GetAnswer(intAnswer, "appcontact"))));
                    oWorkstation.UpdateVirtualRemote(_id, intRemote);
                    oWorkstation.UpdateVirtualAsset(_id, intAsset);
                    oOnDemand.UpdateStepDoneWorkstation(_id, 1, "Assigned to host " + ds.Tables[0].Rows[0]["hostname"].ToString(), 0, false, true);
                    oWorkstation.NextVirtualStep(_id);
                    oRemote.NextRemoteVirtual(intRemote);
                }
                else
                {
                    oOnDemand.UpdateStepDoneWorkstation(_id, 1, "There was a problem assigning a host!!", 1, false, true);
                }
            }
        }
        #endregion


        // Remote
        #region Remote Virtual
        public DataSet GetRemoteVirtuals(string _hostname)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@hostname", _hostname);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVirtualWorkstations", arParams);
        }
        public void NextRemoteVirtual(int _id)
        {
            int intStep = Int32.Parse(GetRemoteVirtual(_id, "step"));
            int intSteps = Int32.Parse(GetRemoteVirtual(_id, "steps"));
            if (intStep < intSteps)
                UpdateRemoteVirtual(_id, intStep + 1);
        }
        public int AddRemoteVirtual(int _environment, int _answerid, string _hostname, string _virtualdir, string _gzippath, string _image, string _name, string _serial, int _steps, int _ram, string _manager)
        {
            arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@environment", _environment);
            arParams[1] = new SqlParameter("@answerid", _answerid);
            arParams[2] = new SqlParameter("@hostname", _hostname);
            arParams[3] = new SqlParameter("@virtualdir", _virtualdir);
            arParams[4] = new SqlParameter("@gzippath", _gzippath);
            arParams[5] = new SqlParameter("@image", _image);
            arParams[6] = new SqlParameter("@name", _name);
            arParams[7] = new SqlParameter("@serial", _serial);
            arParams[8] = new SqlParameter("@steps", _steps);
            arParams[9] = new SqlParameter("@ram", _ram);
            arParams[10] = new SqlParameter("@manager", _manager);
            arParams[11] = new SqlParameter("@id", SqlDbType.Int);
            arParams[11].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVirtualWorkstation", arParams);
            return Int32.Parse(arParams[11].Value.ToString());
        }
        public void UpdateRemoteVirtualCompleted(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVirtualWorkstationCompleted", arParams);
        }
        public void UpdateRemoteVirtual(int _id, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVirtualWorkstation", arParams);
        }
        public DataSet GetRemoteVirtual(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVirtualWorkstation", arParams);
        }
        public string GetRemoteVirtual(int _id, string _column)
        {
            DataSet ds = GetRemoteVirtual(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        #endregion


        // Decommission
        #region Decommission
        public DataSet GetRemoteVirtualDecoms(string _hostname)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@hostname", _hostname);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVirtualWorkstationDecoms", arParams);
        }
        public int AddRemoteVirtualDecom(int _environment, string _hostname, string _virtualdir, string _name)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@environment", _environment);
            arParams[1] = new SqlParameter("@hostname", _hostname);
            arParams[2] = new SqlParameter("@virtualdir", _virtualdir);
            arParams[3] = new SqlParameter("@name", _name);
            arParams[4] = new SqlParameter("@id", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVirtualWorkstationDecom", arParams);
            return Int32.Parse(arParams[4].Value.ToString());
        }
        public void UpdateRemoteVirtualDecomCompleted(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVirtualWorkstationDecomCompleted", arParams);
        }
        public void AddVirtualDecommission(int _requestid, int _itemid, int _number, int _nameid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@nameid", _nameid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWorkstationVirtualDecom", arParams);
        }
        public DataSet GetVirtualDecommissions(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualDecoms", arParams);
        }

        #endregion


        // NAMES
        #region Names
        public int AddName(string _environment, string _code, string _identifier, int _classid, int _environmentid, int _first_number, bool _vdi)
        {
            Functions oFunction = new Functions(user, dsn, 4);
            DataSet ds = GetNames(_environment, _code, _identifier, 0);
            string[] aLetters = new string[33] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            int intID = 0;
            // Loop through all the variations of names environment + code + identifier. Check if it exists. If not, add it and set intID to break loop and return.
            for (int int1 = (_vdi ? _first_number : 0); int1 < 33 && intID == 0; int1++)
            {
                for (int int2 = 0; int2 < 33 && intID == 0; int2++)
                {
                    for (int int3 = 0; int3 < 33 && intID == 0; int3++)
                    {
                        for (int int4 = 0; int4 < 33 && intID == 0; int4++)
                        {
                            for (int int5 = 0; int5 < 33 && intID == 0; int5++)
                            {
                                if (_vdi == false)
                                {
                                    for (int int6 = 0; int6 < 33 && intID == 0; int6++)
                                    {
                                        string strPrefix1 = aLetters[int1];
                                        string strPrefix2 = aLetters[int2];
                                        string strPrefix3 = aLetters[int3];
                                        string strPrefix4 = aLetters[int4];
                                        string strPrefix5 = aLetters[int5];
                                        string strPrefix6 = aLetters[int6];
                                        DataTable dt = ds.Tables[0];
                                        DataRow[] dr = dt.Select("prefix1 = '" + strPrefix1 + "' AND prefix2 = '" + strPrefix2 + "' AND prefix3 = '" + strPrefix3 + "' AND prefix4 = '" + strPrefix4 + "' AND prefix5 = '" + strPrefix5 + "' AND prefix6 = '" + strPrefix6 + "'");
                                        if (dr.Length == 0)
                                        {
                                            if (oFunction.Ping(_environment + _code + _identifier + strPrefix1 + strPrefix2 + strPrefix3 + strPrefix4 + strPrefix5 + strPrefix6, _classid, _environmentid) == false)
                                                intID = AddName(_environment, _code, _identifier, strPrefix1, strPrefix2, strPrefix3, strPrefix4, strPrefix5, strPrefix6, 0);
                                            else
                                                AddName(_environment, _code, _identifier, strPrefix1, strPrefix2, strPrefix3, strPrefix4, strPrefix5, strPrefix6, 0);
                                        }
                                    }
                                }
                                else
                                {
                                    string strPrefix1 = aLetters[int1];
                                    string strPrefix2 = aLetters[int2];
                                    string strPrefix3 = aLetters[int3];
                                    string strPrefix4 = aLetters[int4];
                                    string strPrefix5 = aLetters[int5];
                                    DataTable dt = ds.Tables[0];
                                    DataRow[] dr = dt.Select("prefix1 = '" + strPrefix1 + "' AND prefix2 = '" + strPrefix2 + "' AND prefix3 = '" + strPrefix3 + "' AND prefix4 = '" + strPrefix4 + "' AND prefix5 = '" + strPrefix5 + "'");
                                    if (dr.Length == 0)
                                    {
                                        if (oFunction.Ping(_environment + _code + _identifier + strPrefix1 + strPrefix2 + strPrefix3 + strPrefix4 + strPrefix5, _classid, _environmentid) == false)
                                            intID = AddName(_environment, _code, _identifier, strPrefix1, strPrefix2, strPrefix3, strPrefix4, strPrefix5, "", 0);
                                        else
                                            AddName(_environment, _code, _identifier, strPrefix1, strPrefix2, strPrefix3, strPrefix4, strPrefix5, "", 0);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (intID == 0)
            {
                // An unused name was not found. Check for names that have been used but are now available for reuse (available == 1).
                ds = GetNames(_environment, _code, _identifier, 1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intID = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                    UpdateName(intID, 0);
                }
            }
            return intID;
        }
        public int AddName(string _environment, string _code, string _identifier, string _prefix1, string _prefix2, string _prefix3, string _prefix4, string _prefix5, string _prefix6, int _available)
        {
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@environment", _environment);
            arParams[1] = new SqlParameter("@code", _code);
            arParams[2] = new SqlParameter("@identifier", _identifier);
            arParams[3] = new SqlParameter("@prefix1", _prefix1);
            arParams[4] = new SqlParameter("@prefix2", _prefix2);
            arParams[5] = new SqlParameter("@prefix3", _prefix3);
            arParams[6] = new SqlParameter("@prefix4", _prefix4);
            arParams[7] = new SqlParameter("@prefix5", _prefix5);
            arParams[8] = new SqlParameter("@prefix6", _prefix6);
            arParams[9] = new SqlParameter("@available", _available);
            arParams[10] = new SqlParameter("@id", SqlDbType.Int);
            arParams[10].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWorkstationName", arParams);
            return Int32.Parse(arParams[10].Value.ToString());
        }
        public void UpdateName(int _id, int _available)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@available", _available);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationNameAvailable", arParams);
        }
        public DataSet GetNames(string _environment, string _code, string _identifier, int _available)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@environment", _environment);
            arParams[1] = new SqlParameter("@code", _code);
            arParams[2] = new SqlParameter("@identifier", _identifier);
            arParams[3] = new SqlParameter("@available", _available);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationNames", arParams);
        }
        public DataSet GetNameId(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationNameId", arParams);
        }
        public DataSet GetName(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationName", arParams);
        }
        public string GetName(int _id)
        {
            DataSet ds = GetNameId(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string _environment = ds.Tables[0].Rows[0]["environment"].ToString();
                string _code = ds.Tables[0].Rows[0]["code"].ToString();
                string _identifier = ds.Tables[0].Rows[0]["identifier"].ToString();
                string _prefix1 = ds.Tables[0].Rows[0]["prefix1"].ToString();
                string _prefix2 = ds.Tables[0].Rows[0]["prefix2"].ToString();
                string _prefix3 = ds.Tables[0].Rows[0]["prefix3"].ToString();
                string _prefix4 = ds.Tables[0].Rows[0]["prefix4"].ToString();
                string _prefix5 = ds.Tables[0].Rows[0]["prefix5"].ToString();
                string _prefix6 = ds.Tables[0].Rows[0]["prefix6"].ToString();
                return _environment + _code + _identifier + _prefix1 + _prefix2 + _prefix3 + _prefix4 + _prefix5 + _prefix6;
            }
            else
                return "Unavailable";
        }
        #endregion


        // Accounts
        #region Accounts
        public void AddAccountFix(int _assetid, int _workstationid, int _userid, int _admin, int _remote)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            arParams[1] = new SqlParameter("@workstationid", _workstationid);
            arParams[2] = new SqlParameter("@userid", _userid);
            arParams[3] = new SqlParameter("@admin", _admin);
            arParams[4] = new SqlParameter("@remote", _remote);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWorkstationVirtualAccount", arParams);
        }
        public void UpdateAccount(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualAccount", arParams);
        }
        public void DeleteAccount(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteWorkstationVirtualAccount", arParams);
        }
        public void DeleteAccounts(int _workstationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteWorkstationVirtualAccounts", arParams);
        }
        public DataSet GetAccountsVirtual(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualAccountsVirtual", arParams);
        }
        public DataSet GetAccountsVMware(int _workstationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualAccountsVMware", arParams);
        }
        public DataSet GetAccount(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualAccount", arParams);
        }
        public void AddRemoteAccount(string _name, string _xid, int _admin, int _remote)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@xid", _xid);
            arParams[2] = new SqlParameter("@admin", _admin);
            arParams[3] = new SqlParameter("@remote", _remote);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVirtualWorkstationAccount", arParams);
        }
        public DataSet GetRemoteAccounts(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVirtualWorkstationAccounts", arParams);
        }

        public void AddRemoteNotify(string _name, string _xid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@xid", _xid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVirtualWorkstationNotify", arParams);
        }
        public DataSet GetRemoteNotifys(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVirtualWorkstationNotifys", arParams);
        }
        #endregion


        // Components
        #region Components
        public DataSet GetComponents(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationComponents", arParams);
        }
        public DataSet GetComponent(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationComponent", arParams);
        }
        public string GetComponent(int _id, string _column)
        {
            DataSet ds = GetComponent(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddComponent(string _name, string _zeus_build_type, string _ad_move_location, int _sms_install, string _script, string _workstation_group, string _user_group, string _notifications, int _display, int _enabled)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@zeus_build_type", _zeus_build_type);
            arParams[2] = new SqlParameter("@ad_move_location", _ad_move_location);
            arParams[3] = new SqlParameter("@sms_install", _sms_install);
            arParams[4] = new SqlParameter("@script", _script);
            arParams[5] = new SqlParameter("@workstation_group", _workstation_group);
            arParams[6] = new SqlParameter("@user_group", _user_group);
            arParams[7] = new SqlParameter("@notifications", _notifications);
            arParams[8] = new SqlParameter("@display", _display);
            arParams[9] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWorkstationComponent", arParams);
        }
        public void UpdateComponent(int _id, string _name, string _zeus_build_type, string _ad_move_location, int _sms_install, string _script, string _workstation_group, string _user_group, string _notifications, int _enabled)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@zeus_build_type", _zeus_build_type);
            arParams[3] = new SqlParameter("@ad_move_location", _ad_move_location);
            arParams[4] = new SqlParameter("@sms_install", _sms_install);
            arParams[5] = new SqlParameter("@script", _script);
            arParams[6] = new SqlParameter("@workstation_group", _workstation_group);
            arParams[7] = new SqlParameter("@user_group", _user_group);
            arParams[8] = new SqlParameter("@notifications", _notifications);
            arParams[9] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationComponent", arParams);
        }
        public void UpdateComponentOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationComponentOrder", arParams);
        }
        public void EnableComponent(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationComponentEnabled", arParams);
        }
        public void DeleteComponent(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteWorkstationComponent", arParams);
        }

        public void AddComponentPermission(int _componentid, int _osid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@componentid", _componentid);
            arParams[1] = new SqlParameter("@osid", _osid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWorkstationComponentPermission", arParams);
        }
        public void DeleteComponentPermission(int _componentid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@componentid", _componentid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteWorkstationComponentPermission", arParams);
        }
        public DataSet GetComponentPermissions(int _componentid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@componentid", _componentid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationComponentPermissions", arParams);
        }
        public DataSet GetComponentPermissionsOS(int _osid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@osid", _osid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationComponentPermissionsOS", arParams);
        }

        public void AddComponents(int _workstationid, int _componentid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@componentid", _componentid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWorkstationsComponent", arParams);
        }
        public void UpdateComponents(int _workstationid, int _componentid, int _done)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@componentid", _componentid);
            arParams[2] = new SqlParameter("@done", _done);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationsComponent", arParams);
        }
        public void DeleteComponents(int _workstationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteWorkstationsComponents", arParams);
        }
        public DataSet GetComponentsSelected(int _workstationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationsComponents", arParams);
        }
        public DataSet GetComponents()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationsComponentsAll");
        }
        public DataSet GetComponentsActive(int _workstationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationsComponentsActive", arParams);
        }

        public DataSet GetComponentScripts(int _componentid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@componentid", _componentid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationsComponentScripts", arParams);
        }
        public DataSet GetComponentScript(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationsComponentScript", arParams);
        }
        public string GetComponentScript(int _id, string _column)
        {
            DataSet ds = GetComponentScript(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddComponentScript(int _componentid, string _name, string _script, int _display, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@componentid", _componentid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@script", _script);
            arParams[3] = new SqlParameter("@display", _display);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWorkstationsComponentScript", arParams);
        }
        public void UpdateComponentScript(int _id, int _componentid, string _name, string _script, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@componentid", _componentid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@script", _script);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationsComponentScript", arParams);
        }
        public void UpdateComponentScriptOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationsComponentScriptOrder", arParams);
        }
        public void EnableComponentScript(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationsComponentScriptEnabled", arParams);
        }
        public void DeleteComponentScript(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteWorkstationsComponentScript", arParams);
        }

        #endregion


        // Pools
        #region POOLS
        public DataSet GetPools(int _userid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationPoolsUser", arParams);
        }
        public DataSet GetPools(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationPools", arParams);
        }
        public DataSet GetPool(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationPool", arParams);
        }
        public string GetPool(int _id, string _column)
        {
            DataSet ds = GetPool(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public int AddPool(string _name, string _description, int _contact1, int _contact2, int _modifiedby, int _enabled)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@description", _description);
            arParams[2] = new SqlParameter("@contact1", _contact1);
            arParams[3] = new SqlParameter("@contact2", _contact2);
            arParams[4] = new SqlParameter("@modifiedby", _modifiedby);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            arParams[6] = new SqlParameter("@id", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWorkstationPool", arParams);
            return Int32.Parse(arParams[6].Value.ToString());
        }
        public void UpdatePool(int _id, string _name, string _description, int _contact1, int _contact2, int _modifiedby, int _enabled)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@description", _description);
            arParams[3] = new SqlParameter("@contact1", _contact1);
            arParams[4] = new SqlParameter("@contact2", _contact2);
            arParams[5] = new SqlParameter("@modifiedby", _modifiedby);
            arParams[6] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationPool", arParams);
        }
        public void UpdatePoolEnabled(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationPoolEnabled", arParams);
        }
        public void DeletePool(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteWorkstationPool", arParams);
        }

        public void AddPoolWorkstation(int _poolid, int _workstationid, int _display, int _modifiedby)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@poolid", _poolid);
            arParams[1] = new SqlParameter("@workstationid", _workstationid);
            arParams[2] = new SqlParameter("@display", _display);
            arParams[3] = new SqlParameter("@modifiedby", _modifiedby);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWorkstationPoolWorkstation", arParams);
        }
        public void DeletePoolWorkstations(int _poolid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@poolid", _poolid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteWorkstationPoolWorkstations", arParams);
        }
        public DataSet GetPoolWorkstations(int _poolid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@poolid", _poolid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationPoolWorkstations", arParams);
        }
        public DataSet GetPoolWorkstations(int _userid, int _poolid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@poolid", _poolid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationPoolWorkstationsUser", arParams);
        }
        public DataSet GetPoolWorkstations(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationPoolAvailable", arParams);
        }
        public void UpdatePoolWorkstationOut(string _name, string _xid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@xid", _xid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationPoolAvailableOut", arParams);
        }
        public void UpdatePoolWorkstationIn(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationPoolAvailableIn", arParams);
        }
        public DataSet GetPoolWorkstationsStatus(string _poolname, string _workstationname)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@poolname", _poolname);
            arParams[1] = new SqlParameter("@workstationname", _workstationname);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationPoolStatus", arParams);
        }
        public DataSet GetPoolWorkstationsStatus(string _poolname)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@poolname", _poolname);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationPoolStatusAll", arParams);
        }

        public DataSet GetWorkstationVirtualRemoteStatus(int _intRemote)
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_virtual_workstations WHERE id = " + _intRemote.ToString());
        }
        #endregion


        // Rebuilds
        #region REBUILDS
        public void AddVirtualRebuild(int _workstationid, int _requestid, int _serviceid, int _number, string _scheduled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@requestid", _requestid);
            arParams[2] = new SqlParameter("@serviceid", _serviceid);
            arParams[3] = new SqlParameter("@number", _number);
            arParams[4] = new SqlParameter("@scheduled", (_scheduled == "" ? SqlDateTime.Null : DateTime.Parse(_scheduled)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWorkstationVirtualRebuild", arParams);
        }
        public DataSet GetVirtualRebuild(int _workstationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualRebuilds", arParams);
        }
        public DataSet GetVirtualRebuild(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWorkstationVirtualRebuild", arParams);
        }
        public void UpdateVirtualRebuild(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualRebuild", arParams);
        }
        public void UpdateVirtualRebuildCancel(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualRebuildCancel", arParams);
        }
        public void UpdateVirtualRebuildCancel(int _workstationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualRebuildsCancel", arParams);
        }
        public void UpdateVirtualRebuildPower(int _workstationid, string _turnedoff, string _rebuild)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@turnedoff", (_turnedoff == "" ? SqlDateTime.Null : DateTime.Parse(_turnedoff)));
            arParams[2] = new SqlParameter("@rebuild", (_rebuild == "" ? SqlDateTime.Null : DateTime.Parse(_rebuild)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualRebuildPower", arParams);
        }
        public void UpdateVirtualRebuildStarted(int _workstationid, string _started)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@started", (_started == "" ? SqlDateTime.Null : DateTime.Parse(_started)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualRebuildStarted", arParams);
        }
        public void UpdateVirtualRebuildCompleted(int _workstationid, string _completed)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@workstationid", _workstationid);
            arParams[1] = new SqlParameter("@completed", (_completed == "" ? SqlDateTime.Null : DateTime.Parse(_completed)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWorkstationVirtualRebuildCompleted", arParams);
        }
        #endregion


        public void ExecuteVMware(int _requestid)
        {
            DataSet dsWorkstation = GetVirtualRequests(_requestid);
            if (dsWorkstation.Tables[0].Rows.Count > 0)
            {
                // There is an automated workstation request waiting.
                Forecast oForecast = new Forecast(user, dsn);
                ServiceRequests oServiceRequest = new ServiceRequests(user, dsn);
                OnDemand oOnDemand = new OnDemand(user, dsn);
                ModelsProperties oModelsProperties = new ModelsProperties(user, dsn);
                int intWorkstation = Int32.Parse(dsWorkstation.Tables[0].Rows[0]["id"].ToString());
                int intAnswer = Int32.Parse(dsWorkstation.Tables[0].Rows[0]["answerid"].ToString());
                int intQuantity = Int32.Parse(oForecast.GetAnswer(intAnswer, "quantity"));
                DataSet dsAccounts = GetAccountsVMware(intWorkstation);
                // Create more workstations (if applicable)
                for (int ii = 2; ii <= intQuantity; ii++)
                {
                    int intCopy = AddVirtualCopy(intWorkstation, ii);
                    foreach (DataRow drAccount in dsAccounts.Tables[0].Rows)
                        AddAccountFix(0, intCopy, Int32.Parse(drAccount["userid"].ToString()), Int32.Parse(drAccount["admin"].ToString()), Int32.Parse(drAccount["remote"].ToString()));
                }
                int intModel = Int32.Parse(dsWorkstation.Tables[0].Rows[0]["modelid"].ToString());
                int intType = oModelsProperties.GetType(intModel);
                oForecast.UpdateAnswer(intAnswer, _requestid);
                oForecast.DeleteReset(intAnswer);
                oServiceRequest.Add(_requestid, 1, 1);
                oForecast.UpdateAnswerExecuted(intAnswer, DateTime.Now.ToString(), user);
                // Update OnDemand Steps
                DataSet dsWizardSteps = oOnDemand.GetWizardSteps(intType, 1);
                foreach (DataRow drStep in dsWizardSteps.Tables[0].Rows)
                    oOnDemand.Next(intAnswer, Int32.Parse(drStep["id"].ToString()));
                // Start Build
                StartVirtual(_requestid);
            }
        }
    }
}
