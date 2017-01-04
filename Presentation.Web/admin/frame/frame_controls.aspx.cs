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
    public partial class frame_controls : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected PageControls oPage;
        protected Controls oControl;
        protected Schema oSchema;
        private ListBox _1, _2, _3, _4, _5, _6, _7, _8, _9, _10;
        protected int intPlaces;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            Users oUser = new Users(intProfile, dsn);
            Permissions oPermission = new Permissions(intProfile, dsn);
            Settings oSetting = new Settings(intProfile, dsn);
            intPlaces = 3;
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "" && Request.QueryString["type"] != null && Request.QueryString["type"] != "")
            {
                oControl = new Controls(intProfile, dsn);
                oPage = new PageControls(intProfile, dsn);
                oSchema = new Schema(intProfile, dsn);
                ddlControl.DataTextField = "name";
                ddlControl.DataValueField = "controlid";
                ddlControl.DataSource = oControl.Gets(1, 1);
                ddlControl.DataBind();
                ddlControl.Items.Insert(0, "-- SELECT --");
                if (!IsPostBack && (Request.QueryString["type"] == "0" || Request.QueryString["type"] == "1"))
                {
                    lblId.Text = Request.QueryString["id"];
                    lblType.Text = Request.QueryString["type"];
                    LoadButtons();
                }
                btnClose.Attributes.Add("onclick", "return HidePanel();");
            }
        }
        public void LoadButtons()
        {
            TableRow oRowTop = new TableRow();
            TableRow oRowMid = new TableRow();
            _1 = new ListBox();
            _2 = new ListBox();
            _3 = new ListBox();
            _4 = new ListBox();
            _5 = new ListBox();
            _6 = new ListBox();
            _7 = new ListBox();
            _8 = new ListBox();
            _9 = new ListBox();
            _10 = new ListBox();
            _1.ID = "lstPH1";
            _2.ID = "lstPH2";
            _3.ID = "lstPH3";
            _4.ID = "lstPH4";
            _5.ID = "lstPH5";
            _6.ID = "lstPH6";
            _7.ID = "lstPH7";
            _8.ID = "lstPH8";
            _9.ID = "lstPH9";
            _10.ID = "lstPH10";
            if (intPlaces > 0) LoadButton(oRowTop, oRowMid, 1, _1, null, _2);
            if (intPlaces > 1) LoadButton(oRowTop, oRowMid, 2, _2, _1, _3);
            if (intPlaces > 2) LoadButton(oRowTop, oRowMid, 3, _3, _2, _4);
            if (intPlaces > 3) LoadButton(oRowTop, oRowMid, 4, _4, _3, _5);
            if (intPlaces > 4) LoadButton(oRowTop, oRowMid, 5, _5, _4, _6);
            if (intPlaces > 5) LoadButton(oRowTop, oRowMid, 6, _6, _5, _7);
            if (intPlaces > 6) LoadButton(oRowTop, oRowMid, 7, _7, _6, _8);
            if (intPlaces > 7) LoadButton(oRowTop, oRowMid, 8, _8, _7, _9);
            if (intPlaces > 8) LoadButton(oRowTop, oRowMid, 9, _9, _8, _10);
            if (intPlaces > 9) LoadButton(oRowTop, oRowMid, 10, _10, _9, null);
            tblControls.Rows.Add(oRowTop);
            tblControls.Rows.Add(oRowMid);
        }

        public void LoadButton(TableRow _top, TableRow _mid, int _ii, ListBox oList, ListBox oPrev, ListBox oNext)
        {
            TableCell oCell = new TableCell();
            LinkButton oLink = new LinkButton();
            oLink.Text = "Add to PH" + _ii.ToString();
            oLink.CssClass = "default";
            oList.CssClass = "default";
            oList.Rows = 6;
            oList.Width = Unit.Pixel(180);
            oLink.Attributes.Add("onclick", "return addControl(" + ddlControl.ClientID + ",'" + oList.ClientID + "');");
            oCell.HorizontalAlign = HorizontalAlign.Center;
            oCell.Controls.Add(oLink);
            if (Request.QueryString["type"] == "1")
                LoadControls(oPage.GetPage(Int32.Parse(lblId.Text), 1), "pagecontrolid", oList, "PH" + _ii.ToString());
            _top.Cells.Add(oCell);
            TableCell oCellTop = new TableCell();
            Table oTable = new Table();
            TableRow oRow = new TableRow();
            oCell = new TableCell();
            oCell.RowSpan = 5;
            oCell.Controls.Add(oList);
            oRow.Cells.Add(oCell);
            if (_ii == 1)
                LoadCell(oRow, oTable, "", "");
            else
                LoadCell(oRow, oTable, "/admin/images/lt.gif", "return MoveLeft('" + oList.ClientID + "','" + oPrev.ClientID + "');");
            LoadCell(null, oTable, "/admin/images/up.gif", "return MoveUp('" + oList.ClientID + "');");
            LoadCell(null, oTable, "/admin/images/dl.gif", "return MoveOut('" + oList.ClientID + "');");
            LoadCell(null, oTable, "/admin/images/dn.gif", "return MoveDown('" + oList.ClientID + "');");
            if (_ii == intPlaces)
                LoadCell(null, oTable, "", "");
            else
                LoadCell(null, oTable, "/admin/images/rt.gif", "return MoveRight('" + oList.ClientID + "','" + oNext.ClientID + "');");
            oCellTop.Controls.Add(oTable);
            _mid.Cells.Add(oCellTop);
        }
        public void LoadCell(TableRow _row, Table _table, string _image, string _onclick)
        {
            TableCell oCell = new TableCell();
            if (_image == "")
                oCell.Text = "&nbsp;";
            else
            {
                ImageButton oButton = new ImageButton();
                oButton.ImageUrl = _image;
                oButton.Attributes.Add("onclick", _onclick);
                oCell.Controls.Add(oButton);
            }
            if (_row == null)
                _row = new TableRow();
            _row.Cells.Add(oCell);
            _table.Rows.Add(_row);
        }
        private void LoadControls(DataSet ds, string strId, ListBox _list, string _ph)
        {
            _list.Items.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["placeholder"].ToString() == _ph)
                    _list.Items.Add(new ListItem(dr["name"].ToString(), "i" + dr[strId].ToString()));
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            if ((Request.Form["hdnPH1"] != null) && (Request.Form["hdnPH1"] != ""))
                addControl(Request.Form["hdnPH1"], "PH1");
            if ((Request.Form["hdnPH2"] != null) && (Request.Form["hdnPH2"] != ""))
                addControl(Request.Form["hdnPH2"], "PH2");
            if ((Request.Form["hdnPH3"] != null) && (Request.Form["hdnPH3"] != ""))
                addControl(Request.Form["hdnPH3"], "PH3");
            if ((Request.Form["hdnPH4"] != null) && (Request.Form["hdnPH4"] != ""))
                addControl(Request.Form["hdnPH4"], "PH4");
            if ((Request.Form["hdnPH5"] != null) && (Request.Form["hdnPH5"] != ""))
                addControl(Request.Form["hdnPH5"], "PH5");
            if ((Request.Form["hdnPH6"] != null) && (Request.Form["hdnPH6"] != ""))
                addControl(Request.Form["hdnPH6"], "PH6");
            if ((Request.Form["hdnPH7"] != null) && (Request.Form["hdnPH7"] != ""))
                addControl(Request.Form["hdnPH7"], "PH7");
            if ((Request.Form["hdnPH8"] != null) && (Request.Form["hdnPH8"] != ""))
                addControl(Request.Form["hdnPH8"], "PH8");
            if ((Request.Form["hdnPH9"] != null) && (Request.Form["hdnPH9"] != ""))
                addControl(Request.Form["hdnPH9"], "PH9");
            if ((Request.Form["hdnPH10"] != null) && (Request.Form["hdnPH10"] != ""))
                addControl(Request.Form["hdnPH10"], "PH10");

            if ((Request.Form["hdnPH1del"] != null) && (Request.Form["hdnPH1del"] != ""))
                deleteControl(Request.Form["hdnPH1del"], "PH1");
            if ((Request.Form["hdnPH2del"] != null) && (Request.Form["hdnPH2del"] != ""))
                deleteControl(Request.Form["hdnPH2del"], "PH2");
            if ((Request.Form["hdnPH3del"] != null) && (Request.Form["hdnPH3del"] != ""))
                deleteControl(Request.Form["hdnPH3del"], "PH3");
            if ((Request.Form["hdnPH4del"] != null) && (Request.Form["hdnPH4del"] != ""))
                deleteControl(Request.Form["hdnPH4del"], "PH4");
            if ((Request.Form["hdnPH5del"] != null) && (Request.Form["hdnPH5del"] != ""))
                deleteControl(Request.Form["hdnPH5del"], "PH5");
            if ((Request.Form["hdnPH6del"] != null) && (Request.Form["hdnPH6del"] != ""))
                deleteControl(Request.Form["hdnPH6del"], "PH6");
            if ((Request.Form["hdnPH7del"] != null) && (Request.Form["hdnPH7del"] != ""))
                deleteControl(Request.Form["hdnPH7del"], "PH7");
            if ((Request.Form["hdnPH8del"] != null) && (Request.Form["hdnPH8del"] != ""))
                deleteControl(Request.Form["hdnPH8del"], "PH8");
            if ((Request.Form["hdnPH9del"] != null) && (Request.Form["hdnPH9del"] != ""))
                deleteControl(Request.Form["hdnPH9del"], "PH9");
            if ((Request.Form["hdnPH10del"] != null) && (Request.Form["hdnPH10del"] != ""))
                deleteControl(Request.Form["hdnPH10del"], "PH10");
            Reload();
        }
        private void addControl(string strHiddenData, string strPH)
        {
            string strField;
            int intOrder;
            while (strHiddenData != "")
            {
                strField = strHiddenData.Substring(0, strHiddenData.IndexOf("&"));
                strHiddenData = strHiddenData.Substring(strHiddenData.IndexOf("&") + 1);

                intOrder = Convert.ToInt32(strField.Substring(strField.IndexOf("_") + 1));
                strField = strField.Substring(0, strField.IndexOf("_"));

                if (strField.Substring(0, 1) == "i")
                    oPage.Update(Convert.ToInt32(strField.Substring(1)), strPH, Int32.Parse(lblId.Text), intOrder, 1);
                else
                    oPage.Add(oSchema.Add(Convert.ToInt32(strField.Substring(1))), strPH, Int32.Parse(lblId.Text), intOrder, 1);
            }
        }
        private void deleteControl(string strHiddenData, string strPH)
        {
            string strField;
            while (strHiddenData != "")
            {
                strField = strHiddenData.Substring(0, strHiddenData.IndexOf("&"));
                strHiddenData = strHiddenData.Substring(strHiddenData.IndexOf("&") + 1);
                if (strField.Substring(0, 1) == "i")
                    oPage.Delete(Convert.ToInt32(strField.Substring(1)));
            }
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
