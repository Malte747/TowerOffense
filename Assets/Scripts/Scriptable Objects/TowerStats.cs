using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "TowerStats")]
public class TowerStats : ScriptableObject
{
    [Header("Base Stats")]
    [Tooltip("Health Amount of this Tower")]
    public int health;
    [Tooltip("Goldcost of this Tower")]
    public int goldCost;
    [Tooltip("Supplycost of this Tower")]
    public int supplyCost;
    [Tooltip("Amount of Tiles in the X axis")]
    public int xSize;
    [Tooltip("Amount of Tiles in the Z axis")]
    public int zSize;

    [Header("Mine Stats")]
    [Tooltip("Amount of Gold this Mine adds to the Gold gained each round")]
    public int goldProduced;

    [Header("Animation Infos")]
    [Tooltip("Select Animations for the Tower.")]
    public string animationTriggerString;


public enum Targets
    {
        IndicatorTower,
        Closest,
        First,
        LowHP,
        BiggestGroup,
        MainTower
    }
    [Header("Attack Stats")]
    [Tooltip("Select which units this tower will attack. It will try to avoid the others.")]
    public Targets target = Targets.Closest;
    [Tooltip("Unit sees and can Attack all towers within x tiles")]
    public int attackRange;
    [Tooltip("Damage per attack. Units targeting everything always do base damage")]
    public int damage;
    [Tooltip("Time in seconds between attacks")]
    public float attackCooldown;
    [Tooltip("Unit does damage x seconds into the attack animation")]
    public float damageDelay;

    [Header("Projectile")]
    [Tooltip("Does this Tower Shoot something")]
    public bool isRangeUnit;
    [Tooltip("Assign a projectile here")]
    public GameObject projectile;
    [Tooltip("Assign the projectile spawn position here")]
    public Vector3 projectileStartPos;
    [Tooltip("Assign the correction Amount: 1 high Curve - 5 low curve / 0 = Arrow flies straight")]
    public float projectileCorrection;
    [Tooltip("Speed of the projectile")]
    public int speed;
    [Tooltip("Does the Projectile do Aoe Damage")]
    public bool aoe;
    [Tooltip("Does the Projectile crerate a Zone of Aoe Damage")]
    public bool lingeringAoe;
    [Tooltip("Size of the Aoe Damage")]
    public int aoeSize;
}