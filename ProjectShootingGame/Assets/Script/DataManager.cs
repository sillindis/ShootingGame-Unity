using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataManager : MonoBehaviour
{
    static GameObject _container;
    static DataManager instance = null;
    public string gameDataFileName = ".json";

    static GameObject Container
    {
        get
        {
            return _container;
        }
    }
    public static DataManager Instance
    {
        get
        {
            if(instance == null)
            {
                _container = new GameObject();
                _container.name = "DataManager";
                instance = _container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(_container);
            }
            return instance;
        }
    }

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + gameDataFileName;

        if (File.Exists(filePath))
        {
            Debug.Log("불러오기 성공!");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
