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
    public partial class mnemonics : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnMnemonics = ConfigurationManager.ConnectionStrings["mnemonicDSN"].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Mnemonic oMnemonic;
        protected int intProfile;
        private int intID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/mnemonics.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oMnemonic = new Mnemonic(intProfile, dsn);
            bool boolView = false;
            if (Request.QueryString["import"] != null)
            {
                panImport.Visible = true;
                int intImport = Int32.Parse(Request.QueryString["import"]);
                if (intImport > 0)
                {
                    btnImport.Enabled = false;
                    txtImport.Text = oMnemonic.GetImport(intImport, "results");
                }
                rptImport.DataSource = oMnemonic.GetImports();
                rptImport.DataBind();
            }
            else if (Request.QueryString["id"] == null)
            {
                if (!IsPostBack) 
                {
                    if (Request.QueryString["parent"] == null)
                        boolView = true;
                    else
                    {
                        panAdd.Visible = true;
                    }
                }
            }
            else
            {
                intID = Int32.Parse(Request.QueryString["id"]);
                if (!IsPostBack)
                {
                    panAdd.Visible = true;
                    DataSet ds = oMnemonic.Get(intID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        txtName.Text = dr["name"].ToString();
                        txtFactoryCode.Text = dr["factory_code"].ToString();
                        Status.Text = dr["Status"].ToString();
                        ResRating.Text = dr["ResRating"].ToString();
                        DRRating.Text = dr["DRRating"].ToString();
                        Infrastructure.Text = dr["Infrastructure"].ToString();
                        CriticalityFactor.Text = dr["CriticalityFactor"].ToString();
                        Platform.Text = dr["Platform"].ToString();
                        CICS.Text = dr["CICS"].ToString();
                        PagerLevel.Text = dr["PagerLevel"].ToString();
                        ATLName.Text = dr["ATLName"].ToString();
                        PMName.Text = dr["PMName"].ToString();
                        FMName.Text = dr["FMName"].ToString();
                        DMName.Text = dr["DMName"].ToString();
                        CIO.Text = dr["CIO"].ToString();
                        AppOwner.Text = dr["AppOwner"].ToString();
                        AppLOBName.Text = dr["AppLOBName"].ToString();
                        Segment1.Text = dr["Segment1"].ToString();
                        RiskManager.Text = dr["RiskManager"].ToString();
                        BRContact.Text = dr["BRContact"].ToString();
                        AppRating.Text = dr["AppRating"].ToString();
                        Source.Text = dr["Source"].ToString();
                        OriginalApp.Text = dr["OriginalApp"].ToString();
                        chkEnabled.Checked = (dr["enabled"].ToString() == "1");
                    }
                    else
                        boolView = true;
                }
            }

            if (boolView == true)
            {
                panView.Visible = true;
                LoopRepeater();
            }

            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            btnImport.Attributes.Add("onclick", "return confirm('WARNING: This import will modify production data and CANNOT be undone!\\n\\nAre you sure you want to continue?')&& ProcessButton(this) && WaitDDL('" + divWait.ClientID + "');;");
        }
        private void LoopRepeater()
        {
            DataSet ds = oMnemonic.Gets(0);
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
            Response.Redirect(Request.Path + "?parent=0");
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnImportRun_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?import=0");
        }
        protected void btnImport_Click(Object Sender, EventArgs e)
        {
            txtImport.Text = oMnemonic.Import(dsnMnemonics, dsn, intEnvironment, true);
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (intID == 0)
                oMnemonic.Add(txtName.Text, txtFactoryCode.Text, Status.Text, ResRating.Text, DRRating.Text, Infrastructure.Text, CriticalityFactor.Text, Platform.Text, CICS.Text, PagerLevel.Text, ATLName.Text, PMName.Text, FMName.Text, DMName.Text, CIO.Text, AppOwner.Text, AppLOBName.Text, Segment1.Text, RiskManager.Text, BRContact.Text, AppRating.Text, Source.Text, OriginalApp.Text, (chkEnabled.Checked ? 1 : 0));
            else
                oMnemonic.Update(intID, txtName.Text, txtFactoryCode.Text, Status.Text, ResRating.Text, DRRating.Text, Infrastructure.Text, CriticalityFactor.Text, Platform.Text, CICS.Text, PagerLevel.Text, ATLName.Text, PMName.Text, FMName.Text, DMName.Text, CIO.Text, AppOwner.Text, AppLOBName.Text, Segment1.Text, RiskManager.Text, BRContact.Text, AppRating.Text, Source.Text, OriginalApp.Text, (chkEnabled.Checked ? 1 : 0));
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oMnemonic.Enable(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oMnemonic.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oMnemonic.Delete(intID);
            Response.Redirect(Request.Path);
        }
    }
}
