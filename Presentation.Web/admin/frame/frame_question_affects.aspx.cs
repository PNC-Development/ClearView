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
    public partial class frame_question_affects : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected Forecast oForecast;
        protected Platforms oPlatform;
        protected int intQuestion;
        protected int intAffected;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oForecast = new Forecast(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            btnClose.Attributes.Add("onclick", "return HidePanel();");
            if (Request.QueryString["qid"] != null && Request.QueryString["qid"] != "" && Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
            {
                intQuestion = Int32.Parse(Request.QueryString["qid"]);
                intAffected = Int32.Parse(Request.QueryString["aid"]);
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
            lblAffected.Text = oForecast.GetQuestion(intAffected, "name");
            DataSet ds = oForecast.GetAffects(intQuestion, intAffected);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlState.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
                lblId.Text = ds.Tables[0].Rows[0]["id"].ToString();
            }
            else
                btnDelete.Enabled = false;
        }
        protected  void btnSave_Click(Object Sender, EventArgs e)
        {
            if (lblId.Text == "")
                oForecast.AddAffects(intQuestion, intAffected, Int32.Parse(ddlState.SelectedItem.Value));
            else
                oForecast.UpdateAffects(Int32.Parse(lblId.Text), Int32.Parse(ddlState.SelectedItem.Value));
            Reload();
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oForecast.DeleteAffects(Int32.Parse(lblId.Text));
            Reload();
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">ReloadAdminFrame();<" + "/" + "script>");
        }
    }
}
