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
    public partial class IM_Storage_demand : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strQuestion = ConfigurationManager.AppSettings["FORECAST_RAM_QUESTIONS"];
        protected Platforms oPlatform;
        protected Types oType;
        protected ModelsProperties oModelsProperties;
        protected ServiceRequests oServiceRequest;
        protected Forecast oForecast;
        protected Pages oPage;
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
        protected DataTable dtSupplyAndDemand=null;
        protected InventoryManager oIM;
        protected Locations oLocation;
        protected int intGroup = 0;
        int intPlatform = 0;
        protected int intMax = 50;
        protected int intMaxPorts = 50;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oIM = new InventoryManager(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);

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
                intMax = Int32.Parse(oPlatform.Get(intPlatform, "max_inventory1"));

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

            //Locations oLocation = new Locations(intProfile, dsn);
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

            Classes oClass = new Classes(intProfile, dsn);
            DataSet dsClasses = oClass.Gets(1);
            lstClasses.DataValueField = "id";
            lstClasses.DataTextField = "name";
            lstClasses.DataSource = dsClasses;
            lstClasses.DataBind();
            lstClasses.Items.Insert(0, new ListItem("-- ALL --", "0"));

            
            lstEnvironments.Items.Insert(0, new ListItem("-- Please select a Class --", "0"));
            lstEnvironments.Enabled = false;
            

            for (int i = 1; i <= 4; i++)
            {
                DropDownList ddl = this.FindControl("ddlGroup" + i.ToString()) as System.Web.UI.WebControls.DropDownList;
                ddl.Items.Insert(0, new ListItem("Confidence", "ConfidenceId"));
                ddl.Items.Insert(0, new ListItem("Environment", "EnvironmentId"));
                ddl.Items.Insert(0, new ListItem("Class", "ClassId"));
                ddl.Items.Insert(0, new ListItem("Project", "ProjectId"));
                ddl.Items.Insert(0, new ListItem("-- NONE --", "0"));
            }

        }

        protected void btnGo_Click(Object Sender, EventArgs e)
        {
            string strLocationIds="";
            string strProjectIds="";
            string strModelIds="";
            string strClassIds="";
            string strEnvironmentIds="";
            string strConfidenceIds="";
            string strModelPlatformIds = intPlatform.ToString();
            DateTime? dtFrom=null;
            DateTime? dtTo = null; ;

            int intGrpLocation=1;
	        int intGrpProject =0;
	        int intGrpClass =0;
	        int intGrpEnv =0;
	        int intGrpConfidence =0;

            if (txtStart.Text.Trim() != "") dtFrom = DateTime.Parse(txtStart.Text.Trim());
            if (txtEnd.Text.Trim() != "") dtTo = DateTime.Parse(txtEnd.Text.Trim());

            CheckDuplicateGroups();
            for (int i = 1; i <= 4; i++)
            {
                DropDownList ddl = this.FindControl("ddlGroup" + i.ToString()) as System.Web.UI.WebControls.DropDownList;

                switch (ddl.SelectedValue)
                {
                    case "ProjectId":
                        intGrpProject = 1;
                        chkIncludeServerGrowthReqs.Checked = false;
                        break;
                    case "ClassId":
                        intGrpClass = 1;
                        chkIncludeServerGrowthReqs.Checked = false;
                        break;
                    case "EnvironmentId":
                        intGrpEnv = 1;
                        chkIncludeServerGrowthReqs.Checked = false;
                        break;
                    case "ConfidenceId":
                        intGrpConfidence = 1;
                        chkIncludeServerGrowthReqs.Checked = false;
                        break;
                }
            }
            LoadFilters(ref strLocationIds, ref strProjectIds, ref strClassIds, ref strEnvironmentIds, ref strConfidenceIds);

            if (strLocationIds!="" || strProjectIds!="" || strClassIds!="" || strEnvironmentIds!="" || strConfidenceIds!="")
                chkIncludeServerGrowthReqs.Checked = false;

            if (chkIncludeServerGrowthReqs.Checked == true)
                panGrowth.Visible = true;
            else
                panGrowth.Visible = false;

            DataSet dsSupplyAndDemand = oIM.GetIMStorageDemand
                                (strLocationIds,
                                 strProjectIds,
                                 strModelIds,
                                 strClassIds,
                                 strEnvironmentIds,
                                 strConfidenceIds,
                                 dtFrom,
                                 dtTo,
                                 intGrpLocation,
                                 intGrpProject,
                                 intGrpClass,
                                 intGrpEnv,
                                 intGrpConfidence,
                                 (chkIncludeServerGrowthReqs.Checked?1:0));

            dtSupplyAndDemand = dsSupplyAndDemand.Tables[0];

            System.Text.StringBuilder sb = new StringBuilder();
            sb.Append(LoadGroups(1, new ArrayList(), new ArrayList()));

            strDemand = sb.ToString();


        }

        private void CheckDuplicateGroups()
        {
            //Check for Duplicate Grouping
            for (int i = 1; i <= 4; i++)
            {
                DropDownList ddl = this.FindControl("ddlGroup" + i.ToString()) as System.Web.UI.WebControls.DropDownList;

                for (int j = i; j <= 4; j++)
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
            strClassIds = sb.ToString();
            sb = new StringBuilder();


            string strEnvironment = Request.Form[hdnEnvironment.UniqueID];
            string[] strEnvironments;
            char[] strSplit = { ';' };
            strEnvironments = strEnvironment.Split(strSplit);

            for (int ii = 0; ii < strEnvironments.Length; ii++)
            {
                if (strEnvironments[ii].Trim() != "")
                {
                    if (sb.Length > 0)
                        sb.Append("," + strEnvironments[ii].Trim());
                    else
                        sb.Append(strEnvironments[ii].Trim());
                }
            }
            strEnvironmentIds = sb.ToString();
            sb = new StringBuilder();

        }


        private string LoadGroups(int intGrp, ArrayList arrDistinctCols, ArrayList arrSortCols)
        {
                StringBuilder sb = new StringBuilder();
                DataTable dtGroup=null;
                int intNextGrp=intGrp+1;
                DropDownList ddlGrp = this.FindControl("ddlGroup" + intGrp.ToString()) as System.Web.UI.WebControls.DropDownList;
                DropDownList ddlGrpNext = this.FindControl("ddlGroup" + intNextGrp.ToString()) as System.Web.UI.WebControls.DropDownList;
                string strGrpValue="";
                string strGrpValueNext="";
               
                
                if (ddlGrp != null)
                {
                    if (ddlGrp.SelectedValue!="0")
                        strGrpValue = ddlGrp.SelectedValue;
                    else
                        strGrpValue = "LocationId";
                }
                else
                {
                    strGrpValue = "LocationId";
                }
                
                if (ddlGrpNext != null)
                {
                    if (ddlGrpNext.SelectedValue!="0")
                        strGrpValueNext=ddlGrpNext.SelectedValue;
                    else
                        strGrpValueNext="";
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
                                foreach(DataRow dr in dtGroup.Rows)
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

                                    if (strGrpValueNext == "" )
                                    {
                                        sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                                        sb.Append(intGroup.ToString());
                                        sb.Append("\" style=\"display:none\">");
                                        sb.Append(LoadGroups(5, arrDistinctCols, arrSortCols));
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
                                sb.Append("\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">");
                                sb.Append(dr["Class"].ToString());
                                sb.Append("</td></tr>");

                                if (strGrpValueNext == "")
                                {
                                    sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                                    sb.Append(intGroup.ToString());
                                    sb.Append("\" style=\"display:none\">");
                                    sb.Append(LoadGroups(5, arrDistinctCols, arrSortCols));
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
                                sb.Append("\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">");
                                sb.Append(dr["Environment"].ToString());
                                sb.Append("</td></tr>");

                                if (strGrpValueNext == "")
                                {
                                    sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                                    sb.Append(intGroup.ToString());
                                    sb.Append("\" style=\"display:none\">");
                                    sb.Append(LoadGroups(5, arrDistinctCols, arrSortCols));
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
                                sb.Append("\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">");
                                sb.Append(dr["Confidence"].ToString());
                                sb.Append("</td></tr>");

                                if (strGrpValueNext == "")
                                {
                                    sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                                    sb.Append(intGroup.ToString());
                                    sb.Append("\" style=\"display:none\">");
                                    sb.Append(LoadGroups(5, arrDistinctCols, arrSortCols));
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
                            arrDistinctCols.Remove("ConfidenceId");
                            arrDistinctCols.Remove("Confidence");
                            arrSortCols.Remove("Confidence");
                            strFilterConfidence = "";
                            break;

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
                                sb.Append("\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\" class=\"header\">");
                                sb.Append(dr["Location"].ToString());
                                sb.Append("</td></tr>");

                                //Load the details for Location
                                sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                                sb.Append(intGroup.ToString());
                                sb.Append("\" style=\"display:none\">");
                                sb.Append(LoadDetails());
                                sb.Append("</td></tr>");
                            }
                            arrDistinctCols.Remove("LocationId");
                            arrDistinctCols.Remove("Location");
                            arrSortCols.Remove("Location");
                            strFilterLocation = "";
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
            string strFilters="";

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


            drs = dtSupplyAndDemand.Select(strFilters, strSort);

            foreach (DataRow dr in drs)
                dtNew.ImportRow(dr);

            //get the distinct
            return dtNew.DefaultView.ToTable(true, strColumnNames);
          
        }

        protected string LoadDetails()
        {
            StringBuilder sb = new StringBuilder();
           

            string strFilters = "";

            strFilters = strFilters + (strFilters != "" ? (strFilterLocation != "" ? " AND " + strFilterLocation : strFilterLocation) : strFilterLocation);
            strFilters = strFilters + (strFilters != "" ? (strFilterProject != "" ? " AND " + strFilterProject : strFilterProject) : strFilterProject);
            strFilters = strFilters + (strFilters != "" ? (strFilterClass != "" ? " AND " + strFilterClass : strFilterClass) : strFilterClass);
            strFilters = strFilters + (strFilters != "" ? (strFilterEnv != "" ? " AND " + strFilterEnv : strFilterEnv) : strFilterEnv);
            strFilters = strFilters + (strFilters != "" ? (strFilterConfidence != "" ? " AND " + strFilterConfidence : strFilterConfidence) : strFilterConfidence);


            string strSort = "Location";
            DataList dl = this.FindControl("dlDetails") as System.Web.UI.WebControls.DataList;

            DataRow[] drSelect = null;
            drSelect = dtSupplyAndDemand.Select(strFilters, strSort);

            foreach (DataRow drDetails in drSelect)
            {

                //Add Location 

                // Production 
                sb.Append("<tr><td colspan=\"4\" class=\"header\">Production</td></tr>");

                sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                sb.Append("<td nowrap width=\"20%\">High Performance:</td>");
                sb.Append("<td width=\"60%\">");
                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StorageHighProd"].ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                sb.Append(double.Parse(drDetails["StorageHighProd"].ToString()));
                sb.Append(" GB</td></tr>");
                
                sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                sb.Append("<td nowrap width=\"20%\">Standard Performance:</td>");
                sb.Append("<td width=\"60%\">");
                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StorageStandardProd"].ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                sb.Append(double.Parse(drDetails["StorageStandardProd"].ToString()));
                sb.Append(" GB</td></tr>");

                sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                sb.Append("<td nowrap width=\"20%\">Low Performance:</td>");
                sb.Append("<td width=\"60%\">");
                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StorageLowProd"].ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                sb.Append(double.Parse(drDetails["StorageLowProd"].ToString()));
                sb.Append(" GB</td></tr>");

                sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                sb.Append("<td nowrap width=\"20%\">Ports:</td>");
                sb.Append("<td width=\"60%\">");
                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StoragePortsProd"].ToString()) / double.Parse(intMaxPorts.ToString())) * 100.00), "95", false, "#CC3300"));
                sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                sb.Append(double.Parse(drDetails["StoragePortsProd"].ToString()));
                sb.Append("</td></tr>");

                sb.Append("<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>");

                // QA 
                sb.Append("<tr><td colspan=\"4\" class=\"header\">QA</td></tr>");

                sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                sb.Append("<td nowrap width=\"20%\">High Performance:</td>");
                sb.Append("<td width=\"60%\">");
                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StorageHighQA"].ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                sb.Append(double.Parse(drDetails["StorageHighQA"].ToString()));
                sb.Append(" GB</td></tr>");

                sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                sb.Append("<td nowrap width=\"20%\">Standard Performance:</td>");
                sb.Append("<td width=\"60%\">");
                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StorageStandardQa"].ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                sb.Append(double.Parse(drDetails["StorageStandardQa"].ToString()));
                sb.Append(" GB</td></tr>");

                sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                sb.Append("<td nowrap width=\"20%\">Low Performance:</td>");
                sb.Append("<td width=\"60%\">");
                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StorageLowQa"].ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                sb.Append(double.Parse(drDetails["StorageLowQa"].ToString()));
                sb.Append(" GB</td></tr>");

                sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                sb.Append("<td nowrap width=\"20%\">Ports:</td>");
                sb.Append("<td width=\"60%\">");
                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StoragePortsQA"].ToString()) / double.Parse(intMaxPorts.ToString())) * 100.00), "95", false, "#CC3300"));
                sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                sb.Append(double.Parse(drDetails["StoragePortsQA"].ToString()));
                sb.Append("</td></tr>");

                sb.Append("<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>");

                // Test 
                sb.Append("<tr><td colspan=\"4\" class=\"header\">Test</td></tr>");

                sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                sb.Append("<td nowrap width=\"20%\">High Performance:</td>");
                sb.Append("<td width=\"60%\">");
                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StorageHighTest"].ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                sb.Append(double.Parse(drDetails["StorageHighTest"].ToString()));
                sb.Append(" GB</td></tr>");

                sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                sb.Append("<td nowrap width=\"20%\">Standard Performance:</td>");
                sb.Append("<td width=\"60%\">");
                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StorageStandardTest"].ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                sb.Append(double.Parse(drDetails["StorageStandardTest"].ToString()));
                sb.Append(" GB</td></tr>");

                sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                sb.Append("<td nowrap width=\"20%\">Low Performance:</td>");
                sb.Append("<td width=\"60%\">");
                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StorageLowTest"].ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                sb.Append(double.Parse(drDetails["StorageLowTest"].ToString()));
                sb.Append(" GB</td></tr>");

                sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                sb.Append("<td nowrap width=\"20%\">Ports:</td>");
                sb.Append("<td width=\"60%\">");
                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StoragePortsTest"].ToString()) / double.Parse(intMaxPorts.ToString())) * 100.00), "95", false, "#CC3300"));
                sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                sb.Append(double.Parse(drDetails["StoragePortsTest"].ToString()));
                sb.Append("</td></tr>");

                sb.Append("<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>");

                // HA 
                sb.Append("<tr><td colspan=\"4\" class=\"header\">High Availability</td></tr>");

                sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                sb.Append("<td nowrap width=\"20%\">High Performance:</td>");
                sb.Append("<td width=\"60%\">");
                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StorageHighHA"].ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                sb.Append(double.Parse(drDetails["StorageHighHA"].ToString()));
                sb.Append(" GB</td></tr>");

                sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                sb.Append("<td nowrap width=\"20%\">Standard Performance:</td>");
                sb.Append("<td width=\"60%\">");
                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StorageStandardHA"].ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                sb.Append(double.Parse(drDetails["StorageStandardHA"].ToString()));
                sb.Append(" GB</td></tr>");

                sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                sb.Append("<td nowrap width=\"20%\">Low Performance:</td>");
                sb.Append("<td width=\"60%\">");
                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StorageLowHA"].ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                sb.Append(double.Parse(drDetails["StorageLowHA"].ToString()));
                sb.Append(" GB</td></tr>");

                sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                sb.Append("<td nowrap width=\"20%\">Ports:</td>");
                sb.Append("<td width=\"60%\">");
                sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StoragePortsHA"].ToString()) / double.Parse(intMaxPorts.ToString())) * 100.00), "95", false, "#CC3300"));
                sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                sb.Append(double.Parse(drDetails["StoragePortsHA"].ToString()));
                sb.Append("</td></tr>");

                sb.Append("<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>");

                if (oLocation.GetAddress(Int32.Parse(drDetails["LocationId"].ToString()), "dr") == "1")
                {
                    // Replicated 
                    sb.Append("<tr><td colspan=\"4\" class=\"header\">Replication</td></tr>");

                    sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                    sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                    sb.Append("<td nowrap width=\"20%\">High Performance:</td>");
                    sb.Append("<td width=\"60%\">");
                    sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StorageHighReplicated"].ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                    sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                    sb.Append(double.Parse(drDetails["StorageHighReplicated"].ToString()));
                    sb.Append(" GB</td></tr>");

                    sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                    sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                    sb.Append("<td nowrap width=\"20%\">Standard Performance:</td>");
                    sb.Append("<td width=\"60%\">");
                    sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StorageStandardReplicated"].ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                    sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                    sb.Append(double.Parse(drDetails["StorageStandardReplicated"].ToString()));
                    sb.Append(" GB</td></tr>");

                    sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                    sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                    sb.Append("<td nowrap width=\"20%\">Low Performance:</td>");
                    sb.Append("<td width=\"60%\">");
                    sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StorageLowReplicated"].ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                    sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                    sb.Append(double.Parse(drDetails["StorageLowReplicated"].ToString()));
                    sb.Append(" GB</td></tr>");
                }

                //sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\">");
                //sb.Append("<td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td>");
                //sb.Append("<td nowrap width=\"20%\">Ports:</td>");
                //sb.Append("<td width=\"60%\">");
                //sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(drDetails["StoragePortsReplicated"].ToString()) / double.Parse(intMaxPorts.ToString())) * 100.00), "95", false, "#CC3300"));
                //sb.Append("</td><td nowrap width=\"20%\" align=\"right\">");
                //sb.Append(double.Parse(drDetails["StoragePortsReplicated"].ToString()));
                //sb.Append("</td></tr>");

                //sb.Append("<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>");

            }


            if (sb.ToString() != "")
            {
                sb.Insert(0, "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"border:solid 1px #CCCCCC\">");
                sb.Append("</table>");
            }

            return sb.ToString();


        }

    }
}