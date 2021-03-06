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
    public partial class frame_question_affected : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected Forecast oForecast;
        protected Platforms oPlatform;
        protected int intQuestion;
        protected int intAffected;
        protected int intResponse;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oForecast = new Forecast(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            btnClose.Attributes.Add("onclick", "return HidePanel();");
            if (Request.QueryString["qid"] != null && Request.QueryString["qid"] != "" && Request.QueryString["aid"] != null && Request.QueryString["aid"] != "" && Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
            {
                intQuestion = Int32.Parse(Request.QueryString["qid"]);
                intAffected = Int32.Parse(Request.QueryString["aid"]);
                intResponse = Int32.Parse(Request.QueryString["rid"]);
                if (!IsPostBack)
                    Load();
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this association?');");
            }
            else
                Reload();
        }
        private void Load()
        {
            lblQuestion.Text = oForecast.GetQuestion(intQuestion, "name");
            lblResponse.Text = oForecast.GetResponse(intResponse, "name");
            lblAffected.Text = oForecast.GetQuestion(intAffected, "name");
            DataSet ds = oForecast.GetAffected(intQuestion, intAffected, intResponse);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlState.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
                lblId.Text = ds.Tables[0].Rows[0]["id"].ToString();
            }
            else
                btnDelete.Enabled = false;
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            if (lblId.Text == "")
                oForecast.AddAffected(intQuestion, intAffected, intResponse, Int32.Parse(ddlState.SelectedItem.Value));
            else
                oForecast.UpdateAffected(Int32.Parse(lblId.Text), Int32.Parse(ddlState.SelectedItem.Value));
            Reload();
        }
        protected  void btnDelete_Click(Object Sender, EventArgs e)
        {
            oForecast.DeleteAffected(Int32.Parse(lblId.Text));
            Reload();
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
