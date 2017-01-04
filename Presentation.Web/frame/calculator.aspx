<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="calculator.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.calculator" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
    var oTotal = null;
    var oNum1 = null;
    var oNum2 = null;
    var oFunc = null;
    var oNew = null;
    var oSave = null;
    var oStore = null;
    var oRecall = null;
    function LoadCalc(strStart) {
        document.attachEvent("onkeypress", CatchKeyPress);
        oTotal = document.getElementById('divTotal');
        oSave = document.getElementById('btnSave');
//        oSave.disabled = true;
        oRecall = document.getElementById('btnRecall');
        oRecall.disabled = true;
        oTotal.focus();
        KeyPress(strStart);
        oNew = true;
    }
    function CatchKeyPress() {
        var intCode = parseInt(event.keyCode);
//        alert(event.keyCode);
        if ((intCode > 47 && intCode < 58) || (intCode == 46)) {
            KeyPress(String.fromCharCode(event.keyCode));
            oNew = false;
            return false;
        }
        else {
            if (intCode == 47)
                return Divide();
            else if (intCode == 42)
                return Multiply();
            else if (intCode == 45)
                return Subtract();
            else if (intCode == 43)
                return Add();
            else if (intCode == 13)
                return Equals();
            else if (intCode == 27)
                return Clear();
            else
                return false;
        }
    }
    function KeyPress(strNum) {
        if (oNew == true) {
            oTotal.innerText = strNum;
            oNew = false;
        }
        else
            oTotal.innerText += strNum;
        if (oFunc == null)
            oNum1 = oTotal.innerText;
        else {
            oNum2 = oTotal.innerText;
        }
    }
    function Equals() {
        if (oFunc != null) {
            oNew = true;
            DoFunc();
            oSave.disabled = false;
        }
        return false;
    }
    function DoFunc() {
        var intNum1 = parseFloat(oNum1);
        var intNum2 = parseFloat(oNum2);
        var intTotal = 0.00;
        if (oFunc == "ADD") {
            intTotal = intNum1 + intNum2;
            oNum1 = intTotal;
            oTotal.innerText = intTotal;
        }
        if (oFunc == "DIV") {
            intTotal = intNum1 / intNum2;
            oNum1 = intTotal;
            oTotal.innerText = intTotal;
        }
        if (oFunc == "MUL") {
            intTotal = intNum1 * intNum2;
            oNum1 = intTotal;
            oTotal.innerText = intTotal;
        }
        if (oFunc == "SUB") {
            intTotal = intNum1 - intNum2;
            oNum1 = intTotal;
            oTotal.innerText = intTotal;
        }
        oTotal.focus();
        oFunc = null;
    }
    function Add() {
        oNew = true;
        oFunc = "ADD";
        oTotal.focus();
        return false;
    }
    function Divide() {
        oNew = true;
        oFunc = "DIV";
        oTotal.focus();
        return false;
    }
    function Multiply() {
        oNew = true;
        oFunc = "MUL";
        oTotal.focus();
        return false;
    }
    function Subtract() {
        oNew = true;
        oFunc = "SUB";
        oTotal.focus();
        return false;
    }
    function Convert() {
        var intNum1 = parseFloat(oNum1);
        var intTotal = intNum1 * -1;
        oNum1 = intTotal;
        oTotal.innerText = intTotal;
        oTotal.focus();
        oFunc = null;
    }
    function MemStore() {
        oStore = parseFloat(oNum1);
        oRecall.disabled = false;
        oTotal.focus();
    }
    function MemRecall() {
        if (oStore != null) {
            oTotal.innerText = oStore;
            if (oFunc == null)
                oNum1 = oTotal.innerText;
            else {
                oNum2 = oTotal.innerText;
            }
        }
        oTotal.focus();
    }
    function SquareRoot() {
        var intNum1 = parseFloat(oNum1);
        if (intNum1 < 0) {
            alert('The number must be greater than 0');
        }
        else {
            var intTotal = Math.sqrt(intNum1);
            oNum1 = intTotal;
            oTotal.innerText = intTotal;
            oNew = true;
            oFunc = null;
            oSave.disabled = false;
        }
        oTotal.focus();
    }
    function Square() {
        var intNum1 = parseFloat(oNum1);
        var intTotal = intNum1 * intNum1;
        oNum1 = intTotal;
        oTotal.innerText = intTotal;
        oTotal.focus();
        oFunc = null;
    }
    function Clear() {
        oFunc = null;
        oNum1 = null;
        oNum2 = null;
        oNew = true;
        oTotal.innerText = "";
        oSave.disabled = true;
        oTotal.focus();
        return false;
    }
    function Save() {
        if (oSave.disabled == false) {
            if (oTotal.innerText != "") {
                if(window.opener == null)  
                    window.top.UpdateCalculator(oTotal.innerText);
                else
                {
                    window.opener.CloseCalculator(oTotal.innerText);
                    window.close();
                }
            }
            else
                window.top.UpdateCalculator('0');
        }
        return false;
    }
</script>
<table cellpadding="2" cellspacing="0" border="0">
    <tr>
        <td colspan="5"><div align="right" contenteditable="true" id="divTotal" style="width:100%; border:solid 1px #0066CC; padding:3" class="mainheader" /></td>
    </tr>
    <tr height="5">
        <td colspan="5"><img src="/images/spacer.gif" border="0" width="1" height="5" /></td>
    </tr>
    <tr>
        <td><input type="button" value="&radic;" onclick="SquareRoot();" title="Square Root" class="header" style="width:35; height:35" /></td>
        <td><input type="button" value="7" onclick="KeyPress('7');" class="header" style="width:35; height:35" /></td>
        <td><input type="button" value="8" onclick="KeyPress('8');" class="header" style="width:35; height:35" /></td>
        <td><input type="button" value="9" onclick="KeyPress('9');" class="header" style="width:35; height:35" /></td>
        <td><input type="button" value="&divide;" onclick="Divide();" class="header" style="width:35; height:35" /></td>
    </tr>
    <tr>
        <td><input type="button" value="x&sup2;" onclick="Square();" title="Square" class="header" style="width:35; height:35" /></td>
        <td><input type="button" value="4" onclick="KeyPress('4');" class="header" style="width:35; height:35" /></td>
        <td><input type="button" value="5" onclick="KeyPress('5');" class="header" style="width:35; height:35" /></td>
        <td><input type="button" value="6" onclick="KeyPress('6');" class="header" style="width:35; height:35" /></td>
        <td><input type="button" value="&times;" onclick="Multiply();" class="header" style="width:35; height:35" /></td>
    </tr>
    <tr>
        <td><input type="button" value="+/-" onclick="Convert();" class="header" style="width:35; height:35" /></td>
        <td><input type="button" value="1" onclick="KeyPress('1');" class="header" style="width:35; height:35" /></td>
        <td><input type="button" value="2" onclick="KeyPress('2');" class="header" style="width:35; height:35" /></td>
        <td><input type="button" value="3" onclick="KeyPress('3');" class="header" style="width:35; height:35" /></td>
        <td><input type="button" value="-" onclick="Subtract();" class="header" style="width:35; height:35" /></td>
    </tr>
    <tr>
        <td><input type="button" value="MS" onclick="MemStore();" title="Memory Store" class="header" style="width:35; height:35" /></td>
        <td><input type="button" value="0" onclick="KeyPress('0');" class="header" style="width:35; height:35" /></td>
        <td><input type="button" value="." onclick="Decimal();" class="header" style="width:35; height:35" /></td>
        <td><input type="button" value="+" onclick="Add();" class="header" style="width:35; height:35" /></td>
        <td><input type="button" value="=" onclick="Equals();" class="header" style="width:35; height:35" /></td>
    </tr>
    <tr>
        <td><input type="button" value="MR" id="btnRecall" onclick="MemRecall();" title="Memory Recall" class="header" style="width:35; height:35" /></td>
        <td colspan="2"><input type="button" value="Clear" onclick="Clear();" class="header" style="width:75; height:35" /></td>
        <td colspan="2"><input type="button" value="Save" id="btnSave" onclick="Save();" class="header" style="width:75; height:35" /></td>
    </tr>
</table>
</asp:Content>
