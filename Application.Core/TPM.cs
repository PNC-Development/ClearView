using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;

namespace NCC.ClearView.Application.Core
{
    public class TPM
    {
        private string dsn = "";
        private int user = 0;
        private SqlParameter[] arParams;
        private Users oUser;
        private Functions oFunction;
        private Variables oVariable;
        private Pages oPage;
        private string strEMailIdsBCC = "";
        public TPM(int _user, string _dsn, int _environment)
        {
            user = _user;
            dsn = _dsn;
            oUser = new Users(user, dsn);
            oFunction = new Functions(user, dsn, _environment);
            oVariable = new Variables(_environment);
            oPage = new Pages(user, dsn);

            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROJECT");
        }
        
        // Vijay Code 
        public int AddCSRC(int _requestid, int _itemid, int _number, string _name, int _d, int _p, int _e, int _c,string _ds, string _de, string _di, string _dex, string _dh, string _ps, string _pe, string _pi, string _pex, string _ph, string _es, string _ee, string _ei, string _eex, string _eh, string _cs, string _ce, string _ci, string _cex, string _ch,string _path)
        {
            arParams = new SqlParameter[30];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@name", _name);
            arParams[4] = new SqlParameter("@d", _d);
            arParams[5] = new SqlParameter("@p", _p);
            arParams[6] = new SqlParameter("@e", _e);
            arParams[7] = new SqlParameter("@c", _c);             
            arParams[8] = new SqlParameter("@ds", (_ds == "" ? SqlDateTime.Null : DateTime.Parse(_ds)));
            arParams[9] = new SqlParameter("@de", (_de == "" ? SqlDateTime.Null : DateTime.Parse(_de)));
            arParams[10] = new SqlParameter("@di", (_di == "" ? 0.00 : double.Parse(_di)));
            arParams[11] = new SqlParameter("@dex", (_dex == "" ? 0.00 : double.Parse(_dex)));
            arParams[12] = new SqlParameter("@dh", (_dh == "" ? 0.00 : double.Parse(_dh)));
            arParams[13] = new SqlParameter("@ps", (_ps == "" ? SqlDateTime.Null : DateTime.Parse(_ps)));
            arParams[14] = new SqlParameter("@pe", (_pe == "" ? SqlDateTime.Null : DateTime.Parse(_pe)));
            arParams[15] = new SqlParameter("@pi", (_pi == "" ? 0.00 : double.Parse(_pi)));
            arParams[16] = new SqlParameter("@pex", (_pex == "" ? 0.00 : double.Parse(_pex)));
            arParams[17] = new SqlParameter("@ph", (_ph == "" ? 0.00 : double.Parse(_ph)));
            arParams[18] = new SqlParameter("@es", (_es == "" ? SqlDateTime.Null : DateTime.Parse(_es)));
            arParams[19] = new SqlParameter("@ee", (_ee == "" ? SqlDateTime.Null : DateTime.Parse(_ee)));
            arParams[20] = new SqlParameter("@ei", (_ei == "" ? 0.00 : double.Parse(_ei)));
            arParams[21] = new SqlParameter("@eex", (_eex == "" ? 0.00 : double.Parse(_eex)));
            arParams[22] = new SqlParameter("@eh", (_eh == "" ? 0.00 : double.Parse(_eh)));
            arParams[23] = new SqlParameter("@cs", (_cs == "" ? SqlDateTime.Null : DateTime.Parse(_cs)));
            arParams[24] = new SqlParameter("@ce", (_ce == "" ? SqlDateTime.Null : DateTime.Parse(_ce)));
            arParams[25] = new SqlParameter("@ci", (_ci == "" ? 0.00 : double.Parse(_ci)));
            arParams[26] = new SqlParameter("@cex", (_cex == "" ? 0.00 : double.Parse(_cex)));
            arParams[27] = new SqlParameter("@ch", (_ch == "" ? 0.00 : double.Parse(_ch)));
            arParams[28] = new SqlParameter("@path", _path);
            arParams[29] = new SqlParameter("@id", SqlDbType.Int);
            arParams[29].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addCSRC", arParams);
            return Int32.Parse(arParams[29].Value.ToString());
        }
        public void UpdateCSRC(int _id, int _step, int _status)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@status", _status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCSRCStepStatus", arParams);
        }
        public void UpdateCSRC(int _id, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCSRCStep", arParams);
        }

        public void UpdateCSRCCC(int _id, int _cc)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@cc", _cc);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCSRCCC", arParams);
        }

        public void UpdateCSRCPath(int _id, string _path)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@path", _path);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCSRCPath", arParams);
        }

        public void UpdateCSRC(int _id, int _d, int _p, int _e, int _c, string _ds, string _de, string _di, string _dex, string _dh, string _ps, string _pe, string _pi, string _pex, string _ph, string _es, string _ee, string _ei, string _eex, string _eh, string _cs, string _ce, string _ci, string _cex, string _ch)
        {
            arParams = new SqlParameter[25];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@d", _d);
            arParams[2] = new SqlParameter("@p", _p);
            arParams[3] = new SqlParameter("@e", _e);
            arParams[4] = new SqlParameter("@c", _c);
            arParams[5] = new SqlParameter("@ds", (_ds == "" ? SqlDateTime.Null : DateTime.Parse(_ds)));
            arParams[6] = new SqlParameter("@de", (_de == "" ? SqlDateTime.Null : DateTime.Parse(_de)));
            arParams[7] = new SqlParameter("@di", (_di == "" ? 0.00 : double.Parse(_di)));
            arParams[8] = new SqlParameter("@dex", (_dex == "" ? 0.00 : double.Parse(_dex)));
            arParams[9] = new SqlParameter("@dh", (_dh == "" ? 0.00 : double.Parse(_dh)));
            arParams[10] = new SqlParameter("@ps", (_ps == "" ? SqlDateTime.Null : DateTime.Parse(_ps)));
            arParams[11] = new SqlParameter("@pe", (_pe == "" ? SqlDateTime.Null : DateTime.Parse(_pe)));
            arParams[12] = new SqlParameter("@pi", (_pi == "" ? 0.00 : double.Parse(_pi)));
            arParams[13] = new SqlParameter("@pex", (_pex == "" ? 0.00 : double.Parse(_pex)));
            arParams[14] = new SqlParameter("@ph", (_ph == "" ? 0.00 : double.Parse(_ph)));
            arParams[15] = new SqlParameter("@es", (_es == "" ? SqlDateTime.Null : DateTime.Parse(_es)));
            arParams[16] = new SqlParameter("@ee", (_ee == "" ? SqlDateTime.Null : DateTime.Parse(_ee)));
            arParams[17] = new SqlParameter("@ei", (_ei == "" ? 0.00 : double.Parse(_ei)));
            arParams[18] = new SqlParameter("@eex", (_eex == "" ? 0.00 : double.Parse(_eex)));
            arParams[19] = new SqlParameter("@eh", (_eh == "" ? 0.00 : double.Parse(_eh)));
            arParams[20] = new SqlParameter("@cs", (_cs == "" ? SqlDateTime.Null : DateTime.Parse(_cs)));
            arParams[21] = new SqlParameter("@ce", (_ce == "" ? SqlDateTime.Null : DateTime.Parse(_ce)));
            arParams[22] = new SqlParameter("@ci", (_ci == "" ? 0.00 : double.Parse(_ci)));
            arParams[23] = new SqlParameter("@cex", (_cex == "" ? 0.00 : double.Parse(_cex)));
            arParams[24] = new SqlParameter("@ch", (_ch == "" ? 0.00 : double.Parse(_ch)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCSRC", arParams);

        }
        public void AddCSRCDetail(int _parent, int _step, int _userid, int _status)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@userid", _userid);
            arParams[3] = new SqlParameter("@status", _status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addCSRCDetail", arParams);
        }
        public void UpdateCSRCDetail(int _id, int _status, string _comments)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@status", _status);
            arParams[2] = new SqlParameter("@comments", _comments);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCSRCDetail", arParams);
        }
        public void ApproveCSRC(int _parent, int _next_step, int _status, int _approve_pageid, int _wm_pageid )
        {
            DataSet ds = GetCSRC(_parent);
            int intCC = Int32.Parse(ds.Tables[0].Rows[0]["cc"].ToString());
            string strCC = (intCC == 0 ? "" : oUser.GetName(intCC));
            string strNotify = "";
            if (_status != -1)
            {
                UpdateCSRC(_parent, _next_step);
                UpdateCSRCDetail(_parent, _next_step);
                // Approved
                if (_next_step < 5)
                {
                    // Move to next person
                    ds = GetCSRCDetail(_parent, _next_step);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int intUser = Int32.Parse(dr["userid"].ToString());
                            //int intUser = 39331;
                            string strDefault = oUser.GetApplicationUrl(intUser, _approve_pageid);
                            if (strDefault == "")
                                oFunction.SendEmail("ClearView CSRC Approval", oUser.GetName(intUser), strCC, strEMailIdsBCC, "ClearView CSRC Approval", "<p><b>A CSRC has been submitted and requires your approval.</b></p>", true, false);
                            else
                                oFunction.SendEmail("ClearView CSRC Approval", oUser.GetName(intUser), strCC, strEMailIdsBCC, "ClearView CSRC Approval", "<p><b>A CSRC has been submitted and requires your approval.</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_approve_pageid) + "?id=" + _parent.ToString() + "\" target=\"_blank\">Click here to view this CSRC.</a></p>", true, false);
                        }
                    }
                    else
                        ApproveCSRC(_parent, _next_step + 1, _status, _approve_pageid, _wm_pageid, strEMailIdsBCC);
                }
                else
                {
                    // Done - notify requestor
                    strNotify = "APPROVED";
                }
            }
            else
            {
                // Rejected - notify requestor
                strNotify = "REJECTED";
            }
            if (strNotify != "")
            {
                ds = GetCSRC(_parent);
                UpdateCSRC(_parent, 0, _status);
                int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                DateTime datModified = DateTime.Parse(ds.Tables[0].Rows[0]["modified"].ToString());
                ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
                DataSet dsResource = oResourceRequest.Get(intRequest, intItem, intNumber);
                foreach (DataRow drResource in dsResource.Tables[0].Rows)
                {
                    int intRR = Int32.Parse(drResource["id"].ToString());
                    int intUser = Int32.Parse(drResource["userid"].ToString());
                    string strDefault = oUser.GetApplicationUrl(intUser, _wm_pageid);
                    if (strDefault == "")
                        oFunction.SendEmail("ClearView CSRC Approval", oUser.GetName(intUser), strCC, strEMailIdsBCC, "ClearView CSRC Approval", "<p><b>The CSRC Form that was submitted on " + datModified.ToLongDateString() + " has been " + strNotify + "</b></p>", true, false);
                    else
                        oFunction.SendEmail("ClearView CSRC Approval", oUser.GetName(intUser), strCC, strEMailIdsBCC, "ClearView CSRC Approval", "<p><b>The CSRC Form that was submitted on " + datModified.ToLongDateString() + " has been " + strNotify + "</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_wm_pageid) + "?rrid=" + intRR.ToString() + "\" target=\"_blank\">Click here to view this CSRC.</a></p>", true, false);
                }
            }
        }
        // Vijay Code

        public void ApproveCSRC(int _parent, int _next_step, int _status, int _approve_pageid, int _wm_pageid, string _path)
        {
            DataSet ds = GetCSRC(_parent);
            int intCC = Int32.Parse(ds.Tables[0].Rows[0]["cc"].ToString());
            string strCC = (intCC == 0 ? "" : oUser.GetName(intCC));
            string strNotify = "";
            if (_status != -1)
            {
                UpdateCSRC(_parent, _next_step);
                UpdateCSRCDetail(_parent, _next_step);
                // Approved
                if (_next_step < 5)
                {
                    // Move to next person
                    ds = GetCSRCDetail(_parent, _next_step);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int intUser = Int32.Parse(dr["userid"].ToString());
                            //int intUser = 39331;
                            string strDefault = oUser.GetApplicationUrl(intUser, _approve_pageid);
                            if (strDefault == "")
                                oFunction.SendEmail("ClearView CSRC Approval", oUser.GetName(intUser), strCC, strEMailIdsBCC, "ClearView CSRC Approval", "<p><b>A CSRC has been submitted and requires your approval.</b></p>", true, false, _path);
                            else
                                oFunction.SendEmail("ClearView CSRC Approval", oUser.GetName(intUser), strCC, strEMailIdsBCC, "ClearView CSRC Approval", "<p><b>A CSRC has been submitted and requires your approval.</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_approve_pageid) + "?id=" + _parent.ToString() + "\" target=\"_blank\">Click here to view this CSRC.</a></p>", true, false, _path);
                        }
                    }
                    else
                        ApproveCSRC(_parent, _next_step + 1, _status, _approve_pageid, _wm_pageid, _path);
                }
                else
                {
                    // Done - notify requestor
                    strNotify = "APPROVED";
                }
            }
            else
            {
                // Rejected - notify requestor
                strNotify = "REJECTED";
            }
            if (strNotify != "")
            {
                ds = GetCSRC(_parent);
                UpdateCSRC(_parent, 0, _status);
                int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                DateTime datModified = DateTime.Parse(ds.Tables[0].Rows[0]["modified"].ToString());
                ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
                DataSet dsResource = oResourceRequest.Get(intRequest, intItem, intNumber);
                foreach (DataRow drResource in dsResource.Tables[0].Rows)
                {
                    int intRR = Int32.Parse(drResource["id"].ToString());
                    int intPid = Int32.Parse(drResource["projectid"].ToString());
                    int intUser = Int32.Parse(drResource["userid"].ToString());
                    string strDefault = oUser.GetApplicationUrl(intUser, _wm_pageid);
                    if (strDefault == "")
                        oFunction.SendEmail("ClearView CSRC Approval", oUser.GetName(intUser), strCC, strEMailIdsBCC, "ClearView CSRC Approval", "<p><b>The CSRC Form that was submitted on " + datModified.ToLongDateString() + " has been " + strNotify + "</b></p>", true, false);
                    else
                        oFunction.SendEmail("ClearView CSRC Approval", oUser.GetName(intUser), strCC, strEMailIdsBCC, "ClearView CSRC Approval", "<p><b>The CSRC Form that was submitted on " + datModified.ToLongDateString() + " has been " + strNotify + "</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_wm_pageid) + "?pid=" + intPid.ToString() + "&div=I\" target=\"_blank\">Click here to view this CSRC.</a></p>", true, false);
                }
            }
        }

        public void UpdateCSRCDetail(int _parent, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCSRCDetailStep", arParams);
        }
        public DataSet GetCSRCDetail(int _parent, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCSRCDetailStep", arParams);
        }
        public DataSet GetCSRCDetail(int _parent)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@parent", _parent);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCSRCDetail", arParams);
        }
        public DataSet GetCSRCs(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCSRCs", arParams);
        }
        public DataSet GetCSRC(int _id, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCSRCUser", arParams);
        }
        public DataSet GetCSRC(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCSRC", arParams);
        }

        public string GetCSRCPath(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getCSRCPath", arParams).ToString();
        } 

        // Vijay Code
        public int AddPCR(int _requestid, int _itemid, int _number,string _name, int _scope, int _s, string _sds, string _sde, string _sps, string _spe, string _ses, string _see, string _scs, string _sce, int _f, string _fd, string _fp, string _fe, string _fc, string _reasons,string _path,string _scope_comments,string _sch_comments,string _fin_comments)
        {
            arParams = new SqlParameter[25];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@name", _name);
            arParams[4] = new SqlParameter("@scope", _scope);
            arParams[5] = new SqlParameter("@s", _s);
            arParams[6] = new SqlParameter("@sds", (_sds == "" ? SqlDateTime.Null : DateTime.Parse(_sds)));
            arParams[7] = new SqlParameter("@sde", (_sde == "" ? SqlDateTime.Null : DateTime.Parse(_sde)));
            arParams[8] = new SqlParameter("@sps", (_sps == "" ? SqlDateTime.Null : DateTime.Parse(_sps)));
            arParams[9] = new SqlParameter("@spe", (_spe == "" ? SqlDateTime.Null : DateTime.Parse(_spe)));
            arParams[10] = new SqlParameter("@ses", (_ses == "" ? SqlDateTime.Null : DateTime.Parse(_ses)));
            arParams[11] = new SqlParameter("@see", (_see == "" ? SqlDateTime.Null : DateTime.Parse(_see)));
            arParams[12] = new SqlParameter("@scs", (_scs == "" ? SqlDateTime.Null : DateTime.Parse(_scs)));
            arParams[13] = new SqlParameter("@sce", (_sce == "" ? SqlDateTime.Null : DateTime.Parse(_sce)));
            arParams[14] = new SqlParameter("@f", _f);
            arParams[15] = new SqlParameter("@fd", (_fd == "" ? 0.00 : double.Parse(_fd)));
            arParams[16] = new SqlParameter("@fp", (_fp == "" ? 0.00 : double.Parse(_fp)));
            arParams[17] = new SqlParameter("@fe", (_fe == "" ? 0.00 : double.Parse(_fe)));
            arParams[18] = new SqlParameter("@fc", (_fc == "" ? 0.00 : double.Parse(_fc)));
            arParams[19] = new SqlParameter("@reasons", _reasons);
            arParams[20] = new SqlParameter("@path", _path);
            arParams[21] = new SqlParameter("@scopecomments", _scope_comments);
            arParams[22] = new SqlParameter("@schcomments", _sch_comments);
            arParams[23] = new SqlParameter("@fincomments", _fin_comments);
            arParams[24] = new SqlParameter("@id", SqlDbType.Int);
            arParams[24].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addPCR", arParams);
            return Int32.Parse(arParams[24].Value.ToString());
        }


        public void UpdatePCR(int _id, int _step, int _status)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@status", _status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePCRStepStatus", arParams);
        }
        public void UpdatePCR(int _id, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePCRStep", arParams);
        }

        public void UpdatePCRCC(int _id, int _cc)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@cc", _cc);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePCRCC", arParams);
        }
        public void UpdatePCRPath(int _id, string  _path)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@path", _path);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePCRPath", arParams);
        }

        public void UpdatePCRStatus(int _id, int _status)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@status", _status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePCRStatus", arParams);
        }

        public void UpdatePCR(int _id, int _scope, int _s, string _sds, string _sde, string _sps, string _spe, string _ses, string _see, string _scs, string _sce, int _f, string _fd, string _fp, string _fe, string _fc, string _reasons, string _scope_comments, string _sch_comments, string _fin_comments)
        {
            arParams = new SqlParameter[20];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@scope", _scope);
            arParams[2] = new SqlParameter("@s", _s);
            arParams[3] = new SqlParameter("@sds", (_sds == "" ? SqlDateTime.Null : DateTime.Parse(_sds)));
            arParams[4] = new SqlParameter("@sde", (_sde == "" ? SqlDateTime.Null : DateTime.Parse(_sde)));
            arParams[5] = new SqlParameter("@sps", (_sps == "" ? SqlDateTime.Null : DateTime.Parse(_sps)));
            arParams[6] = new SqlParameter("@spe", (_spe == "" ? SqlDateTime.Null : DateTime.Parse(_spe)));
            arParams[7] = new SqlParameter("@ses", (_ses == "" ? SqlDateTime.Null : DateTime.Parse(_ses)));
            arParams[8] = new SqlParameter("@see", (_see == "" ? SqlDateTime.Null : DateTime.Parse(_see)));
            arParams[9] = new SqlParameter("@scs", (_scs == "" ? SqlDateTime.Null : DateTime.Parse(_scs)));
            arParams[10] = new SqlParameter("@sce", (_sce == "" ? SqlDateTime.Null : DateTime.Parse(_sce)));
            arParams[11] = new SqlParameter("@f", _f);
            arParams[12] = new SqlParameter("@fd", (_fd == "" ? 0.00 : double.Parse(_fd)));
            arParams[13] = new SqlParameter("@fp", (_fp == "" ? 0.00 : double.Parse(_fp)));
            arParams[14] = new SqlParameter("@fe", (_fe == "" ? 0.00 : double.Parse(_fe)));
            arParams[15] = new SqlParameter("@fc", (_fc == "" ? 0.00 : double.Parse(_fc)));
            arParams[16] = new SqlParameter("@reasons", _reasons);
            arParams[17] = new SqlParameter("@scopecomments", _scope_comments);
            arParams[18] = new SqlParameter("@schcomments", _sch_comments);
            arParams[19] = new SqlParameter("@fincomments", _fin_comments);            
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePCR", arParams);
        }
        public void AddPCRDetail(int _parent, int _step, int _userid, int _status)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@step", _step);
            arParams[2] = new SqlParameter("@userid", _userid);
            arParams[3] = new SqlParameter("@status", _status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addPCRDetail", arParams);
        }
        public void UpdatePCRDetail(int _id, int _status, string _comments)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@status", _status);
            arParams[2] = new SqlParameter("@comments", _comments);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePCRDetail", arParams);
        }
        public void ApprovePCR(int _parent, int _next_step, int _status, int _approve_pageid, int _wm_pageid)
        {
            DataSet ds = GetPCR(_parent);
            int intCC = Int32.Parse(ds.Tables[0].Rows[0]["cc"].ToString());
            string strCC = (intCC == 0 ? "" : oUser.GetName(intCC));
            string strNotify = "";
            if (_status != -1)
            {
                UpdatePCR(_parent, _next_step);
                UpdatePCRDetail(_parent, _next_step);
                // Approved
                if (_next_step < 5)
                {
                    // Move to next person
                    ds = GetPCRDetail(_parent, _next_step);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int intUser = Int32.Parse(dr["userid"].ToString());
                            //int intUser = 39331;
                            string strDefault = oUser.GetApplicationUrl(intUser, _approve_pageid);
                            if (strDefault == "")
                                oFunction.SendEmail("ClearView PCR Approval", oUser.GetName(intUser), strCC, strEMailIdsBCC, "ClearView PCR Approval", "<p><b>A PCR has been submitted and requires your approval.</b></p>", true, false);
                            else
                                oFunction.SendEmail("ClearView PCR Approval", oUser.GetName(intUser), strCC, strEMailIdsBCC, "ClearView PCR Approval", "<p><b>A PCR has been submitted and requires your approval.</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_approve_pageid) + "?id=" + _parent.ToString() + "\" target=\"_blank\">Click here to view this PCR.</a></p>", true, false);
                        }
                    }
                    else
                        ApprovePCR(_parent, _next_step + 1, _status, _approve_pageid, _wm_pageid);
                }
                else
                {
                    // Done - notify requestor
                    strNotify = "APPROVED";
                }
            }
            else
            {
                // Rejected - notify requestor
                strNotify = "REJECTED";
            }
            if (strNotify != "")
            {
                ds = GetPCR(_parent);
                UpdatePCR(_parent, 0, _status);
                int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                DateTime datModified = DateTime.Parse(ds.Tables[0].Rows[0]["modified"].ToString());
                ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
                DataSet dsResource = oResourceRequest.Get(intRequest, intItem, intNumber);
                foreach (DataRow drResource in dsResource.Tables[0].Rows)
                {
                    int intRR = Int32.Parse(drResource["id"].ToString());
                    int intUser = Int32.Parse(drResource["userid"].ToString());
                    string strDefault = oUser.GetApplicationUrl(intUser, _wm_pageid);
                    if (strDefault == "")
                        oFunction.SendEmail("ClearView PCR Approval", oUser.GetName(intUser), strCC, strEMailIdsBCC, "ClearView PCR Approval", "<p><b>The PCR Form that was submitted on " + datModified.ToLongDateString() + " has been " + strNotify + "</b></p>", true, false);
                    else
                        oFunction.SendEmail("ClearView PCR Approval", oUser.GetName(intUser), strCC, strEMailIdsBCC, "ClearView PCR Approval", "<p><b>The PCR Form that was submitted on " + datModified.ToLongDateString() + " has been " + strNotify + "</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_wm_pageid) + "?rrid=" + intRR.ToString() + "\" target=\"_blank\">Click here to view this PCR.</a></p>", true, false);
                }
            }
        }

        // Vijay Code
        public void ApprovePCR(int _parent, int _next_step, int _status, int _approve_pageid, int _wm_pageid, string _path)
        {
            DataSet ds = GetPCR(_parent);
            int intCC = Int32.Parse(ds.Tables[0].Rows[0]["cc"].ToString());
            string strCC = (intCC == 0 ? "" : oUser.GetName(intCC));
            string strNotify = "";
            if (_status != -1)
            {
                UpdatePCR(_parent, _next_step);
                UpdatePCRDetail(_parent, _next_step);
                // Approved
                if (_next_step < 5)
                {
                    // Move to next person
                    ds = GetPCRDetail(_parent, _next_step);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int intUser = Int32.Parse(dr["userid"].ToString());
                            //int intUser = 39331;
                            string strDefault = oUser.GetApplicationUrl(intUser, _approve_pageid);
                            if (strDefault == "")
                                oFunction.SendEmail("ClearView PCR Approval", oUser.GetName(intUser), strCC, strEMailIdsBCC, "ClearView PCR Approval", "<p><b>A PCR has been submitted and requires your approval.</b></p>", true, false, _path);
                            else
                                oFunction.SendEmail("ClearView PCR Approval", oUser.GetName(intUser), strCC, strEMailIdsBCC, "ClearView PCR Approval", "<p><b>A PCR has been submitted and requires your approval.</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_approve_pageid) + "?id=" + _parent.ToString() + "\" target=\"_blank\">Click here to view this PCR.</a></p>", true, false, _path);
                        }
                    }
                    else
                        ApprovePCR(_parent, _next_step + 1, _status, _approve_pageid, _wm_pageid, _path);
                }
                else
                {
                    // Done - notify requestor
                    strNotify = "APPROVED";
                }
            }
            else
            {
                // Rejected - notify requestor
                strNotify = "REJECTED";
            }
            if (strNotify != "")
            {
                ds = GetPCR(_parent);
                UpdatePCR(_parent, 0, _status);
                int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                DateTime datModified = DateTime.Parse(ds.Tables[0].Rows[0]["modified"].ToString());
                ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
                DataSet dsResource = oResourceRequest.Get(intRequest, intItem, intNumber);
                foreach (DataRow drResource in dsResource.Tables[0].Rows)
                {
                    int intRR = Int32.Parse(drResource["id"].ToString());
                    int intPid = Int32.Parse(drResource["projectid"].ToString());
                    int intUser = Int32.Parse(drResource["userid"].ToString());
                    string strDefault = oUser.GetApplicationUrl(intUser, _wm_pageid);
                    if (strDefault == "")
                        oFunction.SendEmail("ClearView PCR Approval", oUser.GetName(intUser), strCC, strEMailIdsBCC, "ClearView PCR Approval", "<p><b>The PCR Form that was submitted on " + datModified.ToLongDateString() + " has been " + strNotify + "</b></p>", true, false, _path);
                    else
                        oFunction.SendEmail("ClearView PCR Approval", oUser.GetName(intUser), strCC, strEMailIdsBCC, "ClearView PCR Approval", "<p><b>The PCR Form that was submitted on " + datModified.ToLongDateString() + " has been " + strNotify + "</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_wm_pageid) + "?pid=" + intPid.ToString() + "&div=I\" target=\"_blank\">Click here to view this PCR.</a></p>", true, false, _path);
                }
            }
        }
        public void UpdatePCRDetail(int _parent, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@step", _step);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePCRDetailStep", arParams);
        }
        public DataSet GetPCRDetail(int _parent, int _step)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPCRDetailStep", arParams);
        }
        public DataSet GetPCRDetail(int _parent)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@parent", _parent);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPCRDetail", arParams);
        }
        public DataSet GetPCRs(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPCRs", arParams);
        }
        public DataSet GetPCR(int _id, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPCRUser", arParams);
        }
        public DataSet GetPCR(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPCR", arParams);
        }

        public string GetPCRPath(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getPCRPath", arParams).ToString();
        }

        public void AddProjectClosurePDF(int _requestid, int _itemid, int _number, string _path)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@path", _path);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProjectClosurePDF", arParams);
        }

        public void UpdateProjectClosurePDF(int _requestid, int _itemid, int _number, string _path)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@path", _path);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectClosurePDF", arParams);
        }

        public DataSet GetProjectClosurePDFs(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectClosurePDFs", arParams);
        }

        public DataSet GetProjectClosurePDF(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);           
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectClosurePDF", arParams);
        }
        public DataSet Get(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWMprojectcoordinator", arParams);
        }
        public void Add(int _requestid, int _itemid, int _number, int _userid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWMprojectcoordinator", arParams);
        }
        public void Unlock(int _requestid, int _itemid, int _number, string _d_hrs, string _costs, string _project_start_date, int _userid)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@d_hrs", (_d_hrs == "" ? 0.00 : double.Parse(_d_hrs)));
            arParams[4] = new SqlParameter("@costs", _costs);
            arParams[5] = new SqlParameter("@project_start_date", (_project_start_date == "" ? SqlDateTime.Null : DateTime.Parse(_project_start_date)));
            arParams[6] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWMprojectcoordinatorUnlock", arParams);
        }
        public void Discovery(int _requestid, int _itemid, int _number, string _appid, string _appexd, string _apphd, string _actid, string _acted, string _acthd, string _estid, string _ested, string _esthd, string _appsd, string _apped, string _start_d, string _end_d, string _d_done, string _p_hrs, int _userid)
        {
            arParams = new SqlParameter[19];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@appid", (_appid == "" ? 0.00 : double.Parse(_appid)));
            arParams[4] = new SqlParameter("@appexd", (_appexd == "" ? 0.00 : double.Parse(_appexd)));
            arParams[5] = new SqlParameter("@apphd", (_apphd == "" ? 0.00 : double.Parse(_apphd)));
            arParams[6] = new SqlParameter("@actid", (_actid == "" ? 0.00 : double.Parse(_actid)));
            arParams[7] = new SqlParameter("@acted", (_acted == "" ? 0.00 : double.Parse(_acted)));
            arParams[8] = new SqlParameter("@acthd", (_acthd == "" ? 0.00 : double.Parse(_acthd)));
            arParams[9] = new SqlParameter("@estid", (_estid == "" ? 0.00 : double.Parse(_estid)));
            arParams[10] = new SqlParameter("@ested", (_ested == "" ? 0.00 : double.Parse(_ested)));
            arParams[11] = new SqlParameter("@esthd", (_esthd == "" ? 0.00 : double.Parse(_esthd)));
            arParams[12] = new SqlParameter("@appsd", (_appsd == "" ? SqlDateTime.Null : DateTime.Parse(_appsd)));
            arParams[13] = new SqlParameter("@apped", (_apped == "" ? SqlDateTime.Null : DateTime.Parse(_apped)));
            arParams[14] = new SqlParameter("@start_d", (_start_d == "" ? SqlDateTime.Null : DateTime.Parse(_start_d)));
            arParams[15] = new SqlParameter("@end_d", (_end_d == "" ? SqlDateTime.Null : DateTime.Parse(_end_d)));
            arParams[16] = new SqlParameter("@d_done", (_d_done == "" ? SqlDateTime.Null : DateTime.Parse(_d_done)));
            arParams[17] = new SqlParameter("@p_hrs", (_p_hrs == "" ? 0.00 : double.Parse(_p_hrs)));
            arParams[18] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWMprojectcoordinatorDiscovery", arParams);
        }
        public void Planning(int _requestid, int _itemid, int _number, string _appip, string _appexp, string _apphp, string _actip, string _actep, string _acthp, string _estip, string _estep, string _esthp, string _appsp, string _appep, string _start_p, string _end_p, string _p_done, string _e_hrs, int _userid)
        {
            arParams = new SqlParameter[19];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@appip", (_appip == "" ? 0.00 : double.Parse(_appip)));
            arParams[4] = new SqlParameter("@appexp", (_appexp == "" ? 0.00 : double.Parse(_appexp)));
            arParams[5] = new SqlParameter("@apphp", (_apphp == "" ? 0.00 : double.Parse(_apphp)));
            arParams[6] = new SqlParameter("@actip", (_actip == "" ? 0.00 : double.Parse(_actip)));
            arParams[7] = new SqlParameter("@actep", (_actep == "" ? 0.00 : double.Parse(_actep)));
            arParams[8] = new SqlParameter("@acthp", (_acthp == "" ? 0.00 : double.Parse(_acthp)));
            arParams[9] = new SqlParameter("@estip", (_estip == "" ? 0.00 : double.Parse(_estip)));
            arParams[10] = new SqlParameter("@estep", (_estep == "" ? 0.00 : double.Parse(_estep)));
            arParams[11] = new SqlParameter("@esthp", (_esthp == "" ? 0.00 : double.Parse(_esthp)));
            arParams[12] = new SqlParameter("@appsp", (_appsp == "" ? SqlDateTime.Null : DateTime.Parse(_appsp)));
            arParams[13] = new SqlParameter("@appep", (_appep == "" ? SqlDateTime.Null : DateTime.Parse(_appep)));
            arParams[14] = new SqlParameter("@start_p", (_start_p == "" ? SqlDateTime.Null : DateTime.Parse(_start_p)));
            arParams[15] = new SqlParameter("@end_p", (_end_p == "" ? SqlDateTime.Null : DateTime.Parse(_end_p)));
            arParams[16] = new SqlParameter("@p_done", (_p_done == "" ? SqlDateTime.Null : DateTime.Parse(_p_done)));
            arParams[17] = new SqlParameter("@e_hrs", (_e_hrs == "" ? 0.00 : double.Parse(_e_hrs)));
            arParams[18] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWMprojectcoordinatorPlanning", arParams);
        }
        public void Execution(int _requestid, int _itemid, int _number, string _appie, string _appexe, string _apphe, string _actie, string _actee, string _acthe, string _estie, string _estee, string _esthe, string _appse, string _appee, string _start_e, string _end_e, string _e_done, string _c_hrs, int _userid)
        {
            arParams = new SqlParameter[19];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@appie", (_appie == "" ? 0.00 : double.Parse(_appie)));
            arParams[4] = new SqlParameter("@appexe", (_appexe == "" ? 0.00 : double.Parse(_appexe)));
            arParams[5] = new SqlParameter("@apphe", (_apphe == "" ? 0.00 : double.Parse(_apphe)));
            arParams[6] = new SqlParameter("@actie", (_actie == "" ? 0.00 : double.Parse(_actie)));
            arParams[7] = new SqlParameter("@actee", (_actee == "" ? 0.00 : double.Parse(_actee)));
            arParams[8] = new SqlParameter("@acthe", (_acthe == "" ? 0.00 : double.Parse(_acthe)));
            arParams[9] = new SqlParameter("@estie", (_estie == "" ? 0.00 : double.Parse(_estie)));
            arParams[10] = new SqlParameter("@estee", (_estee == "" ? 0.00 : double.Parse(_estee)));
            arParams[11] = new SqlParameter("@esthe", (_esthe == "" ? 0.00 : double.Parse(_esthe)));
            arParams[12] = new SqlParameter("@appse", (_appse == "" ? SqlDateTime.Null : DateTime.Parse(_appse)));
            arParams[13] = new SqlParameter("@appee", (_appee == "" ? SqlDateTime.Null : DateTime.Parse(_appee)));
            arParams[14] = new SqlParameter("@start_e", (_start_e == "" ? SqlDateTime.Null : DateTime.Parse(_start_e)));
            arParams[15] = new SqlParameter("@end_e", (_end_e == "" ? SqlDateTime.Null : DateTime.Parse(_end_e)));
            arParams[16] = new SqlParameter("@e_done", (_e_done == "" ? SqlDateTime.Null : DateTime.Parse(_e_done)));
            arParams[17] = new SqlParameter("@c_hrs", (_c_hrs == "" ? 0.00 : double.Parse(_c_hrs)));
            arParams[18] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWMprojectcoordinatorExecution", arParams);
        }
        public void Closing(int _requestid, int _itemid, int _number, string _appic, string _appexc, string _apphc, string _actic, string _actec, string _acthc, string _estic, string _estec, string _esthc, string _appsc, string _appec, string _start_c, string _end_c, string _c_done, string _better, string _worse, string _lessons, int _userid)
        {
            arParams = new SqlParameter[21];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@appic", (_appic == "" ? 0.00 : double.Parse(_appic)));
            arParams[4] = new SqlParameter("@appexc", (_appexc == "" ? 0.00 : double.Parse(_appexc)));
            arParams[5] = new SqlParameter("@apphc", (_apphc == "" ? 0.00 : double.Parse(_apphc)));
            arParams[6] = new SqlParameter("@actic", (_actic == "" ? 0.00 : double.Parse(_actic)));
            arParams[7] = new SqlParameter("@actec", (_actec == "" ? 0.00 : double.Parse(_actec)));
            arParams[8] = new SqlParameter("@acthc", (_acthc == "" ? 0.00 : double.Parse(_acthc)));
            arParams[9] = new SqlParameter("@estic", (_estic == "" ? 0.00 : double.Parse(_estic)));
            arParams[10] = new SqlParameter("@estec", (_estec == "" ? 0.00 : double.Parse(_estec)));
            arParams[11] = new SqlParameter("@esthc", (_esthc == "" ? 0.00 : double.Parse(_esthc)));
            arParams[12] = new SqlParameter("@appsc", (_appsc == "" ? SqlDateTime.Null : DateTime.Parse(_appsc)));
            arParams[13] = new SqlParameter("@appec", (_appec == "" ? SqlDateTime.Null : DateTime.Parse(_appec)));
            arParams[14] = new SqlParameter("@start_c", (_start_c == "" ? SqlDateTime.Null : DateTime.Parse(_start_c)));
            arParams[15] = new SqlParameter("@end_c", (_end_c == "" ? SqlDateTime.Null : DateTime.Parse(_end_c)));
            arParams[16] = new SqlParameter("@c_done", (_c_done == "" ? SqlDateTime.Null : DateTime.Parse(_c_done)));
            arParams[17] = new SqlParameter("@better", _better);
            arParams[18] = new SqlParameter("@worse", _worse);
            arParams[19] = new SqlParameter("@lessons", _lessons);
            arParams[20] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWMprojectcoordinatorClosing", arParams);
        }

        public DataSet GetHours(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWMprojectcoordinatorHours", arParams);
        }

        public DataSet GetHours(int _requestid, int _itemid, int _number, string _phase)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@phase", _phase);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWMprojectcoordinatorHoursPhase", arParams);
        }
        public double GetHoursTotal(int _requestid, int _itemid, int _number)
        {
            double dblTotal = 0.00;
            DataSet ds = GetHours(_requestid, _itemid, _number);
            foreach (DataRow dr in ds.Tables[0].Rows)
                dblTotal += double.Parse(dr["used"].ToString());
            return dblTotal;
        }
        public double GetHoursTotal(int _requestid, int _itemid, int _number, string _phase)
        {
            double dblTotal = 0.00;
            DataSet ds = GetHours(_requestid, _itemid, _number, _phase);
            foreach (DataRow dr in ds.Tables[0].Rows)
                dblTotal += double.Parse(dr["used"].ToString());
            return dblTotal;
        }
        public double GetHoursAllocated(int _requestid, int _itemid, int _number)
        {
            double dblTotal = 0.00;
            DataSet ds = Get(_requestid, _itemid, _number);
            if (ds.Tables[0].Rows.Count > 0)
                dblTotal = double.Parse(ds.Tables[0].Rows[0]["d_hrs"].ToString()) + double.Parse(ds.Tables[0].Rows[0]["p_hrs"].ToString()) + double.Parse(ds.Tables[0].Rows[0]["e_hrs"].ToString()) + double.Parse(ds.Tables[0].Rows[0]["c_hrs"].ToString());
            return dblTotal;
        }

        public void UpdateHours(int _requestid, int _itemid, int _number, string _phase, double _used)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@phase", _phase);
            arParams[4] = new SqlParameter("@used", _used);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWMprojectcoordinatorHours", arParams);
        }
    }
}
