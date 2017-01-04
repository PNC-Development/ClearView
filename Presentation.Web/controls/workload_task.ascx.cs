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
    public partial class workload_task : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Applications oApplication;
        protected Pages oPage;
        protected ResourceRequest oResourceRequest;
        protected Users oUser;
        protected Requests oRequest;
        protected int intApplication = 0;
        protected int intPage = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oApplication = new Applications(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
                panFinish.Visible = true;
            else
            {
                panTask.Visible = true;
                LoadList(intProfile, intApplication);
                LoadAvailable(intProfile, intApplication);
            }
            imgStart.Attributes.Add("onclick", "return ShowCalendar('" + txtStart.ClientID + "');");
            imgEnd.Attributes.Add("onclick", "return ShowCalendar('" + txtEnd.ClientID + "');");
            btnSubmit.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a task name')" +
                " && ValidateText('" + txtDescription.ClientID + "','Please enter a statement of work')" +
                " && ValidateNumber('" + txtHours.ClientID + "','Please enter a valid number for the hours allocated')" +
                " && ValidateDate('" + txtStart.ClientID + "','Please enter a valid start date')" +
                " && ValidateDate('" + txtEnd.ClientID + "','Please enter a valid end date')" +
                " && ValidateDropDown('" + ddlUser.ClientID + "','Please make a selection for the technician assigned')" +
                ";");
            lnkAvailable.Attributes.Add("onclick", "return ShowHideAvailable('" + divAvailable.ClientID + "');");
        }
        protected void LoadList(int _appmanager, int _app)
        {
            DataSet ds = oUser.GetManagerReports(intProfile, 0, 0, 0);
            ddlUser.DataValueField = "userid";
            ddlUser.DataTextField = "username";
            ddlUser.DataSource = ds;
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void LoadAvailable(int _appmanager, int _app)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("name", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("hours", System.Type.GetType("System.Double")));
            dt.Columns.Add(new DataColumn("graph", System.Type.GetType("System.Double")));
            DataSet ds = oUser.GetManagerReports(intProfile, 0, 0, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intUser = Int32.Parse(dr["userid"].ToString());
                DataSet dsAss = oResourceRequest.GetWorkflowAssigned(intUser, 2);
                double dblTotal = 0;
                foreach (DataRow drAss in dsAss.Tables[0].Rows)
                {
                    int intId = Int32.Parse(drAss["id"].ToString());
                    double dblAllocated = double.Parse(drAss["allocated"].ToString());
                    double dblUsed = oResourceRequest.GetWorkflowUsed(intId);
                    dblTotal += dblAllocated - dblUsed;
                }
                DataRow drRow = dt.NewRow();
                drRow["name"] = oUser.GetFullName(intUser);
                drRow["hours"] = dblTotal;
                dt.Rows.Add(drRow);
            }
            DataSet dsNew = new DataSet();
            dsNew.Tables.Add(dt);
            double dblMax = 0;
            foreach (DataRow dr in dsNew.Tables[0].Rows)
            {
                double dblHours = double.Parse(dr["hours"].ToString());
                if (dblMax < dblHours)
                    dblMax = dblHours;
            }
            if (dblMax > 0)
            {
                foreach (DataRow dr in dsNew.Tables[0].Rows)
                {
                    double dblHours = double.Parse(dr["hours"].ToString());
                    dblHours = dblHours / dblMax;
                    dr["graph"] = dblHours * 400;
                }
            }
            rptAvailable.DataSource = dsNew;
            rptAvailable.DataBind();
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            int intRequest = oRequest.AddTask(0, intProfile, txtDescription.Text, DateTime.Parse(txtStart.Text), DateTime.Parse(txtEnd.Text));
            int intWorkflowParent = oResourceRequest.Add(intRequest, -1, 0, 0, txtName.Text, 0, double.Parse(txtHours.Text), 2, 1, 1, 1);
            int intWorkflowResource = oResourceRequest.AddWorkflow(intWorkflowParent, 0, txtName.Text, Int32.Parse(ddlUser.SelectedItem.Value), 0, double.Parse(txtHours.Text), 2, 0);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
    }
}