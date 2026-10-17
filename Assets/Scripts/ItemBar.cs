using TMPro;
using UnityEngine;

public class ItemBar : MonoBehaviour
{
    public static ItemBar instance;
    public TextMeshProUGUI nameBox;
    public float moveTime;
    public float stayTime;
    int stage = 0;
    float timer;
    Vector3 upPosition;
    Vector3 downPosition;

    void Awake()
    {
        instance = this;
        downPosition = Vector3.zero;
        upPosition = new Vector3(0, 256, 0);
        transform.localPosition = upPosition;
        stage = -1;
    }
    void Update()
    {
        if (stage == -1) return;

        timer -= Time.deltaTime;
        if (stage == 0){
            float lerp = Mathf.Clamp01(1-timer/moveTime);
            transform.localPosition = Vector3.Lerp(upPosition, downPosition, lerp);
            if (timer < 0)
            {
                timer = stayTime;
                stage = 1;
            }
        }
        else if (stage == 1)
        {
            if (timer < 0)
            {
                timer = stayTime;
                stage = 2;
            }
        }
        else
        {
            float lerp = Mathf.Clamp01(timer/moveTime);
            transform.localPosition = Vector3.Lerp(upPosition, downPosition, lerp);
            if (timer < 0)
            {
                timer = stayTime;
                stage = -1;
            }
        }
    }

    public void UpdateName(string itemName)
    {
        stage = 0;
        timer = moveTime;
        nameBox.text = itemName + "!";
    }

}
