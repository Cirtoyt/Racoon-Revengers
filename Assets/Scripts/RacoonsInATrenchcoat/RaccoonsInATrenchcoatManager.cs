using UnityEngine;

public class RaccoonsInATrenchcoatManager : MonoBehaviour
{
    static public RaccoonsInATrenchcoatManager Instance => instance;
    static private RaccoonsInATrenchcoatManager instance;

    [Header("Statics")]
    [SerializeField] private LegRaccoon _leftLegRaccoon;
    [SerializeField] private LegRaccoon _rightLegRaccoon;
    [SerializeField] private ArmRaccoon _leftArmRaccoon;
    [SerializeField] private ArmRaccoon _rightArmRaccoon;
    [SerializeField] private HeadRaccoon _headRaccoon;
    [Header("Properties")]
    [SerializeField] private float _stepDelay = 0.5f;

    public LegRaccoon LeftLegRaccoon => _leftLegRaccoon;
    public LegRaccoon RightLegRaccoon => _rightLegRaccoon;
    public ArmRaccoon LeftArmRaccoon => _leftArmRaccoon;
    public ArmRaccoon RightArmRaccoon => _rightArmRaccoon;
    public HeadRaccoon HeadRaccoon => _headRaccoon;

    private bool stepDelayActive = false;
    private float stepDelayTimer = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (stepDelayActive)
        {
            stepDelayTimer += Time.deltaTime;
            if (stepDelayTimer >= _stepDelay)
            {
                // Reset
                stepDelayTimer = 0;
                stepDelayActive = false;
            }
        }
    }

    public void BeginStepDelay()
    {
        stepDelayActive = true;
    }

    public bool IsStepDelayUp()
    {
        return !stepDelayActive;
    }
}
