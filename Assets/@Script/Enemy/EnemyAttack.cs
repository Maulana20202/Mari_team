using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum AttackType
{
    Melee,
    Shooting
}
public class EnemyAttack : MonoBehaviour
{

     public AttackType attackType;
    private IAttack attackStrategy;

    [Header ("Projectile")]
    public GameObject Projectile;
    public float AmmoSpeed;
    public float timeLimit;
    public Transform ShootingLocation;

    [Header("Attack Area")]
    public float coneAngle = 45f; // Lebar sudut cone
    public float attackRange = 5f; // Jarak maksimal serangan

    [Header("Damage Stats")]
    public float Damage;

    [Header ("Attack Timing")]
    public float AttackTime;
    private float AttackTimeCurrent;
    public float AttackDelay;
    private float AttackDelayCurrent;

    void Start(){
        AttackTimeCurrent = AttackTime;

        SetAttackStrategy();
    }

    void Update(){
        CheckConeAttack();
    }

    void CheckConeAttack()
    {
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        foreach (var hit in hitColliders)
        {

              if (hit.gameObject == gameObject) continue;

            Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            if (angle < coneAngle / 2)
            {
                if(hit.gameObject.CompareTag("Player")){
                    if(AttackDelayCurrent <= 0){
                        if(AttackTimeCurrent <= 0){
                            attackStrategy.Attacking(hit.gameObject);
                            AttackDelayCurrent = AttackDelay;
                            AttackTimeCurrent = AttackTime;
                        } else {
                            AttackTimeCurrent -= Time.deltaTime;
                        }
                    } else {
                        AttackDelayCurrent -= Time.deltaTime;
                    }
                    
                }
                
            }
        }
    }


    private void SetAttackStrategy()
    {
        switch (attackType)
        {
            case AttackType.Melee:
                attackStrategy = new MeleeAttack(Damage);
                break;
            case AttackType.Shooting:
                attackStrategy = new ShootingAttack(Projectile,ShootingLocation,AmmoSpeed,timeLimit,Damage);
                break;
        }
    }




    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        // Posisi awal cone
        Vector3 origin = transform.position;

        // Garis utama (forward player)
        Vector3 forward = transform.forward * attackRange;

        // Hitung tepi kiri dan kanan cone
        Quaternion leftRotation = Quaternion.Euler(0, -coneAngle / 2, 0);
        Quaternion rightRotation = Quaternion.Euler(0, coneAngle / 2, 0);

        Vector3 leftBoundary = leftRotation * forward;
        Vector3 rightBoundary = rightRotation * forward;

        // Gambar garis cone
        Gizmos.DrawRay(origin, forward);
        Gizmos.DrawRay(origin, leftBoundary);
        Gizmos.DrawRay(origin, rightBoundary);

        // Gambar lengkungan sederhana di antara batas kiri dan kanan
        int segments = 20;
        for (int i = 0; i < segments; i++)
        {
            float t1 = (float)i / segments;
            float t2 = (float)(i + 1) / segments;

            Vector3 point1 = Quaternion.Euler(0, Mathf.Lerp(-coneAngle / 2, coneAngle / 2, t1), 0) * forward;
            Vector3 point2 = Quaternion.Euler(0, Mathf.Lerp(-coneAngle / 2, coneAngle / 2, t2), 0) * forward;

            Gizmos.DrawLine(origin + point1, origin + point2);
        }
    }
}

public interface IAttack
{
    public void Attacking(GameObject player);
}

public class ShootingAttack : IAttack
{
    public Transform ShootingLocation;
    public float Damage;
    public GameObject Projectile;

    public float AmmoSpeed;

    public float TimeLimit;

    public ShootingAttack(GameObject projectilePrefab,Transform ShootingLocation,float AmmoSpeed,float timeLimit,float Damage)
    {
        this.Projectile = projectilePrefab;
        this.AmmoSpeed = AmmoSpeed;
        this.TimeLimit = timeLimit;
        this.Damage = Damage;
        this.ShootingLocation = ShootingLocation;
    }

    
    public void Attacking(GameObject player)
    {
        GameObject ProjectileObject = GameObject.Instantiate(Projectile,ShootingLocation.position, ShootingLocation.rotation);

        Projectile projectile = ProjectileObject.GetComponent<Projectile>();

        projectile.AmmoSpeed = AmmoSpeed;
        projectile.Damage = Damage;
        projectile.TimeLimit = TimeLimit; 
    }
}

public class MeleeAttack : IAttack
{

    public float Damage;

    public MeleeAttack(float Damage)
    {

        this.Damage = Damage;
    }
    public void Attacking(GameObject player){

        PlayerHealth playerHealth;

        playerHealth = player.GetComponent<PlayerHealth>();

        playerHealth.OnHealthDecrease(Damage);
    }
}
