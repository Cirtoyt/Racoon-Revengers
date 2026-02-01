using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    [SerializeField] private List<GameObject> _slides = new();
    [SerializeField] private int _gameSceneIndex = 2;

    private int currentSlide = 0;

    public void NextSlide()
    {
        _slides[currentSlide].SetActive(false);
        currentSlide++;

        if (_slides.Count <= currentSlide)
        {
            SceneManager.LoadScene(_gameSceneIndex);
        }
        else
        {
            _slides[currentSlide].SetActive(true);
        }
    }
}
