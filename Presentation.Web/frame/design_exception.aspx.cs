using NCC.ClearView.Application.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCC.ClearView.Presentation.Web
{
    public partial class design_exception : System.Web.UI.Page
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;

        protected Design oDesign;
        protected Users oUser;

        protected string ARB = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oDesign = new Design(intProfile, dsn);
            oUser = new Users(intProfile, dsn);

            if (!IsPostBack)
            {
                DataSet dsGroup = oDesign.GetApproverGroups(1, 1);
                foreach (DataRow drGroup in dsGroup.Tables[0].Rows)
                {
                    int intGroup = Int32.Parse(drGroup["groupid"].ToString());
                    // Get Users
                    DataSet dsUser = oDesign.GetApprovalUsers(intGroup);
                    foreach (DataRow drUser in dsUser.Tables[0].Rows)
                    {
                        if (ARB != "")
                            ARB += "\\n";
                        ARB += oUser.GetFullNameWithLanID(Int32.Parse(drUser["userid"].ToString())).Replace("'", "\\'");
                    }
                }
                btnAgree.Attributes.Add("onclick", "return Done(true,'" + chkTerms.ClientID + "');");
                btnDisagree.Attributes.Add("onclick", "return Done(false,'" + chkTerms.ClientID + "');");
            }
        }
    }
}