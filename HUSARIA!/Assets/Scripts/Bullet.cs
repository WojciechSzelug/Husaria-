using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D rb;
    public float liveTime;

    private void Start()
    {
        Destroy(gameObject, liveTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
            collision.collider.GetComponent<PlayerStats>().dmg(10);

            Destroy(gameObject);

    }
}
