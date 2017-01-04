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
    public partial class RegistrationForm : BaseControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Users oUser;
        protected string strUser = "Unvailable";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "ClearView Registration";
            oUser = new Users(0, dsn);
            strUser = Request.ServerVariables["LOGON_USER"];
            strUser = strUser.Substring(strUser.LastIndexOf("\\") + 1);
            if (!IsPostBack)
            {
                if (Request.QueryString["done"] != null)
                    panFinish.Visible = true;
                else
                {
                    int intUser = oUser.GetId(strUser);
                    if (intUser > 0)
                        panRegistered.Visible = true;
                    else
                    {
                        AD oAD = new AD(0, dsn, intEnvironment);
                        Variables oVariable = new Variables(intEnvironment);
                        DirectoryEntry oEntry = oAD.UserSearch(strUser);
                        bool boolFound = false;
                        if (oEntry != null)
                        {
                            txtXID.Text = oEntry.Properties["sAMAccountName"].Value.ToString();
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
                        else
                            panNotRegisteredNone.Visible = true;
                    }
                }
            }
        }

        protected void btnRegister_Click(Object Sender, EventArgs e)
        {
            int intManager = Int32.Parse(Request.Form[hdnManager.UniqueID]);
            oUser.Add(strUser, strUser, txtFirst.Text, txtLast.Text, intManager, 0, 0, 0, "", 0, txtPhone.Text, "", 0, 0, 0, 0, 1);
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
    }
}