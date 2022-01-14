using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public int stage;
    public Animator stageAnim;
    public Animator clearAnim;
    public Animator fadeAnim;
    public Transform playerPos;

    public string[] enemyObjs;
    public Transform[] spawnPoints;

    public float nextSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;
    public GameObject gameOverSet;
    public GameObject gameClearSet;
    public ObjectManager objectManager;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    private void Awake()
    {
        spawnList = new List<Spawn>();
        enemyObjs = new string[] { "EnemyS", "EnemyM", "EnemyL", "EnemyB" };
        StageStart();
    }

    public void StageStart()
    {
        //#.Stage UI Load
        stageAnim.SetTrigger("OnText");
        stageAnim.GetComponent<Text>().text = "Stage " + stage;
        clearAnim.GetComponent<Text>().text = "Clear !";

        //#.Enemy Spawn File Read
        ReadSpawnFile();

        //#.Fade In
        fadeAnim.SetTrigger("In");
    }

    public void StageEnd()
    {
        //#.Clear UI Load
        clearAnim.SetTrigger("OnText");

        //#.Fade Out
        fadeAnim.SetTrigger("Out");

        //#.Player Repos
        playerPos.transform.position = playerPos.position;

        //#.Stage Increament
        stage++;

        if (stage > 2) //All Clear
            Invoke("AllClear", 4);
        else
            Invoke("StageStart", 5);
    }

    void ReadSpawnFile()
    {
        //#1. Initialize variables
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        //#2. Read respawn file
        TextAsset textFile = Resources.Load("STAGE "+ stage) as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while(stringReader != null)
        {
            string line = stringReader.ReadLine();
            Debug.Log(line);

            if (line == null)
                break;

            //#3.Creat respawn data
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }

        //#. Close text file
        stringReader.Close();

        //#.Spawn delay
        nextSpawnDelay = spawnList[0].delay;
    }

    // public 
    private void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > nextSpawnDelay && !spawnEnd) //Create enemies every random time
        {
            SpawnEnemy();
            curSpawnDelay = 0;
        }

        //#.UI Score Update
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    void SpawnEnemy()
    {
        int enemyIndex = 0;
        switch(spawnList[spawnIndex].type)
        {
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 2;
                break;
            case "B":
                enemyIndex = 3;
                break;
        }
        int enemyPoint = spawnList[spawnIndex].point;

        int ranPoint = Random.Range(0, 9); //random spawnPoint 0~8
        GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]); //Instantiate enemies in a random position
        enemy.transform.position = spawnPoints[enemyPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player; //hand over player information to the enemy
        enemyLogic.gameManager = this;
        enemyLogic.objectManager = objectManager;

        //Due to the nature of the arrangement, the number becomes -1
        if (enemyPoint == 5 || enemyPoint == 6) //Point 6, Point 7
        {
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
            enemy.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (enemyPoint == 7 || enemyPoint == 8) //Point 8, Point 9
        {
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
            enemy.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
            enemy.transform.rotation = Quaternion.Euler(0, 0, 180);
        }

        //#.puls respawn index
        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        //#.Next respawn delay update.
        nextSpawnDelay = spawnList[spawnIndex].delay;
    }

    public void UpdateLifeIcon(int life)
    {
        //#.UI life Init Disable
        for (int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }
        //#.UI life Active
        for (int index = 0; index < life; index++) //Actuate the remaining life
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon(int boom)
    {
        //#.UI Boom Init Disable
        for (int index = 0; index < 3; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 0);
        }
        //#.UI Boom Active
        for (int index = 0; index < boom; index++) //Actuate the remaining life
        {
            boomImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2f);
    }

    public void CallExplosion(Vector3 pos, string type)
    {
        GameObject explosion = objectManager.MakeObj("Explosion");
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);
    }
    void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 4f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void AllClear()
    {
        gameClearSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
