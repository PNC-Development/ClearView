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
    public partial class bypass : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Asset oAsset;
        protected Log oLog;
        protected Users oUser;
        protected Servers oServer;
        protected TSM oTSM;
        protected ResourceRequest oResourceRequest;
        protected IPAddresses oIPAddresses;
        protected ServerName oServerName;
        protected AssetOrder oAssetOrder;

        protected int intProfile;
        private char[] strSplit = { ';' };

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/bypass.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oAsset = new Asset(intProfile, dsnAsset, dsn);
            oLog = new Log(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oTSM = new TSM(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
            oServerName = new ServerName(intProfile, dsn);
            oAssetOrder = new AssetOrder(intProfile, dsn, dsnAsset, intEnvironment);
            if (!IsPostBack)
            {
                Variables oVariable = new Variables(intEnvironment);
                txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','hdnAJAXValue','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
                txtFind.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnFind.ClientID + "').click();return false;}} else {return true}; ");
                btnFind.Attributes.Add("onclick", "SearchText('" + txtFind.ClientID + "','" + panResult.ClientID + "');return false;");
                txtName.Focus();
                btnSearch.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a device name');");
                btnSubmit.Attributes.Add("onclick", "return ValidateHidden0('hdnAJAXValue','" + txtUser.ClientID + "','Please select the requestor / client')"
                    + " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid date')"
                    + " && ValidateText('" + txtPTM.ClientID + "','Please enter a change control')"
                    + " && ValidateText('" + txtReason.ClientID + "','Please enter a reason')"
                    + "&& confirm('WARNING: You are about to alter production data! This action CANNOT be undone!\\n\\nAre you sure you want to continue?') && confirm('LAST CHANCE! If you do not know what you are doing, I suggest you do not do this!\\n\\nAre you REALLY sure you want to do this?');");
            }
            if (Request.QueryString["assets"] != null)
            {
                string[] strAssets = Request.QueryString["assets"].Split(strSplit);
                StringBuilder strDeploy = new StringBuilder();
                strDeploy.Append("<table cellpadding=\"3\" cellspacing=\"0\" border=\"0\" style=\"border:solid 1px #CCCCCC\">");
                strDeploy.Append("<tr bgcolor=\"#EEEEEE\"><td><b>Server Name</b></td><td><b>Serial Number</b></td><td><b>Power On Using</b></td><td><b>Backup?</b></td></tr>");
                int intBypass = 0;
                for (int ii = 0; ii < strAssets.Length; ii++)
                {
                    if (strAssets[ii].Trim() != "")
                    {
                        int intAsset = Int32.Parse(strAssets[ii].Trim());
                        string strILO = oAsset.GetServerOrBlade(intAsset, "ilo");
                        strDeploy.Append("<tr>");
                        strDeploy.Append("<td>");
                        string strName = oAsset.GetStatus(intAsset, "name");
                        strDeploy.Append(strName);
                        strDeploy.Append("</td>");
                        strDeploy.Append("<td>");
                        strDeploy.Append(oAsset.Get(intAsset, "serial"));
                        strDeploy.Append("</td>");
                        strDeploy.Append("<td>");
                        strDeploy.Append(strILO == "" ? "VMware Virtual Center" : "ILO: <a href=\"https://" + strILO + "\" target=\"_blank\">" + strILO + "</a>");
                        strDeploy.Append("</td>");
                        strDeploy.Append("<td>");
                        DataSet dsTSM = oTSM.GetDecom(strName);
                        if (dsTSM.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTSM = dsTSM.Tables[0].Rows[0];
                            strDeploy.Append("YES<br/>");
                            strDeploy.Append("Server: " + drTSM["server"].ToString() + "<br/>");
                            strDeploy.Append("Port: " + drTSM["port"].ToString() + "<br/>");
                            strDeploy.Append("Domain: " + drTSM["domain"].ToString() + "<br/>");
                            strDeploy.Append("Schedule: " + drTSM["schedule"].ToString() + "<br/>");
                            strDeploy.Append("Contact(s): " + drTSM["contacts"].ToString());
                        }
                        else
                            strDeploy.Append("NO");
                        strDeploy.Append("</td>");
                        strDeploy.Append("</tr>");
                        intBypass++;
                    }
                }
                strDeploy.Append("</table>");
                lblResult.Text = "<p>ClearView bypassed the cooldown for " + intBypass.ToString() + " device(s)...</p>";
                lblResult.Text += "<p>Please use the following information to configure the devices...</p>";
                lblResult.Text += strDeploy.ToString();
            }
        }
        protected void btnSearch_Click(Object Sender, EventArgs e)
        {
            DataSet ds = oAsset.GetDecommissionBypass(txtName.Text);
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = ddlOrder.SelectedItem.Value;
            rptDevices.DataSource = dv;
            rptDevices.DataBind();
            int intCount = 0;
            foreach (RepeaterItem ri in rptDevices.Items)
            {
                CheckBox chkDevice = (CheckBox)ri.FindControl("chkDevice");
                Label lblStatus = (Label)ri.FindControl("lblStatus");
                if (lblStatus.Text == "-1")
                {
                    lblStatus.Text = "<span class=\"denied\">Error</span>";
                    chkDevice.Visible = false;
                }
                else if (lblStatus.Text == "-2")
                {
                    lblStatus.Text = "<span class=\"denied\">Cancelled</span>";
                    chkDevice.Visible = false;
                }
                else
                {
                    Label lblTurnedOff = (Label)ri.FindControl("lblTurnedOff");
                    Label lblDestroyed = (Label)ri.FindControl("lblDestroyed");
                    if (lblStatus.Text == "2")
                        lblStatus.Text = "Assigned";
                    else if (lblStatus.Text == "1")
                        lblStatus.Text = "Running...";
                    else if (lblTurnedOff.Text != "" && lblDestroyed.Text == "")
                        lblStatus.Text = "<span class=\"bold\">In Cooldown</span>";
                    else
                    {
                        lblStatus.Text = "<span class=\"waiting\">Not available</span>";
                        chkDevice.Enabled = false;
                    }
                    Label lblBypassed = (Label)ri.FindControl("lblBypassed");
                    if (lblBypassed.Text != "")
                    {
                        string strUser = "0";
                        string strPTM = "0";
                        if (lblBypassed.ToolTip.Contains("_") == true)
                        {
                            strUser = lblBypassed.ToolTip.Substring(0, lblBypassed.ToolTip.IndexOf("_"));
                            lblBypassed.ToolTip = lblBypassed.ToolTip.Substring(lblBypassed.ToolTip.IndexOf("_") + 1);
                            if (lblBypassed.ToolTip.Contains("_") == true)
                            {
                                strPTM = lblBypassed.ToolTip.Substring(0, lblBypassed.ToolTip.IndexOf("_"));
                                lblBypassed.ToolTip = lblBypassed.ToolTip.Substring(lblBypassed.ToolTip.IndexOf("_") + 1);
                            }
                        }
                        lblStatus.Text = "<span class=\"shelved\">Already<br/>Bypassed</span>";
                        lblBypassed.Text = "<span class=\"redheader\">Already Bypassed</span><br/><br/><span class=\"reddefault\"><b>On</b> : " + lblBypassed.Text + "<br/><b>By</b> : " + oUser.GetFullName(Int32.Parse(strUser)) + "<br/><b>PTM</b> : " + strPTM + "<br/><b>Reason</b> : " + lblBypassed.ToolTip + "</span>";
                        chkDevice.Visible = false;
                    }
                    else
                    {
                        intCount++;
                        lblBypassed.Text = "";
                        if (lblDestroyed.Text != "")
                        {
                            lblStatus.Text = "<span class=\"approved\">Completed</span>";
                            chkDevice.Attributes.Add("onclick", "UpdateCompleteCheck(this);");
                        }
                    }
                }
            }
            lblResult.Text = "Your search returned " + intCount.ToString() + " asset(s)...";
            panResult.Visible = true;
            txtFind.Text = txtName.Text;
            btnFind.Enabled = true;
            btnSubmit.Enabled = (intCount > 0);
            txtFind.Focus();
            if (txtDate.Text == "")
                txtDate.Text = DateTime.Today.ToShortDateString();
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            int intUser = 0;
            string strAssets = "";
            if (Int32.TryParse(Request.Form["hdnAJAXValue"], out intUser) == true && intUser > 0)
            {
                foreach (RepeaterItem ri in rptDevices.Items)
                {
                    CheckBox chkDevice = (CheckBox)ri.FindControl("chkDevice");
                    Label lblName = (Label)ri.FindControl("lblName");
                    Label lblSerial = (Label)ri.FindControl("lblSerial");
                    if (chkDevice.Checked == true)
                    {
                        int intServer = Int32.Parse(chkDevice.ToolTip);
                        DataSet dsServers = oServer.GetAssetsServer(intServer);
                        foreach (DataRow drServer in dsServers.Tables[0].Rows)
                        {
                            int intAsset = Int32.Parse(drServer["assetid"].ToString());
                            string strName = lblName.Text;
                            if (drServer["dr"].ToString() == "1")
                                strName += "-DR";
                            // Update Bypass Reason
                            oAsset.UpdateDecommissionBypass(intAsset, txtDate.Text, intUser, txtReason.Text, txtPTM.Text);
                            // Set strAssets to assets bypassed (for status message on postback)
                            if (strAssets != "")
                                strAssets += strSplit[0].ToString();
                            strAssets += intAsset.ToString();
                        }
                        // Add log entry
                        oLog.AddEvent(lblName.Text, lblSerial.Text, "Asset Decommission Bypass (Client = " + oUser.GetFullName(intUser) + ", Date = " + txtDate.Text + ", PTM = " + txtPTM.Text + ")", LoggingType.Information);
                    }
                }
            }
            Response.Redirect(Request.Path + "?assets=" + strAssets);
        }
    }
}
