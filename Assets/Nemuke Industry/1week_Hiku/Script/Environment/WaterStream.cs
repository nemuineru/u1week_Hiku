using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterStream : MonoBehaviour
{
    [SerializeField]
    float Power = 3.0f;
    [SerializeField]
    float effectRange = 3.0f;
    // Start is called before the first frame update
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Item")
        {
            var Rigid = other.GetComponent<Rigidbody>();
            Vector3 vectTowards = Vector3.Project(other.transform.position - transform.position, transform.forward);
            Rigid.AddForce(vectTowards.normalized * Power * (effectRange - vectTowards.magnitude));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
