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
    public partial class forecast_filter : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            btnSave.Attributes.Add("onclick", "return Update('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "');");
            btnClose.Attributes.Add("onclick", "return parent.HidePanel();");
            if (!IsPostBack)
                LoadLists();
        }
        private void LoadLists()
        {
            Classes oClass = new Classes(intProfile, dsn);
            ddlClass.DataValueField = "id";
            ddlClass.DataTextField = "name";
            ddlClass.DataSource = oClass.Gets(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            int intClass = 0;
            if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
                intClass = Int32.Parse(Request.QueryString["cid"]);
            if (oClass.Get(intClass).Tables[0].Rows.Count > 0)
            {
                ddlClass.SelectedValue = intClass.ToString();
                ddlEnvironment.Enabled = true;
                Environments oEnvironment = new Environments(intProfile, dsn);
                ddlEnvironment.DataValueField = "id";
                ddlEnvironment.DataTextField = "name";
                ddlEnvironment.DataSource = oClass.GetEnvironment(intClass, 1);
                ddlEnvironment.DataBind();
                ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                int intEnvironment = 0;
                if (Request.QueryString["eid"] != null && Request.QueryString["eid"] != "")
                    intEnvironment = Int32.Parse(Request.QueryString["eid"]);
                if (oEnvironment.Get(intEnvironment).Tables[0].Rows.Count > 0)
                    ddlEnvironment.SelectedValue = intEnvironment.ToString();
            }
            else
            {
                ddlEnvironment.Enabled = false;
                ddlEnvironment.Items.Insert(0, new ListItem("-- Please select a Class --", "0"));
            }
        }
        protected void ddlClass_Change(Object Sender, EventArgs e)
        {
            if (ddlClass.SelectedIndex > 0)
                Response.Redirect(Request.Path + "?cid=" + ddlClass.SelectedItem.Value + "&cc=" + Request.QueryString["cc"] + "&ct=" + Request.QueryString["ct"] + "&ec=" + Request.QueryString["ec"] + "&et=" + Request.QueryString["et"]);
            else
                Response.Redirect(Request.Path + "?cc=" + Request.QueryString["cc"] + "&ct=" + Request.QueryString["ct"] + "&ec=" + Request.QueryString["ec"] + "&et=" + Request.QueryString["et"]);
        }
    }
}
