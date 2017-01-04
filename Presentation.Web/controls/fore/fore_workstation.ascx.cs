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
    public partial class fore_workstation : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected int intProfile;
        protected Forecast oForecast;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Platforms oPlatform;
        protected OperatingSystems oOperatingSystems;
        protected VirtualHDD oVirtualHDD;
        protected VirtualRam oVirtualRam;
        protected VirtualCPU oVirtualCPU;
        protected Classes oClass;
        protected int intForecast;
        protected int intPlatform = 0;
        protected int intType = 0;
        protected int intID = 0;
        protected bool boolProduction = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oOperatingSystems = new OperatingSystems(intProfile, dsn);
            oVirtualHDD = new VirtualHDD(intProfile, dsn);
            oVirtualRam = new VirtualRam(intProfile, dsn);
            oVirtualCPU = new VirtualCPU(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            if (Request.QueryString["parent"] != null && Request.QueryString["parent"] != "")
                intForecast = Int32.Parse(Request.QueryString["parent"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["step"] != null && Request.QueryString["step"] != "")
                panUpdate.Visible = true;
            else
                panNavigation.Visible = true;
            
            if (intID > 0)
            {
                DataSet ds = oForecast.GetAnswer(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    bool boolHundred = false;
                    int intConfidence = Int32.Parse(ds.Tables[0].Rows[0]["confidenceid"].ToString());
                    if (intConfidence > 0)
                    {
                        Confidence oConfidence = new Confidence(intProfile, dsn);
                        string strConfidence = oConfidence.Get(intConfidence, "name");
                        if (strConfidence.Contains("100%") == true)
                            boolHundred = true;
                    }
                    if (boolHundred == true)
                    {
                        panUpdate.Visible = false;
                        panNavigation.Visible = false;
                        btnHundred.Visible = true;
                    }
                    intPlatform = Int32.Parse(ds.Tables[0].Rows[0]["platformid"].ToString());
                    int intStep = Int32.Parse(ds.Tables[0].Rows[0]["step"].ToString());
                    int intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                    int intEnv = Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString());
                    int intAddress = Int32.Parse(ds.Tables[0].Rows[0]["addressid"].ToString());
                    int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
                    boolProduction = oClass.IsProd(Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString()));
                    intForecast = Int32.Parse(ds.Tables[0].Rows[0]["forecastid"].ToString());
                    DataSet dsSteps = oForecast.GetSteps(intPlatform, 1);
                    if (dsSteps.Tables[0].Rows.Count == intStep)
                        btnNext.Text = "Finish";
                    if (intStep == 0 || intStep == 1)
                        btnBack.Enabled = false;
                    if (Request.QueryString["type"] != null && Request.QueryString["type"] != "")
                    {
                        intType = Int32.Parse(Request.QueryString["type"]);
                        if (!IsPostBack)
                            LoadPlatform(intPlatform, intType, intClass, intEnv, intAddress);
                        if (Request.QueryString["model"] != null && Request.QueryString["model"] != "")
                        {
                            intModel = Int32.Parse(Request.QueryString["model"]);
                            ddlModels.SelectedValue = intModel.ToString();
                            int intParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                            if (Int32.Parse(oModel.Get(intParent, "hostid")) > 0)
                            {
                                // Virtual - requires additional info
                                panVirtual.Visible = true;
                                if (boolProduction == true)
                                    panProduction.Visible = true;
                            }
                        }
                    }
                    else if (!IsPostBack)
                    {
                        if (intModel > 0)
                        {
                            ddlModels.SelectedValue = intModel.ToString();
                            intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                            intType = Int32.Parse(oModel.Get(intModel, "typeid"));
                            LoadPlatform(intPlatform, intType, intClass, intEnv, intAddress);
                            if (Int32.Parse(oModel.Get(intModel, "hostid")) > 0)
                            {
                                // Virtual - requires additional info
                                panVirtual.Visible = true;
                                DataSet dsVirtual = oForecast.GetWorkstation(intID);
                                if (dsVirtual.Tables[0].Rows.Count > 0)
                                {
                                    ddlRam.SelectedValue = dsVirtual.Tables[0].Rows[0]["ramid"].ToString();
                                    ddlOS.SelectedValue = dsVirtual.Tables[0].Rows[0]["osid"].ToString();
                                    ddlRecovery.SelectedValue = dsVirtual.Tables[0].Rows[0]["recovery"].ToString();
                                    ddlCPU.SelectedValue = dsVirtual.Tables[0].Rows[0]["cpuid"].ToString();
                                    ddlHardDrive.SelectedValue = dsVirtual.Tables[0].Rows[0]["hddid"].ToString();
                                }
                                if (boolProduction == true)
                                    panProduction.Visible = true;
                            }
                        }
                        else
                            LoadPlatform(intPlatform, intType, intClass, intEnv, intAddress);
                    }
                }
            }
            btnClose.Attributes.Add("onclick", "return window.close();");
            btnUpdate.Attributes.Add("onclick", "return ValidateDropDown('" + ddlModels.ClientID + "','Please select a model');");
        }
        protected int LoadPlatform(int _platformid, int _typeid, int _classid, int _environmentid, int _addressid)
        {
            int intCount = 0;
            ddlTypes.DataTextField = "name";
            ddlTypes.DataValueField = "id";
            ddlTypes.DataSource = oType.Gets(_platformid, 1);
            ddlTypes.DataBind();
            ddlTypes.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            if (_typeid > 0)
            {
                ddlTypes.SelectedValue = _typeid.ToString();
                panModels.Visible = true;
                ddlModels.DataTextField = "name";
                ddlModels.DataValueField = "id";
                DataSet ds = oModelsProperties.GetTypes(1, _typeid, 1);
                if (ds.Tables[0].Rows.Count > 0)
                    //                ddlModels.DataSource = ds;
                    ddlModels.DataSource = oModel.GetLocation(_classid, _environmentid, _addressid, _typeid);
                else
                    ddlModels.DataSource = oModel.Gets(_typeid, 1);
                ddlModels.DataBind();
                ddlModels.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlRam.DataTextField = "name";
                ddlRam.DataValueField = "id";
                ddlRam.DataSource = oVirtualRam.GetVMwares(1);
                ddlRam.DataBind();
                ddlRam.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlOS.DataTextField = "name";
                ddlOS.DataValueField = "id";
                ddlOS.DataSource = oOperatingSystems.Gets(1, 1);
                ddlOS.DataBind();
                ddlOS.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlCPU.DataTextField = "name";
                ddlCPU.DataValueField = "id";
                ddlCPU.DataSource = oVirtualCPU.GetVMwares(1);
                ddlCPU.DataBind();
                ddlCPU.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlHardDrive.DataTextField = "name";
                ddlHardDrive.DataValueField = "id";
                ddlHardDrive.DataSource = oVirtualHDD.GetVMwares(1);
                ddlHardDrive.DataBind();
                ddlHardDrive.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                intCount = ddlModels.Items.Count;
            }
            return intCount;
        }
        protected void ddlTypes_Change(Object Sender, EventArgs e)
        {
            if (ddlTypes.SelectedIndex > 0)
                Response.Redirect(Request.Path + "?parent=" + intForecast + "&id=" + intID + "&type=" + ddlTypes.SelectedItem.Value + "&step=" + Request.QueryString["step"]);
            else
                Response.Redirect(Request.Path + "?parent=" + intForecast + "&id=" + intID + "&step=" + Request.QueryString["step"]);
        }
        protected void ddlModels_Change(Object Sender, EventArgs e)
        {
            if (ddlModels.SelectedIndex > 0)
                Response.Redirect(Request.Path + "?parent=" + intForecast + "&id=" + intID + "&type=" + ddlTypes.SelectedItem.Value + "&model=" + ddlModels.SelectedItem.Value + "&step=" + Request.QueryString["step"]);
            else
                Response.Redirect(Request.Path + "?parent=" + intForecast + "&id=" + intID + "&type=" + ddlTypes.SelectedItem.Value + "&step=" + Request.QueryString["step"]);
        }
        protected int Save()
        {
            if (intID == 0)
            {
                // oForecast.DeleteAnswer(intForecast, intPlatform);
                intID = oForecast.AddAnswer(intForecast, intPlatform, 0, intProfile);
            }
            // Check to see if step is done
            int intDone = 0;
            if (ddlModels.SelectedIndex > 0)
            {
                int intModel = Int32.Parse(ddlModels.SelectedItem.Value);
                oForecast.UpdateAnswerModel(intID, intModel);
                if (panVirtual.Visible == true)
                {
                    oForecast.DeleteWorkstation(intID);
                    oForecast.AddWorkstation(intID, Int32.Parse(ddlRam.SelectedItem.Value), Int32.Parse(ddlOS.SelectedItem.Value), Int32.Parse(ddlRecovery.SelectedItem.Value), 0, Int32.Parse(ddlHardDrive.SelectedItem.Value), Int32.Parse(ddlCPU.SelectedItem.Value));
                    if (ddlRam.SelectedIndex > 0 && ddlOS.SelectedIndex > 0 && ddlHardDrive.SelectedIndex > 0 && ddlCPU.SelectedIndex > 0 && (boolProduction == false || ddlRecovery.SelectedIndex > 0))
                        intDone = 1;
                }
                else
                    intDone = 1;
            }
            return intDone;
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            oForecast.UpdateAnswerStep(intID, -1, -1);
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            int intDone = Save();
            oForecast.UpdateAnswerStep(intID, 1, intDone);
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }
        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            int intDone = Save();
            int intStep = Int32.Parse(Request.QueryString["step"]);
            string strAlert = oForecast.AddStepDone(intID, intStep, intDone);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">" + strAlert + "window.navigate('" + Request.Path + "?id=" + intID.ToString() + "&save=true');<" + "/" + "script>");
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
    }
}