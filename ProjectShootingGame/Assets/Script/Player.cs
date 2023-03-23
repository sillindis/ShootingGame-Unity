using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isTouchTop;
    public bool isTouchButtom;
    public bool isTouchRight;
    public bool isTouchLeft;
    public bool isBoomTime;
    public bool isRespawnTime;
    public bool isHit;

    public int life;
    public double score;
    public double coin;
    public float speed;
    public int power;
    public int maxPower;
    public float maxShotDelay;
    public float curShotDelay;
    public int boom;
    public int maxBoom;
    public bool isBoomButton;


    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject boomObj;

    public GameManager gameManager;
    public ObjectManager objectManager;

    SpriteRenderer spriteRenderer;


    public float moveSpeed = 2f; // 비행기의 이동 속도
    private Rigidbody2D rigid2D; // 비행기의 강체(Rigidbody)
    private bool isFar = false; // 터치와 비행기의 거리 체크
    private Vector3 inputPosition; // 터치의 월드 포지션
    private Vector2 moveDir; // 화면 터치 시 비행기 이동 방향

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid2D = GetComponent<Rigidbody2D>();

        score = 0;
        coin = 0;
    }

    private void OnEnable() //function that works when a player is activated
    {
        Unbeatable();

        Invoke("Unbeatable", 3);
    }

    void Update()
    {
        Debug.Log("coin: " + coin);
        Move();
        Move1();
        Fire(); //Shot Bullet
        FireReload();
        Boom(); //Shot Boom

        if (Input.GetKeyDown(KeyCode.Space))
            Boom();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if ((h == 1 && isTouchRight == true) || (h == -1 && isTouchLeft == true)) //if the player touches the border, player don't move.
            h = 0;
        float v = Input.GetAxisRaw("Vertical");
        if ((v == 1 && isTouchTop == true) || (v == -1 && isTouchButtom == true))
            v = 0;
        Vector2 curpos = transform.position;
        Vector2 nextpos = new Vector2(h, v) * speed * Time.deltaTime;

        transform.position = curpos + nextpos;
    }

    //
    private void Move1()
    {
        if (rigid2D == null || Input.touchCount < 1) // 강체가 없거나 터치가 없으면 리턴
            return;
        moveDir = Vector2.zero;  // 방향 초기화

        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            inputPosition = GetInputPosition(Input.GetTouch(0).position);
            if (Vector3.Distance(transform.position, inputPosition) > .2f)
            {
                isFar = true; // 비행기와 터치 사이의 거리가 먼 상태
            }
        }
        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            isFar = false;
        }
        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            inputPosition = GetInputPosition(Input.GetTouch(0).position);
            if (isFar) // 거리가 먼 상태
            {
                moveDir = GetDirection(transform.position, inputPosition); // 터치와 비행기 거리를 체크
                isFar = (Vector3.Distance(transform.position, inputPosition) > .2f);
            }
            else // 터치에 가까운 상태
            {
                transform.position = inputPosition;
            }
        }
        rigid2D.velocity = moveDir * moveSpeed; // 방향에 속도를 곱해서 강체에 적용
    }

    // 터치의 스크린 포지션을 월드 포지션으로 변경
    public Vector3 GetInputPosition(Vector3 position)
    {
        Vector3 screenPosition = position + (Vector3.back * Camera.main.transform.position.z);
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }

    // 두 포지션 사이의 방향
    public Vector2 GetDirection(Vector2 from, Vector2 to)
    {
        Vector2 delta = to - from;
        float radian = Mathf.Atan2(delta.y, delta.x);
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
    //



    public void BoomButton()
    {
        isBoomButton = true;
    }

    void Fire()
    {

        //if (!Input.GetButton("Fire1"))
        //    return;

        if (curShotDelay < maxShotDelay) //It needs to be loaded by the set time(maxShotDelay)
            return;

        switch (power)
        {
            case 1:
                GameObject bullet = objectManager.MakeObj("BulletPlayerA");
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.Euler(0, 0, 90);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bulletR = objectManager.MakeObj("BulletPlayerA");
                bulletR.transform.position = transform.position + Vector3.right * 0.1f;
                GameObject bulletL = objectManager.MakeObj("BulletPlayerA");
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;
                bulletR.transform.rotation = Quaternion.Euler(0, 0, 90);
                bulletL.transform.rotation = Quaternion.Euler(0, 0, 90);
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 3:
                GameObject bulletRR = objectManager.MakeObj("BulletPlayerA");
                bulletRR.transform.position = transform.position + Vector3.right * 0.25f;
                GameObject bulletCC = objectManager.MakeObj("BulletPlayerA");
                bulletCC.transform.position = transform.position;
                GameObject bulletLL = objectManager.MakeObj("BulletPlayerA");
                bulletLL.transform.position = transform.position + Vector3.left * 0.25f;
                bulletRR.transform.rotation = Quaternion.Euler(0, 0, 90);
                bulletCC.transform.rotation = Quaternion.Euler(0, 0, 90);
                bulletLL.transform.rotation = Quaternion.Euler(0, 0, 90);
                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 4:
                GameObject bulletUp = objectManager.MakeObj("BulletPlayerB");
                bulletUp.transform.position = transform.position;
                bulletUp.transform.rotation = Quaternion.Euler(0, 0, 90);
                Rigidbody2D rigidUp = bulletUp.GetComponent<Rigidbody2D>();
                rigidUp.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 5:
                GameObject bulletUpR = objectManager.MakeObj("BulletPlayerB");
                bulletUpR.transform.position = transform.position + Vector3.right * 0.1f;
                GameObject bulletUpL = objectManager.MakeObj("BulletPlayerB");
                bulletUpL.transform.position = transform.position + Vector3.left * 0.1f;
                bulletUpR.transform.rotation = Quaternion.Euler(0, 0, 90);
                bulletUpL.transform.rotation = Quaternion.Euler(0, 0, 90);
                Rigidbody2D rigidUpR = bulletUpR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidUpL = bulletUpL.GetComponent<Rigidbody2D>();
                rigidUpR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidUpL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 6:
            default:
                GameObject bulletUpRR = objectManager.MakeObj("BulletPlayerA");
                bulletUpRR.transform.position = transform.position + Vector3.right * 0.25f;
                GameObject bulletUpCC = objectManager.MakeObj("BulletPlayerB");
                bulletUpCC.transform.position = transform.position;
                GameObject bulletUpLL = objectManager.MakeObj("BulletPlayerA");
                bulletUpLL.transform.position = transform.position + Vector3.left * 0.25f;
                bulletUpRR.transform.rotation = Quaternion.Euler(0, 0, 90);
                bulletUpCC.transform.rotation = Quaternion.Euler(0, 0, 90);
                bulletUpLL.transform.rotation = Quaternion.Euler(0, 0, 90);
                Rigidbody2D rigidUpRR = bulletUpRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidUpCC = bulletUpCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidUpLL = bulletUpLL.GetComponent<Rigidbody2D>();
                rigidUpRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidUpCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidUpLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
        }


        curShotDelay = 0; //Initialization
    }

    void FireReload() //Reload Bullet 
    {
        curShotDelay += Time.deltaTime;
    }

    void Boom()
    {
        //if (!Input.GetButton("Fire2"))
        //    return;

        if (!isBoomButton)
            return;

        if (isBoomTime)
            return;

        if (boom == 0)
            return;

        boom--;
        isBoomTime = true;
        gameManager.UpdateBoomIcon(boom);

        //The bomb goes forward
        GameObject shotBoom = objectManager.MakeObj("BoomPlayer");
        shotBoom.transform.position = transform.position;
        shotBoom.transform.rotation = Quaternion.Euler(0, 0, 90);
        Rigidbody2D rigid = shotBoom.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 3, ForceMode2D.Impulse);

        isBoomButton = false;

        Invoke("BoomReload", 4f); //Reload Boom
    }

    void BoomReload() //CoolTime Boom
    {
        isBoomTime = false;
    }

    void Unbeatable()
    {
        isRespawnTime = !isRespawnTime;

        if (isRespawnTime) //Invincible time effect
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1); //Invincible time is over
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //If player touches the border
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Buttom":
                    isTouchButtom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
        else if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            if (isRespawnTime)
                return;

            if (isHit)
                return;

            isHit = true;

            life--;
            gameManager.UpdateLifeIcon(life);

            gameManager.CallExplosion(transform.position, "P");

            if (life == 0)
            {
                gameManager.GameOver();
            }
            else
            {
                gameManager.RespawnPlayer();
            }

            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);

        }
        else if (collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type)
            {
                case "Coin":
                    coin += 10;
                    break;
                case "Power":
                    if (power >= maxPower)
                        score += 200;
                    else
                        power++;
                    break;
                case "Boom":
                    if (boom >= maxBoom)
                        score += 200;
                    else
                    {
                        boom++;
                        gameManager.UpdateBoomIcon(boom);
                    }
                    break;
                case "HealPack":
                    if (life >= 3)
                        score += 200;
                    else
                    {
                        life++;
                        gameManager.UpdateLifeIcon(life);
                    }
                    break;
            }
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Buttom":
                    isTouchButtom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}


