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
    public partial class consistency_new : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected ConsistencyGroups oConsistencyGroup;
        protected void Page_Load(object sender, EventArgs e)
        {
            oConsistencyGroup = new ConsistencyGroups(0, dsn);
            txtName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSubmit.ClientID + "').click();return false;}} else {return true}; ");
            btnSubmit.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a valid name');");
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            int intID = oConsistencyGroup.Add(txtName.Text, 0, 1);
            if (intID == 0)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('There is already a consistency group with that name.\\n\\nPlease enter a different name, or close this window and choose \"Select a consistency group\" to choose this group');<" + "/" + "script>");
            else
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">window.parent.UpdateConsistency('" + txtName.Text + "'," + intID.ToString() + ");<" + "/" + "script>");
        }
    }
}
