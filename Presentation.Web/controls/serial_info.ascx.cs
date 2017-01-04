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
using Microsoft.ApplicationBlocks.Data;
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class serial_info : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intUnder48A = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_RESPONSE"]);
        protected int intUnder48Q = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_QUESTION"]);
        protected Pages oPage;
        protected Users oUser;
        protected Servers oServer;
        protected ResourceRequest oResourceRequest;
        protected ModelsProperties oModelsProperties;
        protected Asset oAsset;
        protected Functions oFunctions;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strMultiple = "";
        protected string strResults = "";
        protected string strAssetAdmins = ConfigurationManager.AppSettings["ASSET_ADMINS"];

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oFunctions = new Functions(intProfile, dsn, intEnvironment);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            bool boolAdmin = false;
            string[] strProfile;
            char[] strSplit = { ';' };
            strProfile = strAssetAdmins.Split(strSplit);
            for (int ii = 0; ii < strProfile.Length; ii++)
            {
                if (strProfile[ii].Trim() != "")
                {
                    if (Int32.Parse(strProfile[ii].Trim()) == intProfile)
                        boolAdmin = true;
                }
            }
            btnStatus.Enabled = boolAdmin;
            if (!IsPostBack)
            {
                if (Request.QueryString["serial"] != null)
                {
                    string strQuery = oFunctions.decryptQueryString(Request.QueryString["serial"]);
                    txtSerial.Text = strQuery;
                    CheckResults(strQuery);
                }
                txtSerial.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");
                btnSearch.Attributes.Add("onclick", "return ValidateText('" + txtSerial.ClientID + "','Please enter a serial number');");
                btnStatus.Attributes.Add("onclick", "return (ValidateText('" + txtName.ClientID + "','Please enter the server name') && confirm('NOTE: Changing the status of a device could greatly impact the ability of ClearView to auto-provision devices.\\n\\nAre you sure you want to update the status?'));");
            }
        }
        private void CheckResults(string strSerial)
        {
            DataSet ds = oAsset.GetAssetsSerial(strSerial);
            StringBuilder sb = new StringBuilder();
            
            if (ds.Tables[0].Rows.Count == 1)
            {
                panSearch.Visible = true;
                lblSerial.Text = strSerial;
                int intAsset = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
                sb = new StringBuilder(strResults);
                sb.Append("<tr><td nowrap>AssetID:</td><td width=\"100%\">");
                sb.Append(intAsset.ToString());
                sb.Append("</td></tr>");
                lblAsset.Text = intAsset.ToString();
                sb.Append("<tr><td nowrap>Model:</td><td width=\"100%\">");
                sb.Append(oModelsProperties.Get(intModel, "name"));
                sb.Append("</td></tr>");
                if (ds.Tables[0].Rows[0]["asset_attribute"].ToString() == "1")
                {
                    sb.Append("<tr><td colspan=\"2\" class=\"reddefault\"><img src=\"/images/cancel.gif\" border=\"0\" align=\"absmiddle\"/> <b>There is a problem with this asset - it is currently unavailable!</b></td></tr>");
                }
                ds = oAsset.GetServerOrBlade(intAsset);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sb.Append("<tr><td nowrap>ILO:</td><td width=\"100%\">");
                    sb.Append(ds.Tables[0].Rows[0]["ilo"].ToString());
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap>Dummy Name:</td><td width=\"100%\">");
                    sb.Append(ds.Tables[0].Rows[0]["dummy_name"].ToString());
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap>VLAN:</td><td width=\"100%\">");
                    sb.Append(ds.Tables[0].Rows[0]["vlan"].ToString());
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap>Class:</td><td width=\"100%\">");
                    sb.Append(ds.Tables[0].Rows[0]["class"].ToString());
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap>Environment:</td><td width=\"100%\">");
                    sb.Append(ds.Tables[0].Rows[0]["environment"].ToString());
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap>Location:</td><td width=\"100%\">");
                    sb.Append(ds.Tables[0].Rows[0]["location"].ToString());
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap>Room:</td><td width=\"100%\">");
                    sb.Append(ds.Tables[0].Rows[0]["room"].ToString());
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap>Rack:</td><td width=\"100%\">");
                    sb.Append(ds.Tables[0].Rows[0]["rack"].ToString());
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td nowrap>Rack Position:</td><td width=\"100%\">");
                    sb.Append(ds.Tables[0].Rows[0]["rackposition"].ToString());
                    sb.Append("</td></tr>");

                    int intEnclosure = Int32.Parse(ds.Tables[0].Rows[0]["enclosureid"].ToString());
                    if (intEnclosure > 0)
                    {
                        sb.Append("<tr><td nowrap>EnclosureID:</td><td width=\"100%\">");
                        sb.Append(intEnclosure.ToString());
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td nowrap>Enclosure Name:</td><td width=\"100%\">");
                        sb.Append(oAsset.GetEnclosure(intEnclosure, "name"));
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td nowrap>Slot #:</td><td width=\"100%\">");
                        sb.Append(ds.Tables[0].Rows[0]["slot"].ToString());
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td nowrap>Spare?:</td><td width=\"100%\">");
                        sb.Append(ds.Tables[0].Rows[0]["spare"].ToString() == "1" ? "Yes" : "No");
                        sb.Append("</td></tr>");
                    }
                }

                StringBuilder sbStatus = new StringBuilder();
                string strName = "";
                ds = oAsset.GetCVAStatus(intAsset);
                int intStatus = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    intStatus = Int32.Parse(dr["status"].ToString());
                    strName = dr["name"].ToString();
                    sbStatus.Append("<tr>");
                    sbStatus.Append("<td>");
                    sbStatus.Append(dr["datestamp"].ToString());
                    sbStatus.Append("</td>");
                    sbStatus.Append("<td>");
                    sbStatus.Append(dr["statusname"].ToString());
                    sbStatus.Append("</td>");
                    sbStatus.Append("<td>");
                    sbStatus.Append(oUser.GetFullName(Int32.Parse(dr["userid"].ToString())));
                    sbStatus.Append("</td>");
                    sbStatus.Append("<td>");
                    sbStatus.Append(dr["name"].ToString());
                    sbStatus.Append("</td>");
                    sbStatus.Append("</tr>");
                }

                if (sbStatus.ToString() == "")
                {
                    sbStatus.Append("<tr><td colspan=\"4\"><img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\"/> There is no history of this device</td></tr>");
                }

                sbStatus.Insert(0, "<tr class=\"bold\"><td>Datestamp</td><td>Status</td><td>User</td><td>Name</td></tr>");
                sbStatus.Insert(0, "<table cellpadding=\"3\" cellspacing=\"2\" border=\"0\">");
                sbStatus.Append("</table>");

                sb.Append("<tr><td nowrap colspan=\"2\">");
                sb.Append(sbStatus.ToString());
                sb.Append("</td></tr>");

                ddlStatus.SelectedValue = intStatus.ToString();
                txtName.Text = strName;

                strResults = sb.ToString();
            }
            else
            {
                panMultiple.Visible = true;
                sb = new StringBuilder(strMultiple);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sb.Append("<tr onmouseover=\"CellRowOver(this);\" onmouseout=\"CellRowOut(this);\" onclick=\"window.navigate('");
                    sb.Append(oPage.GetFullLink(intPage));
                    sb.Append("?serial=");
                    sb.Append(oFunctions.encryptQueryString(dr["serial"].ToString()));
                    sb.Append("');\">");
                    sb.Append("<td>");
                    sb.Append(dr["serial"].ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(dr["asset"].ToString());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(oModelsProperties.Get(Int32.Parse(dr["modelid"].ToString()), "name"));
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }

                strMultiple = sb.ToString();
            }
        }
        protected void btnSearch_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?serial=" + oFunctions.encryptQueryString(txtSerial.Text));
        }
        protected void btnStatus_Click(Object Sender, EventArgs e)
        {
            oAsset.AddStatus(Int32.Parse(lblAsset.Text), txtName.Text, Int32.Parse(ddlStatus.SelectedItem.Value), intProfile, DateTime.Now);
            Response.Redirect(oPage.GetFullLink(intPage) + "?serial=" + oFunctions.encryptQueryString(txtSerial.Text));
        }
    }
}