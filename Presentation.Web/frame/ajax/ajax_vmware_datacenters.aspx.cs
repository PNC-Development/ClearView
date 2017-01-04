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
    public partial class ajax_vmware_datacenters : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            VMWare oVMWare = new VMWare(0, dsn);
            if (String.IsNullOrEmpty(Request.QueryString["id"]) == false)
            {
                Response.ContentType = "application/json";
                DataSet ds = oVMWare.GetDatacenters(Int32.Parse(Request.QueryString["id"]), 0);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("JsonReturn([");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sb.Append("{");
                    sb.Append("\"id\":" + dr["id"].ToString());
                    sb.Append(",");
                    sb.Append("\"name\":\"" + dr["name"].ToString() + "\"");
                    sb.Append(",");
                    sb.Append("\"build_folder\":\"" + dr["build_folder"].ToString() + "\"");
                    sb.Append(",");
                    sb.Append("\"enabled\":" + (dr["enabled"].ToString() == "1" ? "true" : "false"));
                    sb.AppendLine("}");
                }
                sb.AppendLine("])");
                Response.Write(sb.ToString());
                Response.End();
            }
        }
    }
}
