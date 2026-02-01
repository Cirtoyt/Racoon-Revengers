using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void StartFade(float duration)
    {
        StartCoroutine(RunFade(duration));
    }

    private IEnumerator RunFade(float duration)
    {
        float time = 0;
        while(time < duration)
        {
            time += Time.deltaTime;
            Color color = image.color;
            color.a = Mathf.Lerp(0, 1, time / duration);
            image.color = color;

            yield return 0;
        }
    }
}
