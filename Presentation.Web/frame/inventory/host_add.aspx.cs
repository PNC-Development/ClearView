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
    public partial class host_add : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected Forecast oForecast;
        protected Confidence oConfidence;
        protected Classes oClass;
        protected Host oHost;
        protected int intProfile;
        protected int intModel = 0;
        protected string strResults = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oConfidence = new Confidence(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oHost = new Host(intProfile, dsn);
            if (Request.QueryString["add"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "add", "<script type=\"text/javascript\">RefreshOpeningWindow();alert('Host successfully ordered!\\n\\nPlease be patient while the inventory management window refreshes...');window.close();<" + "/" + "script>");
            if (Request.QueryString["update"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "update", "<script type=\"text/javascript\">RefreshOpeningWindow();alert('Host successfully updated!\\n\\nPlease be patient while the inventory management window refreshes...');window.close();<" + "/" + "script>");
            if (!IsPostBack)
            {
                int intPlatform = 0;
                
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                    intPlatform = Int32.Parse(Request.QueryString["id"]);
                if (intPlatform > 0)
                {
                    LoadLists(intPlatform);
                    if (Request.QueryString["hostid"] != null && Request.QueryString["hostid"] != "")
                    {
                        int intHost = Int32.Parse(Request.QueryString["hostid"]);
                        DataSet dsAnswer = oForecast.GetAnswer(intHost);
                        if (dsAnswer.Tables[0].Rows.Count > 0)
                        {
                            ddlHost.SelectedValue = dsAnswer.Tables[0].Rows[0]["hostid"].ToString();
                            txtName.Text = dsAnswer.Tables[0].Rows[0]["name"].ToString();
                            ddlLocation.SelectedValue = dsAnswer.Tables[0].Rows[0]["addressid"].ToString();
                            int intClass = Int32.Parse(dsAnswer.Tables[0].Rows[0]["classid"].ToString());
                            ddlClass.SelectedValue = intClass.ToString();
                            int intEnv = Int32.Parse(dsAnswer.Tables[0].Rows[0]["environmentid"].ToString());
                            hdnEnvironment.Value = intEnv.ToString();
                            ddlEnvironment.SelectedValue = intEnv.ToString();
                            ddlEnvironment.Enabled = true;
                            ddlEnvironment.DataTextField = "name";
                            ddlEnvironment.DataValueField = "id";
                            ddlEnvironment.DataSource = oClass.GetEnvironment(intClass, 0);
                            ddlEnvironment.DataBind();
                            ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                            ddlMaintenance.SelectedValue = dsAnswer.Tables[0].Rows[0]["maintenanceid"].ToString();
                            txtQuantity.Text = dsAnswer.Tables[0].Rows[0]["quantity"].ToString();
                            txtDate.Text = DateTime.Parse(dsAnswer.Tables[0].Rows[0]["implementation"].ToString()).ToShortDateString();
                            ddlConfidence.SelectedValue = dsAnswer.Tables[0].Rows[0]["confidenceid"].ToString();
                            if (ddlConfidence.SelectedIndex == ddlConfidence.Items.Count - 1)
                                btnUpdateHost.Visible = false;
                            btnAddHost.Visible = false;
                        }
                    }
                    else
                        btnUpdateHost.Visible = false;
                    imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
                    ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',0);EnableMaintenance();");
                    ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
                    ddlLocation.Attributes.Add("onchange", "WaitDDL('" + divLocation.ClientID + "');");
                    btnAddHost.Attributes.Add("onclick", "return ValidateDropDown('" + ddlHost.ClientID + "','Please select a platform')" +
                        " && ValidateDropDown('" + ddlLocation.ClientID + "','Please select a location')" +
                        " && ValidateDropDown('" + ddlClass.ClientID + "','Please select a class')" +
                        " && ValidateDropDown('" + ddlEnvironment.ClientID + "','Please select an environment')" +
                        " && EnsureMaintenance('" + ddlMaintenance.ClientID + "','Please select a maintenance window')" +
                        " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid commitment date')" +
                        " && ValidateDateToday('" + txtDate.ClientID + "','The commitment date must occur after today')" +
                        " && ValidateNumber0('" + txtQuantity.ClientID + "','Please enter a valid quantity')" +
                        ";");
                }
            }
        }
        public void LoadLists(int _platformid)
        {
            int intAddress = 0;
            if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                intAddress = Int32.Parse(Request.QueryString["aid"]);
            Locations oLocation = new Locations(intProfile, dsn);
            ddlLocation.DataValueField = "id";
            ddlLocation.DataTextField = "fullname";
            if (intAddress == 0)
                ddlLocation.DataSource = oLocation.GetAddressCommon();
            else
                ddlLocation.DataSource = oLocation.GetAddresss(1);
            ddlLocation.DataBind();
            ddlLocation.SelectedValue = intAddress.ToString();
            if (intAddress == 0)
                ddlLocation.Items.Add(new ListItem("-- NOT LISTED --", "-1"));
            ddlLocation.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlHost.DataTextField = "name";
            ddlHost.DataValueField = "id";
            ddlHost.DataSource = oHost.Gets(_platformid, 1);
            ddlHost.DataBind();
            ddlHost.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlClass.DataTextField = "name";
            ddlClass.DataValueField = "id";
            ddlClass.DataSource = oClass.Gets(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            MaintenanceWindow oMaintenanceWindow = new MaintenanceWindow(intProfile, dsn);
            ddlMaintenance.DataTextField = "name";
            ddlMaintenance.DataValueField = "id";
            ddlMaintenance.DataSource = oMaintenanceWindow.Gets(1);
            ddlMaintenance.DataBind();
            ddlMaintenance.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlConfidence.DataValueField = "id";
            ddlConfidence.DataTextField = "name";
            ddlConfidence.DataSource = oConfidence.Gets(1);
            ddlConfidence.DataBind();
        }
        protected void ddlLocation_Change(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&aid=" + ddlLocation.SelectedItem.Value);
        }
        protected void btnAddHost_Click(Object Sender, EventArgs e)
        {
            int intAnswer = oForecast.AddAnswer(0, Int32.Parse(Request.QueryString["id"]), Int32.Parse(ddlHost.SelectedItem.Value), intProfile);
            oForecast.UpdateAnswer(intAnswer, 0, 0, "", 0, "", "", txtName.Text, Int32.Parse(ddlLocation.SelectedItem.Value), Int32.Parse(ddlClass.SelectedItem.Value), 0, Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), Int32.Parse(ddlMaintenance.SelectedItem.Value), 0, 0, Int32.Parse(txtQuantity.Text), 0);
            oForecast.UpdateAnswer(intAnswer, DateTime.Parse(txtDate.Text), Int32.Parse(ddlConfidence.SelectedItem.Value), -999);
            Response.Redirect(Request.Path + "?add=true");
        }
        protected void btnUpdateHost_Click(Object Sender, EventArgs e)
        {
            int intAnswer = Int32.Parse(Request.QueryString["hostid"]);
            oForecast.UpdateAnswer(intAnswer, 0, 0, "", 0, "", "", txtName.Text, Int32.Parse(ddlLocation.SelectedItem.Value), Int32.Parse(ddlClass.SelectedItem.Value), 0, Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), Int32.Parse(ddlMaintenance.SelectedItem.Value), 0, 0, Int32.Parse(txtQuantity.Text), 0);
            oForecast.UpdateAnswer(intAnswer, DateTime.Parse(txtDate.Text), Int32.Parse(ddlConfidence.SelectedItem.Value), -999);
            Response.Redirect(Request.Path + "?update=true");
        }
    }
}
