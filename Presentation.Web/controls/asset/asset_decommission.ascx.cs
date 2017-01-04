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
    public partial class asset_decommission : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Platforms oPlatform;
        protected Types oType;
        protected Asset oAsset;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intPlatform = 0;
        protected int intType = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Asset Successfully Decommissioned');window.navigate('" + oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&tid=" + Request.QueryString["tid"] + "');<" + "/" + "script>");
            LoadLists();
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                panPath.Visible = true;
                string strPath = oType.Get(intType, "asset_decommission_path");
                if (strPath != "")
                    PHControl.Controls.Add((Control)LoadControl(strPath));
                else
                    panNoPath.Visible = true;
            }
            else
            {
                if (!IsPostBack)
                    LoadAll();
            }
            ddlPlatform.Attributes.Add("onchange", "WaitDDL('" + divWait.ClientID + "');");
            ddlTypes.Attributes.Add("onchange", "WaitDDL('" + divWait2.ClientID + "');");
        }
        private void LoadLists()
        {
            DataSet ds = oPlatform.GetAssets(1);
            ddlPlatform.DataValueField = "platformid";
            ddlPlatform.DataTextField = "name";
            ddlPlatform.DataSource = ds;
            ddlPlatform.DataBind();
            ddlPlatform.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != "")
                intPlatform = Int32.Parse(Request.QueryString["pid"]);
            if (oPlatform.Get(intPlatform).Tables[0].Rows.Count > 0)
            {
                ddlPlatform.SelectedValue = intPlatform.ToString();
                panTypes.Visible = true;
                ds = oType.Gets(intPlatform, 1);
                ddlTypes.DataValueField = "id";
                ddlTypes.DataTextField = "name";
                ddlTypes.DataSource = ds;
                ddlTypes.DataBind();
                ddlTypes.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                if (Request.QueryString["tid"] != null && Request.QueryString["tid"] != "")
                    intType = Int32.Parse(Request.QueryString["tid"]);
                if (oType.Get(intType).Tables[0].Rows.Count > 0)
                {
                    panPath.Visible = true;
                    ddlTypes.SelectedValue = intType.ToString();
                }
            }
        }
        protected void ddlPlatform_Change(Object Sender, EventArgs e)
        {
            if (ddlPlatform.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + ddlPlatform.SelectedItem.Value);
            else
                Response.Redirect(oPage.GetFullLink(intPage));
        }
        protected void ddlTypes_Change(Object Sender, EventArgs e)
        {
            if (ddlTypes.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&tid=" + ddlTypes.SelectedItem.Value);
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"]);
        }
        private void LoadAll()
        {
            if (intType > 0)
            {
                DataSet ds = oAsset.GetAll(intType, (int)AssetStatus.InUse);
                DataView dv = ds.Tables[0].DefaultView;
                try
                {
                    if (Request.QueryString["sort"] != null)
                        dv.Sort = Request.QueryString["sort"];
                }
                catch { }
                rptAll.DataSource = dv;
                rptAll.DataBind();
                lblNone.Visible = (rptAll.Items.Count == 0);
                panAll.Visible = true;
            }
        }
        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            LinkButton oOrder = (LinkButton)Sender;
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
            {
                if (Request.QueryString["sort"] == oOrder.CommandArgument)
                    strOrder = oOrder.CommandArgument + " DESC";
            }
            if (strOrder == "")
                strOrder = oOrder.CommandArgument;
            string strPID = "";
            if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != "")
                strPID = Request.QueryString["pid"];
            string strTID = "";
            if (Request.QueryString["tid"] != null && Request.QueryString["tid"] != "")
                strTID = Request.QueryString["tid"];
            Response.Redirect(oPage.GetFullLink(intPage) + "?sort=" + strOrder + "&pid=" + strPID + "&tid=" + strTID);
        }
    }
}