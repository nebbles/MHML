using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

public class main : MonoBehaviour{
	//public GameObject obj; 
	public InputField userNameInput;
	Networking login = new Networking();
	//Networking login = obj.AddComponent<Networking>();
	//public string input;
	//public bool dataAvailable = false; 
	//string aELKJF =  "mhml.greenberg.io/api/users";

	public main() {
	}

	void Start(){
		Debug.Log("bensanoob"); 
	}

	public void loginButton() {
		// add user name				
		Debug.Log(userNameInput);

		login.setRoute("mhml.greenberg.io/api/users/" + userNameInput.text);
		login.makeRequest(this); 	
	}

	void Update() {
		Debug.Log("leahizcoo");
		if(login.dataAvailable==true) {
			Debug.Log("true");
			Debug.Log(login.dataWeb);
			//initial = JsonUtility.FromJson<User>(login.dataWeb);
			//Debug.Log(initial);
			//Debug.Log(initial.usernames);
		}

	}
}
