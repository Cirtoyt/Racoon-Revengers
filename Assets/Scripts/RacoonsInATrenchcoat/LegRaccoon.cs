using UnityEngine;
using UnityEngine.Rendering;

public class LegRaccoon : MonoBehaviour
{
    public enum StepDirection
    {
        Forward,
        Left,
        Right,
    }

    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private Handedness _whichSide;
    [SerializeField] private float _stepSize = 1;
    [SerializeField] private float _stepSpeed = 1;
    [SerializeField] private LayerMask _stepCollisionCheckLayerMask;
    [SerializeField] private float _stepCollisionCheckRadiusOffset = -0.02f;
    [SerializeField] private bool _debugLeg = false;

    static StepDirection TargetStepDirection;
    static bool AwaitingFollowUpStep = false;

    public Vector3 TargetSteppedPosition => targetSteppedPosition;
    public Quaternion TargetSteppedRotation => targetSteppedRotation;

    private float initialDistanceToOtherLeg;
    private bool justDidLeadStep = false;
    private Vector3 targetSteppedPosition;
    private Quaternion targetSteppedRotation;

    private void Awake()
    {
        targetSteppedPosition = transform.position;
    }

    #region Inputs
    private void OnLeftLegRaccoonAction1()
    {
        if (_whichSide != Handedness.Left)
            return;

        TryStep(StepDirection.Left);
    }

    private void OnLeftLegRaccoonAction2()
    {
        if (_whichSide != Handedness.Left)
            return;

        TryStep(StepDirection.Forward);
    }

    private void OnRightLegRaccoonAction1()
    {
        if (_whichSide != Handedness.Right)
            return;

        TryStep(StepDirection.Forward);
    }

    private void OnRightLegRaccoonAction2()
    {
        if (_whichSide != Handedness.Right)
            return;

        TryStep(StepDirection.Right);
    }
    #endregion

    private void Start()
    {
        if (_whichSide == Handedness.Left)
        {
            initialDistanceToOtherLeg = Vector3.Distance(transform.position, RaccoonsInATrenchcoatManager.Instance.RightLegRaccoon.transform.position);
        }
        else // Right
        {
            initialDistanceToOtherLeg = Vector3.Distance(transform.position, RaccoonsInATrenchcoatManager.Instance.LeftLegRaccoon.transform.position);
        }
    }

    private void TryStep(StepDirection sourceDirection)
    {
        if (!RaccoonsInATrenchcoatManager.Instance.IsStepDelayUp())
            return;

        if (!AwaitingFollowUpStep)
        {
            // Step in direction
            SetStepTargets(sourceDirection);

            if (CanStepTowardsStepTargets())
            {
                TargetStepDirection = sourceDirection;
                AwaitingFollowUpStep = true;
                justDidLeadStep = true;
            }
            else
            {
                // Undo step targets being set
                targetSteppedPosition = transform.position;
                targetSteppedRotation = transform.rotation;
            }
        }
        else if (AwaitingFollowUpStep && !justDidLeadStep)
        {
            // Do follow-up step
            if (TargetStepDirection == StepDirection.Forward && sourceDirection == StepDirection.Forward)
            {
                //Debug.Log($"Correctly following with other leg stepping to align with leading leg");
                // Step in same direction as target step direction
                FollowUpStep();
            }
            else if ((TargetStepDirection == StepDirection.Left || TargetStepDirection == StepDirection.Right)
                     && (sourceDirection == StepDirection.Left || sourceDirection == StepDirection.Right))
            {
                //Debug.Log($"Correctly following with other leg stepping to align with leading leg");
                // Step in same direction as target step direction
                FollowUpStep();
            }
            else // Left/Right
            {
                Debug.Log($"Stepped in wrong direction: {sourceDirection} compared to desired direction: {TargetStepDirection}");
                // Trigger slip due to uncoordination?
                // Make the estate agent sus if she's looking
                // Then step correctly still in target direction
                FollowUpStep();
            }

            // Notify other leg we did a follow-up step
            if (_whichSide == Handedness.Left)
            {
                RaccoonsInATrenchcoatManager.Instance.RightLegRaccoon.NotifyOtherLegStartedFollowUpStep();
            }
            else // Right
            {
                RaccoonsInATrenchcoatManager.Instance.LeftLegRaccoon.NotifyOtherLegStartedFollowUpStep();
            }

            AwaitingFollowUpStep = false;
        }
    }

    private void SetStepTargets(StepDirection direction)
    {
        RaccoonsInATrenchcoatManager.Instance.HeadRaccoon.GetCurrentLookDirection(out Vector3 localHeadForwardDirection,
                                                                                  out Quaternion localHeadRotation);

        Vector3 worldHeadDirection = RaccoonsInATrenchcoatManager.Instance.HeadRaccoon.transform.rotation * localHeadForwardDirection;
        // Flatten head direction
        worldHeadDirection.y = 0;
        worldHeadDirection.Normalize();

        // Set leg rotation target to face stepping direction based on head facing direction
        targetSteppedRotation = Quaternion.LookRotation(worldHeadDirection, Vector3.up);

        // Temp yoink transform for easy forward/right calculations
        Quaternion currentRotation = transform.rotation;
        transform.rotation = targetSteppedRotation;
        Vector3 stepDirection = Vector3.zero;
        switch (direction)
        {
            case StepDirection.Forward:
                stepDirection = transform.forward;
                break;
            case StepDirection.Left:
                stepDirection = -transform.right;
                break;
            case StepDirection.Right:
                stepDirection = transform.right;
                break;
        }

        // Flatten target step core direction
        stepDirection.y = 0;
        stepDirection.Normalize();

        // Set leg position target in direction of step by step size
        targetSteppedPosition += stepDirection * _stepSize;

        // Reassign current rotation to transform
        transform.rotation = currentRotation;

        RaccoonsInATrenchcoatManager.Instance.BeginStepDelay();

        //Debug.Log($"Stepped in direction: {stepDirection}");
    }

    private bool CanStepTowardsStepTargets()
    {
        bool isHittingSomething = Physics.CheckCapsule(transform.position + (Vector3.up * _capsuleCollider.radius),
                                                       targetSteppedPosition + (Vector3.up * _capsuleCollider.radius),
                                                       _capsuleCollider.radius + _stepCollisionCheckRadiusOffset,
                                                       _stepCollisionCheckLayerMask,
                                                       QueryTriggerInteraction.Ignore);

        if (isHittingSomething)
            Debug.Log("Cannot step as something is in the way!");

        return !isHittingSomething;
    }

    private void FollowUpStep()
    {
        if (_whichSide == Handedness.Left)
        {
            targetSteppedPosition = RaccoonsInATrenchcoatManager.Instance.RightLegRaccoon.TargetSteppedPosition
                                    + -RaccoonsInATrenchcoatManager.Instance.RightLegRaccoon.transform.right * initialDistanceToOtherLeg;

            targetSteppedRotation = RaccoonsInATrenchcoatManager.Instance.RightLegRaccoon.TargetSteppedRotation;
        }
        else // Right
        {
            targetSteppedPosition = RaccoonsInATrenchcoatManager.Instance.LeftLegRaccoon.TargetSteppedPosition
                                    + RaccoonsInATrenchcoatManager.Instance.LeftLegRaccoon.transform.right * initialDistanceToOtherLeg;

            targetSteppedRotation = RaccoonsInATrenchcoatManager.Instance.LeftLegRaccoon.TargetSteppedRotation;
        }

        RaccoonsInATrenchcoatManager.Instance.BeginStepDelay();
    }

    private void FixedUpdate()
    {
        if (!_debugLeg)
        {
            // Move foot to target stepped position
            transform.position = Vector3.Lerp(transform.position, targetSteppedPosition, _stepSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetSteppedRotation, _stepSpeed * Time.deltaTime);
        }
    }

    public void NotifyOtherLegStartedFollowUpStep()
    {
        justDidLeadStep = false;
    }
}
