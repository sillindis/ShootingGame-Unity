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

        //stage ������Ʈ
        for(int i = 0; i < stages.Length; i++)
        {
            if (DataManager.Instance.data.stageUnlock[i] == true)
            {
                stages[i].SetActive(true);
            }
        }

        //coin ������Ʈ
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

        //Ŭ���� icon�� �̸��� ��������
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;
        stageName = clickObj.name;

        //gameStartPopUp�� UI text�� ����
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
        PlayerPrefs.SetString("TextFile", stageName); //Stage ���� �̸� �Ѱ��ֱ�
        SceneManager.LoadScene("GamePlay");
    }
}
