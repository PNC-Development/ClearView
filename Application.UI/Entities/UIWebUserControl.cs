using System;
using System.Collections.Generic;
using System.Text;

namespace NCC.ClearView.Application.UI.Entities
{
    public class UIWebUserControl
    {
        #region Constructor/Instance
            // <summary>
            // Constructor
            // </summary>
            public UIWebUserControl()
            {
            }
        #endregion
        #region Properties


        private long lngWebUserControlId;
        public long WebUserControlId
        {
            get { return lngWebUserControlId; }
            set { lngWebUserControlId = value; }
        }

        private string strPath;
        public string Path
        {
            get { return strPath; }
            set { strPath = value; }
        }

        private string strTitle;
        public string Title
        {
            get { return strTitle; }
            set { strTitle = value; }
        }

        private string strHTMLHelp;
        public string HTMLHelp
        {
            get { return strHTMLHelp; }
            set { strHTMLHelp = value; }
        }

        private List<UIControl> lstControls = new List<UIControl>();
        public List<UIControl> ControlList
        {
            get { return lstControls; }
            set { lstControls = value; }
        }

        private long lngDisplayOrder;
        public long DisplayOrder
        {
            get { return lngDisplayOrder; }
            set { lngDisplayOrder = value; }
        }

        private int intEnabled;
        public int Enabled
        {
            get { return intEnabled; }
            set { intEnabled = value; }
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
