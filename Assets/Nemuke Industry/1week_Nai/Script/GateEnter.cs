using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateEnter : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && GameSystem.self.isStageCleared)
        {
            GameSystem.self.AscendStage();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
