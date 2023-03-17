using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public GameObject btnClose;
    public GameObject textLevel;
    public GameObject textDamage;
    public GameObject textUpgrade;
    public GameObject btnUpgrade;
    public GameObject textCoin;

    private void Start()
    {
        Text tl = textLevel.GetComponent<Text>();
        tl.text = (DataManager.Instance.data.level).ToString();

        Text td = textDamage.GetComponent<Text>();
        td.text = (DataManager.Instance.data.damage).ToString();

        //업그레이드 데이타 만들기

        Text tc = textCoin.GetComponent<Text>();
        tc.text = (DataManager.Instance.data.coin).ToString();


    }

    public void GoLobbyScence()
    {
        DataManager.Instance.SaveGameData();
        SceneManager.LoadScene("Lobby");
    }
    
}
