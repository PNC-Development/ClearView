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
using System.Data.OleDb;
namespace NCC.ClearView.Presentation.Web
{
    public partial class mnemonic_import : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Mnemonic oMnemonic;
        protected int intProfile;
        protected int intCount;
        protected int intDuplicate;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/mnemonic_import.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oMnemonic = new Mnemonic(intProfile, dsn);
        }
        protected  void btnLoad1_Click(Object Sender, EventArgs e)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=f:\\Mnemonic.xls;Extended Properties=Excel 8.0;";
            OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Mnemonic$]", strConn);
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "ExcelInfo");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr[0].ToString().Trim() == "")
                    break;
                string strCode = dr[0].ToString().Trim();
                string strName = dr[1].ToString().Trim();
                DataSet dsImport = oMnemonic.Get(strName, strCode);
                if (dsImport.Tables[0].Rows.Count == 0)
                {
                    oMnemonic.Add(strName, strCode, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 1);
                    intCount++;
                }
                else
                    intDuplicate++;
            }
            Response.Write("Done = " + intCount.ToString() + "<br/>");
            Response.Write("Duplicate = " + intDuplicate.ToString() + "<br/>");
        }
    }
}
