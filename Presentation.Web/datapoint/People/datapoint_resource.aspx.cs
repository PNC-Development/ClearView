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
    public partial class datapoint_resource : BasePage
    {
        public event DataListItemEventHandler ItemDataBound;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private int intDataPointAvailablePeople = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_PEOPLE"]);
        private DataPoint oDataPoint;
        private Functions oFunction;
        private Users oUser;
        private int intProfile = 0;
        private int intApplication = 0;
        private int intUserId=0;
        protected string strMenuTab1 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            oDataPoint = new DataPoint(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);

            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "PEOPLE") == true || intDataPointAvailablePeople == 1))
            {
             
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    intUserId = Int32.Parse(oFunction.decryptQueryString(Request.QueryString["id"]));
                    LoadData();
                }
                pnlAllow.Visible = true;
                lblHeaderSub.Text = "Provides all the information about a resource...";
                //Tabs
                int intMenuTab = 0;
             
                Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);

                oTab.AddTab("Resource Information", "");
                oTab.AddTab("Service(s) Progression", "");
                oTab.AddTab("Resource(s) Involvement", "");
                oTab.AddTab("Personnel Asset", "");
                oTab.AddTab("Documents", "");
                oTab.AddTab("Raves", "");
                oTab.AddTab("Education and Certificates", "");
                oTab.AddTab("Blog", "");
                oTab.AddTab("Contact Information", "");
                
                strMenuTab1 = oTab.GetTabs();
            }
            else
                pnlDenied.Visible = true;

            }

        private void LoadData()
        {
            LoadUserInformation();

            ucServiceProgression.ResourceAssigned = intUserId;
            ucResourceInvolvement.ResourceId = intUserId;
            ucUserContactInfo.UserId = intUserId;
        }

        private void LoadUserInformation()
        {
            DataSet ds = oDataPoint.GetPeopleSearchResults(
                          intUserId,
                          "",
                          "",
                          "",
                          null,
                          null,
                          null,
                          "", 0, 1, 0);
            if (ds.Tables.Count > 0)
            {   if (ds.Tables[0].Rows.Count == 1)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    Master.Page.Title = "DataPoint | Resource ( " + dr["UserName"].ToString() + " )";

                    lblHeader.Text = dr["UserName"].ToString() + "(" + dr["XID"].ToString() + ")";
                
                    lblLANId.Text = dr["XID"].ToString();
                    lblFName.Text =  dr["fname"].ToString();
                    lblLName.Text = dr["lname"].ToString();
                    lblManager.Text = dr["ManagerName"].ToString();
                    lblManager.ToolTip = "User ID :"+dr["Manager"].ToString();
                    chkIsManager.Checked= (dr["ismanager"] != DBNull.Value ? (dr["ismanager"].ToString() == "1"? true : false):false);
                    chkIsBoardMember.Checked = (dr["board"] != DBNull.Value ? (dr["board"].ToString() == "1" ? true : false) : false);
                    chkIsDirector.Checked = (dr["director"] != DBNull.Value ? (dr["director"].ToString() == "1" ? true : false) : false);

                    lblPager.Text = dr["Pager"].ToString() +(dr["atid"]!= DBNull.Value && dr["atid"].ToString()!= "0" ? " @ " + dr["ATName"].ToString() : ""); 
                    lblPhone.Text = dr["phone"].ToString();
                    lblSpecialSkills.Text = dr["other"].ToString();
                    lblVacationDays.Text = (dr["VacationDaysUsed"] != DBNull.Value ? dr["VacationDaysUsed"].ToString():"0") + " / " + (dr["vacation"] != DBNull.Value ?dr["vacation"].ToString():"0");
                   
                    chkMultipleApps.Checked = (dr["MultipleApplications"] != DBNull.Value ? (dr["MultipleApplications"].ToString() == "1" ? true : false) : false);
                    chkAddLocation.Checked = (dr["AddLocation"] != DBNull.Value ? (dr["AddLocation"].ToString() == "1" ? true : false) : false);
                    chkAdmin.Checked = (dr["Administrator"] != DBNull.Value ? (dr["Administrator"].ToString() == "1" ? true : false) : false);
                    chkEnabled.Checked = (dr["enabled"] != DBNull.Value ? (dr["enabled"].ToString() == "1" ? true : false) : false);
                    imgPicture.ImageUrl = "/frame/picture.aspx?xid=" + dr["XID"].ToString();
                    imgPicture.Style["border"] = "solid 1px #999999";
                    

                }
            }
        }
    }
}

