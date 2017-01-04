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
    public partial class service_task_order : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected ServiceDetails oServiceDetail;
        protected Services oService;
        protected int intService = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            oServiceDetail = new ServiceDetails(0, dsn);
            oService = new Services(0, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intService = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">window.parent.navigate(window.parent.location);<" + "/" + "script>");
            if (!IsPostBack)
            {
                lstOrder.DataValueField = "id";
                lstOrder.DataTextField = "name";
                lstOrder.DataSource = oServiceDetail.Gets(intService, 0, 0);
                lstOrder.DataBind();
                imgOrderUp.Attributes.Add("onclick", "return MoveOrderUp(" + lstOrder.ClientID + ");");
                imgOrderDown.Attributes.Add("onclick", "return MoveOrderDown(" + lstOrder.ClientID + ");");
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            string strOrder = Request.Form["hdnUpdateOrder"];
            int intCount = 0;
            while (strOrder != "")
            {
                intCount++;
                int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                oServiceDetail.Update(intId, intCount);
            }
            Response.Redirect(Request.Path + "?save=true");
        }
    }
}
