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
	// These are all the variables/methods that can be accessed in main.cs to control the class as if it was a function.

	public bool dataUploaded = false; // Boolean variable to check if data was uploaded successfully
	public bool senduserdata = false; // Set to true in main.cs if we are performing a user_data upload.
	public bool sendsessiondata = false; // Set to true in main.cs if we are performing a session_data upload.
	public string userwebaddress; // address to send the user data to
	public string sessionwebaddress; // address to send the session data to
	public User user = new User(); // set this object to the user data without Jsonification 
	public Session session = new Session(); //set this oject to the sessionid data without Jsonification


	public void SetUserRoute(string route) { // Use this function to set the user data webaddress
		userwebaddress = route;
	}

	public void SetSessionRoute(string route) { // Use this function to set the session data webaddress
		sessionwebaddress = route;
	}

	public void makeRequest(MonoBehaviour myMonoBehaviour){
		myMonoBehaviour.StartCoroutine(Upload());
	}

	// This is the function used to upload the user_data and/or session_data depending on how the variables defined above are set. 

	IEnumerator Upload() {
		if (senduserdata == true){ // Executes if we are sending user data (Note: senduserdata must be set to True in main.cs for this to execute).
			dataUploaded = false;
			Debug.Log("Starting User Data Upload");					
			WWWForm user_form = new WWWForm(); //Create a form object needed to be able to encapsulate data for Post method 
			string user_json = JsonUtility.ToJson(user); //We jsonify the user_data object
			user_form.AddField("User", user_json); // We add the jsonified string to the form object
			UnityWebRequest www = UnityWebRequest.Post(userwebaddress, user_form); //We Post the form to the userwebaddress (set above)
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

		if (sendsessiondata == true){ // Executes if we are sending session data (Note: sendsessiondata must be set to True for this to execute.
			dataUploaded = false; //Same logic as that described for the userdata if statement above.
			Debug.Log("Starting Session Data Upload");					
			WWWForm session_form = new WWWForm();
			string session_json = JsonUtility.ToJson(session); 
			session_form.AddField("Session", session_json); 
			UnityWebRequest www = UnityWebRequest.Post(sessionwebaddress, session_form); 
			yield return www.SendWebRequest();

			if(www.isNetworkError || www.isHttpError) {
				Debug.Log(www.error); 
				dataUploaded = false;
			}
			else {
				Debug.Log("Form upload complete!");
				dataUploaded = true;
			}
		}

	}

}
