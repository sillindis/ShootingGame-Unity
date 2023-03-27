using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public GameObject player;
    public Animator startTextAnim;
    public GameObject ExitButtonSet;
    public GameObject resetButton;
    public GameObject resetPopUp;
    public GameObject textPopUp;

    public bool IsPause = false;

    Rigidbody2D rigid;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1;
        rigid = player.GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(0, 2);
        Invoke("PlayerStop", 2);
        Invoke("StartAnim", 3);
        Invoke("ExitSet", 3);
        Invoke("ResetIcon", 3);
        DataManager.Instance.LoadGameData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void PlayerStop() //플레이어 연출
    {
        rigid.velocity = Vector2.zero;
    }

    public void ExitSet()
    {
        ExitButtonSet.SetActive(true);
    }

    public void ResetIcon()
    {
        resetButton.SetActive(true);
    }

    public void ResetPopUp() // 팝업: 리셋
    {
        if (IsPause != false)
            return;

        Time.timeScale = 0;
        IsPause = true;
        resetPopUp.SetActive(true);
    }

    public void ResetPopUpCancle() //팝업: 리셋 닫기
    {
        if (IsPause != true)
            return;

        Time.timeScale = 1;
        IsPause = false;
        resetPopUp.SetActive(false);
    }
    public void ResetButton() //리셋 버튼 눌렀을때
    {
        //reset data
        DataManager.Instance.data.coin = 200.0;
        DataManager.Instance.data.level = 1;
        DataManager.Instance.data.damage = 0;
        DataManager.Instance.data.stageUnlock[0] = true;
        for (int i =1; i< DataManager.Instance.data.stageUnlock.Length; i++)
        {
            DataManager.Instance.data.stageUnlock[i] = false;
        }
        for (int i = 0; i < DataManager.Instance.data.stageScore.Length; i++)
        {
            DataManager.Instance.data.stageScore[i] = 0.0;
        }
        DataManager.Instance.data.musicVolume = 0.7f;
        DataManager.Instance.data.musicOn = true;
        MusicManager.Instance.MusicRestore(); //음악 설정 복구

        DataManager.Instance.storeData.level = 1;
        DataManager.Instance.storeData.damage = 0;
        DataManager.Instance.storeData.price = 30;

        //save data
        DataManager.Instance.SaveGameData();
        DataManager.Instance.SaveStoreData();

        ResetPopUpCancle();

        //print text
        textPopUp.SetActive(true);
        Invoke("TextPopUp", 1);
    }

    public void TextPopUp() //리셋 완료후 메시지 띄우기
    {
        textPopUp.SetActive(false);
    }

    public void Exit() //exit 버튼 눌렀을때
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //eitor exit
#else
        Application.Quit(); //App exit
#endif
    }

    public void StartAnim() //스타트 버튼 띄우는 애니메이션
    {
        startTextAnim.SetTrigger("OnStart");
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Lobby");
    }


}
