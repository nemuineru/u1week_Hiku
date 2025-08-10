using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//動作方向を x-y方面に制限. zは0に. 
public class PlayerMovement : MonoBehaviour
{
    //carryedItemsを登録し、その動きに合わせて自分の動きを制限する.
    //アーム作らないとなー.
    [SerializeField]
    List<ItemMovement> carryedItems;

    //ArmMovementを登録し、Handに設定されたRigidBodyを動かす..
    [SerializeField]
    ArmMovement arm;
    const float waterGravDrag = 0.005f;
    const float minimumRotate = 0.001f;
    const float Plerp = 0.3f;

    //MaxArm Range. これを超えると移動に制限がかかる.
    internal float MxArmRange = 3.1f;
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
        ItemCarrySet();
        carryedOnce = false;
    }

    void PlayerVelSet()
    {
        //重力値の設定..
        selfBody.velocity += Physics.gravity * Time.fixedDeltaTime *
        (selfBody.position.y >= 0 ? 3.0f : waterGravDrag * (1.0f + carryedItems.Count));

        Vector3 SetSpeed = Vector3.zero;
        //y軸が0以下のとき潜水可能. また、所持重量に基づいて速度を落とす.
        if (selfBody.position.y < 0)
        {
            SetSpeed = speed * InputInstance.self.inputValues.Movement / (1.0f + carryedItems.Count);
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
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(selfBody.velocity + tilt), Plerp);
        }
    }

    bool carryedOnce = false;
    //アイテム回収挙動.
    internal void registerCarry(ItemMovement item)
    {
        if (!carryedItems.Any(it => it.GetHashCode() == item.GetHashCode()) && carryedOnce != true)
        {
            carryedItems.Add(item);
            carryedOnce = true;
        }
    }

    //アイテム運搬全般.
    void ItemCarrySet()
    {
        //左クリックを押している間はItem対象にRayが出る.
        if (InputInstance.self.inputValues.LeftClickRead > 0)
        {
            //スクリーンポイントのZ=0面位置での距離など.
            Vector3 scPos_Dif = InputInstance.self.inputValues.ScreenPosCalc() - transform.position;
            Ray GrabVerd = new Ray(transform.position, scPos_Dif);
            LayerMask msk = LayerMask.GetMask("Terrain", "Item");
            if (Physics.Raycast(GrabVerd, out var HitInfo, MxArmRange, msk))
            {
                GameObject HitOBJ = HitInfo.transform.gameObject;
                if (HitOBJ.tag == "Item")
                {
                    HitOBJ.GetComponent<ItemMovement>();
                }
            }
            
        }
        //もしアイテム回収範囲にいるなら持ち出す.
        //アイテム登録位置において、そのインデックス分配列させる.
        Vector3 separationDist_Start = Vector3.left * 0.3f;
        Vector3 separationDist_End = Vector3.right * 0.3f;
        for (int index = 0; index < carryedItems.Count; index++)
        {
            //配列数分の距離設定.
            Vector3 SetCarryPos = carryedItems.Count > 1 ?
            separationDist_Start + (separationDist_End - separationDist_Start) * ((index + 1.0f) / carryedItems.Count) :
            Vector3.zero;

            ItemMovement it = carryedItems[index];
            if (it != null)
            {
                //右クリックが押されたら離す.
                if (InputInstance.self.inputValues.RightClickRead > 0)
                {
                    ARMOFF(it);
                    continue;
                }

                if (it.transform.parent == null)
                {
                    ARMON(it);
                }
                it.transform.localPosition = Vector3.Lerp(it.transform.localPosition,SetCarryPos,Plerp);
                
                //距離が離れた際の作用..
                Vector3 e = (it.transform.position - transform.position);
                float tempermix = e.magnitude - MxArmRange;
                if (tempermix > 0)
                {
                    //作用反作用.
                    selfBody.AddForce(it.weight * e.normalized * tempermix, ForceMode.Force);
                    arm.Hand.AddForce(-it.weight * e.normalized * tempermix * Time.fixedDeltaTime, ForceMode.Force);
                }
            }
        }
        //nullになったcarryedItemを削除.
        carryedItems.RemoveAll(i => i == null || i.transform.parent == null);
    }

    //アームを離す動作.
    void ARMOFF(ItemMovement it)
    {
        it.setCarrying(false, selfBody);
        it.transform.parent = null;
        it.setPulse(arm.Hand.velocity / it.weight);
    }

    //アームを掴む動作.

    void ARMON(ItemMovement it)
    {
        arm.Hand.AddForce((it.transform.position - arm.Hand.position).normalized * Time.fixedDeltaTime, ForceMode.Force);
        arm.Hand.MovePosition(it.transform.position);
        it.transform.parent = arm.CarryPoint.transform;
        it.setCarrying(true, arm.Hand);
    }
}
