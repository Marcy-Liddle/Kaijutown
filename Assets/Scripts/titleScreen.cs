using UnityEngine;
using UnityEngine.SceneManagement;

public class titleScreen : MonoBehaviour
{

    //changes scene when UI button is clicked
    public void startButton ()
    {
        SceneManager.LoadScene("GameScene");
    }
}
