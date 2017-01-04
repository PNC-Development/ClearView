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
using System.Xml;


namespace NCC.ClearView.Presentation.Web
{
    public partial class ajax_asset_category : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected ModelsProperties oModelsProperties;
        protected void Page_Load(object sender, EventArgs e)
        {
            oModelsProperties = new ModelsProperties(0, dsn);

            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                XmlDocument oDoc = new XmlDocument();
                oDoc.Load(Request.InputStream);

                int intModel = Int32.Parse(Server.UrlDecode(oDoc.ChildNodes[0].ChildNodes[0].InnerText));
                string strAssetCategory = oModelsProperties.Get(intModel, "asset_category");

                Response.ContentType = "text/xml";
                Response.Write("<values><value>" + strAssetCategory + "</value></values>");
                Response.End();
            }
        }
    }
}
