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
    public partial class frame_vmware_datacenters : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected int intProfile;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected Locations oLocation;
        protected VMWare oVMWare;
        protected int intID;
        protected int intParent = 0;
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
                    if (intID > 0)
                        lblName.Text = oVMWare.GetDatacenter(intID, "name");
                    intParent = Int32.Parse(oVMWare.GetDatacenter(intID, "virtualcenterid"));
                    rptLocations.DataSource = oVMWare.GetVirtualCentersID(intParent);
                    rptLocations.DataBind();
                    lblNone.Visible = (rptLocations.Items.Count == 0);
                    DataSet ds = oVMWare.GetDatacentersID(intID);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        foreach (RepeaterItem ri in rptLocations.Items)
                        {
                            Label lblLocation = (Label)ri.FindControl("lblLocation");
                            Label lblClass = (Label)ri.FindControl("lblClass");
                            Label lblEnvironment = (Label)ri.FindControl("lblEnvironment");
                            if (lblClass.Text == dr["classid"].ToString() && lblEnvironment.Text == dr["environmentid"].ToString() && lblLocation.Text == dr["addressid"].ToString())
                            {
                                CheckBox chkYes = (CheckBox)ri.FindControl("chkYes");
                                chkYes.Checked = true;
                            }
                        }
                    }
                }
            }
            else
                btnSave.Enabled = false;
        }
        protected  void btnSave_Click(Object Sender, EventArgs e)
        {
            oVMWare.DeleteDatacenters(intID);
            foreach (RepeaterItem ri in rptLocations.Items)
            {
                CheckBox chkYes = (CheckBox)ri.FindControl("chkYes");
                if (chkYes.Checked == true)
                {
                    Label lblLocation = (Label)ri.FindControl("lblLocation");
                    Label lblClass = (Label)ri.FindControl("lblClass");
                    Label lblEnvironment = (Label)ri.FindControl("lblEnvironment");
                    oVMWare.AddDatacenters(intID, Int32.Parse(lblClass.Text), Int32.Parse(lblEnvironment.Text), Int32.Parse(lblLocation.Text));
                }
            }
            Response.Redirect(Request.Url.PathAndQuery);
        }
    }
}
