using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Refrences")]
    public PlayerMovement pm;


    [Header("Animator")]
    public Animator anim;

    [Header("Weapon")]
    public weaponState weapon;

    [Header("Stats")]
    public float range;
    public float attackSpeed;
    public Transform attackPoint;
    public LayerMask[] whatIsHittable;
    private bool isAttacking = false;


    [Header("Inputs")]
    public KeyCode lightAttack = KeyCode.JoystickButton5;

    [Header("Combo")]
    public float timeBeforeCantCombo;

    public enum weaponState
    {
        greatSword,
        sword,
        wand,
        dagger
    }
    private void Update()
    {
        MyInputs();
    }

    void MyInputs()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking || Input.GetKeyDown(lightAttack))
        {
            pm.attacking = true;
            isAttacking= true;
            anim.SetBool("Attack", true);
            Invoke(nameof(Attack), 0.18f);
            // Skal skiftes ud med et animation OnEvent 
            Invoke(nameof(StopAnim), 0.49f);
            Invoke(nameof(ResetAttack), attackSpeed);
            
        }
    }

    public void Attack()
    {
        Collider[] colliders =  Physics.OverlapSphere(attackPoint.position, range);
        foreach(Collider c in colliders)
        {
            if (c.GetComponent<EnemyManager>())
            {
                print(c.gameObject.name);
            }
        }
        
    }
    void StopAnim()
    {
        anim.SetBool("Attack", false);
        pm.attacking = false;
    }

    void ResetAttack()
    {
        isAttacking= false;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position,range);
    }


}
