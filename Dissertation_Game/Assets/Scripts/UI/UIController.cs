using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {

    public Image m_informationRight, m_menuLeft;

    public Image m_foodBack, m_foodRed, m_foodGreen;
    public Image m_cleanBack, m_cleanRed, m_cleanGreen;
    public Image m_waterBack, m_waterRed, m_waterGreen;

    public Text m_foodText, m_waterText, m_cleanText;

    public Sprite m_leftArrow, m_rightArrow, m_upArrow;

    bool m_informationTrigger, m_menuTrigger = false;

    void Start()
    {
        m_informationTrigger = false;
        m_menuTrigger = false;

        m_foodBack.enabled = m_foodRed.enabled = m_foodGreen.enabled = false;
        m_cleanBack.enabled = m_cleanRed.enabled = m_cleanGreen.enabled = false;
        m_waterBack.enabled = m_waterRed.enabled = m_waterGreen.enabled = false;
        m_foodText.enabled = m_waterText.enabled = m_cleanText.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == "UIButton")
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
                    }
                }
                else if (hit.collider.name == "Main Menu")
                {
                    if (m_informationTrigger)
                    {
                        m_informationTrigger = false;

                    }
                    else if (!m_informationTrigger)
                    {
                        m_informationTrigger = true;

                    }
                }
            }
        }
    }
}
