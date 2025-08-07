using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : Enemy
{
    bool isRushed = false;
    [ReadOnly]
    
    public Vector3 stalkTo; 

    public MovementSet moveSet = MovementSet.Constant;

    public enum MovementSet
    {
        Constant,
        Accel
    }

    public float EmitBtween = 0.3f;
    public GameObject Emits;

    float CurrentEmitTime = 0f;
    // Start is called before the first frame update

    public override void Start()
    {
        base.Start();
    }

    //ラッシュ型も経路に沿うように移動.
    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(GameSystem.self.isOnCombatState)
        {
            stalkTo = Main.transform.position - transform.position;
            switch(moveSet)
            {
                case MovementSet.Constant:
                rBody.velocity = stalkTo.normalized * speed;   
            break;
            case MovementSet.Accel:
                rBody.velocity += stalkTo.normalized * speed;   
            break;
            }
            rBody.rotation = Quaternion.LookRotation(stalkTo.normalized,Vector3.up);
            CurrentEmitTime += Time.fixedDeltaTime;
            if(CurrentEmitTime > EmitBtween && Emits != null)
            {      
                Instantiate(Emits, transform.position, Quaternion.identity);
                CurrentEmitTime = 0;
            }
        }
    }
}
