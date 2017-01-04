using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace NCC.ClearView.Presentation.Web
{
    public class BaseControl : UserControl
    {
        protected override void RenderChildren(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "controlContainer");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.Write(_title);
            writer.RenderEndTag(); //</th>
            writer.RenderEndTag(); //</tr>
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            base.RenderChildren(writer);
            writer.RenderEndTag(); //</td>
            writer.RenderEndTag(); //</tr>
            writer.RenderEndTag(); //</table>
        }

        /// <summary>
        /// Title of control, to be displayed in header
        /// </summary>
        protected string Title
        {
            get { return HttpUtility.HtmlDecode(_title); }
            set { _title = HttpUtility.HtmlEncode(value); }
        }
        private string _title = "Unnamed&#32;Control";
    }
}
