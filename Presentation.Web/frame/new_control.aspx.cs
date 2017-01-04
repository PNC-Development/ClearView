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
using System.Collections.Generic;

namespace NCC.ClearView.Presentation.Web
{
    public partial class new_control : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intProfile;
        protected string strFields = "";
        protected ServiceEditor oServiceEditor;
        protected int intService = 0;
        protected int intWMFlag = 0;
        protected int intField = 0;
        protected int intConfig = 0;
        protected string strCode = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">window.parent.navigate(window.parent.location);<" + "/" + "script>");
            if (Request.QueryString["delete"] != null && Request.QueryString["delete"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "delete", "<script type=\"text/javascript\">window.parent.navigate(window.parent.location);<" + "/" + "script>");
            oServiceEditor = new ServiceEditor(intProfile, dsnServiceEditor);
            if (Request.QueryString["serviceid"] != null && Request.QueryString["serviceid"] != "")
                intService = Int32.Parse(Request.QueryString["serviceid"]);
            if (Request.QueryString["wm"] != null && Request.QueryString["wm"] != "")
                intWMFlag = Int32.Parse(Request.QueryString["wm"]);
            if (intWMFlag == 0)
                panWrite.Visible = true;
            if (Request.QueryString["fieldid"] != null && Request.QueryString["fieldid"] != "")
            {
                panField.Visible = true;
                intField = Int32.Parse(Request.QueryString["fieldid"]);
                DataSet ds = oServiceEditor.GetField(intField);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblField.Text = oServiceEditor.GetField(intField, "name");
                    strCode = oServiceEditor.GetField(intField, "code");
                    ShowPanels();
                    btnSave.Enabled = true;
                    btnSave.Text = "Add";
                    btnBack.Visible = true;
                    btnBack.Attributes.Add("onclick", "return confirm('WARNING: Any unsaved changes will be discarded!\\n\\nAre you sure you want to continue?');");
                }
            }
            else if (Request.QueryString["configid"] != null && Request.QueryString["configid"] != "")
            {
                panField.Visible = true;
                intConfig = Int32.Parse(Request.QueryString["configid"]);
                DataSet ds = oServiceEditor.GetConfig(intConfig);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intField = Int32.Parse(ds.Tables[0].Rows[0]["fieldid"].ToString());
                    lblField.Text = oServiceEditor.GetField(intField, "name");
                    strCode = oServiceEditor.GetField(intField, "code");
                    lblID.Visible = true;
                    lblID.Text = "<b>Field ID:</b> " + ds.Tables[0].Rows[0]["dbfield"].ToString();
                    ShowPanels();
                    btnSave.Enabled = true;
                    btnSave.Text = "Update";
                    btnDelete.Enabled = true;
                    btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this control?');");
                    if (intConfig > 0 && !IsPostBack)
                    {
                        txtQuestion.Text = ds.Tables[0].Rows[0]["question"].ToString();
                        txtURL.Text = ds.Tables[0].Rows[0]["url"].ToString();
                        txtText.Text = ds.Tables[0].Rows[0]["other_text"].ToString();
                        txtLength.Text = ds.Tables[0].Rows[0]["length"].ToString();
                        txtWidth.Text = ds.Tables[0].Rows[0]["width"].ToString();
                        txtRows.Text = ds.Tables[0].Rows[0]["height"].ToString();
                        txtMinimum.Text = ds.Tables[0].Rows[0]["width"].ToString();
                        txtMaximum.Text = ds.Tables[0].Rows[0]["height"].ToString();
                        radCheckYes.Checked = (ds.Tables[0].Rows[0]["checked"].ToString() == "1");
                        radCheckNo.Checked = (ds.Tables[0].Rows[0]["checked"].ToString() == "0");
                        radDirectionH.Checked = (ds.Tables[0].Rows[0]["direction"].ToString() == "1");
                        radDirectionV.Checked = (ds.Tables[0].Rows[0]["direction"].ToString() == "0");
                        radMultipleYes.Checked = (ds.Tables[0].Rows[0]["multiple"].ToString() == "1");
                        radMultipleNo.Checked = (ds.Tables[0].Rows[0]["multiple"].ToString() == "0");
                        if (ds.Tables[0].Rows[0]["required"].ToString() == "1")
                        {
                            radRequiredYes.Checked = true;
                            txtRequired.Text = ds.Tables[0].Rows[0]["required_text"].ToString();
                            divRequired.Style["display"] = "inline";
                        }
                        else
                            radRequiredNo.Checked = true;
                        txtTip.Text = ds.Tables[0].Rows[0]["tip"].ToString();
                        chkWrite.Checked = (ds.Tables[0].Rows[0]["wm"].ToString() == "1");
                        chkWrite.ToolTip = ds.Tables[0].Rows[0]["wm"].ToString();
                        ds = oServiceEditor.GetConfigValues(intConfig);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            lstValues.Items.Add(dr["value"].ToString());
                            hdnValues.Value += dr["value"].ToString() + ";";
                        }
                    }
                }
            }
            else
            {
                panFields.Visible = true;
                DataSet ds = oServiceEditor.GetFields(1);
                StringBuilder sb = new StringBuilder(strFields);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (sb.ToString() != "")
                    {
                        sb.Append("<tr><td colspan=\"2\"><span style=\"width:100%;border-bottom:1 dotted #CCCCCC;\"/></td></tr>");
                    }

                    sb.Append("<tr><td class=\"header\">");
                    sb.Append(dr["name"].ToString());
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td>");
                    sb.Append(dr["description"].ToString());
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td class=\"required\"><a href=\"javascript:void(0);\" onclick=\"ShowHideDivExample('TR");
                    sb.Append(dr["id"].ToString());
                    sb.Append("',this);\">Show Example</a></td></tr>");
                    sb.Append("<tr id=\"TR");
                    sb.Append(dr["id"].ToString());
                    sb.Append("\" style=\"display:none\"><td><img src=\"");
                    sb.Append(dr["image"].ToString());
                    sb.Append("\" border=\"0\"/></td></tr>");
                    sb.Append("<tr><td><input type=\"button\" value=\"Add to Form\" class=\"default\" onclick=\"window.navigate('");
                    sb.Append(Request.Path);
                    sb.Append("?wm=");
                    sb.Append(intWMFlag.ToString());
                    sb.Append("&serviceid=");
                    sb.Append(intService.ToString());
                    sb.Append("&fieldid=");
                    sb.Append(dr["id"].ToString());
                    sb.Append("');\" style=\"width:100px\"/></td></tr>");
                }

                if (sb.ToString() != "")
                {
                    sb.Insert(0, "<table height=\"100%\" border=\"0\" cellpadding=\"4\" cellspacing=\"3\">");
                    sb.Append("</table>");
                }

                strFields = sb.ToString();
            }
            if (txtMinimum.Text == "")
                txtMinimum.Text = "0";
            if (txtMaximum.Text == "")
                txtMaximum.Text = "0";
            radRequiredYes.Attributes.Add("onclick", "ShowHideDiv('" + divRequired.ClientID + "','inline');");
            radRequiredNo.Attributes.Add("onclick", "ShowHideDiv('" + divRequired.ClientID + "','none');");
            txtAdd.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnAdd.ClientID + "').click();return false;}} else {return true}; ");
            btnUp.Attributes.Add("onclick", "return MoveOrderUp(" + lstValues.ClientID + ",'" + hdnValues.ClientID + "');");
            btnOut.Attributes.Add("onclick", "return MoveOrderOut(" + lstValues.ClientID + ",'" + hdnValues.ClientID + "');");
            btnDown.Attributes.Add("onclick", "return MoveOrderDown(" + lstValues.ClientID + ",'" + hdnValues.ClientID + "');");
            btnAdd.Attributes.Add("onclick", "return ValidateText('" + txtAdd.ClientID + "','Please enter a value') && ValidateNoComma('" + txtAdd.ClientID + "','The value cannot contain a comma (,)\\n\\nPlease click OK and remove all commas from the value field') && ValidateNoDuplicate('" + txtAdd.ClientID + "','" + lstValues.ClientID + "','This value already exists. You cannot add the same value more than once.\\n\\nPlease change the value and try again.') && MoveOrderIn(" + lstValues.ClientID + ",'" + hdnValues.ClientID + "','" + txtAdd.ClientID + "');");
        }
        private void ShowPanels()
        {
            panQuestion.Visible = false;
            panValues.Visible = false;
            panLength.Visible = false;
            panWidth.Visible = false;
            panRows.Visible = false;
            panMultiple.Visible = false;
            panCheck.Visible = false;
            panDirection.Visible = false;
            panRequired.Visible = false;
            panTip.Visible = false;
            switch (strCode.ToUpper())
            {
                case "DATE":
                    panQuestion.Visible = true;
                    panRange.Visible = true;
                    panRequired.Visible = true;
                    panTip.Visible = true;
                    btnSave.Attributes.Add("onclick", "return ValidateText('" + txtQuestion.ClientID + "','Please enter a question')" +
                        " && ValidateNumber('" + txtMinimum.ClientID + "','Please enter a valid number')" +
                        " && ValidateNumber('" + txtMaximum.ClientID + "','Please enter a valid number')" +
                        " && ValidateRadioButtons('" + radRequiredYes.ClientID + "','" + radRequiredNo.ClientID + "','Please select if you want this control to be required')" +
                        " && (document.getElementById('" + radRequiredYes.ClientID + "').checked == false || (document.getElementById('" + radRequiredYes.ClientID + "').checked == true && ValidateText('" + txtRequired.ClientID + "','Please enter a message to display to the user if this field is incomplete')))" +
                        ";");
                    break;
                case "TEXTBOX":
                    panQuestion.Visible = true;
                    panLength.Visible = true;
                    panWidth.Visible = true;
                    panRequired.Visible = true;
                    panTip.Visible = true;
                    btnSave.Attributes.Add("onclick", "return ValidateText('" + txtQuestion.ClientID + "','Please enter a question')" +
                        " && ValidateNumber0('" + txtLength.ClientID + "','Please enter a valid number for the length')" +
                        " && ValidateNumber0('" + txtWidth.ClientID + "','Please enter a valid number for the width')" +
                        " && ValidateRadioButtons('" + radRequiredYes.ClientID + "','" + radRequiredNo.ClientID + "','Please select if you want this control to be required')" +
                        " && (document.getElementById('" + radRequiredYes.ClientID + "').checked == false || (document.getElementById('" + radRequiredYes.ClientID + "').checked == true && ValidateText('" + txtRequired.ClientID + "','Please enter a message to display to the user if this field is incomplete')))" +
                        ";");
                    break;
                case "TEXTAREA":
                    panQuestion.Visible = true;
                    panWidth.Visible = true;
                    panRows.Visible = true;
                    panRequired.Visible = true;
                    panTip.Visible = true;
                    btnSave.Attributes.Add("onclick", "return ValidateText('" + txtQuestion.ClientID + "','Please enter a question')" +
                        " && ValidateNumber0('" + txtWidth.ClientID + "','Please enter a valid number for the width')" +
                        " && ValidateNumber1('" + txtRows.ClientID + "','Please enter a valid number for the number of rows')" +
                        " && ValidateRadioButtons('" + radRequiredYes.ClientID + "','" + radRequiredNo.ClientID + "','Please select if you want this control to be required')" +
                        " && (document.getElementById('" + radRequiredYes.ClientID + "').checked == false || (document.getElementById('" + radRequiredYes.ClientID + "').checked == true && ValidateText('" + txtRequired.ClientID + "','Please enter a message to display to the user if this field is incomplete')))" +
                        ";");
                    break;
                case "USERS":
                case "SERVERS":
                case "MNEMONICS":
                case "APPROVER":
                    panQuestion.Visible = true;
                    panWidth.Visible = true;
                    panRows.Visible = true;
                    panRequired.Visible = true;
                    panTip.Visible = true;
                    btnSave.Attributes.Add("onclick", "return ValidateText('" + txtQuestion.ClientID + "','Please enter a question')" +
                        " && ValidateNumber0('" + txtWidth.ClientID + "','Please enter a valid number for the width')" +
                        " && ValidateNumber1('" + txtRows.ClientID + "','Please enter a valid number for the number of rows')" +
                        " && ValidateRadioButtons('" + radRequiredYes.ClientID + "','" + radRequiredNo.ClientID + "','Please select if you want this control to be required')" +
                        " && (document.getElementById('" + radRequiredYes.ClientID + "').checked == false || (document.getElementById('" + radRequiredYes.ClientID + "').checked == true && ValidateText('" + txtRequired.ClientID + "','Please enter a message to display to the user if this field is incomplete')))" +
                        ";");
                    break;
                case "CHECKBOX":
                    panQuestion.Visible = true;
                    panCheck.Visible = true;
                    panTip.Visible = true;
                    btnSave.Attributes.Add("onclick", "return ValidateText('" + txtQuestion.ClientID + "','Please enter a question')" +
                        " && ValidateRadioButtons('" + radCheckYes.ClientID + "','" + radCheckNo.ClientID + "','Please select if you want this control to be checked by default')" +
                        ";");
                    break;
                case "CHECKLIST":
                    panQuestion.Visible = true;
                    panValues.Visible = true;
                    panDirection.Visible = true;
                    panRequired.Visible = true;
                    panTip.Visible = true;
                    btnSave.Attributes.Add("onclick", "return ValidateText('" + txtQuestion.ClientID + "','Please enter a question')" +
                        " && ValidateHidden('" + hdnValues.ClientID + "','" + txtAdd.ClientID + "','Please enter at least one (1) value')" +
                        " && ValidateRadioButtons('" + radDirectionV.ClientID + "','" + radDirectionH.ClientID + "','Please select the direction of the control')" +
                        " && ValidateRadioButtons('" + radRequiredYes.ClientID + "','" + radRequiredNo.ClientID + "','Please select if you want this control to be required')" +
                        " && (document.getElementById('" + radRequiredYes.ClientID + "').checked == false || (document.getElementById('" + radRequiredYes.ClientID + "').checked == true && ValidateText('" + txtRequired.ClientID + "','Please enter a message to display to the user if this field is incomplete')))" +
                        ";");
                    break;
                case "DROPDOWN":
                    panQuestion.Visible = true;
                    panValues.Visible = true;
                    panWidth.Visible = true;
                    panRequired.Visible = true;
                    panTip.Visible = true;
                    btnSave.Attributes.Add("onclick", "return ValidateText('" + txtQuestion.ClientID + "','Please enter a question')" +
                        " && ValidateHidden('" + hdnValues.ClientID + "','" + txtAdd.ClientID + "','Please enter at least one (1) value')" +
                        " && ValidateNumber0('" + txtWidth.ClientID + "','Please enter a valid number for the width')" +
                        " && ValidateRadioButtons('" + radRequiredYes.ClientID + "','" + radRequiredNo.ClientID + "','Please select if you want this control to be required')" +
                        " && (document.getElementById('" + radRequiredYes.ClientID + "').checked == false || (document.getElementById('" + radRequiredYes.ClientID + "').checked == true && ValidateText('" + txtRequired.ClientID + "','Please enter a message to display to the user if this field is incomplete')))" +
                        ";");
                    break;
                case "RADIOLIST":
                    panQuestion.Visible = true;
                    panValues.Visible = true;
                    panDirection.Visible = true;
                    panRequired.Visible = true;
                    panTip.Visible = true;
                    btnSave.Attributes.Add("onclick", "return ValidateText('" + txtQuestion.ClientID + "','Please enter a question')" +
                        " && ValidateHidden('" + hdnValues.ClientID + "','" + txtAdd.ClientID + "','Please enter at least one (1) value')" +
                        " && ValidateRadioButtons('" + radDirectionV.ClientID + "','" + radDirectionH.ClientID + "','Please select the direction of the control')" +
                        " && ValidateRadioButtons('" + radRequiredYes.ClientID + "','" + radRequiredNo.ClientID + "','Please select if you want this control to be required')" +
                        " && (document.getElementById('" + radRequiredYes.ClientID + "').checked == false || (document.getElementById('" + radRequiredYes.ClientID + "').checked == true && ValidateText('" + txtRequired.ClientID + "','Please enter a message to display to the user if this field is incomplete')))" +
                        ";");
                    break;
                case "LISTBOX":
                    panQuestion.Visible = true;
                    panValues.Visible = true;
                    panWidth.Visible = true;
                    panRows.Visible = true;
                    panMultiple.Visible = true;
                    panRequired.Visible = true;
                    panTip.Visible = true;
                    btnSave.Attributes.Add("onclick", "return ValidateText('" + txtQuestion.ClientID + "','Please enter a question')" +
                        " && ValidateHidden('" + hdnValues.ClientID + "','" + txtAdd.ClientID + "','Please enter at least one (1) value')" +
                        " && ValidateNumber0('" + txtWidth.ClientID + "','Please enter a valid number for the width')" +
                        " && ValidateNumber1('" + txtRows.ClientID + "','Please enter a valid number for number of rows')" +
                        " && ValidateRadioButtons('" + radMultipleYes.ClientID + "','" + radMultipleNo.ClientID + "','Please select if you want the client to be able to select multiple values')" +
                        " && ValidateRadioButtons('" + radRequiredYes.ClientID + "','" + radRequiredNo.ClientID + "','Please select if you want this control to be required')" +
                        " && (document.getElementById('" + radRequiredYes.ClientID + "').checked == false || (document.getElementById('" + radRequiredYes.ClientID + "').checked == true && ValidateText('" + txtRequired.ClientID + "','Please enter a message to display to the user if this field is incomplete')))" +
                        ";");
                    break;
                case "LIST":
                case "SERVERLIST":
                    panQuestion.Visible = true;
                    panLength.Visible = true;
                    panWidth.Visible = true;
                    panRows.Visible = true;
                    panRequired.Visible = true;
                    panTip.Visible = true;
                    btnSave.Attributes.Add("onclick", "return ValidateText('" + txtQuestion.ClientID + "','Please enter a question')" +
                        " && ValidateNumber0('" + txtLength.ClientID + "','Please enter a valid number for the length')" +
                        " && ValidateNumber0('" + txtWidth.ClientID + "','Please enter a valid number for the width')" +
                        " && ValidateNumber1('" + txtRows.ClientID + "','Please enter a valid number for number of rows')" +
                        " && ValidateRadioButtons('" + radRequiredYes.ClientID + "','" + radRequiredNo.ClientID + "','Please select if you want this control to be required')" +
                        " && (document.getElementById('" + radRequiredYes.ClientID + "').checked == false || (document.getElementById('" + radRequiredYes.ClientID + "').checked == true && ValidateText('" + txtRequired.ClientID + "','Please enter a message to display to the user if this field is incomplete')))" +
                        ";");
                    break;
                case "HYPERLINK":
                    panQuestion.Visible = true;
                    panWidth.Visible = true;
                    panRequired.Visible = true;
                    panTip.Visible = true;
                    btnSave.Attributes.Add("onclick", "return ValidateText('" + txtQuestion.ClientID + "','Please enter a question')" +
                        " && ValidateNumber0('" + txtWidth.ClientID + "','Please enter a valid number for the width')" +
                        " && ValidateRadioButtons('" + radRequiredYes.ClientID + "','" + radRequiredNo.ClientID + "','Please select if you want this control to be required')" +
                        " && (document.getElementById('" + radRequiredYes.ClientID + "').checked == false || (document.getElementById('" + radRequiredYes.ClientID + "').checked == true && ValidateText('" + txtRequired.ClientID + "','Please enter a message to display to the user if this field is incomplete')))" +
                        ";");
                    break;
                case "FILEUPLOAD":
                    panQuestion.Visible = true;
                    panWidth.Visible = true;
                    panRequired.Visible = true;
                    panTip.Visible = true;
                    btnSave.Attributes.Add("onclick", "return ValidateText('" + txtQuestion.ClientID + "','Please enter a question')" +
                        " && ValidateNumber0('" + txtWidth.ClientID + "','Please enter a valid number for the width')" +
                        " && ValidateRadioButtons('" + radRequiredYes.ClientID + "','" + radRequiredNo.ClientID + "','Please select if you want this control to be required')" +
                        " && (document.getElementById('" + radRequiredYes.ClientID + "').checked == false || (document.getElementById('" + radRequiredYes.ClientID + "').checked == true && ValidateText('" + txtRequired.ClientID + "','Please enter a message to display to the user if this field is incomplete')))" +
                        ";");
                    break;
                case "DISCLAIMER":
                    panQuestion.Visible = true;
                    panURL.Visible = true;
                    panText.Visible = true;
                    litQuestion.Text = "Disclaimer Text";
                    litText.Text = "Highlighted Text";
                    txtText.Width = Unit.Pixel(300);
                    panRequired.Visible = true;
                    panTip.Visible = true;
                    btnSave.Attributes.Add("onclick", "return ValidateText('" + txtQuestion.ClientID + "','Please enter a question')" +
                        " && ValidateNumber0('" + txtWidth.ClientID + "','Please enter a valid number for the width')" +
                        " && ValidateRadioButtons('" + radRequiredYes.ClientID + "','" + radRequiredNo.ClientID + "','Please select if you want this control to be required')" +
                        " && (document.getElementById('" + radRequiredYes.ClientID + "').checked == false || (document.getElementById('" + radRequiredYes.ClientID + "').checked == true && ValidateText('" + txtRequired.ClientID + "','Please enter a message to display to the user if this field is incomplete')))" +
                        ";");
                    break;
                case "LOCATION":
                    Locations oLocation = new Locations(0, dsn);
                    ddlLocation.DataTextField = "commonname";
                    ddlLocation.DataValueField = "id";
                    ddlLocation.DataSource = oLocation.GetAddressCommon();
                    ddlLocation.DataBind();
                    ddlLocation.Items.Insert(0, new ListItem("-- SELECT --", "0"));

                    panQuestion.Visible = true;
                    panLocation.Visible = true;
                    panRequired.Visible = true;
                    panTip.Visible = true;
                    btnSave.Attributes.Add("onclick", "return ValidateText('" + txtQuestion.ClientID + "','Please enter a question')" +
                        " && ValidateDropDown('" + ddlLocation.ClientID + "','Please select a default location')" +
                        " && ValidateRadioButtons('" + radRequiredYes.ClientID + "','" + radRequiredNo.ClientID + "','Please select if you want this control to be required')" +
                        " && (document.getElementById('" + radRequiredYes.ClientID + "').checked == false || (document.getElementById('" + radRequiredYes.ClientID + "').checked == true && ValidateText('" + txtRequired.ClientID + "','Please enter a message to display to the user if this field is incomplete')))" +
                        ";");
                    break;
            }
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?wm=" + intWMFlag.ToString() + "&serviceid=" + intService.ToString());
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intWM = (intWMFlag == 1 || chkWrite.Checked ? 1 : 0);
            if (intConfig == 0)
            {
                int intOrder = oServiceEditor.GetConfigs(intService, intWM, 0).Tables[0].Rows.Count + 1;
                switch (strCode.ToUpper())
                {
                    case "DATE":
                        intConfig = oServiceEditor.AddConfig(intService, intField, intWM, txtQuestion.Text, -1, Int32.Parse(txtMinimum.Text), Int32.Parse(txtMaximum.Text), -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", intOrder, 1);
                        break;
                    case "TEXTBOX":
                        intConfig = oServiceEditor.AddConfig(intService, intField, intWM, txtQuestion.Text, Int32.Parse(txtLength.Text), Int32.Parse(txtWidth.Text), -1, -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", intOrder, 1);
                        break;
                    case "TEXTAREA":
                        intConfig = oServiceEditor.AddConfig(intService, intField, intWM, txtQuestion.Text, -1, Int32.Parse(txtWidth.Text), Int32.Parse(txtRows.Text), -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", intOrder, 1);
                        break;
                    case "USERS":
                    case "SERVERS":
                    case "MNEMONICS":
                    case "APPROVER":
                        intConfig = oServiceEditor.AddConfig(intService, intField, intWM, txtQuestion.Text, -1, Int32.Parse(txtWidth.Text), Int32.Parse(txtRows.Text), -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", intOrder, 1);
                        break;
                    case "CHECKBOX":
                        intConfig = oServiceEditor.AddConfig(intService, intField, intWM, txtQuestion.Text, -1, -1, -1, (radCheckYes.Checked ? 1 : 0), -1, -1, txtTip.Text, 0, "", "", "", intOrder, 1);
                        break;
                    case "CHECKLIST":
                        intConfig = oServiceEditor.AddConfig(intService, intField, intWM, txtQuestion.Text, 1, -1, -1, -1, (radDirectionH.Checked ? 1 : 0), -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", intOrder, 1);
                        break;
                    case "DROPDOWN":
                        intConfig = oServiceEditor.AddConfig(intService, intField, intWM, txtQuestion.Text, -1, Int32.Parse(txtWidth.Text), 0, -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", intOrder, 1);
                        break;
                    case "RADIOLIST":
                        intConfig = oServiceEditor.AddConfig(intService, intField, intWM, txtQuestion.Text, 1, -1, -1, -1, (radDirectionH.Checked ? 1 : 0), -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", intOrder, 1);
                        break;
                    case "LISTBOX":
                        intConfig = oServiceEditor.AddConfig(intService, intField, intWM, txtQuestion.Text, -1, Int32.Parse(txtWidth.Text), Int32.Parse(txtRows.Text), -1, -1, (radMultipleYes.Checked ? 1 : 0), txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", intOrder, 1);
                        break;
                    case "LIST":
                    case "SERVERLIST":
                        intConfig = oServiceEditor.AddConfig(intService, intField, intWM, txtQuestion.Text, Int32.Parse(txtLength.Text), Int32.Parse(txtWidth.Text), Int32.Parse(txtRows.Text), -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", intOrder, 1);
                        break;
                    case "HYPERLINK":
                        intConfig = oServiceEditor.AddConfig(intService, intField, intWM, txtQuestion.Text, -1, Int32.Parse(txtWidth.Text), -1, -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", intOrder, 1);
                        break;
                    case "FILEUPLOAD":
                        intConfig = oServiceEditor.AddConfig(intService, intField, intWM, txtQuestion.Text, -1, Int32.Parse(txtWidth.Text), -1, -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", intOrder, 1);
                        break;
                    case "DISCLAIMER":
                        intConfig = oServiceEditor.AddConfig(intService, intField, intWM, txtQuestion.Text, -1, -1, -1, -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, txtURL.Text, txtText.Text, intOrder, 1);
                        break;
                    case "LOCATION":
                        intConfig = oServiceEditor.AddConfig(intService, intField, intWM, txtQuestion.Text, -1, Int32.Parse(ddlLocation.SelectedItem.Value), -1, -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", intOrder, 1);
                        break;
                }
            }
            else
            {
                switch (strCode.ToUpper())
                {
                    case "DATE":
                        oServiceEditor.UpdateConfig(intConfig, intWM, txtQuestion.Text, -1, Int32.Parse(txtMinimum.Text), Int32.Parse(txtMaximum.Text), -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", 1);
                        break;
                    case "TEXTBOX":
                        oServiceEditor.UpdateConfig(intConfig, intWM, txtQuestion.Text, Int32.Parse(txtLength.Text), Int32.Parse(txtWidth.Text), -1, -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", 1);
                        break;
                    case "TEXTAREA":
                        oServiceEditor.UpdateConfig(intConfig, intWM, txtQuestion.Text, -1, Int32.Parse(txtWidth.Text), Int32.Parse(txtRows.Text), -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", 1);
                        break;
                    case "USERS":
                    case "SERVERS":
                    case "MNEMONICS":
                    case "APPROVER":
                        oServiceEditor.UpdateConfig(intConfig, intWM, txtQuestion.Text, -1, Int32.Parse(txtWidth.Text), Int32.Parse(txtRows.Text), -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", 1);
                        break;
                    case "CHECKBOX":
                        oServiceEditor.UpdateConfig(intConfig, intWM, txtQuestion.Text, -1, -1, -1, (radCheckYes.Checked ? 1 : 0), -1, -1, txtTip.Text, 0, "", "", "", 1);
                        break;
                    case "CHECKLIST":
                        oServiceEditor.UpdateConfig(intConfig, intWM, txtQuestion.Text, 1, -1, -1, -1, (radDirectionH.Checked ? 1 : 0), -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", 1);
                        break;
                    case "DROPDOWN":
                        oServiceEditor.UpdateConfig(intConfig, intWM, txtQuestion.Text, -1, Int32.Parse(txtWidth.Text), 0, -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", 1);
                        break;
                    case "RADIOLIST":
                        oServiceEditor.UpdateConfig(intConfig, intWM, txtQuestion.Text, 1, -1, -1, -1, (radDirectionH.Checked ? 1 : 0), -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", 1);
                        break;
                    case "LISTBOX":
                        oServiceEditor.UpdateConfig(intConfig, intWM, txtQuestion.Text, -1, Int32.Parse(txtWidth.Text), Int32.Parse(txtRows.Text), -1, -1, (radMultipleYes.Checked ? 1 : 0), txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", 1);
                        break;
                    case "LIST":
                    case "SERVERLIST":
                        oServiceEditor.UpdateConfig(intConfig, intWM, txtQuestion.Text, Int32.Parse(txtLength.Text), Int32.Parse(txtWidth.Text), Int32.Parse(txtRows.Text), -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", 1);
                        break;
                    case "HYPERLINK":
                        oServiceEditor.UpdateConfig(intConfig, intWM, txtQuestion.Text, -1, Int32.Parse(txtWidth.Text), -1, -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", 1);
                        break;
                    case "FILEUPLOAD":
                        oServiceEditor.UpdateConfig(intConfig, intWM, txtQuestion.Text, -1, Int32.Parse(txtWidth.Text), -1, -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", 1);
                        break;
                    case "DISCLAIMER":
                        oServiceEditor.UpdateConfig(intConfig, intWM, txtQuestion.Text, -1, -1, -1, -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, txtURL.Text, txtText.Text, 1);
                        break;
                    case "LOCATION":
                        oServiceEditor.UpdateConfig(intConfig, intWM, txtQuestion.Text, -1, Int32.Parse(ddlLocation.SelectedItem.Value), -1, -1, -1, -1, txtTip.Text, (radRequiredYes.Checked ? 1 : 0), txtRequired.Text, "", "", 1);
                        break;
                }
            }

            //oServiceEditor.DeleteConfigValues(intConfig);
            List<int> lstValues = new List<int>();
            string[] strValues = Request.Form[hdnValues.UniqueID].Split(new char[]{';'});
            DataSet dsValues = oServiceEditor.GetConfigValues(intConfig);
            for (int ii = 0; ii < strValues.Length; ii++)
            {
                if (strValues[ii] != "")
                {
                    // Loop through the values
                    int intValue = 0;
                    foreach (DataRow drValue in dsValues.Tables[0].Rows)
                    {
                        // Try to find matching, existing value
                        if (drValue["value"].ToString() == strValues[ii])
                        {
                            intValue = Int32.Parse(drValue["id"].ToString());
                            break;
                        }
                    }
                    if (intValue > 0)
                    {
                        // If match found, update with new order
                        lstValues.Add(intValue);
                        oServiceEditor.UpdateConfigValue(intValue, ii + 1);
                    }
                    else
                    {
                        // No match, add
                        oServiceEditor.AddConfigValue(intConfig, strValues[ii], ii + 1);
                    }
                }
            }
            // Delete the ones that were not added or selected
            foreach (DataRow drValue in dsValues.Tables[0].Rows)
            {
                int intValue = Int32.Parse(drValue["id"].ToString());
                bool boolDelete = true;
                // Try to find matching, existing value
                foreach (int intExists in lstValues)
                {
                    if (intExists == intValue)
                    {
                        boolDelete = false;
                        break;
                    }
                }
                if (boolDelete)
                    oServiceEditor.DeleteConfigValue(intValue);
            }

            /*
            string strValues = Request.Form[hdnValues.UniqueID];
            int intDisplay = 0;
            while (strValues != "")
            {
                intDisplay++;
                string strValue = strValues.Substring(0, strValues.IndexOf(";"));
                strValues = strValues.Substring(strValues.IndexOf(";") + 1);
                oServiceEditor.AddConfigValue(intConfig, strValue, intDisplay);
            }
            */
            // Check Workload Manager Flag
            if (intWMFlag == 0 && intWM.ToString() != chkWrite.ToolTip.ToString())
            {
                int intDBField_Read = 0;
                Int32.TryParse(oServiceEditor.GetConfig(intConfig, "dbfield"), out intDBField_Read);
                if (intDBField_Read > 0)
                {
                    // A modification was made (either add or remove)
                    if (intWM == 1)
                        oServiceEditor.AddConfigWorkflowShared(intDBField_Read);
                    else
                        oServiceEditor.DeleteConfigWorkflowShared(intDBField_Read);
                }
            }
            oServiceEditor.AlterTable(intService);
            Response.Redirect(Request.Path + "?save=true");
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oServiceEditor.DeleteConfig(intConfig);
            oServiceEditor.AlterTable(intService);
            Response.Redirect(Request.Path + "?delete=true");
        }
    }
}
