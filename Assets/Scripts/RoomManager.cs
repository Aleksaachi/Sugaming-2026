using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    public List<string> combatRooms;
    public List<string> itemRooms;
    public List<string> bossRooms;
    public List<string> shopRooms;
    public Vector2Int itemRoomRange;
    public int bossStage;
    public bool changingScenes;
    int counter = 0;
    int roomNumber = 0;
    int shopCounter = -1;
    float timer;
    bool loading;
    string sceneToLoad;
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void GoToNextRoom()
    {
        if (changingScenes) return;

        if (roomNumber == bossStage + 1)
        {
            sceneToLoad = "Win";
            Destroy(PlayerController.instance.gameObject, 0.6f);
            Destroy(PlayerController.instance.healthBar, 0.6f);
        }
        else if (roomNumber == bossStage)
        {
            sceneToLoad = bossRooms[Random.Range(0, bossRooms.Count)];
        }
        else if (shopCounter == 0)
        {
            sceneToLoad = shopRooms[Random.Range(0, shopRooms.Count)];
        }
        else{
            if (counter >= itemRoomRange.x)
            {
                int chance = counter == itemRoomRange.y ? 0 : Random.Range(0, itemRoomRange.y - counter);
                if (chance == 0)
                {
                    sceneToLoad = itemRooms[Random.Range(0, itemRooms.Count)];
                    counter = 0;
                    shopCounter = 2;
                }
                else
                {
                    sceneToLoad = combatRooms[Random.Range(0, combatRooms.Count)];
                    counter++;
                }
            }
            else
            {
                sceneToLoad = combatRooms[Random.Range(0, combatRooms.Count)];
                counter++;
            }
        }
        roomNumber++;
        shopCounter--;
        changingScenes = true;
        DarkScreen.instance.Cover(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
        timer = 0.5f;
        loading = true;
    }
    void Update()
    {
        if (!loading) return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            loading = false;
            LoadNextScene();
        }
    }
    void LoadNextScene()
    {
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
