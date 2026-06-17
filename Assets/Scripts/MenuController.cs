using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private string gameSceneName = "Main";

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
