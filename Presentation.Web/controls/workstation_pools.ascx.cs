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
using NCC.ClearView.Presentation.Web.Custom;
using System.DirectoryServices;
namespace NCC.ClearView.Presentation.Web
{
    public partial class workstation_pools : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Functions oFunction;
        protected Variables oVariable;
        protected Workstations oWorkstation;
        protected Users oUser;
        protected Requests oRequest;
        protected ServiceRequests oServiceRequest;
        protected Locations oLocation;
        protected Forecast oForecast;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected OperatingSystems oOperatingSystems;
        protected VirtualHDD oVirtualHDD;
        protected VirtualRam oVirtualRam;
        protected VirtualCPU oVirtualCPU;
        protected Classes oClass;
        protected AD oAD;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected int intConfidence = Int32.Parse(ConfigurationManager.AppSettings["CONFIDENCE_100"]);
        protected int intModelVirtual = Int32.Parse(ConfigurationManager.AppSettings["VirtualWorkstationModelID"]);
        protected int intModelVMware = Int32.Parse(ConfigurationManager.AppSettings["VMwareWorkstationModelID"]);
        protected int intCore = Int32.Parse(ConfigurationManager.AppSettings["CoreEnvironmentID"]);
        protected int intMaxWorkstationsPerDay = Int32.Parse(ConfigurationManager.AppSettings["VirtualWorkstationsMax"]);
        protected string strSubscribers = "";
        protected string strMenuTab1 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oVariable = new Variables(intEnvironment);
            oWorkstation = new Workstations(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oOperatingSystems = new OperatingSystems(intProfile, dsn);
            oVirtualHDD = new VirtualHDD(intProfile, dsn);
            oVirtualRam = new VirtualRam(intProfile, dsn);
            oVirtualCPU = new VirtualCPU(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oAD = new AD(intProfile, dsn, intEnvironment);

            //Menus
            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
            Tab oTab = new Tab("", intMenuTab, "divMenu1", true, false);

            oTab.AddTab("Pool Configurtion", "");
            oTab.AddTab("Workstation History", "");
            oTab.AddTab("Workstations Currently Available", "");
            oTab.AddTab("Subscribed Users", "");
            strMenuTab1 = oTab.GetTabs();

            //End Menus

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                panPool.Visible = true;
                int intID = Int32.Parse(Request.QueryString["id"]);
                if (Request.QueryString["save"] != null)
                    panSave.Visible = true;
                DataSet ds = oWorkstation.GetPool(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string strName = ds.Tables[0].Rows[0]["name"].ToString();
                    txtName.Text = strName;
                    rptHistory.DataSource = oWorkstation.GetPoolWorkstationsStatus(strName);
                    rptHistory.DataBind();
                    lblHistory.Visible = (rptHistory.Items.Count == 0);
                    rptAvailable.DataSource = oWorkstation.GetPoolWorkstations(strName);
                    rptAvailable.DataBind();
                    lblAvailable.Visible = (rptAvailable.Items.Count == 0);
                    DirectoryEntry oEntry = oAD.GroupSearch("GSGwra_" + strName);
                    if (oEntry != null)
                    {
                        if (oEntry.Properties.Contains("member") == true)
                        {
                            foreach (string strUser in oEntry.Properties["member"])
                            {
                                DirectoryEntry oEntry2 = new DirectoryEntry("LDAP://" + oVariable.primaryDCName(dsn) + "/" + strUser, oVariable.Domain() + "\\" + oVariable.ADUser(), oVariable.ADPassword());
                                strSubscribers += "<tr><td>" + oEntry2.Properties["displayname"].Value.ToString() + " (" + oEntry2.Properties["name"].Value.ToString() + ")</td></tr>";
                            }
                        }
                        else
                            strSubscribers += "<tr><td><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> There are no subscribers</td></tr>";
                    }
                    else
                        strSubscribers += "<tr><td><img src=\"/images/bigError.gif\" border=\"0\" align=\"absmiddle\"/> Could not find Active Directory Group <b>" + "GSGwra_" + strName + "</b></td></tr>";
                    txtDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                    int intContact1 = 0;
                    int intContact2 = 0;
                    if (ds.Tables[0].Rows[0]["contact1"].ToString() != "")
                        intContact1 = Int32.Parse(ds.Tables[0].Rows[0]["contact1"].ToString());
                    if (ds.Tables[0].Rows[0]["contact2"].ToString() != "")
                        intContact2 = Int32.Parse(ds.Tables[0].Rows[0]["contact2"].ToString());
                    if (intContact1 > 0)
                    {
                        txtContact1.Text = oUser.GetFullName(intContact1) + " (" + oUser.GetName(intContact1) + ")";
                        hdnContact1.Value = intContact1.ToString();
                    }
                    if (intContact2 > 0)
                    {
                        txtContact2.Text = oUser.GetFullName(intContact2) + " (" + oUser.GetName(intContact2) + ")";
                        hdnContact2.Value = intContact2.ToString();
                    }
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                    lstCurrent.DataValueField = "id";
                    lstCurrent.DataTextField = "name";
                    lstCurrent.DataSource = oWorkstation.GetPoolWorkstations(intID);
                    lstCurrent.DataBind();
                    lstAvailable.DataValueField = "id";
                    lstAvailable.DataTextField = "name";
                    lstAvailable.DataSource = oWorkstation.GetPoolWorkstations(intProfile, intID);
                    lstAvailable.DataBind();
                    string strWorkstations = "";
                    int intCount = 0;
                    foreach (ListItem oItem in lstCurrent.Items)
                    {
                        strWorkstations = strWorkstations + oItem.Value + "_" + intCount.ToString() + "&";
                        intCount++;
                    }
                    hdnWorkstations.Value = strWorkstations;
                    txtContact1.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divContact1.ClientID + "','" + lstContact1.ClientID + "','" + hdnContact1.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                    lstContact1.Attributes.Add("ondblclick", "AJAXClickRow();");
                    txtContact2.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divContact2.ClientID + "','" + lstContact2.ClientID + "','" + hdnContact2.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                    lstContact2.Attributes.Add("ondblclick", "AJAXClickRow();");
                    btnAdd.Attributes.Add("onclick", "return MoveList('" + lstAvailable.ClientID + "','" + lstCurrent.ClientID + "','" + hdnWorkstations.ClientID + "','" + lstCurrent.ClientID + "');");
                    lstAvailable.Attributes.Add("ondblclick", "return MoveList('" + lstAvailable.ClientID + "','" + lstCurrent.ClientID + "','" + hdnWorkstations.ClientID + "','" + lstCurrent.ClientID + "');");
                    btnRemove.Attributes.Add("onclick", "return MoveList('" + lstCurrent.ClientID + "','" + lstAvailable.ClientID + "','" + hdnWorkstations.ClientID + "','" + lstCurrent.ClientID + "');");
                    lstCurrent.Attributes.Add("ondblclick", "return MoveList('" + lstCurrent.ClientID + "','" + lstAvailable.ClientID + "','" + hdnWorkstations.ClientID + "','" + lstCurrent.ClientID + "');");
                    btnUpdate.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a name')" +
                        " && ValidateText('" + txtDescription.ClientID + "','Please enter a description')" +
                        " && ValidateHidden0('" + hdnContact1.ClientID + "','" + txtContact1.ClientID + "','Please enter a primary contact')" +
                        " && ValidateHidden0('" + hdnContact2.ClientID + "','" + txtContact2.ClientID + "','Please enter a secondary contact')" +
                        ";");
                }
            }
            else if (Request.QueryString["create"] != null)
            {
                panCreate.Visible = true;
                LoadLists();
                if (Request.QueryString["qty"] != null && Request.QueryString["qty"] != "")
                {
                    if (Request.QueryString["qty"] == intMaxWorkstationsPerDay.ToString())
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "qty", "<script type=\"text/javascript\">alert('NOTE: You can request up to " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day.\\n\\nCurrently, you have requested " + Request.QueryString["qty"] + " virtual workstations and cannot be allocated additional hardware until tomorrow.\\n\\nIf your initiative requires more than " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day, you must use design builder.\\nPlease contact your technical lead or ClearView administrator for additional information.');<" + "/" + "script>");
                    else if (Request.QueryString["qty"] == "0")
                    {
                        int intDiff = intMaxWorkstationsPerDay - Int32.Parse(Request.QueryString["qty"]);
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "qty", "<script type=\"text/javascript\">alert('NOTE: You can request up to " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day.\\n\\nCurrently, you have requested " + Request.QueryString["qty"] + " virtual workstations. Please enter a quantity of " + intDiff.ToString() + " or less to continue.\\n\\nIf your initiative requires more than " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day, you must use design builder.\\nPlease contact your technical lead or ClearView administrator for additional information.');<" + "/" + "script>");
                    }
                    else
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "qty", "<script type=\"text/javascript\">alert('NOTE: You can request up to " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day.\\n\\nPlease enter a quantity of " + intMaxWorkstationsPerDay.ToString() + " or less to continue.\\n\\nIf your initiative requires more than " + intMaxWorkstationsPerDay.ToString() + " virtual workstations per day, you must use design builder.\\nPlease contact your technical lead or ClearView administrator for additional information.');<" + "/" + "script>");
                }
                int intAddress = intLocation;
                if (intAddress > 0)
                    txtParent.Text = oLocation.GetFull(intAddress);
                hdnParent.Value = intAddress.ToString();
                btnContinue.Attributes.Add("onclick", "return ValidateNumber0('" + txtQuantity.ClientID + "','Please enter a valid quantity')" +
                    " && ValidateDropDown('" + ddlRam.ClientID + "','Please select a RAM')" +
                    " && ValidateDropDown('" + ddlOS.ClientID + "','Please select an operating system')" +
                    " && ValidateDropDown('" + ddlCPU.ClientID + "','Please select a CPU')" +
                    " && ValidateDropDown('" + ddlHardDrive.ClientID + "','Please select a hard drive')" +
                    ";");
            }
            else if (Request.QueryString["rid"] != null)
            {
                int intRequest = Int32.Parse(Request.QueryString["rid"]);
                panExecute.Visible = true;
                DataSet dsService = oForecast.GetAnswerService(intRequest);
                if (dsService.Tables[0].Rows.Count > 0)
                {
                    int intAnswer = Int32.Parse(dsService.Tables[0].Rows[0]["id"].ToString());
                    int intType = oModelsProperties.GetType(intModelVirtual);
                    string strExecute = oType.Get(intType, "forecast_execution_path");
                    if (strExecute != "")
                        btnExecute.Attributes.Add("onclick", "return OpenWindow('FORECAST_EXECUTE','" + strExecute + "?id=" + intAnswer.ToString() + "');");
                    else
                        btnExecute.Attributes.Add("onclick", "alert('Execution has not been configured for asset type " + oType.Get(intType, "name") + "');return false;");
                }
                else
                    btnExecute.Attributes.Add("onclick", "alert('There was a problem executing this request...please contact your ClearView administrator');return false;");
            }
            else
            {
                DataSet ds = oWorkstation.GetPools(0);
                panPools.Visible = true;
                rptPools.DataSource = ds;
                rptPools.DataBind();
                foreach (RepeaterItem ri in rptPools.Items)
                {
                    Label lblWorkstations = (Label)ri.FindControl("lblWorkstations");
                    lblWorkstations.Text = oWorkstation.GetPoolWorkstations(Int32.Parse(lblWorkstations.Text)).Tables[0].Rows.Count.ToString();
                }
                lblPools.Visible = (rptPools.Items.Count == 0);
            }
        }
        private void LoadLists()
        {
            ddlRam.DataTextField = "name";
            ddlRam.DataValueField = "id";
            ddlRam.DataSource = oVirtualRam.GetVirtuals(1);
            ddlRam.DataBind();
            ddlRam.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlOS.DataTextField = "name";
            ddlOS.DataValueField = "id";
            ddlOS.DataSource = oOperatingSystems.Gets(1, 1);
            ddlOS.DataBind();
            ddlOS.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlCPU.DataTextField = "name";
            ddlCPU.DataValueField = "id";
            ddlCPU.DataSource = oVirtualCPU.GetVirtuals(1);
            ddlCPU.DataBind();
            ddlCPU.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlHardDrive.DataTextField = "name";
            ddlHardDrive.DataValueField = "id";
            ddlHardDrive.DataSource = oVirtualHDD.GetVirtuals(1);
            ddlHardDrive.DataBind();
            ddlHardDrive.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void btnCreate_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?create=true");
        }
        protected void btnContinue_Click(Object Sender, EventArgs e)
        {
            DataSet dsToday = oForecast.GetAnswersDay(intWorkstationPlatform, intProfile, 1);
            int intQuantity = 0;
            foreach (DataRow drToday in dsToday.Tables[0].Rows)
                intQuantity += Int32.Parse(drToday["quantity"].ToString());
            int intTotal = Int32.Parse(txtQuantity.Text) + intQuantity;
            if (intTotal <= intMaxWorkstationsPerDay)
            {
                // Add Answer
                int intAnswer = oForecast.AddAnswer(0, intWorkstationPlatform, 0, intProfile);
                oForecast.UpdateAnswerStep(intAnswer, 1, 1);
                int intRequest = oRequest.Add(0, intProfile);
                oServiceRequest.Add(intRequest, 1, 1);
                oForecast.UpdateAnswerService(intAnswer, intRequest);
                int intClass = 0;
                DataSet dsClasses = oClass.Gets(1);
                foreach (DataRow drClass in dsClasses.Tables[0].Rows)
                {
                    if (drClass["prod"].ToString() == "1" && drClass["pnc"].ToString() != "1")
                    {
                        intClass = Int32.Parse(drClass["id"].ToString());
                        break;
                    }
                }
                oForecast.UpdateAnswer(intAnswer, 0, 0, "", 0, Request.ServerVariables["REMOTE_HOST"], "", "", Int32.Parse(Request.Form[hdnParent.UniqueID]), intClass, 0, intCore, 0, 0, 0, Int32.Parse(txtQuantity.Text), 0);
                // Add Model
                oForecast.UpdateAnswerModel(intAnswer, intModelVirtual);
                oForecast.DeleteWorkstation(intAnswer);
                oForecast.AddWorkstation(intAnswer, Int32.Parse(ddlRam.SelectedItem.Value), Int32.Parse(ddlOS.SelectedItem.Value), 0, 0, Int32.Parse(ddlHardDrive.SelectedItem.Value), Int32.Parse(ddlCPU.SelectedItem.Value));
                oForecast.UpdateAnswerStep(intAnswer, 1, 1);
                // Add Commitment Date
                oForecast.UpdateAnswer(intAnswer, DateTime.Today, intConfidence, intProfile);
                oForecast.UpdateAnswerStep(intAnswer, 1, 1);
                Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
            }
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?create=true&qty=" + intQuantity.ToString());
        }
        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            int intID = Int32.Parse(Request.QueryString["id"]);
            oWorkstation.UpdatePool(intID, txtName.Text, txtDescription.Text, Int32.Parse(Request.Form[hdnContact1.UniqueID]), Int32.Parse(Request.Form[hdnContact2.UniqueID]), intProfile, (chkEnabled.Checked ? 1 : 0));
            // Add workstations
            oWorkstation.DeletePoolWorkstations(intID);
            string strWorkstations = Request.Form[hdnWorkstations.UniqueID];
            while (strWorkstations != "")
            {
                string strField = strWorkstations.Substring(0, strWorkstations.IndexOf("&"));
                strWorkstations = strWorkstations.Substring(strWorkstations.IndexOf("&") + 1);
                int intOrder = Int32.Parse(strField.Substring(strField.IndexOf("_") + 1));
                strField = strField.Substring(0, strField.IndexOf("_"));
                oWorkstation.AddPoolWorkstation(intID, Int32.Parse(strField), intOrder, intProfile);
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intID.ToString() + "&save=true");
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage));
        }
    }
}