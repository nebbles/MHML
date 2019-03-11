using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelBusters.NativePlugins.Demo
{
	public enum AddressBookDemoActionType
		{
			GetAuthStatus,
			ReadContacts,
			ResourcePage,
		}

	[RequireComponent(typeof(Button))]
	public class AddressBookDemoActionButton : DemoActionButton<AddressBookDemoActionType> 
	{}
}