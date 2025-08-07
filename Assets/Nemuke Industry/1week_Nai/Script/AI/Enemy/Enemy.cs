using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;
using System.Linq;

public class Enemy : MonoBehaviour
{
    public float HitStop = 0.3f;
    public float HitPoint = 10f;
    public float speed = 10f;
    public GameObject onDestroyEff;
    public int Score = 5;
    internal Rigidbody rBody;
    
    internal Animator animator;

    internal NavMeshPath navPath;
    internal Vector3[] TargetPosList = new Vector3[4];
    internal int positonIndex = 1;

    internal Character Main;

    public void Awake()
    {
        Main = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        animator = GetComponent<Animator>();
        rBody = GetComponent<Rigidbody>();
    }

    public virtual void Start()
    {
        GameSystem.self.EnemyList.Add(this);
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        GetCurrentPosList();
        if(HitPoint <= 0)
        {
            for(int i = 0; i < Score;i++)
            {
                Instantiate(GameSystem.self.Luminus ,transform.position, Quaternion.identity);
            }
            Instantiate(onDestroyEff,transform.position, Quaternion.identity);
            GameSystem.self.SlowedTime += HitStop;
            if(Score != 0)
            {
                GameSystem.self.CurrentEnemyCount--;
                GameSystem.self.EnemyList.Remove(this);
            }
            Destroy(gameObject);
        }
        setAnim();
    }

    public void AddDmg(Vector3 setVect, float dmgs)
    {
        rBody.velocity = setVect;
        GameSystem.self.SlowedTime += 0.01f;
        HitPoint -= dmgs;
    }

    

    public void UpdatePosList(Vector3 wishPos)
    {
        navPath = new NavMeshPath();
        NavMesh.SamplePosition(transform.position, out NavMeshHit hits,4f,NavMesh.AllAreas);
        if( NavMesh.CalculatePath(hits.position, wishPos, NavMesh.AllAreas, navPath))
        {
            TargetPosList = navPath.corners;
            positonIndex = 1;
        }
        else
        {
            positonIndex = 0;
        }        
    }

    public void GetCurrentPosList()
    {        
        if(positonIndex < TargetPosList.Count())
        {
            Vector3 moveTowards =  TargetPosList[positonIndex] - transform.position;
            Vector3 solver = Vector3.ProjectOnPlane(moveTowards, Vector3.up);
            if(Vector3.Magnitude(solver) < 0.05f)
            {
                positonIndex++;
            }
        }
    }

    public virtual void SetDamageAnim()
    {
        animator.SetTrigger("Damaged");
    }
    
    public virtual void setAnim()
    {

    }
}
