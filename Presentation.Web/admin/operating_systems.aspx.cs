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
    public partial class operating_systems : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected OperatingSystems oOperatingSystems;
        protected ServicePacks oServicePack;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/operating_systems.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oOperatingSystems = new OperatingSystems(intProfile, dsn);
            oServicePack = new ServicePacks(intProfile, dsn);
            if (!IsPostBack)
            {
                LoadList();
                LoopRepeater();
                btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=OPERATING_SYSTEM" + "',false,400,400);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                btnServicePacks.Attributes.Add("onclick", "return OpenWindow('OSSERVICEPACKS','" + hdnId.ClientID + "','',false,'500',300);");
            }
        }
        private void LoadList()
        {
            ddlDefaultSP.DataValueField = "id";
            ddlDefaultSP.DataTextField = "name";
            ddlDefaultSP.DataSource = oServicePack.Gets(1);
            ddlDefaultSP.DataBind();
            ddlDefaultSP.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void LoopRepeater()
        {
            DataSet ds = oOperatingSystems.Gets(0);
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
                oOperatingSystems.Add(txtName.Text, (chkCluster.Checked ? 1 : 0), (chkWorkstation.Checked ? 1 : 0), txtCode.Text, txtFactoryCode.Text, txtCitrixConfig.Text, txtZEUSOs.Text, txtZEUSOsVersion.Text, txtZEUSBuildType.Text, txtZEUSBuildTypePNC.Text, txtVMware.Text, txtBootEnvironment.Text, txtTaskSequence.Text, txtAltiris.Text, (chkLinux.Checked ? 1 : 0), (chkAix.Checked ? 1 : 0), (chkSolaris.Checked ? 1 : 0), (chkWindows.Checked ? 1 : 0), (chkWindows2008.Checked ? 1 : 0), (radRDPAltiris.Checked ? 1 : 0), (radRDPMDT.Checked ? 1 : 0), (chkE1000.Checked ? 1 : 0), (chkManualBuild.Checked ? 1 : 0), Int32.Parse(ddlDefaultSP.SelectedItem.Value), (chkStandard.Checked ? 1 : 0), (oOperatingSystems.Gets(0).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
            else
                oOperatingSystems.Update(Int32.Parse(Request.Form[hdnId.UniqueID]), txtName.Text, (chkCluster.Checked ? 1 : 0), (chkWorkstation.Checked ? 1 : 0), txtCode.Text, txtFactoryCode.Text, txtCitrixConfig.Text, txtZEUSOs.Text, txtZEUSOsVersion.Text, txtZEUSBuildType.Text, txtZEUSBuildTypePNC.Text, txtVMware.Text, txtBootEnvironment.Text, txtTaskSequence.Text, txtAltiris.Text, (chkLinux.Checked ? 1 : 0), (chkAix.Checked ? 1 : 0), (chkSolaris.Checked ? 1 : 0), (chkWindows.Checked ? 1 : 0), (chkWindows2008.Checked ? 1 : 0), (radRDPAltiris.Checked ? 1 : 0), (radRDPMDT.Checked ? 1 : 0), (chkE1000.Checked ? 1 : 0), (chkManualBuild.Checked ? 1 : 0), Int32.Parse(ddlDefaultSP.SelectedItem.Value), (chkStandard.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oOperatingSystems.UpdateOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oOperatingSystems.Enable(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oOperatingSystems.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oOperatingSystems.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
