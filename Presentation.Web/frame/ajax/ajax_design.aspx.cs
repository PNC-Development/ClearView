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
using System.Xml;
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class ajax_design : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Design oDesign;
        protected void Page_Load(object sender, EventArgs e)
        {
            oDesign = new Design(0, dsn);
            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                XmlDocument oDoc = new XmlDocument();
                oDoc.Load(Request.InputStream);
                Response.ContentType = "application/xml";
                StringBuilder sb = new StringBuilder("<values>");
                string strValues = Server.UrlDecode(oDoc.FirstChild.InnerXml);
                // Format: 3_5 where 3 = QuestionID and 5 = ResponseID
                int intDesign = 0;
                int intQuestion = 0;
                int intResponse = 0;
                bool boolSelected = false;
                if (strValues.Contains("_") == true)
                {
                    Int32.TryParse(strValues.Substring(0, strValues.IndexOf("_")), out intDesign);
                    strValues = strValues.Substring(strValues.IndexOf("_") + 1);
                    Int32.TryParse(strValues.Substring(0, strValues.IndexOf("_")), out intQuestion);
                    strValues = strValues.Substring(strValues.IndexOf("_") + 1);
                    Int32.TryParse(strValues.Substring(0, strValues.IndexOf("_")), out intResponse);
                    boolSelected = (strValues.Substring(strValues.IndexOf("_") + 1) == "1");
                }
                if (intQuestion > 0 && intResponse == 0)
                {
                    // Hide all related ones
                    DataSet dsHide = oDesign.GetShows(intQuestion, 0);
                    foreach (DataRow drHide in dsHide.Tables[0].Rows)
                    {
                        sb.Append("<value>");
                        sb.Append(drHide["questionid"].ToString());
                        sb.Append("</value><text>");
                        sb.Append("none");
                        sb.Append("</text>");
                        // Loop through the children also
                        //sb.Append(HideAll(Int32.Parse(drHide["questionid"].ToString())));
                    }
                }
                if (intResponse > 0)
                {
                    sb.Append(ShowResponses(intResponse, boolSelected, intDesign));
                }
                sb.Append("</values>");
                Response.Write(sb.ToString());
                Response.End();
            }
        }
        public string HideAll(int _questionid)
        {
            StringBuilder strReturn = new StringBuilder("");
            DataSet dsResponse = oDesign.GetResponses(_questionid, 1, 1);
            foreach (DataRow drResponse in dsResponse.Tables[0].Rows)
            {
                int intResponse = Int32.Parse(drResponse["id"].ToString());
                DataSet dsShow = oDesign.GetShowsRelated(intResponse, 0);
                foreach (DataRow drShow in dsShow.Tables[0].Rows)
                {
                    strReturn.Append("<value>");
                    strReturn.Append(drShow["questionid"].ToString());
                    strReturn.Append("</value><text>");
                    strReturn.Append("none");
                    strReturn.Append("</text>");
                    strReturn.Append(HideAll(Int32.Parse(drShow["questionid"].ToString())));
                }
            }
            return strReturn.ToString();
        }
        public string ShowResponses(int _responseid, bool _selected, int _designid)
        {
            StringBuilder strReturn = new StringBuilder("");
            // Show all related ones
            DataSet dsShow = oDesign.GetShowsRelated(_responseid, 0);
            foreach (DataRow drShow in dsShow.Tables[0].Rows)
            {
                strReturn.Append("<value>");
                int intQuestion = Int32.Parse(drShow["questionid"].ToString());
                strReturn.Append(intQuestion.ToString());
                strReturn.Append("</value><text>");
                strReturn.Append(_selected ? "inline" : "none");
                strReturn.Append("</text>");
                if (_selected == false)
                {
                    // Loop through the children also
                    strReturn.Append(HideAll(Int32.Parse(drShow["questionid"].ToString())));
                }
                else
                {
                    // Check to see if the shown question has only one response (and if selected, show all of those questions too).
                    bool boolUnder48 = oDesign.IsUnder48(_designid, true);
                    // Check if the response is the only one and SELECT IF ONE is enabled.
                    //DataSet dsResponse = oDesign.GetResponses(intQuestion, (boolUnder48 ? 1 : 0), (boolUnder48 ? 0 : 1), 1, 1);
                    DataRow[] drResponses = oDesign.LoadResponses(_designid, intQuestion, boolUnder48);
                    int intResponseOne = 0;
                    //int intResponseCount = dsResponse.Tables[0].Rows.Count;
                    int intResponseCount = drResponses.Length;
                    //foreach (DataRow drResponse in dsResponse.Tables[0].Rows)
                    foreach (DataRow drResponse in drResponses)
                    {
                        int intResponseTemp = Int32.Parse(drResponse["id"].ToString());
                        //if (oDesign.IsResponseVisible(_designid, intResponseTemp) == false)
                        if (drResponse["visible"].ToString() != "1")
                            intResponseCount--;
                        else
                        {
                            if (intResponseOne != 0)    // No longer the default, so must be more than one valid response.
                                break;
                            intResponseOne = intResponseTemp;
                        }
                    }
                    if (intResponseCount == 1 && oDesign.GetResponse(intResponseOne, "select_if_one") == "1")
                    {
                        strReturn.Append(ShowResponses(intResponseOne, true, _designid));
                    }
                    else
                    {
                        foreach (DataRow drResponse in drResponses)
                        {
                            //if (drResponse["selected"].ToString() == "1" && drResponse["QuestionShows"].ToString() != "")
                            if (drResponse["selected"].ToString() == "1" && drResponse["QuestionShowsDisabled"].ToString() != "")
                            {
                                // The selected response of the newly shown question has other questions that should be shown / hidden.
                                strReturn.Append(ShowResponses(Int32.Parse(drResponse["id"].ToString()), true, _designid));
                            }
                        }
                    }
                }
            }
            return strReturn.ToString();
        }
    }
}
