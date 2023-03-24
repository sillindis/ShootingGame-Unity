using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    static GameObject container;
    static DataManager instance;
    string dataFileName = "GameData.json"; //게임 데이터 파일이름 설정
    string storeDataFileName = "StoreData.json"; //상점 데이터 파일이름 설정
    public Data data = new Data(); //저장용 변수
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
            Debug.Log("데이터 불러오기 완료");
        }
        else
        {
            Debug.LogError("데이터 불러오기 실패");
        }
    }
    public void LoadStoreData()
    {
        string filePath = Application.persistentDataPath + "/" + storeDataFileName;

        if (File.Exists(filePath))
        {
            string fromJsonData = File.ReadAllText(filePath);
            storeData = JsonUtility.FromJson<StoreData>(fromJsonData);
            Debug.Log("상점 데이터 불러오기 완료");
        }
        else
        {
            Debug.LogError("상점 데이터 불러오기 실패");
        }
    }

    public void SaveGameData()
    {
        string toJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.persistentDataPath + "/" + dataFileName;

        //이미 저장된 파일이 있다면 덮어쓰기
        File.WriteAllText(filePath, toJsonData);

        Debug.Log("데이터 저장완료");
    }

    public void SaveStoreData()
    {
        string toJsonData = JsonUtility.ToJson(storeData, true);
        string filePath = Application.persistentDataPath + "/" + storeDataFileName;

        //이미 저장된 파일이 있다면 덮어쓰기
        File.WriteAllText(filePath, toJsonData);

        Debug.Log("상점데이터 저장완료");
    }

    private void OnApplicationQuit()
    {
        DataManager.Instance.SaveGameData();
        //DataManager.Instance.SaveStoreData();

        Debug.Log("게임종료");
    }
}
