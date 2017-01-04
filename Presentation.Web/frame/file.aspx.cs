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

namespace NCC.ClearView.Presentation.Web
{
    public partial class file : BasePage
    {
        protected string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Documents oDocument;
        protected Customized oCustomized;
        protected Icons oIcon;
        protected int intProfile;
        protected int intDocument = 0;
        protected int intRepository = 0;
        protected string strContentType = "";
        protected string strPreview = "<b>Preview Not Available</b>";
        protected Variables oVariables;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oVariables = new Variables(intEnvironment);
            Int32.TryParse(Request.QueryString["id"], out intDocument);
            Int32.TryParse(Request.QueryString["repository"], out intRepository);

            if (intDocument > 0 || intRepository > 0)
            {
                oDocument = new Documents(intProfile, dsn);
                oCustomized = new Customized(intProfile, dsn);
                oIcon = new Icons(intProfile, dsn);
                if (intDocument > 0 && oDocument.CanView(intDocument, intProfile) == true)
                {
                    panShow.Visible = true;
                    DataSet ds = oDocument.Get(intDocument);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        System.IO.FileInfo oFile = new System.IO.FileInfo(oVariables.UploadsFolder() + ds.Tables[0].Rows[0]["path"].ToString());
                        string strPath = ds.Tables[0].Rows[0]["path"].ToString();
                        string strName = ds.Tables[0].Rows[0]["name"].ToString();
                        Page.Title = strName;
                        string strExtension = strPath.Substring(strPath.LastIndexOf(".")).ToUpper();
                        imgIcon.ImageUrl = oIcon.GetIcon(strExtension, true);
                        lblTitle.Text = strName;
                        if (oFile.Exists == true)
                        {
                            lblUpdated.Text = oFile.LastWriteTime.ToShortDateString() + " " + oFile.LastWriteTime.ToShortTimeString();
                            lblCreated.Text = oFile.CreationTime.ToShortDateString() + " " + oFile.CreationTime.ToShortTimeString();
                            decimal oFileSize = oFile.Length / 1024;
                            if (oFileSize > 1024)
                                lblSize.Text = (Decimal.Round((oFileSize / 1024), 1)).ToString() + " MB";
                            else
                                lblSize.Text = (Decimal.Round(oFileSize, 0)).ToString() + " KB";
                            imgIcon.Attributes.Add("oncontextmenu", "return RCH('" + strPath.Substring(0, strPath.LastIndexOf(".")) + "');");
                            string strDescription = ds.Tables[0].Rows[0]["description"].ToString();
                            if (strDescription != "")
                            {
                                panDescription.Visible = true;
                                lblDescription.Text = strDescription;
                            }
                            bool boolPreview = false;
                            bool boolIFrame = false;
                            DataSet dsIcon = oIcon.Get(strExtension);
                            if (dsIcon.Tables[0].Rows.Count > 0)
                            {
                                boolPreview = (dsIcon.Tables[0].Rows[0]["preview"].ToString() == "1");
                                boolIFrame = (dsIcon.Tables[0].Rows[0]["iframe"].ToString() == "1");
                                strContentType = dsIcon.Tables[0].Rows[0]["content_type"].ToString();
                            }
                            if (boolPreview == true)
                            {
                                if (boolIFrame == true)
                                    strPreview = "<iframe width=\"100%\" height=\"100%\" frameborder=\"0\" scrolling=\"yes\" src=\"" + oVariables.UploadsFolder() + ds.Tables[0].Rows[0]["path"].ToString() + "\"></iframe>";
                                else if (strContentType != "")
                                    strPreview = "<OBJECT DATA=\"" + oVariables.UploadsFolder() + ds.Tables[0].Rows[0]["path"].ToString() + "\" TYPE=\"" + strContentType + "\" width=\"100%\" height=\"100%\"> ";
                            }
                            switch (ds.Tables[0].Rows[0]["security"].ToString())
                            {
                                case "1":
                                    lblSecurity.Text = "Public";
                                    break;
                                case "0":
                                    lblSecurity.Text = "Shared";
                                    break;
                                case "-1":
                                    lblSecurity.Text = "Private";
                                    break;
                            }
                            btnPermissions.Enabled = true;
                            btnPermissions.Attributes.Add("onclick", "return OpenWindow('FILE_PERMISSIONS','?id=" + intDocument.ToString() + "');");
                            if (oDocument.CanEdit(intDocument, intProfile) == true)
                                btnDelete.Enabled = true;
                            panPreview.Visible = true;
                        }
                        else
                            panDenied.Visible = true;
                    }
                    else
                        panDenied.Visible = true;
                }
                else if (intRepository > 0)
                {
                    panShow.Visible = true;
                    DataSet ds = oCustomized.GetDocumentRepositoryId(intRepository);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        System.IO.FileInfo oFile = new System.IO.FileInfo(ds.Tables[0].Rows[0]["path"].ToString());
                        string strPath = ds.Tables[0].Rows[0]["path"].ToString();
                        string strName = ds.Tables[0].Rows[0]["name"].ToString();
                        Page.Title = strName;
                        string strExtension = strPath.Substring(strPath.LastIndexOf(".")).ToUpper();
                        imgIcon.ImageUrl = oIcon.GetIcon(strExtension, true);
                        lblTitle.Text = strName;
                        if (oFile.Exists == true)
                        {
                            lblUpdated.Text = oFile.LastWriteTime.ToShortDateString() + " " + oFile.LastWriteTime.ToShortTimeString();
                            lblCreated.Text = oFile.CreationTime.ToShortDateString() + " " + oFile.CreationTime.ToShortTimeString();
                            decimal oFileSize = oFile.Length / 1024;
                            if (oFileSize > 1024)
                                lblSize.Text = (Decimal.Round((oFileSize / 1024), 1)).ToString() + " MB";
                            else
                                lblSize.Text = (Decimal.Round(oFileSize, 0)).ToString() + " KB";
                            imgIcon.Attributes.Add("oncontextmenu", "return RCH('" + strPath.Substring(0, strPath.LastIndexOf(".")) + "');");
                            //string strDescription = ds.Tables[0].Rows[0]["description"].ToString();
                            //if (strDescription != "")
                            //{
                            //    panDescription.Visible = true;
                            //    lblDescription.Text = strDescription;
                            //}
                            panDescription.Visible = true;
                            lblDescription.Text = "Document Repository";
                            bool boolPreview = false;
                            bool boolIFrame = false;
                            DataSet dsIcon = oIcon.Get(strExtension);
                            if (dsIcon.Tables[0].Rows.Count > 0)
                            {
                                boolPreview = (dsIcon.Tables[0].Rows[0]["preview"].ToString() == "1");
                                boolIFrame = (dsIcon.Tables[0].Rows[0]["iframe"].ToString() == "1");
                                strContentType = dsIcon.Tables[0].Rows[0]["content_type"].ToString();
                            }
                            if (boolPreview == true)
                            {
                                if (boolIFrame == true)
                                    strPreview = "<iframe width=\"100%\" height=\"100%\" frameborder=\"0\" scrolling=\"yes\" src=\"file:////" + ds.Tables[0].Rows[0]["path"].ToString().Replace("\\","/") + "\"></iframe>";
                                //else if (strContentType != "")
                                //    strPreview = "<OBJECT DATA=\"" + ds.Tables[0].Rows[0]["path"].ToString() + "\" TYPE=\"" + strContentType + "\" width=\"100%\" height=\"100%\"> ";
                                else
                                    strPreview = "<img src=\"/images/hugeAlert.gif\" border=\"0\" align=\"absmiddle\"/> <span class=\"header\">Preview not available. Click &quot;Save File&quot; to view or download the file.</span>";
                            }
                            switch (ds.Tables[0].Rows[0]["security"].ToString())
                            {
                                case "1":
                                    lblSecurity.Text = "Public";
                                    break;
                                case "0":
                                    lblSecurity.Text = "Shared";
                                    break;
                                case "-1":
                                    lblSecurity.Text = "Private";
                                    break;
                            }
                            //btnPermissions.Enabled = true;
                            //btnPermissions.Attributes.Add("onclick", "return OpenWindow('FILE_PERMISSIONS','?id=" + intDocument.ToString() + "');");
                            //if (oDocument.CanEdit(intDocument, intProfile) == true)
                            //    btnDelete.Enabled = true;
                            panPreview.Visible = true;
                        }
                        else
                            panDenied.Visible = true;
                    }
                    else
                        panDenied.Visible = true;
                }
                else
                    panDenied.Visible = true;
            }
            btnClose.Attributes.Add("onclick", "window.close();");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this document?');");
            btnDenied.Attributes.Add("onclick", "window.close();");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            string strPath = "";
            string strName = "";
            string strExtension = "";
            if (intDocument > 0)
            {
                DataSet ds = oDocument.Get(intDocument);
                strPath = oVariables.UploadsFolder() + ds.Tables[0].Rows[0]["path"].ToString();
                strName = ds.Tables[0].Rows[0]["name"].ToString();
                strExtension = strPath.Substring(strPath.LastIndexOf("."));
            }
            else if (intRepository > 0)
            {
                DataSet ds = oCustomized.GetDocumentRepositoryId(intRepository);
                strPath = ds.Tables[0].Rows[0]["path"].ToString();
                strName = ds.Tables[0].Rows[0]["name"].ToString();
                strExtension = strPath.Substring(strPath.LastIndexOf("."));
            }
            if (strName == "")
                strName = "Untitled";
            if (strPath != "" && strName != "" && strExtension != "")
            {
                Response.ContentType = "";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + strName + strExtension);
                Response.TransmitFile(strPath);
                Response.End();
            }
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            bool boolDelete = oDocument.Delete(intDocument, true, oVariables.DocumentsFolder());
            if (boolDelete == true)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "delete", "<script type=\"text/javascript\">window.opener.location.reload();window.close();<" + "/" + "script>");
            else
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "delete", "<script type=\"text/javascript\">alert('There was a problem deleting the file');<" + "/" + "script>");
        }
    }
}