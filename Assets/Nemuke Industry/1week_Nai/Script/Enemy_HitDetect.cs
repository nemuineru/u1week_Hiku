using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HitDetect : MonoBehaviour
{
    // Start is called before the first frame update
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {   
            GameSystem.self.onPlayerDefeated();
            Enemy ene = transform.parent.GetComponent<Enemy>();
            ene.HitPoint = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
