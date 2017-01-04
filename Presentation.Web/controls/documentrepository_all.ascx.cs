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
using NCC.ClearView.Presentation.Web.Custom;
using System.Net.NetworkInformation;

namespace NCC.ClearView.Presentation.Web
{
    public partial class documentrepository_all : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected Pages oPage;
        protected Users oUser;
        protected Customized oCustomized;
        protected Variables oVariable;
        protected Applications oApplication;
        protected Functions oFunction;

        protected bool boolApp = true;
        protected bool boolOthers = false;
        protected string strDirectory = "";
        protected string strHome;
        private DirectoryInfo dir_info;
        protected string strFolder = "";
        protected string strRedirect = "";
        protected string strMenuTab1 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oApplication = new Applications(intProfile, dsn);
            oFunction = new Functions(0, dsn, intEnvironment);
            string strNavigation = "";

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            //Menus
            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
            //Tab oTab = new Tab("", intMenuTab, "divMenu1", true, false);
            Tab oTab = new Tab(hdnType.ClientID, intMenuTab, "divMenu1", true, false);
            oTab.AddTab("&quot;" + oApplication.GetName(intApplication) + "&quot; Documents", "");
            oTab.AddTab("Documents Shared by Other Departments", "");
            strMenuTab1 = oTab.GetTabs();
            //End Menus

            lblTitle.Text = oPage.Get(intPage, "title");
            strRedirect = oPage.GetFullLink(intPage);

            if (lblCurrent.Text == "")
            {
                lblCurrent.Text = oVariable.DocumentsFolder() + "department\\" + intApplication.ToString();
                if (!Directory.Exists(lblCurrent.Text))
                {
                    DirectoryInfo drinfo = new DirectoryInfo(lblCurrent.Text);
                    drinfo.Create();
                    drinfo.LastAccessTime = DateTime.Now;
                    drinfo.LastWriteTime = DateTime.Now;
                }
            }
            strHome = lblCurrent.Text;
            strDirectory = strHome;

            if (strFolder == string.Empty)
            {
                strFolder = "\\";
            }

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["folder"] != null && Request.QueryString["folder"] != "")
                {
                    string strFolderTemp = Request.QueryString["folder"];

                    if (!strFolderTemp.StartsWith("\\"))
                    {
                        strFolderTemp = "\\" + strFolderTemp;
                    }

                    if (Directory.Exists(strHome + strFolderTemp) == true)
                    {
                        strFolder = strFolderTemp;
                        strDirectory = strHome + strFolderTemp;
                    }
                }

                dir_info = new DirectoryInfo(strDirectory);

                if (dir_info.Parent.Parent.Name != "documents")
                {
                    string strNav = strFolder;
                    while (strNav != "")
                    {
                        strNavigation = " / <a class=\"breadcrumb\" href=\"" + strRedirect + "?folder=" + Server.UrlEncode(strNav) + "\">" + strNav.Substring(strNav.LastIndexOf("\\") + 1) + "</a>" + strNavigation;
                        strNav = strNav.Substring(0, strNav.LastIndexOf("\\"));
                    }
                    strRedirect = oPage.GetFullLink(intPage) + "?folder=" + strFolder.Substring(0, strFolder.LastIndexOf("\\"));
                    strFolder = "<a href=\"" + strRedirect + "\">" + strFolder.Substring(0, strFolder.LastIndexOf("\\")) + "</a>" + strFolder.Substring(strFolder.LastIndexOf("\\"));
                }
                DataSet ds = oCustomized.GetDocumentRepositoryApplication(intApplication, strDirectory);
                DataView dv = ds.Tables[0].DefaultView;
                if (Request.QueryString["sort"] != null && Request.QueryString["sort"] != "")
                {
                    if (Request.QueryString["sort"].ToLower().Contains("desc"))
                        dv.Sort = "type desc," + Request.QueryString["sort"];
                    else
                        dv.Sort = "type," + Request.QueryString["sort"];
                }
                else
                    dv.Sort = "type desc";

                rptDocs.DataSource = dv;
                rptDocs.DataBind();
                lblNoDocs.Visible = rptDocs.Items.Count == 0;

                DataSet dsOthers = oCustomized.GetDocumentRepositorySharesByApplication(intApplication);
                dsOthers.Relations.Add("relationship", dsOthers.Tables[0].Columns["applicationid"], dsOthers.Tables[1].Columns["applicationid"], false);
                DataView dvOthers = dsOthers.Tables[0].DefaultView;
                rptOthers.DataSource = dvOthers;
                rptOthers.DataBind();
                lblNoShares.Visible = rptOthers.Items.Count == 0;

                foreach (RepeaterItem ri in rptDocs.Items)
                {
                    LinkButton btnName = (LinkButton)ri.FindControl("btnName");
                    Label lblType = (Label)ri.FindControl("lblType");
                    Label lblDeleted = (Label)ri.FindControl("lblDeleted");
                    Label lblId = (Label)ri.FindControl("lblId");
                    Label lblSize = (Label)ri.FindControl("lblSize");

                    ImageButton imgDelete = (ImageButton)ri.FindControl("imgDelete");
                    ImageButton imgRename = (ImageButton)ri.FindControl("imgRename");
                    ImageButton imgShare = (ImageButton)ri.FindControl("imgShare");
                    imgRename.Attributes.Add("onclick", "return OpenWindow('DOCUMENT_REPOSITORY_RENAME','?id=" + lblId.Text + "');");
                    imgShare.Attributes.Add("onclick", "return OpenWindow('DOCUMENT_REPOSITORY_SHARE','?id=" + imgShare.CommandArgument + "');");
                    imgDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this " + (lblType.Text.ToLower() != "folder" ? "File" : lblType.Text) + " ?');");

                    if (!FileExists(btnName.CommandArgument, lblType.Text))
                    {
                        btnName.Text = btnName.Text + " (Not Found)";
                        btnName.Enabled = false;
                        imgRename.Visible = false;
                        imgShare.Visible = false;
                    }
                    else
                    {
                        if (lblType.Text.ToLower() != "folder")
                        {
                            btnName.Text = "<a href=\"" + oVariable.URL() + "/frame/document_repository_permissions.aspx?docid=" + oFunction.encryptQueryString(lblId.Text) + "\" target=\"_blank\">" + btnName.Text + "</a>";
                            //btnName.Text = "<a href=\"" + btnName.CommandArgument + "\" target=\"_blank\">" + btnName.Text + "</a>";
                            int intSize = Int32.Parse(lblSize.Text);
                            decimal decSize = intSize / 1024;
                            if (decSize > 1024)
                                lblSize.Text = (Decimal.Round((decSize / 1024), 1)).ToString() + " MB";
                            else
                                lblSize.Text = (Decimal.Round(decSize, 0)).ToString() + " KB";
                        }
                        else
                            imgShare.Visible = false;
                    }
                }

                foreach (RepeaterItem riOther in rptOthers.Items)
                {
                    Repeater rptShared = (Repeater)riOther.FindControl("rptShared");
                    foreach (RepeaterItem ri in rptShared.Items)
                    {
                        LinkButton btnName = (LinkButton)ri.FindControl("btnName");
                        Label lblType = (Label)ri.FindControl("lblType");
                        Label lblId = (Label)ri.FindControl("lblId");
                        Label lblOwnerId = (Label)ri.FindControl("lblOwnerId");
                        Label lblDeleted = (Label)ri.FindControl("lblDeleted");
                        Label lblSize = (Label)ri.FindControl("lblSize");

                        if (!FileExists(btnName.CommandArgument, lblType.Text))
                        {
                            btnName.Text = btnName.Text + " (Not Found)";
                            btnName.Enabled = false;
                        }
                        else
                        {
                            if (lblType.Text.ToLower() != "folder")
                            {
                                btnName.Text = "<a href=\"" + oVariable.URL() + "/frame/document_repository_permissions.aspx?docid=" + oFunction.encryptQueryString(lblId.Text) + "\" target=\"_blank\">" + btnName.Text + "</a>";
                                int intSize = Int32.Parse(lblSize.Text);
                                decimal decSize = intSize / 1024;
                                if (decSize > 1024)
                                    lblSize.Text = (Decimal.Round((decSize / 1024), 1)).ToString() + " MB";
                                else
                                    lblSize.Text = (Decimal.Round(decSize, 0)).ToString() + " KB";
                            }
                        }
                    }
                }

                lblTitle.Text = oPage.Get(intPage, "title");
                lblFolder.Text = strFolder != "" ? "<a class=\"breadcrumb\" href=\"" + oPage.GetFullLink(intPage) + "\">Home</a>" + strNavigation : "<a class=\"breadcrumb\" href=\"" + oPage.GetFullLink(intPage) + "\">Home</a>";
                hdnDir.Value = dir_info.FullName;
                btnCreate.Attributes.Add("onclick", "return ValidateText('" + txtDirectory.ClientID + "','Please enter a name for the new directory') && ValidateDirName('" + txtDirectory.ClientID + "') && ProcessButton(this);");
                btnUpload.Attributes.Add("onclick", "return ProcessButton(this);");
                txtDirectory.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnCreate.ClientID + "').click();return false;}} else {return true}; ");
            }
        }

        protected bool FileExists(string strPath, string strType)
        {
            if (strPath.ToUpper().StartsWith(oVariable.DocumentsFolder().ToUpper()) == true)
            {
                string strServer = oVariable.ParseDocument(strPath);
                Ping oPing = new Ping();
                string strPingStatus = "";
                try
                {
                    PingReply oPingReply = oPing.Send(strServer, 1000);
                    strPingStatus = oPingReply.Status.ToString().ToUpper();
                }
                catch { }
                bool boolPingSuccess = (strPingStatus == "SUCCESS");
                if (boolPingSuccess == true)
                {
                    if (strType.ToLower() == "folder")
                        return Directory.Exists(strPath);
                    else
                        return new System.IO.FileInfo(strPath).Exists;
                }
                else
                    return false;
            }
            else
                return false;
        }

        protected string GetIcon(string txt)
        {
            string iconame;

            switch (txt.ToLower().Replace(".", ""))
            {
                case "bmp":
                case "gif":
                case "jpg":
                case "tif":
                case "jpeg":
                case "tiff":
                    iconame = "jpg.gif";
                    break;
                case "doc":
                case "rtf":
                    iconame = "doc.gif";
                    break;
                case "exe":
                case "bat":
                case "bas":
                case "c":
                case "src":
                    iconame = "exe.gif";
                    break;
                case "htm":
                case "html":
                case "asa":
                case "asp":
                case "cfm":
                case "php3":
                case "inc":
                case "aspx":
                case "css":
                case "ascx":
                    iconame = "html.gif";
                    break;

                case "cs":
                case "vb":
                case "asmx":
                    iconame = "ascx.gif";
                    break;
                case "wav":
                case "mp3":
                case "mpg":
                case "avi":
                case "asf":
                case "rm":
                case "mov":
                    iconame = "wav.gif";
                    break;
                case "txt":
                case "ini":
                    iconame = "txt.gif";
                    break;
                case "zip":
                case "arc":
                case "sit":
                case "rar":
                    iconame = "zip.gif";
                    break;
                case "xls":
                case "csv":
                    iconame = "xls.gif";
                    break;
                default:
                    iconame = "unknown.gif";
                    break;
            }

            return "/images/icons/" + iconame;
        }

        protected void btnCreate_Click(Object Sender, EventArgs E)
        {
            string strNewDir;
            try
            {
                strNewDir = hdnDir.Value + "\\" + txtDirectory.Text;
                DirectoryInfo dir = new DirectoryInfo(strNewDir);
                if (dir.Exists)
                    lblError.Text = "The directory named " + txtDirectory.Text + " already exists";
                else
                {
                    dir.Create();
                    dir.LastAccessTime = DateTime.Now;
                    dir.LastWriteTime = DateTime.Now;
                    WriteLog("Created the directory " + strNewDir);
                    int intId = oCustomized.AddDocumentRepository(intApplication, intProfile, txtDirectory.Text, "Folder", 1, strNewDir, hdnDir.Value, 0);

                    if (strHome != null)
                    {
                        strNewDir = strNewDir.Remove(0, strHome.Trim().Length);
                    }

                    Response.Redirect(oPage.GetFullLink(intPage) + "?folder=" + Server.UrlEncode(strNewDir));
                }
            }
            catch (Exception exc)
            {
                lblError.Text = "Error in creating directory: " + exc.Message;
            }
        }

        protected void btnUpload_Click(Object Sender, EventArgs E)
        {
            string strNewDir = "";
            try
            {
                if (txtFile.FileName != "" && txtFile.PostedFile != null)
                {
                    string strFile = txtFile.PostedFile.FileName.Trim();
                    string strFileName = strFile.Substring(strFile.LastIndexOf("\\") + 1);
                    string strExtension = txtFile.FileName;
                    strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                    strNewDir = hdnDir.Value + "\\" + strFileName;
                    txtFile.PostedFile.SaveAs(strNewDir);
                    string strParent = new DirectoryInfo(strNewDir).Parent.Name;
                    oCustomized.AddDocumentRepository(intApplication, intProfile, strFileName, strExtension, 1, strNewDir, hdnDir.Value, txtFile.PostedFile.ContentLength);
                    WriteLog("Uploaded the document " + strFileName + " to the " + strNewDir + " folder");

                    if (strHome != null)
                    {
                        strNewDir = hdnDir.Value.Remove(0, strHome.Trim().Length);
                    }

                    Response.Redirect(oPage.GetFullLink(intPage) + "?folder=" + Server.UrlEncode(strNewDir));
                }
                else
                {
                    lblError.Text = "You must select a file before uploading!";
                }
            }
            catch (Exception exc)
            {
                lblError.Text = "Error in uploading file: " + exc.Message;
            }
        }
        protected void WriteLog(string strDesc)
        {
            string strFile = ConfigurationManager.AppSettings["LOG_FILE"];
            strFile = oVariable.DocumentsFolder() + strFile;
            StreamWriter sw;
            if (File.Exists(strFile))
                sw = new StreamWriter(strFile, true);
            else
                sw = File.CreateText(strFile);
            sw.WriteLine(Page.User.Identity.Name + ": " + DateTime.Now + " - " + strDesc);
            sw.Close();
        }

        protected void Name_Click(Object Sender, EventArgs e)
        {
            LinkButton but = (LinkButton)Sender;
            string strTemp = but.CommandArgument;
            string strPath = strTemp.Substring(strTemp.IndexOf(intApplication.ToString())).Replace(intApplication.ToString(), "");
            Response.Redirect(oPage.GetFullLink(intPage) + "?folder=" + strPath);
        }

        protected void imgDelete_Click(Object Sender, EventArgs e)
        {
            int intId = 0;
            ImageButton button = (ImageButton)Sender;
            string strFolderTemp = string.Empty;

            if (button.CommandArgument != "")
            {
                intId = Int32.Parse(button.CommandArgument);
                oCustomized.DeleteDocumentRepository(intId);
                string strPath = oCustomized.GetDocumentRepository(intId, "path");

                if (strHome != null)
                {
                    strFolderTemp = strPath.Remove(0, strHome.Trim().Length);
                    strFolderTemp = strFolderTemp.Substring(0, strFolderTemp.LastIndexOf("\\"));
                }

                if (oCustomized.GetDocumentRepository(intId, "type").ToLower() == "folder")
                {
                    if (Directory.Exists(strPath) == true)
                        Directory.Delete(strPath, true);
                }
                else
                {
                    if (File.Exists(strPath) == true)
                        File.Delete(strPath);
                }

                WriteLog("Delete file " + strPath);
            }
            
            Response.Redirect(oPage.GetFullLink(intPage) + "?folder=" + strFolderTemp);
        }

        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            LinkButton oOrder = (LinkButton)Sender;
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
            {
                if (Request.QueryString["sort"] == oOrder.CommandArgument)
                    strOrder = oOrder.CommandArgument + " DESC";
            }
            if (strOrder == "")
                strOrder = oOrder.CommandArgument;

            string strFolderTemp = Request.QueryString["folder"];

            if (strFolderTemp != null && !strFolderTemp.StartsWith("\\"))
            {
                strFolderTemp = "\\" + strFolderTemp;
            }

            Response.Redirect(oPage.GetFullLink(intPage) + "?folder=" + strFolderTemp + "&sort=" + strOrder);
        }
    }
}