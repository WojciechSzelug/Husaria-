using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : MonoBehaviour
{
    
    private Rigidbody2D rb;
    public Transform player;

    [Header("Bullet Value")]
    public Transform firePosition;
    public GameObject bulletPrefab;
    private bool playerInRange;
    public float bulletForce = 20f;

    public float fireRate = 10F;
    private float nextFire = 0.0F;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        playerInRange = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -180f;
        rb.rotation = angle;
        if (playerInRange && Time.time > nextFire)
        {

            nextFire = Time.time + fireRate;

            Bullet();
        }
    }
    private void Bullet()
    {
           
           GameObject bullet = Instantiate(bulletPrefab, firePosition.position, firePosition.rotation );
           Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
           rbBullet.AddForce(firePosition.right*-1 * bulletForce, ForceMode2D.Impulse);
        
       
    }
 
}


