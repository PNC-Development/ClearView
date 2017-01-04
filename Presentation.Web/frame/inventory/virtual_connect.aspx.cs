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
    public partial class virtual_connect : BasePage
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
        protected int intEdit = 0;
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
            if (Request.QueryString["edit"] != null && Request.QueryString["edit"] != "")
                intEdit = Int32.Parse(Request.QueryString["edit"]);
            if (Request.QueryString["did"] != null && Request.QueryString["did"] != "")
            {
                int intDelete = Int32.Parse(Request.QueryString["did"]);
                int intParent = Int32.Parse(oAsset.GetEnclosureVC(intDelete, "enclosureid"));
                oAsset.DeleteEnclosureVC(intDelete);
                Response.Redirect(Request.Path + "?id=" + intParent.ToString() + "&delete=true");
            }
            if (Request.QueryString["add"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "add", "<script type=\"text/javascript\">alert('The Virtual Connect IP was successfully added');<" + "/" + "script>");
            if (Request.QueryString["update"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "update", "<script type=\"text/javascript\">alert('The Virtual Connect IP was updated added');<" + "/" + "script>");
            if (Request.QueryString["delete"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "delete", "<script type=\"text/javascript\">alert('The Virtual Connect IP was successfully deleted');<" + "/" + "script>");
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
                    if (intEdit > 0)
                    {
                        btnAdd.Text = "Update";
                        btnCancel.Visible = true;
                        DataSet dsVC = oAsset.GetEnclosureVC(intEdit);
                        if (dsVC.Tables[0].Rows.Count > 0)
                        {
                            txtVirtualConnect.Text = dsVC.Tables[0].Rows[0]["virtual_connect"].ToString();
                            btnSave.Enabled = false;
                            imgOrderUp.Enabled = false;
                            imgOrderDown.Enabled = false;
                            imgRemove.Enabled = false;
                            lstCurrent.Enabled = false;
                        }
                        else
                            btnAdd.Enabled = false;
                    }
                    else
                    {
                        imgOrderUp.Attributes.Add("onclick", "return MoveOrderUp(" + lstCurrent.ClientID + ",'" + hdnOrder.ClientID + "');");
                        imgOrderDown.Attributes.Add("onclick", "return MoveOrderDown(" + lstCurrent.ClientID + ",'" + hdnOrder.ClientID + "');");
                        imgRemove.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?') && MoveOut(" + lstCurrent.ClientID + ");");
                        lstCurrent.Attributes.Add("ondblclick", "return EditVC(this);");
                        btnAdd.Attributes.Add("onclick", "return ValidateText('" + txtVirtualConnect.ClientID + "','Please enter a Virtual Connect IP address')" +
                            ";");
                        txtVirtualConnect.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnAdd.ClientID + "').click();return false;}} else {return true}; ");
                        lstCurrent.DataTextField = "virtual_connect";
                        lstCurrent.DataValueField = "id";
                        lstCurrent.DataSource = oAsset.GetEnclosureVCs(intID, 1);
                        lstCurrent.DataBind();
                    }
                }
                else
                    btnAdd.Enabled = false;
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            string strOrder = Request.Form[hdnOrder.UniqueID];
            int intDisplay = 0;
            while (strOrder != "")
            {
                intDisplay++;
                int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                oAsset.UpdateEnclosureVCOrder(intId, intDisplay);
            }
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&update=true");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (intEdit == 0)
            {
                oAsset.AddEnclosureVC(intID, txtVirtualConnect.Text, (oAsset.GetEnclosureVCs(intID, 1).Tables[0].Rows.Count + 1), 1);
                Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&add=true");
            }
            else
            {
                oAsset.UpdateEnclosureVC(intEdit, txtVirtualConnect.Text, 1);
                Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&update=true");
            }
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
    }
}
