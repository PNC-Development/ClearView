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
    public partial class frame_user_roles : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Permissions oPermission;
        protected Users oUser;
        private NCC.ClearView.Application.Core.Roles oRole;
        protected Groups oGroup;
        private DataSet ds;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oPermission = new Permissions(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oRole = new NCC.ClearView.Application.Core.Roles(intProfile, dsn);
            oGroup = new Groups(intProfile, dsn);
            lblFinish.Visible = false;
            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                lblFinish.Visible = true;
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                if (!IsPostBack)
                {
                    lblUserId.Text = Request.QueryString["id"];
                    int intUser = Int32.Parse(lblUserId.Text);
                    lblUser.Text = oUser.GetFullName(intUser);
                    LoadLists();
                    btnAdd.Attributes.Add("onclick", "return MoveIn(" + ddlAvailable.ClientID + "," + lstCurrent.ClientID + ",'" + hdnRoles.ClientID + "');");
                    btnRemove.Attributes.Add("onclick", "return MoveOut(" + lstCurrent.ClientID + "," + ddlAvailable.ClientID + ",'" + hdnRoles.ClientID + "');");
                    lstCurrent.Attributes.Add("ondblclick", "return MoveOut(this," + ddlAvailable.ClientID + ",'" + hdnRoles.ClientID + "');");
                    btnClose.Attributes.Add("onclick", "return HidePanel();");
                }
            }
        }
        private void LoadLists()
        {
            int intUser = Int32.Parse(lblUserId.Text);
            ds = oRole.GetUser(intUser);
            string strGroupId = "0";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (strGroupId != dr["groupid"].ToString())
                {
                    strGroupId = dr["groupid"].ToString();
                    lstCurrent.Items.Add(new ListItem(dr["groupname"].ToString(), dr["groupid"].ToString()));
                }
            }
            ds = oGroup.Gets(1);
            ddlAvailable.DataValueField = "groupid";
            ddlAvailable.DataTextField = "name";
            ddlAvailable.DataSource = ds;
            ddlAvailable.DataBind();
            ddlAvailable.Items.Insert(0, "-- SELECT --");
            for (int ii = 0; ii < lstCurrent.Items.Count; ii++)
            {
                for (int jj = 0; jj < ddlAvailable.Items.Count; jj++)
                {
                    if (lstCurrent.Items[ii].Text == ddlAvailable.Items[jj].Text)
                    {
                        ddlAvailable.Items.Remove(ddlAvailable.Items[jj]);
                        jj--;
                    }
                }
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intUser = Int32.Parse(lblUserId.Text);
            string strRoles = Request.Form[hdnRoles.UniqueID];
            if (strRoles != "")
            {
                oRole.DeleteUser(intUser);
                while (strRoles != "")
                {
                    int intId = Int32.Parse(strRoles.Substring(0, strRoles.IndexOf("&")));
                    strRoles = strRoles.Substring(strRoles.IndexOf("&") + 1);
                    oRole.Add(intUser, intId);
                }
            }
            Response.Redirect(Request.Path + "?id=" + intUser.ToString() + "&save=true");
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
