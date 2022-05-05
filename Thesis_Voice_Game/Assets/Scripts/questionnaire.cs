using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class questionnaire : MonoBehaviour
{
    [SerializeField] GameObject OpenEnded;
    [SerializeField] GameObject question;
    [SerializeField] ToggleGroup toggleGroup;
    [SerializeField] InputField comments;
    [SerializeField] InputField email;
    private List<string> questionArr;
    private Dictionary<string, string> data;
    public sendFeedback sendToGoogle;

    public Toggle selectedToggle
    {
        get { return toggleGroup.ActiveToggles().First(); }
    }

    private void Start()
    {
        questionArr = new List<string>
                            { "I blocked out things around me when I was playing the game.",
                              "I was so involved in this experience that I lost track of time.",
                              "Playing the game was mentally taxing.",
                              "I found the game confusing to play.",
                              "I felt frustrated while playing the game.",
                              "The game was aesthetically appealing.",
                              "The screen layout of the game was visually pleasing.",
                              "Playing the game on this website was worthwhile.",
                              "I felt interested in my gaming task.",
                              "My gaming experience was rewarding."
                            };

        data = new Dictionary<string, string>();
        Debug.Log("sessionName is " + staticVar.sessionName);
        data.Add("sessionName", staticVar.sessionName);
        pickRandomQ();
    }

    //this function is specified because we cannot know when to move on from
    //toggle questions from UI. Conversely, we do not need a goToEmail function
    //because it will always follow the 1 open ended question
    private void goToComments ()
    {
        GameObject currQ = GameObject.Find("Toggles");
        OpenEnded.SetActive(true);
        currQ.SetActive(false);
    }

    //chooses and displays next question chosen randomly from the defined array
    public void pickRandomQ ()
    {
        if (questionArr.Count == 0)
        {
            goToComments();
        }

        int index = Random.Range(0, questionArr.Count - 1); // pick random element from the list
        string randomQ = questionArr[index];
        questionArr.RemoveAt(index);   // remove question to prevent repeats

        question.GetComponent<Text>().text = randomQ;
    }

    //convert dictionary to string 
    private string dataString (Dictionary<string, string> data)
    {
        string s = "";
        foreach (KeyValuePair<string, string> kvp in data)
        {
            s += kvp.Key + " " + kvp.Value + "\n";
        }

        return s;
    }

    //add response to dictionary 
    public void saveToggleData ()
    {
        string qText = question.GetComponent<Text>().text;
        data.Add(qText, selectedToggle.name);
    }

    public void saveCommentsData ()
    {
        data.Add("Comments:", comments.text);
    }

    //submit data to google form
    public void submitData()
    {
        sendToGoogle.send(false, dataString(data));
        data.Clear();
    }

    //submit email to google form
    //submitted separatly so not linked with previous responses
    //follow up survery needs to be sent a week from submission, so timestsamp also submitted
    public void submitEmail ()
    {
        string time = System.DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy");
        string s = email.text + " " + time;
        sendToGoogle.send(true, s);
    }
}
