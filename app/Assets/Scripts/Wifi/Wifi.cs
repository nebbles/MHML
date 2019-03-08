using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.


public class selfReported{
	float anxiety;
	float stress;
	float fatigue;
	float productivity;
}

public class PPG {                                    
        int bodySensorLocation;
        IDictionary<string, int> heartrate = new Dictionary<string, int>();
	IDictionary<string, int> interbeatInterval = new Dictionary<string, int>();
	IDictionary<string, float> sp = new Dictionary<string, float>();
}

public class GSR {                                    
        int bodySensorLocation;
        IDictionary<string, int> scl = new Dictionary<string, int>();
}

public class Session {
	string session_id;
	string firmwareRevision;
    	selfReported self_reported = new selfReported();
	PPG ppg = new PPG();
	GSR gsr = new GSR();
	
	}

public class User {
    string username;
    string name;
    float age;
    int gender;
    string ethnicity;
    string location;
    string occupation;
}
public class Wifi : MonoBehaviour {
	public void send_message_now() { //This is the function attached to a button in the app
		StartCoroutine(upload()); //it sends a message to the API whenever the button is pressed
	}
	
	

	IEnumerator upload() {
		// change to have input as webaddress & object
		WWWForm form = new WWWForm(); //Type of data needed to be able to encapsulate data
		personClass panos = new personClass(); //Object created of the class that contains the variables that will store the data
		panos.username = "teehee";
		panos.name = "MEE7A"; //In this case the name is a string "MEE7A"
		panos.age = 22; //the age is an integer 22
		panos.gender = true ; 
		panos.ethnicity = "white"; //how to specify and send arrays of any type, in this case it is byte arrays
		panos.location = "London"; //how to specify and send arrays of any type, in this case it is integer arrays
		panos.occupation = "Student";
		string panos_json = JsonUtility.ToJson(panos); //We jsonify the object panos into a string panos_json so that we send our data in json format for processing by the API
		form.AddField("Panos", panos_json); //The json message is then encapsulated in a form object (needed for http Post method used for sending)

		UnityWebRequest www = UnityWebRequest.Post("mhml.greenberg.io/person", form); //We Post (i.e.) send the form containing the json message to the API
		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error); //If an error occurs, the console on unity displays this message
		}
		else {
			Debug.Log("Form upload complete!"); //If the sending goes well, the console on unity displays this message
		}
	}



}


