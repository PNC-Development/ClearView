using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class Resiliency
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Resiliency(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public void Add(string _name, int _bir, int _min, int _max, int _display, int _enabled)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@bir", _bir);
            arParams[2] = new SqlParameter("@min", _min);
            arParams[3] = new SqlParameter("@max", _max);
            arParams[4] = new SqlParameter("@display", _display);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResiliency", arParams);
        }
        public void Update(int _id, string _name, int _bir, int _min, int _max, int _enabled)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@bir", _bir);
            arParams[3] = new SqlParameter("@min", _min);
            arParams[4] = new SqlParameter("@max", _max);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResiliency", arParams);
        }
        public void UpdateOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResiliencyOrder", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateResiliencyEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteResiliency", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResiliency", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResiliencys", arParams);
        }
        public bool IsBIR(int _id)
        {
            return (Get(_id, "bir") == "1");
        }

        public void AddLocation(int _resiliencyid, int _prod, int _dr)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[1] = new SqlParameter("@prod", _prod);
            arParams[2] = new SqlParameter("@dr", _dr);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addResiliencyLocation", arParams);
        }
        public void DeleteLocation(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteResiliencyLocation", arParams);
        }
        public DataSet GetLocations(int _resiliencyid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@resiliencyid", _resiliencyid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getResiliencyLocations", arParams);
        }
        public int GetLocationDR(int _addressid, int _bir)
        {
            int intDR = 0;
            DataSet ds = Gets(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (intDR > 0)
                    break;
                if (Int32.Parse(dr["bir"].ToString()) == _bir)
                {
                    DataSet dsLocations = GetLocations(Int32.Parse(dr["id"].ToString()));
                    foreach (DataRow drLocation in dsLocations.Tables[0].Rows)
                    {
                        if (Int32.Parse(drLocation["prodID"].ToString()) == _addressid)
                        {
                            intDR = Int32.Parse(drLocation["drID"].ToString());
                            break;
                        }
                    }
                }
            }
            return intDR;
        }

        public int GetIDFromMnemonic(int _mnemonicid, bool _bir, int _answerid)
        {
            Log oLog = new Log(0, dsn);
            int intID = 0;
            if (_answerid > 0)
                oLog.AddEvent(_answerid, "", "", "GetIDFromMnemonic -> MnemonicID : " + _mnemonicid.ToString(), LoggingType.Debug);
            if (_mnemonicid > 0)
            {
                Mnemonic oMnemonic = new Mnemonic(0, dsn);
                int intHours = oMnemonic.GetResRatingHRs(_mnemonicid);
                // 7/7/15 - override MMS information with requestor.
                if (_answerid > 0)
                {
                    Design oDesign = new Design(0, dsn);
                    DataSet dsDesign = oDesign.GetAnswer(_answerid);
                    if (dsDesign.Tables[0].Rows.Count > 0)
                    {
                        int DR = 0;
                        if (Int32.TryParse(dsDesign.Tables[0].Rows[0]["dr"].ToString(), out DR))
                        {
                            if (intHours < 48)      // MMS => Under 48 Hours
                            {
                                if (DR == 0)        // Client => 48 Hours or More
                                {
                                    intHours = 72;  // Result = 48 Hours or More
                                    oLog.AddEvent(_answerid, "", "", "Resiliency rating has been overridden by the requestor (should be under 48 hrs)", LoggingType.Information);
                                }
                                else
                                {
                                    // Client has accepted the MMS default
                                }
                            }
                            else                    // MMS => 48 Hours or More
                            {
                                if (DR == 1)        // Client => Under 48 Hours 
                                {
                                    intHours = 24;  // Result = Under 48 Hours
                                    oLog.AddEvent(_answerid, "", "", "Resiliency rating has been overridden by the requestor (should be 48 hrs or more)", LoggingType.Information);
                                }
                                else
                                {
                                    // Client has accepted the MMS default
                                }
                            }
                        }
                        else
                            oLog.AddEvent(_answerid, "", "", "Unable to parse the DR value (" + dsDesign.Tables[0].Rows[0]["dr"].ToString() + ")", LoggingType.Debug);
                    }
                    else
                        oLog.AddEvent(_answerid, "", "", "No design information (" + _answerid.ToString() + ")", LoggingType.Debug);
                }
                if (_answerid > 0)
                    oLog.AddEvent(_answerid, "", "", "GetIDFromMnemonic -> intHours : " + intHours.ToString(), LoggingType.Debug);
                if (intHours > 0)
                {
                    DataSet ds = Gets(1);
                    if (_answerid > 0)
                        oLog.AddEvent(_answerid, "", "", "GetIDFromMnemonic -> ds.Tables[0].Rows.Count : " + ds.Tables[0].Rows.Count.ToString(), LoggingType.Debug);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        // Added on 3/1/13...all designs are cleveland <--> summit.
                        //if ((dr["bir"].ToString() == "1" && _bir == true) || (dr["bir"].ToString() == "0" && _bir == false))
                        //{
                            int intMin = Int32.Parse(dr["min"].ToString());
                            int intMax = Int32.Parse(dr["max"].ToString());
                            if (_answerid > 0)
                            {
                                oLog.AddEvent(_answerid, "", "", "GetIDFromMnemonic -> " + dr["id"].ToString() + ", intMin : " + intMin.ToString(), LoggingType.Debug);
                                oLog.AddEvent(_answerid, "", "", "GetIDFromMnemonic -> " + dr["id"].ToString() + ", intMax : " + intMax.ToString(), LoggingType.Debug);
                            }
                            if (intHours >= intMin && intHours <= intMax)
                            {
                                intID = Int32.Parse(dr["id"].ToString());
                                if (_answerid > 0)
                                    oLog.AddEvent(_answerid, "", "", "GetIDFromMnemonic -> intResiliency : " + intID.ToString(), LoggingType.Debug);
                                break;
                            }
                        //}
                    }
                }
            }
            return intID;
        }
    }
}
