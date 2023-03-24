using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public GameObject[] Score;

    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI score1 = Score[0].GetComponent<TextMeshProUGUI>();
        score1.text = (DataManager.Instance.data.stageScore[0]).ToString();

        TextMeshProUGUI score2 = Score[1].GetComponent<TextMeshProUGUI>();
        score2.text = (DataManager.Instance.data.stageScore[1]).ToString();

        TextMeshProUGUI score3 = Score[2].GetComponent<TextMeshProUGUI>();
        score3.text = (DataManager.Instance.data.stageScore[2]).ToString();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoLobbyScence();
        }
    }

    public void GoLobbyScence()
    {
        SceneManager.LoadScene("Lobby");
    }
}
