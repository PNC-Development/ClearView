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
    public partial class hba : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected Asset oAsset;
        protected IPAddresses oIPAddresses;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected int intProfile;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oAsset = new Asset(0, dsnAsset);
            oIPAddresses = new IPAddresses(0, dsnIP, dsn);
            oModel = new Models(0, dsn);
            oModelsProperties = new ModelsProperties(0, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["add"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "add", "<script type=\"text/javascript\">alert('The HBA was successfully added');<" + "/" + "script>");
            if (Request.QueryString["delete"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "delete", "<script type=\"text/javascript\">alert('The HBA was successfully deleted');<" + "/" + "script>");
            if (!IsPostBack)
            {
                DataSet ds = oAsset.Get(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblTracking.Text = ds.Tables[0].Rows[0]["tracking"].ToString();
                    lblSerial.Text = ds.Tables[0].Rows[0]["serial"].ToString();
                    int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
                    intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                    lblModel.Text = ds.Tables[0].Rows[0]["modelname"].ToString();
                    lblAsset.Text = ds.Tables[0].Rows[0]["asset"].ToString();
                    rptHBA.DataSource = oAsset.GetHBA(intID);
                    rptHBA.DataBind();
                    lblHBA.Visible = (rptHBA.Items.Count == 0);
                    foreach (RepeaterItem ri in rptHBA.Items)
                    {
                        LinkButton btnDelete = (LinkButton)ri.FindControl("btnDelete");
                        btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                    }
                    btnAdd.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a name')" +
                        ";");
                    txtName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnAdd.ClientID + "').click();return false;}} else {return true}; ");
                }
                else
                    btnAdd.Enabled = false;
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)sender;
            oAsset.DeleteHBA(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&delete=true");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            oAsset.AddHBA(intID, txtName.Text);
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&add=true");
        }
    }
}
