var strToolbarColor = "";
var oSelection = null;
var oEditing = null;
var oEditWindow = null;
var oToolbar = null;
var oPagebar = null;
var oCssDiv = null;
var oFontDiv = null;
var oSymbolDiv = null;
var strCancelEdit = null;
var oEditRow = null;
var oColor = null;
var oPos = null;
var strInsert = "";
var oxml = null;
var oFlagUrl = "";
var oFlagValue = "";
function ToolbarMouseOver(oCell) {
    strToolbarColor = oCell.style.border;
    oCell.style.border = "solid 1px #FF6600";
}
function ToolbarMouseOut(oCell) {
    oCell.style.border = strToolbarColor;
}
function ToolbarMouseUp(oCell) {
    oSelection = document.selection.createRange();
    callCommand(oCell.firstChild.alt, oCell.firstChild.title.toUpperCase());
}
function callCommand(strCommand, strText) {
	addUndoAction(strText, oEditing.innerHTML);
	if (oSelection.text == "") {
	    oEditing.setActive();
	    oEditing.document.execCommand(strCommand);
	}
	else
		oSelection.execCommand(strCommand);
	setTimeout("updateToolbar()",20);
}
function pasteCommand(strValue, strText) {
	addUndoAction(strText, oEditing.innerHTML);
	if (oPos != null)
		oPos.innerHTML = strValue;
	else {
		if (oSelection.text == "") {
		    alert('Click where you want to insert the information...');
		    strInsert = strValue;
		}
		else
			oSelection.pasteHTML(strValue);
	}
	oPos = null;
	setTimeout("updateToolbar()",20);
}
function updateToolbar() {
    if (oEditing != null) {
	    oSelection = document.selection.createRange();
	    var oRow = oToolbar.getElementsByTagName("tr").item(0);
	    for (var yy=0; yy<oRow.children.length; yy++) {
		    oCell = oRow.getElementsByTagName("td").item(yy);
		    if (oCell.firstChild.tagName == "IMG" && oCell.firstChild.alt != "") {
		        if(oSelection.queryCommandValue(oCell.firstChild.alt) == true)
		            oCell.style.border = "solid 1px #FF6600";
		        else
		            oCell.style.border = "solid 1px #F4F3EA";
		    }
	    }
	}
}
function SavePos() {
    if (oEditing != null) {
        if (event.srcElement.tagName == "A")
            oPos = event.srcElement;
        else if (event.srcElement.tagName == "IMG")
            oPos = event.srcElement;
        else
            oPos = null;
        if (strInsert != "")
            setTimeout("InsertSaved()",100);
    }
}
function InsertSaved() {
    oEditing.setActive();
    oSelection = document.selection.createRange();
    oSelection.pasteHTML(strInsert);
    strInsert = "";
}
function divDblClick() {
    if (oEditing != null) {
        if (event.srcElement.tagName == "A") {
            oPos = event.srcElement;
            ShowLink();
        }
        else if (event.srcElement.tagName == "IMG") {
            oPos = event.srcElement;
            ShowImage();
        }
        else
            oPos = null;
    }
}
function EditHTML(oDiv, oMenuEdit, oMenuSave, oTool, oPage, oRow) {
    window.onbeforeunload = ConfirmExit;
    if (oEditing == null) {
        oEditRow = oRow;
        InitToolbar();
        strCancelEdit = oDiv.innerHTML;
	    oEditing = oDiv;
	    oDiv.style.border = 'dashed 1px #666666';
	    oDiv.style.padding = '2';
	    oDiv.contentEditable = true;
	    if (oToolbar == null)
	        oToolbar = Get(oTool);
    	oToolbar.style.display = "inline";
	    if (oPagebar == null)
	        oPagebar = Get(oPage);
    	oPagebar.style.display = "none";
    	oMenuSave = Get(oMenuSave);
        oMenuSave.style.display = "inline";
    	oMenuEdit = Get(oMenuEdit);
        oMenuEdit.style.display = "none";
	    if (oDiv.innerText == "")
		    oDiv.focus();
    }
    else
        alert('You are currently editing another HTML control on this page.\n\nPlease Save or Cancel your changes before you continue...');
	return false;
}
function OpenHTML(oDiv, strSchema) {
    if (strSchema == "")
        oEditing = oDiv;
    else if (oEditing != null) {
        alert('You are currently editing another HTML control on this page.\n\nPlease Save or Cancel your changes before you continue...');
        return false;
    }
    oEditWindow = window.open("/admin/loading.htm?referrer=/admin/html.aspx?control=" + oDiv.id + "&schema=" + strSchema,"NONE","height=600,width=800,menubar=no,resizable=yes,scrollbars=no,status=no,toolbar=no");
	return false;
}
function OpenHTMLUpdate(strHtml) {
    oEditing.innerHTML = strHtml;
    cleanUp(oEditing);
    oEditing = null;
}
function ApproveHTML(oApprove, oMenuEdit, strRequest, strSchema) {
    ApproveHtml_(strRequest, strSchema);
    oApprove.style.display = "none";
	oMenuEdit = Get(oMenuEdit);
    oMenuEdit.src = "/__images/controlBlue.gif";
    return false;
}
function SaveHTML(oDiv, oMenuEdit, oMenuSave, strRequest, strSchema) {
    AddHtml_(strRequest, strSchema);
    if (oEditRow != null) {
        oEditRow.style.display = "inline";
    	var oRed = Get(oMenuEdit);
        oRed.src = "/__images/controlRed.gif";
    }
    return ResetHTML(oDiv, oMenuEdit, oMenuSave);
}
function CancelHTML(oDiv, oMenuEdit, oMenuSave) {
    oEditing = null;
    oDiv.innerHTML = strCancelEdit;
    return ResetHTML(oDiv, oMenuEdit, oMenuSave);
}
function ResetHTML(oDiv, oMenuEdit, oMenuSave) {
	oDiv.style.border = '';
	oDiv.style.padding = '0';
	oEditing = null;
	oDiv.contentEditable = false;
	oToolbar.style.display = "none";
	oPagebar.style.display = "inline";
	oMenuSave = Get(oMenuSave);
    oMenuSave.style.display = "none";
	oMenuEdit = Get(oMenuEdit);
    oMenuEdit.style.display = "inline";
    return false;
}
function AddHtml_(strUrl, strSchema) {
    strUrl += "&type=ADD&schema=" + strSchema;
    oxml = new ActiveXObject("Microsoft.XMLHTTP");
    oxml.onreadystatechange = AddHtml_ajax;
    oxml.open("POST", strUrl, false);
    oxml.send("<ajax>" + escape(oEditing.innerHTML) + "</ajax>");
}
function AddHtml_ajax() {
    if (oxml.readyState == 4)
    {
        if (oxml.status == 200)
            alert('The content has been saved successfully!');
        else 
            alert('There was a problem saving the information');
    }
}
function ApproveHtml_(strUrl, strSchema) {
    strUrl += "&type=APPROVE&schema=" + strSchema;
    oxml = new ActiveXObject("Microsoft.XMLHTTP");
    oxml.onreadystatechange = ApproveHtml_ajax;
    oxml.open("POST", strUrl, true);
    oxml.send(null);
}
function ApproveHtml_ajax() {
    if (oxml.readyState == 4)
    {
        if (oxml.status == 200)
            alert('The content is approved and has been published!');
        else 
            alert('There was a problem saving the information');
    }
}
function GetRevision(intRevision, strUrl, strSchema) {
    strUrl += "&type=REVISION&revisionid=" + intRevision + "&schema=" + strSchema;
    oxml = new ActiveXObject("Microsoft.XMLHTTP");
    oxml.onreadystatechange = GetRevision_ajax;
    oxml.open("POST", strUrl, true);
    oxml.send(null);
}
function GetRevision_ajax() {
    if (oxml.readyState == 4)
    {
        if (oxml.status == 200)
            oEditing.innerHTML = oxml.responseText;
        else 
            alert('There was a problem retrieving the information');
    }
}
function InitToolbar() {
	oCssDiv = document.getElementById("divCss");
	oFontDiv = document.getElementById("divFont");
	oSymbolDiv = document.getElementById("divSymbol");
	aUndo[0] = aTitle;
	aUndo[1] = aText;
	aRedo[0] = aTitle2;
	aRedo[1] = aText2;
}
function ShowFont(oCell) {
	oSelection = document.selection.createRange();
	ShowMenuDown(oFontDiv, oCell, 27);
}
function FontSelect(strValue) {
    addUndoAction("FONT SIZE", oEditing.innerHTML);
    if (oSelection.text == "") {
        oEditing.setActive();
        oEditing.document.execCommand("FontSize", false, strValue);
    }
    else
	    oSelection.execCommand("FontSize", false,  strValue);
    HideHtmlMenus();
    oSelection.select();
}
function ShowCss(oCell) {
	oSelection = document.selection.createRange();
	ShowMenuDown(oCssDiv, oCell, 27);
}
function CssSelect(strValue) {
	var oTemp = oSelection.parentElement();
	while (oTemp != null && oTemp != oEditing)
		oTemp = oTemp.parentElement;
	if (oTemp != null) {
	    addUndoAction("CSS FORMAT", oEditing.innerHTML);
	    if (oSelection.text == "" && oSelection.parentElement() != null) {
	        oSelection.parentElement().className = strValue;
	    }
        else if (oPos != null && (oSelection.text == "" || oPos.tagName == "A"))
            oPos.className = strValue;
        else {
	        oSelection.execCommand('RemoveFormat');
	        oSelection.execCommand('FontName',0,'TemporaryPlaceholder1234509876');
	        var oChildren = document.all.tags("FONT");
	        for (i=0; i<oChildren.length; i++) {
		        if (oChildren[i].face == 'TemporaryPlaceholder1234509876') {
			        oChildren[i].face = "";
			        oChildren[i].className = strValue;
			        oChildren[i].outerHTML = oChildren[i].outerHTML.replace("face=", "");
			        oChildren[i].outerHTML = oChildren[i].outerHTML.replace("??", "");
		        }
	        }
	    }
        oSelection.select();
	    HideHtmlMenus();
	}
}
function ChooseColor(oFunction) {
    oSaveFunction = oFunction;
    oSelection = document.selection.createRange();
    OpenWindow('COLOR','','',false,230,200);
    return false;
}
function SelectColor(strColor) {
	var oTemp = oSelection.parentElement();
	while (oTemp != null && oTemp != oEditing)
		oTemp = oTemp.parentElement;
	if (oTemp != null) {
		addUndoAction("COLOR", oEditing.innerHTML);
		if (oSelection.text == "") {
		    oEditing.setActive();
			oSelection = document.selection.createRange();
			oSelection.execCommand(oSaveFunction, false, '#' + strColor);
		}
		else
			oSelection.execCommand(oSaveFunction, false, '#' + strColor);
		setTimeout("updateToolbar()",20);
	}
}
function ShowImage() {
	oSelection = document.selection.createRange();
    if (oPos != null && oPos.tagName == "IMG") 
    {
        if (oPos.getAttribute("id") == "")
            OpenWindow('UPDATEIMAGE','',"src=" + oPos.getAttribute("src") + "&border=" + oPos.getAttribute("border") + "&hspace=" + oPos.getAttribute("hspace") + "&vspace=" + oPos.getAttribute("vspace") + "&alt=" + oPos.getAttribute("alt") + "&align=" + oPos.getAttribute("align"),false,500,550);
    }
    else
	    OpenWindow('INSERTIMAGE','',"src=/images/spacer.gif",false,500,550);
}
function InsertImage(strValue) {
	pasteCommand(strValue, "INSERT IMAGE");
	cleanUp(oEditing);
}
function UpdateImage(strValue) {
	addUndoAction("UPDATE IMAGE", oEditing.innerHTML)
	oPos.outerHTML = strValue;
    oPos = null;
	cleanUp(oEditing);
}
function ShowLink() {
	oSelection = document.selection.createRange();
    if (oPos != null && oPos.tagName == "A")
        OpenWindow('UPDATELINK','',"title=" + oPos.innerText + "&tip=" + oPos.getAttribute("title") + "&address=" + oPos.getAttribute("href") + "&target=" + oPos.getAttribute("target"),false,500,650);
    else
	    OpenWindow('INSERTLINK','',"title=" + oSelection.text,false,500,650);
}
function InsertLink(strValue) {
	pasteCommand(strValue, "INSERT LINK");
	cleanUp(oEditing);
}
function UpdateLink(strValue) {
	addUndoAction("UPDATE LINK", oEditing.innerHTML)
    oPos.outerHTML = strValue;
    oPos = null;
    cleanUp(oEditing);
}
function ShowSymbol(oCell) {
	oSelection = document.selection.createRange();
	ShowMenuDown(oSymbolDiv, oCell, 27);
}
function SelectSymbol(oCell) {
	addUndoAction("CHARACTER", oEditing.innerHTML);
	oEditing.setActive();
	if (oSelection.text == "")
		oSelection = document.selection.createRange();
	oSelection.text = oCell.innerText;
}
function ShowTable() {
	oSelection = document.selection.createRange();
//    if (oPos != null && oPos.tagName == "A")
//        OpenWindow('UPDATELINK','',"title=" + oPos.innerText + "&tip=" + oPos.getAttribute("title") + "&address=" + oPos.getAttribute("href") + "&target=" + oPos.getAttribute("target"),false,500,650);
//    else
	    OpenWindow('INSERTTABLE','','',false,450,400);
}
function InsertTable(strValue) {
	pasteCommand(strValue, "INSERT TABLE");
}
function InsertHR() {
    oSelection = document.selection.createRange();
	pasteCommand("<hr size=1 noshade />","HORIZONTAL");
}
var oFlagSchema = "";
function ShowFlag(oControl, strFlagSchema, strFlagUrl) {
    oFlagUrl = strFlagUrl;
    oFlagSchema = strFlagSchema;
    oPos = null;
	oSelection = document.selection.createRange();
    OpenWindow('INSERTFLAG', oControl ,'',false,300,200);
}
function InsertFlag(strValue) {
    oFlagUrl += "&type=FLAG&schema=" + oFlagSchema;
    oFlagValue = strValue;
    oxml = new ActiveXObject("Microsoft.XMLHTTP");
    oxml.onreadystatechange = AddFlag_ajax;
    oxml.open("POST", oFlagUrl, false);
    oxml.send("<ajax>" + strValue + "</ajax>");
}
function AddFlag_ajax() {
    if (oxml.readyState == 4)
    {
        if (oxml.status == 200) {
	        pasteCommand("<img id='" + oxml.responseText + "' unselectable='on' src='/__images/activeFlag.gif' border='0' title='" + oFlagValue + "'>", "INSERT FLAG");
	        cleanUp(oEditing);
            alert('The content has been saved successfully!');
        }
        else 
            alert('There was a problem saving the information');
    }
}
function checkFlag() {
    alert(window.event.keyCode);
}
function ShowMenuDown(oDiv, oFind, intHeight) {
	oDiv.style.posLeft = findPosX(oFind);
    oDiv.style.posTop = 25;
	oDiv.style.display = "inline";
	setTimeout("SetHideHtmlMenus()",200);
}
function DivOver(oCell) {
	oCell.style.border = 'solid 1px #0066FF';
	oCell.style.cursor = "hand";
}
function DivOut(oCell) {
	oCell.style.border = 'solid 1px #FFFFFF';
	oCell.style.cursor = "default";
}
function SetHideHtmlMenus() {
	document.attachEvent("onclick", HideHtmlMenus);
}
function HideHtmlMenus() {
	document.detachEvent("onclick", HideHtmlMenus);
	if (oCssDiv != null)
		oCssDiv.style.display = "none";
	if (oFontDiv != null)
		oFontDiv.style.display = "none";
	if (oDivUndoTop != null)
		oDivUndoTop.style.display = "none";
	if (oDivRedoTop != null)
		oDivRedoTop.style.display = "none";
	if (oDivUndo != null)
		oDivUndo.style.display = "none";
	if (oDivRedo != null)
		oDivRedo.style.display = "none";
	if (oSymbolDiv != null)
		oSymbolDiv.style.display = "none";
}
function ConfirmExit() {
    if (oEditing != null && strCancelEdit != null) {
        if (oEditing.innerHTML != strCancelEdit) {
            return "You changed data on this form without saving!";
        }
    }
}

















var maxRedos = 50;
var aTitle = new Array(maxRedos);
var aText = new Array(maxRedos);
var aUndo = new Array(2);
var aTitle2 = new Array(maxRedos);
var aText2 = new Array(maxRedos);
var aRedo = new Array(2);
var boolRedo = false;
var UndoCount = -1;	
var RedoCount = -1;
var oUndoTimer = null;
var strStoreText;
var boolAddOn = false;
var oDivUndoTop;
var oDivUndo;
var oDivRedoTop;
var oDivRedo;
var oSpanDos;

// ***************************************************************
// *******************  UNDO / REDO ******************************
// ***************************************************************
// TWO OPTIONS...
//		1) Update undo action for typing all at once until user makes another change (insert table, hr, etc...)
//		UNCOMMENT FOLLOWING FUNCTION
/*
function updateTypeUndo(oDiv) {
    if (oEditing != null) {
	    if ((UndoCount < maxRedos) && (LastUndoCount != UndoCount)) {
		    strStoreText = oDiv.innerHTML;
		    UndoCount = UndoCount + 1;
		    aUndo[0][UndoCount] = "Typing";
		    aUndo[1][UndoCount] = strStoreText;
		    LastUndoCount = UndoCount;
	    }
	}
}
*/

//		2) Update undo action for typing every second there is a pause
//		// UNCOMMENT FOLLOWING TWO FUNCTIONS
function updateTypeUndo(oDiv) {
    if (oEditing != null) {
	    clearTimeout(oUndoTimer);
	    if (boolAddOn == false) {			// User has stopped typing for one second - store new text
		    strStoreText = oDiv.innerHTML;	// Store text
		    boolAddOn = true;				// User is typing
	    }
	    // Call the function to save a redo after one second
	    oUndoTimer = setTimeout("updateTypeUndo2()",1000);
	}
}

// One second has passed - save the actions made as an undo action
function updateTypeUndo2() {
	clearTimeout(oUndoTimer);
	boolAddOn = false;				// User has stopped typing
	addUndoAction("TYPING", strStoreText);
}

// Change the background color for UNDO / REDO divs (and all the are before them)
function MouseOverUndo() {
	var oCell = event.srcElement;
	var tbody = oCell.parentElement.parentElement;
	for(var ii=0; ii<tbody.children.length; ii++) {
		// Change all that are before the selected item
		if (parseInt(tbody.children.item(ii).firstChild.id) >= parseInt(oCell.id))
			tbody.children.item(ii).firstChild.style.border='solid 1px #FF6600';
		else
			tbody.children.item(ii).firstChild.style.border='solid 1px #FFFFFF';
	}
	// Update the Label with number of actions
	oSpanDos.innerHTML = "<b>Last " + (tbody.children.length - oCell.id) + " Actions</b>";
}

// Change background back to color
function MouseOutUndo() {
	var oCell = event.srcElement;
	var tbody = oCell.parentElement.parentElement;
	for(var ii=0; ii<tbody.children.length; ii++) {
		tbody.children.item(ii).firstChild.style.border='solid 1px #FFFFFF';
	}
	// Reset the label with 0 actions
	oSpanDos.innerHTML = "<b>Last 0 Actions</b>";
}

// Open the UNDO div to select actions to undo
function ShowUndo(oButton) {
	var tbody = document.getElementById("tblUndo").getElementsByTagName("TBODY")[0];
	// Reset table
	tbody.innerText = "";
	// Add actions in reverse order (100 -> 0)
	for(ii=UndoCount; ii>=0; ii--) {
		var row = document.createElement("TR");
		var td1 = document.createElement("TD");
		td1.appendChild(document.createTextNode(aUndo[0][ii]));
		td1.attachEvent("onmouseover", MouseOverUndo);
		td1.attachEvent("onmouseout", MouseOutUndo);
		td1.attachEvent("onclick", ClickUndo);
		td1.setAttribute("id", ii);
		td1.style.border = 'solid 1px #FFFFFF';
		row.appendChild(td1);
		tbody.appendChild(row);
	}
	// Check to see if there's only one action to undo (and do it)
	if ((UndoCount == 0) || (oDivUndo != null && oDivUndo.style.display == "inline")) {
		addRedoAction(aUndo[0][UndoCount], oEditing.innerHTML);
		oEditing.innerHTML = aUndo[1][UndoCount];
		tbody.innerText = "";
		UndoCount = UndoCount - 1;
		boolRedo = true;
	}
	else {
		// Multiple actions - show the DIV
		if (tbody.innerText != "") {
		    if (oDivUndoTop == null)
		        oDivUndoTop = document.getElementById("oUndoTop");
		    if (oDivUndo == null)
		        oDivUndo = document.getElementById("oUndo");
	        ShowUndoDown(oDivUndo, oDivUndoTop, oButton, 27);
			setTimeout("SetHideHtmlMenus()",200);
	        oSpanDos = document.getElementById("spnUndo");
		}
		// NO ACTIONS - DO NOTHING
	}
	return false;		// To prevent postback
}

// Click on an action to undo
function ClickUndo() {
	var oCell = event.srcElement;
	// Add previous UNDOs to REDO
	addRedoAction(aUndo[0][UndoCount], oEditing.innerHTML);
	for(var ii=UndoCount; ii>parseInt(oCell.id); ii--) {
		addRedoAction(aUndo[0][ii-1], aUndo[1][ii]);
	}
	// UNDO Changes
	oEditing.innerHTML = aUndo[1][oCell.id];
	oCell.parentElement.parentElement.parentElement.parentElement.style.display="none";
	UndoCount = parseInt(oCell.id - 1);
	boolRedo = true;
	setTimeout("updateToolbar()",20);
}

// Show the REDO div
function ShowRedo(oButton) {
	if (boolRedo == true) {
    	var tbody = document.getElementById("tblRedo").getElementsByTagName("TBODY")[0];
		// Reset table
		tbody.innerText = "";
		// Populate the Table with actions
		for(ii=RedoCount; ii>=0; ii--) {
			var row = document.createElement("TR");
			var td1 = document.createElement("TD");
			td1.appendChild(document.createTextNode(aRedo[0][ii]));
			td1.attachEvent("onmouseover", MouseOverUndo);
			td1.attachEvent("onmouseout", MouseOutUndo);
			td1.attachEvent("onclick", ClickRedo);
			td1.setAttribute("id", ii);
			td1.style.border = 'solid 1px #FFFFFF';
			row.appendChild(td1);
			tbody.appendChild(row);
		}
		// Check to see if only one action and do it
		if ((RedoCount == 0) || (oDivRedo != null && oDivRedo.style.display == "inline")) {
			var intTempRedoCount = RedoCount;
			addUndoAction(aRedo[0][RedoCount], oEditing.innerHTML);
			oEditing.innerHTML = aRedo[1][intTempRedoCount];
			tbody.innerText = "";
			if (oDivRedo != null && oDivRedo.style.display == "inline")
				RedoCount = intTempRedoCount - 1;
			boolRedo = true;
		}
		else {
			// Multiple actions - show list
			if (tbody.innerText != "") {
		        if (oDivRedoTop == null)
		            oDivRedoTop = document.getElementById("oRedoTop");
		        if (oDivRedo == null)
		            oDivRedo = document.getElementById("oRedo");
    	        ShowUndoDown(oDivRedo, oDivRedoTop, oButton, 27);
				setTimeout("SetHideHtmlMenus()",200);
	            oSpanDos = document.getElementById("spnRedo");
			}
		}
	}
	return false;		// prevent postback
}

// Clicked on an action to redo
function ClickRedo() {
	var oCell = event.srcElement;
	var intRedos = RedoCount;
	// Add previous UNDOs to REDO
	addUndoAction(aRedo[0][intRedos], oEditing.innerHTML);
	for(var ii=intRedos; ii>parseInt(oCell.id); ii--) {
		addUndoAction(aRedo[0][ii-1], aRedo[1][ii]);
	}
	boolRedo = true;
	// REDO Changes
	oEditing.innerHTML = aRedo[1][oCell.id];
	oCell.parentElement.parentElement.parentElement.parentElement.style.display="none";
	RedoCount = parseInt(oCell.id - 1);
	setTimeout("updateToolbar()",20);
}

// Add an undo action
function addUndoAction(strTitle, strText) {
	clearTimeout(oUndoTimer);
	if (UndoCount + 1 >= maxRedos) {
		for(var ii=0; ii<UndoCount; ii++) {
			aUndo[0][ii] = aUndo[0][ii+1];
			aUndo[1][ii] = aUndo[1][ii+1];
		}
		UndoCount = UndoCount - 1;
	}
	UndoCount = UndoCount + 1;
	aUndo[0][UndoCount] = strTitle;
	aUndo[1][UndoCount] = strText;
	boolRedo = false;
	RedoCount = -1;
}

// Add a redo action
function addRedoAction(strTitle, strText) {
	clearTimeout(oUndoTimer);
	if (RedoCount + 1 >= maxRedos) {
		for(var ii=0; ii<RedoCount; ii++) {
			aRedo[0][ii] = aRedo[0][ii+1];
			aRedo[1][ii] = aRedo[1][ii+1];
		}
		RedoCount = RedoCount - 1;
	}
	RedoCount = RedoCount + 1;
	aRedo[0][RedoCount] = strTitle;
	aRedo[1][RedoCount] = strText;
}
function ShowUndoDown(oDiv, oDivTop, oFind, intHeight) {
    ShowMenuDown(oDiv, oFind, intHeight);
    oDivTop.style.posLeft = parseInt(oDiv.style.posLeft);
    var intDivTop = parseInt(oDiv.style.posTop);
    oDivTop.style.posTop = intDivTop;
    oDiv.style.posTop = intDivTop + parseInt(oDivTop.style.height);
	oDivTop.style.display = "inline";
}
