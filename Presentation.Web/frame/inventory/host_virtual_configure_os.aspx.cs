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
    public partial class host_virtual_configure_os : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Asset oAsset;
        protected int intProfile;
        protected int intAsset = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oAsset = new Asset(intProfile, dsnAsset);
            if (Request.QueryString["save"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">alert('Host operating system added successfully!');<" + "/" + "script>");
            if (Request.QueryString["delete"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "delete", "<script type=\"text/javascript\">alert('Host operating system deleted successfully!');<" + "/" + "script>");
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intAsset = Int32.Parse(Request.QueryString["id"]);
            if (!IsPostBack)
            {
                if (intAsset > 0)
                {
                    LoadLists();
                    DataSet ds = oAsset.GetVirtualHostOs(intAsset);
                    rptOS.DataSource = ds;
                    rptOS.DataBind();
                    lblNone.Visible = (rptOS.Items.Count == 0);
                    foreach (RepeaterItem ri in rptOS.Items)
                    {
                        LinkButton btnDelete = (LinkButton)ri.FindControl("btnDelete");
                        btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                    }
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        foreach (ListItem oList in ddlOS.Items)
                        {
                            if (dr["osid"].ToString() == oList.Value)
                            {
                                ddlOS.Items.Remove(oList);
                                break;
                            }
                        }
                    }
                    btnSave.Attributes.Add("onclick", "return ValidateDropDown('" + ddlOS.ClientID + "','Please select an operating system')" +
                        " && ValidateText('" + txtVirtual.ClientID + "','Please enter a valid virtual directory')" +
                        " && ValidateText('" + txtGZip.ClientID + "','Please enter a valid gzip directory')" +
                        " && ValidateText('" + txtImage.ClientID + "','Please enter a valid image')" +
                        ";");
                }
                else
                    btnSave.Enabled = false;
            }
        }
        public void LoadLists()
        {
            OperatingSystems oOperatingSystem = new OperatingSystems(intProfile, dsn);
            ddlOS.DataValueField = "id";
            ddlOS.DataTextField = "name";
            ddlOS.DataSource = oOperatingSystem.Gets(1, 1);
            ddlOS.DataBind();
            ddlOS.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oAsset.AddVirtualHostOs(intAsset, Int32.Parse(ddlOS.SelectedItem.Value), txtVirtual.Text, txtGZip.Text, txtImage.Text);
            Response.Redirect(Request.Path + "?id=" + intAsset.ToString() + "&save=true");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)sender;
            oAsset.DeleteVirtualHostOs(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + intAsset.ToString() + "&delete=true");
        }
    }
}
