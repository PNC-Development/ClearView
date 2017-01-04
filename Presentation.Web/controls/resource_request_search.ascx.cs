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
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;
namespace NCC.ClearView.Presentation.Web
{
    public partial class resource_request_search : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intWorkload = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intWorkloadManager = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkloadManager"]);
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected Pages oPage;
        protected ServiceDetails oServiceDetail;
        protected Services oService;
        protected Applications oApplication;
        protected RequestItems oRequestItem;
        protected string strView = "";
        protected int intCount = 0;
        protected int intZero = 0;
        protected int intOne = 1;
        protected string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oServiceDetail = new ServiceDetails(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "" && Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
                LoadSearch(Int32.Parse(Request.QueryString["aid"]), Request.QueryString["sid"]);
            else
            {
                panSearch.Visible = true;
                lblTitle.Text = "Search";
                if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                {
                    int _applicationid = Int32.Parse(Request.QueryString["aid"]);
                    if (Request.QueryString["iid"] != null && Request.QueryString["iid"] != "")
                    {
                        int _itemid = Int32.Parse(Request.QueryString["iid"]);
                        LoadDepartments();
                        ddlDepartments.SelectedValue = _applicationid.ToString();
                        LoadGroups(_applicationid);
                        LoadDeliverables(_applicationid);
                        ddlGroups.SelectedValue = _itemid.ToString();
                        panCategory.Visible = true;
                        LoadServices(_applicationid, _itemid);
                    }
                    else
                    {
                        LoadDepartments();
                        LoadDeliverables(_applicationid);
                        ddlDepartments.SelectedValue = _applicationid.ToString();
                        if (oApplication.Get(_applicationid, "service_search_items") == "1")
                        {
                            LoadSubServices(_applicationid);
                            lstServices.Enabled = false;
                            btnSearch.Enabled = false;
                            lstServices.Items.Insert(0, new ListItem("Please select a category", "0"));
                        }
                        else
                            LoadServices(_applicationid, 0);
                    }
                }
                else
                {
                    LoadDepartments();
                    lstServices.Enabled = false;
                    btnSearch.Enabled = false;
                    hypDeliverable.Enabled = false;
                    lstServices.Items.Insert(0, new ListItem("Please select a department", "0"));
                }
            }
            hypClear.NavigateUrl = oPage.GetFullLink(intPage);
        }
        private void LoadDepartments()
        {
            DataSet ds = oApplication.GetApplicationsServices(intZero, intZero);
            ddlDepartments.DataValueField = "applicationid";
            ddlDepartments.DataTextField = "name";
            ddlDepartments.DataSource = ds;
            ddlDepartments.DataBind();
            ddlDepartments.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void LoadGroups(int _applicationid)
        {
            DataSet ds = oRequestItem.GetItemActivities(_applicationid);
            ddlGroups.DataValueField = "itemid";
            ddlGroups.DataTextField = "name";
            ddlGroups.DataSource = ds;
            ddlGroups.DataBind();
            ddlGroups.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void LoadDeliverables(int _applicationid)
        {
            string strDeliverable = oApplication.Get(_applicationid, "deliverables_doc");
            if (strDeliverable != "")
            {
                hypDeliverable.Enabled = true;
                hypDeliverable.NavigateUrl = strDeliverable;
            }
            else
                hypDeliverable.Enabled = false;
        }
        protected void ddlDepartments_Change(Object Sender, EventArgs e)
        {
            if (ddlDepartments.SelectedItem.Value == "0")
                Response.Redirect(oPage.GetFullLink(intPage));
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?aid=" + ddlDepartments.SelectedItem.Value);
        }
        protected void ddlGroups_Change(Object Sender, EventArgs e)
        {
            if (ddlGroups.SelectedItem.Value == "0")
                Response.Redirect(oPage.GetFullLink(intPage) + "?aid=" + Request.QueryString["aid"]);
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?aid=" + Request.QueryString["aid"] + "&iid=" + ddlGroups.SelectedItem.Value);
        }
        protected void btnSearch_Click(Object Sender, EventArgs e)
        {
            string strServices = "";
            for (int ii = 0; ii < lstServices.Items.Count; ii++)
            {
                if (lstServices.Items[ii].Selected == true)
                    strServices += lstServices.Items[ii].Value + " ";
            }
            if (strServices == "")
            {
                if (panCategory.Visible == true)
                    Response.Redirect(oPage.GetFullLink(intPage) + "?aid=" + ddlDepartments.SelectedItem.Value + "&iid=" + ddlGroups.SelectedItem.Value);
                else
                    Response.Redirect(oPage.GetFullLink(intPage) + "?aid=" + ddlDepartments.SelectedItem.Value);
            }
            else
            {
                if (panCategory.Visible == true)
                    Response.Redirect(oPage.GetFullLink(intPage) + "?aid=" + ddlDepartments.SelectedItem.Value + "&iid=" + ddlGroups.SelectedItem.Value + "&sid=" + strServices);
                else
                    Response.Redirect(oPage.GetFullLink(intPage) + "?aid=" + ddlDepartments.SelectedItem.Value + "&sid=" + strServices);
            }
        }
        private void LoadSubServices(int _applicationid)
        {
            panCategory.Visible = true;
            btnSearch.Enabled = false;
            LoadDepartments();
            LoadGroups(_applicationid);
            ddlDepartments.SelectedValue = _applicationid.ToString();
        }
        private void LoadServices(int _applicationid, int _itemid)
        {
            btnSearch.Enabled = true;
            DataSet ds = oApplication.GetApplicationsServices(_applicationid, _itemid);
            lstServices.DataValueField = "serviceid";
            lstServices.DataTextField = "name";
            lstServices.DataSource = ds;
            lstServices.DataBind();
        }
        private void LoadSearch(int _applicationid, string _services)
        {
            panResult.Visible = true;
            btnPrinter.Attributes.Add("onclick", "return OpenWindow('SERVICES_DETAIL','?sid=" + _services + "');");
            btnPDF.Attributes.Add("onclick", "return OpenWindow('SERVICES_DETAIL_PDF','?sid=" + _services + "');");
            btnPDF.Enabled = false;
            lblTitle.Text = "Search Results";
            string[] strServices;
            char[] strSplit = { ' ' };
            strServices = _services.Split(strSplit);
            for (int ii = 0; ii < strServices.Length; ii++)
            {
                if (strServices[ii].Trim() != "")
                {
                    int intService = Int32.Parse(strServices[ii]);
                    strView += "<tr><td class=\"header\">" + oService.GetName(intService) + "</td></tr>";
                    strView += "<tr><td>";
                    string strSummary = GetSummary(intService, 0, 1.00, -1, 1);
                    if (strSummary != "")
                        strView += strSummary;
                    else
                        strView += "<img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\"> There is no information available on this service.";
                    strView += "</td></tr>";
                    strView += "<tr><td>&nbsp;</td></tr>";
                }
            }
            strView = "<table width=\"100%\" cellpadding=\"2\" cellspacing=\"2\" border=\"0\" class=\"default\">" + strView + "</table>";
        }
        protected string GetSummary(int _serviceid, int _id, double _quantity, int _state, int _level)
        {
            StringBuilder sb = new StringBuilder();
            DataSet ds = oServiceDetail.Gets(_serviceid, _id, 1);
            double dblTotalHours = 0.00;
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intID = Int32.Parse(dr["id"].ToString());
                    intCount++;
                    if (sb.ToString() != "")
                    {
                        sb.Append(strSpacerRow);
                    }
                    sb.Append("<tr>");
                    DataSet dsServices = oServiceDetail.Gets(_serviceid, intID, 1);
                    if (dsServices.Tables[0].Rows.Count == 0)
                    {
                        sb.Append("<td valign=\"top\">&nbsp;</td>");
                    }
                    else
                    {
                        if (_state == 0 || _state == 1)
                        {
                            sb.Append("<td valign=\"top\"><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgSummary_");
                            sb.Append(intCount.ToString());
                            sb.Append("','divSummary_");
                            sb.Append(intCount.ToString());
                            sb.Append("');\"><img id=\"imgSummary_");
                            sb.Append(intCount.ToString());
                            sb.Append("\" src=\"/images/minus.gif\" border=\"0\" align=\"absmiddle\"></a>&nbsp;</td>");
                        }
                        else
                        {
                            sb.Append("<td valign=\"top\"><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgSummary_");
                            sb.Append(intCount.ToString());
                            sb.Append("','divSummary_");
                            sb.Append(intCount.ToString());
                            sb.Append("');\"><img id=\"imgSummary_");
                            sb.Append(intCount.ToString());
                            sb.Append("\" src=\"/images/plus.gif\" border=\"0\" align=\"absmiddle\"></a>&nbsp;</td>");
                        }
                    }
                    sb.Append("<td width=\"100%\" valign=\"top\">");
                    sb.Append(dr["name"].ToString());
                    sb.Append("</td>");
                    double dblSingle = 0.00;
                    double dblAdditional = 0.00;
                    string strX = "";
                    string strTotal = "";
                    for (double ii = 0.00; ii < _quantity; ii = ii + 1.00)
                    {
                        if (ii == 0.00)
                            dblSingle += oServiceDetail.GetDetailHours(_serviceid, intID, false);
                        else
                            dblAdditional += oServiceDetail.GetDetailHours(_serviceid, intID, true);
                    }
                    double dblHours = dblSingle + dblAdditional;
                    bool boolAdditional = false;
                    dblSingle = 0.00;
                    dblAdditional = 0.00;
                    for (double ii = 0.00; ii < _quantity; ii = ii + 1.00)
                    {
                        if (ii == 0.00)
                            dblSingle += oServiceDetail.GetDetailHours(_serviceid, intID, false);
                        else
                        {
                            dblAdditional = oServiceDetail.GetDetailHours(_serviceid, intID, true);
                            if (dblSingle != dblAdditional)
                            {
                                boolAdditional = true;
                                double _new_quantity = _quantity - 1.00;
                                double _new_hours = dblHours - dblSingle;
                                strX = dblSingle.ToString("F") + " HRs x 1 Qty = <br/>" + dblAdditional.ToString("F") + " HRs x " + _new_quantity.ToString() + " Qty = ";
                                strTotal = dblSingle.ToString("F") + " HRs<br/>" + _new_hours.ToString("F") + " HRs";
                            }
                            break;
                        }
                    }
                    if (boolAdditional == false)
                    {
                        strX = dblSingle.ToString("F") + " HRs x " + _quantity.ToString() + " Qty = ";
                        strTotal = dblHours.ToString("F") + " HRs";
                    }
                    sb.Append("<td nowrap valign=\"top\" align=\"right\">");
                    sb.Append(strX);
                    sb.Append("</td>");
                    sb.Append("<td nowrap align=\"right\" valign=\"top\">");
                    sb.Append(strTotal);
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append(strSpacerRow);
                    sb.Append("<tr><td></td>");
                    if (_state == 0 || _state == 1)
                    {
                        sb.Append("<td colspan=\"3\" valign=\"top\" id=\"divSummary_");
                        sb.Append(intCount.ToString());
                        sb.Append("\" style=\"display:inline\">");
                        sb.Append(GetSummary(_serviceid, intID, _quantity, _state, _level + 1));
                        sb.Append("</td>");
                    }
                    else
                    {
                        sb.Append("<td colspan=\"3\" valign=\"top\" id=\"divSummary_");
                        sb.Append(intCount.ToString());
                        sb.Append("\" style=\"display:none\">");
                        sb.Append(GetSummary(_serviceid, intID, _quantity, _state, _level + 1));
                        sb.Append("</td>");
                    }
                    sb.Append("</tr>");
                    dblTotalHours += dblHours;
                }
                sb.Append(strSpacerRow);
                double dblSLA = double.Parse(oService.Get(_serviceid, "sla"));
                sb.Append("<tr>");
                sb.Append("<td colspan=\"4\" align=\"right\" style=\"color:#990000\">(SLA: ");
                sb.Append(dblSLA.ToString("F"));
                sb.Append(" HRs)&nbsp;&nbsp;<b>TOTAL: ");
                sb.Append(dblTotalHours.ToString("F"));
                sb.Append(" HRs</b></td>");
                sb.Append("</tr>");
                sb.Append(strSpacerRow);
            }

            if (sb.ToString() != "")
            {
                if (_level == 1)
                {
                    return "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">" + sb.ToString() + "</table>";
                }
                else if (_level == 2)
                {
                    return "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">" + sb.ToString() + "</table>";
                }
                else
                {
                    return "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">" + sb.ToString() + "</table>";
                }
            }
            else
            {
                return sb.ToString(); ;
            }
        }
    }
}