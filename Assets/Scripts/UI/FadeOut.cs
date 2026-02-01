using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public static FadeOut Instance;
    private Image image;

    private bool fading = false;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void StartFade(float duration)
    {
        StartCoroutine(RunFade(duration, true));
    }

    public void EndFade(float duration)
    {
        StartCoroutine(RunFade(duration, false));
    }

    private IEnumerator RunFade(float duration, bool fadeOut)
    {
        if(fading)
        {
            yield break;
        }
        fading = true;
        float time = 0;
        while(time < duration)
        {
            time += Time.deltaTime;
            Color color = image.color;
            float a = Mathf.Lerp(0, 1, time / duration);
            color.a = fadeOut ? a : 1 - a;
            image.color = color;

            yield return 0;
        }
        fading = false;
        if (fadeOut)
        { 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            EndFade(duration);
        }

    }
}
