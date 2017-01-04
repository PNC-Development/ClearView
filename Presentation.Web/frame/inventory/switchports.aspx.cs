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
    public partial class switchports : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected Asset oAsset;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected int intProfile;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oAsset = new Asset(0, dsnAsset);
            oModel = new Models(0, dsn);
            oModelsProperties = new ModelsProperties(0, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["add"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "add", "<script type=\"text/javascript\">alert('The switchport was successfully added');<" + "/" + "script>");
            if (Request.QueryString["delete"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "delete", "<script type=\"text/javascript\">alert('The switchport was successfully deleted');<" + "/" + "script>");
            if (!IsPostBack)
            {
                DataSet ds = oAsset.Get(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblSerial.Text = ds.Tables[0].Rows[0]["serial"].ToString();
                    int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
                    intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                    lblModel.Text = ds.Tables[0].Rows[0]["modelname"].ToString();
                    lblAsset.Text = ds.Tables[0].Rows[0]["asset"].ToString();
                    DataSet dsAsset = oAsset.GetServerOrBlade(intID);
                    if (dsAsset.Tables[0].Rows.Count > 0)
                    {
                        int intRackID = Int32.Parse(dsAsset.Tables[0].Rows[0]["rackid"].ToString());
                        ddlSwitch.DataTextField = "name";
                        ddlSwitch.DataValueField = "id";
                        ddlSwitch.DataSource = oAsset.GetSwitchsByRack(intRackID, 1);
                        ddlSwitch.DataBind();
                        ddlSwitch.Items.Insert(0, new ListItem("-- SELECT --", "0"));

                        ddlSwitch.Attributes.Add("onchange", "PopulateSwitchs('" + ddlSwitch.ClientID + "','" + ddlBlade.ClientID + "','" + txtFexID.ClientID + "','" + ddlPort.ClientID + "','" + txtInterface.ClientID + "','" + lblInterface.ClientID + "','" + hdnBlade.ClientID + "');");
                        txtFexID.Attributes.Add("onblur", "UpdateTextHidden('" + txtFexID.ClientID + "','" + hdnBlade.ClientID + "');UpdateNetworkInterface('" + txtFexID.ClientID + "','" + ddlPort.ClientID + "','" + lblInterface.ClientID + "');");
                        ddlPort.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlPort.ClientID + "','" + hdnPort.ClientID + "');UpdateNetworkInterface('" + txtFexID.ClientID + "','" + ddlPort.ClientID + "','" + lblInterface.ClientID + "');");
                        txtFexID.Style["display"] = "none";

                        rptSwitchports.DataSource = oAsset.GetSwitchports(intID, SwitchPortType.ALL);
                        rptSwitchports.DataBind();
                        lblSwitchports.Visible = (rptSwitchports.Items.Count == 0);
                        foreach (RepeaterItem ri in rptSwitchports.Items)
                        {
                            LinkButton btnDelete = (LinkButton)ri.FindControl("btnDelete");
                            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                        }
                        btnAdd.Attributes.Add("onclick", "return ValidateDropDown('" + ddlType.ClientID + "','Please select a type')" +
                            " && ValidateDropDown('" + ddlNIC.ClientID + "','Please select a NIC')" +
                            " && ValidateDropDown('" + ddlSwitch.ClientID + "','Please select a switch')" +
                            " && ValidateDropDown('" + ddlBlade.ClientID + "','Please select a blade')" +
                            " && ValidateText('" + txtFexID.ClientID + "','Please enter a FEX ID')" +
                            " && ValidateDropDown('" + ddlPort.ClientID + "','Please select a port')" +
                            " && ValidateText('" + txtInterface.ClientID + "','Please enter an interface')" +
                            ";");
                        //txtName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnAdd.ClientID + "').click();return false;}} else {return true}; ");
                    }
                    else
                        btnAdd.Enabled = false;
                }
                else
                    btnAdd.Enabled = false;
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)sender;
            oAsset.DeleteSwitchport(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&delete=true");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            oAsset.AddSwitchport(Int32.Parse(ddlSwitch.SelectedItem.Value), intID, (SwitchPortType)Int32.Parse(ddlType.SelectedItem.Value), Int32.Parse(ddlNIC.SelectedItem.Value), Request.Form[hdnBlade.UniqueID], Int32.Parse(Request.Form[hdnPort.UniqueID]), txtInterface.Text);
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&add=true");
        }
    }
}
