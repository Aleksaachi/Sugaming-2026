using UnityEngine;

[CreateAssetMenu(fileName = "HomingProjEffect", menuName = "Effects/HomingProjEffect")]
public class HomingProjEffect : ItemEffect
{
    [SerializeField] private GameObject projectilePrefab;

    public override void Activate()
    {
        if (PlayerItemHolder.instance == null)
            return;

        Vector3 spawnPos = PlayerItemHolder.instance.transform.position;
        GameObject bullet = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Projectile projScript = bullet.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.isHoming = true;
            projScript.isPlayerProjectile = true;
        }
    }
}