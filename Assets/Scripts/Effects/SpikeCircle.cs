using UnityEngine;

[CreateAssetMenu(fileName = "SpikeCircle", menuName = "Effects/SpikeCircle")]
public class SpikeCircle : ItemEffect
{
    public Vector2Int amountRange;
    public float distance;
    int amount;
    public GameObject spikePrefab;
    
    public override void Initialize()
    {
        amount = Random.Range(amountRange.x, amountRange.y);
    }
    public override void Activate()
    {
        float radStart = Random.Range(0, 2*Mathf.PI);
        for (int i = 0; i < amount; i++)
        {
            float radians = 2*Mathf.PI/amount * i + radStart;
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);
            Vector3 dir = new Vector3(x, y, 0) * distance;
            Projectile fp = Instantiate(spikePrefab, PlayerItemHolder.instance.triggerLocation + dir, Quaternion.identity).GetComponent<Projectile>();
            fp.SetDirection(dir);
        }
    }
}
