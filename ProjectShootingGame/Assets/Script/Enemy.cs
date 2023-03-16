using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int enemyScore;
    public float speed;
    public int health;

    public float maxShotDelay;
    public float curShotDelay;

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject itemCoin;
    public GameObject itemPower;
    public GameObject itemBoom;
    public GameObject itemHealPack;
    public GameObject player;
    public ObjectManager objectManager;
    public GameManager gameManager;

    SpriteRenderer spriteRenderer;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        switch (enemyName)
        {
            case "B":
                health = 1000;
                Invoke("Stop", 2);
                break;
            case "L":
                health = 40;
                break;
            case "M":
                health = 25;
                break;
            case "S":
                health = 10;
                break;
        }
        ReturnSprite();
    }
    void Stop()
    {
        if (!gameObject.activeSelf)
            return;

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        Invoke("Think", 2);
    }

    //Boss Attact Pattern
    void Think()
    {
        if (!gameObject.activeSelf)
            return;

        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        switch (patternIndex)
        {
            case 0:
                FireFoward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }
    void FireFoward()
    {
        if (!gameObject.activeSelf)
            return;

        //#.Fire 4 Bullet Foward
        GameObject bulletR = objectManager.MakeObj("BulletBossA");
        GameObject bulletRR = objectManager.MakeObj("BulletBossA");
        GameObject bulletL = objectManager.MakeObj("BulletBossA");
        GameObject bulletLL = objectManager.MakeObj("BulletBossA");
        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
        bulletRR.transform.position = transform.position + Vector3.right * 0.6f;
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
        bulletLL.transform.position = transform.position + Vector3.left * 0.6f;
        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

        rigidR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);

        //#.Pattern Counting
        curPatternCount++;

        if(curPatternCount<maxPatternCount[patternIndex])
            Invoke("FireFoward", 2);
        else 
            Invoke("Think", 3);
    }
    void FireShot()
    {
        if (!gameObject.activeSelf)
            return;

        //#.Fire S Random Shotgun Bullet to Player
        for (int index=0;index<5;index++)
        {
            GameObject bullet = objectManager.MakeObj("BulletBossB");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = player.transform.position - transform.position; //calculate the distance from the player.
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
        }

        //#.Pattern Counting
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireShot", 2f);
        else
            Invoke("Think", 3);
    }
    void FireArc()
    {
        if (!gameObject.activeSelf)
            return;

        //#.Fire Arc Continue Fire
        GameObject bullet = objectManager.MakeObj("BulletBossB");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 17 * curPatternCount / maxPatternCount[patternIndex]), -1);
        rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);

        //#.Pattern Counting
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireArc", 0.15f);
        else
            Invoke("Think", 3);
    }
    void FireAround()
    {
        if (!gameObject.activeSelf)
            return;

        //#.Fire around
        int roundNumA = 45;
        int roundNumB = 35;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;

        for (int index=0 ; index< roundNum; index++)
        {
            GameObject bullet = objectManager.MakeObj("BulletBossB");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roundNum)
                                        ,Mathf.Sin(Mathf.PI * 2 * index / roundNum));
            rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 360 * index / roundNum + Vector3.forward*90;
            bullet.transform.Rotate(rotVec);
        }

        //#.Pattern Counting
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireAround", 1f);
        else
            Invoke("Think", 5);
    }
    //

    void Update()
    {
        if (enemyName == "B")
            return;

        Fire();
        Reload();
    }
    void Fire()
    {
        if (curShotDelay < maxShotDelay) //It needs to be loaded by the set time(maxShotDelay)
            return;

        if(enemyName == "S")
        {
            GameObject bullet = objectManager.MakeObj("BulletEnemyA");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = player.transform.position - transform.position; //calculate the distance from the player.
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
        else if(enemyName == "L")
        {
            GameObject bulletR = objectManager.MakeObj("BulletEnemyB");
            GameObject bulletL = objectManager.MakeObj("BulletEnemyB");
            bulletR.transform.position = transform.position + Vector3.right * 0.3f;
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;
            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f); //calculate the distance from the player.
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);
            rigidR.AddForce(dirVecR.normalized * 3, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 3, ForceMode2D.Impulse);
        }

        curShotDelay = 0; //Initialization
    }

    void Reload() //Reload Bullet 
    {
        curShotDelay += Time.deltaTime;
    }

    private void OnHit(int dmg)
    {
        //It prevents more than two items from coming out.
        if (health <= 0)
            return;

        health -= dmg;

        Color color = spriteRenderer.material.color; //Translucent when attacked
        color.a = 0.5f;
        spriteRenderer.material.color = color;
        Invoke("ReturnSprite", 0.05f);

        if (health <= 0)    // Enemy death
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            //#.Random Ratio Item Drop
            int ran = enemyName == "B" ? 0 : Random.Range(0, 20); //boss not drop

            if (ran < 9)
            {
                //None
            }
            else if(ran < 14) //Coin 25%
            {
                GameObject itemCoin = objectManager.MakeObj("ItemCoin");
                itemCoin.transform.position = transform.position;
            }
            else if (ran < 16) //Power 10%
            {
                GameObject itemPower = objectManager.MakeObj("ItemPower");
                itemPower.transform.position = transform.position;
            }
            else if (ran < 18) //Boom 10%
            {
                GameObject itemBoom = objectManager.MakeObj("ItemBoom");
                itemBoom.transform.position = transform.position;
            }
            else if (ran < 20) //HealPack 10%
            {
                GameObject itemHealPack = objectManager.MakeObj("ItemHealPack");
                itemHealPack.transform.position = transform.position;
            }

            gameObject.SetActive(false);
            CancelInvoke(); //Boss Error Solusion : Always Attack after Boss dead

            gameManager.CallExplosion(transform.position, enemyName);

            //#.Boss Kill
            if(enemyName=="B")
            {
                //Boss Error Solusion : Boss Attack Pattern
                patternIndex = -1;
                curPatternCount = 0;

                gameManager.StageEnd();
            }
        }
    }

    void ReturnSprite()
    {
        Color color = spriteRenderer.material.color; //After the attack, Turn it back to normal
        color.a = 1f;
        spriteRenderer.material.color = color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet" && enemyName != "B")
            gameObject.SetActive(false);
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "PlayerBoom")
        {
            Boom boom = collision.gameObject.GetComponent<Boom>();
            OnHit(boom.dmg);
        }

    }
}
