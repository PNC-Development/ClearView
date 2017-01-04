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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class frame_location_room_rack : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intProfile = 0;
        protected Locations oLocation;
        protected RoomsNew oRoom;
        protected RacksNew oRack;
        protected Zones oZone;
        protected string strSelectionType = "";
        protected int intSelectionId = 0;

       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            
            oLocation = new Locations(intProfile, dsn);
            oRoom = new RoomsNew(intProfile, dsn);
            oRack = new RacksNew(intProfile, dsn);
            oZone = new Zones(intProfile, dsn);

            if (!Page.IsPostBack)
            {

                if (Request.QueryString["type"] != null && Request.QueryString["type"] != "")
                    strSelectionType = Request.QueryString["type"].ToString();

                strSelectionType = strSelectionType.ToLower();

                //Get controls to populate on parent page
                if (Request.QueryString["Id"] != null && Request.QueryString["Id"] != "")
                    intSelectionId = Int32.Parse(Request.QueryString["Id"]);
                else
                    chkCommonLocation.Checked = true;

                if (Request.QueryString["hdnId"] != null && Request.QueryString["hdnId"] != "")
                    lblParentCtrlHdnId.Text = Request.QueryString["hdnId"];

                if (Request.QueryString["ctrlLocation"] != null && Request.QueryString["ctrlLocation"] != "")
                    lblParentCtrlLocation.Text = Request.QueryString["ctrlLocation"];

                if (Request.QueryString["ctrlRoom"] != null && Request.QueryString["ctrlRoom"] != "")
                    lblParentCtrlRoom.Text = Request.QueryString["ctrlRoom"];


                if (Request.QueryString["ctrlZone"] != null && Request.QueryString["ctrlZone"] != "")
                    lblParentCtrlZone.Text = Request.QueryString["ctrlZone"];

                if (Request.QueryString["ctrlRack"] != null && Request.QueryString["ctrlRack"] != "")
                    lblParentCtrlRack.Text = Request.QueryString["ctrlRack"];

               
                 SetControls();
                
                 if (intSelectionId > 0)
                 {
                     DataSet dsLocation = null;
                     if (strSelectionType=="location")
                     {
                         dsLocation = oLocation.GetAddress(intSelectionId);
                         if (dsLocation.Tables[0].Rows.Count > 0)
                         {
                             DataRow dr = dsLocation.Tables[0].Rows[0];
                             int intLocationId = Int32.Parse(dr["id"].ToString());
                             int intCityId = Int32.Parse(oLocation.GetAddress(intLocationId, "cityid"));
                             int intStateId = Int32.Parse(oLocation.GetCity(intCityId, "stateid")); ;
                             LoadState();
                             ddlState.SelectedValue = intStateId.ToString();
                             LoadCity();
                             ddlCity.SelectedValue = intCityId.ToString();
                             LoadAddress();
                             ddlLocation.SelectedValue = intLocationId.ToString();

                         }
                     }
                     else if (strSelectionType == "room")
                     {
                         dsLocation = oRoom.Gets(intSelectionId);
                         if (dsLocation.Tables[0].Rows.Count > 0)
                         {
                             DataRow dr = dsLocation.Tables[0].Rows[0];
                             LoadState();
                             ddlState.SelectedValue = dr["StateId"].ToString();
                             LoadCity();
                             ddlCity.SelectedValue = dr["CityId"].ToString();
                             LoadAddress();
                             ddlLocation.SelectedValue = dr["LocationId"].ToString();
                             LoadRoom();
                             ddlRoom.SelectedValue = dr["RoomId"].ToString();
                         }
                     }
                     else if (strSelectionType == "zone")
                     {
                         dsLocation = oZone.Gets(intSelectionId);
                         if (dsLocation.Tables[0].Rows.Count > 0)
                         {
                             DataRow dr = dsLocation.Tables[0].Rows[0];
                             LoadState();
                             ddlState.SelectedValue = dr["StateId"].ToString();
                             LoadCity();
                             ddlCity.SelectedValue = dr["CityId"].ToString();
                             LoadAddress();
                             ddlLocation.SelectedValue = dr["LocationId"].ToString();
                             LoadRoom();
                             ddlRoom.SelectedValue = dr["RoomId"].ToString();
                             LoadZone();
                             ddlZone.SelectedValue = dr["ZoneId"].ToString();
                         }
                     }
                     else if (strSelectionType == "rack")
                     {
                         dsLocation = oRack.Gets(intSelectionId);

                         if (dsLocation.Tables[0].Rows.Count > 0)
                         {
                             DataRow dr = dsLocation.Tables[0].Rows[0];
                             LoadState();
                             ddlState.SelectedValue = dr["StateId"].ToString();
                             LoadCity();
                             ddlCity.SelectedValue = dr["CityId"].ToString();
                             LoadAddress();
                             ddlLocation.SelectedValue = dr["LocationId"].ToString();
                             LoadRoom();
                             ddlRoom.SelectedValue = dr["RoomId"].ToString();
                             LoadZone();
                             ddlZone.SelectedValue = dr["ZoneId"].ToString();
                             LoadRack();
                             ddlRack.SelectedValue = dr["RackId"].ToString();

                         }

                     }
                }

            }

          
        }


        private void SetControls()
        {

            trState.Visible = false;
            trCity.Visible = false;
            trLocation.Visible = false;
            trRoom.Visible = false;
            trZone.Visible = false;
            trRack.Visible = false;
            

            if (strSelectionType == "location")
            {
                trState.Visible = true;
                trCity.Visible = true;
                trLocation.Visible = true;
            }
            else if (strSelectionType == "room")
            {
                trState.Visible = true;
                trCity.Visible = true;
                trLocation.Visible = true;
                trRoom.Visible = true;
            }
            else if (strSelectionType == "zone")
            {
                trState.Visible = true;
                trCity.Visible = true;
                trLocation.Visible = true;
                trRoom.Visible = true;
                trZone.Visible = true;
            }
            else if (strSelectionType == "rack")
            {
                trState.Visible = true;
                trCity.Visible = true;
                trLocation.Visible = true;
                trRoom.Visible = true;
                trZone.Visible = true;
                trRack.Visible = true;
                
                
            }

            //Validations
            if (strSelectionType == "location")
            {
                btnSave.Attributes.Add("onclick", "return ValidateDropDown('" + ddlState.ClientID + "','Please select a state')" +
                       " && ValidateDropDown('" + ddlCity.ClientID + "','Please select a city')" +
                       " && ValidateDropDown('" + ddlLocation.ClientID + "','Please select a location')" +
                       ";");
            }
            else if (strSelectionType == "room")
            {
                btnSave.Attributes.Add("onclick", "return ValidateDropDown('" + ddlState.ClientID + "','Please select a state')" +
                       " && ValidateDropDown('" + ddlCity.ClientID + "','Please select a city')" +
                       " && ValidateDropDown('" + ddlLocation.ClientID + "','Please select a location')" +
                       " && ValidateDropDown('" + ddlRoom.ClientID + "','Please select a room')" +
                       ";");
            }
            else if (strSelectionType == "zone")
            {
                btnSave.Attributes.Add("onclick", "return ValidateDropDown('" + ddlState.ClientID + "','Please select a state')" +
                       " && ValidateDropDown('" + ddlCity.ClientID + "','Please select a city')" +
                       " && ValidateDropDown('" + ddlLocation.ClientID + "','Please select a location')" +
                       " && ValidateDropDown('" + ddlRoom.ClientID + "','Please select a room')" +
                       " && ValidateDropDown('" + ddlZone.ClientID + "','Please select a zone')" +
                       ";");
            }
            else if (strSelectionType == "rack")
            {
                btnSave.Attributes.Add("onclick", "return ValidateDropDown('" + ddlState.ClientID + "','Please select a state')" +
                     " && ValidateDropDown('" + ddlCity.ClientID + "','Please select a city')" +
                     " && ValidateDropDown('" + ddlLocation.ClientID + "','Please select a location')" +
                     " && ValidateDropDown('" + ddlRoom.ClientID + "','Please select a room')" +
                     " && ValidateDropDown('" + ddlZone.ClientID + "','Please select a zone')" +
                     " && ValidateDropDown('" + ddlRack.ClientID + "','Please select a rack')" +
                     ";");
            }

            if (chkCommonLocation.Checked == true)
            {
                trState.Style.Add("display", "none");
                trCity.Style.Add("display", "none");
            }
            else
            {
                trState.Style.Add("display", "inline");
                trCity.Style.Add("display", "inline");
            }
            LoadState();
            LoadCity();
            LoadAddress();
            LoadRoom();
            LoadZone();
            LoadRack();
        }
    

        protected void LoadState()
        {
            ddlState.Items.Clear();
            ddlState.ClearSelection();
            if (chkCommonLocation.Checked == true && ddlLocation.SelectedIndex>-1 )
            //if (chkCommonLocation.Checked == true && (ddlLocation.SelectedIndex>-1 Int32.Parse(ddlLocation.SelectedValue.ToString()) > 0)
            {
                int intLocation = Int32.Parse(ddlLocation.SelectedValue.ToString());
                int intCity = Int32.Parse(oLocation.GetAddress(intLocation, "cityid"));
                int intState = Int32.Parse(oLocation.GetCity(intCity, "stateid"));

                ddlState.DataTextField = "name";
                ddlState.DataValueField = "id";
                ddlState.DataSource = oLocation.GetState(intState);
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlState.Items.FindByValue(intState.ToString()).Selected = true; 

            }
            else
            {
                ddlState.Items.Clear();
                ddlState.DataTextField = "name";
                ddlState.DataValueField = "id";
                ddlState.DataSource = oLocation.GetStates(1);
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlState.Items.FindByValue("0").Selected = true; 
            }
        }

        protected void LoadCity()
        {
            ddlCity.ClearSelection();
            ddlCity.Items.Clear();
            if (chkCommonLocation.Checked == true && ddlLocation.SelectedIndex > -1)
            //if (chkCommonLocation.Checked == true && Int32.Parse(ddlLocation.SelectedValue.ToString()) > 0)
            {
                int intLocation = Int32.Parse(ddlLocation.SelectedValue.ToString());
                int intCity = Int32.Parse(oLocation.GetAddress(intLocation, "cityid"));
                int intState = Int32.Parse(oLocation.GetCity(intCity, "stateid"));

                ddlCity.Items.Clear();
                ddlCity.DataTextField = "name";
                ddlCity.DataValueField = "id";
                ddlCity.DataSource = oLocation.GetCity(intCity); ;
                ddlCity.DataBind();
                ddlCity.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlCity.Enabled = true;
                ddlCity.Items.FindByValue(intCity.ToString()).Selected = true; 
            }
            else
            {

                if (Int32.Parse(ddlState.SelectedValue.ToString()) > 0)
                {
                    ddlCity.DataTextField = "name";
                    ddlCity.DataValueField = "id";
                    ddlCity.DataSource = oLocation.GetCitys(Int32.Parse(ddlState.SelectedValue.ToString()), 1);
                    ddlCity.DataBind();
                    ddlCity.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                    ddlCity.Enabled = true;
                }
                else
                {
                    ddlCity.Enabled = false;
                    ddlCity.Items.Insert(0, new ListItem("-- SELECT STATE --", "0"));
                }
                ddlCity.Items.FindByValue("0").Selected = true; 
            }
        }

        protected void LoadAddress()
        {
            ddlLocation.Items.Clear();
            ddlLocation.ClearSelection();
            if (chkCommonLocation.Checked == true)
            {
                ddlLocation.DataTextField = "Commonname";
                ddlLocation.DataValueField = "id";
                ddlLocation.DataSource = oLocation.GetAddressCommon();
                ddlLocation.DataBind();
                ddlLocation.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlLocation.Enabled = true;
            }
            else
            {
                if (Int32.Parse(ddlCity.SelectedValue.ToString()) > 0)
                {
                    ddlLocation.DataTextField = "name";
                    ddlLocation.DataValueField = "id";
                    ddlLocation.DataSource = oLocation.GetAddresss(Int32.Parse(ddlCity.SelectedValue.ToString()), 1);
                    ddlLocation.DataBind();
                    ddlLocation.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                    ddlLocation.Enabled = true;
                }
                else
                {
                    ddlLocation.Enabled = false;
                    ddlLocation.Items.Insert(0, new ListItem("-- SELECT CITY --", "0"));
                }
            }
            ddlLocation.Items.FindByValue("0").Selected = true; 

        }

        protected void LoadRoom()
        {
            ddlRoom.Items.Clear();
            ddlRoom.ClearSelection();
            if (Int32.Parse(ddlLocation.SelectedValue.ToString()) > 0)
            {
                ddlRoom.DataTextField = "Room";
                ddlRoom.DataValueField = "RoomId";
                ddlRoom.DataSource = oRoom.Gets(Int32.Parse(ddlLocation.SelectedValue.ToString()), 1);
                ddlRoom.DataBind();
                ddlRoom.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlRoom.Enabled = true;
            }
            else
            {
                ddlRoom.Enabled = false;
                ddlRoom.Items.Insert(0, new ListItem("-- SELECT ADDRESS --", "0"));
            }

            ddlRoom.Items.FindByValue("0").Selected=true; 

        }

        protected void LoadZone()
        {
            ddlZone.Items.Clear();
            ddlZone.ClearSelection();
            if (Int32.Parse(ddlRoom.SelectedValue.ToString()) > 0)
            {
                ddlZone.DataTextField = "Zone";
                ddlZone.DataValueField = "ZoneId";
                ddlZone.DataSource = oZone.Gets(Int32.Parse(ddlRoom.SelectedValue.ToString()), 1);
                ddlZone.DataBind();
                ddlZone.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlZone.Enabled = true;
            }
            else
            {
                ddlZone.Enabled = false;
                ddlZone.Items.Insert(0, new ListItem("-- SELECT ADDRESS --", "0"));
            }

            ddlZone.Items.FindByValue("0").Selected = true;

        }

        protected void LoadRack()
        {
            ddlRack.Items.Clear();
            ddlRack.ClearSelection();
            if (Int32.Parse(ddlZone.SelectedValue.ToString()) > 0)
            {
                ddlRack.DataTextField = "Rack";
                ddlRack.DataValueField = "RackId";
                ddlRack.DataSource = oRack.Gets(Int32.Parse(ddlZone.SelectedValue.ToString()), 1);
                ddlRack.DataBind();
                ddlRack.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlRack.Enabled = true;
            }
            else
            {
                ddlRack.Enabled = false;
                ddlRack.Items.Insert(0, new ListItem("-- SELECT ROOM --", "0"));
            }

            ddlRack.Items.FindByValue("0").Selected = true; 

        }
        

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(typeof(Page), "updatelocationroomrack", "<script type=\"text/javascript\">UpdateLocationRoomRack();<" + "/" + "script>");

        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkCommonLocation.Checked==false)
            {
                LoadCity();
                LoadAddress();
                LoadRoom();
                LoadRack();
                if (trCity.Visible == true) ddlCity.Focus();
            }


        }

        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkCommonLocation.Checked == false)
            {
                LoadAddress();
                LoadRoom();
                LoadZone();
                LoadRack();
                if (trLocation.Visible == true) ddlLocation.Focus();
            }

        }

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkCommonLocation.Checked == true)
            {
                LoadState();
                LoadCity();
            }
                LoadRoom();
                LoadRack();
                if (trRoom.Visible == true) ddlRoom.Focus();   
        }

        protected void ddlRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadZone();
            if (trZone.Visible == true) ddlZone.Focus();
        }

        protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRack();
            if (trRack.Visible == true) ddlRack.Focus();
        }

        

        protected void chkCommonLocation_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCommonLocation.Checked == true)
            {
                trState.Style.Add("display", "none");
                trCity.Style.Add("display", "none");

                LoadAddress();
                LoadRoom();
                LoadZone();
                LoadRack();
            }
            else
            {

                trState.Style.Add("display", "inline");
                trCity.Style.Add("display", "inline");

                LoadState();
                LoadCity();
                LoadAddress();
                LoadRoom();
                LoadZone();
                LoadRack();
            }
            //ddlState.SelectedValue = "0";
            //ddlCity.SelectedValue = "0";
            //ddlLocation.SelectedValue = "0";
        }
    }
}
