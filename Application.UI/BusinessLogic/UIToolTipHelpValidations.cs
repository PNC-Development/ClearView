using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCC.ClearView.Application.UI.Entities;
using NCC.ClearView.Application.UI.DataAccess;
namespace NCC.ClearView.Application.UI.BusinessLogic
{
    public class UIToolTipHelpValidations 
    {
      

        public static void LoadToolTipHelpValidations(ref System.Web.UI.Page oPage)
        {
            if (oPage != null)
            {
                UIFactory oUIFactory = new UIFactory();
                UIWebPage oUIWebPage = oUIFactory.GetUIWebPage(oPage.AppRelativeVirtualPath.Substring(1));
                ContentPlaceHolder oPlaceHolder = (ContentPlaceHolder)oPage.Master.FindControl("AllContent");

                if (oPlaceHolder != null)
                {
                    foreach (UIControl oUIControl in oUIWebPage.ControlList)
                    {
                        //Find the Label Control to Add Tooltip and Help
                        Control oLabelControl = null;
                        oLabelControl = oPlaceHolder.FindControl(oUIControl.LabelName);
                        if (oLabelControl != null)
                        {
                            Label lbl = (Label)oLabelControl;
                            //lbl.ToolTip = oUIControl.ToolTipText;
                            //Check if already added the help
                            if (lbl.Text.Contains("ShowHelpWindow") == false)
                            {
                                string strHelpPopup = "";
                                if (oUIControl.HTMLHelp != "")
                                    strHelpPopup = " <a href=\"javascript:void(0);\" onclick=\"ShowHelpWindow('" + oUIControl.ControlId + "');\"><img src=\"/images/help_control.gif\" border=\"0\" align=\"absmiddle\"/></a>";
                                lbl.Text = lbl.Text + strHelpPopup;
                                //lbl.Text = oUIControl.LabelText+ strHelpPopup;
                            }
                        }
                        //Find the Primary Control to add validations
                        if (oUIControl.ControlName != "")
                        {
                            Control oPrimaryControl = oPlaceHolder.FindControl(oUIControl.ControlName);
                            if (oPrimaryControl != null)
                            {
                                ApplyToolTip(ref oPrimaryControl, oUIControl);
                                if (ConfigurationManager.AppSettings["CONTROLS_ADD_VALIDATIONS_FROM_DB"].ToString() == "1")
                                    ApplyValidations(ref oPrimaryControl, ref oPlaceHolder, oUIControl);
                            }
                        }

                    }
                }

                //Loop through Page controls to check for User controls
                foreach (Control oControl in oPage.Controls)
                {
                    Control oTmp = new Control();
                    oTmp = oControl;
                    FindUserControlRecurrsive(ref oTmp);
                }
            }

        }

        public static void LoadToolTipHelpValidationsForUserControl(ref System.Web.UI.UserControl oUserControl)
        {
            if (oUserControl != null)
            {
                UIFactory oUIFactory = new UIFactory();
                UIWebUserControl oUIWebUserControl = oUIFactory.GetUIWebUserControl(oUserControl.AppRelativeVirtualPath.Substring(1));
                foreach (UIControl oUIControl in oUIWebUserControl.ControlList)
                {
                    //Find the Label Control to Add Tooltip and Help
                    Control oLabelControl = null;
                    oLabelControl = oUserControl.FindControl(oUIControl.LabelName);
                    if (oLabelControl != null)
                    {
                        Label lbl = (Label)oLabelControl;
                        //lbl.ToolTip = oUIControl.ToolTipText;
                        //Check if already added the help
                        if (lbl.Text.Contains("ShowHelpWindow") == false)
                        {
                            string strHelpPopup = "";
                            if (oUIControl.HTMLHelp != "")
                                strHelpPopup = " <a href=\"javascript:void(0);\" onclick=\"ShowHelpWindow('" + oUIControl.ControlId + "');\"><img src=\"/images/help_control.gif\" border=\"0\" align=\"absmiddle\"/></a>";
                            lbl.Text = lbl.Text + strHelpPopup;
                            //lbl.Text = oUIControl.LabelText+ strHelpPopup;
                        }
                    }

                    //Find the Primary Control to add validations
                    if (oUIControl.ControlName != "")
                    {
                        Control oPrimaryControl = oUserControl.FindControl(oUIControl.ControlName);
                        if (oPrimaryControl != null)
                        {
                            ApplyToolTip(ref oPrimaryControl, oUIControl);
                            if (ConfigurationManager.AppSettings["CONTROLS_ADD_VALIDATIONS_FROM_DB"].ToString() == "1")
                                ApplyValidations(ref oPrimaryControl, ref oUserControl, oUIControl);
                        }

                    }
                }
                //Loop through UserControl controls to check for more User controls
                foreach (Control oControl in oUserControl.Controls)
                {
                    Control oTmp = new Control();
                    oTmp = oControl;
                    FindUserControlRecurrsive(ref oTmp);
                }
            }

        }

        private static void FindUserControlRecurrsive(ref Control oControl)
        {
            foreach (Control oTmpControl in oControl.Controls)
            {
                try
                {
                    if (oTmpControl is UserControl)
                    {
                        UserControl oUserControl = (UserControl)oTmpControl;
                        if (oUserControl.AppRelativeVirtualPath.Contains(".ascx")) //Additional check for user control
                            LoadToolTipHelpValidationsForUserControl(ref oUserControl);

                    }

                    //System.Diagnostics.Debug.Writeline(oTmpControl.GetType().BaseType.ToString());
                    //System.Diagnostics.Debug.("Parent Control " + oControl.ID + " *** Child Control *** " + oTmpControl.ID + "-" + ((System.Type)oTmpControl.GetType()).Name + "<br>");
                    if (oTmpControl.HasControls())
                    {
                        Control oTmp = new Control();
                        oTmp = oTmpControl;
                        FindUserControlRecurrsive(ref oTmp);
                    }

                }
                catch
                {
                }
            }

        }

        private static void ApplyToolTip(ref Control oPrimaryControl, UIControl oUIControl)
        {
            System.Type oControlType = oPrimaryControl.GetType();
            switch (oControlType.Name.ToUpper())
            {
                case "TEXTBOX":
                    TextBox tb = (TextBox)oPrimaryControl;
                    tb.ToolTip = oUIControl.ToolTipText;
                    break;
                case "DROPDOWNLIST":
                    DropDownList ddl = (DropDownList)oPrimaryControl;
                    ddl.ToolTip = oUIControl.ToolTipText;
                    break;
                case "RADIOBUTTONLIST":
                    RadioButtonList oOptButtonList = (RadioButtonList)oPrimaryControl;
                    oOptButtonList.ToolTip = oUIControl.ToolTipText;
                    break;
                case "LISTBOX":
                    ListBox oListBox = (ListBox)oPrimaryControl;
                    oListBox.ToolTip = oUIControl.ToolTipText;
                    break;
                case "BUTTON":
                    Button btn = (Button)oPrimaryControl;
                    btn.ToolTip = oUIControl.ToolTipText;
                    break;
                case "LINKBUTTON":
                    LinkButton lbtn = (LinkButton)oPrimaryControl;
                    lbtn.ToolTip = oUIControl.ToolTipText;
                    break;
                case "CHECKBOX":
                    CheckBox chk = (CheckBox)oPrimaryControl;
                    chk.ToolTip = oUIControl.ToolTipText;
                    break;
                case "RADIOBUTTON":
                    RadioButton optBtn = (RadioButton)oPrimaryControl;
                    optBtn.ToolTip = oUIControl.ToolTipText;
                    break;
            };
        
        }

        private static void ApplyValidations(ref Control oControl, ref ContentPlaceHolder oParentControl, UIControl oUIControl)
        {

            if (oControl != null)
            {
                System.Type oControlType = oControl.GetType();

                switch (oControlType.Name.ToUpper())
                {
                    case "TEXTBOX":
                        TextBox tb = (TextBox)oControl;
                        tb.ToolTip = oUIControl.ToolTipText;

                        //Required field validator
                        if (oUIControl.ValidationRequired == 1)
                        {
                            if (oParentControl.FindControl("rfv" + oUIControl.ControlName) == null)
                            {
                                RequiredFieldValidator oRFV = new RequiredFieldValidator();
                                oRFV.ID = "rfv" + oUIControl.ControlName;
                                oRFV.ControlToValidate = oUIControl.ControlName;
                                oRFV.Display = ValidatorDisplay.None;

                                if (oUIControl.ValidationMsg!= "")
                                    oRFV.ErrorMessage = oUIControl.ValidationMsg;
                                else
                                    oRFV.ErrorMessage = oUIControl.ShortName+ " : " + "Please enter " + oUIControl.LabelText;

                                oParentControl.Controls.Add(oRFV);
                                oRFV.SetFocusOnError = true;
                            }
                        }
                        //Regular Expression Validation (if not exists add  OR if exists not required remove)
                        if (oUIControl.ValidationRegularExp != null && oUIControl.ValidationRegularExp.RegularExpId > 0)
                        {
                            if (oParentControl.FindControl("rev" + oUIControl.ControlName) == null)
                            {
                                RegularExpressionValidator oREV = new RegularExpressionValidator();
                                oREV.ID = "rev" + oUIControl.ControlName;
                                oREV.ControlToValidate = oUIControl.ControlName;
                                oREV.ValidationExpression = oUIControl.ValidationRegularExp.RegularExp;
                                oREV.Display = ValidatorDisplay.None;

                                oREV.ErrorMessage = oUIControl.ShortName + " : " + oUIControl.ValidationRegularExp.DefaultMsg;

                                oParentControl.Controls.Add(oREV);
                                oREV.SetFocusOnError = true;
                            }
                        }
                        //Range Validation
                        if (oUIControl.ValidationRangeFrom != "" && oUIControl.ValidationRangeTo != "")
                        {
                            if (oParentControl.FindControl("rv" + oUIControl.ControlName) == null)
                            {
                                RangeValidator oRV = new RangeValidator();
                                oRV.ID = "rv" + oUIControl.ControlName;
                                oRV.ControlToValidate = oUIControl.ControlName;
                                oRV.MinimumValue = oUIControl.ValidationRangeFrom.ToString();
                                oRV.MaximumValue = oUIControl.ValidationRangeTo.ToString();
                                if (oUIControl.ControlDataType.DataTypeId == 1) //String
                                    oRV.Type = ValidationDataType.String;
                                else if (oUIControl.ControlDataType.DataTypeId == 2) //Integer
                                    oRV.Type = ValidationDataType.Integer;
                                else if (oUIControl.ControlDataType.DataTypeId == 3)//Double
                                    oRV.Type = ValidationDataType.Double;
                                else if (oUIControl.ControlDataType.DataTypeId == 4)//Currency
                                    oRV.Type = ValidationDataType.Currency;
                                else if (oUIControl.ControlDataType.DataTypeId == 5)//Date
                                    oRV.Type = ValidationDataType.Date;

                                oRV.Display = ValidatorDisplay.None;

                                oRV.ErrorMessage = oUIControl.ShortName + " : " + "Please enter " + oUIControl.ShortName + " value from " + oUIControl.ValidationRangeFrom.ToString() + " to " + oUIControl.ValidationRangeTo.ToString();
                                
                                oParentControl.Controls.Add(oRV);
                                oRV.SetFocusOnError = true;
                            }
                        }
                        //Length Validation
                        if (oUIControl.ValidationMinLen >0  &&  oUIControl.ValidationMinLen > 0)
                        {
                            if (oParentControl.FindControl("revlen" + oUIControl.ControlName) == null)
                            {
                                RegularExpressionValidator oREV = new RegularExpressionValidator();
                                oREV.ID = "revlen" + oUIControl.ControlName;
                                oREV.ControlToValidate = oUIControl.ControlName;
                                oREV.ValidationExpression = "^.{"+oUIControl.ValidationMinLen+","+oUIControl.ValidationMaxLen+"}$";
                                oREV.Display = ValidatorDisplay.None;
                                oREV.ErrorMessage = oUIControl.ShortName + " : " + "It must be between " + oUIControl.ValidationMinLen.ToString() + " and " + oUIControl.ValidationMaxLen.ToString() +" characters";
                                oParentControl.Controls.Add(oREV);
                                oREV.SetFocusOnError = true;
                            }
                        }

                         //Client Valdiation Using Client Side Validation
                        if (oUIControl.ValidationCustomClientSideFunction !="" )
                        {
                            if (oParentControl.FindControl("customValClientSide" + oUIControl.ControlName) == null)
                            {
                                CustomValidator ocustomValClientSide = new CustomValidator();
                                ocustomValClientSide.ID = "customValClientSide" + oUIControl.ControlName;
                                ocustomValClientSide.ControlToValidate = oUIControl.ControlName;
                                ocustomValClientSide.ClientValidationFunction = oUIControl.ValidationCustomClientSideFunction;
                                ocustomValClientSide.Display = ValidatorDisplay.None;
                                ocustomValClientSide.ErrorMessage = oUIControl.ShortName + " : " + oUIControl.ValidationMsg;
                                oParentControl.Controls.Add(ocustomValClientSide);
                                ocustomValClientSide.SetFocusOnError = true;
                            }
                        }

                        ////Custom Valdiation Using Server Side Validation -Not working for onServerValidate
                        //if (oUIControl.ValidationCustomServerSideFunction != "")
                        //{
                        //    if (oParentControl.FindControl("customValServerSide" + oUIControl.ControlName) == null)
                        //    {
                        //        CustomValidator ocustomValServerSide = new CustomValidator();
                        //        ocustomValServerSide.ID = "customValServerSide" + oUIControl.ControlName;
                        //        ocustomValServerSide.ControlToValidate = oUIControl.ControlName;
                        //        ocustomValServerSide.onServerValidate = oUIControl.ValidationCustomServerSideFunction;
                        //        ocustomValServerSide.Display = ValidatorDisplay.None;
                        //        ocustomValServerSide.ErrorMessage = oUIControl.ShortName + " : " + oUIControl.ValidationMsg;
                        //        oParentControl.Controls.Add(ocustomValServerSide);
                        //        ocustomValServerSide.SetFocusOnError = true;
                        //    }
                        //}

                        //Compare Validation
                        if (oUIControl.ValidationCompareControl != "" )
                        {
                            if (oParentControl.FindControl("compVal" + oUIControl.ControlName) == null)
                            {
                                CompareValidator oCompareValidator = new CompareValidator();
                                oCompareValidator.ID = "compVal" + oUIControl.ControlName;
                                oCompareValidator.ControlToValidate = oUIControl.ControlName;
                                oCompareValidator.ControlToCompare = oUIControl.ValidationCompareControl;
                                oCompareValidator.Display = ValidatorDisplay.None;
                                oCompareValidator.ErrorMessage = oUIControl.ShortName + " : " + oUIControl.ValidationMsg;
                                oParentControl.Controls.Add(oCompareValidator);
                                oCompareValidator.SetFocusOnError = true;
                            }
                        }
                        
                        break;

                    case "DROPDOWNLIST":
                        DropDownList ddl = (DropDownList)oControl;
                        ddl.ToolTip = oUIControl.ToolTipText;
                        if (oUIControl.ValidationRequired == 1)
                        {
                            if (oParentControl.FindControl("rfv" + oUIControl.ControlName) == null)
                            {
                                RequiredFieldValidator oRFV = new RequiredFieldValidator();
                                oRFV.ID = "rfv" + oUIControl.ControlName;
                                oRFV.ControlToValidate = oUIControl.ControlName;
                                oRFV.Display = ValidatorDisplay.None;
                                oRFV.InitialValue = "-1";
                                //oRFV.ErrorMessage = oUIControl.ValidationMsg;
                                oRFV.ErrorMessage = oUIControl.ShortName + " : " + "Please select " + oUIControl.LabelText;
                                oParentControl.Controls.Add(oRFV);
                                oRFV.SetFocusOnError = true;
                            }
                        }
                        break;

                    case "RADIOBUTTONLIST":
                        RadioButtonList oOptButtonList = (RadioButtonList)oControl;
                        oOptButtonList.ToolTip = oUIControl.ToolTipText;
                        if (oUIControl.ValidationRequired == 1)
                        {
                            if (oParentControl.FindControl("rfv" + oUIControl.ControlName) == null)
                            {
                                RequiredFieldValidator oRFV = new RequiredFieldValidator();
                                oRFV.ID = "rfv" + oUIControl.ControlName;
                                oRFV.ControlToValidate = oUIControl.ControlName;
                                oRFV.Display = ValidatorDisplay.None;
                                oRFV.InitialValue = "";
                                if (oUIControl.ValidationMsg=="")
                                    oRFV.ErrorMessage = oUIControl.ShortName + " : " + "Please select " + oUIControl.LabelText;
                                else
                                    oRFV.ErrorMessage = oUIControl.ValidationMsg;

                                oParentControl.Controls.Add(oRFV);
                                oRFV.SetFocusOnError = true;
                            }
                        }
                        break;

                    case "LISTBOX":
                        ListBox oListBox = (ListBox)oControl;
                        oListBox.ToolTip = oUIControl.ToolTipText;
                        if (oUIControl.ValidationRequired == 1)
                        {
                            if (oParentControl.FindControl("rfv" + oUIControl.ControlName) == null)
                            {
                                RequiredFieldValidator oRFV = new RequiredFieldValidator();
                                oRFV.ID = "rfv" + oUIControl.ControlName;
                                oRFV.ControlToValidate = oUIControl.ControlName;
                                oRFV.Display = ValidatorDisplay.None;
                                oRFV.InitialValue = "-1";
                                if (oUIControl.ValidationMsg=="")
                                    oRFV.ErrorMessage = oUIControl.ShortName + " : " + "Please select " + oUIControl.LabelText;
                                else
                                    oRFV.ErrorMessage = oUIControl.ValidationMsg;

                                oParentControl.Controls.Add(oRFV);
                                oRFV.SetFocusOnError = true;
                            }
                        }
                        
                        break;

                     case "BUTTON":
                        Button btn = (Button)oControl;
                        btn.ToolTip = oUIControl.ToolTipText;
                        //btn.Text = oUIControl.LabelText;
                        break;
                    case "LINKBUTTON":
                        LinkButton lbtn = (LinkButton)oControl;
                        lbtn.ToolTip = oUIControl.ToolTipText;
                        //lbtn.Text = oUIControl.LabelText;
                        break;
                    case "CHECKBOX":
                        CheckBox chk = (CheckBox)oControl;
                        chk.ToolTip = oUIControl.ToolTipText;
                        //chk.Text = oUIControl.LabelText;
                        break;
                    case "RADIOBUTTON":
                        RadioButton optBtn = (RadioButton)oControl;
                        optBtn.ToolTip = oUIControl.ToolTipText;
                        //optBtn.Text = oUIControl.LabelText;
                        break;
                };

            }

        }

        private static void ApplyValidations(ref Control oControl, ref UserControl oParentControl, UIControl oUIControl)
        {

            if (oControl != null)
            {
                System.Type oControlType = oControl.GetType();

                switch (oControlType.Name.ToUpper())
                {
                    case "TEXTBOX":
                        TextBox tb = (TextBox)oControl;
                        tb.ToolTip = oUIControl.ToolTipText;

                        //Required field validator
                        if (oUIControl.ValidationRequired == 1)
                        {
                            if (oParentControl.FindControl("rfv" + oUIControl.ControlName) == null)
                            {
                                RequiredFieldValidator oRFV = new RequiredFieldValidator();
                                oRFV.ID = "rfv" + oUIControl.ControlName;
                                oRFV.ControlToValidate = oUIControl.ControlName;
                                oRFV.Display = ValidatorDisplay.None;

                                if (oUIControl.ValidationMsg != "")
                                    oRFV.ErrorMessage = oUIControl.ValidationMsg;
                                else
                                    oRFV.ErrorMessage = oUIControl.ShortName + " : " + "Please enter " + oUIControl.LabelText;

                                oParentControl.Controls.Add(oRFV);
                                oRFV.SetFocusOnError = true;
                            }
                        }
                        //Regular Expression Validation (if not exists add  OR if exists not required remove)
                        if (oUIControl.ValidationRegularExp != null && oUIControl.ValidationRegularExp.RegularExpId > 0)
                        {
                            if (oParentControl.FindControl("rev" + oUIControl.ControlName) == null)
                            {
                                RegularExpressionValidator oREV = new RegularExpressionValidator();
                                oREV.ID = "rev" + oUIControl.ControlName;
                                oREV.ControlToValidate = oUIControl.ControlName;
                                oREV.ValidationExpression = oUIControl.ValidationRegularExp.RegularExp;
                                oREV.Display = ValidatorDisplay.None;

                                oREV.ErrorMessage = oUIControl.ShortName + " : " + oUIControl.ValidationRegularExp.DefaultMsg;

                                oParentControl.Controls.Add(oREV);
                                oREV.SetFocusOnError = true;
                            }
                        }
                        //Range Validation
                        if (oUIControl.ValidationRangeFrom != "" && oUIControl.ValidationRangeTo != "")
                        {
                            if (oParentControl.FindControl("rv" + oUIControl.ControlName) == null)
                            {
                                RangeValidator oRV = new RangeValidator();
                                oRV.ID = "rv" + oUIControl.ControlName;
                                oRV.ControlToValidate = oUIControl.ControlName;
                                oRV.MinimumValue = oUIControl.ValidationRangeFrom.ToString();
                                oRV.MaximumValue = oUIControl.ValidationRangeTo.ToString();
                                if (oUIControl.ControlDataType.DataTypeId == 1) //String
                                    oRV.Type = ValidationDataType.String;
                                else if (oUIControl.ControlDataType.DataTypeId == 2) //Integer
                                    oRV.Type = ValidationDataType.Integer;
                                else if (oUIControl.ControlDataType.DataTypeId == 3)//Double
                                    oRV.Type = ValidationDataType.Double;
                                else if (oUIControl.ControlDataType.DataTypeId == 4)//Currency
                                    oRV.Type = ValidationDataType.Currency;
                                else if (oUIControl.ControlDataType.DataTypeId == 5)//Date
                                    oRV.Type = ValidationDataType.Date;

                                oRV.Display = ValidatorDisplay.None;

                                oRV.ErrorMessage = oUIControl.ShortName + " : " + "Please enter " + oUIControl.ShortName + " value from " + oUIControl.ValidationRangeFrom.ToString() + " to " + oUIControl.ValidationRangeTo.ToString();

                                oParentControl.Controls.Add(oRV);
                                oRV.SetFocusOnError = true;
                            }
                        }
                        //Length Validation
                        if (oUIControl.ValidationMinLen > 0 && oUIControl.ValidationMinLen > 0)
                        {
                            if (oParentControl.FindControl("revlen" + oUIControl.ControlName) == null)
                            {
                                RegularExpressionValidator oREV = new RegularExpressionValidator();
                                oREV.ID = "revlen" + oUIControl.ControlName;
                                oREV.ControlToValidate = oUIControl.ControlName;
                                oREV.ValidationExpression = "^.{" + oUIControl.ValidationMinLen + "," + oUIControl.ValidationMaxLen + "}$";
                                oREV.Display = ValidatorDisplay.None;
                                oREV.ErrorMessage = oUIControl.ShortName + " : " + "It must be between " + oUIControl.ValidationMinLen.ToString() + " and " + oUIControl.ValidationMaxLen.ToString() + " characters";
                                oParentControl.Controls.Add(oREV);
                                oREV.SetFocusOnError = true;
                            }
                        }

                        //Client Valdiation Using Client Side Validation
                        if (oUIControl.ValidationCustomClientSideFunction != "")
                        {
                            if (oParentControl.FindControl("customValClientSide" + oUIControl.ControlName) == null)
                            {
                                CustomValidator ocustomValClientSide = new CustomValidator();
                                ocustomValClientSide.ID = "customValClientSide" + oUIControl.ControlName;
                                ocustomValClientSide.ControlToValidate = oUIControl.ControlName;
                                ocustomValClientSide.ClientValidationFunction = oUIControl.ValidationCustomClientSideFunction;
                                ocustomValClientSide.Display = ValidatorDisplay.None;
                                ocustomValClientSide.ErrorMessage = oUIControl.ShortName + " : " + oUIControl.ValidationMsg;
                                oParentControl.Controls.Add(ocustomValClientSide);
                                ocustomValClientSide.SetFocusOnError = true;
                            }
                        }

                        // //Custom Valdiation Using Server Side Validation -Not working for onServerValidate
                        //if (oUIControl.ValidationCustomServerSideFunction != "" )
                        //{
                        //    if (oParentControl.FindControl("customValServerSide" + oUIControl.ControlName) == null)
                        //    {
                        //        CustomValidator ocustomValServerSide = new CustomValidator();
                        //        ocustomValServerSide.ID = "customValServerSide" + oUIControl.ControlName;
                        //        ocustomValServerSide.ControlToValidate = oUIControl.ControlName;
                        //        ocustomValServerSide.onServerValidate = oUIControl.ValidationCustomServerSideFunction;
                        //        ocustomValServerSide.Display = ValidatorDisplay.None;
                        //        ocustomValServerSide.ErrorMessage = oUIControl.ShortName + " : " + oUIControl.ValidationMsg;
                        //        oParentControl.Controls.Add(ocustomValServerSide);
                        //        ocustomValServerSide.SetFocusOnError = true;
                        //    }
                        //}

                        //Compare Validation
                        if (oUIControl.ValidationCompareControl != "")
                        {
                            if (oParentControl.FindControl("compVal" + oUIControl.ControlName) == null)
                            {
                                CompareValidator oCompareValidator = new CompareValidator();
                                oCompareValidator.ID = "compVal" + oUIControl.ControlName;
                                oCompareValidator.ControlToValidate = oUIControl.ControlName;
                                oCompareValidator.ControlToCompare = oUIControl.ValidationCompareControl;
                                oCompareValidator.Display = ValidatorDisplay.None;
                                oCompareValidator.ErrorMessage = oUIControl.ShortName + " : " + oUIControl.ValidationMsg;
                                oParentControl.Controls.Add(oCompareValidator);
                                oCompareValidator.SetFocusOnError = true;
                            }
                        }

                        break;

                    case "DROPDOWNLIST":
                        DropDownList ddl = (DropDownList)oControl;
                        ddl.ToolTip = oUIControl.ToolTipText;
                        if (oUIControl.ValidationRequired == 1)
                        {
                            if (oParentControl.FindControl("rfv" + oUIControl.ControlName) == null)
                            {
                                RequiredFieldValidator oRFV = new RequiredFieldValidator();
                                oRFV.ID = "rfv" + oUIControl.ControlName;
                                oRFV.ControlToValidate = oUIControl.ControlName;
                                oRFV.Display = ValidatorDisplay.None;
                                oRFV.InitialValue = "-1";
                                //oRFV.ErrorMessage = oUIControl.ValidationMsg;
                                oRFV.ErrorMessage = oUIControl.ShortName + " : " + "Please select " + oUIControl.LabelText;
                                oParentControl.Controls.Add(oRFV);
                                oRFV.SetFocusOnError = true;
                            }
                        }
                        break;

                    case "RADIOBUTTONLIST":
                        RadioButtonList oOptButtonList = (RadioButtonList)oControl;
                        oOptButtonList.ToolTip = oUIControl.ToolTipText;
                        if (oUIControl.ValidationRequired == 1)
                        {
                            if (oParentControl.FindControl("rfv" + oUIControl.ControlName) == null)
                            {
                                RequiredFieldValidator oRFV = new RequiredFieldValidator();
                                oRFV.ID = "rfv" + oUIControl.ControlName;
                                oRFV.ControlToValidate = oUIControl.ControlName;
                                oRFV.Display = ValidatorDisplay.None;
                                oRFV.InitialValue = "";
                                if (oUIControl.ValidationMsg == "")
                                    oRFV.ErrorMessage = oUIControl.ShortName + " : " + "Please select " + oUIControl.LabelText;
                                else
                                    oRFV.ErrorMessage = oUIControl.ValidationMsg;

                                oParentControl.Controls.Add(oRFV);
                                oRFV.SetFocusOnError = true;
                            }
                        }
                        break;

                    case "LISTBOX":
                        ListBox oListBox = (ListBox)oControl;
                        oListBox.ToolTip = oUIControl.ToolTipText;
                        if (oUIControl.ValidationRequired == 1)
                        {
                            if (oParentControl.FindControl("rfv" + oUIControl.ControlName) == null)
                            {
                                RequiredFieldValidator oRFV = new RequiredFieldValidator();
                                oRFV.ID = "rfv" + oUIControl.ControlName;
                                oRFV.ControlToValidate = oUIControl.ControlName;
                                oRFV.Display = ValidatorDisplay.None;
                                oRFV.InitialValue = "-1";
                                if (oUIControl.ValidationMsg == "")
                                    oRFV.ErrorMessage = oUIControl.ShortName + " : " + "Please select " + oUIControl.LabelText;
                                else
                                    oRFV.ErrorMessage = oUIControl.ValidationMsg;

                                oParentControl.Controls.Add(oRFV);
                                oRFV.SetFocusOnError = true;
                            }
                        }

                        break;

                    case "BUTTON":
                        Button btn = (Button)oControl;
                        btn.ToolTip = oUIControl.ToolTipText;
                        //btn.Text = oUIControl.LabelText;
                        break;
                    case "LINKBUTTON":
                        LinkButton lbtn = (LinkButton)oControl;
                        lbtn.ToolTip = oUIControl.ToolTipText;
                        //lbtn.Text = oUIControl.LabelText;
                        break;
                    case "CHECKBOX":
                        CheckBox chk = (CheckBox)oControl;
                        chk.ToolTip = oUIControl.ToolTipText;
                        //chk.Text = oUIControl.LabelText;
                        break;
                    case "RADIOBUTTON":
                        RadioButton optBtn = (RadioButton)oControl;
                        optBtn.ToolTip = oUIControl.ToolTipText;
                        //optBtn.Text = oUIControl.LabelText;
                        break;
                };

            }
        }
    }
}
