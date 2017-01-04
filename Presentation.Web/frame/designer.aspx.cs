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
    public partial class designer : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected DataSet ds;
        protected int intProfile;
        protected Controls oControl;
        protected Designer oDesign;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oControl = new Controls(intProfile, dsn);
            oDesign = new Designer(intProfile, dsn);
            imgOrderUp.Attributes.Add("onclick", "return MoveOrderUp(" + lstCurrent.ClientID + ",'" + hdnOrder.ClientID + "');");
            imgOrderDown.Attributes.Add("onclick", "return MoveOrderDown(" + lstCurrent.ClientID + ",'" + hdnOrder.ClientID + "');");
            btnAdd.Attributes.Add("onclick", "return MoveIn(" + ddlAvailable.ClientID + "," + lstCurrent.ClientID + ",'" + hdnOrder.ClientID + "');");
            imgRemove.Attributes.Add("onclick", "return MoveOut(" + lstCurrent.ClientID + "," + ddlAvailable.ClientID + ",'" + hdnOrder.ClientID + "');");
            lstCurrent.Attributes.Add("ondblclick", "return MoveOut(this," + ddlAvailable.ClientID + ",'" + hdnOrder.ClientID + "');");
            btnCancel.Attributes.Add("onclick", "return parent.HidePanel();");
            if (!IsPostBack)
                LoadList();
            lblSaved.Visible = false;
            if (Request.QueryString["saved"] != null)
            {
                trSaved.Visible = true;
                lblSaved.Text = "<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Designer Saved";
            }
        }
        private void LoadList()
        {
            ds = oDesign.Get(intProfile, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstCurrent.Items.Add(new ListItem(dr["name"].ToString(), dr["controlid"].ToString()));
                hdnOrder.Value += dr["controlid"].ToString() + "&";
            }
            ds = oControl.Gets(1, 0);
            ddlAvailable.DataValueField = "controlid";
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
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            string strOrder = Request.Form[hdnOrder.UniqueID];
            oDesign.Delete(intProfile);
            int intDisplay = 0;
            while (strOrder != "")
            {
                intDisplay++;
                int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                oDesign.Add(intProfile, intId, intDisplay, 1);
            }
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">parent.navigate(parent.location);<" + "/" + "script>");
        }
    }
}
