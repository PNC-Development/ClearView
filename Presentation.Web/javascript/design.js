    function UpdateDDL(oDDL, oDesignID, oHidden) {
        var strQuestion = oDDL.name.substring(3);
        var strResponse = oDDL.options[oDDL.selectedIndex].value;
        strResponse = strResponse.substring(0, strResponse.indexOf("_"));
        if (strResponse == "")
            strResponse = "0";
        oHidden = document.getElementById(oHidden);
        oHidden.value = oDDL.options[oDDL.selectedIndex].value;
        
        //alert(strQuestion + "_" + strResponse + "_1");
        LoadAffects(oDesignID + "_" + strQuestion + "_" + strResponse + "_1");
    }
    function UpdateTextValue(oText, oNumber) {
        oNumber = document.getElementById(oNumber);
        oNumber.value = oText.value;
    }
    function UpdateCheckValue(oCheck, oDesignID, oID, oHidden) {
        var strQuestion = oCheck.name.substring(3);
        oHidden = document.getElementById(oHidden);
        var strHidden = oHidden.value;
        var strFind = oID + "_" + oCheck.value;
        if (oCheck.checked == true) {
            // Add
            strHidden += strFind + ",";
        }
        else {
            // Remove
            if (strHidden.indexOf(strFind) > -1) {
                var strBefore = strHidden.substring(0, strHidden.indexOf(strFind));
                strHidden = strHidden.substring(strHidden.indexOf(strFind));
                var strAfter = strHidden.substring(strFind.length);
                if (strAfter.substring(0,1) == ",")
                    strAfter = strAfter.substring(1);
                strHidden = strBefore + strAfter;
            }
        }
        oHidden.value = strHidden;
        //alert(oHidden.name + " = " + oHidden.value);
        
        //alert(strQuestion + "_" + oID + "_" + (oCheck.checked ? "1" : "0"));
        LoadAffects(oDesignID + "_" + strQuestion + "_" + oID + "_" + (oCheck.checked ? "1" : "0"));
    }
    function UpdateChecksValue(oCheck, oDesignID, oID, oHidden) {
        var strQuestion = oCheck.name.substring(3);
        var oChecks = document.getElementsByName(oCheck.name);
        for(var ii = 0; ii < oChecks.length; ii++) {
            if (oChecks[ii].checked == true)
                oChecks[ii].click();
        }
        oHidden = document.getElementById(oHidden);
        oHidden.value = (oCheck.checked ? oID + "_" + oCheck.value : "");
        //alert(oCheck.value);
        //alert(oHidden.value);
        
        //alert(strQuestion + "_" + oID + "_" + (oCheck.checked ? "1" : "0"));
        LoadAffects(oDesignID + "_" + strQuestion + "_" + oID + "_" + (oCheck.checked ? "1" : "0"));
    }
    var oActiveXForecast = null;
    var oTimerForecast = null;
    function LoadAffects(strResponse) {
        LoadWait();
        setTimeout(function(){LoadAffects2(strResponse)},300);
    }
    function LoadAffects2(strResponse) {
        oActiveXForecast = new ActiveXObject("Microsoft.XMLHTTP");
        oActiveXForecast.onreadystatechange = LoadAffects_a;
        oActiveXForecast.open("GET", "/frame/ajax/ajax_design.aspx?u=GET", false);
        // strResponse Example = 3_5 where 3 is questionid and 5 is responseid.  3_0 is for reset of all responses for question # 3.
        oActiveXForecast.send("<ajax>" + escape(strResponse) + "</ajax>");
    }
    function LoadAffects_a() {
        if (oActiveXForecast.readyState == 4)
        {
            if (oActiveXForecast.status == 200) {
                var or = oActiveXForecast.responseXML.documentElement.childNodes;
                PopulateAffectForecast(or);
            }
            else 
                alert('There was a problem getting the information');
        }
    }
    function PopulateAffectForecast(or) {
        var strScript = "";
        for (var ii=0; ii<or.length; ii=ii+2) {
    //            alert(or[ii].childNodes[0].text + ' = ' + or[ii+1].childNodes[0].text);
            var oTemp = document.getElementById('DIV' + or[ii].childNodes[0].text);
            if (oTemp != null) {
                oTemp.style.display = or[ii+1].childNodes[0].text;
//                if (oTemp.style.display == "none") {
//                    // Uncheck the ones that are hidden
//                    var oChecks = oTemp.getElementsByTagName("input"); 
//                    for (var jj = 0; jj < oChecks.length; jj++) {
//                        if (oChecks[jj].type == "checkbox") {
//                            if (oChecks[jj].checked == true)
//                                oChecks[jj].click();
//                        }
//                    }                
//                }
            }
        }
        HidePanel();
        HideDemo();
    }
    function OpenAccountWindow(strID) {
        ShowPanel('/frame/loading.htm?referrer=/frame/design_accounts.aspx?id=' + strID,500,400);
    }
    function OpenStorageWindow(strID) {
        ShowPanel('/frame/loading.htm?referrer=/frame/design_storage.aspx?id=' + strID,500,400);
    }
    
    
    var boolConfigCheck = false;
    var boolConfigCancel = false;
    // GRID FUNCTIONS
    function SetConflictConfig(_ddl, _table, _hidden) {
        var strConfig = _ddl.options[_ddl.selectedIndex].value;
        if (confirm('WARNING: Configurations may take a few seconds to load...please be patient.\n\nAre you sure you want to continue?') == true) {
            var strText = _ddl.options[_ddl.selectedIndex].text;
            _ddl.options[_ddl.selectedIndex].text = "Processing...";
            _ddl.disabled = true;

            var oTable = document.getElementById(_table);
            if (oTable.childNodes.length == 1)  // assume it is a TBODY.. go down one level
                oTable = oTable.firstChild;

            // "S" = Exclude M-F, 8-5
            // "E" = Include only 7PM to midnight
            // "M" = Include only 12AM to 7AM
            // "D" = Opposite of "S", Include only M-F, 8-5
            // "W" = Exclude M-F
            
            // First, do the includes...
            boolConfigCheck = true;
	        for(var intHour=0; intHour<oTable.childNodes.length - 1; intHour++) {
	            // For each hour
	            var oHour = oTable.childNodes[intHour + 1];
	            for(var intDay=1; intDay<oHour.childNodes.length; intDay++) {
	                // For each day
	                if (strConfig == "S" && (intDay == 1 || intDay == 7 || intHour <= 7 || intHour >= 18))
                        oHour.childNodes[intDay].firstChild.click();
                    else if (strConfig == "E"  && (intHour >= 19 && intHour <= 24))
                        oHour.childNodes[intDay].firstChild.click();
                    else if (strConfig == "M"  && (intHour >= 0 && intHour <= 7))
                        oHour.childNodes[intDay].firstChild.click();
	                else if (strConfig == "D" && (intDay >= 2 && intDay <= 6) && (intHour >= 8 && intHour <= 17))
                        oHour.childNodes[intDay].firstChild.click();
	                else if (strConfig == "W" && (intDay == 1 || intDay == 7))
                        oHour.childNodes[intDay].firstChild.click();
                }
            }
            boolConfigCheck = false;
            
            // Next, do the excludes...
            boolConfigCancel = true;
	        for(var intHour=0; intHour<oTable.childNodes.length - 1; intHour++) {
	            // For each hour
	            var oHour = oTable.childNodes[intHour + 1];
                //alert(oHour.childNodes.length);
	            for(var intDay=1; intDay<oHour.childNodes.length; intDay++) {
	                // For each day
	                if (strConfig == "S" && (intDay >= 2 && intDay <= 6) && (intHour >= 8 && intHour <= 17))
                        oHour.childNodes[intDay].firstChild.click();
                    else if (strConfig == "E"  && (intHour <= 18))
                        oHour.childNodes[intDay].firstChild.click();
                    else if (strConfig == "M"  && (intHour >= 8))
                        oHour.childNodes[intDay].firstChild.click();
	                else if (strConfig == "D" && (intDay == 1 || intDay == 7 || intHour <= 7 || intHour >= 18))
                        oHour.childNodes[intDay].firstChild.click();
	                else if (strConfig == "W" && (intDay >= 2 && intDay <= 6))
                        oHour.childNodes[intDay].firstChild.click();
                }
            }
            boolConfigCancel = false;
            
            // Alert
            //alert('The configuration \"' + _ddl.options[_ddl.selectedIndex].text + '\" has been applied to the matrix');
            _ddl.options[_ddl.selectedIndex].text = strText;
            _ddl.disabled = false;
        }
        _ddl.selectedIndex = 0;
        _ddl.blur();
    }
    function SetConflictsDay(_a, _hidden, _day) {
        var oCell = _a.parentElement;
        var oRow = oCell.parentElement;
        var oTable = oRow.parentElement;
        //alert(oTable.childNodes.length);
	    for(var ii=1; ii<oTable.childNodes.length; ii++)
            oTable.childNodes[ii].childNodes[_day].firstChild.click();
    }
    function SetConflictsTime(_a, _hidden, _time) {
        var oCell = _a.parentElement;
        var oRow = oCell.parentElement;
	    for(var ii=0; ii<oRow.childNodes.length; ii++)
	        oRow.childNodes[ii].firstChild.click();
    }

    // CheckBox
    function SetConflictCheck(_check, _hidden, _day, _time) {
        _hidden = document.getElementById(_hidden);
        if ((_check.className == "selectGridCancel" && boolConfigCancel == false) || boolConfigCheck == true)
            ClickGridCheck(_check, _hidden, _day, _time, true);
        else
            ClickGridCheck(_check, _hidden, _day, _time, false);
    }
    function ClickGridCheck(oCheck, oHidden, strDay, strTime, boolCheck) {
        var intAvailable = 0;
        if (oCheck.className == "selectGridCancel") {
            oCheck.className = "selectGridCheck";
            intAvailable = 1;
        }
        else {
            oCheck.className = "selectGridCancel";
            intAvailable = 0;
        }
        //alert('click2');
        //oHidden = document.getElementById(oHidden);
        oHidden.value = UpdateStringItems(oHidden.value, strDay + "_" + strTime, intAvailable);
        //alert('click4');
        if (boolCheck != null) {
            if (boolCheck == true && intAvailable == 0)
                ClickGridCheck(oCheck, oHidden, strDay, strTime, boolCheck);
            else if (boolCheck == false && intAvailable == 1)
                ClickGridCheck(oCheck, oHidden, strDay, strTime, boolCheck);
        }
        //alert('click5');
    }
    
    // Letter
    function SetConflictLetter(_check, _hidden, _day, _time, _letter) {
        _hidden = document.getElementById(_hidden);
        if ((_check.innerHTML == "-" && boolConfigCancel == false) || boolConfigCheck == true)
            ClickGridLetter(_check, _hidden, _day, _time, _letter, true);
        else
            ClickGridLetter(_check, _hidden, _day, _time, _letter, false);
    }
    function ClickGridLetter(oCheck, oHidden, strDay, strTime, strLetter, boolCheck) {
        var intAvailable = 0;
        if (oCheck.innerHTML == "-") {
            oCheck.innerHTML = "<B>" + strLetter + "</B>";
            intAvailable = 1;
        }
        else {
            oCheck.innerHTML = "-";
            intAvailable = 0;
        }
        //alert('click2');
        //oHidden = document.getElementById(oHidden);
        oHidden.value = UpdateStringItems(oHidden.value, strDay + "_" + strTime, intAvailable);
        //alert('click4');
        if (boolCheck != null) {
            if (boolCheck == true && intAvailable == 0)
                ClickGridLetter(oCheck, oHidden, strDay, strTime, strLetter, boolCheck);
            else if (boolCheck == false && intAvailable == 1)
                ClickGridLetter(oCheck, oHidden, strDay, strTime, strLetter, boolCheck);
        }
        //alert('click5');
    }
    var inLunChanged = 0;
    function ChangeLUN(intSize, strPath, oSize, oPath) {
        oSize = document.getElementById(oSize);
        if (oPath != null)
            oPath = document.getElementById(oPath);
        // Check to see if drive size has changed.
        var boolChanged = false;
        if (intSize != oSize.value) {
            boolChanged = true;
            oSize.style.backgroundColor = "#FFEE99";
        }
        if (oPath != null && strPath != oPath.value) {
            boolChanged = true;
            oPath.style.backgroundColor = "#FFEE99";
        }
        else
            oPath.style.backgroundColor = "#FFFFFF";
        if (boolChanged)
            inLunChanged++;
    }
    function CheckLUNs(divStorage) {
        divStorage = document.getElementById(divStorage);
        if (divStorage.style.display == "inline" && inLunChanged > 0) {
            //return confirm('You have made a change to one or more of the storage LUNs without saving it.\n\nWARNING: You could lose your changes if you proceed without saving!\n\nAre you sure you want to continue?');
        }
        return true;
    }
    var oCurrentDiv = null;
    var eException = null;
    var eReject = null;
    var eValid = null;
    var eInvalid = null;
    function ShowDesignDIVException(oException, oReject, oValid, oInvalid, oControl, oControlID, oPage) {
        // Agree to terms first
        oControl.checked = false;
        ShowPanel("/frame/loading.htm?referrer=" + oPage + "?control=" + oControlID, 600, 500);
        eException = oException;
        eReject = oReject;
        eValid = oValid;
        eInvalid = oInvalid;
    }
    function ShowDesignDIVExceptionOK() {
        ShowDesignDIV(true, false, eException, eReject, eValid, eInvalid);
    }
    function ShowDesignDIV(boolException, boolChange, oException, oReject, oValid, oInvalid) {
        oException = document.getElementById(oException);
        oReject = document.getElementById(oReject);
        oValid = document.getElementById(oValid);
        oInvalid = document.getElementById(oInvalid);
        if (oReject.style.display == "inline")
            oCurrentDiv = oReject;
        else if (oValid.style.display == "inline")
            oCurrentDiv = oValid;
        else if (oInvalid.style.display == "inline")
            oCurrentDiv = oInvalid;

        if (oCurrentDiv != null) {
            if (boolException == true) {
                oException.style.display = "inline";
                oCurrentDiv.style.display = "none";
            }
            else {
                oException.style.display = "none";
                oCurrentDiv.style.display = "inline";
            }
        }
        else {
            if (boolException == true)
                oException.style.display = "inline";
            else if (boolChange == true)
                oException.style.display = "none";
        }
    }