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
    public partial class support_controls : System.Web.UI.UserControl
    {


        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected AppPages oAppPage;
        protected Users oUser;
        protected Supports oSupport;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oAppPage = new AppPages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oSupport = new Supports(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Request Submitted Successfully');window.navigate('" + oPage.GetFullLink(intPage) + "?mid=" + Request.QueryString["mid"] + "');<" + "/" + "script>");
            if (!IsPostBack)
            {
                LoadLists();
                lblRequestBy.Text = oUser.GetFullName(intProfile);
                lblRequestOn.Text = DateTime.Now.ToLongDateString();
                btnSubmit.Attributes.Add("onclick", "return ValidateRadioButtons('" + radSuggestion.ClientID + "','" + radIssue.ClientID + "','Please select the request type')" +
                    " && ValidateText('" + txtDescription.ClientID + "','Please enter a description')" +
                    ";");
                lblId.Text = "0";
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    DataSet ds = oSupport.Get(Int32.Parse(Request.QueryString["id"]));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int intUser = Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                        if (intProfile == intUser)
                        {
                            lblId.Text = Request.QueryString["id"];
                            panEdit.Visible = true;
                            txtTitle.Text = ds.Tables[0].Rows[0]["name"].ToString();
                            ddlModule.SelectedValue = ds.Tables[0].Rows[0]["pageid"].ToString();
                            if (ds.Tables[0].Rows[0]["type"].ToString() == "0")
                                radIssue.Checked = true;
                            else
                                radSuggestion.Checked = true;
                            txtDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                            lblRequestBy.Text = oUser.GetFullName(intUser);
                            lblRequestOn.Text = DateTime.Parse(ds.Tables[0].Rows[0]["modified"].ToString()).ToLongDateString();
                            panEditAdmin.Visible = true;
                            lblEditComments.Text = ds.Tables[0].Rows[0]["comments"].ToString();
                            if (lblEditComments.Text == "")
                                lblEditComments.Text = "<i>None</i>";
                            lblEditStatus.Text = ds.Tables[0].Rows[0]["statusname"].ToString();
                        }
                        else
                        {
                            panView.Visible = true;
                            lblName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                            lblModule.Text = oPage.Get(Int32.Parse(ds.Tables[0].Rows[0]["pageid"].ToString()), "menutitle");
                            if (ds.Tables[0].Rows[0]["type"].ToString() == "0")
                                lblType.Text = "Issue";
                            else
                                lblType.Text = "Suggestion";
                            lblDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                            lblBy.Text = oUser.GetFullName(intUser);
                            lblOn.Text = DateTime.Parse(ds.Tables[0].Rows[0]["modified"].ToString()).ToLongDateString();
                            lblViewComments.Text = ds.Tables[0].Rows[0]["comments"].ToString();
                            if (lblViewComments.Text == "")
                                lblViewComments.Text = "<i>None</i>";
                            lblViewStatus.Text = ds.Tables[0].Rows[0]["statusname"].ToString();
                        }
                    }
                    else
                        panEdit.Visible = true;
                }
                else
                    panEdit.Visible = true;
            }
        }
        private void LoadLists()
        {
            ddlModule.DataValueField = "pageid";
            ddlModule.DataTextField = "menutitle";
            ddlModule.DataSource = oPage.Gets(intApplication, intProfile, 0, 1, 1);
            ddlModule.DataBind();
            ddlModule.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            int intType = 1;
            if (radIssue.Checked == true)
                intType = 0;
            int intId = Int32.Parse(lblId.Text);
            if (intId == 0)
                oSupport.Add(txtTitle.Text, Int32.Parse(ddlModule.SelectedItem.Value), intType, txtDescription.Text, intProfile);
            else
                oSupport.Update(intId, txtTitle.Text, Int32.Parse(ddlModule.SelectedItem.Value), intType, txtDescription.Text, intProfile);
            Response.Redirect(oPage.GetFullLink(intPage) + "?save=true");
        }
    }
}