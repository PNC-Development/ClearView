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
    public partial class reports : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Reports oReport;
        protected Functions oFunction;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/reports.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oReport = new Reports(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            if (!IsPostBack)
            {
                LoadReports(0);
                btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnParent.ClientID + "','" + hdnOrder.ClientID + "&type=REPORTS" + "',false,400,400);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                btnParent.Attributes.Add("onclick", "return OpenWindow('REPORTBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
                btnPath.Attributes.Add("onclick", "return OpenWindow('REPORTFILEBROWSER','" + txtPath.ClientID + "','',false,400,600);");
                btnImage.Attributes.Add("onclick", "return OpenWindow('IMAGEPATH','','" + txtImage.ClientID + "',false,500,550);");
                btnPermissions.Attributes.Add("onclick", "return OpenWindow('REPORTPERMISSIONS','" + hdnId.ClientID + "','',false,'500',300);");
                btnApplications.Attributes.Add("onclick", "return OpenWindow('REPORTAPPLICATIONS','" + hdnId.ClientID + "','',false,'500',300);");
                btnUsers.Attributes.Add("onclick", "return OpenWindow('REPORTUSERS','" + hdnId.ClientID + "','',false,'500',300);");
                btnPhysical.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtPhysical.ClientID + "','',false,400,600);");
            }
        }
        private void LoadReports(int _parent)
        {
            TreeNode oNode = new TreeNode();
            oNode.Text = "ClearView Reports";
            oNode.ToolTip = "ClearView Reports";
            oNode.ImageUrl = "/images/folder.gif";
            oNode.SelectAction = TreeNodeSelectAction.Expand;
            oTreeview.Nodes.Add(oNode);
            LoadReports(0, oNode);
            TreeNode oNew = new TreeNode();
            oNew.Text = "&nbsp;Add Report";
            oNew.ToolTip = "Add Report";
            oNew.ImageUrl = "/images/green_right.gif";
            oNew.NavigateUrl = "javascript:Add('0','No Parent');";
            oNode.ChildNodes.Add(oNew);
        }
        private void LoadReports(int _parent, TreeNode oParent)
        {
            DataSet ds = oReport.Gets(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["title"].ToString();
                oNode.ToolTip = dr["title"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = "javascript:Edit('" + dr["reportid"].ToString() + "','" + dr["title"].ToString() + "','" + dr["old"].ToString() + "','" + dr["path"].ToString() + "','" + dr["physical"].ToString() + "','" + dr["description"].ToString() + "','" + dr["about"].ToString() + "','" + dr["image"].ToString() + "','" + dr["parent"].ToString() + "','" + oReport.GetName(Int32.Parse(dr["parent"].ToString())) + "','" + dr["percentage"].ToString() + "','" + dr["toggle"].ToString() + "','" + dr["application"].ToString() + "','" + dr["enabled"].ToString() + "');";
                oParent.ChildNodes.Add(oNode);
                LoadReports(Int32.Parse(dr["reportid"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Report";
                oNew.ToolTip = "Add Report";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = "javascript:Add('" + dr["reportid"].ToString() + "','" + oReport.GetName(Int32.Parse(dr["reportid"].ToString())) + "');";
                oNode.ChildNodes.Add(oNew);
            }
            oTreeview.ExpandDepth = 1;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intItem = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            if (Request.Form[hdnId.UniqueID] == "0")
                oReport.Add(txtTitle.Text, (chkOld.Checked ? 1 : 0), txtPath.Text, txtPhysical.Text, txtDesc.Text, txtAbout.Text, txtImage.Text, Int32.Parse(Request.Form[hdnParent.UniqueID]), Int32.Parse(txtPercent.Text), Int32.Parse(ddlToggle.SelectedItem.Value), (chkApplication.Checked ? 1 : 0), 0, (chkEnabled.Checked ? 1 : 0));
            else
                oReport.Update(Int32.Parse(Request.Form[hdnId.UniqueID]), txtTitle.Text, (chkOld.Checked ? 1 : 0), txtPath.Text, txtPhysical.Text, txtDesc.Text, txtAbout.Text, txtImage.Text, Int32.Parse(Request.Form[hdnParent.UniqueID]), Int32.Parse(txtPercent.Text), Int32.Parse(ddlToggle.SelectedItem.Value), (chkApplication.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oReport.Update(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oReport.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
