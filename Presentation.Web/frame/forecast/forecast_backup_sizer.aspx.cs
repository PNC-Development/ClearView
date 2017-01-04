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
    public partial class forecast_backup_sizer : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Forecast oForecast;
        protected int intID = 0;

        // Lookup Definitions

        //===============================================================================================
        // File Size Lookup    
        enum Nw_Lookup { LAN10 = 0, LANGig, WAN };
        enum File_Size_Lookup { Medium = 0, Large = 1, Small = 2 };
        protected double[,] Backup_Perf = new double[3, 3] { { 30.1, 29.1, 17.7 }, { 32.8, 34.5, 21.0 }, { 13.3, 13.9, 13.3 } };
        protected double[,] Restore_Perf = new double[3, 3] { { 21.2, 21.2, 16.0 }, { 23.7, 24.3, 15.9 }, { 10.5, 10.6, 10.5 } };
        //===================================================================================================


        // Variables below hold pre-defined default values. The values can be modified according to the user preference.

        //================================================================================================== 
        // Client File System Data (CFSD)
        protected int BaseClientCPUQty = 0;
        protected int ExchangeClientCPUQty = 0;
        protected int TotalFileSystemData = 0;
        protected double PercentageChangedDailyCFSD = 0.05;
        protected double CompressionRatioCFSD = 0.6;
        protected string strAvgFileSize = "Medium";

        protected double BackupVerRatio = 1.25;
        protected double ArchiveRatio = 0.25;
        protected string strBackupStartTimeCFSD = "Don't Care";
        protected int BackupWindowCFSD = 4;
        protected int BackupSets = 0;

        // Client Database Data (CDD)
        protected int TotalDbData = 0;
        protected string strDbType = "Exchange";
        protected double PercentageChangedDailyCDD = 1.00;
        protected double CompressionRatioCDD = 0.00;
        protected int BackupVersions = 38;
        protected string strBackupStartTimeCDD = "Don't Care";
        protected int BackupWindowCDD = 4;
        protected string strNetworkConn = "LAN 10/100";
        protected double GrowthFactor = 0.3;


        // TSM Server Information

        protected string strServerLoc = "Ops Center";
        protected string strTapeDriveType = "Magstar";
        protected int GBPerTapeCartridge = 90;
        protected int MigrationThroughput = 23;
        protected double MaxFileSizeDiskPool = 1.5;
        protected double ReclamationThreshold = 0.4;

        // Pricing Defaults

        protected int MagstarQty = 3;
        protected int TLSQty = 2;
        protected int FPQty = 6;
        protected int BaseClientQty = 3;
        protected int ExchangeClientQty = 1;
        protected int TCQty = 14;
        protected int DiskQty = 7;

        protected int UCFP = 1100;
        protected int UCTLS = 50;
        protected int UCBC = 406;
        protected int UCEC = 779;
        protected double UCDisk = 2.90;
        //==========================================================================================================

        // Variables below are used to hold formula results

        //==========================================================================================================
        protected int UCMagstar;
        protected int UCTC;

        protected double EPMagstar;
        protected double EPTLS;
        protected double EPFP;
        protected double EPBC;
        protected double EPEC;
        protected double EPTC;
        protected double EPUCDisk;

        protected double FullBackup;
        protected double IncrBU;
        protected double Dbdata;
        protected double ServerIBU;
        protected double ServerDD;

        protected double TSMServerDb;
        protected double TotalClientFiles;
        protected double FileSystemDiskPool;
        protected double DbDiskPool;
        protected double TotalDbGB;
        protected double TotalTSMSrvr;
        protected double OnSiteFileTapes;
        protected double OnSiteDbTapes;
        protected double BackupTapes;
        protected double LibrarySlots;
        protected double TotalDisk;
        protected double NightlyBackup;
        protected double FullRestore;
        protected double EBFSD;
        protected double EFRFSD;
        protected double EIBFSD;
        protected double EFBDD;
        protected double EFRDD;
        protected double EMD;
        protected double DbBak;
        protected double StoragePool;
        protected int Backup;
        protected double Restore;

        protected double backup_perf;
        protected double restore_perf;

        //========================================================================================================   


        protected void Page_Load(object sender, EventArgs e)
        {
            lblDefaultTFSD.Text = "0";
            lblDefaultPCD.Text = "5%";
            lblDefaultCR.Text = "60%";
            lblDefaultAFS.Text = "Medium";
            lblDefaultBVR.Text = "1.25";
            lblDefaultAR.Text = "0.25";
            lblDefaultBST.Text = "Don't Care";
            lblDefaultBW.Text = "4";
            lblDefaultBS.Text = "0";

            lblDefaultDbData.Text = "0";
            lblDefaultDbType.Text = "Exchange";
            lblDefaultPctChg.Text = "100%";
            lblDefaultCRDbData.Text = "0%";
            lblDefaultBackupVers.Text = "38";
            lblDefaultBSTDbData.Text = "Don't Care";
            lblDefaultBWDbData.Text = "4";
            lblDefaultNwConn.Text = "LAN 10/100";
            lblDefaultGF.Text = "30%";

            lblDefaultTSM.Text = "Ops Center";
            lblDefaultTDT.Text = "Magstar";
            lblDefaultGB.Text = "90";
            lblDefaultMT.Text = "23";
            lblDefaultMFS.Text = "1.5";
            lblDefaultRT.Text = "0.4";
            oForecast = new Forecast(0, dsn);
            if (Request.QueryString["id"] != null)
            {
                intID = Int32.Parse(Request.QueryString["id"]);
                int intForecast = Int32.Parse(oForecast.GetAnswer(intID, "forecastid"));
                int intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                Requests oRequest = new Requests(0, dsn);
                int intProject = oRequest.GetProjectNumber(intRequest);
                Projects oProject = new Projects(0, dsn);
                lblAppName.Text = oProject.Get(intProject, "name");
                lblPlanView.Text = oProject.Get(intProject, "number");
                Users oUser = new Users(0, dsn);
                string strLead = oProject.Get(intProject, "lead");
                if (strLead != "")
                    lblLead.Text = oUser.GetFullName(Int32.Parse(strLead));
                string strEngineer = oProject.Get(intProject, "engineer");
                if (strEngineer != "")
                    lblEngineer.Text = oUser.GetFullName(Int32.Parse(strEngineer));
                lblEstimate.Text = DateTime.Parse(oForecast.GetAnswer(intID, "implementation")).ToLongDateString();
                lblDataService.Text = DateTime.Parse(oForecast.GetAnswer(intID, "implementation")).ToLongDateString();
                if (oForecast.IsStorage(intID) == true)
                {
                    DataSet dsStorage = oForecast.GetStorage(intID);
                    double intHigh = double.Parse(dsStorage.Tables[0].Rows[0]["high_total"].ToString());
                    double intStandard = double.Parse(dsStorage.Tables[0].Rows[0]["standard_total"].ToString());
                    double intLow = double.Parse(dsStorage.Tables[0].Rows[0]["low_total"].ToString());
                    double intHighTest = double.Parse(dsStorage.Tables[0].Rows[0]["high_test"].ToString());
                    double intStandardTest = double.Parse(dsStorage.Tables[0].Rows[0]["standard_test"].ToString());
                    double intLowTest = double.Parse(dsStorage.Tables[0].Rows[0]["low_test"].ToString());
                    double dblTotal = intHigh + intStandard + intLow + intHighTest + intStandardTest + intLowTest;
                    TotalFileSystemData = Int32.Parse(dblTotal.ToString());
                }
                lblActualQBCC.Text = "3";
                lblActualQECC.Text = "1";
                lblActualTFSD.Text = TotalFileSystemData.ToString();
                DataSet dsBackup = oForecast.GetBackup(intID);
                if (dsBackup.Tables[0].Rows.Count > 0)
                {
                    // Actuals for Client File System Data
                    lblActualPCD.Text = dsBackup.Tables[0].Rows[0]["cf_percent"].ToString();
                    lblActualCR.Text = dsBackup.Tables[0].Rows[0]["cf_compression"].ToString();
                    lblActualAFS.Text = dsBackup.Tables[0].Rows[0]["cf_average"].ToString();
                    lblActualBVR.Text = dsBackup.Tables[0].Rows[0]["cf_backup"].ToString();
                    lblActualAR.Text = dsBackup.Tables[0].Rows[0]["cf_archive"].ToString();
                    if (dsBackup.Tables[0].Rows[0]["time"].ToString() == "1")
                    {
                        lblActualBST.Text = dsBackup.Tables[0].Rows[0]["time_hour"].ToString() + " " + dsBackup.Tables[0].Rows[0]["time_switch"].ToString();
                        lblActualBSTDbData.Text = dsBackup.Tables[0].Rows[0]["time_hour"].ToString() + " " + dsBackup.Tables[0].Rows[0]["time_switch"].ToString();
                    }
                    else
                    {
                        lblActualBST.Text = "Don't Care";
                        lblActualBSTDbData.Text = "Don't Care";
                    }
                    lblActualBW.Text = dsBackup.Tables[0].Rows[0]["cf_window"].ToString();
                    lblActualBS.Text = dsBackup.Tables[0].Rows[0]["cf_sets"].ToString();
                    // Actuals for Client Database Data
                    lblActualDbData.Text = TotalFileSystemData.ToString();
                    lblActualDbType.Text = dsBackup.Tables[0].Rows[0]["cd_type"].ToString();
                    lblActualPctChg.Text = dsBackup.Tables[0].Rows[0]["cd_percent"].ToString();
                    lblActualCRDbData.Text = dsBackup.Tables[0].Rows[0]["cd_compression"].ToString();
                    lblActualBackupVers.Text = dsBackup.Tables[0].Rows[0]["cd_versions"].ToString();
                    lblActualBWDbData.Text = dsBackup.Tables[0].Rows[0]["cd_window"].ToString();
                    lblActualNwConn.Text = "LAN 10/100";
                    lblActualGF.Text = dsBackup.Tables[0].Rows[0]["cd_growth"].ToString();
                }
                else
                {
                    // Actuals for Client File System Data
                    lblActualPCD.Text = "5%";
                    lblActualCR.Text = "32%";
                    lblActualAFS.Text = "Medium";
                    lblActualBVR.Text = "3.2";
                    lblActualAR.Text = "2.1";
                    lblActualBST.Text = "8:00PM";
                    lblActualBW.Text = "5";
                    lblActualBS.Text = "25";
                    // Actuals for Client Database Data
                    lblActualDbData.Text = "45";
                    lblActualDbType.Text = "DB2";
                    lblActualPctChg.Text = "23%";
                    lblActualCRDbData.Text = "16%";
                    lblActualBackupVers.Text = "38";
                    lblActualBSTDbData.Text = "10:00PM";
                    lblActualBWDbData.Text = "6";
                    lblActualNwConn.Text = "LAN 10/100";
                    lblActualGF.Text = "30%";
                }
            }
            else
            {
                lblActualQBCC.Text = "3";
                lblActualQECC.Text = "1";
                lblActualTFSD.Text = "96";
            }


            //========================================================================================

            // Actuals for TSM Server Info
            lblActualTSM.Text = "---";
            lblActualTDT.Text = "---";
            lblActualGB.Text = "---";
            lblActualMT.Text = "---";
            lblActualMFS.Text = "---";
            lblActualRT.Text = "---";


            // Pricing Override
            lblORMagstar.Text = "23";
            lblORTLS.Text = "10";
            lblORFP.Text = "5";
            lblORBaseClient.Text = "15";
            lblORExchangeClient.Text = "23";
            lblORTC.Text = "12";
            lblORDisk.Text = "2";
            //===============================================================================================


            // Section : Dynamic Assignment of values.NEEDS TO BE CHANGED.
            //=================================================================================================================================
            TotalFileSystemData = lblActualTFSD.Text == String.Empty ? Int32.Parse(lblDefaultTFSD.Text) : Int32.Parse(lblActualTFSD.Text);
            PercentageChangedDailyCFSD = lblActualPCD.Text == String.Empty ? Double.Parse(lblDefaultPCD.Text.Replace("%", "")) / 100 : Double.Parse(lblActualPCD.Text.Replace("%", "")) / 100;
            CompressionRatioCFSD = lblActualCR.Text == String.Empty ? Double.Parse(lblDefaultCR.Text.Replace("%", "")) / 100 : Double.Parse(lblActualCR.Text.Replace("%", "")) / 100;
            GrowthFactor = lblActualGF.Text == String.Empty ? Double.Parse(lblDefaultGF.Text.Replace("%", "")) / 100 : Double.Parse(lblActualGF.Text.Replace("%", "")) / 100;
            BackupVerRatio = lblActualBVR.Text == String.Empty ? Double.Parse(lblDefaultBVR.Text) : Double.Parse(lblActualBVR.Text);
            ArchiveRatio = lblActualAR.Text == String.Empty ? Double.Parse(lblDefaultAR.Text) : Double.Parse(lblActualAR.Text);
            strBackupStartTimeCFSD = lblActualBST.Text == String.Empty ? lblDefaultBST.Text : lblActualBST.Text;
            BackupWindowCFSD = lblActualBW.Text == String.Empty ? Int32.Parse(lblDefaultBW.Text) : Int32.Parse(lblActualBW.Text);
            BackupSets = lblActualBS.Text == String.Empty ? Int32.Parse(lblDefaultBS.Text) : Int32.Parse(lblActualBS.Text);
            strAvgFileSize = lblActualAFS.Text == String.Empty ? lblDefaultAFS.Text : lblActualAFS.Text;

            TotalDbData = lblActualDbData.Text == String.Empty ? Int32.Parse(lblDefaultDbData.Text) : Int32.Parse(lblActualDbData.Text);
            strDbType = lblActualDbType.Text == String.Empty ? lblDefaultDbType.Text : lblActualDbType.Text;
            PercentageChangedDailyCDD = lblActualPctChg.Text == String.Empty ? Double.Parse(lblDefaultPctChg.Text.Replace("%", "")) / 100 : Double.Parse(lblActualPctChg.Text.Replace("%", "")) / 100;
            CompressionRatioCDD = lblActualCRDbData.Text == String.Empty ? Double.Parse(lblDefaultCRDbData.Text.Replace("%", "")) / 100 : Double.Parse(lblActualCRDbData.Text.Replace("%", "")) / 100;
            BackupVersions = lblActualBackupVers.Text == String.Empty ? Int32.Parse(lblDefaultBackupVers.Text) : Int32.Parse(lblActualBackupVers.Text);
            strBackupStartTimeCDD = lblActualBSTDbData.Text == String.Empty ? lblDefaultBSTDbData.Text : lblActualBSTDbData.Text;
            BackupWindowCDD = lblActualBWDbData.Text == String.Empty ? Int32.Parse(lblDefaultBWDbData.Text) : Int32.Parse(lblActualBWDbData.Text);
            strNetworkConn = lblActualNwConn.Text == String.Empty ? lblDefaultNwConn.Text : lblActualNwConn.Text;

            strTapeDriveType = lblDefaultTDT.Text;
            GBPerTapeCartridge = Int32.Parse(lblDefaultGB.Text);
            MigrationThroughput = Int32.Parse(lblDefaultMT.Text);
            MaxFileSizeDiskPool = Double.Parse(lblDefaultMFS.Text);
            ReclamationThreshold = Double.Parse(lblDefaultRT.Text);


            // Pricing Extended Price (EP)

            EPMagstar = lblORMagstar.Text != String.Empty ? Double.Parse(lblORMagstar.Text) * UCMagstar : MagstarQty * UCMagstar;
            EPTLS = lblORTLS.Text != String.Empty ? Double.Parse(lblORTLS.Text) * UCTLS : TLSQty * UCTLS;
            EPFP = lblORFP.Text != String.Empty ? Double.Parse(lblORFP.Text) * UCFP : FPQty * UCFP;
            EPBC = lblORBaseClient.Text != String.Empty ? Double.Parse(lblORBaseClient.Text) * UCBC : BaseClientQty * UCBC;
            EPEC = lblORExchangeClient.Text != String.Empty ? Double.Parse(lblORExchangeClient.Text) * UCEC : ExchangeClientQty * UCEC;
            EPTC = lblORTC.Text != String.Empty ? Double.Parse(lblORTC.Text) * UCTC : TCQty * UCTC;
            EPUCDisk = lblORDisk.Text != String.Empty ? Double.Parse(lblORDisk.Text) * UCDisk : DiskQty * UCDisk;
            //===================================================================================================================================              


            // Section: DEFAULT VALUES
            //================================================================================================================================        
            // Set Defaults
            // TSM Client Information  
            lblDefaultQBCC.Text = BaseClientCPUQty.ToString();
            lblDefaultQECC.Text = ExchangeClientCPUQty.ToString();

            //Client File System Data
            // AVS  -  Avg File Size
            // AR   -  Archive Ratio
            // BVR  -  Backup Version Ratio
            // PCD  -  Percentage Changed Daily
            // TFSD -  Total File System Data
            // CR   -  Compression Ratio
            // BST  -  Backup Start Time
            // BW   -  Backup Window
            // BS   -  Backup Sets


            // TSM Server Information
            //  TDT - Tape Drive Type
            //  MT  - Migration Throughput
            //  MFS - Max File Size 
            //  RT  - Reclamation Threshold  



            // Pricing Quantity
            lblMagstarQty.Text = MagstarQty.ToString();
            lblTLSQty.Text = TLSQty.ToString();
            lblFPQty.Text = Convert.ToString(MagstarQty * 2);
            lblBaseClientQty.Text = BaseClientQty.ToString();
            lblExchangeClientQty.Text = ExchangeClientQty.ToString();
            lblTCQty.Text = TCQty.ToString();
            lblDiskQty.Text = DiskQty.ToString();



            // Pricing Unit Cost Lookup values for Magstar
            // UCTC - Unit Cost for Tape Cartridges
            // UCFP - Unit Cost for Fibre Ports
            // UCBC - Unit Cost for TSM Base client CPU licenses
            // UCEC - Unit Cost for TSM Exchange client CPU licenses 
            UCMagstar = strTapeDriveType.ToLower() == "magstar" ? 23000 : 18323;
            UCTC = strTapeDriveType.ToLower() == "magstar" ? 25 : 120;

            lblUCMagstar.Text = String.Format("{0:c}", UCMagstar);
            lblUCTLS.Text = String.Format("{0:c}", UCTC);
            lblUCFP.Text = String.Format("{0:c}", UCFP);
            lblUCBaseClient.Text = String.Format("{0:c}", UCBC);
            lblUCExchangeClient.Text = String.Format("{0:c}", UCEC);


            lblUCTC.Text = String.Format("{0:c}", UCTC);
            lblUCDisk.Text = String.Format("{0:c}", UCDisk);
            lblUCTLS.Text = String.Format("{0:c}", UCTLS);


            lblEPMagstar.Text = String.Format("{0:c}", EPMagstar);
            lblEPTLS.Text = String.Format("{0:c}", EPTLS);
            lblEPFP.Text = String.Format("{0:c}", EPFP);
            lblEPBaseClient.Text = String.Format("{0:c}", EPBC);
            lblEPExchangeClient.Text = String.Format("{0:c}", EPEC);
            lblEPTC.Text = String.Format("{0:c}", EPTC);

            lblEPTOP.Text = String.Format("{0:c}", EPMagstar + EPTLS + EPFP + EPBC + EPEC + EPTC);
            lblEPDisk.Text = String.Format("{0:c}", EPUCDisk);
            lblEPTMLE.Text = lblEPDisk.Text;
            //=============================================================================================================================================


            // Section: Formula Computations
            //======================================================================================================================================================
            TotalClientFiles = (TotalFileSystemData * 1000000 / NwLookup("", strAvgFileSize, ref backup_perf, ref restore_perf)) * (1 + GrowthFactor);
            TSMServerDb = Math.Round((TotalClientFiles / 1000) * BackupVerRatio * (1 + GrowthFactor));
            FileSystemDiskPool = (TotalFileSystemData * (1 - CompressionRatioCFSD) * PercentageChangedDailyCFSD) * 1.2 * (1 + GrowthFactor);
            DbDiskPool = (TotalDbData * PercentageChangedDailyCDD * (1 - CompressionRatioCDD) * (1 + GrowthFactor)) < MaxFileSizeDiskPool ? (TotalDbData * PercentageChangedDailyCDD * (1 - CompressionRatioCDD) * (1 + GrowthFactor)) : 0;
            TotalDbGB = ((TotalDbData + (TotalDbData * PercentageChangedDailyCDD * BackupVersions) * (1 - CompressionRatioCDD))) * (1 + GrowthFactor);
            OnSiteFileTapes = Math.Ceiling(((TotalFileSystemData * BackupVerRatio) / (GBPerTapeCartridge * ReclamationThreshold)) * (1 + GrowthFactor) * 1.2);
            OnSiteDbTapes = Math.Ceiling((TotalDbGB / (GBPerTapeCartridge * ReclamationThreshold) * 1.2));

            FullBackup = Math.Round(TotalFileSystemData * (1 - CompressionRatioCFSD) * (1 + GrowthFactor), 1);
            IncrBU = Math.Round(FullBackup * PercentageChangedDailyCFSD, 1);
            Dbdata = Math.Round(TotalDbData * (1 - CompressionRatioCDD) * PercentageChangedDailyCDD * (1 + GrowthFactor), 1);
            ServerIBU = Math.Round(TotalDbData * PercentageChangedDailyCDD * (1 - CompressionRatioCDD) * (1 + GrowthFactor) < MaxFileSizeDiskPool ? TotalDbData * PercentageChangedDailyCDD * (1 - CompressionRatioCDD) * (1 + GrowthFactor) : 0, 1);
            ServerDD = Math.Round(TotalDbData * PercentageChangedDailyCDD * (1 - CompressionRatioCDD) * (1 + GrowthFactor) > MaxFileSizeDiskPool ? TotalDbData * PercentageChangedDailyCDD * (1 - CompressionRatioCDD) * (1 + GrowthFactor) : 0, 1);
            TotalTSMSrvr = ((TotalFileSystemData * BackupVerRatio) + (TotalFileSystemData * ArchiveRatio)) * (1 + GrowthFactor);
            TotalDisk = Math.Round((TSMServerDb / 1000) + FileSystemDiskPool + DbDiskPool);
            BackupTapes = Math.Round(Math.Ceiling((TotalFileSystemData * (1 - CompressionRatioCFSD) / GBPerTapeCartridge)) * BackupSets * (1 + GrowthFactor) * 1.2);
            LibrarySlots = Math.Ceiling((OnSiteFileTapes + OnSiteDbTapes) * 1.2);
            NightlyBackup = ((TotalFileSystemData * PercentageChangedDailyCFSD * (1 - CompressionRatioCFSD)) + (TotalDbData * PercentageChangedDailyCDD * (1 - CompressionRatioCDD))) * (1 + GrowthFactor);
            FullRestore = ((TotalFileSystemData * (1 - CompressionRatioCFSD)) + (TotalDbData * (1 - CompressionRatioCDD))) * (1 + GrowthFactor);
            NwLookup(strNetworkConn, strAvgFileSize, ref backup_perf, ref restore_perf);
            EBFSD = Math.Round((TotalFileSystemData * (1 + GrowthFactor)) / backup_perf, 1);
            EFRFSD = Math.Round((TotalFileSystemData * (1 + GrowthFactor)) / restore_perf, 1);
            EIBFSD = Math.Round((TotalFileSystemData * PercentageChangedDailyCFSD * (1 + GrowthFactor)) / backup_perf, 1);

            dbLookup(strDbType, ref Backup, ref Restore);
            EFBDD = Math.Round((TotalDbData * (1 + GrowthFactor)) / Backup, 1);
            EFRDD = Math.Round((TotalDbData * (1 + GrowthFactor)) / Restore, 1);

            EMD = (ServerIBU / MigrationThroughput) * (1 + GrowthFactor);
            DbBak = Math.Round(TotalDbData / backup_perf * (1 + GrowthFactor));
            StoragePool = Math.Round((IncrBU / MigrationThroughput) * (1 + GrowthFactor), 1);
            //=======================================================================================================================================================================


            // Section: Assigning Formula Results to local variables
            //====================================================================================================================        

            lblFB.Text = FullBackup.ToString();
            lblIBU.Text = IncrBU.ToString();
            lbldbdata.Text = Dbdata.ToString();

            lblServerFB.Text = IncrBU.ToString();
            lblServerIBU.Text = ServerIBU.ToString();
            lblServerDD.Text = ServerDD.ToString();
            lblDiskFB.Text = Convert.ToString(IncrBU + ServerIBU);


            lblTotalClientFiles.Text = String.Format("{0:0,0}", TotalClientFiles);
            lblTotalTSMSrvrGB.Text = TotalTSMSrvr.ToString();
            lblTotalDbGB.Text = TotalDbGB.ToString();
            lblTSMServerDB.Text = String.Format("{0:0,0}", TSMServerDb);
            lblFileSystemDiskPool.Text = FileSystemDiskPool.ToString();
            lblDbDiskPool.Text = DbDiskPool.ToString();

            lblTotalDisk.Text = TotalDisk.ToString();
            lblOnsiteFileTapes.Text = OnSiteFileTapes.ToString();
            lblOnsiteDbTapes.Text = OnSiteDbTapes.ToString();
            lblOffsiteFileTapes.Text = OnSiteFileTapes.ToString();
            lblOffsiteDbTapes.Text = OnSiteDbTapes.ToString();
            lblBackupTapes.Text = BackupTapes.ToString();
            lblLibrarySlots.Text = LibrarySlots.ToString();

            lblNightlyBackup.Text = String.Format("{0:0.0}", NightlyBackup);
            lblFullRestore.Text = String.Format("{0:0.0}", FullRestore);

            lblEBTFSD.Text = backup_perf.ToString();
            lblERTFSD.Text = restore_perf.ToString();
            lblEFBFSD.Text = EBFSD.ToString();
            lblEFRFSD.Text = EFRFSD.ToString();
            lblEIBFSD.Text = EIBFSD.ToString();

            lblEBTDD.Text = Backup.ToString();
            lblERTDD.Text = Restore.ToString();
            lblEFBDD.Text = EFBDD.ToString();
            lblEFRDD.Text = EFRDD.ToString();

            lblEMD.Text = EMD.ToString();
            lblDbBak.Text = DbBak.ToString();
            lblStoragePool.Text = StoragePool.ToString();
            //===========================================================================================================================================
        }

        /*
         * ---------------------------------------------------------------------------------------------------------
         *  Function        : NwLookup
         *  Input  Args     : Network Conn,File Size
         *  Output Args     : Backup_Perf, Restore_Perf  
         *  Purpose         : Retrieve backup and restore performance lookup values from 2-D array for the specified
         *                    network connection and file size.
         * 
         * ---------------------------------------------------------------------------------------------------------
         */
        protected int NwLookup(string strNetwork, string strFileSize, ref double backup_perf, ref double restore_perf)
        {
            int fileSizeLookup = -1;
            int networkLookup = -1;
            int fileSize = -1;


            if (strNetwork != String.Empty)
            {
                switch (strNetwork.ToLower())
                {
                    case "lan 10/100": networkLookup = 0; break;
                    case "lan gige": networkLookup = 1; break;
                    default: networkLookup = 2; break;
                }
            }
            if (strFileSize != String.Empty)
            {
                switch (strFileSize.ToLower())
                {
                    case "medium": fileSizeLookup = 0; fileSize = 400; break;
                    case "large": fileSizeLookup = 1; fileSize = 4000; break;
                    default: fileSizeLookup = 2; fileSize = 40; break;
                }
            }

            if (fileSizeLookup != -1 && networkLookup != -1)
            {
                backup_perf = Backup_Perf[fileSizeLookup, networkLookup];
                restore_perf = Restore_Perf[fileSizeLookup, networkLookup];
            }
            return fileSize;

        }

        /*-----------------------------------------------------------------------------------------------------
         *      Function        : dbLookup
         *      Input  Args     : Dbname
         *      Output Args     : Backup and Restore lookup values  
         *      Purpose         : Retrieves backup and restore lookup values for the specified
         *                        database.
         * 
         * -----------------------------------------------------------------------------------------------------
        */
        private void dbLookup(string dbname, ref int Backup, ref double Restore)
        {
            if (dbname != String.Empty)
            {
                switch (dbname.ToLower())
                {
                    case "exchange": Backup = 30; Restore = 15.4; break;
                    default: Backup = 50; Restore = 25.6; break;
                }
            }
        }
    }
}
