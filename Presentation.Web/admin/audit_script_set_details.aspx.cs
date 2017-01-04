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
    public partial class audit_script_set_details : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Audit oAudit;
        protected int intProfile;
        protected int intParent = 0;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oAudit = new Audit(intProfile, dsn);

            if (Request.QueryString["parent"] != null)
                intParent = Int32.Parse(Request.QueryString["parent"]);

            if (!IsPostBack)
                LoadList();
            if (intParent > 0)
            {
                hdnParent.Value = intParent.ToString();
                imgOrderUp.Attributes.Add("onclick", "return MoveOrderUp(" + lstCurrent.ClientID + ",'" + hdnOrder.ClientID + "');");
                imgOrderDown.Attributes.Add("onclick", "return MoveOrderDown(" + lstCurrent.ClientID + ",'" + hdnOrder.ClientID + "');");
                btnAdd.Attributes.Add("onclick", "return MoveIn(" + ddlAvailable.ClientID + "," + lstCurrent.ClientID + ",'" + hdnOrder.ClientID + "');");
                imgRemove.Attributes.Add("onclick", "return MoveOut(" + lstCurrent.ClientID + "," + ddlAvailable.ClientID + ",'" + hdnOrder.ClientID + "');");
                lstCurrent.Attributes.Add("ondblclick", "return MoveOut(this," + ddlAvailable.ClientID + ",'" + hdnOrder.ClientID + "');");
            }
        }
        private void LoadList()
        {
            DataSet ds = oAudit.GetScriptSetDetails(intParent);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstCurrent.Items.Add(new ListItem(dr["name"].ToString(), dr["scriptid"].ToString()));
                hdnOrder.Value += dr["scriptid"].ToString() + "&";
            }
            ds = oAudit.GetScripts(1);
            ddlAvailable.DataValueField = "id";
            ddlAvailable.DataTextField = "name";
            ddlAvailable.DataSource = ds;
            ddlAvailable.DataBind();
            ddlAvailable.Items.Insert(0, new ListItem("** REBOOT **", "0"));
            ddlAvailable.Items.Insert(0, new ListItem("-- SELECT --", "-1"));
            for (int ii = 0; ii < lstCurrent.Items.Count; ii++)
            {
                for (int jj = 0; jj < ddlAvailable.Items.Count; jj++)
                {
                    //if (lstCurrent.Items[ii].Value == ddlAvailable.Items[jj].Value && lstCurrent.Items[ii].Value != "0")
                    if (lstCurrent.Items[ii].Value == ddlAvailable.Items[jj].Value)
                    {
                        ddlAvailable.Items.Remove(ddlAvailable.Items[jj]);
                        jj--;
                    }
                }
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            string strOrder = Request.Form[hdnOrder.UniqueID];
            oAudit.DeleteScriptSetDetail(intParent);
            int intDisplay = 0;
            while (strOrder != "")
            {
                intDisplay++;
                int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                oAudit.AddScriptSetDetail(intParent, intId, intDisplay);
            }
            Response.Redirect(Request.Path + "?parent=" + intParent.ToString());
        }
    }
}
