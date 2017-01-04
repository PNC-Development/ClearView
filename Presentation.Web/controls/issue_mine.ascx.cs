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
    public partial class issue_mine : System.Web.UI.UserControl
    {

        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected Pages oPage;
        protected ResourceRequest oResourceRequest;
        protected Customized oCustomized;
        protected Variables oVariable;
        protected StatusLevels oStatusLevel;
        protected string strRedirect = "";
        protected string strStatusIds = "";

        protected int intProfile;
        protected int intPage = 0;
        protected int intRequest = 0;
        protected int intRecords = 20;
        protected int intRecordStart = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oStatusLevel = new StatusLevels(intProfile, dsn);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            strRedirect = oPage.GetFullLinkRelated(intPage);
            lblTitle.Text = oPage.Get(intPage, "title");
            lblPage.Text = "1";
            lblSort.Text = "";

            if (Request.QueryString["page"] != null && Request.QueryString["page"] != "")
                lblPage.Text = Request.QueryString["page"];
            if (Request.QueryString["sort"] != null && Request.QueryString["sort"] != "")
                lblSort.Text = Request.QueryString["sort"];
            lblTopPaging.Text = "";

            LoadDefaultValues();
            LoadPaging(Int32.Parse(lblPage.Text), lblSort.Text);


        }

        private void LoadDefaultValues()
        {


            lstStatus.DataValueField = "StatusValue";
            lstStatus.DataTextField = "StatusDescription";
            lstStatus.DataSource = oStatusLevel.GetStatusList("ENHANCEMENTANDISSUESTATUS");
            lstStatus.DataBind();
            lstStatus.Items.Insert(0, new ListItem("-- ALL --", ""));



            txtStart.Text = DateTime.Today.AddYears(-1).ToShortDateString();
            txtEnd.Text = DateTime.Today.ToShortDateString();
            imgStart.Attributes.Add("onclick", "return ShowCalendar('" + txtStart.ClientID + "');");
            imgEnd.Attributes.Add("onclick", "return ShowCalendar('" + txtEnd.ClientID + "');");

            if (Request.QueryString["statusids"] != null)
            {
                strStatusIds = Request.QueryString["statusids"];
                string[] strStatus = strStatusIds.Split(',');
                for (int i = 0; i < strStatus.Length; i++)
                    lstStatus.Items.FindByValue(strStatus[i]).Selected = true;
            }
            else
            {
                strStatusIds = "0,2";
                lstStatus.Items.FindByValue("0").Selected = true; //Under Review
                lstStatus.Items.FindByValue("2").Selected = true; //Under Development
            }
            if (Request.QueryString["reqdatefrom"] != null && Request.QueryString["reqdatefrom"] != "")
                txtStart.Text = DateTime.Parse(Request.QueryString["reqdatefrom"]).ToShortDateString();
            if (Request.QueryString["reqdateto"] != null && Request.QueryString["reqdateto"] != "")
                txtEnd.Text = DateTime.Parse(Request.QueryString["reqdateto"]).ToShortDateString();
        }

        protected void LoopRepeater(string _sort, int _start)
        {
           // ds = oCustomized.GetIssueUser(intProfile);
            ds = oCustomized.GetIssueUser(intProfile, strStatusIds, DateTime.Parse(txtStart.Text.Trim()), DateTime.Parse(txtEnd.Text.Trim()));
            if (_start > ds.Tables[0].Rows.Count)
                _start = 0;
            intRecordStart = _start + 1;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
                intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
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

            Response.Redirect(oPage.GetFullLink(intPage) + "?sort=" + strOrder + "&page=" + strPage +
                                                 "&statusids=" + strStatusIds +
                                                 "&reqdatefrom=" + txtStart.Text +
                                                 "&reqdateto=" + txtEnd.Text);
 
        }

        protected void btnPage_Click(Object Sender, ImageClickEventArgs e)
        {
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
                strOrder = Request.QueryString["sort"];

            Response.Redirect(oPage.GetFullLink(intPage) + "?sort=" + strOrder + "&page=" + txtPage.Text +
                                                                       "&statusids=" + strStatusIds +
                                                                       "&reqdatefrom=" + txtStart.Text +
                                                                       "&reqdateto=" + txtEnd.Text);

        }

        protected void btnFilter_Click(Object Sender, EventArgs e)
        {

            string strOrder = "";
            strStatusIds = "";
            foreach (ListItem oList in lstStatus.Items)
            {
                if (oList.Selected == true)
                {
                    if (strStatusIds == "")
                        strStatusIds = oList.Value;
                    else
                        strStatusIds = strStatusIds + "," + oList.Value;

                }
            }

            Response.Redirect(oPage.GetFullLink(intPage) + "?sort=" + strOrder +
                                                           "&statusids=" + strStatusIds +
                                                           "&reqdatefrom=" + txtStart.Text +
                                                           "&reqdateto=" + txtEnd.Text);
        }

        protected void LoadPaging(int intStart, string _sort)
        {
            //int intCount = oCustomized.GetIssueUser(intProfile).Tables[0].Rows.Count;
            int intCount = oCustomized.GetIssueUser(intProfile, strStatusIds, DateTime.Parse(txtStart.Text.Trim()), DateTime.Parse(txtEnd.Text.Trim())).Tables[0].Rows.Count;
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

                sb.Append("<a href=\"");
                sb.Append(oPage.GetFullLink(intPage));
                sb.Append("?sort=");
                sb.Append(strSort);
                sb.Append("&page=");
                sb.Append(_number.ToString());

                sb.Append("&statusids=");
                sb.Append(strStatusIds);

                sb.Append("&reqdatefrom=");
                sb.Append(txtStart.Text);

                sb.Append("&reqdateto=");
                sb.Append(txtEnd.Text);

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