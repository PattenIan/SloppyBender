using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Animator")]
   //public Animator anim;
    float animVertical;
    float animHorizontal;
    public TextMeshProUGUI text;

    
    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float GroundDrag;
    private float MoveSpeed;
    bool playerIsMoving;
    public float dodgeSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpMultiplier;
    public float jumpCooldown;
    public bool readyToJump = true;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Attacking")]
    public bool attacking = false;

    [Header("Keybinds Keyboard")]
    public KeyCode jumpKeyK = KeyCode.Space ;
    public KeyCode sprintKeyK = KeyCode.LeftShift;
    public KeyCode crouchKeyK = KeyCode.LeftControl;

    [Header("Keybinds Controller")]
    public KeyCode jumpKeyC = KeyCode.JoystickButton0;
    public KeyCode sprintKeyC = KeyCode.JoystickButton8;
    public KeyCode crouchKeyC = KeyCode.JoystickButton9;

    [Header("Ground Check")]
    public float playerHeight;
    public bool isGrounded;
    public LayerMask whatIsGround;

    public bool dodgeing;
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 movementDir;
    public Rigidbody rb;
    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        air,
        crouching,
        dodgeing
    }
    private void Start()
    {
        rb= GetComponent<Rigidbody>();
        rb.freezeRotation= true;
        startYScale = transform.localScale.y;
       
    }

    private void Update()
    {
        //text.text = "Speed: " + MoveSpeed;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .2f, whatIsGround);

        MyInput();
        speedControl();
        StateHandler();

        if(state == MovementState.walking || state == MovementState.sprinting || state == MovementState.crouching)
        {
            rb.drag = GroundDrag;
        }
        else
        {
            rb.drag = 0f;

        }
        animHorizontal = Mathf.Abs(rb.velocity.y);
      //  anim.SetFloat("Horizontal", animHorizontal);
        animVertical = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z);
       // anim.SetFloat("Vertical", animVertical);
      
    }
    private void FixedUpdate()
    {
        MovePLayer();
        
    }
    void MyInput()
    {
        
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKeyK) && readyToJump && isGrounded && !attacking || Input.GetKey(jumpKeyC) && readyToJump && isGrounded && !attacking)
        {
            Jump();
            readyToJump= false;

            Invoke(nameof(resetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(crouchKeyK) && !attacking || Input.GetKeyDown(crouchKeyC) && !attacking)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 7f, ForceMode.Impulse);
        }

        if(Input.GetKeyUp(crouchKeyK) && !attacking || Input.GetKeyUp(crouchKeyC) && !attacking)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        

    }
    void StateHandler()
    {
        if (dodgeing)
        {
            state = MovementState.dodgeing;
            MoveSpeed = dodgeSpeed;
            
        }
        else if (Input.GetKey(crouchKeyK)||Input.GetKey(crouchKeyC))
        {
            state = MovementState.crouching;
            MoveSpeed = crouchSpeed;
        } else if (isGrounded && Input.GetKey(sprintKeyK) || isGrounded && Input.GetKey(sprintKeyC))
        {
            state = MovementState.sprinting;
           MoveSpeed = sprintSpeed;
        } else if (isGrounded)
        {
            state = MovementState.walking;
           MoveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
            
        }
    }

    void MovePLayer()
    {
        
        movementDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isGrounded && !attacking)
        {
        rb.AddForce(movementDir.normalized * MoveSpeed * 10f, ForceMode.Force);
        }else if(!isGrounded)
        {
            rb.AddForce(movementDir.normalized * MoveSpeed * 10f * jumpMultiplier, ForceMode.Force);
        }
        
        
    }
    void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x,0f,rb.velocity.z);
        if(flatVel.magnitude > MoveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * MoveSpeed;
            rb.velocity= new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void resetJump()
    {
        readyToJump = true;
    }

    

   
}
