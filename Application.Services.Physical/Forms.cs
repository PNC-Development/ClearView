using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Diagnostics;
using NCC.ClearView.Application.Core;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace ClearViewAP_Physical
{
    class Forms
    {
        private string dsn = "";
        private string dsnIP = "";
        private string dsnAsset = "";
        private int intEnvironment = 0;
        private int intImplementorDistributed = 0;
        private int intImplementorMidrange = 0;
        private int intWorkstationPlatform = 0;
        private int intProduction = 0;
        private int intUnder48A = 0;
        private int intUnder48Q = 0;
        private bool boolAdd = false;
        private bool boolSend = false;
        private bool boolUsePNCNaming = false;
        private int intProject = 0;
        private Projects oProject;
        private EventLog oLog;
        private PDFs oPDF;


        public Forms(string _scripts_path, bool _use_pnc_naming, int _projectid)
        {
            oLog = new EventLog();
            oLog.Source = "ClearView";
            DataSet dsConfig = new DataSet();
            dsConfig.ReadXml(_scripts_path + "birth.xml");
            intEnvironment = Int32.Parse(dsConfig.Tables[0].Rows[0]["environment"].ToString());
            intProduction = Int32.Parse(dsConfig.Tables[0].Rows[0]["productionid"].ToString());
            string strDSN = dsConfig.Tables[0].Rows[0]["DSN"].ToString();
            string strDSNAsset = dsConfig.Tables[0].Rows[0]["AssetDSN"].ToString();
            string strDSNIP = dsConfig.Tables[0].Rows[0]["IpDSN"].ToString();
            dsn = dsConfig.Tables[0].Rows[0][strDSN].ToString();
            dsnAsset = dsConfig.Tables[0].Rows[0][strDSNAsset].ToString();
            dsnIP = dsConfig.Tables[0].Rows[0][strDSNIP].ToString();
            boolAdd = (dsConfig.Tables[0].Rows[0]["func_add"].ToString() == "1");
            boolSend = (dsConfig.Tables[0].Rows[0]["func_send"].ToString() == "1");
            boolUsePNCNaming = _use_pnc_naming;
            oProject = new Projects(0, dsn);
            intProject = _projectid;

            oPDF = new PDFs(dsn, dsnAsset, dsnIP, intEnvironment);
        }

        public void SendForm(int _answerid, bool _san, bool _tsm)
        {
            oPDF.CreateDocuments(_answerid, _san, _tsm, null, true, true, true, false, boolUsePNCNaming, false);
        }
        //public void ServiceCenter(int _serverid)
        //{
        //    if (oProject.IsTest(intProject) == false)
        //        oPDF.CreateSCRequest(_serverid, boolUsePNCNaming);
        //}
        //public void ServiceCenterDecom(int _requestid, int _itemid, int _number)
        //{
        //    oPDF.CreateServerDecommSCRequest(_requestid, _itemid, _number, 0);
        //}
        //public void SendDNS(int _answerid)
        //{
        //    oPDF.PNC_DNS(_answerid);
        //}
    }
}
