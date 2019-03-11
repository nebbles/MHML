using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelBusters.NativePlugins.Demo
{
	public enum RateMyAppDemoActionType
    {
        AskForReviewNow,
        ResourcePage
    }

	[RequireComponent(typeof(Button))]
    public class RateMyAppDemoActionButton : DemoActionButton<RateMyAppDemoActionType> 
    {}
}