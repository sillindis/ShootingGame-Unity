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

        //stage 업데이트
        for (int i = 0; i < stages.Length; i++)
        {
            if (DataManager.Instance.data.stageUnlock[i] == true)
            {
                stages[i].SetActive(true);
            }
        }

        //coin 업데이트
        Text coinText = coin.GetComponent<Text>();
        coinText.text = (DataManager.Instance.data.coin).ToString();

        //music ui 업데이트
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
        if (Input.GetKeyDown(KeyCode.Escape)) //모바일에서 뒤로가기시
        {
            HomePopUp();
        }

    }

    public void HomePopUp() //팝업: 홈으로
    {
        if (IsPause != false)
            return;

        Time.timeScale = 0;
        IsPause = true;
        homePopUp.SetActive(true);
    }

    public void HomePopUpCancle() //팝업: 홈 닫기
    {
        if (IsPause != true)
            return;

        Time.timeScale = 1;
        IsPause = false;
        homePopUp.SetActive(false);
    }

    public void GameStartPopUp() //팝업: 게임 시작
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

    public void GameStartPopUpCancle() //팝업: 게임 시작 닫기
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

    public void MusicPopUp() //팝업: 뮤직ui
    {
        if (IsPause != false)
            return;

        Time.timeScale = 0;
        IsPause = true;

        settingPopUp.SetActive(false);
        musicPopUp.SetActive(true);
    }

    public void MusicUpCancle() //팝업: 뮤직ui 닫기
    {
        if (IsPause != true)
            return;

        Time.timeScale = 1;
        IsPause = false;
        musicPopUp.SetActive(false);
    }
    public void MusicCheck() //뮤직ui On off 체크
    {
        MusicManager.Instance.MusiceCheck();
        DataManager.Instance.SaveGameData();

        musicOnCheck.SetActive(!musicOnCheck.activeSelf);
    }

    public void MusicVolume(float vol) //뮤직 볼륨 조절
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
        PlayerPrefs.SetString("TextFile", stageName); //Stage 파일 이름 넘겨주기
        MusicManager.Instance.MusicChangeBattle();
        SceneManager.LoadScene("GamePlay");
    }
}
