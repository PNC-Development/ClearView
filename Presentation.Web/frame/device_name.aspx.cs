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
using System.DirectoryServices;

namespace NCC.ClearView.Presentation.Web
{
    public partial class device_name : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected int intAsset;
        protected Workstations oWorkstation;
        protected Servers oServer;
        protected ServerName oServerName;
        protected Asset oAsset;
        private int intStatus = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Int32.TryParse(Request.Cookies["profileid"].Value, out intProfile);
            oServer = new Servers(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
            oWorkstation = new Workstations(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset, dsn);

            Int32.TryParse(Request.QueryString["assetid"], out intAsset);

            bool boolMessage = false;
            if (Request.QueryString["changed"] != null)
            {
                boolMessage = true;
                Page.ClientScript.RegisterStartupScript(typeof(Page), "changed", "<script type=\"text/javascript\">alert('Device Name Changed Successfully!');RefreshOpeningWindow();window.close();<" + "/" + "script>");
            }
            if (Request.QueryString["cleared"] != null)
            {
                boolMessage = true;
                Page.ClientScript.RegisterStartupScript(typeof(Page), "cleared", "<script type=\"text/javascript\">alert('Device Name Cleared Successfully!');RefreshOpeningWindow();window.close();<" + "/" + "script>");
            }
            if (Request.QueryString["error"] != null)
            {
                boolMessage = true;
                Page.ClientScript.RegisterStartupScript(typeof(Page), "bad", "<script type=\"text/javascript\">alert('There was a problem changing the device name...\\n\\n" + Request.QueryString["error"] + "');<" + "/" + "script>");
            }

            if (Int32.TryParse(oAsset.GetStatus(intAsset, "status"), out intStatus) == true)
            {
                if (!IsPostBack)
                {
                    string strAdditional = "";
                    if (intAsset > 0 && Int32.TryParse(oAsset.GetStatus(intAsset, "status"), out intStatus) == true)
                    {
                        if (Request.QueryString["clear"] != null)
                        {
                            if (Request.QueryString["clear"] == "Clear")
                            {
                                radClear.SelectedValue = "Clear";
                                if (intStatus == (int)AssetStatus.InUse)
                                    lblError.Text = "Can not clear the name of an asset while its Status is &quot;In Use&quot;.";
                                panClear.Visible = true;
                                panID.Visible = true;
                            }
                            if (Request.QueryString["clear"] == "Change")
                            {
                                radClear.SelectedValue = "Change";
                                panChange.Visible = true;
                                panID.Visible = true;
                            }
                        }
                        DataSet dsWorkstations = oWorkstation.GetVirtualAsset(intAsset);
                        if (dsWorkstations.Tables[0].Rows.Count == 1)
                        {
                            radClear.Items[0].Enabled = false;
                            radClear.Items[0].Text += " - cannot clear device name for workstations";

                            ddlType.SelectedValue = "-1";
                            int intWorkstation = 0;
                            int intName = 0;
                            if (Int32.TryParse(dsWorkstations.Tables[0].Rows[0]["id"].ToString(), out intWorkstation) == true)
                            {
                                lblID.Text = intWorkstation.ToString();
                                if (Int32.TryParse(oWorkstation.GetVirtual(intWorkstation, "nameid"), out intName) == true)
                                {
                                    lblNameID.Text = intName.ToString();
                                    lblName.Text = oWorkstation.GetName(intName);
                                    lblNew.Text = lblName.Text;
                                    panWorkstation.Visible = true;

                                    DataSet dsWorkstation = oWorkstation.GetNameId(intName);
                                    if (dsWorkstation.Tables[0].Rows.Count == 1)
                                    {
                                        DataRow drWorkstation = dsWorkstation.Tables[0].Rows[0];

                                        #region WORKSTATION NAMING
                                        // Environment
                                        ddlWorkstationEnvironment.Attributes.Add("onchange", "UpdateNamingDDL(this,'" + lblWorkstationEnvironment.ClientID + "','WORKSTATION');");
                                        lblWorkstationEnvironment.Text = drWorkstation["environment"].ToString();
                                        ddlWorkstationEnvironment.SelectedValue = lblWorkstationEnvironment.Text;

                                        // Code
                                        ddlWorkstationCode.Attributes.Add("onchange", "UpdateNamingDDL(this,'" + lblWorkstationCode.ClientID + "','WORKSTATION');");
                                        lblWorkstationCode.Text = drWorkstation["code"].ToString();
                                        ddlWorkstationCode.SelectedValue = lblWorkstationCode.Text;

                                        //Sequence
                                        txtWorkstationSequence.Attributes.Add("onblur", "UpdateNamingText(this,'" + lblWorkstationSequence.ClientID + "','WORKSTATION');");
                                        lblWorkstationSequence.Text = drWorkstation["prefix1"].ToString() + drWorkstation["prefix2"].ToString() + drWorkstation["prefix3"].ToString() + drWorkstation["prefix4"].ToString() + drWorkstation["prefix5"].ToString() + drWorkstation["prefix6"].ToString();
                                        txtWorkstationSequence.Text = lblWorkstationSequence.Text;

                                        panButtons.Visible = (Request.QueryString["clear"] != null);
                                        lblNew.Text = lblWorkstationEnvironment.Text + lblWorkstationCode.Text + lblWorkstationIdentifier.Text + lblWorkstationSequence.Text;
                                        strAdditional += "ValidateTextLength('" + txtWorkstationSequence.ClientID + "', 'Please enter a valid sequence\\n\\n - Must be exactly 6 characters in length', 6) && ";

                                        #endregion
                                    }
                                }
                                else
                                    lblError.Text = "Invalid Workstation Name Record(" + oWorkstation.GetVirtual(intWorkstation, "nameid") + ")";
                            }
                            else
                                lblError.Text = "Invalid Workstation Record(" + dsWorkstations.Tables[0].Rows[0]["id"].ToString() + ")";
                        }
                        else
                        {
                            DataSet dsServers = oServer.GetAssetsAsset(intAsset);
                            if (dsServers.Tables[0].Rows.Count == 1)
                            {
                                int intServer = 0;
                                int intName = 0;
                                if (Int32.TryParse(dsServers.Tables[0].Rows[0]["serverid"].ToString(), out intServer) == true)
                                {
                                    lblID.Text = intServer.ToString();
                                    if (Int32.TryParse(oServer.Get(intServer, "nameid"), out intName) == true)
                                    {
                                        lblNameID.Text = intName.ToString();
                                        lblName.Text = oServer.GetName(intServer, true);
                                        if (oServer.Get(intServer, "pnc") == "1")
                                        {
                                            ddlType.SelectedValue = "1";
                                            panPNC.Visible = true;
                                            DataSet dsPNC = oServerName.GetFactory(intName);
                                            if (dsPNC.Tables[0].Rows.Count == 1 || intName == 0)
                                            {
                                                //Operating System
                                                OperatingSystems oOperatingSystem = new OperatingSystems(intProfile, dsn);
                                                DataSet dsOS = oOperatingSystem.Gets(0, 1);
                                                DataView dvOS = dsOS.Tables[0].DefaultView;
                                                dvOS.RowFilter = "factory_code <> '' AND factory_code IS NOT NULL";
                                                ddlPNCOS.DataTextField = "name";
                                                ddlPNCOS.DataValueField = "factory_code";
                                                ddlPNCOS.DataSource = dvOS;
                                                ddlPNCOS.DataBind();
                                                ddlPNCOS.Attributes.Add("onchange", "UpdateNamingDDL(this,'" + lblPNCOS.ClientID + "','PNC');");

                                                //Location
                                                Locations oLocation = new Locations(intProfile, dsn);
                                                DataSet dsLocation = oLocation.GetAddresss(1);
                                                DataView dvLocation = dsLocation.Tables[0].DefaultView;
                                                dvLocation.RowFilter = "factory_code <> 'X' AND factory_code <> '' AND factory_code IS NOT NULL";
                                                ddlPNCLocation.DataTextField = "commonname";
                                                ddlPNCLocation.DataValueField = "factory_code";
                                                ddlPNCLocation.DataSource = dvLocation;
                                                ddlPNCLocation.DataBind();
                                                ddlPNCLocation.Items.Add(new ListItem("Other", "X"));
                                                ddlPNCLocation.Attributes.Add("onchange", "UpdateNamingDDL(this,'" + lblPNCLocation.ClientID + "','PNC');");

                                                //Mnemonic
                                                Variables oVariable = new Variables(intEnvironment);
                                                Mnemonic oMnemonic = new Mnemonic(intProfile, dsn);
                                                txtPNCMnemonic.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'350','150','" + divPNCMnemonic.ClientID + "','" + lstPNCMnemonic.ClientID + "','" + hdnPNCMnemonic.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_mnemonics.aspx',2);");
                                                lstPNCMnemonic.Attributes.Add("ondblclick", "AJAXClickRow();");
                                                hdnPNCMnemonic.Attributes.Add("onpropertychange", "UpdateNamingMnemonic('" + hdnPNCMnemonic.ClientID + "','" + txtPNCMnemonic.ClientID + "','" + lblPNCMnemonic.ClientID + "','PNC');");

                                                //Environment
                                                Classes oClass = new Classes(intProfile, dsn);
                                                DataSet dsClass = oClass.Gets(1);
                                                DataView dvClass = dsClass.Tables[0].DefaultView;
                                                dvClass.RowFilter = "factory_code <> '' AND factory_code IS NOT NULL";
                                                ddlPNCEnvironment.DataTextField = "name";
                                                ddlPNCEnvironment.DataValueField = "factory_code";
                                                ddlPNCEnvironment.DataSource = dvClass;
                                                ddlPNCEnvironment.DataBind();
                                                ddlPNCEnvironment.Attributes.Add("onchange", "UpdateNamingDDL(this,'" + lblPNCEnvironment.ClientID + "','PNC');");

                                                //Sequence
                                                txtPNCSequence.Attributes.Add("onblur", "UpdateNamingText(this,'" + lblPNCSequence.ClientID + "','PNC');");

                                                //Function
                                                ddlPNCFunction.Attributes.Add("onchange", "UpdateNamingDDL(this,'" + lblPNCFunction.ClientID + "','PNC');");

                                                //Specific
                                                ddlPNCSpecific.Attributes.Add("onchange", "UpdateNamingDDL(this,'" + lblPNCSpecific.ClientID + "','PNC');");
                                                
                                                #region PNC NAMING
                                                if (intName > 0)
                                                {
                                                    DataRow drPNC = dsPNC.Tables[0].Rows[0];
                                                    //Operating System
                                                    lblPNCOS.Text = drPNC["os"].ToString();
                                                    ddlPNCOS.SelectedValue = lblPNCOS.Text;
                                                    //Location
                                                    lblPNCLocation.Text = drPNC["location"].ToString();
                                                    ddlPNCLocation.SelectedValue = lblPNCLocation.Text;
                                                    //Mnemonic
                                                    lblPNCMnemonic.Text = drPNC["mnemonic"].ToString();
                                                    DataSet dsMnemonic = oMnemonic.Get(lblPNCMnemonic.Text + " - ");
                                                    int intMnemonic = Int32.Parse(dsMnemonic.Tables[0].Rows[0]["id"].ToString());
                                                    hdnPNCMnemonic.Value = intMnemonic.ToString();
                                                    txtPNCMnemonic.Text = lblPNCMnemonic.Text + " - " + oMnemonic.Get(intMnemonic, "name");
                                                    //Environment
                                                    lblPNCEnvironment.Text = drPNC["environment"].ToString();
                                                    ddlPNCEnvironment.SelectedValue = lblPNCEnvironment.Text;
                                                    //Sequence
                                                    lblPNCSequence.Text = drPNC["name1"].ToString() + drPNC["name2"].ToString();
                                                    txtPNCSequence.Text = lblPNCSequence.Text;
                                                    //Function
                                                    lblPNCFunction.Text = drPNC["func"].ToString();
                                                    ddlPNCFunction.SelectedValue = lblPNCFunction.Text;
                                                    //Specific
                                                    lblPNCSpecific.Text = drPNC["specific"].ToString();
                                                    ddlPNCSpecific.SelectedValue = lblPNCSpecific.Text;
                                                }
                                                else
                                                {
                                                    //Operating System
                                                    ddlPNCOS.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                                    strAdditional += "ValidateDropDown('" + ddlPNCOS.ClientID + "','Please select an Operating System') && ";
                                                    //Location
                                                    ddlPNCLocation.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                                    strAdditional += "ValidateDropDown('" + ddlPNCLocation.ClientID + "','Please select a Location') && ";
                                                    //Mnemonic
                                                    strAdditional += "ValidateHidden0('" + hdnPNCMnemonic.ClientID + "','" + txtPNCMnemonic.ClientID + "','Please enter the mnemonic') && ";
                                                    //Environment
                                                    ddlPNCEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                                    strAdditional += "ValidateDropDown('" + ddlPNCEnvironment.ClientID + "','Please select an Environment') && ";
                                                    //Sequence - Done down below
                                                    //Function
                                                    ddlPNCFunction.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                                    strAdditional += "ValidateDropDown('" + ddlPNCFunction.ClientID + "','Please select a Function') && ";
                                                    //Specific
                                                    ddlPNCSpecific.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                                    strAdditional += "ValidateDropDown('" + ddlPNCSpecific.ClientID + "','Please select an Option') && ";
                                                }

                                                panButtons.Visible = (Request.QueryString["clear"] != null);
                                                lblNew.Text = lblPNCOS.Text + lblPNCLocation.Text + lblPNCMnemonic.Text + lblPNCEnvironment.Text + lblPNCSequence.Text + lblPNCFunction.Text + lblPNCSpecific.Text;
                                                strAdditional += "ValidateTextLength('" + txtPNCSequence.ClientID + "', 'Please enter a valid sequence\\n\\n - Must be exactly 2 characters in length', 2) && ";

                                                #endregion
                                            }
                                            else
                                                lblError.Text = "PNC Names (" + dsPNC.Tables[0].Rows.Count.ToString() + ")";
                                        }
                                        else
                                        {
                                            ddlType.SelectedValue = "0";
                                            panNCB.Visible = true;
                                            DataSet dsNCB = oServerName.Get(intName);
                                            if (dsNCB.Tables[0].Rows.Count == 1 || intName == 0)
                                            {
                                                //State
                                                txtNCBState.Attributes.Add("onblur", "UpdateNamingText(this,'" + lblNCBState.ClientID + "','NCB');");

                                                //City
                                                txtNCBCity.Attributes.Add("onblur", "UpdateNamingText(this,'" + lblNCBCity.ClientID + "','NCB');");

                                                //Function
                                                DataSet dsFunction = oServerName.GetFunctions();
                                                DataView dvFunction = dsFunction.Tables[0].DefaultView;
                                                dvFunction.RowFilter = "code <> '' AND code IS NOT NULL";
                                                ddlNCBFunction.DataTextField = "name";
                                                ddlNCBFunction.DataValueField = "code";
                                                ddlNCBFunction.DataSource = dvFunction;
                                                ddlNCBFunction.DataBind();
                                                ddlNCBFunction.Attributes.Add("onchange", "UpdateNamingDDL(this,'" + lblNCBFunction.ClientID + "','NCB');");

                                                //SiteCode
                                                ddlNCBSiteCode.Attributes.Add("onchange", "UpdateNamingDDL(this,'" + lblNCBSiteCode.ClientID + "','NCB');");

                                                //Sequence
                                                txtNCBSequence.Attributes.Add("onblur", "UpdateNamingText(this,'" + lblNCBSequence.ClientID + "','NCB');");

                                                #region NCB NAMING
                                                if (intName > 0)
                                                {
                                                    DataRow drNCB = dsNCB.Tables[0].Rows[0];
                                                    string strPrefix = drNCB["prefix1"].ToString();
                                                    string strPrefix1 = strPrefix.Substring(0, 2);
                                                    string strPrefix2 = strPrefix.Substring(2, 3);
                                                    //State
                                                    lblNCBState.Text = strPrefix1;
                                                    txtNCBState.Text = lblNCBState.Text;
                                                    //City
                                                    lblNCBCity.Text = strPrefix2;
                                                    txtNCBCity.Text = lblNCBCity.Text;
                                                    //Function
                                                    lblNCBFunction.Text = drNCB["prefix2"].ToString();
                                                    ddlNCBFunction.SelectedValue = lblNCBFunction.Text;
                                                    //SiteCode
                                                    lblNCBSiteCode.Text = drNCB["sitecode"].ToString();
                                                    ddlNCBSiteCode.SelectedValue = lblNCBSiteCode.Text;
                                                    //Sequence
                                                    lblNCBSequence.Text = drNCB["name1"].ToString() + drNCB["name2"].ToString();
                                                    txtNCBSequence.Text = lblNCBSequence.Text;
                                                }
                                                else
                                                {
                                                    //State - Done down below
                                                    //City - Done down below
                                                    //Function
                                                    ddlNCBFunction.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                                    strAdditional += "ValidateDropDown('" + ddlNCBFunction.ClientID + "','Please select a Function') && ";
                                                    //SiteCode
                                                    ddlNCBSiteCode.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                                    strAdditional += "ValidateDropDown('" + ddlNCBSiteCode.ClientID + "','Please select a Site Code') && ";
                                                    //Sequence - Done down below
                                                }

                                                panButtons.Visible = (Request.QueryString["clear"] != null);
                                                lblNew.Text = lblNCBState.Text + lblNCBCity.Text + lblNCBFunction.Text + lblNCBSiteCode.Text + lblNCBSequence.Text;
                                                strAdditional += "ValidateTextLength('" + txtNCBState.ClientID + "', 'Please enter a valid state\\n\\n - Must be exactly 2 characters in length', 2) && ";
                                                strAdditional += "ValidateTextLength('" + txtNCBCity.ClientID + "', 'Please enter a valid city\\n\\n - Must be exactly 3 characters in length', 3) && ";
                                                strAdditional += "ValidateTextLength('" + txtNCBSequence.ClientID + "', 'Please enter a valid sequence\\n\\n - Must be exactly 2 characters in length', 2) && ";

                                                #endregion
                                            }
                                            else
                                                lblError.Text = "NCB Names (" + dsNCB.Tables[0].Rows.Count.ToString() + ")";
                                        }
                                    }
                                    else
                                        lblError.Text = "Invalid Server Name Record(" + oServer.Get(intServer, "nameid") + ")";
                                }
                                else
                                    lblError.Text = "Invalid Server Record(" + dsServers.Tables[0].Rows[0]["serverid"].ToString() + ")";
                            }
                            else
                            {
                                if (dsWorkstations.Tables[0].Rows.Count > 1)
                                    lblError.Text = "Workstations (" + dsWorkstations.Tables[0].Rows.Count.ToString() + ")";
                                else if (dsServers.Tables[0].Rows.Count > 1)
                                    lblError.Text = "Servers (" + dsServers.Tables[0].Rows.Count.ToString() + ")";
                                else
                                {
                                    radClear.Items[1].Enabled = false;
                                    panButtons.Visible = (Request.QueryString["clear"] != null);
                                    //lblError.Text = "Asset Not Found";
                                }
                            }
                        }
                        if (lblName.Text == "")
                            lblName.Text = "---";
                        else
                            lblNew.CssClass = (lblNew.Text == lblName.Text ? "header" : "redheader");
                        if (panChange.Visible == true)
                            btnSubmit.Attributes.Add("onclick", "return " + strAdditional + " IsOKtoChange() && confirm('WARNING: This will permanently change the device name and release the previous name for re-use (if applicable)!\\n\\nAre you sure you want to continue?') && ProcessButton(this,'Changing...','100');");
                        if (panClear.Visible == true)
                            btnSubmit.Attributes.Add("onclick", "return confirm('WARNING: This will permanently clear the device name and release it for re-use (if applicable)!\\n\\nAre you sure you want to continue?') && ProcessButton(this,'Clearing...','100');");
                        btnReset.Attributes.Add("onclick", "return IsOKtoReset(this);");
                        btnAlreadyChange.Attributes.Add("onclick", "return confirm('LAST CHANCE! This will permanently change the device name and release the previous name for re-use (if applicable)!\\n\\nAre you sure you want to continue?') && ProcessButton(this,'Changing...','100');");
                    }
                    else
                        lblError.Text = "Either the asset or the status of the asset could not be identified";
                }
            }
            else
                lblError.Text = "Either the asset or the status of the asset could not be identified";
            if (lblError.Text != "" && boolMessage == false)
            {
                panError.Visible = true;
                panClear.Visible = false;
                btnSubmit.Enabled = false;
            }
        }
        protected void radClear_Change(Object Sender, EventArgs e)
        {
            LoadChange();
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            if (radClear.SelectedItem.Value == "Clear")
            {
                if (lblNameID.Text != "")
                {
                    // If lblNameID.Text == "", then clear only the asset name, else clear workstation or server info...
                    int intNameID = Int32.Parse(lblNameID.Text);
                    int intID = Int32.Parse(lblID.Text);
                    // Clear Name
                    switch (ddlType.SelectedValue)
                    {
                        case "-1":
                            // Workstation
                            // Set old name to available
                            oWorkstation.UpdateName(intNameID, 1);
                            // Clear workstation record
                            oWorkstation.UpdateVirtualName(intID, 0);
                            break;
                        case "0":
                            // Server (National City)
                            // Set old name to available
                            oServerName.Update(intNameID, 1);
                            // Clear server record
                            oServer.UpdateServerNamed(intID, 0);
                            break;
                        case "1":
                            // Server (PNC Financial Services) 
                            // Set old name to available
                            oServerName.UpdateFactory(intNameID, 1);
                            // Clear server record
                            oServer.UpdateServerNamed(intID, 0);
                            break;
                    }
                }
                // Update asset
                oAsset.UpdateStatus(intAsset, "", intStatus, intProfile, DateTime.Now);
                // Redirect
                Response.Redirect(Request.Path + "?assetid=" + intAsset.ToString() + "&clear=" + radClear.SelectedItem.Value + "&cleared=true");
            }
            if (radClear.SelectedItem.Value == "Change")
            {
                // Change Name
                MakeChange(false);
            }
        }
        protected void btnAlreadyChange_Click(Object Sender, EventArgs e)
        {
            MakeChange(true);
        }
        protected void btnAlreadyCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?assetid=" + intAsset.ToString() + "&clear=" + radClear.SelectedItem.Value);
        }
        protected void btnReset_Click(Object Sender, EventArgs e)
        {
            LoadChange();
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?assetid=" + intAsset.ToString());
        }
        protected void LoadChange()
        {
            Response.Redirect(Request.Path + "?assetid=" + intAsset.ToString() + "&clear=" + radClear.SelectedItem.Value);
        }
        protected void MakeChange(bool _override)
        {
            // Check to see if new name already exists
            int intNameID = Int32.Parse(lblNameID.Text);
            int intID = Int32.Parse(lblID.Text);
            bool boolExists = false;
            string strName = Request.Form[hdnName.UniqueID];
            string strError = "";

            switch (ddlType.SelectedValue)
            {
                case "-1":
                    // Workstation
                    DataSet dsWorkstation = oWorkstation.GetName(strName);
                    if (dsWorkstation.Tables[0].Rows.Count > 0 && dsWorkstation.Tables[0].Rows[0]["available"].ToString() == "0" && _override == false)
                        boolExists = true;
                    else 
                    {
                        // Does not exist or is otherwise available for use - update
                        boolExists = false;
                        // Set old name to available
                        oWorkstation.UpdateName(intNameID, 1);
                        if (dsWorkstation.Tables[0].Rows.Count > 0)
                        {
                            // If name already exists (available is marked or _override = true) then use that
                            intNameID = Int32.Parse(dsWorkstation.Tables[0].Rows[0]["id"].ToString());
                        }
                        else
                        {
                            // Create the name
                            string strSequence = txtWorkstationSequence.Text;
                            intNameID = oWorkstation.AddName(ddlWorkstationEnvironment.SelectedItem.Value, ddlWorkstationCode.SelectedItem.Value, lblWorkstationIdentifier.Text, strSequence.Substring(0, 1), strSequence.Substring(1, 1), strSequence.Substring(2, 1), strSequence.Substring(3, 1), strSequence.Substring(4, 1), strSequence.Substring(5, 1), 0);
                        }
                        // Set new name to unavailable
                        oWorkstation.UpdateName(intNameID, 0);
                        // Update workstation record
                        oWorkstation.UpdateVirtualName(intID, intNameID);
                    }
                    break;
                case "0":
                    // Server (National City)
                    int intNameNCB = oServerName.GetName(strName);
                    DataSet dsNCB = oServerName.Get(intNameNCB);
                    if (dsNCB.Tables[0].Rows.Count > 0 && dsNCB.Tables[0].Rows[0]["available"].ToString() == "0" && _override == false)
                        boolExists = true;
                    else
                    {
                        // Does not exist or is otherwise available for use - update
                        boolExists = false;
                        // Set old name to available
                        oServerName.Update(intNameID, 1);
                        if (dsNCB.Tables[0].Rows.Count > 0)
                        {
                            // If name already exists (available is marked or _override = true) then use that
                            intNameID = intNameNCB;
                        }
                        else
                        {
                            // Create the name
                            string strSequence = txtNCBSequence.Text;
                            intNameID = oServerName.Add(0, txtNCBState.Text + txtNCBCity.Text, ddlNCBFunction.SelectedItem.Value, ddlNCBSiteCode.SelectedItem.Value, strSequence.Substring(0, 1), strSequence.Substring(1, 1), intProfile, "CHANGE", 0);
                        }
                        // Set new name to unavailable
                        oServerName.Update(intNameID, 0);
                        // Update server record
                        oServer.UpdateServerNamed(intID, intNameID);
                    }
                    break;
                case "1":
                    // Server (PNC Financial Services) 
                    int intNamePNC = oServerName.GetNameFactory(strName);
                    DataSet dsPNC = oServerName.GetFactory(intNamePNC);
                    if (dsPNC.Tables[0].Rows.Count > 0 && dsPNC.Tables[0].Rows[0]["available"].ToString() == "0" && _override == false)
                        boolExists = true;
                    else
                    {
                        // Does not exist or is otherwise available for use - update
                        boolExists = false;
                        // Set old name to available
                        oServerName.UpdateFactory(intNameID, 1);
                        if (dsPNC.Tables[0].Rows.Count > 0)
                        {
                            // If name already exists (available is marked or _override = true) then use that
                            intNameID = intNamePNC;
                        }
                        else
                        {
                            // Create the name
                            string strSequence = txtPNCSequence.Text;
                            intNameID = oServerName.AddFactory(ddlPNCOS.SelectedItem.Value, ddlPNCLocation.SelectedItem.Value, Request.Form["hdnPNCMnemonic"], ddlPNCEnvironment.SelectedItem.Value, strSequence.Substring(0, 1), strSequence.Substring(1, 1), ddlPNCFunction.SelectedItem.Value, ddlPNCSpecific.SelectedItem.Value, intProfile, "CHANGE", 0);
                        }
                        // Set new name to unavailable
                        oServerName.UpdateFactory(intNameID, 0);
                        // Update server record
                        oServer.UpdateServerNamed(intID, intNameID);
                    }
                    break;
            }

            if (boolExists == true)
            {
                panExists.Visible = true;
                lblAlready.Text = strName;
                panChange.Visible = false;
                panButtons.Visible = false;
                btnAlreadyCancel.Focus();
            }
            else
            {
                if (strError == "")
                {
                    // Update asset
                    oAsset.AddStatus(intAsset, strName, intStatus, intProfile, DateTime.Now);
                    // Redirect
                    Response.Redirect(Request.Path + "?assetid=" + intAsset.ToString() + "&clear=" + radClear.SelectedItem.Value + "&changed=true");
                }
                else
                {
                    // Build Error Message
                    Response.Redirect(Request.Path + "?assetid=" + intAsset.ToString() + "&clear=" + radClear.SelectedItem.Value + "&error=" + strError);
                }
            }
        }
    }
}
