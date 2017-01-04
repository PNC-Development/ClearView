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
    public partial class project_request_view_all : System.Web.UI.UserControl
    {

        private DataSet ds;
        protected Pages oPage;
        protected AppPages oAppPage;
        protected Applications oApplication;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewRequest"]);
        protected string strRedirect = "";
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected int intRecords = 20;
        protected int intRecordStart = 1;
        protected ProjectRequest oProjectRequest;
        protected Users oUser;
        protected char[] charsToTrim = { '%' };
        protected string strFilters = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oProjectRequest = new ProjectRequest(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oAppPage = new AppPages(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
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
            //try
            //{
            LoadPaging(Int32.Parse(lblPage.Text), lblSort.Text);
            //}
            //catch
            //{
            //    LoadPaging(1, "");
            //}
        }
        protected void LoopRepeater(string _sort, int _start)
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
            if (dv.Count > 0)
            {
                for (ii = 0; ii < _start; ii++)
                    dv[0].Delete();
            }
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
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
            {
                if (Request.QueryString["sort"] == oOrder.CommandArgument)
                    strOrder = oOrder.CommandArgument + " DESC";
            }
            if (strOrder == "")
                strOrder = oOrder.CommandArgument;
            string strPage = "";
            if (Request.QueryString["page"] != null)
                strPage = Request.QueryString["page"];
            string strFilter = "";
            if (Request.QueryString["filter"] != null)
                strFilter = Request.QueryString["filter"];
            Response.Redirect(oPage.GetFullLink(intPage) + "?sort=" + strOrder + "&page=" + strPage + "&filter=" + strFilter);
        }
        protected void btnPage_Click(Object Sender, ImageClickEventArgs e)
        {
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
                strOrder = Request.QueryString["sort"];
            string strFilter = "";
            if (Request.QueryString["filter"] != null)
                strFilter = Request.QueryString["filter"];
            Response.Redirect(oPage.GetFullLink(intPage) + "?sort=" + strOrder + "&page=" + txtPage.Text + "&filter=" + strFilter);
        }
        protected void LoadPaging(int intStart, string _sort)
        {
            int intDeleted = 0;
            ds = oProjectRequest.Gets(1);
            if (Request.QueryString["filter"] != null && Request.QueryString["filter"] != "")
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["status"].ToString() != Request.QueryString["filter"])
                    {
                        dr.Delete();
                        intDeleted++;
                    }
                }
            }
            int intCount = ds.Tables[0].Rows.Count - intDeleted;
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
            // LOAD STATUS HEADERS
            string strPage = "";
            if (Request.QueryString["page"] != null)
                strPage = Request.QueryString["page"];
            string strSort = "";
            if (Request.QueryString["sort"] != null)
                strSort = Request.QueryString["sort"];

            StringBuilder sb = new StringBuilder(strFilters);

            sb.Append("<td title=\"Pending\"><a class=\"pending\" href=\"");
            sb.Append(oPage.GetFullLink(intPage));
            sb.Append("?sort=");
            sb.Append(strSort);
            sb.Append("&page=");
            sb.Append(1);
            sb.Append("&filter=0\" title=\"Filter: Pending\"><img src=\"/images/pending.gif\" border=\"0\" align=\"absmiddle\" /> ");
            sb.Append(oProjectRequest.GetApproval(0).Tables[0].Rows.Count.ToString());
            sb.Append(" Pending</a></td>");
            sb.Append("<td class=\"header\">&nbsp;</td>");

            sb.Append("<td title=\"Approved\"><a class=\"approved\" href=\"");
            sb.Append(oPage.GetFullLink(intPage));
            sb.Append("?sort=");
            sb.Append(strSort);
            sb.Append("&page=");
            sb.Append(1);
            sb.Append("&filter=1\" title=\"Filter: Approved\"><img src=\"/images/approved.gif\" border=\"0\" align=\"absmiddle\" /> ");
            sb.Append(oProjectRequest.GetApproval(1).Tables[0].Rows.Count.ToString());
            sb.Append(" Approved</a></td>");
            sb.Append("<td class=\"header\">&nbsp;</td>");

            sb.Append("<td title=\"Denied\"><a class=\"denied\" href=\"");
            sb.Append(oPage.GetFullLink(intPage));
            sb.Append("?sort=");
            sb.Append(strSort);
            sb.Append("&page=");
            sb.Append(1);
            sb.Append("&filter=-1\" title=\"Filter: Denied\"><img src=\"/images/denied.gif\" border=\"0\" align=\"absmiddle\" /> ");
            sb.Append(oProjectRequest.GetApproval(-1).Tables[0].Rows.Count.ToString());
            sb.Append(" Denied</a></td>");
            sb.Append("<td class=\"header\">&nbsp;</td>");

            sb.Append("<td title=\"Shelved\"><a class=\"shelved\" href=\"");
            sb.Append(oPage.GetFullLink(intPage));
            sb.Append("?sort=");
            sb.Append(strSort);
            sb.Append("&page=");
            sb.Append(1);
            sb.Append("&filter=10\" title=\"Filter: Shelved\"><img src=\"/images/shelved.gif\" border=\"0\" align=\"absmiddle\" /> ");
            sb.Append(oProjectRequest.GetApproval(10).Tables[0].Rows.Count.ToString());
            sb.Append(" Shelved</a></td>");
            sb.Append("<td class=\"header\">&nbsp;</td>");

            sb.Append("<td title=\"Active\"><a class=\"approved\" href=\"");
            sb.Append(oPage.GetFullLink(intPage));
            sb.Append("?sort=");
            sb.Append(strSort);
            sb.Append("&page=");
            sb.Append(1);
            sb.Append("&filter=2\" title=\"Filter: Active\"><img src=\"/images/active.gif\" border=\"0\" align=\"absmiddle\" /> ");
            sb.Append(oProjectRequest.GetApproval(2).Tables[0].Rows.Count.ToString());
            sb.Append(" Active</a></td>");
            sb.Append("<td class=\"header\">&nbsp;</td>");

            sb.Append("<td title=\"Cancelled\"><a class=\"denied\" href=\"");
            sb.Append(oPage.GetFullLink(intPage));
            sb.Append("?sort=");
            sb.Append(strSort);
            sb.Append("&page=");
            sb.Append(1);
            sb.Append("&filter=-2\" title=\"Filter: Cancelled\"><img src=\"/images/cancel.gif\" border=\"0\" align=\"absmiddle\" /> ");
            sb.Append(oProjectRequest.GetApproval(-2).Tables[0].Rows.Count.ToString());
            sb.Append(" Cancelled</a></td>");
            sb.Append("<td class=\"header\">&nbsp;</td>");

            sb.Append("<td title=\"On Hold\"><a class=\"hold\" href=\"");
            sb.Append(oPage.GetFullLink(intPage));
            sb.Append("?sort=");
            sb.Append(strSort);
            sb.Append("&page=");
            sb.Append(1);
            sb.Append("&filter=5\" title=\"Filter: On Hold\"><img src=\"/images/hold.gif\" border=\"0\" align=\"absmiddle\" /> ");
            sb.Append(oProjectRequest.GetApproval(5).Tables[0].Rows.Count.ToString());
            sb.Append(" On Hold</a></td>");
            sb.Append("<td class=\"header\">&nbsp;</td>");

            sb.Append("<td title=\"Completed\"><a class=\"shelved\" href=\"");
            sb.Append(oPage.GetFullLink(intPage));
            sb.Append("?sort=");
            sb.Append(strSort);
            sb.Append("&page=");
            sb.Append(1);
            sb.Append("&filter=3\" title=\"Filter: Completed \"><img src=\"/images/completed.gif\" border=\"0\" align=\"absmiddle\" /> ");
            sb.Append(oProjectRequest.GetApproval(3).Tables[0].Rows.Count.ToString());
            sb.Append(" Completed</a></td>");

            strFilters = sb.ToString();
        }
        protected void LoadLink(Label _label, int _number, string _spacer, int _start)
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

                string strFilter = "";
                if (Request.QueryString["filter"] != null)
                {
                    strFilter = Request.QueryString["filter"];
                }

                sb.Append("<a href=\"");
                sb.Append(oPage.GetFullLink(intPage));
                sb.Append("?sort=");
                sb.Append(strSort);
                sb.Append("&page=");
                sb.Append(_number.ToString());
                sb.Append("&filter=");
                sb.Append(strFilter);
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
    }
}