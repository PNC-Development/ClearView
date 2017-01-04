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
    public partial class updateUserProfile : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected DataSet ds;
        protected string strTitle = ConfigurationManager.AppSettings["appTitle"];
        protected string strPage = "";
        protected Users oUser;
        protected AD oAD;
        protected NCC.ClearView.Application.Core.Roles oRole;
        protected Users_At oUserAt;
        protected int intProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = strTitle;
            Control oControl = (Control)LoadControl("/controls/sys/sys_rotator_header.ascx");
            PH4.Controls.Add(oControl);

            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
            {

                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
                oUser = new Users(intProfile, dsn);
                oUserAt = new Users_At(intProfile, dsn);
                Variables oVariable = new Variables(intEnvironment);
                txtManager.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','" + hdnManager.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");

                DataSet ds = new DataSet();
                ds = oUser.Get(intProfile);
                LoadList();
                if (!IsPostBack)
                    LoadUserDetails(ds.Tables[0].Rows[0]);
                btnCancel.Enabled = false;
                string strPNC = "";
                if (rowPNC.Visible == true)
                    strPNC = " && ValidateTextWarning('" + txtPNC.ClientID + "','WARNING: You did not enter a PNC ID / P-ID. It is recommended that you click CANCEL and enter your PNC ID.\\n\\nAre you sure you want to continue?\\n')";
                btnUpdate.Attributes.Add("onclick", "return ValidateText('" + txtFirst.ClientID + "','Please enter your first name')" +
                    " && ValidateText('" + txtLast.ClientID + "','Please enter your last name')" +
                    " && ValidateHidden0('" + hdnManager.ClientID + "','" + txtManager.ClientID + "','Please enter the LAN ID or P-ID of your manager and select from the list\\n\\nNOTE: If you do not see your manager in the list, please use the SUPPORT \\nbutton (located in the top right of this page) to submit a new support ticket')" +
                    strPNC +
                    ";");
            }
            else
                btnUpdate.Enabled = false;
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
        private void LoadUserDetails(DataRow dr)
        {
            txtUser.Text = dr["XID"].ToString();
            txtUser.Enabled = false;
            txtPNC.Text = dr["pnc_id"].ToString();
            if (txtPNC.Text == txtUser.Text)
            {
                // User is a PNC employee, hide the LAN ID
                rowPNC.Visible = false;
            }
            txtFirst.Text = dr["fname"].ToString();
            txtLast.Text = dr["lname"].ToString();

            int intManager = Int32.Parse(dr["manager"].ToString());
            txtManager.Text = oUser.GetFullName(intManager);
            hdnManager.Value = intManager.ToString();
            if (intManager == 0)
                panManager.Visible = true;
            txtPagers.Text = dr["pager"].ToString();
            ddlUserAt.Items.FindByValue(dr["atid"].ToString()).Selected = true;
            txtPhone.Text = dr["Phone"].ToString();
            txtSkills.Text = dr["other"].ToString();
            imgPicture.ImageUrl = "/frame/picture.aspx?xid=" + dr["xid"].ToString();
            imgPicture.Style["border"] = "solid 1px #999999";
            btnPicture.Attributes.Add("onclick", "return OpenWindow('IMAGE','');");
        }
        private void UpdateUserDetails()
        {
            int intManager = 0;
            if (Request.Form[hdnManager.UniqueID] != "")
                intManager = Int32.Parse(Request.Form[hdnManager.UniqueID]);
            oUser.UpdateProfileUpdateByUser(intProfile, (rowPNC.Visible == true ? txtPNC.Text.ToUpper() : txtUser.Text.ToUpper()), txtFirst.Text, txtLast.Text, intManager, txtPagers.Text, Int32.Parse(ddlUserAt.SelectedItem.Value), txtPhone.Text, txtSkills.Text);
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateUserDetails();

            string strRedirect = "/index.aspx";
            if (Request.QueryString["referrer"] != null)
            {
                strRedirect = Request.QueryString["referrer"];
            }
            if (Request.Cookies["userloginreferrer"] != null && Request.Cookies["userloginreferrer"].Value != "")
            {
                strRedirect = Request.Cookies["userloginreferrer"].Value;
                Response.Cookies["userloginreferrer"].Value = "";
            }
            Response.Redirect(strRedirect);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string strRedirect = "/index.aspx";
            if (Request.QueryString["referrer"] != null)
            {
                strRedirect = Request.QueryString["referrer"];
            }
            if (Request.Cookies["userloginreferrer"] != null && Request.Cookies["userloginreferrer"].Value != "")
            {
                strRedirect = Request.Cookies["userloginreferrer"].Value;
                Response.Cookies["userloginreferrer"].Value = "";
            }
            Response.Redirect(strRedirect);
        }
    }
}
