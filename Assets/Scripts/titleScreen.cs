using UnityEngine;
using UnityEngine.SceneManagement;

public class titleScreen : MonoBehaviour
{
    public void startButton ()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void quitButton()
    {
        Application.Quit();

    }
}
