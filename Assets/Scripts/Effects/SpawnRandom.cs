using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnRandom", menuName = "Effects/SpawnRandom")]
public class SpawnRandom : ItemEffect
{
    public float pureRandomChance;
    public float chanceToSpawn;
    public List<GameObject> objects;
    bool pureRandom;
    GameObject objectToSpawn;
    public override void Initialize()
    {
        if (Random.value > pureRandomChance) pureRandom = true;
        else
        {
            objectToSpawn = objects[Random.Range(0, objects.Count)];
        }
    }
    public override void Activate()
    {
        if (Random.value >= chanceToSpawn) return;
        
        Vector3 target, triggerPosition = PlayerItemHolder.instance.triggerLocation;

        do{
            target = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0);
        }while (!GridManager.EmptyAt(triggerPosition + target) && !(target.x == target.y && target.x == 0));

        if (pureRandom) objectToSpawn = objects[Random.Range(0, objects.Count)];

        Instantiate(objectToSpawn, triggerPosition + target, Quaternion.identity);
    }
}
