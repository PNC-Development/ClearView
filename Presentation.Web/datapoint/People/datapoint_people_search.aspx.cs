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

namespace NCC.ClearView.Presentation.Web
{
    public partial class datapoint_people_search : System.Web.UI.Page
    {
        public event DataListItemEventHandler ItemDataBound;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private int intDataPointAvailablePeople = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_PEOPLE"]);
        private DataPoint oDataPoint;
        private Functions oFunction;
        private Users oUser;
        private Variables oVariable;
        private Pages oPage;
        private Groups oGroup;
       
        private int intProfile = 0;
        private int intApplication = 0;
        private bool boolAdvSearch = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Page.Title = "DataPoint | People Search";
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            oDataPoint = new DataPoint(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oUser = new Users(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oPage = new Pages(intProfile, dsn);
            oGroup = new Groups(intProfile, dsn);

            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "PEOPLE") == true || intDataPointAvailablePeople == 1))
            {
                pnlAllow.Visible = true;

                if (Request.QueryString["advanced"] == null)
                {
                    pnlBasicSearch.Visible = true;
                    pnlAdvancedSearch.Visible = false;
                    lnkbtnAdvancedSearch.Visible = true;
                    lnkbtnBasicSearch.Visible = false;
                    btnAdvancedSearch.Visible = false;
                    btnBasicSearch.Visible = true;
                }
                else
                {
                    boolAdvSearch = true;
                    pnlBasicSearch.Visible = true;
                    pnlAdvancedSearch.Visible = true;
                    lnkbtnAdvancedSearch.Visible = false;
                    lnkbtnBasicSearch.Visible = true;
                    btnAdvancedSearch.Visible = true;
                    btnBasicSearch.Visible = false;

                }
                if (!IsPostBack)
                    LoadList();

                AddControlAttributes();
            }
            else
                pnlDenied.Visible = true;
        }

        private void LoadList()
        {

            ddlStatus.Items.Insert(0, new ListItem("--All--", "-1"));
            ddlStatus.Items.Insert(1, new ListItem("Out Of Office", "1"));

            DataSet ds = oGroup.Gets(1);
            ddlDepartment.DataValueField = "groupid";
            ddlDepartment.DataTextField = "name";
            ddlDepartment.DataSource = ds;
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new ListItem("-- SELECT --", "-1"));

           

        }

        private void AddControlAttributes()
        {
            //Basic Search
            if (boolAdvSearch == false)
            {
                txtFName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnBasicSearch.ClientID + "').click();return false;}} else {return true}; ");
                txtLName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnBasicSearch.ClientID + "').click();return false;}} else {return true}; ");
                btnBasicSearch.Attributes.Add("onclick", "return ValidateSearchControls()" +
                " && ProcessButton(this)" +
                ";");
            }
            //Advance Search
            else
            {
                txtFName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnAdvancedSearch.ClientID + "').click();return false;}} else {return true}; ");
                txtLName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnAdvancedSearch.ClientID + "').click();return false;}} else {return true}; ");

              
                txtManager.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divManager.ClientID + "','" + lstManager.ClientID + "','" + hdnManager.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstManager.Attributes.Add("ondblclick", "AJAXClickRow();");

                btnAdvancedSearch.Attributes.Add("onclick", "return ValidateSearchControls()" +
                " && ProcessButton(this)" +
                ";");
            }
            lnkbtnClearHistory.Attributes.Add("onclick", "ClearSearchCriteria();");

        }
        protected void btnBasicSearch_Click(object sender, EventArgs e)
        {
           
            hdnPageNo.Value = "1";
            hdnRecsPerPage.Value = ddlResultsPerPage.SelectedItem.Value;
            BindControls();
        }

        protected void btnAdvancedSearch_Click(object sender, EventArgs e)
        {
            hdnPageNo.Value = "1";
            hdnRecsPerPage.Value = ddlResultsPerPage.SelectedItem.Value;
            BindControls();

           
        }

        protected void lnkbtnClearHistory_Click(object sender, EventArgs e)
        {

        }

        protected void lnkbtnBasicSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }

        protected void lnkbtnAdvancedSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?advanced=true");
        }

        protected void dlPeople_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                HtmlImage imgResource = (HtmlImage)e.Item.FindControl("imgResource");
                imgResource.Src = "/frame/picture.aspx?xid=" + drv["XID"];

                LinkButton lnkLANId = (LinkButton)e.Item.FindControl("lnkLANId");
                lnkLANId.Text = drv["xid"].ToString();
                lnkLANId.ToolTip = "User Id: " + drv["UserID"].ToString();
                lnkLANId.Attributes.Add("onclick", "return OpenNewWindowMenu('/datapoint/people/datapoint_resource.aspx?id=" + oFunction.encryptQueryString(drv["UserID"].ToString()) + "', '800', '600');");


                Label lblFName = (Label)e.Item.FindControl("lblFName");
                lblFName.Text = drv["Fname"].ToString();

                Label lblLName = (Label)e.Item.FindControl("lblLName");
                lblLName.Text = drv["Lname"].ToString();

                Label lblPhone = (Label)e.Item.FindControl("lblPhone");
                lblPhone.Text = drv["Phone"].ToString();

                Label lblEmail = (Label)e.Item.FindControl("lblEmail");
                //lblEmail.Text = drv["Phone"].ToString();

                Label lblPager = (Label)e.Item.FindControl("lblPager");
                lblPager.Text = drv["Pager"].ToString();

                Label lblDepartment = (Label)e.Item.FindControl("lblDepartment");
                lblDepartment.Text = drv["ApplicationName"].ToString();

                Label lblOutOfOffice = (Label)e.Item.FindControl("lblOutOfOffice");
                lblOutOfOffice.Text = drv["OutOfOfficeStatus"].ToString();


            }
        }

        private void BindControls()
        {
            pnlResults.Visible = true;
            DataSet ds;
            if (boolAdvSearch == true)
                ds = oDataPoint.GetPeopleSearchResults(
                                null,
                                txtFName.Text.Trim(),
                                txtLName.Text.Trim(),
                                txtLANID.Text.Trim(),
                                ((ddlStatus.SelectedValue.ToString() != "-1") ? Int32.Parse(ddlStatus.SelectedValue.ToString()) : (int?)null),
                                ((hdnManager.Value != "") ? Int32.Parse(hdnManager.Value) : (int?)null),
                                ((ddlDepartment.SelectedValue.ToString() != "-1") ? Int32.Parse(ddlDepartment.SelectedValue.ToString()) : (int?)null),
                                hdnOrderBy.Value.ToString(), Int32.Parse(hdnOrder.Value.ToString()), Int32.Parse(hdnPageNo.Value), Int32.Parse(hdnRecsPerPage.Value));

            else
                ds = oDataPoint.GetPeopleSearchResults(
                                 null,
                                 txtFName.Text.Trim(),
                                 txtLName.Text.Trim(),
                                 "",
                                 null,
                                 null,
                                 null,
                                 hdnOrderBy.Value.ToString(), Int32.Parse(hdnOrder.Value.ToString()), Int32.Parse(hdnPageNo.Value), Int32.Parse(hdnRecsPerPage.Value));

            dlPeople.DataSource = ds.Tables[0];
            dlPeople.DataBind();

            if (ds.Tables[0].Rows.Count > 0)
            {
                long lngDisplayRecords = Int64.Parse(ds.Tables[0].Rows[0]["rownum"].ToString()) + Int64.Parse((ds.Tables[0].Rows.Count - 1).ToString());
                lblRecords.Text = "Showing Results <b>" + ds.Tables[0].Rows[0]["rownum"].ToString() + " - " + lngDisplayRecords.ToString() + "</b> of <b>" + ds.Tables[1].Rows[0]["TotalRecords"].ToString() + "</b>...";
            }
            else
                lblRecords.Text = "No Results Found...";

            // Calculate total numbers of pages
            long lngRecsPerPage = Int64.Parse(hdnRecsPerPage.Value);
            if (lngRecsPerPage != 0)
            {
                long lngTotalRecsCount = Int64.Parse(ds.Tables[1].Rows[0]["TotalRecords"].ToString());
                long lngPgCount = lngTotalRecsCount / lngRecsPerPage + ((lngTotalRecsCount % lngRecsPerPage) > 0 ? 1 : 0);
                if (Int32.Parse(hdnRecsPerPage.Value) != 0)
                {
                    // Display Next button
                    if (lngPgCount - 1 >= Convert.ToInt64(hdnPageNo.Value))
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                    // Display Prev button
                    if ((Convert.ToInt64(hdnPageNo.Value)) > 1)
                        btnPrevious.Enabled = true;
                    else
                        btnPrevious.Enabled = false;
                }
            }
            else
            {
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
            }
        }

        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            LinkButton oOrder = (LinkButton)Sender;
            if (hdnOrderBy.Value == oOrder.CommandArgument)
            {
                if (hdnOrder.Value == "1")
                    hdnOrder.Value = "0";
                else if (hdnOrder.Value == "0")
                    hdnOrder.Value = "1";
            }
            else
            {
                hdnOrderBy.Value = oOrder.CommandArgument;
                hdnOrder.Value = "0";
            }

            BindControls();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            hdnPageNo.Value = Convert.ToString(Convert.ToInt64(hdnPageNo.Value) + 1);
            BindControls();
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            hdnPageNo.Value = Convert.ToString(Convert.ToInt64(hdnPageNo.Value) - 1);
            BindControls();
        }
    }
}
