using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public static Room currentRoom;
    public Transform spawnPoint;
    public Transform door;
    public Transform pickupSpot;
    public List<GameObject> rewards;
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
        if (enemies.Count == 0) cleared = true;
    }
    void Update()
    {
        if (!cleared && enemies.Count == 0)
        {
            Instantiate(rewards[Random.Range(0, rewards.Count)], pickupSpot.position, Quaternion.identity);
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
