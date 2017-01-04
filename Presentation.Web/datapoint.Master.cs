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
using NCC.ClearView.Application.UI.BusinessLogic;

namespace NCC.ClearView.Presentation.Web
{
    public partial class datapoint : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.LoadComplete += new EventHandler(Page_LoadComplete);

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            try
            {

                if (ConfigurationManager.AppSettings["CONTROLS_ADD_HELP"].ToString() == "1")
                {
                    //Lookup for all ContentPlaceHolders 
                    //ContentPlaceHolders will have Page and Page will contain UserControls
                    foreach (string cphId in this.ContentPlaceHolders)
                    {
                        ContentPlaceHolder oPlaceHolder = (ContentPlaceHolder)this.FindControl(cphId);
                        System.Web.UI.Page oPage = oPlaceHolder.Page;
                        UIToolTipHelpValidations.LoadToolTipHelpValidations(ref oPage);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
