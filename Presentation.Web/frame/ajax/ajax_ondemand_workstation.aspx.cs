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
    public partial class ajax_ondemand_workstation : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            Workstations oWorkstation = new Workstations(0, dsn);
            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                string strResponse = "";
                try
                {
                    XmlDocument oDoc = new XmlDocument();
                    oDoc.Load(Request.InputStream);
                    Response.ContentType = "application/xml";
                    strResponse = "<values>";
                    int intWorkstation = Int32.Parse(oDoc.ChildNodes[0].ChildNodes[0].InnerText);
                    string strStep = oDoc.ChildNodes[0].ChildNodes[1].InnerText;
                    int intStep = oWorkstation.GetVirtualStep(intWorkstation);
                    if (intStep.ToString() == strStep)
                        strResponse += "<value>0</value>";
                    else
                        strResponse += "<value>1</value>";
                    strResponse += "</values>";
                }
                catch { }
                Response.Write(strResponse);
                Response.End();
            }
        }
    }
}
