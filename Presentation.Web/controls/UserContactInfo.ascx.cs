using System;
using System.Data;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NCC.ClearView.Application.Core;
using NCC.ClearView.Presentation.Web.Custom;


namespace NCC.ClearView.Presentation.Web
{
    public partial class UserContactInfo : System.Web.UI.UserControl
    {
        protected string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Users oUser;
        protected Variables oVariable;
        protected Functions oFunction;
        protected Users_At oUserAt;
        protected int intProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Initialize Variables
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oUser = new Users(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oUserAt = new Users_At(intProfile, dsn);

            //Initial Page Load
            if (!IsPostBack)
            {
                LoadList();
                hdnUserId.Value = intUserId.ToString();
                LoadUserContactDetails(intUserId);
            }

            btnSaveContactInfo.Attributes.Add("onclick", "return ProcessButton(this)" +
                                              ";");
            
        }

        #region Properties
            private int intUserId;
            public int UserId
            {
                get { return intUserId; }
                set { intUserId = value; }
            }

        #endregion

        
        #region User Contact Details
            private void LoadList()
            {

                DataSet ds = oUserAt.Gets(1);

                ddlWorkPagerAt.DataTextField = "name";
                ddlWorkPagerAt.DataValueField = "atid";
                ddlWorkPagerAt.DataSource = ds;
                ddlWorkPagerAt.DataBind();
                ddlWorkPagerAt.Items.Insert(0, new ListItem("-- SELECT --", "0"));

                ddlHomePagerAt.DataTextField = "name";
                ddlHomePagerAt.DataValueField = "atid";
                ddlHomePagerAt.DataSource = ds;
                ddlHomePagerAt.DataBind();
                ddlHomePagerAt.Items.Insert(0, new ListItem("-- SELECT --", "0"));

            }
            private void LoadUserContactDetails(int intUserId)
            {
                if (intProfile != intUserId)
                    btnSaveContactInfo.Enabled = false;
                else
                    btnSaveContactInfo.Enabled = true;

                DataSet dsContactInfo = oUser.GetUserContactInfo(intUserId);

                if (dsContactInfo.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsContactInfo.Tables[0].Rows[0];
                    txtWorkTitle.Text = dr["Work_Title"].ToString();
                    txtWorkCompany.Text = dr["Work_Company"].ToString();
                    txtWorkDepartment.Text = dr["Work_Department"].ToString();
                    txtWorkPhoneNo.Text = dr["Work_Phone"].ToString();
                    txtWorkFaxNo.Text = dr["Work_Fax"].ToString();
                    txtWorkCellNo.Text = dr["Work_CellPhone"].ToString();
                    txtWorkPager.Text = dr["Work_Pager"].ToString();
                    if (dr["Work_PagerATId"].ToString() != "")
                        ddlWorkPagerAt.Items.FindByValue(dr["Work_PagerATId"].ToString()).Selected = true;
                    else
                        ddlWorkPagerAt.Items.FindByValue("0").Selected = true;

                    txtWorkEmail.Text = dr["Work_Email"].ToString();
                    txtWorkMailLocator.Text = dr["Work_MailLocator"].ToString();
                    txtWorkOfficeNo.Text = dr["Work_OfficeNo"].ToString();
                    txtWorkAddressLine1.Text = dr["Work_AddressLine1"].ToString();
                    txtWorkAddressLine2.Text = dr["Work_AddressLine2"].ToString();
                    txtWorkCity.Text = dr["Work_City"].ToString();
                    txtWorkState.Text = dr["Work_State"].ToString();
                    txtWorkZIP.Text = dr["Work_ZIP"].ToString();
                    txtWorkCountry.Text = dr["Work_Country"].ToString();

                    txtHomePhoneNo.Text = dr["Home_Phone"].ToString();
                    txtHomeFaxNo.Text = dr["Home_Fax"].ToString();
                    txtHomeCellNo.Text = dr["Home_CellPhone"].ToString();
                    txtHomePager.Text = dr["Home_Pager"].ToString();
                    if (dr["Home_PagerATId"].ToString() != "")
                        ddlHomePagerAt.Items.FindByValue(dr["Home_PagerATId"].ToString()).Selected = true;
                    else
                        ddlHomePagerAt.Items.FindByValue("0").Selected = true;

                    txtHomeEmail.Text = dr["Home_Email"].ToString();
                    txtHomeAddressLine1.Text = dr["Home_AddressLine1"].ToString();
                    txtHomeAddressLine2.Text = dr["Home_AddressLine2"].ToString();
                    txtHomeCity.Text = dr["Home_City"].ToString();
                    txtHomeState.Text = dr["Home_State"].ToString();
                    txtHomeZip.Text = dr["Home_ZIP"].ToString();
                    txtHomeCountry.Text = dr["Home_Country"].ToString();

                }

            }
            protected void btnSaveContactInfo_Click(Object Sender, EventArgs e)
            {
                intUserId = Int32.Parse(hdnUserId.Value);
               

                oUser.AddUpdateUserContactInfo(intUserId,
                                               txtWorkTitle.Text.Trim(),
                                               txtWorkCompany.Text.Trim(),
                                               txtWorkDepartment.Text.Trim(),
                                               txtWorkPhoneNo.Text.Trim(),
                                               txtWorkFaxNo.Text.Trim(),
                                               txtWorkCellNo.Text.Trim(),
                                               txtWorkPager.Text.Trim(),
                                               Int32.Parse(ddlWorkPagerAt.SelectedValue.ToString()),
                                               txtWorkEmail.Text.Trim(),
                                               txtWorkMailLocator.Text.Trim(),
                                               txtWorkOfficeNo.Text.Trim(),
                                               txtWorkAddressLine1.Text.Trim(),
                                               txtWorkAddressLine2.Text.Trim(),
                                               txtWorkCity.Text.Trim(),
                                               txtWorkState.Text.Trim(),
                                               txtWorkZIP.Text.Trim(),
                                               txtWorkCountry.Text.Trim(),
                                               txtHomePhoneNo.Text.Trim(),
                                               txtHomeFaxNo.Text.Trim(),
                                               txtHomeCellNo.Text.Trim(),
                                               txtHomePager.Text.Trim(),
                                               Int32.Parse(ddlHomePagerAt.SelectedValue.ToString()),
                                               txtHomeEmail.Text.Trim(),
                                               txtHomeAddressLine1.Text.Trim(),
                                               txtHomeAddressLine2.Text.Trim(),
                                               txtHomeCity.Text.Trim(),
                                               txtHomeState.Text.Trim(),
                                               txtHomeZip.Text.Trim(),
                                               txtHomeCountry.Text.Trim(),
                                               intProfile,
                                               intProfile,
                                               1,
                                               0);

                lblContactInfoSaved.Visible = true;
                LoadUserContactDetails(intUserId);
            }
        #endregion
    }
}