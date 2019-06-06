using UnityEngine;
using System.Collections;

public class WaterBottle : MonoBehaviour {

    //Water 5 Times a Day

    public UIBarControl m_waterUI;
    public PAD m_pad;

    //Food Increase
    [SerializeField] float m_timer = 0.0f;
    [SerializeField] float m_counter = 0.0f;

    //Food Decrease
    [SerializeField] float m_deCounter = 0.0f;
    [SerializeField] float m_deTimer = 0.0f;

    bool m_inter = false;

    void Start()
    {
        m_pad = GameObject.Find("Player").GetComponent<PAD>();

        //Saved current time
        m_deCounter = m_pad.GetWaterTimer();

        //Get the number of calls missed 
        float CallsMissed = m_pad.getTotalTimeMissed() / m_deTimer;
        int totalCallsMissed = (int)CallsMissed;
        //Get the decimal value 
        float Decimal = CallsMissed - totalCallsMissed;

        float TimeToAdd = m_deTimer * Decimal;

        m_deCounter += TimeToAdd;

        if (m_deCounter >= m_deTimer)
        {
            totalCallsMissed++;
            m_deCounter -= m_deTimer;
        }

        for (int i = 0; i < totalCallsMissed; i++)
        {
            m_pad.LoadInWaterCheck(); 
        }
    }

    void Update()
    {
        if (!m_inter)
        {
            m_deCounter += Time.deltaTime;

            if (m_deCounter >= m_deTimer)
            {
                m_deCounter = 0.0f;
                m_pad.DrinkDecrease("Drink Decrease");
            }
        }
    }

    void OnTriggerEnter()
    {
        m_deCounter = 0.0f;
        Debug.Log("WIN");
        m_counter += Time.deltaTime;
        m_waterUI.Interaction(true);
        m_inter = true;
    }

    void OnTriggerStay()
    {
        Debug.Log("WSTAY");
        if (m_counter >= m_timer)
        {
            m_counter = 0.0f;
            m_pad.DrinkIncrease("Drink Increase");
        }
        m_counter += Time.deltaTime;
        m_waterUI.Interaction(true);
    }

    void OnTriggerExit()
    {
        Debug.Log("WOUT");
        m_waterUI.Interaction(false);
        m_counter = 0.0f;
        m_inter = false;
    }

    public float GetCurrentCounter()
    {
        return m_deCounter;
    }
}
