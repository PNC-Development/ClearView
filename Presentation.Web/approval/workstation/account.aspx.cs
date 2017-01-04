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
    public partial class account : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected string strTitle = ConfigurationManager.AppSettings["appTitle"];
        protected int intProfile;
        protected string strAccounts = "";
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            StringBuilder sb = new StringBuilder(strAccounts);
            Page.Title = strTitle;
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">alert('Approval has been submitted');<" + "/" + "script>");
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intID = Int32.Parse(Request.QueryString["id"]);
                Workstations oWorkstation = new Workstations(intProfile, dsn);
                Forecast oForecast = new Forecast(intProfile, dsn);
                Users oUser = new Users(intProfile, dsn);
                Asset oAsset = new Asset(intProfile, dsnAsset);
                int intAnswer = Int32.Parse(oWorkstation.GetVirtual(intID, "answerid"));
                if (Int32.Parse(oForecast.GetAnswer(intAnswer, "appcontact")) == intProfile)
                {
                    panPermit.Visible = true;
                    int intAsset = Int32.Parse(oWorkstation.GetVirtual(intID, "assetid"));
                    lblName.Text = oAsset.Get(intAsset, "name");
                    lblManager.Text = oUser.GetFullName(intProfile);
                    DataSet ds = oWorkstation.GetAccountsVirtual(intAsset);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>");
                        sb.Append(oUser.GetFullName(Int32.Parse(dr["userid"].ToString())));
                        sb.Append("</td>");

                        string strRights = "";

                        if (dr["remote"].ToString() == "1")
                        {
                            strRights = "Remote Access";
                        }

                        if (dr["admin"].ToString() == "1")
                        {
                            if (strRights != "")
                            {
                                strRights += ", ";
                            }

                            strRights += "Administrator";
                        }

                        sb.Append("<td>");
                        sb.Append(strRights);
                        sb.Append("</td>");
                        sb.Append("<td align=\"center\"><input type=\"radio\" name=\"approve");
                        sb.Append(dr["id"].ToString());
                        sb.Append("\" onclick=\"ApproveCheckBox(this, '");
                        sb.Append(dr["id"].ToString());
                        sb.Append("','");
                        sb.Append(hdnApprove.ClientID);
                        sb.Append("');\"/></td>");
                        sb.Append("<td align=\"center\"><input type=\"radio\" name=\"approve");
                        sb.Append(dr["id"].ToString());
                        sb.Append("\" onclick=\"ApproveCheckBox(this, '");
                        sb.Append(dr["id"].ToString());
                        sb.Append("','");
                        sb.Append(hdnDeny.ClientID);
                        sb.Append("');\"/></td>");
                        sb.Append("</tr>");
                    }

                    if (sb.ToString() == "")
                    {
                        sb.Append("<tr><td colspan=\"4\"><img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\"/> There are no accounts</td></tr>");
                        btnSave.Enabled = false;
                    }
                }
                else
                {
                    panDenied.Visible = true;
                }
            }

            strAccounts = sb.ToString();
            btnSave.Attributes.Add("onclick", "return confirm('WARNING: The accounts you have approved will automatically be added to the appropriate groups.\\n\\nAre you sure you want to continue?');");
            btnClose.Attributes.Add("onclick", "return CloseWindow();");
            Control oControl = (Control)LoadControl("/controls/sys/sys_rotator_header.ascx");
            PH4.Controls.Add(oControl);
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            string strGroupRemote = "GSGu_WKS" + lblName.Text + "RemoteA";
            string strGroupAdmin = "GSGu_WKS" + lblName.Text + "Adm";
            bool bool2000 = (lblName.Text.ToUpper().StartsWith("T2K") == true || lblName.Text.ToUpper().StartsWith("W2K") == true);
            Workstations oWorkstation = new Workstations(intProfile, dsn);
            Users oUser = new Users(intProfile, dsn);
            Domains oDomain = new Domains(intProfile, dsn);
            int intDomain = Int32.Parse(oWorkstation.GetVirtual(intID, "domainid"));
            intDomain = Int32.Parse(oDomain.Get(intDomain, "environment"));
            AD oAD = new AD(intProfile, dsn, intDomain);
            string strHidden = Request.Form[hdnApprove.UniqueID];
            while (strHidden != "")
            {
                string strField = strHidden.Substring(0, strHidden.IndexOf("&"));
                strHidden = strHidden.Substring(strHidden.IndexOf("&") + 1);
                string strFlag = strField.Substring(strField.IndexOf("_") + 1);
                strField = strField.Substring(0, strField.IndexOf("_"));
                if (strFlag == "1")
                {
                    int intAccount = Int32.Parse(strField);
                    DataSet ds = oWorkstation.GetAccount(intAccount);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int intUser = Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                        string strID = oUser.GetName(intUser);
                        if (intDomain != (int)CurrentEnvironment.CORPDMN && intDomain != (int)CurrentEnvironment.PNCNT_PROD)
                        {
                            strID = "E" + strID.Substring(1);
                            if (oAD.Search(strID, false) == null)
                            {
                                strID = "T" + strID.Substring(1);
                                if (oAD.Search(strID, false) == null)
                                    oAD.CreateUser(strID, strID, strID, "Abcd1234", "", "Created by ClearView - " + DateTime.Now.ToShortDateString(), "");
                            }
                            if (ds.Tables[0].Rows[0]["admin"].ToString() == "1")
                                oAD.JoinGroup(strID, strGroupAdmin, 0);
                        }
                        if (ds.Tables[0].Rows[0]["remote"].ToString() == "1" && bool2000 == false)
                            oAD.JoinGroup(strID, strGroupRemote, 0);
                    }
                    oWorkstation.UpdateAccount(intAccount);
                }
            }
            strHidden = Request.Form[hdnDeny.UniqueID];
            while (strHidden != "")
            {
                string strField = strHidden.Substring(0, strHidden.IndexOf("&"));
                strHidden = strHidden.Substring(strHidden.IndexOf("&") + 1);
                string strFlag = strField.Substring(strField.IndexOf("_") + 1);
                strField = strField.Substring(0, strField.IndexOf("_"));
                if (strFlag == "1")
                {
                    int intAccount = Int32.Parse(strField);
                    oWorkstation.UpdateAccount(intAccount);
                }
            }
        }
    }
}
