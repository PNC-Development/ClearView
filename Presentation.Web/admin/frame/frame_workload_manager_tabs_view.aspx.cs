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
    public partial class frame_workload_manager_tabs_view : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Tabs oTabs;
        protected int intProfile;
        private DataSet ds;
        protected bool boolTaskTab = false;
        protected int intItem = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            if (Request.QueryString["tab"] != null)
                boolTaskTab = true;
            btnAdd.Enabled = false;
            btnSave.Enabled = false;
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                oTabs = new Tabs(intProfile, dsn);
                intItem = Int32.Parse(Request.QueryString["id"]);
                if (!IsPostBack)
                    LoadList();
            }
            imgOrderUp.Attributes.Add("onclick", "return MoveOrderUp(" + lstCurrent.ClientID + ",'" + hdnOrder.ClientID + "');");
            imgOrderDown.Attributes.Add("onclick", "return MoveOrderDown(" + lstCurrent.ClientID + ",'" + hdnOrder.ClientID + "');");
            btnAdd.Attributes.Add("onclick", "return MoveIn(" + ddlAvailable.ClientID + "," + lstCurrent.ClientID + ",'" + hdnOrder.ClientID + "');");
            imgRemove.Attributes.Add("onclick", "return MoveOut(" + lstCurrent.ClientID + "," + ddlAvailable.ClientID + ",'" + hdnOrder.ClientID + "');");
            lstCurrent.Attributes.Add("ondblclick", "return MoveOut(this," + ddlAvailable.ClientID + ",'" + hdnOrder.ClientID + "');");
            btnClose.Attributes.Add("onclick", "return HidePanel();");
        }
        private void LoadList()
        {
            if (boolTaskTab)
                ds = oTabs.GetRequestItemsTaskTabs(intItem);
            else
                ds = oTabs.GetRequestItemsTabs(intItem);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstCurrent.Items.Add(new ListItem(dr["tabname"].ToString(), dr["tabid"].ToString()));
                hdnOrder.Value += dr["tabid"].ToString() + "&";
            }
            ds = oTabs.GetTabs(1);
            ddlAvailable.DataValueField = "id";
            ddlAvailable.DataTextField = "name";
            ddlAvailable.DataSource = ds;
            ddlAvailable.DataBind();
            ddlAvailable.Items.Insert(0, new ListItem("-- SELECT --", "0"));
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
            btnAdd.Enabled = true;
            btnSave.Enabled = true;
        }
        protected  void btnSave_Click(Object Sender, EventArgs e)
        {
            string strOrder = Request.Form[hdnOrder.UniqueID];
            if (boolTaskTab)
                oTabs.DeleteRequestItemsTaskTab(intItem);
            else
                oTabs.DeleteRequestItemsTab(intItem);
            int intDisplay = 0;
            while (strOrder != "")
            {
                intDisplay++;
                int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                if (boolTaskTab)
                    oTabs.AddRequestItemsTaskTab(intId, intItem, intDisplay);
                else
                    oTabs.AddRequestItemsTab(intId, intItem, intDisplay);
            }
            ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "alert('Tab order saved successfully!!');HidePanel();", true);
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
