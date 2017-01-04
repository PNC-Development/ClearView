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
using System.Text;
namespace NCC.ClearView.Presentation.Web
{
    public partial class wm_storage_san : System.Web.UI.UserControl
    {

        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intMyWork = Int32.Parse(ConfigurationManager.AppSettings["MyWork"]);
        protected int intServiceDistributed = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_SERVER_GROWTH_DISTRIBUTED"]);
        protected int intServiceMidrange = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_SERVER_GROWTH_MIDRANGE"]);
        protected int intProfile;
        protected Projects oProject;
        protected Functions oFunction;
        protected Users oUser;
        protected Pages oPage;
        protected ResourceRequest oResourceRequest;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected Services oService;
        protected ServiceRequests oServiceRequest;
        protected RequestFields oRequestField;
        protected Applications oApplication;
        protected ServiceDetails oServiceDetail;
        protected Delegates oDelegate;
        protected Customized oCustomized;
        protected Variables oVariable;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected Locations oLocation;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolDetails = false;
        protected bool boolExecution = false;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intNumber = 0;
        protected int intService = 0;
        protected string strDetails = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oProject = new Projects(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oUser = new Users(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oRequestField = new RequestFields(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oServiceDetail = new ServiceDetails(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
            {
                // Start Workflow Change
                lblResourceWorkflow.Text = Request.QueryString["rrid"];
                int intResourceWorkflow = Int32.Parse(Request.QueryString["rrid"]);
                int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                ds = oResourceRequest.Get(intResourceParent);
                // End Workflow Change
                intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                lblRequestedBy.Text = oUser.GetFullName(oRequest.GetUser(intRequest));
                lblRequestedOn.Text = DateTime.Parse(oResourceRequest.Get(intResourceParent, "created")).ToString();
                intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                // Start Workflow Change
                bool boolComplete = (oResourceRequest.GetWorkflow(intResourceWorkflow, "status") == "3");
                int intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                txtCustom.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "name");
                double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                // End Workflow Change
                intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                lblService.Text = oService.Get(intService, "name");
                int intApp = oRequestItem.GetItemApplication(intItem);

                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Status Updates has been Added');<" + "/" + "script>");
                if (!IsPostBack)
                {
                    double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                    lblUpdated.Text = oResourceRequest.GetUpdated(intResourceWorkflow);
                    if (dblAllocated == dblUsed)
                    {
                        if (boolComplete == false)
                        {
                            oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                            btnComplete.Attributes.Add("onclick", "return confirm('Are you sure you want to mark this as completed and remove it from your workload?');");
                        }
                        else
                        {
                            oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                            btnComplete.Attributes.Add("onclick", "alert('This task has already been marked COMPLETE. You can close this window.');return false;");
                        }
                        panComplete.Visible = true;
                    }
                    else
                    {
                        btnComplete.ImageUrl = "/images/tool_complete_dbl.gif";
                        btnComplete.Enabled = false;
                    }
                    dblUsed = (dblUsed / dblAllocated) * 100;
                    intProject = oRequest.GetProjectNumber(intRequest);
                    hdnTab.Value = "D";
                    panWorkload.Visible = true;
                    LoadStatus(intResourceWorkflow);
                    LoadInformation(intResourceWorkflow);
                    LoadDetails();

                    btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                    oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                    oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                    btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                    oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                    btnClose.Attributes.Add("onclick", "return ExitWindow();");
                    btnSave.Attributes.Add("onclick", "return ValidateNumber0('" + txtAmount.ClientID + "','Please enter the total amount of storage')" +
                        " && ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "')" +
                        ";");
                    // 6/1/2009 - Load ReadOnly View
                    if (oResourceRequest.CanUpdate(intProfile, intResourceWorkflow, false) == false)
                    {
                        oFunction.ConfigureToolButtonRO(btnSave, btnComplete);
                        //panDenied.Visible = true;
                    }
                }
            }
            else
                panDenied.Visible = true;
        }
        private void LoadInformation(int _request)
        {
            if (intProject > 0)
            {
                lblName.Text = oProject.Get(intProject, "name");
                lblNumber.Text = oProject.Get(intProject, "number");
                lblType.Text = "Project";
            }
            else
            {
                lblName.Text = oResourceRequest.GetWorkflow(_request, "name");
                lblNumber.Text = "CVT" + intRequest.ToString();
                lblType.Text = "Task";
            }
            if (Request.QueryString["div"] != null)
            {
                switch (Request.QueryString["div"])
                {
                    case "E":
                        boolExecution = true;
                        break;
                }
            }
            if (boolDetails == false && boolExecution == false)
                boolDetails = true;
        }

        private void LoadStatus(int _resourceid)
        {
            DataSet dsStatus = oResourceRequest.GetStatuss(_resourceid);
            rptStatus.DataSource = dsStatus;
            rptStatus.DataBind();
            lblNoStatus.Visible = (rptStatus.Items.Count == 0);
            double dblTotalStatus = 0.00;
            foreach (RepeaterItem ri in rptStatus.Items)
            {
                Label _status = (Label)ri.FindControl("lblStatus");
                double dblStatus = double.Parse(_status.Text);
                if (dblTotalStatus == 0.00)
                    dblTotalStatus = dblStatus;
                _status.Text = oResourceRequest.GetStatus(dblStatus, 50, 15);
            }
            lblStatus.Text = oResourceRequest.GetStatus(dblTotalStatus, 50, 15);
        }


        private void LoadDetails()
        {
            StringBuilder sb = new StringBuilder(strDetails);

            DataSet ds = oCustomized.GetStorage3rd(intRequest, intItem, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intUser = oRequest.GetUser(intRequest);

                sb.Append("<tr><td colspan=\"2\">Name of Requestor:</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(oUser.GetFullName(intUser));
                sb.Append(" (");
                sb.Append(oUser.GetName(intUser));
                sb.Append(")");
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">Statement of Work:</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["description"].ToString());
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">Server Name:</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["servername"].ToString());
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">Is this device already SAN Connected:</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["currently"].ToString());
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">What type of device is this:</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["type"].ToString());
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">What class is this device in:</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(oClass.Get(Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString()), "name"));
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">What environment is this device in:</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(oEnvironment.Get(Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString()), "name"));
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">What location is this device in:</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(oLocation.GetFull(Int32.Parse(ds.Tables[0].Rows[0]["addressid"].ToString())));
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">What fabric is the device on:</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["fabric"].ToString());
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">Is this device being replicated:</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["replicated"].ToString());
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">Please select a type of storage:</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["shared"].ToString());
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">Do you want to expand a LUN or add an additional LUN:</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["expand"].ToString());
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">Additional Total Capacity Needed (in GB):</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(double.Parse(ds.Tables[0].Rows[0]["amount"].ToString()).ToString("F"));
                sb.Append("</td></tr>");
                
                char[] strSplit = { ',' };
                string[] strLUNs = ds.Tables[0].Rows[0]["luns"].ToString().Split(strSplit);
                string strLUN = "";
                
                for (int ii = 0; ii < strLUNs.Length; ii++)
                {
                    if (strLUNs[ii].Trim() != "")
                    {
                        strLUN += strLUNs[ii].Trim() + "<br/>";
                    }
                }

                sb.Append("<tr><td colspan=\"2\">Please enter the LUN drive and UID, followed by the amount of storage you want to have added to that LUN (additional capacity only) (in GB):</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(strLUN);
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">Enter the World Wide Port names:</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["www"].ToString());
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">UID:</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["uid"].ToString());
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">Select the Storage Performance Tier:</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["performance"].ToString());
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">Clustered NODE Server Names:</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["node"].ToString());
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">Enclosure Name (if blade):</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["encname"].ToString());
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">Enclosure Slot (if blade):</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["encslot"].ToString());
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">Server name of DR device (if replicated):</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["repservername"].ToString());
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">World Wide Port names of the DR device (if replicated):</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["repwww"].ToString());
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">Enclosure Name (if blade and replicated):</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["repencname"].ToString());
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=\"2\">Enclosure Slot (if blade and replicated):</td></tr>");
                sb.Append("<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"30\" height=\"1\" /></td><td width=\"100%\">");
                sb.Append(ds.Tables[0].Rows[0]["repencslot"].ToString());
                sb.Append("</td></tr>");
                
                txtAmount.Text = double.Parse(ds.Tables[0].Rows[0]["allocated"].ToString()).ToString("F");
                lblMidrange.Text = ds.Tables[0].Rows[0]["midrange"].ToString();
            }

            if (sb.ToString() != "")
            {
                sb.Insert(0, "<table width=\"100%\" border=\"0\" cellSpacing=\"2\" cellPadding=\"4\" class=\"default\">");
                sb.Append("</table>");
            }

            strDetails = sb.ToString();
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);

            if (ddlStatus.SelectedIndex > -1 && txtComments.Text.Trim() != "")
            {
                oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text, intProfile);
                //CVT62149 Workload Manager Red Light Status =Hold
                if (ddlStatus.SelectedValue == "1") //Red
                    oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 5, true);
                else
                    oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 2, true);
            }


            double dblHours = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
            double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
            dblHours = (dblHours - dblUsed);
            if (dblHours > 0.00)
                oResourceRequest.UpdateWorkflowHours(intResourceWorkflow, dblHours);
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            oCustomized.UpdateStorage3rd(intRequest, intItem, intNumber, double.Parse(txtAmount.Text));
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            // Send to Remediation Team (based off initial midrange flag)
            oCustomized.UpdateStorage3rd(intRequest, intItem, intNumber, double.Parse(txtAmount.Text));
            int intNewService = intServiceDistributed;
            if (lblMidrange.Text == "1")
                intNewService = intServiceMidrange;
            int intNewItem = oService.GetItemId(intNewService);
            int intNewNumber = oResourceRequest.GetNumber(intRequest, intNewItem);
            oCustomized.UpdateStorage3rd(intRequest, intItem, intNumber, intNewItem, intNewNumber);
            oCustomized.UpdateStorage3rdFlow3(intRequest, intItem, intNumber, intNewItem, intNewNumber);
            int intResource = oServiceRequest.AddRequest(intRequest, intNewItem, intNewService, 0, 0.00, 2, intNewNumber, dsnServiceEditor);
            oServiceRequest.NotifyTeamLead(intNewItem, intResource, intAssignPage, intViewPage, intEnvironment,  "", dsnServiceEditor, dsnAsset, dsnIP, 0);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
    }
}