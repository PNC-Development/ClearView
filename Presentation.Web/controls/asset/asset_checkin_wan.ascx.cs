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
    public partial class asset_checkin_wan : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Asset oAsset;
        protected Platforms oPlatform;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Depot oDepot;
        protected DepotRoom oDepotRoom;
        protected Shelf oShelf;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oDepot = new Depot(intProfile, dsn);
            oDepotRoom = new DepotRoom(intProfile, dsn);
            oShelf = new Shelf(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Asset Successfully Added');window.navigate('" + oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&tid=" + Request.QueryString["tid"] + "&mid=" + Request.QueryString["mid"] + "');<" + "/" + "script>");
            if (Request.QueryString["duplicate"] != null && Request.QueryString["duplicate"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "duplicated", "<script type=\"text/javascript\">alert('Duplicate Asset Information Found.\\n\\nAnother asset has the same serial number or asset tag as the one you entered. You can locate this device by using the search page.');window.navigate('" + oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&tid=" + Request.QueryString["tid"] + "&mid=" + Request.QueryString["mid"] + "');<" + "/" + "script>");
            if (!IsPostBack)
            {
                LoadLists();
                txtDate.Text = DateTime.Today.ToShortDateString();
            }
            imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
            btnSubmit.Attributes.Add("onclick", "return ValidateDropDown('" + ddlModel.ClientID + "','Please make a selection for the model')" +
                " && ValidateText('" + txtSerial.ClientID + "','Please enter the serial number')" +
                " && ValidateDropDown('" + ddlDepot.ClientID + "','Please make a selection for the location of the asset')" +
                " && ValidateText('" + txtDepotRoom.ClientID + "','Please enter the depot room')" +
                " && ValidateText('" + txtShelf.ClientID + "','Please enter the shelf')" +
                " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid date')" +
                " && ValidateNumber0('" + txtPorts.ClientID + "','Please enter a valid number of available ports')" +
                ";");
        }
        private void LoadLists()
        {
            int intPlatform = 0;
            if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != "")
                intPlatform = Int32.Parse(Request.QueryString["pid"]);
            if (oPlatform.Get(intPlatform).Tables[0].Rows.Count > 0)
            {
                int intType = 0;
                if (Request.QueryString["tid"] != null && Request.QueryString["tid"] != "")
                    intType = Int32.Parse(Request.QueryString["tid"]);
                if (oType.Get(intType).Tables[0].Rows.Count > 0)
                {
                    ddlModel.Items.Clear();
                    ddlModel.DataValueField = "id";
                    ddlModel.DataTextField = "name";
                    ddlModel.DataSource = oModelsProperties.GetTypes(1, intType, 1);
                    ddlModel.DataBind();
                    if (Request.QueryString["mid"] != null && Request.QueryString["mid"] != "")
                        ddlModel.SelectedValue = Request.QueryString["mid"];
                    ddlDepot.DataValueField = "id";
                    ddlDepot.DataTextField = "name";
                    ddlDepot.DataSource = oDepot.Gets(1);
                    ddlDepot.DataBind();
                }
            }
            ddlModel.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlDepot.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            int intRoom = GetID(txtDepotRoom.Text, "cv_depot_rooms");
            if (intRoom == 0)
            {
                oDepotRoom.Add(txtDepotRoom.Text, 1);
                intRoom = GetID(txtDepotRoom.Text, "cv_depot_rooms");
            }
            int intShelf = GetID(txtShelf.Text, "cv_shelfs");
            if (intShelf == 0)
            {
                oShelf.Add(txtShelf.Text, 1);
                intShelf = GetID(txtShelf.Text, "cv_shelfs");
            }
            int intModel = Int32.Parse(ddlModel.SelectedItem.Value);
            DataSet ds = oAsset.Get(txtSerial.Text, intModel);
            if (ds.Tables[0].Rows.Count > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&tid=" + Request.QueryString["tid"] + "&mid=" + ddlModel.SelectedItem.Value + "&duplicate=true");
            else
            {
                int intAsset = oAsset.Add("", intModel, txtSerial.Text, txtAsset.Text, (int)AssetStatus.Arrived, intProfile, DateTime.Now, 0, 1);
                oAsset.AddNetwork(intAsset, "", (int)AssetStatus.Available, intProfile, DateTime.Now, Int32.Parse(ddlDepot.SelectedItem.Value), intRoom, intShelf, Int32.Parse(txtPorts.Text), 0, 0, 0, 0, 0, "");
                Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + Request.QueryString["pid"] + "&tid=" + Request.QueryString["tid"] + "&mid=" + ddlModel.SelectedItem.Value + "&save=true");
            }
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