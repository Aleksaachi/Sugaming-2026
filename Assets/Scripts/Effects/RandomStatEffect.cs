using UnityEngine;

[CreateAssetMenu(fileName = "RandomStatEffect", menuName = "Effects/RandomStatEffect")]
public class RandomStatEffect : ItemEffect
{
    public GameObject heartParticles;
    public override void Activate()
    {
        if (PlayerItemHolder.instance != null)
        {
            PlayerController player = PlayerItemHolder.instance.GetComponent<PlayerController>();
            GameObject part = Instantiate(heartParticles, player.transform.position, Quaternion.identity);
            Destroy(part, 3f);
            if (player != null)
                player.UpgradeRandomStat();
        }
    }
}