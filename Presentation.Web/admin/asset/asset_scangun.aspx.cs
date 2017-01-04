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
using System.Xml;

namespace NCC.ClearView.Presentation.Web
{
    public partial class asset_scangun : BasePage
    {
     
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    protected Platforms oPlatform;
    protected Types oType;
    protected Models oModel;
    protected ModelsProperties oModelsProperties;
    protected int intProfile;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cookies["loginreferrer"].Value = "/admin/asset/asset_scangun.aspx";
        Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
        if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
        else
            Response.Redirect("/admin/login.aspx");
        oPlatform = new Platforms(intProfile, dsn);
        oType = new Types(intProfile, dsn);
        oModel = new Models(intProfile, dsn);
        oModelsProperties = new ModelsProperties(intProfile, dsn);
    }
    protected  void btnGo_Click(Object Sender, EventArgs e)
    {
        XmlDocument oDoc = new XmlDocument();
        string strResponse = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
        strResponse += "<models>";
        DataSet dsPlatform = oPlatform.Gets(1);
        foreach (DataRow drPlatform in dsPlatform.Tables[0].Rows)
        {
            DataSet dsTypes = oType.Gets(Int32.Parse(drPlatform["platformid"].ToString()), 1);
            foreach (DataRow drType in dsTypes.Tables[0].Rows)
            {
                DataSet ds = oModelsProperties.GetTypes(0, Int32.Parse(drType["id"].ToString()), 1);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    strResponse += "<model>";
                    strResponse += "<id><![CDATA[" + dr["id"].ToString() + "]]></id><name><![CDATA[" + dr["name"].ToString() + "]]></name>";
                    strResponse += "</model>";
                }
            }
        }
        strResponse += "</models>";
        Response.Write(strResponse);
        Response.ContentType = "application/xml";
        Response.AddHeader("Content-Disposition", "attachment; filename=models.xml");
        Response.End();
        Response.Flush();
    }

    }
}
