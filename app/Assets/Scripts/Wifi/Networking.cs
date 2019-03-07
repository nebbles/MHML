using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;


//Once we know what data we expect to receive from the API we can create a class for it to be able to handle it and extract 
//and process each part of the data in the required manner

public class Networking {	
	public bool dataAvailable = false; 
	public string dataWeb ;
	public string webAddress;
	public bool dataNotAvailable = false;


	public void setRoute(string route) {
		webAddress = route;
	}
		
	public void makeRequest(MonoBehaviour myMonoBehaviour){ //Function attached to a button on the app to retreive data from the API
		myMonoBehaviour.StartCoroutine(getUsers());
		}
		
	IEnumerator getUsers() {
		//"mhml.greenberg.io/api/users"
		Debug.Log("GetUSers");
		UnityWebRequest www = UnityWebRequest.Get(webAddress); //Get request to API
		yield return www.SendWebRequest();
			
		if(www.isNetworkError || www.isHttpError) { //This statement is executed if the get request fails
			Debug.Log(www.error); //A message is displayed on the console in unity
			dataNotAvailable = true;
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


		

}
