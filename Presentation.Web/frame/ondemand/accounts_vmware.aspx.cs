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
using System.DirectoryServices;

namespace NCC.ClearView.Presentation.Web
{
    public partial class accounts_vmware : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected int intProfile;
        protected Users oUser;
        protected Variables oVariable;
        protected Functions oFunction;
        protected Forecast oForecast;
        protected Workstations oWorkstation;
        protected OnDemand oOnDemand;
        protected int intWorkstation = 0;
        protected int intDomain = 0;
        protected int intRequest = 0;
        protected int intAnswer = 0;
        protected int intStep = 0;
        protected int intInternal = 0;
        protected int intQuantity = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "ClearView Account Configuration";
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oUser = new Users(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(0, dsn, intEnvironment);
            oForecast = new Forecast(intProfile, dsn);
            oWorkstation = new Workstations(intProfile, dsn);
            oOnDemand = new OnDemand(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intWorkstation = Int32.Parse(Request.QueryString["id"]);
                Page.Title = "ClearView Account Configuration | Workstation # " + intWorkstation.ToString();
                DataSet ds = oWorkstation.GetVirtual(intWorkstation);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Request.QueryString["userid"] != null && Request.QueryString["userid"] != "")
                    {
                        int intUser = Int32.Parse(Request.QueryString["userid"]);
                        trUpdate.Visible = true;
                        lblXID.Text = oUser.GetFullName(intUser) + " (" + oUser.GetName(intUser) + ")";
                        btnAdd.Text = "Update";
                        btnCancel.Visible = true;
                    }
                    else
                        trNew.Visible = true;
                    intStep = Int32.Parse(ds.Tables[0].Rows[0]["step"].ToString());
                    intInternal = Int32.Parse(ds.Tables[0].Rows[0]["internal"].ToString());
                    intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                    int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                    DataSet dsAnswer = oForecast.GetAnswer(intAnswer);
                    if (dsAnswer.Tables[0].Rows.Count > 0)
                    {
                        intQuantity = Int32.Parse(dsAnswer.Tables[0].Rows[0]["quantity"].ToString());
                        chkApply.Visible = (intQuantity > 1 && intNumber == 1);
                    }
                    intRequest = oForecast.GetRequestID(intAnswer, true);
                    intDomain = Int32.Parse(ds.Tables[0].Rows[0]["domainid"].ToString());
                    Domains oDomain = new Domains(intProfile, dsn);
                    int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
                    Classes oClass = new Classes(intProfile, dsn);
                    if (oForecast.GetAnswer(intAnswer, "test") == "1")
                        intDomain = Int32.Parse(ds.Tables[0].Rows[0]["test_domainid"].ToString());
                    lblDomain.Text = oDomain.Get(intDomain, "name");
                    Requests oRequest = new Requests(intProfile, dsn);
                    //if (oRequest.GetUser(intRequest) == intProfile)
                    //{
                    panPermit.Visible = true;
                    txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                    lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
                    chkAdmin.Attributes.Add("onclick", "CheckAdmin(this);");
                    ds = oWorkstation.GetAccountsVMware(intWorkstation);
                    rptAccounts.DataSource = ds;
                    rptAccounts.DataBind();
                    foreach (RepeaterItem ri in rptAccounts.Items)
                    {
                        LinkButton _delete = (LinkButton)ri.FindControl("btnDelete");
                        _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this account?');");
                    }
                    if (rptAccounts.Items.Count == 0)
                    {
                        lblNone.Visible = true;
                        btnSubmit.Attributes.Add("onclick", "alert('You must add at least one account or select the skip button');return false;");
                    }
                    //if (oClass.IsProd(intClass))
                        panProduction.Visible = true;
                    //else
                    //    panAdmin.Visible = true;
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
            btnAdd.Attributes.Add("onclick", "return ValidateHidden('" + hdnUser.ClientID + "','" + txtUser.ClientID + "','Please enter a username, first name or last name');");
            btnManager.Attributes.Add("onclick", "return OpenWindow('NEW_USER','');");
        }

        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (Request.QueryString["userid"] != null && Request.QueryString["userid"] != "")
            {
                lblError.Text = "";
                int intUser = Int32.Parse(Request.QueryString["userid"]);
                AD oAD = new AD(0, dsn, (int)CurrentEnvironment.CORPDMN);
                string strXID = txtXID.Text.Trim().ToUpper();
                SearchResultCollection oResults = oAD.Search(strXID, "sAMAccountName");
                if (oResults.Count == 1)
                {
                    SearchResult oResult = oResults[0];
                    if (oResult.Properties.Contains("sAMAccountName") == true)
                        strXID = oResult.GetDirectoryEntry().Properties["sAMAccountName"].Value.ToString();
                    if (oResult.Properties.Contains("extensionattribute10") == true)
                    {
                        string strID = oUser.GetName(intUser, false);
                        string strPNCID = oResult.GetDirectoryEntry().Properties["extensionattribute10"].Value.ToString();
                        if (strID.ToUpper().Trim() == strPNCID.Trim().ToUpper())
                        {
                            oUser.Update(intUser, strXID, strID.ToUpper());
                            int intAdmin = Int32.Parse(Request.QueryString["admin"]);
                            if (panAdmin.Visible == false)
                                intAdmin = 0;
                            oWorkstation.AddAccountFix(0, intWorkstation, intUser, intAdmin, Int32.Parse(Request.QueryString["remote"]));
                            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&add=true");
                        }
                        else
                            lblError.Text = "<p>The X-ID you entered (" + strXID + ") has a different PNC ID configured (" + strPNCID + ").</p><p>Please try again or contact your clearview administrator.</p>";
                    }
                    else
                        lblError.Text = "<p>The X-ID you entered (" + strXID + ") does not have a PNC ID attribute configured.</p><p>Please try again or contact your clearview administrator.</p>";
                }
                else if (oResults.Count > 1)
                {
                    lblError.Text = "There were " + oResults.Count.ToString() + " accounts found in CORPDMN for the account " + txtXID.Text + ". Please try again.";
                }
                else
                {
                    lblError.Text = "Could not find that X-ID in CORPDMN. Please try again.";
                }
            }
            else
            {
                int intUser = Int32.Parse(Request.Form[hdnUser.UniqueID]);
                bool boolContinue = false;
                string strID = oUser.GetName(intUser, false);
                string strXID = oUser.GetName(intUser, true);
                if (strXID == "" || strID == "" || strXID == strID)
                {
                    // Get X-ID (since it is needed for the INI file and AD setup)
                    SearchResultCollection oCollection = oFunction.eDirectory(strID);
                    if (oCollection.Count == 1 && oCollection[0].GetDirectoryEntry().Properties.Contains("businesscategory") == true)
                    {
                        boolContinue = true;
                        strXID = oCollection[0].GetDirectoryEntry().Properties["businesscategory"].Value.ToString();
                        oUser.Update(intUser, strXID, strID);
                    }
                    else if (strXID != "")
                    {
                        oCollection = oFunction.eDirectory("businesscategory", strXID);
                        if (oCollection.Count == 1)
                        {
                            boolContinue = true;
                            strID = oCollection[0].GetDirectoryEntry().Properties["cn"].Value.ToString();
                            oUser.Update(intUser, strXID, strID);
                        }
                        else
                            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&userid=" + intUser.ToString() + "&admin=" + (chkAdmin.Checked ? "1" : "0") + "&remote=" + (chkRemote.Checked ? "1" : "0"));
                    }
                }
                else
                    boolContinue = true;

                if (boolContinue == true)
                {
                    oWorkstation.AddAccountFix(0, intWorkstation, intUser, (chkAdmin.Checked ? 1 : 0), (chkRemote.Checked ? 1 : 0));
                    Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&add=true");
                }
                else
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "offshore", "<script type=\"text/javascript\">alert('There was a problem finding the user in the directory.');<" + "/" + "script>");
            }
        }

        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"]);
        }

        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oWorkstation.DeleteAccount(Int32.Parse(oDelete.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&delete=true");
        }

        protected void btnSkip_Click(Object Sender, EventArgs e)
        {
            oWorkstation.UpdateVirtualAccounts(intWorkstation, 1);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.opener.location.reload();window.close();<" + "/" + "script>");
        }

        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            oWorkstation.UpdateVirtualAccounts(intWorkstation, 1);
            if (chkApply.Visible && chkApply.Checked)
            {
                DataSet dsAccounts = oWorkstation.GetAccountsVMware(intWorkstation);
                for (int ii = 2; dsAccounts.Tables[0].Rows.Count > 0 && ii <= intQuantity; ii++)
                {
                    DataSet dsCopy = oWorkstation.GetVirtual(intAnswer, ii);
                    int intCopy = 0;
                    if (dsCopy.Tables[0].Rows.Count > 0)
                    {
                        intCopy = Int32.Parse(dsCopy.Tables[0].Rows[0]["id"].ToString());
                        oWorkstation.DeleteAccounts(intCopy);
                        foreach (DataRow drAccount in dsAccounts.Tables[0].Rows)
                        {
                            int intUser = Int32.Parse(drAccount["userid"].ToString());
                            int intAdmin = Int32.Parse(drAccount["admin"].ToString());
                            int intRemote = Int32.Parse(drAccount["remote"].ToString());
                            oWorkstation.AddAccountFix(0, intCopy, intUser, intAdmin, intRemote);
                        }
                        oWorkstation.UpdateVirtualAccounts(intCopy, 1);
                    }
                }
            }
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.opener.location.reload();window.close();<" + "/" + "script>");
        }
    }
}
