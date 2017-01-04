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
    public partial class document_repository_permissions : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Customized oCustomized;
        protected Icons oIcon;
        protected Functions oFunction;
        protected int intProfile;
        protected int intDocId;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oCustomized = new Customized(intProfile, dsn);
            oIcon = new Icons(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            if (Request.QueryString["docid"] != null && Request.QueryString["docid"] != "")
                Int32.TryParse(oFunction.decryptQueryString(Request.QueryString["docid"]), out intDocId);
            int intApplication = 0;
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            if (intDocId > 0)
            {
                string strSecurity = oCustomized.GetDocumentRepositorySharesByIds(intDocId, "security");
                if (strSecurity == "")
                    strSecurity = oCustomized.GetDocumentRepository(intDocId, "security");
                string strOwner = oCustomized.GetDocumentRepositorySharesByIds(intDocId, "ownerid");
                if (strOwner == "")
                    strOwner = oCustomized.GetDocumentRepository(intDocId, "profileid");
                string strType = oCustomized.GetDocumentRepository(intDocId, "type");
                string strPath = oCustomized.GetDocumentRepository(intDocId, "path");
                string strName = oCustomized.GetDocumentRepository(intDocId, "name");
                string strSize = oCustomized.GetDocumentRepository(intDocId, "size");
                int intIsDepartment = Int32.Parse(oCustomized.GetDocumentRepository(intDocId, "department"));
                int intDepartment = Int32.Parse(oCustomized.GetDocumentRepository(intDocId, "applicationid"));
                if (strSecurity == "1" || strSecurity == "10" || strOwner == intProfile.ToString()
                    || (strSecurity == "-1" && intIsDepartment == 1 && intDepartment == intApplication)
                )
                {
                    if (strType != "Folder" && new System.IO.FileInfo(strPath).Exists == true)
                    {
                        DataSet ds = oIcon.Get(strType.ToUpper());
                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["content_type"].ToString() != "")
                            Response.ContentType = ds.Tables[0].Rows[0]["content_type"].ToString();
                        else
                            Response.Redirect(strPath);
                        //Response.ContentType = "application/octet-stream";                         
                        Response.AppendHeader("content-disposition", "inline;filename=" + strName);
                        Response.AppendHeader("content-length", strSize);
                        Response.WriteFile(strPath);
                        Response.Flush();
                        Response.End();
                    }
                }
                else
                    Response.Write("<script type=\"text/javascript\">alert('Access Denied (" + strSecurity.ToString() + "." + intIsDepartment.ToString() + "." + strOwner + "." + intProfile.ToString() + "." + intDepartment.ToString() + "." + intApplication.ToString() + ")!');window.close();</script>");
            }
            else
                Response.Write("<script type=\"text/javascript\">alert('Access Denied!');window.close();</script>");
        }
    }
}
