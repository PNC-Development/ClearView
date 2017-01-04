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
    public partial class resource_request_view_mine : System.Web.UI.UserControl
    {
        private DataSet ds;
        protected Pages oPage;
        protected AppPages oAppPage;
        protected Applications oApplication;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequest"]);
        protected string strRedirect = "";
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected int intRecords = 20;
        protected int intRecordStart = 1;
        protected ServiceRequests oServiceRequest;
        
        protected Users oUser;
        protected int intDraft = 0;
        protected int intSubmitted = 0;
        protected int intCancelled = 0;
        protected int intProgress = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
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
            ds = oServiceRequest.GetMine(intProfile);
            LoadPaging(Int32.Parse(lblPage.Text), lblSort.Text);
            //}
            //catch
            //{
            //    LoadPaging(1, "");
            //}
            btnDelete.Attributes.Add("onclick", "return ValidateStringItems('" + hdnDelete.ClientID + "','Please select at least one service request to delete') && confirm('Are you sure you want to delete the selected service request(s)?');");
            btnDelete.Enabled = false;
        }
        private void LoopRepeater(string _sort, int _start)
        {
            ds.Relations.Add("relationship", ds.Tables[0].Columns["requestid"], ds.Tables[1].Columns["requestid"], false);
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
            foreach (RepeaterItem ri in rptView.Items)
            {
                bool boolDraft = false;
                Label _lblCompleted = (Label)ri.FindControl("lblCompleted");
                Label _lblRejected = (Label)ri.FindControl("lblRejected");
                Label _lblCheckout = (Label)ri.FindControl("lblCheckout");
                if (_lblCompleted.Text != "")
                {
                    _lblCheckout.Text = "Pending Approval";
                    _lblCheckout.CssClass = "waiting";
                }
                else if (_lblRejected.Text != "")
                {
                    _lblCheckout.Text = "Project Rejected";
                    _lblCheckout.CssClass = "denied";
                }
                else
                {
                    switch (_lblCheckout.Text)
                    {
                        case "-2":
                            _lblCheckout.Text = "Cancelled";
                            _lblCheckout.CssClass = "denied";
                            break;
                        case "-1":
                            _lblCheckout.Text = "Pending";
                            _lblCheckout.CssClass = "pending";
                            break;
                        case "0":
                            _lblCheckout.Text = "Draft";
                            _lblCheckout.CssClass = "pending";
                            boolDraft = true;
                            break;
                        case "1":
                            _lblCheckout.Text = "Submitted";
                            _lblCheckout.CssClass = "approved";
                            break;
                        case "2":
                            _lblCheckout.Text = "In Progress";
                            _lblCheckout.CssClass = "default";
                            break;
                        case "3":
                            _lblCheckout.Text = "Completed";
                            _lblCheckout.CssClass = "shelved";
                            break;
                    }
                }

                //Check for if any service item is awaiting client response
                string strStatus = "";
                Repeater oRptServices = (Repeater)ri.FindControl("rptServices");
                if (oRptServices != null)
                {
                    foreach (RepeaterItem riRptServices in oRptServices.Items)
                    {
                        Label _lblRRStatus = (Label)riRptServices.FindControl("lblRRStatus");
                        if (_lblRRStatus != null)
                        {
                            if (_lblRRStatus.Text == "-1")
                            {
                                _lblCheckout.Text = "Denied";
                                _lblCheckout.CssClass = "denied";
                            }
                            if (_lblRRStatus.Text == "7")
                            {
                                _lblCheckout.Text = "Awaiting Client Response";
                                _lblCheckout.CssClass = "default";
                            }
                        }
                        Label _lblApproved = (Label)riRptServices.FindControl("lblApproved");
                        if (_lblApproved != null)
                        {
                            if (_lblApproved.Text == "-1")
                            {
                                _lblCheckout.Text = "Denied";
                                _lblCheckout.CssClass = "denied";
                            }
                            if (_lblApproved.Text == "0" && boolDraft == false)
                            {
                                _lblCheckout.Text = "Pending Approval";
                                _lblCheckout.CssClass = "pending";
                            }
                        }
                    }
                }
            
            }
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
            Response.Redirect(oPage.GetFullLink(intPage) + "?sort=" + strOrder + "&page=" + strPage);
        }
        protected void btnPage_Click(Object Sender, ImageClickEventArgs e)
        {
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
                strOrder = Request.QueryString["sort"];
            Response.Redirect(oPage.GetFullLink(intPage) + "?sort=" + strOrder + "&page=" + txtPage.Text);
        }
        protected void LoadPaging(int intStart, string _sort)
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
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            string strHidden = Request.Form[hdnDelete.UniqueID];
            while (strHidden != "")
            {
                string strField = strHidden.Substring(0, strHidden.IndexOf("&"));
                strHidden = strHidden.Substring(strHidden.IndexOf("&") + 1);
                string strFlag = strField.Substring(strField.IndexOf("_") + 1);
                strField = strField.Substring(0, strField.IndexOf("_"));
                if (strFlag == "1")
                    oServiceRequest.DeleteAll(Int32.Parse(strField));
            }
            string strSort = "";
            if (Request.QueryString["sort"] != null)
                strSort = Request.QueryString["sort"];
            string strPage = "1";
            if (Request.QueryString["page"] != null)
                strPage = Request.QueryString["page"];
            Response.Redirect(oPage.GetFullLink(intPage) + "?sort=" + strSort + "&page=" + strPage);
        }
    }
}