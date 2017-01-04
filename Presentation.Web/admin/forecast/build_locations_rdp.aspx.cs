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
    public partial class build_locations_rdp : BasePage
    {

    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
    protected BuildLocation oBuildLocation;
    protected Classes oClass;
    protected Locations oLocation;
    protected Resiliency oResiliency;
    protected int intProfile;
    protected string strLocation = "";
    protected int intID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cookies["loginreferrer"].Value = "/admin/forecast/build_locations_rdp.aspx";
        Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
        if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
        else
            Response.Redirect("/admin/login.aspx");
        oBuildLocation = new BuildLocation(intProfile, dsn);
        oClass = new Classes(intProfile, dsn);
        oLocation = new Locations(intProfile, dsn);
        oResiliency = new Resiliency(intProfile, dsn);

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
                    DataSet ds = oBuildLocation.GetRDP(intID);
                    int intClass = 0;
                    Int32.TryParse(ds.Tables[0].Rows[0]["classid"].ToString(), out intClass);
                    ddlClass.SelectedValue = intClass.ToString();
                    ddlEnvironment.Enabled = true;
                    ddlEnvironment.DataTextField = "name";
                    ddlEnvironment.DataValueField = "id";
                    ddlEnvironment.DataSource = oClass.GetEnvironment(intClass, 0);
                    ddlEnvironment.DataBind();
                    ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                    int intEnv = 0;
                    Int32.TryParse(ds.Tables[0].Rows[0]["environmentid"].ToString(), out intEnv);
                    ddlEnvironment.SelectedValue = intEnv.ToString();
                    hdnEnvironment.Value = intEnv.ToString();
                    Int32.TryParse(ds.Tables[0].Rows[0]["addressid"].ToString(), out intLocation);
                    txtRDP_Schedule_WS.Text = ds.Tables[0].Rows[0]["rdp_schedule_ws"].ToString();
                    txtRDP_Computer_WS.Text = ds.Tables[0].Rows[0]["rdp_computer_ws"].ToString();
                    txtRDP_MDT_WS.Text = ds.Tables[0].Rows[0]["rdp_mdt_ws"].ToString();
                    txtJumpstartCGI.Text = ds.Tables[0].Rows[0]["jumpstart_cgi"].ToString();
                    ddlJumpstartBuildType.SelectedValue = ds.Tables[0].Rows[0]["jumpstart_build_type"].ToString();
                    txtBladeVLAN.Text = ds.Tables[0].Rows[0]["blade_vlan"].ToString();
                    txtVMwareVLAN.Text = ds.Tables[0].Rows[0]["vmware_vlan"].ToString();
                    txtVSphereVLAN.Text = ds.Tables[0].Rows[0]["vsphere_vlan"].ToString();
                    txtDellVLAN.Text = ds.Tables[0].Rows[0]["dell_vlan"].ToString();
                    txtDellVmwareVLAN.Text = ds.Tables[0].Rows[0]["dell_vmware_vlan"].ToString();
                    ddlResiliency.SelectedValue = ds.Tables[0].Rows[0]["resiliencyid"].ToString();
                    ddlSource.SelectedValue = ds.Tables[0].Rows[0]["source"].ToString();
                    chkServerAltiris.Checked = (ds.Tables[0].Rows[0]["server_altiris"].ToString() == "1");
                    chkServerMDT.Checked = (ds.Tables[0].Rows[0]["server_mdt"].ToString() == "1");
                    chkWorkstation.Checked = (ds.Tables[0].Rows[0]["workstation"].ToString() == "1");
                    chkZones.Checked = (ds.Tables[0].Rows[0]["zones"].ToString() == "1");
                    btnZones.Enabled = chkZones.Checked;

                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                    btnAdd.Text = "Update";
                }
            }
        }

        strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, intLocation, true, "ddlCommon");
        hdnLocation.Value = intLocation.ToString();
        ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',0);");
        ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
        btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
        btnZones.Attributes.Add("onclick", "return OpenWindow('RDP_ZONES','','" + intID.ToString() + "',false,'650',500);");
    }
    private void LoadLists()
    {
        ddlClass.DataTextField = "name";
        ddlClass.DataValueField = "id";
        ddlClass.DataSource = oClass.Gets(1);
        ddlClass.DataBind();
        ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        ddlResiliency.DataValueField = "id";
        ddlResiliency.DataTextField = "name";
        ddlResiliency.DataSource = oResiliency.Gets(1);
        ddlResiliency.DataBind();
        ddlResiliency.Items.Insert(0, new ListItem("-- SELECT --", "0"));
    }
    private void LoopRepeater()
    {
        panView.Visible = true;
        DataSet ds = oBuildLocation.GetRDPs(0);
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

    protected void OrderView(Object Sender, EventArgs e)
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
        int intLocation = 0;
        if (Request.Form[hdnLocation.UniqueID] != "")
            intLocation = Int32.Parse(Request.Form[hdnLocation.UniqueID]);
        if (intID == 0)
            oBuildLocation.AddRDP(Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), intLocation, txtRDP_Schedule_WS.Text, txtRDP_Computer_WS.Text, txtRDP_MDT_WS.Text, txtJumpstartCGI.Text, ddlJumpstartBuildType.SelectedItem.Value, txtBladeVLAN.Text, txtVMwareVLAN.Text, txtVSphereVLAN.Text, txtDellVLAN.Text, txtDellVmwareVLAN.Text, Int32.Parse(ddlResiliency.SelectedItem.Value), ddlSource.SelectedItem.Value, (chkServerAltiris.Checked ? 1 : 0), (chkServerMDT.Checked ? 1 : 0), (chkWorkstation.Checked ? 1 : 0), (chkZones.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
        else
            oBuildLocation.UpdateRDP(intID, Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), intLocation, txtRDP_Schedule_WS.Text, txtRDP_Computer_WS.Text, txtRDP_MDT_WS.Text, txtJumpstartCGI.Text, ddlJumpstartBuildType.SelectedItem.Value, txtBladeVLAN.Text, txtVMwareVLAN.Text, txtVSphereVLAN.Text, txtDellVLAN.Text, txtDellVmwareVLAN.Text, Int32.Parse(ddlResiliency.SelectedItem.Value), ddlSource.SelectedItem.Value, (chkServerAltiris.Checked ? 1 : 0), (chkServerMDT.Checked ? 1 : 0), (chkWorkstation.Checked ? 1 : 0), (chkZones.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
        Response.Redirect(Request.Path);
    }
    protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
    {
        ImageButton oButton = (ImageButton)Sender;
        oBuildLocation.DeleteRDP(Int32.Parse(oButton.CommandArgument));
        Response.Redirect(Request.Path);
    }
    protected void btnCancel_Click(Object Sender, EventArgs e)
    {
        Response.Redirect(Request.Path);
    }
    protected void btnDelete_Click(Object Sender, EventArgs e)
    {
        oBuildLocation.DeleteRDP(intID);
        Response.Redirect(Request.Path);
    }

    }
}
