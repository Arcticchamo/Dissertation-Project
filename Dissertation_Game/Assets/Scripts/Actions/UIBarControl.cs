using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class UIBarControl : MonoBehaviour {

    private const float m_maxHealth = 100.0f;
    private const float m_minHealth = 0.0f;
    private float m_repeatVar;
    private float m_decreaseVar;

    [SerializeField]
    private bool m_connected;

    public float m_deTimer; //Max Time
    public float m_deCounter = 0.0f;
    private float m_deTimer2;
    public float m_deTimerConnected;

    public float m_currentHealth;
    public float m_decreaseRate;
    public float m_repeatRate;
    public Image m_healthBar;
    public PAD m_pad;

    void Start()
    {
        m_deTimer2 = m_deTimer;
        m_connected = false;
        
        m_repeatVar = m_repeatRate;
        m_decreaseVar = m_decreaseRate;

        //Saved current time
        if (gameObject.name == "FoodUI") 
        {
            m_currentHealth = m_pad.GetFoodHealth();
            m_deCounter = m_pad.GetUIFoodtimer();
        }
        else if (gameObject.name == "CleanUI")
        {
            m_currentHealth = m_pad.GetCleanHealth();
            m_deCounter = m_pad.GetUICleantimer();
        }
        else if (gameObject.name == "WaterUI")
        {
            m_currentHealth = m_pad.GetWaterHealth();
            m_deCounter = m_pad.GetUIWatertimer();
        }

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
            m_currentHealth -= m_decreaseRate;
            m_repeatRate = m_repeatVar;
            m_decreaseRate = m_decreaseVar;
        }
    }

    void Update()
    {
        if (m_deCounter >= m_deTimer)
        {
            m_deCounter = 0.0f;

            if (!m_connected)
            {
                m_currentHealth -= m_decreaseRate;
                m_deTimer = m_deTimer2;
                m_decreaseRate = m_decreaseVar;
            }
            else if (m_connected)
            {
                m_currentHealth += m_decreaseRate * 3;
                m_deTimer = m_deTimerConnected;
                m_decreaseRate = 2.5f;
            }

            //Error Checking
            if (m_currentHealth > m_maxHealth) m_currentHealth = m_maxHealth;
            else if (m_currentHealth < m_minHealth) m_currentHealth = m_minHealth;

            float CalulatedHealth = m_currentHealth / m_maxHealth; //Gives me the percentage of health left.
            UpdatedHealth(CalulatedHealth);
        }

        m_deCounter += Time.deltaTime;
    }

    IEnumerator CalculateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_repeatRate);

            if (!m_connected)
            {
                m_currentHealth -= m_decreaseRate;
                m_repeatRate = m_repeatVar;
                m_decreaseRate = m_decreaseVar;
            }
            else if (m_connected)
            {
                m_currentHealth += m_decreaseRate * 3;
                m_repeatRate = 0.5f;
                m_decreaseRate = 2.5f;
            }


            //Error Checking
            if (m_currentHealth > m_maxHealth) m_currentHealth = m_maxHealth;
            else if (m_currentHealth < m_minHealth) m_currentHealth = m_minHealth;

            float CalulatedHealth = m_currentHealth / m_maxHealth; //Gives me the percentage of health left.
            UpdatedHealth(CalulatedHealth);
        }
    }

    public void Interaction(bool _change)
    {
        m_connected = _change;
    }

    void UpdatedHealth(float _health)
    {
        m_healthBar.transform.localScale = new Vector3(_health, m_healthBar.transform.localScale.y, m_healthBar.transform.localScale.z);
    }

	public float GetCurrentHealth()
	{
		return m_currentHealth;
	}

    public float GetCurrentCounter()
    {
        return m_deCounter;
    }
}
