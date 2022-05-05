//the following code is derived from the sample code provided in
//https://github.com/alessandroTironi/Unity-DialogflowV2Agents/tree/master/Assets/Scripts
//written by Alessandro Tironi with contributions by Hoa Tong

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Syrus.Plugins.DFV2Client;
using Random = UnityEngine.Random;

public class agent : MonoBehaviour
{
	private DialogFlowV2Client client;
	public string sessionName;
	public AudioSource audioPlayer;
	bool isRecording;				// determines whether the next spacebar keydown will start or stop bot recording
	public bool isSpeaking;
	public GameObject listening;	// the question mark icon that indicates chatbot is listening
    public GameObject thinking;		// the elipses animation that indicates chatbot is processing
    public keyLogger keyLogger;
	public databaseManager databaseManager;
	public gameStateController controller;

	public int sittingState;
	public int mood;

	//AUDIO RECORDING
	AudioClip recordedAudioClip;
    //Can use this as global variable too and GetComponent during start() to save resources
    //AudioSource audioSource;
    private float startRecordingTime;
	private string botOutput;

	private string GetSessionName()
	{
		string session = Random.Range(1, 1000000).ToString();
		staticVar.sessionName = session;
		return session;
	}

	void Start()
    {
		//Show which agent is being used
		keyLogger.addLog("Agent ver: " + controller.scene);
		Debug.Log(keyLogger.log);

		client = GetComponent<DialogFlowV2Client>();
		audioPlayer = GetComponent<AudioSource>();
		isRecording = false;
		isSpeaking = false;
        //keyLogger.addPhase(1);
		//databaseManager.addPhase(1);
		//databaseManager.addLog("1", "Phase Started");
		initiatePhaseContext(1);
		sessionName = GetSessionName();
		keyLogger.addLog(sessionName);
		//databaseManager.addLog(sessionName, "Session Name");
		//Debug.Log(sessionName);

		//subscribing to events
		client.ChatbotResponded += LogResponseText;
		client.DetectIntentError += LogError;
		client.SessionCleared += sess => Debug.Log("Cleared session " + sessionName);

		//client.ReactToContext("phase2finish",
		//	 context => Debug.Log("Reacting to phase2 finish"));
		sittingState = 0;
	}

	void Update()
    {
        if (isRecording == false && Input.GetKeyDown(KeyCode.Space))
        {
			StartRecord();
			isRecording = true;
			listening.SetActive(true);
			databaseManager.addLogSpacebar("Start Recording");
			//Debug.Log(isRecording);
        } else if (isRecording == true && Input.GetKeyDown(KeyCode.Space)) {
			AudioClip recorded = StopRecord();
			isRecording = false;
			listening.SetActive(false);
			thinking.SetActive(true);
			//Debug.Log(isRecording);
            keyLogger.addLogSpacebar();
			databaseManager.addLogSpacebar("Stop Recording");

			byte[] audioBytes = WavUtility.FromAudioClip(recorded);
			string audioString = Convert.ToBase64String(audioBytes);
            
            SendAudio(audioString);
		}
    }

    public AudioClip StopRecord()
    {
	    Microphone.End("");

	    //Trim the audioclip by the length of the recording
	    AudioClip recordingNew = AudioClip.Create(recordedAudioClip.name,
		    (int) ((Time.time - startRecordingTime) * recordedAudioClip.frequency), recordedAudioClip.channels,
		    recordedAudioClip.frequency, false);
	    float[] data = new float[(int) ((Time.time - startRecordingTime) * recordedAudioClip.frequency)];
	    recordedAudioClip.GetData(data, 0);
	    recordingNew.SetData(data, 0);
	    this.recordedAudioClip = recordingNew;

	    return recordedAudioClip;

        //Play recording
        //audioSource.clip = recordedAudioClip;
        //audioSource.Play();
    }

    public void StartRecord()
    {
	    //Get the max frequency of a microphone, if it's less than 44100 record at the max frequency, else record at 44100
	    int minFreq;
	    int maxFreq;
	    int freq = 44100;
	    Microphone.GetDeviceCaps("", out minFreq, out maxFreq);
	    if (maxFreq < 44100)
		    freq = maxFreq;

	    //Start the recording, the length of 300 gives it a cap of 5 minutes
	    recordedAudioClip = Microphone.Start("", false, 300, 44100);
	    startRecordingTime = Time.time;
    }
    public void SendAudio(string audioString)
	{
		client.DetectIntentFromAudio(audioString, sessionName , "en");
    }

	public void Clear()
	{
        client.ClearSession(sessionName);
	}

	//this function should return true when the chatbot has finished speaking
	public IEnumerator finishedSpeaking()
    {
		yield return new WaitUntil(() => audioPlayer.isPlaying == false);
		isSpeaking = false;
		mood = 0;
	}

	private void LogResponseText(DF2Response response)
	{
		if (response.responseId == null)
        {
            client.DetectIntentFromText("unknown", sessionName, "en");
            keyLogger.addLog("player input error - asked to repeat their input\n");
			databaseManager.addLog("player input error - asked to repeat their input", "Error");
		}

		string playerInput = "Player said: " + response.queryResult.queryText + "\n";
        botOutput = response.queryResult.fulfillmentText;
        keyLogger.addLog(playerInput);
		if (response.queryResult.queryText == null)
        {
			response.queryResult.queryText = "null";
		}
		databaseManager.addLog(response.queryResult.queryText, "Player Input");
		keyLogger.addLog("Chatbot said: " + botOutput + "\n");
		databaseManager.addLog(botOutput, "Chatbot Output");

		byte[] audioBytes = Convert.FromBase64String(response.OutputAudio);
        AudioClip clip = WavUtility.ToAudioClip(audioBytes);
        thinking.SetActive(false);

		if (botOutput == "I’ve felt like this for months. It’s gotten really unbearable lately." ||
			botOutput == "I’ve been feeling restless because I have no motivation to do anything but at the same time, I know I should be doing something productive with my life and so I hate myself for that." ||
			botOutput == "I feel like I’ve lost some weight lately. Actually, come to think of it, I’ve also been feeling really fatigued lately. This might be one of the reasons behind that." ||
            botOutput == "It makes me feel worn down. Like I wouldn’t be able to last much longer like this but I can’t see a light at the end of the tunnel so everything feels hopeless." ||
            botOutput == "I just feel so tired from all this. Even on the days that I do get some decent sleep I still feel fatigue when I wake up. I… I just feel like I can’t go on living like this." ||
            botOutput == "I… I need to improve. It’s not just minor things like commuting that this has affected me in. I feel like my mind is slow to react in thinks like my work place and classrooms as well and I really don’t like it." ||
            botOutput == "Trying to be perfect just made me feel like like I’m inadequate. Like I’m not good enough’ I used to keep trying and redoing tasks to try and make them perfect. But eventually I realised that nothing I do will ever be good enough so I gave up trying." ||
            botOutput == "It makes me feel worthless. But at the same time I feel like it might be true. It’s like, I’m worthless and everything I do is so there’s no point in my trying to do anything. Thoughts like that keep popping into my mind at night and it keeps me up at night." ||
            botOutput == "Well I remember being taught the dangers of falling into drug addiction in school and honestly it feels like another rabbit hole that I don’t want to fall in at this point." ||
            botOutput == "It makes life feel hopeless." ||
            botOutput == "It’s pretty sad actually. I remember there’s this one game I’ve played for years and spent over 1000 hours playing in. But now, I just don’t want to bother with it anymore though I do feel kind of bad about not seeing the friends I made in it. For some of them, the only way I had of contacting them was through that game’. I wonder how they’re doing’. I’m kind of scared to ask." ||
            botOutput == "It’s one of the things that keeps me up at night. It makes me feel sad and leaves me feeling like there’s no point in doing anything. I feel like I can’t go on living like this." ||
            botOutput == "Well, I would never actually try to carry out a suicide. I don’t want to hurt my friends and family after all." ||
            botOutput == "Trying to be perfect just made me feel like I’m inadequate. Like I’m not good enough’ I used to keep trying and redoing tasks to try and make them perfect. But eventually I realised that nothing I do will ever be good enough so I gave up trying." ||
			botOutput == "I want to but at the same time I’m not sure if I’m ready for the commitment.")
        {
			if (controller.scene != 0)
            {
				mood = 1;
            }
        }
        isSpeaking = true;

        Debug.Log(JsonConvert.SerializeObject(response, Formatting.Indented));
        Debug.Log(playerInput + "\n");
        Debug.Log("Chatbot said: " + botOutput + "\n");

		Debug.Log("Output contexts are ");
		for (int i = 0; i < response.queryResult.outputContexts.Length; i++)
        {
			Debug.Log(response.queryResult.outputContexts[i].Name + ", ");
        }

		//play chatbot response
		audioPlayer.clip = clip;
        audioPlayer.Play();
        StartCoroutine(finishedSpeaking());

        parseOutput(botOutput);
    }

    private void LogError(DF2ErrorResponse errorResponse)
	{
		Debug.LogError(string.Format("Error {0}: {1}", errorResponse.error.code.ToString(), errorResponse.error.message));
		databaseManager.addLog(errorResponse.error.message, "Error");
	}

	public void notReadyForChange ()
    {
		client.AddInputContext(new DF2Context("notready", 5), sessionName);
    }

	//public IEnumerator waitToSpeak()
 //   {
	//	yield return new WaitUntil(() => audioPlayer.isPlaying == false);
	//	isSpeaking = false;
	//}

	//public void sayGoodbye()
 //   {
	//	client.ReactToContext("phase2finish",
	//		 context => Debug.Log("Reacting to phase2 finish"));
	//}

	private void parseOutput (string response)
    {
		controller.changePrompt(response);
    }

	public void removeOutputContext(string context)
    {
		client.StopReactingToContext(context);
    }

	//hardcoded reset of all possible contexts in phase1
	//in an attempt for better conversational flow
	//this may or may not be necessary based on the quality of
	//context logic of the bot
	public void resetAllOutputContexts()
    {
		client.StopReactingToContext("phase1");
		client.StopReactingToContext("reflections");
		client.StopReactingToContext("phase1changeinappetite-followup");
		client.StopReactingToContext("phase1Doyoufeelphysicallywornout-followup");
		client.StopReactingToContext("phase1Haveyoubeenfeelingsadanxiousoranemptymoodlately-followup");
		client.StopReactingToContext("phase1Haveyouexperiencedalackofconfidencelately-followup");
		client.StopReactingToContext("phase1Haveyouexperiencedfeelingsofhopelessnesslately-followup");
		client.StopReactingToContext("phase1Haveyouexperiencedfeelingsofperfectionismlately-followup");
		client.StopReactingToContext("phase1Haveyouexperiencedthoughtsofdeathorsuiciderecently-followup");
		client.StopReactingToContext("phase1Haveyouexperiencedtroubleconcentratinglately-followup");
		client.StopReactingToContext("phase1Haveyoutakenanydrugsoralcoholbecauseofthis-followup");
		client.StopReactingToContext("phase1Haveyoubeenexperiencingissueswithsleeplately-followup");
		client.StopReactingToContext("phase1lossofinterestorpleasureinhobbies-followup");
		client.StopReactingToContext("notready");
		client.StopReactingToContext("reflectionsfallback");
		client.StopReactingToContext("phase1whatarewedoingtoday-custom-followup");
		client.StopReactingToContext("phase1whatarewedoingtoday-followup");
		client.StopReactingToContext("learningbyteaching1");
		client.StopReactingToContext("phase1whatseemstobeyourproblem-followup");
		client.StopReactingToContext("phase1whyarewedoingthis-followup");
		client.StopReactingToContext("phase1whatarewedoingtoday-custom-followup");
		client.StopReactingToContext("phase12readyforchange-followup");
	}

	public void setSittingState(int state)
    {
		sittingState = state;
    }

	public void initiatePhaseContext(int phaseNum)
    {
		string phase = "phase" + phaseNum;
		client.AddInputContext(new DF2Context(phase, 5), sessionName);
    }

	public void clearSession ()
    {
		client.ClearSession(sessionName);
    }
}
