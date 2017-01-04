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
//using PAObjectsLib;
using Vim25Api;
using System.Reflection;
using Tamir.SharpSsh;
using System.Net.NetworkInformation;

namespace NCC.ClearView.Presentation.Web
{
    public partial class test3 : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intCount = 0;
        private string strResult;
        private string strError;
        private Mnemonic oMnemonic;
        private Servers oServer;
        private Forecast oForecast;
        private Requests oRequest;
        private Projects oProject;
        private Organizations oOrganization;
        private Users oUser;
        private AD oAD;
        private Variables oVar;
        private Functions oFunction;
        private AccountRequest oAccountRequest;
        private OnDemand oOnDemand;
        private string strMnemonicCode;
        private string strMnemonicName;
        private string strOrganizationCode;
        private int intServer = 0;
        private int intAnswer = 0;
        private int intRequest = 0;
        private int intProject = 0;
        private int intOrganization = 0;
        private char[] strSplit = { ';' };
        private string strIP;
        private string strDomain;
        private string strScripts;
        private string strName;

        protected void Page_Load(object sender, EventArgs e)
        {
            Asset oAsset = new Asset(0, dsnAsset, dsn);
            // 7,1,1675,713,5778,2,47
            DataSet dsAssets = oAsset.GetServerOrBladeAvailable(7, 1, 1675, 713, 5778, 2, 47);
            ServiceEditor oServiceEditor = new ServiceEditor(0, dsnServiceEditor);
            DataSet dsAssetsManual = oServiceEditor.APGetSerials();
            for (int ii = 0; ii < dsAssets.Tables[0].Rows.Count; ii++)
            {
                DataRow drAsset = dsAssets.Tables[0].Rows[ii];
                string strSerial = drAsset["serial"].ToString();
                bool boolIsManual = false;
                DataView dvAssetsManual = dsAssetsManual.Tables[0].DefaultView;
                dvAssetsManual.RowFilter = "serial = '" + strSerial + "'";
                if (dvAssetsManual.Count > 0)
                {
                    Response.Write("delete asset " + strSerial + "<br/>");
                    dsAssets.Tables[0].Rows.Remove(drAsset);
                    ii--;
                }
            }


            //strResult = "";
            //strError = "";
            //oMnemonic = new Mnemonic(0, dsn);
            //oServer = new Servers(0, dsn);
            //oForecast = new Forecast(0, dsn);
            //oRequest = new Requests(0, dsn);
            //oProject = new Projects(0, dsn);
            //oOrganization = new Organizations(0, dsn);
            //oUser = new Users(0, dsn);
            //intEnvironment = 999;
            //oAD = new AD(0, dsn, intEnvironment);
            //oVar = new Variables(intEnvironment);
            //oFunction = new Functions(0, dsn, intEnvironment);
            //oAccountRequest = new AccountRequest(0, dsn);
            //oOnDemand = new OnDemand(0, dsn);
            //strMnemonicCode = "XQM";
            //strMnemonicName = "TESTING CLEARVIEW";
            //strOrganizationCode = "CVW";
            //intServer = 11424;
            //intAnswer = Int32.Parse(oServer.Get(intServer, "answerid"));
            //intRequest = oForecast.GetRequestID(intAnswer, true);
            //intProject = oRequest.GetProjectNumber(intRequest);
            //intOrganization = Int32.Parse(oProject.Get(intProject, "organization"));
            //strIP = "10.33.253.246";
            //strDomain = "PNCNT";
            //strScripts = Request.PhysicalApplicationPath + "scripts\\";
            ////strName = "WDRDP300A";
            //strName = "WCXQM203D";

            //Response.Write(oFunction.PingName(strName));
        }

        protected void btn1_Click(object sender, EventArgs e)
        {
            // Configure Active Directory Accounts
            DataSet dsAccounts = oServer.GetAccounts(intServer);
            //string strNameGG = "GG_URA_" + strOrganizationCode + "_" + strMnemonicCode + "_";
            string strNameGG = "GSGu_" + strMnemonicCode + "_";
            foreach (DataRow drAccount in dsAccounts.Tables[0].Rows)
            {
                string strDomainGroups = "";
                string[] strDomainGroupArray = drAccount["domaingroups"].ToString().Split(strSplit);
                for (int ii = 0; ii < strDomainGroupArray.Length; ii++)
                {
                    // Add Prefix and Remove Suffix ("_0" / "_1") for remote desktop
                    if (strDomainGroupArray[ii].Trim() != "")
                    {
                        string strDomainGroup = strDomainGroupArray[ii].Trim();
                        if (strDomainGroup.Contains("_") == true)
                            strDomainGroup = strDomainGroup.Substring(0, strDomainGroup.IndexOf("_"));
                        if (strDomainGroups != "")
                            strDomainGroups += ";";
                        strDomainGroups += strNameGG + strDomainGroup;
                    }
                }
                oAccountRequest.Add(intRequest, 0, 0, drAccount["xid"].ToString(), Int32.Parse(drAccount["domain"].ToString()), strDomainGroups, drAccount["localgroups"].ToString(), Int32.Parse(drAccount["email"].ToString()), 0);
                strResult = "AD Groups Have Been Queued";
            }

            Response.Write("<p><b>RESULT...</b></p><p>" + strResult + "</p>");
            Response.Write("<p><b>ERROR...</b></p><p>" + strError + "</p>");
        }

        protected void btn2_Click(object sender, EventArgs e)
        {
            // Create ADM Active Directory Group 
            Variables oVariable = new Variables(intEnvironment);
            if (intOrganization > 0)
            {
                DataSet dsOrganization = oOrganization.Get(intOrganization);
                if (dsOrganization.Tables[0].Rows.Count > 0)
                {
                    if (strOrganizationCode != "")
                    {
                        //int intCustodian = Int32.Parse(dsOrganization.Tables[0].Rows[0]["userid"].ToString());
                        int intCustodian = 0;
                        Int32.TryParse(oForecast.GetAnswer(intAnswer, "appowner"), out intCustodian);
                        if (intCustodian > 0)
                        {
                            // BEGIN PROCESSING
                            // Create DLG OUg_<mnemonic> groups
                            string strMnemonicGroupDLG = "OUg_" + strMnemonicCode;
                            if (oAD.SearchOU(strMnemonicGroupDLG) == null)
                            {
                                string strResultMnemonic = oAD.CreateOU(strMnemonicGroupDLG, "", "OU=OUc_Resources,");
                                if (strResultMnemonic == "")
                                    strResult += "The OU " + strMnemonicGroupDLG + " was successfully created in " + oVar.Name() + "<br/>";
                                else
                                    strError = "There was a problem creating the OU ~ " + strMnemonicGroupDLG + " in " + oVar.Name();
                            }
                            else
                                strResult += "The OU " + strMnemonicGroupDLG + " already exists in " + oVar.Name() + "<br/>";

                            if (strError == "")
                            {
                                // Assume the GG OU exists and continue creating
                                string strGroupDescription = "Custodian: " + oUser.GetFullName(intCustodian) + " (" + oUser.GetName(intCustodian) + "), LOB-" + strOrganizationCode + ", MIS-" + strMnemonicCode + " " + strMnemonicName;
                                string[] strGroups = oVariable.PNC_AD_Groups();

                                for (int ii = 0; ii < strGroups.Length && strError == ""; ii++)
                                {
                                    //string strNameDLG = "DLG_URA_" + strOrganizationCode + "_" + strMnemonicCode + "_" + strGroups[ii];
                                    string strNameDLG = "GSLfsaSP_" + strMnemonicCode + "_" + strGroups[ii];
                                    //string strNameGG = "GG_URA_" + strOrganizationCode + "_" + strMnemonicCode + "_" + strGroups[ii];
                                    string strNameGG = "GSGu_" + strMnemonicCode + "_" + strGroups[ii];

                                    if (strError == "")
                                    {
                                        if (oAD.Search(strNameDLG, false) == null)
                                        {
                                            string strResultDLG = oAD.CreateGroup(strNameDLG, strGroupDescription, "", "OU=" + strMnemonicGroupDLG + ",OU=OUc_Resources,", "DLG", "S");
                                            if (strResultDLG == "")
                                                strResult += "The group " + strNameDLG + " was successfully created in " + oVar.Name() + "<br/>";
                                            else
                                            {
                                                strError = "There was a problem creating the group ~ " + strNameDLG + " in " + oVar.Name();
                                                break;
                                            }
                                        }
                                        else
                                            strResult += "The group " + strNameDLG + " already exists in " + oVar.Name() + "<br/>";
                                    }

                                    if (strError == "")
                                    {
                                        if (oAD.Search(strNameGG, false) == null)
                                        {
                                            string strResultGG = oAD.CreateGroup(strNameGG, strGroupDescription, "", "OU=OUg_Applications,OU=OUc_AccessGroups,", "GG", "S");
                                            if (strResultGG == "")
                                                strResult += "The group " + strNameGG + " was successfully created in " + oVar.Name() + "<br/>";
                                            else
                                            {
                                                strError = "There was a problem creating the group ~ " + strNameGG + " in " + oVar.Name();
                                                break;
                                            }
                                        }
                                        else
                                            strResult += "The group " + strNameGG + " already exists in " + oVar.Name() + "<br/>";
                                    }

                                    if (strError == "")
                                    {
                                        // Add the GG to the DLG
                                        string strResultJoin = oAD.JoinGroup(strNameGG, strNameDLG, 0);
                                        if (strResultJoin == "")
                                            strResult += "The global group " + strNameGG + " was successfully added to the domain local group " + strNameDLG + " in " + oVar.Name() + "<br/>";
                                        else
                                        {
                                            strError = "There was a problem adding the global group ~ " + strNameGG + " to the domain local group " + strNameDLG + " in " + oVar.Name();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            strError = "The organization is not configured for Active Directory Group naming (CUSTODIAN / USERID is blank) ~ (" + dsOrganization.Tables[0].Rows[0]["name"].ToString() + ")";
                        }
                    }
                    else
                    {
                        strError = "The organization is not configured for Active Directory Group naming (CODE is blank) ~ (" + dsOrganization.Tables[0].Rows[0]["name"].ToString() + ")";
                    }
                }
                else
                {
                    strError = "Invalid Organization";
                }
            }
            else
            {
                strError = "There is no organization specified for this project";
            }
            int intStep = 6;
            //oOnDemand.UpdateStepDoneServer(intServer, intStep, strResult, 0, false, true);

            Response.Write("<p><b>RESULT...</b></p><p>" + strResult + "</p>");
            Response.Write("<p><b>ERROR...</b></p><p>" + strError + "</p>");
        }


        protected void btn3_Click(object sender, EventArgs e)
        {
            oAccountRequest.Process(intRequest, 0, 0, strName, "@DD4cv1ew", intEnvironment, 1);
            DataSet dsNotify = oRequest.GetResult(intRequest);
            string strNotify = "";
            foreach (DataRow drResult in dsNotify.Tables[0].Rows)
                strNotify += drResult["result"].ToString();
            if (strNotify != "")
            {
                strResult = "Active Directory accounts and permissions were created";
            }
            else
                strResult = "Active Directory accounts and permissions were skipped";
            int intStep = 17;
            //oOnDemand.UpdateStepDoneServer(intServer, intStep, strResult, 0, false, true);
            Response.Write("<p><b>RESULT...</b></p><p>" + strResult + "</p>");
            Response.Write("<p><b>ERROR...</b></p><p>" + strError + "</p>");
        }
        protected void btn4_Click(object sender, EventArgs e)
        {
            // Add Local Admin Groups 
            // 1st part - create VBS file to copy to server
            string strScriptPath = strScripts + "ADSTEST_";
            string strFile = strScripts + "ADSTEST.vbs";
            StreamWriter oWriter1 = new StreamWriter(strFile);
            oWriter1.WriteLine("On Error Resume Next");

            //string strNameDLG = "DLG_URA_" + strOrganizationCode + "_" + strMnemonicCode + "_";
            string strNameDLG = "GSLfsaSP_" + strMnemonicCode + "_";

            string strRemoteDesktopGroups = "";
            DataSet dsAccounts = oServer.GetAccounts(intServer);
            foreach (DataRow drAccount in dsAccounts.Tables[0].Rows)
            {
                string[] strDomainGroupArray = drAccount["domaingroups"].ToString().Split(strSplit);
                for (int ii = 0; ii < strDomainGroupArray.Length; ii++)
                {
                    // Get all the groups where "_1" was suffixed (for remote desktop)
                    if (strDomainGroupArray[ii].Trim() != "")
                    {
                        string strDomainGroup = strDomainGroupArray[ii].Trim();
                        if (strDomainGroup.Contains("_1") == true)
                        {
                            strDomainGroup = strDomainGroup.Substring(0, strDomainGroup.IndexOf("_"));
                            bool boolRemoteDesktopExists = false;
                            string[] strRemoteDesktopGroupArray = strRemoteDesktopGroups.Split(strSplit);
                            for (int jj = 0; jj < strRemoteDesktopGroupArray.Length; jj++)
                            {
                                if (strRemoteDesktopGroupArray[jj].Trim() != "" && strRemoteDesktopGroupArray[jj].Trim() == strDomainGroup)
                                {
                                    boolRemoteDesktopExists = true;
                                    break;
                                }
                            }
                            if (boolRemoteDesktopExists == false)
                            {
                                if (strRemoteDesktopGroups != "")
                                    strRemoteDesktopGroups += ";";
                                strRemoteDesktopGroups += strDomainGroup;
                            }
                        }
                    }
                }
            }

            oWriter1.WriteLine("Set objGroup1 = GetObject(\"WinNT://localhost/Remote Desktop Users\")");
            string[] strRemoteDesktopGroup = strRemoteDesktopGroups.Split(strSplit);
            for (int ii = 0; ii < strRemoteDesktopGroup.Length; ii++)
            {
                // Add all remote desktop groups
                if (strRemoteDesktopGroup[ii].Trim() != "")
                    oWriter1.WriteLine("objGroup1.Add (\"WinNT://" + strDomain + "/" + strNameDLG + strRemoteDesktopGroup[ii].Trim() + "\")");
            }

            oWriter1.WriteLine("Set objGroup2 = GetObject(\"WinNT://localhost/AppSupport\")");
            oWriter1.WriteLine("objGroup2.Add (\"WinNT://" + strDomain + "/" + strNameDLG + "AppSupport\")");

            oWriter1.WriteLine("Set objGroup3 = GetObject(\"WinNT://localhost/AppUsers\")");
            oWriter1.WriteLine("objGroup3.Add (\"WinNT://" + strDomain + "/" + strNameDLG + "AppUsers\")");

            oWriter1.WriteLine("Set objGroup4 = GetObject(\"WinNT://localhost/Developers\")");
            oWriter1.WriteLine("objGroup4.Add (\"WinNT://" + strDomain + "/" + strNameDLG + "Developers\")");

            oWriter1.WriteLine("Set objGroup5 = GetObject(\"WinNT://localhost/Promoters\")");
            oWriter1.WriteLine("objGroup5.Add (\"WinNT://" + strDomain + "/" + strNameDLG + "Promoters\")");

            oWriter1.WriteLine("Set objGroup6 = GetObject(\"WinNT://localhost/Administrators\")");
            oWriter1.WriteLine("objGroup6.Add (\"WinNT://" + strDomain + "/" + strNameDLG + "AuthProbMgmt\")");
            oWriter1.WriteLine("objGroup6.Add (\"WinNT://" + strDomain + "/" + strNameDLG + "AuthPromoters\")");

            oWriter1.WriteLine("Set objGroup1 = Nothing");
            oWriter1.WriteLine("Set objGroup2 = Nothing");
            oWriter1.WriteLine("Set objGroup3 = Nothing");
            oWriter1.WriteLine("Set objGroup4 = Nothing");
            oWriter1.WriteLine("Set objGroup5 = Nothing");
            oWriter1.WriteLine("Set objGroup6 = Nothing");

            oWriter1.WriteLine("On Error GoTo 0");
            oWriter1.Flush();
            oWriter1.Close();
            int intReturn = oFunction.ExecuteVBScript(intServer, false, true, "ACCOUNTS", strName, "TESTSERIAL", strIP, strFile, strScriptPath, "ADSTEST", "%windir%\\system32\\wscript.exe", "OPTIONS\\CV_ADSTEST", "VBS", "", strScripts, oVar.Domain() + "\\" + oVar.ADUser(), oVar.ADPassword(), 5, true, false, 0, false);

            Response.Write("<p><b>RESULT...</b></p><p>" + intReturn.ToString() + "</p>");
        }
    }
}
