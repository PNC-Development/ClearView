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
using System.IO;
namespace NCC.ClearView.Presentation.Web
{
    public partial class IM_Server_demand : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strQuestion = ConfigurationManager.AppSettings["FORECAST_RAM_QUESTIONS"];
        protected Functions oFunction;
        protected Platforms oPlatform;
        protected Types oType;
        protected ModelsProperties oModelsProperties;
        protected ServiceRequests oServiceRequest;
        protected Forecast oForecast;
        protected Pages oPage;
        protected Locations oLocation;
        protected Classes oClass;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;

        protected string strDemand = "";
        protected string strViews = "";
        protected string strParameters = "";
        protected string strFilterLocation = "";
        protected string strFilterProject = "";
        protected string strFilterClass = "";
        protected string strFilterEnv = "";
        protected string strFilterConfidence = "";
        protected string strFilterModelType = "";
        protected DataTable dtSupplyAndDemand = null;
        protected InventoryManager oIM;
        protected int intGroup = 0;
        int intPlatform = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oIM = new InventoryManager(intProfile, dsn);

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intPlatform = Int32.Parse(Request.QueryString["id"]);

            if (intPlatform > 0)
            {
                if (!IsPostBack)
                {
                    LoadLists();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "loadCurrentTab", "<script type=\"text/javascript\">window.top.LoadCurrentTab();<" + "/" + "script>");
                }

                btnProjects.Attributes.Add("onclick", "return MakeWider(this, '" + lstProjects.ClientID + "');");
                btnProjectsClear.Attributes.Add("onclick", "return ClearList('" + lstProjects.ClientID + "');");
                btnClasses.Attributes.Add("onclick", "return MakeWider(this, '" + lstClasses.ClientID + "');");
                btnClassesClear.Attributes.Add("onclick", "return ClearList('" + lstClasses.ClientID + "');");
                btnConfidences.Attributes.Add("onclick", "return MakeWider(this, '" + lstConfidences.ClientID + "');");
                btnConfidencesClear.Attributes.Add("onclick", "return ClearList('" + lstConfidences.ClientID + "');");
                btnEnvironments.Attributes.Add("onclick", "return MakeWider(this, '" + lstEnvironments.ClientID + "');");
                btnEnvironmentsClear.Attributes.Add("onclick", "return ClearList('" + lstEnvironments.ClientID + "');");
                btnLocations.Attributes.Add("onclick", "return MakeWider(this, '" + lstLocations.ClientID + "');");
                btnLocationsClear.Attributes.Add("onclick", "return ClearList('" + lstLocations.ClientID + "');");
                lstClasses.Attributes.Add("onchange", "PopulateEnvironmentsList('" + lstClasses.ClientID + "','" + lstEnvironments.ClientID + "',0);");
                lstEnvironments.Attributes.Add("onchange", "UpdateListHidden('" + lstEnvironments.ClientID + "','" + hdnEnvironment.ClientID + "');");
                imgStart.Attributes.Add("onclick", "return ShowCalendar('" + txtStart.ClientID + "');");
                imgEnd.Attributes.Add("onclick", "return ShowCalendar('" + txtEnd.ClientID + "');");

                btnGo1.Attributes.Add("onclick", "return ProcessButtons(this) && LoadWait();");
                btnGo2.Attributes.Add("onclick", "return ProcessButtons(this) && LoadWait();");
            }
        }

        protected void LoadLists()
        {
            Projects oProject = new Projects(intProfile, dsn);
            lstProjects.DataValueField = "projectid";
            lstProjects.DataTextField = "name";
            lstProjects.DataSource = oProject.GetActive();
            lstProjects.DataBind();
            lstProjects.Items.Insert(0, new ListItem("-- ALL --", "0"));

            lstLocations.DataValueField = "id";
            lstLocations.DataTextField = "fullname";
            lstLocations.DataSource = oLocation.GetAddresssOrdered(1);
            lstLocations.DataBind();
            lstLocations.Items.Insert(0, new ListItem("-- ALL --", "0"));

            Confidence oConfidence = new Confidence(intProfile, dsn);
            lstConfidences.DataValueField = "id";
            lstConfidences.DataTextField = "name";
            lstConfidences.DataSource = oConfidence.Gets(1);
            lstConfidences.DataBind();
            lstConfidences.Items.Insert(0, new ListItem("-- ALL --", "0"));

            DataSet dsClasses = oClass.Gets(1);
            lstClasses.DataValueField = "id";
            lstClasses.DataTextField = "name";
            lstClasses.DataSource = dsClasses;
            lstClasses.DataBind();
            lstClasses.Items.Insert(0, new ListItem("-- ALL --", "0"));


            lstEnvironments.Items.Insert(0, new ListItem("-- Please select a Class --", "0"));
            lstEnvironments.Enabled = false;


            for (int i = 1; i <= 5; i++)
            {
                DropDownList ddl = this.FindControl("ddlGroup" + i.ToString()) as System.Web.UI.WebControls.DropDownList;
                ddl.Items.Insert(0, new ListItem("Confidence", "ConfidenceId"));
                ddl.Items.Insert(0, new ListItem("Environment", "EnvironmentId"));
                ddl.Items.Insert(0, new ListItem("Class", "ClassId"));
                ddl.Items.Insert(0, new ListItem("Project", "ProjectId"));
                ddl.Items.Insert(0, new ListItem("Location", "LocationId"));
                ddl.Items.Insert(0, new ListItem("-- NONE --", "0"));
            }

        }

        protected void btnGo_Click(Object Sender, EventArgs e)
        {
            string strLocationIds = "";
            string strProjectIds = "";
            string strModelIds = "";
            string strClassIds = "";
            string strEnvironmentIds = "";
            string strConfidenceIds = "";
            string strModelPlatformIds = intPlatform.ToString();
            DateTime? dtFrom = null;
            DateTime? dtTo = null; ;

            int intGrpLocation = 0;
            int intGrpProject = 0;
            int intGrpClass = 0;
            int intGrpEnv = 0;
            int intGrpConfidence = 0;
            int intGrpModelType = 1;

            if (txtStart.Text.Trim() != "") dtFrom = DateTime.Parse(txtStart.Text.Trim());
            if (txtEnd.Text.Trim() != "") dtTo = DateTime.Parse(txtEnd.Text.Trim());

            CheckDuplicateGroups();
            for (int i = 1; i <= 5; i++)
            {
                DropDownList ddl = this.FindControl("ddlGroup" + i.ToString()) as System.Web.UI.WebControls.DropDownList;

                switch (ddl.SelectedValue)
                {
                    case "ProjectId":
                        intGrpProject = 1;
                        break;
                    case "LocationId":
                        intGrpLocation = 1;
                        break;
                    case "ClassId":
                        intGrpClass = 1;
                        break;
                    case "EnvironmentId":
                        intGrpEnv = 1;
                        break;
                    case "ConfidenceId":
                        intGrpConfidence = 1;
                        break;
                }
            }
            LoadFilters(ref strLocationIds, ref strProjectIds, ref strClassIds, ref strEnvironmentIds, ref strConfidenceIds);

            DataSet dsSupplyAndDemand = oIM.GetIMServerDemand
                                (strLocationIds,
                                 strProjectIds,
                                 strModelIds,
                                 strClassIds,
                                 strEnvironmentIds,
                                 strConfidenceIds,
                                 strModelPlatformIds,
                                 dtFrom,
                                 dtTo,
                                 intGrpLocation,
                                 intGrpProject,
                                 intGrpClass,
                                 intGrpEnv,
                                 intGrpConfidence,
                                 intGrpModelType);

            dlDetails.Visible = true;
            dtSupplyAndDemand = dsSupplyAndDemand.Tables[0];

            System.Text.StringBuilder sb = new StringBuilder();
            sb.Append(LoadGroups(1, new ArrayList(), new ArrayList()));

            strDemand = sb.ToString();

            dlDetails.DataSource = null;
            dlDetails.Visible = false;

        }

        private void CheckDuplicateGroups()
        {
            //Check for Duplicate Grouping
            for (int i = 1; i <= 5; i++)
            {
                DropDownList ddl = this.FindControl("ddlGroup" + i.ToString()) as System.Web.UI.WebControls.DropDownList;

                for (int j = i; j <= 5; j++)
                {
                    DropDownList ddlCheckDuplicateGrp = this.FindControl("ddlGroup" + j.ToString()) as System.Web.UI.WebControls.DropDownList;

                    if (i != j && (ddl.SelectedValue.ToString() != "0" && ddl.SelectedValue.ToString() != ""))
                    {
                        if (ddl.SelectedValue.ToString() == ddlCheckDuplicateGrp.SelectedValue.ToString())
                        {
                            if (ddlCheckDuplicateGrp.Enabled == true)
                            {
                                ddlCheckDuplicateGrp.ClearSelection();
                                ddlCheckDuplicateGrp.Items.FindByValue("0").Selected = true;
                            }
                        }
                    }

                }
            }
        }
        private void LoadFilters(ref string strLocationIds,
                           ref string strProjectIds,
                           ref string strClassIds,
                           ref string strEnvironmentIds,
                           ref string strConfidenceIds)
        {
            StringBuilder sb;
            sb = new StringBuilder();
            foreach (ListItem oList in lstLocations.Items)
            {
                if (oList.Selected == true)
                {
                    if (oList.Value == "0") break;

                    if (sb.Length > 0)
                        sb.Append("," + oList.Value);
                    else
                        sb.Append(oList.Value);
                }
            }
            strLocationIds = sb.ToString();

            sb = new StringBuilder();


            foreach (ListItem oList in lstProjects.Items)
            {
                if (oList.Selected == true)
                {
                    if (oList.Value == "0") break;

                    if (sb.Length > 0)
                        sb.Append("," + oList.Value);
                    else
                        sb.Append(oList.Value);
                }
            }
            strProjectIds = sb.ToString();
            sb = new StringBuilder();

            foreach (ListItem oList in lstConfidences.Items)
            {
                if (oList.Selected == true)
                {
                    if (oList.Value == "0") break;

                    if (sb.Length > 0)
                        sb.Append("," + oList.Value);
                    else
                        sb.Append(oList.Value);
                }
            }

            strConfidenceIds = sb.ToString();
            sb = new StringBuilder();

            foreach (ListItem oList in lstClasses.Items)
            {
                if (oList.Selected == true)
                {
                    if (oList.Value == "0") break;

                    if (sb.Length > 0)
                        sb.Append("," + oList.Value);
                    else
                        sb.Append(oList.Value);
                }
            }
            string strClassEnvironments = strClassIds = sb.ToString();
            sb = new StringBuilder();

            // Load Environments
            if (strClassEnvironments == "")
            {
                DataSet dsClass = oClass.Gets(1);
                foreach (DataRow drClass in dsClass.Tables[0].Rows)
                    strClassEnvironments += drClass["id"] + ",";
            }
            lstEnvironments.DataValueField = "id";
            lstEnvironments.DataTextField = "name";
            lstEnvironments.DataSource = oClass.GetEnvironments(oFunction.BuildXmlString("data", strClassEnvironments.Split(new char[] { ',' })), 0);
            lstEnvironments.DataBind();
            lstEnvironments.Items.Insert(0, new ListItem("-- ALL --", "0"));
            lstEnvironments.Enabled = true;

            string strEnvironment = Request.Form[hdnEnvironment.UniqueID];
            string[] strEnvironments;
            strEnvironments = strEnvironment.Split(new char[] { ';' });

            for (int ii = 0; ii < strEnvironments.Length; ii++)
            {
                if (strEnvironments[ii].Trim() != "" && strEnvironments[ii].Trim() != "0")
                {
                    if (sb.Length > 0)
                        sb.Append("," + strEnvironments[ii].Trim());
                    else
                        sb.Append(strEnvironments[ii].Trim());
                }
            }
            strEnvironmentIds = sb.ToString();
            sb = new StringBuilder();

            // Select environment
            for (int ii = 0; ii < strEnvironments.Length; ii++)
            {
                foreach (ListItem li in lstEnvironments.Items)
                {
                    if (strEnvironments[ii].Trim() == li.Value)
                    {
                        li.Selected = true;
                        break;
                    }
                }
            }

        }


        private string LoadGroups(int intGrp, ArrayList arrDistinctCols, ArrayList arrSortCols)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtGroup = null;
            int intNextGrp = intGrp + 1;
            DropDownList ddlGrp = this.FindControl("ddlGroup" + intGrp.ToString()) as System.Web.UI.WebControls.DropDownList;
            DropDownList ddlGrpNext = this.FindControl("ddlGroup" + intNextGrp.ToString()) as System.Web.UI.WebControls.DropDownList;
            string strGrpValue = "";
            string strGrpValueNext = "";


            if (ddlGrp != null)
            {
                if (ddlGrp.SelectedValue != "0")
                    strGrpValue = ddlGrp.SelectedValue;
                else
                    strGrpValue = "ModelTypeId";
            }
            else
            {
                strGrpValue = "ModelTypeId";
            }

            if (ddlGrpNext != null)
            {
                if (ddlGrpNext.SelectedValue != "0")
                    strGrpValueNext = ddlGrpNext.SelectedValue;
                else
                    strGrpValueNext = "";
            }
            else
            {
                strGrpValueNext = "";
            }

            switch (strGrpValue)
            {
                case "ProjectId":
                    arrDistinctCols.Add("ProjectId");
                    arrDistinctCols.Add("ProjectName");
                    arrDistinctCols.Add("ProjectNumber");

                    arrSortCols.Add("ProjectName");

                    dtGroup = GetDistinctWithFilter(arrDistinctCols, arrSortCols);
                    foreach (DataRow dr in dtGroup.Rows)
                    {

                        strFilterProject = (dr["ProjectId"] == DBNull.Value ? " ProjectId IS Null" :
                                        (dr["ProjectId"].ToString() == "" ? "ProjectId =''" : "ProjectId =" + dr["ProjectId"].ToString()));

                        intGroup++;
                        sb.Append("<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("','divGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("');\"><img id=\"imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">");
                        sb.Append(dr["ProjectName"].ToString());
                        sb.Append("</td></tr>");

                        if (strGrpValueNext == "")
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:none\">");
                            sb.Append(LoadGroups(6, arrDistinctCols, arrSortCols));
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:none\">");
                            sb.Append(LoadGroups(intGrp + 1, arrDistinctCols, arrSortCols));
                            sb.Append("</td></tr>");
                        }

                    }

                    arrDistinctCols.Remove("ProjectId");
                    arrDistinctCols.Remove("ProjectName");
                    arrDistinctCols.Remove("ProjectNumber");
                    arrSortCols.Remove("ProjectName");
                    strFilterProject = "";
                    break;
                // Else (next DDL is null) show data

                case "LocationId":

                    arrDistinctCols.Add("LocationId");
                    arrDistinctCols.Add("Location");
                    arrSortCols.Add("Location");

                    dtGroup = GetDistinctWithFilter(arrDistinctCols, arrSortCols);
                    foreach (DataRow dr in dtGroup.Rows)
                    {
                        strFilterLocation = (dr["LocationId"] == DBNull.Value ? " LocationId IS Null" :
                                            (dr["LocationId"].ToString() == "" ? "LocationId =''" : "LocationId =" + dr["LocationId"].ToString()));


                        intGroup++;
                        sb.Append("<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("','divGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("');\"><img id=\"imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("\" src=\"/images/biggerMinus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td class=\"biggerbold\" width=\"100%\">");
                        sb.Append(dr["Location"].ToString());
                        sb.Append("</td></tr>");

                        if (strGrpValueNext == "")
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:inline\">");
                            sb.Append(LoadGroups(6, arrDistinctCols, arrSortCols));
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:inline\">");
                            sb.Append(LoadGroups(intGrp + 1, arrDistinctCols, arrSortCols));
                            sb.Append("</td></tr>");
                        }

                    }
                    arrDistinctCols.Remove("LocationId");
                    arrDistinctCols.Remove("Location");
                    arrSortCols.Remove("Location");
                    strFilterLocation = "";
                    break;

                case "ClassId":
                    arrDistinctCols.Add("ClassId");
                    arrDistinctCols.Add("Class");
                    arrSortCols.Add("Class");

                    dtGroup = GetDistinctWithFilter(arrDistinctCols, arrSortCols);
                    foreach (DataRow dr in dtGroup.Rows)
                    {
                        strFilterClass = (dr["ClassId"] == DBNull.Value ? " ClassId IS Null" :
                                            (dr["ClassId"].ToString() == "" ? "ClassId =''" : "ClassId =" + dr["ClassId"].ToString()));
                        intGroup++;
                        sb.Append("<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("','divGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("');\"><img id=\"imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("\" src=\"/images/biggerMinus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td class=\"biggerbold\" width=\"100%\">");
                        sb.Append(dr["Class"].ToString());
                        sb.Append("</td></tr>");

                        if (strGrpValueNext == "")
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:inline\">");
                            sb.Append(LoadGroups(6, arrDistinctCols, arrSortCols));
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:inline\">");
                            sb.Append(LoadGroups(intGrp + 1, arrDistinctCols, arrSortCols));
                            sb.Append("</td></tr>");
                        }

                    }
                    arrDistinctCols.Remove("ClassId");
                    arrDistinctCols.Remove("Class");
                    arrSortCols.Remove("Class");

                    strFilterClass = "";
                    break;


                case "EnvironmentId":
                    arrDistinctCols.Add("EnvironmentId");
                    arrDistinctCols.Add("Environment");
                    arrSortCols.Add("Environment");
                    dtGroup = GetDistinctWithFilter(arrDistinctCols, arrSortCols);
                    foreach (DataRow dr in dtGroup.Rows)
                    {
                        strFilterEnv = (dr["EnvironmentId"] == DBNull.Value ? " EnvironmentId IS Null" :
                                       (dr["EnvironmentId"].ToString() == "" ? "EnvironmentId =''" : "EnvironmentId =" + dr["EnvironmentId"].ToString()));

                        intGroup++;
                        sb.Append("<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("','divGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("');\"><img id=\"imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("\" src=\"/images/biggerMinus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td class=\"biggerbold\" width=\"100%\">");
                        sb.Append(dr["Environment"].ToString());
                        sb.Append("</td></tr>");

                        if (strGrpValueNext == "")
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:inline\">");
                            sb.Append(LoadGroups(6, arrDistinctCols, arrSortCols));
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:inline\">");
                            sb.Append(LoadGroups(intGrp + 1, arrDistinctCols, arrSortCols));
                            sb.Append("</td></tr>");
                        }

                    }
                    arrDistinctCols.Remove("EnvironmentId");
                    arrDistinctCols.Remove("Environment");
                    arrSortCols.Remove("Environment");
                    strFilterEnv = "";
                    break;
                case "ConfidenceId":
                    arrDistinctCols.Add("ConfidenceId");
                    arrDistinctCols.Add("Confidence");
                    arrSortCols.Add("Confidence");
                    dtGroup = GetDistinctWithFilter(arrDistinctCols, arrSortCols);
                    foreach (DataRow dr in dtGroup.Rows)
                    {
                        strFilterConfidence = (dr["ConfidenceId"] == DBNull.Value ? " ConfidenceId IS Null" :
                                       (dr["ConfidenceId"].ToString() == "" ? "ConfidenceId =''" : "ConfidenceId =" + dr["ConfidenceId"].ToString()));

                        intGroup++;
                        sb.Append("<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("','divGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("');\"><img id=\"imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("\" src=\"/images/biggerMinus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td class=\"biggerbold\" width=\"100%\">");
                        sb.Append(dr["Confidence"].ToString());
                        sb.Append("</td></tr>");

                        if (strGrpValueNext == "")
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:inline\">");
                            sb.Append(LoadGroups(6, arrDistinctCols, arrSortCols));
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:inline\">");
                            sb.Append(LoadGroups(intGrp + 1, arrDistinctCols, arrSortCols));
                            sb.Append("</td></tr>");
                        }

                    }
                    arrDistinctCols.Remove("ConfidenceId");
                    arrDistinctCols.Remove("Confidence");
                    arrSortCols.Remove("Confidence");
                    strFilterConfidence = "";
                    break;

                case "ModelTypeId":
                    arrDistinctCols.Add("ModelTypeId");
                    arrDistinctCols.Add("ModelTypeName");
                    arrDistinctCols.Add("ModelAssetCategoryId");
                    arrDistinctCols.Add("ModelAssetCategory");

                    arrSortCols.Add("ModelTypeName");
                    arrSortCols.Add("ModelAssetCategory");

                    dtGroup = GetDistinctWithFilter(arrDistinctCols, arrSortCols);
                    foreach (DataRow dr in dtGroup.Rows)
                    {
                        strFilterModelType = (dr["ModelTypeId"] == DBNull.Value ? " ModelTypeId IS Null" :
                                    (dr["ModelTypeId"].ToString() == "" ? "ModelTypeId =''" : "ModelTypeId =" + dr["ModelTypeId"].ToString()));

                        strFilterModelType = strFilterModelType + " AND " +
                                    (dr["ModelAssetCategoryId"] == DBNull.Value ? " ModelAssetCategoryId IS Null" :
                                    (dr["ModelAssetCategoryId"].ToString() == "" ? " ModelAssetCategoryId =''" : "ModelAssetCategoryId =" + dr["ModelAssetCategoryId"].ToString()));


                        intGroup++;
                        sb.Append("<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("','divGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("');\"><img id=\"imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("\" src=\"/images/biggerMinus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td class=\"biggerbold\" width=\"100%\">");
                        sb.Append(dr["ModelTypeName"].ToString() + " | " + dr["ModelAssetCategory"].ToString());
                        sb.Append("</td></tr>");

                        sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("\" style=\"display:inline\">");
                        sb.Append(LoadDetails());
                        sb.Append("</td></tr>");


                    }
                    arrDistinctCols.Remove("ModelTypeId");
                    arrDistinctCols.Remove("ModelTypeName");
                    arrDistinctCols.Remove("ModelAssetCategoryId");
                    arrDistinctCols.Remove("ModelAssetCategory");

                    arrSortCols.Remove("ModelTypeName");
                    arrSortCols.Remove("ModelAssetCategory");
                    strFilterModelType = "";
                    break;


            }


            if (sb.ToString() != "")
            {
                return "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\">" + sb.ToString() + "</table>";
            }
            else
            {
                return sb.ToString();
            }

        }

        private DataTable GetDistinctWithFilter(ArrayList arrDistinctCols, ArrayList arrSortCols)
        {
            string[] strColumnNames = (string[])arrDistinctCols.ToArray(typeof(string));
            string strFilters = "";

            string strSort = "";
            for (int i = 0; i <= arrSortCols.Count - 1; i++)
                strSort = (strSort.Length > 0 ? strSort + "," + arrSortCols[i].ToString() : arrSortCols[i].ToString());

            DataTable dtNew = dtSupplyAndDemand.Clone();
            DataRow[] drs = null;

            //apply the filters
            strFilters = strFilters + (strFilters != "" ? (strFilterLocation != "" ? " AND " + strFilterLocation : strFilterLocation) : strFilterLocation);
            strFilters = strFilters + (strFilters != "" ? (strFilterProject != "" ? " AND " + strFilterProject : strFilterProject) : strFilterProject);
            strFilters = strFilters + (strFilters != "" ? (strFilterClass != "" ? " AND " + strFilterClass : strFilterClass) : strFilterClass);
            strFilters = strFilters + (strFilters != "" ? (strFilterEnv != "" ? " AND " + strFilterEnv : strFilterEnv) : strFilterEnv);
            strFilters = strFilters + (strFilters != "" ? (strFilterConfidence != "" ? " AND " + strFilterConfidence : strFilterConfidence) : strFilterConfidence);
            strFilters = strFilters + (strFilters != "" ? (strFilterModelType != "" ? " AND " + strFilterModelType : strFilterModelType) : strFilterModelType);


            drs = dtSupplyAndDemand.Select(strFilters, strSort);

            foreach (DataRow dr in drs)
                dtNew.ImportRow(dr);

            //get the distinct
            return dtNew.DefaultView.ToTable(true, strColumnNames);

        }

        protected string LoadDetails()
        {
            string strFilters = "";

            strFilters = strFilters + (strFilters != "" ? (strFilterLocation != "" ? " AND " + strFilterLocation : strFilterLocation) : strFilterLocation);
            strFilters = strFilters + (strFilters != "" ? (strFilterProject != "" ? " AND " + strFilterProject : strFilterProject) : strFilterProject);
            strFilters = strFilters + (strFilters != "" ? (strFilterClass != "" ? " AND " + strFilterClass : strFilterClass) : strFilterClass);
            strFilters = strFilters + (strFilters != "" ? (strFilterEnv != "" ? " AND " + strFilterEnv : strFilterEnv) : strFilterEnv);
            strFilters = strFilters + (strFilters != "" ? (strFilterConfidence != "" ? " AND " + strFilterConfidence : strFilterConfidence) : strFilterConfidence);
            strFilters = strFilters + (strFilters != "" ? (strFilterModelType != "" ? " AND " + strFilterModelType : strFilterModelType) : strFilterModelType);


            string strSort = "ModelName";
            DataList dl = this.FindControl("dlDetails") as System.Web.UI.WebControls.DataList;

            DataRow[] drSelect = null;
            drSelect = dtSupplyAndDemand.Select(strFilters, strSort);


            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(tw);


            dlDetails.DataSource = drSelect;
            dlDetails.DataBind();
            dlDetails.RenderControl(hw);
            return sb.ToString();

            ////dl.
            ////return this.ParseControl(dl.()).ToString();

            //foreach (DataRow dr in drSelect)
            //{ 


            //}

        }

        protected void dlDetails_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            int intMax = 100;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRow drv = (DataRow)e.Item.DataItem;
                int intDemand = Int32.Parse(drv["ForcastServerCount"].ToString());

                Label lblModel = (Label)e.Item.FindControl("lblModel");
                lblModel.Text = drv["ModelName"].ToString();

                Label lblDemand = (Label)e.Item.FindControl("lblDemand");
                Label lblDemandNo = (Label)e.Item.FindControl("lblDemandNo");

                lblDemandNo.Text = intDemand.ToString();
                lblDemand.Text = oServiceRequest.GetStatusBarFill(((double.Parse(intDemand.ToString()) / double.Parse(intMax.ToString())) * 100.00), "75%", false, "#CC3300");

            }

        }

    }
}