using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    //換金可能なら
    public enum ITEMTYPE
    {
        JUNK,
        TREASURE
    }

    [SerializeField]
    internal ITEMTYPE itemType = ITEMTYPE.JUNK;

    [SerializeField]
    internal Material OutlineCol;

    [SerializeField]
    internal float weight = 0.10f;


    //水中のDrag値
    const float waterGravDrag = 0.001f;

    //つかみ距離.
    const float GRABBINGAREA = 1.5f;
    const float minimumRotate = 0.001f;
    const float lerp = 0.03f;
    Vector3 speed = new Vector3(0.20f, 0.12f);
    float mxspeed = 6.0f;
    Rigidbody selfBody;

    GameObject PlayerFind;
    PlayerMovement PMove;

    bool isCarrying = false;

    

    // Start is called before the first frame update
    void Start()
    {
        //get material and self-rigidbody.
        //自動的に複製される.
        OutlineCol = GetComponent<MeshRenderer>().material;
        selfBody = GetComponent<Rigidbody>();

        //Playerで探す.
        PlayerFind = GameObject.FindWithTag("Player");
        if (PlayerFind != null)
        {
            PMove = PlayerFind.GetComponent<PlayerMovement>();
        }
        selfBody.mass = weight;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        setSelfVel();
        setCarry();
    }

    void setSelfVel()
    {
        if (!isCarrying)
        {
            //重力値の設定..
            selfBody.velocity += Physics.gravity * Time.fixedDeltaTime * (selfBody.position.y >= 0 ? 3.0f : waterGravDrag);
        }
    }

    //持ち運び時の処理. RayCastで当てられたとき..
    void setCarry()
    {
        Vector3 PPos_Dif = PlayerFind.transform.position - transform.position;
        //スクリーンポイントのZ=0面位置での距離など.
        Vector3 scPos_Dif = transform.position - InputInstance.self.inputValues.ScreenPosCalc();
        //inputValuesが押された際..
        if (PPos_Dif.magnitude < PMove.MxArmRange && scPos_Dif.magnitude < GRABBINGAREA)
        {
            Debug.Log("Can Carry " + gameObject.name);
            //範囲内なら..
            if (InputInstance.self.inputValues.LeftClickRead > 0)
            {
                PMove.registerCarry(this);
            }
            //マテリアルカラーのアウトラインを変更..(白)
            OutlineCol.SetColor("_OutlineColor", Color.white);
        }
        else
        {
            //マテリアルカラーのアウトラインを変更..(黄色)
            OutlineCol.SetColor("_OutlineColor" , Color.yellow);
        }
    }

    internal void setCarrying(bool setVal, Rigidbody armBody)
    {
        selfBody.isKinematic = setVal;
        isCarrying = setVal;
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
