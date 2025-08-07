using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Volcano : Building
{
    float BaseEmitInterval = 2.5f;

    int EmitCount = 0;

    float CurrentTime = 0f;

    public GameObject emitObject;

    Vector3 EmitPos = Vector3.up * 0.8f;

    public bool isShrineNearby;

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        float EmitInterval = BaseEmitInterval;
        Building Holy = GameSystem.self.BuildingList.Find(x => x.isHolyShrine);
        Vector3 Dist = Holy.transform.position - transform.position;
        isShrineNearby = Dist.magnitude < Holy.Level * 2.0f;
        CurrentTime+= Time.fixedDeltaTime;

        if(emitObject != null && GameSystem.self.isOnCombatState)
        {
            if(CurrentTime > EmitInterval)
            {
                for(int i = 0;i < Level * 1.5f + 1;i++)
                {
                    GameObject tr = Instantiate(emitObject, transform.position + EmitPos, Quaternion.identity);
                    Bullet x = tr.GetComponent<Bullet>();
                    if(isShrineNearby)
                    {
                        x.transform.localScale = x.transform.localScale * 1.3f; 
                        x.Damage = x.Damage * 2;                      
                    }
                    x.Initvelocity 
                    = new Vector3(Random.Range(-1.0f,1.0f) , 0f , Random.Range(-1.0f,1.0f)) * Random.Range(3.0f, 4.0f) + Vector3.up * 2.0f;        
                    tr.SetActive(true);             
                }
                CurrentTime = 0f;
            }
        }
    }
}
