using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;
using System.Configuration;

namespace NCC.ClearView.Application.Core
{
    public class Storage
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        private string strBootVolumeBackground = "BootVolume";

        public Storage(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public int GetNextLun(int _answerid, int _clusterid, int _csmconfigid, int _number)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            arParams[2] = new SqlParameter("@csmconfigid", _csmconfigid);
            arParams[3] = new SqlParameter("@number", _number);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getStorageNextLun", arParams);
            if (o == null || o.ToString() == "" || Int32.Parse(o.ToString()) <= 0)
                return 1;
            else
                return (Int32.Parse(o.ToString()) + 1);
        }
        public int AddLun(int _answerid, int _instanceid, int _clusterid, int _csmconfigid, int _number, int _driveid, string _performance, string _path, double _size, double _size_qa, double _size_test, int _replicated, int _high_availability)
        {
            arParams = new SqlParameter[14];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@instanceid", _instanceid);
            arParams[2] = new SqlParameter("@clusterid", _clusterid);
            arParams[3] = new SqlParameter("@csmconfigid", _csmconfigid);
            arParams[4] = new SqlParameter("@number", _number);
            arParams[5] = new SqlParameter("@driveid", _driveid);
            arParams[6] = new SqlParameter("@performance", _performance);
            arParams[7] = new SqlParameter("@path", _path);
            arParams[8] = new SqlParameter("@size", _size);
            arParams[9] = new SqlParameter("@size_qa", _size_qa);
            arParams[10] = new SqlParameter("@size_test", _size_test);
            arParams[11] = new SqlParameter("@replicated", _replicated);
            arParams[12] = new SqlParameter("@high_availability", _high_availability);
            arParams[13] = new SqlParameter("@id", SqlDbType.Int);
            arParams[13].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addStorageLun", arParams);
            return Int32.Parse(arParams[13].Value.ToString());
        }
        public void UpdateLun(int _id, string _path, string _performance, double _size, double _size_qa, double _size_test, int _replicated, int _high_availability)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@path", _path);
            arParams[2] = new SqlParameter("@performance", _performance);
            arParams[3] = new SqlParameter("@size", _size);
            arParams[4] = new SqlParameter("@size_qa", _size_qa);
            arParams[5] = new SqlParameter("@size_test", _size_test);
            arParams[6] = new SqlParameter("@replicated", _replicated);
            arParams[7] = new SqlParameter("@high_availability", _high_availability);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateStorageLun", arParams);
        }
        public void UpdateLun(int _id, int _instanceid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@instanceid", _instanceid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateStorageLunInstance", arParams);
        }
        public void UpdateLunActual(int _id, string _serialno, double _actual_size, double _actual_size_qa, double _actual_size_test, int _actual_replicated, int _actual_high_availability)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@serialno", _serialno);
            arParams[2] = new SqlParameter("@actual_size", _actual_size);
            arParams[3] = new SqlParameter("@actual_size_qa", _actual_size_qa);
            arParams[4] = new SqlParameter("@actual_size_test", _actual_size_test);
            arParams[5] = new SqlParameter("@actual_replicated", _actual_replicated);
            arParams[6] = new SqlParameter("@actual_high_availability", _actual_high_availability);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateStorageLunActual", arParams);
        }
        public DataSet GetLunsClusterShared(int _answerid, int _clusterid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorageLunsClusterShared", arParams);
        }
        public DataSet GetLunsClusterNonShared(int _answerid, int _clusterid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorageLunsClusterNonShared", arParams);
        }
        public DataSet GetLuns(int _answerid, int _instanceid, int _clusterid, int _csmconfigid, int _number)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@instanceid", _instanceid);
            arParams[2] = new SqlParameter("@clusterid", _clusterid);
            arParams[3] = new SqlParameter("@csmconfigid", _csmconfigid);
            arParams[4] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorageLuns", arParams);
        }
        public DataSet GetLun(int _answerid, int _instanceid, int _clusterid, int _csmconfigid, int _number, int _driveid)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@instanceid", _instanceid);
            arParams[2] = new SqlParameter("@clusterid", _clusterid);
            arParams[3] = new SqlParameter("@csmconfigid", _csmconfigid);
            arParams[4] = new SqlParameter("@number", _number);
            arParams[5] = new SqlParameter("@driveid", _driveid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorageLun", arParams);
        }
        public DataSet GetLuns(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorageLunsAnswer", arParams);
        }
        public DataSet GetSharedMapping(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorageLunsClusterSharedMapping", arParams);
        }
        public bool IsSharedMappingDone(int _answerid)
        {
            DataSet ds = GetSharedMapping(_answerid);
            bool done = false;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (String.IsNullOrEmpty(dr["uuid"].ToString()) == false)
                {
                    done = true;
                    break;
                }
            }
            return done;
        }
        public DataSet GetLunsDistinct(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorageLunsAnswerDistinct", arParams);
        }
        public void DeleteLuns(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteStorageLunsAnswer", arParams);
            FixLuns(_answerid);
        }
        public void DeleteLuns(int _answerid, int _instanceid, int _clusterid, int _csmconfigid, int _number)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@instanceid", _instanceid);
            arParams[2] = new SqlParameter("@clusterid", _clusterid);
            arParams[3] = new SqlParameter("@csmconfigid", _csmconfigid);
            arParams[4] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteStorageLuns", arParams);
            FixLuns(_answerid);
        }
        public void DeleteLun(int _answerid, int _instanceid, int _clusterid, int _csmconfigid, int _number, int _driveid)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@instanceid", _instanceid);
            arParams[2] = new SqlParameter("@clusterid", _clusterid);
            arParams[3] = new SqlParameter("@csmconfigid", _csmconfigid);
            arParams[4] = new SqlParameter("@number", _number);
            arParams[5] = new SqlParameter("@driveid", _driveid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteStorageLun", arParams);
            FixLuns(_answerid);
        }

        public void DeleteLunByLunID(int _lunid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@lunid", _lunid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteStorageLunByLunId", arParams);
          
        }
        public void FixLuns(int _answerid)
        {
            // Update the DRIVEID of the rest of the LUNs
            DataSet ds = GetLunsDistinct(_answerid);
            int intOldDrive = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intDrive = Int32.Parse(dr["driveid"].ToString());
                if (intDrive > 0)
                {
                    if (intOldDrive != intDrive)
                    {
                        intOldDrive = intOldDrive + 1;
                        if (intOldDrive != intDrive)
                            UpdateLuns(_answerid, Int32.Parse(dr["instanceid"].ToString()), Int32.Parse(dr["clusterid"].ToString()), Int32.Parse(dr["csmconfigid"].ToString()), Int32.Parse(dr["number"].ToString()), intDrive, intOldDrive);
                    }
                }
            }
        }
        public void UpdateLuns(int _answerid, int _instanceid, int _clusterid, int _csmconfigid, int _number, int _olddriveid, int _newdriveid)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@instanceid", _instanceid);
            arParams[2] = new SqlParameter("@clusterid", _clusterid);
            arParams[3] = new SqlParameter("@csmconfigid", _csmconfigid);
            arParams[4] = new SqlParameter("@number", _number);
            arParams[5] = new SqlParameter("@olddriveid", _olddriveid);
            arParams[6] = new SqlParameter("@newdriveid", _newdriveid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateStorageLuns", arParams);
        }
        public int AddLun(int _answerid, int _instanceid, int _clusterid, int _csmconfigid, int _number, int _driveid, double _size, double _size_qa, double _size_test)
        {
            return AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, _driveid, "Standard", "", _size, _size_qa, _size_test, 0, 0);
        }
        public void AddLun(int _answerid, int _instanceid, int _clusterid, int _csmconfigid, int _number)
        {
            int intDrive = GetNextLun(_answerid, _clusterid, _csmconfigid, _number);
            AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "", 1.00, 1.00, 1.00, 0, 0);
        }
        public void AddLunSQL(int _answerid, int _instanceid, int _clusterid, int _csmconfigid, int _number, double _size, double _size_qa, double _size_test, double _non, double _non_qa, double _non_test)
        {
            int intDrive = GetNextLun(_answerid, _clusterid, _csmconfigid, _number);
            AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "", 1.00, 1.00, 1.00, 0, 0);
            double dblDB = Math.Ceiling(_size * .1);
            double dblDBQA = Math.Ceiling(_size_qa * .1);
            double dblDBTest = Math.Ceiling(_size_test * .1);
            AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "\\Production", Minimum(dblDB), Minimum(dblDBQA), Minimum(dblDBTest), 0, 0);
            dblDB = Math.Ceiling(_size * .2);
            dblDBQA = Math.Ceiling(_size_qa * .2);
            dblDBTest = Math.Ceiling(_size_test * .2);
            AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "\\Production\\Logs", Minimum(dblDB), Minimum(dblDBQA), Minimum(dblDBTest), 0, 0);
            if ((_size > 0.00 && _size <= 100.00) || (_size_qa > 0.00 && _size_qa <= 100.00) || (_size_test > 0.00 && _size_test <= 100.00))
            {
                // 0 GB - 100 GB
                AddLuns(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, Minimum(_size), Minimum(_size_qa), Minimum(_size_test), 1.00);
            }
            else if ((_size > 100.00 && _size <= 500.00) || (_size_qa > 100.00 && _size_qa <= 500.00) || (_size_test > 100.00 && _size_test <= 500.00))
            {
                // 100 GB - 500 GB
                AddLuns(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, Minimum(_size), Minimum(_size_qa), Minimum(_size_test), 2.00);
            }
            else if ((_size > 500.00 && _size <= 2000.00) || (_size_qa > 500.00 && _size_qa <= 2000.00) || (_size_test > 500.00 && _size_test <= 2000.00))
            {
                // 500 GB - 2000 GB
                AddLuns(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, Minimum(_size), Minimum(_size_qa), Minimum(_size_test), 4.00);
            }
            else
            {
                // Over 2000 GB
                int intStart = 0;
                while (_size > 0.00 || _size_qa > 0.00 || _size_test > 0.00)
                {
                    double dblSize = 750.00;
                    double dblQA = 750.00;
                    double dblTest = 750.00;
                    if ((_size - dblSize) <= 0.00)
                        dblSize = _size;
                    _size = _size - dblSize;
                    if ((_size_qa - dblQA) <= 0.00)
                        dblQA = _size_qa;
                    _size_qa = _size_qa - dblQA;
                    if ((_size_test - dblTest) <= 0.00)
                        dblTest = _size_test;
                    _size_test = _size_test - dblTest;
                    intStart++;
                    if (dblSize > 0.00 || dblQA > 0.00 || dblTest > 0.00)
                    {
                        string strBefore = "";
                        if (intStart.ToString().Length == 1)
                            strBefore = "0";
                        AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "\\Production\\Database\\SQL" + strBefore + intStart.ToString(), Minimum(dblSize), Minimum(dblQA), Minimum(dblTest), 0, 0);
                        AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "\\Production\\Backups\\SQL" + strBefore + intStart.ToString(), Minimum(dblSize), Minimum(dblQA), Minimum(dblTest), 0, 0);
                    }
                }
            }
            if (_non > 0.00 || _non_qa > 0.00 || _non_test > 0.00)
                AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "\\AppData", Minimum(_non), Minimum(_non_qa), Minimum(_non_test), 0, 0);
        }
        public void AddLunSQLPNC(int _answerid, int _instanceid, int _clusterid, int _csmconfigid, int _number, double _size, double _non, double _percent, double _tempDB, double _compressionPercentage, double _tempDBOverhead, bool _2008)
        {
            double dblLunMin = 500.00;
            double dblLunMax = 750.00;

            Forecast oForecast = new Forecast(0, dsn);
            Classes oClass = new Classes(0, dsn);
            int intClass = 0;
            _percent = (_percent / 100.00);
            //_compressionPercentage = (_compressionPercentage / 100.00);
            if (Int32.TryParse(oForecast.GetAnswer(_answerid, "classid"), out intClass) == true)
            {
                bool boolProd = (oClass.IsProd(intClass));
                bool boolQA = (oClass.IsQA(intClass));
                bool boolTest = (oClass.IsTestDev(intClass));
                double dblLUN = 0.00;

                double dblOverallSize = (_size * (1.00 + _percent + 0.05));
                double dblDividend = dblOverallSize / dblLunMax;
                dblDividend = Math.Ceiling(dblDividend);
                //Response.Write(dblDividend.ToString("0") + "<br/>");
                double dblEachLUN = (dblOverallSize / dblDividend);
                //Response.Write(dblEachLUN.ToString() + "<br/>");
                dblEachLUN = RoundUp(dblEachLUN);
                dblOverallSize = (dblEachLUN * dblDividend);
                //Response.Write(dblEachLUN.ToString() + " x " + dblDividend.ToString() + " = " + dblOverallSize.ToString() + "<br/>");


                int intDrive = GetNextLun(_answerid, _clusterid, _csmconfigid, _number);
                // R:
                dblLUN = 1.00;
                AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "", (boolProd ? dblLUN : 0.00), (boolQA ? dblLUN : 0.00), (boolTest ? dblLUN : 0.00), 0, 0);


                // R:\Production
                dblLUN = 10.00;
                AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "\\Production", (boolProd ? dblLUN : 0.00), (boolQA ? dblLUN : 0.00), (boolTest ? dblLUN : 0.00), 0, 0);


                if (_clusterid > 0)
                {
                    // R:\Production\OEM
                    dblLUN = 10.00;
                    AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "\\Production\\oracle_oem", (boolProd ? dblLUN : 0.00), (boolQA ? dblLUN : 0.00), (boolTest ? dblLUN : 0.00), 0, 0);
                }


                // R:\Production\Database\SQL01 ***
                dblLUN = dblOverallSize;
                double dblLUN_Database = Minimum(dblLUN);
                for (double dblStart = 1.00; dblStart <= dblDividend; dblStart += 1.00)
                {
                    string strBefore = "";
                    if (dblStart.ToString().Length == 1)
                        strBefore = "0";
                    AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "\\Production\\Database\\SQL" + strBefore + dblStart.ToString(), (boolProd ? Minimum(dblEachLUN) : 0.00), (boolQA ? Minimum(dblEachLUN) : 0.00), (boolTest ? Minimum(dblEachLUN) : 0.00), 0, 0);
                }


                // R:\Production\Logs
                dblLUN = dblOverallSize;
                if (dblLUN > 2000.00)
                    dblLUN = 2000.00;   // 2 TB is maximum for LOGS
                double dblLUN_Logs = Minimum(dblLUN);
                AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "\\Production\\Logs", (boolProd ? Minimum(dblLUN) : 0.00), (boolQA ? Minimum(dblLUN) : 0.00), (boolTest ? Minimum(dblLUN) : 0.00), 0, 0);


                // R:\Production\TempDB
                //dblLUN = (_tempDB * _tempDBOverhead);
                if (_tempDB == 0.00)
                {
                    // Calcualate tempDB dynamically
                    _tempDB = (_size * .02);                                // 2% of database size
                    _tempDB += (_size * _percent);                          // Plus x% of largest table
                }
                dblLUN = RoundUp(_tempDB + (_tempDB * _tempDBOverhead));    // Plus OverHead value
                AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "\\Production\\TempDB", (boolProd ? Minimum(dblLUN) : 0.00), (boolQA ? Minimum(dblLUN) : 0.00), (boolTest ? Minimum(dblLUN) : 0.00), 0, 0);


                // R:\Production\Backups\SQL01*
                double dblBackups = dblOverallSize;
                if (_2008 == true)
                    dblBackups = ((dblLUN_Logs + dblLUN_Database) * _compressionPercentage);
                else
                    dblBackups = (dblLUN_Logs + dblLUN_Database);
                if (dblBackups < dblLunMax)
                {
                    // Save backups to one single LUN
                    AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "\\Production\\Backups\\SQL01", (boolProd ? Minimum(dblBackups) : 0.00), (boolQA ? Minimum(dblBackups) : 0.00), (boolTest ? Minimum(dblBackups) : 0.00), 0, 0);
                }
                else
                {
                    if (dblBackups == dblLUN_Database)
                    {
                        // Same configuration as Database LUNs
                        for (double dblStart = 1.00; dblStart <= (dblDividend * 1.00); dblStart += 1.00)
                        {
                            string strBefore = "";
                            if (dblStart.ToString().Length == 1)
                                strBefore = "0";
                            AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "\\Production\\Backups\\SQL" + strBefore + dblStart.ToString(), (boolProd ? Minimum(dblEachLUN) : 0.00), (boolQA ? Minimum(dblEachLUN) : 0.00), (boolTest ? Minimum(dblEachLUN) : 0.00), 0, 0);
                        }
                    }
                    else if (dblBackups == (dblLUN_Database * 2.00))
                    {
                        // Double the configuration of Database LUNs
                        for (double dblStart = 1.00; dblStart <= (dblDividend * 2.00); dblStart += 1.00)
                        {
                            string strBefore = "";
                            if (dblStart.ToString().Length == 1)
                                strBefore = "0";
                            AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "\\Production\\Backups\\SQL" + strBefore + dblStart.ToString(), (boolProd ? Minimum(dblEachLUN) : 0.00), (boolQA ? Minimum(dblEachLUN) : 0.00), (boolTest ? Minimum(dblEachLUN) : 0.00), 0, 0);
                        }
                    }
                    else
                    {
                        // Neither of the above, split them out
                        double dblDividendB = dblBackups / dblLunMax;
                        dblDividendB = Math.Ceiling(dblDividendB);
                        double dblEachLUNBackup = (dblBackups / dblDividendB);
                        dblEachLUNBackup = RoundUp(dblEachLUNBackup);
                        dblBackups = (dblEachLUNBackup * dblDividendB);

                        for (double dblStart = 1.00; dblStart <= dblDividendB; dblStart += 1.00)
                        {
                            string strBefore = "";
                            if (dblStart.ToString().Length == 1)
                                strBefore = "0";
                            AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "\\Production\\Backups\\SQL" + strBefore + dblStart.ToString(), (boolProd ? Minimum(dblEachLUNBackup) : 0.00), (boolQA ? Minimum(dblEachLUNBackup) : 0.00), (boolTest ? Minimum(dblEachLUNBackup) : 0.00), 0, 0);
                        }
                    }
                }


                // R:\AppData
                if (_non > 0.00)
                    AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, intDrive, "Standard", "\\AppData", (boolProd ? Minimum(_non) : 0.00), (boolQA ? Minimum(_non) : 0.00), (boolTest ? Minimum(_non) : 0.00), 0, 0);
            }
        }
        private double RoundUp(double _size)
        {
            _size = Math.Ceiling(_size);
            while (_size % 5.00 != 0.00)
                _size += 1.00;
            return _size;
        }
        private double Minimum(double _size)
        {
            if (_size > 0.00 && _size < 10.00)
                _size = 10.00;
            return Math.Ceiling(_size);
        }
        private void AddLuns(int _answerid, int _instanceid, int _clusterid, int _csmconfigid, int _number, int _drive, double _size, double _size_qa, double _size_test, double dblTimes)
        {
            for (double ii = dblTimes; ii > 0.00; ii = ii - 1.00)
            {
                double dblProd = 0.00;
                double dblQA = 0.00;
                double dblTest = 0.00;
                if (_size > 0.00)
                {
                    dblProd = _size / ii;
                    dblProd = Math.Ceiling(dblProd);
                    _size = _size - dblProd;
                }
                if (_size_qa > 0.00)
                {
                    dblQA = _size_qa / ii;
                    dblQA = Math.Ceiling(dblQA);
                    _size_qa = _size_qa - dblQA;
                }
                if (_size_test > 0.00)
                {
                    dblTest = _size_test / ii;
                    dblTest = Math.Ceiling(dblTest);
                    _size_test = _size_test - dblTest;
                }
                AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, _drive, "Standard", "\\Production\\Database\\SQL0" + ii.ToString("0"), dblProd, dblQA, dblTest, 0, 0);
                AddLun(_answerid, _instanceid, _clusterid, _csmconfigid, _number, _drive, "Standard", "\\Production\\Backups\\SQL0" + ii.ToString("0"), dblProd, dblQA, dblTest, 0, 0);
            }
        }


        public void AddMountPoint(int _lunid, string _path, string _performance, double _size, double _size_qa, double _size_test, int _replicated, int _high_availability)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@lunid", _lunid);
            arParams[1] = new SqlParameter("@path", _path);
            arParams[2] = new SqlParameter("@performance", _performance);
            arParams[3] = new SqlParameter("@size", _size);
            arParams[4] = new SqlParameter("@size_qa", _size_qa);
            arParams[5] = new SqlParameter("@size_test", _size_test);
            arParams[6] = new SqlParameter("@replicated", _replicated);
            arParams[7] = new SqlParameter("@high_availability", _high_availability);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addStorageMountPoint", arParams);
        }
        public void UpdateMountPoint(int _id, string _path, string _performance, double _size, double _size_qa, double _size_test, int _replicated, int _high_availability)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@path", _path);
            arParams[2] = new SqlParameter("@performance", _performance);
            arParams[3] = new SqlParameter("@size", _size);
            arParams[4] = new SqlParameter("@size_qa", _size_qa);
            arParams[5] = new SqlParameter("@size_test", _size_test);
            arParams[6] = new SqlParameter("@replicated", _replicated);
            arParams[7] = new SqlParameter("@high_availability", _high_availability);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateStorageMountPoint", arParams);
        }
        public void UpdateMountPointActual(int _id, string _serialno,double _actual_size, double _actual_size_qa, double _actual_size_test, int _actual_replicated, int _actual_high_availability)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@serialno", _serialno);
            arParams[2] = new SqlParameter("@actual_size", _actual_size);
            arParams[3] = new SqlParameter("@actual_size_qa", _actual_size_qa);
            arParams[4] = new SqlParameter("@actual_size_test", _actual_size_test);
            arParams[5] = new SqlParameter("@actual_replicated", _actual_replicated);
            arParams[6] = new SqlParameter("@actual_high_availability", _actual_high_availability);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateStorageMountPointActual", arParams);
        }
        public void DeleteMountPoint(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteStorageMountPoint", arParams);
        }
        public DataSet GetMountPoints(int _lunid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@lunid", _lunid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorageMountPoints", arParams);
        }
        public int GetLun(int _answerid, int _instanceid, int _clusterid, int _csmconfigid, int _number)
        {
            int intLun = 0;
            DataSet ds = GetLuns(_answerid, _instanceid, _clusterid, _csmconfigid, _number);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intDrive = Int32.Parse(dr["driveid"].ToString());
                if (intDrive > 0 || intDrive == -100)
                {
                    intLun = Int32.Parse(dr["id"].ToString());
                    break;
                }
            }
            return intLun;
        }

        public void AddLunDisk(int _lunid, int _virtual_bus_number, int _virtual_unit_number, string _vmware_disk_uuid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@lunid", _lunid);
            arParams[1] = new SqlParameter("@virtual_bus_number", _virtual_bus_number);
            arParams[2] = new SqlParameter("@virtual_unit_number", _virtual_unit_number);
            arParams[3] = new SqlParameter("@vmware_disk_uuid", _vmware_disk_uuid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addStorageLunDisk", arParams);
        }
        public DataSet GetLunDisks(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorageLunDisks", arParams);
        }


        public DataSet GetStorageDW(string _Host)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@Host", _Host);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorageAmt", arParams);
        }


        public string GetBody(int _requestid, int _itemid, int _number, string _dsn_asset, string _dsn_ip, int _environment, bool _edit)
        {
            // Get Answerid
            OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
            DataSet dsTask = oOnDemandTasks.GetServerStorage(_requestid, _itemid, _number);
            if (dsTask.Tables[0].Rows.Count > 0)
            {
                int intAnswer = Int32.Parse(dsTask.Tables[0].Rows[0]["answerid"].ToString());
                return GetBody(intAnswer, _dsn_asset, _dsn_ip, _environment, _edit);
            }
            else
                return "";
        }
        public string GetBody(int _answerid, string _dsn_asset, string _dsn_ip, int _environment, bool _edit)
        {
            Servers oServer = new Servers(0, dsn);
            Forecast oForecast = new Forecast(0, dsn);
            OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
            Requests oRequest = new Requests(0, dsn);
            Classes oClass = new Classes(0, dsn);
            Projects oProject = new Projects(0, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
            Organizations oOrganization = new Organizations(0, dsn);
            Users oUser = new Users(0, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            Mnemonic oMnemonic = new Mnemonic(0, dsn);
            Asset oAsset = new Asset(0, _dsn_asset, dsn);
            ConsistencyGroups oConsistencyGroups = new ConsistencyGroups(0, dsn);
            Environments oEnvironment = new Environments(0, dsn);
            OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);
            Solaris oSolaris = new Solaris(0, dsn);
            Functions oFunction = new Functions(0, dsn, _environment);
            ServerName oServerName = new ServerName(0, dsn);
            string strName = "";
            int intUseLunSerial = Int32.Parse(ConfigurationManager.AppSettings["USE_SAN_LUN_SERIAL"]);

            StringBuilder sbData = new StringBuilder();
            DataSet dsAnswer = oServer.GetAnswer(_answerid);
            if (dsAnswer.Tables[0].Rows.Count > 0)
            {
                int intForecast = Int32.Parse(oForecast.GetAnswer(_answerid, "forecastid"));
                int intForecastRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                int intProject = oRequest.GetProjectNumber(intForecastRequest);
                int intClass = Int32.Parse(oForecast.GetAnswer(_answerid, "classid"));
                int intEnv = Int32.Parse(oForecast.GetAnswer(_answerid, "environmentid"));

                int intModel = oForecast.GetModelAsset(_answerid);
                if (intModel == 0)
                    intModel = oForecast.GetModel(_answerid);

                bool boolTest = false;
                bool boolQA = false;
                bool boolProd = false;
                bool boolUnder = false;
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
                string strClass = "Test";
                if (boolProd == true)
                    strClass = "Production";
                else if (boolQA == true)
                    strClass = "QA";

                int intRequest = 0;
                int intItem = 0;
                int intService = 0;
                int intNumber = 0;
                DataSet dsTask = oOnDemandTasks.GetServerStorage(_answerid, (boolProd ? 1 : 0));
                if (dsTask.Tables[0].Rows.Count > 0)
                {
                    intRequest = Int32.Parse(dsTask.Tables[0].Rows[0]["requestid"].ToString());
                    intItem = Int32.Parse(dsTask.Tables[0].Rows[0]["itemid"].ToString());
                    intNumber = Int32.Parse(dsTask.Tables[0].Rows[0]["number"].ToString());
                }

                sbData.Append("<tr><td colspan=\"2\">");
                sbData.Append(oForecast.GetResiliencyAlert(_answerid));
                sbData.Append("</td></tr>");
                sbData.Append("<tr><td colspan=\"2\" class=\"header\">This is a <u>");
                sbData.Append(strClass);
                sbData.Append("</u> Request!</tr>");
                sbData.Append("<tr><td nowrap>Request ID:</td><td width=\"100%\">CVT");
                sbData.Append(intRequest.ToString());
                sbData.Append("</td></tr>");
                sbData.Append("<tr><td colspan=\"2\" class=\"header\">&nbsp;</tr>");

                int intImplementor = 0;
                DataSet dsTasks = oOnDemandTasks.GetPending(_answerid);
                if (dsTasks.Tables[0].Rows.Count > 0)
                {
                    intImplementor = Int32.Parse(dsTasks.Tables[0].Rows[0]["resourceid"].ToString());
                    intImplementor = Int32.Parse(oResourceRequest.GetWorkflow(intImplementor, "userid"));
                }
                else
                    intImplementor = -999;

                sbData.Append("<tr><td colspan=\"2\" class=\"header\">Project Information</tr>");
                sbData.Append("<tr><td nowrap>Project Name:</td><td width=\"100%\">");
                sbData.Append(oProject.Get(intProject, "name"));
                sbData.Append("</td></tr>");
                sbData.Append("<tr><td nowrap>Project Number:</td><td width=\"100%\">");
                sbData.Append(oProject.Get(intProject, "number"));
                sbData.Append("</td></tr>");
                sbData.Append("<tr><td nowrap>Project Type:</td><td width=\"100%\">");
                sbData.Append(oProject.Get(intProject, "bd"));
                sbData.Append("</td></tr>");
                sbData.Append("<tr><td nowrap>Portfolio:</td><td width=\"100%\">");
                sbData.Append(oOrganization.GetName(Int32.Parse(oProject.Get(intProject, "organization"))));
                sbData.Append("</td></tr>");
                string strLead = oProject.Get(intProject, "lead");
                string strRequester = oForecast.Get(intForecast, "userid");
                string strEngineer = oProject.Get(intProject, "engineer");
                string strTechnical = oProject.Get(intProject, "technical");
                sbData.Append("<tr><td nowrap>Project Manager:</td><td width=\"100%\">");
                sbData.Append(strLead != "" ? oUser.GetFullName(Int32.Parse(strLead)) : "N/A");
                sbData.Append("</td></tr>");
                sbData.Append("<tr><td nowrap>Requester:</td><td width=\"100%\">");
                sbData.Append(strRequester != "" ? oUser.GetFullName(Int32.Parse(strRequester)) : "N/A");
                sbData.Append("</td></tr>");
                sbData.Append("<tr><td nowrap>Integration Engineer:</td><td width=\"100%\">");
                sbData.Append(strEngineer != "" ? oUser.GetFullName(Int32.Parse(strEngineer)) : "N/A");
                sbData.Append("</td></tr>");
                sbData.Append("<tr><td nowrap>Technical Lead:</td><td width=\"100%\">");
                sbData.Append(strTechnical != "" ? oUser.GetFullName(Int32.Parse(strTechnical)) : "N/A");
                sbData.Append("</td></tr>");
                sbData.Append("<tr><td nowrap>II Resource:</td><td width=\"100%\">");
                sbData.Append(intImplementor > 0 || intImplementor == -999 ? oUser.GetFullName(intImplementor) : "N/A");
                sbData.Append("</td></tr>");

                // Design Information
                sbData.Append("<tr><td colspan=\"2\">&nbsp;</tr>");
                sbData.Append("<tr><td colspan=\"2\" class=\"header\">Design Information</tr>");
                double dblQuantity = double.Parse(oForecast.GetAnswer(_answerid, "quantity")) + double.Parse(oForecast.GetAnswer(_answerid, "recovery_number"));
                sbData.Append("<tr><td nowrap>Commitment Date:</td><td width=\"100%\">");
                sbData.Append(oForecast.GetAnswer(_answerid, "implementation") == "" ? "" : DateTime.Parse(oForecast.GetAnswer(_answerid, "implementation")).ToShortDateString());
                sbData.Append("</td></tr>");
                double dblA = 0.00;
                DataSet dsA = oForecast.GetAcquisitions(intModel, 1);
                foreach (DataRow drA in dsA.Tables[0].Rows)
                    dblA += double.Parse(drA["cost"].ToString());
                sbData.Append("<tr><td nowrap>Acquisition Costs:</td><td width=\"100%\">");
                sbData.Append(dblA.ToString("N"));
                sbData.Append("</td></tr>");
                double dblO = 0.00;
                DataSet dsO = oForecast.GetOperations(intModel, 1);
                foreach (DataRow drO in dsO.Tables[0].Rows)
                    dblO += double.Parse(drO["cost"].ToString());
                sbData.Append("<tr><td nowrap>Operational Costs:</td><td width=\"100%\">");
                sbData.Append(dblO.ToString("N"));
                sbData.Append("</td></tr>");
                double dblAmp = (double.Parse(oModelsProperties.Get(intModel, "amp")) * dblQuantity);
                sbData.Append("<tr><td nowrap>AMPs:</td><td width=\"100%\">");
                sbData.Append(dblAmp.ToString("N"));
                sbData.Append(" AMPs");
                sbData.Append("</td></tr>");
                sbData.Append("<tr><td nowrap>Application Name:</td><td width=\"100%\">");
                sbData.Append(oForecast.GetAnswer(_answerid, "appname"));
                sbData.Append("</td></tr>");
                int intMnemonic = 0;
                if (oForecast.GetAnswer(_answerid, "mnemonicid") != "")
                    intMnemonic = Int32.Parse(oForecast.GetAnswer(_answerid, "mnemonicid"));
                if (intMnemonic > 0)
                {
                    sbData.Append("<tr><td valign=\"top\">Mnemonic:</td><td valign=\"top\">");
                    sbData.Append(oMnemonic.Get(intMnemonic, "factory_code"));
                    sbData.Append("</td></tr>");
                }
                else
                {
                    sbData.Append("<tr><td nowrap>Application Code:</td><td width=\"100%\">");
                    sbData.Append(oForecast.GetAnswer(_answerid, "appcode"));
                    sbData.Append("</td></tr>");
                }
                string strContact1 = oForecast.GetAnswer(_answerid, "appcontact");
                if (strContact1 != "")
                {
                    sbData.Append("<tr><td nowrap>Departmental Manager:</td><td width=\"100%\">");
                    sbData.Append(oUser.GetFullName(Int32.Parse(strContact1)));
                    sbData.Append(" (");
                    sbData.Append(oUser.GetName(Int32.Parse(strContact1)));
                    sbData.Append(")");
                    sbData.Append("</td></tr>");
                }
                string strContact2 = oForecast.GetAnswer(_answerid, "admin1");
                if (strContact2 != "")
                {
                    sbData.Append("<tr><td nowrap>Application Technical Lead:</td><td width=\"100%\">");
                    sbData.Append(oUser.GetFullName(Int32.Parse(strContact2)));
                    sbData.Append(" (");
                    sbData.Append(oUser.GetName(Int32.Parse(strContact2)));
                    sbData.Append(")");
                    sbData.Append("</td></tr>");
                }
                string strContact3 = oForecast.GetAnswer(_answerid, "admin2");
                if (strContact3 != "")
                {
                    sbData.Append("<tr><td nowrap>Administrative Contact:</td><td width=\"100%\">");
                    sbData.Append(oUser.GetFullName(Int32.Parse(strContact3)));
                    sbData.Append(" (");
                    sbData.Append(oUser.GetName(Int32.Parse(strContact3)));
                    sbData.Append(")");
                    sbData.Append("</td></tr>");
                }
                int intPlatform = Int32.Parse(oForecast.GetAnswer(_answerid, "platformid"));

                sbData.Append("<tr><td colspan=\"2\"><div style=\"border:solid 2px #000099; padding:5px; width:425px\"><img src=\"/images/bigInfo.gif\" border=\"0\" align=\"absmiddle\"/><b>NOTE:</b> All existing LUNs should be EXPANDED</div></td></tr>");

                StringBuilder sbDataInfo = new StringBuilder();
                int intCount = 0;

                string strEmail = "";
                string strShared = "";
                foreach (DataRow dr in dsAnswer.Tables[0].Rows)
                {
                    intCount++;
                    int intServer = Int32.Parse(dr["id"].ToString());
                    int intUser = Int32.Parse(oForecast.GetAnswer(_answerid, "userid"));
                    if (intUser > 0)
                        strEmail = oUser.GetName(intUser);
                    int intCSM = Int32.Parse(dr["csmconfigid"].ToString());
                    int intCluster = Int32.Parse(dr["clusterid"].ToString());
                    int intNumber2 = Int32.Parse(dr["number"].ToString());
                    int intName = Int32.Parse(dr["nameid"].ToString());
                    strName = oServer.GetName(intServer, true);
                    if (strShared != "")
                        strShared += ", ";
                    strShared += strName;
                    sbDataInfo.Append("<tr><td nowrap>Design Nickname:</td><td width=\"100%\">");
                    sbDataInfo.Append(oForecast.GetAnswer(_answerid, "name"));
                    sbDataInfo.Append("</td></tr>");
                    DataSet dsGeneric = oServer.GetGeneric(intServer);
                    string strVIO = "";
                    string strVIODR = "";
                    if (oModelsProperties.IsVIO(intModel) == true)
                    {
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
                        sbDataInfo.Append("<tr><td nowrap>Server Name(s):</td><td width=\"100%\">");
                        sbDataInfo.Append(strVIO);
                        sbDataInfo.Append("&nbsp;&nbsp;&nbsp;&nbsp;(");
                        sbDataInfo.Append(strVIO.ToLower());
                        sbDataInfo.Append(")");
                        sbDataInfo.Append("</td></tr>");
                    }
                    else
                    {
                        sbDataInfo.Append("<tr><td nowrap>Server Name:</td><td width=\"100%\">");
                        sbDataInfo.Append(strName);
                        sbDataInfo.Append("&nbsp;&nbsp;&nbsp;&nbsp;(");
                        sbDataInfo.Append(strName.ToLower());
                        sbDataInfo.Append(")");
                        sbDataInfo.Append("</td></tr>");
                    }
                    int intAsset = 0;
                    DataSet dsAssets = oServer.GetAssets(intServer);
                    foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                    {
                        int intClassAsset = Int32.Parse(drAsset["classid"].ToString());
                        int intTestAsset = -1;
                        if (intClassAsset > 0)
                            intTestAsset = (oClass.IsTestDev(intClassAsset) ? 1 : 0);
                        if (intTestAsset == 1 && (boolTest == true || boolQA == true))
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
                                sbDataInfo.Append("<tr><td nowrap>Dummy Name (BFS):</td><td width=\"100%\">");
                                sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["dummy_name"].ToString());
                                sbDataInfo.Append("&nbsp;&nbsp;&nbsp;&nbsp;(");
                                sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["dummy_name"].ToString().ToLower());
                                sbDataInfo.Append(")");
                                sbDataInfo.Append("</td></tr>");
                            }
                            if (boolProd == true && dsGeneric.Tables[0].Rows[0]["dummy_name_prod"].ToString() != "")
                            {
                                sbDataInfo.Append("<tr><td nowrap>Dummy Name (BFS):</td><td width=\"100%\">");
                                sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["dummy_name_prod"].ToString());
                                sbDataInfo.Append("&nbsp;&nbsp;&nbsp;&nbsp;(");
                                sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["dummy_name_prod"].ToString().ToLower());
                                sbDataInfo.Append(")");
                                sbDataInfo.Append("</td></tr>");
                            }
                        }
                        if (oModelsProperties.IsVIO(intModel) == false)
                        {
                            if ((boolTest == true || boolQA == true) && (dsGeneric.Tables[0].Rows[0]["ww1"].ToString() != "" || dsGeneric.Tables[0].Rows[0]["ww2"].ToString() != ""))
                            {
                                sbDataInfo.Append("<tr><td nowrap>World Wide Port Name(s):</td><td width=\"100%\">");
                                sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["ww1"].ToString());
                                sbDataInfo.Append(", ");
                                sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["ww2"].ToString());
                                sbDataInfo.Append("</td></tr>");
                            }
                            if (boolProd == true && (dsGeneric.Tables[0].Rows[0]["ww1_prod"].ToString() != "" || dsGeneric.Tables[0].Rows[0]["ww2_prod"].ToString() != ""))
                            {
                                sbDataInfo.Append("<tr><td nowrap>World Wide Port Name(s):</td><td width=\"100%\">");
                                sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["ww1_prod"].ToString());
                                sbDataInfo.Append(", ");
                                sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["ww2_prod"].ToString());
                                sbDataInfo.Append("</td></tr>");
                            }
                        }
                    }
                    else
                    {
                        if (oModelsProperties.IsSUNVirtual(intModel) == false)
                        {
                            if (oModelsProperties.IsStorageDB_BootSANUnix(intModel) == true || oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                            {
                                sbDataInfo.Append("<tr><td nowrap>Dummy Name (BFS):</td><td width=\"100%\">");
                                sbDataInfo.Append(oAsset.GetServerOrBlade(intAsset, "dummy_name"));
                                sbDataInfo.Append("&nbsp;&nbsp;&nbsp;&nbsp;(");
                                sbDataInfo.Append(oAsset.GetServerOrBlade(intAsset, "dummy_name").ToLower());
                                sbDataInfo.Append(")");
                                sbDataInfo.Append("</td></tr>");
                            }
                            DataSet dsHBA = oAsset.GetHBA(intAsset);
                            string strHBA = "";
                            foreach (DataRow drHBA in dsHBA.Tables[0].Rows)
                            {
                                if (strHBA != "")
                                    strHBA += ", ";
                                strHBA += drHBA["name"].ToString();
                            }
                            sbDataInfo.Append("<tr><td nowrap>World Wide Port Names:</td><td width=\"100%\">");
                            sbDataInfo.Append(strHBA);
                            sbDataInfo.Append("</td></tr>");
                        }
                    }

                    int intDR = 0;
                    if (dr["drid"].ToString() != "")
                        intDR = Int32.Parse(dr["drid"].ToString());
                    if (intDR == 0 && boolUnder == true && dr["dr"].ToString() == "1")
                        sbDataInfo.Append("<tr><td></td><td><img src=\"/images/returned.gif\" border=\"0\" align=\"absmiddle\"/>&nbsp;There is no DR asset</td></tr>");
                    if (oModelsProperties.IsVIO(intModel) == true)
                    {
                        if (boolUnder == true && dr["dr"].ToString() == "1")
                        {
                            if (dr["dr_name"].ToString() != "")
                            {
                                sbDataInfo.Append("<tr><td nowrap>DR Server Name:</td><td width=\"100%\">");
                                sbDataInfo.Append(dr["dr_name"].ToString());
                                sbDataInfo.Append("&nbsp;&nbsp;&nbsp;&nbsp;(");
                                sbDataInfo.Append(dr["dr_name"].ToString().ToLower());
                                sbDataInfo.Append(")");
                                sbDataInfo.Append("</td></tr>");
                            }
                            else
                            {
                                strVIODR = dsGeneric.Tables[0].Rows[0]["vio1_dr"].ToString() + ", " + dsGeneric.Tables[0].Rows[0]["vio2_dr"].ToString();
                                sbDataInfo.Append("<tr><td nowrap>DR Server Name(s):</td><td width=\"100%\">");
                                sbDataInfo.Append(strVIODR);
                                sbDataInfo.Append("&nbsp;&nbsp;&nbsp;&nbsp;(");
                                sbDataInfo.Append(strVIODR.ToLower());
                                sbDataInfo.Append(")");
                                sbDataInfo.Append("</td></tr>");
                            }
                        }
                    }
                    else
                    {
                        if (intDR > 0 && boolUnder == true && dr["dr"].ToString() == "1")
                        {
                            if (dr["dr_name"].ToString() != "")
                            {
                                sbDataInfo.Append("<tr><td nowrap>DR Server Name:</td><td width=\"100%\">");
                                sbDataInfo.Append(dr["dr_name"].ToString());
                                sbDataInfo.Append("&nbsp;&nbsp;&nbsp;&nbsp;(");
                                sbDataInfo.Append(dr["dr_name"].ToString().ToLower());
                                sbDataInfo.Append(")");
                                sbDataInfo.Append("</td></tr>");
                            }
                            else
                            {
                                sbDataInfo.Append("<tr><td nowrap>DR Server Name:</td><td width=\"100%\">");
                                sbDataInfo.Append(strName);
                                sbDataInfo.Append("-DR");
                                sbDataInfo.Append("&nbsp;&nbsp;&nbsp;&nbsp;(");
                                sbDataInfo.Append(strName.ToLower());
                                sbDataInfo.Append("-dr");
                                sbDataInfo.Append(")");
                                sbDataInfo.Append("</td></tr>");
                            }
                        }
                    }
                    if (boolProd == true)
                    {
                        if (dsGeneric.Tables[0].Rows.Count > 0)
                        {
                            if (oModelsProperties.IsStorageDB_BootSANUnix(intModel) == true || oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                            {
                                if (dsGeneric.Tables[0].Rows[0]["dummy_name_dr"].ToString() != "")
                                {
                                    sbDataInfo.Append("<tr><td nowrap>DR Dummy Name (BFS):</td><td width=\"100%\">");
                                    sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["dummy_name_dr"].ToString());
                                    sbDataInfo.Append("&nbsp;&nbsp;&nbsp;&nbsp;(");
                                    sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["dummy_name_dr"].ToString().ToLower());
                                    sbDataInfo.Append(")");
                                    sbDataInfo.Append("</td></tr>");
                                }
                            }
                            if (oModelsProperties.IsVIO(intModel) == false)
                            {
                                if (dsGeneric.Tables[0].Rows[0]["ww1_dr"].ToString() != "" || dsGeneric.Tables[0].Rows[0]["ww2_dr"].ToString() != "")
                                {
                                    sbDataInfo.Append("<tr><td nowrap>DR World Wide Port Name(s):</td><td width=\"100%\">");
                                    sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["ww1_dr"].ToString());
                                    sbDataInfo.Append(", ");
                                    sbDataInfo.Append(dsGeneric.Tables[0].Rows[0]["ww2_dr"].ToString());
                                    sbDataInfo.Append("</td></tr>");
                                }
                            }
                        }
                        else
                        {
                            if (intDR > 0 && (oModelsProperties.IsStorageDB_BootSANUnix(intModel) == true || oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true))
                            {
                                sbDataInfo.Append("<tr><td nowrap>DR Dummy Name (BFS):</td><td width=\"100%\">");
                                sbDataInfo.Append(oAsset.GetServerOrBlade(intDR, "dummy_name"));
                                sbDataInfo.Append("&nbsp;&nbsp;&nbsp;&nbsp;(");
                                sbDataInfo.Append(oAsset.GetServerOrBlade(intDR, "dummy_name").ToLower());
                                sbDataInfo.Append(")");
                                sbDataInfo.Append("</td></tr>");
                            }
                            DataSet dsHBA = oAsset.GetHBA(intDR);
                            string strHBA = "";
                            foreach (DataRow drHBA in dsHBA.Tables[0].Rows)
                            {
                                if (strHBA != "")
                                    strHBA += ", ";
                                strHBA += drHBA["name"].ToString();
                            }
                            if (intDR > 0)
                            {
                                sbDataInfo.Append("<tr><td nowrap>DR World Wide Port Names:</td><td width=\"100%\">");
                                sbDataInfo.Append(strHBA);
                                sbDataInfo.Append("</td></tr>");
                            }
                        }
                    }
                    sbDataInfo.Append("<tr><td nowrap>Assigned IP Address:</td><td width=\"100%\">");
                    sbDataInfo.Append(oServer.GetIPs(intServer, 1, 0, 0, 0, _dsn_ip, "", ""));
                    sbDataInfo.Append("</td></tr>");
                    sbDataInfo.Append("<tr><td nowrap>Final IP Address:</td><td width=\"100%\">");
                    sbDataInfo.Append(oServer.GetIPs(intServer, 0, 1, 0, 0, _dsn_ip, "", ""));
                    sbDataInfo.Append("</td></tr>");
                    sbDataInfo.Append("<tr><td nowrap>Backup IP Address:</td><td width=\"100%\">");
                    sbDataInfo.Append(oServer.GetIPs(intServer, 0, 0, 0, 1, _dsn_ip, "", ""));
                    sbDataInfo.Append("</td></tr>");
                    int intType = oModelsProperties.GetType(intModel);

                    sbDataInfo.Append("<tr><td nowrap>Is a High Availability Device:</td><td width=\"100%\">");
                    sbDataInfo.Append(oForecast.IsHARoom(_answerid) ? (dr["ha"].ToString() == "10" ? "Yes" : "No") : "N / A");
                    sbDataInfo.Append("</td></tr>");
                    sbDataInfo.Append("<tr><td nowrap>Model:</td><td width=\"100%\">");
                    sbDataInfo.Append(oModelsProperties.Get(intModel, "name"));
                    sbDataInfo.Append("</td></tr>");
                    sbDataInfo.Append("<tr><td nowrap>Fabric:</td><td width=\"100%\">");
                    sbDataInfo.Append(oModelsProperties.GetFabric(intModel));
                    sbDataInfo.Append("</td></tr>");
                    if (intAsset > 0)
                    {
                        sbDataInfo.Append("<tr><td nowrap>Serial Number:</td><td width=\"100%\">");
                        sbDataInfo.Append(oAsset.Get(intAsset, "serial").ToUpper());
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>Asset Tag:</td><td width=\"100%\">");
                        sbDataInfo.Append(oAsset.Get(intAsset, "asset").ToUpper());
                        sbDataInfo.Append("</td></tr>");
                        if (oModelsProperties.IsSUNVirtual(intModel) == false)
                        {
                            sbDataInfo.Append("<tr><td nowrap>Room:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.GetServerOrBlade(intAsset, "room"));
                            sbDataInfo.Append("</td></tr>");
                            sbDataInfo.Append("<tr><td nowrap>Rack:</td><td width=\"100%\">");
                            sbDataInfo.Append(oAsset.GetServerOrBlade(intAsset, "rack"));
                            sbDataInfo.Append("</td></tr>");
                        }
                        if (boolProd == true)
                        {
                            foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                            {
                                if (drAsset["latest"].ToString() == "0" && drAsset["dr"].ToString() == "0")
                                {
                                    int intAssetOld = Int32.Parse(drAsset["assetid"].ToString());
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Serial Number:</td><td width=\"100%\">");
                                    sbDataInfo.Append(oAsset.Get(intAssetOld, "serial").ToUpper());
                                    sbDataInfo.Append("</td></tr>");
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Asset Tag:</td><td width=\"100%\">");
                                    sbDataInfo.Append(oAsset.Get(intAssetOld, "asset").ToUpper());
                                    sbDataInfo.Append("</td></tr>");
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Class:</td><td width=\"100%\">");
                                    sbDataInfo.Append(drAsset["class"].ToString());
                                    sbDataInfo.Append("</td></tr>");
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Environment:</td><td width=\"100%\">");
                                    sbDataInfo.Append(drAsset["environment"].ToString());
                                    sbDataInfo.Append("</td></tr>");
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Room:</td><td width=\"100%\">");
                                    sbDataInfo.Append(oAsset.GetServerOrBlade(intAssetOld, "room"));
                                    sbDataInfo.Append("</td></tr>");
                                    sbDataInfo.Append("<tr><td nowrap> - Previous Rack:</td><td width=\"100%\">");
                                    sbDataInfo.Append(oAsset.GetServerOrBlade(intAssetOld, "rack"));
                                    sbDataInfo.Append("</td></tr>");
                                    DataSet dsHBA = oAsset.GetHBA(intAssetOld);
                                    string strHBA = "";
                                    foreach (DataRow drHBA in dsHBA.Tables[0].Rows)
                                    {
                                        if (strHBA != "")
                                            strHBA += ", ";
                                        strHBA += drHBA["name"].ToString();
                                    }
                                    sbDataInfo.Append("<tr><td nowrap> - Previous World Wide Port Names:</td><td width=\"100%\">");
                                    sbDataInfo.Append(strHBA);
                                    sbDataInfo.Append("</td></tr>");
                                    if (oAsset.GetServerOrBlade(intAssetOld, "enclosureid") != "")
                                    {
                                        int intEnclosureID = Int32.Parse(oAsset.GetServerOrBlade(intAssetOld, "enclosureid"));
                                        if (intEnclosureID > 0)
                                        {
                                            sbDataInfo.Append("<tr><td nowrap> - Previous Enclosure:</td><td width=\"100%\">");
                                            sbDataInfo.Append(oAsset.Get(intEnclosureID, "name"));
                                            sbDataInfo.Append("</td></tr>");
                                            sbDataInfo.Append("<tr><td nowrap> - Previous Slot:</td><td width=\"100%\">");
                                            sbDataInfo.Append(oAsset.GetServerOrBlade(intAssetOld, "slot"));
                                            sbDataInfo.Append("</td></tr>");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (boolProd == true && intDR > 0)
                    {
                        sbDataInfo.Append("<tr><td nowrap>DR Serial Number:</td><td width=\"100%\">");
                        sbDataInfo.Append(oAsset.Get(intDR, "serial").ToUpper());
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>DR Asset Tag:</td><td width=\"100%\">");
                        sbDataInfo.Append(oAsset.Get(intDR, "asset").ToUpper());
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>DR Room:</td><td width=\"100%\">");
                        sbDataInfo.Append(oAsset.GetServerOrBlade(intDR, "room"));
                        sbDataInfo.Append("</td></tr>");
                        sbDataInfo.Append("<tr><td nowrap>DR Rack:</td><td width=\"100%\">");
                        sbDataInfo.Append(oAsset.GetServerOrBlade(intDR, "rack"));
                        sbDataInfo.Append("</td></tr>");
                    }
                    sbDataInfo.Append("<tr><td nowrap>Current Class:</td><td width=\"100%\">");
                    sbDataInfo.Append(strClass);
                    sbDataInfo.Append("</td></tr>");
                    sbDataInfo.Append("<tr><td nowrap>Environment:</td><td width=\"100%\">");
                    sbDataInfo.Append(oEnvironment.Get(intEnv, "name"));
                    sbDataInfo.Append("</td></tr>");
                    int intOS = Int32.Parse(dr["osid"].ToString());
                    sbDataInfo.Append("<tr><td nowrap>Operating System:</td><td width=\"100%\">");
                    sbDataInfo.Append(oOperatingSystem.Get(intOS, "name"));
                    sbDataInfo.Append("</td></tr>");
                    sbDataInfo.Append("<tr><td nowrap>Clustered Server Name:</td><td width=\"100%\">");
                    sbDataInfo.Append("N / A");
                    sbDataInfo.Append("</td></tr>");
                    int intConsistency = Int32.Parse(dr["dr_consistencyid"].ToString());
                    if (intConsistency > 0)
                    {
                        sbDataInfo.Append("<tr><td nowrap>Consistency Group Name:</td><td width=\"100%\">");
                        sbDataInfo.Append(oConsistencyGroups.Get(intConsistency, "name"));
                        sbDataInfo.Append("</td></tr>");
                        DataSet dsMembers = oConsistencyGroups.GetMember(intServer);
                        string strMembers = "";
                        foreach (DataRow drMember in dsMembers.Tables[0].Rows)
                        {
                            if (strMembers != "")
                                strMembers += ", ";
                            strMembers += drMember["name"].ToString();
                        }
                        sbDataInfo.Append("<tr><td nowrap>Consistency Group Members:</td><td width=\"100%\">");
                        sbDataInfo.Append(strMembers);
                        sbDataInfo.Append("</td></tr>");
                    }
                    else
                    {
                        sbDataInfo.Append("<tr><td nowrap>Consistency Group:</td><td width=\"100%\">");
                        sbDataInfo.Append("N / A");
                        sbDataInfo.Append("</td></tr>");
                    }

                    if (oModelsProperties.IsSUNVirtual(intModel) == true)
                    {
                        sbDataInfo.Append("<tr><td colspan=\"2\" class=\"framegreen\">Host Information</td></tr>");
                        // Show Host Information
                        StringBuilder sbHostInfo = new StringBuilder();
                        DataSet dsSVE = oSolaris.GetSVEGuest(intServer);
                        if (dsSVE.Tables[0].Rows.Count == 1)
                        {
                            int intSVECluster = Int32.Parse(dsSVE.Tables[0].Rows[0]["clusterid"].ToString());
                            sbHostInfo.Append("<tr class=\"bold\"><td>Name</td><td>Serial</td><td>Model</td><td>WWPNs</td><td>Dummy</td></tr>");
                            DataSet dsSVEHosts = oServer.GetSVEClusters(intSVECluster);
                            foreach (DataRow drSVEHost in dsSVEHosts.Tables[0].Rows)
                            {
                                sbHostInfo.Append("<tr>");
                                sbHostInfo.Append("<td valign=\"top\"><a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/asset/server.aspx?t=name&q=" + oFunction.encryptQueryString(drSVEHost["servername"].ToString()) + "&id=" + oFunction.encryptQueryString(drSVEHost["id"].ToString()) + "','800','600')\">" + drSVEHost["servername"].ToString() + "</a></td>");
                                sbHostInfo.Append("<td valign=\"top\"><a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/asset/" + drSVEHost["url"].ToString() + ".aspx?t=serial&q=" + oFunction.encryptQueryString(drSVEHost["serial"].ToString()) + "&id=" + oFunction.encryptQueryString(drSVEHost["assetid"].ToString()) + "','800','600')\">" + drSVEHost["serial"].ToString() + "</a></td>");
                                sbHostInfo.Append("<td valign=\"top\">" + drSVEHost["model"].ToString() + "</td>");
                                DataSet dsSVEHBA = oAsset.GetHBA(Int32.Parse(drSVEHost["assetid"].ToString()));
                                string strSVEHBA = "";
                                foreach (DataRow drSVEHBA in dsSVEHBA.Tables[0].Rows)
                                {
                                    if (strSVEHBA != "")
                                        strSVEHBA += "<br/>";
                                    strSVEHBA += drSVEHBA["name"].ToString();
                                }
                                sbHostInfo.Append("<td valign=\"top\">" + strSVEHBA + "</td>");
                                sbHostInfo.Append("<td valign=\"top\">" + drSVEHost["dummy_name"].ToString() + "</td>");
                                sbHostInfo.Append("<tr>");
                            }
                            sbHostInfo.Insert(0, "<table width=\"100%\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\">");
                            sbHostInfo.Append("</table>");
                        }
                        else
                            sbHostInfo.Append("Unable to retrieve host information (" + intServer.ToString() + " = " + dsSVE.Tables[0].Rows.Count.ToString() + ")");

                        sbDataInfo.Append("<tr><td colspan=\"2\">");
                        sbDataInfo.Append(sbHostInfo.ToString());
                        sbDataInfo.Append("</td></tr>");
                    }

                    string strStorageType = "";
                    if (oModelsProperties.IsSUNVirtual(intModel) == true)
                        strStorageType = "SUN VIRTUAL";
                    else if (oModelsProperties.IsTypeBlade(intModel) == true)
                        strStorageType = "BLADE";
                    else
                        strStorageType = "RACKED";
                    if (strVIO == "")
                    {
                        if (intCluster > 0)
                            strStorageType += " - CLUSTER";
                        sbData.Append("<tr><td colspan=\"2\">&nbsp;</tr>");
                        sbData.Append("<tr><td colspan=\"2\" class=\"header\">");
                        sbData.Append("ClearView Storage Request Form for ");
                        sbData.Append(strName);
                        sbData.Append(" (");
                        sbData.Append(strStorageType);
                        sbData.Append(")");
                        sbData.Append("</tr>");
                    }
                    else
                    {
                        sbData.Append("<tr><td colspan=\"2\">&nbsp;</tr>");
                        sbData.Append("<tr><td colspan=\"2\" class=\"header\">");
                        sbData.Append("ClearView Storage Request Form for ");
                        sbData.Append(strVIO);
                        sbData.Append(" (");
                        sbData.Append(strStorageType);
                        sbData.Append(" - VIO)");
                        sbData.Append("</tr>");
                    }
                    sbData.Append(sbDataInfo.ToString());
                    sbDataInfo = new StringBuilder();

                    // STORAGE INFORMATION
                    StringBuilder sbStorage = new StringBuilder();
                    StringBuilder sbStorageShared = new StringBuilder();
                    string strStorageSharedHeader = "";
                    StringBuilder sbHidden = new StringBuilder();
                    double dblTotal = 0.00;
                    bool boolOther = false;
                    int intRow = 0;
                    DataSet dsLuns = new DataSet();
                    bool boolOverride = (oForecast.GetAnswer(_answerid, "storage_override") == "1");
                    if (intCluster == 0)
                    {
                        dsLuns = GetLuns(_answerid, 0, intCluster, intCSM, intNumber2);
                        //Response.Write("Answerid: " + intAnswer.ToString() + ", clusterid: " + intCluster.ToString() + ", csmconfigid: " + intCSM.ToString() + ", number: " + intNumber2.ToString());
                    }
                    else
                        dsLuns = GetLunsClusterNonShared(_answerid, intCluster, intNumber2);
                    if (oModelsProperties.IsSUNVirtual(intModel) == false)
                    {
                        string strFirstLunPerformance = "Standard";
                        string strFirstLunReplicated = "0";
                        string strFirstLunHA = "0";
                        string strFirstLunSize = "0";

                        if (dsLuns.Tables[0].Rows.Count > 0)
                        {
                            strFirstLunPerformance = dsLuns.Tables[0].Rows[0]["performance"].ToString();
                            //strFirstLunReplicated = dsLuns.Tables[0].Rows[0]["replicated"].ToString();
                            strFirstLunHA = dsLuns.Tables[0].Rows[0]["high_availability"].ToString();
                            //strFirstLunSize = dsLuns.Tables[0].Rows[0]["size"].ToString();
                        }

                        bool boolBootFromSAN = (oModelsProperties.IsStorageDB_BootSANWindows(intModel) || oModelsProperties.IsStorageDB_BootSANUnix(intModel));

                        if (oOperatingSystem.IsSolaris(intOS) == true)
                        {
                            // ADD DATA VOL "/" (100 GB hardcoded)
                            intRow++;
                            sbStorage.Append("<tr class=\"" + strBootVolumeBackground + "\">");
                            sbStorage.Append("<td>");
                            sbStorage.Append("1 *");
                            sbStorage.Append("</td>");
                            sbStorage.Append("<td>");
                            sbStorage.Append("/");
                            sbStorage.Append("</td>");
                            sbStorage.Append("<td>");
                            sbStorage.Append(strFirstLunPerformance);
                            sbStorage.Append("</td>");

                            if (intUseLunSerial > 0)
                            {
                                //Lun Serial # IsStorageDB_BootSANUnix
                                sbStorage.Append("<td nowrap>---</td>");
                                //Lun Serial # IsStorageDB_BootSANUnix
                            }

                            sbStorage.Append("<td>");
                            sbStorage.Append("100 GB");
                            sbStorage.Append("</td>");
                            sbStorage.Append("<td>");
                            sbStorage.Append("100 GB");
                            sbStorage.Append("</td>");
                            //dblTotal += 100.00;

                            if (boolProd == true)
                            {
                                sbStorage.Append("<td>");
                                //sbStorage.Append(strFirstLunReplicated == "0" ? "No" : "Yes");
                                sbStorage.Append(intDR > 0 ? "Yes" : "No");
                                sbStorage.Append("</td>");
                                sbStorage.Append("<td>");
                                sbStorage.Append("<select class=\"default\" style=\"width:50px;\"><option value=\"No\"");
                                sbStorage.Append(intDR == 0 ? " selected" : "");
                                sbStorage.Append(">No</option><option value=\"Yes\"");
                                sbStorage.Append(intDR > 0 ? " selected" : "");
                                sbStorage.Append(">Yes</option></select>");
                                sbStorage.Append("</td>");
                            }
                            if (boolProd == true)
                            {
                                sbStorage.Append("<td>No</td>");
                                sbStorage.Append("<td>");
                                sbStorage.Append("<select class=\"default\" style=\"width:50px;\"><option value=\"No\" selected>No</option><option value=\"Yes\">Yes</option></select></td>");
                            }
                            sbStorage.Append("</tr>");
                        }
                        else if (boolBootFromSAN == true)
                        {
                            if (oOperatingSystem.IsWindows(intOS) || oOperatingSystem.IsWindows2008(intOS))
                            {
                                intRow++;
                                sbStorage.Append("<tr class=\"" + strBootVolumeBackground + "\">");
                                sbStorage.Append("<td>");
                                sbStorage.Append(intRow.ToString());
                                sbStorage.Append(" *</td>");
                                sbStorage.Append("<td>");
                                sbStorage.Append("C:");
                                sbStorage.Append("</td>");
                                sbStorage.Append("<td>");
                                sbStorage.Append(strFirstLunPerformance);
                                sbStorage.Append("</td>");

                                if (intUseLunSerial > 0)
                                {
                                    //Lun Serial # BootSANWindows
                                    sbStorage.Append("<td nowrap>---</td>");
                                    //Lun Serial # BootSANWindows
                                }


                                if (boolProd == true || boolQA == true || boolTest == true)
                                {
                                    sbStorage.Append("<td>");
                                    sbStorage.Append("60 GB");
                                    sbStorage.Append("</td>");
                                    sbStorage.Append("<td>");
                                    sbStorage.Append("60 GB");
                                    sbStorage.Append("</td>");
                                    //dblTotal += 40.00;
                                }

                                if (boolProd == true)
                                {
                                    sbStorage.Append("<td>");
                                    //sbStorage.Append(strFirstLunReplicated == "0" ? "No" : "Yes");
                                    sbStorage.Append(intDR > 0 ? "Yes" : "No");
                                    sbStorage.Append("</td>");
                                    sbStorage.Append("<td>");
                                    sbStorage.Append("<select class=\"default\" style=\"width:50px;\"><option value=\"No\"");
                                    sbStorage.Append(intDR == 0 ? " selected" : "");
                                    sbStorage.Append(">No</option><option value=\"Yes\"");
                                    sbStorage.Append(intDR > 0 ? " selected" : "");
                                    sbStorage.Append(">Yes</option></select>");
                                    sbStorage.Append("</td>");
                                }
                                if (boolProd == true)
                                {
                                    sbStorage.Append("<td>No</td>");
                                    sbStorage.Append("<td>");
                                    sbStorage.Append("<select class=\"default\" style=\"width:50px;\"><option value=\"No\" selected>No</option><option value=\"Yes\">Yes</option></select></td>");
                                }
                                sbStorage.Append("</tr>");



                                intRow++;
                                sbStorage.Append("<tr class=\"" + strBootVolumeBackground + "\">");
                                sbStorage.Append("<td>");
                                sbStorage.Append(intRow.ToString());
                                sbStorage.Append(" *</td>");
                                sbStorage.Append("<td>");
                                sbStorage.Append("D:");
                                sbStorage.Append("</td>");
                                sbStorage.Append("<td>");
                                sbStorage.Append(strFirstLunPerformance);
                                sbStorage.Append("</td>");

                                if (intUseLunSerial > 0)
                                {
                                    //Lun Serial # BootSANWindows
                                    sbStorage.Append("<td nowrap>---</td>");
                                    //Lun Serial # BootSANWindows
                                }


                                if (boolProd == true || boolQA == true || boolTest == true)
                                {
                                    sbStorage.Append("<td>");
                                    sbStorage.Append("10 GB");
                                    sbStorage.Append("</td>");
                                    sbStorage.Append("<td>");
                                    sbStorage.Append("10 GB");
                                    sbStorage.Append("</td>");
                                    //dblTotal += 40.00;
                                }

                                if (boolProd == true)
                                {
                                    sbStorage.Append("<td>");
                                    //sbStorage.Append(strFirstLunReplicated == "0" ? "No" : "Yes");
                                    sbStorage.Append(intDR > 0 ? "Yes" : "No");
                                    sbStorage.Append("</td>");
                                    sbStorage.Append("<td>");
                                    sbStorage.Append("<select class=\"default\" style=\"width:50px;\"><option value=\"No\"");
                                    sbStorage.Append(intDR == 0 ? " selected" : "");
                                    sbStorage.Append(">No</option><option value=\"Yes\"");
                                    sbStorage.Append(intDR > 0 ? " selected" : "");
                                    sbStorage.Append(">Yes</option></select>");
                                    sbStorage.Append("</td>");
                                }
                                if (boolProd == true)
                                {
                                    sbStorage.Append("<td>No</td>");
                                    sbStorage.Append("<td>");
                                    sbStorage.Append("<select class=\"default\" style=\"width:50px;\"><option value=\"No\" selected>No</option><option value=\"Yes\">Yes</option></select></td>");
                                }
                                sbStorage.Append("</tr>");

                            }
                            if (oOperatingSystem.IsLinux(intOS))
                            {
                                intRow++;
                                sbStorage.Append("<tr class=\"" + strBootVolumeBackground + "\">");
                                sbStorage.Append("<td>");
                                sbStorage.Append(intRow.ToString());
                                sbStorage.Append(" *</td>");
                                sbStorage.Append("<td>");
                                sbStorage.Append("rootvg");
                                sbStorage.Append("</td>");
                                sbStorage.Append("<td>");
                                sbStorage.Append(strFirstLunPerformance);
                                sbStorage.Append("</td>");

                                if (intUseLunSerial > 0)
                                {
                                    //Lun Serial # IsStorageDB_BootSANUnix
                                    sbStorage.Append("<td nowrap>---</td>");
                                    //Lun Serial # IsStorageDB_BootSANUnix
                                }

                                if (oClass.Get(intClass, "pnc") == "1")
                                {
                                    if (boolProd == true)
                                    {
                                        sbStorage.Append("<td>");
                                        sbStorage.Append("60 GB");
                                        sbStorage.Append("</td>");
                                        sbStorage.Append("<td>");
                                        sbStorage.Append("60 GB");
                                        sbStorage.Append("</td>");
                                        //dblTotal += 40.00;
                                    }
                                    if (boolQA == true)
                                    {
                                        sbStorage.Append("<td>");
                                        sbStorage.Append("60 GB");
                                        sbStorage.Append("</td>");
                                        sbStorage.Append("<td>");
                                        sbStorage.Append("60 GB");
                                        sbStorage.Append("</td>");
                                        //dblTotal += 40.00;
                                    }
                                    if (boolTest == true)
                                    {
                                        sbStorage.Append("<td>");
                                        sbStorage.Append("60 GB");
                                        sbStorage.Append("</td>");
                                        sbStorage.Append("<td>");
                                        sbStorage.Append("60 GB");
                                        sbStorage.Append("</td>");
                                        //dblTotal += 40.00;
                                    }
                                }
                                else
                                {
                                    if (boolProd == true)
                                    {
                                        sbStorage.Append("<td>");
                                        sbStorage.Append("50 GB");
                                        sbStorage.Append("</td>");
                                        sbStorage.Append("<td>");
                                        sbStorage.Append("50 GB");
                                        sbStorage.Append("</td>");
                                        //dblTotal += 50.00;
                                    }
                                    if (boolQA == true)
                                    {
                                        sbStorage.Append("<td>");
                                        sbStorage.Append("50 GB");
                                        sbStorage.Append("</td>");
                                        sbStorage.Append("<td>");
                                        sbStorage.Append("50 GB");
                                        sbStorage.Append("</td>");
                                        //dblTotal += 50.00;
                                    }
                                    if (boolTest == true)
                                    {
                                        sbStorage.Append("<td>");
                                        sbStorage.Append("50 GB");
                                        sbStorage.Append("</td>");
                                        sbStorage.Append("<td>");
                                        sbStorage.Append("50 GB");
                                        sbStorage.Append("</td>");
                                        //dblTotal += 50.00;
                                    }
                                }
                                if (boolProd == true)
                                {
                                    sbStorage.Append("<td>");
                                    //sbStorage.Append(strFirstLunReplicated == "0" ? "No" : "Yes");
                                    sbStorage.Append(intDR > 0 ? "Yes" : "No");
                                    sbStorage.Append("</td>");
                                    sbStorage.Append("<td>");
                                    sbStorage.Append("<select class=\"default\" style=\"width:50px;\"><option value=\"No\"");
                                    sbStorage.Append(intDR == 0 ? " selected" : "");
                                    sbStorage.Append(">No</option><option value=\"Yes\"");
                                    sbStorage.Append(intDR > 0 ? " selected" : "");
                                    sbStorage.Append(">Yes</option></select>");
                                    sbStorage.Append("</td>");
                                }
                                if (boolProd == true)
                                {
                                    sbStorage.Append("<td>No</td>");
                                    sbStorage.Append("<td>");
                                    sbStorage.Append("<select class=\"default\" style=\"width:50px;\"><option value=\"No\" selected>No</option><option value=\"Yes\">Yes</option></select></td>");
                                }
                                sbStorage.Append("</tr>");


                                // ADD DATA VOL "/opt" (10 GB hardcoded)
                                intRow++;
                                sbStorage.Append("<tr class=\"" + strBootVolumeBackground + "\">");
                                sbStorage.Append("<td>");
                                sbStorage.Append(intRow.ToString());
                                sbStorage.Append(" *</td>");
                                sbStorage.Append("<td>");
                                sbStorage.Append("rootvg");
                                sbStorage.Append("</td>");
                                sbStorage.Append("<td>");
                                sbStorage.Append(strFirstLunPerformance);
                                sbStorage.Append("</td>");

                                if (intUseLunSerial > 0)
                                {
                                    //Lun Serial # IsStorageDB_BootSANUnix
                                    sbStorage.Append("<td nowrap>---</td>");
                                    //Lun Serial # IsStorageDB_BootSANUnix
                                }

                                sbStorage.Append("<td>");
                                sbStorage.Append("10 GB");
                                sbStorage.Append("</td>");
                                sbStorage.Append("<td>");
                                sbStorage.Append("10 GB");
                                sbStorage.Append("</td>");
                                //dblTotal += 10.00;

                                if (boolProd == true)
                                {
                                    sbStorage.Append("<td>");
                                    //sbStorage.Append(strFirstLunReplicated == "0" ? "No" : "Yes");
                                    sbStorage.Append(intDR > 0 ? "Yes" : "No");
                                    sbStorage.Append("</td>");
                                    sbStorage.Append("<td>");
                                    sbStorage.Append("<select class=\"default\" style=\"width:50px;\"><option value=\"No\"");
                                    sbStorage.Append(intDR == 0 ? " selected" : "");
                                    sbStorage.Append(">No</option><option value=\"Yes\"");
                                    sbStorage.Append(intDR > 0 ? " selected" : "");
                                    sbStorage.Append(">Yes</option></select>");
                                    sbStorage.Append("</td>");
                                }
                                if (boolProd == true)
                                {
                                    sbStorage.Append("<td>No</td>");
                                    sbStorage.Append("<td>");
                                    sbStorage.Append("<select class=\"default\" style=\"width:50px;\"><option value=\"No\" selected>No</option><option value=\"Yes\">Yes</option></select></td>");
                                }
                                sbStorage.Append("</tr>");
                            }
                        }
                    }
                    foreach (DataRow drLun in dsLuns.Tables[0].Rows)
                    {
                        intRow++;
                        sbStorage.Append("<tr>");
                        sbStorage.Append("<td>");
                        sbStorage.Append(intRow.ToString());
                        sbStorage.Append("</td>");
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
                        if (boolOverride == true || oForecast.IsOSMidrange(_answerid) == true)
                        {
                            sbStorage.Append("<td>");
                            sbStorage.Append(strPath);
                            sbStorage.Append("</td>");
                        }
                        else
                        {
                            sbStorage.Append("<td>");
                            sbStorage.Append(strLetter);
                            sbStorage.Append(":");
                            sbStorage.Append(strPath);
                            sbStorage.Append("</td>");
                        }
                        sbStorage.Append("<td>");
                        sbStorage.Append(drLun["performance"].ToString());
                        sbStorage.Append("</td>");
                        if (intUseLunSerial > 0)
                        {
                            //Lun Serial #
                            sbStorage.Append("<td nowrap>");
                            if (_edit == true)
                            {
                                if (intUseLunSerial > 1)
                                    sbStorage.Append("<input type=\"text\" name=\"ValidTextbox\" class=\"default\" style=\"width:150px\" onblur=\"UpdateTextValue(this,'HDN_");
                                else
                                    sbStorage.Append("<input type=\"text\" class=\"default\" style=\"width:150px\" onblur=\"UpdateTextValue(this,'HDN_");
                                sbStorage.Append(drLun["id"].ToString());
                                sbStorage.Append("_L_SERIALNO');\" value=\"");
                                sbStorage.Append(drLun["serialNo"].ToString());
                                sbStorage.Append("\">");
                            }
                            else
                                sbStorage.Append(drLun["serialNo"].ToString());
                            sbStorage.Append(" </td>");

                            sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                            sbHidden.Append(drLun["id"].ToString());
                            sbHidden.Append("_L_SERIALNO\" id=\"HDN_");
                            sbHidden.Append(drLun["id"].ToString());
                            sbHidden.Append("_L_SERIALNO\" value=\"");
                            sbHidden.Append(drLun["serialNo"].ToString());
                            sbHidden.Append("\" />");
                            //Lun Serial #
                        }

                        if (boolProd == true)
                        {
                            sbStorage.Append("<td>");
                            sbStorage.Append(drLun["size"].ToString());
                            sbStorage.Append(" GB</td>");
                            sbStorage.Append("<td nowrap>");
                            if (_edit == true)
                            {
                                sbStorage.Append("<input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:50px\" onblur=\"UpdateText(this,'HDN_");
                                sbStorage.Append(drLun["id"].ToString());
                                sbStorage.Append("_L_SIZE');\" value=\"");
                                sbStorage.Append(drLun["actual_size"].ToString() == "-1" ? "0" : drLun["actual_size"].ToString());
                                sbStorage.Append("\">");
                            }
                            else
                                sbStorage.Append(drLun["actual_size"].ToString() == "-1" ? "0" : drLun["actual_size"].ToString());
                            sbStorage.Append(" GB</td>");
                            dblTotal += double.Parse(drLun["size"].ToString());
                        }
                        if (boolQA == true)
                        {
                            sbStorage.Append("<td>");
                            sbStorage.Append(drLun["size_qa"].ToString());
                            sbStorage.Append(" GB</td>");
                            sbStorage.Append("<td nowrap>");
                            if (_edit == true)
                            {
                                sbStorage.Append("<input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:50px\" onblur=\"UpdateText(this,'HDN_");
                                sbStorage.Append(drLun["id"].ToString());
                                sbStorage.Append("_L_SIZE_QA');\" value=\"");
                                sbStorage.Append(drLun["actual_size_qa"].ToString() == "-1" ? "0" : drLun["actual_size_qa"].ToString());
                                sbStorage.Append("\">");
                            }
                            else
                                sbStorage.Append(drLun["actual_size_qa"].ToString() == "-1" ? "0" : drLun["actual_size_qa"].ToString());
                            sbStorage.Append(" GB</td>");
                            dblTotal += double.Parse(drLun["size_qa"].ToString());
                        }
                        if (boolTest == true)
                        {
                            sbStorage.Append("<td>");
                            sbStorage.Append(drLun["size_test"].ToString());
                            sbStorage.Append(" GB</td>");
                            sbStorage.Append("<td nowrap>");
                            if (_edit == true)
                            {
                                sbStorage.Append("<input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:50px\" onblur=\"UpdateText(this,'HDN_");
                                sbStorage.Append(drLun["id"].ToString());
                                sbStorage.Append("_L_SIZE_TEST');\" value=\"");
                                sbStorage.Append(drLun["actual_size_test"].ToString() == "-1" ? "0" : drLun["actual_size_test"].ToString());
                                sbStorage.Append("\">");
                            }
                            else
                                sbStorage.Append(drLun["actual_size_test"].ToString() == "-1" ? "0" : drLun["actual_size_test"].ToString());
                            sbStorage.Append(" GB</td>");
                            dblTotal += double.Parse(drLun["size_test"].ToString());
                        }
                        if (boolProd == true)
                        {
                            sbStorage.Append("<td>");
                            sbStorage.Append(intDR > 0 ? "Yes" : "No");
                            sbStorage.Append("</td>");
                            sbStorage.Append("<td>");
                            if (_edit == true)
                            {
                                sbStorage.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'HDN_");
                                sbStorage.Append(drLun["id"].ToString());
                                sbStorage.Append("_L_REPLICATED');\" style=\"width:50px;\">");
                                sbStorage.Append("<option value=\"No\"");
                                sbStorage.Append(drLun["actual_replicated"].ToString() == "0" ? " selected" : "");
                                sbStorage.Append(">No</option>");
                                sbStorage.Append("<option value=\"Yes\"");
                                sbStorage.Append(drLun["actual_replicated"].ToString() == "1" ? " selected" : "");
                                sbStorage.Append(">Yes</option>");
                                sbStorage.Append("</select>");
                            }
                            else
                                sbStorage.Append(drLun["actual_replicated"].ToString() == "1" ? "Yes" : "No");
                            sbStorage.Append("</td>");
                        }
                        if (boolProd == true)
                        {
                            sbStorage.Append("<td>");
                            sbStorage.Append(drLun["high_availability"].ToString() == "0" ? "No" : "Yes (" + drLun["size"].ToString() + " GB)");
                            sbStorage.Append("</td>");
                            sbStorage.Append("<td>");
                            if (_edit == true)
                            {
                                sbStorage.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'HDN_");
                                sbStorage.Append(drLun["id"].ToString());
                                sbStorage.Append("_L_HIGH_AVAILABILITY');\" style=\"width:50px;\">");
                                sbStorage.Append("<option value=\"No\"");
                                sbStorage.Append(drLun["actual_high_availability"].ToString() == "0" ? " selected" : "");
                                sbStorage.Append(">No</option>");
                                sbStorage.Append("<option value=\"Yes\"");
                                sbStorage.Append(drLun["actual_high_availability"].ToString() == "1" ? " selected" : "");
                                sbStorage.Append(">Yes</option>");
                                sbStorage.Append("</select>");
                            }
                            else
                                sbStorage.Append(drLun["actual_high_availability"].ToString() == "1" ? "Yes" : "No");
                            sbStorage.Append("</td>");
                        }
                        sbStorage.Append("</tr>");
                        sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                        sbHidden.Append(drLun["id"].ToString());
                        sbHidden.Append("_L_SIZE\" id=\"HDN_");
                        sbHidden.Append(drLun["id"].ToString());
                        sbHidden.Append("_L_SIZE\" value=\"");
                        sbHidden.Append(drLun["actual_size"].ToString() == "-1" ? "0" : drLun["actual_size"].ToString());
                        sbHidden.Append("\" />");
                        sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                        sbHidden.Append(drLun["id"].ToString());
                        sbHidden.Append("_L_SIZE_QA\" id=\"HDN_");
                        sbHidden.Append(drLun["id"].ToString());
                        sbHidden.Append("_L_SIZE_QA\" value=\"");
                        sbHidden.Append(drLun["actual_size_qa"].ToString() == "-1" ? "0" : drLun["actual_size_qa"].ToString());
                        sbHidden.Append("\" />");
                        sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                        sbHidden.Append(drLun["id"].ToString());
                        sbHidden.Append("_L_SIZE_TEST\" id=\"HDN_");
                        sbHidden.Append(drLun["id"].ToString());
                        sbHidden.Append("_L_SIZE_TEST\" value=\"");
                        sbHidden.Append(drLun["actual_size_test"].ToString() == "-1" ? "0" : drLun["actual_size_test"].ToString());
                        sbHidden.Append("\" />");
                        sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                        sbHidden.Append(drLun["id"].ToString());
                        sbHidden.Append("_L_REPLICATED\" id=\"HDN_");
                        sbHidden.Append(drLun["id"].ToString());
                        sbHidden.Append("_L_REPLICATED\" value=\"");
                        sbHidden.Append(drLun["actual_replicated"].ToString() == "1" ? "Yes" : "No");
                        sbHidden.Append("\" />");
                        sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                        sbHidden.Append(drLun["id"].ToString());
                        sbHidden.Append("_L_HIGH_AVAILABILITY\" id=\"HDN_");
                        sbHidden.Append(drLun["id"].ToString());
                        sbHidden.Append("_L_HIGH_AVAILABILITY\" value=\"");
                        sbHidden.Append(drLun["actual_high_availability"].ToString() == "1" ? "Yes" : "No");
                        sbHidden.Append("\" />");
                        DataSet dsPoints = GetMountPoints(Int32.Parse(drLun["id"].ToString()));
                        int intPoint = 0;
                        foreach (DataRow drPoint in dsPoints.Tables[0].Rows)
                        {
                            intRow++;
                            intPoint++;
                            sbStorage.Append("<tr>");
                            sbStorage.Append("<td>");
                            sbStorage.Append(intRow.ToString());
                            sbStorage.Append("</td>");
                            strPath = drPoint["path"].ToString();
                            if (boolOverride == true || oForecast.IsOSMidrange(_answerid) == true)
                            {
                                sbStorage.Append("<td>");
                                sbStorage.Append(strPath);
                                sbStorage.Append("</td>");
                            }
                            else
                            {
                                sbStorage.Append("<td>");
                                sbStorage.Append(strLetter);
                                sbStorage.Append(":\\SH");
                                sbStorage.Append(drLun["driveid"].ToString());
                                sbStorage.Append("VOL");
                                sbStorage.Append(intPoint < 10 ? "0" : "");
                                sbStorage.Append(intPoint.ToString());
                                sbStorage.Append("</td>");
                            }
                            sbStorage.Append("<td>");
                            sbStorage.Append(drPoint["performance"].ToString());
                            sbStorage.Append("</td>");

                            if (intUseLunSerial > 0)
                            {
                                //Mount Point Serial #
                                sbStorage.Append("<td nowrap>");
                                if (_edit == true)
                                {
                                    if (intUseLunSerial > 1)
                                        sbStorage.Append("<input type=\"text\" name=\"ValidTextbox\" class=\"default\" style=\"width:150px\" onblur=\"UpdateTextValue(this,'HDN_");
                                    else
                                        sbStorage.Append("<input type=\"text\" class=\"default\" style=\"width:150px\" onblur=\"UpdateTextValue(this,'HDN_");
                                    sbStorage.Append(drPoint["id"].ToString());
                                    sbStorage.Append("_M_SERIALNO');\" value=\"");
                                    sbStorage.Append(drPoint["serialNo"].ToString());
                                    sbStorage.Append("\">");
                                }
                                else
                                    sbStorage.Append(drPoint["serialNo"].ToString());
                                sbStorage.Append(" </td>");

                                //Mount Point Serial # Hdntextbox
                                sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                sbHidden.Append(drPoint["id"].ToString());
                                sbHidden.Append("_M_SERIALNO\" id=\"HDN_");
                                sbHidden.Append(drLun["id"].ToString());
                                sbHidden.Append("_M_SERIALNO\" value=\"");
                                sbHidden.Append(drPoint["serialNo"].ToString());
                                sbHidden.Append("\" />");
                                //Mount Point Serial #
                            }
                            if (boolProd == true)
                            {
                                sbStorage.Append("<td>");
                                sbStorage.Append(drPoint["size"].ToString());
                                sbStorage.Append(" GB</td>");
                                sbStorage.Append("<td nowrap>");
                                if (_edit == true)
                                {
                                    sbStorage.Append("<input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:50px\" onblur=\"UpdateText(this,'HDN_");
                                    sbStorage.Append(drPoint["id"].ToString());
                                    sbStorage.Append("_M_SIZE');\" value=\"");
                                    sbStorage.Append(drPoint["actual_size"].ToString() == "-1" ? "0" : drPoint["actual_size"].ToString());
                                    sbStorage.Append("\">");
                                }
                                else
                                    sbStorage.Append(drPoint["actual_size"].ToString() == "-1" ? "0" : drPoint["actual_size"].ToString());
                                sbStorage.Append(" GB</td>");
                                dblTotal += double.Parse(drPoint["size"].ToString());
                            }
                            if (boolQA == true)
                            {
                                sbStorage.Append("<td>");
                                sbStorage.Append(drPoint["size_qa"].ToString());
                                sbStorage.Append(" GB</td>");
                                sbStorage.Append("<td nowrap>");
                                if (_edit == true)
                                {
                                    sbStorage.Append("<input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:50px\" onblur=\"UpdateText(this,'HDN_");
                                    sbStorage.Append(drPoint["id"].ToString());
                                    sbStorage.Append("_M_SIZE_QA');\" value=\"");
                                    sbStorage.Append(drPoint["actual_size_qa"].ToString() == "-1" ? "0" : drPoint["actual_size_qa"].ToString());
                                    sbStorage.Append("\">");
                                }
                                else
                                    sbStorage.Append(drPoint["actual_size_qa"].ToString() == "-1" ? "0" : drPoint["actual_size_qa"].ToString());
                                sbStorage.Append(" GB</td>");
                                dblTotal += double.Parse(drPoint["size_qa"].ToString());
                            }
                            if (boolTest == true)
                            {
                                sbStorage.Append("<td>");
                                sbStorage.Append(drPoint["size_test"].ToString());
                                sbStorage.Append(" GB</td>");
                                sbStorage.Append("<td nowrap>");
                                if (_edit == true)
                                {
                                    sbStorage.Append("<input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:50px\" onblur=\"UpdateText(this,'HDN_");
                                    sbStorage.Append(drPoint["id"].ToString());
                                    sbStorage.Append("_M_SIZE_TEST');\" value=\"");
                                    sbStorage.Append(drPoint["actual_size_test"].ToString() == "-1" ? "0" : drPoint["actual_size_test"].ToString());
                                    sbStorage.Append("\">");
                                }
                                else
                                    sbStorage.Append(drPoint["actual_size_test"].ToString() == "-1" ? "0" : drPoint["actual_size_test"].ToString());
                                sbStorage.Append(" GB</td>");
                                dblTotal += double.Parse(drPoint["size_test"].ToString());
                            }
                            if (boolProd == true)
                            {
                                sbStorage.Append("<td>");
                                sbStorage.Append(intDR > 0 ? "Yes" : "No");
                                sbStorage.Append("</td>");
                                sbStorage.Append("<td>");
                                if (_edit == true)
                                {
                                    sbStorage.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'HDN_");
                                    sbStorage.Append(drPoint["id"].ToString());
                                    sbStorage.Append("_M_REPLICATED');\" style=\"width:50px;\">");
                                    sbStorage.Append("<option value=\"No\"");
                                    sbStorage.Append(drPoint["actual_replicated"].ToString() == "0" ? " selected" : "");
                                    sbStorage.Append(">No</option>");
                                    sbStorage.Append("<option value=\"Yes\"");
                                    sbStorage.Append(drPoint["actual_replicated"].ToString() == "1" ? " selected" : "");
                                    sbStorage.Append(">Yes</option>");
                                    sbStorage.Append("</select>");
                                }
                                else
                                    sbStorage.Append(drPoint["actual_replicated"].ToString() == "1" ? "Yes" : "No");
                                sbStorage.Append("</td>");
                            }
                            if (boolProd == true)
                            {
                                sbStorage.Append("<td>");
                                sbStorage.Append(drPoint["high_availability"].ToString() == "0" ? "No" : "Yes (" + drPoint["size"].ToString() + " GB)");
                                sbStorage.Append("</td>");
                                sbStorage.Append("<td>");
                                if (_edit == true)
                                {
                                    sbStorage.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'HDN_");
                                    sbStorage.Append(drPoint["id"].ToString());
                                    sbStorage.Append("_M_HIGH_AVAILABILITY');\" style=\"width:50px;\">");
                                    sbStorage.Append("<option value=\"No\"");
                                    sbStorage.Append(drPoint["actual_high_availability"].ToString() == "0" ? " selected" : "");
                                    sbStorage.Append(">No</option>");
                                    sbStorage.Append("<option value=\"Yes\"");
                                    sbStorage.Append(drPoint["actual_high_availability"].ToString() == "1" ? " selected" : "");
                                    sbStorage.Append(">Yes</option>");
                                    sbStorage.Append("</select>");
                                }
                                else
                                    sbStorage.Append(drPoint["actual_high_availability"].ToString() == "1" ? "Yes" : "No");
                                sbStorage.Append("</td>");
                            }
                            sbStorage.Append("</tr>");
                            sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                            sbHidden.Append(drPoint["id"].ToString());
                            sbHidden.Append("_M_SIZE\" id=\"HDN_");
                            sbHidden.Append(drPoint["id"].ToString());
                            sbHidden.Append("_M_SIZE\" value=\"");
                            sbHidden.Append(drPoint["actual_size"].ToString() == "-1" ? "0" : drPoint["actual_size"].ToString());
                            sbHidden.Append("\" />");
                            sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                            sbHidden.Append(drPoint["id"].ToString());
                            sbHidden.Append("_M_SIZE_QA\" id=\"HDN_");
                            sbHidden.Append(drPoint["id"].ToString());
                            sbHidden.Append("_M_SIZE_QA\" value=\"");
                            sbHidden.Append(drPoint["actual_size_qa"].ToString() == "-1" ? "0" : drPoint["actual_size_qa"].ToString());
                            sbHidden.Append("\" />");
                            sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                            sbHidden.Append(drPoint["id"].ToString());
                            sbHidden.Append("_M_SIZE_TEST\" id=\"HDN_");
                            sbHidden.Append(drPoint["id"].ToString());
                            sbHidden.Append("_M_SIZE_TEST\" value=\"");
                            sbHidden.Append(drPoint["actual_size_test"].ToString() == "-1" ? "0" : drPoint["actual_size_test"].ToString());
                            sbHidden.Append("\" />");
                            sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                            sbHidden.Append(drPoint["id"].ToString());
                            sbHidden.Append("_M_REPLICATED\" id=\"HDN_");
                            sbHidden.Append(drPoint["id"].ToString());
                            sbHidden.Append("_M_REPLICATED\" value=\"");
                            sbHidden.Append(drPoint["actual_replicated"].ToString() == "1" ? "Yes" : "No");
                            sbHidden.Append("\" />");
                            sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                            sbHidden.Append(drPoint["id"].ToString());
                            sbHidden.Append("_M_HIGH_AVAILABILITY\" id=\"HDN_");
                            sbHidden.Append(drPoint["id"].ToString());
                            sbHidden.Append("_M_HIGH_AVAILABILITY\" value=\"");
                            sbHidden.Append(drPoint["actual_high_availability"].ToString() == "1" ? "Yes" : "No");
                            sbHidden.Append("\" />");
                        }
                    }
                    if (intCluster > 0)
                    {
                        bool boolAddShared = false;
                        if (intCount < dsAnswer.Tables[0].Rows.Count)
                        {
                            int intNextCluster = Int32.Parse(dsAnswer.Tables[0].Rows[intCount]["clusterid"].ToString());
                            if (intNextCluster != intCluster)
                                boolAddShared = true;
                        }
                        else
                        {
                            boolAddShared = true;
                        }
                        if (boolAddShared == true)
                        {
                            // Get Shared Storage
                            strStorageSharedHeader = "<br/><br/><table width=\"100%\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\"><tr><td colspan=\"2\" class=\"header\">SHARED Storage Configuration (" + strShared + ")</td></tr></table>";
                            dsLuns = GetLunsClusterShared(_answerid, intCluster);
                            intRow = 0;
                            boolOther = false;
                            foreach (DataRow drLun in dsLuns.Tables[0].Rows)
                            {
                                boolOther = !boolOther;
                                intRow++;
                                sbStorageShared.Append("<tr>");
                                sbStorageShared.Append("<td>");
                                sbStorageShared.Append(intRow.ToString());
                                sbStorageShared.Append("</td>");
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
                                if (boolOverride == true || oForecast.IsOSMidrange(_answerid) == true)
                                {
                                    sbStorageShared.Append("<td>");
                                    sbStorageShared.Append(strPath);
                                    sbStorageShared.Append("</td>");
                                }
                                else
                                {
                                    sbStorageShared.Append("<td>");
                                    sbStorageShared.Append(strLetter);
                                    sbStorageShared.Append(":");
                                    sbStorageShared.Append(strPath);
                                    sbStorageShared.Append("</td>");
                                }
                                sbStorageShared.Append("<td>");
                                sbStorageShared.Append(drLun["performance"].ToString());
                                sbStorageShared.Append("</td>");
                                if (intUseLunSerial > 0)
                                {
                                    //Shared Lun Serial #
                                    sbStorageShared.Append("<td nowrap>");
                                    if (_edit == true)
                                    {
                                        if (intUseLunSerial > 1)
                                            sbStorageShared.Append("<input type=\"text\" name=\"ValidTextbox\" class=\"default\" style=\"width:150px\" onblur=\"UpdateTextValue(this,'HDN_");
                                        else
                                            sbStorageShared.Append("<input type=\"text\" class=\"default\" style=\"width:150px\" onblur=\"UpdateTextValue(this,'HDN_");
                                        sbStorageShared.Append(drLun["id"].ToString());
                                        sbStorageShared.Append("_L_SERIALNO');\" value=\"");
                                        sbStorageShared.Append(drLun["serialNo"].ToString());
                                        sbStorageShared.Append("\">");
                                    }
                                    else
                                        sbStorageShared.Append(drLun["serialNo"].ToString());
                                    sbStorageShared.Append(" </td>");

                                    //Shared Lun Serial # Hdntextbox
                                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                    sbHidden.Append(drLun["id"].ToString());
                                    sbHidden.Append("_L_SERIALNO\" id=\"HDN_");
                                    sbHidden.Append(drLun["id"].ToString());
                                    sbHidden.Append("_L_SERIALNO\" value=\"");
                                    sbHidden.Append(drLun["serialNo"].ToString());
                                    sbHidden.Append("\" />");
                                    //Shared Lun Serial #
                                }
                                if (boolProd == true)
                                {
                                    sbStorageShared.Append("<td>");
                                    sbStorageShared.Append(drLun["size"].ToString());
                                    sbStorageShared.Append(" GB</td>");
                                    sbStorageShared.Append("<td nowrap>");
                                    if (_edit == true)
                                    {
                                        sbStorageShared.Append("<input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:50px\" onblur=\"UpdateText(this,'HDN_");
                                        sbStorageShared.Append(drLun["id"].ToString());
                                        sbStorageShared.Append("_L_SIZE');\" value=\"");
                                        sbStorageShared.Append(drLun["actual_size"].ToString() == "-1" ? "0" : drLun["actual_size"].ToString());
                                        sbStorageShared.Append("\">");
                                    }
                                    else
                                        sbStorageShared.Append(drLun["actual_size"].ToString() == "-1" ? "0" : drLun["actual_size"].ToString());
                                    sbStorageShared.Append(" GB</td>");
                                    //dblTotal += double.Parse(drLun["size"].ToString());
                                }
                                if (boolQA == true)
                                {
                                    sbStorageShared.Append("<td>");
                                    sbStorageShared.Append(drLun["size_qa"].ToString());
                                    sbStorageShared.Append(" GB</td>");
                                    sbStorageShared.Append("<td nowrap>");
                                    if (_edit == true)
                                    {
                                        sbStorageShared.Append("<input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:50px\" onblur=\"UpdateText(this,'HDN_");
                                        sbStorageShared.Append(drLun["id"].ToString());
                                        sbStorageShared.Append("_L_SIZE_QA');\" value=\"");
                                        sbStorageShared.Append(drLun["actual_size_qa"].ToString() == "-1" ? "0" : drLun["actual_size_qa"].ToString());
                                        sbStorageShared.Append("\">");
                                    }
                                    else
                                        sbStorageShared.Append(drLun["actual_size_qa"].ToString() == "-1" ? "0" : drLun["actual_size_qa"].ToString());
                                    sbStorageShared.Append(" GB</td>");
                                    //dblTotal += double.Parse(drLun["size_qa"].ToString());
                                }
                                if (boolTest == true)
                                {
                                    sbStorageShared.Append("<td>");
                                    sbStorageShared.Append(drLun["size_test"].ToString());
                                    sbStorageShared.Append(" GB</td>");
                                    sbStorageShared.Append("<td nowrap>");
                                    if (_edit == true)
                                    {
                                        sbStorageShared.Append("<input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:50px\" onblur=\"UpdateText(this,'HDN_");
                                        sbStorageShared.Append(drLun["id"].ToString());
                                        sbStorageShared.Append("_L_SIZE_TEST');\" value=\"");
                                        sbStorageShared.Append(drLun["actual_size_test"].ToString() == "-1" ? "0" : drLun["actual_size_test"].ToString());
                                        sbStorageShared.Append("\">");
                                    }
                                    else
                                        sbStorageShared.Append(drLun["actual_size_test"].ToString() == "-1" ? "0" : drLun["actual_size_test"].ToString());
                                    sbStorageShared.Append(" GB</td>");
                                    //dblTotal += double.Parse(drLun["size_test"].ToString());
                                }
                                if (boolProd == true)
                                {
                                    sbStorageShared.Append("<td>");
                                    sbStorageShared.Append(intDR > 0 ? "Yes" : "No");
                                    sbStorageShared.Append("</td>");
                                    sbStorageShared.Append("<td>");
                                    if (_edit == true)
                                    {
                                        sbStorageShared.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'HDN_");
                                        sbStorageShared.Append(drLun["id"].ToString());
                                        sbStorageShared.Append("_L_REPLICATED');\" style=\"width:50px;\">");
                                        sbStorageShared.Append("<option value=\"No\"");
                                        sbStorageShared.Append(drLun["actual_replicated"].ToString() == "0" ? " selected" : "");
                                        sbStorageShared.Append(">No</option>");
                                        sbStorageShared.Append("<option value=\"Yes\"");
                                        sbStorageShared.Append(drLun["actual_replicated"].ToString() == "1" ? " selected" : "");
                                        sbStorageShared.Append(">Yes</option>");
                                        sbStorageShared.Append("</select>");
                                    }
                                    else
                                        sbStorageShared.Append(drLun["actual_replicated"].ToString() == "1" ? "Yes" : "No");
                                    sbStorageShared.Append("</td>");
                                }
                                if (boolProd == true)
                                {
                                    sbStorageShared.Append("<td>");
                                    sbStorageShared.Append(drLun["high_availability"].ToString() == "0" ? "No" : "Yes (" + drLun["size"].ToString() + " GB)");
                                    sbStorageShared.Append("</td>");
                                    sbStorageShared.Append("<td>");
                                    if (_edit == true)
                                    {
                                        sbStorageShared.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'HDN_");
                                        sbStorageShared.Append(drLun["id"].ToString());
                                        sbStorageShared.Append("_L_HIGH_AVAILABILITY');\" style=\"width:50px;\">");
                                        sbStorageShared.Append("<option value=\"No\"");
                                        sbStorageShared.Append(drLun["actual_high_availability"].ToString() == "0" ? " selected" : "");
                                        sbStorageShared.Append(">No</option>");
                                        sbStorageShared.Append("<option value=\"Yes\"");
                                        sbStorageShared.Append(drLun["actual_high_availability"].ToString() == "1" ? " selected" : "");
                                        sbStorageShared.Append(">Yes</option>");
                                        sbStorageShared.Append("</select>");
                                    }
                                    else
                                        sbStorageShared.Append(drLun["actual_high_availability"].ToString() == "1" ? "Yes" : "No");
                                    sbStorageShared.Append("</td>");
                                }
                                sbStorageShared.Append("</tr>");
                                sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                sbHidden.Append(drLun["id"].ToString());
                                sbHidden.Append("_L_SIZE\" id=\"HDN_");
                                sbHidden.Append(drLun["id"].ToString());
                                sbHidden.Append("_L_SIZE\" value=\"");
                                sbHidden.Append(drLun["actual_size"].ToString() == "-1" ? "0" : drLun["actual_size"].ToString());
                                sbHidden.Append("\" />");
                                sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                sbHidden.Append(drLun["id"].ToString());
                                sbHidden.Append("_L_SIZE_QA\" id=\"HDN_");
                                sbHidden.Append(drLun["id"].ToString());
                                sbHidden.Append("_L_SIZE_QA\" value=\"");
                                sbHidden.Append(drLun["actual_size_qa"].ToString() == "-1" ? "0" : drLun["actual_size_qa"].ToString());
                                sbHidden.Append("\" />");
                                sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                sbHidden.Append(drLun["id"].ToString());
                                sbHidden.Append("_L_SIZE_TEST\" id=\"HDN_");
                                sbHidden.Append(drLun["id"].ToString());
                                sbHidden.Append("_L_SIZE_TEST\" value=\"");
                                sbHidden.Append(drLun["actual_size_test"].ToString() == "-1" ? "0" : drLun["actual_size_test"].ToString());
                                sbHidden.Append("\" />");
                                sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                sbHidden.Append(drLun["id"].ToString());
                                sbHidden.Append("_L_REPLICATED\" id=\"HDN_");
                                sbHidden.Append(drLun["id"].ToString());
                                sbHidden.Append("_L_REPLICATED\" value=\"");
                                sbHidden.Append(drLun["actual_replicated"].ToString() == "1" ? "Yes" : "No");
                                sbHidden.Append("\" />");
                                sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                sbHidden.Append(drLun["id"].ToString());
                                sbHidden.Append("_L_HIGH_AVAILABILITY\" id=\"HDN_");
                                sbHidden.Append(drLun["id"].ToString());
                                sbHidden.Append("_L_HIGH_AVAILABILITY\" value=\"");
                                sbHidden.Append(drLun["actual_high_availability"].ToString() == "1" ? "Yes" : "No");
                                sbHidden.Append("\" />");
                                DataSet dsPoints = GetMountPoints(Int32.Parse(drLun["id"].ToString()));
                                int intPoint = 0;
                                foreach (DataRow drPoint in dsPoints.Tables[0].Rows)
                                {
                                    boolOther = !boolOther;
                                    intRow++;
                                    intPoint++;
                                    sbStorageShared.Append("<tr>");
                                    sbStorageShared.Append("<td>");
                                    sbStorageShared.Append(intRow.ToString());
                                    sbStorageShared.Append("</td>");
                                    strPath = drPoint["path"].ToString();
                                    if (boolOverride == true || oForecast.IsOSMidrange(_answerid) == true)
                                    {
                                        sbStorageShared.Append("<td>");
                                        sbStorageShared.Append(strPath);
                                        sbStorageShared.Append("</td>");
                                    }
                                    else
                                    {
                                        sbStorageShared.Append("<td>");
                                        sbStorageShared.Append(strLetter);
                                        sbStorageShared.Append(":\\SH");
                                        sbStorageShared.Append(drLun["driveid"].ToString());
                                        sbStorageShared.Append("VOL");
                                        sbStorageShared.Append(intPoint < 10 ? "0" : "");
                                        sbStorageShared.Append(intPoint.ToString());
                                        sbStorageShared.Append("</td>");
                                    }
                                    sbStorageShared.Append("<td>");
                                    sbStorageShared.Append(drPoint["performance"].ToString());
                                    sbStorageShared.Append("</td>");

                                    if (intUseLunSerial > 0)
                                    {
                                        //Shared Mount Point Serial #
                                        sbStorageShared.Append("<td nowrap>");
                                        if (_edit == true)
                                        {
                                            if (intUseLunSerial > 1)
                                                sbStorageShared.Append("<input type=\"text\" name=\"ValidTextbox\" class=\"default\" style=\"width:150px\" onblur=\"UpdateTextValue(this,'HDN_");
                                            else
                                                sbStorageShared.Append("<input type=\"text\" class=\"default\" style=\"width:150px\" onblur=\"UpdateTextValue(this,'HDN_");
                                            sbStorageShared.Append(drPoint["id"].ToString());
                                            sbStorageShared.Append("_M_SERIALNO');\" value=\"");
                                            sbStorageShared.Append(drPoint["serialNo"].ToString());
                                            sbStorageShared.Append("\">");
                                        }
                                        else
                                            sbStorageShared.Append(drPoint["serialNo"].ToString());
                                        sbStorageShared.Append(" </td>");

                                        //Shared Mount Point Serial # Hdntextbox
                                        sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                        sbHidden.Append(drPoint["id"].ToString());
                                        sbHidden.Append("_M_SERIALNO\" id=\"HDN_");
                                        sbHidden.Append(drLun["id"].ToString());
                                        sbHidden.Append("_M_SERIALNO\" value=\"");
                                        sbHidden.Append(drPoint["serialNo"].ToString());
                                        sbHidden.Append("\" />");
                                        //Shared Mount Point Serial #
                                    }

                                    if (boolProd == true)
                                    {
                                        sbStorageShared.Append("<td>");
                                        sbStorageShared.Append(drPoint["size"].ToString());
                                        sbStorageShared.Append(" GB</td>");
                                        sbStorageShared.Append("<td nowrap>");
                                        if (_edit == true)
                                        {
                                            sbStorageShared.Append("<input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:50px\" onblur=\"UpdateText(this,'HDN_");
                                            sbStorageShared.Append(drPoint["id"].ToString());
                                            sbStorageShared.Append("_M_SIZE');\" value=\"");
                                            sbStorageShared.Append(drPoint["actual_size"].ToString() == "-1" ? "0" : drPoint["actual_size"].ToString());
                                            sbStorageShared.Append("\">");
                                        }
                                        else
                                            sbStorageShared.Append(drPoint["actual_size"].ToString() == "-1" ? "0" : drPoint["actual_size"].ToString());
                                        sbStorageShared.Append(" GB</td>");
                                        //dblTotal += double.Parse(drPoint["size"].ToString());
                                    }
                                    if (boolQA == true)
                                    {
                                        sbStorageShared.Append("<td>");
                                        sbStorageShared.Append(drPoint["size_qa"].ToString());
                                        sbStorageShared.Append(" GB</td>");
                                        sbStorageShared.Append("<td nowrap>");
                                        if (_edit == true)
                                        {
                                            sbStorageShared.Append("<input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:50px\" onblur=\"UpdateText(this,'HDN_");
                                            sbStorageShared.Append(drPoint["id"].ToString());
                                            sbStorageShared.Append("_M_SIZE_QA');\" value=\"");
                                            sbStorageShared.Append(drPoint["actual_size_qa"].ToString() == "-1" ? "0" : drPoint["actual_size_qa"].ToString());
                                            sbStorageShared.Append("\">");
                                        }
                                        else
                                            sbStorageShared.Append(drPoint["actual_size_qa"].ToString() == "-1" ? "0" : drPoint["actual_size_qa"].ToString());
                                        sbStorageShared.Append(" GB</td>");
                                        //dblTotal += double.Parse(drPoint["size_qa"].ToString());
                                    }
                                    if (boolTest == true)
                                    {
                                        sbStorageShared.Append("<td>");
                                        sbStorageShared.Append(drPoint["size_test"].ToString());
                                        sbStorageShared.Append(" GB</td>");
                                        sbStorageShared.Append("<td nowrap>");
                                        if (_edit == true)
                                        {
                                            sbStorageShared.Append("<input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:50px\" onblur=\"UpdateText(this,'HDN_");
                                            sbStorageShared.Append(drPoint["id"].ToString());
                                            sbStorageShared.Append("_M_SIZE_TEST');\" value=\"");
                                            sbStorageShared.Append(drPoint["actual_size_test"].ToString() == "-1" ? "0" : drPoint["actual_size_test"].ToString());
                                            sbStorageShared.Append("\">");
                                        }
                                        else
                                            sbStorageShared.Append(drPoint["actual_size_test"].ToString() == "-1" ? "0" : drPoint["actual_size_test"].ToString());
                                        sbStorageShared.Append(" GB</td>");
                                        //dblTotal += double.Parse(drPoint["size_test"].ToString());
                                    }
                                    if (boolProd == true)
                                    {
                                        sbStorageShared.Append("<td>");
                                        sbStorageShared.Append(intDR > 0 ? "Yes" : "No");
                                        sbStorageShared.Append("</td>");
                                        sbStorageShared.Append("<td>");
                                        if (_edit == true)
                                        {
                                            sbStorageShared.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'HDN_");
                                            sbStorageShared.Append(drPoint["id"].ToString());
                                            sbStorageShared.Append("_M_REPLICATED');\" style=\"width:50px;\">");
                                            sbStorageShared.Append("<option value=\"No\"");
                                            sbStorageShared.Append(drPoint["actual_replicated"].ToString() == "0" ? " selected" : "");
                                            sbStorageShared.Append(">No</option>");
                                            sbStorageShared.Append("<option value=\"Yes\"");
                                            sbStorageShared.Append(drPoint["actual_replicated"].ToString() == "1" ? " selected" : "");
                                            sbStorageShared.Append(">Yes</option>");
                                            sbStorageShared.Append("</select>");
                                        }
                                        else
                                            sbStorageShared.Append(drPoint["actual_replicated"].ToString() == "1" ? "Yes" : "No");
                                        sbStorageShared.Append("</td>");
                                    }
                                    if (boolProd == true)
                                    {
                                        sbStorageShared.Append("<td>");
                                        sbStorageShared.Append(drPoint["high_availability"].ToString() == "0" ? "No" : "Yes (" + drPoint["size"].ToString() + " GB)");
                                        sbStorageShared.Append("</td>");
                                        sbStorageShared.Append("<td>");
                                        if (_edit == true)
                                        {
                                            sbStorageShared.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'HDN_");
                                            sbStorageShared.Append(drPoint["id"].ToString());
                                            sbStorageShared.Append("_M_HIGH_AVAILABILITY');\" style=\"width:50px;\">");
                                            sbStorageShared.Append("<option value=\"No\"");
                                            sbStorageShared.Append(drPoint["actual_high_availability"].ToString() == "0" ? " selected" : "");
                                            sbStorageShared.Append(">No</option>");
                                            sbStorageShared.Append("<option value=\"Yes\"");
                                            sbStorageShared.Append(drPoint["actual_high_availability"].ToString() == "1" ? " selected" : "");
                                            sbStorageShared.Append(">Yes</option>");
                                            sbStorageShared.Append("</select>");
                                        }
                                        else
                                            sbStorageShared.Append(drPoint["actual_high_availability"].ToString() == "1" ? "Yes" : "No");
                                        sbStorageShared.Append("</td>");
                                    }
                                    sbStorageShared.Append("</tr>");
                                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("_M_SIZE\" id=\"HDN_");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("_M_SIZE\" value=\"");
                                    sbHidden.Append(drPoint["actual_size"].ToString() == "-1" ? "0" : drPoint["actual_size"].ToString());
                                    sbHidden.Append("\" />");
                                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("_M_SIZE_QA\" id=\"HDN_");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("_M_SIZE_QA\" value=\"");
                                    sbHidden.Append(drPoint["actual_size_qa"].ToString() == "-1" ? "0" : drPoint["actual_size_qa"].ToString());
                                    sbHidden.Append("\" />");
                                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("_M_SIZE_TEST\" id=\"HDN_");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("_M_SIZE_TEST\" value=\"");
                                    sbHidden.Append(drPoint["actual_size_test"].ToString() == "-1" ? "0" : drPoint["actual_size_test"].ToString());
                                    sbHidden.Append("\" />");
                                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("_M_REPLICATED\" id=\"HDN_");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("_M_REPLICATED\" value=\"");
                                    sbHidden.Append(drPoint["actual_replicated"].ToString() == "1" ? "Yes" : "No");
                                    sbHidden.Append("\" />");
                                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("_M_HIGH_AVAILABILITY\" id=\"HDN_");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("_M_HIGH_AVAILABILITY\" value=\"");
                                    sbHidden.Append(drPoint["actual_high_availability"].ToString() == "1" ? "Yes" : "No");
                                    sbHidden.Append("\" />");
                                }
                            }
                            strShared = "";
                        }
                    }

                    if (oModelsProperties.IsSUNVirtual(intModel) == true)
                    {
                        int intSubApplication = 0;
                        if (oForecast.GetAnswer(_answerid, "subapplicationid") != "")
                            Int32.TryParse(oForecast.GetAnswer(_answerid, "subapplicationid"), out intSubApplication);
                        bool boolSVEClustered = false;
                        if (intSubApplication > 0)
                            boolSVEClustered = (oServerName.GetSubApplication(intSubApplication, "code") == "Z");
                        if (boolSVEClustered == true)
                        {
                            // SVE Clustered requires only shared storage (all non-shared storage should be discarded)
                            sbStorage.Length = 0;
                        }
                    }
                    if (sbStorage.ToString() != "" || sbStorageShared.ToString() != "")
                    {
                        StringBuilder sbStorageTitle = new StringBuilder();
                        sbStorageTitle.Append("<td>LUN #</td>");
                        sbStorageTitle.Append("<td>Path</td>");
                        sbStorageTitle.Append("<td>Performance</td>");
                        if (intUseLunSerial > 0)
                        {
                            sbStorageTitle.Append("<td>LUN Serial #</td>");
                        }

                        if (boolProd == true)
                        {
                            sbStorageTitle.Append("<td>Requested<br/>Size in Prod (GB)</td>");
                            sbStorageTitle.Append("<td>Actual<br/>Size in Prod (GB)</td>");
                        }
                        if (boolQA == true)
                        {
                            sbStorageTitle.Append("<td>Requested<br/>Size in QA (GB)</td>");
                            sbStorageTitle.Append("<td>Actual<br/>Size in QA (GB)</td>");
                        }
                        if (boolTest == true)
                        {
                            sbStorageTitle.Append("<td>Requested<br/>Size in Test (GB)</td>");
                            sbStorageTitle.Append("<td>Actual<br/>Size in Test (GB)</td>");
                        }
                        if (boolProd == true)
                        {
                            sbStorageTitle.Append("<td>Requested<br/>Replicated</td>");
                            sbStorageTitle.Append("<td>Actual<br/>Replicated</td>");
                        }
                        if (boolProd == true)
                        {
                            sbStorageTitle.Append("<td>Requested<br/>High Avail</td>");
                            sbStorageTitle.Append("<td>Actual<br/>High Avail</td>");
                        }
                        if (sbStorage.ToString() != "")
                        {
                            sbStorage.Insert(0, "<tr bgcolor=\"#EEEEEE\" class=\"bold\">" + sbStorageTitle.ToString() + "</tr>");
                            if (intUseLunSerial == 1)
                            {
                                sbStorage.Insert(0, "<tr><td colspan=\"3\"></td><td colspan=\"20\"><table cellpadding=\"3\" cellspacing=\"2\" border=\"0\" style=\"border:solid 1px #999999\" bgcolor=\"#f9f9f9\"><tr><td rowspan=\"2\"><img src=\"/images/new.gif\" border=\"0\" align=\"absmiddle\"/></td><td><b>NOTE:</b> The LUN serial number is now available to be entered into ClearView.</td></tr><tr><td>It is highly recommended that you copy and paste this information since it might be used to initialize the disks</td></tr></table></td></tr>");
                            }
                            sbStorage.Insert(0, "<table width=\"100%\" cellpadding=\"3\" cellspacing=\"0\" border=\"0\" style=\"border:solid 1px #CCCCCC\">");
                            if (dblTotal == 0.00)
                            {
                                sbStorage.Append("<tr><td colspan=\"10\">&nbsp;&nbsp;<img src=\"/images/downright.gif\" border=\"0\" align=\"absmiddle\"/> <i>There is no additional storage for this device. You just need to update the dummy and server names (if applicable).</i></td></tr>");
                            }
                            sbStorage.Append("</table>");
                            sbStorage.Append("<table width=\"100%\"><tr class=\"" + strBootVolumeBackground + "\"><td align=\"right\">* = Boot Volume(s) that have already been allocated.</td></tr></table>");
                        }
                        if (sbStorageShared.ToString() != "")
                        {
                            sbStorageShared.Insert(0, "<tr bgcolor=\"#EEEEEE\" class=\"bold\">" + sbStorageTitle.ToString() + "</tr>");
                            if (intUseLunSerial == 1)
                            {
                                sbStorageShared.Insert(0, "<tr><td colspan=\"3\"></td><td colspan=\"20\"><table cellpadding=\"3\" cellspacing=\"2\" border=\"0\" style=\"border:solid 1px #999999\" bgcolor=\"#f9f9f9\"><tr><td rowspan=\"2\"><img src=\"/images/new.gif\" border=\"0\" align=\"absmiddle\"/></td><td><b>NOTE:</b> The LUN serial number is now available to be entered into ClearView.</td></tr><tr><td>It is highly recommended that you copy and paste this information since it might be used to initialize the disks</td></tr></table></td></tr>");
                            }
                            sbStorageShared.Insert(0, "<table width=\"100%\" cellpadding=\"3\" cellspacing=\"0\" border=\"0\" style=\"border:solid 1px #CCCCCC\">");
                            sbStorageShared.Append("</table>");
                        }
                    }
                    else
                    {
                        sbStorage = new StringBuilder("<br/><img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\"/> There is no storage to configure for this server.  You just need to update the dummy name and server name (if applicable).");
                    }

                    sbData.Append("<tr><td colspan=\"2\">");
                    sbData.Append(sbStorage.ToString());
                    sbData.Append(strStorageSharedHeader);
                    sbData.Append(sbStorageShared.ToString());
                    sbData.Append(sbHidden.ToString());
                    sbData.Append("</td></tr>");
                }
                sbData.Insert(0, "<table width=\"100%\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\">");
                sbData.Append("</table>");
            }
            return sbData.ToString();
        }
    }
}
