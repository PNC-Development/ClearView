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
using System.Text;
namespace NCC.ClearView.Presentation.Web
{
    public partial class project_update : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Users oUser;
        protected Projects oProject;
        protected Organizations oOrganization;
        protected Segment oSegment;
        protected Functions oFunction;
        protected Variables oVariable;
        protected StatusLevels oStatusLevel;
        protected Forecast oForecast;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strMultiple = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            oOrganization = new Organizations(intProfile, dsn);
            oSegment = new Segment(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oVariable = new Variables(intEnvironment);
            //oProjectPending = new ProjectsPending(intProfile, dsn, intEnvironment);
            //oRequest = new Requests(intProfile, dsn);
            oStatusLevel = new StatusLevels();
            oForecast = new Forecast(intProfile, dsn);
            //oModel = new Models(intProfile, dsn);
            //oType = new Types(intProfile, dsn);
            //oModelsProperties = new ModelsProperties(intProfile, dsn);
            //oPlatform = new Platforms(intProfile, dsn);
            //oConfidence = new Confidence(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            if (Request.QueryString["project"] != null && Request.QueryString["project"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "projectsaved", "<script type=\"text/javascript\">alert('Project Information Saved');<" + "/" + "script>");
            if (!IsPostBack)
            {
                if (Request.QueryString["name"] != null)
                {
                    string strQuery = oFunction.decryptQueryString(Request.QueryString["name"]);
                    txtNameSearch.Text = strQuery;
                    CheckResults(strQuery, "", 0, 0);
                }
                else if (Request.QueryString["number"] != null)
                {
                    string strQuery = oFunction.decryptQueryString(Request.QueryString["number"]);
                    txtNumberSearch.Text = strQuery;
                    CheckResults("", strQuery, 0, 0);
                }
                else if (Request.QueryString["lead"] != null)
                {
                    int intQuery = Int32.Parse(oFunction.decryptQueryString(Request.QueryString["lead"]));
                    txtManagerSearch.Text = oUser.GetFullName(intQuery) + " (" + oUser.GetName(intQuery) + ")";
                    hdnManagerSearch.Value = intQuery.ToString();
                    CheckResults("", "", intQuery, 0);
                }
                else if (Request.QueryString["engineer"] != null)
                {
                    int intQuery = Int32.Parse(oFunction.decryptQueryString(Request.QueryString["engineer"]));
                    txtEngineerSearch.Text = oUser.GetFullName(intQuery) + " (" + oUser.GetName(intQuery) + ")";
                    hdnEngineerSearch.Value = intQuery.ToString();
                    CheckResults("", "", 0, intQuery);
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    LoadLists();
                    LoadProject(Int32.Parse(oFunction.decryptQueryString(Request.QueryString["id"])));
                }
                else
                    panNone.Visible = true;
                txtWorking.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divWorking.ClientID + "','" + lstWorking.ClientID + "','" + hdnWorking.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstWorking.Attributes.Add("ondblclick", "AJAXClickRow();");
                txtExecutive.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divExecutive.ClientID + "','" + lstExecutive.ClientID + "','" + hdnExecutive.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstExecutive.Attributes.Add("ondblclick", "AJAXClickRow();");
                txtManager.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divManager.ClientID + "','" + lstManager.ClientID + "','" + hdnManager.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstManager.Attributes.Add("ondblclick", "AJAXClickRow();");
                txtEngineer.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divEngineer.ClientID + "','" + lstEngineer.ClientID + "','" + hdnEngineer.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstEngineer.Attributes.Add("ondblclick", "AJAXClickRow();");
                txtLead.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divLead.ClientID + "','" + lstLead.ClientID + "','" + hdnLead.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstLead.Attributes.Add("ondblclick", "AJAXClickRow();");
                txtManagerSearch.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divManagerSearch.ClientID + "','" + lstManagerSearch.ClientID + "','" + hdnManagerSearch.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstManagerSearch.Attributes.Add("ondblclick", "AJAXClickRow();");
                txtEngineerSearch.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divEngineerSearch.ClientID + "','" + lstEngineerSearch.ClientID + "','" + hdnEngineerSearch.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstEngineerSearch.Attributes.Add("ondblclick", "AJAXClickRow();");
                ddlPortfolio.Attributes.Add("onchange", "PopulateSegments('" + ddlPortfolio.ClientID + "','" + ddlSegment.ClientID + "');");
                ddlSegment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlSegment.ClientID + "','" + hdnSegment.ClientID + "');");
            }
        }
        private void LoadLists()
        {
            ddlPortfolio.DataValueField = "organizationid";
            ddlPortfolio.DataTextField = "name";
            ddlPortfolio.DataSource = oOrganization.Gets(1);
            ddlPortfolio.DataBind();
            ddlPortfolio.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void CheckResults(string strName, string strNumber, int intLead, int intEngineer)
        {
            DataSet dsProject = new DataSet();
            if (strName != "")
                dsProject = oProject.GetProjectsLikeName(strName);
            else if (strNumber != "")
                dsProject = oProject.GetProjectsLikeNumber(strNumber);
            else if (intLead > 0)
                dsProject = oProject.GetProjectsLead(intLead);
            else if (intEngineer > 0)
                dsProject = oProject.GetProjectsEngineer(intEngineer);

            if (dsProject.Tables[0].Rows.Count == 1)
                Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + oFunction.encryptQueryString(dsProject.Tables[0].Rows[0]["projectid"].ToString()));
            else
            {
                panNone.Visible = true;
                panMultiple.Visible = true;
                StringBuilder sb = new StringBuilder(strMultiple);

                foreach (DataRow dr in dsProject.Tables[0].Rows)
                {
                    sb.Append("<tr onmouseover=\"CellRowOver(this);\" onmouseout=\"CellRowOut(this);\" onclick=\"window.navigate('");
                    sb.Append(oPage.GetFullLink(intPage));
                    sb.Append("?id=");
                    sb.Append(oFunction.encryptQueryString(dr["projectid"].ToString()));
                    sb.Append("');\">");
                    sb.Append("<td>");
                    sb.Append(dr["name"].ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(dr["number"].ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(dr["bd"].ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(oOrganization.GetName(Int32.Parse(dr["organization"].ToString())));
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(oStatusLevel.HTML(Int32.Parse(dr["status"].ToString())));
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }

                strMultiple = sb.ToString();
            }
        }
        protected void LoadProject(int intProject)
        {
            DataSet dsProject = oProject.Get(intProject);
            if (dsProject.Tables[0].Rows.Count > 0)
            {
                panSearch.Visible = true;
                lblProject.Text = intProject.ToString();
                txtName.Text = dsProject.Tables[0].Rows[0]["name"].ToString();
                txtNumber.Text = dsProject.Tables[0].Rows[0]["number"].ToString();
                ddlBaseDisc.SelectedValue = dsProject.Tables[0].Rows[0]["bd"].ToString();
                int intPortfolio = Int32.Parse(dsProject.Tables[0].Rows[0]["organization"].ToString());
                ddlPortfolio.SelectedValue = intPortfolio.ToString();
                if (intPortfolio > 0)
                {
                    int intSegment = Int32.Parse(dsProject.Tables[0].Rows[0]["segmentid"].ToString());
                    hdnSegment.Value = intSegment.ToString();
                    ddlSegment.Enabled = true;
                    ddlSegment.DataTextField = "name";
                    ddlSegment.DataValueField = "id";
                    ddlSegment.DataSource = oSegment.Gets(intPortfolio, 1);
                    ddlSegment.DataBind();
                    ddlSegment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                    ddlSegment.SelectedValue = intSegment.ToString();
                }

                lblStatus.Text = oStatusLevel.HTML(Int32.Parse(dsProject.Tables[0].Rows[0]["status"].ToString()));
                int intExecutive = 0;
                int intWorking = 0;
                int intManager = 0;
                int intEngineer = 0;
                int intLead = 0;
                if (dsProject.Tables[0].Rows[0]["executive"].ToString() != "")
                    intExecutive = Int32.Parse(dsProject.Tables[0].Rows[0]["executive"].ToString());
                if (dsProject.Tables[0].Rows[0]["working"].ToString() != "")
                    intWorking = Int32.Parse(dsProject.Tables[0].Rows[0]["working"].ToString());
                if (dsProject.Tables[0].Rows[0]["lead"].ToString() != "")
                    intManager = Int32.Parse(dsProject.Tables[0].Rows[0]["lead"].ToString());
                if (dsProject.Tables[0].Rows[0]["engineer"].ToString() != "")
                    intEngineer = Int32.Parse(dsProject.Tables[0].Rows[0]["engineer"].ToString());
                if (dsProject.Tables[0].Rows[0]["technical"].ToString() != "")
                    intLead = Int32.Parse(dsProject.Tables[0].Rows[0]["technical"].ToString());
                if (intExecutive > 0)
                {
                    txtExecutive.Text = oUser.GetFullName(intExecutive) + " (" + oUser.GetName(intExecutive) + ")";
                    hdnExecutive.Value = intExecutive.ToString();
                }
                if (intWorking > 0)
                {
                    txtWorking.Text = oUser.GetFullName(intWorking) + " (" + oUser.GetName(intWorking) + ")";
                    hdnWorking.Value = intWorking.ToString();
                }
                if (intManager > 0)
                {
                    txtManager.Text = oUser.GetFullName(intManager) + " (" + oUser.GetName(intManager) + ")";
                    hdnManager.Value = intManager.ToString();
                }
                if (intEngineer > 0)
                {
                    txtEngineer.Text = oUser.GetFullName(intEngineer) + " (" + oUser.GetName(intEngineer) + ")";
                    hdnEngineer.Value = intEngineer.ToString();
                }
                if (intLead > 0)
                {
                    txtLead.Text = oUser.GetFullName(intLead) + " (" + oUser.GetName(intLead) + ")";
                    hdnLead.Value = intLead.ToString();
                }
                btnSave.Attributes.Add("onclick", "return ValidateText('" + txtName.Text + "','Please enter a project name')" +
                    " && ValidateText('" + txtNumber.ClientID + "','Please enter a project number')" +
                    " && ValidateDropDown('" + ddlPortfolio.ClientID + "','Please select a portfolio')" +
                    //" && ValidateHidden0('" + hdnExecutive.ClientID + "','" + txtExecutive.ClientID + "','Please enter the LAN ID of your executive sponsor')" +
                    //" && ValidateHidden0('" + hdnWorking.ClientID + "','" + txtWorking.ClientID + "','Please enter the LAN ID of your working sponsor')" +
                    //" && ValidateHidden0('" + hdnManager.ClientID + "','" + txtManager.ClientID + "','Please enter the LAN ID of your project lead')" +
                    //" && ValidateHidden0('" + hdnEngineer.ClientID + "','" + txtEngineer.ClientID + "','Please enter the LAN ID of your integration engineer')" +
                    //" && ValidateHidden0('" + hdnLead.ClientID + "','" + txtLead.ClientID + "','Please enter the LAN ID of your technical lead')" +
                    ";");
            }
            else
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "delete", "<script type=\"text/javascript\">alert('Invalid Project Specified\\n\\nPlease enter valid project information to continue');<" + "/" + "script>");
        }
        protected void btnSearch_Click(Object Sender, EventArgs e)
        {
            if (txtNameSearch.Text != "")
                Response.Redirect(oPage.GetFullLink(intPage) + "?name=" + oFunction.encryptQueryString(txtNameSearch.Text));
            else if (txtNumberSearch.Text != "")
                Response.Redirect(oPage.GetFullLink(intPage) + "?number=" + oFunction.encryptQueryString(txtNumberSearch.Text));
            else if (Request.Form[hdnManagerSearch.UniqueID] != "")
                Response.Redirect(oPage.GetFullLink(intPage) + "?lead=" + oFunction.encryptQueryString(Request.Form[hdnManagerSearch.UniqueID]));
            else if (Request.Form[hdnEngineerSearch.UniqueID] != "")
                Response.Redirect(oPage.GetFullLink(intPage) + "?engineer=" + oFunction.encryptQueryString(Request.Form[hdnManagerSearch.UniqueID]));
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?id=0");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intProject = Int32.Parse(lblProject.Text);
            int intEngineerOld = 0;
            if (oProject.Get(intProject, "engineer") != "")
                intEngineerOld = Int32.Parse(oProject.Get(intProject, "engineer"));
            int intManager = 0;
            if (Request.Form[hdnManager.UniqueID] != "")
                intManager = Int32.Parse(Request.Form[hdnManager.UniqueID]);
            int intExecutive = 0;
            if (Request.Form[hdnExecutive.UniqueID] != "")
                intExecutive = Int32.Parse(Request.Form[hdnExecutive.UniqueID]);
            int intWorking = 0;
            if (Request.Form[hdnWorking.UniqueID] != "")
                intWorking = Int32.Parse(Request.Form[hdnWorking.UniqueID]);
            int intLead = 0;
            if (Request.Form[hdnLead.UniqueID] != "")
                intLead = Int32.Parse(Request.Form[hdnLead.UniqueID]);
            int intEngineer = 0;
            if (Request.Form[hdnEngineer.UniqueID] != "")
                intEngineer = Int32.Parse(Request.Form[hdnEngineer.UniqueID]);
            int intSegment = 0;
            if (Request.Form[hdnSegment.UniqueID] != "")
                intSegment = Int32.Parse(Request.Form[hdnSegment.UniqueID]);
            oProject.Update(intProject, txtName.Text, ddlBaseDisc.SelectedItem.Value, txtNumber.Text, Int32.Parse(ddlPortfolio.SelectedItem.Value), intSegment);
            oProject.Update(intProject, intManager, intExecutive, intWorking, intLead, intEngineer, 0);
            // Update Design Builder to Reflect New IE
            if (intEngineerOld > 0)
            {
                DataSet ds = oProject.GetProjectForecast(intProject);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intAnswer = Int32.Parse(dr["id"].ToString());
                    oProject.UpdateForecastAnswer(intEngineer, intAnswer);
                    int intForecast = Int32.Parse(dr["forecastid"].ToString());
                    oProject.UpdateForecastForID(intEngineer, intForecast);
                    int intRequest = Int32.Parse(dr["requestid"].ToString());
                    oProject.UpdateForecastForRequestID(intEngineer, intRequest);
                }
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + oFunction.encryptQueryString(intProject.ToString()) + "&project=true");
        }
    }
}