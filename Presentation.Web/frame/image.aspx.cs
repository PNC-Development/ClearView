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

namespace NCC.ClearView.Presentation.Web
{
    public partial class image : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Users oUser;
        protected Variables oVariable;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oUser = new Users(0, dsn);
            oVariable = new Variables(intEnvironment);
            if (intProfile > 0)
            {
                imgPicture.ImageUrl = "/frame/picture.aspx?xid=" + oUser.GetName(intProfile);
                imgPicture.Style["border"] = "solid 1px #999999";
            }
            imgNew.ImageUrl = "/images/spacer.gif";
            imgNew.Style["border"] = "solid 1px #999999";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "loader", "<script type=\"text/javascript\">StartPic432();<" + "/" + "script>");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            string strFile = oUser.GetName(intProfile) + ".jpg";
            string strPhysical = oVariable.DocumentsFolder() + "photos\\";
            if (Directory.Exists(strPhysical) == false)
                Directory.CreateDirectory(strPhysical);
            if (File.Exists(strPhysical + strFile) == true)
                File.Delete(strPhysical + strFile);
            if (fileUpload.FileName != "" && fileUpload.PostedFile != null)
            {
                string strPath = strPhysical + strFile;
                fileUpload.PostedFile.SaveAs(strPath);
            }
            Response.Redirect(Request.Url.PathAndQuery);
        }
    }
}
