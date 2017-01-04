using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;

namespace NCC.ClearView.Presentation.Web.Custom
{
    public class Tab
	{
        private string strMenu = "";
        private int intSelected = 0;
        private string strHidden = "";
        private string strTabMenu = "";
        private bool boolShowFirstTab = false;
        private bool boolSkipInitialize = false;
        private int intCount = 0;
        public Tab(string _hidden_control_id, int _selected_div, string _menu_id, bool _show_first_tab, bool _skip_initialize)
        {
            strMenu = _menu_id;
            intSelected = _selected_div;
            strHidden = _hidden_control_id;
            boolShowFirstTab = _show_first_tab;
            boolSkipInitialize = _skip_initialize;
        }
        public void AddTab(string _title, string _tooltip)
        {
            intCount++;
            strTabMenu += "<li id=\"li_tsmenu" + intCount.ToString() + "\"><a href=\"javascript:void(0);\" onclick=\"NewTab('" + strMenu + "','" + strHidden + "',this," + intCount.ToString() + ");\" title=\"" + (_tooltip == "" ? _title : _tooltip) + "\"><strong>" + _title + "</strong></a></li>";
        }
        public string GetTabs()
        {
            if (intSelected == 0 && boolShowFirstTab == true)
                intSelected = 1;
            string strReturn = "<table id=\"tsmenu" + intSelected.ToString() + "\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td><div id=\"ts_tabmenu\"><ul>" + strTabMenu + "</ul></div></td></tr><tr><td style=\"border-top:solid 1px #DDDDDD\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"1\" height=\"1\" /></td></tr></table>";
            if (boolSkipInitialize == false)
                strReturn += "<script type=\"text/javascript\">InitiateNewTab('" + strMenu + "','" + strHidden + "'," + intSelected.ToString() + ");</" + "script>";
            return strReturn;
        }

      

        public int SelectedTab
        {
            get { return intSelected; }
            set { intSelected = value; }
        }
	
    }
}
