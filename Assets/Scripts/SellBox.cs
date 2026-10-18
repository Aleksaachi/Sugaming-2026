using UnityEngine;

public class SellBox : MonoBehaviour
{
    public float interactRange;
    public ParticleSystem particles;
    public SquashAndStretch squashAndStretch;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) 
        && Vector3.Distance(transform.position, PlayerItemHolder.instance.transform.position) <= interactRange
        && PlayerItemHolder.instance.items.Count > 0)
        {
            PlayerItemHolder.RemoveLastItem();
            PlayerItemHolder.instance.AddCoins(5);
            particles.Play();
            squashAndStretch.Play();
        }
    }
}
