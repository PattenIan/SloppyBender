using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollOnOff : MonoBehaviour
{
    [SerializeField] CapsuleCollider mainCollider;
    [SerializeField] GameObject rig;
    [SerializeField] Animator animator;

    Collider[] ragDollColliders;
    Rigidbody[] limbsRigidBodies;
    void Start()
    {
        GetRagdollBits();
        RagDollOff();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.J))
        {
            RagDollOff();
        } else if(Input.GetKey(KeyCode.K)) 
        { 
            RagDollOn();
        }
    }

    private void RagDollOn()
    {
        animator.enabled = false;
        mainCollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        foreach (Collider col in ragDollColliders)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rigidbody in limbsRigidBodies)
        {
            rigidbody.isKinematic = false;
        }
    }

    private void RagDollOff()
    {
        foreach(Collider col in ragDollColliders)
        {
            col.enabled = false;
        }
        foreach(Rigidbody rigidbody in limbsRigidBodies)
        {
            rigidbody.isKinematic = true;
        }

        animator.enabled = true;
        mainCollider.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void GetRagdollBits()
    {
        ragDollColliders = rig.GetComponentsInChildren<Collider>();
        limbsRigidBodies = rig.GetComponentsInChildren<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "KnockDown")
        {
            RagDollOn();
        }
    }
}
