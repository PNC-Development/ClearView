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
using System.Collections.Generic;
namespace NCC.ClearView.Presentation.Web
{
    public partial class workload_mine : System.Web.UI.UserControl
    {

        private DataSet ds;
        protected Pages oPage;
        protected AppPages oAppPage;
        protected Applications oApplication;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected ResourceRequest oResourceRequest;
        protected string strRedirect = "";
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected int intRecords = 20;
        protected int intRecordStart = 1;
        protected Users oUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oAppPage = new AppPages(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
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
            if (Request.QueryString["request"] != null && Request.QueryString["request"] != "")
                txtRequest.Text = Request.QueryString["request"];
            lblTopPaging.Text = "";
            //try
            //{
            if (!IsPostBack)
            {
                int intPaging = 0;
                Int32.TryParse(lblPage.Text, out intPaging);
                if (intPaging == 0)
                    intPaging = 1;
                LoadPaging(intPaging, lblSort.Text);
            }
            //}
            //catch
            //{
            //    LoadPaging(1, "");
            //}
            ddlType.Attributes.Add("onchange", "LoadWait();");
            btnGo.Attributes.Add("onclick", "ProcessButton(this) && LoadWait();");
            txtRequest.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnGo.ClientID + "').click();return false;}} else {return true}; ");
        }

        protected int IsNewWorkflowRequest(DataRowView dr)
        {
            int intIsNewWorkflowRequest = 0;
            if (dr["newwindow"].ToString() == "1")
            {
                int intRRId = oResourceRequest.GetWorkflowParent(Int32.Parse(dr["Id"].ToString()));
                DataSet dsRR = oResourceRequest.Get(intRRId);
                int intRequest = Int32.Parse(dsRR.Tables[0].Rows[0]["requestid"].ToString());
                int intService = Int32.Parse(dsRR.Tables[0].Rows[0]["serviceid"].ToString());
                int intNumber = Int32.Parse(dsRR.Tables[0].Rows[0]["number"].ToString());

                DataSet dsRRReturn;

                //Check if this request was returned & re-submitted again(WITHOUT service workflow)
                dsRRReturn = oResourceRequest.getResourceRequestReturn(intRRId, intService, intNumber, 0, 1);
                if (dsRRReturn.Tables[0].Rows.Count > 0)
                    if (DateTime.Parse(dr["modified"].ToString()) > DateTime.Parse(dsRRReturn.Tables[0].Rows[0]["modified"].ToString()))
                        dsRRReturn.Tables[0].Rows.Clear();

                //Check if this request is Returned by Service Workflow(WITH service workflow)
                if (dsRRReturn.Tables[0].Rows.Count == 0)
                    dsRRReturn = oResourceRequest.getResourceRequestReturn(intRRId, intService, intNumber, 1, 0);
                if (dsRRReturn.Tables[0].Rows.Count > 0)
                    if (DateTime.Parse(dr["modified"].ToString()) > DateTime.Parse(dsRRReturn.Tables[0].Rows[0]["modified"].ToString()))
                        dsRRReturn.Tables[0].Rows.Clear();


                //Check if this request was returned and re-submitted by Service Workflow(Workload Manager)
                if (dsRRReturn.Tables[0].Rows.Count == 0)
                {
                    Services oService = new Services(0, dsn);
                    DataSet dsReceive = oService.GetWorkflowsReceive(intService);
                    if (dsReceive.Tables[0].Rows.Count > 0)
                    {
                        int intPreService = Int32.Parse(dsReceive.Tables[0].Rows[0]["serviceid"].ToString());

                        if (intPreService != 0) //Request came from service workflow
                        {
                            DataSet dsPreRR = oResourceRequest.GetRequestService(intRequest, intPreService, intNumber);
                            if (dsPreRR.Tables[0].Rows.Count > 0)
                            {

                                int intPreRRId = Int32.Parse(dsPreRR.Tables[0].Rows[0]["parent"].ToString());
                                int intPreRRItem = Int32.Parse(dsPreRR.Tables[0].Rows[0]["itemid"].ToString());
                                int intPreNumber = Int32.Parse(dsPreRR.Tables[0].Rows[0]["number"].ToString());

                                dsRRReturn = oResourceRequest.getResourceRequestReturn(intPreRRId, intPreService, intPreNumber, 1, 1);
                            }
                        }
                    }
                }

                if (dsRRReturn.Tables[0].Rows.Count > 0)
                    if (DateTime.Parse(dr["modified"].ToString()) > DateTime.Parse(dsRRReturn.Tables[0].Rows[0]["modified"].ToString()))
                        dsRRReturn.Tables[0].Rows.Clear();





                if (dsRRReturn.Tables[0].Rows.Count > 0)
                {
                    if ((dr["modified"].ToString() != "") &&
                        dsRRReturn.Tables[0].Rows[0]["modified"].ToString() != "")
                        if (DateTime.Parse(dr["modified"].ToString()) <= DateTime.Parse(dsRRReturn.Tables[0].Rows[0]["modified"].ToString()))
                            intIsNewWorkflowRequest = 1;
                }
                else
                {
                    if ((dr["created"].ToString() != "") && dr["modified"].ToString() != "")
                        if (dr["created"].ToString() == dr["modified"].ToString())
                            intIsNewWorkflowRequest = 1;
                }

            }
            return intIsNewWorkflowRequest;
        }
        protected void LoopRepeater(string _sort, int _start)
        {
            if (_start > ds.Tables[0].Rows.Count)
                _start = 0;
            intRecordStart = _start + 1;

            //Check if new request
            DataColumn oColumn;
            oColumn = new DataColumn();
            oColumn.DataType = System.Type.GetType("System.String");
            oColumn.ColumnName = "new";
            ds.Tables[0].Columns.Add(oColumn);
            DataView dv = ds.Tables[0].DefaultView;
            
            if (Request.QueryString["sort"] != null)
            {
                try
                {
                    dv.Sort = Request.QueryString["sort"];
                }
                catch { }
            }
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

            foreach (DataRowView dr in dv)
            {
                dr["new"] = IsNewWorkflowRequest(dr).ToString();

            }

            rptView.DataSource = dv;
            rptView.DataBind();
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            foreach (RepeaterItem ri in rptView.Items)
            {
                Label _number = (Label)ri.FindControl("lblNumber");
                if (_number.Text == "")
                    _number.Text = "<i>To Be Determined...</i>";
                Label _status = (Label)ri.FindControl("lblColor");
                string strStatus = _status.Text;
                int intGreen = 0;
                Int32.TryParse(strStatus.Substring(0, strStatus.IndexOf("_")), out intGreen);
                strStatus = strStatus.Substring(strStatus.IndexOf("_") + 1);
                int intYellow = 0;
                Int32.TryParse(strStatus.Substring(0, strStatus.IndexOf("_")), out intYellow);
                strStatus = strStatus.Substring(strStatus.IndexOf("_") + 1);
                int intRed = 0;
                Int32.TryParse(strStatus, out intRed);
                strStatus = "<table cellpadding=\"0\" cellspacing=\"2\" border=\"0\">";
                if (intRed > 0)
                    strStatus += "<tr><td>" + oFunction.GetBox("FF0000", 15, 8) + "</td>" + (intRed > 1 ? "<td> (" + intRed.ToString() + ")</td>" : "") + "</tr>";
                if (intYellow > 0)
                    strStatus += "<tr><td>" + oFunction.GetBox("FFFF00", 15, 8) + "</td>" + (intYellow > 1 ? "<td> (" + intYellow.ToString() + ")</td>" : "") + "</tr>";
                if (intGreen > 0)
                    strStatus += "<tr><td>" + oFunction.GetBox("00FF00", 15, 8) + "</td>" + (intGreen > 1 ? "<td> (" + intGreen.ToString() + ")</td>" : "") + "</tr>";
                strStatus += "</table>";
                _status.Text = strStatus;
                // Actual Status
                Label lblStatus = (Label)ri.FindControl("lblStatus");
                int intRRID = Int32.Parse(lblStatus.Text);
                List<WorkflowStatus> RR = oResourceRequest.GetStatus(null, intRRID, null, null, null, null, false, dsnServiceEditor);
                if (RR.Count > 0)
                    lblStatus.Text = RR[0].status;
                else
                    lblStatus.Text = "---";
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
            string strFilter = "";
            if (Request.QueryString["filter"] != null)
                strFilter = "&filter=" + Request.QueryString["filter"];
            if (Request.QueryString["page"] != null)
                strPage = Request.QueryString["page"];
            string strRequest = "";
            if (Request.QueryString["request"] != null)
                strRequest = "&request=" + Request.QueryString["request"];
            Response.Redirect(oPage.GetFullLink(intPage) + "?sort=" + strOrder + "&page=" + strPage + strFilter + strRequest);
        }
        protected void btnPage_Click(Object Sender, ImageClickEventArgs e)
        {
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
                strOrder = Request.QueryString["sort"];
            string strFilter = "";
            if (Request.QueryString["filter"] != null)
                strFilter = "&filter=" + Request.QueryString["filter"];
            string strRequest = "";
            if (Request.QueryString["request"] != null)
                strRequest = "&request=" + Request.QueryString["request"];
            Response.Redirect(oPage.GetFullLink(intPage) + "?sort=" + strOrder + "&page=" + txtPage.Text + strFilter + strRequest);
        }
        protected void LoadPaging(int intStart, string _sort)
        {
            string strRequest = (Request.QueryString["request"] == null ? "" : Request.QueryString["request"]);
            if (Request.QueryString["filter"] != null && Request.QueryString["filter"] == "buddy")
            {
                ds = oResourceRequest.GetWorkflowAssigned(intProfile, 0, 1, strRequest);
                ddlType.SelectedValue = "2";
            }
            else if (Request.QueryString["filter"] != null && Request.QueryString["filter"] == "all")
            {
                ds = oResourceRequest.GetWorkflowAssigned(intProfile, 1, 1, strRequest);
                ddlType.SelectedValue = "1";
            }
            else
                ds = oResourceRequest.GetWorkflowAssigned(intProfile, 1, 0, strRequest);
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
            if (_number == _start)
                _label.Text += "<b><font style=\"color:#CC0000\">" + _number.ToString() + "</font></b>";
            else
            {
                string strSort = "";
                if (Request.QueryString["sort"] != null)
                    strSort = Request.QueryString["sort"];
                string strFilter = "";
                if (Request.QueryString["filter"] != null)
                    strFilter = "&filter=" + Request.QueryString["filter"];
                string strRequest = "";
                if (Request.QueryString["request"] != null)
                    strRequest = "&request=" + Request.QueryString["request"];
                _label.Text += "<a href=\"" + oPage.GetFullLink(intPage) + "?sort=" + strSort + "&page=" + _number.ToString() + strFilter + strRequest + "\" title=\"Go to Page " + _number.ToString() + "\">" + _number.ToString() + "</a>";
            }
            if (_spacer != "")
                _label.Text += _spacer;
        }
        protected void ddlType_Change(Object Sender, EventArgs e)
        {
            string strSort = "";
            if (Request.QueryString["sort"] != null)
                strSort = Request.QueryString["sort"];
            string strPage = "";
            if (Request.QueryString["page"] != null)
                strPage = Request.QueryString["page"];
            string strRequest = "";
            if (Request.QueryString["request"] != null)
                strRequest = "&request=" + Request.QueryString["request"];
            if (strPage == "")
                strPage = "1";
            if (ddlType.SelectedIndex == 1)
                Response.Redirect(oPage.GetFullLink(intPage) + "?page=" + strPage + "&sort=" + strSort + "&filter=all" + strRequest);
            else if (ddlType.SelectedIndex == 2)
                Response.Redirect(oPage.GetFullLink(intPage) + "?page=" + strPage + "&sort=" + strSort + "&filter=buddy" + strRequest);
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?page=" + strPage + "&sort=" + strSort + "&filter=mine" + strRequest);
        }
        protected void btnGo_Click(Object Sender, EventArgs e)
        {
            string strSort = "";
            if (Request.QueryString["sort"] != null)
                strSort = Request.QueryString["sort"];
            string strFilter = "";
            if (Request.QueryString["filter"] != null)
                strFilter = "&filter=" + Request.QueryString["filter"];
            string strPage = "";
            if (Request.QueryString["page"] != null)
                strPage = Request.QueryString["page"];
            if (strPage == "")
                strPage = "1";
            Response.Redirect(oPage.GetFullLink(intPage) + "?page=" + strPage + "&sort=" + strSort + strFilter + "&request=" + txtRequest.Text);
        }
    }
}