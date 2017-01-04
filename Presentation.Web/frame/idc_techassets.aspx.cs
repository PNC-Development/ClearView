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
    public partial class idc_techassets : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Customized oCustom;
        protected Users oUser;
        protected Pages oPage;
        protected int intProfile;
        protected DataSet dsAsset;
        protected int intRequest;
        protected int intItem;
        protected int intNumber;
        protected int intId;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oCustom = new Customized(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);

            lblModified.Text = oUser.GetFullNameAD(intProfile, intEnvironment);
            lblUpdated.Text = DateTime.Now.ToString();

            if (Request.QueryString["save"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">window.top.refreshIE();<" + "/" + "script>");
            if (Request.QueryString["rid"] != "" && Request.QueryString["rid"] != null)
                intRequest = Int32.Parse(Request.QueryString["rid"]);
            if (Request.QueryString["iid"] != "" && Request.QueryString["iid"] != null)
                intItem = Int32.Parse(Request.QueryString["iid"]);
            if (Request.QueryString["num"] != "" && Request.QueryString["num"] != null)
                intNumber = Int32.Parse(Request.QueryString["num"]);
            if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                intId = Int32.Parse(Request.QueryString["id"]);

            if (!IsPostBack)
            {
                drpAssetType.DataSource = oCustom.GetIDCAssetTypes(1);
                drpAssetType.DataTextField = "name";
                drpAssetType.DataValueField = "id";
                drpAssetType.DataBind();
                drpAssetType.Items.Insert(0, "-- SELECT --");
                if (intId > 0)
                {
                    btnUpdate.Visible = true;
                    btnAdd.Visible = false;
                    dsAsset = oCustom.GetTechAsset(intId);
                    drpAssetType.SelectedValue = dsAsset.Tables[0].Rows[0]["asset_typeid"].ToString();
                    drpSaleStatus.SelectedValue = dsAsset.Tables[0].Rows[0]["salestatus"].ToString();
                    lblModified.Text = dsAsset.Tables[0].Rows[0]["lastmodified"].ToString();
                    lblUpdated.Text = dsAsset.Tables[0].Rows[0]["modified"].ToString();
                }
            }
            btnAdd.Attributes.Add("onclick", "return ValidateDropDown('" + drpAssetType.ClientID + "','Please make a selection for Asset Type')" +
              "&& ValidateDropDown('" + drpSaleStatus.ClientID + "','Please make a selection for Sale Status')" +
              ";");
            btnUpdate.Attributes.Add("onclick", "return ValidateDropDown('" + drpAssetType.ClientID + "','Please make a selection for Asset Type')" +
             "&& ValidateDropDown('" + drpSaleStatus.ClientID + "','Please make a selection for Sale Status')" +
             ";");

        }

        protected void AddAsset(object sender, CommandEventArgs e)
        {
            oCustom.AddTechAsset(intRequest, intItem, intNumber, Int32.Parse(drpAssetType.SelectedValue), drpAssetType.SelectedItem.Text, drpSaleStatus.SelectedItem.Text, lblModified.Text, Convert.ToDateTime(lblUpdated.Text));
            Response.Redirect(Request.Path + "?save=true");
        }

        protected void UpdateAsset(object sender, CommandEventArgs e)
        {
            oCustom.UpdateTechAsset(intId, Int32.Parse(drpAssetType.SelectedValue), drpAssetType.SelectedItem.Text, drpSaleStatus.SelectedItem.Text, lblModified.Text, Convert.ToDateTime(lblUpdated.Text));
            string url = Request.UrlReferrer.OriginalString + "&div=I";
            Response.Redirect(Request.Path + "?save=true");
        }
    }
}
