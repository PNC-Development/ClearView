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
using System.DirectoryServices;

namespace NCC.ClearView.Presentation.Web
{
    public partial class ajax_ad_groups : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intMaximum = Int32.Parse(ConfigurationManager.AppSettings["MAX_AD_RESULTS"]);
        protected string strResponse = "";
        protected int intCount = 0;
        protected DataTable oTable;
        protected AccountRequest oAccountRequest;
        protected bool boolFound = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            oAccountRequest = new AccountRequest(0, dsn);
            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                XmlDocument oDoc = new XmlDocument();
                oDoc.Load(Request.InputStream);
                strResponse += "<values>";
                AddGroups(Int32.Parse(oDoc.ChildNodes[0].ChildNodes[1].InnerText), Server.UrlDecode(oDoc.ChildNodes[0].ChildNodes[0].InnerText));
                //strResponse += "<value>TEST</value>";
                strResponse += "</values>";
                Response.ContentType = "text/xml";
                if (intCount > intMaximum)
                    Response.Write("<values><value>MULTIPLE</value></values>");
                else
                    Response.Write(strResponse);
                Response.End();
            }
        }
        private void AddGroups(int _domain, string _name)
        {
            if (boolFound == false)
                AddGroups(_domain, _name, "OUg_Admins,ou=OUc_Orgs,", false);
            if (boolFound == false)
                AddGroups(_domain, _name, "OUg_Apps,ou=OUc_Orgs,", false);
            if (boolFound == false)
                AddGroups(_domain, _name, "OUg_Funcs,ou=OUc_Orgs,", false);
            if (boolFound == false)
                AddGroups(_domain, _name, "OUg_Resources,", true);
        }
        private void AddGroups(int _domain, string _name, string _ou, bool _disect)
        {
            Variables oVariable = new Variables(_domain);
            string strPath = "LDAP://" + oVariable.primaryDCName(dsn) + "/ou=" + _ou + oVariable.LDAP();
            DirectoryEntry oParent = new DirectoryEntry(strPath, oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
            if (_disect == false)
            {
                foreach (DirectoryEntry oChild in oParent.Children)
                {
                    if (boolFound == true)
                        break;
                    string strName = oChild.Name.Substring(3);
                    if (oAccountRequest.GetExceptions(_domain, strName) == false)
                    {
                        if (CheckName(_name, strName) == true)
                        {
                            strResponse += "<value><![CDATA[" + Add(strName) + "]]></value><text><![CDATA[" + Add(strName) + "]]></text>";
                            intCount++;
                        }
                    }
                }
            }
            else
            {
                oTable = new DataTable();
                DataColumn oColumn = new DataColumn();
                oColumn.DataType = System.Type.GetType("System.String");
                oColumn.ColumnName = "name";
                oTable.Columns.Add(oColumn);
                Populate(oParent, _domain);
                DataView dv = oTable.DefaultView;
                dv.Sort = "name";
                foreach (DataRowView dr in dv)
                {
                    if (boolFound == true)
                        break;
                    if (CheckName(_name, dr["name"].ToString()) == true)
                    {
                        strResponse += "<value><![CDATA[" + Add(dr["name"].ToString()) + "]]></value><text><![CDATA[" + Add(dr["name"].ToString()) + "]]></text>";
                        intCount++;
                    }
                }
            }
        }
        private void Populate(DirectoryEntry oParent, int _domain)
        {
            foreach (DirectoryEntry oChild in oParent.Children)
            {
                string strName = oChild.Name.Substring(3);
                foreach (Object oValue in oChild.Properties["objectClass"])
                {
                    if (oValue.ToString().ToUpper().Trim() == "GROUP")
                    {
                        if (oAccountRequest.GetExceptions(_domain, strName) == false)
                        {
                            DataRow oRow = oTable.NewRow();
                            oRow["name"] = strName;
                            oTable.Rows.Add(oRow);
                        }
                    }
                    else if (oValue.ToString().ToUpper().Trim() == "ORGANIZATIONALUNIT")
                        Populate(oChild, _domain);
                }
            }
        }
        private bool CheckName(string _typed, string _name)
        {
            if (_name.ToUpper() == _typed.ToUpper())
            {
                boolFound = true;
                return true;
            }
            else if (_name.ToUpper() == "GSGU_" + _typed.ToUpper())
            {
                boolFound = true;
                return true;
            }
            else if (_name.ToUpper() == "GSGU_" + _typed.ToUpper() + "ADM")
            {
                boolFound = true;
                return true;
            }
            else if (_name.ToUpper().StartsWith(_typed.ToUpper()) == true)
                return true;
            else if (_name.ToUpper().StartsWith("GSGU_" + _typed.ToUpper()) == true)
                return true;
            else
                return false;
        }
        private string Add(string _name)
        {
            //return Regex.Replace(_name, "[^a-zA-Z0-9_]", "");
            return _name;
        }
    }
}
