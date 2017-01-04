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
    public partial class costavoidance_view : System.Web.UI.UserControl
    {

        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;

        protected Customized oCustomized;
        protected Pages oPage;
        protected Users oUser;
        protected int intPage;
        protected int intProfile;
        protected int intId;

        protected void Page_Load(object sender, EventArgs e)
        {

            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oCustomized = new Customized(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intId = Int32.Parse(Request.QueryString["id"]);

            if (intId > 0)
            {
                DataSet ds = oCustomized.GetCostAvoidanceById(intId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblTitle.Text = "View Cost Avoidance";
                    lblCAO.Text = ds.Tables[0].Rows[0]["opportunity"].ToString();
                    lblDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                    if (ds.Tables[0].Rows[0]["path"].ToString() != "")
                    {
                        hypUpload.NavigateUrl = ds.Tables[0].Rows[0]["path"].ToString();
                    }
                    else
                        hypUpload.Text = "";
                    string strVal = ds.Tables[0].Rows[0]["addtlcostavoidance"].ToString();
                    strVal = strVal == "" ? "0" : strVal;
                    lblAddtlCA.Text = String.Format("{0:C}", Double.Parse(strVal));
                    lblDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["date"].ToString()).ToShortDateString();
                    lblSubmitter.Text = oUser.GetFullName(Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString()));
                    lblDateSubmit.Text = DateTime.Parse(ds.Tables[0].Rows[0]["created"].ToString()).ToShortDateString();
                    rptView.DataSource = oCustomized.GetCategoryList(intId);
                    rptView.DataBind();
                    lblNone.Visible = rptView.Items.Count == 0;
                }
            }

        }   
    }
}