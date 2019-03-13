using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/* 
 * Below are declarations of data object types for communicating with the API.
 * The specfication for this format can be found in docs/Data_Specification.md
 */

public class SelfReported
{
    public float anxiety;
    public float stress;
    public float fatigue;
    public float productivity;
}

public class PPG
{
    public int bodySensorLocation;
    //public Dictionary<string, int> heartrate = new Dictionary<string, int>();
    //public Dictionary<string, int> interbeatInterval = new Dictionary<string, int>();
    //public Dictionary<string, float> sp = new Dictionary<string, float>();
    public JObject heartRate;
    public JObject interbeatInterval;
    public JObject spO2;
}

public class GSR
{
    public int bodySensorLocation;
    //public Dictionary<string, int> scl = new Dictionary<string, int>();
    public JObject scl;
}

public class Session
{
    public string session_id;
    public string firmwareRevision;
    public SelfReported self_reported = new SelfReported();
    public PPG ppg = new PPG();
    public GSR gsr = new GSR();
}

public class User
{
    public string username;
    public string name;
    public float age;
    public int gender;
    public string ethnicity;
    public string location;
    public string occupation;
}


/* 
 * Networking Class: Abstraction of communicating to API through http requests.
 * 
 * Once we know what data we expect to receive from the API we can create a class for it to be able to handle it and extract 
 * and process each part of the data in the required manner
 */
public class Networking
{
    public enum RequestStates : int
    {
        inactive,
        requested,
        success,
        failed
    };
    public RequestStates requestStatus = RequestStates.inactive;
    public string webAddress;
    public string responseData;
    WWWForm form; // Needed to be able to encapsulate data for Post method 

    public bool dataUploaded = false; // Boolean variable to check if data was uploaded successfully
    //public string webaddress; // address to send the data to
    string timestamp;

    public void SetRoute(string route)
    {
        webAddress = route;
    }

    /*
     * Attached to a button on the app to retreive data from the API
     */
    //public void MakeGetRequest(MonoBehaviour myMonoBehaviour)
    //{

    //}

    public void RequestUserData(MonoBehaviour MBContext, string username)
    {
        SetRoute("mhml.greenberg.io/api/users/" + username);
        MBContext.StartCoroutine(StartGetRequest());
    }

    //public void MakePostRequest(MonoBehaviour myMonoBehaviour)
    //{
    //    myMonoBehaviour.StartCoroutine(StartPostRequest());
    //}


    public void UploadSession(MonoBehaviour MBContext, string username, Session sessionToUpload)
    {
        SetRoute("mhml.greenberg.io/api/users/" + username + "/sessions");
        string session_json = JsonConvert.SerializeObject(sessionToUpload);
        string key = "Session";
        form = new WWWForm();
        form.AddField(key, session_json);

        MBContext.StartCoroutine(StartPostRequest());
    }

    public void UploadUser(MonoBehaviour MBContext, User userObj)
    {
        SetRoute("mhml.greenberg.io/api/users/");
        string user_json = JsonConvert.SerializeObject(userObj);
        string key = "User";
        form = new WWWForm();
        form.AddField(key, user_json);

        MBContext.StartCoroutine(StartPostRequest());
    }

    private IEnumerator StartGetRequest()
    {
        requestStatus = RequestStates.requested;
        Debug.Log("GET Request has been triggered: "+webAddress);

        UnityWebRequest www = UnityWebRequest.Get(webAddress); 
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error with GET request to : "+webAddress);
            Debug.Log(www.error);
            requestStatus = RequestStates.failed;
        }
        else
        {
            Debug.Log("Successful GET request to : " + webAddress);
            Debug.Log(www.downloadHandler.text);
            responseData = www.downloadHandler.text;

            //person = JsonUtility.FromJson<User>(login.responseData); // should be here

            requestStatus = RequestStates.success;
        }
    }

    private IEnumerator StartPostRequest()
    {
        requestStatus = RequestStates.requested;
        Debug.Log("POST Request has been triggered: "+webAddress);

        UnityWebRequest www = UnityWebRequest.Post(webAddress, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error with POST request to : " + webAddress);
            Debug.Log(www.error);
            requestStatus = RequestStates.failed;
        }
        else
        {
            Debug.Log("POST Request has completed.");
            requestStatus = RequestStates.success;
        }
    }
}
