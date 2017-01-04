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
using NCC.ClearView.Application.UI.Entities;
using NCC.ClearView.Application.UI.BusinessLogic;


namespace NCC.ClearView.Presentation.Web
{
    public partial class ControlHelp : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intResourceRequest = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequest"]);
        protected string strTitle = ConfigurationManager.AppSettings["appTitle"];
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Title1.Text = strTitle;
            try
            {
                if (Request.QueryString["ctrlid"] != null)
                {
                    long lngControlId = long.Parse(Request.QueryString["ctrlid"]);
                    UIFactory oUIFactory = new UIFactory();
                    UIControl oWebControl = oUIFactory.GetUIControl(lngControlId);

                    tdHeader.InnerHtml = oWebControl.LabelText;
                    divHelp.InnerHtml = oWebControl.HTMLHelp;
                }

            }
            catch 
            { 

            }
        }
    }
}
