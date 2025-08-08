using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion;
using UnityEngine;

//Setting for Arm movement. set parent fw axis always facing gamecam north.
public class ArmMovement : MonoBehaviour
{

    [SerializeField]
    internal Rigidbody Hand;
    [SerializeField]
    internal Transform CarryPoint;
    
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward,Vector3.up);
    }
}
