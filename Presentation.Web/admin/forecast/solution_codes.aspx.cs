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
    public partial class solution_codes : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Solution oSolution;
        protected  Services oService;
        protected ModelsProperties oModelsProperties;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/forecast/solution_codes.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oSolution = new Solution(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            if (!IsPostBack)
            {
                LoopRepeater();
                btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=S_CODE" + "',false,400,400);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                btnParent.Attributes.Add("onclick", "return OpenWindow('SERVICEBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
                btnModel.Attributes.Add("onclick", "return OpenWindow('SUBMODELBROWSER','" + hdnModel.ClientID + "','&control=" + hdnModel.ClientID + "&controltext=" + lblModel.ClientID + "',false,400,600);");
            }
        }
        private void LoopRepeater()
        {
            DataSet ds = oSolution.GetCodes(0);
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
                if (oEnable.ImageUrl == "images/enabled.gif")
                {
                    oEnable.ToolTip = "Click to disable";
                    oEnable.Attributes.Add("onClick", "return confirm('Are you sure you want to disable this item?');");
                }
                else
                    oEnable.ToolTip = "Click to enable";
            }
        }
        public void OrderView(Object Sender, EventArgs e)
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
            int intService = 0;
            if (Request.Form[hdnParent.UniqueID] != "")
                intService = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            int intModel = 0;
            if (Request.Form[hdnModel.UniqueID] != "")
                intModel = Int32.Parse(Request.Form[hdnModel.UniqueID]);
            if (Request.Form[hdnId.UniqueID] == "0")
                oSolution.AddCode(txtName.Text, intService, intModel, oSolution.GetCodes(0).Tables[0].Rows.Count + 1, (chkEnabled.Checked == true ? 1 : 0));
            else
                oSolution.UpdateCode(Int32.Parse(Request.Form[hdnId.UniqueID]), txtName.Text, intService, intModel, (chkEnabled.Checked == true ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oSolution.UpdateCodeOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oSolution.EnableCode(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oSolution.DeleteCode(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oSolution.DeleteCode(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
