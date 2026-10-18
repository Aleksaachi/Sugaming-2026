using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class DarkScreen : MonoBehaviour
{
    public static DarkScreen instance;
    Vector3 rightPosition;
    Vector3 centerPosition;
    Vector3 leftPosition;
    Vector3 startPosition;
    Vector3 targetPosition;
    public float animationTime;
    float timer;
    bool active;
    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        SceneManager.activeSceneChanged += Uncover;
    }
    void Start()
    {
        DontDestroyOnLoad(transform.parent.gameObject);
        rightPosition = new Vector3(2200, 0, 0);
        centerPosition = new Vector3(0, 0, 0);
        leftPosition = new Vector3(-2200, 0, 0);
        Uncover(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
        GetComponent<Image>().color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;

        timer -= Time.deltaTime;
        transform.localPosition = Vector3.Slerp(startPosition, targetPosition, 1-timer/animationTime);
        if (timer <= 0)
        {
            active = false;
        }
    }
    public void Uncover(Scene s1, Scene s2)
    {
        transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
        if (ItemBar.instance != null)
        {
            ItemBar.instance.transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
        }

        timer = animationTime;

        active = true;
        transform.localPosition = centerPosition;
        startPosition = centerPosition;
        targetPosition = leftPosition;
    }
    public void Cover(Scene s1, Scene s2)
    {
        timer = animationTime;

        active = true;
        transform.localPosition = rightPosition;
        startPosition = rightPosition;
        targetPosition = centerPosition;
    }
}
