using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class Person { //For any data we need to send, we need to first store it in an object of a pre-defined class,
	public string name; //For example, if we want to send a user_name and an ID together, then the class created
	public byte[] key; //needs two variables, one of type int (for the age) and one of type string (for the username)
	public int age; //The code here contains more than this for testing purposes to see the types of data that we can send
	public int[] numbers;//An object of this class is then created in the Upload() method and used to store the data that needs to be sent (see below)

}
	
public class Wifi : MonoBehaviour {
	public void send_message_now() { //This is the function attached to a button in the app
		StartCoroutine(upload()); //it sends a message to the API whenever the button is pressed
	}


	IEnumerator upload() {
		WWWForm form = new WWWForm(); //Type of data needed to be able to encapsulate data
		Person panos = new Person(); //Object created of the class that contains the variables that will store the data
		panos.name = "MEE7A"; //In this case the name is a string "MEE7A"
		panos.age = 22; //the age is an integer 22
		panos.key = new byte[] {1, 2, 3}; //how to specify and send arrays of any type, in this case it is byte arrays
		panos.numbers = new int[] { 1, 2, 3, 4, 5}; //how to specify and send arrays of any type, in this case it is integer arrays
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


