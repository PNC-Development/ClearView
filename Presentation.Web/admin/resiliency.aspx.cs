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
using System.IO;

namespace NCC.ClearView.Presentation.Web
{
    public partial class resiliency : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Resiliency oResiliency;
        protected Locations oLocation;
        protected int intProfile;
        protected int intID = 0;
        protected string strLocationProd = "";
        protected string strLocationDR = "";
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = Request.Path;
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oResiliency = new Resiliency(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            if (!IsPostBack)
                LoadLists();
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["add"] == null)
                    LoopRepeater();
                else
                {
                    panAdd.Visible = true;
                    btnDelete.Enabled = false;
                    txtMin.Text = "0";
                    txtMax.Text = "0";
                }
            }
            else
            {
                panAdd.Visible = true;
                intID = Int32.Parse(Request.QueryString["id"]);
                if (intID > 0 && !IsPostBack)
                {
                    DataSet ds = oResiliency.Get(intID);
                    hdnId.Value = intID.ToString();
                    txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    chkBIR.Checked = (ds.Tables[0].Rows[0]["bir"].ToString() == "1");
                    txtMin.Text = ds.Tables[0].Rows[0]["min"].ToString();
                    txtMax.Text = ds.Tables[0].Rows[0]["max"].ToString();
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                    btnAdd.Text = "Update";

                    rptItems.DataSource = oResiliency.GetLocations(intID);
                    rptItems.DataBind();
                    lblNone.Visible = (rptItems.Items.Count == 0);
                    foreach (RepeaterItem ri in rptItems.Items)
                        ((LinkButton)ri.FindControl("btnDeleteLocation")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");

                    strLocationProd = oLocation.LoadDDL("ddlStateFinal", "ddlCityFinal", "ddlAddressFinal", hdnLocationProd.ClientID, intLocation, true, "ddlCommonFinal");
                    hdnLocationProd.Value = intLocation.ToString();
                    strLocationDR = oLocation.LoadDDL("ddlStateBuild", "ddlCityBuild", "ddlAddressBuild", hdnLocationDR.ClientID, intLocation, true, "ddlCommonBuild");
                    hdnLocationDR.Value = intLocation.ToString();
                }
            }
            // Modify "/admin/frame/frame_support_order.aspx" with the "&type=xxx" value.
            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=RESILIENCY" + "',false,400,400);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
        }
        private void LoadLists()
        {
        }
        private void LoopRepeater()
        {
            panView.Visible = true;
            DataSet ds = oResiliency.Gets(0);
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
            if (intID == 0)
                oResiliency.Add(txtName.Text, (chkBIR.Checked ? 1 : 0), Int32.Parse(txtMin.Text), Int32.Parse(txtMax.Text), (oResiliency.Gets(0).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
            else
                oResiliency.Update(intID, txtName.Text, (chkBIR.Checked ? 1 : 0), Int32.Parse(txtMin.Text), Int32.Parse(txtMax.Text), (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oResiliency.UpdateOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oResiliency.Enable(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oResiliency.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oResiliency.Delete(intID);
            Response.Redirect(Request.Path);
        }
        protected void btnAddLocation_Click(Object Sender, EventArgs e)
        {
            oResiliency.AddLocation(intID, Int32.Parse(Request.Form[hdnLocationProd.UniqueID]), Int32.Parse(Request.Form[hdnLocationDR.UniqueID]));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }
        protected void btnDeleteLocation_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oResiliency.DeleteLocation(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&delete=true");
        }
    }
}
