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
        if (Input.GetKeyDown(KeyCode.Escape))//����Ͽ��� �ڷΰ����
        {
            GoLobbyScence();
        }
    }

    public void Upgrade()
    {
        //## MAX ���� ����ó�� �������
        if(DataManager.Instance.data.coin >= DataManager.Instance.storeData.price)
        {
            //�÷��̾� ���׷��̵� ������ ����
            DataManager.Instance.data.coin -= DataManager.Instance.storeData.price; //������ ���� - ���׷��̵� ����
            DataManager.Instance.data.level++;
            DataManager.Instance.data.damage += 2;


            //���� ���� ���׷��̵� �ܰ�� ������ ����
            DataManager.Instance.storeData.level++;
            DataManager.Instance.storeData.damage += 2;
            DataManager.Instance.storeData.price += 20;

            Debug.Log("���׷��̵� �Ϸ�");
            //���� �ؽ�Ʈ ������Ʈ
            UpdateText();
        }
        else
        {
            Debug.Log("���׷��̵� ���� ����");
            textNotUpgrade.SetActive(true);
            Invoke("DiasspearText", 1);
        }
    }

    public void UpdateText() //������ �ִ� text ������Ʈ
    {
        TextMeshProUGUI tl = textLevel.GetComponent<TextMeshProUGUI>();
        tl.text = (DataManager.Instance.storeData.level).ToString();

        TextMeshProUGUI td = textDamage.GetComponent<TextMeshProUGUI>();
        td.text = (DataManager.Instance.storeData.damage).ToString();

        TextMeshProUGUI tp = textPrice.GetComponent<TextMeshProUGUI>();
        tp.text = (DataManager.Instance.storeData.price).ToString();
        if(DataManager.Instance.data.coin < DataManager.Instance.storeData.price)
        {
            tp.color = Color.red; // ���׷��̵忡 �ʿ��� ���ݿ� ��ġ�� ���� ��� ���������� �ٲٱ�
        }

        // ���׷��̵�� ���� ������ ��� ���޽��� ����
        TextMeshProUGUI tc = textCoin.GetComponent<TextMeshProUGUI>();
        tc.text = (DataManager.Instance.data.coin).ToString();
    }

    public void DiasspearText() // ���׷��̵�� ���� ������ ��� ���޽��� ���� �ݱ�
    {
        textNotUpgrade.SetActive(false);
    }

    public void GoLobbyScence() //�κ�� ���� ������ ������ �̵�
    {
        DataManager.Instance.SaveGameData();
        DataManager.Instance.SaveStoreData();
        SceneManager.LoadScene("Lobby");
    }
    
}
