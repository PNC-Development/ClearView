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
    public partial class sys_whatsnew : System.Web.UI.UserControl
    {
        protected DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected WhatsNew oWhatsNew;
        protected Functions oFunction;
        private long lngTotalRecsCount;
        private long lngRecsPerPage = Int64.Parse(ConfigurationManager.AppSettings["CVNEWSPERPAGE"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oWhatsNew = new WhatsNew(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            if (Request.Path == "/index.aspx")
                Response.Redirect("/interior.aspx");

            BindControls();
            lblNew.Visible = (rptNew.Items.Count == 0);
        }

        private void BindControls()
        {
            DataSet ds;
            ds = oWhatsNew.GetNewsWithPaging(Convert.ToInt64(txtHdnPageNo.Value), lngRecsPerPage);
            rptNew.DataSource = ds.Tables[0];
            rptNew.DataBind();

            foreach (RepeaterItem ri in rptNew.Items)
            {
                Label lblAttachment = (Label)ri.FindControl("lblAttachment");
                if (lblAttachment.Text != "")
                {
                    Panel panAttachment = (Panel)ri.FindControl("panAttachment");
                    panAttachment.Visible = true;
                    string strFile = lblAttachment.Text;
                    strFile = strFile.Substring(strFile.LastIndexOf("/") + 1);
                    lblAttachment.Text = "<img src=\"/images/file.gif\" align=\"absmiddle\" border=\"0\"/> <a href=\"" + lblAttachment.Text + "\" target=\"_blank\">" + strFile + "</a>";
                }
            }

            lngTotalRecsCount = long.Parse(ds.Tables[1].Rows[0]["TotalCount"].ToString());
            // Calculate total numbers of pages
            long lngPgCount = lngTotalRecsCount / lngRecsPerPage + ((lngTotalRecsCount % lngRecsPerPage) > 0 ? 1 : 0);


            // Display Next button
            if (lngPgCount > Convert.ToInt64(txtHdnPageNo.Value))
                lnkBtnNext.Visible = true;

            else
                lnkBtnNext.Visible = false;


            // Display Prev button
            if ((Convert.ToInt64(txtHdnPageNo.Value)) > 1)
                lnkBtnPrevious.Visible = true;
            else
                lnkBtnPrevious.Visible = false;

            lblPageNo.Text = "Page " + txtHdnPageNo.Value.ToString() + " of " + lngPgCount.ToString();

        }

        protected void lnkBtnPrevious_Click(object sender, EventArgs e)
        {
            txtHdnPageNo.Value = Convert.ToString(Convert.ToInt64(txtHdnPageNo.Value) - 1);
            BindControls();

        }

        protected void lnkBtnNext_Click(object sender, EventArgs e)
        {
            txtHdnPageNo.Value = Convert.ToString(Convert.ToInt64(txtHdnPageNo.Value) + 1);
            BindControls();

        }
    }
}