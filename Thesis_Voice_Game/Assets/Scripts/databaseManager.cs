using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Firebase.Database;
using TMPro;

public class databaseManager : MonoBehaviour
{
    public TMP_InputField Name;
    public keyLogger keyLogger;

    private string userID;
    private DatabaseReference dbReference;
    private string currentTime;

    // Start is called before the first frame update
    void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateUser()
    {
        User newUser = new User(Name.text);
        Debug.Log(Name.text);
        string json = JsonUtility.ToJson(newUser);
        Debug.Log(json);

        PlayerPrefs.SetString("name", Name.text);
        dbReference.Child("users").Child(userID).SetRawJsonValueAsync(json);
        dbReference.Child("users").Child(userID).Child("meganVer").SetValueAsync(PlayerPrefs.GetInt("meganVer"));
        keyLogger.addLog("Nickname: " + Name.text);
        keyLogger.addLog("Megan Version: " + PlayerPrefs.GetInt("meganVer").ToString());
    }

    public void addLog ()
    {
        DatabaseReference newRef = dbReference.Child("users").Child(userID).Push();
        newRef.Child("time").SetValueAsync(time());
        newRef.Child("type").SetValueAsync("Button Clicked");
        string btnName = EventSystem.current.currentSelectedGameObject.name;
        newRef.Child("btnName").SetValueAsync(btnName);
    }

    public void addLog (string agentResponse, string type)
    {
        DatabaseReference newRef = dbReference.Child("users").Child(userID).Push();
        newRef.Child("time").SetValueAsync(time());
        newRef.Child("type").SetValueAsync(type);
        newRef.Child("value").SetValueAsync(agentResponse);
    }

    public void addLogSpacebar (string type)
    {
        DatabaseReference newRef = dbReference.Child("users").Child(userID).Push();
        newRef.Child("time").SetValueAsync(time());
        newRef.Child("type").SetValueAsync(type);
        dbReference.Child("users").Child(userID).Child(time()).SetValueAsync("Phase Started");
    }
    
    public void addPhase (int phase)
    {
        DatabaseReference newRef = dbReference.Child("users").Child(userID).Push();
        newRef.Child("time").SetValueAsync(time());
        newRef.Child("type").SetValueAsync("Phase Started");
        newRef.Child("phase").SetValueAsync(phase);
    }

    private string time ()
    {
        string t = " " + System.DateTime.UtcNow.ToLocalTime().ToString("HH:mm:ss dd-MM-yyyy") + "\n";
        return t;
    }

}
