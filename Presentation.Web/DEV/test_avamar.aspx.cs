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
using System.Net;
using System.IO;
using PAObjectsLib;
using Vim25Api;
using System.Reflection;
using NCC.ClearView.Application.Core.ClearViewWS;
using Tamir.SharpSsh;
using Tamir.Streams;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class test_avamar : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        private int EnvironmentID = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        private string strResults { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnWebService_Click(object sender, EventArgs e)
        {
            string host = "yb19-avr01-01.test.pncbank.com";
            ClearViewWebServices ws = new ClearViewWebServices();
            ws.Url = "http://localhost:64919/ClearViewWebServices.asmx";

            string result = ws.GetAvamarGrid(host);
            if (!string.IsNullOrEmpty(result) && result.TrimStart().StartsWith("<"))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                XmlNodeList xnList = doc.SelectNodes("/CLIOutput/Data/Row");
                foreach (XmlNode xn in xnList)
                {
                    //if (xn["Attribute"].InnerText == "Server utilization")
                    //{
                    //    Response.Write(xn["Value"].InnerText + "<br/>");
                    //    break;
                    //}
                    Response.Write(xn["Attribute"].InnerText + " = " + xn["Value"].InnerText + "<br/>");
                }
            }
            else
                Response.Write(result + "<br/>");
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            AvamarRegistration oAvamarAutomation = new AvamarRegistration(0, dsn);
            string strScripts = Server.MapPath("/scripts/");
            oAvamarAutomation.Registrations(EnvironmentID, strScripts, dsnAsset, dsnServiceEditor, dsnIP, intViewPage, intAssignPage);
        }

        protected void btnDecom_Click(object sender, EventArgs e)
        {
            Variables oVariable = new Variables((int)CurrentEnvironment.PNCNT_DEV);
            string grid = "yb19-avr01-01.test.pncbank.com";
            string domain = "/BRII";
            string client = "WDCLV015A.pncbank.com";
            string strError = "";


            Avamar oAvamar = new Avamar(0, dsn);
            AvamarRegistration oAvamarRegistration = new AvamarRegistration(0, dsn);
            ClearViewWebServices oWebService = new ClearViewWebServices();
            System.Net.NetworkCredential oCredentialsDNS = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
            oWebService.Credentials = oCredentialsDNS;
            oWebService.Url = oVariable.WebServiceURL();

            // First, query for groups.
            AvamarReturnType groups = oAvamarRegistration.API(oWebService.GetAvamarClient(grid, domain, client));
            if (groups.Error == false)
            {
                List<string> members = new List<string>();
                foreach (XmlNode node in groups.Nodes)
                {
                    if (node["Attribute"].InnerText == "Member of Group")
                        members.Add(node["Value"].InnerText);
                }

                if (members.Count > 0)
                {
                    // Second, add /Decom group (so there will always be at least one group)
                    AvamarReturnType decom = oAvamarRegistration.API(oWebService.AddAvamarGroup(grid, domain, client, oAvamar.DecomGroup));
                    if (decom.Error == false)
                    {
                        oAvamar.DeleteDecom(client);
                        foreach (string member in members)
                        {
                            if (String.IsNullOrEmpty(strError) == false)
                                break;
                            // Third, remove groups (one at a time)
                            AvamarReturnType remove = oAvamarRegistration.API(oWebService.DeleteAvamarGroup(grid, domain, client, member));
                            if (remove.Error == false)
                            {
                                // Fourth, save groups.
                                oAvamar.AddDecom(client, grid, domain, member);
                            }
                            else
                                strError = remove.Message;
                        }
                    }
                    else
                        strError = decom.Message;
                }
            }
            else
                strError = groups.Message;
        }

        protected void btnRecom_Click(object sender, EventArgs e)
        {
            Variables oVariable = new Variables((int)CurrentEnvironment.PNCNT_DEV);
            string client = "WDCLV015A.pncbank.com";
            string strError = "";

            Avamar oAvamar = new Avamar(0, dsn);
            AvamarRegistration oAvamarRegistration = new AvamarRegistration(0, dsn);
            ClearViewWebServices oWebService = new ClearViewWebServices();
            System.Net.NetworkCredential oCredentialsDNS = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
            oWebService.Credentials = oCredentialsDNS;
            oWebService.Url = oVariable.WebServiceURL();

            // First, query for groups.
            DataSet dsGroups = oAvamar.GetDecoms(client);
            if (dsGroups.Tables[0].Rows.Count > 0)
            {
                string grid = dsGroups.Tables[0].Rows[0]["grid"].ToString();
                string domain = dsGroups.Tables[0].Rows[0]["domain"].ToString();
                // Second, add the groups.
                foreach (DataRow drGroup in dsGroups.Tables[0].Rows)
                {
                    if (String.IsNullOrEmpty(strError) == false)
                        break;
                    AvamarReturnType restore = oAvamarRegistration.API(oWebService.AddAvamarGroup(grid, domain, client, drGroup["group"].ToString()));
                    if (restore.Error == false)
                    {
                    }
                    else
                        strError = restore.Message;
                }
                // Third, remove the /Decom group
                AvamarReturnType decom = oAvamarRegistration.API(oWebService.DeleteAvamarGroup(grid, domain, client, oAvamar.DecomGroup));
                if (decom.Error == false)
                {
                    // Fourth, recommission the saved decom groups.
                    oAvamar.UpdateDecom(client);
                }
                else
                    strError = decom.Message;
            }
        }

        protected void btnCSV_Click(object sender, EventArgs e)
        {
            Functions oFunction = new Functions(0, dsn, (int)CurrentEnvironment.PNCNT_DEV);
            string strName = "wcclv309a";
            DataSet dsKey = oFunction.GetSetupValuesByKey("AVAMAR_REGISTRATIONS");
            if (dsKey.Tables[0].Rows.Count > 0)
            {
                string registrations = dsKey.Tables[0].Rows[0]["Value"].ToString();
                StreamReader theReader = new StreamReader(registrations);
                string theContents = theReader.ReadToEnd();
                string[] theLines = theContents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string theLine in theLines)
                {
                    if (theLine.Contains(strName))
                    {
                        string[] theFields = theLine.Split(new char[] { ',' }, StringSplitOptions.None);
                        if (theFields.Length == 3)
                        {
                            string grid = theFields[0];
                            string domain = theFields[1];
                            string client = theFields[2];
                            if (client.EndsWith("\r"))
                                client = client.Replace("\r", "");
                        }
                        break;
                    }
                }
            }


        }

        protected void btnActivated_Click(object sender, EventArgs e)
        {
            string grid = "yb19-avr01-01.test.pncbank.com";
            ClearViewWebServices oWebService = new ClearViewWebServices();
            oWebService.Url = "http://localhost:64919/ClearViewWebServices.asmx";

            AvamarRegistration oAvamarRegistration = new AvamarRegistration(0, dsn);
            AvamarReturnType activated = oAvamarRegistration.API(oWebService.GetAvamarClient(grid, "/BRII", "WCIRU301A.pncbank.com"));
            if (activated.Error == false)
            {
                foreach (XmlNode node in activated.Nodes)
                {
                    if (node["Attribute"].InnerText == "Activated")
                        Response.Write(node["Value"].InnerText + "<br/>");
                }
            }
            else
                Response.Write(activated.Message + "<br/>");
        }



    }
}
