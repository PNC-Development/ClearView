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
    public partial class workstation_ping : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Users oUser;
        protected int intProfile;
        protected int intInterval = 15;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/workstation_ping.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oUser = new Users(intProfile, dsn);
            if (!IsPostBack)
            {
                int intOldUser = 0;
                DateTime datOldDate = DateTime.MinValue;
                DateTime datOldTime = DateTime.MinValue;
                DateTime datStart = DateTime.MinValue;
                DateTime datEnd = DateTime.MinValue;
                string strDuration = "";
                TreeNode oParent = new TreeNode();
                TreeNode oDate = new TreeNode();
                DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "sv_getWorkstationPings");
                lblCount.Text = "(" + ds.Tables[0].Rows.Count.ToString() + " Records)";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intUser = Int32.Parse(dr["userid"].ToString());
                    if (intOldUser != intUser)
                    {
                        intOldUser = intUser;
                        oParent = new TreeNode();
                        oParent.Text = dr["username"].ToString();
                        oParent.ToolTip = dr["username"].ToString();
                        oParent.ImageUrl = "/images/folder.gif";
                        oParent.SelectAction = TreeNodeSelectAction.Expand;
                        oTreeview.Nodes.Add(oParent);
                        if (strDuration != "")
                        {
                            datEnd = datOldTime;
                            strDuration += " - " + datEnd.ToShortTimeString();
                            TreeNode oNode = new TreeNode();
                            oNode.Text = strDuration;
                            oNode.ToolTip = strDuration;
                            oNode.SelectAction = TreeNodeSelectAction.None;
                            oDate.ChildNodes.Add(oNode);
                            strDuration = "";
                        }
                        datOldDate = DateTime.MinValue;
                        datOldTime = DateTime.MinValue;
                    }
                    DateTime datDate = DateTime.Parse(dr["modified"].ToString());
                    if (datOldDate == DateTime.MinValue || datOldDate.ToShortDateString() != datDate.ToShortDateString())
                    {
                        datOldDate = datDate;
                        if (strDuration != "")
                        {
                            datEnd = datOldTime;
                            strDuration += " - " + datEnd.ToShortTimeString();
                            TreeNode oNode = new TreeNode();
                            oNode.Text = strDuration;
                            oNode.ToolTip = strDuration;
                            oNode.SelectAction = TreeNodeSelectAction.None;
                            oDate.ChildNodes.Add(oNode);
                            // Calculate Time
                            TimeSpan oHRs = datEnd.Subtract(datStart);
                            double dblMinutes = double.Parse(oHRs.Minutes.ToString());
                            dblMinutes = dblMinutes / 60.00;
                            oDate.Text += " (" + dblMinutes.ToString("N") + " HRs)";
                            strDuration = "";
                        }
                        oDate = new TreeNode();
                        oDate.Text = datDate.ToLongDateString();
                        oDate.ToolTip = datDate.ToLongDateString();
                        oDate.ImageUrl = "/images/folder.gif";
                        oDate.SelectAction = TreeNodeSelectAction.Expand;
                        oParent.ChildNodes.Add(oDate);
                        datOldTime = DateTime.MinValue;
                    }
                    if (datOldTime == DateTime.MinValue)
                    {
                        datStart = datDate;
                        strDuration = " - " + datStart.ToShortTimeString();
                    }
                    else
                    {
                        TimeSpan oSpan = datDate.Subtract(datOldTime);
                        if (oSpan.Minutes > intInterval)
                        {
                            datEnd = datOldTime;
                            strDuration += " - " + datEnd.ToShortTimeString();
                            TreeNode oNode = new TreeNode();
                            oNode.Text = strDuration;
                            oNode.ToolTip = strDuration;
                            oNode.SelectAction = TreeNodeSelectAction.None;
                            oDate.ChildNodes.Add(oNode);
                            strDuration = "";
                        }
                        else if (strDuration == "")
                        {
                            datStart = datDate;
                            strDuration = " - " + datStart.ToShortTimeString();
                        }
                    }
                    datOldTime = datDate;
                }
                if (strDuration != "")
                {
                    datEnd = datOldTime;
                    strDuration += " - " + datEnd.ToShortTimeString();
                    TreeNode oNode = new TreeNode();
                    oNode.Text = strDuration;
                    oNode.ToolTip = strDuration;
                    oNode.SelectAction = TreeNodeSelectAction.None;
                    oDate.ChildNodes.Add(oNode);
                    // Calculate Time
                    TimeSpan oHRs = datEnd.Subtract(datStart);
                    double dblMinutes = double.Parse(oHRs.Minutes.ToString());
                    dblMinutes = dblMinutes / 60.00;
                    oDate.Text += " (" + dblMinutes.ToString("N") + " HRs)";
                    strDuration = "";
                }
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete these records?');");
                oTreeview.CollapseAll();
            }
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            SqlHelper.ExecuteDataset(dsn, CommandType.Text, "DELETE FROM sv_workstation_ping_results WHERE modified < '" + txtDate.Text + "'");
            Response.Redirect(Request.Path);
        }
    }
}
