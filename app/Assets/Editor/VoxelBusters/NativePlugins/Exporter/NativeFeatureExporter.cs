using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
	[Serializable]
	public class NativeFeatureExporter : ScriptableObject
	{
		#region Fields

		[SerializeField]
		private			bool			m_isEnabled			= true;
		[SerializeField]
		private			iOSSettings		m_iOSSettings		= new iOSSettings();

		#endregion

		#region Properties

		public bool IsEnabled
		{
			get 
			{ 
				return m_isEnabled; 
			}
			set 
			{ 
				m_isEnabled = value; 
			}
		}

		public iOSSettings iOS
		{
			get 
			{ 
				return m_iOSSettings; 
			}
			set 
			{ 
				m_iOSSettings = value; 
			}
		}

		#endregion

		#region Nested types

		[Serializable]
		public class iOSSettings
		{
			#region Fields

			[SerializeField]
			private			XcodeFileOrFolderInfo[]		m_files				= null;
			[SerializeField]
			private			XcodeFileOrFolderInfo[]		m_folders			= null;
			[SerializeField]
			private			XcodeFrameworkInfo[]		m_frameworks		= null;
			[SerializeField]
			private			XcodeCapabilityType[]		m_capabilities		= null;

			#endregion

			#region Properties

			public XcodeFileOrFolderInfo[] Files
			{
				get 
				{ 
					return m_files; 
				}
				set 
				{ 
					m_files = value; 
				}
			}

			public XcodeFileOrFolderInfo[] Folders
			{
				get 
				{ 
					return m_folders; 
				}
				set 
				{ 
					m_folders = value; 
				}
			}

			public XcodeFrameworkInfo[] Frameworks
			{
				get 
				{ 
					return m_frameworks; 
				}
				set 
				{ 
					m_frameworks = value; 
				}
			}

			public XcodeCapabilityType[] Capabilities
			{
				get 
				{ 
					return m_capabilities; 
				}
				set 
				{ 
					m_capabilities = value; 
				}
			}

			#endregion
		}

		#endregion
	}
}
