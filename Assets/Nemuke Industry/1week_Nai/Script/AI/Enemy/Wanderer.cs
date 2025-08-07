using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wanderer : Enemy
{
    public float WanderDelay = 0.8f;
    public float WanderRange = 0.8f;

    public float rotateSpeed;
    float CurrentWander = 2f;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        UpdatePosList(transform.position + new Vector3(Random.Range(-1.0f,1.0f),0f,Random.Range(-1.0f,1.0f) * WanderRange));
        CurrentWander = WanderDelay;
    }

    public float EmitBtween = 0.3f;
    public GameObject Emits;

    float CurrentEmitTime = 0f;
    // Start is called before the first frame update

    //ラッシュ型も経路に沿うように移動.
    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if(GameSystem.self.isOnCombatState)
        {
            if(Vector3.ProjectOnPlane(rBody.velocity,Vector3.up).magnitude > 0.01f)
            {
                rBody.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(rBody.velocity,Vector3.up));
            }        
            if(CurrentWander < 0)
            {   
                UpdatePosList(transform.position + new Vector3(Random.Range(-1.0f,1.0f),0f,Random.Range(-1.0f,1.0f) * WanderRange));
                CurrentWander = WanderDelay;
            }
            if(positonIndex < TargetPosList.Count())
                rBody.velocity = Vector3.Lerp(rBody.velocity,(TargetPosList[positonIndex] - transform.position).normalized * speed,rotateSpeed);  
            CurrentWander -= Time.fixedDeltaTime;       
            

            CurrentEmitTime += Time.fixedDeltaTime;
            if(CurrentEmitTime > EmitBtween && Emits != null)
            {      
                Instantiate(Emits, transform.position, Quaternion.identity);
                CurrentEmitTime = 0;
            }
        }
    }
}
