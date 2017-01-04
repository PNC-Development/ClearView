using System;
using System.Net;
using System.Data;
using System.Web;

namespace NCC.ClearView.Application.Core
{
    public enum CurrentEnvironment : int
    {
        CORPDMN = 4,
        PNCENG = 200,
        PNCADSTEST = 900,
        PNCNT_LOCALHOST = 901,
        PNCRND = 802,
        PNCUAT = 803,
        PNCQA = 804,
        PNCNT_DEV = 902,
        PNCNT_TEST = 903,
        PNCNT_QA = 904,
        PNCNT_PROD = 999
    }
	public class Variables
	{
		private int intEnvironment;
		public Variables(int _environment)
		{
            intEnvironment = _environment;
		}
        public string NotifyWorkstationProd()
        {
            if (intEnvironment == 1 || intEnvironment == 2 || intEnvironment == 3 || intEnvironment == 11 || intEnvironment == 12 || intEnvironment == 21 || intEnvironment == 22)
                return "esxh33t;";
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "GM5587W;";
            return "";
        }
        public string Name()
        {
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "CORPDMN";
            if (intEnvironment == (int)CurrentEnvironment.PNCENG)
                return "PNCENG";
            if (intEnvironment == (int)CurrentEnvironment.PNCADSTEST)
                return "PNCADSTEST";
            if (intEnvironment == (int)CurrentEnvironment.PNCRND)
                return "PNCRND";
            if (intEnvironment == (int)CurrentEnvironment.PNCUAT)
                return "PNCUAT";
            if (intEnvironment == (int)CurrentEnvironment.PNCQA)
                return "PNCQA";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST || 
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                return "PNCNT";
            return "";
        }
        public string primaryDC(string _dsn)
        {
            string DC = primaryDCName(_dsn);
            if (String.IsNullOrEmpty(DC))
                return "LDAP://" + LDAP();
            else
                return "LDAP://" + DC + "/" + LDAP();
        }
        public string primaryDCName(string _dsn)
        {
            string strReturn = "";
            Functions oFunction = new Functions(0, _dsn, intEnvironment);
            string key = "PNCBANK_DC";
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN)
                key = "CORPDMN_DC";
            else if (intEnvironment == (int)CurrentEnvironment.PNCRND)
                key = "PNCRND_DC";
            else if (intEnvironment == (int)CurrentEnvironment.PNCUAT)
                key = "PNCUAT_DC";
            else if (intEnvironment == (int)CurrentEnvironment.PNCQA)
                key = "PNCQA_DC";
            DataSet dsKey = oFunction.GetSetupValuesByKey(key);
            if (dsKey.Tables[0].Rows.Count > 0)
                strReturn = dsKey.Tables[0].Rows[0]["Value"].ToString();
            return strReturn;
        }
        public string ADUser() 
		{
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN)
				return "SEPS13R";
            if (intEnvironment == (int)CurrentEnvironment.PNCRND ||
                intEnvironment == (int)CurrentEnvironment.PNCUAT ||
                intEnvironment == (int)CurrentEnvironment.PNCQA ||
                intEnvironment == (int)CurrentEnvironment.PNCENG ||
                intEnvironment == (int)CurrentEnvironment.PNCADSTEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                return "xacview";
            return "";
		}
        public string URL()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST)
                return "http://localhost:4412";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_DEV)
                return "http://localhost:4412";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_TEST)
                return "https://clearview-test.pncbank.com";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_QA)
                return "https://clearview-qa.pncbank.com";
            
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
            {
                string strPath = "";
                try
                {
                    strPath = HttpContext.Current.Request.Url.AbsoluteUri;
                }
                catch { }
                if (strPath == "" || strPath.ToUpper().Contains("PNCBANK.COM") == true)
                    return "https://clearview.pncbank.com";
                else
                    return "https://clearview";
            }
            return "";
        }
        public string ImageURL()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST)
                return "http://localhost:4412";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_DEV)
                return "http://localhost:4412";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_TEST)
                return "https://clearview-test.pncbank.com";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_QA)
                return "https://clearview-qa.pncbank.com";

            if (intEnvironment == (int)CurrentEnvironment.CORPDMN ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
            {
                string strPath = "";
                try
                {
                    strPath = HttpContext.Current.Request.Url.AbsoluteUri;
                }
                catch { }
                if (strPath == "" || strPath.ToUpper().Contains("PNCBANK.COM") == true)
                    return "https://clearview.pncbank.com";
                else
                    return "https://clearview";
            }
            return "";
        }
        //public string PictureURL()
        //{
        //    if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST)
        //        return "http://localhost:4412/images/emppics/";
        //    if (intEnvironment == (int)CurrentEnvironment.CORPDMN ||
        //        intEnvironment == (int)CurrentEnvironment.PNCENG ||
        //        intEnvironment == (int)CurrentEnvironment.PNCADSTEST ||
        //        intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
        //        intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
        //        intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
        //        intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
        //        intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
        //        return "http://epsweb.ntl-city.com/subwebs/people/isorgchart/images/emppics/";
        //    return "";
        //}
        public string Domain()
        {
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "corpdmn";
            if (intEnvironment == (int)CurrentEnvironment.PNCENG)
                return "pnceng";
            if (intEnvironment == (int)CurrentEnvironment.PNCADSTEST)
                return "pncadstest";
            if (intEnvironment == (int)CurrentEnvironment.PNCRND)
                return "pncrnd";
            if (intEnvironment == (int)CurrentEnvironment.PNCUAT)
                return "pncuat";
            if (intEnvironment == (int)CurrentEnvironment.PNCQA)
                return "pncqa";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                return "pncnt";
            return "";
        }
        public string FullyQualified()
        {
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "corp.ntl-city.net";
            if (intEnvironment == (int)CurrentEnvironment.PNCENG)
                return "pnceng.pvt";
            if (intEnvironment == (int)CurrentEnvironment.PNCADSTEST)
                return "pncadstest.com";
            if (intEnvironment == (int)CurrentEnvironment.PNCRND)
                return "rnd.pncint.net";
            if (intEnvironment == (int)CurrentEnvironment.PNCUAT)
                return "uat.pncint.net";
            if (intEnvironment == (int)CurrentEnvironment.PNCQA)
                return "qa.pncint.net";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                return "pncbank.com";
            return "";
        }
        public string LDAP()
        {
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "DC=corp,DC=ntl-city,DC=net";
            if (intEnvironment == (int)CurrentEnvironment.PNCENG)
                return "DC=pnceng,DC=pvt";
            if (intEnvironment == (int)CurrentEnvironment.PNCADSTEST)
                return "DC=pncadstest,DC=com";
            if (intEnvironment == (int)CurrentEnvironment.PNCRND)
                return "DC=rnd,DC=pncint,DC=net";
            if (intEnvironment == (int)CurrentEnvironment.PNCUAT)
                return "DC=uat,DC=pncint,DC=net";
            if (intEnvironment == (int)CurrentEnvironment.PNCQA)
                return "DC=qa,DC=pncint,DC=net";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                return "DC=pncbank,DC=com";
            return "";
        }
        public int DomainID(string _domain)
        {
            if (_domain.ToUpper() == "CORPDMN" || _domain.ToUpper() == "CORP.NTL-CITY.NET")
                return (int)CurrentEnvironment.CORPDMN;
            if (_domain.ToUpper() == "PNCENG" || _domain.ToUpper() == "PNCENG.PVT")
                return (int)CurrentEnvironment.PNCENG;
            if (_domain.ToUpper() == "PNCADSTEST" || _domain.ToUpper() == "PNCADSTEST.COM")
                return (int)CurrentEnvironment.PNCADSTEST;
            if (_domain.ToUpper() == "PNCRND" || _domain.ToUpper() == "RND.PNCINT.NET")
                return (int)CurrentEnvironment.PNCRND;
            if (_domain.ToUpper() == "PNCUAT" || _domain.ToUpper() == "UAT.PNCINT.NET")
                return (int)CurrentEnvironment.PNCUAT;
            if (_domain.ToUpper() == "PNCQA" || _domain.ToUpper() == "QA.PNCINT.NET")
                return (int)CurrentEnvironment.PNCQA;
            if (_domain.ToUpper() == "PNCNT" || _domain.ToUpper() == "PNCBANK.COM")
                return (int)CurrentEnvironment.PNCNT_PROD;
            return 0;
        }
        public string ReportServiceASMX()
        {
            return "/ReportService2005.asmx?wsdl";
        }
        public string WebServiceURL()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST)
                return "http://localhost:64919/ClearViewWebServices.asmx";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_DEV)
                return "https://clearviewapps-test.pncbank.com/ClearViewWebServices.asmx";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_TEST)
                return "https://clearviewapps-test.pncbank.com/ClearViewWebServices.asmx";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_QA)
                return "https://clearviewapps-qa.pncbank.com/ClearViewWebServices.asmx";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                return "https://clearviewapps.pncbank.com/ClearViewWebServices.asmx";

            return "";
        }
        public string AltirisDomain()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD|| 
                intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "CORPDMN";
            if (intEnvironment == (int)CurrentEnvironment.PNCENG)
                return "CORPENG";
            return "";
        }
        public string AltirisUsername()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD ||
                intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "SISDP56";
            if (intEnvironment == (int)CurrentEnvironment.PNCENG)
                return "SIMADMIN";
            return "";
        }
        public string AltirisPassword()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD ||
                intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "noid10ts";
            if (intEnvironment == (int)CurrentEnvironment.PNCENG)
                return "Abcd1234";
            return "";
        }
        public string AvamarUsername()
        {
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN ||
                intEnvironment == (int)CurrentEnvironment.PNCRND ||
                intEnvironment == (int)CurrentEnvironment.PNCUAT ||
                intEnvironment == (int)CurrentEnvironment.PNCQA ||
                intEnvironment == (int)CurrentEnvironment.PNCENG ||
                intEnvironment == (int)CurrentEnvironment.PNCADSTEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                return "xsebrpavrcv";
            return "";
        }
        public string SolarisUsername()
        {
            return "admin";
        }
        public string SolarisPassword()
        {
            return "Abcd1234";
        }
        public string SolarisConsoleUsername()
        {
            return "root";
        }
        public string SolarisConsolePassword()
        {
            return "$h4d0w";
        }
        public string DNSUsername()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD ||
                intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "clearview";
            if (intEnvironment == (int)CurrentEnvironment.PNCENG)
                return "TBD";
            return "";
        }
        public string DNSPassword()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD ||
                intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "cl3arvi3w";
            if (intEnvironment == (int)CurrentEnvironment.PNCENG)
                return "TBD";
            return "";
        }
        public string BlueCatWebService(string _dsn)
        {
            string strReturn = "http://10.20.72.50/Services/API";
            Functions oFunction = new Functions(0, _dsn, intEnvironment);
            DataSet dsKey = oFunction.GetSetupValuesByKey("BLUECAT_WEBSERVICE");
            if (dsKey.Tables[0].Rows.Count > 0)
                strReturn = dsKey.Tables[0].Rows[0]["Value"].ToString();
            return strReturn;
        }
        public string BlueCatUsername()
        {
            return ADUser();
        }
        public string BlueCatPassword()
        {
            return ADPassword();
        }
        public string BlueCatConfiguration()
        {
            return "Corporate - PNC IPAM";
        }
        public string BlueCatView()
        {
            return "Corporate";
        }
        public string DNSOrganization()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST)
                return "PNC Internal Network";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD ||
                intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "PNC Internal Network";
            return "";
        }
        public string ILOUsername()
        {
            return "iadmin";
        }
        public string ILOPassword()
        {
            return "qwertyui";
        }
        public WebProxy GetProxy(string _dsn)
        {
            return new WebProxy(Proxy(_dsn));
        }
        public string Proxy(string _dsn)
        {
            string strReturn = "http://ipsbcproxy.pncbank.com:8080";
            Functions oFunction = new Functions(0, _dsn, intEnvironment);
            DataSet dsKey = oFunction.GetSetupValuesByKey("PROXY");
            if (dsKey.Tables[0].Rows.Count > 0)
                strReturn = dsKey.Tables[0].Rows[0]["Value"].ToString();
            return strReturn;
        }
        public NetworkCredential GetCredentials()
        {
            return new NetworkCredential(ADUser(), ADPassword(), Domain());
        }
        public string SwitchUsername()
        {
            return ADUser();
        }
        public string SwitchPassword()
        {
            return ADPassword();
        }
        public string NexusUsername()
        {
            if (intEnvironment  == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV)
                return "clearview";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD ||
                intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "xsclvpcview";
            return "";
        }
        public string NexusPassword()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV)
                return "Clearvi3w";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD ||
                intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "4tac@cs";
            return "";
        }
        public string ADPassword() 
		{
            Encryption oEncrypt = new Encryption();
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "05EPSiisAD316";
            if (intEnvironment == (int)CurrentEnvironment.PNCENG)
                return "@DD4cv1ew";
            if (intEnvironment == (int)CurrentEnvironment.PNCADSTEST)
                return "@D4cv1ew";
            if (intEnvironment == (int)CurrentEnvironment.PNCRND ||
                intEnvironment == (int)CurrentEnvironment.PNCUAT ||
                intEnvironment == (int)CurrentEnvironment.PNCQA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                return "@DD4cv1ew";
            return "";
		}
		public string FromEmail() 
		{
            return "clearview@pnc.com";
		}
		public string SmtpServer() 
		{
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST)
                return "vwalltest.pncbank.com";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD ||
                intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "vwall.pncbank.com";
            return "";
		}
        public string DefaultFontStyle()
        {
            return "font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:12px; color:#404040";
        }
        public string DefaultFontStyleBold()
        {
            return "font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:12px; color:#404040; font-weight:bold";
        }
        public string DefaultFontStyleHeader()
        {
            return "font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:14px; color:#404040; font-weight:bold";
        }
        public string UserOUTechnical()
        {
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "OU=OUu_Technical,OU=OUc_UsrAccts,OU=OUc_InfoClients,";
            if (intEnvironment == (int)CurrentEnvironment.PNCENG)
                return "CN=PNC_Users,";
            if (intEnvironment == (int)CurrentEnvironment.PNCADSTEST)
                return "CN=Users,";
            if (intEnvironment == (int)CurrentEnvironment.PNCRND ||
                intEnvironment == (int)CurrentEnvironment.PNCUAT ||
                intEnvironment == (int)CurrentEnvironment.PNCQA)
                return "OU=OUu_Technical,OU=OUc_User,OU=OUc_Accounts,";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                return "OU=PNC_Users,";
            return "";
        }
        public string UserOU()
        {
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "OU=OUu_Standard,OU=OUc_UsrAccts,OU=OUc_InfoClients,";
            if (intEnvironment == (int)CurrentEnvironment.PNCENG)
                return "CN=PNC_Users,";
            if (intEnvironment == (int)CurrentEnvironment.PNCADSTEST)
                return "CN=Users,";
            if (intEnvironment == (int)CurrentEnvironment.PNCRND ||
                intEnvironment == (int)CurrentEnvironment.PNCUAT ||
                intEnvironment == (int)CurrentEnvironment.PNCQA)
                return "OU=OUu_Standard,OU=OUc_User,OU=OUc_Accounts,";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                return "OU=PNC_Users,";
            return "";
        }
        public string OU()
        {
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "";
            if (intEnvironment == (int)CurrentEnvironment.PNCENG)
                return "OU=OUc_Resources,";
            if (intEnvironment == 900)
                return "OU=OUc_Resources,";
            if (intEnvironment == (int)CurrentEnvironment.PNCRND ||
                intEnvironment == (int)CurrentEnvironment.PNCUAT ||
                intEnvironment == (int)CurrentEnvironment.PNCQA)
                return "OU=OUc_Resources,";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                return "OU=OUc_Resources,";
            return "";
        }
        public string GroupOU()
        {
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "OU=OUg_Admins,OU=OUc_Orgs,";
            if (intEnvironment == (int)CurrentEnvironment.PNCENG)
                return "";
            if (intEnvironment == (int)CurrentEnvironment.PNCADSTEST)
                return "";
            if (intEnvironment == (int)CurrentEnvironment.PNCRND ||
                intEnvironment == (int)CurrentEnvironment.PNCUAT ||
                intEnvironment == (int)CurrentEnvironment.PNCQA)
                return "";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                return "";
            return "";
        }
        public string ServerOU()
        {
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "OU=OUc_Srvs,OU=OUc_DmnCptrs,";
            if (intEnvironment == (int)CurrentEnvironment.PNCENG)
                return "OU=Servers,";
            if (intEnvironment == (int)CurrentEnvironment.PNCADSTEST)
                return "OU=Servers,";
            if (intEnvironment == (int)CurrentEnvironment.PNCRND ||
                intEnvironment == (int)CurrentEnvironment.PNCUAT ||
                intEnvironment == (int)CurrentEnvironment.PNCQA)
                return "OU=OUc_Servers,OU=OUc_Computers,";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                return "OU=Servers,";
            return "";
        }
        public string WorkstationOU()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCRND ||
                intEnvironment == (int)CurrentEnvironment.PNCUAT ||
                intEnvironment == (int)CurrentEnvironment.PNCQA)
                return "OU=OUw_VDI,OU=OUw_Standard,OU=OUc_Workstations,OU=OUc_Computers,";
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN ||
                intEnvironment == (int)CurrentEnvironment.PNCENG ||
                intEnvironment == (int)CurrentEnvironment.PNCADSTEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_QA ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                return "OU=OUw_Standard,OU=OUc_Wksts,OU=OUc_DmnCptrs,";
            return "";
        }

        public string Community()
        {
            // if changes are made, be sure to make the change in the /down.htm file also
            return "https://connections.pncbank.com/communities/community/clearview";
        }

        public string DocumentsFolder()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST)
                return @"\\wdclv110a\documents$\";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_QA)
                return @"\\wcclv206a\documents$\";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                return @"\\wcclv309a\documents$\";
            return string.Empty;
        }

        public string ParseDocument(string _path)
        {
            string strReturn = _path.Substring(2);
            strReturn = strReturn.Substring(0, strReturn.IndexOf("\\"));
            return strReturn;
        }

        public string UploadsFolder()
        {
            return DocumentsFolder() + @"Uploads\";
        }

        public string MnemonicsImportTable()
        {
            return "tbl_PNC_TO_SC_APPCODEFEED";
        }

        public string EventLogs()
        {
            if (intEnvironment == 1 || intEnvironment == 2)
                return "OHCLEIIS4333";
            if (intEnvironment == 3)
                return "OHCLEIIS4569";
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "OHCLEIIS1319";
            return "";
        }
        public string DNS_Domain()
        {
            if (intEnvironment == 1 || intEnvironment == 2)
                return "cle.ncc";
            if (intEnvironment == 3)
                return "cle.ncc";
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "pncbank.com";
            return "";
        }
        public string DNS_NameService()
        {
            return "A PTR";
        }
        public string DNS_DynamicDNSUpdate()
        {
            return "A PTR CNAME MX";
        }
        public string SolarisFlags(string _build_server, string _build_net, string _build_type, bool _vas_enabled)
        {
            return "AUTO_BUILD:true,BUILD_SERVER:" + _build_server + ",BUILD_NET:" + _build_net + ",CLEARVIEW_LOG:true,BUILD_TYPE:" + _build_type + ",VAS_ENABLED:" + (_vas_enabled ? "true" : "false");
        }
        public string LocalAdminUsername(bool _pnc)
        {
            if (_pnc == true)
                return "WTMPBUILD";
            else
                return "onevoice";
        }
        public string LocalAdminPassword(bool _pnc, string _name)
        {
            if (_pnc == true && _name != "")
            {
                string strLast3 = _name.Trim().Substring(_name.Trim().Length - 3);
                return "tIEr@" + strLast3.ToLower();
            }
            else
                return "4AdminW03";
        }
        public string dsnZeus()
        {
            return "data source=DTGPCSQA20\\DTGPCSQA20;uid=cvzeus;password=hor1z@n;database=ClearviewZeus";
        }
        public string dsnRemote()
        {
            return "data source=OHCLEIIS4569;uid=cvremote;password=sh33p;database=ClearviewRemote";
        }
        public string dsnBetween()
        {
            return "data source=DTGPCSQA20\\DTGPCSQA20;Integrated Security=SSPI;Initial Catalog=ClearviewProd";
        }
        public string[] PNC_AD_Groups()
        {
            //return new string[] { "AppSupport", "AppUsers", "AuthProbMgmt", "Developers", "Promoters", "AuthPromoters" };
            return new string[] { "AppSupport", "AppUsers", "AuthProbMgmt", "Developers", "AuthPromoters" };
        }
        public string[] PingDNS()
        {
            return new string[] { 
                "",
                ".pncbank.com", 
                ".pnc.com", 
                ".corpdev.ntl-city.net", 
                ".corptest.ntl-city.net", 
                ".corp.ntl-city.net", 
                ".ntl-city.net", 
                ".ntl-city.com", 
                ".ntl-city.sec", 
                ".tstctr.ntl-city.net",
                ".mwbranch.corp.ntl-city.net",
                ".mwbranch.corpdev.ntl-city.net",
                ".mwbranch.corptest.ntl-city.net",
                ".pncadstest.com"
            };
        }
        public string eDirectoryHost()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST)
                return "mdsemptest.pncbank.com";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_QA)
                return "mdsempqa.pncbank.com";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_PROD ||
                intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "mdsemp.pncbank.com";
            return "";
        }
        public string eDirectoryUsername()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST)
                return "cn=clv-serviceid,ou=TrustedApplications,ou=FrameworkSystems,o=pnc";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_QA)
                return "cn=clv-serviceid,ou=TrustedApplications,ou=FrameworkSystems,o=pnc";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_PROD ||
                intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "cn=clv-serviceid,ou=TrustedApplications,ou=FrameworkSystems,o=pnc";
            return "";
        }
        public string eDirectoryPassword()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST)
                return "AY3AVN";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_QA)
                return "-L-M2z";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_PROD ||
                intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "cPPnuh";
            return "";
        }
        public string ServiceNowHost()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST)
                return "https://webtest-itsm.pncbank.com";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_QA)
                return "https://webqa-itsm.pncbank.com";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_PROD ||
                intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "https://itsm.pncbank.com";
            return "";
        }
        public string ServiceNowUsername()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST)
                return "_clearview_user";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_QA)
                return "_clearview_user";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_PROD ||
                intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "_clearview_user";
            return "";
        }
        public string ServiceNowPassword()
        {
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_LOCALHOST ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_DEV ||
                intEnvironment == (int)CurrentEnvironment.PNCNT_TEST)
                return "_clearview_user";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_QA)
                return "_clearview_userQA";
            if (intEnvironment == (int)CurrentEnvironment.PNCNT_PROD ||
                intEnvironment == (int)CurrentEnvironment.CORPDMN)
                return "_clearview_user$prod1";
            return "";
        }
    }
}
