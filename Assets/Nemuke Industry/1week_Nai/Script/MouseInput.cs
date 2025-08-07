using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    public Character player;

    public ParticleSystem emitter;
    Vector3 position = Vector3.zero;

    float ClickInputTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string[] st = {"Default","Terrain"};
        string[] build = {"Building"};
        if( InputInstance.self.inputValues.LeftClick > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay( InputInstance.self.inputValues.ScreenMousePos);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit ,1000f, LayerMask.GetMask(st)))
            {
                position = hit.point;
                player.WishPos = position;
                player.UpdatePosList();
                emitter.transform.position = position + Vector3.up * 0.20f;
                if(!emitter.isPlaying)
                {
                    emitter.Play();
                }
            }
            if(Physics.Raycast(ray, out hit ,1000f, LayerMask.GetMask(build)))
            {
                var selected = hit.collider.gameObject.GetComponent<Building>();
                Vector3 buildDist = (selected.transform.position - player.transform.position);
                if(buildDist.magnitude < 0.8f)
                {
                    player.setBuilding(selected);
                }
            }
        }
        else
        {
            if(emitter.isPlaying)
            {
                emitter.Stop();
            }
        }
        //右クリックで投擲.
        {
            if(InputInstance.self.inputValues.RightClick > 0)
            {
                player.ThrowBuilding();
            }
        }
    }
}
