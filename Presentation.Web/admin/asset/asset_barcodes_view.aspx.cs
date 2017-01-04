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
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using NCC.ClearView.Application.Core;


namespace NCC.ClearView.Presentation.Web
{
    public partial class asset_barcodes_view : BasePage
    {
    
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    protected ModelsProperties oModelsProperties;
    protected int intProfile;
    protected string strCodes1 = "";
    protected string strCodes2 = "";
    protected string strCodes3 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string strDirectory = Request.PhysicalApplicationPath + "images\\temp\\barcode";
            if (Directory.Exists(strDirectory) == false)
                Directory.CreateDirectory(strDirectory);
            else
            {
                DirectoryInfo oDir = new DirectoryInfo(strDirectory);
                DirectoryInfo[] oDirs = oDir.GetDirectories();
                System.IO.FileInfo[] oFiles = oDir.GetFiles();
                foreach (System.IO.FileInfo oFile in oFiles)
                    if ((File.GetAttributes(strDirectory + "\\" + oFile.Name) & FileAttributes.ReadOnly) != FileAttributes.ReadOnly)
                        File.Delete(strDirectory + "\\" + oFile.Name);
            }
            oModelsProperties = new ModelsProperties(0, dsn);
            int intCol = 0;
            if (Request.QueryString["modelid"] != null)
            {
                string strFile = LoadImage(Int32.Parse(Request.QueryString["modelid"]), strDirectory);
                strCodes1 = "<p><img src=\"/images/temp/barcode/" + strFile + "\" border=\"0\" align=\"absmiddle\"/></p>";
            }
            else if (Request.QueryString["id"] != null)
            {
                DataSet ds = oModelsProperties.GetTypes(0, Int32.Parse(Request.QueryString["id"]), 1);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    intCol++;
                    string strFile = LoadImage(Int32.Parse(dr["id"].ToString()), strDirectory);
                    if (intCol == 3)
                        intCol = 1;
                    if (intCol == 1)
                        strCodes1 += "<p class=\"header\">" + dr["name"].ToString() + "<br/><img src=\"/images/temp/barcode/" + strFile + "\" border=\"0\" align=\"absmiddle\"/></p>";
                    else if (intCol == 2)
                        strCodes2 += "<p class=\"header\">" + dr["name"].ToString() + "<br/><img src=\"/images/temp/barcode/" + strFile + "\" border=\"0\" align=\"absmiddle\"/></p>";
                    else if (intCol == 3)
                        strCodes3 += "<p class=\"header\">" + dr["name"].ToString() + "<br/><img src=\"/images/temp/barcode/" + strFile + "\" border=\"0\" align=\"absmiddle\"/></p>";
                }
            }
        }

        public string LoadImage(int _id, string _directory)
        {
            string strFile = _id.ToString() + ".JPG";
            if (File.Exists(_directory + "\\" + _id.ToString() + ".JPG") == false)
            {
                Bitmap oBitmap = new Bitmap(350, 100);
                SolidBrush wBrush = new SolidBrush(Color.White);
                Graphics oGraphic = Graphics.FromImage(oBitmap);
                oGraphic.FillRectangle(wBrush, 0, 0, 350, 100);
                SolidBrush oWaterBrush = new SolidBrush(ColorTranslator.FromHtml("#000000"));
                Font oWaterFont = new Font("IDAutomationHC39M", 18, FontStyle.Regular, GraphicsUnit.Point);
                Point oWaterPoint = new Point(10, 10);
                oGraphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                oGraphic.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                oGraphic.DrawString("(" + _id.ToString() + ")", oWaterFont, oWaterBrush, oWaterPoint);
                oGraphic.Dispose();
                oBitmap.Save(_directory + "\\" + strFile, ImageFormat.Jpeg);
                oBitmap.Dispose();
            }
            return strFile;
        }

    }
}
