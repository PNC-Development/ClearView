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
    public partial class design_approval : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected int intAnswer;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "ClearView Design Approval";
            Int32.TryParse(Request.Cookies["profileid"].Value, out intProfile);
            Int32.TryParse(Request.QueryString["id"], out intAnswer);
            if (intAnswer > 0)
            {
                Page.Title = "ClearView Design Approval | Design # " + intAnswer.ToString();
                Functions oFunction = new Functions(0, dsn, intEnvironment);
                frmDesign.Attributes.Add("src", "/datapoint/service/design.aspx?id=" + oFunction.encryptQueryString(intAnswer.ToString()) + "&readonly=true");
            }
            btnApprove.Attributes.Add("onclick", "return ValidateCheckLists('" + chkAgreeList.ClientID + "','Please select all disclaimer warnings');");
            btnDeny.Attributes.Add("onclick", "return ValidateText('" + txtComments.ClientID + "','Please enter some comments');");
            btnCancel1.Attributes.Add("onclick", "ShowHideDiv('trDesign','inline');ShowHideDiv('trApprove','none');ShowHideDiv('trDeny','none');return false;");
            btnCancel2.Attributes.Add("onclick", "ShowHideDiv('trDesign','inline');ShowHideDiv('trApprove','none');ShowHideDiv('trDeny','none');return false;");
        }
    }
}
