using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMove : MonoBehaviour
{
    Movement movement;
    Movement movementPlayer;

    [Header("Movment Variables")]

    [SerializeField] private float movmentAcceleration;
    public float MovmentAcceleration   // property
    {
        get { return movmentAcceleration; }
        set { movmentAcceleration = value; }
    }
    [SerializeField] private float maxSpeed;
    public float MaxSpeed   // property
    {
        get { return maxSpeed; }
        set { maxSpeed = value; }
    }
    [SerializeField] private float groundedlinearDrag;
    public float GroundedlinearDrag   // property
    {
        get { return groundedlinearDrag; }
        set { groundedlinearDrag = value; }
    }

    [Header("Jump Variables")]
    [SerializeField] private float jumpForce;
    public float JumpForce   // property
    {
        get { return jumpForce; }
        set { jumpForce = value; }
    }
    [SerializeField] private float airLinearDrag;
    public float AirLinearDrag   // property
    {
        get { return airLinearDrag; }
        set { airLinearDrag = value; }
    }
    [SerializeField] private float fallMultiplier;
    public float FallMultiplier   // property
    {
        get { return fallMultiplier; }
        set { fallMultiplier = value; }
    }
    [SerializeField] private float lowJumpFallMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            movementPlayer = collision.GetComponentInParent<Movement>();
            ChangeAllPlayer();
            
        }
     
        

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            movementPlayer = collision.GetComponentInParent<Movement>();
            movementPlayer.ResetValues();
            Debug.Log("player exit");
        }

        Debug.Log("colision exit");

    }

    private void ChangeAll()
    {
        movement.MovmentAcceleration = movmentAcceleration;
        movement.MaxSpeed = maxSpeed;
        movement.GroundedlinearDrag = groundedlinearDrag;
        movement.JumpForce = jumpForce;
        movement.AirLinearDrag = airLinearDrag;
        movement.FallMultiplier = fallMultiplier;

    }
    private void ChangeAllPlayer() 
    {
        movementPlayer.MovmentAcceleration = movmentAcceleration;
        movementPlayer.MaxSpeed = maxSpeed;
        movementPlayer.GroundedlinearDrag = groundedlinearDrag;
        movementPlayer.JumpForce = jumpForce;
        movementPlayer.AirLinearDrag = airLinearDrag;
        movementPlayer.FallMultiplier = fallMultiplier;
    }

}
