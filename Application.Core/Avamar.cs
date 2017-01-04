using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;

namespace NCC.ClearView.Application.Core
{
	public class Avamar
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public string DecomGroup = "/Decom";
        public Avamar(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}

        public int AddGrid(string _name, int _registered, int _threshold, int _maximum, int _enabled)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@registered", _registered);
            arParams[2] = new SqlParameter("@threshold", _threshold);
            arParams[3] = new SqlParameter("@maximum", _maximum);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            arParams[5] = new SqlParameter("@id", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAvamarGrid", arParams);
            return Int32.Parse(arParams[5].Value.ToString());
        }
        public void UpdateGrid(int _id, string _name, int _threshold, int _maximum, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@threshold", _threshold);
            arParams[3] = new SqlParameter("@maximum", _maximum);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAvamarGrid", arParams);
        }
        public void UpdateGrid(int _id, int _registered)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@registered", _registered);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAvamarGridRegistered", arParams);
        }
        public void EnableGrid(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAvamarGridEnabled", arParams);
        }
        public void DeleteGrid(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAvamarGrid", arParams);
        }
        public DataSet GetGrid(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarGrid", arParams);
        }
        public string GetGrid(int _id, string _column)
        {
            DataSet ds = GetGrid(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetGrids(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarGrids", arParams);
        }

        public void AddLocation(int _gridid, int _addressid, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@gridid", _gridid);
            arParams[1] = new SqlParameter("@addressid", _addressid);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAvamarLocation", arParams);
        }
        public void UpdateLocation(int _id, int _gridid, int _addressid, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@gridid", _gridid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAvamarLocation", arParams);
        }
        public void EnableLocation(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAvamarLocationEnabled", arParams);
        }
        public void DeleteLocation(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAvamarLocation", arParams);
        }
        public DataSet GetLocation(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarLocation", arParams);
        }
        public string GetLocation(int _id, string _column)
        {
            DataSet ds = GetLocation(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetLocations(int _gridid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@gridid", _gridid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarLocations", arParams);
        }

        public void AddEnvironment(int _gridid, int _classid, int _environmentid, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@gridid", _gridid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAvamarEnvironment", arParams);
        }
        public void UpdateEnvironment(int _id, int _gridid, int _classid, int _environmentid, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@gridid", _gridid);
            arParams[2] = new SqlParameter("@classid", _classid);
            arParams[3] = new SqlParameter("@environmentid", _environmentid);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAvamarEnvironment", arParams);
        }
        public void EnableEnvironment(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAvamarEnvironmentEnabled", arParams);
        }
        public void DeleteEnvironment(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAvamarEnvironment", arParams);
        }
        public DataSet GetEnvironment(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarEnvironment", arParams);
        }
        public string GetEnvironment(int _id, string _column)
        {
            DataSet ds = GetEnvironment(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetEnvironments(int _gridid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@gridid", _gridid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarEnvironments", arParams);
        }


        public int AddDomain(int _gridid, int _domainid, string _name, int _root, int _catchall, int _registered, int _enabled)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@gridid", _gridid);
            arParams[1] = new SqlParameter("@domainid", _domainid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@root", _root);
            arParams[4] = new SqlParameter("@catchall", _catchall);
            arParams[5] = new SqlParameter("@registered", _registered);
            arParams[6] = new SqlParameter("@enabled", _enabled);
            arParams[7] = new SqlParameter("@id", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAvamarDomain", arParams);
            return Int32.Parse(arParams[7].Value.ToString());
        }
        public void UpdateDomain(int _id, int _gridid, int _domainid, string _name, int _catchall, int _enabled)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@gridid", _gridid);
            arParams[2] = new SqlParameter("@domainid", _domainid);
            arParams[3] = new SqlParameter("@name", _name);
            arParams[4] = new SqlParameter("@catchall", _catchall);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAvamarDomain", arParams);
        }
        public void UpdateDomain(int _id, int _registered)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@registered", _registered);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAvamarDomainRegistered", arParams);
        }
        public void EnableDomain(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAvamarDomainEnabled", arParams);
        }
        public void DeleteDomain(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAvamarDomain", arParams);
        }
        public DataSet GetDomain(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarDomain", arParams);
        }
        public string GetDomain(int _id, string _column)
        {
            DataSet ds = GetDomain(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetDomainsGrid(int _gridid, int _root, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@gridid", _gridid);
            arParams[1] = new SqlParameter("@root", _root);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarDomainsGrid", arParams);
        }
        public DataSet GetDomainsDomain(int _domainid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarDomainsDomain", arParams);
        }
        public void AddDomainResiliency(int _domainid, int _resiliencyid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            arParams[1] = new SqlParameter("@resiliencyid", _resiliencyid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAvamarDomainResiliency", arParams);
        }
        public void DeleteDomainResiliency(int _domainid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAvamarDomainResiliency", arParams);
        }
        public DataSet GetDomainResiliencys(int _domainid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarDomainResiliencys", arParams);
        }
        public void AddDomainApplication(int _domainid, int _applicationid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAvamarDomainApplication", arParams);
        }
        public void DeleteDomainApplication(int _domainid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAvamarDomainApplication", arParams);
        }
        public DataSet GetDomainApplications(int _domainid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarDomainApplications", arParams);
        }


        public int AddGroup(int _domainid, string _name, int _daily, int _weekly, int _sunday, int _monday, int _tuesday, int _wednesday, int _thursday, int _friday, int _saturday, int _monthly, int _day, int _AM1200, int _AM1230, int _AM100, int _AM130, int _AM200, int _AM230, int _AM300, int _AM330, int _AM400, int _AM430, int _AM500, int _AM530, int _AM600, int _AM630, int _AM700, int _AM730, int _AM800, int _AM830, int _AM900, int _AM930, int _AM1000, int _AM1030, int _AM1100, int _AM1130, int _PM1200, int _PM1230, int _PM100, int _PM130, int _PM200, int _PM230, int _PM300, int _PM330, int _PM400, int _PM430, int _PM500, int _PM530, int _PM600, int _PM630, int _PM700, int _PM730, int _PM800, int _PM830, int _PM900, int _PM930, int _PM1000, int _PM1030, int _PM1100, int _PM1130, int _registered, int _threshold, int _maximum, int _clustering, int _enabled)
        {
            arParams = new SqlParameter[67];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@daily", _daily);
            arParams[3] = new SqlParameter("@weekly", _weekly);
            arParams[4] = new SqlParameter("@sunday", _sunday);
            arParams[5] = new SqlParameter("@monday", _monday);
            arParams[6] = new SqlParameter("@tuesday", _tuesday);
            arParams[7] = new SqlParameter("@wednesday", _wednesday);
            arParams[8] = new SqlParameter("@thursday", _thursday);
            arParams[9] = new SqlParameter("@friday", _friday);
            arParams[10] = new SqlParameter("@saturday", _saturday);
            arParams[11] = new SqlParameter("@monthly", _monthly);
            arParams[12] = new SqlParameter("@day", _day);
            arParams[13] = new SqlParameter("@AM1200", _AM1200);
            arParams[14] = new SqlParameter("@AM1230", _AM1230);
            arParams[15] = new SqlParameter("@AM100", _AM100);
            arParams[16] = new SqlParameter("@AM130", _AM130);
            arParams[17] = new SqlParameter("@AM200", _AM200);
            arParams[18] = new SqlParameter("@AM230", _AM230);
            arParams[19] = new SqlParameter("@AM300", _AM300);
            arParams[20] = new SqlParameter("@AM330", _AM330);
            arParams[21] = new SqlParameter("@AM400", _AM400);
            arParams[22] = new SqlParameter("@AM430", _AM430);
            arParams[23] = new SqlParameter("@AM500", _AM500);
            arParams[24] = new SqlParameter("@AM530", _AM530);
            arParams[25] = new SqlParameter("@AM600", _AM600);
            arParams[26] = new SqlParameter("@AM630", _AM630);
            arParams[27] = new SqlParameter("@AM700", _AM700);
            arParams[28] = new SqlParameter("@AM730", _AM730);
            arParams[29] = new SqlParameter("@AM800", _AM800);
            arParams[30] = new SqlParameter("@AM830", _AM830);
            arParams[31] = new SqlParameter("@AM900", _AM900);
            arParams[32] = new SqlParameter("@AM930", _AM930);
            arParams[33] = new SqlParameter("@AM1000", _AM1000);
            arParams[34] = new SqlParameter("@AM1030", _AM1030);
            arParams[35] = new SqlParameter("@AM1100", _AM1100);
            arParams[36] = new SqlParameter("@AM1130", _AM1130);
            arParams[37] = new SqlParameter("@PM1200", _PM1200);
            arParams[38] = new SqlParameter("@PM1230", _PM1230);
            arParams[39] = new SqlParameter("@PM100", _PM100);
            arParams[40] = new SqlParameter("@PM130", _PM130);
            arParams[41] = new SqlParameter("@PM200", _PM200);
            arParams[42] = new SqlParameter("@PM230", _PM230);
            arParams[43] = new SqlParameter("@PM300", _PM300);
            arParams[44] = new SqlParameter("@PM330", _PM330);
            arParams[45] = new SqlParameter("@PM400", _PM400);
            arParams[46] = new SqlParameter("@PM430", _PM430);
            arParams[47] = new SqlParameter("@PM500", _PM500);
            arParams[48] = new SqlParameter("@PM530", _PM530);
            arParams[49] = new SqlParameter("@PM600", _PM600);
            arParams[50] = new SqlParameter("@PM630", _PM630);
            arParams[51] = new SqlParameter("@PM700", _PM700);
            arParams[52] = new SqlParameter("@PM730", _PM730);
            arParams[53] = new SqlParameter("@PM800", _PM800);
            arParams[54] = new SqlParameter("@PM830", _PM830);
            arParams[55] = new SqlParameter("@PM900", _PM900);
            arParams[56] = new SqlParameter("@PM930", _PM930);
            arParams[57] = new SqlParameter("@PM1000", _PM1000);
            arParams[58] = new SqlParameter("@PM1030", _PM1030);
            arParams[59] = new SqlParameter("@PM1100", _PM1100);
            arParams[60] = new SqlParameter("@PM1130", _PM1130);
            arParams[61] = new SqlParameter("@registered", _registered);
            arParams[62] = new SqlParameter("@threshold", _threshold);
            arParams[63] = new SqlParameter("@maximum", _maximum);
            arParams[64] = new SqlParameter("@clustering", _clustering);
            arParams[65] = new SqlParameter("@enabled", _enabled);
            arParams[66] = new SqlParameter("@id", SqlDbType.Int);
            arParams[66].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAvamarGroup", arParams);
            return Int32.Parse(arParams[66].Value.ToString());
        }
        public void UpdateGroup(int _id, int _domainid, string _name, int _daily, int _weekly, int _sunday, int _monday, int _tuesday, int _wednesday, int _thursday, int _friday, int _saturday, int _monthly, int _day, int _AM1200, int _AM1230, int _AM100, int _AM130, int _AM200, int _AM230, int _AM300, int _AM330, int _AM400, int _AM430, int _AM500, int _AM530, int _AM600, int _AM630, int _AM700, int _AM730, int _AM800, int _AM830, int _AM900, int _AM930, int _AM1000, int _AM1030, int _AM1100, int _AM1130, int _PM1200, int _PM1230, int _PM100, int _PM130, int _PM200, int _PM230, int _PM300, int _PM330, int _PM400, int _PM430, int _PM500, int _PM530, int _PM600, int _PM630, int _PM700, int _PM730, int _PM800, int _PM830, int _PM900, int _PM930, int _PM1000, int _PM1030, int _PM1100, int _PM1130, int _threshold, int _maximum, int _clustering, int _enabled)
        {
            arParams = new SqlParameter[66];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@domainid", _domainid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@daily", _daily);
            arParams[4] = new SqlParameter("@weekly", _weekly);
            arParams[5] = new SqlParameter("@sunday", _sunday);
            arParams[6] = new SqlParameter("@monday", _monday);
            arParams[7] = new SqlParameter("@tuesday", _tuesday);
            arParams[8] = new SqlParameter("@wednesday", _wednesday);
            arParams[9] = new SqlParameter("@thursday", _thursday);
            arParams[10] = new SqlParameter("@friday", _friday);
            arParams[11] = new SqlParameter("@saturday", _saturday);
            arParams[12] = new SqlParameter("@monthly", _monthly);
            arParams[13] = new SqlParameter("@day", _day);
            arParams[14] = new SqlParameter("@AM1200", _AM1200);
            arParams[15] = new SqlParameter("@AM1230", _AM1230);
            arParams[16] = new SqlParameter("@AM100", _AM100);
            arParams[17] = new SqlParameter("@AM130", _AM130);
            arParams[18] = new SqlParameter("@AM200", _AM200);
            arParams[19] = new SqlParameter("@AM230", _AM230);
            arParams[20] = new SqlParameter("@AM300", _AM300);
            arParams[21] = new SqlParameter("@AM330", _AM330);
            arParams[22] = new SqlParameter("@AM400", _AM400);
            arParams[23] = new SqlParameter("@AM430", _AM430);
            arParams[24] = new SqlParameter("@AM500", _AM500);
            arParams[25] = new SqlParameter("@AM530", _AM530);
            arParams[26] = new SqlParameter("@AM600", _AM600);
            arParams[27] = new SqlParameter("@AM630", _AM630);
            arParams[28] = new SqlParameter("@AM700", _AM700);
            arParams[29] = new SqlParameter("@AM730", _AM730);
            arParams[30] = new SqlParameter("@AM800", _AM800);
            arParams[31] = new SqlParameter("@AM830", _AM830);
            arParams[32] = new SqlParameter("@AM900", _AM900);
            arParams[33] = new SqlParameter("@AM930", _AM930);
            arParams[34] = new SqlParameter("@AM1000", _AM1000);
            arParams[35] = new SqlParameter("@AM1030", _AM1030);
            arParams[36] = new SqlParameter("@AM1100", _AM1100);
            arParams[37] = new SqlParameter("@AM1130", _AM1130);
            arParams[38] = new SqlParameter("@PM1200", _PM1200);
            arParams[39] = new SqlParameter("@PM1230", _PM1230);
            arParams[40] = new SqlParameter("@PM100", _PM100);
            arParams[41] = new SqlParameter("@PM130", _PM130);
            arParams[42] = new SqlParameter("@PM200", _PM200);
            arParams[43] = new SqlParameter("@PM230", _PM230);
            arParams[44] = new SqlParameter("@PM300", _PM300);
            arParams[45] = new SqlParameter("@PM330", _PM330);
            arParams[46] = new SqlParameter("@PM400", _PM400);
            arParams[47] = new SqlParameter("@PM430", _PM430);
            arParams[48] = new SqlParameter("@PM500", _PM500);
            arParams[49] = new SqlParameter("@PM530", _PM530);
            arParams[50] = new SqlParameter("@PM600", _PM600);
            arParams[51] = new SqlParameter("@PM630", _PM630);
            arParams[52] = new SqlParameter("@PM700", _PM700);
            arParams[53] = new SqlParameter("@PM730", _PM730);
            arParams[54] = new SqlParameter("@PM800", _PM800);
            arParams[55] = new SqlParameter("@PM830", _PM830);
            arParams[56] = new SqlParameter("@PM900", _PM900);
            arParams[57] = new SqlParameter("@PM930", _PM930);
            arParams[58] = new SqlParameter("@PM1000", _PM1000);
            arParams[59] = new SqlParameter("@PM1030", _PM1030);
            arParams[60] = new SqlParameter("@PM1100", _PM1100);
            arParams[61] = new SqlParameter("@PM1130", _PM1130);
            arParams[62] = new SqlParameter("@threshold", _threshold);
            arParams[63] = new SqlParameter("@maximum", _maximum);
            arParams[64] = new SqlParameter("@clustering", _clustering);
            arParams[65] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAvamarGroup", arParams);
        }
        public void UpdateGroup(int _id, int _registered)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@registered", _registered);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAvamarGroupRegistered", arParams);
        }
        public void EnableGroup(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAvamarGroupEnabled", arParams);
        }
        public void DeleteGroup(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAvamarGroup", arParams);
        }
        public DataSet GetGroup(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarGroup", arParams);
        }
        public string GetGroup(int _id, string _column)
        {
            DataSet ds = GetGroup(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetGroupsGrid(int _gridid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@gridid", _gridid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarGroupsGrid", arParams);
        }
        public DataSet GetGroupsDomain(int _domainid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@domainid", _domainid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarGroupsDomain", arParams);
        }
        public void AddGroupOperatingSystem(int _groupid, int _osid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@groupid", _groupid);
            arParams[1] = new SqlParameter("@osid", _osid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAvamarGroupOperatingSystem", arParams);
        }
        public void DeleteGroupOperatingSystem(int _groupid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@groupid", _groupid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAvamarGroupOperatingSystem", arParams);
        }
        public DataSet GetGroupOperatingSystems(int _groupid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@groupid", _groupid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarGroupOperatingSystems", arParams);
        }
        public int AddLog(int _gridid, int _groupid, int _domainid, string _output)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@gridid", _gridid);
            arParams[1] = new SqlParameter("@groupid", _groupid);
            arParams[2] = new SqlParameter("@domainid", _domainid);
            arParams[3] = new SqlParameter("@output", _output);
            arParams[4] = new SqlParameter("@id", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAvamarLog", arParams);
            return Int32.Parse(arParams[4].Value.ToString());
        }


        // Business Logic
        public DataSet GetGridQuery(int _addressid, int _classid, int _environmentid, int _mnemonicid, int _serverid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@addressid", _addressid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@mnemonicid", _mnemonicid);
            arParams[4] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarGridQuery", arParams);
        }
        public DataSet GetDomainQuery(int _gridid, int _serverid, int _hours)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@gridid", _gridid);
            arParams[1] = new SqlParameter("@serverid", _serverid);
            arParams[2] = new SqlParameter("@hours", _hours);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarDomainQuery", arParams);
        }
        public DataSet GetGroupQuery(int _gridid, int _serverid, int _clustering)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@gridid", _gridid);
            arParams[1] = new SqlParameter("@serverid", _serverid);
            arParams[2] = new SqlParameter("@clustering", _clustering);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarGroupQuery", arParams);
        }
        

        // Decoms
        public int AddDecom(string _client, string _grid, string _domain, string _group)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@client", _client);
            arParams[1] = new SqlParameter("@grid", _grid);
            arParams[2] = new SqlParameter("@domain", _domain);
            arParams[3] = new SqlParameter("@group", _group);
            arParams[4] = new SqlParameter("@id", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAvamarDecom", arParams);
            return Int32.Parse(arParams[4].Value.ToString());
        }
        public void UpdateDecom(string _client)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@client", _client);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAvamarDecom", arParams);
        }
        public void DeleteDecom(string _client)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@client", _client);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAvamarDecom", arParams);
        }
        public DataSet GetDecoms(string _client)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@client", _client);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAvamarDecoms", arParams);
        }
    }
}
