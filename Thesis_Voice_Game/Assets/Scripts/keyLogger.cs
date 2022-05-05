using UnityEngine;
using UnityEngine.EventSystems;

public class keyLogger : MonoBehaviour
{
    public string log;
    public sendKeylog sendToGoogle;

    void Start()
    {
        log = "";
    }

    public void addLog ()
    {
        string btnName = EventSystem.current.currentSelectedGameObject.name;
        log += "Clicked on " + btnName + time();
        //Debug.Log(log);
    }

    public void addLog (string agentResponse)
    {
        log += agentResponse + time();
        //Debug.Log(log);
    }

    public void addLogSpacebar ()
    {
        log += "Spacebar pressed, speaking to chatbot" + time();
        //Debug.Log(log);
    }

    public void addPhase (int phase)
    {
        log += "Starting phase " + phase + time();
        //Debug.Log(log);
    }

    private string time ()
    {
        string t = " " + System.DateTime.UtcNow.ToLocalTime().ToString("HH:mm:ss dd-MM-yyyy") + "\n";
        return t;
    }

    public void submitLog ()
    {
        sendToGoogle.send(log);
    }

}
