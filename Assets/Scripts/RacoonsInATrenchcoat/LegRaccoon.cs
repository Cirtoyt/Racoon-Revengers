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

    [SerializeField] private Handedness _whichSide;
    [SerializeField] private float _headDirectionInfluenceMultiplier = 1;
    [SerializeField] private float _stepSize = 1;
    [SerializeField] private float _stepSpeed = 1;
    [SerializeField] private bool _debugLeg = false;

    static StepDirection TargetStepDirection;
    static bool AwaitingFollowUpStep = false;

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

    private void TryStep(StepDirection sourceDirection)
    {
        if (!AwaitingFollowUpStep)
        {
            TargetStepDirection = sourceDirection;
            AwaitingFollowUpStep = true;
            justDidLeadStep = true;

            // Step in direction
            Step(TargetStepDirection);
        }
        else if (AwaitingFollowUpStep && !justDidLeadStep)
        {
            // Do follow-up step
            if (TargetStepDirection == StepDirection.Forward
                && sourceDirection == StepDirection.Forward)
            {
                Debug.Log($"Correctly following with other leg stepping in direction: {sourceDirection}");
                // Step in same direction as target step direction
                Step(TargetStepDirection);
            }
            else if ((TargetStepDirection == StepDirection.Left || TargetStepDirection == StepDirection.Right)
                     &&(sourceDirection == StepDirection.Left || sourceDirection == StepDirection.Right))
            {
                Debug.Log($"Correctly following with other leg stepping in direction: {TargetStepDirection}");
                // Step in same direction as target step direction
                Step(TargetStepDirection);
            }
            else // Left/Right
            {
                Debug.Log($"Stepped in wrong direction: {sourceDirection} compared to desired direction: {TargetStepDirection}");
                // Trigger slip due to uncoordination?
                // Make the estate agent sus if she's looking
                // Then step correctly still in target direction
                Step(TargetStepDirection);
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

    private void Step(StepDirection direction)
    {
        RaccoonsInATrenchcoatManager.Instance.HeadRaccoon.GetCurrentLookDirection(out Vector3 localHeadForwardDirection,
                                                                                  out Quaternion localHeadRotation);

        Vector3 stepCoreDirection = Vector3.zero;
        switch (direction)
        {
            case StepDirection.Forward:
                stepCoreDirection = RaccoonsInATrenchcoatManager.Instance.transform.forward;
                break;
            case StepDirection.Left:
                stepCoreDirection = -RaccoonsInATrenchcoatManager.Instance.transform.right;
                // Rotate head facing directioon to align with step left direction
                localHeadForwardDirection = Quaternion.Euler(0, -90, 0) * localHeadForwardDirection;
                break;
            case StepDirection.Right:
                stepCoreDirection = RaccoonsInATrenchcoatManager.Instance.transform.right;
                // Rotate head facing directioon to align with step right direction
                localHeadForwardDirection = Quaternion.Euler(0, 90, 0) * localHeadForwardDirection;
                break;
        }

        // Flatten target step core direction
        stepCoreDirection.y = 0;
        stepCoreDirection.Normalize();

        Vector3 worldHeadDirection = RaccoonsInATrenchcoatManager.Instance.HeadRaccoon.transform.TransformDirection(localHeadForwardDirection);
        // Flatten head direction
        worldHeadDirection.y = 0;
        worldHeadDirection.Normalize();

        // Add head influence to core step direction
        Vector3 stepDirection = stepCoreDirection + (worldHeadDirection * _headDirectionInfluenceMultiplier);
        stepDirection.Normalize();

        targetSteppedPosition += stepDirection * _stepSize;
        targetSteppedRotation = Quaternion.LookRotation(stepDirection, Vector3.up);

        Debug.Log($"Stepped in direction: {stepDirection}");
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
