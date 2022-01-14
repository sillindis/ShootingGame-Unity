using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public int dmg;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBoom")
        {
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "EnemyBullet")
        {
            collision.gameObject.SetActive(false);
        }
    }
}
