using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButton : MonoBehaviour {

    public Image m_menuLeft;

    public Sprite m_leftArrow, m_upArrow;
    public Button m_quit;
    public Text m_quitText;
    private bool m_menuTrigger;

    void Start()
    {
        m_menuTrigger = false;
        m_quit.image.enabled = false;
        m_quitText.enabled = false;
        m_quit.interactable = false;
    }

    public void Trigger()
    {
        if (m_menuTrigger)
        {
            m_menuTrigger = false;
            m_menuLeft.sprite = m_leftArrow;
            m_quit.image.enabled = false;
            m_quitText.enabled = false;
            m_quit.interactable = false;
            Time.timeScale = 1.0f;
            Debug.Log("Menu Off");
        }
        else if (!m_menuTrigger)
        {
            m_menuTrigger = true;
            m_menuLeft.sprite = m_upArrow;
            m_quit.image.enabled = true;
            m_quitText.enabled = true;
            m_quit.interactable = true;
            Time.timeScale = 0.0f;
            Debug.Log("Menu On");
        }
    }
}
