using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; // Required when Using UI elements.

public class sliderValues : MonoBehaviour
{
	public Slider productivity;
	public Slider stress; 
	public Slider fatigue;
	public Slider anxiety;


	//Invoked when a submit button is clicked.
	public void SubmitSliderSetting()
	{
		//Displays the value of the slider in the console.
		Debug.Log(anxiety.value);
		JsonUtility.ToJson(this);

	}
}