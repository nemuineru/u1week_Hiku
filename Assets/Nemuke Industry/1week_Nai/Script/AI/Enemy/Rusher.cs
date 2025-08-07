using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rusher : Enemy
{
    public float rushWait = 0.8f;
    public float rushDelay = 2f;

    public float currentRush;
    bool isRushed = false;
    [ReadOnly]
    
    public Vector3 rushTo; 
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        currentRush = rushDelay;
    }

    //ラッシュ型も経路に沿うように移動.
    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(GameSystem.self.isOnCombatState)
        {
            Vector3 rushTo = Main.transform.position - transform.position;
            rBody.rotation = Quaternion.LookRotation(rushTo.normalized,Vector3.up);
            currentRush -= Time.fixedDeltaTime;
            if(currentRush < 0 && !isRushed)
            {
                UpdatePosList(Main.transform.position);
                rushTo = TargetPosList[positonIndex] - transform.position;
                rBody.velocity += rushTo.normalized * speed;            
                isRushed = true;
                animator.SetTrigger("Attacking");
            }
            else if(currentRush < -rushDelay)
            {
                currentRush = rushWait;
                isRushed = false;
            }
        }
    }
    
    public override void SetDamageAnim()
    {
        base.SetDamageAnim();
        currentRush = rushWait;
        isRushed = false;
    }
    
    public override void setAnim()
    {
        animator.SetBool("isRushReady",!isRushed);
    }
}
