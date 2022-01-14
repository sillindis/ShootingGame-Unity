using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed;
    public Transform sprites;

    float viewHeight;

    private void Awake()
    {
        viewHeight = Camera.main.orthographicSize * 2;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Scrolling();
    }

    private void Move()
    {
        //Lower the background.
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
        transform.position = curPos + nextPos;
    }

    private void Scrolling()
    {
        //When the background goes out of the camera, I raise it up again.
        if (sprites.position.y < viewHeight * (-1))
        {
            //sprites.transform.localPosition = Vector3.up * viewHeight * 3;
            sprites.position = new Vector3(0, 1, 0) * viewHeight * 1;
        }
    }
}
