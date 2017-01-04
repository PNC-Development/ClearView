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
using System.IO;

namespace NCC.ClearView.Presentation.Web
{
    public partial class _EXAMPLE : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Audit oAudit;
        protected Variables oVariable;
        protected int intProfile;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/audit_scripts.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oAudit = new Audit(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            if (!IsPostBack)
                LoadList();
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["add"] == null)
                    LoopRepeater();
                else
                {
                    panAdd.Visible = true;
                    btnDelete.Enabled = false;
                    txtTimeout.Text = "0";
                }
            }
            else
            {
                panAdd.Visible = true;
                intID = Int32.Parse(Request.QueryString["id"]);
                if (intID > 0 && !IsPostBack)
                {
                    DataSet ds = oAudit.GetScript(intID);
                    hdnId.Value = intID.ToString();
                    txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    ddlHardcode.SelectedValue = ds.Tables[0].Rows[0]["hardcode"].ToString();
                    if (ddlHardcode.SelectedItem.Value != "")
                    {
                        radLocal.Enabled = false;
                        radRemote.Enabled = false;
                        ddlLanguage.Enabled = false;
                        txtFile.Enabled = false;
                        txtParameters.Enabled = false;
                        txtTimeout.Enabled = false;
                    }
                    else
                    {
                        radLocal.Checked = (ds.Tables[0].Rows[0]["local"].ToString() == "1");
                        radRemote.Checked = (ds.Tables[0].Rows[0]["local"].ToString() == "0");
                        ddlLanguage.SelectedValue = ds.Tables[0].Rows[0]["languageid"].ToString();
                        lblPath.Text = ds.Tables[0].Rows[0]["path"].ToString();
                        txtParameters.Text = ds.Tables[0].Rows[0]["parameters"].ToString();
                        txtTimeout.Text = ds.Tables[0].Rows[0]["timeout"].ToString();
                    }
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                }
            }
            if (lblPath.Text == "")
                lblPath.Text = "<i>A script has not been uploaded</i>";
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
        }
        private void LoadList()
        {
            DataSet ds = oAudit.GetScriptLanguages(1);
            ddlLanguage.DataTextField = "name";
            ddlLanguage.DataValueField = "id";
            ddlLanguage.DataSource = ds;
            ddlLanguage.DataBind();
            ddlLanguage.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void LoopRepeater()
        {
            panView.Visible = true;
            DataSet ds = oAudit.GetScripts(0);
            DataView dv = ds.Tables[0].DefaultView;
            if (Request.QueryString["sort"] != null)
                dv.Sort = Request.QueryString["sort"].ToString();
            rptView.DataSource = dv;
            rptView.DataBind();
            foreach (RepeaterItem ri in rptView.Items)
            {
                ImageButton oDelete = (ImageButton)ri.FindControl("btnDelete");
                oDelete.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this item?');");
                ImageButton oEnable = (ImageButton)ri.FindControl("btnEnable");
                if (oEnable.ImageUrl == "/admin/images/enabled.gif")
                {
                    oEnable.ToolTip = "Click to disable";
                    oEnable.Attributes.Add("onClick", "return confirm('Are you sure you want to disable this item?');");
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
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=0");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            string strPath = lblPath.Text;
            if (txtFile.FileName != "" && txtFile.PostedFile != null)
            {
                string strDirectory = oVariable.DocumentsFolder() + "scripts";
                if (Directory.Exists(strDirectory) == false)
                    Directory.CreateDirectory(strDirectory);
                string strFile = txtFile.PostedFile.FileName.Trim();
                string strFileName = strFile.Substring(strFile.LastIndexOf("\\") + 1);
                if (File.Exists(strDirectory + "\\" + strFileName) == true)
                {
                    int intFileCount = 1;
                    string strFileCount = strFileName + ".VERSION_" + intFileCount.ToString();
                    while (File.Exists(strDirectory + "\\" + strFileCount) == true)
                    {
                        intFileCount++;
                        strFileCount = strFileName + ".VERSION_" + intFileCount.ToString();
                    }
                    File.Move(strDirectory + "\\" + strFileName, strDirectory + "\\" + strFileCount);
                }
                strPath = strDirectory + "\\" + strFileName;
                txtFile.PostedFile.SaveAs(strPath);
            }
            int intLanguage = 0;
            Int32.TryParse(ddlLanguage.SelectedItem.Value, out intLanguage);
            int intTimeout = 0;
            Int32.TryParse(txtTimeout.Text, out intTimeout);
            if (intID == 0)
                oAudit.AddScript(txtName.Text, ddlHardcode.SelectedItem.Value, (radLocal.Checked ? 1 : 0), intLanguage, strPath, txtParameters.Text, intTimeout, (chkEnabled.Checked ? 1 : 0));
            else
                oAudit.UpdateScript(intID, txtName.Text, ddlHardcode.SelectedItem.Value, (radLocal.Checked ? 1 : 0), intLanguage, strPath, txtParameters.Text, intTimeout, (chkEnabled.Checked ? 1 : 0));
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oAudit.EnableScript(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oAudit.DeleteScript(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oAudit.DeleteScript(intID);
            Response.Redirect(Request.Path);
        }
        protected void ddlHardcode_Change(Object Sender, EventArgs e)
        {
            bool boolEnabled = (ddlHardcode.SelectedItem.Value == "");
            radLocal.Enabled = boolEnabled;
            radRemote.Enabled = boolEnabled;
            ddlLanguage.Enabled = boolEnabled;
            txtFile.Enabled = boolEnabled;
            txtParameters.Enabled = boolEnabled;
            txtTimeout.Enabled = boolEnabled;
        }
    }
}
