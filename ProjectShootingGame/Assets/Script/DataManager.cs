using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    static GameObject container;
    static DataManager instance;
    string dataFileName = "GameData.json"; //게임 데이터 파일이름 설정
    public Data data = new Data(); //저장용 변수

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
            Debug.Log("데이터 불러오기 완료");
        }
        else
        {
            Debug.LogError("데이터 불러오기 실패");
        }
    }

    public void SaveGameData()
    {
        string toJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.streamingAssetsPath + "/" + dataFileName;

        File.WriteAllText(filePath, toJsonData);

        Debug.Log("데이터 저장완료");
    }
}
