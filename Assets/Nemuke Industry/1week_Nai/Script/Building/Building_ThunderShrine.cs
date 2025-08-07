using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_ThunderShrine : Building
{
    float BaseEmitInterval = 0.8f;
    float Burst = 0.10f;

    int EmitCount = 0;

    float CurrentTime = 0f;

    public GameObject emitObject;

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        float EmitInterval = BaseEmitInterval - Level * 0.03f;
        Building Holy = GameSystem.self.BuildingList.Find(x => x.isHolyShrine);
        Vector3 Dist = Holy.transform.position - transform.position;
        CurrentTime += Time.fixedDeltaTime;
        Quaternion FaceEnemy = Quaternion.LookRotation(Vector3.ProjectOnPlane(FindEnemyNearPos() - transform.position, Vector3.up).normalized, Vector3.up);

        if(emitObject != null && CurrentTime > EmitInterval && GameSystem.self.isOnCombatState)
        {
            if(EmitCount == 0 
            && Dist.magnitude < 2.0f * Holy.Level )
            {
                GameObject tr = Instantiate(emitObject,transform.position + Vector3.up * 0.01f, FaceEnemy);
                tr.SetActive(true);

                EmitCount++;
            }
            if(CurrentTime > EmitInterval + Burst)
            {
                GameObject tr = Instantiate(emitObject,transform.position + Vector3.up * 0.01f, FaceEnemy);
                tr.SetActive(true);

                EmitCount = 0;
                CurrentTime = 0f;
            }
        }
    }
}

