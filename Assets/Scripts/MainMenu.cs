using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
