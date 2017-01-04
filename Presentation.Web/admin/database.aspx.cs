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
using Microsoft.ApplicationBlocks.Data;
using System.Text;
using System.IO;

namespace NCC.ClearView.Presentation.Web
{
    public partial class database : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected string strResults = "";
        protected string strDays = "";
        protected string strSearch = "";
        protected string strScript = "<tr><td>Enter a script to run</td></tr>";
        protected Functions oFunctions;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/database.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            if (!IsPostBack)
            {
                txtDays.Text = DateTime.Now.ToShortDateString();
                ddlFrom.SelectedValue = "devDSN";
                ddlTo.SelectedValue = "testDSN";
            }
            oFunctions = new Functions(intProfile, dsn, intEnvironment);
            Enhancements oEnhancement = new Enhancements(0, dsn);
            lblVersion.Text = "Version: " + oEnhancement.GetVersion();
            lblDate.Text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();
            btnPrint.Attributes.Add("onclick", "javascript:window.print();return false;");
        }
        protected void btnGo_Click(Object Sender, EventArgs e)
        {
            string strFrom = ConfigurationManager.ConnectionStrings[ddlFrom.SelectedItem.Value].ConnectionString;
            string strTo = ConfigurationManager.ConnectionStrings[ddlTo.SelectedItem.Value].ConnectionString;

            if (chkShow.Checked == true)
            {
                strResults += "<p><b>Tables</b></p>";
                DataSet dsTD = SqlHelper.ExecuteDataset(strFrom, CommandType.Text, "select name from sys.tables order by name");
                foreach (DataRow drTD in dsTD.Tables[0].Rows)
                    strResults += drTD["name"].ToString() + "<br/>";
                strResults += "<p><b>Stored Procedures</b></p>";
                DataSet dsSD = SqlHelper.ExecuteDataset(strFrom, CommandType.Text, "select name from sys.procedures order by name");
                foreach (DataRow drSD in dsSD.Tables[0].Rows)
                    strResults += drSD["name"].ToString() + "<br/>";
            }
            else
            {
                strResults += "<p><b>Add Tables to Production</b></p>";
                DataSet dsTD = SqlHelper.ExecuteDataset(strFrom, CommandType.Text, "select name from sys.tables order by name");
                DataSet dsTP = SqlHelper.ExecuteDataset(strTo, CommandType.Text, "select name from sys.tables order by name");
                foreach (DataRow drTD in dsTD.Tables[0].Rows)
                {
                    bool boolFound = false;
                    foreach (DataRow drTP in dsTP.Tables[0].Rows)
                    {
                        if (drTD["name"].ToString() == drTP["name"].ToString())
                        {
                            boolFound = true;
                            break;
                        }
                    }
                    if (boolFound == false)
                        strResults += drTD["name"].ToString() + "<br/>";
                }

                strResults += "<p><b>Modify Tables in Production</b></p>";
                foreach (DataRow drTD in dsTD.Tables[0].Rows)
                {
                    string strName = "";
                    foreach (DataRow drTP in dsTP.Tables[0].Rows)
                    {
                        if (drTD["name"].ToString() == drTP["name"].ToString())
                        {
                            strName = drTD["name"].ToString();
                            break;
                        }
                    }
                    if (strName != "")
                    {
                        DataSet dsD = SqlHelper.ExecuteDataset(strFrom, CommandType.Text, "select c.name, c.max_length AS length, t.name AS type from sys.columns c LEFT OUTER JOIN sys.types t ON t.system_type_id = c.system_type_id AND t.user_type_id <= 255 where c.object_id =object_id('" + strName + "')");
                        DataSet dsP = SqlHelper.ExecuteDataset(strTo, CommandType.Text, "select c.name, c.max_length AS length, t.name AS type from sys.columns c LEFT OUTER JOIN sys.types t ON t.system_type_id = c.system_type_id AND t.user_type_id <= 255 where c.object_id =object_id('" + strName + "')");
                        string strOld = "";
                        foreach (DataRow drD in dsD.Tables[0].Rows)
                        {
                            bool boolFound = false;
                            foreach (DataRow drP in dsP.Tables[0].Rows)
                            {
                                if (drD["name"].ToString() == drP["name"].ToString())
                                {
                                    boolFound = true;
                                    break;
                                }
                            }
                            if (boolFound == false)
                                strResults += strName + ": ADD " + drD["name"].ToString() + " (after " + strOld + " - " + drD["type"].ToString() + ", " + drD["length"].ToString() + ")<br/>";
                            strOld = drD["name"].ToString();
                        }
                    }
                }

                foreach (DataRow drTD in dsTD.Tables[0].Rows)
                {
                    string strName = "";
                    foreach (DataRow drTP in dsTP.Tables[0].Rows)
                    {
                        if (drTD["name"].ToString() == drTP["name"].ToString())
                        {
                            strName = drTD["name"].ToString();
                            break;
                        }
                    }
                    if (strName != "")
                    {
                        DataSet dsD = SqlHelper.ExecuteDataset(strFrom, CommandType.Text, "select c.name, c.max_length AS length, t.name AS type from sys.columns c JOIN sys.types t ON t.system_type_id = c.system_type_id AND t.user_type_id <= 255 where c.object_id =object_id('" + strName + "')");
                        DataSet dsP = SqlHelper.ExecuteDataset(strTo, CommandType.Text, "select c.name, c.max_length AS length, t.name AS type from sys.columns c JOIN sys.types t ON t.system_type_id = c.system_type_id AND t.user_type_id <= 255 where c.object_id =object_id('" + strName + "')");
                        foreach (DataRow drP in dsP.Tables[0].Rows)
                        {
                            bool boolFound = false;
                            foreach (DataRow drD in dsD.Tables[0].Rows)
                            {
                                if (drP["name"].ToString() == drD["name"].ToString())
                                {
                                    boolFound = true;
                                    break;
                                }
                            }
                            if (boolFound == false)
                                strResults += strName + ": REMOVE " + drP["name"].ToString() + "<br/>";
                        }
                    }
                }

                strResults += "<p><b>Remove Tables from Production</b></p>";
                foreach (DataRow drTP in dsTP.Tables[0].Rows)
                {
                    bool boolFound = false;
                    foreach (DataRow drTD in dsTD.Tables[0].Rows)
                    {
                        if (drTP["name"].ToString() == drTD["name"].ToString())
                        {
                            boolFound = true;
                            break;
                        }
                    }
                    if (boolFound == false)
                        strResults += drTP["name"].ToString() + "<br/>";
                }

                strResults += "<p><b>Add Stored Procedures to Production</b></p>";
                DataSet dsSD = SqlHelper.ExecuteDataset(strFrom, CommandType.Text, "select name from sys.procedures order by name");
                DataSet dsSP = SqlHelper.ExecuteDataset(strTo, CommandType.Text, "select name from sys.procedures order by name");
                foreach (DataRow drSD in dsSD.Tables[0].Rows)
                {
                    bool boolFound = false;
                    foreach (DataRow drSP in dsSP.Tables[0].Rows)
                    {
                        if (drSD["name"].ToString() == drSP["name"].ToString())
                        {
                            boolFound = true;
                            break;
                        }
                    }
                    if (boolFound == false)
                        strResults += drSD["name"].ToString() + "<br/>";
                }

                strResults += "<p><b>Remove Stored Procedures from Production</b></p>";
                foreach (DataRow drSP in dsSP.Tables[0].Rows)
                {
                    bool boolFound = false;
                    foreach (DataRow drSD in dsSD.Tables[0].Rows)
                    {
                        if (drSP["name"].ToString() == drSD["name"].ToString())
                        {
                            boolFound = true;
                            break;
                        }
                    }
                    if (boolFound == false)
                        strResults += drSP["name"].ToString() + "<br/>";
                }

            }
        }
        protected void btnDays_Click(Object Sender, EventArgs e)
        {
            string strFrom = ConfigurationManager.ConnectionStrings[ddlFrom.SelectedItem.Value].ConnectionString;
            string strTo = ConfigurationManager.ConnectionStrings[ddlTo.SelectedItem.Value].ConnectionString;
            DateTime datModified = DateTime.Parse(txtDays.Text);
            DataSet dsSD = SqlHelper.ExecuteDataset(strFrom, CommandType.Text, "select name, modify_date from sys.procedures WHERE modify_date > '" + datModified.ToShortDateString() + "' OR create_date > '" + datModified.ToShortDateString() + "' order by " + ddlOrder.SelectedItem.Value);
            strDays += "<p><b>Stored Procedures in DEV modified on or after " + datModified.ToShortDateString() + "</b></p>";
            foreach (DataRow drSD in dsSD.Tables[0].Rows)
            {
                DateTime datLast = DateTime.Parse(drSD["modify_date"].ToString());
                strDays += drSD["name"].ToString() + " (" + datLast.ToString() + ")<br/>";
            }
        }
        protected void btnSearch_Click(Object Sender, EventArgs e)
        {
            string strSearchDSN = ConfigurationManager.ConnectionStrings[ddlSearch.SelectedItem.Value].ConnectionString;
            string strSearchColumn = txtSearchColumn.Text.Trim();
            string strSearchValue = txtSearchValue.Text.Trim();
            if (strSearchValue != "")
            {
                DataSet dsTable = SqlHelper.ExecuteDataset(strSearchDSN, CommandType.Text, "select name from sys.tables order by name");
                foreach (DataRow drTable in dsTable.Tables[0].Rows)
                {
                    DataSet dsField = SqlHelper.ExecuteDataset(strSearchDSN, CommandType.Text, "select c.name, c.max_length AS length, t.name AS type from sys.columns c LEFT OUTER JOIN sys.types t ON t.system_type_id = c.system_type_id AND t.user_type_id <= 255 where c.object_id =object_id('" + drTable["name"].ToString() + "')");
                    foreach (DataRow drField in dsField.Tables[0].Rows)
                    {
                        if (strSearchColumn == "" || strSearchColumn.ToUpper() == drField["name"].ToString().ToUpper())
                        {
                            DataSet dsResult = SqlHelper.ExecuteDataset(strSearchDSN, CommandType.Text, "SELECT * FROM " + drTable["name"].ToString() + " WHERE [" + drField["name"].ToString() + "] LIKE '%" + strSearchValue + "%'");
                            if (dsResult.Tables[0].Rows.Count > 0)
                                strSearch += drTable["name"].ToString() + "." + drField["name"].ToString() + "<br/>";
                        }
                    }
                }
            }
            else
                strSearch += "Please enter a value";
        }

        protected void btnScript_Click(object sender, EventArgs e)
        {
            StringBuilder script = new StringBuilder();
            try
            {
                string dsnScript = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings[ddlScript.SelectedItem.Value]].ConnectionString;
                DataSet dsScript = SqlHelper.ExecuteDataset(dsnScript, CommandType.Text, txtScript.Text);
                if (!chkScript.Checked)
                    script.AppendLine("<tr>");
                if (dsScript.Tables.Count > 0)
                {
                    foreach (DataColumn dc in dsScript.Tables[0].Columns)
                    {
                        if (!chkScript.Checked)
                            script.Append("<th>");
                        script.Append(dc.ColumnName);
                        if (!chkScript.Checked)
                            script.Append("</th>");
                        else
                            script.Append(",");
                    }
                    if (!chkScript.Checked)
                        script.AppendLine("</tr>");
                    foreach (DataRow dr in dsScript.Tables[0].Rows)
                    {
                        if (!chkScript.Checked)
                            script.AppendLine("<tr>");
                        else
                            script.AppendLine("");
                        foreach (DataColumn dc in dsScript.Tables[0].Columns)
                        {
                            if (!chkScript.Checked)
                                script.Append("<td>");
                            script.Append(dr[dc.ColumnName].ToString());
                            if (!chkScript.Checked)
                                script.Append("</td>");
                            else
                                script.Append(",");
                        }
                        if (!chkScript.Checked)
                            script.AppendLine("</tr>");
                    }
                }
                else if (!chkScript.Checked)
                    script.AppendLine("<td>Script Finished. No results</td></tr>");

                if (chkScript.Checked)
                {
                    Response.Clear();
                    Response.ContentType = "text/csv";
                    Response.AddHeader("Content-Disposition", "attachment;filename=results.csv");
                    Response.Write(script.ToString());
                    Response.End();
                }
                else
                    strScript = script.ToString();
            }
            catch (Exception ex)
            {
                strScript = ex.Message;
                if (ex.InnerException != null)
                    strScript += " ~ " + ex.InnerException.Message;
            }
            Page.ClientScript.RegisterStartupScript(typeof(Page), "dbScript", "<script type=\"text/javascript\">location.href='#dbScript'<" + "/" + "script>");
        }
    }
}
