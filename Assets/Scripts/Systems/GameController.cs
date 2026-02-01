using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public FadeOut fadeOut;

    public float fadeOutDuration;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            fadeOut.StartFade(fadeOutDuration);
        }
    }

}
