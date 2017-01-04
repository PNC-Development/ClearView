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
    public partial class header_wide : BasePage
    {
        protected string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Applications oApplication;

        protected void Page_Load()
        {
            int intApplication = 0;
            string strApplication = "Welcome to ClearView";
            oApplication = new Applications(0, dsn);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (intApplication > 0)
                strApplication = oApplication.Get(intApplication, "name");
            if (Request.QueryString["t"] != null)
                strApplication = Request.QueryString["t"];
            Bitmap oBitmap = new Bitmap(Server.MapPath("/images/header_left.gif"));
            oBitmap = ConvertToRGB(oBitmap);
            Graphics oGraphic = Graphics.FromImage(oBitmap);
            oGraphic.TextRenderingHint = TextRenderingHint.AntiAlias;
            SolidBrush oWaterBrush = new SolidBrush(ColorTranslator.FromHtml("#333333"));
            Font oWaterFont = new Font("Arial Black", 11, FontStyle.Regular, GraphicsUnit.Point);
            Point oWaterPoint = new Point(15, 46);
            oGraphic.DrawString(strApplication, oWaterFont, oWaterBrush, oWaterPoint);
            SolidBrush oWhiteBrush = new SolidBrush(ColorTranslator.FromHtml("#FFFFFF"));
            Font oBrushFont = new Font("Arial Black", 11, FontStyle.Regular, GraphicsUnit.Point);
            Point oBrushPoint = new Point(11, 42);
            oGraphic.DrawString(strApplication, oBrushFont, oWhiteBrush, oBrushPoint);
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
