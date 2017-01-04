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
    public partial class asset_racks_old : BasePage
    {
    
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    protected Racks oRacks;
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
        oRacks = new Racks(intProfile, dsn);
        oAsset = new Asset(intProfile, dsnAsset);
        oModelsProperties = new ModelsProperties(intProfile, dsn);
        if (!IsPostBack)
        {
            LoadList();
            LoopRepeater();
            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=A_RACK" + "',false,400,400);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            btnCancel.Attributes.Add("onclick", "return Cancel();");

            btnLocation.Attributes.Add("onclick", "return OpenWindow('LOCATION_BROWSER','" + hdnLocationId.ClientID + "','&control=" + hdnLocationId.ClientID + "&controltext=" + lblLocation.ClientID + "',false,400,600);");
            
            btnRoom.Attributes.Add("onclick", "return ValidateHidden0('" + hdnLocationId.ClientID + "','btnLocation','Please select a location')" +
            " && OpenWindowBasedOnParentSelection('ROOM_BROWSER','" + hdnRoomId.ClientID + "','" + hdnLocationId.ClientID + "','&control=" + hdnRoomId.ClientID + "&controltext=" + lblRoom.ClientID + "',false,400,600)" +
            ";");

            btnAdd.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a rack name')" +
            " && ValidateText('" + txtAssetSerial.ClientID + "','Please enter asset serial #')" +
            " && ValidateText('" + txtAssetTag.ClientID + "','Please enter asset tag')" +
            " && ValidateDropDown('" + ddlModel.ClientID + "','Please select a model')" +
            " && ValidateHidden0('" + hdnLocationId.ClientID + "','btnLocation','Please select a location')" +
            " && ValidateHidden0('" + hdnRoomId.ClientID + "','btnRoom','Please select a room')" +
            ";");



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
        DataSet ds = oRacks.Gets(0);
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
        Response.Redirect(Request.Path + "?sort=" + strSort);
    }
    protected void btnAdd_Click(Object Sender, EventArgs e)
    {
    if (hdnLocationId.Value == "0") hdnLocationId.Value = "0";
    if (hdnRoomId.Value == "0") hdnRoomId.Value = "0";

    if (Request.Form[hdnId.UniqueID] == "0")
    {
        int intAsset = 0;
        intAsset = oAsset.Add(0, 0, Int32.Parse(ddlModel.SelectedValue), txtAssetSerial.Text.Trim(), txtAssetTag.Text.Trim(), (int)AssetStatus.Available, intProfile, DateTime.Now, 0, 1, 0, "");
        //oRacks.Add(txtName.Text, (chkEnabled.Checked ? 1 : 0));
        oRacks.Add(intAsset, txtName.Text, Int32.Parse(hdnRoomId.Value), (chkEnabled.Checked ? 1 : 0), intProfile);
    }
    else
    {
        oAsset.Update(Int32.Parse(hdnAssetId.Value), Int32.Parse(ddlModel.SelectedValue), txtAssetSerial.Text.Trim(), txtAssetTag.Text.Trim(), 0);
        oRacks.Update(Int32.Parse(Request.Form[hdnId.UniqueID]), Int32.Parse(hdnAssetId.Value), txtName.Text.Trim(), Int32.Parse(hdnRoomId.Value), (chkEnabled.Checked ? 1 : 0), intProfile);
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
            oRacks.UpdateOrder(intId, intCount);
        }
    }
    Response.Redirect(Request.Path);
    }
    protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
{
    ImageButton oButton = (ImageButton)Sender;
    oRacks.Enable(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
    Response.Redirect(Request.Path);
}
    protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
{
    ImageButton oButton = (ImageButton)Sender;
    oRacks.Delete(Int32.Parse(oButton.CommandArgument));
    Response.Redirect(Request.Path);
}
    protected void btnDelete_Click(Object Sender, EventArgs e)
    {
        oRacks.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]));
        Response.Redirect(Request.Path);
    }

    }
}
