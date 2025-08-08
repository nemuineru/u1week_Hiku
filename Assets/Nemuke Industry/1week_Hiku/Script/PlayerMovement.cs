using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//動作方向を x-y方面に制限. zは0に. 
public class PlayerMovement : MonoBehaviour
{
    //carryedItemsを登録し、その動きに合わせて自分の動きを制限する.
    //アーム作らないとなー.
    [SerializeField]
    List<ItemMovement> carryedItems;
    const float waterGravDrag = 0.001f;
    const float minimumRotate = 0.001f;
    const float lerp = 0.3f;

    //MaxArm Range. これを超えると移動に制限がかかる.
    const float MxArmRange = 4.0f;
    Vector3 speed = new Vector3(0.20f, 0.12f);
    float mxspeed = 2.5f;
    Rigidbody selfBody;
    // Start is called before the first frame update
    void Start()
    {
        selfBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerVelSet();
    }

    void PlayerVelSet()
    {
        //重力値の設定..
        selfBody.velocity += Physics.gravity * Time.fixedDeltaTime * (selfBody.position.y >= 0 ? 3.0f : waterGravDrag);
        Vector3 SetSpeed = Vector3.zero;
        //y軸が0以下のとき潜水可能.
        if (selfBody.position.y < 0)
        {
            SetSpeed = speed * InputInstance.self.inputValues.Movement;
        }
        selfBody.AddForce(SetSpeed, ForceMode.VelocityChange);
        float velMag = selfBody.velocity.magnitude;
        //設定速度の規定を超えないようにする.
        if (velMag > mxspeed)
        {
            selfBody.velocity = selfBody.velocity.normalized * mxspeed;
        }
        //Abs値により自己を回転する.
        if (Mathf.Abs(velMag) > minimumRotate)
        {
            //真っ逆さまにはしない.
            Vector3 tilt = Mathf.Sign(selfBody.velocity.x) * Vector3.right * 1.0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(selfBody.velocity + tilt), lerp);
        }
    }

    void ItemCarrySet()
    {
        foreach (ItemMovement i in carryedItems)
        {
            if ((transform.position - i.transform.position).magnitude > MxArmRange)
            { 
                
            }
        }
    }
}
