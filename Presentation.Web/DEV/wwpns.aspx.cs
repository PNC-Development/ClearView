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
using System.Net;
using System.IO;
//using PAObjectsLib;
using Vim25Api;
using System.Reflection;
using Tamir.SharpSsh;
using System.Net.NetworkInformation;

namespace NCC.ClearView.Presentation.Web
{
    public partial class wwpns : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intCount = 0;
        private string strResult;
        private string strError;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn1_Click(object sender, EventArgs e)
        {
            Asset oAsset = new Asset(0, dsnAsset, dsn);
            int _assetid = 32532;
            int _answerid = 0;
            int intModelProperty = 713;
            string strScripts = Server.MapPath("/scripts/");
            for (int ii = 1; ii <= 2; ii++)
            {
                //string strValue = oAsset.GetDellSysInfo(_assetid, _answerid, intModelProperty, strScripts, DellQueryType.WWPN, ii);
                //Response.Write(ii.ToString() + " = " + strValue + "<br/>");
            }
        }

    }
}
