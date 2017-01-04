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
    public partial class asset_enclosures : BasePage
    {

    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    protected Asset oAsset;
    protected int intProfile;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cookies["loginreferrer"].Value = "/admin/asset/asset_enclosures.aspx";
        Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
        if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
        else
            Response.Redirect("/admin/login.aspx");
        oAsset = new Asset(intProfile, dsnAsset);
        if (!IsPostBack)
        {
            LoadList();
            LoopRepeater();
            btnCancel.Attributes.Add("onclick", "return Cancel();");
        }
    }
    private void LoadList()
    {
        ddlDR.DataTextField = "name";
        ddlDR.DataValueField = "id";
        ddlDR.DataSource = oAsset.GetEnclosuresDR();
        ddlDR.DataBind();
        ddlDR.Items.Insert(0, new ListItem("-- SELECT --", "0"));
    }
    private void LoopRepeater()
    {
        Classes oClass = new Classes(0, dsn);
        //int intClass = 0;
        //DataSet dsClass = oClass.Gets(1);
        //foreach (DataRow drClass in dsClass.Tables[0].Rows)
        //{
        //    if (drClass["prod"].ToString() == "1" && drClass["pnc"].ToString() != "1")
        //    {
        //        intClass = Int32.Parse(drClass["id"].ToString());
        //        break;
        //    }
        //}
        //DataSet ds = oAsset.GetEnclosuresClass(intClass);
        string strClass = "";
        DataSet dsClass = oClass.Gets(1);
        foreach (DataRow drClass in dsClass.Tables[0].Rows)
        {
            if (drClass["prod"].ToString() == "1")
                strClass += drClass["id"] + ",";
        }
        string[] strValues;
        char[] strSplit = { ',' };
        strValues = strClass.Split(strSplit);
        Functions oFunction = new Functions(0, dsn, intEnvironment);
        strClass = oFunction.BuildXmlString("data", strValues);
        DataSet ds = oAsset.GetEnclosuresClass(strClass);
        
        DataView dv = ds.Tables[0].DefaultView;
        if (Request.QueryString["sort"] != null)
            dv.Sort = Request.QueryString["sort"].ToString();
        rptView.DataSource = dv;
        rptView.DataBind();
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
    protected void btnSave_Click(Object Sender, EventArgs e)
    {
        oAsset.AddEnclosure(Int32.Parse(Request.Form[hdnId.UniqueID]), Int32.Parse(ddlDR.SelectedItem.Value));
        Response.Redirect(Request.Path);
    }

    }
}
