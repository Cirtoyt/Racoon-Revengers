using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ArmType
{
    LeftArm,
    RightArm
}

public class ArmRacoon : MonoBehaviour
{
    public Transform PivotPoint;
    public ArmType ArmType;
    private InputAction action1;
    private Vector3 startRot;

    private void Start()
    {
        startRot = transform.rotation.eulerAngles;
        action1 = InputSystem.actions.FindAction("LeftArmRacoonAction1");

    }

    private void Update()
    {
        MoveArmsXAxis();
    }

    private void MoveArmsXAxis()
    {

        float targetRot = startRot.x;
        Vector3 rotDirection = ArmType == ArmType.LeftArm ? Vector3.left : Vector3.right;

        if (action1.IsPressed() == true)
        {
            targetRot = ArmType == ArmType.LeftArm ? 90.0f : 270.0f;
            rotDirection = -rotDirection;
        }

        float rotateDegs = Mathf.Abs(targetRot - transform.rotation.eulerAngles.x);

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
