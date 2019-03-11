using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelBusters.NativePlugins.Demo
{
	public enum ShareSheetDemoActionType
	{
		New,
		AddText,
		AddScreenshot,
		AddImage,
		AddURL,
		Show,
		ResourcePage
	}

	[RequireComponent(typeof(Button))]
	public class ShareSheetDemoActionButton : DemoActionButton<ShareSheetDemoActionType> 
	{}
}
