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
    public partial class _delegate : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Delegates oDelegate;
        protected Pages oPage;
        protected int intUser = 0;
        protected int intParent = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            oDelegate = new Delegates(0, dsn);
            oPage = new Pages(0, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intUser = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["parent"] != null && Request.QueryString["parent"] != "")
                intParent = Int32.Parse(Request.QueryString["parent"]);
            Variables oVariable = new Variables(intEnvironment);
            txtDelegate.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divDelegate.ClientID + "','" + lstDelegate.ClientID + "','" + hdnDelegate.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstDelegate.Attributes.Add("ondblclick", "AJAXClickRow();");
            btnSave.Attributes.Add("onclick", "return ValidateHidden('" + hdnDelegate.ClientID + "','" + txtDelegate.ClientID + "','Please enter the LAN ID of the delegate')" +
                " && ValidateDropDown('" + ddlRights.ClientID + "','Please make a selection for the permission')" +
                ";");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oDelegate.Add(intUser, Int32.Parse(Request.Form[hdnDelegate.UniqueID]), Int32.Parse(ddlRights.SelectedItem.Value));
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">window.parent.navigate('" + oPage.GetFullLink(intParent) + "?action=addbuddy');window.close();<" + "/" + "script>");
        }
    }
}
