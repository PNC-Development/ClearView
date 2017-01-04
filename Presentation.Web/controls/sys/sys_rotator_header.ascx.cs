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
using System.Xml;
namespace NCC.ClearView.Presentation.Web
{
    public partial class sys_rotator_header : System.Web.UI.UserControl
    {

        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile = 0;
        protected Rotator oRotator;
        protected void Page_Load(object sender, EventArgs e)
        {
            oRotator = new Rotator(intProfile, dsn);
            ds = oRotator.GetHeaders(1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?><Advertisements>");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sb.Append("<Ad>");
                    sb.Append("<ImageUrl><![CDATA[" + dr["imageurl"].ToString() + "]]></ImageUrl>");
                    sb.Append("<Impressions>" + dr["impressions"].ToString() + "</Impressions>");
                    sb.Append("</Ad>");
                }
                sb.Append("</Advertisements>");
                XmlTextReader xtr = new XmlTextReader(sb.ToString(), XmlNodeType.Document, null);
                xtr.ReadOuterXml();
                DataSet dsNew = new DataSet();
                dsNew.ReadXml(xtr);
                adRotator.DataSource = dsNew;
                adRotator.DataBind();
            }
            else
                adRotator.Visible = false;
        }
    }
}