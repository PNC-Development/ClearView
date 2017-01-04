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
    public partial class fore_vendor : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intProfile;
        protected Forecast oForecast;
        protected Types oType;
        protected Models oModel;
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
            int intAddress = 0;
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
                    int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
                    int intVendor = Int32.Parse(ds.Tables[0].Rows[0]["vendorid"].ToString());
                    intForecast = Int32.Parse(ds.Tables[0].Rows[0]["forecastid"].ToString());
                    DataSet dsSteps = oForecast.GetSteps(intPlatform, 1);
                    if (dsSteps.Tables[0].Rows.Count == intStep)
                        btnNext.Text = "Finish";
                    if (intStep == 0 || intStep == 1)
                        btnBack.Enabled = false;
                    lblVendor.Text = intVendor.ToString();
                    if (Request.QueryString["type"] != null && Request.QueryString["type"] != "")
                        intType = Int32.Parse(Request.QueryString["type"]);
                    else if (!IsPostBack)
                    {
                        if (ds.Tables[0].Rows[0]["vplatform"].ToString() != "")
                            intType = Int32.Parse(ds.Tables[0].Rows[0]["vplatform"].ToString());
                        intAddress = Int32.Parse(ds.Tables[0].Rows[0]["addressid"].ToString());
                        txtMake.Text = ds.Tables[0].Rows[0]["make"].ToString();
                        txtModel.Text = ds.Tables[0].Rows[0]["modelname"].ToString();
                        txtOther.Text = ds.Tables[0].Rows[0]["description"].ToString();
                        txtWidth.Text = ds.Tables[0].Rows[0]["size_w"].ToString();
                        txtHeight.Text = ds.Tables[0].Rows[0]["size_h"].ToString();
                        txtAmp.Text = ds.Tables[0].Rows[0]["amp"].ToString();
                        ddlTypes.SelectedValue = ds.Tables[0].Rows[0]["typeid"].ToString();
                    }
                    if (!IsPostBack)
                        intCount = LoadPlatform(intPlatform, intType);
                }
            }
            btnClose.Attributes.Add("onclick", "return window.close();");
            btnNext.Attributes.Add("onclick", "return ValidateDropDown('" + ddlPlatforms.ClientID + "','Please select a classification')" +
                " && ValidateDropDown('" + ddlTypes.ClientID + "','Please select a type')" +
                " && ValidateText('" + txtMake.ClientID + "','Please enter the make')" +
                " && ValidateText('" + txtModel.ClientID + "','Please enter the model')" +
                " && ValidateText('" + txtWidth.ClientID + "','Please enter the width')" +
                " && ValidateText('" + txtHeight.ClientID + "','Please enter the height')" +
                " && ValidateNumber('" + txtAmp.ClientID + "','Please enter the number of AMPs')" +
                ";");
            btnUpdate.Attributes.Add("onclick", "return ValidateDropDown('" + ddlPlatforms.ClientID + "','Please select a classification')" +
                " && ValidateDropDown('" + ddlTypes.ClientID + "','Please select a type')" +
                " && ValidateText('" + txtMake.ClientID + "','Please enter the make')" +
                " && ValidateText('" + txtModel.ClientID + "','Please enter the model')" +
                " && ValidateText('" + txtWidth.ClientID + "','Please enter the width')" +
                " && ValidateText('" + txtHeight.ClientID + "','Please enter the height')" +
                " && ValidateNumber('" + txtAmp.ClientID + "','Please enter the number of AMPs')" +
                ";");
        }
        protected int LoadPlatform(int _platformid, int _typeid)
        {
            int intCount = 0;
            ddlPlatforms.DataTextField = "name";
            ddlPlatforms.DataValueField = "platformid";
            DataSet ds = oPlatform.Gets(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["name"].ToString().ToUpper() == "VENDOR")
                    dr.Delete();
            }
            ddlPlatforms.DataSource = ds;
            ddlPlatforms.DataBind();
            ddlPlatforms.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            if (_typeid > 0)
            {
                ddlPlatforms.SelectedValue = _typeid.ToString();
                panTypes.Visible = true;
                ddlTypes.DataTextField = "name";
                ddlTypes.DataValueField = "id";
                ddlTypes.DataSource = oType.Gets(_typeid, 1);
                ddlTypes.DataBind();
                ddlTypes.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            }
            return intCount;
        }
        protected void ddlPlatforms_Change(Object Sender, EventArgs e)
        {
            if (ddlPlatforms.SelectedIndex > 0)
                Response.Redirect(Request.Path + "?parent=" + intForecast + "&id=" + intID + "&type=" + ddlPlatforms.SelectedItem.Value + "&step=" + Request.QueryString["step"]);
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
            int intVendor = Int32.Parse(lblVendor.Text);
            if (intVendor == 0)
                intVendor = oForecast.AddAnswerVendor(Int32.Parse(ddlPlatforms.SelectedItem.Value), Int32.Parse(ddlTypes.SelectedItem.Value), txtMake.Text, txtModel.Text, txtWidth.Text, txtHeight.Text, txtAmp.Text, txtOther.Text);
            else
                oForecast.UpdateAnswerVendor(intVendor, Int32.Parse(ddlPlatforms.SelectedItem.Value), Int32.Parse(ddlTypes.SelectedItem.Value), txtMake.Text, txtModel.Text, txtWidth.Text, txtHeight.Text, txtAmp.Text, txtOther.Text);
            oForecast.UpdateAnswerVendor(intID, intVendor);
            // Check to see if step is done
            int intDone = 0;
            if (txtMake.Text.Trim() != "" && txtModel.Text.Trim() != "" && txtWidth.Text.Trim() != "" && txtHeight.Text.Trim() != "" && txtAmp.Text.Trim() != "")
                intDone = 1;
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