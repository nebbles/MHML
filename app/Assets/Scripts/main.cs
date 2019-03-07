using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

public class main : MonoBehaviour{
	public GameObject loginScreen, homeScreen, createAccount, canvas, relaxScreen, logScreen,settings, thankYou ;
    Text usernameInput;
	Networking login = new Networking();
	personClass initial = new personClass(); 

	public main() {

	}

	void Start(){
		canvas = GameObject.Find("Canvas"); 
        loginScreen = canvas.transform.Find("LogInScreen").gameObject;
        homeScreen = canvas.transform.Find("HomeScreen").gameObject;
		createAccount = canvas.transform.Find("CreateAccount").gameObject;
		relaxScreen = canvas.transform.Find("RelaxScreen").gameObject;
		logScreen = canvas.transform.Find("Log Screen").gameObject;
		thankYou = canvas.transform.Find("Thank-you").gameObject;
		settings = canvas.transform.Find("You Screen").gameObject;
        usernameInput = loginScreen.GetComponent<InputField>().GetComponent<Text>();
        
    }
    
    void Update()
    {
        // Login screen update script
        if (loginScreen.activeSelf) {
            Debug.Log("leahizcoo");
            if (login.dataAvailable == true)
            {
                Debug.Log("true");
                loadUserData();
                login.dataAvailable = false;

                loginScreen.SetActive(false);
                homeScreen.SetActive(true);
            }

            if (login.dataNotAvailable == true)
            {
                Debug.Log("no user");
                loginScreen.SetActive(false);
                createAccount.SetActive(true);
                login.dataNotAvailable = false;
            }
        }

        

    }

    public void loginButton() {
		// add user name				
		//Debug.Log(userNameInput);
		login.setRoute("mhml.greenberg.io/api/users/" + usernameInput );
		login.makeRequest(this); 	
	}

	public void loadUserData(){
		Debug.Log("loadUserData"); 
		initial = JsonUtility.FromJson<personClass>(login.dataWeb);
		Debug.Log(initial.name);
    }

}
