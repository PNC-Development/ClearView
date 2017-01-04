using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace Presentation.Web.Services
{
    public class Other : BaseClass
    {
        public Other(string strWebMethodName)
            : base(strWebMethodName)
        {
        }
        [Serializable()]
        public class Project
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string Number { get; set; }
            public string Manager { get; set; }
            public string Description { get; set; }
            public int CostCenter { get; set; }
            public int CreatedBy { get; set; }
            public DateTime Created { get; set; }
            public int ModifiedBy { get; set; }
            public DateTime Modified { get; set; }
            public int Deleted { get; set; }
        }
        public bool InsertProject(string _ProjectName, string _ProjectNumber, string _ProjectManager, string _ProjectDesc, int _CostCenter)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            //if (boolCanWrite)
            //{
            //    Project oProject = new Project();
            //    oProject.Name = _ProjectName;
            //    oProject.Number = _ProjectNumber;
            //    oProject.Manager = _ProjectManager;
            //    oProject.Description = _ProjectDesc;
            //    oProject.CostCenter = _CostCenter;
            //    oProject.CreatedBy = 0;
            //    oProject.ModifiedBy = 0;

            //    using (cvServicesDal ocvServicesDal = new cvServicesDal(DataSource.ClearViewServicesDB))
            //    {
            //        return ocvServicesDal.InsertServiceProject(oProject);
            //    }
            //}
            //else
                return false;
        }

        public string GetMacFromILO(string _ilo, int _environment, bool _delete_files)
        {
            string strReturn = RunILOCommand(_ilo, "Get_Host_Data", _environment, _delete_files);
            string strStartString = "<FIELD NAME=\"MAC\" VALUE=\"";
            string strMAC = strReturn.Substring(strReturn.IndexOf(strStartString) + strStartString.Length);
            strMAC = strMAC.Substring(0, strMAC.IndexOf("\""));
            return strMAC;
        }

        private string RunILOCommand(string _ilo, string _xml_script, int _environment, bool _delete_files)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            oLog.WriteEntry(String.Format("\"" + strMethodName + "\" Starting..."), EventLogEntryType.Information);
            // C:\380s>CPQLOCFG.EXE -s 10.49.186.110 -l c:\380s\ilo.txt -f c:\380s\Get_Host_Data.xml -v -u iadmin -p qwertyui
            string strServer = "localhost";
            if (_environment == 1)
            {
                strAPIdir = @"C:\ClearViewAPI\";
                strServer = "DXP380MPB1";
            }
            string strILO = _ilo;
            while (strILO.Contains(".") == true)
                strILO = strILO.Replace(".", "_");
            string strOutput = strAPIdir + strILO + ".out";
            string strBatCommand = strAPIdir + "CPQLOCFG.EXE -s " + _ilo + " -l " + strOutput + " -f " + strAPIdir + _xml_script + ".xml -v -u " + oVariable.ILOUsername() + " -p " + oVariable.ILOPassword();

            string strBAT = strAPIdir + strILO + ".bat";
            string strBATOUT = strAPIdir + strILO + ".txt";
            base.LogIt(oOutput, "Starting registration script..." + strBAT);
            StreamWriter oWriter = new StreamWriter(strBAT);
            base.LogIt(oOutput, "  Contents of script: " + strBatCommand);
            oWriter.WriteLine(strBatCommand);
            oWriter.Flush();
            oWriter.Close();

            string strPSEXEC = strAPIdir + "psexec.exe";
            ProcessStartInfo _info = new ProcessStartInfo(strPSEXEC);
            _info.WorkingDirectory = strAPIdir;
            _info.Arguments = "\\\\" + strServer + " -u " + oVariable.Domain() + "\\" + oVariable.ADUser() + " -p " + oVariable.ADPassword() + " -e -i cmd.exe /c " + strBAT + " >" + strBATOUT + " 2>&1";
            base.LogIt(oOutput, "PSEXEC Script = " + strPSEXEC + " \\\\" + strServer + " -u " + oVariable.Domain() + "\\" + oVariable.ADUser() + " -p ******** -e -i cmd.exe /c " + strBAT + " >" + strBATOUT + " 2>&1");
            Process _proc = Process.Start(_info);
            _proc.WaitForExit();
            _proc.Close();

            base.LogIt(oOutput, "Completed script..." + strBAT);
            string strReturn = ReadOutput(strOutput, oOutput, true);
            base.LogIt(oOutput, strReturn);
            if (boolOutputSuccess == true && _delete_files == true)
            {
                base.LogIt(oOutput, "Deleting files...");
                foreach (string strFile in Directory.GetFiles(strAPIdir, strILO + "*.*"))
                {
                    File.Delete(strFile);
                    base.LogIt(oOutput, "Deleted file: " + strFile);
                }
            }
            return strReturn;
        }
    }
}