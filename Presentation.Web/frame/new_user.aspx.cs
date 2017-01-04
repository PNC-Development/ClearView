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
using System.DirectoryServices;

namespace NCC.ClearView.Presentation.Web
{
    public partial class new_user : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Users oUser;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            oUser = new Users(intProfile, dsn);

            if (!IsPostBack)
            {
                if (Request.QueryString["s"] != null && Request.QueryString["s"] != "")
                {
                    txtXID.Text = Request.QueryString["s"];
                    string strSearch = txtXID.Text;
                    Functions oFunction = new Functions(intProfile, dsn, intEnvironment);
                    SearchResultCollection oCollection = oFunction.eDirectory(strSearch);
                    int intCount = oCollection.Count;
                    if (intCount > 0)
                    {
                        btnSave.Visible = true;
                        bool boolOther = false;
                        lblResult.Text = "<b>Your search for \"" + strSearch + "\" returned " + intCount.ToString() + " results...</b>";
                        TableRow oRow = new TableRow();
                        TableCell oCell = new TableCell();
                        oRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
                        oCell = new TableCell();
                        oCell.Text = "<b>Select...</b>";
                        oRow.Cells.Add(oCell);
                        oCell = new TableCell();
                        oCell.Text = "<b>X-ID</b>";
                        oRow.Cells.Add(oCell);
                        oCell = new TableCell();
                        oCell.Text = "<b>PNC ID</b>";
                        oRow.Cells.Add(oCell);
                        oCell = new TableCell();
                        oCell.Text = "<b>First Name</b>";
                        oRow.Cells.Add(oCell);
                        oCell = new TableCell();
                        oCell.Text = "<b>Last Name</b>";
                        oRow.Cells.Add(oCell);
                        tblResults.Rows.Add(oRow);
                        tblResults.Style["border"] = "solid 1px #CCCCCC";
                        int intCounter = 0;
                        foreach (SearchResult oResult in oCollection)
                        {
                            if (oResult.Properties.Contains("businesscategory") == true || oResult.Properties.Contains("cn") == true || oResult.Properties.Contains("mail") == true)
                            {
                                intCounter++;
                                string strXid = "";
                                if (oResult.Properties.Contains("businesscategory") == true)
                                    strXid = oResult.GetDirectoryEntry().Properties["businesscategory"].Value.ToString();
                                string strId = "";
                                if (oResult.Properties.Contains("cn") == true)
                                    strId = oResult.GetDirectoryEntry().Properties["cn"].Value.ToString();

                                if (strId == "")
                                    strId = strXid;
                                if (strXid == "")
                                    strXid = strId;

                                string strFName = "";
                                if (oResult.Properties.Contains("givenname") == true)
                                    strFName = oResult.GetDirectoryEntry().Properties["givenname"].Value.ToString();
                                string strLName = "";
                                if (oResult.Properties.Contains("sn") == true)
                                    strLName = oResult.GetDirectoryEntry().Properties["sn"].Value.ToString();
                                oRow = new TableRow();
                                oCell = new TableCell();
                                if (oUser.Gets(strXid, 0).Tables[0].Rows.Count == 0)
                                {
                                    CheckBox oCheck = new CheckBox();
                                    oCheck.Text = "Check&nbsp;to&nbsp;Register";
                                    oCheck.Attributes.Add("onclick", "ADCheck(this,'" + strXid + "','" + strId + "','txtFName_" + intCounter.ToString() + "','txtLName_" + intCounter.ToString() + "','" + hdnUsers.ClientID + "');");
                                    oCell.Controls.Add(oCheck);
                                }
                                else
                                {
                                    oCell.Text = "<img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\"/>&nbsp;Already&nbsp;Registered";
                                }
                                oRow.Cells.Add(oCell);
                                oCell = new TableCell();
                                oCell.Text = strXid;
                                oRow.Cells.Add(oCell);
                                oCell = new TableCell();
                                oCell.Text = strId;
                                oRow.Cells.Add(oCell);
                                oCell = new TableCell();
                                oCell.Text += "<input name=\"txtFName_" + intCounter.ToString() + "\" type=\"text\" value=\"" + strFName + "\" maxlength=\"100\" id=\"txtFName_" + intCounter.ToString() + "\" class=\"default\" style=\"width:125px;\" />";
                                oRow.Cells.Add(oCell);
                                oCell = new TableCell();
                                oCell.Text += "<input name=\"txtLName_" + intCounter.ToString() + "\" type=\"text\" value=\"" + strLName + "\" maxlength=\"100\" id=\"txtLName_" + intCounter.ToString() + "\" class=\"default\" style=\"width:125px;\" />";
                                oRow.Cells.Add(oCell);
                                if (boolOther == true)
                                    oRow.BackColor = System.Drawing.Color.WhiteSmoke;
                                boolOther = (!boolOther);
                                tblResults.Rows.Add(oRow);
                            }
                        }
                    }
                    else
                    {
                        AD oAD = new AD(intProfile, dsn, intEnvironment);
                        SearchResultCollection oResults = oAD.Search(strSearch, "cn");
                        if (oResults.Count == 0)
                        {
                            // Search PNC IDs
                            oResults = oAD.Search(strSearch, "extensionattribute10");
                        }

                        intCount = oResults.Count;
                        if (intCount == 0)
                        {
                            lblResult.Text = "<b>Your search for \"" + strSearch + "\" did not return any results...</b>";
                            btnSave.Visible = false;
                        }
                        if (intCount > 0)
                        {
                            btnSave.Visible = true;
                            bool boolOther = false;
                            lblResult.Text = "<b>Your search for \"" + strSearch + "\" returned " + intCount.ToString() + " results...</b>";
                            TableRow oRow = new TableRow();
                            TableCell oCell = new TableCell();
                            oRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
                            oCell = new TableCell();
                            oCell.Text = "<b>Select...</b>";
                            oRow.Cells.Add(oCell);
                            oCell = new TableCell();
                            oCell.Text = "<b>X-ID</b>";
                            oRow.Cells.Add(oCell);
                            oCell = new TableCell();
                            oCell.Text = "<b>PNC ID</b>";
                            oRow.Cells.Add(oCell);
                            oCell = new TableCell();
                            oCell.Text = "<b>First Name</b>";
                            oRow.Cells.Add(oCell);
                            oCell = new TableCell();
                            oCell.Text = "<b>Last Name</b>";
                            oRow.Cells.Add(oCell);
                            tblResults.Rows.Add(oRow);
                            tblResults.Style["border"] = "solid 1px #CCCCCC";
                            int intCounter = 0;
                            foreach (SearchResult oResult in oResults)
                            {
                                if (oResult.Properties.Contains("extensionattribute10") == true || oResult.Properties.Contains("sAMAccountName") == true || oResult.Properties.Contains("mailnickname") == true)
                                {
                                    intCounter++;
                                    string strId = "";
                                    if (oResult.Properties.Contains("extensionattribute10") == true)
                                        strId = oResult.GetDirectoryEntry().Properties["extensionattribute10"].Value.ToString();
                                    string strXid = "";
                                    if (oResult.Properties.Contains("sAMAccountName") == true)
                                        strXid = oResult.GetDirectoryEntry().Properties["sAMAccountName"].Value.ToString();
                                    else if (oResult.Properties.Contains("mailnickname") == true)
                                        strXid = oResult.GetDirectoryEntry().Properties["mailnickname"].Value.ToString();

                                    if (strId == "")
                                        strId = strXid;
                                    if (strXid == "")
                                        strXid = strId;

                                    string strFName = "";
                                    if (oResult.Properties.Contains("givenname") == true)
                                        strFName = oResult.GetDirectoryEntry().Properties["givenname"].Value.ToString();
                                    string strLName = "";
                                    if (oResult.Properties.Contains("sn") == true)
                                        strLName = oResult.GetDirectoryEntry().Properties["sn"].Value.ToString();
                                    oRow = new TableRow();
                                    oCell = new TableCell();
                                    if (oUser.Gets(strXid, 0).Tables[0].Rows.Count == 0)
                                    {
                                        CheckBox oCheck = new CheckBox();
                                        oCheck.Text = "Check&nbsp;to&nbsp;Register";
                                        oCheck.Attributes.Add("onclick", "ADCheck(this,'" + strXid + "','" + strId + "','txtFName_" + intCounter.ToString() + "','txtLName_" + intCounter.ToString() + "','" + hdnUsers.ClientID + "');");
                                        oCell.Controls.Add(oCheck);
                                    }
                                    else
                                    {
                                        oCell.Text = "<img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\"/>&nbsp;Already&nbsp;Registered";
                                    }
                                    oRow.Cells.Add(oCell);
                                    oCell = new TableCell();
                                    oCell.Text = strXid;
                                    oRow.Cells.Add(oCell);
                                    oCell = new TableCell();
                                    oCell.Text = strId;
                                    oRow.Cells.Add(oCell);
                                    oCell = new TableCell();
                                    oCell.Text += "<input name=\"txtFName_" + intCounter.ToString() + "\" type=\"text\" value=\"" + strFName + "\" maxlength=\"100\" id=\"txtFName_" + intCounter.ToString() + "\" class=\"default\" style=\"width:125px;\" />";
                                    oRow.Cells.Add(oCell);
                                    oCell = new TableCell();
                                    oCell.Text += "<input name=\"txtLName_" + intCounter.ToString() + "\" type=\"text\" value=\"" + strLName + "\" maxlength=\"100\" id=\"txtLName_" + intCounter.ToString() + "\" class=\"default\" style=\"width:125px;\" />";
                                    oRow.Cells.Add(oCell);
                                    if (boolOther == true)
                                        oRow.BackColor = System.Drawing.Color.WhiteSmoke;
                                    boolOther = (!boolOther);
                                    tblResults.Rows.Add(oRow);
                                }
                            }
                        }
                    }
                }
            }
            txtXID.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSubmit.ClientID + "').click();return false;}} else {return true}; ");
            btnSubmit.Attributes.Add("onclick", "return ValidateText('" + txtXID.ClientID + "','Please enter a valid LAN ID') && ProcessButtons(this);");
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?s=" + txtXID.Text);
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            string strUsers = Request.Form[hdnUsers.UniqueID];
            while (strUsers != "")
            {
                string strUserName = strUsers.Substring(0, strUsers.IndexOf("&"));
                strUsers = strUsers.Substring(strUsers.IndexOf("&") + 1);
                string strPNC = strUsers.Substring(0, strUsers.IndexOf("&"));
                strUsers = strUsers.Substring(strUsers.IndexOf("&") + 1);
                string strFname = strUsers.Substring(0, strUsers.IndexOf("&"));
                strUsers = strUsers.Substring(strUsers.IndexOf("&") + 1);
                string strLname = strUsers.Substring(0, strUsers.IndexOf("&"));
                strUsers = strUsers.Substring(strUsers.IndexOf("&&") + 2);
                if (oUser.Gets(strUserName, 0).Tables[0].Rows.Count == 0)
                    oUser.Add(strUserName, strPNC, strFname, strLname, 0, 0, 0, 0, "", 0, "", "", 0, 0, 0, 0, 1);
            }
            lblResult.Text = "<img src='/images/alert.gif' border='0' align='absmiddle'> Accounts have been created successfully!";
            btnSave.Visible = false;
        }
    }
}
