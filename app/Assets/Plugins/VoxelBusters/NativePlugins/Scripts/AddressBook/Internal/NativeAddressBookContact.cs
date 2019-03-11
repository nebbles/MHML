using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using VoxelBusters.NativePlugins.Internal;

namespace VoxelBusters.NativePlugins
{
    internal class NativeAddressBookContact : IAddressBookContact
    {
        #region Fields

        private     NativeAddressBookContactData    m_nativeData;
        private     ILoadImageCallbackResult        m_cachedLoadImageResult;

        #endregion

        #region Constructors

        public NativeAddressBookContact(NativeAddressBookContactData data)
        {
            // set properties
            m_nativeData            = data;
            m_cachedLoadImageResult = null;

            if (IntPtr.Zero != m_nativeData.FirstNamePtr)
            {
                FirstName   = Marshal.PtrToStringAuto(data.FirstNamePtr);
            }
            if (IntPtr.Zero != m_nativeData.LastNamePtr)
            {
                LastName    = Marshal.PtrToStringAuto(data.LastNamePtr);
            }
        }

        #endregion

        #region System.object methods

        public override string ToString()
        {
            return string.Format("First name: {0} Last name: {1}", FirstName, LastName);
        }

        #endregion

        #region IAddressBookContact implementation

        public string FirstName
        {
            get; 
            private set;
        }

        public string MiddleName
        {
            get 
            {
                if (IntPtr.Zero == m_nativeData.MiddleNamePtr)
                {
                    return null;
                }
                
                return Marshal.PtrToStringAuto(m_nativeData.MiddleNamePtr);
            }
        }

        public string LastName
        {
            get; 
            private set;
        }

        public string[] PhoneNumbers
        {
            get 
            {
                return MarshalUtility.UnmanagedArrayToStringArray(m_nativeData.PhoneNumbersPtr, m_nativeData.PhoneNumbersCount);
            }
        }

        public string[] EmailAddresses
        {
            get 
            {
                return MarshalUtility.UnmanagedArrayToStringArray(m_nativeData.EmailAddressesPtr, m_nativeData.EmailAddressesCount);
            }
        }

        public void LoadImage(GenericCallback<ILoadImageCallbackResult> callback)
        {
            if (null == callback)
            {
                throw ErrorCentre.ArgumentNullException("callback");
            }

            // check whether image data exists
            if (null == m_cachedLoadImageResult)
            {
                LoadImageInternal(callback);
            }
            else
            {
                callback(m_cachedLoadImageResult);
            }
        }

        private void LoadImageInternal(GenericCallback<ILoadImageCallbackResult> callback)
        {
            // send default texture data as initial data
            LoadImageCallbackResult proxyResult = new LoadImageCallbackResult(AddressBook.defaultImage, null);
            if (callback != null)
            {
                callback(proxyResult);
            }

            // fetch actual data
#if UNITY_IOS
            IOSNativeCommonOperations.LoadImage(m_nativeData.ImageDataPtr, (result) => 
            {
                m_cachedLoadImageResult = result;
                callback(result);
            });
#endif
        }

        #endregion
    }
}