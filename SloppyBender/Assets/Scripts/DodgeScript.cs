using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class DodgeScript : MonoBehaviour
{
    [Header("Refrences")]
    private PlayerMovement pm;
    public Transform orientation;
    private Rigidbody rb;
   

    [Header("Dodgeing")]
    public float dodgeForce;
    public float dodgeDuration;

    [Header("Cooldown")]
    public float dodgeCd;
    private float dodgeCdTimer;

    [Header("Inputs")]
    public KeyCode dodgeKeyK = KeyCode.C;
    private KeyCode dodgeKeyC = KeyCode.JoystickButton1;

    private void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if(Input.GetKey(dodgeKeyK) || Input.GetKey(dodgeKeyC))
        {
            
            Dodge();
        }
        if (dodgeCdTimer > 0)
        {
            dodgeCdTimer-= Time.deltaTime;
        }
    }

    void Dodge()
    {
        if(dodgeCdTimer> 0)
        {
            return;
        }
        else
        {
            dodgeCdTimer = dodgeCd;
        }
        
        pm.dodgeing= true;
        Vector3 direction = GetDirection();
        Vector3 forceToApply = direction * dodgeForce;

       delayedForceToApply= forceToApply;
        Invoke(nameof(DelayedDodge), 0.025f);

        Invoke(nameof(ResetDodge), dodgeDuration);
    }
    private Vector3 delayedForceToApply;
    void DelayedDodge()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    void ResetDodge()
    {
        
        pm.dodgeing = false;
    }

    private Vector3 GetDirection()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();
        direction = orientation.forward*verticalInput+orientation.right*horizontalInput;

        if(verticalInput == 0 && horizontalInput == 0)
        {
            direction = orientation.forward;
        }
        return direction.normalized;
    }
}
