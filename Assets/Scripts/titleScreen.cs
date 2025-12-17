using UnityEngine;
using UnityEngine.SceneManagement;

public class titleScreen : MonoBehaviour
{
    public void startButton ()
    {
        SceneManager.LoadScene("GameScene");
    }
}
