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
using System.DirectoryServices;
using NCC.ClearView.Application.Core;
using System.Data.OleDb;
using System.Net.NetworkInformation;
using NCC.ClearView.Application.Core.ClearViewWS;
using System.Threading;

namespace NCC.ClearView.Presentation.Web.DEV
{
    public partial class decoms : System.Web.UI.Page
    {
        private int intEnvironment = 999;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private Variables oVariable;
        private Functions oFunction;

        protected void Page_Load(object sender, EventArgs e)
        {
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(0, dsn, intEnvironment);
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\temp\\destroys.xls;Extended Properties=Excel 8.0;";

            System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
            ClearViewWebServices oServiceNow = new ClearViewWebServices();
            oServiceNow.Timeout = Timeout.Infinite;
            oServiceNow.Credentials = oCredentials;
            oServiceNow.Url = oVariable.WebServiceURL();

            System.Net.NetworkCredential oCredentialsSN = new System.Net.NetworkCredential(oVariable.ServiceNowUsername(), oVariable.ServiceNowPassword());
            string url = oVariable.ServiceNowHost();
            string user = oVariable.ServiceNowUsername();
            string pass = oVariable.ServiceNowPassword();

            OleDbDataAdapter myCommand1 = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
            DataSet ds1 = new DataSet();
            myCommand1.Fill(ds1, "ExcelInfo");
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                if (dr[0].ToString().Trim() == "")
                    break;
                string name = dr[0].ToString().Trim();
                Response.Write(name + "...");
                if (name.Contains("-DR") == false)
                {
                    // Ping server name
                    Ping ping = new Ping();
                    string response = "";
                    try
                    {
                        PingReply reply = ping.Send(name);
                        response = reply.Status.ToString().ToUpper();
                    }
                    catch { }
                    Response.Write((response == "SUCCESS" ? "** ONLINE **" : "offline") + "...");

                    // Check Service Now
                    string result = oServiceNow.GetServiceNowServer(url, user, pass, name);
                    if (String.IsNullOrEmpty(result) == false)
                    {
                        int _state = result.IndexOf("<u_desired_operational_state");
                        if (_state > -1)
                        {
                            string state = result.Substring(_state);
                            state = state.Substring(state.IndexOf(">") + 1);
                            state = state.Substring(0, state.IndexOf("<"));
                            if (state == "-2")
                                Response.Write("decommissioned" + "...");
                            else
                                Response.Write("** " + state + " **...");
                        }
                        else
                            Response.Write("** missing desired operational state **...");

                        int _status = result.IndexOf("<install_status");
                        if (_status > -1)
                        {
                            string status = result.Substring(_status);
                            status = status.Substring(status.IndexOf(">") + 1);
                            status = status.Substring(0, status.IndexOf("<"));
                            if (status == "7")
                                Response.Write("retired" + "...");
                            else
                                Response.Write("** " + status + " **...");
                        }
                        else
                            Response.Write("** missing install status **...");
                    }
                    else
                        Response.Write("** NOT THERE **" + "...");
                }
                else
                    Response.Write("dr");

                Response.Write("<br/>");
            }
        }

    }
}
