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
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Presentation.Web
{
    public partial class pcr_csrc_nicknames : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected TPM oTPM;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            bool boolFound = false;
            string strType = Request.QueryString["type"];
            if ((Request.QueryString["rid"] != null && Request.QueryString["rid"] != "") && (Request.QueryString["item"] != null && Request.QueryString["item"] != "") && (Request.QueryString["num"] != null && Request.QueryString["num"] != ""))
            {
                string name = Request.QueryString["name"];
                oTPM = new TPM(intProfile, dsn, intEnvironment);
                int intRequest = Int32.Parse(Request.QueryString["rid"]);
                int intItem = Int32.Parse(Request.QueryString["item"]);
                int intNumber = Int32.Parse(Request.QueryString["num"]);

                DataSet ds;
                if (strType == "PCR")
                    ds = oTPM.GetPCRs(intRequest, intItem, intNumber);
                else
                    ds = oTPM.GetCSRCs(intRequest, intItem, intNumber);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["name"].ToString().ToLower().Trim() == name.ToLower().Trim())
                    {
                        boolFound = true;
                        break;
                    }
                }

                Response.ContentType = "application/text";
                Response.Write(boolFound);
                Response.End();
            }
        }
    }
}
