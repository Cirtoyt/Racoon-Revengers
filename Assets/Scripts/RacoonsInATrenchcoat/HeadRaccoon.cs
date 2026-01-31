using UnityEngine;
using UnityEngine.InputSystem;
using static LegRaccoon;

public class HeadRaccoon : MonoBehaviour
{
    public enum HeadDirection
    {
        Neutral,
        Left,
        Right,
        Up,
    }

    [SerializeField] private Transform _neckPivot;
    [SerializeField] private float _shakeHeadDegrees = 30;
    [SerializeField] private float _nodHeadDegrees = 20;
    [SerializeField] private float _neutralHeadDegrees = 10;
    [SerializeField] private float _rotateHeadSpeed = 1;

    private InputAction leftAction;
    private InputAction rightAction;
    private bool leftActionHeld = false;
    private bool rightActionHeld = false;

    private void Awake()
    {
        leftAction = InputSystem.actions.FindAction("HeadRaccoonAction1");
        rightAction = InputSystem.actions.FindAction("HeadRaccoonAction2");
    }

    private void OnEnable()
    {
        leftAction.started += OnLeftActionStarted;
        rightAction.started += OnRightActionStarted;
        leftAction.canceled += OnLeftActionCanceled;
        rightAction.canceled += OnRightActionCanceled;
    }

    private void OnDisable()
    {
        leftAction.started -= OnLeftActionStarted;
        rightAction.started -= OnRightActionStarted;
        leftAction.canceled -= OnLeftActionCanceled;
        rightAction.canceled -= OnRightActionCanceled;
    }

    private void OnLeftActionStarted(InputAction.CallbackContext obj)
    {
        leftActionHeld = true;
    }

    private void OnRightActionStarted(InputAction.CallbackContext obj)
    {
        rightActionHeld = true;
    }

    private void OnLeftActionCanceled(InputAction.CallbackContext obj)
    {
        leftActionHeld = false;
    }

    private void OnRightActionCanceled(InputAction.CallbackContext obj)
    {
        rightActionHeld = false;
    }

    public HeadDirection GetTargetHeadDirection()
    {
        if (!leftActionHeld && !rightActionHeld)
        {
            return HeadDirection.Neutral;
        }
        else if (leftActionHeld && !rightActionHeld)
        {
            return HeadDirection.Left;
        }
        else if (rightActionHeld && !leftActionHeld)
        {
            return HeadDirection.Right;
        }
        else// if (leftActionHeld && rightActionHeld)
        {
            return HeadDirection.Up;
        }
    }

    public void GetCurrentLookDirection(out Vector3 localForwardDirection, out Quaternion localRotation)
    {
        localForwardDirection = Vector3.forward;
        localRotation = Quaternion.identity;

        switch (GetTargetHeadDirection())
        {
            case HeadDirection.Neutral:
                localRotation = Quaternion.Euler(_neutralHeadDegrees, 0, 0);
                break;
            case HeadDirection.Left:
                localRotation = Quaternion.Euler(0, -_shakeHeadDegrees, 0);
                break;
            case HeadDirection.Right:
                localRotation = Quaternion.Euler(0, _shakeHeadDegrees, 0);
                break;
            case HeadDirection.Up:
                localRotation = Quaternion.Euler(-_nodHeadDegrees, 0, 0);
                break;
        }

        localForwardDirection = localRotation * localForwardDirection;
    }

    private void FixedUpdate()
    {
        // Smooth move head to facing target direction

        GetCurrentLookDirection(out Vector3 targetLocalForward, out Quaternion targetLocalRotation);

        _neckPivot.localRotation = Quaternion.Slerp(_neckPivot.localRotation, targetLocalRotation, _rotateHeadSpeed * Time.deltaTime);
    }
}