using UnityEngine;

public class RaccoonsInATrenchcoatManager : MonoBehaviour
{
    static public RaccoonsInATrenchcoatManager Instance => instance;
    static private RaccoonsInATrenchcoatManager instance;

    [SerializeField] private LegRaccoon _leftLegRaccoon;
    [SerializeField] private LegRaccoon _rightLegRaccoon;
    [SerializeField] private ArmRaccoon _leftArmRaccoon;
    [SerializeField] private ArmRaccoon _rightArmRaccoon;
    [SerializeField] private HeadRaccoon _headRaccoon;

    public LegRaccoon LeftLegRaccoon => _leftLegRaccoon;
    public LegRaccoon RightLegRaccoon => _rightLegRaccoon;
    public ArmRaccoon LeftArmRaccoon => _leftArmRaccoon;
    public ArmRaccoon RightArmRaccoon => _rightArmRaccoon;
    public HeadRaccoon HeadRaccoon => _headRaccoon;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
