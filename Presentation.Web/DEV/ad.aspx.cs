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
using System.Xml;

namespace NCC.ClearView.Presentation.Web.DEV
{
    public partial class ad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["xml"] != null && Request.QueryString["xml"] != "")
            {
                string strPath = Server.MapPath(Request.QueryString["xml"]);
                if (System.IO.File.Exists(strPath))
                    adRotator.AdvertisementFile = Request.QueryString["xml"];
            }


            //XmlDocument oDoc = new XmlDocument();
            //oDoc.Load(Server.MapPath(adRotator.AdvertisementFile));
            //int intCount = oDoc.ChildNodes[0].ChildNodes.Count;

            //if (Session["adRotatorAds"] == null)
            //{
            //    // Create an array to hold the ads
            //    Session["adRotatorAds"] = new string[intCount];
            //}
            //else
            //{
            //    // See if all are full (if so, hide this rotator)
            //    string[] oAds = (string[])Session["adRotatorAds"];
            //    bool boolShow = false;
            //    for (int ii = 0; ii < oAds.Length; ii++)
            //    {
            //        if (oAds[ii] == null || oAds[ii] == "")
            //        {
            //            boolShow = true;
            //            break;
            //        }
            //    }
            //    if (boolShow == false)
            //        adRotator.Visible = false;
            //}
        }
        protected void adRotator_AdCreated(object sender, AdCreatedEventArgs e)
        {
            //string strID = e.AdProperties["ID"].ToString();
            //string[] oAds = (string[])Session["adRotatorAds"];
            //for (int ii=0; ii<oAds.Length; ii++)
            //{
            //    if (oAds[ii] == null || oAds[ii] == "")
            //    {
            //        oAds[ii] = strID;
            //        break;
            //    }
            //    else if (oAds[ii] == strID)
            //    {
            //        Response.Redirect(Request.Path);
            //    }
            //}
        }
    }
}
