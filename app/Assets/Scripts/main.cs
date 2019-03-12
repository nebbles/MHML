using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.
using VoxelBusters.NativePlugins;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class main : MonoBehaviour{
    GameObject loginScreen, homeScreen, createAccount, canvas, relaxScreen, logScreen, settings, thankYou, technicalDifficulties;
    public InputField usernameInput, createUsername, cname, ethnicity, location, occupation, age;
    public GameObject  anxious ;
    public Dropdown gender;
    public Slider productivity, stress, fatigue, anxiety;
    public int ppgsensorLocation, gsrsensorLocation, HR, IBI, SCL;
    public float SP;
    public Text textspazz ; 
    string timestamp;
    bool deviceConnected = false;
    public UnityEngine.UI.Extensions.UILineRenderer LineRenderer; 
    

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
        
        if (bluetoothData.isConnected == true)
        {
            bluetoothData._storeSubscribeData = true;
            newSession.session_id = System.DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            newSession.firmwareRevision = bluetoothData._deviceInfo_data[bluetoothData._deviceInfo_data.Count - 1];
            textspazz.text = newSession.firmwareRevision;
            anxious.SetActive(true);
        }
        
        else
        {
            string[] _buttons = new string[]
            {
                "Cancel",
                "Connect"
            }; 

            NPBinding.UI.ShowAlertDialogWithMultipleButtons("Cannot start session", "Please connect to bluetooth device", _buttons, OnButtonPressed); 
        }
    }

    private void OnButtonPressed(string _buttonPressed)
    {
        Debug.Log("Button pressed: " + _buttonPressed);
        if (_buttonPressed == "Connect")
        {
            settings.SetActive(true);
            logScreen.SetActive(false);
        }
        else
        {
            Debug.Log("Cancel");
            logScreen.SetActive(false);
            logScreen.SetActive(true);
        }
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

       
        HR = bluetoothData._HR_data[bluetoothData._HR_data.Count - 1];
        IBI = bluetoothData._IBI_data[bluetoothData._IBI_data.Count - 1];
        SP = bluetoothData._Spo2_data[bluetoothData._Spo2_data.Count - 1];

        var heartRateobj = new JObject();
        heartRateobj.Add(timestamp, HR);

        var interbeatInterval_obj = new JObject();
        interbeatInterval_obj.Add(timestamp, IBI);

        var spO2_obj = new JObject();
        spO2_obj.Add(timestamp, SP);

        var scl_obj = new JObject();
        scl_obj.Add(timestamp, SP);

        //PPG Service
        newSession.ppg.heartRate = heartRateobj;
        newSession.ppg.interbeatInterval = interbeatInterval_obj;
        newSession.ppg.spO2 = spO2_obj;
        ppgsensorLocation = bluetoothData._ppgLocation_data[bluetoothData._ppgLocation_data.Count - 1];


        //GSR Service
        gsrsensorLocation = bluetoothData._gsrLocation_data[bluetoothData._gsrLocation_data.Count - 1];
        SCL = bluetoothData._skinConductance_data[bluetoothData._skinConductance_data.Count - 1];
        newSession.gsr.scl = scl_obj; 

        newSession.gsr.bodySensorLocation = gsrsensorLocation;
        newSession.gsr.scl.Add(timestamp, SCL);
    }


    public void technicalButton()
    {
        if (thankYou.activeSelf == true)
        {
            thankYou.SetActive(false);
            logScreen.SetActive(true);
        }

        else if (createAccount == true)
        {
            loginScreen.SetActive(true);
            createAccount.SetActive(false); 
        }
        technicalDifficulties.SetActive(false);
    }


    public void submitSession()
    {

        wifiSession.sessionUpload(person.username, newSession); 
        wifiSession.makeRequest(this);

        bluetoothData._storeSubscribeData = false;
        bluetoothData.clearDataAfterSession(); 

        if (wifiSession.dataUploaded == false)
        {
            technicalDifficulties.SetActive(true); 
        }

        else
        {
            wifiSession.dataUploaded = false;
            homeScreen.SetActive(true);
            thankYou.SetActive(false);
        }
    }

    public void graphData()
    {
        // need to normalise data between -400 & 400 == x values, -200 & 200 == y values 
        float xval = 2;
        float yval = 3;

        var point = new Vector2() { x = xval, y = yval };
        var pointlist = new List<Vector2>(LineRenderer.Points);
        pointlist.Add(point);


    }




}
