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
    public partial class asset_zones : BasePage
    {
    
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    protected Zones oZones;
    protected int intProfile;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cookies["loginreferrer"].Value = "/admin/asset/asset_zones.aspx";
        Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
        if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
        else
            Response.Redirect("/admin/login.aspx");
        oZones = new Zones(intProfile, dsn);
        if (!IsPostBack)
        {

            if (Request.QueryString["roomid"] != null && Request.QueryString["roomid"] != "")
            {
                hdnRoomId.Value = Request.QueryString["roomid"];
                LoadRoom();
                LoopRepeater();
            }

            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=A_RACK" + "',false,400,400);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            btnCancel.Attributes.Add("onclick", "return Cancel();");

            btnAdd.Attributes.Add("onclick", "return ValidateHidden0('" + hdnRoomId.ClientID + "','" + btnSelectLocation.ClientID + "','Please select a location')" +
             " && ValidateText('" + txtName.ClientID + "','Please enter a rack name')" +
             " && ValidateNumber('" + txtVLan.ClientID + "','Please enter vLAN')" +
             " && ValidateText('" + txtDescription.ClientID + "','Please select a description')" +
             ";");

            btnSelectLocation.Attributes.Add("onclick", "return LoadLocationRoomRack('" + "room" + "','" + hdnRoomId.ClientID + "', '" + txtLocation.ClientID + "', '" + txtRoom.ClientID + "','');");

        }
    }
        
    private void LoopRepeater()
    {
        if (hdnRoomId.Value == "") hdnRoomId.Value = "0";

        DataSet ds = oZones.GetByRoom(Int32.Parse(hdnRoomId.Value));
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
        Response.Redirect(Request.Path + "?roomid=" + hdnRoomId.Value + "&sort=" + strSort);

    }
    protected void btnAdd_Click(Object Sender, EventArgs e)
    {
        if (hdnRoomId.Value == "")
            hdnRoomId.Value = "0";

    if (Request.Form[hdnId.UniqueID] == "0")
    {
         DataSet dsZone = oZones.Gets(Int32.Parse(hdnRoomId.Value), txtName.Text.Trim());
         if (dsZone.Tables[0].Rows.Count > 0)
         {
             Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "DuplicateZone", "<script type=\"text/javascript\">alert('Zone " + txtName.Text + " already exists for selected room.');<" + "/" + "script>");
         }
         else
         {
             oZones.Add(txtName.Text, Int32.Parse(hdnRoomId.Value), Int32.Parse(txtVLan.Text.Trim()), txtDescription.Text.Trim(), (chkEnabled.Checked ? 1 : 0));
         }
    }
    else
    {
        Boolean blDuplicate = false;
        DataSet dsZone = oZones.Gets(Int32.Parse(hdnRoomId.Value), txtName.Text.Trim());
        if (dsZone.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow drZone in dsZone.Tables[0].Rows)
            {
                if (drZone["ZoneId"].ToString() != Request.Form[hdnId.UniqueID].ToString() && blDuplicate == false)
                {
                    blDuplicate = true;
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "DuplicateZone", "<script type=\"text/javascript\">alert('Zone " + txtName.Text + " already exists for selected room.');<" + "/" + "script>");
                }
            }
        }
        if (blDuplicate==false)
            oZones.Update(Int32.Parse(Request.Form[hdnId.UniqueID]), txtName.Text.Trim(), Int32.Parse(hdnRoomId.Value), Int32.Parse(txtVLan.Text.Trim()), txtDescription.Text.Trim(), (chkEnabled.Checked ? 1 : 0));
    
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
            //oZones.UpdateOrder(intId, intCount);
        }
    }
    LoopRepeater();
    //Response.Redirect(Request.Path);
    }
    protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
{
    ImageButton oButton = (ImageButton)Sender;
    oZones.Enable(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
    Response.Redirect(Request.Path + "?roomid=" + hdnRoomId.Value);
}
    protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
{
    ImageButton oButton = (ImageButton)Sender;
    oZones.Delete(Int32.Parse(oButton.CommandArgument),1);
    Response.Redirect(Request.Path + "?roomid=" + hdnRoomId.Value);
}
    protected void btnDelete_Click(Object Sender, EventArgs e)
    {
        oZones.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]),1);
        Response.Redirect(Request.Path + "?roomid=" + hdnRoomId.Value);
    }

    protected void btnGetZones_Click(object sender, EventArgs e)
    {
        if (hdnRoomId.Value == "") hdnRoomId.Value = "0";
        Response.Redirect(Request.Path + "?roomid=" + hdnRoomId.Value);
    }

    protected void LoadRoom()
    {
        RoomsNew oRooms=new RoomsNew(0,dsn);
        if (hdnRoomId.Value == "") hdnRoomId.Value = "0";
        DataSet dsRoom = oRooms.Gets(Int32.Parse(hdnRoomId.Value));
        if (dsRoom.Tables[0].Rows.Count > 0)
        {
            hdnLocationId.Value = dsRoom.Tables[0].Rows[0]["locationid"].ToString();
            txtLocation.Text = dsRoom.Tables[0].Rows[0]["location"].ToString();
            txtRoom.Text = dsRoom.Tables[0].Rows[0]["room"].ToString();
            hdnRoomId.Value = dsRoom.Tables[0].Rows[0]["roomid"].ToString();
        }
    }

    protected void btnSelectLocation_Click(object sender, EventArgs e)
    {
        hdnRoomId.Value = "0";
        LoadRoom();
        LoopRepeater();
    }

    }
}
