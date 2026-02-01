using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    public static GameplayUIManager Instance { get; private set; }

    [SerializeField] private Slider _susSlider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetSuspicionValue(float value)
    {
        _susSlider.value = value;
    }
}
