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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class cp_restart : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected string strDone = "";
        protected int intProfile = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            RequestItems oRequestItem = new RequestItems(intProfile, dsn);
            RequestFields oRequestField = new RequestFields(intProfile, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
            Services oService = new Services(intProfile, dsn);
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            string strStatus = oServiceRequest.Get(intRequest, "checkout");
            DataSet dsItems = oRequestItem.GetForms(intRequest);
            int intItem = 0;
            int intService = 0;
            int intNumber = 0;
            if (dsItems.Tables[0].Rows.Count > 0)
            {
                bool boolBreak = false;
                foreach (DataRow drItem in dsItems.Tables[0].Rows)
                {
                    if (boolBreak == true)
                        break;
                    if (drItem["done"].ToString() == "0")
                    {
                        intItem = int.Parse(drItem["itemid"].ToString());
                        intService = Int32.Parse(drItem["serviceid"].ToString());
                        intNumber = Int32.Parse(drItem["number"].ToString());
                        boolBreak = true;
                    }
                    if (intItem > 0 && (strStatus == "1" || strStatus == "2"))
                    {
                        bool boolSuccess = true;
                        StringBuilder sbResult = new StringBuilder(oService.GetName(intService) + " Completed");
                        string strError = oService.GetName(intService) + " Error";
                        
                        // ********* BEGIN PROCESSING **************
                        Restart oRestart = new Restart(0, dsn);
                        Scripts oScript = new Scripts(0, dsn);
                        DataSet ds = oRestart.GetRequests(intRequest);
                        sbResult = new StringBuilder();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string strName = dr["name"].ToString();
                            string strSeconds = "30";
                            oRestart.DeleteRequest(Int32.Parse(dr["id"].ToString()));
                            StringBuilder sbScript = new StringBuilder();
                            sbScript.Append("Dim objShell\r\n");
                            sbScript.Append("Set objShell = CreateObject(\"WScript.Shell\")\r\n");
                            sbScript.Append("objShell.Run(\"cmd.exe /c shutdown -r -m \\\\");
                            sbScript.Append(strName);
                            sbScript.Append(" -t ");
                            sbScript.Append(strSeconds);
                            sbScript.Append(" -f\")\r\n");
                            sbScript.Append("Set objShell = Nothing\r\n");

                            oScript.Add(sbScript.ToString(), Int32.Parse(dr["environment"].ToString()));

                            sbResult.Append(strName);
                            sbResult.Append(" was successfully sent a restart request");
                        }

                        // ******** END PROCESSING **************
                        if (oService.Get(intService, "automate") == "1" && boolSuccess == true)
                        {
                            strDone += "<table border=\"0\"><tr><td valign=\"top\"><img src=\"/images/bigCheck.gif\" border=\"0\" align=\"absmiddle\"/></td><td valign=\"top\" class=\"biggerbold\">" + sbResult.ToString() + "</td></tr></table>";
                        }
                        else
                        {
                            if (boolSuccess == false)
                            {
                                strDone += "<table border=\"0\"><tr><td valign=\"top\"><img src=\"/images/bigError.gif\" border=\"0\" align=\"absmiddle\"/></td><td valign=\"top\" class=\"biggerbold\">" + strError + "</td></tr></table>";
                            }
                            else
                            {
                                strDone += "<table border=\"0\"><tr><td valign=\"top\"><img src=\"/images/bigCheck.gif\" border=\"0\" align=\"absmiddle\"/></td><td valign=\"top\" class=\"biggerbold\">" + oService.GetName(intService) + " Submitted</td></tr></table>";
                            }
                        }

                        oRequestItem.UpdateFormDone(intRequest, intItem, intNumber, 1);
                    }
                }
            }
        }
    }
}