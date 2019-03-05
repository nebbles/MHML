using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//Once we know what data we expect to receive from the API we can create a class for it to be able to handle it and extract 
//and process each part of the data in the required manner
public class receive : MonoBehaviour {	
		
			public void uploadUsers(){ //Function attached to a button on the app to retreive data from the API
				StartCoroutine(getUsers());
			}

		IEnumerator getUsers() {
			
			UnityWebRequest www = UnityWebRequest.Get("mhml.greenberg.io/api/users"); //Get request to API
			yield return www.SendWebRequest();
				
			if(www.isNetworkError || www.isHttpError) { //This statement is executed if the get request fails
				Debug.Log(www.error); //A message is displayed on the console in unity
			}
			else { //This statement is executed when the get request has been processed successfully. 
				// Show data retrieved from the API as text in the console on unity
				Debug.Log(www.downloadHandler.text);

				// Or retrieve results as binary data
				byte[] results = www.downloadHandler.data; 
			}
		}


	}
