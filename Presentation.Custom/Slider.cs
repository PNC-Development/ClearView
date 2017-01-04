using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;

namespace NCC.ClearView.Presentation.Web.Custom
{
    public partial class Slider : Control
	{
        public Slider()
        {
        }
        private string _sliderimage = "/images/slide_green.gif";
        private string _spacerimage = "/images/spacer.gif";
        private string _lineimage = "/images/gray_dot.gif";
        private string _markimage = "/images/slide_bar.gif";
        private string _width = "310";
        private string _divclass = "default";
        private string _percentclass = "default";
        private string _hidden = "hdnSlider";
        private string _startpercent = "0";
        private string _totalhours = "0";
        private string _showdiv = "false";
        private string _parentelement = "";
        private bool _parentdiv = false;
        private bool _enabled = true;
        private bool _restrictless = false;

        [Bindable(true), Category("Appearance"), DefaultValue("/images/slide_green.gif")]
        public string _SliderImage
        {
            get { return _sliderimage; }
            set { _sliderimage = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue("/images/spacer.gif")]
        public string _SpacerImage
        {
            get { return _spacerimage; }
            set { _spacerimage = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue("/images/gray_dot.gif")]
        public string _LineImage
        {
            get { return _lineimage; }
            set { _lineimage = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue("/images/slide_bar.gif")]
        public string _MarkImage
        {
            get { return _markimage; }
            set { _markimage = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue("310")]
        public string _Width
        {
            get { return _width; }
            set { _width = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue("default")]
        public string _DivClass
        {
            get { return _divclass; }
            set { _divclass = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue("default")]
        public string _PercentClass
        {
            get { return _percentclass; }
            set { _percentclass = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue("hdnSlider")]
        public string _Hidden
        {
            get { return _hidden; }
            set { _hidden = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue("0")]
        public string _StartPercent
        {
            get { return _startpercent; }
            set { _startpercent = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue("0")]
        public string _TotalHours
        {
            get { return _totalhours; }
            set { _totalhours = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue("false")]
        public string _ShowDiv
        {
            get { return _showdiv; }
            set { _showdiv = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string _ParentElement
        {
            get { return _parentelement; }
            set { _parentelement = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue(true)]
        public bool _ParentDiv
        {
            get { return _parentdiv; }
            set { _parentdiv = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue(true)]
        public bool _Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue(false)]
        public bool _RestrictLess
        {
            get { return _restrictless; }
            set { _restrictless = value; }
        }
        protected override void OnPreRender(EventArgs e)
        {
            System.Drawing.Image oImage = System.Drawing.Image.FromFile(this.Context.Server.MapPath(_SliderImage));
            int intWidth = (oImage.Width / 2);
            int intHeight = (oImage.Height / 2);
            string strJavascript = "";
            strJavascript += "<script type=\"text/javascript\">\r\n";
            strJavascript += "var oOldStart" + this.ID + " = null;var oStart" + this.ID + " = null;var oEnd" + this.ID + " = null;var oArrow" + this.ID + " = null;var oDiv" + this.ID + " = null;var oMoving" + this.ID + " = null;var xStartPos" + this.ID + " = null;var xEndPos" + this.ID + " = null;var yPos" + this.ID + " = null;var oHidden" + this.ID + " = null;var intOffSet" + this.ID + " = " + intWidth + ";var intOffHeight" + this.ID + " = " + intHeight + ";var boolDiv" + this.ID + " = null;var dblTotal" + this.ID + " = " + _TotalHours + ";\r\n";
            strJavascript += "addLoadEvent(function() {LoadSlider" + this.ID + "();});\r\n";
            strJavascript += "function LoadSlider" + this.ID + "(){oMoving" + this.ID + " = false;boolDiv" + this.ID + " = " + _ShowDiv + ";var oTD" + this.ID + " = document.getElementById('slide" + this.ID + "');\r\n";
            if (_Enabled == true)
            {
                strJavascript += "oTD" + this.ID + ".onmouseover = function() {oTD" + this.ID + ".style.cursor = 'hand';}\r\n";
                strJavascript += "oTD" + this.ID + ".onmouseout = function() {oTD" + this.ID + ".style.cursor = 'default';}\r\n";
                strJavascript += "oTD" + this.ID + ".onclick = function() {MoveSlider" + this.ID + "(parseInt(window.event.x) - intOffSet" + this.ID + ");}\r\n";
            }
            double dblPercent = double.Parse(_StartPercent);
            dblPercent = dblPercent / 100.00;
            strJavascript += "var oParent = " + (_ParentElement == "" ? "document.body" : "document.getElementById('" + _ParentElement + "')") + ";" + (_parentdiv == true ? "oParent = oParent.parentElement;" : "") + "var oTable" + this.ID + " = document.getElementById('tblSlider" + this.ID + "');oTable" + this.ID + ".width = " + _Width + ";if (dblTotal" + this.ID + " > 0.0) {oHidden" + this.ID + " = document.getElementById('" + _Hidden + "');var odblTemp = parseFloat('" + dblPercent.ToString("F") + "');oHidden" + this.ID + ".value = odblTemp * dblTotal" + this.ID + ";oStart" + this.ID + " = document.getElementById('slideStart" + this.ID + "');oEnd" + this.ID + " = document.getElementById('slideEnd" + this.ID + "');oArrow" + this.ID + " = document.createElement('IMG');oArrow" + this.ID + ".src = '" + _SliderImage + "';oArrow" + this.ID + ".style.position = 'absolute';oArrow" + this.ID + ".style.display = 'none';oArrow" + this.ID + ".title = '" + (_StartPercent == "0" ? "Click to move" : _StartPercent.ToString() + "% of " + _TotalHours + " HRs") + "';oArrow" + this.ID + ".style.zIndex = 200;oParent.appendChild(oArrow" + this.ID + ");oDiv" + this.ID + " = document.createElement('DIV');oDiv" + this.ID + ".className = '" + _DivClass + "';oDiv" + this.ID + ".style.width = '40';oDiv" + this.ID + ".style.height = '16';oDiv" + this.ID + ".style.backgroundColor = '#FFFFDD';oDiv" + this.ID + ".style.border = 'solid 1px black';oDiv" + this.ID + ".style.padding = '2';oDiv" + this.ID + ".align = 'right';oDiv" + this.ID + ".style.position = 'absolute';oDiv" + this.ID + ".style.display = '" + (_StartPercent != "0" && _ShowDiv == "true" ? "inline" : "none") + "';oDiv" + this.ID + ".innerText = '" + _StartPercent + "%';oParent.appendChild(oDiv" + this.ID + ");for (ii=1; ii<10; ii++) {var oBar" + this.ID + " = document.createElement('IMG');oBar" + this.ID + ".id = 'IMGBar' + ii + '" + this.ID + "';oBar" + this.ID + ".src = '" + _MarkImage + "';oBar" + this.ID + ".style.position = 'absolute';oBar" + this.ID + ".style.zIndex = 100;oParent.appendChild(oBar" + this.ID + ");}ConfigureSlider" + this.ID + "();\r\n";
            if (_Enabled == true)
            {

                strJavascript += "oArrow" + this.ID + ".onmouseover = function() {oArrow" + this.ID + ".style.cursor = 'hand';}\r\n";
                strJavascript += "oArrow" + this.ID + ".onmouseout = function() {oArrow" + this.ID + ".style.cursor = 'default';}\r\n";
                strJavascript += "oArrow" + this.ID + ".onmousedown = function() {oMoving" + this.ID + " = true;}\r\n";
                strJavascript += "oArrow" + this.ID + ".ondrag = function() {return false;}\r\n";
                strJavascript += "document.attachEvent(\"onmousemove\", MouseMove" + this.ID + ");\r\n";
                strJavascript += "document.attachEvent(\"onmouseup\", MouseUp" + this.ID + ");\r\n";
                strJavascript += "document.body.attachEvent(\"onmouseleave\", MouseLeave" + this.ID + ");\r\n";
            }
            strJavascript += "window.attachEvent(\"onresize\", WindowResize" + this.ID + ");\r\n";
            if (_ParentElement != "")
                strJavascript += "oParent.attachEvent(\"onpropertychange\", ActivateSlider" + this.ID + ");\r\n";
            strJavascript += "}}\r\n";
            strJavascript += "function ActivateSlider" + this.ID + "() {oArrow" + this.ID + ".style.display='none';setTimeout(\"ConfigureSlider" + this.ID + "()\",100);}\r\n";
            if (_Enabled == true)
            {
                strJavascript += "function MouseUp" + this.ID + "() {StopMove" + this.ID + "();}\r\n";
                strJavascript += "function MouseMove" + this.ID + "() {if (oMoving" + this.ID + " == true) {oArrow" + this.ID + ".style.posTop = yPos" + this.ID + ";MoveSlider" + this.ID + "(parseInt(window.event.x) - intOffSet" + this.ID + ");}}\r\n";
                strJavascript += "function MouseLeave" + this.ID + "() {StopMove" + this.ID + "();}\r\n";
            }
            strJavascript += "function StopMove" + this.ID + "() {oMoving" + this.ID + " = false;oDiv" + this.ID + ".style.display='none';}\r\n";
            strJavascript += "function WindowResize" + this.ID + "() {StopMove" + this.ID + "();ConfigureSlider" + this.ID + "();}\r\n";
            strJavascript += "function ConfigureSlider" + this.ID + "() {xStartPos" + this.ID + " = parseInt(findPosX(oStart" + this.ID + ") + intOffSet" + this.ID + ");xEndPos" + this.ID + " = parseInt(findPosX(oEnd" + this.ID + "));var intTotal = xEndPos" + this.ID + " - xStartPos" + this.ID + ";yPos" + this.ID + " = findPosY(oStart" + this.ID + ") - intOffHeight" + this.ID + ";var intStart = parseFloat(oHidden" + this.ID + ".value) / dblTotal" + this.ID + " * 100;intStart = intStart * .01;oArrow" + this.ID + ".style.posLeft = (intStart * intTotal) + xStartPos" + this.ID + " - intOffSet" + this.ID + ";oOldStart" + this.ID + " = oArrow" + this.ID + ".style.posLeft;oArrow" + this.ID + ".style.posTop = yPos" + this.ID + ";oArrow" + this.ID + ".style.display='inline';oDiv" + this.ID + ".style.posLeft = (intStart * intTotal) + xStartPos" + this.ID + " - intOffSet" + this.ID + " - 15;oDiv" + this.ID + ".style.posTop = yPos" + this.ID + " + 25;intTotal = intTotal / 10;for (ii=1; ii<10; ii++) {var intSpot = intTotal * ii;intSpot = intSpot + xStartPos" + this.ID + ";var oBar" + this.ID + " = document.getElementById('IMGBar' + ii + '" + this.ID + "');oBar" + this.ID + ".style.posLeft = intSpot;oBar" + this.ID + ".style.posTop = yPos" + this.ID + " + 2;}xStartPos" + this.ID + " = xStartPos" + this.ID + " - intOffSet" + this.ID + ";}\r\n";
            strJavascript += "function MoveSlider" + this.ID + "(intSpot) {if (" + (_RestrictLess == false ? "true" : "parseInt(oOldStart" + this.ID + ") < parseInt(intSpot)") + ") {var intStart = xStartPos" + this.ID + ";var intEnd = xEndPos" + this.ID + ";if (intSpot < intStart){intSpot = intStart;}else if (intSpot > (intEnd - intOffSet" + this.ID + ")){intSpot = intEnd - intOffSet" + this.ID + ";}oArrow" + this.ID + ".style.posLeft = intSpot;oDiv" + this.ID + ".style.posLeft = intSpot - 15;var intTotal = intEnd - intStart - intOffSet" + this.ID + ";intSpot = intSpot - intStart;if (intSpot < 0){alert('There was a problem with the slider control : ' + intSpot);}if (intSpot == 0) {oDiv" + this.ID + ".innerText = '0%';oDiv" + this.ID + ".style.display = 'none';oHidden" + this.ID + ".value = '0';}else {var intPercent = parseFloat(intSpot / intTotal);intPercent = intPercent * 100;var strPercent = intPercent.toString();if (strPercent.indexOf('.') > -1){strPercent = strPercent.substring(0, strPercent.indexOf('.'));}oDiv" + this.ID + ".innerText = strPercent + '%';if (boolDiv" + this.ID + " == true){oDiv" + this.ID + ".style.display = 'inline';}var odblTemp = parseFloat(strPercent);odblTemp = odblTemp / 100.00;oHidden" + this.ID + ".value = odblTemp * dblTotal" + this.ID + ";oArrow" + this.ID + ".title = strPercent + '% of " + _TotalHours + " HRs';}}}\r\n";
            strJavascript += "<" + "/" + "script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "SliderCustomControl" + this.ID, strJavascript);
            base.OnPreRender(e);
        }
        protected override void Render(HtmlTextWriter output)
        {
            output.Write("<table id=\"tblSlider" + this.ID + "\" cellpadding=\"0\" cellpadding=\"0\" border=\"0\">");
            output.Write("<tr>");
            output.Write("<td class=\"" + _PercentClass + "\">0%</td>");
            output.Write("<td><img src=\"" + _SpacerImage + "\" border=\"0\" width=\"5\" height=\"1\" /></td>");
            output.Write("<td><a id=\"slideStart" + this.ID + "\" /></td>");
            output.Write("<td id=\"slide" + this.ID + "\" width=\"100%\" background=\"" + _LineImage + "\"><img src=\"" + _LineImage + "\" border=\"0\" /></td>");
            output.Write("<td><a id=\"slideEnd" + this.ID + "\" /></td>");
            output.Write("<td><img src=\"" + _SpacerImage + "\" border=\"0\" width=\"10\" height=\"1\" /></td>");
            output.Write("<td class=\"" + _PercentClass + "\">100%</td>");
            output.Write("</tr>");
            output.Write("</table>");
            output.Write("<input type=\"hidden\" id=\"" + _Hidden + "\" name=\"" + _Hidden + "\" />");
        }
    }
}
