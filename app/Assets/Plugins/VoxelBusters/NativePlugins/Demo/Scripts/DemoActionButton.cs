using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelBusters.NativePlugins.Demo
{
	public class DemoActionButton<TActionType> : MonoBehaviour where TActionType : struct, System.IConvertible
	{
		#region Fields

		[SerializeField]
		private		TActionType 		m_actionType	= default(TActionType);

		#endregion

		#region Properties

		public Button Button
		{
			get;
			private set;
		}

		public TActionType ActionType
		{
			get
			{
				return m_actionType;
			}
		}

		#endregion

		#region Unity methods

		private void Awake() 
		{
			// cache component
			Button	= GetComponent<Button>();	
		}

		#endregion
	}
}
