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
    public partial class build_locations : BasePage
    {
        
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
    protected BuildLocation oBuildLocation;
    protected Locations oLocation;
    protected Models oModel;
        protected Classes oClass;
    protected int intProfile;
    protected string strLocationFinal = "";
        protected string strLocationBuild = "";
    protected int intID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cookies["loginreferrer"].Value = "/admin/forecast/build_locations.aspx";
        Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
        if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
        else
            Response.Redirect("/admin/login.aspx");
        oBuildLocation = new BuildLocation(intProfile, dsn);
        oLocation = new Locations(intProfile, dsn);
        oModel = new Models(intProfile, dsn);
        oClass = new Classes(intProfile, dsn);

        int intLocationFinal = intLocation;
        int intLocationBuild = intLocation;

        if (!IsPostBack)
            LoadLists();

        if (Request.QueryString["id"] == null)
            LoopRepeater();
        else
        {
            intID = Int32.Parse(Request.QueryString["id"]);
            if (intID == 0)
            {
                panAdd.Visible = true;
                btnDelete.Enabled = false;
            }
            else
            {
                panAdd.Visible = true;
                if (!IsPostBack)
                {
                    DataSet ds = oBuildLocation.Get(intID);
                    int intClassFinal = 0;
                    Int32.TryParse(ds.Tables[0].Rows[0]["classid"].ToString(), out intClassFinal);
                    ddlClass.SelectedValue = intClassFinal.ToString();
                    ddlEnvironment.Enabled = true;
                    ddlEnvironment.DataTextField = "name";
                    ddlEnvironment.DataValueField = "id";
                    ddlEnvironment.DataSource = oClass.GetEnvironment(intClassFinal, 0);
                    ddlEnvironment.DataBind();
                    ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                    int intEnvironmentFinal = 0;
                    Int32.TryParse(ds.Tables[0].Rows[0]["environmentid"].ToString(), out intEnvironmentFinal);
                    ddlEnvironment.SelectedValue = intEnvironmentFinal.ToString();
                    hdnEnvironmentFinal.Value = intEnvironmentFinal.ToString();
                    Int32.TryParse(ds.Tables[0].Rows[0]["addressid"].ToString(), out intLocationFinal);

                    int intClassBuild = 0;
                    Int32.TryParse(ds.Tables[0].Rows[0]["build_classid"].ToString(), out intClassBuild);
                    ddlClassBuild.SelectedValue = intClassBuild.ToString();
                    ddlEnvironmentBuild.Enabled = true;
                    ddlEnvironmentBuild.DataTextField = "name";
                    ddlEnvironmentBuild.DataValueField = "id";
                    ddlEnvironmentBuild.DataSource = oClass.GetEnvironment(intClassBuild, 0);
                    ddlEnvironmentBuild.DataBind();
                    ddlEnvironmentBuild.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                    int intEnvironmentBuild = 0;
                    Int32.TryParse(ds.Tables[0].Rows[0]["build_environmentid"].ToString(), out intEnvironmentBuild);
                    ddlEnvironmentBuild.SelectedValue = intEnvironmentBuild.ToString();
                    hdnEnvironmentBuild.Value = intEnvironmentBuild.ToString();
                    Int32.TryParse(ds.Tables[0].Rows[0]["build_addressid"].ToString(), out intLocationBuild);

                    int intModel = 0;
                    Int32.TryParse(ds.Tables[0].Rows[0]["modelid"].ToString(), out intModel);
                    lblModel.Text = oModel.Get(intModel, "name");
                    hdnModel.Value = intModel.ToString();

                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                    btnAdd.Text = "Update";
                }
            }
        }

        strLocationFinal = oLocation.LoadDDL("ddlStateFinal", "ddlCityFinal", "ddlAddressFinal", hdnLocationFinal.ClientID, intLocationFinal, true, "ddlCommonFinal");
        hdnLocationFinal.Value = intLocationFinal.ToString();
        strLocationBuild = oLocation.LoadDDL("ddlStateBuild", "ddlCityBuild", "ddlAddressBuild", hdnLocationBuild.ClientID, intLocationBuild, true, "ddlCommonBuild");
        hdnLocationBuild.Value = intLocationBuild.ToString();
        btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
        btnModel.Attributes.Add("onclick", "return OpenWindow('MODELBROWSER','" + hdnModel.ClientID + "','&control=" + hdnModel.ClientID + "&controltext=" + lblModel.ClientID + "',false,400,600);");
        ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',0);");
        ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironmentFinal.ClientID + "');");
        ddlClassBuild.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironmentBuild.ClientID + "',0);");
        ddlEnvironmentBuild.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironmentBuild.ClientID + "','" + hdnEnvironmentBuild.ClientID + "');");
    }
    private void LoadLists()
    {
        Classes oClass = new Classes(0, dsn);
        ddlClass.DataTextField = "name";
        ddlClass.DataValueField = "id";
        ddlClass.DataSource = oClass.Gets(1);
        ddlClass.DataBind();
        ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        ddlClassBuild.DataTextField = "name";
        ddlClassBuild.DataValueField = "id";
        ddlClassBuild.DataSource = oClass.Gets(1);
        ddlClassBuild.DataBind();
        ddlClassBuild.Items.Insert(0, new ListItem("-- SELECT --", "0"));
    }
    private void LoopRepeater()
    {
        panView.Visible = true;
        DataSet ds = oBuildLocation.Gets(0);
        DataView dv = ds.Tables[0].DefaultView;
        if (Request.QueryString["sort"] != null)
            dv.Sort = Request.QueryString["sort"].ToString();
        rptView.DataSource = dv;
        rptView.DataBind();
        foreach (RepeaterItem ri in rptView.Items)
        {
            ImageButton oDelete = (ImageButton)ri.FindControl("btnDelete");
            oDelete.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this item?');");
            //ImageButton oEnable = (ImageButton)ri.FindControl("btnEnable");
            //if (oEnable.ImageUrl == "/admin/images/enabled.gif")
            //{
            //    oEnable.ToolTip = "Click to disable";
            //    oEnable.Attributes.Add("onClick", "return confirm('Are you sure you want to disable this item?');");
            //}
            //else
            //    oEnable.ToolTip = "Click to enable";
        }
    }
    public void OrderView(Object Sender, EventArgs e)
    {
        LinkButton oButton = (LinkButton)Sender;
        string strSort;
        if (Request.QueryString["sort"] == null)
            strSort = oButton.CommandArgument + " ASC";
        else
            if (Request.QueryString["sort"].ToString() == (oButton.CommandArgument + " ASC"))
                strSort = oButton.CommandArgument + " DESC";
            else
                strSort = oButton.CommandArgument + " ASC";
        Response.Redirect(Request.Path + "?sort=" + strSort);
    }
    protected void btnNew_Click(Object Sender, EventArgs e)
    {
        Response.Redirect(Request.Path + "?id=0");
    }
    protected void btnAdd_Click(Object Sender, EventArgs e)
    {
        int intModel = 0;
        if (Request.Form[hdnModel.UniqueID] != "")
            intModel = Int32.Parse(Request.Form[hdnModel.UniqueID]);
        int intLocationFinal = 0;
        if (Request.Form[hdnLocationFinal.UniqueID] != "")
            intLocationFinal = Int32.Parse(Request.Form[hdnLocationFinal.UniqueID]);
        int intLocationBuild = 0;
        if (Request.Form[hdnLocationBuild.UniqueID] != "")
            intLocationBuild = Int32.Parse(Request.Form[hdnLocationBuild.UniqueID]);
        if (intID == 0)
            oBuildLocation.Add(Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironmentFinal.UniqueID]), intLocationFinal, Int32.Parse(ddlClassBuild.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironmentBuild.UniqueID]), intLocationBuild, intModel, (chkEnabled.Checked ? 1 : 0));
        else
            oBuildLocation.Update(intID, Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironmentFinal.UniqueID]), intLocationFinal, Int32.Parse(ddlClassBuild.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironmentBuild.UniqueID]), intLocationBuild, intModel, (chkEnabled.Checked ? 1 : 0));
        Response.Redirect(Request.Path);
    }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
    {
        ImageButton oButton = (ImageButton)Sender;
        oBuildLocation.Delete(Int32.Parse(oButton.CommandArgument));
        Response.Redirect(Request.Path);
    }
    protected void btnCancel_Click(Object Sender, EventArgs e)
    {
        Response.Redirect(Request.Path);
    }
    protected void btnDelete_Click(Object Sender, EventArgs e)
    {
        oBuildLocation.Delete(intID);
        Response.Redirect(Request.Path);
    }

    }
}
