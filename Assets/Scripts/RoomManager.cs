using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    public List<string> combatRooms;
    public List<string> itemRooms;
    public Vector2Int itemRoomRange;
    public bool changingScenes;
    int counter = 0;
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void GoToNextRoom()
    {
        if (changingScenes) return;

        string sceneToLoad;
        if (counter >= itemRoomRange.x)
        {
            int chance = counter == itemRoomRange.y ? 0 : Random.Range(0, itemRoomRange.y - counter);
            if (chance == 0)
            {
                sceneToLoad = itemRooms[Random.Range(0, itemRooms.Count)];
                counter = 0;
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
        
        changingScenes = true;
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
