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
using Vim25Api;

namespace NCC.ClearView.Presentation.Web
{
    public partial class ondemand_vmware_preview : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            bool boolError = true;
            if (Request.QueryString["name"] != null && Request.QueryString["name"] != "" & Request.QueryString["token"] != null && Request.QueryString["token"] != "")
            {
                Tokens oToken = new Tokens(0, dsn);
                string strName = Request.QueryString["name"];
                string strToken = Request.QueryString["token"];
                if (oToken.Get(strName, strToken) == true)
                {
                    // Delete Token
                    oToken.Update(strName, strToken);

                    // Render Preview
                    VMWare oVMWare = new VMWare(0, dsn);
                    //strName = "WDBIX103A";
                    string strConnect = oVMWare.Connect(strName);
                    VimService _service = oVMWare.GetService();
                    if (strConnect == "")
                    {
                        ManagedObjectReference vmRef = oVMWare.GetVM(strName);
                        if (vmRef.Value != null)
                        {
                            try
                            {
                                VirtualMachineMksTicket oTicket = _service.AcquireMksTicket(vmRef);
                                string strVMHost = oTicket.host;
                                string strVMPort = oTicket.port.ToString();
                                string strVMPath = oTicket.cfgFile;
                                string strVMTicket = oTicket.ticket;
                                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "start_count", "<script type=\"text/javascript\">window.onload = new Function(\"connect('" + strVMHost + "','" + strVMPath + "','" + strVMTicket + "','" + strVMPort + "');\");<" + "/" + "script>");
                                boolError = false;
                            }
                            catch { }
                        }
                    }
                    if (_service != null)
                    {
                        ServiceContent _sic = oVMWare.GetSic();
                        _service.Abort();
                        if (_service.Container != null)
                            _service.Container.Dispose();
                        try
                        {
                            _service.Logout(_sic.sessionManager);
                        }
                        catch { }
                        _service.Dispose();
                        _service = null;
                        _sic = null;
                    }
                }
            }
            if (boolError == true)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "start_count", "<script type=\"text/javascript\">window.onload = new Function(\"window.parent.VMWareNoPreview();\");<" + "/" + "script>");
        }
    }
}
