using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;


public class selfReported{
	public float anxiety;
	public float stress;
	public float fatigue;
	public float productivity;
	}

public class PPG {                                    
	public int bodySensorLocation;
	public Dictionary<string, int> heartrate = new Dictionary<string, int>();
	public Dictionary<string, int> interbeatInterval = new Dictionary<string, int>();
	public Dictionary<string, float> sp = new Dictionary<string, float>();
	}

public class GSR {                                    
	public int bodySensorLocation;
	public Dictionary<string, int> scl = new Dictionary<string, int>();
	}

public class Session {
	public string session_id;
	public string firmwareRevision;
	public selfReported self_reported = new selfReported();
	public PPG ppg = new PPG();
	public GSR gsr = new GSR();
	}

public class User {
	public string username;
	public string name;
	public float age;
	public int gender;
	public string ethnicity;
	public string location;
	public string occupation;
	}

public class Wifi { // Class created to send user_data and session_data (based on the classes User and Session defined above).
	// These are all the variables or methods that can be accessed in main.cs to control the class as if it was a function.

	public bool dataUploaded = false; // Boolean variable to check if data was uploaded successfully
	public string webaddress; // address to send the data to
  	public string key; // use "U" for user data and "S" for session data
	public string jsondata; // jsonified object to be sent
    

	public void SetUserRoute(string route) { // Use this function to set the data webaddress
		webaddress = route;
		}

	public void makeRequest(MonoBehaviour myMonoBehaviour){
		myMonoBehaviour.StartCoroutine(Upload());
		}

    // This is the function used to upload the user_data and/or session_data depending on how the variables defined above are set. 


    public void sessionUpload(string username, string session_json)
    {
        webaddress = "mhml.greenberg.io/api/users/" + username + "sessions";
        key = "S";
        jsondata = session_json;


    }        
	IEnumerator Upload() {
			dataUploaded = false;
			Debug.Log("Starting Data Upload");					
			WWWForm form = new WWWForm(); //Create a form object needed to be able to encapsulate data for Post method 
			form.AddField(key, jsondata); // We add the jsonified string to the form object
			UnityWebRequest www = UnityWebRequest.Post(webaddress, form); //We Post the form to the userwebaddress (set above)
			yield return www.SendWebRequest();

			if(www.isNetworkError || www.isHttpError) {
				Debug.Log(www.error); //If an error occurs, the console on unity displays this message
				dataUploaded = false; // And the boolean dataUploaded is set to False
				}
			else {
				Debug.Log("Form upload complete!"); //If the sending goes well, the console on unity displays this message
				dataUploaded = true; // And the boolean dataUploaded is set to True
				}
			}
	}
