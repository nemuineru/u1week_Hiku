using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.AI.Navigation;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Character : MonoBehaviour
{
    public float ShieldPoint = 5f;
    public float CurShieldPoint = 5f;
    public Rigidbody charRBody;
    public Animator charAnim;
    public Building buildHolded;

    public GameObject ShieldObj;

    public Vector3 WishPos;
    public Vector3[] TargetPosList;

    [SerializeField, ReadOnly]
    int positonIndex = 0;

    [SerializeField, ReadOnly]
    public NavMeshPath navPath;
    // Start is called before the first frame update
    void Awake()
    {
        charRBody = GetComponent<Rigidbody>();
        charAnim = GetComponent<Animator>();
    }

    public float speed = 50.0f;
    
    float curSpeed = 50.0f;

    float slowDist = 1f;

    Vector3 HoldPos = Vector3.up * 0.8f;

    // Update is called once per frame
    void FixedUpdate()
    {        
        if(CurShieldPoint > 0)
        {
            ShieldObj.SetActive(true);
            CurShieldPoint -= Time.fixedDeltaTime;
        }
        else
        {
            ShieldObj.SetActive(false);
        }
        anim_isCarrying = (buildHolded != null);
        if(buildHolded)
        {
            curSpeed = speed / 1.3f;
            buildHolded.transform.parent = transform;
            buildHolded.transform.localPosition = HoldPos;
            buildHolded.transform.rotation = transform.rotation;
            buildHolded.isCarryed = true;
        }
        else
        {
            curSpeed = Mathf.Lerp(curSpeed, speed, 0.1f);
        }

        if(TargetPosList.Count() > 1 && positonIndex < TargetPosList.Count())
        {
            Vector3 moveTowards =  TargetPosList[positonIndex] - transform.position;
            Vector3 solver = Vector3.ProjectOnPlane(moveTowards, Vector3.up);


            float setMinimum = 1.0f;
            if(positonIndex + 1 == TargetPosList.Count())
            {
                setMinimum = Mathf.Min(Mathf.Pow(solver.magnitude / slowDist, 0.5f), 1.0f) * curSpeed;
            }

            charRBody.velocity += solver.normalized * setMinimum * Time.fixedDeltaTime;
            if(Vector3.Magnitude(solver) < 0.05f)
            {
                positonIndex++;
            }
            Quaternion rotateTowards = Quaternion.LookRotation(charRBody.velocity.normalized, Vector3.up);
            charRBody.rotation =  Quaternion.Lerp(transform.rotation, rotateTowards , .5f);
        }
        AnimUpdate();
    }

    public void setBuilding(Building build)
    {
        if(buildHolded != build)
        {
            if(buildHolded != null)
            {
                buildHolded.transform.parent = null;
                buildHolded.isCarryed = false;
            }
            build.transform.position = transform.position;
            buildHolded = build;
            build.isCarryed = true;            
        }
    }

    public void ThrowBuilding()
    {
        if(buildHolded != null)
        {
            charAnim.SetTrigger("Throwing");
            buildHolded.isCarryed = false;
            buildHolded.transform.parent = null;
            buildHolded.Throwing();
            buildHolded = null;
        }
    }

    public void UpdatePosList()
    {
        navPath = new NavMeshPath();
        NavMesh.SamplePosition(transform.position, out NavMeshHit hits,10f,NavMesh.AllAreas);
        if( NavMesh.CalculatePath(hits.position, WishPos, NavMesh.AllAreas, navPath))
        {
            TargetPosList = navPath.corners;
        }
        positonIndex = 1;
    }

    bool anim_isCarrying = false;

    void AnimUpdate()
    {
        charAnim.SetFloat("speed", charRBody.velocity.magnitude);
        charAnim.SetBool("isCarrying", anim_isCarrying);
    }

    public void BuildingLevelUp()
    {
        buildHolded.BuildingLevelUp();
    }
}
