using System;
using System.Collections.Generic;
using System.Text;

namespace NCC.ClearView.Application.UI.Entities
{
    public class UIControl
    {
        #region Constructor/Instance
            // <summary>
            // Constructor
            // </summary>
            public UIControl()
            {
            }
        #endregion
        #region Properties


        private long lngControlId;
        public long ControlId
        {
            get { return lngControlId; }
            set { lngControlId = value; }
        }

        private long lngWebPageId;
        public long WebPageId
        {
            get { return lngWebPageId; }
            set { lngWebPageId = value; }
        }

        private long lngWebUserControlId;
        public long WebUserControlId
        {
            get { return lngWebUserControlId; }
            set { lngWebUserControlId = value; }
        }

        private string strLabelName;
        public string LabelName
        {
            get { return strLabelName; }
            set { strLabelName = value; }
        }

        private string strLabelText;
        public string LabelText
        {
            get { return strLabelText; }
            set { strLabelText = value; }
        }

        private string strShortName;
        public string ShortName
        {
            get { return strShortName; }
            set { strShortName = value; }
        }

        private string strControlName;
        public string ControlName
        {
            get { return strControlName; }
            set { strControlName = value; }
        }


        private string strTitle;
        public string Title
        {
            get { return strTitle; }
            set { strTitle = value; }
        }

        private UIControlDataType oControlDataType;
        public UIControlDataType ControlDataType
        {
            get { return oControlDataType; }
            set { oControlDataType = value; }
        }

        private string strToolTipText;
        public string ToolTipText
        {
            get { return strToolTipText; }
            set { strToolTipText = value; }
        }

        private string strHTMLHelp;
        public string HTMLHelp
        {
            get { return strHTMLHelp; }
            set { strHTMLHelp = value; }
        }

        # region Validation
        private int intValidationRequired;
        public int ValidationRequired
        {
            get { return intValidationRequired; }
            set { intValidationRequired = value; }
        }

        private UIControlValidationRegularExp oUIControlValidationRegularExp;
        public UIControlValidationRegularExp ValidationRegularExp
        {
            get { return oUIControlValidationRegularExp; }
            set { oUIControlValidationRegularExp = value; }
        }

        private string strValidationCompareControl;
        public string ValidationCompareControl
        {
            get { return strValidationCompareControl; }
            set { strValidationCompareControl = value; }
        }

        private long lngValidationMinLen;
        public long ValidationMinLen
        {
            get { return lngValidationMinLen; }
            set { lngValidationMinLen = value; }
        }

        private long lngValidationMaxLen;
        public long ValidationMaxLen
        {
            get { return lngValidationMaxLen; }
            set { lngValidationMaxLen = value; }
        }


        private string strValidationRangeFrom;
        public string ValidationRangeFrom
        {
            get { return strValidationRangeFrom; }
            set { strValidationRangeFrom = value; }
        }

        private string strValidationRangeTo;
        public string ValidationRangeTo
        {
            get { return strValidationRangeTo; }
            set { strValidationRangeTo = value; }
        }

        private string strValidationCustomClientSideFunction;
        public string ValidationCustomClientSideFunction
        {
            get { return strValidationCustomClientSideFunction; }
            set { strValidationCustomClientSideFunction = value; }
        }

        private string strValidationCustomServerSideFunction;
        public string ValidationCustomServerSideFunction
        {
            get { return strValidationCustomServerSideFunction; }
            set { strValidationCustomServerSideFunction = value; }
        }

        private string strValidationMsg;
        public string ValidationMsg
        {
            get { return strValidationMsg; }
            set { strValidationMsg = value; }
        }

        #endregion

        private long lngDisplayOrder;
        public long DisplayOrder
        {
            get { return lngDisplayOrder; }
            set { lngDisplayOrder = value; }
        }

        private DateTime dtCreated;
        public DateTime Created
        {
            get { return dtCreated; }
            set { dtCreated = value; }
        }

        private long lngCreatedBy;
        public long CreatedBy
        {
            get { return lngCreatedBy; }
            set { lngCreatedBy = value; }
        }

        private DateTime dtModified;
        public DateTime Modified
        {
            get { return dtModified; }
            set { dtModified = value; }
        }

        private long lngModifiedBy;
        public long ModifiedBy
        {
            get { return lngModifiedBy; }
            set { lngModifiedBy = value; }
        }

        private int intDeleted;
        public int Deleted
        {
            get { return intDeleted; }
            set { intDeleted = value; }
        }



        #endregion
    }
}
