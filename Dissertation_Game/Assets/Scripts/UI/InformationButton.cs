using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InformationButton : MonoBehaviour {

    public Image m_informationRight;
    public Image m_foodBack, m_foodRed, m_foodGreen;
    public Image m_cleanBack, m_cleanRed, m_cleanGreen;
    public Image m_waterBack, m_waterRed, m_waterGreen;

    public Text m_foodText, m_waterText, m_cleanText;

    public Sprite m_rightArrow, m_upArrow;

    private bool m_informationTrigger;

    void Start()
    {
        m_informationTrigger = false;

        m_foodBack.enabled = m_foodRed.enabled = m_foodGreen.enabled = false;
        m_cleanBack.enabled = m_cleanRed.enabled = m_cleanGreen.enabled = false;
        m_waterBack.enabled = m_waterRed.enabled = m_waterGreen.enabled = false;
        m_foodText.enabled = m_waterText.enabled = m_cleanText.enabled = false;
    }

    public void Trigger()
    {
        if (m_informationTrigger)
        {
            Debug.Log("Off");
            m_informationTrigger = false;

            m_informationRight.sprite = m_rightArrow;

            m_foodBack.enabled = m_foodRed.enabled = m_foodGreen.enabled = false;
            m_cleanBack.enabled = m_cleanRed.enabled = m_cleanGreen.enabled = false;
            m_waterBack.enabled = m_waterRed.enabled = m_waterGreen.enabled = false;
            m_foodText.enabled = m_waterText.enabled = m_cleanText.enabled = false;
        }
        else if (!m_informationTrigger)
        {
            Debug.Log("On");
            m_informationTrigger = true;

            m_informationRight.sprite = m_upArrow;

            m_foodBack.enabled = m_foodRed.enabled = m_foodGreen.enabled = true;
            m_cleanBack.enabled = m_cleanRed.enabled = m_cleanGreen.enabled = true;
            m_waterBack.enabled = m_waterRed.enabled = m_waterGreen.enabled = true;
            m_foodText.enabled = m_waterText.enabled = m_cleanText.enabled = true;
            StartCoroutine(Timer());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(7.0f);
        Trigger();
    }
}
