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
    public partial class forecast_responses : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Forecast oForecast;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/forecast/forecast_responses.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oForecast = new Forecast(intProfile, dsn);
            if (!IsPostBack)
            {
                Load();
                btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnParent.ClientID + "','" + hdnOrder.ClientID + "&type=F_RESPONSE" + "',false,400,400);");
                btnSelection.Attributes.Add("onclick", "return OpenWindow('SELECTIONCRITERIA','" + hdnId.ClientID + "','',false,'500',500);");
                btnAdditional.Attributes.Add("onclick", "return OpenWindow('RESPONSES_ADDITIONAL','" + hdnId.ClientID + "','',false,'500',500);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                btnParent.Attributes.Add("onclick", "return OpenWindow('FORECASTQUESTIONBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
                btnResponse.Attributes.Add("onclick", "return OpenWindow('FORECASTRESPONSEBROWSER','" + hdnResponse.ClientID + "','&control=" + hdnResponse.ClientID + "&controltext=" + lblResponse.ClientID + "',false,400,600);");
            }
        }
        private void Load()
        {
            DataSet ds = oForecast.GetQuestions(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                Load(Int32.Parse(dr["id"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Response";
                oNew.ToolTip = "Add Response";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = "javascript:Add('" + dr["id"].ToString() + "','" + oForecast.GetQuestion(Int32.Parse(dr["id"].ToString()), "name") + "');";
                oNode.ChildNodes.Add(oNew);
            }
            ddlResponseCategory.DataTextField = "name";
            ddlResponseCategory.DataValueField = "id";
            ddlResponseCategory.DataSource = oForecast.GetForecastResposeCategory(null, "", 1);
            ddlResponseCategory.DataBind();
            ddlResponseCategory.Items.Insert(0, new ListItem("-- SELECT --", "0"));

        }
        private void Load(int _parent, TreeNode oParent)
        {
            DataSet ds = oForecast.GetResponses(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = "javascript:Edit('" + dr["id"].ToString() + "','" + dr["questionid"].ToString() + "','" + oForecast.GetQuestion(Int32.Parse(dr["questionid"].ToString()), "name") + "','" + dr["name"].ToString() + "','" + dr["response"].ToString() + "','" + dr["components"].ToString() + "','" + dr["variance"].ToString() + "','" + dr["custom"].ToString() + "','" + oForecast.GetResponse(Int32.Parse(dr["custom"].ToString()), "name") + "','" + dr["forecast_response_category_id"].ToString() + "','" + dr["enabled"].ToString() + "','" + dr["os_distributed"].ToString() + "','" + dr["os_midrange"].ToString() + "','" + dr["cores"].ToString() + "','" + dr["ram"].ToString() + "','" + dr["web"].ToString() + "','" + dr["dbase"].ToString() + "','" + dr["ha_none"].ToString() + "','" + dr["ha_cluster"].ToString() + "','" + dr["ha_csm"].ToString() + "','" + dr["ha_csm_middleware"].ToString() + "','" + dr["ha_csm_app"].ToString() + "','" + dr["ha_room"].ToString() + "','" + dr["dr_under"].ToString() + "','" + dr["dr_over"].ToString() + "','" + dr["one_one"].ToString() + "','" + dr["many_one"].ToString() + "');";
                oParent.ChildNodes.Add(oNode);
            }
            oTreeview.ExpandDepth = 1;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intQuestion = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            int intResponse = Int32.Parse(Request.Form[hdnResponse.UniqueID]);
            if (Request.Form[hdnId.UniqueID] == "0")
                oForecast.AddResponse(intQuestion, txtName.Text, txtResponse.Text, (chkVariance.Checked ? 1 : 0), intResponse, (chkOSDistributed.Checked ? 1 : 0), (chkOSMidrange.Checked ? 1 : 0), (chkCores.Checked ? 1 : 0), (chkRAM.Checked ? 1 : 0), (chkWeb.Checked ? 1 : 0), (chkDatabase.Checked ? 1 : 0), (chkHANone.Checked ? 1 : 0), (chkHACluster.Checked ? 1 : 0), (chkHACSM.Checked ? 1 : 0), (chkHACSMMiddleware.Checked ? 1 : 0), (chkHACSMApp.Checked ? 1 : 0), (chkHARoom.Checked ? 1 : 0), (chkDRUnder.Checked ? 1 : 0), (chkDROver.Checked ? 1 : 0), (chkDROne.Checked ? 1 : 0), (chkDRMany.Checked ? 1 : 0), txtComponents.Text, Int32.Parse(ddlResponseCategory.SelectedValue), oForecast.GetResponses(intQuestion, 1).Tables[0].Rows.Count + 1, (chkEnabled.Checked ? 1 : 0));
            else
                oForecast.UpdateResponse(Int32.Parse(Request.Form[hdnId.UniqueID]), intQuestion, txtName.Text, txtResponse.Text, (chkVariance.Checked ? 1 : 0), intResponse, (chkOSDistributed.Checked ? 1 : 0), (chkOSMidrange.Checked ? 1 : 0), (chkCores.Checked ? 1 : 0), (chkRAM.Checked ? 1 : 0), (chkWeb.Checked ? 1 : 0), (chkDatabase.Checked ? 1 : 0), (chkHANone.Checked ? 1 : 0), (chkHACluster.Checked ? 1 : 0), (chkHACSM.Checked ? 1 : 0), (chkHACSMMiddleware.Checked ? 1 : 0), (chkHACSMApp.Checked ? 1 : 0), (chkHARoom.Checked ? 1 : 0), (chkDRUnder.Checked ? 1 : 0), (chkDROver.Checked ? 1 : 0), (chkDROne.Checked ? 1 : 0), (chkDRMany.Checked ? 1 : 0), txtComponents.Text, Int32.Parse(ddlResponseCategory.SelectedValue), (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oForecast.UpdateResponseOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected  void btnDelete_Click(Object Sender, EventArgs e)
        {
            oForecast.DeleteResponse(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
