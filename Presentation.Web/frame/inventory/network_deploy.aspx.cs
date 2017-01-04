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
    public partial class network_deploy : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected Asset oAsset;
        protected ModelsProperties oModelsProperties;
        protected int intProfile;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oAsset = new Asset(0, dsnAsset);
            oModelsProperties = new ModelsProperties(0, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["save"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">window.parent.navigate(window.parent.location);<" + "/" + "script>");
            if (!IsPostBack)
            {
                DataSet ds = oAsset.Get(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblTracking.Text = ds.Tables[0].Rows[0]["tracking"].ToString();
                    lblSerial.Text = ds.Tables[0].Rows[0]["serial"].ToString();
                    lblModel.Text = ds.Tables[0].Rows[0]["modelname"].ToString();
                    lblAsset.Text = ds.Tables[0].Rows[0]["asset"].ToString();
                }
                LoadLists();
            }
            ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',0);");
            ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
            btnSubmit.Attributes.Add("onclick", "return ValidateDropDown('" + ddlLocation.ClientID + "','Please select a location')" +
                " && ValidateDropDown('" + ddlRoom.ClientID + "','Please select a room')" +
                " && ValidateDropDown('" + ddlShelf.ClientID + "','Please select a rack')" +
                ";");
            btnClose.Attributes.Add("onclick", "return parent.HidePanel();");
        }
        public void LoadLists()
        {
            Depot oDepot = new Depot(intProfile, dsn);
            ddlLocation.DataValueField = "id";
            ddlLocation.DataTextField = "name";
            ddlLocation.DataSource = oDepot.Gets(1);
            ddlLocation.DataBind();
            DepotRoom oDepotRoom = new DepotRoom(intProfile, dsn);
            ddlRoom.DataValueField = "id";
            ddlRoom.DataTextField = "name";
            ddlRoom.DataSource = oDepotRoom.Gets(1);
            ddlRoom.DataBind();
            Shelf oShelf = new Shelf(intProfile, dsn);
            ddlShelf.DataValueField = "id";
            ddlShelf.DataTextField = "name";
            ddlShelf.DataSource = oShelf.Gets(1);
            ddlShelf.DataBind();
            Classes oClasses = new Classes(intProfile, dsn);
            ddlClass.DataValueField = "id";
            ddlClass.DataTextField = "name";
            ddlClass.DataSource = oClasses.Gets(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            Locations oLocation = new Locations(intProfile, dsn);
            ddlLocation.DataValueField = "id";
            ddlLocation.DataTextField = "fullname";
            if (Request.QueryString["address"] != null)
                ddlLocation.DataSource = oLocation.GetAddresss(1);
            else
                ddlLocation.DataSource = oLocation.GetAddressCommon();
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            Rooms oRooms = new Rooms(intProfile, dsn);
            ddlRoom.DataValueField = "id";
            ddlRoom.DataTextField = "name";
            ddlRoom.DataSource = oRooms.Gets(1);
            ddlRoom.DataBind();
            ddlRoom.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            Racks oRacks = new Racks(intProfile, dsn);
            ddlRack.DataValueField = "id";
            ddlRack.DataTextField = "name";
            ddlRack.DataSource = oRacks.Gets(1);
            ddlRack.DataBind();
            ddlRack.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void chkLocation_Change(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + (chkLocation.Checked ? "&address=true" : ""));
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            oAsset.AddNetwork(intID, txtName.Text, Int32.Parse(ddlStatus.SelectedItem.Value), intProfile, DateTime.Now, Int32.Parse(ddlDepot.SelectedItem.Value), Int32.Parse(ddlDepotRoom.SelectedItem.Value), Int32.Parse(ddlShelf.SelectedItem.Value), Int32.Parse(txtPorts.Text), Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), Int32.Parse(ddlLocation.SelectedItem.Value), Int32.Parse(ddlRoom.SelectedItem.Value), Int32.Parse(ddlRack.SelectedItem.Value), txtRackPosition.Text);
            Response.Redirect(Request.Path + "?save=true");
        }
    }
}
