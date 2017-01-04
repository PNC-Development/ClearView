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
    public partial class frame_solution_codes_location : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected Solution oSolution;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oSolution = new Solution(intProfile, dsn);
            lblFinish.Visible = false;
            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                lblFinish.Visible = true;
            if (!IsPostBack)
            {
                LoadLists();
                btnClose.Attributes.Add("onclick", "return HidePanel();");
            }
        }
        private void LoadLists()
        {
            ddlClass.DataValueField = "id";
            ddlClass.DataTextField = "name";
            ddlClass.DataSource = oClass.Gets(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, "-- SELECT --");
            ddlEnvironment.DataValueField = "id";
            ddlEnvironment.DataTextField = "name";
            ddlEnvironment.DataSource = oEnvironment.Gets(1);
            ddlEnvironment.DataBind();
            ddlEnvironment.Items.Insert(0, "-- SELECT --");
            ddlCode.DataValueField = "id";
            ddlCode.DataTextField = "code";
            ddlCode.DataSource = oSolution.GetCodes(1);
            ddlCode.DataBind();
            ddlCode.Items.Insert(0, "-- SELECT --");
        }
        protected  void btnAdd_Click(Object Sender, EventArgs e)
        {
            Locations oLocation = new Locations(intProfile, dsn);
            DataSet dsStates = oLocation.GetStates(1);
            foreach (DataRow drState in dsStates.Tables[0].Rows)
            {
                DataSet dsCitys = oLocation.GetCitys(Int32.Parse(drState["id"].ToString()), 1);
                foreach (DataRow drCity in dsCitys.Tables[0].Rows)
                {
                    DataSet dsAddress = oLocation.GetAddresss(Int32.Parse(drCity["id"].ToString()), 1);
                    foreach (DataRow drAddress in dsAddress.Tables[0].Rows)
                        oSolution.AddLocation(Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(ddlEnvironment.SelectedItem.Value), Int32.Parse(drAddress["id"].ToString()), Int32.Parse(ddlCode.SelectedItem.Value));
                }
            }
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
