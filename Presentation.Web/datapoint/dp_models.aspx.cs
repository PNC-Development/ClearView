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
    public partial class dp_models : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intProfile = 0;
        protected DataPoint oDataPoint;
        protected Users oUser;
        protected Platforms oPlatform;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oDataPoint = new DataPoint(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            if (oUser.IsAdmin(intProfile) == true)
            {
                if (!IsPostBack)
                    LoadList();
                Variables oVariable = new Variables(intEnvironment);
                txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'400','195','" + divUser.ClientID + "','" + lstUser.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstUser.Attributes.Add("ondblclick", "AJAXClickRow();");
                ddlPlatform.Attributes.Add("onchange", "PopulatePlatformTypes('" + ddlPlatform.ClientID + "','" + ddlPlatformType.ClientID + "','" + ddlPlatformModel.ClientID + "',null);ResetDropDownHidden('" + hdnModel.ClientID + "');");
                ddlPlatformType.Attributes.Add("onchange", "PopulatePlatformModels('" + ddlPlatformType.ClientID + "','" + ddlPlatformModel.ClientID + "',null);ResetDropDownHidden('" + hdnModel.ClientID + "');");
                ddlPlatformModel.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlPlatformModel.ClientID + "','" + hdnModel.ClientID + "');");
                btnAdd.Attributes.Add("onclick", "return ValidateDropDown('" + ddlPlatform.ClientID + "','Please select a platform')" +
                    " && ValidateDropDown('" + ddlPlatformType.ClientID + "','Please select a type')" +
                    " && ValidateDropDown('" + ddlPlatformModel.ClientID + "','Please select a model')" +
                    " && ValidateHidden0('" + hdnUser.ClientID + "','" + txtUser.ClientID + "','Please enter and select a user')" +
                    " && ProcessButton(this)" +
                    ";");
            }
            else
            {
                btnAdd.Enabled = false;
                btnRemove.Enabled = false;
            }
        }
        private void LoadList()
        {
            lstCurrent.DataValueField = "id";
            lstCurrent.DataTextField = "name";
            lstCurrent.DataSource = oDataPoint.GetDeployModel();
            lstCurrent.DataBind();
            ddlPlatform.DataValueField = "platformid";
            ddlPlatform.DataTextField = "name";
            ddlPlatform.DataSource = oPlatform.Gets(1);
            ddlPlatform.DataBind();
            ddlPlatform.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (Request.Form[hdnUser.UniqueID] != "")
            {
                oDataPoint.AddDeployModel(Int32.Parse(Request.Form[hdnUser.UniqueID]), Int32.Parse(Request.Form[hdnModel.UniqueID]));
                Response.Redirect(Request.Path + "?save=true");
            }
            else
                Response.Redirect(Request.Path);
        }
        protected void btnRemove_Click(Object Sender, EventArgs e)
        {
            if (lstCurrent.SelectedIndex > -1)
            {
                oDataPoint.DeleteDeployModel(Int32.Parse(lstCurrent.Items[lstCurrent.SelectedIndex].Value));
                Response.Redirect(Request.Path + "?&delete=true");
            }
            else
                Response.Redirect(Request.Path);
        }
    }
}
