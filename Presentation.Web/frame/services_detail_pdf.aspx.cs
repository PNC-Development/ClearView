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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class services_detail_pdf : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;

        protected ServiceDetails oServiceDetail;
        protected Services oService;
        protected string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
        protected int intCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            oServiceDetail = new ServiceDetails(0, dsn);
            oService = new Services(0, dsn);
            StringBuilder sbView = new StringBuilder();
            if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
            {
                string _services = Request.QueryString["sid"];
                string[] strServices;
                char[] strSplit = { ' ' };
                strServices = _services.Split(strSplit);
                for (int ii = 0; ii < strServices.Length; ii++)
                {
                    if (strServices[ii].Trim() != "")
                    {
                        int intService = Int32.Parse(strServices[ii]);
                        sbView.Append("<tr><td bgcolor=\"#6A8359\" class=\"whitedefault\"><div style=\"padding:3\"><b>");
                        sbView.Append(oService.GetName(intService));
                        sbView.Append("</b></div></td></tr>");
                        sbView.Append("<tr><td style=\"border-left:solid 1px #999999;border-right:solid 1px #999999;border-bottom:solid 1px #999999;\"><div style=\"padding:3\">");
                        sbView.Append(GetSummary(intService, 0, 1.00, 1, 1));
                        sbView.Append("</div></td></tr>");
                        sbView.Append("<tr><td>&nbsp;</td></tr>");
                    }
                }
                sbView.Insert(0, "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"default\">");
                sbView.Append("</table>");
            }
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            //StringWriter stringWriter = new StringWriter();
            //HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
            //htmlTextWriter.Write(strView);
            //this.RenderControl(htmlTextWriter);
            //Response.Write(stringWriter.ToString());
            //Response.Write(strView);

            //        Response.BinaryWrite(strView);
            Response.Flush();
            Response.Close();

            //Response.Flush();
            //Response.Close();
            //Response.End();
        }
        private string GetSummary(int _serviceid, int _id, double _quantity, int _state, int _level)
        {
            StringBuilder sbSummary = new StringBuilder();
            DataSet ds = oServiceDetail.Gets(_serviceid, _id, 1);
            double dblTotalHours = 0.00;
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intID = Int32.Parse(dr["id"].ToString());
                    intCount++;
                    if (sbSummary.ToString() != "")
                    {
                        sbSummary.Append(strSpacerRow);
                    }
                    sbSummary.Append("<tr>");
                    DataSet dsServices = oServiceDetail.Gets(_serviceid, intID, 1);
                    if (dsServices.Tables[0].Rows.Count == 0)
                    {
                        sbSummary.Append("<td valign=\"top\">&nbsp;</td>");
                    }
                    else
                    {
                        if (_state == 0 || _state == 1)
                        {
                            sbSummary.Append("<td valign=\"top\"><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgSummary_");
                            sbSummary.Append(intCount.ToString());
                            sbSummary.Append("','divSummary_");
                            sbSummary.Append(intCount.ToString());
                            sbSummary.Append("');\"><img id=\"imgSummary_");
                            sbSummary.Append(intCount.ToString());
                            sbSummary.Append("\" src=\"/images/minus.gif\" border=\"0\" align=\"absmiddle\"></a>&nbsp;</td>");
                        }
                        else
                        {
                            sbSummary.Append("<td valign=\"top\"><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgSummary_");
                            sbSummary.Append(intCount.ToString());
                            sbSummary.Append("','divSummary_");
                            sbSummary.Append(intCount.ToString());
                            sbSummary.Append("');\"><img id=\"imgSummary_");
                            sbSummary.Append(intCount.ToString());
                            sbSummary.Append("\" src=\"/images/plus.gif\" border=\"0\" align=\"absmiddle\"></a>&nbsp;</td>");
                        }
                    }
                    sbSummary.Append("<td width=\"100%\" valign=\"top\">");
                    sbSummary.Append(dr["name"].ToString());
                    sbSummary.Append("</td>");
                    double dblSingle = 0.00;
                    double dblAdditional = 0.00;
                    string strX = "";
                    string strTotal = "";
                    for (double ii = 0.00; ii < _quantity; ii = ii + 1.00)
                    {
                        if (ii == 0.00)
                        {
                            dblSingle += oServiceDetail.GetDetailHours(_serviceid, intID, false);
                        }
                        else
                        {
                            dblAdditional += oServiceDetail.GetDetailHours(_serviceid, intID, true);
                        }
                    }
                    double dblHours = dblSingle + dblAdditional;
                    bool boolAdditional = false;
                    dblSingle = 0.00;
                    dblAdditional = 0.00;
                    for (double ii = 0.00; ii < _quantity; ii = ii + 1.00)
                    {
                        if (ii == 0.00)
                        {
                            dblSingle += oServiceDetail.GetDetailHours(_serviceid, intID, false);
                        }
                        else
                        {
                            dblAdditional = oServiceDetail.GetDetailHours(_serviceid, intID, true);
                            if (dblSingle != dblAdditional)
                            {
                                boolAdditional = true;
                                double _new_quantity = _quantity - 1.00;
                                double _new_hours = dblHours - dblSingle;
                                strX = dblSingle.ToString("F") + " HRs x 1 Qty = <br/>" + dblAdditional.ToString("F") + " HRs x " + _new_quantity.ToString() + " Qty = ";
                                strTotal = dblSingle.ToString("F") + " HRs<br/>" + _new_hours.ToString("F") + " HRs";
                            }
                            break;
                        }
                    }
                    if (boolAdditional == false)
                    {
                        strX = dblSingle.ToString("F") + " HRs x " + _quantity.ToString() + " Qty = ";
                        strTotal = dblHours.ToString("F") + " HRs";
                    }
                    sbSummary.Append("<td nowrap valign=\"top\" align=\"right\">");
                    sbSummary.Append(strX);
                    sbSummary.Append("</td>");
                    sbSummary.Append("<td nowrap align=\"right\" valign=\"top\">");
                    sbSummary.Append(strTotal);
                    sbSummary.Append("</td>");
                    sbSummary.Append("</tr>");
                    sbSummary.Append(strSpacerRow);
                    sbSummary.Append("<tr><td></td>");
                    if (_state == 0 || _state == 1)
                    {
                        sbSummary.Append("<td colspan=\"3\" valign=\"top\" id=\"divSummary_");
                        sbSummary.Append(intCount.ToString());
                        sbSummary.Append("\" style=\"display:inline\">");
                        sbSummary.Append(GetSummary(_serviceid, intID, _quantity, _state, _level + 1));
                        sbSummary.Append("</td>");
                    }
                    else
                    {
                        sbSummary.Append("<td colspan=\"3\" valign=\"top\" id=\"divSummary_");
                        sbSummary.Append(intCount.ToString());
                        sbSummary.Append("\" style=\"display:none\">");
                        sbSummary.Append(GetSummary(_serviceid, intID, _quantity, _state, _level + 1));
                        sbSummary.Append("</td>");
                    }
                    sbSummary.Append("</tr>");
                    dblTotalHours += dblHours;
                }
                sbSummary.Append(strSpacerRow);
                sbSummary.Append("<tr>");
                sbSummary.Append("<td colspan=\"2\" style=\"color:#990000\">Total</td>");
                sbSummary.Append("<td colspan=\"2\" align=\"right\" style=\"color:#990000\">");
                sbSummary.Append(dblTotalHours.ToString("F"));
                sbSummary.Append(" HRs</td>");
                sbSummary.Append("</tr>");
            }
            if (sbSummary.ToString() != "")
            {
                if (_level == 1)
                {
                    sbSummary.Insert(0, "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:13px; font-weight:bold;\">");
                    sbSummary.Append("</table>");
                }
                else if (_level == 2)
                {
                    sbSummary.Insert(0, "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:13px;\">");
                    sbSummary.Append("</table>");
                }
                else
                {
                    sbSummary.Insert(0, "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"font-family:'openSans', Verdana, Arial, Helvetica, sans-serif; font-size:12px; color:#808080\">");
                    sbSummary.Append("</table>");
                }
            }

            return sbSummary.ToString();
        }
    }
}
