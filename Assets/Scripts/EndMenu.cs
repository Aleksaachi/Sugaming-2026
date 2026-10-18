using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            MenuButton();
        }
    }
    public void MenuButton()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
