using UnityEngine;

public class Vfx : MonoBehaviour
{
    public static void Instant(GameObject item)
    {
        var go = Instantiate(Resources.Load<GameObject>("vfx"));

        var sprite = item.GetComponent<SpriteRenderer>().sprite;
        go.GetComponent<SpriteRenderer>().sprite = sprite;
        go.transform.position = item.transform.position;

        //var rb = go.AddComponent<Rigidbody2D>();
        //rb.gravityScale = Random.Range(3.0f, 8.0f);

        //rb.angularVelocity = Random.Range(-90.0f, 90.0f);
        //rb.AddForce(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(10.0f, 13.0f)), ForceMode2D.Impulse);

        Destroy(go, 2.5f);
    }
}
