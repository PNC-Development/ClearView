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
    public partial class resource_request : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected string strPCWM = ConfigurationManager.AppSettings["PC_WM"];
        protected string strTaskWM = ConfigurationManager.AppSettings["TASK_WM"];
        protected int intProfile;
        protected ResourceRequest oResourceRequest;
        protected RequestItems oRequestItem;
        protected Applications oApplication;
        protected Requests oRequest;
        protected Projects oProject;
        protected ProjectNumber oProjectNumber;
        protected Services oService;
        protected Users oUser;
        protected Delegates oDelegate;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            oProjectNumber = new ProjectNumber(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            if (Request.QueryString["action"] != null && Request.QueryString["action"] != "")
                panFinish.Visible = true;
            else
            {
                panForm.Visible = true;
                if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
                {
                    int intResourceWorkflow = Int32.Parse(Request.QueryString["rrid"]);
                    int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                    DataSet ds = oResourceRequest.Get(intResourceParent);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                        int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                        int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                        int intApp = oRequestItem.GetItemApplication(intItem);
                        //if (intProfile == intUser || oService.IsManager(intService, intProfile) || oDelegate.Get(intUser, intProfile) > 0 || (oApplication.IsManager(intApp, intProfile) && oApplication.Get(intApp, "disable_manager") != "1") || (oUser.IsManager(intUser, intProfile, true) && oApplication.Get(intApp, "disable_manager") != "1"))
                        //{
                        int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                        hdnRequestID.Value = intRequest.ToString();
                        int intProject = oRequest.GetProjectNumber(intRequest);
                        string strPath = "";
                        if (intItem == 0)
                            strPath = strPCWM;
                        else if (intItem == -1)
                            strPath = strTaskWM;
                        else
                        {
                            if (oApplication.Get(intApp, "tpm") != "1" && oProject.Get(intProject, "number") == "")
                                oProject.Update(intProject, oProjectNumber.New());
                            strPath = oService.Get(intService, "wm_path");
                        }
                        if (strPath != "")
                            oForm.Controls.Add((Control)LoadControl(strPath));
                        string strTitle = "Task";
                        if (intProject > 0)
                            strTitle = oProject.Get(intProject, "name");
                        Master.Page.Title = strTitle + " | " + oService.GetName(intService);
                        //}
                        //else
                        //    panDenied.Visible = true;
                    }
                    else
                        panDenied.Visible = true;
                }
            }
            btnFinish.Attributes.Add("onclick", "return CloseWindow();");
            btnClose.Attributes.Add("onclick", "return CloseWindow();");
        }
    }
}
