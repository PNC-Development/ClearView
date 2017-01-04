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
using System.Collections.Specialized;
using NCC.ClearView.Application.Core.ClearViewWS;
using System.Threading;
namespace NCC.ClearView.Presentation.Web
{
    public partial class ip_address_controls : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Locations oLocation;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected IPAddresses oIPAddresses;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strRedirect = "";
        protected bool boolDelete = false;
        private int[] intIPArray;
        protected int intArrayCount = 0;
        protected int intResetNetwork1 = 0;
        protected int intResetNetwork2 = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
            intIPArray = new int[100];
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            string strPublic = "";
            string strProduction = "";
            string strService = "";
            string strPublic1 = "";
            string strProduction1 = "";
            string strService1 = "";
            string strPrivate1 = "";
            string strPrivate2 = "";
            string strWorkstation = "";
            string strIPs = "";
            bool boolDenied = false;
            bool boolNodes = false;
            if (Request.QueryString["pub"] != null && Request.QueryString["pub"] != "")
            {
                strPublic = LoadNames("pub");
                if (strPublic == "")
                    boolDenied = true;
                else
                    strIPs += "<tr><td valign=\"top\"><b>Public:</b></td><td valign=\"top\">" + strPublic + "</td></tr>";
            }
            if (Request.QueryString["prod"] != null && Request.QueryString["prod"] != "")
            {
                strProduction = LoadNames("prod");
                if (strProduction == "")
                    boolDenied = true;
                else
                    strIPs += "<tr><td valign=\"top\"><b>Production:</b></td><td valign=\"top\">" + strProduction + "</td></tr>";
            }
            if (Request.QueryString["serv"] != null && Request.QueryString["serv"] != "")
            {
                strService = LoadNames("serv");
                if (strService == "")
                    boolDenied = true;
                else
                    strIPs += "<tr><td valign=\"top\"><b>Service:</b></td><td valign=\"top\">" + strService + "</td></tr>";
            }
            if (Request.QueryString["pub1"] != null && Request.QueryString["pub1"] != "")
            {
                boolNodes = true;
                strPublic1 = LoadNodes("pub1");
                if (strPublic1 == "")
                    boolDenied = true;
            }
            if (Request.QueryString["prod1"] != null && Request.QueryString["prod1"] != "")
            {
                boolNodes = true;
                strProduction1 = LoadNodes("prod1");
                if (strProduction1 == "")
                    boolDenied = true;
            }
            if (Request.QueryString["serv1"] != null && Request.QueryString["serv1"] != "")
            {
                boolNodes = true;
                strService1 = LoadNodes("serv1");
                if (strService1 == "")
                    boolDenied = true;
            }
            if (Request.QueryString["priv1"] != null && Request.QueryString["priv1"] != "")
            {
                boolNodes = true;
                strPrivate1 = LoadNodes("priv1");
                if (strPrivate1 == "")
                    boolDenied = true;
            }
            if (Request.QueryString["priv2"] != null && Request.QueryString["priv2"] != "")
            {
                boolNodes = true;
                strPrivate2 = LoadNodes("priv2");
                if (strPrivate2 == "")
                    boolDenied = true;
            }
            if (Request.QueryString["wkst"] != null && Request.QueryString["wkst"] != "")
            {
                strWorkstation = LoadNames("wkst");
                if (strWorkstation == "")
                    boolDenied = true;
                else
                    strIPs += "<tr><td valign=\"top\"><b>Workstation:</b></td><td valign=\"top\">" + strWorkstation + "</td></tr>";
            }
            if (boolDenied == false && (strIPs != "" || boolNodes == true))
            {
                if (boolNodes == true)
                {
                    if (strPublic1 != "")
                        strIPs += "<tr><td><b><u>PUBLIC</u></b></td></tr><tr><td>" + strPublic1 + "</td></tr>";
                    if (strProduction1 != "")
                        strIPs += "<tr><td><b><u>PRODUCTION</u></b></td></tr><tr><td>" + strProduction1 + "</td></tr>";
                    if (strService1 != "")
                        strIPs += "<tr><td><b><u>SERVICE</u></b></td></tr><tr><td>" + strService1 + "</td></tr>";
                    if (strPrivate1 != "")
                        strIPs += "<tr><td><b><u>PRIVATE 1</u></b></td></tr><tr><td>" + strPrivate1 + "</td></tr>";
                    if (strPrivate2 != "")
                        strIPs += "<tr><td><b><u>PRIVATE 2</u></b></td></tr><tr><td>" + strPrivate2 + "</td></tr>";
                }
                lblName.Text = "<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\">" + strIPs + "</table>";
                panName.Visible = true;
                btnDuplicate.Attributes.Add("onclick", "return confirm('Are you sure you want to duplicate the configuration?') && ProcessButton(this);");
                btnDuplicate.Enabled = false;
            }
            if (!IsPostBack)
            {
                btnSubmit.Enabled = false;
                radCSMVip.Enabled = false;
                radCluster.Enabled = false;
                radILO.Enabled = false;
                radServer.Enabled = false;
                radAdditional.Enabled = false;
                LoadLists();
                btnSubmit.Attributes.Add("onclick", "return Ensure('" + radCSMVip.ClientID + "','" + radCluster.ClientID + "','" + radILO.ClientID + "','" + radServer.ClientID + "','" + radAdditional.ClientID + "','" + txtURL.ClientID + "','" + hdnProject.ClientID + "','" + txtProject.ClientID + "','" + txtInstance.ClientID + "','" + ddlVlan.ClientID + "','" + txtSerial.ClientID + "','" + radSingleServer.ClientID + "','" + radClusterWindows.ClientID + "','" + radClusterUnixRAC.ClientID + "','" + radClusterUnixOther.ClientID + "','" + radClusterVCS.ClientID + "','" + hdnModel.ClientID + "','" + ddlClusterModelProperty.ClientID + "','" + txtServer.ClientID + "','" + radCSM.ClientID + "','" + radLTM.ClientID + "','" + radNone.ClientID + "','" + ddlCSMVlan.ClientID + "','" + ddlLTMVlan.ClientID + "','" + ddlType.ClientID + "','" + ddlBladeHpVlan.ClientID + "','" + ddlBladeSunVlan.ClientID + "','" + ddlHardware.ClientID + "','" + ddlClusterBladeHp.ClientID + "','" + ddlClusterBladeSun.ClientID + "','" + hdnNodes.ClientID + "','" + txtNode.ClientID + "','" + txtAdditional.ClientID + "','" + ddlAdditionalVlan.ClientID + "','" + divSending.ClientID + "','" + hdnModel.ClientID + "','" + ddlSendingModelProperty.ClientID + "','" + txtSendingRoom.ClientID + "','" + txtSendingRack.ClientID + "') && ProcessButton(this);");
            }
            btnParent.Attributes.Add("onclick", "return OpenLocations('" + hdnParent.ClientID + "','" + txtParent.ClientID + "');");
            ddlLocation.Attributes.Add("onchange", "WaitDDL('" + divLocation.ClientID + "');");
            ddlClass.Attributes.Add("onchange", "WaitDDL('" + divClass.ClientID + "');");
            ddlEnvironment.Attributes.Add("onchange", "WaitDDL('" + divEnvironment.ClientID + "');");
            radCSMVip.Attributes.Add("onclick", "ShowHideDiv('" + divCSMVip.ClientID + "','inline');ShowHideDiv('" + divCluster.ClientID + "','none');ShowHideDiv('" + divILO.ClientID + "','none');ShowHideDiv('" + divServer.ClientID + "','none');ShowHideDiv('" + divAdditional.ClientID + "','none');ShowHideDiv('" + divSending.ClientID + "','none');");
            radCluster.Attributes.Add("onclick", "ShowHideDiv('" + divCluster.ClientID + "','inline');ShowHideDiv('" + divCSMVip.ClientID + "','none');ShowHideDiv('" + divILO.ClientID + "','none');ShowHideDiv('" + divServer.ClientID + "','none');ShowHideDiv('" + divAdditional.ClientID + "','none');ShowHideDiv('" + divSending.ClientID + "','none');");
            radILO.Attributes.Add("onclick", "ShowHideDiv('" + divILO.ClientID + "','inline');ShowHideDiv('" + divCSMVip.ClientID + "','none');ShowHideDiv('" + divCluster.ClientID + "','none');ShowHideDiv('" + divServer.ClientID + "','none');ShowHideDiv('" + divAdditional.ClientID + "','none');ShowHideDiv('" + divSending.ClientID + "','none');");
            radServer.Attributes.Add("onclick", "ShowHideDiv('" + divServer.ClientID + "','inline');ShowHideDiv('" + divCSMVip.ClientID + "','none');ShowHideDiv('" + divCluster.ClientID + "','none');ShowHideDiv('" + divILO.ClientID + "','none');ShowHideDiv('" + divAdditional.ClientID + "','none');");
            radAdditional.Attributes.Add("onclick", "ShowHideDiv('" + divAdditional.ClientID + "','inline');ShowHideDiv('" + divCSMVip.ClientID + "','none');ShowHideDiv('" + divCluster.ClientID + "','none');ShowHideDiv('" + divILO.ClientID + "','none');ShowHideDiv('" + divServer.ClientID + "','none');ShowHideDiv('" + divSending.ClientID + "','none');");
            radSingleServer.Attributes.Add("onclick", "ShowHideDiv('" + divSingleServer.ClientID + "','inline');ShowHideDiv('" + divClusterHardware.ClientID + "','none');ShowHideDiv('" + divClusterSystem.ClientID + "','none');ShowHideDiv('" + divSending.ClientID + "','none');");
            radClusterWindows.Attributes.Add("onclick", "ShowHideDiv('" + divClusterSystem.ClientID + "','inline');ShowHideDiv('" + divClusterHardware.ClientID + "','inline');ShowHideDiv('" + divSingleServer.ClientID + "','none');ShowHideDiv('" + divSending.ClientID + "','none');");
            radClusterUnixRAC.Attributes.Add("onclick", "ShowHideDiv('" + divClusterSystem.ClientID + "','inline');ShowHideDiv('" + divClusterHardware.ClientID + "','none');ShowHideDiv('" + divSingleServer.ClientID + "','none');ShowHideDiv('" + divSending.ClientID + "','none');");
            radClusterUnixOther.Attributes.Add("onclick", "ShowHideDiv('" + divClusterSystem.ClientID + "','inline');ShowHideDiv('" + divClusterHardware.ClientID + "','none');ShowHideDiv('" + divSingleServer.ClientID + "','none');ShowHideDiv('" + divSending.ClientID + "','none');");
            radClusterVCS.Attributes.Add("onclick", "ShowHideDiv('" + divClusterSystem.ClientID + "','inline');ShowHideDiv('" + divClusterHardware.ClientID + "','none');ShowHideDiv('" + divSingleServer.ClientID + "','none');ShowHideDiv('" + divSending.ClientID + "','none');");
            radCSM.Attributes.Add("onclick", "ShowHideDiv('" + divCSM.ClientID + "','inline');ShowHideDiv('" + divCSM2.ClientID + "','inline');ShowHideDiv('" + divLTM.ClientID + "','none');ShowHideDiv('" + divLTM2.ClientID + "','none');ShowHideDiv('" + divCSMNo.ClientID + "','none');");
            radLTM.Attributes.Add("onclick", "ShowHideDiv('" + divLTM.ClientID + "','inline');ShowHideDiv('" + divLTM2.ClientID + "','inline');ShowHideDiv('" + divCSM.ClientID + "','none');ShowHideDiv('" + divCSM2.ClientID + "','none');ShowHideDiv('" + divCSMNo.ClientID + "','none');");
            radNone.Attributes.Add("onclick", "ShowHideDiv('" + divCSMNo.ClientID + "','inline');ShowHideDiv('" + divCSM.ClientID + "','none');ShowHideDiv('" + divCSM2.ClientID + "','none');ShowHideDiv('" + divLTM.ClientID + "','none');ShowHideDiv('" + divLTM2.ClientID + "','none');");
            ddlHardware.Attributes.Add("onchange", "CheckBlade(this,'" + divClusterBladeHp.ClientID + "','" + divClusterBladeHp2.ClientID + "','" + divClusterBladeSun.ClientID + "','" + divClusterBladeSun2.ClientID + "');");
            ddlType.Attributes.Add("onchange", "CheckBlade(this,'" + divBladeHpVlan.ClientID + "','" + divBladeHpVlan2.ClientID + "','" + divBladeSunVlan.ClientID + "','" + divBladeSunVlan2.ClientID + "');");
            Variables oVariable = new Variables(intEnvironment);
            txtProject.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divProject.ClientID + "','" + lstProject.ClientID + "','" + hdnProject.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_projects.aspx',0);");
            lstProject.Attributes.Add("ondblclick", "AJAXClickRow();");
            btnNodeAdd.Attributes.Add("onclick", "return AddListBoxItem('" + txtNode.ClientID + "','" + txtClusterRoom.ClientID + "','" + txtClusterRack.ClientID + "','" + txtClusterEnclosure.ClientID + "','" + txtClusterSlot.ClientID + "','" + lstNodes.ClientID + "','" + hdnNodes.ClientID + "');");
            btnNodeRemove.Attributes.Add("onclick", "return RemoveListBoxItem('" + lstNodes.ClientID + "','" + hdnNodes.ClientID + "');");
            txtNode.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnNodeAdd.ClientID + "').click();return false;}} else {return true}; ");
            Types oType = new Types(intProfile, dsn);
            ddlSendingType.DataValueField = "id";
            ddlSendingType.DataTextField = "name";
            ddlSendingType.DataSource = oType.Gets(1, 1);
            ddlSendingType.DataBind();
            ddlSendingType.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlSendingType.Attributes.Add("onchange", "PopulatePlatformModels('" + ddlSendingType.ClientID + "','" + ddlSendingModel.ClientID + "','" + ddlSendingModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
            ddlSendingModel.Attributes.Add("onchange", "PopulatePlatformModelProperties('" + ddlSendingModel.ClientID + "','" + ddlSendingModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
            ddlSendingModelProperty.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlSendingModelProperty.ClientID + "','" + hdnModel.ClientID + "');");
            ddlClusterType.DataValueField = "id";
            ddlClusterType.DataTextField = "name";
            ddlClusterType.DataSource = oType.Gets(1, 1);
            ddlClusterType.DataBind();
            ddlClusterType.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlClusterType.Attributes.Add("onchange", "PopulatePlatformModels('" + ddlClusterType.ClientID + "','" + ddlClusterModel.ClientID + "','" + ddlClusterModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
            ddlClusterModel.Attributes.Add("onchange", "PopulatePlatformModelProperties('" + ddlClusterModel.ClientID + "','" + ddlClusterModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
            ddlClusterModelProperty.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlClusterModelProperty.ClientID + "','" + hdnModel.ClientID + "');");
        }
        private string LoadNames(string _querystring)
        {
            string strReturn = "";
            NameValueCollection oCollection = Request.QueryString;
            string strQuerys = oCollection.Get(_querystring);
            char[] strSplit = { ',' };
            string[] strQuery = strQuerys.Split(strSplit);
            for (int ii = 0; ii < strQuery.Length; ii++)
            {
                if (strQuery[ii].Trim() != "")
                {
                    int intAddress = Int32.Parse(strQuery[ii].Trim());
                    string strName = oIPAddresses.GetName(intAddress, intProfile);
                    if (strName == "DENIED")
                    {
                        strReturn = "";
                        break;
                    }
                    else
                    {
                        if (strReturn != "")
                            strReturn += "<br/>";
                        int intNetwork = Int32.Parse(oIPAddresses.Get(intAddress, "networkid"));
                        int intVlan = Int32.Parse(oIPAddresses.GetNetwork(intNetwork, "vlanid"));
                        strReturn += strName + "&nbsp;&nbsp;(Subnet Mask: " + oIPAddresses.GetNetwork(intNetwork, "mask") + ")&nbsp;&nbsp;&nbsp;(Gateway: " + oIPAddresses.GetNetwork(intNetwork, "gateway") + ")&nbsp;&nbsp;&nbsp;(VLAN: " + oIPAddresses.GetVlan(intVlan, "vlan") + ")";
                    }
                }
            }
            return strReturn;
        }
        private string LoadNodes(string _querystring)
        {
            string strReturn = "";
            NameValueCollection oCollection = Request.QueryString;
            string strQuerys = oCollection.Get(_querystring);
            char[] strSplit = { ',' };
            string[] strQuery = strQuerys.Split(strSplit);
            for (int ii = 0; ii < strQuery.Length; ii++)
            {
                if (strQuery[ii].Trim() != "")
                {
                    int intAddress = Int32.Parse(strQuery[ii].Trim());
                    string strName = oIPAddresses.GetName(intAddress, intProfile);
                    if (strName == "DENIED")
                    {
                        strReturn = "";
                        break;
                    }
                    else
                    {
                        int intTemp = ii + 1;
                        strReturn += "<tr><td>Node " + intTemp.ToString() + ":</td><td>" + strName + "</td></tr>";
                    }
                }
            }
            if (strReturn != "")
                strReturn = "<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\">" + strReturn + "</table>";
            return strReturn;
        }
        protected void DeleteIPs(string _querystring)
        {
           
            NameValueCollection oCollection = Request.QueryString;
            string strQuerys = oCollection.Get(_querystring);
            char[] strSplit = { ',' };
            string[] strQuery = strQuerys.Split(strSplit);
            for (int ii = 0; ii < strQuery.Length; ii++)
            {
                if (strQuery[ii].Trim() != "")
                {
                    int intAddress = Int32.Parse(strQuery[ii].Trim());
                    if (intAddress > 0)
                        oIPAddresses.Delete(intAddress);
                }
            }
        }
        private void LoadLists()
        {
            ddlLocation.Enabled = true;
            ddlLocation.DataValueField = "id";
            ddlLocation.DataTextField = "name";
            ddlLocation.DataSource = oLocation.GetAddressCommon();
            ddlLocation.DataBind();
            ddlLocation.Items.Add(new ListItem("-- NOT LISTED --", "-1"));
            ddlLocation.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            int intAddress = 0;
            if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                intAddress = Int32.Parse(Request.QueryString["aid"]);
            if (oLocation.GetAddress(intAddress).Tables[0].Rows.Count > 0)
            {
                ddlLocation.SelectedValue = intAddress.ToString();
                hdnParent.Value = intAddress.ToString();
                if (ddlLocation.SelectedItem.Value == "0")
                {
                    ddlLocation.SelectedValue = "-1";
                    panLocation.Visible = true;
                    txtParent.Text = oLocation.GetFull(intAddress);
                }
                ddlClass.Enabled = true;
                ddlClass.DataValueField = "id";
                ddlClass.DataTextField = "name";
                ddlClass.DataSource = oIPAddresses.GetVlansClasses(intAddress);
                ddlClass.DataBind();
                lblConfig.Visible = (ddlClass.Items.Count == 0);
                ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                int intClass = 0;
                if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
                    intClass = Int32.Parse(Request.QueryString["cid"]);
                if (oClass.Get(intClass).Tables[0].Rows.Count > 0)
                {
                    ddlClass.SelectedValue = intClass.ToString();
                    ddlEnvironment.Enabled = true;
                    Environments oEnvironment = new Environments(intProfile, dsn);
                    ddlEnvironment.DataValueField = "id";
                    ddlEnvironment.DataTextField = "name";
                    ddlEnvironment.DataSource = oIPAddresses.GetVlansEnvironments(intAddress, intClass);
                    ddlEnvironment.DataBind();
                    lblConfig.Visible = (lblConfig.Visible || ddlEnvironment.Items.Count == 0);
                    ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                    int intEnv = 0;
                    if (Request.QueryString["eid"] != null && Request.QueryString["eid"] != "")
                        intEnv = Int32.Parse(Request.QueryString["eid"]);
                    if (oEnvironment.Get(intEnv).Tables[0].Rows.Count > 0)
                    {
                        ddlEnvironment.SelectedValue = intEnv.ToString();
                        hdnNetwork.Value = "0";

                        ddlVlan.DataValueField = "id";
                        ddlVlan.DataTextField = "vlan";
                        ddlVlan.DataSource = oIPAddresses.GetVlansList(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 1);
                        ddlVlan.DataBind();
                        ddlVlan.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        ddlVlan.Attributes.Add("onchange", "PopulateIPNetworks('" + ddlVlan.ClientID + "','" + ddlVlanNetwork.ClientID + "');");
                        ddlVlanNetwork.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlVlanNetwork.ClientID + "','" + hdnNetwork.ClientID + "');");

                        ddlVlanNetwork.DataValueField = "id";
                        ddlVlanNetwork.DataTextField = "name";
                        ddlVlanNetwork.DataSource = oIPAddresses.GetNetworksList(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 1);
                        ddlVlanNetwork.DataBind();
                        ddlVlanNetwork.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        ddlVlanNetwork.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlVlanNetwork.ClientID + "','" + hdnNetwork.ClientID + "');PopulateIPVLAN('" + ddlVlan.ClientID + "','" + ddlVlanNetwork.ClientID + "');");

                        ddlCSMVlan.DataValueField = "id";
                        ddlCSMVlan.DataTextField = "vlan";
                        ddlCSMVlan.DataSource = oIPAddresses.GetVlansList(intClass, intEnv, intAddress, 0, 0, 1, 0, 0, 0, 1);
                        ddlCSMVlan.DataBind();
                        ddlCSMVlan.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        ddlCSMVlan.Attributes.Add("onchange", "PopulateIPNetworks('" + ddlCSMVlan.ClientID + "','" + ddlCSMVlanNetwork.ClientID + "');");
                        ddlCSMVlanNetwork.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlCSMVlanNetwork.ClientID + "','" + hdnNetwork.ClientID + "');");

                        ddlLTMVlan.DataValueField = "id";
                        ddlLTMVlan.DataTextField = "vlan";
                        ddlLTMVlan.DataSource = oIPAddresses.GetVlansList(intClass, intEnv, intAddress, 0, 0, 0, 1, 0, 0, 1);
                        ddlLTMVlan.DataBind();
                        ddlLTMVlan.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        ddlLTMVlan.Attributes.Add("onchange", "PopulateIPNetworks('" + ddlLTMVlan.ClientID + "','" + ddlLTMVlanNetwork.ClientID + "');");
                        ddlLTMVlanNetwork.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlLTMVlanNetwork.ClientID + "','" + hdnNetwork.ClientID + "');");

                        ddlAdditionalVlan.DataValueField = "id";
                        ddlAdditionalVlan.DataTextField = "vlan";
                        ddlAdditionalVlan.DataSource = oIPAddresses.GetVlansList(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 1);
                        ddlAdditionalVlan.DataBind();
                        ddlAdditionalVlan.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        ddlAdditionalVlan.Attributes.Add("onchange", "PopulateIPNetworks('" + ddlAdditionalVlan.ClientID + "','" + ddlAdditionalVlanNetwork.ClientID + "');");
                        ddlAdditionalVlanNetwork.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlAdditionalVlanNetwork.ClientID + "','" + hdnNetwork.ClientID + "');");

                        ddlAdditionalVlanNetwork.DataValueField = "id";
                        ddlAdditionalVlanNetwork.DataTextField = "name";
                        ddlAdditionalVlanNetwork.DataSource = oIPAddresses.GetNetworksList(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 1);
                        ddlAdditionalVlanNetwork.DataBind();
                        ddlAdditionalVlanNetwork.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        ddlAdditionalVlanNetwork.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlAdditionalVlanNetwork.ClientID + "','" + hdnNetwork.ClientID + "');PopulateIPVLAN('" + ddlAdditionalVlan.ClientID + "','" + ddlAdditionalVlanNetwork.ClientID + "');");

                        ddlBladeHpVlan.DataValueField = "id";
                        ddlBladeHpVlan.DataTextField = "vlan";
                        ddlBladeHpVlan.DataSource = oIPAddresses.GetVlansList(intClass, intEnv, intAddress, 1, 0, 0, 0, 0, 0, 1);
                        ddlBladeHpVlan.DataBind();
                        ddlBladeHpVlan.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        ddlBladeHpVlan.Attributes.Add("onchange", "PopulateIPNetworks('" + ddlBladeHpVlan.ClientID + "','" + ddlBladeHpVlanNetwork.ClientID + "');");
                        ddlBladeHpVlanNetwork.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlBladeHpVlanNetwork.ClientID + "','" + hdnNetwork.ClientID + "');");

                        ddlBladeSunVlan.DataValueField = "id";
                        ddlBladeSunVlan.DataTextField = "vlan";
                        ddlBladeSunVlan.DataSource = oIPAddresses.GetVlansList(intClass, intEnv, intAddress, 0, 1, 0, 0, 0, 0, 1);
                        ddlBladeSunVlan.DataBind();
                        ddlBladeSunVlan.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        ddlBladeSunVlan.Attributes.Add("onchange", "PopulateIPNetworks('" + ddlBladeSunVlan.ClientID + "','" + ddlBladeSunVlanNetwork.ClientID + "');");
                        ddlBladeSunVlanNetwork.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlBladeSunVlanNetwork.ClientID + "','" + hdnNetwork.ClientID + "');");

                        ddlClusterBladeHp.DataValueField = "id";
                        ddlClusterBladeHp.DataTextField = "vlan";
                        ddlClusterBladeHp.DataSource = oIPAddresses.GetVlansList(intClass, intEnv, intAddress, 1, 0, 0, 0, 0, 0, 1);
                        ddlClusterBladeHp.DataBind();
                        ddlClusterBladeHp.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        ddlClusterBladeHp.Attributes.Add("onchange", "PopulateIPNetworks('" + ddlClusterBladeHp.ClientID + "','" + ddlClusterBladeHpNetwork.ClientID + "');");
                        ddlClusterBladeHpNetwork.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlClusterBladeHpNetwork.ClientID + "','" + hdnNetwork.ClientID + "');");

                        ddlClusterBladeSun.DataValueField = "id";
                        ddlClusterBladeSun.DataTextField = "vlan";
                        ddlClusterBladeSun.DataSource = oIPAddresses.GetVlansList(intClass, intEnv, intAddress, 0, 1, 0, 0, 0, 0, 1);
                        ddlClusterBladeSun.DataBind();
                        ddlClusterBladeSun.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        ddlClusterBladeSun.Attributes.Add("onchange", "PopulateIPNetworks('" + ddlClusterBladeSun.ClientID + "','" + ddlClusterBladeSunNetwork.ClientID + "');");
                        ddlClusterBladeSunNetwork.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlClusterBladeSunNetwork.ClientID + "','" + hdnNetwork.ClientID + "');");

                        //radCSMVip.Enabled = true;
                        //radCluster.Enabled = true;
                        //radILO.Enabled = true;
                        //radServer.Enabled = true;
                        radAdditional.Enabled = true;
                        btnSubmit.Enabled = true;
                    }
                }
                else
                {
                    ddlEnvironment.Enabled = false;
                    ddlEnvironment.Items.Insert(0, new ListItem("-- Please select a Class --", "0"));
                }
            }
            else
            {
                ddlLocation.SelectedValue = intAddress.ToString();
                if (intAddress == -1)
                {
                    panLocation.Visible = true;
                    hdnParent.Value = "0";
                }
                ddlClass.Enabled = false;
                ddlClass.Items.Insert(0, new ListItem("-- Please select a Location --", "0"));
                ddlEnvironment.Enabled = false;
                ddlEnvironment.Items.Insert(0, new ListItem("-- Please select a Location --", "0"));
            }
        }
        protected void ddlLocation_Change(Object Sender, EventArgs e)
        {
            if (ddlLocation.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?aid=" + ddlLocation.SelectedItem.Value);
            else if (ddlLocation.SelectedItem.Value == "-1")
                Response.Redirect(oPage.GetFullLink(intPage) + "?aid=-1");
            else
                Response.Redirect(oPage.GetFullLink(intPage));
        }
        protected void ddlClass_Change(Object Sender, EventArgs e)
        {
            if (ddlClass.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?aid=" + Request.QueryString["aid"] + "&cid=" + ddlClass.SelectedItem.Value);
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?aid=" + Request.QueryString["aid"]);
        }
        protected void ddlEnvironment_Change(Object Sender, EventArgs e)
        {
            if (ddlEnvironment.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?aid=" + Request.QueryString["aid"] + "&cid=" + ddlClass.SelectedItem.Value + "&eid=" + ddlEnvironment.SelectedItem.Value);
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?aid=" + Request.QueryString["aid"] + "&cid=" + ddlClass.SelectedItem.Value);
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            Variables oVariable = new Variables(intEnvironment);
            int intID = -1;
            int intDetail = 0;
            int intClass = Int32.Parse(ddlClass.SelectedItem.Value);
            int intEnv = Int32.Parse(ddlEnvironment.SelectedItem.Value);
            bool boolEcom = (oEnvironment.Get(intEnv, "ecom") == "1");
            int intAddress = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            int intNetwork = Int32.Parse(Request.Form[hdnNetwork.UniqueID]);
            if (radCSMVip.Checked == true)
            {
                if (boolEcom == false)
                {
                    intID = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, 0, intEnvironment, dsnServiceEditor);
                    if (intID > 0)
                    {
                        intDetail = oIPAddresses.AddDetails(txtURL.Text, Int32.Parse(Request.Form[hdnProject.UniqueID]), "", 0, "", "", intClass, intEnv, intAddress, 0, "", "", "", "", 0, 1);
                        oIPAddresses.AddDetail(intID, intDetail);
                        AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub");
                    }
                    else
                        boolDelete = true;
                }
                else
                {
                    intID = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, 0, intEnvironment, dsnServiceEditor);
                    if (intID > 0)
                    {
                        intDetail = oIPAddresses.AddDetails(txtURL.Text, Int32.Parse(Request.Form[hdnProject.UniqueID]), "", 0, "", "", intClass, intEnv, intAddress, 0, "", "", "", "", 0, 1);
                        oIPAddresses.AddDetail(intID, intDetail);
                        AddRelation(intClass, intEnv, intAddress, intID, intDetail, "prod");
                    }
                    else
                        boolDelete = true;
                    intID = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, 0, intEnvironment, dsnServiceEditor);
                    if (intID > 0)
                    {
                        intDetail = oIPAddresses.AddDetails(txtURL.Text, Int32.Parse(Request.Form[hdnProject.UniqueID]), "", 0, "", "", intClass, intEnv, intAddress, 0, "", "", "", "", 0, 1);
                        oIPAddresses.AddDetail(intID, intDetail);
                        AddRelation(intClass, intEnv, intAddress, intID, intDetail, "serv");
                    }
                    else
                        boolDelete = true;
                }
            }
            else if (radCluster.Checked == true)
            {
                int intVlan = Int32.Parse(ddlVlan.SelectedItem.Text);
                string strVLAN = oIPAddresses.GetVlan(Int32.Parse(ddlVlan.SelectedItem.Value), "vlan");
                if (boolEcom == false)
                {
                    intID = oIPAddresses.Get_VLANExclude(intClass, intEnv, intAddress, 0, 0, intVlan, intNetwork, 0, 0, 0, 0, 0, true, intEnvironment, dsnServiceEditor);
                    if (intID > 0)
                    {
                        intDetail = oIPAddresses.AddDetails("", 0, txtInstance.Text, intVlan, "", "", intClass, intEnv, intAddress, 0, "", "", "", "", 0, 2);
                        oIPAddresses.AddDetail(intID, intDetail);
                        AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub");
                        UpdateIPAM(txtInstance.Text, intID, IPAddressType.ClusterInstance);
                    }
                    else
                        boolDelete = true;
                }
                else
                {
                    intID = oIPAddresses.Get_VLANExclude(intClass, intEnv, intAddress, 1, 0, intVlan, intNetwork, 0, 0, 0, 0, 0, true, intEnvironment, dsnServiceEditor);
                    if (intID > 0)
                    {
                        intDetail = oIPAddresses.AddDetails("", 0, txtInstance.Text, intVlan, "", "", intClass, intEnv, intAddress, 0, "", "", "", "", 0, 2);
                        oIPAddresses.AddDetail(intID, intDetail);
                        AddRelation(intClass, intEnv, intAddress, intID, intDetail, "prod");
                        UpdateIPAM(txtInstance.Text, intID, IPAddressType.ClusterInstance);
                    }
                    else
                        boolDelete = true;
                    intID = oIPAddresses.Get_VLANExclude(intClass, intEnv, intAddress, 0, 1, intVlan, intNetwork, 0, 0, 0, 0, 0, true, intEnvironment, dsnServiceEditor);
                    if (intID > 0)
                    {
                        intDetail = oIPAddresses.AddDetails("", 0, txtInstance.Text, intVlan, "", "", intClass, intEnv, intAddress, 0, "", "", "", "", 0, 2);
                        oIPAddresses.AddDetail(intID, intDetail);
                        AddRelation(intClass, intEnv, intAddress, intID, intDetail, "serv");
                        UpdateIPAM(txtInstance.Text, intID, IPAddressType.ClusterInstance);
                    }
                    else
                        boolDelete = true;
                }
            }
            else if (radILO.Checked == true)
            {
                intID = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, 0, intEnvironment, dsnServiceEditor);
                if (intID > 0)
                {
                    intDetail = oIPAddresses.AddDetails("", 0, "", 0, txtSerial.Text, "", intClass, intEnv, intAddress, 0, "", "", "", "", 0, 3);
                    oIPAddresses.AddDetail(intID, intDetail);
                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub");
                    UpdateIPAM(txtSerial.Text, intID, IPAddressType.ILO);
                }
                else
                    boolDelete = true;
            }
            else if (radServer.Checked == true)
            {
                if (radSingleServer.Checked == true)
                {
                    if (radNone.Checked == true)
                    {
                        int intVMwareWindows = 0;
                        int intVMwareLinux = 0;
                        int intVMwareHost = 0;
                        int intAPV = 0;
                        int intBladesHP = 0;
                        int intPhysicalWindows = 0;
                        int intPhysicalUNIX = 0;
                        int intIPX = 0;
                        int intBladesSUN = 0;
                        if (ddlType.SelectedItem.Value == "1")
                            intVMwareWindows = 1;
                        if (ddlType.SelectedItem.Value == "2")
                            intVMwareLinux = 1;
                        if (ddlType.SelectedItem.Value == "3")
                            intVMwareHost = 1;
                        if (ddlType.SelectedItem.Value == "4")
                            intAPV = 1;
                        if (ddlType.SelectedItem.Value == "5")
                            intBladesHP = 1;
                        if (ddlType.SelectedItem.Value == "6")
                            intPhysicalWindows = 1;
                        if (ddlType.SelectedItem.Value == "7")
                            intPhysicalUNIX = 1;
                        if (ddlType.SelectedItem.Value == "8")
                            intIPX = 1;
                        if (ddlType.SelectedItem.Value == "10")
                            intBladesSUN = 1;
                        if (ddlType.SelectedItem.Value == "9")
                        {
                            if (boolEcom == false)
                            {
                                intID = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, 0, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", 0, "", txtServer.Text, intClass, intEnv, intAddress, Int32.Parse(ddlSendingModelProperty.SelectedItem.Value), txtSendingRoom.Text, txtSendingRack.Text, txtSendingEnclosure.Text, txtSendingSlot.Text, 0, Int32.Parse(ddlType.SelectedItem.Value));
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "wkst");
                                }
                                else
                                    boolDelete = true;
                            }
                            else
                            {
                                // NOT SURE ??
                                //intID = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                                //if (intID > 0)
                                //{
                                //    intDetail = oIPAddresses.AddDetails("", 0, "", 0, "", txtServer.Text, intClass, intEnv, intAddress, 0, Int32.Parse(ddlType.SelectedItem.Value));
                                //    oIPAddresses.AddDetail(intID, intDetail);
                                //    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "wkst");
                                //}
                                //else
                                //    boolDelete = true;
                            }
                            intPhysicalWindows = 1;
                        }
                        if (intBladesHP == 1)
                        {
                            int intBladeHP = Int32.Parse(ddlBladeHpVlan.SelectedItem.Text);
                            if (boolEcom == false)
                            {
                                intID = oIPAddresses.Get_Blade_HP(intClass, intEnv, intAddress, 0, 0, intBladeHP, intNetwork, 0, true, 0, -1, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", intBladeHP, "", txtServer.Text, intClass, intEnv, intAddress, Int32.Parse(ddlSendingModelProperty.SelectedItem.Value), txtSendingRoom.Text, txtSendingRack.Text, txtSendingEnclosure.Text, txtSendingSlot.Text, 0, Int32.Parse(ddlType.SelectedItem.Value));
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub");
                                    UpdateIPAM(txtServer.Text, intID, IPAddressType.Primary);
                                }
                                else
                                    boolDelete = true;
                            }
                            else
                            {
                                intID = oIPAddresses.Get_Blade_HP(intClass, intEnv, intAddress, 1, 0, intBladeHP, intNetwork, 0, true, 0, -1, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", intBladeHP, "", txtServer.Text, intClass, intEnv, intAddress, Int32.Parse(ddlSendingModelProperty.SelectedItem.Value), txtSendingRoom.Text, txtSendingRack.Text, txtSendingEnclosure.Text, txtSendingSlot.Text, 0, Int32.Parse(ddlType.SelectedItem.Value));
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "prod");
                                    UpdateIPAM(txtServer.Text, intID, IPAddressType.EcomProd);
                                }
                                else
                                    boolDelete = true;
                                intID = oIPAddresses.Get_Blade_HP(intClass, intEnv, intAddress, 0, 1, intBladeHP, intNetwork, 0, true, 0, -1, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", intBladeHP, "", txtServer.Text, intClass, intEnv, intAddress, Int32.Parse(ddlSendingModelProperty.SelectedItem.Value), txtSendingRoom.Text, txtSendingRack.Text, txtSendingEnclosure.Text, txtSendingSlot.Text, 0, Int32.Parse(ddlType.SelectedItem.Value));
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "serv");
                                    UpdateIPAM(txtServer.Text, intID, IPAddressType.EcomService);
                                }
                                else
                                    boolDelete = true;
                            }
                        }
                        else if (intBladesSUN == 1)
                        {
                            int intBladeSUN = Int32.Parse(ddlBladeSunVlan.SelectedItem.Text);
                            if (boolEcom == false)
                            {
                                intID = oIPAddresses.Get_Blade_SUN(intClass, intEnv, intAddress, 0, 0, intBladeSUN, 0, intNetwork, 0, true, 0, -1, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", intBladeSUN, "", txtServer.Text, intClass, intEnv, intAddress, Int32.Parse(ddlSendingModelProperty.SelectedItem.Value), txtSendingRoom.Text, txtSendingRack.Text, txtSendingEnclosure.Text, txtSendingSlot.Text, 0, Int32.Parse(ddlType.SelectedItem.Value));
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub");
                                    UpdateIPAM(txtServer.Text, intID, IPAddressType.Primary);
                                }
                                else
                                    boolDelete = true;
                            }
                            else
                            {
                                intID = oIPAddresses.Get_Blade_SUN(intClass, intEnv, intAddress, 1, 0, intBladeSUN, 0, intNetwork, 0, true, 0, -1, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", intBladeSUN, "", txtServer.Text, intClass, intEnv, intAddress, Int32.Parse(ddlSendingModelProperty.SelectedItem.Value), txtSendingRoom.Text, txtSendingRack.Text, txtSendingEnclosure.Text, txtSendingSlot.Text, 0, Int32.Parse(ddlType.SelectedItem.Value));
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "prod");
                                    UpdateIPAM(txtServer.Text, intID, IPAddressType.EcomProd);
                                }
                                else
                                    boolDelete = true;
                                intID = oIPAddresses.Get_Blade_SUN(intClass, intEnv, intAddress, 0, 1, intBladeSUN, 0, intNetwork, 0, true, 0, -1, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", intBladeSUN, "", txtServer.Text, intClass, intEnv, intAddress, Int32.Parse(ddlSendingModelProperty.SelectedItem.Value), txtSendingRoom.Text, txtSendingRack.Text, txtSendingEnclosure.Text, txtSendingSlot.Text, 0, Int32.Parse(ddlType.SelectedItem.Value));
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "serv");
                                    UpdateIPAM(txtServer.Text, intID, IPAddressType.EcomService);
                                }
                                else
                                    boolDelete = true;
                            }
                        }
                        else
                        {
                            if (boolEcom == false)
                            {
                                intID = oIPAddresses.Get_(intClass, intEnv, intAddress, intPhysicalWindows, intPhysicalUNIX, 0, 0, intIPX, 0, 0, 0, 0, intVMwareHost, 0, intVMwareWindows, intVMwareLinux, intAPV, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, 0, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", 0, "", txtServer.Text, intClass, intEnv, intAddress, Int32.Parse(ddlSendingModelProperty.SelectedItem.Value), txtSendingRoom.Text, txtSendingRack.Text, txtSendingEnclosure.Text, txtSendingSlot.Text, 0, Int32.Parse(ddlType.SelectedItem.Value));
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub");
                                    UpdateIPAM(txtServer.Text, intID, IPAddressType.Primary);
                                }
                                else
                                    boolDelete = true;
                            }
                            else
                            {
                                intID = oIPAddresses.Get_(intClass, intEnv, intAddress, intPhysicalWindows, intPhysicalUNIX, 1, 0, intIPX, 0, 0, 0, 0, intVMwareHost, 0, intVMwareWindows, intVMwareLinux, intAPV, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, 0, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", 0, "", txtServer.Text, intClass, intEnv, intAddress, Int32.Parse(ddlSendingModelProperty.SelectedItem.Value), txtSendingRoom.Text, txtSendingRack.Text, txtSendingEnclosure.Text, txtSendingSlot.Text, 0, Int32.Parse(ddlType.SelectedItem.Value));
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "prod");
                                    UpdateIPAM(txtServer.Text, intID, IPAddressType.EcomProd);
                                }
                                else
                                    boolDelete = true;
                                intID = oIPAddresses.Get_(intClass, intEnv, intAddress, intPhysicalWindows, intPhysicalUNIX, 0, 1, intIPX, 0, 0, 0, 0, intVMwareHost, 0, intVMwareWindows, intVMwareLinux, intAPV, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, 0, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", 0, "", txtServer.Text, intClass, intEnv, intAddress, Int32.Parse(ddlSendingModelProperty.SelectedItem.Value), txtSendingRoom.Text, txtSendingRack.Text, txtSendingEnclosure.Text, txtSendingSlot.Text, 0, Int32.Parse(ddlType.SelectedItem.Value));
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "serv");
                                    UpdateIPAM(txtServer.Text, intID, IPAddressType.EcomService);
                                }
                                else
                                    boolDelete = true;
                            }
                        }
                    }
                    if (radCSM.Checked == true)
                    {
                        int intServer = Int32.Parse(ddlCSMVlan.SelectedItem.Text);
                        if (boolEcom == false)
                        {
                            intID = oIPAddresses.Get_VLAN(intClass, intEnv, intAddress, 0, 0, intServer, intNetwork, 1, 0, 0, 0, 0, 0, true, 0, 0, 0, -1, intEnvironment, dsnServiceEditor);
                            if (intID > 0)
                            {
                                intDetail = oIPAddresses.AddDetails("", 0, "", intServer, "", txtServer.Text, intClass, intEnv, intAddress, Int32.Parse(ddlSendingModelProperty.SelectedItem.Value), txtSendingRoom.Text, txtSendingRack.Text, txtSendingEnclosure.Text, txtSendingSlot.Text, 0, -1);
                                oIPAddresses.AddDetail(intID, intDetail);
                                AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub");
                                UpdateIPAM(txtServer.Text, intID, IPAddressType.Primary);
                            }
                            else
                                boolDelete = true;
                        }
                        else
                        {
                            intID = oIPAddresses.Get_VLAN(intClass, intEnv, intAddress, 1, 0, intServer, intNetwork, 1, 0, 0, 0, 0, 0, true, 0, 0, 0, -1, intEnvironment, dsnServiceEditor);
                            if (intID > 0)
                            {
                                intDetail = oIPAddresses.AddDetails("", 0, "", intServer, "", txtServer.Text, intClass, intEnv, intAddress, Int32.Parse(ddlSendingModelProperty.SelectedItem.Value), txtSendingRoom.Text, txtSendingRack.Text, txtSendingEnclosure.Text, txtSendingSlot.Text, 0, -1);
                                oIPAddresses.AddDetail(intID, intDetail);
                                AddRelation(intClass, intEnv, intAddress, intID, intDetail, "prod");
                                UpdateIPAM(txtServer.Text, intID, IPAddressType.EcomProd);
                            }
                            else
                                boolDelete = true;
                            intID = oIPAddresses.Get_VLAN(intClass, intEnv, intAddress, 0, 1, intServer, intNetwork, 1, 0, 0, 0, 0, 0, true, 0, 0, 0, -1, intEnvironment, dsnServiceEditor);
                            if (intID > 0)
                            {
                                intDetail = oIPAddresses.AddDetails("", 0, "", intServer, "", txtServer.Text, intClass, intEnv, intAddress, Int32.Parse(ddlSendingModelProperty.SelectedItem.Value), txtSendingRoom.Text, txtSendingRack.Text, txtSendingEnclosure.Text, txtSendingSlot.Text, 0, -1);
                                oIPAddresses.AddDetail(intID, intDetail);
                                AddRelation(intClass, intEnv, intAddress, intID, intDetail, "serv");
                                UpdateIPAM(txtServer.Text, intID, IPAddressType.EcomService);
                            }
                            else
                                boolDelete = true;
                        }
                    }
                    if (radLTM.Checked == true)
                    {
                        int intServer = Int32.Parse(ddlLTMVlan.SelectedItem.Text);
                        if (boolEcom == false)
                        {
                            intID = oIPAddresses.Get_VLAN(intClass, intEnv, intAddress, 0, 0, intServer, intNetwork, 0, 0, 1, 0, 0, 0, true, 0, 0, 0, -1, intEnvironment, dsnServiceEditor);
                            if (intID > 0)
                            {
                                intDetail = oIPAddresses.AddDetails("", 0, "", intServer, "", txtServer.Text, intClass, intEnv, intAddress, Int32.Parse(ddlSendingModelProperty.SelectedItem.Value), txtSendingRoom.Text, txtSendingRack.Text, txtSendingEnclosure.Text, txtSendingSlot.Text, 0, -2);
                                oIPAddresses.AddDetail(intID, intDetail);
                                AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub");
                                UpdateIPAM(txtServer.Text, intID, IPAddressType.Primary);
                            }
                            else
                                boolDelete = true;
                        }
                        else
                        {
                            intID = oIPAddresses.Get_VLAN(intClass, intEnv, intAddress, 1, 0, intServer, intNetwork, 0, 0, 1, 0, 0, 0, true, 0, 0, 0, -1, intEnvironment, dsnServiceEditor);
                            if (intID > 0)
                            {
                                intDetail = oIPAddresses.AddDetails("", 0, "", intServer, "", txtServer.Text, intClass, intEnv, intAddress, Int32.Parse(ddlSendingModelProperty.SelectedItem.Value), txtSendingRoom.Text, txtSendingRack.Text, txtSendingEnclosure.Text, txtSendingSlot.Text, 0, -2);
                                oIPAddresses.AddDetail(intID, intDetail);
                                AddRelation(intClass, intEnv, intAddress, intID, intDetail, "prod");
                                UpdateIPAM(txtServer.Text, intID, IPAddressType.EcomProd);
                            }
                            else
                                boolDelete = true;
                            intID = oIPAddresses.Get_VLAN(intClass, intEnv, intAddress, 0, 1, intServer, intNetwork, 0, 0, 1, 0, 0, 0, true, 0, 0, 0, -1, intEnvironment, dsnServiceEditor);
                            if (intID > 0)
                            {
                                intDetail = oIPAddresses.AddDetails("", 0, "", intServer, "", txtServer.Text, intClass, intEnv, intAddress, Int32.Parse(ddlSendingModelProperty.SelectedItem.Value), txtSendingRoom.Text, txtSendingRack.Text, txtSendingEnclosure.Text, txtSendingSlot.Text, 0, -2);
                                oIPAddresses.AddDetail(intID, intDetail);
                                AddRelation(intClass, intEnv, intAddress, intID, intDetail, "serv");
                                UpdateIPAM(txtServer.Text, intID, IPAddressType.EcomService);
                            }
                            else
                                boolDelete = true;
                        }
                    }
                }
                else
                {
                    int intModel = Int32.Parse(Request.Form[hdnModel.UniqueID]);
                    int intCount = 0;
                    string strHidden = Request.Form[hdnNodes.UniqueID];
                    while (strHidden != "")
                    {
                        intCount++;
                        strHidden = strHidden.Substring(strHidden.IndexOf("&") + 1);
                    }
                    if (radClusterWindows.Checked == true)
                    {
                        int intVlanPrivate1 = 0;
                        int intNetworkPrivate1 = oIPAddresses.Get_ClusterNetwork(intClass, intEnv, intAddress, 0, 0, 1, 0, intCount, 0, true, dsnServiceEditor);
                        if (intNetworkPrivate1 > 0)
                        {
                            intVlanPrivate1 = Int32.Parse(oIPAddresses.GetNetwork(intNetworkPrivate1, "vlanid"));
                            if (intVlanPrivate1 > 0)
                                intVlanPrivate1 = Int32.Parse(oIPAddresses.GetVlan(intVlanPrivate1, "vlan"));
                        }
                        if (ddlHardware.SelectedItem.Value == "2")
                        {
                            // HP Blade
                            int intBladeHP = Int32.Parse(ddlClusterBladeSun.SelectedItem.Text);
                            strHidden = Request.Form[hdnNodes.UniqueID];
                            while (strHidden != "")
                            {
                                intCount++;
                                string strHiddenTemp = strHidden.Substring(0, strHidden.IndexOf("&"));
                                strHidden = strHidden.Substring(strHidden.IndexOf("&") + 1);
                                string strNode = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                                strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                                string strRoom = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                                strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                                string strRack = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                                strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                                string strEnclosure = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                                strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                                string strSlot = strHiddenTemp;
                                // Assign 1 public for each node (based on Blade VLAN)
                                if (boolEcom == false)
                                {
                                    intID = oIPAddresses.Get_Blade_HP(intClass, intEnv, intAddress, 0, 0, intBladeHP, intNetwork, 0, true, 0, -1, intEnvironment, dsnServiceEditor);
                                    if (intID > 0)
                                    {
                                        intDetail = oIPAddresses.AddDetails("", 0, "", intBladeHP, "", strNode, intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -101);
                                        oIPAddresses.AddDetail(intID, intDetail);
                                        AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub1");
                                        UpdateIPAM(strNode, intID, IPAddressType.Primary);
                                    }
                                    else
                                        boolDelete = true;
                                }
                                else
                                {
                                    intID = oIPAddresses.Get_Blade_HP(intClass, intEnv, intAddress, 1, 0, intBladeHP, intNetwork, 0, true, 0, -1, intEnvironment, dsnServiceEditor);
                                    if (intID > 0)
                                    {
                                        intDetail = oIPAddresses.AddDetails("", 0, "", intBladeHP, "", strNode, intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -101);
                                        oIPAddresses.AddDetail(intID, intDetail);
                                        AddRelation(intClass, intEnv, intAddress, intID, intDetail, "prod1");
                                        UpdateIPAM(strNode, intID, IPAddressType.EcomProd);
                                    }
                                    else
                                        boolDelete = true;
                                    intID = oIPAddresses.Get_Blade_HP(intClass, intEnv, intAddress, 0, 1, intBladeHP, intNetwork, 0, true, 0, -1, intEnvironment, dsnServiceEditor);
                                    if (intID > 0)
                                    {
                                        intDetail = oIPAddresses.AddDetails("", 0, "", intBladeHP, "", strNode, intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -101);
                                        oIPAddresses.AddDetail(intID, intDetail);
                                        AddRelation(intClass, intEnv, intAddress, intID, intDetail, "serv1");
                                        UpdateIPAM(strNode, intID, IPAddressType.EcomService);
                                    }
                                    else
                                        boolDelete = true;
                                }
                                // Assign 1 private for each node (based on Windows Cluster)
                                intID = oIPAddresses.Get_Cluster(intClass, intEnv, intAddress, 0, 0, 1, 0, intNetworkPrivate1, true, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", intVlanPrivate1, "", strNode, intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -102);
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "priv1");
                                    UpdateIPAM(strNode, intID, IPAddressType.ClusterNode);
                                }
                                else
                                    boolDelete = true;
                            }
                        }
                        else if (ddlHardware.SelectedItem.Value == "3")
                        {
                            // SUN Blade
                            int intBladeSUN = Int32.Parse(ddlClusterBladeSun.SelectedItem.Text);
                            strHidden = Request.Form[hdnNodes.UniqueID];
                            while (strHidden != "")
                            {
                                intCount++;
                                string strHiddenTemp = strHidden.Substring(0, strHidden.IndexOf("&"));
                                strHidden = strHidden.Substring(strHidden.IndexOf("&") + 1);
                                string strNode = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                                strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                                string strRoom = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                                strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                                string strRack = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                                strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                                string strEnclosure = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                                strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                                string strSlot = strHiddenTemp;
                                // Assign 1 public for each node (based on Blade VLAN)
                                if (boolEcom == false)
                                {
                                    intID = oIPAddresses.Get_Blade_SUN(intClass, intEnv, intAddress, 0, 0, intBladeSUN, 0, intNetwork, 0, true, 0, -1, intEnvironment, dsnServiceEditor);
                                    if (intID > 0)
                                    {
                                        intDetail = oIPAddresses.AddDetails("", 0, "", intBladeSUN, "", strNode, intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -101);
                                        oIPAddresses.AddDetail(intID, intDetail);
                                        AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub1");
                                        UpdateIPAM(strNode, intID, IPAddressType.Primary);
                                    }
                                    else
                                        boolDelete = true;
                                }
                                else
                                {
                                    intID = oIPAddresses.Get_Blade_SUN(intClass, intEnv, intAddress, 1, 0, intBladeSUN, 0, intNetwork, 0, true, 0, -1, intEnvironment, dsnServiceEditor);
                                    if (intID > 0)
                                    {
                                        intDetail = oIPAddresses.AddDetails("", 0, "", intBladeSUN, "", strNode, intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -101);
                                        oIPAddresses.AddDetail(intID, intDetail);
                                        AddRelation(intClass, intEnv, intAddress, intID, intDetail, "prod1");
                                        UpdateIPAM(strNode, intID, IPAddressType.EcomProd);
                                    }
                                    else
                                        boolDelete = true;
                                    intID = oIPAddresses.Get_Blade_SUN(intClass, intEnv, intAddress, 0, 1, intBladeSUN, 0, intNetwork, 0, true, 0, -1, intEnvironment, dsnServiceEditor);
                                    if (intID > 0)
                                    {
                                        intDetail = oIPAddresses.AddDetails("", 0, "", intBladeSUN, "", strNode, intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -101);
                                        oIPAddresses.AddDetail(intID, intDetail);
                                        AddRelation(intClass, intEnv, intAddress, intID, intDetail, "serv1");
                                        UpdateIPAM(strNode, intID, IPAddressType.EcomService);
                                    }
                                    else
                                        boolDelete = true;
                                }
                                // Assign 1 private for each node (based on Windows Cluster)
                                intID = oIPAddresses.Get_Cluster(intClass, intEnv, intAddress, 0, 0, 1, 0, intNetworkPrivate1, true, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", intVlanPrivate1, "", strNode, intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -102);
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "priv1");
                                    UpdateIPAM(strNode, intID, IPAddressType.ClusterNode);
                                }
                                else
                                    boolDelete = true;
                            }
                        }
                        else
                        {
                            // Physical Windows
                            strHidden = Request.Form[hdnNodes.UniqueID];
                            while (strHidden != "")
                            {
                                intCount++;
                                string strHiddenTemp = strHidden.Substring(0, strHidden.IndexOf("&"));
                                strHidden = strHidden.Substring(strHidden.IndexOf("&") + 1);
                                string strNode = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                                strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                                string strRoom = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                                strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                                string strRack = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                                strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                                string strEnclosure = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                                strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                                string strSlot = strHiddenTemp;
                                // Assign 1 public for each node (based on Physical Windows)
                                if (boolEcom == false)
                                {
                                    intID = oIPAddresses.Get_(intClass, intEnv, intAddress, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, 0, intEnvironment, dsnServiceEditor);
                                    if (intID > 0)
                                    {
                                        intDetail = oIPAddresses.AddDetails("", 0, "", 0, "", strNode + " (Public)", intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -201);
                                        oIPAddresses.AddDetail(intID, intDetail);
                                        AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub1");
                                        UpdateIPAM(strNode, intID, IPAddressType.Primary);
                                    }
                                    else
                                        boolDelete = true;
                                }
                                else
                                {
                                    intID = oIPAddresses.Get_(intClass, intEnv, intAddress, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, 0, intEnvironment, dsnServiceEditor);
                                    if (intID > 0)
                                    {
                                        intDetail = oIPAddresses.AddDetails("", 0, "", 0, "", strNode + " (Production)", intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -201);
                                        oIPAddresses.AddDetail(intID, intDetail);
                                        AddRelation(intClass, intEnv, intAddress, intID, intDetail, "prod1");
                                        UpdateIPAM(strNode, intID, IPAddressType.EcomProd);
                                    }
                                    else
                                        boolDelete = true;
                                    intID = oIPAddresses.Get_(intClass, intEnv, intAddress, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, 0, intEnvironment, dsnServiceEditor);
                                    if (intID > 0)
                                    {
                                        intDetail = oIPAddresses.AddDetails("", 0, "", 0, "", strNode + " (Service)", intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -201);
                                        oIPAddresses.AddDetail(intID, intDetail);
                                        AddRelation(intClass, intEnv, intAddress, intID, intDetail, "serv1");
                                        UpdateIPAM(strNode, intID, IPAddressType.EcomService);
                                    }
                                    else
                                        boolDelete = true;
                                }
                                // Assign 1 private for each node (based on Windows Cluster)
                                intID = oIPAddresses.Get_Cluster(intClass, intEnv, intAddress, 0, 0, 1, 0, intNetworkPrivate1, true, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", intVlanPrivate1, "", strNode + " (Private 1)", intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -202);
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "priv1");
                                    UpdateIPAM(strNode, intID, IPAddressType.ClusterNode);
                                }
                                else
                                    boolDelete = true;
                            }
                        }
                        if (oIPAddresses.GetNetwork(intNetworkPrivate1, "cluster_inuse") != "1")
                            intResetNetwork1 = intNetworkPrivate1;
                        oIPAddresses.UpdateNetwork(intNetworkPrivate1, 1);
                    }
                    else if (radClusterUnixRAC.Checked == true)
                    {
                        int intVlanPrivate1 = 0;
                        int intVlanPrivate2 = 0;
                        int intNetworkPrivate1 = oIPAddresses.Get_ClusterNetwork(intClass, intEnv, intAddress, 0, 0, 0, 1, intCount, 0, true, dsnServiceEditor);
                        if (intNetworkPrivate1 > 0)
                        {
                            intVlanPrivate1 = Int32.Parse(oIPAddresses.GetNetwork(intNetworkPrivate1, "vlanid"));
                            if (intVlanPrivate1 > 0)
                                intVlanPrivate1 = Int32.Parse(oIPAddresses.GetVlan(intVlanPrivate1, "vlan"));
                        }
                        int intNetworkPrivate2 = oIPAddresses.Get_ClusterNetwork(intClass, intEnv, intAddress, 0, 0, 0, 1, intCount, intVlanPrivate1, true, dsnServiceEditor);
                        if (intNetworkPrivate2 > 0)
                        {
                            intVlanPrivate2 = Int32.Parse(oIPAddresses.GetNetwork(intNetworkPrivate2, "vlanid"));
                            if (intVlanPrivate2 > 0)
                                intVlanPrivate2 = Int32.Parse(oIPAddresses.GetVlan(intVlanPrivate2, "vlan"));
                        }
                        strHidden = Request.Form[hdnNodes.UniqueID];
                        while (strHidden != "")
                        {
                            intCount++;
                            string strHiddenTemp = strHidden.Substring(0, strHidden.IndexOf("&"));
                            strHidden = strHidden.Substring(strHidden.IndexOf("&") + 1);
                            string strNode = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                            strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                            string strRoom = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                            strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                            string strRack = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                            strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                            string strEnclosure = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                            strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                            string strSlot = strHiddenTemp;
                            // Assign 1 public for each node (based on Physical UNIX)
                            if (boolEcom == false)
                            {
                                intID = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, 0, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", 0, "", strNode + " (Public)", intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -301);
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub1");
                                    UpdateIPAM(strNode, intID, IPAddressType.Primary);
                                }
                                else
                                    boolDelete = true;
                            }
                            else
                            {
                                intID = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, 0, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", 0, "", strNode + " (Production)", intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -301);
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "prod1");
                                    UpdateIPAM(strNode, intID, IPAddressType.EcomProd);
                                }
                                else
                                    boolDelete = true;
                                intID = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, 0, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", 0, "", strNode + " (Service)", intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -301);
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "serv1");
                                    UpdateIPAM(strNode, intID, IPAddressType.EcomService);
                                }
                                else
                                    boolDelete = true;
                            }
                            // Assign 2 private for each node (based on UNIX Cluster)
                            intID = oIPAddresses.Get_Cluster(intClass, intEnv, intAddress, 0, 0, 0, 1, intNetworkPrivate1, true, intEnvironment, dsnServiceEditor);
                            if (intID > 0)
                            {
                                intDetail = oIPAddresses.AddDetails("", 0, "", intVlanPrivate1, "", strNode + " (Private 1)", intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -302);
                                oIPAddresses.AddDetail(intID, intDetail);
                                AddRelation(intClass, intEnv, intAddress, intID, intDetail, "priv1");
                                UpdateIPAM(strNode, intID, IPAddressType.ClusterNode);
                            }
                            else
                                boolDelete = true;
                            intID = oIPAddresses.Get_Cluster(intClass, intEnv, intAddress, 0, 0, 0, 1, intNetworkPrivate2, true, intEnvironment, dsnServiceEditor);
                            if (intID > 0)
                            {
                                intDetail = oIPAddresses.AddDetails("", 0, "", intVlanPrivate2, "", strNode + " (Private 2)", intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -303);
                                oIPAddresses.AddDetail(intID, intDetail);
                                AddRelation(intClass, intEnv, intAddress, intID, intDetail, "priv2");
                                UpdateIPAM(strNode, intID, IPAddressType.ClusterNode);
                            }
                            else
                                boolDelete = true;
                        }
                        if (oIPAddresses.GetNetwork(intNetworkPrivate1, "cluster_inuse") != "1")
                            intResetNetwork1 = intNetworkPrivate1;
                        oIPAddresses.UpdateNetwork(intNetworkPrivate1, 1);
                        if (oIPAddresses.GetNetwork(intNetworkPrivate2, "cluster_inuse") != "1")
                            intResetNetwork2 = intNetworkPrivate2;
                        oIPAddresses.UpdateNetwork(intNetworkPrivate2, 1);
                    }
                    else if (radClusterUnixOther.Checked == true || radClusterVCS.Checked == true)
                    {
                        strHidden = Request.Form[hdnNodes.UniqueID];
                        while (strHidden != "")
                        {
                            intCount++;
                            string strHiddenTemp = strHidden.Substring(0, strHidden.IndexOf("&"));
                            strHidden = strHidden.Substring(strHidden.IndexOf("&") + 1);
                            string strNode = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                            strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                            string strRoom = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                            strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                            string strRack = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                            strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                            string strEnclosure = strHiddenTemp.Substring(0, strHiddenTemp.IndexOf("_"));
                            strHiddenTemp = strHiddenTemp.Substring(strHiddenTemp.IndexOf("_") + 1);
                            string strSlot = strHiddenTemp;
                            // Assign 1 public for each node (based on Physical UNIX)
                            if (boolEcom == false)
                            {
                                intID = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, 0, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", 0, "", strNode + " (Public)", intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -102);
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub1");
                                    UpdateIPAM(strNode, intID, IPAddressType.Primary);
                                }
                                else
                                    boolDelete = true;
                            }
                            else
                            {
                                intID = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, 0, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", 0, "", strNode + " (Production)", intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -102);
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "prod1");
                                    UpdateIPAM(strNode, intID, IPAddressType.EcomProd);
                                }
                                else
                                    boolDelete = true;
                                intID = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, 0, intEnvironment, dsnServiceEditor);
                                if (intID > 0)
                                {
                                    intDetail = oIPAddresses.AddDetails("", 0, "", 0, "", strNode + " (Service)", intClass, intEnv, intAddress, intModel, strRoom, strRack, strEnclosure, strSlot, 0, -102);
                                    oIPAddresses.AddDetail(intID, intDetail);
                                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "serv1");
                                    UpdateIPAM(strNode, intID, IPAddressType.EcomService);
                                }
                                else
                                    boolDelete = true;
                            }
                        }
                    }
                }
            }
            else if (radAdditional.Checked == true)
            {
                int intAdditional = Int32.Parse(ddlAdditionalVlan.SelectedItem.Text);
                intID = oIPAddresses.Get_VLANExcludeNoRoute(intClass, intEnv, intAddress, 0, 0, intAdditional, intNetwork, 0, 0, 0, 0, 0, false, intEnvironment, dsnServiceEditor);
                if (intID > 0)
                {
                    intDetail = oIPAddresses.AddDetails("", 0, "", intAdditional, "", txtAdditional.Text, intClass, intEnv, intAddress, Int32.Parse(ddlSendingModelProperty.SelectedItem.Value), txtSendingRoom.Text, txtSendingRack.Text, txtSendingEnclosure.Text, txtSendingSlot.Text, 0, 4);
                    oIPAddresses.AddDetail(intID, intDetail);
                    AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub");
                    UpdateIPAM(txtAdditional.Text, intID, IPAddressType.Primary);
                }
                else
                    boolDelete = true;
            }
            Redirect();
        }
        protected void btnDuplicate_Click(Object Sender, EventArgs e)
        {
            //    int intID = -1;
            //    int intPrevious = Int32.Parse(Request.QueryString["id"]);
            //    int intDetail = 0;
            //    DataSet ds = oIPAddresses.GetDetail(intPrevious);
            //    string strURL = ds.Tables[0].Rows[0]["url"].ToString();
            //    int intProject = Int32.Parse(ds.Tables[0].Rows[0]["projectid"].ToString());
            //    string strInstance = ds.Tables[0].Rows[0]["instance"].ToString();
            //    int intVLAN = Int32.Parse(ds.Tables[0].Rows[0]["vlan"].ToString());
            //    string strSerial = ds.Tables[0].Rows[0]["serial"].ToString();
            //    string strServername = ds.Tables[0].Rows[0]["server_name"].ToString();
            //    int intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
            //    int intEnv = Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString());
            //    int intAddress = Int32.Parse(ds.Tables[0].Rows[0]["addressid"].ToString());
            //    int intCSM = Int32.Parse(ds.Tables[0].Rows[0]["csm"].ToString());
            //    int intType = Int32.Parse(ds.Tables[0].Rows[0]["type"].ToString());
            //    int intVMwareWindows = 0;
            //    int intVMwareLinux = 0;
            //    int intVMwareHost = 0;
            //    int intAPV = 0;
            //    int intBlades = 0;
            //    int intPhysicalWindows = 0;
            //    int intPhysicalUNIX = 0;
            //    int intIPX = 0;
            //    if (intType == -1)
            //    {
            //        // CSM
            //        intID = oIPAddresses.Get_VLAN(intClass, intEnv, intAddress, intVLAN, 0, 1, 0, 0, 0, 0, 0, 0);
            //        if (intID > 0)
            //        {
            //            intDetail = oIPAddresses.AddDetails("", 0, "", intVLAN, "", txtServer.Text, intClass, intEnv, intAddress, 0, -1);
            //            oIPAddresses.AddDetail(intID, intDetail);
            //            AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub");
            //        }
            //    }
            //    else if (intType == -2)
            //    {
            //        // LTM
            //        intID = oIPAddresses.Get_VLAN(intClass, intEnv, intAddress, intVLAN, 0, 0, 0, 0, 1, 0, 0, 0);
            //        if (intID > 0)
            //        {
            //            intDetail = oIPAddresses.AddDetails("", 0, "", intVLAN, "", txtServer.Text, intClass, intEnv, intAddress, 0, -2);
            //            oIPAddresses.AddDetail(intID, intDetail);
            //            AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub");
            //        }
            //    }
            //    else
            //    {
            //        if (intType == 1)
            //            intVMwareWindows = 1;
            //        else if (intType == 2)
            //            intVMwareLinux = 1;
            //        else if (intType == 3)
            //            intVMwareHost = 1;
            //        else if (intType == 4)
            //            intAPV = 1;
            //        else if (intType == 5)
            //            intBlades = 1;
            //        else if (intType == 6)
            //            intPhysicalWindows = 1;
            //        else if (intType == 7)
            //            intPhysicalUNIX = 1;
            //        else if (intType == 8)
            //            intIPX = 1;
            //        else if (intType == 9)
            //        {
            //            intID = oIPAddresses.Get_(intClass, intEnv, intAddress, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            //            if (intID > 0)
            //            {
            //                intDetail = oIPAddresses.AddDetails("", 0, "", 0, "", txtServer.Text, intClass, intEnv, intAddress, 0, intType);
            //                oIPAddresses.AddDetail(intID, intDetail);
            //                AddRelation(intClass, intEnv, intAddress, intID, intDetail, "wkst");
            //            }
            //            intPhysicalWindows = 1;
            //        }
            //        if (intBlades == 0)
            //        {
            //            intID = oIPAddresses.Get_(intClass, intEnv, intAddress, intPhysicalWindows, intPhysicalUNIX, intIPX, 0, intVMwareHost, intVMwareWindows, intVMwareLinux, intAPV, 0, 0, 0, 0, 0, 0);
            //            if (intID > 0)
            //            {
            //                intDetail = oIPAddresses.AddDetails("", 0, "", 0, "", txtServer.Text, intClass, intEnv, intAddress, 0, intType);
            //                oIPAddresses.AddDetail(intID, intDetail);
            //                AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub");
            //            }
            //        }
            //        else
            //        {
            //            intID = oIPAddresses.Get_Blade(intClass, intEnv, intAddress, intVLAN);
            //            if (intID > 0)
            //            {
            //                intDetail = oIPAddresses.AddDetails("", 0, "", intVLAN, "", txtServer.Text, intClass, intEnv, intAddress, 0, Int32.Parse(ddlType.SelectedItem.Value));
            //                oIPAddresses.AddDetail(intID, intDetail);
            //                AddRelation(intClass, intEnv, intAddress, intID, intDetail, "pub");
            //            }
            //        }
            //    }
            //    Redirect();
        }
        private void AddRelation(int _classid, int _environmentid, int _addressid, int intID, int intDetail, string _prefix)
        {
            intIPArray[intArrayCount] = intID;
            intArrayCount++;
            oIPAddresses.Update(intID, intProfile);
            oIPAddresses.UpdateAvailable(intID, 0);
            int intNetwork = Int32.Parse(oIPAddresses.Get(intID, "networkid"));
            strRedirect += (strRedirect == "" ? "?" : "&") + _prefix + "=" + intID.ToString();
            //DataSet ds = oIPAddresses.GetNetworkRelations(intNetwork);
            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    intID = oIPAddresses.Get_Related(_classid, _environmentid, _addressid, Int32.Parse(dr["id"].ToString()));
            //    oIPAddresses.AddDetail(intID, intDetail);
            //    strRedirect += (strRedirect == "" ? "?" : "&") + _prefix + "=" + intID.ToString();
            //    oIPAddresses.Update(intID, intProfile);
            //    oIPAddresses.UpdateAvailable(intID, 0);
            //}
        }
        protected void Redirect()
        {
            if (strRedirect == "" || boolDelete == true)
            {
                if (strRedirect == "")
                    panFailed.Visible = true;
                else
                {
                    panDelete.Visible = true;
                    for (int ii = 0; ii <= intArrayCount; ii++)
                        oIPAddresses.Delete(intIPArray[ii]);
                    oIPAddresses.UpdateNetwork(intResetNetwork1, 0);
                    oIPAddresses.UpdateNetwork(intResetNetwork2, 0);
                    hdnError.Value = strRedirect;
                }
                if (radCSMVip.Checked == true)
                    divCSMVip.Style["display"] = "inline";
                else if (radCluster.Checked == true)
                    divCluster.Style["display"] = "inline";
                else if (radILO.Checked == true)
                    divILO.Style["display"] = "inline";
                else if (radServer.Checked == true)
                {
                    divServer.Style["display"] = "inline";
                    if (radSingleServer.Checked == true)
                    {
                        divSingleServer.Style["display"] = "inline";
                        if (radCSM.Checked == true)
                        {
                            divCSM.Style["display"] = "inline";
                            divCSM2.Style["display"] = "inline";
                        }
                        if (radLTM.Checked == true)
                        {
                            divLTM.Style["display"] = "inline";
                            divLTM2.Style["display"] = "inline";
                        }
                        if (radNone.Checked == true)
                        {
                            divCSMNo.Style["display"] = "inline";
                            if (ddlType.SelectedItem.Text == "HP Blade")
                            {
                                divBladeHpVlan.Style["display"] = "inline";
                                divBladeHpVlan2.Style["display"] = "inline";
                            }
                            if (ddlType.SelectedItem.Text == "SUN Blade")
                            {
                                divBladeSunVlan.Style["display"] = "inline";
                                divBladeSunVlan2.Style["display"] = "inline";
                            }
                        }
                    }
                    if (radClusterWindows.Checked == true)
                    {
                        divClusterSystem.Style["display"] = "inline";
                        divClusterHardware.Style["display"] = "inline";
                        if (ddlHardware.SelectedItem.Text == "HP Blade")
                        {
                            divClusterBladeHp.Style["display"] = "inline";
                            divClusterBladeHp2.Style["display"] = "inline";
                        }
                        if (ddlHardware.SelectedItem.Text == "SUN Blade")
                        {
                            divClusterBladeSun.Style["display"] = "inline";
                            divClusterBladeSun2.Style["display"] = "inline";
                        }
                    }
                    if (radClusterUnixRAC.Checked == true)
                        divClusterSystem.Style["display"] = "inline";
                    if (radClusterUnixOther.Checked == true)
                        divClusterSystem.Style["display"] = "inline";
                }
                else if (radAdditional.Checked == true)
                    divAdditional.Style["display"] = "inline";
            }
            else
                Response.Redirect(oPage.GetFullLink(intPage) + strRedirect);
        }
        private void UpdateIPAM(string _name, int _ipaddressid, IPAddressType _type)
        {
            int intAsset = 0;
            int intServer = 0;
            Asset oAsset = new Asset(0, dsnAsset, dsn);
            Servers oServer = new Servers(0, dsn);
            string strServer = _name;

            if (_name.ToUpper().Contains("-BACKUP") == true)
            {
                strServer = _name.Substring(0, _name.ToUpper().IndexOf("-BACKUP"));
                _type = IPAddressType.Backup;
            }

            if (_type == IPAddressType.ILO)
            {
                DataSet dsAsset = oAsset.Get(txtSerial.Text);
                if (dsAsset.Tables[0].Rows.Count > 0)
                    Int32.TryParse(dsAsset.Tables[0].Rows[0]["id"].ToString(), out intAsset);
            }
            else
            {
                DataSet dsServer = oServer.Get(strServer, false);
                if (dsServer.Tables[0].Rows.Count > 0)
                    Int32.TryParse(dsServer.Tables[0].Rows[0]["id"].ToString(), out intServer);
            }

            if (intServer > 0)
            {
                DataSet dsAssets = oServer.GetAssets(intServer);
                foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                {
                    if (drAsset["latest"].ToString() == "1")
                    {
                        intAsset = Int32.Parse(drAsset["assetid"].ToString());
                        break;
                    }
                }
                if (_name.ToUpper().Contains("-RIB") == false && _name.ToUpper().Contains("-RM") == false)
                {
                    if (_name.ToUpper().Contains("-BACKUP") == true)
                        oServer.AddIP(intServer, _ipaddressid, 0, 0, 0, 1);
                    else
                        oServer.AddIP(intServer, _ipaddressid, 0, 1, 0, 0);
                }
            }

            Log oLog = new Log(0, dsn);
            string strSerial = oAsset.Get(intAsset, "serial");
            //if (intAsset > 0 && _name.Trim() != "" && _type != IPAddressType.ILO)
            //{
            //    Variables oVariable = new Variables(intEnvironment);
            //    Settings oSetting = new Settings(0, dsn);
            //    Functions oFunction = new Functions(0, dsn, intEnvironment);

            //    string strIP = oIPAddresses.GetName(_ipaddressid, 0);
            //    string strDescription = oIPAddresses.GetDescription(_ipaddressid, _name, intAsset, dsnAsset, "", intEnvironment);
            //    System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
            //    ClearViewWebServices oWebService = new ClearViewWebServices();
            //    oWebService.Timeout = Timeout.Infinite;
            //    oWebService.Credentials = oCredentials;
            //    oWebService.Url = oVariable.WebServiceURL();
            //    bool boolDNS_QIP = oSetting.IsDNS_QIP();
            //    bool boolDNS_Bluecat = oSetting.IsDNS_Bluecat();
            //    string strEMailIdsBCC = "";

            //    if (boolDNS_QIP == true)
            //    {
            //        // QIP
            //        oLog.AddEvent(_name, strSerial, "Calling Web Service Function CreateDNSforPNC(" + strIP + ", " + _name + ", Server, , " + oVariable.DNS_Domain() + ", " + oVariable.DNS_NameService() + ", " + oVariable.DNS_DynamicDNSUpdate() + ", " + intProfile.ToString() + ", true) on " + oVariable.WebServiceURL(), LoggingType.Information);
            //        string strDNS = oWebService.CreateDNSforPNC(strIP, _name, "Server", "", oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), intProfile, 0, true);
            //        if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
            //        {
            //            oLog.AddEvent(_name, strSerial, "QIP DNS Record = SUCCESS", LoggingType.Information);
            //        }
            //        else
            //        {
            //            if (strDNS.StartsWith("***CONFLICT") == true)
            //            {
            //                oLog.AddEvent(_name, strSerial, "QIP DNS Record = CONFLICT", LoggingType.Warning);
            //                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
            //                oFunction.SendEmail("QIP DNS Automation Conflict", strEMailIdsBCC, "", "", "QIP DNS Automation Conflict", "<p>There was a CONFLICT creating the QIP DNS Record for " + _name + " (SERVERID: " + intServer.ToString() + ")</p><p>Error: " + strDNS + "</p>", true, false);
            //            }
            //            else if (strDNS.ToUpper().Contains("SUBNET FOR") == true)
            //            {
            //                oLog.AddEvent(_name, strSerial, "QIP DNS Record = SUBNET ERROR: " + strDNS, LoggingType.Error);
            //                // This occurs if a subnet does not exist.  Will need to be routed for assignment and continue.
            //                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
            //                oFunction.SendEmail("QIP DNS Automation Subnet Error", strEMailIdsBCC, "", "", "QIP DNS Automation Subnet Error", "<p>There was a SUBNET ERROR creating the QIP DNS Record for " + _name + " (SERVERID: " + intServer.ToString() + ")</p><p>Error: " + strDNS + "</p>", true, false);
            //            }
            //            else
            //            {
            //                oLog.AddEvent(_name, strSerial, "QIP DNS Record = ERROR: " + strDNS, LoggingType.Error);
            //                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
            //                oFunction.SendEmail("QIP DNS Automation Error", strEMailIdsBCC, "", "", "QIP DNS Automation Error", "<p>There was an ERROR creating the QIP DNS Record for " + _name + " (SERVERID: " + intServer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
            //            }
            //        }
            //        oLog.AddEvent(_name, strSerial, "QIP DNS Record Finished", LoggingType.Information);
            //    }
            //    if (boolDNS_Bluecat == true)
            //    {
            //        // BlueCat
            //        oLog.AddEvent(_name, strSerial, "Calling Web Service Function CreateBluecatDNS(" + strIP + ", " + _name + ", " + strDescription + ", " + "" + ") on " + oVariable.WebServiceURL(), LoggingType.Information);
            //        string strDNS = oWebService.CreateBluecatDNS(strIP, _name, strDescription, "");
            //        if (strDNS == "SUCCESS" || strDNS.StartsWith("***DUPLICATE") == true)
            //        {
            //            oLog.AddEvent(_name, strSerial, "BlueCat DNS Record = SUCCESS", LoggingType.Information);
            //        }
            //        else
            //        {
            //            if (strDNS.StartsWith("***CONFLICT") == true)
            //            {
            //                oLog.AddEvent(_name, strSerial, "BlueCat DNS Record = CONFLICT", LoggingType.Warning);
            //                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
            //                oFunction.SendEmail("BlueCat DNS Automation Conflict", strEMailIdsBCC, "", "", "BlueCat DNS Automation Conflict", "<p>There was a CONFLICT creating the BlueCat DNS Record for " + _name + " (SERVERID: " + intServer.ToString() + ")</p><p>Error: " + strDNS + "</p>", true, false);
            //                // Still update the IPAM field / description
            //                oLog.AddEvent(_name, strSerial, "Try updating the IPAM Record", LoggingType.Debug);
            //                strDNS = oWebService.UpdateBluecatDescriptionDNS(strIP, strDescription);
            //                if (strDNS == "SUCCESS")
            //                    oLog.AddEvent(_name, strSerial, "BlueCat DNS Description / IPAM = SUCCESS", LoggingType.Information);
            //                else
            //                    oLog.AddEvent(_name, strSerial, "BlueCat DNS Description / IPAM = ERROR: " + strDNS, LoggingType.Error);
            //            }
            //            else
            //            {
            //                oLog.AddEvent(_name, strSerial, "BlueCat DNS Record = ERROR: " + strDNS, LoggingType.Error);
            //                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
            //                oFunction.SendEmail("BlueCat DNS Automation Error", strEMailIdsBCC, "", "", "BlueCat DNS Automation Error", "<p>There was an ERROR creating the BlueCat DNS Record for " + _name + " (SERVERID: " + intServer.ToString() + ")</p><p>Error: " + strDNS + "</p><p>NOTE: Since this is an unexpected error, there was no task initiated to fix this problem. You must correct it yourself.</p>", true, false);
            //            }
            //        }
            //        oLog.AddEvent(_name, strSerial, "BlueCat DNS Record Finished", LoggingType.Information);
            //    }
            //}
            //else
                oLog.AddEvent(_name, strSerial, "BlueCat IP Address Skipped : " + intAsset.ToString() + "," + _name.Trim() + "," + _type.ToString(), LoggingType.Information);
        }
    }
}