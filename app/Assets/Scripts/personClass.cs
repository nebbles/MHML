using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.


public class personClass : MonoBehaviour {
	// all class values
	public string Username;
	public string Name;
	//public string secondName;    ---> API POST does not currently accept first & second names
	//public string email ; 
	public int Age;
	public bool Gender;  // needs to be changed to string
	public string Ethnicity;
	public string Location;
	public string Occupation;

	public int session_id; //server expects str
	public Slider productivity;
	public Slider stress; 
	public Slider fatigue;
	public Slider anxiety;
	public float heartRate;
	public float GSR; 

	// This is placeholder at the moment, but functions to be written
	public void exportPersonData() { 
		//exporting current person data as json
		//string jsonLogging = JsonUtility.ToJson(productivity, stress, fatigue, anxiety);
	}

	public void exportLoggingData() { //This is the function attached to a button in the app
		//packaging current logging data as JSON
		//string jsonLogging = JsonUtility.ToJson();
	}

	public void resetSliders(){
		productivity.value = 0;
		stress.value = 0;
		fatigue.value = 0;
		anxiety.value = 0;
	}

	public void newSession(){
		//Session id should have been instantiated using last registered id of user
		session_id =+ 1 ; 
	}

}








