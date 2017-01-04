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
    public partial class domains_dns : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected Domains oDomain;
        protected Classes oClass;
        protected Locations oLocation;
        protected int intProfile;
        protected int intID = 0;
        protected int intDomain = 0;
        protected int intClass = 0;
        protected string strLocation = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/domains_dns.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oDomain = new Domains(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);

            if (Request.QueryString["domainid"] != null)
                intDomain = Int32.Parse(Request.QueryString["domainid"]);
            if (Request.QueryString["parent"] != null)
                intClass = Int32.Parse(Request.QueryString["parent"]);

            if (Request.QueryString["id"] == null && intClass == 0)
                LoadDomains();
            else
            {
                if (intClass > 0)
                {
                    panAdd.Visible = true;
                    btnDelete.Enabled = false;
                }
                else
                {
                    intID = Int32.Parse(Request.QueryString["id"]);
                    panAdd.Visible = true;
                    if (!IsPostBack)
                    {
                        DataSet ds = oDomain.GetClassDNS(intID);
                        intDomain = Int32.Parse(ds.Tables[0].Rows[0]["domainid"].ToString());
                        intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                        intLocation = Int32.Parse(ds.Tables[0].Rows[0]["addressid"].ToString());
                        txtDNS1.Text = ds.Tables[0].Rows[0]["dns_ip1"].ToString();
                        txtDNS2.Text = ds.Tables[0].Rows[0]["dns_ip2"].ToString();
                        txtDNS3.Text = ds.Tables[0].Rows[0]["dns_ip3"].ToString();
                        txtDNS4.Text = ds.Tables[0].Rows[0]["dns_ip4"].ToString();
                        txtWINS1.Text = ds.Tables[0].Rows[0]["wins_ip1"].ToString();
                        txtWINS2.Text = ds.Tables[0].Rows[0]["wins_ip2"].ToString();
                        txtWINS3.Text = ds.Tables[0].Rows[0]["wins_ip3"].ToString();
                        txtWINS4.Text = ds.Tables[0].Rows[0]["wins_ip4"].ToString();
                        btnAdd.Text = "Update";
                    }
                }
                lblDomain.Text = oDomain.Get(intDomain, "name");
                lblClass.Text = oClass.Get(intClass, "name");
                if (!IsPostBack)
                {
                    strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, intLocation, true, "ddlCommon");
                    hdnLocation.Value = intLocation.ToString();
                }
            }
        }
        private void LoadDomains()
        {
            panView.Visible = true;
            DataSet ds = oDomain.GetClassEnvironments();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                LoadClasses(Int32.Parse(dr["id"].ToString()), oNode);
            }
            oTreeview.ExpandDepth = 0;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadClasses(int _domainid, TreeNode oParent)
        {
            DataSet ds = oDomain.GetClassEnvironments(_domainid);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadAddress(_domainid, Int32.Parse(dr["id"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Address";
                oNew.ToolTip = "Add Address";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = Request.Path + "?domainid=" + _domainid.ToString() + "&parent=" + dr["id"].ToString();
                oNode.ChildNodes.Add(oNew);
            }
        }
        private void LoadAddress(int _domainid, int _classid, TreeNode oParent)
        {
            DataSet ds = oDomain.GetClassDNS(_domainid, _classid);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["location"].ToString();
                oNode.ToolTip = dr["location"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = Request.Path + "?id=" + dr["id"].ToString();
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intLocation = 0;
            if (Request.Form[hdnLocation.UniqueID] != "")
                intLocation = Int32.Parse(Request.Form[hdnLocation.UniqueID]);
            if (intID == 0)
                oDomain.AddClassDNS(intDomain, intClass, intLocation, txtDNS1.Text, txtDNS2.Text, txtDNS3.Text, txtDNS4.Text, txtWINS1.Text, txtWINS2.Text, txtWINS3.Text, txtWINS4.Text);
            else
                oDomain.UpdateClassDNS(intID, txtDNS1.Text, txtDNS2.Text, txtDNS3.Text, txtDNS4.Text, txtWINS1.Text, txtWINS2.Text, txtWINS3.Text, txtWINS4.Text);
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oDomain.DeleteClassDNS(intID);
            Response.Redirect(Request.Path);
        }
    }
}
