using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Wifi : MonoBehaviour {
	public void send_message_now() {
		StartCoroutine(Upload());
	}
		
	IEnumerator Upload() {
		var myjsonmessage = "{\"Name\" :\"Panos\", \"Age\":\"22\"}";
		byte[] myData = System.Text.Encoding.UTF8.GetBytes(myjsonmessage);
		UnityWebRequest www = UnityWebRequest.Put("https://meeha.free.beeceptor.com", myData);
		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
		}
		else {
			Debug.Log("Upload complete!");
		}
	}
}


