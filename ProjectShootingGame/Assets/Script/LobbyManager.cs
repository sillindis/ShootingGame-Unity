using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    static LobbyManager instance;

    static public LobbyManager Instance
    {
        get
        {
            return instance;
        }
    }

    public GameObject[] stages;
    public GameObject coin;
    public GameObject homePopUp;
    public GameObject gameStartPopUp;

    public GameObject settingPopUp;
    public GameObject musicPopUp;
    public GameObject musicVolume;
    public GameObject musicOnCheck;
    bool IsPause;
    bool isSetting;

    string stageName;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        IsPause = false;
        isSetting = false;

        //stage ������Ʈ
        for (int i = 0; i < stages.Length; i++)
        {
            if (DataManager.Instance.data.stageUnlock[i] == true)
            {
                stages[i].SetActive(true);
            }
        }

        //coin ������Ʈ
        Text coinText = coin.GetComponent<Text>();
        coinText.text = (DataManager.Instance.data.coin).ToString();

        //music ui ������Ʈ
        Image image = musicPopUp.GetComponent<Image>();
        image.transform.SetAsLastSibling();

        Slider slider = musicVolume.GetComponent<Slider>();
        slider.value = DataManager.Instance.data.musicVolume;
        Debug.Log("DataManager.Instance.data.musicOn: " + DataManager.Instance.data.musicOn);
        if (DataManager.Instance.data.musicOn == true)
            musicOnCheck.SetActive(true);
        else
            musicOnCheck.SetActive(false);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //����Ͽ��� �ڷΰ����
        {
            HomePopUp();
        }

    }

    public void HomePopUp() //�˾�: Ȩ����
    {
        if (IsPause != false)
            return;

        Time.timeScale = 0;
        IsPause = true;
        homePopUp.SetActive(true);
    }

    public void HomePopUpCancle() //�˾�: Ȩ �ݱ�
    {
        if (IsPause != true)
            return;

        Time.timeScale = 1;
        IsPause = false;
        homePopUp.SetActive(false);
    }

    public void GameStartPopUp() //�˾�: ���� ����
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

    public void GameStartPopUpCancle() //�˾�: ���� ���� �ݱ�
    {
        if (IsPause != true)
            return;

        Time.timeScale = 1;
        IsPause = false;
        gameStartPopUp.SetActive(false);
    }

    public void SettingPopUp()
    {
        if(isSetting == false)
        {
            Time.timeScale = 0;
            settingPopUp.SetActive(true);
            isSetting = true;
        }
        else
        {
            Time.timeScale = 1;
            settingPopUp.SetActive(false);
            isSetting = false;
        }
    }

    public void MusicPopUp() //�˾�: ����ui
    {
        if (IsPause != false)
            return;

        Time.timeScale = 0;
        IsPause = true;

        settingPopUp.SetActive(false);
        musicPopUp.SetActive(true);
    }

    public void MusicUpCancle() //�˾�: ����ui �ݱ�
    {
        if (IsPause != true)
            return;

        Time.timeScale = 1;
        IsPause = false;
        musicPopUp.SetActive(false);
    }
    public void MusicCheck() //����ui On off üũ
    {
        MusicManager.Instance.MusiceCheck();
        DataManager.Instance.SaveGameData();

        musicOnCheck.SetActive(!musicOnCheck.activeSelf);
    }

    public void MusicVolume(float vol) //���� ���� ����
    {
        MusicManager.Instance.SetVolume(vol);
        DataManager.Instance.SaveGameData();
    }

    public void GoScoreScenes()
    {
        DataManager.Instance.LoadGameData();
        DataManager.Instance.SaveGameData();
        SceneManager.LoadScene("Score");
    }

    public void GoStoreScenes()
    {
        DataManager.Instance.LoadStoreData();
        DataManager.Instance.SaveGameData();
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
        MusicManager.Instance.MusicChangeBattle();
        SceneManager.LoadScene("GamePlay");
    }
}
