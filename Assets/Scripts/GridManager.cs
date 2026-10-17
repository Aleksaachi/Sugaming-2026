using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    public Tilemap map;
    void Awake()
    {
        instance = this;
    }
    public static bool EmptyAt(Vector2 worldPosition)
    {
        if (instance != null && instance.map != null && instance.map.GetTile(instance.map.WorldToCell(worldPosition)) == null)
        {
            return true;
        }
        return false;
    }
}
