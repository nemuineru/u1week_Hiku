using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.Math;
using UnityEngine;
using UnityEngine.UI;

public class IrisManager : MonoBehaviour
{
    public RawImage rawImage;
    float TransitInTime = 0.5f;
    float TransitWaitTime = 0.8f;
    float TransitOutTime = 0.5f;
    float CurTime = 0.0f;

    public bool setTransit = false; 

    Rect IrisOut = new Rect(0.25f,0.25f,0.5f,0.5f);
    Rect IrisIn = new Rect(-100f,-100f,201f,201f);
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (setTransit)
        {
            CurTime += Time.deltaTime;
            SetTransit();
        }
    }

    void SetTransit()
    {
        if(CurTime < TransitInTime)
        {   
            IrisInInvoke();
        }
        else if(CurTime > TransitInTime + TransitWaitTime && CurTime < TransitInTime + TransitWaitTime + TransitOutTime)
        {
            IrisOutInvoke();
        }
        else if(CurTime > TransitInTime + TransitWaitTime + TransitOutTime)
        {
            CurTime = 0f;
            setTransit = false;
        }
    }
    
    public void IrisOutInvoke()
    {
        rawImage.uvRect = LerpRect(rawImage.uvRect, IrisOut, 0.6f);
    }

    public void IrisInInvoke()
    {
        rawImage.uvRect = LerpRect(rawImage.uvRect, IrisIn, 0.6f);
    }

    Rect LerpRect(Rect a, Rect b, float t)
    {
        Rect newRect = new Rect(Vector2.Lerp(a.position,b.position,t), Vector2.Lerp(a.size,b.size,t));
        return newRect;
    }
}
