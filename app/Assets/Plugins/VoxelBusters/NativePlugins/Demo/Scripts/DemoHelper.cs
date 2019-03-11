using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelBusters.NativePlugins.Demo
{
	public static class DemoHelper 
	{
		#region Static methods

		public static string[] GetMultivaluedString(InputField inputField)
		{
			return inputField.text.Split(',');
		}

		public static string GetDummyBody()
		{
			return "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
		}

		#endregion
	}
}
