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
    public partial class enclosure_name : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intPlatform = Int32.Parse(ConfigurationManager.AppSettings["ServerPlatformID"]);
        protected Pages oPage;
        protected RacksNew oRacksNew;
        protected Locations oLocations;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oRacksNew = new RacksNew(intProfile, dsn);
            oLocations = new Locations(intProfile, dsn);

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            string strName = "";
            if (Request.QueryString["name"] != null)
                strName = Request.QueryString["name"];
            if (!IsPostBack)
            {
                btnParent.Attributes.Add("onclick", "LoadLocationRoomRack('" + "rack" + "','" + hdnParent.ClientID + "', '', '','', '" + lblParent.ClientID + "');return false;");
                btnSubmit.Attributes.Add("onclick", "return ValidateHidden0('" + hdnParent.ClientID + "','" + btnParent.ClientID + "','Please select a rack')" +
                               " && ValidateRadioList('" + radClass.ClientID + "','Please select a class')" +
                               " && ValidateRadioList('" + radPosition.ClientID + "','Please select a position')" +
                               " && ProcessControlButton()" +
                               ";");
            }
            if (strName != "")
            {
                lblName.Text = strName;
                panName.Visible = true;
                if (strName.Length == 11)
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "name", "<script type=\"text/javascript\">alert('Enclosure Name Generated Successfully!\\n\\nEnclosure Name: " + Request.QueryString["name"] + "');<" + "/" + "script>");
                else
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "none", "<script type=\"text/javascript\">alert('Enclosure Name Failed!');<" + "/" + "script>");
            }
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            int intRack = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            string strLocation = oRacksNew.Get(intRack, "locationid");
            int intLocation = 0;
            if (Int32.TryParse(strLocation, out intLocation) == true)
            {
                int intCity = Int32.Parse(oLocations.GetAddress(intLocation, "cityid"));
                string strCity = oLocations.GetCity(intCity, "enclosure_name");
                string strRoom = oRacksNew.Get(intRack, "room");
                string strRack = oRacksNew.Get(intRack, "rack");
                string strRackBefore = "";
                string strRackAfter = "";
                int intTemp = 0;
                for (int ii=0; ii<strRack.Length; ii++)
                {
                    char chrLetter = strRack[ii];
                    if (Int32.TryParse(chrLetter.ToString(), out intTemp) == true)
                        strRackAfter += chrLetter.ToString();
                    else
                        strRackBefore += chrLetter.ToString();
                }
                if (strRackBefore.Length == 1)
                    strRackBefore = "X" + strRackBefore;
                strRack = strRackBefore + strRackAfter;
                if (strRack.Length < 5)
                    strRack = strRackBefore + "0" + strRackAfter;

                string strName = strCity + strRoom + strRack + radClass.SelectedItem.Value + radPosition.SelectedItem.Value;
                Response.Redirect(oPage.GetFullLink(intPage) + "?name=" + strName);
            }
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?name=");
        }
    }
}