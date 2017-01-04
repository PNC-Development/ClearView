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
using System.IO;
namespace NCC.ClearView.Presentation.Web
{
    public partial class fix_vsg : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected string strResults = "";
        protected int intCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/fix_vsg.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            btnGo.Attributes.Add("onclick", "return confirm('This function will modify production data and cannot be reversed!\\n\\nAre you sure you want to continue?') && ProcessButton(this);");
        }
        protected  void btnGo_Click(Object Sender, EventArgs e)
        {
            Asset oAsset = new Asset(0, dsnAsset);
            Variables oVariable = new Variables(intEnvironment);
            if (oFile.PostedFile != null && oFile.FileName != "")
            {
                string strExtension = oFile.FileName;
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                string strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + intProfile.ToString() + strExtension;
                string strDirectory = oVariable.DocumentsFolder() + "imports";
                if (Directory.Exists(strDirectory) == false)
                    Directory.CreateDirectory(strDirectory);
                string strPath = strDirectory + "\\" + strFile;
                oFile.PostedFile.SaveAs(strPath);
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strPath + ";Extended Properties=Excel 8.0;";
                DataSet ds = new DataSet();
                OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
                myCommand.Fill(ds, "ExcelInfo");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[0].ToString().Trim() == "")
                        break;
                    oAsset.AddVSG(dr[0].ToString().Trim(), ddlType.SelectedItem.Value);
                    intCount++;
                }
                lblDone.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView imported " + intCount.ToString() + " VSG numbers successfully";
            }
        }
    }
}
