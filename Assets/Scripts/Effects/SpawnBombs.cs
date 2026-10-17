using UnityEngine;

[CreateAssetMenu(fileName = "SpawnBombs", menuName = "Effects/SpawnBombs")]
public class SpawnBombs : ItemEffect
{
    public Vector2Int bombAmountRange;
    public GameObject bombPrefab;
    int bombsPerTrigger;
    
    public override void Initialize()
    {
        bombsPerTrigger = Random.Range(bombAmountRange.x, bombAmountRange.y);
    }
    public override void Activate()
    {
        for (int i = 0; i < bombsPerTrigger; i++)
        {
            float x = Random.Range(-5, 5) + 0.5f;
            float y = Random.Range(-5, 5) + 0.5f;
            if (GridManager.EmptyAt(new Vector2(x, y))) Instantiate(bombPrefab, new Vector3(x, y, 0), Quaternion.identity);
            else i--;
        }
    }
}
