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
    public partial class index_new : BasePage
    {
        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected string strTitle = ConfigurationManager.AppSettings["appTitle"];
        protected Applications oApplication;
        protected Pages oPage;
        protected AppPages oAppPage;
        protected PageControls oPageControl;
        protected Designer oDesigner;
        protected Settings oSetting;
        protected int intProfile;
        protected string strPage = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Control oControl;
            Page.Title = strTitle;
            if (Request.QueryString["down"] != null)
            {
                oControl = (Control)LoadControl("/controls/sys/sys_down.ascx");
                PH3.Controls.Add(oControl);
            }
            else
            {
                oControl = (Control)LoadControl("/controls/sys/sys_will_down.ascx");
                PHDown.Controls.Add(oControl);
                if (Request.Cookies["profileid"] == null || Request.Cookies["profileid"].Value == "")
                {
                    Response.Cookies["userloginreferrer"].Value = Request.Url.PathAndQuery;
                    oControl = (Control)LoadControl("/controls/sys/sys_login.ascx");
                    PH3.Controls.Add(oControl);
                }
                else
                {
                    intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
                    oPage = new Pages(intProfile, dsn);
                    oApplication = new Applications(intProfile, dsn);
                    oPageControl = new PageControls(intProfile, dsn);
                    oAppPage = new AppPages(intProfile, dsn);
                    oDesigner = new Designer(intProfile, dsn);
                    oControl = (Control)LoadControl("/controls/sys/sys_topnav.ascx");
                    PH1.Controls.Add(oControl);
                    int intApplication = 0;
                    int intPage = 0;
                    if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                        intApplication = Int32.Parse(Request.QueryString["applicationid"]);
                    if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                        intPage = Int32.Parse(Request.QueryString["pageid"]);
                    if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                        intApplication = Int32.Parse(Request.Cookies["application"].Value);
                    string strVariables = "";
                    foreach (string strName in Request.QueryString)
                    {
                        if (strName != "pageid" && strName != "apppageid")
                            strVariables += (strVariables == "" ? "?" : "&") + strName + "=" + Request.QueryString[strName];
                    }
                    if (intPage > 0)
                        strPage = oPage.GetFullLink(intPage).Substring(1) + strVariables;
                    if (intApplication > 0 || intPage > 0)
                    {
                        if (intApplication > 0)
                        {
                            ds = oApplication.Get(intApplication);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                this.Page.Title = "ClearView | " + ds.Tables[0].Rows[0]["name"].ToString();
                            }
                        }
                        if (intPage > 0)
                        {
                            this.Page.Title = "ClearView | " + oPage.Get(intPage, "browsertitle");
                            // Load Page Controls
                            ds = oPageControl.GetPage(intPage, 1);
                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                oControl = (Control)LoadControl("/controls/sys/sys_pages.ascx");
                                PH3.Controls.Add(oControl);
                            }
                            else
                            {
                                ContentPlaceHolder oPlaceHolder;
                                oPlaceHolder = (ContentPlaceHolder)Master.FindControl("AllContent");
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    Control oTempControl = oPlaceHolder.FindControl(dr["placeholder"].ToString());
                                    if (oTempControl != null)
                                    {
                                        oControl = LoadControl(Request.ApplicationPath + dr["path"].ToString());
                                        oTempControl.Controls.Add(oControl);
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Load User's Home Page Controls
                            oControl = (Control)LoadControl("/controls/sys/sys_personal.ascx");
                            PH3.Controls.Add(oControl);
                            //oControl = (Control)LoadControl("/controls/sys/sys_new.ascx");
                            oControl = (Control)LoadControl("/controls/sys/sys_whatsnew.ascx");
                            PH3.Controls.Add(oControl);
                            ds = oDesigner.Get(intProfile, 1);
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                oControl = (Control)LoadControl(dr["path"].ToString());
                                PH3.Controls.Add(oControl);
                            }
                        }
                    }
                    else
                    {
                        // Load Available Applications
                        oControl = (Control)LoadControl("/controls/sys/sys_application.ascx");
                        PH3.Controls.Add(oControl);
                    }
                }
            }
        }
    }
}
