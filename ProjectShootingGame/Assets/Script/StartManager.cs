using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public GameObject player;
    public Animator startTextAnim;
    public GameObject ExitButtonSet;
    
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
    }

    void PlayerStop()
    {
        rigid.velocity = Vector2.zero;
    }

    public void ExitSet()
    {
        ExitButtonSet.SetActive(true);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //eitor exit
#else
        Application.Quit(); //App exit
#endif
    }

    public void StartAnim()
    {
        startTextAnim.SetTrigger("OnStart");
    }

    public void StartButton()
    {
        SceneManager.LoadScene("GamePlay");
    }


}
