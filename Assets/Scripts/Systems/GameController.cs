using UnityEngine;

public class GameController : MonoBehaviour
{
    public FadeOut fadeOut;

    private void Start()
    {
        fadeOut.StartFade(10.0f);
    }

}
