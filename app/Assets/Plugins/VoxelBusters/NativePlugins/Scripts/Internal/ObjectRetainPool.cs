using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Internal
{
	internal static class ObjectRetainPool 
	{
		#region Static fields

		private	static		List<object>		m_objectList = new List<object>(capacity: 8);

		#endregion

		#region Static methods

		public static void RetainObject(this object obj)
		{
			if (null == obj)
			{
				throw ErrorCentre.ArgumentNullException("obj");
			}

			m_objectList.Add(obj);
		}

		public static bool ReleaseObject(this object obj)
		{
			if (null == obj)
			{
				throw ErrorCentre.ArgumentNullException("obj");
			}

			return m_objectList.Remove(obj);
		}

		#endregion
	}
}