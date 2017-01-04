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
    public partial class NotFound : BasePage
    {
        protected string strTitle = ConfigurationManager.AppSettings["appTitle"];
        protected void Page_Load(object sender, EventArgs e)
        {
            Control oControl;
            Page.Title = strTitle;
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
            {
                oControl = (Control)LoadControl("/controls/sys/sys_topnav.ascx");
                PH1.Controls.Add(oControl);
                oControl = (Control)LoadControl("/controls/sys/sys_leftnav.ascx");
                PH2.Controls.Add(oControl);
            }
            oControl = (Control)LoadControl("/controls/sys/sys_rotator_header.ascx");
            PH4.Controls.Add(oControl);
        }
    }
}
