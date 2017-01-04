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
    public partial class datapoint_asset_search : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intDataPointAvailableAsset = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_ASSET"]);
        protected int intLists = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_LISTS"]);
        protected DataPoint oDataPoint;
        protected Functions oFunction;
        protected Users oUser;
        protected Models oModel;
        protected Variables oVariable;
        protected Mnemonic oMnemonic;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected string strResults = "";
        protected int intCounter = 0;
        protected int intPage = 1;
        protected int intPerPage = 0;
        protected string strCookie = "ASSET_SEARCH";
        protected bool boolAdvanced = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            Master.Page.Title = "DataPoint | Asset Search";
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
            oMnemonic = new Mnemonic(intProfile, dsn);
            Response.Cookies["storage"].Value = "0";
            Response.Cookies["applications"].Value = "0";
            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "ASSET") == true || intDataPointAvailableAsset == 1))
            {
                panAllow.Visible = true;
                oDataPoint.LoadButtonAdmin(fldSearch, intProfile, strCookie);
                if (oUser.IsAdmin(intProfile) == true)
                    fldModel.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('DATAPOINT_MODELS','');\">" + fldModel.Text + "</a>";
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
                    int intMnemonic = 0;
                    if (Request.QueryString["mnem"] != null)
                        Int32.TryParse(Request.QueryString["mnem"], out intMnemonic);
                    string strBuildStart = "";
                    if (String.IsNullOrEmpty(Request.QueryString["bs"]) == false)
                        strBuildStart = oFunction.decryptQueryString(Request.QueryString["bs"]);
                    string strBuildEnd = "";
                    if (String.IsNullOrEmpty(Request.QueryString["be"]) == false)
                        strBuildEnd = oFunction.decryptQueryString(Request.QueryString["be"]);
                    bool boolDecoms = false;
                    if (String.IsNullOrEmpty(Request.QueryString["decoms"]) == false)
                        boolDecoms = (Request.QueryString["decoms"] == "1");

                    // Check to see if advanced
                    if (intMnemonic > 0 || strBuildStart != "" || strBuildEnd != "" || boolDecoms == true)
                    {
                        // Load Advanced
                        Advanced(true);
                        // Load fields
                        if (intMnemonic > 0)
                        {
                            txtSearchMnemonic.Text = oMnemonic.Get(intMnemonic, "factory_code") + " - " + oMnemonic.Get(intMnemonic, "name");
                            hdnSearchMnemonic.Value = intMnemonic.ToString();
                        }
                        if (strBuildStart != "")
                            txtBuildStart.Text = strBuildStart;
                        if (strBuildEnd != "")
                            txtBuildEnd.Text = strBuildEnd;
                        chkDecommissions.Checked = boolDecoms;
                    }

                    if (Request.QueryString["t"] != null && Request.QueryString["t"] != "" && Request.QueryString["q"] != null)
                    {
                        btnNew.Visible = true;
                        string strType = Request.QueryString["t"];
                        strQuery = oFunction.decryptQueryString(Request.QueryString["q"]);
                        if (Request.QueryString["multiple"] != null)
                            Page.ClientScript.RegisterStartupScript(typeof(Page), "close", "<script type=\"text/javascript\">alert('There were multiple assets found for " + strQuery + "\\n\\nPlease contact a clearview administrator regarding these assets');<" + "/" + "script>");
                        radSearch.SelectedValue = strType;
                        ddlSearch.SelectedValue = intPerPage.ToString();
                        switch (strType)
                        {
                            case "name":
                                if (strQuery != "" || intMnemonic > 0 || strBuildStart != "" || strBuildEnd != "")
                                {
                                    DataSet dsName = oDataPoint.GetAssetName(strQuery, 0, intMnemonic, strBuildStart, strBuildEnd, (boolDecoms ? 1 : 0));
                                    if (dsName.Tables[0].Rows.Count == 1)
                                        Response.Redirect("/datapoint/asset/" + dsName.Tables[0].Rows[0]["url"].ToString() + ".aspx?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(dsName.Tables[0].Rows[0]["id"].ToString()));
                                    else
                                    {
                                        panName.Visible = true;
                                        LoadRepeater(dsName, rptName, lblName, strQuery);
                                    }
                                }
                                else
                                    boolError = true;
                                break;
                            case "serial":
                                if (strQuery != "")
                                {
                                    DataSet dsSerial = oDataPoint.GetAssetSerialOrTag(strQuery, "");
                                    if (dsSerial.Tables[0].Rows.Count == 1)
                                        Response.Redirect("/datapoint/asset/" + dsSerial.Tables[0].Rows[0]["url"].ToString() + ".aspx?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(dsSerial.Tables[0].Rows[0]["id"].ToString()));
                                    else
                                    {
                                        panSerial.Visible = true;
                                        LoadRepeater(dsSerial, rptSerial, lblSerial, strQuery);
                                    }
                                }
                                else
                                    boolError = true;
                                break;
                            case "tag":
                                if (strQuery != "")
                                {
                                    DataSet dsTag = oDataPoint.GetAssetSerialOrTag("", strQuery);
                                    if (dsTag.Tables[0].Rows.Count == 1)
                                        Response.Redirect("/datapoint/asset/" + dsTag.Tables[0].Rows[0]["url"].ToString() + ".aspx?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(dsTag.Tables[0].Rows[0]["id"].ToString()));
                                    else
                                    {
                                        panTag.Visible = true;
                                        LoadRepeater(dsTag, rptTag, lblTag, strQuery);
                                    }
                                }
                                else
                                    boolError = true;
                                break;
                            case "deploy":
                                DataSet dsDeploy = oDataPoint.GetAssetDeploy(Int32.Parse(strQuery));
                                if (dsDeploy.Tables[0].Rows.Count == 1)
                                    Response.Redirect("/datapoint/asset/" + dsDeploy.Tables[0].Rows[0]["url"].ToString() + ".aspx?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(dsDeploy.Tables[0].Rows[0]["id"].ToString()));
                                else
                                {
                                    panDeploy.Visible = true;
                                    LoadRepeater(dsDeploy, rptDeploy, lblDeploy, oModel.Get(Int32.Parse(strQuery), "name"));
                                }
                                break;
                            default:
                                boolError = true;
                                break;
                        }
                    }
                    else if (Request.Cookies[strCookie] != null && Request.Cookies[strCookie].Value != "")
                        radSearch.SelectedValue = Request.Cookies[strCookie].Value;
                    if (radSearch.SelectedValue == "deploy")
                    {
                        ddlModel.SelectedValue = strQuery;
                        divModel.Style["display"] = "inline";
                    }
                    else
                    {
                        txtSearch.Text = strQuery;
                        divValue.Style["display"] = "inline";
                    }
                    if (boolError == true)
                        Response.Redirect(Request.Path + "?error=true");
                    if (radSearch.SelectedIndex > -1)
                        hdnSearchType.Value = radSearch.SelectedItem.Value;
                }
                if (intLists == 1)
                {
                    lnkSearch.Enabled = true;
                    //AJAXTextBoxGet(oText,oWidth,oHeight,oDiv,oList,oHidden,oRequest,oNumber,oMultiple,param1,param2,oHiddens,oTableAdd)
                    txtSearch.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','145','" + divSearch.ClientID + "','" + lstSearch.ClientID + "','" + hdnSearchText.ClientID + "','" + oVariable.URL() + "/frame/ajax/datapoint/ajax_datapoint_asset.aspx',2,null," + intProfile.ToString() + ",document.getElementById('" + hdnSearchType.ClientID + "').value);");
                    lstSearch.Attributes.Add("ondblclick", "AJAXClickRow();");
                }
                txtSearch.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");
                radSearch.Attributes.Add("onclick", "ChangeCookieSearch(this,'" + strCookie + "','" + divValue.ClientID + "','" + divModel.ClientID + "');UpdateDataPointSearch('" + hdnSearchType.ClientID + "');");
                btnSearch.Attributes.Add("onclick", "return ValidateRadioList('" + radSearch.ClientID + "','Please select a search type')" +
                    " && EnsureAssetSearch('" + txtSearch.ClientID + "','" + txtSearchMnemonic.ClientID + "','" + hdnSearchMnemonic.ClientID + "')" +
                    " && ProcessButton(this)" +
                    ";");
                txtSearchMnemonic.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'500','195','" + divSearchMnemonic.ClientID + "','" + lstSearchMnemonic.ClientID + "','" + hdnSearchMnemonic.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_mnemonics.aspx',2);");
                lstSearchMnemonic.Attributes.Add("ondblclick", "AJAXClickRow();");
                imgBuildStart.Attributes.Add("onclick", "return ShowCalendar('" + txtBuildStart.ClientID + "');");
                imgBuildEnd.Attributes.Add("onclick", "return ShowCalendar('" + txtBuildEnd.ClientID + "');");
            }
            else
                panDenied.Visible = true;
        }
        private void LoadList()
        {
            radSearch.Items.Add(new ListItem("Device Name", "name"));
            radSearch.Items.Add(new ListItem("Serial Number", "serial"));
            radSearch.Items.Add(new ListItem("Asset Tag", "tag"));
            if (oDataPoint.GetFieldPermission(intProfile, strCookie) == true || oUser.IsAdmin(intProfile) == true)
            {
                radSearch.Items.Add(new ListItem("Awaiting Deployment", "deploy"));
                ddlModel.DataValueField = "modelid";
                ddlModel.DataTextField = "name";
                ddlModel.DataSource = oDataPoint.GetDeployModel(intProfile);
                ddlModel.DataBind();
                ddlModel.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            }
        }
        protected void btnAdvanced_Click(Object Sender, EventArgs e)
        {
            Advanced(btnAdvanced.Text == "Advanced Search");
        }
        private void Advanced(bool _advanced)
        {
            if (_advanced == true)
            {
                boolAdvanced = true;
                btnAdvanced.Text = "Basic Search";
            }
            else
            {
                boolAdvanced = false;
                btnAdvanced.Text = "Advanced Search";
            }
        }
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnSearch_Click(Object Sender, EventArgs e)
        {
            if (radSearch.SelectedValue == "deploy")
                Response.Redirect(Request.Path + "?t=" + radSearch.SelectedItem.Value + "&q=" + oFunction.encryptQueryString(ddlModel.SelectedItem.Value) + "&r=" + ddlSearch.SelectedItem.Value);
            else
            {
                DateTime datDate;
                int intSearchMnemonic = 0;
                if (Request.Form[hdnSearchMnemonic.UniqueID] != "")
                    intSearchMnemonic = Int32.Parse(Request.Form[hdnSearchMnemonic.UniqueID]);
                string strBuildStart = "";
                if (DateTime.TryParse(txtBuildStart.Text, out datDate) == true)
                    strBuildStart = oFunction.encryptQueryString(datDate.ToShortDateString());
                string strBuildEnd = "";
                if (DateTime.TryParse(txtBuildEnd.Text, out datDate) == true)
                    strBuildEnd = oFunction.encryptQueryString(datDate.ToShortDateString());
                if (intProfile > 0)
                {
                    oDataPoint.DeleteSearch(txtSearch.Text.Trim(), intProfile, Request.Form[hdnSearchType.UniqueID]);
                    oDataPoint.AddSearch(txtSearch.Text.Trim(), intProfile, Request.Form[hdnSearchType.UniqueID]);
                }
                Response.Redirect(Request.Path + "?t=" + radSearch.SelectedItem.Value + "&q=" + oFunction.encryptQueryString(txtSearch.Text.Trim()) + "&mnem=" + intSearchMnemonic.ToString() + "&bs=" + strBuildStart + "&be=" + strBuildEnd + "&decoms=" + (chkDecommissions.Checked ? "1" : "0") + "&r=" + ddlSearch.SelectedItem.Value);
            }
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            intPage--;
            if (radSearch.SelectedValue == "deploy")
                Response.Redirect(Request.Path + "?t=" + radSearch.SelectedItem.Value + "&q=" + oFunction.encryptQueryString(ddlModel.SelectedItem.Value) + "&r=" + ddlSearch.SelectedItem.Value + "&p=" + intPage.ToString());
            else
                Response.Redirect(Request.Path + "?t=" + radSearch.SelectedItem.Value + "&q=" + oFunction.encryptQueryString(txtSearch.Text) + "&mnem=" + Request.QueryString["mnem"] + "&r=" + ddlSearch.SelectedItem.Value + "&p=" + intPage.ToString());
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            intPage++;
            if (radSearch.SelectedValue == "deploy")
                Response.Redirect(Request.Path + "?t=" + radSearch.SelectedItem.Value + "&q=" + oFunction.encryptQueryString(ddlModel.SelectedItem.Value) + "&r=" + ddlSearch.SelectedItem.Value + "&p=" + intPage.ToString());
            else
                Response.Redirect(Request.Path + "?t=" + radSearch.SelectedItem.Value + "&q=" + oFunction.encryptQueryString(txtSearch.Text) + "&mnem=" + Request.QueryString["mnem"] + "&r=" + ddlSearch.SelectedItem.Value + "&p=" + intPage.ToString());
        }
        private void LoadRepeater(DataSet ds, Repeater rptRepeater, Label lblNone, string strQuery)
        {
            panResults.Visible = true;
            intPage--;
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
        protected void lnkSearch_Click(Object Sender, EventArgs e)
        {
            if (intProfile > 0)
                oDataPoint.DeleteSearch(intProfile);
            if (Request.QueryString["t"] != null && Request.QueryString["q"] != null)
                Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&mnem=" + Request.QueryString["mnem"] + "&r=" + ddlSearch.SelectedItem.Value + "&p=" + intPage.ToString());
            else
                Response.Redirect(Request.Path);
        }
    }
}
