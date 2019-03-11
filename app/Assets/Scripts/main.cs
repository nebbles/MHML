using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.


public class main : MonoBehaviour{
    GameObject loginScreen, homeScreen, createAccount, canvas, relaxScreen, logScreen, settings, thankYou, technicalDifficulties;
    public InputField usernameInput, createUsername, cname, ethnicity, location, occupation, age;
    public GameObject  measurement ;
    public Dropdown gender;
    public Slider productivity, stress, fatigue, anxiety;
    public int ppgsensorLocation, gsrsensorLocation, HR, IBI, SCL;
    public float SP;
    public Text textspazz ; 
    string timestamp;
    bool deviceConnected = false; 

    Networking login = new Networking();
    Wifi wifiPerson = new Wifi();
    Wifi wifiSession = new Wifi();

    User person = new User();
    Session newSession = new Session();
    public controller bluetoothData; 



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
        technicalDifficulties = canvas.transform.Find("TechnicalProblems").gameObject;
        
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

    public void loginButton()
    {
        // add user name				
        Debug.Log(usernameInput.text);
        
        login.setRoute("mhml.greenberg.io/api/users/" + usernameInput.text );
        Debug.Log(usernameInput);
        login.makeRequest(this); 
        
	}

	public void loadUserData()
    {
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
        person.occupation = occupation.text;
        person.age = int.Parse(age.text); 

        if (gender.ToString() == "Female")
        {
            person.gender = 1; 
        }
        else
        {
            person.gender = 0;
        }

        //Creating Wifi person object
        wifiPerson.userUpload(person);
        wifiPerson.makeRequest(this);

        if (wifiPerson.dataUploaded == false)
        {
            // activate sorry technical issues page
            createAccount.SetActive(false);
            technicalDifficulties.SetActive(true);

        }

        else {
            wifiPerson.dataUploaded = false ;
            homeScreen.SetActive(true);
            createAccount.SetActive(false); 

        }

    }


    public void startData()
    {
        
        
        // if bluetoothConnected == true: // _allSubscribingComplete  //_allReadingComplete // isConnected

        // TODO Make sure you add in the checks here to ensure the BLE device is connected and has data, otherwise one of three things could happen:
        // 1. The app could freeze.
        // 2. The app could 'stop working'. 
        // 3. The 'Begin' button click will not permit a click. 

        newSession.session_id = System.DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
        newSession.firmwareRevision = bluetoothData._deviceInfo_data.Peek();
        textspazz.text = newSession.firmwareRevision;
        measurement.SetActive(true); 
        
        // else - pop up modal window
        
    }

    public void logSelfReported()
    {
        newSession.self_reported.anxiety = anxiety.value;
        newSession.self_reported.productivity = productivity.value;
        newSession.self_reported.stress = stress.value;
        newSession.self_reported.fatigue = fatigue.value;

        resetSliders(); 
    }

    void resetSliders()
    {
        productivity.value = 0;
        stress.value = 0;
        fatigue.value = 0;
        anxiety.value = 0;
    }

    public void sensorData()
    {
        timestamp = System.DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture);

        //PPG Service
        ppgsensorLocation = bluetoothData._ppgLocation_data.Peek(); 
        HR = bluetoothData._HR_data.Peek();
        IBI = bluetoothData._IBI_data.Peek();
        SP = bluetoothData._Spo2_data.Peek();

        newSession.ppg.bodySensorLocation = ppgsensorLocation;
        newSession.ppg.heartrate.Add(timestamp, HR);
        newSession.ppg.interbeatInterval.Add(timestamp, IBI);
        newSession.ppg.sp.Add(timestamp, SP);

        textspazz.text = newSession.ppg.heartrate.ToString();


        //GSR Service
        gsrsensorLocation = bluetoothData._gsrLocation_data.Peek();
        SCL = bluetoothData._skinConductance_data.Peek();


        newSession.gsr.bodySensorLocation = gsrsensorLocation;
        newSession.gsr.scl.Add(timestamp, SCL);
    }

    public void submitSession()
    {
        Debug.Log(newSession.self_reported.anxiety);

        wifiSession.sessionUpload(person.username, newSession); 
        wifiSession.makeRequest(this);

        if (wifiSession.dataUploaded == false)
        {
            thankYou.SetActive(false);
            technicalDifficulties.SetActive(true); 

        }

        else
        {
            wifiSession.dataUploaded = false;
            homeScreen.SetActive(true);
            thankYou.SetActive(false);
        }
    }


}
