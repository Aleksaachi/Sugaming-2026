using UnityEngine;

[CreateAssetMenu(fileName = "TeleportPlayer", menuName = "Effects/TeleportPlayer")]
public class TeleportPlayer : ItemEffect
{
    public Vector2 teleportRange;
    float range;
    
    public override void Initialize()
    {
        range = Random.Range(teleportRange.x, teleportRange.y);
    }
    public override void Activate()
    {
        float radius = Random.Range(teleportRange.x, range);
        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 vector = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
        Vector3 playerPosition = PlayerItemHolder.instance.transform.position;
        PlayerItemHolder.instance.transform.position = playerPosition + vector;
    }
}
