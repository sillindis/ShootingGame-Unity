using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    //static GameObject container;
    static MusicManager instance;

    public AudioClip background;
    public AudioClip battle;

    private AudioSource musicSource;
    private float musicVolume = 1f;

    public static MusicManager Instance
    {
        get
        {
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);

        musicSource = GetComponent<AudioSource>();

        musicSource.clip = background; //������� ����
        musicVolume = DataManager.Instance.data.musicVolume; //���� ���������� ���� ���� ����

        if (DataManager.Instance.data.musicOn == true) // //���� ���������� ���� on off ����
            musicSource.Play();
        else
            musicSource.Stop();
    }

    public void Update()
    {
        musicSource.volume = musicVolume;
    }
    public void MusicChangeBattle() //��Ʋ �������� ����
    {
        musicSource.clip = battle;
        musicSource.Play();
    }

    public void MusicChangeBackGround() //��� �������� ����
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void MusiceCheck() //���� ON/OFF
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
            DataManager.Instance.data.musicOn = false;
        }
        else
        {
            musicSource.Play();
            DataManager.Instance.data.musicOn = true;
        }
    }

    public void SetVolume(float vol) //���� ���� ����
    {
        musicVolume = vol;
        DataManager.Instance.data.musicVolume = musicVolume;
        
    }

    public void MusicRestore()
    {
        musicSource.Stop();
        musicSource.Play();
        musicVolume = DataManager.Instance.data.musicVolume;
    }
}
