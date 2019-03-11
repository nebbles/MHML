using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Demo
{
	public class RateMyAppDemo : DemoBase<RateMyAppDemoActionButton, RateMyAppDemoActionType>
	{
		#region Base methods

        protected override void OnButtonPress(RateMyAppDemoActionButton selectedButton)
        {
            switch (selectedButton.ActionType)
            {
                case RateMyAppDemoActionType.AskForReviewNow:
					Log("Asking for review."); 
                    RateMyApp.AskForReviewNow();
                    break;

                case RateMyAppDemoActionType.ResourcePage:
                    Application.OpenURL(Internal.Constants.kRateMyAppResourcePage);
                    break;

                default:
                    break;
            }
        }

        #endregion
	}
}