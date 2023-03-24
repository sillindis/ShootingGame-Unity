using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    static GameObject container;
    static DataManager instance;
    string dataFileName = "GameData.json"; //���� ������ �����̸� ����
    string storeDataFileName = "StoreData.json"; //���� ������ �����̸� ����
    public Data data = new Data(); //����� ����
    public StoreData storeData = new StoreData();

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
        string filePath = Application.persistentDataPath + "/" + dataFileName; //streamingAssetsPath

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
    public void LoadStoreData()
    {
        string filePath = Application.persistentDataPath + "/" + storeDataFileName;

        if (File.Exists(filePath))
        {
            string fromJsonData = File.ReadAllText(filePath);
            storeData = JsonUtility.FromJson<StoreData>(fromJsonData);
            Debug.Log("���� ������ �ҷ����� �Ϸ�");
        }
        else
        {
            Debug.LogError("���� ������ �ҷ����� ����");
        }
    }

    public void SaveGameData()
    {
        string toJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.persistentDataPath + "/" + dataFileName;

        //�̹� ����� ������ �ִٸ� �����
        File.WriteAllText(filePath, toJsonData);

        Debug.Log("������ ����Ϸ�");
    }

    public void SaveStoreData()
    {
        string toJsonData = JsonUtility.ToJson(storeData, true);
        string filePath = Application.persistentDataPath + "/" + storeDataFileName;

        //�̹� ����� ������ �ִٸ� �����
        File.WriteAllText(filePath, toJsonData);

        Debug.Log("���������� ����Ϸ�");
    }

    private void OnApplicationQuit()
    {
        DataManager.Instance.SaveGameData();
        //DataManager.Instance.SaveStoreData();

        Debug.Log("��������");
    }
}
