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
    public partial class idc_resource_assignement : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Customized oCustom;
        protected Variables oVariable;
        protected int intProfile;
        protected string path;
        protected DataSet dsResource;
        protected int intRequest;
        protected int intItem;
        protected int intNumber;
        protected int intId;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oCustom = new Customized(intProfile, dsn);
            oVariable = new Variables(intEnvironment);

            if (Request.QueryString["save"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">window.top.refreshIE();<" + "/" + "script>");
            if (Request.QueryString["rid"] != "" && Request.QueryString["rid"] != null)
                intRequest = Int32.Parse(Request.QueryString["rid"]);
            if (Request.QueryString["iid"] != "" && Request.QueryString["iid"] != null)
                intItem = Int32.Parse(Request.QueryString["iid"]);
            if (Request.QueryString["num"] != "" && Request.QueryString["num"] != null)
                intNumber = Int32.Parse(Request.QueryString["num"]);
            if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                intId = Int32.Parse(Request.QueryString["id"]);
            if (!IsPostBack)
            {
                drpResourceRequired.DataSource = oCustom.GetResourceTypes(1);
                drpResourceRequired.DataTextField = "name";
                drpResourceRequired.DataValueField = "id";
                drpResourceRequired.DataBind();
                drpResourceRequired.Items.Insert(0, "-- SELECT --");
                if (intId > 0)
                {
                    btnUpdate.Visible = true;
                    btnAdd.Visible = false;
                    dsResource = oCustom.GetResourceAssignment(intId);
                    if (dsResource != null)
                    {
                        if (dsResource.Tables[0].Rows.Count > 0)
                        {
                            txtRequestedBy.Text = dsResource.Tables[0].Rows[0]["requestedby"].ToString();
                            txtDateRequested.Text = Convert.ToDateTime(dsResource.Tables[0].Rows[0]["requesteddate"].ToString()).ToShortDateString();
                            txtFulfillDate.Text = Convert.ToDateTime(dsResource.Tables[0].Rows[0]["fulfilldate"].ToString()).ToShortDateString();
                            txtResourceAssigned.Text = dsResource.Tables[0].Rows[0]["resourceassigned"].ToString();
                            txtStatus.Text = dsResource.Tables[0].Rows[0]["status"].ToString();
                            drpResourceRequired.SelectedValue = dsResource.Tables[0].Rows[0]["resourcetypeid"].ToString();
                        }
                    }
                }
            }

            btnAdd.Attributes.Add("onclick", "return ValidateDropDown('" + drpResourceRequired.ClientID + "','Please make a selection for Resource Type')" +
              "&& ValidateText('" + txtRequestedBy.ClientID + "','Please enter a valid name for Requested By')" +
              "&& ValidateDate('" + txtDateRequested.ClientID + "','Please enter a valid requested date')" +
              "&& ValidateDate('" + txtFulfillDate.ClientID + "','Please enter a valid fulfill date')" +
              "&& ValidateText('" + txtResourceAssigned.ClientID + "','Please enter a valid name for Resource Assigned')" +
              "&& ValidateText('" + txtStatus.ClientID + "','Please enter a valid status')" +
               ";");

            imgDateRequested.Attributes.Add("onclick", "return OpenCalendar('" + txtDateRequested.ClientID + "');");
            imgFulfillDate.Attributes.Add("onclick", "return OpenCalendar('" + txtFulfillDate.ClientID + "');");

            txtRequestedBy.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','" + hdnManager.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtResourceAssigned.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX2.ClientID + "','" + lstAJAX2.ClientID + "','" + hdnManager.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstAJAX2.Attributes.Add("ondblclick", "AJAXClickRow();");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            oCustom.AddResourceAssignment(intRequest, intItem, intNumber, Int32.Parse(drpResourceRequired.SelectedValue), txtRequestedBy.Text, Convert.ToDateTime(txtDateRequested.Text), Convert.ToDateTime(txtFulfillDate.Text), txtResourceAssigned.Text, txtStatus.Text);
            Response.Redirect(Request.Path + "?save=true");
        }

        protected void btnUpdate_Click(object sender, CommandEventArgs e)
        {
            string strRequestedBy = txtRequestedBy.Text;
            string strDateRequested = txtDateRequested.Text;
            string strFulfilDate = txtFulfillDate.Text;
            string strResourceAssigned = txtResourceAssigned.Text;
            string strStatus = txtStatus.Text;
            oCustom.UpdateResourceAssignement(intId, Int32.Parse(drpResourceRequired.SelectedValue), strRequestedBy, Convert.ToDateTime(strDateRequested), Convert.ToDateTime(strFulfilDate), strResourceAssigned, strStatus);
            Response.Redirect(Request.Path + "?save=true");
        }
    }
}
