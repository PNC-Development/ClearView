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
    public partial class application : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected bool boolUseCostCenter = (ConfigurationManager.AppSettings["USE_COST_CENTER"] == "1");
        protected int intProfile;
        protected OnDemand oOnDemand;
        protected Forecast oForecast;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Users oUser;
        protected Variables oVariable;
        protected Classes oClass;
        protected Mnemonic oMnemonic;
        protected CostCenter oCostCenter;
        protected int intID = 0;
        protected int intStep = 0;
        protected int intType = 0;
        protected int intRequest = 0;
        protected bool boolPNC = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "ClearView Application Information";
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oOnDemand = new OnDemand(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oClass = new Classes(intProfile, dsn);
            oMnemonic = new Mnemonic(intProfile, dsn);
            oCostCenter = new CostCenter(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
                intStep = Int32.Parse(Request.QueryString["sid"]);
            if (oForecast.GetAnswer(intID, "completed") == "" && Request.QueryString["view"] == null)
            {
                if (Request.QueryString["step"] != null && Request.QueryString["step"] != "")
                    panUpdate.Visible = true;
                else
                    panNavigation.Visible = true;
            }
            else
                btnClose.Text = "Close";
            string strPNC = "";
            int intClass = 0;
            if (intID > 0)
            {
                Page.Title = "ClearView Application Information | Design # " + intID.ToString();
                DataSet ds = oForecast.GetAnswer(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intRequest = oForecast.GetRequestID(intID, true);
                    txtName.Text = ds.Tables[0].Rows[0]["appname"].ToString();
                    intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                    if (oClass.Get(intClass, "pnc") == "1")
                        boolPNC = true;
                    if (boolPNC == true)
                    {
                        panMnemonic.Visible = true;
                        strPNC = " && ValidateHidden0('" + hdnMnemonic.ClientID + "','" + txtMnemonic.ClientID + "','Please enter the mnemonic of this design\\n\\n(Start typing and a list will be presented...)')";
                        int intMnemonic = Int32.Parse(ds.Tables[0].Rows[0]["mnemonicid"].ToString());
                        if (intMnemonic > 0)
                            txtMnemonic.Text = oMnemonic.Get(intMnemonic, "factory_code") + " - " + oMnemonic.Get(intMnemonic, "name");
                        hdnMnemonic.Value = intMnemonic.ToString();

                        int intCostCenter = 0;
                        if (boolUseCostCenter == true)
                        {
                            panCostCenter.Visible = true;
                            strPNC = " && ValidateHidden0('" + hdnCostCenter.ClientID + "','" + txtCostCenter.ClientID + "','Please enter the cost center of this design\\n\\n(Start typing and a list will be presented...)')";
                            if (Int32.TryParse(ds.Tables[0].Rows[0]["costcenterid"].ToString(), out intCostCenter) == true)
                                txtCostCenter.Text = oCostCenter.GetName(intCostCenter);
                        }
                        hdnCostCenter.Value = intCostCenter.ToString();
                    }
                    else
                    {
                        panCode.Visible = true;
                        strPNC = " && ValidateText('" + txtCode.ClientID + "','Please enter an application code')";
                        txtCode.Text = ds.Tables[0].Rows[0]["appcode"].ToString();
                    }
                    int intDR = Int32.Parse(ds.Tables[0].Rows[0]["dr_criticality"].ToString());
                    if (intDR == 1)
                        radHigh.Checked = true;
                    if (intDR == 2)
                        radLow.Checked = true;
                    int intOwner = Int32.Parse(ds.Tables[0].Rows[0]["appcontact"].ToString());
                    int intPrimary = Int32.Parse(ds.Tables[0].Rows[0]["admin1"].ToString());
                    int intSecondary = Int32.Parse(ds.Tables[0].Rows[0]["admin2"].ToString());
                    int intAppOwner = Int32.Parse(ds.Tables[0].Rows[0]["appowner"].ToString());
                    int intEngineer = Int32.Parse(ds.Tables[0].Rows[0]["networkengineer"].ToString());
                    if (intOwner > 0)
                        txtOwner.Text = oUser.GetFullName(intOwner) + " (" + oUser.GetName(intOwner) + ")";
                    if (intPrimary > 0)
                        txtPrimary.Text = oUser.GetFullName(intPrimary) + " (" + oUser.GetName(intPrimary) + ")";
                    if (intSecondary > 0)
                        txtSecondary.Text = oUser.GetFullName(intSecondary) + " (" + oUser.GetName(intSecondary) + ")";
                    if (intAppOwner > 0)
                        txtAppOwner.Text = oUser.GetFullName(intAppOwner) + " (" + oUser.GetName(intAppOwner) + ")";
                    if (intEngineer > 0)
                        txtEngineer.Text = oUser.GetFullName(intEngineer) + " (" + oUser.GetName(intEngineer) + ")";
                    hdnOwner.Value = intOwner.ToString();
                    hdnPrimary.Value = intPrimary.ToString();
                    hdnSecondary.Value = intSecondary.ToString();
                    hdnAppOwner.Value = intAppOwner.ToString();
                    hdnEngineer.Value = intEngineer.ToString();
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
            txtOwner.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divOwner.ClientID + "','" + lstOwner.ClientID + "','" + hdnOwner.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstOwner.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtPrimary.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divPrimary.ClientID + "','" + lstPrimary.ClientID + "','" + hdnPrimary.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstPrimary.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtSecondary.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divSecondary.ClientID + "','" + lstSecondary.ClientID + "','" + hdnSecondary.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstSecondary.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtAppOwner.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAppOwner.ClientID + "','" + lstAppOwner.ClientID + "','" + hdnAppOwner.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstAppOwner.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtEngineer.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divEngineer.ClientID + "','" + lstEngineer.ClientID + "','" + hdnEngineer.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstEngineer.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtMnemonic.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'500','195','" + divMnemonic.ClientID + "','" + lstMnemonic.ClientID + "','" + hdnMnemonic.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_mnemonics.aspx',2);");
            lstMnemonic.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtCostCenter.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'200','195','" + divCostCenter.ClientID + "','" + lstCostCenter.ClientID + "','" + hdnCostCenter.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_cost_centers.aspx',5);");
            lstCostCenter.Attributes.Add("ondblclick", "AJAXClickRow();");
            btnClose.Attributes.Add("onclick", "return window.close();");
            string strEngineer = "";
            if (oForecast.IsHACSM(intID) == true)
            {
                panEngineer.Visible = true;
                strEngineer = " && ValidateHidden0('" + hdnEngineer.ClientID + "','" + txtEngineer.ClientID + "','Please enter the LAN ID of your network engineer')";
            }
            string strAppOwner = "";
            if (boolPNC == true)
            {
                panAppOwner.Visible = true;
                strAppOwner = " && ValidateHidden0('" + hdnAppOwner.ClientID + "','" + txtAppOwner.ClientID + "','Please enter the LAN ID of your application owner')";
            }
            if (oClass.IsProd(intClass))
            {
                panDR.Visible = true;
                btnNext.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter an application name')" +
                    strPNC +
                    " && ValidateRadioButtons('" + radHigh.ClientID + "','" + radLow.ClientID + "','Please select the disaster recovery criticality')" +
                    " && ValidateHidden0('" + hdnOwner.ClientID + "','" + txtOwner.ClientID + "','Please enter the LAN ID of your Departmental Manager')" +
                    " && ValidateHidden0('" + hdnPrimary.ClientID + "','" + txtPrimary.ClientID + "','Please enter the LAN ID of your Application Technical Lead')" +
                    //" && ValidateHidden0('" + hdnSecondary.ClientID + "','" + txtSecondary.ClientID + "','Please enter the LAN ID of your secondary administrator')" +
                    strAppOwner +
                    strEngineer +
                    ";");
                btnUpdate.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter an application name')" +
                    strPNC +
                    " && ValidateRadioButtons('" + radHigh.ClientID + "','" + radLow.ClientID + "','Please select the disaster recovery criticality')" +
                    " && ValidateHidden0('" + hdnOwner.ClientID + "','" + txtOwner.ClientID + "','Please enter the LAN ID of your Departmental Manager')" +
                    " && ValidateHidden0('" + hdnPrimary.ClientID + "','" + txtPrimary.ClientID + "','Please enter the LAN ID of your Application Technical Lead')" +
                    //" && ValidateHidden0('" + hdnSecondary.ClientID + "','" + txtSecondary.ClientID + "','Please enter the LAN ID of your secondary administrator')" +
                    strAppOwner +
                    strEngineer +
                    ";");
            }
            else
            {
                btnNext.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter an application name')" +
                    strPNC +
                    " && ValidateHidden0('" + hdnOwner.ClientID + "','" + txtOwner.ClientID + "','Please enter the LAN ID of your Departmental Manager')" +
                    " && ValidateHidden0('" + hdnPrimary.ClientID + "','" + txtPrimary.ClientID + "','Please enter the LAN ID of your Application Technical Lead')" +
                    //" && ValidateHidden0('" + hdnSecondary.ClientID + "','" + txtSecondary.ClientID + "','Please enter the LAN ID of your secondary administrator')" +
                    strAppOwner +
                    strEngineer +
                    ";");
                btnUpdate.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter an application name')" +
                    strPNC +
                    " && ValidateHidden0('" + hdnOwner.ClientID + "','" + txtOwner.ClientID + "','Please enter the LAN ID of your Departmental Manager')" +
                    " && ValidateHidden0('" + hdnPrimary.ClientID + "','" + txtPrimary.ClientID + "','Please enter the LAN ID of your Application Technical Lead')" +
                    //" && ValidateHidden0('" + hdnSecondary.ClientID + "','" + txtSecondary.ClientID + "','Please enter the LAN ID of your secondary administrator')" +
                    strAppOwner +
                    strEngineer +
                    ";");
            }
            btnManager.Attributes.Add("onclick", "return OpenWindow('NEW_USER','');");
        }
        private void Save()
        {
            int intDR = 0;
            if (radHigh.Checked == true)
                intDR = 1;
            if (radLow.Checked == true)
                intDR = 2;
            int intAppOwner = 0;
            if (Request.Form[hdnAppOwner.UniqueID] != "")
                intAppOwner = Int32.Parse(Request.Form[hdnAppOwner.UniqueID]);
            int intEngineer = 0;
            if (Request.Form[hdnEngineer.UniqueID] != "")
                intEngineer = Int32.Parse(Request.Form[hdnEngineer.UniqueID]);
            int intMnemonic = 0;
            if (Request.Form[hdnMnemonic.UniqueID] != "")
                intMnemonic = Int32.Parse(Request.Form[hdnMnemonic.UniqueID]);
            int intCostCenter = 0;
            if (Request.Form[hdnCostCenter.UniqueID] != "")
                intCostCenter = Int32.Parse(Request.Form[hdnCostCenter.UniqueID]);
            oForecast.UpdateAnswer(intID, txtName.Text, txtCode.Text, intMnemonic, intCostCenter, intDR, Int32.Parse(Request.Form[hdnOwner.UniqueID]), Int32.Parse(Request.Form[hdnPrimary.UniqueID]), Int32.Parse(Request.Form[hdnSecondary.UniqueID]), intAppOwner, intEngineer, 0, 0);
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