using UnityEngine;
using System.Collections;

public class QuitButton : MonoBehaviour {

    public PAD m_pad;

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
