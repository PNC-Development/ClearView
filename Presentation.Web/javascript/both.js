function SetCookie(sName, sValue)
{
    expireDate=new Date;
    expireDate.setMonth(expireDate.getMonth()+1);
    document.cookie = sName + "=" + escape(sValue) + ";expires=" + expireDate.toGMTString();
}
function GetCookie(sName)
{
    var aCookie = document.cookie.split("; ");
    for (var i=0; i < aCookie.length; i++)
    {
	    var aCrumb = aCookie[i].split("=");
	    if (sName == aCrumb[0])
		    return unescape(aCrumb[1]);
    }
    return "";
} 
function NewTab(oDiv, oHidden, oA, intTab) {
    if (oA != null) {
        var oLI = oA.parentElement;
        var strLI = oLI.id;
        strLI = strLI.substring(strLI.indexOf("_") + 1);
        var oTable = oLI.parentElement;
        while (oTable.tagName != "TABLE")
            oTable = oTable.parentElement;
        oTable.id = strLI;
    }
    oDiv = document.getElementById(oDiv);
    if (oDiv != null)
    {
	    var oDivs = oDiv.getElementsByTagName("DIV");
	    var oDivSelected = oDiv;
	    var intDivCount = 0;
	    for (var ii=0; ii<oDivs.length; ii++) {
	        if (oDivs[ii].parentElement == oDiv) {
	            intDivCount = intDivCount + 1;
	            if (intDivCount == intTab)
	                oDivSelected = oDivs[ii];
                oDivs[ii].style.display = "none";
            }
        }	        
        oDivSelected.style.display = "inline";
	    if (oHidden != null && oHidden != "") {
	        oHidden = document.getElementById(oHidden);
	        oHidden.value = intTab;
	    }
	}
}
function InitiateNewTab(oDiv, oHidden, intTab) {
    addDOMLoadEvent(function() {
	    InitiateNewTab2(oDiv, oHidden, intTab);
    });
}
function InitiateNewTab2(oDiv, oHidden, intTab) {
    NewTab(oDiv, oHidden, null, intTab);
}
addDOMLoadEvent = (function(){
    // create event function stack
    var load_events = [],
        load_timer,
        script,
        done,
        exec,
        old_onload,
        init = function () {
            done = true;

            // kill the timer
            clearInterval(load_timer);

            // execute each function in the stack in the order they were added
            while (exec = load_events.shift())
                exec();

            if (script) script.onreadystatechange = '';
            HideDynamicWait();
        };

    return function (func) {
        // if the init function was already ran, just run this function now and stop
        if (done) return func();

        if (!load_events[0]) {
            //alert('starting');
            // for Mozilla/Opera9
            if (document.addEventListener)
                document.addEventListener("DOMContentLoaded", init, false);

            // for Internet Explorer
            document.write('<script id=\"__ie_onload\" defer src=\"javascript:void(0);\"><\/script>');
            script = document.getElementById("__ie_onload");
            script.onreadystatechange = function() {
                //if (this.readyState == "complete")
                ShowDynamicWait();
                if (this.readyState == "complete")
                    init(); // call the onload handler
            };

            // for Safari
            if (/WebKit/i.test(navigator.userAgent)) { // sniff
                load_timer = setInterval(function() {
                    if (/loaded|complete/.test(document.readyState))
                        init(); // call the onload handler
                }, 10);
            }

            // for other browsers set the window.onload, but also execute the old window.onload
            old_onload = window.onload;
            window.onload = function() {
                init();
                if (old_onload) old_onload();
            };
        }

        load_events.push(func);
    }
})();
function UpdateListHidden(oDDL, oHidden) {
    oDDL = document.getElementById(oDDL);
    oHidden = document.getElementById(oHidden);
    oHidden.value = "";
    for (var ii=0; ii<oDDL.length; ii++) {
        if (oDDL.options[ii].selected)
            oHidden.value = oHidden.value + oDDL.options[ii].value + ";";
    }
}
function UpdateDropDownHidden(oDDL, oHidden) {
    oDDL = document.getElementById(oDDL);
    oHidden = document.getElementById(oHidden);
    oHidden.value = oDDL.options[oDDL.selectedIndex].value;
}
function UpdateTextHidden(oText, oHidden) {
    oText = document.getElementById(oText);
    oHidden = document.getElementById(oHidden);
    oHidden.value = oText.value;
}
function AlertWindow(strText, strHeader) {
    var intWidth = 350;
    var intHeight = 200;
    if (strHeader != null) {
        if (strText != null)
            ShowPanel('/frame/saved.aspx?alert=' + strText + '&header=' + strHeader,intWidth,intHeight,true);
        else
            ShowPanel('/frame/saved.aspx?header=' + strHeader,intWidth,intHeight,true);
    }
    else {
        if (strText != null)
            ShowPanel('/frame/saved.aspx?alert=' + strText,intWidth,intHeight,true);
        else
            ShowPanel('/frame/saved.aspx',intWidth,intHeight,true);
    }
    setTimeout("HidePanel()",3000);
    return true;
}
var oCountdown = null;
function StartCountdown(strControl) {
    oCountdown = document.getElementById(strControl);
    oCountdown.innerText = "2";
    setTimeout("Countdown()",1000);
}
function Countdown() {
    var intTimer = parseInt(oCountdown.innerText);
    intTimer = intTimer - 1;
    oCountdown.innerText = intTimer;
    setTimeout("Countdown()",1000);
}
function LoadWait() {
    ShowPanel('/frame/spinning.htm',60,60,true);
    return true;
}
var oDynamicWait = null;
function ShowDynamicWait() {
    var oWait = document.getElementById("spnWait");
    if (oWait != null)
    {
        oDynamicWait = oWait;
        window.top.document.body.scroll = "NO";
	    //window.top.document.body.style.filter = "alpha(opacity=40)";
        oDynamicWait.style.posLeft = ((parseInt(window.top.document.body.clientWidth) / 2) - 30) + parseInt(window.top.document.body.scrollLeft);
        oDynamicWait.style.posTop = ((parseInt(window.top.document.body.clientHeight) / 2) - 80) + parseInt(window.top.document.body.scrollTop);
    }
}
function HideDynamicWait() {
    if (oDynamicWait == null)
        oDynamicWait = document.getElementById("spnWait");
    if (oDynamicWait != null) {
        window.top.document.body.scroll = "YES";
	    //window.top.document.body.style.filter = "";
        oDynamicWait.style.display = "none";
    }
    oDynamicWait = null;
}

var oActiveXEnvironment = null;
var oEnvironmentForecast = null;
// **************************************************************************
// Class is a drop down list, and environment is a drop down list
// **************************************************************************
var oDDLEnvironment = null;
function PopulateEnvironments(_classid, _environment, _forecast) {
    _classid = document.getElementById(_classid);
    _environment = document.getElementById(_environment);
    if (_forecast == null)
        oEnvironmentForecast = 0;
    else
        oEnvironmentForecast = _forecast;
    PopulateEnvironments2(_classid, _environment);
}
function PopulateEnvironments2(_classid, _environment) {
    oDDLEnvironment = _environment;
    oActiveXEnvironment = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXEnvironment.onreadystatechange = PopulateEnvironments_a;
    oActiveXEnvironment.open("GET", "/frame/ajax/ajax_environments.aspx?u=GET", false);
    if (oEnvironmentForecast == null)
        oEnvironmentForecast = 0;
    oActiveXEnvironment.send("<ajax><value>" + escape(_classid.options[_classid.selectedIndex].value) + "</value><value>" + oEnvironmentForecast + "</value></ajax>");
}
function PopulateEnvironments_a() {
    if (oActiveXEnvironment.readyState == 4)
    {
        if (oActiveXEnvironment.status == 200) {
            var or = oActiveXEnvironment.responseXML.documentElement.childNodes;
            oDDLEnvironment.length = 0;
            oDDLEnvironment.disabled = false;
            PopulateEnvironment(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateEnvironment(or) {
    var strScript = "";
    if (or.length > 0) {
		var oOption = document.createElement("OPTION");
		oDDLEnvironment.add(oOption);
		oOption.text = " -- SELECT --";
		oOption.value = "0";
        for (var ii=0; ii<or.length; ii=ii+2) {
		    oOption = document.createElement("OPTION");
		    oDDLEnvironment.add(oOption);
		    oOption.text = or[ii+1].childNodes[0].text;
		    oOption.value = or[ii].childNodes[0].text;
        }
    }
    else {
		var oOption = document.createElement("OPTION");
		oDDLEnvironment.add(oOption);
		oOption.text = " -- Please select an Environment --";
		oOption.value = "0";
		oDDLEnvironment.disabled = true;
    }
}
// **************************************************************************
// Class is a listbox, and environment is a listbox
// **************************************************************************
var oListEnvironment = null;
function PopulateEnvironmentsList(_classid, _environment, _forecast) {
    _classid = document.getElementById(_classid);
    _environment = document.getElementById(_environment);
    if (_forecast == null)
        oEnvironmentForecast = 0;
    else
        oEnvironmentForecast = _forecast;
    PopulateEnvironmentsList2(_classid, _environment);
}
function PopulateEnvironmentsList2(_classid, _environment) {
    oListEnvironment = _environment;
    oActiveXEnvironment = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXEnvironment.onreadystatechange = PopulateEnvironmentsList_a;
    oActiveXEnvironment.open("GET", "/frame/ajax/ajax_environments_multiple.aspx?u=GET", false);
    var strClasses = "";
    for (var ii=0; ii<_classid.length; ii++) {
        if (_classid.options[ii].selected)
            strClasses = strClasses + _classid.options[ii].value + ",";
    }
    if (oEnvironmentForecast == null)
        oEnvironmentForecast = 0;
    oActiveXEnvironment.send("<ajax><value>" + strClasses + "</value><value>" + oEnvironmentForecast + "</value></ajax>");
}
function PopulateEnvironmentsList_a() {
    if (oActiveXEnvironment.readyState == 4)
    {
        if (oActiveXEnvironment.status == 200) {
            var or = oActiveXEnvironment.responseXML.documentElement.childNodes;
            oListEnvironment.length = 0;
            oListEnvironment.disabled = false;
            PopulateEnvironmentList(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateEnvironmentList(or) {
    var strScript = "";
    if (or.length > 0) {
		var oOption = document.createElement("OPTION");
		oListEnvironment.add(oOption);
		oOption.text = " -- ALL --";
		oOption.value = "0";
        for (var ii=0; ii<or.length; ii=ii+2) {
		    oOption = document.createElement("OPTION");
		    oListEnvironment.add(oOption);
		    oOption.text = or[ii+1].childNodes[0].text;
		    oOption.value = or[ii].childNodes[0].text;
        }
    }
    else {
		var oOption = document.createElement("OPTION");
		oListEnvironment.add(oOption);
		oOption.text = " -- Please select an Environment --";
		oOption.value = "0";
		oListEnvironment.disabled = true;
    }
}
// **************************************************************************
// OS is a drop down list, and SP is a drop down list
// **************************************************************************
var oDDLServicePack = null;
function PopulateServicePacks(_osid, _sp) {
    _osid = document.getElementById(_osid);
    _sp = document.getElementById(_sp);
    PopulateServicePacks2(_osid, _sp);
}
function PopulateServicePacks2(_osid, _sp) {
    oDDLServicePack = _sp;
    oActiveXEnvironment = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXEnvironment.onreadystatechange = PopulateServicePacks_a;
    oActiveXEnvironment.open("GET", "/frame/ajax/ajax_sps.aspx?u=GET", false);
    oActiveXEnvironment.send("<ajax>" + escape(_osid.options[_osid.selectedIndex].value) + "</ajax>");
}
function PopulateServicePacks_a() {
    if (oActiveXEnvironment.readyState == 4)
    {
        if (oActiveXEnvironment.status == 200) {
            var or = oActiveXEnvironment.responseXML.documentElement.childNodes;
            oDDLServicePack.length = 0;
            oDDLServicePack.disabled = false;
            PopulateServicePack(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateServicePack(or) {
    var strScript = "";
    if (or.length > 0) {
		var oOption = document.createElement("OPTION");
		oDDLServicePack.add(oOption);
		oOption.text = " -- SELECT --";
		oOption.value = "0";
        for (var ii=0; ii<or.length; ii=ii+2) {
		    oOption = document.createElement("OPTION");
		    oDDLServicePack.add(oOption);
		    oOption.text = or[ii+1].childNodes[0].text;
		    oOption.value = or[ii].childNodes[0].text;
        }
    }
    else {
		var oOption = document.createElement("OPTION");
		oDDLServicePack.add(oOption);
		oOption.text = " -- Please select an Operating System --";
		oOption.value = "0";
		oDDLServicePack.disabled = true;
    }
}


var oActiveXSegment = null;
// **************************************************************************
// Organization is a drop down list, and segment is a drop down list
// **************************************************************************
var oDDLSegment = null;
function PopulateSegments(_organizationid, _segment) {
    _organizationid = document.getElementById(_organizationid);
    _segment = document.getElementById(_segment);
    PopulateSegments2(_organizationid, _segment);
}
function PopulateSegments2(_organizationid, _segment) {
    oDDLSegment = _segment;
    oActiveXSegment = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXSegment.onreadystatechange = PopulateSegments_a;
    oActiveXSegment.open("GET", "/frame/ajax/ajax_segments.aspx?u=GET", false);
    oActiveXSegment.send("<ajax>" + escape(_organizationid.options[_organizationid.selectedIndex].value) + "</ajax>");
}
function PopulateSegments_a() {
    if (oActiveXSegment.readyState == 4)
    {
        if (oActiveXSegment.status == 200) {
            var or = oActiveXSegment.responseXML.documentElement.childNodes;
            oDDLSegment.length = 0;
            oDDLSegment.disabled = false;
            PopulateSegment(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateSegment(or) {
    var strScript = "";
    if (or.length > 0) {
		var oOption = document.createElement("OPTION");
		oDDLSegment.add(oOption);
		oOption.text = " -- NONE --";
		oOption.value = "0";
        for (var ii=0; ii<or.length; ii=ii+2) {
		    oOption = document.createElement("OPTION");
		    oDDLSegment.add(oOption);
		    oOption.text = or[ii+1].childNodes[0].text;
		    oOption.value = or[ii].childNodes[0].text;
        }
    }
    else {
		var oOption = document.createElement("OPTION");
		oDDLSegment.add(oOption);
		oOption.text = " -- NONE --";
		oOption.value = "0";
    }
}



var oActiveXSubApplication = null;
// **************************************************************************
// Application is a drop down list, and SubApplication is a drop down list
// **************************************************************************
var oDDLSubApplication = null;
var oDIVSubApplication = null;
function PopulateSubApplications(_applicationid, _subapplication, _applicationdiv) {
    _applicationid = document.getElementById(_applicationid);
    _subapplication = document.getElementById(_subapplication);
    _applicationdiv = document.getElementById(_applicationdiv);
    PopulateSubApplications2(_applicationid, _subapplication, _applicationdiv);
}
function PopulateSubApplications2(_applicationid, _subapplication, _applicationdiv) {
    oDDLSubApplication = _subapplication;
    oDIVSubApplication = _applicationdiv;
    oActiveXSubApplication = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXSubApplication.onreadystatechange = PopulateSubApplications_a;
    oActiveXSubApplication.open("GET", "/frame/ajax/ajax_subapplications.aspx?u=GET", false);
    oActiveXSubApplication.send("<ajax>" + escape(_applicationid.options[_applicationid.selectedIndex].value) + "</ajax>");
}
function PopulateSubApplications_a() {
    if (oActiveXSubApplication.readyState == 4)
    {
        if (oActiveXSubApplication.status == 200) {
            var or = oActiveXSubApplication.responseXML.documentElement.childNodes;
            oDDLSubApplication.length = 0;
            oDDLSubApplication.disabled = false;
            PopulateSubApplication(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateSubApplication(or) {
    var strScript = "";
    if (or.length > 0) {
        oDIVSubApplication.style.display = "inline";
		var oOption = document.createElement("OPTION");
		oDDLSubApplication.add(oOption);
		oOption.text = " -- SELECT --";
		oOption.value = "0";
        for (var ii=0; ii<or.length; ii=ii+2) {
		    oOption = document.createElement("OPTION");
		    oDDLSubApplication.add(oOption);
		    oOption.text = or[ii+1].childNodes[0].text;
		    oOption.value = or[ii].childNodes[0].text;
        }
    }
    else {
        oDIVSubApplication.style.display = "none";
    }
}


var oActiveXSoftwareComponent = null;
// **************************************************************************
// Product is a drop down list, and Component is a drop down list
// **************************************************************************
var oDDLSoftwareComponent = null;
function PopulateSoftwareComponents(_productid, __componentid) {
    _productid = document.getElementById(_productid);
    __componentid = document.getElementById(__componentid);
    PopulateSoftwareComponents2(_productid, __componentid);
}
function PopulateSoftwareComponents2(_productid, __componentid) {
    oDDLSoftwareComponent = __componentid;
    oActiveXSoftwareComponent = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXSoftwareComponent.onreadystatechange = PopulateSoftwareComponents_a;
    oActiveXSoftwareComponent.open("GET", "/frame/ajax/ajax_software_components.aspx?u=GET", false);
    oActiveXSoftwareComponent.send("<ajax>" + escape(_productid.options[_productid.selectedIndex].value) + "</ajax>");
}
function PopulateSoftwareComponents_a() {
    if (oActiveXSoftwareComponent.readyState == 4)
    {
        if (oActiveXSoftwareComponent.status == 200) {
            var or = oActiveXSoftwareComponent.responseXML.documentElement.childNodes;
            oDDLSoftwareComponent.length = 0;
            oDDLSoftwareComponent.disabled = false;
            PopulateSoftwareComponent(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateSoftwareComponent(or) {
    var strScript = "";
    if (or.length > 0) {
		var oOption = document.createElement("OPTION");
		oDDLSoftwareComponent.add(oOption);
		oOption.text = " -- SELECT --";
		oOption.value = "0";
        for (var ii=0; ii<or.length; ii=ii+2) {
		    oOption = document.createElement("OPTION");
		    oDDLSoftwareComponent.add(oOption);
		    oOption.text = or[ii+1].childNodes[0].text;
		    oOption.value = or[ii].childNodes[0].text;
        }
    }
    else {
		var oOption = document.createElement("OPTION");
		oListEnvironment.add(oOption);
		oOption.text = " -- Please select a Product --";
		oOption.value = "0";
		oListEnvironment.disabled = true;
    }
}


var oActiveXPort = null;
// **************************************************************************
// Switch is a drop down list, and Port is a drop down list
// **************************************************************************
var oDDLBlade = null;
var oDDLBladeID = null;
var oTXTFex = null;
var oDDLPort = null;
var oTXTInterface = null;
var oLBLInterface = null;
var oHiddenBlade = null;
function PopulateSwitchs(_switchid, _bladeid, _fexid, _portid, _intefaceTXT, _interfaceLBL, _hdnblade) {
    _switchid = document.getElementById(_switchid);
    oDDLBladeID = _bladeid;
    oDDLBlade = document.getElementById(_bladeid);
    if (_fexid != null)
        oTXTFex = document.getElementById(_fexid);
    else
        oTXTFex = null;
    oDDLPort = document.getElementById(_portid);
    oTXTInterface = document.getElementById(_intefaceTXT);
    if (_interfaceLBL != null)
        oLBLInterface = document.getElementById(_interfaceLBL);
    else
        oLBLInterface = null;
    oHiddenBlade = _hdnblade;
    PopulateSwitchs2(_switchid);
}
function PopulateSwitchs2(_switchid) {
    oActiveXPort = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXPort.onreadystatechange = PopulateSwitchs_a;
    oActiveXPort.open("GET", "/frame/ajax/ajax_switches.aspx?u=GET", false);
    oActiveXPort.send("<ajax>" + escape(_switchid.options[_switchid.selectedIndex].value) + "</ajax>");
}
function PopulateSwitchs_a() {
    if (oActiveXPort.readyState == 4)
    {
        if (oActiveXPort.status == 200) {
            var or = oActiveXPort.responseXML.documentElement.childNodes;
            oDDLBlade.length = 0;
            oDDLBlade.disabled = false;
            oDDLPort.length = 0;
            oDDLPort.disabled = false;
            PopulateSwitch(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateSwitch(or) {
	var oOption = document.createElement("OPTION");
    if (or.length > 0) {
        var intBlades = parseInt(or[0].childNodes[0].text);
        if (intBlades > 1) {
		    oOption = document.createElement("OPTION");
		    oDDLBlade.add(oOption);
		    oOption.text = "-- SELECT --";
		    oOption.value = "0";
        }
        for (var ii=1; ii<=intBlades; ii++) {
		    oOption = document.createElement("OPTION");
		    oDDLBlade.add(oOption);
		    oOption.text = "Blade # " + ii;
		    oOption.value = ii;
        }
        if (intBlades == 1) {
            oDDLBlade.disabled = true;
            UpdateDropDownHidden(oDDLBladeID, oHiddenBlade);
        }
        var intPorts = parseInt(or[1].childNodes[0].text);
	    oOption = document.createElement("OPTION");
	    oDDLPort.add(oOption);
	    oOption.text = "-- SELECT --";
	    oOption.value = "0";
        for (var ii=1; ii<=intPorts; ii++) {
		    oOption = document.createElement("OPTION");
		    oDDLPort.add(oOption);
		    oOption.text = "Port # " + ii;
		    oOption.value = ii;
        }
        var intNexus = parseInt(or[2].childNodes[0].text);
        if (intNexus == "1" && oTXTFex != null) {
            oLBLInterface.style.display = "inline";
            oTXTInterface.disabled = true;
            oTXTInterface.style.display = "none";
            oDDLBlade.style.display = "none";
            oTXTFex.style.display = "inline";
            oTXTFex.focus();
        }
        else {
            oLBLInterface.style.display = "none";
            oTXTInterface.disabled = false;
            oTXTInterface.style.display = "inline";
            oDDLBlade.style.display = "inline";
            if (oTXTFex != null)
                oTXTFex.style.display = "none";
            if (oDDLBlade.disabled == false)
                oDDLBlade.focus();
            else
                oDDLPort.focus();
        }
    }
    else {
        oDDLBlade.style.display = "inline";
        if (oTXTFex != null)
            oTXTFex.style.display = "none";
		oDDLBlade.add(oOption);
        oDDLBlade.disabled = true;
		oOption.text = " -- SELECT A SWITCH --";
		oOption.value = "0";
		oOption = document.createElement("OPTION");
		oDDLPort.add(oOption);
        oDDLPort.disabled = true;
		oOption.text = " -- SELECT A SWITCH --";
		oOption.value = "0";
    }
}
function UpdateNetworkInterface(oText, oDDL, oLabel) {
    oText = document.getElementById(oText);
    oDDL = document.getElementById(oDDL);
    oLabel = document.getElementById(oLabel);
    for (var ii=0; ii<oDDL.length; ii++) {
        if (oDDL.options[ii].selected) {
            if (oDDL.options[ii].value > 0 || oText.value != "")
                oLabel.innerHTML = oText.value + "/" + oDDL.options[ii].value;
            else
                oLabel.innerHTML = "<i>Not Configured</i>";
        }
    }
}


// **************************************************************************
// LOCATION (State, City and Address)
// **************************************************************************
function UpdateDropDownLocation(oDDL, oHidden, oDivS, oDivC, oDivA) {
    oDDL = document.getElementById(oDDL);
    oDivS = document.getElementById(oDivS);
    oDivC = document.getElementById(oDivC);
    oDivA = document.getElementById(oDivA);
    if (oDDL.options[oDDL.selectedIndex].value == "-1") {
        oDivS.style.display = "inline";
        oDivC.style.display = "inline";
        oDivA.style.display = "inline";
    }
    else {
        oDivS.style.display = "none";
        oDivC.style.display = "none";
        oDivA.style.display = "none";
        oHidden = document.getElementById(oHidden);
        oHidden.value = oDDL.options[oDDL.selectedIndex].value;
    }
}
var oActiveXLocation = null;
var oDDLCity = null;
function LoadLocationCity(_object) {
    oDDLCity = document.getElementById(_object);
}
function PopulateCitys(_state, _city) {
    _state = document.getElementById(_state);
    _city = document.getElementById(_city);
    PopulateCitys2(_state, _city);
}
function PopulateCitys2(_state, _city) {
    oDDLCity = _city;
    oActiveXLocation = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXLocation.onreadystatechange = PopulateCitys_a;
    oActiveXLocation.open("GET", "/frame/ajax/ajax_citys.aspx?u=GET", false);
    oActiveXLocation.send("<ajax>" + escape(_state.options[_state.selectedIndex].value) + "</ajax>");
}
function PopulateCitys_a() {
    if (oActiveXLocation.readyState == 4)
    {
        if (oActiveXLocation.status == 200) {
            var or = oActiveXLocation.responseXML.documentElement.childNodes;
            oDDLCity.length = 0;
            oDDLCity.disabled = false;
            PopulateCity(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateCity(or) {
    var strScript = "";
	var oOption = document.createElement("OPTION");
    if (or.length > 0) {
		oDDLCity.add(oOption);
		oOption.text = " -- SELECT --";
		oOption.value = "0";
        for (var ii=0; ii<or.length; ii=ii+2) {
		    oOption = document.createElement("OPTION");
		    oDDLCity.add(oOption);
		    oOption.text = or[ii+1].childNodes[0].text;
		    oOption.value = or[ii].childNodes[0].text;
        }
    }
    else {
		oDDLCity.add(oOption);
		oOption.text = "-- Select a State --";
		oOption.value = "0";
		oDDLCity.disabled = true;
    }
	oOption = document.createElement("OPTION");
    oDDLAddress.length = 0;
	oDDLAddress.add(oOption);
	oOption.text = "-- Select a City --";
	oOption.value = "0";
	oDDLAddress.disabled = true;
}
var oDDLAddress = null;
function LoadLocationAddress(_object) {
    oDDLAddress = document.getElementById(_object);
}
function PopulateAddresss(_city, _address) {
    _city = document.getElementById(_city);
    _address = document.getElementById(_address);
    PopulateAddresss2(_city, _address);
}
function PopulateAddresss2(_city, _address) {
    oDDLAddress = _address;
    oActiveXLocation = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXLocation.onreadystatechange = PopulateAddresss_a;
    oActiveXLocation.open("GET", "/frame/ajax/ajax_addresses.aspx?u=GET", false);
    oActiveXLocation.send("<ajax>" + escape(_city.options[_city.selectedIndex].value) + "</ajax>");
}
function PopulateAddresss_a() {
    if (oActiveXLocation.readyState == 4)
    {
        if (oActiveXLocation.status == 200) {
            var or = oActiveXLocation.responseXML.documentElement.childNodes;
            oDDLAddress.length = 0;
            oDDLAddress.disabled = false;
            PopulateAddress(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateAddress(or) {
    var strScript = "";
    if (or.length > 0) {
		var oOption = document.createElement("OPTION");
		oDDLAddress.add(oOption);
		oOption.text = " -- SELECT --";
		oOption.value = "0";
        for (var ii=0; ii<or.length; ii=ii+2) {
		    oOption = document.createElement("OPTION");
		    oDDLAddress.add(oOption);
		    oOption.text = or[ii+1].childNodes[0].text;
		    oOption.value = or[ii].childNodes[0].text;
        }
    }
    else {
		var oOption = document.createElement("OPTION");
		oDDLAddress.add(oOption);
		oOption.text = "-- Select a City --";
		oOption.value = "0";
		oDDLAddress.disabled = true;
    }
}
function ResetDropDownHidden(oHidden) {
    oHidden = document.getElementById(oHidden);
    oHidden.value = "0";
}
function LoadAddresses(oState, oCity, oAddress, oCommon, oHidden, strState, strCity, strAddress) {
    oCommon = document.getElementById(oCommon);
    oState = document.getElementById(oState);
    oCity = document.getElementById(oCity);
    oAddress = document.getElementById(oAddress);
    var boolAddressFound = LoadAddressCommon(oCommon, strAddress);
    if (boolAddressFound == false) {
        // Address was not found in the common DDL, load the other DDLs
        LoadAddressCommon(oCommon, "-1");
        oState.style.display = "inline";
        for (var ii=0; ii<oState.length; ii++) {
            if (oState.options[ii].value == strState) {
                oState.selectedIndex = ii;
                break;
            }
        }
        oCity.style.display = "inline";
        PopulateCitys2(oState, oCity);
        for (var ii=0; ii<oCity.length; ii++) {
            if (oCity.options[ii].value == strCity) {
                oCity.selectedIndex = ii;
                break;
            }
        }
        oAddress.style.display = "inline";
        PopulateAddresss2(oCity, oAddress);
        for (var ii=0; ii<oAddress.length; ii++) {
            if (oAddress.options[ii].value == strAddress) {
                oAddress.selectedIndex = ii;
                break;
            }
        }
        oHidden.value = strAddress;
    }
    else {
        oState.style.display = "none";
        oCity.style.display = "none";
        oAddress.style.display = "none";
    }
}
function LoadAddressCommon(oCommon, strCommon) {
    var boolAddressFound = false;
    for (var ii=0; ii<oCommon.length; ii++) {
        if (oCommon.options[ii].value == strCommon) {
            oCommon.selectedIndex = ii;
            boolAddressFound = true;
            break;
        }
    }
    return boolAddressFound;
}


// **************************************************************************
// TSM (Server, Domain, Schedule)
// **************************************************************************
var oActiveXTSM = null;
var oDDLDomain = null;
var oDDLSchedule = null;
function PopulateTSMDomains(_server, _domain, _schedule) {
    _server = document.getElementById(_server);
    _domain = document.getElementById(_domain);
    oDDLDomain = _domain;
    _schedule = document.getElementById(_schedule);
    oDDLSchedule = _schedule;
    PopulateTSMDomains2(_server, _domain);
}
function PopulateTSMDomains2(_server, _domain) {
    oActiveXTSM = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXTSM.onreadystatechange = PopulateTSMDomains_a;
    oActiveXTSM.open("GET", "/frame/ajax/ajax_tsm_domains.aspx?u=GET", false);
    oActiveXTSM.send("<ajax>" + escape(_server.options[_server.selectedIndex].value) + "</ajax>");
}
function PopulateTSMDomains_a() {
    if (oActiveXTSM.readyState == 4)
    {
        if (oActiveXTSM.status == 200) {
            var or = oActiveXTSM.responseXML.documentElement.childNodes;
            oDDLDomain.length = 0;
            oDDLDomain.disabled = false;
            PopulateTSMDomain(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateTSMDomain(or) {
    var strScript = "";
	var oOption = document.createElement("OPTION");
    if (or.length > 0) {
		oDDLDomain.add(oOption);
		oOption.text = " -- SELECT --";
		oOption.value = "0";
        for (var ii=0; ii<or.length; ii=ii+2) {
		    oOption = document.createElement("OPTION");
		    oDDLDomain.add(oOption);
		    oOption.text = or[ii+1].childNodes[0].text;
		    oOption.value = or[ii].childNodes[0].text;
        }
    }
    else {
		oDDLDomain.add(oOption);
		oOption.text = "-- Select a Server --";
		oOption.value = "0";
		oDDLDomain.disabled = true;
    }
	oOption = document.createElement("OPTION");
    oDDLSchedule.length = 0;
	oDDLSchedule.add(oOption);
	oOption.text = "-- Select a Domain --";
	oOption.value = "0";
	oDDLSchedule.disabled = true;
}
function PopulateTSMSchedules(_domain, _schedule) {
    _domain = document.getElementById(_domain);
    oDDLDomain = _domain;
    _schedule = document.getElementById(_schedule);
    oDDLSchedule = _schedule;
    PopulateTSMSchedules2(_domain, _schedule);
}
function PopulateTSMSchedules2(_domain, _schedule) {
    oActiveXTSM = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXTSM.onreadystatechange = PopulateTSMSchedules_a;
    oActiveXTSM.open("GET", "/frame/ajax/ajax_tsm_schedules.aspx?u=GET", false);
    oActiveXTSM.send("<ajax>" + escape(_domain.options[_domain.selectedIndex].value) + "</ajax>");
}
function PopulateTSMSchedules_a() {
    if (oActiveXTSM.readyState == 4)
    {
        if (oActiveXTSM.status == 200) {
            var or = oActiveXTSM.responseXML.documentElement.childNodes;
            oDDLSchedule.length = 0;
            oDDLSchedule.disabled = false;
            PopulateTSMSchedule(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateTSMSchedule(or) {
    var strScript = "";
    if (or.length > 0) {
		var oOption = document.createElement("OPTION");
		oDDLSchedule.add(oOption);
		oOption.text = " -- SELECT --";
		oOption.value = "0";
        for (var ii=0; ii<or.length; ii=ii+2) {
		    oOption = document.createElement("OPTION");
		    oDDLSchedule.add(oOption);
		    oOption.text = or[ii+1].childNodes[0].text;
		    oOption.value = or[ii].childNodes[0].text;
        }
    }
    else {
		var oOption = document.createElement("OPTION");
		oDDLSchedule.add(oOption);
		oOption.text = "-- Select a Domain --";
		oOption.value = "0";
		oDDLSchedule.disabled = true;
    }
}
// **************************************************************************
// Asset (Platform, Type, Model, ModelProp)
// **************************************************************************
var oActiveXPlatform = null;
var oDDLType = null;
var oDDLModel = null;
var oDDLModelProp = null;
function PopulatePlatformTypes(_platform, _type, _model, _model_prop) {
    _platform = document.getElementById(_platform);
    _type = document.getElementById(_type);
    oDDLType = _type;
    _model = document.getElementById(_model);
    oDDLModel = _model;
    if (_model_prop != null) {
        _model_prop = document.getElementById(_model_prop);
        oDDLModelProp = _model_prop;
    }
    PopulatePlatformTypes2(_platform);
}
function PopulatePlatformTypes2(_platform) {
    oActiveXPlatform = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXPlatform.onreadystatechange = PopulatePlatformTypes_a;
    oActiveXPlatform.open("GET", "/frame/ajax/ajax_platform_types.aspx?u=GET", false);
    oActiveXPlatform.send("<ajax>" + escape(_platform.options[_platform.selectedIndex].value) + "</ajax>");
}
function PopulatePlatformTypes_a() {
    if (oActiveXPlatform.readyState == 4)
    {
        if (oActiveXPlatform.status == 200) {
            var or = oActiveXPlatform.responseXML.documentElement.childNodes;
            oDDLType.length = 0;
            oDDLType.disabled = false;
            PopulatePlatformType(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulatePlatformType(or) {
    var strScript = "";
	var oOption = document.createElement("OPTION");
    if (or.length > 0) {
		oDDLType.add(oOption);
		oOption.text = " -- SELECT --";
		oOption.value = "0";
        for (var ii=0; ii<or.length; ii=ii+2) {
		    oOption = document.createElement("OPTION");
		    oDDLType.add(oOption);
		    oOption.text = or[ii+1].childNodes[0].text;
		    oOption.value = or[ii].childNodes[0].text;
        }
    }
    else {
		oDDLType.add(oOption);
		oOption.text = "-- Select a Platform --";
		oOption.value = "0";
		oDDLType.disabled = true;
    }
	oOption = document.createElement("OPTION");
    oDDLModel.length = 0;
	oDDLModel.add(oOption);
	oOption.text = "-- Select a Type --";
	oOption.value = "0";
	oDDLModel.disabled = true;
    if (oDDLModelProp != null) {
	    oOption = document.createElement("OPTION");
        oDDLModelProp.length = 0;
	    oDDLModelProp.add(oOption);
	    oOption.text = "-- Select a Model --";
	    oOption.value = "0";
	    oDDLModelProp.disabled = true;
	}
}
function PopulatePlatformModels(_type, _model, _model_prop) {
    _type = document.getElementById(_type);
    oDDLType = _type;
    _model = document.getElementById(_model);
    oDDLModel = _model;
    if (_model_prop != null) {
        _model_prop = document.getElementById(_model_prop);
        oDDLModelProp = _model_prop;
    }
    PopulatePlatformModels2(_type);
}
function PopulatePlatformModels2(_type) {
    oActiveXPlatform = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXPlatform.onreadystatechange = PopulatePlatformModels_a;
    oActiveXPlatform.open("GET", "/frame/ajax/ajax_platform_models.aspx?u=GET", false);
    oActiveXPlatform.send("<ajax>" + escape(_type.options[_type.selectedIndex].value) + "</ajax>");
}
function PopulatePlatformModels_a() {
    if (oActiveXPlatform.readyState == 4)
    {
        if (oActiveXPlatform.status == 200) {
            var or = oActiveXPlatform.responseXML.documentElement.childNodes;
            oDDLModel.length = 0;
            oDDLModel.disabled = false;
            PopulatePlatformModel(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulatePlatformModel(or) {
    var strScript = "";
    if (or.length > 0) {
		var oOption = document.createElement("OPTION");
		oDDLModel.add(oOption);
		oOption.text = " -- SELECT --";
		oOption.value = "0";
        for (var ii=0; ii<or.length; ii=ii+2) {
		    oOption = document.createElement("OPTION");
		    oDDLModel.add(oOption);
		    oOption.text = or[ii+1].childNodes[0].text;
		    oOption.value = or[ii].childNodes[0].text;
        }
    }
    else {
		var oOption = document.createElement("OPTION");
		oDDLModel.add(oOption);
		oOption.text = "-- Select a Type --";
		oOption.value = "0";
		oDDLModel.disabled = true;
    }
    if (oDDLModelProp != null) {
	    oOption = document.createElement("OPTION");
        oDDLModelProp.length = 0;
	    oDDLModelProp.add(oOption);
	    oOption.text = "-- Select a Model --";
	    oOption.value = "0";
	    oDDLModelProp.disabled = true;
	}
}
function PopulatePlatformModelProperties(_model, _model_prop) {
    _model = document.getElementById(_model);
    oDDLModel = _model;
    _model_prop = document.getElementById(_model_prop);
    oDDLModelProp = _model_prop;
    PopulatePlatformModelProperties2(_model);
}
function PopulatePlatformModelProperties2(_model) {
    oActiveXPlatform = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXPlatform.onreadystatechange = PopulatePlatformModelProperties_a;
    oActiveXPlatform.open("GET", "/frame/ajax/ajax_platform_model_properties.aspx?u=GET", false);
    oActiveXPlatform.send("<ajax>" + escape(_model.options[_model.selectedIndex].value) + "</ajax>");
}
function PopulatePlatformModelProperties_a() {
    if (oActiveXPlatform.readyState == 4)
    {
        if (oActiveXPlatform.status == 200) {
            var or = oActiveXPlatform.responseXML.documentElement.childNodes;
            oDDLModelProp.length = 0;
            oDDLModelProp.disabled = false;
            PopulatePlatformModelProperty(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulatePlatformModelProperty(or) {
    var strScript = "";
    if (or.length > 0) {
		var oOption = document.createElement("OPTION");
		oDDLModelProp.add(oOption);
		oOption.text = " -- SELECT --";
		oOption.value = "0";
        for (var ii=0; ii<or.length; ii=ii+2) {
		    oOption = document.createElement("OPTION");
		    oDDLModelProp.add(oOption);
		    oOption.text = or[ii+1].childNodes[0].text;
		    oOption.value = or[ii].childNodes[0].text;
        }
    }
    else {
		var oOption = document.createElement("OPTION");
		oDDLModelProp.add(oOption);
		oOption.text = "-- Select a Model --";
		oOption.value = "0";
		oDDLModelProp.disabled = true;
    }
}

function PopulatePlatformModelPropertiesAll(_model, _model_prop) {
    _model = document.getElementById(_model);
    oDDLModel = _model;
    _model_prop = document.getElementById(_model_prop);
    oDDLModelProp = _model_prop;
    PopulatePlatformModelPropertiesAll2(_model);
}
function PopulatePlatformModelPropertiesAll2(_model) {
    oActiveXPlatform = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXPlatform.onreadystatechange = PopulatePlatformModelPropertiesAll_a;
    oActiveXPlatform.open("GET", "/frame/ajax/ajax_platform_model_properties_all.aspx?u=GET", false);
    oActiveXPlatform.send("<ajax>" + escape(_model.options[_model.selectedIndex].value) + "</ajax>");
}
function PopulatePlatformModelPropertiesAll_a() {
    if (oActiveXPlatform.readyState == 4)
    {
        if (oActiveXPlatform.status == 200) {
            var or = oActiveXPlatform.responseXML.documentElement.childNodes;
            oDDLModelProp.length = 0;
            oDDLModelProp.disabled = false;
            PopulatePlatformModelPropertyAll(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulatePlatformModelPropertyAll(or) {
    var strScript = "";
    if (or.length > 0) {
		var oOption = document.createElement("OPTION");
		oDDLModelProp.add(oOption);
		oOption.text = " -- SELECT --";
		oOption.value = "0";
        for (var ii=0; ii<or.length; ii=ii+2) {
		    oOption = document.createElement("OPTION");
		    oDDLModelProp.add(oOption);
		    oOption.text = or[ii+1].childNodes[0].text;
		    oOption.value = or[ii].childNodes[0].text;
        }
    }
    else {
		var oOption = document.createElement("OPTION");
		oDDLModelProp.add(oOption);
		oOption.text = "-- Select a Model --";
		oOption.value = "0";
		oDDLModelProp.disabled = true;
    }
}

var oActiveXIPNetwork = null;
// **************************************************************************
// IPVlan is a drop down list, and IPNetwork is a drop down list
// **************************************************************************
var oDDLIPNetwork = null;
function PopulateIPNetworks(_ipvlanid, _subipvlan) {
    _ipvlanid = document.getElementById(_ipvlanid);
    _subipvlan = document.getElementById(_subipvlan);
    PopulateIPNetworks2(_ipvlanid, _subipvlan);
}
function PopulateIPNetworks2(_ipvlanid, _subipvlan) {
    oDDLIPNetwork = _subipvlan;
    oActiveXIPNetwork = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXIPNetwork.onreadystatechange = PopulateIPNetworks_a;
    oActiveXIPNetwork.open("GET", "/frame/ajax/ajax_ipnetworks.aspx?u=GET", false);
    oActiveXIPNetwork.send("<ajax>" + escape(_ipvlanid.options[_ipvlanid.selectedIndex].value) + "</ajax>");
}
function PopulateIPNetworks_a() {
    if (oActiveXIPNetwork.readyState == 4)
    {
        if (oActiveXIPNetwork.status == 200) {
            var or = oActiveXIPNetwork.responseXML.documentElement.childNodes;
            oDDLIPNetwork.length = 0;
            oDDLIPNetwork.disabled = false;
            PopulateIPNetwork(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateIPNetwork(or) {
    var strScript = "";
	var oOption = document.createElement("OPTION");
	oDDLIPNetwork.add(oOption);
	oOption.text = " -- ANY --";
	oOption.value = "0";
    if (or.length > 0) {
        for (var ii=0; ii<or.length; ii=ii+2) {
		    oOption = document.createElement("OPTION");
		    oDDLIPNetwork.add(oOption);
		    oOption.text = or[ii+1].childNodes[0].text;
		    oOption.value = or[ii].childNodes[0].text;
        }
    }
}
var oDDLIPVLAN = null;
function PopulateIPVLAN(_ipvlanid, _subipvlan) {
    _ipvlanid = document.getElementById(_ipvlanid);
    _subipvlan = document.getElementById(_subipvlan);
    PopulateIPVLAN2(_ipvlanid, _subipvlan);
}
function PopulateIPVLAN2(_ipvlanid, _subipvlan) {
    oDDLIPVLAN = _ipvlanid;
    oActiveXIPNetwork = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXIPNetwork.onreadystatechange = PopulateIPVLAN_a;
    oActiveXIPNetwork.open("GET", "/frame/ajax/ajax_ipvlan.aspx?u=GET", false);
    oActiveXIPNetwork.send("<ajax>" + escape(_subipvlan.options[_subipvlan.selectedIndex].value) + "</ajax>");
}
function PopulateIPVLAN_a() {
    if (oActiveXIPNetwork.readyState == 4)
    {
        if (oActiveXIPNetwork.status == 200) {
            var or = oActiveXIPNetwork.responseXML.documentElement.childNodes;
            PopulateIPVLANs(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateIPVLANs(or) {
    var strScript = "";
    if (or.length > 0) {
        for (var ii=0; ii<oDDLIPVLAN.length; ii++) {
            if (oDDLIPVLAN.options[ii].value == or[0].childNodes[0].text)
                oDDLIPVLAN.selectedIndex = ii;
        }
    }
}
function SaveDeviceComponents(strSave, boolWait) {
    if (boolWait) {
        addDOMLoadEvent(function() {
	        SaveDeviceComponents2(strSave);
        });
    }
    else {
        SaveDeviceComponents2(strSave);
    }
}
function SaveDeviceComponents2(strSave) {
    var oHidden = document.getElementById("hdnComponents");
    if (oHidden == null) {
        alert('ERROR: No hidden variable named "hdnComponents" was found');
        return false;
    }
    else
        oHidden.value = strSave;
    //alert(oHidden.value);
    return true;
}




var oActiveXErrorType2 = null;
// **************************************************************************
// ErrorType1 is a drop down list, and ErrorType2 is a drop down list
// **************************************************************************
var oDDLErrorType2 = null;
function PopulateErrorType2s(_ErrorType1id, _ErrorType2) {
    _ErrorType1id = document.getElementById(_ErrorType1id);
    _ErrorType2 = document.getElementById(_ErrorType2);
    PopulateErrorType2s2(_ErrorType1id, _ErrorType2);
}
function PopulateErrorType2s2(_ErrorType1id, _ErrorType2) {
    oDDLErrorType2 = _ErrorType2;
    oActiveXErrorType2 = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXErrorType2.onreadystatechange = PopulateErrorType2s_a;
    oActiveXErrorType2.open("GET", "/frame/ajax/ajax_error_types.aspx?u=GET", false);
    oActiveXErrorType2.send("<ajax>" + escape(_ErrorType1id.options[_ErrorType1id.selectedIndex].value) + "</ajax>");
}
function PopulateErrorType2s_a() {
    if (oActiveXErrorType2.readyState == 4)
    {
        if (oActiveXErrorType2.status == 200) {
            var or = oActiveXErrorType2.responseXML.documentElement.childNodes;
            oDDLErrorType2.length = 0;
            oDDLErrorType2.disabled = false;
            PopulateErrorType2(or);
        }
        else 
            alert('There was a problem getting the information');
    }
}
function PopulateErrorType2(or) {
    var strScript = "";
    if (or.length > 0) {
		var oOption = document.createElement("OPTION");
		oDDLErrorType2.add(oOption);
		oOption.text = " -- SELECT --";
		oOption.value = "0";
        for (var ii=0; ii<or.length; ii=ii+2) {
		    oOption = document.createElement("OPTION");
		    oDDLErrorType2.add(oOption);
		    oOption.text = or[ii+1].childNodes[0].text;
		    oOption.value = or[ii].childNodes[0].text;
        }
    }
    else {
		var oOption = document.createElement("OPTION");
		oDDLErrorType2.add(oOption);
		oOption.text = " -- Please select a case code --";
		oOption.value = "0";
		oDDLErrorType2.disabled = true;
    }
}


var oHelpWindowHolder = null;
function ShowHelpWindow(intControlID) {
    if (oHelpWindowHolder != null)
        oHelpWindowHolder.close();
    var intWidth = 400;
    var intHeight = 400;
    var intScreenWidth = parseInt(document.body.clientWidth);
    var intScreenHeight = parseInt(document.body.clientHeight);
    var intLeft = (intScreenWidth / 2) - (intWidth / 2);
    var intTop = (intScreenHeight / 2) - (intHeight / 2);
    oHelpWindowHolder = window.open('/ControlHelp.aspx?ctrlid=' + intControlID,'Help','height=' + intHeight + ',width=' + intWidth + ',left=' + intLeft + ',top=' + intTop + ',titlebar=no,toolbar=no,statusbar=no');
}



var strActiveXPassword = "";
var oActiveXPassword = null;
function GetPassword(password) {
    oActiveXPassword = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXPassword.onreadystatechange = GetPassword_a;
    oActiveXPassword.open("GET", "/frame/ajax/ajax_password.aspx?u=" + password + "&id=" + (new Date()).getTime(), false);
    oActiveXPassword.send();
    while (strActiveXPassword == "") {
    }
    return strActiveXPassword;
}
function GetPassword_a() {
    if (oActiveXPassword.readyState == 4)
    {
        if (oActiveXPassword.status == 200) {
            var or = oActiveXPassword.responseXML.documentElement.childNodes;
            strActiveXPassword = or[0].childNodes[0].text;
            //alert(strActiveXPassword);
        }
        else 
            alert('There was a problem getting the information');
    }
}

var strActiveXDecrypt = "";
var oActiveXDecrypt = null;
function Decrypt(strEncrypt) {
    strActiveXDecrypt = new ActiveXObject("Microsoft.XMLHTTP");
    strActiveXDecrypt.onreadystatechange = Decrypt_a;
    strActiveXDecrypt.open("GET", "/frame/ajax/ajax_decrypt.aspx", false);
    strActiveXDecrypt.send("<ajax>" + strEncrypt + "</ajax>");
    while (strActiveXDecrypt == "") {
    }
    return strActiveXDecrypt;
}
function Decrypt_a() {
    if (strActiveXDecrypt.readyState == 4)
    {
        if (strActiveXDecrypt.status == 200) {
            var or = strActiveXDecrypt.responseXML.documentElement;
            strActiveXDecrypt = or.text;
            //alert(strActiveXDecrypt);
        }
        else 
            alert('There was a problem getting the information');
    }
}
