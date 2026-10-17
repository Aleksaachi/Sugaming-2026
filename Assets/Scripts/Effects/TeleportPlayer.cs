using UnityEngine;

[CreateAssetMenu(fileName = "TeleportPlayer", menuName = "Effects/TeleportPlayer")]
public class TeleportPlayer : ItemEffect
{
    public int tileRange;
    public override void Activate()
    {
        Vector3 target, playerPosition = PlayerItemHolder.instance.transform.position;
        do{
            target = new Vector3(Random.Range(-tileRange, tileRange + 1), Random.Range(-tileRange, tileRange + 1), 0);
            Debug.Log(target);
        }while (!GridManager.EmptyAt(playerPosition + target));
        PlayerItemHolder.instance.transform.position = playerPosition + target;
    }
}
