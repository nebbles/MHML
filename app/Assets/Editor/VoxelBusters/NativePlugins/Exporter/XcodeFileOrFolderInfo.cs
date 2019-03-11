using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
	[Serializable]
	public class XcodeFileOrFolderInfo
	{
		#region Fields

		[SerializeField]
		private			string			m_path;
		[SerializeField]
		private			string			m_compileFlags;

		#endregion

		#region Properties

		public string Path
		{
			get
			{
				return m_path;
			}
			set
			{
				m_path = value;
			}
		}

		public string[] CompileFlags
		{
			get
			{
				return m_compileFlags.Split(',');
			}
			set
			{
				m_compileFlags = string.Join(",", value);
			}
		}

		#endregion
	}
}