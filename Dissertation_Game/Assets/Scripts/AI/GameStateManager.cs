using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameStateManager : MonoBehaviour {

    [Serializable]
    public class HamsterData
    {

    }

    HamsterData m_hamsterData;

    public static GameStateManager Instance = null;

    void Awake()
    {
        if (Instance != null) Debug.LogError("Gamestate Instance Already Exists");

        Instance = this;
    }

    void Start()
    {
    
    }

    void Update()
    { 
    
    }

    public void LoadSystemData()
    { 
        //If the file exists
        if (File.Exists(Application.persistentDataPath + "/HamsterData.dat"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/HamsterData.dat", FileMode.Open);
            m_hamsterData = (HamsterData)binaryFormatter.Deserialize(file);
            file.Close();
        }
        else
        {
            Debug.LogError("Save file does not exist");
        }
    }

    public void SaveSystemData()
    {
        BinaryFormatter binaryformatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/HamsterData.dat");
        binaryformatter.Serialize(file, m_hamsterData);
        file.Close();
    }

    public void SaveHamsterData(HamsterData _hamster)
    {
        m_hamsterData = _hamster;
    }
}