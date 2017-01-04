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
using NCC.ClearView.Presentation.Web.Custom;
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class asset_search : System.Web.UI.UserControl
    {
        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intWorkload = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intWorkloadManager = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkloadManager"]);
        protected string strAdmins = ConfigurationManager.AppSettings["Administrators"];
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected Pages oPage;
        protected Users oUser;
        protected Classes oClasses;
        protected Environments oEnvironment;
        protected Platforms oPlatform;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Racks oRacks;
        protected Rooms oRooms;
        protected Floor oFloor;
        protected Asset oAsset;
        protected Locations oLocation;
        protected Depot oDepot;
        protected int intRecords = 20;
        protected int intRecordStart = 1;
        protected string strMenuTab1 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oClasses = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oRacks = new Racks(intProfile, dsn);
            oRooms = new Rooms(intProfile, dsn);
            oFloor = new Floor(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oLocation = new Locations(intProfile, dsn);
            oDepot = new Depot(intProfile, dsn);



            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                int intID = Int32.Parse(Request.QueryString["id"]);
                if (Request.QueryString["decommed"] != null && Request.QueryString["decommed"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "decommed", "<script type=\"text/javascript\">alert('Asset Successfully Decommissioned');window.navigate('" + oPage.GetFullLink(intPage) + "?sid=" + Request.QueryString["sid"] + "');<" + "/" + "script>");
                if (Request.QueryString["commed"] != null && Request.QueryString["commed"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "commed", "<script type=\"text/javascript\">alert('Asset Successfully Commissioned');window.navigate('" + oPage.GetFullLink(intPage) + "?sid=" + Request.QueryString["sid"] + "');<" + "/" + "script>");
                int intStatus = Int32.Parse(oAsset.Get(intID, "status"));
                int intModel = Int32.Parse(oAsset.Get(intID, "modelid"));
                int intType = 0;
                if (intModel > 0)
                {
                    intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                    intType = Int32.Parse(oModel.Get(intModel, "typeid"));
                }
                
                string[] strProfile;
                char[] strSplit = { ';' };
                strProfile = strAdmins.Split(strSplit);
                for (int ii = 0; ii < strProfile.Length; ii++)
                {
                    if (strProfile[ii].Trim() != "")
                    {
                        
                    }
                }
                panPath.Visible = true;
                string strPath = "";
                switch (intStatus)
                {
                    case 0:
                        strPath = oType.Get(intType, "asset_checkin_path");
                        break;
                    case 1:
                        strPath = oType.Get(intType, "asset_commission_path");
                        break;
                    case 10:
                        strPath = oType.Get(intType, "asset_update_path");
                        break;
                }
                if (strPath != "")
                    PHControl.Controls.Add((Control)LoadControl(strPath));
                else
                    panNoPath.Visible = true;
            }
            else if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
            {
                LoadSearch(Int32.Parse(Request.QueryString["sid"]));
            }
            else
            {
                lblCount.Text = "Currently searching " + SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, "SELECT * FROM cva_assets WHERE deleted = 0").Tables[0].Rows.Count.ToString() + " devices";
                panSearch.Visible = true;
                LoadLists();
                btnSearch.Attributes.Add("onclick", "return ValidateSearch('" + hdnType.ClientID + "','" + txtName.ClientID + "','" + txtSerial.ClientID + "','" + txtAsset.ClientID + "','" + ddlPlatformClass.ClientID + "','" + ddlClass.ClientID + "','" + ddlPlatform.ClientID + "','" + ddlPlatformDepot.ClientID + "','" + ddlDepot.ClientID + "');");
            }
            ddlClass.Attributes.Add("onchange", "WaitDDL('" + divWaitClass.ClientID + "');");
            ddlPlatform.Attributes.Add("onchange", "WaitDDL('" + divWaitPlatform.ClientID + "');");
            ddlTypes.Attributes.Add("onchange", "WaitDDL('" + divWaitType.ClientID + "');");
            ddlPlatformDepot.Attributes.Add("onchange", "WaitDDL('" + divPlatformDepot.ClientID + "');");
            ddlPlatformClass.Attributes.Add("onchange", "WaitDDL('" + divPlatformClass.ClientID + "');");
            btnClose.Attributes.Add("onclick", "return CloseWindow();");
        }
        private void LoadLists()
        {

            //Menu
            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
            Tab oTab = new Tab(hdnType.ClientID, intMenuTab, "divMenu1", true, false);
            //End Menu

            ddlPlatform.DataValueField = "platformid";
            ddlPlatform.DataTextField = "name";
            ddlPlatform.DataSource = oPlatform.GetAssets(1);
            ddlPlatform.DataBind();
            ddlPlatform.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlPlatformDepot.DataValueField = "platformid";
            ddlPlatformDepot.DataTextField = "name";
            ddlPlatformDepot.DataSource = oPlatform.GetAssets(1);
            ddlPlatformDepot.DataBind();
            ddlPlatformDepot.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlPlatformClass.DataValueField = "platformid";
            ddlPlatformClass.DataTextField = "name";
            ddlPlatformClass.DataSource = oPlatform.GetAssets(1);
            ddlPlatformClass.DataBind();
            ddlPlatformClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            int intPlatform = 0;
            if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != "")
                intPlatform = Int32.Parse(Request.QueryString["pid"]);
            if (oPlatform.Get(intPlatform).Tables[0].Rows.Count > 0)
            {
                ddlPlatform.SelectedValue = intPlatform.ToString();
                ddlPlatformDepot.SelectedValue = intPlatform.ToString();
                ddlPlatformClass.SelectedValue = intPlatform.ToString();
                // Types
                ddlTypes.DataValueField = "id";
                ddlTypes.DataTextField = "name";
                ddlTypes.DataSource = oType.Gets(intPlatform, 1);
                ddlTypes.DataBind();
                ddlTypes.Items.Insert(0, new ListItem("-- ALL --", "0"));
                // Classes
                ddlClass.DataValueField = "id";
                ddlClass.DataTextField = "name";
                ddlClass.DataSource = oClasses.Gets(1);
                ddlClass.DataBind();
                ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                int intClass = 0;
                if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
                    intClass = Int32.Parse(Request.QueryString["cid"]);
                if (oClasses.Get(intClass).Tables[0].Rows.Count > 0)
                {
                    ddlClass.SelectedValue = intClass.ToString();
                    ddlEnvironment.DataValueField = "id";
                    ddlEnvironment.DataTextField = "name";
                    ddlEnvironment.DataSource = oClasses.GetEnvironment(intClass, 0);
                    ddlEnvironment.DataBind();
                    ddlEnvironment.Items.Insert(0, new ListItem("-- ALL --", "0"));
                }
                else
                {
                    ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                    ddlEnvironment.Enabled = false;
                }
                // Depot
                ddlDepot.DataValueField = "id";
                ddlDepot.DataTextField = "name";
                ddlDepot.DataSource = oDepot.Gets(1);
                ddlDepot.DataBind();
                ddlDepot.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                int intType = 0;
                if (Request.QueryString["tid"] != null && Request.QueryString["tid"] != "")
                    intType = Int32.Parse(Request.QueryString["tid"]);
                if (oType.Get(intType).Tables[0].Rows.Count > 0)
                {
                    ddlTypes.SelectedValue = intType.ToString();
                    ddlModel.DataValueField = "id";
                    ddlModel.DataTextField = "name";
                    ddlModel.DataSource = oModel.Gets(intType, 1);
                    ddlModel.DataBind();
                    ddlModel.Items.Insert(0, new ListItem("-- ALL --", "0"));
                }
                else
                {
                    ddlModel.Items.Insert(0, new ListItem("-- ALL --", "0"));
                    ddlModel.Enabled = false;
                }
            }
            else
            {
                ddlTypes.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlTypes.Enabled = false;
                ddlModel.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlModel.Enabled = false;
                ddlDepot.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlDepot.Enabled = false;
                ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlClass.Enabled = false;
                ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlEnvironment.Enabled = false;
            }

            //if (boolClass == false && boolType == false && boolDepot == false)
            //{
            //    boolName = true;
            //    hdnType.Value = "N";
            //strMenuTab1 += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab1','" + hdnType.ClientID + "','N',true);\" class=\"tabheader\">Search By Name / Serial / Asset</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
            //strMenuTab1 += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab2','" + hdnType.ClientID + "','C',true);\" class=\"tabheader\">Search By Class / Environment</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
            //strMenuTab1 += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab3','" + hdnType.ClientID + "','T',true);\" class=\"tabheader\">Search By Type / Model</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
            //strMenuTab1 += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab4','" + hdnType.ClientID + "','D',true);\" class=\"tabheader\">Search By Depot</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
            oTab.AddTab("Search By Name / Serial / Asset", "");
            oTab.AddTab("Search By Class / Environment", "");
            oTab.AddTab("Search By Type / Model", "");
            oTab.AddTab("Search By Depot", "");

            //}
            //else 
            //{
            //    //strMenuTab1 += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab1','" + hdnType.ClientID + "','N',true);\" class=\"tabheader\">Search By Name / Serial / Asset</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
            //    oTab.AddTab("Search By Name / Serial / Asset", "");
            //    if (boolClass == true)
            //        //strMenuTab1 += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab2','" + hdnType.ClientID + "','C',true);\" class=\"tabheader\">Search By Class / Environment</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
            //        oTab.AddTab("Search By Class / Environment", "");
            //    else
            //        //strMenuTab1 += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab2','" + hdnType.ClientID + "','C',true);\" class=\"tabheader\">Search By Class / Environment</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
            //        oTab.AddTab("Search By Class / Environment", "");
            //    if (boolType == true)
            //        //strMenuTab1 += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab3','" + hdnType.ClientID + "','T',true);\" class=\"tabheader\">Search By Type / Model</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
            //        oTab.AddTab("Search By Type / Model", "");
            //    else
            //        //strMenuTab1 += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab3','" + hdnType.ClientID + "','T',true);\" class=\"tabheader\">Search By Type / Model</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
            //        oTab.AddTab("Search By Type / Model", "");
            //    if (boolDepot == true)
            //        //strMenuTab1 += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab4','" + hdnType.ClientID + "','D',true);\" class=\"tabheader\">Search By Depot</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
            //         oTab.AddTab("Search By Depot", "");
            //    else
            //        //strMenuTab1 += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab4','" + hdnType.ClientID + "','D',true);\" class=\"tabheader\">Search By Depot</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
            //        oTab.AddTab("Search By Depot", "");
            //}
            strMenuTab1 = oTab.GetTabs();
        }
        protected void ddlPlatformClass_Change(Object Sender, EventArgs e)
        {
            if (ddlPlatformClass.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?menu_tab=2&pid=" + ddlPlatformClass.SelectedItem.Value);
            else
                Response.Redirect(oPage.GetFullLink(intPage));
        }
        protected void ddlClass_Change(Object Sender, EventArgs e)
        {
            if (ddlClass.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?menu_tab=2pid=" + Request.QueryString["pid"] + "&cid=" + ddlClass.SelectedItem.Value);
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?menu_tab=2&pid=" + Request.QueryString["pid"]);
        }
        protected void ddlPlatform_Change(Object Sender, EventArgs e)
        {
            if (ddlPlatform.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?menu_tab=3&pid=" + ddlPlatform.SelectedItem.Value);
            else
                Response.Redirect(oPage.GetFullLink(intPage));
        }
        protected void ddlTypes_Change(Object Sender, EventArgs e)
        {
            if (ddlTypes.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?menu_tab=3&pid=" + Request.QueryString["pid"] + "&tid=" + ddlTypes.SelectedItem.Value);
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?menu_tab=3&pid=" + Request.QueryString["pid"]);
        }
        protected void ddlPlatformDepot_Change(Object Sender, EventArgs e)
        {
            if (ddlPlatformDepot.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?menu_tab=4&pid=" + ddlPlatformDepot.SelectedItem.Value);
            else
                Response.Redirect(oPage.GetFullLink(intPage));
        }
        protected void btnSort_Click(Object Sender, EventArgs e)
        {
            LinkButton oOrder = (LinkButton)Sender;
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
            {
                if (Request.QueryString["sort"] == oOrder.CommandArgument)
                    strOrder = oOrder.CommandArgument + " DESC";
            }
            if (strOrder == "")
                strOrder = oOrder.CommandArgument;
            if (Request.QueryString["expand"] == null)
                Reload("?pid=" + Request.QueryString["pid"] + "&sid=" + Request.QueryString["sid"] + "&sort=" + strOrder);
            else
                Reload("?pid=" + Request.QueryString["pid"] + "&sid=" + Request.QueryString["sid"] + "&sort=" + strOrder + "&expand=true");
        }
        private void LoadSearch(int _search)
        {
            lblTitle.Text = "Search Results";
            ds = oAsset.GetSearch(_search);
            if (ds.Tables[0].Rows.Count > 0)
            {
                panResult.Visible = true;
                lblSearch.Text = _search.ToString();
                string strSort = "";
                if (Request.QueryString["sort"] != null && Request.QueryString["sort"] != "")
                    strSort = "&sort=" + Request.QueryString["sort"];
                btnPrint.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','/frame/print_asset_search.aspx?sid=" + _search.ToString() + strSort + "');");
                if (ds.Tables[0].Rows[0]["userid"].ToString() == intProfile.ToString())
                {
                    string strSearch = ds.Tables[0].Rows[0]["type"].ToString();
                    switch (strSearch)
                    {
                        case "1":
                            if (ds.Tables[0].Rows[0]["name"].ToString().Trim() != "")
                            {
                                lblResults.Text = "Device Name LIKE &quot;" + ds.Tables[0].Rows[0]["name"].ToString().Trim() + "&quot;";
                            }
                            if (ds.Tables[0].Rows[0]["serial"].ToString().Trim() != "")
                            {
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text = "Serial Number LIKE &quot;" + ds.Tables[0].Rows[0]["serial"].ToString().Trim() + "&quot;";
                            }
                            if (ds.Tables[0].Rows[0]["asset"].ToString().Trim() != "")
                            {
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text = "Asset Tag LIKE &quot;" + ds.Tables[0].Rows[0]["asset"].ToString().Trim() + "&quot;";
                            }
                            break;
                        case "2":
                            if (ds.Tables[0].Rows[0]["classid"].ToString().Trim() != "0")
                            {
                                lblResults.Text = "Class = &quot;" + oClasses.Get(Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString().Trim()), "name") + "&quot;";
                            }
                            if (ds.Tables[0].Rows[0]["environmentid"].ToString().Trim() != "0")
                            {
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Environment = &quot;" + oEnvironment.Get(Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString().Trim()), "name") + "&quot;";
                            }
                            break;
                        case "3":
                            string strPlatform = ds.Tables[0].Rows[0]["platformid"].ToString().Trim();
                            if (strPlatform != "" && strPlatform != "0")
                            {
                                lblResults.Text = "Platform = &quot;" + oPlatform.GetName(Int32.Parse(strPlatform)) + "&quot;";
                            }
                            string strType = ds.Tables[0].Rows[0]["typeid"].ToString().Trim();
                            if (strType != "" && strType != "0")
                            {
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Asset Type = &quot;" + oType.Get(Int32.Parse(strType), "name") + "&quot;";
                            }
                            string strModel = ds.Tables[0].Rows[0]["modelid"].ToString().Trim();
                            if (strModel != "" && strModel != "0")
                            {
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Model = &quot;" + oModel.Get(Int32.Parse(strModel), "name") + "&quot;";
                            }
                            break;
                        case "4":
                            string strDepot = ds.Tables[0].Rows[0]["depotid"].ToString().Trim();
                            if (strDepot != "" && strDepot != "0")
                            {
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Depot = &quot;" + oDepot.Get(Int32.Parse(strDepot), "name") + "&quot;";
                            }
                            break;
                    }
                    ds = oAsset.GetSearchResults(_search);
                    lblPage.Text = "1";
                    lblSort.Text = "";
                    if (Request.QueryString["page"] != null && Request.QueryString["page"] != "")
                        lblPage.Text = Request.QueryString["page"];
                    if (Request.QueryString["sort"] != null && Request.QueryString["sort"] != "")
                        lblSort.Text = Request.QueryString["sort"];
                    lblTopPaging.Text = "";
                    LoadPaging(Int32.Parse(lblPage.Text), lblSort.Text);
                }
                else
                    panDenied.Visible = true;
            }
            else
                panDenied.Visible = true;
        }
        protected void btnSearch_Click(Object Sender, EventArgs e)
        {
            string strSearch = Request.Form[hdnType.UniqueID];
            int intSearch = 0;
            switch (strSearch)
            {
                case "1":
                    intSearch = oAsset.AddSearchName(intProfile, strSearch, txtName.Text, txtSerial.Text, txtAsset.Text);
                    break;
                case "2":
                    intSearch = oAsset.AddSearchClass(intProfile, strSearch, Int32.Parse(ddlPlatformClass.SelectedItem.Value), Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(ddlEnvironment.SelectedItem.Value));
                    break;
                case "3":
                    intSearch = oAsset.AddSearchPlatform(intProfile, strSearch, Int32.Parse(ddlPlatform.SelectedItem.Value), Int32.Parse(ddlTypes.SelectedItem.Value), Int32.Parse(ddlModel.SelectedItem.Value));
                    break;
                case "4":
                    intSearch = oAsset.AddSearchDepot(intProfile, strSearch, Int32.Parse(ddlPlatformDepot.SelectedItem.Value), Int32.Parse(ddlDepot.SelectedItem.Value));
                    break;
            }
            Reload("?sid=" + intSearch.ToString());
        }
        private void LoopRepeater(string _sort, int _start)
        {
            if (_start > ds.Tables[0].Rows.Count)
                _start = 0;
            intRecordStart = _start + 1;
            DataView dv = ds.Tables[0].DefaultView;
            if (Request.QueryString["sort"] != null)
                dv.Sort = Request.QueryString["sort"];
            int intCount = _start + intRecords;
            if (dv.Count < intCount)
                intCount = dv.Count;
            int ii = 0;
            lblRecords.Text = "Requests " + intRecordStart.ToString() + " - " + intCount.ToString() + " of " + dv.Count.ToString();
            for (ii = 0; ii < _start; ii++)
                dv[0].Delete();
            int intTotalCount = (dv.Count - intRecords);
            for (ii = 0; ii < intTotalCount; ii++)
                dv[intRecords].Delete();
            rptView.DataSource = dv;
            rptView.DataBind();
            lblNone.Visible = (rptView.Items.Count == 0);
            _start++;
        }
        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            LinkButton oOrder = (LinkButton)Sender;
            string strPage = "";
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
            {
                if (Request.QueryString["sort"] == oOrder.CommandArgument)
                    strOrder = oOrder.CommandArgument + " DESC";
            }
            if (strOrder == "")
                strOrder = oOrder.CommandArgument;
            if (Request.QueryString["page"] != null)
                strPage = Request.QueryString["page"];
            Reload("?sid=" + Request.QueryString["sid"] + "&sort=" + strOrder + "&page=" + strPage);
        }
        protected void btnPage_Click(Object Sender, ImageClickEventArgs e)
        {
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
                strOrder = Request.QueryString["sort"];
            Reload("?sid=" + Request.QueryString["sid"] + "&sort=" + strOrder + "&page=" + txtPage.Text);
        }
        private void LoadPaging(int intStart, string _sort)
        {
            int intCount = ds.Tables[0].Rows.Count;
            double dblEnd = Math.Ceiling(Double.Parse(intCount.ToString()) / Double.Parse(intRecords.ToString()));
            int intEnd = Int32.Parse(dblEnd.ToString());
            int ii = 0;
            txtPage.Text = intStart.ToString();
            lblPages.Text = intEnd.ToString();
            if (intEnd < 7)
            {
                for (ii = 1; ii < intEnd; ii++)
                {
                    LoadLink(lblTopPaging, ii, ", ", intStart);
                    LoadLink(lblBottomPaging, ii, ", ", intStart);
                }
                LoadLink(lblTopPaging, intEnd, "", intStart);
                LoadLink(lblBottomPaging, intEnd, "", intStart);
            }
            else
            {
                if (intStart < 5)
                {
                    for (ii = 1; ii < 6 && ii < intEnd; ii++)
                    {
                        LoadLink(lblTopPaging, ii, ", ", intStart);
                        LoadLink(lblBottomPaging, ii, ", ", intStart);
                    }
                    if (ii < intEnd)
                    {
                        LoadLink(lblTopPaging, ii, " .... ", intStart);
                        LoadLink(lblBottomPaging, ii, " .... ", intStart);
                    }
                    LoadLink(lblTopPaging, intEnd, "", intStart);
                    LoadLink(lblBottomPaging, intEnd, "", intStart);
                }
                else if (intStart > (intEnd - 4))
                {
                    LoadLink(lblTopPaging, 1, " .... ", intStart);
                    LoadLink(lblBottomPaging, 1, " .... ", intStart);
                    for (ii = (intEnd - 5); ii < intEnd && ii > 0; ii++)
                    {
                        LoadLink(lblTopPaging, ii, ", ", intStart);
                        LoadLink(lblBottomPaging, ii, ", ", intStart);
                    }
                    LoadLink(lblTopPaging, intEnd, "", intStart);
                    LoadLink(lblBottomPaging, intEnd, "", intStart);
                }
                else
                {
                    LoadLink(lblTopPaging, 1, " .... ", intStart);
                    LoadLink(lblBottomPaging, 1, " .... ", intStart);
                    for (ii = (intStart - 2); ii < (intStart + 3) && ii < intEnd && ii > 0; ii++)
                    {
                        if (ii == (intStart + 2))
                        {
                            LoadLink(lblTopPaging, ii, " .... ", intStart);
                            LoadLink(lblBottomPaging, ii, " .... ", intStart);
                        }
                        else
                        {
                            LoadLink(lblTopPaging, ii, ", ", intStart);
                            LoadLink(lblBottomPaging, ii, ", ", intStart);
                        }
                    }
                    LoadLink(lblTopPaging, intEnd, "", intStart);
                    LoadLink(lblBottomPaging, intEnd, "", intStart);
                }
            }
            LoopRepeater(_sort, ((intStart - 1) * intRecords));
        }
        private void LoadLink(Label _label, int _number, string _spacer, int _start)
        {
            StringBuilder sb = new StringBuilder(_label.Text);

            if (_number == _start)
            {
                sb.Append("<b><font style=\"color:#CC0000\">");
                sb.Append(_number.ToString());
                sb.Append("</font></b>");
            }
            else
            {
                string strSort = "";
                if (Request.QueryString["sort"] != null)
                {
                    strSort = Request.QueryString["sort"];
                }

                sb.Append("<a href=\"");
                sb.Append(oPage.GetFullLink(intPage));
                sb.Append("?sid=");
                sb.Append(Request.QueryString["sid"]);
                sb.Append("&sort=");
                sb.Append(strSort);
                sb.Append("&page=");
                sb.Append(_number.ToString());
                sb.Append("\" title=\"Go to Page ");
                sb.Append(_number.ToString());
                sb.Append("\">");
                sb.Append(_number.ToString());
                sb.Append("</a>");
            }
            if (_spacer != "")
            {
                sb.Append(_spacer);
            }

            _label.Text = sb.ToString();
        }
        private void Reload(string _redirect)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + _redirect);
        }
    }
}