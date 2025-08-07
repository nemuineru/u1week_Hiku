using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Building : MonoBehaviour
{
    public string BuildName = "";
    public float AttackPower = 10f;
    public bool isCarryed = false;
    public Rigidbody buildRBody;

    [ReadOnly]
    public bool isOnGround;

    public int Level = 1;
    public int LevelMax = 5;

    public int LevelCost;

    float ThrowPower = 5.0f;

    Collider coll;

    public bool isHolyShrine;

    public static Building ShrineBuild;

    public GameObject ThrowHit;
    

    float LevelPoint = 0.0f;
    // Start is called before the first frame update
    void Awake()
    {
        buildRBody = GetComponent<Rigidbody>();
        if(this.isHolyShrine)
        {
            ShrineBuild = this;
        }
    }
    
    void Start()
    {
        GameSystem.self.BuildingList.Add(this);
    }

    public void BuildingLevelUp()
    {
        int Cost = (int)(LevelCost * Mathf.Pow(Level, 2));
        if(Level < LevelMax && GameSystem.self.CurrentLuminus > Cost)
        {
            Level++;
            Instantiate(GameSystem.self.upGradeObj,transform.position,Quaternion.identity);
            GameSystem.self.CurrentLuminus -= LevelCost;
        }
    }
    
    

    // Update is called once per frame
    public virtual void FixedUpdate()
    {                
        Ray ray = new Ray(transform.position + Vector3.up * Physics.defaultContactOffset, Vector3.down);
        RaycastHit hit;
        string[] st = {"Default","Terrain"};
        Physics.Raycast(ray ,out hit, 0.1f, LayerMask.GetMask(st));
        isOnGround = hit.collider;
        buildRBody.isKinematic = isCarryed;
    }

    public void Throwing()
    {
        buildRBody.isKinematic = false;
        buildRBody.velocity = transform.forward * ThrowPower + Vector3.up * 2.0f;
    }

    public void AddDmg(ref Enemy enemy)
    {
        if (enemy.gameObject != null)
        {
            if(ThrowHit != null)
            {
                Vector3 midPos = (enemy.transform.position + transform.position) / 2f;
                Instantiate(ThrowHit,midPos,Quaternion.identity);
            }
            enemy.AddDmg((transform.forward + Vector3.up) * 2f, AttackPower * Level);
            enemy.SetDamageAnim();
        }
    }
    
    public Vector3 FindEnemyNearPos()
    {
        Vector3 NearPos = Vector3.zero;
        Enemy[] EnemyNearBy = GameSystem.self.EnemyList.ToArray();
        if(EnemyNearBy != null && EnemyNearBy.Length > 0)
        {
            float calc = Mathf.Infinity;
            foreach(Enemy enemy in EnemyNearBy)
            {
                if(enemy != null)
                {
                    Vector3 distance = transform.position - enemy.transform.position;
                    if(calc > distance.magnitude)
                    {
                        NearPos = enemy.transform.position;
                        calc = distance.magnitude;
                    }
                }
            }
            return NearPos;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
