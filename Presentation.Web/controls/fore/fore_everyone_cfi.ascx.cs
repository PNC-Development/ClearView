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
    public partial class fore_everyone_cfi : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Pages oPage;
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ForecastEdit"]);
        protected string strRedirect = "";
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected int intRecords = 20;
        protected int intRecordStart = 1;
        protected Classes oClass;
        protected Design oDesign;
        protected LinkButton oButton;
        protected string strTotals = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oClass = new Classes(intProfile, dsn);
            oDesign = new Design(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            strRedirect = oPage.GetFullLink(intViewPage);
            lblTitle.Text = oPage.Get(intPage, "title");
            lblPage.Text = "1";
            lblSort.Text = "";
            if (Request.QueryString["page"] != null && Request.QueryString["page"] != "")
                lblPage.Text = Request.QueryString["page"];
            if (Request.QueryString["sort"] != null && Request.QueryString["sort"] != "")
                lblSort.Text = Request.QueryString["sort"];
            lblTopPaging.Text = "";
            LoadPaging(Int32.Parse(lblPage.Text), lblSort.Text);
        }
        private void LoadLists(DataView dv)
        {
            string[] strDistinct = new string[2] { "managerid", "manager" };
            DataTable dt = dv.ToTable(true, new string[2] { "managerid", "manager" });
            DataView dvTemp = dt.DefaultView;
            dvTemp.Sort = "manager";
            //DataTable dt = new DataTable();
            //DataColumn dc1 = new DataColumn("managerid", System.Type.GetType("System.Int32"));
            //DataColumn dc2 = new DataColumn("manager", System.Type.GetType("System.String"));
            //dt.Columns.Add(dc1);
            //dt.Columns.Add(dc2);
            //foreach (DataRowView dr in dv) 
            //{
            //    DataRow drNew = new DataRow();
            //    drNew["managerid"] = dr["managerid"].ToString();
            //    drNew["manager"] = dr["manager"].ToString();
            //    dt.Rows.Add(drNew);
            //}
            ddlFilterRequester.DataValueField = "managerid";
            ddlFilterRequester.DataTextField = "manager";
            ddlFilterRequester.DataSource = dt;
            ddlFilterRequester.DataBind();
            ddlFilterRequester.Items.Insert(0, new ListItem("-- ALL --", "0"));
            if (Request.QueryString["filter_1"] != null)
                txtFilterName.Text = Request.QueryString["filter_1"];
            if (Request.QueryString["filter_2"] != null)
                txtFilterNumber.Text = Request.QueryString["filter_2"];
            if (Request.QueryString["filter_3"] != null)
                txtFilterDesignID.Text = Request.QueryString["filter_3"];
            if (Request.QueryString["filter_4"] != null)
               if (ddlFilterRequester.Items.FindByValue(Request.QueryString["filter_4"])!=null)
                    ddlFilterRequester.SelectedValue = Request.QueryString["filter_4"];
               else
                    ddlFilterRequester.Items.FindByValue("0").Selected=true;
        }
        private void LoopRepeater(string _sort, int _start)
        {
            //DataSet ds = oForecast.Gets();
            DataSet ds = oDesign.Gets();
            double dblCount1 = 0;

            DataColumn dc1 = new DataColumn("implementation", System.Type.GetType("System.String"));
            ds.Tables[0].Columns.Add(dc1);
            DataColumn dc2 = new DataColumn("quantity", System.Type.GetType("System.Int32"));
            ds.Tables[0].Columns.Add(dc2);
            DataColumn dc3 = new DataColumn("amp", System.Type.GetType("System.Double"));
            ds.Tables[0].Columns.Add(dc3);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intForecast = Int32.Parse(dr["id"].ToString());
                //DataSet dsAnswers = oForecast.GetAnswers(intForecast);
                DataSet dsAnswers = oDesign.Gets(intForecast);
                DateTime _date = DateTime.MaxValue;
                foreach (DataRow drAnswer in dsAnswers.Tables[0].Rows)
                {
                    //bool boolOverride = (dr["override"].ToString() == "1");
                    if (drAnswer["commitment"].ToString() != "")
                    {
                        DateTime _commitment = DateTime.Parse(drAnswer["commitment"].ToString());
                        if (_commitment < _date)
                            _date = _commitment;
                    }
                    double dblQuantity = 0.00;
                    double.TryParse(drAnswer["quantity"].ToString(), out dblQuantity);
                    int intClass = 0;
                    if (Int32.TryParse(drAnswer["classid"].ToString(), out intClass) == true && oClass.IsProd(intClass))
                    {
                        double dblRecovery = 0.00;
                        double.TryParse(drAnswer["quantity"].ToString(), out dblRecovery);
                        dblQuantity += dblRecovery;
                    }
                    dblCount1 += dblQuantity;
                }
                //dblCount2 += dblCount1;
                if (_date == DateTime.MaxValue)
                    dr["implementation"] = "N / A";
                else
                    dr["implementation"] = _date.ToShortDateString();
                dr["quantity"] = dblCount1.ToString();
                dblCount1 = 0.00;
            }



            if (_start > ds.Tables[0].Rows.Count)
                _start = 0;
            intRecordStart = _start + 1;
            DataView dv = ds.Tables[0].DefaultView;
            if (Request.QueryString["sort"] != null)
                dv.Sort = Request.QueryString["sort"];
            dv.RowFilter = LoadFilter();
            LoadLists(dv);
            int intCount = _start + intRecords;
            if (dv.Count < intCount)
                intCount = dv.Count;
            int ii = 0;
            lblRecords.Text = "Models " + intRecordStart.ToString() + " - " + intCount.ToString() + " of " + dv.Count.ToString();
            for (ii = 0; ii < _start && ii < intCount; ii++)
                dv[0].Delete();
            int intTotalCount = (dv.Count - intRecords);
            for (ii = 0; ii < intTotalCount; ii++)
                dv[intRecords].Delete();
            rptView.DataSource = dv;
            rptView.DataBind();
            Projects oProject = new Projects(intProfile, dsn);
            Requests oRequest = new Requests(intProfile, dsn);
            ProjectsPending oProjectPending = new ProjectsPending(intProfile, dsn, intEnvironment);
            Users oUser = new Users(intProfile, dsn);
            StatusLevels oStatusLevel = new StatusLevels();
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
            Response.Redirect(oPage.GetFullLink(intPage) + "?sort=" + strOrder + "&page=" + strPage + GetFilter());
        }
        protected void btnPage_Click(Object Sender, ImageClickEventArgs e)
        {
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
                strOrder = Request.QueryString["sort"];
            Response.Redirect(oPage.GetFullLink(intPage) + "?sort=" + strOrder + "&page=" + txtPage.Text + GetFilter());
        }
        private void LoadPaging(int intStart, string _sort)
        {
            //DataSet ds = oForecast.Gets();
            DataSet ds = oDesign.Gets();
            DataView dv = ds.Tables[0].DefaultView;
            dv.RowFilter = LoadFilter();

            int intCount = dv.Count;
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
            /*
            // LOAD PLATFORMS
            Platforms oPlatform = new Platforms(intProfile, dsn);
            DataSet dsPlatforms = oPlatform.GetForecasts(1);
            foreach (DataRow dr in dsPlatforms.Tables[0].Rows)
                strTotals += "<td class=\"default\">" + (dr["image"].ToString() != "" ? "<img src=\"" + dr["image"].ToString() + "\" border=\"0\" align=\"absmiddle\" />" : "") + " <b>" + oForecast.GetPlatformCount(Int32.Parse(dr["platformid"].ToString())) + "</b> " + dr["name"].ToString() + "</td><td class=\"default\">&nbsp;</td>";
            strTotals = "<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\"><tr>" + strTotals + "</tr></table>";
            */
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
                sb.Append("?sort=");
                sb.Append(strSort);
                sb.Append("&page=");
                sb.Append(_number.ToString());
                sb.Append(GetFilter());
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
        private string LoadFilter()
        {
            string strFilter = "";
            if (Request.QueryString["filter_1"] != null && Request.QueryString["filter_1"] != "")
            {
                if (strFilter != "")
                    strFilter += " AND ";
                strFilter += "name" + " LIKE '" + Request.QueryString["filter_1"] + "%'";
            }
            if (Request.QueryString["filter_2"] != null && Request.QueryString["filter_2"] != "")
            {
                if (strFilter != "")
                    strFilter += " AND ";
                strFilter += "number" + " LIKE '" + Request.QueryString["filter_2"] + "%'";
            }
            if (Request.QueryString["filter_3"] != null && Request.QueryString["filter_3"] != "")
            {
                string strFilter3 = "";
                int intFilter3 = 0;
                Int32.TryParse(Request.QueryString["filter_3"], out intFilter3);
                DataSet dsFilter3 = oDesign.GetsSearch(intFilter3);
                if (dsFilter3.Tables[0].Rows.Count == 1)
                    Response.Redirect(strRedirect + "?id=" + dsFilter3.Tables[0].Rows[0]["id"].ToString() + "&highlight=" + dsFilter3.Tables[0].Rows[0]["designid"].ToString());
                else
                {
                    foreach (DataRow drFilter3 in dsFilter3.Tables[0].Rows)
                    {
                        if (strFilter3 != "")
                            strFilter3 += ",";
                        strFilter3 += drFilter3["id"].ToString();
                    }
                }
                if (strFilter3 != "")
                {
                    if (strFilter != "")
                        strFilter += " AND ";
                    strFilter += "id" + " IN (" + strFilter3 + ")";
                }
            }
            if (Request.QueryString["filter_4"] != null && Request.QueryString["filter_4"] != "0")
            {
                if (strFilter != "")
                    strFilter += " AND ";
                strFilter += "managerid" + " = " + Request.QueryString["filter_4"];
            }
            return strFilter;
        }
        protected void btnFilter_Click(Object Sender, EventArgs e)
        {
            string strSort = "";
            if (Request.QueryString["sort"] != null)
                strSort = Request.QueryString["sort"];
            string strPage = "";
            if (Request.QueryString["page"] != null)
                strPage = Request.QueryString["page"];
            string strFilter = "";
            strFilter += "&filter_1=" + txtFilterName.Text;
            strFilter += "&filter_2=" + txtFilterNumber.Text;
            strFilter += "&filter_3=" + txtFilterDesignID.Text;
            strFilter += "&filter_4=" + ddlFilterRequester.SelectedItem.Value;
            Response.Redirect(oPage.GetFullLink(intPage) + "?sort=" + strSort + "&page=" + strPage + strFilter);
        }
        private string GetFilter()
        {
            string strFilter = "";
            if (Request.QueryString["filter_1"] != null)
                strFilter += "&filter_1=" + Request.QueryString["filter_1"];
            if (Request.QueryString["filter_2"] != null)
                strFilter += "&filter_2=" + Request.QueryString["filter_2"];
            if (Request.QueryString["filter_3"] != null)
                strFilter += "&filter_3=" + Request.QueryString["filter_3"];
            if (Request.QueryString["filter_4"] != null)
                strFilter += "&filter_4=" + Request.QueryString["filter_4"];
            return strFilter;
        }
    }
}