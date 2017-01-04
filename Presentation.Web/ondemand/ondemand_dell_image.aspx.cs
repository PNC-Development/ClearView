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
using System.Net;
using System.IO;

namespace NCC.ClearView.Presentation.Web
{
    public partial class ondemand_dell_image : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private int intAsset = 0;
        private Asset oAsset;
        protected string strILO = "";

        protected void Page_Load()
        {
            if (Request.QueryString["Showimage"] != null)
            {
                string strImage = Request.QueryString["showimage"];
                if (strImage != "ERROR")
                    imgDell.ImageUrl = strImage;
            }
        }
    }
}
