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
    public partial class server_deploy : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Asset oAsset;
        protected IPAddresses oIPAddresses;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Solaris oSolaris;
        protected Resiliency oResiliency;
        protected OperatingSystems oOperatingSystem;
        protected int intProfile;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oAsset = new Asset(0, dsnAsset);
            oIPAddresses = new IPAddresses(0, dsnIP, dsn);
            oModel = new Models(0, dsn);
            oModelsProperties = new ModelsProperties(0, dsn);
            oSolaris = new Solaris(intProfile, dsn);
            oResiliency = new Resiliency(intProfile, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["save"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Asset Deployed Successfully!');window.close();<" + "/" + "script>");
            if (!IsPostBack)
            {
                DataSet ds = oAsset.Get(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblTracking.Text = ds.Tables[0].Rows[0]["tracking"].ToString();
                    lblSerial.Text = ds.Tables[0].Rows[0]["serial"].ToString();
                    int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
                    int intParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                    ddlModels.DataTextField = "name";
                    ddlModels.DataValueField = "id";
                    ddlModels.DataSource = oModelsProperties.GetModels(1, intParent, 1);
                    ddlModels.DataBind();
                    ddlModels.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                    ddlModels.SelectedValue = intModel.ToString();
                    //lblModel.Text = ds.Tables[0].Rows[0]["modelname"].ToString();
                    lblAsset.Text = ds.Tables[0].Rows[0]["asset"].ToString();
                    lblHBA.Text = "&nbsp;(Currently = " + oAsset.GetHBA(intID).Tables[0].Rows.Count.ToString() + ")";

                    if (oModelsProperties.IsTypeBlade(intModel) == true)
                    {
                        panBlade.Visible = true;
                        ddlEnclosure.DataValueField = "id";
                        ddlEnclosure.DataTextField = "name";
                        ddlEnclosure.DataSource = oAsset.GetEnclosures((int)AssetStatus.InUse);
                        ddlEnclosure.DataBind();
                        ddlEnclosure.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        btnSubmit.Attributes.Add("onclick", "return ValidateDropDown('" + ddlStatus.ClientID + "','Please select a status')" +
                            " && ValidateDropDown('" + ddlModels.ClientID + "','Please select a model')" +
                            " && ValidateDropDown('" + ddlEnclosure.ClientID + "','Please select an enclosure')" +
                            " && ValidateNumber0('" + txtSlot.ClientID + "','Please enter a valid slot number')" +
                            " && ValidateRadioButtons('" + radSpareYes.ClientID + "','" + radSpareNo.ClientID + "','Please select whether or not this blade is a spare')" +
                            " && ValidateText('" + txtILO.ClientID + "','Please enter an ILO address')" +
                            " && ValidateText('" + txtDummy.ClientID + "','Please enter a dummy name')" +
                            " && ValidateText('" + txtMAC.ClientID + "','Please enter a mac address')" +
                            " && ValidateNumber0('" + txtVLAN.ClientID + "','Please enter a valid original VLAN')" +
                            ";");
                    }
                    else
                    {
                        panServer.Visible = true;
                        LoadLists();
                        ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',0);");
                        ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
                        btnSubmit.Attributes.Add("onclick", "return ValidateDropDown('" + ddlStatus.ClientID + "','Please select a status')" +
                            " && ValidateDropDown('" + ddlModels.ClientID + "','Please select a model')" +
                            " && ValidateDropDown('" + ddlClass.ClientID + "','Please select a class')" +
                            " && ValidateDropDown('" + ddlEnvironment.ClientID + "','Please select an environment')" +
                            " && ValidateHidden0('" + hdnRackId.ClientID + "','" + btnChangeLocation.ClientID + "','Please select a location details')" +
                            " && ValidateHidden0('" + hdnRackPosition.ClientID + "','" + btnChangeLocation.ClientID +"','Please enter a rack position')" +
                            " && ValidateText('" + txtILO.ClientID + "','Please enter an ILO address')" +
                            " && ValidateText('" + txtDummy.ClientID + "','Please enter a dummy name')" +
                            " && ValidateText('" + txtMAC.ClientID + "','Please enter a mac address')" +
                            " && ValidateNumber0('" + txtVLAN.ClientID + "','Please enter a valid original VLAN')" +
                            ";");
                    }
                }
            }
            btnClose.Attributes.Add("onclick", "return parent.HidePanel();");
            btnHBAs.Attributes.Add("onclick", "return OpenWindow('ASSET_DEPLOY_HBAs','" + Request.QueryString["id"] + "');");
        }
        public void LoadLists()
        {
            Classes oClasses = new Classes(intProfile, dsn);
            ddlClass.DataValueField = "id";
            ddlClass.DataTextField = "name";
            ddlClass.DataSource = oClasses.Gets(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            int intAddress = 0;
            if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                intAddress = Int32.Parse(Request.QueryString["aid"]);
            Locations oLocation = new Locations(intProfile, dsn);
            //ddlLocation.DataValueField = "id";
            //ddlLocation.DataTextField = "fullname";
            //if (intAddress == 0)
            //    ddlLocation.DataSource = oLocation.GetAddressCommon();
            //else
            //    ddlLocation.DataSource = oLocation.GetAddresss(1);
            //ddlLocation.DataBind();
            //ddlLocation.SelectedValue = intAddress.ToString();
            //if (intAddress == 0)
            //    ddlLocation.Items.Add(new ListItem("-- NOT LISTED --", "-1"));
            //ddlLocation.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            //Rooms oRooms = new Rooms(intProfile, dsn);
            //ddlRoom.DataValueField = "id";
            //ddlRoom.DataTextField = "name";
            //ddlRoom.DataSource = oRooms.Gets(1);
            //ddlRoom.DataBind();
            //ddlRoom.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            //Racks oRacks = new Racks(intProfile, dsn);
            //ddlRack.DataValueField = "id";
            //ddlRack.DataTextField = "name";
            //ddlRack.DataSource = oRacks.Gets(1);
            //ddlRack.DataBind();
            //ddlRack.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlBuildNetwork.DataValueField = "id";
            ddlBuildNetwork.DataTextField = "name";
            ddlBuildNetwork.DataSource = oSolaris.GetBuildNetworks(1);
            ddlBuildNetwork.DataBind();
            ddlBuildNetwork.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            
            ddlResiliency.DataValueField = "id";
            ddlResiliency.DataTextField = "name";
            ddlResiliency.DataSource = oResiliency.Gets(1);
            ddlResiliency.DataBind();
            ddlResiliency.Items.Insert(0, new ListItem("-- SELECT --", "0"));

            ddlOperatingSystemGroup.DataValueField = "id";
            ddlOperatingSystemGroup.DataTextField = "name";
            ddlOperatingSystemGroup.DataSource = oOperatingSystem.GetGroups(1);
            ddlOperatingSystemGroup.DataBind();
            ddlOperatingSystemGroup.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void ddlLocation_Change(Object Sender, EventArgs e)
        {
         //   Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&aid=" + ddlLocation.SelectedItem.Value);
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            oAsset.UpdateModel(intID, Int32.Parse(ddlModels.SelectedItem.Value));
            if (panServer.Visible == true)
            {
                // Server
                oAsset.AddServer(intID, Int32.Parse(ddlStatus.SelectedItem.Value), intProfile, DateTime.Now, Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), Int32.Parse(hdnRackId.Value), hdnRackPosition.Value.Trim(), txtILO.Text, txtDummy.Text, txtMAC.Text, Int32.Parse(txtVLAN.Text), Int32.Parse(ddlBuildNetwork.SelectedItem.Value), Int32.Parse(ddlResiliency.SelectedItem.Value), Int32.Parse(ddlOperatingSystemGroup.SelectedItem.Value));
            }
            else
            {
                // Blade
                oAsset.AddBlade(intID, Int32.Parse(ddlStatus.SelectedItem.Value), intProfile, DateTime.Now, Int32.Parse(ddlEnclosure.SelectedItem.Value), txtILO.Text, txtDummy.Text, txtMAC.Text, Int32.Parse(txtVLAN.Text), Int32.Parse(ddlBuildNetwork.SelectedItem.Value), Int32.Parse(txtSlot.Text), (radSpareYes.Checked ? 1 : 0), Int32.Parse(ddlResiliency.SelectedItem.Value), Int32.Parse(ddlOperatingSystemGroup.SelectedItem.Value));
            }
            Response.Redirect(Request.Path + "?save=true");
        }
    }
}
