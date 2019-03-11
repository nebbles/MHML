using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelBusters.NativePlugins.Demo
{
	public enum AlertDialogDemoActionType
	{
		New,
		SetTitle,
		GetTitle,
		SetMessage,
		GetMessage,
		AddButton,
		AddCancelButton,
		Show,
		ResourcePage,
	}

	[RequireComponent(typeof(Button))]
	public class AlertDialogDemoActionButton : DemoActionButton<AlertDialogDemoActionType> 
	{}
}