//var strBackColor = "#DFEEE9";
var strBackColor = "#DEE6EF";
var strTableBorder = null;
var strStoreBack = null;
var strBorderColor = "#007253";
function EnablePostBack(strPath, strRequest) {
    if (strPath == "")
        document.forms[0].action = strRequest;
    else
        document.forms[0].action = '/' + strPath;
}
function ShowHideSubMenu(od) {
    var odt = document.getElementById(od + "t");
    var odb = document.getElementById(od + "b");
    if (odt.style.display == "inline") {
        odt.style.display = "none";
        odb.style.display = "none";
    }
    else {
        odt.style.display = "inline";
        odb.style.display = "inline";
    }
}
function TableOver(oTable) {
    strStoreBack = oTable.style.backgroundColor;
    oTable.style.backgroundColor = strBackColor;
    strTableBorder = oTable.style.border;
    oTable.style.border = "solid 1px #444444";
    oTable.style.cursor = "hand";
}
function TableOut(oTable) {
    oTable.style.backgroundColor = strStoreBack;
    oTable.style.border = strTableBorder;
    oTable.style.cursor = "default";
}
function CellRowOver(oRow) {
	var intCount = 0;
	var strBorder = "solid 2px " + strBackColor;
	for(var ii=0; ii<oRow.children.length; ii++) {
        strStoreBack = oRow.children.item(ii).style.backgroundColor;
	    oRow.children.item(ii).style.backgroundColor = strBackColor;
        oRow.children.item(ii).style.cursor = "hand";
	}
}
function CellRowOut(oRow) {
	for(var ii=0; ii<oRow.children.length; ii++) {
	    oRow.children.item(ii).style.backgroundColor = strStoreBack;
        oRow.children.item(ii).style.cursor = "default";
	}
}
function ImageCellRowOver(oRow) {
	var intCount = 0;
	var strBorder = "solid 2px " + strBackColor;
	for(var ii=0; ii<oRow.children.length; ii++) {
        strStoreBack = oRow.children.item(ii).style.backgroundColor;
	    oRow.children.item(ii).style.backgroundColor = strBackColor;
        oRow.children.item(ii).style.cursor = "hand";
	}
}
function ImageCellRowOut(oRow) {
	for(var ii=0; ii<oRow.children.length; ii++) {
	    oRow.children.item(ii).style.backgroundColor = strStoreBack;
        oRow.children.item(ii).style.cursor = "default";
	}
}
function ImageCellRowClick(strPath) {
    OpenNewWindowMenu(strPath, "800", "600");
}
function Expedite(oYes, oNo) {
    var oResponse = confirm('WARNING: By selecting expedite, this initiative will be flagged and senior management will be notified.\n\nAre you sure you want to expedite this request?');
    if (oResponse == false)
        oYes = document.getElementById(oNo);
    oYes.checked = true;
}
function OpenFile(strFile) {
    window.open(strFile);
}
function CloseWindow() {
    window.top.close();
    return false;
}
function ShowStatus(oId,strT) {
    OpenWindow("STATUS", "?id=" + oId + "&type=" + strT);
    return false;
}
function ShowChange(oId) {
    OpenWindow("CHANGE_CONTROL", "?id=" + oId);
    return false;
}
function ShowChanges(oDay) {
	var o = event.srcElement;
	if (o.tagName != "A")
        OpenWindow("CHANGE_CONTROLS", "?d=" + oDay);
    return false;
}
function ShowHelp(oText) {
    OpenWindow("HELP", "?helpid=" + oText);
    return false;
}
var oCalcOpenText = null; 
function OpenCalculator(oText) {
    oCalcOpenText = document.getElementById(oText);       
    OpenWindow("CALCULATOR_OPEN", "?value=" + oCalcOpenText.value);
    return false;
}
function CloseCalculator(strValue)
{
    oCalcOpenText.value = strValue;
    oCalcOpenText = null;
}
var oCalcText = null;
function ShowCalculator(oText) {
    oCalcText = document.getElementById(oText);
    OpenWindow("CALCULATOR", "?value=" + oCalcText.value);
    return false;
}
function UpdateCalculator(strValue) {
	oCalcText.value = strValue;
    oCalcText = null;
    HidePanel();
}
var oCalText1 = null;
var oCalText2 = null;
var oCalTextAdd = null;
function ShowCalendar(oText1, oText2, intDaysAdd) {
    oCalText1 = document.getElementById(oText1);
    if (oText2 != null)
        oCalText2 = document.getElementById(oText2);
    if (intDaysAdd != null)
        oCalTextAdd = intDaysAdd;
    OpenWindow("CALENDAR", "?date=" + oCalText1.value);
    return false;
}
function ShowCalendarMin(oText, strMin) {
    oCalText1 = document.getElementById(oText);
    OpenWindow("CALENDAR", "?date=" + oCalText1.value + "&min=" + strMin);
    return false;
}
function UpdateCalendar(strValue) {
	oCalText1.value = strValue;
    oCalText1 = null;
    if (oCalText2 != null) {
        if (oCalTextAdd != null) {
            var datValue = new Date(strValue);
            datValue.setDate(datValue.getDate() + oCalTextAdd);
            var intMonth = (datValue.getMonth() + 1);
            var intDay = datValue.getDate();
            var intYear = datValue.getFullYear();
            var strDate = intMonth.toString() + "/" + intDay.toString() + "/" + intYear.toString();
            oCalText2.value = strDate;
        }
        else
	        oCalText2.value = strValue;
        oCalText2 = null;
    }
    HidePanel();
}
var oCalOpenText = null;
function OpenCalendar(oText) {
    oCalOpenText = document.getElementById(oText);
    OpenWindow("CALENDAR_OPEN", "?date=" + oCalOpenText.value);
    return false;
}
function CloseCalendar(strValue) {
	oCalOpenText.value = strValue;
    oCalOpenText = null;
}
var oProjectName = null;
var oProjectBD = null;
var oProjectOrg = null;
var oProjectNumber = null;
function ShowProjectInfo(oName, oBD, oOrg, oNumber, oSearch, strType) {
    oProjectName = document.getElementById(oName);
    oProjectBD = document.getElementById(oBD);
    oProjectOrg = document.getElementById(oOrg);
    oProjectNumber = document.getElementById(oNumber);
    oSearch = document.getElementById(oSearch);
    OpenWindow(strType, "?s=" + oSearch.value);
    return false;
}
function UpdateProjectInfo(strName, strBD, strOrg, strNumber) {
    if (oProjectInfo != null) {
        UpdateProjectInfoShort(strName, strNumber);
    }
    else {
	    oProjectName.value = strName;
	    oProjectName.readOnly = true;
        oProjectName = null;
	    oProjectBD.value = strBD;
	    oProjectBD.disabled = true;
        oProjectBD = null;
	    oProjectOrg.value = strOrg;
	    oProjectOrg.disabled = true;
        oProjectOrg = null;
	    oProjectNumber.value = strNumber;
	    oProjectNumber.readOnly = true;
        oProjectNumber = null;
    }
    HidePanel();
}
var oProjectInfo = null;
var oProjectType = null;
function ShowProjectInfoShort(oText, strType) {
    oProjectInfo = document.getElementById(oText);
    oProjectType = strType;
    OpenWindow(strType, "?s=" + oProjectInfo.value);
    return false;
}
function UpdateProjectInfoShort(strName, strNumber) {
    if (oProjectType == "PNAME_SEARCH" || oProjectType == "PNAME_SEARCH_NOCV")
        oProjectInfo.value = strName;
    if (oProjectType == "PNUMBER_SEARCH" || oProjectType == "PNUMBER_SEARCH_NOCV")
        oProjectInfo.value = strNumber;
    oProjectInfo = null;
    oProjectType = null;
    HidePanel();
}
var oTextInfo = null;
function ShowTextInfo(oText, strType) {
    oTextInfo = document.getElementById(oText);
    OpenWindow(strType, "?s=" + oTextInfo.value);
    return false;
}
function UpdateTextInfo(strValue) {
	oTextInfo.value = strValue;
    oTextInfo = null;
    HidePanel();
}
function findPosX(obj)
{
    var curleft = 0;
    if (obj.offsetParent)
    {
	    while (obj.offsetParent)
	    {
		    curleft += obj.offsetLeft
		    obj = obj.offsetParent;
	    }
    }
    else if (obj.x)
	    curleft += obj.x;
    return curleft;
}
function findPosY(obj)
{
    var curtop = 0;
    if (obj.offsetParent)
    {
	    while (obj.offsetParent)
	    {
		    curtop += obj.offsetTop
		    obj = obj.offsetParent;
	    }
    }
    else if (obj.y)
	    curtop += obj.y;
    return curtop;
}
var boolValidate = true;
function ConfirmCheckBox(oObject, strConfirm) {
    if (oObject.checked == true) {
        var oReturn = confirm(strConfirm);
        oObject.checked = oReturn;
    }
}
function ValidateCheck(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && SetFocusTry(oObject) && oObject.checked == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidatePassword(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    var str = oObject.value;
    if (oObject != null && str.length < 6 && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    var ValidChars = "0123456789"; 
    var found = false;
    var Char;
    for (ii = 0; ii < str.length; ii++) {
        Char = str.charAt(ii);
        if (ValidChars.indexOf(Char) > -1) {
            found = true;
            break;
        }
    }
    if (oObject != null && found == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidatePasswords(oObject1, oObject2, strAlert) {
    oObject1 = document.getElementById(oObject1);
    oObject2 = document.getElementById(oObject2);
    if (trim(oObject1.value) != trim(oObject2.value) && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject1);
        return false;
    }
    return true;
}
function ValidateNoComma(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    var oText = trim(oObject.value);
    var boolComma = false;
    var Char;
    for (ii = 0; ii < oText.length; ii++) {
        Char = oText.charAt(ii);
        if (Char == ',')
            boolComma = true;
    }
    if (oObject != null && boolComma == true && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateNoDuplicate(oText, oList, strAlert) {
    oText = document.getElementById(oText);
    oList = document.getElementById(oList);
    var isDup = false;
    for (var ii=0; ii<oList.length; ii++) {
	    if (oList.options[ii].value == trim(oText.value))
	    {
	        isDup = true;
	        break;
	    }
    }
    if (isDup == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oText);
        return false;
    }
    return true;
}
function ValidateHyperlink(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && SetFocusTry(oObject) && (trim(oObject.value) == "" || (oObject.value.indexOf("http://") != 0 && oObject.value.indexOf("https://") != 0)) && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateTextLength(oObject, strAlert, intLength, arrStartsWith, arrExclude) {

    oObject = document.getElementById(oObject);
    if (oObject != null && boolValidate == true) {
        if (trim(oObject.value) == "" || (intLength != null && oObject.value.length != intLength) || ValidateStartsWith(oObject, arrStartsWith) == false || ValidateExcludes(oObject, arrExclude) == true)
        {
            if (strAlert != "")
                alert(strAlert);
            SetFocus(oObject);
            return false;
        }
    }
    return true;
}
function ValidateStartsWith(oObject, arrStartsWith) {
    var StartsWith = false;
    if (arrStartsWith == null)
        StartsWith = true;
    else {
        //arrStartsWith = arrStartsWith.split("|"); 
        for (ii = 0; ii < arrStartsWith.length; ii++) {
            if (oObject.value.toUpperCase().indexOf(arrStartsWith[ii].toUpperCase()) != 0)
            {
                StartsWith = true;
                break;
            }
        }
    }
    return StartsWith;
}
function ValidateExcludes(oObject, arrExclude) {
    var Exlude = false;
    if (arrExclude != null) {
        //arrExclude = arrExclude.split("|"); 
        for (ii = 0; ii < arrExclude.length; ii++) {
            if (oObject.value.toUpperCase() == arrExclude[ii].toUpperCase())
            {
                Exlude = true;
                break;
            }
        }
    }
    return Exlude;
}
function ValidateText(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && SetFocusTry(oObject) && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateTextWarning(oObject, strWarning) {
    oObject = document.getElementById(oObject);
    if (oObject != null && trim(oObject.value) == "" && boolValidate == true) {
        return confirm(strWarning);
    }
    return true;
}
function ValidateTextDisabled(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    var boolHidden = false;
    try {
        oObject.focus();
    }
    catch (ex) {
        boolHidden = true;
    }
    if (oObject != null && boolHidden == false && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateRepeater(oItems, strAlert) {
    if (oItems < 1 && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        return false;
    }
    return true;
}
function ValidateTextNOCV(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    var strName = trim(oObject.value).toUpperCase();
    if (oObject != null && strName.substring(0, 2) == "CV" && boolValidate == true) {
        alert('This project number does not qualify for a new project request');
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateNumber(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && SetFocusTry(oObject) && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && SetFocusTry(oObject) && isNumber(oObject.value) == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateSlider(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && oObject.value != "100" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        return false;
    }
    return true;
}
function ValidateNumber0(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && oObject.disabled == false && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && oObject.disabled == false && isNumber(oObject.value) == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && oObject.disabled == false && parseFloat(oObject.value) <= 0.00 && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateNumber0Warning(oObject, strAlert, strWarning) {
    oObject = document.getElementById(oObject);
    if (oObject != null && oObject.disabled == false && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && oObject.disabled == false && isNumber(oObject.value) == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && oObject.disabled == false && parseFloat(oObject.value) < 0.00 && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && oObject.disabled == false && parseFloat(oObject.value) == 0.00 && boolValidate == true) {
        if (trim(strWarning) == "") {
            if (strAlert != "")
                alert(strAlert);
            SetFocus(oObject);
            return false;
        }
        else
            return confirm(strWarning);
    }
    return true;
}
function ValidateNumber1(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && oObject.disabled == false && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && oObject.disabled == false && isNumber(oObject.value) == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && oObject.disabled == false && parseInt(oObject.value) <= 1 && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateNumberLength(oObject, iLength, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && oObject.disabled == false && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && oObject.disabled == false && isNumber(oObject.value) == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && oObject.disabled == false && parseInt(oObject.value) <= 0 && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    var str = oObject.value;
    if (oObject != null && oObject.disabled == false && str.length != iLength && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateNumberLess(oObject, oMax, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && oObject.disabled == false && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && oObject.disabled == false && isNumber(oObject.value) == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && oObject.disabled == false && (parseInt(oObject.value) < 0 || parseInt(oObject.value) > oMax) && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateNumberBetween(oObject, oMin, oMax, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && oObject.disabled == false && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && oObject.disabled == false && isNumber(oObject.value) == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && oObject.disabled == false && (parseInt(oObject.value) < oMin || parseInt(oObject.value) > oMax) && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateIntBetween(oObject, oMin, oMax, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && oObject.disabled == false && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && oObject.disabled == false && isInt(oObject.value) == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && oObject.disabled == false && (parseInt(oObject.value) < oMin || parseInt(oObject.value) > oMax) && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateNumberGreater(oObjectLess, oObjectMax, strAlert) {
    oObjectLess = document.getElementById(oObjectLess);
    oObjectMax = document.getElementById(oObjectMax);
    if (oObjectLess != null && oObjectMax != null && trim(oObjectLess.value) == "" && trim(oObjectMax.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObjectLess);
        return false;
    }
    if (oObjectLess != null && oObjectMax != null && isNumber(oObjectLess.value) == false && isNumber(oObjectMax.value) == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObjectLess);
        return false;
    }
    if (oObjectLess != null && oObjectMax != null && (parseInt(oObjectLess.value) > parseInt(oObjectMax.value)) && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObjectLess);
        return false;
    }
    return true;
}
function ValidateNumberGreaterNumbers(oObjectLess, oObjectMax, strAlert) {
    if (oObjectLess != null && oObjectMax != null && oObjectLess > oObjectMax && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        return false;
    }
    return true;
}
function ValidateEqual(oObject1, oObject2, strAlert) {
    if (oObject1 != oObject2 && boolValidate == true) {
        return confirm(strAlert);
    }
    return true;
}
function ValidateBoolean(oObject, strAlert) {
    if (oObject == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        return false;
    }
    return true;
}
function ValidateHidden(oObject, oText, strAlert) {
    oObject = document.getElementById(oObject);
    oText = document.getElementById(oText);
    if (oObject != null && SetFocusTry(oText) && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oText);
        return false;
    }
    return true;
}
function ValidateHiddenDisabled(oObject, oText, strAlert) {
    oObject = document.getElementById(oObject);
    oText = document.getElementById(oText);
    var boolHidden = false;
    try {
        oText.focus();
    }
    catch (ex) {
        boolHidden = true;
    }
    if (oObject != null && oText != null && boolHidden == false && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oText);
        return false;
    }
    return true;
}
function ValidateHiddenNumber(oObject, oText, strAlert) {
    oObject = document.getElementById(oObject);
    oText = document.getElementById(oText);
    if (oObject != null && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oText);
        return false;
    }
    if (oObject != null && isNumber(oObject.value) == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oText);
        return false;
    }
    return true;
}
function ValidateHidden0(oObject, oText, strAlert) {
    oObject = document.getElementById(oObject);
    oText = document.getElementById(oText);
    if (oObject != null && SetFocusTry(oText) && (trim(oObject.value) == "" || trim(oObject.value) == "0" || isNumber(oObject.value) == false) && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oText);
        return false;
    }
    return true;
}
function ValidateDropDown(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && SetFocusTry(oObject) && oObject.selectedIndex == 0 && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateDropDownDisabled(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && oObject.disabled == false && oObject.selectedIndex == 0 && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateList(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && oObject.length == 0 && boolValidate == true) {
        alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateListBox(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && SetFocusTry(oObject) && oObject.selectedIndex == -1 && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
var boolCheckList = true;
function ValidateCheckLists(oTable, strAlert) {
    boolCheckList = true;
    oTable = document.getElementById(oTable);
    if (oTable != null && SetFocusTry(oTable) && boolValidate == true) {
        ValidateCheckList(oTable);
        if (boolCheckList == false) {
            if (strAlert != "")
                alert(strAlert);
            return false;
        }
    }
    return true;
}
function ValidateCheckListsNoFocus(oTable, strAlert) {
    boolCheckList = true;
    oTable = document.getElementById(oTable);
    if (oTable != null && boolValidate == true) {
        ValidateCheckList(oTable);
        if (boolCheckList == false) {
            if (strAlert != "")
                alert(strAlert);
            return false;
        }
    }
    return true;
}
function ValidateCheckList(oParent) {
    for(var ii=0; ii<oParent.children.length && boolCheckList == true; ii++) {
        var oObject = oParent.children.item(ii);
        if (oObject.tagName == "INPUT" && oObject.getAttribute("type").toUpperCase() == "CHECKBOX" && oObject.checked == false) {
            boolCheckList = false;
        }
        ValidateCheckList(oObject);
    }
}
var boolRadioList = false;
function ValidateRadioList(oTable, strAlert) {
    boolRadioList = false;
    oTable = document.getElementById(oTable);
    if (oTable != null && SetFocusTry(oTable) && boolValidate == true) {
        ValidateRadio(oTable);
        if (boolRadioList == false) {
            if (strAlert != "")
                alert(strAlert);
            return false;
        }
    }
    return true;
}
function ValidateRadio(oParent) {
    for(var ii=0; ii<oParent.children.length && boolRadioList == false; ii++) {
        var oObject = oParent.children.item(ii);
        if (oObject.tagName == "INPUT" && oObject.checked == true) {
            boolRadioList = true;
        }
        ValidateRadio(oObject);
    }
}
function ValidateRadioButtons(oObject1, oObject2, strAlert) {
    oObject1 = document.getElementById(oObject1);
    if (oObject1.checked == true)
        return true;
    else {
        oObject2 = document.getElementById(oObject2);
        if (oObject2.checked == true)
            return true;
        else {
            if (strAlert != "")
                alert(strAlert);
            SetFocus(oObject1);
            return false;
        }
    }
    return true;
}
function ValidateRadioButtons3(oObject1, oObject2, oObject3, strAlert) {
    oObject1 = document.getElementById(oObject1);
    if (oObject1.checked == true)
        return true;
    else {
        oObject2 = document.getElementById(oObject2);
        if (oObject2.checked == true)
            return true;
        else {
            oObject3 = document.getElementById(oObject3);
            if (oObject3.checked == true)
                return true;
            else {
                if (strAlert != "")
                    alert(strAlert);
                SetFocus(oObject1);
                return false;
            }
        }
    }
    return true;
}
function ValidateDate(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && SetFocusTry(oObject) && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && SetFocusTry(oObject) && isDate(oObject.value) == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateDateToday(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && isDate(oObject.value) == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null) {
        var datePat = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
        var oDate = Date.parse(oObject.value);
        var oDateArray = oObject.value.match(datePat);
        var oDateMonth = oDateArray[1];
        var oDateDay = oDateArray[3];
        var oDateYear = oDateArray[5];
        var oToday = new Date();
        var oTodayMonth = (oToday.getMonth() + 1);
        var oTodayDay = oToday.getDate();
        var oTodayYear = oToday.getFullYear();
        if ((oDate < oToday && (oTodayMonth != oDateMonth || oTodayDay != oDateDay || oTodayYear != oDateYear)) && boolValidate == true) {
            if (strAlert != "")
                alert(strAlert);
            SetFocus(oObject);
            return false;
        }
    }
    return true;
}
function ValidateTime(oObject, strAlert) {
    oObject = document.getElementById(oObject);
    if (oObject != null && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && isTime(oObject.value) == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}
function ValidateDates(oStart, oEnd, strAlert) {
    oStart = document.getElementById(oStart);
    oEnd = document.getElementById(oEnd);
    if (oStart == null || trim(oStart.value) == "" || boolValidate == false)
        return true;
    if (oStart == null || isDate(oStart.value) == false || boolValidate == false)
        return true;
    if (oEnd == null || trim(oEnd.value) == "" || boolValidate == false)
        return true;
    if (oEnd == null || isDate(oEnd.value) == false || boolValidate == false)
        return true;
    if (new Date(oStart.value) > new Date(oEnd.value)) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oStart);
        return false;
    }
    return true;
}
function ValidateDateHidden(oText, oHiiden, strAlert) {
    oText = document.getElementById(oText);
    oHiiden = document.getElementById(oHiiden);
    if (oText != null && SetFocusTry(oText) && new Date(oText.value) < new Date(oHiiden.value) && boolValidate == true) {
        alert(strAlert + 'Enter a date after ' + oHiiden.value + '.');
        SetFocus(oText);
        return false;
    }
    return true;
}
function ValidateDateHiddenBefore(oText, oHiiden, strAlert) {
    oText = document.getElementById(oText);
    oHiiden = document.getElementById(oHiiden);
    if (oText != null && SetFocusTry(oText) && new Date(oText.value) > new Date(oHiiden.value) && boolValidate == true) {
        alert(strAlert + 'Enter a date before ' + oHiiden.value + '.');
        SetFocus(oText);
        return false;
    }
    return true;
}
function ValidateRemediation(strApproved, oFunding, oTPM, oHours, oShow, oHide) {
    if (strApproved == "1")
        return true;
    oFunding = document.getElementById(oFunding);
    oTPM = document.getElementById(oTPM);
    oHours = document.getElementById(oHours);
    oShow = document.getElementById(oShow);
    oHide = document.getElementById(oHide);
    var oRetVal = true;
    if (oFunding != null && oFunding.selectedIndex == 4 && boolValidate == true)
        oRetVal = false;
    if (oTPM != null && oTPM.checked == true && boolValidate == true)
        oRetVal = false;
    var dHours = parseFloat(oHours.value);
    if (oHours != null && dHours > 40 && boolValidate == true)
        oRetVal = false;
    if (oRetVal == false) {
        oShow.style.display = "none";
        oHide.style.display = "inline";
        return false;
    }
    else
        return true;
}
function trim(str) { 
     str = str.replace( /^\s+/g, "" );
     str = str.replace( /\s+$/g, "" );
     return str; 
} 
function isInt(str) {
   var ValidChars = "0123456789"; 
   var Char;
   for (ii = 0; ii < str.length; ii++) {
       Char = str.charAt(ii);
       if (ValidChars.indexOf(Char) == -1)
          return false;
    }
    if (str.length == 0)
        return false;
   return true;
}
function isNumber(str) {
   var ValidChars = "0123456789."; 
   var Char;
   for (ii = 0; ii < str.length; ii++) {
       Char = str.charAt(ii);
       if (ValidChars.indexOf(Char) == -1)
          return false;
    }
    if (str.length == 0)
        return false;
   return true;
}
function isAlphaNumeric(str) {
   var ValidChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"; 
   var Char;
   for (ii = 0; ii < str.length; ii++) {
       Char = str.charAt(ii);
       if (ValidChars.indexOf(Char) == -1)
          return false;
    }
    if (str.length == 0)
        return false;
   return true;
}
function isDate(dateStr) {
    var datePat = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var matchArray = dateStr.match(datePat);
    if (matchArray == null) {
        alert("Please enter date in one of the following formats:\nmm/dd/yy, mm/dd/yyyy, mm-dd-yyyy, or mm-dd-yyyy.");
        return false;
    }
    month = matchArray[1];
    day = matchArray[3];
    year = matchArray[5];
    if (month < 1 || month > 12) {
        alert("Month must be between 1 and 12.");
        return false;
    }
    if (day < 1 || day > 31) {
        alert("Day must be between 1 and 31.");
        return false;
    }
    if ((month==4 || month==6 || month==9 || month==11) && day==31) {
        alert("Month "+month+" doesn't have 31 days!")
        return false;
    }
    if (month == 2) {
        var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
        if (day > 29 || (day==29 && !isleap)) {
            alert("February " + year + " doesn't have " + day + " days!");
            return false;
        }
    }
    return true;
}
function isTime(timeStr) {
    var timePat = /^(\d{1,2}):(\d{2})(:(\d{2}))?(\s?(AM|am|PM|pm))?$/;
    var matchArray = timeStr.match(timePat);
    if (matchArray == null) {
        alert("Time is not in a valid format");
        return false;
    }
    hour = matchArray[1];
    minute = matchArray[2];
    second = matchArray[4];
    ampm = matchArray[6];
    if (second == "")
         second = null;
    if (ampm == "")
        ampm = null;
    if (hour < 0  || hour > 23) {
        alert("Hour must be between 1 and 12. (or 0 and 23 for military time)");
        return false;
    }
    if (hour <= 12 && ampm == null) {
        if (confirm("Please indicate which time format you are using.  OK = Standard Time, CANCEL = Military Time")) {
            alert("You must specify AM or PM.");
            return false;
        }
    }
    if  (hour > 12 && ampm != null) {
        alert("You can't specify AM or PM for military time.");
        return false;
    }
    if (minute < 0 || minute > 59) {
        alert ("Minute must be between 0 and 59.");
        return false;
    }
    if (second != null && (second < 0 || second > 59)) {
        alert ("Second must be between 0 and 59.");
        return false;
    }
    return true;
}
function dateDiff(strDate1, strDate2) {
    date1 = new Date();
    date2 = new Date();
    diff  = new Date();

    date1temp = new Date(strDate1);
    date1.setTime(date1temp.getTime());

    date2temp = new Date(strDate2);
    date2.setTime(date2temp.getTime());

    diff.setTime(Math.abs(date1.getTime() - date2.getTime()));
    timediff = diff.getTime();

    return timediff;
}


//var oxml = null;
//var oADText = null;
//var oADLabel = null;
//var oADHidden = null;
//var oADType = null;
//function ADSearch(strUrl, oText, oLabel, oHidden, oType) {
//    oADText = document.getElementById(oText);
//    if (oADText.value.length > 1) {
//        oADLabel = document.getElementById(oLabel);
//        oADHidden = document.getElementById(oHidden);
//        oADType = oType;
//        strUrl += "?" + oADType + "=" + oADText.value;
//        oxml = new ActiveXObject("Microsoft.XMLHTTP");
//        oxml.onreadystatechange = ADSearch_ajax;
//        oxml.open("GET", strUrl, false);
//        oxml.send(null);
//    }
//    else {
//        alert('Please enter at least two characters');
//        oADText.focus();
//        oADText = null;
//    }
//    return false;
//}
//function ADSearch_ajax() {
//    if (oxml.readyState == 4)
//    {
//        if (oxml.status == 200) {
//            var strName = oxml.responseText;
//            if (strName == "NONE") {
//                alert('There was no match for your criteria');
//                oADText.focus();
//            }
//            else if (strName == "MULTIPLE") {
//                if (oADType == "u")
//                    OpenWindow("MULTIPLE", "?u=" + oADText.value);
//                if (oADType == "c")
//                    OpenWindow("MULTIPLE_SERVER", "?c=" + oADText.value);
//            }
//            else {
//                var strId = strName.substring(0, strName.indexOf("_"));
//                strName = strName.substring(strName.indexOf("_") + 1);
//                var strXId = strName.substring(0, strName.indexOf("_"));
//                strName = strName.substring(strName.indexOf("_") + 1);
//                oADLabel.innerText = "[" + strName + "]";
//                oADText.innerText = strXId;
//                oADHidden.innerText = strId;
//                oADLabel = null;
//                oADText = null;
//                oADHidden = null;
//            }
//        }
//        else 
//            alert(oxml.statusText);
//    }
//}
//function UpdateAD(strId, strXid, strName) {
//    oADLabel.innerText = "[" + strName + "]";
//    oADText.innerText = strXid;
//    oADHidden.innerText = strId;
//    oADLabel = null;
//    oADText = null;
//    oADHidden = null;
//    oADType = null;
//    HidePanel();
//}
function WaitWindow() {
    setTimeout("WaitWindow2()",50);
    return true;
}
function WaitWindow2() {
    try {
        ShowPanel('/frame/please_wait.htm',200,200,true);
    }
    catch (ex) {}
}
 
function OpenWindow(strType, strVar) {
    if (strType == "CALENDAR")
        ShowPanel('/frame/loading.htm?referrer=/frame/calendar.aspx' + strVar,275,314);
    if (strType == "CALENDAR_OPEN")
        window.open('/frame/loading.htm?referrer=/frame/calendar.aspx' + strVar,"_blank","height=280,width=266,menubar=no,resizable=no,scrollbars=no,status=no,toolbar=no");
    if (strType == "CALCULATOR")
        ShowPanel('/frame/loading.htm?referrer=/frame/calculator.aspx' + strVar,258,284);
    if (strType == "CALCULATOR_OPEN")
        window.open('/frame/loading.htm?referrer=/frame/calculator.aspx' + strVar,"_blank","height=280,width=266,menubar=no,resizable=no,scrollbars=no,status=no,toolbar=no");
    if (strType == "MULTIPLE")
        ShowPanel('/frame/loading.htm?referrer=/frame/ad_multiple.aspx' + strVar,400,400);
    if (strType == "MULTIPLE_SERVER")
        ShowPanel('/frame/loading.htm?referrer=/frame/server_multiple.aspx' + strVar,400,400);
    if (strType == "HELP")
        ShowPanel('/frame/loading.htm?referrer=/frame/help.aspx' + strVar,400,400);
    if (strType == "DOCUMENTS")
        ShowPanel('/frame/loading.htm?referrer=/frame/documents.aspx' + strVar,600,550);
    if (strType == "DOCUMENTS_SECURE")
        ShowPanel('/frame/loading.htm?referrer=/frame/documents_secure.aspx' + strVar,600,550);
    if (strType == "STATUS")
        ShowPanel('/frame/loading.htm?referrer=/frame/weekly_status.aspx' + strVar,425,425);
    if (strType == "CHANGE_CONTROL")
        ShowPanel('/frame/loading.htm?referrer=/frame/change_control.aspx' + strVar,500,500);
    if (strType == "CHANGE_CONTROLS")
        ShowPanel('/frame/loading.htm?referrer=/frame/change_controls.aspx' + strVar,500,500);
    if (strType == "VACATION")
        ShowPanel('/frame/loading.htm?referrer=/frame/vacation.aspx' + strVar,400,400);
    if (strType == "DESIGNER")
        ShowPanel('/frame/loading.htm?referrer=/frame/designer.aspx' + strVar,425,400);
    if (strType == "FORMCONTROLS")
        ShowPanel('/frame/loading.htm?referrer=/frame/form_controls.aspx' + strVar,400,475);
    if (strType == "PNUMBER_SEARCH")
        ShowPanel('/frame/loading.htm?referrer=/frame/search_pnumber.aspx' + strVar,550,550);
    if (strType == "PNUMBER_SEARCH_NOCV")
        ShowPanel('/frame/loading.htm?referrer=/frame/search_pnumber.aspx' + strVar + '&NOCV=true',550,550);
    if (strType == "PNUMBER_SEARCH_APP")
        ShowPanel('/frame/loading.htm?referrer=/frame/search_pnumber.aspx' + strVar + '&APP=true',550,550);
    if (strType == "PNAME_SEARCH")
        ShowPanel('/frame/loading.htm?referrer=/frame/search_pname.aspx' + strVar,550,550);
    if (strType == "PNAME_SEARCH_NOCV")
        ShowPanel('/frame/loading.htm?referrer=/frame/search_pname.aspx' + strVar + '&NOCV=true',550,550);
    if (strType == "PNAME_SEARCH_APP")
        ShowPanel('/frame/loading.htm?referrer=/frame/search_pname.aspx' + strVar + '&APP=true',550,550);
    if (strType == "ARCHIVED_SEARCH")
        ShowPanel('/frame/loading.htm?referrer=/frame/search_archived.aspx' + strVar,300,300);
    if (strType == "TEMPHELP")
        ShowPanel(strVar,400,400);
    if (strType == "CLARITY_NUMBER")
        window.open('/frame/loading.htm?referrer=/frame/clarity_number.aspx' + strVar,"CLARITY","height=600,width=800,menubar=no,resizable=yes,scrollbars=no,status=no,toolbar=no");
    if (strType == "SERVICE_STATUS")
        ShowPanel('/frame/loading.htm?referrer=' + strVar,650,500);
    if (strType == "REPORT_ABOUT")
        ShowPanel('/frame/loading.htm?referrer=/frame/report_about.aspx' + strVar,400,400);
    if (strType == "RESOURCE_REQUEST_SLA")
        ShowPanel('/frame/loading.htm?referrer=/frame/resource_request_sla.aspx' + strVar,400,300);
    if (strType == "RESOURCE_REQUEST_RETURN")
        ShowPanel('/frame/loading.htm?referrer=/frame/resource_request_return.aspx' + strVar,550,300);
    if (strType == "ASSET_STAGING_CONFIG")
        ShowPanel('/frame/loading.htm?referrer=/frame/asset_staging_configuration.aspx' + strVar,800,600);
    if (strType == "ASSET_WM_TASKS")
        ShowPanel('/frame/loading.htm?referrer=/frame/asset_wm_service_tasks.aspx' + strVar,800,600);
//    if (strType == "RESOURCE_REQUEST_RETURN") 
//    {
//        var oWindow = window.open('/frame/loading.htm?referrer=/frame/resource_request_return.aspx' + strVar,"RESOURCE_REQUEST_RETURN","height=400,width=300,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
//        oWindow.focus();
//    }  
    if (strType == "RESOURCE_REQUEST_EMAIL")
        ShowPanel('/frame/loading.htm?referrer=/frame/resource_request_email.aspx' + strVar,500,250);
    if (strType == "PROFILE")
        ShowPanel('/frame/loading.htm?referrer=/frame/profile.aspx' + strVar,400,400);
    if (strType == "IMAGE")
        ShowPanel('/frame/loading.htm?referrer=/frame/image.aspx' + strVar,400,400);
    if (strType == "SERVICE_TASK")
        ShowPanel('/frame/loading.htm?referrer=/frame/service_task.aspx' + strVar,500,250);
    if (strType == "ASSETIMPORT")
        ShowPanel('/frame/loading.htm?referrer=/frame/asset_import.aspx?referrer=' + strVar,500,400);
    if (strType == "PCR_CSRC")
        window.open('/frame/loading.htm?referrer=/frame/resource_request_pcr_csrc.aspx' + strVar,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "RESOURCE_REQUEST")
        window.open('/frame/loading.htm?referrer=/frame/resource_request.aspx?rrid=' + strVar,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
//    if (strType == "RESOURCE_REQUEST_MANAGER")
//        window.open('/frame/loading.htm?referrer=/frame/resource_request_manager.aspx?rrid=' + strVar,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "NEW_WINDOW")
        window.open('/frame/loading.htm?referrer=' + strVar,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "PRINTER_FRIENDLY")
        window.open('/frame/loading.htm?referrer=/frame/printer_friendly.aspx' + strVar,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "FORECAST_EQUIPMENT")
        window.open('/frame/loading.htm?referrer=/frame/forecast_equipment.aspx' + strVar,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "DESIGN_EQUIPMENT")
        window.open('/frame/loading.htm?referrer=/frame/design.aspx' + strVar,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "DESIGN_EQUIPMENT2")
        window.open('/frame/loading.htm?referrer=/frame/design2.aspx' + strVar,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "SERVICES_DETAIL")
        window.open('/frame/loading.htm?referrer=/frame/services_detail.aspx' + strVar,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "SERVICES_DETAIL_PDF")
        window.open('/frame/loading.htm?referrer=/frame/services_detail_pdf.aspx' + strVar,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "WAN_MODULE")
        ShowPanel('/frame/loading.htm?referrer=/frame/modules.aspx' + strVar,400,300);
    if (strType == "LOCATIONS")
        ShowPanel('/frame/loading.htm?referrer=/frame/locations.aspx' + strVar,500,500);
    if (strType == "FILE")
        window.open('/frame/loading.htm?referrer=/frame/file.aspx' + strVar,"_blank","height=600,width=800,menubar=no,resizable=no,scrollbars=no,status=no,toolbar=no");
    if (strType == "FILE_PERMISSIONS")
        ShowPanel('/frame/loading.htm?referrer=/frame/file_permissions.aspx' + strVar,500,500);
    if (strType == "MILESTONE")
        ShowPanel('/frame/loading.htm?referrer=/frame/milestone.aspx?id=' + strVar,450,375);
    if (strType == "FORECAST_FILTER")
        ShowPanel('/frame/loading.htm?referrer=/frame/forecast/forecast_filter.aspx' + strVar,500,150);
    if (strType == "FORECAST_EXECUTE")
        window.open('/frame/loading.htm?referrer=' + strVar,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "DESIGN_EXECUTE")
        window.open('/frame/loading.htm?referrer=' + strVar,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "ONDEMAND_CLUSTER") {
        var oWindow = window.open('/frame/loading.htm?referrer=/frame/ondemand/cluster.aspx' + strVar,"ONDEMAND_CLUSTER","height=600,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
        oWindow.focus();
    }
    if (strType == "ONDEMAND_CLUSTER_QUORUM") {
        var oWindow = window.open('/frame/loading.htm?referrer=/frame/ondemand/cluster_quorum.aspx' + strVar,"ONDEMAND_CLUSTER_QUORUM","height=225,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
        oWindow.focus();
    }
    if (strType == "ONDEMAND_CLUSTER_INSTANCE") {
        var oWindow = window.open('/frame/loading.htm?referrer=/frame/ondemand/cluster_instance.aspx' + strVar,"ONDEMAND_CLUSTER_INSTANCE","height=600,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
        oWindow.focus();
    }
    if (strType == "ONDEMAND_CLUSTER_INSTANCE_NEW")
        ShowPanel('/frame/loading.htm?referrer=/frame/ondemand/cluster_instance_new.aspx' + strVar,500,500);
    if (strType == "ONDEMAND_CLUSTER_INSTANCE_CONFIG") {
        var oWindow = window.open('/frame/loading.htm?referrer=/frame/ondemand/cluster_instance_config.aspx' + strVar,"ONDEMAND_CLUSTER_INSTANCE_CONFIG","height=425,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
        oWindow.focus();
    }
    if (strType == "ONDEMAND_CSM_CONFIG") {
        var oWindow = window.open('/frame/loading.htm?referrer=/frame/ondemand/config_csm.aspx' + strVar,"ONDEMAND_CSM_CONFIG","height=250,width=500,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
        oWindow.focus();
    }
    if (strType == "ONDEMAND_SERVER") {
        var oWindow = window.open('/frame/loading.htm?referrer=/frame/ondemand/config_server.aspx' + strVar,"ONDEMAND_SERVER","height=600,width=750,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
        oWindow.focus();
    }
    if (strType == "ONDEMAND_STORAGE") {
        var oWindow = window.open('/frame/loading.htm?referrer=/frame/ondemand/config_storage.aspx' + strVar,"ONDEMAND_STORAGE","height=500,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
        oWindow.focus();
    }
    if (strType == "ONDEMAND_SERVER_FDRIVE") {
        var oWindow = window.open('/frame/loading.htm?referrer=/frame/ondemand/config_server_f_drive.aspx' + strVar,"ONDEMAND_SERVER_FDRIVE","height=300,width=700,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
        oWindow.focus();
    }
    if (strType == "ONDEMAND_WORKSTATION") {
        var oWindow = window.open('/frame/loading.htm?referrer=/frame/ondemand/config_workstation.aspx' + strVar,"ONDEMAND_WORKSTATION","height=300,width=600,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
        oWindow.focus();
    }
    if (strType == "ONDEMAND_VIRTUALWORKSTATION") {
        var oWindow = window.open('/frame/loading.htm?referrer=/frame/ondemand/config_workstation_virtual.aspx' + strVar,"ONDEMAND_WORKSTATION","height=300,width=600,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
        oWindow.focus();
    }
    if (strType == "ONDEMAND_VMWAREWORKSTATION") {
        var oWindow = window.open('/frame/loading.htm?referrer=/frame/ondemand/config_workstation_vmware.aspx' + strVar,"ONDEMAND_WORKSTATION","height=300,width=600,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
        oWindow.focus();
    }
       
    if (strType == "RESET_STORAGE")
        ShowPanel('/frame/loading.htm?referrer=/frame/forecast/forecast_reset_storage.aspx' + strVar,300,150);
    if (strType == "MODEL_THRESHOLDS")
        ShowPanel('/frame/loading.htm?referrer=/frame/inventory/model_thresholds.aspx' + strVar,450,450);
    if (strType == "DELEGATE")
        ShowPanel('/frame/loading.htm?referrer=/frame/delegate.aspx' + strVar,400,200);
    if (strType == "FORECAST_BACKUP")
        ShowPanel('/frame/loading.htm?referrer=/frame/forecast/forecast_backup.aspx?id=' + strVar,750,200);
//    if (strType == "STORAGE_OVERRIDE")
//        ShowPanel('/frame/loading.htm?referrer=/frame/storage/StorageOverrideUnlock.aspx?id='+strVar,750,200);
    if (strType == "STORAGE_OVERRIDE")
        ShowPanel('/frame/loading.htm?referrer=/frame/storage/storage_override_code.aspx?id='+strVar,750,200);
    if (strType == "DESIGN_UNLOCK")
        ShowPanel('/frame/loading.htm?referrer=/frame/design_unlock.aspx?id='+strVar,600,300);
    if (strType == "INVENTORY_BACKUP")
        window.open('/frame/loading.htm?referrer=/frame/forecast/forecast_backup_sizer.aspx?id=' + strVar,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "INVENTORY_BACKUP_REGISTRATION")
        window.open('/frame/loading.htm?referrer=/frame/forecast/forecast_backup_registration.aspx?id=' + strVar,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "SCHEDULING")
        ShowPanel('/frame/loading.htm?referrer=/frame/scheduling.aspx' + strVar,500,500);
    if (strType == "BACKUP_INCLUSION")
        ShowPanel('/frame/loading.htm?referrer=/frame/ondemand/backup_inclusion.aspx' + strVar,500,170);    
    if (strType == "BACKUP_EXCLUSION")
        ShowPanel('/frame/loading.htm?referrer=/frame/ondemand/backup_exclusion.aspx' + strVar,500,170);
    if (strType == "BACKUP_RETENTION")
        ShowPanel('/frame/loading.htm?referrer=/frame/ondemand/backup_retention.aspx' + strVar,500,400);
    if (strType == "ASSET_DEPLOY")
        window.open('/frame/loading.htm?referrer=' + strVar,"_blank","height=500,width=600,menubar=no,resizable=no,scrollbars=no,status=no,toolbar=no");
    if (strType == "ASSET_DEPLOY_HBAs")
        ShowPanel('/frame/inventory/hba.aspx?id=' + strVar,400,400);
    if (strType == "ASSET_DEPLOY_SWITCHPORTs")
        ShowPanel('/frame/inventory/switchports.aspx?id=' + strVar,600,600);
    if (strType == "ASSET_DEPLOY_VCs")
        ShowPanel('/frame/inventory/virtual_connect.aspx?id=' + strVar,400,450);
    if (strType == "INVENTORY_SUPPLY")
        ShowPanel('/frame/loading.htm?referrer=/frame/inventory/inventory_supply.aspx' + strVar,700,550);
    if (strType == "INVENTORY_DEMAND")
        window.open('/frame/loading.htm?referrer=/frame/inventory/inventory_demand.aspx' + strVar,"_blank","height=550,width=700,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "IDCASSET_TYPE")
        ShowPanel('/frame/loading.htm?referrer=/frame/idc_techassets.aspx' + strVar,450,200);    
    if (strType == "IDCRESOURCE_ASSIGN")
        ShowPanel('/frame/loading.htm?referrer=/frame/idc_resource_assignement.aspx' + strVar,500,320);    
    if (strType == "INVENTORY_HOST")
        window.open('/frame/loading.htm?referrer=/frame/inventory/host_add.aspx' + strVar,"_blank","height=500,width=700,menubar=no,resizable=no,scrollbars=no,status=no,toolbar=no");
    if (strType == "DEPLOY_HOST")
        window.open('/frame/loading.htm?referrer=' + strVar,"_blank","height=250,width=600,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "DEPLOY_HOST_OS")
        window.open('/frame/loading.htm?referrer=/frame/inventory/host_virtual_configure_os.aspx' + strVar,"_blank","height=500,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "DEPLOY_HOST_ENVIRONMENT")
        window.open('/frame/loading.htm?referrer=/frame/inventory/host_virtual_configure_environment.aspx' + strVar,"_blank","height=300,width=400,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "NEW_USER")
        ShowPanel('/frame/loading.htm?referrer=/frame/new_user.aspx' + strVar, 600, 400);
    if (strType == "SERVICE_LINK")
        ShowPanel('/frame/loading.htm?referrer=/frame/service_link.aspx' + strVar, 600, 400);
    if (strType == "DEVICE_NAME")
        ShowPanel('/frame/loading.htm?referrer=/frame/device_name.aspx' + strVar,600,600);    
    if (strType == "DESIGN_APPROVERS")
        ShowPanel('/frame/loading.htm?referrer=/frame/design_approvers.aspx' + strVar,600,600);    
    if (strType == "NEW_CONTROL")
        ShowPanel('/frame/loading.htm?referrer=/frame/new_control.aspx' + strVar,600,500);    
    if (strType == "SERVICE_EDITOR_DISPLAY")
        ShowPanel('/frame/loading.htm?referrer=/frame/service_editor_display.aspx' + strVar,700,500);    
    if (strType == "SERVICE_EDITOR_FIELD_MAPPINGS")
        ShowPanel('/frame/loading.htm?referrer=/frame/service_editor_field_mappings.aspx' + strVar,700,500);    
    if (strType == "SERVICE_EDITOR_CONDITIONS")
        ShowPanel('/frame/loading.htm?referrer=/frame/service_editor_conditions.aspx' + strVar,600,500);    
    if (strType == "SERVICE_EDITOR_WORKFLOW_PRINT")
        window.open('/frame/loading.htm?referrer=/frame/service_editor_workflow_print.aspx' + strVar,"_blank","height=500,width=800,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
    if (strType == "TASKS_ORDER")
        ShowPanel('/frame/loading.htm?referrer=/frame/service_task_order.aspx' + strVar,400,350);    
   if (strType == "FIELD_PLOTS")
       ShowPanel('/frame/loading.htm?referrer=/frame/order_report_plots.aspx'+strVar,450,380);
   if (strType == "PRIORITIZATION_QUESTION")
       ShowPanel('/frame/loading.htm?referrer=/frame/prioritization_question.aspx'+strVar,450,250);   
    if (strType == "PRIORITIZATION_RESPONSE")
       ShowPanel('/frame/loading.htm?referrer=/frame/prioritization_response.aspx'+strVar,450,250);                   
    if (strType == "PRIORITIZATION_QA")              
       ShowPanel('/frame/loading.htm?referrer=/frame/prioritization_qa.aspx'+strVar, 400, 400);
    if (strType == "PCR/CSRC_APPROVALS")
    {
       if(strVar.match('view') != null)
         ShowPanel('/frame/loading.htm?referrer=/frame/pcr_csrc_approvals.aspx'+strVar,450,200);      
        else        
         ShowPanel('/frame/loading.htm?referrer=/frame/pcr_csrc_approvals.aspx'+strVar,550,250);      
     }
    if (strType == "PCR_FORM")
       ShowPanel('/frame/loading.htm?referrer=/frame/pcr_form.aspx'+strVar,800,350);             
    if (strType == "CSRC_FORM")
       ShowPanel('/frame/loading.htm?referrer=/frame/csrc_form.aspx'+strVar,800,350);    
    if (strType == "PDF_TSM")
       ShowPanel('/frame/loading.htm?referrer=/frame/ondemand/pdf_tsm.aspx'+strVar,400,250);    
    if (strType == "PDF_SAN")
       ShowPanel('/frame/loading.htm?referrer=/frame/ondemand/pdf_san.aspx'+strVar,400,250);    
    if (strType == "PDF_BIRTH")
       ShowPanel('/frame/loading.htm?referrer=/frame/ondemand/pdf_birth.aspx'+strVar,400,250);    
    if (strType == "PCR/CSRC_APPROVALS")
    {
        if(strVar.match('view') != null)
            ShowPanel('/frame/loading.htm?referrer=/frame/pcr_csrc_approvals.aspx'+strVar,450,200);      
        else        
            ShowPanel('/frame/loading.htm?referrer=/frame/pcr_csrc_approvals.aspx'+strVar,550,250);      
     }
   if (strType == "ENHANCEMENT")
       ShowPanel('/frame/loading.htm?referrer=/frame/enhancement_view.aspx'+strVar,700,350);    
   if (strType == "SUPPORT")
       ShowPanel('/frame/loading.htm?referrer=/frame/issue_view.aspx'+strVar,700,350);       
   if (strType == "USER_GUIDE")
       ShowPanel('/frame/loading.htm?referrer=/frame/userguide_mail.aspx'+strVar,700,500);
    if (strType == "PROJECT_INFO")
        ShowPanel('/frame/loading.htm?referrer=/frame/project_info.aspx?id=' + strVar,400,400);
   if (strType == "ZEUS_STATUS")
       ShowPanel('/frame/loading.htm?referrer=/frame/zeus.aspx?s=' + strVar,700,500);    
   if (strType == "FIX_SERVER_ERROR")
       ShowPanel('/frame/loading.htm?referrer=/frame/server_error.aspx' + strVar,200,200);    
   if (strType == "PRODUCTION_DATE")
       ShowPanel('/frame/loading.htm?referrer=/frame/production.aspx?id=' + strVar,600,100);    
   if (strType == "COST_AVOIDANCE")
       ShowPanel('/frame/loading.htm?referrer=/frame/costavoidance_mapping.aspx' + strVar,400,300);
   if (strType == "DOCUMENT_REPOSITORY_SHARE")
       ShowPanel('/frame/loading.htm?referrer=/frame/document_repository_share.aspx' + strVar,500,500);
   if (strType == "DOCUMENT_REPOSITORY_RENAME")
       ShowPanel('/frame/loading.htm?referrer=/frame/document_repository_rename.aspx' + strVar,400,150);
   if (strType == "SERVICE_EDITOR_WORKFLOW")
       ShowPanel('/frame/loading.htm?referrer=/frame/workflow.aspx' + strVar,600,500);
   if (strType == "DATAPOINT_FIELDS")
       ShowPanel('/datapoint/dp_fields.aspx' + strVar,600,600);
   if (strType == "DATAPOINT_APPLICATIONS")
       ShowPanel('/datapoint/dp_applications.aspx' + strVar,600,600);
   if (strType == "DATAPOINT_MODELS")
       ShowPanel('/datapoint/dp_models.aspx' + strVar,600,600);
    if (strType == "DESIGN_PRINT")
        window.open('/frame/loading.htm?referrer=/datapoint/service/DesignView.aspx' + strVar,"_blank","height=500,width=700,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
   if (strType == "PROVISIONING_ERROR")
       ShowPanel('/ondemand/error_message.aspx?error=' + strVar,750,500);
   if (strType == "ENHANCEMENT_APPROVAL_GROUPS")
       ShowPanel('/frame/loading.htm?referrer=/frame/enhancement_approvals_groups.aspx' + strVar,600,300);
    return false;
}
function OpenNewWindow(strPath, strW, strH) {
    window.open('/frame/loading.htm?referrer=' + strPath,"_blank","height=" + strH + ",width=" + strW + ",menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
}
function OpenNewWindowMenu(strPath, strW, strH) {
    window.open('/frame/loading.htm?referrer=' + strPath,"_blank","height=" + strH + ",width=" + strW + ",menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=yes");
    return false;
}
function OpenNewWindowMAX(strPath) {
//    window.open('/frame/loading.htm?referrer=' + strPath,"_blank","height=" + window.top.document.body.clientHeight + ",width=" + window.top.document.body.clientWidth + ",menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no,top=0,left=0");
    window.open(strPath,"_blank","height=" + window.top.document.body.clientHeight + ",width=" + window.top.document.body.clientWidth + ",menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no,top=0,left=0");
}
function UpdatePanel(strValue, strControlValue, strText, strControlText) {
    strControlValue = Get(strControlValue);
    strControlValue.value = strValue;
    if (strText != null && strControlText != null) {
        strControlText = Get(strControlText);
        strControlText.innerText = strText;
    }
}
function UpdatePanelText(strValue, strControlValue, strText, strControlText) {
    strControlValue = Get(strControlValue);
    strControlValue.value = strValue;
    if (strText != null && strControlText != null) {
        strControlText = Get(strControlText);
        strControlText.value = strText;
    }
}
function UpdateLocation(strValue, strControlValue, strText, strControlText) {
    strControlValue = Get(strControlValue);
    strControlValue.value = strValue;
    if (strText != null && strControlText != null) {
        strControlText = Get(strControlText);
        strControlText.innerText = strText;
    }
    try {
        LocationFunction();
    }
    catch (ex) {
    }
}
function Get(strObject) {
    var oObject = document.getElementById(strObject);
    var oFrames = window.top.frames;
    if (oObject == null && oFrames.length > 0) {
        for (var ii=0; ii<oFrames.length; ii++) {
            oObject = oFrames(ii).document.getElementById(strObject);
            if (oObject != null)
                break;
        }
    }
    return oObject;
}
function ShowPanel(strFile, intWidth, intHeight, boolNewVersion) {
    window.top.document.body.scroll = "NO";
	var oFrameShow = window.top.document.getElementById('frmLiveShow');
	var oDivCover = window.top.document.getElementById('divLiveCover');
	var oDivDisable = window.top.document.getElementById('divDisableCover');
	if (boolNewVersion != null) 
	{
	    oDivCover.onclick = "";
	    oDivCover.style.filter = "alpha(opacity=40)";
	    oDivCover.style.backgroundColor = "#FFFFFF";
	}
	var oFrameCover = window.top.document.getElementById('frmLiveCover');
	oFrameShow.src = "";
    oFrameShow.style.display = "none";
	if (boolNewVersion == null) 
        oFrameShow.style.border = "solid 2px #999999";
    else
        oFrameShow.style.border = "solid 1px #EEEEEE";
    oDivCover.style.posLeft = window.top.document.body.scrollLeft;
    oDivCover.style.display = "inline";
    oDivCover.style.posTop = window.top.document.body.scrollTop;
    oDivCover.style.width = '100%';
    oDivCover.style.height = '100%';
    DHTMLHelp(oDivCover, oFrameCover);
    oFrameShow.contentWindow.navigate(strFile);
    oFrameShow.style.display = "inline";
    oFrameShow.style.posLeft = ((parseInt(window.top.document.body.clientWidth) / 2) - parseInt(intWidth / 2)) + parseInt(window.top.document.body.scrollLeft);
    oFrameShow.style.posTop = ((parseInt(window.top.document.body.clientHeight) / 2) - parseInt(intHeight / 2)) + parseInt(window.top.document.body.scrollTop);
    oFrameShow.style.width = (intWidth + 10) + "px";
    var intWindowHeight = (window.top.document.body.clientHeight);
    if (intWindowHeight > intHeight)
        oFrameShow.style.height = intHeight + "px";
    else
        oFrameShow.style.height = intWindowHeight + "px";
    if (oDivDisable == null) {
        window.attachEvent("onresize", HidePanel);
        window.attachEvent("onscroll", HidePanel);
    }
    else {
        oDivCover.onclick = null;
    }
	return false;
}
function HidePanel() {
    window.top.document.body.scroll = "YES";
    window.detachEvent("onresize", HidePanel);
    window.detachEvent("onscroll", HidePanel);
	var oFrameShow = window.top.document.getElementById('frmLiveShow');
	var oDivCover = window.top.document.getElementById('divLiveCover');
	var oFrameCover = window.top.document.getElementById('frmLiveCover');
	oFrameShow.src = "";
	oFrameShow.style.display = "none";
    oDivCover.style.display = "none";
	oFrameCover.src = "";
	oFrameCover.style.display = "none";
	return false;
}
function DHTMLHelp(oMainTool, oMainFrame) {
    oMainTool.style.zIndex = 100;
    oMainFrame.style.width = oMainTool.offsetWidth;
    oMainFrame.style.height = parseInt(oMainTool.offsetHeight);
    oMainFrame.style.top = oMainTool.style.top;
    oMainFrame.style.left = oMainTool.style.left;
    oMainFrame.style.zIndex = oMainTool.style.zIndex - 1;
    oMainFrame.style.display = "inline";
}
function ShowHideDiv(oDiv, strDisplay) {
    oDiv = document.getElementById(oDiv);
    oDiv.style.display = strDisplay;
}
function ShowHideDiv2(oDiv) {
    oDiv = document.getElementById(oDiv);
    if (oDiv.style.display == "inline")
        oDiv.style.display = "none";
    else
        oDiv.style.display = "inline";
}
function ShowHideDivs(oDiv1, oDiv2) {
    oDiv1 = document.getElementById(oDiv1);
    oDiv2 = document.getElementById(oDiv2);
    if (oDiv1.style.display == "inline") {
        oDiv1.style.display = "none";
        oDiv2.style.display = "inline";
    }
    else {
        oDiv1.style.display = "inline";
        oDiv2.style.display = "none";
    }
}
function ShowHideDiv3(oDiv,oImg) {
    oDiv = document.getElementById(oDiv);
    oImg= document.getElementById(oImg);
    if (oDiv.style.display == "inline")
    {
        oDiv.style.display = "none";
        SwapImage(oImg, "/images/expand-icon.gif");
        oImg.title = "Expand";
    }
    else
    {
        oDiv.style.display = "inline";
        SwapImage(oImg, "/images/collapse-icon.gif");
        oImg.title = "Collapse";
    }
}
function ShowHideDivCheck(oDiv, oCheck) {
    oDiv = document.getElementById(oDiv);
    if (oCheck.checked == true)
        oDiv.style.display = "inline";
    else
        oDiv.style.display = "none";
}
function ShowHideDivDropDown(oDiv, oDDL, intV1, intV2) {
    oDiv = document.getElementById(oDiv);
    if (oDDL.selectedIndex == intV1 || oDDL.selectedIndex == intV2)
        oDiv.style.display = "inline";
    else
        oDiv.style.display = "none";
}
function ShowHideDivDDL(oDiv, oDDL, strValue) {
    oDiv = document.getElementById(oDiv);
    if (oDDL.options[oDDL.selectedIndex].value == strValue)
        oDiv.style.display = "inline";
    else
        oDiv.style.display = "none";
}
function ShowHideDivExample(oDiv, oA) {
    oDiv = document.getElementById(oDiv);
    if (oDiv.style.display == "inline") {
        oDiv.style.display = "none";
        oA.innerText = "Show Example";
    }
    else {
        oDiv.style.display = "inline";
        oA.innerText = "Hide Example";
    }
}
function addLoadEvent(func) {
    var oldonload = window.onload;
    if (typeof window.onload != 'function') {
        window.onload = func;
    }
    else {
        window.onload = function() {
            if (oldonload) {
                oldonload();
            }
            func();
        }
    }
}
function LoadDemo() {
    var oDemo = document.createElement('DIV');
    oDemo.style.posLeft = '0';
    oDemo.style.posTop = '0';
    oDemo.style.width = '100%';
    oDemo.style.height = '100%';
    oDemo.style.position = 'absolute';
    oDemo.style.backgroundImage = 'url(/images/demo.gif)';
    document.body.appendChild(oDemo);
}
function CancelEnter() {
    if (window.event.keyCode == 13)
        return false;
    else
        return true;
}
function DatabaseName() {
    if ((window.event.keyCode > 96 && window.event.keyCode < 123) || (window.event.keyCode > 47 && window.event.keyCode < 58) || (window.event.keyCode == 95))
        return true;
    else
        return false;
}
function AssignPriority(oC, oT) {
    if (oC != "") {
        oC = document.getElementById(oC);
        oC.innerText = oT;
    }
}
var oldTextItem = "";
function ChangeTextItems(oHidden, oValue, oText) {
    if (isNumber(oText.value) == true) {
        oldTextItem = oText.value;
        if (trim(oText.value) != "")
            UpdateHiddenItems(oHidden, oValue, oText.value);
    }
    else
        oText.value = oldTextItem;
}
function ChangeCheckItemsComma(oHidden, oValue, oCheck) {
    var strCheck = "0";
    if (oCheck.checked == true)
        strCheck = "1";
    UpdateHiddenItemsComma(oHidden, oValue, strCheck);
}
function UpdateHiddenItemsComma(oHidden, strValue, strText) {
    oHidden = document.getElementById(oHidden);
    oHidden.value = UpdateStringItemsComma(oHidden.value, strValue, strText);
}
function UpdateStringItemsComma(strHidden, strValue, strText) {
    if (strHidden.indexOf(strValue) > -1) {
        var strBefore = strHidden.substring(0, strHidden.indexOf(strValue));
        strHidden = strHidden.substring(strHidden.indexOf(strValue));
        var strAfter = strHidden.substring(strHidden.indexOf(",") + 1);
        strHidden = strBefore + strAfter;
    }
    else
        strHidden += strValue + ",";
    return strHidden;
}
function ChangeCheckItems(oHidden, oValue, oCheck) {
    var strCheck = "0";
    if (oCheck.checked == true)
        strCheck = "1";
    UpdateHiddenItems(oHidden, oValue, strCheck);
}
function UpdateHiddenItems(oHidden, strValue, strText) {
    oHidden = document.getElementById(oHidden);
    oHidden.value = UpdateStringItems(oHidden.value, strValue, strText);
}
function ConfirmDeleteSubItem(_string, _url) {
    if (confirm('Are you sure you want to ' + _string + ' this service?') == true) {
        if (_url != null) {
            if (confirm('This service is part of a workflow. Cancelling this service could still initiate subsequent workflows.\n\nOK = Initiate subsequent workflows\nCancel = Cancel subsequent workflows') == false) {
                window.location.href = _url.href + "&wfc=true";
                return false;
            }
        }
        return true;
    }
    else
        return false;
}
var strCheckedRows = "";
function HighlightCheckRow(oCheck, oRow, oValue, oHidden, oDisabled) {
    if (oDisabled == true) {
        var strBack = GetStringItems(strCheckedRows, oRow);
        var strRow = oRow;
	    oRow = document.getElementById(oRow);
        if (strBack == "")
            strCheckedRows = UpdateStringItems(strCheckedRows, strRow, oRow.style.backgroundColor);
        var strCheck = "0";
	    if (oCheck.checked == true) {
            strCheck = "1";
		    oRow.style.backgroundColor = strBackColor;
	    }
	    else {
		    oRow.style.backgroundColor = strBack;
	    }
        UpdateHiddenItems(oHidden, oValue, strCheck);
    }
    else {
        alert('Use "Workload Manager - Add Task" to assign a technician to this service');
        oCheck.checked = false;
    }
}
function UpdateQuantitySR(oText, strItem, oHidden) {
    UpdateHiddenItems(oHidden, strItem, oText.value);
}
function UpdateQuantity(oText, strItem, oHidden) {
    UpdateHiddenItems(oHidden, strItem, oText.value);
}
function UpdateStringItems(strHidden, strValue, strText) {
    if (strHidden.indexOf(strValue + "_") > -1) {
        var strBefore = strHidden.substring(0, strHidden.indexOf(strValue + "_"));
        strHidden = strHidden.substring(strHidden.indexOf(strValue + "_"));
        var strAfter = strHidden.substring(strHidden.indexOf("&"));
        strHidden = strBefore + strValue + "_" + strText + strAfter;
    }
    else
        strHidden += strValue + "_" + strText + "&";
    return strHidden;
}
function ValidateStringItems(oHidden, strText) {
    oHidden = document.getElementById(oHidden);
    var boolReturn = false;
    if (oHidden.value != "")
    {
        var strHidden = oHidden.value;
        while (strHidden != "") {
            var strService = strHidden.substring(0, strHidden.indexOf("&"));
            var strNumber = strService.substring(strHidden.indexOf("_") + 1);
            strHidden = strHidden.substring(strHidden.indexOf("&") + 1);
            if (strNumber != "0") {
                boolReturn = true;
                break;
            }
        }
    }
    if (boolReturn == false) {
        alert(strText);
        return false;
    }
    else
        return true;
}
function GetStringItems(strHidden, strValue) {
    if (strHidden.indexOf(strValue + "_") > -1) {
        strHidden = strHidden.substring(strHidden.indexOf(strValue + "_"));
        strHidden = strHidden.substring(0, strHidden.indexOf("&"));
        strHidden = strHidden.substring(strHidden.indexOf("_") + 1);
        return strHidden;
    }
    else
        return "";
}
function CalendarOver(oCell) {
    oCell.style.cursor = "hand";
    oCell.style.border = "solid 1px #FF8080";
}
function CalendarOut(oCell) {
    oCell.style.cursor = "default";
    oCell.style.border = "solid 1px #E0E0E0";
}
function SwapImage(oImage, strImage) {
    var strOldId = oImage.id;
    var oParent = oImage.parentElement;
    oParent.removeChild(oParent.children(0));
    var oNew = document.createElement("IMG");
    oNew.src = strImage;
    oNew.border = "0";
    if (strOldId != null && strOldId != "")
        oNew.id = strOldId;
    oParent.appendChild(oNew);
}
function SwapImageOnly(oImage, strImage) {
    oImage.src = strImage;
}
function PrintWindow() {
    window.print();
    return false;
}
function ExitWindow() {
    window.close();
    return false;
}
function ValidateStatus(oStatus, oComments) {
    oStatus = document.getElementById(oStatus);
    oComments = document.getElementById(oComments);
    if (oStatus.selectedIndex == 0 || trim(oComments.value) == "")
        return confirm('*** WARNING *** Status updates has not been fully completed.\n\nYou will lose any information in your status updates unless you correct this information.\n\n"OK" - Disregard status update information and continue saving.\n"Cancel" - Return to the form without saving.\n\nAre you sure to continue?');
    else
        return true;
}
function ValidatePhase(oTab, oPhase) {
    oTab = document.getElementById(oTab);
    oPhase = document.getElementById(oPhase);
    if (oTab.value == "S" || oTab.value == "L" || oTab.value == "1" || oTab.value == oPhase.value)
        return true;
    else {
        oTab.value = oPhase.value;
        return true;
    }
}
function ValidateStatusPC(oTab, oVariance, oDate, oComments, oThis, oNext) {
    oTab = document.getElementById(oTab);
    if (oTab.value == "S") {
        var oV = document.getElementById(oVariance);
        if (oV.selectedIndex == 3)
            return ValidateDate(oDate,'Please enter a valid date') && ValidateDropDown(oVariance,'Please make a selection for the variance') && ValidateText(oThis,'Please enter information for this week') && ValidateText(oNext,'Please enter information for next week');
        else
            return ValidateDate(oDate,'Please enter a valid date') && ValidateDropDown(oVariance,'Please make a selection for the variance') && ValidateText(oThis,'Please enter information for this week') && ValidateText(oNext,'Please enter information for next week') && ValidateText(oComments,'Please enter some comments');
    }
    else
        return true;
}
function ValidateStatusTPM(oTab, oScope, oTimeline, oBudget, oDate, oComments, oThis, oNext) {
    oTab = document.getElementById(oTab);
    if (oTab.value == "S") {
        var oS = document.getElementById(oScope);
        var oT = document.getElementById(oTimeline);
        var oB = document.getElementById(oBudget);
        if (oS.selectedIndex == 3 && oT.selectedIndex == 3 && oB.selectedIndex == 3)
            return ValidateDate(oDate,'Please enter a valid date') && ValidateDropDown(oScope,'Please make a selection for the scope') && ValidateDropDown(oTimeline,'Please make a selection for the timeline') && ValidateDropDown(oBudget,'Please make a selection for the budget') && ValidateText(oThis,'Please enter information for this week') && ValidateText(oNext,'Please enter information for next week');
        else
            return ValidateDate(oDate,'Please enter a valid date') && ValidateDropDown(oScope,'Please make a selection for the scope') && ValidateDropDown(oTimeline,'Please make a selection for the timeline') && ValidateDropDown(oBudget,'Please make a selection for the budget') && ValidateText(oThis,'Please enter information for this week') && ValidateText(oNext,'Please enter information for next week') && ValidateText(oComments,'Please enter some comments');
    }
    else
        return true;
}
function ValidateStatusTPM2(oTab, oScope, oTimeline, oBudget, oDate, oComments, oThis, oNext) {
    oTab = document.getElementById(oTab);
    if (oTab.value == "S") {
        oScope = document.getElementById(oScope);
        oTimeline = document.getElementById(oTimeline);
        oBudget = document.getElementById(oBudget);
        oDate = document.getElementById(oDate);
        oComments = document.getElementById(oComments);
        oThis = document.getElementById(oThis);
        oNext = document.getElementById(oNext);
        if (oScope.selectedIndex == 0 || oTimeline.selectedIndex == 0 || oBudget.selectedIndex == 0 || isDate(oDate.value) == false || trim(oComments.value) == "" || trim(oThis.value) == "" || trim(oNext.value) == "")
            return confirm('NOTE: Weekly status has not been completed and will not be saved. By clicking "OK", you will lose any information you put in your weekly status unless you correct this information.\n\nAre you sure to continue?');
        else
            return true;
    }
    else
        return true;
}
function ValidateMilestone(oTab, oApproved, oForecasted, oMilestone, oDescription) {
    oTab = document.getElementById(oTab);
    if (oTab.value == "L") {
        return ValidateDate(oApproved,'Please enter a valid date') && ValidateDate(oForecasted,'Please enter a valid date') && ValidateText(oMilestone,'Please enter a title');
    }
    else
        return true;
}
var oWaitDiv23 = null;
function WaitDDL(oDiv) {
    oWaitDiv23 = document.getElementById(oDiv);
    oWaitDiv23.style.display = "inline";
    oWaitDiv23.style.display = "none";
    setTimeout("WaitDDL23()",50);
    return true;
}
function WaitDDL23() {
    oWaitDiv23.style.display = "inline";
}
function WaitDDL2() {
	var oDivCover = window.top.document.getElementById('divLiveCover');
	var oFrameCover = window.top.document.getElementById('frmLiveCover');
    oDivCover.style.display = "inline";
    oDivCover.style.posLeft = ((parseInt(window.top.document.body.clientWidth) / 2) - 100);
    oDivCover.style.posTop = ((parseInt(window.top.document.body.clientHeight) / 2) - 100) + parseInt(window.top.document.body.scrollTop);
    oDivCover.style.width = "450px";
    oDivCover.style.height = "150px";
    oDivCover.style.padding = "50";
    oDivCover.style.backgroundColor = "#CCCCCC";
    oDivCover.style.border = "solid 1px #999999";
    oDivCover.style.filter = "";
    DHTMLHelp(oDivCover, oFrameCover);
    var oDiv = document.createElement('DIV');
    oDiv.align = "center";
    var oImage = document.createElement('IMG');
    oImage.src = "/images/loader.gif";
    oImage.align = "absmiddle";
    oDiv.appendChild(oImage);
    oDivCover.appendChild(oDiv);
    oWaitDiv23 = oDivCover;
    oWaitDiv23.style.display = "none";
    setTimeout("WaitDDL23()",50);
}
function ValidateNewProject(oNumber, oName, oBase, oOrg) {
    oNumber = document.getElementById(oNumber);
    oName = document.getElementById(oName);
    oBase = document.getElementById(oBase);
    oOrg = document.getElementById(oOrg);
    if (trim(oNumber.value) == "" && (trim(oName.value) == "" || oBase.selectedIndex == 0 || oOrg.selectedIndex == 0)) {
        alert('Validation failed! Please correct one of the following errors:\n\n-  Please enter a project number\n-  Please enter a project name, select the project type, and select a sponsoring portfolio');
        return false;
    }
    else
        return true;
}
function SelectProject(strValue) {
    if(window.opener != null)
        window.opener.navigate(window.location.href + "?pid=" + strValue);
}
function OpenLocations(strHidden, strLabel) {
    var oHidden = document.getElementById(strHidden);
    return OpenWindow('LOCATIONS','?id=' + oHidden.value + '&control=' + strHidden + '&controltext=' + strLabel);
}
function UpdateRRCheckDetail(oHidden, oCheck) {
    oHidden = document.getElementById(oHidden);
    if (oCheck.checked == true)
        oHidden.value = "1";
    else
        oHidden.value = "0";
}
function SelectNode(strValue, oHidden, oLabel) {
    oHidden = document.getElementById(oHidden);
    oHidden.value = strValue;
    if (oLabel != null) {
        oLabel = document.getElementById(oLabel);
        oLabel.innerText = strValue;
    }
}
function SelectRadio(oText, oRadio) {
    oRadio = document.getElementById(oRadio);
    if (trim(oText.value) == "")
        oRadio.checked = false;
    else
        oRadio.checked = true;
}
function ClickRadio(oRadio, oText) {
    oText = document.getElementById(oText);
    if (trim(oText.value) == "")
        oRadio.checked = false;
    else
        oRadio.checked = true;
}
var strQueryCheck = null;
var strQueryUrl = null;
function ForecastFilter(oClass, oEnv, strUrl) {
    if (strUrl.indexOf("?") > -1) {
        strQueryCheck = strUrl.substring(strUrl.indexOf("?"));
        strQueryUrl = strUrl.substring(0, strUrl.indexOf("?"));
        RemoveQuerystring("c", oClass);
        RemoveQuerystring("e", oEnv);
//        window.top.navigate(strUrl + strQueryCheck);
        window.top.location.href = (strQueryUrl + strQueryCheck);
    }
    else {
        window.top.location.href = (strUrl + "?c=" + oClass + "&e=" + oEnv);
    }
}
function RemoveQuerystring(strQ, strValue) {
    if (strQueryCheck.indexOf("?" + strQ + "=") > -1) {
        strQ = "?" + strQ + "=";
    }
    else if (strQueryCheck.indexOf("&" + strQ + "=") > -1) {
        strQ = "&" + strQ + "=";
    }
    else {
        strQ = "&" + strQ + "=";
        strQueryCheck = strQueryCheck + strQ + strValue;
        return;
    }
    var strBefore = strQueryCheck.substring(0, strQueryCheck.indexOf(strQ));
    strQueryCheck = strQueryCheck.substring(strQueryCheck.indexOf(strQ) + strQ.length);
    if (strQueryCheck.indexOf("&") == -1)
        strQueryCheck = strBefore + strQ + strValue;
    else {
        var strAfter = strQueryCheck.substring(strQueryCheck.indexOf("&"));
        strQueryCheck = strBefore + strQ + strValue + strAfter;
    }
}
function CopyTextBox(oFrom, oTo) {
    oFrom = document.getElementById(oFrom);
    oTo = document.getElementById(oTo);
    oTo.value = oFrom.value;
}
function MakeWider(oButton, oObject) {
    oObject = document.getElementById(oObject);
    if (oButton.value == "< >") {
        oObject.style.width = "400";
        oObject.style.height = "200";
        oButton.value = "> <";
        oButton.title = "Minimize Listing";
    }
    else {
        oObject.style.width = "200";
        oObject.style.height = "60";
        oButton.value = "< >";
        oButton.title = "Maximize Listing";
    }
    return false;
}
function ClearList(oObject) {
    oObject = document.getElementById(oObject);
    for (var ii=0; ii<oObject.length; ii++)
        oObject.options[ii].selected = false;
    return false;
}
function ChangeTab(oTD, oDivID, oHidden, oValue, boolDynamic, boolPrompt) {
    // Show DIV
    var strHide = oDivID.substring(0, 6);
	var oDivs = document.getElementsByTagName("DIV");
	for (var ii=0; ii<oDivs.length; ii++) {
	    if (boolDynamic == false) {
            if (oDivs[ii].id.substring(oDivs[ii].id.lastIndexOf('_')+1,oDivs[ii].id.length-1) == strHide)	   
                oDivs[ii].style.display = "none";	        
	    }
	    else {
	        if (oDivs[ii].id.substring(0,6) == strHide)
	            oDivs[ii].style.display = "none";
        }
    }
	oDivID = document.getElementById(oDivID);
	oDivID.style.display = "inline";
	// Change TAB
	
	oTD = oTD.parentElement;
	var oTable = oTD.parentElement.parentElement;
	for (var jj=0; jj<oTable.children.length; jj++) {
	    var oAoffL = document.createElement("IMG");
	    oAoffL.src = "/images/TabOffLeftCap.gif";
	    var oAoffR = document.createElement("IMG");
	    oAoffR.src = "/images/TabOffRightCap.gif";
	    var oAonL = document.createElement("IMG");
	    oAonL.src = "/images/TabOnLeftCap.gif";
	    var oAonR = document.createElement("IMG");
	    oAonR.src = "/images/TabOnRightCap.gif";
	    var oTR = oTable.children[jj];
	    for (var ii=0; ii<oTR.children.length; ii++) {
		    if (oTR.children[ii] == oTD) {
                oTR.children[ii-1].removeChild(oTR.children[ii-1].children[0]);
                oTR.children[ii-1].appendChild(oAonL);
		        oTR.children[ii].background = "/images/TabOnBackground.gif";
                oTR.children[ii+1].removeChild(oTR.children[ii+1].children[0]);
                oTR.children[ii+1].appendChild(oAonR);
		    }
		    else if (oTR.children[ii].background == "/images/TabOnBackground.gif") {
                oTR.children[ii-1].removeChild(oTR.children[ii-1].children[0]);
                oTR.children[ii-1].appendChild(oAoffL);
		        oTR.children[ii].background = "/images/TabOffBackground.gif";
                oTR.children[ii+1].removeChild(oTR.children[ii+1].children[0]);
                oTR.children[ii+1].appendChild(oAoffR);
	        }
	    }
	}
	if (oHidden != null) {
	    oHidden = document.getElementById(oHidden);
	    oHidden.value = oValue;
	}
}
function SideBar(oA) {
	var oTD = document.getElementById("tdSideBar");
	if (oTD.style.display == "none") {
	    oTD.style.display = "inline";
        oA.removeChild(oA.children(0));
        var oANew = document.createElement("IMG");
        oANew.src = "/images/sidebar_collapse.gif";
        oANew.border = "0";
        oA.appendChild(oANew);
    }
	else {
	    oTD.style.display = "none";
        oA.removeChild(oA.children(0));
        var oANew = document.createElement("IMG");
        oANew.src = "/images/sidebar_expand.gif";
        oANew.border = "0";
        oA.appendChild(oANew);
    }
}
function DisableTextBox(oObject) {
    alert("changed - value: " + oObject.value);
    setTimeout("reallySetValue('" + oObject.id + "');", 1);
//    return false;
}
function reallySetValue(id) {
    var control = document.getElementById(id);
    control.value += control.value;
}
var oProcessButton1 = null;
var oProcessButton2 = null;
var oProcessButton3 = null;
var oProcessButton4 = null;
var oProcessButtonText = null;
var oProcessButtonWidth = null;
function ProcessButton(oButton, oText, oWidth) {
//    WaitWindow();
//    return true;
    oProcessButton1 = oButton;
    oProcessButtonText = oText;
    oProcessButtonWidth = oWidth;
    setTimeout("ProcessButton2()", 100);
    return true;
}
function ProcessButtons(oButton1, oButton2, oButton3, oButton4) {
//    WaitWindow();
//    return true;
    oProcessButton1 = oButton1;
    oProcessButton2 = document.getElementById(oButton2);
    oProcessButton3 = document.getElementById(oButton3);
    oProcessButton4 = document.getElementById(oButton4);
    setTimeout("ProcessButton2()", 100);
    return true;
}
function ProcessButton2() {
    oProcessButton1.value = "Processing...";
    if (oProcessButton2 != null)
        oProcessButton2.value = "Processing...";
    if (oProcessButton3 != null)
        oProcessButton3.value = "Processing...";
    if (oProcessButton4 != null)
        oProcessButton4.value = "Processing...";
    if (oProcessButtonText != null) {
        oProcessButton1.value = oProcessButtonText;
        if (oProcessButton2 != null)
            oProcessButton2.value = oProcessButtonText;
        if (oProcessButton3 != null)
            oProcessButton3.value = oProcessButtonText;
        if (oProcessButton4 != null)
            oProcessButton4.value = oProcessButtonText;
    }
    else {
        oProcessButton1.value = "Processing...";
        if (oProcessButton2 != null)
            oProcessButton2.value = "Processing...";
        if (oProcessButton3 != null)
            oProcessButton3.value = "Processing...";
        if (oProcessButton4 != null)
            oProcessButton4.value = "Processing...";
    }
    if (oProcessButtonWidth != null) {
        oProcessButton1.style.width = oProcessButtonWidth + "px";
        if (oProcessButton2 != null)
            oProcessButton2.style.width = oProcessButtonWidth + "px";
        if (oProcessButton3 != null)
            oProcessButton3.style.width = oProcessButtonWidth + "px";
        if (oProcessButton4 != null)
            oProcessButton5.style.width = oProcessButtonWidth + "px";
    }
    oProcessButton1.disabled = true;
    if (oProcessButton2 != null)
        oProcessButton2.disabled = true;
    if (oProcessButton3 != null)
        oProcessButton3.disabled = true;
    if (oProcessButton4 != null)
        oProcessButton4.disabled = true;
}
function ProcessControlButton() {
    var oDiv = document.getElementById('cntrlButtons');
    if (oDiv != null)
        oDiv.style.display = "none";
    var oDiv2 = document.getElementById('cntrlButtons2');
    if (oDiv2 != null)
        oDiv2.style.display = "none";
    var oDivP = document.getElementById('cntrlProcessing');
    if (oDivP != null)
        oDivP.style.display = "inline";
    setTimeout("ProcessControlButton2()", 100);
    return true;
}
function ProcessControlButton2() {
    var oDiv = document.getElementById('cntrlProcessing');
    if (oDiv != null) 
    {
        oDiv.style.display = "none";
        oDiv.style.display = "inline";
    }
}
var oDrag = false;
function overDrag() {
    window.event.returnValue = false;
}
function enterDrag(oTD) {
    oTD.style.backgroundColor = "#007253";
    window.event.dataTransfer.getData('Text');
}
function leaveDrag(oTD) {
    oTD.style.backgroundColor = "";
}
function onDrop(oTD, strRow, oHidden) {
    oTD.style.backgroundColor = "";
    oTD.innerHTML = "<b>Moving the control... please wait...</b>";
    oHidden = document.getElementById(oHidden);
    oHidden.value = strRow + "_" + oDrag;
    document.forms[0].submit();
}
var oType = "";
var oSrc = "";
function onDrop2(oTD, strRow, oHidden1, strID, oHidden2) { 
    if(oType == 'Question' && oTD.id.match("TDRESP") != null) {   
        alert('You cannot drop this item here');
        leaveDrag(oTD);
    }
    else if(oType == 'Response' && oTD.id.match("TDQ") != null) {
        alert('You cannot drop this item here');
        leaveDrag(oTD);
    }      
    else if(oSrc != strID && oType == 'Response') { 
        alert('You cannot drop this item here');
        leaveDrag(oTD); 
    } 
    else  {
        oTD.style.backgroundColor = "";
        oTD.innerHTML = "<b>Moving the control... please wait...</b>";
        oHidden1 = document.getElementById(oHidden1);
        oHidden1.value = strRow + "_" + oDrag;    
        oHidden2 = document.getElementById(oHidden2);
        oHidden2.value = strID;  
        document.forms[0].submit(); 
    }
}
function ListControlOut(ddl, oHidden) {
    ddl = document.getElementById(ddl);
    if (ddl.selectedIndex > -1) {
        var oldIndex = ddl.selectedIndex;
        ddl.remove(ddl.selectedIndex);
        ddl.selectedIndex = oldIndex;
	    UpdateListControl(ddl, oHidden);
    }
    return false;
}
function ListControlIn(ddl, oHidden, oText) {
    ddl = document.getElementById(ddl);
	var oOption = document.createElement("OPTION");
	oText = document.getElementById(oText);
	ddl.add(oOption);
	oOption.text = oText.value;
	oOption.value = oText.value;
	ddl.selectedIndex = ddl.length - 1;
    UpdateListControl(ddl, oHidden);
    oText.value = "";
    SetFocus(oText);
	return false;
}
function ListControlInH(ddl, oHidden, oText, oTextHidden) {
    ddl = document.getElementById(ddl);
	var oOption = document.createElement("OPTION");
	oText = document.getElementById(oText);
	oTextHidden = document.getElementById(oTextHidden);
	ddl.add(oOption);
	oOption.text = oText.value;
	oOption.value = oTextHidden.value;
	ddl.selectedIndex = ddl.length - 1;
    UpdateListControl(ddl, oHidden);
    oText.value = "";
    oTextHidden.value = "";
    SetFocus(oText);
	return false;
}
function UpdateListControl(oDDL, oHidden) {
    oHidden = document.getElementById(oHidden);
    oHidden.value = "";
    for (var ii=0; ii<oDDL.length; ii++) {
	    oHidden.value = oHidden.value + oDDL.options[ii].value + ",";
    }
}
function ValidateIP(oIP1, oIP2, oIP3, oIP4, strAlert) {
    return (ValidateIntBetween(oIP1, 1, 254, strAlert) && ValidateIntBetween(oIP2, 1, 254, strAlert) && ValidateIntBetween(oIP3, 1, 254, strAlert) && ValidateIntBetween(oIP4, 1, 254, strAlert))
}
function ListControlInIP(ddl, oHidden, oIP1, oIP2, oIP3, oIP4) {
    ddl = document.getElementById(ddl);
	var oOption = document.createElement("OPTION");
	oIP1 = document.getElementById(oIP1);
	oIP2 = document.getElementById(oIP2);
	oIP3 = document.getElementById(oIP3);
	oIP4 = document.getElementById(oIP4);
	var strIP = oIP1.value + '.' + oIP2.value + '.' + oIP3.value + '.' + oIP4.value;
	ddl.add(oOption);
	oOption.text = strIP;
	oOption.value = strIP;
	ddl.selectedIndex = ddl.length - 1;
    UpdateListControl(ddl, oHidden);
    oIP1.value = "";
    oIP2.value = "";
    oIP3.value = "";
    oIP4.value = "";
    SetFocus(oIP1);
	return false;
}
function SetFocusTry(oObject) {
    var boolSuccess = false;
    try {
        oObject.focus();
        if (oObject.style.display == "none")
            boolSuccess = false;
        else {
            // Check all parents
		    var oParent = oObject.parentElement;
		    while (oParent != null && oParent.style.display != "none")
			    oParent = oParent.parentElement;
            boolSuccess = (oParent == null);
		}
    }
    catch (ex) {
        var oTabExecution = document.getElementById("tabExecution");
        var oDivExecution = document.getElementById("divExecution");
        if (oTabExecution != null && oDivExecution != null && oDivExecution.style.display == "none") {
            oTabExecution.click();
            return SetFocusTry(oObject);
        }
    }
    return boolSuccess;
}
function SetFocus(oObject) {
    try {
        oObject.focus();
    }
    catch (ex) {
        // Change Tab Divs
        var oParent = oObject.parentElement;
        var oParentDiv = null;
        while (oParent.tagName != "BODY" && oParent.className != "tabbing") {
            oParentDiv = oParent;
            oParent = oParent.parentElement;
        }
        var intParentTabNew = 1;
        var intParentTabOld = 1;
        var boolParentTabNewFound = false;
        var boolParentTabOldFound = false;
        if (oParent.className == "tabbing") {
	        for(var ii=0; ii<oParent.children.length; ii++) {
	            if (oParentDiv != oParent.children.item(ii) && boolParentTabNewFound == false)
	                intParentTabNew++;
	            else
	                boolParentTabNewFound = true;
	            if (oParent.children.item(ii).style.display == "none" && boolParentTabOldFound == false)
	                intParentTabOld++;
	            else
	                boolParentTabOldFound = true;
            }
            var oParentTab = null;
            while (oParent.tagName != "BODY" && oParentTab == null) {
    	        for(var ii=0; ii<oParent.children.length; ii++) {
	                if (oParent.children.item(ii).id == "tsmenu" + intParentTabOld)
	                    oParentTab = oParent.children.item(ii);
                }
                oParent = oParent.parentElement;
            }
            if (oParentTab != null) {
                var oTabUL = oParentTab;
                while (oTabUL.tagName != "UL")
                    oTabUL = oTabUL.firstChild
                // <ul><li id="li_tsmenu1"><a href="javascript:void(0);" onclick="NewTab('divMenu1','hdnTab',this,1);" title="Asset Information"><strong>Asset Information</strong></a></li>
                var oTabLI = oTabUL.children.item(intParentTabNew - 1);
                var oTabA = oTabLI.firstChild;
                oTabA.click();
                oObject.focus();
            }
            else
                alert('There was a problem changing the tab\nMSG: 201');
        }
    }
}
var oConsistencyName = null;
var oConsistencyID = null;
function ShowConsistency(strType, strVar, oName, oID) {
    oConsistencyName = document.getElementById(oName);
    oConsistencyID = document.getElementById(oID);
    if (strType == "CONSISTENCY_SERVER")
        ShowPanel('/frame/loading.htm?referrer=/frame/ondemand/consistency_server.aspx' + strVar,500,200);    
    if (strType == "CONSISTENCY_SELECT")
        ShowPanel('/frame/loading.htm?referrer=/frame/ondemand/consistency_select.aspx' + strVar,500,200);    
    if (strType == "CONSISTENCY_NEW")
        ShowPanel('/frame/loading.htm?referrer=/frame/ondemand/consistency_new.aspx' + strVar,500,200);    
    return false;
}
function UpdateConsistency(strName, strID) {
	oConsistencyName.value = strName;
	oConsistencyID.value = strID;
    oConsistencyName = null;
    oConsistencyID = null;
    HidePanel();
}
function CheckOnDemandStepServer(intServer, intStep) {
    oActiveXOnDemandStep = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXOnDemandStep.onreadystatechange = CheckOnDemandStep_a;
    oActiveXOnDemandStep.open("GET", "/frame/ajax/ajax_ondemand_server.aspx?u=GET", false);
    oActiveXOnDemandStep.send("<ajax><value>" + intServer + "</value><value>" + intStep + "</value></ajax>");
}
function CheckOnDemandStepWorkstation(intWorkstation, intStep) {
    oActiveXOnDemandStep = new ActiveXObject("Microsoft.XMLHTTP");
    oActiveXOnDemandStep.onreadystatechange = CheckOnDemandStep_a;
    oActiveXOnDemandStep.open("GET", "/frame/ajax/ajax_ondemand_workstation.aspx?u=GET", false);
    oActiveXOnDemandStep.send("<ajax><value>" + intWorkstation + "</value><value>" + intStep + "</value></ajax>");
}
function CheckOnDemandStep_a() {
    if (oActiveXOnDemandStep.readyState == 4)
    {
        if (oActiveXOnDemandStep.status == 200) {
            if (oActiveXOnDemandStep.responseXML.documentElement.childNodes[0].text == "1")
                redirectAJAXGo();
        }
        else 
            alert('There was a problem getting the information');
    }
}
function UpdateTextBox(oText, oHidden) {
	oHidden = document.getElementById(oHidden);
	oHidden.value = oText.value;
}
function DisableDDL(oDDL, oText) {
	oDDL = document.getElementById(oDDL);
	oDDL.attachEvent("onchange", "alert('" + oText + "');");
}
function UpdateCheckBox(oCheck, oHidden) {
	oHidden = document.getElementById(oHidden);
	if (oCheck.checked == true)
    	oHidden.value = "1";
   	else
    	oHidden.value = "0";
}
var oClock = null;
var oClockSpan = null;
function StartClockCountdown(oControl, oSpan) {
    oClock = oControl;
    if (oSpan > 0) {
        oClockSpan = oSpan;
        setTimeout("StartClockCountdown2()",1000);
    }
}
function StartClockCountdown2() {
    var oClockText = document.getElementById(oClock);
    if (oClockText.innerText != "")
        oClockSpan = oClockSpan - 1000;

    if (oClockSpan < 0)
        window.navigate(window.location);
    else
    {
        var oDateDiff = oClockSpan;

        //var weeks = Math.floor(oDateDiff / (1000 * 60 * 60 * 24 * 7));
        //oDateDiff -= weeks * (1000 * 60 * 60 * 24 * 7);
        
        var days = Math.floor(oDateDiff / (1000 * 60 * 60 * 24)); 
        oDateDiff -= days * (1000 * 60 * 60 * 24);
        
        var hours = Math.floor(oDateDiff / (1000 * 60 * 60)); 
        oDateDiff -= hours * (1000 * 60 * 60);
        
        var mins = Math.floor(oDateDiff / (1000 * 60)); 
        oDateDiff -= mins * (1000 * 60);
        
        var secs = Math.floor(oDateDiff / 1000); 
        oDateDiff -= secs * 1000;

        //oClockText.innerText = "Begin in " + weeks + " weeks, " + days + " days, " + hours + " hours, " + mins + " mins and " + secs + " seconds...";
        oClockText.innerText = "This build will automatically begin in " + days + " day(s) and " + FormatClock(hours) + ":" + FormatClock(mins) + ":" + FormatClock(secs);
        setTimeout("StartClockCountdown2()",1000);
    }
}
function FormatClock(intValue) {
    var strValue = intValue.toString();
    if (strValue.length == 1)
        strValue = "0" + strValue;
    return strValue;
}
function OpenTextBoxHyperlink(oText) {
    var strAlert = 'You have entered an invalid address...please try again';
    if (ValidateHyperlink(oText, strAlert) == true) {
        oText = document.getElementById(oText);
        try {
            window.open(oText.value);
        }
        catch (ex) {
            alert(strAlert);
            SetFocus(oText);
        }
    }
}
function OpenFileUpload(strURL) {
    try {
        window.open(strURL);
    }
    catch (ex) {
        alert('There was a problem opening the file...');
    }
}
function RefreshOpeningWindow() {
//    alert("WO = " + window.opener);
//    if (window.opener != null)
//        alert("WO = W = " + (window.opener != window));
//    alert("WP = " + window.parent);
//    if (window.parent != null)
//        alert("WP = W = " + (window.parent != window));
    if (window.opener != null && window.opener != window) {
        try {
        window.opener.navigate(window.opener.location.href);
        }
        catch (exOpen) {}
    }
    if (window.parent != null && window.parent != window) {
//        alert("WPO = " + window.parent.opener);
//        if (window.parent.opener != null)
//            alert("WPO = W = " + (window.parent.opener != window));
        try {
            if (window.parent.opener != null)
                window.parent.opener.navigate(window.parent.opener.location.href);
            else
                window.parent.navigate(window.parent.location.href);
        }
        catch (exOpen) {}
    }
}
function CheckChange3(oCheck, oCheck1, oCheck2, oControl1, oControl2, oControl3, oControl4, oControl5) {
	oCheck = document.getElementById(oCheck);
	CheckChange(oCheck, oControl1);
	CheckChange(oCheck, oControl2);
	CheckChange(oCheck, oControl3);
	CheckChange(oCheck, oControl4);
	CheckChange(oCheck, oControl5);
	oCheck1 = document.getElementById(oCheck1);
	if (oCheck1.checked == true)
	    oCheck1.click();
	oCheck2 = document.getElementById(oCheck2);
	if (oCheck2.checked == true)
	    oCheck2.click();
}
function CheckChange(oCheck, oControl) {
	if (oControl != null) {
	    oControl = document.getElementById(oControl);
	    if (oControl != null)
	        oControl.disabled = (oCheck.checked == false);
	}
}
function AlertMessage(strAlert)
{
    alert(strAlert);
    return true;
}

function ValidateIPAddress(oObject, strAlert) 
{
    oObject = document.getElementById(oObject);
     var octet = '(?:25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9][0-9]|[0-9])'; 
    var ip    = '(?:' + octet + '\\.){3}' + octet; 
    var quad  = '(?:\\[' + ip + '\\])|(?:' + ip + ')'; 
    var ipRE  = new RegExp( '(' + quad + ')' ); 
    if (oObject != null && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && ipRE.test(oObject.value) == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    return true;
}

function ValidateMACAddress(oObject, strAlert) 
{
    oObject = document.getElementById(oObject);
     //var macRegExp = '([0-9a-fA-F][0-9a-fA-F]-){5}([0-9a-fA-F][0-9a-fA-F])'; 
    var macRegExp = '([0-9a-fA-F][0-9a-fA-F]:){5}([0-9a-fA-F][0-9a-fA-F])';; 
    var macRE  = new RegExp( '(' + macRegExp + ')' ); 
    
    if (oObject != null && trim(oObject.value) == "" && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    if (oObject != null && macRE.test(oObject.value) == false && boolValidate == true) {
        if (strAlert != "")
            alert(strAlert);
        SetFocus(oObject);
        return false;
    }
    
    return true;
}

var intSearchTextList = -1;
var strSearchTextList = "";
function SearchTextList(oText1, oText2, oList) {
    oText1 = document.getElementById(oText1);
    oText2 = document.getElementById(oText2);
    oList = document.getElementById(oList);
    return SearchTextList2(oText1, oText2, oList);
}
function SearchTextList2(oText1, oText2, oList) {
    var strSearchText = "";
	if (oText1.value != "")
		strSearchText = oText1.value;
	else if (oText2.value != "")
		strSearchText = oText2.value;
	
	if (strSearchText != strSearchTextList) {
	    strSearchTextList = strSearchText;
	    intSearchTextList = -1;
	}
	
	if (strSearchText != "") {
    	var boolFound = false;
        for (var ii=0; ii<oList.length; ii++) {
            if (ii > intSearchTextList && oList.options[ii].text.toUpperCase().indexOf(strSearchText.toUpperCase()) > -1) {
                intSearchTextList = ii;
                oList.selectedIndex = intSearchTextList;
                boolFound = true;
                break;
            }
        }
        if (boolFound == false) {
            if (intSearchTextList > -1) {
                if (confirm('You have reached the end of the list.\n\nWould you like to restart the search from the beginning?') == true)
                {
                    intSearchTextList = -1;
                    return SearchTextList2(oText1, oText2, oList);
                }
            }
            else
                alert('The specified text was not found');
        }
	}
	else
	{
		alert('Please enter some value to find');
	}
	return false;
}
var oSelection = null;
var boolSelectionRepeat = false;
function SearchText(oText, oRepeat) {
    oText = document.getElementById(oText);
    oRepeat = document.getElementById(oRepeat);
   
	if (oText.value != "") {
		if (oSelection == null)
			oSelection = document.body.createTextRange();
		SearchTextNext(oText.value, oRepeat);
	}
	else {
		//alert('Please enter a name to find');
	}
	return false;
}

function SearchTextNext(strText, oRepeat) {
	if (oSelection.findText(strText) == true) {
	    boolSelectionRepeat = false;
		var oParent = oSelection.parentElement();
		while ((oParent != null) && (oParent.id != oRepeat.id)) {
			oParent = oParent.parentElement;
		}
		if (oParent != null) {
			oSelection.select();
			oSelection.scrollIntoView();
			oSelection.collapse(false);
		}
		else {
			oSelection.collapse(false);
			SearchTextNext(strText, oRepeat);
		}
	}
	else if (boolSelectionRepeat == false) {
	    boolSelectionRepeat = true;
		oSelection = document.body.createTextRange();
		SearchTextNext(strText, oRepeat);
	}
	else {
		//alert('The search text was not found');
		oSelection = document.body.createTextRange();
	}
}


function LoadLocationRoomRack( strType,hdnId, ctrlLocation, ctrlRoom, ctrlZone, ctrlRack)
{
   var strVar="";
   var hdnId1 = document.getElementById(hdnId);
   strVar=strVar+"type="+strType;
   strVar=strVar+"&Id="+hdnId1.value;
   strVar=strVar+"&hdnId="+hdnId;
   strVar=strVar+"&ctrlLocation="+ctrlLocation;
   strVar=strVar+"&ctrlRoom="+ctrlRoom;
   strVar=strVar+"&ctrlZone="+ctrlZone;
   strVar=strVar+"&ctrlRack="+ctrlRack;
   
    var oCallingWindow=null;
    oCallingWindow=window.opener;
   if (oCallingWindow!=null)
    { 
        oCallingWindow=window.parent;
        ShowPanel('/frame/loading.htm?referrer=/frame/location_room_rack.aspx?'+strVar,600,300);
    }
    else
    {
        oCallingWindow=window.opener;
        var intHeight=300;
        var intWidth=600;
        var intposLeft=0;
        var intposTop=0;
        intposLeft = ((parseInt(window.top.document.body.clientWidth) / 2) - parseInt(intWidth / 2)) + parseInt(window.top.document.body.scrollLeft);
        intposTop = ((parseInt(window.top.document.body.clientHeight) / 2) - parseInt(intHeight / 2)) + parseInt(window.top.document.body.scrollTop);
        var strPath="/frame/location_room_rack.aspx?"+strVar;
        window.open(strPath,"_blank","height=" + intHeight + ",width=" + intWidth + ",menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no,top="+intposTop+",left="+intposLeft);
    }
   
    return false;

}
function EnableDisableCheck(oCheck, oObject) {
    oObject = document.getElementById(oObject);
    oObject.disabled = (oCheck.checked == false);
    return true;
}
function CheckListAll(oCheck, oParent) {
    var oSpan = oCheck.parentElement;
    oParent = document.getElementById(oParent);
    var oInputs = oParent.getElementsByTagName("INPUT");
    for(var ii=0; ii<oInputs.length; ii++) {
        var oObject = oInputs[ii];
        var oObjectParent = oObject.parentElement;
        if (oObject.getAttribute("type").toUpperCase() == "CHECKBOX" && (oSpan.href == null || oSpan.href == oObjectParent.href) && oObject.disabled == false)
            oObject.checked = oCheck.checked;
    }
}
function GetQuerystringParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}
function DropDownMappings(selected) {
    var oldValue = selected.getAttribute("data-value");
    var oldText = selected.getAttribute("data-text");
    var newValue = selected.options[selected.selectedIndex].value;
    var newText = selected.options[selected.selectedIndex].text;

    var oParent = selected.parentElement;
    while (oParent.tagName != "TABLE") {
        oParent = oParent.parentElement;
    }
    var oInputs = oParent.getElementsByTagName("SELECT");
    for (var ii = 0; ii < oInputs.length; ii++) {
        var oDDL = oInputs[ii];
        if (oDDL != selected) {
            if (newValue != "0") {
                for (var jj = 0; jj < oDDL.length; jj++) {
                    if (oDDL.options[jj].value == newValue) {
                        oDDL.remove(jj);
                        break;
                    }
                }
            }
            if (oldValue != null && oldValue != "0") {
                var oOption = document.createElement("OPTION");
                oDDL.add(oOption);
                oOption.text = oldText;
                oOption.value = oldValue;
            }
        }
    }
    // Set selected value as attribute so it can be retrieved later.
    selected.setAttribute("data-value", newValue);
    selected.setAttribute("data-text", newText);
}