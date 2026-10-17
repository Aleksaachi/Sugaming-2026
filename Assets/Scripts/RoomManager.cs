using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void GoToNextRoom()
    {
        if (changingScenes) return;

        string sceneToLoad;
        if (roomNumber == bossStage)
        {
            sceneToLoad = bossRooms[Random.Range(0, bossRooms.Count)];
        }else if (shopCounter == 0)
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
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
