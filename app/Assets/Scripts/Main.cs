using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.
using VoxelBusters.NativePlugins;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class Main : MonoBehaviour
{
    GameObject canvas;
    GameObject settings;
    GameObject loginScreen;
    GameObject createAccount;
    GameObject homeScreen;
    GameObject logScreen;
    GameObject thankYou;
    GameObject technicalProblems;
    GameObject accountDetails;

    public Text accountDetailsName;
    public Text accountDetailsAge;
    public Text accountDetailsEthnicity;
    public Text accountDetailsLocation;
    public Text accountDetailsOccupation;

    public Text createAccountUsernameField;

    public InputField usernameInput, createUsername, cname, ethnicity, location, occupation, age;
    public GameObject anxious;
    public Dropdown gender;
    public Slider productivity, stress, fatigue, anxiety;
    public Text pleaseWaitSubmission;
    public int ppgsensorLocation, gsrsensorLocation, HR, IBI, SCL;
    public float SP;

    string username;
    string timestamp;
    public UnityEngine.UI.Extensions.UILineRenderer LineRenderer;

    Networking login = new Networking();
    Networking createUserRequest = new Networking();
    Networking submitSessionRequest = new Networking();

    //Wifi wifiSession = new Wifi();

    User person = new User();
    Session newSession = new Session();
    public controller bluetoothData;

    public Main()
    {

    }

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        settings = canvas.transform.Find("SettingsScreen").gameObject;
        loginScreen = canvas.transform.Find("LogInScreen").gameObject;
        createAccount = canvas.transform.Find("CreateAccount").gameObject;
        homeScreen = canvas.transform.Find("HomeScreen").gameObject;
        logScreen = canvas.transform.Find("LogScreen").gameObject;
        thankYou = canvas.transform.Find("ThankYou").gameObject;
        technicalProblems = canvas.transform.Find("TechnicalProblems").gameObject;
        accountDetails = canvas.transform.Find("AccountDetails").gameObject;
    }

    void Update()
    {
        if (loginScreen.activeSelf) // Login screen background behaviour
        {
            if (login.requestStatus == Networking.RequestStates.success)
            {
                Debug.Log("Data was received for user.");
                person = JsonUtility.FromJson<User>(login.responseData);
                login.requestStatus = Networking.RequestStates.inactive;

                //accountDetailsName.text = person.name;

                loginScreen.SetActive(false);
                homeScreen.SetActive(true);
            }

            if (login.requestStatus == Networking.RequestStates.failed)
            {
                Debug.Log("No user data was found for username. Sending to <create account>.");
                login.requestStatus = Networking.RequestStates.inactive;

                createAccountUsernameField.text = username;

                loginScreen.SetActive(false);
                createAccount.SetActive(true);
            }
        }
        if (createAccount.activeSelf)
        {
            if (createUserRequest.requestStatus == Networking.RequestStates.success)
            {
                homeScreen.SetActive(true);
                createAccount.SetActive(false);
                createUserRequest.requestStatus = Networking.RequestStates.inactive;
            }
            if (createUserRequest.requestStatus == Networking.RequestStates.failed)
            {
                technicalProblems.SetActive(true); // activate sorry technical issues page
                createUserRequest.requestStatus = Networking.RequestStates.inactive;
            }
        }
        if (logScreen.activeSelf)
        {
            if (submitSessionRequest.requestStatus == Networking.RequestStates.success)
            {
                pleaseWaitSubmission.enabled = false; // reset
                thankYou.SetActive(true);
                logScreen.SetActive(false);

                // Clear session data
                newSession = new Session();
            }

            if (submitSessionRequest.requestStatus == Networking.RequestStates.failed)
            {
                pleaseWaitSubmission.enabled = false; // reset
                technicalProblems.SetActive(true);
                logScreen.SetActive(false);
            }
        }
    }

    public void LoginButton()
    {
        Debug.Log("Login button pressed");
        username = usernameInput.text;
        Debug.Log("Username requested: " + username);
        login.RequestUserData(this, username);
    }

    public void CreateAnAccount()
    {
        Debug.Log("Create account is being submitted.");
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
        createUserRequest.UploadUser(this, person);
    }


    public void StartSessionButton()
    {

        if (bluetoothData.isConnected == true)
        {
            bluetoothData._storeSubscribeData = true;
            //newSession.session_id = System.DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            //newSession.firmwareRevision = bluetoothData._deviceInfo_data[bluetoothData._deviceInfo_data.Count - 1];
            anxious.SetActive(true);
        }

        else
        {
            string[] _buttons = new string[]
            {
                "Cancel",
                "Connect"
            };

            NPBinding.UI.ShowAlertDialogWithMultipleButtons("Cannot start session", "Please connect to bluetooth device", _buttons, OnModalPress);
        }
    }

    private void OnModalPress(string _buttonPressed)
    {
        Debug.Log("Modal button pressed: " + _buttonPressed);
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

    public void StoreSelfReportedData()
    {
        newSession.self_reported.anxiety = anxiety.value;
        newSession.self_reported.productivity = productivity.value;
        newSession.self_reported.stress = stress.value;
        newSession.self_reported.fatigue = fatigue.value;

        // reset sliders
        productivity.value = 0;
        stress.value = 0;
        fatigue.value = 0;
        anxiety.value = 0;
    }

    public void SubmitSessionButton()
    {
        Debug.Log("start");

        pleaseWaitSubmission.enabled = true;
        timestamp = System.DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture);

        Debug.Log("start2");

        newSession.session_id = timestamp;
        //newSession.firmwareRevision = "don't have yet";
        newSession.firmwareRevision = bluetoothData._deviceInfo_data[0];


        Debug.Log("start3");


        /*
         * Debug method without BLE >>>>>>>>>>>>
         */

        //HR = 50;
        //IBI = 50;
        //SP = 50.5f;
        //SCL = 50;

        //var heartRateobj = new JObject();
        //heartRateobj.Add(timestamp, HR);
        //var interbeatInterval_obj = new JObject();
        //interbeatInterval_obj.Add(timestamp, IBI);
        //var spO2_obj = new JObject();
        //spO2_obj.Add(timestamp, SP);
        //var scl_obj = new JObject();
        //scl_obj.Add(timestamp, SCL);

        // END DEBUG WITHOUT BLE  <<<<<<<<<<<<<<<<



        var heartRateobj = new JObject();
        for (int i = 0; i < bluetoothData._HR_data.Count; i++)
        {
            timestamp = Random.Range(0.0f, 100.0f).ToString();
            heartRateobj.Add(timestamp, bluetoothData._HR_data[i]);
        }

        Debug.Log("start4");

        var interbeatInterval_obj = new JObject();
        for (int i = 0; i < bluetoothData._IBI_data.Count; i++)
        {
            timestamp = Random.Range(0.0f, 100.0f).ToString();
            interbeatInterval_obj.Add(timestamp, bluetoothData._IBI_data[i]);
        }

        Debug.Log("start5");

        var spO2_obj = new JObject();
        for (int i = 0; i < bluetoothData._Spo2_data.Count; i++)
        {
            timestamp = Random.Range(0.0f, 100.0f).ToString();
            spO2_obj.Add(timestamp, bluetoothData._Spo2_data[i]);
        }

        Debug.Log("start6");

        var scl_obj = new JObject();
        for (int i = 0; i < bluetoothData._skinConductance_data.Count; i++)
        {
            timestamp = Random.Range(0.0f, 100.0f).ToString();
            scl_obj.Add(timestamp, bluetoothData._skinConductance_data[i]);
        }

        Debug.Log("star7t");

        //int bsl = 5;
        int bsl = bluetoothData._ppgLocation_data[0];

        Debug.Log("start8");

        // PPG Service
        newSession.ppg.heartRate = heartRateobj;
        newSession.ppg.interbeatInterval = interbeatInterval_obj;
        newSession.ppg.spO2 = spO2_obj;
        newSession.ppg.bodySensorLocation = bsl;

        // GSR Service
        newSession.gsr.scl = scl_obj;
        newSession.gsr.bodySensorLocation = bsl;

        // Submit session

        //username = "scottin"; // DEBUG OVERRIDE
        //timestamp = System.DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture);

        submitSessionRequest.UploadSession(this, username, newSession);

        //wifiSession.sessionUpload(newSession);
        //wifiSession.makeRequest(this);

        // Handle BLE disabling
        bluetoothData._storeSubscribeData = false;
        bluetoothData.clearDataAfterSession();
    }


    public void TechnicalButton()
    {
        if (createAccount.activeSelf == true)
        {
            loginScreen.SetActive(true);
            createAccount.SetActive(false);
        }

        if (logScreen.activeSelf == true)
        {
            homeScreen.SetActive(true);
            logScreen.SetActive(false);
        }

        technicalProblems.SetActive(false);
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
