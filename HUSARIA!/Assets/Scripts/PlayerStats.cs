using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Stats")]
    [Range(0f, 100f)]
    [SerializeField] private float maxhealth = 100;
    [SerializeField] private float health = 100;

    Rigidbody2D rb;
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        health = maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void dmg(float dmg)
    {
        if(rb.velocity.x > GetComponent<Movement>().MaxSpeed*60/100)
        {
            Debug.Log("obra¿enia zredukowane do 0");
            return;
        }
        else
        {
            health = health - dmg;
            Debug.Log("zosta³o hp: " + health);
        }
        dead();
    }

    private void dead()
    {
        if (health <= 0f)
        {
            Debug.Log("koniec gry");
        }
    }
}
