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
    public partial class facilities_supply : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Locations oLocation;
        protected RoomsNew oRoomsNew;
        protected Zones oZone;
        protected RacksNew oRacksNew;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oRoomsNew = new RoomsNew(intProfile, dsn);
            oZone = new Zones(intProfile, dsn);
            oRacksNew = new RacksNew(intProfile, dsn);

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(Request.QueryString["ra"]) == false)
                {
                    DataSet rec = oRacksNew.Gets(Int32.Parse(Request.QueryString["ra"]));
                    if (rec.Tables[0].Rows.Count > 0)
                    {
                        lblRackZone.ToolTip = rec.Tables[0].Rows[0]["zoneid"].ToString();
                        txtRackName.Text = rec.Tables[0].Rows[0]["rack"].ToString();
                        txtRackU.Text = "50";
                        txtRackAmp.Text = "100";
                        txtRackDescription.Text = rec.Tables[0].Rows[0]["description"].ToString();
                        chkRackAvailable.Checked = (rec.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnRackUpdate.Visible = true;
                        btnRackUpdate.Attributes.Add("onclick", "return ValidateText('" + txtRackName.ClientID + "','Please enter a name')" +
                            " && ProcessButton(this) && LoadWait()" +
                            ";");
                        btnRackDelete.Visible = true;
                        btnRackDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item') && ProcessButton(this) && LoadWait();");
                    }
                    else if (String.IsNullOrEmpty(Request.QueryString["z"]) == false)
                    {
                        lblRackZone.ToolTip = Request.QueryString["z"];
                        btnRackAdd.Visible = true;
                        btnRackAdd.Attributes.Add("onclick", "return ValidateText('" + txtRackName.ClientID + "','Please enter a name')" +
                            " && ProcessButton(this) && LoadWait()" +
                            ";");
                    }
                    lblRackZone.Text = oZone.Get(Int32.Parse(lblRackZone.ToolTip), "zone");
                    btnRackCancel.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                    panRack.Visible = true;
                }
                else if (String.IsNullOrEmpty(Request.QueryString["z"]) == false)
                {
                    DataSet rec = oZone.Gets(Int32.Parse(Request.QueryString["z"]));
                    if (rec.Tables[0].Rows.Count > 0)
                    {
                        lblZoneRoom.ToolTip = rec.Tables[0].Rows[0]["roomid"].ToString();
                        txtZoneName.Text = rec.Tables[0].Rows[0]["zone"].ToString();
                        txtZoneDescription.Text = rec.Tables[0].Rows[0]["description"].ToString();
                        chkZoneAvailable.Checked = (rec.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnZoneUpdate.Visible = true;
                        btnZoneUpdate.Attributes.Add("onclick", "return ValidateText('" + txtZoneName.ClientID + "','Please enter a name')" +
                            " && ProcessButton(this) && LoadWait()" +
                            ";");
                        btnZoneDelete.Visible = true;
                        btnZoneDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item') && ProcessButton(this) && LoadWait();");

                        rptRacks.DataSource = oLocation.GetInventory(null, null, Int32.Parse(Request.QueryString["z"]));
                        rptRacks.DataBind();
                        lblRacks.Visible = (rptRacks.Items.Count == 0);
                    }
                    else if (String.IsNullOrEmpty(Request.QueryString["ro"]) == false)
                    {
                        lblZoneRoom.ToolTip = Request.QueryString["ro"];
                        btnZoneAdd.Visible = true;
                        btnZoneAdd.Attributes.Add("onclick", "return ValidateText('" + txtZoneName.ClientID + "','Please enter a name')" +
                            " && ProcessButton(this) && LoadWait()" +
                            ";");
                        btnAddRack.Enabled = false;
                    }
                    lblZoneRoom.Text = oRoomsNew.Get(Int32.Parse(lblZoneRoom.ToolTip), "room");
                    btnZoneCancel.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                    panZone.Visible = true;
                }
                else if (String.IsNullOrEmpty(Request.QueryString["ro"]) == false)
                {
                    DataSet rec = oRoomsNew.Gets(Int32.Parse(Request.QueryString["ro"]));
                    if (rec.Tables[0].Rows.Count > 0)
                    {
                        lblRoomLocation.ToolTip = rec.Tables[0].Rows[0]["locationid"].ToString();
                        txtRoomName.Text = rec.Tables[0].Rows[0]["room"].ToString();
                        txtRoomDescription.Text = rec.Tables[0].Rows[0]["description"].ToString();
                        chkRoomAvailable.Checked = (rec.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnRoomUpdate.Visible = true;
                        btnRoomUpdate.Attributes.Add("onclick", "return ValidateText('" + txtRoomName.ClientID + "','Please enter a name')" +
                            " && ProcessButton(this) && LoadWait()" +
                            ";");
                        btnRoomDelete.Visible = true;
                        btnRoomDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item') && ProcessButton(this) && LoadWait();");

                        rptZones.DataSource = oLocation.GetInventory(null, Int32.Parse(Request.QueryString["ro"]), null);
                        rptZones.DataBind();
                        lblZones.Visible = (rptZones.Items.Count == 0);
                    }
                    else if (String.IsNullOrEmpty(Request.QueryString["l"]) == false)
                    {
                        lblRoomLocation.ToolTip = Request.QueryString["l"];
                        btnRoomAdd.Visible = true;
                        btnRoomAdd.Attributes.Add("onclick", "return ValidateText('" + txtRoomName.ClientID + "','Please enter a name')" +
                            " && ProcessButton(this) && LoadWait()" +
                            ";");
                        btnAddZone.Enabled = false;
                    }
                    lblRoomLocation.Text = oLocation.GetAddress(Int32.Parse(lblRoomLocation.ToolTip), "commonname");
                    btnRoomCancel.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                    panRoom.Visible = true;
                }
                else if (String.IsNullOrEmpty(Request.QueryString["l"]) == false)
                {
                    DataSet rec = oLocation.GetAddress(Int32.Parse(Request.QueryString["l"]));
                    if (rec.Tables[0].Rows.Count > 0)
                    {
                        txtLocationName.Text = rec.Tables[0].Rows[0]["commonname"].ToString();
                        txtLocationAddress.Text = rec.Tables[0].Rows[0]["name"].ToString();
                        txtLocationCode.Text = rec.Tables[0].Rows[0]["factory_code"].ToString();
                        chkLocationStorage.Checked = (rec.Tables[0].Rows[0]["storage"].ToString() == "1");
                        chkLocationTSM.Checked = (rec.Tables[0].Rows[0]["tsm"].ToString() == "1");
                        chkLocationProd.Checked = (rec.Tables[0].Rows[0]["prod"].ToString() == "1");
                        chkLocationQA.Checked = (rec.Tables[0].Rows[0]["qa"].ToString() == "1");
                        chkLocationTest.Checked = (rec.Tables[0].Rows[0]["test"].ToString() == "1");
                        chkLocationDR.Checked = (rec.Tables[0].Rows[0]["dr"].ToString() == "1");
                        txtLocationBuildingCode.Text = rec.Tables[0].Rows[0]["building_code"].ToString();
                        chkLocationAssignIP.Checked = (rec.Tables[0].Rows[0]["vmware_ipaddress"].ToString() == "1");
                        chkLocationAvailable.Checked = (rec.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        lblLocationCityID.Text = rec.Tables[0].Rows[0]["cityid"].ToString();

                        btnLocationUpdate.Visible = true;
                        btnLocationUpdate.Attributes.Add("onclick", "return ValidateText('" + txtRoomName.ClientID + "','Please enter a name')" +
                            " && ProcessButton(this) && LoadWait()" +
                            ";");
                        btnLocationDelete.Visible = true;
                        btnLocationDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item') && ProcessButton(this) && LoadWait();");

                        rptRooms.DataSource = oLocation.GetInventory(Int32.Parse(Request.QueryString["l"]), null, null);
                        rptRooms.DataBind();
                        lblRooms.Visible = (rptRooms.Items.Count == 0);
                    }
                    else 
                    {
                        btnLocationAdd.Visible = true;
                        btnLocationAdd.Attributes.Add("onclick", "return ValidateText('" + txtLocationName.ClientID + "','Please enter a name')" +
                            " && ProcessButton(this) && LoadWait()" +
                            ";");
                        btnAddRoom.Enabled = false;
                    }
                    btnLocationCancel.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                    panLocation.Visible = true;
                }
                else
                {
                    panLocations.Visible = true;
                    rptLocations.DataSource = oLocation.GetInventory(null, null, null);
                    rptLocations.DataBind();
                }
            }
        }
        protected string FormURL(string _additional_querystring)
        {
            string strRedirect = "";
            strRedirect += BuildURL("id", strRedirect);
            strRedirect += BuildURL("tid", strRedirect);
            if (_additional_querystring != "")
            {
                if (strRedirect == "")
                    _additional_querystring = "?" + _additional_querystring;
                else
                    _additional_querystring = "&" + _additional_querystring;
            }
            return oPage.GetFullLink(intPage) + strRedirect + _additional_querystring;
        }
        protected string BuildURL(string _value, string _url)
        {
            string strReturn = "";
            if (Request.QueryString[_value] != null)
            {
                if (_url == "")
                    strReturn = "?" + _value + "=" + Request.QueryString[_value];
                else
                    strReturn = "&" + _value + "=" + Request.QueryString[_value];
            }
            return strReturn;
        }

        protected void btnAddLocation_Click(object sender, EventArgs e)
        {
            Response.Redirect(FormURL("l=0"));
        }

        protected void btnLocationAdd_Click(object sender, EventArgs e)
        {
            oLocation.AddAddress(0, txtLocationAddress.Text, txtLocationCode.Text, 1, txtLocationName.Text, chkLocationStorage.Checked ? 1 : 0, chkLocationTSM.Checked ? 1 : 0, chkLocationDR.Checked ? 1 : 0, 0, 0, txtLocationCode.Text, "", 0, chkLocationAssignIP.Checked ? 1 : 0, chkLocationProd.Checked ? 1 : 0, chkLocationQA.Checked ? 1 : 0, chkLocationTest.Checked ? 1 : 0, chkLocationAvailable.Checked ? 1 : 0);
            Response.Redirect(FormURL(""));
        }

        protected void btnLocationCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(FormURL(""));
        }

        protected void btnLocationDelete_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(Request.QueryString["l"]);
            oLocation.DeleteAddress(id);
            Response.Redirect(FormURL(""));
        }

        protected void btnLocationUpdate_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(Request.QueryString["l"]);
            oLocation.UpdateAddress(id, Int32.Parse(lblLocationCityID.Text), txtLocationAddress.Text, txtLocationCode.Text, 1, txtLocationName.Text, chkLocationStorage.Checked ? 1 : 0, chkLocationTSM.Checked ? 1 : 0, chkLocationDR.Checked ? 1 : 0, 0, 0, txtLocationCode.Text, "", 0, chkLocationAssignIP.Checked ? 1 : 0, chkLocationProd.Checked ? 1 : 0, chkLocationQA.Checked ? 1 : 0, chkLocationTest.Checked ? 1 : 0, chkLocationAvailable.Checked ? 1 : 0);
            Response.Redirect(FormURL("l=" + id.ToString()));
        }

        protected void btnAddRoom_Click(object sender, EventArgs e)
        {
            Response.Redirect(FormURL("l=" + Request.QueryString["l"] + "&ro=0"));
        }

        protected void btnRoomCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(FormURL("l=" + Request.QueryString["l"]));
        }

        protected void btnRoomDelete_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(Request.QueryString["ro"]);
            oRoomsNew.Delete(id, 1);
            Response.Redirect(FormURL("l=" + lblRoomLocation.ToolTip));
        }

        protected void btnRoomUpdate_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(Request.QueryString["ro"]);
            oRoomsNew.Update(id, txtRoomName.Text, Int32.Parse(lblRoomLocation.ToolTip), txtRoomDescription.Text, chkRoomAvailable.Checked ? 1 : 0);
            Response.Redirect(FormURL("l=" + lblRoomLocation.ToolTip + "&ro=" + id.ToString()));
        }

        protected void btnRoomAdd_Click(object sender, EventArgs e)
        {
            int id = oRoomsNew.Add(txtRoomName.Text, Int32.Parse(lblRoomLocation.ToolTip), txtRoomDescription.Text, chkRoomAvailable.Checked ? 1 : 0);
            Response.Redirect(FormURL("l=" + lblRoomLocation.ToolTip));
        }

        protected void btnAddZone_Click(object sender, EventArgs e)
        {
            Response.Redirect(FormURL("ro=" + Request.QueryString["ro"] + "&z=0"));
        }

        protected void btnZoneAdd_Click(object sender, EventArgs e)
        {
            int id = oZone.Add(txtZoneName.Text, Int32.Parse(lblZoneRoom.ToolTip), 0, txtZoneDescription.Text, chkZoneAvailable.Checked ? 1 : 0);
            Response.Redirect(FormURL("ro=" + lblZoneRoom.ToolTip));
        }

        protected void btnZoneUpdate_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(Request.QueryString["z"]);
            oZone.Update(id, txtZoneName.Text, Int32.Parse(lblZoneRoom.ToolTip), 0, txtZoneDescription.Text, chkZoneAvailable.Checked ? 1 : 0);
            Response.Redirect(FormURL("ro=" + lblZoneRoom.ToolTip + "&z=" + id.ToString()));
        }

        protected void btnZoneDelete_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(Request.QueryString["z"]);
            oZone.Delete(id, 1);
            Response.Redirect(FormURL("ro=" + lblZoneRoom.ToolTip));
        }

        protected void btnZoneCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(FormURL("ro=" + Request.QueryString["ro"]));
        }

        protected void btnAddRack_Click(object sender, EventArgs e)
        {
            Response.Redirect(FormURL("z=" + Request.QueryString["z"] + "&ra=0"));
        }

        protected void btnRackAdd_Click(object sender, EventArgs e)
        {
            int id = oRacksNew.Add(txtRackName.Text, 0, Int32.Parse(lblRackZone.ToolTip), txtRackDescription.Text, chkRackAvailable.Checked ? 1 : 0);
            Response.Redirect(FormURL("z=" + lblRackZone.ToolTip));
        }

        protected void btnRackUpdate_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(Request.QueryString["ra"]);
            oRacksNew.Update(id, txtRackName.Text, 0, Int32.Parse(lblRackZone.ToolTip), txtRackDescription.Text, chkRackAvailable.Checked ? 1 : 0);
            Response.Redirect(FormURL("z=" + lblRackZone.ToolTip + "&ra=" + id.ToString()));
        }

        protected void btnRackDelete_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(Request.QueryString["ra"]);
            oRacksNew.Delete(id, 1);
            Response.Redirect(FormURL("z=" + lblRackZone.ToolTip));
        }

        protected void btnRackCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(FormURL("z=" + Request.QueryString["z"]));
        }
    }
}