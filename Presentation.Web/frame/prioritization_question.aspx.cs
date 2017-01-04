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
    public partial class prioritization_question : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected Reports oReport;
        protected ProjectRequest oRequest;
        protected Pages oPage;
        protected int intProfile;
        protected int intQuestionId;
        protected DataSet ds;
        protected string strBase;
        protected int intOrganizationId;
        protected bool boolAdd;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oReport = new Reports(intProfile, dsn);
            oRequest = new ProjectRequest(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            if (Request.QueryString["questionid"] != "" && Request.QueryString["questionid"] != null)
                intQuestionId = Int32.Parse(Request.QueryString["questionid"]);

            if (Request.QueryString["oid"] != null && Request.QueryString["oid"] != "")
                intOrganizationId = Int32.Parse(Request.QueryString["oid"]);

            if (Request.QueryString["bd"] != null && Request.QueryString["bd"] != "")
                strBase = Request.QueryString["bd"];


            boolAdd = Request.QueryString["type"] == "add" ? true : false;

            if (!IsPostBack)
            {
                btnUpdate.Text = boolAdd == true ? "Add" : "Update";
                btnDelete.Visible = boolAdd == false;
                drpClass.DataSource = oRequest.GetClasses(1);
                drpClass.DataTextField = "name";
                drpClass.DataValueField = "id";
                drpClass.DataBind();
                ds = oRequest.GetQuestion(intQuestionId);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtQuestion.Text = ds.Tables[0].Rows[0]["question"].ToString();
                    hdnID.Value = ds.Tables[0].Rows[0]["display"].ToString();

                    chkRequired.Checked = ds.Tables[0].Rows[0]["required"].ToString().Trim() == "1" ? true : false;
                    chkEnabled.Checked = ds.Tables[0].Rows[0]["enabled"].ToString().Trim() == "1" ? true : false;
                    DataSet ds2 = oRequest.GetQuestionsClass(intQuestionId);
                    if (ds2.Tables[0].Rows.Count > 0)
                        drpClass.SelectedValue = ds2.Tables[0].Rows[0]["classid"].ToString();

                }

                hdnRequired.Value = chkRequired.Checked.ToString().ToLower();
                hdnEnabled.Value = chkEnabled.Checked.ToString().ToLower();

            }



            btnUpdate.Attributes.Add("onclick", "return ValidateText('" + txtQuestion.ClientID + "','Please enter a question'); ");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this question?');");
            chkRequired.Attributes.Add("onclick", "return chkHidden('" + chkRequired.ClientID + "','" + hdnRequired.ClientID + "');");
            chkEnabled.Attributes.Add("onclick", "return chkHidden('" + chkEnabled.ClientID + "','" + hdnEnabled.ClientID + "');");

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (boolAdd)
            {
                string strDisplay = oRequest.MaximumProjectRequestQuestions();
                int intDisplay = 0;
                if (strDisplay != "") intDisplay = Int32.Parse(strDisplay);
                int intQuestionID = oRequest.AddQuestion(txtQuestion.Text, txtQuestion.Text, intDisplay + 1, (hdnEnabled.Value.ToString() == "true" ? 1 : 0), (hdnRequired.Value.ToString() == "true" ? 1 : 0));
                oRequest.AddQuestionsClass(intQuestionID, Int32.Parse(drpClass.SelectedValue));
                oRequest.AddQA(strBase, intOrganizationId, intQuestionID);
            }
            else
            {
                oRequest.UpdateQuestionsClass(intQuestionId, Int32.Parse(drpClass.SelectedValue));
                oRequest.UpdateQuestion(intQuestionId, txtQuestion.Text.Trim(), txtQuestion.Text.Trim(), (hdnEnabled.Value.ToString() == "true" ? 1 : 0), (hdnRequired.Value.ToString() == "true" ? 1 : 0));
            }
            Redirect();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            oRequest.DeleteQuestion(intQuestionId);
            Redirect();
        }

        private void Redirect()
        {
            ClientScript.RegisterClientScriptBlock(typeof(Page), "redirect", "window.top.navigate(window.top.location);", true);
        }
    }
}
