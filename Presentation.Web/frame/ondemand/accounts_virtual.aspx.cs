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
    public partial class accounts_virtual : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnRemote = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["RemoteDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected int intProfile;
        protected Workstations oWorkstation;
        protected Workstations oRemote;
        protected Users oUser;
        protected Variables oVariable;
        protected OnDemand oOnDemand;
        protected Forecast oForecast;
        protected Classes oClass;
        protected int intAsset = 0;
        protected int intRemote = 0;
        protected int intAnswer = 0;
        protected int intStep = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            intProfile = 0;
            oWorkstation = new Workstations(intProfile, dsn);
            oRemote = new Workstations(intProfile, dsnRemote);
            oUser = new Users(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oOnDemand = new OnDemand(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            string strUsers = "";
          
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                int intWorkstation = Int32.Parse(Request.QueryString["id"]);
                DataSet ds = oWorkstation.GetVirtual(intWorkstation);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intStep = Int32.Parse(ds.Tables[0].Rows[0]["step"].ToString());
                    intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                    intAsset = Int32.Parse(ds.Tables[0].Rows[0]["assetid"].ToString());
                    intRemote = Int32.Parse(ds.Tables[0].Rows[0]["remoteid"].ToString());
                    int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
                    int intDomain = Int32.Parse(ds.Tables[0].Rows[0]["domainid"].ToString());
                    Domains oDomain = new Domains(intProfile, dsn);
                    lblDomain.Text = oDomain.Get(intDomain, "name");
                    intDomain = Int32.Parse(oDomain.Get(intDomain, "environment"));
                    int intName = Int32.Parse(ds.Tables[0].Rows[0]["nameid"].ToString());
                    lblWorkstation.Text = oWorkstation.GetName(intName);
                    txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                    lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
                    DataSet dsAccounts = oWorkstation.GetAccountsVirtual(intAsset);
                    rptAccounts.DataSource = dsAccounts;
                    rptAccounts.DataBind();
                    foreach (RepeaterItem ri in rptAccounts.Items)
                    {
                        LinkButton _delete = (LinkButton)ri.FindControl("btnDelete");
                        _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this account?');");
                    }
                    foreach (DataRow drAccount in dsAccounts.Tables[0].Rows)
                        strUsers += oUser.GetName(Int32.Parse(drAccount["userid"].ToString())) + ";";
                    if (rptAccounts.Items.Count == 0)
                    {
                        lblNone.Visible = true;
                        btnSubmit.Attributes.Add("onclick", "alert('You must add at least one account or select the skip button');return false;");
                    }
                    if (oClass.IsProd(intClass))
                    {
                      
                        panProduction.Visible = true;
                    }
                    else
                        panAdmin.Visible = true;
                }
            }
            else
            {
                btnAdd.Enabled = false;
                btnSubmit.Enabled = false;
                btnSkip.Enabled = false;
            }
            btnSkip.Enabled = false;
            //btnSkip.Attributes.Add("onclick", "return confirm('Are you sure you want to skip the account configuration process?');");
            btnAdd.Attributes.Add("onclick", "return ValidateHidden('" + hdnUser.ClientID + "','" + txtUser.ClientID + "','Please enter a username, first name or last name') && EnsureAccenture('" + hdnUser.ClientID + "','" + strUsers + "');");
            //btnAdd.Attributes.Add("onclick", "return ValidateHidden('" + hdnUser.ClientID + "','" + txtUser.ClientID + "','Please enter a username, first name or last name')" + (boolProduction == true ? " && EnsureAccenture('" + hdnUser.ClientID + "','" + strUsers + "')" : "") + ";");
            btnManager.Attributes.Add("onclick", "return OpenWindow('NEW_USER','');");
        }

        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intUser = Int32.Parse(Request.Form[hdnUser.UniqueID]);
            oWorkstation.AddAccountFix(intAsset, 0, intUser, (chkAdmin.Checked ? 1 : 0), (chkRemote.Checked ? 1 : 0));
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&add=true");
        }

        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oWorkstation.DeleteAccount(Int32.Parse(oDelete.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&delete=true");
        }

        protected void btnSkip_Click(Object Sender, EventArgs e)
        {
            int intWorkstation = Int32.Parse(Request.QueryString["id"]);
            oOnDemand.UpdateStepDoneWorkstation(intWorkstation, intStep, "Accounts were skipped", 0, false, false);
            oWorkstation.NextVirtualStep(intWorkstation);
            oRemote.NextRemoteVirtual(intRemote);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.opener.location.reload();window.close();<" + "/" + "script>");
        }

        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            // Copy to remote system
            int intWorkstation = Int32.Parse(Request.QueryString["id"]);
            DataSet dsAccounts = oWorkstation.GetAccountsVirtual(intAsset);
            foreach (DataRow drAccount in dsAccounts.Tables[0].Rows)
            {
                oRemote.AddRemoteAccount(lblWorkstation.Text, oUser.GetName(Int32.Parse(drAccount["userid"].ToString())), Int32.Parse(drAccount["admin"].ToString()), Int32.Parse(drAccount["remote"].ToString()));
                oWorkstation.UpdateAccount(Int32.Parse(drAccount["id"].ToString()));
            }
            oOnDemand.UpdateStepDoneWorkstation(intWorkstation, intStep, "Accounts were configured", 0, false, false);
            oWorkstation.NextVirtualStep(intWorkstation);
            oRemote.NextRemoteVirtual(intRemote);
            // Close Window and Update Parent Screen
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.opener.location.reload();window.close();<" + "/" + "script>");
        }
    }
}
