using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float RemainTime = 0.05f;
    // Start is called before the first frame update

    public GameObject HitEff;
    public GameObject EmitObj;

    public Building selfBuild;

    public Vector3 Initvelocity = Vector3.zero;

    public int Damage = 3;
    
    Rigidbody rBody;

    void Awake()
    {
        rBody = GetComponent<Rigidbody>();
        rBody.velocity = Initvelocity;
    }
    
    //空中にいるとき当たり判定が発生する.
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {   
            Enemy ene = other.GetComponent<Enemy>();
            if(ene != null)
            {            
                AddDmg(ref ene);
                if(HitEff != null)
                {                    
                    Vector3 midPos = (ene.transform.position + transform.position) / 2f;
                    Instantiate(HitEff,midPos,Quaternion.identity);
                }                
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RemainTime -= Time.fixedDeltaTime;
        if(RemainTime < 0 || transform.position.y < 0)
        {
            if(EmitObj != null)
            {
                GameObject obj = Instantiate(EmitObj,transform.position + Vector3.up * 0.1f,Quaternion.identity);
                obj.transform.localScale = transform.localScale;
                obj.SetActive(true);
            }
            Destroy(gameObject);
        }
    }

    public void AddDmg(ref Enemy enemy)
    {
        if (enemy.gameObject != null)
        {
            enemy.AddDmg((transform.forward  * 4f + Vector3.up), Damage);
            enemy.SetDamageAnim();
        }
    }
}
