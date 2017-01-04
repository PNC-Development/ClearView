using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NCC.ClearView.Application.Core;
using NCC.ClearView.Presentation.Web.Custom;
using Microsoft.ApplicationBlocks.Data;
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class asset_wm_service_tasks : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Users oUser;
        protected WMServiceTasks oWMServiceTasks;
        protected Requests oRequest;
        protected Services oService;
        protected ServiceRequests oServiceRequest;
        protected ServiceDetails oServiceDetail;
        protected Functions oFunction;
        protected DataPoint oDataPoint;

        protected int intProfile = 0;
        protected int intApplication = 0;
       
        protected string strBurnInAndDeploymentSteps = "";
       
        
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();

            oUser = new Users(intProfile, dsn);
            oWMServiceTasks = new WMServiceTasks(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oServiceDetail = new ServiceDetails(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oDataPoint = new DataPoint(intProfile, dsn);

            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            if (
                (Request.QueryString["requestid"] != null && Request.QueryString["requestid"] != "") &&
                (Request.QueryString["serviceid"] != null && Request.QueryString["serviceid"] != "") &&
                (Request.QueryString["itemid"] != null && Request.QueryString["itemid"] != "") &&
                (Request.QueryString["number"] != null && Request.QueryString["number"] != "") &&
                (Request.QueryString["assetid"] != null && Request.QueryString["assetid"] != ""))
            {
                hdnRequestId.Value = oFunction.decryptQueryString(Request.QueryString["requestid"]);
                hdnServiceId.Value = oFunction.decryptQueryString(Request.QueryString["serviceid"]);
                hdnItemId.Value = oFunction.decryptQueryString(Request.QueryString["itemid"]);
                hdnNumber.Value = oFunction.decryptQueryString(Request.QueryString["number"]);
                hdnAssetId.Value = oFunction.decryptQueryString(Request.QueryString["assetid"]);

                if (!IsPostBack)
                {
                  
                    LoadWMServiceTasks();
                }
                btnSave.Attributes.Add("onclick", "return ProcessControlButton();");
                btnSaveAndClose.Attributes.Add("onclick", "return ProcessControlButton();");
            
            }
            else
            {
                pnlAllow.Visible = false;
                pnlDenied.Visible = true;
            }
        }

        #region Load WM Service Tasks
            
            private void LoadWMServiceTasks()
            {
                
                DataSet ds = oWMServiceTasks.getWMServiceTasksStatus(
                                                                Int32.Parse(hdnRequestId.Value),
                                                                Int32.Parse(hdnServiceId.Value),
                                                                Int32.Parse(hdnItemId.Value),
                                                                Int32.Parse(hdnNumber.Value),
                                                                Int32.Parse(hdnAssetId.Value));

                dlWMServiceTaskList.DataSource = ds;
                dlWMServiceTaskList.DataBind();
                string strService = oService.Get(Int32.Parse(hdnServiceId.Value), "name");

                if (ds.Tables[0].Rows.Count > 1)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    int intAsset = Int32.Parse(dr["AssetId"].ToString());

                    if (intAsset == 0)
                    {
                        pnlAllow.Visible = false;
                        pnlDenied.Visible = true;
                    }
                    else
                    {
                        lblAssetID.Text = "#" + intAsset.ToString();

                        string strSerial = dr["AssetSerial"].ToString();
                        string strAsset = dr["AssetTag"].ToString();
                        string strHeader = (strSerial.Length > 15 ? strSerial.Substring(0, 15) + "..." : strSerial);

                        lblHeader.Text = "&quot;" + strHeader.ToUpper() + "&quot;";
                        Master.Page.Title = strService + "(" + strHeader + ")";
                        lblHeaderSub.Text = strService + "...";

                        txtAssetSerial.Text = dr["AssetSerial"].ToString();
                        txtAssetTag.Text = dr["AssetTag"].ToString();

                        dlWMServiceTaskList.DataSource = ds;
                        dlWMServiceTaskList.DataBind();
                    }
                }
               

                
            }

            protected void dlWMServiceTaskList_ItemDataBound(object sender, DataListItemEventArgs e)
            {

                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView drv = (DataRowView)e.Item.DataItem;

                    HiddenField hdnTaskStatusId = (HiddenField)e.Item.FindControl("hdnTaskStatusId");
                    hdnTaskStatusId.Value = drv["TaskStatusId"].ToString();


                    HiddenField hdnTaskId = (HiddenField)e.Item.FindControl("hdnTaskId");
                    hdnTaskId.Value = drv["TaskId"].ToString();

                 
                    Label lblWMServiceTaskListTask = (Label)e.Item.FindControl("lblWMServiceTaskListTask");
                    lblWMServiceTaskListTask.Text = drv["TaskName"].ToString() + (drv["Completed"].ToString() != "" ? " [Completed : " + drv["Completed"].ToString() + "]" : "");

                    CheckBox chkSelectTask = (CheckBox)e.Item.FindControl("chkSelectTask");

                    Label lblWMServiceTaskListRequestAdd = (Label)e.Item.FindControl("lblWMServiceTaskListRequestAdd");
                    

                    TextBox txtRequestNo = (TextBox)e.Item.FindControl("txtRequestNo");
                    Button btnWMServiceTaskReqAdd = (Button)e.Item.FindControl("btnWMServiceTaskReqAdd");
                   
                    btnWMServiceTaskReqAdd.Attributes.Add("onclick", "return ValidateText('" + txtRequestNo.ClientID + "','Please enter valid active request number')" +
                                                                   " && ProcessControlButton()" +
                                                                   ";");

                    if (drv["Completed"].ToString() == "")
                    {
                        chkSelectTask.Enabled = true;
                        chkSelectTask.Checked = false;
                    }
                    else
                    {
                        chkSelectTask.Checked = true;
                        chkSelectTask.Enabled = false;

                    }
                    lblWMServiceTaskListTask.Enabled = chkSelectTask.Enabled;
                    lblWMServiceTaskListRequestAdd.Enabled = chkSelectTask.Enabled;
                    
                    txtRequestNo.Enabled = chkSelectTask.Enabled;
                    btnWMServiceTaskReqAdd.Enabled = chkSelectTask.Enabled;

                    //Load the associated service request
                    if (hdnTaskStatusId.Value != "")
                    {
                        DataList dlWMServiceTaskReqList = (DataList)e.Item.FindControl("dlWMServiceTaskReqList");

                        DataTable dtWMServiceTaskReqs = getTaskReqDetails(Int32.Parse(hdnTaskStatusId.Value));
                        if (dtWMServiceTaskReqs != null)
                        {
                            foreach (DataRow drTmp in dtWMServiceTaskReqs.Rows)
                            {
                                if (drTmp["ServiceStatus"].ToString() != "3")
                                    chkSelectTask.Enabled = false;
                            }
                            dlWMServiceTaskReqList.DataSource = dtWMServiceTaskReqs;
                            dlWMServiceTaskReqList.DataBind();
                            dlWMServiceTaskReqList.ItemDataBound +=
                             new DataListItemEventHandler(this.dlWMServiceTaskReqList_ItemDataBound);
                        }
                    }

                }

            }

            protected void dlWMServiceTaskReqList_ItemDataBound(object sender, DataListItemEventArgs e)
            {

                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView drv = (DataRowView)e.Item.DataItem;

                    HiddenField hdnTaskReqStatusId = (HiddenField)e.Item.FindControl("hdnTaskReqStatusId");
                    hdnTaskReqStatusId.Value = drv["TaskStatusId"].ToString();

                    HiddenField hdnTaskRequestId = (HiddenField)e.Item.FindControl("hdnTaskRequestId");
                    hdnTaskRequestId.Value = drv["RequestId"].ToString();

                    LinkButton lnkbtnAssetBnDTaskReqNo = (LinkButton)e.Item.FindControl("lnkbtnAssetBnDTaskReqNo");
                    lnkbtnAssetBnDTaskReqNo.Text = drv["ReqServiceNumber"].ToString();
                    lnkbtnAssetBnDTaskReqNo.Attributes.Add("onclick", "return OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" +oFunction.encryptQueryString(drv["ResourceRequestID"].ToString())+"', '800', '600');");
                   
                    Label lblWMServiceTaskReqListReqName = (Label)e.Item.FindControl("lblWMServiceTaskReqListReqName");
                    lblWMServiceTaskReqListReqName.Text = drv["ServiceName"].ToString();

                    Label lblWMServiceTaskReqListResource = (Label)e.Item.FindControl("lblWMServiceTaskReqListResource");
                    lblWMServiceTaskReqListResource.Text = drv["UserAssignedToName"].ToString();

                    Label lblWMServiceTaskReqListReqStatus = (Label)e.Item.FindControl("lblWMServiceTaskReqListReqStatus");
                    lblWMServiceTaskReqListReqStatus.Text = drv["ServiceStatusName"].ToString();

                    Label lblWMServiceTaskReqListLastUpdated = (Label)e.Item.FindControl("lblWMServiceTaskReqListLastUpdated");
                    lblWMServiceTaskReqListLastUpdated.Text = (drv["RequestModified"].ToString() != "" ? DateTime.Parse(drv["RequestModified"].ToString()).ToShortDateString() : "");

                    LinkButton lnkbtnWMServiceTaskReqListRemove = (LinkButton)e.Item.FindControl("lnkbtnWMServiceTaskReqListRemove");
                    lnkbtnWMServiceTaskReqListRemove.Text = "Remove";
                    lnkbtnWMServiceTaskReqListRemove.CommandArgument = drv["RequestId"].ToString();
                    lnkbtnWMServiceTaskReqListRemove.CommandName = "REMOVEREQUEST";
                    lnkbtnWMServiceTaskReqListRemove.Attributes.Add("onclick", "return confirm('Are you sure you want to remove all requests with request# " + drv["RequestNumber"].ToString()  + " ?')&& ProcessControlButton();");

                }

            }

            protected void dlWMServiceTaskList_Command(object sender, DataListCommandEventArgs e)
            {

                if (e.CommandName.ToUpper() == "ADDREQUEST")
                {
                    //Validate if its valid request number
                    TextBox txtRequestNo = (TextBox)e.Item.FindControl("txtRequestNo");
                    int intRequest = 0;
                    if (validateRequest(txtRequestNo.Text.Trim(),ref intRequest) == false)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "InvalidRequest", "<script type=\"text/javascript\">alert('Please enter valid request number.');<" + "/" + "script>");

                    }
                    else
                    {
                        HiddenField hdnTaskId = (HiddenField)e.Item.FindControl("hdnTaskId");
                        HiddenField hdnTaskStatusId = (HiddenField)e.Item.FindControl("hdnTaskStatusId");

                        if (hdnTaskStatusId.Value == "0" || hdnTaskStatusId.Value == "")
                        {
                            int intTaskStatusId=0;
                            intTaskStatusId = oWMServiceTasks.addWMServiceTaskStatus(intTaskStatusId,
                                        Int32.Parse(hdnRequestId.Value),
                                        Int32.Parse(hdnServiceId.Value),
                                        Int32.Parse(hdnItemId.Value),
                                        Int32.Parse(hdnNumber.Value),
                                        Int32.Parse(hdnTaskId.Value),
                                        Int32.Parse(hdnAssetId.Value),
                                        double.Parse("0"),
                                        intProfile, 0);
                            hdnTaskStatusId.Value = intTaskStatusId.ToString();
                        }

                        
                        
                        oWMServiceTasks.addRemoveWMServiceTasksStatusRequest(Int32.Parse(hdnTaskStatusId.Value),
                                                                            intRequest, intProfile, 1, 0);

                        Response.Redirect(Request.Url.PathAndQuery);
                    }
                }

               
            }

            protected void dlWMServiceTaskReqList_Command(object sender, DataListCommandEventArgs e)
                {

                    if (e.CommandName.ToUpper() == "REMOVEREQUEST")
                    {

                        HiddenField hdnTaskReqStatusId = (HiddenField)e.Item.FindControl("hdnTaskReqStatusId");

                        HiddenField hdnTaskRequestId = (HiddenField)e.Item.FindControl("hdnTaskRequestId");

                        oWMServiceTasks.addRemoveWMServiceTasksStatusRequest(Int32.Parse(hdnTaskReqStatusId.Value),
                                                                            Int32.Parse(hdnTaskRequestId.Value), intProfile, 1, 1);
                        Response.Redirect(Request.Url.PathAndQuery);
                    }


                }

            protected bool validateRequest(string _requestnumber, ref int _intRequestId)
            {
                 DataTable dtRequestsActive = null;
                //Get the Active Request 
                DataSet dsRequests = oDataPoint.GetServiceRequestSearchResults(
                                           _requestnumber,
                                           "",
                                           2, null, null, null,
                                           null,
                                           null,
                                           null, null, null, null, null, null,
                                           "SubmittedOn", 1, 1, 0);

                string[] strColumnNames ={ "RequestNumber", "RequestId" };
                dtRequestsActive = dsRequests.Tables[0].DefaultView.ToTable(true, strColumnNames);
                
                DataRow[] drReqs = null;
                string strFilterCriteria = ("RequestNumber ='" + _requestnumber + "'");
                drReqs = dtRequestsActive.Select(strFilterCriteria);

                if (drReqs.Length > 0)
                {
                    _intRequestId = Int32.Parse(drReqs[0]["requestid"].ToString());
                    return true;
                }
                else
                    return false;
            }

            private DataTable getTaskReqDetails(int _taskStatusId)
            {
                DataTable dtTaskRequests = null;

                DataSet dsTaskReqs = oWMServiceTasks.getWMServiceTasksStatusRequest(_taskStatusId);
                foreach (DataRow dr in dsTaskReqs.Tables[0].Rows)
                {
                    DataSet dsReqsDetails = oDataPoint.GetServiceRequestSearchResults(
                                            dr["RequestId"].ToString(),
                                            "",
                                            null, null, null, null,
                                            null,
                                            null,
                                            null, null, null, null, null, null,
                                            "SubmittedOn", 1, 1, 0);
                    if (dtTaskRequests == null)
                        dtTaskRequests = dsReqsDetails.Tables[0];
                    else
                    {
                        foreach (DataRow drTmp in dsReqsDetails.Tables[0].Rows)
                        {
                            dtTaskRequests.ImportRow(drTmp);
                        }
                    }

                }
                if (dtTaskRequests != null)
                {
                    dtTaskRequests.Columns.Add("TaskStatusId", Type.GetType("System.Int32"));

                    foreach (DataRow dr in dtTaskRequests.Rows)
                    {
                        dr["TaskStatusId"] = _taskStatusId.ToString();
                    }
                }
                return dtTaskRequests;
            }

        #endregion


        #region SAVE WM Service Task status

        private void SaveWMServiceTaskStatus()
        {
           foreach (DataListItem dlItem in dlWMServiceTaskList.Items)
            {
                HiddenField hdnTaskId = (HiddenField)dlItem.FindControl("hdnTaskId");
                HiddenField hdnTaskStatusId = (HiddenField)dlItem.FindControl("hdnTaskStatusId");
                CheckBox chkSelectTask = (CheckBox)dlItem.FindControl("chkSelectTask");

                if (chkSelectTask.Checked == true && chkSelectTask.Enabled == true)
                {
                   
                    int intTaskStatusId = 0;

                    if (hdnTaskStatusId.Value != "")
                        intTaskStatusId = Int32.Parse(hdnTaskStatusId.Value);

                    intTaskStatusId = oWMServiceTasks.addWMServiceTaskStatus(intTaskStatusId,
                                    Int32.Parse(hdnRequestId.Value),
                                    Int32.Parse(hdnServiceId.Value),
                                    Int32.Parse(hdnItemId.Value),
                                    Int32.Parse(hdnNumber.Value),
                                    Int32.Parse(hdnTaskId.Value),
                                    Int32.Parse(hdnAssetId.Value),
                                    double.Parse("0"),
                                    intProfile,1);
                }
            }

        }

        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            SaveWMServiceTaskStatus();
            Response.Redirect(Request.Url.PathAndQuery);
        }
        
        protected void btnSaveAndClose_Click(Object Sender, EventArgs e)
        {
            SaveWMServiceTaskStatus();
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">if (window.opener == null) { parent.HidePanel(); } else { window.close(); };window.parent.SaveAndRefreshWindow();<" + "/" + "script>");

        }
                
        protected void btnClose_Click(Object Sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">if (window.opener == null) { parent.HidePanel(); } else { window.close(); };window.parent.SaveAndRefreshWindow();<" + "/" + "script>");

        }

       

        #endregion

      }
}
