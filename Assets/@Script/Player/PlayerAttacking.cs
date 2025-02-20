using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{

     [Header("Attack Area")]
    public float coneAngle = 45f; // Lebar sudut cone
    public float attackRange = 5f; // Jarak maksimal serangan

    [Header("Damage Stats")]
    public float Damage;

    public float critDamage;

    public float critChange;

    [Header ("Attack Timing")]
    public float AttackTime;
    private float AttackTimeCurrent;
    public float AttackDelay;
    private float AttackDelayCurrent;

    void Update(){
        AttackCheck();
    }


    void AttackCheck(){
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        foreach (var hit in hitColliders)
        {

              if (hit.gameObject == gameObject) continue;

            Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            if (angle < coneAngle / 2)
            {
                if(hit.gameObject.CompareTag("Enemy")){
                    if(AttackDelayCurrent <= 0){
                        if(AttackTimeCurrent <= 0){
                            Attacking(hit.gameObject);
                            AttackDelayCurrent = AttackDelay;
                            AttackTimeCurrent = AttackTime;
                        } else {
                            AttackTimeCurrent -= Time.deltaTime;
                        }
                    } else {
                        AttackDelayCurrent -= Time.deltaTime;
                    }
                    
                } else {
                    if(AttackDelayCurrent >= 0){
                         AttackDelayCurrent -= Time.deltaTime;
                    }
                   
                }
                
            }
        }
    }

    void Attacking(GameObject Enemy){
        EnemyHP EHP = Enemy.GetComponent<EnemyHP>();
        float newDamage;

        int critCheck = Random.Range(0,100);


        if(critCheck <= critChange){
            newDamage = Damage * critDamage / 100;
            EHP.GotHit(newDamage);
            Debug.Log("Critical");
        } else {
            EHP.GotHit(Damage);
            Debug.Log("NotCritical");
        }


        
    }

#region  Gizmos
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
#endregion
}
