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

namespace NCC.ClearView.Presentation.Web
{
    public partial class fore_new_cfi : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
        protected int intImplementorDistributedService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrangeService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_IMPLEMENTOR_MIDRANGE"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected string strAdmins = ConfigurationManager.AppSettings["Administrators"];
        protected bool boolAdmin = false;
        protected Pages oPage;
        protected Users oUser;
        protected Projects oProject;
        protected Organizations oOrganization;
        protected Segment oSegment;
        protected ProjectsPending oProjectPending;
        protected Requests oRequest;
        protected StatusLevels oStatusLevel;
        protected Variables oVariable;
        protected Functions oFunction;
        protected Forecast oForecast;
        protected Models oModel;
        protected Types oType;
        protected ModelsProperties oModelsProperties;
        protected Platforms oPlatform;
        protected Confidence oConfidence;
        protected Classes oClass;
        protected Design oDesign;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intID;
        protected string strDiv = "";
        protected string strMenuTab1 = "";
        protected bool boolDemo = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            oOrganization = new Organizations(intProfile, dsn);
            oSegment = new Segment(intProfile, dsn);
            oProjectPending = new ProjectsPending(intProfile, dsn, intEnvironment);
            oRequest = new Requests(intProfile, dsn);
            oStatusLevel = new StatusLevels();
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oForecast = new Forecast(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oConfidence = new Confidence(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oDesign = new Design(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Design Saved');<" + "/" + "script>");
            if (Request.QueryString["project"] != null && Request.QueryString["project"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "projectsaved", "<script type=\"text/javascript\">alert('Project Information Saved');<" + "/" + "script>");
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["delete"] != null && Request.QueryString["delete"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Line Item Deleted');<" + "/" + "script>");
            string[] strProfile;
            char[] strSplit = { ';' };
            strProfile = strAdmins.Split(strSplit);
            for (int ii = 0; ii < strProfile.Length; ii++)
            {
                if (strProfile[ii].Trim() != "")
                {
                    if (Int32.Parse(strProfile[ii].Trim()) == intProfile)
                        boolAdmin = true;
                }
            }
            if (intID > 0)
            {
                if (!IsPostBack)
                {
                    LoadEquipment();
                    if (Request.QueryString["executed"] != null)
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "executed", "<script type=\"text/javascript\">alert('Your design implementor has been notified of your auto-provisioning request.\\n\\nThis resource will call you within the next few minutes to begin your implementation.');<" + "/" + "script>");
                }
            }
            else
            {
                panAllow.Visible = true;
                panPending.Visible = true;
                if (Request.QueryString["new"] != null)
                {
                    panPendingNew.Visible = true;
                    DataSet dsOrgs = oOrganization.Gets(1);
                    ddlOrganization.DataValueField = "organizationid";
                    ddlOrganization.DataTextField = "name";
                    ddlOrganization.DataSource = dsOrgs;
                    ddlOrganization.DataBind();
                    ddlOrganization.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                }
                else
                {
                    if (Request.QueryString["select"] != null)
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "validation", "<script type=\"text/javascript\">alert('Please select a project or choose \"-- PROJECT NOT LISTED --\" to create a new project');<" + "/" + "script>");
                    string strOrder = "namenumber";
                    if (Request.QueryString["order"] != null)
                        strOrder = Request.QueryString["order"];
                    DataSet dsProjs = oProject.GetActive();
                    DataView dv = dsProjs.Tables[0].DefaultView;
                    if (strOrder == "namenumber")
                        btnOrderName.Enabled = false;
                    else if (strOrder == "numbername")
                        btnOrderNumber.Enabled = false;
                    else
                        strOrder = "namenumber";
                    dv.Sort = strOrder;
                    panPendingChoose.Visible = true;
                    lstProjects.DataTextField = strOrder;
                    lstProjects.DataValueField = "projectid";
                    lstProjects.DataSource = dv;
                    lstProjects.DataBind();
                    lstProjects.Items.Insert(0, new ListItem("-- PROJECT NOT LISTED --", "0"));
                    lstProjects.Items[0].Attributes.Add("style", "color:#DD0000");
                    lstProjects.Attributes.Add("ondblclick", "ShowProjectInfo(this);");
                }
            }
            btnDenied.Attributes.Add("onclick", "return CloseWindow();");
            btnView.Attributes.Add("onclick", "return OpenWindow('FORECAST_FILTER','');");
            btnNew.Attributes.Add("onclick", "return OpenWindow('DESIGN_EQUIPMENT','?parent=" + intID.ToString() + "&id=0');");
            btnProjectNew.Attributes.Add("onclick", "return ValidateNewProject('" + txtNumber.ClientID + "','" + txtName.ClientID + "','" + ddlBaseDisc.ClientID + "','" + ddlOrganization.ClientID + "');");
            btnDeleteForecast.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this design?');");
            ddlOrganization.Attributes.Add("onchange", "PopulateSegments('" + ddlOrganization.ClientID + "','" + ddlSegment.ClientID + "');");
            ddlSegment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlSegment.ClientID + "','" + hdnSegment.ClientID + "');");
            txtSearchName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");
            txtSearchNumber.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");
            btnSearch.Attributes.Add("onclick", "SearchTextList('" + txtSearchNumber.ClientID + "','" + txtSearchName.ClientID + "','" + lstProjects.ClientID + "');return false;");
            btnProjectSelect.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            btnProjectNew.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
        }
        private void LoadEquipment()
        {

            //Menu
            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
            Tab oTab = new Tab("", intMenuTab, "divMenu1", true, false);
            //End Menu

            ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
            DataSet ds = oForecast.Get(intID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                panAllow.Visible = true;
                int intUser = Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                txtPNC.Text = ds.Tables[0].Rows[0]["pnc_project"].ToString();
                //if (intProfile == intUser)
                //    btnDeleteForecast.Enabled = true;
                lblRequestor.Text = oUser.GetFullName(intUser);
                lblDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["created"].ToString()).ToLongDateString();
                int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                int intProject = oRequest.GetProjectNumber(intRequest);
                DataSet dsProject = oProject.Get(intProject);
                lblProject.Text = intProject.ToString();
                btnPrint.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','/frame/forecast/forecast_print_forecast.aspx?id=" + intID.ToString() + "');");
                panSelected.Visible = true;
                int intManager = 0;
                int intEngineer = 0;
                int intLead = 0;

                if (intProject > 0)
                {
                    if (dsProject.Tables[0].Rows.Count > 0)
                    {
                        lblName.Text = dsProject.Tables[0].Rows[0]["name"].ToString();
                        lblBaseDisc.Text = dsProject.Tables[0].Rows[0]["bd"].ToString();
                        lblOrganization.Text = oOrganization.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["organization"].ToString()));
                        lblSegment.Text = oSegment.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["segmentid"].ToString()));
                        lblNumber.Text = dsProject.Tables[0].Rows[0]["number"].ToString();
                        if (lblNumber.Text == "")
                        {
                            lblNumber.Text = "<i>To Be Determined...</i>";
                            btnNew.Enabled = false;
                            btnNew.ToolTip = "Project Number Required";
                        }
                        lblStatus.Text = oStatusLevel.HTML(Int32.Parse(dsProject.Tables[0].Rows[0]["status"].ToString()));
                        //intManager = Int32.Parse(dsProject.Tables[0].Rows[0]["lead"].ToString());
                        //intEngineer = Int32.Parse(dsProject.Tables[0].Rows[0]["engineer"].ToString());
                        //intLead = Int32.Parse(dsProject.Tables[0].Rows[0]["technical"].ToString());
                        lblManager.Text = dsProject.Tables[0].Rows[0]["manager"].ToString();
                        // Load Implementor
                        bool boolManualD = false;
                        bool boolManualM = false;
                        //DataSet dsDesigns = oForecast.GetAnswersAccenture(intID);
                        DataSet dsDesigns = oDesign.Gets(intID);
                        foreach (DataRow drDesign in dsDesigns.Tables[0].Rows)
                        {
                            int intDesign = Int32.Parse(drDesign["id"].ToString());
                        }

                        if (strDiv == "")
                        {

                            //strMenuTab1 += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab2',null,null,true);\" class=\"tabheader\">Pending</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
                            oTab.AddTab("Pending", "");
                            strDiv += "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"3\" border=\"0\">";
                            strDiv += "<tr><td colspan=\"2\" align=\"center\" nowrap><img src=\"/frame/picture.aspx?xid=pending\" border=\"0\" align=\"absmiddle\" style=\"height:90px;width:90px;border-width:0px;border:solid 1px #999999;\"/></td></tr>";
                            strDiv += "<tr><td colspan=\"2\" align=\"center\" nowrap><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> <b>No Resources Assigned</b></td></tr>";
                            strDiv += "</table>";
                        }
                    }
                }
                else
                {
                    DataSet dsPending = oProjectPending.GetRequest(intRequest);
                    if (dsPending.Tables[0].Rows.Count > 0)
                    {
                        lblName.Text = dsPending.Tables[0].Rows[0]["name"].ToString();
                        lblBaseDisc.Text = dsPending.Tables[0].Rows[0]["bd"].ToString();
                        lblOrganization.Text = oOrganization.GetName(Int32.Parse(dsPending.Tables[0].Rows[0]["organization"].ToString()));
                        lblNumber.Text = dsPending.Tables[0].Rows[0]["number"].ToString();
                        if (lblNumber.Text == "")
                            lblNumber.Text = "<i>To Be Determined...</i>";
                        lblStatus.Text = "PENDING";
                        lblStatus.CssClass = "pending";
                        //intManager = Int32.Parse(dsPending.Tables[0].Rows[0]["lead"].ToString());
                        //intEngineer = Int32.Parse(dsPending.Tables[0].Rows[0]["engineer"].ToString());
                        //intLead = Int32.Parse(dsPending.Tables[0].Rows[0]["technical"].ToString());
                    }
                    //strMenuTab1 += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab2',null,null,true);\" class=\"tabheader\">Pending</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
                    oTab.AddTab("Pending", "");
                    strDiv += "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"3\" border=\"0\">";
                    strDiv += "<tr><td colspan=\"2\" align=\"center\" nowrap><img src=\"/frame/picture.aspx?xid=pending\" border=\"0\" align=\"absmiddle\" style=\"height:90px;width:90px;border-width:0px;border:solid 1px #999999;\"/></td></tr>";
                    strDiv += "<tr><td colspan=\"2\" align=\"center\" nowrap><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> <b>Awaiting Project Approval</b></td></tr>";
                    strDiv += "</table>";
                }

                // Check to see if Demo
                DataSet dsDemo = oFunction.GetSetupValuesByKey("DEMO_PROJECT");
                foreach (DataRow drDemo in dsDemo.Tables[0].Rows)
                {
                    if (lblNumber.Text == drDemo["Value"].ToString())
                    {
                        boolDemo = true;
                        break;
                    }
                }

                //ds = oForecast.GetAnswersAccenture(intID);
                ds = oDesign.Gets(intID);
                if ((Request.QueryString["c"] != null && Request.QueryString["c"] != "") || (Request.QueryString["e"] != null && Request.QueryString["e"] != ""))
                {
                    string strC = Request.QueryString["c"];
                    if (strC != "")
                    {
                        btnClear.Enabled = true;
                        if (lblFilter.Text != "")
                            lblFilter.Text += ", ";
                        lblFilter.Text += oClass.Get(Int32.Parse(strC), "name");

                    }
                    string strE = Request.QueryString["e"];
                    if (strE != "")
                    {
                        Environments oEnvironments = new Environments(intProfile, dsn);
                        btnClear.Enabled = true;
                        if (lblFilter.Text != "")
                            lblFilter.Text += ", ";
                        lblFilter.Text += oEnvironments.Get(Int32.Parse(strE), "name");

                    }
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (strC != "" && dr["classid"].ToString() != strC)
                            dr.Delete();
                        else if (strE != "" && dr["environmentid"].ToString() != strE)
                            dr.Delete();
                    }
                }
                if (Request.QueryString["f"] != null && Request.QueryString["f"] != "")
                {
                    string strF = Request.QueryString["f"];
                    switch (strF)
                    {
                        case "mine":
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                try
                                {
                                    if (dr["userid"].ToString() != intProfile.ToString())
                                        dr.Delete();
                                }
                                catch { }
                            }
                            break;
                        case "active":
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                try
                                {
                                    if (dr["completed"].ToString() != "")
                                        dr.Delete();
                                }
                                catch { }
                            }
                            break;
                        case "complete":
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                try
                                {
                                    if (dr["completed"].ToString() == "")
                                        dr.Delete();
                                }
                                catch { }
                            }
                            break;
                    }
                    ddlFilter.SelectedValue = strF;
                }
                if (lblFilter.Text == "")
                    lblFilter.Text = "All Environments";
                rptAll.DataSource = ds;
                rptAll.DataBind();
                double dblQT = 0.00;
                double dblAT = 0.00;
                double dblOT = 0.00;
                double dblAmp = 0.00;
                double dblHours = 0.00;
                double dblStorageTotal = 0.00;
                foreach (RepeaterItem ri in rptAll.Items)
                {
                    Label lblId = (Label)ri.FindControl("lblId");
                    int intDesign = 0;
                    if (Int32.TryParse(lblId.Text, out intDesign))
                    {
                        Label lblPlatformId = (Label)ri.FindControl("lblPlatformId");
                        Label lblCommitment = (Label)ri.FindControl("lblCommitment");
                        string strCommitment = "";
                        DateTime datCommitment = DateTime.Now;
                        if (DateTime.TryParse(lblCommitment.Text, out datCommitment) == true)
                            strCommitment = datCommitment.ToShortDateString();
                        bool boolProd = oDesign.IsProd(intDesign);
                        // QUANTITY
                        Label lblQuantity = (Label)ri.FindControl("lblQuantity");
                        double dblQuantity = 0.00;
                        double dblQuantityTotal = 0.00;
                        double.TryParse(lblQuantity.Text, out dblQuantity);
                        dblQuantityTotal = dblQuantity;
                        bool boolDR = false;
                        if (boolProd)
                        {
                            if (oDesign.Get(intDesign, "dr") != "0")
                            {
                                dblQuantityTotal = dblQuantityTotal * 2;
                                boolDR = true;
                            }
                        }
                        //lblQuantity.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/frame/forecast/forecast_print_quantity.aspx?id=" + lblId.Text + "',275,200);\">" + dblQuantity.ToString() + "</a>";
                        lblQuantity.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/frame/design_list.aspx?id=" + lblId.Text + "&type=QUANTITY',275,200);\">" + dblQuantityTotal.ToString() + "</a>";
                        dblQT += dblQuantityTotal;
                        // AMP and MODEL
                        int intModel = 0;
                        int intType = 0;
                        string strModel = "";
                        double dblReplicate = 0.00;
                        Label lblModel = (Label)ri.FindControl("lblModel");
                        Label lblAmp = (Label)ri.FindControl("lblAmp");
                        // If completed, use the model of the asset.  If not, find the model.
                        Label lblComplete = (Label)ri.FindControl("lblComplete");
                        Label lblAbandoned = (Label)ri.FindControl("lblAbandoned");
                        Label lblScheduled = (Label)ri.FindControl("lblScheduled");

                        // PROGRESS BAR
                        Label lblStep = (Label)ri.FindControl("lblStep");
                        double dblStep = double.Parse(lblStep.Text);
                        double dblSteps = double.Parse(oDesign.GetPhases(1).Tables[0].Rows.Count.ToString());
                        double dblComplete = (((dblStep) / dblSteps) * 100);
                        bool boolComplete = false;
                        if (lblComplete.Text != "")
                        {
                            if (lblAbandoned.Text == "")
                            {
                                // Not executed, so abandoned
                                Panel panAbandoned = (Panel)ri.FindControl("panAbandoned");
                                lblAbandoned.Text = "<a href=\"javascript:void(0);\" onclick=\"abandon('" + strCommitment + "','" + lblId.Text + "');\">Abandoned</a><br/>" + DateTime.Parse(lblComplete.Text).ToShortDateString() + "";
                                panAbandoned.Visible = true;
                            }
                            else
                            {
                                // Completed
                                Panel panComplete = (Panel)ri.FindControl("panComplete");
                                lblComplete.Text = "Built on<br/>" + DateTime.Parse(lblComplete.Text).ToShortDateString() + "";
                                panComplete.Visible = true;
                            }
                            intModel = oDesign.GetModelProperty(intDesign);
                            double dblAmpTemp = 0.00;
                            if (intModel > 0)
                                dblAmpTemp = (double.Parse(oModelsProperties.Get(intModel, "amp")) * dblQuantityTotal);
                            lblAmp.Text = dblAmpTemp.ToString("N");
                            dblAmp += dblAmpTemp;
                            strModel = oModelsProperties.Get(intModel, "name");
                            if (intModel > 0)
                                double.TryParse(oModelsProperties.Get(intModel, "replicate_times"), out dblReplicate);
                            if (intModel > 0)
                                intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                            if (intModel > 0)
                                intType = oModel.GetType(intModel);
                            string strPDF = oModel.Get(intModel, "pdf");
                            if (strPDF != "")
                                lblModel.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMAX('" + strPDF.Replace("\\", "\\\\") + "');\">" + strModel + "</a>";
                            else
                                lblModel.Text = strModel;
                            boolComplete = true;
                        }
                        else if (lblScheduled.Text != "")
                        {
                            Panel panScheduled = (Panel)ri.FindControl("panScheduled");
                            string strScheduledDate = DateTime.Parse(lblScheduled.Text).ToShortDateString();
                            string strScheduledTime = DateTime.Parse(lblScheduled.Text).ToShortTimeString();
                            while (strScheduledTime.Contains(" ") == true)
                                strScheduledTime = strScheduledTime.Replace(" ", "&nbsp;");
                            lblScheduled.Text = "Scheduled<br/>" + strScheduledDate + "<br/>" + strScheduledTime;
                            panScheduled.Visible = true;
                        }
                        else
                        {
                            Panel panStep = (Panel)ri.FindControl("panStep");
                            lblStep.Text = oServiceRequest.GetStatusBar(dblComplete, "50", "8", false);
                            panStep.Visible = true;
                        }




                        // Production date                 
                        LinkButton lnkProduction = (LinkButton)ri.FindControl("lnkProduction");


                        // LINKS (Delete, Execute, Etc...)                
                        LinkButton oEdit = (LinkButton)ri.FindControl("btnEdit");
                        oEdit.Attributes.Add("onclick", "return EditForecast('" + oEdit.CommandArgument + "');");
                        LinkButton oDelete = (LinkButton)ri.FindControl("btnDelete");
                        oDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this line item?');");
                        LinkButton oExecute = (LinkButton)ri.FindControl("btnExecute");
                        if (boolComplete == false)
                        {
                            int intServerModel = oDesign.GetModelProperty(intDesign);
                            if (intServerModel > 0)
                            {
                                double.TryParse(oModelsProperties.Get(intServerModel, "replicate_times"), out dblReplicate);
                                double dblAmpTemp = 0.00;
                                double.TryParse(oModelsProperties.Get(intServerModel, "amp"), out dblAmpTemp);
                                if (dblAmpTemp > 0.00)
                                {
                                    dblAmpTemp = dblAmpTemp * dblQuantityTotal;
                                    lblAmp.Text = dblAmpTemp.ToString("N");
                                    dblAmp += dblAmpTemp;
                                }
                                if (intModel == 0)
                                    Int32.TryParse(oModelsProperties.Get(intServerModel, "modelid"), out intModel);
                                strModel = oModelsProperties.Get(intServerModel, "name");
                            }
                            else if (intModel > 0)
                            {
                                strModel = oModel.Get(intModel, "name");
                            }
                            if (intModel == 0)
                            {
                                lblModel.Text = "Solution Unavailable";
                                lblModel.CssClass = "reddefault";
                            }
                            else
                            {
                                if (intModel > 0)
                                    intType = oModel.GetType(intModel);
                                string strPDF = oModel.Get(intModel, "pdf");
                                if (strPDF != "")
                                    lblModel.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMAX('" + strPDF.Replace("\\", "\\\\") + "');\">" + strModel + "</a>";
                                else
                                    lblModel.Text = strModel;
                            }
                        }

                        //if (oDesign.IsOther(intDesign, 1, 0, 0) == true)
                        if (oDesign.GetSubmitted(intDesign).Tables[0].Rows.Count > 0)
                            oEdit.Text = "View";
                        /*
                        string strReady = oDesign.CanExecute(intDesign);
                        if (boolComplete == true || strReady == "")
                        {
                            if (intModel == 0)
                                oExecute.Attributes.Add("onclick", "alert('This design has not found a solution based on the requirements you have provided.\\nPlease verify that your information is correct.\\n\\nIf you have validated this information, please contact your Design Implementor to find an alternative solution.');return false;");
                            else
                            {
                                string strExecute = oType.Get(intType, "design_execution_path");
                                if (strExecute != "")
                                    oExecute.Attributes.Add("onclick", "return OpenWindow('DESIGN_EXECUTE','" + strExecute + "?id=" + lblId.Text + "');");
                                else
                                    oExecute.Attributes.Add("onclick", "alert('Execution has not been configured for asset type " + oType.Get(intType, "name") + "');return false;");
                            }
                        }
                        else
                        {
                            oExecute.Enabled = false;
                            oExecute.ToolTip = strReady;
                            oExecute.Attributes.Add("oncontextmenu", "alert('" + strReady + "');");
                        }
                        */

                        oDelete.Enabled = (boolAdmin || oDelete.CommandName == "");


                        Label lblHours = (Label)ri.FindControl("lblHours");
                        double dblH = 0.00;
                        dblHours += dblH;
                        lblHours.Text = dblH.ToString("N");
                        Label lblAcquisition = (Label)ri.FindControl("lblAcquisition");
                        double dblA = 0.00;
                        DataSet dsA = oForecast.GetAcquisitions(intModel, 1);
                        foreach (DataRow drA in dsA.Tables[0].Rows)
                            dblA += double.Parse(drA["cost"].ToString()) * dblQuantityTotal;
                        dblAT += dblA;
                        //lblAcquisition.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/frame/forecast/forecast_print_acquisition.aspx?id=" + lblId.Text + "',400,300);\">$" + dblA.ToString("N") + "</a>";
                        lblAcquisition.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/frame/design_list.aspx?id=" + lblId.Text + "&type=ACQUISITION',400,450);\">$" + dblA.ToString("N") + "</a>";
                        Label lblOperational = (Label)ri.FindControl("lblOperational");
                        double dblO = 0.00;
                        DataSet dsO = oForecast.GetOperations(intModel, 1);
                        foreach (DataRow drO in dsO.Tables[0].Rows)
                            dblO += double.Parse(drO["cost"].ToString()) * dblQuantityTotal;
                        dblOT += dblO;
                        //lblOperational.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/frame/forecast/forecast_print_operational.aspx?id=" + lblId.Text + "',400,300);\">$" + dblO.ToString("N") + "</a>";
                        lblOperational.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/frame/design_list.aspx?id=" + lblId.Text + "&type=OPERATIONAL',400,450);\">$" + dblO.ToString("N") + "</a>";
                        // STORAGE
                        DataSet dsStorage = oDesign.GetStorageDrives(intDesign);
                        double dblStorage = 0.00;
                        double dblTemp = 0.00;
                        foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
                        {
                            if (double.TryParse(drStorage["size"].ToString(), out dblTemp) == true)
                            {
                                dblStorage += (dblTemp + (boolDR ? dblTemp * dblReplicate : 0.00));
                                dblStorageTotal += dblStorage;
                            }
                        }
                        DataSet dsApp = oDesign.GetStorageDrive(intDesign, -1000);
                        if (dsApp.Tables[0].Rows.Count > 0 && oDesign.IsWindows(intDesign))
                        {
                            if (double.TryParse(dsApp.Tables[0].Rows[0]["size"].ToString(), out dblTemp) == true)
                            {
                                dblStorage += (dblTemp + (boolDR ? dblTemp * dblReplicate : 0.00));
                                dblStorageTotal += dblStorage;
                            }
                        }
                        dblStorage = dblStorage * dblQuantity;
                        /*
                        DataSet dsStorage = oForecast.GetStorage(intDesign);
                        double dblStorage = 0.00;
                        if (dsStorage.Tables[0].Rows.Count > 0)
                        {
                            double dblHigh = double.Parse(dsStorage.Tables[0].Rows[0]["high_total"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["high_qa"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["high_test"].ToString()) + (double.Parse(dsStorage.Tables[0].Rows[0]["high_replicated"].ToString()) * dblReplicate) + double.Parse(dsStorage.Tables[0].Rows[0]["high_ha"].ToString());
                            double dblStandard = double.Parse(dsStorage.Tables[0].Rows[0]["standard_total"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["standard_qa"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["standard_test"].ToString()) + (double.Parse(dsStorage.Tables[0].Rows[0]["standard_replicated"].ToString()) * dblReplicate) + double.Parse(dsStorage.Tables[0].Rows[0]["standard_ha"].ToString());
                            double dblLow = double.Parse(dsStorage.Tables[0].Rows[0]["low_total"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["low_qa"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["low_test"].ToString()) + (double.Parse(dsStorage.Tables[0].Rows[0]["low_replicated"].ToString()) * dblReplicate) + double.Parse(dsStorage.Tables[0].Rows[0]["low_ha"].ToString());
                            //if (dsStorage.Tables[0].Rows[0]["high_level"].ToString().ToUpper() == "HIGH")
                            //    dblHigh = dblHigh * 2;
                            //if (dsStorage.Tables[0].Rows[0]["standard_level"].ToString().ToUpper() == "HIGH")
                            //    dblStandard = dblStandard * 2;
                            //if (dsStorage.Tables[0].Rows[0]["low_level"].ToString().ToUpper() == "HIGH")
                            //    dblLow = dblLow * 2;
                            dblStorage = dblHigh + dblStandard + dblLow;
                            dblStorageTotal += dblStorage;
                        }
                        */
                        Label lblStorage = (Label)ri.FindControl("lblStorage");
                        //lblStorage.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/frame/forecast/forecast_print_storage.aspx?id=" + lblId.Text + "',650,200);\">" + dblStorage.ToString() + " GB</a>";
                        lblStorage.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/frame/design_list.aspx?id=" + lblId.Text + "&type=STORAGE_AMOUNT',650,200);\">" + dblStorage.ToString() + " GB</a>";
                    }
                }
                lblQuantityTotal.Text = dblQT.ToString();
                lblStorageTotal.Text = dblStorageTotal.ToString() + " GB";
                lblAmpTotal.Text = dblAmp.ToString("N");
                lblHoursTotal.Text = dblHours.ToString("N");
                lblAcquisitionTotal.Text = "$" + dblAT.ToString("N");
                lblOperationalTotal.Text = "$" + dblOT.ToString("N");
                lblNone.Visible = (rptAll.Items.Count == 0);
            }
            else
                panDenied.Visible = true;

            strMenuTab1 = oTab.GetTabs();

        }
        //public DataSet Get(int _id)
        //{
        //    SqlParameter[] arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@id", _id);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_accenture WHERE id = @id AND deleted = 0", arParams);
        //}
        //public string Get(int _id, string _column)
        //{
        //    DataSet ds = Get(_id);
        //    if (ds.Tables[0].Rows.Count > 0)
        //        return ds.Tables[0].Rows[0][_column].ToString();
        //    else
        //        return "";
        //}
        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            Response.Redirect(oPage.GetFullLink(intPage) + "?order=" + oButton.CommandArgument);
        }
        protected void btnProjectSelect_Click(Object Sender, EventArgs e)
        {
            if (lstProjects.SelectedIndex > -1)
            {
                if (lstProjects.SelectedIndex == 0)
                    Response.Redirect(oPage.GetFullLink(intPage) + "?new=true");
                else
                {
                    int intProject = Int32.Parse(lstProjects.SelectedItem.Value);
                    DataSet ds = oForecast.GetProject(intProject);
                    if (ds.Tables[0].Rows.Count > 0)
                        intID = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                    else
                    {
                        int intRequest = oRequest.Add(intProject, intProfile);
                        intID = oForecast.Add(intRequest, "", intProfile);
                    }
                    oForecast.Update(intID, 1);
                    Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intID.ToString());
                }
            }
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?select=false");
        }
        protected void btnProjectNew_Click(Object Sender, EventArgs e)
        {
            bool boolAdd = false;
            if (txtNumber.Text != "")
            {
                DataSet ds = oProject.Get(txtNumber.Text);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int intProject = Int32.Parse(ds.Tables[0].Rows[0]["projectid"].ToString());
                    DataSet dsForecast = oForecast.GetProject(intProject);
                    if (dsForecast.Tables[0].Rows.Count == 0)
                    {
                        int intRequest = oRequest.Add(intProject, intProfile);
                        intID = oForecast.Add(intRequest, "", intProfile);
                    }
                    else
                        intID = Int32.Parse(dsForecast.Tables[0].Rows[0]["id"].ToString());
                    oForecast.Update(intID, 1);
                    Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intID.ToString());
                }
                else
                {
                    if (txtName.Text == "" || ddlBaseDisc.SelectedIndex == 0 || ddlOrganization.SelectedIndex == 0)
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "validation2", "<script type=\"text/javascript\">alert('Invalid Project Number!\\n\\nPlease enter a project name, select a project type, and select a sponsoring portfolio to continue.');<" + "/" + "script>");
                    else
                        boolAdd = true;
                }
            }
            else
                boolAdd = true;
            if (boolAdd == true)
            {
                int intRequest = oRequest.Add(-100, intProfile);
                int intSegment = 0;
                if (Request.Form[hdnSegment.UniqueID] != "")
                    intSegment = Int32.Parse(Request.Form[hdnSegment.UniqueID]);
                oProjectPending.Add(intRequest, txtName.Text, ddlBaseDisc.SelectedItem.Text, txtNumber.Text, intProfile, Int32.Parse(ddlOrganization.SelectedItem.Value), intSegment, 0, "");
                intID = oForecast.Add(intRequest, "", intProfile);
                Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intID.ToString());
            }
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            int intDesign = Int32.Parse(oButton.CommandArgument);
            oDesign.Delete(intDesign);
            /*
            // Delete servers
            Servers oServer = new Servers(0, dsn);
            ServerName oServerName = new ServerName(0, dsn);
            Asset oAsset = new Asset(0, dsnAsset);
            DataSet dsServer = oServer.GetAnswer(intDesign);
            foreach (DataRow drServer in dsServer.Tables[0].Rows)
            {
                int intServer = Int32.Parse(drServer["id"].ToString());
                // Delete Name
                int intName = Int32.Parse(drServer["nameid"].ToString());
                if (drServer["pnc"].ToString() == "1")
                    oServerName.UpdateFactory(intName, 1);
                else
                    oServerName.Update(intName, 1);
                // Delete IPs
                oServer.DeleteIP(intServer, 0, 0, 0, 0, dsnIP);
                // Delete Asset
                if (drServer["assetid"].ToString() != "")
                {
                    int intAsset = Int32.Parse(drServer["assetid"].ToString());
                    oAsset.AddStatus(intAsset, "", (int)AssetStatus.Available, -100, DateTime.Now);
                }
                if (drServer["drid"].ToString() != "")
                {
                    int intAssetDR = Int32.Parse(drServer["drid"].ToString());
                    oAsset.AddStatus(intAssetDR, "", (int)AssetStatus.Available, -100, DateTime.Now);
                }
                oServer.DeleteAsset(intServer);
                // Delete Server
                oServer.Delete(intServer);
            }
            // Delete any resource requests
            OnDemandTasks oOnDemandTask = new OnDemandTasks(intProfile, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(intProfile, dsn);
            DataSet dsResources = oOnDemandTask.GetPending(intDesign);
            foreach (DataRow drResource in dsResources.Tables[0].Rows)
            {
                int intRR = Int32.Parse(drResource["resourceid"].ToString());
                if (oResourceRequest.GetWorkflow(intRR, "status") != "3")
                    oResourceRequest.DeleteWorkflow(intRR);
                oOnDemandTask.DeletePending(intDesign);
            }
            */
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intID.ToString() + "&delete=true");
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            DataSet ds = oForecast.Get(intID);
            oForecast.Update(intID, txtPNC.Text);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intProject = Int32.Parse(lblProject.Text);
                int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                if (intProject > 0)
                    oProject.Update(intProject, Int32.Parse(Request.Form[hdnManager.UniqueID]), 0, 0, Int32.Parse(Request.Form[hdnLead.UniqueID]), Int32.Parse(Request.Form[hdnEngineer.UniqueID]), 0);
                else
                    oProjectPending.Update(intRequest, Int32.Parse(Request.Form[hdnManager.UniqueID]), 0, 0, Int32.Parse(Request.Form[hdnLead.UniqueID]), Int32.Parse(Request.Form[hdnEngineer.UniqueID]), 0);
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intID.ToString() + "&project=true");
        }
        protected void btnDeleteForecast_Click(Object Sender, EventArgs e)
        {
            oForecast.Delete(intID);
            //DataSet ds = oForecast.GetAnswersAccenture(intID);
            DataSet ds = oDesign.Gets(intID);
            OnDemandTasks oOnDemandTask = new OnDemandTasks(intProfile, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(intProfile, dsn);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intAnswer = Int32.Parse(dr["id"].ToString());
                oForecast.DeleteAnswer(intAnswer);
                // Delete any resource requests
                DataSet dsResources = oOnDemandTask.GetPending(intAnswer);
                foreach (DataRow drResource in dsResources.Tables[0].Rows)
                {
                    int intRR = Int32.Parse(drResource["resourceid"].ToString());
                    if (oResourceRequest.GetWorkflow(intRR, "status") != "3")
                        oResourceRequest.DeleteWorkflow(intRR);
                    oOnDemandTask.DeletePending(intAnswer);
                }
            }
            Response.Redirect(oPage.GetFullLink(oPage.GetParent(intPage)));
        }
        protected void btnClear_Click(Object Sender, EventArgs e)
        {
            string strF = "";
            if (Request.QueryString["f"] != null)
                strF = "&f=" + Request.QueryString["f"];
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intID.ToString() + strF);
        }
        protected void btnFilter_Click(Object Sender, EventArgs e)
        {
            string strC = "";
            if (Request.QueryString["c"] != null)
                strC = "&c=" + Request.QueryString["c"];
            string strE = "";
            if (Request.QueryString["e"] != null)
                strE = "&e=" + Request.QueryString["e"];
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intID.ToString() + "&f=" + ddlFilter.SelectedItem.Value + strC + strE);
        }
    }
}