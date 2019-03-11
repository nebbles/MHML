using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
	[Serializable]
	public class XcodeFrameworkInfo
	{
		#region Fields

		[SerializeField]
		private			string			m_name		= string.Empty;
		[SerializeField]
		private			bool			m_isWeak	= false;

		#endregion

		#region Properties

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		public bool IsWeak
		{
			get
			{
				return m_isWeak;
			}
			set
			{
				m_isWeak = value;
			}
		}

		#endregion
	}
}