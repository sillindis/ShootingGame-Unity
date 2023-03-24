using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StoreManager : MonoBehaviour
{
    public GameObject textLevel;
    public GameObject textDamage;
    public GameObject textPrice;
    public GameObject btnUpgrade;
    public GameObject textCoin;
    public GameObject textNotUpgrade;

    private void Start()
    {
        UpdateText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//모바일에서 뒤로가기시
        {
            GoLobbyScence();
        }
    }

    public void Upgrade()
    {
        //## MAX 레벨 예외처리 해줘야함
        if(DataManager.Instance.data.coin >= DataManager.Instance.storeData.price)
        {
            //플레이어 업그레이드 데이터 갱신
            DataManager.Instance.data.coin -= DataManager.Instance.storeData.price; //보유한 코인 - 업그레이드 가격
            DataManager.Instance.data.level++;
            DataManager.Instance.data.damage += 2;


            //상점 다음 업그레이드 단계로 데이터 갱신
            DataManager.Instance.storeData.level++;
            DataManager.Instance.storeData.damage += 2;
            DataManager.Instance.storeData.price += 20;

            Debug.Log("업그레이드 완료");
            //상점 텍스트 업데이트
            UpdateText();
        }
        else
        {
            Debug.Log("업그레이드 가격 부족");
            textNotUpgrade.SetActive(true);
            Invoke("DiasspearText", 1);
        }
    }

    public void UpdateText() //상점에 있는 text 업데이트
    {
        TextMeshProUGUI tl = textLevel.GetComponent<TextMeshProUGUI>();
        tl.text = (DataManager.Instance.storeData.level).ToString();

        TextMeshProUGUI td = textDamage.GetComponent<TextMeshProUGUI>();
        td.text = (DataManager.Instance.storeData.damage).ToString();

        TextMeshProUGUI tp = textPrice.GetComponent<TextMeshProUGUI>();
        tp.text = (DataManager.Instance.storeData.price).ToString();
        if(DataManager.Instance.data.coin < DataManager.Instance.storeData.price)
        {
            tp.color = Color.red; // 업그레이드에 필요한 가격에 미치지 못할 경우 빨간색으로 바꾸기
        }

        // 업그레이드시 코인 부족할 경우 경고메시지 띄우기
        TextMeshProUGUI tc = textCoin.GetComponent<TextMeshProUGUI>();
        tc.text = (DataManager.Instance.data.coin).ToString();
    }

    public void DiasspearText() // 업그레이드시 코인 부족할 경우 경고메시지 띄운거 닫기
    {
        textNotUpgrade.SetActive(false);
    }

    public void GoLobbyScence() //로비로 갈때 데이터 저장후 이동
    {
        DataManager.Instance.SaveGameData();
        DataManager.Instance.SaveStoreData();
        SceneManager.LoadScene("Lobby");
    }
    
}
