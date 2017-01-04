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
    public partial class asset_decommission_wan : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Users oUser;
        protected Asset oAsset;
        protected Depot oDepot;
        protected DepotRoom oDepotRoom;
        protected Shelf oShelf;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intPlatform = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oDepot = new Depot(intProfile, dsn);
            oDepotRoom = new DepotRoom(intProfile, dsn);
            oShelf = new Shelf(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (!IsPostBack)
            {
                if (Request.QueryString["decom"] != null)
                    btnCancel.Visible = true;
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    LoadLists();
                    Load(Int32.Parse(Request.QueryString["id"]));
                    txtDate.Text = DateTime.Today.ToShortDateString();
                    imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
                    radStatusDecom.Attributes.Add("onclick", "ShowHideDiv('" + divDecom1.ClientID + "','inline');ShowHideDiv('" + divDecom2.ClientID + "','inline');ShowHideDiv('" + divDecom3.ClientID + "','inline');");
                    radStatusDispose.Attributes.Add("onclick", "ShowHideDiv('" + divDecom1.ClientID + "','none');ShowHideDiv('" + divDecom2.ClientID + "','none');ShowHideDiv('" + divDecom3.ClientID + "','none');");
                    btnSubmit.Attributes.Add("onclick", "return ValidateRadioButtons('" + radStatusDecom.ClientID + "','" + radStatusDispose.ClientID + "','Please select an asset status')" +
                        " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid date')" +
                        " && (document.getElementById('" + radStatusDecom.ClientID + "').checked == false || (document.getElementById('" + radStatusDecom.ClientID + "').checked == true && ValidateDropDown('" + ddlDecomLocation.ClientID + "','Please make a selection for the depot location')))" +
                        " && (document.getElementById('" + radStatusDecom.ClientID + "').checked == false || (document.getElementById('" + radStatusDecom.ClientID + "').checked == true && ValidateDropDown('" + ddlRoom.ClientID + "','Please make a selection for the room')))" +
                        " && (document.getElementById('" + radStatusDecom.ClientID + "').checked == false || (document.getElementById('" + radStatusDecom.ClientID + "').checked == true && ValidateDropDown('" + ddlShelf.ClientID + "','Please make a selection for the shelf on which the asset resides')))" +
                        ";");
                }
            }
        }
        private void LoadLists()
        {
            ddlDecomLocation.DataValueField = "id";
            ddlDecomLocation.DataTextField = "name";
            ddlDecomLocation.DataSource = oDepot.Gets(1);
            ddlDecomLocation.DataBind();
            ddlDecomLocation.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlRoom.DataValueField = "id";
            ddlRoom.DataTextField = "name";
            ddlRoom.DataSource = oDepotRoom.Gets(1);
            ddlRoom.DataBind();
            ddlRoom.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlShelf.DataValueField = "id";
            ddlShelf.DataTextField = "name";
            ddlShelf.DataSource = oShelf.Gets(1);
            ddlShelf.DataBind();
            ddlShelf.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void Load(int _id)
        {
            DataSet ds = oAsset.GetNetwork(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                lblSerial.Text = ds.Tables[0].Rows[0]["serial"].ToString();
                lblAsset.Text = ds.Tables[0].Rows[0]["asset"].ToString();
                lblType.Text = ds.Tables[0].Rows[0]["type"].ToString();
                lblModel.Text = ds.Tables[0].Rows[0]["model"].ToString();
                lblLocation.Text = ds.Tables[0].Rows[0]["location"].ToString();
                lblRack.Text = ds.Tables[0].Rows[0]["rack"].ToString();
                lblEnvironment.Text = ds.Tables[0].Rows[0]["environment"].ToString();
                lblClass.Text = ds.Tables[0].Rows[0]["class"].ToString();
                lblCommissionedOn.Text = DateTime.Parse(ds.Tables[0].Rows[0]["datestamp"].ToString()).ToLongDateString();
                lblCommissionedBy.Text = ds.Tables[0].Rows[0]["statusby"].ToString();
                lblDecommissionedBy.Text = oUser.GetFullName(intProfile);
            }
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            int intAsset = Int32.Parse(Request.QueryString["id"]);
            if (radStatusDecom.Checked == true)
                oAsset.UpdateNetwork(intAsset, "", (int)AssetStatus.Available, intProfile, DateTime.Parse(txtDate.Text), Int32.Parse(ddlDecomLocation.SelectedItem.Value), Int32.Parse(ddlRoom.SelectedItem.Value), Int32.Parse(ddlShelf.SelectedItem.Value), Int32.Parse(oAsset.GetNetwork(intAsset, "ports")), 0, 0, 0, 0, 0, "");
            else
                oAsset.UpdateNetwork(intAsset, "", (int)AssetStatus.Disposed, intProfile, DateTime.Parse(txtDate.Text), 0, 0, 0, Int32.Parse(oAsset.GetNetwork(intAsset, "ports")), 0, 0, 0, 0, 0, "");
            if (Request.QueryString["sid"] != null)
                Response.Redirect(oPage.GetFullLink(intPage) + "?sid=" + Request.QueryString["sid"] + "&id=" + Request.QueryString["id"] + "&decommed=true");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&tid=" + Request.QueryString["tid"] + "&save=true");
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?sid=" + Request.QueryString["sid"] + "&id=" + Request.QueryString["id"]);
        }
    }
}