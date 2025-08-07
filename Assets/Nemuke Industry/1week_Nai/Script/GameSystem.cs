using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    public enum GameState 
    {
        gameready,
        ongame,
        gameover
    }
    
    public GameState gameState;
    public bool isOnCombatState = false;
    

    public int MaxEnemyCount = 50;
    public int CurrentEnemyCount = 50;

    public Transform[] EnemySpawnPoint;

    public List<Enemy> EnemyList = new List<Enemy>();

    float CheckEnemyNum;

    [System.Serializable]
    public class EnemyRanks
    {
        public GameObject Enemy;
        public int MinimumDepth = 1;
        public float Rarerity = 1;
    }

    public List<EnemyRanks> SpawnEnemyList = new List<EnemyRanks>();
    
    public List<Building> SpawnBuildList = new List<Building>();


    static public GameSystem self; 
    static public Character Player;
    [SerializeField]
    public GameObject respawnObj;    
    public GameObject defeatedObj;
    
    public GameObject upGradeObj;

    [SerializeField]
    public GameObject Luminus;
    public GameObject EnemySpawnEff;
    public GameObject SweepEffect;

    public AudioSource BuySound;

    Transform buildingSpawnPos;

    float GameReadyTime = 4.0f;

    float GameReadyCurTime = 4.0f;

    public IrisManager iris;



//プレイヤーの生成時間.
    float respawnTime = 1.5f;

    float curRespawnTime = 0;

    public int PlayerLife = 2;

//到達領域.
    public int Depth = 0;
    public int CurrentLuminus = 0;
    
    public int Score = 0;

//自分が生成済み？
    bool isRespawned = false;


//敵の生成時間.
    float EnemyRespawnTime = 0.4f;
    float CurrentEnemyRespawnTime = 0;

    [ReadOnly]
    public bool isStageCleared = false;

    public GameObject GateBlock;
    public GameObject GateShow;


//スローモー.
    public float SlowedTime = 0.0f; 
    public List<Building> BuildingList;

[ReadOnly]
    public int BuildingNextCost = 0; 

    //プレイヤーの敵接触判定.
    public void onPlayerDefeated()
    {
        if (Player != null && Player.CurShieldPoint < 0)
        {
            curRespawnTime = 0;
            
            GameObject ptcl = Instantiate(defeatedObj, Player.transform.position, Quaternion.identity);
            ptcl.SetActive(true);

            Player.gameObject.SetActive(false);
            Player.CurShieldPoint = Player.ShieldPoint;
            PlayerLife -= 1;
            isRespawned = false;
        }
        if(PlayerLife < 0)
        {
            gameState = GameState.gameover;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if(self == null)
        {
            self = this;
        }        
        else
        {
            Destroy(this);
        }
        Player = GameObject.FindWithTag("Player").GetComponent<Character>();
        buildingSpawnPos = GameObject.Find("Wagon").transform;
        Player.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Score = Depth * 1000 + CurrentLuminus * 30;
        if(gameState != GameState.gameover && !iris.setTransit)
        {
            if(GameReadyCurTime > 0)
            {
                GameReadyCurTime -= Time.deltaTime;
                gameState = GameState.gameready;
            }
            else
            {
                gameState = GameState.ongame;
            }

            BuildingNextCost = (int)(Mathf.Pow(BuildingList.Count - 1, 2) * 25);
            curRespawnTime += Time.deltaTime;
            CurrentEnemyRespawnTime += Time.deltaTime;
            if(!isRespawned && curRespawnTime > respawnTime)
            {
                GameObject ptcl = Instantiate(respawnObj, Player.transform.position, Quaternion.identity);
                ptcl.SetActive(true);
                Player.gameObject.SetActive(true);
                isRespawned = true;
            }
            if(SlowedTime > 0)
            {
                SlowedTime -= Time.deltaTime;
                Time.timeScale = Mathf.Lerp(Time.timeScale,0.8f,.03f);
            }
            else
            {
                Time.timeScale = Mathf.Lerp(Time.timeScale,1f,.8f);
            }
            //敵が少なくなったら生成.
            if(isOnCombatState == true && CurrentEnemyCount > 0)
            {
                if(EnemyList.Count < Depth * 2 && EnemyRespawnTime < CurrentEnemyRespawnTime)
                {
                    SpawnEnemy();
                    CurrentEnemyRespawnTime = 0;
                }
            }
            EnemyList.RemoveAll(x => x == null);
            if(CurrentEnemyCount <= 0 && !isStageCleared)
            {
                Instantiate(SweepEffect,Player.transform.position, Quaternion.AngleAxis(-90f,Vector3.right));
                foreach(Enemy e in EnemyList)
                {
                    Destroy(e.gameObject);
                }
                isOnCombatState = false;
                isStageCleared = true;
                GateBlock.SetActive(false);
                GateShow.SetActive(true);
            }
        }
        isOnCombatState = gameState == GameState.ongame && Player.gameObject.activeSelf && !isStageCleared;        
    }

//敵が少なくなったら生成.
    void SpawnEnemy()
    {
        Vector3 setSpawn = EnemySpawnPoint[UnityEngine.Random.Range(0,EnemySpawnPoint.Length)].position;
        List<EnemyRanks> GetRank = SpawnEnemyList.FindAll( o => o.MinimumDepth <= Depth);
        float sp = 0;
        foreach (var ER in GetRank)
        {
            sp += ER.Rarerity;
        }
        float Rdm = UnityEngine.Random.Range(0,sp);
        float Cur = 0;
        GameObject spawn = GetRank[0].Enemy;
        foreach(var ER in GetRank)
        {
            if(Rdm >= Cur && Rdm <= Cur + ER.Rarerity)
            {
                spawn = ER.Enemy;
            }            
            Cur += ER.Rarerity;
        }
        GameObject EnemyObj = Instantiate(spawn,setSpawn,Quaternion.identity);

        Instantiate(EnemySpawnEff,setSpawn,EnemySpawnEff.transform.rotation);
    }

    public void BuyBuild()
    {
        if(CurrentLuminus > BuildingNextCost)
        {
            CurrentLuminus -= BuildingNextCost;
            int rd = UnityEngine.Random.Range(0,SpawnBuildList.Count);
            GameObject bd = Instantiate(SpawnBuildList[rd].gameObject , Player.transform.position,Quaternion.identity);
            bd.GetComponent<Rigidbody>().velocity = (Vector3.up + Vector3.forward) * 3.0f;
            BuySound.PlayOneShot(BuySound.clip);
        }
    }

    public void AscendStage()
    {
        iris.setTransit = true;
        if(gameState != GameState.gameover)
        {            
            GateBlock.SetActive(true);
            GateShow.SetActive(false);
            Depth++;
            MaxEnemyCount = Depth * 15;
            CurrentEnemyCount = MaxEnemyCount;
            GameReadyCurTime = GameReadyTime;
            gameState = GameState.gameready;
            for(int i = 0; i < BuildingList.Count; i++)
            {
                Vector3 placePos = Quaternion.Euler(0f, 360f * i / BuildingList.Count,0f) * (Vector3.forward * 1.5f);
                Player.transform.position = Vector3.zero;
                BuildingList[i].transform.position = placePos;
            }
            curRespawnTime = 0;
            isRespawned = false;
            isStageCleared = false;
            Player.gameObject.SetActive(false);
        }
    }

    public void SceneReload()
    {
        SceneManager.LoadScene("GameScene");
    }
    
    public void SceneBack()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
