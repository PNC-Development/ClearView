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

namespace NCC.ClearView.Presentation.Web
{
    public partial class cluster_instance_new : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intUnder48A = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_RESPONSE"]);
        protected int intUnder48Q = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_QUESTION"]);
        protected double dblCompressionPercentage = double.Parse(ConfigurationManager.AppSettings["SQL2008_COMPRESSION_PERCENTAGE"]);
        protected double dblTempDBOverhead = double.Parse(ConfigurationManager.AppSettings["SQL2008_TEMPDB_OVERHEAD"]);
        protected int intProfile;
        protected Servers oServer;
        protected ServerName oServerName;
        protected Cluster oCluster;
        protected Forecast oForecast;
        protected Storage oStorage;
        protected Requests oRequest;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Classes oClass;
        protected int intAnswer = 0;
        protected int intCluster = 0;
        protected int intInstance = 0;
        protected bool boolProd = false;
        protected bool boolQA = false;
        protected bool boolTest = false;
        protected bool boolMidrange = false;
        protected bool boolDatabase = false;
        protected string strMount = "";
        protected string strPaths = "";
        protected bool boolOverride = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oServer = new Servers(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
            oCluster = new Cluster(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oStorage = new Storage(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                intAnswer = Int32.Parse(Request.QueryString["aid"]);
            if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
                intCluster = Int32.Parse(Request.QueryString["cid"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intInstance = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["save"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">window.parent.navigate(window.parent.location.href);alert('Storage Configuration Saved');window.close();<" + "/" + "script>");
            if (intAnswer > 0)
            {
                DataSet ds = oForecast.GetAnswer(intAnswer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int intModel = oForecast.GetModel(intAnswer);
                    int intType = oModelsProperties.GetType(intModel);
                    if (oForecast.IsOSMidrange(intAnswer) == true)
                        boolMidrange = true;
                    bool boolHADisabled = (oModelsProperties.IsHighAvailability(intModel) == false);
                    
                    int intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                    bool boolPNC = (oClass.Get(intClass, "pnc") == "1");
                    boolOverride = (ds.Tables[0].Rows[0]["storage_override"].ToString() == "1");
                    if (oClass.IsProd(intClass))
                    {
                        boolProd = true;
                        if (oForecast.GetAnswer(intAnswer, "test") == "1")
                            boolTest = true;
                        
                    }
                    else if (oClass.IsQA(intClass))
                        boolQA = true;
                    else
                        boolTest = true;

                    if (boolTest == false)
                    {
                        txtTest.Text = "0";
                        txtTest.Enabled = false;
                        txtAmountTest.Text = "0";
                        txtAmountTest.Enabled = false;
                        txtMountTest.Text = "0";
                        txtMountTest.Enabled = false;
                    }
                    if (boolQA == false)
                    {
                        txtQA.Text = "0";
                        txtQA.Enabled = false;
                        txtAmountQA.Text = "0";
                        txtAmountQA.Enabled = false;
                        txtMountQA.Text = "0";
                        txtMountQA.Enabled = false;
                    }
                    if (boolProd == false)
                    {
                        txtSize.Text = "0";
                        txtSize.Enabled = false;
                        txtAmountProd.Text = "0";
                        txtAmountProd.Enabled = false;
                        txtMountProd.Text = "0";
                        txtMountProd.Enabled = false;

                        ddlReplicated.SelectedValue = "No";
                        ddlReplicated.Enabled = false;
                        ddlHigh.SelectedValue = "No";
                        ddlHigh.Enabled = false;
                        ddlMountReplicated.SelectedValue = "No";
                        ddlMountReplicated.Enabled = false;
                        ddlMountHigh.SelectedValue = "No";
                        ddlMountHigh.Enabled = false;
                    }

                    if (intInstance > 0)
                    {
                        ds = oStorage.GetLuns(intAnswer, intInstance, intCluster, 0, 0);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string strPath = dr["path"].ToString();
                            if (strPath != "")
                            {
                                if (strPaths != "")
                                    strPaths += ",";
                                while (strPath != "" && strPath.EndsWith("\\") == true)
                                    strPath = strPath.Substring(0, strPath.Length - 1);
                                strPaths += "\"" + strPath + "\"";
                            }
                            DataSet dsPoints = oStorage.GetMountPoints(Int32.Parse(dr["id"].ToString()));
                            foreach (DataRow drPoint in dsPoints.Tables[0].Rows)
                            {
                                strPath = drPoint["path"].ToString();
                                if (strPath != "")
                                {
                                    if (strPaths != "")
                                        strPaths += ",";
                                    while (strPath != "" && strPath.EndsWith("\\") == true)
                                        strPath = strPath.Substring(0, strPath.Length - 1);
                                    strPaths += "\"" + strPath + "\"";
                                }
                            }
                        }
                    }

                    if (!IsPostBack)
                    {
                        string strFilesystem = "";
                        if (intInstance == 0)
                        {
                            panName.Visible = true;
                            ds = oServer.Get(intAnswer, 0, intCluster, 0);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                bool boolSQL2008 = false;
                                int intServer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                                ds = oServerName.GetComponentDetailSelected(intServer, 1);
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    if (dr["sql"].ToString() == "1" || dr["dbase"].ToString() == "1")
                                    {
                                        boolDatabase = true;
                                        if (dr["name"].ToString().Contains("2008") == true)
                                            boolSQL2008 = true;
                                    }
                                }
                                lblDatabase.Text = (boolDatabase ? (boolSQL2008 ? "2008" : "1") : "0");
                                if (boolDatabase == true && boolMidrange == false)
                                {
                                    panSQL.Visible = true;
                                    if (Request.QueryString["sql"] != null && Request.QueryString["sql"] == "yes")
                                    {
                                        txtName.Text = Request.QueryString["name"];
                                        radYes.Checked = true;
                                        if (boolPNC == false)
                                        {
                                            panDatabase.Visible = true;
                                            radNonYes.Attributes.Add("onclick", "ShowHideDiv('" + divNon.ClientID + "','inline');");
                                            radNonNo.Attributes.Add("onclick", "ShowHideDiv('" + divNon.ClientID + "','none');");
                                            btnGenerate.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a nickname')" +
                                                " && ValidateNumber('" + txtSize.ClientID + "','Please enter a valid size')" +
                                                " && ValidateNumber('" + txtQA.ClientID + "','Please enter a valid size')" +
                                                " && ValidateNumber('" + txtTest.ClientID + "','Please enter a valid size')" +
                                                " && ValidateRadioButtons('" + radNonYes.ClientID + "','" + radNonNo.ClientID + "','Please select whether or not you want to store non-database data on the same instance')" +
                                                " && (document.getElementById('" + radNonYes.ClientID + "').checked == false || (document.getElementById('" + radNonYes.ClientID + "').checked == true && ValidateNumber('" + txtNon.ClientID + "','Please enter a valid size')))" +
                                                " && (document.getElementById('" + radNonYes.ClientID + "').checked == false || (document.getElementById('" + radNonYes.ClientID + "').checked == true && ValidateNumber('" + txtNonQA.ClientID + "','Please enter a valid size')))" +
                                                " && (document.getElementById('" + radNonYes.ClientID + "').checked == false || (document.getElementById('" + radNonYes.ClientID + "').checked == true && ValidateNumber('" + txtNonTest.ClientID + "','Please enter a valid size')))" +
                                                ";");
                                        }
                                        else
                                        {
                                            panDatabasePNC.Visible = true;
                                            radNonPNCYes.Attributes.Add("onclick", "ShowHideDiv('" + divNonPNC.ClientID + "','inline');");
                                            radNonPNCNo.Attributes.Add("onclick", "ShowHideDiv('" + divNonPNC.ClientID + "','none');");
                                            btnGeneratePNC.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a nickname')" +
                                                " && ValidateNumber('" + txtSizePNC.ClientID + "','Please enter a valid size')" +
                                                " && ValidateNumber0('" + txtPercentPNC.ClientID + "', 'Please enter a valid whole number percentage for the total space of the largest Table and/or Index')" +
                                                " && ValidateNumber0('" + txtTempPNC.ClientID + "', 'Please enter a valid number for the amount of storage of TempDB')" +
                                                " && ValidateRadioButtons('" + radNonPNCYes.ClientID + "','" + radNonPNCNo.ClientID + "','Please select whether or not you want to store non-database data on the same instance')" +
                                                " && (document.getElementById('" + radNonPNCYes.ClientID + "').checked == false || (document.getElementById('" + radNonPNCYes.ClientID + "').checked == true && ValidateNumber('" + txtNonPNC.ClientID + "','Please enter a valid size')))" +
                                                ";");
                                        }
                                    }
                                    if (Request.QueryString["sql"] != null && Request.QueryString["sql"] == "no")
                                    {
                                        txtName.Text = Request.QueryString["name"];
                                        radNo.Checked = true;
                                        panDatabaseNo.Visible = true;
                                        if (boolMidrange)
                                        {
                                            panFilesystem.Visible = true;
                                            strFilesystem = " && ValidateText('" + txtFilesystem.ClientID + "','Please enter a filesystem') && PathIsOK('" + txtFilesystem.ClientID + "')";
                                        }
                                        btnSave.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a nickname')" +
                                            " && ValidateDropDown('" + ddlPerformance.ClientID + "','Please select a performance')" +
                                            strFilesystem +
                                            " && ValidateNumber('" + txtAmountProd.ClientID + "','Please enter a valid size')" +
                                            " && ValidateNumber('" + txtAmountTest.ClientID + "','Please enter a valid size')" +
                                            " && ValidateDropDown('" + ddlReplicated.ClientID + "','Please select a replicated option')" +
                                            " && ValidateDropDown('" + ddlHigh.ClientID + "','Please select a high availability')" +
                                            " && ValidateRadioButtons('" + radMountYes.ClientID + "','" + radMountNo.ClientID + "','Please select whether or not you want to add mount points')" +
                                            ";");
                                    }
                                }
                                else
                                {
                                    panDatabaseNo.Visible = true;
                                    if (boolMidrange)
                                    {
                                        panFilesystem.Visible = true;
                                        strFilesystem = " && ValidateText('" + txtFilesystem.ClientID + "','Please enter a filesystem') && PathIsOK('" + txtFilesystem.ClientID + "')";
                                    }
                                    btnSave.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a nickname')" +
                                        " && ValidateDropDown('" + ddlPerformance.ClientID + "','Please select a performance')" +
                                        strFilesystem +
                                        " && ValidateNumber('" + txtAmountProd.ClientID + "','Please enter a valid size')" +
                                        " && ValidateNumber('" + txtAmountTest.ClientID + "','Please enter a valid size')" +
                                        " && ValidateDropDown('" + ddlReplicated.ClientID + "','Please select a replicated option')" +
                                        " && ValidateDropDown('" + ddlHigh.ClientID + "','Please select a high availability')" +
                                        " && ValidateRadioButtons('" + radMountYes.ClientID + "','" + radMountNo.ClientID + "','Please select whether or not you want to add mount points')" +
                                        ";");
                                }
                            }
                        }
                        else
                        {
                            panMount.Visible = true;
                            if (boolMidrange)
                            {
                                panMountFilesystem.Visible = true;
                                strFilesystem = " && ValidateText('" + txtMountFilesystem.ClientID + "','Please enter a filesystem') && PathIsOK('" + txtMountFilesystem.ClientID + "')";
                            }
                            int intLun = oStorage.GetLun(intAnswer, intInstance, intCluster, 0, 0);
                            int intCount = oStorage.GetMountPoints(intLun).Tables[0].Rows.Count + 1;
                            strMount = intCount.ToString();
                            btnMount.Attributes.Add("onclick", "return ValidateDropDown('" + ddlMountPerformance.ClientID + "','Please select a performance')" +
                                strFilesystem +
                                " && ValidateNumber('" + txtMountProd.ClientID + "','Please enter a valid size')" +
                                " && ValidateNumber('" + txtMountTest.ClientID + "','Please enter a valid size')" +
                                " && ValidateDropDown('" + ddlMountReplicated.ClientID + "','Please select a replicated option')" +
                                " && ValidateDropDown('" + ddlMountHigh.ClientID + "','Please select a high availability')" +
                                " && ValidateRadioButtons('" + radMoreYes.ClientID + "','" + radMoreNo.ClientID + "','Please select whether or not you want to add additional mount points')" +
                                ";");
                        }
                    }
                }
            }
            btnClose.Attributes.Add("onclick", "return parent.HidePanel();");
            btnClose2.Attributes.Add("onclick", "return parent.HidePanel();");
            btnClose3.Attributes.Add("onclick", "return parent.HidePanel();");
            btnClosePNC.Attributes.Add("onclick", "return parent.HidePanel();");
        }
        protected void radYes_Check(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&cid=" + intCluster + "&name=" + txtName.Text + "&sql=yes");
        }
        protected void radNo_Check(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&cid=" + intCluster + "&name=" + txtName.Text + "&sql=no");
        }
        protected void btnGenerate_Click(Object Sender, EventArgs e)
        {
            int intSQL = (radYes.Checked ? 1 : (radNo.Checked ? 0 : -1));
            intInstance = oCluster.AddInstance(intCluster, txtName.Text, intSQL);
            double dblSize = double.Parse(txtSize.Text);
            double dblQA = double.Parse(txtQA.Text);
            double dblTest = double.Parse(txtTest.Text);
            int intNon = (radNonYes.Checked ? 1 : 0);
            double dblNon = 0.00;
            double dblNonQA = 0.00;
            double dblNonTest = 0.00;
            if (intNon == 1)
            {
                dblNon = double.Parse(txtNon.Text);
                dblNonQA = double.Parse(txtNonQA.Text);
                dblNonTest = double.Parse(txtNonTest.Text);
            }
            oStorage.AddLunSQL(intAnswer, intInstance, intCluster, 0, 0, dblSize, dblQA, dblTest, dblNon, dblNonQA, dblNonTest);
            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&cid=" + intCluster + "&save=true");
        }
        protected void btnGeneratePNC_Click(Object Sender, EventArgs e)
        {
            int intSQL = (radYes.Checked ? 1 : (radNo.Checked ? 0 : -1));
            intInstance = oCluster.AddInstance(intCluster, txtName.Text, intSQL);
            double dblSize = double.Parse(txtSizePNC.Text);
            int intNon = (radNonPNCYes.Checked ? 1 : 0);
            double dblNon = 0.00;
            if (intNon == 1)
                dblNon = double.Parse(txtNonPNC.Text);
            double dblPercent = double.Parse(txtPercentPNC.Text);
            double dblTempDB = double.Parse(txtTempPNC.Text);
            oStorage.AddLunSQLPNC(intAnswer, intInstance, intCluster, 0, 0, dblSize, dblNon, dblPercent, dblTempDB, dblCompressionPercentage, dblTempDBOverhead, (lblDatabase.Text == "2008"));
            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&cid=" + intCluster + "&save=true");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intSQL = (radYes.Checked ? 1 : (radNo.Checked ? 0 : -1));
            intInstance = oCluster.AddInstance(intCluster, txtName.Text, intSQL);
            int intDrive = 0;
            if (boolOverride == false)
                intDrive = oStorage.GetNextLun(intAnswer, intCluster, 0, 0);
            oStorage.AddLun(intAnswer, intInstance, intCluster, 0, 0, intDrive, ddlPerformance.SelectedItem.Value, txtFilesystem.Text, double.Parse(txtAmountProd.Text), double.Parse(txtAmountQA.Text), double.Parse(txtAmountTest.Text), (ddlReplicated.SelectedItem.Value == "Yes" ? 1 : 0), (ddlHigh.SelectedItem.Value == "Yes" ? 1 : 0));
            oCluster.UpdateQuorum(intCluster, 1);
            if (radMountYes.Checked)
                Response.Redirect(Request.Path + "?aid=" + intAnswer + "&cid=" + intCluster + "&id=" + intInstance);
            else
                Response.Redirect(Request.Path + "?aid=" + intAnswer + "&cid=" + intCluster + "&save=true");
        }
        protected void btnMount_Click(Object Sender, EventArgs e)
        {
            int intLun = oStorage.GetLun(intAnswer, intInstance, intCluster, 0, 0);
            oStorage.AddMountPoint(intLun, txtMountFilesystem.Text, ddlMountPerformance.SelectedItem.Value, double.Parse(txtMountProd.Text), double.Parse(txtMountQA.Text), double.Parse(txtMountTest.Text), (ddlMountReplicated.SelectedItem.Value == "Yes" ? 1 : 0), (ddlMountHigh.SelectedItem.Value == "Yes" ? 1 : 0));
            if (radMoreYes.Checked)
                Response.Redirect(Request.Path + "?aid=" + intAnswer + "&cid=" + intCluster + "&id=" + intInstance);
            else
                Response.Redirect(Request.Path + "?aid=" + intAnswer + "&cid=" + intCluster + "&save=true");
        }
    }
}
