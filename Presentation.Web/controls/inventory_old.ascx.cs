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
    public partial class inventory_old : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Platforms oPlatform;
        protected int intPlatform = 0;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected bool boolAction = false;
        protected bool boolDemand = false;
        protected bool boolSupply = false;
        protected bool boolOrder = false;
        protected bool boolOrderView = false;
        protected bool boolAdd = false;
        protected bool boolSettings = false;
        protected bool boolForms = false;
        protected bool boolAlert = false;
        protected bool boolSecurity = false;
        protected string strPlatforms = "";
        protected string strActionForm = "";
        protected string strDemandForm = "";
        protected string strSupplyForm = "";
        protected string strOrderForm = "";
        protected string strOrderViewForm = "";
        protected string strAddForm = "";
        protected string strSettingsForm = "";
        protected string strAlertForm = "";
        protected string strFormForm = "";
        protected string strTabs = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intPlatform = Int32.Parse(Request.QueryString["id"]);
            bool Access = false;
            // Load Platforms
            DataSet ds = oPlatform.GetInventorys(intProfile, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (intPlatform == Int32.Parse(dr["platformid"].ToString()))
                {
                    strPlatforms += "<td><img src=\"/images/arrow_red.gif\" border=\"0\" align=\"absmiddle\" /></td><td><a class=\"greentableheader\" onclick=\"LoadWait();\" href=\"" + oPage.GetFullLink(intPage) + "?id=" + dr["platformid"].ToString() + "\">" + dr["name"].ToString() + "</a></td>";
                    Access = true;
                }
                else
                    strPlatforms += "<td><img src=\"/images/arrow_green.gif\" border=\"0\" align=\"absmiddle\" /></td><td><a class=\"greentableheader\" onclick=\"LoadWait();\" href=\"" + oPage.GetFullLink(intPage) + "?id=" + dr["platformid"].ToString() + "\">" + dr["name"].ToString() + "</a></td>";
            }
            strPlatforms = "<table cellpadding=\"3\" cellspacing=\"1\" border=\"0\">" + strPlatforms + "</table>";
            if (intPlatform == 0 || Access == false)
                panSelect.Visible = true;
            else
            {
                panShow.Visible = true;
                bool boolStart = false;
                strActionForm = oPlatform.Get(intPlatform, "action_form");
                if (strActionForm != "")
                {
                    if (Request.QueryString["div"] == "A" || (Request.QueryString["div"] == null && boolStart == false))
                    {
                        boolAction = true;
                        boolStart = true;
                    }
                    PHAction.Controls.Add((Control)LoadControl(strActionForm));
                    if (boolAction == true)
                        strTabs += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a id=\"aTab1\" href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab1',null,null,true);setTabvalue(1);\" class=\"tabheader\">Action Required</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
                    else
                        strTabs += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a id=\"aTab1\" href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab1',null,null,true);setTabvalue(1);\" class=\"tabheader\">Action Required</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
                }
                strDemandForm = oPlatform.Get(intPlatform, "demand_form");
                if (strDemandForm != "")
                {
                    if (Request.QueryString["div"] == "D" || (Request.QueryString["div"] == null && boolStart == false))
                    {
                        boolDemand = true;
                        boolStart = true;
                    }
                    PHDemand.Controls.Add((Control)LoadControl(strDemandForm));
                    if (boolDemand == true)
                        strTabs += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a id=\"aTab2\" href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab2',null,null,true);setTabvalue(2);\" class=\"tabheader\">Demand</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
                    else
                        strTabs += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a id=\"aTab2\" href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab2',null,null,true);setTabvalue(2);\" class=\"tabheader\">Demand</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
                }
                strSupplyForm = oPlatform.Get(intPlatform, "supply_form");
                if (strSupplyForm != "")
                {
                    if (Request.QueryString["div"] == "S" || (Request.QueryString["div"] == null && boolStart == false))
                    {
                        boolSupply = true;
                        boolStart = true;
                    }
                    PHSupply.Controls.Add((Control)LoadControl(strSupplyForm));
                    if (boolSupply == true)
                        strTabs += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a id=\"aTab3\" href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab3',null,null,true);setTabvalue(3);\" class=\"tabheader\">Supply</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
                    else
                        strTabs += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a id=\"aTab3\" href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab3',null,null,true);setTabvalue(3);\" class=\"tabheader\">Supply</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
                }
                strOrderForm = oPlatform.Get(intPlatform, "order_form");
                if (strOrderForm != "")
                {
                    if (Request.QueryString["div"] == "O" || (Request.QueryString["div"] == null && boolStart == false))
                    {
                        boolOrder = true;
                        boolStart = true;
                    }
                    PHOrder.Controls.Add((Control)LoadControl(strOrderForm));
                    if (boolOrder == true)
                        strTabs += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a id=\"aTab4\" href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab4',null,null,true); setTabvalue(4);\" class=\"tabheader\">New Order</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
                    else
                        strTabs += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a id=\"aTab4\" href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab4',null,null,true); setTabvalue(4);\" class=\"tabheader\">New Order</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
                }

                strOrderViewForm = oPlatform.Get(intPlatform, "order_view_form");
                if (strOrderViewForm != "")
                {
                    if (Request.QueryString["div"] == "V" || (Request.QueryString["div"] == null && boolStart == false))
                    {
                        boolOrderView = true;
                        boolStart = true;
                    }
                    PHOrderView.Controls.Add((Control)LoadControl(strOrderViewForm));
                    if (boolOrderView == true)
                        strTabs += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a id=\"aTab5\" href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab5',null,null,true) ;setTabvalue(5);\" class=\"tabheader\">View Orders</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
                    else
                        strTabs += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a id=\"aTab5\" href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab5',null,null,true);setTabvalue(5);\" class=\"tabheader\">View Orders</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
                }
                strAddForm = oPlatform.Get(intPlatform, "add_form");
                if (strAddForm != "")
                {
                    if (Request.QueryString["div"] == "N" || (Request.QueryString["div"] == null && boolStart == false))
                    {
                        boolAdd = true;
                        boolStart = true;
                    }
                    PHAdd.Controls.Add((Control)LoadControl(strAddForm));
                    if (boolAdd == true)
                        strTabs += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab6',null,null,true);setTabvalue(6);\" class=\"tabheader\">Deploy</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
                    else
                        strTabs += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab6',null,null,true);setTabvalue(6);\" class=\"tabheader\">Deploy</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
                }
                strSettingsForm = oPlatform.Get(intPlatform, "settings_form");
                if (strSettingsForm != "")
                {
                    if (Request.QueryString["div"] == "T" || (Request.QueryString["div"] == null && boolStart == false))
                    {
                        boolSettings = true;
                        boolStart = true;
                    }
                    PHSettings.Controls.Add((Control)LoadControl(strSettingsForm));
                    if (boolSettings == true)
                        strTabs += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab7',null,null,true);setTabvalue(7);\" class=\"tabheader\">Settings</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
                    else
                        strTabs += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab7',null,null,true);setTabvalue(7);\" class=\"tabheader\">Settings</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
                }
                strFormForm = oPlatform.Get(intPlatform, "forms_form");
                if (strFormForm != "")
                {
                    if (Request.QueryString["div"] == "F" || (Request.QueryString["div"] == null && boolStart == false))
                    {
                        boolForms = true;
                        boolStart = true;
                    }
                    PHForms.Controls.Add((Control)LoadControl(strFormForm));
                    if (boolForms == true)
                        strTabs += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab8',null,null,true);setTabvalue(8);\" class=\"tabheader\">Forms</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
                    else
                        strTabs += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab8',null,null,true);setTabvalue(8);\" class=\"tabheader\">Forms</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
                }
                strAlertForm = oPlatform.Get(intPlatform, "alert_form");
                if (strAlertForm != "")
                {
                    if (Request.QueryString["div"] == "X" || (Request.QueryString["div"] == null && boolStart == false))
                    {
                        boolAlert = true;
                        boolStart = true;
                    }
                    PHAlert.Controls.Add((Control)LoadControl(strAlertForm));
                    if (boolAlert == true)
                        strTabs += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab9',null,null,true);setTabvalue(9);\" class=\"tabheader\">Alerts</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
                    else
                        strTabs += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab9',null,null,true);setTabvalue(9);\" class=\"tabheader\">Alerts</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
                }
                if (Request.QueryString["div"] == "U" || (Request.QueryString["div"] == null && boolStart == false))
                {
                    boolSecurity = true;
                    boolStart = true;
                }
                if (boolSecurity == true)
                    strTabs += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab10',null,null,true);setTabvalue(10);\" class=\"tabheader\">Security</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
                else
                    strTabs += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab10',null,null,true);setTabvalue(10);\" class=\"tabheader\">Security</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
                Variables oVariable = new Variables(intEnvironment);
                txtAdministrator.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAdministrator.ClientID + "','" + lstAdministrator.ClientID + "','" + hdnAdministrator.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstAdministrator.Attributes.Add("ondblclick", "AJAXClickRow();");
                rptAdministrators.DataSource = oPlatform.GetUsers(intPlatform);
                rptAdministrators.DataBind();
                lblAdministrators.Visible = (rptAdministrators.Items.Count == 0);
                btnAdministrator.Attributes.Add("onclick", "return ValidateHidden0('" + hdnAdministrator.ClientID + "','" + txtAdministrator.ClientID + "','Please enter the LAN ID of the administrator') && ProcessButton(this) && LoadWait();");
            }
        }

        protected void btnAdministrator_Click(object sender, EventArgs e)
        {
            bool boolDuplicate = false;
            int intUser = 0;
            if (Int32.TryParse(Request.Form[hdnAdministrator.UniqueID], out intUser) && intUser > 0)
            {
                DataSet ds = oPlatform.GetUsers(intPlatform);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Int32.Parse(dr["userid"].ToString()) == intUser)
                    {
                        boolDuplicate = true;
                        break;
                    }
                }
                if (boolDuplicate == false)
                    oPlatform.AddUser(intPlatform, intUser, 1);
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intPlatform.ToString() + "&U=true");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)sender;
            oPlatform.DeleteUser(Int32.Parse(oDelete.CommandArgument));
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intPlatform.ToString() + "&U=true");
        }
    }
}