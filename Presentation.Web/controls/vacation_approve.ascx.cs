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
    public partial class vacation_approve : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Vacation oVacation;
        protected Pages oPage;
        protected Users oUser;
        protected Functions oFunction;
        protected Variables oVariable;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strBody = "";
          private string strEMailIdsBCC = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oVacation = new Vacation(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oVariable = new Variables(intEnvironment);
            if (Request.QueryString["action"] != null && Request.QueryString["action"] != "")
                panFinish.Visible = true;
            else
            {
                if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                    intApplication = Int32.Parse(Request.QueryString["applicationid"]);
                if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                    intPage = Int32.Parse(Request.QueryString["pageid"]);
                if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                    intApplication = Int32.Parse(Request.Cookies["application"].Value);
                int intVacation = 0;
                if (!IsPostBack)
                {
                    if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                    {
                        intVacation = Int32.Parse(Request.QueryString["id"]);
                        lblVacation.Text = Request.QueryString["id"];
                        int intRequester = Int32.Parse(oVacation.Get(intVacation, "userid"));
                        if (intVacation > 0)
                        {
                            if (oUser.IsManager(intRequester, intProfile, true))
                            {
                                panRequest.Visible = true;
                                strBody = oVacation.GetBody(intVacation, intEnvironment);
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
            }
            btnClose.Attributes.Add("onclick", "return CloseWindow();");
            btnFinish.Attributes.Add("onclick", "return CloseWindow();");
            btnApprove.Attributes.Add("onclick", "return confirm('Are you sure you want to APPROVE this request?');");
            btnDeny.Attributes.Add("onclick", "return confirm('Are you sure you want to DENY this request?');");
        }
        protected void btnApprove_Click(Object Sender, EventArgs e)
        {
            int intVacation = Int32.Parse(lblVacation.Text);
            oVacation.Update(intVacation, 1);
            int intUser = Int32.Parse(oVacation.Get(intVacation, "userid"));
            oFunction.SendEmail("ClearView Out of Office Request", oUser.GetName(intUser), "", strEMailIdsBCC, "ClearView Out of Office Request", "<p><b>The following out of office request has been APPROVED by your MANAGER (" + oUser.GetFullName(intProfile) + ")</b><p><p>" + oVacation.GetBody(intVacation, intEnvironment) + "</p>", true, false);
            Response.Redirect(oPage.GetFullLink(intPage) + "?action=finish");
        }
        protected void btnDeny_Click(Object Sender, EventArgs e)
        {
            int intVacation = Int32.Parse(lblVacation.Text);
            oVacation.Update(intVacation, -1);
            int intUser = Int32.Parse(oVacation.Get(intVacation, "userid"));
            oFunction.SendEmail("ClearView Out of Office Request", oUser.GetName(intUser), "", strEMailIdsBCC, "ClearView Out of Office Request", "<p><b>The following out of office request has been DENIED by your MANAGER (" + oUser.GetFullName(intProfile) + ")</b><p><p>" + oVacation.GetBody(intVacation, intEnvironment) + "</p>", true, false);
            Response.Redirect(oPage.GetFullLink(intPage) + "?action=finish");
        }
    }
}