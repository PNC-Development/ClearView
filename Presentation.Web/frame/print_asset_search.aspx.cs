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
    public partial class print_asset_search : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
                LoadSearch(Int32.Parse(Request.QueryString["sid"]));
        }
        private void LoadSearch(int _search)
        {
            Asset oAsset = new Asset(intProfile, dsnAsset);
            Classes oClasses = new Classes(intProfile, dsn);
            Environments oEnvironment = new Environments(intProfile, dsn);
            Platforms oPlatform = new Platforms(intProfile, dsn);
            Types oType = new Types(intProfile, dsn);
            Models oModel = new Models(intProfile, dsn);
            Depot oDepot = new Depot(intProfile, dsn);
            DataSet ds = oAsset.GetSearch(_search);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["userid"].ToString() == intProfile.ToString())
                {
                    string strSearch = ds.Tables[0].Rows[0]["type"].ToString();
                   
                    switch (strSearch)
                    {
                        case "N":
                            if (ds.Tables[0].Rows[0]["name"].ToString().Trim() != "")
                            {
                                lblResults.Text = "Device Name LIKE &quot;" + ds.Tables[0].Rows[0]["name"].ToString().Trim() + "&quot;";
                            }
                            if (ds.Tables[0].Rows[0]["serial"].ToString().Trim() != "")
                            {
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text = "Serial Number LIKE &quot;" + ds.Tables[0].Rows[0]["serial"].ToString().Trim() + "&quot;";
                            }
                            if (ds.Tables[0].Rows[0]["asset"].ToString().Trim() != "")
                            {
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text = "Asset Tag LIKE &quot;" + ds.Tables[0].Rows[0]["asset"].ToString().Trim() + "&quot;";
                            }
                            break;
                        case "C":
                            if (ds.Tables[0].Rows[0]["classid"].ToString().Trim() != "0")
                            {
                                lblResults.Text = "Class = &quot;" + oClasses.Get(Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString().Trim()), "name") + "&quot;";
                            }
                            if (ds.Tables[0].Rows[0]["environmentid"].ToString().Trim() != "0")
                            {
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Environment = &quot;" + oEnvironment.Get(Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString().Trim()), "name") + "&quot;";
                            }
                            break;
                        case "T":
                            string strPlatform = ds.Tables[0].Rows[0]["platformid"].ToString().Trim();
                            if (strPlatform != "" && strPlatform != "0")
                            {
                                lblResults.Text = "Platform = &quot;" + oPlatform.GetName(Int32.Parse(strPlatform)) + "&quot;";
                            }
                            string strType = ds.Tables[0].Rows[0]["typeid"].ToString().Trim();
                            if (strType != "" && strType != "0")
                            {
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Asset Type = &quot;" + oType.Get(Int32.Parse(strType), "name") + "&quot;";
                            }
                            string strModel = ds.Tables[0].Rows[0]["modelid"].ToString().Trim();
                            if (strModel != "" && strModel != "0")
                            {
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Model = &quot;" + oModel.Get(Int32.Parse(strModel), "name") + "&quot;";
                            }
                            break;
                        case "D":
                            string strDepot = ds.Tables[0].Rows[0]["depotid"].ToString().Trim();
                            if (strDepot != "" && strDepot != "0")
                            {
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Depot = &quot;" + oDepot.Get(Int32.Parse(strDepot), "name") + "&quot;";
                            }
                            break;
                    }
                    ds = oAsset.GetSearchResults(_search);
                    DataView dv = ds.Tables[0].DefaultView;
                    if (Request.QueryString["sort"] != null)
                        dv.Sort = Request.QueryString["sort"];
                    rptView.DataSource = dv;
                    rptView.DataBind();
                    lblNone.Visible = (rptView.Items.Count == 0);
                }
            }
        }
    }
}
