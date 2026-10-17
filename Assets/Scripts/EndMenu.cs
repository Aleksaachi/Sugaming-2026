using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public void MenuButton()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
