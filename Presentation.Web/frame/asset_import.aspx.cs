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
using System.IO;
using NCC.ClearView.Application.Core;

namespace NCC.ClearView.Presentation.Web
{
    public partial class asset_import : BasePage
    {
        protected string strFile = "";
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Variables oVariables;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["referrer"] != null && Request.QueryString["referrer"] != "")
            {
                oVariables = new Variables(intEnvironment);
                strFile = Request.QueryString["referrer"];
                if (File.Exists(oVariables.DocumentsFolder() + strFile.Replace("/", "\\")) == false)
                    btnTemplate.Attributes.Add("onclick", "alert('There was a problem locating the template...');return false;");
            }
        }
        protected void btnTemplate_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(strFile);
        }
    }
}
