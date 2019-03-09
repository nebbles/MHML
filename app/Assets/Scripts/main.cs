using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

public class main : MonoBehaviour{
	public GameObject loginScreen, homeScreen, createAccount, canvas, relaxScreen, logScreen,settings, thankYou ;
    public InputField usernameInput, createUsername, cname, ethnicity, location, occupation, age; 
    public Dropdown  gender;
    Networking login = new Networking();
	User person = new User();
    //public LineRenderer line;

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
        //line.sortingOrder = 4; line.sortingLayerName = "UI";
        
    }
    
    void Update()
    {
        // Login screen update script
        if (loginScreen.activeSelf) {
            //Debug.Log("leahizcoo");
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
                createAnAccount(); 
            }
        }

        

    }

    public void loginButton() {
        // add user name				
        Debug.Log(usernameInput.text);
        
        login.setRoute("mhml.greenberg.io/api/users/" + usernameInput.text );
        Debug.Log(usernameInput);
        login.makeRequest(this); 
        
	}

	public void loadUserData(){
		//Debug.Log("loadUserData"); 
		person = JsonUtility.FromJson<User>(login.dataWeb);
		//Debug.Log(person.name);
    }

    public void createAnAccount()
    {
        Debug.Log("creating account");
        person.username = createUsername.text;
        person.name = cname.text;
        person.ethnicity = ethnicity.text;
        person.location = location.text; 
        person.age = int.Parse(age.text); 

        if (gender.ToString() == "Female")
        {
            person.gender = 1; 
        }
        else
        {
            person.gender = 0;
        }


    }

}
