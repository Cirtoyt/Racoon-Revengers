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
    public float rotateSpeed = 250.0f;
    private InputAction action1;
    private Quaternion startRot;

    private void Start()
    {
        startRot = transform.rotation;
        action1 = InputSystem.actions.FindAction("LeftArmRaccoonAction1");

    }

    private void Update()
    {
        MoveArmsXAxis();
    }

    private void MoveArmsXAxis()
    {

        Vector3 targetRotEuler = new Vector3(0.0f, 0.0f, ArmType == ArmType.RightArm ? 90.0f : 270.0f);
        Quaternion targetRot = Quaternion.Euler(targetRotEuler);

        if (action1.IsPressed() == true)
        {
            PivotPoint.rotation = Quaternion.RotateTowards(PivotPoint.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }
        else
        {
            PivotPoint.rotation = Quaternion.RotateTowards(PivotPoint.rotation, startRot, rotateSpeed * Time.deltaTime);
        }

    }
}
