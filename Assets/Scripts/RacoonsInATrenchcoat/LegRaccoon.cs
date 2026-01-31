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

    static StepDirection TargetStepDirection;
    static bool AwaitingFollowUpStep = false;

    private bool justDidLeadStep = false;

    #region Inputs
    private void OnLeftLegRaccoonAction1()
    {
        if (_whichSide != Handedness.Left)
            return;

        if (justDidLeadStep)
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
            Debug.Log($"Stepping in direction: {sourceDirection}");
        }
        else // AwaitingFollowUpStep
        {
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
                Debug.Log($"Correctly following with other leg stepping in direction: {sourceDirection}");
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

            AwaitingFollowUpStep = false;
        }
    }

    private void Step(StepDirection direction)
    {
        switch (direction)
        {
            case StepDirection.Forward:
                // Step forward
                return;
            case StepDirection.Left:
                // Step left
                return;
            case StepDirection.Right:
                // Step right
                return;
        }
    }
}
