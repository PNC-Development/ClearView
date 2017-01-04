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
    public partial class model_thresholds : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected ModelsProperties oModelsProperties;
        protected int intModel = 0;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            oModelsProperties = new ModelsProperties(0, dsn);
            if (Request.QueryString["mid"] != null && Request.QueryString["mid"] != "")
            {
                intModel = Int32.Parse(Request.QueryString["mid"]);
                if (intModel > 0)
                    lblName.Text = oModelsProperties.Get(intModel, "name");
            }
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intID = Int32.Parse(Request.QueryString["id"]);
                if (IsPostBack == false)
                {
                    panOne.Visible = true;
                    DataSet ds = oModelsProperties.GetThreshold(intID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtFrom.Text = ds.Tables[0].Rows[0]["qty_from"].ToString();
                        txtTo.Text = ds.Tables[0].Rows[0]["qty_to"].ToString();
                        txtNumber.Text = ds.Tables[0].Rows[0]["number_days"].ToString();
                        chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnSubmit.Text = "Update";
                    }
                    else
                        btnSubmit.Text = "Add";
                }
            }
            else if (intModel > 0)
            {
                panAll.Visible = true;
                rptView.DataSource = oModelsProperties.GetThresholds(intModel, 0);
                rptView.DataBind();
                lblNone.Visible = (rptView.Items.Count == 0);
            }
            btnSubmit.Attributes.Add("onclick", "return ValidateNumber0('" + txtFrom.ClientID + "','Please enter a valid number for the quantity')" +
                " && ValidateNumber0('" + txtTo.ClientID + "','Please enter a valid number for the quantity')" +
                " && ValidateNumber0('" + txtNumber.ClientID + "','Please enter a valid number for the number of days')" +
                ";");
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            if (btnSubmit.Text == "Add")
                oModelsProperties.AddThreshold(intModel, Int32.Parse(txtFrom.Text), Int32.Parse(txtTo.Text), Int32.Parse(txtNumber.Text), (chkEnabled.Checked ? 1 : 0));
            else
                oModelsProperties.UpdateThreshold(intID, Int32.Parse(txtFrom.Text), Int32.Parse(txtTo.Text), Int32.Parse(txtNumber.Text), (chkEnabled.Checked ? 1 : 0));
            Response.Redirect(Request.Path + "?mid=" + intModel.ToString());
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?mid=" + intModel.ToString());
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oModelsProperties.EnableThreshold(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path + "?mid=" + intModel.ToString());
        }
    }
}
