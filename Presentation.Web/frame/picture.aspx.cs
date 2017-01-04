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
using System.IO;
using System.Net;
using System.Drawing.Imaging;

namespace NCC.ClearView.Presentation.Web
{
    public partial class picture : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Variables oVariable = new Variables(intEnvironment);
            Users oUser = new Users(0, dsn);
            System.Drawing.Image oImage;
            if (Request.QueryString["xid"] != null && Request.QueryString["xid"] != "")
            {
                string strImage = "";
                string fileName = Request.QueryString["xid"];
                string fileName2 = "";
                int intUser = oUser.GetId(fileName);
                DataSet dsUser = oUser.Get(intUser);
                if (dsUser.Tables[0].Rows.Count > 0)
                {
                    DataRow drUser = dsUser.Tables[0].Rows[0];
                    if (drUser["xid"].ToString().Trim().ToUpper() == fileName.Trim().ToUpper())
                        fileName2 = drUser["pnc_id"].ToString();
                    else if (drUser["pnc_id"].ToString().Trim().ToUpper() == fileName.Trim().ToUpper())
                        fileName2 = drUser["xid"].ToString();
                }

                if (fileName.ToUpper() == oVariable.ADUser().ToUpper() || (fileName2 != "" && fileName2.ToUpper() == oVariable.ADUser().ToUpper()))
                {
                    // ClearView User
                    strImage = Request.PhysicalApplicationPath + "\\images\\clearview.gif";
                    oImage = System.Drawing.Image.FromFile(strImage);
                    oImage.Save(Response.OutputStream, ImageFormat.Jpeg);
                    oImage.Dispose();
                    oImage = null;
                }
                else
                {
                    string strPhysical = oVariable.DocumentsFolder() + "photos\\";
                    if (File.Exists(strPhysical + fileName + ".jpg") == false && (fileName2 == "" || File.Exists(strPhysical + fileName2 + ".jpg") == false))
                    {
                        strImage = Request.PhysicalApplicationPath + "\\images\\nophoto.gif";
                        oImage = System.Drawing.Image.FromFile(strImage);
                        oImage.Save(Response.OutputStream, ImageFormat.Jpeg);
                        oImage.Dispose();
                        oImage = null;
                    }
                    else
                    {
                        if (Directory.Exists(strPhysical) == false)
                            Directory.CreateDirectory(strPhysical);
                        string strTemp = strPhysical + "temp\\";
                        if (Directory.Exists(strTemp) == false)
                            Directory.CreateDirectory(strTemp);

                        if (File.Exists(strPhysical + fileName + ".jpg") == true)
                        {
                            strImage = strPhysical + fileName + ".jpg";
                            oImage = System.Drawing.Image.FromFile(strImage);
                            oImage.Save(Response.OutputStream, ImageFormat.Jpeg);
                            oImage.Dispose();
                            oImage = null;
                        }
                        else
                        {
                            strImage = strPhysical + fileName2 + ".jpg";
                            oImage = System.Drawing.Image.FromFile(strImage);
                            oImage.Save(Response.OutputStream, ImageFormat.Jpeg);
                            oImage.Dispose();
                            oImage = null;
                        }
                    }
                }
            }
        }
    }
}
