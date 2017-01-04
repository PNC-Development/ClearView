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
    public partial class consistency_server : BasePage
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
            DataSet ds = oConsistencyGroup.GetServer(txtName.Text);
            if (ds.Tables[0].Rows.Count == 0)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('There is no consistency group associated with this server.\\n\\nPlease make sure you typed in the name of the server correctly.');<" + "/" + "script>");
            else if (ds.Tables[0].Rows.Count == 1)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">window.parent.UpdateConsistency('" + ds.Tables[0].Rows[0]["name"].ToString() + "'," + ds.Tables[0].Rows[0]["id"].ToString() + ");<" + "/" + "script>");
            else
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('There were multiple consiste');<" + "/" + "script>");
        }
    }
}
