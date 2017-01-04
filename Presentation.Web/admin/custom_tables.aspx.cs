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
    public partial class custom_tables : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Field oField;
        protected Functions oFunctions;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/custom_tables.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oField = new Field(intProfile, dsn);
            oFunctions = new Functions(intProfile, dsn, intEnvironment);
            if (!IsPostBack)
            {
                LoopRepeater();
                btnTable.Attributes.Add("onclick", "return OpenWindow('SQLTABLEBROWSER','" + txtName.ClientID + "','&control=" + txtName.ClientID + "&controltext=" + txtName.ClientID + "',false,400,600);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this table?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                //btnItems.Attributes.Add("onclick", "return OpenWindow('TABLEITEMBROWSER','" + hdnId.ClientID + "','',false,'500',500);");
                btnItems.Attributes.Add("onclick", "return OpenWindow('TABLESERVICEBROWSER','" + hdnId.ClientID + "','',false,'500',500);");
                btnPopulate.Attributes.Add("onclick", "return confirm('Are you sure you want to remove all existing fields and populate this table?');");
            }
        }
        private void LoopRepeater()
        {
            DataSet ds = oField.GetTables(0);
            DataView dv = ds.Tables[0].DefaultView;
            if (Request.QueryString["sort"] != null)
                dv.Sort = Request.QueryString["sort"].ToString();
            rptView.DataSource = dv;
            rptView.DataBind();
            foreach (RepeaterItem ri in rptView.Items)
            {
                ImageButton oDelete = (ImageButton)ri.FindControl("btnDelete");
                oDelete.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this table?');");
                ImageButton oEnable = (ImageButton)ri.FindControl("btnEnable");
                if (oEnable.ImageUrl == "images/enabled.gif")
                {
                    oEnable.ToolTip = "Click to disable";
                    oEnable.Attributes.Add("onClick", "return confirm('Are you sure you want to disable this table?');");
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
                oField.AddTable(txtName.Text, (chkEnabled.Checked == true ? 1 : 0));
            else
                oField.UpdateTable(Int32.Parse(Request.Form[hdnId.UniqueID]), txtName.Text, (chkEnabled.Checked == true ? 1 : 0));
            Response.Redirect(Request.Path);
        }
        protected void btnPopulate_Click(Object Sender, EventArgs e)
        {
            int intTable = Int32.Parse(Request.Form[hdnId.UniqueID]);
            DataSet ds = oField.Gets(intTable, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                oField.Delete(Int32.Parse(dr["id"].ToString()));
            }
            DataSet dsD = oFunctions.GetSystemTableColumns(oField.GetTable(intTable, "tablename"));
            int intCount = 0;
            foreach (DataRow drD in dsD.Tables[0].Rows)
            {
                if (drD["name"].ToString().ToUpper() != "REQUESTID" && drD["name"].ToString().ToUpper() != "ITEMID" && drD["name"].ToString().ToUpper() != "NUMBER" && drD["name"].ToString().ToUpper() != "MODIFIED" && drD["name"].ToString().ToUpper() != "DELETED")
                {
                    intCount++;
                    oField.Add(intTable, drD["name"].ToString(), drD["name"].ToString(), "S", "", "", "", 1, intCount, 1);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oField.EnableTable(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oField.DeleteTable(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oField.DeleteTable(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
