using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class sendFeedback : MonoBehaviour
{
    private string url = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSeOkHpB0JP9209zbQTDdSa0HA7LuZ0sPpzBFgYeIjxMz6kXxg/formResponse";

    public void send(bool isEmail, string response)
    {
          StartCoroutine(Post(isEmail, response));
    }

    IEnumerator Post(bool e, string s)
    {
        WWWForm form = new WWWForm();

        //if input is not email...
        if (!e)
        {
            form.AddField("entry.1071583623", s);

        }
        else
        {
            form.AddField("entry.662866229", s);
        }

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
    }
}
