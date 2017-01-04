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
    public partial class accounts_new : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected int intProfile;
        protected Users oUser;
        protected Variables oVariable;
        protected Forecast oForecast;
        protected Servers oServer;
        protected OnDemand oOnDemand;
        protected Classes oClass;
        protected int intServer = 0;
        protected int intDomain = 0;
        protected int intRequest = 0;
        protected int intAnswer = 0;
        protected int intStep = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "ClearView Account Configuration";
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oUser = new Users(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oForecast = new Forecast(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oOnDemand = new OnDemand(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            if (Request.QueryString["duplicate"] != null && Request.QueryString["duplicate"] != "")
                Page.ClientScript.RegisterStartupScript(typeof(Page), "duplicate", "<script type=\"text/javascript\">alert('The account " + Request.QueryString["duplicate"] + " already exists');<" + "/" + "script>");
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intServer = Int32.Parse(Request.QueryString["id"]);
                Page.Title = "ClearView Account Configuration | Server # " + intServer.ToString();
                DataSet ds = oServer.Get(intServer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intStep = Int32.Parse(ds.Tables[0].Rows[0]["step"].ToString());
                    intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                    int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
                    intRequest = oForecast.GetRequestID(intAnswer, true);
                    intDomain = Int32.Parse(ds.Tables[0].Rows[0]["domainid"].ToString());
                    Domains oDomain = new Domains(intProfile, dsn);
                    bool boolProcess = true;
                    if (oDomain.Get(intDomain, "account_setup") != "1")
                    {
                        if (oForecast.GetAnswer(intAnswer, "test") == "1")
                            intDomain = Int32.Parse(ds.Tables[0].Rows[0]["test_domainid"].ToString());
                        else
                            boolProcess = false;
                    }
                    if (boolProcess == false)
                        btnAdd.Enabled = false;
                    lblDomain.Text = oDomain.Get(intDomain, "name");
                    bool boolPNC = (oClass.Get(intClass, "pnc") == "1");
                    if (boolPNC == true)
                    {
                        panPNC.Visible = true;
                        txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2,null,'PNC');");
                    }
                    else
                    {
                        txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2,null,'NCB');");
                        panNCB.Visible = true;
                    }
                    Requests oRequest = new Requests(intProfile, dsn);
                    //if (oRequest.GetUser(intRequest) == intProfile)
                    //{
                    panPermit.Visible = true;
                    lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
                    chkAdmin.Attributes.Add("onclick", "CheckAdmin(this);");
                    ds = oServer.GetAccounts(intServer);
                    rptAccounts.DataSource = ds;
                    rptAccounts.DataBind();
                    foreach (RepeaterItem ri in rptAccounts.Items)
                    {
                        LinkButton _delete = (LinkButton)ri.FindControl("btnDelete");
                        _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this account?');");
                        Label _local = (Label)ri.FindControl("lblLocal");
                        Label _domain = (Label)ri.FindControl("lblDomain");
                        string strPermissions = "";
                        Label _permissions = (Label)ri.FindControl("lblPermissions");
                        if (boolPNC == true)
                        {
                            char[] strAccountSplit = { ';' };

                            // Domain Groups
                            string[] strAccountDomainArray = _domain.Text.Split(strAccountSplit);
                            for (int jj = 0; jj < strAccountDomainArray.Length; jj++)
                            {
                                string strAccountDomain = strAccountDomainArray[jj].Trim();
                                if (strAccountDomain.Contains("_") == true)
                                {
                                    strPermissions += strAccountDomain.Substring(0, strAccountDomain.IndexOf("_"));
                                    strAccountDomain = strAccountDomain.Substring(strAccountDomain.IndexOf("_") + 1);
                                    if (strAccountDomain == "1")
                                        strPermissions += " (Remote Desktop)";
                                }
                                else
                                    strPermissions += strAccountDomain;
                                strPermissions += "<br/>";
                            }

                            // Local Groups
                            string[] strAccountLocalArray = _local.Text.Split(strAccountSplit);
                            for (int jj = 0; jj < strAccountLocalArray.Length; jj++)
                            {
                                string strAccountLocal = strAccountLocalArray[jj].Trim();
                                if (strAccountLocal.Contains("_") == true)
                                {
                                    strPermissions += strAccountLocal.Substring(0, strAccountLocal.IndexOf("_"));
                                    strAccountLocal = strAccountLocal.Substring(strAccountLocal.IndexOf("_") + 1);
                                    if (strAccountLocal == "1")
                                        strPermissions += " (Remote Desktop)";
                                }
                                else
                                    strPermissions += strAccountLocal;
                                strPermissions += "<br/>";
                            }
                        }
                        else
                        {
                            Label _admin = (Label)ri.FindControl("lblAdmin");
                            string strPermission = _admin.Text;
                            if (strPermission == "1")
                                strPermissions += "ADMINISTRATOR<br/>";
                            strPermission = _local.Text;
                            if (strPermission.Contains("GLCfsaRO_SysVol"))
                                strPermissions += "SYS_VOL (C:) - Read Only<br/>";
                            else if (strPermission.Contains("GLCfsaRW_SysVol"))
                                strPermissions += "SYS_VOL (C:) - Read / Write<br/>";
                            else if (strPermission.Contains("GLCfsaFC_SysVol"))
                                strPermissions += "SYS_VOL (C:) - Full Control<br/>";
                            if (strPermission.Contains("GLCfsaRO_UtlVol"))
                                strPermissions += "UTL_VOL (E:) - Read Only<br/>";
                            else if (strPermission.Contains("GLCfsaRW_UtlVol"))
                                strPermissions += "UTL_VOL (E:) - Read / Write<br/>";
                            else if (strPermission.Contains("GLCfsaFC_UtlVol"))
                                strPermissions += "UTL_VOL (E:) - Full Control<br/>";
                            if (strPermission.Contains("GLCfsaRO_AppVol"))
                                strPermissions += "APP_VOL (F:) - Read Only<br/>";
                            else if (strPermission.Contains("GLCfsaRW_AppVol"))
                                strPermissions += "APP_VOL (F:) - Read / Write<br/>";
                            else if (strPermission.Contains("GLCfsaFC_AppVol"))
                                strPermissions += "APP_VOL (F:) - Full Control<br/>";
                        }
                        if (strPermissions == "")
                            strPermissions = "-----";
                        _permissions.Text = strPermissions;
                    }
                    if (rptAccounts.Items.Count == 0)
                    {
                        lblNone.Visible = true;
                        btnSubmit.Attributes.Add("onclick", "alert('You must add at least one account or select the skip button');return false;");
                    }
                    //}
                    //else
                    //    panDenied.Visible = true;
                }
            }
            else
                panDenied.Visible = true;
            btnSkip.Attributes.Add("onclick", "return confirm('Are you sure you want to skip the account request process?');");
            btnAdd.Attributes.Add("onclick", "return ValidateHidden('" + hdnUser.ClientID + "','" + txtUser.ClientID + "','Please enter a username, first name or last name');");
            btnManager.Attributes.Add("onclick", "return OpenWindow('NEW_USER','');");
            chkDeveloper.Attributes.Add("onclick", "SwapChecks(this,'" + chkPromoter.ClientID + "');");
            chkPromoter.Attributes.Add("onclick", "SwapChecks(this,'" + chkDeveloper.ClientID + "');");
        }

        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intUser = Int32.Parse(Request.Form[hdnUser.UniqueID]);
            string strUser = oUser.GetName(intUser);
            string strLocal = "";
            string strDomain = "";
            int intAdmin = 0;

            if (panNCB.Visible == true)
            {
                if (chkAdmin.Checked == true)
                    intAdmin = 1;
                else
                {
                    switch (radSysVol.SelectedItem.Value)
                    {
                        case "R":
                            strLocal += "GLCfsaRO_SysVol;";
                            break;
                        case "W":
                            strLocal += "GLCfsaRW_SysVol;";
                            break;
                        case "F":
                            strLocal += "GLCfsaFC_SysVol;";
                            break;
                    }
                    switch (radUtlVol.SelectedItem.Value)
                    {
                        case "R":
                            strLocal += "GLCfsaRO_UtlVol;";
                            break;
                        case "W":
                            strLocal += "GLCfsaRW_UtlVol;";
                            break;
                        case "F":
                            strLocal += "GLCfsaFC_UtlVol;";
                            break;
                    }
                    switch (radAppVol.SelectedItem.Value)
                    {
                        case "R":
                            strLocal += "GLCfsaRO_AppVol;";
                            break;
                        case "W":
                            strLocal += "GLCfsaRW_AppVol;";
                            break;
                        case "F":
                            strLocal += "GLCfsaFC_AppVol;";
                            break;
                    }
                }
                strUser = oUser.GetName(intUser, true);
            }
            if (panPNC.Visible == true)
            {
                if (chkDeveloper.Checked)
                    strDomain += "Developers_" + (chkDeveloperR.Checked ? "1" : "0") + ";";
                if (chkPromoter.Checked)
                    strDomain += "Promoters_" + (chkPromoterR.Checked ? "1" : "0") + ";";
                if (chkAppSupport.Checked)
                    strDomain += "AppSupport_" + (chkAppSupportR.Checked ? "1" : "0") + ";";
                if (chkAppUsers.Checked)
                    strDomain += "AppUsers_" + (chkAppUsersR.Checked ? "1" : "0") + ";";
                //if (chkAuthProbMgmt.Checked)
                //    strDomain += "AuthProbMgmt;";
            }
            bool boolDuplicate = false;
            DataSet ds = oServer.GetAccounts(intServer);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["xid"].ToString().Trim().ToUpper() == strUser.Trim().ToUpper())
                {
                    boolDuplicate = true;
                    break;
                }
            }
            if (boolDuplicate == false)
            {
                oServer.AddAccount(intServer, strUser, intDomain, intAdmin, strLocal, strDomain, 0);
                Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&add=true");
            }
            else
            {
                Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&duplicate=" + strUser);
            }
        }

        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oServer.DeleteAccount(Int32.Parse(oDelete.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&delete=true");
        }

        protected void btnSkip_Click(Object Sender, EventArgs e)
        {
            oServer.UpdateAccounts(intServer, 1);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.opener.location.reload();window.close();<" + "/" + "script>");
        }

        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            oServer.UpdateAccounts(intServer, 1);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.opener.location.reload();window.close();<" + "/" + "script>");
        }
    }
}
