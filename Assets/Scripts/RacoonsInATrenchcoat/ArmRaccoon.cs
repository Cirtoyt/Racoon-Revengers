using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ArmType
{
    LeftArm,
    RightArm
}

public class ArmRaccoon : MonoBehaviour
{
    [SerializeField]
    private Transform PivotPoint;
    [SerializeField]
    private ArmType ArmType;

    [SerializeField]
    private InputActionReference action1reference;
    [SerializeField]
    private InputActionReference action2reference;
    private InputAction action1;
    private InputAction action2;

    private bool action1Pressed;
    private bool action2Pressed;

    [SerializeField]
    private float rotateSpeed = 5.0f;

    private Vector3 startRot;

    public HandGrab handGrab;

    private void Awake()
    {
        action1 = action1reference.action;
        action2 = action2reference.action;
    }

    private void Start()
    {
        startRot = PivotPoint.rotation.eulerAngles;
    }

    private void OnEnable()
    {
        action1.started += Action1_started;
        action2.started += Action2_started;

        action1.canceled += Action1_canceled;
        action2.canceled += Action2_canceled;
    }

    private void OnDisable()
    {
        action1.started -= Action1_started;
        action2.started -= Action2_started;

        action1.canceled -= Action1_canceled;
        action2.canceled -= Action2_canceled;
    }

    private void Action1_started(InputAction.CallbackContext obj)
    {
        action1Pressed = true;

        if(handGrab)
        {
            handGrab.SetGrabbing(true);
        }
    }
    private void Action2_started(InputAction.CallbackContext context)
    {
        action2Pressed = true;

        if (handGrab)
        {
            handGrab.SetGrabbing(true);
        }
    }
    private void Action1_canceled(InputAction.CallbackContext obj)
    {
        action1Pressed = false;

        if (handGrab)
        {
            handGrab.SetGrabbing(action2Pressed);
        }
    }
    private void Action2_canceled(InputAction.CallbackContext obj)
    {
        action2Pressed = false;

        if (handGrab)
        {
            handGrab.SetGrabbing(action1Pressed);
        }
    }

    private void FixedUpdate()
    {
        RotateArm();
    }

    private void RotateArm()
    {
        int xLook = 0;
        int yLook = -1;
        int zLook = 0;
        if(action1Pressed)
        {
            zLook = 1;
            yLook = 0;
        }
        if(action2Pressed)
        {
            xLook = ArmType == ArmType.RightArm ? 1 : -1;
            yLook = 0;
        }
        Quaternion targetRot = Quaternion.LookRotation(new Vector3(xLook, yLook, zLook), Vector3.up);

        PivotPoint.localRotation = Quaternion.Slerp(PivotPoint.localRotation, targetRot, rotateSpeed * Time.deltaTime);
    }
}
