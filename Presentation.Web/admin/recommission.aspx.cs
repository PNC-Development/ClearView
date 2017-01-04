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
using NCC.ClearView.Application.Core.ClearViewWS;
namespace NCC.ClearView.Presentation.Web
{
    public partial class recommission : BasePage
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
        protected Variables oVariable;

        protected int intProfile;
        private char[] strSplit = { ';' };

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/recommission.aspx";
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
            oVariable = new Variables(intEnvironment);
            if (!IsPostBack)
            {
                txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','hdnAJAXValue','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
                txtFind.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnFind.ClientID + "').click();return false;}} else {return true}; ");
                btnFind.Attributes.Add("onclick", "SearchText('" + txtFind.ClientID + "','" + panResult.ClientID + "');return false;");
                txtName.Focus();
                btnSearch.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a device name');");
                btnSubmit.Attributes.Add("onclick", "return ValidateHidden0('hdnAJAXValue','" + txtUser.ClientID + "','Please select the requestor / client') && ValidateText('" + txtReason.ClientID + "','Please enter a reason') && confirm('WARNING: You are about to alter production data! This action CANNOT be undone!\\n\\nAre you sure you want to continue?') && confirm('LAST CHANCE! If you do not know what you are doing, I suggest you do not do this!\\n\\nAre you REALLY sure you want to do this?');");
            }
            if (Request.QueryString["assets"] != null)
            {
                string[] strAssets = Request.QueryString["assets"].Split(strSplit);
                StringBuilder strDeploy = new StringBuilder();
                strDeploy.Append("<table cellpadding=\"3\" cellspacing=\"0\" border=\"0\" style=\"border:solid 1px #CCCCCC\">");
                strDeploy.Append("<tr bgcolor=\"#EEEEEE\"><td><b>Server Name</b></td><td><b>Serial Number</b></td><td><b>Power On Using</b></td><td><b>Backup?</b></td></tr>");
                int intRecommission = 0;
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
                        intRecommission++;
                    }
                }
                strDeploy.Append("</table>");
                lblResult.Text = "<p>ClearView Recommissioned " + intRecommission.ToString() + " device(s)...</p>";
                lblResult.Text += "<p>Please use the following information to configure the devices...</p>";
                lblResult.Text += strDeploy.ToString();
            }
        }
        protected void btnSearch_Click(Object Sender, EventArgs e)
        {
            DataSet ds = oAsset.GetDecommissionRecommission(txtName.Text);
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
                    if (lblStatus.Text == "2")
                        lblStatus.Text = "Assigned";
                    else if (lblStatus.Text == "1")
                        lblStatus.Text = "Running...";
                    else
                        lblStatus.Text = "<span class=\"waiting\">Awaiting<br/>Start Date</span>";
                    Label lblRecommissioned = (Label)ri.FindControl("lblRecommissioned");
                    if (lblRecommissioned.Text != "")
                    {
                        string strUser = "0";
                        if (lblRecommissioned.ToolTip.Contains("_") == true)
                        {
                            strUser = lblRecommissioned.ToolTip.Substring(0, lblRecommissioned.ToolTip.IndexOf("_"));
                            lblRecommissioned.ToolTip = lblRecommissioned.ToolTip.Substring(lblRecommissioned.ToolTip.IndexOf("_") + 1);
                        }
                        lblStatus.Text = "<span class=\"shelved\">Already<br/>Recommissioned</span>";
                        lblRecommissioned.Text = "<span class=\"redheader\">Already Recommissioned</span><br/><br/><span class=\"reddefault\"><b>On</b> : " + lblRecommissioned.Text + "<br/><b>By</b> : " + oUser.GetFullName(Int32.Parse(strUser)) + "<br/><b>Reason</b> : " + lblRecommissioned.ToolTip + "</span>";
                        chkDevice.Visible = false;
                    }
                    else
                    {
                        intCount++;
                        lblRecommissioned.Text = "";
                        Label lblDestroyed = (Label)ri.FindControl("lblDestroyed");
                        if (lblDestroyed.Text != "")
                        {
                            lblStatus.Text = "<span class=\"approved\">Completed</span>";
                            chkDevice.Attributes.Add("onclick", "UpdateCompleteCheck(this);");
                        }
                    }
                }
            }
            lblResult.Text = "There are " + intCount.ToString() + " asset(s) available for recommission";
            panResult.Visible = true;
            txtFind.Text = txtName.Text;
            btnFind.Enabled = true;
            btnSubmit.Enabled = (intCount > 0);
            txtFind.Focus();
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
                    Label lblStatus = (Label)ri.FindControl("lblStatus");
                    bool boolComplete = lblStatus.Text.ToUpper().Contains("COMPLETED");
                    oLog.AddEvent(lblName.Text, lblSerial.Text, "RECOMMISSION: Started by " + oUser.GetFullNameWithLanID(intProfile), LoggingType.Information);

                    if (chkDevice.Checked == true)
                    {
                        if (chkDevice.ToolTip[0].ToString() == "S")
                        {
                            int intServer = Int32.Parse(chkDevice.ToolTip.Substring(1));
                            DataSet dsServers = oServer.GetAssetsServer(intServer);
                            foreach (DataRow drServer in dsServers.Tables[0].Rows)
                            {
                                int intAsset = Int32.Parse(drServer["assetid"].ToString());
                                string strName = lblName.Text;
                                if (drServer["dr"].ToString() == "1")
                                    strName += "-DR";
                                // Update Recommission Reason
                                oAsset.UpdateDecommissionRecommission(intAsset, intUser, txtReason.Text);
                                // Set status to InUse
                                oAsset.AddStatus(intAsset, strName, (int)AssetStatus.InUse, intUser, DateTime.Now);
                                // Clear cv_servers_assets DECOM field
                                oServer.UpdateAssetDecom(intServer, intAsset, "");
                                if (boolComplete == true)
                                {
                                    DataSet dsOrders = oAssetOrder.GetByAsset(intAsset, false);
                                    foreach (DataRow drOrders in dsOrders.Tables[0].Rows)
                                    {
                                        int intOrder = Int32.Parse(drOrders["orderid"].ToString());
                                        // Cancel Resource Requests
                                        int intResource = 0;
                                        if (Int32.TryParse(drOrders["resourceid"].ToString(), out intResource) == true)
                                            oResourceRequest.UpdateStatusOverallWorkflow(intResource, (int)ResourceRequestStatus.Cancelled);
                                        // Delete Order
                                        oAssetOrder.DeleteOrder(intOrder);
                                        // Delete Asset Order Asset Selection
                                        oAssetOrder.DeleteAssetOrderAssetSelection(intOrder, intAsset);
                                    }
                                    // Set NewOrderID = 0
                                    oAsset.updateNewOrderId(0, intAsset);
                                }
                                // Set strAssets to assets recommissioned (for status message on postback)
                                if (strAssets != "")
                                    strAssets += strSplit[0].ToString();
                                strAssets += intAsset.ToString();
                            }
                            // Remove previous decom records
                            if (boolComplete == true)
                            {
                                bool boolPNC = (oServer.Get(intServer, "pnc") == "1");
                                // Update Server Name Record
                                int intName = Int32.Parse(oServer.Get(intServer, "nameid"));
                                if (boolPNC)
                                    oServerName.UpdateFactory(intName, 0);
                                else
                                    oServerName.Update(intName, 0);
                            }
                            // Clear cv_servers DECOM field
                            oServer.UpdateDecommissioned(intServer, "");
                            // Update IP Address(es) availability
                            DataSet dsIP = oServer.GetIP(intServer, 0, 0, 0, 0);
                            foreach (DataRow drIP in dsIP.Tables[0].Rows)
                            {
                                int intIP = Int32.Parse(drIP["ipaddressid"].ToString());
                                oIPAddresses.UpdateAvailable(intIP, 0);
                            }
                            // Restore Avamar Group(s)
                            if (chkAvamar.Checked)
                            {
                                Avamar oAvamar = new Avamar(0, dsn);
                                AvamarRegistration oAvamarRegistration = new AvamarRegistration(0, dsn);
                                ClearViewWebServices oWebService = new ClearViewWebServices();
                                System.Net.NetworkCredential oCredentialsDNS = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                                oWebService.Credentials = oCredentialsDNS;
                                oWebService.Url = oVariable.WebServiceURL();
                                string strError = "";

                                // First, query for groups.
                                DataSet dsGroups = oAvamar.GetDecoms(lblName.Text);
                                if (dsGroups.Tables[0].Rows.Count > 0)
                                {
                                    string client = dsGroups.Tables[0].Rows[0]["client"].ToString();
                                    string grid = dsGroups.Tables[0].Rows[0]["grid"].ToString();
                                    string domain = dsGroups.Tables[0].Rows[0]["domain"].ToString();
                                    // Second, add the groups.
                                    foreach (DataRow drGroup in dsGroups.Tables[0].Rows)
                                    {
                                        if (String.IsNullOrEmpty(strError) == false)
                                            break;
                                        AvamarReturnType restore = oAvamarRegistration.API(oWebService.AddAvamarGroup(grid, domain, client, drGroup["group"].ToString()));
                                        if (restore.Error == true)
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
                                if (String.IsNullOrEmpty(strError))
                                    oLog.AddEvent(lblName.Text, lblSerial.Text, "RECOMMISSION: Avamar completed.", LoggingType.Information);
                                else
                                    oLog.AddEvent(lblName.Text, lblSerial.Text, "RECOMMISSION: Avamar encountered an error = " + strError, LoggingType.Error);
                            }
                            // Add log entry
                            oLog.AddEvent(lblName.Text, lblSerial.Text, "Asset Recommissioned (Client = " + oUser.GetFullName(intUser) + ")", LoggingType.Information);
                        }
                        else
                        {
                            // Manual recommission - just delete the resource request to get out of the person's queue
                            int intResource = Int32.Parse(chkDevice.ToolTip.Substring(1));
                            oResourceRequest.UpdateStatusOverall(intResource, -2);
                        }
                    }
                }
            }
            Response.Redirect(Request.Path + "?assets=" + strAssets);
        }
    }
}
