// the following code is derived from the original website files from 2020
// namely Website/app/static/js/streamwebcam.js (misleading name for all the js logic)
// also sprinkled with derivations from main.py (but most of the dialogflow logic has been 
// placed in Assets/Scripts/agent.cs)

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class gameStateController : MonoBehaviour
{
    public agent agent;
    [SerializeField] Text hintPrompt;

    [SerializeField] GameObject hint1;
    [SerializeField] GameObject hint2;
    [SerializeField] GameObject hint3;
    [SerializeField] GameObject hint4;
    [SerializeField] GameObject hint5;
    [SerializeField] GameObject hint6;
    [SerializeField] GameObject hint7;

    [SerializeField] GameObject instruction1;
    [SerializeField] GameObject instruction2;
    [SerializeField] GameObject instruction3;
    [SerializeField] GameObject instruction4;

    [SerializeField] GameObject objective1;
    [SerializeField] GameObject objective2;
    [SerializeField] GameObject about1;
    [SerializeField] GameObject about2;
    [SerializeField] GameObject note1;
    [SerializeField] GameObject note2;

    [SerializeField] GameObject phase2instr;
    [SerializeField] GameObject finish;
    [SerializeField] GameObject fadePanel;
    public fade fadeAnimation;

    int reflectionCounter;
    int cbtCounter;
    public keyLogger keyLogger;
    public databaseManager databaseManager;

    public Light light;
    public ParticleSystem rainPS;

    public AudioSource rainAudioPlayer;
    public AudioClip rain1;
    public AudioClip rain2;

    public AudioSource bgmAudioPlayer;
    public AudioClip bgm1;
    public AudioClip bgm2;

    public int scene;

    private void Start()
    {
        reflectionCounter = 0;
        cbtCounter = 0;
        if (scene == 0)
        {
            agent.setSittingState(2);
        }
        if (scene == 2)
        {
            light.color = new Color(77/255f, 72/255f, 60/255f);
        }
        //startPhase2();
    }
    public void changePrompt(string botResponse)
    {
        Debug.Log(botResponse); 
        var emission = rainPS.emission;

        switch (botResponse)
        {
            //learning by teaching
            case "I¡¯ve felt like this for months. It¡¯s gotten really unbearable lately.":
            case "I¡¯ve been feeling restless because I have no motivation to do anything but at the same time, I know I should be doing something productive with my life and so I hate myself for that.":
                keyLogger.addPhase(1);
                databaseManager.addPhase(1);
                keyLogger.addLog("learning by teaching session 1");
                databaseManager.addLog("learning by teaching session 1", "Game State");
                Debug.Log("learning by teaching session 1");
                resetHints();
                hint7.SetActive(true);
                hintPrompt.text = "Talk about the activity we're going to do today (Try reading the About Tab)";
                break;
            case "Ok, so from what I understand we are going to do some talk therapy that can promote self reflection and enhance the effects of other treatments. To be honest I was having second thoughts about coming here. I think this can help me. Shall we start?":
                Debug.Log("removing hint1/prompt1 loading hint2/prompt2");
                resetHints();
                hint2.SetActive(true);
                hintPrompt.text = "e.g. \"Have you had problems sleeping lately?\" -> \"How does this make you feel?\" Check the Useful Notes Tab";
                agent.removeOutputContext("phase1whyarewedoingthis-followup");
                emission.rateOverTime = 300.0f;
                break;
            //reflections
            case "I used to eat three meals a day. Now I eat 2 and sometimes even less than that?":
            case "Yes, for months actually. I¡¯ve been feeling so exhausted from lack of sleep.":
            case "Yes, I have actually. I just feel so worried about things lately that my thoughts keep me up at night":
            case "Well yeah¡¯. That¡¯s why I¡¯m here.":
            case "Yeah¡¯ It¡¯s hard for me to feel confident about myself. Lately I feel like there¡¯s no point in doing anything because whatever I do won¡¯t be perfect and there are probably tons of people out there who could do it better than me.":
            case "Yeah, I¡¯ve felt like nothing I do really matters.":
            case "No¡¯. Even if I try to be perfect, I know my best isn¡¯t enough¡¯. So I sort of gave up.":
            case "No. Though occasionally I do get the feeling that death might not be such a scary thing after all.":
            case "Yes. I¡­ It¡¯s a bit of an embarrassing story but I actually blanked out before on my commute to university. I got on the wrong train and didn¡¯t realise until my trip was halfway done.":
            case "No¡¯ It¡¯s tempting but no.":
            case "Yeah, I used to be really into playing games on my computer. Lately though I just can¡¯t be bothered with it.":
                hintPrompt.text = "\"How do you feel about that?\"";
                break;
            case "I feel like I¡¯ve lost some weight lately. Actually, come to think of it, I¡¯ve also been feeling really fatigued lately. This might be one of the reasons behind that.":
            case "It makes me feel worn down. Like I wouldn¡¯t be able to last much longer like this but I can¡¯t see a light at the end of the tunnel so everything feels hopeless.":
            case "I just feel so tired from all this. Even on the days that I do get some decent sleep I still feel fatigue when I wake up. I¡­ I just feel like I can¡¯t go on living like this.":
            case "I¡­ I need to improve. It¡¯s not just minor things like commuting that this has affected me in. I feel like my mind is slow to react in thinks like my work place and classrooms as well and I really don¡¯t like it.":
            case "Trying to be perfect just made me feel like like I¡¯m inadequate. Like I¡¯m not good enough¡¯ I used to keep trying and redoing tasks to try and make them perfect. But eventually I realised that nothing I do will ever be good enough so I gave up trying.":
            case "It makes me feel worthless. But at the same time I feel like it might be true. It¡¯s like, I¡¯m worthless and everything I do is so there¡¯s no point in my trying to do anything. Thoughts like that keep popping into my mind at night and it keeps me up at night.":
            case "Well I remember being taught the dangers of falling into drug addiction in school and honestly it feels like another rabbit hole that I don¡¯t want to fall in at this point.":
            case "It makes life feel hopeless.":
            case "It¡¯s pretty sad actually. I remember there¡¯s this one game I¡¯ve played for years and spent over 1000 hours playing in. But now, I just don¡¯t want to bother with it anymore though I do feel kind of bad about not seeing the friends I made in it. For some of them, the only way I had of contacting them was through that game¡¯. I wonder how they¡¯re doing¡¯. I¡¯m kind of scared to ask.":
            case "It¡¯s one of the things that keeps me up at night. It makes me feel sad and leaves me feeling like there¡¯s no point in doing anything. I feel like I can¡¯t go on living like this.":
            case "Well, I would never actually try to carry out a suicide. I don¡¯t want to hurt my friends and family after all.":
            case "Trying to be perfect just made me feel like I¡¯m inadequate. Like I¡¯m not good enough¡¯ I used to keep trying and redoing tasks to try and make them perfect. But eventually I realised that nothing I do will ever be good enough so I gave up trying.":
                reflectionCounter++;
                keyLogger.addLog("Reflections = " + reflectionCounter);
                databaseManager.addLog("Reflections = " + reflectionCounter, "Game State");
                Debug.Log("reflections = " + reflectionCounter);
                hintPrompt.text = "e.g. \"Have you had problems ...?\" -> \"How does this make you feel?\"   Check the Useful Notes Tab";
                if (reflectionCounter >= 2)
                {
                    Debug.Log("Reflections >= 2");
                    keyLogger.addLog("Reflections = " + reflectionCounter);
                    databaseManager.addLog("Reflections = " + reflectionCounter, "Game State");
                    resetHints();
                    hint3.SetActive(true);
                    hintPrompt.text = "\"Do you think you're ready to change?\"";

                    if (scene > 0)
                    {
                        agent.setSittingState(1);
                        if (scene == 2)
                        {
                            light.color = new Color(118/255f, 113/255f, 100/255f);
                        }
                    }
                    emission.rateOverTime = 100.0f;
                }
                break;
            case "Well, I¡¯m here because I don¡¯t want to keep living like that but I think medication is a bit too much for me to want to commit at this stage. I've heard that you could teach me coping techniques and counseling though and I think I'm ready for that.":
                hintPrompt.text = "\"Do you want to change?\"";

                if (reflectionCounter < 2)
                {
                    agent.notReadyForChange();
                    agent.removeOutputContext(" phase12readyforchange-followup");
                    keyLogger.addLog("Reflection = " + reflectionCounter + "Bot not ready to change");
                    databaseManager.addLog("Reflection = " + reflectionCounter + "Bot not ready to change", "Game State");
                    Debug.Log("Reflection = " + reflectionCounter + "Bot not ready to change\n");
                }

                break;
            case "I want to but at the same time I¡¯m not sure if I¡¯m ready for the commitment.":
                int reflectionsToDo = 2 - reflectionCounter;
                hintPrompt.text = "It¡¯s possible the patient may change their mind. Try getting them to reflect on their experiences "
                    + reflectionsToDo + " more time(s)+ before revisiting this topic again.";
                break;
            case "I want to change":
                StartCoroutine(agent.finishedSpeaking());
                startPhase2(); 
                reflectionCounter = 0;
                if (scene == 2)
                {
                    rainAudioPlayer.clip = rain2;
                    rainAudioPlayer.volume = 0.12f;
                    rainAudioPlayer.Play();
                    bgmAudioPlayer.clip = bgm2;
                    bgmAudioPlayer.volume = 0.7f;
                    bgmAudioPlayer.Play();
                }
                break;
            //phase 2 - Laddering
            case "Ok, so from my understanding, we are going to do some therapy activities that can help me identify and manage negative thoughts. The idea behind it is that my beliefs act sort of like a filter or a lens though which I view the world so having negative beliefs all the time makes the world seem depressing and scary. The goal of this session seems to be teaching me how to identify negative thoughts and try and challenge them or turn them into positive ones. Shall we start then?":
                keyLogger.addLog("learning by teaching session 2");
                databaseManager.addLog("learning by teaching session 2", "Game State");
                Debug.Log("learning by teaching session 2");
                resetHints();
                hint4.SetActive(true);
                hintPrompt.text = "\"Let¡¯s talk about your sleeping problems/anxiety/fatigue etc?\"";
                emission.rateOverTime = 70.0f;
                break;
            case "I¡¯m just worried and can¡¯t sleep. Its nothing specific really, I¡¯m just worried about a bunch of random things. Like how the future seems so uncertain. about how I don¡¯t talk with my friends anymore. About how I don¡¯t know what to do with my life.":
            case "I just feel empty all the time and sometimes I wonder what's the point of living life. Nothing seems fun anymore to me either. I used to read books and play games. But now I just don't get out any joy from it.":
            case "I feel like I need to do things perfectly or not at all. It eats away at me and in the end I never end up finishing anything that I want to do.":
            case "I¡¯ve been feeling depressed lately because I used to be really close to some friends of mine but now we don¡¯t really talk to each other that much and the last time we met up in person was a year ago.":
            case "I feel like everything in life just feels so hopeless nowadays. There's just so much going on. Its all chaos in my life and in the greater world and I don't know how I'm going to handle it all.":
            case "I feel tired all the time. I feel like there¡¯s so many issues with life that I have to deal with and I don¡¯t feel I¡¯m ready to deal with it all.":
            case "I feel like its hard for me to feel confident about myself. I just feel really unsure, especially when I have to talk to other people.":
            case "I have noticed that I have trouble concentrating at times. Like my mind just blanks out on me and I forget most of what I experienced in the past few minutes. Its like my body goes on auto pilot but my mind isn't processing it all. Part of this could be due to how tired I always feel nowadays.  I believe this is developing into a problem.":
            case "I've noticed that I've been eating a lot less these days. Some of this is due to me trying to manage my time and failing and some of this is just due to not being hungry. I feel this is sort of behaviour could be unhealthy but at the same time I feel like its what I deserve.":
                keyLogger.addLog("Laddering");
                databaseManager.addLog("Laddering", "Game State");
                hintPrompt.text = "\"Why is that?\"";
                break;
            case "I feel like I¡¯m not capable enough to handle the uncertainty.":
            case "I never end up finishing the things I want to do because they're not perfect to me. So I scrap what I'm doing and try to redo it only to scrap it again. I feel like I'm stuck in a cycle that I can't escape from.":
            case "I feel scared of interacting with people unless I know I have some accomplishments that I can be proud of.":
            case "I still really like my friends but I¡¯m afraid of us drifting apart. I am afraid that if I don¡¯t talk to them regularly my friends will forget about me. Despite all that, I¡¯m also kind of scared of talking to them again. Maybe they don¡¯t consider themselves to be friends with me anymore.":
            case "It just feels like a chore to partake in my hobbies now. It feels like something I shouldn't be doing.":
            case "I don¡¯t feel like I¡¯m ready to deal with all of my life¡¯s issues but at the same time I know that I have to. So I tell my self I¡¯ll just handle things by sacrificing some time sleeping less or eating less but in the end I can¡¯t stop myself from procrastinating and hating myself even more.":
            case "I feel like there's just so many things I need to do and not enough time to do it all. I'm just so overwhelmed and everything I try to do just falls apart.":
            case "A small part of me feels a twisted sense of satisfaction with how bad this makes me feels. Its sort of like a form of punishment for how much I hate myself.":
            case "I feel exhausted. I feel sleepy. I feel like life is just best handled if I didn't have to think through my actions at all. I just have so many things I have to do.":
                keyLogger.addLog("Laddering");
                databaseManager.addLog("Laddering", "Game State");
                hintPrompt.text = "\"Why is that?\" (Laddering)";
                break;
            //case "¡¯Placeholder Core belief¡¯":
            case "I¡¯m not good enough.":
            case "I'm boring.":
            case "I need to suffer because I don't meet my own standards":
            case "Life is full of stress and overload.":
                keyLogger.addLog("Core belief");
                databaseManager.addLog("Core belief", "Game State");
                hintPrompt.text = "\"Can you think of an event that makes you feel this way?\"";
                break;
            //case "¡¯Place holder activating event¡¯":
            case "Even when I try my best at doing something, I always eventually realise that there are other people who are better than me or get better results.":
            case "I haven't had contact with the friends I used to be close with in a while.":
            case "I lost contact with some friends that I used to be close with.":
            case "I spend time I could have spent eating and sleeping on simply procrastinating.":
            case "I make a perfect schedule of all the things I need to do but I screw up with keeping it which causes everything to fall apart like a line of dominoes.":
            case "I play a game but I can't enjoy it because I keep thinking about how my time could be better spent doing things to improve myself or my life.":
                keyLogger.addLog("Laddering A: activating event");
                databaseManager.addLog("Laddering A: activating event", "Game State");
                hintPrompt.text = "\"What sort of thoughts do you associate with this event?\"";
                break;
            //case "¡¯Placeholder belief¡¯":
            case "I¡¯m not anything special. I will never be the best at anything. I don¡¯t have any talent.":
            case "I'm not an interesting person¡­ I don't have anything that makes me stand out from others. Why should my friends spend time with me when there are more amazing people in the world they could be spending their time with.":
            case "I feel like I'm procrastinating. I feel like I shouldn't be having fun but there are more important things waiting for me.":
            case "I¡¯m not an interesting person¡­ No one likes me and no one will ever like me¡­ Maybe I was the only one who thought that we were friends¡­.":
            case "I have really weak willpower. I can't even stay on task for something I set myself to do. I am a terrible person with no integrity.":
            case "It makes me believe that I don't have the capability or willpower to handle anything. I'm not able to handle things that I should be able to.":
                keyLogger.addLog("Laddering B: belief");
                databaseManager.addLog("Laddering B: belief", "Game State");
                hintPrompt.text = "\"What effect do these beliefs have on you as a consequence?\"";
                break;
            //case "¡¯Placeholder consequence¡¯":
            case "Those thoughts make me feel depressed. It makes me feel worthless. It makes me feel as if there¡¯s no point in trying to do anything.":
            case "I¡­I feel depressed about it. It¡¯s one of the things that worry me. It¡¯s one of the things that keep me up at night¡­ Because of that¡­ I¡­ I don¡¯t get much sleep and I don¡¯t feel rested when I do manage to sleep.":
            case "I have a miserable experience playing the game. I feel depressed and life feels empty because the things that used to bring me pleasure no longer do so. This brings down my motivation to do anything else.":
            case "As a result of these beliefs, I feel overcome with self-loathing and this then spirals out into self-pity.":
            case "I feel depressed because of these thoughts. It makes me think, if I'm such a boring person that not even my friends want to deal with me, wouldn't it be even worse for a stranger I've just met? What if its with someone I have to work with because of a job or an assignment or something else important?":
            case "I approach things expecting them to fail so I don't try my hardest. This becomes a self fulfilling prophecy where I fail things that I shouldn't if only I just tried hard enough.":
                keyLogger.addLog("Laddering C: consequence");
                databaseManager.addLog("Laddering C: consequence", "Game State");
                hintPrompt.text = "\"Can you think of a way to dispute this belief?\"";
                break;
            //case "¡¯Placeholder dispute¡¯":
            //case "Sorry, I blanked out. Could you repeat that?":
            //case "Sorry, could you say that again?":
            //case "Sorry, I couldn¡¯t quite catch that.":
            //case "Pardon but could you repeat that?":
            //case "Sorry, what was that?":
            //case "Pardon?":
                //keyLogger.addLog("Laddering fallback");
                //hintPrompt.text = "\"Let¡¯s talk about your sleeping problems/anxiety/fatigue etc?\"";
                //break;
            case "Although I feel like I¡¯m not achieving anything, that could be because I¡¯m comparing myself to the best of the best. I should be better than the average person in one of my areas of interest.":
            case "If I was a boring person, those friends wouldn¡¯t have interacted with me as much. We share similar hobbies and have played games with each other in the past.":
            case "Maintaining morale is an important part of ensuring the completion of any task so having fun can improve other aspects of my life. There are other people who have fun and achieve their goals, I should be able to do the same.":
            case "There are other people who undergo stressful and overloaded lives. There are people who fail to uphold promises they make to themselves but they manage to bounce back from it. I should be able to bounce back too.":
            case "If I was such a boring person, I probably wouldn't have managed to make friends with people in the first place. There are billions of people in the world, I must appear as interesting to at least one of them...":
            case "Uhm¡­ At least I'm trying to take the first steps I guess? I'm trying to improve this part of me instead of wallowing in despair. I¡­ I want to change. No, I need to change.":
                keyLogger.addLog("Laddering D: dispute");
                databaseManager.addLog("Laddering D: dispute", "Game State");
                hintPrompt.text = "\"Let¡¯s talk about your sleeping problems/anxiety/fatigue etc?\"";
                cbtCounter++;
                keyLogger.addLog("CBT Counter = " + cbtCounter);
                databaseManager.addLog("CBT Counter = " + cbtCounter, "Game State");
                Debug.Log("cbt counter is " + cbtCounter);
                if (cbtCounter >= 2)
                {
                    agent.setSittingState(2);
                    emission.rateOverTime = 3.0f; 
                    if (scene == 2)
                    {
                        light.color = new Color(207/255f, 201/255f, 183/255f);
                    }
                } else
                {
                    emission.rateOverTime = 30.0f;
                    if (scene == 2)
                    {
                        light.color = new Color(169/255f, 161/255f, 138/255f);
                    }
                }
                break;
            default:
                Debug.Log("no reaction needed");
                keyLogger.addLog("no reaction needed");
                databaseManager.addLog("no reaction needed", "Game State");
                break;
        }

        
        if (cbtCounter >= 2)
        {
            emission.rateOverTime = 0.0f;
            resetHints();
            finishGame();
        }
    }

    private void resetHints()
    {
        hint1.SetActive(false);
        hint2.SetActive(false);
        hint3.SetActive(false);
        hint4.SetActive(false);
        hint5.SetActive(false);
        hint6.SetActive(false);
        hint7.SetActive(false);

        instruction1.SetActive(false);
        instruction2.SetActive(false);
        instruction3.SetActive(false);
        instruction4.SetActive(false);
        phase2instr.SetActive(false);
    }

    private void phase2menu()
    {
        objective1.SetActive(false);
        objective2.SetActive(true);
        about1.SetActive(false);
        about2.SetActive(true);
        note1.SetActive(false);
        note2.SetActive(true);
    }

    private void startPhase2()
    {

        fadePanel.SetActive(true);
        fadeAnimation.fadeToNextPhase();
        keyLogger.addPhase(2);
        databaseManager.addPhase(2);
        //databaseManager.addLog("2", "Phase Started");
        hintPrompt.text = "Talk about the next activity (Try reading the About tab).";
        resetHints();

        phase2instr.SetActive(true);
        phase2menu();

        agent.resetAllOutputContexts();
        agent.initiatePhaseContext(2);
    }

    public void finishGame()
    {
        //StartCoroutine(agent.finishedSpeaking());
        //Debug.Log("HERE");
        //agent.sayGoodbye();
        //StartCoroutine(agent.finishedSpeaking());

        Debug.Log("Game completed");
        keyLogger.addLog("Game completed");
        databaseManager.addLog("Game completed", "Game State");
        resetHints();
        finish.SetActive(true);
    }
}