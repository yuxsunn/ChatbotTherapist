using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{

    Animator animator;
    agent agent;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<agent>();
        animator.SetInteger("sittingState", agent.sittingState);
        animator.SetInteger("mood", agent.mood);
    }

    void Update()
    {
        animator.SetInteger("sittingState", agent.sittingState);
        animator.SetInteger("mood", agent.mood);
        if (agent.isSpeaking)
        {
            animator.SetBool("isSpeaking", true);
        }

        if (!agent.isSpeaking)
        {
            animator.SetBool("isSpeaking", false);
        }
    }
}
