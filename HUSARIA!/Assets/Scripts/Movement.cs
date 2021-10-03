using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{


    public Rigidbody2D rb;
#pragma warning disable CS0108 // Sk³adowa ukrywa dziedziczon¹ sk³adow¹; brak s³owa kluczowego new
    public GameObject camera;
#pragma warning restore CS0108 // Sk³adowa ukrywa dziedziczon¹ sk³adow¹; brak s³owa kluczowego new
    private Cinemachine.CinemachineVirtualCamera cinemachine;

    private Animator animator;

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

    private float maxSpeedOnStart;

    private float horizontal;
    private bool changingDirection => (rb.velocity.x > 0f && horizontal < 0f) || (rb.velocity.x < 0f && horizontal > 0f);
    private bool facingRight = true;

    [Header("Layer Mask")]
    [SerializeField] private LayerMask groundLayer;

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
    private bool canJump => Input.GetButtonDown("Jump") && onGround;

    [Header("Ground Collision Variables")]
    [SerializeField] private float groundRaycastLength;
    [SerializeField] private Vector3 groundRaycastOffset;
    private bool onGround;


    private float startMovmentAcceleration;
    private float startMaxSpeed;
    private float startGroundedlinearDrag;
    private float startJumpForce;
    private float startAirLinearDrag;
    private float startFallMultiplier;


    public float startCinemachineOrthographicSize = 5f;
    public float endCinemachineOrthographicSize = 12f;
    // Start is called before the first frame update
    void Start()
    {
        cinemachine = camera.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        rb = GetComponent<Rigidbody2D>();
        startMovmentAcceleration = movmentAcceleration;
        startMaxSpeed = maxSpeed;
        startGroundedlinearDrag = groundedlinearDrag;
        startJumpForce = jumpForce;
        startAirLinearDrag = airLinearDrag;
        startFallMultiplier = fallMultiplier;
        animator = GetComponent<Animator>();
        
}

    

    // Update is called once per frame
    void Update()
    {
        horizontal = GetInput().x;
        if (canJump) Jump();
        SetCamera();
        //Animation
        animator.SetBool("isGrounded", onGround);
        animator.SetFloat("velocity", Mathf.Abs(rb.velocity.x));

        if(rb.velocity.x < 0f && facingRight)
        {
            Flip();
        }
        else if(rb.velocity.x > 0f && !facingRight)
        {
            Flip();
        }
        if(rb.velocity.y < 0)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
        
    }

    private void FixedUpdate()
    {
        CheckCollisions();
        MoveCharacter();
        

        if (onGround)
        {
            ApplyGroundedLinearDrag();
            //Animation
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
        else
        {
            ApplyAirLinearDrag();
            Fall();
        }
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    private void MoveCharacter()
    {
        rb.AddForce(new Vector2(horizontal, 0f) * movmentAcceleration);

        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
      //  Debug.Log(rb.velocity);
    }
    private void ApplyGroundedLinearDrag()
    {
        if (Mathf.Abs(horizontal) < 0.4f || changingDirection)
        {
            rb.drag = groundedlinearDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }
    private void ApplyAirLinearDrag()
    {

            rb.drag = airLinearDrag;

    }
    private void Jump() 
    {
        //rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //animation
        animator.SetBool("isJumping", true);
        animator.SetBool("isFalling", false);
       
    }
    private void CheckCollisions()
    {
        onGround = Physics2D.Raycast(transform.position + groundRaycastOffset, Vector2.down, groundRaycastLength, groundLayer) ||
                                Physics2D.Raycast(transform.position - groundRaycastOffset, Vector2.down, groundRaycastLength, groundLayer)||
                                Physics2D.Raycast(transform.position, Vector2.down, groundRaycastLength, groundLayer);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + groundRaycastOffset, transform.position + groundRaycastOffset + Vector3.down * groundRaycastLength);
        Gizmos.DrawLine(transform.position - groundRaycastOffset, transform.position - groundRaycastOffset + Vector3.down * groundRaycastLength);
        Gizmos.DrawLine(transform.position , transform.position  + Vector3.down * groundRaycastLength);
    }
    private void Fall()
    {
        if(rb.velocity.y < 0)
        {
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y >0 && !Input.GetButton("Jump"))
        {
            rb.gravityScale = lowJumpFallMultiplier;
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }
    private void SetCamera()
    {
        
        float lastset = cinemachine.m_Lens.OrthographicSize;
        cinemachine.m_Lens.OrthographicSize = Mathf.Lerp(lastset, startCinemachineOrthographicSize + Mathf.Abs((endCinemachineOrthographicSize-startCinemachineOrthographicSize) *rb.velocity.x/startMaxSpeed), Time.deltaTime * 2);
        
    }
    public void ResetValues()
    {
        movmentAcceleration= startMovmentAcceleration;
        maxSpeed = startMaxSpeed;
        groundedlinearDrag = startGroundedlinearDrag;
        jumpForce = startJumpForce;
        airLinearDrag = startAirLinearDrag;
        fallMultiplier = startFallMultiplier;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

}
