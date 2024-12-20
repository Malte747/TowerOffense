using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTower : MonoBehaviour
{
    [HideInInspector] public Vector3 p1;
    [HideInInspector] public Vector3 p2;
    [HideInInspector] public Vector3 p3;

    [HideInInspector] public GameObject victim;

    [HideInInspector] public TowerStats TowerStats;

    private Vector3 lastPosition;
    float t;

    void Start()
    {
        lastPosition = transform.position;
    }
    void Update()
    {
        //Debug.Log("in THe Aor");
        if (victim != null)
        {
            p3 = victim.transform.position;
        }

        t += TowerStats.speed * Time.deltaTime;
        t = Mathf.Clamp01(t);

        transform.position = CalculateQuadraticBezierPoint(t, p1, p2, p3);

        RotateTowardsMovementDirection();

        if (transform.position == p3)
        {
            if(!TowerStats.aoe) Damage(victim);
            else DamageAoe();
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

    void Damage(GameObject target)
    {
        //Debug.Log("Hit");
        if (target != null)
        {
            Health health = target.GetComponent<Health>();
            health.health -= TowerStats.damage;
        }
        if(!TowerStats.aoe) Destroy(gameObject);
    }

    void DamageAoe()
    {
        foreach (Vector3 targetPos in EnemyBibleScript.EnemyBible.Keys)
        {
            if (Vector3.Distance(victim.transform.position, targetPos) <= TowerStats.aoeSize) Damage(EnemyBibleScript.EnemyBible[targetPos]);
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
