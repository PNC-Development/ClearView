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
    public partial class prioritization_response : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Reports oReport;
        protected ProjectRequest oRequest;
        protected Pages oPage;
        protected int intProfile;
        protected int intQuestionId;
        protected int intResponseId;


        protected DataSet ds;
        protected bool boolAdd = false;
        protected int intWtCount;
        protected string strResult;


        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oReport = new Reports(intProfile, dsn);
            oRequest = new ProjectRequest(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);

            if (Request.QueryString["questionid"] != "" && Request.QueryString["questionid"] != null)
                intQuestionId = Int32.Parse(Request.QueryString["questionid"]);

            if (Request.QueryString["responseid"] != "" && Request.QueryString["responseid"] != null)
                intResponseId = Int32.Parse(Request.QueryString["responseid"]);

            if (Request.QueryString["type"] != null && Request.QueryString["type"] != "")
                boolAdd = Request.QueryString["type"].ToLower() == "add" ? true : false;

            if (Request.QueryString["confirm"] != "" && Request.QueryString["confirm"] != null)
            {
                string strName = Request.QueryString["name"] != null && Request.QueryString["name"] != "" ? Request.QueryString["name"] : "";
                string strResponse = Request.QueryString["resp"] != null && Request.QueryString["resp"] != "" ? Request.QueryString["resp"] : "";
                int intWeight = Request.QueryString["wt"] != null && Request.QueryString["wt"] != "" ? Int32.Parse(Request.QueryString["wt"]) : 0;
                int intDisplay = Request.QueryString["disp"] != null && Request.QueryString["disp"] != "" ? Int32.Parse(Request.QueryString["disp"]) : 0;
                int intEnabled = Request.QueryString["enabled"] != null && Request.QueryString["enabled"] != "" ? (Request.QueryString["enabled"] == "True" ? 1 : 0) : 0;
                int intRespID = Request.QueryString["respid"] != null && Request.QueryString["respid"] != "" ? Int32.Parse(Request.QueryString["respid"]) : 0;
                if (boolAdd)
                    oRequest.AddResponse(intQuestionId, strName, strResponse, intWeight, intDisplay, intEnabled);
                else
                    oRequest.UpdateResponse(intRespID, intQuestionId, strName, strResponse, intWeight, intEnabled);

                ClientScript.RegisterClientScriptBlock(typeof(Page), "redirect", "window.top.navigate(window.top.location);", true);
                //ClientScript.RegisterClientScriptBlock(typeof(Page), "redirect", "window.top.navigate('" + oPage.GetFullLink(intPageId) + "?bd=" + Request.QueryString["bd"] + "&oid=" + Request.QueryString["oid"] + "');", true);
            }
            if (!IsPostBack)
            {
                ds = oRequest.GetResponse(intResponseId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtResponse.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtResponse.Text = ds.Tables[0].Rows[0]["response"].ToString();
                    hdnID.Value = ds.Tables[0].Rows[0]["display"].ToString();
                    hdnRespID.Value = ds.Tables[0].Rows[0]["id"].ToString();
                    chkEnabled.Checked = ds.Tables[0].Rows[0]["enabled"].ToString().Trim() == "1" ? true : false;
                    drpWeight.SelectedValue = ds.Tables[0].Rows[0]["weight"].ToString();

                }

            }

            btnUpdate.Text = boolAdd == true ? "Add" : "Update";
            btnDelete.Visible = boolAdd == false;
            hdnEnabled.Value = chkEnabled.Checked.ToString();
            hdnResponse.Value = txtResponse.Text;

            chkEnabled.Attributes.Add("onclick", "return chkHidden('" + chkEnabled.ClientID + "','" + hdnEnabled.ClientID + "');");
            btnUpdate.Attributes.Add("onclick", "return ValidateText('" + txtResponse.ClientID + "','Please enter a response'); ");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this response?');");

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int intDisplay = 0;
            string strDisplay = "";
            strDisplay = oRequest.MaximumProjectRequestResponses(intQuestionId);
            if (strDisplay != "") intDisplay = Int32.Parse(strDisplay);
            int intCurrent = oRequest.ProjectRequestResponsesCount(intQuestionId, Int32.Parse(drpWeight.SelectedValue), intResponseId);
            if (intCurrent == 0)
                intWtCount = oRequest.ProjectRequestResponsesCount(intQuestionId, Int32.Parse(drpWeight.SelectedValue));
            if (intWtCount > 0)
                ClientScript.RegisterStartupScript(typeof(Page), "check", "if(confirm('The selected weight has already been assigned to another question!!Do you still want to assign the same weight?')) { window.navigate(window.location+'" + "&confirm=Y" + "&respid=" + intResponseId + "&name=" + txtResponse.Text + "&resp=" + txtResponse.Text + "&wt=" + drpWeight.SelectedValue + "&disp=" + (intDisplay + 1) + "&enabled=" + (chkEnabled.Checked ? 1 : 0) + "'); }", true);
            else
            {
                if (btnUpdate.Text == "Add")
                {
                    strDisplay = oRequest.MaximumProjectRequestResponses(intQuestionId);
                    if (strDisplay != "") intDisplay = Int32.Parse(strDisplay);
                    oRequest.AddResponse(intQuestionId, txtResponse.Text.Trim(), txtResponse.Text.Trim(), Int32.Parse(drpWeight.SelectedValue), intDisplay + 1, (chkEnabled.Checked ? 1 : 0));
                }
                else
                    oRequest.UpdateResponse(intResponseId, intQuestionId, hdnResponse.Value, hdnResponse.Value, Int32.Parse(drpWeight.SelectedValue), (chkEnabled.Checked ? 1 : 0));

                ClientScript.RegisterClientScriptBlock(typeof(Page), "redirect", "window.top.navigate(window.top.location);", true);
            }

        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            oRequest.DeleteResponse(intResponseId);
            ClientScript.RegisterClientScriptBlock(typeof(Page), "redirect", "window.top.navigate(window.top.location);", true);
        }
    }
}
