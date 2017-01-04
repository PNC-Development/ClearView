using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;

namespace NCC.ClearView.Application.Core
{
    class Upload : IHttpHandler
    {
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private Variables oVariables;

        public Upload()
        {
            oVariables = new Variables(intEnvironment);
        }

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files.Count > 0)
            {
                string strType = "unavailable";
                string strId = "unavailable";
                if (context.Request.Cookies["projectid"] != null && context.Request.Cookies["projectid"].Value != "")
                {
                    strType = "projects";
                    strId = context.Request.Cookies["projectid"].Value;
                }
                else if (context.Request.Cookies["requestid"] != null && context.Request.Cookies["requestid"].Value != "")
                {
                    strType = "requests";
                    strId = context.Request.Cookies["requestid"].Value;
                }
                else if (context.Request.Cookies["resourceid"] != null && context.Request.Cookies["resourceid"].Value != "")
                {
                    strType = "resource";
                    strId = context.Request.Cookies["resourceid"].Value;
                }
                // loop through all the uploaded files
                for (int j = 0; j < context.Request.Files.Count; j++)
                {
                    // get the current file
                    HttpPostedFile uploadFile = context.Request.Files[j];
                    // if there was a file uploded
                    if (uploadFile.ContentLength > 0)
                    {
                        // save the file to the upload directory
                        uploadFile.SaveAs(oVariables.UploadsFolder() + strType + "\\" + strId + "\\" + uploadFile.FileName);
                    }
                }
            }
            // Used as a fix for a bug in mac flash player that makes the
            // onComplete event not fire
            HttpContext.Current.Response.Write(" ");
        }

        #endregion
    }
}
