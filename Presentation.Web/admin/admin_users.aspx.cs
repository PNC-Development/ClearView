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
    public partial class admin_users : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Users oUser;
        protected AD oAD;
        private NCC.ClearView.Application.Core.Roles oRole;
        protected Users_At oUserAt;
        protected int intProfile;
        protected string strView = "display:inline";
        protected string strAdd = "display:none";
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/admin_users.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oRole = new NCC.ClearView.Application.Core.Roles(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oAD = new AD(intProfile, dsn, intEnvironment);
            oUserAt = new Users_At(intProfile, dsn);
            lblResult.Text = "";
            txtName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");
            if (!IsPostBack)
            {
                if (Request.QueryString["f"] != null)
                {
                    divAdd.Style["display"] = "inline";
                    if (Request.QueryString["f"].Trim() != "")
                        Find(Request.QueryString["f"]);
                    else
                        lblResult.Text = "<img src='/images/error.gif' border='0' align='absmiddle'> Please enter a name";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    divEdit.Style["display"] = "inline";
                    hdnId.Value = "0";
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                }
                else if (Request.QueryString["s"] != null)
                {
                    divView.Style["display"] = "inline";
                    if (Request.QueryString["s"].Trim() != "")
                        LoopRepeater(Request.QueryString["s"]);
                    else
                        lblResult.Text = "<img src='/images/error.gif' border='0' align='absmiddle'> Please enter a name";
                    LoadList();
                }
                else
                    divView.Style["display"] = "inline";
                Variables oVariable = new Variables(intEnvironment);
                txtManager.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','" + hdnManager.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this user?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
            }
            btnRoles.Attributes.Add("onclick", "return OpenWindow('USERROLES','" + hdnId.ClientID + "','',false,'500',300);");
            btnPages.Attributes.Add("onclick", "return OpenWindow('USERPAGEBROWSER','" + hdnId.ClientID + "','',false,'400',475);");
        }
        private void LoadList()
        {
            DataSet ds = oUserAt.Gets(1);
            ddlUserAt.DataTextField = "name";
            ddlUserAt.DataValueField = "atid";
            ddlUserAt.DataSource = ds;
            ddlUserAt.DataBind();
            ddlUserAt.Items.Insert(0, new ListItem("-- NONE --", "0"));
        }
        private void LoopRepeater(string _search)
        {
            DataSet ds = new DataSet();
            ds = oUser.Gets(_search, 1);
            DataView dv = ds.Tables[0].DefaultView;
            if (Request.QueryString["sort"] != null)
                dv.Sort = Request.QueryString["sort"].ToString();
            rptView.DataSource = dv;
            rptView.DataBind();
            foreach (RepeaterItem ri in rptView.Items)
            {
                ImageButton oDelete = (ImageButton)ri.FindControl("btnDelete");
                oDelete.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this user?');");
                ImageButton oEnable = (ImageButton)ri.FindControl("btnEnable");
                if (oEnable.ImageUrl == "images/enabled.gif")
                {
                    oEnable.ToolTip = "Click to disable";
                    oEnable.Attributes.Add("onClick", "return confirm('Are you sure you want to disable this user?');");
                }
                else
                    oEnable.ToolTip = "Click to enable";
            }
        }
        protected void OrderView(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            string strSort;
            if (Request.QueryString["sort"] == null)
                strSort = oButton.CommandArgument + " ASC";
            else
                if (Request.QueryString["sort"].ToString() == (oButton.CommandArgument + " ASC"))
                    strSort = oButton.CommandArgument + " DESC";
                else
                    strSort = oButton.CommandArgument + " ASC";
            Response.Redirect(Request.Path + "?sort=" + strSort);
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intUserId = Int32.Parse(Request.Form[hdnId.UniqueID]);
            int intManager = Int32.Parse(Request.Form[hdnManager.UniqueID]);
            oUser.Update(intUserId, txtUser.Text, txtPNC.Text, txtFirst.Text, txtLast.Text, intManager, (chkManager.Checked == true ? 1 : 0), (chkBoard.Checked == true ? 1 : 0), (chkDirector.Checked ? 1 : 0), txtPagers.Text, Int32.Parse(ddlUserAt.SelectedItem.Value), txtPhone.Text, txtSkills.Text, Int32.Parse(txtVacation.Text), (chkMultiple.Checked ? 1 : 0), (chkUngroupProjects.Checked ? 1 : 0), (chkShowReturns.Checked ? 1 : 0), (chkAddLocation.Checked ? 1 : 0), (chkAdmin.Checked ? 1 : 0), (chkEnabled.Checked == true ? 1 : 0));
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oEnable = (ImageButton)Sender;
            oUser.Enable(Int32.Parse(oEnable.CommandArgument), (oEnable.ImageUrl == "images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oDelete = (ImageButton)Sender;
            oUser.Delete(Int32.Parse(oDelete.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oUser.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
        protected void btnSearch_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?f=" + txtName.Text);
        }
        protected void btnSearch2_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?s=" + txtSearch2.Text);
        }
        protected void btnCancelSearch_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        private void Find(string strSearch)
        {
            SearchResultCollection oResults = oAD.Search(strSearch, "samaccountname");
            bool boolPNC = false;
            int intCount = oResults.Count;
            if (intCount == 0 && (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD))
            {
                // Only search PNC if in production
                AD oPNC = new AD(intProfile, dsn, 999);
                oResults = oPNC.Search(strSearch, "samaccountname");
                intCount = oResults.Count;
                boolPNC = true;
            }
            if (intCount == 0)
            {
                btnSave.Visible = false;
                TableRow oRow = new TableRow();
                TableCell oCell = new TableCell();
                oCell.Text = "<b>Your search for \"" + strSearch + "\" did not return any results...</b>";
                oRow.ForeColor = System.Drawing.Color.Crimson;
                oRow.Cells.Add(oCell);
                tblResults.Rows.Add(oRow);
            }
            else
            {
                btnSave.Visible = true;
                bool boolOther = true;
                TableRow oRow = new TableRow();
                TableCell oCell = new TableCell();
                oCell.ColumnSpan = 4;
                oCell.Text = "<b>Search for \"" + strSearch + "\" returned " + intCount.ToString() + " results...</b>";
                oRow.ForeColor = System.Drawing.Color.Crimson;
                oRow.Cells.Add(oCell);
                tblResults.Rows.Add(oRow);
                oRow = new TableRow();
                oCell = new TableCell();
                oCell.Text = "&nbsp;";
                oRow.Cells.Add(oCell);
                oCell = new TableCell();
                if (boolPNC == true)
                    oCell.Text = "<b>PNC ID</b>";
                else
                    oCell.Text = "<b>XID</b>";
                oRow.Cells.Add(oCell);
                oCell = new TableCell();
                oCell.Text = "<b>First Name</b>";
                oRow.Cells.Add(oCell);
                oCell = new TableCell();
                oCell.Text = "<b>Last Name</b>";
                oRow.Cells.Add(oCell);
                oCell = new TableCell();
                oCell.Text = "<b>Is Manager</b>";
                oRow.Cells.Add(oCell);
                oCell = new TableCell();
                oCell.Text = "<b>Board</b>";
                oRow.Cells.Add(oCell);
                oCell = new TableCell();
                oCell.Text = "<b>Director</b>";
                oRow.Cells.Add(oCell);
                tblResults.Rows.Add(oRow);
                int intCounter = 0;
                foreach (SearchResult oResult in oResults)
                {
                    if (oResult.Properties.Contains("extensionattribute10") == true || oResult.Properties.Contains("sAMAccountName") == true || oResult.Properties.Contains("mailnickname") == true)
                    {
                        intCounter++;
                        string strXid = "";
                        if (oResult.Properties.Contains("extensionattribute10") == true)
                            strXid = oResult.GetDirectoryEntry().Properties["extensionattribute10"].Value.ToString();
                        else if (oResult.Properties.Contains("sAMAccountName") == true)
                            strXid = oResult.GetDirectoryEntry().Properties["sAMAccountName"].Value.ToString();
                        else if (oResult.Properties.Contains("mailnickname") == true)
                            strXid = oResult.GetDirectoryEntry().Properties["mailnickname"].Value.ToString();
                        string strFName = "";
                        if (oResult.Properties.Contains("givenname") == true)
                            strFName = oResult.GetDirectoryEntry().Properties["givenname"].Value.ToString();
                        string strLName = "";
                        if (oResult.Properties.Contains("sn") == true)
                            strLName = oResult.GetDirectoryEntry().Properties["sn"].Value.ToString();
                        oRow = new TableRow();
                        oCell = new TableCell();
                        TextBox txtFName = new TextBox();
                        txtFName.ID = "txtFName_" + intCounter.ToString();
                        txtFName.Width = Unit.Pixel(125);
                        txtFName.MaxLength = 100;
                        txtFName.CssClass = "default";
                        txtFName.Text = strFName;
                        TextBox txtLName = new TextBox();
                        txtLName.ID = "txtLName_" + intCounter.ToString();
                        txtLName.Width = Unit.Pixel(125);
                        txtLName.MaxLength = 100;
                        txtLName.CssClass = "default";
                        txtLName.Text = strLName;
                        CheckBox chkM = new CheckBox();
                        chkM.ID = "chkM" + intCounter.ToString();
                        chkM.CssClass = "default";
                        CheckBox chkB = new CheckBox();
                        chkB.ID = "chkB" + intCounter.ToString();
                        chkB.CssClass = "default";
                        CheckBox chkD = new CheckBox();
                        chkD.ID = "chkD" + intCounter.ToString();
                        chkD.CssClass = "default";
                        CheckBox oCheck = new CheckBox();
                        oCheck.Attributes.Add("onclick", "ADCheck(this,'" + (boolPNC ? "" : strXid) + "','" + (boolPNC ? strXid : "") + "','" + txtFName.ID + "','" + txtLName.ID + "','" + chkM.ID + "','" + chkB.ID + "','" + chkD.ID + "','" + hdnUsers.ClientID + "');");
                        oCell.Controls.Add(oCheck);
                        oRow.Cells.Add(oCell);
                        oCell = new TableCell();
                        oCell.Text = strXid;
                        oRow.Cells.Add(oCell);
                        oCell = new TableCell();
                        oCell.Controls.Add(txtFName);
                        oRow.Cells.Add(oCell);
                        oCell = new TableCell();
                        oCell.Controls.Add(txtLName);
                        oRow.Cells.Add(oCell);
                        oCell = new TableCell();
                        oCell.Controls.Add(chkM);
                        oRow.Cells.Add(oCell);
                        oCell = new TableCell();
                        oCell.Controls.Add(chkB);
                        oRow.Cells.Add(oCell);
                        oCell = new TableCell();
                        oCell.Controls.Add(chkD);
                        oRow.Cells.Add(oCell);
                        if (boolOther == true)
                            oRow.BackColor = System.Drawing.Color.WhiteSmoke;
                        boolOther = (!boolOther);
                        tblResults.Rows.Add(oRow);
                    }
                }
            }
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
                strUsers = strUsers.Substring(strUsers.IndexOf("&") + 1);
                string strIsManager = strUsers.Substring(0, strUsers.IndexOf("&"));
                strUsers = strUsers.Substring(strUsers.IndexOf("&") + 1);
                string strBoard = strUsers.Substring(0, strUsers.IndexOf("&"));
                strUsers = strUsers.Substring(strUsers.IndexOf("&") + 1);
                string strDirector = strUsers.Substring(0, strUsers.IndexOf("&"));
                strUsers = strUsers.Substring(strUsers.IndexOf("&") + 1);
                string strMultiple = strUsers.Substring(0, strUsers.IndexOf("&"));
                strUsers = strUsers.Substring(strUsers.IndexOf("&") + 1);
                string strAddLocation = strUsers.Substring(0, strUsers.IndexOf("&&"));
                strUsers = strUsers.Substring(strUsers.IndexOf("&&") + 2);
                oUser.Add(strUserName, strPNC, strFname, strLname, 0, Int32.Parse(strIsManager), Int32.Parse(strBoard), Int32.Parse(strDirector), "", 0, "", "", 0, Int32.Parse(strMultiple), Int32.Parse(strAddLocation), 0, 1);
            }
            lblResult.Text = "<img src='/images/alert.gif' border='0' align='absmiddle'> Accounts have been created successfully!";
            btnSave.Visible = false;
        }
    }
}
