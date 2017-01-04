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
    public partial class frame_report_permissions : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Reports oReport;
        private DataSet ds;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oReport = new Reports(intProfile, dsn);
            lblFinish.Visible = false;
            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                lblFinish.Visible = true;
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                if (!IsPostBack)
                {
                    lblId.Text = Request.QueryString["id"];
                    int intReport = Int32.Parse(lblId.Text);
                    lblName.Text = oReport.GetName(intReport);
                    LoadLists();
                    btnClose.Attributes.Add("onclick", "return HidePanel();");
                }
            }
        }
        private void LoadLists()
        {
            int intReport = Int32.Parse(lblId.Text);
            ds = oReport.GetPermissions(intReport);
            string strGroupId = "0";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (strGroupId != dr["groupid"].ToString())
                {
                    strGroupId = dr["groupid"].ToString();
                    lstCurrent.Items.Add(new ListItem(dr["groupname"].ToString(), dr["groupid"].ToString()));
                }
            }
            ds = oReport.GetGroups(1);
            ddlAvailable.DataValueField = "groupid";
            ddlAvailable.DataTextField = "name";
            ddlAvailable.DataSource = ds;
            ddlAvailable.DataBind();
            ddlAvailable.Items.Insert(0, "-- SELECT --");
            for (int ii = 0; ii < lstCurrent.Items.Count; ii++)
            {
                for (int jj = 0; jj < ddlAvailable.Items.Count; jj++)
                {
                    if (lstCurrent.Items[ii].Text == ddlAvailable.Items[jj].Text)
                    {
                        ddlAvailable.Items.Remove(ddlAvailable.Items[jj]);
                        jj--;
                    }
                }
            }
        }
        protected  void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intReport = Int32.Parse(lblId.Text);
            if (ddlAvailable.SelectedIndex > 0)
            {
                oReport.AddPermission(intReport, Int32.Parse(ddlAvailable.SelectedItem.Value));
                Response.Redirect(Request.Path + "?id=" + intReport.ToString() + "&save=true");
            }
            else
                Response.Redirect(Request.Path + "?id=" + intReport.ToString());
        }
        protected void btnRemove_Click(Object Sender, EventArgs e)
        {
            int intReport = Int32.Parse(lblId.Text);
            if (lstCurrent.SelectedIndex > -1)
            {
                oReport.DeletePermission(intReport, Int32.Parse(lstCurrent.SelectedItem.Value));
                Response.Redirect(Request.Path + "?id=" + intReport.ToString() + "&save=true");
            }
            else
                Response.Redirect(Request.Path + "?id=" + intReport.ToString());
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
