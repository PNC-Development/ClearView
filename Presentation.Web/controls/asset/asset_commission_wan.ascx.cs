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
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Presentation.Web
{
    public partial class asset_commission_wan : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected Pages oPage;
        protected Platforms oPlatform;
        protected Types oType;
        protected Locations oLocation;
        protected Asset oAsset;
        protected Racks oRacks;
        protected Classes oClasses;
        protected Rooms oRooms;
        protected Floor oFloor;
        protected Environments oEnvironment;
        protected Depot oDepot;
        protected DepotRoom oDepotRoom;
        protected Shelf oShelf;
        protected Users oUser;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strLocation = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oRacks = new Racks(intProfile, dsn);
            oClasses = new Classes(intProfile, dsn);
            oRooms = new Rooms(intProfile, dsn);
            oFloor = new Floor(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oDepot = new Depot(intProfile, dsn);
            oDepotRoom = new DepotRoom(intProfile, dsn);
            oShelf = new Shelf(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    LoadLists();
                    Load(Int32.Parse(Request.QueryString["id"]));
                    txtDate.Text = DateTime.Today.ToShortDateString();
                    imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
                    btnSubmit.Attributes.Add("onclick", "return ValidateText('" + txtAsset.ClientID + "','Please enter the asset tag')" +
                        " && ValidateDropDown('" + ddlDepot.ClientID + "','Please make a selection for the location of the asset')" +
                        " && ValidateText('" + txtDepotRoom.ClientID + "','Please enter the depot room')" +
                        " && ValidateText('" + txtShelf.ClientID + "','Please enter the shelf')" +
                        " && ValidateNumber0('" + txtPorts.ClientID + "','Please enter a valid number of available ports')" +
                        " && ValidateHidden0('" + hdnLocation.ClientID + "','ddlState','Please select a location')" +
                        " && ValidateText('" + txtRoom.ClientID + "','Please enter the room')" +
                        " && ValidateText('" + txtRack.ClientID + "','Please enter the rack')" +
                        " && ValidateText('" + txtRackPosition.ClientID + "','Please enter the rack position')" +
                        " && ValidateDropDown('" + ddlEnvironment.ClientID + "','Please make a selection for the environment')" +
                        " && ValidateDropDown('" + ddlClass.ClientID + "','Please make a selection for the current class')" +
                        " && ValidateText('" + txtName.ClientID + "','Please enter the device name')" +
                        " && ValidateNumber0('" + txtIP1.ClientID + "','Please enter a valid IP address')" +
                        " && ValidateNumber0('" + txtIP2.ClientID + "','Please enter a valid IP address')" +
                        " && ValidateNumber0('" + txtIP3.ClientID + "','Please enter a valid IP address')" +
                        " && ValidateNumber0('" + txtIP4.ClientID + "','Please enter a valid IP address')" +
                        " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid date')" +
                        ";");
                    ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',0);");
                    ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
                }
            }
        }
        private void LoadLists()
        {
            ddlDepot.DataValueField = "id";
            ddlDepot.DataTextField = "name";
            ddlDepot.DataSource = oDepot.Gets(1);
            ddlDepot.DataBind();
            ddlDepot.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlClass.DataValueField = "id";
            ddlClass.DataTextField = "name";
            ddlClass.DataSource = oClasses.Gets(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void Load(int _id)
        {
            DataSet ds = oAsset.GetNetwork(_id);
            int intAddress = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblSerial.Text = ds.Tables[0].Rows[0]["serial"].ToString();
                txtAsset.Text = ds.Tables[0].Rows[0]["asset"].ToString();
                lblType.Text = ds.Tables[0].Rows[0]["type"].ToString();
                lblModel.Text = ds.Tables[0].Rows[0]["model"].ToString();
                ddlDepot.SelectedValue = ds.Tables[0].Rows[0]["depotid"].ToString();
                txtDepotRoom.Text = ds.Tables[0].Rows[0]["depotroom"].ToString();
                txtShelf.Text = ds.Tables[0].Rows[0]["shelf"].ToString();
                txtPorts.Text = ds.Tables[0].Rows[0]["ports"].ToString();
                lblReceivedOn.Text = DateTime.Parse(ds.Tables[0].Rows[0]["datestamp"].ToString()).ToLongDateString();
                lblReceivedBy.Text = ds.Tables[0].Rows[0]["statusby"].ToString();
                lblCommissionedOn.Text = DateTime.Parse(ds.Tables[0].Rows[0]["datestamp"].ToString()).ToLongDateString();
                lblCommissionedBy.Text = ds.Tables[0].Rows[0]["statusby"].ToString();
                int intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                ddlClass.SelectedValue = intClass.ToString();
                int intEnv = Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString());
                hdnEnvironment.Value = intEnv.ToString();
                ddlEnvironment.SelectedValue = intEnv.ToString();
                if (intClass > 0)
                {
                    ddlEnvironment.Enabled = true;
                    ddlEnvironment.DataTextField = "name";
                    ddlEnvironment.DataValueField = "id";
                    ddlEnvironment.DataSource = oClasses.GetEnvironment(intClass, 0);
                    ddlEnvironment.DataBind();
                    ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                }
                txtRoom.Text = ds.Tables[0].Rows[0]["room"].ToString();
                txtRack.Text = ds.Tables[0].Rows[0]["rack"].ToString();
                txtRackPosition.Text = ds.Tables[0].Rows[0]["rackposition"].ToString();
                intAddress = Int32.Parse(ds.Tables[0].Rows[0]["addressid"].ToString());
                txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                txtIP1.Text = ds.Tables[0].Rows[0]["add1"].ToString();
                txtIP2.Text = ds.Tables[0].Rows[0]["add2"].ToString();
                txtIP3.Text = ds.Tables[0].Rows[0]["add3"].ToString();
                txtIP4.Text = ds.Tables[0].Rows[0]["add4"].ToString();
                if (Int32.Parse(ds.Tables[0].Rows[0]["status"].ToString()) == (int)AssetStatus.InUse)
                {
                    panCommissioned.Visible = true;
                }
                else
                {
                    panCommission.Visible = true;
                    panReceived.Visible = true;
                    lblCommissionedBy.Text = oUser.GetFullName(intProfile);
                }
            }
            if (intAddress > 0)
                intLocation = intAddress;
            strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, intLocation, true, "ddlCommon");
            hdnLocation.Value = intLocation.ToString();
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            int intAsset = Int32.Parse(Request.QueryString["id"]);
            oAsset.Update(intAsset, txtAsset.Text);
            IPAddresses oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
            int intIP = oIPAddresses.Add(0, Int32.Parse(txtIP1.Text), Int32.Parse(txtIP2.Text), Int32.Parse(txtIP3.Text), Int32.Parse(txtIP4.Text), intProfile);
            int intDepotRoom = GetID(txtDepotRoom.Text, "cv_depot_rooms");
            if (intDepotRoom == 0)
            {
                oDepotRoom.Add(txtDepotRoom.Text, 1);
                intDepotRoom = GetID(txtDepotRoom.Text, "cv_depot_rooms");
            }
            int intShelf = GetID(txtShelf.Text, "cv_shelfs");
            if (intShelf == 0)
            {
                oShelf.Add(txtShelf.Text, 1);
                intShelf = GetID(txtShelf.Text, "cv_shelfs");
            }
            int intRoom = GetID(txtRoom.Text, "cv_rooms");
            if (intRoom == 0)
            {
                oRooms.Add(txtRoom.Text, 1);
                intRoom = GetID(txtRoom.Text, "cv_rooms");
            }
            int intRack = GetID(txtRack.Text, "cv_racks");
            if (intRack == 0)
            {
                oRacks.Add(txtRack.Text, 1);
                intRack = GetID(txtRack.Text, "cv_racks");
            }
            oAsset.UpdateNetwork(intAsset, txtName.Text, (int)AssetStatus.InUse, intProfile, DateTime.Parse(txtDate.Text), Int32.Parse(ddlDepot.SelectedItem.Value), intDepotRoom, intShelf, Int32.Parse(txtPorts.Text), Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), Int32.Parse(Request.Form[hdnLocation.UniqueID]), intRoom, intRack, txtRackPosition.Text);
            oAsset.DeleteIP(intAsset);
            oAsset.AddIP(intAsset, intIP);
            if (Request.QueryString["sid"] != null)
                Response.Redirect(oPage.GetFullLink(intPage) + "?sid=" + Request.QueryString["sid"] + "&id=" + intAsset.ToString() + "&commed=true");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&save=true");
        }
        protected int GetID(string _name, string _table)
        {
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM " + _table + " WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
            if (ds.Tables[0].Rows.Count == 0)
                return 0;
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        }
    }
}