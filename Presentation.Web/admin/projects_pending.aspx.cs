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
namespace NCC.ClearView.Presentation.Web
{
    public partial class projects_pending : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intViewRequest = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequest"]);
        protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
        protected int intImplementorDistributedService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrangeService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_IMPLEMENTOR_MIDRANGE"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected Projects oProject;
        protected ProjectNumber oProjectNumber;
        protected Requests oRequest;
        protected RequestItems oRequestItem;
        protected Services oService;
        protected ProjectsPending oProjectPending;
        protected Organizations oOrganization;
        protected Documents oDocument;
        protected Users oUser;
        protected Forecast oForecast;
        protected ServiceRequests oServiceRequest;
        protected int intProfile;
        protected string strProject = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/projects_pending.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oProject = new Projects(intProfile, dsn);
            oProjectNumber = new ProjectNumber(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oProjectPending = new ProjectsPending(intProfile, dsn, intEnvironment);
            oOrganization = new Organizations(intProfile, dsn);
            oDocument = new Documents(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            if (!IsPostBack)
            {
                if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
                {
                    // Process Service Requests
                    int intRequest = Int32.Parse(Request.QueryString["rid"]);
                    DataSet dsForm = oRequestItem.GetForms(intRequest);
                    foreach (DataRow drForm in dsForm.Tables[0].Rows)
                    {
                        if (drForm["done"].ToString() == "0")
                        {
                            int intItem = Int32.Parse(drForm["itemid"].ToString());
                            int intNumber = Int32.Parse(drForm["number"].ToString());
                            int intService = Int32.Parse(drForm["serviceid"].ToString());
                            string strPath = oService.Get(intService, "cp_path");
                            if (strPath.Trim() == "")
                            {
                                // For now, bypass
                                oRequestItem.UpdateFormDone(intRequest, intItem, intNumber, 1);
                            }
                            else
                            {
                                Control oControl = (Control)LoadControl(strPath);
                                PHcp.Controls.Add(oControl);
                            }
                        }
                    }
                    panPH.Visible = true;
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    DataSet ds = oProjectPending.Get(Int32.Parse(Request.QueryString["id"]));
                    int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                    if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != "")
                    {
                        int intProject = Int32.Parse(Request.QueryString["pid"]);
                        if (intProject > 0)
                        {
                            // Process Design Builder
                            DataSet dsForecast = oForecast.GetRequest(intRequest);
                            if (dsForecast.Tables[0].Rows.Count > 0)
                            {
                                int intForecast = Int32.Parse(dsForecast.Tables[0].Rows[0]["id"].ToString());
                                DataSet dsDesigns = oForecast.GetAnswers(intForecast);
                                OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
                                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                                foreach (DataRow drDesign in dsDesigns.Tables[0].Rows)
                                {
                                    int intDesign = Int32.Parse(drDesign["id"].ToString());
                                    int intDesignRequest = 0;
                                    if (Int32.TryParse(oForecast.GetAnswer(intDesign, "requestid"), out intDesignRequest) == true)
                                    {
                                        if (intDesignRequest > 0)
                                        {
                                            // Update the REQUESTID in CV_FORECAST_ANSWERS with the new project correlation
                                            oRequest.Update(intDesignRequest, intProject);
                                        }
                                    }
                                    if (oForecast.CanAutoProvision(intDesign) == false)
                                    {
                                        // Need to add a builder
                                        int intImplementorD = 0;
                                        DataSet dsResourceD = oResourceRequest.GetProjectItem(intProject, intImplementorDistributed);
                                        if (dsResourceD.Tables[0].Rows.Count > 0)
                                            intImplementorD = Int32.Parse(dsResourceD.Tables[0].Rows[0]["userid"].ToString());
                                        int intImplementorM = 0;
                                        DataSet dsResourceM = oResourceRequest.GetProjectItem(intProject, intImplementorMidrange);
                                        if (dsResourceM.Tables[0].Rows.Count > 0)
                                            intImplementorM = Int32.Parse(dsResourceM.Tables[0].Rows[0]["userid"].ToString());
                                        if (oForecast.GetPlatformDistributedForecast(intDesign, intWorkstationPlatform) == true)
                                        {
                                            if (intImplementorD > 0)
                                            {
                                                int intNextNumber = oResourceRequest.GetNumber(intRequest);
                                                int intResourceParent = oResourceRequest.Add(intRequest, -1, -1, intNextNumber, "Provisioning Task (Distributed)", 0, 6.00, 2, 1, 1, 1);
                                                int intResourceWorkflow = oResourceRequest.AddWorkflow(intResourceParent, 0, "Provisioning Task (Distributed)", intImplementorD, 0, 6.00, 2, 0);
                                                oOnDemandTasks.AddPending(intDesign, intResourceWorkflow);
                                                oResourceRequest.UpdateAssignedBy(intResourceParent, -999);
                                            }
                                            else
                                            {
                                                // Submit for assignment
                                                if (oServiceRequest.Get(intRequest, "requestid") == "")
                                                    oServiceRequest.Add(intRequest, 1, 1);
                                                int intResource = oServiceRequest.AddRequest(intRequest, intImplementorDistributed, intImplementorDistributedService, 0, 0.00, 2, 1, dsnServiceEditor);
                                                if (oServiceRequest.NotifyApproval(intResource, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                                                    oServiceRequest.NotifyTeamLead(intImplementorDistributed, intResource, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                                            }
                                        }
                                        if (oForecast.GetPlatformMidrangeForecast(intDesign) == true)
                                        {
                                            if (intImplementorM > 0)
                                            {
                                                int intNextNumber = oResourceRequest.GetNumber(intRequest);
                                                int intResourceParent = oResourceRequest.Add(intRequest, -1, -1, intNextNumber, "Provisioning Task (Midrange)", 0, 6.00, 2, 1, 1, 1);
                                                int intResourceWorkflow = oResourceRequest.AddWorkflow(intResourceParent, 0, "Provisioning Task (Midrange)", intImplementorM, 0, 6.00, 2, 0);
                                                oOnDemandTasks.AddPending(intDesign, intResourceWorkflow);
                                                oResourceRequest.UpdateAssignedBy(intResourceParent, -999);
                                            }
                                            else
                                            {
                                                // Submit for assignment
                                                if (oServiceRequest.Get(intRequest, "requestid") == "")
                                                    oServiceRequest.Add(intRequest, 1, 1);
                                                int intResource = oServiceRequest.AddRequest(intRequest, intImplementorMidrange, intImplementorMidrangeService, 0, 0.00, 2, 1, dsnServiceEditor);
                                                if (oServiceRequest.NotifyApproval(intResource, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                                                    oServiceRequest.NotifyTeamLead(intImplementorMidrange, intResource, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        oRequest.Update(intRequest, intProject);
                        oDocument.Update(intRequest, intProject);
                        oProjectPending.Approve(Int32.Parse(Request.QueryString["id"]));
                        if (intProject == 0)
                            oServiceRequest.Update(intRequest, ds.Tables[0].Rows[0]["name"].ToString());
                        Response.Redirect(Request.Path + "?rid=" + intRequest.ToString());
                    }
                    else
                    {
                        if (Request.QueryString["reject"] != null)
                        {
                            panReject.Visible = true;
                        }
                        else
                        {
                            strProject = LoadProject(0, ds.Tables[0].Rows[0], intRequest, true);
                            btnView.Attributes.Add("onclick", "return ShowServiceRequests('" + intRequest.ToString() + "');");
                            btnViewDesigns.Attributes.Add("onclick", "return ShowDesigns('" + intRequest.ToString() + "');");
                            if (Request.QueryString["c"] != null && Request.QueryString["c"] != "")
                            {
                                panCompare.Visible = true;
                                string[] strProjects;
                                char[] strSplit = { ' ' };
                                strProjects = Request.QueryString["c"].Split(strSplit);
                                bool boolOther = false;
                                for (int jj = 0; jj < strProjects.Length; jj++)
                                {
                                    if (strProjects[jj].Trim() != "")
                                    {
                                        TableRow oRow = new TableRow();
                                        TableCell oCell = new TableCell();
                                        oCell.Text = strProject;
                                        oRow.Cells.Add(oCell);
                                        oCell = new TableCell();
                                        HyperLink oCheck = new HyperLink();
                                        oCheck.ImageUrl = "/images/bigCheck.gif";
                                        oCheck.ToolTip = "Click to Assign";
                                        oCheck.NavigateUrl = Request.Path + "?id=" + Request.QueryString["id"] + "&pid=" + strProjects[jj];
                                        oCheck.Attributes.Add("onclick", "return confirm('Are you sure you want to assign this project?');");
                                        oCell.Controls.Add(oCheck);
                                        oRow.Cells.Add(oCell);
                                        oCell = new TableCell();
                                        oCell.Text = LoadProject(Int32.Parse(strProjects[jj]), null, 0, false);
                                        if (boolOther == true)
                                            oRow.Attributes.Add("bgcolor", "#EFEFEF");
                                        boolOther = !boolOther;
                                        oRow.Cells.Add(oCell);
                                        tblCompare.Rows.Add(oRow);
                                    }
                                }
                            }
                            else
                            {
                                panView.Visible = true;
                                LoadList();
                            }
                        }
                    }
                }
                else
                {
                    panAll.Visible = true;
                    DataSet ds = oProjectPending.Gets();
                    rptView.DataSource = ds;
                    rptView.DataBind();
                }
            }
            btnCreateProject.Attributes.Add("onclick", "return confirm('Are you sure you want to create this PROJECT?');");
            btnCreateTask.Attributes.Add("onclick", "return confirm('Are you sure you want to create this TASK?');");
            btnCreateTask.Enabled = false;
            btnRejectConfirm.Attributes.Add("onclick", "return confirm('Are you sure you want to reject this project / task?');");
        }
        private void LoadList()
        {
            DataSet ds = oProject.GetActive();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                string strNumber = dr["number"].ToString();
                if (strNumber != "")
                    strNumber = " (" + strNumber + ")";
                oNode.Text = dr["name"].ToString() + strNumber;
                oNode.ToolTip = dr["name"].ToString() + strNumber;
                oNode.Value = dr["projectid"].ToString();
                oTree.Nodes.Add(oNode);
            }
        }
        private string LoadProject(int _id, DataRow dr, int _requestid, bool _task)
        {
            string strReturn = "";
            if (_id == 0)
            {
                strReturn += "<p><table cellpadding=\"3\" cellspacing=\"2\" border=\"0\">";
                if (_task == true && dr["task"].ToString() == "1")
                {
                    strReturn += "<tr><td nowrap><b>Task Name:</b></td><td width=\"100%\">" + dr["name"].ToString() + "</td></tr>";
                    strReturn += "<tr><td nowrap><b>Description:</b></td><td width=\"100%\">" + dr["description"].ToString() + "</td></tr>";
                }
                else
                {
                    strReturn += "<tr><td nowrap><b>Project Name:</b></td><td width=\"100%\">" + dr["name"].ToString() + "</td></tr>";
                    strReturn += "<tr><td nowrap><b>Project Type:</b></td><td width=\"100%\">" + dr["bd"].ToString() + "</td></tr>";
                    strReturn += "<tr><td nowrap><b>Portfolio:</b></td><td width=\"100%\">" + oOrganization.GetName(Int32.Parse(dr["organization"].ToString())) + "</td></tr>";
                    strReturn += "<tr><td nowrap><b>Project Number:</b></td><td width=\"100%\">" + dr["number"].ToString() + "</td></tr>";
                }
                strReturn += "<tr><td nowrap><b>Submitted By:</b></td><td width=\"100%\">" + oUser.GetFullName(Int32.Parse(dr["userid"].ToString())) + "</td></tr>";
                strReturn += "</table></p>";
            }
            else
            {
                DataSet ds = oProject.Get(_id);
                return LoadProject(0, ds.Tables[0].Rows[0], _requestid, _task);
            }
            return strReturn;
        }
        protected void btnCompare_Click(Object Sender, EventArgs e)
        {
            string strReturn = "";
            foreach (TreeNode oNode in oTree.Nodes)
            {
                if (oNode.Checked == true)
                    strReturn += oNode.Value + " ";
            }
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&c=" + strReturn);
        }
        protected void btnCreateProject_Click(Object Sender, EventArgs e)
        {
            DataSet ds = oProjectPending.Get(Int32.Parse(Request.QueryString["id"]));
            string number = ds.Tables[0].Rows[0]["number"].ToString();
            if (String.IsNullOrEmpty(number))
                number = oProjectNumber.New();
            int intProject = oProject.Add(ds.Tables[0].Rows[0]["name"].ToString(), ds.Tables[0].Rows[0]["bd"].ToString(), number, Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString()), Int32.Parse(ds.Tables[0].Rows[0]["organization"].ToString()), Int32.Parse(ds.Tables[0].Rows[0]["segmentid"].ToString()), 1);
            oProject.Update(intProject, Int32.Parse(ds.Tables[0].Rows[0]["lead"].ToString()), Int32.Parse(ds.Tables[0].Rows[0]["executive"].ToString()), Int32.Parse(ds.Tables[0].Rows[0]["working"].ToString()), Int32.Parse(ds.Tables[0].Rows[0]["technical"].ToString()), Int32.Parse(ds.Tables[0].Rows[0]["engineer"].ToString()), Int32.Parse(ds.Tables[0].Rows[0]["other"].ToString()));
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&pid=" + intProject.ToString());
        }
        protected void btnCreateTask_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&pid=0");
        }
        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            int intID = Int32.Parse(Request.QueryString["id"]);
            oProjectPending.Update(intID, txtName.Text, txtNumber.Text);
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
        protected void btnReject_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&reject=true");
        }
        protected void btnRejectConfirm_Click(Object Sender, EventArgs e)
        {
            int intID = Int32.Parse(Request.QueryString["id"]);
            DataSet ds = oProjectPending.Get(intID);
            int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
            oRequest.Update(intRequest, -3);
            oProjectPending.Deny(Int32.Parse(Request.QueryString["id"]), txtReason.Text, intViewRequest, true);
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"]);
        }
        protected void btnFinish_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
    }
}
