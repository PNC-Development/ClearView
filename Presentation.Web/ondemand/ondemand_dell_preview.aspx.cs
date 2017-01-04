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
    public partial class ondemand_dell_preview : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private int intAsset = 0;
        protected string strILO = "";
        protected string strUser = "";
        protected string strPass = "";

        protected void Page_Load()
        {
            Int32.TryParse(Request.QueryString["id"], out intAsset);
            if (intAsset > 0)
            {
                int intModelProperty = 0;
                Asset oAsset = new Asset(0, dsnAsset, dsn);
                if (Int32.TryParse(oAsset.Get(intAsset, "modelid"), out intModelProperty) == true)
                {
                    ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
                    if (oModelsProperties.IsDell(intModelProperty) == true)
                    {
                        int intDellConfig = 0;
                        Int32.TryParse(oModelsProperties.Get(intModelProperty, "dellconfigid"), out intDellConfig);
                        Dells oDell = new Dells(0, dsn);
                        DataSet dsDell = oDell.Get(intDellConfig);
                        if (dsDell.Tables[0].Rows.Count == 1)
                        {
                            DataRow drDell = dsDell.Tables[0].Rows[0];
                            strUser = drDell["username"].ToString();
                            strPass = drDell["password"].ToString();
                            strILO = oAsset.GetServerOrBlade(intAsset, "ilo");

                            Functions oFunction = new Functions(0, dsn, intEnvironment);
                            Variables oVariable = new Variables(intEnvironment);

                            if (Request.QueryString["showimage"] != null)
                                Response.Redirect("/ondemand/ondemand_dell_image.aspx?showimage=https://" + strILO + Request.QueryString["showimage"]);
                            else if (Request.QueryString["bypass"] != null)
                            {
                                if (strUser == "" || strPass == "")
                                {
                                    strUser = oVariable.ADUser() + "@" + oVariable.FullyQualified();
                                    strPass = oFunction.AddPassword(50);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
