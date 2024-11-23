using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 p1; 
    public Vector3 p2; 
    public Vector3 p3; 

    public GameObject victim;
    public int damage;

private Vector3 lastPosition;
    float t; 

    public float speed = 1f; 

void Start()
{
    lastPosition = transform.position;
}
    void Update()
    {
        
        t += speed * Time.deltaTime;
        t = Mathf.Clamp01(t);

        transform.position = CalculateQuadraticBezierPoint(t, p1, p2, p3);

        RotateTowardsMovementDirection();

        if (transform.position == p3)
        {
            Damage();
        }
    }

    Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = uu * p1; // (1-t)^2 * p1
        point += 2 * u * t * p2; // 2(1-t)t * p2
        point += tt * p3; // t^2 * p3

        return point;
    }

    void Damage()
    {
        Health health;
        if (victim != null)
        {
            health = victim.GetComponent<Health>();
            {
                health.health -= damage;
            }
        } 
        Destroy(gameObject);
    }

    void RotateTowardsMovementDirection()
    {
        Vector3 movementDirection = transform.position - lastPosition;

        if (movementDirection.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        lastPosition = transform.position;
    }
}
