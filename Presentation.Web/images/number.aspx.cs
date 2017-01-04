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
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class number : BasePage
    {
        protected string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;

        protected void Page_Load()
        {
            string strNumber = "X";
            if (Request.QueryString["num"] != null)
                strNumber = Request.QueryString["num"];
            Bitmap oBitmap = new Bitmap(Server.MapPath("/images/number.gif"));
            oBitmap = ConvertToRGB(oBitmap);
            Graphics oGraphic = Graphics.FromImage(oBitmap);
            oGraphic.TextRenderingHint = TextRenderingHint.AntiAlias;
            SolidBrush oWhiteBrush = new SolidBrush(ColorTranslator.FromHtml("#000000"));
            Font oBrushFont = new Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point);
            Point oBrushPoint = new Point(4, 2);
            oGraphic.DrawString(strNumber, oBrushFont, oWhiteBrush, oBrushPoint);
            oGraphic.Dispose();
            Response.ContentType = "image/jpeg";
            oBitmap.Save(Response.OutputStream, ImageFormat.Jpeg);
            oBitmap.Dispose();
        }
        protected Bitmap ConvertToRGB(Bitmap original)
        {
            Bitmap newImage = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppArgb);
            newImage.SetResolution(original.HorizontalResolution, original.VerticalResolution);
            Graphics g = Graphics.FromImage(newImage);
            g.DrawImageUnscaled(original, 0, 0);
            g.Dispose();
            return newImage;
        }
    }
}
