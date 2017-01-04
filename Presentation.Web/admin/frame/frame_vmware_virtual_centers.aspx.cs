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
    public partial class frame_vmware_virtual_centers : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected int intProfile;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected Locations oLocation;
        protected VMWare oVMWare;
        protected int intID;
        protected string strLocation = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            {
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
                oClass = new Classes(intProfile, dsn);
                oEnvironment = new Environments(intProfile, dsn);
                oLocation = new Locations(intProfile, dsn);
                oVMWare = new VMWare(intProfile, dsn);
                btnClose.Attributes.Add("onclick", "return HidePanel();");
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                    intID = Int32.Parse(Request.QueryString["id"]);
                if (!IsPostBack)
                {
                    LoadLists();
                    if (intID > 0)
                        lblName.Text = oVMWare.GetVirtualCenter(intID, "name");
                    strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, intLocation, true, "ddlCommon");
                    hdnLocation.Value = intLocation.ToString();
                    ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',0);");
                    ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
                    rptLocations.DataSource = oVMWare.GetVirtualCentersID(intID);
                    rptLocations.DataBind();
                    lblNone.Visible = (rptLocations.Items.Count == 0);
                    foreach (RepeaterItem ri in rptLocations.Items)
                        ((LinkButton)ri.FindControl("btnDelete")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                }
            }
            else
                btnAdd.Enabled = false;
        }
        private void LoadLists()
        {
            ddlClass.DataTextField = "name";
            ddlClass.DataValueField = "id";
            ddlClass.DataSource = oClass.Gets(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            oVMWare.AddVirtualCenters(intID, Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), Int32.Parse(Request.Form[hdnLocation.UniqueID]));
            Response.Redirect(Request.Url.PathAndQuery);
        }
        protected  void btnDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oVMWare.DeleteVirtualCenters(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Url.PathAndQuery);
        }
    }
}
