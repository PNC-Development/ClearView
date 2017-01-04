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

namespace NCC.ClearView.Presentation.Web.DEV
{
    public partial class controls_screenshot : System.Web.UI.Page
    {
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            Variables oVariable = new Variables(intEnvironment);
            
            txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divUser.ClientID + "','" + lstUser.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstUser.Attributes.Add("ondblclick", "AJAXClickRow();");

            txtServer.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'200','195','" + divServer.ClientID + "','" + lstServer.ClientID + "','" + hdnServer.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_servernames.aspx',2);");
            lstServer.Attributes.Add("ondblclick", "AJAXClickRow();");

            txtMnemonic.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divMnemonic.ClientID + "','" + lstMnemonic.ClientID + "','" + hdnMnemonic.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_mnemonics_pending.aspx',2);");
            lstMnemonic.Attributes.Add("ondblclick", "AJAXClickRow();");

        }
    }
}
