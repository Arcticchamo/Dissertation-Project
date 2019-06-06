using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AITESTTEXTSCRIPT : MonoBehaviour {

    public Text m_aitext;

    void Start()
    {
        m_aitext = GetComponent<Text>();
    }

    void Update()
    { 
    }

    public void SetDisplay(string _emotion)
    {
        Debug.Log(_emotion + "  " + Application.persistentDataPath);
        m_aitext.text = _emotion + "  " + Application.persistentDataPath;
    }
}
