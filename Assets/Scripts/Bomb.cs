using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float destroyAfter;
    public GameObject explosion;
    float timer;
    void Start()
    {
        timer = destroyAfter;
        timer += Random.Range(-0.5f, 0.5f);
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GameObject expl = Instantiate(explosion, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            Destroy(expl, 0.74f);
            Destroy(gameObject);
        }
    }
}
