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
    public partial class frame_forecast_steps_reset : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected Forecast oForecast;
        protected int intStep;
        protected int intPlatform;
        protected int intStepId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oForecast = new Forecast(intProfile, dsn);
            btnClose.Attributes.Add("onclick", "return HidePanel();");
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intStep = Int32.Parse(Request.QueryString["id"]);
                intPlatform = Int32.Parse(oForecast.GetStep(intStep, "platformid"));
                intStepId = Int32.Parse(oForecast.GetStep(intStep, "step"));
            }
            if (!IsPostBack)
            {
                Load(null);
                if (intStep > 0)
                    lblName.Text = oForecast.GetStep(intStep, "name");
            }
        }
        private void Load(TreeNode oParent)
        {
            DataSet ds = oForecast.GetSteps(intPlatform, 1);
            DataSet dsOther = oForecast.GetStepReset(intPlatform, intStepId);
            int intCount = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intCount++;
                TreeNode oNode = new TreeNode();
                oNode.Text = intCount.ToString() + ") " + dr["name"].ToString();
                oNode.ToolTip = intCount.ToString() + ") " + dr["name"].ToString();
                oNode.Value = dr["step"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oNode.Checked = false;
                oTree.Nodes.Add(oNode);
                if (dr["id"].ToString() == intStep.ToString())
                    oNode.ShowCheckBox = false;
                else
                {
                    foreach (DataRow drOther in dsOther.Tables[0].Rows)
                    {
                        if (dr["step"].ToString() == drOther["reset"].ToString())
                            oNode.Checked = true;
                    }
                }
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oForecast.DeleteStepReset(intPlatform, intStepId);
            foreach (TreeNode oNode in oTree.Nodes)
            {
                if (oNode.Checked == true)
                    oForecast.AddStepReset(intPlatform, intStepId, Int32.Parse(oNode.Value));
            }
            Reload();
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
