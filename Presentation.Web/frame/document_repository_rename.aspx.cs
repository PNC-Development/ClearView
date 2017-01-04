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
    public partial class document_repository_rename : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Customized oCustomized;
        protected Icons oIcon;
        protected int intProfile;
        protected int intDocId;
        protected string strPath;
        protected string strType;
        protected string strProfile;
        protected string strApplication;
        protected string strSecurity;
        protected string strOwner;

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            //    Response.Redirect("/redirect.aspx?referrer=/frame/document_repository_rename.aspx?id=" + intDocId);

            oCustomized = new Customized(intProfile, dsn);
            oIcon = new Icons(intProfile, dsn);

            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intDocId = Int32.Parse(Request.QueryString["id"]);
                strType = oCustomized.GetDocumentRepository(intDocId, "type");
                if (!IsPostBack)
                {
                    txtName.Text = oCustomized.GetDocumentRepository(intDocId, "name");
                    if (txtName.Text.EndsWith(strType))
                        txtName.Text = txtName.Text.Substring(0, txtName.Text.IndexOf(strType));
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "loader", "<script type=\"text/javascript\">document.getElementById('" + txtName.ClientID + "').focus();document.getElementById('" + txtName.ClientID + "').select();<" + "/" + "script>");
                }
            }

            // Security code
            strSecurity = oCustomized.GetDocumentRepositorySharesByIds(intDocId, "security");
            if (strSecurity == "")
                strSecurity = oCustomized.GetDocumentRepository(intDocId, "security");
            strOwner = oCustomized.GetDocumentRepositorySharesByIds(intDocId, "ownerid");
            if (strOwner == "")
                strOwner = oCustomized.GetDocumentRepository(intDocId, "profileid");
            strPath = oCustomized.GetDocumentRepository(intDocId, "path");
            strProfile = oCustomized.GetDocumentRepository(intDocId, "profileid");
            strApplication = oCustomized.GetDocumentRepository(intDocId, "applicationid");
            btnRename.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a valid name');");
        }

        protected void btnRename_Click(Object sender, EventArgs e)
        {
            try
            {
                if (strSecurity == "10" || strOwner == intProfile.ToString()) // If owner or share type is 'Edit' then allow else deny the operation
                {
                    if (strType != "Folder")
                    {
                        System.IO.FileInfo oFile = new System.IO.FileInfo(strPath);
                        if (oFile.Exists)
                        {
                            txtName.Text += strType;
                            oFile.MoveTo(oFile.Directory.FullName + "\\" + txtName.Text);
                            oFile.LastAccessTime = DateTime.Now;
                            oFile.LastWriteTime = DateTime.Now;
                            oCustomized.UpdateDocumentRepository(intDocId, txtName.Text, strType, GetVirtualPath(oFile.Directory.FullName + "\\" + txtName.Text));
                        }
                        else
                        {
                            lblError.Text = "The file " + oFile.FullName + " does not exist";
                        }
                    }
                    else
                    {
                        DirectoryInfo oDir = new DirectoryInfo(strPath);
                        if (oDir.Exists)
                        {
                            string strOldPath = oCustomized.GetDocumentRepository(intDocId, "path");
                            string strNewPath = GetVirtualPath(oDir.Parent.FullName + "\\" + txtName.Text);

                            oDir.MoveTo(strNewPath);
                            oDir.LastAccessTime = DateTime.Now;
                            oDir.LastWriteTime = DateTime.Now;
                            DataSet ds = oCustomized.GetDocumentRepositoryByParent(strOldPath);
                            int intCount = 0;
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                int intId = Int32.Parse(dr["id"].ToString());
                                string strTemp1 = dr["path"].ToString();
                                string strTemp2 = dr["parent"].ToString();
                                if (intCount == 0)
                                    oCustomized.UpdateDocumentRepository(intId, txtName.Text, "Folder", "");
                                strTemp1 = strTemp1.Replace(strOldPath, strNewPath);
                                strTemp2 = strTemp2.Replace(strOldPath, strNewPath);
                                oCustomized.UpdateDocumentRepositoryParent(intId, strTemp2, strTemp1);
                                intCount++;
                            }
                        }
                        else
                        {
                            lblError.Text = "The directory " + oDir.FullName + " does not exist";
                        }
                    }
                    ClientScript.RegisterClientScriptBlock(typeof(Page), "redirect", "window.top.navigate(window.top.location);", true);
                }
                else
                    ClientScript.RegisterClientScriptBlock(typeof(Page), "msg", "alert('Access Denied !!');", true);


            }
            catch (Exception exc)
            {
                txtName.Text = "";
                lblError.Text = "Directory/File Rename Error: " + exc.Message;
            }

        }

        public string GetVirtualPath(string physicalPath)
        {
            return physicalPath;
        }
    }
}
