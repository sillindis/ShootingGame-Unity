using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    GameManager gameManager;

    public GameObject homePopUp;
    public GameObject gameStartPopUp;
    bool IsPause;

    // Start is called before the first frame update
    void Start()
    {
        IsPause = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    public void GameStartPopUpCancle()
    {
        if (IsPause != true)
            return;

        Time.timeScale = 1;
        IsPause = false;
        gameStartPopUp.SetActive(false);
    }

    public void GoHomeScenes()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void GoGameScenes()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
