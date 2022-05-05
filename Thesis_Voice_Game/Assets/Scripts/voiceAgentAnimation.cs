using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voiceAgentAnimation : MonoBehaviour
{

    private agent agent;
    public GameObject speakingAnim;

    void Start()
    {
        agent = GetComponent<agent>();
    }

    void Update()
    {
        if (agent.isSpeaking)
        {
            speakingAnim.SetActive(true);

        }

        if (!agent.isSpeaking)
        {
            speakingAnim.SetActive(false);

        }
    }
}
