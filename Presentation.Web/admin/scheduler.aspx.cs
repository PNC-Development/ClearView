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
using NCC.ClearView.Presentation.Web.Custom;

namespace NCC.ClearView.Presentation.Web
{
    public partial class scheduler : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intPlatform = Int32.Parse(ConfigurationManager.AppSettings["ServerPlatformID"]);
        protected Scheduler oScheduler;
        protected int intProfile;
        protected int intID = 0;
        protected string strMenuTab1 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/scheduler.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");

            oScheduler = new Scheduler(intProfile, dsn);

            if (!IsPostBack)
                LoadLists();

            if (Request.QueryString["id"] == null)
                LoopRepeater();
            else
            {
                intID = Int32.Parse(Request.QueryString["id"]);
                if (intID == 0)
                {
                    panAdd.Visible = true;
                    btnDelete.Enabled = false;
                }
                else
                {
                    if (Request.QueryString["save"] != null)
                        panSave.Visible = true;
                    panAdd.Visible = true;
                    if (!IsPostBack)
                    {
                        DataSet ds = oScheduler.Get(intID);
                        txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                        txtDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                        txtServer.Text = ds.Tables[0].Rows[0]["server"].ToString();
                        txtParameters.Text = ds.Tables[0].Rows[0]["parameters"].ToString();
                        ddlEnvironment.SelectedValue = ds.Tables[0].Rows[0]["credentials"].ToString();
                        txtDays.Text = ds.Tables[0].Rows[0]["days"].ToString();
                        txtTimes.Text = ds.Tables[0].Rows[0]["times"].ToString();
                        txtTimeout.Text = ds.Tables[0].Rows[0]["timeout"].ToString();
                        chkPrivledges.Checked = (ds.Tables[0].Rows[0]["privledges"].ToString() == "1");
                        chkInteractive.Checked = (ds.Tables[0].Rows[0]["interactive"].ToString() == "1");
                        ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                        chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnAdd.Text = "Update";
                        LoadLogs();
                    }
                }
            }

            if (panAdd.Visible == true && !IsPostBack)
            {
                int intMenuTab = 0;
                if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                    intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                LoadTab(intMenuTab);
            }

            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
        }
        private void LoadLists()
        {
            ddlStatus.Items.Add(new ListItem("Waiting", SchedulerStatus.Waiting.ToString()));
            ddlStatus.Items.Add(new ListItem("Running", SchedulerStatus.Running.ToString()));
            ddlStatus.Items.Add(new ListItem("RunOnce", SchedulerStatus.RunOnce.ToString()));
        }
        private void LoopRepeater()
        {
            panView.Visible = true;
            DataSet ds = oScheduler.Gets(0);
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
            if (intID == 0)
                intID = oScheduler.Add(txtName.Text, txtDescription.Text, txtServer.Text, txtParameters.Text, Int32.Parse(ddlEnvironment.SelectedItem.Value), txtDays.Text, txtTimes.Text, Int32.Parse(txtTimeout.Text), (chkPrivledges.Checked ? 1 : 0), (chkInteractive.Checked ? 1 : 0), Int32.Parse(ddlStatus.SelectedItem.Value), "", -1, (chkEnabled.Checked ? 1 : 0));
            else
                oScheduler.Update(intID, txtName.Text, txtDescription.Text, txtServer.Text, txtParameters.Text, Int32.Parse(ddlEnvironment.SelectedItem.Value), txtDays.Text, txtTimes.Text, Int32.Parse(txtTimeout.Text), (chkPrivledges.Checked ? 1 : 0), (chkInteractive.Checked ? 1 : 0), Int32.Parse(ddlStatus.SelectedItem.Value), (chkEnabled.Checked ? 1 : 0));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oScheduler.Enable(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oScheduler.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oScheduler.Delete(intID);
            Response.Redirect(Request.Path);
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLogs();
            LoadTab(2);
        }
        private void LoadLogs()
        {
            rptLogs.DataSource = oScheduler.GetLogs(intID, Int32.Parse(txtRows.Text));
            rptLogs.DataBind();
            lblLogs.Visible = (rptLogs.Items.Count == 0);
        }
        private void LoadTab(int tab)
        {
            Tab oTab = new Tab("", tab, "divMenu1", true, false);
            oTab.AddTab("Details", "");
            oTab.AddTab("History", "");
            strMenuTab1 = oTab.GetTabs();
        }
    }
}
