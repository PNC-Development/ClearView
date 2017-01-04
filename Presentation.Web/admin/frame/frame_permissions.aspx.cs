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
    public partial class frame_permissions : BasePage
    {
   
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    protected int intProfile;
    protected Permissions oPermission;
    protected Applications oApplication;
    protected Groups oGroup;
    private DataSet ds;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
        else
            Reload();
        oPermission = new Permissions(intProfile, dsn);
        oApplication = new Applications(intProfile, dsn);
        oGroup = new Groups(intProfile, dsn);
        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
        {
            if (!IsPostBack)
            {
                hdnApplication.Value = "0";
                lblApplication.Text = "No Category";
                hdnId.Value = Request.QueryString["id"];
                LoadLists();
                if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                {
                    hdnApplication.Value = Request.QueryString["applicationid"];
                    int intApplication = Int32.Parse(hdnApplication.Value);
                    lblApplication.Text = oApplication.GetName(intApplication);
                }
                if (hdnId.Value == "0")
                    btnDelete.Visible = false;
                else
                {
                    btnSave.Text = "Update";
                    LoadProperties();
                    btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this permission?');");
                }
                btnClose.Attributes.Add("onclick", "return HidePanel();");
            }
        }
    }
    private void LoadLists()
    {
        ddlGroup.DataTextField = "name";
        ddlGroup.DataValueField = "groupid";
        ddlGroup.DataSource = oGroup.Gets(1);
        ddlGroup.DataBind();
        ddlGroup.Items.Insert(0, new ListItem("-- SELECT --", "0"));
    }
    private void LoadProperties()
    {
        ds = oPermission.Get(Int32.Parse(hdnId.Value));
        if (ds.Tables[0].Rows.Count > 0)
        {
            int intApplication = Int32.Parse(ds.Tables[0].Rows[0]["applicationid"].ToString());
            hdnApplication.Value = intApplication.ToString();
            if (intApplication == 0)
                lblApplication.Text = "No Application";
            else
                lblApplication.Text = oApplication.GetName(intApplication);
            ddlGroup.SelectedValue = ds.Tables[0].Rows[0]["groupid"].ToString();
            ddlPermission.SelectedValue = ds.Tables[0].Rows[0]["permission"].ToString();
        }
    }
    protected void btnSave_Click(Object Sender, EventArgs e)
    {
        if (Request.Form[hdnId.UniqueID] == "0")
            oPermission.Add(Int32.Parse(hdnApplication.Value), Int32.Parse(ddlGroup.SelectedItem.Value), Int32.Parse(ddlPermission.SelectedItem.Value));
        else
            oPermission.Update(Int32.Parse(Request.Form[hdnId.UniqueID]), Int32.Parse(hdnApplication.Value), Int32.Parse(ddlGroup.SelectedItem.Value), Int32.Parse(ddlPermission.SelectedItem.Value));
        Reload();
    }
    protected void btnDelete_Click(Object Sender, EventArgs e)
    {
        oPermission.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]));
        Reload();
    }
    private void Reload()
    {
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
    }
    }
}
