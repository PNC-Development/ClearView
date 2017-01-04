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
    public partial class asset_checkin : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Platforms oPlatform;
        protected Types oType;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Asset Successfully Added');window.navigate('" + oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&tid=" + Request.QueryString["tid"] + "&mid=" + Request.QueryString["mid"] + "');<" + "/" + "script>");
            LoadLists();
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
            int intPlatform = 0;
            if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != "")
                intPlatform = Int32.Parse(Request.QueryString["pid"]);
            if (oPlatform.Get(intPlatform).Tables[0].Rows.Count > 0)
            {
                string strPath = oPlatform.Get(intPlatform, "asset_checkin_path");
                if (strPath != "")
                    PHImport.Controls.Add((Control)LoadControl(strPath));
                ddlPlatform.SelectedValue = intPlatform.ToString();
                panTypes.Visible = true;
                ds = oType.Gets(intPlatform, 1);
                ddlTypes.DataValueField = "id";
                ddlTypes.DataTextField = "name";
                ddlTypes.DataSource = ds;
                ddlTypes.DataBind();
                ddlTypes.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                int intType = 0;
                if (Request.QueryString["tid"] != null && Request.QueryString["tid"] != "")
                    intType = Int32.Parse(Request.QueryString["tid"]);
                if (oType.Get(intType).Tables[0].Rows.Count > 0)
                {
                    panPath.Visible = true;
                    ddlTypes.SelectedValue = intType.ToString();
                    strPath = oType.Get(intType, "asset_checkin_path");
                    if (strPath != "")
                        PHControl.Controls.Add((Control)LoadControl(strPath));
                    else
                        panNoPath.Visible = true;
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
    }
}