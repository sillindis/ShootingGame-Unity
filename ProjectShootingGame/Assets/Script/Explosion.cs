using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void OnEnable()
    {
        Invoke("Disable", 2f);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

    public void StartExplosion(string target)
    {
        anim.SetTrigger("OnExplosion");

        switch (target)
        {
            case "P":
                transform.localScale = Vector3.one * 2f;
                break;
            case "S":
                transform.localScale = Vector3.one * 2f;
                break;
            case "M":
                transform.localScale = Vector3.one * 5f;
                break;
            case "L":
                transform.localScale = Vector3.one * 7f;
                break;
            case "B":
                transform.localScale = Vector3.one * 10f;
                break;
        }
    }
}
