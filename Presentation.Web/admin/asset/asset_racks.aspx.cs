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
    public partial class asset_racks : BasePage
    {
    
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    protected RacksNew oRacks;
    protected Asset oAsset;
    protected ModelsProperties oModelsProperties;
    protected int intProfile;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cookies["loginreferrer"].Value = "/admin/asset/asset_racks.aspx";
        Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
        if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
        else
            Response.Redirect("/admin/login.aspx");
        oRacks = new RacksNew(intProfile, dsn);
        oAsset = new Asset(intProfile, dsnAsset);
        oModelsProperties = new ModelsProperties(intProfile, dsn);
        if (!IsPostBack)
        {

            LoadList();
            if (Request.QueryString["zoneid"] != null && Request.QueryString["zoneid"] != "")
            {
                hdnZoneId.Value = Request.QueryString["zoneid"];
                LoadZones();
                LoopRepeater();
            }

            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=A_RACK" + "',false,400,400);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            btnCancel.Attributes.Add("onclick", "return Cancel();");

            btnAdd.Attributes.Add("onclick", "return ValidateHidden0('" + hdnZoneId.ClientID + "','" + btnSelectLocation.ClientID + "','Please select a location')" +
             " && ValidateText('" + txtName.ClientID + "','Please enter a rack name')" +
             " && ValidateText('" + txtAssetSerial.ClientID + "','Please enter asset serial #')" +
             " && ValidateText('" + txtAssetTag.ClientID + "','Please enter asset tag')" +
             " && ValidateDropDown('" + ddlModel.ClientID + "','Please select a model')" +
             " && ValidateText('" + txtDescription.ClientID + "','Please select a description')" +
             ";");

            btnSelectLocation.Attributes.Add("onclick", "return LoadLocationRoomRack('" + "zone" + "','" + hdnZoneId.ClientID + "', '" + txtLocation.ClientID + "', '" + txtRoom.ClientID + "','" + txtZone.ClientID + "');");

        }
    }
        private void LoadList()
        {
            ddlModel.DataTextField = "name";
            ddlModel.DataValueField = "id";
            ddlModel.DataSource = oModelsProperties.GetPlatforms(0, 4, 0);
            ddlModel.DataBind();
            ddlModel.Items.Insert(0, new ListItem("-- SELECT --", "0"));

        }
    private void LoopRepeater()
    {
        if (hdnZoneId.Value == "") hdnZoneId.Value = "0";

        DataSet ds = oRacks.GetByZone(Int32.Parse(hdnZoneId.Value));
        DataView dv = ds.Tables[0].DefaultView;
        if (Request.QueryString["sort"] != null)
            dv.Sort = Request.QueryString["sort"].ToString();
        rptView.DataSource = dv;
        rptView.DataBind();
        foreach (RepeaterItem ri in rptView.Items)
        {
            ImageButton oDelete = (ImageButton)ri.FindControl("btnDelete");
            oDelete.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this item?');");
            ImageButton oEnable = (ImageButton)ri.FindControl("btnEnable");
            if (oEnable.ImageUrl == "/admin/images/enabled.gif")
            {
                oEnable.ToolTip = "Click to disable";
                oEnable.Attributes.Add("onClick", "return confirm('Are you sure you want to disable this item?');");
            }
            else
                oEnable.ToolTip = "Click to enable";
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
        Response.Redirect(Request.Path + "?zoneid=" + hdnZoneId.Value + "&sort=" + strSort);

    }
    protected void btnAdd_Click(Object Sender, EventArgs e)
    {
        if (hdnZoneId.Value == "")
            hdnZoneId.Value = "0";

    if (Request.Form[hdnId.UniqueID] == "0")
    {
        DataSet dsRack = oRacks.Gets(Int32.Parse(hdnZoneId.Value), txtName.Text.Trim());
         if (dsRack.Tables[0].Rows.Count > 0)
         {
             Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "DuplicateRack", "<script type=\"text/javascript\">alert('Rack " + txtName.Text + " already exists for selected zone.');<" + "/" + "script>");
         }
         else
         {
             int intAsset = 0;
             intAsset = oAsset.Add(0, 0, Int32.Parse(ddlModel.SelectedValue), txtAssetSerial.Text.Trim(), txtAssetTag.Text.Trim(), (int)AssetStatus.Available, intProfile, DateTime.Now, 0, 1, 0, "");
             oRacks.Add(txtName.Text, intAsset, Int32.Parse(hdnZoneId.Value), txtDescription.Text.Trim(), (chkEnabled.Checked ? 1 : 0));
         }
    }
    else
    {
        Boolean blDuplicate = false;
        DataSet dsRack = oRacks.Gets(Int32.Parse(hdnZoneId.Value), txtName.Text.Trim());
        if (dsRack.Tables[0].Rows.Count > 0)
         {
             foreach (DataRow drRack in dsRack.Tables[0].Rows)
             {
                 if (drRack["RackId"].ToString() != Request.Form[hdnId.UniqueID].ToString() && blDuplicate==false)
                 {
                     blDuplicate = true;
                     Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "DuplicateRack", "<script type=\"text/javascript\">alert('Rack " + txtName.Text + " already exists for selected zone.');<" + "/" + "script>");
                 }
             }
         }
         if (blDuplicate==false)
         {
             if (hdnAssetId.Value == "" || hdnAssetId.Value == "0")
             {
                 int intAsset = 0;
                 intAsset = oAsset.Add(0, 0, Int32.Parse(ddlModel.SelectedValue), txtAssetSerial.Text.Trim(), txtAssetTag.Text.Trim(), (int)AssetStatus.Available, intProfile, DateTime.Now, 0, 1, 0, "");
             }
             else
                 oAsset.Update(Int32.Parse(hdnAssetId.Value), Int32.Parse(ddlModel.SelectedValue), txtAssetSerial.Text.Trim(), txtAssetTag.Text.Trim(), 0);

             oRacks.Update(Int32.Parse(Request.Form[hdnId.UniqueID]), txtName.Text.Trim(), Int32.Parse(hdnAssetId.Value), Int32.Parse(hdnZoneId.Value), txtDescription.Text.Trim(), (chkEnabled.Checked ? 1 : 0));
         }
    }
    if (Request.Form[hdnOrder.UniqueID] != "")
    {
        string strOrder = Request.Form[hdnOrder.UniqueID];
        int intCount = 0;
        while (strOrder != "")
        {
            intCount++;
            int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
            strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
            //oRacks.UpdateOrder(intId, intCount);
        }
    }
    LoopRepeater();
    //Response.Redirect(Request.Path);
    }
    protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
{
    ImageButton oButton = (ImageButton)Sender;
    oRacks.Enable(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
    Response.Redirect(Request.Path + "?zoneid=" + hdnZoneId.Value);
}
    protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
{
    ImageButton oButton = (ImageButton)Sender;
    oRacks.Delete(Int32.Parse(oButton.CommandArgument),1);
    Response.Redirect(Request.Path + "?zoneid=" + hdnZoneId.Value);
}
    protected void btnDelete_Click(Object Sender, EventArgs e)
    {
        oRacks.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]),1);
        Response.Redirect(Request.Path + "?zoneid=" + hdnZoneId.Value);
    }

    protected void btnGetRacks_Click(object sender, EventArgs e)
    {
        if (hdnZoneId.Value == "") hdnZoneId.Value = "0";
        Response.Redirect(Request.Path + "?zoneid=" + hdnZoneId.Value);
    }

    protected void LoadZones()
    {
        Zones oZone = new Zones(0, dsn);
        if (hdnZoneId.Value == "") hdnZoneId.Value = "0";
        DataSet dsZone = oZone.Gets(Int32.Parse(hdnZoneId.Value));
        if (dsZone.Tables[0].Rows.Count > 0)
        {
            hdnLocationId.Value=dsZone.Tables[0].Rows[0]["locationid"].ToString();
            txtLocation.Text=dsZone.Tables[0].Rows[0]["location"].ToString();
            txtRoom.Text = dsZone.Tables[0].Rows[0]["room"].ToString();
            hdnZoneId.Value = dsZone.Tables[0].Rows[0]["zoneid"].ToString();
            txtZone.Text = dsZone.Tables[0].Rows[0]["Zone"].ToString();
        }
    }

    protected void btnSelectLocation_Click(object sender, EventArgs e)
    {
        hdnZoneId.Value = "0";
        LoadZones();
        LoopRepeater();
    }

    }
}
