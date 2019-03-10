using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.


public class anxietyClass : MonoBehaviour {

	 
	public Slider productivity;
	public Slider stress; 
	public Slider fatigue;
	public Slider anxiety;
	public float heartRate;
	public float GSR; 


	// This is placeholder at the moment, but functions to be written
	public void exportPersonData() { //This is the function attached to a button in the app
		//exporting current person data as json
		//string jsonLogging = JsonUtility.ToJson(productivity, stress, fatigue, anxiety);
	}

	public void exportLoggingData() { //This is the function attached to a button in the app
		//packaging current logging data as JSON
		//string jsonLogging = JsonUtility.ToJson();
	}

	public void exportSensorData(){
		//packaging current heart rate & GSR data as JSON
		//string jsonLogging = JsonUtility.ToJson (heartRate, GSR); 
	}



}








