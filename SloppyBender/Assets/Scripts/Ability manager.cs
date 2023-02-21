using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] AbilityState Ability;
    private Rigidbody rb;
    [SerializeField] float JumpForce;
    [SerializeField] Transform orientation;
    private float JumpMissDirection;
    private float JumpMissDirectionAmount;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public enum AbilityState
    {
        
        Speed,
        IceFingers,
        FireFingers,
        WaterFingers,
        Strength

    }

    void StateHandler()
    {
        Ability = AbilityState.Strength;
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.Space) && Ability == AbilityState.Strength) 
        {
            Jump();
            
        }
    }

    void Jump()
    {
        JumpMissDirection = Random.Range(1, 3);
        JumpMissDirectionAmount= Random.Range(-111, 112);
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
        if (JumpMissDirection == 2) 
        {
            rb.AddForce(orientation.forward * JumpMissDirection* 100, ForceMode.VelocityChange);
        }
        else
        {
            rb.AddForce(orientation.right * JumpMissDirection* 100, ForceMode.VelocityChange);
        }
    }
}
