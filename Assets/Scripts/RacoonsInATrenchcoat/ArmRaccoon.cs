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
    public Transform PivotPoint;
    public ArmType ArmType;
    private InputAction action1;
    private Vector3 startRot;

    private void Start()
    {
        startRot = transform.rotation.eulerAngles;
        action1 = InputSystem.actions.FindAction("LeftArmRaccoonAction1");

    }

    private void Update()
    {
        MoveArmsXAxis();
    }

    private void MoveArmsXAxis()
    {

        float targetRot = startRot.z;
        Vector3 rotDirection = ArmType == ArmType.LeftArm ? transform.forward : -transform.forward;

        if (action1.IsPressed() == true)
        {
            targetRot = ArmType == ArmType.RightArm ? 90.0f : 270.0f;
            rotDirection = -rotDirection;
        }

        float rotateDegs = Mathf.Abs(targetRot - transform.rotation.eulerAngles.z);



        transform.RotateAround(PivotPoint.position, rotDirection, Mathf.Min(0.1f, rotateDegs));

        if(ArmType == ArmType.LeftArm)
        {
            return;
        }
        
        Debug.Log(rotateDegs);
        if (rotateDegs <= 0.01f && ArmType == ArmType.RightArm)
        {
            //Debug.Log(transform.rotation.eulerAngles);
            //Debug.Log(startRot);
        }
    }
}
