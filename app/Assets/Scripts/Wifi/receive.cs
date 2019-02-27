using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class receive : MonoBehaviour {	
		public void Upload(){
			StartCoroutine(GetText());
		}

		IEnumerator GetText() {
			UnityWebRequest www = UnityWebRequest.Get("https://meeha.free.beeceptor.com");
			yield return www.SendWebRequest();

			if(www.isNetworkError || www.isHttpError) {
				Debug.Log(www.error);
			}
			else {
				// Show results as text
				Debug.Log(www.downloadHandler.text);

				// Or retrieve results as binary data
				byte[] results = www.downloadHandler.data;
			}
		}
	}
