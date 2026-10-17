using UnityEngine;

[CreateAssetMenu(fileName = "RandomStatEffect", menuName = "Effects/RandomStatEffect")]
public class RandomStatEffect : ItemEffect
{
    public override void Activate()
    {
        if (PlayerItemHolder.instance != null)
        {
            PlayerController player = PlayerItemHolder.instance.GetComponent<PlayerController>();
            
            if (player != null)
                player.UpgradeRandomStat();
        }
    }
}