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
    private float rotateSpeed = 250.0f;

    private Quaternion startRot;

    private void OnEnable()
    {

        action1 = action1reference.action;
        action2 = action2reference.action;

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
    }
    private void Action2_started(InputAction.CallbackContext context)
    {
        action2Pressed = true;
    }
    private void Action1_canceled(InputAction.CallbackContext obj)
    {
        action1Pressed = false;
    }
    private void Action2_canceled(InputAction.CallbackContext obj)
    {
        action2Pressed = false;
    }

    private void Awake()
    {
        startRot = transform.rotation;
    }

    private void Update()
    {
        float targetZ = GetTargetZAxis();
        float targetX = GetTargetXAxis();

        RotateArm(targetX, targetZ);
    }

    private float GetTargetXAxis()
    {
        if(action2Pressed)
        {
            return 90.0f;
        }
        return startRot.x;
    }

    private float GetTargetZAxis()
    {
        if(action1Pressed)
        {
            return (ArmType == ArmType.RightArm ? 90.0f : 270.0f);
        }
        return startRot.z;
    }

    private void RotateArm(float x, float z)
    {
        Quaternion targetRot = Quaternion.Euler(x, startRot.y, z);

        PivotPoint.rotation = Quaternion.RotateTowards(PivotPoint.rotation, targetRot, rotateSpeed * Time.deltaTime);
        
        PivotPoint.rotation = Quaternion.RotateTowards(PivotPoint.rotation, targetRot, rotateSpeed * Time.deltaTime);
    }
}
