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
    public partial class pnc_tasks : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected PNCTasks oPNCTask;
        protected Services oService;
        protected int intProfile;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/pnc_tasks.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oPNCTask = new PNCTasks(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["add"] == null)
                    LoopRepeater();
                else
                    panAdd.Visible = true;
            }
            else
            {
                panAdd.Visible = true;
                intID = Int32.Parse(Request.QueryString["id"]);
                if (intID > 0 && !IsPostBack)
                {
                    DataSet ds = oPNCTask.Get(intID);
                    int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                    hdnParent.Value = intService.ToString();
                    lblParent.Text = oService.Get(intService, "name");
                    chkIfCluster.Checked = (ds.Tables[0].Rows[0]["if_cluster"].ToString() == "1");
                    chkIfSQL.Checked = (ds.Tables[0].Rows[0]["if_sql"].ToString() == "1");
                    chkIfCitrix.Checked = (ds.Tables[0].Rows[0]["if_citrix"].ToString() == "1");
                    chkIfLTMcfg.Checked = (ds.Tables[0].Rows[0]["if_ltm_config"].ToString() == "1");
                    chkIfLTMins.Checked = (ds.Tables[0].Rows[0]["if_ltm_install"].ToString() == "1");
                    chkVirtual.Checked = (ds.Tables[0].Rows[0]["if_virtual"].ToString() == "1");
                    chkPhysical.Checked = (ds.Tables[0].Rows[0]["if_physical"].ToString() == "1");
                    chkStorage.Checked = (ds.Tables[0].Rows[0]["storage"].ToString() == "1");
                    chkDNS.Checked = (ds.Tables[0].Rows[0]["dns"].ToString() == "1");
                    chkTSM.Checked = (ds.Tables[0].Rows[0]["tsm"].ToString() == "1");
                    chkLegato.Checked = (ds.Tables[0].Rows[0]["legato"].ToString() == "1");
                    chkPNC.Checked = (ds.Tables[0].Rows[0]["pnc"].ToString() == "1");
                    chkNCB.Checked = (ds.Tables[0].Rows[0]["ncb"].ToString() == "1");
                    chkDistributed.Checked = (ds.Tables[0].Rows[0]["distributed"].ToString() == "1");
                    chkMidrange.Checked = (ds.Tables[0].Rows[0]["midrange"].ToString() == "1");
                    chkOffsite.Checked = (ds.Tables[0].Rows[0]["offsite"].ToString() == "1");
                    chkImplementor.Checked = (ds.Tables[0].Rows[0]["implementor"].ToString() == "1");
                    chkNetworkEngineer.Checked = (ds.Tables[0].Rows[0]["network_engineer"].ToString() == "1");
                    chkDBA.Checked = (ds.Tables[0].Rows[0]["dba"].ToString() == "1");
                    chkProjectManager.Checked = (ds.Tables[0].Rows[0]["project_manager"].ToString() == "1");
                    chkDepartmentalManager.Checked = (ds.Tables[0].Rows[0]["departmental_manager"].ToString() == "1");
                    chkApplicationLead.Checked = (ds.Tables[0].Rows[0]["application_lead"].ToString() == "1");
                    chkAdministrativeContact.Checked = (ds.Tables[0].Rows[0]["administrative_contact"].ToString() == "1");
                    chkApplicationOwner.Checked = (ds.Tables[0].Rows[0]["application_owner"].ToString() == "1");
                    chkRequestor.Checked = (ds.Tables[0].Rows[0]["requestor"].ToString() == "1");
                    chkService.Checked = (chkImplementor.Checked == false && chkNetworkEngineer.Checked == false && chkDBA.Checked == false && chkProjectManager.Checked == false && chkDepartmentalManager.Checked == false && chkApplicationLead.Checked == false && chkAdministrativeContact.Checked == false && chkApplicationOwner.Checked == false && chkRequestor.Checked == false);
                    chkDecom.Checked = (ds.Tables[0].Rows[0]["decom"].ToString() == "1");
                    txtStep.Text = ds.Tables[0].Rows[0]["step"].ToString();
                    txtSubStep.Text = ds.Tables[0].Rows[0]["substep"].ToString();
                    chkNonTransparent.Checked = (ds.Tables[0].Rows[0]["non_transparent"].ToString() == "1");
                    chkClient.Checked = (ds.Tables[0].Rows[0]["client"].ToString() == "1");
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                }
            }
            btnParent.Attributes.Add("onclick", "return OpenWindow('SERVICEBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
        }
        private void LoopRepeater()
        {
            panView.Visible = true;
            DataSet ds = oPNCTask.Gets(0, 0, 0, 0, -1, 0);
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
                Label lblCustom = (Label)ri.FindControl("lblCustom");
                int intCustom = Int32.Parse(lblCustom.Text);
                DataSet dsCustom = oPNCTask.Get(intCustom);
                if (dsCustom.Tables[0].Rows.Count > 0)
                {
                    lblCustom.Text = "";
                    DataRow drCustom = dsCustom.Tables[0].Rows[0];

                    if (drCustom["if_cluster"].ToString() == "1")
                        lblCustom.Text += "Cluster";
                    if (drCustom["if_sql"].ToString() == "1")
                    {
                        if (lblCustom.Text != "")
                            lblCustom.Text += " AND ";
                        lblCustom.Text += "SQL";
                    }
                    if (drCustom["if_citrix"].ToString() == "1")
                    {
                        if (lblCustom.Text != "")
                            lblCustom.Text += " AND ";
                        lblCustom.Text += "Citrix";
                    }
                    if (drCustom["if_ltm_config"].ToString() == "1")
                    {
                        if (lblCustom.Text != "")
                            lblCustom.Text += " AND ";
                        lblCustom.Text += "LTM Config";
                    }
                    if (drCustom["if_ltm_install"].ToString() == "1")
                    {
                        if (lblCustom.Text != "")
                            lblCustom.Text += " AND ";
                        lblCustom.Text += "LTM Install";
                    }
                    if (drCustom["dns"].ToString() == "1")
                    {
                        if (lblCustom.Text != "")
                            lblCustom.Text += " AND ";
                        lblCustom.Text += "DNS";
                    }
                    if (drCustom["tsm"].ToString() == "1")
                    {
                        if (lblCustom.Text != "")
                            lblCustom.Text += " AND ";
                        lblCustom.Text += "TSM";
                    }
                    if (drCustom["legato"].ToString() == "1")
                    {
                        if (lblCustom.Text != "")
                            lblCustom.Text += " AND ";
                        lblCustom.Text += "Legato";
                    }
                    if (drCustom["implementor"].ToString() == "1")
                    {
                        if (lblCustom.Text != "")
                            lblCustom.Text += " AND ";
                        lblCustom.Text += "Implementor";
                    }

                    if (lblCustom.Text != "")
                        lblCustom.Text += " ONLY";
                }
                else
                    lblCustom.Text = "xxx";
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
            int intService = 0;
            if (Request.Form[hdnParent.UniqueID] != "")
                intService = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            if (intID == 0)
                oPNCTask.Add(intService, (chkIfCluster.Checked ? 1 : 0), (chkIfSQL.Checked ? 1 : 0), (chkIfCitrix.Checked ? 1 : 0), (chkIfLTMcfg.Checked ? 1 : 0), (chkIfLTMins.Checked ? 1 : 0), (chkVirtual.Checked ? 1 : 0), (chkPhysical.Checked ? 1 : 0), (chkStorage.Checked ? 1 : 0), (chkDNS.Checked ? 1 : 0), (chkTSM.Checked ? 1 : 0), (chkLegato.Checked ? 1 : 0), (chkPNC.Checked ? 1 : 0), (chkNCB.Checked ? 1 : 0), (chkDistributed.Checked ? 1 : 0), (chkMidrange.Checked ? 1 : 0), (chkOffsite.Checked ? 1 : 0), (chkImplementor.Checked ? 1 : 0), (chkNetworkEngineer.Checked ? 1 : 0), (chkDBA.Checked ? 1 : 0), (chkProjectManager.Checked ? 1 : 0), (chkDepartmentalManager.Checked ? 1 : 0), (chkApplicationLead.Checked ? 1 : 0), (chkAdministrativeContact.Checked ? 1 : 0), (chkApplicationOwner.Checked ? 1 : 0), (chkRequestor.Checked ? 1 : 0), (chkDecom.Checked ? 1 : 0), Int32.Parse(txtStep.Text), Int32.Parse(txtSubStep.Text), (chkNonTransparent.Checked ? 1 : 0), (chkClient.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            else
                oPNCTask.Update(intID, intService, (chkIfCluster.Checked ? 1 : 0), (chkIfSQL.Checked ? 1 : 0), (chkIfCitrix.Checked ? 1 : 0), (chkIfLTMcfg.Checked ? 1 : 0), (chkIfLTMins.Checked ? 1 : 0), (chkVirtual.Checked ? 1 : 0), (chkPhysical.Checked ? 1 : 0), (chkStorage.Checked ? 1 : 0), (chkDNS.Checked ? 1 : 0), (chkTSM.Checked ? 1 : 0), (chkLegato.Checked ? 1 : 0), (chkPNC.Checked ? 1 : 0), (chkNCB.Checked ? 1 : 0), (chkDistributed.Checked ? 1 : 0), (chkMidrange.Checked ? 1 : 0), (chkOffsite.Checked ? 1 : 0), (chkImplementor.Checked ? 1 : 0), (chkNetworkEngineer.Checked ? 1 : 0), (chkDBA.Checked ? 1 : 0), (chkProjectManager.Checked ? 1 : 0), (chkDepartmentalManager.Checked ? 1 : 0), (chkApplicationLead.Checked ? 1 : 0), (chkAdministrativeContact.Checked ? 1 : 0), (chkApplicationOwner.Checked ? 1 : 0), (chkRequestor.Checked ? 1 : 0), (chkDecom.Checked ? 1 : 0), Int32.Parse(txtStep.Text), Int32.Parse(txtSubStep.Text), (chkNonTransparent.Checked ? 1 : 0), (chkClient.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oPNCTask.Enable(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oPNCTask.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oPNCTask.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
