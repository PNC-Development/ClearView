using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Web.UI.WebControls;
using System.Web;
using System.Text;

namespace NCC.ClearView.Application.Core
{
	public class Pages
	{
		private string dsn = "";
		private int user = 0;
		private bool logging = false;
		private SqlParameter[] arParams;
		private DataSet ds;
		private Log oLog;
		private Settings oSetting;
        private string _suffix = "/default.aspx";
        protected int intRecords = 20;
        protected int intRecordStart = 1;

        public Pages(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
			oLog = new Log(user, dsn);
			oSetting = new Settings(user, dsn);
		}
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
			return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPage", arParams);
		}
		public string Get(int _id, string _column) 
		{
			ds = Get(_id);
			if (ds.Tables[0].Rows.Count > 0)
				return ds.Tables[0].Rows[0][_column].ToString();
			else
				return "";
		}
		public string GetName(int _id)
		{
			string strName = "Unavailable";
			try { strName = Get(_id).Tables[0].Rows[0]["title"].ToString(); }
			catch {}
			return strName;
		}
        public DataSet Gets(int _enabled)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@enabled", _enabled);
			return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPages", arParams);
		}
        public DataSet Gets(int _parent, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPagesTree", arParams);
        }
        public void Add(int _parent, string _title, string _urltitle, string _menutitle, string _browsertitle, int _templateid, int _related, string _navimage, string _navoverimage, string _description, string _tooltip, string _sproc, int _window, string _url, string _target, int _navigation, int _enabled)
		{
			if (logging == true) 
				oLog.Add("Add page " + _title);
			arParams = new SqlParameter[17];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@title", _title);
			arParams[2] = new SqlParameter("@urltitle", _urltitle);
			arParams[3] = new SqlParameter("@menutitle", _menutitle);
            arParams[4] = new SqlParameter("@browsertitle", _browsertitle);
            arParams[5] = new SqlParameter("@templateid", _templateid);
            arParams[6] = new SqlParameter("@related", _related);
            arParams[7] = new SqlParameter("@navimage", _navimage);
			arParams[8] = new SqlParameter("@navoverimage", _navoverimage);
            arParams[9] = new SqlParameter("@description", _description);
            arParams[10] = new SqlParameter("@tooltip", _tooltip);
            arParams[11] = new SqlParameter("@sproc", _sproc);
			arParams[12] = new SqlParameter("@window", _window);
			arParams[13] = new SqlParameter("@url", _url);
			arParams[14] = new SqlParameter("@target", _target);
            arParams[15] = new SqlParameter("@navigation", _navigation);
            arParams[16] = new SqlParameter("@enabled", _enabled);
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addPage", arParams);
		}
        public void Update(int _id, int _parent, string _title, string _urltitle, string _menutitle, string _browsertitle, int _templateid, int _related, string _navimage, string _navoverimage, string _description, string _tooltip, string _sproc, int _window, string _url, string _target, int _navigation, int _enabled)
		{
			if (logging == true) 
				oLog.Add("Update page " + GetName(_id));
			arParams = new SqlParameter[18];
			arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@parent", _parent);
            arParams[2] = new SqlParameter("@title", _title);
            arParams[3] = new SqlParameter("@urltitle", _urltitle);
            arParams[4] = new SqlParameter("@menutitle", _menutitle);
            arParams[5] = new SqlParameter("@browsertitle", _browsertitle);
            arParams[6] = new SqlParameter("@templateid", _templateid);
            arParams[7] = new SqlParameter("@related", _related);
            arParams[8] = new SqlParameter("@navimage", _navimage);
            arParams[9] = new SqlParameter("@navoverimage", _navoverimage);
            arParams[10] = new SqlParameter("@description", _description);
            arParams[11] = new SqlParameter("@tooltip", _tooltip);
            arParams[12] = new SqlParameter("@sproc", _sproc);
            arParams[13] = new SqlParameter("@window", _window);
            arParams[14] = new SqlParameter("@url", _url);
            arParams[15] = new SqlParameter("@target", _target);
            arParams[16] = new SqlParameter("@navigation", _navigation);
            arParams[17] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePage", arParams);
		}
        public void UpdateOrder(int _id, int _display)
        {
            if (logging == true)
                oLog.Add("Update Order for " + GetName(_id));
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePageOrder", arParams);
        }
        public void Enable(int _id, int _enabled) 
		{
			if (logging == true) 
			{
				if (_enabled == 1)
					oLog.Add("Enable page " + GetName(_id));
				else
					oLog.Add("Disable page " + GetName(_id));
			}
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@enabled", _enabled);
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePageEnabled", arParams);
		}
		public void Delete(int _id)
		{
			if (logging == true) 
				oLog.Add("Delete page " + GetName(_id));
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
			SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deletePage", arParams);
		}
        public int GetParent(int _pageid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@pageid", _pageid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getPageParent", arParams);
            if (o == null)
                return 0;
            else
                return Int32.Parse(o.ToString());
        }
        public string GetFullLinkRelated(int _pageid)
        {
            if (_pageid == 0)
                return "/index.aspx";
            else
            {
                int intRelated = Int32.Parse(Get(_pageid, "related"));
                if (intRelated == 0)
                    return "/index.aspx";
                else
                    return GetFullLink(intRelated);
            }
        }
        public string GetFullLinkParent(int _pageid)
        {
            if (_pageid == 0)
                return "/index.aspx";
            else
            {
                int intParent = GetParent(_pageid);
                if (intParent == 0)
                    return "/index.aspx";
                else
                    return GetFullLink(intParent);
            }
        }
        public string GetFullLink(int _pageid)
        {
            string strLink = "";
            while (_pageid > 0)
            {
                strLink = "/" + Get(_pageid, "urltitle") + strLink;
                _pageid = GetParent(_pageid);
            }
            return strLink + _suffix;
        }
        public string GetHref(int _pageid)
        {
            if (Get(_pageid, "window") == "0")
                return "href=\"" + GetFullLink(_pageid) + "\"";
            else
                return "href=\"" + Get(_pageid, "url") + "\" target=\"" + Get(_pageid, "target") + "\"";
        }

        public DataSet Gets(int _applicationid, int _userid, int _parent, int _link, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@parent", _parent);
            arParams[3] = new SqlParameter("@link", _link);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPagesAll", arParams);
        }

        public DataSet GetTotal(int _userid, string _storedProcedure)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, _storedProcedure, arParams);
        }


        // PAGING
        public void LoadPaging(DataSet ds, HttpRequest Request, int intPage, Label lblPage, Label lblSort, Label lblTopPaging, Label lblBottomPaging, TextBox txtPage, Label lblPages, Label lblRecords, Repeater rptView, Label lblNone)
        {
            lblPage.Text = "1";
            lblSort.Text = "";
            if (Request.QueryString["page"] != null && Request.QueryString["page"] != "")
                lblPage.Text = Request.QueryString["page"];
            if (Request.QueryString["sort"] != null && Request.QueryString["sort"] != "")
                lblSort.Text = Request.QueryString["sort"];
            lblTopPaging.Text = "";
            int intStart = 0;
            Int32.TryParse(lblPage.Text, out intStart);
            if (intStart == 0)
                intStart = 1;
            string strSort = lblSort.Text;

            int intCount = ds.Tables[0].Rows.Count;
            double dblEnd = Math.Ceiling(Double.Parse(intCount.ToString()) / Double.Parse(intRecords.ToString()));
            int intEnd = Int32.Parse(dblEnd.ToString());
            int ii = 0;
            txtPage.Text = intStart.ToString();
            lblPages.Text = intEnd.ToString();
            if (intEnd < 7)
            {
                for (ii = 1; ii < intEnd; ii++)
                {
                    LoadLink(Request, intPage, lblTopPaging, ii, ", ", intStart);
                    LoadLink(Request, intPage, lblBottomPaging, ii, ", ", intStart);
                }
                LoadLink(Request, intPage, lblTopPaging, intEnd, "", intStart);
                LoadLink(Request, intPage, lblBottomPaging, intEnd, "", intStart);
            }
            else
            {
                if (intStart < 5)
                {
                    for (ii = 1; ii < 6 && ii < intEnd; ii++)
                    {
                        LoadLink(Request, intPage, lblTopPaging, ii, ", ", intStart);
                        LoadLink(Request, intPage, lblBottomPaging, ii, ", ", intStart);
                    }
                    if (ii < intEnd)
                    {
                        LoadLink(Request, intPage, lblTopPaging, ii, " .... ", intStart);
                        LoadLink(Request, intPage, lblBottomPaging, ii, " .... ", intStart);
                    }
                    LoadLink(Request, intPage, lblTopPaging, intEnd, "", intStart);
                    LoadLink(Request, intPage, lblBottomPaging, intEnd, "", intStart);
                }
                else if (intStart > (intEnd - 4))
                {
                    LoadLink(Request, intPage, lblTopPaging, 1, " .... ", intStart);
                    LoadLink(Request, intPage, lblBottomPaging, 1, " .... ", intStart);
                    for (ii = (intEnd - 5); ii < intEnd && ii > 0; ii++)
                    {
                        LoadLink(Request, intPage, lblTopPaging, ii, ", ", intStart);
                        LoadLink(Request, intPage, lblBottomPaging, ii, ", ", intStart);
                    }
                    LoadLink(Request, intPage, lblTopPaging, intEnd, "", intStart);
                    LoadLink(Request, intPage, lblBottomPaging, intEnd, "", intStart);
                }
                else
                {
                    LoadLink(Request, intPage, lblTopPaging, 1, " .... ", intStart);
                    LoadLink(Request, intPage, lblBottomPaging, 1, " .... ", intStart);
                    for (ii = (intStart - 2); ii < (intStart + 3) && ii < intEnd && ii > 0; ii++)
                    {
                        if (ii == (intStart + 2))
                        {
                            LoadLink(Request, intPage, lblTopPaging, ii, " .... ", intStart);
                            LoadLink(Request, intPage, lblBottomPaging, ii, " .... ", intStart);
                        }
                        else
                        {
                            LoadLink(Request, intPage, lblTopPaging, ii, ", ", intStart);
                            LoadLink(Request, intPage, lblBottomPaging, ii, ", ", intStart);
                        }
                    }
                    LoadLink(Request, intPage, lblTopPaging, intEnd, "", intStart);
                    LoadLink(Request, intPage, lblBottomPaging, intEnd, "", intStart);
                }
            }
            LoopRepeater(Request, strSort, ((intStart - 1) * intRecords), lblRecords, rptView, lblNone, ds);
        }
        private void LoopRepeater(HttpRequest Request, string _sort, int _start, Label lblRecords, Repeater rptView, Label lblNone, DataSet ds)
        {
            if (_start > ds.Tables[0].Rows.Count)
                _start = 0;
            intRecordStart = _start + 1;
            DataView dv = ds.Tables[0].DefaultView;
            if (Request.QueryString["sort"] != null)
            {
                try
                {
                    dv.Sort = Request.QueryString["sort"];
                }
                catch { }
            }
            int intCount = _start + intRecords;
            if (dv.Count < intCount)
                intCount = dv.Count;
            int ii = 0;
            lblRecords.Text = "Requests " + intRecordStart.ToString() + " - " + intCount.ToString() + " of " + dv.Count.ToString();
            for (ii = 0; ii < _start; ii++)
                dv[0].Delete();
            int intTotalCount = (dv.Count - intRecords);
            for (ii = 0; ii < intTotalCount; ii++)
                dv[intRecords].Delete();
            rptView.DataSource = dv;
            rptView.DataBind();
            lblNone.Visible = (rptView.Items.Count == 0);
            _start++;
        }
        private void LoadLink(HttpRequest Request, int intPage, Label _label, int _number, string _spacer, int _start)
        {
            StringBuilder sb = new StringBuilder(_label.Text);

            if (_number == _start)
            {
                sb.Append("<b><font style=\"color:#CC0000\">");
                sb.Append(_number.ToString());
                sb.Append("</font></b>");
            }
            else
            {
                string strSort = "";
                if (Request.QueryString["sort"] != null)
                {
                    strSort = Request.QueryString["sort"];
                }

                sb.Append("<a href=\"");
                sb.Append(GetFullLink(intPage));
                sb.Append("?sort=");
                sb.Append(strSort);
                sb.Append("&page=");
                sb.Append(_number.ToString());
                sb.Append("\" title=\"Go to Page ");
                sb.Append(_number.ToString());
                sb.Append("\">");
                sb.Append(_number.ToString());
                sb.Append("</a>");
            }

            if (_spacer != "")
            {
                sb.Append(_spacer);
            }

            _label.Text = sb.ToString();
        }
        public void btnOrder(HttpRequest Request, Object Sender, HttpResponse Response, int intPage)
        {
            LinkButton oOrder = (LinkButton)Sender;
            string strPage = "";
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
            {
                if (Request.QueryString["sort"] == oOrder.CommandArgument)
                    strOrder = oOrder.CommandArgument + " DESC";
            }
            if (strOrder == "")
                strOrder = oOrder.CommandArgument;
            if (Request.QueryString["page"] != null)
                strPage = Request.QueryString["page"];
            Response.Redirect(GetFullLink(intPage) + "?sort=" + strOrder + "&page=" + strPage);
        }
        public void btnPage(HttpRequest Request, HttpResponse Response, int intPage, TextBox txtPage)
        {
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
                strOrder = Request.QueryString["sort"];
            Response.Redirect(GetFullLink(intPage) + "?sort=" + strOrder + "&page=" + txtPage.Text);
        }
    }
}
