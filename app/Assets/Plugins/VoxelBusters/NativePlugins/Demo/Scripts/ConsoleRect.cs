using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelBusters.NativePlugins.Demo
{
	public class ConsoleRect : MonoBehaviour 
	{
		#region Properties

		[SerializeField]
		private 	Text 	m_debugText;

		#endregion

		#region Unity methods

		private void Awake()
		{
			Reset();
		}

		#endregion

		#region Public methods

		public void Log(string message, bool append)
		{
			if (append)
			{
                m_debugText.text    = message + "\n" + m_debugText.text;
			}
			else
			{
				m_debugText.text    = message;
			}
		}

		#endregion

		#region UI callback methods

		private void Reset()
		{
			if (m_debugText)
			{
				m_debugText.text	= "Console";
			}
		}

		#endregion
	}

}
