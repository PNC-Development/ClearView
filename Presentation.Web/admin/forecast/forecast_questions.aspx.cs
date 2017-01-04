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
    public partial class forecast_questions : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Forecast oForecast;
        protected int intProfile;
        protected int intQuestion;
        protected int intAffected;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/forecast/forecast_questions.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oForecast = new Forecast(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intQuestion = Int32.Parse(Request.QueryString["id"]);
                if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                {
                    intAffected = Int32.Parse(Request.QueryString["aid"]);
                    Response.Cookies["loginreferrer"].Value = "/admin/forecast/forecast_questions.aspx?id=" + intQuestion.ToString() + "&aid=" + intAffected.ToString();
                    panAffected.Visible = true;
                    lblQuestion.Text = oForecast.GetQuestion(intQuestion, "name");
                    lblAffected.Text = oForecast.GetQuestion(intAffected, "name");
                    rptAffected.DataSource = oForecast.GetAffected(intQuestion, intAffected);
                    rptAffected.DataBind();
                }
                else
                {
                    panAffects.Visible = true;
                    lblName.Text = oForecast.GetQuestion(intQuestion, "name") + " Affects";
                    rptAffects.DataSource = oForecast.GetAffectsByQuestionAll(intQuestion);
                    rptAffects.DataBind();
                }
            }
            else
            {
                panView.Visible = true;
                if (!IsPostBack)
                {
                    LoopRepeater();
                    btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=F_QUESTION" + "',false,400,400);");
                    btnPlatforms.Attributes.Add("onclick", "return OpenWindow('QUESTIONPLATFORMS','" + hdnId.ClientID + "','',false,'500',500);");
                    btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                    btnCancel.Attributes.Add("onclick", "return Cancel();");
                }
            }
        }
        private void LoopRepeater()
        {
            DataSet ds = oForecast.GetQuestions(0);
            DataView dv = ds.Tables[0].DefaultView;
            if (Request.QueryString["sort"] != null)
                dv.Sort = Request.QueryString["sort"].ToString();
            rptView.DataSource = dv;
            rptView.DataBind();
            foreach (RepeaterItem ri in rptView.Items)
            {
                ImageButton oDelete = (ImageButton)ri.FindControl("btnDelete");
                oDelete.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this item?');");
                ImageButton oEnable = (ImageButton)ri.FindControl("btnEnable");
                if (oEnable.ImageUrl == "/admin/images/enabled.gif")
                {
                    oEnable.ToolTip = "Click to disable";
                    oEnable.Attributes.Add("onClick", "return confirm('Are you sure you want to disable this item?');");
                }
                else
                    oEnable.ToolTip = "Click to enable";
            }
        }
        protected void OrderView(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            string strSort;
            if (Request.QueryString["sort"] == null)
                strSort = oButton.CommandArgument + " ASC";
            else
                if (Request.QueryString["sort"].ToString() == (oButton.CommandArgument + " ASC"))
                    strSort = oButton.CommandArgument + " DESC";
                else
                    strSort = oButton.CommandArgument + " ASC";
            Response.Redirect(Request.Path + "?sort=" + strSort);
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (Request.Form[hdnId.UniqueID] == "0")
                oForecast.AddQuestion(txtName.Text, txtQuestion.Text, Int32.Parse(ddlType.SelectedItem.Value), (chkOverride.Checked ? 1 : 0), (chkRequired.Checked ? 1 : 0), 0, (chkEnabled.Checked ? 1 : 0));
            else
                oForecast.UpdateQuestion(Int32.Parse(Request.Form[hdnId.UniqueID]), txtName.Text, txtQuestion.Text, Int32.Parse(ddlType.SelectedItem.Value), (chkOverride.Checked ? 1 : 0), (chkRequired.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oForecast.UpdateQuestionOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oEnable = (ImageButton)Sender;
            oForecast.EnableQuestion(Int32.Parse(oEnable.CommandArgument), (oEnable.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, EventArgs e)
        {
            ImageButton oDelete = (ImageButton)Sender;
            oForecast.DeleteQuestion(Int32.Parse(oDelete.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oForecast.DeleteQuestion(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
        protected void btnAffects_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + Request.Form["hdnId"]);
        }
        protected void btnAffected_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&aid=" + oButton.CommandArgument);
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnBackAffected_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"]);
        }
        
    }
}
