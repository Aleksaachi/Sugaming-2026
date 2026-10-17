using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public static Room currentRoom;
    public Transform spawnPoint;
    public Transform door;
    public List<GameObject> enemies;
    bool cleared;
    void Awake()
    {
        currentRoom = this;
    }
    void Start()
    {
        RoomManager.instance.changingScenes = false;
        PlayerItemHolder.instance.transform.position = spawnPoint.position;
    }
    void Update()
    {
        if (!cleared && enemies.Count == 0)
        {
            cleared = true;
        }
        else if (cleared)
        {
            if (Vector3.Distance(door.transform.position, PlayerItemHolder.instance.transform.position) <= 1.5f)
            {
                RoomManager.instance.GoToNextRoom();
            }
        }
    }
}
