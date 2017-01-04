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
    public partial class register : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected bool boolDebug = (ConfigurationManager.AppSettings["ERROR_SHOW"] == "1");
        

        protected string strTitle = ConfigurationManager.AppSettings["appTitle"];
        protected string strUser = "Unvailable";
        protected Users oUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = strTitle;
            oUser = new Users(0, dsn);
            lblTitle.Text = "ClearView Registration";
            strUser = Request.ServerVariables["LOGON_USER"];
            strUser = strUser.Substring(strUser.LastIndexOf("\\") + 1);
            if (boolDebug == true)
                strUser = "XXXXX";
            if (!IsPostBack)
            {
                if (Request.QueryString["done"] != null)
                    panFinish.Visible = true;
                else
                {
                    if (String.IsNullOrEmpty(Request.QueryString["debug-user"]) == false)
                        strUser = Request.QueryString["debug-user"];
                    int intUser = oUser.GetId(strUser);
                    if (intUser > 0)
                        panRegistered.Visible = true;
                    else
                    {
                        bool boolFound = false;
                        AD oAD = new AD(0, dsn, intEnvironment);
                        Functions oFunction = new Functions(0, dsn, intEnvironment);
                        Variables oVariable = new Variables(intEnvironment);
                        SearchResultCollection oCollection = oFunction.eDirectory(strUser);
                        if (oCollection.Count == 1)
                        {
                            if (oCollection[0].GetDirectoryEntry().Properties.Contains("businesscategory") == true)
                                txtXID.Text = oCollection[0].GetDirectoryEntry().Properties["businesscategory"].Value.ToString().ToUpper();
                            if (oCollection[0].GetDirectoryEntry().Properties.Contains("cn") == true)
                                txtPNC.Text = oCollection[0].GetDirectoryEntry().Properties["cn"].Value.ToString().ToUpper();
                            txtFirst.Text = oFunction.ToTitleCase(oCollection[0].GetDirectoryEntry().Properties["givenname"].Value.ToString());
                            txtLast.Text = oFunction.ToTitleCase(oCollection[0].GetDirectoryEntry().Properties["sn"].Value.ToString());
                            if (oCollection[0].Properties.Contains("pncmanagerid") == true)
                            {
                                string strManager = oFunction.eDirectory(oCollection[0], "pncmanagerid");
                                DataSet dsManager = oUser.Gets(strManager);
                                if (dsManager.Tables[0].Rows.Count == 1)
                                {
                                    int intManager = Int32.Parse(dsManager.Tables[0].Rows[0]["userid"].ToString());
                                    hdnManager.Value = intManager.ToString();
                                    txtManager.Text = oUser.GetFullName(intManager) + " (" + oUser.GetName(intManager) + ")";
                                }
                                else
                                {
                                    trManager.Visible = true;
                                    btnRegister.Enabled = false;
                                    hdnManager.Value = lblManager.Text = oFunction.eDirectory(oCollection[0], "pncmanagerid");
                                    txtManager.Text = oFunction.eDirectory(oCollection[0], "pncmanagername") + " (" + oFunction.eDirectory(oCollection[0], "pncmanagerid") + ")";
                                }
                            }
                            if (oCollection[0].Properties.Contains("telephonenumber") == true)
                                txtPhone.Text = oCollection[0].GetDirectoryEntry().Properties["telephonenumber"].Value.ToString();
                            boolFound = true;
                        }
                        else
                        {
                            DirectoryEntry oEntry = oAD.UserSearch(strUser);
                            bool boolEmailFound = false;
                            if (Request.QueryString["email"] != null)
                            {
                                txtEmail.Text = Request.QueryString["email"];
                                SearchResultCollection oResults = oAD.Search(txtEmail.Text, "mail");
                                if (oResults.Count == 1)
                                {
                                    oEntry = oResults[0].GetDirectoryEntry();
                                    if (oEntry != null)
                                    {
                                        strUser = oEntry.Properties["sAMAccountName"].Value.ToString();
                                        intUser = oUser.GetId(strUser);
                                        if (intUser > 0)
                                        {
                                            boolEmailFound = true;
                                            panRegistered.Visible = true;
                                        }
                                    }
                                }
                            }
                            if (boolEmailFound == false)
                            {
                                if (oEntry != null)
                                {
                                    strUser = oEntry.Properties["sAMAccountName"].Value.ToString();
                                    txtPNC.Text = strUser;
                                    txtFirst.Text = oEntry.Properties["givenname"].Value.ToString();
                                    txtLast.Text = oEntry.Properties["sn"].Value.ToString();
                                    try { txtPhone.Text = oEntry.Properties["telephonenumber"].Value.ToString(); }
                                    catch { }
                                    try
                                    {
                                        string strManager = oEntry.Properties["manager"].Value.ToString();
                                        strManager = strManager.Substring(strManager.IndexOf("=") + 1);
                                        strManager = strManager.Substring(0, strManager.IndexOf(","));
                                        DirectoryEntry oManager = oAD.UserSearch(strManager);
                                        if (oManager != null)
                                        {
                                            string strXID = oManager.Properties["sAMAccountName"].Value.ToString();
                                            strManager = "(" + strXID + ")";
                                            strManager = oManager.Properties["sn"].Value.ToString() + " " + strManager;
                                            strManager = oManager.Properties["givenname"].Value.ToString() + " " + strManager;
                                            txtManager.Text = strManager;
                                            hdnManager.Value = oUser.GetId(strXID).ToString();
                                        }
                                    }
                                    catch { }
                                    boolFound = true;
                                }
                            }
                        }
                        if (boolFound == true)
                        {
                            panNotRegistered.Visible = true;
                            txtManager.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divManager.ClientID + "','" + lstManager.ClientID + "','" + hdnManager.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                            lstManager.Attributes.Add("ondblclick", "AJAXClickRow();");
                            btnRegister.Attributes.Add("onclick", "return ValidateText('" + txtFirst.ClientID + "','Please enter your first name')" +
                                " && ValidateText('" + txtLast.ClientID + "','Please enter your last name')" +
                                " && ValidateHidden('" + hdnManager.ClientID + "','" + txtManager.ClientID + "','Please enter the name or LAN ID of your manager')" +
                                ";");
                        }
                        else if (panRegistered.Visible == false)
                        {
                            panNotRegisteredNone.Visible = true;
                            btnEmail.Attributes.Add("onclick", "return ValidateText('" + txtEmail.ClientID + "','Please enter your FULL email address')" +
                                ";");
                        }
                    }
                }
            }
        }
        protected void btnRegister_Click(Object Sender, EventArgs e)
        {
            int intManager = Int32.Parse(Request.Form[hdnManager.UniqueID]);
            oUser.Add(txtXID.Text, txtPNC.Text, txtFirst.Text, txtLast.Text, intManager, 0, 0, 0, "", 0, txtPhone.Text, "", 0, 0, 0, 0, 1);
            int intUser = oUser.GetId(strUser);
            // Load Manager's Role(s)
            NCC.ClearView.Application.Core.Roles oRole = new NCC.ClearView.Application.Core.Roles(0, dsn);
            DataSet dsRoles = oRole.Gets(intManager);
            foreach (DataRow drRole in dsRoles.Tables[0].Rows)
            {
                int intApp = Int32.Parse(drRole["applicationid"].ToString());
                oRole.Add(intUser, oRole.Get(intManager, intApp));
            }
            Response.Redirect(Request.Path + "?done=true");
        }
        protected void btnEmail_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?email=" + txtEmail.Text);
        }
    }
}
