using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 p1; 
    public Vector3 p2; 
    public Vector3 p3; 

    public GameObject victim;
    public int damage;

    private AudioManager generalSFX; // Referenz zum AudioManager Script
    [SerializeField] private int soundNumber1, soundNumber2, impactSoundNumber1, impactSoundNumber2; //Tabellennummer für SFX

    [HideInInspector] public GameObject gotShotBy;

private Vector3 lastPosition;
    float t; 

    public float speed = 1f; 

void Start()
{
    lastPosition = transform.position;

        generalSFX = GameObject.Find("AudioManager").GetComponent<AudioManager>(); //Nimmt den Audio Manager in das Script
        //An der stelle, an welcher SFX ausgelöst werden sollen platzieren
        generalSFX.PlayGeneralSound(Random.Range(soundNumber1, soundNumber2)); //Spielt die gewünschte SFX Nummer
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
        HealthTowers health;
        if (victim != null)
        {
            health = victim.GetComponent<HealthTowers>();
            {
                health.health -= damage;
                if (victim.CompareTag("MainTower") && !health.attackedMainTower.Contains(gotShotBy)) health.attackedMainTower.Add(gotShotBy);
            }
        }
        //Spielt Impact Sound
        generalSFX.PlayGeneralSound(Random.Range(impactSoundNumber1, impactSoundNumber2));
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
