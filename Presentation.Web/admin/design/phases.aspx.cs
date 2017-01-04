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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class design_phases : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Design oDesign;
        protected int intProfile;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = Request.Path;
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oDesign = new Design(intProfile, dsn);
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["add"] == null)
                    LoopRepeater();
                else
                {
                    panAdd.Visible = true;
                    btnOrder.Enabled = false;
                    btnDelete.Enabled = false;
                }
            }
            else
            {
                panAdd.Visible = true;
                intID = Int32.Parse(Request.QueryString["id"]);
                if (intID > 0)
                {
                    if (!IsPostBack)
                    {
                        DataSet ds = oDesign.GetPhase(intID);
                        hdnId.Value = intID.ToString();
                        txtTitle.Text = ds.Tables[0].Rows[0]["title"].ToString();
                        txtDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                        txtHelp.Text = ds.Tables[0].Rows[0]["help"].ToString();
                        chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnAdd.Text = "Save";
                        btnAddBack.Text = "Save & Return";

                        // Load Configuration
                        StringBuilder strConfiguration = new StringBuilder();
                        strConfiguration.Append("<p><b>Responses causing this phase to be disabled:</b><br/>");
                        DataSet dsSelections = oDesign.GetRestriction(intID, 1);
                        foreach (DataRow drSelection in dsSelections.Tables[0].Rows)
                        {
                            int intResponse = Int32.Parse(drSelection["responseid"].ToString());
                            strConfiguration.Append("&nbsp;&nbsp;-&nbsp;<a href=\"responses.aspx?id=" + intResponse.ToString() + "&menu_tab=2\">");
                            strConfiguration.Append(oDesign.GetResponse(intResponse, "response"));
                            strConfiguration.Append("</a><br/>");
                        }
                        strConfiguration.Append("</p>");
                        strConfiguration.Append("<p><b>Responses causing this phase to be enabled:</b><br/>");
                        dsSelections = oDesign.GetRestriction(intID, 0);
                        foreach (DataRow drSelection in dsSelections.Tables[0].Rows)
                        {
                            int intResponse = Int32.Parse(drSelection["responseid"].ToString());
                            strConfiguration.Append("&nbsp;&nbsp;-&nbsp;<a href=\"responses.aspx?id=" + intResponse.ToString() + "&menu_tab=2\">");
                            strConfiguration.Append(oDesign.GetResponse(intResponse, "response"));
                            strConfiguration.Append("</a><br/>");
                        }
                        strConfiguration.Append("</p>");
                        litConfiguration.Text = strConfiguration.ToString();
                    }
                }
                else
                {
                    btnDelete.Enabled = false;
                }
            }
            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=D_PHASES" + "',false,400,400);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
        }
        private void LoopRepeater()
        {
            panView.Visible = true;
            DataSet ds = oDesign.GetPhases(0);
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
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=0");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            Save();
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }
        protected void btnAddBack_Click(Object Sender, EventArgs e)
        {
            Save();
            Response.Redirect(Request.Path);
        }
        private void Save()
        {
            if (intID == 0)
                oDesign.AddPhase(txtTitle.Text, txtDescription.Text, txtHelp.Text, (oDesign.GetPhases(0).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
            else
                oDesign.UpdatePhase(intID, txtTitle.Text, txtDescription.Text, txtHelp.Text, (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oDesign.UpdatePhaseOrder(intId, intCount);
                }
            }
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oDesign.EnablePhase(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oDesign.DeletePhase(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oDesign.DeletePhase(intID);
            Response.Redirect(Request.Path);
        }
    }
}
