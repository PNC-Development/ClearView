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

namespace NCC.ClearView.Presentation.Web.DEV
{
    public partial class SIW : System.Web.UI.Page
    {
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private string dsnDW = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ClearViewDWDSN"]].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            Functions oFunction = new Functions(0, dsnDW, intEnvironment);
            DataSet ds = oFunction.ExecuteDataset("SELECT * FROM cvFactoryFeed");

            if (String.IsNullOrEmpty(Request.QueryString["type"]) == false)
            {
                StringBuilder output = new StringBuilder();
                bool file = (String.IsNullOrEmpty(Request.QueryString["output"]) == false && Request.QueryString["output"].Trim().ToUpper() == "FILE");

                if (Request.QueryString["type"].Trim().ToUpper() == "XML")
                {
                    Response.ContentType = "application/xml";
                    if (file)
                        Response.AddHeader("Content-Disposition", "attachment; filename=siw.xml");
                    output.AppendLine("<data>");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        output.AppendLine("<server>");
                        foreach (DataColumn column in ds.Tables[0].Columns)
                        {
                            string data = dr[column.ColumnName].ToString();
                            output.Append("<");
                            output.Append(column.ColumnName);
                            if (String.IsNullOrEmpty(data))
                                output.Append("></");
                            else
                            {
                                output.Append("><![CDATA[");
                                output.Append(data);
                                output.Append("]]></");
                            }
                            output.Append(column.ColumnName);
                            output.AppendLine(">");
                        }
                        output.AppendLine("</server>");
                    }
                    output.AppendLine("</data>");
                    Response.Write(output.ToString());
                    Response.End();
                }
                else if (Request.QueryString["type"].Trim().ToUpper() == "CSV")
                {
                    Response.ContentType = "text/plain";
                    if (file)
                        Response.AddHeader("Content-Disposition", "attachment; filename=siw.csv");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        for (int ii = 0; ii < ds.Tables[0].Columns.Count; ii++)
                        {
                            if (ii > 0)
                                output.Append(",");
                            output.Append(dr[ds.Tables[0].Columns[ii].ColumnName].ToString());
                        }
                        output.AppendLine("");
                    }
                    Response.Write(output.ToString());
                    Response.End();
                }
                else if (Request.QueryString["type"].Trim().ToUpper() == "JSON")
                {
                    Response.ContentType = "text/plain";
                    if (file)
                        Response.AddHeader("Content-Disposition", "attachment; filename=siw.txt");
                    //Response.Write(boolFound);
                    Response.End();
                }
                else if (Request.QueryString["type"].Trim().ToUpper() == "TAB")
                {
                    Response.ContentType = "text/plain";
                    if (file)
                        Response.AddHeader("Content-Disposition", "attachment; filename=siw.txt");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        for (int ii = 0; ii < ds.Tables[0].Columns.Count; ii++)
                        {
                            if (ii > 0)
                                output.Append("\t");
                            output.Append(dr[ds.Tables[0].Columns[ii].ColumnName].ToString());
                        }
                        output.AppendLine("");
                    }
                    Response.Write(output.ToString());
                    Response.End();
                }
            }
            else
            {
                // Help
                StringBuilder columns = new StringBuilder();
                foreach (DataColumn column in ds.Tables[0].Columns)
                {
                    columns.Append("<li>");
                    columns.Append(column.ColumnName);
                    columns.AppendLine("</li>");
                }
                litColumns.Text = columns.ToString();
            }
        }
    }
}
