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
    public partial class sys_reports : System.Web.UI.UserControl
    {

        protected Reports oReport;
        protected Functions oFunction;
        protected Pages oPage;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected int intPage = 0;
        protected bool boolPreview = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oReport = new Reports(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oPage = new Pages(intProfile, dsn);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            lblTitle.Text = oPage.Get(intPage, "title");
            bool boolForm = true;
            if (Request.QueryString["s"] != null && Request.QueryString["s"] != "")
            {
                if (Request.QueryString["no_prev"] != null)
                    boolPreview = false;
                chkPreview.Checked = boolPreview;
                Find(Request.QueryString["s"]);
                boolForm = false;
                panNoForm.Visible = true;
                panSearch.Visible = true;
            }
            else if (Request.QueryString["order"] != null && Request.QueryString["order"] != "")
            {
                if (Request.QueryString["order"] == "sent")
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "send_order", "<script type=\"text/javascript\">alert('Your order has been submitted');<" + "/script>");
                if (Request.QueryString["order"] == "form")
                {
                    boolForm = false;
                    panNoForm.Visible = true;
                    panOrder.Visible = true;
                }
            }
            if (boolForm == true)
            {
                panReport.Visible = true;
                if (Request.QueryString["add"] != null && Request.QueryString["add"] != "")
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "add_fav", "<script type=\"text/javascript\">alert('The report has been added to your favorites');<" + "/script>");
                if (Request.QueryString["delete"] != null && Request.QueryString["delete"] != "")
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "remove_fav", "<script type=\"text/javascript\">alert('The report has been removed from your favorites');<" + "/script>");
                TreeNode oNode = new TreeNode();
                oNode = new TreeNode();
                oNode.Text = "Favorites";
                oNode.ToolTip = "Favorites";
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                LoadFavorites(oNode);
                LoadReports(0, oTreeview, null);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "NavResize", "<script type=\"text/javascript\">window.onload = Loader;<" + "/script>");
            }
            lnkAdd.Attributes.Add("onclick", "return CheckReport('" + hdnReport.ClientID + "','');");
            lnkRemove.Attributes.Add("onclick", "return CheckReport('" + hdnReport.ClientID + "','Are you sure you want to remove this report from your favorites?');");
            btnOrder.Attributes.Add("onclick", "return ValidateText('" + txtOrder.ClientID + "','Please enter a description of your request');");
            txtSearch.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");
            btnSearch.Attributes.Add("onclick", "return ValidateText('" + txtSearch.ClientID + "','Please enter some text to search');");
            txtSearch2.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch2.ClientID + "').click();return false;}} else {return true}; ");
            btnSearch2.Attributes.Add("onclick", "return ValidateText('" + txtSearch2.ClientID + "','Please enter some text to search');");
            imgMaximize.Attributes.Add("onclick", "return NewMaximize('');");
        }
        private void LoadReports(int _parent, TreeView oTree, TreeNode oParent)
        {
            DataSet ds = oReport.Gets(_parent, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intReport = Int32.Parse(dr["reportid"].ToString());
                DataSet dsChildren = oReport.Gets(intReport, 1);
                if (dsChildren.Tables[0].Rows.Count > 0)
                {
                    // Load Parent Reports
                    if (oReport.GetRole(intProfile, intReport).Tables[0].Rows.Count > 0)
                    {
                        TreeNode oNode = new TreeNode();
                        oNode.Text = dr["title"].ToString();
                        oNode.ToolTip = dr["title"].ToString();
                        string strImage = dr["image"].ToString().Trim();
                        if (dr["image"].ToString().Trim() != "")
                            oNode.ImageUrl = dr["image"].ToString();
                        else
                            oNode.ImageUrl = "/images/folder.gif";
                        if (dr["path"].ToString().Trim() == "" && dr["physical"].ToString().Trim() == "")
                        {
                            oNode.SelectAction = TreeNodeSelectAction.Expand;
                            if (strImage == "")
                                strImage = "/images/folder.gif";
                        }
                        else
                        {
                            if (dr["path"].ToString().Trim() != "")
                                oNode.NavigateUrl = "javascript:SelectReport('/frame/loading.htm?referrer=/frame/report.aspx?r=" + oFunction.encryptQueryString(dr["reportid"].ToString()) + "','frmReport','" + dr["reportid"].ToString() + "','" + hdnReport.ClientID + "');";
                            else
                                oNode.NavigateUrl = "javascript:SelectReport('" + dr["physical"].ToString() + "','frmReport','" + dr["reportid"].ToString() + "','" + hdnReport.ClientID + "');";
                            if (strImage == "")
                                strImage = "/images/report.gif";
                        }
                        oNode.ImageUrl = strImage;
                        if (oTree == null)
                            oParent.ChildNodes.Add(oNode);
                        else
                            oTree.Nodes.Add(oNode);
                        LoadReports(intReport, null, oNode);
                    }
                }
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intReport = Int32.Parse(dr["reportid"].ToString());
                DataSet dsChildren = oReport.Gets(intReport, 1);
                if (dsChildren.Tables[0].Rows.Count == 0)
                {
                    // Load Parent Reports
                    if (oReport.GetRole(intProfile, intReport).Tables[0].Rows.Count > 0)
                    {
                        TreeNode oNode = new TreeNode();
                        oNode.Text = dr["title"].ToString();
                        oNode.ToolTip = dr["title"].ToString();
                        string strImage = dr["image"].ToString().Trim();
                        if (dr["path"].ToString().Trim() == "" && dr["physical"].ToString().Trim() == "")
                        {
                            oNode.SelectAction = TreeNodeSelectAction.Expand;
                            if (strImage == "")
                                strImage = "/images/folder.gif";
                        }
                        else
                        {
                            if (dr["path"].ToString().Trim() != "")
                                oNode.NavigateUrl = "javascript:SelectReport('/frame/loading.htm?referrer=/frame/report.aspx?r=" + oFunction.encryptQueryString(dr["reportid"].ToString()) + "','frmReport','" + dr["reportid"].ToString() + "','" + hdnReport.ClientID + "');";
                            else
                                oNode.NavigateUrl = "javascript:SelectReport('" + dr["physical"].ToString() + "','frmReport','" + dr["reportid"].ToString() + "','" + hdnReport.ClientID + "');";
                            if (strImage == "")
                                strImage = "/images/report.gif";
                        }
                        oNode.ImageUrl = strImage;
                        if (oTree == null)
                            oParent.ChildNodes.Add(oNode);
                        else
                            oTree.Nodes.Add(oNode);
                        LoadReports(intReport, null, oNode);
                    }
                }
            }
            oTreeview.CollapseAll();
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadFavorites(TreeNode oParent)
        {
            DataSet ds = oReport.GetFavorites(intProfile);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["title"].ToString();
                oNode.ToolTip = dr["title"].ToString();
                if (dr["image"].ToString().Trim() != "")
                    oNode.ImageUrl = dr["image"].ToString();
                else
                    oNode.ImageUrl = "/images/report.gif";
                if (dr["path"].ToString().Trim() == "" && dr["physical"].ToString().Trim() == "")
                    oNode.SelectAction = TreeNodeSelectAction.Expand;
                else
                {
                    if (dr["path"].ToString().Trim() != "")
                        oNode.NavigateUrl = "javascript:SelectReport('/frame/loading.htm?referrer=/frame/report.aspx?r=" + oFunction.encryptQueryString(dr["reportid"].ToString()) + "','frmReport','" + dr["reportid"].ToString() + "','" + hdnReport.ClientID + "');";
                    else
                        oNode.NavigateUrl = "javascript:SelectReport('" + dr["physical"].ToString() + "','frmReport','" + dr["reportid"].ToString() + "','" + hdnReport.ClientID + "');";
                }
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void lnkOrder_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?order=form");
        }
        protected void lnkAdd_Click(Object Sender, EventArgs e)
        {
            string strReport = Request.Form[hdnReport.UniqueID];
            if (strReport != "")
            {
                oReport.AddFavorite(intProfile, Int32.Parse(strReport));
                Response.Redirect(oPage.GetFullLink(intPage) + "?add=sent");
            }
            Response.Redirect(oPage.GetFullLink(intPage));
        }
        protected void lnkRemove_Click(Object Sender, EventArgs e)
        {
            string strReport = Request.Form[hdnReport.UniqueID];
            if (strReport != "")
            {
                oReport.DeleteFavorite(intProfile, Int32.Parse(strReport));
                Response.Redirect(oPage.GetFullLink(intPage) + "?delete=sent");
            }
            Response.Redirect(oPage.GetFullLink(intPage));
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage));
        }
        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?order=sent");
        }
        protected void btnSearch_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?s=" + txtSearch.Text);
        }
        protected void btnSearch2_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?s=" + txtSearch2.Text);
        }
        protected void chkPreview_Change(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?s=" + Request.QueryString["s"] + (chkPreview.Checked ? "" : "&no_prev=true"));
        }
        private void Find(string _search)
        {
            txtSearch2.Text = _search;
            DataSet ds = oReport.Gets(intProfile, _search);
            if (boolPreview == true)
            {
                panViewPreview.Visible = true;
                rptViewPreview.DataSource = ds;
                rptViewPreview.DataBind();
                lblViewPreview.Visible = (ds.Tables[0].Rows.Count == 0);
            }
            else
            {
                panView.Visible = true;
                rptView.DataSource = ds;
                rptView.DataBind();
                lblView.Visible = (ds.Tables[0].Rows.Count == 0);
            }
        }
    }
}