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
    public partial class datapoint_service_searchOld : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intDataPointAvailableService = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_SERVICE"]);
        protected int intLists = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_LISTS"]);
        protected DataPoint oDataPoint;
        protected Functions oFunction;
        protected Users oUser;
        protected Models oModel;
        protected Variables oVariable;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected string strResults = "";
        protected int intCounter = 0;
        protected int intPage = 1;
        protected int intPerPage = 0;
        protected string strCookie = "SERVICE_SEARCH";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            Master.Page.Title = "DataPoint | Service Search";
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
            oVariable = new Variables(intEnvironment);
            Response.Cookies["storage"].Value = "0";
            Response.Cookies["applications"].Value = "0";
            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "SERVICE") == true || intDataPointAvailableService == 1))
            {
                panAllow.Visible = true;
                if (Request.QueryString["advanced"] == null)
                    panSimple.Visible = true;
                else
                    panAdvanced.Visible = true;
                oDataPoint.LoadButtonAdmin(fldSearch, intProfile, strCookie);
                try
                {
                    intPerPage = Int32.Parse(Request.QueryString["r"]);
                }
                catch { }
                try
                {
                    intPage = Int32.Parse(Request.QueryString["p"]);
                }
                catch { }
                if (!IsPostBack)
                {
                    LoadList();
                    txtSearch.Focus();

                    // Load Query
                    bool boolError = false;
                    string strQuery = "";
                    if (Request.QueryString["t"] != null && Request.QueryString["t"] != "" && Request.QueryString["q"] != null)
                    {
                        btnBasicNew.Visible = true;
                        string strType = Request.QueryString["t"];
                        strQuery = oFunction.decryptQueryString(Request.QueryString["q"]);
                        radSearch.SelectedValue = strType;
                        ddlSearch.SelectedValue = intPerPage.ToString();
                        switch (strType)
                        {
                            case "id":
                                if (strQuery != "")
                                {
                                    DataSet dsId = oDataPoint.GetServiceRequest(strQuery);
                                    if (dsId.Tables[0].Rows.Count == 1)
                                        Response.Redirect("/datapoint/service/request.aspx?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"]);
                                    else
                                    {
                                        panService.Visible = true;
                                        LoadRepeater(dsId, rptService, lblService, strQuery);
                                    }
                                }
                                else
                                    boolError = true;
                                break;
                            case "name":
                                if (strQuery != "")
                                {
                                    DataSet dsName = oDataPoint.GetServiceName(strQuery);
                                    if (dsName.Tables[0].Rows.Count == 1)
                                        Response.Redirect("/datapoint/service/request.aspx?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"]);
                                    else
                                    {
                                        panService.Visible = true;
                                        LoadRepeater(dsName, rptService, lblService, strQuery);
                                    }
                                }
                                else
                                    boolError = true;
                                break;
                            case "design":
                                if (strQuery != "")
                                {
                                    DataSet dsName = oDataPoint.GetServiceDesign(strQuery);
                                    if (dsName.Tables[0].Rows.Count == 1)
                                        Response.Redirect("/datapoint/service/design.aspx?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"]);
                                    else
                                    {
                                        panDesign.Visible = true;
                                        LoadRepeater(dsName, rptDesign, lblService, strQuery);
                                        foreach (RepeaterItem ri in rptDesign.Items)
                                        {
                                            int intUser = 0;
                                            Label lblUser = (Label)ri.FindControl("lblUser");
                                            if (lblUser.Text != "")
                                                intUser = Int32.Parse(lblUser.Text);
                                            if (intUser > 0)
                                                lblUser.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenWindow('PROFILE','?userid=" + intUser.ToString() + "');\">" + oUser.GetFullName(intUser) + " [" + oUser.GetName(intUser) + "]</a>";
                                            else
                                                lblUser.Text = "---";

                                            Label lblStatus = (Label)ri.FindControl("lblStatus");
                                            Label lblDelete = (Label)ri.FindControl("lblDelete");
                                            Label lblDeleteForecast = (Label)ri.FindControl("lblDeleteForecast");
                                            Label lblDeleteRequest = (Label)ri.FindControl("lblDeleteRequest");
                                            Label lblDeleteProject = (Label)ri.FindControl("lblDeleteProject");
                                            Label lblCompleted = (Label)ri.FindControl("lblCompleted");
                                            Label lblFinished = (Label)ri.FindControl("lblFinished");
                                            if (lblDelete != null && lblDelete.Text == "1")
                                                lblStatus.Text = "Design Deleted!";
                                            else if (lblDeleteForecast != null && lblDeleteForecast.Text == "1")
                                                lblStatus.Text = "Forecast Deleted!";
                                            else if (lblDeleteRequest != null && lblDeleteRequest.Text == "1")
                                                lblStatus.Text = "Request Deleted!";
                                            else if (lblDeleteProject != null && lblDeleteProject.Text == "1")
                                                lblStatus.Text = "Project Deleted!";
                                            else if (lblFinished.Text != "")
                                                lblStatus.Text = "Finished on " + lblFinished.Text;
                                            else if (lblCompleted.Text != "")
                                                lblStatus.Text = "Built on " + lblFinished.Text;
                                            else
                                            {
                                            }

                                        }
                                    }
                                }
                                else
                                    boolError = true;
                                break;
                            default:
                                boolError = true;
                                break;
                        }
                    }
                    else if (Request.Cookies[strCookie] != null && Request.Cookies[strCookie].Value != "")
                        radSearch.SelectedValue = Request.Cookies[strCookie].Value;
                    txtSearch.Text = strQuery;
                    if (boolError == true)
                        Response.Redirect(Request.Path + "?error=true");
                    if (radSearch.SelectedIndex > -1)
                        hdnSearchType.Value = radSearch.SelectedItem.Value;
                }
                if (intLists == 1)
                {
                    lnkSearch.Enabled = true;
                    lnkSearch2.Enabled = true;
                    //AJAXTextBoxGet(oText,oWidth,oHeight,oDiv,oList,oHidden,oRequest,oNumber,oMultiple,param1,param2,oHiddens,oTableAdd)
                    txtSearch.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','145','" + divSearch.ClientID + "','" + lstSearch.ClientID + "','" + hdnSearchText.ClientID + "','" + oVariable.URL() + "/frame/ajax/datapoint/ajax_datapoint_asset.aspx',2,null," + intProfile.ToString() + ",document.getElementById('" + hdnSearchType.ClientID + "').value);");
                    lstSearch.Attributes.Add("ondblclick", "AJAXClickRow();");
                }
                txtSearch.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnBasicSearch.ClientID + "').click();return false;}} else {return true}; ");
                radSearch.Attributes.Add("onclick", "ChangeCookieSearch(this,'" + strCookie + "');UpdateDataPointSearch('" + hdnSearchType.ClientID + "');");
                btnBasicSearch.Attributes.Add("onclick", "return ValidateRadioList('" + radSearch.ClientID + "','Please select a search type')" +
                    " && ValidateTextDisabled('" + txtSearch.ClientID + "','Please enter a value to search')" +
                    " && ProcessButton(this)" +
                    ";");
            }
            else
                panDenied.Visible = true;
        }
        private void LoadList()
        {
            radSearch.Items.Add(new ListItem("RequestID", "id"));
            radSearch.Items.Add(new ListItem("Request Name", "name"));
            radSearch.Items.Add(new ListItem("DesignID", "design"));
        }
        protected void btnBasicNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnBasicSearch_Click(Object Sender, EventArgs e)
        {
            if (intProfile > 0)
            {
                oDataPoint.DeleteSearch(txtSearch.Text.Trim(), intProfile, Request.Form[hdnSearchType.UniqueID]);
                oDataPoint.AddSearch(txtSearch.Text.Trim(), intProfile, Request.Form[hdnSearchType.UniqueID]);
            }
            Response.Redirect(Request.Path + "?t=" + radSearch.SelectedItem.Value + "&q=" + oFunction.encryptQueryString(txtSearch.Text.Trim()) + "&r=" + ddlSearch.SelectedItem.Value);
        }
        protected void btnAdvancedNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnAdvancedSearch_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?t=" + radSearch.SelectedItem.Value + "&q=" + oFunction.encryptQueryString(txtSearch.Text.Trim()) + "&r=" + ddlSearch.SelectedItem.Value);
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            intPage--;
            Response.Redirect(Request.Path + "?t=" + radSearch.SelectedItem.Value + "&q=" + oFunction.encryptQueryString(txtSearch.Text) + "&r=" + ddlSearch.SelectedItem.Value + "&p=" + intPage.ToString());
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            intPage++;
            Response.Redirect(Request.Path + "?t=" + radSearch.SelectedItem.Value + "&q=" + oFunction.encryptQueryString(txtSearch.Text) + "&r=" + ddlSearch.SelectedItem.Value + "&p=" + intPage.ToString());
        }
        private void LoadRepeater(DataSet ds, Repeater rptRepeater, Label lblNone, string strQuery)
        {
            panResults.Visible = true;
            intPage--;
            if (ds.Tables.Count > 1)
                ds.Relations.Add("relationship", ds.Tables[0].Columns["requestid"], ds.Tables[1].Columns["requestid"]);
            DataView dv = ds.Tables[0].DefaultView;
            int intTotal = dv.Count;
            int intStart = (intPerPage * intPage);
            if (intStart > intTotal)
                intStart = 0;
            if (intPage == 0)
                btnBack.Enabled = false;
            int intEnd = intStart + intPerPage;
            if (intPerPage == 0)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = false;
                intEnd = intTotal;
            }
            if (intTotal < intEnd)
            {
                intEnd = intTotal;
                btnNext.Enabled = false;
            }
            int ii = 0;
            for (ii = 0; ii < intStart; ii++)
                dv[0].Delete();
            int intTotalCount = (intTotal - intEnd);
            for (ii = 0; ii < intTotalCount; ii++)
                dv[intPerPage].Delete();
            rptRepeater.DataSource = dv;
            rptRepeater.DataBind();
            foreach (RepeaterItem ri in rptRepeater.Items)
            {
                Repeater rptServices = (Repeater)ri.FindControl("rptServices");
                Panel panServicesYes = (Panel)ri.FindControl("panServicesYes");
                Panel panServicesNo = (Panel)ri.FindControl("panServicesNo");
                if (rptServices != null)
                {
                    if (rptServices.Items.Count == 0)
                        panServicesNo.Visible = true;
                    else
                        panServicesYes.Visible = true;
                }
            }
            lblNone.Visible = (rptRepeater.Items.Count == 0);
            if (intEnd > 0)
            {
                intStart++;
                intCounter = intStart;
                lblRecords.Text = "Showing Results <b>" + intStart.ToString() + " - " + intEnd.ToString() + "</b> of <b>" + intTotal.ToString() + "</b> for <b>" + strQuery + "</b>...";
            }
            else
                lblRecords.Text = "No Results Found for <b>" + strQuery + "</b>...";
        }
        protected void btnAdvanced_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?advanced=true");
        }
        protected void btnBasic_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void lnkSearch_Click(Object Sender, EventArgs e)
        {
            if (intProfile > 0)
                oDataPoint.DeleteSearch(intProfile);
            if (Request.QueryString["t"] != null && Request.QueryString["q"] != null)
                Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&r=" + ddlSearch.SelectedItem.Value + "&p=" + intPage.ToString());
            else
                Response.Redirect(Request.Path);
        }
    }
}
