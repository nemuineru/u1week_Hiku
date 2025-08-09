using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputInstance : MonoBehaviour
{
    public InputValueManagers inputValues =  new InputValueManagers();
    static public InputInstance self;
    // Start is called before the first frame update
    
    Input_Basic inputBasic;

    public class InputValueManagers
    {
        public int LeftClickRead = 0;
        public int RightClickRead = 0;
        public Vector2 Movement = Vector2.zero;

        public float LeftClick
        {
            get { return self.inputBasic.Base.Select_Click.ReadValue<float>(); }
        }


        public float RightClick
        {
            get { return self.inputBasic.Base.Attack_Click.ReadValue<float>(); }
        }


        public Vector2 MovementInput
        {
            get { return self.inputBasic.Base.ControllerAxis.ReadValue<Vector2>(); }
        }

        private Vector2 mousepos;

        public Vector2 ScreenMousePos
        {
            get
            {
                return self.inputBasic.Base.CursorPosition.ReadValue<Vector2>();
            }
        }

        public void AnalogButtonSet(float vals, ref int inputNum)
        {
            if (vals > 0)
            {
                if (inputNum >= 0)
                {
                    inputNum += 1;
                }
            }
            else
            {
                if (inputNum > 0)
                {
                    inputNum = -1;
                }
                else
                {
                    inputNum = 0;
                }
            }
        }

        public Vector3 ScreenPosCalc()
        {
            Plane ZPlane = new Plane(Vector3.forward, Vector3.zero);
            Vector3 XYpos = Vector3.zero;

            Vector3 scPos_Raw = InputInstance.self.inputValues.ScreenMousePos;

            Ray ray = Camera.main.ScreenPointToRay(scPos_Raw);


            scPos_Raw.z = 1.0f;

            //この状態だとZ=0のカメラ位置が取れない.
            Vector3 scPos = Camera.main.ScreenToWorldPoint(scPos_Raw);
            Vector3 campos = Camera.main.transform.position;

            //Z=0上のPlaneにHitした際..
            if(ZPlane.Raycast(ray,out var enter))
            {
                XYpos = ray.origin + ray.direction.normalized * enter;
            }

            //Debug.Log("Point At " + XYpos);

            Debug.DrawLine(XYpos, Camera.main.transform.position);
            Debug.DrawLine(XYpos, XYpos + Vector3.up);
            Debug.DrawLine(XYpos, XYpos - Vector3.forward);
            Debug.DrawLine(XYpos, XYpos - Vector3.right);
            return XYpos;
        }
    }
    
    void Awake()
    {
        if (self != null)
        {
            Destroy(self);
        }
        else
        {
            self = this;
        }
        inputBasic = new Input_Basic();
        inputBasic.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        inputValues.AnalogButtonSet(inputValues.LeftClick, ref inputValues.LeftClickRead);
        inputValues.AnalogButtonSet(inputValues.RightClick, ref inputValues.RightClickRead);
        inputValues.Movement = inputValues.MovementInput;
    }
}
