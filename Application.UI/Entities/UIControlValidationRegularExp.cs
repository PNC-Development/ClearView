using System;
using System.Collections.Generic;
using System.Text;

namespace NCC.ClearView.Application.UI.Entities
{
    public class UIControlValidationRegularExp
    {
        #region Constructor/Instance
            // <summary>
            // Constructor
            // </summary>
            public UIControlValidationRegularExp()
            {
            }
        #endregion
        #region Properties


        private int intRegularExpId;
        public int RegularExpId
        {
            get { return intRegularExpId; }
            set { intRegularExpId = value; }
        }

        private string strName;
        public string Name
        {
            get { return strName; }
            set { strName = value; }
        }

        private string strRegularExp;
        public string RegularExp
        {
            get { return strRegularExp; }
            set { strRegularExp = value; }
        }

        private string strDefaultMsg;
        public string DefaultMsg
        {
            get { return strDefaultMsg; }
            set { strDefaultMsg = value; }
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
