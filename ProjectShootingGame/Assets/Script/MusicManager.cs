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

        musicSource.clip = background; //배경음악 설정
        musicVolume = DataManager.Instance.data.musicVolume; //이전 유저데이터 음악 볼륨 적용

        if (DataManager.Instance.data.musicOn == true) // //이전 유저데이터 음악 on off 적용
            musicSource.Play();
        else
            musicSource.Stop();
    }

    public void Update()
    {
        musicSource.volume = musicVolume;
    }
    public void MusicChangeBattle() //배틀 음악으로 변경
    {
        musicSource.clip = battle;
        musicSource.Play();
    }

    public void MusicChangeBackGround() //배경 음악으로 변경
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void MusiceCheck() //음악 ON/OFF
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

    public void SetVolume(float vol) //음악 볼륨 조절
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
