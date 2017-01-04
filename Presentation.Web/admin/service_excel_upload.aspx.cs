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
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.ApplicationBlocks.Data;
using System.Data.OleDb;

namespace NCC.ClearView.Presentation.Web
{
    public partial class service_excel_upload : BasePage
    {
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceDSN"]].ConnectionString;
        protected string strConn;
        protected int intProfile;
        private static Hashtable table;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/service_excel_upload.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");

            if (Request.QueryString["success"] != "" && Request.QueryString["success"] != null)
            {
                string strMsg = "";
                if (Request.QueryString["type"] == "CHG")
                    strMsg = "alert('Excel file successfully imported into clearview service db!!\\n\\t Total files exported = " + Request.QueryString["count"] + "');";
                else
                    strMsg = "alert('Excel file successfully imported into clearview service db!!\\n\\t Total files exported (ack) = " + Request.QueryString["count1"] + "\\n\\t Total files exported (non-ack) = " + Request.QueryString["count2"] + "');";

                if (!IsPostBack)
                    ClientScript.RegisterClientScriptBlock(typeof(Page), "success", strMsg, true);

            }
            if (Request.QueryString["export"] != "" && Request.QueryString["export"] != null)
                ClientScript.RegisterClientScriptBlock(typeof(Page), "no export", "alert('Nothing to export!!');", true);
        }


        protected void btnLoad1_Click(Object Sender, EventArgs e)
        {
            if (fileUpload.PostedFile != null && fileUpload.FileName != "")
            {
                try
                {
                    string strExtension = fileUpload.FileName;
                    string strType = strExtension.Substring(0, 3);
                    strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                    if (strExtension != ".xls") throw new Exception("Only .xls files are allowed");
                    string strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + intProfile.ToString() + strExtension;
                    string strPath = Request.PhysicalApplicationPath + "imports\\" + strFile;
                    fileUpload.PostedFile.SaveAs(strPath);
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strPath + ";Extended Properties=Excel 8.0;";

                    DataSet ds = new DataSet();
                    if (strType == "CHG")
                    {
                        SqlConnection conn = new SqlConnection(dsn);
                        SqlTransaction transact = null;
                        try
                        {

                            conn.Open();
                            transact = conn.BeginTransaction();

                            DataSet ds2 = SqlHelper.ExecuteDataset(transact, CommandType.Text, "select top 1 * from cvs_change");
                            ds2.Tables[0].Columns.Remove("id");
                            SqlHelper.ExecuteNonQuery(transact, CommandType.Text, "DELETE FROM cvs_change;DBCC CHECKIDENT('cvs_change',reseed,0);");
                            OleDbDataAdapter myCommand1 = new OleDbDataAdapter("SELECT * FROM [Change Report$]", strConn);
                            myCommand1.Fill(ds, "ExcelInfo");
                            ds.Tables.Add(ds2.Tables[0].Copy());

                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                foreach (DataColumn dc in ds.Tables[0].Columns)
                                {
                                    if (dr[dc] != DBNull.Value)
                                    {
                                        dr[dc] = dr[dc].ToString().Trim();
                                    }
                                }
                            }
                            IList<SqlBulkCopyColumnMapping> mappings = FetchMappings(ref ds, "");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                using (SqlBulkCopy bulk = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transact))
                                {
                                    foreach (SqlBulkCopyColumnMapping mapping in mappings)
                                    {
                                        bulk.ColumnMappings.Add(mapping);

                                    }

                                    bulk.BatchSize = 500;
                                    bulk.DestinationTableName = "cvs_change";
                                    bulk.WriteToServer(ds.Tables[0]);
                                    transact.Commit();
                                }
                                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "delete from cvs_change where change_number is null and implementor_name is null or lower(implementor_name) in ('summary','-  1  -')");
                                Response.Redirect(Request.Path + "?success=true&type=CHG&count=" + ds.Tables[0].Rows.Count.ToString());
                            }
                            else
                            {
                                transact.Rollback();
                                Response.Redirect(Request.Path + "?export=false");
                            }

                        }
                        catch (Exception exc)
                        {
                            transact.Rollback();
                            lblError.Text = "Export Error(CHG): " + exc.Message;
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                    else if (strType == "INC")
                    {
                        SqlConnection conn = new SqlConnection(dsn);
                        SqlTransaction transact = null;
                        try
                        {
                            conn.Open();
                            transact = conn.BeginTransaction();

                            SqlHelper.ExecuteNonQuery(transact, CommandType.Text, "DELETE FROM cvs_incident;DBCC CHECKIDENT('cvs_incident',reseed,0);");
                            OleDbDataAdapter myCommand1 = new OleDbDataAdapter("SELECT * FROM [Page1-1$]", strConn);
                            myCommand1.Fill(ds, "ExcelInfo");

                            DataSet ds2 = SqlHelper.ExecuteDataset(transact, CommandType.Text, "select top 1 * from cvs_incident");
                            ds2.Tables[0].Columns.Remove("id");
                            ds.Tables.Add(ds2.Tables[0].Copy());

                            table = new Hashtable();
                            foreach (DataColumn dct in ds2.Tables[0].Columns)
                                table.Add(dct.ColumnName, dct.Ordinal);

                            DataSet ds3 = new DataSet();
                            OleDbDataAdapter myCommand2 = new OleDbDataAdapter("SELECT * FROM [Page2-2$]", strConn);
                            myCommand2.Fill(ds3, "ExcelInfo");
                            ds3.Tables.Add(ds2.Tables[0].Copy());

                            IList<SqlBulkCopyColumnMapping> mappings1 = FetchMappings(ref ds, "Ack");
                            IList<SqlBulkCopyColumnMapping> mappings2 = FetchMappings(ref ds3, "Non-Ack");


                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                foreach (DataColumn dc in ds.Tables[0].Columns)
                                {
                                    if (dr[dc] != DBNull.Value)
                                        dr[dc] = dr[dc].ToString().Trim();
                                }
                            }

                            foreach (DataRow dr2 in ds3.Tables[0].Rows)
                            {
                                foreach (DataColumn dc2 in ds3.Tables[0].Columns)
                                {
                                    if (dr2[dc2] != DBNull.Value)
                                        dr2[dc2] = dr2[dc2].ToString().Trim();
                                }
                            }
                            if (ds.Tables[0].Rows.Count > 0 || ds3.Tables[0].Rows.Count > 0)
                            {
                                using (SqlBulkCopy bulk = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transact))
                                {
                                    bulk.BatchSize = 500;
                                    bulk.DestinationTableName = "cvs_incident";
                                    foreach (SqlBulkCopyColumnMapping mapping in mappings1)
                                    {
                                        bulk.ColumnMappings.Add(mapping);
                                    }
                                    bulk.WriteToServer(ds.Tables[0]);
                                    bulk.ColumnMappings.Clear();
                                    foreach (SqlBulkCopyColumnMapping mapping in mappings2)
                                    {
                                        bulk.ColumnMappings.Add(mapping);
                                    }
                                    bulk.WriteToServer(ds3.Tables[0]);
                                    transact.Commit();
                                    Response.Redirect(Request.Path + "?success=true&type=INC&count1=" + ds.Tables[0].Rows.Count + "&count2=" + ds3.Tables[0].Rows.Count);
                                }
                            }
                            else
                            {
                                transact.Rollback();
                                Response.Redirect(Request.Path + "?export=false");
                            }

                        }
                        catch (Exception exc)
                        {
                            transact.Rollback();
                            lblError.Text = "Export Error (INC): " + exc.Message;
                        }
                        finally
                        {
                            conn.Close();
                        }

                    }
                }
                catch (Exception exc)
                {
                    lblError.Text = "Export Error: " + exc.Message;
                }
            }
            else
                ClientScript.RegisterStartupScript(this.GetType(), "File_Missing_Message", "alert('Please select a file to upload');", true);
        }


        private static IList<SqlBulkCopyColumnMapping> FetchMappings(ref DataSet ds, string strAck)
        {
            IList<SqlBulkCopyColumnMapping> mappings = new List<SqlBulkCopyColumnMapping>();

            if (strAck == "Ack")
            {
                DataColumn dc1 = new DataColumn("non_ack_placeholder", typeof(string));
                dc1.DefaultValue = "0";
                ds.Tables[0].Columns.Add(dc1);

                DataColumn dc2 = new DataColumn("total_nonack", typeof(int));
                dc2.DefaultValue = 0;
                ds.Tables[0].Columns.Add(dc2);

                DataColumn dc3 = new DataColumn("isack", typeof(int));
                dc3.DefaultValue = 1;
                ds.Tables[0].Columns.Add(dc3);
            }
            else if (strAck == "Non-Ack")
            {

                DataColumn dc1 = new DataColumn("ack", typeof(string));
                dc1.DefaultValue = "";
                ds.Tables[0].Columns.Add(dc1);
                dc1.SetOrdinal((int)table["ack"]);

                DataColumn dc2 = new DataColumn("description_other_ack", typeof(string));
                dc2.DefaultValue = "";
                ds.Tables[0].Columns.Add(dc2);
                dc2.SetOrdinal((int)table["description_other_ack"]);

                DataColumn dc3 = new DataColumn("count_ack", typeof(int));
                dc3.DefaultValue = 0;
                ds.Tables[0].Columns.Add(dc3);
                dc3.SetOrdinal((int)table["count_ack"]);

                DataColumn dc4 = new DataColumn("isack", typeof(int));
                dc4.DefaultValue = 0;
                ds.Tables[0].Columns.Add(dc4);
                dc4.SetOrdinal((int)table["isack"]);

            }

            DataColumn dc = new DataColumn("modified", typeof(DateTime));
            dc.DefaultValue = DateTime.Now.ToString();
            ds.Tables[0].Columns.Add(dc);

            for (int i = 0; i < ds.Tables[1].Columns.Count; i++)
            {
                mappings.Add(new SqlBulkCopyColumnMapping(ds.Tables[0].Columns[i].ColumnName, ds.Tables[1].Columns[i].ColumnName));

            }
            return mappings;
        }

    }
}
