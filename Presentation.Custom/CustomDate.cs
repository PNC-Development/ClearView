using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCC.ClearView.Presentation.Web.Custom
{
    public partial class CustomDate : TextBox
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
        protected override void Render(HtmlTextWriter output)
        {
            base.Render(output);
            output.Write("&nbsp;<a href=\"javascript:void(0);\" title=\"View Calendar\" onclick=\"ShowCalendar('" + this.ClientID + "');\"><img src=\"/images/calendar.gif\" border=\"0\" align=\"absmiddle\"/></a>");
        }
    }
}
