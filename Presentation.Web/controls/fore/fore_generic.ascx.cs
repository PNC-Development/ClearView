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
    public partial class fore_generic : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intProfile;
        protected Forecast oForecast;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Platforms oPlatform;
        protected int intForecast;
        protected int intPlatform = 0;
        protected int intType = 0;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            if (Request.QueryString["parent"] != null && Request.QueryString["parent"] != "")
                intForecast = Int32.Parse(Request.QueryString["parent"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["step"] != null && Request.QueryString["step"] != "")
                panUpdate.Visible = true;
            else
                panNavigation.Visible = true;
            int intCount = 0;
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
                    bool boolOverride = (ds.Tables[0].Rows[0]["override"].ToString() == "1" || ds.Tables[0].Rows[0]["override"].ToString() == "-1");
                    intForecast = Int32.Parse(ds.Tables[0].Rows[0]["forecastid"].ToString());
                    DataSet dsSteps = oForecast.GetSteps(intPlatform, 1);
                    if (dsSteps.Tables[0].Rows.Count == intStep)
                        btnNext.Text = "Finish";
                    if (intStep == 0 || intStep == 1)
                        btnBack.Enabled = false;
                    if (Request.QueryString["type"] != null && Request.QueryString["type"] != "")
                        intType = Int32.Parse(Request.QueryString["type"]);
                    else if (!IsPostBack)
                    {
                        if (intModel > 0)
                        {
                            if (boolOverride == true)
                            {
                                ddlModels.SelectedValue = intModel.ToString();
                                intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                            }
                            else
                                ddlModels.SelectedValue = intModel.ToString();
                            intType = Int32.Parse(oModel.Get(intModel, "typeid"));
                        }
                    }
                    if (!IsPostBack)
                        intCount = LoadPlatform(intPlatform, intType, intClass, intEnv, intAddress);
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
                ddlModels.DataSource = oModel.GetLocation(_classid, _environmentid, _addressid, _typeid);
                try
                {
                    ddlModels.DataBind();
                }
                catch
                {
                    panModels.Visible = false;
                    oForecast.UpdateAnswerModel(intID, 0);
                    ddlTypes.SelectedValue = "0";
                }
                ddlModels.Items.Insert(0, new ListItem("-- SELECT --", "0"));
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
                intDone = 1;
                int intModel = Int32.Parse(ddlModels.SelectedItem.Value);
                oForecast.UpdateAnswerModel(intID, intModel);
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