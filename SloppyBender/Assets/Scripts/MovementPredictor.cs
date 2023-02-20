using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MovementPredictor : MonoBehaviour
{
    public float maxTimePredict = 1;
    public PlayerMovement Enemy;
    public Vector3 prediction;
    [SerializeField] private float _maxDistancePredict = 100;
    [SerializeField] private float _minDistancePredict = 5;
    public GameObject bullet;
    public Transform firepoint;

    public void Shoot()
    {

       
        

    }

    private void FixedUpdate()
    {

      
        var leadTimePredicted = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(Enemy.transform.position, transform.position));
        predictMovement(leadTimePredicted);
        transform.LookAt(prediction);

    }


    void predictMovement(float leadTimePredicted)
    {
        var predictedTime = Mathf.Lerp(0,maxTimePredict, leadTimePredicted);

        prediction=Enemy.rb.position + Enemy.rb.velocity * predictedTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, prediction);
    }
}
