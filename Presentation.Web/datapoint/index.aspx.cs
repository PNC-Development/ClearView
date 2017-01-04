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

namespace NCC.ClearView.Presentation.Web
{
    public partial class datapointIndex : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string strTitle = ConfigurationManager.AppSettings["appTitle"];
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Control oControl = (Control)LoadControl("/controls/datapoint_controls.ascx");
            Page.Title = strTitle;
            PH3.Controls.Add(oControl);
            oControl = (Control)LoadControl("/controls/sys/sys_rotator_header.ascx");
            PH4.Controls.Add(oControl);
        }
    }
}
