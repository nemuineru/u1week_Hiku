using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Building_HitDetect : MonoBehaviour
{
    public Building self;

    //空中にいるとき当たり判定が発生する.
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" && self.isOnGround == false && self.isCarryed != true)
        {   
            Enemy ene = other.GetComponent<Enemy>();
            if(ene != null)
            {            
                self.AddDmg(ref ene);
            }
        }
    }
}
