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
    public partial class asset_rooms : BasePage
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected RoomsNew oRooms;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/asset/asset_rooms.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oRooms = new RoomsNew(intProfile, dsn);
            if (!IsPostBack)
            {
                if (Request.QueryString["locationid"] != null && Request.QueryString["locationid"] != "")
                {
                    hdnLocationId.Value = Request.QueryString["locationid"];
                    LoadLocation();
                    LoopRepeater();
                }
                btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=A_ROOM" + "',false,400,400);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                btnAdd.Attributes.Add("onclick", "return ValidateHidden0('" + hdnLocationId.ClientID + "','" + btnSelectLocation.ClientID + "','Please select a location')" +
               " &&  ValidateText('" + txtName.ClientID + "','Please enter a room name')" +
               " &&  ValidateText('" + txtDescription.ClientID + "','Please select a description')" +
               ";");

                btnSelectLocation.Attributes.Add("onclick", "return LoadLocationRoomRack('" + "location" + "','" + hdnLocationId.ClientID + "', '" + txtLocation.ClientID + "','','');");

            }
        }
        private void LoopRepeater()
        {
            if (hdnLocationId.Value == "") hdnLocationId.Value = "0";

            DataSet ds = oRooms.GetByLocation(Int32.Parse(hdnLocationId.Value));
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
            Response.Redirect(Request.Path + "?locationid="+hdnLocationId.Value+ "&sort=" + strSort);
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (hdnLocationId.Value == "")
                hdnLocationId.Value = "0";
           
           
                if (Request.Form[hdnId.UniqueID] == "0")
                {
                    DataSet dsRoom = oRooms.Gets(Int32.Parse(hdnLocationId.Value), txtName.Text.Trim());
                    if (dsRoom.Tables[0].Rows.Count > 0)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "DuplicateRoom", "<script type=\"text/javascript\">alert('Room " + txtName.Text + " already exists for selected location.');<" + "/" + "script>");
                    }
                    else
                        oRooms.Add(txtName.Text, Int32.Parse(hdnLocationId.Value), txtDescription.Text.Trim(), (chkEnabled.Checked ? 1 : 0));
                }
                else
                {
                    Boolean blDuplicate = false;
                    DataSet dsRoom = oRooms.Gets(Int32.Parse(hdnLocationId.Value), txtName.Text.Trim());
                    if (dsRoom.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drRoom in dsRoom.Tables[0].Rows)
                        {
                            if (drRoom["RoomId"].ToString() != Request.Form[hdnId.UniqueID].ToString() && blDuplicate == false)
                            {
                                blDuplicate=true;
                                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "DuplicateRoom", "<script type=\"text/javascript\">alert('Room " + txtName.Text + " already exists for selected location.');<" + "/" + "script>");
                            }
                        }
                    }
                   if (blDuplicate==false)
                        oRooms.Update(Int32.Parse(Request.Form[hdnId.UniqueID]), txtName.Text, Int32.Parse(hdnLocationId.Value), txtDescription.Text.Trim(), (chkEnabled.Checked ? 1 : 0));
                    
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
                        //oRooms.UpdateOrder(intId, intCount);
                    }
                }
                //Response.Redirect(Request.Path);
                LoopRepeater();
            
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oRooms.Enable(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path + "?locationid=" + hdnLocationId.Value);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oRooms.Delete(Int32.Parse(oButton.CommandArgument),1);
            Response.Redirect(Request.Path + "?locationid=" + hdnLocationId.Value);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oRooms.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]),1);
            Response.Redirect(Request.Path + "?locationid=" + hdnLocationId.Value);
        }


        protected void btnGetRooms_Click(object sender, EventArgs e)
        {
            Locations oLocation = new Locations(0, dsn);
            if (hdnLocationId.Value == "") hdnLocationId.Value = "0";

            Response.Redirect(Request.Path + "?locationid=" + hdnLocationId.Value);
        }

        protected void LoadLocation()
        {
            Locations oLocation = new Locations(0, dsn);
            if (hdnLocationId.Value == "") hdnLocationId.Value = "0";

            int intLocationId = Int32.Parse(hdnLocationId.Value);
            DataSet dsLocation = oLocation.GetAddress(intLocationId);
            if (dsLocation.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsLocation.Tables[0].Rows[0];
                int intCityId = Int32.Parse(oLocation.GetAddress(intLocationId, "cityid"));
                int intStateId = Int32.Parse(oLocation.GetCity(intCityId, "stateid")); ;

                txtLocation.Text=oLocation.GetAddress(intLocationId,"name") +" (" +
                                         oLocation.GetCity(intCityId,"name") +"," +
                                         oLocation.GetState(intStateId,"name")+" )";
                                         
            }
            else
                txtLocation.Text = "";

        }

        protected void btnSelectLocation_Click(object sender, EventArgs e)
        {
            hdnLocationId.Value = "0";
            LoadLocation();
            LoopRepeater();
        }
    }
}
