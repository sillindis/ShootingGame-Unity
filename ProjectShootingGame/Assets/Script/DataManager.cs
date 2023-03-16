using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    static GameObject container;
    static DataManager instance;
    string dataFileName = "GameData.json"; //���� ������ �����̸� ����
    public Data data = new Data(); //����� ����

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                container = new GameObject();
                container.name = "DataManager";
                instance = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    public void LoadGameData()
    {
        string filePath = Application.streamingAssetsPath + "/" + dataFileName;

        if (File.Exists(filePath))
        {
            string fromJsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<Data>(fromJsonData);
            Debug.Log("������ �ҷ����� �Ϸ�");
        }
        else
        {
            Debug.LogError("������ �ҷ����� ����");
        }
    }

    public void SaveGameData()
    {
        string toJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.streamingAssetsPath + "/" + dataFileName;

        File.WriteAllText(filePath, toJsonData);

        Debug.Log("������ ����Ϸ�");
    }
}
