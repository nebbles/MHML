using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;


//Once we know what data we expect to receive from the API we can create a class for it to be able to handle it and extract 
//and process each part of the data in the required manner

public class User{
	public string[] usernames;

	}

public class receive : MonoBehaviour {	
	public string input;
	User initial;
	public bool dataAvailable = false; 
	//string aELKJF =  "mhml.greenberg.io/api/users";
	string dataWeb ; 


	public void uploadUsers(string webAddress){ //Function attached to a button on the app to retreive data from the API
			
		StartCoroutine(getUsers(webAddress));
		}




	IEnumerator getUsers(string WebAddress) {
		//"mhml.greenberg.io/api/users"
		UnityWebRequest www = UnityWebRequest.Get(WebAddress); //Get request to API
		yield return www.SendWebRequest();
			
		if(www.isNetworkError || www.isHttpError) { //This statement is executed if the get request fails
			Debug.Log(www.error); //A message is displayed on the console in unity

		}

		else { //This statement is executed when the get request has been processed successfully. 
			// Show data retrieved from the API as text in the console on unity
			Debug.Log(www.downloadHandler.text);
			dataWeb = www.downloadHandler.text;

			// Or retrieve results as binary data
			byte[] results = www.downloadHandler.data; 

			dataAvailable = true;
		}
	}

	void Start(){
	
	}

	void Update() {
		if(dataAvailable==true) {
			Debug.Log("true");
			Debug.Log(dataWeb);
			initial = JsonUtility.FromJson<User>(dataWeb);
			//Debug.Log(initial);
			Debug.Log(initial.usernames);
		}

	}

}
