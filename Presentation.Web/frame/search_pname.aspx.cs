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
    public partial class search_pname : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            Projects oProject = new Projects(0, dsn);
            if (Request.QueryString["s"] != null)
            {
                DataSet ds = new DataSet();
                int intApplication = 0;
                if (Request.QueryString["app"] != null)
                {
                    if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                        intApplication = Int32.Parse(Request.Cookies["application"].Value);
                    ds = oProject.GetNames(Request.QueryString["s"], intApplication);
                }
                else
                {
                    ds = oProject.GetNames(Request.QueryString["s"]);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Request.QueryString["NOCV"] != null)
                        {
                            if (dr["number"].ToString().Trim().ToUpper().StartsWith("CV") || dr["number"].ToString().Trim().ToUpper().StartsWith("EPS"))
                                dr.Delete();
                        }
                    }
                }
                rptView.DataSource = ds;
                rptView.DataBind();
                lblNone.Visible = (rptView.Items.Count == 0);
            }
        }
    }
}
