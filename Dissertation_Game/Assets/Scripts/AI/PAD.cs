using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PAD : MonoBehaviour {

    public AnimatorTestScript m_animator;
    public AITESTTEXTSCRIPT m_Text;

    public PlayerInputTest2 m_inputTest2;
    public FoodBowl m_foodbowl;
    public WaterBottle m_waterbottle;
    public CleanBox m_cleanbox;

    public UIBarControl m_UIfood;
    public UIBarControl m_UIwater;
    public UIBarControl m_UIclean;


    const float m_minimumChange = 0.01f;
    const float m_maxChange = 0.03f;

    public float m_pleasure = 0.0f,
                  m_arousal = 0.0f,
                  m_dominance = 0.0f;

    private const float m_min = -1.0f,
                        m_max = 1.0f;

    public enum CURRENT_EMOTION {
        Exuberant,
        Dependant,
        Relaxed,
        Docile,
        Bored,
        Disdainful,
        Anxious,
        Hostile,
        PassiveHappy, 
        PassiveSad,
        Neutral
    }

    [Serializable]
    public class HamsterData
    {
        public float Pleasure, Arousal, Dominance; //The PAD values
        public float FoodHealth, WaterHealth, CleanHealth; //current health of the UI
        public float FoodTimer, WaterTimer, CleanTimer, InterTimer; //Countdown timers
        public float UIFoodTimer, UIWaterTimer, UICleanTimer;
        public float Hours, Minutes, Seconds; //Time of ending
        public CURRENT_EMOTION Emotion; //Current Emotion 
    }

    List<string> txtTime;
    List<float> txtP;
    List<float> txtA;
    List<float> txtD;
    List<CURRENT_EMOTION> txtEmotion;
    List<string> txtAction;

    StreamWriter writer;

    CURRENT_EMOTION m_emotion = CURRENT_EMOTION.Neutral;

    float totalTimeMissedInSecs = 0;
    private HamsterData m_hamsterdata;

    private bool m_startup = false;

    void Awake()
    {
        m_hamsterdata = new HamsterData();

        LoadData(); //Get past data

        //Calculate the time it took from 
        //when the game last ended to when it began 
        //and see how many seconds have passed since then
        float OldHours = m_hamsterdata.Hours;
        float OldMinutes = m_hamsterdata.Minutes;
        float OldSeconds = m_hamsterdata.Seconds;

        float SystemHour = System.DateTime.Now.Hour;
        float SystemMinute = System.DateTime.Now.Minute;
        float SystemSecond = System.DateTime.Now.Second;

        //If the time passes midnight, the hour turns to 0.
        //This means that we have to make sure that the total 
        //is more than OldHours. 
        if (SystemHour < OldHours) SystemHour += 24.0f; 

        float difhour = SystemHour - OldHours;
        float difmin = SystemMinute - OldMinutes;
        float difsec = SystemSecond - OldSeconds;

        //Make sure the time is positive
        if (difhour < 0.0f) difhour = -difhour;
        if (difmin < 0.0f) difmin = -difmin;
        if (difsec < 0.0f) difsec = -difsec;

        float hourinsecs = ((difhour * 60) * 60);
        float minuteinsecs = (difmin * 60);

        totalTimeMissedInSecs = hourinsecs + minuteinsecs + difsec;

        //Set up Starting Emotions
        m_pleasure = m_hamsterdata.Pleasure;
        m_dominance = m_hamsterdata.Dominance;
        m_arousal = m_hamsterdata.Arousal;
        m_emotion = m_hamsterdata.Emotion;
    }

    void Start()
    {
        string path = Application.persistentDataPath + "/Data.txt";
        
        writer = new StreamWriter(path, true);

        writer.WriteLine("  Time     P   A   D   Emotion   Action");

        txtP = new List<float>();
        txtA = new List<float>();
        txtD = new List<float>();
        txtEmotion = new List<CURRENT_EMOTION>();
        txtAction = new List<string>();
        txtTime = new List<string>();
    }

    void Update()
    {
        if (m_startup == false)
        {
            m_startup = true;
            Check("Start Up");
        }
    }

    void Check(string _action)
    {
        ErrorChecking();

        if (m_pleasure == 0.0f && m_arousal == 0.0f && m_dominance == 0.0f)
        {
            //Neutral
            m_emotion = CURRENT_EMOTION.Neutral;
            Debug.Log(m_emotion.ToString());
            //m_animator.SetCurrentEmotion(0);
            m_Text.SetDisplay(m_emotion.ToString() +
                " P: " + m_pleasure.ToString() +
                " A: " + m_arousal.ToString() +
                " D: " + m_dominance.ToString());
        }
        else if ((m_pleasure >= 0.0f && m_pleasure <= 0.03f) &&
            (m_arousal >= 0.0f && m_arousal <= 0.05f) &&
            (m_dominance >= 0.0f && m_arousal <= 0.05f))
        {
            //passive Happy
            m_emotion = CURRENT_EMOTION.PassiveHappy;
            Debug.Log(m_emotion.ToString());
            //m_animator.SetCurrentEmotion(0);
            m_Text.SetDisplay(m_emotion.ToString() +
                " P: " + m_pleasure.ToString() +
                " A: " + m_arousal.ToString() +
                " D: " + m_dominance.ToString());
        }
        else if ((m_pleasure <= 0.0f && m_pleasure >= -0.03f) &&
            (m_arousal <= 0.0f && m_arousal >= -0.05f) &&
            (m_dominance <= 0.0f && m_arousal >= -0.05f))
        {
            //Passive Sad
            m_emotion = CURRENT_EMOTION.PassiveSad;
            Debug.Log(m_emotion.ToString());
            //m_animator.SetCurrentEmotion(0);
            m_Text.SetDisplay(m_emotion.ToString() +
                " P: " + m_pleasure.ToString() +
                " A: " + m_arousal.ToString() +
                " D: " + m_dominance.ToString());
        }


        // HAPPY EMOTIONS
        else if (m_pleasure > 0.0f && m_arousal > 0.0f && m_dominance > 0.0f)
        {
            //Exuberant
            m_emotion = CURRENT_EMOTION.Exuberant;
            Debug.Log(m_emotion.ToString());
            //m_animator.SetCurrentEmotion(0);
            m_Text.SetDisplay(m_emotion.ToString() +
                " P: " + m_pleasure.ToString() +
                " A: " + m_arousal.ToString() +
                " D: " + m_dominance.ToString());
        }
        else if (m_pleasure > 0.0f && m_arousal > 0.0f && m_dominance <= 0.0f)
        {
            //Dependant
            m_emotion = CURRENT_EMOTION.Dependant;
            Debug.Log(m_emotion.ToString());
            //m_animator.SetCurrentEmotion(0);
            m_Text.SetDisplay(m_emotion.ToString() +
                " P: " + m_pleasure.ToString() +
                " A: " + m_arousal.ToString() +
                " D: " + m_dominance.ToString());
        }
        else if (m_pleasure > 0.0f && m_arousal <= 0.0f && m_dominance > 0.0f)
        {
            //Relaxed 
            m_emotion = CURRENT_EMOTION.Relaxed;
            Debug.Log(m_emotion.ToString());
            //m_animator.SetCurrentEmotion(0);
            m_Text.SetDisplay(m_emotion.ToString() +
                " P: " + m_pleasure.ToString() +
                " A: " + m_arousal.ToString() +
                " D: " + m_dominance.ToString());
        }
        else if (m_pleasure > 0.0f && m_arousal <= 0.0f && m_dominance <= 0.0f)
        {
            //Docile
            m_emotion = CURRENT_EMOTION.Docile;
            Debug.Log(m_emotion.ToString());
            //m_animator.SetCurrentEmotion(0);
            m_Text.SetDisplay(m_emotion.ToString() +
                " P: " + m_pleasure.ToString() +
                " A: " + m_arousal.ToString() +
                " D: " + m_dominance.ToString());
        }


        //SAD EMOTIONS
        else if (m_pleasure < -0.0f && m_arousal < -0.0f && m_dominance < -0.0f)
        {
            //Bored
            m_emotion = CURRENT_EMOTION.Bored;
            Debug.Log(m_emotion.ToString());
            //m_animator.SetCurrentEmotion(0);
            m_Text.SetDisplay(m_emotion.ToString() +
                " P: " + m_pleasure.ToString() +
                " A: " + m_arousal.ToString() +
                " D: " + m_dominance.ToString());
        }
        else if (m_pleasure < -0.0f && m_arousal < -0.0f && m_dominance >= 0.0f)
        {
            //Disdainful 
            m_emotion = CURRENT_EMOTION.Disdainful;
            Debug.Log(m_emotion.ToString());
            //m_animator.SetCurrentEmotion(0);
            m_Text.SetDisplay(m_emotion.ToString() +
                " P: " + m_pleasure.ToString() +
                " A: " + m_arousal.ToString() +
                " D: " + m_dominance.ToString());
        }
        else if (m_pleasure < -0.0f && m_arousal >= 0.0f && m_dominance < -0.0f)
        {
            //Anxious
            m_emotion = CURRENT_EMOTION.Anxious;
            Debug.Log(m_emotion.ToString());
            //m_animator.SetCurrentEmotion(0);
            m_Text.SetDisplay(m_emotion.ToString() +
                " P: " + m_pleasure.ToString() +
                " A: " + m_arousal.ToString() +
                " D: " + m_dominance.ToString());
        }
        else if (m_pleasure < -0.0f && m_arousal >= 0.0f && m_dominance >= 0.0f)
        {
            //Hostile
            m_emotion = CURRENT_EMOTION.Hostile;
            Debug.Log(m_emotion.ToString());
            //m_animator.SetCurrentEmotion(0);
            m_Text.SetDisplay(m_emotion.ToString() +
                " P: " + m_pleasure.ToString() +
                " A: " + m_arousal.ToString() +
                " D: " + m_dominance.ToString());
        }

        float hour = System.DateTime.Now.Hour;
        float min = System.DateTime.Now.Minute;
        float sec = System.DateTime.Now.Second;
        txtTime.Add(hour + ":" + min + ":" + sec);

        //txtTime.Add(System.DateTime.Now.TimeOfDay.ToString());
        txtP.Add(m_pleasure);
        txtA.Add(m_arousal);
        txtD.Add(m_dominance);
        txtEmotion.Add(m_emotion);
        txtAction.Add(_action);
    }

    public void FoodIncrease(string _action)
    {
        m_arousal += m_maxChange;
        m_pleasure += m_minimumChange;
        Check(_action);
    }

    public void FoodDecrease(string _action)
    {
        m_arousal -= m_minimumChange;
        m_pleasure -= m_minimumChange;
        Check(_action);
    }

    public void DrinkIncrease(string _action)
    {
        m_arousal += m_minimumChange;
        m_dominance += m_minimumChange;
        Check(_action);
    }

    public void DrinkDecrease(string _action)
    {
        m_arousal -= m_minimumChange;
        m_dominance -= m_minimumChange;
        Check(_action);
    }

    public void CleanIncrease(string _action)
    {
        m_dominance += m_minimumChange;
        Check(_action);
    }

    public void CleanDecrease(string _action)
    {
        m_dominance -= m_minimumChange;
        Check(_action);
    }

    public void Interactions(string _action)
    {
        m_pleasure += m_maxChange;
        Check(_action);
        StartCoroutine(InterTimer());
    }

    public void NoInteractions(string _action)
    {
        m_pleasure -= m_maxChange;
        Check(_action);
        StartCoroutine(InterTimer());
    }

    public void LoadInFeedCheck()
    {
        m_arousal -= m_minimumChange;
        m_pleasure -= m_minimumChange;
    }

    public void LoadInWaterCheck()
    {
        m_arousal -= m_minimumChange;
        m_dominance -= m_minimumChange;
    }

    public void LoadInCleanCheck()
    {
        m_dominance -= m_minimumChange;
    }

    public void LoadInInterCheck()
    {
        m_pleasure -= m_maxChange;
    }

    void ErrorChecking()
    {
        if (m_pleasure > m_max) m_pleasure = m_max;
        else if (m_pleasure < m_min) m_pleasure = m_min;
        if (m_arousal > m_max) m_arousal = m_max;
        else if (m_arousal < m_min) m_arousal = m_min;
        if (m_dominance > m_max) m_dominance = m_max;
        else if (m_dominance < m_min) m_dominance = m_min;
    }

    public void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/HamsterData.dat"))
        {
            BinaryFormatter binaryformatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/HamsterData.dat", FileMode.Open);
            m_hamsterdata = (HamsterData)binaryformatter.Deserialize(file);
            file.Close();
        }
        else
        {
            m_hamsterdata.Pleasure = 0.0f;
            m_hamsterdata.Arousal = 0.0f;
            m_hamsterdata.Dominance = 0.0f;
            m_hamsterdata.FoodHealth = 100.0f;
            m_hamsterdata.WaterHealth = 100.0f;
            m_hamsterdata.CleanHealth = 100.0f;
            m_hamsterdata.FoodTimer = 0.0f;
            m_hamsterdata.CleanTimer = 0.0f;
            m_hamsterdata.WaterTimer = 0.0f;
            m_hamsterdata.InterTimer = 0.0f;
            m_hamsterdata.UIFoodTimer = 0.0f;
            m_hamsterdata.UIWaterTimer = 0.0f;
            m_hamsterdata.UICleanTimer = 0.0f;
            m_hamsterdata.Hours = System.DateTime.Now.Hour;
            m_hamsterdata.Minutes = System.DateTime.Now.Minute;
            m_hamsterdata.Seconds = System.DateTime.Now.Second;
            m_hamsterdata.Emotion = CURRENT_EMOTION.Neutral;
            SavedData();
        }
    }

    public void SavedData()
    {
        BinaryFormatter binaryformatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/HamsterData.dat");
        binaryformatter.Serialize(file, m_hamsterdata);
        file.Close();
    }

    public void SetUpSavedData()
    { 
        m_hamsterdata.Pleasure = m_pleasure;
        m_hamsterdata.Arousal = m_arousal;
        m_hamsterdata.Dominance = m_dominance;
        m_hamsterdata.Emotion = m_emotion;
        m_hamsterdata.FoodHealth = m_UIfood.GetCurrentHealth();
        m_hamsterdata.WaterHealth = m_UIwater.GetCurrentHealth();
        m_hamsterdata.CleanHealth = m_UIclean.GetCurrentHealth();
        m_hamsterdata.UIFoodTimer = m_UIfood.GetCurrentCounter();
        m_hamsterdata.UIWaterTimer = m_UIwater.GetCurrentCounter();
        m_hamsterdata.UICleanTimer = m_UIclean.GetCurrentCounter();
        m_hamsterdata.FoodTimer = m_foodbowl.GetCurrentCounter();
        m_hamsterdata.WaterTimer = m_waterbottle.GetCurrentCounter();
        m_hamsterdata.CleanTimer = m_cleanbox.GetCurrentCounter();
        m_hamsterdata.InterTimer = m_inputTest2.GetInterCounter();
        m_hamsterdata.Hours = System.DateTime.Now.Hour;
        m_hamsterdata.Minutes = System.DateTime.Now.Minute;
        m_hamsterdata.Seconds = System.DateTime.Now.Second;
    }

    void OnApplicationQuit()
    {
        SetUpSavedData();
        SavedData();

        for (int i = 0; i < txtP.Count; i++)
        {
            writer.WriteLine(txtTime[i] + "   " + 
                             txtP[i] + "   " + 
                             txtA[i] + "   " + 
                             txtD[i] + "   " + 
                             txtEmotion[i] + "   " + 
                             txtAction[i]);
        }

        writer.WriteLine("=============================================================");

        writer.Close();
    }

    IEnumerator InterTimer()
    {
        m_animator.SetPassive(true);
        switch(m_emotion)
        {
            case CURRENT_EMOTION.Neutral:
                m_animator.SetCurrentEmotion(0);
                break;
            case CURRENT_EMOTION.PassiveHappy:
                m_animator.SetCurrentEmotion(1);
                break;
            case CURRENT_EMOTION.PassiveSad:
                m_animator.SetCurrentEmotion(2);
                break;
            case CURRENT_EMOTION.Exuberant:
                m_animator.SetCurrentEmotion(3);
                break;
            case CURRENT_EMOTION.Dependant:
                m_animator.SetCurrentEmotion(4);
                break;
            case CURRENT_EMOTION.Relaxed:
                m_animator.SetCurrentEmotion(5);
                break;
            case CURRENT_EMOTION.Docile:
                m_animator.SetCurrentEmotion(6);
                break;
            case CURRENT_EMOTION.Bored:
                m_animator.SetCurrentEmotion(7);
                break;
            case CURRENT_EMOTION.Disdainful:
                m_animator.SetCurrentEmotion(8);
                break;
            case CURRENT_EMOTION.Anxious:
                m_animator.SetCurrentEmotion(9);
                break;
            case CURRENT_EMOTION.Hostile:
                m_animator.SetCurrentEmotion(10);
                break;
            default:
                m_animator.SetCurrentEmotion(0);
                break;
        }
        yield return new WaitForSeconds(0.5f);
        m_animator.SetPassive(false);
        m_animator.SetCurrentEmotion(0);
    }

    public float getTotalTimeMissed()
    {
        return totalTimeMissedInSecs;
    }

    public float GetFoodHealth()
    {
        return m_hamsterdata.FoodHealth;
    }

    public float GetWaterHealth()
    {
        return m_hamsterdata.WaterHealth;
    }

    public float GetCleanHealth()
    {
        return m_hamsterdata.CleanHealth;
    }

    public float GetFoodTimer()
    {
        return m_hamsterdata.FoodTimer;
    }

    public float GetWaterTimer()
    {
        return m_hamsterdata.WaterTimer;
    }

    public float GetCleanTimer()
    {
        return m_hamsterdata.CleanTimer;
    }

    public float GetInteractionTimer()
    {
        return m_hamsterdata.InterTimer;
    }

    public float GetUIFoodtimer()
    {
        return m_hamsterdata.UIFoodTimer;
    }

    public float GetUIWatertimer()
    {
        return m_hamsterdata.UIWaterTimer;
    }

    public float GetUICleantimer()
    {
        return m_hamsterdata.UICleanTimer;
    }
}

