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
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Presentation.Web
{
    public partial class datapoint_project_search : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intDataPointAvailableProject = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_PROJECT"]);
        protected DataPoint oDataPoint;
        protected Functions oFunction;
        protected Users oUser;
        protected Models oModel;
        protected Applications oApplications;
        protected Variables oVariable;
        protected Pages oPage;
        protected Organizations oOrganizations;
        protected StatusLevels oStatusLevel;

        private int intProfile = 0;
        private int intApplication = 0;
        private bool boolAdvSearch = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Page.Title = "DataPoint | Project Search";
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            oDataPoint = new DataPoint(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oUser = new Users(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oApplications = new Applications(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oPage = new Pages(intProfile, dsn);
            oOrganizations = new Organizations(intProfile, dsn);
            oStatusLevel = new StatusLevels();

            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "PROJECT") == true || intDataPointAvailableProject == 1))
            {
                pnlAllow.Visible = true;
              
                if (Request.QueryString["advanced"] == null)
                {   
                    pnlBasicSearch.Visible = true;
                    pnlAdvancedSearch.Visible = false;
                    lnkbtnAdvancedSearch.Visible = true;
                    lnkbtnBasicSearch.Visible = false;
                    btnAdvancedSearch.Visible = false;
                    btnBasicSearch.Visible = true;
                }
                else
                {
                    boolAdvSearch = true;
                    pnlBasicSearch.Visible = true;
                    pnlAdvancedSearch.Visible = true;
                    lnkbtnAdvancedSearch.Visible = false;
                    lnkbtnBasicSearch.Visible = true;
                    btnAdvancedSearch.Visible = true;
                    btnBasicSearch.Visible = false;
                   
                }
                if (!IsPostBack) 
                    LoadList();

                AddControlAttributes();
            }
            else
                pnlDenied.Visible = true;
        }

        private void AddControlAttributes()
        {
            //Basic Search
            if (boolAdvSearch == false)
            {
                txtProjectNumber.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnBasicSearch.ClientID + "').click();return false;}} else {return true}; ");
                txtProjectName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnBasicSearch.ClientID + "').click();return false;}} else {return true}; ");
                btnBasicSearch.Attributes.Add("onclick", "return ValidateBasicSearchControls()" +
                " && ProcessButton(this)" +
                ";");
            }
            //Advance Search
            else
            {
                txtProjectNumber.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnAdvancedSearch.ClientID + "').click();return false;}} else {return true}; ");
                txtProjectName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnAdvancedSearch.ClientID + "').click();return false;}} else {return true}; ");
               
                txtProjectManager.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divProjectManager.ClientID + "','" + lstProjectManager.ClientID + "','" + hdnProjectManager.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstProjectManager.Attributes.Add("ondblclick", "AJAXClickRow();");

                imgOpenedAfterDate.Attributes.Add("onclick", "return ShowCalendar('" + txtCreatedAfter.ClientID + "');");
                imgOpenedBeforeDate.Attributes.Add("onclick", "return ShowCalendar('" + txtCreatedBefore.ClientID + "');");

                imgUpdatedAfterDate.Attributes.Add("onclick", "return ShowCalendar('" + txtModifiedAfter.ClientID + "');");
                imgUpdatedBeforeDate.Attributes.Add("onclick", "return ShowCalendar('" + txtModifiedBefore.ClientID + "');");

                imgClosedAfterDate.Attributes.Add("onclick", "return ShowCalendar('" + txtCompletedAfter.ClientID + "');");
                imgClosedBeforeDate.Attributes.Add("onclick", "return ShowCalendar('" + txtCompletedBefore.ClientID + "');");

                btnAdvancedSearch.Attributes.Add("onclick", "return ValidateAdvanceSearchControls()" +
                " && ProcessButton(this)" +
                ";");
            }
            lnkbtnClearHistory.Attributes.Add("onclick", "ClearSearchCriteria();");

        }

        private void LoadList()
        {
            if (boolAdvSearch == true)
            {
                DataSet dsOrg = oOrganizations.Gets(1);
                ddlOrganization.DataTextField = "name";
                ddlOrganization.DataValueField = "organizationid";
                ddlOrganization.DataSource = dsOrg;
                ddlOrganization.DataBind();
                ddlOrganization.Items.Insert(0, new ListItem("--All--", "-1"));
            }
            
            DataSet dsStatus = SqlHelper.ExecuteDataset(dsn, CommandType.Text, oStatusLevel.ProjectStatusList());
            ddlStatus.DataTextField = "name";
            ddlStatus.DataValueField = "id";
            ddlStatus.DataSource = dsStatus;
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("--All--", "-100"));

        }
        protected void lnkbtnBasicSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }

        protected void lnkbtnAdvancedSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?advanced=true");
        }

        protected void btnBasicSearch_Click(object sender, EventArgs e)
        {
            //Basic Search
            hdnPageNo.Value = "1";
            hdnRecsPerPage.Value = ddlResultsPerPage.SelectedItem.Value;
            BindControls();
        }

        protected void btnAdvancedSearch_Click(object sender, EventArgs e)
        {
            hdnPageNo.Value = "1";
            hdnRecsPerPage.Value = ddlResultsPerPage.SelectedItem.Value;
            BindControls();
        }
        protected void lnkbtnClearHistory_Click(object sender, EventArgs e)
        {

        }

        private void BindControls()
        {
            pnlResults.Visible = true;
            DataSet ds;
            if (boolAdvSearch==true)
                    ds= oDataPoint.GetProjectSearchResults(
                                    null,
                                    txtProjectNumber.Text.Trim(),
                                    txtProjectName.Text.Trim(),
                                    ((ddlStatus.SelectedValue.ToString() != "-100") ? Int32.Parse(ddlStatus.SelectedValue.ToString()) : (int?)null),
                                    ((hdnProjectManager.Value != "") ? Int32.Parse(hdnProjectManager.Value) : (int?)null),
                                    ((ddlOrganization.SelectedValue.ToString() != "-1") ? Int32.Parse(ddlOrganization.SelectedValue.ToString()) : (int?)null),
                                    ((txtCreatedAfter.Text.Trim() != "") ? DateTime.Parse(txtCreatedAfter.Text.Trim()) : (DateTime?)null),
                                    ((txtCreatedBefore.Text.Trim() != "") ? DateTime.Parse(txtCreatedBefore.Text.Trim()) : (DateTime?)null),
                                    ((txtModifiedAfter.Text.Trim() != "") ? DateTime.Parse(txtModifiedAfter.Text.Trim()) : (DateTime?)null),
                                    ((txtModifiedBefore.Text.Trim() != "") ? DateTime.Parse(txtModifiedBefore.Text.Trim()) : (DateTime?)null),
                                    ((txtCompletedAfter.Text.Trim() != "") ? DateTime.Parse(txtCompletedAfter.Text.Trim()) : (DateTime?)null),
                                    ((txtCompletedBefore.Text.Trim() != "") ? DateTime.Parse(txtCompletedBefore.Text.Trim()) : (DateTime?)null),
                                hdnOrderBy.Value.ToString(), Int32.Parse(hdnOrder.Value.ToString()), Int32.Parse(hdnPageNo.Value), Int32.Parse(hdnRecsPerPage.Value));
            else
                     ds= oDataPoint.GetProjectSearchResults(
                                null,
                                txtProjectNumber.Text.Trim(),
                                txtProjectName.Text.Trim(),
                                ((ddlStatus.SelectedValue.ToString() != "-100") ? Int32.Parse(ddlStatus.SelectedValue.ToString()) : (int?)null),
                                null,null,null,null,null,null,null,null,
                                hdnOrderBy.Value.ToString(), Int32.Parse(hdnOrder.Value.ToString()), Int32.Parse(hdnPageNo.Value), Int32.Parse(hdnRecsPerPage.Value));


           dlProjects.DataSource = ds.Tables[0];
           dlProjects.DataBind();

           if (ds.Tables[0].Rows.Count > 0)
           {
               long lngDisplayRecords = Int64.Parse(ds.Tables[0].Rows[0]["rownum"].ToString()) + Int64.Parse((ds.Tables[0].Rows.Count - 1).ToString());
               lblRecords.Text = "Showing Results <b>" + ds.Tables[0].Rows[0]["rownum"].ToString() + " - " + lngDisplayRecords.ToString() + "</b> of <b>" + ds.Tables[1].Rows[0]["TotalRecords"].ToString() + "</b>...";
           }
           else
               lblRecords.Text = "No Results Found...";

           // Calculate total numbers of pages
           long lngRecsPerPage = Int64.Parse(hdnRecsPerPage.Value);
           if (lngRecsPerPage != 0)
           {
               long lngTotalRecsCount = Int64.Parse(ds.Tables[1].Rows[0]["TotalRecords"].ToString());
               long lngPgCount = lngTotalRecsCount / lngRecsPerPage + ((lngTotalRecsCount % lngRecsPerPage) > 0 ? 1 : 0);
               if (Int32.Parse(hdnRecsPerPage.Value) != 0)
               {
                   // Display Next button
                   if (lngPgCount - 1 >= Convert.ToInt64(hdnPageNo.Value))
                       btnNext.Enabled = true;
                   else
                       btnNext.Enabled = false;
                   // Display Prev button
                   if ((Convert.ToInt64(hdnPageNo.Value)) > 1)
                       btnPrevious.Enabled = true;
                   else
                       btnPrevious.Enabled = false;
               }
           }
           else
           {
               btnNext.Enabled = false;
               btnPrevious.Enabled = false;
           }
        }

        protected void dlProjects_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                
                ((Label)e.Item.FindControl("lblProjectNo")).Text = (drv["ProjectNumber"]!=DBNull.Value ? drv["ProjectNumber"].ToString():"--");
                ((Label)e.Item.FindControl("lblProjectNo")).ToolTip = "Project Id: "+drv["ProjectId"].ToString();

                LinkButton lnkbtnProjectName = (LinkButton)e.Item.FindControl("lnkProjectName");
                lnkbtnProjectName.Text = drv["ProjectName"].ToString();
                lnkbtnProjectName.ToolTip = "Project Id: " + drv["ProjectId"].ToString();
                lnkbtnProjectName.Attributes.Add("onclick", "return OpenNewWindowMenu('/datapoint/projects/datapoint_projects.aspx?id=" + oFunction.encryptQueryString(drv["projectid"].ToString()) + "', '800', '600');");

                ((Label)e.Item.FindControl("lblSponsoringOrg")).Text = drv["ProjectOrgName"].ToString();

                ((Label)e.Item.FindControl("lblProjectType")).Text = drv["ProjectBaseDiscretion"].ToString();

                LinkButton lnkbtnProjectManager = (LinkButton)e.Item.FindControl("lnkProjectManager");
                lnkbtnProjectManager.Text = drv["ProjectLeadName"].ToString() + (drv["ProjectLeadXID"].ToString()!="" ?"(" + drv["ProjectLeadXID"].ToString() + ")":"");
                lnkbtnProjectManager.ToolTip = "User Id :" + drv["ProjectLead"].ToString();
                lnkbtnProjectManager.Attributes.Add("onclick", "return OpenWindow('PROFILE','?userid=" + drv["ProjectLead"].ToString() + "');");
                //btnView.Attributes.Add("onclick", "return OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(intResource.ToString()) + "', '800', '600');");
                

                ((Label)e.Item.FindControl("lblProjectStatus")).Text = drv["ProjectStatus"].ToString();
               


            }
        }


        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            LinkButton oOrder = (LinkButton)Sender;
            if (hdnOrderBy.Value == oOrder.CommandArgument)
            {
                if (hdnOrder.Value == "1")
                    hdnOrder.Value = "0";
                else if (hdnOrder.Value == "0")
                    hdnOrder.Value = "1";
            }
            else
            {
                hdnOrderBy.Value = oOrder.CommandArgument;
                hdnOrder.Value = "0";
            }

            BindControls();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            hdnPageNo.Value = Convert.ToString(Convert.ToInt64(hdnPageNo.Value) + 1);
            BindControls();
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            hdnPageNo.Value = Convert.ToString(Convert.ToInt64(hdnPageNo.Value) - 1);
            BindControls();
        }

    }
}
