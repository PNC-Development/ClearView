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
    public partial class workload : System.Web.UI.UserControl
    {

        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intViewRequest = Int32.Parse(ConfigurationManager.AppSettings["ViewRequest"]);
        protected int intViewResourceRequest = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequest"]);
        protected string strPCWM = ConfigurationManager.AppSettings["PC_WM"];
        protected string strTPMWM = ConfigurationManager.AppSettings["TPM_WM"];
        protected string strTaskWM = ConfigurationManager.AppSettings["TASK_WM"];
        protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
        protected int intMyWork = Int32.Parse(ConfigurationManager.AppSettings["MyWork"]);
        protected int intProfile;
        protected Projects oProject;
        protected ProjectRequest oProjectRequest;
        protected ResourceRequest oResourceRequest;
        protected RequestItems oRequestItem;
        protected Pages oPage;
        protected Variables oVariable;
        protected Applications oApplication;
        protected ProjectNumber oProjectNumber;
        protected Requests oRequest;
        protected RequestFields oRequestField;
        protected Documents oDocument;
        protected Users oUser;
        protected StatusLevels oStatus;
        protected Services oService;
        protected Delegates oDelegate;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strSummary = "Project Information Not Available";
        protected string strProjectSummary = "";
        protected bool boolDocuments = false;
        protected bool boolMyDocuments = false;
        protected bool boolMine = false;
        protected bool boolResource = false;
        private string strEMailIdsBCC = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oProjectRequest = new ProjectRequest(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oApplication = new Applications(intProfile, dsn);
            oProjectNumber = new ProjectNumber(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oRequestField = new RequestFields(intProfile, dsn);
            oDocument = new Documents(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oStatus = new StatusLevels();
            oService = new Services(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != "")
            {
                int intProject = Int32.Parse(Request.QueryString["pid"]);
                lblProject.Text = intProject.ToString();
                ds = oProject.GetCoordinator(intProject, 0);
                int intCoordinator = 0;
                int intRequest = 0;
                int intResource = 0;
                bool boolCoordinator = false;
                bool boolTPM = false;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if ((Int32.Parse(dr["userid"].ToString()) == intProfile || oDelegate.Get(Int32.Parse(dr["userid"].ToString()), intProfile) > 0) && (Request.QueryString["search"] == null))
                    {
                        if (dr["tpm"].ToString() == "1")
                            boolTPM = true;
                        else
                            boolCoordinator = true;
                    }
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //panProject.Visible = true;
                    intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                    intResource = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                    intCoordinator = Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                    lblCoordinator.Text = oUser.GetFullName(intCoordinator);
                    lblPhone.Text = (oUser.Get(intCoordinator, "phone") == "" ? "N / A" : oUser.Get(intCoordinator, "phone"));
                    string strEmail = oUser.GetEmail(oUser.GetName(intCoordinator), intEnvironment);
                    lblEmail.Text = (strEmail == "" ? "N / A" : "<a href=\"mailto:" + strEmail + "\" target=\"_blank\">" + strEmail + "</a>");
                    string strPager = oUser.Get(intCoordinator, "pager");
                    lblMobileDevice.Text = (strPager == "" || strPager == "0" ? "N / A" : strPager);
                    string strAt = oUser.Get(intCoordinator, "atid");
                    if (strAt != "0" && strAt != "")
                    {
                        Users_At oUserAt = new Users_At(intProfile, dsn);
                        strPager += "@" + oUserAt.GetName(Int32.Parse(strAt));
                        lblMobileEmail.Text = "<a href=\"mailto:" + strPager + "\" target=\"_blank\">" + strPager + "</a>";
                    }
                    else
                        lblMobileEmail.Text = "N / A";
                }
                lblRequest.Text = intRequest.ToString();
                if (Request.QueryString["comm"] != null && Request.QueryString["comm"] != "")
                    trCommunication.Visible = true;
                if (boolTPM == true)
                {
                    //                if (CheckConfigured(intProject, intRequest, intResource) == true)
                    //                {
                    panControl.Visible = true;
                    Control oControl = (Control)LoadControl(strTPMWM);
                    phControl.Controls.Add(oControl);
                    LoadProject(intProject);
                    //                }
                    //                else
                    //                    panConfigure.Visible = true;
                }
                else if (boolCoordinator == true)
                {
                    panControl.Visible = true;
                    Control oControl = (Control)LoadControl(strPCWM);
                    phControl.Controls.Add(oControl);
                    LoadProject(intProject);
                }
                else if (oApplication.GetName(intApplication).Contains("IDC") == true || oApplication.GetName(intApplication).Contains("Integration Engineer") == true)
                {
                    panControl.Visible = true;
                    Control oControl = (Control)LoadControl("/controls/wm/wm_ie.ascx");
                    phControl.Controls.Add(oControl);
                    LoadProject(intProject);
                }
                else
                {
                    panWorkload.Visible = true;
                    ds = oResourceRequest.GetWorkflowProjectAll(intProject);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds = oProjectRequest.GetProject(intProject);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            btnViewPR.Text = "View Original Project Request";
                            btnViewPR.Attributes.Add("onclick", "return OpenWindow('PRINTER_FRIENDLY','?page=" + intViewRequest.ToString() + "&rid=" + intRequest.ToString() + "');");
                        }
                        else
                        {
                            //if (intResource > 0)
                            //{
                            //    btnViewPR.Text = "View Original Request Details";
                            //    btnViewPR.Attributes.Add("onclick", "return OpenWindow('PRINTER_FRIENDLY','?page=" + intViewResourceRequest.ToString() + "&rrid=" + intResource.ToString() + "');");
                            //}
                            //else
                            btnViewPR.Visible = false;
                        }
                        // Documents
                        btnDocuments.Attributes.Add("onclick", "return OpenWindow('DOCUMENTS_SECURE','?pid=" + intProject.ToString() + "');");
                        chkMyDescription.Checked = (Request.QueryString["mydoc"] != null);
                        lblMyDocuments.Text = oDocument.GetDocuments_Mine(intProject, intProfile, oVariable.DocumentsFolder(), -1, (Request.QueryString["mydoc"] != null));
                        // GetDocuments(string _physical, int _projectid, int _requestid, int _userid, int _security, bool _show_description, bool _mine)
                        //lblMyDocuments.Text = oDocument.GetDocuments(Request.PhysicalApplicationPath, intProject, 0, intProfile, -1, (Request.QueryString["mydoc"] != null), true);
                        chkDescription.Checked = (Request.QueryString["doc"] != null);
                        lblDocuments.Text = oDocument.GetDocuments_Project(intProject, intProfile, oVariable.DocumentsFolder(), 1, (Request.QueryString["doc"] != null));
                        // GetDocuments(string _physical, int _projectid, int _requestid, int _userid, int _security, bool _show_description, bool _mine)
                        //lblDocuments.Text = oDocument.GetDocuments(Request.PhysicalApplicationPath, intProject, 0, intProfile, 1, (Request.QueryString["doc"] != null), false);

                        /*
                        ds = oResourceRequest.GetWorkflowProject(intProject);
                        int intOldUser = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (intOldUser == Int32.Parse(dr["userid"].ToString()))
                                dr.Delete();
                            else
                                intOldUser = Int32.Parse(dr["userid"].ToString());
                        }
                        ddlResource.DataValueField = "userid";
                        ddlResource.DataTextField = "userid";
                        ddlResource.DataSource = ds;
                        ddlResource.DataBind();
                        foreach (ListItem oItem in ddlResource.Items)
                            oItem.Text = oUser.GetFullName(Int32.Parse(oItem.Value));
                        ddlResource.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        // Load Involvement
                        DataSet dsInvolvement = oResourceRequest.GetWorkflowProject(intProject);
                        int intOldItem = 0;
                        intOldUser = 0;
                        foreach (DataRow dr in dsInvolvement.Tables[0].Rows)
                        {
                            if (intImplementorDistributed == Int32.Parse(dr["itemid"].ToString()))
                                dr.Delete();
                            else if (intImplementorMidrange == Int32.Parse(dr["itemid"].ToString()))
                                dr.Delete();
                            else if (intOldItem == Int32.Parse(dr["itemid"].ToString()) && intOldUser == Int32.Parse(dr["userid"].ToString()))
                                dr.Delete();
                            else
                            {
                                intOldItem = Int32.Parse(dr["itemid"].ToString());
                                intOldUser = Int32.Parse(dr["userid"].ToString());
                            }
                        }
                        rptInvolvement.DataSource = dsInvolvement;
                        rptInvolvement.DataBind();
                        lblNoInvolvement.Visible = (rptInvolvement.Items.Count == 0);
                        foreach (RepeaterItem ri in rptInvolvement.Items)
                        {
                            Label _id = (Label)ri.FindControl("lblId");
                            Label _user = (Label)ri.FindControl("lblUser");
                            Label _status = (Label)ri.FindControl("lblStatus");
                            int intStatus = Int32.Parse(_status.Text);
                            int intUser = Int32.Parse(_user.Text);
                            _user.Text = oUser.GetFullName(intUser);
                            Label _item = (Label)ri.FindControl("lblItem");
                            int intItem = Int32.Parse(_item.Text);
                            Label _allocated = (Label)ri.FindControl("lblAllocated");
                            Label _used = (Label)ri.FindControl("lblUsed");
                            double dblAllocated = oResourceRequest.GetAllocated(intProject, intUser, intItem);
                            double dblUsed = oResourceRequest.GetUsed(intProject, intUser, intItem);
                            Label _percent = (Label)ri.FindControl("lblPercent");
                            _allocated.Text = dblAllocated.ToString();
                            _used.Text = dblUsed.ToString();
                            if (dblAllocated > 0)
                            {
                                dblUsed = dblUsed / dblAllocated;
                                _percent.Text = dblUsed.ToString("P");
                            }
                            else
                                _percent.Text = dblAllocated.ToString("P");
                            if (intItem == 0)
                                _item.Text = "Project Coordinator";
                            else
                            {
                                if (intItem == -1)
                                    _item.Text = "Design Implementor";
                                else
                                {
                                    int intApp = oRequestItem.GetItemApplication(intItem);
                                    _item.Text = oApplication.GetName(intApp);
                                }
                            }
                            _status.Text = oStatus.Name(intStatus);
                        }
                        */
                        // MY Involvement
                        DataSet dsMine = oResourceRequest.GetWorkflowProject(intProject);
                        //Check if new request
                        DataColumn oColumn;
                        oColumn = new DataColumn();
                        oColumn.DataType = System.Type.GetType("System.String");
                        oColumn.ColumnName = "new";
                        dsMine.Tables[0].Columns.Add(oColumn);
                        foreach (DataRow dr in dsMine.Tables[0].Rows)
                        {
                            if (intImplementorDistributed == Int32.Parse(dr["itemid"].ToString()))
                                dr.Delete();
                            else if (intImplementorMidrange == Int32.Parse(dr["itemid"].ToString()))
                                dr.Delete();
                            else if (DateTime.Parse(dr["created"].ToString()) <= DateTime.Parse(dr["modified"].ToString()))
                                dr["new"] = "1";
                            //else if (intProfile != Int32.Parse(dr["userid"].ToString()) && oDelegate.Get(Int32.Parse(dr["userid"].ToString()), intProfile) <= 0)
                            //    dr.Delete();

                             
                        }
                        Functions oFunction = new Functions(0, dsn, intEnvironment);
                        rptMine.DataSource = dsMine;
                        rptMine.DataBind();
                        lblNoMine.Visible = (rptMine.Items.Count == 0);
                        foreach (RepeaterItem ri in rptMine.Items)
                        {
                            Label _id = (Label)ri.FindControl("lblId");
                            Label _user = (Label)ri.FindControl("lblUser");
                            Label _status = (Label)ri.FindControl("lblStatus");
                            Label _color = (Label)ri.FindControl("lblColor");
                            Label _service = (Label)ri.FindControl("lblServiceId");
                            int _serviceid = Int32.Parse(_service.Text);
                            Label _name = (Label)ri.FindControl("lblName");
                            Image imgDelegate = (Image)ri.FindControl("imgDelegate");
                            int intStatus = Int32.Parse(_status.Text);
                            int intUser = Int32.Parse(_user.Text);
                            if ((intStatus < 1 || intStatus > 2) && intStatus != 5)
                                ri.Visible = false;
                            else if (intUser != intProfile)
                            {
                                if (oDelegate.Get(intUser, intProfile) <= 0)
                                    ri.Visible = false;
                                else
                                    imgDelegate.Visible = true;
                            }
                            if (ri.Visible == true)
                            {
                                string strColor = _color.Text;
                                int intGreen = 0;
                                Int32.TryParse(strColor.Substring(0, strColor.IndexOf("_")), out intGreen);
                                strColor = strColor.Substring(strColor.IndexOf("_") + 1);
                                int intYellow = 0;
                                Int32.TryParse(strColor.Substring(0, strColor.IndexOf("_")), out intYellow);
                                strColor = strColor.Substring(strColor.IndexOf("_") + 1);
                                int intRed = 0;
                                Int32.TryParse(strColor, out intRed);
                                strColor = "<table cellpadding=\"0\" cellspacing=\"2\" border=\"0\">";
                                if (intRed > 0)
                                    strColor += "<tr><td>" + oFunction.GetBox("FF0000", 15, 8) + "</td>" + (intRed > 1 ? "<td> (" + intRed.ToString() + ")</td>" : "") + "</tr>";
                                if (intYellow > 0)
                                    strColor += "<tr><td>" + oFunction.GetBox("FFFF00", 15, 8) + "</td>" + (intYellow > 1 ? "<td> (" + intYellow.ToString() + ")</td>" : "") + "</tr>";
                                if (intGreen > 0)
                                    strColor += "<tr><td>" + oFunction.GetBox("00FF00", 15, 8) + "</td>" + (intGreen > 1 ? "<td> (" + intGreen.ToString() + ")</td>" : "") + "</tr>";
                                strColor += "</table>";
                                _color.Text = strColor;
                                
                                if (_name.Text == "")
                                    _name.Text = oService.GetName(_serviceid);
                                Label _item = (Label)ri.FindControl("lblItem");
                                Label _allocated = (Label)ri.FindControl("lblAllocated");
                                double dblAllocated = double.Parse(_allocated.Text);
                                Label _used = (Label)ri.FindControl("lblUsed");
                                double dblUsed = oResourceRequest.GetWorkflowUsed(Int32.Parse(_id.Text));
                                Label _percent = (Label)ri.FindControl("lblPercent");
                                int intItem2 = Int32.Parse(_item.Text);
                                if (intItem2 == 0)
                                    _item.Text = "Project Coordinator";
                                else
                                {
                                    if (intItem2 == -1)
                                        _item.Text = "Pending Execution";
                                    else
                                    {
                                        int intApp = oRequestItem.GetItemApplication(intItem2);
                                        _item.Text = oApplication.GetName(intApp);
                                    }
                                }
                                _allocated.Text = dblAllocated.ToString();
                                _used.Text = dblUsed.ToString();
                                if (dblAllocated > 0)
                                {
                                    dblUsed = dblUsed / dblAllocated;
                                    _percent.Text = dblUsed.ToString("P");
                                }
                                else
                                    _percent.Text = dblAllocated.ToString("P");
                                _status.Text = oStatus.Name(intStatus);
                            }
                        }
                    }
                    else
                    {
                        panDenied.Visible = true;
                        lblTitle.Text = "Zero Dataset ";
                    }
                    LoadProject(intProject);
                }
            }
            else
            {
                panDenied.Visible = true;
                lblTitle.Text = "Invalid Querystring ";
            }
            btnDenied.Attributes.Add("onclick", "return CloseWindow();");
            btnConfigure.Attributes.Add("onclick", "return ValidateText('" + txtNumber.ClientID + "','Please enter the project number');");
        }
        protected bool CheckConfigured(int _projectid, int _requestid, int _resourceid)
        {
            lblNumber.Text = oProject.Get(_projectid, "number");
            txtNumber.Text = lblNumber.Text;
            if (lblNumber.Text == "")
            {
                txtNumber.Visible = true;
                panLink.Visible = true;
                hypNumber.NavigateUrl = "javascript:void(0);";
                hypNumber.Attributes.Add("onclick", "return OpenWindow('CLARITY_NUMBER','');");
                DataSet ds2 = oResourceRequest.Get(_resourceid);
                if (ds2.Tables[0].Rows[0]["solo"].ToString() == "0")
                {
                    btnView.Text = "View Original Project Request";
                    btnView.Attributes.Add("onclick", "return OpenWindow('PRINTER_FRIENDLY','?page=" + intViewRequest.ToString() + "&rid=" + _requestid.ToString() + "');");
                }
                else
                {
                    btnView.Text = "View Original Request Details";
                    btnView.Attributes.Add("onclick", "return OpenWindow('PRINTER_FRIENDLY','?page=" + intViewResourceRequest.ToString() + "&rrid=" + _resourceid.ToString() + "');");
                }
                strProjectSummary = oProject.GetBody(_projectid, intEnvironment, false);
                return false;
            }
            else
            {
                lblNumber.Visible = true;
                return true;
            }
        }
        protected void LoadProject(int intProject)
        {
            strSummary = oProject.GetBody(intProject, intEnvironment, false);
            if (Request.QueryString["div"] != null)
            {
                switch (Request.QueryString["div"])
                {
                    case "D":
                        boolDocuments = true;
                        break;
                    case "M":
                        boolMyDocuments = true;
                        break;
                    case "R":
                        boolResource = true;
                        break;
                }
            }
            if (boolMine == false && boolMyDocuments == false && boolDocuments == false && boolResource == false)
                boolMine = true;
        }
        protected void btnConfigure_Click(Object Sender, EventArgs e)
        {
            int intProject = Int32.Parse(lblProject.Text);
            string strNumber = "";
            if (txtNumber.Visible == true)
            {
                strNumber = txtNumber.Text.ToUpper();
                ds = oProject.Get(strNumber);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblDuplicate.Visible = true;
                    return;
                }
            }
            else
                strNumber = lblNumber.Text.ToUpper();
            oProject.Update(intProject, strNumber);
            Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString());
        }
        protected void chkMyDescription_Change(Object Sender, EventArgs e)
        {
            if (chkMyDescription.Checked == true)
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&mydoc=true&div=M");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&div=M");
        }
        protected void chkDescription_Change(Object Sender, EventArgs e)
        {
            if (chkDescription.Checked == true)
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&doc=true&div=D");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&div=D");
        }
        protected void btnMessage_Click(Object Sender, EventArgs e)
        {
            int intProject = Int32.Parse(lblProject.Text);
            int intUser = Int32.Parse(ddlResource.SelectedItem.Value);
            Functions oFunction = new Functions(intProfile, dsn, intEnvironment);
            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
            if (ddlCommunication.SelectedItem.Value.ToUpper() == "EMAIL")
            {
                string strEmail = oUser.GetName(intUser);
                oFunction.SendEmail("", strEmail, oUser.GetName(intProfile), strEMailIdsBCC, "ClearView Communication from " + oUser.GetFullName(intProfile), oProject.GetBody(intProject, intEnvironment, false) + "<table width=\"100%\" border=\"0\" cellpadding=\"2\" cellspacing=\"1\"><tr><td><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr></table>" + txtMessage.Text, true, false);
            }
            else
            {
                string strPager = oUser.Get(intUser, "pager") + "@archwireless.net";
                oFunction.SendEmail("", strPager, oUser.GetName(intProfile), strEMailIdsBCC, "ClearView Communication from " + oUser.GetFullName(intProfile), txtMessage.Text, false, true);
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString() + "&comm=sent&div=R");
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intMyWork));
        }
    }
}