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
    public partial class consistency_select : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected ConsistencyGroups oConsistencyGroup;
        protected void Page_Load(object sender, EventArgs e)
        {
            oConsistencyGroup = new ConsistencyGroups(0, dsn);
            if (!IsPostBack)
            {
                ddlName.DataValueField = "id";
                ddlName.DataTextField = "name";
                ddlName.DataSource = oConsistencyGroup.Gets(1);
                ddlName.DataBind();
                ddlName.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                btnSubmit.Attributes.Add("onclick", "return ValidateDropDown('" + ddlName.ClientID + "','Please select a consistency group name');");
            }
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">window.parent.UpdateConsistency('" + ddlName.SelectedItem.Text + "'," + ddlName.SelectedItem.Value + ");<" + "/" + "script>");
        }
    }
}
