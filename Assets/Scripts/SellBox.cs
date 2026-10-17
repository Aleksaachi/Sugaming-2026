using UnityEngine;

public class SellBox : MonoBehaviour
{
    public float interactRange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) 
        && Vector3.Distance(transform.position, PlayerItemHolder.instance.transform.position) <= interactRange
        && PlayerItemHolder.instance.items.Count > 0)
        {
            PlayerItemHolder.RemoveLastItem();
            PlayerItemHolder.instance.AddCoins(5);
        }
    }
}
