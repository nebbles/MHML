using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

public class Session {
	public string session_id; //server expects str
	public string firmwareRevision;
	public string[] selfReported;
	public string[] ppg;
	public string[] gsr;


	public Slider productivity;
	public Slider stress; 
	public Slider fatigue;
	public Slider anxiety;

	public float heartRate;
	public float GSR; 


	public void resetSliders(){
		productivity.value = 0;
		stress.value = 0;
		fatigue.value = 0;
		anxiety.value = 0;
	}

	public void newSession(){
		//Session id should have been instantiated using last registered id of user
		//session_id =+ 1 ; 
	}
}


public class personClass {
	public string username;
	public string name;
	public float age;
	public int gender; 
	public string ethnicity;
	public string location;
	public string occupation;


	// This is placeholder at the moment, but functions to be written
	public void exportPersonData() { 
		//exporting current person data as json
		//string jsonLogging = JsonUtility.ToJson(productivity, stress, fatigue, anxiety);
	}

	public void exportLoggingData() { //This is the function attached to a button in the app
		//packaging current logging data as JSON
		//string jsonLogging = JsonUtility.ToJson();
	}
		

}








