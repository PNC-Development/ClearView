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
    public partial class mnemonics_control : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected Mnemonic oMnemonic;
        protected Pages oPage;
        protected Applications oApplication;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oMnemonic = new Mnemonic(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = "PNC Mnemonics";

            if (!IsPostBack)
            {
                string strFilter = "";
                if (Request.QueryString["f"] != null)
                    strFilter = Request.QueryString["f"].Trim();
                txtFilter.Text = strFilter;
                txtFilter.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnFilter.ClientID + "').click();return false;}} else {return true}; ");
                btnFilter.Attributes.Add("onclick", "return ValidateText('" + txtFilter.ClientID + "','Please enter a filter') && WaitDDL('" + trWait.ClientID + "');");
                btnFilterRemove.Attributes.Add("onclick", "return WaitDDL('" + trWait.ClientID + "');");

                string strSort = "";
                if (Request.QueryString["o"] != null)
                    strSort = Request.QueryString["o"].Trim();
                if (strSort != "")
                    ddlOrder.SelectedValue = strSort;
                ddlOrder.Attributes.Add("onchange", "WaitDDL('" + trWait.ClientID + "');");

                DataSet dsNew = oMnemonic.GetRecent(1, strFilter);
                DataView dvNew = dsNew.Tables[0].DefaultView;
                try
                {
                    if (strSort != "")
                        dvNew.Sort = strSort;
                }
                catch { }
                rptNew.DataSource = dvNew;
                rptNew.DataBind();
                lblNew.Visible = (rptNew.Items.Count == 0);

                DataSet dsOld = oMnemonic.GetRecent(0, strFilter);
                DataView dvOld = dsOld.Tables[0].DefaultView;
                try
                {
                    if (strSort != "")
                        dvOld.Sort = strSort;
                }
                catch { }
                rptOld.DataSource = dvOld;
                rptOld.DataBind();
                lblOld.Visible = (rptOld.Items.Count == 0);

                if (strFilter != "")
                {
                    trFilter.Visible = true;
                    litFilter.Text = "Your filter for &quot;" + strFilter + "&quot; returned " + rptOld.Items.Count.ToString() + " results...";
                }
                else
                    btnFilterRemove.Enabled = false;
            }
        }

        protected void ddlOrder_Change(Object Sender, EventArgs e)
        {
            Response.Redirect("/" + oApplication.Get(intApplication, "url") + "/default.aspx?o=" + ddlOrder.SelectedItem.Value + "&f=" + txtFilter.Text);
        }

        protected void btnFilter_Click(Object Sender, ImageClickEventArgs e)
        {
            Response.Redirect("/" + oApplication.Get(intApplication, "url") + "/default.aspx?o=" + ddlOrder.SelectedItem.Value + "&f=" + txtFilter.Text);
        }

        protected void btnFilterRemove_Click(Object Sender, ImageClickEventArgs e)
        {
            Response.Redirect("/" + oApplication.Get(intApplication, "url") + "/default.aspx?o=" + ddlOrder.SelectedItem.Value + "&f=");
        }

    }
}