using System;
using System.Collections.Generic;
using System.Text;

namespace NCC.ClearView.Application.UI.Entities
{
    public class UIControlDataType
    {
        #region Constructor/Instance
            // <summary>
            // Constructor
            // </summary>
        public UIControlDataType()
            {
            }
        #endregion
        #region Properties


        private long lngDataTypeId;
        public long DataTypeId
        {
            get { return lngDataTypeId; }
            set { lngDataTypeId = value; }
        }

        private string strName;
        public string Name
        {
            get { return strName; }
            set { strName = value; }
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
