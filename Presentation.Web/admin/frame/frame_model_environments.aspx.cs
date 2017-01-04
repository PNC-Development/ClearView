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
    public partial class frame_model_environments : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Models oModel;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected int intParent = 0;
        protected int intModel = 0;
        protected int intClass = 0;
        protected int intEnv = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oModel = new Models(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">alert('Record Added Successfully');<" + "/" + "script>");
            if (Request.QueryString["delete"] != null && Request.QueryString["delete"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "delete", "<script type=\"text/javascript\">alert('Record Deleted Successfully');<" + "/" + "script>");
            if (Request.QueryString["modelid"] != null && Request.QueryString["modelid"] != "")
                intModel = Int32.Parse(Request.QueryString["modelid"]);
            if (Request.QueryString["classid"] != null && Request.QueryString["classid"] != "")
                intClass = Int32.Parse(Request.QueryString["classid"]);
            if (Request.QueryString["environmentid"] != null && Request.QueryString["environmentid"] != "")
                intEnv = Int32.Parse(Request.QueryString["environmentid"]);
            if (intModel > 0 && intClass > 0 && intEnv > 0)
            {
                DataSet ds = oModel.GetReservation(intModel, intClass, intEnv);
                if (ds.Tables[0].Rows.Count > 0)
                    intParent = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                if (!IsPostBack)
                {
                    lblName.Text = oModel.Get(intModel, "name");
                    lblClass.Text = oClass.Get(intClass, "name");
                    lblEnvironment.Text = oEnvironment.Get(intEnv, "name");
                    LoadLists();
                }
                btnClose.Attributes.Add("onclick", "return HidePanel();");
                btnRemove.Attributes.Add("onclick", "return confirm('Are you sure you want to remove this item?');");
                ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',0);");
                ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
            }
        }
        private void LoadLists()
        {
            Classes oClass = new Classes(intProfile, dsn);
            ddlClass.DataTextField = "name";
            ddlClass.DataValueField = "id";
            ddlClass.DataSource = oClass.Gets(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            lstCurrent.DataValueField = "id";
            lstCurrent.DataTextField = "name";
            lstCurrent.DataSource = oModel.GetReservationLists(intModel, intClass, intEnv);
            lstCurrent.DataBind();
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (intParent == 0)
                intParent = oModel.AddReservation(intModel, intClass, intEnv);
            oModel.AddReservationList(intParent, Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), (chkMovable.Checked ? 1 : 0), (oModel.GetReservationLists(intModel, intClass, intEnv).Tables[0].Rows.Count + 1));
            Response.Redirect(Request.Path + "?modelid=" + intModel.ToString() + "&classid=" + intClass.ToString() + "&environmentid=" + intEnv.ToString() + "&save=true");
        }
        protected void btnUp_Click(Object Sender, EventArgs e)
        {
            int intIndex = lstCurrent.SelectedIndex;
            if (intIndex > -1 && intIndex > 0)
            {
                ListItem oTemp = lstCurrent.SelectedItem;
                lstCurrent.Items.Remove(lstCurrent.SelectedItem);
                lstCurrent.Items.Insert(intIndex - 1, oTemp);
                ReOrder();
            }
            else
                Response.Redirect(Request.Path + "?modelid=" + intModel.ToString() + "&classid=" + intClass.ToString() + "&environmentid=" + intEnv.ToString());
        }
        protected void btnRemove_Click(Object Sender, EventArgs e)
        {
            if (lstCurrent.SelectedIndex > -1)
            {
                oModel.DeleteReservationList(Int32.Parse(lstCurrent.Items[lstCurrent.SelectedIndex].Value));
                Response.Redirect(Request.Path + "?modelid=" + intModel.ToString() + "&classid=" + intClass.ToString() + "&environmentid=" + intEnv.ToString() + "&delete=true");
            }
            else
                Response.Redirect(Request.Path + "?modelid=" + intModel.ToString() + "&classid=" + intClass.ToString() + "&environmentid=" + intEnv.ToString());
        }
        protected void btnDown_Click(Object Sender, EventArgs e)
        {
            int intIndex = lstCurrent.SelectedIndex;
            if (intIndex > -1 && intIndex < lstCurrent.Items.Count - 1)
            {
                ListItem oTemp = lstCurrent.SelectedItem;
                lstCurrent.Items.Remove(lstCurrent.SelectedItem);
                lstCurrent.Items.Insert(intIndex + 1, oTemp);
                ReOrder();
            }
            else
                Response.Redirect(Request.Path + "?modelid=" + intModel.ToString() + "&classid=" + intClass.ToString() + "&environmentid=" + intEnv.ToString());
        }
        private void ReOrder()
        {
            int intDisplay = 0;
            foreach (ListItem oItem in lstCurrent.Items)
            {
                intDisplay++;
                oModel.UpdateReservationList(Int32.Parse(oItem.Value), intDisplay);
            }
            Response.Redirect(Request.Path + "?modelid=" + intModel.ToString() + "&classid=" + intClass.ToString() + "&environmentid=" + intEnv.ToString() + "&save=true");
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
