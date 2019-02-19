using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; // Required when Using UI elements.

public class sliderValues : MonoBehaviour
{
	public Slider Anxious;
	public Slider Stressed; 
	public Slider Tired;
	public Slider Productivity;



	//Invoked when a submit button is clicked.
	public void SubmitSliderSetting()
	{
		//Displays the value of the slider in the console.
		Debug.Log(Anxious.value);
		//return JsonUtility.ToJson(this);
	}
}