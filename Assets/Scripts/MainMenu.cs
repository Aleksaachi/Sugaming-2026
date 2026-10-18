using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    float timer;
    public bool changingScene;
    public void PlayButton()
    {
        timer = 0.5f;
        DarkScreen.instance.Cover(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
        changingScene = true;
        
    }
    void Update()
    {
        if (!changingScene) return;

        
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            changingScene = false;
            SceneManager.LoadSceneAsync("SampleScene");
        }
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
