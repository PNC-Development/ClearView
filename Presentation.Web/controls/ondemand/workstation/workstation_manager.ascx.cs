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
    public partial class workstation_manager : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected OnDemand oOnDemand;
        protected Forecast oForecast;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Users oUser;
        protected Variables oVariable;
        protected Workstations oWorkstation;
        protected Classes oClass;
        protected CostCenter oCostCenter;
        protected int intID = 0;
        protected int intStep = 0;
        protected int intType = 0;
        protected int intRequest = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oOnDemand = new OnDemand(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oWorkstation = new Workstations(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oCostCenter = new CostCenter(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
                intStep = Int32.Parse(Request.QueryString["sid"]);
            if (Request.QueryString["step"] != null && Request.QueryString["step"] != "")
                panUpdate.Visible = true;
            else
                panNavigation.Visible = true;
            int intClass = 0;
            string strPool = "";
            if (intID > 0)
            {
                DataSet ds = oForecast.GetAnswer(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (!IsPostBack)
                        LoadLists();
                    intRequest = oForecast.GetRequestID(intID, true);
                    //ddlCost.SelectedValue = ds.Tables[0].Rows[0]["costid"].ToString();
                    int intCostCenter = 0;
                    if (Int32.TryParse(ds.Tables[0].Rows[0]["costcenterid"].ToString(), out intCostCenter) == true && intCostCenter > 0)
                        txtCostCenter.Text = oCostCenter.GetName(intCostCenter);
                    hdnCostCenter.Value = intCostCenter.ToString();
                    intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                    if (oClass.IsProd(intClass))
                    {
                        if (ds.Tables[0].Rows[0]["poolid"].ToString() != "" && ds.Tables[0].Rows[0]["poolid"].ToString() != "0")
                        {
                            radExisting.Checked = true;
                            divExisting.Style["display"] = "inline";
                            ddlExisting.SelectedValue = ds.Tables[0].Rows[0]["poolid"].ToString();
                        }
                        else
                            radNone.Checked = true;
                        panPool.Visible = true;
                        strPool = " && EnsurePool('" + radNone.ClientID + "','" + radNew.ClientID + "','" + txtName.ClientID + "','" + txtDescription.ClientID + "','" + radExisting.ClientID + "','" + ddlExisting.ClientID + "')";

                    }
                    int intManager = Int32.Parse(ds.Tables[0].Rows[0]["appcontact"].ToString());
                    if (intManager > 0)
                        txtManager.Text = oUser.GetFullName(intManager) + " (" + oUser.GetName(intManager) + ")";
                    hdnManager.Value = intManager.ToString();
                    int intModel = oForecast.GetModel(intID);
                    intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                    intType = oModel.GetType(intModel);
                    DataSet dsSteps = oOnDemand.GetWizardSteps(intType, 1);
                    int intCount = Int32.Parse(oOnDemand.GetWizardStep(intStep, "step"));
                    if (dsSteps.Tables[0].Rows.Count == intCount)
                        btnNext.Text = "Finish";
                    if (intCount == 0 || intCount == 1)
                        btnBack.Enabled = false;
                }
            }
            txtManager.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divManager.ClientID + "','" + lstManager.ClientID + "','" + hdnManager.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstManager.Attributes.Add("ondblclick", "AJAXClickRow();");
            btnManager.Attributes.Add("onclick", "return OpenWindow('NEW_USER','');");
            btnClose.Attributes.Add("onclick", "return window.close();");
            btnNext.Attributes.Add("onclick", "return ValidateHidden0('" + hdnManager.ClientID + "','" + txtManager.ClientID + "','Please enter the LAN ID of your user access administrator')" +
                " && ValidateHidden0('" + hdnCostCenter.ClientID + "','" + txtCostCenter.ClientID + "','Please enter the cost center to be billed for this workstation')" +
                strPool +
                ";");
            btnUpdate.Attributes.Add("onclick", "return ValidateHidden0('" + hdnManager.ClientID + "','" + txtManager.ClientID + "','Please enter the LAN ID of your user access administrator')" +
                " && ValidateHidden0('" + hdnCostCenter.ClientID + "','" + txtCostCenter.ClientID + "','Please enter the cost center to be billed for this workstation')" +
                strPool +
                ";");
            radNone.Attributes.Add("onclick", "ShowHideDiv('" + divNew.ClientID + "','none');ShowHideDiv('" + divExisting.ClientID + "','none');");
            radNew.Attributes.Add("onclick", "ShowHideDiv('" + divNew.ClientID + "','inline');ShowHideDiv('" + divExisting.ClientID + "','none');");
            radExisting.Attributes.Add("onclick", "ShowHideDiv('" + divNew.ClientID + "','none');ShowHideDiv('" + divExisting.ClientID + "','inline');");
            txtCostCenter.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'200','195','" + divCostCenter.ClientID + "','" + lstCostCenter.ClientID + "','" + hdnCostCenter.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_cost_centers.aspx',5);");
            lstCostCenter.Attributes.Add("ondblclick", "AJAXClickRow();");
        }
        private void LoadLists()
        {
            ddlExisting.DataValueField = "id";
            ddlExisting.DataTextField = "name";
            ddlExisting.DataSource = oWorkstation.GetPools(intProfile, 1);
            ddlExisting.DataBind();
            ddlExisting.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void Save()
        {
            int intPool = 0;
            if (panPool.Visible == true)
            {
                if (radNew.Checked == true)
                    intPool = oWorkstation.AddPool(txtName.Text, txtDescription.Text, intProfile, 0, intProfile, 1);
                if (radExisting.Checked == true)
                    intPool = Int32.Parse(ddlExisting.SelectedItem.Value);
            }
            int intCostCenter = 0;
            Int32.TryParse(Request.Form[hdnCostCenter.UniqueID], out intCostCenter);
            oForecast.UpdateAnswer(intID, "", "", 0, intCostCenter, 0, Int32.Parse(Request.Form[hdnManager.UniqueID]), 0, 0, 0, 0, 0, intPool);
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            oOnDemand.Back(intID);
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&backward=true");
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            Save();
            oOnDemand.Next(intID, Int32.Parse(Request.QueryString["sid"]));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&forward=true");
        }
        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            Save();
            oOnDemand.Next(intID, Int32.Parse(Request.QueryString["sid"]));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&forward=true");
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
    }
}