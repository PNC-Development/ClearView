using System;
using System.DirectoryServices;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;
using System.Management;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace NCC.ClearView.Application.Core
{
	public class DataPoint
	{
        private string dsn = "";
        private int user = 0;
        private SqlParameter[] arParams;
        private string strValidation = "";
        private bool boolEdit = false;
        public DataPoint(int _user, string _dsn)
		{
            user = _user;
            dsn = _dsn;
		}
        public ManagementObjectCollection GetWin32Fix(string _server, string _query, string _location, int _environment)
        {
            ManagementScope oScope = GetScope(_server, _location, _environment);
            if (oScope == null)
                return null;
            else
            {
                SelectQuery oQuery = new SelectQuery(_query);
                ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oScope, oQuery);
                return oSearcher.Get();
            }
        }
        public ManagementScope GetScope(string _server, string _location, int _environment)
        {
            Variables oVariable;
            WMIConnection oConnection;
            if (_environment == 0)
            {
                try
                {
                    // CORPDEV
                    oVariable = new Variables(2);
                    oConnection = new WMIConnection(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain(), _server, _location);
                    return oConnection.GetConnectionScope;
                }
                catch { }
                try
                {
                    // CORPTEST
                    oVariable = new Variables(3);
                    oConnection = new WMIConnection(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain(), _server, _location);
                    return oConnection.GetConnectionScope;
                }
                catch { }
                try
                {
                    // CORPDMN
                    oVariable = new Variables(4);
                    oConnection = new WMIConnection(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain(), _server, _location);
                    return oConnection.GetConnectionScope;
                }
                catch { }
                try
                {
                    // PNC
                    oVariable = new Variables(999);
                    oConnection = new WMIConnection(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain(), _server, _location);
                    return oConnection.GetConnectionScope;
                }
                catch { }
            }
            else
            {
                oVariable = new Variables(_environment);
                oConnection = new WMIConnection(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain(), _server, _location);
                return oConnection.GetConnectionScope;
            }
            return null;
        }
        public ManagementClass GetClassFix(string _server, string _path, string _StdRegProv, int _environment)
        {
            Variables oVariable = new Variables(GetDomainIDFix(_server, _environment));
            ConnectionOptions options = new ConnectionOptions();
            options.Username = oVariable.Domain() + "\\" + oVariable.ADUser();
            options.Password = oVariable.ADPassword();
            options.Impersonation = ImpersonationLevel.Impersonate;
            options.EnablePrivileges = true;
            ManagementScope myScope = new ManagementScope("\\\\" + _server + _path, options);
            ManagementPath mypath = new ManagementPath(_StdRegProv);
            return new ManagementClass(myScope, mypath, null);
        }
        public string GetDomainNameFix(string _server, int _environment)
        {
            Variables oVariable = new Variables(0);
            string strDomain = "";
            ManagementObjectCollection oItems = GetWin32Fix(_server, "SELECT * FROM Win32_ComputerSystem", "CIMV2", _environment);
            if (oItems != null)
            {
                foreach (ManagementObject oItem in oItems)
                {
                    strDomain = oItem["domain"].ToString();
                    break;
                }
            }
            return strDomain;
        }
        public int GetDomainIDFix(string _server, int _environment)
        {
            Variables oVariable = new Variables(0);
            return oVariable.DomainID(GetDomainNameFix(_server, _environment));
        }

        public string AssetHistorySelect(int _assetid)
        {
            string strSQL="";
            strSQL=strSQL+" SELECT cva_status.*, cvStatusList.StatusDescription AS statusname FROM cva_status ";
            strSQL=strSQL+" INNER JOIN ClearView.dbo.cv_StatusList cvStatusList ";
            strSQL=strSQL+" ON  cva_status.status = cvStatusList.StatusValue ";
            strSQL=strSQL+" AND cvStatusList.StatusKey='ASSETSTATUS' AND cvStatusList.deleted = 0 ";
            strSQL=strSQL+" WHERE assetid = " + _assetid.ToString() ;
            strSQL = strSQL + " ORDER BY cva_status.deleted, cva_status.datestamp DESC ";

            return strSQL;
        }

        public void AddSearch(string _name, int _userid, string _type)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@type", _type);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDatapointSearch", arParams);
        }
        public void DeleteSearch(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDatapointSearches", arParams);
        }
        public void DeleteSearch(string _name, int _userid, string _type)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@type", _type);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDatapointSearch", arParams);
        }
        public DataSet GetSearch(string _name, int _userid, string _type)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@type", _type);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointSearch", arParams);
        }


        #region ASSET
        public DataSet GetAssetNameExact(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointAssetNameExact", arParams);
        }
        public DataSet GetAssetName(string _name, int _id, int _mnemonicid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@id", _id);
            arParams[2] = new SqlParameter("@mnemonicid", _mnemonicid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointAssetName", arParams);
        }
        public DataSet GetAssetName(string _name, int _id, int _mnemonicid, string _start_date, string _end_date, int _decommissions)
        {
            //return GetAssetName(_name, _id, _mnemonicid);
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@id", _id);
            arParams[2] = new SqlParameter("@mnemonicid", _mnemonicid);
            arParams[3] = new SqlParameter("@start_date", (_start_date == "" ? SqlDateTime.Null : DateTime.Parse(_start_date)));
            arParams[4] = new SqlParameter("@end_date", (_end_date == "" ? SqlDateTime.Null : DateTime.Parse(_end_date)));
            arParams[5] = new SqlParameter("@decommissions", _decommissions);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointAssetName", arParams);
        }
        //public string GetAssetName(string _name, int _id, int _mnemonicid, string _start_date, string _end_date, int _decommissions, string _column)
        //{
        //    DataSet ds = GetAssetName(_name, _id, _mnemonicid, _start_date, _end_date, _decommissions);
        //    if (ds.Tables[0].Rows.Count > 0)
        //        return ds.Tables[0].Rows[0][_column].ToString();
        //    else
        //        return "";
        //}
        public DataSet GetAssetSerialOrTag(string _serial, string _asset)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serial", _serial);
            arParams[1] = new SqlParameter("@asset", _asset);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointAssetSerialOrTag", arParams);
        }
        public string GetAssetSerialOrTag(string _serial, string _asset, string _column)
        {
            DataSet ds = GetAssetSerialOrTag(_serial, _asset);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetAssetDeploy(int _modelid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointAssetDeploy", arParams);
        }
        public DataSet GetAsset(int _assetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@assetid", _assetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointAsset", arParams);
        }
        public string GetAsset(int _assetid, string _column)
        {
            DataSet ds = GetAsset(_assetid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        #endregion


        #region SERVICE
        public DataSet GetServiceDesign(string _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointServiceDesign", arParams);
        }
        public DataSet GetServiceDesignSearchResults(
                                            string _designid,
                                            string _designname,
                                            int? _status,
                                            int? _confidence,
                                            int? _createdby,
                                            int? _projectid,
                                            DateTime? _createdafter,
                                            DateTime? _createdbefore,
                                            DateTime? _modifiedafter,
                                            DateTime? _modifiedbefore,
                                            DateTime? _completedafter,
                                            DateTime? _completedbefore, 
                                            string _orderby, int? _order, int _page, int _recsperpage)
        {
            arParams = new SqlParameter[20];
            if (_designid != "")
                arParams[0] = new SqlParameter("@DesignId", _designid);
            if (_designname != "")
                arParams[1] = new SqlParameter("@DesignName", _designname);
            if (_status != null)
                arParams[2] = new SqlParameter("@Status", _status);
            if (_confidence != null)
                arParams[3] = new SqlParameter("@Confidence", _confidence);
            if (_createdby != null)
                arParams[4] = new SqlParameter("@CreatedBy", _createdby);
            if (_projectid != null)
                arParams[5] = new SqlParameter("@ProjectId", _projectid);
            if (_createdafter != null)
                arParams[6] = new SqlParameter("@CreatedAfter", _createdafter);
            if (_createdbefore != null)
                arParams[7] = new SqlParameter("@CreatedBefore", _createdbefore);
            if (_modifiedafter != null)
                arParams[8] = new SqlParameter("@ModifiedAfter", _modifiedafter);
            if (_modifiedbefore != null)
                arParams[9] = new SqlParameter("@ModifiedBefore", _modifiedbefore);
            if (_completedafter != null)
                arParams[10] = new SqlParameter("@CompletedAfter", _completedafter);
            if (_orderby!="")
                arParams[11] = new SqlParameter("@OrderBy", _orderby);
            if (_order != null)
                arParams[12] = new SqlParameter("@Order", _order);
            arParams[13] = new SqlParameter("@Page", _page);
            if (_recsperpage != 0)
                arParams[14] = new SqlParameter("@RecsPerPage", _recsperpage);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointServiceDesignSearch", arParams);
        }
        public DataSet GetServiceDesign(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointServiceDesignID", arParams);
        }
        public DataSet GetServiceName(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointServiceName", arParams);
        }
        public string GetServiceName(string _name, string _column)
        {
            DataSet ds = GetServiceName(_name);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetServiceRequest(string _requestid)
        {
            _requestid = _requestid.ToUpper();
            if (_requestid.StartsWith("CVT") == false)
                _requestid = "CVT" + _requestid;
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointServiceRequest", arParams);
        }

        public DataSet GetServiceRequestSearchResults(string _requestNumber ,
                                            string _requestName,     
                                            int? _status,
                                            int? _requestor,
                                            int? _assignedBy,
                                            int? _assigneegroup,
                                            int? _assignedTo,
                                            int? _projectid,
                                            DateTime? _createdafter,
                                            DateTime? _createdbefore,
                                            DateTime? _modifiedafter,
                                            DateTime? _modifiedbefore,
                                            DateTime? _completedafter,
                                            DateTime? _completedbefore, 
                                            string _orderby, int? _order, int _page, int _recsperpage)
        {
            
            arParams = new SqlParameter[18];
            if (_requestNumber != "")
            {
                _requestNumber = _requestNumber.ToUpper();
                if (_requestNumber.StartsWith("CVT") == false)
                    _requestNumber = "CVT" + _requestNumber;
                arParams[0] = new SqlParameter("@RequestNumber", _requestNumber);
            }

            if (_requestName != "")
                arParams[1] = new SqlParameter("@RequestName", _requestName);
            if (_status != null)
                arParams[2] = new SqlParameter("@Status", _status);
            if (_requestor != null)
                arParams[3] = new SqlParameter("@Requestor", _requestor);
            if (_assignedBy != null)
                arParams[4] = new SqlParameter("@AssignedBy", _assignedBy);
            if (_assigneegroup != null)
                arParams[5] = new SqlParameter("@AssignedByGroup", _assigneegroup);
            if (_assignedTo != null)
                arParams[6] = new SqlParameter("@AssignedTo", _assignedTo);
            if (_projectid != null)
                arParams[7] = new SqlParameter("@ProjectId", _projectid);
            if (_createdafter != null)
                arParams[8] = new SqlParameter("@CreatedAfter", _createdafter);
            if (_createdbefore != null)
                arParams[9] = new SqlParameter("@CreatedBefore", _createdbefore);
            if (_modifiedafter != null)
                arParams[10] = new SqlParameter("@ModifiedAfter", _modifiedafter);
            if (_modifiedbefore != null)
                arParams[11] = new SqlParameter("@ModifiedBefore", _modifiedbefore);
            if (_completedafter != null)
                arParams[12] = new SqlParameter("@CompletedAfter", _completedafter);
            if (_orderby!="")
                arParams[13] = new SqlParameter("@OrderBy", _orderby);
            if (_order != null)
                arParams[14] = new SqlParameter("@Order", _order);
            arParams[15] = new SqlParameter("@Page", _page);
            if (_recsperpage != 0)
                arParams[16] = new SqlParameter("@RecsPerPage", _recsperpage);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointServiceRequestSearch", arParams);
        }
        public string GetServiceRequest(string _requestid, string _column)
        {
            DataSet ds = GetServiceRequest(_requestid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetServiceRequest(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointServiceRequestID", arParams);
        }
        public string GetServiceRequest(int _requestid, string _column)
        {
            DataSet ds = GetServiceRequest(_requestid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetServiceRequestResource(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointServiceRequestResourceID", arParams);
        }
        public DataSet GetServiceRequestForm(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointServiceRequestFormID", arParams);
        }
        public string GetServiceRequestResource(int _id, string _column)
        {
            DataSet ds = GetServiceRequestResource(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        public DataSet GetServiceRequestResourceInvolvement(int? _projectId,
                                                            int? _requestId, 
                                                            int? _resourceRequestId,
                                                            int? _resourceId,
                                                            int? _latest     )
        {
            arParams = new SqlParameter[5];
            if (_projectId != null)
                arParams[0] = new SqlParameter("@ProjectId", _projectId);
            if (_requestId != null)
                arParams[1] = new SqlParameter("@RequestId", _requestId);
            if (_resourceRequestId != null)
                arParams[2] = new SqlParameter("@ResourceRequestId", _resourceRequestId);
            if (_resourceId != null)
                arParams[3] = new SqlParameter("@ResourceId", _resourceId);
            if (_latest != null)
                arParams[4] = new SqlParameter("@Latest", _latest);
            DataSet dsReturn = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointResourceInvolvement", arParams);
            return dsReturn;
        }
        #endregion


        public void AddFieldPermission(int _userid, string _key)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@key", _key);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDatapointField", arParams);
        }
        public void DeleteFieldPermission(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDatapointField", arParams);
        }
        public DataSet GetFieldPermission(string _key)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@key", _key);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointFields", arParams);
        }
        public bool GetFieldPermission(int _userid, string _key)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@key", _key);
            DataSet dsReturn = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointField", arParams);
            return (dsReturn.Tables[0].Rows.Count > 0);
        }

        public void AddPagePermission(int _applicationid, string _key)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@key", _key);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDatapointApplication", arParams);
        }
        public void DeletePagePermission(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDatapointApplication", arParams);
        }
        public DataSet GetPagePermission(string _key)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@key", _key);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointApplications", arParams);
        }
        public bool GetPagePermission(int _applicationid, string _key)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@key", _key);
            DataSet dsReturn = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointApplication", arParams);
            return (dsReturn.Tables[0].Rows.Count > 0);
        }

        public void AddDeployModel(int _userid, int _modelid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@modelid", _modelid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDatapointDeployModel", arParams);
        }
        public void DeleteDeployModel(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDatapointDeployModel", arParams);
        }
        public DataSet GetDeployModel()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointDeployModelsAll");
        }
        public DataSet GetDeployModel(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointDeployModels", arParams);
        }
        public bool GetDeployModel(int _userid, int _modelid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@modelid", _modelid);
            DataSet dsReturn = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointDeployModel", arParams);
            return (dsReturn.Tables[0].Rows.Count > 0);
        }

        public void LoadDropDownAJAX(TextBox txtControl, HiddenField hdnControl, HtmlGenericControl divControl, ListBox lstControl, int _environment, int _userid, Button btnLookup, string _lookup, Label lblControl, Label lblAdmin, string _key, int _value_id, string _value_text, string _ajax_url, string _id, bool _no_updates, bool _required)
        {
            LoadDropDownAJAX(txtControl, hdnControl, divControl, lstControl, _environment, _userid, btnLookup, _lookup, lblControl, lblAdmin, _key, _value_id, _value_text, _ajax_url, 2, _id, _no_updates, _required);
        }
        public void LoadDropDownAJAX(TextBox txtControl, HiddenField hdnControl, HtmlGenericControl divControl, ListBox lstControl, int _environment, int _userid, Button btnLookup, string _lookup, Label lblControl, Label lblAdmin, string _key, int _value_id, string _value_text, string _ajax_url, int _length, string _id, bool _no_updates, bool _required)
        {
            Users oUser = new Users(user, dsn);
            Variables oVariable = new Variables(_environment);
            LoadButtonAdmin(lblAdmin, _userid, _key);
            hdnControl.Value = _value_id.ToString();
            if (_no_updates == false && (GetFieldPermission(_userid, _key) == true || oUser.IsAdmin(_userid) == true))
            {
                LoadButtonLookup(btnLookup, _lookup);
                txtControl.Text = _value_text;
                if (_required == true)
                {
                    if (strValidation != "")
                        strValidation += " && ";
                    strValidation += "ValidateHidden0('" + hdnControl.ClientID + "','" + txtControl.ClientID + "','Please enter a value (and select from the list)')";
                }
                int intWidth = Int32.Parse(txtControl.Width.Value.ToString());
                txtControl.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'" + intWidth.ToString() + "','195','" + divControl.ClientID + "','" + lstControl.ClientID + "','" + hdnControl.ClientID + "','" + oVariable.URL() + _ajax_url + "'," + _length.ToString() + ");");
                lstControl.Attributes.Add("ondblclick", "AJAXClickRow();");
                txtControl.Visible = true;
            }
            else
            {
                if (btnLookup != null)
                    btnLookup.Visible = false;
                if (txtControl != null)
                    txtControl.Visible = false;
                if (lblControl != null)
                {
                    if (_lookup != "")
                        lblControl.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('" + _lookup + "', '800', '600');\">" + _value_text + "</a>";
                    else
                        lblControl.Text = _value_text;
                }
            }
        }
        public void LoadDropDown(DropDownList oDDL, int _userid, Button btnLookup, string _lookup, Label lblControl, Label lblAdmin, string _key, string _text, string _value, DataSet dsSource, int _selected, bool _exclude_0, bool _no_updates, bool _required)
        {
            Users oUser = new Users(user, dsn);
            LoadButtonAdmin(lblAdmin, _userid, _key);
            oDDL.DataTextField = _text;
            oDDL.DataValueField = _value;
            oDDL.DataSource = dsSource;
            oDDL.DataBind();
            string strSelected = "";
            if (oDDL.Items.Count > 0)
            {
                if (_exclude_0 == false)
                {
                    if (_required == false)
                        oDDL.Items.Insert(0, new ListItem("-- NONE --", "0"));
                    else
                        oDDL.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                }
            }
            else
            {
                if (_exclude_0 == false)
                    oDDL.Items.Insert(0, new ListItem("-- N / A --", "0"));
                strSelected = "N / A";
            }
            oDDL.SelectedValue = _selected.ToString();
            if (_selected > 0 || _exclude_0==true)
                strSelected = oDDL.SelectedItem.Text;
            if (_no_updates == false && (GetFieldPermission(_userid, _key) == true || oUser.IsAdmin(_userid) == true))
            {
                LoadButtonLookup(btnLookup, _lookup);
                oDDL.Visible = true;
                Common oCommon = new Common();
                oCommon.AddAttribute(oDDL, "onchange", "DataPointChanged();");
                if (_required == true && _exclude_0 == false)
                {
                    if (strValidation != "")
                        strValidation += " && ";
                    strValidation += "ValidateDropDown('" + oDDL.ClientID + "','Please select a value')";
                }
            }
            else
            {
                if (btnLookup != null)
                    btnLookup.Visible = false;
                if (oDDL != null)
                    oDDL.Visible = false;
                if (lblControl != null)
                {
                    if (strSelected == "")
                        strSelected = "N / A";
                    if (_lookup != "")
                        lblControl.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('" + _lookup + "', '800', '600');\">" + strSelected + "</a>";
                    else
                        lblControl.Text = strSelected;
                }
            }
        }
        public void LoadCheckBoxList(CheckBoxList oList, int _userid, Button btnLookup, string _lookup, Label lblControl, Label lblAdmin, string _key, string _text, string _value, DataSet dsSource, DataSet dsSelected, string _compare, bool _no_updates)
        {
            Users oUser = new Users(user, dsn);
            LoadButtonAdmin(lblAdmin, _userid, _key);
            oList.DataTextField = _text;
            oList.DataValueField = _value;
            oList.DataSource = dsSource;
            oList.DataBind();
            string strSelected = "";
            foreach (DataRow drSelected in dsSelected.Tables[0].Rows)
            {
                foreach (ListItem oItem in oList.Items)
                {
                    int intCompare = Int32.Parse(drSelected[_compare].ToString());
                    if (intCompare == Int32.Parse(oItem.Value))
                    {
                        oItem.Selected = true;
                        if (strSelected != "")
                            strSelected += ", ";
                        strSelected += oItem.Text;
                        break;
                    }
                }
            }
            if (_no_updates == false && (GetFieldPermission(_userid, _key) == true || oUser.IsAdmin(_userid) == true))
            {
                LoadButtonLookup(btnLookup, _lookup);
                oList.Visible = true;
                Common oCommon = new Common();
                oCommon.AddAttribute(oList, "onclick", "DataPointChanged();");
            }
            else
            {
                if (btnLookup != null)
                    btnLookup.Visible = false;
                if (oList != null)
                    oList.Visible = false;
                if (lblControl != null)
                {
                    if (strSelected == "")
                        strSelected = "N / A";
                    if (_lookup != "")
                        lblControl.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('" + _lookup + "', '800', '600');\">" + strSelected + "</a>";
                    else
                        lblControl.Text = strSelected;
                }
            }
        }
        public void LoadTextBox(TextBox txtControl, int _userid, Button btnLookup, string _lookup, Label lblControl, Label lblAdmin, string _key, string _value, string _id, bool _no_updates, bool _required)
        {
            Users oUser = new Users(user, dsn);
            LoadButtonAdmin(lblAdmin, _userid, _key);
            txtControl.Text = _value;
            if (_no_updates == false && (GetFieldPermission(_userid, _key) == true || oUser.IsAdmin(_userid) == true))
            {
                LoadButtonLookup(btnLookup, _lookup);
                txtControl.ToolTip = _id;
                txtControl.Visible = true;
                Common oCommon = new Common();
                oCommon.AddAttribute(txtControl, "onchange", "DataPointChanged();");
                if (_required == true)
                {
                    if (strValidation != "")
                        strValidation += " && ";
                    strValidation += "ValidateText('" + txtControl.ClientID + "','Please enter a value')";
                }
            }
            else
            {
                if (btnLookup != null)
                    btnLookup.Visible = false;
                if (txtControl != null)
                    txtControl.Visible = false;
                if (lblControl != null)
                {
                    if (_lookup != "")
                        lblControl.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('" + _lookup + "', '800', '600');\">" + _value + "</a>";
                    else
                        lblControl.Text = _value;
                }
            }
        }
        public void LoadTextBoxDate(TextBox txtControl, ImageButton imgControl, int _userid, Button btnLookup, string _lookup, Label lblControl, Label lblAdmin, string _key, string _value, string _id, bool _no_updates, bool _required)
        {
            Users oUser = new Users(user, dsn);
            LoadButtonAdmin(lblAdmin, _userid, _key);
            try { _value = DateTime.Parse(_value).ToShortDateString(); }
            catch { }
            txtControl.Text = _value;
            if (_no_updates == false && (GetFieldPermission(_userid, _key) == true || oUser.IsAdmin(_userid) == true))
            {
                LoadButtonLookup(btnLookup, _lookup);
                txtControl.ToolTip = _id;
                txtControl.Visible = true;
                Common oCommon = new Common();
                oCommon.AddAttribute(txtControl, "onchange", "DataPointChanged();");
                if (_required == true)
                {
                    if (strValidation != "")
                        strValidation += " && ";
                    strValidation += "ValidateDate('" + txtControl.ClientID + "','Please enter a valid date')";
                }
                imgControl.Attributes.Add("onclick", "return ShowCalendar('" + txtControl.ClientID + "');");
                imgControl.Visible = true;
            }
            else
            {
                if (btnLookup != null)
                    btnLookup.Visible = false;
                if (txtControl != null)
                    txtControl.Visible = false;
                if (imgControl != null)
                    imgControl.Visible = false;
                if (lblControl != null)
                {
                    if (_lookup != "")
                        lblControl.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('" + _lookup + "', '800', '600');\">" + _value + "</a>";
                    else
                        lblControl.Text = _value;
                }
            }
        }
        public void LoadTextBoxDeviceName(TextBox txtControl, Button btnControl, CheckBox chkControl, bool _use_hidden_company_prompt, HiddenField hdnControl, int _userid, Button btnLookup, string _lookup, Label lblControl, Label lblAdmin, string _key, string _value, string _id, bool _no_updates, bool _required)
        {
            Users oUser = new Users(user, dsn);
            LoadButtonAdmin(lblAdmin, _userid, _key);
            txtControl.Text = _value;

            txtControl.ReadOnly = true;
            btnControl.Enabled = true;
            if (_value == "")
            {
                //txtControl.ReadOnly = false;
                //btnControl.Enabled = false;
                if (chkControl != null)
                    chkControl.Enabled = false;
            }
            else
            {
                //txtControl.ReadOnly = true;
                //btnControl.Enabled = true;
                if (chkControl != null)
                    chkControl.Enabled = true;
            }
            if (_no_updates == false && (GetFieldPermission(_userid, _key) == true || oUser.IsAdmin(_userid) == true))
            {
                LoadButtonLookup(btnLookup, _lookup);
                txtControl.ToolTip = _id;
                txtControl.Visible = true;
                Common oCommon = new Common();
                oCommon.AddAttribute(txtControl, "onchange", "DataPointChanged();");
                if (_required == true && txtControl.Enabled == true)
                {
                    if (strValidation != "")
                        strValidation += " && ";
                    if (_use_hidden_company_prompt == true)
                        strValidation += "ValidateDataPointDeviceName(true, '" + txtControl.ClientID + "','Please enter a value','" + hdnControl.ClientID + "')";
                    else
                        strValidation += "ValidateText('" + txtControl.ClientID + "','Please enter a value')";
                }
                else if (txtControl.Enabled == true && _use_hidden_company_prompt == true)
                {
                    if (strValidation != "")
                        strValidation += " && ";
                    strValidation += "ValidateDataPointDeviceName(false, '" + txtControl.ClientID + "','Please enter a value','" + hdnControl.ClientID + "')";
                }
                btnControl.Visible = true;
                if (chkControl != null)
                    chkControl.Visible = true;
            }
            else
            {
                if (btnLookup != null)
                    btnLookup.Visible = false;
                if (txtControl != null)
                    txtControl.Visible = false;
                if (btnControl != null)
                    btnControl.Visible = false;
                if (chkControl != null)
                    chkControl.Visible = false;
                if (lblControl != null)
                {
                    if (_lookup != "")
                        lblControl.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('" + _lookup + "', '800', '600');\">" + _value + "</a>";
                    else
                        lblControl.Text = _value;
                }
            }
        }
        public void LoadCheckBox(CheckBox chkControl, int _userid, Button btnLookup, string _lookup, Label lblControl, Label lblAdmin, string _key, bool _checked, string _id, bool _no_updates)
        {
            Users oUser = new Users(user, dsn);
            LoadButtonAdmin(lblAdmin, _userid, _key);
            chkControl.Checked = _checked;
            if (_no_updates == false && (GetFieldPermission(_userid, _key) == true || oUser.IsAdmin(_userid) == true))
            {
                LoadButtonLookup(btnLookup, _lookup);
                chkControl.ToolTip = _id;
                chkControl.Visible = true;
                Common oCommon = new Common();
                oCommon.AddAttribute(chkControl, "onclick", "DataPointChanged();");
            }
            else
            {
                if (btnLookup != null)
                    btnLookup.Visible = false;
                if (chkControl != null)
                    chkControl.Visible = false;
                if (lblControl != null)
                {
                    if (_lookup != "")
                        lblControl.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('" + _lookup + "', '800', '600');\">" + (_checked ? "Yes" : "No") + "</a>";
                    else
                        lblControl.Text = (_checked ? "Yes" : "No");
                }
            }
        }
        public void LoadPanel(Panel panControl, int _userid, Label lblAdmin, string _key)
        {
            Users oUser = new Users(user, dsn);
            LoadButtonAdmin(lblAdmin, _userid, _key);
            if (GetFieldPermission(_userid, _key) == true || oUser.IsAdmin(_userid) == true)
                panControl.Visible = true;
            else
                panControl.Visible = false;
        }
        public void LoadButton(Button btnControl, int _userid, Label lblAdmin, string _key, string _link)
        {
            Users oUser = new Users(user, dsn);
            LoadButtonAdmin(lblAdmin, _userid, _key);
            if (GetFieldPermission(_userid, _key) == true || oUser.IsAdmin(_userid) == true)
            {
                btnControl.Visible = true;
                btnControl.Attributes.Add("onclick", _link);
            }
            else
                btnControl.Visible = false;
        }
        public void LoadButton(Button btnControl, int _userid, Label lblAdmin, string _key)
        {
            Users oUser = new Users(user, dsn);
            LoadButtonAdmin(lblAdmin, _userid, _key);
            if (GetFieldPermission(_userid, _key) == true || oUser.IsAdmin(_userid) == true)
                btnControl.Enabled = true;
            else
                btnControl.Enabled = false;
        }
        public void LoadButtonLookup(Button btnLookup, string _lookup)
        {
            if (btnLookup != null)
            {
                if (_lookup != "")
                {
                    btnLookup.Visible = true;
                    btnLookup.Attributes.Add("onclick", "return OpenNewWindowMenu('" + _lookup + "', '800', '600');");
                }
                else
                    btnLookup.Visible = false;
            }
        }
        public void LoadButtonAdmin(Label lblAdmin, int _userid, string _key)
        {
            Users oUser = new Users(user, dsn);
            if (lblAdmin != null)
            {
                if (oUser.IsAdmin(_userid) == true)
                {
                    string strClass = lblAdmin.CssClass;
                    lblAdmin.Text = "<a href=\"javascript:void(0);\" " + (strClass != "" && strClass != "default" ? "class=\"" + strClass + "\" " : "") + "onclick=\"OpenWindow('DATAPOINT_FIELDS','?key=" + _key + "');\">" + lblAdmin.Text + "</a>";
                }
            }
            boolEdit = true;
        }
        public string LoadValidation()
        {
            return LoadValidation("");
        }
        public string LoadValidation(string strAdditional)
        {
            if (strValidation == "")
            {
                if (boolEdit == true)
                    return "return DataPointSaved();";
                else
                    return "alert('You do not have permission to modify this record');DataPointSaved();return false;";
            }
            else
                return "return DataPointSaved() && " + strValidation + (strAdditional == "" ? "" : " && " + strAdditional) + ";";
        }

        #region Projects
        public DataSet GetProjectSearchResults(
                                        int? _ProjectId,
                                        string _ProjectNumber,
                                        string _ProjectName,
                                        int? _Status,
                                        int? _ProjectManager,
                                        int? _Organization,
                                        DateTime? _createdafter,
                                        DateTime? _createdbefore,
                                        DateTime? _modifiedafter,
                                        DateTime? _modifiedbefore,
                                        DateTime? _completedafter,
                                        DateTime? _completedbefore, 
                                        string _orderby, int? _order, int _page, int _recsperpage)
        {
            arParams = new SqlParameter[20];
            if (_ProjectId != null)
                arParams[0] = new SqlParameter("@ProjectId", _ProjectId);
            if (_ProjectNumber != "")
                arParams[1] = new SqlParameter("@ProjectNumber", _ProjectNumber);
            if (_ProjectName != "")
                arParams[2] = new SqlParameter("@ProjectName", _ProjectName);
            if (_Organization != null)
                arParams[3] = new SqlParameter("@Organization", _Organization);
            if (_ProjectManager != null)
                arParams[4] = new SqlParameter("@ProjectManager", _ProjectManager);
            if (_Status != null)
                arParams[5] = new SqlParameter("@Status", _Status);
            if (_createdafter != null)
                arParams[6] = new SqlParameter("@CreatedAfter", _createdafter);
            if (_createdbefore != null)
                arParams[7] = new SqlParameter("@CreatedBefore", _createdbefore);
            if (_modifiedafter != null)
                arParams[8] = new SqlParameter("@ModifiedAfter", _modifiedafter);
            if (_modifiedbefore != null)
                arParams[9] = new SqlParameter("@ModifiedBefore", _modifiedbefore);
            if (_completedafter != null)
                arParams[10] = new SqlParameter("@ClosedAfter", _completedafter);
            if (_completedafter != null)
                arParams[11] = new SqlParameter("@ClosedBefore", _completedbefore);
            if (_orderby != "")
                arParams[12] = new SqlParameter("@OrderBy", _orderby);
            if (_order != null)
                arParams[13] = new SqlParameter("@Order", _order);
            arParams[14] = new SqlParameter("@Page", _page);
            if (_recsperpage != 0)
                arParams[15] = new SqlParameter("@RecsPerPage", _recsperpage);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointProjectSearch", arParams);
        }

        public DataSet GetProjectAssets(int? _ProjectId)
        {
            arParams = new SqlParameter[20];
            if (_ProjectId != null)
                arParams[0] = new SqlParameter("@ProjectId", _ProjectId);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointAssetSearch", arParams);

        }

        public DataSet GetProjectFinancials(int _ProjectId)
        {
            arParams = new SqlParameter[20];
            arParams[0] = new SqlParameter("@ProjectId", _ProjectId);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointProjectFinancials", arParams);

        }
       
        #endregion


        #region People Search
        public DataSet GetPeopleSearchResults(
                                    int? _userid,
                                    string _FirstName,
                                    string _LastName,
                                    string _LANId,
                                    int? _OutOfOffice,
                                    int? _Manager,
                                    int? _Application,
                                    string _orderby, int? _order, int _page, int _recsperpage)
        {
            arParams = new SqlParameter[20];
            if (_userid != null)
                arParams[0] = new SqlParameter("@UserId", _userid);
            if (_FirstName != "")
                arParams[1] = new SqlParameter("@FirstName", _FirstName);
            if (_LastName != "")
                arParams[2] = new SqlParameter("@LastName", _LastName);
            if (_LANId != null)
                arParams[3] = new SqlParameter("@LANId", _LANId);
            if (_OutOfOffice != null)
                arParams[4] = new SqlParameter("@OutOfOffice", _OutOfOffice);
            if (_Manager != null)
                arParams[5] = new SqlParameter("@Manager", _Manager);
            if (_Application != null)
                arParams[6] = new SqlParameter("@Application", _Application);
            if (_orderby != "")
                arParams[7] = new SqlParameter("@OrderBy", _orderby);
            if (_order != null)
                arParams[8] = new SqlParameter("@Order", _order);
            arParams[9] = new SqlParameter("@Page", _page);
            if (_recsperpage != 0)
                arParams[10] = new SqlParameter("@RecsPerPage", _recsperpage);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointPeopleSearch", arParams);
        }

        #endregion
    }
}
