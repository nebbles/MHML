using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
	internal abstract class NativeAlertDialogInterfaceBase : NativeInterfaceObject, INativeAlertDialogInterface 
	{
		#region INativeAlertDialogInterface implementation

		protected ButtonClickCallback onButtonClickEvent;
		public event ButtonClickCallback onButtonClick
		{
			add
			{
				onButtonClickEvent	+= value;
			}
			remove
			{
				onButtonClickEvent	-= value;
			}
		}

		public abstract void SetTitle(string value);

        public abstract string GetTitle();

        public abstract void SetMessage(string value);

        public abstract string GetMessage();

        public abstract void AddButton(string text, bool isCancelType);

        public abstract void Show();
		
        public abstract void Dismiss();

		#endregion
	}
}