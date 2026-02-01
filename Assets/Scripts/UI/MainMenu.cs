using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int _introSceneIndex = 1;
    public void StartGame()
    {
        SceneManager.LoadScene(_introSceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
