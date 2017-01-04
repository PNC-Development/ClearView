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
using iTextSharp.text;
using NCC.ClearView.Application.Core;
using System.IO;
using iTextSharp.text.pdf;

namespace NCC.ClearView.Presentation.Web
{
    public partial class pdf_birth : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);

        protected string strEmail = ConfigurationManager.AppSettings["TSM_MAILBOX"];
        protected int intUnder48A = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_RESPONSE"]);
        protected int intUnder48Q = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_QUESTION"]);
        protected bool boolUsePNCNaming = (ConfigurationManager.AppSettings["USE_PNC_NAMING"] == "1");
        private Variables oVariables;

        private string strEMailIdsBCC = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            bool boolSave = false;
            if (Request.QueryString["save"] != null)
                boolSave = true;
            bool boolAdd = false;
            if (Request.QueryString["add"] != null)
                boolAdd = true;
            bool boolSend = false;
            if (Request.QueryString["send"] != null)
                boolSend = true;
            bool boolProd = false;
            if (Request.QueryString["prod"] != null)
                boolProd = true;
            bool boolDebug = false;
            if (Request.QueryString["debug"] != null)
                boolDebug = true;
            if (Request.QueryString["id"] != null)
            {
                int intAnswer = Int32.Parse(Request.QueryString["id"]);
                if (boolDebug == true)
                {
                    // Use local function
                    iTextSharp.text.Document oDocument = CreateDocuments(intAnswer, false, false, Response.OutputStream, boolSave, boolAdd, boolSend, boolProd, boolUsePNCNaming);
                }
                else
                {
                    // Use class function
                    PDFs oPDF = new PDFs(dsn, dsnAsset, dsnIP, intEnvironment);
                    iTextSharp.text.Document oDocument = oPDF.CreateDocuments(intAnswer, false, false, Response.OutputStream, boolSave, boolAdd, boolSend, boolProd, boolUsePNCNaming, false);
                }
                if (boolSave == false)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=export.pdf");
                    Response.End();
                    Response.Flush();
                }
            }
        }
        public Document CreateDocuments(int _answerid, bool _san, bool _tsm, Stream _stream, bool _save, bool _add, bool _send, bool _prod, bool _use_pnc_naming)
        {
            oVariables = new Variables(intEnvironment);
            Documents oDocument = new Documents(0, dsn);
            int intProject = 0;
            string strFile = "";
            string strFileName = "";
            string strName = "";
            Servers oServer = new Servers(0, dsn);
            Asset oAsset = new Asset(0, dsnAsset);
            Forecast oForecast = new Forecast(0, dsn);
            Environments oEnvironment = new Environments(0, dsn);
            OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);
            Classes oClass = new Classes(0, dsn);
            Requests oRequest = new Requests(0, dsn);
            Projects oProject = new Projects(0, dsn);
            ServerName oServerName = new ServerName(0, dsn);
            Models oModel = new Models(0, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            Storage oStorage = new Storage(0, dsn);
            Users oUser = new Users(0, dsn);
            Organizations oOrganization = new Organizations(0, dsn);
            IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
            ConsistencyGroups oConsistencyGroups = new ConsistencyGroups(0, dsn);
            Variables oVariable = new Variables(intEnvironment);
            Locations oLocation = new Locations(0, dsn);
            OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
            Mnemonic oMnemonic = new Mnemonic(0, dsn);
            bool boolOverride = (oForecast.GetAnswer(_answerid, "storage_override") == "1");
            DataSet ds = oServer.GetAnswer(_answerid);
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_ALERT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intForecast = 0;
                int intRequest = 0;
                if (oForecast.GetAnswer(_answerid, "forecastid") != "" && oForecast.GetAnswer(_answerid, "forecastid") != "0")
                {
                    intForecast = Int32.Parse(oForecast.GetAnswer(_answerid, "forecastid"));
                    intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                    intProject = oRequest.GetProjectNumber(intRequest);
                }
                int intClass = Int32.Parse(oForecast.GetAnswer(_answerid, "classid"));
                bool boolPNC = (ds.Tables[0].Rows[0]["pnc"].ToString() == "1");
                int intEnv = Int32.Parse(oForecast.GetAnswer(_answerid, "environmentid"));
                int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString()); ;
                int intLead = 0;
                bool boolTest = false;
                bool boolQA = false;
                bool boolProd = false;
                bool boolUnder = false;
                if (oClass.Get(intClass, "pnc") == "1")
                {
                    if (oClass.IsProd(intClass))
                    {
                        boolProd = true;
                        if (oForecast.GetAnswerPlatform(_answerid, intUnder48Q, intUnder48A) == true)
                            boolUnder = true;
                    }
                    else if (oClass.IsQA(intClass))
                        boolQA = true;
                    else
                        boolTest = true;
                }
                else
                {
                    if (oClass.IsProd(intClass))
                    {
                        if (_prod == false)
                        {
                            if (oForecast.GetAnswer(_answerid, "test") == "1")
                                boolTest = true;
                        }
                        else
                            boolProd = true;
                        if (oForecast.GetAnswerPlatform(_answerid, intUnder48Q, intUnder48A) == true)
                            boolUnder = true;
                    }
                    else if (oClass.IsQA(intClass))
                    {
                        if (_prod == false)
                            boolQA = true;
                    }
                    else
                    {
                        if (_prod == false)
                            boolTest = true;
                    }
                }
                string strClass = "Test";
                if (boolProd == true)
                    strClass = "Production";
                else if (boolQA == true)
                    strClass = "QA";

                int intImplementor = -999;
                DataSet dsPending = oOnDemandTasks.GetPending(_answerid);
                if (dsPending.Tables[0].Rows.Count > 0)
                {
                    intImplementor = Int32.Parse(dsPending.Tables[0].Rows[0]["resourceid"].ToString());
                    intImplementor = Int32.Parse(oResourceRequest.GetWorkflow(intImplementor, "userid"));
                }


                Document doc = new Document();
                iTextSharp.text.Font oFontHeader = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10, 1);
                iTextSharp.text.Font oFontBold = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 1);
                iTextSharp.text.Font oFontBoldRed = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 1, iTextSharp.text.Color.RED);
                iTextSharp.text.Font oFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 0);
                iTextSharp.text.Font oFontRed = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 0, iTextSharp.text.Color.RED);
                iTextSharp.text.Font oFontSpace = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, 0, iTextSharp.text.Color.WHITE);
                if (_save == true)
                {
                    string _save_location = oVariables.UploadsFolder() + "BIRTH\\";
                    if (_san == true)
                        _save_location = oVariables.UploadsFolder() + "SAN\\";
                    if (_tsm == true)
                        _save_location = oVariables.UploadsFolder() + "TSM\\";
                    strFileName = "BIRTH_" + _answerid.ToString() + "_" + oProject.Get(intProject, "number") + ".PDF";
                    if (_san == true)
                        strFileName = "SAN_" + _answerid.ToString() + "_" + oProject.Get(intProject, "number") + ".PDF";
                    if (_tsm == true)
                        strFileName = "TSM_" + _answerid.ToString() + "_" + oProject.Get(intProject, "number") + ".PDF";
                    if (Directory.Exists(_save_location) == false)
                        Directory.CreateDirectory(_save_location);
                    strFile = _save_location + strFileName;
                    PdfWriter.GetInstance(doc, new FileStream(strFile, FileMode.Create));
                }
                else
                    PdfWriter.GetInstance(doc, _stream);
                //string strHeader = "ClearView - Birth Certificate";
                string strHeader = "";
                if (_san == true)
                    strHeader = "ClearView - Server Build Request Form (Storage)";
                if (_tsm == true)
                    strHeader = "ClearView - Server Build Request Form (Backup)";
                if (String.IsNullOrEmpty(strHeader) == false)
                {
                    HeaderFooter header = new HeaderFooter(new Phrase(strHeader, new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 6)), false);
                    header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    header.Alignment = 2;
                    doc.Header = header;
                }
                else
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(@"\\wcclv309a\documents$\logo.gif");
                    logo.ScalePercent(50);
                    HeaderFooter header = new HeaderFooter(new Phrase(new Chunk(logo, 0, 0)), false);
                    header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    header.Alignment = 2;
                    doc.Header = header;
                }
                string strFooter = "NOTE: Birth Certificate information is generated at point in time. This information was accurate as of " + DateTime.Now.ToLongDateString() + " at " + DateTime.Now.ToShortTimeString();
                HeaderFooter footer = new HeaderFooter(new Phrase(strFooter, new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 6)), false);
                footer.Border = iTextSharp.text.Rectangle.NO_BORDER;
                footer.Alignment = 2;
                doc.Footer = footer;
                doc.Open();

                iTextSharp.text.Table oTableTop = new iTextSharp.text.Table(1);
                oTableTop.BorderWidth = 0;
                oTableTop.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                oTableTop.Padding = 2;
                oTableTop.Width = 100;

                // Project Information
                if (intForecast > 0 && intProject > 0)
                {
                    Cell oCellProject = new Cell(new Phrase("Project Information", oFontBold));
                    oCellProject.BackgroundColor = new iTextSharp.text.Color(204, 204, 204);
                    oTableTop.AddCell(oCellProject);
                    iTextSharp.text.Table oTableProject = new iTextSharp.text.Table(2);
                    oTableProject.BorderWidth = 0;
                    oTableProject.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                    oTableProject.Padding = 2;
                    oTableProject.Width = 100;
                    oTableProject.AddCell(new Cell(new Phrase("Project Name:", oFontBold)));
                    oTableProject.AddCell(new Cell(new Phrase(oProject.Get(intProject, "name"), oFont)));
                    oTableProject.AddCell(new Cell(new Phrase("Project Number:", oFontBold)));
                    oTableProject.AddCell(new Cell(new Phrase(oProject.Get(intProject, "number"), oFont)));
                    oTableProject.AddCell(new Cell(new Phrase("Project Type:", oFontBold)));
                    oTableProject.AddCell(new Cell(new Phrase(oProject.Get(intProject, "bd"), oFont)));
                    oTableProject.AddCell(new Cell(new Phrase("Portfolio:", oFontBold)));
                    oTableProject.AddCell(new Cell(new Phrase(oOrganization.GetName(Int32.Parse(oProject.Get(intProject, "organization"))), oFont)));
                    string strLead = oProject.Get(intProject, "lead");
                    oTableProject.AddCell(new Cell(new Phrase("Project Manager:", oFontBold)));
                    if (strLead != "")
                    {
                        intLead = Int32.Parse(strLead);
                        oTableProject.AddCell(new Cell(new Phrase(oUser.GetFullName(intLead), oFont)));
                    }
                    else
                        oTableProject.AddCell(new Cell(new Phrase("N / A", oFont)));
                    string strRequester = oForecast.Get(intForecast, "userid");
                    oTableProject.AddCell(new Cell(new Phrase("Requester:", oFontBold)));
                    if (strRequester != "")
                        oTableProject.AddCell(new Cell(new Phrase(oUser.GetFullName(Int32.Parse(strRequester)), oFont)));
                    else
                        oTableProject.AddCell(new Cell(new Phrase("N / A", oFont)));
                    string strEngineer = oProject.Get(intProject, "engineer");
                    //oTableProject.AddCell(new Cell(new Phrase("Integration Engineer:", oFontBold)));
                    //if (strEngineer != "")
                    //    oTableProject.AddCell(new Cell(new Phrase(oUser.GetFullName(Int32.Parse(strEngineer)), oFont)));
                    //else
                    //    oTableProject.AddCell(new Cell(new Phrase("N / A", oFont)));
                    //string strTechnical = oProject.Get(intProject, "technical");
                    //oTableProject.AddCell(new Cell(new Phrase("Technical Lead:", oFontBold)));
                    //if (strTechnical != "")
                    //    oTableProject.AddCell(new Cell(new Phrase(oUser.GetFullName(Int32.Parse(strTechnical)), oFont)));
                    //else
                    //    oTableProject.AddCell(new Cell(new Phrase("N / A", oFont)));
                    //oTableProject.AddCell(new Cell(new Phrase("II Resource:", oFontBold)));
                    //if (intImplementor > 0 || intImplementor == -999)
                    //    oTableProject.AddCell(new Cell(new Phrase(oUser.GetFullName(intImplementor), oFont)));
                    //else
                    //    oTableProject.AddCell(new Cell(new Phrase("N / A", oFont)));
                    oTableTop.InsertTable(oTableProject);
                }

                if (_san == false && _tsm == false)
                {
                    // Design Information
                    Cell oCellSpace1 = new Cell(new Phrase("spacer", oFontSpace));
                    oCellSpace1.BackgroundColor = new iTextSharp.text.Color(255, 255, 255);
                    oTableTop.AddCell(oCellSpace1);
                    Cell oCellDesign = new Cell(new Phrase("Design Configuration", oFontBold));
                    oCellDesign.BackgroundColor = new iTextSharp.text.Color(204, 204, 204);
                    oTableTop.AddCell(oCellDesign);
                    iTextSharp.text.Table oTableDesign = new iTextSharp.text.Table(2);
                    oTableDesign.BorderWidth = 0;
                    oTableDesign.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                    oTableDesign.Padding = 2;
                    oTableDesign.Width = 100;
                    double dblQuantity = double.Parse(oForecast.GetAnswer(_answerid, "quantity")) + double.Parse(oForecast.GetAnswer(_answerid, "recovery_number"));
                    //oTableDesign.AddCell(new Cell(new Phrase("Quantity:", oFontBold)));
                    //oTableDesign.AddCell(new Cell(new Phrase(dblQuantity.ToString("F"), oFont)));
                    oTableDesign.AddCell(new Cell(new Phrase("Commitment Date:", oFontBold)));
                    oTableDesign.AddCell(new Cell(new Phrase((oForecast.GetAnswer(_answerid, "implementation") == "" ? "" : DateTime.Parse(oForecast.GetAnswer(_answerid, "implementation")).ToShortDateString()), oFont)));
                    oTableDesign.AddCell(new Cell(new Phrase("Completion / Installation Date:", oFontBold)));
                    oTableDesign.AddCell(new Cell(new Phrase((oForecast.GetAnswer(_answerid, "completed") == "" ? "" : DateTime.Parse(oForecast.GetAnswer(_answerid, "completed")).ToShortDateString()), oFont)));
                    double dblA = 0.00;
                    DataSet dsA = oForecast.GetAcquisitions(intModel, 1);
                    foreach (DataRow drA in dsA.Tables[0].Rows)
                        dblA += double.Parse(drA["cost"].ToString());
                    oTableDesign.AddCell(new Cell(new Phrase("Acquisition Costs:", oFontBold)));
                    oTableDesign.AddCell(new Cell(new Phrase(dblA.ToString("N"), oFont)));
                    double dblO = 0.00;
                    DataSet dsO = oForecast.GetOperations(intModel, 1);
                    foreach (DataRow drO in dsO.Tables[0].Rows)
                        dblO += double.Parse(drO["cost"].ToString());
                    oTableDesign.AddCell(new Cell(new Phrase("Operational Costs:", oFontBold)));
                    oTableDesign.AddCell(new Cell(new Phrase(dblO.ToString("N"), oFont)));
                    double dblAmp = (double.Parse(oModelsProperties.Get(intModel, "amp")) * dblQuantity);
                    oTableDesign.AddCell(new Cell(new Phrase("AMPs:", oFontBold)));
                    oTableDesign.AddCell(new Cell(new Phrase(dblAmp.ToString("N") + " AMPs", oFont)));
                    oTableDesign.AddCell(new Cell(new Phrase("Application Name:", oFontBold)));
                    oTableDesign.AddCell(new Cell(new Phrase(oForecast.GetAnswer(_answerid, "appname"), oFont)));
                    if (boolPNC == true)
                    {
                        oTableDesign.AddCell(new Cell(new Phrase("Mnemonic:", oFontBold)));
                        int intMnemonic = Int32.Parse(oForecast.GetAnswer(_answerid, "mnemonicid"));
                        oTableDesign.AddCell(new Cell(new Phrase(oMnemonic.Get(intMnemonic, "factory_code"), oFont)));
                    }
                    else
                    {
                        oTableDesign.AddCell(new Cell(new Phrase("Application Code:", oFontBold)));
                        oTableDesign.AddCell(new Cell(new Phrase(oForecast.GetAnswer(_answerid, "appcode"), oFont)));
                    }
                    string strContact1 = oForecast.GetAnswer(_answerid, "appcontact");
                    if (strContact1 != "")
                    {
                        oTableDesign.AddCell(new Cell(new Phrase("Departmental Manager:", oFontBold)));
                        oTableDesign.AddCell(new Cell(new Phrase(oUser.GetFullName(Int32.Parse(strContact1)) + " (" + oUser.GetName(Int32.Parse(strContact1)) + ")", oFont)));
                    }
                    string strContact2 = oForecast.GetAnswer(_answerid, "admin1");
                    if (strContact2 != "")
                    {
                        oTableDesign.AddCell(new Cell(new Phrase("Application Technical Lead:", oFontBold)));
                        oTableDesign.AddCell(new Cell(new Phrase(oUser.GetFullName(Int32.Parse(strContact2)) + " (" + oUser.GetName(Int32.Parse(strContact2)) + ")", oFont)));
                    }
                    string strContact3 = oForecast.GetAnswer(_answerid, "admin2");
                    if (strContact3 != "")
                    {
                        oTableDesign.AddCell(new Cell(new Phrase("Administrative Contact:", oFontBold)));
                        oTableDesign.AddCell(new Cell(new Phrase(oUser.GetFullName(Int32.Parse(strContact3)) + " (" + oUser.GetName(Int32.Parse(strContact3)) + ")", oFont)));
                    }

                    int intPlatform = Int32.Parse(oForecast.GetAnswer(_answerid, "platformid"));
                    DataSet dsQuestions = oForecast.GetQuestionPlatform(intPlatform, intClass, intEnv);
                    foreach (DataRow drQuestion in dsQuestions.Tables[0].Rows)
                    {
                        string strResponsePDF = "";
                        int intQuestion = Int32.Parse(drQuestion["id"].ToString());
                        DataSet dsAnswers = oForecast.GetAnswerPlatform(_answerid, intQuestion);
                        foreach (DataRow drAnswer in dsAnswers.Tables[0].Rows)
                        {
                            if (strResponsePDF != "")
                                strResponsePDF += ", ";
                            strResponsePDF += oForecast.GetResponse(Int32.Parse(drAnswer["responseid"].ToString()), "response");
                        }
                        if (strResponsePDF != "")
                        {
                            Cell oCellQ = new Cell(new Phrase(drQuestion["question"].ToString(), oFontBold));
                            oCellQ.Colspan = 2;
                            oTableDesign.AddCell(oCellQ);
                            Cell oCellA = new Cell(new Phrase(strResponsePDF, oFont));
                            oCellA.Colspan = 2;
                            oTableDesign.AddCell(oCellA);
                        }
                    }
                    oTableTop.InsertTable(oTableDesign);
                }

                int intCount = 0;
                bool boolMidrange = false;
                string strEmail = "";
                string strShared = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    oTableTop.AddCell(new Cell(new Phrase("spacer", oFontSpace)));
                    intCount++;
                    int intServer = Int32.Parse(dr["id"].ToString());
                    int intOS = Int32.Parse(dr["osid"].ToString());
                    _answerid = Int32.Parse(dr["answerid"].ToString());
                    int intUser = Int32.Parse(oForecast.GetAnswer(_answerid, "userid"));
                    if (intUser > 0)
                        strEmail = oUser.GetName(intUser) + ";";
                    int intCSM = Int32.Parse(dr["csmconfigid"].ToString());
                    int intCluster = Int32.Parse(dr["clusterid"].ToString());
                    int intNumber = Int32.Parse(dr["number"].ToString());
                    iTextSharp.text.Table oTableInfo = new iTextSharp.text.Table(2);
                    oTableInfo.BorderWidth = 0;
                    oTableInfo.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                    oTableInfo.Padding = 2;
                    oTableInfo.Width = 100;
                    int intName = Int32.Parse(dr["nameid"].ToString());
                    strName = oServer.GetName(intServer, _use_pnc_naming);
                    if (strShared != "")
                        strShared += ", ";
                    strShared += strName;
                    oTableInfo.AddCell(new Cell(new Phrase("Design Nickname:", oFontBold)));
                    oTableInfo.AddCell(new Cell(new Phrase(oForecast.GetAnswer(_answerid, "name"), oFont)));
                    oTableInfo.AddCell(new Cell(new Phrase("Change Control #:", oFontBold)));
                    if (oForecast.GetAnswer(_answerid, "change") == "")
                        oTableInfo.AddCell(new Cell(new Phrase("N / A", oFont)));
                    else
                        oTableInfo.AddCell(new Cell(new Phrase(oForecast.GetAnswer(_answerid, "change"), oFont)));
                    DataSet dsGeneric = oServer.GetGeneric(intServer);
                    string strVIO = "";
                    string strVIODR = "";
                    if (oModelsProperties.IsVIO(intModel) == true)
                    {
                        oTableInfo.AddCell(new Cell(new Phrase("Server Name(s):", oFontBold)));
                        if (boolTest == true || boolQA == true)
                            strVIO = dsGeneric.Tables[0].Rows[0]["vio1"].ToString() + ", " + dsGeneric.Tables[0].Rows[0]["vio2"].ToString();
                        if (boolProd == true)
                            strVIO = dsGeneric.Tables[0].Rows[0]["vio1_prod"].ToString() + ", " + dsGeneric.Tables[0].Rows[0]["vio2_prod"].ToString();
                        if (strVIO == "")
                        {
                            if (dsGeneric.Tables[0].Rows[0]["vio1_prod"].ToString() != "" || dsGeneric.Tables[0].Rows[0]["vio2_prod"].ToString() != "")
                                strVIO = dsGeneric.Tables[0].Rows[0]["vio1_prod"].ToString() + ", " + dsGeneric.Tables[0].Rows[0]["vio2_prod"].ToString();
                            else
                                strVIO = dsGeneric.Tables[0].Rows[0]["vio1"].ToString() + ", " + dsGeneric.Tables[0].Rows[0]["vio2"].ToString();
                        }
                        oTableInfo.AddCell(new Cell(new Phrase(strVIO, oFont)));
                    }
                    else
                    {
                        oTableInfo.AddCell(new Cell(new Phrase("Server Name:", oFontBold)));
                        oTableInfo.AddCell(new Cell(new Phrase(strName, oFont)));
                    }
                    int intAsset = 0;
                    DataSet dsAssets = oServer.GetAssets(intServer);
                    foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                    {
                        int intClassAsset = Int32.Parse(drAsset["classid"].ToString());
                        int intTestAsset = -1;
                        if (intClassAsset > 0)
                            intTestAsset = (oClass.IsTestDev(intClassAsset) ? 1 : 0);
                        if (intTestAsset == 1 && (boolTest == true || boolQA == true || (boolProd == true && oModelsProperties.IsVMwareVirtual(intModel) == true)))
                        {
                            intAsset = Int32.Parse(drAsset["assetid"].ToString());
                            break;
                        }
                        if (intTestAsset == 0 && boolProd == true)
                        {
                            intAsset = Int32.Parse(drAsset["assetid"].ToString());
                            break;
                        }
                    }
                    if (intAsset == 0 && dr["assetid"].ToString() != "")
                        intAsset = Int32.Parse(dr["assetid"].ToString());
                    if (dsGeneric.Tables[0].Rows.Count > 0)
                    {
                        if (oModelsProperties.IsStorageDB_BootSANUnix(intModel) == true || oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                        {
                            if ((boolTest == true || boolQA == true) && dsGeneric.Tables[0].Rows[0]["dummy_name"].ToString() != "")
                            {
                                oTableInfo.AddCell(new Cell(new Phrase("Dummy Name (BFS):", oFontBold)));
                                oTableInfo.AddCell(new Cell(new Phrase(dsGeneric.Tables[0].Rows[0]["dummy_name"].ToString(), oFont)));
                            }
                            if (boolProd == true && dsGeneric.Tables[0].Rows[0]["dummy_name_prod"].ToString() != "")
                            {
                                oTableInfo.AddCell(new Cell(new Phrase("Dummy Name (BFS):", oFontBold)));
                                oTableInfo.AddCell(new Cell(new Phrase(dsGeneric.Tables[0].Rows[0]["dummy_name_prod"].ToString(), oFont)));
                            }
                        }
                        if (oModelsProperties.IsVIO(intModel) == false)
                        {
                            if ((boolTest == true || boolQA == true) && (dsGeneric.Tables[0].Rows[0]["ww1"].ToString() != "" || dsGeneric.Tables[0].Rows[0]["ww2"].ToString() != ""))
                            {
                                oTableInfo.AddCell(new Cell(new Phrase("World Wide Port Name(s):", oFontBold)));
                                oTableInfo.AddCell(new Cell(new Phrase(dsGeneric.Tables[0].Rows[0]["ww1"].ToString() + ", " + dsGeneric.Tables[0].Rows[0]["ww2"].ToString(), oFont)));
                            }
                            if (boolProd == true && (dsGeneric.Tables[0].Rows[0]["ww1_prod"].ToString() != "" || dsGeneric.Tables[0].Rows[0]["ww2_prod"].ToString() != ""))
                            {
                                oTableInfo.AddCell(new Cell(new Phrase("World Wide Port Name(s):", oFontBold)));
                                oTableInfo.AddCell(new Cell(new Phrase(dsGeneric.Tables[0].Rows[0]["ww1_prod"].ToString() + ", " + dsGeneric.Tables[0].Rows[0]["ww2_prod"].ToString(), oFont)));
                            }
                        }
                    }
                    else
                    {
                        if (oModelsProperties.IsStorageDB_BootSANUnix(intModel) == true || oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                        {
                            oTableInfo.AddCell(new Cell(new Phrase("Dummy Name (BFS):", oFontBold)));
                            oTableInfo.AddCell(new Cell(new Phrase(oAsset.GetServerOrBlade(intAsset, "dummy_name"), oFont)));
                        }
                        DataSet dsHBA = oAsset.GetHBA(intAsset);
                        string strHBA = "";
                        foreach (DataRow drHBA in dsHBA.Tables[0].Rows)
                        {
                            if (strHBA != "")
                                strHBA += ", ";
                            strHBA += drHBA["name"].ToString();
                        }
                        oTableInfo.AddCell(new Cell(new Phrase("World Wide Port Names:", oFontBold)));
                        oTableInfo.AddCell(new Cell(new Phrase(strHBA, oFont)));
                    }
                    string strDRName = "";
                    if (oModelsProperties.IsVIO(intModel) == true)
                    {
                        if (boolUnder == true && dr["dr"].ToString() == "1")
                        {
                            if (dr["dr_name"].ToString() != "")
                            {
                                oTableInfo.AddCell(new Cell(new Phrase("DR Server Name:", oFontBold)));
                                strDRName = dr["dr_name"].ToString();
                                oTableInfo.AddCell(new Cell(new Phrase(strDRName, oFont)));
                            }
                            else
                            {
                                oTableInfo.AddCell(new Cell(new Phrase("DR Server Name(s):", oFontBold)));
                                strVIODR = dsGeneric.Tables[0].Rows[0]["vio1_dr"].ToString() + ", " + dsGeneric.Tables[0].Rows[0]["vio2_dr"].ToString();
                                oTableInfo.AddCell(new Cell(new Phrase(strVIODR, oFont)));
                            }
                        }
                    }
                    else
                    {
                        if (boolUnder == true && dr["dr"].ToString() == "1")
                        {
                            oTableInfo.AddCell(new Cell(new Phrase("DR Server Name:", oFontBold)));
                            if (dr["dr_name"].ToString() != "")
                                strDRName = dr["dr_name"].ToString();
                            else
                                strDRName = strName + "-DR";
                            oTableInfo.AddCell(new Cell(new Phrase(strDRName, oFont)));
                        }
                    }
                    int intDR = 0;
                    if (dr["drid"].ToString() != "")
                        intDR = Int32.Parse(dr["drid"].ToString());
                    if (boolProd == true)
                    {
                        if (dsGeneric.Tables[0].Rows.Count > 0)
                        {
                            if (oModelsProperties.IsStorageDB_BootSANUnix(intModel) == true || oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                            {
                                if (dsGeneric.Tables[0].Rows[0]["dummy_name_dr"].ToString() != "")
                                {
                                    oTableInfo.AddCell(new Cell(new Phrase("DR Dummy Name (BFS):", oFontBold)));
                                    oTableInfo.AddCell(new Cell(new Phrase(dsGeneric.Tables[0].Rows[0]["dummy_name_dr"].ToString(), oFont)));
                                }
                            }
                            if (oModelsProperties.IsVIO(intModel) == false)
                            {
                                if (dsGeneric.Tables[0].Rows[0]["ww1_dr"].ToString() != "" || dsGeneric.Tables[0].Rows[0]["ww2_dr"].ToString() != "")
                                {
                                    oTableInfo.AddCell(new Cell(new Phrase("DR World Wide Port Name(s):", oFontBold)));
                                    oTableInfo.AddCell(new Cell(new Phrase(dsGeneric.Tables[0].Rows[0]["ww1_dr"].ToString() + ", " + dsGeneric.Tables[0].Rows[0]["ww2_dr"].ToString(), oFont)));
                                }
                            }
                        }
                        else
                        {
                            if (oModelsProperties.IsStorageDB_BootSANUnix(intModel) == true || oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                            {
                                oTableInfo.AddCell(new Cell(new Phrase("DR Dummy Name (BFS):", oFontBold)));
                                oTableInfo.AddCell(new Cell(new Phrase(oAsset.GetServerOrBlade(intDR, "dummy_name"), oFont)));
                            }
                            DataSet dsHBA = oAsset.GetHBA(intDR);
                            string strHBA = "";
                            foreach (DataRow drHBA in dsHBA.Tables[0].Rows)
                            {
                                if (strHBA != "")
                                    strHBA += ", ";
                                strHBA += drHBA["name"].ToString();
                            }
                            oTableInfo.AddCell(new Cell(new Phrase("DR World Wide Port Names:", oFontBold)));
                            oTableInfo.AddCell(new Cell(new Phrase(strHBA, oFont)));
                        }
                    }
                    oTableInfo.AddCell(new Cell(new Phrase("IP Address (Assigned):", oFontBold)));
                    oTableInfo.AddCell(new Cell(new Phrase(oServer.GetIPs(intServer, 1, 0, 0, 0, dsnIP, "", ""), oFont)));
                    oTableInfo.AddCell(new Cell(new Phrase("IP Address (Final):", oFontBold)));
                    oTableInfo.AddCell(new Cell(new Phrase(oServer.GetIPs(intServer, 0, 1, 0, 0, dsnIP, "", ""), oFont)));
                    oTableInfo.AddCell(new Cell(new Phrase("IP Address (Backup):", oFontBold)));
                    oTableInfo.AddCell(new Cell(new Phrase(oServer.GetIPs(intServer, 0, 0, 0, 1, dsnIP, "", ""), oFont)));
                    int intType = oModelsProperties.GetType(intModel);
                    if (oOperatingSystem.IsMidrange(intOS) == true)
                        boolMidrange = true;
                    oTableInfo.AddCell(new Cell(new Phrase("Is a High Availability Device:", oFontBold)));
                    oTableInfo.AddCell(new Cell(new Phrase((oForecast.IsHARoom(_answerid) ? (dr["ha"].ToString() == "10" ? "Yes" : "No") : "N / A"), oFont)));
                    oTableInfo.AddCell(new Cell(new Phrase("Model:", oFontBold)));
                    oTableInfo.AddCell(new Cell(new Phrase(oModelsProperties.Get(intModel, "name"), oFont)));
                    oTableInfo.AddCell(new Cell(new Phrase("Fabric:", oFontBold)));
                    oTableInfo.AddCell(new Cell(new Phrase(oModelsProperties.GetFabric(intModel), oFont)));
                    if (intAsset > 0)
                    {
                        oTableInfo.AddCell(new Cell(new Phrase("Serial Number:", oFontBold)));
                        if (oModelsProperties.IsVMwareVirtual(intModel) == true)
                            oTableInfo.AddCell(new Cell(new Phrase(oAsset.Get(intAsset, "asset").ToUpper(), oFont)));
                        else
                            oTableInfo.AddCell(new Cell(new Phrase(oAsset.Get(intAsset, "serial").ToUpper(), oFont)));
                        oTableInfo.AddCell(new Cell(new Phrase("Asset Tag:", oFontBold)));
                        oTableInfo.AddCell(new Cell(new Phrase(oAsset.Get(intAsset, "asset").ToUpper(), oFont)));
                        oTableInfo.AddCell(new Cell(new Phrase("Remote Management Address:", oFontBold)));
                        oTableInfo.AddCell(new Cell(new Phrase(oAsset.GetServerOrBlade(intAsset, "ilo"), oFont)));
                        oTableInfo.AddCell(new Cell(new Phrase("Room:", oFontBold)));
                        oTableInfo.AddCell(new Cell(new Phrase(oAsset.GetServerOrBlade(intAsset, "room"), oFont)));
                        oTableInfo.AddCell(new Cell(new Phrase("Rack:", oFontBold)));
                        oTableInfo.AddCell(new Cell(new Phrase(oAsset.GetServerOrBlade(intAsset, "rack"), oFont)));
                        if (boolProd == true)
                        {
                            foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                            {
                                if (drAsset["latest"].ToString() == "0" && drAsset["dr"].ToString() == "0")
                                {
                                    int intAssetOld = Int32.Parse(drAsset["assetid"].ToString());
                                    oTableInfo.AddCell(new Cell(new Phrase(" - Previous Serial Number:", oFontBoldRed)));
                                    oTableInfo.AddCell(new Cell(new Phrase(oAsset.Get(intAssetOld, "serial").ToUpper(), oFontRed)));
                                    oTableInfo.AddCell(new Cell(new Phrase(" - Previous Asset Tag:", oFontBoldRed)));
                                    oTableInfo.AddCell(new Cell(new Phrase(oAsset.Get(intAssetOld, "asset").ToUpper(), oFontRed)));
                                    oTableInfo.AddCell(new Cell(new Phrase(" - Previous Class:", oFontBoldRed)));
                                    oTableInfo.AddCell(new Cell(new Phrase(drAsset["class"].ToString(), oFontRed)));
                                    oTableInfo.AddCell(new Cell(new Phrase(" - Previous Environment:", oFontBoldRed)));
                                    oTableInfo.AddCell(new Cell(new Phrase(drAsset["environment"].ToString(), oFontRed)));
                                    oTableInfo.AddCell(new Cell(new Phrase(" - Previous Room:", oFontBoldRed)));
                                    oTableInfo.AddCell(new Cell(new Phrase(oAsset.GetServerOrBlade(intAssetOld, "room"), oFontRed)));
                                    oTableInfo.AddCell(new Cell(new Phrase(" - Previous Rack:", oFontBoldRed)));
                                    oTableInfo.AddCell(new Cell(new Phrase(oAsset.GetServerOrBlade(intAssetOld, "rack"), oFontRed)));
                                    DataSet dsHBA = oAsset.GetHBA(intAssetOld);
                                    string strHBA = "";
                                    foreach (DataRow drHBA in dsHBA.Tables[0].Rows)
                                    {
                                        if (strHBA != "")
                                            strHBA += ", ";
                                        strHBA += drHBA["name"].ToString();
                                    }
                                    oTableInfo.AddCell(new Cell(new Phrase(" - Previous World Wide Port Names:", oFontBoldRed)));
                                    oTableInfo.AddCell(new Cell(new Phrase(strHBA, oFontRed)));
                                    if (oAsset.GetServerOrBlade(intAssetOld, "enclosureid") != "")
                                    {
                                        int intEnclosureID = Int32.Parse(oAsset.GetServerOrBlade(intAssetOld, "enclosureid"));
                                        if (intEnclosureID > 0)
                                        {
                                            oTableInfo.AddCell(new Cell(new Phrase(" - Previous Enclosure:", oFontBoldRed)));
                                            oTableInfo.AddCell(new Cell(new Phrase(oAsset.Get(intEnclosureID, "name"), oFontRed)));
                                            oTableInfo.AddCell(new Cell(new Phrase(" - Previous Slot:", oFontBoldRed)));
                                            oTableInfo.AddCell(new Cell(new Phrase(oAsset.GetServerOrBlade(intAssetOld, "slot"), oFontRed)));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (intDR == 0 && boolProd == true && oModelsProperties.IsVMwareVirtual(intModel) && oForecast.IsDRUnder48(_answerid, true))
                    {
                        // Add a DR counterpart for VMWare production bound (DR required) guests
                        string strDRAsset = oAsset.GetVSG("SERVER");
                        intDR = oAsset.Add("", oForecast.GetModel(_answerid), strDRAsset, strDRAsset, (int)AssetStatus.Available, intUser, DateTime.Now, 0, 1);
                        if (strDRName == "")
                        {
                            if (dr["dr_name"].ToString() != "")
                                strDRName = dr["dr_name"].ToString();
                            else
                                strDRName = strName + "-DR";
                        }
                        oAsset.UpdateVSG(strDRAsset, strDRName, "SERVER");
                        oAsset.AddStatus(intDR, strDRName, (int)AssetStatus.InUse, intUser, DateTime.Now);
                        oServer.AddAsset(intServer, intDR, intClass, intEnv, 0, 1);
                    }
                    if ((boolProd == true || (_san == false && _tsm == false)) && intDR > 0)
                    {
                        oTableInfo.AddCell(new Cell(new Phrase("DR Serial Number:", oFontBold)));
                        oTableInfo.AddCell(new Cell(new Phrase(oAsset.Get(intDR, "serial").ToUpper(), oFont)));
                        oTableInfo.AddCell(new Cell(new Phrase("DR Asset Tag:", oFontBold)));
                        oTableInfo.AddCell(new Cell(new Phrase(oAsset.Get(intDR, "asset").ToUpper(), oFont)));
                        oTableInfo.AddCell(new Cell(new Phrase("DR Room:", oFontBold)));
                        oTableInfo.AddCell(new Cell(new Phrase(oAsset.GetServerOrBlade(intDR, "room"), oFont)));
                        oTableInfo.AddCell(new Cell(new Phrase("DR Rack:", oFontBold)));
                        oTableInfo.AddCell(new Cell(new Phrase(oAsset.GetServerOrBlade(intDR, "rack"), oFont)));
                    }
                    oTableInfo.AddCell(new Cell(new Phrase("Current Class:", oFontBold)));
                    oTableInfo.AddCell(new Cell(new Phrase(strClass, oFont)));
                    oTableInfo.AddCell(new Cell(new Phrase("Final Class:", oFontBold)));
                    oTableInfo.AddCell(new Cell(new Phrase(oClass.Get(intClass, "name"), oFont)));
                    oTableInfo.AddCell(new Cell(new Phrase("Environment:", oFontBold)));
                    oTableInfo.AddCell(new Cell(new Phrase(oEnvironment.Get(intEnv, "name"), oFont)));
                    oTableInfo.AddCell(new Cell(new Phrase("Operating System:", oFontBold)));
                    oTableInfo.AddCell(new Cell(new Phrase(oOperatingSystem.Get(intOS, "name"), oFont)));
                    oTableInfo.AddCell(new Cell(new Phrase("Clustered Server Name:", oFontBold)));
                    oTableInfo.AddCell(new Cell(new Phrase("N / A", oFont)));
                    int intConsistency = Int32.Parse(dr["dr_consistencyid"].ToString());
                    if (intConsistency > 0)
                    {
                        oTableInfo.AddCell(new Cell(new Phrase("Consistency Group Name:", oFontBold)));
                        oTableInfo.AddCell(new Cell(new Phrase(oConsistencyGroups.Get(intConsistency, "name"), oFont)));
                        oTableInfo.AddCell(new Cell(new Phrase("Consistency Group Members:", oFontBold)));
                        DataSet dsMembers = oConsistencyGroups.GetMember(intServer);
                        string strMembers = "";
                        foreach (DataRow drMember in dsMembers.Tables[0].Rows)
                        {
                            if (strMembers != "")
                                strMembers += ", ";
                            strMembers += drMember["name"].ToString();
                        }
                        oTableInfo.AddCell(new Cell(new Phrase(strMembers, oFont)));
                    }
                    else
                    {
                        oTableInfo.AddCell(new Cell(new Phrase("Consistency Group:", oFontBold)));
                        oTableInfo.AddCell(new Cell(new Phrase("N / A", oFont)));
                    }

                    Cell oCell;
                    if (_tsm == true)
                        oCell = new Cell(new Phrase("ClearView Backup Request Form for " + strName, oFontHeader));
                    else if (_san == true)
                    {
                        string strStorageType = "";
                        if (oModelsProperties.IsTypeBlade(intModel) == true)
                            strStorageType = "BLADE";
                        else
                            strStorageType = "RACKED";
                        if (strVIO == "")
                        {
                            if (intCluster > 0)
                                strStorageType += " - CLUSTER";
                            oCell = new Cell(new Phrase("ClearView Storage Request Form for " + strName + " (" + strStorageType + ")", oFontHeader));
                        }
                        else
                            oCell = new Cell(new Phrase("ClearView Storage Request Form for " + strVIO + " (" + strStorageType + " - VIO)", oFontHeader));
                    }
                    else
                        oCell = new Cell(new Phrase("ClearView Birth Certificate for " + strName, oFontHeader));
                    oCell.BackgroundColor = new iTextSharp.text.Color(204, 204, 204);
                    oTableTop.AddCell(oCell);
                    oTableTop.InsertTable(oTableInfo);

                    if (_san == false)
                    {
                        // Backup Information
                        Cell oCellSpace2 = new Cell(new Phrase("spacer", oFontSpace));
                        oCellSpace2.BackgroundColor = new iTextSharp.text.Color(255, 255, 255);
                        oTableTop.AddCell(oCellSpace2);
                        Cell oCellBackup = new Cell(new Phrase("Backup Configuration", oFontBold));
                        oCellBackup.BackgroundColor = new iTextSharp.text.Color(204, 204, 204);
                        oTableTop.AddCell(oCellBackup);

                        iTextSharp.text.Table oTableBackup = new iTextSharp.text.Table(2);
                        oTableBackup.BorderWidth = 0;
                        oTableBackup.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                        oTableBackup.Padding = 2;
                        oTableBackup.Width = 100;
                        DataSet dsBackup = oForecast.GetBackup(_answerid);
                        if (dsBackup.Tables[0].Rows.Count > 0)
                        {
                            oTableBackup.AddCell(new Cell(new Phrase("Recovery Location:", oFontBold)));
                            if (dsBackup.Tables[0].Rows[0]["recoveryid"].ToString() != "")
                                oTableBackup.AddCell(new Cell(new Phrase(oLocation.GetFull(Int32.Parse(dsBackup.Tables[0].Rows[0]["recoveryid"].ToString())), oFont)));
                            oTableBackup.AddCell(new Cell(new Phrase("Timing/Frequency of Backups:", oFontBold)));
                            if (dsBackup.Tables[0].Rows[0]["daily"].ToString() == "1")
                                oTableBackup.AddCell(new Cell(new Phrase("Daily", oFont)));
                            else if (dsBackup.Tables[0].Rows[0]["weekly"].ToString() == "1")
                            {
                                oTableBackup.AddCell(new Cell(new Phrase("Weekly", oFont)));
                                oTableBackup.AddCell(new Cell(new Phrase("Occurring on:", oFontBold)));
                                oTableBackup.AddCell(new Cell(new Phrase(dsBackup.Tables[0].Rows[0]["weekly_day"].ToString(), oFont)));
                            }
                            else if (dsBackup.Tables[0].Rows[0]["monthly"].ToString() == "1")
                                oTableBackup.AddCell(new Cell(new Phrase("Monthly", oFont)));
                            oTableBackup.AddCell(new Cell(new Phrase("Start Time:", oFontBold)));
                            if (dsBackup.Tables[0].Rows[0]["time"].ToString() == "1")
                                oTableBackup.AddCell(new Cell(new Phrase(dsBackup.Tables[0].Rows[0]["time_hour"].ToString() + " " + dsBackup.Tables[0].Rows[0]["time_switch"].ToString(), oFont)));
                            else
                                oTableBackup.AddCell(new Cell(new Phrase("Don't Care", oFont)));
                            oTableBackup.AddCell(new Cell(new Phrase("Start Date:", oFontBold)));
                            if (dsBackup.Tables[0].Rows[0]["start_date"].ToString() != "")
                                oTableBackup.AddCell(new Cell(new Phrase(DateTime.Parse(dsBackup.Tables[0].Rows[0]["start_date"].ToString()).ToShortDateString(), oFont)));
                            else
                                oTableBackup.AddCell(new Cell(new Phrase("N / A", oFont)));
                            double dblHighU = 0.00;
                            double dblStandardU = 0.00;
                            double dblLowU = 0.00;
                            double dblHighQAU = 0.00;
                            double dblStandardQAU = 0.00;
                            double dblLowQAU = 0.00;
                            double dblHighTestU = 0.00;
                            double dblStandardTestU = 0.00;
                            double dblLowTestU = 0.00;
                            DataSet dsStorage = oStorage.GetLuns(_answerid, 0, intCluster, intCSM, intNumber);
                            foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
                            {
                                if (drStorage["size"].ToString() != "")
                                {
                                    if (drStorage["performance"].ToString() == "High")
                                        dblHighU += double.Parse(drStorage["size"].ToString());
                                    if (drStorage["performance"].ToString() == "Standard")
                                        dblStandardU += double.Parse(drStorage["size"].ToString());
                                    if (drStorage["performance"].ToString() == "Low")
                                        dblLowU += double.Parse(drStorage["size"].ToString());
                                }
                                if (drStorage["size_qa"].ToString() != "")
                                {
                                    if (drStorage["performance"].ToString() == "High")
                                        dblHighQAU += double.Parse(drStorage["size_qa"].ToString());
                                    if (drStorage["performance"].ToString() == "Standard")
                                        dblStandardQAU += double.Parse(drStorage["size_qa"].ToString());
                                    if (drStorage["performance"].ToString() == "Low")
                                        dblLowQAU += double.Parse(drStorage["size_qa"].ToString());
                                }
                                if (drStorage["size_test"].ToString() != "")
                                {
                                    if (drStorage["performance"].ToString() == "High")
                                        dblHighTestU += double.Parse(drStorage["size_test"].ToString());
                                    if (drStorage["performance"].ToString() == "Standard")
                                        dblStandardTestU += double.Parse(drStorage["size_test"].ToString());
                                    if (drStorage["performance"].ToString() == "Low")
                                        dblLowTestU += double.Parse(drStorage["size_test"].ToString());
                                }
                                DataSet dsMount = oStorage.GetMountPoints(Int32.Parse(drStorage["id"].ToString()));
                                foreach (DataRow drMount in dsMount.Tables[0].Rows)
                                {
                                    if (drMount["size"].ToString() != "")
                                    {
                                        if (drMount["performance"].ToString() == "High")
                                            dblHighU += double.Parse(drMount["size"].ToString());
                                        if (drMount["performance"].ToString() == "Standard")
                                            dblStandardU += double.Parse(drMount["size"].ToString());
                                        if (drMount["performance"].ToString() == "Low")
                                            dblLowU += double.Parse(drMount["size"].ToString());
                                    }
                                    if (drMount["size_qa"].ToString() != "")
                                    {
                                        if (drMount["performance"].ToString() == "High")
                                            dblHighQAU += double.Parse(drMount["size_qa"].ToString());
                                        if (drMount["performance"].ToString() == "Standard")
                                            dblStandardQAU += double.Parse(drMount["size_qa"].ToString());
                                        if (drMount["performance"].ToString() == "Low")
                                            dblLowQAU += double.Parse(drMount["size_qa"].ToString());
                                    }
                                    if (drMount["size_test"].ToString() != "")
                                    {
                                        if (drMount["performance"].ToString() == "High")
                                            dblHighTestU += double.Parse(drMount["size_test"].ToString());
                                        if (drMount["performance"].ToString() == "Standard")
                                            dblStandardTestU += double.Parse(drMount["size_test"].ToString());
                                        if (drMount["performance"].ToString() == "Low")
                                            dblLowTestU += double.Parse(drMount["size_test"].ToString());
                                    }
                                }
                            }
                            double dblTotal = dblHighU + dblStandardU + dblLowU + dblHighQAU + dblStandardQAU + dblLowQAU + dblHighTestU + dblStandardTestU + dblLowTestU;
                            oTableBackup.AddCell(new Cell(new Phrase("Total Combined Disk Capacity (GB):", oFontBold)));
                            oTableBackup.AddCell(new Cell(new Phrase(dblTotal.ToString("0") + " GB", oFont)));
                            oTableBackup.AddCell(new Cell(new Phrase("Current Combined Disk Utilized (GB):", oFontBold)));
                            oTableBackup.AddCell(new Cell(new Phrase("5 GB", oFont)));
                            oTableBackup.AddCell(new Cell(new Phrase("Average Size of One Typical Data File:", oFontBold)));
                            oTableBackup.AddCell(new Cell(new Phrase(dsBackup.Tables[0].Rows[0]["average_one"].ToString() + " GB", oFont)));
                            oTableBackup.AddCell(new Cell(new Phrase("Production Turnover Documentation:", oFontBold)));
                            if (dsBackup.Tables[0].Rows[0]["documentation"].ToString() == "")
                                oTableBackup.AddCell(new Cell(new Phrase("Not Specified", oFont)));
                            else
                                oTableBackup.AddCell(new Cell(new Phrase(dsBackup.Tables[0].Rows[0]["documentation"].ToString(), oFont)));
                        }
                        oTableTop.InsertTable(oTableBackup);
                    }

                    if (_tsm == false)
                    {
                        // Storage Information
                        Cell oCellSpace3 = new Cell(new Phrase("spacer", oFontSpace));
                        oCellSpace3.BackgroundColor = new iTextSharp.text.Color(255, 255, 255);
                        oTableTop.AddCell(oCellSpace3);
                        string strStorageType = "NON-SHARED Storage Configuration";
                        if (oModelsProperties.IsVIO(intModel) == true)
                            strStorageType = "SHARED Storage Configuration";
                        Cell oCellStorage = new Cell(new Phrase(strStorageType, oFontBold));
                        oCellStorage.BackgroundColor = new iTextSharp.text.Color(204, 204, 204);
                        oTableTop.AddCell(oCellStorage);
                        int intStorageCols = 7;
                        if (boolProd == true)
                            intStorageCols = intStorageCols - 1;
                        if (boolTest == true || boolQA == true)
                            intStorageCols = intStorageCols - 3;
                        iTextSharp.text.Table oTableStorage = new iTextSharp.text.Table(intStorageCols);
                        oTableStorage.BorderWidth = 0;
                        oTableStorage.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                        oTableStorage.Padding = 2;
                        oTableStorage.Width = 100;
                        oTableStorage.AddCell(new Cell(new Phrase("LUN #", oFontBold)));
                        oTableStorage.AddCell(new Cell(new Phrase("Path", oFontBold)));
                        oTableStorage.AddCell(new Cell(new Phrase("Performance", oFontBold)));
                        if (boolProd == true)
                            oTableStorage.AddCell(new Cell(new Phrase("Size - Prod (GB)", oFontBold)));
                        if (boolQA == true)
                            oTableStorage.AddCell(new Cell(new Phrase("Size - QA (GB)", oFontBold)));
                        if (boolTest == true)
                            oTableStorage.AddCell(new Cell(new Phrase("Size - Test (GB)", oFontBold)));
                        if (boolProd == true)
                            oTableStorage.AddCell(new Cell(new Phrase("Replicated", oFontBold)));
                        if (boolProd == true)
                            oTableStorage.AddCell(new Cell(new Phrase("High Avail", oFontBold)));
                        DataSet dsLuns = new DataSet();
                        if (intCluster == 0)
                            dsLuns = oStorage.GetLuns(_answerid, 0, intCluster, intCSM, intNumber);
                        else
                            dsLuns = oStorage.GetLunsClusterNonShared(_answerid, intCluster, intNumber);
                        if (dsLuns.Tables[0].Rows.Count > 0)
                        {
                            if (oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                            {
                                oTableStorage.AddCell(new Cell(new Phrase("1", oFont)));
                                if (oClass.Get(intClass, "pnc") == "1")
                                {
                                    oTableStorage.AddCell(new Cell(new Phrase("C:, D:", oFont)));
                                    oTableStorage.AddCell(new Cell(new Phrase(dsLuns.Tables[0].Rows[0]["performance"].ToString(), oFont)));
                                    if (boolProd == true)
                                        oTableStorage.AddCell(new Cell(new Phrase("40 GB", oFont)));
                                    if (boolQA == true)
                                        oTableStorage.AddCell(new Cell(new Phrase("40 GB", oFont)));
                                    if (boolTest == true)
                                        oTableStorage.AddCell(new Cell(new Phrase("40 GB", oFont)));
                                    if (boolProd == true)
                                        oTableStorage.AddCell(new Cell(new Phrase((dsLuns.Tables[0].Rows[0]["replicated"].ToString() == "0" ? "No" : "Yes"), oFont)));
                                    if (boolProd == true)
                                        oTableStorage.AddCell(new Cell(new Phrase((dsLuns.Tables[0].Rows[0]["high_availability"].ToString() == "0" ? "No" : "Yes (40 GB)"), oFont)));
                                }
                                else
                                {
                                    oTableStorage.AddCell(new Cell(new Phrase("C:, E:", oFont)));
                                    oTableStorage.AddCell(new Cell(new Phrase(dsLuns.Tables[0].Rows[0]["performance"].ToString(), oFont)));
                                    if (boolProd == true)
                                        oTableStorage.AddCell(new Cell(new Phrase("40 GB", oFont)));
                                    if (boolQA == true)
                                        oTableStorage.AddCell(new Cell(new Phrase("40 GB", oFont)));
                                    if (boolTest == true)
                                        oTableStorage.AddCell(new Cell(new Phrase("40 GB", oFont)));
                                    if (boolProd == true)
                                        oTableStorage.AddCell(new Cell(new Phrase((dsLuns.Tables[0].Rows[0]["replicated"].ToString() == "0" ? "No" : "Yes"), oFont)));
                                    if (boolProd == true)
                                        oTableStorage.AddCell(new Cell(new Phrase((dsLuns.Tables[0].Rows[0]["high_availability"].ToString() == "0" ? "No" : "Yes (40 GB)"), oFont)));
                                }
                            }
                            if (oModelsProperties.IsStorageDB_BootSANUnix(intModel) == true)
                            {
                                oTableStorage.AddCell(new Cell(new Phrase("1", oFont)));
                                oTableStorage.AddCell(new Cell(new Phrase("/", oFont)));
                                oTableStorage.AddCell(new Cell(new Phrase(dsLuns.Tables[0].Rows[0]["performance"].ToString(), oFont)));
                                if (oClass.Get(intClass, "pnc") == "1")
                                {
                                    if (boolProd == true)
                                        oTableStorage.AddCell(new Cell(new Phrase("40 GB", oFont)));
                                    if (boolQA == true)
                                        oTableStorage.AddCell(new Cell(new Phrase("40 GB", oFont)));
                                    if (boolTest == true)
                                        oTableStorage.AddCell(new Cell(new Phrase("40 GB", oFont)));
                                    if (boolProd == true)
                                        oTableStorage.AddCell(new Cell(new Phrase((dsLuns.Tables[0].Rows[0]["replicated"].ToString() == "0" ? "No" : "Yes"), oFont)));
                                    if (boolProd == true)
                                        oTableStorage.AddCell(new Cell(new Phrase((dsLuns.Tables[0].Rows[0]["high_availability"].ToString() == "0" ? "No" : "Yes (40 GB)"), oFont)));
                                }
                                else
                                {
                                    if (boolProd == true)
                                        oTableStorage.AddCell(new Cell(new Phrase("50 GB", oFont)));
                                    if (boolQA == true)
                                        oTableStorage.AddCell(new Cell(new Phrase("50 GB", oFont)));
                                    if (boolTest == true)
                                        oTableStorage.AddCell(new Cell(new Phrase("50 GB", oFont)));
                                    if (boolProd == true)
                                        oTableStorage.AddCell(new Cell(new Phrase((dsLuns.Tables[0].Rows[0]["replicated"].ToString() == "0" ? "No" : "Yes"), oFont)));
                                    if (boolProd == true)
                                        oTableStorage.AddCell(new Cell(new Phrase((dsLuns.Tables[0].Rows[0]["high_availability"].ToString() == "0" ? "No" : "Yes (50 GB)"), oFont)));
                                }
                            }
                        }
                        int intRow = 1;
                        bool boolOther = false;
                        foreach (DataRow drLun in dsLuns.Tables[0].Rows)
                        {
                            boolOther = !boolOther;
                            intRow++;
                            oTableStorage.AddCell(new Cell(new Phrase(intRow.ToString(), oFont)));
                            string strPath = drLun["path"].ToString();
                            string strLetter = drLun["letter"].ToString();
                            if (strLetter == "")
                            {
                                if (drLun["driveid"].ToString() == "-1000")
                                    strLetter = "E";
                                else if (drLun["driveid"].ToString() == "-100")
                                    strLetter = "F";
                                else if (drLun["driveid"].ToString() == "-10")
                                    strLetter = "P";
                                else if (drLun["driveid"].ToString() == "-1")
                                    strLetter = "Q";
                            }
                            if ((boolOverride == true && drLun["driveid"].ToString() == "0") || boolMidrange == true)
                                oTableStorage.AddCell(new Cell(new Phrase(strPath, oFont)));
                            else
                                oTableStorage.AddCell(new Cell(new Phrase(strLetter + ":" + drLun["path"].ToString(), oFont)));
                            oTableStorage.AddCell(new Cell(new Phrase(drLun["performance"].ToString(), oFont)));
                            if (boolProd == true)
                                oTableStorage.AddCell(new Cell(new Phrase(drLun["size"].ToString() + " GB", oFont)));
                            if (boolQA == true)
                                oTableStorage.AddCell(new Cell(new Phrase(drLun["size_qa"].ToString() + " GB", oFont)));
                            if (boolTest == true)
                                oTableStorage.AddCell(new Cell(new Phrase(drLun["size_test"].ToString() + " GB", oFont)));
                            if (boolProd == true)
                                oTableStorage.AddCell(new Cell(new Phrase((drLun["replicated"].ToString() == "0" ? "No" : "Yes"), oFont)));
                            if (boolProd == true)
                                oTableStorage.AddCell(new Cell(new Phrase((drLun["high_availability"].ToString() == "0" ? "No" : "Yes (" + drLun["size"].ToString() + " GB)"), oFont)));
                            DataSet dsPoints = oStorage.GetMountPoints(Int32.Parse(drLun["id"].ToString()));
                            int intPoint = 0;
                            foreach (DataRow drPoint in dsPoints.Tables[0].Rows)
                            {
                                boolOther = !boolOther;
                                intRow++;
                                intPoint++;
                                oTableStorage.AddCell(new Cell(new Phrase(intRow.ToString(), oFont)));
                                strPath = drPoint["path"].ToString();
                                if (boolMidrange == true)
                                    oTableStorage.AddCell(new Cell(new Phrase(strPath, oFont)));
                                else
                                    oTableStorage.AddCell(new Cell(new Phrase(strLetter + ":\\SH" + drLun["driveid"].ToString() + "VOL" + (intPoint < 10 ? "0" : "") + intPoint.ToString(), oFont)));
                                oTableStorage.AddCell(new Cell(new Phrase(drPoint["performance"].ToString(), oFont)));
                                if (boolProd == true)
                                    oTableStorage.AddCell(new Cell(new Phrase(drPoint["size"].ToString() + " GB", oFont)));
                                if (boolQA == true)
                                    oTableStorage.AddCell(new Cell(new Phrase(drPoint["size_qa"].ToString() + " GB", oFont)));
                                if (boolTest == true)
                                    oTableStorage.AddCell(new Cell(new Phrase(drPoint["size_test"].ToString() + " GB", oFont)));
                                if (boolProd == true)
                                    oTableStorage.AddCell(new Cell(new Phrase((drPoint["replicated"].ToString() == "0" ? "No" : "Yes"), oFont)));
                                if (boolProd == true)
                                    oTableStorage.AddCell(new Cell(new Phrase((drPoint["high_availability"].ToString() == "0" ? "No" : "Yes (" + drPoint["size"].ToString() + " GB)"), oFont)));
                            }
                        }
                        oTableTop.InsertTable(oTableStorage);
                        if (intCluster > 0)
                        {
                            bool boolAddShared = false;
                            if (intCount < ds.Tables[0].Rows.Count)
                            {
                                int intNextCluster = Int32.Parse(ds.Tables[0].Rows[intCount]["clusterid"].ToString());
                                if (intNextCluster != intCluster)
                                    boolAddShared = true;
                            }
                            else
                                boolAddShared = true;
                            if (boolAddShared == true)
                            {
                                // Get Shared Storage
                                Cell oCellSpace4 = new Cell(new Phrase("spacer", oFontSpace));
                                oCellSpace4.BackgroundColor = new iTextSharp.text.Color(255, 255, 255);
                                oTableTop.AddCell(oCellSpace4);
                                Cell oCellStorage2 = new Cell(new Phrase("SHARED Storage Configuration (" + strShared + ")", oFontBold));
                                oCellStorage2.BackgroundColor = new iTextSharp.text.Color(204, 204, 204);
                                oTableTop.AddCell(oCellStorage2);
                                iTextSharp.text.Table oTableStorage2 = new iTextSharp.text.Table(intStorageCols);
                                oTableStorage2.BorderWidth = 0;
                                oTableStorage2.BorderColor = new iTextSharp.text.Color(255, 255, 255);
                                oTableStorage2.Padding = 2;
                                oTableStorage2.Width = 100;
                                oTableStorage2.AddCell(new Cell(new Phrase("LUN #", oFontBold)));
                                oTableStorage2.AddCell(new Cell(new Phrase("Path", oFontBold)));
                                oTableStorage2.AddCell(new Cell(new Phrase("Performance", oFontBold)));
                                if (boolProd == true)
                                    oTableStorage2.AddCell(new Cell(new Phrase("Size - Prod (GB)", oFontBold)));
                                if (boolQA == true)
                                    oTableStorage2.AddCell(new Cell(new Phrase("Size - QA (GB)", oFontBold)));
                                if (boolTest == true)
                                    oTableStorage2.AddCell(new Cell(new Phrase("Size - Test (GB)", oFontBold)));
                                if (boolProd == true)
                                    oTableStorage2.AddCell(new Cell(new Phrase("Replicated", oFontBold)));
                                if (boolProd == true)
                                    oTableStorage2.AddCell(new Cell(new Phrase("High Avail", oFontBold)));
                                dsLuns = oStorage.GetLunsClusterShared(_answerid, intCluster);
                                intRow = 0;
                                boolOther = false;
                                foreach (DataRow drLun in dsLuns.Tables[0].Rows)
                                {
                                    boolOther = !boolOther;
                                    intRow++;
                                    oTableStorage2.AddCell(new Cell(new Phrase(intRow.ToString(), oFont)));
                                    string strPath = drLun["path"].ToString();
                                    string strLetter = drLun["letter"].ToString();
                                    if (strLetter == "")
                                    {
                                        if (drLun["driveid"].ToString() == "-1000")
                                            strLetter = "E";
                                        else if (drLun["driveid"].ToString() == "-100")
                                            strLetter = "F";
                                        else if (drLun["driveid"].ToString() == "-10")
                                            strLetter = "P";
                                        else if (drLun["driveid"].ToString() == "-1")
                                            strLetter = "Q";
                                    }
                                    if ((boolOverride == true && drLun["driveid"].ToString() == "0") || boolMidrange == true)
                                        oTableStorage2.AddCell(new Cell(new Phrase(strPath, oFont)));
                                    else
                                        oTableStorage2.AddCell(new Cell(new Phrase(strLetter + ":" + drLun["path"].ToString(), oFont)));
                                    oTableStorage2.AddCell(new Cell(new Phrase(drLun["performance"].ToString(), oFont)));
                                    if (boolProd == true)
                                        oTableStorage2.AddCell(new Cell(new Phrase(drLun["size"].ToString() + " GB", oFont)));
                                    if (boolQA == true)
                                        oTableStorage2.AddCell(new Cell(new Phrase(drLun["size_qa"].ToString() + " GB", oFont)));
                                    if (boolTest == true)
                                        oTableStorage2.AddCell(new Cell(new Phrase(drLun["size_test"].ToString() + " GB", oFont)));
                                    if (boolProd == true)
                                        oTableStorage2.AddCell(new Cell(new Phrase((drLun["replicated"].ToString() == "0" ? "No" : "Yes"), oFont)));
                                    if (boolProd == true)
                                        oTableStorage2.AddCell(new Cell(new Phrase((drLun["high_availability"].ToString() == "0" ? "No" : "Yes (" + drLun["size"].ToString() + " GB)"), oFont)));
                                    DataSet dsPoints = oStorage.GetMountPoints(Int32.Parse(drLun["id"].ToString()));
                                    int intPoint = 0;
                                    foreach (DataRow drPoint in dsPoints.Tables[0].Rows)
                                    {
                                        boolOther = !boolOther;
                                        intRow++;
                                        intPoint++;
                                        oTableStorage2.AddCell(new Cell(new Phrase(intRow.ToString(), oFont)));
                                        strPath = drPoint["path"].ToString();
                                        if (boolMidrange == true)
                                            oTableStorage2.AddCell(new Cell(new Phrase(strPath, oFont)));
                                        else
                                            oTableStorage2.AddCell(new Cell(new Phrase(strLetter + ":\\SH" + drLun["driveid"].ToString() + "VOL" + (intPoint < 10 ? "0" : "") + intPoint.ToString(), oFont)));
                                        oTableStorage2.AddCell(new Cell(new Phrase(drPoint["performance"].ToString(), oFont)));
                                        if (boolProd == true)
                                            oTableStorage2.AddCell(new Cell(new Phrase(drPoint["size"].ToString() + " GB", oFont)));
                                        if (boolQA == true)
                                            oTableStorage2.AddCell(new Cell(new Phrase(drPoint["size_qa"].ToString() + " GB", oFont)));
                                        if (boolTest == true)
                                            oTableStorage2.AddCell(new Cell(new Phrase(drPoint["size_test"].ToString() + " GB", oFont)));
                                        if (boolProd == true)
                                            oTableStorage2.AddCell(new Cell(new Phrase((drPoint["replicated"].ToString() == "0" ? "No" : "Yes"), oFont)));
                                        if (boolProd == true)
                                            oTableStorage2.AddCell(new Cell(new Phrase((drPoint["high_availability"].ToString() == "0" ? "No" : "Yes (" + drPoint["size"].ToString() + " GB)"), oFont)));
                                    }
                                }
                                oTableTop.InsertTable(oTableStorage2);
                                strShared = "";
                            }
                        }
                    }
                }

                doc.Add(oTableTop);
                doc.Close();

                if (_save == true)
                {
                    
                    if (_add == true)
                    {
                        if (_tsm == true)
                            oDocument.Add(intProject, 0, "TSM Registration Form [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", strFile, "The form sent to the TSM Request mailbox for your TSM configuration of server " + strName, 1, 0);
                        else if (_san == true)
                        {
                            if (boolProd == true || boolQA == true || boolTest == true)
                                oDocument.Add(intProject, 0, "SAN Registration Form [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", strFile, "The form sent to the SAN Request mailbox for your SAN configuration of server " + strName, 1, 0);
                        }
                        else
                            oDocument.Add(intProject, 0, "Birth Certificate [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", strFile, "The birth certificate contains all the information for your device", 1, 0);
                    }
                    if (_send == true)
                    {
                        if (_tsm == true)
                            oFunction.SendEmail("TSM Registration Form [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", "", "", strEMailIdsBCC, "TSM Registration Form [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", "<p><b>A new TSM registration form has been created. You are required to act on this request.</b></p><p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p><p>Please view the attached document for the details of this request.</p>", true, false, strFile);
                        else if (_san == true)
                        {
                            if (boolProd == true || boolQA == true || boolTest == true)
                                oFunction.SendEmail("SAN Registration Form [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", "", "", strEMailIdsBCC, "SAN Registration Form [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", "<p><b>A new SAN registration form has been created. You are required to act on this request.</b></p><p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p><p>Please view the attached document for the details of this request.</p>", true, false, strFile);
                        }
                        else
                        {
                           
                            string strEMailIdsCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_BIRTH_CERTIFICATE");
                            string strEMailIdsCCPNC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_BIRTH_CERTIFICATE_PNC");
                            oFunction.SendEmail("Birth Certificate [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", "", strEMailIdsCC, strEMailIdsBCC, "Birth Certificate [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", "<p><b>New devices have been installed into the environment.</b></p><p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p><p>Please reference the attached document to view all the information about these devices.</p>", true, false, strFile);
                            if (boolPNC == true)
                                oFunction.SendEmail("Birth Certificate [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", "", strEMailIdsCCPNC, strEMailIdsBCC, "Birth Certificate [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", "<p><b>New devices have been installed into the environment.</b></p><p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p><p>Please reference the attached document to view all the information about these devices.</p>", false, false, strFile);
                            //oFunction.SendEmail("Birth Certificate [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", "catherine.zorn@pnc.com;gary.swiderski@pnc.com;donald.corace@pnc.com;anthony.denicola@pnc.com;lisa.stevens@pnc.com;joseph.cavolo@pnc.com;karen.folmar@pnc.com;vance.porter@pnc.com;siw@pnc.com;james.white@pnc.com;", "GM3725E@nationalcity.com;NCCIS6EPSNetworkControl@nationalcity.com;Noah.Hester@nationalcity.com;Darrel.Latimer@nationalcity.com;Marcus.Noel@nationalcity.com;QP-IP@nationalcity.com;Paul.Elwell@nationalcity.com;Chris.Clymer@nationalcity.com;Bryan.Murphy@nationalcity.com;", strBCC, "Birth Certificate [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", "<p><b>New devices have been installed into the environment.</b></p><p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p><p>Please reference the attached document to view all the information about these devices.</p>", false, false, strFile);
                        }
                    }
                    return null;
                }
                else if (_san == true)
                {
                    if (boolProd == true || boolQA == true || boolTest == true)
                        return doc;
                    else
                        return null;
                }
                else
                    return doc;
            }
            else
                return null;
        }
    }
}
