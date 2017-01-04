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
    public partial class asset_category_deployment_config : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected AssetCategory oAssetCategory;
        protected AssetCategoryDeploymentConfig oAssetCategoryDeploymentConfig;
        protected Services oService;
        protected StatusLevels oStatusLevels;
        protected int intProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/asset/asset_category_burnin_and_deployment_steps.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oAssetCategory = new AssetCategory(intProfile, dsn);
            oAssetCategoryDeploymentConfig = new AssetCategoryDeploymentConfig(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oStatusLevels = new StatusLevels(intProfile, dsn);
            if (!IsPostBack)
            {
                LoadList();

                if (Request.QueryString["AssetCategoryId"] != null && Request.QueryString["AssetCategoryId"] != "")
                {
                    ddlAssetCategory.SelectedValue = Request.QueryString["AssetCategoryId"].ToString();
                }

                btnUpdate.Attributes.Add("onclick", "return ValidateDropDown('" + ddlAssetCategory.ClientID + "','Please select asset category')" +
                                        " && confirm('Are you sure you want to update deployments steps ?')" +
                                        ";");
                btnService.Attributes.Add("onclick", "return OpenWindow('SERVICEBROWSER','" + hdnServiceId.ClientID + "','&control=" + hdnServiceId.ClientID + "&controltext=" + txtService.ClientID + "',false,400,600);");
                btnAddService.Attributes.Add("onclick", "return ValidateHidden0('" + hdnServiceId.ClientID + "','" + btnService.ClientID + "','Please select service');");
                PopulateConfigurations();
            }
        }

        private void LoadList()
        { 
            //Load the Asset Category

            ddlAssetCategory.DataTextField = "AssetCategoryName";
            ddlAssetCategory.DataValueField = "id";
            ddlAssetCategory.DataSource = oAssetCategory.Gets(1);
            ddlAssetCategory.DataBind();
            ddlAssetCategory.Items.Insert(0, new ListItem("-- Select --", "0"));

        }
              
        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            foreach (DataListItem dlItem in dlConfigSteps.Items)
            {
                HiddenField hdnConfigStepId = (HiddenField)dlItem.FindControl("hdnConfigStepId");
                HiddenField hdnConfigStepServiceId = (HiddenField)dlItem.FindControl("hdnConfigStepServiceId");
                Label lblConfigStepService = (Label)dlItem.FindControl("lblConfigStepService");
                CheckBox chkConfigStepProcure = (CheckBox)dlItem.FindControl("chkConfigStepProcure");
                CheckBox chkConfigStepReDeploy = (CheckBox)dlItem.FindControl("chkConfigStepReDeploy");
                CheckBox chkConfigStepMovement = (CheckBox)dlItem.FindControl("chkConfigStepMovement");
                CheckBox chkConfigStepDispose = (CheckBox)dlItem.FindControl("chkConfigStepDispose");
                CheckBox chkConfigStepStatusChange = (CheckBox)dlItem.FindControl("chkConfigStepStatusChange");
                DropDownList ddlConfigStepAssetStatusIn = (DropDownList)dlItem.FindControl("ddlConfigStepAssetStatusIn");
                DropDownList ddlConfigStepAssetStatusOut = (DropDownList)dlItem.FindControl("ddlConfigStepAssetStatusOut");
                TextBox txtConfigStepCustomName = (TextBox)dlItem.FindControl("txtConfigStepCustomName");
                CheckBox chkConfigStepEnabled = (CheckBox)dlItem.FindControl("chkConfigStepEnabled");

                oAssetCategoryDeploymentConfig.update(
                                                Int32.Parse(hdnConfigStepId.Value),
                                                Int32.Parse(ddlAssetCategory.SelectedValue),
                                                Int32.Parse(hdnConfigStepServiceId.Value),
                                                (chkConfigStepProcure.Checked?1:0),
                                                (chkConfigStepReDeploy.Checked?1:0),
                                                (chkConfigStepMovement.Checked?1:0),
                                                (chkConfigStepDispose.Checked?1:0),
                                                (chkConfigStepStatusChange.Checked?1:0),
                                                Int32.Parse(ddlConfigStepAssetStatusIn.SelectedValue),
                                                Int32.Parse(ddlConfigStepAssetStatusOut.SelectedValue),
                                                txtConfigStepCustomName.Text.Trim(),
                                                dlItem.ItemIndex,
                                                (chkConfigStepEnabled.Checked?1:0),intProfile);
                    
            }

            Response.Redirect(Request.Path + "?AssetCategoryId=" + ddlAssetCategory.SelectedValue);    

        }

        protected void dlConfigSteps_ItemDataBound(object sender, DataListItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                ImageButton imgbtnConfigStepDelete = (ImageButton)e.Item.FindControl("imgbtnConfigStepDelete");
                HiddenField hdnConfigStepId = (HiddenField)e.Item.FindControl("hdnConfigStepId");
                HiddenField hdnConfigStepServiceId = (HiddenField)e.Item.FindControl("hdnConfigStepServiceId");
                Label lblConfigStepService = (Label)e.Item.FindControl("lblConfigStepService");
                CheckBox chkConfigStepProcure = (CheckBox)e.Item.FindControl("chkConfigStepProcure");
                CheckBox chkConfigStepReDeploy = (CheckBox)e.Item.FindControl("chkConfigStepReDeploy");
                CheckBox chkConfigStepMovement = (CheckBox)e.Item.FindControl("chkConfigStepMovement");
                CheckBox chkConfigStepDispose = (CheckBox)e.Item.FindControl("chkConfigStepDispose");
                CheckBox chkConfigStepStatusChange = (CheckBox)e.Item.FindControl("chkConfigStepStatusChange");
                DropDownList ddlConfigStepAssetStatusIn = (DropDownList)e.Item.FindControl("ddlConfigStepAssetStatusIn");
                DropDownList ddlConfigStepAssetStatusOut = (DropDownList)e.Item.FindControl("ddlConfigStepAssetStatusOut");
                TextBox txtConfigStepCustomName = (TextBox)e.Item.FindControl("txtConfigStepCustomName");
                CheckBox chkConfigStepEnabled = (CheckBox)e.Item.FindControl("chkConfigStepEnabled");


                ddlConfigStepAssetStatusIn.DataTextField = "StatusDescription";
                ddlConfigStepAssetStatusIn.DataValueField = "StatusValue";
                ddlConfigStepAssetStatusIn.DataSource = oStatusLevels.GetStatusList("ASSETSTATUS");
                ddlConfigStepAssetStatusIn.DataBind();
                ddlConfigStepAssetStatusIn.Items.Insert(0, new ListItem("-- Select --", "-999"));

                ddlConfigStepAssetStatusOut.DataTextField = "StatusDescription";
                ddlConfigStepAssetStatusOut.DataValueField = "StatusValue";
                ddlConfigStepAssetStatusOut.DataSource = oStatusLevels.GetStatusList("ASSETSTATUS");
                ddlConfigStepAssetStatusOut.DataBind();
                ddlConfigStepAssetStatusOut.Items.Insert(0, new ListItem("-- Select --", "-999"));

                


                imgbtnConfigStepDelete.CommandArgument = drv["Id"].ToString();
                imgbtnConfigStepDelete.CommandName = "DELETESTEP";
                imgbtnConfigStepDelete.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this service?');");

                hdnConfigStepId.Value = drv["Id"].ToString();
                hdnConfigStepServiceId.Value = drv["ServiceId"].ToString();
                lblConfigStepService.Text = drv["ServiceName"].ToString();
                chkConfigStepProcure.Checked = (drv["IsRequiredForProcurement"].ToString() == "1"?true:false);
                chkConfigStepReDeploy.Checked = (drv["IsRequiredForReDeployment"].ToString() == "1" ? true : false);
                chkConfigStepMovement.Checked = (drv["IsRequiredForMovement"].ToString() == "1" ? true : false);
                chkConfigStepDispose.Checked = (drv["IsRequiredForDispose"].ToString() == "1" ? true : false);
                chkConfigStepStatusChange.Checked = (drv["IsAssetStatusChangeApplicable"].ToString() == "1" ? true : false);
                ddlConfigStepAssetStatusIn.SelectedValue = drv["AssetStatusIn"].ToString();
                ddlConfigStepAssetStatusOut.SelectedValue = drv["AssetStatusOut"].ToString();
                txtConfigStepCustomName.Text = drv["CustomTaskName"].ToString();
                chkConfigStepEnabled.Checked = (drv["Enabled"].ToString() == "1" ? true : false);

              

            }

        }

        protected void dlConfigSteps_Command(object sender, DataListCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "DELETESTEP")
            {
                oAssetCategoryDeploymentConfig.delete(Int32.Parse(e.CommandArgument.ToString()), intProfile);
                Response.Redirect(Request.Path + "?AssetCategoryId=" + ddlAssetCategory.SelectedValue);    
            }
            
        }

        protected void ddlAssetCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateConfigurations();
        }

        protected void btnAddService_Click(object sender, EventArgs e)
        {
                oAssetCategoryDeploymentConfig.add(
                            Int32.Parse(ddlAssetCategory.SelectedValue),
                            Int32.Parse(hdnServiceId.Value),
                            0, 0, 0, 0, 0, 0, 0, "", 0, 1, intProfile);

                Response.Redirect(Request.Path + "?AssetCategoryId=" + ddlAssetCategory.SelectedValue);    
        }

        protected void PopulateConfigurations()
        { 
            dlConfigSteps.DataSource = oAssetCategoryDeploymentConfig.gets(Int32.Parse(ddlAssetCategory.SelectedValue), null).Tables[0];
            dlConfigSteps.DataBind();
            lblNoResults.Visible = (dlConfigSteps.Items.Count>0?false:true);
            
        
        }
    }
}
