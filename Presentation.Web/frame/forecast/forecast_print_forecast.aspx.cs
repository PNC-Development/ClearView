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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class forecast_print_forecast : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Forecast oForecast;
        protected Users oUser;
        protected Projects oProject;
        protected ProjectsPending oProjectPending;
        protected Requests oRequest;
        protected Organizations oOrganization;
        protected StatusLevels oStatusLevel;
        protected Platforms oPlatform;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected string strQuestions = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                StringBuilder sb = new StringBuilder(strQuestions);
                oForecast = new Forecast(intProfile, dsn);
                oUser = new Users(intProfile, dsn);
                oProject = new Projects(intProfile, dsn);
                oProjectPending = new ProjectsPending(intProfile, dsn, intEnvironment);
                oRequest = new Requests(intProfile, dsn);
                oOrganization = new Organizations(intProfile, dsn);
                oStatusLevel = new StatusLevels();
                oPlatform = new Platforms(intProfile, dsn);
                oModel = new Models(intProfile, dsn);
                oModelsProperties = new ModelsProperties(intProfile, dsn);
                int intForecast = Int32.Parse(Request.QueryString["id"]);
                if (!IsPostBack)
                {
                    Document doc = new Document();
                    iTextSharp.text.Table oTable = new iTextSharp.text.Table(2);
                    oTable.BorderWidth = 0;
                    oTable.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                    oTable.Padding = 2;
                    oTable.Width = 100;
                    iTextSharp.text.Font oFontHeader = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10, 1);
                    iTextSharp.text.Font oFontBold = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 1);
                    iTextSharp.text.Font oFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 0);
                    if (Request.QueryString["export"] != null)
                    {
                        PdfWriter.GetInstance(doc, Response.OutputStream);
                        string strHeader = "ClearView Design Summary";
                        HeaderFooter header = new HeaderFooter(new Phrase(strHeader, new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 6)), false);
                        header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        header.Alignment = 2;
                        doc.Header = header;
                        string strFooter = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                        HeaderFooter footer = new HeaderFooter(new Phrase(strFooter, new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 6)), false);
                        footer.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        footer.Alignment = 2;
                        doc.Footer = footer;
                        doc.Open();
                        //iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(Request.MapPath("~/images/nc_logo.gif"));
                        //gif.Alignment = iTextSharp.text.Image.RIGHT_ALIGN;
                        //gif.ScalePercent(50f); // change it's size
                        //doc.Add(gif);
                        Cell cell = new Cell(new Phrase("General Information", oFontHeader));
                        cell.Colspan = 2;
                        cell.BackgroundColor = new iTextSharp.text.Color(204, 204, 204);
                        oTable.AddCell(cell);
                    }
                    DataSet ds = oForecast.Get(intForecast);
                    int intRequest = 0;
                    sb.Append("<table width=\"100%\" cellpadding=\"4\" cellspacing=\"3\" border=\"0\">");
                    sb.Append("<tr><td colspan=\"2\" class=\"header\">General Information</td></tr>");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        oTable.AddCell(new Cell(new Phrase("Requestor:", oFontBold)));
                        oTable.AddCell(new Cell(new Phrase(oUser.GetFullName(Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString())), oFont)));
                        sb.Append("<tr><td nowrap>Requestor:</td><td width=\"100%\">");
                        sb.Append(oUser.GetFullName(Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString())));
                        sb.Append("</td></tr>");
                        oTable.AddCell(new Cell(new Phrase("Submission Date:", oFontBold)));
                        oTable.AddCell(new Cell(new Phrase(DateTime.Parse(ds.Tables[0].Rows[0]["created"].ToString()).ToLongDateString(), oFont)));
                        sb.Append("<tr><td nowrap>Submission Date:</td><td width=\"100%\">");
                        sb.Append(DateTime.Parse(ds.Tables[0].Rows[0]["created"].ToString()).ToLongDateString());
                        sb.Append("</td></tr>");
                        intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                    }
                    int intManager = 0;
                    int intEngineer = 0;
                    int intLead = 0;
                    int intProject = oRequest.GetProjectNumber(intRequest);
                    if (intProject > 0)
                    {
                        DataSet dsProject = oProject.Get(intProject);
                        if (dsProject.Tables[0].Rows.Count > 0)
                        {
                            oTable.AddCell(new Cell(new Phrase("Project Name:", oFontBold)));
                            oTable.AddCell(new Cell(new Phrase(dsProject.Tables[0].Rows[0]["name"].ToString(), oFont)));
                            oTable.AddCell(new Cell(new Phrase("Initiative Type:", oFontBold)));
                            oTable.AddCell(new Cell(new Phrase(dsProject.Tables[0].Rows[0]["bd"].ToString(), oFont)));
                            oTable.AddCell(new Cell(new Phrase("Organization:", oFontBold)));
                            oTable.AddCell(new Cell(new Phrase(oOrganization.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["organization"].ToString())), oFont)));
                            oTable.AddCell(new Cell(new Phrase("Project Number:", oFontBold)));
                            oTable.AddCell(new Cell(new Phrase(dsProject.Tables[0].Rows[0]["number"].ToString(), oFont)));
                            oTable.AddCell(new Cell(new Phrase("Project Status:", oFontBold)));
                            oTable.AddCell(new Cell(new Phrase(oStatusLevel.Name(Int32.Parse(dsProject.Tables[0].Rows[0]["status"].ToString())), oFont)));
                            sb.Append("<tr><td nowrap>Project Name:</td><td width=\"100%\">");
                            sb.Append(dsProject.Tables[0].Rows[0]["name"].ToString());
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap>Initiative Type:</td><td width=\"100%\">");
                            sb.Append(dsProject.Tables[0].Rows[0]["bd"].ToString());
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap>Organization:</td><td width=\"100%\">");
                            sb.Append(oOrganization.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["organization"].ToString())));
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap>Project Number:</td><td width=\"100%\">");
                            sb.Append(dsProject.Tables[0].Rows[0]["number"].ToString());
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap>Project Status:</td><td width=\"100%\">");
                            sb.Append(oStatusLevel.HTML(Int32.Parse(dsProject.Tables[0].Rows[0]["status"].ToString())));
                            sb.Append("</td></tr>");
                            intManager = Int32.Parse(dsProject.Tables[0].Rows[0]["lead"].ToString());
                            intEngineer = Int32.Parse(dsProject.Tables[0].Rows[0]["engineer"].ToString());
                            intLead = Int32.Parse(dsProject.Tables[0].Rows[0]["technical"].ToString());
                        }
                    }
                    else
                    {
                        DataSet dsPending = oProjectPending.GetRequest(intRequest);
                        if (dsPending.Tables[0].Rows.Count > 0)
                        {
                            oTable.AddCell(new Cell(new Phrase("Project Name:", oFontBold)));
                            oTable.AddCell(new Cell(new Phrase(dsPending.Tables[0].Rows[0]["name"].ToString(), oFont)));
                            oTable.AddCell(new Cell(new Phrase("Initiative Type:", oFontBold)));
                            oTable.AddCell(new Cell(new Phrase(dsPending.Tables[0].Rows[0]["bd"].ToString(), oFont)));
                            oTable.AddCell(new Cell(new Phrase("Organization:", oFontBold)));
                            oTable.AddCell(new Cell(new Phrase(oOrganization.GetName(Int32.Parse(dsPending.Tables[0].Rows[0]["organization"].ToString())), oFont)));
                            oTable.AddCell(new Cell(new Phrase("Project Number:", oFontBold)));
                            oTable.AddCell(new Cell(new Phrase(dsPending.Tables[0].Rows[0]["number"].ToString(), oFont)));
                            oTable.AddCell(new Cell(new Phrase("Project Status:", oFontBold)));
                            oTable.AddCell(new Cell(new Phrase("PENDING", oFont)));
                            sb.Append("<tr><td nowrap>Project Name:</td><td width=\"100%\">");
                            sb.Append(dsPending.Tables[0].Rows[0]["name"].ToString());
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap>Initiative Type:</td><td width=\"100%\">");
                            sb.Append(dsPending.Tables[0].Rows[0]["bd"].ToString());
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap>Organization:</td><td width=\"100%\">");
                            sb.Append(oOrganization.GetName(Int32.Parse(dsPending.Tables[0].Rows[0]["organization"].ToString())));
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap>Project Number:</td><td width=\"100%\">");
                            sb.Append(dsPending.Tables[0].Rows[0]["number"].ToString());
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td nowrap>Project Status:</td><td width=\"100%\" class=\"pending\">PENDING</td></tr>");
                            intManager = Int32.Parse(dsPending.Tables[0].Rows[0]["lead"].ToString());
                            intEngineer = Int32.Parse(dsPending.Tables[0].Rows[0]["engineer"].ToString());
                            intLead = Int32.Parse(dsPending.Tables[0].Rows[0]["technical"].ToString());
                        }
                    }
                    if (intManager > 0)
                    {
                        sb.Append("<tr><td nowrap>Project Manager:</td><td width=\"100%\">");
                        sb.Append(oUser.GetFullName(intManager));
                        sb.Append("</td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr><td nowrap>Project Manager:</td><td width=\"100%\"></td></tr>");
                    }
                    if (intEngineer > 0)
                    {
                        sb.Append("<tr><td nowrap>Integration Engineer:</td><td width=\"100%\">");
                        sb.Append(oUser.GetFullName(intEngineer));
                        sb.Append("</td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr><td nowrap>Integration Engineer:</td><td width=\"100%\"></td></tr>");
                    }
                    if (intLead > 0)
                    {
                        sb.Append("<tr><td nowrap>Technical Lead:</td><td width=\"100%\">");
                        sb.Append(oUser.GetFullName(intLead));
                        sb.Append("</td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr><td nowrap>Technical Lead:</td><td width=\"100%\"></td></tr>");
                    }

                    oTable.AddCell(new Cell(new Phrase("Project Manager:", oFontBold)));
                    oTable.AddCell(new Cell(new Phrase(oUser.GetFullName(intManager), oFont)));
                    oTable.AddCell(new Cell(new Phrase("Integration Engineer:", oFontBold)));
                    oTable.AddCell(new Cell(new Phrase(oUser.GetFullName(intEngineer), oFont)));
                    oTable.AddCell(new Cell(new Phrase("Technical Lead:", oFontBold)));
                    oTable.AddCell(new Cell(new Phrase(oUser.GetFullName(intLead), oFont)));
                    oTable.AddCell(new Cell(new Phrase(" ", oFontBold)));
                    oTable.AddCell(new Cell(new Phrase(" ", oFont)));
                    Cell cell2 = new Cell(new Phrase("Line Items", oFontHeader));
                    cell2.Colspan = 2;
                    cell2.BackgroundColor = new iTextSharp.text.Color(204, 204, 204);
                    oTable.AddCell(cell2);
                    sb.Append("<tr><td colspan=\"2\"><hr size=\"1\" noshade /></td></tr>");
                    sb.Append("<tr><td colspan=\"2\" class=\"header\">Line Items</td></tr>");
                    sb.Append("<tr><td colspan=\"2\">");
                    ds = oForecast.GetAnswers(intForecast);
                    bool boolChange = false;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int intID = Int32.Parse(dr["id"].ToString());
                        int intPlatform = Int32.Parse(dr["platformid"].ToString());
                        int intClass = Int32.Parse(dr["classid"].ToString());
                        int intEnvir = Int32.Parse(dr["environmentid"].ToString());
                        double dblQuantity = double.Parse(dr["quantity"].ToString()) + double.Parse(dr["recovery_number"].ToString());
                        int intModel = 0;
                        int intServerModel = oForecast.GetModelAsset(intID);
                        if (intServerModel == 0)
                            intServerModel = oForecast.GetModel(intID);
                        if (intServerModel == 0)
                        {
                            // Get the model selected in the equipment dropdown (if not server)
                            intModel = Int32.Parse(dr["modelid"].ToString());
                        }
                        double dblAmp = 0.00;
                        double dblReplicate = 0.00;
                        string strModel = "";
                        if (intServerModel > 0)
                        {
                            dblAmp = (double.Parse(oModelsProperties.Get(intServerModel, "amp")) * dblQuantity);
                            double.TryParse(oModelsProperties.Get(intServerModel, "replicate_times"), out dblReplicate);
                            if (intModel == 0)
                                intModel = Int32.Parse(oModelsProperties.Get(intServerModel, "modelid"));
                            strModel = oModelsProperties.Get(intServerModel, "name");
                        }
                        else if (intModel > 0)
                            strModel = oModel.Get(intModel, "name");
                        else
                        {
                            DataSet dsVendor = oForecast.GetAnswer(intID);
                            if (dsVendor.Tables[0].Rows.Count > 0 && dsVendor.Tables[0].Rows[0]["modelname"].ToString() != "")
                            {
                                dblAmp = (double.Parse(dsVendor.Tables[0].Rows[0]["amp"].ToString()) * dblQuantity);
                                strModel = dsVendor.Tables[0].Rows[0]["modelname"].ToString();
                            }
                        }
                        if (intModel == 0)
                            strModel = "Solution Unavailable";
                        // STORAGE
                        DataSet dsStorage = oForecast.GetStorage(intID);
                        double dblStorage = 0.00;
                        if (dsStorage.Tables[0].Rows.Count > 0)
                        {
                            double dblHigh = double.Parse(dsStorage.Tables[0].Rows[0]["high_total"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["high_qa"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["high_test"].ToString()) + (double.Parse(dsStorage.Tables[0].Rows[0]["high_replicated"].ToString()) * dblReplicate) + double.Parse(dsStorage.Tables[0].Rows[0]["high_ha"].ToString());
                            double dblStandard = double.Parse(dsStorage.Tables[0].Rows[0]["standard_total"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["standard_qa"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["standard_test"].ToString()) + (double.Parse(dsStorage.Tables[0].Rows[0]["standard_replicated"].ToString()) * dblReplicate) + double.Parse(dsStorage.Tables[0].Rows[0]["standard_ha"].ToString());
                            double dblLow = double.Parse(dsStorage.Tables[0].Rows[0]["low_total"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["low_qa"].ToString()) + double.Parse(dsStorage.Tables[0].Rows[0]["low_test"].ToString()) + (double.Parse(dsStorage.Tables[0].Rows[0]["low_replicated"].ToString()) * dblReplicate) + double.Parse(dsStorage.Tables[0].Rows[0]["low_ha"].ToString());
                            dblStorage = dblHigh + dblStandard + dblLow;
                        }
                        sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"background-color:");
                        sb.Append(boolChange ? "#FFFFFF" : "#F6F6F6");
                        sb.Append("\">");
                        sb.Append("<tr><td>");
                        sb.Append("<table width=\"100%\" cellpadding=\"5\" cellspacing=\"2\" border=\"0\">");
                        sb.Append("<tr><td valign=\"top\" nowrap>Nickname:</td><td>");
                        sb.Append(dr["name"].ToString());
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td valign=\"top\" nowrap>Platform:</td><td>");
                        sb.Append(oPlatform.GetName(intPlatform));
                        sb.Append("</td></tr>");
                        oTable.AddCell(new Cell(new Phrase("Nickname:", oFontBold)));
                        oTable.AddCell(new Cell(new Phrase(dr["name"].ToString(), oFont)));
                        oTable.AddCell(new Cell(new Phrase("Platform:", oFontBold)));
                        oTable.AddCell(new Cell(new Phrase(oPlatform.GetName(intPlatform), oFont)));
                        oTable.AddCell(new Cell(new Phrase("Model:", oFontBold)));
                        oTable.AddCell(new Cell(new Phrase(strModel, oFont)));
                        oTable.AddCell(new Cell(new Phrase("Commitment Date:", oFontBold)));
                        oTable.AddCell(new Cell(new Phrase((dr["implementation"].ToString() == "" ? "" : DateTime.Parse(dr["implementation"].ToString()).ToShortDateString()), oFont)));
                        oTable.AddCell(new Cell(new Phrase("Quantity:", oFontBold)));
                        oTable.AddCell(new Cell(new Phrase(dblQuantity.ToString(), oFont)));
                        string strPDF = oModel.Get(intModel, "pdf");
                        sb.Append("<tr><td valign=\"top\" nowrap>Model:</td><td width=\"100%\">");
                        sb.Append(strPDF == "" ? strModel : "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMAX('" + strPDF.Replace("\\", "\\\\") + "');\">" + strModel + "</a>");
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td valign=\"top\" nowrap>Commitment Date:</td><td width=\"100%\">");
                        sb.Append(dr["implementation"].ToString() == "" ? "" : DateTime.Parse(dr["implementation"].ToString()).ToShortDateString());
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td valign=\"top\" nowrap>Quantity:</td><td width=\"100%\"><a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/frame/forecast/forecast_print_quantity.aspx?id=");
                        sb.Append(intID.ToString());
                        sb.Append("',275,200);\">");
                        sb.Append(dblQuantity.ToString());
                        sb.Append("</a></td></tr>");
                        double dblA = 0.00;
                        DataSet dsA = oForecast.GetAcquisitions(intModel, 1);
                        foreach (DataRow drA in dsA.Tables[0].Rows)
                            dblA += double.Parse(drA["cost"].ToString());
                        sb.Append("<tr><td valign=\"top\" nowrap>Acquisition Costs:</td><td width=\"100%\"><a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/frame/forecast/forecast_print_acquisition.aspx?id=");
                        sb.Append(intID.ToString());
                        sb.Append("',400,300);\">$");
                        sb.Append(dblA.ToString("N"));
                        sb.Append("</a></td></tr>");
                        oTable.AddCell(new Cell(new Phrase("Acquisition Costs:", oFontBold)));
                        oTable.AddCell(new Cell(new Phrase(dblA.ToString("N"), oFont)));
                        double dblO = 0.00;
                        DataSet dsO = oForecast.GetOperations(intModel, 1);
                        foreach (DataRow drO in dsO.Tables[0].Rows)
                            dblO += double.Parse(drO["cost"].ToString());
                        oTable.AddCell(new Cell(new Phrase("Operational Costs:", oFontBold)));
                        oTable.AddCell(new Cell(new Phrase(dblO.ToString("N"), oFont)));
                        oTable.AddCell(new Cell(new Phrase("Storage:", oFontBold)));
                        oTable.AddCell(new Cell(new Phrase(dblStorage.ToString("N") + " GB", oFont)));
                        oTable.AddCell(new Cell(new Phrase("AMPs:", oFontBold)));
                        oTable.AddCell(new Cell(new Phrase(dblAmp.ToString("N") + " AMPs", oFont)));
                        sb.Append("<tr><td valign=\"top\" nowrap>Operational Costs:</td><td width=\"100%\"><a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/frame/forecast/forecast_print_operational.aspx?id=");
                        sb.Append(intID.ToString());
                        sb.Append("',400,300);\">$");
                        sb.Append(dblO.ToString("N"));
                        sb.Append("</a></td></tr>");
                        sb.Append("<tr><td valign=\"top\" nowrap>Storage:</td><td width=\"100%\"><a href=\"javascript:void(0);\" onclick=\"OpenNewWindow('/frame/forecast/forecast_print_storage.aspx?id=");
                        sb.Append(intID.ToString());
                        sb.Append("',650,200);\">");
                        sb.Append(dblStorage.ToString("N"));
                        sb.Append(" GB</a></td></tr>");
                        sb.Append("<tr><td valign=\"top\" nowrap>AMPs:</td><td width=\"100%\">");
                        sb.Append(dblAmp.ToString("N"));
                        sb.Append("</td></tr>");
                        if (Request.QueryString["checked"] != null)
                        {
                            sb.Append("<tr><td valign=\"top\" colspan=\"2\"><b>Questions & Responses</b></td></tr>");
                            DataSet dsQuestions = oForecast.GetQuestionPlatform(intPlatform, intClass, intEnvir);
                            foreach (DataRow drQuestion in dsQuestions.Tables[0].Rows)
                            {
                                string strResponse = "";
                                string strResponsePDF = "";
                                int intQuestion = Int32.Parse(drQuestion["id"].ToString());
                                DataSet dsAnswers = oForecast.GetAnswerPlatform(intID, intQuestion);
                                foreach (DataRow drAnswer in dsAnswers.Tables[0].Rows)
                                {
                                    strResponse += "<tr><td valign=\"top\"></td><td> " + oForecast.GetResponse(Int32.Parse(drAnswer["responseid"].ToString()), "response") + "</td></tr>";
                                    if (strResponsePDF != "")
                                        strResponsePDF += ", ";
                                    strResponsePDF += oForecast.GetResponse(Int32.Parse(drAnswer["responseid"].ToString()), "response");
                                }
                                if (strResponse != "")
                                {
                                    sb.Append("<tr><td valign=\"top\" colspan=\"2\"><table cellpadding=\"1\" cellspacing=\"1\" border=\"0\">");
                                    sb.Append("<tr><td valign=\"top\"><img src=\"/images/help.gif\" align=\"absmiddle\" border=\"0\"/></td><td>");
                                    sb.Append(drQuestion["question"].ToString());
                                    sb.Append("</td></tr>");
                                    Cell oCellQ = new Cell(new Phrase(drQuestion["question"].ToString(), oFontBold));
                                    oCellQ.Colspan = 2;
                                    oTable.AddCell(oCellQ);
                                    Cell oCellA = new Cell(new Phrase(strResponsePDF, oFont));
                                    oCellA.Colspan = 2;
                                    oTable.AddCell(oCellA);
                                    sb.Append(strResponse);
                                    sb.Append("</table></td></tr>");
                                }
                            }
                        }
                        sb.Append("</table></td></tr>");
                        Cell oCellD = new Cell(new Phrase("", oFont));
                        oCellD.Colspan = 2;
                        oCellD.BackgroundColor = new iTextSharp.text.Color(100, 100, 100);
                        oTable.AddCell(oCellD);
                        sb.Append("<tr height=\"1\"><td colspan=\"2\" style=\"border-bottom:dashed 1px #CCCCCC\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"1\" height=\"1\" /></td></tr>");
                        sb.Append("</table>");
                        boolChange = !boolChange;
                    }
                    sb.Append("</td></tr>");
                    chkQuestions.Checked = (Request.QueryString["checked"] != null);
                    chkQuestions.Attributes.Add("onclick", "WaitDDL('" + divWait.ClientID + "');");
                    if (Request.QueryString["export"] != null)
                    {
                        doc.Add(oTable);
                        doc.Close();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Disposition", "attachment; filename=export.pdf");
                        Response.End();
                        Response.Flush();
                    }
                }

                strQuestions = sb.ToString();
            }
        }
        protected void chkQuestions_Change(Object Sender, EventArgs e)
        {
            if (Request.QueryString["checked"] == null)
                Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&checked=true");
            else
                Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"]);
        }
        protected void btnExport_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Url.PathAndQuery + "&export=true");
        }
    }
}
