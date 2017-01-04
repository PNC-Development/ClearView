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

namespace NCC.ClearView.Presentation.Web
{
    public partial class config_server_components : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Forecast oForecast;
        protected Servers oServer;
        protected ServerName oServerName;
        protected Design oDesign;
        protected int intDesign = 0;
        protected int intAnswer = 0;
        protected int intCluster = 0;
        protected int intConfig = 0;
        protected int intNumber = 0;
        protected int intServer = 0;
        protected int intApplication = 0;
        protected bool boolCompleted = false;
        protected string strImageWidth = "30";
        private string strUnavailable = " [Unavailable]";
        private string strRequired = " [Required]";
        private int intClass = 0;
        private int intEnv = 0;
        private int intModel = 0;
        private int intOS = 0;
        private int intSP = 0;
        private TableCell oCell;
        private TableRow oRow;
        protected string strMenuTab1 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
            oDesign = new Design(intProfile, dsn);

            if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
                intClass = Int32.Parse(Request.QueryString["cid"]);
            if (Request.QueryString["eid"] != null && Request.QueryString["eid"] != "")
                intEnv = Int32.Parse(Request.QueryString["eid"]);
            if (Request.QueryString["mid"] != null && Request.QueryString["mid"] != "")
                intModel = Int32.Parse(Request.QueryString["mid"]);
            if (Request.QueryString["osid"] != null && Request.QueryString["osid"] != "")
                intOS = Int32.Parse(Request.QueryString["osid"]);
            if (Request.QueryString["spid"] != null && Request.QueryString["spid"] != "")
                intSP = Int32.Parse(Request.QueryString["spid"]);

            if (intClass > 0 && intEnv > 0 && intModel > 0 && intOS > 0 && intSP > 0)
            {
                if (!IsPostBack)
                {
                    LoadAvailable();
                    LoadPrerequisites();
                }
            }
            else
            {
                if (Request.QueryString["designid"] != null && Request.QueryString["designid"] != "")
                {
                    // EXCEPTION APPROVAL for REVIEW BOARD
                    intDesign = Int32.Parse(Request.QueryString["designid"]);
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    // DATAPOINT
                    intServer = Int32.Parse(Request.QueryString["id"]);
                }
                else
                {
                    // DESIGN EXECUTION
                    if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                        intAnswer = Int32.Parse(Request.QueryString["aid"]);
                    if (Request.QueryString["clusterid"] != null && Request.QueryString["clusterid"] != "")
                        intCluster = Int32.Parse(Request.QueryString["clusterid"]);
                    if (Request.QueryString["csmid"] != null && Request.QueryString["csmid"] != "")
                        intConfig = Int32.Parse(Request.QueryString["csmid"]);
                    if (Request.QueryString["num"] != null && Request.QueryString["num"] != "")
                        intNumber = Int32.Parse(Request.QueryString["num"]);
                }
                if (intDesign > 0)
                {
                    DataSet ds = oDesign.Get(intDesign);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        intModel = oDesign.GetModelProperty(intDesign);
                        intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                        intEnv = Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString());
                        Tab oTab = new Tab("", 0, "divMenu1", true, false);
                        oTab.AddTab("Prerequisites / Required", "");
                        oTab.AddTab("Exceptions / Unavailable", "");
                        strMenuTab1 = oTab.GetTabs();

                        bool boolConfigured = true;
                        Int32.TryParse(ds.Tables[0].Rows[0]["applicationid"].ToString(), out intApplication);
                        Int32.TryParse(ds.Tables[0].Rows[0]["osid"].ToString(), out intOS);
                        Int32.TryParse(ds.Tables[0].Rows[0]["spid"].ToString(), out intSP);
                        if (!IsPostBack)
                        {
                            LoadAvailable();

                            if (boolConfigured == true || Request.QueryString["required"] != null)
                            {
                                // Either the server has already been configured, or additional information is required.
                                // Either way, load listbox with selected components
                                DataSet dsSelected = oDesign.GetSoftwareComponents(intDesign);
                                foreach (DataRow drSelected in dsSelected.Tables[0].Rows)
                                {
                                    //int intPrerequisite = Int32.Parse(drSelected["prerequisiteid"].ToString());
                                    int intPrerequisite = 0;
                                    if (intPrerequisite < 0)
                                        AddDetail(Int32.Parse(drSelected["componentid"].ToString()), intApplication, false, true);
                                    else
                                        AddDetail(Int32.Parse(drSelected["componentid"].ToString()), intPrerequisite, true, true);
                                }
                            }
                            else
                            {
                                // This is the first time loading the page, get selected components from DB
                                DataSet dsSelected = oDesign.GetSoftwareComponents(intDesign);
                                foreach (DataRow drSelected in dsSelected.Tables[0].Rows)
                                    AddDetail(Int32.Parse(drSelected["componentid"].ToString()), 0, true, false);
                            }

                            LoadPrerequisites();

                            // Preload Selected Components
                            Page.ClientScript.RegisterStartupScript(typeof(Page), "preload", "<script type=\"text/javascript\">window.parent.SaveDeviceComponents('" + GetSelected() + "'," + (Request.QueryString["osid"] == null ? "true" : "false") + ");<" + "/" + "script>");
                        }
                    }
                }
                else
                {
                    if (intServer > 0)
                    {
                        DataSet dsServer = oServer.Get(intServer);
                        if (dsServer.Tables[0].Rows.Count > 0)
                        {
                            intAnswer = Int32.Parse(dsServer.Tables[0].Rows[0]["answerid"].ToString());
                            intCluster = Int32.Parse(dsServer.Tables[0].Rows[0]["clusterid"].ToString());
                            intConfig = Int32.Parse(dsServer.Tables[0].Rows[0]["csmconfigid"].ToString());
                            intNumber = Int32.Parse(dsServer.Tables[0].Rows[0]["number"].ToString());
                        }
                    }
                    if (intAnswer > 0)
                    {
                        DataSet ds = oForecast.GetAnswer(intAnswer);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            intModel = oForecast.GetModel(intAnswer);
                            intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                            intEnv = Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString());
                            ds = oServer.Get(intAnswer, intConfig, intCluster, intNumber);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                Tab oTab = new Tab("", 0, "divMenu1", true, false);
                                oTab.AddTab("Prerequisites / Required", "");
                                oTab.AddTab("Exceptions / Unavailable", "");
                                strMenuTab1 = oTab.GetTabs();

                                intServer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                                bool boolConfigured = (ds.Tables[0].Rows[0]["configured"].ToString() == "1");
                                intApplication = Int32.Parse(oForecast.GetAnswer(intAnswer, "applicationid"));
                                boolCompleted = (ds.Tables[0].Rows[0]["build_completed"].ToString() != "");
                                if (boolCompleted == false)
                                    boolCompleted = (oForecast.Get(intAnswer, "completed") != "");
                                Int32.TryParse(ds.Tables[0].Rows[0]["osid"].ToString(), out intOS);
                                Int32.TryParse(ds.Tables[0].Rows[0]["spid"].ToString(), out intSP);
                                if (!IsPostBack)
                                {
                                    LoadAvailable();

                                    //if (oStorage.GetLuns(intAnswer, 0, intCluster, intConfig, intNumber).Tables[0].Rows.Count > 0)
                                    //{
                                    //    foreach (DataRow dr in ds.Tables[0].Rows)
                                    //    {
                                    //        foreach (ListItem oItem in chkComponents.Items)
                                    //        {
                                    //            if (oItem.Value == dr["id"].ToString() && dr["reset_storage"].ToString() == "1")
                                    //            {
                                    //                if (dr["dbase"].ToString() == "1")
                                    //                    oItem.Attributes.Add("onclick", "return confirm('NOTE: Changing this component will erase your storage configuration!\\n\\nCertain components require SAN to be allocated in a pre-defined format.\\nClearView adheres to these standards and will not allow you to change it.\\n\\nAre you sure you want to continue? (Changes take place after you save)') && ShowHideDivCheck('" + divDBA.ClientID + "',this);");
                                    //                else
                                    //                    oItem.Attributes.Add("onclick", "return confirm('NOTE: Changing this component will erase your storage configuration!\\n\\nCertain components require SAN to be allocated in a pre-defined format.\\nClearView adheres to these standards and will not allow you to change it.\\n\\nAre you sure you want to continue? (Changes take place after you save)');");
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    foreach (DataRow dr in ds.Tables[0].Rows)
                                    //    {
                                    //        foreach (ListItem oItem in chkComponents.Items)
                                    //        {
                                    //            if (oItem.Value == dr["id"].ToString() && dr["dbase"].ToString() == "1")
                                    //                oItem.Attributes.Add("onclick", "ShowHideDivCheck('" + divDBA.ClientID + "',this);");
                                    //        }
                                    //    }
                                    //}


                                    if (boolConfigured == true || Request.QueryString["required"] != null)
                                    {
                                        // Either the server has already been configured, or additional information is required.
                                        // Either way, load listbox with selected components
                                        DataSet dsSelected = oServerName.GetComponentDetailSelected(intServer, 1);
                                        foreach (DataRow drSelected in dsSelected.Tables[0].Rows)
                                        {
                                            int intPrerequisite = Int32.Parse(drSelected["prerequisiteid"].ToString());
                                            if (intPrerequisite < 0)
                                                AddDetail(Int32.Parse(drSelected["detailid"].ToString()), intApplication, false, true);
                                            else
                                                AddDetail(Int32.Parse(drSelected["detailid"].ToString()), intPrerequisite, true, true);
                                        }
                                    }
                                    else
                                    {
                                        // This is the first time loading the page, get selected components from DB
                                        string strComponents = oForecast.GetComponents(intAnswer);
                                        char[] strSplit = { ',' };
                                        string[] strComponent = strComponents.Split(strSplit);
                                        for (int ii = 0; ii < strComponent.Length; ii++)
                                        {
                                            if (strComponent[ii].Trim() != "")
                                            {
                                                int intDetail = Int32.Parse(strComponent[ii].Trim());
                                                AddDetail(intDetail, 0, true, false);
                                            }
                                        }
                                    }

                                    LoadPrerequisites();

                                    // Preload Selected Components
                                    Page.ClientScript.RegisterStartupScript(typeof(Page), "preload", "<script type=\"text/javascript\">window.parent.SaveDeviceComponents('" + GetSelected() + "'," + (Request.QueryString["osid"] == null ? "true" : "false") + ");<" + "/" + "script>");
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void ClearPreRequisites()
        {
            for (int ii = 0; ii < lstSelected.Items.Count; ii++)
            {
                if (lstSelected.Items[ii].Text.Contains(strRequired) == true)
                {
                    lstSelected.Items.Remove(lstSelected.Items[ii]);
                    ii--;
                }
            }
        }
        protected void LoadAvailable()
        {
            // Load Components
            lstAvailable.Items.Clear();
            DataSet dsAvailable = oServerName.GetComponentsAvailable(intClass, intEnv, intModel, intOS, intSP);
            lstAvailable.DataValueField = "id";
            lstAvailable.DataTextField = "name";
            lstAvailable.DataSource = dsAvailable;
            lstAvailable.DataBind();

            // Add and Remove Application Prerequisites
            DataSet dsInclude = oServerName.GetComponentDetailSelectedRelated(intApplication, 1);
            foreach (DataRow drInclude in dsInclude.Tables[0].Rows)
                AddDetail(Int32.Parse(drInclude["detailid"].ToString()), intApplication, false, false);
            DataSet dsExclude = oServerName.GetComponentDetailSelectedRelated(intApplication, 0);
            foreach (DataRow drExclude in dsExclude.Tables[0].Rows)
                RemoveDetail(Int32.Parse(drExclude["detailid"].ToString()), true);

            // Remove the selected components from the available
            for (int ii = 0; ii < lstSelected.Items.Count; ii++)
            {
                for (int jj = 0; jj < lstAvailable.Items.Count; jj++)
                {
                    if (Int32.Parse(lstSelected.Items[ii].Value) == Int32.Parse(lstAvailable.Items[jj].Value))
                    {
                        lstAvailable.Items.Remove(lstAvailable.Items[jj]);
                        jj--;
                    }
                }
            }
            // Check for exclusions of the available components
            for (int ii = 0; ii < lstAvailable.Items.Count; ii++)
            {
                int intExclusion = oServerName.IsComponentRelatedExclude(lstSelected, Int32.Parse(lstAvailable.Items[ii].Value));
                if (intExclusion > 0)
                {
                    // Add to Exceptions
                    LoadTableItem(tblExceptions, oServerName.GetComponentDetailName(intExclusion), lstAvailable.Items[ii].Text, false, "");

                    lstAvailable.Items[ii].Text = lstAvailable.Items[ii].Text + strUnavailable;
                    lstAvailable.Items[ii].Attributes.Add("class", "component_unavailable");
                }
            }
        }


        protected void LoadPrerequisites()
        {
            // For the selected components, add prerequisites
            for (int ii = 0; ii < lstSelected.Items.Count; ii++)
                LoadPrerequisites(Int32.Parse(lstSelected.Items[ii].Value));

            // If no prerequisites, add a label showing that
            if (tblPrerequisites.Rows.Count == 0)
            {
                oCell = new TableCell();
                oCell.ColumnSpan = 2;
                oCell.Text = "<i>There are no prerequisites</i>";
                oRow = new TableRow();
                oRow.Cells.Add(oCell);
                tblPrerequisites.Rows.Add(oRow);
            }

            // If no exceptions, add a label showing that
            if (tblExceptions.Rows.Count == 0)
            {
                oCell = new TableCell();
                oCell.ColumnSpan = 2;
                oCell.Text = "<i>There are no exceptions</i>";
                oRow = new TableRow();
                oRow.Cells.Add(oCell);
                tblExceptions.Rows.Add(oRow);
            }
        }
        protected void LoadPrerequisites(int _detailid)
        {
            // Get all the prerequisites of the DETAILID...add it to the PREREQUISITES table and then recurse
            DataSet dsInclude = oServerName.GetComponentDetailRelatedsByRelated(_detailid, 1);
            foreach (DataRow drInclude in dsInclude.Tables[0].Rows)
            {
                int intRelated = Int32.Parse(drInclude["detailid"].ToString());
                AddDetail(intRelated, _detailid, true, false);
                LoadPrerequisites(intRelated);
            }
        }

        
        protected void AddDetail(int _detailid, int _prerequisiteid, bool _is_component, bool _exists)
        {
            // Remove from lstAvailable
            bool boolRemoved = RemoveFromList(_detailid, lstAvailable);

            // Remove from lstSelected
            RemoveFromList(_detailid, lstSelected);

            if (boolRemoved == true)
            {
                string strName = oServerName.GetComponentDetailName(_detailid);
                // Check to see if there is an exclusion preventing the item from being added (intExclusion > 0)
                int intExclusion = oServerName.IsComponentRelatedExclude(lstSelected, _detailid);

                if (_prerequisiteid > 0)
                {
                    if (intExclusion == 0)
                    {
                        // Check to see if it has already been added (the same prerequisite for multiple components would cause this)
                        bool boolAdd = true;
                        foreach (TableRow oTR in tblPrerequisites.Rows)
                        {
                            foreach (TableCell oTD in oTR.Cells)
                            {
                                if (oTD.ToolTip != "")
                                {
                                    string strTemp = oTD.ToolTip;
                                    string strDetail = strTemp.Substring(0, strTemp.IndexOf("_"));
                                    strTemp = strTemp.Substring(strTemp.IndexOf("_") + 1);
                                    if (_detailid.ToString() == strDetail && _prerequisiteid.ToString() == strTemp)
                                        boolAdd = false;
                                    break;
                                }
                            }
                        }
                        if (boolAdd == true)
                        {
                            if (_is_component == true)
                            {
                                // Adding an COMPONENT PREREQUISITE
                                LoadTableItem(tblPrerequisites, oServerName.GetComponentDetailName(_prerequisiteid), strName, true, _detailid.ToString() + "_" + _prerequisiteid.ToString());

                                // Add the item to selected list (with unavailable flag)
                                AddToList(lstSelected, strName + strRequired, "0", (_exists ? "component_exists" : "component_required"));
                            }
                            else
                            {
                                // Adding an APPLICATION PREREQUISITE
                                LoadTableItem(tblPrerequisites, oServerName.GetApplication(_prerequisiteid, "name"), strName, true, _detailid.ToString() + "_" + _prerequisiteid.ToString());

                                // Add the item to selected list (with unavailable flag)
                                AddToList(lstSelected, strName + strRequired, "0", (_exists ? "component_exists" : "component_required"));
                            }
                        }
                    }
                    else
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "inclusions", "<script type=\"text/javascript\">alert('One or more components were not added to the installation list due to an administrative restriction...\\n\\n(Exclusion [1] = " + oServerName.GetComponentDetailName(intExclusion) + "');<" + "/" + "script>");
                }
                else
                {
                    if (intExclusion == 0)
                    {
                        // Add to lstSelected
                        AddToList(lstSelected, strName, _detailid.ToString(), (_exists ? "component_exists" : ""));
                    }
                    else
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "inclusions", "<script type=\"text/javascript\">alert('One or more components were not added to the installation list due to an administrative restriction...\\n\\n(Exclusion [2] = " + oServerName.GetComponentDetailName(intExclusion) + "');<" + "/" + "script>");
                }
            }
        }
        protected void RemoveDetail(int _detailid, bool _application)
        {
            if (_application == true)
            {
                // Remove from lstAvailable
                for (int ii = 0; ii < lstAvailable.Items.Count; ii++)
                {
                    if (Int32.Parse(lstAvailable.Items[ii].Value) == _detailid)
                    {
                        LoadTableItem(tblExceptions, oServerName.GetApplication(intApplication, "name") + " (Server Type)", lstAvailable.Items[ii].Text, false, _detailid.ToString() + "_" + intApplication.ToString());
                        lstAvailable.Items[ii].Text = lstAvailable.Items[ii].Text + strUnavailable;
                        lstAvailable.Items[ii].Attributes.Add("class", "component_unavailable");
                    }
                }
            }
            else
            {
                // Remove from lstSelected
                RemoveFromList(_detailid, lstSelected);

                // Add to lstAvailable
                AddToList(lstAvailable, oServerName.GetComponentDetailName(_detailid), _detailid.ToString(), "");

                // Remove all pre-requisites (and add to lstSelected)
                DataSet dsInclude = oServerName.GetComponentDetailRelatedsByRelated(_detailid, 1);
                foreach (DataRow drInclude in dsInclude.Tables[0].Rows)
                {
                    int intRelated = Int32.Parse(drInclude["detailid"].ToString());
                    AddDetail(intRelated, 0, true, false);
                }
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            string strUnable = "";
            for (int ii = 0; ii < lstAvailable.Items.Count; ii++)
            {
                if (lstAvailable.Items[ii].Selected)
                {
                    if (lstAvailable.Items[ii].Text.Contains(strUnavailable) == false)
                    {
                        AddDetail(Int32.Parse(lstAvailable.Items[ii].Value), 0, true, false);
                        break;
                    }
                    else 
                    {
                        string strTemp = lstAvailable.Items[ii].Text;
                        strTemp = strTemp.Substring(0, strTemp.IndexOf(strUnavailable));
                        strUnable += strTemp;
                    }
                }
            }
            ClearPreRequisites();
            LoadAvailable();
            LoadPrerequisites();
            if (strUnable != "")
                Page.ClientScript.RegisterStartupScript(typeof(Page), "inclusions", "<script type=\"text/javascript\">alert('Due to an administrative restriction, the following component(s) cannot be added to the installation list...\\n\\n" + strUnable + "');<" + "/" + "script>");
            Page.ClientScript.RegisterStartupScript(typeof(Page), "saving", "<script type=\"text/javascript\">window.parent.SaveDeviceComponents('" + GetSelected() + "'," + (Request.QueryString["osid"] == null ? "true" : "false") + ");<" + "/" + "script>");
        }
        protected void btnRemove_Click(Object Sender, EventArgs e)
        {
            string strUnable = "";
            for (int ii = 0; ii < lstSelected.Items.Count; ii++)
            {
                if (lstSelected.Items[ii].Selected)
                {
                    if (lstSelected.Items[ii].Text.Contains(strRequired) == false)
                    {
                        RemoveDetail(Int32.Parse(lstSelected.Items[ii].Value), false);
                        break;
                    }
                    else
                    {
                        string strTemp = lstSelected.Items[ii].Text;
                        strTemp = strTemp.Substring(0, strTemp.IndexOf(strRequired));
                        strUnable += strTemp;
                    }
                }
            }
            ClearPreRequisites();
            LoadAvailable();
            LoadPrerequisites();
            if (strUnable != "")
                Page.ClientScript.RegisterStartupScript(typeof(Page), "exclusions", "<script type=\"text/javascript\">alert('Due to an administrative restriction, the following component(s) cannot be removed from the installation list...\\n\\n" + strUnable + "');<" + "/" + "script>");
            Page.ClientScript.RegisterStartupScript(typeof(Page), "saving", "<script type=\"text/javascript\">window.parent.SaveDeviceComponents('" + GetSelected() + "'," + (Request.QueryString["osid"] == null ? "true" : "false") + ");<" + "/" + "script>");
        }


        protected void LoadTableItem(Table _table, string _name1, string _name2, bool _checkmark, string _tip)
        {
            oCell = new TableCell();
            oRow = new TableRow();
            oCell.Text = "<img src='/images/" + (_checkmark ? "check" : "cancel") + ".gif' border='0' align='absmiddle' /> " + _name1 + " <u>" + (_checkmark ? "requires" : "is incompatible with") + "</u> <b>" + _name2 + "</b>";
            if (_tip != "")
                oCell.ToolTip = _tip;
            oRow.Cells.Add(oCell);
            _table.Rows.Add(oRow);
        }
        protected bool RemoveFromList(int _detailid, ListBox _list)
        {
            bool boolFound = false;
            for (int ii = 0; ii < _list.Items.Count; ii++)
            {
                if (Int32.Parse(_list.Items[ii].Value) == _detailid)
                {
                    _list.Items.Remove(_list.Items[ii]);
                    boolFound = true;
                    ii--;
                }
            }
            return boolFound;
        }
        protected void AddToList(ListBox _list, string _name, string _value, string _class)
        {
            bool boolAdd = true;
            for (int ii = 0; ii < _list.Items.Count; ii++)
            {
                if (_list.Items[ii].Text == _name)
                {
                    boolAdd = false;
                    break;
                }
            }
            if (boolAdd == true)
            {
                ListItem oItem = new ListItem();
                oItem.Text = _name;
                oItem.Value = _value;
                if (_class != "")
                    oItem.Attributes.Add("class", _class);
                _list.Items.Add(oItem);
            }
        }
        protected string GetSelected()
        {
            string strSelected = "";
            for (int ii = 0; ii < lstSelected.Items.Count; ii++)
                strSelected += lstSelected.Items[ii].Value + "&";
            return strSelected;
        }
    }
}
