﻿using UnityEngine;
using System.Collections;

public class CleanBox : MonoBehaviour {

    public UIBarControl m_cleanUI;
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
        m_deCounter = m_pad.GetCleanTimer(); //

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
            m_pad.LoadInCleanCheck();
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
                m_pad.CleanDecrease("Clean Decrease");
            }
        }
    }

    void OnTriggerEnter()
    {
        m_deCounter = 0.0f;
        Debug.Log("CIN");
        m_counter += Time.deltaTime;
        m_cleanUI.Interaction(true);
        m_inter = true;
    }

    void OnTriggerStay()
    {
        Debug.Log("CSTAY");
        if (m_counter >= m_timer)
        {
            m_counter = 0.0f;
            m_pad.CleanIncrease("Clean Increase");
        }
        m_counter += Time.deltaTime;
        m_cleanUI.Interaction(true);
    }

    void OnTriggerExit()
    {
        Debug.Log("COUT");
        m_cleanUI.Interaction(false);
        m_counter = 0.0f;
        m_inter = false;
    }

    public float GetCurrentCounter()
    {
        return m_deCounter;
    }
}
