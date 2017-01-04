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
using Microsoft.ApplicationBlocks.Data;
using NCC.ClearView.Application.Core;

namespace NCC.ClearView.Presentation.Web
{
    public partial class fix_leads : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected string strResults = "";
        protected int intCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/fix_leads.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            btnGo.Attributes.Add("onclick", "return confirm('This function will modify production data and cannot be reversed!\\n\\nAre you sure you want to continue?') && ProcessButton(this);");
        }
        protected  void btnGo_Click(Object Sender, EventArgs e)
        {
            Requests oRequest = new Requests(0, dsn);
            Projects oProject = new Projects(0, dsn);
            DataSet dsApplications = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_applications WHERE tpm = 1 AND deleted = 0");
            foreach (DataRow drApplication in dsApplications.Tables[0].Rows)
            {
                DataSet dsItems = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_request_items WHERE itemid = " + drApplication["applicationid"].ToString() + " AND deleted = 0");
                foreach (DataRow drItem in dsItems.Tables[0].Rows)
                {
                    int intItem = Int32.Parse(drItem["itemid"].ToString());
                    DataSet dsRR = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_resource_requests WHERE itemid = " + intItem.ToString() + " AND userid > 0 AND deleted = 0");
                    foreach (DataRow drRR in dsRR.Tables[0].Rows)
                    {
                        int intRequest = Int32.Parse(drRR["requestid"].ToString());
                        int intProject = oRequest.GetProjectNumber(intRequest);
                        if (oProject.Get(intProject, "lead") == "" || oProject.Get(intProject, "lead") == "0")
                        {
                            oProject.Update(intProject, Int32.Parse(drRR["userid"].ToString()), 0, 0, 0, 0, 0);
                            intCount++;
                        }
                    }
                }
            }
            DataSet dsPC = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_resource_requests WHERE itemid = 0 AND userid > 0 AND deleted = 0");
            foreach (DataRow drPC in dsPC.Tables[0].Rows)
            {
                int intRequest = Int32.Parse(drPC["requestid"].ToString());
                int intProject = oRequest.GetProjectNumber(intRequest);
                if (oProject.Get(intProject, "lead") == "" || oProject.Get(intProject, "lead") == "0")
                {
                    oProject.Update(intProject, Int32.Parse(drPC["userid"].ToString()), 0, 0, 0, 0, 0);
                    intCount++;
                }
            }
            lblDone.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView updated " + intCount.ToString() + " projects successfully";
        }
    }
}
