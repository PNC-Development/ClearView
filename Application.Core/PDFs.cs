using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;

namespace NCC.ClearView.Application.Core
{
    public class PDFs
    {
        private string dsn = "";
        private string dsnAsset = "";
        private string dsnIP = "";
        private int intEnvironment;
        private Variables oVariables;

        public PDFs(string _dsn, string _dsn_asset, string _dsn_ip, int _environment)
        {
            dsn = _dsn;
            dsnAsset = _dsn_asset;
            dsnIP = _dsn_ip;
            intEnvironment = _environment;
            oVariables = new Variables(_environment);
        }
        public Document CreateDocuments(int _answerid, bool _san, bool _tsm, Stream _stream, bool _save, bool _add, bool _send, bool _prod, bool _use_pnc_naming, bool _send_to_project)
        {
            Documents oDocument = new Documents(0, dsn);
            int intProject = 0;
            int intImplementor = -999;
            string strFile = "";
            string strFileName = "";
            string strName = "";
            string strEMailIdsBCC = "";
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
                        if (oForecast.IsDRUnder48(_answerid, false) == true)
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
                        if (oForecast.IsDRUnder48(_answerid, false) == true)
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

                bool boolFileExists = false;
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
                    if (File.Exists(strFile) == true)
                        boolFileExists = true;
                    else
                        PdfWriter.GetInstance(doc, new FileStream(strFile, FileMode.Create));
                }
                else
                    PdfWriter.GetInstance(doc, _stream);

                if (boolFileExists == false)
                {
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
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(oVariable.DocumentsFolder() + "logo.gif");
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
                        Functions oFunction = new Functions(0, dsn, intEnvironment);
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
                            {
                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_ALERT");
                                oFunction.SendEmail("A new TSM registration form has been created. You are required to act on this request.", "GM2768P", "", strEMailIdsBCC, "TSM Registration Form [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", "<p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p><p>Please view the attached document for the details of this request.</p>", true, false, strFile);
                            }
                            else if (_san == true)
                            {
                                if (boolProd == true || boolQA == true || boolTest == true)
                                {
                                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_ALERT");
                                    oFunction.SendEmail("A new SAN registration form has been created. You are required to act on this request.", "GM2134E", "", strEMailIdsBCC, "SAN Registration Form [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", "<p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p><p>Please view the attached document for the details of this request.</p>", true, false, strFile);
                                }
                            }
                            else
                            {
                                string strProject = oProject.Get(intProject, "number");
                                //string strTo = oUser.GetName(intImplementor);
                                if (oProject.IsTest(intProject) == true)
                                    boolPNC = false;

                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_ALERT");
                                string strEMailIdsCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_BIRTH_CERTIFICATE");
                                string strEMailIdsCCPNC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_BIRTH_CERTIFICATE_PNC");
                                oFunction.SendEmail("New devices have been installed into the environment.", strEMailIdsBCC, strEMailIdsCC, "", "Birth Certificate [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", "<p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p><p>Please reference the attached document to view all the information about these devices.</p>", true, false, strFile);
                                if (boolPNC == true)
                                    oFunction.SendEmail("New devices have been installed into the environment.", strEMailIdsCCPNC, "", strEMailIdsBCC, "Birth Certificate [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", "<p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p><p>Please reference the attached document to view all the information about these devices.</p>", false, false, strFile);

                                if (oProject.IsTest(intProject) == false && _send_to_project == true)
                                {
                                    string strRecipients = "";
                                    string strTO = "";
                                    string strCC = "";
                                    string strExecutedBy = oForecast.GetAnswer(_answerid, "executed_by");
                                    if (strExecutedBy != "")
                                    {
                                        strRecipients += "Executed By : " + oUser.GetFullName(Int32.Parse(strExecutedBy)) + "<br/>";
                                        strTO += oUser.GetName(Int32.Parse(strExecutedBy)) + ";";
                                    }
                                    string strUserId = oForecast.GetAnswer(_answerid, "userid");
                                    if (strUserId != "")
                                    {
                                        strRecipients += "Entered By : " + oUser.GetFullName(Int32.Parse(strUserId)) + "<br/>";
                                        strTO += oUser.GetName(Int32.Parse(strUserId)) + ";";
                                    }

                                    // Project Manager
                                    string strManager = oProject.Get(intProject, "lead");
                                    if (strManager != "")
                                    {
                                        strRecipients += "Project Manager : " + oUser.GetFullName(Int32.Parse(strManager)) + "<br/>";
                                        strCC += oUser.GetName(Int32.Parse(strManager)) + ";";
                                    }
                                    // Integration Engineer
                                    string strEngineer = oProject.Get(intProject, "engineer");
                                    if (strEngineer != "")
                                    {
                                        strRecipients += "Technical Contact : " + oUser.GetFullName(Int32.Parse(strEngineer)) + "<br/>";
                                        strCC += oUser.GetName(Int32.Parse(strEngineer)) + ";";
                                    }
                                    // Technical Lead
                                    string strTechnical = oProject.Get(intProject, "technical");
                                    if (strTechnical != "")
                                    {
                                        strRecipients += "Technical Lead : " + oUser.GetFullName(Int32.Parse(strTechnical)) + "<br/>";
                                        strCC += oUser.GetName(Int32.Parse(strTechnical)) + ";";
                                    }

                                    // Implementor
                                    if (intImplementor > 0)
                                    {
                                        strRecipients += "Implementor : " + oUser.GetFullName(intImplementor) + "<br/>";
                                        strCC += oUser.GetName(intImplementor) + ";";
                                    }

                                    if (strTO != "" || strCC != "")
                                        oFunction.SendEmail("Birth Certificate [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", strTO, strCC, strEMailIdsBCC, "Birth Certificate [" + oProject.Get(intProject, "name") + " | " + oForecast.GetAnswer(_answerid, "name") + "]", "<p><b>New devices have been installed into the environment.</b></p><p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p><p>Please reference the attached document to view all the information about these devices.</p><p>This email was sent to the following people...</p><p>" + strRecipients + "</p>", true, false, strFile);
                                }
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
            else
                return null;
        }

        public void CreateSCRequest(int _serverid, bool _use_pnc_naming)
        {
            Servers oServer = new Servers(0, dsn);
            VMWare oVMWare = new VMWare(0, dsn);
            Forecast oForecast = new Forecast(0, dsn);
            Environments oEnvironment = new Environments(0, dsn);
            OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);
            Classes oClass = new Classes(0, dsn);
            Requests oRequest = new Requests(0, dsn);
            Projects oProject = new Projects(0, dsn);
            ServerName oServerName = new ServerName(0, dsn);
            Locations oLocations = new Locations(0, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            ServicePacks oServicePacks = new ServicePacks(0, dsn);
            Users oUser = new Users(0, dsn);
            Organizations oOrganization = new Organizations(0, dsn);
            IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
            Domains oDomains = new Domains(0, dsn);
            Variables oVariable = new Variables(intEnvironment);
            OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
            Mnemonic oMnemonic = new Mnemonic(0, dsn);
            
            StringBuilder sbTable = new StringBuilder();
            StringBuilder sbTable2 = new StringBuilder();
            DataSet ds = oServer.Get(_serverid);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                bool boolSAN = (oForecast.GetAnswer(intAnswer, "storage") == "1");
                int intName = Int32.Parse(ds.Tables[0].Rows[0]["nameid"].ToString());
                int intOS = Int32.Parse(ds.Tables[0].Rows[0]["osid"].ToString());
                int intCluster = Int32.Parse(ds.Tables[0].Rows[0]["clusterid"].ToString());
                int intCSM = Int32.Parse(ds.Tables[0].Rows[0]["csmconfigid"].ToString());
                int intDBA = Int32.Parse(ds.Tables[0].Rows[0]["dba"].ToString());
                bool boolPNC = (ds.Tables[0].Rows[0]["pnc"].ToString() == "1");
                string strName = oServer.GetName(_serverid, _use_pnc_naming);
                int intForecast = 0;
                int intRequest = 0;
                int intProject = 0;
                if (oForecast.GetAnswer(intAnswer, "forecastid") != "" && oForecast.GetAnswer(intAnswer, "forecastid") != "0")
                {
                    intForecast = Int32.Parse(oForecast.GetAnswer(intAnswer, "forecastid"));
                    intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                    intProject = oRequest.GetProjectNumber(intRequest);
                }
                int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
                int intEnv = Int32.Parse(oForecast.GetAnswer(intAnswer, "environmentid"));
                int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());

                string _save_location = oVariables.UploadsFolder() + "SC\\";
                string strFileName = "SC_" + _serverid.ToString() + "_" + oProject.Get(intProject, "number") + ".HTM";
                if (Directory.Exists(_save_location) == false)
                    Directory.CreateDirectory(_save_location);

                string strFile = _save_location + strFileName;
                if (File.Exists(strFile) == false)
                {
                    StreamWriter fp = File.CreateText(strFile);

                    string strDowntime = "Not Applicable";
                    if (oClass.IsProd(intClass))
                    {
                        if (oForecast.IsDRUnder48(intAnswer, false) == true)
                            strDowntime = "0 - 48 Hours (Cincinnati)";
                        else if (oForecast.IsDROver48(intAnswer, false) == true)
                            strDowntime = "48 Hours or More (Sunguard)";
                    }

                    string strDefault = "<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" class=\"default\">";

                    StringBuilder sbLeft1 = new StringBuilder();
                    string strLead = oProject.Get(intProject, "lead");
                    int intLead = 0;
                    if (strLead != "")
                        intLead = Int32.Parse(strLead);
                    sbLeft1.Append("<fieldset>");
                    sbLeft1.Append("<legend><b>Who is this for?</b></legend>");
                    sbLeft1.Append(strDefault);
                    sbLeft1.Append("<tr>");
                    sbLeft1.Append("<td>Client XID:</td>");
                    sbLeft1.Append("<td>");
                    sbLeft1.Append(GetBox(intLead > 0 ? oUser.GetName(intLead) : "***ERROR**", 200));
                    sbLeft1.Append("</td>");
                    sbLeft1.Append("</tr>");
                    sbLeft1.Append("<tr>");
                    sbLeft1.Append("<td>Client Name:</td>");
                    sbLeft1.Append("<td>");
                    sbLeft1.Append(GetBox(intLead > 0 ? oUser.GetFullName(intLead) : "***ERROR**", 200));
                    sbLeft1.Append("</td>");
                    sbLeft1.Append("</tr>");
                    sbLeft1.Append("<tr>");
                    sbLeft1.Append("<td>Client Phone No.:</td>");
                    sbLeft1.Append("<td>");
                    sbLeft1.Append(GetBox(intLead > 0 ? oUser.Get(intLead, "phone") : "***ERROR**", 200));
                    sbLeft1.Append("</td>");
                    sbLeft1.Append("</tr>");
                    sbLeft1.Append("<tr>");
                    sbLeft1.Append("<td>Client Department:</td>");
                    sbLeft1.Append("<td>");
                    sbLeft1.Append(GetBox("", 200));
                    sbLeft1.Append("</td>");
                    sbLeft1.Append("</tr>");
                    sbLeft1.Append("<tr>");
                    sbLeft1.Append("<td>Client Fax:</td>");
                    sbLeft1.Append("<td>");
                    sbLeft1.Append(GetBox("", 200));
                    sbLeft1.Append("</td>");
                    sbLeft1.Append("</tr>");
                    sbLeft1.Append("<tr>");
                    sbLeft1.Append("<td>Client Cost Center:</td>");
                    sbLeft1.Append("<td>");
                    sbLeft1.Append(GetBox("", 200));
                    sbLeft1.Append("</td>");
                    sbLeft1.Append("</tr>");
                    sbLeft1.Append("</table>");
                    sbLeft1.Append("</fieldset>");

                    StringBuilder sbRight1 = new StringBuilder();
                    string strIE = oProject.Get(intProject, "engineer");
                    int intIE = 0;
                    if (strIE != "")
                        intIE = Int32.Parse(strIE);
                    sbRight1.Append("<fieldset>");
                    sbRight1.Append("<legend><b>Requestor Information</b></legend>");
                    sbRight1.Append(strDefault);
                    sbRight1.Append("<tr>");
                    sbRight1.Append("<td>Requested By Name:</td>");
                    sbRight1.Append("<td>");
                    sbRight1.Append(GetBox(intIE > 0 ? oUser.GetFullName(intIE) : "***ERROR**", 200));
                    sbRight1.Append("</td>");
                    sbRight1.Append("</tr>");
                    sbRight1.Append("<tr>");
                    sbRight1.Append("<td>Requested By Phone No.:</td>");
                    sbRight1.Append("<td>");
                    sbRight1.Append(GetBox(intIE > 0 ? oUser.Get(intIE, "phone") : "***ERROR**", 200));
                    sbRight1.Append("</td>");
                    sbRight1.Append("</tr>");
                    sbRight1.Append("<tr>");
                    sbRight1.Append("<td>Requested By Email:</td>");
                    sbRight1.Append("<td>");
                    sbRight1.Append(GetBox(intIE > 0 ? oUser.GetEmail(oUser.GetName(intIE), intEnvironment) : "***ERROR**", 200));
                    sbRight1.Append("</td>");
                    sbRight1.Append("</tr>");
                    sbRight1.Append("</table>");
                    sbRight1.Append("</fieldset>");
                    sbRight1.Append("<fieldset>");
                    sbRight1.Append("<legend><b>Location Information:</b></legend>");
                    sbRight1.Append(strDefault);
                    sbRight1.Append("<tr>");
                    sbRight1.Append("<td>Location:</td>");
                    sbRight1.Append("<td>");
                    sbRight1.Append(GetBox("", 200));
                    sbRight1.Append("</td>");
                    sbRight1.Append("</tr>");
                    sbRight1.Append("<tr>");
                    sbRight1.Append("<td>Location Full Name:</td>");
                    sbRight1.Append("<td>");
                    sbRight1.Append(GetBox("", 200));
                    sbRight1.Append("</td>");
                    sbRight1.Append("</tr>");
                    sbRight1.Append("</table>");
                    sbRight1.Append("</fieldset>");

                    StringBuilder sbCenter1 = new StringBuilder();
                    sbCenter1.Append("<fieldset>");
                    sbCenter1.Append("<legend><b>Brief Desc:</b></legend>");
                    sbCenter1.Append(strDefault);
                    sbCenter1.Append("<tr>");
                    sbCenter1.Append("<td>");
                    sbCenter1.Append(GetBox(oForecast.GetAnswer(intAnswer, "name") + " (" + strName + ")", 600));
                    sbCenter1.Append("</td>");
                    sbCenter1.Append("</tr>");
                    sbCenter1.Append("</table>");
                    sbCenter1.Append("</fieldset>");

                    StringBuilder sbLeft2 = new StringBuilder();
                    sbLeft2.Append("<fieldset>");
                    sbLeft2.Append("<legend><b>Project Information:</b></legend>");
                    sbLeft2.Append(strDefault);
                    sbLeft2.Append("<tr>");
                    sbLeft2.Append("<td>Project ID:</td>");
                    sbLeft2.Append("<td>");
                    sbLeft2.Append(GetBox(oProject.Get(intProject, "number"), 200));
                    sbLeft2.Append("</td>");
                    sbLeft2.Append("<td>Project Manager XID:</td>");
                    sbLeft2.Append("<td>");
                    sbLeft2.Append(GetBox(intLead > 0 ? oUser.GetName(intLead) : "***ERROR**", 200));
                    sbLeft2.Append("</td>");
                    sbLeft2.Append("</tr>");
                    sbLeft2.Append("<tr>");
                    sbLeft2.Append("<td>Project Name:</td>");
                    sbLeft2.Append("<td>");
                    sbLeft2.Append(GetBox(oProject.Get(intProject, "name"), 200));
                    sbLeft2.Append("</td>");
                    sbLeft2.Append("<td>Project Manager Name:</td>");
                    sbLeft2.Append("<td>");
                    sbLeft2.Append(GetBox(intLead > 0 ? oUser.GetFullName(intLead) : "***ERROR**", 200));
                    sbLeft2.Append("</td>");
                    sbLeft2.Append("</tr>");
                    sbLeft2.Append("<tr>");
                    sbLeft2.Append("<td>Project Budgeted:</td>");
                    sbLeft2.Append("<td>");
                    sbLeft2.Append(GetBox(oProject.Get(intProject, "bd"), 200));
                    sbLeft2.Append("</td>");
                    sbLeft2.Append("<td>Project Manager Phone:</td>");
                    sbLeft2.Append("<td>");
                    sbLeft2.Append(GetBox(intLead > 0 ? oUser.Get(intLead, "phone") : "***ERROR**", 200));
                    sbLeft2.Append("</td>");
                    sbLeft2.Append("</tr>");
                    sbLeft2.Append("<tr>");
                    sbLeft2.Append("<td>Project Cost Ctr:</td>");
                    sbLeft2.Append("<td>");
                    sbLeft2.Append(GetBox("", 200));
                    sbLeft2.Append("</td>");
                    sbLeft2.Append("<td>Project Manager Email:</td>");
                    sbLeft2.Append("<td>");
                    sbLeft2.Append(GetBox(intLead > 0 ? oUser.GetEmail(oUser.GetName(intLead), intEnvironment) : "***ERROR**", 200));
                    sbLeft2.Append("</td>");
                    sbLeft2.Append("</tr>");
                    sbLeft2.Append("</table>");
                    sbLeft2.Append("</fieldset>");

                    int intImplementor = -999;
                    sbTable.Append("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" class=\"default\">");
                    sbTable.Append("<tr>");
                    sbTable.Append("<td colspan=\"2\"><b>NOTE:</b> ASSIGN THIS REQUEST TO: ");
                    sbTable.Append(intImplementor > 0 || intImplementor == -999 ? oUser.GetFullName(intImplementor) + " (" + oUser.GetName(intImplementor, true) + ")" : "***ERROR**");
                    sbTable.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(VERSION: 2.0)</td>");
                    sbTable.Append("</tr>");
                    sbTable.Append("<tr>");
                    sbTable.Append("<td>");
                    sbTable.Append(sbLeft1.ToString());
                    sbTable.Append("</td>");
                    sbTable.Append("<td>");
                    sbTable.Append(sbRight1.ToString());
                    sbTable.Append("</td>");
                    sbTable.Append("</tr>");
                    sbTable.Append("<tr>");
                    sbTable.Append("<td colspan=\"2\">");
                    sbTable.Append(sbCenter1.ToString());
                    sbTable.Append("</td>");
                    sbTable.Append("</tr>");
                    sbTable.Append("<tr>");
                    sbTable.Append("<td colspan=\"2\">");
                    sbTable.Append(sbLeft2.ToString());
                    sbTable.Append("</td>");
                    sbTable.Append("</tr>");
                    sbTable.Append("<tr>");
                    sbTable.Append("<td colspan=\"2\"><p>&nbsp;</p></td>");
                    sbTable.Append("</tr>");

                    bool boolVirtual = (oModelsProperties.IsTypeVMware(intModel) == true && oOperatingSystem.IsAix(intOS) == false);
                    if (boolVirtual == true)
                    {
                        sbTable.Append("<tr>");
                        sbTable.Append("<td colspan=\"2\" class=\"header\">REQUISITION VIRTUAL SERVER GUEST</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Requested Server Completion Date:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oForecast.GetAnswer(intAnswer, "implementation"), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Server Hardware Specifications:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("VIRTUAL SERVER GUEST VMWARE", 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Server Operating System:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oOperatingSystem.Get(intOS, "name").ToUpper() + " (" + oServicePacks.Get(Int32.Parse(ds.Tables[0].Rows[0]["spid"].ToString()), "name").ToUpper() + ")", 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Environment:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oEnvironment.Get(Int32.Parse(oForecast.GetAnswer(intAnswer, "environmentid")), "name").ToUpper(), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Destination Class:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oClass.Get(Int32.Parse(oForecast.GetAnswer(intAnswer, "classid")), "name").ToUpper(), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr><td colspan=\"2\"><p>&nbsp;</p></td></tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Destination Domain:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oDomains.Get(Int32.Parse(ds.Tables[0].Rows[0]["domainid"].ToString()), "name").ToUpper(), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Backup Method:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("TSM", 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        string strHost = "***ERROR***";
                        DataSet dsGuest = oVMWare.GetGuest(strName);
                        if (dsGuest.Tables[0].Rows.Count > 0)
                            strHost = oVMWare.GetHost(Int32.Parse(dsGuest.Tables[0].Rows[0]["hostid"].ToString()), "name").ToUpper();
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Host Server Name:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(strHost, 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr><td colspan=\"2\"><p>&nbsp;</p></td></tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td bgcolor=\"#CCCCCC\" style=\"border:2px outset #FFFFFF\">Network Protocols</td>");
                        sbTable.Append("<td bgcolor=\"#CCCCCC\" style=\"border:2px outset #FFFFFF\">Server Software - click in box below to add</td>");
                        sbTable.Append("</tr>");
                        bool boolSQL = false;
                        bool boolIIS = false;
                        DataSet dsComponents = oServerName.GetComponentDetailSelected(_serverid, 1);
                        foreach (DataRow drComp in dsComponents.Tables[0].Rows)
                        {
                            if (drComp["code"].ToString() == "IIS")
                                boolIIS = true;
                            if (drComp["code"].ToString() == "SQL")
                                boolSQL = true;
                        }
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("TCP/IP", 250));
                        sbTable.Append("</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox((boolSQL ? "SQL [DBA: " + oUser.GetFullName(intDBA) + " (" + oUser.GetName(intDBA) + ")]" : ""), 350));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("", 250));
                        sbTable.Append("</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox((boolIIS ? "IIS" : ""), 350));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr><td colspan=\"2\"><p>&nbsp;</p></td></tr>");
                    }
                    else
                    {
                        sbTable.Append("<tr>");
                        sbTable.Append("<td colspan=\"2\" class=\"header\">REQUISITION SERVER</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Requested Server Completion Date:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oForecast.GetAnswer(intAnswer, "implementation"), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Server Hardware Specifications:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oModelsProperties.Get(intModel, "name"), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td colspan=\"2\"><input type=\"checkbox\" class=\"default\" checked/> User re-deployable hardware if applicable</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>PO:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("", 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Server Operating System:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oOperatingSystem.Get(Int32.Parse(ds.Tables[0].Rows[0]["osid"].ToString()), "name").ToUpper() + " (" + oServicePacks.Get(Int32.Parse(ds.Tables[0].Rows[0]["spid"].ToString()), "name").ToUpper() + ")", 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Environment:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oEnvironment.Get(Int32.Parse(oForecast.GetAnswer(intAnswer, "environmentid")), "name").ToUpper(), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Destination Class:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oClass.Get(Int32.Parse(oForecast.GetAnswer(intAnswer, "classid")), "name").ToUpper(), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr><td colspan=\"2\"><p>&nbsp;</p></td></tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Destination Domain:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oDomains.Get(Int32.Parse(ds.Tables[0].Rows[0]["domainid"].ToString()), "name").ToUpper(), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Server Location:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox(oLocations.GetFull(Int32.Parse(oForecast.GetAnswer(intAnswer, "addressid"))).ToUpper(), 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Backup Method:</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("TSM", 300));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr><td colspan=\"2\"><p>&nbsp;</p></td></tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td bgcolor=\"#CCCCCC\" style=\"border:2px outset #FFFFFF\">Network Protocols</td>");
                        sbTable.Append("<td bgcolor=\"#CCCCCC\" style=\"border:2px outset #FFFFFF\">Server Software - click in box below to add</td>");
                        sbTable.Append("</tr>");
                        bool boolSQL = false;
                        bool boolIIS = false;
                        DataSet dsComponents = oServerName.GetComponentDetailSelected(_serverid, 1);
                        foreach (DataRow drComp in dsComponents.Tables[0].Rows)
                        {
                            if (drComp["code"].ToString() == "IIS")
                                boolIIS = true;
                            if (drComp["code"].ToString() == "SQL")
                                boolSQL = true;
                        }
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("TCP/IP", 250));
                        sbTable.Append("</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox((boolSQL ? "SQL [DBA: " + oUser.GetFullName(intDBA) + " (" + oUser.GetName(intDBA) + ")]" : ""), 350));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox("", 250));
                        sbTable.Append("</td>");
                        sbTable.Append("<td>");
                        sbTable.Append(GetBox((boolIIS ? "IIS" : ""), 350));
                        sbTable.Append("</td>");
                        sbTable.Append("</tr>");
                        sbTable.Append("<tr><td colspan=\"2\"><p>&nbsp;</p></td></tr>");
                    }
                    sbTable.Append("</table>");

                    sbTable2.Append("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" class=\"default\">");
                    sbTable2.Append("<tr>");
                    sbTable2.Append("<td>Attached to SAN?:</td>");
                    sbTable2.Append("<td>");
                    sbTable2.Append(GetBox((boolSAN == true ? "YES" : "NO"), 150));
                    sbTable2.Append("</td>");
                    sbTable2.Append("<td>Application Name:</td>");
                    sbTable2.Append("<td>");
                    sbTable2.Append(GetBox(oForecast.GetAnswer(intAnswer, "appname"), 300));
                    sbTable2.Append("</td>");
                    sbTable.Append("</tr>");
                    sbTable2.Append("<tr>");
                    sbTable2.Append("<td>Server is Clustered:</td>");
                    sbTable2.Append("<td>");
                    sbTable2.Append(GetBox((oForecast.IsHACluster(intAnswer) ? "YES" : "NO"), 150));
                    sbTable2.Append("</td>");
                    if (boolPNC == true)
                    {
                        sbTable2.Append("<td>Mnemonic:</td>");
                        int intMnemonic = Int32.Parse(oForecast.GetAnswer(intAnswer, "mnemonicid"));
                        sbTable2.Append("<td>");
                        sbTable2.Append(GetBox(oMnemonic.Get(intMnemonic, "factory_code"), 100));
                        sbTable2.Append("</td>");
                    }
                    else
                    {
                        sbTable2.Append("<td>Application Code:</td>");
                        sbTable2.Append("<td>");
                        sbTable2.Append(GetBox(oForecast.GetAnswer(intAnswer, "appcode"), 100));
                        sbTable2.Append("</td>");
                    }
                    sbTable2.Append("</tr>");
                    sbTable2.Append("<tr>");
                    int intNetworkEngineer = Int32.Parse(oForecast.GetAnswer(intAnswer, "networkengineer"));
                    sbTable2.Append("<td>Server Load Balanced:</td>");
                    sbTable2.Append("<td>");
                    sbTable2.Append(GetBox((oForecast.IsHACSM(intAnswer) ? "YES" : "NO"), 150));
                    sbTable2.Append("</td>");
                    sbTable2.Append("<td>DR Criticality:</td>");
                    sbTable2.Append("<td>");
                    sbTable2.Append(GetBox((oForecast.GetAnswer(intAnswer, "dr_criticality") == "1" ? "1 - High" : "2 - Low"), 100));
                    sbTable2.Append("</td>");
                    sbTable2.Append("</tr>");
                    sbTable2.Append("<tr>");
                    sbTable2.Append("<td>Network Engineer:</td>");
                    sbTable2.Append("<td>");
                    sbTable2.Append(GetBox((oForecast.IsHACSM(intAnswer) ? oUser.GetFullName(intNetworkEngineer) + " (" + oUser.GetName(intNetworkEngineer) + ")" : "N/A"), 150));
                    sbTable2.Append("</td>");
                    int intOwner = Int32.Parse(oForecast.GetAnswer(intAnswer, "appcontact"));
                    int intPrimary = Int32.Parse(oForecast.GetAnswer(intAnswer, "admin1"));
                    int intSecondary = Int32.Parse(oForecast.GetAnswer(intAnswer, "admin2"));
                    sbTable2.Append("<td>Departmental Manager:</td>");
                    sbTable2.Append("<td>");
                    sbTable2.Append(GetBox(oUser.GetFullName(intOwner) + " (" + oUser.GetName(intOwner) + ")", 300));
                    sbTable2.Append("</td>");
                    sbTable2.Append("</tr>");
                    sbTable2.Append("<tr>");
                    sbTable2.Append("<td>Maximum Allowable Downtime:</td>");
                    sbTable2.Append("<td>");
                    sbTable2.Append(GetBox(strDowntime, 150));
                    sbTable2.Append("</td>");
                    sbTable2.Append("<td>Application Technical Lead:</td>");
                    sbTable2.Append("<td>");
                    sbTable2.Append(GetBox(oUser.GetFullName(intPrimary) + " (" + oUser.GetName(intPrimary) + ")", 300));
                    sbTable2.Append("</td>");
                    sbTable2.Append("</tr>");
                    sbTable2.Append("<tr>");
                    sbTable2.Append("<td colspan=\"2\"><input type=\"checkbox\" class=\"default\"/> Hardware Refresh</td>");
                    sbTable2.Append("<td>Administrative Contact:</td>");
                    sbTable2.Append("<td>");
                    sbTable2.Append(GetBox(oUser.GetFullName(intSecondary) + " (" + oUser.GetName(intSecondary) + ")", 300));
                    sbTable2.Append("</td>");
                    sbTable2.Append("</tr>");
                    sbTable2.Append("<tr>");
                    sbTable2.Append("<td colspan=\"4\">Intended Use Description:</td>");
                    sbTable2.Append("</tr>");
                    sbTable2.Append("<tr>");
                    sbTable2.Append("<td colspan=\"4\">");
                    sbTable2.Append(GetBox("", 500));
                    sbTable2.Append("</td>");
                    sbTable2.Append("</tr>");
                    sbTable2.Append("</table>");

                    fp.WriteLine("<html>");
                    fp.WriteLine("<head>");
                    fp.WriteLine("<title>ClearView | Service Center Request Form</title>");
                    fp.WriteLine("<style type=\"text/css\">");
                    fp.WriteLine(".default {font-family: Verdana, Arial, Helvetica, sans-serif;font-size: 10px;}");
                    fp.WriteLine(".header {font-family: Verdana, Arial, Helvetica, sans-serif;font-size: 16px;font-style: italic;font-weight: bold;}");
                    fp.WriteLine("</style>");
                    fp.WriteLine("<body leftmargin=\"0\" topmargin=\"0\">");
                    fp.WriteLine(sbTable.ToString());
                    fp.WriteLine(sbTable2.ToString());
                    fp.WriteLine("</body>");
                    fp.WriteLine("</html>");
                    fp.Close();

                    Functions oFunction = new Functions(0, dsn, intEnvironment);
                    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_ALERT,EMAILGRP_SERVICE_CENTER");
                    if (intEnvironment == 1)
                        oFunction.SendEmail("Service Center Request [" + strName + "]", "ejjc335", "", strEMailIdsBCC, "Service Center Request [" + strName + "]", "<p><b>A new device needs to be added in Service Center.</b></p><p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p><p>Please reference the attached document to view all the information about this device.</p>", true, false, strFile);
                    else
                        oFunction.SendEmail("Service Center Request [" + strName + "]", "XKACQ32", "", strEMailIdsBCC, "Service Center Request [" + strName + "]", "<p><b>A new device needs to be added in Service Center.</b></p><p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p><p>Please reference the attached document to view all the information about this device.</p>", true, false, strFile);
                }
            }
        }

        public void CreateServerDecommSCRequest(int _RequestId, int _ItemId, int _Number, int _intAssignedTo)
        {
            Customized oCustomized = new Customized(0, dsn);
            Projects oProject = new Projects(0, dsn);
            Requests oRequest = new Requests(0, dsn);
            Servers oServers = new Servers(0, dsn);
            ModelsProperties oModelsProperties=new ModelsProperties(0,dsn);
            Variables oVariable=new Variables(intEnvironment);
            Users oUser = new Users(0, dsn);

            int intRequest = _RequestId;
            int intItem = _ItemId;
            int intNumber = _Number;
            int intServer = 0;
           
            int intProject=0;
            int intModel = 0;
            int intProfile = 0;
            int intReqAssignedTo=0;
            int intRequestor = 0;
            Int32.TryParse(oRequest.Get(intRequest, "userid"), out intRequestor);
            bool boolIsServerVMWare = false;

            string strName = "";
            string strServerName = "";
            string strDRServerName = "";
            string strPowerOffDate = "";
            string strReasonforDecom="";

          

            intProject = oRequest.GetProjectNumber(intRequest);
            DataSet dsProject=oProject.Get(intProject);

            //Get Decomm Server Details
            DataSet dsDecom = oCustomized.GetDecommissionServer(intRequest, intItem, intNumber);
            if (dsDecom.Tables[0].Rows.Count > 0)
            {
                intServer = Int32.Parse(dsDecom.Tables[0].Rows[0]["serverid"].ToString());
                strServerName = dsDecom.Tables[0].Rows[0]["servername"].ToString();
                strPowerOffDate=dsDecom.Tables[0].Rows[0]["poweroff"].ToString();
                strReasonforDecom=dsDecom.Tables[0].Rows[0]["reason"].ToString();
            }

            if (intServer > 0)
            {
                DataSet dsServer = oServers.Get(intServer);
                if (dsServer.Tables[0].Rows.Count > 0)
                {
                    if (dsServer.Tables[0].Rows[0]["modelid"] != DBNull.Value)
                        intModel = Int32.Parse(dsServer.Tables[0].Rows[0]["modelid"].ToString());
                    if (dsServer.Tables[0].Rows[0]["DR"].ToString() == "1")
                    {
                        if ((dsServer.Tables[0].Rows[0]["dr_exist"].ToString() == "1") && (dsServer.Tables[0].Rows[0]["DR_Name"].ToString() != ""))
                            strDRServerName = dsServer.Tables[0].Rows[0]["DR_Name"].ToString();
                        else
                            strDRServerName = strServerName + "-DR";
                    }

                    //If current project details is not valid then get project Id based on the Server Request Id
                    DataSet dsProjectVerify = oProject.Get(intProject);
                    if (dsProjectVerify.Tables[0].Rows.Count == 0)
                    {
                        if (dsServer.Tables[0].Rows[0]["requestid"] != DBNull.Value)
                        {
                            DataSet dsRequest = oRequest.Get(Int32.Parse(dsServer.Tables[0].Rows[0]["requestid"].ToString()));
                            if (dsRequest.Tables[0].Rows.Count > 0)
                                if (dsRequest.Tables[0].Rows[0]["projectid"] != DBNull.Value)
                                    intProject = Int32.Parse(dsRequest.Tables[0].Rows[0]["projectid"].ToString());
                        }
                    }
                    //End of get project Id
                }
               if (intModel > 0 && oModelsProperties.IsTypeVMware(intModel) == true)
                    boolIsServerVMWare = true;
            }

            string strLead = oProject.Get(intProject, "lead");
            int intLead = 0;
            if (strLead != "")
                intLead = Int32.Parse(strLead);

            //Get Assignement Details
            intReqAssignedTo = _intAssignedTo;

            
              
            

         
                     
            
            StringBuilder sbTable = new StringBuilder();
            StringBuilder sbTable2 = new StringBuilder();
            DataSet ds = oServers.Get(intServer);
          

            string _save_location = oVariables.UploadsFolder() + "SC\\";
            string strFileName = "SC_" + intServer.ToString() + "_" + intRequest.ToString() + "_" + intNumber.ToString() + ".HTM";
            if (Directory.Exists(_save_location) == false)
                Directory.CreateDirectory(_save_location);

            string strFile = _save_location + strFileName;
            if (File.Exists(strFile) == false)
            {
                StreamWriter fp = File.CreateText(strFile);

                # region "Note"
                sbTable.Append("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" width=\"95%\" class=\"default\">");
                sbTable.Append("<tr>");
                if (boolIsServerVMWare == false)
                {
                    sbTable.Append("<td colspan=\"2\"><b>NOTE:</b> ASSIGN THIS REQUEST TO : ");
                    sbTable.Append(intReqAssignedTo > 0 ? oUser.GetFullName(intReqAssignedTo) + " (" + oUser.GetName(intReqAssignedTo, true) + ")" : "***ERROR**");
                }
                else
                {
                    sbTable.Append("<td colspan=\"2\"><b>NOTE:</b> REQUEST COMPLETED AND CLOSED : ");
                    sbTable.Append(intReqAssignedTo > 0 ? oUser.GetFullName(intReqAssignedTo) + " (" + oUser.GetName(intReqAssignedTo, true) + ")" : "***ERROR**");

                }

                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("</table>");
                # endregion

                #region "Title"
                sbTable.Append("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" width=\"95%\" class=\"default\">");
                sbTable.Append("<tr>");
                sbTable.Append("<td class=\"header\" width=\"50%\"  >REQUEST MANAGEMENT SUMMARY </td>");
                sbTable.Append("<td class=\"header\" width=\"50%\" align=\"right\" >Requests for Internal IS Services </td>");
                sbTable.Append("</tr>");
                sbTable.Append("<tr>");
                sbTable.Append("<td>&nbsp;</td>");
                sbTable.Append("</tr>");
                sbTable.Append("</table>");
                # endregion

                sbTable.Append("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" width=\"95%\" class=\"default\">");

                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"100%\">");
                # region "Request Details"
                sbTable.Append("<fieldset>");
                sbTable.Append("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" width=\"100%\" class=\"default\">");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"25%\" >Request Number:</td>");
                sbTable.Append("<td width=\"25%\">");
                sbTable.Append(GetBox(intRequest > 0 ? "CVT" + intRequest.ToString() : "***ERROR**", 200));
                sbTable.Append("</td>");

                sbTable.Append("<td width=\"25%\">Status:</td>");
                sbTable.Append("<td width=\"25%\">");
                sbTable.Append(GetBox("initial", 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"25%\">Current Phase:</td>");
                sbTable.Append("<td width=\"25%\">");
                sbTable.Append(GetBox("In Process", 200));
                sbTable.Append("</td>");

                sbTable.Append("<td width=\"25%\">Approval Status:</td>");
                sbTable.Append("<td width=\"25%\">");
                sbTable.Append(GetBox("approved", 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"25%\">Brief Desc:</td>");
                sbTable.Append("<td width=\"75%\" colspan=\"3\">");
                sbTable.Append(GetBox("Decommission of Server " + strServerName.ToString(), 600));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("</table>");
                sbTable.Append("</fieldset>");

                # endregion
                sbTable.Append("</td></tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td>&nbsp;</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"100%\">");
                #region "Other Details"
                sbTable.Append("<fieldset>");
                sbTable.Append("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" width=\"100%\" class=\"default\">");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" valign=\"top\">");
                #region "Left"
                sbTable.Append("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" width=\"100%\" class=\"default\">");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >Client Name:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox("", 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >Client XID:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox("", 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >Requestor Phone No.:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox("", 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >Client Email.:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox("", 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >Requested By:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox(intRequestor > 0 ? oUser.GetFullName(intRequestor) : "***ERROR**", 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >Requested By Phone No.:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox(intRequestor > 0 ? oUser.Get(intRequestor, "phone") : "***ERROR**", 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >&nbsp;</td>");
                sbTable.Append("<td width=\"50%\" >&nbsp;</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >Location:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox("", 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >Location Full Name:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox("", 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >Locator:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox("", 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");

                sbTable.Append("</table>");
                # endregion
                sbTable.Append("</td>");
                sbTable.Append("<td width=\"50%\" valign=\"top\">");
                #region "Right"
                sbTable.Append("<fieldset>");
                sbTable.Append("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" width=\"100%\" class=\"default\">");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >Assignment Group:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox("INFRASTRUCTURE IMPLEMENTATION", 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >Assignee:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox("", 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("</table>");
                sbTable.Append("</fieldset>");
                # region "Blank Table "
                sbTable.Append("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" width=\"100%\" class=\"default\">");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >&nbsp;</td>");
                sbTable.Append("<td width=\"50%\" >&nbsp;</td>");
                sbTable.Append("</tr>");
                sbTable.Append("</table>");
                # endregion
                sbTable.Append("<fieldset>");
                sbTable.Append("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" width=\"100%\" class=\"default\">");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >Project ID:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox(oProject.Get(intProject, "number"), 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >Project Name:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox(oProject.Get(intProject, "name"), 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >Project Budgeted:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox(oProject.Get(intProject, "bd"), 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("</table>");
                sbTable.Append("</fieldset>");

                # region "Blank Table "
                sbTable.Append("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" width=\"100%\" class=\"default\">");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >&nbsp;</td>");
                sbTable.Append("<td width=\"50%\" >&nbsp;</td>");
                sbTable.Append("</tr>");
                sbTable.Append("</table>");
                # endregion

                sbTable.Append("<fieldset>");
                sbTable.Append("<legend><b>Manager/Project Manager:</b></legend>");
                sbTable.Append("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" width=\"100%\" class=\"default\">");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >XID:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox(intLead > 0 ? oUser.GetName(intLead) : "***ERROR**", 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >Name:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox(intLead > 0 ? oUser.GetFullName(intLead) : "***ERROR**", 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >Phone:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox(intLead > 0 ? oUser.Get(intLead, "phone") : "***ERROR**", 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"50%\" >Email:</td>");
                sbTable.Append("<td width=\"50%\">");
                sbTable.Append(GetBox(intLead > 0 ? oUser.GetEmail(oUser.GetName(intLead), intEnvironment) : "***ERROR**", 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("</table>");
                sbTable.Append("</fieldset>");
                # endregion
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("</table>");
                sbTable.Append("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" width=\"100%\" class=\"default\">");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"25%\"  >Description: </td>");
                sbTable.Append("<td width=\"75%\">");
                sbTable.Append(GetBox(strReasonforDecom, 600));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("</table>");
                sbTable.Append("</fieldset>");
                #endregion
                sbTable.Append("</td></tr>");
                sbTable.Append("<tr>");
                sbTable.Append("<td>&nbsp;</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"100%\">");
                # region "Decommission Server Details"
                sbTable.Append("<tr>");
                sbTable.Append("<td class=\"header\" width=\"100%\"  >DECOMMISSION SERVER </td>");
                sbTable.Append("</tr>");
                sbTable.Append("<tr>");
                sbTable.Append("<td>&nbsp;</td>");
                sbTable.Append("</tr>");
                sbTable.Append("</td></tr>");
                sbTable.Append("<tr>");
                sbTable.Append("<td>&nbsp;</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td>");
                sbTable.Append("<fieldset>");
                sbTable.Append("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" width=\"100%\" class=\"default\">");
                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"25%\" >Server Name:</td>");
                sbTable.Append("<td width=\"75%\">");
                sbTable.Append(GetBox(strServerName, 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"25%\" >DR Server Name:</td>");
                sbTable.Append("<td width=\"75%\">");
                sbTable.Append(GetBox(strDRServerName, 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"25%\" >Requested Power Off Date:</td>");
                sbTable.Append("<td width=\"75%\">");
                sbTable.Append(GetBox(strPowerOffDate, 200));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                sbTable.Append("<td width=\"25%\" >Reason for Decommission:</td>");
                sbTable.Append("<td width=\"75%\">");
                sbTable.Append(GetBox(strReasonforDecom, 600));
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("</table>");
                sbTable.Append("</fieldset>");
                sbTable.Append("</td></tr>");

                #endregion
                sbTable.Append("</td>");
                sbTable.Append("</tr>");
                sbTable.Append("</table>");

                fp.WriteLine("<html>");
                fp.WriteLine("<head>");
                fp.WriteLine("<title>ClearView | Service Center Request Form</title>");
                fp.WriteLine("<style type=\"text/css\">");
                fp.WriteLine(".default {font-family: Verdana, Arial, Helvetica, sans-serif;font-size: 10px;}");
                fp.WriteLine(".header {font-family: Verdana, Arial, Helvetica, sans-serif;font-size: 16px;font-style: italic;font-weight: bold;}");
                fp.WriteLine("</style>");
                fp.WriteLine("<body leftmargin=\"0\" topmargin=\"0\">");
                fp.WriteLine(sbTable.ToString());

                fp.WriteLine("</body>");
                fp.WriteLine("</html>");
                fp.Close();

                strName = "CVT" + intRequest.ToString();



                Functions oFunction = new Functions(0, dsn, intEnvironment);
                string strEMailIdsBCC = "";
                //string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_ALERT");
                string strEMailIdsServiceCenter = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_SERVICE_CENTER");

                oFunction.SendEmail("Service Center Request [" + strName + "]", strEMailIdsServiceCenter, oUser.GetName(_intAssignedTo), strEMailIdsBCC, "Service Center Request [" + strName + "]", "<p><b>A new device needs to be added in Service Center.</b></p><p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p><p>Please reference the attached document to view all the information about this device.</p>", true, false, strFile);
            }
        }
        

        private string GetBox(string _text, int _width)
        {
            return "<input type=\"text\" class=\"default\" style=\"width:" + _width.ToString() + "px\" value=\"" + _text + "\" />";
        }

        //public void PNC_DNS(int _answerid)
        //{
        //    string strBody = "";
        //    Variables oVariable = new Variables(intEnvironment);
        //    Functions oFunction = new Functions(0, dsn, intEnvironment);
        //    IPAddresses oIPAddress = new IPAddresses(0, dsnIP, dsn);
        //    Forecast oForecast = new Forecast(0, dsn);
        //    Servers oServer = new Servers(0, dsn);
        //    ServerName oServerName = new ServerName(0, dsn);
        //    Classes oClass = new Classes(0, dsn);
        //    Users oUser = new Users(0, dsn);
        //    int intUser = 0;
        //    string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");

        //    if (oForecast.GetAnswer(_answerid, "userid") != "")
        //        intUser = Int32.Parse(oForecast.GetAnswer(_answerid, "userid"));
        //    if (oForecast.GetAnswer(_answerid, "classid") != "")
        //    {
        //        int intClass = Int32.Parse(oForecast.GetAnswer(_answerid, "classid"));
        //        if (oClass.Get(intClass, "pnc") == "1")
        //        {
        //            DataSet dsServer = oServer.GetAnswer(_answerid);
        //            foreach (DataRow drServer in dsServer.Tables[0].Rows)
        //            {
        //                int intServer = Int32.Parse(drServer["id"].ToString());
        //                // Get IP
        //                string strIP = oServer.GetIPs(intServer, 0, 1, 0, 0, dsnIP, "", "");
        //                if (strIP != "")
        //                {
        //                    // Get Name
        //                    bool boolPNC = (drServer["pnc"].ToString() == "1");
        //                    int intName = Int32.Parse(drServer["nameid"].ToString());
        //                    string strName = "";
        //                    if (boolPNC == true)
        //                        strName = oServerName.GetNameFactory(intName, 0);
        //                    else
        //                        strName = oServerName.GetName(intName, 0);
        //                    if (strName != "")
        //                    {
        //                        //strBody += strName + "," + "pncbank.com" + "," + strIP + "," + "Server" + "," + oUser.GetFullName(intUser) + "," + "CV" + Environment.NewLine;
        //                        //strBody += strName + "," + "pncbank.com" + "," + strIP + "," + "Server" + "," + oUser.GetFullName(intUser) + "," + _answerid.ToString() + Environment.NewLine;
        //                        //strBody += strName + "," + "pncbank.com" + "," + strIP + "," + "Server" + "," + oUser.GetFullName(intUser) + Environment.NewLine;
        //                        strBody += strName + "," + "pncbank.com" + "," + strIP + "," + "Server" + Environment.NewLine;
        //                    }
        //                    else
        //                        oFunction.SendEmail("ClearView DNS Error (1)", strEMailIdsBCC, "", "", "ClearView DNS Error (1)", "<p>There was no <b>Name</b> for serverID: " + intServer.ToString() + " (IP Address: " + strIP + ")</p><p>Not good...no name assocated with the server???</p><p>There was no email generated to clearview_dns@pncbank.com for this server</p>", true, false);
        //                }
        //                else
        //                    oFunction.SendEmail("ClearView DNS Error (2)", strEMailIdsBCC, "", "", "ClearView DNS Error (2)", "<p>There was no <b>IP Address</b> for serverID: " + intServer.ToString() + "</p><p>This message is OK if there truly is no IP address associated with this ServerID</p><p>There was no email generated to clearview_dns@pncbank.com for this server</p>", true, false);
        //            }

        //            // Send Email
        //            if (strBody != "")
        //            {
        //                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
        //                oFunction.SendEmail("ClearView DNS Registration", oVariable.PNC(), "", strEMailIdsBCC, "ClearView DNS Registration: DesignID # " + _answerid.ToString(), strBody, false, true);
        //            }
        //            else
        //                oFunction.SendEmail("ClearView DNS Error (3)", strEMailIdsBCC, "", "", "ClearView DNS Error (3)", "<p>There are no <b>Servers</b> for answerID: " + _answerid.ToString() + "</p><p>This is not good...check to make sure there is at least one server for the answerid in cv_servers table</p><p>There was no email generated to clearview_dns@pncbank.com for this design</p>", true, false);
        //        }
        //        else
        //        {
        //            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
        //            oFunction.SendEmail("ClearView DNS Warning", strEMailIdsBCC, "", "", "ClearView DNS Warning", "<p>NOTE: DesignID " + _answerid.ToString() + " is not in a PNC class</p><p>Normally, this message is OK and requires no action. This is just an FYI...</p><p>There was no email generated to clearview_dns@pncbank.com for this design</p>", true, false);
        //        }
        //    }
        //}
    }
}