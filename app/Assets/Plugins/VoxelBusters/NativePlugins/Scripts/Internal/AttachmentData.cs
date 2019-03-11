using System.Collections;
using System.Collections.Generic;
using System;

namespace VoxelBusters.NativePlugins
{
    internal struct AttachmentData
    {
        #region Properties

        public int DataArrayLength
        {
            get;
            set;
        }

        public IntPtr DataArrayPtr
        {
            get;
            set;
        }

        public IntPtr MimeTypePtr
        {
            get;
            set;
        }

        public IntPtr FileNamePtr
        {
            get;
            set;
        }

        #endregion
    }
}