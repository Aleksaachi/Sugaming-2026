using UnityEngine;

[CreateAssetMenu(fileName = "TeleportPlayer", menuName = "Effects/TeleportPlayer")]
public class TeleportPlayer : ItemEffect
{
    public int tileRange;
    public GameObject teleportParticles;
    public override void Activate()
    {
        
        Vector3 target, playerPosition = PlayerItemHolder.instance.transform.position;

        GameObject part1 = Instantiate(teleportParticles, playerPosition, Quaternion.identity);
        Destroy(part1, 3f);
        do{
            target = new Vector3(Random.Range(-tileRange, tileRange + 1), Random.Range(-tileRange, tileRange + 1), 0);
        }while (!GridManager.EmptyAt(playerPosition + target));
        PlayerItemHolder.instance.transform.position = playerPosition + target;

        GameObject part2 = Instantiate(teleportParticles, playerPosition + target, Quaternion.identity);
        Destroy(part2, 3f);
    }
}
