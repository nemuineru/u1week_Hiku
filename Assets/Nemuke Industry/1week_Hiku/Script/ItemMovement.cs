using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    [SerializeField]
    internal float weight = 0.10f;
    const float waterGravDrag = 0.001f;
    const float minimumRotate = 0.001f;
    const float lerp = 0.03f;
    Vector3 speed = new Vector3(0.20f, 0.12f);
    float mxspeed = 6.0f;
    Rigidbody selfBody;
    // Start is called before the first frame update
    void Start()
    {
        selfBody = GetComponent<Rigidbody>();
        selfBody.mass = weight;
    }

    // Update is called once per frame
    void Update()
    {        
        //重力値の設定..
        selfBody.velocity += Physics.gravity * Time.fixedDeltaTime * (selfBody.position.y >= 0 ? 3.0f : waterGravDrag);
    }

    internal void setCarrying(bool setVal)
    {
        selfBody.isKinematic = setVal;
    }
    internal void setPulse(Vector3 pulseAdd)
    {
        selfBody.AddForce(pulseAdd, ForceMode.Impulse);
    }
    
    internal void setPos(Vector3 positions)
    {
        selfBody.position = (positions);
    }

}
