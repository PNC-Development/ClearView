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
    public partial class costavoidance_mapping : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Customized oCustomized;
        protected Pages oPage;
        protected int intProfile;
        protected int intPage;
        protected int intId;

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oCustomized = new Customized(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);

            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intId = Int32.Parse(Request.QueryString["id"]);

            if (!IsPostBack)
            {
                drpCategory.DataSource = oCustomized.GetCategories(0);
                drpCategory.DataTextField = "name";
                drpCategory.DataValueField = "id";
                drpCategory.DataBind();
                drpCategory.Items.Insert(0, new ListItem("--SELECT--"));

                drpItem.DataSource = oCustomized.GetItems(0);
                drpItem.DataTextField = "name";
                drpItem.DataValueField = "id";
                drpItem.DataBind();
                drpItem.Items.Insert(0, new ListItem("--SELECT--"));

                rptView.DataSource = oCustomized.GetCategoryList(intId);
                rptView.DataBind();

            }

            foreach (RepeaterItem ri in rptView.Items)
            {
                LinkButton oEdit = (LinkButton)ri.FindControl("btnEdit");
                Panel panEdit = (Panel)ri.FindControl("panEdit");
                panEdit.Visible = true;
                LinkButton oDelete = (LinkButton)ri.FindControl("btnDelete");
                oDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item ?');");
            }


            lblNone.Visible = rptView.Items.Count == 0;
            drpItem.Attributes.Add("onchange", "AjaxGetItemAmount('" + drpItem.ClientID + "','" + lblAmt.ClientID + "','');");
            btnAdd.Attributes.Add("onclick", "return ValidateDropDown('" + drpCategory.ClientID + "','Please select a category')" +
                        " && ValidateDropDown('" + drpItem.ClientID + "','Please select an item')" +
                        ";");
            //btnUpdate.Attributes.Add("onclick", "return ValidateDropDown('" + drpCategory.ClientID + "','Please select a category')" +
            //            " && ValidateDropDown('" + drpItem.ClientID + "','Please select an item')" +
            //            ";");

        }

        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            //   SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "insert into vijay_category_list values(0," + Int32.Parse(drpCategory.SelectedValue) + "," + Int32.Parse(drpItem.SelectedValue) + ","+intProfile +",'" + DateTime.Now + "');");
            if (Request.Form[hdnVal.UniqueID] == "Update")
                oCustomized.UpdateCategoryList(Int32.Parse(Request.Form[hdnId.UniqueID]), Int32.Parse(drpCategory.SelectedValue), Int32.Parse(drpItem.SelectedValue));
            else
                oCustomized.AddCategoryList(intId, Int32.Parse(drpCategory.SelectedValue), Int32.Parse(drpItem.SelectedValue), intProfile);
            Response.Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)sender;
            int intId = Int32.Parse(oButton.CommandArgument);
            oCustomized.DeleteCategoryList(intId);
            Response.Redirect(Request.UrlReferrer.AbsoluteUri);
            //ClientScript.RegisterClientScriptBlock(typeof(Page), "redirect", "window.navigate(window.location);", true);
        }
    }
}
