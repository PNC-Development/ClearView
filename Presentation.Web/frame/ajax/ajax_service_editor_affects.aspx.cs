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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class ajax_service_editor_affects : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected ServiceEditor oServiceEditor;
        protected void Page_Load(object sender, EventArgs e)
        {
            oServiceEditor = new ServiceEditor(0, dsnServiceEditor);
            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                XmlDocument oDoc = new XmlDocument();
                oDoc.Load(Request.InputStream);
                Response.ContentType = "application/xml";
                StringBuilder sb = new StringBuilder("<values>");
                string strValues = Server.UrlDecode(oDoc.FirstChild.InnerXml);
                string strValue = strValues.Substring(0, strValues.IndexOf("_"));
                string strConfig = strValues.Substring(strValues.IndexOf("_") + 1);
                int intValue = Int32.Parse(strValue);
                int intConfig = Int32.Parse(strConfig);
                sb.Append(AddResets(intConfig, intValue));
                DataSet dsAffects = oServiceEditor.GetConfigAffectsValue(intValue);
                foreach (DataRow drAffect in dsAffects.Tables[0].Rows)
                {
                    sb.Append("<value>");
                    sb.Append(drAffect["configid"].ToString());
                    sb.Append("</value><text>");
                    sb.Append("inline");
                    sb.Append("</text>");
                }
                sb.Append("</values>");
                Response.Write(sb.ToString());
                Response.End();
            }
        }
        protected string AddResets(int _configid, int _valueid)
        {
            StringBuilder sb = new StringBuilder("");
            DataSet ds = oServiceEditor.GetConfigValues(_configid);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intValue = Int32.Parse(dr["id"].ToString());
                if (intValue != _valueid)
                {
                    DataSet dsAffects = oServiceEditor.GetConfigAffectsValue(intValue);
                    foreach (DataRow drAffect in dsAffects.Tables[0].Rows)
                    {
                        int intConfig = Int32.Parse(drAffect["configid"].ToString());
                        sb.Append("<value>");
                        sb.Append(intConfig.ToString());
                        sb.Append("</value><text>");
                        sb.Append("none");
                        sb.Append("</text>");
                        sb.Append(AddResets(intConfig, 0));
                    }
                }
            }
            return sb.ToString();
        }
    }
}
