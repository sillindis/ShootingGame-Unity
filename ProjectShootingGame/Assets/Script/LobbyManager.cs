using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public GameObject[] stages;
    public GameObject coin;
    public GameObject homePopUp;
    public GameObject gameStartPopUp;
    bool IsPause;

    string stageName;

    // Start is called before the first frame update
    void Start()
    {
        IsPause = false;

        //stage 업데이트
        for(int i = 0; i < stages.Length; i++)
        {
            if (DataManager.Instance.data.stageUnlock[i] == true)
            {
                stages[i].SetActive(true);
            }
        }

        //coin 업데이트
        Text coinText = coin.GetComponent<Text>();
        coinText.text = (DataManager.Instance.data.coin).ToString();
    }

    public void HomePopUp()
    {
        if (IsPause != false)
            return;

        Time.timeScale = 0;
        IsPause = true;
        homePopUp.SetActive(true);
    }

    public void HomePopUpCancle()
    {
        if (IsPause != true)
            return;

        Time.timeScale = 1;
        IsPause = false;
        homePopUp.SetActive(false);
    }

    public void GameStartPopUp()
    {
        if (IsPause != false)
            return;

        Time.timeScale = 0;
        IsPause = true;
        gameStartPopUp.SetActive(true);

        //클릭한 icon의 이름을 가져오기
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;
        stageName = clickObj.name;

        //gameStartPopUp의 UI text를 변경
        GameObject child = gameStartPopUp.transform.GetChild(0).gameObject;
        Text childText = child.GetComponent<Text>();
        childText.text = "Fly " + stageName +"?";
    }

    public void GameStartPopUpCancle()
    {
        if (IsPause != true)
            return;

        Time.timeScale = 1;
        IsPause = false;
        gameStartPopUp.SetActive(false);
    }

    public void GoStoreScenes()
    {
        DataManager.Instance.LoadStoreData();
        SceneManager.LoadScene("Store");
    }
    public void GoHomeScenes()
    {
        DataManager.Instance.SaveGameData();
        SceneManager.LoadScene("StartScreen");
    }

    public void GoGameScenes()
    {
        PlayerPrefs.SetString("TextFile", stageName); //Stage 파일 이름 넘겨주기
        SceneManager.LoadScene("GamePlay");
    }
}
