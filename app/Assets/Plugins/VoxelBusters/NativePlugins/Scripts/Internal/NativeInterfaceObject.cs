using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
	internal class NativeInterfaceObject 
	{
		#region Fields

		private		bool		m_isInvalidated;	

		#endregion

		#region Constructors

		public NativeInterfaceObject()
		{
			// set properties
			m_isInvalidated	= false;
		}

		#endregion

		#region Public methods

		public void Invalidate()
		{
			if (m_isInvalidated)
			{
				throw ErrorCentre.ObjectInvalidatedException();
			}

            // mark as invalidated
#if NATIVE_PLUGINS_DEBUG
            Debug.Log("[NativePlugins] Invalidating native object.");
#endif
			m_isInvalidated	= true;
			InvalidateInternal();
		}

        #endregion

        #region Protected methods

		protected virtual void InvalidateInternal()
		{}

        #endregion
	}
}
