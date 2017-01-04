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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class inventory_supply : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Asset oAsset;
        protected ModelsProperties oModelsProperties;
        protected Locations oLocation;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected int intProfile;
        protected int intModel = 0;
        protected int intStatus = 0;
        protected string strResults = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oAsset = new Asset(intProfile, dsnAsset);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            if (Request.QueryString["model"] != null && Request.QueryString["model"] != "" && Request.QueryString["status"] != null && Request.QueryString["status"] != "")
            {
                StringBuilder sb = new StringBuilder(strResults);
                intModel = Int32.Parse(Request.QueryString["model"]);
                intStatus = Int32.Parse(Request.QueryString["status"]);
                lblModel.Text = oModelsProperties.Get(intModel, "name");
                DataSet ds = oAsset.GetServerOrBlades(intModel, intStatus);
                int intLocation = 0;
                int intClass = 0;
                int intEnv = 0;
                int intCount = 0;
                bool boolOther = false;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (intLocation > 0 && intClass > 0 && intEnv > 0)
                    {
                        if (intLocation != Int32.Parse(dr["addressid"].ToString()) || intClass != Int32.Parse(dr["classid"].ToString()) || intEnv != Int32.Parse(dr["environmentid"].ToString()))
                        {
                            boolOther = !boolOther;
                            sb.Append("<tr");
                            sb.Append(boolOther ? " bgcolor=\"F6F6F6\"" : "");
                            sb.Append("><td>");
                            sb.Append(oClass.Get(intClass, "name"));
                            sb.Append("</td><td>");
                            sb.Append(oEnvironment.Get(intEnv, "name"));
                            sb.Append("</td><td>");
                            sb.Append(oLocation.GetFull(intLocation));
                            sb.Append("</td><td align=\"right\">");
                            sb.Append(intCount.ToString());
                            sb.Append("</td></tr>");
                            intCount = 0;
                        }
                    }
                    intCount++;
                    intLocation = Int32.Parse(dr["addressid"].ToString());
                    intClass = Int32.Parse(dr["classid"].ToString());
                    intEnv = Int32.Parse(dr["environmentid"].ToString());
                }

                boolOther = !boolOther;

                if (intLocation > 0 && intClass > 0 && intEnv > 0)
                {
                    sb.Append("<tr");
                    sb.Append(boolOther ? " bgcolor=\"F6F6F6\"" : "");
                    sb.Append("><td>");
                    sb.Append(oClass.Get(intClass, "name"));
                    sb.Append("</td><td>");
                    sb.Append(oEnvironment.Get(intEnv, "name"));
                    sb.Append("</td><td>");
                    sb.Append(oLocation.GetFull(intLocation));
                    sb.Append("</td><td align=\"right\">");
                    sb.Append(intCount.ToString());
                    sb.Append("</td></tr>");
                }

                strResults = sb.ToString();
            }
        }
    }
}
